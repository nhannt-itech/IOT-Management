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
    public class DeviceController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public DeviceController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult Index(string? id, DeviceHomeVM deviceHomeVM)
        {
            deviceHomeVM.Devices = _unitOfWork.Device.GetAll(x => x.SensorBoardId == id, includeProperties: "Sensor,DeviceType");

            return View(deviceHomeVM);
        }
    }
}
