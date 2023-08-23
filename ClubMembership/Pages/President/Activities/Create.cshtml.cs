using ClubMembership.Attributes.Auth;
using Domain.Dtos.Creates;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClubMembership.Pages.President.Activities
{
    [Auth("PRESIDENT")]
    public class CreateModel : PageModel
    {
        private readonly IClubActivityService clubActivityService;
        private readonly IMembershipService membershipService;
        private readonly ILogger<CreateModel> logger;
        public CreateModel(IClubActivityService clubActivityService, IMembershipService membershipService, ILogger<CreateModel> logger)
        {
            this.clubActivityService = clubActivityService;
            this.membershipService = membershipService;
            this.logger = logger;
        }

        public SelectList Participants { get; set; } = default!;
        public async Task<IActionResult> OnGetAsync()
        {
            long clubId = HttpContext.Session.GetInt32("CLUBID").Value;
            await membershipService.GetMembershipByClubIdAsync(clubId).ContinueWith(t =>
            {
                if (t.Result.Values.Any())
                {
                    Dictionary<long, string> memberships = t.Result.Values.ToDictionary(k => k.Id, v => $"ID {v.Id} : {v.Name}");
                    Participants = new SelectList(items: memberships, dataValueField: "Key", dataTextField: "Value");
                }
            });
            return Page();
        }

        [BindProperty]
        public ClubActivityCreateDto ClubActivity { get; set; } = default!;


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            else
            {
                var result = await clubActivityService.AddClubActivityAsync(ClubActivity);
                TempData["Notification"] = "Successfully";
                return Redirect("./");
            }
        }
    }
}
