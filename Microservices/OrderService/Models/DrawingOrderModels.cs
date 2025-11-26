using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderService.Models
{
    [Table("OESDrawingsOrder")]
    public class DrawingOrderModels
    {
        [Key, Column(Order = 1)]
        public int DrawingID { get; set; }

        [Key, Column(Order = 2)]
        public int Revision { get; set; }

        [Key, Column(Order = 3)]
        public int OrderNumber { get; set; }

        [Key, Column(Order = 4)]
        public string StructureElement { get; set; }

        [Key, Column(Order = 5)]
        public string ProductType { get; set; }

        [Key, Column(Order = 6)]
        public string ScheduledProd { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string UpdatedBy { get; set; }
    }

}