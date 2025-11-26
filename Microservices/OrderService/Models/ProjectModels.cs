using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderService.Models
{
    [Table("OESProject")]
    public class ProjectModels
    {
        [Key, Column(Order = 1)]
        [Display(Name = "Customer Code")]
        public string CustomerCode { get; set; }

        [Display(Name = "Project Code")]
        [Key, Column(Order = 2)]
        public string ProjectCode { get; set; }

        [Display(Name = "Project Title")]
        public string ProjectTitle { get; set; }

        public string SiteEngr_Name { get; set; }
        public string SiteEngr_HP { get; set; }
        public string SiteEngr_Tel { get; set; }

        public string Scheduler_Name { get; set; }
        public string Scheduler_HP { get; set; }
        public string Scheduler_Tel { get; set; }

        public string Contact1 { get; set; }
        public string Contact2 { get; set; }
        public string Contact3 { get; set; }
        public string Contact4 { get; set; }
        public string Contact5 { get; set; }
        public string Contact6 { get; set; }

        public string Tel1 { get; set; }
        public string Tel2 { get; set; }
        public string Tel3 { get; set; }
        public string Tel4 { get; set; }
        public string Tel5 { get; set; }
        public string Tel6 { get; set; }

        public string Email1 { get; set; }
        public string Email2 { get; set; }
        public string Email3 { get; set; }
        public string Email4 { get; set; }
        public string Email5 { get; set; }
        public string Email6 { get; set; }

        public string ProjectCodeCAB { get; set; }
        public string ProjectCodeMESH { get; set; }
        public string ProjectCodeCage { get; set; }

        public bool? EspliceN { get; set; }
        public bool? EspliceS { get; set; }
        public bool? Nsplice { get; set; }

        public string EmailDistribution { get; set; }
        public bool? AdvancedOrder { get; set; }
        public bool? ProjectCAB { get; set; }
        public bool? ProjectMESH { get; set; }
        public bool? ProjectBPC { get; set; }
        public bool? ProjectCage { get; set; }

        public int MaxBarLength { get; set; }

        public bool? bpc_template_editable { get; set; }
        public bool? bpc_change_cagedata { get; set; }
        public bool? bpc_order_misccages { get; set; }
        public bool? bpc_spiral_lapping { get; set; }

        public string CustomerBar { get; set; }
        public bool? SkipBendCheck { get; set; }
        public bool? AllowGrade500M { get; set; }
        public bool? VarianceBarSplit { get; set; }
        public string TransportMode { get; set; }
        public string PaymentType { get; set; }
        public string BBSStandard { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateBy { get; set; }
    }

    public class ProjectAccessModels
    {
        [Key, Column(Order = 1)]
        public string CustomerCode { get; set; }
        [Key, Column(Order = 2)]
        public string ProjectCode { get; set; }
        public string ProjectTitle { get; set; }
        public string SiteEngr_Name { get; set; }
        public string SiteEngr_HP { get; set; }
        public string SiteEngr_Tel { get; set; }
        public string Scheduler_Name { get; set; }
        public string Scheduler_HP { get; set; }
        public string Scheduler_Tel { get; set; }
        public string Contact1 { get; set; }
        public string Contact2 { get; set; }
        public string Contact3 { get; set; }
        public string Contact4 { get; set; }
        public string Contact5 { get; set; }
        public string Contact6 { get; set; }
        public string Tel1 { get; set; }
        public string Tel2 { get; set; }
        public string Tel3 { get; set; }
        public string Tel4 { get; set; }
        public string Tel5 { get; set; }
        public string Tel6 { get; set; }
        public string Email1 { get; set; }
        public string Email2 { get; set; }
        public string Email3 { get; set; }
        public string Email4 { get; set; }
        public string Email5 { get; set; }
        public string Email6 { get; set; }
        public string OrderSubmission { get; set; }
        public string OrderCreation { get; set; }
        public bool? AdvancedOrder { get; set; }
        public int MaxBarLength { get; set; }
        public bool? bpc_template_editable { get; set; }
        public bool? bpc_change_cagedata { get; set; }
        public bool? bpc_order_misccages { get; set; }
        public string CustomerBar { get; set; }
        public string SkipBendCheck { get; set; } // Y/N
        public string BBSStandard { get; set; }
        public string VarianceBarSplit { get; set; }

    }
}