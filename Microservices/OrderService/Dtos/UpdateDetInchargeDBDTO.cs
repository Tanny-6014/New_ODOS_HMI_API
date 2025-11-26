namespace OrderService.Dtos
{
    public class UpdateDetInchargeDBDTO
    {
        public List<string> CustomerCode { get; set; }
        public List<string> ProjectCode { get; set; }
        public string DetIncharge { get; set; }
    }
}
