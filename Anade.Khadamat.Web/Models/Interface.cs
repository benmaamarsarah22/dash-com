using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anade.Khadamat.Web.Models
{
   public interface Interface
    {
        public int Duree { get; set; }
        public int TypesId { get; set; }
        public bool IsRequired { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public IFormFile File { get; set; }

    }
}
