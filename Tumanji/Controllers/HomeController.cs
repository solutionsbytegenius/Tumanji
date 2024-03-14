using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using Tumanji.Data;
using Tumanji.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;


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
		/*
				[HttpGet]
				public IActionResult Create()
				{
					Panino panino = new Panino();
					return PartialView("_EditPaninoPartialView", panino);
				}

				[HttpPost]
				public IActionResult Create(PaninoEntity panino)
				{
					_db.Panino.Add(panino);
					_db.SaveChanges();
					return RedirectToAction("Home", "EditMenu");

				}*/

		// GET: Panini/AggiungiPanino
		[HttpGet]
		public ActionResult AggiungiPanino()
		{
			return PartialView("_AggiungiPanino");
		}

		// POST: Panini/AggiungiPanino
		[HttpPost]
		public ActionResult AggiungiPanino(PaninoEntity panino)
		{
			if (ModelState.IsValid)
			{
				_db.Panino.Add(panino);
				_db.SaveChanges();
				return RedirectToAction("Index");
			}
			return PartialView("_AggiungiPanino", panino);
		}

	[HttpGet]
        public IActionResult Login()
        {
            if (String.IsNullOrEmpty(HttpContext?.Session.GetString("UserID")))
            {
                return View();
            }
            else
            {
                return View("Index");
            }
        }

        [HttpPost]
        public IActionResult Verify(AccountPipe A)
        {
            IEnumerable<UserEntity> User = _db.User.ToList();

            var UserLog = User.FirstOrDefault(x => x.UserName == A.Username && x.Password == A.Password);
            if (UserLog != null)
            {
                HttpContext.Session.SetString("UserID", UserLog.UserID.ToString());
                HttpContext.Session.SetString("UserName", UserLog.UserName);
                return View("Index");
            }

            return View("Login");
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
            IEnumerable<OrarioEntity> orario = _db.Orario.OrderBy(x=>x.NumeroGiorno).ToList();
            return View(orario);
        }
        public IActionResult Menu()
        {
            IEnumerable<PaninoEntity> Panino = _db.Panino.Where(x=>x.InMenu==true).ToList();
            return View(Panino);
        }

        public IActionResult EditMenu()
        {
            IEnumerable<PaninoEntity> Panino = _db.Panino.ToList();
            
            if (!String.IsNullOrEmpty(HttpContext?.Session.GetString("UserID")))
            {
                return View(Panino);
            }
            else
            {
                return View("Index");
            }
        }

        public IActionResult News()
        {
            IEnumerable<NewsEntity> News = _db.News.ToList();

            if (!String.IsNullOrEmpty(HttpContext?.Session.GetString("UserID")))
            {
                return View(News);
            }
            else
            {
                return View("Index");
            }
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
