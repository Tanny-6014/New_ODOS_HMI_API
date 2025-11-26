namespace DrainService.Dtos
{
    public class GetProjectParamDrainLapDto
    {         
        public int intDrainLapId { get; set; }
        public int tntParamSetNumber { get; set; }
        public string vchProductCode { get; set; }
        public int intProductCodeId { get; set; }
        public int sitLap { get; set; }
        public bool bitConfirm { get; set; }

        public int intUserId { get; set; }
    }
}
