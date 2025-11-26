namespace WBSService.Dtos
{
    public class SendEmail
    {
        public string EmailTo { get; set; }
        public string EmailCc { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
    }
}
