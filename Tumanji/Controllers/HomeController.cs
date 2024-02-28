using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using Tumanji.Data;
using Tumanji.Models;


namespace Tumanji.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _db = db;
            _logger = logger;
        }

        public IActionResult Index()
        {
            IEnumerable<NewsEntity> News = _db.News.ToList();
            return View(News);
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(UserEntity u)
        {
            IEnumerable<UserEntity> User = _db.User.ToList();
            if (Request.RouteValues.GetValueOrDefault("Login") != null)
            {
                var username = Request.RouteValues.GetValueOrDefault("UserName")?.ToString();
                var password = Request.RouteValues.GetValueOrDefault("Password")?.ToString();
                var UserLog = User.FirstOrDefault(x => x.UserName == username && x.Password == password);
                if (UserLog != null)
                {
                    HttpContext.Session.SetString("UserID", UserLog.UserID.ToString());
                    HttpContext.Session.SetString("UserName", UserLog.UserName);
                    return Redirect("Index");
                }
            }

            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserID");
            HttpContext.Session.Remove("UserName");
            HttpContext.Session.Clear();
            return Redirect("Index");
        }
        public IActionResult Contatti()
        {
            return View();
        }
        public IActionResult Menu()
        {
            IEnumerable<PaninoEntity> Panino = _db.Panino.ToList();
            return View(Panino);
        }

        public IActionResult Carrello()
        {
            IEnumerable<OrdineEntity> Ordine = _db.Ordine.ToList();
            return View(Ordine);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
