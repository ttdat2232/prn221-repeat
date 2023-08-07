using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Base;

namespace Domain.Entities
{
    public class Club : BaseEntity
    {
        [Column(TypeName = "nvarchar(50)")]
        public string Name { get; set; }
        public DateOnly CreateAt { get; set; }
        public virtual ICollection<Membership>? Memberships { get; set; }
        public virtual ICollection<ClubActivity>? ClubActivities { get; set; }
    }
}
