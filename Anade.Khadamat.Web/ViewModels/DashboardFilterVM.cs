using System;
using System.Collections.Generic;

namespace Anade.Khadamat.Web.ViewModels
{
    public class DashboardFilterVM
    {
        public DateTime? DateDebut { get; set; }
        public DateTime? DateFin { get; set; }

        public List<DashboardActiviteVM> Data { get; set; }
 

    }
}
