namespace WBSService.Model
{
    public class PostGroupMark
    {
        public int PostGroupMarkingDetailsId { get; set; }
        public int PostHeaderId { get; set; }
        public int GroupMarkId { get; set; }
        public string GroupMarkingName { get; set; }
        public int GroupMarkingRevisionNumber { get; set; }
        public string Remarks { get; set; }
        public int GroupQty { get; set; }
        public int NoOfStructureMark { get; set; }
        public string BBSNo { get; set; }
        public string BBSRemarks { get; set; }
        public int PostedQty { get; set; }
        public decimal PostedWeight { get; set; }
        public int ProjectId { get; set; }
        public string Result { get; set; }
        public string ToolTipValue { get; set; }
        public string BOMPath { get; set; }
        public string isModular { get; set; } //Modular 31014
        public int isCABDE { get; set; } //Added for CABDE on 27.06.2016
        public string productcode { get; set; } //added for difot

    }
}
