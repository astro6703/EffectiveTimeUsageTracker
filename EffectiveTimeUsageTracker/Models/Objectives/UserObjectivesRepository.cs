using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EffectiveTimeUsageTracker.Models.Objectives
{
    public class UserObjectivesRepository : IUserObjectivesRepository
    {
        private readonly IMongoClient _client;

        public UserObjectivesRepository(IMongoClient client)
            => _client = client ?? throw new ArgumentNullException("Mongo client was null");

        public async Task<UserObjectives> GetUserObjectivesAsync(string username)
        {
            if (username == null)
                throw new ArgumentNullException("Username string instance was null");

            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username string was empty or whitespace");

            var database = _client.GetDatabase("TomatoTimeTracker");
            var collection = database.GetCollection<UserObjectives>("UserObjectives");

            return await collection
                         .AsQueryable()
                         .Where(x => x.Username == username)
                         .FirstOrDefaultAsync();
        }

        public async Task SaveUserObjectivesAsync(UserObjectives userObjectives)
        {
            if (userObjectives == null)
                throw new ArgumentNullException("UserObjectives instance was null");

            if (userObjectives.Username == null)
                throw new ArgumentException("UserObjectives username property was null");

            if (string.IsNullOrWhiteSpace(userObjectives.Username))
                throw new ArgumentException("UserObjectives username property was empty or whitespace");

            var database = _client.GetDatabase("TomatoTimeTracker");
            var collection = database.GetCollection<UserObjectives>("UserObjectives");

            if (await GetUserObjectivesAsync(userObjectives.Username) != null)
                throw new InvalidOperationException("Instance of UserObjectives for this user already exists");

            await collection.InsertOneAsync(userObjectives);
        }

        public async Task UpdateUserObjectivesAsync(UserObjectives newObjectives)
        {
            if (newObjectives == null)
                throw new ArgumentNullException("UserObjectives instance was null");

            if (newObjectives.Username == null)
                throw new ArgumentException("UserObjectives username property was null");

            if (string.IsNullOrWhiteSpace(newObjectives.Username))
                throw new ArgumentException("UserObjectives username property was empty or whitespace");

            var database = _client.GetDatabase("TomatoTimeTracker");
            var collection = database.GetCollection<UserObjectives>("UserObjectives");

            var oldObjective = await GetUserObjectivesAsync(newObjectives.Username);
            if (oldObjective == null)
                throw new InvalidOperationException("UserObjective instance to update is missing");

            newObjectives.Id = oldObjective.Id;
            var filter = Builders<BsonDocument>.Filter.Eq("username", newObjectives.Username).ToBsonDocument();
            await collection.ReplaceOneAsync(filter, newObjectives);
        }
    }
}