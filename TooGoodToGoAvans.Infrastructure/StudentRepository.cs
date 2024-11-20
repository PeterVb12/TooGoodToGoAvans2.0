using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TooGoodToGoAvans.Domain.Models;
using TooGoodToGoAvans.DomainService;

namespace TooGoodToGoAvans.Infrastructure
{
    public class StudentRepository : IStudentRepository
    {
        public Task<Student> AddAsync(Student student)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteStudentAsync(int studentId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Student> GetStudentByIdAsync(int studentId)
        {
            throw new NotImplementedException();
        }

        public Task<Student> UpdateStudentAsync(Student student)
        {
            throw new NotImplementedException();
        }
    }
}
