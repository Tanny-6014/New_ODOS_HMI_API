namespace OrderService.Models
{
    public class GetOrderNumberUpdateDto
    {
            public string CustomerCode{get;set;}
            public string ProjectCode{get;set;}
            public string ContractNo{get;set;}
            public int JobID{get;set;}
            public string StructureElement{get;set;}
            public string ProdType{get;set;}
            public string OrderSource{get;set;}
            public string ScheduledProd{get;set;}
            public string ActionType{get;set;}
            public string UserName { get; set; }
            public string SAPSOR { get; set; }
            public string TransportMode { get; set; }
            public string PONumber { get; set; }
    }
    
}
