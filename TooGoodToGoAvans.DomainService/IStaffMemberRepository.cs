using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TooGoodToGoAvans.Domain.Models;

namespace TooGoodToGoAvans.DomainService
{
    public interface IStaffMemberRepository
    {
        Task<StaffMember> AddStaffMemberAsync(StaffMember staffMember);
        Task<StaffMember> GetStaffMemberByIdAsync(int staffMemberId);
        Task<IEnumerable<StaffMember>> GetAllStaffMembersAsync();
        Task<StaffMember> UpdateStaffMemberAsync(StaffMember staffMember);
        Task<bool> DeleteStaffMemberAsync(int staffMemberId);
    }
}
