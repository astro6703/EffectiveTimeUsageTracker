using System;
using MongoDB.Bson.Serialization.Attributes;

namespace EffectiveTimeUsageTracker.Models.Objectives
{
    public class Objective
    {
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("description")]
        public string Discription { get; set; }

        [BsonElement("timegoal")]
        public TimeSpan TimeGoal { get; set; }

        [BsonElement("timespent")]
        public TimeSpan TimeSpent { get; set; }

        [BsonElement("weeks")]
        public int Weeks { get; set; }

        [BsonIgnore]
        public TimeSpan TimeLeft => TimeGoal.Subtract(TimeSpent);
    }
}
