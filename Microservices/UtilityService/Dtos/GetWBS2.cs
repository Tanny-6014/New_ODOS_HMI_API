namespace UtilityService.Dtos
{
    public class GetWBS2
    {
        public string WBS2 { get; set; }// copywbs2

        //added for getwbs2
        public string VCHBBSNO { get; set; }
        public string BBS_DESC { get; set; }
        public int NDS_WBSELEMENT_ID { get; set; }
    }
}
