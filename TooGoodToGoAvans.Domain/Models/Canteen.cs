using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TooGoodToGoAvans.Domain.Models
{
    public class Canteen
    {
        public Guid Id { get; set; }
        public City City { get; set; }
        public string CanteenLocation { get; set; }
        public bool OffersWarmMeals { get; set; }

        public Canteen(Guid id, City city, string canteenLocation, bool offersWarmMeals)
        {
            Id = id;
            City = city;
            CanteenLocation = canteenLocation;
            OffersWarmMeals = offersWarmMeals;
        }
    }
}
