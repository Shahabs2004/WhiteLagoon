using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Inrfastructure.Data;

namespace whiteLagoon.Web.Controllers
{
    // Defining the VillaController class which inherits from the Controller class
    public class VillaNumberController : Controller
    {
        // Declaring a private readonly field of type ApplicationDbContext
        private readonly ApplicationDbContext _db;

        // Defining the constructor for the VillaController class
        public VillaNumberController(ApplicationDbContext db)
        {
            _db = db; // Assigning the passed in db value to the _db field
        }

        // Defining the Index action method
        public IActionResult Index()
        {
            var villasNumbers = _db.VillaNumbers.ToList(); // Getting all villas from the database and converting to a list
            return View(villasNumbers); // Returning the view with the list of villas
        }

        // Defining the Create action method
        public IActionResult Create()
        {
            IEnumerable<SelectListItem> list = _db.Villas.ToList().Select(u=> new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }
                );
            ViewBag.VillaList = list;
            return View(); // Returning the view
        }

        // Defining the Create action method that responds to HTTP POST requests
        [HttpPost]
        public IActionResult Create(VillaNumber obj)
        {
            //ModelState.Remove("Villa"); Commented out Due to Added Annotation to Property
            // Checking if the ModelState is valid
            if (ModelState.IsValid)
            {
                _db.VillaNumbers.Add(obj); // Adding the villa to the database
                _db.SaveChanges(); // Saving the changes to the database
                TempData["Success"] = "Villa number Created Successfully";
                return RedirectToAction("Index"); // Redirecting to the Index action method
            }
            else
            {
                TempData["Error"] = "Error occurred while creating the villa number ";
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