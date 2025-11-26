namespace DetailingService.Dtos
{
    public class BeamProductByStructureMarkIdDto
    {
        public int INTPRODUCTMARKID { get; set; }
        public int INTSTRUCTUREMARKID { get; set; }
        public string VCHPRODUCTMARKINGNAME { get; set; }
        public int INTPRODUCTCODE { get; set; }
        public int NUMCAGEWIDTH { get; set; }
        public int NUMCAGEDEPTH { get; set; }
        public int NUMCAGESLOPE { get; set; }
        public int NUMBEAMWIDTH { get; set; }

        public int NUMBEAMDEPTH { get; set; }
        public int NUMBEAMSLOPE { get; set; }

        public int NUMINVOICECWLENGTH { get; set; }
       
        public int NUMINVOICEMWLENGTH { get; set; }
        public int INTSHAPECODEID { get; set; }
        public int INTINVOICEMWQTY { get; set; }
        public int INTINVOICECWQTY { get; set; }

        public decimal NUMINVOICEMWWEIGHT { get; set; }
        public decimal NUMINVOICECWWEIGHT { get; set; }                       

        public decimal NUMINVOICEAREA { get; set; }
        public decimal NUMPRODUCTIONAREA { get; set; }
        public decimal NUMINVOICEWEIGHT { get; set; }
        public decimal NUMTHEORATICALWEIGHT { get; set; }

        public int INTMEMBERQTY { get; set; }
        public int INTMO1 { get; set; }
        public int INTMO2 { get; set; }

        public int INTCO1 { get; set; }
        public int INTCO2 { get; set; }

        public int INTPRODUCTIONMO1 { get; set; }
        public int INTPRODUCTIONMO2 { get; set; }

        public int INTPRODUCTIONCO1 { get; set; }
        public int INTPRODUCTIONCO2 { get; set; }

        public decimal NUMPRODUCTIONMWLENGTH { get; set; }
        public decimal NUMPRODUCTIONCWLENGTH { get; set; }
        public decimal NUMPRODUCTIONMWWEIGHT { get; set; }

        public decimal NUMPRODUCTIONCWWEIGHT { get; set; }
        public decimal NUMPRODUCTIONWEIGHT { get; set; }

        public int INTPINSIZEID { get; set; }
            public int TNTGENERATIONSTATUS { get; set; }
        public int NUMMWSPACING { get; set; }
        public int NUMCWSPACING { get; set; }

        public string PARAMVALUES { get; set; }
        public string NVCHBOMDRAWINGPATH { get; set; }
        public string NVCHMWBVBSSTRING { get; set; }
        public string NVCHCWBVBSSTRING { get; set; }

        public string CHRSHAPECODE { get; set; }
        public string PRODUCEINDICATOR { get; set; }
        public string BOMIND { get; set; }
        public int  INTPRODUCTVALIDATOR        { get; set; }

    
    }
}
