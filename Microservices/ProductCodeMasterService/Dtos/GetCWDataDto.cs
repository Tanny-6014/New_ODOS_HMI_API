namespace ProductCodeMasterService.Dtos
{
    public class GetCWDataDto
    {
        public int intProductCodeId { get; set; }
        public string vchProductCode { get; set; }
        public decimal? decCWDiameter { get; set; }
        public decimal? decCWWeigthPerMeterRun { get; set; }
        public int intMaterialTypeId { get; set; }
        public string C_GRADE { get; set; }
    }
}

