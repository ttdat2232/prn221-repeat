using Domain.Dtos.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos
{
    public class MembershipDto : BaseDto
    {
        public string Name { get; set; }
        public long StudentId { get; set; }
        public string ClubName { get; set; }
    }
}
