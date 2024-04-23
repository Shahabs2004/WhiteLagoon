using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhiteLagoon.Domain.Entities
{
    public class Villa
    {
        public int Id { get; set; }
        [Display(Name = "Villa Name")]
        public required string Name { get; set; }
        [Display(Name = "Villa Description")]
        public string? Description { get; set; }
        [Display(Name = "Price Per Night")]
        [Range(0, 9999, ErrorMessage = "The Price Per Night must be between 0 and 9999.")]
        public double Price { get; set; }
        [Display(Name = "Square Feet")]
        [Range(10,500,ErrorMessage = "Could not Be very low or high")]
        public int Sqft { get; set; }
        [Display(Name = "Number of Bedrooms")]
        public int Occupancy { get; set; }
        [Display(Name = "Image Url")]
        public string? ImageUrl { get; set; }
        public DateTime? Created_Date { get; set; }
        public DateTime? Update_Date { get; set; }

    }
}
