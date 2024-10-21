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

        public Package() { }

        public Package(Guid packageId, string name, DateTime dateAndTimePickup, DateTime dateAndTimeLastPickup, bool ageRestricted, double price, string mealType, Canteen? canteenServedAt, Student? reservedBy, ICollection<Product>? products)
        {
            PackageId = packageId;
            Name = name;
            DateAndTimePickup = dateAndTimePickup;
            DateAndTimeLastPickup = dateAndTimeLastPickup;
            AgeRestricted = ageRestricted;
            Price = price;
            MealType = mealType;
            CanteenServedAt = canteenServedAt;
            ReservedBy = reservedBy;
            Products = products;
        }
    }
}
