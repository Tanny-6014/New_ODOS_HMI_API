using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAP_API.Modelss
{
    [Table("OESBPCDetails")]
    public class BPCDetailsModels
    {
        [Key, Column(Order = 1)]
        public string CustomerCode { get; set; }
        [Key, Column(Order = 2)]
        public string ProjectCode { get; set; }
        [Key, Column(Order = 3)]
        public bool Template { get; set; }
        [Key, Column(Order = 4)]
        public int JobID { get; set; }
        [Key, Column(Order = 5)]
        public int cage_id { get; set; }
        public string pile_type { get; set; }
        public int pile_dia { get; set; }
        public int cage_dia { get; set; }
        public string main_bar_arrange { get; set; }
        public string main_bar_type { get; set; }
        public string main_bar_ct { get; set; }
        public string main_bar_shape { get; set; }
        public string main_bar_grade { get; set; }
        public string main_bar_dia { get; set; }
        public string main_bar_topjoin { get; set; }
        public string main_bar_endjoin { get; set; }
        public string cage_length { get; set; }
        public string spiral_link_type { get; set; }
        public string spiral_link_grade { get; set; }
        public string spiral_link_dia { get; set; }
        public string spiral_link_spacing { get; set; }
        public int lap_length { get; set; }
        public int end_length { get; set; }
        public int? per_set { get; set; }
        public string cage_location { get; set; }
        //advance options
        public int rings_start { get; set; }
        public int rings_end { get; set; }
        public int rings_addn_no { get; set; }
        public int rings_addn_member { get; set; }
        public string coupler_top { get; set; }
        public string coupler_end { get; set; }
        public int no_of_sr { get; set; }
        public string sr_grade { get; set; }
        public int sr_dia { get; set; }
        public int? sr_dia_add { get; set; }
        public int sr1_location { get; set; }
        public int sr2_location { get; set; }
        public int sr3_location { get; set; }
        public int sr4_location { get; set; }
        public int sr5_location { get; set; }
        public int crank_height_top { get; set; }
        public int crank_height_end { get; set; }
        public int? crank2_height_top { get; set; }
        public int? crank2_height_end { get; set; }
        public int sl1_length { get; set; }
        public int sl2_length { get; set; }
        public int sl3_length { get; set; }
        public int sl1_dia { get; set; }
        public int sl2_dia { get; set; }
        public int sl3_dia { get; set; }
        public int total_sl_length { get; set; }
        public int no_of_cr_top { get; set; }
        public int cr_spacing_top { get; set; }
        public int no_of_cr_end { get; set; }
        public int cr_spacing_end { get; set; }
        public string cr_end_remarks { get; set; }
        public string extra_support_bar_ind { get; set; }
        public int? extra_support_bar_dia { get; set; }
        public int? extra_cr_no { get; set; }
        public int? extra_cr_loc { get; set; }
        public int? extra_cr_dia { get; set; }
        public int mainbar_length_2layer { get; set; }
        public int mainbar_location_2layer { get; set; }
        public string bundle_same_type { get; set; }

        public string bbs_no { get; set; }
        public int cage_qty { get; set; }
        public decimal cage_weight { get; set; }
        public string cage_remarks { get; set; }
        public string set_code { get; set; }
        public string nds_groupmarking { get; set; }
        public string nds_wbs1 { get; set; }
        public string nds_wbs2 { get; set; }
        public string nds_wbs3 { get; set; }
        public string sor_no { get; set; }
        public string sap_mcode { get; set; }
        public string copyfrom_project { get; set; }
        public bool? copyfrom_template { get; set; }
        public int? copyfrom_jobid { get; set; }
        public string copyfrom_ponumber { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
    }

}