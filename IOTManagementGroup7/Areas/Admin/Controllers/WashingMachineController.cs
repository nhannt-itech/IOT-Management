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
    public class WashingMachineController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _db;
        public WashingMachineController(IUnitOfWork unitOfWork, ApplicationDbContext db)
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

            WashingMachineVM washingMachineVM = new WashingMachineVM()
            {
                WashingMachine = new WashingMachine(),
                ApplicationUserList = _unitOfWork.ApplicationUser.GetAll().Select(i => new SelectListItem
                {
                    Text = i.UserName,
                    Value = i.Id.ToString()
                })
            };

            if (id == null)
            {
                return View(washingMachineVM);
            }
            washingMachineVM.WashingMachine = _unitOfWork.WashingMachine.Get(id.GetValueOrDefault());
            if (washingMachineVM.WashingMachine == null)
            {
                return NotFound();
            }
            return View(washingMachineVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(WashingMachineVM washingMachineVM)
        {
            if (ModelState.IsValid)
            {
                if (washingMachineVM.WashingMachine.Id == 0)
                {
                    _unitOfWork.WashingMachine.Add(washingMachineVM.WashingMachine);

                }
                else
                {
                    _unitOfWork.WashingMachine.Update(washingMachineVM.WashingMachine);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                washingMachineVM.ApplicationUserList = _unitOfWork.ApplicationUser.GetAll().Select(i => new SelectListItem
                {
                    Text = i.UserName,
                    Value = i.Id.ToString()
                });
                
                if (washingMachineVM.WashingMachine.Id != 0)
                {
                    washingMachineVM.WashingMachine = _unitOfWork.WashingMachine.Get(washingMachineVM.WashingMachine.Id);
                }
            }
          

            return View(washingMachineVM);
        }

        #region API_Calls

        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.WashingMachine.GetAll(includeProperties: "ApplicationUser");
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = _unitOfWork.WashingMachine.Get(id.GetValueOrDefault());
            if (obj == null)
            {
                return Json(new { success = false, message = "Error When Delete!" });
            }
            _unitOfWork.WashingMachine.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful!" });
        }
        #endregion
    }
}
