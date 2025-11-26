using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP_API.Modelss
{
    [Table("OESStdSheetDetails")]
    public class StdSheetDetailsModels
    {
        [Key, Column(Order = 1)]
        public string CustomerCode { get; set; }
        [Key, Column(Order = 2)]
        public string ProjectCode { get; set; }
        [Key, Column(Order = 3)]
        public int JobID { get; set; }
        [Key, Column(Order = 4)]
        public int SheetID { get; set; }
        public int SheetSort { get; set; }
        public string std_type { get; set; }
        public string mesh_series { get; set; }
        public string sheet_name { get; set; }
        public int mw_length { get; set; }
        public int mw_size { get; set; }
        public int mw_spacing { get; set; }
        public int mo1 { get; set; }
        public int mo2 { get; set; }
        public int cw_length { get; set; }
        public int cw_size { get; set; }
        public int cw_spacing { get; set; }
        public int co1 { get; set; }
        public int co2 { get; set; }
        public decimal unit_weight { get; set; }
        public int order_pcs { get; set; }
        public decimal order_wt { get; set; }
        public string sap_mcode { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
