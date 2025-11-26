namespace OrderService.Dtos
{
    public class CheckBBSNoProcess
    {
        public string CustomerCode { get; set; }
        public string ProjectCode { get; set; }
        public int JobID { get; set; }
        public string StructureElement { get; set; }
        public string ProdType { get; set; }
        public string ScheduledProd { get; set; }
        public string OrderSource { get; set; }
        public string BBSNo { get; set; }
    }
}
