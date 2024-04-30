using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Inrfastructure.Data;
using whiteLagoon.Web.ViewModels;

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
            var villasNumbers = _db.VillaNumbers.Include(u=>u.Villa).ToList(); // Getting all villas from the database and converting to a list
            return View(villasNumbers); // Returning the view with the list of villas
        }

        // Defining the Create action method
        public IActionResult Create()
        {
            VillaNumberVM villaNumberVM = new()
            {
                VillaList =  _db.Villas.ToList().Select(u=> new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                })
            };
            return View(villaNumberVM); // Returning the view
        }

        // Defining the Create action method that responds to HTTP POST requests
        [HttpPost]
        public IActionResult Create(VillaNumberVM obj)
        {
            // Checking if the ModelState is valid
            bool roomNumberExists =_db.VillaNumbers.Any(u => u.Villa_Number == obj.VillaNumber.Villa_Number);
            if (ModelState.IsValid && !roomNumberExists)
            {
                _db.VillaNumbers.Add(obj.VillaNumber); // Adding the villa to the database
                _db.SaveChanges(); // Saving the changes to the database
                TempData["Success"] = "Villa number Created Successfully";
                return RedirectToAction(nameof(Index)); // Redirecting to the Index action method
            }
            if(roomNumberExists)
            {
                TempData["Error"] = "Duplicate Room Number Detected.Please Select another room Number";
            }

            obj.VillaList = _db.Villas.ToList().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            return View(obj); // Returning the view with the villa object
        }

         //Defining the Update action method
        public IActionResult Update(int villaNumberId)
        {
            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _db.Villas.ToList().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                VillaNumber = _db.VillaNumbers.FirstOrDefault(u=>u.Villa_Number == villaNumberId)
            };
            // Checking if the villa is null
            if (villaNumberVM.VillaNumber is null)
            {
                return RedirectToAction("Error", "Home"); // Returning a NotFound result
            }

            return View(villaNumberVM); // Returning the view with the villa object
        }
        [HttpPost]
        public IActionResult Update(VillaNumberVM villaNumberVM)
        {
            // Checking if the ModelState is valid
            
            if (ModelState.IsValid)
            {
                _db.VillaNumbers.Update(villaNumberVM.VillaNumber); // Adding the villa to the database
                _db.SaveChanges(); // Saving the changes to the database
                TempData["Success"] = "Villa number Updated Successfully";
                return RedirectToAction(nameof(Index)); // Redirecting to the Index action method
            }


            villaNumberVM.VillaList = _db.Villas.ToList().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            return View(villaNumberVM); // Returning the view with the villa object
        }

        // Defining the Delete action method
        public IActionResult Delete(int villaNumberId)
        {
            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _db.Villas.ToList().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                VillaNumber = _db.VillaNumbers.FirstOrDefault(u => u.Villa_Number == villaNumberId)
            };
            // Checking if the villa is null
            if (villaNumberVM.VillaNumber is null)
            {
                return RedirectToAction("Error", "Home"); // Returning a NotFound result
            }

            return View(villaNumberVM); // Returning the view with the villa object
        }

        [HttpPost]
        public IActionResult Delete(VillaNumberVM villaNumberVM)
        {
            VillaNumber? objFromDb = _db.VillaNumbers
                .FirstOrDefault(u => u.Villa_Number == villaNumberVM.VillaNumber.Villa_Number);
            // Checking if the ModelState is valid
            if (objFromDb is not null)
            {
                _db.VillaNumbers.Remove(objFromDb); // Remove the villa from the database
                _db.SaveChanges(); // Saving the changes to the database
                TempData["Success"] = "Villa Number Deleted Successfully";
                return RedirectToAction(nameof(Index)); // Redirecting to the Index action method
            }

            TempData["Error"] = "Error occurred while deleting the villa Number";
            return View();
        } 
    }
}