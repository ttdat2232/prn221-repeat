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
        [Key]
        public long Id { get; set; }
        public DateTime JoinDate { get; set; }
        public DateTime? LeaveDate { get; set; }
        public MemberStatus Status { get; set; }
        public MemberRole Role { get; set; }
        public long? ClubId { get; set; }
        public long StudentId { get; set; }
        public virtual Club? Club { get; set; }
        public virtual Student? Student { get; set; }
        public virtual ICollection<ClubBoard>? ClubBoards { get; set; }
        public virtual ICollection<Participant>? ParticipatedActivities { get; set; }
        public Membership() { }
        public Membership(long clubId, long studentId) 
        {
            Id = long.Parse(clubId + "" + studentId);
            ClubId = clubId;
            StudentId = studentId;
        }
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
