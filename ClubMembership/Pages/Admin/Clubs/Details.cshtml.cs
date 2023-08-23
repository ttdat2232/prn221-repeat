using ClubMembership.Attributes.Auth;
using Domain.Dtos;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClubMembership.Pages.Admin.Clubs
{
    [Auth("ADMIN")]
    public class DetailsModel : PageModel
    {
        private readonly IClubService clubService;

        public DetailsModel(IClubService clubService)
        {
            this.clubService = clubService;
        }

        public ClubDto Club { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Club = await clubService.GetClubByIdAsync(id.Value);
            return Page();
        }
    }
}
