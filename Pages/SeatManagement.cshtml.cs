using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NuGet.Packaging;
using PRN221_Project.Models;
using PRN221_Project.Utils;

namespace PRN221_Project.Pages
{
    public class SelectedSeat
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public int TypeId { get; set; }
        public string SeatName { get; set; } = null!;
        public bool Available { get; set; }
        public int RoomId { get; set; }
    }

    public class SeatManagementModel : PageModel
    {
        private readonly CinphileDbContext _context;

        [BindProperty]
        public Room Room { get; set; } = null!;
        [BindProperty]
        public List<SeatType> SeatType { get; set; } = null!;

        public SeatManagementModel(CinphileDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            SeatType = await _context.SeatTypes.ToListAsync();

            if (id == null)
            {
                return NotFound();
            }

            Room = await _context.Rooms
                .Include(r => r.Seats)
                .ThenInclude(s => s.SeatType)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (Room == null)
            {
                return NotFound();  // Room not found
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync([FromBody] List<SelectedSeat> selectedSeats)
        {
            if (selectedSeats == null)
            {
                return NotFound();
            }

            Room = await _context.Rooms
                .Include(r => r.Seats)
                .FirstOrDefaultAsync(r => r.Id == selectedSeats[0].RoomId);

            if (Room == null)
            {
                return NotFound();  // Room not found
            }

            if (Room.Seats.Count == 0)
            {
                foreach (var seat in selectedSeats)
                {
                    await _context.Seats.AddAsync(new Seat
                    {
                        SeatCol = seat.Col,
                        SeatRow = seat.Row,
                        IsBookable = seat.Available,
                        SeatName = seat.SeatName,
                        RoomId = seat.RoomId,
                        SeatTypeId = seat.TypeId
                    });
                }
            }

            else
            {
                var existingSeats = Room.Seats.ToDictionary(seat => (seat.SeatRow, seat.SeatCol));
                var seatsToDelete = new List<Seat>();

                foreach (var selectedSeat in selectedSeats)
                {
                    var seatKey = (selectedSeat.Row, selectedSeat.Col);
                    if (existingSeats.TryGetValue(seatKey, out var existingSeat))
                    {
                        // Update existing seat attributes
                        existingSeat.SeatTypeId = selectedSeat.TypeId;
                        existingSeat.IsBookable = selectedSeat.Available;
                        existingSeat.SeatName = selectedSeat.SeatName;

                        // Remove seat from dictionary to avoid duplicate updates
                        existingSeats.Remove(seatKey);
                    }
                    else
                    {
                        //Add not exist seat
                        await _context.Seats.AddAsync(new Seat
                        {
                            SeatCol = selectedSeat.Col,
                            SeatRow = selectedSeat.Row,
                            IsBookable = selectedSeat.Available,
                            SeatName = selectedSeat.SeatName,
                            RoomId = selectedSeat.RoomId,
                            SeatTypeId = selectedSeat.TypeId
                        });
                    }
                }

                // Mark seats for deletion
                seatsToDelete.AddRange(existingSeats.Values);

                // Remove seats marked for deletion from both database and room's collection
                if (seatsToDelete.Any())
                {
                    _context.Seats.RemoveRange(seatsToDelete);
                    var seatsToKeep = Room.Seats.Where(seat => !seatsToDelete.Contains(seat)).ToList();
                    Room.Seats.Clear();
                    Room.Seats.AddRange(seatsToKeep);
                }
            }

            await _context.SaveChangesAsync();

            return Page();
        }
    }
}
