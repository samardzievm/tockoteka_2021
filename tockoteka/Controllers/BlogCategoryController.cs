using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tockoteka.Data;
using tockoteka.Models;

namespace tockoteka.Controllers
{
    public class BlogCategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public BlogCategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        // get: /BlogCategory/
        public IActionResult Index()
        {
            IEnumerable<BlogCategory> objList = _db.BlogCategory;
            return View(objList);
        }
        // get: Create
        public IActionResult Create()
        {
            return View();
        }

        // post: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BlogCategory obj)
        {
            _db.Add(obj);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // get: Edit
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var obj = _db.BlogCategory.Find(id);

            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        // post: Edit
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Edit(BlogCategory obj)
        {
            if(ModelState.IsValid)
            {
                _db.BlogCategory.Update(obj);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }

        // get: delete
        public ActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _db.BlogCategory.Find(id);

            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        // post: delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            var obj = _db.BlogCategory.Find(id);

            if (obj == null)
            {
                return NotFound();
            }

            _db.BlogCategory.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
