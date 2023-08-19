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
            rooms = _context.Rooms.Include(m => m.MovieSchedules).ThenInclude(m => m.Movie).ToList();
            movies= _context.Movies.ToList();
        }
    }
}
