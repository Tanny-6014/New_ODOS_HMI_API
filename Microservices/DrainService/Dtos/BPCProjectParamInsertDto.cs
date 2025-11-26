namespace DrainService.Dtos
{
    public class BPCProjectParamInsertDto
    {
        public int tntParamSetNumber {get;set;}
        public int intProjectId {get;set;}
        public int sitProductTypeL2Id{get;set;}
        public int tntTransportModeId{get;set;}
        public int intLapLength {get;set;}
        public int intEndLength {get;set;}
        public int intAdjFactor {get;set;}
        public int intCoverToLink {get;set;}
        public int tntStatusId {get;set;}
        public int intParameterSet{ get;set;}
        public int vchParameterType { get; set; }                 
        public bool bitStructureMarkingLevel { get; set; }
        public int intuserid { get;set;}
        public string vchDescription { get;set;}     
    }
}
