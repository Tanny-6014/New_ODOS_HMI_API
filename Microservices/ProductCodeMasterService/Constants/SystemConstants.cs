namespace ProductCodeMasterService.Constants
{
    public class SystemConstants
    {
        public static string DefaultDBConnection = "DefaultConnection";

        public static string GetProductType = "ProductType_Get";

        //MESH 
        public static string SapMaterial_Get = "SapMaterial_Get";

        public static string SapMaterial_Get_Raw = "SapMaterial_Get_RawM";

        public static string TwinIndicator_Get = "TwinIndicator_Get";
        public static string BomIndicator_Get = "BomIndicator_Get";
        public static string StructureElement_Get = "StructureElement_Get";
        public static string StructureElement_Get_Mesh = "StructureElement_Get_Mesh";
        public static string GetSAPMaterialList = "GetSAPMaterialList";
        public static string Get_MESH_MasterData = "Get_MESH_MasterData";
        public static string MeshDetails_Get = "MESHMaterialDetails_Get";
        public static string MeshDetails_GetbyId = "MESHMaterialDetails_ByProductID";
        public static string ProductCode_Insert = "ProductCode_Insert";
        public static string MESHProductCodeMasterDetails_Delete = "MESHProductCodeMasterDetails_Delete";
        public static string Get_MainWireData = "Get_MainWireData";
        public static string Get_CrossWireData = "Get_CrossWireData";


        //---------------CAB ------------------
        public static string GetGradeTypes = "GradeTypes_Get";
        public static string GetCabProductCodeFGSAPMaterial = "CabProductCodeSapMaterial_Get";
        public static string GetCabProductCodeRMSAPMaterial = "CabProductCodeRMSapMaterial_Get";
        public static string GetCabProductCodeMasterDetails = "CabProductCodeMasterDetails_Get";
        
        public static string InsertCabProductCodeMaster = "CabProductCodeMaster_Insert";
      //  public static string UpdateCabProductCodeMaster = "CabProductCodeMaster_Update";
        public static string CabProductCodeMasterDetails_Delete = "CabProductCodeMasterDetails_Delete";

        //---------------CORECAGE---------- 
        public static string GetSapMaterial_CoreCage = "CoreCage_SapMaterial_Get";
        public static string GetCARProductCodeMaster = "CARProductCodeMaster_Get";
        public static string CARProductCode_Insert = "CARProductCode_Insert";
        public static string CARProductCodeMasterDetails_Delete = "CARProductCodeMasterDetails_Delete";
        public static string CARProductCodeMaster_Update = "CARProductCodeMaster_Update";

        //RAW Material
        public static string Grade_Get_Raw_Mat = "Grade_Get_Raw_Mat";
        public static string GetMaterial_Raw_Mat = "MaterialType_Get_Raw_Mat";
        public static string RawMaterial_Insert = "RawMaterial_Insert";
        public static string RawMaterialDetails_Get = "RawMaterialDetails_Get";
        public static string RawMaterial_Update = "RawMaterial_Update";
        public static string RawMaterial_Delete = "RawMaterial_Delete";
     


        //ACS
        public static string GetGradeTypes_Acs = "GradeTypes_Get_ACS";
        public static string GetACSProductCodeSAPMaterial = "ACS_SapMaterial_Get";
        public static string GetACSProductCodeMasterDetails = "AccessoriesProductCodeMasterDetails_Get";
        public static string InsertAcsProductCodeMaster = "AccessoriesProductCodeMaster_Insert";
        //public static string UpdateACSProductCodeMaster = "AccessoriesProductCodeMaster_Update";
        public static string ACSProductCodeMasterDetails_Delete = "AccessoriesProductCodeMaster_Delete";



        ///Common Product Code 
        public static string CommonProductcodeList = "CommonProductcodeList";
        public static string CommonProductcode_Delete = "CommonProductcode_Delete";


    }
}
