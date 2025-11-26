using System.Web.Helpers;

namespace OrderService.Dtos
{
    public class DeliveredOrderHMIdto
    {
        public string lSONo { get; set; }
        public string lWBS1 { get; set; } 
        public string lWBS2 { get; set; }  
        public string lWBS3 { get; set; }  
        public string lStEle { get; set; } 
        public string lPONo { get; set; }
        public string lPODate { get; set; }
        public string lReqDate { get; set; }
        public string lProdType { get; set; }
        public string lOrderWT { get; set; }
        public string lTransportMode { get; set; }
        public string lVehicleNo { get; set; }
        public string lActDelDate { get; set; }
        public string lOutTime { get; set; }
        public string lLoadWT { get; set; }
        public string lLoadPcs { get; set; }
        public string lLoadNo { get; set; }
        public string lUXInd { get; set; }
        public string lSOR { get; set; }
        public string lDONo { get; set; }
        public string lBBSNo { get; set; }
        public int lPartialDelivery { get; set; }
        public int lLeftMultiLoad { get; set; }
   }
}
