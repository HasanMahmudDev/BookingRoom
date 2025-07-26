using BookingRoom.Domain.Entities;
using BookingRoom.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace BookingRoom.Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly ApplicationDbContext _db;
        public VillaNumberController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult VillaNumberIndex()
        {
            var villaNumbers = _db.VillaNumbers.ToList();
            return View(villaNumbers);
        }

        public IActionResult VillaNumberCreate()
        {
            return View();
        }
        [HttpPost]
        public IActionResult VillaNumberCreate(Villa obj)
        {
            if (ModelState.IsValid)
            {
                _db.Villas.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "Villa Number created successfully.";
                return RedirectToAction("Index", "Villa");
            }
            TempData["error"] = "Villa creation failed.";
            return View();
           
        }

        public IActionResult Update(int VillaId)
        {
            Villa? obj = _db.Villas.FirstOrDefault(v => v.Id == VillaId);
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
                _db.Villas.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "Villa updated successfully.";
                return RedirectToAction("Index", "Villa");
            }
            TempData["error"] = "Villa not found.";
            return View();

        }

        public IActionResult Delete(int VillaId)
        {
            Villa? obj = _db.Villas.FirstOrDefault(v => v.Id == VillaId);
            if (obj is null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(obj);
        }

        [HttpPost]
        public IActionResult Delete(Villa obj)
        {
            Villa? villaFromDb = _db.Villas.FirstOrDefault(v => v.Id == obj.Id);
            if (villaFromDb is not null)
            {
                _db.Villas.Remove(villaFromDb);
                _db.SaveChanges();
                TempData["success"] = "Villa deleted successfully.";
                return RedirectToAction("Index", "Villa");
            }
            TempData["error"] = "Villa not found.";
            return View();

        }
    }
}
