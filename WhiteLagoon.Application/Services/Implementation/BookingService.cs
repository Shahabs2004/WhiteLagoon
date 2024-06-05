using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Application.Common.Utility;
using WhiteLagoon.Application.Services.Interface;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Application.Services.Implementation
{
    public  class BookingService:IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }
        public void CreateBooking(Booking booking)
        {
            _unitOfWork.Booking.Add(booking);
            _unitOfWork.Save();
        }

        public Booking GetBookingById(int bookingId)
        {
            return _unitOfWork.Booking.Get(u=>u.Id==bookingId,includeProperties: "User,Villa");
        }

        public IEnumerable<Booking> getAllBookings(string userId = "", string? statusFilterList = "")
        {
            IEnumerable<string> statusList = statusFilterList.ToLower().Split(",");
            if (Equals(!string.IsNullOrEmpty(statusFilterList)&& !string.IsNullOrEmpty(userId)))
            {
                return _unitOfWork.Booking.GetAll(u => statusList.Contains(u.Status.ToLower()) && u.UserId == userId,
                                                  includeProperties: "User,Villa");
            }
            else
            {
                if (Equals(!string.IsNullOrEmpty(statusFilterList)))
                {
                    return _unitOfWork.Booking.GetAll(u => statusList.Contains(u.Status.ToLower()) , includeProperties: "User,Villa");
                }

                if (Equals(!string.IsNullOrEmpty(userId)))

                {
                    return _unitOfWork.Booking.GetAll(u=> u.UserId == userId, includeProperties: "User,Villa");
                }
            }
            return _unitOfWork.Booking.GetAll(includeProperties: "User,Villa");
        }

        public void UpdateStatus(int bookingId, string bookingStatus, int villaNumber = 0)
        {
            var bookingFromDb =_unitOfWork.Booking.Get(u => u.Id == bookingId,tracked:true);
            if (bookingFromDb is not null)
            {
                bookingFromDb.Status = bookingStatus;
                if (bookingStatus == SD.StatusCheckedIn)
                {
                    bookingFromDb.VillaNumber = villaNumber;
                    bookingFromDb.ActualCheckInDate = DateTime.Now;
                }

                if (bookingStatus == SD.StatusCompleted) bookingFromDb.ActualCheckOutDate = DateTime.Now;
            }
            _unitOfWork.Save();
        }

        public void UpdateStripePaymentID(int bookingId, string sessionId, string paymentIntentId)
        {
            var bookingFromDb = _unitOfWork.Booking.Get(u => u.Id == bookingId,tracked:true);
            if (bookingFromDb is not null)
            {
                if (!string.IsNullOrEmpty(sessionId)) bookingFromDb.StripeSessionId = sessionId;
                if (!string.IsNullOrEmpty(paymentIntentId))
                {
                    bookingFromDb.StripePaymentIntentId = paymentIntentId;
                    bookingFromDb.PaymentDate = DateTime.Now;
                    bookingFromDb.IsPaymentSuccessful = true;
                }
            }
            _unitOfWork.Save();
        }

        public IEnumerable<int> GetCheckedinVillaNumbers(int villaId)
        {

            return _unitOfWork.Booking.GetAll(u => u.VillaId == villaId && u.Status == SD.StatusCheckedIn)
                .Select(u => u.VillaNumber);

        }
    }
}
