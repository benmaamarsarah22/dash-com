using System;
using System.ComponentModel.DataAnnotations;

namespace Anade.Khadamat.Web.ViewModels
{
    public class ActiviteJourneeInfoVM
    {
        [Display(Name = "موضوع النشاط")]
        [Required(ErrorMessage = "Le sujet est obligatoire")]
        [StringLength(200, ErrorMessage = "Le sujet ne peut dépasser 200 caractères")]
        public string Sujet { get; set; }


        [Display(Name = "مكان النشاط")]
        [Required(ErrorMessage = "Le lieu est obligatoire")]
        [StringLength(200, ErrorMessage = "Le lieu ne peut dépasser 200 caractères")]
        public string Lieu { get; set; }


        [Display(Name = "المنظمين")]
        [Required(ErrorMessage = "Les organisateurs sont obligatoires")]
        [StringLength(300, ErrorMessage = "Les organisateurs ne peuvent dépasser 300 caractères")]
        public string Organisateurs { get; set; }



        [Display(Name = "المشاركين")]
        [StringLength(500, ErrorMessage = "Les participants ne peuvent dépasser 500 caractères")]
        public string Participants { get; set; }

 
        [Display(Name = "عدد الزوار")]
        [Range(0, int.MaxValue, ErrorMessage = "Le nombre de visiteurs doit être positif")]
        public int? NombreVisiteurs { get; set; }

        [Display(Name = "التاريخ")]
        [Required(ErrorMessage = "La date est obligatoire")]
        [DataType(DataType.Date)]
        public DateTime DateActivite { get; set; } = DateTime.Today;
    }
}