using Domain.Dtos.Base;
using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos.Updates
{
    public class ClubBoardUpdateDto : BaseDto
    {
        [MinLength(1), MaxLength(50)]
        public string? Name { get; set; }
        public long ClubId { get; set; }
        public List<long>? MembershipIds { get; set; }
    }
}
