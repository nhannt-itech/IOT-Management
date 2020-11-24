using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IOTManagementGroup7.DataAccess.Repository.IRepository;
using IOTManagementGroup7.Models;
using IOTManagementGroup7.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IOTManagementGroup7.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LightController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public LightController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            LightVM lightVM = new LightVM()
            {
                Light = new Light(),
                ApplicationUserList = _unitOfWork.ApplicationUser.GetAll().Select(i => new SelectListItem
                {
                    Text = i.UserName,
                    Value = i.Id.ToString()
                })
            };

            if (id == null)
            {
                return View(lightVM);
            }
            lightVM.Light = _unitOfWork.Light.Get(id.GetValueOrDefault());
            if (lightVM.Light == null)
            {
                return NotFound();
            }
            return View(lightVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(LightVM lightVM)
        {
            if (ModelState.IsValid)
            {
                if (lightVM.Light.Id == 0)
                {
                    _unitOfWork.Light.Add(lightVM.Light);
                }
                else
                {
                    _unitOfWork.Light.Update(lightVM.Light);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                lightVM.ApplicationUserList = _unitOfWork.ApplicationUser.GetAll().Select(i => new SelectListItem
                {
                    Text = i.UserName,
                    Value = i.Id.ToString()
                });

                if (lightVM.Light.Id != 0)
                {
                    lightVM.Light = _unitOfWork.Light.Get(lightVM.Light.Id);
                }
            }
            return View(lightVM);
        }

        #region API_Calls

        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.Light.GetAll(includeProperties: "ApplicationUser");
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = _unitOfWork.Light.Get(id.GetValueOrDefault());
            if (obj == null)
            {
                return Json(new { success = false, message = "Error When Delete!" });
            }
            _unitOfWork.Light.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful!" });
        }
        #endregion
    }
}
