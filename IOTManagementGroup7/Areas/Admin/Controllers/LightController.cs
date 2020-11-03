using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IOTManagementGroup7.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace IOTManagementGroup7.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LightController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public IActionResult Index()
        {
            return View();
        }
    }
}
