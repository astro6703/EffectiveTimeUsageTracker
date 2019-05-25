using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace EffectiveTimeUsageTracker.Models.Objectives
{
    public class Objective
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int WeeklyTimeGoal { get; set; }

        [Required]
        public int TotalWeeks { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public TimeSpan TimeSpent { get; set; }

        public TimeSpan TimeSpentToday { get; set; }

        public DateTime LastDate { get; set; }

        public TimeSpan ProjectedForToday
        {
            get
            {
                var projectedTimeLeftFromTodaysNight = new TimeSpan(WeeklyTimeGoal * TotalWeeks, 0, 0) - TimeSpent + TimeSpentToday;
                var daysLeft = Math.Floor((StartDate.AddDays(TotalWeeks * 7) - StartDate).TotalDays);

                return projectedTimeLeftFromTodaysNight / daysLeft;
            }
        }

        public void Spend(TimeSpan time)
        {
            if (LastDate == DateTime.Now.Date.ToUniversalTime())
            {
                TimeSpentToday += time;
                TimeSpent += time;
            }
            else
            {
                LastDate = DateTime.Now.Date.ToUniversalTime();
                TimeSpentToday = time;
                TimeSpent += time;
            }
        }
    }
}