using System;
using System.ComponentModel.DataAnnotations;

namespace Anade.Khadamat.Web.ViewModels
{
    public class ActivitePresseVM
    {

        [Display(Name = "الصحيفة / الموقع الإلكتروني")]
        [Required(ErrorMessage = "La chaine television est obligatoire")]
        [StringLength(200)]
        public string Media { get; set; }


        [Display(Name = "الموضوع")]
        [Required(ErrorMessage = "La date est obligatoire")]
        [DataType(DataType.Date)]
        public DateTime DateActivite { get; set; } = DateTime.Today;

        [Display(Name = "التاريخ")]
        [Required(ErrorMessage = "Le sujet est obligatoire")]
        [StringLength(500, ErrorMessage = "Les organisateurs ne peuvent dépasser 500 caractères")]
        public string Sujet { get; set; }

    }
}
