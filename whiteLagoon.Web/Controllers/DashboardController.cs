using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Application.Common.Utility;
using WhiteLagoon.Infrastructure.Repository;
using whiteLagoon.Web.ViewModels;

namespace whiteLagoon.Web.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        static int previousMonth = DateTime.Now.Month == 1 ? 12 : DateTime.Now.Month - 1;
        readonly DateTime previousMonthStartDate = new(DateTime.Now.Year, previousMonth - 1, 1);
        readonly DateTime currentMonthStartDate = new(DateTime.Now.Year, DateTime.Now.Month, 1);
        public DashboardController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()

        {
            return View();
        }
        [Authorize(Roles = SD.Role_Admin)]
        public async Task<IActionResult> GetTotalBookingRadialChartData()
        {
            var totalBookings =  _unitOfWork.Booking.GetAll(u => u.Status != SD.StatusPending || u.Status == SD.StatusCancelled);
            var countByCurrentMonth = totalBookings.Count(u => u.BookingDate >= currentMonthStartDate && u.BookingDate <= DateTime.Now);
            var countByPreviousMonth = totalBookings.Count(u => u.BookingDate >= previousMonthStartDate && u.BookingDate <= currentMonthStartDate);
            RadialBarChartVM radialBarChartVm = new();
            int increaseDecreaseRatio = 100;
            if (countByPreviousMonth != 0)
            {
                increaseDecreaseRatio =
                    Convert.ToInt32((countByCurrentMonth - countByPreviousMonth) / countByPreviousMonth * 100);
            }

            radialBarChartVm.TotalCount = totalBookings.Count();
            radialBarChartVm.CountInCurrentMonth = countByCurrentMonth;
            radialBarChartVm.HasRatioIncreased = currentMonthStartDate > previousMonthStartDate;
            radialBarChartVm.Series = new int[] { increaseDecreaseRatio };
            return Json(radialBarChartVm);
        }
    }
}
