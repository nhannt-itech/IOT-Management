using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using IOTManagementGroup7.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using IOTManagementGroup7.DataAccess.Repository.IRepository;
using IOTManagementGroup7.Utility;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace IOTManagementGroup7.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager,
            IUnitOfWork unitOfWork,
            IWebHostEnvironment hostEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;

            _emailSender = emailSender;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }


        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
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
            public string ImageUrl { get; set; }
            public string Role { get; set; }
            public IEnumerable<SelectListItem> RoleList { get; set; }

        }
        


        public async Task OnGetAsync(string returnUrl = null)
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
            Input = new InputModel()
            {
                RoleList = _roleManager.Roles.Select(i => i.Name).Select(x => new SelectListItem
                {
                    Text = x,
                    Value = x
                })
            };
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    Name = Input.Name,
                    PhoneNumber = Input.PhoneNumber,
                    Address = Input.Address,
                    ImageUrl = Input.ImageUrl,
                    Role = Input.Role
                };
                string webRootPath = _hostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webRootPath, @"asset\img\users\");
                    var extenstion = Path.GetExtension(files[0].FileName);


                    using (var filesStreams = new FileStream(Path.Combine(uploads, fileName + extenstion), FileMode.Create))
                    {
                        files[0].CopyTo(filesStreams);
                    }
                    user.ImageUrl = @"\asset\img\users\" + fileName + extenstion;
                }
                var result = await _userManager.CreateAsync(user, Input.Password);
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
                    if (!await _roleManager.RoleExistsAsync(SD.Role_Manager))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(SD.Role_Manager));
                    }

                    if (user.Role == null)
                    {
                        await _userManager.AddToRoleAsync(user, SD.Role_Manager);
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, user.Role);
                    }
                    //------------------------------Send Email-----------------------------------
                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    //var callbackUrl = Url.Page(
                    //    "/Account/ConfirmEmail",
                    //    pageHandler: null,
                    //    values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                    //    protocol: Request.Scheme);

                    //await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                    //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
                    //-------------------------------Send Email End-------------------------------
                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        if (user.Role == null)
                        {
                            await _signInManager.SignInAsync(user, isPersistent: false);
                            //return LocalRedirect(returnUrl);
                        }
                        else
                        {
                            // admin is registering a new user
                            return RedirectToAction("Index", "ApplicationUser", new { Area = "Admin" });
                        }
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
