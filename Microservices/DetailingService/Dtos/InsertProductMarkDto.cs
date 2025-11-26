using DetailingService.Repositories;


namespace DetailingService.Dtos
{
    public class InsertProductMarkDto
    {
        public string VCHCABPRODUCTMARKNAME { get; set; }
        public int INTSEDETAILINGID { get; set; }
        public int INTGROUPMARKID { get; set; }
        public int INTMEMBERQTY { get; set; }
        public string INTSHAPECODE { get; set; }
        public int INTPINSIZEID { get; set; }
        public int NUMINVOICELENGTH { get; set; }
        public int NUMPRODUCTIONLENGTH { get; set; }
        public int NUMINVOICEWEIGHT { get; set; }
        public int NUMPRODUCTIONWEIGHT { get; set; }
        public string GRADE { get; set; }
        public int INTDIAMETER { get; set; }
        public string BARSTANDARD { get; set; }
        public string VCHSHAPETYPE { get; set; }
        public string VCHSHAPEGROUP { get; set; }
        public string COUPLERTYPE1 { get; set; }
        public string COUPLERMATERIAL1 { get; set; }
        public string COUPLERSTANDARD1 { get; set; }
        public string COUPLERTYPE2 { get; set; }
        public string COUPLERMATERIAL2 { get; set; }
        public string COUPLERSTANDARD2 { get; set; }
        public string INTSTATUS { get; set; }
        public string VCHCUSTREMARKS { get; set; }
        public string VCHSHAPEIMAGE { get; set; }
        public string VCHCAB_BVBS { get; set; }
        public string VCHPAGENUMBER { get; set; }
        public string VCHCOMMDESCRIPT { get; set; }
        public int NUMENVLENGTH { get; set; }
        public int NUMENVWIDTH { get; set; }
        public int NUMENVHEIGHT { get; set; }
        public int INTNOOFBENDS { get; set; }
        public bool BITVARIESBAR { get; set; }

        List<ShapeParameter> ShapeParamList    { get; set; }

        public ShapeCode Shape { get; set; }


    }
}





