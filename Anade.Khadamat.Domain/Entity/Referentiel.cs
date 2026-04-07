using Anade.Domain.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Anade.Khadamat.Domain.Entity

{
    public abstract partial class Referentiel : IEntity<int>
    {
        [Display(Name = "Id")]
        [Required(ErrorMessage = "RequiredMessage")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Display(Name = "Code")]
        [Required(ErrorMessage = "RequiredMessage")]
        public string Code { get; set; }
        [Display(Name = "DesignationFr")]
        [Required(ErrorMessage = "RequiredMessage")]
        public string DesignationFr { get; set; }
        [Display(Name = "DesignationFr")]
        [Required(ErrorMessage = "RequiredMessage")]
        public string DesignationAr { get; set; }
    }
}