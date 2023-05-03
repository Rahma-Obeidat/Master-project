using Masters3.Data;
using Masters3.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Masters3.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<User> user;
        private readonly SignInManager<User> signInManager;
        private readonly ApplicationDbContext context;

        public AdminController(ILogger<HomeController> logger, UserManager<User> user, ApplicationDbContext context, SignInManager<User> signInManager)
        {
            _logger = logger;
            this.user = user;
            this.context = context;
            this.signInManager = signInManager;
        }
        public IActionResult statistic()
        {
            var auserID = HttpContext.Session.GetString("IdUser");

            var u = context.Users.Where(obj => obj.Id == auserID).FirstOrDefault();
            ViewBag.username = u.UserName;
            ViewBag.image = u.Imagepath;

            var User = context.Users.ToList();
            var category = context.categories.ToList();
            var product = context.products.ToList();
            var order = context.orders.ToList();
            var testimonial = context.testimonials.ToList();

            var tuple = new Tuple<IEnumerable<User>, IEnumerable<Category>, IEnumerable<Product>, IEnumerable<Order>, IEnumerable<Testimonial>>(User, category, product, order, testimonial);
            return View(tuple);
        }
        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult Profile()
        {
            var auserID = HttpContext.Session.GetString("IdUser");

            var u = context.Users.Where(obj => obj.Id == auserID).FirstOrDefault();
            ViewBag.username = u.UserName;
            ViewBag.image = u.Imagepath;

            return View(u);
        }
        [HttpPost]
        public async Task<IActionResult> Profile(string Fname, string password, string email, string phone, IFormFile image)
        {
            var auserID = HttpContext.Session.GetString("IdUser");

            var u = context.Users.Where(obj => obj.Id == auserID).FirstOrDefault();
            ViewBag.username = u.UserName;
            ViewBag.image = u.Imagepath;
            if (u != null)
            {
                if (Fname != null && u.UserName != Fname)
                {
                    u.UserName = Fname;
                }
                if (email != null && u.Email != email)
                {
                    u.Email = email;
                }
                if (phone != null && u.PhoneNumber != phone)
                {
                    u.PhoneNumber = phone;
                }
                var fileName = "";
                if (image != null)
                {
                    fileName = Path.GetFileName(image.FileName);
                    u.Imagepath = image.FileName;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Img", fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(fileStream);
                    }
                }
                if (u.UserName == Fname || u.Email == email || u.PhoneNumber == phone || u.Imagepath == fileName)
                {
                    await user.UpdateAsync(u);
                    await context.SaveChangesAsync();
                }
            }

            return View(u);
        }
        [HttpGet]
        public IActionResult User()
        {
            var auserID = HttpContext.Session.GetString("IdUser");

            var u = context.Users.Where(obj => obj.Id == auserID).FirstOrDefault();
            ViewBag.username = u.UserName;
            ViewBag.image = u.Imagepath;
            return View(context.Users.ToList());  
        }
        [HttpPost]
        public IActionResult User(string name)
        {
            var auserID = HttpContext.Session.GetString("IdUser");

            var u = context.Users.Where(obj => obj.Id == auserID).FirstOrDefault();
            ViewBag.username = u.UserName;
            ViewBag.image = u.Imagepath;
            if (name != null)
            return View(context.Users.Where(obj=>obj.UserName== name).ToList()); 
            else
                return View(context.Users.ToList());
        }

        [HttpGet]
        public IActionResult Testimonial()
        {
            var auserID = HttpContext.Session.GetString("IdUser");

            var u = context.Users.Where(obj => obj.Id == auserID).FirstOrDefault();
            ViewBag.username = u.UserName;
            ViewBag.image = u.Imagepath;

            return View(context.testimonials.Include(obj=>obj.User).ToList());
        }

        [HttpPost]
        public IActionResult Testimonial(string status,int id)
        {
            var auserID = HttpContext.Session.GetString("IdUser");

            var u = context.Users.Where(obj => obj.Id == auserID).FirstOrDefault();
            ViewBag.username = u.UserName;
            ViewBag.image = u.Imagepath;

            var tes = context.testimonials.Where(obj => obj.Id == id).FirstOrDefault();
            tes.Status = status;

            context.Update(tes);
            context.SaveChanges();

            return View(context.testimonials.Include(obj => obj.User).ToList());
        }
    }
}