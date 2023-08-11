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

namespace ClubMembership.Pages.President.Memberships
{
    public class IndexModel : PageModel
    {
        private readonly IMembershipService membershipService;

        public IndexModel(IMembershipService membershipService)
        {
            this.membershipService = membershipService;
        }

        public IList<MembershipDto> Membership { get;set; } = new List<MembershipDto>();
        public PaginationResult<MembershipDto> PaginationResult { get; set; }

        public async Task OnGetAsync()
        {
            PaginationResult = await membershipService.GetMembershipByNameAsync();
            Membership = PaginationResult.Values;
        }
    }
}
