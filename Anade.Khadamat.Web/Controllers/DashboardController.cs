using Anade.Khadamat.Business;
using Anade.Khadamat.Data;
using Anade.Khadamat.Domain.Entity;
using Anade.Khadamat.Identity;
using Anade.Khadamat.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Anade.Khadamat.Web.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ActiviteJourneeInfoBusinessService _journeeBusinessService;
        private readonly ActiviteForumBusinessService _ForumBusinessService;
        private readonly ActivitePresseBusinessService _PresseBusinessService;
        private readonly ActiviteRadioBusinessService _RadioBusinessService;
        private readonly ActiviteReunionExterneBusinessService _externeBusinessService;
        private readonly ActiviteSalonBusinessService _salonBusinessService;
        private readonly ActiviteTelevisionBusinessService _TvBusinessService;
        private readonly AgenceWilayaBusinessService _agenceWilayaBusiness;
        private readonly UserService _userService;

        public DashboardController(
           ActiviteJourneeInfoBusinessService journeeBusinessService,
           ActiviteForumBusinessService ForumBusinessService,
           ActivitePresseBusinessService PresseBusinessService,
           ActiviteRadioBusinessService RadioBusinessService,
           ActiviteReunionExterneBusinessService externeBusinessService,
           AgenceWilayaBusinessService agenceWilayaBusiness,
           ActiviteSalonBusinessService salonBusinessService,
           ActiviteTelevisionBusinessService TvBusinessService,
           UserService userService)
        {
            _journeeBusinessService = journeeBusinessService;
            _ForumBusinessService = ForumBusinessService;
            _PresseBusinessService = PresseBusinessService;
            _RadioBusinessService = RadioBusinessService;
            _externeBusinessService = externeBusinessService;
            _salonBusinessService = salonBusinessService;
            _TvBusinessService = TvBusinessService;
            _agenceWilayaBusiness = agenceWilayaBusiness;
            _userService = userService;
        }

        public async Task<IActionResult> Index(DateTime? dateDebut, DateTime? dateFin)
        {
            var minDate = new DateTime(2026, 1, 1);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userStructure = userId != null
                ? await _userService.GetStructureFromUserAsync(userId)
                : null;

            bool isDg = userStructure?.Niveau?.Code == "1";

            var agences = _agenceWilayaBusiness
                .GetAll(_agenceWilayaBusiness.GetDefaultLoadProperties())
                .ToList();

            if (!isDg && userStructure != null)
            {
                agences = agences
                    .Where(a => a.Code == userStructure.CodeStructure)
                    .ToList();
            }

            var journees = _journeeBusinessService.GetAll(_journeeBusinessService.GetDefaultLoadProperties());
            var forums = _ForumBusinessService.GetAll(_ForumBusinessService.GetDefaultLoadProperties());
            var presses = _PresseBusinessService.GetAll(_PresseBusinessService.GetDefaultLoadProperties());
            var radios = _RadioBusinessService.GetAll(_RadioBusinessService.GetDefaultLoadProperties());
            var reunions = _externeBusinessService.GetAll(_externeBusinessService.GetDefaultLoadProperties());
            var salons = _salonBusinessService.GetAll(_salonBusinessService.GetDefaultLoadProperties());
            var tvs = _TvBusinessService.GetAll(_TvBusinessService.GetDefaultLoadProperties());

            // ── Filtre par plage de dates ──
            journees = journees.Where(x => x.Activite.DateActivite >= minDate
                && (!dateDebut.HasValue || x.Activite.DateActivite.Date >= dateDebut.Value.Date)
                && (!dateFin.HasValue || x.Activite.DateActivite.Date <= dateFin.Value.Date)).ToList();

            forums = forums.Where(x => x.Activite.DateActivite >= minDate
                && (!dateDebut.HasValue || x.Activite.DateActivite.Date >= dateDebut.Value.Date)
                && (!dateFin.HasValue || x.Activite.DateActivite.Date <= dateFin.Value.Date)).ToList();

            presses = presses.Where(x => x.Activite.DateActivite >= minDate
                && (!dateDebut.HasValue || x.Activite.DateActivite.Date >= dateDebut.Value.Date)
                && (!dateFin.HasValue || x.Activite.DateActivite.Date <= dateFin.Value.Date)).ToList();

            radios = radios.Where(x => x.Activite.DateActivite >= minDate
                && (!dateDebut.HasValue || x.Activite.DateActivite.Date >= dateDebut.Value.Date)
                && (!dateFin.HasValue || x.Activite.DateActivite.Date <= dateFin.Value.Date)).ToList();

            reunions = reunions.Where(x => x.Activite.DateActivite >= minDate
                && (!dateDebut.HasValue || x.Activite.DateActivite.Date >= dateDebut.Value.Date)
                && (!dateFin.HasValue || x.Activite.DateActivite.Date <= dateFin.Value.Date)).ToList();

            salons = salons.Where(x => x.Activite.DateActivite >= minDate
                && (!dateDebut.HasValue || x.Activite.DateActivite.Date >= dateDebut.Value.Date)
                && (!dateFin.HasValue || x.Activite.DateActivite.Date <= dateFin.Value.Date)).ToList();

            tvs = tvs.Where(x => x.Activite.DateActivite >= minDate
                && (!dateDebut.HasValue || x.Activite.DateActivite.Date >= dateDebut.Value.Date)
                && (!dateFin.HasValue || x.Activite.DateActivite.Date <= dateFin.Value.Date)).ToList();

            var allActivites = journees.Select(x => new { x.Activite.AgenceWilayaId, Type = 1 })
                .Concat(forums.Select(x => new { x.Activite.AgenceWilayaId, Type = 3 }))
                .Concat(presses.Select(x => new { x.Activite.AgenceWilayaId, Type = 7 }))
                .Concat(radios.Select(x => new { x.Activite.AgenceWilayaId, Type = 5 }))
                .Concat(salons.Select(x => new { x.Activite.AgenceWilayaId, Type = 2 }))
                .Concat(reunions.Select(x => new { x.Activite.AgenceWilayaId, Type = 4 }))
                .Concat(tvs.Select(x => new { x.Activite.AgenceWilayaId, Type = 6 }))
                .ToList();

            var dashboard = agences
                .GroupJoin(
                    allActivites,
                    ag => ag.Id,
                    ac => ac.AgenceWilayaId,
                    (ag, acts) => new DashboardActiviteVM
                    {
                        StructureCode = ag.Code,
                        StructureDesignation = ag.DesignationAr,
                        JourneeInfoCount = acts.Count(x => x.Type == 1),
                        SalonCount = acts.Count(x => x.Type == 2),
                        ForumCount = acts.Count(x => x.Type == 3),
                        ReunionCount = acts.Count(x => x.Type == 4),
                        RadioCount = acts.Count(x => x.Type == 5),
                        TVCount = acts.Count(x => x.Type == 6),
                        PresseCount = acts.Count(x => x.Type == 7),
                    })
                .OrderBy(x => x.StructureCode)
                .ToList();

            var model = new DashboardFilterVM
            {
                DateDebut = dateDebut,
                DateFin = dateFin,
                Data = dashboard,
            };

            return View(model);
        }
    }
}