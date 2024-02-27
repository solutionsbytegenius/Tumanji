using Microsoft.AspNetCore.Mvc;
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
            IEnumerable<UserEntity> User = _db.User.ToList();
            return View(User);
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
