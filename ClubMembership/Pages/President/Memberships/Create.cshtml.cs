using ClubMembership.Attributes.Auth;
using Domain.Dtos;
using Domain.Dtos.Creates;
using Domain.Entities;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClubMembership.Pages.President.Memberships
{
    [Auth(allowRoles: "PRESIDENT")]
    public class CreateModel : PageModel
    {
        private readonly IMembershipService membershipService;
        private readonly IClubService clubService;
        private readonly IStudentService studentService;
        private readonly ILogger<CreateModel> logger;
        public CreateModel(IMembershipService membershipService, IClubService clubService, IStudentService studentService, ILogger<CreateModel> logger)
        {
            this.membershipService = membershipService;
            this.clubService = clubService;
            this.studentService = studentService;
            this.logger = logger;
        }

        public ClubDto? Club { get; set; }
        public long clubId { get; set; }
        [BindProperty]
        public MembershipCreateDto Membership { get; set; } = new MembershipCreateDto();
        [BindProperty]
        public List<long>? CheckedClubBoards { get; set; }
        public List<SelectListItem>? Students { get; set; }
        public List<SelectListItem>? Roles { get; set; }
        public async Task<IActionResult> OnGet()
        {
            clubId = HttpContext.Session.GetInt32("CLUBID").Value;
            Club = await clubService.GetClubByIdAsync(clubId);
            var studentIdInClub = Club.Memberships?.Where(m => m.Status == MemberStatus.JOIN).Select(m => m.StudentId).ToList();
            await studentService.GetAllStudents()
                .ContinueWith(t =>
                {
                    Students = t.Result.Values.Where(s => studentIdInClub != null && !studentIdInClub.Any(e => e == s.Id)).Select(s => new SelectListItem { Value = s.Id.ToString(), Text = $"ID: {s.Id} | {s.Name}" }).ToList();
                });
            Roles = Enum.GetValues<MemberRole>().Where(r => r != MemberRole.PRESIDENT).Select(role => new SelectListItem { Value = role.ToString(), Text = role.ToString() }).ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Membership.ClubBoardIds = CheckedClubBoards;
            if (ModelState.IsValid)
            {
                var result = await membershipService.AddMemberShipAsync(Membership);
                if (result != null)
                    TempData["Notification"] = "Successfully";
            }
            else
            {
                return Redirect("./create");
            }
            return Redirect("./");
        }
    }
}
