using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRN221_Project.Models
{
    [Table("Review")]
    public class Rating
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Range(0, 5)]
        public int Rates { get; set; }
        public string ApplicationAccountId { get; set; } = null!;
        public int MovieId { get; set; }
        public virtual ApplicationAccount ApplicationAccount { get; set; } = null!;
        public virtual Movie Movie { get; set; } = null!;
    }
}
