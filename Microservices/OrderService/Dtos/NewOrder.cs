namespace OrderService.Dtos
{
    public class NewOrder
    {
        public string ProjectCode { get; set; }

        public List<string> WBS1 { get; set; }

        public List<string> WBS2 { get; set; }

        public List<string> WBS3 { get; set; }

        public string CustomerCode { get; set; }


    }
}
