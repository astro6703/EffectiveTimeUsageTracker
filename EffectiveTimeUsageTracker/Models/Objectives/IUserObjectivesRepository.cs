using System.Threading.Tasks;

namespace EffectiveTimeUsageTracker.Models.Objectives
{
    public interface IUserObjectivesRepository
    {
        Task SaveUserObjectivesAsync(UserObjectives userObjectives);

        Task UpdateUserObjectivesAsync(UserObjectives newObjectives);

        Task<UserObjectives> GetUserObjectivesAsync(string username);
    }
}