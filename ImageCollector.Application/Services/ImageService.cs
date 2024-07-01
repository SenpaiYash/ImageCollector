using ImageCollector.Application.DTOs;
using ImageCollector.Domain.Entities;
using ImageCollector.Domain.Interfaces;
using Microsoft.Extensions.Logging;


namespace ImageCollector.Application.Services
{
    public class ImageService
    {
        private readonly IImageRepository _imageRepository;
        private readonly FourSquareService _fourSquareService;
        private readonly FlickrService _flickrService;
        private readonly ILogger<ImageService> _logger;

        public ImageService(IImageRepository imageRepository, FourSquareService fourSquareService, FlickrService flickrService, ILogger<ImageService> logger)
        {
            _imageRepository = imageRepository;
            _fourSquareService = fourSquareService;
            _flickrService = flickrService;
            _logger = logger;
        }

        public async Task<IEnumerable<ImageDto>> GetImagesByLocationAsync(string location)
        {
            var images = await _imageRepository.GetByLocationAsync(location);
            return images.Select(img => new ImageDto
            {
                Location = img.Location,
                ImageUrl = img.ImageUrl,
                Description = img.Description
            });
        }

        public async Task<ImageDto> AddImageAsync(ImageDto imageDto)
        {
            if (await _imageRepository.ExistsAsync(imageDto.ImageUrl))
            {
                throw new InvalidOperationException("Image already exists");
            }

            var image = new Image
            {
                Location = imageDto.Location,
                ImageUrl = imageDto.ImageUrl,
                Description = imageDto.Description,
                DateAdded = DateTime.UtcNow
            };

            var addedImage = await _imageRepository.AddAsync(image);
            return new ImageDto
            {
                Location = addedImage.Location,
                ImageUrl = addedImage.ImageUrl,
                Description = addedImage.Description
            };
        }
                
        
    }
}
