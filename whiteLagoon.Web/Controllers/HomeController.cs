using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Syncfusion.DocIO.DLS;
using Syncfusion.Presentation;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Application.Common.Utility;
using whiteLagoon.Web.Models;
using whiteLagoon.Web.ViewModels;
using ListType = Syncfusion.DocIO.DLS.ListType;

namespace whiteLagoon.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(IUnitOfWork unitOfWork,IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;

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

        [HttpPost]
        public IActionResult GeneratePPTExport(int Id)
        {
            var villa = _unitOfWork.Villa.GetAll(includeProperties: "VillaAmenity").FirstOrDefault(x => x.Id == Id);
            if (villa == null)
            {
                return RedirectToAction(nameof(Error));
            }

            string basePath = _webHostEnvironment.WebRootPath;
            string filePath = basePath + @"/Export/ExportVillaDetails.pptx";



            using IPresentation presentation = Presentation.Open(filePath);

            ISlide slide = presentation.Slides[0];

            IShape? shape = slide.Shapes.FirstOrDefault(u => u.ShapeName == "txtVillaName") as IShape;

            if (shape is not null)
            {
                shape.TextBody.Text = villa.Name;
            }

            shape = slide.Shapes.FirstOrDefault(u => u.ShapeName == "txtVillaDescription") as IShape;
            if (shape is not null)
            {
                shape.TextBody.Text = villa.Description;
            }
            shape = slide.Shapes.FirstOrDefault(u => u.ShapeName == "txtOccupancy") as IShape;
            if (shape is not null)
            {
                shape.TextBody.Text = $"Max Occupancy : {villa.Occupancy} adults";
            }
            shape = slide.Shapes.FirstOrDefault(u => u.ShapeName == "txtVillaSize") as IShape;
            if (shape is not null)
            {
                shape.TextBody.Text = $"Villa Size: {villa.Sqft} sqft";
            }
            shape = slide.Shapes.FirstOrDefault(u => u.ShapeName == "txtPricePerNight") as IShape;
            if (shape is not null)
            {
                shape.TextBody.Text = $"USD {villa.Price:c}/night";
            }

            shape = slide.Shapes.FirstOrDefault(u => u.ShapeName == "txtVillaAmenitiesHeading") as IShape;
            if (shape is not null)
            {
                List<string> listItems = villa.VillaAmenity.Select(u => u.Name).ToList();
                shape.TextBody.Text = "Villa Amenities";
                foreach (var item in listItems)
                {
                    IParagraph paragraph = shape.TextBody.AddParagraph();
                    ITextPart textPart = paragraph.AddTextPart(item);
                    paragraph.ListFormat.Type = (Syncfusion.Presentation.ListType)ListType.Bulleted;
                    paragraph.ListFormat.BulletCharacter = '\u2022';

                    textPart.Font.FontSize = 18;
                    textPart.Font.FontName = "Calibri";
                    textPart.Font.Color = ColorObject.FromArgb(144, 148, 152);


                }
            }

            shape = slide.Shapes.FirstOrDefault(u => u.ShapeName == "imgVilla") as IShape;
            if (shape is not null)
            {
                byte[] imageData;
                try
                {
                    imageData = System.IO.File.ReadAllBytes(basePath + villa.ImageUrl);
                }
                catch (Exception e)
                {
                    imageData = System.IO.File.ReadAllBytes(basePath + "/images/placeholder.png");
                }
                slide.Shapes.Remove(shape);
                using MemoryStream imageStream = new(imageData);
                IPicture newPicture = slide.Pictures.AddPicture(imageStream, 60, 120, 300, 200);

            }

            MemoryStream memoryStream = new();
            presentation.Save(memoryStream);
            memoryStream.Position = 0;
            return File(memoryStream, "application/pptx", $"Villa_{villa.Name}.pptx");

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
