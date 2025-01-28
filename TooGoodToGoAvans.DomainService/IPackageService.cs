using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TooGoodToGoAvans.Domain.Models;

namespace TooGoodToGoAvans.DomainService
{
    public interface IPackageService
    {
        public Task<bool> CheckIfPackageIsReserved(Guid packageId);
        public Task<IEnumerable<Package>> GetPackagesSpecificLocation(string location);
        public Task<IEnumerable<Package>> GetPackagesSpecificMealtype(string mealType);
        public Task CreateValidPackageAsync(Package package);
        public Task RemovePackageAsync(Package package);

    }
}
