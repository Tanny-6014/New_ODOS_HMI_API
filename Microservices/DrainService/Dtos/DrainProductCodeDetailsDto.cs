namespace DrainService.Dtos
{
    public class DrainProductCodeDetailsDto
    {
        public int intProductCodeId { get; set; }
        public string vchProductCode { get; set; }
        public int intCWSpace { get; set; }
        public int intMWSpace { get; set; }
        
        //added for OthProductCode_Get
        public decimal decMWDiameter { get; set; }
        public decimal decCWDiameter { get; set; }

    }
}
 		
