namespace WBSService.Dtos
{
    public class SaveWBSDto
    {
        public int WBSElementId { get; set; }
        public int ProjectId { get; set; }
        public int StructureElementTypeId { get; set; }
        public int ProductTypeId { get; set; }
        public string BBSNO { get; set; }
        public string BBSSDesc { get; set; }
    }
}
