namespace ShapeCodeService.Dtos
{
    public class UpdateShapeParamDto
    {
        public string StrShapeID{get;set;} 
        public Int32 ParameterSeq{get;set;} 
        public string ParameterName{get;set;} 
        public Int32 XCoor{get;set;} 
        public Int32 YCoor{get;set;} 
        public Int32 ZCoor{get;set;} 
        public bool Visible{get;set;} 
        public string symmetricTo{get;set;} 
        public string Formula{get;set;} 
        public string HAFormula{get;set;} 
        public string OAFormula{get;set;} 
        public Int32 Range { get; set; }
    }
}
