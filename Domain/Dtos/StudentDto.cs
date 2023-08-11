using Domain.Dtos.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos
{
    public class StudentDto : BaseDto
    {
        public string Name { get; set; } = string.Empty;
    }
}
