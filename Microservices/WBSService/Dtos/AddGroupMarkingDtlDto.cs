namespace WBSService.Dtos
{
    public class AddGroupMarkingDtlDto
    {
        public int? INTPOSTHEADERID { get; set; }
        public int? INTGROUPMARKID { get; set; }
        public string VCHGROUPMARKINGNAME { get; set; }
        public int TNTGROUPREVNO { get; set; }

        public int TNTGROUPQTY { get; set; }

        public string VCHREMARKS { get; set; }

        public int INTCREATEDUID { get; set; }

        public int INTPROJECID { get; set; }

        public int INTWBSELEMENTID { get; set; }

        public int INTSTRUCTUREELEMENTTYPEID { get; set; }

        public int SITPRODUCTTYPEID { get; set; }

        public string VCHBBSNO { get; set; }

        public string BBS_DESC { get; set; }

    }
}
