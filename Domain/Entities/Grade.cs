using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
