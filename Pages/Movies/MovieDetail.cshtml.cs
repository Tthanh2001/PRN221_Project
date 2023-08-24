using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using NuGet.DependencyResolver;
using PRN221_Project.Models;
using PRN221_Project.Utils;
using System.Net.Http.Headers;
using System.Text.Json;
using static Microsoft.AspNetCore.Razor.Language.TagHelperMetadata;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;
using static PRN221_Project.Pages.IndexModel;
using static System.Net.WebRequestMethods;

namespace PRN221_Project.Pages.Home
{
    public class MovieDetailModel : PageModel

    {
        private readonly CinphileDbContext _db;
        private readonly HttpClient client;
        private readonly IConfiguration _configuration;
        private readonly string AuthorApiUrl;

        private readonly UserManager<ApplicationAccount> _userManager;
        [BindProperty]
        public Movie movies { get; set; }

        public string title { get; set; }
        public string poster_path { get; set; }
        public string backdrop_path { get; set; }
        public string overview { get; set; }
        public int runtime { get; set; }
        public List<string> video { get; set; } = new List<string>();
        public List<string> genres { get; set; } = new List<string>();

        public int rate { get; set; }
        public MovieDetailModel(IConfiguration configuration, CinphileDbContext db, UserManager<ApplicationAccount> userManager)
        {
            _db = db;
            _configuration = configuration;
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            _userManager = userManager;
        }
        public List<string> movie { get; set; }

        public List<MovieApi> listFilmSimilar { get; set; } = new List<MovieApi>();
        public class MovieApi
        {
            public int? Id { get; set; }
            public string? title { get; set; }
            public string? poster_path { get; set; }
            public int runtime { get; set; }

        }
        public int Id { get; set; }
        public async Task<IActionResult> OnGetAsync(int rating, int id)
        {
            int mid = _db.Movies.Where(o => o.MovieIdApi == id.ToString()).Select(o => o.Id).First();

            if (rating != 0)
            {
                Rating rating1 = new Rating
                {
                    Rates = rating,
                    MovieId = mid,
                    ApplicationAccountId = _userManager.GetUserId(User),
                };
                _db.Ratings.Add(rating1);
                _db.SaveChanges();
            }
            int rated = _db.Ratings
    .Where(o => o.MovieId == mid && o.ApplicationAccountId == _userManager.GetUserId(User))
    .OrderByDescending(o => o.Id) // Sắp xếp theo thuộc tính Id giảm dần
    .Select(o => o.Rates)
    .FirstOrDefault();
            if (rated != 0)
            {
                rate = rated;
            }
            Id = id;
            string apiUrl = "https://api.themoviedb.org/3/movie/" + id + "?api_key=e9e9d8da18ae29fc430845952232787c&append_to_response=videos";
            HttpResponseMessage response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                dynamic dataFromApi = JsonConvert.DeserializeObject(json);
                title = dataFromApi.title;
                runtime = dataFromApi.runtime;
                poster_path = "https://image.tmdb.org/t/p/w500/" + dataFromApi.poster_path;
                backdrop_path = "https://image.tmdb.org/t/p/w500/" + dataFromApi.backdrop_path;
                foreach (var result in dataFromApi["videos"]["results"])
                {
                    if (result["type"] == "Trailer")
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

            HttpResponseMessage response1 = await client.GetAsync("https://api.themoviedb.org/3/movie/" + id + "/similar?api_key=e9e9d8da18ae29fc430845952232787c&language=en-US&page=1");

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
                            int  runtime = result["runtime"];


                            listFilmSimilar.Add(new MovieApi { Id = Id, poster_path = poster_path, title = title, runtime = runtime });
                        }
                    }
                }
            }
            
            return Page();
        }
      
    }
}