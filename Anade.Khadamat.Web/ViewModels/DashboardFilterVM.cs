using System.Collections.Generic;

namespace Anade.Khadamat.Web.ViewModels
{
    public class DashboardFilterVM
    {
        public int? Mois { get; set; }
        public int? Annee { get; set; }

        public List<DashboardActiviteVM> Data { get; set; }
 

    }
}
