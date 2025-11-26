
namespace DetailingService.Dtos
{
    public class SlabStructureMarkingDto
    {
       public int INTSTRUCTUREMARKID { get; set; }
        public int INTPRODUCTCODEID { get; set; }
        public string VCHSTRUCTUREMARKINGNAME { get; set; }
        public int TNTPARAMSETNUMBER { get; set; }
        public decimal DECTOTALMESHMAINLENGTH { get; set; }
        public decimal DECTOTALMESHCROSSLENGTH { get; set; }
        public int INTMEMBERQTY { get; set; }
        public byte BITBENDINGCHECK { get; set; }
        public byte BITMACHINECHECK { get; set; }
        public byte BITTRANSPORTCHECK { get; set; }
        public string VCHPRODUCTCODE { get; set; }
        public string BITSINGLEMESH { get; set; }
        public string PRODUCEINDICATOR { get; set; }
        public int SITPINSIZE { get; set; }
        public int SIDEFORCODE { get; set; }
        public int PRODUCTSPLITUP { get; set; }
        public int ProductGenerationStatus { get; set; }
    }
}
