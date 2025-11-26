namespace ShapeCodeService.Dtos
{
    public class addShapeGrpDetailDto
    {
        public string ShapeID { get; set; }
        public string ShapeGroup { get; set; }
        public int Coupler { get; set; }
        public int Stud { get; set; }
        public int Thread { get; set; }
        public int Locknut { get; set; }
        public string ShapePrefix { get; set; }
        public string CouplerType { get; set; }
        public int Flag { get; set; }
    }
}
