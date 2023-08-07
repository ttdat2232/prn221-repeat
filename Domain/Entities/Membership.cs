using Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Membership
    {
        public DateOnly JoinDate { get; set; }
        public DateOnly LeaveDate { get; set; }
        public MemberStatus Status { get; set; }
        public MemberRole Role { get; set; }
        [Key, Column(Order = 0)]
        public long ClubId { get; set; }
        [Key, Column(Order = 1)]
        public long StudentId { get; set; }
        public virtual Club? Club { get; set; }
        public virtual Student? Student { get; set; }
    }
    
    public enum MemberStatus
    {
        JOIN = 0,
        LEAVE = 1,
    }
    public enum MemberRole
    {
        MEMBER = 0,
        PRESIDENT = 1,
    }
}
