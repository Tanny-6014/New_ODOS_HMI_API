namespace DrainService.Dtos
{
    public class DrainParamDepthValues_GetNewDto
    {
        public int sitAdjust { get; set; }
        public int sitChannel { get; set; }
        public int sitSlabThickness { get; set; }
        public int sitMaxDepth1 { get; set; }
        public int sitMaxDepth2 { get; set; }
        public int sitMaxDepth3 { get; set; }
        public int sitMaxDepth4 { get; set; }
        public int sitMaxDepth5 { get; set; }
        public List<string> vchDrainLayer { get; set;}
    }
}
