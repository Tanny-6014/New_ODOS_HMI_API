using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OrderService.Models
{
    [Table("OESComponent")]
    public class ComponentModel
    {
        [Key, Column(Order = 1)]
        public int ComponentID{ get; set; }
        public string CustomerCode{ get; set; }
        public string ProjectCode{ get; set; }
        public string StructureElement { get; set; }
        public string ProductType{ get; set; }
        public string ComponentName{ get; set; }
        public int? Revision{ get; set; }
        public int? ParentID { get; set; }
        public string TransportMode{ get; set; }
        public decimal? TotalWeight { get; set; }
        public int? TotalPCs { get; set; }
        public string Status{ get; set; }
        public int? StandardInd{ get; set; }
        public string Remarks{ get; set; }
        public int? CABJobID{ get; set; }
        public int? MeshJobID{ get; set; }
        public int? CarJobID{ get; set; }
        public int? PRCJobID{ get; set; }
        public int? AccJobID{ get; set; }
        public string CreatedBy{ get; set; }
        public DateTime? CreatedDate { get; set; }

        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string ConfirmedBy{ get; set; }
        public DateTime? ConfirmedDate { get; set; }

    }

    public class DisplayComponentModel
    {
        //public ComponentModel Component { get; set; }
        public ComponentModelDateFormat Component { get; set; }

        public decimal total_wt { get; set; }

        public int total_pcs { get; set; }
    }

    public class ComponentModelDateFormat
    {
       
        public int ComponentID { get; set; }
        public string CustomerCode { get; set; }
        public string ProjectCode { get; set; }
        public string StructureElement { get; set; }
        public string ProductType { get; set; }
        public string ComponentName { get; set; }
        public int? Revision { get; set; }
        public int? ParentID { get; set; }
        public decimal? TotalWeight { get; set; }
        public int? TotalPCs { get; set; }
        public string TransportMode { get; set; }
        public string Status { get; set; }
        public int? StandardInd { get; set; }
        public int? ConfirmInd { get; set; }
        public string Remarks { get; set; }
        public int? CABJobID { get; set; }
        public int? CTSMeshJobID { get; set; }
        public int? BeamMeshJobID { get; set; }
        public int? ColumnMeshJobID { get; set; }
        public int? CarJobID { get; set; }
        public int? PRCJobID { get; set; }
        public int? AccJobID { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedDate_string { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedDate_string { get; set; }
        public string UpdatedBy { get; set; }
        public string ConfirmedDate_string { get; set; }
        public string ConfirmedBy { get; set; }
        public DateTime? ConfirmedDate { get; set; }

    }

}