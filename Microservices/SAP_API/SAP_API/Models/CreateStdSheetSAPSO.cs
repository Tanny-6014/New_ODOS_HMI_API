using SAP_API.Modelss;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAP_API.Models
{
    public class CreateStdSheetSAPSO
    {
        public string pCustomerCode{get;set;}
        public string pProjectCode{get;set;}
        public string pContractNo{get;set;}
        public string pReqDate{get;set;}
        public string pInRemarks{get;set;}
        public string pExRemarks{get;set;}
        public string pInvRemarks{get;set;}
        public string pPONo{get;set;}
        public string pPODate{get;set;}
        public string pBBSNo{get;set;}
        public string pBBSDesc{get;set;}
        public List<StdSheetDetailsModels> pBBSDetails{get;set;} 
        public string pPaymentType{get;set;}
        public SAPVBAKExtModels pVBAKExt{get;set;}
        public string pPriceDate{get;set;}
        public string pPaymentTerm{get;set;}
        public string pPriceGroup{get;set;}
        public string pIncoTerms1{get;set;}
        public string pIncoTerms2{get;set;}
        public string pUserID { get; set; }
    }
}