using Application.Converters;
using Domain.Dtos;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Models;

namespace Application.Services
{
    public class StudentService : IStudentService
    {
        private readonly IUnitOfWork unitOfWork;

        public StudentService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<PaginationResult<StudentDto>> GetAllStudents(StudentStatus status = StudentStatus.IN_PROGRESS)
        {
            return await unitOfWork.Students.GetAsync(expression: s => s.Status == status, isTakeAll: true)
                .ContinueWith(t => new PaginationResult<StudentDto>
                {
                    PageCount = t.Result.PageCount,
                    TotalCount = t.Result.TotalCount,
                    PageIndex = t.Result.PageIndex,
                    TotalPages = t.Result.TotalPages,
                    Values = t.Result.Values.Select(v => AppConverter.ToDto(v)).ToList(),
                });
        }
    }
}
