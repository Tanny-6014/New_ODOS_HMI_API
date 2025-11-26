namespace WBSService.Dtos
{
    public class PostingUpdateDto
    {
        public int PostHeaderId { get; set; }
        public int? PostedBy { get; set; }
        public string UserName { get; set; }
    }
}
