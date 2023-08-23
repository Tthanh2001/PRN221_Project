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
            EditMovie.MovieIdApi = Request.Form["MovieIdApi"];
            EditMovie.ReleaseDate = DateTime.Parse(Request.Form["ReleaseDate"]);
            EditMovie.DurationMinutes = int.Parse(Request.Form["duration"]);
            if (Request.Form["Status"] == 0)
            {
                EditMovie.IsReleased = false;
            }
            else if (Request.Form["Status"] == 1)
            {
                EditMovie.IsReleased = true;
            }
            else
            {
                EditMovie.IsReleased = null;
            }
            _context.Attach(EditMovie).State = EntityState.Modified;
            _context.SaveChanges();
            return RedirectToPage("/Admin/ManagerMovie/Index");
        }
    }
}
