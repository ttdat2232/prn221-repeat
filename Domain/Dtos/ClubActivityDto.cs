using Domain.Dtos.Base;
using Domain.Entities;

namespace Domain.Dtos
{
    public class ClubActivityDto : BaseDto
    {
        public string Name { get; set; } = string.Empty;
        public ActivityStatus Status { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public ClubDto? Club { get; set; }
        public List<ParticipantDto> Participants { get; set; } = new List<ParticipantDto>();
    }
}
