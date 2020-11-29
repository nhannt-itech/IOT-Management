using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using IOTManagementGroup7.DataAccess.Repository.IRepository;
using IOTManagementGroup7.Models;
using IOTManagementGroup7.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace IOTManagementGroup7.Areas.AuthCustomer.Controllers
{
    [Area("AuthCustomer")]
    public class ProjectController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProjectController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index(ProjectHomeVM projectHomeVM)
        {
            projectHomeVM.Projects = _unitOfWork.Project.GetAll(includeProperties:"ApplicationUser");
            return View(projectHomeVM);
        }

        [HttpGet]
        public IActionResult Upsert(string? id)
        {
            Project project = _unitOfWork.Project.GetFirstOrDefault(x => x.Id == id);
            if (project == null){
                project = new Project(){
                    Id = ""
                };
            }
            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Project project)
        {
            string webRootPath = _hostEnvironment.WebRootPath; 
            var files = HttpContext.Request.Form.Files; 
            if (files.Count > 0){
                string fileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(webRootPath, @"images\project");
                var extension = Path.GetExtension(files[0].FileName); 
                if (project.Image != null && !project.Image.Contains("basic")){
                    var imagePath = Path.Combine(webRootPath, project.Image.TrimStart('\\'));
                    if (System.IO.File.Exists(imagePath)){
                        System.IO.File.Delete(imagePath);
                    }
                }
                using (var filesStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create)){
                    files[0].CopyTo(filesStreams);
                }
                project.Image = @"\images\project\" + fileName + extension;
            }
            else{
                project.Image = @"\images\project\basic\" + "room" + ".png";
            }
            if (project.Id == null){
                if (_unitOfWork.Project.GetAll().Count() == 0){
                    project.Id = "Pr1";
                }
                else{
                    int maxId = _unitOfWork.Project.GetAll()
                        .Select(x => Convert.ToInt32(x.Id.Replace("Pr", ""))).Max();
                    project.Id = "Pr" + (maxId + 1).ToString();
                }
                _unitOfWork.Project.Add(project);
            }
            else{
                _unitOfWork.Project.Update(project);
            }
            _unitOfWork.Save();
            ProjectHomeVM projectHomeVM = new ProjectHomeVM(){
                Projects = _unitOfWork.Project.GetAll(includeProperties: "ApplicationUser")
            };
            return RedirectToAction("Index", projectHomeVM);
        }

        [HttpDelete]
        public IActionResult Delete(string? id)
        {
            var obj = _unitOfWork.Project.Get(id);
            if (obj == null){
                return Json(new { success = false, message = "Error When Delete!" });
            }
            _unitOfWork.Project.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful!" });
        }
    }
}
