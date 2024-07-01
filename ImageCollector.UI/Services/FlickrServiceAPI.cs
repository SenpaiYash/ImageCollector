using Newtonsoft.Json.Linq;

namespace ImageCollector.UI.Services
{
    public class FlickrServiceAPI
    {
        private readonly HttpClient _httpClient;

        public FlickrServiceAPI(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetImageUrlAsync(string tags)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7140/api/flickr/image?tags={tags}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var jsonObject = JObject.Parse(json);
            //return jsonObject["url"].ToString();
            var value = jsonObject["url"].ToString();
            return value;
        }
    }
}
