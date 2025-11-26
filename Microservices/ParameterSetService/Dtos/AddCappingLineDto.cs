namespace ParameterSetService.Dtos
{
    public class AddCappingLineDto
    {
        public int? tntParamCageId { get; set; }                         
        public int? tntParamSetNumber { get; set; } 
        public int intDiameter { get; set; }
        public int sitLeg { get; set; }
        public int? Hook { get; set; }
        public int Length { get; set; }
        public string chrStandard { get; set; }

        public int CappingProductId { get; set; }
    }
}
