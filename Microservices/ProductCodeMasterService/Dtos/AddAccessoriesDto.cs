namespace ProductCodeMasterService.Dtos
{
    public class AddAccessoriesDto
    {
		public int? AccessoriesProductCodeId { get; set; }
		public string? GradeType { get; set; }
		public int? Diameter { get; set; }
		public int? FGSAPMaterialID { get; set; }
		public int? RMSAPMaterialID { get; set; }
		public int? StatusId { get; set; }
		public int? UserId { get; set; }
		public string? ProductCode { get; set; }
		public string? Description { get; set; }
		public int? ProductType { get; set; }
	}
}
