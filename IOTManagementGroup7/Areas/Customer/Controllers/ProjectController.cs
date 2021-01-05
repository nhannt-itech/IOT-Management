using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IOTManagementGroup7.DataAccess.Repository.IRepository;
using IOTManagementGroup7.Models;
using IOTManagementGroup7.Models.ViewModels;
using IOTManagementGroup7.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IOTManagementGroup7.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Auth_Customer + "," + SD.Role_Customer)]
    public class ProjectController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly UserManager<IdentityUser> _userManager;

        public ProjectController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
            _userManager = userManager;
        }

        public IActionResult Index(ProjectHomeVM projectHomeVM)
        {
            projectHomeVM.Projects = _unitOfWork.Project.GetAll(x => x.CustomerUserId == _userManager.GetUserId(User)
                                                                , includeProperties: "ApplicationUser,CustomerUser");
            return View(projectHomeVM);
        }
        [HttpGet]
        public IActionResult GetDeviceAPI(string? idDevice, string userName, string idProject, string idSensor)
        {
            List<Device> obj = _unitOfWork.Device.GetAll(x => x.SensorBoardId == idSensor).ToList();

            if (String.IsNullOrEmpty(userName))
                userName = String.Empty;

            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                byte[] textData = System.Text.Encoding.UTF8.GetBytes(userName);
                byte[] hash = sha.ComputeHash(textData);
                userName = BitConverter.ToString(hash).Replace("-", String.Empty);
            }
            string[] deviceStatus = { "1", "1", "1", "1" };
            for (int i = 0; i < obj.Count(); i++)
            {
                deviceStatus[i] = obj[i].PowerStatus.ToString();
            }
            string result = "1" + ',' + userName + ',' + idProject + ',' + idSensor + ',' + deviceStatus[0] + '-' + deviceStatus[1] + '-' + deviceStatus[2] + '-' + deviceStatus[3];
            return Json(result);
        }
    }
}
