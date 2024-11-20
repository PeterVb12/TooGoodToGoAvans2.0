using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TooGoodToGoAvans.Domain.Models;

namespace TooGoodToGoAvans.UI.Models
{
    public class RegisterViewModel : IValidatableObject
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; } = null!;

        [Required]
        [UIHint("Password")]
        [PasswordPropertyText]
        public string Password { get; set; } = null!;

        [Required]
        [UIHint("Password")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string PasswordConfirmation { get; set; } = null!;

        public UserRole Role { get; set; }
        public required List<Canteen> Canteens { get; set; }
        public Guid SelectedCanteenId { get; set; }
        public City City { get; set; }
        public string? ReturnUrl { get; set; } = "/";
        // Student fields
        public string? StudentId { get; set; }
        public DateTime? Birthdate { get; set; }
        public string? Phonenumber { get; set; }

        // StaffMember fields
        public string? StaffMemberId { get; set; }

        // Custom validation for required fields based on Role
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Role == UserRole.Student)
            {
                if (string.IsNullOrWhiteSpace(StudentId))
                    yield return new ValidationResult("Student ID is required.", new[] { nameof(StudentId) });
                if (!Birthdate.HasValue)
                    yield return new ValidationResult("Birthdate is required.", new[] { nameof(Birthdate) });
                if (string.IsNullOrWhiteSpace(Phonenumber))
                    yield return new ValidationResult("Phonenumber is required.", new[] { nameof(Phonenumber) });
            }
            else if (Role == UserRole.StaffMember)
            {
                if (string.IsNullOrWhiteSpace(StaffMemberId))
                    yield return new ValidationResult("Staff Member ID is required.", new[] { nameof(StaffMemberId) });
            }
        }
    }
}
