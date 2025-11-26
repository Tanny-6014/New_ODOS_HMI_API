namespace OrderService.Dtos
{
    public class WBS1Multiple
    {
       public string ProjectCode { get; set; }
        public List<string> WBS1 { get; set; }
          public List<string> WBS2 { get; set; }

        public string UserName { get; set; }

    }
}
