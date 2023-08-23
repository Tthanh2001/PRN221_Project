using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN221_Project.Models;
using PRN221_Project.Utils;

namespace PRN221_Project.Pages.Admin.ManagerMovie
{
    public class EditModel : PageModel
    {
        private readonly CinphileDbContext _context;
        public EditModel(CinphileDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Movie EditMovie { get; set; }
        public void OnGet(int? id)
        {
            Console.WriteLine(id);
            EditMovie = _context.Movies.FirstOrDefault(x => x.Id == id);
            
        }
        public IActionResult OnPost()
        {
            Console.WriteLine(EditMovie);
            EditMovie.Id = int.Parse(Request.Form["title"]);
            EditMovie.ReleaseDate = DateTime.Parse(Request.Form["ReleaseDate"]);
            EditMovie.DurationMinutes = int.Parse(Request.Form["duration"]);           
            EditMovie.IsReleased = false;
            _context.Attach(EditMovie).State = EntityState.Modified;
            _context.SaveChanges();
            return RedirectToPage("/Admin/ManagerMovie/Index");
        }
    }
}
