namespace OrderService.Dtos
{
    public class UpdateProjInchargeDBDTO
    {
        public List<string> CustomerCode { get; set; }
        public List<string> ProjectCode { get; set; }
        public string ProjIncharge { get; set; }
    }
}
