namespace OrderService.Dtos
{
    public class WithdrawOrderPE
    {
       public string pCustomerCode { get; set; }
       public string pProjectCode { get; set; }
       public int pOrderNo { get; set; }
        public string pStructureElement { get; set; }
        public string pProductType { get; set; }
        public string pScheduledProd { get; set; }

        public string UserName { get; set; }
    }
}
