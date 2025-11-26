namespace OrderService.Dtos
{
    public class StandardProductDetails
    {
        public int? SSID { get; set; }
        public string CustomerCode { get; set; }

        public string ProjectCode { get; set; }

        public string JobID { get; set; }

        public string? UpdateBy { get; set; }

        public DateTime? UpdateDate { get; set; }
        
    }
}
