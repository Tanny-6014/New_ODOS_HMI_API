namespace OrderService.Models
{
    public class GenerateBatchRequest
    {
        public List<string> SoNums { get; set; }
        public string Type { get; set; }
        public string DeliveredBy { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public string? ContractNo { get; set; }
        public DateTime? ReqDelDate { get; set; }
    }
}
