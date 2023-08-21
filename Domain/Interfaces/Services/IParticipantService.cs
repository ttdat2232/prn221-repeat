using Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Services
{
    public interface IParticipantService
    {
        Task<ParticipantDto> AddParticipantAsync(long memberId, long activityId);
        Task DeleteParticipantAsync(long memberId, long clubActivityId);
    }
}
