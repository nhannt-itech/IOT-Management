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
    public class CameraController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CameraController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {

            return View();
        }



        #region API_Calls

        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.Camera.GetAll();
            return Json(new { data = allObj });
        }

        public IActionResult Upsert(int? id)
        {
            Camera camera = new Camera();
            if (id == null)
            {
                return View(camera);
            }

            camera = _unitOfWork.Camera.Get(id.GetValueOrDefault()); //use int? id --> GetValueOrDefault

            if (camera == null)
            {
                return NotFound();
            }
            return View(camera);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Camera camera)
        {
            if (ModelState.IsValid)
            {
                if (camera.Id.Length == 0)
                {
                    _unitOfWork.Camera.Add(camera);
                }
                else
                {
                    _unitOfWork.Camera.Update(camera);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(camera);
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = _unitOfWork.Camera.Get(id.GetValueOrDefault());
            if (obj == null)
            {
                return Json(new { success = false, message = "Error When Delete!" });
            }
            _unitOfWork.Camera.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful!" });
        }
        #endregion
    }
}
