namespace ProductCodeMasterService.Dtos
{
    public class MeshGetListDto
    {
         

        public int? ProductCodeId { get; set; }
        public string? ProductCode { get; set; }
        public string? ProductDescription { get; set; }
        public string? MainWire_Productcode { get; set; }
        public string? CrossWire_Productcode { get; set; }     
        public int? MWSpace { get; set; }
        public int? CWSpace { get; set; }
        public int? ProductTypeId { get; set; }
        public string? ProductType { get; set; }
        public decimal? WeightRun { get; set; }
        public string? TwinIndicator { get; set; }
        public int? StatusId { get; set; }
        public string? StructureElementType { get; set; }
        




    }
}
