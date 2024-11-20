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
