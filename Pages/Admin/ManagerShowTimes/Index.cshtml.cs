using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN221_Project.Models;
using PRN221_Project.Utils;

namespace PRN221_Project.Pages.Admin.ManagerShowTimes
{
    public class IndexModel : PageModel
    {
        private readonly CinphileDbContext _context;

        DateTime _date;

        [BindProperty]
        public List<Room> rooms { get; set; }

        [BindProperty]
        public List<Movie> movies { get; set; }

        public IndexModel(CinphileDbContext context)
        {
            _context = context;
        }
        public void OnGet()
        {
            LoadMovieInRoomToday();
            //rooms = _context.Rooms.Include(m => m.MovieSchedules).ThenInclude(m => m.Movie).ToList();
            movies = _context.Movies.ToList();
        }

        public void LoadMovieInRoomToday()
        {
            var today = DateTime.Today;
            _date = today;
            var startDate = today;
            var endDate = today.AddDays(1).AddTicks(-1); // Kết thúc vào 23:59:59

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
        public IActionResult OnPost()
        {
            var selectedDateStr = Request.Form["date"];
            _date = DateTime.Parse(selectedDateStr);
            if (DateTime.TryParse(selectedDateStr, out var selectedDate))
            {
                var startDate = selectedDate.Date;
                var endDate = startDate.AddDays(1).AddTicks(-1);


                rooms = _context.Rooms
                    .Include(m => m.MovieSchedules)
                    .ThenInclude(m => m.Movie)
                    .Where(r => r.MovieSchedules.Any(s => s.StartTime >= startDate && s.StartTime <= endDate))
                    .ToList();
            }
            movies = _context.Movies.ToList();

            return Page();
        }
        public IActionResult OnPostAdd()
        {
            DateTime selectedDate = DateTime.Parse(Request.Form["dateInput"]);
            int idRoom = int.Parse(Request.Form["IdRoom"]);
            int IdMovie = int.Parse(Request.Form["MovieRoom-" + idRoom]);

            TimeSpan selectedStartTime = TimeSpan.Parse(Request.Form["StartTime-" + idRoom]);
            DateTime StartTime = selectedDate.Date.Add(selectedStartTime);

            Movie m = _context.Movies.FirstOrDefault(m => m.Id == IdMovie);
            

            if (IsStartTimeValid(selectedDate, StartTime))
            {
                Console.WriteLine(m);
                if (m != null)
                {
                    TimeSpan convertTime = TimeSpan.FromMinutes(m.DurationMinutes);

                    MovieSchedule movieSchedule = new MovieSchedule();
                    movieSchedule.RoomId = idRoom;
                    movieSchedule.MovieId = IdMovie;
                    movieSchedule.StartTime = StartTime;
                    movieSchedule.EndTime = StartTime.Add(convertTime);

                    _context.MovieSchedules.Add(movieSchedule);
                    _context.SaveChanges();
                }                
            }
            else
            {
                Console.WriteLine("Không hợp lệ");
            }




            return RedirectToPage();
        }

        public bool IsStartTimeValid(DateTime selectedDate, DateTime newStartTime)
        {
            List<MovieSchedule> allSchedules = new List<MovieSchedule>();
            allSchedules = _context.MovieSchedules.ToList();

            TimeSpan minTimeBetweenSchedules = TimeSpan.FromMinutes(30);

            foreach (var schedule in allSchedules)
            {
                if (schedule.StartTime.Date == selectedDate.Date)
                {
                    TimeSpan timeDifference = newStartTime - schedule.StartTime;

                    if (timeDifference.Duration() < minTimeBetweenSchedules)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

    }
}
