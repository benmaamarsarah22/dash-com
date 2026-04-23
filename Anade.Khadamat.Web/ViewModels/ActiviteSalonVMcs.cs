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

        [Required(ErrorMessage = "المنظمين إجباري ")]
        [StringLength(300, ErrorMessage = "لا يمكن أن يتجاوز المنظمين 300 حرف")]
        public string Organisateurs { get; set; }

        [Required(ErrorMessage = "المشاركين إجباري ")]
        [StringLength(500, ErrorMessage = "لا يمكن أن يتجاوز المشاركين 500 حرف")]
        public string Participants { get; set; }


        [Required(ErrorMessage = "عدد الزوار إجباري ")]
        [Range(0, int.MaxValue, ErrorMessage = "يجب أن يكون عدد الزوار رقماً موجباً")]
        public int? NombreVisiteurs { get; set; }
         
        [Required(ErrorMessage = "التاريخ إجباري")]
        [DataType(DataType.Date)]
        public DateTime DateActivite { get; set; } = DateTime.Today;
    }
}