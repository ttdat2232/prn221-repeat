using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Domain.Dtos;
using Domain.Interfaces.Services;
using ClubMembership.Attributes.Auth;

namespace ClubMembership.Pages.President.Activities
{
    [Auth("PRESIDENT")]
    public class IndexModel : PageModel
    {
        private readonly IClubActivityService clubActivityService;
        public IndexModel(IClubActivityService clubActivityService)
        {
            this.clubActivityService = clubActivityService;
        }

        public IList<ClubActivityDto> ClubActivity { get;set; } = default!;

        public async Task OnGetAsync()
        {
            long clubId = HttpContext.Session.GetInt32("CLUBID").Value;
            var result = await clubActivityService.GetAllClubActivitiesByClubIdAsync(clubId);
            ClubActivity = result.Values.ToList();
        }
    }
}
