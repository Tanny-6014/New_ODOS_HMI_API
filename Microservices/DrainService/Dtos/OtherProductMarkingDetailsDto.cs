using System.Data.SqlTypes;


namespace DrainService.Dtos
{
    public class OtherProductMarkingDetailsDto
    {
        public int intProductMarkId { get; set; }
        public int tntStructureMarkRevNo { get; set; }
        public int intProductCodeId { get; set; }
        public string vchProductMarkingName { get; set; }
        public int intShapeId { get; set; }
        public int MhMwLength { get; set; }
        public int MhCwLength { get; set; }
        public int MainWireNo { get;set;}
        public int CrossWireNo { get;set;}
        public int TotalQty { get; set; }
        public int numProductionMWLength { get; set; }
        public int numProductionCWLength { get; set; }
        public int MhNoMw { get; set; }
        public int MhNoCw { get; set; }
        public int ProductionTotalQty { get; set; }
        public int SwQuantity { get; set; }
        public int MhMo1 { get; set; }
        public int MhMo2 { get; set; }
        public int MhCo1 { get; set; }
        public int MhCo2 { get; set; }
        public int MhProdMo1 { get; set; }
        public int MhProdMo2 { get; set; }
        public int MhProdCo1 { get; set; }
        public int MhProdCo2 { get; set; }
        public int intMWSpacing { get; set; }
        public int intCWSpacing { get; set; }
        public int numProdMwLength { get; set; }
        public int numProdCwLength { get; set; }
        public int MhPinDia { get; set; }
        public int numTheoraticalWeight { get; set; }
        public int numinvoiceArea { get; set; }
        public int numNetWeight { get; set; }
        public int numProductionWeight { get; set; }
        public int numConversionFactor { get; set; }
        public string vchShapeSurcharge { get; set; }
        public string BC { get; set; }
        public string chrCalculationIndicator { get; set; }
        public int tntGenerationStatus { get; set; }
        public string MC { get; set; }
        public string TC { get; set; }
        public string xmlResult { get; set; }
        public string vchFilePath { get; set; }
        public string ParamValues { get; set; }
        public int intShapeTransHeaderId { get; set; }
        public int intEnvelopLength { get; set; }
        public int intEnvelopWidth { get; set; }
        public int intEnvelopHeight { get; set; }
        public string vchProductCode { get; set; }
        public string Shape { get; set; }
        public string vchDrainLayer { get; set; }
        public bool SM_bitBendingCheck { get; set; }
        public bool SM_bitMachineCheck { get; set; }
        public bool SM_bitTransportCheck { get; set; }
        public int intDrainStructureMarkId { get; set; }
       // public SqlXml xmlResult { get; set; }
       public int intMWDia { get; set; }
        public int intCWDia { get; set; }
        public string meshShape { get; set; }
        public string WireDtls { get; set; }
        public string ProduceIndicator { get; set; }
        public string vchMeshtype { get; set; }
        public string bitOHDtls { get; set; }
    }
}						