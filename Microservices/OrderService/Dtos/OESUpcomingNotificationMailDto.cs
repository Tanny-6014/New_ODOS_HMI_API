namespace OrderService.Dtos
{
    public class OESUpcomingNotificationMailDto
    {
        public int OrderNo { get; set; }
        public string EmailTo { get; set; }
        public DateTime Date { get; set; }
        public string EmailBy { get; set; }
    }
}
