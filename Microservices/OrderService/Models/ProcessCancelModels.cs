using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OrderService.Models
{
    [Table("OESCancelProcess")]
    public class ProcessCancelModels
    {
        [Key, Column(Order = 1)]
        public string CustomerCode { get; set; }
        [Key, Column(Order = 2)]
        public string ProjectCode { get; set; }
        [Key, Column(Order = 3)]
        public int JobID { get; set; }
        [Key, Column(Order = 4)]
        public string StructureElement { get; set; }
        [Key, Column(Order = 5)]
        public string ProdType { get; set; }
        [Key, Column(Order = 6)]
        public string ScheduledProd { get; set; }

        public string Contract { get; set; }
        public bool? NonContract { get; set; }
        public bool? CashPayment { get; set; }
        public string CABFormer { get; set; }
        public string ShipToParty { get; set; }
        public string ProjectStage { get; set; }
        public DateTime? RequiredDateFrom { get; set; }
        public DateTime? RequiredDateTo { get; set; }
        public string PONumber { get; set; }
        public string WBS1 { get; set; }
        public string WBS2 { get; set; }
        public string WBS3 { get; set; }
        public decimal? TotalCABWeight { get; set; }
        public decimal? TotalSTDWeight { get; set; }
        public string TransportLimit { get; set; }
        public string VehicleType { get; set; }
        public bool Urgent { get; set; }
        public bool Conquas { get; set; }
        public bool Crane { get; set; }
        public bool Premium { get; set; }
        public bool ZeroTol { get; set; }
        public bool CallDel { get; set; }
        public bool SpecialPass { get; set; }
        public bool LowBed { get; set; }
        public bool Veh50Ton { get; set; }
        public bool Borge { get; set; }
        public bool PoliceEscort { get; set; }
        public string TimeRange { get; set; }
        public string IntRemarks { get; set; }
        public string ExtRemarks { get; set; }
        public DateTime UpdateDate { get; set; }
        public string ProcessedBy { get; set; }
        [Key, Column(Order = 7)]
        public DateTime CancelDate { get; set; }
        public string CancelledBy { get; set; }
        public string SAPSO { get; set; }
        public bool? DoNotMix { get; set; }
        public string OrderType { get; set; }
        public string InvRemarks { get; set; }
        public bool? FabricateESM { get; set; }
    }
}