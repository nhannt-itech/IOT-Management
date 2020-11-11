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
    public class CameraController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IUnitOfWork _unitOfWork;
        public CameraController(IUnitOfWork unitOfWork, ApplicationDbContext db)
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
            CameraVM cameraVM = new CameraVM()
            {
                Camera = new Camera(),
                ApplicationUserList = _unitOfWork.ApplicationUser.GetAll().Select(i => new SelectListItem
                {
                    Text = i.UserName,
                    Value = i.Id.ToString()
                })
            };

            if (id == null)
            {
                return View(cameraVM);
            }

            cameraVM.Camera = _unitOfWork.Camera.Get(id.GetValueOrDefault()); //use int? id --> GetValueOrDefault

            if (cameraVM.Camera == null)
            {
                return NotFound();
            }
            return View(cameraVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(CameraVM cameraVM)
        {
            if (ModelState.IsValid)
            {
                if (cameraVM.Camera.Id == 0)
                {
                    _unitOfWork.Camera.Add(cameraVM.Camera);
                }
                else
                {
                    _unitOfWork.Camera.Update(cameraVM.Camera);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                cameraVM.ApplicationUserList = _unitOfWork.ApplicationUser.GetAll().Select(i => new SelectListItem
                {
                    Text = i.UserName,
                    Value = i.Id.ToString()
                });

                if (cameraVM.Camera.Id != 0)
                {
                    cameraVM.Camera = _unitOfWork.Camera.Get(cameraVM.Camera.Id);
                }
            }


            return View(cameraVM);

        }

        #region API_Calls

        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.Camera.GetAll(includeProperties: "ApplicationUser");
            return Json(new { data = allObj });
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
