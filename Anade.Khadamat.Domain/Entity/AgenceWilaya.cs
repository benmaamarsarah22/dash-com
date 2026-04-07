using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anade.Khadamat.Domain.Entity
{
    public class AgenceWilaya : Referentiel
    {
        [InverseProperty("AgenceWilaya")]
        public virtual ICollection<Activite> Activities { get; set; }
    }
}
