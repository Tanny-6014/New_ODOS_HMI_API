using DetailingService.Repositories;

namespace DetailingService.Dtos
{
    public class AddColumnStructureMarkDto
    {
        public int StructureMarkId { get; set; }
        public string StructureMarkingName { get; set; }
        public ProductCode productCode { get; set; }
        public ShapeCode ShapeCode { get; set; }
        public int ColumnHeight { get; set; }
        public int ColumnWidth { get; set; }
        public int ColumnLength { get; set; }
        public int TotalNoOfLinks { get; set; }
        public int MemberQty { get; set; }
        public ProductCode ClinkProductLength { get; set; }
        public ProductCode ClinkProductWidth { get; set; }
        public int RowatLength { get; set; }
        public int RowatWidth { get; set; }
        public bool IsCLink { get; set; }
        public bool CLOnly { get; set; }
        public bool ProduceIndicator { get; set; }
        public bool BendingCheckInd { get; set; }
        public int TotalQty { get; set; }
        public int PinSize { get; set; }
        public int SEDetailingID { get; set; }
        public int ParamSetNumber { get; set; }

        public int ParentStructureMarkId { get; set; }

    }
}
