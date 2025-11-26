namespace DetailingService.Dtos
{
    public class MeshStructureMarkingDetailsDto
    {
        public int intStructureMarkId { get; set; }
        public int? tntStructureRevNo { get; set; }
        public int? intSEDetailingId { get; set; }
        public int? intParentStructureMarkId { get; set; }
        public int? intConsignmentType { get; set; }
        public int? intProductCodeId { get; set; }
        public int? intShapeId { get; set; }
        public string? vchStructureMarkingName { get; set; }
        public int? tntParamSetNumber { get; set; }
        public bool? bitSimilarStructure { get; set; }
        public string? vchSimilarTo { get; set; }
        public decimal? decTotalMeshMainLength { get; set; }
        public decimal? decTotalMeshCrossLength { get; set; }
        public int? intMemberQty { get; set; }
        public bool? bitCoat { get; set; }
        public bool? bitBendingCheck { get; set; }
        public bool? bitMachineCheck { get; set; }
        public int? numArea { get; set; }
        public int? intTotalQty { get; set; }
        public int? intTotalBend { get; set; }
        public int? numTheoreticTonnage { get; set; }
        public int? numNetTonnage { get; set; }
        public string? vchDrawingReference { get; set; }
        public string? chrDrawingVersion { get; set; }
        public string? vchDrawingRemarks { get; set; }
        public string? vchTactonConfigurationState { get; set; }
        public int? tntStatusId { get; set; }
        public bool? bitTransportCheck { get; set; }
        public int? intShapeTransHeaderId { get; set; }
        public int? intSAPMaterialCodeId { get; set; }
        public decimal? width { get; set; }
        public decimal? depth { get; set; }
        public decimal? length { get; set; }
        public bool? AssemblyIndicator { get; set; }
        public string? Remarks { get; set; }
        public int? intArmaid { get; set; }
        public bool? bitSingleMesh { get; set; }
        public int? intCreatedBy { get; set; }
        public DateTime? datCreatedDate { get; set; }
        public int? intUpdatedBy { get; set; }
        public DateTime? datUpdatedDate { get; set; }
        public string? ProduceIndicator { get; set; }
        public bool? ProductSplitUp { get; set; }
        public string? SideForCode { get; set; }
    }
}
