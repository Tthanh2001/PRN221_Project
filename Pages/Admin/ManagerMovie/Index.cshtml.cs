using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN221_Project.Models;
using PRN221_Project.Utils;

namespace PRN221_Project.Pages.Admin.ManagerMovie
{
    public class IndexModel : PageModel
    {
        private readonly CinphileDbContext _context;

        [BindProperty]
        public List<Movie> movies { get; set; }

        [BindProperty]
        public Movie AddMovie { get; set; }

        public IndexModel(CinphileDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            movies = _context.Movies.ToList();

        }

        public IActionResult OnPost()
        {
            if (AddMovie.ReleaseDate < DateTime.Now)
            {
                ViewData["Message"] = "Đã quá thời gian. Vui lòng chọn ngày khác";
                OnGet();
                return Page();
            }
            else
            {
                AddMovie.MovieIdApi = Request.Form["MovieIdApi"];
                AddMovie.DurationMinutes = int.Parse(Request.Form["duration"]);
                _context.Movies.Add(AddMovie);
                _context.SaveChanges();
                OnGet();
                return Page();
            }

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
