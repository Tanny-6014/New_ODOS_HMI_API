namespace ProductCodeMasterService.Dtos
{
    public class MeshListByIdDto
    {
        public int? ProductCodeId { get; set; }
        public string? ProductCode { get; set; }
        public string? ProductDescription { get; set; }
        public int ProductTypeId { get; set; }
        public int StatusId { get; set; }
        public int? RawMaterial { get; set; }

        public int? SAPMaterialCodeId { get; set; }
        public int? CWProductCodeId { get; set; }
        public int? MWProductCodeId { get; set; }
        public int? MWSpace { get; set; }
        public int? CWSpace { get; set; }
        public decimal? WeightRun { get; set; }
        public int? CWMaterialType { get; set; }
        public int? MWMaterialType { get; set; }
        public decimal? CWDiameter { get; set; }
        public decimal? MWDiameter { get; set; }
        public decimal? CWWeightRun { get; set; }
        public decimal? MWWeightRun { get; set; }

        public string? CWGrade { get; set; }
        public string? MWGrade { get; set; }
        public string? StructureId { get; set; }
        public string TwinIndicator { get; set; }
        public int MinLinkFactor { get; set; }
        public int MaxLinkFactor { get; set; }

       



    }
}
