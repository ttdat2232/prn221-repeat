using Domain.Dtos;
using Domain.Dtos.Creates;
using Domain.Dtos.Updates;
using Domain.Models;

namespace Domain.Interfaces.Services
{
    public interface IClubBoardService
    {
        Task<PaginationResult<ClubBoardDto>> GetClubBoardsByClubIdAsync(long clubId, int pageIndex = 0);
        Task<ClubBoardDto> GetClubBoardByIdAsync(long id);
        Task<ClubBoardDto> AddClubBoardAsync(ClubBoardCreateDto clubBoard);
        Task<ClubBoardDto> UpdateClubBoardAsync(ClubBoardUpdateDto updateClubBoard);
        Task DeleteClubBoardByIdAsync(long clubBoardId);
        Task AddMembersToBoard(long id, List<long> newMemberId);
        Task RemoveMemberFromBoard(long memberId, long clubBoardId);
    }
}
