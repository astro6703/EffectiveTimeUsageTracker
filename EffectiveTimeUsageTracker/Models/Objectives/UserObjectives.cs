using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace EffectiveTimeUsageTracker.Models.Objectives
{
    public class UserObjectives
    {
        [BsonId]
        public BsonObjectId Id { get; set; }

        [BsonRequired]
        public string Username { get; set; }

        public string UserId { get; set; }

        public IEnumerable<Objective> Objectives { get; set; }
    }
}