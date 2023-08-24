using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN221_Project.Models;
using PRN221_Project.Services;
using PRN221_Project.Utils;
using System.Text.Json;

namespace PRN221_Project.Pages.Booking
{
    public class PaymentModel : PageModel
    {
        public List<Seat> Seats { get; set; } = null!;
        public MovieSchedule MovieSchedule { get; set; } = null!;

        private readonly UserManager<ApplicationAccount> _userManager;
        private readonly CinphileDbContext _context;
        private readonly IVnPayService _vnpay;

        public PaymentModel(CinphileDbContext context, 
            IVnPayService vnpay, UserManager<ApplicationAccount> userManager)
        {
            _context = context;
            _vnpay = vnpay;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync(string selectedSeats, int? scheduleId)
        {
            if (selectedSeats == null || scheduleId == null)
            {
                return BadRequest();
            }

            List<int> bookedSeat = JsonSerializer.Deserialize<List<int>>(selectedSeats)!;

            Seats = await _context.Seats
               .Include(s => s.SeatType)
               .Where(s => bookedSeat.Contains(s.Id))
               .ToListAsync();

            MovieSchedule = await _context.MovieSchedules
                .Include(ms => ms.Movie)
                .Include(ms => ms.Room)
                .FirstOrDefaultAsync(s => s.Id == scheduleId);

            return Page();
        }

        public IActionResult OnPost(int amount) 
        {
            try
            {
                string paymentUrl = _vnpay.CreatePaymentUrl(amount, 1);

                // Return the payment URL as a response
                return Content(paymentUrl);
            }
            catch (Exception ex)
            {
                // Handle any errors that might occur during payment processing
                return BadRequest("Error processing payment: " + ex.Message);
            }
        }
    }
}
