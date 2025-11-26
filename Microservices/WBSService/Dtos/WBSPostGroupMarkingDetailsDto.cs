namespace WBSService.Dtos
{
    public class WBSPostGroupMarkingDetailsDto
    {
        public int? intSMGroupMarkId { get; set; }
        public string? vchSMGroupMarkingName { get; set; }


        public int? intSMProductQty { get; set; }

        public string? vchSMRemarks { get; set; }

        public string? vchSMStructureMarkingName { get; set; }

        public string? SMqty { get; set; }

        public bool selFlag { get; set; }

        public int? tntSMGroupRevNo { get; set; }

        public string BBSNo { get; set; }
        public string BBSRemarks { get; set; }
        public int PostedQty { get; set; }
        public decimal PostedWeight { get; set; }

        public int PostGroupMarkingDetailsId { get; set; }

        public int PostHeaderId { get; set; }
        public int GroupMarkId { get; set; }
        public string GroupMarkingName { get; set; }
        public int GroupMarkingRevisionNumber { get; set; }

        public string Remarks { get; set; }

        public int GroupQty { get; set; }

    }
}
