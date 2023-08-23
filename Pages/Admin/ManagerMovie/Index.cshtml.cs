using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN221_Project.Models;
using PRN221_Project.Utils;

namespace PRN221_Project.Pages.Admin.ManagerMovie
{
    [Authorize(Roles = "Admin, Vip, Editor")]
    public class IndexModel : PageModel
    {
        private readonly CinphileDbContext _context;

        [BindProperty]
        public List<Movie> movies { get; set; }

        [BindProperty]
        public Movie AddMovie { get; set; }

        [BindProperty]
        public List<Genre> Genres { get; set; }

        public IndexModel(CinphileDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            movies = _context.Movies.Include(m => m.Director).Include(m => m.Genre).ToList();
            Genres = _context.Genres.ToList();
        }

        public IActionResult OnPost()
        {      
            AddMovie.GenreId = int.Parse(Request.Form["Genre"]);
            AddMovie.DirectorId = 1;
            AddMovie.DurationMinutes = int.Parse(Request.Form["duration"]);            
            _context.Movies.Add(AddMovie);
            _context.SaveChanges();
            movies = _context.Movies.Include(m => m.Director).Include(m => m.Genre).ToList();
            Genres = _context.Genres.ToList();
            return Page();
        }
        public IActionResult OnPostDelete(int id)
        {
            var movie = _context.Movies.Find(id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
                _context.SaveChanges();
            }
            OnGet();
            return Page();
        }
    }
}
