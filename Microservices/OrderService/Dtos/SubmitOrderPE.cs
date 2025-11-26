namespace OrderService.Dtos
{
    public class SubmitOrderPE
    {
        public string pCustomerCode { get; set; }
        public string pProjectCode { get; set; }
        
        public int pOrderNo { get; set; }
        public string pStructureElement { get; set; }
         public string pProductType { get; set; }
         public string pScheduledProd { get; set; }
         public string pPONo { get; set; }
         public string pRequiredDate { get; set; }

         public string pTransport { get; set; }
    }
}
