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

        public Task<StaffMember> GetStaffMemberByIdAsync(int staffMemberId)
        {
            throw new NotImplementedException();
        }

        public Task<StaffMember> UpdateStaffMemberAsync(StaffMember staffMember)
        {
            throw new NotImplementedException();
        }
    }
}
