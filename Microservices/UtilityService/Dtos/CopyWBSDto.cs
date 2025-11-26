namespace UtilityService.Dtos
{
    public class CopyWBSDto
    {
        public int DESTPROJECTID { get; set; } 
        public int STRUCTUREELEMENTTYPEID { get; set; } 
        public int PRODUCTTYPEID { get; set; }
        public int WBSTYPEID { get; set; }   
        public int SOURCEPOSTHEADERID { get; set; }
        public string DESTWBSELEMENTIDS { get; set; }
        public string BBSNOS { get; set; } 
        public string BBSDESCS { get; set; }
        public int USERID { get; set; }
        public string UserName { get; set; }
    }
}
