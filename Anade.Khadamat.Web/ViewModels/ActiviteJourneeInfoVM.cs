using System;
using System.ComponentModel.DataAnnotations;
namespace Anade.Khadamat.Web.ViewModels
{
    public class ActiviteJourneeInfoVM
    {
        [Required(ErrorMessage = "الموضوع إجباري")]
        [StringLength(200, ErrorMessage = "لا يمكن أن يتجاوز الموضوع 200 حرف")]
        public string Sujet { get; set; }

        [Required(ErrorMessage = "المكان إجباري")]
        [StringLength(50, ErrorMessage = "لا يمكن أن يتجاوز المكان 50 حرفاً")]
        public string Lieu { get; set; }

        [Required(ErrorMessage = "المنظمون إجباري")]
        [StringLength(200, ErrorMessage = "لا يمكن أن يتجاوز حقل المنظمين 200 حرف")]
        public string Organisateurs { get; set; }

        [StringLength(200, ErrorMessage = "لا يمكن أن يتجاوز حقل المشاركين 200 حرف")]
        public string Participants { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "يجب أن يكون عدد الزوار رقماً موجباً")]
        public int? NombreVisiteurs { get; set; }

        [Required(ErrorMessage = "التاريخ إجباري")]
        [DataType(DataType.Date)]
        public DateTime DateActivite { get; set; } = DateTime.Today;
    }
}