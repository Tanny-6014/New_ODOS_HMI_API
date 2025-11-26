
namespace DetailingService.Dtos
{
    public class Get_bomDto
    {
        public int intDetailingBOMDetailId { get; set; }
        public bool bitEnabled { get; set; }

        public string vchWireType { get; set; }

        public int tntWireSequence { get; set; }

        public decimal decStartPosition { get; set; }

        public int tntNoOfPitch { get; set; }
        public int intWirePitch { get; set; }
        public decimal decWireLength { get; set; }

        public decimal decWireDiameter { get; set; }

        public string vchRawMaterial { get; set; }
        public string WireSpec { get; set; }

        public int tntRepetitionFrom { get; set; }

        public int tntRepetitionTo { get; set; }

        public int tntRepetitionNumber { get; set; }

        public int bitTwinInd { get; set; }
        public string BOMType { get; set; }
        public string chrBOMIndicator { get; set; }

        public string nvchParamWire { get; set; }





    }
}
