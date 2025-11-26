namespace WBSService.Dtos
{
    public class GetPostingCLinkHeaderInfoDto
    {
        public string? VCHWBS1 { get; set; }
        public string? VCHWBS2 { get; set; }
        public string? VCHWBS3 { get; set; }
        public int INTWBSELEMENTID { get; set; }
        public string? VCHPROJECTNAME { get; set; }
        public string? DESCRIPTION { get; set; }
        public int INTCONTRACTID { get; set; }
        public int INTPROJECTID { get; set; }
        public int INTSTRUCTUREELEMENTTYPEID { get; set; }
        public int SITPRODUCTTYPEID { get; set; }
        public string? VCHSTRUCTUREELEMENTTYPE { get; set; }
        public string? VCHPRODUCTTYPEL1 { get; set; }
        public string? VCHCONTRACTNUMBER { get; set; }
        public string? VCHNDSCONTRACTDESCRIPTION { get; set; }
        public int INTCUSTOMERCODE { get; set; }
        public string? VCHCUSTOMERNAME { get; set; }

        public string PROJ_DESC1 { get; set; }
        public int vchProjectCode { get; set; }
    }
}
