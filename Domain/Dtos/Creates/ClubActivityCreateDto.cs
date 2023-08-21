using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos.Creates
{
    public class ClubActivityCreateDto
    {
        public long ClubId { get; set; } = 0;
        [Required, MinLength(1), MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        public ActivityStatus Status { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public List<long> ParticipatingMembersIds{ get; set; } = new List<long>();
    }
}
