using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAP_API.Models
{
    public class Batch
    {
        public string BatchNumber { get; set; }

        public string MATERIAL { get; set; }

        public string Production_Date { get; set; }

        public string Shift { get; set; }

        public string KGPERCOIL { get; set; }

        public string HEATNO { get; set; }

        public string MILLSOURCE { get; set; }

        public string STANDARD { get; set; }
    }
}