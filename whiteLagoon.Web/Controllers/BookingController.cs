using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Application.Common.Utility;
using WhiteLagoon.Domain.Entities;

namespace whiteLagoon.Web.Controllers;

public class BookingController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public BookingController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    [Authorize]
    public IActionResult Index()
    {
        return View();
    }

    [Authorize]
    public IActionResult FinalizeBooking(int villaId, DateOnly checkInDate, int nights)
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        var user = _unitOfWork.User.Get(u => u.Id == userId);
        Booking booking = new()
                          {
                              VillaId = villaId,
                              Villa = _unitOfWork.Villa.Get(u => u.Id == villaId, "VillaAmenity"),
                              CheckInDate = checkInDate,
                              Nights = nights,
                              CheckOutDate = checkInDate.AddDays(nights),
                              UserId = userId,
                              Phone = user.PhoneNumber,
                              Name = user.Name
                          };
        booking.TotalCost = booking.Villa.Price * booking.Nights;
        return View(booking);
    }

    [Authorize]
    [HttpPost]
    public IActionResult FinalizeBooking(Booking booking)
    {
        var villa = _unitOfWork.Villa.Get(u => u.Id == booking.VillaId);
        booking.TotalCost = villa.Price * booking.Nights;
        booking.Status = SD.StatusPending;
        booking.BookingDate = DateTime.Now;
        _unitOfWork.Booking.Add(booking);
        _unitOfWork.Save();


        var domain = Request.Scheme + "://" + Request.Host.Value + "/";
        var options = new SessionCreateOptions
                      {
                          LineItems = new List<SessionLineItemOptions>(),
                          Mode = "payment",
                          SuccessUrl = domain + $"booking/BookingConfirmation?bookingId={booking.Id}",
                          CancelUrl = domain + $"booking/FinalizeBooking?villaId={booking.VillaId}&checkInDate={booking.CheckInDate}&nights={booking.Nights}"
                      };
        options.LineItems.Add(new SessionLineItemOptions
                              {
                                  PriceData = new SessionLineItemPriceDataOptions
                                              {
                                                  UnitAmount = (long)booking.TotalCost * 100,
                                                  Currency = "usd",
                                                  ProductData = new SessionLineItemPriceDataProductDataOptions
                                                                {
                                                                    Name = villa.Name,
                                                                    Description = villa.Description
                                                                    //Images = new List<string> { domain + villa.ImageUrl }//is in the Local Host
                                                                }
                                              },
                                  Quantity = 1
                              });

        var service = new SessionService();
        var session = service.Create(options);
        _unitOfWork.Booking.UpdateStripePaymentID(booking.Id,session.Id,session.PaymentIntentId);
        _unitOfWork.Save();

        Response.Headers.Add("Location", session.Url);
        return new StatusCodeResult(303);

         //there is no need to have this line after Adding Stripe Return Line
        //return RedirectToAction(nameof(BookingConfirmation), new { bookingId = booking.Id });
    }

    public IActionResult BookingConfirmation(int bookingId)
    {
        Booking bookingFromDb = _unitOfWork.Booking.Get(u => u.Id == bookingId, includeProperties: "Users,Villa");
        if (bookingFromDb.Status ==SD.StatusPending)
        {
            var service = new SessionService();
            Session session = service.Get(bookingFromDb.StripeSessionId);
            if (session.PaymentStatus == "paid")
            {
                _unitOfWork.Booking.UpdateStatus(bookingFromDb.Id, SD.StatusApproved,0);
                _unitOfWork.Booking.UpdateStripePaymentID(bookingFromDb.Id,session.Id,session.PaymentIntentId);
                _unitOfWork.Save();
            }
        }

        return View(bookingId);
    }

    [Authorize]
    public IActionResult BookingDetails(int bookingId)
    {
        Booking bookingFromDb = _unitOfWork.Booking.Get(u => u.Id == bookingId, includeProperties: "Users,Villa");
        if (bookingFromDb.VillaNumber ==0 && bookingFromDb.Status == SD.StatusApproved)
        {
            
        }
        return View(bookingFromDb);
    }

    #region API Calls
    [HttpGet]
    [Authorize]
    public IActionResult GetAll(String status)
    {
        IEnumerable<Booking> objBookings;
        if (User.IsInRole(SD.Role_Admin))
        {
            objBookings = _unitOfWork.Booking.GetAll(includeProperties: "Users,Villa");
        }
        else
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity!;
            //var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            
            objBookings = _unitOfWork.Booking.GetAll(u => u.UserId == userId, includeProperties: "User,Villa");
            
        }

        if (TryValidateModel(!string.IsNullOrEmpty(status)))
        {
            objBookings = objBookings.Where(u => u.Status.ToLower().Equals(status.ToLower()));
        }
        return Json(new { data = objBookings });
    }
    #endregion

    private List<int> AssignAvailableVillaNumberByVilla(int villaId, DateOnly checkInDate, int nights)
    {
        List<int> availableVillaNumbers = new();
        var villaNumbers = _unitOfWork.VillaNumber.GetAll(u => u.VillaId == villaId);
        var checkedInVilla = _unitOfWork.Booking.GetAll(u => u.VillaId == villaId && u.Status == SD.StatusCheckedIn)
            .Select(u => u.VillaNumber);
        foreach (var villaNumber in villaNumbers)
        {
            if (!checkedInVilla.Contains(villaNumber.Villa_Number))
            {
                availableVillaNumbers.Add(villaNumber.Villa_Number);
            }
        }

        return availableVillaNumbers;

    }

}