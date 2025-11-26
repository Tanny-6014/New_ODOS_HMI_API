namespace WBSService.Dtos
{
    public class UpdateGroupMarkDetailsDto
    {
        public int PostHeaderId { get; set; }
        public int intGroupMarkId { get; set; }
        public int tntGroupQty { get; set; }
        public string VCHRemarks { get; set; }
        public int intUpdatedId { get; set; }
    }
}
