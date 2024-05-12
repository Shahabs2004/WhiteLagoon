using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Application.Common.Utility;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Infrastructure.Repository;

public class BookingRepository : Repository<Booking>, IBookingRepository

{
    private readonly ApplicationDbContext _db;

    public BookingRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    public void Update(Booking entity)
    {
        _db.Bookings.Update(entity);
    }

    public void UpdateStatus(int bookingId, string bookingStatus)
    {
        var bookingFromDb = _db.Bookings.FirstOrDefault(u => u.Id == bookingId);
        if (bookingFromDb is not null)
        {
            bookingFromDb.Status = bookingStatus;
            if (bookingStatus == SD.StatusCheckedIn) bookingFromDb.ActualCheckInDate = DateTime.Now;

            if (bookingStatus == SD.StatusCompleted) bookingFromDb.ActualCheckOutDate = DateTime.Now;
        }
    }

    public void UpdayeStripePaymentID(int bookingId, string sessionId, string paymentIntentId)
    {
        var bookingFromDb = _db.Bookings.FirstOrDefault(u => u.Id == bookingId);
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
    }
}