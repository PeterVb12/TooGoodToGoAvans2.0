using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TooGoodToGoAvans.Domain.Models;

namespace TooGoodToGoAvans.DomainService
{
    public interface IPackageRepository
    {
        public Task <IEnumerable<Package>> GetPackagesAsync();
        public Task AddPackageAsync(Package package);
        public Task UpdatePackageAsync(Package package);
        public Task RemovePackageAsync(Package package);

        public Task ReservePackage(Student student);
    }
}
