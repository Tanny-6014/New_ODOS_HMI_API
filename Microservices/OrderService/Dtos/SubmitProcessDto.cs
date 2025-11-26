namespace OrderService.Dtos
{
    public class SubmitProcessDto
    {
        public string CustomerCode{get;set;}
        public string ProjectCode{get;set;}
        public string ContractNo{get;set;}
        public string ProdType{get;set;}
        public int JobID{get;set;}
        public string CashPayment{get;set;}
        public string CABFormer{get;set;}
        public string ShipToParty{get;set;}
        public string ProjectStage{get;set;}
        public string ReqDateFrom{get;set;}
        public string ReqDateTo{get;set;}
        public string PONumber{get;set;}
        public string PODate{get;set;}
        public string WBS1{get;set;}
        public string WBS2{get;set;}
        public string WBS3{get;set;}
        public string VehicleType{get;set;}
        public bool UrgentOrder{get;set;}
        public bool Conquas{get;set;}
        public bool Crane{get;set;}
        public bool PremiumService{get;set;}
        public bool ZeroTol{get;set;}
        public bool CallBDel{get;set;}
        public bool DoNotMix{get;set;}
        public bool SpecialPass{get;set;}
        public bool VehLowBed{get;set;}
        public bool Veh50Ton{get;set;}
        public bool Borge{get;set;}
        public bool PoliceEscort{get;set;}
        public string TimeRange{get;set;}
        public string IntRemarks{get;set;}
        public string ExtRemarks{get;set;}
        public string OrderSource{get;set;}
        public string StructureElement{get;set;}
        public string ScheduledProd{get;set;}
        public string OrderType{get;set;}
        public string InvRemarks{get;set;}
        public bool FabricateESM { get; set; }
        public string UserName { get; set; }
        public string UserType { get; set; }
        public string IsGreensteel { get; set; }
    }
}
