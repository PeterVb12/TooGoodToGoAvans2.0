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
    public class StaffMemberRepository : IStaffMemberRepository
    {
        private readonly TooGoodToGoAvansDBContext _context;
        private readonly ILogger<StaffMemberRepository> _logger;

        public StaffMemberRepository(TooGoodToGoAvansDBContext context, ILogger<StaffMemberRepository> logger)
        {
            _context = context;
            _logger = logger;

        }
        public Task<StaffMember> AddAsync(StaffMember staffMember)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteStaffMemberAsync(int staffMemberId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<StaffMember>> GetAllStaffMembersAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<StaffMember> GetStaffMemberByIdAsync(string incomingUserId)
        {
            try
            {
                // Controleer of incomingUserId null of leeg is
                if (string.IsNullOrEmpty(incomingUserId))
                {
                    _logger.LogWarning("GetStaffMemberByIdAsync: incomingUserId is null or empty.");
                    return null;
                }

                // Haal de StaffMember op via UserId
                var staffMember = await _context.StaffMembers
                    .FirstOrDefaultAsync(p => p.UserId == incomingUserId);

                if (staffMember == null)
                {
                    _logger.LogInformation($"No StaffMember found with UserId: {incomingUserId}");
                }

                return staffMember;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting StaffMember by UserId.");
                throw;
            }
        }

        public Task<StaffMember> UpdateStaffMemberAsync(StaffMember staffMember)
        {
            throw new NotImplementedException();
        }
    }
}
