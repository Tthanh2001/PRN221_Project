using Newtonsoft.Json;
using PRN221_Project.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace PRN221_Project.Models
{
    [Table("Movie")]
    public class Movie
    {
        public Movie()
        {
            this.MovieSchedules = new HashSet<MovieSchedule>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime? ReleaseDate { get; set; }

        [Required]
        public int? DurationMinutes { get; set; }

        public string MovieIdApi { get; set; } = null!;
        public string? Title { get; set; } = null!;
        public bool? IsReleased { get; set; } 
        public virtual ICollection<MovieSchedule> MovieSchedules { get; set; } = null!;

    }
}
