using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tockoteka.Models.ViewModels
{
    public class BlogUserVM
    {
        public Blog Blog { get; set; }
        public IEnumerable<SelectListItem> CategorySelectList { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
