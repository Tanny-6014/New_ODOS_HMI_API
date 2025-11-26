namespace WBSService.Dtos
{
    public class WBSAtCollapseLevelDto
    {
        public int StoreyLevelWBSId { get; set; }
        public int WBSId { get; set; }
        public string? WBS1From { get; set; }
        public string? WBS1To { get; set; }
        public string? WBS2From { get; set; }
        public string? WBS2To { get; set; }
        public string? WBS3From { get; set; }
        public string? WBS3To { get; set; }
        public string? WBS4From { get; set; }
        public string? WBS4To { get; set; }
        public string? WBS5From { get; set; }
        public string? WBS5To { get; set; }
        public int StatusId { get; set; }
        public int? StructureElementTypeId { get; set; }
        public int? sitProductTypeId { get; set; }

        public int? CreatedBy { get; set; }
        public DateTime? Created_On { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? Updated_On { get; set; }

        public int WBSMTNID { get; set; }

    }
}
