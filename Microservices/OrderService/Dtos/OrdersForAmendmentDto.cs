namespace OrderService.Dtos
{
    public class OrdersForAmendmentDto
    {
            public string CustomerCode{get;set;} 
            public string ProjectCode{get;set;}
            public string RDateFrom{get;set;}
            public string RDateTo{get;set;} 
            public string SORNofrom{get;set;} 
            public string SORNoTo{get;set;} 
            public string WBS1{get;set;} 
            public string WBS2{get;set;} 
            public string WBS3{get;set;} 
            public string SearchOptions{get;set;} 
            public string txtSearch{get;set;} 
            public string SO_SOR_Search_Range{get;set;}
            public string Req_PO_Date_Search_Range{get;set;} 
            public string Rev_required_Conf_date_from{get;set;} 
            public string Rev_required_Conf_date_to{get;set;} 
            public string Rev_Req_Confirmed_Date_Search_Range{get;set;} 
            public string SearchProducts{get;set;} 
            public string SearchOptionsByDesignation{get;set;} 
            public string lSearchByDesignation { get; set; }
    }
}
