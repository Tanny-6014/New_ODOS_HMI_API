namespace OrderService.Dtos
{
    public class UploadImageDto
    {
        public string CustomerCode { get; set; }
        public string ProjectCode { get; set; }
        public int JobID { get; set; }
        public int CageID { get; set; }
        public bool Template { get; set; }
        public IFormFile ElevatedView { get; set; }
        public IFormFile PlanView { get; set; }  

    }
}
