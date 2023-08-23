using Domain.Dtos.Base;
using Domain.Entities;

namespace Domain.Dtos.Updates
{
    public class MembershipUpdateDto : BaseDto
    {
        public DateTime? JoinDate { get; set; }
        public DateTime? LeaveDate { get; set; }
        public MemberRole? Role { get; set; }
        public MemberStatus? Status { get; set; }
    }
}
