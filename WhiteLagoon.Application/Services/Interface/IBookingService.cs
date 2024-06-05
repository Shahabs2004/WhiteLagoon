using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Application.Services.Interface;

public interface IBookingService
{
    void CreateBooking(Booking booking);
    Booking GetBookingById(int bookingId);
    IEnumerable<Booking> getAllBookings(string userId = "", string? statusFilterList = "");

    void UpdateStatus(int bookingId, string orderStatus, int villaNumber);
    void UpdateStripePaymentID(int id, string sessionId, string paymentintentId);


    public IEnumerable<int> GetCheckedinVillaNumbers(int villaId);

}