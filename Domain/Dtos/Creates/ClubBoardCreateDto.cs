using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos.Creates
{
    public class ClubBoardCreateDto
    {
        [Required, MinLength(1), MaxLength(50)]
        public string Name { get; set; }
        public long? ClubId { get; set; }
        public List<long>? MembershipIds { get; set; }
    }
}
