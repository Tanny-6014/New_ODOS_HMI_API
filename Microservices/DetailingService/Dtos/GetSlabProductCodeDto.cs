namespace DetailingService.Dtos
{
    public class GetSlabProductCodeDto
    {
        public int INTPRODUCTCODEID { get; set; }
        public string VCHPRODUCTCODE { get; set; }
        public string VCHPRODUCTDESCRIPTION { get; set; }
        public int INTMWSPACE { get; set; }
        public int INTCWSPACE { get; set; }
        public double DECWEIGHTAREA { get; set; }
        public double DECMWDIAMETER { get; set; }
        public double DECCWDIAMETER { get; set; }
        public double DECWEIGTHPERMETERRUN { get; set; }
        public double DECCWWEIGTHPERMETERRUN { get; set; }
    }
}