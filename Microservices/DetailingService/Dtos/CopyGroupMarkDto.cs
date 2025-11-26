namespace DetailingService.Dtos
{
    public class CopyGroupMarkDto
    {

        public int STRUCTUREELEMENTTYPEID { get; set; }
        public int PRODUCTTYPEID { get; set; }
        public int WBSTYPEID { get; set; }
        public int SOURCEPROJECTID { get; set; }
        public int DESTPROJECTID { get; set; }
        public int SOURCEPARAMETERSETID { get; set; }
        public int DESTPARAMETERSETID { get; set; }
        public int SOURCEGROUPMARKID { get; set; }
        public string DESTGROUPMARKNAME { get; set; }
        public string COPYFROM { get; set; }
        public string WBSELEMENTIDS { get; set; }
        public int ISGROUPMARKREVISION { get; set; }
        public int USERID { get; set; }
       
    }
}
