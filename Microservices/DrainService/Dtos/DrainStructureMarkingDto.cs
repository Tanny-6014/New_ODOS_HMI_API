namespace DrainService.Dtos
{
    public class DrainStructureMarkingDto
    {
        public int intStructureMarkId { get; set; }
        public int intSEDetailingID { get; set; }
        public int tntStructureRevNo { get; set; }
        public string vchStructureMarkingName { get; set; }
        public int tntParamSetNumber { get; set; }
        public decimal decStartChainage { get; set; }
        public decimal decEndChainage { get; set; }
        public decimal decDistance { get; set; }
        public decimal decStartTopLevel { get; set; }
        public decimal decEndTopLevel { get; set; }
        public decimal decStartInvertLevel { get; set; }
        public decimal decEndInvertLevel { get; set; }
        public decimal decStartHeight { get; set; }
        public decimal decEndHeight { get; set; }
        public decimal decStartDepth { get; set; }
        public decimal decEndDepth { get; set; }
        public int intCascadeNo { get; set; }
        public decimal decCascadeDropHeight { get; set; }
        public decimal decCascadeWidth { get; set; }
        public decimal decCascadeCWLength { get; set; }
        public bool bitCascade { get; set; }
        public bool bitCoatingIndicator { get; set; }
        public bool bitBendingCheck { get; set; }
        public bool bitMachineCheck { get; set; }
        public bool bitTransportCheck { get; set; }
        public string vchDrawingReference { get; set; }
        public string chrDrawingVersion { get; set; }
        public string vchDrawingRemarks { get; set; }
        public bool bitAssemblyIndicator { get; set; }
        public bool bitProduceIndicator { get; set; }
        public int intArmaid { get; set; }
        public string CascadeBit { get; set; }
        public int intParameterSet { get; set; }
        public bool bitCrossLenInner { get; set; }
        public bool bitCrossLenOuter { get; set; }
        public bool bitCrossLenSlab { get; set; }
        public bool bitCrossLenBase { get; set; }
        public decimal decCrossLenInner { get; set; }
        public decimal decCrossLenOuter { get; set; }
        public decimal decCrossLenSlab { get; set; }
        public decimal decCrossLenBase { get; set; }
        public int intMemberQty { get; set; }
    }
}
 														