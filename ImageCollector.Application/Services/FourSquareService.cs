using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ImageCollector.Application.Services
{
    public class FourSquareService
    {
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _version;
        private readonly HttpClient _httpClient;

        public FourSquareService(IConfiguration configuration)
        {
            _clientId = configuration["FourSquare:ClientId"];
            _clientSecret = configuration["FourSquare:ClientSecret"];
            _version = configuration["FourSquare:Version"];
            _httpClient = new HttpClient();
        }

        public async Task<string> GetLocationIdAsync(string locationName)
        {
            var url = $"https://api.foursquare.com/v2/venues/search?client_id={_clientId}&client_secret={_clientSecret}&v={_version}&near={locationName}&limit=1";
            var response = await _httpClient.GetStringAsync(url);
            var json = JObject.Parse(response);
            var locationId = json["response"]["venues"][0]["id"].ToString();
            return locationId;
        }

        public async Task<(string Name, string Description)> GetLocationDetailsAsync(string locationId)
        {
            var url = $"https://api.foursquare.com/v2/venues/{locationId}?client_id={_clientId}&client_secret={_clientSecret}&v={_version}";
            var response = await _httpClient.GetStringAsync(url);
            var json = JObject.Parse(response);
            var venue = json["response"]["venue"];
            var name = venue["name"].ToString();
            var description = venue["description"]?.ToString() ?? "No description available";
            return (name, description);
        }
    }
}
