using ClubMembership.Attributes.Auth;
using Domain.Dtos.Creates;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClubMembership.Pages.President.ClubBoards
{
    [Auth("PRESIDENT")]
    public class CreateModel : PageModel
    {
        private readonly IClubBoardService clubBoardService;
        private readonly IMembershipService membershipService;
        public CreateModel(IClubBoardService clubBoardService, IMembershipService membershipService)
        {
            this.clubBoardService = clubBoardService;
            this.membershipService = membershipService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var clubId = HttpContext.Session.GetInt32("CLUBID");
            if (!clubId.HasValue) return NotFound();
            var members = await membershipService.GetMembershipByClubIdAsync(clubId.Value)
                .ContinueWith(t => t.Result.Values.ToDictionary(k => k.Id, v => $"ID: {v.Id} | {v.Name}"));
            Members = new SelectList(members, "Key", "Value");
            ClubId = clubId.Value;
            return Page();
        }

        [BindProperty]
        public ClubBoardCreateDto ClubBoard { get; set; } = default!;
        public SelectList Members { get; set; } = default!;
        public long ClubId { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || ClubBoard == null)
            {
                return Page();
            }
            await clubBoardService.AddClubBoardAsync(ClubBoard);
            TempData["Notification"] = "Successfully";
            return RedirectToPage("./Index");
        }
    }
}
