using BookingRoom.Domain.Entities;
using BookingRoom.Infrastructure.Data;
using BookingRoom.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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
            var villaNumbers = _db.VillaNumbers.Include(v=>v.Villa).ToList();
            return View(villaNumbers);
        }

        public IActionResult VillaNumberCreate()
        {
            VillaNumberVM villaNumberVM = new VillaNumberVM()
            {
                VillaList = _db.Villas.Select(v => new SelectListItem
                {
                    Text = v.Name,
                    Value = v.Id.ToString()
                })
            };
            return View(villaNumberVM);
        }
        [HttpPost]
        public IActionResult VillaNumberCreate(VillaNumberVM obj)
        {
            //ModelState.Remove("Villa");
            bool roomNumberExist = _db.VillaNumbers.Any(u => u.Villa_Number == obj.VillaNumber.Villa_Number);

            if (ModelState.IsValid && !roomNumberExist)
            {
                _db.VillaNumbers.Add(obj.VillaNumber);
                _db.SaveChanges();
                TempData["success"] = "Villa Number created successfully.";
                return RedirectToAction("VillaNumberIndex", "VillaNumber");
            }
            if (roomNumberExist)
            {
                TempData["error"] = "The villa number already exists.";
            }
            obj.VillaList = _db.Villas.Select(v => new SelectListItem
            {
                Text = v.Name,
                Value = v.Id.ToString()
            });
            return View(obj);
           
        }

        public IActionResult VillaNumberUpdate(int VillaNumberId)
        {
            VillaNumberVM villaNumberVM = new VillaNumberVM()
            {
                VillaList = _db.Villas.Select(v => new SelectListItem
                {
                    Text = v.Name,
                    Value = v.Id.ToString()
                }),
                VillaNumber = _db.VillaNumbers.FirstOrDefault(v=>v.Villa_Number == VillaNumberId)
            };

            if(villaNumberVM.VillaNumber == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(villaNumberVM);
        }

        [HttpPost]
        public IActionResult VillaNumberUpdate(VillaNumberVM villaNumberVM)
        {
            if (ModelState.IsValid)
            {
                _db.VillaNumbers.Update(villaNumberVM.VillaNumber);
                _db.SaveChanges();
                TempData["success"] = "Villa Number Updated successfully.";
                return RedirectToAction(nameof(VillaNumberIndex));
            }
            villaNumberVM.VillaList = _db.Villas.Select(v => new SelectListItem
            {
                Text = v.Name,
                Value = v.Id.ToString()
            });

            return View(villaNumberVM);

        }

        public IActionResult VillaNumberDelete(int VillaNumberId)
        {
            VillaNumberVM villaNumberVM = new VillaNumberVM()
            {
                VillaList = _db.Villas.Select(v => new SelectListItem
                {
                    Text = v.Name,
                    Value = v.Id.ToString()
                }),
                VillaNumber = _db.VillaNumbers.FirstOrDefault(v => v.Villa_Number == VillaNumberId)
            };

            if (villaNumberVM.VillaNumber == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(villaNumberVM);
        }

        [HttpPost]
        public IActionResult VillaNumberDelete(VillaNumberVM villaNumberVM)
        {
            VillaNumber? villaFromDb = _db.VillaNumbers.FirstOrDefault(v => v.Villa_Number == villaNumberVM.VillaNumber.Villa_Number);
            if (villaFromDb is not null)
            {
                _db.VillaNumbers.Remove(villaFromDb);
                _db.SaveChanges();
                TempData["success"] = "Villa number deleted successfully.";
                return RedirectToAction(nameof(VillaNumberIndex));
            }
            TempData["error"] = "Villa number coluld not ne deleted.";
            return View();

        }
    }
}
