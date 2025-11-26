namespace ProductCodeMasterService.Dtos
{
    public class AddRawMaterialDto
    {  
        public int? ProductCodeId { get; set; }
        public string? ProductCode { get; set; }
        public string? ProductDescription { get; set; }
        public int? RawMaterial { get; set; }
        public int? MaterialType { get; set; }
        public decimal? Diameter { get; set; }
        public string? MaterialAbbr { get; set; }
        public int? Grade { get; set; }
        public decimal? WeightRun { get; set; }
        public int? StatusId { get; set; }
        public int? UserId { get; set; }
        public int? SapMaterialId { get; set; }
    }
}
