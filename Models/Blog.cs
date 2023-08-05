﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoderHive.Models
{
    public class Blog
    {

        public int Id { get; set; }

        public string AuthorId { get; set; }

        [Required]
        [StringLength(100,ErrorMessage="The {0} must be at least {2} and at most {1} characters",MinimumLength=2)]
        public string Name { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "The {0} must be at least {2} and at most {1} characters", MinimumLength = 2)]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Created Date")]
        public DateTime Created { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Created Date")]
        public DateTime? Updated { get; set; }

        [Display(Name="Blog Image")]
        public byte[] ImageData { get; set; }

        [Display(Name = "Image Type")]
        public string ImageType { get; set; }

        [NotMapped]
        public IFormFile Image { get; set; }

    }
}