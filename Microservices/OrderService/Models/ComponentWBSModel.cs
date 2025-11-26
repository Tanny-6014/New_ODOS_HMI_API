using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OrderService.Models
{
    [Table("OESComponentWBS")]
    public class ComponentWBSModel
    {
        [Key, Column(Order = 1)]
        public int PostHeaderID { get; set; }
        [Key, Column(Order = 2)]
        public int ComponentID { get; set; }
        [Key, Column(Order = 3)]
        public int? SplitID { get; set; }
        public int SSID { get; set; }
        public string ComponentName { get; set; }
        public int Revision { get; set; }
        public string SplitType { get; set; }
        public int? NoOfSet { get; set; }
        public int? PCsPerSet { get; set; }
        public decimal? WTPerSet { get; set; }
        public int? TotalPCs { get; set; }
        public decimal? TotalWeight { get; set; }
        public string ProductType { get; set; }
        public string StructureElement { get; set; }
        public string WBS1 { get; set; }
        public string WBS2 { get; set; }
        public string WBS3 { get; set; }
        public string BBSNo { get; set; }
        public string BBSDesc { get; set; }
        public int? SplitPostHeaderID { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateBy { get; set; }

    }



    public class CompWBSModel
    {
        [Key, Column(Order = 1)]
        public int CompWBS_ID { get; set; }

        public int ProjectId { get; set; }

        public int wbs_id { get; set; }

        public int intWBSElementid { get; set; }

        public int comp_id { get; set; }

        public int? comp_rev { get; set; }

        public int intStoreyLevelWBSId { get; set; }

        public string wbs1 { get; set; }

        public string wbs2 { get; set; }

        //public string wbs2To { get; set; }

        public string wbs3 { get; set; }

        public int qty { get; set; }

        public decimal total_wt { get; set; }

        public int total_pcs { get; set; }

        public string split_type { get; set; }

        public int split_pcs { get; set; }

        public int split_wt { get; set; }

        public string comp_wbs_status { get; set; }

        public DateTime update_date { get; set; }

        public string update_by { get; set; }

        public string BBSNo { get; set; }
    }

    public class ListCompWBSModel
    {

        public CompWBSModel CompWbs { get; set; }

        public ComponentModel Component { get; set; }

        public string WBS_Group { get; set; }

        public string WBS2From_display { get; set; }

        public string WBSS2To_display { get; set; }

        public string BBSNo { get; set; }

        public int QtyToAssign { get; set; }

    }

    


}