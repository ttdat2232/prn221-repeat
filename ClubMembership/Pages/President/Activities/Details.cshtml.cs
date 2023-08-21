using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Domain.Dtos;
using Domain.Interfaces.Services;
using ClubMembership.Attributes.Auth;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClubMembership.Pages.President.Activities
{
    [Auth("PRESIDENT")]
    public class DetailsModel : PageModel
    {
        private readonly IClubActivityService clubActivityService;
        private readonly IMembershipService membershipService;
        private readonly IParticipantService participantService;
        private readonly ILogger<DetailsModel> logger;
        public DetailsModel(IClubActivityService clubActivityService, IParticipantService participantService, IMembershipService membershipService, ILogger<DetailsModel> logger)
        {
            this.clubActivityService = clubActivityService;
            this.participantService = participantService;
            this.membershipService = membershipService;
            this.logger = logger;
        }

        public SelectList Members { get; set; }
        public ClubActivityDto ClubActivity { get; set; } = default!;
        [BindProperty]
        public List<long>? NewMemberId { get; set; } = new List<long>();
        [BindProperty]
        public long ActivityId { get; set; }
        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            long clubId = HttpContext.Session.GetInt32("CLUBID").Value;
            var clubactivity = await clubActivityService.GetClubActivityByIdAsync(id.Value);
            ClubActivity = clubactivity;
            ActivityId = id.Value;
            await membershipService.GetMembershipByClubIdAsync(clubId).ContinueWith(t =>
            {
                if (t.Result.Values.Any())
                {
                    var existedMember = clubactivity.Participants.Select(p => p.Membership?.Id).ToList();
                    Dictionary<long, string> memberships = t.Result.Values.Where(m => !existedMember.Any(e => e.HasValue && e.Value == m.Id)).ToDictionary(k => k.Id, v => $"ID {v.Id} : {v.Name}");
                    Members = new SelectList(items: memberships, dataValueField: "Key", dataTextField: "Value");
                }
            });
            if (clubactivity == null)
            {
                return NotFound();
            }
            if(clubactivity.Club != null &&  clubactivity.Club.Id != HttpContext.Session.GetInt32("CLUBID").Value)
            {
                TempData["Error"] = "You are not allowed";
                return Redirect("/");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAddParticipant()
        {
            if(NewMemberId != null  && NewMemberId.Any())
                foreach(var mem in NewMemberId)
                    await participantService.AddParticipantAsync(mem, ActivityId);
            TempData["Notification"] = "Successfully";
            return RedirectToPage("./details", new { id = ActivityId });
        }

        public async Task<IActionResult> OnGetDeleteParticipant(long memberId, long activityId)
        {
            var activity = await clubActivityService.GetClubActivityByIdAsync(activityId);
            var clubId = HttpContext.Session.GetInt32("CLUBID").Value;
            if(activity.Club != null && clubId != activity.Club.Id)
            {
                TempData["Error"] = "You are not allowed to edit activity of another club";
                return Redirect("./");
            }
            await participantService.DeleteParticipantAsync(memberId, activityId);
            TempData["Notification"] = "Successfully";
            return RedirectToPage("./details", new { id = activityId });
        }
    }
}
