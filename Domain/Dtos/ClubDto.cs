using Domain.Dtos.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
