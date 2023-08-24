using ClubMembership.Attributes.Auth;
using Domain.Dtos;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClubMembership.Pages.President.ClubBoards
{
    [Auth("PRESIDENT")]
    public class DetailsModel : PageModel
    {
        private readonly IClubBoardService clubBoardService;
        private readonly IMembershipService membershipService;
        public DetailsModel(IClubBoardService clubBoardService, IMembershipService membershipService)
        {
            this.clubBoardService = clubBoardService;
            this.membershipService = membershipService;
        }
        public ClubBoardDto ClubBoard { get; set; } = default!;
        public SelectList Members { get; set; } = default!;
        [BindProperty]
        public List<long> NewMemberId { get; set; } = new List<long>();
        [BindProperty]
        public long ClubBoardId { get; set; }
        public async Task<IActionResult> OnGetAsync(long? id)
        {
            long? clubId = HttpContext.Session.GetInt32("CLUBID");
            if (id == null || !clubId.HasValue)
            {
                return NotFound();
            }
            ClubBoardId = id.Value;
            ClubBoard = await clubBoardService.GetClubBoardByIdAsync(id.Value);
            var members = await membershipService.GetMembershipByClubIdAsync(clubId.Value, pageSize: 100);
            if (members.Values.Any())
            {
                if (ClubBoard.MembershipDtos.Any())
                {
                    var existedMembers = ClubBoard.MembershipDtos.Select(s => s.Id).ToList();
                    Dictionary<long, string> selectMemberMap = members.Values.Where(m => !existedMembers.Any(e => e == m.Id)).ToDictionary(k => k.Id, v => $"ID: {v.Id} | {v.Name}");
                    Members = new SelectList(items: selectMemberMap, dataTextField: "Value", dataValueField: "Key");
                }
                else
                {
                    Dictionary<long, string> selectMemberMap = members.Values.ToDictionary(k => k.Id, v => $"ID: {v.Id} | {v.Name}");
                    Members = new SelectList(items: selectMemberMap, dataTextField: "Value", dataValueField: "Key");
                }
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAddMembersAsync()
        {
            long? clubId = HttpContext.Session.GetInt32("CLUBID");
            if (!ModelState.IsValid)
                return Page();
            if (!clubId.HasValue)
                return NotFound();
            var clubBoard = await clubBoardService.GetClubBoardByIdAsync(ClubBoardId);
            if (clubBoard.ClubId != clubId.Value)
                return NotFound();

            await clubBoardService.AddMembersToBoard(clubBoard.Id, NewMemberId);
            TempData["Notification"] = "Successfully";
            return RedirectToPage("./Details", new { id = ClubBoardId });
        }

        public async Task<IActionResult> OnGetRemoveMemberAsync(long memberId, long clubBoardId)
        {
            long? clubId = HttpContext.Session.GetInt32("CLUBID");
            if (!clubId.HasValue)
                return NotFound();
            var clubBoard = await clubBoardService.GetClubBoardByIdAsync(clubBoardId);
            if (clubBoard.ClubId != clubId.Value)
                return NotFound();
            await clubBoardService.RemoveMemberFromBoard(memberId, clubBoardId);
            TempData["Notification"] = "Successfully";
            return RedirectToPage("./Details", new { id = clubBoardId });
        }
    }
}
