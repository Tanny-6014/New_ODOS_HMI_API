namespace DrainService.Dtos
{
    public class GetProjectParamDrainWMDto
    {
        public int? intDrainWMId { get;set;}
        public int tntParamSetNumber { get;set;}
        public string vchDrainType { get; set;}
        public string vchDrainLayer { get; set;}
        public int intDrainDepthParamId { get; set;}
        public int sitDrainWidth { get; set;}
        public int sitMaxDepth { get; set;}
        public string vchProductCode{ get; set;}
        public string chrShapeCode { get; set; }
        public int numLeftWallThickness { get; set; }
        public int numLeftWallFactor { get; set; }
        public int numRightWallThickness { get; set; }
        public int numRightWallFactor { get; set; }
        public int numBaseThickness { get; set; }
        public int numMainLength { get; set; }
        public int intQty { get; set; }
        public bool bitPreDefined { get; set; }
        public int intShapeId { get; set; }
        public int intProductCodeId { get; set; }
        public int intParamA { get; set; }

        public int sitDrainTypeId { get; set; }
        public int tntDrainLayerId { get; set; }
        public bool bitDetail { get; set; }
        public int intUserId { get; set; }
    }
}


//numLeftWallThickness//var   
//numLeftWallFactor //VARCHAR(100)
//numRightWallThickness// VARCHAR(100)
//numRightWallFactor// VARCHAR(100)
//numBaseThickness //VARCHAR(100)
