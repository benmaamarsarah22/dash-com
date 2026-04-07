using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Anade.Khadamat.Web.Controllers
{
    public class MoisClotureController : Controller
    {
        private readonly MoisClotureBusinessService _moisService;

        public MoisClotureController(MoisClotureBusinessService moisService)
        {
            _moisService = moisService;
        }

        [HttpGet]
        public IActionResult Index1()
        {
            // Obtenir tous les mois existants
            var moisList = _moisService.GetAll()
                            .OrderByDescending(x => x.Annee)
                            .ThenByDescending(x => x.Mois)
                            .ToList();

            return View(moisList);
        }

        [HttpPost]
        public IActionResult Ouvrir(int annee, int mois)
        {
            try
            {
                var userId = User.Identity.Name;

                _moisService.OuvrirMois(annee, mois, userId);

                TempData["Success"] = "✅ تم فتح الشهر بنجاح";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Index1));
        }


        [HttpPost]
        public IActionResult Cloturer(int annee, int mois)
        {
            try
            {
                var userId = User.Identity.Name;

                _moisService.CloturerMois(annee, mois, userId);

                TempData["Success"] = "✅ تم غلق الشهر بنجاح";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Index1));
        }

        [HttpPost]
        [Authorize(Roles = "DG")]
        public IActionResult Reouvrir(int id)
        {
            try
            {
                var mois = _moisService.GetById(id);

                if (mois == null)
                {
                    TempData["Error"] = "⛔ الشهر غير موجود";
                    return RedirectToAction(nameof(Index1));
                }

                _moisService.ReouvrirMois(mois.Annee, mois.Mois, true);

                TempData["Success"] = "✅ تم إعادة فتح الشهر بنجاح!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "⛔ " + ex.Message;
            }

            return RedirectToAction(nameof(Index1));
        }
    }
}
