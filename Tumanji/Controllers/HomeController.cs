using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using Tumanji.Data;
using System.Runtime.Serialization.Formatters.Binary;
using Tumanji.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System.Text.Json;


namespace Tumanji.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
        private CartItemCollection CartCollection = new CartItemCollection();
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


        [HttpPost]
        public IActionResult AddCart(CartItem item)
        {
            if (!String.IsNullOrEmpty(HttpContext?.Session.GetString("Collection")))
            {
#pragma warning disable CS8602 // Dereferenziamento di un possibile riferimento Null.
                var jsonStringFromSession = HttpContext.Session.GetString("Collection");
#pragma warning restore CS8602 // Dereferenziamento di un possibile riferimento Null.
#pragma warning disable CS8601 // Possibile assegnazione di riferimento Null.
#pragma warning disable CS8604 // Possibile argomento di riferimento Null.
                CartCollection = JsonConvert.DeserializeObject<CartItemCollection>(jsonStringFromSession);
#pragma warning restore CS8604 // Possibile argomento di riferimento Null.
#pragma warning restore CS8601 // Possibile assegnazione di riferimento Null.
            }
            item.ID = Guid.NewGuid();
            if (String.IsNullOrEmpty(item.Note)) item.Note = "";
            if (String.IsNullOrEmpty(item.Bevanda)) item.Bevanda = "";
            if (item.Plus) item.Prezzo += 2.50;
            if (String.IsNullOrEmpty(item.Panino) || item.Panino.ToLower() == "undefined" ) 
            {
                item.Panino = _db.Panino.FirstOrDefault(x => x.PaninoID == item.PaninoID).Nome;
            }

#pragma warning disable CS8602 // Dereferenziamento di un possibile riferimento Null.
            CartCollection.Add(item);
#pragma warning restore CS8602 // Dereferenziamento di un possibile riferimento Null.
            var jsonString = JsonConvert.SerializeObject(CartCollection);
            HttpContext.Session.SetString("Collection", jsonString.ToString());
            return RedirectToAction("Menu");
        }
        
        [HttpPost]
        public IActionResult DeleteFromCart(Guid ID)
        {
            if (!String.IsNullOrEmpty(HttpContext?.Session.GetString("Collection")))
            {
#pragma warning disable CS8602 // Dereferenziamento di un possibile riferimento Null.
                var jsonStringFromSession = HttpContext.Session.GetString("Collection");
#pragma warning restore CS8602 // Dereferenziamento di un possibile riferimento Null.
#pragma warning disable CS8601 // Possibile assegnazione di riferimento Null.
#pragma warning disable CS8604 // Possibile argomento di riferimento Null.
                CartCollection = JsonConvert.DeserializeObject<CartItemCollection>(jsonStringFromSession);
#pragma warning restore CS8604 // Possibile argomento di riferimento Null.
#pragma warning restore CS8601 // Possibile assegnazione di riferimento Null.
            }
            CartItem item = CartCollection.FirstOrDefault(x => x.ID == ID);

#pragma warning disable CS8602 // Dereferenziamento di un possibile riferimento Null.
            CartCollection.Remove(item);
#pragma warning restore CS8602 // Dereferenziamento di un possibile riferimento Null.
            var jsonString = JsonConvert.SerializeObject(CartCollection);
            if (jsonString == "[]") jsonString = "";
            HttpContext.Session.SetString("Collection", jsonString.ToString());
            return RedirectToAction("Carrello",CartCollection);
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
                return RedirectToAction("Index");
            }

            return RedirectToAction("Login");
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
            IEnumerable<OrarioEntity> orario = _db.Orario.OrderBy(x => x.NumeroGiorno).ToList();
            return View(orario);
        }
        [HttpGet]
        public IActionResult Menu()
        {
            IEnumerable<PaninoEntity> Panino = _db.Panino.Where(x => x.InMenu == true).ToList();
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

        public IActionResult ShowModal(Guid PaninoID)
        {
#pragma warning disable CS8600 // Conversione del valore letterale Null o di un possibile valore Null in un tipo che non ammette i valori Null.
            PaninoEntity Panino = _db.Panino.FirstOrDefault(x => x.PaninoID == PaninoID);
#pragma warning restore CS8600 // Conversione del valore letterale Null o di un possibile valore Null in un tipo che non ammette i valori Null.
            return PartialView("ModalDetail", Panino);
        }

        public IActionResult Carrello()
        {
            if (!String.IsNullOrEmpty(HttpContext?.Session.GetString("Collection")))
            {
#pragma warning disable CS8602 // Dereferenziamento di un possibile riferimento Null.
                var jsonStringFromSession = HttpContext.Session.GetString("Collection");
#pragma warning restore CS8602 // Dereferenziamento di un possibile riferimento Null.
#pragma warning disable CS8601 // Possibile assegnazione di riferimento Null.
#pragma warning disable CS8604 // Possibile argomento di riferimento Null.
                CartCollection = JsonConvert.DeserializeObject<CartItemCollection>(jsonStringFromSession);
#pragma warning restore CS8604 // Possibile argomento di riferimento Null.
#pragma warning restore CS8601 // Possibile assegnazione di riferimento Null.
            }
            if (CartCollection.Any())
            {
                return View("Carrello",CartCollection);
            }
            return View("Carrello");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
