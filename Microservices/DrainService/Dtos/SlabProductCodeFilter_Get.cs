using System.Drawing.Drawing2D;

namespace DrainService.Dtos
{
    public class SlabProductCodeFilter_Get
    {

        public int INTPRODUCTCODEID { get; set; }

        public int intMaxLinkFactor { get; set; }

        public int DECCWDIAMETER { get; set; }

        public decimal DECMWDIAMETER { get; set; }
        public decimal DECWEIGHTAREA { get; set; }
        public int INTCWSPACE { get; set; }
        public int INTMWSPACE { get; set; }
        public string VCHPRODUCTDESCRIPTION { get; set; }
        public string VCHPRODUCTCODE { get; set; }
        public int intMinLinkFactor { get; set; }
        public int intSAPMaterialCodeId { get; set; }


        public string vchMaterialNumber { get; set; }

        public decimal DECWEIGTHPERMETERRUN { get; set; }

        public decimal DECCWWEIGTHPERMETERRUN { get; set; }



         

    }
}
