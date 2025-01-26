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
        public string EmployeeNumber { get; set; }
        public Canteen WorkLocation { get; set; }
        public City StaffmemberCity { get; set; }
        public StaffMember() { }

        // Identity UserId
        public string UserId { get; set; }

        public StaffMember(Guid staffMemberId, string name, string employeeNumber, Canteen workLocation, City staffmemberCity)
        {
            StaffMemberId = staffMemberId;
            Name = name;
            EmployeeNumber = employeeNumber;
            WorkLocation = workLocation;
            StaffmemberCity = staffmemberCity;
        }
    }
}
