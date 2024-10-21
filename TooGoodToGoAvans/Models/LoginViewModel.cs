using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using TooGoodToGoAvans.UI.Models;

namespace TooGoodToGoAvans.UI.Models
{
    public class LoginViewModel
    {
        [Required]
        public string Name { get; set; } = null!;
        [EmailAddress]
        public string? EmailAddress { get; set; } = null!;
        [Required]
        [UIHint("Password")]
        [PasswordPropertyText]
        public string Password { get; set; } = null!;
        public string? ReturnUrl { get; set; } = "/";
    }
}
