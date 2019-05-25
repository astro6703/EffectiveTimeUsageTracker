using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EffectiveTimeUsageTracker.ViewModels
{
    public class ObjectiveCreateModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int WeeklyTimeGoal { get; set; }

        [Required]
        public int TotalWeeks { get; set; }
    }
}
