namespace OrderService.Dtos
{
    public class AmendmentIndicatorsDto
    {
            public bool Chk_Order_OnHold{get;set;}
            public string txtReason_Change_Order_OnHold{get;set;}
            public bool Chk_Lorry_Crane{get;set;}
            public bool Chk_Do_Not_Mix{get;set;} 
            public bool Chk_Call_Before_Delivery{get;set;}
            public bool Chk_Special_Pass{get;set;}
            public bool Chk_Barge_Booked{get;set;} 
            public bool Chk_Crane_Booked_Onsite{get;set;}
            public bool Chk_Police_Escort_Service{get;set;}
            public bool Chk_Premium_Service{get;set;}
            public bool Chk_Urgent_Order{get;set;}
            public bool Chk_Conquas_Order{get;set;}
            public bool Chk_Zero_Tolerance{get;set;} 
            public bool Chk_Low_Bed_Vehicle_Allowed{get;set;}
            public bool Chk_50_Ton_Vehicle_Allowed{get;set;} 
            public bool Chk_Unit_Mode{get;set;} 
            public string SORNumbers_to_amend { get; set; }

    }
}
