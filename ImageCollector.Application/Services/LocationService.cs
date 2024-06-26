using ImageCollector.Application.DTOs;
using ImageCollector.Domain.Entities;
using ImageCollector.Domain.Interfaces;
using Microsoft.Extensions.Logging;


namespace ImageCollector.Application.Services
{
    public class LocationService
    {
        private readonly ILocationRepository _locationRepository;
        private readonly FourSquareService _fourSquareService;
        private readonly ILogger<LocationService> _logger;

        public LocationService(ILocationRepository locationRepository, FourSquareService fourSquareService, ILogger<LocationService> logger)
        {
            _locationRepository = locationRepository;
            _fourSquareService = fourSquareService;
            _logger = logger;
        }

        public async Task<IEnumerable<LocationDto>> GetAllLocationsAsync(string userId)
        {
            var locations = await _locationRepository.GetAllAsync();
            return locations.Where(loc => loc.UserId == userId).Select(loc => new LocationDto
            {
                Id = loc.Id,
                Name = loc.Name,
                Description = loc.Description,
                UserId = loc.UserId
            });
        }

        public async Task<LocationDto> AddLocationAsync(string locationName, string userId)
        {
            var locationId = await _fourSquareService.GetLocationIdAsync(locationName);
            var locationDetails = await _fourSquareService.GetLocationDetailsAsync(locationId);

            var location = new Location
            {
                Name = locationDetails.Name,
                Description = locationDetails.Description,
                UserId = userId
            };

            var addedLocation = await _locationRepository.AddAsync(location);
            return new LocationDto
            {
                Id = addedLocation.Id,
                Name = addedLocation.Name,
                Description = addedLocation.Description,
                UserId = addedLocation.UserId
            };
        }
    }
}
