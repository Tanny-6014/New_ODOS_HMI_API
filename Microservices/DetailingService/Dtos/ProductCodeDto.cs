namespace DetailingService.Dtos
{
    public class ProductCodeDto
    {
        public int ProductCodeId { get; set; }
        public string ProductCodeName { get; set; }
        public int StructureElementTypeId { get; set; }
        public int ProductTypeId { get; set; }
        public int MainWireDia { get; set; }
        public int MainWireSpacing { get; set; }
        public int CrossWireSpacing { get; set; }
        public decimal WeightArea { get; set; }
        public decimal WeightPerMeterRun { get; set; }
        public int MinLinkFactor { get; set; }
        public int MaxLinkFactor { get; set; }
        public double CwDia { get; set; }
        public decimal CwWeightPerMeterRun { get; set; }
        public double DECMWLength { get; set; }

    }
}
