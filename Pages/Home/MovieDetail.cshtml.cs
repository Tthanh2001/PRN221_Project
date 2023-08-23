using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using NuGet.DependencyResolver;
using PRN221_Project.Models;
using PRN221_Project.Utils;
using System.Net.Http.Headers;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace PRN221_Project.Pages.Home
{
    public class MovieDetailModel : PageModel

    {
        private readonly CinphileDbContext _context;
        private readonly HttpClient client;
        private readonly IConfiguration _configuration;
        private readonly string AuthorApiUrl;
        [BindProperty]
        public Movie movies { get; set; }

        public string title { get; set; }
        public string poster_path { get; set; }
        public string backdrop_path { get; set; }
        public string overview { get; set; }
        public List<string> video { get; set; } = new List<string>();
        public List<string> genres { get; set; } = new List<string>();


        public MovieDetailModel(IConfiguration configuration)
        {
            _configuration = configuration;
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

        }

        

        public async Task<IActionResult> OnGetAsync()
        {
            string apiUrl = "https://api.themoviedb.org/3/movie/872585?api_key=e9e9d8da18ae29fc430845952232787c&append_to_response=videos";
            HttpResponseMessage response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                dynamic dataFromApi = JsonConvert.DeserializeObject(json);
                 title = dataFromApi.title;
                poster_path = "https://image.tmdb.org/t/p/w500/"+dataFromApi.poster_path;
                backdrop_path = "https://image.tmdb.org/t/p/w500/" + dataFromApi.backdrop_path;
                foreach (var result in dataFromApi["videos"]["results"])
                {
                    if (result["type"]== "Trailer")
                    {
                        string key = "https://www.youtube.com/embed/" + result["key"];
                        video.Add(key);
                    }
                   
                }
                overview = dataFromApi.overview;
                foreach (var genre in dataFromApi["genres"])
                {
                    string name = genre["name"];
                    genres.Add(name);
                }

            }
            return Page();
        }
    }
}