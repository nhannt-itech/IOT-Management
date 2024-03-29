﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IOTManagementGroup7.DataAccess.Repository.IRepository;
using IOTManagementGroup7.Models;
using IOTManagementGroup7.Models.ViewModels;
using IOTManagementGroup7.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace IOTManagementGroup7.Areas.AuthCustomer.Controllers
{
    [Area("AuthCustomer")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Auth_Customer)]
    public class SensorController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;
        private static string ProId;
        public SensorController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult Index(string? id)
        {
            //id này là id của Phòng
            SensorHomeVM sensorHomeVM = new SensorHomeVM();
            sensorHomeVM.Sensors = _unitOfWork.Sensor.GetAll(x => x.ProjectId == id, includeProperties: "Project");
            sensorHomeVM.Project = _unitOfWork.Project.Get(id);
            ProId = id;
            return View(sensorHomeVM);
        }

        public IActionResult Upsert(string? id)
        {
            ViewBag.projectId = ProId;
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            Sensor sensor = new Sensor();

            sensor.ProjectId = ProId;
            // sensor.Project.ApplicationUser = _unitOfWork.ApplicationUser.Get(claim.Value);

            if (id == null)
            {
                //create new product
                return View(sensor);
            }
            //this is for edit
            sensor = _unitOfWork.Sensor.Get(id);

            if (sensor == null)
            {
                return NotFound();
            }

            return View(sensor);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Sensor sensor)
        {

            if (sensor.Id == null)
            {
                //XỬ LÝ LẤY ID CAO NHẤT CỦA PROJECT
                if (_unitOfWork.Sensor.GetAll().Count() == 0)
                {
                    sensor.Id = "Se1";
                }
                else
                {
                    int maxId = _unitOfWork.Sensor.GetAll()
                        .Select(x => Convert.ToInt32(x.Id.Replace("Se", ""))).Max();
                    sensor.Id = "Se" + (maxId + 1).ToString();
                }
                _unitOfWork.Sensor.Add(sensor);
            }
            else
            {
                _unitOfWork.Sensor.Update(sensor);
            }

            _unitOfWork.Save();
            SensorHomeVM sensorHomeVM = new SensorHomeVM()
            {
                Sensors = _unitOfWork.Sensor.GetAll(includeProperties: "Project"),
                Project = _unitOfWork.Project.Get(sensor.ProjectId)
            };
            return RedirectToAction("Index", new { id = sensor.ProjectId });
        }

        [HttpDelete]
        public IActionResult Delete(string? id)
        {
            var obj = _unitOfWork.Sensor.Get(id);
            obj.Project = _unitOfWork.Project.Get(ProId);
            obj.Devices = _unitOfWork.Device.GetAll(x => x.SensorBoardId == id,
                                        includeProperties: "DeviceType");
            if (obj == null)
            {
                return Json(new { success = false, message = "Error When Delete!" });
            }
            foreach (var device in obj.Devices)
            {
                _unitOfWork.Device.Remove(device);
            }
            _unitOfWork.Sensor.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful!" });
        }
    }
}
