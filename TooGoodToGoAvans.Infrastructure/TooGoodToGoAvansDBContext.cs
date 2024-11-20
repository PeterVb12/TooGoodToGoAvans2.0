using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TooGoodToGoAvans.Domain.Models;
using Microsoft.EntityFrameworkCore;


namespace TooGoodToGoAvans.Infrastructure
{
    public class TooGoodToGoAvansDBContext : DbContext
    {
        public TooGoodToGoAvansDBContext() { }

        public TooGoodToGoAvansDBContext(DbContextOptions<TooGoodToGoAvansDBContext> options) : base(options)
        {
        }
        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<StaffMember> StaffMembers { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Package> Packages { get; set; } = null!;   
        public DbSet<Canteen> Canteens { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Canteen>().HasData(
                new Canteen(
                    id: Guid.NewGuid(),
                    city: City.Breda,
                    canteenLocation: "Breda Campus Kantine",
                    offersWarmMeals: true
                ),
                new Canteen(
                    id: Guid.NewGuid(),
                    city: City.Tilburg,
                    canteenLocation: "Tilburg Campus Kantine",
                    offersWarmMeals: false
                ),
                new Canteen(
                    id: Guid.NewGuid(),
                    city: City.DenBosch,
                    canteenLocation: "Den Bosch Campus Kantine",
                    offersWarmMeals: true
                )
            );
        }

    }
}
