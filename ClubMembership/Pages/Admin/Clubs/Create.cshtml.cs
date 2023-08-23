using ClubMembership.Attributes.Auth;
using Domain.Dtos.Creates;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClubMembership.Pages.Admin.Clubs
{
    [Auth("ADMIN")]
    public class CreateModel : PageModel
    {
        private readonly IClubService clubService;
        private readonly IStudentService studentService;

        public CreateModel(IClubService clubService, IStudentService studentService)
        {
            this.clubService = clubService;
            this.studentService = studentService;
        }

        public async Task<IActionResult> OnGet()
        {
            var students = await studentService.GetAllStudents()
                .ContinueWith(t => t.Result.Values.ToDictionary(k => k.Id, v => $"ID: {v.Id} | {v.Name}"));
            Students = new SelectList(students, "Key", "Value");
            return Page();
        }
        [BindProperty]
        public ClubCreateDto Club { get; set; } = default!;
        [BindProperty]
        public IFormFile? Image { get; set; }
        public SelectList Students { get; set; } = default!;
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (Image != null)
            {
                using (var imageFileStream = Image.OpenReadStream())
                {
                    Club.Image = new byte[imageFileStream.Length];
                    imageFileStream.Read(Club.Image, 0, (int)imageFileStream.Length);
                }
            }
            var result = await clubService.AddClubAsync(Club);
            if (result != null)
            {
                TempData["Notification"] = "Successfully";
                return Redirect("./");
            }
            else
                return Page();
        }
    }
}
