using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TooGoodToGoAvans.Domain.Models;
using TooGoodToGoAvans.DomainService;

namespace TooGoodToGoAvans.WebApi.GraphQl
{
    public class PackageQuery
    {
        private readonly IPackageRepository _packageRepository;

        public PackageQuery(IPackageRepository packageRepository)
        {
            _packageRepository = packageRepository;
        }

        [GraphQLName("getPackages")]
        public async Task<IEnumerable<Package>> GetPackagesAsync()
        {
            return await _packageRepository.GetPackagesAsync();
        }
    }
}
