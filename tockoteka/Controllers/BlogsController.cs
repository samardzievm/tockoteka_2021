using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using tockoteka.Data;
using tockoteka.Models;
using tockoteka.Models.ViewModels;


namespace tockoteka.Controllers
{
    public class BlogsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment; // to access the WC.cs class where we store global variables  

        public BlogsController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Blogs
        public IActionResult Index()
        {
            BlogCategoryVM blogCategoryVM = new BlogCategoryVM()
            {
                Blogs = _db.Blog
                    .FromSqlRaw("SELECT * FROM blog").OrderByDescending(o => o.Id) // sql query
                    .Include(c => c.BlogCategory), // foreign key
                Categories = _db.BlogCategory
            };

            return View(blogCategoryVM);

            //IEnumerable<Blog> objList = _db.Blog
            //    .FromSqlRaw("SELECT * FROM blog").OrderByDescending(o => o.Id) // sql query
            //    .Include(u => u.BlogCategory); // foreign key

            //return View(objList);
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
        [Authorize]
        public ActionResult Create()
        {
            // get the full name of the user approach
            var claimsIdentity = (ClaimsIdentity)User.Identity; // get the identity of the user
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier); // match the user

            BlogUserVM blogUserVM = new BlogUserVM()
            {
                Blog = new Blog(),
                CategorySelectList = _db.BlogCategory.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                ApplicationUser = _db.ApplicationUser.FirstOrDefault(u => u.Id == claim.Value)
            };
            return View(blogUserVM);
            //BlogVM blogVM = new BlogVM()
            //{
            //    Blog = new Blog(),
            //    CategorySelectList = _db.BlogCategory.Select(i => new SelectListItem
            //    {
            //        Text = i.Name,
            //        Value = i.Id.ToString()
            //    })
            //};
            //return View(blogVM);
        }

        // POST: Blogs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BlogVM blogVM)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;

                if (blogVM.Blog.Id == 0)
                {
                    

                    string upload = webRootPath + WC.BlogImagePath; // where we want to store(upload) the images
                    string fileName = Guid.NewGuid().ToString(); // what is the FileName we want to give that will be uploaded in the folder... Guid is random name
                    string extension = Path.GetExtension(files[0].FileName); // get the extension from files

                    // upload functionality

                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    blogVM.Blog.Cover = fileName + extension;
                    

                }
            }

            _db.Blog.Add(blogVM.Blog);
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));    
        }

        

        // GET: Blogs/Edit/5
        [Authorize(Roles = WC.AdminRole)]
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
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;

                var objFromDb = _db.Blog.AsNoTracking().FirstOrDefault(u => u.Id == blogVM.Blog.Id); // retrieve data from the database. Because EF can track only 1 file at the time, we don't need to track this file from EF, and we use the method AsNoTracking()

                if (files.Count > 0)
                {
                    // upload the new file
                    string upload = webRootPath + WC.BlogImagePath; // where we want to store(upload) the images
                    string fileName = Guid.NewGuid().ToString(); // what is the FileName we want to give that will be uploaded in the folder... Guid is random name
                    string extension = Path.GetExtension(files[0].FileName); // get the extension from files

                    // remove the old file
                    var oldFile = Path.Combine(upload, objFromDb.Cover); // get the path from the app

                    if (System.IO.File.Exists(oldFile))
                    {
                        System.IO.File.Delete(oldFile);
                    }

                    // add the new file
                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    blogVM.Blog.Cover = fileName + extension;
                }
                else
                {
                    // if the image is not edited, then save/keep the same image
                    blogVM.Blog.Cover = objFromDb.Cover;
                }

                _db.Blog.Update(blogVM.Blog);
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // GET: Blogs/Delete/5
        [Authorize(Roles = WC.AdminRole)]
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

            // delete the stored image cover

            string upload = _webHostEnvironment.WebRootPath + WC.BlogImagePath;
            var oldFile = Path.Combine(upload, obj.Cover);

            if (System.IO.File.Exists(oldFile))
            {
                System.IO.File.Delete(oldFile);
            }

            _db.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
