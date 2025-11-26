namespace DrainService.Dtos
{
    public class InsertProjectDrainParamDetailsDto
    {

        public int intDrainWMId { get; set; }
        public string nvchParamValues { get; set; }//changed string to int
        public int intMo1 { get; set; }
        public int intMo2 { get; set; }
        public string vchCriticalIndicator { get; set; }
        public string vchSequence { get; set; }//changed string to int
    }
}