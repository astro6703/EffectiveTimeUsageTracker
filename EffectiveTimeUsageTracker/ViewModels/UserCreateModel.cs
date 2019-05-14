using System.ComponentModel.DataAnnotations;

namespace EffectiveTimeUsageTracker.ViewModels
{
    public class UserCreateModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}