using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectPRN.Models
{
    [Table("Actor")]
    public class Actor
    {
        public Actor()
        {
            this.Movies = new HashSet<Movie>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; } = null!;

        public virtual ICollection<Movie> Movies { get; set; } = null!;
    }
}
