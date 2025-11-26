namespace ProductCodeMasterService.Dtos
{
    public class AddCoreCageDto
    {
        public int? ProductCodeId { get; set; }
        public string? ProductCode { get; set; }
        public string? ProductDescription { get; set; }
        public int? SapMaterial { get; set; }
        public int? StatusId { get; set; }
        public decimal? WeightSqm { get; set; }       
        public int? StructureElement { get; set; }
        public int? ProductType { get; set; }
        public int? UserId { get; set; }
        
    }
}
