namespace UtilityService.Dtos
{
    public class SlabProductCodeDto
    {

        public int INTPRODUCTCODEID { get; set; }
        public string?  VCHPRODUCTCODE { get; set; }
        public string? VCHPRODUCTDESCRIPTION { get; set; }
        public int INTMWSPACE { get; set; }
        public int INTCWSPACE { get; set; }
        public decimal  DECWEIGHTAREA   { get; set; }
        public decimal  DECMWDIAMETER { get; set; }
        public decimal DECCWDIAMETER { get; set; }

        public decimal DECWEIGTHPERMETERRUN { get; set; }

        public decimal DECCWWEIGTHPERMETERRUN { get; set; }
        public int? intSAPMaterialCodeId { get; set; }
        public int intMaxLinkFactor { get; set; }

        public int intMinLinkFactor { get; set; }
        public string vchMaterialNumber { get; set; }

        


    }
}
