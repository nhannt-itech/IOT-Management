using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using IOTManagementGroup7.DataAccess.Data;
using IOTManagementGroup7.DataAccess.Repository.IRepository;
using IOTManagementGroup7.Models;
using Microsoft.AspNetCore.Mvc;
using IOTManagementGroup7.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IOTManagementGroup7.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FridgeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _db;
        public FridgeController(IUnitOfWork unitOfWork, ApplicationDbContext db)
        {
            _unitOfWork = unitOfWork;
            _db = db;
        }

        public IActionResult Index()
        {
            
            return View();
        }

        public IActionResult Upsert(int? id)
        {

            FridgeVM fridgeVM = new FridgeVM()
            {
                Fridge = new Fridge(),
                ApplicationUserList = _unitOfWork.ApplicationUser.GetAll().Select(i => new SelectListItem
                {
                    Text = i.UserName,
                    Value = i.Id.ToString()
                })
            };

            if (id == null)
            {
                return View(fridgeVM);
            }
            fridgeVM.Fridge = _unitOfWork.Fridge.Get(id.GetValueOrDefault());
            if (fridgeVM.Fridge == null)
            {
                return NotFound();
            }
            return View(fridgeVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(FridgeVM fridgeVM)
        {
            if (ModelState.IsValid)
            {
                if (fridgeVM.Fridge.Id == 0)
                {
                    _unitOfWork.Fridge.Add(fridgeVM.Fridge);

                }
                else
                {
                    _unitOfWork.Fridge.Update(fridgeVM.Fridge);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                fridgeVM.ApplicationUserList = _unitOfWork.ApplicationUser.GetAll().Select(i => new SelectListItem
                {
                    Text = i.UserName,
                    Value = i.Id.ToString()
                });
                
                if (fridgeVM.Fridge.Id != 0)
                {
                    fridgeVM.Fridge = _unitOfWork.Fridge.Get(fridgeVM.Fridge.Id);
                }
            }
          

            return View(fridgeVM);
        }

        #region API_Calls

        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.Fridge.GetAll(includeProperties: "ApplicationUser");
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = _unitOfWork.Fridge.Get(id.GetValueOrDefault());
            if (obj == null)
            {
                return Json(new { success = false, message = "Error When Delete!" });
            }
            _unitOfWork.Fridge.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful!" });
        }
        #endregion
    }
}
