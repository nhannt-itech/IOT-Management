using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IOTManagementGroup7.DataAccess.Repository.IRepository;
using IOTManagementGroup7.Models;
using IOTManagementGroup7.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        public IActionResult Index(string? id)
        {
            //id này của Sensor
            DeviceHomeVM deviceHomeVM = new DeviceHomeVM()
            {
                Devices = _unitOfWork.Device.GetAll(x => x.SensorBoardId == id,
                                            includeProperties: "Sensor,DeviceType"),
                Sensor = _unitOfWork.Sensor.Get(id)
            };
            deviceHomeVM.Sensor.Project = _unitOfWork.Project.Get(deviceHomeVM.Sensor.ProjectId);
            return View(deviceHomeVM);
        }
        [HttpGet]
        public IActionResult Upsert(string? id, string? idSensor)
        {
            Device device = _unitOfWork.Device.GetFirstOrDefault(x => x.Id == id, includeProperties: "Sensor");
            if (device == null)
            {
                device = new Device()
                {
                    Id = "",
                    SensorBoardId = idSensor,
                    DeviceTypeList = _unitOfWork.DeviceType.GetAll().Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    })
                };
            }
            device.DeviceTypeList = _unitOfWork.DeviceType.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            return View(device);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Device device)
        {
            device.DeviceTypeList = _unitOfWork.DeviceType.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            if (device.Id == null)
            {
                if (_unitOfWork.Device.GetAll().Count() == 0)
                {
                    device.Id = "D1";
                }
                else
                {
                    int maxId = _unitOfWork.Device.GetAll()
                        .Select(x => Convert.ToInt32(x.Id.Replace("D", ""))).Max();
                    device.Id = "D" + (maxId + 1).ToString();
                }
                _unitOfWork.Device.Add(device);
            }
            else
            {
                _unitOfWork.Device.Update(device);
            }
            _unitOfWork.Save();

            string idPro = _unitOfWork.Sensor.Get(device.SensorBoardId).ProjectId;
            return RedirectToAction("Index", "Device", new { id = device.SensorBoardId });
        }
        [HttpPost]
        public IActionResult TurnOnOff(string? id)
        {
            var obj = _unitOfWork.Device.GetFirstOrDefault(x => x.Id == id);
            if (obj.PowerStatus == 1)
            {
                obj.PowerStatus = 0;
                _unitOfWork.Device.Update(obj);
                _unitOfWork.Save();
                return Json(new { success = false, message = obj.Name + " đã tắt." });
            }
            obj.PowerStatus = 1;
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
                userName =  String.Empty;

            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                byte[] textData = System.Text.Encoding.UTF8.GetBytes(userName);
                byte[] hash = sha.ComputeHash(textData);
                userName =  BitConverter.ToString(hash).Replace("-", String.Empty);
            }
            string[] deviceStatus = { "0", "0", "0", "0" };
            for(int i = 0; i < obj.Count(); i++)
            {
                deviceStatus[i] = obj[i].PowerStatus.ToString();
            }
            string result = idDevice + ',' + userName + ',' + idProject + ',' + idSensor + ',' + deviceStatus[0] + '-' + deviceStatus[1] + '-' + deviceStatus[2] + '-' + deviceStatus[3];
            return Json(result);
        }

        [HttpDelete]
        public IActionResult Delete(string? id)
        {
            var obj = _unitOfWork.Device.Get(id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error When Delete!" });
            }
            _unitOfWork.Device.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful!" });
        }
    }

}