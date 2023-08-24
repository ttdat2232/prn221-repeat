using ClubMembership.Attributes.Auth;
using Domain.Dtos;
using Domain.Interfaces.Services;
using Domain.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClubMembership.Pages.President.Activities
{
    [Auth("PRESIDENT")]
    public class IndexModel : PageModel
    {
        private readonly IClubActivityService clubActivityService;
        public IndexModel(IClubActivityService clubActivityService)
        {
            this.clubActivityService = clubActivityService;
        }

        public IList<ClubActivityDto> ClubActivity { get; set; } = default!;
        public PaginationResult<ClubActivityDto> PaginationResult { get; set; } = default!;

        public async Task OnGetAsync(int pageIndex = 0)
        {
            long clubId = HttpContext.Session.GetInt32("CLUBID").Value;
            PaginationResult = await clubActivityService.GetAllClubActivitiesByClubIdAsync(clubId);
            ClubActivity = PaginationResult.Values.ToList();
        }
    }
}
