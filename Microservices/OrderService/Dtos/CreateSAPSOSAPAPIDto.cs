using OrderService.Models;

namespace OrderService.Dtos
{
    public class CreateSAPSOSAPAPIDto
    {
        public string pCustomerCode { get; set; }
        public string pProjectCode { get; set; }
        public string pContractNo { get; set; }
        public string pReqDate { get; set; }
        public string pInRemarks { get; set; }
        public string pExRemarks { get; set; }
        public string pInvRemarks { get; set; }
        public string pPONo { get; set; }
        public string pPODate { get; set; }
        public string pBBSNo { get; set; }
        public string pBBSDesc { get; set; }
        public List<OrderDetailsModels> pBBSDetails { get; set; }
        public string pPaymentType { get; set; }
        public SAPVBAKExtModels pVBAKExt { get; set; }
        public string pPriceDate { get; set; }
        public string pPaymentTerm { get; set; }
        public string pPriceGroup { get; set; }
        public string pIncoTerms1 { get; set; }
        public string pIncoTerms2 { get; set; }
        public string pUserID { get; set; }
    }
}
