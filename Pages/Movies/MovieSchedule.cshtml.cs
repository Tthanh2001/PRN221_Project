using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN221_Project.Models;
using PRN221_Project.Utils;

namespace PRN221_Project.Pages.Home
{
    public class MovieScheduleModel : PageModel
    {
        private readonly CinphileDbContext _db;

        public MovieScheduleModel(CinphileDbContext db)
        {
            _db = db;   
        }
        public List<DateTime> date { get; set; } = new List<DateTime>();
        public List<MovieSchedule> movieSchedules { get; set; } = new List<MovieSchedule>();
        public int Id { get; set; }
        public DateTime Day { get; set; }
        public async Task<IActionResult> OnGetAsync(int id, string day)
        {
            if (day != null)
            {
                Day = DateTime.Parse(day);

            }
            Id = id;
            DateTime currentDate = DateTime.Now.Date;
            int mid = _db.Movies.Where(o => o.MovieIdApi == id.ToString()).Select(o => o.Id).First();
            for (int i = 1; i <= 7; i++)
            {
                DateTime nextDay = currentDate.AddDays(i);
                date.Add(nextDay);
            }
            if(day!= null)
            {
                movieSchedules = _db.MovieSchedules.Where(o => o.MovieId == mid && o.StartTime.Date == DateTime.Parse(day).Date).ToList();
            }
            
            return Page();
        }
    }
}
