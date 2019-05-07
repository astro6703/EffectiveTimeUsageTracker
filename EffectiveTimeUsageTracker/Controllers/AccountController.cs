using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EffectiveTimeUsageTracker.Models;
using EffectiveTimeUsageTracker.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EffectiveTimeUsageTracker.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager   = userManager ?? throw new ArgumentNullException($"{nameof(userManager)} was null");
            _signInManager = signInManager ?? throw new ArgumentNullException($"{nameof(signInManager)} was null");
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginModel loginModel, string returnUrl)
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
                        return Redirect(returnUrl ?? "/");
                }

                ModelState.AddModelError(nameof(loginModel.Email), "Invalid email or password");
            }

            return View(loginModel);
        }
    }
}