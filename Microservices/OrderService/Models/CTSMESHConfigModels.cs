using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderService.Models
{
    [Table("OESCTSConfigBeam")]
    public class CTSMESHConfigBeamModels
    {
        [Key, Column(Order = 1)]
        public string CustomerCode { get; set; }
        [Key, Column(Order = 2)]
        public string ProjectCode { get; set; }
        [Key, Column(Order = 3)]
        public int beam_dia { get; set; }
        public int? beam_hook { get; set; }
        public int? beam_leg { get; set; }
        public string capping_type { get; set; }
        public string capping_product { get; set; }
        public int? capping_leg { get; set; }
        public int? capping_cw_length { get; set; }
    }

    [Table("OESCTSConfigColumn")]
    public class CTSMESHConfigColumnModels
    {
        [Key, Column(Order = 1)]
        public string CustomerCode { get; set; }
        [Key, Column(Order = 2)]
        public string ProjectCode { get; set; }
        [Key, Column(Order = 3)]
        public int column_dia { get; set; }
        public int? column_leg { get; set; }
        public int? clink_leg { get; set; }
        public int? clink_cw_length { get; set; }
        public string clink_len_type { get; set; }
        public string clink_len_product { get; set; }
        public int? clink_len_space { get; set; }
        public string clink_wid_type { get; set; }
        public string clink_wid_product { get; set; }
        public int? clink_wid_space { get; set; }
    }
}