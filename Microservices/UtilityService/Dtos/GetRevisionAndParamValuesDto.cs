namespace UtilityService.Dtos
{
    public class GetRevisionAndParamValuesDto
    {
        public int INTGROUPMARKID { get; set; }
        public string VCHGROUPMARKINGNAME { get; set; }
        public int TNTGROUPREVNO { get; set; }
        public string VCHREMARKS { get; set; }
        public int CABDEFLAG { get; set; }
        public int TNTPARAMSETNUMBER { get; set; }
        public int INTPARAMETESET { get; set; }
        public int SITTOPCOVER { get; set; }
        public int SITBOTTOMCOVER { get; set; }
        public int SITLEFTCOVER { get; set; }
        public int SITRIGHTCOVER { get; set; }
    }
}
