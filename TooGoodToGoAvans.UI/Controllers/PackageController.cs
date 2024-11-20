using Microsoft.AspNetCore.Mvc;
using TooGoodToGoAvans.Domain.Models;
using TooGoodToGoAvans.DomainService;
using TooGoodToGoAvans.Infrastructure;
using TooGoodToGoAvans.UI.Models;
using System.Linq;

namespace TooGoodToGoAvans.UI.Controllers
{
    public class PackageController : Controller
    {
        private readonly IPackageService _packageService;
        private readonly IPackageRepository _packageRepository;
        private readonly IProductRepository _productRepository;
        public PackageController(IPackageService packageService, IPackageRepository packageRepository, IProductRepository productRepository)
        {
            _packageService = packageService;
            _packageRepository = packageRepository;
            _productRepository = productRepository;
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
                var package = new Package
                {
                    Name = model.Name,
                    DateAndTimePickup = model.DateAndTimePickup,
                    DateAndTimeLastPickup = model.DateAndTimeLastPickup,
                    AgeRestricted = model.AgeRestricted,
                    Price = model.Price,
                    MealType = model.MealType,
                    CanteenServedAt = model.CanteenServedAt,
                    ReservedBy = null, // Het pakket is nog niet gereserveerd
                    Products = new List<Product>()
                };

                // Verkrijg de geselecteerde producten en voeg ze toe aan het pakket
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


    }
}
