namespace DrainService.Dtos
{
    public class GetBorePilePopulateMethodsDto
    {
        //FABMETHODS
        public int intFabMethodID { get; set; }
        public string vchDescription { get; set; }
        public string vchFabMethod { get; set; }

        // for ELEVATIONPATTERN
        public int intElevationPatternID { get; set; }
        public string vchElevationCode { get; set; }

        //for MAINBARPATTERN
        public int intMainBarPatternID { get; set; }
        public string vchMainBarPatternCode { get; set; }

        // for MACHINETYPE
        public int tntMachineTypeId { get; set; }

        //SIZETYPES
        public int intCABProductCodeID { get; set; }

        public string vchSizeTypes { get; set; }



    }
}
