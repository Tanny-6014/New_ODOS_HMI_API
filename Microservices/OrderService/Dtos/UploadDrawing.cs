namespace OrderService.Dtos
{
    public class UploadDrawing
    {
        public string lCustomerCode { get; set; }

        public string lProjectCode { get; set; }

        public List<int> lDrawingIDList { get; set; }

        public int lRevision { get; set; }

        public int lDrawingID { get; set; }

        public string lDrawingNo { get; set; }

        public string lRemarks { get; set; }

        public string lWBS1 { get; set; }

        public string lWBS2 { get; set; }

        public string lWBS3 { get; set; }

        public string lProdType { get; set; }

        public string lStructureElement { get; set; }

        public string lUploadType { get; set; }

        public string lScheduledProd { get; set; }

    }
}
