using ProjectPRN.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRN221_Project.Models
{
    public class Bill
    {
        public Bill()
        {
            this.SeatBookings = new HashSet<SeatBooking>();
        }
        [Key]
        public int Id { get; set; }
        public DateTime BookingDate { get; set; }
        [Column(TypeName="money")]
        public decimal TotalMoney { get; set; }
        public string ApplicationAccountId { get; set; } = null!;
        public virtual ApplicationAccount ApplicationAccount { get; set; } = null!;
        public virtual ICollection<SeatBooking> SeatBookings { get; set; } = null!;
    }
}
