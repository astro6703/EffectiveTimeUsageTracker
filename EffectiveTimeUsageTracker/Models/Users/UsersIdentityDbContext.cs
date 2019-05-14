using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EffectiveTimeUsageTracker.Models
{
    public class UsersIdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public UsersIdentityDbContext(DbContextOptions<UsersIdentityDbContext> options) : base(options) { }
    }
}