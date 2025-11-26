namespace SAP_API.Models
{
    public class WBSMaintainence
    {
        public int intWBSMTNCId { get; set; }
        public int intWBSId { get; set; }
        public string Block { get; set; }
        public string StoryFrom { get; set; }
        public string StoryTo { get; set; }
        public string Part { get; set; }
        public string ProductType { get; set; }
        public string Structure { get; set; }
    }
}
