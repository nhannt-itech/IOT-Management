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
using Microsoft.AspNetCore.Mvc;

namespace IOTManagementGroup7.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Auth_Customer + "," + SD.Role_Customer)]

    public class DeviceController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeviceController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index(string? id)
        {
            //id is SensorId
            DeviceHomeVM deviceHomeVM = new DeviceHomeVM()
            {
                Devices = _unitOfWork.Device.GetAll(x => x.SensorBoardId == id,
                                            includeProperties: "Sensor,DeviceType"),
                Sensor = _unitOfWork.Sensor.Get(id)
            };
            deviceHomeVM.Sensor.Project = _unitOfWork.Project.Get(deviceHomeVM.Sensor.ProjectId);
            return View(deviceHomeVM);
        }

        [HttpPost]
        public IActionResult TurnOnOff(string? id)
        {
            var obj = _unitOfWork.Device.GetFirstOrDefault(x => x.Id == id);
            if (obj.PowerStatus == 0)
            {
                obj.PowerStatus = 1;
                _unitOfWork.Device.Update(obj);
                _unitOfWork.Save();
                return Json(new { success = false, message = obj.Name + " đã tắt." });
            }
            obj.PowerStatus = 0;
            _unitOfWork.Device.Update(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = obj.Name + " đã kích hoạt." });
        }
        [HttpPost]
        public IActionResult ChangeRangeSlider(string? id, int value)
        {
            var obj = _unitOfWork.Device.GetFirstOrDefault(x => x.Id == id);
            obj.SliderRange = value;
            _unitOfWork.Device.Update(obj);
            _unitOfWork.Save();
            return Json(new { success = true, maxvalue = obj.SliderMaxRange });
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
