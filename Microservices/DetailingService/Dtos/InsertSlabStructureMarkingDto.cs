using DetailingService.Repositories;

namespace DetailingService.Dtos
{
    public class InsertSlabStructureMarkingDto
    {
        public int SEDetailingID { get; set; }
        public int StructureMarkId { get; set; }

        public int ParentStructureMarkId { get; set; }
        public string StructureMarkingName { get; set; }
        public int ParamSetNumber { get; set; }
        public int MainWireLength { get; set; }
        public int CrossWireLength { get; set; }
        public int MemberQty { get; set; }
        public bool BendingCheck { get; set; }
        public bool MachineCheck { get; set; }
        public bool TransportCheck { get; set; }
        
        public ProductCode ProductCode { get; set; }
        public bool MultiMesh { get; set; }
        public bool ProduceIndicator { get; set; }
        public int PinSize { get; set; }

        // public List<SlabProduct> SlabProduct { get; set; }
        public bool ProductGenerationStatus { get; set; }
        public ShapeCodeParameterSetDto ParameterSet { get; set; }
        public string SideForCode { get; set; }
        public bool ProductSplitUp { get; set; }

        public ShapeCode Shapecode { get; set; }
        public int MWLength { get; set; }
        public int CWLength { get; set; }
        public int MO1 { get; set; }
        public int MO2 { get; set; }
        public int CO1 { get; set; }
        public int CO2 { get; set; }
    }
}
