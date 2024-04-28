using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Inrfastructure.Data;

namespace whiteLagoon.Web.Controllers
{
    // Defining the VillaController class which inherits from the Controller class
    public class VillaController : Controller
    {
        // Declaring a private readonly field of type ApplicationDbContext
        private readonly ApplicationDbContext _db;

        // Defining the constructor for the VillaController class
        public VillaController(ApplicationDbContext db)
        {
            _db = db; // Assigning the passed in db value to the _db field
        }

        // Defining the Index action method
        public IActionResult Index()
        {
            var villas = _db.Villas.ToList(); // Getting all villas from the database and converting to a list
            return View(villas); // Returning the view with the list of villas
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
                _db.Villas.Add(obj); // Adding the villa to the database
                _db.SaveChanges(); // Saving the changes to the database
                TempData["Success"] = "Villa Created Successfully";
                return RedirectToAction("Index"); // Redirecting to the Index action method
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
            Villa? obj = _db.Villas.FirstOrDefault((u => u.Id == villaId));
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
                _db.Villas.Update(obj); // Adding the villa to the database
                _db.SaveChanges(); // Saving the changes to the database
                TempData["Success"] = "Villa Updated Successfully";
                return RedirectToAction("Index"); // Redirecting to the Index action method
            }

            TempData["Error"] = "Error occurred while updating the villa";
            return View();
        }

        // Defining the Delete action method
        public IActionResult Delete(int villaId)
        {
            // Getting the villa with the specified id from the database
            Villa? obj = _db.Villas.FirstOrDefault((u => u.Id == villaId));
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
            Villa? objFromDb = _db.Villas.FirstOrDefault((u => u.Id == obj.Id));
            // Checking if the ModelState is valid
            if (objFromDb is not null)
            {
                _db.Villas.Remove(objFromDb); // Remove the villa from the database
                _db.SaveChanges(); // Saving the changes to the database
                TempData["Success"] = "Villa Deleted Successfully";
                return RedirectToAction("Index"); // Redirecting to the Index action method
            }

            TempData["Error"] = "Error occurred while deleting the villa";
            return View();
        }
    }
}