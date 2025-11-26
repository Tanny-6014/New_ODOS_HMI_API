using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderService.Models
{
    [Table("OESBPCDetailsProc")]
    public class BPCDetailsProcModels
    {
        [Key, Column(Order = 1)]
        public string CustomerCode { get; set; }
        [Key, Column(Order = 2)]
        public string ProjectCode { get; set; }
        [Key, Column(Order = 3)]
        public int JobID { get; set; }
        [Key, Column(Order = 4)]
        public int cage_id { get; set; }
        [Key, Column(Order = 5)]
        public int load_id { get; set; }
        public int load_qty { get; set; }
        public DateTime required_date { get; set; }    //yyyy-mm-dd
        public string int_remarks { get; set; }
        public string ext_remarks { get; set; }
        public string nds_groupmarking { get; set; }
        public string nds_wbs1 { get; set; }
        public string nds_wbs2 { get; set; }
        public string nds_wbs3 { get; set; }
        public string sor_no { get; set; }
        public string sap_mcode { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}