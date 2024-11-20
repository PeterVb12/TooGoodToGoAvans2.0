using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TooGoodToGoAvans.Domain.Models;

namespace TooGoodToGoAvans.DomainService
{
    public interface IStudentRepository
    {
        Task<Student> AddAsync(Student student);
        Task<Student> GetStudentByIdAsync(int studentId);
        Task<IEnumerable<Student>> GetAllStudentsAsync();
        Task<Student> UpdateStudentAsync(Student student);
        Task<bool> DeleteStudentAsync(int studentId);
    }
}
