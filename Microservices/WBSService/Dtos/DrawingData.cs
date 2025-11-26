namespace WBSService.Dtos
{
    public class DrawingData
    {
        public int intDrawingId { get; set; }

        public int intDrawingNo { get; set; }
        public string vchFileName { get; set; }
        public int intRevision { get; set; }
        public string vchDetailer_Remark { get; set; }

        public string vchCustomer_Remark { get; set; }
        public string vchApprovedBy { get; set; }
        public string vchApprovedDate { get; set; }

        public bool? IsApproved { get; set; }




    }
}
