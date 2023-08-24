using ClubMembership.Attributes.Auth;
using Domain.Dtos;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClubMembership.Pages.President.ClubBoards
{
    [Auth("PRESIDENT")]
    public class DeleteModel : PageModel
    {
        private readonly IClubBoardService clubBoardService;
        public DeleteModel(IClubBoardService clubBoardService)
        {
            this.clubBoardService = clubBoardService;
        }

        [BindProperty]
        public ClubBoardDto ClubBoard { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            var clubId = HttpContext.Session.GetInt32("CLUBID");
            if (id == null || !clubId.HasValue)
            {
                return NotFound();
            }
            ClubBoard = await clubBoardService.GetClubBoardByIdAsync(id.Value);
            if (ClubBoard.ClubId != clubId.Value)
            {
                TempData["Error"] = "Not allowed";
                return RedirectToPage("./Index");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(long? id)
        {
            var clubId = HttpContext.Session.GetInt32("CLUBID");
            if (id == null || !clubId.HasValue)
            {
                return NotFound();
            }
            ClubBoard = await clubBoardService.GetClubBoardByIdAsync(id.Value);
            if (ClubBoard.ClubId != clubId.Value)
            {
                TempData["Error"] = "Not allowed";
                return RedirectToPage("./Index");
            }
            await clubBoardService.DeleteClubBoardByIdAsync(id.Value);
            TempData["Notification"] = "Successfully";
            return RedirectToPage("./Index");
        }
    }
}
