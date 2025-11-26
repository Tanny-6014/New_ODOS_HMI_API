using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderService.Models
{
    [Table("OESStdSheetMaster")]
    public class StdSheetMasterModels
    {
        [Key]
        public int std_sheet_id { get; set; }
        public int s_no { get; set; }
        public string std_type { get; set; }
        public string mesh_series { get; set; }
        public string ss561_name { get; set; }
        public string ss32_name { get; set; }
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
        public string sap_mcode { get; set; }
        public int oes_order { get; set; }

        public string GreenType { get; set; }
    }
}