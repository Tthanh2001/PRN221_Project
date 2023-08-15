using ProjectPRN.Models;

namespace PRN221_Project.Models
{
    public class SeatBooking
    {
        public int SeatId { get; set; }
        public virtual Seat Seat { get; set; } = null!;
        public int MovieScheduleId { get; set; }
        public virtual MovieSchedule MovieSchedule { get; set; } = null!;
        public int BillId { get; set; }
        public virtual Bill Bill { get; set; } = null!;
    }
}
