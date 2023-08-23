using ClubMembership.Attributes.Auth;
using Domain.Dtos;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClubMembership.Pages.President.Memberships
{
    [Auth(allowRoles: "PRESIDENT")]
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
            if (!id.HasValue)
                return NotFound();
            Membership = await membershipService.GetMemberShipByIdAsync(id.Value);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(long? id)
        {
            if (!id.HasValue)
                return NotFound();
            await membershipService.DeleteMembershipAsync(id.Value);
            TempData["Notification"] = "Successfully";
            return Redirect("./");
        }
    }
}
