namespace OrderService.Dtos
{
    public class CancelSAPSODto
    {
        public string pCustomerCode { get; set; }
        public string pProjectCode { get; set; }
        public int pJobID { get; set; }
        public string pPONo { get; set; }
        public string pBBSNo { get; set; }
        public string pSAPSO { get; set; }
    }
}
