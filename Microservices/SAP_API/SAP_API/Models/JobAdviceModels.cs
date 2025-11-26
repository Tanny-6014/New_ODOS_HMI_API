using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAP_API.Modelss
{
    [Table("OESJobAdvice")]
    public class JobAdviceModels
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

        public string CouplerType { get; set; }
        public string OrderStatus { get; set; }     //New;Created;Submitted;Processed
        public decimal? TotalCABWeight { get; set; }
        public decimal? TotalSTDWeight { get; set; }
        public decimal? TotalWeight { get; set; }
        public string TransportLimit { get; set; }

        public string SiteEngr_Name { get; set; }
        public string SiteEngr_HP { get; set; }
        public string SiteEngr_Tel { get; set; }

        public string Scheduler_Name { get; set; }
        public string Scheduler_HP { get; set; }
        public string Scheduler_Tel { get; set; }

        public string WBS1 { get; set; }

        public string WBS2 { get; set; }

        public string WBS3 { get; set; }

        public string DeliveryAddress { get; set; }

        public string ProjectStage { get; set; }

        public string Remarks { get; set; }

        public string OrderSource { get; set; }

        public string TransportMode { get; set; }

        public string BBSStandard { get; set; }

        public DateTime UpdateDate { get; set; }

        public string UpdateBy { get; set; }
    }

    public class JobAdviceListModels
    {
        public string ProdType { get; set; }
        public string ProdTypeDis { get; set; }
        public string CustomerCode { get; set; }
        public string ProjectCode { get; set; }
        public string CustomerName { get; set; }
        public string ProjectTitle { get; set; }
        public int JobID { get; set; }
        public int JobIDDis { get; set; }
        public string PONumber { get; set; }
        public DateTime PODate { get; set; }    //yyyy-mm-dd
        public DateTime RequiredDate { get; set; }    //yyyy-mm-dd
        public string CouplerType { get; set; }
        public string OrderStatus { get; set; }     //New;Created;Submitted;Processed
        public decimal? TotalCABWeight { get; set; }
        public decimal? TotalSTDWeight { get; set; }
        public decimal? TotalWeight { get; set; }
        public string TransportLimit { get; set; }
        public string TransportMode { get; set; }
        public string WBS1 { get; set; }
        public string WBS2 { get; set; }
        public string WBS3 { get; set; }
        public string ProjectStage { get; set; }
        public string Remarks { get; set; }
        public string SAPSONo { get; set; }
        public string SAPPONo { get; set; }
        public string SORNo { get; set; }
        public string SORNoDis { get; set; }
        public string DeliveryAddress { get; set; }
        public string SiteEngr_Name { get; set; }
        public string SiteEngr_HP { get; set; }
        public string Scheduler_Name { get; set; }
        public string Scheduler_HP { get; set; }
        public DateTime UpdateDate { get; set; }
        public string StructureElementDis { get; set; }
        public string OrderSource { get; set; }
        public string StructureElement { get; set; }
        public string ScheduledProd { get; set; }
        public string ProjectIncharge { get; set; }
        public string DetailingIncharge { get; set; }
        public string BBSNo { get; set; }
        public int PostHeaderID { get; set; }
        public DateTime? OrigReqDate { get; set; }
        public int AttachedNo { get; set; }
        public string PlanDelDate { get; set; }
        public string ConfirmedDelDate { get; set; }
        public string PMDRemarks { get; set; }
        public string PMDRemarksHis { get; set; }
        public string TECHRemarks { get; set; }
        public string TECHRemarksHis { get; set; }
        public string UpdateBy { get; set; }
        public string DataEntryBy { get; set; }
        public string RunNo { get; set; }
        public string OrderType { get; set; }
        public string ProcessDate { get; set; }
        public string ForecastDate { get; set; }
        public string LastDeliveryDate { get; set; }
        public int DiffDays { get; set; }
    }

    public class JobAdviceListExtModels
    {
        public string ProdType { get; set; }
        public string ProdTypeDis { get; set; }
        public string CustomerCode { get; set; }
        public string ProjectCode { get; set; }
        public string CustomerName { get; set; }
        public string ProjectTitle { get; set; }
        public int JobID { get; set; }
        public int JobIDDis { get; set; }
        public string PONumber { get; set; }
        public DateTime PODate { get; set; }    //yyyy-mm-dd
        public DateTime RequiredDate { get; set; }    //yyyy-mm-dd
        public string CouplerType { get; set; }
        public string OrderStatus { get; set; }     //New;Created;Submitted;Processed
        public decimal? TotalCABWeight { get; set; }
        public decimal? TotalSTDWeight { get; set; }
        public decimal? TotalWeight { get; set; }
        public string TransportLimit { get; set; }
        public string TransportMode { get; set; }
        public string WBS1 { get; set; }
        public string WBS2 { get; set; }
        public string WBS3 { get; set; }
        public string ProjectStage { get; set; }
        public string Remarks { get; set; }
        public string SAPSONo { get; set; }
        public string SAPPONo { get; set; }
        public string SORNo { get; set; }
        public string SORNoDis { get; set; }
        public string DeliveryAddress { get; set; }
        public string SiteEngr_Name { get; set; }
        public string SiteEngr_HP { get; set; }
        public string Scheduler_Name { get; set; }
        public string Scheduler_HP { get; set; }
        public DateTime UpdateDate { get; set; }
        public string StructureElementDis { get; set; }
        public string OrderSource { get; set; }
        public string StructureElement { get; set; }
        public string ScheduledProd { get; set; }
        public string ProjectIncharge { get; set; }
        public string DetailingIncharge { get; set; }
        public string BBSNo { get; set; }
        public int PostHeaderID { get; set; }
        public DateTime? OrigReqDate { get; set; }
        public int AttachedNo { get; set; }
        public string PlanDelDate { get; set; }
        public string ConfirmedDelDate { get; set; }
        public string PMDRemarks { get; set; }
        public string PMDRemarksHis { get; set; }
        public string TECHRemarks { get; set; }
        public string TECHRemarksHis { get; set; }
        public string UpdateBy { get; set; }
        public string DataEntryBy { get; set; }
        public string RunNo { get; set; }
        public string OrderType { get; set; }
        public string ProcessDate { get; set; }
        public string ForecastDate { get; set; }
        public string LastDeliveryDate { get; set; }
        public int DiffDays { get; set; }

        //Added by Siddhi

        public string Int_Remark { get; set; }

        public string Ext_Remark { get; set; }

        public string BBSDesc { get; set; }

        public string ERROR_CD { get; set; }

        public string UserID { get; set; }

        public string URG_ORD_IND { get; set; }
        public string PRM_SVC_IND { get; set; }
        public string CRN_BKD_IND { get; set; }
        public string BRG_BKD_IND { get; set; }
        public string POL_ESC_IND { get; set; }
        public string ON_HOLD_IND { get; set; }
        public string ZERO_TOLERANCE_I { get; set; }
        public string CALL_BEF_DEL_IND { get; set; }
        public string CONQUAS_IND { get; set; }
        public string SPECIAL_PASS_IND { get; set; }
        public string LORRY_CRANE_IND { get; set; }
        public string LOW_BED_IND { get; set; }
        public string T50_VEH_IND { get; set; }

        public string Total_MT_SAPY { get; set; }//total_Mt_sapy

        public string SubSegment { get; set; }

        public string DeliveredPcs { get; set; }

        public string LoadNo { get; set; }

        public string PPContract { get; set; }

        public string ContractNo { get; set; }

        public string ProjCoord { get; set; }

        public string DeliveryNo { get; set; }

        public string SO_REJECT_STATUS { get; set; }

        public string SORStatus { get; set; }
        public string NDSStatus { get; set; }

        public string DetailerName { get; set; }

        public string NDSReleaseTime { get; set; }

        public string Cust_Lead_Time { get; set; }

        public string OrderedPcs { get; set; }

        public string BalancePCS { get; set; }

        public string Wt_No { get; set; }
        public string Wt_Date { get; set; }

        public string DeliveryStatus { get; set; }
        public string CreditStatus { get; set; }
        public string AccManager { get; set; }
    }
}