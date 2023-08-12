using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Repository.Models;
using Domain.Dtos.Updates;
using Domain.Interfaces.Services;

namespace ClubMembership.Pages.President.Memberships
{
    public class EditModel : PageModel
    {
        private readonly IMembershipService membershipService;
        private readonly ILogger<EditModel> logger;
        public EditModel(IMembershipService membershipService, ILogger<EditModel> logger)
        {
            this.membershipService = membershipService;
            this.logger = logger;
        }

        [BindProperty]
        public MembershipUpdateDto Membership { get; set; } = default!;
        public string Name { get; set; } = string.Empty;
        public List<SelectListItem>? Roles { get; set; }
        public List<SelectListItem>? Status { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (!id.HasValue)
                return NotFound();
            var result = await membershipService.GetMemberShipByIdAsync(id.Value);
            Name = result.Name;
            Membership = new MembershipUpdateDto
            {
                Id = result.Id,
                Status = result.Status,
                LeaveDate = result.LeaveDate,
                JoinDate = result.JoinDate,
                Role = result.Role,
            };
            Status = Enum.GetValues<MemberStatus>().Select(s => new SelectListItem { Text = s.ToString(), Value = s.ToString(), Selected = s.Equals(Membership.Status)}).ToList();
            Roles = Enum.GetValues<MemberRole>().Select(r => new SelectListItem { Text = r.ToString(), Value = r.ToString(), Selected = r.Equals(Membership.Role)}).ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var result = await membershipService.UpdateMembershipAsync(Membership);
                if (result != null)
                    TempData["Notification"] = "Successfully";
                return RedirectToPage("./Edit", new { id = Membership.Id });
            }
            else
            {
                return RedirectToPage("./Edit", new { id = Membership.Id });
            }
        }
    }
}
