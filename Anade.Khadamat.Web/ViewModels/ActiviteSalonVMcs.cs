using System;
using System.ComponentModel.DataAnnotations;

namespace Anade.Khadamat.Web.ViewModels
{
    public class ActiviteSalonVM
    {
        [Required(ErrorMessage = "الموضوع إجباري")]
        [StringLength(200, ErrorMessage = "لا يمكن أن يتجاوز الموضوع 200 حرف")]
        public string Sujet { get; set; }

        [Required(ErrorMessage = "المكان إجباري")]
        [StringLength(50, ErrorMessage = "لا يمكن أن يتجاوز المكان 50 حرف")]
        public string Lieu { get; set; }

        [Required(ErrorMessage = "المنظمون إجباريون")]
        [StringLength(300, ErrorMessage = "لا يمكن أن يتجاوز المنظمون 300 حرف")]
        public string Organisateurs { get; set; }

        [Display(Name = "المشاركون")]
        [StringLength(500, ErrorMessage = "لا يمكن أن يتجاوز المشاركون 500 حرف")]
        public string Participants { get; set; }

        [Display(Name = "عدد الزوار")]
        [Range(0, int.MaxValue, ErrorMessage = "يجب أن يكون عدد الزوار رقماً موجباً")]
        public int? NombreVisiteurs { get; set; }

        [Display(Name = "التاريخ")]
        [Required(ErrorMessage = "التاريخ إجباري")]
        [DataType(DataType.Date)]
        public DateTime DateActivite { get; set; } = DateTime.Today;
    }
}