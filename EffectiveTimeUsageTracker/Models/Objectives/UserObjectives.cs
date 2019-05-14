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
        [BsonElement("username")]
        public string Username { get; set; }

        [BsonElement("objectives")]
        public IEnumerable<Objective> Objectives { get; set; }
    }
}