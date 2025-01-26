using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TooGoodToGoAvans.DomainService;
using TooGoodToGoAvans.Infrastructure;
using TooGoodToGoAvans.Models;

namespace TooGoodToGoAvans.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPackageRepository _packageRepository;

        public HomeController(ILogger<HomeController> logger, IPackageRepository packageRepository)
        {
            _logger = logger;
            _packageRepository = packageRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> PackageReserving()
        {
            if (!User.Identity.IsAuthenticated)
            {
                // Gebruiker is niet ingelogd, stuur door naar login
                return RedirectToAction("Login", "Account");
            }
            var packages = await _packageRepository.GetPackagesAsync();
            return View("/Views/Package/PackageReserving.cshtml", packages);
        }

        public async Task<IActionResult> ReservedPackages()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized(); 
            }

            var packages = await _packageRepository.GetReservedPackagesByUserAsync(userId);
            return View("/Views/Package/ReservedPackages.cshtml", packages);
        }
    }
}
