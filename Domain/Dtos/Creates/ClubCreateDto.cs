using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos.Creates
{
    public class ClubCreateDto
    {
        [Required]
        [MinLength(1), MaxLength(50)]
        public string Name { get; set; }
        public byte[]? Image { get; set; }
    }
}
