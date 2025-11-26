namespace ParameterSetService.Constants
{
    public class SystemConstants
    {
        public static string DefaultDBConnection = "DefaultConnection";


        public static string Common_InsertParamterSet = "usp_InsertParamterSet";
        public static string GetParameterList = "GetParameterList";
        public static string Common_DeleteParamterSet = "usp_MeshProjectParam_Delete";






        #region Mesh Parameter Set
        public static string usp_MeshParameterSet_InsUpd = "usp_MeshParameterSet_InsUpd_PV";
      
        public static string MeshProductCode_Get = "usp_MeshProductCode_Get";
        public static string GetMeshList = "usp_MeshCWMWList";
        public static string usp_WallCWMWList = "usp_WallCWMWList";
        public static string MeshProjectParamLap_InsUpd = "usp_MeshProjectParamLap_InsUpd";
        public static string usp_MeshProjectParamLap_Delete = "usp_MeshProjectParamLap_Delete";
        public static string usp_MeshProjectParamLap_Update = "usp_MeshProjectParamLap_Update";

        #endregion

        #region Column Parameter Set
       
        public static string ColumnParameterSet_InsUpd = "usp_ColumnParameterSet_InsUpd_PV";
       // public static string CLinkParameterSet_InsUpd = "usp_CLinkParameterSet_InsUpd_PV";
        public static string ClinkLineItem_InsUpd = "usp_ClinkLineItem_InsUpd";        
        public static string ClinkParamCage_Del = "usp_ClinkParamCage_Del";
        public static string GetColumnParameterList = "GetColumnParameterList";
        public static string Get_ColumnParameterSetTable = "Get_ColumnParameterSetTable";
        public static string GetClinkLineItemList = "GetClinkLineItemList";
        public static string GetColumnParameterSetTable = "Get_ColumnParameterSetTable";
        #endregion

        #region Beam
        public static string CappingProduct_Get = "usp_CappingProduct_Get";
        public static string BeamParameterSet_InsUpd = "usp_BeamParameterSet_InsUpd_PV";
        public static string GetBeamParameterList = "Get_BeamParameterSetList";
        public static string CappingItem_InsUpd = "usp_CappingLineItem_InsUpd";
        public static string GetCappingLineItemList = "GetCappingLineItemList";
        
        public static string Del_usp_CappingParamCage = "usp_CappingParamCage_Del";

        public static string Get_BeamParameterSetTable = "Get_BeamParameterSetTable";

        public static string Get_TRANSPORTMASTER = "Get_TRANSPORTMASTER";


        #endregion
    }
}
