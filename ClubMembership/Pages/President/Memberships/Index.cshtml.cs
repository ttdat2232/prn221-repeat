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
using Domain.Models;
using ClubMembership.Attributes.Auth;

namespace ClubMembership.Pages.President.Memberships
{
    [Auth(allowRoles: "PRESIDENT")]
    public class IndexModel : PageModel
    {
        private readonly IMembershipService membershipService;

        public IndexModel(IMembershipService membershipService)
        {
            this.membershipService = membershipService;
        }

        public IList<MembershipDto> Membership {  get;set; } = new List<MembershipDto>();
        public PaginationResult<MembershipDto> PaginationResult { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            long clubId = HttpContext.Session.GetInt32("CLUBID").Value;
            PaginationResult = await membershipService.GetMembershipByClubIdAsync(clubId);
            Membership = PaginationResult.Values;
            return Page();
        }
    }
}
