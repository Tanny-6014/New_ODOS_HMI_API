
namespace DrainService.Repositories
{
  
    public class CABInfo
    {
      
        public byte[] CSM_SHAPE_IMAGE{ get; set; }
        
        public string CSD_ARC_DIR{ get; set; }
        
        public string CSD_MATCH_PAR3{ get; set; }
        
        public string CSD_MATCH_PAR2{ get; set; }
        
        public string CSD_MATCH_PAR1{ get; set; }

        public decimal CSD_PAR3{ get; set; }
        
        public decimal CSD_PAR2{ get; set; }

        public decimal CSD_PAR1{ get; set; }
        
        public string CSD_INPUT_TYPE{ get; set; }
       
        public decimal CSD_TXT_Y{ get; set; }
        
        public decimal CSD_TXT_X{ get; set; }
        

        public decimal CSD_END_POINT_Y{ get; set; }
        
        public decimal CSD_END_POINT_X{ get; set; }
       
        public string CSD_PAR3_CAP { get; set; }
        
        public string CSD_PAR2_CAP { get; set; }
        public string CSD_PAR1_CAP { get; set; }

        public string CSD_TYPE { get; set; }

        public string CSD_SHAPE_ID { get; set; }
        
        public decimal CSD_SEQ_NO { get; set; }


        public string CSC_CAT_DESC { get; set; }
        
        public int CSC_CAT_ID { get; set; }
       
        public string CSM_ACT_INACTIVE { get; set; }
        
        public string CSM_SURCHARGE { get; set; }

        public decimal CSM_SCALE { get; set; }

        public string CSM_UNIT {get; set; }

        public decimal CSM_NO_ARC {get; set; }

        public decimal CSM_NO_BEND { get; set; }

        public decimal CSM_Y_COR { get; set; }
        
        public decimal CSM_X_COR { get; set; }

        public int CSM_CAT_ID { get; set; }

        public string CSM_SHAPE_ID { get; set; }

        public int sitNoOfBends { get; set; }

        public int intStartCoordX { get; set; }

        public int intStartCoordY { get; set; }

        public string ShapeID { get; set; }

        public string createdUser { get; set; }

        public string createdDate { get; set; }
        
        public string TO_SOR { get; set; }

        public string FROM_SOR { get; set; }

        public char chrBBSStatus { get; set; }

        public string ORD_REQ_NO { get; set; }

        public string BBS_NO { get; set; }

        public string CouplerType { get; set; }

        public Int32 intX { get; set; }

        public Int32 intY { get; set; }
        
        public Int32 intZ { get; set; }
        
        public decimal numInvoiceWeightPerPC { get; set; }

        public decimal numExternalWidth { get; set; }

        public decimal numExternalHeight { get; set; }

        public decimal numExternalLength { get; set; }
        
        public decimal numLength { get; set; }
        
        public bool IsbitCoupler { get; set; }

        public string vchCouplerType1 { get; set; }
       
        public string vchCouplerType2 { get; set; }

        public string bitCouplerIndicator { get; set; }

        public decimal numSlabwidth { get; set; }

        public decimal numSlabLength { get; set; }

        public decimal numSlabDepth { get; set; }

        public int intEnvelopewidth { get; set; }

        public int intEnvelopeHeight { get; set; }

        public int intEnvelopeLength { get; set; }

        public string SAPMaterialCode { get; set; }

        public int intDiameter { get; set; }

        public string chrGradeType { get; set; }

        public string chrShapeCode { get; set; }

        public decimal decCWLength { get; set; }

        public decimal intCWSpace { get; set; }

        public decimal decMWLength { get; set; }

        public int intProjectTypeId { get; set; }

        public int SAPMAterialCodeid { get; set; }

        public int ParentStructureId { get; set; }

        public int ParentProductId { get; set; }

        public bool ParentFlag { get; set; }

        public int SEDetailingId { get; set; }

        public string strCWSpacing { get; set; }

        public int intMwLength { get; set; }

        public string strBendResult { get; set; }

        public string strTransportResult { get; set; }

        public int intLinkQty { get; set; }

        public string vchUploadedFiles { get; set; }

        public string vchUploadedFileVersions { get; set; }

        public string vchFileRemarks { get; set; }

        public string vchWBSElementIds { get; set; }

        // ''''
        public string vchSMUploadedFiles { get; set; }

        public string vchSMFileRemarks { get; set; }

        public string vchStructureRevNos { get; set; }

        public string vchStructureVersionNos { get; set; }

        public string vchStructureMarkingIds { get; set; }

        public string strWBSElementId { get; set; }

        public int intNoofLinks { get; set; }

        public int intRowsAtLen { get; set; }

        public int intRowsAtWidth { get; set; }

        public bool bitCLOnly { get; set; }

        public int intCLinkShapeCodeIdAtlen { get; set; }

        public int intClinkShapeCodeIdAtWidth { get; set; }

        public int intClinkQtyAtLen { get; set; }

        public int intClinkQtyAtWidth { get; set; }

        public int intCLinkProdCodeIdAtlen { get; set; }

        public int intClinkProdCodeIdAtWidth { get; set; }

        public decimal numColumnHeight { get; set; }

        public decimal numLinkwidth { get; set; }

        public decimal numLinkLength { get; set; }

        public Int32 intUploadedUid { get; set; }

        // Common Variables
        public string Output { get; set; }

        public string vchInputValueParameter { get; set; }

        public int intRounfOffinput { get; set; }

        // CustomerMaster Def
        public Int32 intContractID { get; set; }

        public string vchContractNumber { get; set; }

        public string vchNDSContractDescription { get; set; }

        public Int32 intCustomerCode { get; set; }

        public string vchMarketSegment { get; set; }

        public string vchSAPContract { get; set; }

        public string vchSAPContractDescription { get; set; }

        public Int32 intSegmentManagerUID { get; set; }

        public Int32 intAccountManagerUID { get; set; }

        public DateTime datStartDate { get; set; }

        public DateTime datEndDate { get; set; }

        public string vchContractCopiedFrom { get; set; }

        public string vchProjectCopiedFrom { get; set; }

        public char chrCABFormerType { get; set; }

        // CustomerMaster Def


        // ProjectParameter Def
        public int tntParamSetNumber { get; set; }

        // Public Property intProjectId() As Int32
        // Get
        // Return _intProjectId
        // End Get
        // Set(ByVal value As Int32)
        // _intProjectId = value
        // End Set
        // End Property
        public string vchProductType { get; set; }

        public string vchProductCode { get; set; }

        public string vchProjectAbbr { get; set; }

        public string vchDescription { get; set; }

        public string vchTransportMode { get; set; }

        public Int32 sitTopCover { get; set; }
       
        public Int32 sitBottomCover { get; set; }

        public Int32 sitLeftCover { get; set; }

        public Int32 sitRightCover { get; set; }

        public Int32 sitGap1 { get; set; }

        public Int32 sitGap2 { get; set; }

        public string chrCLMaterialType { get; set; }

        public decimal numDiameter { get; set; }

        public char chrStandard { get; set; }

        public Int32 sitHook1 { get; set; }

        public Int32 sitHook2 { get; set; }

        public Int32 sitLeg1 { get; set; }

        public Int32 sitLeg2 { get; set; }
        
        public Int32 sitMWAnchor1 { get; set; }

        public Int32 sitMWAnchor2 { get; set; }

        public Int32 sitCWAnchor1 { get; set; }

        public Int32 sitCWAnchor2 { get; set; }

        public Int32 sitSpiralPitch { get; set; }

        public byte tntStatusId { get; set; }

        public Int32 intCreatedUID { get; set; }

        public DateTime datCreatedDate { get; set; }

        public Int32 intUpdatedUID { get; set; }

        public DateTime datUpdatedDate { get; set; }

        // Project Master

        public Int32 intProjectId { get; set; }

        public string vchProjectName { get; set; }

        public string vchSAPProject { get; set; }

        public string vchSAPProjectName { get; set; }

        // Structure Marking

        public Int32 intStructureMarkId { get; set; }

        public Int32 tntStructureRevNo { get; set; }

        public Int32 intDetailingHeaderId { get; set; }

        public Int32 intGroupMarkId { get; set; }

        public Int32 tntGroupRevNo { get; set; }

        public Int32 intConsignmentType { get; set; }

        public string vchStructureMarkingName { get; set; }

        public Int32 intMemberQty { get; set; }

        public bool bitSimilarStructure { get; set; }

        public string vchSimilarTo { get; set; }

        public decimal numBeamWidth { get; set; }

        public decimal numBeamDepth { get; set; }

        public decimal numBeamSlope { get; set; }

        public decimal numCageWidth { get; set; }

        public decimal numCageDepth { get; set; }

        public decimal numCageSlope { get; set; }

        public decimal numColumnWidth { get; set; }

        public decimal numColumnLength { get; set; }

        public decimal numColumnLinkWidth { get; set; }

        public decimal numColumnLinkLength { get; set; }

        public Int32 intClearSpan { get; set; }

        public Int32 intTotalLinks { get; set; }

        public bool bitIsCapping { get; set; }
        
        public string vchColumnCageProductCode { get; set; }

        public string vchColumnCageShapeCode { get; set; }
        
        public bool bitIsCLink { get; set; }

        public Int32 intCLinkRowsAtLength { get; set; }

        public string vchCLinkProductCodeAtLength { get; set; }

        public Int32 intCLinkRowAtWidth { get; set; }

        public string vchCLinkProductCodeAtWidth { get; set; }

        public string CLinkShapeCode { get; set; }

        public string vchSlabWallProductCode { get; set; }

        public decimal numTotalMeshMainLength { get; set; }

        public decimal numTotalMeshCrossLength { get; set; }

        public decimal numArea { get; set; }

        public Int32 intTotalQty { get; set; }
       
        public Int32 intTotalBend { get; set; }

        public decimal numTheoreticTonnage { get; set; }

        public decimal numNetTonnage { get; set; }

        public bool bitBendingCheck { get; set; }

        public bool bitCoat { get; set; }

        public bool bitMachineCheck { get; set; }

        public bool bitTransportCheck { get; set; }

        public bool bitAssemblyIndicator { get; set; }

        public string vchDrawingReference { get; set; }

        public string chrDrawingVersion { get; set; }

        public string vchDrawingRemarks { get; set; }

        // 'new
        public Int32 intTotalStirrups { get; set; }

        public Int32 intBeamProductCodeId { get; set; }

        public Int32 intBeamShapeId { get; set; }

        public Int32 intCappingProductCodeId { get; set; }

        public Int32 intCappingShapeCodeId { get; set; }

        public string vchTactonConfigurationState { get; set; }

        // WBSElements
        public Int32 intWBSElementId { get; set; }

        public Int32 intWBSId { get; set; }

        public Int32 vchWBS1 { get; set; }

        public string vchWBS2 { get; set; }

        public string vchWBS3 { get; set; }

        public string vchWBS4 { get; set; }

        public string vchWBS5 { get; set; }

        // '$$$
        public int intWBSTypeId { get; set; }

        public int intUserid { get; set; }


        // WBS
        public string vchWBSType { get; set; }

        // Product Type Master

        public Int32 sitProductTypeId { get; set; }
        
        public Int32 intStructureElementTypeId { get; set; }

        // Structure Element Master

        public string vchStructureElementType { get; set; }

        // ProductCABDetails
        public Int32 intCABId { get; set; }

        public string vchBarMark { get; set; }

        public string vchGDia { get; set; }

        public Int32 intNoOfMBars { get; set; }

        public Int32 intNoEach { get; set; }

        public Int32 intShapeId { get; set; }
        
        public Int32 sitPinSize { get; set; }

        public decimal numCutLength { get; set; }

        public decimal numInvoiceLength { get; set; }

        public decimal numCutWeight { get; set; }

        public Int32 intBBSPage { get; set; }

        public string vchRemarks { get; set; }

        // ProductAccessories
        public Int32 intAccessoriesId { get; set; }

        public Int32 intItemNo { get; set; }

        public string vchMark { get; set; }

        public Int32 intQty { get; set; }

        public string vchUOM { get; set; }

        public decimal numWeight { get; set; }

        public string vchBillType { get; set; }

        public string vchShape { get; set; }

        // GroupMarking Details    
        public string vchGroupMarkingName { get; set; }

        public int intProductQty { get; set; }

        public string vchCopyFrom { get; set; }

        // 'Product Marking   

        public Int32 intProductMarkId { get; set; }

        public string vchProductMarkingName { get; set; }

        public decimal numInvoiceMWLength { get; set; }

        public decimal numInvoiceCWLength { get; set; }

        public Int32 intShapeCodeId { get; set; }

        public Int32 intProductCode { get; set; }

        public Int32 tntShapeCodeRevNo { get; set; }

        public Int32 intCutQty { get; set; }

        public Int32 intInvoiceMWQty { get; set; }

        public Int32 intInvoiceCWQty { get; set; }

        public decimal numInvoiceMWWeight { get; set; }

        public decimal numInvoiceCWWeight { get; set; }

        public decimal numInvoiceArea { get; set; }

        public decimal numInvoiceWeight { get; set; }

        public decimal numNetWeight { get; set; }

        public Int32 intMO1 { get; set; }

        public Int32 intMO2 { get; set; }

        public Int32 intCO1 { get; set; }

        public Int32 intCO2 { get; set; }

        public Int32 intProductionMO1 { get; set; }

        public Int32 intProductionMO2 { get; set; }

        public Int32 intProductionCO1 { get; set; }

        public Int32 intProductionCO2 { get; set; }

        public decimal numProductionMWLength { get; set; }

        public decimal numProductionCWLength { get; set; }

        public Int32 intProductionMWQty { get; set; }

        public Int32 intProductionCWQty { get; set; }

        public decimal numProductionMWWeight { get; set; }

        public decimal numProductionCWWeight { get; set; }

        public decimal numProductionWeight { get; set; }

        public Int32 intPinSizeId { get; set; }

        public bool bitCoatingIndicator { get; set; }
        
        public decimal numConversionFactor { get; set; }

        public string vchShapeSurcharge{ get; set; }
       
        public bool bitBendIndicator { get; set; }

        public bool bitShiftIndicator { get; set; }

        public string chrCalculationIndicator { get; set; }

        public Int32 tntGenerationStatus { get; set; }

        public bool bitMachineCheckIndicator { get; set; }

        public string vchTransportCheckResult { get; set; }

        public decimal numMWSpacing { get; set; }

        public decimal numCWSpacing { get; set; }

        public string ParamValues { get; set; }
        
        public string BendingPos { get; set; }

        public decimal numTheoraticalWeight { get; set; }

        // ProductCodeMaster
        public Int32 intProductCodeId { get; set; }

        // ShapeCode
        public int intShapeTransHeaderId { get; set; }

        public string vchShapeDescription { get; set; }

        public string nvchParamValues { get; set; }

        // Added by KarthikA
        public Int32 intMeshProductMarkId { get; set; }

        public Int32 tntStructureMarkRevNo { get; set; }

        public Int32 intInvoiceTotalQty { get; set; }

        public Int32 intProductionMainQty { get; set; }

        public Int32 intProductionCrossQty { get; set; }

        public Int32 intProductionTotalQty { get; set; }
        
        public Int32 intInvoiceMO1 { get; set; }

        public Int32 intInvoiceMO2 { get; set; }

        public Int32 intInvoiceCO1 { get; set; }

        public Int32 intInvoiceCO2 { get; set; }
        
        public Int32 intMWSpacing { get; set; }

        public Int32 intCWSpacing { get; set; }

        public bool bitTransportIndicator { get; set; }

        public string vchBendCheckResult { get; set; }

        public string xmlResult { get; set; }

        public string vchFilePath { get; set; }

        // Slab Wall
        public decimal decTotalMeshMainLength { get; set; }

        public decimal decTotalMeshCrossLength { get; set; }

        public Int32 intInvoiceMainQty { get; set; }

        public Int32 intInvoiceCrossQty { get; set; }

        public decimal decMWDiameter { get; set; }

        public decimal decCWDiameter { get; set; }
       
        public decimal intParamSetID { get; set; }

        // bore pile structure marking variables
        public Int32 tntNumOfLinksatStart { get; set; }

        public Int32 tntNumOfLinksatEnd { get; set; }

        public Int32 tntAdditionalSpiral { get; set; }

        public Int32 intParameterSet { get; set; }

        public Int32 intSELevelDetailsID { get; set; }

        public string vchMainBarDia { get; set; }
        public string vchLinkDia { get; set; }

        public string vchLinkPitch { get; set; }

        public string vchParameterType { get; set; }

        public bool bitStructureMarkingLevel { get; set; }

        public Int32 intTransportId { get; set; }

        public Int32 intMainBarSize { get; set; }

        public Int32 intMainBarLength { get; set; }

        public Int32 intSpiralSize { get; set; }

        public Int32 intSpiralLength { get; set; }

        public Int32 intSpiralPitch { get; set; }

        public Int32 intMainBarId { get; set; }
       
        public Int32 intElevationId { get; set; }

        public Int32 intCageSeqNo { get; set; }

        public Int32 numPileDia { get; set; }

        public Int32 numCageLength { get; set; }

        public string vchFabricationType { get; set; }

        public string vchMainBarPattern { get; set; }

        public string vchElevationPattern { get; set; }

        public Int32 tntNumOfMainBar { get; set; }

        public Int32 tntNumOfSpiralLink { get; set; }

        public Int32 tntNumberOfStiffnerOrCentralizer { get; set; }

        public Int32 tntNumOfSquareStiffner { get; set; }

        public Int32 tntNumOfCircularRings { get; set; }

        public int intLapLength { get; set; }

        public int intEndLength { get; set; }

        public int intCoverToLink { get; set; }

        public int intAjustmentFactor { get; set; }

        public string vchCageNote { get; set; }

        public int intMachineTypeId { get; set; }

        public Int32 numStructNetWeight { get; set; }

        public Int32 numActualWeight { get; set; }

        public string vchTactonXmlResult { get; set; }

        public bool bitGenerate3DforPreview { get; set; }

        public string vch3DImageRef { get; set; }

        public string vchOutputDrawingRef { get; set; }

        // BORE PILE PRODUCT MARKING
        public string chrGenerationStatus { get; set; }

        public Int32 tntLayer { get; set; }

        public Int32 intCABProductMarkID { get; set; }

        public bool bitMachineIndicator { get; set; }

        public int intTransport { get; set; }

        // Added by panchatapa for CHM114465630 (Shape Group)
        public string ShapeGroup { get; set; }

        public int Coupler { get; set; }

        public int Stud { get; set; }

        public int Thread { get; set; }

        public int Locknut { get; set; }

        public string ShapePrefix { get; set; }

        public int Flag { get; set; }

      
    }
}
