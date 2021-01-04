using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using IOTManagementGroup7.Areas.Identity.Pages.Account;
using IOTManagementGroup7.DataAccess.Data;
using IOTManagementGroup7.DataAccess.Repository.IRepository;
using IOTManagementGroup7.Models;
using IOTManagementGroup7.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IOTManagementGroup7.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        public UserController(
            ApplicationDbContext db,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager,
            IUnitOfWork unitOfWork)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;

            _emailSender = emailSender;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
        }
        public LoginPopupModel LoginPopup { get; set; }
        public RegisterPopupModel RegisterPopup { get; set; }
        public string ReturnUrl { get; set; }
        public string ErrorMessage { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        public class RegisterPopupModel
        {
            [Required(ErrorMessage ="Bạn phải nhập Email.")]
            [EmailAddress(ErrorMessage = "Email không hợp lệ")]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Bạn phải nhập mật khẩu.")]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [Remote("checkPassword", "User", HttpMethod = "POST", ErrorMessage = "Must contains at least one digit and at least one lower case or upper case")]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required(ErrorMessage = "Bạn phải nhập tên.")]
            [Display(Name = "Name")]
            public string Name { get; set; }

            [Required(ErrorMessage = "Bạn phải nhập số điện thoại.")]
            [Display(Name = "Phone")]
            public string PhoneNumber { get; set; }

            [Display(Name = "Address")]
            public string Address { get; set; }
            public string Role { get; set; }
        }
        public class LoginPopupModel
        {
            [Required(ErrorMessage = "Bạn phải nhập Email")]
            [EmailAddress(ErrorMessage = "Email không hợp lệ")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Bạn phải nhập mật khẩu")]
            [DataType(DataType.Password)]
            [Remote("loginUser", "User", HttpMethod = "POST", ErrorMessage = "Your Email or Password is Incorrect", AdditionalFields = "Email")]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Update(string? id)
        {
            ApplicationUser applicationUser = new ApplicationUser();
            applicationUser = _db.ApplicationUsers.FirstOrDefault(x => x.Id == id);
            return View(applicationUser);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                var obj = _db.ApplicationUsers.FirstOrDefault(x => x.Id == applicationUser.Id);
                obj.Name = applicationUser.Name;
                obj.PhoneNumber = applicationUser.PhoneNumber;
                _db.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<ActionResult> Register(RegisterPopupModel registerPopup, string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    UserName = registerPopup.Email,
                    Email = registerPopup.Email,
                    Name = registerPopup.Name,
                    PhoneNumber = registerPopup.PhoneNumber,
                    Address = registerPopup.Address,
                    Role = registerPopup.Role
                };
                var result = await _userManager.CreateAsync(user, registerPopup.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    if (_unitOfWork.ApplicationUser.GetAll().Count() == 1)
                    {
                        if (!await _roleManager.RoleExistsAsync(SD.Role_Admin))
                        {
                            await _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin));
                        }
                        await _userManager.AddToRoleAsync(user, SD.Role_Admin);
                    }
                    else
                    {
                        if (!await _roleManager.RoleExistsAsync(SD.Role_Auth_Customer))
                        {
                            await _roleManager.CreateAsync(new IdentityRole(SD.Role_Auth_Customer));
                        }
                        await _userManager.AddToRoleAsync(user, SD.Role_Auth_Customer);
                    }
                    
                    //------------------------------Send Email-----------------------------------
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(registerPopup.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
                    //-------------------------------Send Email End-------------------------------

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<ActionResult> Login(LoginPopupModel loginPopup, string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginPopup.Email, loginPopup.Password, loginPopup.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return LocalRedirect(returnUrl);
                }
                else if (result.IsLockedOut)
                {
                    return Redirect("~/Identity/Account/Lockout");
                }
            }
            return NotFound();
        }

        #region API_Calls
        public IActionResult GetAll()
        {
            var listUser = _db.ApplicationUsers.ToList();
            var userRoles = _db.UserRoles.ToList();
            var listRole = _db.Roles.ToList();

            foreach (var user in listUser)
            {
                var idUserRole = userRoles.FirstOrDefault(x => x.UserId == user.Id).RoleId;
                user.Role = listRole.FirstOrDefault(x => x.Id == idUserRole).Name;
            }
            return Json(new { data = listUser });
        }

        [HttpPost]
        public IActionResult LockUnlock([FromBody] string id) //Take notice this [FromBody]
        {
            var objFromDb = _db.ApplicationUsers.FirstOrDefault(u => u.Id == id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while Locking/Unclocking" });
            }
            if (objFromDb.LockoutEnd != null && objFromDb.LockoutEnd > DateTime.Now)
            {
                objFromDb.LockoutEnd = DateTime.Now;
                _db.SaveChanges();
                return Json(new { success = true, message = "Mở khóa " + objFromDb.Email });
            }
            else
            {
                objFromDb.LockoutEnd = DateTime.Now.AddYears(1000);
                _db.SaveChanges();
                return Json(new { success = false, message = "Khóa " + objFromDb.Email });
            }
        }

        [HttpDelete]
        public IActionResult Delete(string? id)
        {
            var userObj = _db.ApplicationUsers.FirstOrDefault(x => x.Id == id);
            var roleObj = _db.UserRoles.FirstOrDefault(x => x.UserId == id);
            if (userObj == null)
            {
                return Json(new { success = false, message = "Error When Delete!" });
            }
            _db.UserRoles.Remove(roleObj);
            _db.ApplicationUsers.Remove(userObj);
            _db.SaveChanges();
            return Json(new { success = true, message = "Delete successful!" });
        }
        #endregion

        #region ErrorValidationModel
        [AcceptVerbs("Get", "Post")]
        public JsonResult isEmailExists(string Email)
        {
            int isExist = _db.ApplicationUsers.Count(x => x.Email == Email);
            if (isExist == 0)
                return Json(true);
            else
                return Json(false);
        }

        [AcceptVerbs("Get", "Post")]
        public JsonResult isPasswordFine(string Password)
        {
            string MatchEmailPattern = @"(?=^[^\s]{6,}$)(?=.*\d)(?=.*[a-zA-Z])";
            return Json(Regex.IsMatch(Password, MatchEmailPattern));
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<ActionResult> isPasswordCorrect(string Email, string Password)
        {
            var user = _unitOfWork.ApplicationUser.GetFirstOrDefault(x => x.Email == Email);
            var result = await _signInManager.UserManager.CheckPasswordAsync(user, Password);
            return Json(result);
        }
        #endregion
    }
}
