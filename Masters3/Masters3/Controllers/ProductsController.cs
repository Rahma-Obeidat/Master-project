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
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var auserID = HttpContext.Session.GetString("IdUser");

            var u = _context.Users.Where(obj => obj.Id == auserID).FirstOrDefault();
            ViewBag.username = u.UserName;
            ViewBag.image = u.Imagepath;
            var applicationDbContext = _context.products.Include(p => p.Categorys);
            return View(await applicationDbContext.ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> Index(string name)
        {
            var auserID = HttpContext.Session.GetString("IdUser");

            var u = _context.Users.Where(obj => obj.Id == auserID).FirstOrDefault();
            ViewBag.username = u.UserName;
            ViewBag.image = u.Imagepath;
            if (name != null)
                return View(_context.products.Where(obj => obj.Name == name).Include(p => p.Categorys).ToList());
            else
                return View(_context.products.Include(p => p.Categorys).ToList());
        }
        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var auserID = HttpContext.Session.GetString("IdUser");

            var u = _context.Users.Where(obj => obj.Id == auserID).FirstOrDefault();
            ViewBag.username = u.UserName;
            ViewBag.image = u.Imagepath;
            if (id == null || _context.products == null)
            {
                return NotFound();
            }

            var product = await _context.products
                .Include(p => p.Categorys)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            var auserID = HttpContext.Session.GetString("IdUser");

            var u = _context.Users.Where(obj => obj.Id == auserID).FirstOrDefault();
            ViewBag.username = u.UserName;
            ViewBag.image = u.Imagepath;
            ViewData["CategoryId"] = new SelectList(_context.categories, "Id", "CategoryName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Quantity,Price,ImagePath,CategoryId")] Product product, IFormFile image  )
        {
            var auserID = HttpContext.Session.GetString("IdUser");

            var u = _context.Users.Where(obj => obj.Id == auserID).FirstOrDefault();
            ViewBag.username = u.UserName;
            ViewBag.image = u.Imagepath;
            if (!ModelState.IsValid)
            {
                var fileName = Path.GetFileName(image.FileName);
                product.ImagePath = image.FileName;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Img", fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.categories, "Id", "Id", product.CategoryId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var auserID = HttpContext.Session.GetString("IdUser");

            var u = _context.Users.Where(obj => obj.Id == auserID).FirstOrDefault();
            ViewBag.username = u.UserName;
            ViewBag.image = u.Imagepath;
            if (id == null || _context.products == null)
            {
                return NotFound();
            }

            var product = await _context.products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.categories, "Id", "CategoryName", product.CategoryId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Quantity,Price,ImagePath,CategoryId")] Product product , IFormFile image)
        {
            var auserID = HttpContext.Session.GetString("IdUser");

            var u = _context.Users.Where(obj => obj.Id == auserID).FirstOrDefault();
            ViewBag.username = u.UserName;
            ViewBag.image = u.Imagepath;
            if (id != product.Id)
            {
                return NotFound();
            }

            Product c = _context.products.Find(id);
            c.Name = product.Name;
            c.Price = product.Price;
            c.Description = product.Description;
            c.Quantity = product.Quantity;
            //c.ImagePath = product.ImagePath;

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
            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(c);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.categories, "Id", "Id", product.CategoryId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var auserID = HttpContext.Session.GetString("IdUser");

            var u = _context.Users.Where(obj => obj.Id == auserID).FirstOrDefault();
            ViewBag.username = u.UserName;
            ViewBag.image = u.Imagepath;
            if (id == null || _context.products == null)
            {
                return NotFound();
            }

            var product = await _context.products
                .Include(p => p.Categorys)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var auserID = HttpContext.Session.GetString("IdUser");

            var u = _context.Users.Where(obj => obj.Id == auserID).FirstOrDefault();
            ViewBag.username = u.UserName;
            ViewBag.image = u.Imagepath;
            if (_context.products == null)
            {
                return Problem("Entity set 'ApplicationDbContext.products'  is null.");
            }
            var product = await _context.products.FindAsync(id);
            if (product != null)
            {
                _context.products.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return (_context.products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
