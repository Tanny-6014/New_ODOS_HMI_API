using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderService.Models
{
    [Table("OESStdProdCode")]
    public class StdProductsModels
    {
        [Key, Column(Order = 1)]
        public string ProdType { get; set; }
        [Key, Column(Order = 2)]
        public string ProdCode { get; set; }
        public string ProdDesc { get; set; }
        public decimal Diameter { get; set; }
        public string Grade { get; set; }
        public decimal UnitWT { get; set; }
        public string GreenType { get; set; }
        public int ProdId { get; set; }
    }
}