using Domain.Dtos.Base;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos.Updates
{
    public class MembershipUpdateDto : BaseDto
    {
        public DateTime? JoinDate { get; set; }
        public DateTime? LeaveDate { get; set; }
        public MemberRole? Role { get; set; }
        public MemberStatus? MemberStatus { get; set; }
    }
}
