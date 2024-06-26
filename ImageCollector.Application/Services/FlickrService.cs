using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ImageCollector.Application.Services
{
    public class FlickrService
    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;

        public FlickrService(IConfiguration configuration)
        {
            _apiKey = configuration["Flickr:ApiKey"];
            _httpClient = new HttpClient();
        }

        public async Task<JArray> GetImagesAsync(string locationId)
        {
            var url = $"https://api.flickr.com/services/rest/?method=flickr.photos.search&api_key={_apiKey}&tags={locationId}&format=json&nojsoncallback=1";
            var response = await _httpClient.GetStringAsync(url);
            var json = JObject.Parse(response);
            var photos = (JArray)json["photos"]["photo"];
            return photos;
        }
    }
}
