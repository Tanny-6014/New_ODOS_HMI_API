namespace OrderService.Dtos
{
    public class WBSPostCappingInfoDto
    {
        public string CAPPRODUCT { get; set; }
        public string? SHAPECODE { get; set; }

        public int WIDTH { get; set; }

        public int DEPTH { get; set; }

        public int COUNT { get; set; }
        public string TYPE { get; set; }
        public string STRUCTUREELEMENT { get; set; }
        public int MWLENGTH { get; set; }

        public int CWQTY { get; set; }
        public int MO1 { get; set; }
        public int MO2 { get; set; }
        public int CWSPACE { get; set; }
        public int MWSPACE { get; set; }
        public int LENGTH { get; set; }

        public int CO1 { get; set; }
        public int CO2 { get; set; }
        public int MWQTY { get; set; }

        public decimal INVOICEMWWEIGHT { get; set; }
        public decimal INVOICECWWEIGHT { get; set; }

        public decimal THEORITICALWEIGHT { get; set; }

        public decimal AREA { get; set; }
        public int SMID { get; set; }

        public int PMID { get; set; }

        public string ParamValues { get; set; }



    }
}
