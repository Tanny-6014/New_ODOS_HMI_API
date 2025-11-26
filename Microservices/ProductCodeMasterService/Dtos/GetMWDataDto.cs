namespace ProductCodeMasterService.Dtos
{
    public class GetMWDataDto
    {

        public int intProductCodeId { get; set; }
        public string vchProductCode { get; set; }
        public decimal? decMWDiameter { get; set; }
        public decimal? decWeigthPerMeterRun { get; set; }
        public int intMaterialTypeId { get; set; }
        public string M_GRADE { get; set; }

    }
}
