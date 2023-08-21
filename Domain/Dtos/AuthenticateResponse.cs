using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos
{
    public class AuthenticateResponse
    {
        public long UserId { get; set; }
        public long ClubId { get; set; }
        public bool IsAdmin { get; set; } = false;
    }
}
