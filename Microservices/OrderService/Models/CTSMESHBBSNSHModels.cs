using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderService.Models
{
    [Table("OESCTSMESHBBSNSH")]
    public class CTSMESHBBSNSHModels
    {
        [Key, Column(Order = 1)]
        public string CustomerCode { get; set; }
        [Key, Column(Order = 2)]
        public string ProjectCode { get; set; }
        [Key, Column(Order = 3)]
        public int JobID { get; set; }
        [Key, Column(Order = 4)]
        public int BBSID { get; set; }

        public bool BBSOrder { get; set; }
        public string BBSProdCategory { get; set; }

        public string BBSStrucElem { get; set; }

        public string BBSDesc { get; set; }

        public int BBSTotalPcs { get; set; }
        public decimal BBSTotalWT { get; set; }

        public string BBSDrawing { get; set; }
        public string BBSDrawingRev { get; set; }

        public int BBSNDSPostID { get; set; }
        public DateTime BBSNDSPostDate { get; set; }
        public string BBSNDSPostBy { get; set; }
        public string BBSNDSGroupMark { get; set; }

        public string BBSSOR { get; set; }

        public DateTime UpdateDate { get; set; }
        public string UpdateBy { get; set; }
    }
}