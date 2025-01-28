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
        private readonly IPackageService _packageService;
        private readonly IStaffMemberRepository _staffMemberRepository;

        public HomeController(ILogger<HomeController> logger, IPackageRepository packageRepository, IPackageService packageService, IStaffMemberRepository staffMemberRepository)
        {
            _logger = logger;
            _packageRepository = packageRepository;
            _packageService = packageService;
            _staffMemberRepository = staffMemberRepository;
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

        public async Task<IActionResult> StaffPackageOverview()
        {
            if (!User.Identity.IsAuthenticated)
            {
                // Gebruiker is niet ingelogd, stuur door naar login
                return RedirectToAction("Login", "Account");
            }

            // Haal de ingelogde medewerker op
            var staffMemberId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(staffMemberId))
            {
                return Forbid(); // Geen toegang als de medewerker niet gevonden wordt
            }

            // Haal de medewerker op uit de database (gebruik repository/service voor StaffMember)
            var staffMember = await _staffMemberRepository.GetStaffMemberByIdAsync(staffMemberId);
            if (staffMember == null)
            {
                return NotFound("Staff member not found");
            }

            // Verkrijg de locatie (City) van de medewerker
            var city = staffMember.StaffmemberCity.ToString();

            // Haal pakketten op voor de locatie
            var packages = await _packageService.GetPackagesSpecificLocation(city);

            // Sorteer de pakketten op datum
            var sortedPackages = packages.OrderBy(p => p.DateAndTimePickup).ToList();

            return View("/Views/Package/PackageStaffOverview.cshtml", sortedPackages);
        }

    }
}

