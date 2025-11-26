namespace ShapeCodeService.Dtos
{
    public class ChkShapeExistDto
    {
        public int intShapeID { get; set; }
        //intShapeCodeIdNotUsed   
        //tntShapeCodeRev 
        public string chrShapeCode { get; set; }
        //vchMWShapeId 
        //vchCWShapeId    
        public string vchMeshShapeGroup { get; set; }
        public string vchBendingGroup { get; set; }
        public string vchMWBendingGroup { get; set; }
        public string vchCWBendingGroup { get; set; }
        public int intNoOfSegments { get; set; }
        public int intNoOfParameters { get; set; }
        public int sitNoOfBends { get; set; }
        public int sitNoOfCut { get; set; }
        public string vchImage { get; set; }
        public string vchImagePath { get; set; }
        public int sitNoOfRoll { get; set; }
        public string chrShapeType { get; set; }
        public int intMWShapeId { get; set; }
        //tntMWShapeRevNo 
        public int intCWShapeId { get; set; }
        //tntCWShapeRevNo 
        public int sitEvenMO1 { get; set; }
        public int sitEvenMO2 { get; set; }
        public int sitOddMO1 { get; set; }
        public int sitOddMO2 { get; set; }
        public int sitEvenCO1 { get; set; }
        public int sitEvenCO2 { get; set; }
        public int sitOddCO1 { get; set; }
        public int sitOddCO2 { get; set; }
        public string bitBendIndicator { get; set; }
        //vchStartDirection   
        //intStartCoordX 
        //intStartCoordY  
        //intStartCoordZ 
        public string chrBendPosition { get; set; }
        public string chrCheckLength { get; set; }
        //chrCutSequence  
        //vchWidthFormula 
        //vchLengthFormula    
        //vchHeightFormula 
        //sitCatalog  
        //intWireSpecId 
        //tntWireShapeRev 
        //vchWireBendingPosition 
        public string bitBendSeqIndicator { get; set; }
        public string bitBendType { get; set; }
        public string bitCreepDeductAtMo1 { get; set; }
        public string bitCreepDeductAtCo1 { get; set; }
        public string bitVerifyIndicator { get; set; }
        //vchVerifyFormula 
        public int tntStatusId { get; set; }
        public string vchCwBVBSTemplate { get; set; }
        public string vchMwBVBSTemplate { get; set; }
        //bitThreeDIndicator 
        public string chrMOCO { get; set; }
        public string bitDefaultOHIndicator { get; set; }
        //intCreatedUID   
        //datCreatedDate 
        //intUpdatedUID   
        //datUpdatedDate 
        //chrVersion  
        //bitShapePopUp BitIsCappingDefault INTMWBENDPOSITION INTCWBENDPOSITION   INTNOOFMWBEND INTNOOFCWBEND   bitEdit vchDescription  vchProdLengthFormula bitProdLength   intNO_OF_COUPLER vchCOUPLER_TYPE_START   vchCOUPLER_TYPE_END vchInvLengthFormula bitInvLength bitEnvFormula   vchEnvLengthFormula vchEnvWidthFormula  vchEnvHeightFormula
    }
}