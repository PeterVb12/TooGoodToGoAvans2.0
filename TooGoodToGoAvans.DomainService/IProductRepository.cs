using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TooGoodToGoAvans.Domain.Models;

namespace TooGoodToGoAvans.DomainService
{
    public interface IProductRepository
    {
        Task AddProductAsync(Product product);
        Task<Product> GetProductByIdAsync(Guid id);
        Task<IEnumerable<Product>> GetProductsAsync();
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(Guid id);
    }
}
