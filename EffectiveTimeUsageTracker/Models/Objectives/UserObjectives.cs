using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

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
