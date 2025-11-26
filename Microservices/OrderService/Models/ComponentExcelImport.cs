using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderService.Models
{
    public class ComponentExcelImport
    {
        public ComponentModel Component { get; set; }

        public string existsInSystem { get; set; }

        public int? CurrentRevision { get; set; }

        public int? NewRevision { get; set; }
    }
}