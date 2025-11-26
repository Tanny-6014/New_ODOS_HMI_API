namespace WBSService.Dtos
{
    public class DrawingSubmission
    {
        public int? DrawingId { get; set; } // intDrawingId
        public string? DrawingNo { get; set; } // intDrawingNo
        public string? CustomerCode { get; set; } // vchCustomerCode
        public string? ProjectCode { get; set; } // vchProjectCode
        public string? FileName { get; set; } // vchFileName
        public int? Revision { get; set; } // intRevision
        public string? DetailerRemark { get; set; } // vchDetailer_Remark
        public string? CustomerRemark { get; set; } // vchCustomer_Remark
        public int? WBSElementID { get; set; } // intWBSElementID
        public string? SubmitDate { get; set; } // vchSubmitDate
        public string? SubmitBy { get; set; } // vchSubmitBy
        public string? ApprovedBy { get; set; } // vchApprovedBy
        public string? ApprovedDate { get; set; } // vchApprovedDate
        public bool? IsApproved { get; set; } // IsApproved
        public bool? IsSubmitted { get; set; } // IsSubmitted
        public bool? IsSubmitMail { get; set; } // IsSubmitMail
        public bool? IsApprovedMail { get; set; } // IsApprovedMail
        public bool? IsDeleted { get; set; } // isDeleted

        public string WBS1 { get; set; }
        public string WBS2 { get; set; }

        public string WBS3 { get; set; }


        //public IFormFile? File { get; set; } // The file to be uploaded

    }

    public class DrawingSubmission_New
    {
        public int? DrawingId { get; set; } // intDrawingId
        public string? DrawingNo { get; set; } // intDrawingNo
        public string? CustomerCode { get; set; } // vchCustomerCode
        public string? ProjectCode { get; set; } // vchProjectCode
        public string? FileName { get; set; } // vchFileName
        public int? Revision { get; set; } // intRevision
        public string? DetailerRemark { get; set; } // vchDetailer_Remark
        public string? CustomerRemark { get; set; } // vchCustomer_Remark
        public int? WBSElementID { get; set; } // intWBSElementID
        public string? SubmitDate { get; set; } // vchSubmitDate
        public string? SubmitBy { get; set; } // vchSubmitBy
        public string? ApprovedBy { get; set; } // vchApprovedBy
        public string? ApprovedDate { get; set; } // vchApprovedDate
        public bool? IsApproved { get; set; } // IsApproved
        public bool? IsSubmitted { get; set; } // IsSubmitted
        public bool? IsSubmitMail { get; set; } // IsSubmitMail
        public bool? IsApprovedMail { get; set; } // IsApprovedMail
        public bool? IsDeleted { get; set; } // isDeleted

        public string WBS1 { get; set; }
        public string WBS2 { get; set; }

        public string WBS3 { get; set; }

        public string StructureElement { get; set; }

        public string ProductType { get; set; }

        public string DrawingStatus { get; set; }


        //public IFormFile? File { get; set; } // The file to be uploaded

    }

}
