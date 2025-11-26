namespace ProductCodeMasterService.Dtos
{
    public class AddMeshDto
    {
      public int ProductCodeId { get; set; }
      public string? ProductCode { get; set; }
      public string? ProductDescription { get; set; }
      public bool? RawMaterial { get; set; }
      public int? MWProductCodeId { get; set; }
      public int? MWMaterialType { get; set; }
      public decimal? MWDiameter { get; set; }
      public string? MWMaterialAbbr { get; set; }
      public int? MWSpace { get; set; }
      public string? MWGrade { get; set; }
      public string? MWLength { get; set; }
      public string? MWMaxBendLen { get; set; }
      public decimal? MWWeightRun { get; set; }
      public int? CWProductcodeId { get; set; }
      public int? CWMaterialType { get; set; }
      public decimal? CWDiameter { get; set; }
      public string? CWMaterialAbbr { get; set; }
      public int? CWSpace { get; set; }
      public string? CWGrade { get; set; }
      public string? CWLength { get; set; }
      public string? CWMaxBendLen { get; set; }
      public decimal? CWWeightRun { get; set; }
      public decimal? WeightSqm { get; set; }
      public decimal? ConversionFactor { get; set; }
      public string? TwinInd { get; set; }
      public bool? StaggeredInd { get; set; }
      public bool? BendInd { get; set; }
      public string? BomInd { get; set; }
      public int? MinLink { get; set; }
      public int? MaxLink { get; set; }
      public int? SapMaterial { get; set; }
      public int? ProductTypeID { get; set; }
      public string? StructureElement { get; set; }
        
      public int? StatusId { get; set; }
      public string? WireType { get; set; }
      public int? ShapeCodeId { get; set; }
      public int? UserId { get; set; }
      

    }
}
  