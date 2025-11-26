using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace OrderService.Models
{
    [Table("OESWBSList")]
    public class WBSModels
    {
        [Key, Column(Order = 1)]
        public string ProjectCode { get; set; }
        [Key, Column(Order = 2)]
        public string WBS1 { get; set; }
        [Key, Column(Order = 3)]
        public string WBS2 { get; set; }
        [Key, Column(Order = 4)]
        public string WBS3 { get; set; }
        [Key, Column(Order = 5)]
        public string ProductType { get; set; }     //Beeam, Column, Slab, Wall, Misc
        public string StructureElem { get; set; }     //Beeam, Column, Slab, Wall, Misc
    }

    public class WBSStatusModels
    {
        public string ProjectCode { get; set; }
        public string WBS1 { get; set; }
        public string WBS2 { get; set; }
        public string WBS3 { get; set; }
        public string ProductType { get; set; }     //Beeam, Column, Slab, Wall, Misc
        public string StructureElem { get; set; }     //Beeam, Column, Slab, Wall, Misc
        public string WBSStatus { get; set; }
        public decimal WBSTonnage { get; set; }
        public int DrawingCount { get; set; }            // No of Drawing Attached
        public string SORNo { get; set; }
    }
}