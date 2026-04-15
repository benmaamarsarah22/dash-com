using System;
using System.ComponentModel.DataAnnotations;

namespace Anade.Khadamat.Web.ViewModels
{
    public class ActivitePresseVM
    
    {
        [Required(ErrorMessage = "الصحيفة / الموقع الإلكتروني إجباري")]
        [StringLength(100)]
        public string Media { get; set; }


        [Required(ErrorMessage = "التاريخ إجباري")]
        [DataType(DataType.Date)]
        public DateTime DateActivite { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "الموضوع إجباري")]
        [StringLength(500, ErrorMessage = "لا يمكن أن يتجاوز الموضوع 500 حرف")]
        public string Sujet { get; set; }

    }
}

