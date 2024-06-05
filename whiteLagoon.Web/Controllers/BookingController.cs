using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocIORenderer;
using Syncfusion.Drawing;
using WhiteLagoon.Application.Common.Utility;
using WhiteLagoon.Application.Services.Interface;
using WhiteLagoon.Domain.Entities;

namespace whiteLagoon.Web.Controllers;

public class BookingController : Controller
{
    private readonly IBookingService _bookingService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IVillaNumberService _villaNumberService;
    private readonly IVillaService _villaService;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public BookingController(
        IBookingService bookingService,
        IWebHostEnvironment webHostEnvironment,
        IVillaService villaService,
        UserManager<ApplicationUser> userManager,
        IVillaNumberService villaNumberService
    )
    {
        _userManager = userManager;
        _webHostEnvironment = webHostEnvironment;
        _bookingService = bookingService;
        _villaNumberService = villaNumberService;
        _villaService = villaService;
        _userManager = userManager;

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
        var user = _userManager.FindByIdAsync(userId).GetAwaiter().GetResult();
        Booking booking = new()
                          {
                              VillaId = villaId,
                              Villa = _villaService.GetVillaById(villaId),
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
        var villa = _villaService.GetVillaById(booking.VillaId);
        booking.TotalCost = villa.Price * booking.Nights;
        booking.Status = SD.StatusPending;
        booking.BookingDate = DateTime.Now;


        if (!_villaService.IsVillaAvailableByDate(villa.Id,booking.Nights,booking.CheckInDate))
        {
            TempData["error"] = "Room has been sold out!";
            return RedirectToAction(nameof(FinalizeBooking), new
                                                             {
                                                                 villaId = booking.VillaId,
                                                                 checkInDate = booking.CheckInDate,
                                                                 nights = booking.Nights
                                                             });
        }



        _bookingService.CreateBooking(booking);


        var domain = Request.Scheme + "://" + Request.Host.Value + "/";
        var options = new SessionCreateOptions
                      {
                          LineItems = new List<SessionLineItemOptions>(),
                          Mode = "payment",
                          SuccessUrl = domain + $"booking/BookingConfirmation?bookingId={booking.Id}",
                          CancelUrl = domain +
                                      $"booking/FinalizeBooking?villaId={booking.VillaId}&checkInDate={booking.CheckInDate}&nights={booking.Nights}"
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
        _bookingService.UpdateStripePaymentID(booking.Id, session.Id, session.PaymentIntentId);

        Response.Headers.Add("Location", session.Url);
        return new StatusCodeResult(303);

        //there is no need to have this line after Adding Stripe Return Line
        //return RedirectToAction(nameof(BookingConfirmation), new { bookingId = booking.Id });
    }

    public IActionResult BookingConfirmation(int bookingId)
    {
        Booking bookingFromDb = _bookingService.GetBookingById(bookingId);
        if (bookingFromDb.Status == SD.StatusPending)
        {
            var service = new SessionService();
            var session = service.Get(bookingFromDb.StripeSessionId);
            if (session.PaymentStatus == "paid")
            {
                _bookingService.UpdateStatus(bookingFromDb.Id, SD.StatusApproved, 0);
                _bookingService.UpdateStripePaymentID(bookingFromDb.Id, session.Id, session.PaymentIntentId);
            }
        }

        return View(bookingId);
    }

    [Authorize]
    public IActionResult BookingDetails(int bookingId)
    {
        var bookingFromDb = _bookingService.GetBookingById(bookingId);
        if (bookingFromDb.VillaNumber == 0 && bookingFromDb.Status == SD.StatusApproved)
        {
            var availableVillaNumber = AssignAvailableVillaNumberByVilla(bookingFromDb.VillaId);
            bookingFromDb.VillaNumbers = _villaNumberService.GetAllVillaNumbers().Where(u => u.VillaId == bookingFromDb.VillaId &&
                                                                             availableVillaNumber.Any(x => x == u.Villa_Number)).ToList();
        }

        return View(bookingFromDb);
    }


    [HttpPost]
    [Authorize]
    public IActionResult GenerateInvoice(int id, string downloadType)
    {
        var basePath = _webHostEnvironment.WebRootPath;
        var document = new WordDocument();
        //Load Template
        var dataPath = basePath + @"/export/BookingDetails.docx";
        using FileStream fileStream = new(dataPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        document.Open(fileStream, FormatType.Automatic);
        //update Template
        var bookingFromDb = _bookingService.GetBookingById(id);
        var textSelection = document.Find("xx_customer_name", false, true);
        var textRange = textSelection.GetAsOneRange();
        textRange.Text = bookingFromDb.Name;

        textSelection = document.Find("XX_BOOKING_NUMBER", false, true);
        textRange = textSelection.GetAsOneRange();
        textRange.Text = bookingFromDb.Id.ToString();

        textSelection = document.Find("XX_BOOKING_DATE", false, true);
        textRange = textSelection.GetAsOneRange();
        textRange.Text = bookingFromDb.BookingDate.ToShortDateString();

        textSelection = document.Find("xx_customer_phone", false, true);
        textRange = textSelection.GetAsOneRange();
        textRange.Text = bookingFromDb.Phone;

        textSelection = document.Find("xx_customer_email", false, true);
        textRange = textSelection.GetAsOneRange();
        textRange.Text = bookingFromDb.Email;

        textSelection = document.Find("xx_payment_date", false, true);
        textRange = textSelection.GetAsOneRange();
        textRange.Text = bookingFromDb.PaymentDate.ToShortDateString();

        textSelection = document.Find("xx_checkin_date", false, true);
        textRange = textSelection.GetAsOneRange();
        textRange.Text = bookingFromDb.CheckInDate.ToShortDateString();

        textSelection = document.Find("xx_checkout_date", false, true);
        textRange = textSelection.GetAsOneRange();
        textRange.Text = bookingFromDb.CheckOutDate.ToShortDateString();

        textSelection = document.Find("xx_booking_total", false, true);
        textRange = textSelection.GetAsOneRange();
        textRange.Text = bookingFromDb.TotalCost.ToString("c");

        WTable table = new(document);
        table.TableFormat.Borders.LineWidth = 1f;
        table.TableFormat.Borders.Color = Color.Black;
        table.TableFormat.Paddings.Top = 7f;
        table.TableFormat.Paddings.Bottom = 7f;
        table.TableFormat.Borders.Horizontal.LineWidth = 1f;

        var rows = bookingFromDb.VillaNumber > 0 ? 3 : 2;
        table.ResetCells(rows, 4);
        var row0 = table.Rows[0];
        row0.Cells[0].AddParagraph().AppendText("Nights");
        row0.Cells[0].Width = 80;
        row0.Cells[1].AddParagraph().AppendText("Villa");
        row0.Cells[1].Width = 200;
        row0.Cells[2].AddParagraph().AppendText("Price/night");
        row0.Cells[3].Width = 80;
        row0.Cells[3].AddParagraph().AppendText("Total");

        var row1 = table.Rows[1];
        row1.Cells[0].AddParagraph().AppendText(bookingFromDb.Nights.ToString());
        row1.Cells[0].Width = 80;
        row1.Cells[1].AddParagraph().AppendText(bookingFromDb.Villa.Name);
        row1.Cells[1].Width = 200;
        row1.Cells[2].AddParagraph().AppendText((bookingFromDb.TotalCost / bookingFromDb.Nights).ToString("c"));
        row1.Cells[3].Width = 80;
        row1.Cells[3].AddParagraph().AppendText(bookingFromDb.TotalCost.ToString("C"));

        if (bookingFromDb.VillaNumber > 0)
        {
            var row2 = table.Rows[2];
            row2.Cells[0].Width = 80;
            row2.Cells[1].AddParagraph().AppendText("Villa Number - " + bookingFromDb.VillaNumber.ToString());
            row2.Cells[1].Width = 200;
            row2.Cells[3].Width = 80;
        }

        var tableStyle = document.AddTableStyle("CustomStyle");
        tableStyle.TableProperties.RowStripe = 1;
        tableStyle.TableProperties.ColumnStripe = 2;
        tableStyle.TableProperties.Paddings.Top = 2;
        tableStyle.TableProperties.Paddings.Bottom = 1;
        tableStyle.TableProperties.Paddings.Left = 5.4f;
        tableStyle.TableProperties.Paddings.Right = 5.4f;
        var firstRowStyle = tableStyle.ConditionalFormattingStyles.Add(ConditionalFormattingType.FirstRow);
        firstRowStyle.CharacterFormat.Bold = true;
        firstRowStyle.CharacterFormat.TextColor = Color.FromArgb(255, 255, 255, 255);
        firstRowStyle.CellProperties.BackColor = Color.Black;

        table.ApplyStyle("CustomStyle");


        TextBodyPart bodyPart = new(document);

        bodyPart.BodyItems.Add(table);
        document.Replace("<ADDTABLEHERE>", bodyPart, false, false);


        using DocIORenderer renderer = new();
        MemoryStream stream = new();
        if (downloadType == "word")
        {
            document.Save(stream, FormatType.Docx);
            stream.Position = 0;
            return File(stream, "application/docx", $"BookingDetails_{bookingFromDb.Id}.docx");
        }

        var pdfDocument = renderer.ConvertToPDF(document);
        pdfDocument.Save(stream);
        stream.Position = 0;
        return File(stream, "application/pdf", $"BookingDetails_{bookingFromDb.Id}.pdf");
    }

    [HttpPost]
    [Authorize(Roles = SD.Role_Admin)]
    public IActionResult CheckIn(Booking booking)
    {
        _bookingService.UpdateStatus(booking.Id, SD.StatusCheckedIn, booking.VillaNumber);
        TempData["Success"] = "Booking Checked In Successfully";
        return RedirectToAction(nameof(BookingDetails), new { bookingId = booking.Id });
    }

    [HttpPost]
    [Authorize(Roles = SD.Role_Admin)]
    public IActionResult CheckOut(Booking booking)
    {
        _bookingService.UpdateStatus(booking.Id, SD.StatusCompleted, booking.VillaNumber);
        TempData["Success"] = "Booking Checked Out Successfully";
        return RedirectToAction(nameof(BookingDetails), new { bookingId = booking.Id });
    }

    [HttpPost]
    [Authorize(Roles = SD.Role_Admin)]
    public IActionResult CancelBooking(Booking booking)
    {
        _bookingService.UpdateStatus(booking.Id, SD.StatusCancelled, booking.VillaNumber);
        TempData["Success"] = "Booking Cancelled Successfully";
        return RedirectToAction(nameof(BookingDetails), new { bookingId = booking.Id });
    }


    #region API Calls

    [HttpGet]
    [Authorize]
    public IActionResult GetAll(string status)
    {
        IEnumerable<Booking> objBookings;
        string userId = "";
        if (string.IsNullOrEmpty(status))
        {
            status = "";
        }
        if (!User.IsInRole(SD.Role_Admin))
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity!;
            //var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
        objBookings = _bookingService.getAllBookings(userId,status);

        return Json(new { data = objBookings });
    }

    #endregion

    private List<int> AssignAvailableVillaNumberByVilla(int villaId)
    {
        List<int> availableVillaNumbers = new();
        var villaNumbers = _villaNumberService.GetAllVillaNumbers().Where(u => u.VillaId == villaId);

        var checkedInVilla = _bookingService.GetCheckedinVillaNumbers(villaId);
        
        foreach (var villaNumber in villaNumbers)
            if (!checkedInVilla.Contains(villaNumber.Villa_Number))
                availableVillaNumbers.Add(villaNumber.Villa_Number);

        return availableVillaNumbers;
    }
}