namespace WBSService.Model
{
    public class Project
    {
        public string ProjectName { get; set; }
        public int ProjectId { get; set; }
        public string ProjectDescription { get; set; }
        public string SAPProjectCode { get; set; }
        public int SAPProjectId { get; set; }
        public string SAPProjectDescription { get; set; }
        public string BBSSAPProjectDescription { get; set; }
        public string ShipToParty { get; set; }   //added for ship to party
        public string PhysicalProjectName { get; set; }  //added for ship to party
    }

}
