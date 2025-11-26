namespace ParameterSetService.Dtos
{
    public class ProjectParameterDto
    {
        public byte? ParamSetNumber { get; set; }
        public int? ProjectId { get; set; }
        public int? ProductTypeL2Id { get; set; }
        public int? ParameteSet { get; set; }
        public byte? RefParamSetNumber { get; set; }
        public string? ProjectAbbr { get; set; }
        public string? Description { get; set; }
        public int? ParamCageID { get; set; }
        public byte? TransportModeId { get; set; }
        public int? TopCover { get; set; }
        public int? BottomCover { get; set; }
        public int? LeftCover { get; set; }
        public int? RightCover { get; set; }
        public int? CPLeftCover { get; set; }
        public int? CPRightCover { get; set; }
        public int? CPCWLength { get; set; }
        public int? Gap1 { get; set; }
        public int? Gap2 { get; set; }
        public int? Hook { get; set; }
        public int? Leg { get; set; }
        public int? PinSize { get; set; }
        public string? CLMaterialType { get; set; }
        public string? StandardCP { get; set; }
        public int? MWLap { get; set; }
        public int? CWLap { get; set; }
        public int? MO1 { get; set; }
        public int? MO2 { get; set; }
        public int? CO1 { get; set; }
        public int? CO2 { get; set; }
        public int? LapLength { get; set; }
        public int? EndLength { get; set; }
        public int? AdjFactor { get; set; }
        public int? CoverToLink { get; set; }
        public string? StandarCL { get; set; }
        public byte? StatusId { get; set; }
        public string? ParameterType { get; set; }
        public bool? StructureMarkingLevel { get; set; }
        public int? ProductTypeId { get; set; }
        public int? CreatedUID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedUID { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? InnerCover { get; set; }
        public int? OuterCover { get; set; }
        public bool? NetWeight { get; set; }
    }
}
