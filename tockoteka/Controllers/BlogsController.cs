using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tockoteka.Data;
using tockoteka.Models;
using tockoteka.Models.ViewModels;

namespace tockoteka.Controllers
{
    public class BlogsController : Controller
    {
        private readonly ApplicationDbContext _db;
        public BlogsController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: Blogs
        public ActionResult Index()
        {
            IEnumerable<Blog> objList = _db.Blog
                .FromSqlRaw("SELECT * FROM blog").OrderByDescending(o => o.Id) // sql query
                .Include(u => u.BlogCategory); // foreign key

            return View(objList);
        }

        // GET: Blogs/Details/5
        public ActionResult Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var blog = _db.Blog.Include(b => b.BlogCategory).FirstOrDefault(i => i.Id == id);
            return View(blog);
        }

        // GET: Blogs/Create
        public ActionResult Create()
        {
            BlogVM blogVM = new BlogVM()
            {
                Blog = new Blog(),
                CategorySelectList = _db.BlogCategory.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };
            return View(blogVM);
        }

        // POST: Blogs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BlogVM blogVM)
        {
            if (ModelState.IsValid)
            {
                _db.Add(blogVM.Blog);
            }

            _db.SaveChanges();

            return RedirectToAction(nameof(Index));    
        }

        // GET: Blogs/Edit/5
        public ActionResult Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            //var blog = _db.Blog.Include(b => b.BlogCategory).FirstOrDefault(i => i.Id == id);

            BlogVM blogVM = new BlogVM()
            {
                Blog = new Blog(),
                CategorySelectList = _db.BlogCategory.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };

            blogVM.Blog = _db.Blog.Find(id);

            if (blogVM.Blog == null)
            {
                return NotFound();
            }

            return View(blogVM);
        }

        // POST: Blogs/Edit/5
        [HttpPost, ActionName("Edit")] // , ActionName("Edit")
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(BlogVM blogVM)
        {
            if (ModelState.IsValid)
            {
                _db.Blog.Update(blogVM.Blog);
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // GET: Blogs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Blog obj = _db.Blog.Include(c => c.BlogCategory).FirstOrDefault(u => u.Id == id);
            if(obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        // POST: Blogs/Delete/5
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(int? id)
        {
            var obj = _db.Blog.Find(id);

            if (obj == null)
            {
                return NotFound();
            }

            _db.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
