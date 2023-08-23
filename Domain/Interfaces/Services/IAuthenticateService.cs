using Domain.Dtos;

namespace Domain.Interfaces.Services
{
    public interface IAuthenticateService
    {
        Task<AuthenticateResponse> LoginAsync(string username, string password);
    }
}
