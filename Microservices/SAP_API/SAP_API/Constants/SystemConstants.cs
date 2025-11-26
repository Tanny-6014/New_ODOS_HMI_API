namespace SAP_API.Constants
{
    public static class SystemConstant
    {
       
        public static string DefaultDBConnection = "DefaultConnection"; 

        public static string sp_Insert_WBSMaintenance = "sp_Insert_WBSMaintenance";

        public static string sp_Update_WBSMaintenance = "sp_Update_WBSMaintenance";

        public static string Storey_Get = "Storey_Get";
        public static string sp_Delete_WBSALL = "sp_Delete_WBSALL";
        public static string DeleteWBSCollapseLevel = "sp_DeleteWBSCollapseLevel";

        public static string sp_Delete_WBSElement = "sp_Delete_WBSElement_Vanita";
        public static string usp_PostingCappingInfo_Get = "usp_PostingCappingInfo_Get";
        public static string usp_CapProductCodeNoFilter_Cache_Get = "usp_CapProductCodeNoFilter_Cache_Get";
        public static string usp_ShapeCodeNoFilter_Get = "usp_ShapeCodeNoFilter_Get";
        public static string usp_StructureElement_Get = "usp_StructureElement_Get";
        public static string usp_Customer_Get = "usp_Customer_Get";
        public static string usp_Contract_Get = "usp_Contract_Get";
        public static string Contract_Get_new = "Contract_Get_new";
        public static string usp_Project_Get = "usp_Project_Get";
        public static string Project_Get_new = "Project_Get_new";
        public static string DestProject_Get_new = "DestProject_Get_new";
        public static string PhysicalProject_get = "PhysicalProject_get";
        public static string usp_ProductType_Get = "usp_ProductType_Get";
        public static string usp_ParameterSetProductType_Get = "usp_ParameterSetProductType_Get";
        public static string WBSProductType_Get_new = "WBSProductType_Get_new";
        public static string usp_ParameterSetProductType_Get_BorePile = "usp_ParameterSetProductType_Get_BorePile";

        #region WEB Posting
        public static string sp_PostingWbs_Get_PV = "usp_PostingWbs_Get_PV";
        public static string sp_GroupMarking_ddl = "usp_PostingGroupMarkDetails_Get";
        public static string posting_unpost_update = "usp_posting_unpost_update";
        public static string posting_post_update = "Usp_posting_post_update";
        public static string sp_GroupMarkingDetails = "WBSGroupMarking_Get";
        public static string BBSRelease_Insert = "usp_BBSRelease_Insert";

        public static string BBSGroupMarking_Insert = "USP_POSTINGPOSTGM_INSERT_CUBE";

        public static string BBSCappingPost_Info = "usp_PostingCappingInfo_Get";
        public static string BBSClinkPost_Info = "usp_PostingClinkInfo_Get";

        public static string PostingInsertCCLMarkDetails_Insert = "usp_PostingInsertCCLMarkDetails_Insert";
        public static string PostingInsertClinkCCLMarkDetails_Insert = "usp_PostingInsertClinkCCLMarkDetails_Insert";
        public static string PostingCappingHeaderInfo_Get = "usp_PostingCappingHeaderInfo_Get";
        public static string PostingCLinkHeaderInfo_Get = "usp_PostingCLinkHeaderInfo_Get";
        public static string PostingCapStructure_Delete = "usp_PostingCapStructure_Delete";
        public static string PostingCLinkStructure_Delete = "usp_PostingCLinkStructure_Delete";
        public static string PostingGroupMarking_Delete = "usp_PostingWbsAttachedGM_Delete";

        public static string CapProductCode_Get = "usp_CapProductCode_Cache_Get";

        public static string ClinkProductCode_Get = "usp_ClinkProductCode_Get";

        public static string Exists_Check_PostingCapClinkExists = "usp_PostingCapClinkExists_Get";

        public static string CapShapeCode_Get = "usp_ShapeCode_Get";

        public static string ClinkShapeCode_Get = "usp_ShapeCode_Get_Clink";

        public static string CappingMO1MO2CO1Co2_Get = "usp_PostingCappingMO1MO2CO1Co2_Get";

        public static string ClinkMO1MO2CO1Co2_Get = "usp_PostingClinkMO1MO2CO1Co2_Get";

        public static string UpdateGroupMarking_Delete = "usp_PostingWbsAttachedGM_Update";
        #endregion



    }
}
