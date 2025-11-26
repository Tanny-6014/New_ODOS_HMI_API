namespace DrainService.Dtos
{
    public class UpdateParameterSet
    {
        public int ProjectId { get; set; }
        public int ParameterSetId { get; set; }
        public int TransportModeId { get; set; }
        public int LapLength { get; set; }

        public int EndLength { get; set; }
        public int AdjFactor { get; set; }
        public int CoverToLink { get; set; }
        public int ProductTypeL2Id { get; set; }

        public int UserId { get; set; }
        public string Description { get; set; }
    }
}
