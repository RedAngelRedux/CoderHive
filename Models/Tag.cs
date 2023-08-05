using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CoderHive.Models
{
    public class Tag
    {
        // Key Properties
        public int Id { get; set; }

        public int PostId { get; set; }

        public string AuthorId { get; set; }

        // Class Properties
        [Required]
        [StringLength(25, ErrorMessage = "The {0} must be at least {2} and nomore than {1}", MinimumLength = 2)]
        public string Text { get; set; }

        // Navigation Properies
        public virtual Post Post { get; set; }

        public virtual IdentityUser Author { get; set; }


    }
}
