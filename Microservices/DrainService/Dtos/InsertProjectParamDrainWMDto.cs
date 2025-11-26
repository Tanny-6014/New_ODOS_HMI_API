namespace DrainService.Dtos
{
    public class InsertProjectParamDrainWMDto
    {
        public int intDrainWMId { get; set; }
        public int tntParamSetNumber { get; set; }
        public int sitDrainTypeId { get; set; }
        public int tntDrainLayerId { get; set; }
        public int sitDrainWidth { get; set; }
        public int sitMaxDepth { get; set; }
        public int intDrainDepthParamId { get; set; }
        public int intProductCodeId { get; set; }
        public int intShapeId { get; set; }
        public int intParamA { get; set; }
        public int numLeftWallThickness { get; set; }//string to int
        public int numLeftWallFactor { get; set; }
        public int numRightWallThickness { get; set; }
        public int numRightWallFactor { get; set; }
        public int numBaseThickness { get; set; }
        public int intQty { get; set; }
        public bool bitDetail { get; set; }
        public int intUserId { get; set; }

      

       
    }
}
  