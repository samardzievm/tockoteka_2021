using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tockoteka.Models.ViewModels
{
    public class BlogVM
    {
        public Blog Blog { get; set; }
        //public BlogCategory Categories { get; set; }
        public IEnumerable<SelectListItem> CategorySelectList { get; set; }
    }
}
