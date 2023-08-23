using ClubMembership.Attributes.Auth;
using Domain.Dtos;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClubMembership.Pages.President.ClubBoards
{
    [Auth("PRESIDENT")]
    public class IndexModel : PageModel
    {
        private readonly IClubBoardService clubBoardService;
        public IndexModel(IClubBoardService clubBoardService)
        {
            this.clubBoardService = clubBoardService;
        }

        public IList<ClubBoardDto> ClubBoard { get; set; } = default!;
        public async Task OnGetAsync()
        {
            long? clubId = HttpContext.Session.GetInt32("CLUBID");
            if (clubId.HasValue)
            {
                var result = await clubBoardService.GetClubBoardsByClubIdAsync(clubId.Value);
                ClubBoard = result.Values.ToList();
            }
        }
    }
}
