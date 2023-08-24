using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PRN221_Project.Models;
using PRN221_Project.Utils;
using System.Net.Http.Headers;

namespace PRN221_Project.Pages.Admin.ManagerMovie
{
    //[Authorize(Roles = "Admin, Vip, Editor")]
    public class IndexModel : PageModel
    {
        private readonly HttpClient client;
        private readonly IConfiguration _configuration;
        private readonly string PopularFilm;
        private readonly CinphileDbContext _context;

        [BindProperty]
        public List<Movie> movies { get; set; }

        [BindProperty]
        public Movie AddMovie { get; set; }

        public List<string> movie { get; set; }

        [BindProperty]
        public List<MovieApi> listFilmsPopular { get; set; } = new List<MovieApi>();

        [BindProperty]
        public List<MovieApi> listAllFilmsPopular { get; set; } = new List<MovieApi>();

        public IndexModel(CinphileDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            PopularFilm = "https://api.themoviedb.org/3/movie/popular?api_key=e9e9d8da18ae29fc430845952232787c&append_to_response=videos";
        }
        public async Task LoadAllDataFromApi()
        {
            string apiUrl = PopularFilm;
            HttpResponseMessage response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                dynamic dataFromApi = JsonConvert.DeserializeObject(json);


                foreach (var result in dataFromApi["results"])
                {
                    int Id = result["id"];
                    string poster_path = "https://image.tmdb.org/t/p/w500/" + result["poster_path"];
                    string title = result["title"];
                    listAllFilmsPopular.Add(new MovieApi { Id = Id, poster_path = poster_path, title = title });
                }
                Console.WriteLine(listAllFilmsPopular);
            }
        }
        public async Task SetStatusMovie()
        {
            movies = _context.Movies.ToList();
           foreach(var movie in movies)
            {
                if(movie.IsReleased == null)
                {
                    if(DateTime.Now > movie.ReleaseDate)
                    {
                        movie.IsReleased = true;    
                    }
                }
            }
        }
        public async Task OnGetAsync()
        {
            await SetStatusMovie();
            await LoadDataFromApi();
            await LoadAllDataFromApi();

        }        
        public async Task LoadDataFromApi()
        {
            string apiUrl = PopularFilm;
            HttpResponseMessage response = await client.GetAsync(apiUrl);

            movie = _context.Movies.Select(o => o.MovieIdApi).ToList();

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
        }

        public Movie getMovieById(string api)
        {
            Movie m = new Movie();
            m = _context.Movies.FirstOrDefault(x => x.MovieIdApi == api);
            return m;
        }
        public async Task<IActionResult> OnPost()
        {
            Movie m = new Movie();
            AddMovie.MovieIdApi = Request.Form["MovieIdApi"];
            AddMovie.DurationMinutes = int.Parse(Request.Form["duration"]);
            m = getMovieById(AddMovie.MovieIdApi);
            if (m == null)
            {
                if (AddMovie.ReleaseDate < DateTime.Now)
                {
                    ViewData["Message"] = "Đã quá thời gian. Vui lòng chọn ngày khác";
                    await OnGetAsync();
                    return Page();
                }
                else if (AddMovie.ReleaseDate == DateTime.Now)
                {
                    AddMovie.IsReleased = true;
                    _context.Movies.Add(AddMovie);
                    _context.SaveChanges();
                    await OnGetAsync();
                    return Page();
                }
                else
                {
                    _context.Movies.Add(AddMovie);
                    _context.SaveChanges();
                    await OnGetAsync();
                    return Page();
                }
            }
            else
            {
                ViewData["Message"] = "Phim đã tồn tại";
                await OnGetAsync();
                return Page();
            }


        }
        public async Task<IActionResult> OnPostDelete(int id)
        {
            var movie = _context.Movies.Find(id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
                _context.SaveChanges();
            }
            await OnGetAsync();
            return Page();
        }
    }
    public class MovieApi
    {
        public int? Id { get; set; }
        public string? title { get; set; }
        public string? poster_path { get; set; }

    }
}
