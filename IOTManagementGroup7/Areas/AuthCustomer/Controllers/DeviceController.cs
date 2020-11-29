using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IOTManagementGroup7.DataAccess.Repository.IRepository;
using IOTManagementGroup7.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IOTManagementGroup7.Areas.AuthCustomer.Controllers
{
    [Area("AuthCustomer")]
    public class DeviceController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeviceController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
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
            return RedirectToAction("Index", "Sensor", new { id = idPro });
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

    }
}
