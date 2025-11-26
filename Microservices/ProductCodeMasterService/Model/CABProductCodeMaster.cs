namespace ProductCodeMasterService.Model
{
    public class CABProductCodeMaster
	{
        public int intCABProductCodeID { get; set; }
        public string? chrGradeType { get; set; }
		public int? intGrade { get; set; }
		public int? intDiameter { get; set; }
		public int? intStdID { get; set; }
		public string? vchDescription { get; set; }
		public bool? bitCouplerIndicator { get; set; }
        public string? vchCouplerType { get; set; }
		public int? intCoupGradeID { get; set; }
		public int? intCouplStdID { get; set; }
		public int? intCoupDiameter { get; set; }
		public int? intFGSAPMaterialID { get; set; }
        public int? intRMSAPMaterialID { get; set; }
		//public string? FG_SAPMaterialCode { get; set; }
		//public string? RM_SAPMaterialCode { get; set; }
		public int? intCoupSAPMaterialID { get; set; }
		public double? intWeightPerMRun { get; set; }
		public int? intBaseUOM { get; set; }
		public int? intSalesUOM { get; set; }
		public bool? bitBPCItem { get; set; }
		public int? tntStatusId { get; set; }
		public string? CreatedUser { get; set; }
		public string? UpdatedUser { get; set; }
		public int? intCreatedUId { get; set; }
		public string? ProductCode { get; set; }
		public string? Description { get; set; }
		public int? ProductType { get; set; }
		public DateTime? datCreatedDate { get; set; }
        public int? intUpdatedUId { get; set; }
        public DateTime? datUpdatedDate { get; set; }
		

	}
}
