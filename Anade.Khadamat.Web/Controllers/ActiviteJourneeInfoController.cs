using Anade.Khadamat.Business;
using Anade.Khadamat.Domain.Entity;
using Anade.Khadamat.Identity;
using Anade.Khadamat.Web.Models;
using Anade.Khadamat.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Anade.Khadamat.Web.Controllers
{
    public class ActiviteJourneeInfoController : Controller
    {
        private readonly ActiviteBusinessService _activiteBusinessService;
        private readonly ActiviteJourneeInfoBusinessService _journeeBusinessService;
        private readonly UserService _userService;
        private readonly AgenceWilayaBusinessService _agenceWilayaBusinessService;

        public ActiviteJourneeInfoController(
            ActiviteBusinessService activiteBusinessService,
            ActiviteJourneeInfoBusinessService journeeBusinessService,
            AgenceWilayaBusinessService agenceWilayaBusinessService,
            UserService userService)
        {
            _activiteBusinessService = activiteBusinessService;
            _journeeBusinessService = journeeBusinessService;
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
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ActiviteJourneeInfoVM model)
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
                AgenceWilayaId= _agenceWilayaBusinessService.GetAllFiltered(x => x.Code == structure.CodeStructure).FirstOrDefault().Id
            };

            var resultActivite = _activiteBusinessService.Add(activite);
            if (!resultActivite.Succeeded)
            {
                TempData["Message"] = resultActivite.ToBootstrapAlerts();
                return View(model);
            }

            var journee = new ActiviteJourneeInfo
            {
                ActiviteId = activite.Id
            };

            var resultJournee = _journeeBusinessService.Add(journee);
            if (!resultJournee.Succeeded)
            {
                TempData["Message"] = resultJournee.ToBootstrapAlerts();
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Details
        [HttpGet]
        public IActionResult Details(int id)
        {
            var journee = _journeeBusinessService.GetById(
                id,
                _journeeBusinessService.GetDefaultLoadProperties()
            );

            if (journee == null)
                return NotFound();

            return View(journee);
        }
        // GET: edit
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var journee = _journeeBusinessService.GetById(id, _journeeBusinessService.GetDefaultLoadProperties());
            if (journee == null)
                return NotFound();

            var model = new ActiviteJourneeInfoVM
            {
                Sujet = journee.Activite.Sujet,
                Lieu = journee.Activite.Lieu,
                Organisateurs = journee.Activite.Organisateurs,
                Participants = journee.Activite.Participants,
                NombreVisiteurs = journee.Activite.NombreVisiteurs,
                DateActivite = journee.Activite.DateActivite
            };

            ViewBag.JourneeId = id;
            ViewBag.ActiviteId = journee.ActiviteId;

            return View(model);
        }

        // POST: edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int activiteId, ActiviteJourneeInfoVM model)
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
                ModelState.AddModelError("", result.Messages.First().Message);
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Delete
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var journee = _journeeBusinessService.GetById(id, _journeeBusinessService.GetDefaultLoadProperties());
            if (journee == null)
                return NotFound();

            return View(journee);
        }

        // POST: Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var journee = _journeeBusinessService.GetById(id,_journeeBusinessService.GetDefaultLoadProperties());
            if (journee == null)
                return NotFound();

          
            var resultJournee = _journeeBusinessService.Delete(journee);
            if (!resultJournee.Succeeded)
            {
                TempData["Error"] = resultJournee.Messages.First().Message;
                return RedirectToAction(nameof(Index));
            }

            if (journee.Activite != null)
            {
                var resultActivite = _activiteBusinessService.Delete(journee.Activite);
                if (!resultActivite.Succeeded)
                {
                    TempData["Error"] = resultActivite.Messages.First().Message;
                    return RedirectToAction(nameof(Index));
                }
            }

            return RedirectToAction(nameof(Index));
        }


        

        [HttpPost]
        public virtual async Task<IActionResult> journeeDataTable(DataTableAjaxModel model)
        {
            var user = await _userService.GetUserEagerLoadedAsync(User);
            var structure = await _userService.GetStructureFromUserAsync(user.Id);

            GetDataTableParameters(model, out string search, out string orderBy, out int startRowIndex, out int maxRows);

            if (!string.IsNullOrEmpty(search))
            {
                if (structure.Designation == "DG")
                {
                    var result = _journeeBusinessService.GetAllFilteredPaged(
                        x => x.Activite.Sujet.Contains(search),
                        orderBy, startRowIndex, maxRows,
                        _journeeBusinessService.GetDefaultLoadProperties());

                    return Json(new JQueryDataTableRetunedData<ActiviteJourneeInfo>
                    {
                        draw = model.draw,
                        recordsFiltered = result.TotalCount,
                        recordsTotal = result.TotalCount,
                        data = result.Items
                    });
                }
                else
                {
                    var result = _journeeBusinessService.GetAllFilteredPaged(
                        x => x.Activite.structureCode.StartsWith(structure.CodeStructure)
                             && x.Activite.Sujet.Contains(search),
                        orderBy, startRowIndex, maxRows,
                        _journeeBusinessService.GetDefaultLoadProperties());

                    return Json(new JQueryDataTableRetunedData<ActiviteJourneeInfo>
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
                    var result = _journeeBusinessService.GetAllFilteredPaged(
                        x => true,
                        orderBy, startRowIndex, maxRows,
                        _journeeBusinessService.GetDefaultLoadProperties());

                    return Json(new JQueryDataTableRetunedData<ActiviteJourneeInfo>
                    {
                        draw = model.draw,
                        recordsFiltered = result.TotalCount,
                        recordsTotal = result.TotalCount,
                        data = result.Items
                    });
                }
                else
                {
                    var result = _journeeBusinessService.GetAllFilteredPaged(
                        x => x.Activite.structureCode.StartsWith(structure.CodeStructure),
                        orderBy, startRowIndex, maxRows,
                        _journeeBusinessService.GetDefaultLoadProperties());

                    return Json(new JQueryDataTableRetunedData<ActiviteJourneeInfo>
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
  