using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace EffectiveTimeUsageTracker.Models.Objectives
{
    public class Objective
    {
        [Required]
        [BsonElement("name")]
        public string Name { get; set; }

        [Required]
        [BsonElement("timegoal")]
        public int WeeklyTimeGoal { get; set; }

        [Required]
        [BsonElement("totalweeks")]
        public int TotalWeeks { get; set; }

        [BsonElement("timespent")]
        public TimeSpan TimeSpent { get; set; }

        [BsonElement("timespenttoday")]
        public TimeSpan TimeSpentToday { get; set; }

        [BsonElement("todaydate")]
        public DateTime LastDate { get; set; }

        [BsonElement("startdate")]
        public DateTime StartDate { get; set; }

        [BsonIgnore]
        public int WeeksLeft
            => (int)Math.Floor((DateTime.Now - StartDate).TotalDays) / 7;

        public TimeSpan TimeLeftToday
        {
            get
            {
                return new TimeSpan(hours: (int)Math.Floor(TotalTimeLeft().TotalHours / DaysLeft()),
                                    minutes: 0,
                                    seconds: 0) - TimeSpentToday;

                TimeSpan TotalTimeLeft() => new TimeSpan(hours: WeeklyTimeGoal * WeeksLeft(), minutes: 0, seconds: 0) - TimeSpent;

                double DaysLeft() => (StartDate.AddHours(TotalWeeks * WeeklyTimeGoal) - DateTime.Now).TotalDays;

                int WeeksLeft() => (int)((StartDate.AddHours(TotalWeeks * WeeklyTimeGoal) - DateTime.Now).TotalDays / 7);
            }
        }

        public void Spend(TimeSpan time)
        {
            if (LastDate == DateTime.Now.Date)
            {
                TimeSpentToday += time;
                TimeSpent += time;
            }
            else
            {
                LastDate = DateTime.Now.Date;
                TimeSpentToday = time;
                TimeSpent += time;
            }
        }
    }
}