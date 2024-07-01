using ImageCollector.Application.DTOs;
using ImageCollector.Application.Services;
using ImageCollector.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ImageCollector.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LocationController : ControllerBase
    {
        private readonly LocationService _locationService;
        private readonly UserManager<ApplicationUser> _userManager;

        public LocationController(LocationService locationService, UserManager<ApplicationUser> userManager)
        {
            _locationService = locationService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetLocations()
        {   

            var userId = _userManager.GetUserId(User);
            var locations = await _locationService.GetAllLocationsAsync(userId);
            return Ok(locations);
        }

        [HttpPost]
        public async Task<IActionResult> AddLocation([FromBody] string locationName)
        {
            var userId = _userManager.GetUserId(User);
            var location = await _locationService.AddLocationAsync(locationName, userId);
            return CreatedAtAction(nameof(GetLocations), new { id = location.Id }, location);
        }



    }
}
