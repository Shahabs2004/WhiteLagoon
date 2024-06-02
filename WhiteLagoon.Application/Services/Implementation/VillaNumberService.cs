using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Application.Services.Interface;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Application.Services.Implementation
{
    public class VillaNumberService : IVillaNumberService
    {
        // Declaring a private readonly field of type ApplicationDbContext
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        // Defining the constructor for the VillaController class
        public VillaNumberService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<VillaNumber> GetAllVillaNumbers()
        {
            return _unitOfWork.VillaNumber.GetAll(includeProperties: "Villa");
        }

        public VillaNumber GetVillaNumberById(int id)
        {
            return (_unitOfWork.VillaNumber.Get(u => u.Villa_Number == id, includeProperties: "Villa"));
        }

        public void CreateVillaNumber(VillaNumber villaNumber)
        {
            _unitOfWork.VillaNumber.Add(villaNumber); // Adding the villa to the database
            _unitOfWork.Save();           // Saving the changes to the database
        }

        public void UpdateVillaNumber(VillaNumber villanumber)
        {
            _unitOfWork.VillaNumber.Update(villanumber); // Adding the villa to the database
            _unitOfWork.Save();      // Saving the changes to the database
        }

        public bool DeleteVillaNumber(int id)
        {
            try
            {
                VillaNumber? objFromDb = _unitOfWork.VillaNumber.Get((u => u.Villa_Number == id));
                // Checking if the ModelState is valid
                if (objFromDb is not null)
                {
                    _unitOfWork.VillaNumber.Remove(objFromDb); // Remove the villa from the database
                    _unitOfWork.Save();            // Saving the changes to the database
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public bool CheckVillaNumberExists(int villa_Number)
        {
            return _unitOfWork.VillaNumber.Any(u => u.Villa_Number == villa_Number);
        }
    }
}
