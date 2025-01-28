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
                return View("/Views/Home/Index.cshtml");
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

        [HttpGet]
        public async Task<IActionResult> EditPackage(Guid packageId)
        {
            var package = await _packageRepository.GetByIdAsync(packageId);
            if (package == null)
            {
                return NotFound();
            }

            // Haal alle producten op
            var allProducts = await _productRepository.GetProductsAsync();

            // Maak een lijst van SelectableProductViewModel
            var productViewModels = allProducts.Select(product => new SelectableProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Selected = package.Products.Any(p => p.Id == product.Id) // Check of product al in package zit
            }).ToList();

            // Maak het viewmodel
            var model = new PackageViewModel
            {
                PackageId = package.PackageId,
                Name = package.Name,
                DateAndTimePickup = package.DateAndTimePickup,
                DateAndTimeLastPickup = package.DateAndTimeLastPickup,
                AgeRestricted = package.AgeRestricted,
                Price = package.Price,
                MealType = package.MealType,
                CanteenServedAt = package.CanteenServedAt,
                Products = productViewModels
            };

            return View(model);
        }

        // Verwerk de bewerking
        [HttpPost]
        public async Task<IActionResult> EditPackage(PackageViewModel model)
        {
            if (ModelState.IsValid)
            {
                var package = await _packageRepository.GetByIdAsync(model.PackageId);
                if (package == null)
                {
                    return NotFound();
                }

                // Werk de pakketgegevens bij
                package.Name = model.Name;
                package.DateAndTimePickup = model.DateAndTimePickup;
                package.DateAndTimeLastPickup = model.DateAndTimeLastPickup;
                package.AgeRestricted = model.AgeRestricted;
                package.Price = model.Price;
                package.MealType = model.MealType;
                package.CanteenServedAt = model.CanteenServedAt;

                // Verwijder niet-geselecteerde producten
                var selectedProductIds = model.Products
                    .Where(p => p.Selected)
                    .Select(p => p.Id)
                    .ToList();

                var productsToRemove = package.Products
                    .Where(p => !selectedProductIds.Contains(p.Id))
                    .ToList();

                foreach (var product in productsToRemove)
                {
                    package.Products.Remove(product);
                }

                // Voeg nieuwe geselecteerde producten toe
                foreach (var productId in selectedProductIds)
                {
                    if (!package.Products.Any(p => p.Id == productId))
                    {
                        var product = await _productRepository.GetProductByIdAsync(productId);
                        if (product != null)
                        {
                            package.Products.Add(product);
                        }
                    }
                }

                // Sla het pakket op
                await _packageRepository.UpdatePackageAsync(package);

                return RedirectToAction("Index"); // Of een andere pagina
            }

            // Als de modelstate ongeldig is, herlaad de producten voor de view
            var allProducts = await _productRepository.GetProductsAsync();
            model.Products = allProducts.Select(p => new SelectableProductViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Selected = model.Products.Any(sp => sp.Id == p.Id && sp.Selected)
            }).ToList();

            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> DeletePackage(Guid id)
        {
            var package = await _packageRepository.GetByIdAsync(id);
            if (package == null)
            {
                TempData["ErrorMessage"] = "Package not found.";
                return View("/Views/Home/Index.cshtml");
            }

            // Controleer of het pakket gereserveerd is
            bool isReserved = await _packageService.CheckIfPackageIsReserved(id);
            if (isReserved)
            {
                TempData["ErrorMessage"] = "The package cannot be deleted as it is already reserved.";
                return View("/Views/Home/Index.cshtml");
            }

            try
            {
                // Verwijder het pakket
                await _packageRepository.RemoveAsync(package);
                TempData["SuccessMessage"] = "Package deleted successfully.";
                return View("/Views/Home/Index.cshtml");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                return View("/Views/Home/Index.cshtml");
            }
        }

    }
}
