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
    public class TVController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public TVController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            TV tV = new TV();
            if (id == null)
            {
                return View(tV);
            }

            tV = _unitOfWork.TV.Get(id.GetValueOrDefault()); //use int? id --> GetValueOrDefault

            if (tV == null)
            {
                return NotFound();
            }
            return View(tV);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(TV tV)
        {
            if (ModelState.IsValid)
            {
                if (tV.Id == 0)
                {
                    _unitOfWork.TV.Add(tV);
                }
                else
                {
                    _unitOfWork.TV.Update(tV);
                }

            }
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        #region API_Calls

        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.TV.GetAll();
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = _unitOfWork.TV.Get(id.GetValueOrDefault());
            if (obj == null)
            {
                return Json(new { success = false, message = "Error When Delete!" });
            }
            _unitOfWork.TV.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful!" });
        }
        #endregion
    }
}
