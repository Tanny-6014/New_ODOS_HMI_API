namespace WBSService.Dtos
{
    public class User_Drawing_get
    {
        public int intWBSElementID { get; set; }

        public string vchWBS1 { get; set; }
        public string vchWBS2 { get; set; }
        public string vchWBS3 { get; set; }
        public string vchSubmitBy { get; set; }
        public string vchSubmitDate { get; set; }
        public bool ?IsApproved { get; set; }

        public string vchStructureElementType { get; set; }
        public string vchProductType { get; set; }

        public List<DrawingData> Drawing_List { get ; set ;}

     


    }

    public class User_Drawing_get_new
    {
        public int intWBSElementID { get; set; }

        public string WBS1 { get; set; }
        public string WBS2 { get; set; }
        public string WBS3 { get; set; }
        public string vchSubmitBy { get; set; }
        public string vchSubmitDate { get; set; }
        public bool? IsApproved { get; set; }

        public string StructureElement { get; set; }
        public string ProductType { get; set; }

        public string vchCustomer_Remark { get; set; }

        public int tntStatusId { get; set; }
        public float PostedTonage { get; set; }

        public string DrawingStatus { get; set; }

        public List<DrawingDetails> Drawing_List { get; set; }

        public string Required_Date { get; set; }

        public string PO_Number { get; set; }

        public string PO_Date { get; set; }


            





    }



}
