using ClubMembership.Attributes.Auth;
using Domain.Dtos;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClubMembership.Pages.Admin.Clubs
{
    [Auth("ADMIN")]
    public class IndexModel : PageModel
    {
        private readonly IClubService clubService;
        public IndexModel(IClubService clubService)
        {
            this.clubService = clubService;
        }

        public IList<ClubDto> Club { get; set; } = default!;

        public async Task OnGetAsync()
        {
            await clubService.GetClubsAsync(pageSize: 100).ContinueWith(t =>
            {
                Club = t.Result.Values.ToList();
            });
        }
    }
}
