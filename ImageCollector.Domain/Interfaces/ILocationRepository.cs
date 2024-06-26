using ImageCollector.Domain.Entities;

namespace ImageCollector.Domain.Interfaces
{
    public interface ILocationRepository
    {
        Task<IEnumerable<Location>> GetAllAsync();
        Task<Location> AddAsync(Location location);
    }
}
