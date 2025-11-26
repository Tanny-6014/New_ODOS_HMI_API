namespace ParameterSetService.Models
{
    public class ProductTypeMaster
    {
        public int? sitProductTypeID { get; set; }
        public string? vchProductType { get; set; }
        public string? vchProductTypeDescription { get; set; }
        public byte? tntStatusId { get; set; }
        public int? intCreatedUId { get; set; }
        public DateTime? datCreatedDate { get; set; }
        public int? intUpdatedUId { get; set; }
        public DateTime? datUpdatedDate { get; set; }
        public string? chrprefix { get; set; }
    }
}