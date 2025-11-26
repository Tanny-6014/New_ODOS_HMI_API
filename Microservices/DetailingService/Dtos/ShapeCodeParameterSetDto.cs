namespace DetailingService.Dtos
{
    public class ShapeCodeParameterSetDto
    {
        public int TNTPARAMSETNUMBER { get; set; }
        public int? INTPARAMETESET { get; set; }
        public string? VCHDESCRIPTION { get; set; }
        public int? TNTTRANSPORTMODEID { get; set; }
        public int? MaxMWLength { get; set; }
        public int? MaxCWLength { get; set; }
        public int MACHINEMAXMWLENGTH { get; set; }
        public int MACHINEMAXCWLENGTH { get; set; }
        public int? MINMWLENGTH { get; set; }
        public int? MINCWLENGTH { get; set; }
        public int? MINMO1 { get; set; }
        public int? MINMO2 { get; set; }
        public int? MINCO1 { get; set; }
        public int? MINCO2 { get; set; }
        
        public int TransportMaxLength { get; set; }
        public int TransportMaxWidth { get; set; }
        public int TransportMaxHeight { get; set; }
        public int TransportMaxWeight { get; set; }


        public ShapeCodeParameterSetDto() {
            INTPARAMETESET = 14;

            MINCO1 = 10;
            MINCO2 = 10;
            MINMO1 = 15;
            MINMO2 = 15;
            TNTPARAMSETNUMBER = 7007;
            TNTTRANSPORTMODEID = 8;
            VCHDESCRIPTION = "OLD AND TME";


        }
             
    }
}
                                
                               
                                
                                
                            