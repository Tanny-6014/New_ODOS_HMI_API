namespace OrderService.Dtos
{
    public class GetEmailNotificationDataDto
    {
        public string CustomerCode { get; set; }

        public string ProjectCode { get; set; }
        public List<EmailNotificationDataDto> Content { get; set; }

    }
}
