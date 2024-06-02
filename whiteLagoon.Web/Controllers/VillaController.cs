using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Application.Common.Utility;
using WhiteLagoon.Application.Services.Interface;
using WhiteLagoon.Domain.Entities;

namespace whiteLagoon.Web.Controllers
{
    [Authorize]
    // Defining the VillaController class which inherits from the Controller class
    public class VillaController : Controller
    {
        private readonly IVillaService _villaService;
        public VillaController(IVillaService villaService)
        {
            _villaService = villaService;

        }
        // Defining the Index action method
        public IActionResult Index()
        {
            return View(_villaService.GetAllVillas()); // Returning the view with the list of villas
        }

        // Defining the Create action method
        public IActionResult Create()
        {
            return View(); // Returning the view
        }

        // Defining the Create action method that responds to HTTP POST requests
        [HttpPost]
        public IActionResult Create(Villa obj)
        {
            // Checking if the name and description of the villa are the same
            if (obj.Name == obj.Description)
            {
                // Adding an error to the ModelState
                ModelState.AddModelError("description", "the Description cannot be Exactly match the Name.");
            }
            // Checking if the ModelState is valid
            if (ModelState.IsValid)
            {
                _villaService.CreateVilla(obj);
                TempData["Success"] = "Villa Created Successfully";
                return RedirectToAction(nameof(Index)); // Redirecting to the Index action method
            }
            else
            {
                TempData["Error"] = "Error occurred while creating the villa";
                return View(obj); // Returning the view with the villa object
            }
        }

        // Defining the Update action method
        public IActionResult Update(int villaId)
        {
            // Getting the villa with the specified id from the database
            Villa? obj = _villaService.GetVillaById(villaId);
            // Checking if the villa is null
            if (obj is null)
            {
                return RedirectToAction("Error", "Home"); // Returning a NotFound result
            }

            return View(obj); // Returning the view with the villa object
        }
        [HttpPost]
        public IActionResult Update(Villa obj)
        {
            // Checking if the ModelState is valid
            if (ModelState.IsValid && obj.Id > 0)
            {
                _villaService.UpdateVilla(obj);
                TempData["Success"] = "Villa Updated Successfully";
                return RedirectToAction(nameof(Index)); // Redirecting to the Index action method
            }

            TempData["Error"] = "Error occurred while updating the villa";
            return View();
        }
        // Defining the Delete action method
        public IActionResult Delete(int villaId)
        {
            // Getting the villa with the specified id from the database            ``
            Villa? obj = _villaService.GetVillaById(villaId);
            // Checking if the villa is null
            if (obj is null)
            {
                return RedirectToAction("Error", "Home"); // Returning a NotFound result
            }

            return View(obj); // Returning the view with the villa object
        }
        [HttpPost]
        public IActionResult Delete(Villa obj)
        {
            if (_villaService.DeleteVilla(obj.Id))
            {
                TempData["Success"] = "Villa Deleted Successfully";
                return RedirectToAction(nameof(Index)); // Redirecting to the Index action method
            }
            TempData["Error"] = "Error occurred while deleting the villa";
            return View();

            
        }
    }
}