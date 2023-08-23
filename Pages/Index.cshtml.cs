using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN221_Project.Utils;
using PRN221_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace PRN221_Project.Pages
{
    public class IndexModel : PageModel
    {
        private readonly CinphileDbContext _context;

        [BindProperty]
        public List<Movie> movies { get; set; }

        [BindProperty]
        public List<Room> rooms { get; set; }   
        public IndexModel(CinphileDbContext context)
        {
            _context = context;
        }
        public void OnGet()
        {
            movies = _context.Movies.Include(m => m.Director).Include(m => m.Genre).ToList();
            rooms = _context.Rooms.Include(m => m.MovieSchedules).ThenInclude(m => m.Movie).ToList();
        }
    }
}