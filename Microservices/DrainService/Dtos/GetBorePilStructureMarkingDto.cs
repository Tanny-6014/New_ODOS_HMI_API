namespace DrainService.Dtos
{
    public class GetBorePilStructureMarkingDto
    {
        public int intStructureMarkId { get; set; } 
        public int intSEDetailingId { get; set; }
        public string vchStructureMarkingName { get; set; }
        public string structcode { get; set; }
        public string structlastthreecode { get; set; }
        public int tntStructureRevNo { get; set; }
        public int intGroupMarkId { get; set; }
        public int tntGroupRevNo { get; set; }   
        public int tntParamSetNumber { get; set; }
        public int tntCageSeqNo { get; set; }
        public int intNoofWeldPoints { get; set; }
        public int intShenellTemplateId { get; set; }
        public string vchShenellAlternateTemplates { get; set; }
        public int numPileDia { get; set; }
        public int numCageLength { get; set; }
        public int numCageDia { get; set; }
        public int numCageWeight { get; set; }
        public string vchFabricationType { get; set; }
        public string vchMainBarPattern { get; set; }
        public string vchElevationPattern { get; set; }
        public int tntNumOfMainBar { get; set; }
        public int tntNumOfLinksatStart { get; set; }
        public int tntNumOfLinksatEnd { get; set; }
        public int tntAdditionalSpiral { get; set; }
        public int tntNumberOfStiffnerOrCentralizer { get; set; }
        public int tntNumOfSquareStiffner { get; set; }
        public int tntNumOfCircularRings { get; set; }
        public bool bitCageNoteChangeIndicator { get; set; }
        public int tntCageNoteId { get; set; }
        public string vchCageNote { get; set; }
        public int MachineTypeId { get; set; }
        public int intMemberQty { get; set; }
        public int numNetWeight { get; set; }
        public int numActualWeight { get; set; } 
        public string vchDrawingReference { get; set; }
        public string chrDrawingVersion{ get; set; } 
        public string vchDrawingRemarks { get; set; }
        public string vchTactonConfigurationState { get; set; }
        public string vchHoleConfigurationXML { get; set; }
        public string vchTactonXmlResult { get; set; }
        public bool bitAssemblyIndicator { get; set; }
        public bool bitGenerate3DforPreview { get; set; }
        public string nvchEdwgPath { get; set; }
        public string chrGeneration3DStatus { get; set; }
        public string vchOutputDrawingRef { get; set; }
        public bool bitCoat { get; set; }
        public int tntStatusId { get; set; }
        public int intCreatedUId { get; set; }
        public DateTime datCreatedDate { get; set; }
        public int intUploadedUId { get; set; }
        public DateTime datUpdatedDate { get; set; }
        public string vchDescription{ get; set; } 
        public int intParameteSet { get; set; } 
        public int intLapLength { get; set; } 
        public int intEndLength { get; set; } 
        public int intCoverToLink { get; set; }
        public int intAjustmentFactor { get; set; }
        public string vchMainBarDia { get; set; }
        public string vchLinkDia { get; set; }
        public string vchLinkPitch { get; set; }
        public string vchPDFReference { get; set; }

        public string Indicator { get; set; }

    }
}
