namespace OrderService.Dtos
{
    public class SaveOrderDto
    {
            public string CustomerCode{get;set;}
            public string ProjectCode{get;set;} 
            public string WBS1{get;set;} 
            public int OrderNumber{get;set;} 
            public string PONumber{get;set;} 
            public string PODesc{get;set;} 
            public string CouplerType{get;set;}
            public string Transport { get; set; }

        public string UserName { get; set; }
    }
}
