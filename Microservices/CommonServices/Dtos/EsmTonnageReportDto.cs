namespace CommonServices.Dtos
{
    public class EsmTonnageReportDto
    {
        public string TrackingNo{ get; set; }
        public string CustomerName{get;set;}
        public int CustomerCode { get;set;}
        public string ProjectName { get; set; }
        public string ProjectCode { get; set; }
        public string ContractNo { get; set; }
        public string ProductType { get; set; }
        public string StructureElement { get; set; }
        public string WBS1 { get; set; }
        public string WBS2 { get; set; }
        public string WBS3 { get; set; }
        public string BBS { get; set; }
        public string BBSDesc { get; set; }
        public string PONumber { get; set; }
        public string PODate { get; set; }
        public string ReqDate { get; set; }
        public string ProdDate { get; set; }
        public string OrderType { get; set; }
        public string Location { get; set; }
        public decimal OverDelvTolerance { get; set; }
        public decimal UnderDelvTolerance { get; set; }
        public decimal EstimatedWeight { get; set; }
        public string SiteContactPerson { get; set; }
        public string Remark { get; set; }
        public string IsPosted { get; set; }
        public string IsReleased { get; set; }
        public string Postedby { get; set; }
        public string PostedDate { get; set; }
        public string ReleasedBy { get; set; }
        public string ReleasedDate { get; set; }
        public decimal PostedWeight { get; set; }

    }
}
																														
