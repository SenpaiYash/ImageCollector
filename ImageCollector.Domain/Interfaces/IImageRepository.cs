using ImageCollector.Domain.Entities;

namespace ImageCollector.Domain.Interfaces
{
    public interface IImageRepository
    {
        Task<IEnumerable<Image>> GetAllAsync();
        Task<IEnumerable<Image>> GetByLocationAsync(string location);
        Task<Image> AddAsync(Image image);
        Task<bool> ExistsAsync(string imageUrl);
    }
}
