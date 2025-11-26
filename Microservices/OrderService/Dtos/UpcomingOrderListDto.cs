namespace OrderService.Dtos
{
    public class UpcomingOrderListDto
    {
        public int OrderNumber { get; set; }
        public string WBS1 { get; set; }
        public string WBS2 { get; set; }
        public string WBS3 { get; set; }
        public string StructureElement { get; set; }
        public string ProdType { get; set; }
        public string PONo { get; set; }
        public string ForecastDate { get; set; }
        public string DeliveryDate { get; set; }
        public Decimal FloorTonnage { get; set; }
        public string BBSNo { get; set; }

        public string BBSDesc { get; set; }

        public string OrderType { get; set; }

        public Int32? ConvertedOrderNo { get; set; }

        public string ConvertOrderDate { get; set; }

        public string ConvertedOrderBy { get; set; }

        public string OrderStatus { get; set; }

        public string NotifiedByEmail { get; set; }
        public string NotifiedEmailId { get; set; }
        public DateTime? NotifiedEmailDate { get; set; }

        public string CustomerCode { get; set; }
        public string ProjectCode { get; set; }

        public int SSNNo { get; set; }
    }
}
