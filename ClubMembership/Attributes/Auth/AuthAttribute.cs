using Microsoft.AspNetCore.Mvc;

namespace ClubMembership.Attributes.Auth
{
    public class AuthAttribute : TypeFilterAttribute
    {
        public AuthAttribute(string allowRoles) : base(typeof(AuthorizeAction))
        {
            Arguments = new[] { allowRoles };
        }
    }
}
