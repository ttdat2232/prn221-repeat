using Domain.Entities;

namespace Domain.Dtos
{
    public class ParticipantDto
    {
        public ClubActivityDto? CLubActivityDto { get; set; }
        public MembershipDto? Membership { get; set; }
        public ParticipantStatus Status { get; set; }
    }
}
