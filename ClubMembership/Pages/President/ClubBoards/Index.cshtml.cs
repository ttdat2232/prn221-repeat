using ClubMembership.Attributes.Auth;
using Domain.Dtos;
using Domain.Interfaces.Services;
using Domain.Models;
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
        public PaginationResult<ClubBoardDto> PaginationResult { get; set; } = default!;
        public async Task OnGetAsync(int pageIndex = 0)
        {
            long? clubId = HttpContext.Session.GetInt32("CLUBID");
            if (clubId.HasValue)
            {
                PaginationResult = await clubBoardService.GetClubBoardsByClubIdAsync(clubId.Value);
                ClubBoard = PaginationResult.Values.ToList();
            }
        }
    }
}
