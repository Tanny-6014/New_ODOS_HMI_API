namespace DetailingService.Dtos
{
    public class AddPRCGroupMarkDto
    {
        public int GroupRevisionNumber { get; set; }
        public int ProjectId { get; set; }
        public int StructureElementTypeId { get; set; }
        public int SitProductTypeId { get; set; }
        public int GroupMarkId { get; set; }
        public int SeDetailId { get; set; }
        public int DefaultWidth { get; set; }
        public int DefaultDepth { get; set; }
        public int DefaultLength { get; set; }
        public int AssemblyInd { get; set; }
        public string GroupMarkingName { get; set; }
        public int SAPMaterial { get; set; }
        public string SiderForCode { get; set; }
        public int IsCABDE { get; set; }
        public int? CageSelector_Id { get; set; }


    }
}
