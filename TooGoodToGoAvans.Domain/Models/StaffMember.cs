using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TooGoodToGoAvans.Domain.Models
{
    public class StaffMember
    {
        public Guid StaffMemberId { get; set; }
        public string Name { get; set; }
        public int EmployeeNumber { get; set; }

        public Canteen WorkLocation { get; set; }

        public StaffMember(Guid staffMemberId, string name, int employeeNumber, Canteen workLocation)
        {
            StaffMemberId = staffMemberId;
            Name = name;
            EmployeeNumber = employeeNumber;
            WorkLocation = workLocation;
        }
    }
}
