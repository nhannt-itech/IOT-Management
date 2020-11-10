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
    public class FridgeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public FridgeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Fridge fridge = new Fridge();
            if (id == null)
            {
                return View(fridge);
            }

            fridge = _unitOfWork.Fridge.Get(id.GetValueOrDefault()); //use int? id --> GetValueOrDefault

            if (fridge == null)
            {
                return NotFound();
            }
            return View(fridge);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Fridge fridge)
        {
            if (ModelState.IsValid)
            {
                if (fridge.Id == 0)
                {
                    _unitOfWork.Fridge.Add(fridge);
                }
                else
                {
                    _unitOfWork.Fridge.Update(fridge);
                }
                
            }
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));

        }

        #region API_Calls

        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.Fridge.GetAll();
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
