using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using PRN221_Project.Models;
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

        }

        [BindProperty]
        public Movie movie { get; set; }

        [BindProperty]
        public string title { get; set; }
        [BindProperty]
        public string url { get; set; }
        public async Task<IActionResult> OnGet(int id)
        {
            HttpResponseMessage resp = await client.GetAsync("https://api.themoviedb.org/3/movie/" + id + "?api_key=e9e9d8da18ae29fc430845952232787c&append_to_response=videos");

            var strData = await resp.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            //movie = JsonSerializer.Deserialize<Movie>(strData, options);
            dynamic jsonData = JsonConvert.DeserializeObject(strData);

            title = jsonData.title;

            url = jsonData.backdrop_path;

            return Page();
        }
    }
}