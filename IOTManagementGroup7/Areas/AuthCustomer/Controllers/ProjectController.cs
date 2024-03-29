﻿using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IOTManagementGroup7.Areas.AuthCustomer.Controllers
{
    [Area("AuthCustomer")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Auth_Customer)]
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
            projectHomeVM.Projects = _unitOfWork.Project.GetAll(x => x.ApplicationUserId == _userManager.GetUserId(User)
                                                                , includeProperties: "ApplicationUser,CustomerUser");
            return View(projectHomeVM);
        }

        [HttpGet]
        public IActionResult Upsert(string? id)
        {
            Project project = _unitOfWork.Project.GetFirstOrDefault(x => x.Id == id);

            if (project == null)
            {
                project = new Project()
                {
                    Id = "",
                    CustomerList = _unitOfWork.ApplicationUser.GetAll(x => x.CreaterUserId == _userManager.GetUserId(User)).Select(i => new SelectListItem
                    {
                        Text = i.Email,
                        Value = i.Id.ToString()
                    })
                };
            }
            else
            {
                project.CustomerList = _unitOfWork.ApplicationUser.GetAll(x => x.CreaterUserId == _userManager.GetUserId(User)).Select(i => new SelectListItem
                {
                    Text = i.Email,
                    Value = i.Id.ToString()
                });
            }
            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Project project)
        {

            string webRootPath = _hostEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;
            if (files.Count > 0)
            {
                string fileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(webRootPath, @"images\project");
                var extension = Path.GetExtension(files[0].FileName);
                if (project.Image != null && !project.Image.Contains("basic"))
                {
                    var imagePath = Path.Combine(webRootPath, project.Image.TrimStart('\\'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }
                using (var filesStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                {
                    files[0].CopyTo(filesStreams);
                }
                project.Image = @"\images\project\" + fileName + extension;
            }
            else
            {
                project.Image = @"\images\project\basic\" + "room" + ".png";
            }

            if (project.CustomerUserId == null)
            {
                project.CustomerUserId = _userManager.GetUserId(User);
            }

            if (project.Id == null)
            {
                if (_unitOfWork.Project.GetAll().Count() == 0)
                {
                    project.Id = "Pr1";
                }
                else
                {
                    int maxId = _unitOfWork.Project.GetAll()
                        .Select(x => Convert.ToInt32(x.Id.Replace("Pr", ""))).Max();
                    project.Id = "Pr" + (maxId + 1).ToString();
                }

                _unitOfWork.Project.Add(project);
            }
            else
            {
                _unitOfWork.Project.Update(project);
            }
            _unitOfWork.Save();
            ProjectHomeVM projectHomeVM = new ProjectHomeVM()
            {
                Projects = _unitOfWork.Project.GetAll(x => x.ApplicationUserId == _userManager.GetUserId(User)
                                                                , includeProperties: "ApplicationUser,CustomerUser")
            };
            return RedirectToAction("Index", projectHomeVM);
        }

        [HttpDelete]
        public IActionResult Delete(string? id)
        {
            var obj = _unitOfWork.Project.Get(id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error When Delete!" });
            }
            _unitOfWork.Project.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful!" });
        }
    }
}
