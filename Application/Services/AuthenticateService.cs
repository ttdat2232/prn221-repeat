using Application.Exceptions;
using Domain.Dtos;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;

namespace Application.Services
{
    public class AuthenticateService : IAuthenticateService
    {
        private readonly IUnitOfWork unitOfWork;

        public AuthenticateService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<AuthenticateResponse> LoginAsync(string username, string password)
        {
            if (password != "1")
                throw new AppException("Wrong username or password");
            if (username.ToLower().Equals("admin"))
                return new AuthenticateResponse
                {
                    ClubId = 0,
                    UserId = 0,
                    IsAdmin = true,
                };
            var membership = await unitOfWork.Memberships.GetAsync(expression: m => m.Student != null && m.Student.Name.ToLower().Equals(username.ToLower()) && m.Role == Domain.Entities.MemberRole.PRESIDENT, include: new string[] { nameof(Membership.Club) })
                .ContinueWith(t => t.Result.Values.Any() ? t.Result.Values.First() : throw new AppException("Wrong username or password"));
            return new AuthenticateResponse
            {
                ClubId = membership.ClubId.HasValue ? membership.ClubId.Value : 0,
                UserId = membership.Id,
                ClubLogo = membership.Club?.LogoUrl ?? "",
                ClubName = membership.Club?.Name ?? "",
            };
        }
    }
}
