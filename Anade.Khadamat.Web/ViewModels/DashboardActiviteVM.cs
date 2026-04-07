using System;
using System.ComponentModel.DataAnnotations;

namespace Anade.Khadamat.Web.ViewModels
{
    public class DashboardActiviteVM
    {
        public string StructureCode { get; set; }
        public string StructureDesignation { get; set; }

        public int JourneeInfoCount { get; set; }
        public int SalonCount { get; set; }
        public int ForumCount { get; set; }
        public int ReunionCount { get; set; }
        public int RadioCount { get; set; }
        public int TVCount { get; set; }
        public int PresseCount { get; set; }

        public int Total => JourneeInfoCount + SalonCount + ForumCount + ReunionCount + RadioCount + TVCount + PresseCount;
    }
}