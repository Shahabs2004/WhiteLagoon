﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace WhiteLagoon.Domain.Entities
{
    public class Villa
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Villa Name")]
        [Required(ErrorMessage = "The Villa Name is required.")]
        [StringLength(100, ErrorMessage = "The Villa Name cannot exceed 100 characters.")]
        [MinLength(5, ErrorMessage = "The Villa Name must be at least 5 characters long.")]
        public string Name { get; set; }

        [Display(Name = "Villa Description")]
        public string? Description { get; set; }

        [Display(Name = "Price Per Night")]
        [Range(0, 9999, ErrorMessage = "The Price Per Night must be between 0 and 9999.")]
        public double Price { get; set; }

        [Display(Name = "Square Feet")]
        [Range(10, 500, ErrorMessage = "The Square Feet must be between 10 and 500.")]
        public int Sqft { get; set; }

        [Display(Name = "Number of Bedrooms")]
        [Range(1, 10, ErrorMessage = "The Number of Bedrooms must be between 1 and 10.")]
        public int Occupancy { get; set; }

        [Display(Name = "Image Url")]
        public string? ImageUrl { get; set; }

        [NotMapped]
        public IFormFile? Image { get; set; }

        public DateTime? Created_Date { get; set; }
        public DateTime? Update_Date { get; set; }

        [NotMapped]
        public bool IsAvailable { get; set; } = true;

        [NotMapped]
        [Display(Name = "Villa Amenities")]
        public IEnumerable<Amenity> VillaAmenity { get; set; }
    }
}
