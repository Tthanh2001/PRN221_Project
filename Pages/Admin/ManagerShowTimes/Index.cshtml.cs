using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN221_Project.Models;
using PRN221_Project.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

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
            movies = _context.Movies.ToList();
        }

        public void LoadMovieInRoomToday()
        {
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

        public IActionResult OnPost()
        {
            var today = DateTime.Parse(Request.Form["date"]);
            var startDate = today;
            var endDate = today.AddDays(1).AddTicks(-1);

            rooms = _context.Rooms
             .Include(m => m.MovieSchedules)
             .ThenInclude(m => m.Movie)
             .ToList();
            ViewData["SelectedDate"] = today;
            foreach (var room in rooms)
            {
                room.MovieSchedules = room.MovieSchedules
                    .Where(s => s.StartTime >= startDate && s.StartTime <= endDate)
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
                                TimeSpan convertTime = TimeSpan.FromMinutes(m.DurationMinutes);
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
                                Console.WriteLine("Không hợp lệ");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Phim chưa kết thúc");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Đã quá giờ Công chiếu");
                    }
                }
                else
                {
                    Console.WriteLine("Phim Chưa được công chiếu");
                }
            }
            return RedirectToPage();
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
        //public bool IsEndTimeValid(Movie movie, DateTime startTime)
        //{
        //    TimeSpan convertTime = TimeSpan.FromMinutes(movie.DurationMinutes);
        //    DateTime endTime = startTime.Add(convertTime);

        //    return endTime > startTime;
        //}
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
}
