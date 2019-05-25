using System;

namespace ObjectiveTimeTracker.Objectives
{
    public class Objective
    {
        public string Name { get; set; }
        public int WeeklyTimeGoal { get; set; }
        public int TotalWeeks { get; set; }
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