using Anade.Khadamat.Business;
using Anade.Khadamat.Domain.Entity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Anade.Khadamat.Web.Controllers
{
    public class ActiviteController : Controller
    {
        private readonly ActiviteBusinessService _activiteBS;

        public ActiviteController(ActiviteBusinessService activiteBS)
        {
            _activiteBS = activiteBS;
        }

        // LISTE
        public IActionResult Index()
        {
            var activites = _activiteBS.GetAll();
            return View(activites);
        }

        // CREATE GET
        public IActionResult Create()
        {
            return View();
        }

        // CREATE POST
        [HttpPost]
        public IActionResult Create(Activite activite)
        {
            if (!ModelState.IsValid)
                return View(activite);

            var result = _activiteBS.Add(activite);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", result.Messages.First().Message);
                return View(activite);
            }

            return RedirectToAction("Index");
        }

        // EDIT GET
        public IActionResult Edit(int id)
        {
            var activite = _activiteBS.GetById(id);
            return View(activite);
        }

        // EDIT POST
        [HttpPost]
        public IActionResult Edit(Activite activite)
        {
            if (!ModelState.IsValid)
                return View(activite);

            var result = _activiteBS.Update(activite);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", result.Messages.First().Message);
                return View(activite);
            }

            return RedirectToAction("Index");
        }

        // DELETE
        public IActionResult Delete(int id)
        {
            var activite = _activiteBS.GetById(id);
            _activiteBS.Delete(activite);

            return RedirectToAction("Index");
        }
    }
}