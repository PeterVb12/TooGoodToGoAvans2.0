using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TooGoodToGoAvans.Domain.Models;
using TooGoodToGoAvans.DomainService;

namespace TooGoodToGoAvans.Infrastructure
{
    public class PackageRepository : IPackageRepository
    {
        private readonly TooGoodToGoAvansDBContext _context;

        public PackageRepository(TooGoodToGoAvansDBContext context)
        {
            _context = context;
        }
        public async Task AddPackageAsync(Package package)
        {

            _context.Packages.Add(package);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Package>> GetPackagesAsync()
        {
            return await _context.Packages.ToListAsync();
        }

        public Task RemovePackageAsync(Package package)
        {
            throw new NotImplementedException();
        }

        public async Task ReservePackageAsync(Guid packageId, string userId)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.UserId == userId);
            if (student == null)
            {
                throw new Exception("Student not found for the logged-in user.");
            }

            var package = await _context.Packages.FirstOrDefaultAsync(p => p.PackageId == packageId);
            if (package == null)
            {
                throw new Exception("Package not found.");
            }

            if (package.ReservedBy != null)
            {
                throw new Exception("Package is already reserved.");
            }

            // Koppel de student aan het pakket
            package.ReservedBy = student;

            // Sla de wijzigingen op
            _context.Packages.Update(package);
            await _context.SaveChangesAsync();
        }

        public Task UpdatePackageAsync(Package package)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Package>> GetReservedPackagesByUserAsync(string userId)
        {
            return await _context.Packages
                .Where(p => p.ReservedBy != null && p.ReservedBy.UserId == userId)
                .ToListAsync();
        }


        public async Task<Package> GetByIdAsync(Guid packageId)
        {
            return await _context.Packages
                .FirstOrDefaultAsync(p => p.PackageId == packageId);
        }

        public async Task<IEnumerable<Package>> GetByLocationAsync(string location)
        {
            return await _context.Packages
                .Where(p => p.CityLocation.ToString().Equals(location, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();
        }

        public async Task<IEnumerable<Package>> GetByMealTypeAsync(string mealType)
        {
            return await _context.Packages
                .Where(p => p.MealType.Equals(mealType, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();
        }

        public async Task<bool> CheckReservationLimit(string studentId, DateTime pickupDate)
        {
            var reservedPackage = await _context.Packages
                .Where(p => p.ReservedBy.UserId == studentId && p.DateAndTimePickup.Date == pickupDate.Date)
                .FirstOrDefaultAsync();

            return reservedPackage != null;
        }

        public async Task RemoveAsync(Package package)
        {
            _context.Packages.Remove(package);
            await _context.SaveChangesAsync();
        }

    }
}
