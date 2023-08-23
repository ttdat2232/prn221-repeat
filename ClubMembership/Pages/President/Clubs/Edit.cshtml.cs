using ClubMembership.Attributes.Auth;
using Domain.Dtos.Updates;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace ClubMembership.Pages.President.Clubs
{
    [Auth("PRESIDENT")]
    [BindProperties]
    public class EditModel : PageModel
    {
        private readonly IClubService clubService;
        public EditModel(IClubService clubService)
        {
            this.clubService = clubService;
        }

        public ClubUpdateDto Club { get; set; } = default!;
        [Display(Name = "Logo")]
        public IFormFile? Image { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            var club = await clubService.GetClubByIdAsync(HttpContext.Session.GetInt32("CLUBID").Value);
            Club = new ClubUpdateDto
            {
                Id = club.Id,
                Name = club.Name,
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            Club.Id = HttpContext.Session.GetInt32("CLUBID").Value;
            if (Image != null)
            {
                using (var imageFileStream = Image.OpenReadStream())
                {
                    Club.Image = new byte[imageFileStream.Length];
                    imageFileStream.Read(Club.Image, 0, (int)imageFileStream.Length);
                }
            }
            await clubService.UpdateClubAsync(Club);
            TempData["Notification"] = "Successfully";
            return RedirectToPage("./Edit", new { id = Club.Id });
        }
    }
}
