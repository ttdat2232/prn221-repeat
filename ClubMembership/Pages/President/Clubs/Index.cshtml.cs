using ClubMembership.Attributes.Auth;
using Domain.Dtos;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClubMembership.Pages.President.Clubs
{
    [Auth(allowRoles: "PRESIDENT")]
    public class IndexModel : PageModel
    {
        private readonly IClubService clubService;

        public IndexModel(IClubService clubService)
        {
            this.clubService = clubService;
        }

        public ClubDto? Club { get; set; }
        public async Task OnGet()
        {
            long clubId = HttpContext.Session.GetInt32("CLUBID").Value;
            Club = await clubService.GetClubByIdAsync(clubId);
        }
    }
}
