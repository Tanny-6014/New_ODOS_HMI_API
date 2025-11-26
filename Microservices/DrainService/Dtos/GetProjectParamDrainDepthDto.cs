namespace DrainService.Dtos
{
    public class GetProjectParamDrainDepthDto
    {
        public int intDrainDepthParamId { get; set; }
        public int tntParamSetNumber { get; set; }
        public string vchDrainType { get; set; }
        public int sitDrainWidth { get; set; }
        public int sitAdjust { get; set; }
        public int sitChannel { get; set; }
        public int sitSlabThickness { get; set; }
        public int sitMaxDepth1 { get; set; }
        public int sitMaxDepth2 { get; set; }
        public int sitMaxDepth3 { get; set; }
        public int sitMaxDepth4 { get; set; }
        public int sitMaxDepth5 { get; set; }
        
        //added for insert
        public bool bitConfirm { get; set; }
        public int intUserId { get; set; }
        public int sitDrainTypeId { get; set; }
    }
}