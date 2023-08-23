using Domain.Dtos.Base;
using Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos.Updates
{
    public class ClubActivityUpdateDto : BaseDto
    {
        [MinLength(1), MaxLength(100)]
        public string? Name;
        public ActivityStatus Status { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public List<long> ParticipatingMembersIds { get; set; } = new List<long>();
    }
}
