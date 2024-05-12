using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WhiteLagoon.Application.Common.Interfaces;
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

        // ???? ????? ?÷? ??? ???? ??????.
        /*[HttpPost]
        public IActionResult Index(HomeVM homeVM)
        {
            homeVM.villaList = _unitOfWork.Villa.GetAll(includeProperties: "VillaAmenity");

            return View(homeVM);
        }*/

        [HttpPost]
        public IActionResult GetVillasByDate(int nights, DateOnly checkInDate)
        {
            var villalist = _unitOfWork.Villa.GetAll(includeProperties: "VillaAmenity").ToList();
            foreach (var villa in villalist)
            {
                if (villa.Id % 2 == 0)
                {
                    villa.IsAvailable = false;

                }
            }
            HomeVM homeVM = new()
            {
                villaList = villalist,
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
