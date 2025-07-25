using BookingRoom.Domain.Entities;
using BookingRoom.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace BookingRoom.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly ApplicationDbContext _db;
        public VillaController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var villas = _db.Villas.ToList();
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
                _db.Villas.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index", "Villa");
            }
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
                return RedirectToAction("Index", "Villa");
            }
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
                return RedirectToAction("Index", "Villa");
            }
            return View();

        }
    }
}
