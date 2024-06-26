using ImageCollector.Domain.Entities;
using ImageCollector.Domain.Interfaces;
using ImageCollector.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ImageCollector.Infrastructure.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        private readonly ImageCollectorContext _context;

        public LocationRepository(ImageCollectorContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Location>> GetAllAsync()
        {
            return await _context.Locations.ToListAsync();
        }

        public async Task<Location> AddAsync(Location location)
        {
            _context.Locations.Add(location);
            await _context.SaveChangesAsync();
            return location;
        }
    }
}
