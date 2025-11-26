using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderService.Models
{
    [Table("OESStdProdDetails")]
    public class StdProdDetailsModels
    {
        [Key, Column(Order = 1)]
        public string CustomerCode { get; set; }
        [Key, Column(Order = 2)]
        public string ProjectCode { get; set; }
        [Key, Column(Order = 3)]
        public int JobID { get; set; }
        [Key, Column(Order = 4)]
        public string ProdCode { get; set; }
        public int SSID { get; set; }
        public string ProdType { get; set; }
        public string ProdDesc { get; set; }
        public decimal Diameter { get; set; }
        public string Grade { get; set; }
        public decimal UnitWT { get; set; }
        public int order_pcs { get; set; }
        public decimal order_wt { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}