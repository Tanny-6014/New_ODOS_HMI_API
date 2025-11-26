namespace DetailingService.Dtos
{
    public class BOMHeaderDto
    {
        public string vchProductCode { get; set; }
        public string MarkingName { get; set; }
        public string MarkingDescription { get; set; }
        public string MeshType { get; set; }
        public string ReleasedStatus { get; set; }
        public decimal decMWDiameter { get; set; }
        public int intMWSpace { get; set; }
        public decimal decMWLength { get; set; }
        public decimal decCWDiameter { get; set; }
        public int intCWSpace { get; set; }
        public decimal decCWLength { get; set; }
        public string BOMIndicator { get; set; }
        public string TwinIndicator { get; set; }
        public bool bitStaggeredIndicator { get; set; }
        public int countProduction { get; set; }
    }
}
