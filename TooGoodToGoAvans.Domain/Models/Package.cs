using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TooGoodToGoAvans.Domain.Models
{
    public class Package
    {
        public Guid PackageId { get; set; }
        public string Name { get; set; }
        public DateTime DateAndTimePickup { get; set; }
        public DateTime DateAndTimeLastPickup { get; set; }
        public bool AgeRestricted { get; set; }
        public double Price { get; set; }
        public string MealType { get; set; }

        public Canteen? CanteenServedAt { get; set; }
        public Student? ReservedBy { get; set; } 
        public ICollection<Product>? Products { get; set; }
    }
}
