using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderService.Models
{
    [Table("OESProjOrdersSE")]
    public class OrderProjectSEModels
    {
        [Key, Column(Order = 1)]
        public int OrderNumber { get; set; }
        [Key, Column(Order = 2)]
        public string StructureElement { get; set; }
        [Key, Column(Order = 3)]
        public string ProductType { get; set; }
        [Key, Column(Order = 4)]
        public string ScheduledProd { get; set; }

        public string PONumber { get; set; }
        public DateTime? PODate { get; set; }    //yyyy-mm-dd
        public DateTime? RequiredDate { get; set; }    //yyyy-mm-dd
        public DateTime? OrigReqDate { get; set; }    //yyyy-mm-dd
        public string TransportMode { get; set; }

        public int CABJobID { get; set; }
        public int MESHJobID { get; set; }
        public int BPCJobID { get; set; }
        public int CageJobID { get; set; }
        public int CarpetJobID { get; set; }
        public int StdBarsJobID { get; set; }
        public int StdMESHJobID { get; set; }
        public int CoilProdJobID { get; set; }
        public int? PostHeaderID { get; set; }

        public string OrderStatus { get; set; }

        public decimal? TotalWeight { get; set; }
        public int? TotalPCs { get; set; }

        public string SAPSOR { get; set; }

        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public string ProcessBy { get; set; }
        public DateTime? ProcessDate { get; set; }

        public string SpecialRemark { get; set; }

        public string SiteContact { get; set; }

        public string Handphone { get; set; }

        public string GoodsReceiver { get; set; }

        public string GoodsReceiverHandphone { get; set; }

        public string AdditionalRemark { get; set; }
    }

}