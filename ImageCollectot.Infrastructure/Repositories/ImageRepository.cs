using ImageCollector.Domain.Entities;
using ImageCollector.Domain.Interfaces;
using ImageCollector.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace ImageCollector.Infrastructure.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly ImageCollectorContext _context;

        public ImageRepository(ImageCollectorContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Image>> GetAllAsync()
        {
            return await _context.Images.ToListAsync();
        }

        public async Task<IEnumerable<Image>> GetByLocationAsync(string location)
        {
            return await _context.Images.Where(i => i.Location == location).ToListAsync();
        }

        public async Task<Image> AddAsync(Image image)
        {
            _context.Images.Add(image);
            await _context.SaveChangesAsync();
            return image;
        }

        public async Task<bool> ExistsAsync(string imageUrl)
        {
            return await _context.Images.AnyAsync(i => i.ImageUrl == imageUrl);
        }
    }
}
