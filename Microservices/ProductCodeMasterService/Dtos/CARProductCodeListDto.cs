namespace ProductCodeMasterService.Dtos
{
    public class CARProductCodeListDto
    {
      public int ProductCodeId { get; set; }
      public string ProductCode { get; set; }
      public string ProductDescription { get; set; }
      public string WeightArea { get; set; }
      public int SAPMaterialCodeId { get; set; }
      public string Status { get; set; }
      public int StatusId { get; set; }
      public int ProductTypeId { get; set; }
      public string ProductType { get; set; }

     public int StructureElementID { get; set; }
      public string StructureElementType  { get; set; }
    }
}
