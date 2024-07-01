using ImageCollector.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ImageCollector.UI.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public IndexModel(ILogger<IndexModel> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }


        [BindProperty]
        public string Location { get; set; }
        public List<ImageDto> Images { get; set; } = new List<ImageDto>();
        public int TotalPages { get; set; }

  
        public void OnGet()
        {
            
        }

        private async Task SearchImagesAsync(string location, int page)
        {
            var token = Request.Cookies["jwtToken"];
            if (string.IsNullOrEmpty(token))
            {
                // Handle missing token
                return;
            }

            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await httpClient.GetAsync($"https://localhost:7140/api/image/{location}?page={page}&pageSize=10");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var jsonDocument = JsonDocument.Parse(responseContent);
                var images = jsonDocument.RootElement.GetProperty("data").Deserialize<List<ImageDto>>();
                var totalPages = jsonDocument.RootElement.GetProperty("totalPages").GetInt32();

                Images = images;
                TotalPages = totalPages;
            }
        }
    }
}
