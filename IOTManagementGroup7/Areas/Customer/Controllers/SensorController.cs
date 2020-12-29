using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IOTManagementGroup7.DataAccess.Repository.IRepository;
using IOTManagementGroup7.Models.ViewModels;
using IOTManagementGroup7.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace IOTManagementGroup7.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Auth_Customer + "," + SD.Role_Customer)]
    public class SensorController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;
        public SensorController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult Index(string? id)
        {
            //id is ProjectId
            SensorHomeVM sensorHomeVM = new SensorHomeVM();
            sensorHomeVM.Sensors = _unitOfWork.Sensor.GetAll(x => x.ProjectId == id, includeProperties: "Project");
            sensorHomeVM.Project = _unitOfWork.Project.Get(id);
            return View(sensorHomeVM);
        }
    }
}
