namespace DrainService.Dtos
{
    public class DrainShapeCodeDto
    {
        public int intDrainParamDetailsId { get; set; }
        public string chrSegmentParameter { get; set; }
        public int intSegmentValue { get; set; }
        public string chrShapeCode { get; set; }
        public bool bitEdit { get; set; }
        public string chrAngleType { get; set;}
        public bool bitVisible { get; set; }
        public int Sequence { get; set; }
        public string CriticalIndicator { get; set; }
        public int intMo1 { get; set; }
        public int intMo2 { get; set; }

    }
}
