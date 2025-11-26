namespace OrderService.Dtos
{
    public class SendCreditBlockEmailDto
    {
           public string ProdType{get;set;}
           public string CustomerCode{get;set;} 
           public string ProjectCode{get;set;}
           public int[] pJobID { get; set; }
    }
}
