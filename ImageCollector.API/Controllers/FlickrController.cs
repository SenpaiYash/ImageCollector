using ImageCollector.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ImageCollector.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlickrController : ControllerBase
    {
        private readonly IFlickrService _flickrService;

        public FlickrController(IFlickrService flickrService)
        {
            _flickrService = flickrService;
        }

        [HttpGet("image")]
        public async Task<IActionResult> GetImage(string tags)
        {
            var imageUrl = await _flickrService.GetImageUrlAsync(tags);
            if (imageUrl == null)
            {
                return NotFound("No images found.");
            }
            return Ok(new { Url = imageUrl });
        }
    }
}
