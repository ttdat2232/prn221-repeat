using Domain.Dtos;
using Domain.Dtos.Creates;
using Domain.Dtos.Updates;
using Domain.Models;

namespace Domain.Interfaces.Services
{
    public interface IClubActivityService
    {
        Task<PaginationResult<ClubActivityDto>> GetAllClubActivitiesByClubIdAsync(long clubId, int pageIndex = 0);
        Task<ClubActivityDto> GetClubActivityByIdAsync(long id);
        Task<ClubActivityDto> AddClubActivityAsync(ClubActivityCreateDto createClubActivity);
        Task<ClubActivityDto> UpdateActivity(ClubActivityUpdateDto update);
        Task DeleteActivityByIdAsync(long id);
        Task UpdateStatusAllActivityAsync();
    }
}