using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TooGoodToGoAvans.DomainService;
using NSubstitute;
using Xunit;
using TooGoodToGoAvans.Domain.Models;

namespace TooGoodToGoAvans.UI.Test
{
    public class PackageServiceTests
    {
        private readonly IPackageRepository _packageRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly PackageService _packageService;
        public PackageServiceTests()
        {
            _packageRepository = Substitute.For<IPackageRepository>();
            _studentRepository = Substitute.For<IStudentRepository>();
            _packageService = new PackageService(_packageRepository, _studentRepository);  // Veronderstel dat je de PackageService implementeert
        }

        private Package CreateTestPackage(bool reserved = false)
        {
            return new Package
            {
                PackageId = Guid.NewGuid(),
                Name = "Test Package",
                DateAndTimePickup = DateTime.Now.AddDays(1),
                ReservedBy = reserved ? new Student { UserId = "testUserId" } : null,
                MealType = "Lunch",
                CityLocation = City.Breda
            };
        }

        [Fact]
        public async Task CheckIfPackageIsReservedReturnsTrueWhenPackageIsReserved()
        {
            // Arrange
            var package = CreateTestPackage(reserved: true);
            _packageRepository.GetByIdAsync(package.PackageId).Returns(Task.FromResult(package));

            // Act
            var result = await _packageService.CheckIfPackageIsReserved(package.PackageId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task CheckIfPackageIsReservedThrowsExceptionWhenPackageNotFound()
        {
            // Arrange
            var packageId = Guid.NewGuid();
            _packageRepository.GetByIdAsync(Arg.Any<Guid>()).Returns((Package)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _packageService.CheckIfPackageIsReserved(packageId));

            // Controleer dat de exception het juiste bericht bevat
            Assert.Contains($"Package with ID {packageId} not found.", exception.Message);
        }


        [Fact]
        public async Task CheckIfPackageIsReserved_ReturnsTrue_WhenPackageIsReserved()
        {
            // Arrange
            var package = CreateTestPackage(reserved: true);
            _packageRepository.GetByIdAsync(package.PackageId).Returns(Task.FromResult(package));

            // Act
            var result = await _packageService.CheckIfPackageIsReserved(package.PackageId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task CheckIfPackageIsReserved_ThrowsException_WhenPackageNotFound()
        {
            // Arrange
            _packageRepository.GetByIdAsync(Arg.Any<Guid>()).Returns((Package)null);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _packageService.CheckIfPackageIsReserved(Guid.NewGuid()));
        }

        [Fact]
        public async Task GetPackagesSpecificLocation_ReturnsPackages_ForValidLocation()
        {
            // Arrange
            var location = "Breda";
            var locationEnum = City.Breda;
            var packages = new List<Package> { CreateTestPackage(), CreateTestPackage() };
            _packageRepository.GetByLocationAsync(locationEnum).Returns(packages);

            // Act
            var result = await _packageService.GetPackagesSpecificLocation(location);

            // Assert
            Assert.Equal(packages, result);
        }

        [Fact]
        public async Task CreateValidPackageAsync_AddsPackage_WhenReservationLimitNotReached()
        {
            // Arrange
            var package = CreateTestPackage();
            package.ReservedBy = new Student { UserId = "testUserId" }; // Zorg dat ReservedBy niet null is.
            _packageRepository.CheckReservationLimit(Arg.Any<string>(), Arg.Any<DateTime>()).Returns(false);

            // Act
            await _packageService.CreateValidPackageAsync(package);

            // Assert
            await _packageRepository.Received(1).AddPackageAsync(package);
        }

        [Fact]
        public async Task CreateValidPackageAsync_ThrowsException_WhenReservationLimitReached()
        {
            // Arrange
            var package = CreateTestPackage();
            package.ReservedBy = new Student { UserId = "testUserId" }; // Zorg dat ReservedBy niet null is.
            _packageRepository.CheckReservationLimit(Arg.Any<string>(), Arg.Any<DateTime>()).Returns(true);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _packageService.CreateValidPackageAsync(package));

            // Controleer dat de exception het juiste bericht bevat
            Assert.Equal("Je hebt al een pakket voor deze ophaaldatum.", exception.Message);
        }

        [Fact]
        public async Task RemovePackageAsync_RemovesPackage_WhenNotReserved()
        {
            // Arrange
            var package = CreateTestPackage(reserved: false);

            // Act
            await _packageService.RemovePackageAsync(package);

            // Assert
            await _packageRepository.Received(1).RemoveAsync(package);
        }

        [Fact]
        public async Task RemovePackageAsync_ThrowsException_WhenPackageIsReserved()
        {
            // Arrange
            var package = CreateTestPackage(reserved: true);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _packageService.RemovePackageAsync(package));
        }
    }
}
