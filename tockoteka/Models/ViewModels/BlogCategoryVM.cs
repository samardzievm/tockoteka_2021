using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tockoteka.Models.ViewModels
{
    public class BlogCategoryVM
    {
        public IEnumerable<Blog> Blogs { get; set; }
        public IEnumerable<BlogCategory> Categories { get; set; }
    }
}
