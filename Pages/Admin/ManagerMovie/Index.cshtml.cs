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

        public IndexModel(CinphileDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            movies = _context.Movies.Include(m => m.Director).Include(m => m.Genre).ToList();
        }
    }
}
