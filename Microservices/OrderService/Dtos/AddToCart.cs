namespace OrderService.Dtos
{
    public class AddToCart
    {
        public string pCustomerCode { get; set; }
        public string pProjectCode { get; set; }
        public string pOrderType { get; set; }
        public int pOrderNo { get; set; }
        public int pRefNo { get; set; }
        public string pStructureElement { get; set; }
        public string pProductType { get; set; }
        public string pWBS1 { get; set; }
        public string pWBS2 { get; set; }
        public string pWBS3 { get; set; }
        public string pPONo { get; set; }
        public string pScheduledProd { get; set; }
        public int pPostID { get; set; }
        public string UpdateBy { get; set; }
        public bool? GreenSteel { get; set; }
        public string AddressCode { get; set; }
    }
}
