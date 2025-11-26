namespace ShapeCodeService.Dtos
{
    public class InsUpdShapeHeaderDto
    {
        //public int ShapeId{get; set;}
        public string ShapeDescription{get; set;}
        public string MeshGroup{get; set;}
        public string MOCO{get; set;}
        public int NoOfBends{get; set;}
        public string BendingGroup {get; set;}
        public string MWBendingGroup{get; set;}
        public string CWBendingGroup{get; set;}
        public int NoOfSegments{get; set;}
        public int NoOfParameters{get; set;}

        public int NoOfCuts{get; set;}
        public string  Image{get; set;}
        public string  ImagePath{get; set;}
        public int  NoOfRoll{get; set;}
        public string  ShapeType{get; set;}
        public bool BendIndicator{get; set;}
        
        public bool BendType{get; set;}
        public bool CreepMO1{get; set;}
        public bool CreepCO1{get; set;}
        public int StatusId{get; set;}
        public string CWTemplate{get; set;}
        public string MWTemplate{get; set;}
        public int EvenMO1{get; set;}
        public int EvenMO2{get; set;}
        public int OddMO1{get; set;}
        public int OddMO2{get; set;}
        public int  EvenCO1{get; set;}
        public int  EvenCO2{get; set;}
        public int OddCO1{get; set;}
        public int OddCO2{get; set;}
        public bool OHIndicator{get; set;}
        //public bool UserId{get; set;}
    }
}
