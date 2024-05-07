using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Application.Common.Utility;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Inrfastructure.Data;

namespace whiteLagoon.Web.Controllers
{
    [Authorize]
    // Defining the VillaController class which inherits from the Controller class
    public class VillaController : Controller
    {
        // Declaring a private readonly field of type ApplicationDbContext
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        // Defining the constructor for the VillaController class
        public VillaController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        // Defining the Index action method
        public IActionResult Index()
        {
            var villas = _unitOfWork.Villa.GetAll(); // Getting all villas from the database and converting to a list
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
                if (obj.Image != null)
                {
                    string fileName = Guid.NewGuid().ToString()+Path.GetExtension(obj.Image.FileName);
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"img\VillaImage");
                    using (var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create))
                    {
                        obj.Image.CopyTo(fileStream);
                        obj.ImageUrl = @"\img\VillaImage\"+fileName;
                    }
                }
                else // If the image is null
                {
                    obj.ImageUrl = "https://placehold.co/600*400"; // Setting the image to noimage.png
                }
                _unitOfWork.Villa.Add(obj); // Adding the villa to the database
                _unitOfWork.Save(); // Saving the changes to the database
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
            Villa? obj = _unitOfWork.Villa.Get(u => u.Id == villaId);
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
                if (obj.Image != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(obj.Image.FileName);
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"img\VillaImage");
                    if (!string.IsNullOrEmpty((obj.ImageUrl)))
                    {
                       var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create))
                    {
                        obj.Image.CopyTo(fileStream);
                        obj.ImageUrl = @"\img\VillaImage\" + fileName;
                    }
                }

                _unitOfWork.Villa.Update(obj); // Adding the villa to the database
                _unitOfWork.Villa.Save(); // Saving the changes to the database
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
            Villa? obj = _unitOfWork.Villa.Get((u => u.Id == villaId));
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
            Villa? objFromDb = _unitOfWork.Villa.Get((u => u.Id == obj.Id));
            // Checking if the ModelState is valid
            if (objFromDb is not null)
            {
                if (!string.IsNullOrEmpty((objFromDb.ImageUrl)))
                {
                    var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, objFromDb.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }
                _unitOfWork.Villa.Remove(objFromDb); // Remove the villa from the database
                _unitOfWork.Villa.Save(); // Saving the changes to the database
                TempData["Success"] = "Villa Deleted Successfully";
                return RedirectToAction(nameof(Index)); // Redirecting to the Index action method
            }

            TempData["Error"] = "Error occurred while deleting the villa";
            return View();
        }
    }
}