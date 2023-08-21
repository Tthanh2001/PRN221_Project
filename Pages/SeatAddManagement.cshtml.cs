using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PRN221_Project.Models;
using PRN221_Project.Utils;

namespace PRN221_Project.Pages
{
    public class SelectedSeat
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public int TypeId { get; set; } 
    }

    public class SeatManagementModel : PageModel
    {
        private readonly CinphileDbContext _context;
        public int RoomId;
        public List<SeatType> SeatType { get; set; } = null!;

        public SeatManagementModel(CinphileDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            SeatType = await _context.SeatTypes.ToListAsync();
            RoomId = 1; //Hardcode
            return Page();
        }

        public async Task<IActionResult> OnPostAsync([FromBody] List<SelectedSeat> selectedSeats)
        {
            if (selectedSeats == null)
            {
                return new JsonResult("Selected seats saved successfully.");
            }

            foreach (var seat in selectedSeats)
            {
                await _context.Seats.AddAsync(new Seat
                {
                    SeatCol = seat.Col,
                    SeatRow = seat.Row,
                    IsBookable = true,
                    RoomId = 1, //Hardcode
                    SeatTypeId = seat.TypeId
                });
            }

            await _context.SaveChangesAsync();

            return new JsonResult("Selected seats saved successfully.");
        }
    }
}
