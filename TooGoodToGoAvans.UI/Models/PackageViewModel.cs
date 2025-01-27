using System.ComponentModel.DataAnnotations;
using TooGoodToGoAvans.Domain.Models;

namespace TooGoodToGoAvans.UI.Models
{
    public class PackageViewModel
    {
        [Key]
        public Guid PackageId { get; set; }

        [Required(ErrorMessage = "Package name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Pickup date and time is required")]
        [Display(Name = "Pickup Date and Time")]
        public DateTime DateAndTimePickup { get; set; }

        [Required(ErrorMessage = "Last pickup date and time is required")]
        [Display(Name = "Last Pickup Date and Time")]
        public DateTime DateAndTimeLastPickup { get; set; }

        [Display(Name = "Age Restricted")]
        public bool AgeRestricted { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 10000, ErrorMessage = "Price must be between 0.01 and 10000")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Meal type is required")]
        [StringLength(50, ErrorMessage = "Meal type cannot exceed 50 characters")]
        [Display(Name = "Meal Type")]
        public string MealType { get; set; }

        public Canteen? CanteenServedAt { get; set; }

        public List<SelectableProductViewModel> Products { get; set; } = new List<SelectableProductViewModel>();
    }
}
