using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IOTManagementGroup7.DataAccess.Repository.IRepository;
using IOTManagementGroup7.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IOTManagementGroup7.Areas.Customer.Controllers
{
    [Area("Customer")]
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
            projectHomeVM.Projects = _unitOfWork.Project.GetAll(x => x.CustomerUserId == _userManager.GetUserId(User)
                                                                , includeProperties: "ApplicationUser,CustomerUser");
            return View(projectHomeVM);
        }
    }
}
