using Domain.Dtos.Base;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos
{
    public class MembershipDto : BaseDto
    {
        public string Name { get; set; }
        public long StudentId { get; set; }
        public string ClubName { get; set; }
        public MemberStatus Status { get; set; }
        public DateTime JoinDate { get; set; }
        public DateTime LeaveDate { get; set; }
        public MemberRole Role { get; set; }
    }
}
