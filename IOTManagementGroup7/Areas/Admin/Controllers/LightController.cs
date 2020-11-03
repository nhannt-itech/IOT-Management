using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IOTManagementGroup7.DataAccess.Repository.IRepository;
using IOTManagementGroup7.Models;
using Microsoft.AspNetCore.Mvc;

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
            Light light = new Light();
            if (id == null)
            {
                return View(light);
            }

            light = _unitOfWork.Light.Get(id.GetValueOrDefault()); //use int? id --> GetValueOrDefault

            if (light == null)
            {
                return NotFound();
            }
            return View(light);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Light light)
        {
            if (ModelState.IsValid)
            {
                if (light.Id == 0)
                {
                    _unitOfWork.Light.Add(light);
                }
                else
                {
                    _unitOfWork.Light.Update(light);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(light);
        }

        #region API_Calls

        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.Light.GetAll();
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
