namespace DrainService.Dtos
{
    public class BBSCABReleaseDto
    {
       public string  ORD_REQ_NO { get; set; }
        public int intProjectId { get; set; }
        public int intWBSElementId { get; set; }
        public string BBS_NO { get; set; }
        public string chrBBSStatus { get; set; }
        public int UserID { get; set; }
        public string Username { get; set; }
    }
}
