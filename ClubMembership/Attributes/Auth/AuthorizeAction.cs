using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;

namespace ClubMembership.Attributes.Auth
{
    public class AuthorizeAction : IAuthorizationFilter
    {
        private readonly string roles;

        public AuthorizeAction(string roles)
        {
            this.roles = roles;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var allowRoles = roles.Split(',', ' ');
            if (context.HttpContext.Session.TryGetValue("ROLE", out var sessionRole) is false)
            {
                context.Result = new RedirectResult("/auth/login");
            }
            else
            {
                if (allowRoles.Any(allowRole => Encoding.UTF8.GetString(sessionRole).CompareTo(allowRole) == 0) is false)
                {
                    context.Result = new RedirectResult("/auth/login");
                }
            }
        }
    }
}
