using Masters3.Data;
using Masters3.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Drawing;

namespace Masters3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<User> signInManager;
        private readonly ApplicationDbContext context;

        public HomeController(ILogger<HomeController> logger, SignInManager<User> signInManager,ApplicationDbContext context)
        {
            _logger = logger;
            this.signInManager = signInManager; 
            this.context = context;
        }

        public IActionResult Index()
        {
            ViewBag.RoleID = HttpContext.Session.GetString("Role");

            var category = context.categories.ToList();
            var testimonial = context.testimonials.Where(obj=>obj.Status== "1").Include(obj => obj.User).ToList();

            var tuple = new Tuple<IEnumerable<Category>,IEnumerable<Testimonial>>(category,testimonial);

            return View(tuple);
        }

        public IActionResult category()
        {

            return View(context.categories.ToList());
        }
        [HttpPost]
        public IActionResult category(string name)
        {
            if(name != null)
            return View(context.categories.Where(obj=>obj.CategoryName == name).ToList());
            else
                return View(context.categories.ToList());
        }
        [HttpGet]
        public IActionResult product(int id)
        {
            if(id != 0)
            HttpContext.Session.SetInt32("catID",id);
            
            return View(context.products.Where(obj=> obj.CategoryId == id).ToList());
        }
        [HttpPost]
        public IActionResult product(string name)
        {
            int? ID = HttpContext.Session.GetInt32("catID");
            if (name != null)
                return View(context.products.Where(obj => obj.Name == name && obj.CategoryId == ID).ToList());
            else
                return View(context.products.Where(obj=>obj.CategoryId == ID).ToList());
        }
        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
         

        public async Task<IActionResult> SingleProduct(int id)
        {
            if(id != 0)
            return View(context.products.Where(obj=>obj.Id== id).FirstOrDefault());
            else
                return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public IActionResult About()
        {
            return View();
        }
		public IActionResult Contact()
		{
			return View();
		}


	}
}