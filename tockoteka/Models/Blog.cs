using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace tockoteka.Models
{
    public class Blog
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Наслов")]
        [Required]
        public string Title { get; set; }
        [Display(Name = "Краток опис")]
        public string Description { get; set; }
        [Display(Name = "Содржина")]
        public string Content { get; set; }
        [Display(Name = "Насловна слика")]
        public string Cover { get; set; }
        [Display(Name = "Датум на објава")]
        public string Date { get; set; }
        [Display(Name = "Автор")]
        public string Author { get; set; }
        public int CategoryId { get; set; }
        [Display(Name = "Категорија")]
        [ForeignKey("CategoryId")]
        public virtual BlogCategory BlogCategory { get; set; }
    }
}
