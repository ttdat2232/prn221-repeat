using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Build.Framework;

namespace ClubMembership.Pages.Auth
{
    [BindProperties]
    public class LoginModel : PageModel
    {
        private readonly IAuthenticateService authenticateService;

        public LoginModel(IAuthenticateService authenticateService)
        {
            this.authenticateService = authenticateService;
        }

        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        public async Task<IActionResult> OnPostAsync()
        {
            var user = await authenticateService.LoginAsync(Username, Password);
            if (user.IsAdmin)
            {
                HttpContext.Session.SetString("ROLE", "ADMIN");
                HttpContext.Session.SetString("CLUBNAME", "ADMIN");
                return Redirect("/admin/clubs");
            }
            else
            {
                HttpContext.Session.SetString("ROLE", "PRESIDENT");
                HttpContext.Session.SetInt32("CLUBID", (int)user.ClubId);
                HttpContext.Session.SetString("CLUBNAME", user.ClubName);
                HttpContext.Session.SetString("CLUBLOGO", user.ClubLogo);
                return Redirect("/president/clubs");
            }
        }
    }
}
