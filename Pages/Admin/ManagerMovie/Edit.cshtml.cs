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
    public class EditModel : PageModel
    {

        private readonly HttpClient client;
        private readonly IConfiguration _configuration;
        private readonly string PopularFilm;
        private readonly CinphileDbContext _context;

        public EditModel(IConfiguration configuration, CinphileDbContext context)
        {
            _context = context;
            _configuration = configuration;
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            PopularFilm = "https://api.themoviedb.org/3/movie/popular?api_key=e9e9d8da18ae29fc430845952232787c&append_to_response=videos";
        }

        [BindProperty]
        public List<MovieApi> listFilmsPopular { get; set; } = new List<MovieApi>();


        [BindProperty]
        public Movie EditMovie { get; set; }
        public async Task OnGet(int? id)
        {
            await LoadAllDataFromApi();
            Console.WriteLine(id);
            EditMovie = _context.Movies.FirstOrDefault(x => x.Id == id);
            Console.WriteLine(EditMovie.IsReleased);

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
                    listFilmsPopular.Add(new MovieApi { Id = Id, poster_path = poster_path, title = title });
                }
                Console.WriteLine(listFilmsPopular);
            }
        }
        public Movie getMovieById(int Id)
        {
            Movie m = new Movie();
            m = _context.Movies.FirstOrDefault(x => x.Id == Id);
            return m;
        }
        public async Task<IActionResult> OnPost()
        {
            int id = EditMovie.Id;
            Movie movie = new Movie();
            movie = getMovieById(id);
            movie.MovieIdApi = Request.Form["MovieIdApi"];
            if (movie.IsReleased == null)
            {
                Movie existingMovie = _context.Movies.FirstOrDefault(m => m.MovieIdApi == movie.MovieIdApi);
                if (existingMovie != null && existingMovie.Id != id)
                {
                    ViewData["Message"] = "Phim đã tồn tại";
                    await OnGet(id);
                    return Page();
                }
                else
                {
                    movie.ReleaseDate = DateTime.Parse(Request.Form["ReleaseDate"]);
                    if (movie.ReleaseDate < DateTime.Now)
                    {
                        ViewData["Message"] = "Đã quá thời gian. Vui lòng chọn ngày khác";
                        await OnGet(id);
                        return Page();
                    }
                    else
                    {
                        _context.Attach(movie).State = EntityState.Modified;
                        _context.SaveChanges();
                        return RedirectToPage("/Admin/ManagerMovie/Index");
                    }
                }
            }
            else
            {
                movie.DurationMinutes = int.Parse(Request.Form["duration"]);
                if (Request.Form["Status"] == "0")
                {
                    movie.IsReleased = false;
                }
                else if (Request.Form["Status"] == "1")
                {
                    movie.IsReleased = true;
                }
                string a = Request.Form["Status"];               
                _context.Attach(movie).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToPage("/Admin/ManagerMovie/Index");
            }







        }
    }
}

