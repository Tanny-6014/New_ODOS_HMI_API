namespace DrainService.Dtos
{
    public class GetHeaderInfoDto
    {
        public int intProjectId { get; set; }
        public string vchProjectCode { get; set; }
        public string vchProjectName { get; set; }
        public int intContractID { get; set; }
        public string vchContractNumber { get; set; }
        public string vchNDSContractDescription { get; set; }
        public int intCustomerCode { get; set; }
        public string vchCustomername { get; set; }
        public string vchCustomerNo { get; set; }
        public int intSAPContract { get; set; }
        public string vchSAPContractDescription { get; set; }
        public int intSAPProjectId { get; set; }
        public string SAPProjectDescription { get; set; }
    }
}