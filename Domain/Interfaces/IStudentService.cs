using Domain.Dtos;
using Domain.Entities;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IStudentService
    {
        Task<PaginationResult<StudentDto>> GetAllStudent(StudentStatus status = StudentStatus.IN_PROGRESS);
    }
}
