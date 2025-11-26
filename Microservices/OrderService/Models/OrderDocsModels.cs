using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderService.Models
{
    [Table("OESOrderDocs")]
    public class OrderDocsModels
    {
        [Key, Column(Order = 1)]
        public int OrderNumber { get; set; }

        [Key, Column(Order = 2)]
        public string StructureElement { get; set; }

        [Key, Column(Order = 3)]
        public string ProductType { get; set; }

        [Key, Column(Order = 4)]
        public string ScheduledProd { get; set; }

        [Key, Column(Order = 5)]
        public int ItemID { get; set; }

        [Key, Column(Order = 6)]
        public int DocID { get; set; }

        public int RevID { get; set; }

        public string DocumentNo { get; set; }

        public string FileName { get; set; }

        public string Remarks { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string UpdatedBy { get; set; }

        public byte[] OrderDoc { get; set; }

    }

    [Table("OESOrderDocsRev")]
    public class OrderDocsRevModels
    {
        [Key, Column(Order = 1)]
        public int OrderNumber { get; set; }

        [Key, Column(Order = 2)]
        public string StructureElement { get; set; }

        [Key, Column(Order = 3)]
        public string ProductType { get; set; }

        [Key, Column(Order = 4)]
        public string ScheduledProd { get; set; }

        [Key, Column(Order = 5)]
        public int ItemID { get; set; }

        [Key, Column(Order = 6)]
        public int DocID { get; set; }

        [Key, Column(Order = 7)]
        public int RevID { get; set; }

        public string DocumentNo { get; set; }

        public string FileName { get; set; }

        public string Remarks { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string UpdatedBy { get; set; }

        public byte[] OrderDoc { get; set; }
    }
}