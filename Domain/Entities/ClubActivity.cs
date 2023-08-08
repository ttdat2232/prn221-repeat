using Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ClubActivity : BaseEntity
    {
        public long? ClubId { get; set; }
        public ActivityStatus Status { get; set;}
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public virtual Club? Club { get; set; }
        public virtual ICollection<Participant>? Participants { get; set; }
    }

    public enum ActivityStatus
    {
        UNSTART = 0,
        START = 1,
        END = 2,
    }
}
