using Anade.Domain.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Anade.Khadamat.Domain.Entity
{ 
    public class Activite : IEntity<int>
    {
        public int Id { get; set; }


        
        public int? AgenceWilayaId { get; set; }

        [ForeignKey("AgenceWilayaId")]
        public AgenceWilaya AgenceWilaya { get; set; }


        [Required]
        public int StructureId { get; set; }  // Agence Wilaya

        [Required]
        public string UserId { get; set; }    // Agent communication


        [Required]
        public string UserName { get; set; }
       
        public string structureDesignation { get; set; }

        [Required]
        public string structureCode { get; set; }

        [Required]
        public int TypeActiviteId { get; set; }

        public TypeActivite TypeActivite { get; set; }

        [Required]
        public DateTime DateActivite { get; set; }

        [StringLength(300)]
        public string Sujet { get; set; }

        [StringLength(300)]
        public string Lieu { get; set; }

        public string Organisateurs { get; set; }

        public string Participants { get; set; }

        public int? NombreVisiteurs { get; set; }
        [NotMapped]
        public int Mois => DateActivite.Month;

        [NotMapped]
        public int Annee => DateActivite.Year;

        public List<ActiviteJourneeInfo> activiteJournesInfos { get; set; }
        public List<ActiviteForum> activiteForums { get; set; }
        public List<ActivitePresse> activitePresse { get; set; }
        public List<ActiviteRadio> activiteRadio { get; set; }
        public List<ActiviteReunionExterne> activiteReunionExterne { get; set; }
        public List<ActiviteSalon> activiteSalon { get; set; }
        public List<ActiviteTelevision> activiteTelevision { get; set; }
       

    }


}
    