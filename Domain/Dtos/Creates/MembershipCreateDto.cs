using Domain.Dtos.Base;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos.Creates
{
    public class MembershipCreateDto : BaseDto
    {
        public long StudentId { get; set; }
        public long ClubId { get; set; }
        public List<long>? ClubBoardIds { get; set; }
        public MemberRole Role { get; set; }
    }
}
