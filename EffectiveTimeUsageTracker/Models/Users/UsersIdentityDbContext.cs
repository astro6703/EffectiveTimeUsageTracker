using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace EffectiveTimeUsageTracker.Models
{
    public class UsersIdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public UsersIdentityDbContext(DbContextOptions<UsersIdentityDbContext> options) : base(options) { }
    }
}
