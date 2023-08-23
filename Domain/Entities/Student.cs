using Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Student : BaseEntity
    {
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        public StudentStatus Status { get; set; }
        public long MajorId { get; set; }
        public string GradeId { get; set; }
        public virtual Major? Major { get; set; }
        public virtual Grade? Grade { get; set; }
        public virtual ICollection<Membership>? Memberships { get; set; }
    }

    public enum StudentStatus
    {
        IN_PROGRESS = 0,
        FINISH = 1,
    }
}
