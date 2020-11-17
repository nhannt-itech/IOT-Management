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
    public class FanController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IUnitOfWork _unitOfWork;
        public FanController(IUnitOfWork unitOfWork, ApplicationDbContext db)
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
            FanVM fanVM = new FanVM()
            {
                Fan = new Fan(),
                ApplicationUserList = _unitOfWork.ApplicationUser.GetAll().Select(i => new SelectListItem
                {
                    Text = i.UserName,
                    Value = i.Id.ToString()
                })
            };

            if (id == null)
            {
                return View(fanVM);
            }

            fanVM.Fan = _unitOfWork.Fan.Get(id.GetValueOrDefault()); //use int? id --> GetValueOrDefault

            if (fanVM.Fan == null)
            {
                return NotFound();
            }
            return View(fanVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(FanVM fanVM)
        {
            if (ModelState.IsValid)
            {
                if (fanVM.Fan.Id == 0)
                {
                    _unitOfWork.Fan.Add(fanVM.Fan);
                }
                else
                {
                    _unitOfWork.Fan.Update(fanVM.Fan);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                fanVM.ApplicationUserList = _unitOfWork.ApplicationUser.GetAll().Select(i => new SelectListItem
                {
                    Text = i.UserName,
                    Value = i.Id.ToString()
                });

                if (fanVM.Fan.Id != 0)
                {
                    fanVM.Fan = _unitOfWork.Fan.Get(fanVM.Fan.Id);
                }
            }


            return View(fanVM);

        }

        #region API_Calls

        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.Fan.GetAll(includeProperties: "ApplicationUser");
            return Json(new { data = allObj });
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = _unitOfWork.Fan.Get(id.GetValueOrDefault());
            if (obj == null)
            {
                return Json(new { success = false, message = "Error When Delete!" });
            }
            _unitOfWork.Fan.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful!" });
        }
        #endregion
    }
}
