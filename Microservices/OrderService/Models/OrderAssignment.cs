namespace OrderService.Models
{
    public class OrderAssignment
    {

        public string Codename { get; set; }
        public string Description { get; set; }
        public DateTime? ShippingAt { get; set; }
        public int Position { get; set; }
        public bool Paused { get; set; }
        public bool Completed { get; set; }
        public ExtendedHeader Extended { get; set; }
        public List<Item> Items { get; set; }
    }

    public class ExtendedHeader
    {
        public string OrderNo { get; set; }
        public string CustCode { get; set; }
        public string CustName { get; set; }
        public string ContractNo { get; set; }
        public string ProjNo { get; set; }
        public string ProjName { get; set; }
        public string CustPoNo { get; set; }
        public string? CustOrderDate { get; set; }
        public string? ReqDelDateOriginal { get; set; }
        public string? ReqDelDateRevised { get; set; }
        public DateTime? PromisedDelDate { get; set; }
        public string? ActualDelDate { get; set; }
        public string OrderType { get; set; }
        public string CustClass { get; set; }
        public string ProjSegment { get; set; }
        public string ProjSubSegment { get; set; }
        public string JobSiteLocation { get; set; }
        public string ForecastOrderNo { get; set; }
        public string AccountMgr { get; set; }
        public string SegmentMgr { get; set; }
        public string ProjCoordinator { get; set; }
        public string ProductType { get; set; }
        public string VehicleType { get; set; }
        public int OrderPieces { get; set; }
        public int DeliveredPieces { get; set; }
        public string CreditStatus { get; set; }
        public string ApprovalStatus { get; set; }
        public string DeliveryStatus { get; set; }
        public string CancellationStatus { get; set; }
        public string MatSourceInd { get; set; }
        public string UrgentOrderInd { get; set; }
        public string AllowPartialDeliveryInd { get; set; }
        public string ForecastOrderInd { get; set; }
        public string LocalOrderInd { get; set; }
        public string MtoInd { get; set; }
        public string SelfCollectInd { get; set; }
        public string PremiumServiceInd { get; set; }
        public string CraneBookedInd { get; set; }
        public string BargeBookedInd { get; set; }
        public string PoliceEscortInd { get; set; }
        public string CompanyCode { get; set; }
        public int NoOfItems { get; set; }
        public string IntRemark { get; set; }
        public string ExtRemark { get; set; }
        public string Remarks { get; set; }
        public string OrderGroupId { get; set; }
        public bool OnHoldInd { get; set; }
        public DateTime? FirstPromisedDate { get; set; }
        public bool ContractualLeadtimeInd { get; set; }
        public DateTime? TimeStampUpdate { get; set; }
        public DateTime? TimeStampInsert { get; set; }
        public bool NeedsLorryCraneInd { get; set; }
        public bool ZeroToleranceInd { get; set; }
        public bool CallBefDelInd { get; set; }
        public bool ConquasInd { get; set; }
        public bool SpecialPassInd { get; set; }
        public bool DoNotMixInd { get; set; }
        public int GroupMemberCount { get; set; }
        public string LowBedVehicleAllowed { get; set; }
        public string FiftyTonVehicleAllowed { get; set; }
        public bool HeightControlInd { get; set; }
        public string PhysicalProjNo { get; set; }
        public string PhysicalDeliveryProjNo { get; set; }
        public string Wbs1 { get; set; }
        public string Wbs2 { get; set; }
        public string Wbs3 { get; set; }
        public string Bbs { get; set; }
        public string StElement { get; set; }
        public bool VariousBar { get; set; }
        public string Spare1 { get; set; }
        public string Spare2 { get; set; }
        public string Spare3 { get; set; }
        public string Spare4 { get; set; }
        public string Spare5 { get; set; }
    }

    public class Item
    {
        public string Tag { get; set; }
        public string Part { get; set; }
        public string Collection { get; set; }
        public int? AggregationCode { get; set; }
        public string BvbsString { get; set; }
        public bool Replaced { get; set; }
        public string ExtraInfo { get; set; }
        public int Position { get; set; }
        public int Diameter { get; set; }
        public int Quantity { get; set; }
        public int Length { get; set; }
        public int? MachineId { get; set; }
        public int BundleTypeId { get; set; }
        public string LoadNo { get; set; }
        public int LayerNo { get; set; }

        public ExtendedItem Extended { get; set; }
    }

    public class ExtendedItem
    {
        public string Cliente { get; set; }
        public string OrderNo { get; set; }
        public string ItemNo { get; set; }
        public string ParentItemNo { get; set; }
        public string SapOrderItem { get; set; }
        public string MaterialNo { get; set; }
        public string ProductHierarchy { get; set; }
        public int OrderQty { get; set; }
        public string SalesUom { get; set; }
        public float FgProductionWtKg { get; set; }
        public float ComponentProductionWtKg { get; set; }
        public string ProductMarking { get; set; }
        public string ComponentPrefix { get; set; }
        public int OrderPieces { get; set; }
        public int DeliveredPieces { get; set; }
        public DateTime? ActualDelDate { get; set; }
        public string DeliveryStatus { get; set; }
        public string CancellationStatus { get; set; }
        public int? EnvLength { get; set; }
        public int? EnvWidth { get; set; }
        public int? EnvHeight { get; set; }
        public string Wbs1 { get; set; }
        public string Wbs2 { get; set; }
        public string Wbs3 { get; set; }
        public string Wbs4 { get; set; }
        public string Wbs5 { get; set; }
        public string ProjStage { get; set; }
        public string BbsNo { get; set; }
        public string BbsDesc { get; set; }
        public string NeedsGalvanizingInd { get; set; }
        public string NeedsProductionInd { get; set; }
        public int CabSpiralPitch { get; set; }
        public int CabFleetPitch { get; set; }
        public string CabBarGrade { get; set; }
        public int CabDia { get; set; }
        public string CabBvbs { get; set; }
        public int CabCutLength { get; set; }
        public int CabCouplerEndNo { get; set; }
        public int CabFormerSize { get; set; }
        public string CabShapeGroup { get; set; }
        public string ShapeCode { get; set; }
        public string CabCouEndType1 { get; set; }
        public string CabCouEndType2 { get; set; }
        public int CabNoOfBend { get; set; }
        public string Remarks { get; set; }
        public string StElement { get; set; }
        public int QtyInBaseUom { get; set; }
        public string BaseUom { get; set; }
        public float DeliveryQty { get; set; }
        public int CabMainBarAngle { get; set; }
        public int InvLenInMm { get; set; }
        public string GradeDia { get; set; }
        public string SapProductGroup { get; set; }
        public int CabStandardEndNo { get; set; }
        public int CabFormerType { get; set; }
        public string ProductType { get; set; }
        public int CabExtendedEndNo { get; set; }
        public int CabStudEndNo { get; set; }
        public int BbsPageNo { get; set; }
        public int IdenticalHashcode { get; set; }
        public int SimilarMainbarHashcode { get; set; }
        public int SimilarExtlinkHashcode { get; set; }
        public string BendingHr { get; set; }
        public string Spare1 { get; set; }
        public string Spare2 { get; set; }
        public string Spare3 { get; set; }
        public string Spare4 { get; set; }
        public string Spare5 { get; set; }
    }

}