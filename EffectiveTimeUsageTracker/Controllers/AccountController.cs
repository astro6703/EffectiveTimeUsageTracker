using EffectiveTimeUsageTracker.Models;
using EffectiveTimeUsageTracker.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EffectiveTimeUsageTracker.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException($"{nameof(userManager)} was null");
            _signInManager = signInManager ?? throw new ArgumentNullException($"{nameof(signInManager)} was null");
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginModel loginModel)
        {
            if (loginModel == null) throw new ArgumentNullException($"{nameof(loginModel)} was null");

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(loginModel.Email);

                if (user != null)
                {
                    await _signInManager.SignOutAsync();
                    var result = await _signInManager.PasswordSignInAsync(user, loginModel.Password, true, false);

                    if (result.Succeeded)
                        return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(nameof(loginModel.Email), "Invalid email or password");
            }

            return View(loginModel);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create(UserCreateModel createModel)
        {
            if (createModel == null) throw new ArgumentNullException($"{nameof(createModel)} was null");

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = createModel.Name,
                    Email = createModel.Email
                };

                var result = await _userManager.CreateAsync(user, createModel.Password);

                if (result.Succeeded)
                {
                    await _signInManager.PasswordSignInAsync(user, createModel.Password, true, false);

                    return RedirectToAction("Index", "Home");
                }

                else
                {
                    foreach (var error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                }
            }

            return View(createModel);
        }

        [Authorize]
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}