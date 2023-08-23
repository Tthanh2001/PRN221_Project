using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using NuGet.DependencyResolver;
using PRN221_Project.Models;
using PRN221_Project.Utils;
using System.Net.Http.Headers;
using System.Text.Json;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;
using static PRN221_Project.Pages.IndexModel;
using static System.Net.WebRequestMethods;

namespace PRN221_Project.Pages.Home
{
    public class MovieDetailModel : PageModel

    {
        private readonly CinphileDbContext _db;
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

        
        public MovieDetailModel(IConfiguration configuration,CinphileDbContext db)
        {
            _db = db;
            _configuration = configuration;
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

        }
        public List<string> movie { get; set; }

        public List<MovieApi> listFilmSimilar { get; set; } = new List<MovieApi>();
        public class MovieApi
        {
            public int? Id { get; set; }
            public string? title { get; set; }
            public string? poster_path { get; set; }

        }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            string apiUrl = "https://api.themoviedb.org/3/movie/"+id+"?api_key=e9e9d8da18ae29fc430845952232787c&append_to_response=videos";
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
           
            HttpResponseMessage response1 = await client.GetAsync("https://api.themoviedb.org/3/movie/"+id+"/similar?api_key=e9e9d8da18ae29fc430845952232787c&language=en-US&page=1");

            movie = _db.Movies.Select(o => o.MovieIdApi).ToList();

            if (response.IsSuccessStatusCode)
            {
                string json = await response1.Content.ReadAsStringAsync();
                dynamic dataFromApisimilar = JsonConvert.DeserializeObject(json);


                foreach (var result in dataFromApisimilar["results"])
                {
                    foreach (var item in movie)
                    {

                        if (item == result["id"].ToString())
                        {

                            int Id = result["id"];
                            string poster_path = "https://image.tmdb.org/t/p/w500/" + result["poster_path"];
                            string title = result["title"];



                            listFilmSimilar.Add(new MovieApi { Id = Id, poster_path = poster_path, title = title });
                        }
                    }
                }
            }
            return Page();
        }
    }
}