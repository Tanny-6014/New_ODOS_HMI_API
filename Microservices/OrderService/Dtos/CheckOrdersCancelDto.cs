namespace OrderService.Dtos
{
    public class CheckOrdersCancelDto
    {
        public string CustomerCode{get;set;}
        public string ProjectCode{get;set;}
        public List<string> OrderNumber{get;set;} 
        public List<string> ProdType{get;set;}
        public List<string> StructureElement{get;set;}
        public List<string> ScheduledProd{get;set;}
        public List<string> OrderSource { get; set; }
    }
}
