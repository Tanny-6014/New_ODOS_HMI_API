namespace ProductCodeMasterService.Dtos
{
    public class CARProductCodeMasterDto
    {
		public int? ProductCodeId { get; set; }
		public string? ProductCode { get; set; }
		public string? ProductDescription { get; set; }
		public int? SAPMaterialCodeId { get; set; }
		public decimal? WeightArea { get; set; }
		public string? BaseUOM { get; set; }
		public string? SalesUOM { get; set; }
		public int? StatusId { get; set; }
		public int? CreatedUID { get; set; }
		public DateTime? CreatedDate { get; set; }
		public int? UpdatedUID { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public decimal? ConvertionFactor { get; set; }
		public int? ApprovalId { get; set; }
		public DateTime? ApprovedDate { get; set; }

	}
}
