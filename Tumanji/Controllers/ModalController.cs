using Microsoft.AspNetCore.Mvc;
using Tumanji.Data;
using Tumanji.Models;

namespace Tumanji.Controllers
{
	public class ModalController : Controller
	{
		//private readonly ILogger<ModelController> _logger;
		private readonly ApplicationDbContext _db;
		public ModalController(ApplicationDbContext db)
		{
			_db = db;
		}
		public IActionResult Index()
		{
			return View();
		}
		[HttpPost]
		public IActionResult Create(PaninoEntity panino)
		{
			_db.Panino.Add(panino);
			_db.SaveChanges();
			return RedirectToAction("Home", "Menu");

		}
		public IActionResult Edit(Guid PaninoID)
		{
#pragma warning disable CS8600 // Conversione del valore letterale Null o di un possibile valore Null in un tipo che non ammette i valori Null.
			PaninoEntity panino = _db.Panino.Where(p => p.PaninoID == PaninoID).FirstOrDefault();
#pragma warning restore CS8600 // Conversione del valore letterale Null o di un possibile valore Null in un tipo che non ammette i valori Null.
			if (panino != null)
			{
				return PartialView("_EditPaninoPartialView", panino);
			}
			return null;
		}
		[HttpPost]
		public IActionResult Edit(PaninoEntity panino)
		{
			_db.Panino.Update(panino);
			_db.SaveChanges();
			return RedirectToAction("Home");

		}



	}
}
