﻿using System;
using System.Threading.Tasks;
using System.Linq;
using EffectiveTimeUsageTracker.MyExtensionMethods;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EffectiveTimeUsageTracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using ObjectiveTimeTracker.Objectives;
using ObjectiveTimeTracker.Stopwatches;
using EffectiveTimeUsageTracker.ViewModels;

namespace EffectiveTimeUsageTracker.Controllers
{
    public class TimerController : Controller
    {
        private readonly IUserObjectivesRepository _userObjectivesRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IStopwatchRepository _stopwatchRepository;

        private string UserID;
        private ObjectiveStopwatch UserStopwatch;

        public TimerController(
            IUserObjectivesRepository repository, 
            UserManager<ApplicationUser> userManager,
            IStopwatchRepository stopwatchRepository)
        {
            _userObjectivesRepository = repository ?? throw new ArgumentNullException($"{nameof(repository)} was null");
            _userManager = userManager ?? throw new ArgumentNullException($"{nameof(userManager)}");
            _stopwatchRepository = stopwatchRepository ?? throw new ArgumentNullException($"{nameof(stopwatchRepository)}");                
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            UserID = _userManager.GetUserId(User);
            UserStopwatch = _stopwatchRepository.GetUserStopwatch(UserID);
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var objectives = await _userObjectivesRepository.GetUserObjectivesAsync(_userManager.GetUserId(User));
            
            return View(objectives);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> StartWatch(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));

            UserStopwatch.Stop();

            var objectives = await _userObjectivesRepository.GetUserObjectivesAsync(_userManager.GetUserId(User));

            if (!objectives.Objectives.Where(x => x.Name == name).Any())
                throw new InvalidOperationException("No objective with such name exists");

            var currentObjectiveName = UserStopwatch.ObjectiveName;

            if (currentObjectiveName != null)
            {
                var currentObjective = objectives.Objectives.Where(x => x.Name == currentObjectiveName).FirstOrDefault();

                currentObjective.Spend(UserStopwatch.Elapsed);
                objectives.Objectives = objectives.Objectives.UpdateObjective(currentObjective).ToArray();
                await _userObjectivesRepository.UpdateUserObjectivesAsync(objectives);
            }

            UserStopwatch.SetObjective(name);
            UserStopwatch.Start();

            return NoContent();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> StopWatch()
        {
            UserStopwatch.Stop();

            var userObjectives = await _userObjectivesRepository.GetUserObjectivesAsync(_userManager.GetUserId(User));
            var currentObjectiveName = UserStopwatch.ObjectiveName;

            if (currentObjectiveName == null)
                return BadRequest("Timer is not set up");

            var currentObjective = userObjectives.Objectives.Where(x => x.Name == currentObjectiveName).FirstOrDefault();

            currentObjective.Spend(UserStopwatch.Elapsed);
            UserStopwatch.ResetObjective();
            userObjectives.Objectives = userObjectives.Objectives.UpdateObjective(currentObjective).ToArray();
            await _userObjectivesRepository.UpdateUserObjectivesAsync(userObjectives);

            return NoContent();
        }

        [Authorize]
        public IActionResult CreateObjective()
            => View();

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateObjective(ObjectiveCreateModel objectiveCreateModel)
        {
            if (objectiveCreateModel == null) throw new ArgumentNullException($"{nameof(objectiveCreateModel)} instance was null");
            
            if (ModelState.IsValid)
            {
                var currentUserObjectives = await _userObjectivesRepository.GetUserObjectivesAsync(_userManager.GetUserId(User));

                if (currentUserObjectives == null)
                    ModelState.AddModelError("", "Cannot find userObjectives for this user");
                else if (currentUserObjectives.Objectives.Where(x => x.Name == objectiveCreateModel.Name).Any())
                    ModelState.AddModelError("", "Objective with this name already exists");
                else
                {
                    var objective = new Objective
                    {
                        Name = objectiveCreateModel.Name,
                        TotalWeeks = objectiveCreateModel.TotalWeeks,
                        WeeklyTimeGoal = objectiveCreateModel.WeeklyTimeGoal,
                        StartDate = DateTime.Now.Date.ToUniversalTime(),
                        LastDate = DateTime.Now.Date.ToUniversalTime()
                    };

                    currentUserObjectives.Objectives = currentUserObjectives.Objectives.AddObjective(objective).ToArray();

                    await _userObjectivesRepository.UpdateUserObjectivesAsync(currentUserObjectives);

                    return RedirectToAction("Index", "Timer");
                }
            }

            return View(objectiveCreateModel);
        }

        public async Task<IActionResult> RemoveObjective(string name)
        {
            if (name == null) throw new ArgumentNullException($"{nameof(name)} was null");
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException($"{nameof(name)} was empty or whitespace");

            var userObjectives = await _userObjectivesRepository.GetUserObjectivesAsync(_userManager.GetUserId(User));
            var objective = userObjectives.Objectives.Where(x => x.Name == name).FirstOrDefault();

            userObjectives.Objectives = userObjectives.Objectives.RemoveObjective(objective);
            await _userObjectivesRepository.UpdateUserObjectivesAsync(userObjectives);

            UserStopwatch.ResetObjective();

            return RedirectToAction("Index", "Timer");
        }
    }
}