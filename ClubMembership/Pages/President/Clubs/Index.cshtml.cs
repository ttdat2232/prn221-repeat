using Domain.Dtos;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClubMembership.Pages.President.Clubs
{
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
            //TODO: get club's ID via president that logged in system
            Club = await clubService.GetClubByIdAsync(7);
        }
    }
}
