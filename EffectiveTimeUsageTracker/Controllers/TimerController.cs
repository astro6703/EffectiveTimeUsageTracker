using System;
using System.Threading.Tasks;
using System.Linq;
using EffectiveTimeUsageTracker.Models.Objectives;
using EffectiveTimeUsageTracker.MyExtensionMethods;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EffectiveTimeUsageTracker.Models;
using Microsoft.AspNetCore.Identity;

namespace EffectiveTimeUsageTracker.Controllers
{
    public class TimerController : Controller
    {
        private readonly IUserObjectivesRepository _userObjectivesRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly StopwatchRepository _stopwatchRepository;

        public TimerController(
            IUserObjectivesRepository repository, 
            UserManager<ApplicationUser> userManager,
            StopwatchRepository stopwatchRepository)
        {
            _userObjectivesRepository = repository ?? throw new ArgumentNullException($"{nameof(repository)} was null");
            _userManager = userManager ?? throw new ArgumentNullException($"{nameof(userManager)}");
            _stopwatchRepository = stopwatchRepository ?? throw new ArgumentNullException($"{nameof(stopwatchRepository)}");                
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var objectives = await _userObjectivesRepository.GetUserObjectivesAsync(User.Identity.Name);
            
            return View(objectives);
        }

        [Authorize]
        public async Task<IActionResult> StartWatch(string name)
        {
            var objectives = await _userObjectivesRepository.GetUserObjectivesAsync(User.Identity.Name);
            var objectiveStopwatch = _stopwatchRepository.GetUserStopwatch(User.Identity.Name);

            if (!objectives.Objectives.Where(x => x.Name == name).Any())
                return BadRequest("No objective with such name exists");

            var currentObjectiveName = objectiveStopwatch.ObjectiveName;

            if (currentObjectiveName != null)
            {
                var currentObjective = objectives.Objectives.Where(x => x.Name == currentObjectiveName).FirstOrDefault();

                currentObjective.Spend(objectiveStopwatch.Elapsed);
                objectives.Objectives = objectives.Objectives.UpdateObjectiveStatus(currentObjective).ToArray();
                await _userObjectivesRepository.UpdateUserObjectivesAsync(objectives);
            }

            objectiveStopwatch.SetObjective(name);
            objectiveStopwatch.Start();

            return RedirectToAction("Index", "Timer");
        }

        [Authorize]
        public async Task<IActionResult> StopWatch(string name)
        {
            var objectives = await _userObjectivesRepository.GetUserObjectivesAsync(User.Identity.Name);
            var objectiveStopwatch = _stopwatchRepository.GetUserStopwatch(User.Identity.Name);

            if (!objectives.Objectives.Where(x => x.Name == name).Any())
                return BadRequest("No objective with such name exists");

            var currentObjectiveName = objectiveStopwatch.ObjectiveName;

            if (currentObjectiveName == null)
                return BadRequest("Timer is not set up");

            var currentObjective = objectives.Objectives.Where(x => x.Name == currentObjectiveName).FirstOrDefault();

            currentObjective.Spend(objectiveStopwatch.Elapsed);
            objectives.Objectives = objectives.Objectives.UpdateObjectiveStatus(currentObjective).ToArray();
            await _userObjectivesRepository.UpdateUserObjectivesAsync(objectives);

            objectiveStopwatch.Stop();

            return RedirectToAction("Index", "Timer");
        }

        [Authorize]
        public IActionResult CreateObjective()
            => View();

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateObjective(Objective objective)
        {
            if (objective == null) throw new ArgumentNullException("Objective instance was null");
            
            if (ModelState.IsValid)
            {
                var currentUserObjectives = await _userObjectivesRepository.GetUserObjectivesAsync(User.Identity.Name);

                if (currentUserObjectives == null)
                    ModelState.AddModelError("", "Cannot find userObjectives for this user");
                else if (currentUserObjectives.Objectives.Where(x => x.Name == objective.Name).Any())
                    ModelState.AddModelError("", "Objective with this name already exists");
                else
                {
                    currentUserObjectives.Objectives = currentUserObjectives.Objectives.AddOneObjective(objective).ToArray();

                    await _userObjectivesRepository.UpdateUserObjectivesAsync(currentUserObjectives);
                }
            }

            return RedirectToAction("Index", "Timer");
        }
    }
}