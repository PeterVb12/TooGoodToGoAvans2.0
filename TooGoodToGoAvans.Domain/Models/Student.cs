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

        // Identity UserId
        public string UserId { get; set; }

        public Student() { }

        public Student(Guid id, string name, DateTime birthdate, string studentId, string emailAddress, City studentCity, string phonenumber, string userId)
        {
            Id = id;
            Name = name;
            Birthdate = birthdate;
            StudentId = studentId;
            EmailAddress = emailAddress;
            StudentCity = studentCity;
            Phonenumber = phonenumber;
            UserId = userId;
        }
    }
}
