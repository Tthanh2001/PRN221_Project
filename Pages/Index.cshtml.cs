using MailKit.Search;
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
        private readonly string toprate;
        private readonly string upcoming;

        private readonly CinphileDbContext _db;
        public IndexModel(IConfiguration configuration, CinphileDbContext db)
        {
            _db = db;
            _configuration = configuration;
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            PopularFilm = "https://api.themoviedb.org/3/movie/popular?api_key=e9e9d8da18ae29fc430845952232787c&append_to_response=videos";
            toprate = "https://api.themoviedb.org/3/movie/top_rated?api_key=e9e9d8da18ae29fc430845952232787c&append_to_response=videos";
            upcoming = "https://api.themoviedb.org/3/movie/upcoming?api_key=e9e9d8da18ae29fc430845952232787c&append_to_response=videos";

        }

        public List<MovieApi> listFilmsPopular { get; set; } = new List<MovieApi>();
        public List<MovieApi> listFilmsToprate { get; set; } = new List<MovieApi>();
        public List<MovieApi> listFilmsUpcoming { get; set; } = new List<MovieApi>();

        [BindProperty]
        public List<Movie> movies { get; set; }
        public List<string> movie { get; set; }

        public async Task<IActionResult> OnGetAsync(string searchQuery)
        {
            //popular
            movies = _db.Movies.ToList();
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



                            listFilmsPopular.Add(new MovieApi { Id = Id, poster_path = poster_path, title = title });
                        }
                    }
                }
            }
            //toprate
            string apiUrtopratel = toprate;
            HttpResponseMessage response1 = await client.GetAsync(apiUrtopratel);

            if (response1.IsSuccessStatusCode)
            {
                string json = await response1.Content.ReadAsStringAsync();
                dynamic dataFromApiToprate = JsonConvert.DeserializeObject(json);


                foreach (var result in dataFromApiToprate["results"])
                {
                    foreach (var item in movie)
                    {

                        if (item == result["id"].ToString())
                        {

                            int Id = result["id"];
                            string poster_path = "https://image.tmdb.org/t/p/w500/" + result["poster_path"];
                            string title = result["title"];



                            listFilmsToprate.Add(new MovieApi { Id = Id, poster_path = poster_path, title = title });
                        }
                    }
                }
            }

            //upcoming
            string apiUrlupcoming = upcoming;
            HttpResponseMessage response2 = await client.GetAsync(PopularFilm);



            if (response2.IsSuccessStatusCode)
            {
                string json = await response2.Content.ReadAsStringAsync();
                dynamic dataFromApiupcoming = JsonConvert.DeserializeObject(json);


                foreach (var result in dataFromApiupcoming["results"])
                {
                    foreach (var item in movie)
                    {

                        if (item == result["id"].ToString())
                        {

                            int Id = result["id"];
                            string poster_path = "https://image.tmdb.org/t/p/w500/" + result["poster_path"];
                            string title = result["title"];



                            listFilmsUpcoming.Add(new MovieApi { Id = Id, poster_path = poster_path, title = title });
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(searchQuery))
            {
                // Perform search based on the searchQuery
                string searchUrl = $"https://api.themoviedb.org/3/search/movie?api_key=ededa5fa66293fac3e0230a470cf5788&query={searchQuery}";
                HttpResponseMessage searchResponse = await client.GetAsync(searchUrl);

                if (searchResponse.IsSuccessStatusCode)
                {
                    string searchJson = await searchResponse.Content.ReadAsStringAsync();
                    dynamic searchData = JsonConvert.DeserializeObject(searchJson);

                    listFilmsPopular.Clear(); // Clear the list before adding new search results

                    foreach (var result in searchData["results"])
                    {
                        int id = result["id"];
                        string posterPath = "https://image.tmdb.org/t/p/w500/" + result["poster_path"];
                        string title = result["title"];

                        listFilmsPopular.Add(new MovieApi { Id = id, poster_path = posterPath, title = title });
                    }

                    if (listFilmsPopular.Count == 0)
                    {
                        // No movies found, display "Not found" message
                        ViewData["SearchMessage"] = "No movies found.";
                    }
                }
                else
                {
                    // Handle error response from the API
                    ViewData["SearchMessage"] = "Error occurred while searching for movies.";
                }
            }



            Console.WriteLine(listFilmsUpcoming);
            Console.WriteLine(listFilmsUpcoming);
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

