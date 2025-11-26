namespace OrderService.Dtos
{
    public class UpdateSORDto
    {
        public string CustomerCode { get; set; }
        public string ProjectCode { get; set; }
        public string ContractNo { get; set; }
        public string ProdType { get; set; }
        public int JobID { get; set; }
        public string SOR { get; set; }
        public string CashPayment { get; set; }
        public string ProjectStage { get; set; }
        public string ReqDateFrom { get; set; }
        public string ReqDateTo { get; set; }
        public string PONumber { get; set; }
        public string PODate { get; set; }
        public string WBS1 { get; set; }
        public string WBS2 { get; set; }
        public string WBS3 { get; set; }
        public string VehicleType { get; set; }
        public bool UrgentOrder { get; set; }
        public bool Conquas { get; set; }
        public bool Crane { get; set; }
        public bool PremiumService { get; set; }
        public bool ZeroTol { get; set; }
        public bool CallBDel { get; set; }
        public bool DoNotMix { get; set; }
        public bool SpecialPass { get; set; }
        public bool VehLowBed { get; set; }
        public bool Veh50Ton { get; set; }
        public bool Borge { get; set; }
        public bool PoliceEscort { get; set; }
        public string TimeRange { get; set; }
        public string IntRemarks { get; set; }
        public string ExtRemarks { get; set; }
        public string InvRemarks { get; set; }
        public string OrderSource { get; set; }
        public string StructureElement { get; set; }
        public string ScheduledProd { get; set; }
        public int ChReqDate { get; set; }
        public int ChPONumber { get; set; }
        public int ChVehicleType { get; set; }
        public int ChBookInd { get; set; }
        public int ChBBSNo { get; set; }
        public int ChBBSDesc { get; set; }
        public int ChIntRemakrs { get; set; }
        public int ChExtRemakrs { get; set; }
        public int ChInvRemakrs { get; set; }
        public string OrderStatus { get; set; }
        public bool FabricateESM { get; set; }

        public string strMastReqDate { get; set; }

        public string strUrgOrd { get; set; }
        public string strIntRemark { get; set; }
        public string strExtRemark { get; set; }
        public string strPremiumSvc { get; set; }
        public string strCraneBooked { get; set; }
        public string strBargeBooked { get; set; }
        public string strPoliceEscort { get; set; }
        public string strZeroTol { get; set; }
        public string strCallBefDel { get; set; }
        public string strConquas { get; set; }
        public string strSpecialPass { get; set; }
        public string strLorryCrane { get; set; }
        public string strLowBedVeh { get; set; }
        public string strDoNotMix { get; set; }
        public string strOnHold { get; set; }
        public string str50TonVeh { get; set; }

        public string strBBSNo { get; set; }

        public string strItemReqDate { get; set; }

        public string UserName { get; set; }
    }
}
