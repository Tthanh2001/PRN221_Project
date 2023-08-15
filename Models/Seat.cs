using System.ComponentModel.DataAnnotations;

namespace PRN221_Project.Models
{
    public class Seat
    {
        public Seat()
        {
            this.SeatBookings = new HashSet<SeatBooking>();
        }

        [Key]
        public int Id { get; set; }
        public string SeatPosition { get; set; } = null!;
        public bool IsBookable { get; set; }
        public int RoomId { get; set; }
        public virtual Room Room { get; set; } = null!;
        public int SeatTypeId { get; set; }
        public virtual SeatType SeatType { get; set; } = null!;

        public virtual ICollection<SeatBooking> SeatBookings { get; set; } = null!;
    }
}
