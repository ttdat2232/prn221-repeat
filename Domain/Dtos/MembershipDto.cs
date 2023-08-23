using Domain.Dtos.Base;
using Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos
{
    public class MembershipDto : BaseDto
    {
        public string Name { get; set; }
        public long StudentId { get; set; }
        [Display(Name = "Club Mame")]
        public string ClubName { get; set; }
        public MemberStatus Status { get; set; }
        public DateTime JoinDate { get; set; }
        public DateTime LeaveDate { get; set; }
        public MemberRole Role { get; set; }
    }
}
