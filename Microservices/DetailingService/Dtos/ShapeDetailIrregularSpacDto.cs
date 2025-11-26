namespace DetailingService.Dtos
{
    public class ShapeDetailIrregularSpacDto
    {
        public int INTSHAPEID { get; set; }
        public string VCHSHAPECODE { get; set; }
        public bool BITSHAPEPOPUP { get; set; }
        public bool BITISCAPPING { get; set; }
        public int NOOFBENDS { get; set; }
        public int INTMWBENDPOSITION { get; set; }
        public int INTCWBENDPOSITION { get; set; }
        public int INTNOOFMWBEND { get; set; }
        public int INTNOOFCWBEND { get; set; }
        public string VCHCWBVBSTEMPLATE { get; set; }
        public string VCHMWBVBSTEMPLATE { get; set; }
        public List<ShapeDetailIrregularSpac_DetailsDto> ShapeDetailIrregularSpac_Details { get; set; } = new List<ShapeDetailIrregularSpac_DetailsDto>();


    }
}
 																