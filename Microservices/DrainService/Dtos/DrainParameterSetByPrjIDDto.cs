namespace DrainService.Dtos
{
    public class DrainParameterSetByPrjIDDto
    {
        public int tntParamSetNumber { get; set; }
        public int intProjectId { get; set; }
        public int sitProductTypeL2Id { get; set; }
        public int intParameterSet { get; set; }
        public string vchProjectAbbr { get; set; }
        public string vchDescription { get; set; }
        public int tntTransportModeId { get; set; }
        public int sitTopCover { get; set; }
        public int sitBottomCover { get; set; }
        public int sitInnerCover { get; set; }
        public int sitOuterCover { get; set; }
        public int tntStatusId { get; set; }
        public int intCreatedUID { get; set; }
        public DateTime datCreatedDate { get; set; }
        public int intUpdatedUID { get; set; }
        public DateTime datUpdatedDate { get; set; }

    }
}
// 														