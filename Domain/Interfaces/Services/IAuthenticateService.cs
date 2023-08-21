using Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Services
{
    public interface IAuthenticateService
    {
        Task<AuthenticateResponse> LoginAsync(string username, string password);
    }
}
