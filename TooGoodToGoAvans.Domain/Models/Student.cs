using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TooGoodToGoAvans.Domain.Models
{
    [Index(nameof(StudentId), IsUnique = true)]
    public class Student
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime Birthdate { get; set; }
        public string StudentId { get; set; }
        public string EmailAddress { get; set; }
        public City StudentCity { get; set; }
        public string Phonenumber { get; set; }
    }
}
