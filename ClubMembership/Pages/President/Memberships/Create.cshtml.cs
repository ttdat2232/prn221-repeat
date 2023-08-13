using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Domain.Entities;
using Repository.Models;
using Domain.Dtos.Creates;
using Domain.Interfaces.Services;
using Domain.Dtos;

namespace ClubMembership.Pages.President.Memberships
{
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
        public long ClubId { get; set; }
        [BindProperty]
        public MembershipCreateDto Membership { get; set; } = new MembershipCreateDto();
        [BindProperty]
        public List<long>? CheckedClubBoards { get; set; }
        public List<SelectListItem>? Students { get; set; }
        public List<SelectListItem>? Roles { get; set; }
        public async Task<IActionResult> OnGet()
        {
            //TODO : Get Club's ID via logged in user
            ClubId = 7;
            Club = await clubService.GetClubByIdAsync(ClubId);
            Students = await studentService.GetAllStudent()
                .ContinueWith(t => t.Result.Values.Select(s => new SelectListItem { Value = s.Id.ToString(), Text = $"ID: {s.Id} | {s.Name}" }).ToList());
            Roles = Enum.GetValues<MemberRole>().Select(role => new SelectListItem { Value = role.ToString(), Text = role.ToString() }).ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Membership.ClubBoardIds = CheckedClubBoards;
            if(ModelState.IsValid)
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
