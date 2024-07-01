using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using ImageCollector.Domain.Interfaces;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace ImageCollector.Application.Services
{
    public class FlickrService : IFlickrService

    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://api.flickr.com/services/rest/";

        public FlickrService(IConfiguration configuration)
        {
            _apiKey = configuration["Flickr:ApiKey"];
            _httpClient = new HttpClient();

        }

        public async Task<string> GetImageUrlAsync(string tags)
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}?method=flickr.photos.search&api_key={_apiKey}&tags={tags}&format=json&nojsoncallback=1");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var photos = JObject.Parse(json)["photos"]["photo"];
            if (photos.HasValues)
            {
                var firstPhoto = photos.First;
                var farm = firstPhoto["farm"];
                var server = firstPhoto["server"];
                var id = firstPhoto["id"];
                var secret = firstPhoto["secret"];
                return $"https://farm{farm}.staticflickr.com/{server}/{id}_{secret}.jpg";
            }

            return null;
        }
    }
}
