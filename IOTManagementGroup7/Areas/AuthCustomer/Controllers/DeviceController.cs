using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IOTManagementGroup7.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost]
        public IActionResult TurnOnOff(string? id)
        {
            var obj = _unitOfWork.Device.GetFirstOrDefault(x => x.Id == id);
            if (obj.PowerStatus == 1)
            {
                obj.PowerStatus = 0;
                _unitOfWork.Device.Update(obj);
                _unitOfWork.Save();
                return Json(new { success = false, message = "Thiết bị " + obj.Name + " đã tắt!" });
            }
            obj.PowerStatus = 1;
            _unitOfWork.Device.Update(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Thiết bị " + obj.Name + " đã kích hoạt!" });
        }

    }
}
