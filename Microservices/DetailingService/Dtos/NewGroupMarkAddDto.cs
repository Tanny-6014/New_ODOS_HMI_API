namespace DetailingService.Dtos
{
    public class NewGroupMarkAddDto
    {
        public int  GroupMarkId { get; set; }
        public string  GroupMarkName { get; set; }
        public int  GroupRevisionNumber { get; set; }
        public int  ProjectId { get; set; }
        public int  WBSTypeId { get; set; }
        public int  StructureElementTypeId { get; set; }
        public int  SitProductTypeId { get; set; }
        public int  ParameterSetNumber { get; set; }
        public int  transport { get; set; }
        public int  IsCABOnly { get; set; }
        public string  Remarks { get; set; }
        public int  CreatedUserId { get; set; }
        public string  CreatedUserName { get; set; }
        public string  SiderForCode { get; set; }
    }
}
