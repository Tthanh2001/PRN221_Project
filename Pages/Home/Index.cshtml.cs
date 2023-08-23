using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using PRN221_Project.Models;
using PRN221_Project.Utils;
using System.Net.Http.Headers;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace PRN221_Project.Pages
{
    public class IndexModel : PageModel
    {


        private readonly HttpClient client;
        private readonly IConfiguration _configuration;
        private readonly string PopularFilm;

        private readonly CinphileDbContext _db;
        public IndexModel(IConfiguration configuration, CinphileDbContext db)
        {
            _db = db;
            _configuration = configuration;
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            PopularFilm = "https://api.themoviedb.org/3/movie/popular?api_key=e9e9d8da18ae29fc430845952232787c&append_to_response=videos"
                ;
        }

        public List<MovieApi> listFilms { get; set; } = new List<MovieApi>();

        public List<string> movie { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {

            string apiUrl = PopularFilm;
            HttpResponseMessage response = await client.GetAsync(apiUrl);

            movie = _db.Movies.Select(o => o.MovieIdApi).ToList();

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                dynamic dataFromApi = JsonConvert.DeserializeObject(json);

                
                foreach (var result in dataFromApi["results"])
                {
                    foreach (var item in movie)
                    {
                        
                        if (item == result["id"].ToString())
                        {

                            int Id = result["id"];
                            string poster_path = "https://image.tmdb.org/t/p/w500/" + result["poster_path"];
                            string title = result["title"];



                            listFilms.Add(new MovieApi { Id=Id, poster_path=poster_path,title = title});
                        }
                    }
                }
            }
            return Page();

        }

        public class MovieApi
        {
            public int? Id { get; set; }
            public string? title { get; set; }
            public string? poster_path { get; set; }

        }

    }
}

