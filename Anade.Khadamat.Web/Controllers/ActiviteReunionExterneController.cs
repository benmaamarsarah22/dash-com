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
    public class ActiviteReunionExterneController : Controller
    {
        private readonly ActiviteBusinessService _activiteBusinessService;
        private readonly ActiviteReunionExterneBusinessService _externeBusinessService;
        private readonly AgenceWilayaBusinessService _agenceWilayaBusinessService;
        private readonly UserService _userService;

        public ActiviteReunionExterneController(
            ActiviteBusinessService activiteBusinessService,
            ActiviteReunionExterneBusinessService externeBusinessService,
            AgenceWilayaBusinessService agenceWilayaBusinessService,
            UserService userService)
        {
            _activiteBusinessService = activiteBusinessService;
            _externeBusinessService = externeBusinessService;
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
        public IActionResult Create(ActiviteReunionExterneVM model)
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
                TypeActiviteId = 4,
                DateActivite = model.DateActivite,
                Sujet = model.Sujet,
                Lieu = model.Lieu,
                Organisateurs = model.Organisateurs,
                Participants = model.Participants,
                AgenceWilayaId = _agenceWilayaBusinessService.GetAllFiltered(x => x.Code == structure.CodeStructure).FirstOrDefault().Id
            };

            var resultActivite = _activiteBusinessService.Add(activite);
            if (!resultActivite.Succeeded)
            {
                TempData["Message"] = resultActivite.ToBootstrapAlerts();
                return View(model);
            }

            var Externe = new ActiviteReunionExterne
            {
                ActiviteId = activite.Id
            };

            var resultExterne = _externeBusinessService.Add(Externe);
            if (!resultExterne.Succeeded)
            {
                TempData["Message"] = resultExterne.ToBootstrapAlerts();
                return View(model);
            }
            TempData["Message"] = resultExterne.ToBootstrapAlerts();
            return RedirectToAction(nameof(Index));
        }

        // GET: Details
        [HttpGet]
        public IActionResult Details(int id)
        {
            var Externe = _externeBusinessService.GetById(
                id,
                 _externeBusinessService.GetDefaultLoadProperties()
            );

            if (Externe == null)
                return NotFound();

            return View(Externe);
        }    
        // GET: edit
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var Externe = _externeBusinessService.GetById(id, _externeBusinessService.GetDefaultLoadProperties());
            if (Externe == null)
                return NotFound();

            var model = new ActiviteReunionExterneVM
            {
                Sujet = Externe.Activite.Sujet,
                Lieu = Externe.Activite.Lieu,
                Organisateurs = Externe.Activite.Organisateurs,
                Participants = Externe.Activite.Participants,
                
                DateActivite = Externe.Activite.DateActivite
            };

            ViewBag.ExterneId = id;
            ViewBag.ActiviteId = Externe.ActiviteId;

            return View(model);
        }

        // POST: edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, int activiteId, ActiviteReunionExterneVM model)
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
            var Externe = _externeBusinessService.GetById(id, _externeBusinessService.GetDefaultLoadProperties());
            if (Externe == null)
                return NotFound();

            return View(Externe);
        }

        // POST: Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var Externe = _externeBusinessService  .GetById(id, _externeBusinessService.GetDefaultLoadProperties());
            if (Externe == null)
                return NotFound();

            // Supprimer la Reunion Externe
            var resultExterne = _externeBusinessService.Delete(Externe);
            if (!resultExterne.Succeeded)
            {
                TempData["Message"] = resultExterne.ToBootstrapAlerts();
                return RedirectToAction(nameof(Index));
            }

            // Supprimer l'activité liée si nécessaire
            if (Externe.Activite != null)
            {
                var resultActivite = _activiteBusinessService.Delete(Externe.Activite);
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
        public virtual async Task<IActionResult> ExterneDataTable(DataTableAjaxModel model)
        {
            var user = await _userService.GetUserEagerLoadedAsync(User);
            var structure = await _userService.GetStructureFromUserAsync(user.Id);

            GetDataTableParameters(model, out string search, out string orderBy, out int startRowIndex, out int maxRows);

            if (!string.IsNullOrEmpty(search))
            {
                if (structure.Designation == "DG")
                {
                    var result = _externeBusinessService.GetAllFilteredPaged(
                      x => x.Activite.Sujet.Contains(search)
                        || x.Activite.Lieu.Contains(search)
                        || x.Activite.Participants.Contains(search)
                        || x.Activite.Organisateurs.Contains(search),
                        orderBy, startRowIndex, maxRows,
                      _externeBusinessService.GetDefaultLoadProperties());

                    return Json(new JQueryDataTableRetunedData<ActiviteReunionExterne>
                    {
                        draw = model.draw,
                        recordsFiltered = result.TotalCount,
                        recordsTotal = result.TotalCount,
                        data = result.Items
                    });
                }
                else
                {
                    var result = _externeBusinessService.GetAllFilteredPaged(
                      x => x.Activite.structureCode.StartsWith(structure.CodeStructure)
                     && (x.Activite.Sujet.Contains(search)
                         || x.Activite.Lieu.Contains(search)
                         || x.Activite.Participants.Contains(search)
                         || x.Activite.Organisateurs.Contains(search)),
                        orderBy, startRowIndex, maxRows,
                        _externeBusinessService.GetDefaultLoadProperties());

                    return Json(new JQueryDataTableRetunedData<ActiviteReunionExterne>
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
                    var result = _externeBusinessService.GetAllFilteredPaged(
                        x => true,
                        orderBy, startRowIndex, maxRows,
                        _externeBusinessService.GetDefaultLoadProperties());

                    return Json(new JQueryDataTableRetunedData<ActiviteReunionExterne>
                    {
                        draw = model.draw,
                        recordsFiltered = result.TotalCount,
                        recordsTotal = result.TotalCount,
                        data = result.Items
                    });
                }
                else
                {
                    var result = _externeBusinessService.GetAllFilteredPaged(
                        x => x.Activite.structureCode.StartsWith(structure.CodeStructure),
                        orderBy, startRowIndex, maxRows,
                       _externeBusinessService.GetDefaultLoadProperties());

                    return Json(new JQueryDataTableRetunedData<ActiviteReunionExterne>
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
