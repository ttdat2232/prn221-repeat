using Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class ClubBoard : BaseEntity
    {
        [Column(TypeName = "nvarchar(50)")]
        public string Name { get; set; }
        public long ClubId { get; set; }
        public virtual Club? Club { get; set; }
        public virtual ICollection<Membership>? Memberships { get; set; }
    }
}
