using ImageCollector.Application.DTOs;
using ImageCollector.Application.Services;
using ImageCollector.UI.Services;
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
                private readonly FlickrServiceAPI _flickrApiService;

        public IndexModel(ILogger<IndexModel> logger, IHttpClientFactory httpClientFactory,FlickrServiceAPI flickrApiService)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _flickrApiService = flickrApiService;
        }


        [BindProperty]
        public string Location { get; set; }
        public List<ImageDto> Images { get; set; } = new List<ImageDto>();
        public ImageDto Image { get; set; } 
        public string imageurl { get; set; }
        public int TotalPages { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

         
            await SearchImagesAsync(Location, 1);

            if (!string.IsNullOrEmpty(Location))
            {  
                string test = await _flickrApiService.GetImageUrlAsync(Location);
                imageurl = test;
            }
            return Page();
        }
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
