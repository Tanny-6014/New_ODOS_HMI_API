namespace ParameterSetService.Models
{
    public class TransportMaster
    {
        public byte? tntTransportModeId { get; set; }
        public string? vchTransportMode { get; set; }
        public string? vchTransportDescription { get; set; }
        public byte? tntStatusId { get; set; }
        public int? intCreatedUId { get; set; }
        public DateTime? datCreatedDate { get; set; }
        public int? intUpdatedUId { get; set; }
        public DateTime? datUpdatedDate { get; set; }

    }
}
