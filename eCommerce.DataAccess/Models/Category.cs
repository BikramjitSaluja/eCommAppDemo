using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCommerce.DataAccess.Models
{
    public partial class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Category Name")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Display Order")]
        public int? DisplayOrder { get; set; }
    }
}
