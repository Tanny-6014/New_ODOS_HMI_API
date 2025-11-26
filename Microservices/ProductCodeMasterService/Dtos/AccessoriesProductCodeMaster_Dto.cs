namespace ProductCodeMasterService.Dtos
{
    public class AccessoriesProductCodeMaster_Dto
    {
		public int AccessoriesProductCodeID { get; set; }
		public string? ProductCode { get; set; }
		public string? Description { get; set; }
		public int? ProductType { get; set; }
		public string? GradeType { get; set; }
		public int? GradeId { get; set; }
		public int? Diameter { get; set; }
		public int? StdId { get; set; }
		
		public int? FGSAPMaterialID { get; set; }
		//public int? RMSAPMaterialID { get; set; }
		public string? FG_SAPMaterialCode { get; set; }
		//public string? RM_SAPMaterialCode { get; set; }
		
		public double? WeightPerMRun { get; set; }
		public int? BaseUOM { get; set; }
		public int? SalesUOM { get; set; }
		public bool? BPCItem { get; set; }
		public int? StatusId { get; set; }
		public string? CreatedUser { get; set; }
		public string? UpdatedUser { get; set; }
		public int? CreatedUId { get; set; }
		public DateTime? CreatedDate { get; set; }
		public int? UpdatedUId { get; set; }
		public DateTime? UpdatedDate { get; set; }
	}
}
