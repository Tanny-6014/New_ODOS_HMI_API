namespace OrderService.Dtos
{
    public class GetSubmittedPOSearchAllDto
    {
            public string Category{get;set;} 
            public string OrigReqDateFrom{get;set;} 
            public string OrigReqDateTo{get;set;}
            public string RequiredDateFrom{get;set;} 
            public string RequiredDateTo{get;set;} 
            public string PONo{get;set;} 
            public string BBSNo{get;set;}
            public string CustomerName{get;set;}
            public List<string> ProjectTitle{get;set;}
            public string WBS1{get;set;}
            public string WBS2{get;set;} 
            public string WBS3{get;set;} 
            public List<string> ProductType{get;set;}
            public List<string> ProjectPIC{get;set;}
            public List<string> DetailingPIC{get;set;}
            public List<string> SalesPIC{get;set;} 
            public string SONo{get;set;} 
            public string SOR{get;set;}
            public string PODateFrom{get;set;}
            public string PODateTo{get;set;} 
            public string WTNo{get;set;}
            public string LoadNo{get;set;}
            public string CDelDateFrom{get;set;}
            public string CDelDateTo{get;set;} 
            public string DONo{get;set;} 
            public string InvNo{get;set;}
            public bool Forecast{get;set;} 
            public string UserName { get; set; }

        public string VehicleType { get; set; }
    }
}
