namespace DrainService.Dtos
{
    public class GetBPElevationPatternDetails
    {
        public int intElevationPatternID { get; set; }
        public string vchElevationCode { get; set; }
        public string vchDescription { get; set; }
        public int tntNoOfSpiralLinkSize { get; set; }
        public int tntNoOfSpiralLinkPitch { get; set; }
        public int tntNoOfSpiralLinkLength { get; set; }
        public string vchElevationImage { get; set; }
        public string vchImagePath { get; set; }
    }
}
