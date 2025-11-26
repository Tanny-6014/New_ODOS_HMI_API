namespace WBSService.Dtos
{
    public class DrawingDetails
    {
        public int DrawingID { get; set; }               // Drawing ID (int)
        public string DrawingNo { get; set; }            // Drawing Number (string)
        public string FileName { get; set; }             // File Name (string)
        public int Revision { get; set; }             // Revision (string)
        public string Remarks { get; set; }              
        public DateTime UpdatedDate { get; set; }        
        public string UpdatedBy { get; set; }          
        public string Comments_User { get; set; }        
        public bool Approved_Status { get; set; }     
        public string Comment_Customer { get; set; }

        public string? FileSubmitedBy { get; set; }

        public int? DetailerSubmissionCount { get; set; }

        public int? CustomerSubmissionCount { get; set; }



    }

}
