namespace OrderService.Dtos
{
    public class InsertDataUpcomingOrdersDto
    {
            public int OrderNumber{get;set;}
            public string OrderType{get;set;}
            public string CustomerCode{get;set;}
            public string ProjectCode{get;set;}
            public string WBS1{get;set;}
            public string WBS2{get;set;}
            public string WBS3{get;set;}
            public string StructureElement{get;set;}
            public string ProductType{get;set;}
            public DateTime ForecastDate{get;set;}
            public DateTime DeliveryDate{get;set;}
            public string LowerPONumber{get;set;}
            public string BBSNo{get;set;}
            public string BBSDesc{get;set;}
            public decimal FloorTonnage{get;set;}
            public int ConvertedOrderNo{get;set;}
            public string OrderStatus{get;set;}
            public string NotifiedByEmail{get;set;}
            public string NotifiedEmailId{get;set;}
            public DateTime NotifiedEmailDate { get; set; }
    }
}
