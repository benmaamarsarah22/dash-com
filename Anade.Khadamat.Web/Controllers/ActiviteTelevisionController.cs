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
    public class ActiviteTelevisionController : Controller
    {
        private readonly ActiviteBusinessService _activiteBusinessService;
        private readonly ActiviteTelevisionBusinessService _TvBusinessService;
        private readonly AgenceWilayaBusinessService _agenceWilayaBusinessService;
        private readonly UserService _userService;

        public ActiviteTelevisionController(
            ActiviteBusinessService activiteBusinessService,
            ActiviteTelevisionBusinessService TvBusinessService,
             AgenceWilayaBusinessService agenceWilayaBusinessService,
            UserService userService)
        {
            _activiteBusinessService = activiteBusinessService;
            _agenceWilayaBusinessService = agenceWilayaBusinessService;
            _TvBusinessService = TvBusinessService;
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
        public IActionResult Create(ActiviteTelevisionVM model)
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
                TypeActiviteId = 6,
                DateActivite = model.DateActivite,
                Sujet = model.Sujet,
                AgenceWilayaId = _agenceWilayaBusinessService.GetAllFiltered(x => x.Code == structure.CodeStructure).FirstOrDefault().Id

            };

            var resultActivite = _activiteBusinessService.Add(activite);
            if (!resultActivite.Succeeded)
            {
                ModelState.AddModelError("", resultActivite.Messages.First().Message);
                return View(model);
            }

            var Tv = new ActiviteTelevision
            {
                Activite = activite,
                ChaineTV = model.ChaineTV,

            };
            var resultTv = _TvBusinessService.Add(Tv);
            if (!resultTv.Succeeded)
            {
                ModelState.AddModelError("", resultTv.Messages.First().Message);
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Details
        [HttpGet]
        public IActionResult Details(int id)
        {
            var Tv = _TvBusinessService.GetById(
                id,
                _TvBusinessService.GetDefaultLoadProperties()
            );

            if (Tv == null)
                return NotFound();

            return View(Tv);
        }

        // GET: edit
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var Tv = _TvBusinessService.GetById(id, _TvBusinessService.GetDefaultLoadProperties());
            if (Tv == null)
                return NotFound();

            var model = new ActiviteTelevisionVM
            {
                Sujet = Tv.Activite.Sujet,
                DateActivite = Tv.Activite.DateActivite,
                ChaineTV = Tv.ChaineTV
            };

            ViewBag.TvId = id;
            ViewBag.ActiviteId = Tv.ActiviteId;

            return View(model);
        }

        // POST: edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, int activiteId, ActiviteTelevisionVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var activite = _activiteBusinessService.GetById(activiteId);
            var Tv = _TvBusinessService.GetById(id);

            if (activite == null || Tv == null)
                return NotFound();


            activite.Sujet = model.Sujet;
            activite.DateActivite = model.DateActivite;

            Tv.ChaineTV = model.ChaineTV;
          

            var result = _activiteBusinessService.Update(activite);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", result.Messages.First().Message);
                return View(model);
            }
            var resultTv = _TvBusinessService.Update(Tv);
            if (!resultTv.Succeeded)
            {
                ModelState.AddModelError("", resultTv.Messages.First().Message);
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Delete
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var Tv = _TvBusinessService.GetById(id, _TvBusinessService.GetDefaultLoadProperties());
            if (Tv == null)
                return NotFound();

            return View(Tv);
        }

        // POST: Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var Tv = _TvBusinessService.GetById(id, _TvBusinessService.GetDefaultLoadProperties());
            if (Tv == null)
                return NotFound();

            /// Supprimer la Radio d'abord
            var resultTv = _TvBusinessService.Delete(Tv);
            if (!resultTv.Succeeded)
            {
                TempData["Error"] = resultTv.Messages.First().Message;
                return RedirectToAction(nameof(Index));
            }

            // Supprimer l'activité liée si nécessaire
            if (Tv.Activite != null)
            {
                var resultActivite = _activiteBusinessService.Delete(Tv.Activite);
                if (!resultActivite.Succeeded)
                {
                    TempData["Error"] = resultActivite.Messages.First().Message;
                    return RedirectToAction(nameof(Index));
                }
            }
            return RedirectToAction(nameof(Index));
        }


 
        [HttpPost]
        public virtual async Task<IActionResult> TvDataTable(DataTableAjaxModel model)
        {
            var user = await _userService.GetUserEagerLoadedAsync(User);
            var structure = await _userService.GetStructureFromUserAsync(user.Id);

            GetDataTableParameters(model, out string search, out string orderBy, out int startRowIndex, out int maxRows);

            if (!string.IsNullOrEmpty(search))
            {
                if (structure.Designation == "DG")
                {
                    var result = _TvBusinessService.GetAllFilteredPaged(
                        x => x.Activite.Sujet.Contains(search),
                        orderBy, startRowIndex, maxRows,
                       _TvBusinessService.GetDefaultLoadProperties());

                    return Json(new JQueryDataTableRetunedData<ActiviteTelevision>
                    {
                        draw = model.draw,
                        recordsFiltered = result.TotalCount,
                        recordsTotal = result.TotalCount,
                        data = result.Items
                    });
                }
                else
                {
                    var result = _TvBusinessService.GetAllFilteredPaged(
                        x => x.Activite.structureCode.StartsWith(structure.CodeStructure)
                             && x.Activite.Sujet.Contains(search),
                        orderBy, startRowIndex, maxRows,
                       _TvBusinessService.GetDefaultLoadProperties());

                    return Json(new JQueryDataTableRetunedData<ActiviteTelevision>
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
                    var result = _TvBusinessService.GetAllFilteredPaged(
                        x => true,
                        orderBy, startRowIndex, maxRows,
                       _TvBusinessService.GetDefaultLoadProperties());

                    return Json(new JQueryDataTableRetunedData<ActiviteTelevision>
                    {
                        draw = model.draw,
                        recordsFiltered = result.TotalCount,
                        recordsTotal = result.TotalCount,
                        data = result.Items
                    });
                }
                else
                {
                    var result = _TvBusinessService.GetAllFilteredPaged(
                        x => x.Activite.structureCode.StartsWith(structure.CodeStructure),
                        orderBy, startRowIndex, maxRows,
                       _TvBusinessService.GetDefaultLoadProperties());

                    return Json(new JQueryDataTableRetunedData<ActiviteTelevision>
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
