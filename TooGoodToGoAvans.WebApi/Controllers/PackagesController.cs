using Microsoft.AspNetCore.Mvc;
using TooGoodToGoAvans.Domain.Models;
using TooGoodToGoAvans.DomainService;
using TooGoodToGoAvans.Infrastructure;

namespace TooGoodToGoAvans.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PackagesController : ControllerBase
    {
        private readonly IPackageRepository _packageRepository;

        public PackagesController(IPackageRepository packageRepository)
        {
            _packageRepository = packageRepository;
        }

        // GET api/packages
        [HttpGet]
        public async Task<IActionResult> GetPackagesAsync()
        {
            var packages = await _packageRepository.GetPackagesAsync();
            return Ok(packages);
        }

        // POST api/packages/reserve
        [HttpPost("reserve")]
        public async Task<IActionResult> ReservePackageAsync([FromBody] ReservePackageRequest request)
        {
            if (request == null || request.PackageId == Guid.Empty || string.IsNullOrEmpty(request.UserId))
            {
                return BadRequest("Invalid reservation request.");
            }

            try
            {
                await _packageRepository.ReservePackageAsync(request.PackageId, request.UserId);
                return Ok(new { Message = "Package reserved successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    }

    // DTO voor het reserveren van een pakket
    public class ReservePackageRequest
    {
        public Guid PackageId { get; set; }
        public string UserId { get; set; }
    }
}
