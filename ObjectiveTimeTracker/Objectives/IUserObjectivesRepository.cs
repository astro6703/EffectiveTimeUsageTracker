using System.Threading.Tasks;

namespace ObjectiveTimeTracker.Objectives
{
    public interface IUserObjectivesRepository
    {
        Task SaveUserObjectivesAsync(UserObjectives userObjectives);

        Task UpdateUserObjectivesAsync(UserObjectives newObjectives);

        Task<UserObjectives> GetUserObjectivesAsync(string userId);
    }
}