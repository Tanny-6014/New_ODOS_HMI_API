using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Globalization;


namespace DrainService.Constants
{
    public class SystemConstants
    {
        public static string DefaultDBConnection = "DefaultConnection";
        public static string DSSBOMPDF = "DSSBOMPDF";
        public static string GetUserID = "GetUserID";
        public static string ProjectParameter_Insert = "ProjectParameter_Insert";
        public static string ProjectParamDrainLap_Insert = "ProjectParamDrainLap_Insert";
        public static string ProjectParameterDrainLap_Get = "ProjectParameterDrainLap_Get";
        public static string ProjectParamDrainLap_Delete = "ProjectParamDrainLap_Delete";
        public static string ProductCodeDrain_Get = "ProductCodeDrain_Get";
        public static string ProjectParamDrainOH_Insert = "ProjectParamDrainOH_Insert";
        public static string ProjectParamDrainOH_Delete = "ProjectParamDrainOH_Delete";
        public static string ProjectParameterDrainDepth_Get = "ProjectParameterDrainDepth_Get";
        public static string ProjectParamDrainDepth_Insert = "ProjectParamDrainDepth_Insert";
        public static string ProjectParamDrainDepth_Delete = "ProjectParamDrainDepth_Delete";
        public static string ProjectParameterDrainTypeParamNoGiven_Get = "ProjectParameterDrainTypeParamNoGiven_Get";
        public static string ProjectParameterDrainType_Get = "ProjectParameterDrainType_Get";
        public static string ProjectParameterDrainWidthParamNoGiven_Get = "ProjectParameterDrainWidthParamNoGiven_Get";
        public static string ProjectParameterDrainWM_Get = "ProjectParameterDrainWM_Get";
        public static string ProjectParamDrainWM_Insert = "ProjectParamDrainWM_Insert";
        public static string ProjectParamDrainWM_Delete = "ProjectParamDrainWM_Delete";
        public static string ProjectParameterDrainLayer_Get = "ProjectParameterDrainLayer_Get";
        public static string ProjectParameterDrainMaxDepths_Get = "ProjectParameterDrainMaxDepths_Get";
        public static string ProjectParameterDrainParamDetails_Get = "ProjectParameterDrainParamDetails_Get";
        public static string ProjectParameterDrainShape_Get = "ProjectParameterDrainShape_Get";
        public static string DrainShapeCode_Get = "DrainShapeCode_Get";
        public static string DrainParameterSetDetails_Insert = "DrainParameterSetDetails_Insert";
        public static string DrainProductCodeDetails_Get = "DrainProductCodeDetails_Get";
        public static string ProjectParameterDrainWidths_Get = "ProjectParameterDrainWidths_Get";
        public static string ProjectParameterDrainDepths_Get = "ProjectParameterDrainDepths_Get";
        public static string ShapeParam_Get = "ShapeParam_Get";
        public static string ShapeParam_InsUpd = "ShapeParam_InsUpd";
        public static string ShapeParam_delete = "ShapeParam_delete";
        public static string SaveStructureDimension = "SaveStructureDimension";
        public static string ShapeDetails_InsUpd = "ShapeDetails_InsUpd";
        public static string ShapeCodeDetails_Get = "ShapeCodeDetails_Get";
        public static string CarrierWireMaster_InsUpd = "CarrierWireMaster_InsUpd";
        public static string CarrierWireMaster_select = "CarrierWireMaster_select";
        public static string CarrierWireMaster_Del = "CarrierWireMaster_Del";
        public static string ProjectParameterDrainOH_Get = "ProjectParameterDrainOH_Get";
        public static string ProductCodeDrainLap_Get = "ProductCodeDrainLap_Get";
        public static string PrefWireLen_get = "PrefWireLen_get";
        public static string ProjectParameterDrainCheckDepthLap_Get = "ProjectParameterDrainCheckDepthLap_Get";
        public static string PrefWireLen_InsUpd = "PrefWireLen_InsUpd";
        public static string PrefWireLen_Del = "PrefWireLen_Del";
        public static string ParameterSet_Get_PV = "ParameterSet_Get_PV";
        public static string ProjectParameter_Get = "ProjectParameter_Get";
        public static string ShapeCode_Insert = "ShapeCode_Insert";
        public static string GetParameterList_Drain = "GetParameterList_Drain";
        public static string DrainProductMarkingDetails_InsUpd = "DrainProductMarkingDetails_InsUpd";



        public static string DrainParamDepthValues_Get_New="DrainParamDepthValues_Get_New";
        public static string DrainProductMarkingDetails_Del = "DrainProductMarkingDetails_Del";
        public static string DrainWireDiameter_Get = "DrainWireDiameter_Get";
        public static string ProjectParameterDrainDetails_Get_New = "ProjectParameterDrainDetails_Get_New";
        public static string DrainGroupMark_Check = "DrainGroupMark_Check";
        public static string SWShapeCode_Get_Drain = "SWShapeCode_Get_Drain";
        public static string CABACC_ProductMarkingDetails_Delete = "CABACC_ProductMarkingDetails_Delete";
        public static string OthProductCodeDrain_Get = "OthProductCodeDrain_Get";
        public static string OthProductCode_Get = "OthProductCode_Get";
        public static string OthDrainOverHangs_Get = "OthDrainOverHangs_Get";
        public static string Oth_DrainProductMarkingDetails_Update = "Oth_DrainProductMarkingDetails_Update";
        public static string Oth_DrainProductMarkingDetails_Insert = "Oth_DrainProductMarkingDetails_Insert";
        public static string DrainOth_ShapeCodeDetails_Insert = "DrainOth_ShapeCodeDetails_Insert";
        public static string DrainOth_ProductMarking_Get = "DrainOth_ProductMarking_Get";
        public static string WBSAttachedWithGroupMarking_Get = "WBSAttachedWithGroupMarking_Get";
        public static string DrainParamReport_Get = "DrainParamReport_Get";
        public static string PRCEnvelopeDetailsMaster_Get = "PRCEnvelopeDetailsMaster_Get";
        public static string BCDRndOff_Get = "BCDRndOff_Get";
        public static string BCDTcTransChTCMaster_Get = "BCDTcTransChTCMaster_Get";
        public static string DrainOverHangs_Get = "DrainOverHangs_Get";
        public static string DrainProductMarkingDetails_Update = "DrainProductMarkingDetails_Update";
        public static string MachineCheck_Get = "MachineCheck_Get";
        public static string TMCProgamInput_Get = "TMCProgamInput_Get";
        public static string DrainParamDepthValues_Get = "DrainParamDepthValues_Get";
        public static string DrainProductMarking_Get = "DrainProductMarking_Get";
        public static string GetProductMarkingFormulaForShape = "GetProductMarkingFormulaForShape";
        public static string GetMWLengthFormula = "GetMWLengthFormula";
        public static string GetInputOutputValidationForShape = "GetInputOutputValidationForShape";
        public static string GetShapeParametersForShapeId = "GetShapeParametersForShapeId";
        public static string GetStructureMarkingFormulaeElement = "GetStructureMarkingFormulaeElement";
        public static string GetDrainProductMarkingFormuale = "GetDrainProductMarkingFormuale";

        public static string ProductCodeForBeam_Get = "usp_ProductCodeForBeam_Get";
        public static string FilterProductCodeForBeam_Get = "usp_FilterProductCodeForBeam_Get";
        public static string CapProductForBeam_Get = "usp_CapProductForBeam_Get";
        public static string FilterCapProductForBeam_Get = "usp_FilterCapProductForBeam_Get";
        public static string ColumnProductCode_Get = "usp_ColumnProductCode_Get";
        public static string ColumnProductCodeBind_Get = "usp_ColumnProductCodeBind_Get";
        public static string ColumnCLinkProduct_Get = "usp_ColumnCLinkProduct_Get";
        public static string ColumnCLinkProductBind_Get = "usp_ColumnCLinkProductBind_Get";
        public static string SlabProductCode_Get = "usp_SlabProductCode_Get";
        public static string SlabProductCodeFilter_Get = "usp_SlabProductCodeFilter_Get";

        public static string DrainParamInfo_Get = "DrainParamInfo_Get";

        public static string CarpetProductCode_Get = "usp_CarpetProductCode_Get";
        public static string CarpetProductCodeFilter_Get = "usp_CarpetProductCodeFilter_Get";
        public static string CapProductCode_Cache_Get = "usp_CapProductCode_Cache_Get";
        public static string CapProductCodeNoFilter_Cache_Get = "usp_CapProductCodeNoFilter_Cache_Get";
        public static string ClinkProductCode_Get = "usp_ClinkProductCode_Get";
        public static string ClinkProductCodeNoFilter_Get = "usp_ClinkProductCodeNoFilter_Get";
        public static string DrainStructureMarking_Get = "DrainStructureMarking_Get";
        public static string Updt_DrainProductValidator_byprdmrkid = "Updt_DrainProductValidator_byprdmrkid";
        public static string DrainStructureMarking_InsUpd = "DrainStructureMarking_InsUpd";
        public static string DrainStructureMarking_Del = "DrainStructureMarking_Del";
        public static string DrainParameterInfoByPrjID_Get = "DrainParameterInfoByPrjID_Get";
        public static string BCDChkStructElementExist_Get = "BCDChkStructElementExist_Get";
        public static string BCDHeaderInfo_Get = "BCDHeaderInfo_Get";
        public static string DrainProductMarkingID_Get = "DrainProductMarkingID_Get";
        public static string BCDGroupMark_Get = "BCDGroupMark_Get";
        public static string BCDChkGrpMarkExist_Get = "BCDChkGrpMarkExist_Get";
        public static string BeamGroupMarkingDetails_Get = "BeamGroupMarkingDetails_Get";
        public static string BCDGroupMarking_Insert = "BCDGroupMarking_Insert";
        public static string BCDWBSElementByGroupMarkId_Get = "BCDWBSElementByGroupMarkId_Get";
        public static string BCDWBSListColumn_Get = "BCDWBSListColumn_Get";
        public static string BCDWBSList_Get = "BCDWBSList_Get";
        public static string BCDWBSDetails_Insert = "BCDWBSDetails_Insert";
        public static string BCDWBSList_Delete = "BCDWBSList_Delete";
        //BOMDal
        public static string BOMHeader_Select = "BOMHeader_Select";
        public static string BOM_Get = "BOM_Get";
        public static string RawMaterial_Get = "RawMaterial_Get";
        public static string BOM_Insert = "BOM_Insert";
        public static string BOM_Delete = "BOM_Delete";
        public static string BOMHeaderDetails_Update = "BOMHeaderDetails_Update";
        public static string WireType_Get = "WireType_Get";
        public static string BOMData_Check = "BOMData_Check";
        public static string WireDiameter_Get = "WireDiameter_Get";
        public static string BOMDrawingHeader_Get = "BOMDrawingHeader_Get";
        public static string usp_IsBendingCheckRequiredForProductionBOM_Get = "usp_IsBendingCheckRequiredForProductionBOM_Get";
        public static string GeneratePrdBOMFromDetailing_Insert = "GeneratePrdBOMFromDetailing_Insert";
        public static string BOMEditStatus_Get = "BOMEditStatus_Get";
        public static string BendType_Get = "BendType_Get";
        public static string BendingCheck_Select = "BendingCheck_Select";
        public static string OverHang_Insert = "OverHang_Insert";
        public static string SpaceShift_Insert = "SpaceShift_Insert";
        public static string WireRemovalPass_Insert = "WireRemovalPass_Insert";
        public static string WireRemovalFail_Insert = "WireRemovalFail_Insert";
        public static string BCProdMWLength_Select = "BCProdMWLength_Select";
        public static string UserRights_Validate = "UserRights_Validate";
        public static string BOMPdf_Get = "BOMPdf_Get";
        public static string BOMPdfPath_Update = "BOMPdfPath_Update";

        //CABItem
        public static string USP_GET_SEDETAILING_CUBE = "USP_GET_SEDETAILING_CUBE";
        public static string USP_GET_SHAPE_CORD_CUBE = "USP_GET_SHAPE_CORD_CUBE";
        public static string USP_CABPRODUCTMARKINGDETAILS_COPY_CUBE = "USP_CABPRODUCTMARKINGDETAILS_COPY_CUBE";
        public static string USP_GET_CABPINBYFORMER_CUBE = "USP_GET_CABPINBYFORMER_CUBE";
        public static string USP_CABPRODUCTMARKINGDETAILS_DELETE_CUBE = "USP_CABPRODUCTMARKINGDETAILS_DELETE_CUBE";
        public static string USP_CABDETAILS_DELETE_CUBE = "USP_CABDETAILS_DELETE_CUBE";
        public static string USP_GET_COUPLER_DATA_CUBE = "USP_GET_COUPLER_DATA_CUBE";
        public static string sp_Check_IsCustomInvCutLength = "sp_Check_IsCustomInvCutLength";
        //shapeparameter
        public static string CubeShapeParameter_Insert_forGm = "usp_CubeShapeParameter_Insert_forGm";
        public static string CABSHAPEPARAMETER_PARALLEL_INSERT = "USP_CABSHAPEPARAMETER_PARALLEL_INSERT";
        public static string CubeShapeParameter_GET_forgm = "usp_CubeShapeParameter_GET_forgm";

        //accessory
        public static string Accessories_Get = "usp_AccProductMarkDetailsBySEDetailingID_Get";
        public static string Accessories_Insert = "usp_AccProductMarkingDetails_InsUpd";
        public static string Accessories_Delete = "usp_ACCProductMarkDetails_Del";

        //shapecode
        public static string ShapeCodeForBeam_Get = "usp_ShapeCodeForBeam_Get";
        public static string FilterShapeCodeForBeam_Get = "usp_FilterShapeCodeForBeam_Get";
        public static string ColumnShapeCode_Get = "usp_ColumnShapeCode_Get";
        public static string ColumnShapeCodeBind_Get = "usp_ColumnShapeCodeBind_Get";
        public static string SlabShapeCode_Get = "usp_SlabShapeCode_Get";
        public static string SlabShapeCodeFilter_Get = "usp_SlabShapeCodeFilter_Get";
        public static string ShapeCode_Get = "usp_ShapeCode_Get";
        public static string ShapeCodeNoFilter_Get = "usp_ShapeCodeNoFilter_Get";
        public static string ShapeCode_Get_Clink = "usp_ShapeCode_Get_Clink";
        public static string ShapeCodeNoFilter_Get_Clink = "usp_ShapeCodeNoFilter_Get_Clink";
        public static string ShapeCodeDetailsByProductMarkId_Get = "usp_ShapeCodeDetailsByProductMarkId_Get";
        public static string FILTERSHAPECODEFORCAB_GET = "USP_FILTERSHAPECODEFORCAB_GET";
        public static string allcabshapecodes_get = "allcabshapecodes_get";


        //ShapeParameterCollection

        public static string ShapeParameterForBeam_Get = "usp_ShapeParameterForBeam_Get";
        public static string ColumnShapeParameter_Get = "usp_ColumnShapeParameter_Get";
        public static string SlabShapeParameter_Get = "usp_SlabShapeParameter_Get";
        public static string GET_SHAPEPARAMETER_CUBE = "USP_GET_SHAPEPARAMETER_CUBE";
        public static string SHAPEPARAMETERFORCAB_GET_CUBE = "USP_SHAPEPARAMETERFORCAB_GET_CUBE";
        public static string SHAPEPARAMETERFORCAB_EDIT_CUBE = "USP_SHAPEPARAMETERFORCAB_EDIT_CUBE";
        public static string GET_SHAPEPARAMETER_VARLENGTH_CUBE = "USP_GET_SHAPEPARAMETER_VARLENGTH_CUBE";
        public static string SHAPEPARAMFORCAB_GET_CUBE = "USP_SHAPEPARAMFORCAB_GET_CUBE";
        public static string CustContProjByGroupID_Get = "usp_CustContProjByGroupID_Get";

        //cabcalculation 
        public static string USP_GET_PRODUCTIONLENGTH_FORMULA_CUBE = "USP_GET_PRODUCTIONLENGTH_FORMULA_CUBE";

        //BPC
        public static string BPCStructureMarking_Delete = "BPCStructureMarking_Delete";
        public static string BPCParameterInfoByPrjID_Get = "BPCParameterInfoByPrjID_Get";
        public static string BPCPopulateDetails_Get = "BPCPopulateDetails_Get";
        public static string BPCPopulateCageNotes_Get = "BPCPopulateCageNotes_Get";
        public static string BPCCovercodeForStrucMarking_Get = "BPCCovercodeForStrucMarking_Get";
        public static string BPCMaterialCodeID_Get = "BPCMaterialCodeID_Get";
        public static string BPCStructureMarking_Insert = "BPCStructureMarking_Insert";
        public static string BPCStructureMarking_Update = "BPCStructureMarking_Update";
        public static string usp_BorePileParameterSet_Get = "usp_BorePileParameterSet_Get";
        public static string usp_BorePileParameterSet_InsUpd_PV = "usp_BorePileParameterSet_InsUpd_PV";
        public static string usp_BorePileProductCode_Get = "usp_BorePileProductCode_Get";
        public static string BPCStructureMarking_Get = "BPCStructureMarking_Get";
        public static string BPCMainBarPatternDetails_Get = "BPCMainBarPatternDetails_Get";
        public static string BPCElevationPatternDetails_Get = "BPCElevationPatternDetails_Get";
        public static string BPCAddSpiralDetails_Get = "BPCAddSpiralDetails_Get";
        public static string BPCPopulateCentralizer_Get = "BPCPopulateCentralizer_Get";
        public static string BCDPRCDetails_Get = "BCDPRCDetails_Get";
        public static string CABTransportMode_Get = "CABTransportMode_Get";
        public static string CAB_TransportMaster_Get = "CAB_TransportMaster_Get";
        public static string StructureElementType_Get = "StructureElementType_Get";
        public static string CABACC_ProductMarkingDetails_GET = "CABACC_ProductMarkingDetails_GET";
        public static string ACC_SAPMaterial_GET = "ACC_SAPMaterial_GET";
        public static string BCDConsignment_Get = "BCDConsignment_Get";
        public static string BCDProductAccessorDetails_Update = "BCDProductAccessorDetails_Update";
        public static string CABGroupMarking_Insert = "CABGroupMarking_Insert";
        public static string CABACC_ProductMarkingDetails_Insert = "CABACC_ProductMarkingDetails_Insert";
        public static string BCDProductAccessorDetails_Insert = "BCDProductAccessorDetails_Insert";
        public static string CABCAB_ProductMarkingDetails_GET = "CABCAB_ProductMarkingDetails_GET";
        public static string BPCStiffnerDetails_Get = "BPCStiffnerDetails_Get";
        public static string BPCParameterInfo_Get = "BPCParameterInfo_Get";
        public static string BPCBarMarkPrefix_Get = "BPCBarMarkPrefix_Get";
        public static string BPCSchnellMachineConfig_Get = "BPCSchnellMachineConfig_Get";
        public static string BPCPileDiaDependentValue_Get = "BPCPileDiaDependentValue_Get";
        public static string BPCPopulateTemplateForSchnell_Get = "BPCPopulateTemplateForSchnell_Get";
        public static string BPCPopulateSchnellTemplate_Get = "BPCPopulateSchnellTemplate_Get";
        public static string BPCMEPDetailsForDrawing_Get = "BPCMEPDetailsForDrawing_Get";
        public static string BPCProductMarking_Insert = "BPCProductMarking_Insert";
        public static string AccessoriesCentralizer_Insert = "AccessoriesCentralizer_Insert";
        public static string BPCProjectParameter_Insert = "BPCProjectParameter_Insert";
        public static string BPCMainBarPatternDetails_Update = "BPCMainBarPatternDetails_Update";
        public static string BPCSnlMcnBarPositons_Update = "BPCSnlMcnBarPositons_Update";
        public static string BPCProductMarking_Update = "BPCProductMarking_Update";
        public static string BPCElevationPatternDetails_Update = "BPCElevationPatternDetails_Update";
        public static string CAB_ProductMarkingDetails_GET = "CAB_ProductMarkingDetails_GET";
        public static string BPCSchnellConfiguration_Get = "BPCSchnellConfiguration_Get";
        //CABDAl

        public static string ESM_BBSPostingCABSOR_Get = "ESM_BBSPostingCABSOR_Get";
        public static string usp_ESM_BBSPostingCAB_Get_Range = "usp_ESM_BBSPostingCAB_Get_Range";
        public static string BBSPostingCAB_Get = "BBSPostingCAB_Get";
        public static string usp_CABDetailingDataDescrepancy_Get = "usp_CABDetailingDataDescrepancy_Get";
        public static string BBSPostingCABGM_Get = "BBSPostingCABGM_Get";
        public static string BBSPostingCAB_Insert = "BBSPostingCAB_Insert";
        public static string BBSPostingCAB_Delete = "BBSPostingCAB_Delete";
        public static string ESM_BBSReleaseCAB_Insert = "ESM_BBSReleaseCAB_Insert";
        public static string GroupMarkIdCAB_Get = "GroupMarkIdCAB_Get";


        public static string USP_GET_BBSPOSTING_RANGE_CUBE = "USP_GET_BBSPOSTING_RANGE_CUBE";
        public static string GET_GROUPMARKIDCAB_CUBE = "GET_GROUPMARKIDCAB_CUBE";
        public static string BBSPostingCABSOR_Get = "BBSPostingCABSOR_Get";
        public static string GET_BBS_CUBE = "GET_BBS_CUBE";
        public static string USP_CHECK_BBSSOURCE_CUBE = "USP_CHECK_BBSSOURCE_CUBE";
        public static string GET_COPYBBS_SOURCE_TARGET_UID = "GET_COPYBBS_SOURCE_TARGET_UID";
        public static string COPY_BBS_INSERT_PRODUCTMARK = "COPY_BBS_INSERT_PRODUCTMARK";
        public static string COPY_BBS_INSERT_TRANSDETAILS = "COPY_BBS_INSERT_TRANSDETAILS";
        public static string COPY_BBS_GET_ACCESSORY = "COPY_BBS_GET_ACCESSORY";
        public static string COPY_BBS_INSERT_ACCESSORY = "COPY_BBS_INSERT_ACCESSORY";
        public static string Update_MeshStructureMark = "Update_MeshStructureMark";


        // New ESM Tracking Module
        public static string Get_ColumnNames = "Get_ColumnNames";
        public static string Get_EsmTracking_Details = "Get_EsmTracking_Details";
        public static string Insert_Update_CustomViews_ESM = "Insert_Update_CustomViews_ESM";


        
    }

}




