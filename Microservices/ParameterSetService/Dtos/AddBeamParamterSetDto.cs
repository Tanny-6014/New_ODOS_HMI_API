namespace ParameterSetService.Dtos
{
    public class AddBeamParamterSetDto
    {
        public int? ProjectId { get; set; }
        public string? ProductType { get; set; }       
        public int? BeamParameterSetNo { get; set; }
        public int? BeamTopCover { get; set; }
        public int? BeamBottonCover { get; set; }
        public int? BeamRightCover { get; set; }

        public int? BeamLeftCover { get; set; }
        public int? BeamHook { get; set; }
        public int? BeamLeg { get; set; }
        public int? UserId { get; set; }
        public int? TNTTRANSPORTMODEID { get; set; }
        

    }                 
}
