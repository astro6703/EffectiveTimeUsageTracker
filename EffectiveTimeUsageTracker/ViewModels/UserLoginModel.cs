using System.ComponentModel.DataAnnotations;

namespace EffectiveTimeUsageTracker.ViewModels
{
    public class UserLoginModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
