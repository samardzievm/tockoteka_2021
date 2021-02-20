using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace tockoteka.Models
{
    public class BlogCategory
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Име на категорија")]
        [Required]
        public string Name { get; set; }
    }
}
