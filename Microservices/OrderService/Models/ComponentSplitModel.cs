using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OrderService.Models
{
    [Table("OESComponentSplit")]
    public class ComponentSplitModel
    {
        [Key, Column(Order = 1)]
        public int PostHeaderID { get; set; }
        [Key, Column(Order = 2)]
        public int ComponentID { get; set; }
        [Key, Column(Order = 3)]
        public int SplitID { get; set; }
        [Key, Column(Order = 4)]
        public int JobID { get; set; }
        [Key, Column(Order = 5)]
        public int ItemID { get; set; }

        public int? MeshMemberQty{ get; set; }
        public decimal? MeshTotalWT{ get; set; }
        public string ProductType { get; set; }
        public string StructureElement { get; set; }
        public string SplitBy{ get; set; }
        public DateTime SplitDate{ get; set; }

    }

}