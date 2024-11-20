using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TooGoodToGoAvans.Domain.Models;

namespace TooGoodToGoAvans.DomainService
{
    public interface ICanteenRepository
    {
        public Task<List<Canteen>> GetAllCanteensAsync();
        public Task<Canteen> GetCanteenByIdAsync(Guid canteenId);
    }
}
