using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Bson;

namespace ObjectiveTimeTracker.Objectives
{
    public class UserObjectivesRepository : IUserObjectivesRepository
    {
        private readonly IMongoClient _client;

        public UserObjectivesRepository(IMongoClient client)
            => _client = client ?? throw new ArgumentNullException("Mongo client was null");

        public async Task<UserObjectives> GetUserObjectivesAsync(string userId)
        {
            if (userId == null)
                throw new ArgumentNullException($"{nameof(userId)} string instance was null");

            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException($"{nameof(userId)} string was empty or whitespace");

            var database = _client.GetDatabase("TomatoTimeTracker");
            var collection = database.GetCollection<UserObjectives>("UserObjectives");

            return await collection
                         .AsQueryable()
                         .Where(x => x.UserId == userId)
                         .FirstOrDefaultAsync();
        }

        public async Task SaveUserObjectivesAsync(UserObjectives userObjectives)
        {
            if (userObjectives == null)
                throw new ArgumentNullException($"{nameof(userObjectives)} instance was null");

            if (userObjectives.UserId == null)
                throw new ArgumentException($"{nameof(userObjectives)} {nameof(userObjectives.UserId)} property was null");

            if (string.IsNullOrWhiteSpace(userObjectives.UserId))
                throw new ArgumentException($"{nameof(userObjectives)} {nameof(userObjectives.UserId)} property was empty or whitespace");

            var database = _client.GetDatabase("TomatoTimeTracker");
            var collection = database.GetCollection<UserObjectives>("UserObjectives");

            if (await GetUserObjectivesAsync(userObjectives.UserId) != null)
                throw new InvalidOperationException("Instance of UserObjectives for this user already exists");

            await collection.InsertOneAsync(userObjectives);
        }

        public async Task UpdateUserObjectivesAsync(UserObjectives newObjectives)
        {
            if (newObjectives == null)
                throw new ArgumentNullException($"{nameof(newObjectives)} instance was null");

            if (string.IsNullOrWhiteSpace(newObjectives.UserId))
                throw new ArgumentException($"{nameof(newObjectives)} {nameof(newObjectives.UserId)} property was empty or whitespace");

            var database = _client.GetDatabase("TomatoTimeTracker");
            var collection = database.GetCollection<UserObjectives>("UserObjectives");

            var oldObjective = await GetUserObjectivesAsync(newObjectives.UserId);
            if (oldObjective == null)
                throw new InvalidOperationException($"{nameof(newObjectives)} instance to update is missing");

            newObjectives.Id = oldObjective.Id;
            var filter = new BsonDocument("_id", newObjectives.Id);
            await collection.ReplaceOneAsync(filter, newObjectives);
        }
    }
}