namespace OrderService.Dtos
{
    public class OESProjOrder
    {
        public string CustomerCode { get; set; }
        public string ProjectCode { get; set; }
    public int OrderJobID { get; set; }
public int OrderNumber
{ get; set; }
public string OrderType
{ get; set; }
public string WBS1
{ get; set; }
public string WBS2
{ get; set; }
public string WBS3
{ get; set; }
public string Remarks
{ get; set; }
public float TotalWeight
{ get; set; }

public string DeliveryAddress
{ get; set; }
public string SiteEngr_Name
{ get; set; }
public string SiteEngr_HP
{ get; set; }

public string SiteEngr_Email
{ get; set; }
public string Scheduler_Name
{ get; set; }
public string Scheduler_HP
{ get; set; }
public string Scheduler_Email
{ get; set; }
public string OrderStatus
{ get; set; }
public string OrderSource
{ get; set; }
public string PONumber
{ get; set; }
public string PODate
{ get; set; }
public string RequiredDate
{ get; set; }
public string OrigReqDate
{ get; set; }
public string TransportMode
{ get; set; }
public string UpdateDate
{ get; set; }
public string UpdateBy
{ get; set; }
public string SubmitDate
{ get; set; }
public string SubmitBy
{ get; set; }
public string OrderShared
{ get; set; }
}
}
