using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PRN221_Project.Models;
using PRN221_Project.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;

namespace PRN221_Project.Pages.Admin.ManagerShowTimes
{
    //[Authorize(Roles = "Admin, Vip, Editor")]
    public class IndexModel : PageModel
    {
        private readonly HttpClient client;
        private readonly IConfiguration _configuration;
        private readonly string PopularFilm;

        private readonly CinphileDbContext _context;
        DateTime _date;

        [BindProperty]
        public List<Room> rooms { get; set; }

        [BindProperty]
        public List<MovieApi> listFilmsPopular { get; set; } = new List<MovieApi>();

        [BindProperty]
        public List<Movie> movies { get; set; }
        public List<string> movie { get; set; }

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
            await LoadMovieInRoomToday();
            movies = _context.Movies.Where(x => x.IsReleased != false && x.IsReleased != null).ToList();
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

        public async Task LoadMovieInRoomToday()
        {
            await LoadDataFromApi();
            var today = DateTime.Today;
            _date = today;
            var startDate = today;
            var endDate = today.AddDays(1).AddTicks(-1);

            rooms = _context.Rooms
                .Include(m => m.MovieSchedules)
                .ThenInclude(m => m.Movie)
                .ToList();

            foreach (var room in rooms)
            {
                room.MovieSchedules = room.MovieSchedules
                    .Where(s => s.StartTime >= startDate && s.StartTime <= endDate)
                    .ToList();
            }
        }


        public void GetMovieSchedules(DateTime selectedDate)
        {
            var startDate = selectedDate.Date;
            var endDate = selectedDate.Date.AddDays(1).AddTicks(-1);

            rooms = _context.Rooms
               .Include(m => m.MovieSchedules)
               .ThenInclude(m => m.Movie)
               .ToList();

            foreach (var room in rooms)
            {
                room.MovieSchedules = room.MovieSchedules
                    .Where(s => s.StartTime >= startDate && s.StartTime <= endDate)
                    .ToList();
            }

            ViewData["SelectedDate"] = selectedDate;
            movies = _context.Movies.Where(x => x.IsReleased != false && x.IsReleased != null).ToList();
        }
        public async Task<IActionResult> OnPostDelete(int id)
        {
            DateTime selectedDate = DateTime.Parse(Request.Form["dateInput1-" + id]);
            MovieSchedule m = _context.MovieSchedules.FirstOrDefault(x => x.Id == id);
            if (m != null)
            {
                _context.MovieSchedules.Remove(m);
                _context.SaveChanges();
            }
            await LoadDataFromApi();
            GetMovieSchedules(selectedDate);
            ViewData["SelectedDate"] = selectedDate;

            return Page();
        }

        public async Task<IActionResult> OnPostSelect()
        {
            var selectedDate = DateTime.Parse(Request.Form["date"]);
            var startDate = selectedDate.Date;
            var endDate = selectedDate.Date.AddDays(1).AddTicks(-1);

            rooms = _context.Rooms
               .Include(m => m.MovieSchedules)
               .ThenInclude(m => m.Movie)
               .ToList();

            foreach (var room in rooms)
            {
                room.MovieSchedules = room.MovieSchedules
                    .Where(s => s.StartTime >= startDate && s.StartTime <= endDate)
                    .ToList();
            }

            ViewData["SelectedDate"] = selectedDate;
            await LoadDataFromApi();
            movies = _context.Movies.Where(x => x.IsReleased != false && x.IsReleased != null).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAdd()
        {
            int IdMovie = 0;
            DateTime selectedDate = DateTime.Parse(Request.Form["dateInput"]);
            int idRoom = int.Parse(Request.Form["IdRoom"]);
            string movieApi = Request.Form["MovieRoom-" + idRoom];
            Movie selectMovie = _context.Movies.FirstOrDefault(movie => movie.MovieIdApi == movieApi);
            if (selectMovie != null)
            {
                IdMovie = selectMovie.Id;
            }
            TimeSpan selectedStartTime = TimeSpan.Parse(Request.Form["StartTime-" + idRoom]);
            DateTime StartTime = selectedDate.Date.Add(selectedStartTime);

            Movie m = _context.Movies.FirstOrDefault(movie => movie.Id == IdMovie);
            if (m != null)
            {
                if (checkReleaseDateMovie(m, StartTime))
                {
                    if (CheckStartTime(StartTime))
                    {
                        if (IsEndTimeValid(selectedDate, StartTime, idRoom))
                        {
                            if (IsStartTimeValid(selectedDate, StartTime, idRoom, IdMovie))
                            {
                                TimeSpan convertTime = TimeSpan.FromMinutes((double)m.DurationMinutes);
                                MovieSchedule movieSchedule = new MovieSchedule();
                                movieSchedule.RoomId = idRoom;
                                movieSchedule.MovieId = IdMovie;
                                movieSchedule.StartTime = StartTime;
                                movieSchedule.EndTime = StartTime.Add(convertTime);

                                _context.MovieSchedules.Add(movieSchedule);
                                _context.SaveChanges();

                            }
                            else
                            {
                                ViewData["msg" + idRoom] = "Không thể sếp một bộ phim cùng một giờ chiếu";
                                Console.WriteLine("Không hợp lệ");
                            }
                        }
                        else
                        {
                            ViewData["msg" + idRoom] = "Có một bộ phim chưa kết thúc. Không thể set lịch chiếu";
                            Console.WriteLine("Phim chưa kết thúc");
                        }
                    }
                    else
                    {
                        ViewData["msg" + idRoom] = "Đã quá thời gian để set lịch chiếu phim";
                        Console.WriteLine("Đã quá giờ Công chiếu");
                    }
                }
                else
                {
                    ViewData["msg" + idRoom] = "Phim chưa được công chiếu";
                    Console.WriteLine("Phim Chưa được công chiếu");
                }
            }
            await LoadDataFromApi();
            GetMovieSchedules(selectedDate);
            ViewData["SelectedDate"] = selectedDate;
            //OnGet();
            return Page();
        }

        public bool IsEndTimeValid(DateTime selectedDate, DateTime newStartTime, int roomId)
        {
            var latestScheduleInRoom = _context.MovieSchedules
                .Where(s => s.RoomId == roomId && s.StartTime.Date == selectedDate.Date)
                .OrderByDescending(s => s.StartTime)
                .FirstOrDefault();

            if (latestScheduleInRoom != null && newStartTime < latestScheduleInRoom.EndTime)
            {
                return false;
            }

            return true;
        }
        public bool CheckStartTime(DateTime StartTime)
        {
            DateTime time = DateTime.Now;
            if (StartTime < time)
            {
                return false;
            }
            return true;
        }

        public bool checkReleaseDateMovie(Movie m, DateTime StartTime)
        {
            if (StartTime < m.ReleaseDate)
            {
                return false;
            }
            return true;
        }

        public bool IsStartTimeValid(DateTime selectedDate, DateTime newStartTime, int roomId, int movieId)
        {
            List<MovieSchedule> allSchedules = _context.MovieSchedules.ToList();

            TimeSpan minTimeBetweenSchedules = TimeSpan.FromMinutes(30);

            foreach (var schedule in allSchedules)
            {
                if (schedule.StartTime.Date == selectedDate.Date)
                {
                    TimeSpan timeDifference = newStartTime - schedule.StartTime;

                    if (timeDifference.Duration() < minTimeBetweenSchedules)
                    {
                        if (schedule.MovieId == movieId && schedule.RoomId != roomId)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }
    }
    public class MovieApi
    {
        public int? Id { get; set; }
        public string? title { get; set; }
        public string? poster_path { get; set; }

    }
   

}
