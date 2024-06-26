﻿using whiteLagoon.Web.ViewModels;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Application.Common.Utility;

//SD Stands for Static Details
public static class SD
{
    public const string Role_Customer = "Customer";
    public const string Role_Admin = "Admin";

    public const string StatusPending = "Pending";
    public const string StatusApproved = "Approved";
    public const string StatusCheckedIn = "CheckedIn";
    public const string StatusCompleted = "Completed";
    public const string StatusCancelled = "Cancelled";
    public const string StatusRefunded = "Refunded";


    //Helper Methods goes Here
    public static int VillaRoomsAvailable_Count(int villaId,
                                                List<VillaNumber> villaNumberlist,
                                                DateOnly checkInDate,
                                                int nights,
                                                List<Booking> bookings)
    {
        List<int> bookingIndate = new();
        var roomsInVilla = villaNumberlist.Where(x => x.VillaId == villaId).Count();
        var finalAvailableRoomForAllnights = int.MaxValue;
        for (var i = 0; i < nights; i++)
        {
            var villasBooked = bookings.Where(u => u.CheckInDate <= checkInDate.AddDays(i)
                                                && u.CheckOutDate > checkInDate.AddDays(i) && u.VillaId == villaId);
            foreach (var booking in villasBooked)
                if (!bookingIndate.Contains(booking.Id))
                    bookingIndate.Add(booking.Id);
            var totalAvailableRooms = roomsInVilla - bookingIndate.Count;
            if (totalAvailableRooms == 0)
            {
                return 0;
            }

            if (finalAvailableRoomForAllnights > totalAvailableRooms)
                finalAvailableRoomForAllnights = totalAvailableRooms;
        }

        return finalAvailableRoomForAllnights;
    }

    public static RadialBarChartDto GetRadialChartDataModel(int totalCount, double currentMonthCount, double prevMonthCount)
    {
        RadialBarChartDto radialBarChartDto = new();
        int increaseDecreaseRatio = 100;
        if (prevMonthCount != 0)
        {
            increaseDecreaseRatio =
                Convert.ToInt32((currentMonthCount - prevMonthCount) / prevMonthCount * 100);
        }

        radialBarChartDto.TotalCount = totalCount;
        radialBarChartDto.CountInCurrentMonth = Convert.ToInt32(currentMonthCount);
        radialBarChartDto.HasRatioIncreased = currentMonthCount > prevMonthCount;
        radialBarChartDto.Series = new int[] { increaseDecreaseRatio };
        return radialBarChartDto;
    }

}