using System;
using System.ComponentModel.DataAnnotations;

namespace Anade.Khadamat.Web.ViewModels
{
    public class ActiviteTelevisionVM
    {
        [Display(Name = "القناة التلفزيونية")]
        [Required(ErrorMessage = "La chaine television est obligatoire")]
        [StringLength(200)]
        public string ChaineTV { get; set; }


        [Display(Name = "التاريخ")]
        [Required(ErrorMessage = "La date est obligatoire")]
        [DataType(DataType.Date)]
        public DateTime DateActivite { get; set; } = DateTime.Today;

        [Display(Name = "موضوع ")]
        [Required(ErrorMessage = "Le sujet est obligatoire")]
        [StringLength(500, ErrorMessage = "Les organisateurs ne peuvent dépasser 500 caractères")]
        public string Sujet { get; set; }

    }
}
