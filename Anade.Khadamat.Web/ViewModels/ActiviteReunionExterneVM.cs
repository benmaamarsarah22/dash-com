using System;
using System.ComponentModel.DataAnnotations;

namespace Anade.Khadamat.Web.ViewModels
{
    public class ActiviteReunionExterneVM
    {
        [Required(ErrorMessage = "الموضوع إجباري")]
        [StringLength(200, ErrorMessage = "لا يمكن أن يتجاوز الموضوع 200 حرف")]
        public string Sujet { get; set; }

        [Required(ErrorMessage = "المكان إجباري")]
        [StringLength(50, ErrorMessage = "لا يمكن أن يتجاوز المكان 50 حرف")]
        public string Lieu { get; set; }

        [Required(ErrorMessage = "المنظمون إجباريون")]
        [StringLength(200, ErrorMessage = "لا يمكن أن يتجاوز المنظمون 200 حرف")]
        public string Organisateurs { get; set; }

        [StringLength(200, ErrorMessage = "لا يمكن أن يتجاوز المشاركون 200 حرف")]
        public string Participants { get; set; }

        [Required(ErrorMessage = "التاريخ إجباري")]
        [DataType(DataType.Date)]
        public DateTime DateActivite { get; set; } = DateTime.Today;
    }
}