using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace Anade.Khadamat.Web.ViewModels
{
    public class ActiviteForumVM
    {
        [Required(ErrorMessage = "الموضوع إجباري")]
        [StringLength(200, ErrorMessage = "الموضوع لا يمكن أن يتجاوز 200 حرف")]
        public string Sujet { get; set; }

        [Required(ErrorMessage = "المكان إجباري")]
        [StringLength(50, ErrorMessage = "المكان لا يمكن أن يتجاوز 50 حرف")]
        public string Lieu { get; set; }

        [Required(ErrorMessage = "المنظمون إجباريون")]
        [StringLength(200, ErrorMessage = "المنظمون لا يمكن أن يتجاوزوا  200 حرف")]
        public string Organisateurs { get; set; }

        [StringLength(200, ErrorMessage = "المشاركون لا يمكن أن يتجاوزوا   200 حرف")]
        public string Participants { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "عدد الزوار يجب أن يكون موجباً")]
        public int? NombreVisiteurs { get; set; }

        [Required(ErrorMessage = "التاريخ إجباري")]
        [DataType(DataType.Date)]
        public DateTime DateActivite { get; set; } = DateTime.Today;
    }
}