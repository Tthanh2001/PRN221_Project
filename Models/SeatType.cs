using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRN221_Project.Models
{
    public class SeatType
    {
        public SeatType()
        {
            this.Seats = new HashSet<Seat>();
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        [Column(TypeName = "money")]
        public decimal Price { get; set; }
        public string SeatColor { get; set; }
        public virtual ICollection<Seat> Seats { get; set; } = null!;
    }
}
