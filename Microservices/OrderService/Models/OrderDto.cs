namespace OrderService.Models
{
    public class OrderDto
    {
        public string OrderRequestNo { get; set; }
        public string OrderNo { get; set; }

        public string CustCode { get; set; }
        public string CustName { get; set; }
        public string ContractNo { get; set; }
        public string ProjNo { get; set; }
        public string ProjName { get; set; }
        public string CustPoNo { get; set; }
        public string PRStatus { get; set; }
        public string TypeofOutsourcing { get; set; }
        public string CustOrdDate { get; set; }
        public string ReqDelDatefr { get; set; }
        public string ReqDelDateto { get; set; }
        public string ProjSeg { get; set; }
        public string ProjSubSeg { get; set; }
        public string ProductType { get; set; }
        public string CreateBy { get; set; }
        public string CreateDate { get; set; }

        public decimal? Score { get; set; }
        public string AssignmentStatus { get; set; }
        public string AssignedTo { get; set; }

        public string SOWt { get; set; }
        public string NoofCuts { get; set; }
        public string NoofPieces { get; set; }

        public string SLBendings { get; set; }

        public string SLPieces { get; set; }
        public string SCBMRun { get; set; }

        public string SCBPieces { get; set; }

        public string SBCPieces { get; set; }

        public string CouplrBrPeices { get; set; }

        public string CouplrEnds { get; set; }
    }
}
