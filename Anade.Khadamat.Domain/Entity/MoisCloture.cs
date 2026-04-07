using Anade.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anade.Khadamat.Domain.Entity
{
    public class MoisCloture : IEntity<int>
    {
        public int Id { get; set; }

        public int Annee { get; set; }

        public int Mois { get; set; } // 1 = Janvier, 12 = Décembre

        public bool IsCloture { get; set; } = false;

        public DateTime? DateCloture { get; set; } // Date de clôture

        public string CloturePar { get; set; } // UserId du DG par exemple
    }
}
