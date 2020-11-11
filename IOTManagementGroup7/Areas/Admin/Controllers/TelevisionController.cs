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
    public class TelevisionController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _db;
        public TelevisionController(IUnitOfWork unitOfWork, ApplicationDbContext db)
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

            TelevisionVM televisionVM = new TelevisionVM()
            {
                Television = new Television(),
                ApplicationUserList = _unitOfWork.ApplicationUser.GetAll().Select(i => new SelectListItem
                {
                    Text = i.UserName,
                    Value = i.Id.ToString()
                })
            };

            if (id == null)
            {
                return View(televisionVM);
            }
            televisionVM.Television = _unitOfWork.Television.Get(id.GetValueOrDefault());
            if (televisionVM.Television == null)
            {
                return NotFound();
            }
            return View(televisionVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(TelevisionVM televisionVM)
        {
            if (ModelState.IsValid)
            {
                if (televisionVM.Television.Id == 0)
                {
                    _unitOfWork.Television.Add(televisionVM.Television);

                }
                else
                {
                    _unitOfWork.Television.Add(televisionVM.Television);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                televisionVM.ApplicationUserList = _unitOfWork.ApplicationUser.GetAll().Select(i => new SelectListItem
                {
                    Text = i.UserName,
                    Value = i.Id.ToString()
                });
                
                if (televisionVM.Television.Id != 0)
                {
                    televisionVM.Television = _unitOfWork.Television.Get(televisionVM.Television.Id);
                }
            }
          

            return View(televisionVM);
        }

        #region API_Calls

        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.Television.GetAll(includeProperties: "ApplicationUser");
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = _unitOfWork.Television.Get(id.GetValueOrDefault());
            if (obj == null)
            {
                return Json(new { success = false, message = "Error When Delete!" });
            }
            _unitOfWork.Television.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful!" });
        }
        #endregion
    }
}
