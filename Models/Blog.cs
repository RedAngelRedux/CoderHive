﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoderHive.Models
{
    public class Blog
    {
        // Key Properties
        public int Id { get; set; }

        public string? AuthorId { get; set; }

        // Class Properties
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at most {1} characters", MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(500, ErrorMessage = "The {0} must be at least {2} and at most {1} characters", MinimumLength = 2)]
        public string Description { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [Display(Name = "Created Date")]
        public DateTime Created { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Updated Date")]
        public DateTime? Updated { get; set; }

        [Display(Name="Blog Image")]
        public byte[]? ImageData { get; set; } = Array.Empty<byte>();

        [Display(Name = "Image Type")]
        public string? ImageType { get; set; } = string.Empty;

        [NotMapped]
        public IFormFile? Image { get; set; }

        // Navigation Properties
        public virtual BlogUser? Author { get; set; }

        public virtual ICollection<Post> Posts { get; set; } = new HashSet<Post>();

    }
}
