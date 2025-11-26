namespace ShapeCodeService.Dtos
{
    public class ShapegroupDto
    {
        public int Id { get; set; }
        public string ShapeGroupName { get; set; }
        public string ShapeGroupDesc { get; set; }
        public string DimentionType { get; set; }
        public string StructureType { get; set; }
        public string BendingBarType { get; set; }
        public string CouplerType { get; set; }
        public bool IsArchived { get; set; }
    }
}
