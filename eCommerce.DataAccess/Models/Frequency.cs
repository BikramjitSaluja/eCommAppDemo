using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCommerce.DataAccess.Models
{
    public partial class Frequency
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Frequency Name")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Frequency Count")]
        public int FrequencyCount { get; set; }
    }
}
