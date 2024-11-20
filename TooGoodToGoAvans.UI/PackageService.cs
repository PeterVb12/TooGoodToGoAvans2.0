using TooGoodToGoAvans.Domain.Models;
using TooGoodToGoAvans.DomainService;

namespace TooGoodToGoAvans.UI
{
    public class PackageService : IPackageService
    {
        public Task<bool> CheckIfPackageIsReserved()
        {
            throw new NotImplementedException();
        }

        public Task CreateValidPackageAsync(Package package)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Package>> GetPackagesSpecificLocation(string location)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Package>> GetPackagesSpecificMealtype(string mealType)
        {
            throw new NotImplementedException();
        }

        public Task RemovePackageAsync(Package package)
        {
            throw new NotImplementedException();
        }
    }
}
