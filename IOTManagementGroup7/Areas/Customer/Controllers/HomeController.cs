using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using IOTManagementGroup7.Models;
using IOTManagementGroup7.DataAccess.Repository.IRepository;
using IOTManagementGroup7.Utility;
using Microsoft.AspNetCore.Identity;

namespace IOTManagementGroup7.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult LoadBarChart()
        {
            var index = 0;
            var allObjDeviceType = _unitOfWork.DeviceType.GetAll();
            string[] Labels = new string[allObjDeviceType.Count()];
            string[] Values = new string[allObjDeviceType.Count()];

            foreach (var item in allObjDeviceType)
            {
                Labels[index] = item.Name;
                Values[index] = _unitOfWork.Device.GetAll().Count(x => x.DeviceTypeId == item.Id).ToString();
                index++;
            }
            return Json(new { labels = Labels, values = Values });
        }

        [HttpGet]
        public IActionResult LoadCard()
        {
            int NumUsers = _unitOfWork.ApplicationUser.GetAll().Count();
            int NumProjects = _unitOfWork.Project.GetAll().Count();
            int NumDeviceTypes = _unitOfWork.DeviceType.GetAll().Count();
            int NumDevices = _unitOfWork.Device.GetAll().Count();

            return Json(new
            {
                numUsers = NumUsers,
                numProjects = NumProjects,
                numDeviceTypes = NumDeviceTypes,
                numDevices = NumDevices
            });
        }
    }
}
