namespace OrderService.Dtos
{
    public class OrderSummaryData
    {
        public List<string> lOrderNoList { get; set; }

        public string lProdType { get; set; }

        public string lPONumber { get; set; }

        public string lPODate { get; set; }

        public string lRequiredDate { get; set; }

        public string lTransport { get; set; }

        public List<decimal> lWT { get; set; }

        public List<int> lQty { get; set; }

        public decimal? lProdWT { get; set; }

        public string pSelectedSE { get; set; }

        public string pSelectedProd { get; set; }

        public string pSelectedPostID { get; set; }

        public string pSelectedScheduled { get; set; }

        public string lSelectedWBS { get; set; }

        public string pSelectedWBS1 { get; set; }

        public string pSelectedWBS2 { get; set; }

        public string pSelectedWBS3 { get; set; }

        public string lPEStatus { get; set; }

        public int lStatusDiff { get; set; }

        public string lOrderStatusT { get; set; }

        public string pWBS1 { get; set; }

        public string pWBS2 { get; set; }

        public string pWBS3 { get; set; }

        public string lLeadTimeProdType { get; set; }

        public string lLeadTime { get; set; }

        public string lHolidays { get; set; }

        public List<string> lNoDocs { get; set; }

        public string MainPONo { get; set; }

        public string MainPODate { get; set; }

        public string SiteEngr_Name { get; set; }

        public string SiteEngr_HP { get; set; }

        public string SiteEngr_Email { get; set; }

        public string Scheduler_Name { get; set; }

        public string Scheduler_HP { get; set; }

        public string Scheduler_Email { get; set; }

        public string Remarks { get; set; }

        public List<string> lRemark { get; set; }

        public string DeliveryAddress { get; set; }

        public List<string> lPONos { get; set; }

        public List<string> lRDs { get; set; }

        public List<string> lTransports { get; set; }

        public string pOrderNo { get; set; }

        public string lUpdateBy { get; set; }

        public string lSubmission { get; set; }

        public string lEditable { get; set; }

        public string lPlanEntCompl { get; set; }

        public string MainRequiredDate { get; set; }
        public string MainTranspoprt { get; set; }

        public string lSiteContact { get; set; }
        public List<string> lSiteContacts { get; set; }

        public string lHandphone { get; set; }
        public List<string> lHandphones { get; set; }

        public string lGoodsReceiver { get; set; }
        public List<string> lGoodsReceivers { get; set; }

        public string lGoodsReceiverHandphone { get; set; }
        public List<string> lGoodsReceiverHandphones { get; set; }

        public List<string> AdditionalRemark { get; set; }

        public string? Address { get; set; }

        public string? Gate { get; set; }

    }
}
