using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN221_Project.Models;
using PRN221_Project.Utils;

namespace PRN221_Project.Pages.Booking
{
    public class MovieBookingModel : PageModel
    {
        private readonly CinphileDbContext _context;

        [BindProperty]
        public MovieSchedule MovieSchedule { get; set; } = null!;

        public MovieBookingModel(CinphileDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {

            MovieSchedule = await _context.MovieSchedules
                .Include(ms => ms.Room)
                .ThenInclude(ms => ms.Seats)
                .ThenInclude(s => s.SeatType)
                .Include(ms => ms.SeatBookings)
                .FirstOrDefaultAsync(r => r.Id == 1);  //Hardcode

            if (MovieSchedule == null)
            {
                return NotFound();  // Schedule not found
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync([FromBody] List<int> selectedSeats)
        {
            return Page();
        }
    }
}
