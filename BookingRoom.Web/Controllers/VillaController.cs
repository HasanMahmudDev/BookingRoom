using BookingRoom.Application.Common.Interfaces;
using BookingRoom.Domain.Entities;
using BookingRoom.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace BookingRoom.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public VillaController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var villas = _unitOfWork.Villa.GetAll();
            return View(villas);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Villa obj)
        {
            if (obj.Name == obj.Description)
            {
                ModelState.AddModelError("name", "The description connot exactly match the Name.");
            }
            if (ModelState.IsValid)
            {
                if(obj.Image != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(obj.Image.FileName);
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"Images/VillaImage");
                    using var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create);
                    obj.Image.CopyTo(fileStream);

                    obj.ImageUrl = @"\Images\VillaImage\" + fileName;

                }
                else
                {
                    obj.ImageUrl = "https://placehold.co/600x400";
                }
                _unitOfWork.Villa.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Villa created successfully.";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "Villa creation failed.";
            return View();
           
        }

        public IActionResult Update(int VillaId)
        {
            Villa? obj = _unitOfWork.Villa.Get(v => v.Id == VillaId);
            if(obj == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(obj);
        }

        [HttpPost]
        public IActionResult Update(Villa obj)
        {
            if (ModelState.IsValid && obj.Id>0)
            {
                _unitOfWork.Villa.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Villa updated successfully.";
                return RedirectToAction("Index", "Villa");
            }
            TempData["error"] = "Villa not found.";
            return View();

        }

        public IActionResult Delete(int VillaId)
        {
            Villa? obj = _unitOfWork.Villa.Get(v => v.Id == VillaId);
            if (obj is null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(obj);
        }

        [HttpPost]
        public IActionResult Delete(Villa obj)
        {
            Villa? villaFromDb = _unitOfWork.Villa.Get(v => v.Id == obj.Id);
            if (villaFromDb is not null)
            {
                _unitOfWork.Villa.Remove(villaFromDb);
                _unitOfWork.Save();
                TempData["success"] = "Villa deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "Villa not found.";
            return View();

        }
    }
}
