namespace OrderService.Dtos
{
    public class BPCItemCancelProcessDto
    {
        public string CustomerCode { get; set; }
        public string ProjectCode { get; set; }
        public string ContractNo { get; set; }
        public int JobID { get; set; }
        public int CageID { get; set; }
        public int LoadID { get; set; }
        public string StructureElement { get; set; }
        public string ProdType { get; set; }
        public string OrderSource { get; set; }
        public string ScheduledProd { get; set; }
        public string UserName { get; set; }
    }
}
