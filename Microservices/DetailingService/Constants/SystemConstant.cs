using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Globalization;

namespace DetailingService.Constants
{
    public class SystemConstant
    {
        public static string DefaultDBConnection = "DefaultConnection";
        public static string DSSBOMPDF = "DSSBOMPDF";

        public static string DetailingForm = "usp_DetailingForm";

        public static string MeshStructureMarkingDetails_Get = "MeshStructureMarkingDetails_Get";

        public static string CarpetStructureMarkingDetails_Get = "CarpetStructureMarkingDetails_Get";
                        
        public static string MeshStructMarkDetailsByStructMarkId_Get = "MeshStructMarkDetailsByStructMarkId_Get";

        public static string BOMDrawingPath = "BOMDrawingPath";

        public static string GroupMarkListing_Get_new = "GroupMarkListing_Get_All";
        
        public static string SlabProductMarkingDetails_InsUpd = "usp_SlabProductMarkingDetails_InsUpd";
        public static string SlabProductMark_Delete = "usp_SlabProductMark_Delete";
        public static string SlabProductMarkByStructureMarkId_Delete = "usp_SlabProductMarkByStructureMarkId_Delete";
        public static string SlabProductByStructureMarkId_Get = "usp_SlabProductByStructureMarkId_Get";
        public static string OverHang_Get = "usp_OverHang_Get";
        public static string Get_bom = "BOM_Get";
        public static string BOM_Insert = "BOM_Insert";
        public static string BOM_Delete = "BOM_Delete";
        public static string BOMHeader_Select = "BOMHeader_Select";
        public static string Shape_ParamDetails = "Shape_ParamDetails";
        public static string Bom_Production_Update = "GeneratePrdBOMFromDetailing_Insert";
        public static string BendingGroup_Get = "BendingGroup_Get";





        public static string SlabStructureMarking_Get = "usp_SlabStructureMarking_Get";
        public static string Accessories_Get = "usp_AccProductMarkDetailsBySEDetailingID_Get";
        public static string Accessories_Insert = "usp_AccProductMarkingDetails_InsUpd";
        public static string Accessories_Delete = "usp_ACCProductMarkDetails_Del";
        

        public static string SlabParameterSetbyProjIdProdType_Get = "usp_SlabParameterSetbyProjIdProdType_Get";
        public static string SlabStructureMarking_InsertsUpdate_PRC = "usp_SlabStructureMarking_InsertsUpdate_PRC";
        public static string SlabStructureMarking_InsertsUpdate = "usp_SlabStructureMarking_InsertsUpdate";
        public static string SlabStructureMark_Delete = "usp_SlabStructureMark_Delete";
        
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
        public static string usp_ACSProduct_Get = "usp_ACSProduct_Get";
        

        public static string CarpetProductCode_Get = "usp_CarpetProductCode_Get";
        public static string CarpetProductCode_Get_New = "usp_CarpetProductCode_Get_New";

        
        public static string CarpetProductCodeFilter_Get = "usp_CarpetProductCodeFilter_Get";
        public static string CapProductCode_Cache_Get = "usp_CapProductCode_Cache_Get";
        public static string CapProductCodeNoFilter_Cache_Get = "usp_CapProductCodeNoFilter_Cache_Get";
        public static string GetColumnLeg = "ColumnLeg_Get";
        public static string ClinkProductCode_Get = "usp_ClinkProductCode_Get";
        public static string ClinkProductCodeNoFilter_Get = "usp_ClinkProductCodeNoFilter_Get";


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
        public static string USP_POPULATESHAPECODEFORCAB_GET = "USP_POPULATESHAPECODEFORCAB_GET";


        //public static string GetColumnLeg = "ColumnLeg_Get";
        /// <summary>
        /// 
        /// </summary>
        public static string allcabshapecodes_get = "allcabshapecodes_get";
        public static string ProductMarkingDetailsInsert = "usp_ProductMarkingDetailsInsert";
        public static string ProductMarkingByGroupMarkId_Get = "usp_ProductMarkingByGroupMarkId_Get";
        public static string ParamValuesByProductMarkId_Get = "usp_ParamValuesByProductMarkId_Get";
        public static string ProductmarkByStructId_Delete = "usp_ProductmarkByStructId_Delete";
        public static string ParameterSetByGroupMarkID_Get = "usp_ParameterSetByGroupMarkID_Get";
        public static string ParameterSetByProjectProductTypeId_Get = "usp_ParameterSetByProjectProductTypeId_Get";
        public static string ParameterSetbyProjIdProdType_Get = "usp_ParameterSetbyProjIdProdType_Get";
        public static string SlabParameterSetbyProjIdProdType_Get_CAR = "usp_SlabParameterSetbyProjIdProdType_Get_CAR";
        public static string ProjectParamLap_Get = "usp_ProjectParamLap_Get";
        public static string CarpetProductByStructureMarkId_Get = "usp_CarpetProductByStructureMarkId_Get";
        public static string CarpetProductMark_Delete = "usp_CarpetProductMark_Delete";
        public static string CarpetProductMarkByStructureMarkId_Delete = "usp_CarpetProductMarkByStructureMarkId_Delete";
        public static string CarpetProductMarkingDetails_InsUpd = "usp_CarpetProductMarkingDetails_InsUpd";
        public static string Carpet_GetBOMType = "usp_Carpet_GetBOMType";
        public static string ShapeCode_Get_MWRunPerMetValue_ByProdCode_Carpet = "usp_ShapeCode_Get_MWRunPerMetValue_ByProdCode_Carpet";
        public static string ShapeCode_Get_CWRunPerMetValue_ByProdCode_Carpet = "usp_ShapeCode_Get_CWRunPerMetValue_ByProdCode_Carpet";
        public static string CarpetStructureMarking_Get = "usp_CarpetStructureMarking_Get";
        public static string CarpetStructureMarking_InsertsUpdate = "usp_CarpetStructureMarking_InsertsUpdate";
        public static string CarpetStructureMark_Delete = "usp_CarpetStructureMark_Delete";



        public static string ColumnProductByStructureMarkId_Get = "usp_ColumnProductByStructureMarkId_Get";
        public static string ColumnProductmarkByStructId_Delete = "usp_ColumnProductmarkByStructId_Delete_Out";
        public static string ColumnProductMarkingDetails_Insert = "usp_ColumnProductMarkingDetails_Insert";
        public static string ColumnStructureMark_Get = "usp_ColumnStructureMark_Get";
        public static string ColumnStructureMarkByStructId_Delete = "usp_ColumnStructureMarkByStructId_Delete";
        public static string ColumnStructureMarking_InsUpd_PRC = "Usp_ColumnStructureMarking_InsUpd_PRC_OUT";
        public static string columnstructuremarking_insert = "Usp_columnstructuremarking_insert";
        public static string ColumnStructureMarking_Update = "usp_ColumnStructureMarking_Update";

        public static string PreferredMeshData_Get = "usp_PreferredMeshData_Get";
        public static string PreferredMesh_Get = "usp_PreferredMesh_Get";
        public static string CubeShapeParameter_Insert_forGm = "usp_CubeShapeParameter_Insert_forGm";
        public static string CubeShapeParameter_GET_forgm = "usp_CubeShapeParameter_GET_forgm";
        public static string CABSHAPEPARAMETER_PARALLEL_INSERT = "USP_CABSHAPEPARAMETER_PARALLEL_INSERT";
        public static string ShapeParameterForBeam_Get = "usp_ShapeParameterForBeam_Get";
        public static string ColumnShapeParameter_Get = "usp_ColumnShapeParameter_Get";
        public static string SlabShapeParameter_Get = "usp_SlabShapeParameter_Get";
        public static string GET_SHAPEPARAMETER_CUBE = "USP_GET_SHAPEPARAMETER_CUBE";
        public static string SHAPEPARAMETERFORCAB_GET_CUBE = "USP_SHAPEPARAMETERFORCAB_GET_CUBE";
        public static string SHAPEPARAMETERFORCAB_EDIT_CUBE = "USP_SHAPEPARAMETERFORCAB_EDIT_CUBE";
        public static string GET_SHAPEPARAMETER_VARLENGTH_CUBE = "USP_GET_SHAPEPARAMETER_VARLENGTH_CUBE";
        public static string SHAPEPARAMFORCAB_GET_CUBE = "USP_SHAPEPARAMFORCAB_GET_CUBE";
        public static string CustContProjByGroupID_Get = "usp_CustContProjByGroupID_Get";
        public static string CustContProjID_Get = "usp_CustContProjID_Get";
        public static string GroupmarkingDetailsByGroupID_Get = "usp_GroupmarkingDetailsByGroupID_Get";
        public static string GroupmarkingDetailsByGroupIDCARPET_Get = "usp_GroupmarkingDetailsByGroupIDCARPET_Get";
        public static string PostedBeamByGroupmark_Get = "usp_PostedBeamByGroupmark_Get";
        public static string GroupMarkPostedValidate_Get = "usp_GroupMarkPostedValidate_Get";
        public static string GroupMarkPPosting_Regenerate_CARPET = "usp_GroupMarkPPosting_Regenerate_CARPET";
        public static string GroupMarkPPosting_Regenerate = "usp_GroupMarkPPosting_Regenerate";
        public static string WBSAttachedWithGroupMarking_Get="usp_WBSAttachedWithGroupMarking_Get";
        public static string unpostedwbsattachedwithgroupmarking_get = "Usp_unpostedwbsattachedwithgroupmarking_get";
        public static string CABCustContProjByGroupID_Get = "usp_CABCustContProjByGroupID_Get";
        public static string CABGroupmarkingDetailsByGroupID_Get = "usp_CABGroupmarkingDetailsByGroupID_Get";
        public static string GroupMarkingHeaderDetailsByGroupMarkIdForPRC_Get = "usp_GroupMarkingHeaderDetailsByGroupMarkIdForPRC_Get";
        public static string StructureElement_Get = "usp_StructureElement_Get";
        public static string GetProductMarkingFormulaForShape = "GetProductMarkingFormulaForShape";
        public static string GetMWLengthFormula = "GetMWLengthFormula";
        public static string GetInputOutputValidationForShape="GetInputOutputValidationForShape";
        public static string GetShapeParametersForShapeId = "GetShapeParametersForShapeId";
        public static string GetStructureMarkingFormulaeElement = "GetStructureMarkingFormulaeElement";
        public static string GetDrainProductMarkingFormuale = "GetDrainProductMarkingFormuale";
        public static string SAPMaterialsMaster_Get = "usp_SAPMaterialsMaster_Get";
        public static string SAPMaterialByStructureElementId_Get = "usp_SAPMaterialByStructureElementId_Get";
        public static string SAPMaterialDetailsForValidDatas_Get = "usp_SAPMaterialDetailsForValidDatas_Get";
        public static string SAPMaterialDetailsForInvalidDatas_Get = "usp_SAPMaterialDetailsForInvalidDatas_Get";
        public static string StructureElementforSAPMaterial_Get = "usp_StructureElementforSAPMaterial_Get";
        public static string MaterialforSAPMaterial_Get = "usp_MaterialforSAPMaterial_Get";
        public static string PRODHforSAPMaterial_Get = "usp_PRODHforSAPMaterial_Get";
        public static string SAPMaterialDetails_Update = "usp_SAPMaterialDetails_Update";
        public static string GroupMarking_Insert_PV = "usp_GroupMarking_Insert_PV";
        public static string GroupMarkingPRCDetails_Insert_PV = "usp_GroupMarkingPRCDetails_Insert_PV";
        public static string SeLevelDetail_Insert = "usp_SeLevelDetail_Insert";
        public static string WBSDetails_Insert = "usp_WBSDetails_Insert"; 
        public static string GroupMarkingParamset_Update = "usp_GroupMarkingParamset_Update";
        public static string FilterGroupMarkName_Get = "usp_FilterGroupMarkName_Get";
        public static string FilterGroupMarkNameForPRC_Get = "usp_FilterGroupMarkNameForPRC_Get";
        public static string FilterGroupMarkNameForSideFor_Get = "usp_FilterGroupMarkNameForSideFor_Get";
        public static string PRCDefaultByStructureElementId_Get = "usp_PRCDefaultByStructureElementId_Get";
        public static string PRCParentHeaderDetailsByGroupMarkID_Get = "usp_PRCParentHeaderDetailsByGroupMarkID_Get";
        public static string COPYGMPARAMVALUESBYGROUPMARKNAME_GET_CUBE = "USP_COPYGMPARAMVALUESBYGROUPMARKNAME_GET_CUBE";
        public static string CopyGMGroupmarking_Get = "usp_CopyGMGroupmarking_Get";
        public static string CopyGM_DestParameterSet_Check = "usp_CopyGM_DestParameterSet_Check";
        public static string CopyGM_DestinationGM_Check_PV = "usp_CopyGM_DestinationGM_Check_PV";
        public static string CopyGM_DestinationParameterSet_Create = "usp_CopyGM_DestinationParameterSet_Create";
        public static string CopyGM_DestinationParameterSet_Create_New = "usp_CopyGM_DestinationParameterSet_Create_New";
        public static string CopyGM_CopyWBSElementID_Get = "usp_CopyGM_CopyWBSElementID_Get";
        public static string CopyGM_CopyGroupMarking_Insert_CUBE = "usp_CopyGM_CopyGroupMarking_Insert_CUBE";
        public static string CopyGM_CopyGroupMarking_Insert_New_CUBE = "usp_CopyGM_CopyGroupMarking_Insert_New_CUBE";
        public static string Copy_NDSAndARMA_Qty_Validator = "usp_Copy_NDSAndARMA_Qty_Validator";
        public static string CopyGM_UpdateGM_PostGM = "usp_CopyGM_UpdateGM_PostGM";
        public static string ProductDetailsForIrregularBOMByProductMarkId_Get = "usp_ProductDetailsForIrregularBOMByProductMarkId_Get";
        public static string WireTypeAndSpacingByProductMarkId_Get = "usp_WireTypeAndSpacingByProductMarkId_Get";
        public static string StructureElementTypeIdByStructureElementType_Get = "usp_StructureElementTypeIdByStructureElementType_Get";
        public static string GenerateBOMDetailsBendCheck_Insert = "usp_GenerateBOMDetailsBendCheck_Insert";
        public static string BendingCheckFail_Insert = "usp_BendingCheckFail_Insert";
        public static string CappingProduct_Get = "usp_CappingProduct_Get";
        public static string ClinkProduct_Get = "usp_ClinkProduct_Get";
        public static string Customer_Get = "usp_Customer_Get";
        public static string Contract_Get = "usp_Contract_Get";
        public static string Project_Get = "usp_Project_Get";
        public static string Transport_Get = "usp_Transport_Get";
        public static string contract_Get_new = "contract_Get_new";
        public static string Project_Get_new = "Project_Get_new";
        public static string Bend_BendConstraint_get = "usp_Bend_BendConstraint_get";
        public static string bend_bendspacingconstraint_get = "Usp_bend_bendspacingconstraint_get";
        public static string bend_machineconstraint_get = "Usp_bend_machineconstraint_get";
        public static string DeleteGroupmark_GroupMarId = "DeleteGroupmark_GroupMarId";
        public static string StructureMarking_Get = "usp_StructureMarking_Get";
        public static string StructureMarkingDetailsInsUpd_PRC = "usp_StructureMarkingDetailsInsUpd_PRC";
        public static string StructureMarkingDetailsInsert = "usp_StructureMarkingDetailsInsert";
        public static string StrucutreMarkByStructId_Delete = "usp_StrucutreMarkByStructId_Delete";

        


        public static string UpdateGM_PostGM = "UpdateGM_PostGM";
        public static string GroupMarkingCheck_GET = "GroupMarkingCheck_GET";
        public static string GroupMarkingPostedCheck_Get = "GroupMarkingPostedCheck_Get";
        public static string CABProductType_Get = "CABProductType_Get";
        public static string GROUPMARKLISTING_GET_CUBE = "GROUPMARKLISTING_GET_CUBE";
        public static string CoreCageProductCodeID_Get = "CoreCageProductCodeID_Get";

        //cabitem
        public static string USP_GET_SEDETAILING_CUBE = "USP_GET_SEDETAILING_CUBE";
        public static string USP_GET_SHAPE_CORD_CUBE = "USP_GET_SHAPE_CORD_CUBE";
        public static string USP_CABPRODUCTMARKINGDETAILS_COPY_CUBE = "USP_CABPRODUCTMARKINGDETAILS_COPY_CUBE";
        public static string USP_GET_CABPINBYFORMER_CUBE = "USP_GET_CABPINBYFORMER_CUBE";
        public static string USP_CABPRODUCTMARKINGDETAILS_DELETE_CUBE = "USP_CABPRODUCTMARKINGDETAILS_DELETE_CUBE";
        public static string USP_CABDETAILS_DELETE_CUBE = "USP_CABDETAILS_DELETE_CUBE";
        public static string USP_VALIDATE_CABPIN_CUBE = "USP_VALIDATE_CABPIN_CUBE";
        public static string USP_CABPIN_GET_CUBE = "USP_CABPIN_GET_CUBE";
        public static string USP_GET_PIN_BYSHAPE_CUBE = "USP_GET_PIN_BYSHAPE_CUBE";
        public static string GET_BBS_CUBE = "GET_BBS_CUBE";
        public static string USP_CABPRODUCTMARKING_UPDATE = "USP_CABPRODUCTMARKING_UPDATE";
        public static string USP_PRODUCTMARK_VAR_UPDATE = "USP_PRODUCTMARK_VAR_UPDATE";
        public static string USP_CABSHAPEPARAMETER_DELETE = "USP_CABSHAPEPARAMETER_DELETE";
        public static string USP_SHAPEPARAMETERFORCAB_EDIT_CUBE = "USP_SHAPEPARAMETERFORCAB_EDIT_CUBE";
        public static string USP_CAB_PRODUCTMARKINGDETAILS_INSUPD_CUBE = "USP_CAB_PRODUCTMARKINGDETAILS_INSUPD_CUBE";
        public static string SP_CABPRODUCTMARKINGDETAILS_INSERT_INLINE = "SP_CABPRODUCTMARKINGDETAILS_INSERT_INLINE";
        public static string USP_CABPRODUCTMARKINGDETAILS_GET_CUBE = "USP_CABPRODUCTMARKINGDETAILS_GET_CUBE";
        public static string USP_CABACC_PRODUCTMARKINGDETAILS_INSUPD_CUBE = "USP_CABACC_PRODUCTMARKINGDETAILS_INSUPD_CUBE";
        public static string COPY_BBS_INSERT_CUBE = "COPY_BBS_INSERT_CUBE";
        public static string USP_CHECK_BBSSOURCE_CUBE = "USP_CHECK_BBSSOURCE_CUBE";
        public static string USP_SAVE_SHAPETRANSDETAILS_INSERT = "USP_SAVE_SHAPETRANSDETAILS_INSERT";
        //cabcalculation 
        public static string USP_GET_PRODUCTIONLENGTH_FORMULA_CUBE = "USP_GET_PRODUCTIONLENGTH_FORMULA_CUBE";
        public static string USP_GET_COUPLER_DATA_CUBE = "USP_GET_COUPLER_DATA_CUBE";
        public static string sp_Check_IsCustomInvCutLength = "sp_Check_IsCustomInvCutLength";


        //Bpc
        public static string usp_BorePileParameterSet_Get = "usp_BorePileParameterSet_Get";
        public static string usp_BorePileParameterSet_InsUpd_PV = "usp_BorePileParameterSet_InsUpd_PV";
        public static string usp_BorePileProductCode_Get = "usp_BorePileProductCode_Get";

        //DWall report
        public static string Usp_cubeshapeparameter_insert_forgm = "Usp_cubeshapeparameter_insert_forgm ";
        public static string USP_UPDATE_SHAPE_IMAGE = "USP_UPDATE_SHAPE_IMAGE";
        public static string CABShapeCoordsDWALL_get_cube = "CABShapeCoordsDWALL_get_cube";
        public static string USP_GET_SHAPE_IMAGE = "USP_GET_SHAPE_IMAGE";


        //Column
        public static string Update_ColumnStructureMark = "Update_ColumnStructureMark";

        public static string Update_BeamStructureMark = "Update_BeamStructureMark";

        //call for add gm in WBS Posting 
        public static string get_Groupmarking_Data = "get_Groupmarking_Data";







    }
}
