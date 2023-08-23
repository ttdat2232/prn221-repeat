using Domain.Dtos;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClubMembership.Pages.President.Activities
{
    public class DeleteModel : PageModel
    {

        private readonly IClubActivityService clubActivityService;
        public DeleteModel(IClubActivityService clubActivityService)
        {
            this.clubActivityService = clubActivityService;
        }

        [BindProperty]
        public ClubActivityDto ClubActivity { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ClubActivity = await clubActivityService.GetClubActivityByIdAsync(id.Value);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            await clubActivityService.DeleteActivityByIdAsync(id.Value);
            TempData["Notification"] = "Successfully";
            return Redirect("./");
        }
    }
}
