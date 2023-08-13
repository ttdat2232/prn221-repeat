using Domain.Dtos;
using Domain.Dtos.Creates;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Services
{
    public interface IClubService
    {
        Task<ClubDto> GetClubByIdAsync(long id);
        Task<ClubDto> AddClubAsync(ClubCreateDto club);
        Task<PaginationResult<ClubDto>> GetClubsAsync(int pageSize = 4, int pageIndex = 0);
        Task DeleteClubAsync(long id);
    }
}
