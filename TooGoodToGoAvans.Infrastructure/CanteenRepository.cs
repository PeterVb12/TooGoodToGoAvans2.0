using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TooGoodToGoAvans.Domain.Models;
using TooGoodToGoAvans.DomainService;

namespace TooGoodToGoAvans.Infrastructure
{
    public class CanteenRepository : ICanteenRepository
    {
        private readonly TooGoodToGoAvansDBContext _context;
        private readonly ILogger<CanteenRepository> _logger;

        public CanteenRepository(TooGoodToGoAvansDBContext context, ILogger<CanteenRepository> logger)
        {
            _context = context;
            _logger = logger;

        }

        public async Task<List<Canteen>> GetAllCanteensAsync()
        {
             var canteens = await _context.Canteens.ToListAsync();
            _logger.LogInformation($"Aantal kantines opgehaald: {canteens.Count}"); // Logging
            return canteens;
        }

        public async Task<Canteen> GetCanteenByIdAsync(Guid canteenId)
        {
            return await _context.Canteens.FindAsync(canteenId);
        }

    }
}
