namespace ProductCodeMasterService.Dtos
{
    public class CABProductCodeListDto
    {
        public int CABProductCodeID { get; set; }
        public string ProductCode { get; set; }

        public string Description { get; set; }

        public int ProductTypeID { get; set; }
        public string ProductType { get; set; }

        public string GradeType { get; set; }

        public int Diameter { get; set; }

        public bool bitCouplerIndicator { get; set; }

        public string CouplerType { get; set; }

        public int FGSAPMaterialID { get; set; }
        public int RMSAPMaterialID { get; set; }

        public string FG_SAPMaterialCode { get; set; }
        public string RM_SAPMaterialCode { get; set; }

        public int StatusID { get; set; }

        public string CreatedUser { get; set; }

        public string UpdatedUser { get; set; }



    }
}
