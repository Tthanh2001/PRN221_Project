using PRN221_Project.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace ProjectPRN.Models
{
    [Table("Movie")]
    public class Movie
    {
        public Movie()
        {
            this.Actors = new HashSet<Actor>();
            this.Ratings = new HashSet<Rating>();
            this.MovieSchedules = new HashSet<MovieSchedule>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; } = null!;

        [Required]
        public DateTime ReleaseDate { get; set; }

        [Required]
        public int DurationMinutes { get; set; }

        [StringLength(1000)]
        public string Description { get; set; } = null!;

        public string PosterUrl { get; set; } = null!;

        public string TrailerUrl { get; set; } = null!;

        public int DirectorId { get; set; }
        public virtual Director Director { get; set; } = null!;
        public int GenreId { get; set; }
        public virtual Genre Genre { get; set; } = null!;
        public virtual ICollection<Actor> Actors { get; set; } = null!;
        public virtual ICollection<Rating>? Ratings { get; set; } = null!;
        public virtual ICollection<MovieSchedule> MovieSchedules { get; set; } = null!;

    }
}
