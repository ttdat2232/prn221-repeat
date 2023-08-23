using Domain.Dtos;
using Domain.Dtos.Creates;
using Domain.Dtos.Updates;
using Domain.Entities;
using Domain.Models;

namespace Domain.Interfaces.Services
{
    public interface IMembershipService
    {
        Task<PaginationResult<MembershipDto>> GetMembershipByClubIdAsync(long clubId, MemberStatus status = MemberStatus.JOIN);
        Task<MembershipDto> GetMemberShipByIdAsync(long id, MemberStatus status = MemberStatus.JOIN);
        Task<PaginationResult<MembershipDto>> GetMembershipByNameAsync(string name = "", MemberStatus status = MemberStatus.JOIN, int pageSize = 4, int pageIndex = 0);
        Task<MembershipDto> AddMemberShipAsync(MembershipCreateDto membership);
        Task<MembershipDto> UpdateMembershipAsync(MembershipUpdateDto membership);
        Task DeleteMembershipAsync(long id);
    }
}
