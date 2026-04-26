using Anade.Khadamat.Business;
using Anade.Khadamat.Domain.Entity;
using Anade.Khadamat.Identity;
using Anade.Khadamat.Web.Models;
using Anade.Khadamat.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Anade.Khadamat.Web.Controllers
{
    [Authorize]
    public class ActiviteSalonController : Controller
    {
        private readonly ActiviteBusinessService _activiteBusinessService;
        private readonly ActiviteSalonBusinessService _salonBusinessService;
        private readonly AgenceWilayaBusinessService _agenceWilayaBusinessService;
        private readonly UserService _userService;

        public ActiviteSalonController(
            ActiviteBusinessService activiteBusinessService,
            ActiviteSalonBusinessService salonBusinessService,
            AgenceWilayaBusinessService agenceWilayaBusinessService,
            UserService userService)
        {
            _activiteBusinessService = activiteBusinessService;
            _salonBusinessService = salonBusinessService;
            _agenceWilayaBusinessService = agenceWilayaBusinessService;
            _userService = userService;
        }

        // GET: liste
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // GET: create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ActiviteSalonVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = _userService.GetUserEagerLoadedAsync(User).Result;
            var structure = _userService.GetStructureFromUserAsync(user.Id).Result;


            var activite = new Activite
            {
                StructureId = structure.Id,
                UserId = user.Id,
                UserName = user.UserName,
                structureCode = structure.CodeStructure,
                structureDesignation = structure.Designation,
                TypeActiviteId = 2,
                DateActivite = model.DateActivite,
                Sujet = model.Sujet,
                Lieu = model.Lieu,
                Organisateurs = model.Organisateurs,
                Participants = model.Participants,
                NombreVisiteurs = model.NombreVisiteurs,
                AgenceWilayaId = _agenceWilayaBusinessService.GetAllFiltered(x => x.Code == structure.CodeStructure).FirstOrDefault().Id
            };

            var resultActivite = _activiteBusinessService.Add(activite);
            if (!resultActivite.Succeeded)
            {
                TempData["Message"] = resultActivite.ToBootstrapAlerts();
                return View(model);
            }

            var Salon = new ActiviteSalon
            {
                ActiviteId = activite.Id
            };

            var resultSalon = _salonBusinessService.Add(Salon);
            if (!resultSalon.Succeeded)
            {
                TempData["Message"] = resultSalon.ToBootstrapAlerts();
                return View(model);
            }
            TempData["Message"] = resultSalon.ToBootstrapAlerts();
            return RedirectToAction(nameof(Index));
        }

        // GET: Details
        [HttpGet]
        public IActionResult Details(int id)
        {
            var Salon = _salonBusinessService.GetById(
                id,
                 _salonBusinessService.GetDefaultLoadProperties()
            );

            if (Salon == null)
                return NotFound();

            return View(Salon);
        }

        // GET: edit
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var Salon = _salonBusinessService.GetById(id, _salonBusinessService.GetDefaultLoadProperties());
            if (Salon == null)
                return NotFound();

            var model = new ActiviteSalonVM
            {
                Sujet = Salon.Activite.Sujet,
                Lieu = Salon.Activite.Lieu,
                Organisateurs = Salon.Activite.Organisateurs,
                Participants = Salon.Activite.Participants,
                NombreVisiteurs = Salon.Activite.NombreVisiteurs,
                DateActivite = Salon.Activite.DateActivite
            };

            ViewBag.SalonId = id;
            ViewBag.ActiviteId = Salon.ActiviteId;

            return View(model);
        }

        // POST: edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int activiteId, ActiviteSalonVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var activite = _activiteBusinessService.GetById(activiteId);
            if (activite == null)
                return NotFound();

            activite.Sujet = model.Sujet;
            activite.Lieu = model.Lieu;
            activite.Organisateurs = model.Organisateurs;
            activite.Participants = model.Participants;
            activite.NombreVisiteurs = model.NombreVisiteurs;
            activite.DateActivite = model.DateActivite;

            var result = _activiteBusinessService.Update(activite);
            if (!result.Succeeded)
            {
                TempData["Message"] = result.ToBootstrapAlerts();
                return View(model);
            }
            TempData["Message"] = result.ToBootstrapAlerts();
            return RedirectToAction(nameof(Index));
        }

        // GET: Delete
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var salon = _salonBusinessService.GetById(id, _salonBusinessService.GetDefaultLoadProperties());
            if (salon == null)
                return NotFound();

            return View(salon);
        }

        // POST: Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var salon = _salonBusinessService.GetById(id, _salonBusinessService.GetDefaultLoadProperties());
            if (salon == null)
                return NotFound();

            // Supprimer la Journée d'Info
            var resultSalon = _salonBusinessService.Delete(salon);
            if (!resultSalon.Succeeded)
            {
                TempData["Message"] = resultSalon.ToBootstrapAlerts();
                return RedirectToAction(nameof(Index));
            }

            // Supprimer l'activité liée si nécessaire
            if (salon.Activite != null)
            {
                var resultActivite = _activiteBusinessService.Delete(salon.Activite);
                if (!resultActivite.Succeeded)
                {
                    TempData["Message"] = resultActivite.ToBootstrapAlerts();
                    return RedirectToAction(nameof(Index));
                }

                TempData["Message"] = resultActivite.ToBootstrapAlerts();
            }
     
            return RedirectToAction(nameof(Index));
        }

         

        [HttpPost]
        public virtual async Task<IActionResult> SalonDataTable(DataTableAjaxModel model)
        {
            var user = await _userService.GetUserEagerLoadedAsync(User);
            var structure = await _userService.GetStructureFromUserAsync(user.Id);

            GetDataTableParameters(model, out string search, out string orderBy, out int startRowIndex, out int maxRows);

            if (!string.IsNullOrEmpty(search))
            {
                if (structure.Designation == "DG")
                {
                    var result = _salonBusinessService.GetAllFilteredPaged(
                         x => x.Activite.Sujet.Contains(search)
                        || x.Activite.Lieu.Contains(search)
                        || x.Activite.Participants.Contains(search)
                        || x.Activite.Organisateurs.Contains(search),
                        orderBy, startRowIndex, maxRows,
                        _salonBusinessService.GetDefaultLoadProperties());

                    return Json(new JQueryDataTableRetunedData<ActiviteSalon>
                    {
                        draw = model.draw,
                        recordsFiltered = result.TotalCount,
                        recordsTotal = result.TotalCount,
                        data = result.Items
                    });
                }
                else
                {
                    var result =_salonBusinessService.GetAllFilteredPaged(
                           x => x.Activite.structureCode.StartsWith(structure.CodeStructure)
                     && (x.Activite.Sujet.Contains(search)
                         || x.Activite.Lieu.Contains(search)
                         || x.Activite.Participants.Contains(search)
                         || x.Activite.Organisateurs.Contains(search)),
                        orderBy, startRowIndex, maxRows,
                        _salonBusinessService.GetDefaultLoadProperties());

                    return Json(new JQueryDataTableRetunedData<ActiviteSalon>
                    {
                        draw = model.draw,
                        recordsFiltered = result.TotalCount,
                        recordsTotal = result.TotalCount,
                        data = result.Items
                    });
                }
            }
            else
            {
                if (structure.Designation == "DG")
                {
                    var result = _salonBusinessService.GetAllFilteredPaged(
                        x => true,
                        orderBy, startRowIndex, maxRows,
                        _salonBusinessService.GetDefaultLoadProperties());

                    return Json(new JQueryDataTableRetunedData<ActiviteSalon>
                    {
                        draw = model.draw,
                        recordsFiltered = result.TotalCount,
                        recordsTotal = result.TotalCount,
                        data = result.Items
                    });
                }
                else
                {
                    var result = _salonBusinessService.GetAllFilteredPaged(
                        x => x.Activite.structureCode.StartsWith(structure.CodeStructure),
                        orderBy, startRowIndex, maxRows,
                       _salonBusinessService.GetDefaultLoadProperties());

                    return Json(new JQueryDataTableRetunedData<ActiviteSalon>
                    {
                        draw = model.draw,
                        recordsFiltered = result.TotalCount,
                        recordsTotal = result.TotalCount,
                        data = result.Items
                    });
                }
            }
        }

        #region helper 
        protected static void GetDataTableParameters(DataTableAjaxModel model, out string search, out string orderBy, out int startRowIndex, out int maxRows)
        {
            maxRows = model.length;
            startRowIndex = model.start;
            string sortBy = "", sortDir = "";

            if (model.order != null)
            {
                // in this example we just default sort on the 1st column
                sortBy = model.columns[model.order[0].column].data;
                sortDir = model.order[0].dir.ToLower();
                orderBy = sortBy + " " + sortDir;
            }
            else
            {
                orderBy = "";
            }

            if (model.search.value != null)
            {
                search = model.search.value;
            }
            else
            {
                search = "";
            }

        }

        #endregion
    }
}
