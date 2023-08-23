using ClubMembership.Attributes.Auth;
using Domain.Dtos;
using Domain.Interfaces.Services;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClubMembership.Pages.President.Memberships
{
    [Auth(allowRoles: "PRESIDENT")]
    public class IndexModel : PageModel
    {
        private readonly IMembershipService membershipService;

        public IndexModel(IMembershipService membershipService)
        {
            this.membershipService = membershipService;
        }

        public IList<MembershipDto> Membership { get; set; } = new List<MembershipDto>();
        public PaginationResult<MembershipDto> PaginationResult { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            long clubId = HttpContext.Session.GetInt32("CLUBID").Value;
            PaginationResult = await membershipService.GetMembershipByClubIdAsync(clubId);
            Membership = PaginationResult.Values;
            return Page();
        }
    }
}
