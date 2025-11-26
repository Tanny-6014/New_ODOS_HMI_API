namespace OrderService.Dtos
{
    public class DrawingList
    {
       public string CustomerCode { get; set; }
        public string ProjectCode { get; set; }
        public List<string> WBS1 { get; set; }
        public List<string> WBS2 { get; set; }
        public List<string> WBS3 { get; set; }
        public List<string> ProductType { get; set; }
        public List<string> StructureElement { get; set; }
        public string Category { get; set; }
    }
}
