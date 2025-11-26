namespace OrderService.Dtos
{
    public class CABItemCancelProcessDto
    {
            public string CustomerCode{get;set;}
            public string ProjectCode{get;set;}
            public string ContractNo{get;set;}
            public int JobID{get;set;}
            public int ItemID{get;set;}
            public string StructureElement{get;set;}
            public string ProdType{get;set;}
            public string OrderSource{get;set;}
            public string ScheduledProd{get;set;}
            public string BBSSOR{get;set;}
            public string CouplerSOR{get;set;}
            public string STDBarSO{get;set;}
            public string UserName { get; set; }
    }
}
