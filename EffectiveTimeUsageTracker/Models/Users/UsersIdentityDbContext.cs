using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace EffectiveTimeUsageTracker.Models
{
    public class UsersIdentityDbContext : IdentityDbContext<User>
    {
        public UsersIdentityDbContext(DbContextOptions<UsersIdentityDbContext> options) : base(options) { }
    }
}
