using System;

using System.Collections.Generic;

using System.Linq;

using System.Web;

using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;

namespace OrderService.Models

{

    [Table("OESPrecastJobAdvice")]

    public class PrecastJobAdviceModels

    {

        [Key, Column(Order = 1)]

        public string CustomerCode { get; set; }

        [Key, Column(Order = 2)]

        public string ProjectCode { get; set; }

        [Key, Column(Order = 3)]

        public int JobID { get; set; }      //datetime => number ######

        public string PONumber { get; set; }

        public DateTime? PODate { get; set; }    //yyyy-mm-dd

        public DateTime? RequiredDate { get; set; }    //yyyy-mm-dd

        public string Transport { get; set; }

        public string OrderStatus { get; set; }     //New;Created;Submitted;Processed

        public int TotalPcs { get; set; }

        public decimal TotalWeight { get; set; }

        public bool AutoWBS { get; set; }

        public string WBS1 { get; set; }

        public string WBS2 { get; set; }

        public string WBS3 { get; set; }

        public string DeliveryAddress { get; set; }

        public string Remarks { get; set; }

        public string SiteEngr_Name { get; set; }

        public string SiteEngr_HP { get; set; }

        public string SiteEngr_Tel { get; set; }

        public string Scheduler_Name { get; set; }

        public string Scheduler_HP { get; set; }

        public string Scheduler_Tel { get; set; }

        public bool Model { get; set; }

        public string OrderSource { get; set; }

        public DateTime UpdateDate { get; set; }

        public string UpdateBy { get; set; }

    }

}
