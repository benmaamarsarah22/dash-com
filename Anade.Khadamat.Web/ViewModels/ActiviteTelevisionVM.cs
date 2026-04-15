using System;
using System.ComponentModel.DataAnnotations;

namespace Anade.Khadamat.Web.ViewModels
{
    public class ActiviteTelevisionVM
    {
        [Display(Name = "القناة التلفزيونية")]
        [Required(ErrorMessage = "القناة التلفزيونية إجبارية")]
        [StringLength(100)]
        public string ChaineTV { get; set; }

        [Display(Name = "التاريخ")]
        [Required(ErrorMessage = "التاريخ إجباري")]
        [DataType(DataType.Date)]
        public DateTime DateActivite { get; set; } = DateTime.Today;

        [Display(Name = "الموضوع")]
        [Required(ErrorMessage = "الموضوع إجباري")]
        [StringLength(500, ErrorMessage = "لا يمكن أن يتجاوز الموضوع 500 حرف")]
        public string Sujet { get; set; }
    }
}
