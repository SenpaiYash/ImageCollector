using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace ImageCollector.Application.Services
{
    public class FourSquareService
    {
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _version;
        private readonly string _auth;
        private readonly HttpClient _httpClient;

        public FourSquareService(IConfiguration configuration)
        {
            _clientId = configuration["FourSquare:ClientId"];
            _clientSecret = configuration["FourSquare:ClientSecret"];
            _version = configuration["FourSquare:Version"];
            _auth = configuration["FourSquare:Auth"];
            _httpClient = new HttpClient();
        }

        public async Task<string> GetLocationIdAsync(string locationName)
        {   
            var url = $"https://api.foursquare.com/v2/venues/search?client_id={_clientId}&client_secret={_clientSecret}&v={_version}&near={Uri.EscapeDataString(locationName)}&limit=1";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _auth);

            var response = await _httpClient.GetStringAsync(url);
           
            var json = JObject.Parse(response);
            var locationNameResult = json["response"]["venues"]?[0]?["name"]?.ToString();

            if (locationNameResult == null)
            {
                throw new Exception("Location name not found.");
            }

            return locationNameResult;
        }

        public async Task<(string Name, string Description)> GetLocationDetailsAsync(string locationName)
        {
            var url = $"https://api.foursquare.com/v2/venues/search?client_id={_clientId}&client_secret={_clientSecret}&v={_version}&near={Uri.EscapeDataString(locationName)}&limit=1";
            
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _auth);

            var response = await _httpClient.GetStringAsync(url);

            var json = JObject.Parse(response);
            var venue = json["response"]["venues"]?[0];

            if (venue == null)
            {
                throw new Exception("Venue details not found.");
            }

            var name = venue["name"]?.ToString() ?? "No name available";
            var description = venue["description"]?.ToString() ?? "No description available";

            return (name, description);
        }
    }
}
