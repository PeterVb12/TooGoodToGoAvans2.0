using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TooGoodToGoAvans.Domain.Models;

namespace TooGoodToGoAvans.DomainService
{
    public class PackageService : IPackageService
    {
        private readonly IPackageRepository _packageRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ILogger<PackageService> _logger;

        public PackageService(IPackageRepository packageRepository, IStudentRepository studentRepository, ILogger<PackageService> logger)
        {
            _packageRepository = packageRepository;
            _studentRepository = studentRepository;
            _logger = logger;
        }

        // Methode om te controleren of een pakket al gereserveerd is.
        public async Task<bool> CheckIfPackageIsReserved(Guid packageId)
        {
            var package = await _packageRepository.GetByIdAsync(packageId);
            if (package == null)
            {
                throw new Exception($"Package with ID {packageId} not found.");
            }
            _logger.LogInformation($"Package ID: {package.PackageId}, ReservedBy: {package.ReservedBy?.Name}");
            return package.ReservedBy != null;
        }

        // Methode om alle pakketten op basis van een specifieke locatie op te halen.
        public async Task<IEnumerable<Package>> GetPackagesSpecificLocation(string location)
        {
            if (Enum.TryParse<City>(location, true, out var city))
            {
                return await _packageRepository.GetByLocationAsync(city);
            }
            throw new ArgumentException($"Invalid location: {location}. Must be a valid City value.", nameof(location));

        }

        // Methode om pakketten op basis van een specifiek maaltijdtype op te halen.
        public async Task<IEnumerable<Package>> GetPackagesSpecificMealtype(string mealType)
        {
            return await _packageRepository.GetByMealTypeAsync(mealType);
        }

        // Methode om een geldig pakket aan te maken.
        public async Task CreateValidPackageAsync(Package package)
        {
            var studentHasPackage = await _packageRepository.CheckReservationLimit(package.ReservedBy.UserId, package.DateAndTimePickup);
            if (studentHasPackage)
            {
                throw new InvalidOperationException("Je hebt al een pakket voor deze ophaaldatum.");
            }

            await _packageRepository.AddPackageAsync(package);
        }

        // Methode om een pakket te verwijderen.
        public async Task RemovePackageAsync(Package package)
        {
            if (package.ReservedBy != null)
            {
                throw new InvalidOperationException("Dit pakket kan niet worden verwijderd, omdat het al gereserveerd is.");
            }

            await _packageRepository.RemoveAsync(package);
        }

    }
}
