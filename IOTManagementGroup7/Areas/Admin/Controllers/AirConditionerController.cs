using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
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
    public class AirConditionerController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IUnitOfWork _unitOfWork;
        public AirConditionerController(IUnitOfWork unitOfWork, ApplicationDbContext db)
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
            AirConditionerVM airConditionerVM = new AirConditionerVM()
            {
                AirConditioner = new AirConditioner(),
                ApplicationUserList = _unitOfWork.ApplicationUser.GetAll().Select(i => new SelectListItem
                {
                    Text = i.UserName,
                    Value = i.Id.ToString()
                })
            };

            if (id == null)
            {
                return View(airConditionerVM);
            }

            airConditionerVM.AirConditioner = _unitOfWork.AirConditioner.Get(id.GetValueOrDefault()); //use int? id --> GetValueOrDefault

            if (airConditionerVM.AirConditioner == null)
            {
                return NotFound();
            }
            return View(airConditionerVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(AirConditionerVM airConditionerVM)
        {
            if (ModelState.IsValid)
            {
                if (airConditionerVM.AirConditioner.Id == 0)
                {
                    _unitOfWork.AirConditioner.Add(airConditionerVM.AirConditioner);
                }
                else
                {
                    _unitOfWork.AirConditioner.Update(airConditionerVM.AirConditioner);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                airConditionerVM.ApplicationUserList = _unitOfWork.ApplicationUser.GetAll().Select(i => new SelectListItem
                {
                    Text = i.UserName,
                    Value = i.Id.ToString()
                });

                if (airConditionerVM.AirConditioner.Id != 0)
                {
                    airConditionerVM.AirConditioner = _unitOfWork.AirConditioner.Get(airConditionerVM.AirConditioner.Id);
                }
            }


            return View(airConditionerVM);

        }

        #region API_Calls

        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.AirConditioner.GetAll(includeProperties: "ApplicationUser");
            return Json(new { data = allObj });
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = _unitOfWork.AirConditioner.Get(id.GetValueOrDefault());
            if (obj == null)
            {
                return Json(new { success = false, message = "Error When Delete!" });
            }
            _unitOfWork.AirConditioner.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful!" });
        }
        #endregion
    }
}
