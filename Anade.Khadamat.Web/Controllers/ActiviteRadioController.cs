using Anade.Khadamat.Business;
using Anade.Khadamat.Domain.Entity;
using Anade.Khadamat.Identity;
using Anade.Khadamat.Web.Models;
using Anade.Khadamat.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Anade.Khadamat.Web.Controllers
{
    public class ActiviteRadioController : Controller
    {
        private readonly ActiviteBusinessService _activiteBusinessService;
        private readonly ActiviteRadioBusinessService _RadioBusinessService;
        private readonly AgenceWilayaBusinessService _agenceWilayaBusinessService;
        private readonly UserService _userService;

        public ActiviteRadioController(
            ActiviteBusinessService activiteBusinessService,
            ActiviteRadioBusinessService RadioBusinessService,
             AgenceWilayaBusinessService agenceWilayaBusinessService,
            UserService userService)
        {
            _activiteBusinessService = activiteBusinessService;
            _RadioBusinessService = RadioBusinessService;
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
        public IActionResult Create(ActiviteRadioVM model)
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
                TypeActiviteId = 5,
                DateActivite = model.DateActivite,
                Sujet = model.Sujet,
                AgenceWilayaId = _agenceWilayaBusinessService.GetAllFiltered(x => x.Code == structure.CodeStructure).FirstOrDefault().Id

            };

            var resultActivite = _activiteBusinessService.Add(activite);
            if (!resultActivite.Succeeded)
            {
                TempData["Message"] = resultActivite.ToBootstrapAlerts();
                return View(model);
            }

            var Radio = new ActiviteRadio
            {
                Activite = activite,
                StationRadio = model.StationRadio,
                Intervenants = model.Intervenants
            };
            var resultRadio = _RadioBusinessService.Add(Radio);
            if (!resultRadio.Succeeded)
            {
                TempData["Message"] = resultRadio.ToBootstrapAlerts();
                return View(model);
            }
            TempData["Message"] = resultRadio.ToBootstrapAlerts();
            return RedirectToAction(nameof(Index));
        }

        // GET: Details
        [HttpGet]
        public IActionResult Details(int id)
        {
            var Radio = _RadioBusinessService.GetById(
                id,
                _RadioBusinessService.GetDefaultLoadProperties()
            );

            if (Radio == null)
                return NotFound();

            return View (Radio);
        }

        // GET: edit
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var Radio = _RadioBusinessService.GetById(id, _RadioBusinessService.GetDefaultLoadProperties());
            if (Radio == null)
                return NotFound();

            var model = new ActiviteRadioVM
            {
                Sujet = Radio.Activite.Sujet,
                DateActivite = Radio.Activite.DateActivite,
                Intervenants = Radio.Intervenants,
                StationRadio = Radio.StationRadio
            };

            ViewBag.RadioId = id;
            ViewBag.ActiviteId = Radio.ActiviteId;

            return View(model);
        }

        // POST: edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, int activiteId, ActiviteRadioVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var activite = _activiteBusinessService.GetById(activiteId);
            var radio = _RadioBusinessService.GetById(id);

            if (activite == null || radio == null)
                return NotFound();

          
            activite.Sujet = model.Sujet;
            activite.DateActivite = model.DateActivite;

            radio.StationRadio = model.StationRadio;
            radio.Intervenants = model.Intervenants;

            var result = _activiteBusinessService.Update(activite);
           
            if (!result.Succeeded)
            {

                TempData["Message"] = result.ToBootstrapAlerts();
                return View(model);
            }
            var resultRadio = _RadioBusinessService.Update(radio);
            if (!resultRadio.Succeeded)
            {
                TempData["Message"] = resultRadio.ToBootstrapAlerts();
                return View(model);
            }

            TempData["Message"] = result.ToBootstrapAlerts();
            return RedirectToAction(nameof(Index));
        }

        // GET: Delete
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var Radio = _RadioBusinessService.GetById(id, _RadioBusinessService.GetDefaultLoadProperties());
            if (Radio == null)
                return NotFound();

            return View(Radio);
        }

        // POST: Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var Radio = _RadioBusinessService.GetById(id, _RadioBusinessService.GetDefaultLoadProperties());
            if (Radio == null)
                return NotFound();

            /// Supprimer la Radio d'abord
            var resultRadio = _RadioBusinessService.Delete(Radio);
            if (!resultRadio.Succeeded)
            {

                TempData["Message"] = resultRadio.ToBootstrapAlerts();
                return RedirectToAction(nameof(Index));
            }

            // Supprimer l'activité liée si nécessaire
            if (Radio.Activite != null)
            {
                var resultActivite = _activiteBusinessService.Delete(Radio.Activite);
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
        public virtual async Task<IActionResult> RadioDataTable(DataTableAjaxModel model)
        {
            var user = await _userService.GetUserEagerLoadedAsync(User);
            var structure = await _userService.GetStructureFromUserAsync(user.Id);

            GetDataTableParameters(model, out string search, out string orderBy, out int startRowIndex, out int maxRows);

            if (!string.IsNullOrEmpty(search))
            {
                if (structure.Designation == "DG")
                {
                    var result = _RadioBusinessService.GetAllFilteredPaged(
                           x => x.Activite.Sujet.Contains(search)
                        || x.StationRadio.Contains(search)
                        || x.Intervenants.Contains(search),
                        orderBy, startRowIndex, maxRows,
                       _RadioBusinessService.GetDefaultLoadProperties());

                    return Json(new JQueryDataTableRetunedData<ActiviteRadio>
                    {
                        draw = model.draw,
                        recordsFiltered = result.TotalCount,
                        recordsTotal = result.TotalCount,
                        data = result.Items
                    });
                }
                else
                {
                    var result = _RadioBusinessService.GetAllFilteredPaged(
                        x => x.Activite.structureCode.StartsWith(structure.CodeStructure)
                             && (x.Activite.Sujet.Contains(search)
                                 || x.StationRadio.Contains(search)
                                  ||x.Intervenants.Contains(search)),
                        orderBy, startRowIndex, maxRows,
                       _RadioBusinessService.GetDefaultLoadProperties());

                    return Json(new JQueryDataTableRetunedData<ActiviteRadio>
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
                    var result = _RadioBusinessService.GetAllFilteredPaged(
                        x => true,
                        orderBy, startRowIndex, maxRows,
                        _RadioBusinessService.GetDefaultLoadProperties());

                    return Json(new JQueryDataTableRetunedData<ActiviteRadio>
                    {
                        draw = model.draw,
                        recordsFiltered = result.TotalCount,
                        recordsTotal = result.TotalCount,
                        data = result.Items
                    });
                }
                else
                {
                    var result = _RadioBusinessService.GetAllFilteredPaged(
                        x => x.Activite.structureCode.StartsWith(structure.CodeStructure),
                        orderBy, startRowIndex, maxRows,
                       _RadioBusinessService.GetDefaultLoadProperties());

                    return Json(new JQueryDataTableRetunedData<ActiviteRadio>
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
