namespace DetailingService.Dtos
{
    public class BeamShapeCodeParameterSetDto
    {
        public int INTPARAMETESET { get; set; }
        public int TNTPARAMSETNUMBER { get; set; }
        public int TNTTRANSPORTMODEID { get; set; }
      
        public int SITGAP1 { get; set; }
        public int SITGAP2 { get; set; }
        public int SITTOPCOVER { get; set; }
        public int SITBOTTOMCOVER { get; set; }
        public int SITLEFTCOVER { get; set; }
        public int SITRIGHTCOVER { get; set; }
        public int SITHOOK { get; set; }
        public int SITLEG { get; set; }
        public string CHRSTANDARDCP { get; set; }
        public string VCHTRANSPORTMODE { get; set; }
        public string VCHTRANSPORTDESCRIPTION { get; set; }
    }
}
