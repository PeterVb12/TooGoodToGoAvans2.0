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
        public Task AddPackageAsync(Package package)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Package> GetPackages()
        {
            throw new NotImplementedException();
        }

        public Task RemovePackageAsync(Package package)
        {
            throw new NotImplementedException();
        }

        public Task ReservePackage(Student student)
        {
            throw new NotImplementedException();
        }

        public Task UpdatePackageAsync(Package package)
        {
            throw new NotImplementedException();
        }
    }
}
