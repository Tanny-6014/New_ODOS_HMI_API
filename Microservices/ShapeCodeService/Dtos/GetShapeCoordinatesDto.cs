namespace ShapeCodeService.Dtos
{
    public class GetShapeCoordinatesDto
    {
        public string VERSION { get; set; }
        public int COORDX { get; set; }
        public int COORDY { get; set; }
        public int COORDZ { get; set; }
        public int ACTIVE { get; set; }
        public int INTPARAMSEQ { get; set; }
        public string CHRPARAMNAME { get; set; }
        public int INTXCOORD { get; set; }
        public int INTYCOORD { get; set; }
        public int INTZCOORD { get; set; }
        public string VISIBLE { get; set; }
        public string SYMMETRICTO { get; set; }
        public string FORMULA { get; set; }
        public string HEIGHTANGLEFORMULA { get; set; }
        public string OFFSETANGLEFORMULA { get; set; }
        public int RoundOffRANGE { get; set; }
        public int DEFAULTPARAMVALUE { get; set; }
    }
}
