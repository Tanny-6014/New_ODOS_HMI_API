namespace OrderService.Dtos
{
    public class SavedOrdersDto
    {
        public string CustomerCode{get;set;}
        public string ProjectCode{get;set;}
        public string PONumber{get;set;}
        public string PODate{get;set;} 
        public string RDate{get;set;}
        public string WBS1{get;set;} 
        public string WBS2{get;set;} 
        public string WBS3{get;set;} 
        public bool AllProjects { get; set; }
    }
}
