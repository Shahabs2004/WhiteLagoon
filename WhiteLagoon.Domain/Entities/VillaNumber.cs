using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhiteLagoon.Domain.Entities
{
    public class VillaNumber
    {
        // Represents the primary key of the VillaNumber entity
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Villa Number")]
        public int Villa_Number { get; set; }

        // Represents the foreign key to the Villa entity
        [ForeignKey("Villa")]
        [Display(Name = "Villa Id")]
        public int VillaId { get; set; }

        // Represents the navigation property to the Villa entity
        [ValidateNever]
        public Villa Villa { get; set; }

        // Represents the special details of the VillaNumber
        public string? SpecialDetails { get; set; }
    }
}
