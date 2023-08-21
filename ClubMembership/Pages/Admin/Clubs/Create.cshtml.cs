using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Domain.Entities;
using Repository.Models;
using Domain.Interfaces.Services;
using Domain.Dtos.Creates;
using ClubMembership.Attributes.Auth;

namespace ClubMembership.Pages.Admin.Clubs
{
    [Auth("ADMIN")]
    [BindProperties]
    public class CreateModel : PageModel
    {
        private readonly IClubService clubService;


        public CreateModel(IClubService clubService)
        {
            this.clubService = clubService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public ClubCreateDto Club { get; set; } = default!;
        public IFormFile Image { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }
            if(Image != null) 
            { 
                using (var imageFileStream = Image.OpenReadStream())
                {
                    Club.Image = new byte[imageFileStream.Length];
                    imageFileStream.Read(Club.Image, 0, (int)imageFileStream.Length);
                }
            }
            var result = await clubService.AddClubAsync(Club);
            if(result != null)
            {
                TempData["Notification"] = "Successfully";
                return Redirect("./");
            }
            else 
                return Page();
        }
    }
}
