using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos.Creates
{
    public class ClubBoardCreateDto
    {
        [Required, MinLength(1), MaxLength(50)]
        public string Name { get; set; }
        public long? ClubId { get; set; }
        public List<long>? MembershipIds { get; set; }
    }
}
