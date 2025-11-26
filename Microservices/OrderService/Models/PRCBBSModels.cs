using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderService.Models
{
    [Table("OESPRCBBS")]
    public class PRCBBSModels
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
        public string BBSStrucElem { get; set; }
        public bool BBSAssembly { get; set; }
        public int BBSDrawing { get; set; }

        public int BBSWidth { get; set; }
        public int BBSDepth { get; set; }
        public int BBSLength { get; set; }

        public int BBSQty { get; set; }

        public int BBSCABPcs { get; set; }
        public decimal BBSCABWT { get; set; }

        public int BBSBeamMESHPcs { get; set; }
        public decimal BBSBeamMESHWT { get; set; }

        public int BBSColumnMESHPcs { get; set; }
        public decimal BBSColumnMESHWT { get; set; }

        public int BBSCTSMESHPcs { get; set; }
        public decimal BBSCTSMESHWT { get; set; }

        public int BBSTotalPcs { get; set; }
        public decimal BBSTotalWT { get; set; }
        public string BBSCageMark { get; set; }

        public string BBSRemarks { get; set; }

        public int BBSNDSPostID { get; set; }
        public string BBSNDSGroupMark { get; set; }
        public int BBSNDSGroupMarkID { get; set; }

        public string BBSSOR { get; set; }

        public DateTime UpdateDate { get; set; }
        public string UpdateBy { get; set; }
    }

    [Table("OESPRCBBSNSH")]
    public class PRCBBSNSHModels
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
        public string BBSStrucElem { get; set; }
        public bool BBSAssembly { get; set; }
        public int BBSDrawing { get; set; }

        public int BBSWidth { get; set; }
        public int BBSDepth { get; set; }
        public int BBSLength { get; set; }

        public int BBSQty { get; set; }

        public int BBSCABPcs { get; set; }
        public decimal BBSCABWT { get; set; }

        public int BBSBeamMESHPcs { get; set; }
        public decimal BBSBeamMESHWT { get; set; }

        public int BBSColumnMESHPcs { get; set; }
        public decimal BBSColumnMESHWT { get; set; }

        public int BBSCTSMESHPcs { get; set; }
        public decimal BBSCTSMESHWT { get; set; }

        public int BBSTotalPcs { get; set; }
        public decimal BBSTotalWT { get; set; }
        public string BBSCageMark { get; set; }

        public string BBSRemarks { get; set; }

        public int BBSNDSPostID { get; set; }
        public string BBSNDSGroupMark { get; set; }
        public int BBSNDSGroupMarkID { get; set; }

        public string BBSSOR { get; set; }

        public DateTime UpdateDate { get; set; }
        public string UpdateBy { get; set; }
    }
}