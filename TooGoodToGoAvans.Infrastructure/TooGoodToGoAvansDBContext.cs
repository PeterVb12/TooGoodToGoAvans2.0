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

        public TooGoodToGoAvansDBContext(DbContextOptions<TooGoodToGoAvansDBContext> options) : base(options)
        {
        }
        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<StaffMember> StaffMembers { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Package> Packages { get; set; } = null!;   
        public DbSet<Canteen> Canteens { get; set; } = null!;
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server=localhost;Database=EF_TooGoodToGoAvans;Integrated Security=True;TrustServerCertificate=True;");
        //}
    }
}
