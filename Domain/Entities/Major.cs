using Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Major : BaseEntity
    {
        [Column(TypeName = "nvarchar(50)")]
        public string Name { get; set; }
        public virtual ICollection<Student>? Students { get; set; }
    }
}
