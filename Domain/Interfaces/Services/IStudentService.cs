using Domain.Dtos;
using Domain.Entities;
using Domain.Models;

namespace Domain.Interfaces.Services
{
    public interface IStudentService
    {
        Task<PaginationResult<StudentDto>> GetAllStudents(StudentStatus status = StudentStatus.IN_PROGRESS);
    }
}
