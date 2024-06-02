using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Application.Services.Interface;

public interface IBookingService
{
    void CreateBooking(Booking booking);
    Booking GetBookingById(int bookingId);
    IEnumerable<Booking> getAllBookings(string userId = "", string? statusFilterList = "");

}