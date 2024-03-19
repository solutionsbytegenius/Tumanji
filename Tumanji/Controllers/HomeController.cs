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
			item.ID=Guid.NewGuid();
            if (String.IsNullOrEmpty(item.Note)) item.Note = "";
            if (String.IsNullOrEmpty(item.Bevanda)) item.Bevanda = "";

#pragma warning disable CS8602 // Dereferenziamento di un possibile riferimento Null.
			CartCollection.Add(item);
#pragma warning restore CS8602 // Dereferenziamento di un possibile riferimento Null.
			var jsonString = JsonConvert.SerializeObject(CartCollection);
			HttpContext.Session.SetString("Collection", jsonString.ToString());
			return RedirectToAction("Menu");
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
			PaninoEntity Panino = _db.Panino.FirstOrDefault(x=>x.PaninoID==PaninoID);
#pragma warning restore CS8600 // Conversione del valore letterale Null o di un possibile valore Null in un tipo che non ammette i valori Null.
			return PartialView("ModalDetail", Panino);
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


        #region modals
        private string[] GetNomiColonneDettagli()
        {
            List<string> returnDati = new List<string>();
            returnDati.Add("Nome");
            returnDati.Add("Descrizione");
            returnDati.Add("Prezzo");
            returnDati.Add("Menu +");
            returnDati.Add("PathImage");
            return returnDati.ToArray();
        }
        private string[] GetTipoColonneAnagrafica()
        {
            List<string> returnDati = new List<string>();
            returnDati.Add("Testo");
            returnDati.Add("Testo");
            returnDati.Add("Testo");
            returnDati.Add("Checkbox");
            returnDati.Add("Testo");
            return returnDati.ToArray();
        }

        [HttpPost]
        public ActionResult ModalDetail(ModalPaninoPipe Panino)
        {
            try
            {
                Guid temp = new Guid(Panino.PaninoID);
#pragma warning disable CS8600 // Conversione del valore letterale Null o di un possibile valore Null in un tipo che non ammette i valori Null.
                PaninoEntity panino = _db.Panino.FirstOrDefault(x => x.PaninoID == temp);
#pragma warning restore CS8600 // Conversione del valore letterale Null o di un possibile valore Null in un tipo che non ammette i valori Null.
                    ModalDetail Modale = new ModalDetail
                    {
                        Nome = panino.Nome,
                        Descrizione = panino.Descrizione,
                        Prezzo = panino.Prezzo,
                        PathImage = panino.PathImage
                    };
                    return PartialView(Modale);
            }
            catch (Exception ex)
            {

#pragma warning disable CA2200 // Eseguire il rethrow per conservare i dettagli dello stack
                throw ex;
#pragma warning restore CA2200 // Eseguire il rethrow per conservare i dettagli dello stack
            }
        }

        [HttpPost]
        public ActionResult Add(Microsoft.AspNetCore.Http.IFormCollection form)
        {
          /*  for (int ContaElementi = 0; ContaElementi < form["dati"].Count; ContaElementi++)
            {
                string valore = form["dati"][ContaElementi].ToString();
            }*/
            return RedirectToAction("Menu");
        }
        #endregion
    }
}
