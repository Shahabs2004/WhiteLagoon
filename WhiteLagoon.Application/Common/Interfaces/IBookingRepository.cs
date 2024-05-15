using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Application.Common.Interfaces;

public interface IBookingRepository : IRepository<Booking>
{
    void Update(Booking entity);
    void UpdateStatus(int bookingId, string orderStatus,int villaNumber);
    void UpdateStripePaymentID(int id, string sessionId, string paymentintentId);
}