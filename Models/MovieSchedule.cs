using PRN221_Project.Models;
using System.ComponentModel.DataAnnotations;

namespace PRN221_Project.Models
{
    public class MovieSchedule
    {
        public MovieSchedule()
        {
            this.SeatBookings = new HashSet<SeatBooking>();
        }

        [Key]
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int MovieId { get; set; }
        public virtual Movie Movie { get; set; } = null!;
        public int RoomId { get; set; }
        public virtual Room Room { get; set; } = null!;
        public virtual ICollection<SeatBooking> SeatBookings { get; set; } = null!;
    }
}
