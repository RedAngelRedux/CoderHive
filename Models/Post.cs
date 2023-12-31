﻿using CoderHive.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoderHive.Models
{

    public class Post
    {

        // Key Properties
        public int Id { get; set; }

        [Display(Name ="Blog Name")]
        public int BlogId { get; set; }

        public string? AuthorId { get; set; }

        // Class Properties        
        [Required]
        [StringLength(75, ErrorMessage = "The {0} must be at least {2} and no more than {1}", MinimumLength = 0)]
        public string PostTitle { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "The {0} must be at least {2} and no more than {1}", MinimumLength = 2)]
        public string Abstract { get; set; }

        [Required]
        public string Content { get; set; }

        [DataType(DataType.Date)]
        [Display(Name ="Created Date")]
        public DateTime Created { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Updated Date")]
        public DateTime? Updated { get; set; }

        public PostStatus Status { get; set; }

        public string? Slug { get; set; }

        //public byte[] ImageData { get; set; } = Array.Empty<byte>();
        public byte[]? ImageData { get; set; }

        //public string ImageType { get; set; } = string.Empty;
        public string? ImageType { get; set; }

        [NotMapped]
        public IFormFile? Image { get; set; }

        // Navigation Properties

        // Child Of...
        public virtual Blog? Blog { get; set; }
        public virtual BlogUser? Author { get; set; }

        // Parent Of...
        public virtual ICollection<Tag> Tags { get; set; } = new HashSet<Tag>();
        public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();

    }

}
