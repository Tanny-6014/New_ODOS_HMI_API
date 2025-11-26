using System.Data.SqlTypes;

namespace DrainService.Dtos
{
    public class DrainProductMarkingDetailsDto
    {
        public int intStructureMarkId { get; set; }
        public int tntStructureMarkRevNo { get; set; }
        public int intProductCodeId { get; set; }
        public string vchProductMarkingName { get; set; }
        public int intShapeCodeId { get; set; }
        public int numInvoiceMWLength { get; set; }
        public int numInvoiceCWLength { get; set; }
        public int intInvoiceMainQty { get; set; }
        public int intInvoiceCrossQty { get; set; }
        public int intInvoiceTotalQty { get; set; }
        public int numProductionMWLength { get; set; }
        public int numProductionCWLength { get; set; }
        public int intProductionMainQty { get; set; }
        public int intProductionCrossQty { get; set; }
        public int intProductionTotalQty { get; set; }
        public int intInvoiceMO1 { get; set; }
        public int intInvoiceMO2 { get; set; }
        public int intInvoiceCO1 { get; set; }
        public int intInvoiceCO2 { get; set; }
        public int intProductionMO1 { get; set; }
        public int intProductionMO2 { get; set; }
        public int intProductionCO1 { get; set; }
        public int intProductionCO2 { get; set; }
        public int intMWSpacing { get; set; }
        public int intCWSpacing { get; set; }
        public int sitPinSize { get; set; }
        public bool bitCoatingIndicator { get; set; }
        public int numConversionFactor { get; set; }
        public string vchShapeSurcharge { get; set; }
        public bool bitBendIndicator { get; set; }
        public string chrCalculationIndicator { get; set; }
        public int tntGenerationStatus { get; set; }
        public bool bitMachineCheckIndicator { get; set; }
        public string vchTransportCheckResult { get; set; }
        public bool bitTransportIndicator { get; set; }
        public string vchBendCheckResult { get; set; }
        //public SqlXml xmlResult { get; set; }
        public string vchFilePath { get; set; }
        public int numInvoiceMWWeight { get; set; }
        public int numInvoiceCWWeight { get; set; }
        public int numInvoiceArea { get; set; }
        public int numTheoraticalWeight { get; set; }
        public int numNetWeight { get; set; }
        public int intMemberQty { get; set; }
        public int numProductionMWWeight { get; set; }
        public int numProductionCWWeight { get; set; }
        public int numProductionWeight { get; set; }
        public string ParamValues { get; set; }
        public string BendingPos { get; set; }
        public int intShapeTransHeaderId { get; set; }
        public int intEnvelopLength { get; set; }
        public int intEnvelopWidth { get; set; }
        public int intEnvelopHeight { get; set; }
        public int intProductMarkId { get; set; }
        public int intDrainStructureMarkID { get; set; }
        public int tntDrainLayerId { get; set; }
        public decimal decStartChainage { get; set; }
        public decimal decEndChainage { get; set; }
        public string vchMWBVBSString { get; set; }
        public string vchCWBVBSString { get; set; }
        public int intuserID { get; set; }
        public string nvchProduceIndicator { get; set; }

    }
}

