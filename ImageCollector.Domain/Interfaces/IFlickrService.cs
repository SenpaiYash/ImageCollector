
namespace ImageCollector.Domain.Interfaces
{
    public interface IFlickrService
    {
        Task<string> GetImageUrlAsync(string tags);
    }
}
