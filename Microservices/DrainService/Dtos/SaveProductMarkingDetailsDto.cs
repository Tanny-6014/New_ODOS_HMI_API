namespace DrainService.Dtos
{
    public class SaveProductMarkingDetailsDto
    {
        public decimal drainOuterCrossWire { get; set; }
        public decimal drainInnerCrossWire{ get; set; }
        public decimal drainSlabCrossWire{ get; set; }
        public decimal drainBaseCrossWire{ get; set; }
        public bool IsCascade{ get; set; }      
        public int intCascadeNo{ get; set; }
        public decimal flCsCrossLength{ get; set; }
        public decimal flCsDropHeight{ get; set; }
        public decimal flCsWidth { get; set; }
        public decimal startChainage { get; set; }
        public decimal endChainage { get; set; }
        public decimal startTopLevel { get; set; }
        public decimal endTopLevel { get; set; }
        public decimal startInvertLevel { get; set; }
        public decimal endInvertLevel { get; set; }
        public string structMarkingName { get; set; }
        public string txtMemQty { get; set; }
        public decimal startDepth { get; set; }
        public decimal endDepth { get; set; }
        public int tntStructureRevNo { get; set; }
        public string vchGroupMarkName { get; set; }
        public int intDrainStructureMarkId { get; set; }
        public int intParameterSet { get; set; }
        public bool bitTransportChk { get; set; }
        public bool bitBendingChk { get; set; }
        public bool bitProduceIndicator { get; set; }
        public bool bitMachineChk { get; set; }
        public int intUserID { get; set; }

        public int drainTopCover     { get; set; }
        public int drainBottomCover  { get; set; }
        public int drainOuterCover   { get; set; }
        public int drainInnerCover   { get; set; }





    }
}

