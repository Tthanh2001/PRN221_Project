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

        public IndexModel(CinphileDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            PopularFilm = "https://api.themoviedb.org/3/movie/popular?api_key=e9e9d8da18ae29fc430845952232787c&append_to_response=videos";
        }

        public async Task OnGetAsync()
        {
            movies = _context.Movies.ToList();
            await LoadDataFromApi();

        }
        public async Task<bool> CheckApi(string api)
        {
            string apiUrl = PopularFilm;
            HttpResponseMessage response = await client.GetAsync(apiUrl);
            try
            {
                int Id = int.Parse(api);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    dynamic dataFromApi = JsonConvert.DeserializeObject(json);
                    foreach (var result in dataFromApi["results"])
                    {
                        if (result["id"] == Id)
                        {
                            return true;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }


            return false;
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


        public async Task<IActionResult> OnPost()
        {
            AddMovie.MovieIdApi = Request.Form["MovieIdApi"];
            AddMovie.DurationMinutes = int.Parse(Request.Form["duration"]);
            if (await CheckApi(AddMovie.MovieIdApi))
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
                ViewData["msg"] = "Phim không tồn tại";
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
