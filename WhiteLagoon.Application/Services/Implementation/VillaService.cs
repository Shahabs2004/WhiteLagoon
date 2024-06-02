using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Application.Common.Utility;
using WhiteLagoon.Application.Services.Interface;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Application.Services.Implementation
{
    public class VillaService : IVillaService
    {
        // Declaring a private readonly field of type ApplicationDbContext
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        // Defining the constructor for the VillaController class
        public VillaService(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IEnumerable<Villa> GetAllVillas()
        {
            return _unitOfWork.Villa.GetAll(includeProperties:"VillaAmenity");
        }

        public Villa GetVillaById(int id)
        {
            return (_unitOfWork.Villa.Get(u => u.Id == id,includeProperties:"VillaAmenity"));
        }

        public void CreateVilla(Villa villa)
        {
            if (villa.Image != null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(villa.Image.FileName);
                string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"img\VillaImage");
                using (var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create))
                {
                    villa.Image.CopyTo(fileStream);
                    villa.ImageUrl = @"\img\VillaImage\" + fileName;
                }
            }
            else // If the image is null
            {
                villa.ImageUrl = "https://placehold.co/600*400"; // Setting the image to noimage.png
            }

            _unitOfWork.Villa.Add(villa); // Adding the villa to the database
            _unitOfWork.Save();           // Saving the changes to the database
        }

        public void UpdateVilla(Villa villa)
        {
            if (villa.Image != null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(villa.Image.FileName);
                string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"img\VillaImage");
                if (!string.IsNullOrEmpty((villa.ImageUrl)))
                {
                    var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, villa.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }
                using (var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create))
                {
                    villa.Image.CopyTo(fileStream);
                    villa.ImageUrl = @"\img\VillaImage\" + fileName;
                }
            }

            _unitOfWork.Villa.Update(villa); // Adding the villa to the database
            _unitOfWork.Villa.Save();      // Saving the changes to the database
        }

        public bool DeleteVilla(int id)
        {
            try
            {
                Villa? objFromDb = _unitOfWork.Villa.Get((u => u.Id == id));
                // Checking if the ModelState is valid
                if (objFromDb is not null)
                {
                    if (!string.IsNullOrEmpty((objFromDb.ImageUrl)))
                    {
                        var oldImagePath =
                            Path.Combine(_webHostEnvironment.WebRootPath, objFromDb.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    _unitOfWork.Villa.Remove(objFromDb); // Remove the villa from the database
                    _unitOfWork.Villa.Save();            // Saving the changes to the database
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public IEnumerable<Villa> GetVillasAvailableByDate(int nights, DateOnly checkInDate)
        {
            var villaList = _unitOfWork.Villa.GetAll(includeProperties: "VillaAmenity").ToList();
            var villaNumberList = _unitOfWork.VillaNumber.GetAll().ToList();
            var bookedVillas = _unitOfWork.Booking.GetAll(u => u.Status == SD.StatusApproved || u.Status == SD.StatusCheckedIn).ToList();


            foreach (var villa in villaList)
            {
                int roomAvailable =
                    SD.VillaRoomsAvailable_Count(villa.Id, villaNumberList, checkInDate, nights, bookedVillas);
                villa.IsAvailable = roomAvailable > 0 ? true : false;
            }

            return villaList;
        }
    }
}
