using Domain.Dtos;
using Domain.Dtos.Creates;
using Domain.Dtos.Updates;
using Domain.Models;

namespace Domain.Interfaces.Services
{
    public interface IClubService
    {
        Task<ClubDto> GetClubByIdAsync(long id);
        Task<ClubDto> AddClubAsync(ClubCreateDto club);
        Task<PaginationResult<ClubDto>> GetClubsAsync(int pageSize = 4, int pageIndex = 0);
        Task DeleteClubAsync(long id);
        Task UpdateClubAsync(ClubUpdateDto club);
    }
}
