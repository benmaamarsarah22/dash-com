using Anade.Domain.Core;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Anade.Khadamat.Domain.Entity
{
    public class MoisCloture : IEntity<int>
    {
        public int Id { get; set; }

        [Required]
        [Range(2000, 2100)]
        public int Annee { get; set; }

        [Required]
        [Range(1, 12)]
        public int Mois { get; set; }

        public bool IsCloture { get; set; } = false;

        public DateTime? DateCloture { get; set; }

        [StringLength(450)]
        public string CloturePar { get; set; }

        public DateTime? DateReouverture { get; set; }

        [StringLength(450)]
        public string ReouvertPar { get; set; }

        [NotMapped]
        public string MoisLabel => new DateTime(Annee, Mois, 1).ToString("MMMM yyyy");
    }
}
