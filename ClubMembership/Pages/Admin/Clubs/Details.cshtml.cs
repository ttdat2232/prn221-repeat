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
using ClubMembership.Attributes.Auth;

namespace ClubMembership.Pages.Admin.Clubs
{
    [Auth("ADMIN")]
    public class DetailsModel : PageModel
    {
        private readonly IClubService clubService;

        public DetailsModel(IClubService clubService)
        {
            this.clubService = clubService;
        }

        public ClubDto Club { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Club = await clubService.GetClubByIdAsync(id.Value);
            return Page();
        }
    }
}
