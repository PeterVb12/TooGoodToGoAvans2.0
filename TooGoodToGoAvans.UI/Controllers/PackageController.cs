using Microsoft.AspNetCore.Mvc;
using TooGoodToGoAvans.Domain.Models;
using TooGoodToGoAvans.DomainService;
using TooGoodToGoAvans.Infrastructure;
using TooGoodToGoAvans.UI.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace TooGoodToGoAvans.UI.Controllers
{
    public class PackageController : Controller
    {
        private readonly IPackageService _packageService;
        private readonly IPackageRepository _packageRepository;
        private readonly IProductRepository _productRepository;
        private readonly IStaffMemberRepository _staffMemberRepository;
        private readonly ILogger<PackageController> _logger;
        public PackageController(IPackageService packageService, IPackageRepository packageRepository, IProductRepository productRepository, IStaffMemberRepository staffMemberRepository, ILogger<PackageController> logger)
        {
            _packageService = packageService;
            _packageRepository = packageRepository;
            _productRepository = productRepository;
            _staffMemberRepository = staffMemberRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> PackageCreate()
        {
            // Await the async method to get the products
            var products = await _productRepository.GetProductsAsync();

            // Create the view model
            var packageViewModel = new PackageViewModel
            {
                Products = products.Select(p => new SelectableProductViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Selected = false
                }).ToList()
            };

            // Pass the view model to the view
            return View("/Views/Package/PackageCreate.cshtml", packageViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePackage(PackageViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Verkrijg de ingelogde staffmember
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                Console.WriteLine($"UserId from claims: {userId}");
                _logger.LogInformation("UserId ontvangen in repository: {UserId}", userId);
                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("UserId could not be found in User.Claims.");
                    return View("Error"); // Voeg eventueel een foutmelding toe
                }
                var staffMember = await _staffMemberRepository.GetStaffMemberByIdAsync(userId);
                if (staffMember == null)
                {
                    ModelState.AddModelError(string.Empty, "Geen geldige staffmember gevonden.");
                    return View("/Views/Package/PackageCreate.cshtml", model);
                }

                // Maak een nieuw pakket aan
                var package = new Package
                {
                    Name = model.Name,
                    DateAndTimePickup = model.DateAndTimePickup,
                    DateAndTimeLastPickup = model.DateAndTimeLastPickup,
                    AgeRestricted = model.AgeRestricted,
                    Price = model.Price,
                    MealType = model.MealType,
                    CityLocation = staffMember.StaffmemberCity, // Koppel de stad
                    CanteenServedAt = staffMember.WorkLocation, // Koppel de kantine
                    ReservedBy = null, // Het pakket is nog niet gereserveerd
                    Products = new List<Product>()
                };

                // Voeg geselecteerde producten toe aan het pakket
                var selectedProductIds = model.Products
                    .Where(p => p.Selected)
                    .Select(p => p.Id)
                    .ToList();

                foreach (var productId in selectedProductIds)
                {
                    var product = await _productRepository.GetProductByIdAsync(productId);
                    if (product != null)
                    {
                        package.Products.Add(product);
                    }
                }

                // Sla het pakket op in de database
                await _packageRepository.AddPackageAsync(package);
                return RedirectToAction("Index");
            }

            // Als de modelstate ongeldig is, herlaad de producten voor de view
            var allProducts = await _productRepository.GetProductsAsync();
            model.Products = allProducts.Select(p => new SelectableProductViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Selected = model.Products.Any(sp => sp.Id == p.Id && sp.Selected) // Houd de selectie aan
            }).ToList();

            return View("/Views/Package/PackageCreate.cshtml", model);
        }

        public async Task<IActionResult> PackageDetails(Guid id)
        {
            Console.WriteLine($"Package ID: {id}");
            var packages = await _packageRepository.GetPackagesAsync();

            var package = packages
                .Where(p => p.PackageId == id)
                .FirstOrDefault();

            if (package == null)
            {
                return NotFound();
            }
            Console.WriteLine($"Opgehaalde package id: + {package.PackageId}");
            return View(package);
        }

        public async Task<IActionResult> ReservePackage(Guid packageId)
        {
            try
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    return Unauthorized();
                }
                _logger.LogInformation("ReservePackage: UserId opgehaald: {UserId}", userId);
                await _packageRepository.ReservePackageAsync(packageId, userId);

                return RedirectToAction("PackageReserving", "Home", new { id = packageId });
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
        }


    }
}
