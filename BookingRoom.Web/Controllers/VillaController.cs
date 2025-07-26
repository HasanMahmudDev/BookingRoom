using BookingRoom.Application.Common.Interfaces;
using BookingRoom.Domain.Entities;
using BookingRoom.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace BookingRoom.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IVillaRepository _villaRepository;
        public VillaController(IVillaRepository villaRepository)
        {
            _villaRepository = villaRepository;
        }
        public IActionResult Index()
        {
            var villas = _villaRepository.GetAll();
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
                _villaRepository.Add(obj);
                _villaRepository.Save();
                TempData["success"] = "Villa created successfully.";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "Villa creation failed.";
            return View();
           
        }

        public IActionResult Update(int VillaId)
        {
            Villa? obj = _villaRepository.Get(v => v.Id == VillaId);
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
                _villaRepository.Update(obj);
                _villaRepository.Save();
                TempData["success"] = "Villa updated successfully.";
                return RedirectToAction("Index", "Villa");
            }
            TempData["error"] = "Villa not found.";
            return View();

        }

        public IActionResult Delete(int VillaId)
        {
            Villa? obj = _villaRepository.Get(v => v.Id == VillaId);
            if (obj is null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(obj);
        }

        [HttpPost]
        public IActionResult Delete(Villa obj)
        {
            Villa? villaFromDb = _villaRepository.Get(v => v.Id == obj.Id);
            if (villaFromDb is not null)
            {
                _villaRepository.Remove(villaFromDb);
                _villaRepository.Save();
                TempData["success"] = "Villa deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "Villa not found.";
            return View();

        }
    }
}
