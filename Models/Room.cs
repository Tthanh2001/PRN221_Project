using PRN221_Project.Models;
using System.ComponentModel.DataAnnotations;

namespace PRN221_Project.Models
{
    public class Room
    {
        public Room()
        {
            this.MovieSchedules = new HashSet<MovieSchedule>();
            this.Seats = new HashSet<Seat>();
        }
        [Key]
        public int Id { get; set; }
        public string RoomName { get; set; } = null!;
        [Range(1, 50)]
        public int NumberOfCols { get; set;}
        [Range(1, 50)]
        public int NumberOfRows { get; set;}
        public virtual ICollection<MovieSchedule> MovieSchedules { get; set; } = null!;
        public virtual ICollection<Seat> Seats { get; set; } = null!;
    }
}
