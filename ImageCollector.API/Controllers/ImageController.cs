using ImageCollector.Application.DTOs;
using ImageCollector.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ImageCollector.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController: ControllerBase
    {
        private readonly ImageService _imageService;

        public ImageController(ImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpGet("{location}")]
        public async Task<IActionResult> GetImages(string location, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var images = await _imageService.GetImagesByLocationAsync(location);
            var paginatedImages = images.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return Ok(new { data = paginatedImages, totalPages = (int)Math.Ceiling((double)images.Count() / pageSize) });
        }

        [HttpPost]
        public async Task<IActionResult> AddImage([FromBody] ImageDto imageDto)
        {
            var image = await _imageService.AddImageAsync(imageDto);
            return CreatedAtAction(nameof(GetImages), new { location = image.Location }, image);
        }
    }
}
