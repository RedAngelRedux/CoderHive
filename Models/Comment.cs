using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CoderHive.Models
{
    public class Comment
    {
        // Key Properties
        public int Id { get; set; }

        public int PostId { get; set; }

        public string AuthorId { get; set; }

        public string ModeratorId { get; set; }

        // Comment Properties
        [Required]
        [StringLength(500,ErrorMessage = "The {0} must be at least {2} and nomore than {1}", MinimumLength =2)]
        [Display(Name ="Comment")]
        public string Body { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Created Date")]
        public DateTime Created { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Updated Date")]
        public DateTime? Updated { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Moderated Date")]
        public DateTime? Moderated { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Deleted { get; set; }

        [StringLength(500, ErrorMessage = "The {0} must be at least {2} and nomore than {1}", MinimumLength = 2)]
        [Display(Name = "Moderated Comment")]
        public string? ModeratedBody { get; set; }

        // Navigation Properties

        // Child of...
        public virtual Post Post { get; set; }

        public virtual IdentityUser Author { get; set; }

        public virtual IdentityUser Moderator { get; set; }




    }
}
