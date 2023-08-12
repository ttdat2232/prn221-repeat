using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Repository.Models;
using Domain.Interfaces.Services;
using Domain.Dtos;

namespace ClubMembership.Pages.President.Memberships
{
    public class DeleteModel : PageModel
    {
        private readonly IMembershipService membershipService;

        public DeleteModel(IMembershipService membershipService)
        {
            this.membershipService = membershipService;
        }

        [BindProperty]
        public MembershipDto Membership { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if(!id.HasValue)
                return NotFound();
            Membership = await membershipService.GetMemberShipByIdAsync(id.Value);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(long? id)
        {
            if(!id.HasValue)
                return NotFound();
            await membershipService.DeleteMembershipAsync(id.Value);
            TempData["Notification"] = "Successfully";
            return Redirect("./");
        }
    }
}
