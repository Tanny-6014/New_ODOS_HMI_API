namespace OrderService.Dtos
{
    public class FileExsistDto
    {
        public string CustomerCode { get; set; }
        public string ProjectCode { get; set; }
        public List<string> FileName { get; set; }
    }
}
