using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IOTManagementGroup7.DataAccess.Data;
using IOTManagementGroup7.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace IOTManagementGroup7.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ApplicationUserController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEvironment;

        public ApplicationUserController(ApplicationDbContext db, IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _db = db;
            _unitOfWork = unitOfWork;
            _hostEvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region API CALL
        [HttpGet]
        public IActionResult GetAll()
        {
            var userList = _db.ApplicationUsers.ToList();
            var userRole = _db.UserRoles.ToList();
            var roles = _db.Roles.ToList();
            foreach (var user in userList)
            {
                var roleId = userRole.FirstOrDefault(u => u.UserId == user.Id).RoleId;
                user.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;
            }

            return Json(new { data = userList });
        }

        public IActionResult LockUnlock([FromBody] string id)
        {
            var objFromDb = _db.ApplicationUsers.FirstOrDefault(u => u.Id == id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while Locking/Unclocking" });
            }
            if (objFromDb.LockoutEnd != null && objFromDb.LockoutEnd > DateTime.Now)
            {
                objFromDb.LockoutEnd = DateTime.Now;
            }
            else
            {
                objFromDb.LockoutEnd = DateTime.Now.AddYears(1000);
            }
            _db.SaveChanges();
            return Json(new { success = true, message = "Operation Successful." });
        }
        #endregion
    }
}
