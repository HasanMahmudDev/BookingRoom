using BookingRoom.Application.Common.Interfaces;
using BookingRoom.Domain.Entities;
using BookingRoom.Infrastructure.Data;
using BookingRoom.Infrastructure.Repository;
using BookingRoom.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BookingRoom.Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public VillaNumberController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult VillaNumberIndex()
        {
            var villaNumbers = _unitOfWork.VillaNumber.GetAll(includeProperties:"Villa");
            return View(villaNumbers);
        }

        public IActionResult VillaNumberCreate()
        {
            VillaNumberVM villaNumberVM = new VillaNumberVM()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(v => new SelectListItem
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
            bool roomNumberExist = _unitOfWork.VillaNumber.Any(u => u.Villa_Number == obj.VillaNumber.Villa_Number);

            if (ModelState.IsValid && !roomNumberExist)
            {
                _unitOfWork.VillaNumber.Add(obj.VillaNumber);
                _unitOfWork.Save();
                TempData["success"] = "Villa Number created successfully.";
                return RedirectToAction("VillaNumberIndex", "VillaNumber");
            }
            if (roomNumberExist)
            {
                TempData["error"] = "The villa number already exists.";
            }
            obj.VillaList = _unitOfWork.Villa.GetAll().Select(v => new SelectListItem
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
                VillaList = _unitOfWork.Villa.GetAll().Select(v => new SelectListItem
                {
                    Text = v.Name,
                    Value = v.Id.ToString()
                }),
                VillaNumber = _unitOfWork.VillaNumber.Get(v=>v.Villa_Number == VillaNumberId)
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
                _unitOfWork.VillaNumber.Update(villaNumberVM.VillaNumber);
                _unitOfWork.Save();
                TempData["success"] = "Villa Number Updated successfully.";
                return RedirectToAction(nameof(VillaNumberIndex));
            }
            villaNumberVM.VillaList = _unitOfWork.Villa.GetAll().Select(v => new SelectListItem
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
                VillaList = _unitOfWork.Villa.GetAll().Select(v => new SelectListItem
                {
                    Text = v.Name,
                    Value = v.Id.ToString()
                }),
                VillaNumber = _unitOfWork.VillaNumber.Get(v => v.Villa_Number == VillaNumberId)
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
            VillaNumber? villaFromDb = _unitOfWork.VillaNumber.Get(v => v.Villa_Number == villaNumberVM.VillaNumber.Villa_Number);
            if (villaFromDb is not null)
            {
                _unitOfWork.VillaNumber.Remove(villaFromDb);
                _unitOfWork.Save();
                TempData["success"] = "Villa number deleted successfully.";
                return RedirectToAction(nameof(VillaNumberIndex));
            }
            TempData["error"] = "Villa number coluld not ne deleted.";
            return View();

        }
    }
}
