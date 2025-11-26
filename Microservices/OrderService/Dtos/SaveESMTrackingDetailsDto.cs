namespace OrderService.Dtos
{
    public class SaveESMTrackingDetailsDto
    {
        public string TrakingId { get; set; }
        public int ProjectId { get; set; }
        public int ContractNo { get; set; }
        public string PONumber { get; set; }
        public int WBSElementId { get; set; }
        public int StructureElementTypeId { get; set; }
        public int ProductTypeId { get; set; }
        public string BBSNO { get; set; }
        public string BBSSDesc { get; set; }
        public string ReqDate { get; set; }
        public string IntRemark { get; set; }
        public string ExtRemark { get; set; }
        public string OrdDate { get; set; }
        public string ProdDate { get; set; }
        public string OrderType { get; set; }
        public string Location { get; set; }
        public double OverDelTolerance { get; set; }
        public double UnderDelTolerance { get; set; }
        public string ContactPerson { get; set; }
        public double EstimatedWeight { get; set; }

    }
}
