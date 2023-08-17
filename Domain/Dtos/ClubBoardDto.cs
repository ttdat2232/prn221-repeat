using Domain.Dtos.Base;

namespace Domain.Dtos
{
    public class ClubBoardDto : BaseDto
    {
        public long ClubId { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<MembershipDto> MembershipDtos { get; set; } = new List<MembershipDto>();
    }
}