namespace WBSService.Model
{
    public class WBS
    {
        public int intWBSId { get; set; }
        public int intWBSTypeId { get; set; }
        public int intProjectId { get; set; }
        public int intCreatedUID { get; set; }
        public DateTime datCreatedDate { get; set; }
        public int intUpdatedUID { get; set; }
        public DateTime datUpdatedDate { get; set; }

    }
}
