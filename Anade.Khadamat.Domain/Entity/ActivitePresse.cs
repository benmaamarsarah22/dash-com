using Anade.Domain.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anade.Khadamat.Domain.Entity
{
    //Couverture  Presse
    public class ActivitePresse : IEntity<int>
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ActiviteId { get; set; }

        [ForeignKey("ActiviteId")]
        public Activite Activite { get; set; }

        public string Media { get; set; }
    } 
}
