using System;
using System.Threading.Tasks;
using HelloNetcore.Models;
using IdentityServer4.Services;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HelloNetcore.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signManager;
        private readonly IIdentityServerInteractionService _identityService;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signManager, IIdentityServerInteractionService identityService)
        {
            _userManager = userManager;
            _signManager = signManager;
            _identityService = identityService;
        }

        //private readonly TestUserStore _userStore;

        //public AccountController(TestUserStore userStore)
        //{
        //    _userStore = userStore;
        //}

        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerData, string returnUrl = null)
        {
            if (!ModelState.IsValid) return View();

            ViewData["ReturnUrl"] = returnUrl;
            var identityUser = new ApplicationUser
            {
                Email = registerData.Email,
                UserName = registerData.Email,
                NormalizedUserName = registerData.Email
            };

            var identityResult = await _userManager.CreateAsync(identityUser, registerData.Password);
            if (identityResult.Succeeded)
            {
                await _signManager.SignInAsync(identityUser, new AuthenticationProperties() { IsPersistent = true });

                return RedirectToLocal(returnUrl);
            }

            AddErrors(identityResult);
            return View();
        }

        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        public IActionResult SingnOut()
        {
            return View();
        }

        // GET: /<controller>/
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel viewModel, string returnUrl = null)
        {
            if (!ModelState.IsValid) return View();

            ViewData["ReturnUrl"] = returnUrl;
            var user = await _userManager.FindByEmailAsync(viewModel.Email);
            if (user == null)
            {
                ModelState.AddModelError(nameof(LoginViewModel.Email), "Email not exists");
            }

            if (await _userManager.CheckPasswordAsync(user, viewModel.Password))
            {
                AuthenticationProperties props = null;
                if (viewModel.RememberMe)
                    props = new AuthenticationProperties()
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                    };

                await _signManager.SignInAsync(user, props);
                if (_identityService.IsValidReturnUrl(returnUrl))
                    return RedirectToLocal(returnUrl);
                return RedirectToAction("Index", "Home");
            }


            ModelState.AddModelError(nameof(LoginViewModel.Password), "Password is Wrong");
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
            //await HttpContext.SignOutAsync();
            //return View();
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}
