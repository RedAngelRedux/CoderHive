using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CoderHive.Models
{
    public class BlogUser : IdentityUser
    {

        // Class Properties
        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at most {1} characters", MinimumLength = 2)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = "";

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at most {1} characters", MinimumLength = 2)]
        [Display(Name ="Last Name")]
        public string LastName { get; set; } = "";

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at most {1} characters", MinimumLength = 2)]
        [Display(Name = "Display Name")]
        public string DisplayName { get; set; } = "";

        [Display(Name = "User Image")]
        public byte[]? ImageData { get; set; } = Array.Empty<byte>();

        [Display(Name = "Image Type")]
        public string? ImageType { get; set; } = string.Empty;

        [NotMapped]
        public IFormFile? Image { get; set; }

        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at most {1} characters", MinimumLength = 6)]
        public string? FacebookUrl { get; set; }

        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at most {1} characters", MinimumLength = 6)]
        public string? TwitterUrl { get; set; }

        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at most {1} characters", MinimumLength = 6)]
        public string? LinkedInUrl { get; set; }

        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at most {1} characters", MinimumLength = 6)]
        public string? GitHubUrl { get; set; }

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";

        // Navigation Properties

        // Parent of...
        public virtual ICollection<Blog> Blogs { get; set; } = new HashSet<Blog>();

        public virtual ICollection<Post> Posts { get; set; } = new HashSet<Post>();
    }
}
