using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IOTManagementGroup7.DataAccess.Data;
using IOTManagementGroup7.DataAccess.Repository.IRepository;
using IOTManagementGroup7.Models;
using IOTManagementGroup7.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IOTManagementGroup7.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = SD.Role_Admin)]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IUnitOfWork _unitOfWork;

        public UserController(ApplicationDbContext db, IUnitOfWork unitOfWork)
        {
            _db = db;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Update(string? id)
        {
            ApplicationUser applicationUser = new ApplicationUser();
            applicationUser = _db.ApplicationUsers.FirstOrDefault(x=>x.Id == id);
            return View(applicationUser);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                var obj = _db.ApplicationUsers.FirstOrDefault(x => x.Id == applicationUser.Id);
                obj.Name = applicationUser.Name;
                obj.PhoneNumber = applicationUser.PhoneNumber;
                _db.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        #region API_Calls
        public IActionResult GetAll()
        {
            var listUser = _db.ApplicationUsers.ToList();
            var userRoles = _db.UserRoles.ToList();
            var listRole = _db.Roles.ToList();

            foreach (var user in listUser)
            {
                var idUserRole = userRoles.FirstOrDefault(x => x.UserId == user.Id).RoleId;
                user.Role = listRole.FirstOrDefault(x => x.Id == idUserRole).Name;
            }
            return Json(new { data = listUser });
        }

        [HttpPost]
        public IActionResult LockUnlock([FromBody] string id) //Take notice this [FromBody]
        {
            var obj = _db.ApplicationUsers.FirstOrDefault(x => x.Id == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while Lock/Unlock user!" });
            }
            if (obj.LockoutEnd != null && obj.LockoutEnd > DateTime.Now)
            {
                obj.LockoutEnd = DateTime.Now;
            }
            else
            {
                obj.LockoutEnd = DateTime.Now.AddYears(1000);
            }
            _db.SaveChanges();
            return Json(new { success = true, message = "Lock/Unlock user successfully!" });
        }

        [HttpDelete]
        public IActionResult Delete(string? id)
        {
            var userObj = _db.ApplicationUsers.FirstOrDefault(x => x.Id == id);
            var roleObj = _db.UserRoles.FirstOrDefault(x => x.UserId == id);
            if (userObj == null)
            {
                return Json(new { success = false, message = "Error When Delete!" });
            }
            _db.UserRoles.Remove(roleObj);
            _db.ApplicationUsers.Remove(userObj);
            _db.SaveChanges();
            return Json(new { success = true, message = "Delete successful!" });
        }
        #endregion
    }
}
