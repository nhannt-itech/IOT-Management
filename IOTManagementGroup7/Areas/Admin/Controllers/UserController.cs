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

namespace IOTManagementGroup7.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = SD.Role_Admin)]
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
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            [Remote("isExists", "User", HttpMethod = "POST", ErrorMessage = "User name is already taken")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [Remote("checkPassword", "User", HttpMethod = "POST", ErrorMessage = "Must contains at least one digit and at least one lower case or upper case")]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [Display(Name = "Name")]
            public string Name { get; set; }

            [Display(Name = "Phone")]
            public string PhoneNumber { get; set; }

            [Display(Name = "Address")]
            public string Address { get; set; }
            public string Role { get; set; }
        }
        public class LoginPopupModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
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

        [HttpGet]
        public async Task<ActionResult> Register(string returnUrl = null)
        {
            if (!await _roleManager.RoleExistsAsync(SD.Role_Admin))
            {
                await _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin));
            }
            if (!await _roleManager.RoleExistsAsync(SD.Role_Customer))
            {
                await _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer));
            }
            if (!await _roleManager.RoleExistsAsync(SD.Role_Manager))
            {
                await _roleManager.CreateAsync(new IdentityRole(SD.Role_Manager));
            }
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            return PartialView("_Register", RegisterPopup);
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

                    if (!await _roleManager.RoleExistsAsync(SD.Role_Admin))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin));
                    }
                    if (!await _roleManager.RoleExistsAsync(SD.Role_Customer))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer));
                    }

                    if (user.Role == null)
                    {
                        await _userManager.AddToRoleAsync(user, SD.Role_Customer);
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, user.Role);
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
                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = registerPopup.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        if (user.Role == null)
                        {
                            await _signInManager.SignInAsync(user, isPersistent: false);
                            return LocalRedirect(returnUrl);
                        }
                        else
                        {
                            // admin is registering a new user
                            return RedirectToAction("Index", "User", new { Area = "Admin" });
                        }
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return PartialView("_Register", registerPopup);
        }

        [HttpGet]
        public async Task<ActionResult> Login(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }
            returnUrl = returnUrl ?? Url.Content("~/");
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            ReturnUrl = returnUrl;
            return PartialView("_Login", LoginPopup);
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
                    _logger.LogInformation("User logged in.");
                    if (result.IsLockedOut)
                    {
                        _logger.LogWarning("User account locked out.");
                        return Redirect("~/Identity/Account/Lockout");
                    }
                    return LocalRedirect(returnUrl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Redirect("~/Identity/Account/Login");
                    //return PartialView("_Login", loginPopup);
                }
            }
            return Redirect("~/Identity/Account/Login");
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
            var obj = _db.ApplicationUsers.FirstOrDefault(x => x.Id == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while Lock/Unlock user!" });
            }
            if (obj.LockoutEnd != null && obj.LockoutEnd > DateTime.Now)
            {
                obj.LockoutEnd = DateTime.Now;
            }
            else
            {
                obj.LockoutEnd = DateTime.Now.AddYears(1000);
            }
            _db.SaveChanges();
            return Json(new { success = true, message = "Lock/Unlock user successfully!" });
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
        [HttpPost]
        public JsonResult isExists(string Email)
        {
            int isExist = _db.ApplicationUsers.Count(x => x.Email == Email);
            if (isExist == 0)
                return Json(true);
            else
                return Json(false);
        }

        [HttpPost]
        public JsonResult checkPassword(string Password)
        {
            string MatchEmailPattern = @"(?=^[^\s]{6,}$)(?=.*\d)(?=.*[a-zA-Z])";
            return Json(Regex.IsMatch(Password, MatchEmailPattern));
        }

        [HttpPost]
        public async Task<ActionResult> loginUser(string Email, string Password)
        {
            var result = await _signInManager.PasswordSignInAsync(Email, Password, true, lockoutOnFailure: false);
            await _signInManager.SignOutAsync();
            return Json(result.Succeeded);
        }
        #endregion
    }
}
