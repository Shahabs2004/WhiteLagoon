using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Application.Common.Utility;
using whiteLagoon.Web.Models;
using whiteLagoon.Web.ViewModels;

namespace whiteLagoon.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            HomeVM homeVM = new()
            {
                villaList = _unitOfWork.Villa.GetAll(includeProperties: "VillaAmenity"),
                Nights =  1,
                CheckInDate = DateOnly.FromDateTime(DateTime.Now),
                //CheckOutDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1))
            };
            return View(homeVM);
        }

        // فونت مهم گچپژ
        /*[HttpPost]
        public IActionResult Index(HomeVM homeVM)
        {
            homeVM.villaList = _unitOfWork.Villa.GetAll(includeProperties: "VillaAmenity");

            return View(homeVM);
        }*/

        [HttpPost]
        public IActionResult GetVillasByDate(int nights, DateOnly checkInDate)
        {
            var villaList = _unitOfWork.Villa.GetAll(includeProperties: "VillaAmenity").ToList();
            var villaNumberList = _unitOfWork.VillaNumber.GetAll().ToList();
            var bookedVillas = _unitOfWork.Booking.GetAll(u => u.Status == SD.StatusApproved || u.Status==SD.StatusCheckedIn).ToList();

            
            foreach (var villa in villaList)
            {
                int roomAvailable =
                    SD.VillaRoomsAvailable_Count(villa.Id, villaNumberList, checkInDate, nights, bookedVillas);
                villa.IsAvailable = roomAvailable > 0 ?true : false;
            }
            HomeVM homeVM = new()
            {
                villaList = villaList,
                Nights = nights,
                CheckInDate = checkInDate
            };
            return PartialView("_VillaList", homeVM);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

    }
}
