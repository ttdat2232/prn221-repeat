using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos
{
    public class ParticipantDto
    {
        public ClubActivityDto? CLubActivityDto { get; set; }
        public MembershipDto? Membership { get; set; }
        public ParticipantStatus Status { get; set; }
    }
}
