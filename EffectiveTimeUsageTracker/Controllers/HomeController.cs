using System;
using EffectiveTimeUsageTracker.Models;
using EffectiveTimeUsageTracker.Models.Objectives;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EffectiveTimeUsageTracker.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserObjectivesRepository _repository;

        public HomeController(UserManager<ApplicationUser> userManager, IUserObjectivesRepository repository)
        {
            _userManager = userManager ?? throw new ArgumentNullException($"{nameof(userManager)} was null");
            _repository  = repository ?? throw new ArgumentNullException($"{nameof(repository)} was null");
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
    }
}