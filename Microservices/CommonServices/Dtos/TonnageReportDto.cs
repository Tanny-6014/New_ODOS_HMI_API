namespace CommonServices.Dtos
{
    public class TonnageReportDto
    {
        public string Postedby { get; set; }
        public string PostedDate { get; set; }
        public string ReleasedBy { get; set; }
        public string ReleasedDate{ get; set; }
        public decimal PostedWeight { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string CustomerName { get; set; }
        public string WBS1 { get; set; }
        public string WBS2{ get; set; }
        public string WBS3 { get; set; }
        public string StructureElement{ get; set; }
        public string ProductType{ get; set; }
        public string ProjectCode{ get; set; }
        public string ContractNumber{ get; set; }
        public string BBSNo{ get; set; }
        public string OrderReqNo{ get; set; }
    }
}
