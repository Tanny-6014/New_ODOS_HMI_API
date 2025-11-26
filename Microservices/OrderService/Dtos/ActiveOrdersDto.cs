namespace OrderService.Dtos
{
    public class ActiveOrdersDto
    {
            public string CustomerCode{get;set;}
            public string ProjectCode{get;set;}
            public List<string> AddressCode{get;set;}
        public bool AllProjects{get;set;}
        public string UserName { get; set; }

    }
}
