using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using IOTManagementGroup7.DataAccess.Repository.IRepository;
using IOTManagementGroup7.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace IOTManagementGroup7.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class DeviceTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public DeviceTypeController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }



        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Upsert(string? id )
        {
            DeviceType deviceType = _unitOfWork.DeviceType.GetFirstOrDefault(x => x.Id == id);
            if (deviceType == null)
            {
                deviceType = new DeviceType()
                {
                    Id = ""
                };
            }
            return View(deviceType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(DeviceType deviceType)
        {
            string webRootPath = _hostEnvironment.WebRootPath; //"C:\\Users\\MayXauGiaCao\\Desktop\\IOTManagementGroup7\\IOTManagementGroup7\\wwwroot"
            var files = HttpContext.Request.Form.Files; //Lấy file ở trong Form
            if (files.Count > 0)
            {
                string fileNameOn = Guid.NewGuid().ToString();
                string fileNameOff = Guid.NewGuid().ToString();
                var uploads = Path.Combine(webRootPath, @"images\deviceType");
                var extension = Path.GetExtension(files[0].FileName); // ".png"
                if (deviceType.OnImage != null && !deviceType.OnImage.Contains("basic"))
                {//Nếu như link ảnh của đối tượng khác null và không phải là basic thì xóa ảnh cũ
                    var imagePathOn = Path.Combine(webRootPath, deviceType.OnImage.TrimStart('\\'));
                    if (System.IO.File.Exists(imagePathOn))
                    {
                        System.IO.File.Delete(imagePathOn);
                    }
                }

                if (deviceType.OffImage != null && !deviceType.OffImage.Contains("basic"))
                {//Nếu như link ảnh của đối tượng khác null và không phải là basic thì xóa ảnh cũ
                    var imagePathOff = Path.Combine(webRootPath, deviceType.OffImage.TrimStart('\\'));
                    if (System.IO.File.Exists(imagePathOff))
                    {
                        System.IO.File.Delete(imagePathOff);
                    }
                }
                using (var filesStreams = new FileStream(Path.Combine(uploads, fileNameOn + extension), FileMode.Create))
                {
                    //uploads: "C:\\Users\\MayXauGiaCao\\Desktop\\IOTManagementGroup7\\IOTManagementGroup7\\wwwroot\\images\\project"
                    //filename: "6a9c2297-0ef2-4edf-938f-c19971ce8e26"
                    //extension: ".png"
                    files[0].CopyTo(filesStreams);
                }

                using (var filesStreams = new FileStream(Path.Combine(uploads, fileNameOff + extension), FileMode.Create))
                {
                    //uploads: "C:\\Users\\MayXauGiaCao\\Desktop\\IOTManagementGroup7\\IOTManagementGroup7\\wwwroot\\images\\project"
                    //filename: "6a9c2297-0ef2-4edf-938f-c19971ce8e26"
                    //extension: ".png"
                    files[1].CopyTo(filesStreams);
                }
                deviceType.OnImage = @"\images\deviceType\" + fileNameOn + extension;
                deviceType.OffImage = @"\images\deviceType\" + fileNameOff + extension;
            }
            else
            {//Nếu không có ảnh thì lấy ảnh cơ bản ^^
                deviceType.OnImage = @"\images\deviceType\basic\" + "FanOn" + ".gif";
                deviceType.OffImage = @"\images\deviceType\basic\" + "FanOff" + ".png";
            }
            if (deviceType.Id == null)
            {
                //XỬ LÝ LẤY ID CAO NHẤT CỦA DeviceType
                if (_unitOfWork.DeviceType.GetAll().Count() == 0)
                {
                    deviceType.Id = "D1";
                }
                else
                {
                    int maxId = _unitOfWork.DeviceType.GetAll()
                        .Select(x => Convert.ToInt32(x.Id.Replace("D", ""))).Max();
                    deviceType.Id = "D" + (maxId + 1).ToString();
                }
                _unitOfWork.DeviceType.Add(deviceType);
            }
            else
            {
                _unitOfWork.DeviceType.Update(deviceType);
            }
            _unitOfWork.Save();
            
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.DeviceType.GetAll();
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult Delete(string? id)
        {
            var obj = _unitOfWork.DeviceType.Get(id);
            string webRootPath = _hostEnvironment.WebRootPath;
            if (obj == null)
            {
                return Json(new { success = false, message = "Error When Delete!" });
            }
            if (obj.OnImage != null && !obj.OnImage.Contains("basic"))
            {//Nếu như link ảnh của đối tượng khác null và không phải là basic thì xóa ảnh cũ
                var imagePathOn = Path.Combine(webRootPath, obj.OnImage.TrimStart('\\'));
                if (System.IO.File.Exists(imagePathOn))
                {
                    System.IO.File.Delete(imagePathOn);
                }
            }

            if (obj.OffImage != null && !obj.OffImage.Contains("basic"))
            {//Nếu như link ảnh của đối tượng khác null và không phải là basic thì xóa ảnh cũ
                var imagePathOff = Path.Combine(webRootPath, obj.OffImage.TrimStart('\\'));
                if (System.IO.File.Exists(imagePathOff))
                {
                    System.IO.File.Delete(imagePathOff);
                }
            }
            _unitOfWork.DeviceType.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful!" });
        }
    }
}
