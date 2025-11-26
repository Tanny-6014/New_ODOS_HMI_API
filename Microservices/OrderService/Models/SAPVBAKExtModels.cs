using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderService.Models
{
    public class SAPVBAKExtModels
    {
        public string YWIREROD_IND { get; set; }
        public string YREBAR_IND { get; set; }
        public string YCAB_IND { get; set; }
        public string YMESH_IND { get; set; }
        public string YPRECAGE { get; set; }
        public string YBPC_IND { get; set; }
        public string YPCSTRAND { get; set; }
        public string YMAT_SOURCE { get; set; }
        public string YCOLD_ROLL_WIRE { get; set; }
        public string YPRE_CUT_WIRE { get; set; }
        public string YCARPET_IND { get; set; }
        public string YCSURCHARGE_IND { get; set; }
        public string YPRCMSH_IND { get; set; }
        public decimal YTOT_REBAR { get; set; }
        public decimal YTOT_MESH { get; set; }
        public decimal YTOT_WIROD { get; set; }
        public decimal YTOT_CRWIC { get; set; }
        public decimal YTOT_PCSTR { get; set; }
        public decimal YTOT_COUPLER { get; set; }
        public decimal YTOT_GRAND { get; set; }
    }
}