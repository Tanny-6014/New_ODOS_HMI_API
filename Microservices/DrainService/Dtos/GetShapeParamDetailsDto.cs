namespace DrainService.Dtos
{
    public class GetShapeParamDetailsDto
    {
        public int intShapeDetailId { get; set; }
        public int intShapeID { get; set; }
        public string chrParamName { get; set; }
        public int intParamSeq { get; set; }
        public string vchMWShape { get; set; }
        public string vchCWShape { get; set; }
        public string chrWireType { get; set; }
        public string chrAngleType { get; set; }
        public int intAngleDir { get; set; }
        public int intBendSeq1 { get; set; }
        public int intBendSeq2 { get; set; }
        public string chrCriticalInd { get; set; }
        public int intMinLen { get; set; }
        public int intMaxLen { get; set; }
        public int intConstValue { get; set; }

    }
}

 													
