using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN221_Project.Models;
using PRN221_Project.Utils;

namespace PRN221_Project.Pages.Admin.ManagerRoom
{
    [Authorize(Roles = "Admin, Vip, Editor")]
    public class EditModel : PageModel
    {

        private readonly CinphileDbContext _context;

        [BindProperty]
        public Room rooms { get; set; }

        public EditModel(CinphileDbContext context)
        {
            _context = context;
        }
        public void OnGet(int? id)
        {
            rooms = _context.Rooms.FirstOrDefault(r => r.Id == id);
        }
        public IActionResult OnPost()
        {
            rooms.RoomName = Request.Form["RoomName"];
            rooms.NumberOfRows = int.Parse(Request.Form["NumberOfRows"]);
            rooms.NumberOfCols = int.Parse(Request.Form["NumberOfCols"]);
            _context.Attach(rooms).State = EntityState.Modified;
            _context.SaveChanges();
            return RedirectToPage("/Admin/ManagerRoom/index");
        }
    }
}
