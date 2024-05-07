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

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult GetVillasByDate()
        {
            return View();
        }
    }
}
