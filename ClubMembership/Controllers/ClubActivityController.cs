using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ClubMembership.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClubActivityController : ControllerBase
    {
        private readonly IClubActivityService clubActivityService;
        private readonly ILogger<ClubActivityController> logger;
        public ClubActivityController(IClubActivityService clubActivityService, ILogger<ClubActivityController> logger)
        {
            this.clubActivityService = clubActivityService;
            this.logger = logger;
        }

        [HttpGet("Status")]
        public async Task<IActionResult> UpdateAllClubActivitiesStatus()
        {
            if (!IsLocal())
            {
                logger.LogInformation("Not local access");
                return BadRequest();
            }
            logger.LogInformation("Local access");
            await clubActivityService.UpdateStatusAllActivityAsync();
            return Ok(new { Message = "Success" });
        }

        private bool IsLocal()
        {
            if (HttpContext.Connection.RemoteIpAddress != null && (HttpContext.Connection.RemoteIpAddress.Equals(HttpContext.Connection.LocalIpAddress) || IPAddress.IsLoopback(HttpContext.Connection.RemoteIpAddress)))
                return true;
            return false;
        }
    }
}
