using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN221_Project.Models;
using PRN221_Project.Utils;

namespace PRN221_Project.Pages.Admin.ManagerMovie
{
    [Authorize(Roles = "Admin, Vip, Editor")]
    public class EditModel : PageModel
    {
       
        private readonly CinphileDbContext _context;
        public EditModel(CinphileDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Movie EditMovie { get; set; }

        [BindProperty]
        public List<Genre> Genres { get; set; }
        public void OnGet(int? id)
        {
            Console.WriteLine(id);
            EditMovie = _context.Movies.FirstOrDefault(x => x.Id == id);
            Genres = _context.Genres.ToList();    
        }
        public IActionResult OnPost()
        {
            Console.WriteLine(EditMovie);
            EditMovie.Title = Request.Form["title"];
            EditMovie.ReleaseDate = DateTime.Parse(Request.Form["ReleaseDate"]);
            EditMovie.DurationMinutes = int.Parse(Request.Form["duration"]);
            EditMovie.Description = Request.Form["description"];
            EditMovie.PosterUrl = Request.Form["posterUrl"];
            EditMovie.TrailerUrl = Request.Form["trailerUrl"];
            EditMovie.GenreId = int.Parse(Request.Form["Genre"]);
            EditMovie.DirectorId = 1;
            _context.Attach(EditMovie).State = EntityState.Modified;
            _context.SaveChanges();
            return RedirectToPage("/Admin/ManagerMovie/Index");
        }
    }
}
