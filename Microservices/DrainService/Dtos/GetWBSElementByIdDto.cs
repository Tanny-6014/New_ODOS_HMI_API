namespace DrainService.Dtos
{
    public class GetWBSElementByIdDto
    {
        public int intWBSElementId { get; set; }
        public string WBS { get; set; }
        public int intDetailingWBSId { get; set; }
        public int intGroupMarkId { get; set; }
        public int tntGroupRevNo { get; set; }
        //public int intWBSElementId { get; set; }
        public string vchDrawingReference { get; set; }
        public string chrDrawingVersion { get; set; }
        public string vchDrawingRemarks { get; set; }
        public int tntStatusId { get; set; }
        //public int intWBSElementId { get; set; }
        public int intStoreyLevelWBSId { get; set; }
        public int intWBSId{ get; set; }
        public string vchWBS1{ get; set; }
        public string vchWBS2 { get; set; }
        public string vchWBS3 { get; set; }
        public string vchWBS4 { get; set; }
        public string vchWBS5 { get; set; }
       // public int tntStatusId { get; set; }
        public int WBSMTNID { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int StructureElementID { get; set; }
        public int ProductTypeID { get; set; }
    }
}
