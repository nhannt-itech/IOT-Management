using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IOTManagementGroup7.DataAccess.Repository.IRepository;
using IOTManagementGroup7.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IOTManagementGroup7.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public IndexModel(
            UserManager<IdentityUser> userManager,
             IUnitOfWork unitOfWork,
               IWebHostEnvironment hostEnvironment,
            SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {

            [Required(ErrorMessage = "Vui lòng nhập tên đầy đủ")]
            [Display(Name = "Name")]
            public string Name { get; set; }

            [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
            [Display(Name = "Phone")]
            public string PhoneNumber { get; set; }

            [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
            [Display(Name = "Address")]
            public string Address { get; set; }

            public string ImageUrl { get; set; }

        }

        private async Task LoadAsync(IdentityUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);

            var userId = await _userManager.GetUserIdAsync(user);

            var nd = _unitOfWork.ApplicationUser.Get(userId);
            nd.Id = userId;
            var name = nd.Name;
            var phoneNumber = nd.PhoneNumber;
            var address = nd.Address;
            var imgUrl = nd.ImageUrl;

            Username = userName;

            Input = new InputModel
            {
                Name = name,
                PhoneNumber = phoneNumber,
                Address = address,
                ImageUrl = imgUrl
            };

        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }
            var userId = await _userManager.GetUserIdAsync(user);

            var nd = _unitOfWork.ApplicationUser.Get(userId);
            nd.Id = userId;
            var name = nd.Name;
            var phoneNumber = nd.PhoneNumber;
            var address = nd.Address;

            if (Input.Name != name)
            {
                nd.Name = Input.Name;
            }
            if (Input.PhoneNumber != phoneNumber)
            {
                nd.PhoneNumber = Input.PhoneNumber;
            }
            if (Input.Address != address)
            {
                nd.Address = Input.Address;
            }

            string webRootPath = _hostEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;
            if (files.Count > 0)
            {
                string fileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(webRootPath, @"asset\img\users\");
                var extenstion = Path.GetExtension(files[0].FileName);

                if(nd.ImageUrl != null)
                {
                    //this is an edit and we need to remove old image
                    var imagePath = Path.Combine(webRootPath, nd.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }
              


                using (var filesStreams = new FileStream(Path.Combine(uploads, fileName + extenstion), FileMode.Create))
                {
                    files[0].CopyTo(filesStreams);
                }
                nd.ImageUrl = @"\asset\img\users\" + fileName + extenstion;
            }
           

            //_unitOfWork.ApplicationUser.Update(nd);
            //_unitOfWork.Save();
            await _userManager.UpdateAsync(nd);
            await _signInManager.RefreshSignInAsync(nd);

            StatusMessage = "Your profile has been updated";
            return RedirectToPage();

        }


    }
}

