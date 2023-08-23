using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Grade
    {
        [Column(TypeName = "nvarchar(10)")]
        public string Id { get; set; }
        public DateTime StartAt { get; set; }
        public ICollection<Student>? Students { get; set; }
    }
}
