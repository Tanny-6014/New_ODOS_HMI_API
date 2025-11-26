namespace ParameterSetService.Dtos
{
    public class CappingLineDto
    {
        public int? ProjectID { get; set; }                       
        public int? TNTPARAMSETNUMBER { get; set; }                         
        public int? INTPARAMETESET { get; set; }                        
        public int? TNTPARAMCAGEID { get; set; } 
        
        public int ParameterSetNo { get; set; }
        public int Diameter { get; set; }
        public int Leg { get; set; }
        public int? Hook { get; set; }
        public int? ProductCodeId { get; set; }
        public string? ProductCode { get; set; }
        public int CWLength { get; set; }
        public string CHRSTANDARD { get; set; }
    }
}
