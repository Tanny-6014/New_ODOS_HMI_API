using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderService.Models
{
    [Table("OESBPCLoad")]
    public class BPCLoadModels
    {
        [Key, Column(Order = 1)]
        public string CustomerCode { get; set; }
        [Key, Column(Order = 2)]
        public string ProjectCode { get; set; }
        [Key, Column(Order = 3)]
        public int JobID { get; set; }
        [Key, Column(Order = 4)]
        public int CageID { get; set; }
        [Key, Column(Order = 5)]
        public int LoadID { get; set; }

        public int pile_dia { get; set; }
        public int cage_qty { get; set; }

        public string sap_sor { get; set; }
        public string sap_so { get; set; }

        public DateTime UpdateDate { get; set; }
        public string UpdateBy { get; set; }
    }
}