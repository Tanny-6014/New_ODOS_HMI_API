using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Globalization;

 

namespace UtilityService.Constants
{
    public class SystemConstant
    {
        public static string DefaultDBConnection = "DefaultConnection";
        public static string DSSBOMPDF = "DSSBOMPDF";

        public static string CopyDetailing_Insert = "CopyDetailing_Insert";

        public static string WBSINFOByGroupID_Get = "usp_WBSINFOByGroupID_Get";
        public static string BCDWBSList_Get = "BCDWBSList_Get";
        public static string WBSType_Get = "usp_WBSType_Get";
        public static string USP_WBS1_Get = "USP_WBS1_Get";
        public static string USP_WBS2_Get = "USP_WBS2_Get";
        public static string USP_WBS3_Get = "USP_WBS3_Get";
        public static string CopyWBS1_Get = "usp_CopyWBS1_Get";
        public static string CopyWBS2_Get = "usp_CopyWBS2_Get";
        public static string CopyWBS3_Get = "usp_CopyWBS3_Get";
        public static string CopyWBS_BBSNoAndBBSDesc_Get = "usp_CopyWBS_BBSNoAndBBSDesc_Get";
        public static string GetUserID = "GetUserID";

        public static string CopyWBS_DestWBSDetails_Get = "usp_CopyWBS_DestWBSDetails_Get";
        public static string CopyWBS_BBSNo_Validate = "usp_CopyWBS_BBSNo_Validate";
        public static string CopyWBS_CopyWBSDetailing = "usp_CopyWBS_CopyWBSDetailing";
        public static string CopyWBS_PostedTonnageandQuantityByWBSDetails_Get = "usp_CopyWBS_PostedTonnageandQuantityByWBSDetails_Get";
        public static string CopyProjectParameter_Insert = "CopyProjectParameter_Insert";
        public static string CopyProjectParameterSet_Get = "CopyProjectParameterSet_Get";

        public static string CappingProduct_Get = "usp_CappingProduct_Get";
        public static string ClinkProduct_Get = "usp_ClinkProduct_Get";
        public static string Customer_Get = "usp_Customer_Get";
        public static string Contract_Get = "usp_Contract_Get";
        public static string Project_Get = "usp_Project_Get";
        public static string Transport_Get = "usp_Transport_Get";
        public static string contract_Get_new = "contract_Get_new";
        public static string Project_Get_new = "Project_Get_new";
        public static string SAPMaterialsMaster_Get = "usp_SAPMaterialsMaster_Get";
        public static string SAPMaterialByStructureElementId_Get = "usp_SAPMaterialByStructureElementId_Get";
        public static string SAPMaterialDetailsForValidDatas_Get = "usp_SAPMaterialDetailsForValidDatas_Get";
        public static string SAPMaterialDetailsForInvalidDatas_Get = "usp_SAPMaterialDetailsForInvalidDatas_Get";
        public static string GroupMarking_Insert_PV = "usp_GroupMarking_Insert_PV";
        public static string SeLevelDetail_Insert = "usp_SeLevelDetail_Insert";
        public static string WBSDetails_Insert = "usp_WBSDetails_Insert";
        public static string GroupMarkingParamset_Update = "usp_GroupMarkingParamset_Update";
        public static string FilterGroupMarkName_Get = "usp_FilterGroupMarkName_Get";
        public static string FilterGroupMarkNameForPRC_Get = "usp_FilterGroupMarkNameForPRC_Get";
        public static string FilterGroupMarkNameForSideFor_Get = "usp_FilterGroupMarkNameForSideFor_Get";
        public static string PRCDefaultByStructureElementId_Get = "usp_PRCDefaultByStructureElementId_Get";

        public static string CopyGM_DestParameterSet_Check = "usp_CopyGM_DestParameterSet_Check";
        public static string CopyGM_DestinationGM_Check_PV = "usp_CopyGM_DestinationGM_Check_PV";
        public static string CopyGM_DestinationParameterSet_Create = "usp_CopyGM_DestinationParameterSet_Create";
        public static string CopyGM_DestinationParameterSet_Create_New = "usp_CopyGM_DestinationParameterSet_Create_New";
        public static string CopyGM_CopyWBSElementID_Get = "usp_CopyGM_CopyWBSElementID_Get";
        public static string CopyGM_CopyGroupMarking_Insert_CUBE = "usp_CopyGM_CopyGroupMarking_Insert_CUBE";
        public static string CopyGM_CopyGroupMarking_Insert_New_CUBE = "usp_CopyGM_CopyGroupMarking_Insert_New_CUBE";
        public static string Copy_NDSAndARMA_Qty_Validator = "usp_Copy_NDSAndARMA_Qty_Validator";
        public static string CopyGM_UpdateGM_PostGM = "usp_CopyGM_UpdateGM_PostGM";

        public static string CopyGMGroupmarking_Get = "usp_CopyGMGroupmarking_Get";
        public static string PRCParentHeaderDetailsByGroupMarkID_Get = "usp_PRCParentHeaderDetailsByGroupMarkID_Get";
        public static string COPYGMPARAMVALUESBYGROUPMARKNAME_GET_CUBE = "USP_COPYGMPARAMVALUESBYGROUPMARKNAME_GET_CUBE";
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
     
        public static string MaterialforSAPMaterial_Get = "usp_MaterialforSAPMaterial_Get";
        public static string PRODHforSAPMaterial_Get = "usp_PRODHforSAPMaterial_Get";
        public static string CarpetProductCode_Get = "usp_CarpetProductCode_Get";
        public static string CarpetProductCodeFilter_Get = "usp_CarpetProductCodeFilter_Get";
        public static string CapProductCode_Cache_Get = "usp_CapProductCode_Cache_Get";
        public static string CapProductCodeNoFilter_Cache_Get = "usp_CapProductCodeNoFilter_Cache_Get";
        public static string ClinkProductCode_Get = "usp_ClinkProductCode_Get";
        public static string ClinkProductCodeNoFilter_Get = "usp_ClinkProductCodeNoFilter_Get";
        public static string GroupMarkingPRCDetails_Insert_PV = "usp_GroupMarkingPRCDetails_Insert_PV";
         public static string StructureElementforSAPMaterial_Get = "usp_StructureElementforSAPMaterial_Get";
        public static string SAPMaterialDetails_Update = "usp_SAPMaterialDetails_Update";

        public static string usp_CopyGMGroupmarking_CheckGmName = "usp_CopyGMGroupmarking_CheckGmName";
    }
}
