using Anade.Khadamat.Business;
using Anade.Khadamat.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Anade.Khadamat.Web.Controllers
{
    //[Authorize(Roles = "DG,Admin")]
    //[Route("[controller]/[action]")]
    public class MoisClotureController : Controller
    {
        private readonly MoisClotureBusinessService _moisService;
        private readonly UserService _userService;

        public MoisClotureController(
            MoisClotureBusinessService moisService,
            UserService userService)
        {
            _moisService = moisService;
            _userService = userService;
        }

        [Authorize(Roles = "CommunicationDG,CommunicationAG")]
        [HttpGet]
        public IActionResult Index()
        {
            var moisList = _moisService.GetAll()
                .OrderByDescending(x => x.Annee)
                .ThenByDescending(x => x.Mois)
                .ToList();

            return View(moisList);
        }

        [Authorize(Roles = "CommunicationDG")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Ouvrir(int annee, int mois)
        {
            var userId = _userService.GetUserEagerLoadedAsync(User).Result?.UserName ?? User.Identity.Name;
            var result = _moisService.OuvrirMois(annee, mois, userId);

            TempData["Message"] = result.ToBootstrapAlerts();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "CommunicationDG")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Cloturer(int annee, int mois)
        {
            var userId = _userService.GetUserEagerLoadedAsync(User).Result?.UserName ?? User.Identity.Name;
            var result = _moisService.CloturerMois(annee, mois, userId);

            TempData["Message"] = result.ToBootstrapAlerts();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "CommunicationDG")]
        [HttpPost]
        [ValidateAntiForgeryToken]
      
        public IActionResult Reouvrir(int annee, int mois)
        {
            var userId = _userService.GetUserEagerLoadedAsync(User).Result?.UserName ?? User.Identity.Name;
            var result = _moisService.ReouvrirMois(annee, mois, userId);

            TempData["Message"] = result.ToBootstrapAlerts();
            return RedirectToAction(nameof(Index));
        }
    }
}

