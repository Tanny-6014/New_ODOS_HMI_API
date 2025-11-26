namespace ProductCodeMasterService.Dtos
{
    public class ProductCodeCommonListDto
    {
        //ProductCodeID	ProductCode	Description	ProductTypeId	ProductType	StructureElementID	StructureElementType	statusId
        public int ProductCodeID { get; set; }
        public string? ProductCode { get; set; }
        public string? Description { get; set; }
        public int? ProductTypeId { get; set; }

        public string? ProductType { get; set; }

        public int? StructureElementID { get; set; }
        public string? StructureElementType { get; set; }
        public int? statusId { get; set; }
    }
}
