using Masters3.Data;
using Masters3.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Masters3.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<User> user;
        private readonly SignInManager<User> signInManager;
        private readonly ApplicationDbContext context;

        public UserController(ILogger<HomeController> logger, UserManager<User> user, ApplicationDbContext context, SignInManager<User> signInManager)
        {
            _logger = logger;
            this.user = user;
            this.context = context;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult UserProfile()
        {
			ViewBag.RoleID = HttpContext.Session.GetString("Role");
			var auserID = HttpContext.Session.GetString("IdUser");

            var u = context.Users.Where(obj => obj.Id == auserID).FirstOrDefault();
            ViewBag.username = u.UserName;
            ViewBag.image = u.Imagepath;

            return View(u);
        }
        [HttpPost]
        public async Task<IActionResult> UserProfile(string Fname, string password, string email, string phone, IFormFile image)
        {
			ViewBag.RoleID = HttpContext.Session.GetString("Role");
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
        public IActionResult Testimonial()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Testimonial(string message)
        {
            if(message != null)
            {
                var auserID = HttpContext.Session.GetString("IdUser");
                Testimonial t = new Testimonial();
                t.CommentUser = message;
                t.UserId = auserID;
                t.Status = "0";
                context.testimonials.Add(t);
                context.SaveChanges();
            }
            return View();
        }

    }
}
