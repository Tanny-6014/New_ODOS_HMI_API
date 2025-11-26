using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderService.Models
{
    [Table("OESProjOrder")]
    public class OrderProjectModels
    {
        [Key]
        public int OrderNumber { get; set; }
        public string CustomerCode { get; set; }
        public string ProjectCode { get; set; }
        public int OrderJobID { get; set; }      
        public string OrderType { get; set; }

        public string WBS1 { get; set; }
        public string WBS2 { get; set; }
        public string WBS3 { get; set; }
        public string Remarks { get; set; }

        public string DeliveryAddress { get; set; }

        public string SiteEngr_Name { get; set; }
        public string SiteEngr_HP { get; set; }
        public string SiteEngr_Email { get; set; }

        public string Scheduler_Name { get; set; }
        public string Scheduler_HP { get; set; }
        public string Scheduler_Email { get; set; }

        public decimal? TotalWeight { get; set; }

        public string OrderStatus { get; set; }     //New;Created;Sent;Submitted;Processed

        public string OrderSource { get; set; }

        public string PONumber { get; set; }
        public DateTime? PODate { get; set; }    //yyyy-mm-dd
        public DateTime? RequiredDate { get; set; }    //yyyy-mm-dd
        public DateTime? OrigReqDate { get; set; }    //yyyy-mm-dd
        public string TransportMode { get; set; }

        public DateTime UpdateDate { get; set; }

        public string UpdateBy { get; set; }

        public DateTime? SubmitDate { get; set; }

        public string SubmitBy { get; set; }
        public bool? OrderShared { get; set; }
        public int? OrderReferenceNo { get; set; }

        public bool? GreenSteel { get; set; }

        public string? Address { get; set; }

        public string? Gate { get; set; }

        public string? AddressCode { get; set; }

    }

    public class OrderSearchModels
    {
        public string CustomerCode { get; set; }
        public string ProjectCode { get; set; }
        public string PONumber { get; set; }
        public DateTime PODate { get; set; }    //yyyy-mm-dd
        public DateTime RequiredDate { get; set; }    //yyyy-mm-dd

        public string WBS1 { get; set; }
        public string WBS2 { get; set; }
        public string WBS3 { get; set; }
    }

    public class OrderProdModels
    {
        public string SECode { get; set; }
        public List<string> ProdCode  { get; set; }
        public List<string> ProdName { get; set; }
    }
}