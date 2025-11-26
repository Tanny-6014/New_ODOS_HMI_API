namespace SAP_API.Models
{
    public class WBSPosting
    {
        public int INTWBSELEMENTID { get; set; }
        public string ORD_REQ_NO { get; set; }
        public string VCHWBS1 { get; set; }
        public string VCHWBS2 { get; set; }
        public string VCHWBS3 { get; set; }
        public int StructureElementID { get; set; }
        public int ProductTypeID { get; set; }
        public decimal TOTALWEIGHT { get; set; }
        public int TOTALQTY { get; set; }

        public string VCHBBSNO { get; set; }

        public string BBS_DESC { get; set; }

        public string POSTEDBY { get; set; }


        public string DATPOSTEDDATE { get; set; }

        public string INTRELEASEBY { get; set; }

        public string DATRELEASEDDATE { get; set; }

        public int TNTSTATUSID { get; set; }

        public int INTPOSTHEADERID { get; set; }

        public string CHRSTATUSCODE { get; set; }

        public string IDENTITY_NO { get; set; }


        public string ReqDate { get; set; }

        public int SOReqDetailsId { get; set; }


    }
}
