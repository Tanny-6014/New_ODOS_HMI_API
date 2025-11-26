namespace ProductCodeMasterService.Dtos
{
    public class GetRawMaterialList
    {
        public int? ProductCodeId { get; set; }
        public string? ProductCode { get; set; }
        public string? ProductDescription { get; set; }
        public bool? RawMaterial { get; set; }
        public int? ProductTypeId { get; set; }
        public string? ProductType { get; set; }
        public string MaterialType { get; set; }
        public decimal? Diameter { get; set; }
        public string? MaterialAbbr { get; set; }
        public string? Grade { get; set; }
        public decimal? WeightRun { get; set; }
        public int? SAPMaterialCodeId { get; set; }
        public int? StatusId { get; set; }
        public int? UserId { get; set; }
       
    }
}
