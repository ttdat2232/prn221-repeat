using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Repository.Models;
using Domain.Dtos;
using Domain.Interfaces.Services;
using ClubMembership.Attributes.Auth;

namespace ClubMembership.Pages.Admin.Clubs
{
    [Auth("ADMIN")]
    public class IndexModel : PageModel
    {
        private readonly IClubService clubService;
        public IndexModel(IClubService clubService)
        {
            this.clubService = clubService;
        }

        public IList<ClubDto> Club { get;set; } = default!;

        public async Task OnGetAsync()
        {
            await clubService.GetClubsAsync(pageSize: 100).ContinueWith(t =>
            {
                Club = t.Result.Values.ToList();
            });
        } 
    }
}
