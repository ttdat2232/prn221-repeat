using Domain.Dtos;

namespace Domain.Interfaces.Services
{
    public interface IParticipantService
    {
        Task<ParticipantDto> AddParticipantAsync(long memberId, long activityId);
        Task DeleteParticipantAsync(long memberId, long clubActivityId);
    }
}
