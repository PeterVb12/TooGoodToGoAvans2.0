using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TooGoodToGoAvans.Domain.Models;
using TooGoodToGoAvans.UI.Models;

namespace TooGoodToGoAvans.UI.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string Name { get; set; } = null!;
        [EmailAddress]
        public string? EmailAddress { get; set; } = null!;
        [Required]
        [UIHint("Password")]
        [PasswordPropertyText]
        public string Password { get; set; } = null!;
        [Required]
        [UIHint("Password Validation")]
        [PasswordPropertyText]
        public string PasswordConfirmation { get; set; } = null!;
        public string? ReturnUrl { get; set; } = "/";

        public required UserRole Role { get; set; }
    }
}
