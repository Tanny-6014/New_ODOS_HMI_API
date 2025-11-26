namespace DetailingService.Dtos
{
    public class BOMInsert{
        public int intProductMarkingId { get; set; }
        public int intDetailingBOMDetailId { get; set; }
        public int intMO1 { get; set; }
        public int intMO2 { get; set; }
        public int intCO1 { get; set; }
        public int intCO2 { get; set; }



        public string strStructureElement { get; set; }

        public string BomType { get; set; }
        public string chrWireType { get; set; }
        public string strLineNo { get; set; }
        public string strStartPos { get; set; }
        public string strNoPths { get; set; }
        public string strWireSpace { get; set; }
        public string strWireLen { get; set; }
        public string strWireDia { get; set; }
        public string strRawMaterial { get; set; }
        public string strRepFrom { get; set; }
        public string strRepTo { get; set; }
        public string strRep { get; set; }
        public string bitTwinWire { get; set; }
        public string intUserId { get; set; }
    }
}
