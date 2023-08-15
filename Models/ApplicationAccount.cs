using Microsoft.AspNetCore.Identity;
using PRN221_Project.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectPRN.Models
{
    [Table("ApplicationAccount")]
    public class ApplicationAccount : IdentityUser
    {
        public ApplicationAccount()
        {
            this.Ratings = new HashSet<Rating>();
            this.Bills = new HashSet<Bill>();
        }
        public string FullName { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public DateTime RegistrationDate { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; } = null!;
        public virtual ICollection<Bill> Bills { get; set; } = null!;
    }
}
