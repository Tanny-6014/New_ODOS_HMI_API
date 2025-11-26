namespace OrderService.Dtos
{
    public class CopyBBSDtocs
    {
           public string SourceCustomerCode{get;set;}
           public string SourceProjectCode{get;set;}
           public int SourceOrderNo{get;set;}
           public int SourceBBSID{get;set;}
           public string DesCustomerCode{get;set;}
           public string DesProjectCode{get;set;}
           public int DesOrderNo{get;set;}
           public int DesBBSID{get;set;}
           public int DesBarCount { get; set; }

        public string UpdateBy { get; set; }
    }
}
