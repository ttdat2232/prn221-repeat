using Domain.Dtos.Base;

namespace Domain.Dtos
{
    public class ClubDto : BaseDto
    {
        public string Name { get; set; } = string.Empty;
        public DateTime CreateAt { get; set; }
        public string LogoUrl { get; set; } = string.Empty;
        public List<long>? ClubBoardIds { get; set; }
        public List<ClubBoardDto>? ClubBoards { get; set; }
        public List<MembershipDto>? Memberships { get; set; }
    }
}
