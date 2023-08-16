using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectPRN.Models;
using ProjectPRN.Utils;
using System.Configuration;
using System.Net.Http.Headers;
using System.Text.Json;

namespace PRN221_Project.Pages
{
    public class MovieDetailModel : PageModel
    {
        private readonly HttpClient client = null;
        private readonly IConfiguration _configuration;
        private string movieapi = "";




        public MovieDetailModel(IConfiguration configuration)
        {
            _configuration = configuration;
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            movieapi = "https://api.themoviedb.org/3/movie/297764?api_key=e9e9d8da18ae29fc430845952232787c&append_to_response=videos";
          
        }
        [BindProperty]
        public Movie movie { get; set; }

        public async Task<IActionResult> OnGet()
        {
            HttpResponseMessage resp = await client.GetAsync(movieapi);

            var strData = await resp.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            movie = JsonSerializer.Deserialize<Movie>(strData, options);

            
           
            return Page();
        }
    }
}