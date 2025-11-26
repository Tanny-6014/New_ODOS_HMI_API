namespace WBSService.Model
{
    public class PostingWBS
    {
        public int PostHeaderId { get; set; }
        public int WBSElementId { get; set; }
        public int ProjectId { get; set; }
        public int StructureElementTypeId { get; set; }
        public int ProductTypeId { get; set; }
        public int RefPostHeaderId { get; set; }
        public string WBS1 { get; set; }
        public string WBS2 { get; set; }
        public string WBS3 { get; set; }
        public string WBS4 { get; set; }
        public string WBS5 { get; set; }
        public string BBSNO { get; set; }
        public string BBSSDesc { get; set; }
        public string BBSDescriptionString { get; set; }
        public string SOR { get; set; }
        public bool IsCapping { get; set; }
        public string StructureElement { get; set; }
        public string ProductType { get; set; }
        public int PostedQty { get; set; }
        public int PostedcappingQty { get; set; }
        public int PostedCLinkQty { get; set; }
        public decimal PostedWeight { get; set; }
        public decimal PostedCappingWeight { get; set; }
        public decimal PostedCLinkWeight { get; set; }
        public int PostedById { get; set; }
        public string PostedBy { get; set; }
        public string PostedDate { get; set; }
        public string ReleaseBy { get; set; }
        public string ReleaseDate { get; set; }
        public string Result { get; set; }
        public bool Select { get; set; }
        public string StatusCode { get; set; }
        public string PostedToolTipValue { get; set; }
        public string ReleasedToolTipValue { get; set; }
        public string SalesOrderId { get; set; }
        public PostGroupMark PostGM { get; set; }
        public NDSStatus NDSStatus { get; set; }
        public int ReturnValue { get; set; }
        public string GroupId { get; set; }
        public string ReqDeliveryDate { get; set; }

    }
}
