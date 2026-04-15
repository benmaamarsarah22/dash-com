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
    public class ActiviteForumController : Controller
    {
        private readonly ActiviteBusinessService _activiteBusinessService;
        private readonly ActiviteForumBusinessService _ForumBusinessService;
         
        private readonly AgenceWilayaBusinessService _agenceWilayaBusinessService;
 
        private readonly UserService _userService;

        public ActiviteForumController(
            ActiviteBusinessService activiteBusinessService,
            ActiviteForumBusinessService ForumBusinessService,
            AgenceWilayaBusinessService agenceWilayaBusinessService,
          
            UserService userService)
        {
            _activiteBusinessService = activiteBusinessService;
            _ForumBusinessService = ForumBusinessService;
            _agenceWilayaBusinessService = agenceWilayaBusinessService;
             
            _userService = userService;
        }

        // GET: liste
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            
            return View();
        }
       

        // POST: create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ActiviteForumVM model)
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
                TypeActiviteId = 3,
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

            var Forum = new ActiviteForum
            {
                ActiviteId = activite.Id
            };

            var resultForum = _ForumBusinessService.Add(Forum);
            if (!resultForum.Succeeded)
            {
                TempData["Message"] = resultForum.ToBootstrapAlerts();
                return View(model);
            }
            TempData["Message"] = resultForum .ToBootstrapAlerts();
            return RedirectToAction(nameof(Index));
        }


        // GET: Details
        [HttpGet]
        public IActionResult Details(int id)
        {
            var Forum = _ForumBusinessService.GetById(
                id,
                _ForumBusinessService.GetDefaultLoadProperties()
            );

            if (Forum == null)
                return NotFound();

            return View(Forum);
        }
        // GET: edit
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var Forum = _ForumBusinessService.GetById(id, _ForumBusinessService.GetDefaultLoadProperties());
            if (Forum == null)
                return NotFound();

            var model = new ActiviteForumVM
            {
                Sujet = Forum.Activite.Sujet,
                Lieu = Forum.Activite.Lieu,
                Organisateurs = Forum.Activite.Organisateurs,
                Participants = Forum.Activite.Participants,
                NombreVisiteurs = Forum.Activite.NombreVisiteurs,
                DateActivite = Forum.Activite.DateActivite
            };

            ViewBag.ForumId = id;
            ViewBag.ActiviteId = Forum.ActiviteId;

            return View(model);
        }

        // POST: edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int activiteId, ActiviteForumVM model)
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
            ViewData["Message"] = result.ToBootstrapAlerts();
            return RedirectToAction(nameof(Index));
        }

        // GET: Delete
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var Forum = _ForumBusinessService.GetById(id, _ForumBusinessService.GetDefaultLoadProperties());
            if (Forum == null)
                return NotFound();

            return View(Forum);
        }

        // POST: Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var Forum = _ForumBusinessService.GetById(id, _ForumBusinessService.GetDefaultLoadProperties());
            if (Forum == null)
                return NotFound();

            // Supprimer la  Forum
            var resultForum = _ForumBusinessService.Delete(Forum);
            if (!resultForum.Succeeded)
            {
                TempData["Error"] = resultForum.Messages.First().Message;
                return RedirectToAction(nameof(Index));
            }

            // Supprimer l'activité liée si nécessaire
            if (Forum.Activite != null)
            {
                var resultActivite = _activiteBusinessService.Delete(Forum.Activite);
                if (!resultActivite.Succeeded)
                {
                    TempData["Error"] = resultActivite.Messages.First().Message;
                    return RedirectToAction(nameof(Index));
                }
            }

            return RedirectToAction(nameof(Index));
        }

 
        [HttpPost]
        public virtual async Task<IActionResult> ForumDataTable(DataTableAjaxModel model)
        {
            var user = await _userService.GetUserEagerLoadedAsync(User);
            var structure = await _userService.GetStructureFromUserAsync(user.Id);

            GetDataTableParameters(model, out string search, out string orderBy, out int startRowIndex, out int maxRows);

            if (!string.IsNullOrEmpty(search))
            {
                if (structure.Designation == "DG")
                {
                    var result = _ForumBusinessService.GetAllFilteredPaged(
                        x => x.Activite.Sujet.Contains(search)
                        ||x.Activite.Lieu.Contains(search)
                        ||x.Activite.Participants.Contains(search)
                        ||x.Activite.Organisateurs.Contains(search),
                        orderBy, startRowIndex, maxRows,
                        _ForumBusinessService.GetDefaultLoadProperties());

                    return Json(new JQueryDataTableRetunedData<ActiviteForum>
                    {
                        draw = model.draw,
                        recordsFiltered = result.TotalCount,
                        recordsTotal = result.TotalCount,
                        data = result.Items
                    });
                }
                else
                {
                    var result = _ForumBusinessService.GetAllFilteredPaged(
                     x => x.Activite.structureCode.StartsWith(structure.CodeStructure) 
                     && (x.Activite.Sujet.Contains(search)
                         || x.Activite.Lieu.Contains(search)
                         || x.Activite.Participants.Contains(search)
                         || x.Activite.Organisateurs.Contains(search)),
                        orderBy, startRowIndex, maxRows,
                       _ForumBusinessService.GetDefaultLoadProperties());

                    return Json(new JQueryDataTableRetunedData<ActiviteForum>
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
                    var result = _ForumBusinessService.GetAllFilteredPaged(
                        x => true,
                        orderBy, startRowIndex, maxRows,
                        _ForumBusinessService.GetDefaultLoadProperties());

                    return Json(new JQueryDataTableRetunedData<ActiviteForum>
                    {
                        draw = model.draw,
                        recordsFiltered = result.TotalCount,
                        recordsTotal = result.TotalCount,
                        data = result.Items
                    });
                }
                else
                {
                    var result = _ForumBusinessService.GetAllFilteredPaged(
                        x => x.Activite.structureCode.StartsWith(structure.CodeStructure),
                        orderBy, startRowIndex, maxRows,
                       _ForumBusinessService.GetDefaultLoadProperties());

                    return Json(new JQueryDataTableRetunedData<ActiviteForum>
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
