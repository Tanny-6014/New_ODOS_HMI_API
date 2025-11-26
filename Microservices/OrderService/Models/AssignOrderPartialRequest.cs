namespace OrderService.Models
{
    public class AssignOrderPartialRequest
    {
        public List<string> SoNums { get; set; }
        public string Type { get; set; }
        public string DeliveredBy { get; set; }
        public string Vendor { get; set; }
        public string? ContractNo { get; set; }
        public string Service { get; set; }
        public string Process { get; set; }

    }
}
