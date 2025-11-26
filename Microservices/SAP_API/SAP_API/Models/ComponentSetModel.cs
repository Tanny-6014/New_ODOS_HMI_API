using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SAP_API.Modelss
{
    [Table("OESComponentSetOrder")]
    public class ComponentSetModel
    {
        [Key, Column(Order = 1)]
        public string CustomerCode{ get; set; }
        [Key, Column(Order = 2)]
        public string ProjectCode{ get; set; }
        [Key, Column(Order = 3)]
        public int OrderID{ get; set; }
        [Key, Column(Order = 4)]
        public int ComponentID{ get; set; }
        public int SSID { get; set; }
        public string ComponentName{ get; set; }
        public int? Revision { get; set; }
        public int? NoOfSet { get; set; }
        public int? PCsPerSet { get; set; }
        public decimal? WTPerSet { get; set; }
        public int? TotalPCs { get; set; }
        public decimal? TotalWeight { get; set; }
        public string ProductType { get; set; }
        public string StructureElement { get; set; }
        public string SAPSOR { get; set; }
        public int? PostHeaderID { get; set; }
        public string OrderStatus { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }

    }


}