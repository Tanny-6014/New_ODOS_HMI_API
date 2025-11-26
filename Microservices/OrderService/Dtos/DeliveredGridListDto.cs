namespace OrderService.Dtos
{
    public class DeliveredGridListDto
    {
        public string  PONo { get; set; }

        public string WBS1 { get; set; }

        public string WBS2 { get; set; }

        public string WBS3 { get; set; }

        public string StructureElement { get; set; }

        public string ProductType { get; set; }

        public string PODate { get; set; }

        public string RequiredDate { get; set; }

        public decimal OrderWT { get; set; }

        public int LoadQty { get; set; }

        public decimal LoadWT { get; set; }

        public string Delivery_Date { get; set; }

        public string TransportMode { get; set; }

        public string Vehicle_No { get; set; }

        public string Vehicle_out_time { get; set; }

        public int DigiOSID { get; set; }

        public string DONo { get; set; }

        public string BBSNo { get; set; }

        public string BBSDesc { get; set; }

        public string CustomerCode { get; set; }

        public string ProjectCode { get; set; }

        public string ProjectTitle { get; set; }

        public int PartialDelivery { get; set; }
    }
}
