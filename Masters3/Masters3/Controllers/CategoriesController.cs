using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Masters3.Data;
using Masters3.Models;

namespace Masters3.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {

            var auserID = HttpContext.Session.GetString("IdUser");

            var u = _context.Users.Where(obj => obj.Id == auserID).FirstOrDefault();
            ViewBag.username = u.UserName;
            ViewBag.image = u.Imagepath;
            return _context.categories != null ? 
                          View(await _context.categories.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.categories'  is null.");
        }

        [HttpPost]
        public async Task<IActionResult> Index(string name)
        {
            var auserID = HttpContext.Session.GetString("IdUser");

            var u = _context.Users.Where(obj => obj.Id == auserID).FirstOrDefault();
            ViewBag.username = u.UserName;
            ViewBag.image = u.Imagepath;
            if (name != null)
                return View(_context.categories.Where(obj => obj.CategoryName == name).ToList());
            else
                return View(_context.categories.ToList());
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var auserID = HttpContext.Session.GetString("IdUser");

            var u = _context.Users.Where(obj => obj.Id == auserID).FirstOrDefault();
            ViewBag.username = u.UserName;
            ViewBag.image = u.Imagepath;
            if (id == null || _context.categories == null)
            {
                return NotFound();
            }

            var category = await _context.categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            var auserID = HttpContext.Session.GetString("IdUser");

            var u = _context.Users.Where(obj => obj.Id == auserID).FirstOrDefault();
            ViewBag.username = u.UserName;
            ViewBag.image = u.Imagepath;
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CategoryName,ImagePath")] Category category,IFormFile image)
        {
            var auserID = HttpContext.Session.GetString("IdUser");

            var u = _context.Users.Where(obj => obj.Id == auserID).FirstOrDefault();
            ViewBag.username = u.UserName;
            ViewBag.image = u.Imagepath;
            if (!ModelState.IsValid)
            {

                var fileName = Path.GetFileName(image.FileName);
                category.ImagePath = image.FileName;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Img", fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var auserID = HttpContext.Session.GetString("IdUser");

            var u = _context.Users.Where(obj => obj.Id == auserID).FirstOrDefault();
            ViewBag.username = u.UserName;
            ViewBag.image = u.Imagepath;
            if (id == null || _context.categories == null)
            {
                return NotFound();
            }

            var category = await _context.categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CategoryName,ImagePath")] Category category, IFormFile image)
        {
            var auserID = HttpContext.Session.GetString("IdUser");

            var u = _context.Users.Where(obj => obj.Id == auserID).FirstOrDefault();
            ViewBag.username = u.UserName;
            ViewBag.image = u.Imagepath;
            if (id != category.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                if (id != category.Id)
                {
                    return NotFound();
                }
                Category c = _context.categories.Find(id);

                c.CategoryName = category.CategoryName;

                if (image != null)
                {
                    var fileName = Path.GetFileName(image.FileName);
                    c.ImagePath = image.FileName;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Img", fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(fileStream);
                    }
                }
                if(!ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(c);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!CategoryExists(category.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
            }
                return View(category);
            
        }
        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var auserID = HttpContext.Session.GetString("IdUser");

            var u = _context.Users.Where(obj => obj.Id == auserID).FirstOrDefault();
            ViewBag.username = u.UserName;
            ViewBag.image = u.Imagepath;
            if (id == null || _context.categories == null)
            {
                return NotFound();
            }

            var category = await _context.categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var auserID = HttpContext.Session.GetString("IdUser");

            var u = _context.Users.Where(obj => obj.Id == auserID).FirstOrDefault();
            ViewBag.username = u.UserName;
            ViewBag.image = u.Imagepath;
            if (_context.categories == null)
            {
                return Problem("Entity set 'ApplicationDbContext.categories'  is null.");
            }
            var category = await _context.categories.FindAsync(id);
            if (category != null)
            {
                _context.categories.Remove(category);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
          return (_context.categories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
