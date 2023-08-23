using Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Club : BaseEntity
    {
        [Column(TypeName = "nvarchar(50)")]
        public string Name { get; set; }
        public DateTime CreateAt { get; set; }
        public string LogoUrl { get; set; }
        public virtual ICollection<Membership>? Memberships { get; set; }
        public virtual ICollection<ClubActivity>? ClubActivities { get; set; }
        public virtual ICollection<ClubBoard>? ClubBoards { get; set; }
    }
}
