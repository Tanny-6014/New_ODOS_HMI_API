namespace OrderService.Dtos
{
    public class EmailNotificationDto
    {
        public string EmailTo { get; set; }

        public string CustomerCode { get; set; }

        public string ProjectCode { get; set; }

        public List<int> OrderNumber { get; set; }

        public string EmailBy { get; set; }
    }
}
