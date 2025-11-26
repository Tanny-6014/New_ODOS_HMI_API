using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderService.Dtos
{
    [Table("OESupcomingorder")]
    public class UpcomingOrderDto
    {
        [Key]
        public int OrderNumber { get;set;}
        public string WBS1{get;set;}
        public string WBS2{get;set;}
        public string WBS3{get;set;}
        public string StructureElement { get;set;}
        public string ProductType { get;set;}
        public string LowerPONumber { get;set;}
        public DateTime? ForecastDate { get;set;}
        public DateTime? DeliveryDate { get;set;}
        public Decimal FloorTonnage { get;set;}
        public string BBSNo { get; set; }

        public string BBSDesc { get; set; }

        public string OrderType { get; set; }

        public Int32? ConvertedOrderNo { get; set; }

        public DateTime? ConvertOrderDate { get; set; }

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
