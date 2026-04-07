using System;
using System.ComponentModel.DataAnnotations;

namespace Anade.Khadamat.Web.ViewModels
{
    public class ActiviteRadioVM
    {

        [Display(Name = "المحطة الإذاعية")]
        [Required(ErrorMessage = "La station radio est obligatoire")]
        [StringLength(200)]
        public string StationRadio { get; set; }



        [Display(Name = "المتدخلين")]
        [StringLength(300)]
        public string Intervenants { get; set; }


        [Display(Name = "التاريخ")]
        [Required(ErrorMessage = "La date est obligatoire")]
        [DataType(DataType.Date)]
        public DateTime DateActivite { get; set; } = DateTime.Today;


        [Display(Name = "االموضوع")]
        [Required(ErrorMessage = "Le sujet est obligatoire")]
        [StringLength(500, ErrorMessage = "Les organisateurs ne peuvent dépasser 500 caractères")]
        public string Sujet { get; set; }

        
 
    }
}