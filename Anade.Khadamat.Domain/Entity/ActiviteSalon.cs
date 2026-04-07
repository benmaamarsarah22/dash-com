using Anade.Domain.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anade.Khadamat.Domain.Entity
{ //Salons & Expositions
    public class ActiviteSalon : IEntity<int>
    {
            [Key]
            public int Id { get; set; }

            [Required]
            public int ActiviteId { get; set; }

            [ForeignKey("ActiviteId")]
            public Activite Activite { get; set; }
        
    }
}
