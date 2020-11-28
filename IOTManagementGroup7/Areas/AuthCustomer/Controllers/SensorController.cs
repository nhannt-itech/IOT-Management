using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IOTManagementGroup7.DataAccess.Repository.IRepository;
using IOTManagementGroup7.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace IOTManagementGroup7.Areas.AuthCustomer.Controllers
{
    [Area("AuthCustomer")]
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
            //id này là id của Phòng
            SensorHomeVM sensorHomeVM = new SensorHomeVM();
            sensorHomeVM.Sensors = _unitOfWork.Sensor.GetAll(x => x.ProjectId == id); //Bằng với id của phòng
            foreach (var item in sensorHomeVM.Sensors)
            {
                item.Devices = _unitOfWork.Device.GetAll(x => x.SensorBoardId == item.Id, includeProperties: "DeviceType");
            }
            return View(sensorHomeVM);
        }
    }
}
