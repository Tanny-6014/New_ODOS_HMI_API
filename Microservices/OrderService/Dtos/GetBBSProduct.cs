namespace OrderService.Dtos
{
    public class GetBBSProduct
    {
       public string CustomerCode { get; set; }
        public string ProjectCode { get; set; }
        public int JobID { get; set; }
        public string OrderSource { get; set; }
        public string StructureElement { get; set; }
        public string ProductType { get; set; }
        public string ScheduledProd { get; set; }
    }
}
