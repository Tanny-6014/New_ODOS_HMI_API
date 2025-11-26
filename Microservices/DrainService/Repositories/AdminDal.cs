using DrainService.Context;
using DrainService.Constants;
using System.Data;
using Dapper;
using DrainService.Interfaces;
using Microsoft.EntityFrameworkCore;
using DrainService.Repositories;
using Microsoft.Data.SqlClient;
using DrainService.Dtos;


public class AdminDal : IAdminDal
{
   // private DrainServiceContext _dbContext;
    private readonly IConfiguration _configuration;
    private string connectionString;


    public AdminDal( IConfiguration configuration)
    {
        //_dbContext = dbContext;
        _configuration = configuration;

        connectionString = _configuration.GetConnectionString(SystemConstants.DefaultDBConnection);
    }

    public AdminDal()
    {
    }


    //// added for ship to party
    //public  DataSet GetProject_new(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("Project_Get_new");
    //        dynamicParameters.Add("@intCustomerId", obj.CustomerId);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}
    //public  DataSet GetProductType_new(AdminInfo obj)
    //{
    //    try
    //    {
    //        SqlCacheDependency SqlDep;
    //        DataSet dsCache = (DataSet)HttpRuntime.Cache["CacheWBSProduct"];
    //        SqlDep = new SqlCacheDependency("NDSCaching", "producttypemaster");
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("WBSProductType_Get_new");
    //        dynamicParameters.Add("@intprojectid", obj.ProjectId);
    //        dsCache = DataAccess.DataAccess.GetDataSet(dbcom);
    //        HttpRuntime.Cache.Insert("CacheWBSProduct", dsCache, SqlDep);
    //        // End If
    //        return dsCache;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}
    //public  DataSet GetContract_new(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("contract_Get_new");
    //        dynamicParameters.Add("@intProjectId", obj.ProjectId);
    //        dynamicParameters.Add("@intProductTypeId", obj.ProductCodeId);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}
    //// end added for ship to party
    //public  DataSet GetMasterMenu(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("MastersMenu_Get");
    //        dynamicParameters.Add("@tntRoleId", 10); // obj.RoleId)
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        // Throw ex
    //        throw ex;
    //    }
    //}
    //public  System.Data.DataSet GetStatusDetails()
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("WBSStatus_Get");
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  System.Data.DataSet GetProductCode()
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ProductCodeMaster_Get");
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  System.Data.DataSet GetProductCodeWithoutRawMaterial(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ProductCodeMasterWithoutRawMaterial_Get");
    //        dynamicParameters.Add("@StatusId", obj.StatusId);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  System.Data.DataSet GetProductApprovalRights()
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ProductApprovalRights_Get");
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  System.Data.DataSet GetProductType()
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ProductType_Get");
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetGradeTypes()
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("GradeTypes_Get");
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  System.Data.DataSet GetStandardCode()
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("StandardCode_Get");
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  System.Data.DataSet GetGrade()
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("Grade_Get");
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  System.Data.DataSet GetSapMaterial()
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("SapMaterial_Get");
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //// --> Added For Core Cage Development

    //public  System.Data.DataSet GetSapMaterial_CoreCage()
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("CoreCage_SapMaterial_Get");
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  System.Data.DataSet GetSAPMaterialStructureElement()
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("SapMaterial_StructureDetails_Get");
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  System.Data.DataSet GetCabProductCodeSAPMaterial()
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("CabProductCodeSapMaterial_Get");
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetMaterialType()
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("MaterialType_Get");
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetSapContractNo(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("SapContractNo_Get");
    //        dynamicParameters.Add("@intContractId", obj.ContractId);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetValidUsers(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("UserAccessToForms_Validate");
    //        dynamicParameters.Add("@vchLoginId", obj.LoginId);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet CopySAPContractNo(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("CopySAPContract_Get");
    //        dynamicParameters.Add("@intContractId", obj.ContractId);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetSAPContractDetails(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("SAPContractDetails_Get");
    //        dynamicParameters.Add("@intContractId", obj.ContractId);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetSAPProjectDetails(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("SAPProjectDetails_Get");
    //        dynamicParameters.Add("@intProjectId", obj.ProjectId);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  string[] GetSapContractDesc(AdminInfo obj)
    //{
    //    try
    //    {
    //        string[] strResult = new string[2];
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("SapContractDesc_Get");
    //        dynamicParameters.Add("@intContractId", obj.ContractId);
    //        DataAccess.DataAccess.db.AddOutParameter("@vchContractCode", 100);
    //        DataAccess.DataAccess.db.AddOutParameter("@vchDescription", 100);
    //        DataAccess.DataAccess.GetScalar(dbcom);

    //        if ((Information.IsDBNull(dbcom.Parameters["@vchContractCode"].Value)))
    //            strResult[0] = "";
    //        else
    //            strResult[0] = (dbcom.Parameters["@vchContractCode"].Value);
    //        if ((Information.IsDBNull(dbcom.Parameters["@vchDescription"].Value)))
    //            strResult[1] = "";
    //        else
    //            strResult[1] = (dbcom.Parameters["@vchDescription"].Value);
    //        return strResult;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetProductTypeL2()
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ProductTypeL2_Get");
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetBomIndicator()
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BomIndicator_Get");
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetTwinIndicator()
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("TwinIndicator_Get");
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  SqlDataReader InsertProductCode(AdminInfo obj)
    //{
    //    try
    //    {
    //        // '// IF Condition = Added for Core Cage Development

    //        if (obj.CoreCage)
    //        {
    //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("CARProductCode_Insert");

    //            dynamicParameters.Add("@intProductCodeId", obj.ProductCodeId);
    //            dynamicParameters.Add("@vchProductCode", obj.ProductCode);
    //            dynamicParameters.Add("@vchProductDescription", obj.ProductDesc);

    //            dynamicParameters.Add("@vchWeightSqm", obj.WeightSqm);
    //            dynamicParameters.Add("@vchConversionFactor", obj.ConversionFactor);

    //            dynamicParameters.Add("@intSAPMaterial", obj.SapMaterial);
    //            dynamicParameters.Add("@vchProductTypeL2", obj.ProductTypeL2);
    //            dynamicParameters.Add("@vchStructureElement", obj.StructureElement);
    //            dynamicParameters.Add("@intStatusId", obj.StatusId);

    //            dynamicParameters.Add("@intUserId", obj.UserId);
    //            dynamicParameters.Add("@Output", 0);
    //        }
    //        else
    //        {
    //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ProductCode_Insert");
    //            dynamicParameters.Add("@strGenerateBOMStatus", obj.GenerateBOMStatus);
    //            dynamicParameters.Add("@intProductCodeId", obj.ProductCodeId);
    //            dynamicParameters.Add("@vchProductCode", obj.ProductCode);
    //            dynamicParameters.Add("@vchProductDescription", obj.ProductDesc);
    //            dynamicParameters.Add("@bitRawMaterial", DbType.Boolean, obj.boolRawMaterial);
    //            dynamicParameters.Add("@intMWProductCodeId", obj.MWProduct);
    //            dynamicParameters.Add("@vchMWMaterialType", obj.MWMaterialType);
    //            dynamicParameters.Add("@vchMWDiameter", obj.MWDiameter);
    //            dynamicParameters.Add("@vchMWMaterialAbbr", obj.MWMaterialAbbr);
    //            dynamicParameters.Add("@intMWSpace", obj.MWSpace);
    //            dynamicParameters.Add("@vchMWGrade", obj.MWGrade);
    //            dynamicParameters.Add("@vchMWLength", obj.MWLength);
    //            dynamicParameters.Add("@vchMWMaxBendLen", obj.MWMaxBendLen);
    //            dynamicParameters.Add("@vchMWWeightRun", obj.MWWeightRun);
    //            dynamicParameters.Add("@intCWProductcodeId", obj.CWProduct);
    //            dynamicParameters.Add("@vchCWMaterialType", obj.CWMaterialType);
    //            dynamicParameters.Add("@vchCWDiameter", obj.CWDiameter);
    //            dynamicParameters.Add("@vchCWMaterialAbbr", obj.CWMaterialAbbr);
    //            dynamicParameters.Add("@intCWSpace", obj.CWSpace);
    //            dynamicParameters.Add("@vchCWGrade", obj.CWGrade);
    //            dynamicParameters.Add("@vchCWLength", obj.CWLength);
    //            dynamicParameters.Add("@vchCWMaxBendLen", obj.CWMaxBendLen);
    //            dynamicParameters.Add("@vchCWWeightRun", obj.CWWeightRun);
    //            dynamicParameters.Add("@vchWeightSqm", obj.WeightSqm);
    //            dynamicParameters.Add("@vchConversionFactor", obj.ConversionFactor);
    //            dynamicParameters.Add("@chrTwinInd", obj.TwinInd);
    //            dynamicParameters.Add("@bitStaggeredInd", DbType.Boolean, obj.StaggeredInd);
    //            dynamicParameters.Add("@bitBendInd", DbType.Boolean, obj.BendInd);
    //            dynamicParameters.Add("@charBomInd", obj.BomIndicator);
    //            dynamicParameters.Add("@intMinLink", obj.MinLink);
    //            dynamicParameters.Add("@intMaxLink", obj.MaxLink);
    //            dynamicParameters.Add("@intSAPMaterial", obj.SapMaterial);
    //            dynamicParameters.Add("@vchProductTypeL2", obj.ProductTypeL2);
    //            dynamicParameters.Add("@vchStructureElement", obj.StructureElement);
    //            dynamicParameters.Add("@intStatusId", obj.StatusId);
    //            dynamicParameters.Add("@chrWireType", obj.WireType);
    //            dynamicParameters.Add("@intShapeCodeId", obj.ShapeId);
    //            dynamicParameters.Add("@intUserId", obj.UserId);
    //            dynamicParameters.Add("@Output", 0);
    //        }
    //        return DataAccess.DataAccess.ExecuteReader(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //// 'IF Condition Added:- Modified for Core Cage Development
    //public  DataSet GetProductDetails(AdminInfo obj)
    //{
    //    try
    //    {
    //        if (obj.CoreCage)
    //        {
    //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("CARProductCode_Select");
    //            dynamicParameters.Add("@intProductCodeId", obj.ProductCodeId);
    //        }
    //        else
    //        {
    //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ProductCode_Select");
    //            dynamicParameters.Add("@intProductCodeId", obj.ProductCodeId);
    //        }

    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  System.Data.DataSet GetRole()
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("Role_Get");
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  System.Data.DataSet GetForm()
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("Form_Get");
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  System.Data.DataSet GetRoleAccessDetails(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("RoleAccess_Get");
    //        dynamicParameters.Add("@tntRoleId", obj.RoleId);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  System.Data.DataSet GetRoleUser(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("RoleUser_Get");
    //        dynamicParameters.Add("@tntRoleId", obj.RoleId);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  int InsertRole(AdminInfo obj)
    //{
    //    try
    //    {
    //        Int32 intReturnValue;

    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("RoleAccess_Insert");
    //        dynamicParameters.Add("@vchRoleName", obj.RoleName);
    //        dynamicParameters.Add("@tntRoleId", obj.RoleId);
    //        dynamicParameters.Add("@tntStatusId", obj.StatusId);
    //        dynamicParameters.Add("@bitApproveAccess", DbType.Boolean, obj.bitApproveAccess);
    //        dynamicParameters.Add("@vchRemarks", obj.Remarks);
    //        dynamicParameters.Add("@intUserId", obj.UserId);
    //        dynamicParameters.Add("@vchFormID", obj.FormId);
    //        dynamicParameters.Add("@vchReadAccess", obj.ReadAccess);
    //        dynamicParameters.Add("@vchWriteAccess", obj.WriteAccess);
    //        dynamicParameters.Add("@vchUserName", obj.UserName);
    //        dynamicParameters.Add("@vchEmailId", obj.EmailId);
    //        dynamicParameters.Add("@vchLoginId", obj.LoginId);
    //        DataAccess.DataAccess.db.AddOutParameter("@intOutput", 0);
    //        DataAccess.DataAccess.GetScalar(dbcom);
    //        intReturnValue = (dbcom.Parameters["@intOutput"].Value);
    //        return intReturnValue;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  int InsertStructureElement(AdminInfo obj)
    //{
    //    try
    //    {
    //        Int32 intReturnValue;
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("StructureElementMaster_Insert");
    //        dynamicParameters.Add("@intElementId", obj.ElementId);
    //        dynamicParameters.Add("@vchElementType", obj.ElementType);
    //        dynamicParameters.Add("@vchElementDesc", obj.ElementDesc);
    //        dynamicParameters.Add("@intStatusId", obj.StatusId);
    //        dynamicParameters.Add("@intUserId", obj.UserId);
    //        DataAccess.DataAccess.db.AddOutParameter("@Output", 0);
    //        DataAccess.DataAccess.GetScalar(dbcom);
    //        intReturnValue = (dbcom.Parameters["@Output"].Value);
    //        return intReturnValue;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  SqlDataReader GetProductMainSelect(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ProductCodeSelect_Select");
    //        dynamicParameters.Add("@intProductCodeId", obj.MWProduct);
    //        return DataAccess.DataAccess.ExecuteReader(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  SqlDataReader GetProductCrossSelect(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ProductCodeSelect_Select");
    //        dynamicParameters.Add("@intProductCodeId", obj.CWProduct);
    //        return DataAccess.DataAccess.ExecuteReader(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetStructureElement()
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("StructureElementMaster_Get");
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetStrutureElementType()
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("StructureElement_Get");
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetStrutureElementTypeMesh()
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("[StructureElementMesh_Get]");
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetBomMasterHeader()
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BomMasterHeader_Get");
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  int InsertBomMasterHeader(AdminInfo obj)
    //{
    //    try
    //    {
    //        Int32 intReturnValue;

    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BomMasterHeader_Insert");
    //        dynamicParameters.Add("@intBomMasterHeaderId", obj.BomMasterHeaderId);
    //        dynamicParameters.Add("@intProductCodeId", obj.intProductCodeId);
    //        dynamicParameters.Add("@nvchBomType", obj.BomType);
    //        dynamicParameters.Add("@nvchMeshType", obj.MeshType);
    //        DataAccess.DataAccess.db.AddOutParameter("@intOutput", 0);
    //        DataAccess.DataAccess.GetScalar(dbcom);
    //        intReturnValue = (dbcom.Parameters["@intOutput"].Value);
    //        return intReturnValue;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetBOMHeader()
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BomMasterHeader_Get");
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetBOM(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ProductBomMaster_Get");
    //        dynamicParameters.Add("@intProductCodeId", obj.intProductCodeId);
    //        dynamicParameters.Add("@nvchBomType", obj.BomType);
    //        dynamicParameters.Add("@nvchMeshType", obj.MeshType);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  int InsertProductBOMDetails(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ProductBOMMaster_Insert");
    //        dynamicParameters.Add("@intProductCodeId", obj.intProductCodeId);
    //        dynamicParameters.Add("@strBomType", obj.BomType);
    //        dynamicParameters.Add("@strMeshType", obj.MeshType);
    //        dynamicParameters.Add("@intProductBOMId", obj.ProductBOMId);
    //        dynamicParameters.Add("@vchWireType", obj.WireType);
    //        dynamicParameters.Add("@strLineNo", obj.LineNo);
    //        dynamicParameters.Add("@strStartPos", obj.StartPosition);
    //        dynamicParameters.Add("@strNoPths", obj.NoOfPitch);
    //        dynamicParameters.Add("@strWireSpace", obj.WireSpace);
    //        dynamicParameters.Add("@strWireLen", obj.WireLength);
    //        dynamicParameters.Add("@strWireDia", obj.WireDiameter);
    //        dynamicParameters.Add("@strRawMaterial", obj.RawMaterial);
    //        // dynamicParameters.Add("@strRawVar", obj.RawVar)
    //        dynamicParameters.Add("@strWireSpec", obj.WireSpec);
    //        dynamicParameters.Add("@strRepFrom", obj.RepFrom);
    //        dynamicParameters.Add("@strRepTo", obj.RepTo);
    //        dynamicParameters.Add("@strRep", obj.Rep);
    //        dynamicParameters.Add("@bitTwinWire", DbType.Boolean, obj.TwinWire);
    //        dynamicParameters.Add("@intUserId", obj.UserId);
    //        dynamicParameters.Add("@strOH1", obj.OH1);
    //        dynamicParameters.Add("@strOH2", obj.OH2);
    //        DataAccess.DataAccess.db.AddOutParameter("@Output", 0);
    //        DataAccess.DataAccess.GetScalar(dbcom);
    //        intReturnValue = (dbcom.Parameters["@Output"].Value);
    //        return intReturnValue;
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}

    //public  int DeleteProductBOMDetails(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ProductBOM_Delete");
    //        dynamicParameters.Add("@intProductBOMId", obj.ProductBOMId);
    //        DataAccess.DataAccess.db.AddOutParameter("@intOutput", 0);
    //        DataAccess.DataAccess.GetScalar(dbcom);
    //        intReturnValue = (dbcom.Parameters["@intOutput"].Value);
    //        return intReturnValue;
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}

    //public  DataSet GetRawMaterial()
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("RawMaterial_Get");
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetWireType(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ProductWireType_Get");
    //        dynamicParameters.Add("@intBomMasterHeaderId", obj.BomMasterHeaderId);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public string[] ValidateUser(AdminInfo obj)
    //{
    //    try
    //    {
    //        string[] strResult = new string[7];
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("UserRights_Validate");
    //        // dynamicParameters.Add("@vchLoginId", obj.LoginId)
    //        dynamicParameters.Add("@vchLoginId", "SnehaT_TTL");
    //        // dynamicParameters.Add("@vchLoginId", "AishwaryaP_TTL")
    //        dynamicParameters.Add("@vchFormName", obj.FormName);
    //        DataAccess.DataAccess.db.AddOutParameter("@bitReadAccess", DbType.Boolean, 0);
    //        DataAccess.DataAccess.db.AddOutParameter("@bitWriteAccess", DbType.Boolean, 0);
    //        DataAccess.DataAccess.db.AddOutParameter("@bitApproveAccess", DbType.Boolean, 0);
    //        DataAccess.DataAccess.db.AddOutParameter("@intUserId", 0);
    //        DataAccess.DataAccess.db.AddOutParameter("@vchEmailId", 100);
    //        DataAccess.DataAccess.db.AddOutParameter("@vchUserName", 100);
    //        DataAccess.DataAccess.db.AddOutParameter("@RoleName", 100);
    //        DataAccess.DataAccess.GetScalar(dbcom);

    //        strResult[0] = (dbcom.Parameters["@bitReadAccess"].Value);
    //        strResult[1] = (dbcom.Parameters["@bitWriteAccess"].Value);
    //        strResult[2] = (dbcom.Parameters["@intUserId"].Value);
    //        strResult[3] = (dbcom.Parameters["@vchEmailId"].Value);
    //        strResult[4] = (dbcom.Parameters["@bitApproveAccess"].Value);
    //        strResult[5] = (dbcom.Parameters["@vchUserName"].Value);
    //        strResult[6] = (dbcom.Parameters["@RoleName"].Value);

    //        return strResult;
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}

    //public  DataSet GetCustomer()
    //{
    //    try
    //    {
    //        SqlCacheDependency SqlDep;
    //        DataSet dsCache = (DataSet)HttpRuntime.Cache["CacheCustomer"];
    //        if (dsCache == null)
    //        {
    //            // SqlDep = New SqlCacheDependency("NDSCaching", "customermaster")
    //            SqlDep = new SqlCacheDependency("NDSCaching", "customermaster");
    //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("Customer_Get");
    //            dsCache = DataAccess.DataAccess.GetDataSet(dbcom);
    //            HttpRuntime.Cache.Insert("CacheCustomer", dsCache, SqlDep);
    //        }
    //        return dsCache;
    //    }
    //    // dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("Customer_Get")
    //    // Return DataAccess.DataAccess.GetDataSet(dbcom)
    //    catch (Exception ex)
    //    {
    //    }
    //}
    //// ' Status master
    //public  DataSet GetWBSStatus()
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("WBSStatusMaster_Get");
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  int InsertWBSStatus(AdminInfo obj)
    //{
    //    try
    //    {
    //        Int32 intReturnValue;
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("WBSStatus_Insert");
    //        dynamicParameters.Add("@intStatusId", obj.StatusId);
    //        dynamicParameters.Add("@vchStatusCode", obj.StatusCode);
    //        dynamicParameters.Add("@vchStatus", obj.Status);
    //        dynamicParameters.Add("@vchStatusDesc", obj.StatusDesc);
    //        dynamicParameters.Add("@intUserId", obj.UserId);
    //        DataAccess.DataAccess.db.AddOutParameter("@Output", 0);
    //        DataAccess.DataAccess.GetScalar(dbcom);
    //        intReturnValue = (dbcom.Parameters["@Output"].Value);
    //        return intReturnValue;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}
    //// 'cab product code master
    //public  DataSet GetCabProductCodeMasterDetails()
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("CabProductCodeMasterDetails_Get");
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  int InsertCabProductCodeMasterDetails(AdminInfo obj)
    //{
    //    try
    //    {
    //        Int32 intReturnValue;
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("CabProductCodeMaster_Insert");
    //        dynamicParameters.Add("@intCabProductCodeId", obj.intCabProductCodeID);
    //        dynamicParameters.Add("@chrGradeType", obj.strGradeType);
    //        dynamicParameters.Add("@intDiameter", obj.Diameter);
    //        dynamicParameters.Add("@bitCouplerIndicator", DbType.Byte, obj.bitCouplerIndicator);
    //        dynamicParameters.Add("@vchCouplerType", obj.strCouplerType);
    //        dynamicParameters.Add("@intFGSAPMaterialID", obj.intFGSAPMaterialID);
    //        dynamicParameters.Add("@intRMSAPMaterialID", obj.intRMSAPMaterialID);
    //        dynamicParameters.Add("@tntStatusId", obj.StatusId);
    //        dynamicParameters.Add("@intUserId", obj.UserId);
    //        dynamicParameters.Add("@datCreatedUpdatedDate", DbType.DateTime, obj.datCreatedUpdatedDate);

    //        intReturnValue = System.Convert.ToInt32(DataAccess.DataAccess.GetScalar(dbcom));
    //        return intReturnValue;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  int DeleteCabProductCodeMasterDetails(AdminInfo obj)
    //{
    //    try
    //    {
    //        Int32 intReturnValue;
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("CabProductCodeMasterDetails_Delete");
    //        dynamicParameters.Add("@intCabProductCodeId", obj.intCabProductCodeID);

    //        intReturnValue = System.Convert.ToInt32(DataAccess.DataAccess.GetScalar(dbcom));
    //        return intReturnValue;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //// 'Consignment Master
    //public  int InsertWBSConsignment(AdminInfo obj)
    //{
    //    try
    //    {
    //        Int32 intReturnValue;
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("WBSConsignment_Insert");
    //        dynamicParameters.Add("@intConsignmentId", obj.intConsignmentId);
    //        dynamicParameters.Add("@vchConsignmentType", obj.strConsignmentType);
    //        dynamicParameters.Add("@vchConsignmentDesc", obj.strConsignmentDesc);
    //        dynamicParameters.Add("@intStatusId", obj.StatusId);
    //        dynamicParameters.Add("@intUserId", obj.UserId);
    //        DataAccess.DataAccess.db.AddOutParameter("@Output", 0);
    //        DataAccess.DataAccess.GetScalar(dbcom);
    //        intReturnValue = (dbcom.Parameters["@Output"].Value);
    //        return intReturnValue;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetWBSConsignment()
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("WBSConsignment_Get");
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}
    //// 'Project master
    //public  DataSet GetContract(AdminInfo obj)
    //{
    //    try
    //    {
    //        SqlCacheDependency SqlDep;
    //        DataSet dsCache = (DataSet)HttpRuntime.Cache["CacheContract"];
    //        if (dsCache == null)
    //        {
    //            SqlDep = new SqlCacheDependency("NDSCaching", "contractmaster");
    //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("Contract_Get");
    //            // dynamicParameters.Add("@intCustomerCode", obj.CustomerId)
    //            dsCache = DataAccess.DataAccess.GetDataSet(dbcom);
    //            HttpRuntime.Cache.Insert("CacheContract", dsCache, SqlDep);
    //        }

    //        DataView dvcacheContract;
    //        dvcacheContract = dsCache.Tables[0].DefaultView;
    //        dvcacheContract.RowFilter = "intCustomerCode = " + obj.CustomerId;

    //        DataSet dsfilterContract = new DataSet();
    //        dsfilterContract.Tables.Add(dvcacheContract.ToTable());
    //        return dsfilterContract;
    //    }

    //    // Return dsCache.Tables(0).Select("customer=" + obj.CustomerId)--- this is for single row



    //    // dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("Contract_Get")
    //    // dynamicParameters.Add("@intCustomerCode", obj.CustomerId)
    //    // Return DataAccess.DataAccess.GetDataSet(dbcom)
    //    catch (Exception ex)
    //    {
    //    }
    //}

    //public  DataSet GetProjectType()
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("WBSProjectType_Get");
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetMarketSector()
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("WBSMasterSector_Get");
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetSales()
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("WBSSalesInCharge_Get");
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetProjectInCharge()
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("WBSProjectInCharge_Get");
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetSAPProject2(AdminInfo objinfo)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("SAPProjectForProjectMaster_Get");
    //        dynamicParameters.Add("@intSAPProjectId", objinfo.SAPProjectId);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetSAPProject(AdminInfo obj)
    //{
    //    try
    //    {
    //        // Dim SqlDep As SqlCacheDependency
    //        // Dim dsCache As DataSet = CType(HttpRuntime.Cache("CacheSAPProject"), DataSet)
    //        // If dsCache Is Nothing Then
    //        // SqlDep = New SqlCacheDependency("NDSCaching", "sapprojectmaster")
    //        // dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("SAPProject_Get")
    //        // dsCache = DataAccess.DataAccess.GetDataSet(dbcom)
    //        // HttpRuntime.Cache.Insert("CacheSAPProject", dsCache, SqlDep)
    //        // End If

    //        // Dim dvcacheSAPProject As DataView
    //        // dvcacheSAPProject = dsCache.Tables(0).DefaultView
    //        // dvcacheSAPProject.RowFilter = "ProjId = " & obj.ProjectId

    //        // Dim dsfilterdvcacheSAPProject As New DataSet
    //        // dsfilterdvcacheSAPProject.Tables.Add(dvcacheSAPProject.ToTable())
    //        // Return dsfilterdvcacheSAPProject

    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("SAPProject_Get");
    //        dynamicParameters.Add("@intProjectId", obj.ProjectId);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetSAPTransport()
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("WBSSAPTransport_Get");
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetProjectMaster(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("WBSProjectMaster_Get");
    //        dynamicParameters.Add("@intContractID", obj.ContractId);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  SqlDataReader GetProjectDetails(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("WBSProjectMaster_Select");
    //        dynamicParameters.Add("@intProjectId", obj.ProjectId);
    //        return DataAccess.DataAccess.ExecuteReader(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  int InsertWBSProject(AdminInfo obj)
    //{
    //    try
    //    {
    //        Int32 intReturnValue;
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("WBSProject_Insert");
    //        dynamicParameters.Add("@intContractId", obj.ContractId);
    //        dynamicParameters.Add("@intProjectId", obj.ProjectId);
    //        dynamicParameters.Add("@vchProjectCode", obj.ProjectCode);
    //        dynamicParameters.Add("@vchProjectAbbr", obj.ProjectAbbr);
    //        dynamicParameters.Add("@vchProjectName", obj.ProjectName);
    //        dynamicParameters.Add("@vchaddr1", obj.Address1);
    //        dynamicParameters.Add("@vchaddr2", obj.Address2);
    //        dynamicParameters.Add("@vchaddr3", obj.Address3);
    //        dynamicParameters.Add("@intProjectTypeId", obj.ProjectTypeId);
    //        dynamicParameters.Add("@intMasterSectorId", obj.MarketSectorId);
    //        dynamicParameters.Add("@vchContactPerson", obj.ContactPerson);
    //        dynamicParameters.Add("@vchContact", obj.Contact);
    //        dynamicParameters.Add("@intStatusId", obj.StatusId);
    //        // dynamicParameters.Add("@vchDrawing", obj.DrawingReceived)
    //        dynamicParameters.Add("@intSales1", obj.SalesIncharge1);
    //        dynamicParameters.Add("@intSales2", obj.SalesIncharge2);
    //        dynamicParameters.Add("@intSales3", obj.SalesIncharge3);
    //        dynamicParameters.Add("@intProject1", obj.ProjectIncharge1);
    //        dynamicParameters.Add("@intProject2", obj.ProjectIncharge2);
    //        dynamicParameters.Add("@intProject3", obj.ProjectIncharge3);
    //        dynamicParameters.Add("@dtDeliveryDate", obj.PlanDeliveryDate);
    //        dynamicParameters.Add("@dtStartDate", obj.StartDate);
    //        dynamicParameters.Add("@dtExpiryDate", obj.ExpiryDate);
    //        dynamicParameters.Add("@intSAPProject", obj.SAPProjectId);
    //        dynamicParameters.Add("@vchSAPProjectName", obj.SAPProjectName);
    //        dynamicParameters.Add("@intTransportId", obj.SAPTransportId);
    //        dynamicParameters.Add("@intUserId", obj.UserId);
    //        DataAccess.DataAccess.db.AddOutParameter("@Output", 0);
    //        DataAccess.DataAccess.GetScalar(dbcom);
    //        intReturnValue = (dbcom.Parameters["@Output"].Value);
    //        return intReturnValue;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetContractDetail()
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ContractMaster_Get");
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  int InsertContractDetail(AdminInfo obj)
    //{
    //    try
    //    {
    //        Int32 intReturnValue;
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ContractMaster_Insert");
    //        dynamicParameters.Add("@intContractId", obj.ContractId);
    //        dynamicParameters.Add("@vchContractNo", obj.ContractNo);
    //        dynamicParameters.Add("@vchContractDesc", obj.ContractDesc);
    //        dynamicParameters.Add("@intCustomerCode", obj.Customer);
    //        dynamicParameters.Add("@vchSAPContractNo", obj.SapContractNo);
    //        dynamicParameters.Add("@vchStartDate", obj.StartDt);
    //        dynamicParameters.Add("@vchEndDate", obj.EndDt);
    //        dynamicParameters.Add("@chrStandardType", obj.StandardType);
    //        dynamicParameters.Add("@vchStandardCode", obj.StandardCode);
    //        dynamicParameters.Add("@intStatusId", obj.StatusId);
    //        dynamicParameters.Add("@intUserId", obj.UserId);
    //        DataAccess.DataAccess.db.AddOutParameter("@Output", 0);
    //        DataAccess.DataAccess.GetScalar(dbcom);
    //        intReturnValue = (dbcom.Parameters["@Output"].Value);
    //        return intReturnValue;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetDetailExploreMenu()
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("DetailExploreMenu_Get");
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}
    //// -----Project Parameter------'
    //public  DataSet GetTransport()
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("Trasport_Get");
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetCapCLinkProjectParameter(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ProjectParameterCapCLink_Get");
    //        dynamicParameters.Add("@intProjectId", obj.ProjectId);
    //        dynamicParameters.Add("@tntParamSetNumber", obj.ParameterSetNo);
    //        dynamicParameters.Add("@vchParameterType", obj.ParameterType);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    public List<GetProjectParameterDto> GetProjectParameter(AdminInfo obj)
    {
        try
        {
            IEnumerable<GetProjectParameterDto>getProjectParameters;
            DataSet dGetProjectParamDrainWM = new DataSet();
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@intProjectId", obj.ProjectId);
                dynamicParameters.Add("@tntParamSetNumber", obj.ParameterSetNo);
                dynamicParameters.Add("@vchParameterType", obj.ParameterType);
                getProjectParameters = sqlConnection.Query<GetProjectParameterDto>(SystemConstants.ProjectParameter_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();

                return getProjectParameters.ToList();
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    //public  DataSet GetProjectParameterType(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ProjectParameterType_Get");
    //        dynamicParameters.Add("@tntParamSetNumber", obj.ParameterSetNo);

    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    public List<GetParameterSetDto> GetParameterSet(AdminInfo obj)
    {
        try
        {
            IEnumerable<GetParameterSetDto> getParameterSets;
            DataSet dsProjectParamDrain = new DataSet();
            using (var sqlConnection = new SqlConnection(connectionString))
            {

                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@intProjectId", obj.ProjectId);
                dynamicParameters.Add("@vchStructureElement", obj.StructureElement);
                dynamicParameters.Add("@sitProductTypeL2Id", obj.ProductType);
                getParameterSets = sqlConnection.Query<GetParameterSetDto>(SystemConstants.ParameterSet_Get_PV, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return getParameterSets.ToList();

            }
            
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    //public  DataSet GetGroupMarkingName(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("GroupMarkingName_Get");
    //        dynamicParameters.Add("@intProjectId", obj.ProjectId);
    //        dynamicParameters.Add("@intstructureElementTypeid", obj.StructureElementTypeId);
    //        dynamicParameters.Add("@sitproductTypeid", obj.ProductType);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet CopyParamGet_Get(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("CopyParamGet_Get");
    //        dynamicParameters.Add("@intProjectId", obj.ProjectId);
    //        dynamicParameters.Add("@vchGroupmarkingname", obj.GMName);
    //        dynamicParameters.Add("@tntGroupRevNo", DbType.Int16, obj.GroupRevNo);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetParamStandard()
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("CPStandard_Select");
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetConsignment()
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("Consignment_Get");
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    public int[] InsertProjectParameter(InsertProjectParameterDto obj, out string errorMessage)
    {
        int[] intResult = new int[2];
        errorMessage = "";
        try
        {
           
            using (var sqlConnection = new SqlConnection(connectionString))
            {

                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@tntParamSetNumber", obj.tntParamSetNumber);
                dynamicParameters.Add("@intProjectId", obj.intProjectId);
                dynamicParameters.Add("@sitProductTypeL2Id", obj.sitProductTypeL2Id);
                dynamicParameters.Add("@tntTransportModeId", obj.tntTransportModeId);
                dynamicParameters.Add("@sitTopCover", obj.sitTopCover);
                dynamicParameters.Add("@sitBottomCover", obj.sitBottomCover);
                dynamicParameters.Add("@sitLeftCover", obj.sitLeftCover);
                dynamicParameters.Add("@sitRightCover", obj.sitRightCover);
                dynamicParameters.Add("@sitInnerCover", obj.sitInnerCover);
                dynamicParameters.Add("@sitOuterCover", obj.sitOuterCover);
                dynamicParameters.Add("@sitGap1", obj.sitGap1);
                dynamicParameters.Add("@sitGap2", obj.sitGap2);
                dynamicParameters.Add("@chrStandardCP", obj.chrStandardCP);
                dynamicParameters.Add("@tntStatusId", obj.tntStatusId);
                dynamicParameters.Add("@intParameterSet", obj.intParameterSet);
                dynamicParameters.Add("@intMo1", obj.intMo1);
                dynamicParameters.Add("@intMo2", obj.intMo2);
                dynamicParameters.Add("@intCo1", obj.intCo1);
                dynamicParameters.Add("@intCo2", obj.intCo2);
                dynamicParameters.Add("@chrStandarCL", obj.chrStandarCL);
                dynamicParameters.Add("@chrCLMaterialType", obj.chrCLMaterialType);
                dynamicParameters.Add("@vchParameterType", obj.vchParameterType);
                dynamicParameters.Add("@bitStructureMarkingLevel", obj.bitStructureMarkingLevel);
                dynamicParameters.Add("@sitHook", obj.sitHook);
                dynamicParameters.Add("@sitLeg ", obj.sitLeg);
                dynamicParameters.Add("@tntRefParamSetNumber ", obj.tntRefParamSetNumber);
                dynamicParameters.Add("@sitMWLap", obj.sitMWLap);
                dynamicParameters.Add("@sitCWLap", obj.sitCWLap);
                dynamicParameters.Add("@sitProductTypeId", obj.sitProductTypeId);
                dynamicParameters.Add("@intUserId", obj.intUserId);
                dynamicParameters.Add("@vchDescription", obj.vchDescription);
                dynamicParameters.Add("@Output", 0);
                dynamicParameters.Add("@intScopeIdentity", 0);


                // dynamicParameters.Add("@Output", null, dbType: DbType.Int32, ParameterDirection.Output);

                sqlConnection.QueryFirstOrDefault<int>(SystemConstants.ProjectParameter_Insert, dynamicParameters, commandType: CommandType.StoredProcedure);

                intResult[0] = dynamicParameters.Get<int>("@Output");
                intResult[1] = dynamicParameters.Get<int>("@intScopeIdentity");
                sqlConnection.Close();
                return intResult;
            }
 
            
        }
        catch (Exception ex)
        {
            errorMessage = ex.Message;
        }
        return intResult;
    }

    //public  int[] InsertProjectParameterOHHeader(AdminInfo obj)
    //{
    //    try
    //    {
    //        int[] intResult = new int[2];
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ProjectParameterOHHeader_Insert");
    //        dynamicParameters.Add("@tntParamSetNumber", obj.ParameterSetNo);
    //        dynamicParameters.Add("@intProjectId", obj.ProjectId);
    //        dynamicParameters.Add("@sitProductTypeL2Id", obj.ProductType);
    //        dynamicParameters.Add("@tntTransportModeId", obj.TransportModeId);
    //        dynamicParameters.Add("@tntStatusId", obj.StatusId);
    //        dynamicParameters.Add("@intParameterSet", obj.ParameterSet);
    //        dynamicParameters.Add("@vchParameterType", obj.ParameterType);
    //        dynamicParameters.Add("@bitStructureMarkingLevel", DbType.Byte, obj.StructureMarkingLevel);
    //        dynamicParameters.Add("@sitProductTypeId", obj.ParamOHProduct);
    //        dynamicParameters.Add("@intUserId", obj.UserId);
    //        dynamicParameters.Add("@vchDescription", obj.Description);
    //        DataAccess.DataAccess.db.AddOutParameter("@Output", 0);
    //        DataAccess.DataAccess.db.AddOutParameter("@intScopeIdentity", 0);
    //        DataAccess.DataAccess.GetScalar(dbcom);
    //        intResult[0] = (dbcom.Parameters["@Output"].Value);
    //        intResult[1] = (dbcom.Parameters["@intScopeIdentity"].Value);
    //        return intResult;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  int InsertProjectParamCage(AdminInfo obj)
    //{
    //    try
    //    {
    //        Int32 intReturnValue;
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ProjectParamCage_Insert");
    //        dynamicParameters.Add("@tntParamCageId", obj.ParamCageId);
    //        dynamicParameters.Add("@intProductCodeId", obj.intProductCodeId);
    //        dynamicParameters.Add("@tntParamSetNumber", obj.ParameterSetNo);
    //        dynamicParameters.Add("@chrStandard", obj.StandardCP);
    //        dynamicParameters.Add("@intDiameter", obj.Diameter);
    //        dynamicParameters.Add("@intStdCCLProductID", obj.StdCCLProductID);
    //        dynamicParameters.Add("@sitHook", obj.Hook);
    //        dynamicParameters.Add("@sitLeg", obj.Leg);
    //        dynamicParameters.Add("@length", obj.Length);
    //        DataAccess.DataAccess.db.AddOutParameter("@Output", 0);
    //        DataAccess.DataAccess.GetScalar(dbcom);
    //        intReturnValue = (dbcom.Parameters["@Output"].Value);
    //        return intReturnValue;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  int InsertProjectParamOH(AdminInfo obj)
    //{
    //    try
    //    {
    //        Int32 intReturnValue;
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ProjectParamMeshOH_Insert");
    //        dynamicParameters.Add("@intParamOHId", obj.ParamOHId);
    //        dynamicParameters.Add("@intProjectId", obj.ProjectId);
    //        dynamicParameters.Add("@sitProductTypeL2Id", obj.ProductType);
    //        dynamicParameters.Add("@intStructureElementTypeId", obj.StructureElementTypeId);
    //        dynamicParameters.Add("@vchConsignmentTypeName", obj.consignmentType);
    //        dynamicParameters.Add("@tntParamSetNumber", obj.ParameterSetNo);
    //        dynamicParameters.Add("@intMWSpace", obj.MWSpace);
    //        dynamicParameters.Add("@intCWSpace", obj.CWSpace);
    //        dynamicParameters.Add("@intEvenMO1", obj.EvenMO1);
    //        dynamicParameters.Add("@intEvenMO2", obj.EvenMO2);
    //        dynamicParameters.Add("@intEvenCO1", obj.EvenCO1);
    //        dynamicParameters.Add("@intEvenCO2", obj.EvenCO2);
    //        dynamicParameters.Add("@intOddMO1", obj.OddMO1);
    //        dynamicParameters.Add("@intOddMO2", obj.OddMO2);
    //        dynamicParameters.Add("@intOddCO1", obj.OddCO1);
    //        dynamicParameters.Add("@intOddCO2", obj.OddCO2);
    //        dynamicParameters.Add("@tntStatusId", obj.StatusId);
    //        dynamicParameters.Add("@intUserId", obj.UserId);
    //        DataAccess.DataAccess.db.AddOutParameter("@Output", 0);
    //        DataAccess.DataAccess.GetScalar(dbcom);
    //        intReturnValue = (dbcom.Parameters["@Output"].Value);
    //        return intReturnValue;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  int InsertProjectParamLap(AdminInfo obj)
    //{
    //    try
    //    {
    //        Int32 intReturnValue;
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ProjectParamMeshLap_Insert");
    //        dynamicParameters.Add("@intMeshLapId", obj.MeshLapId);
    //        dynamicParameters.Add("@intProjectId", obj.ProjectId);
    //        dynamicParameters.Add("@tntParamSetNumber", obj.ParameterSetNo);
    //        dynamicParameters.Add("@sitProductTypeL2Id", obj.ProductType);
    //        dynamicParameters.Add("@intStructureElementTypeId", obj.StructureElementTypeId);
    //        dynamicParameters.Add("@intProductCodeId", obj.intProductCodeId);
    //        dynamicParameters.Add("@sitMWLap", obj.MWLap);
    //        dynamicParameters.Add("@sitCWLap", obj.CWLap);
    //        dynamicParameters.Add("@tntStatusId", obj.StatusId);
    //        dynamicParameters.Add("@vchConsignmentTypeName", obj.consignmentType);
    //        dynamicParameters.Add("@intUserId", obj.UserId);
    //        DataAccess.DataAccess.db.AddOutParameter("@Output", 0);
    //        DataAccess.DataAccess.GetScalar(dbcom);
    //        intReturnValue = (dbcom.Parameters["@Output"].Value);
    //        return intReturnValue;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetProjectParamCage(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ProjectParameterCage_Get");
    //        // dynamicParameters.Add("@tntParamCageId", obj.ParamCageId)
    //        dynamicParameters.Add("@vchStructureElement", obj.StructureElement);

    //        dynamicParameters.Add("@tntParamSetNumber", obj.ParameterSetNo);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetProjectParamOH(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ProjectParamOH_Get");
    //        // dynamicParameters.Add("@tntParamCageId", obj.ParamCageId)
    //        dynamicParameters.Add("@tntParamSetNumber", obj.ParameterSetNo);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetParamSelect(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("Param_Select");
    //        dynamicParameters.Add("@intProjectId", obj.ProjectId);
    //        dynamicParameters.Add("@sitProductTypeL2Id", obj.ProductType);
    //        dynamicParameters.Add("@intParameteSet", obj.ParameterSet);

    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetParamMeshSelect(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ParamMesh_Select");
    //        dynamicParameters.Add("@intProjectId", obj.ProjectId);
    //        dynamicParameters.Add("@sitProductTypeId", obj.ProductType);
    //        dynamicParameters.Add("@intParameteSet", obj.ParameterSet);

    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetProductTypeParam(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ParamProductType_Get");
    //        dynamicParameters.Add("@vchParameterType", obj.ParameterType);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetProductTypeParamter(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ProductTypeParam_Get");
    //        dynamicParameters.Add("@vchParameterType", obj.ParameterType);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetCappingParamSet(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("CappingParamSet_Get");
    //        dynamicParameters.Add("@intProjectId", obj.ProjectId);
    //        dynamicParameters.Add("@vchProductType", obj.ProductTypeL2);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetCapClinkParameterSet(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("CappingCLinkParameterSet_Get");
    //        dynamicParameters.Add("@intProjectId", obj.ProjectId);
    //        dynamicParameters.Add("@vchProductType", obj.ProductTypeL2);
    //        dynamicParameters.Add("@chrStandard", obj.StandardCP);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetProductCodeParam(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ParamProductCode_Get");
    //        dynamicParameters.Add("@sitProductTypeL2Id", obj.ProductType);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetCappingProductCodeParam(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("CappingParamProductCode_Get");
    //        dynamicParameters.Add("@sitProductTypeL2Id", obj.ProductType);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetCopyGroupmarking(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("CopyGroupmarking_Get");
    //        dynamicParameters.Add("@intProjectId", obj.ProjectId);
    //        dynamicParameters.Add("@intStructureElementTypeId", obj.CopyStructureElement);
    //        dynamicParameters.Add("@sitProductTypeId", obj.ProductType);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetCopyGroupmarkingRevision(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("CopyGroupmarkingRevision_Get");
    //        dynamicParameters.Add("@intProjectId", obj.ProjectId);
    //        dynamicParameters.Add("@intStructureElementTypeId", obj.CopyStructureElement);
    //        dynamicParameters.Add("@sitProductTypeId", obj.ProductType);
    //        dynamicParameters.Add("@vchGroupmarkingname", obj.GMName);

    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetGroupmarkingRevQuan(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("GroupmarkingRevisionVersion_Get");
    //        dynamicParameters.Add("@intProjectId", obj.ProjectId);
    //        dynamicParameters.Add("@intStructureElementTypeId", obj.CopyStructureElement);
    //        dynamicParameters.Add("@sitProductTypeId", obj.ProductType);
    //        dynamicParameters.Add("@intgroupmarkid", obj.GroupMarkingId);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetCopyStructureMarking(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("CopyStructureMarking_Get");
    //        dynamicParameters.Add("@intGroupMarkId", obj.GroupMarkingId);
    //        dynamicParameters.Add("@intStructureElementTypeId", obj.CopyStructureElement);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetShapeCode(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ShapeCode_Get");
    //        dynamicParameters.Add("@intShapeTransHeaderId", obj.ShapeTransHeaderId);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //     'Public Overrides Function GetShapeDetails(ByVal obj As AdminInfo) As DataSet
    //'    Try
    //'        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ShapeDetails_Get")
    //'        DataAccess.DataAccess.db.AddInParameter(dbcom, "@vchMWLength", DbType.String, obj.MWLength)
    //'        DataAccess.DataAccess.db.AddInParameter(dbcom, "@vchMWLength", DbType.String, obj.MWLength)
    //'        DataAccess.DataAccess.db.AddInParameter(dbcom, "@vchShapeCode", DbType.String, obj.vchShapeCode)
    //'        DataAccess.DataAccess.db.AddInParameter(dbcom, "@vchProductCode", DbType.String, obj.ProductCode)
    //'        Return DataAccess.DataAccess.GetDataSet(dbcom)
    //'    Catch ex As Exception
    //'        Throw ex
    //'    End Try
    //'End Function


    //// REPLACED BY KATHEESH ON ADVICE FROM SOUMYAJIT
    //public  DataSet GetShapeDetails(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ShapeDetails_Get");
    //        dynamicParameters.Add("@vchMWLength", obj.MWLength);
    //        dynamicParameters.Add("@vchCWLength", obj.CWLength);
    //        dynamicParameters.Add("@vchShapeCode", obj.vchShapeCode);
    //        dynamicParameters.Add("@vchProductCode", obj.ProductCode);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}


    //public  DataSet GetGroupMarkingParameterSet(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("GroupMarkingParameterSet_Get");
    //        dynamicParameters.Add("@intProjectId", obj.ProjectId);
    //        dynamicParameters.Add("@sitProductTypeId", obj.ProductType);
    //        dynamicParameters.Add("@intStructureElementTypeId", obj.CopyStructureElement);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //// Function Overloaded for Core Cage Development'
    //public new System.Data.DataSet GetProductCode(bool coreCage)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("CARProductCodeMaster_Get");
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetProductCode(AdminInfo obj)
    //{
    //    try
    //    {
    //        SqlCacheDependency SqlDep;
    //        DataSet dsCache = (DataSet)HttpRuntime.Cache["CacheSlabProduct"];
    //        if (dsCache == null)
    //        {
    //            SqlDep = new SqlCacheDependency("NDSCaching", "productcodemaster");
    //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ProductCodeMesh_Get");
    //            dynamicParameters.Add("@sitProductTypeId", obj.ProductType);
    //            dsCache = DataAccess.DataAccess.GetDataSet(dbcom);
    //            HttpRuntime.Cache.Insert("CacheSlabProduct", dsCache, SqlDep);
    //        }
    //        return dsCache;
    //    }
    //    // dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ProductCodeMesh_Get")
    //    // dynamicParameters.Add("@sitProductTypeId", obj.ProductType)
    //    // Return DataAccess.DataAccess.GetDataSet(dbcom)
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet InsertCopyGroupMarking(AdminInfo obj)
    //{
    //    try
    //    {
    //        Int32 intReturnValue;
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("CopyGroupMarking_Insert");
    //        dynamicParameters.Add("@intSourceGroupMarkId", obj.GroupMarkingId);
    //        dynamicParameters.Add("@vchSourceGroupMarkingName", obj.SourceGroupMarking);
    //        dynamicParameters.Add("@vchSourceStructureMarking", obj.SourceStructureMarking);
    //        dynamicParameters.Add("@intSourceProjectId", obj.ProjectId);
    //        dynamicParameters.Add("@intSourceParamSetNumber", obj.ParameterSetNo);
    //        dynamicParameters.Add("@tntDestGroupRevNo", obj.DestGroupRevNo);
    //        dynamicParameters.Add("@intDestProjectId", obj.DestProjectId);
    //        dynamicParameters.Add("@intDestStructureElementTypeId", obj.DestStructureElementTypeId);
    //        dynamicParameters.Add("@sitDestProductTypeId", obj.DestProductTypeId);
    //        dynamicParameters.Add("@vchDestGroupMarkingName", obj.DestGroupMarkingName);
    //        dynamicParameters.Add("@tntDestParamSetNumber", obj.DestParamSetNumber);
    //        dynamicParameters.Add("@vchDestRemarks", obj.DestRemarks);
    //        dynamicParameters.Add("@tntDestStatusId", obj.DestStatusId);
    //        dynamicParameters.Add("@intDestCreatedUId", obj.DestCreatedUId);
    //        dynamicParameters.Add("@vchCopyFrom", obj.CopyFrom);
    //        dynamicParameters.Add("@strWBSElementID", obj.strWBSElementID);
    //        // DataAccess.DataAccess.db.AddOutParameter("@Output", 0)
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  int DeleteProductParamCage(AdminInfo obj)
    //{
    //    try
    //    {
    //        Int32 intReturnValue;
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ProjectParamCage_Delete ");
    //        dynamicParameters.Add("@tntParamCageId", obj.ParamCageId);
    //        DataAccess.DataAccess.db.AddOutParameter("@Output", 0);
    //        DataAccess.DataAccess.GetScalar(dbcom);
    //        intReturnValue = (dbcom.Parameters["@Output"].Value);
    //        return intReturnValue;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  int DeleteProductParamOH(AdminInfo obj)
    //{
    //    try
    //    {
    //        Int32 intReturnValue;
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ParamOH_Delete");
    //        dynamicParameters.Add("@intParamOHId", obj.ParamOHId);
    //        DataAccess.DataAccess.db.AddOutParameter("@Output", 0);
    //        DataAccess.DataAccess.GetScalar(dbcom);
    //        intReturnValue = (dbcom.Parameters["@Output"].Value);
    //        return intReturnValue;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  int DeleteProductParamLap(AdminInfo obj)
    //{
    //    try
    //    {
    //        Int32 intReturnValue;
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ParamLap_Delete");
    //        dynamicParameters.Add("@intMeshLapId", obj.MeshLapId);
    //        DataAccess.DataAccess.db.AddOutParameter("@Output", 0);
    //        DataAccess.DataAccess.GetScalar(dbcom);
    //        intReturnValue = (dbcom.Parameters["@Output"].Value);
    //        return intReturnValue;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  SqlDataReader GetWireDiameter(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("WireDiameter_Get");
    //        dynamicParameters.Add("@chrWireType", obj.WireType);
    //        dynamicParameters.Add("@intProductCodeId", obj.RawMaterial);
    //        return DataAccess.DataAccess.ExecuteReader(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}

    //public  SqlDataReader GetWireLength(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("WireLength_Get");
    //        dynamicParameters.Add("@intProductCodeId", obj.ProductCodeId);
    //        return DataAccess.DataAccess.ExecuteReader(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}

    //public  int InsertShapeCode(AdminInfo obj)
    //{
    //    try
    //    {
    //        Int32 intReturnValue;
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ShapeCode_Insert");
    //        dynamicParameters.Add("@intShapeTransHeaderId", obj.ShapeTransHeaderId);
    //        dynamicParameters.Add("@intShapeId", obj.ShapeId);
    //        dynamicParameters.Add("@vchShapeDescription", obj.ShapeDescription);
    //        dynamicParameters.Add("@nvchParamValues", obj.ParamValues);
    //        dynamicParameters.Add("@intUserId", obj.UserId);
    //        DataAccess.DataAccess.db.AddOutParameter("@Output", 0);
    //        DataAccess.DataAccess.ExecuteNonQuery(dbcom);
    //        intReturnValue = (dbcom.Parameters["@Output"].Value);
    //        return intReturnValue;
    //    }

    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetShape()
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("Shape_Get");
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //// ' IF- Condition Added for Core Cage Development

    //public  System.Data.DataSet GetProductCodeSearch(AdminInfo obj)
    //{
    //    try
    //    {
    //        if (obj.CoreCage == true)
    //        {
    //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("CARProductCodeMaster_SearchGet");
    //            dynamicParameters.Add("@vchProductCode", obj.ProductCode);
    //            dynamicParameters.Add("@vchProductDescription", obj.ProductDesc);
    //            dynamicParameters.Add("@intSapMaterialId", obj.strSapMaterial);
    //            dynamicParameters.Add("@vchWeightArea", obj.WeightSqm);
    //        }
    //        else
    //        {
    //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ProductCodeMaster_SearchGet");
    //            dynamicParameters.Add("@vchProductCode", obj.ProductCode);
    //            dynamicParameters.Add("@vchProductDescription", obj.ProductDesc);
    //            dynamicParameters.Add("@intSapMaterialId", obj.strSapMaterial);
    //            dynamicParameters.Add("@vchTwinInd", obj.TwinInd);
    //            dynamicParameters.Add("@vchWeightArea", obj.WeightSqm);
    //            dynamicParameters.Add("@chrBOMIndicator", obj.BomIndicator);
    //            dynamicParameters.Add("@vchMWLength", obj.MWLength);
    //            dynamicParameters.Add("@intMWSpace", obj.strMWSpace);
    //            dynamicParameters.Add("@vchCWLength", obj.CWLength);
    //            dynamicParameters.Add("@intCWSpace", obj.strCWSpace);
    //            dynamicParameters.Add("@intShapeCodeId", obj.ShapeDescription);
    //            dynamicParameters.Add("@intMWProductCodeId", obj.MWProduct);
    //            dynamicParameters.Add("@intCWProductCodeId", obj.CWProduct);
    //        }
    //        // Origional Logic Commented for Core Cage modification

    //        // dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ProductCodeMaster_SearchGet")
    //        // dynamicParameters.Add("@vchProductCode", obj.ProductCode)
    //        // dynamicParameters.Add("@vchProductDescription", obj.ProductDesc)
    //        // dynamicParameters.Add("@intSapMaterialId", obj.strSapMaterial)
    //        // dynamicParameters.Add("@vchTwinInd", obj.TwinInd)
    //        // dynamicParameters.Add("@vchWeightArea", obj.WeightSqm)
    //        // dynamicParameters.Add("@chrBOMIndicator", obj.BomIndicator)
    //        // dynamicParameters.Add("@vchMWLength", obj.MWLength)
    //        // dynamicParameters.Add("@intMWSpace", obj.strMWSpace)
    //        // dynamicParameters.Add("@vchCWLength", obj.CWLength)
    //        // dynamicParameters.Add("@intCWSpace", obj.strCWSpace)
    //        // dynamicParameters.Add("@intShapeCodeId", obj.ShapeDescription)
    //        // dynamicParameters.Add("@intMWProductCodeId", obj.MWProduct)
    //        // dynamicParameters.Add("@intCWProductCodeId", obj.CWProduct)
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}
    //// '''''''''''''''''''''''''''''Report''''''''''''''''''''''''''''
    //public  DataSet GetReportBeamProductDetailsHeader(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("RptBeamProductDetails_List_Header");
    //        dynamicParameters.Add("@intCustomerID", obj.CustomerId);
    //        dynamicParameters.Add("@intContractID", obj.ContractId);
    //        dynamicParameters.Add("@intProjectID", obj.ProjectId);

    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetReportBeamProductDetailsDetails(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("RptBeamProductDetails_List_Details");
    //        dynamicParameters.Add("@intCustomerID", obj.CustomerId);
    //        dynamicParameters.Add("@intContractID", obj.ContractId);
    //        dynamicParameters.Add("@intProjectID", obj.ProjectId);

    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetReportColumnProductDetails(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("RptColumnProductDetails_List");
    //        dynamicParameters.Add("@intCustomerID", obj.CustomerId);
    //        dynamicParameters.Add("@intContractID", obj.ContractId);
    //        dynamicParameters.Add("@intProjectID", obj.ProjectId);
    //        dynamicParameters.Add("@intGroupMarkId", obj.strGroupMarkId);
    //        dynamicParameters.Add("@sitProductTypeId", obj.ProductType);

    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetReportMeshUnitTypeListHeader(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("RptMeshUnitType_Listing_Header");
    //        dynamicParameters.Add("@intCustomerID", obj.CustomerId);
    //        dynamicParameters.Add("@intContractID", obj.ContractId);
    //        dynamicParameters.Add("@intProjectID", obj.ProjectId);

    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetReportMeshUnitTypeListDetails(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("RptMeshUnitType_List_Details");
    //        dynamicParameters.Add("@intCustomerID", obj.CustomerId);
    //        dynamicParameters.Add("@intContractID", obj.ContractId);
    //        dynamicParameters.Add("@intProjectID", obj.ProjectId);

    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetReportBeamProductDetails(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("RptBeamProductDetails_List");
    //        dynamicParameters.Add("@intCustomerID", obj.CustomerId);
    //        dynamicParameters.Add("@intContractID", obj.ContractId);
    //        dynamicParameters.Add("@intProjectID", obj.ProjectId);
    //        dynamicParameters.Add("@intGroupMarkId", obj.strGroupMarkId);
    //        dynamicParameters.Add("@sitProductTypeId", obj.ProductType);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetReportDrainProductDetails(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("RptDrainProductDetails_List");
    //        dynamicParameters.Add("@intCustomerID", obj.CustomerId);
    //        dynamicParameters.Add("@intContractID", obj.ContractId);
    //        dynamicParameters.Add("@intProjectID", obj.ProjectId);
    //        dynamicParameters.Add("@intGroupMarkId", obj.strGroupMarkId);
    //        dynamicParameters.Add("@sitProductTypeId", obj.ProductType);
    //        dynamicParameters.Add("@IsSummaryReport", obj.SummaryReport);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetReportMeshSummaryDetails(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("RptMeshSummaryByProduct");
    //        dynamicParameters.Add("@intCustomerID", obj.CustomerId);
    //        dynamicParameters.Add("@intContractID", obj.ContractId);
    //        dynamicParameters.Add("@intProjectID", obj.ProjectId);
    //        dynamicParameters.Add("@intStructureElementId", obj.StructureElementTypeId);
    //        dynamicParameters.Add("@sitProductTypeId", obj.ProductType);
    //        dynamicParameters.Add("@intWBSElementsId", obj.strWBSElementID);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetReportMeshUnitTypeList(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("RptMeshUnitType_List");
    //        dynamicParameters.Add("@intCustomerID", obj.CustomerId);
    //        dynamicParameters.Add("@intContractID", obj.ContractId);
    //        dynamicParameters.Add("@intProjectID", obj.ProjectId);

    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetReportMeshProductMarkingList(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("RptMeshProductMarking_List");
    //        dynamicParameters.Add("@intCustomerID", obj.CustomerId);
    //        dynamicParameters.Add("@intContractID", obj.ContractId);
    //        dynamicParameters.Add("@intProjectID", obj.ProjectId);
    //        dynamicParameters.Add("@intGroupMarkId", obj.strGroupMarkId);
    //        dynamicParameters.Add("@sitProductTypeId", obj.ProductType);


    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetReportBeamBOM(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("RptBeamBOM_Get");
    //        dynamicParameters.Add("@intCustomerID", obj.CustomerId);
    //        dynamicParameters.Add("@intContractID", obj.ContractId);
    //        dynamicParameters.Add("@intProjectID", obj.ProjectId);
    //        dynamicParameters.Add("@intGroupMarkingId", obj.strGroupMarkId);
    //        dynamicParameters.Add("@intProductMarkingId", obj.ProductMarkingId);
    //        dynamicParameters.Add("@nvchBOMType", obj.BomType);

    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetReportColumnBOM(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("RptColumnBOM_Get");
    //        dynamicParameters.Add("@intCustomerID", obj.CustomerId);
    //        dynamicParameters.Add("@intContractID", obj.ContractId);
    //        dynamicParameters.Add("@intProjectID", obj.ProjectId);
    //        dynamicParameters.Add("@intGroupMarkingId", obj.strGroupMarkId);
    //        dynamicParameters.Add("@intProductMarkingId", obj.ProductMarkingId);
    //        dynamicParameters.Add("@nvchBOMType", obj.BomType);

    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetReportMeshBOM(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("RptMeshBOM_Get");
    //        dynamicParameters.Add("@intCustomerID", obj.CustomerId);
    //        dynamicParameters.Add("@intContractID", obj.ContractId);
    //        dynamicParameters.Add("@intProjectID", obj.ProjectId);
    //        dynamicParameters.Add("@intGroupMarkingId", obj.strGroupMarkId);
    //        dynamicParameters.Add("@intProductMarkingId", obj.ProductMarkingId);
    //        dynamicParameters.Add("@nvchBOMType", obj.BomType);

    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetReportDrainBOM(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("RptDrainBOM_Get");
    //        dynamicParameters.Add("@intCustomerID", obj.CustomerId);
    //        dynamicParameters.Add("@intContractID", obj.ContractId);
    //        dynamicParameters.Add("@intProjectID", obj.ProjectId);
    //        dynamicParameters.Add("@intGroupMarkingId", obj.strGroupMarkId);
    //        dynamicParameters.Add("@intProductMarkingId", obj.ProductMarkingId);
    //        dynamicParameters.Add("@nvchBOMType", obj.BomType);

    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetGroupMarkingForProject(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("GroupMarking_ForProject");
    //        dynamicParameters.Add("@intCustomerID", obj.CustomerId);
    //        dynamicParameters.Add("@intContractID", obj.ContractId);
    //        dynamicParameters.Add("@intProjectID", obj.ProjectId);
    //        dynamicParameters.Add("@VchStructureElement", obj.ElementType);
    //        dynamicParameters.Add("@sitProductTypeId", obj.ProductType);

    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetReportWbsPostingDetailsBeam(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("RptWbsPostingBeam_Get");
    //        dynamicParameters.Add("@intCustomerID", obj.CustomerId);
    //        dynamicParameters.Add("@intContractID", obj.ContractId);
    //        dynamicParameters.Add("@intProjectID", obj.ProjectId);
    //        dynamicParameters.Add("@intStructureElementId", obj.StructureElementTypeId);
    //        dynamicParameters.Add("@sitProductTypeId", obj.ProductType);
    //        dynamicParameters.Add("@intWBSElementsId", obj.strWBSElementID);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetReportWbsPostingDetailsColumn(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("RptWbsPostingColumn_Get");
    //        dynamicParameters.Add("@intCustomerID", obj.CustomerId);
    //        dynamicParameters.Add("@intContractID", obj.ContractId);
    //        dynamicParameters.Add("@intProjectID", obj.ProjectId);
    //        dynamicParameters.Add("@intStructureElementId", obj.StructureElementTypeId);
    //        dynamicParameters.Add("@sitProductTypeId", obj.ProductType);
    //        dynamicParameters.Add("@intWBSElementsId", obj.strWBSElementID);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetReportWbsPostingDetailsSlab(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("RptWbsPostingSlab_Get");
    //        dynamicParameters.Add("@intCustomerID", obj.CustomerId);
    //        dynamicParameters.Add("@intContractID", obj.ContractId);
    //        dynamicParameters.Add("@intProjectID", obj.ProjectId);
    //        dynamicParameters.Add("@intStructureElementId", obj.StructureElementTypeId);
    //        dynamicParameters.Add("@sitProductTypeId", obj.ProductType);
    //        dynamicParameters.Add("@intWBSElementsId", obj.strWBSElementID);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetReportWbsPostingDetailsCAB(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("RptWbsPostingCAB_Get");
    //        dynamicParameters.Add("@intCustomerID", obj.CustomerId);
    //        dynamicParameters.Add("@intContractID", obj.ContractId);
    //        dynamicParameters.Add("@intProjectID", obj.ProjectId);
    //        dynamicParameters.Add("@intStructureElementId", obj.StructureElementTypeId);
    //        dynamicParameters.Add("@sitProductTypeId", obj.ProductType);
    //        dynamicParameters.Add("@vchBbsNo", obj.BBS_No);
    //        dynamicParameters.Add("@intWBSElementsId", obj.strWBSElementID);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetReportWbsPostedDetailsCAB(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("RptWbsPostedDetailCAB_Get");
    //        dynamicParameters.Add("@intCustomerID", obj.CustomerId);
    //        dynamicParameters.Add("@intContractID", obj.ContractId);
    //        dynamicParameters.Add("@intProjectID", obj.ProjectId);
    //        dynamicParameters.Add("@intStructureElementId", obj.StructureElementTypeId);
    //        dynamicParameters.Add("@sitProductTypeId", obj.ProductType);
    //        dynamicParameters.Add("@intWBSElementsId", obj.strWBSElementID);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetReportWbsPostingDetailsPrecage(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("RptWbsPostingPrecage_Get");
    //        dynamicParameters.Add("@intCustomerID", obj.CustomerId);
    //        dynamicParameters.Add("@intContractID", obj.ContractId);
    //        dynamicParameters.Add("@intProjectID", obj.ProjectId);
    //        dynamicParameters.Add("@intStructureElementId", obj.StructureElementTypeId);
    //        dynamicParameters.Add("@sitProductTypeId", obj.ProductType);
    //        dynamicParameters.Add("@intWBSElementsId", obj.strWBSElementID);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetReportWbsPostingDetailsBPC(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("RptWbsPostingBPC_Get");
    //        dynamicParameters.Add("@intCustomerID", obj.CustomerId);
    //        dynamicParameters.Add("@intContractID", obj.ContractId);
    //        dynamicParameters.Add("@intProjectID", obj.ProjectId);
    //        dynamicParameters.Add("@intStructureElementId", obj.StructureElementTypeId);
    //        dynamicParameters.Add("@sitProductTypeId", obj.ProductType);
    //        dynamicParameters.Add("@intWBSElementsId", obj.strWBSElementID);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetReportWbsPostingDetailsDrain(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("[RptWbsPostingDrain_Get]");
    //        dynamicParameters.Add("@intCustomerID", obj.CustomerId);
    //        dynamicParameters.Add("@intContractID", obj.ContractId);
    //        dynamicParameters.Add("@intProjectID", obj.ProjectId);
    //        dynamicParameters.Add("@intStructureElementId", obj.StructureElementTypeId);
    //        dynamicParameters.Add("@sitProductTypeId", obj.ProductType);
    //        dynamicParameters.Add("@intWBSElementsId", obj.strWBSElementID);

    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetBBSPostingDetailsForGrid(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("RptBBSPostingForGrid_Get");
    //        dynamicParameters.Add("@intCustomerID", obj.CustomerId);
    //        dynamicParameters.Add("@intContractID", obj.ContractId);
    //        dynamicParameters.Add("@intProjectID", obj.ProjectId);
    //        dynamicParameters.Add("@intStructureElementId", obj.StructureElementTypeId);
    //        dynamicParameters.Add("@sitProductTypeId", obj.ProductType);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //// Public s Function GetReportBeamPostingList(ByVal obj As AdminInfo) As DataSet
    //// Try
    //// dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("RptBeamPostingList")
    //// dynamicParameters.Add("@intCustomerID", obj.CustomerId)
    //// dynamicParameters.Add("@intContractID", obj.ContractId)
    //// dynamicParameters.Add("@intProjectID", obj.ProjectId)
    //// dynamicParameters.Add("@intWBSElementID", obj.strWBSElementID)
    //// dynamicParameters.Add("@sitProductTypeId", obj.ProductType)
    //// dynamicParameters.Add("@intPostHeaderID", obj.PostHeaderId)
    //// Return DataAccess.DataAccess.GetDataSet(dbcom)
    //// Catch ex As Exception
    //// Throw ex
    //// End Try
    //// End Function

    //public  DataSet GetReportCABProductDetails(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("RptCABProductDetails_List");
    //        dynamicParameters.Add("@intCustomerID", obj.CustomerId);
    //        dynamicParameters.Add("@intContractID", obj.ContractId);
    //        dynamicParameters.Add("@intProjectID", obj.ProjectId);
    //        dynamicParameters.Add("@intGroupMarkId", obj.strGroupMarkId);
    //        dynamicParameters.Add("@sitProductTypeId", obj.ProductType);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetReportWBSPostedBeam(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("RptWbsPostedDetailBeam_Get");
    //        dynamicParameters.Add("@intCustomerID", obj.CustomerId);
    //        dynamicParameters.Add("@intContractID", obj.ContractId);
    //        dynamicParameters.Add("@intProjectID", obj.ProjectId);
    //        dynamicParameters.Add("@intStructureElementId", obj.StructureElementTypeId);
    //        dynamicParameters.Add("@sitProductTypeId", obj.ProductType);
    //        dynamicParameters.Add("@intWBSElementsId", obj.strWBSElementID);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetReportWBSPostedSlab(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("RptWbsPostedDetailSlab_Get");
    //        dynamicParameters.Add("@intCustomerID", obj.CustomerId);
    //        dynamicParameters.Add("@intContractID", obj.ContractId);
    //        dynamicParameters.Add("@intProjectID", obj.ProjectId);
    //        dynamicParameters.Add("@intStructureElementId", obj.StructureElementTypeId);
    //        dynamicParameters.Add("@sitProductTypeId", obj.ProductType);
    //        dynamicParameters.Add("@intWBSElementsId", obj.strWBSElementID);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetReportWBSPostedColumn(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("RptWbsPostedDetailColumn_Get");
    //        dynamicParameters.Add("@intCustomerID", obj.CustomerId);
    //        dynamicParameters.Add("@intContractID", obj.ContractId);
    //        dynamicParameters.Add("@intProjectID", obj.ProjectId);
    //        dynamicParameters.Add("@intStructureElementId", obj.StructureElementTypeId);
    //        dynamicParameters.Add("@sitProductTypeId", obj.ProductType);
    //        dynamicParameters.Add("@intWBSElementsId", obj.strWBSElementID);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetReportWBSPostedPrecage(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("RptWbsPostedDetailPrecage_Get");
    //        dynamicParameters.Add("@intCustomerID", obj.CustomerId);
    //        dynamicParameters.Add("@intContractID", obj.ContractId);
    //        dynamicParameters.Add("@intProjectID", obj.ProjectId);
    //        dynamicParameters.Add("@intStructureElementId", obj.StructureElementTypeId);
    //        dynamicParameters.Add("@sitProductTypeId", obj.ProductType);
    //        dynamicParameters.Add("@IsSummaryReport", obj.SummaryReport);
    //        dynamicParameters.Add("@intWBSElementsId", obj.strWBSElementID);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetReportPrecageProductDetails(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("RptPrecageProductDetails_List");
    //        dynamicParameters.Add("@intCustomerID", obj.CustomerId);
    //        dynamicParameters.Add("@intContractID", obj.ContractId);
    //        dynamicParameters.Add("@intProjectID", obj.ProjectId);
    //        dynamicParameters.Add("@intGroupMarkId", obj.strGroupMarkId);
    //        dynamicParameters.Add("@sitProductTypeId", obj.ProductType);
    //        dynamicParameters.Add("@IsSummaryReport", obj.SummaryReport);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetReportBPCProductDetails(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("RptBPCProductDetails_List");
    //        dynamicParameters.Add("@intCustomerID", obj.CustomerId);
    //        dynamicParameters.Add("@intContractID", obj.ContractId);
    //        dynamicParameters.Add("@intProjectID", obj.ProjectId);
    //        dynamicParameters.Add("@intGroupMarkId", obj.strGroupMarkId);
    //        dynamicParameters.Add("@sitProductTypeId", obj.ProductType);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetReportBPCSummary(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("[RptBPCSummary_List]");
    //        dynamicParameters.Add("@intCustomerID", obj.CustomerId);
    //        dynamicParameters.Add("@intContractID", obj.ContractId);
    //        dynamicParameters.Add("@intProjectID", obj.ProjectId);
    //        dynamicParameters.Add("@intGroupMarkId", obj.strGroupMarkId);
    //        dynamicParameters.Add("@sitProductTypeId", obj.ProductType);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetDestGMRevQuanParam(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("DestGMRevQuanParam");
    //        dynamicParameters.Add("@intProjectID", obj.ProjectId);
    //        dynamicParameters.Add("@intGroupMarkId", obj.GroupMarkingId);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetReportProductDetails(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("RptProductDetails");
    //        dynamicParameters.Add("@intCustomerID", obj.CustomerId);
    //        dynamicParameters.Add("@intContractID", obj.ContractId);
    //        dynamicParameters.Add("@intProjectID", obj.ProjectId);
    //        dynamicParameters.Add("@intGroupMarkId", obj.strGroupMarkId);
    //        dynamicParameters.Add("@sitProductTypeId", obj.ProductType);
    //        dynamicParameters.Add("@IsSummaryReport", obj.SummaryReport);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetReportPostedParentMsh(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("RptPostedDetailMsh_Get");
    //        dynamicParameters.Add("@intCustomerID", obj.CustomerId);
    //        dynamicParameters.Add("@intContractID", obj.ContractId);
    //        dynamicParameters.Add("@intProjectID", obj.ProjectId);
    //        dynamicParameters.Add("@intStructureElementId", obj.StructureElementTypeId);
    //        dynamicParameters.Add("@sitProductTypeId", obj.ProductType);
    //        dynamicParameters.Add("@IsSummaryReport", obj.SummaryReport);
    //        dynamicParameters.Add("@intWBSElementsId", obj.strWBSElementID);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetReportPostedDrain(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("RptWbsPostedDetailDrain_List");
    //        dynamicParameters.Add("@intCustomerID", obj.CustomerId);
    //        dynamicParameters.Add("@intContractID", obj.ContractId);
    //        dynamicParameters.Add("@intProjectID", obj.ProjectId);
    //        dynamicParameters.Add("@intStructureElementId", obj.StructureElementTypeId);
    //        dynamicParameters.Add("@sitProductTypeId", obj.ProductType);
    //        dynamicParameters.Add("@IsSummaryReport", obj.SummaryReport);
    //        dynamicParameters.Add("@intWBSElementsId", obj.strWBSElementID);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetSAPMaterialDetails(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("SapMaterialDetails_Get");
    //        dynamicParameters.Add("@strSAPMaterialCode", obj.SAPMaterialCode);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  int UpdateSAPMaterialDetails(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("SapMaterialDetails_Update");
    //        dynamicParameters.Add("@intMaterialCodeId", obj.MaterialCodeId);
    //        dynamicParameters.Add("@vchStructElement", obj.StructureElement);
    //        dynamicParameters.Add("@bitAcc", DbType.Boolean, obj.Accessories);
    //        dynamicParameters.Add("@bitRM", DbType.Boolean, obj.RM);
    //        dynamicParameters.Add("@bitStock", DbType.Boolean, obj.Stock);
    //        dynamicParameters.Add("@bitFG", DbType.Boolean, obj.FG);
    //        dynamicParameters.Add("@intWeight", obj.SAPWeight);
    //        dynamicParameters.Add("@strLength", obj.SAPLength);
    //        dynamicParameters.Add("@strWidth", obj.SAPWidth);
    //        dynamicParameters.Add("@strHeight", obj.SAPHeight);
    //        DataAccess.DataAccess.db.AddOutParameter("@Output", 0);
    //        DataAccess.DataAccess.GetScalar(dbcom);
    //        intReturnValue = (dbcom.Parameters["@Output"].Value);
    //        return intReturnValue;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetProductTypeForReport(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ProductTypeForReport");
    //        dynamicParameters.Add("@vchStructureElementType", obj.ElementType);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //// tree caching
    //public  DataSet GetCustomerTree()
    //{
    //    try
    //    {
    //        SqlCacheDependency SqlDep;
    //        DataSet dsCache = (DataSet)HttpRuntime.Cache["CacheCustomerTreeNodes"];
    //        if (dsCache == null)
    //        {
    //            SqlDep = new SqlCacheDependency("NDSCaching", "customermaster");
    //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("Customer_Get");
    //            dsCache = DataAccess.DataAccess.GetDataSet(dbcom);
    //            HttpRuntime.Cache.Insert("CacheCustomerTreeNodes", dsCache, SqlDep);
    //        }

    //        return dsCache;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetContractTree()
    //{
    //    try
    //    {
    //        SqlCacheDependency SqlDep;
    //        DataSet dsCache = (DataSet)HttpRuntime.Cache["CacheContractTree"];
    //        if (dsCache == null)
    //        {
    //            SqlDep = new SqlCacheDependency("NDSCaching", "contractmaster");
    //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("CustomerTreeContract_Cache_Get");
    //            dsCache = DataAccess.DataAccess.GetDataSet(dbcom);
    //            HttpRuntime.Cache.Insert("CacheContractTree", dsCache, SqlDep);
    //        }

    //        return dsCache;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetProjectTree()
    //{
    //    try
    //    {
    //        SqlCacheDependency SqlDep;
    //        DataSet dsCache = (DataSet)HttpRuntime.Cache["CacheProjectTree"];
    //        if (dsCache == null)
    //        {
    //            SqlDep = new SqlCacheDependency("NDSCaching", "projectmaster");
    //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("CustomerTreeProject_Cache_Get_new"); // modified for ship to party
    //            dsCache = DataAccess.DataAccess.GetDataSet(dbcom);
    //            HttpRuntime.Cache.Insert("CacheProjectTree", dsCache, SqlDep);
    //        }

    //        return dsCache;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetCustomerStartLetters()
    //{
    //    try
    //    {
    //        SqlCacheDependency SqlDep;
    //        DataSet dsCache = (DataSet)HttpRuntime.Cache["CacheCustomerTree"];
    //        if (dsCache == null)
    //        {
    //            SqlDep = new SqlCacheDependency("NDSCaching", "customermaster");
    //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("CustomerTree_Cache_Get");
    //            dsCache = DataAccess.DataAccess.GetDataSet(dbcom);
    //            HttpRuntime.Cache.Insert("CacheCustomerTree", dsCache, SqlDep);
    //        }

    //        return dsCache;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}
    //public  int InsertShapeDetails(AdminInfo obj)
    //{
    //    try
    //    {
    //        Int32 intReturnValue;
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ShapeCodeDetails_Insert");
    //        dynamicParameters.Add("@intShapeTransHeaderId", obj.ShapeTransHeaderId);
    //        dynamicParameters.Add("@vchSequence", obj.Sequence);
    //        dynamicParameters.Add("@vchParamValues", obj.ParamValues);
    //        dynamicParameters.Add("@vchCriticalIndicator", obj.CriticalIndicator);
    //        DataAccess.DataAccess.db.AddOutParameter("@intOutput", 0);
    //        DataAccess.DataAccess.ExecuteNonQuery(dbcom);
    //        intReturnValue = (dbcom.Parameters["@intOutput"].Value);
    //        return intReturnValue;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}


    //public  DataSet GetProjectTree1()
    //{
    //    try
    //    {
    //        SqlCacheDependency SqlDep;
    //        DataSet dsCache = (DataSet)HttpRuntime.Cache["CacheCustomerTreeNodes"];
    //        if (dsCache == null)
    //        {
    //            SqlDep = new SqlCacheDependency("NDSCaching", "customermaster");
    //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("Customer_Get");
    //            dsCache = DataAccess.DataAccess.GetDataSet(dbcom);
    //            HttpRuntime.Cache.Insert("CacheCustomerTreeNodes", dsCache, SqlDep);
    //        }

    //        return dsCache;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    #region Drain Module

    public int InsertProjectParamDrainLap(InsertProjectParamDrainLapDto obj,out string errorMessage)
    {
        int intReturnValue = 0;
        errorMessage = "";
        try
        {
            
            using (var sqlConnection = new SqlConnection(connectionString))
            {

                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@intDrainLapId",obj.intDrainLapId);
                dynamicParameters.Add("@bitConfirm", obj.bitConfirm);
                dynamicParameters.Add("@tntParamSetNumber", obj.tntParamSetNumber);
                dynamicParameters.Add("@intProductCodeId", obj.intProductCodeId);
                dynamicParameters.Add("@sitLap", obj.sitLap);
                dynamicParameters.Add("@intUserId", obj.intUserId);
                dynamicParameters.Add("@intOutput", null, dbType: DbType.Int32, ParameterDirection.Output);

                sqlConnection.Query<int>(SystemConstants.ProjectParamDrainLap_Insert, dynamicParameters, commandType: CommandType.StoredProcedure);

                intReturnValue = dynamicParameters.Get<int>("@intOutput");
                sqlConnection.Close();

                return intReturnValue;
            }


        }
        catch (Exception ex)
        {
           errorMessage = ex.Message;
        }
        return intReturnValue;
    }

    public List<GetProjectParamDrainLapDto>GetProjectParamDrainLap(AdminInfo obj)
    {
        IEnumerable<GetProjectParamDrainLapDto>getProjectParamDrainLaps;

        try
        {
            DataSet dsProjectParamDrain = new DataSet();
            using (var sqlConnection = new SqlConnection(connectionString))
            {

                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@tntParamSetNumber",obj.ParameterSetNo);

                getProjectParamDrainLaps =sqlConnection.Query<GetProjectParamDrainLapDto>(SystemConstants.ProjectParameterDrainLap_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return getProjectParamDrainLaps.ToList();

            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public int DeleteProductParamDrainLap(AdminInfo obj)
    {
        try
        {
            int intReturnValue;

            using (var sqlConnection = new SqlConnection(connectionString))
            {

                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@intDrainLapId", obj.DrainLapId);
                dynamicParameters.Add("@bitConfirm", obj.Confirm);

                dynamicParameters.Add("@intOutput", null, dbType: DbType.Int32, ParameterDirection.Output);

                sqlConnection.Query<int>(SystemConstants.ProjectParamDrainLap_Delete, dynamicParameters, commandType: CommandType.StoredProcedure);

                intReturnValue = dynamicParameters.Get<int>("@intOutput");
                sqlConnection.Close();
                return intReturnValue;

            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<IEnumerable<getDrainProductCodeDto>> GetDrainProductCode()
    {
        try
        {
            DataSet dsProductCode = new DataSet();
            IEnumerable<getDrainProductCodeDto> getDrainProductCodes;
            using (var sqlConnection = new SqlConnection(connectionString))
            {

                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();

                getDrainProductCodes = sqlConnection.Query<getDrainProductCodeDto>(SystemConstants.ProductCodeDrain_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return getDrainProductCodes;

            }



        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public int InsertProjectParamDrainOH(AdminInfo obj)
    {
        try
        {
            Int32 intReturnValue;
            using (var sqlConnection = new SqlConnection(connectionString))
            {

                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();


                dynamicParameters.Add("@intParamDrainOHId", obj.ParamOHId);
                dynamicParameters.Add("@tntParamSetNumber", obj.ParameterSetNo);
                dynamicParameters.Add("@intMWSpace", obj.MWSpace);
                dynamicParameters.Add("@intCWSpace", obj.CWSpace);
                dynamicParameters.Add("@intEvenMO1", obj.EvenMO1);
                dynamicParameters.Add("@intEvenMO2", obj.EvenMO2);
                dynamicParameters.Add("@intEvenCO1", obj.EvenCO1);
                dynamicParameters.Add("@intEvenCO2", obj.EvenCO2);
                dynamicParameters.Add("@intOddMO1", obj.OddMO1);
                dynamicParameters.Add("@intOddMO2", obj.OddMO2);
                dynamicParameters.Add("@intOddCO1", obj.OddCO1);
                dynamicParameters.Add("@intOddCO2", obj.OddCO2);
                dynamicParameters.Add("@intUserId", obj.UserId);
                dynamicParameters.Add("@intOutput", null, dbType: DbType.Int32, ParameterDirection.Output);

                intReturnValue = sqlConnection.QueryFirstOrDefault<int>(SystemConstants.ProjectParamDrainOH_Insert, dynamicParameters, commandType: CommandType.StoredProcedure);
                return intReturnValue;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DataSet GetProjectParamDrainOH(AdminInfo obj)
    {
        try
        {
            DataSet dsProjectParamDrain = new DataSet();
            using (var sqlConnection = new SqlConnection(connectionString))
            {

                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@tntParamSetNumber", obj.ParameterSetNo);
                dsProjectParamDrain = (DataSet)sqlConnection.Query<DataSet>(SystemConstants.ProjectParameterDrainOH_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

                return dsProjectParamDrain;

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public int DeleteProductParamDrainOH(AdminInfo obj)
    {
        try
        {
            Int32 intReturnValue;
            using (var sqlConnection = new SqlConnection(connectionString))
            {

                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@intParamDrainOHId", obj.ParamOHId);
                dynamicParameters.Add("@intOutput", null, dbType: DbType.Int32, ParameterDirection.Output);

                sqlConnection.Query<int>(SystemConstants.ProjectParamDrainOH_Delete, dynamicParameters, commandType: CommandType.StoredProcedure);

                intReturnValue = dynamicParameters.Get<int>("@intOutput");
                sqlConnection.Close();

                return intReturnValue;
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public List<GetProjectParamDrainDepthDto>GetProjectParamDrainDepth(AdminInfo obj)
    {
        DataSet dsProjectParamDrainDepth = new DataSet();
        IEnumerable<GetProjectParamDrainDepthDto> getProjectParamDrainDepths;
        try
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {

                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();

                dynamicParameters.Add("@tntParamSetNumber",obj.ParameterSetNo);
                getProjectParamDrainDepths =sqlConnection.Query<GetProjectParamDrainDepthDto>(SystemConstants.ProjectParameterDrainDepth_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();

                return getProjectParamDrainDepths.ToList();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public List<getDrainProductCodeDto>GetLapProductCodeWM(AdminInfo obj)
    {
        IEnumerable<getDrainProductCodeDto> getDrainProductCode;
        DataSet dsLapProductCodeWM = new DataSet();
        try
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {

                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();

                dynamicParameters.Add("@tntParamSetNumber",obj.ParameterSetNo);
                getDrainProductCode =sqlConnection.Query<getDrainProductCodeDto>(SystemConstants.ProductCodeDrainLap_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();

                return getDrainProductCode.ToList();
            }



        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public int InsertProjectParamDrainDepth(GetProjectParamDrainDepthDto obj,out string errorMessage)
    {
        int intReturnValue=0;
        errorMessage = "";
        try
        {
           
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@intDrainDepthParamId", obj.intDrainDepthParamId);
                dynamicParameters.Add("@tntParamSetNumber", obj.tntParamSetNumber);
                dynamicParameters.Add("@sitDrainTypeId", obj.sitDrainTypeId);
                dynamicParameters.Add("@sitDrainWidth", obj.sitDrainWidth);
                dynamicParameters.Add("@sitAdjust", obj.sitAdjust);
                dynamicParameters.Add("@sitChannel", obj.sitChannel);
                dynamicParameters.Add("@sitSlabThickness", obj.sitSlabThickness);
                dynamicParameters.Add("@sitMaxDepth1", obj.sitMaxDepth1);
                dynamicParameters.Add("@sitMaxDepth2", obj.sitMaxDepth2);
                dynamicParameters.Add("@sitMaxDepth3", obj.sitMaxDepth3);
                dynamicParameters.Add("@sitMaxDepth4", obj.sitMaxDepth4);
                dynamicParameters.Add("@sitMaxDepth5", obj.sitMaxDepth5);
                dynamicParameters.Add("@bitConfirm", obj.bitConfirm);
                dynamicParameters.Add("@intUserId", 1);// obj.intUserId);
                dynamicParameters.Add("@Output",0);

                intReturnValue = sqlConnection.QueryFirstOrDefault<int>(SystemConstants.ProjectParamDrainDepth_Insert, dynamicParameters, commandType: CommandType.StoredProcedure);

                return intReturnValue;


            }

        }
        catch (Exception ex)
        {
            errorMessage = ex.Message;
        }
        return intReturnValue;
    }

    public int DeleteProductParamDrainDepth(AdminInfo obj)
    {
        try
        {
            int intReturnValue;
            using (var sqlConnection = new SqlConnection(connectionString))
            {

                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@intDrainDepthParamId", obj.DepthId);
                dynamicParameters.Add("@bitConfirm", obj.Confirm);
                dynamicParameters.Add("@intOutput", null, dbType: DbType.Int32, ParameterDirection.Output);

                sqlConnection.Query<int>(SystemConstants.ProjectParamDrainDepth_Delete, dynamicParameters, commandType: CommandType.StoredProcedure);

                intReturnValue = dynamicParameters.Get<int>("@intOutput");
                sqlConnection.Close();

                return intReturnValue;
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public List<GetDrainProductTypeDto>GetDrainProductType(AdminInfo obj)
    {
        try
        {
            IEnumerable<GetDrainProductTypeDto> getDrainProductTypes;
            DataSet dsGetDrainProductType = new DataSet();
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();

                dynamicParameters.Add("@tntParamSetNumber",obj.ParameterSetNo);
                getDrainProductTypes =sqlConnection.Query<GetDrainProductTypeDto>(SystemConstants.ProjectParameterDrainTypeParamNoGiven_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();

                return getDrainProductTypes.ToList();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public List<GetDrainProductTypeDto>GetDrainProductType()
    {
        try
        {
            IEnumerable<GetDrainProductTypeDto> getDrainProductTypes;
            DataSet dGetDrainProductType = new DataSet();
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();

                getDrainProductTypes = sqlConnection.Query<GetDrainProductTypeDto>(SystemConstants.ProjectParameterDrainType_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();

                return getDrainProductTypes.ToList();
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<IEnumerable<GetDrainWidthWMDto>> GetDrainWidthWM(int tntParamSetNumber)
    {
        try
        {
            IEnumerable<GetDrainWidthWMDto> getDrainProducts;
            DataSet dsGetDrainWidthWM = new DataSet();
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();

                dynamicParameters.Add("@tntParamSetNumber",tntParamSetNumber);
                getDrainProducts =sqlConnection.Query<GetDrainWidthWMDto>(SystemConstants.ProjectParameterDrainWidthParamNoGiven_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();

                return getDrainProducts;
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public List<GetProjectParamDrainWMDto>GetProjectParamDrainWM(AdminInfo obj)
    {
        try
        {
            IEnumerable<GetProjectParamDrainWMDto> adminInfos;
            DataSet dGetProjectParamDrainWM = new DataSet();
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();

                dynamicParameters.Add("@tntParamSetNumber",obj.ParameterSetNo);
                adminInfos = sqlConnection.Query<GetProjectParamDrainWMDto>(SystemConstants.ProjectParameterDrainWM_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();

                return adminInfos.ToList();
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public int InsertProjectParamDrainWM(InsertProjectParamDrainWMDto obj,out string errorMessage)
    {
        int intReturnValue=0;
        errorMessage = "";
        try
        {
            
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@intDrainWMId", obj.intDrainWMId);
                dynamicParameters.Add("@tntParamSetNumber", obj.tntParamSetNumber);
                dynamicParameters.Add("@sitDrainTypeId", obj.sitDrainTypeId);
                dynamicParameters.Add("@tntDrainLayerId", obj.tntDrainLayerId);
                dynamicParameters.Add("@sitDrainWidth", obj.sitDrainWidth);
                dynamicParameters.Add("@intDrainDepthParamId", obj.intDrainDepthParamId);
                dynamicParameters.Add("@sitMaxDepth", obj.sitMaxDepth);
                dynamicParameters.Add("@intProductCodeId", obj.intProductCodeId);
                dynamicParameters.Add("@intShapeId", obj.intShapeId);
                dynamicParameters.Add("@intParamA", obj.intParamA);
                dynamicParameters.Add("@numLeftWallThickness", obj.numLeftWallThickness.ToString());
                dynamicParameters.Add("@numLeftWallFactor", obj.numLeftWallFactor.ToString());
                dynamicParameters.Add("@numRightWallThickness", obj.numRightWallThickness.ToString());
                dynamicParameters.Add("@numRightWallFactor", obj.numRightWallFactor.ToString());
                dynamicParameters.Add("@numBaseThickness", obj.numBaseThickness.ToString());
                dynamicParameters.Add("@intQty", obj.intQty);
                dynamicParameters.Add("@bitDetail", obj.bitDetail);
                dynamicParameters.Add("@intUserId", obj.intUserId);
                dynamicParameters.Add("@intOutput", 1);
                dynamicParameters.Add("@intScopeIdentity", null, dbType: DbType.Int32, ParameterDirection.Output);

                sqlConnection.Query<int>(SystemConstants.ProjectParamDrainWM_Insert, dynamicParameters, commandType: CommandType.StoredProcedure);

                //intReturnValue = dynamicParameters.Get<int>("@intOutput");
                intReturnValue= dynamicParameters.Get<int>("@intScopeIdentity");
                sqlConnection.Close();

                return intReturnValue;
            }

        }
        catch (Exception ex)
        {
            errorMessage= ex.Message;
        }
        return intReturnValue;
    }

    public int DeleteProductParamDrainWM(AdminInfo obj)
    {
        try
        {
            Int32 intReturnValue;

            using (var sqlConnection = new SqlConnection(connectionString))
            {

                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@intDrainWMId", obj.WMId);
                dynamicParameters.Add("@intOutput", null, dbType: DbType.Int32, ParameterDirection.Output);

                sqlConnection.Query<int>(SystemConstants.ProjectParamDrainWM_Delete, dynamicParameters, commandType: CommandType.StoredProcedure);

                intReturnValue = dynamicParameters.Get<int>("@intOutput");
                sqlConnection.Close();

                return intReturnValue;
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public List<GetProjectParamDrainLayerDto> GetProjectParamDrainLayer()
    {
        try
        {
            IEnumerable<GetProjectParamDrainLayerDto> drainLayerDtos;
            DataSet dsGetProjectParamDrainLayer = new DataSet();
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                drainLayerDtos =sqlConnection.Query<GetProjectParamDrainLayerDto>(SystemConstants.ProjectParameterDrainLayer_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();

                return drainLayerDtos.ToList();
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public List<GetProjectParamDrainLayerDto>GetProjectParamDrainMaxDepth(AdminInfo obj)
    {
        try
        {
            IEnumerable<GetProjectParamDrainLayerDto> drainLayerDtos;
            DataSet dsProjectParamDrainMaxDepth = new DataSet();
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@intDrainDepthParamId",obj.DepthId);

                drainLayerDtos = sqlConnection.Query<GetProjectParamDrainLayerDto>(SystemConstants.ProjectParameterDrainMaxDepths_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();

                return drainLayerDtos.ToList();
            }


        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public List<GetProjectParamDrainParamDto>GetProjectParamDrainParamDetails(AdminInfo obj)
    {
        try
        {
            IEnumerable<GetProjectParamDrainParamDto> getProjectParamDrainParams;
            DataSet dsGetProjectParamDrainParam = new DataSet();
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@intDrainWMId", obj.WMId);

                getProjectParamDrainParams =sqlConnection.Query<GetProjectParamDrainParamDto>(SystemConstants.ProjectParameterDrainParamDetails_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();

                return getProjectParamDrainParams.ToList();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public List<ProjectParamDrainShapeDto>GetProjectParamDrainShapeforLayer(AdminInfo obj)
    {
        try
        {
            IEnumerable<ProjectParamDrainShapeDto> paramDrainShapeDtos;
            DataSet dsProjectParamDrainShapeforLayer = new DataSet();
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@tntDrainLayerId", obj.LayerId);

                paramDrainShapeDtos = sqlConnection.Query<ProjectParamDrainShapeDto>(SystemConstants.ProjectParameterDrainShape_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();

                return paramDrainShapeDtos.ToList();
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public List<DrainShapeCodeDto> GetDrainShapeCode(AdminInfo obj)
    {
        try
        {
            IEnumerable<DrainShapeCodeDto> drainShapeCodes;
            DataSet dsGetDrainShapeCode = new DataSet();
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@intDrainWMId", obj.WMId);
                dynamicParameters.Add("@intShapeId", obj.ShapeId);

                drainShapeCodes =sqlConnection.Query<DrainShapeCodeDto>(SystemConstants.DrainShapeCode_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();

                return drainShapeCodes.ToList();
            }


        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public int InsertProjectDrainParamDetails(InsertProjectDrainParamDetailsDto insertProjectDrainParam,out string erroeMessage)
    {
        Int32 intReturnValue=0;
        erroeMessage = "";

        try
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@intDrainWMId", insertProjectDrainParam.intDrainWMId);
                dynamicParameters.Add("@vchSequence", insertProjectDrainParam.vchSequence);
                dynamicParameters.Add("@nvchParamValues", insertProjectDrainParam.nvchParamValues);
                dynamicParameters.Add("@intMo1", insertProjectDrainParam.intMo1);
                dynamicParameters.Add("@intMo2", insertProjectDrainParam.intMo2);
                dynamicParameters.Add("@vchCriticalIndicator", insertProjectDrainParam.vchCriticalIndicator);
                dynamicParameters.Add("@Output", null, dbType: DbType.Int32, ParameterDirection.Output);
                
                sqlConnection.QueryFirstOrDefault<int>(SystemConstants.DrainParameterSetDetails_Insert, dynamicParameters, commandType: CommandType.StoredProcedure);
                intReturnValue = dynamicParameters.Get<int>("@Output");
               
                return intReturnValue;

            }
        }
        catch (Exception ex)
        {
           erroeMessage=ex.Message;
        }
        return intReturnValue;
    }

    public List<DrainProductCodeDetailsDto>GetDrainParameterDetails(AdminInfo obj)
    {
        try
        {
            IEnumerable<DrainProductCodeDetailsDto> drainProductCodeDetails;
            DataSet dsGetDrainParamDetails = new DataSet();
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@intProductCodeId", obj.ProductCodeId);

                drainProductCodeDetails=sqlConnection.Query<DrainProductCodeDetailsDto>(SystemConstants.DrainProductCodeDetails_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();

                return drainProductCodeDetails.ToList();
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public List<GetDrainWidthWMDto>GetDrainDepthWidth(AdminInfo obj)
    {
        try
        {
            IEnumerable<GetDrainWidthWMDto> getDrainWidths;
            DataSet dGetDrainDepthWidth = new DataSet();
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@tntParamSetNumber", obj.ParameterSetNo);
                dynamicParameters.Add("@sitDrainTypeId", obj.Type);

                getDrainWidths =sqlConnection.Query<GetDrainWidthWMDto>(SystemConstants.ProjectParameterDrainWidths_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();

                return getDrainWidths.ToList();
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public List<DrainWidthDepthDto> GetDrainWidthDepth(AdminInfo obj)
    {
        try
        {
            IEnumerable<DrainWidthDepthDto> drainWidthDepthDtos;
            DataSet dsGetDrainWidthDepth = new DataSet();
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@tntParamSetNumber", obj.ParameterSetNo);
                dynamicParameters.Add("@sitDrainWidth", obj.DrainDepthWidth);

                drainWidthDepthDtos = sqlConnection.Query<DrainWidthDepthDto>(SystemConstants.ProjectParameterDrainDepths_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();

                return drainWidthDepthDtos.ToList();
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public List<GetShapeParamDetailsDto>GetShapeParamDetails(AdminInfo obj)
    {
        try
        {
            DataSet dsshapeParamDetails = new DataSet();
            IEnumerable<GetShapeParamDetailsDto> shapeParamDetailsDtos;
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@intShapeId", obj.ShapeId);

                shapeParamDetailsDtos =sqlConnection.Query<GetShapeParamDetailsDto>(SystemConstants.ShapeParam_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();

                return shapeParamDetailsDtos.ToList();
            }


        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public int InsUpdShapeParamDetails(AdminInfo obj)
    {
        try
        {
            int intReturnValue = 0;
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                var dynamicParameters = new DynamicParameters();
                sqlConnection.Open();
                dynamicParameters.Add("@intShapeId", obj.ShapeId);
                dynamicParameters.Add("@intShapeDetailId", obj.ShapeDetailsId);
                dynamicParameters.Add("@chrParamName", obj.ParamName);
                dynamicParameters.Add("@intParamSeq", obj.ParamSeq);
                dynamicParameters.Add("@vchMWShape", obj.MWShape);
                dynamicParameters.Add("@vchCWShape", obj.CWShape);
                dynamicParameters.Add("@chrWireType", obj.WireType);
                dynamicParameters.Add("@chrAngleType", obj.AngleType);
                dynamicParameters.Add("@intAngleDir", obj.AngleDir);
                dynamicParameters.Add("@intBendSeq1", obj.BendSeq1);
                dynamicParameters.Add("@intBendSeq2", obj.BendSeq2);
                dynamicParameters.Add("@chrCriticalInd", obj.CriticalIndicator);
                dynamicParameters.Add("@intMinLen", obj.MinLen);
                dynamicParameters.Add("@intMaxLen", obj.MaxLen);
                dynamicParameters.Add("@intConstValue", obj.ConstValue);
                dynamicParameters.Add("@intUserId", obj.UserId);
                dynamicParameters.Add("@Output", 0);

                intReturnValue = sqlConnection.QueryFirstOrDefault<int>(SystemConstants.ShapeParam_InsUpd, dynamicParameters, commandType: CommandType.StoredProcedure);

                return intReturnValue;

            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public int DeleteShapeParamDetails(AdminInfo obj)
    {
        try
        {
            int intReturnValue = 0;
            using (var sqlConnection = new SqlConnection(connectionString))
            {

                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@intShapeDetailId ", obj.ShapeDetailsId); dynamicParameters.Add("@intOutput", 0);
                dynamicParameters.Add("@Output", 0);
                sqlConnection.Query<int>(SystemConstants.ShapeParam_delete, dynamicParameters, commandType: CommandType.StoredProcedure);

                intReturnValue = dynamicParameters.Get<int>("@Output");
                sqlConnection.Close();

                return intReturnValue;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public int saveStructureDimension(AdminInfo obj)
    {
        try
        {
            Int32 intReturnValue = 0;
            using (var sqlConnection = new SqlConnection(connectionString))
            {

                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@intDimensionId", obj.DimensionId);
                dynamicParameters.Add("@intSETypeId", obj.StructureElementTypeId);
                dynamicParameters.Add("@vchDimension", obj.DimensionDesc);
                dynamicParameters.Add("@vchDesc", obj.Description);
                dynamicParameters.Add("@vchMinLength", obj.MinLength);
                dynamicParameters.Add("@vchMaxLength", obj.MaxLength);
                dynamicParameters.Add("@intStatusId", obj.StatusId);
                dynamicParameters.Add("@intUserId", obj.UserId);
                dynamicParameters.Add("@intOutput", 0);
                sqlConnection.Query<int>(SystemConstants.SaveStructureDimension, dynamicParameters, commandType: CommandType.StoredProcedure);

                intReturnValue = dynamicParameters.Get<int>("@intOutput");
                sqlConnection.Close();

                return intReturnValue;
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DataSet GetShapeCodeDetails(AdminInfo obj)
    {
        try
        {
            DataSet dsShapeCodeDetails = new DataSet();
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@intShapeCode", obj.ShapeId);
                dsShapeCodeDetails = (DataSet)sqlConnection.Query<DataSet>(SystemConstants.ShapeCodeDetails_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();

                return dsShapeCodeDetails;
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public int InsUpdShapeHeaderDetails(AdminInfo obj)
    {
        try
        {
            int intReturnValue = 0;
            using (var sqlConnection = new SqlConnection(connectionString))
            {

                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@intShapeId", obj.ShapeId);
                dynamicParameters.Add("@chrShapeCode", obj.ShapeDescription);
                dynamicParameters.Add("@vchMeshShapeGroup", obj.MeshGroup);
                dynamicParameters.Add("@chrMOCO", obj.MOCO);
                dynamicParameters.Add("@sitNoOfBends", obj.NoOfBends);
                dynamicParameters.Add("@vchBendingGroup", obj.BendingGroup);
                dynamicParameters.Add("@vchMWBendingGroup", obj.MWBendingGroup);
                dynamicParameters.Add("@vchCWBendingGroup", obj.CWBendingGroup);
                dynamicParameters.Add("@intNoOfSegments", obj.NoOfSegments);
                dynamicParameters.Add("@intNoOfParameters", obj.NoOfParameters);

                dynamicParameters.Add("@sitNoOfCut", obj.NoOfCuts);
                dynamicParameters.Add("@vchImage", obj.Image);
                dynamicParameters.Add("@vchImagePath", obj.ImagePath);
                dynamicParameters.Add("@sitNoOfRoll", obj.NoOfRoll);
                dynamicParameters.Add("@chrShapeType", obj.ShapeType);
                // dynamicParameters.Add("@intMWShapeId", obj.MWShapeId)
                // dynamicParameters.Add("@intCWShapeId", obj.CWShapeId)
                dynamicParameters.Add("@bitBendIndicator", obj.BendIndicator);
                // dynamicParameters.Add("@chrBendPosition", obj.BendPosition)
                // dynamicParameters.Add("@chrCheckLength", obj.CheckLength)
                // dynamicParameters.Add("@bitBendSeqIndicator", DbType.Boolean, obj.BendSeqInd)
                dynamicParameters.Add("@bitBendType", obj.BendType);
                dynamicParameters.Add("@bitCreepDeductAtMo1", obj.CreepMO1);
                dynamicParameters.Add("@bitCreepDeductAtCo1", obj.CreepCO1);
                // dynamicParameters.Add("@bitVerifyIndicator", DbType.Boolean, obj.VerifyInd)
                dynamicParameters.Add("@tntStatusId", obj.StatusId);
                dynamicParameters.Add("@vchCwBVBSTemplate", obj.CWTemplate);
                dynamicParameters.Add("@vchMwBVBSTemplate", obj.MWTemplate);
                dynamicParameters.Add("@sitEvenMO1", obj.EvenMO1);
                dynamicParameters.Add("@sitEvenMO2", obj.EvenMO2);
                dynamicParameters.Add("@sitOddMO1", obj.OddMO1);
                dynamicParameters.Add("@sitOddMO2", obj.OddMO2);
                dynamicParameters.Add("@sitEvenCO1", obj.EvenCO1);
                dynamicParameters.Add("@sitEvenCO2", obj.EvenCO2);
                dynamicParameters.Add("@sitOddCO1", obj.OddCO1);
                dynamicParameters.Add("@sitOddCO2", obj.OddCO2);
                dynamicParameters.Add("@bitOHIndicator", obj.OHIndicator);
                dynamicParameters.Add("@intUID", obj.UserId);
                dynamicParameters.Add("@Output", 0);
                sqlConnection.Query<int>(SystemConstants.ShapeDetails_InsUpd, dynamicParameters, commandType: CommandType.StoredProcedure);

                intReturnValue = dynamicParameters.Get<int>("@Output");
                sqlConnection.Close();

                return intReturnValue;
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DataSet GetCarrierWireMaster(AdminInfo obj)
    {
        try
        {
            DataSet dsGetCarrierWireMaster = new DataSet();
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@intProductTypeL2Id", obj.PrdTypeL2Id);
                dsGetCarrierWireMaster = (DataSet)sqlConnection.Query<DataSet>(SystemConstants.CarrierWireMaster_select, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();

                return dsGetCarrierWireMaster;
            }


        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public int saveCarrierWireMaster(AdminInfo obj)
    {
        try
        {
            Int32 intReturnValue;
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@intCarrierWireId", obj.CarrierWireId);
                dynamicParameters.Add("@sitProductTypeL2Id", obj.PrdTypeL2Id);
                dynamicParameters.Add("@numMinLength", obj.MWMinlen);
                dynamicParameters.Add("@numMaxLength", obj.MWMaxLen);
                dynamicParameters.Add("@tntCAWSequence", obj.CAWSequence);
                dynamicParameters.Add("@intCAWSpace", obj.intCAWSpace);
                dynamicParameters.Add("@intStatusId", obj.StatusId);
                dynamicParameters.Add("@intUserId", obj.UserId);
                dynamicParameters.Add("@intOutput", 0);
                //DataAccess.DataAccess.ExecuteNonQuery(dbcom);
                intReturnValue = sqlConnection.QueryFirstOrDefault<int>(SystemConstants.CarrierWireMaster_InsUpd, dynamicParameters, commandType: CommandType.StoredProcedure);

                return intReturnValue;

            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public int DeleteWireSpecDetails(AdminInfo obj)
    {
        try
        {
            Int32 intReturnValue = 0;

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@intCarrierWireId", obj.CarrierWireId);
                dynamicParameters.Add("@sitProductTypeL2Id", obj.PrdTypeL2Id);
                dynamicParameters.Add("@intOutput", 0);
                //DataAccess.DataAccess.ExecuteNonQuery(dbcom);
                intReturnValue = sqlConnection.QueryFirstOrDefault<int>(SystemConstants.CarrierWireMaster_Del, dynamicParameters, commandType: CommandType.StoredProcedure);

                return intReturnValue;

            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    // Preferred WireLength Master

    public DataSet GetPreferredWireLength(AdminInfo obj)
    {
        try
        {
            DataSet dsGetPreferredWireLength = new DataSet();
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@intProductCodeId", obj.ProductCodeId);
                // dynamicParameters.Add("@intProductCodeId", 0)
                dynamicParameters.Add("@intProjectTypeId", obj.ProjectTypeId);
                dsGetPreferredWireLength = (DataSet)sqlConnection.Query<DataSet>(SystemConstants.PrefWireLen_get, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();

                return dsGetPreferredWireLength;
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    //// Public s Function GetDimensionDetails() As DataSet
    //// Try
    //// dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("StructureDimensionMaster_select")
    //// dynamicParameters.Add("@intProductTypeL2Id", obj.ParameterSetNo)
    //// Return DataAccess.DataAccess.GetDataSet(dbcom)
    //// Catch ex As Exception
    //// Throw ex
    //// End Try
    //// End Function

    public int InsUpdPreferredWireLength(AdminInfo obj)
    {
        try
        {
            int intReturnValue=0;
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@intPrefMeshId", obj.PrefMeshId);
                dynamicParameters.Add("@intProductCodeId", obj.ProductCodeId);
                dynamicParameters.Add("@intProjectTypeId", obj.ProjectTypeId);
                dynamicParameters.Add("@decMWMinLength", obj.MWMinlen);
                dynamicParameters.Add("@decMWMaxLength", obj.MWMaxLen);
                dynamicParameters.Add("@intMWSpace", obj.MWSpace);
                dynamicParameters.Add("@tntMWStep", obj.MWStep);
                dynamicParameters.Add("@intMWExcessLap", obj.MWLap);
                dynamicParameters.Add("@decCWMinLength", obj.CWMinLen);
                dynamicParameters.Add("@decCWMaxLength", obj.CWMaxLen);
                dynamicParameters.Add("@intCWSpace", obj.CWSpace);
                dynamicParameters.Add("@tntCWStep", obj.CWStep);
                dynamicParameters.Add("@intCWExcessLap", obj.CWLap);
                dynamicParameters.Add("@intUserId", obj.UserId);
                dynamicParameters.Add("@intOutput", 0);
                //DataAccess.DataAccess.ExecuteNonQuery(dbcom);
                intReturnValue = sqlConnection.QueryFirstOrDefault<int>(SystemConstants.PrefWireLen_InsUpd, dynamicParameters, commandType: CommandType.StoredProcedure);

                return intReturnValue;

            }


        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public int DeletePreferredWireLength(AdminInfo obj)
    {
        try
        {
            int intReturnValue;
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@intPrefMeshId", obj.PrefMeshId);
                dynamicParameters.Add("@intProductCodeId", obj.ProductCodeId);
                dynamicParameters.Add("@intProjectTypeId", obj.ProjectTypeId);
                dynamicParameters.Add("@intOutput", 0);
                //DataAccess.DataAccess.ExecuteNonQuery(dbcom);
                intReturnValue = sqlConnection.QueryFirstOrDefault<int>(SystemConstants.PrefWireLen_Del, dynamicParameters, commandType: CommandType.StoredProcedure);

                return intReturnValue;

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public async Task<IEnumerable<Get_ParameterSetDropdown>> GetParameterSetAsync(int projectId)
    {
        IEnumerable<Get_ParameterSetDropdown> parameterSetDropdowns;

        using (var sqlConnection = new SqlConnection(connectionString))
        {
            sqlConnection.Open();
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@INTPROJECTID", projectId);
            parameterSetDropdowns = sqlConnection.Query<Get_ParameterSetDropdown>(SystemConstants.GetParameterList_Drain, dynamicParameters, commandType: CommandType.StoredProcedure);
            sqlConnection.Close();
            return parameterSetDropdowns;

        }

    }

    //// [Rani] Product Type Master
    //public  int saveWireSpecDetails(AdminInfo obj)
    //{
    //    try
    //    {
    //        Int32 intReturnValue;

    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("SaveWireSpec");
    //        dynamicParameters.Add("@intWireSpecId", obj.WireSpecId);
    //        dynamicParameters.Add("@nvchWireType", obj.WireType);
    //        dynamicParameters.Add("@nvchWireSpec", obj.WireSpec);
    //        dynamicParameters.Add("@nvchWireSpecDesc", obj.WireSpecDesc);
    //        dynamicParameters.Add("@intStatusId", obj.StatusId);
    //        dynamicParameters.Add("@intUserId", obj.UserId);
    //        DataAccess.DataAccess.db.AddOutParameter("@intOutput", 0);
    //        DataAccess.DataAccess.GetScalar(dbcom);
    //        intReturnValue = (dbcom.Parameters["@intOutput"].Value);
    //        return intReturnValue;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetWireSpecDetails()
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("WireSpec_select");
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}
    //// Public s Function GetDimensionDetails() As DataSet
    //// Try
    //// dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("Dimension_select")
    //// Return DataAccess.DataAccess.GetDataSet(dbcom)
    //// Catch ex As Exception
    //// Throw ex
    //// End Try
    //// End Function
    //public  DataSet GetRoundSettings()
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("Rounding_select");
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  int saveRoundSettings(AdminInfo obj)
    //{
    //    try
    //    {
    //        Int32 intReturnValue;

    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("SaveRoundSettings");
    //        dynamicParameters.Add("@intProdRoundId", obj.RoundId);
    //        // dynamicParameters.Add("@sitProductTypeId", DbType.Int16, obj.PrdTypeId)
    //        // dynamicParameters.Add("@intStructureElementTypeId", obj.StructureElementId)
    //        // dynamicParameters.Add("@numMinDia", obj.MinDia)
    //        // dynamicParameters.Add("@numMaxDia", obj.MaxDia)
    //        dynamicParameters.Add("@numMinLength", obj.MinLen);
    //        dynamicParameters.Add("@numMaxLength", obj.MaxLen);
    //        dynamicParameters.Add("@numRoundLength", obj.RoundLength);
    //        // dynamicParameters.Add("@intStatusId", obj.StatusId)
    //        dynamicParameters.Add("@intUserId", obj.UserId);
    //        DataAccess.DataAccess.db.AddOutParameter("@intOutput", 0);
    //        DataAccess.DataAccess.GetScalar(dbcom);
    //        intReturnValue = (dbcom.Parameters["@intOutput"].Value);
    //        return intReturnValue;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  int DeleteRoundSettings(AdminInfo obj)
    //{
    //    try
    //    {
    //        Int32 intReturnValue;
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("RoundSettings_Del");
    //        dynamicParameters.Add("@intProdRoundId", obj.RoundId);
    //        DataAccess.DataAccess.db.AddOutParameter("@intOutput", 0);
    //        DataAccess.DataAccess.GetScalar(dbcom);
    //        intReturnValue = (dbcom.Parameters["@intOutput"].Value);
    //        return intReturnValue;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetBPCSummaryExportExcel(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("RptBPCSummary_ExportToExcel");
    //        dynamicParameters.Add("@intCustomerID", obj.CustomerId);
    //        dynamicParameters.Add("@intContractID", obj.ContractId);
    //        dynamicParameters.Add("@intProjectID", obj.ProjectId);
    //        dynamicParameters.Add("@sitProductTypeId", obj.ProductType);
    //        dynamicParameters.Add("@vchStartDate", obj.StartDate);
    //        dynamicParameters.Add("@vchEndDate", obj.EndDt);
    //        dynamicParameters.Add("@vchSearch", obj.Search);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetProjectWiseGroupMarking(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("RptProjectWiseGroupMarking_Get");
    //        dynamicParameters.Add("@intCustomerID", obj.CustomerId);
    //        dynamicParameters.Add("@intContractID", obj.ContractId);
    //        dynamicParameters.Add("@intProjectID", obj.ProjectId);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  int DeleteTransCustomerName(AdminInfo obj)
    //{
    //    try
    //    {
    //        Int32 intReturnValue;
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("TransactionTables_Delete_GroupMarks_Given_CustomerName");
    //        dynamicParameters.Add("@vchCustomerName", obj.CustomerName);
    //        DataAccess.DataAccess.db.AddOutParameter("@intOutput", 0);
    //        DataAccess.DataAccess.GetScalar(dbcom);
    //        intReturnValue = (dbcom.Parameters["@intOutput"].Value);
    //        return intReturnValue;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  int DeleteTransContract(AdminInfo obj)
    //{
    //    try
    //    {
    //        Int32 intReturnValue;
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("TransactionTables_Delete_GroupMarks_Given_ContractNumber");
    //        dynamicParameters.Add("@vchContractNumber", obj.ContractNo);
    //        DataAccess.DataAccess.db.AddOutParameter("@intOutput", 0);
    //        DataAccess.DataAccess.GetScalar(dbcom);
    //        intReturnValue = (dbcom.Parameters["@intOutput"].Value);
    //        return intReturnValue;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  int DeleteTransProject(AdminInfo obj)
    //{
    //    try
    //    {
    //        Int32 intReturnValue;
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("Transactiontables_Delete_GroupMarks_For_ProjectCode");
    //        dynamicParameters.Add("@vchProjectCode", obj.ProjectCode);
    //        DataAccess.DataAccess.db.AddOutParameter("@intOutput", 0);
    //        DataAccess.DataAccess.GetScalar(dbcom);
    //        intReturnValue = (dbcom.Parameters["@intOutput"].Value);
    //        return intReturnValue;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  int DeleteTransGroupMarkId(AdminInfo obj)
    //{
    //    try
    //    {
    //        Int32 intReturnValue;
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("TransactionTables_Delete_GroupMarkId");
    //        dynamicParameters.Add("@vchGroupMarkIDs", obj.GMName);
    //        // dynamicParameters.Add("@intStructureElementId", obj.StructureElementTypeId)
    //        DataAccess.DataAccess.db.AddOutParameter("@intOutput", 0);
    //        DataAccess.DataAccess.GetScalar(dbcom);
    //        intReturnValue = (dbcom.Parameters["@intOutput"].Value);
    //        return intReturnValue;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetAllProjects(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ProjectsAll_Get");
    //        dynamicParameters.Add("@intContractID", obj.ContractId);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetAllContracts(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ContractsAll_Get");
    //        dynamicParameters.Add("@intCustomerCode", obj.CustomerId);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    public int GetExistenceDrainDepthLap(AdminInfo obj)
    {
        try
        {

            int intReturnValue;

            using (var sqlConnection = new SqlConnection(connectionString))
            {

                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@tntParamSetNumber", obj.ParameterSetNo);
                dynamicParameters.Add("@intOutput", 0);

                sqlConnection.Query<int>(SystemConstants.ProjectParameterDrainCheckDepthLap_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

                intReturnValue = dynamicParameters.Get<int>("@Output");
                sqlConnection.Close();

                return intReturnValue;
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    #endregion



    //public  DataSet GetWBSEstimationMaster(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("WBSEstimationMaster_select");
    //        dynamicParameters.Add("@intintStructureElementID", obj.WBSStructureElement);
    //        // dynamicParameters.Add("@intProductCodeId", 0)
    //        dynamicParameters.Add("@sitProductTypeId", obj.WBSProductId);
    //        dynamicParameters.Add("@intWBSTypeId", obj.WBSTypeId);

    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  int InsertWBSEstimationMaster(AdminInfo obj)
    //{
    //    try
    //    {
    //        Int32 intreturnValue;
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("WBSEstimationMaster_Insert");
    //        dynamicParameters.Add("@tntProductionDays", obj.WBSProductionDays);
    //        dynamicParameters.Add("@intWBSEstimationDaysId", obj.WBSEstimationDays);
    //        dynamicParameters.Add("@intStructureElementID", obj.WBSStructureElement);
    //        dynamicParameters.Add("@sitProductTypeId", obj.WBSProductId);
    //        dynamicParameters.Add("@intWBSTypeId", obj.WBSTypeId);
    //        dynamicParameters.Add("@tntManDays", obj.WBSManDays);
    //        dynamicParameters.Add("@tntCycleDays", obj.WBSCycleDays);
    //        dynamicParameters.Add("@intstandBydays", obj.WBSStandByDays);
    //        dynamicParameters.Add("@intBufferDays", obj.WBSBufferHours);
    //        dynamicParameters.Add("@intSimilarProductionDays", obj.WBSSPDays);
    //        dynamicParameters.Add("@intSimilarManDays", obj.WBSSMDays);
    //        dynamicParameters.Add("@intStoreyFrom", obj.WBSStoreyFrom);
    //        dynamicParameters.Add("@intStoreyTo", obj.WBSStoreyTo);
    //        dynamicParameters.Add("@intSimilarStandByDays", obj.WBSSSBDays);
    //        dynamicParameters.Add("@intSimilarCycleDays", obj.WBSCHDays);
    //        dynamicParameters.Add("@intUOMDays", obj.WBSUOM);
    //        dynamicParameters.Add("@intCollapseLevelId", obj.WBSCollaspeId);
    //        dynamicParameters.Add("@intCreatedUId", obj.UserId);
    //        dynamicParameters.Add("@intUpdatedUId", obj.UserId);
    //        dynamicParameters.Add("@intWBSGroupId", obj.WBSGroupId);
    //        DataAccess.DataAccess.db.AddOutParameter("@intOutput", 0);
    //        DataAccess.DataAccess.ExecuteNonQuery(dbcom);
    //        intreturnValue = (dbcom.Parameters["@intOutput"].Value);
    //        return intreturnValue;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  SqlDataReader GetGMId(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("GroupMarkId_Get");
    //        dynamicParameters.Add("@vchGroupmarkingname", obj.GMName);
    //        return DataAccess.DataAccess.ExecuteReader(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetShapePopUpDetails(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ShapePopUpDetails_Get");
    //        dynamicParameters.Add("@intProductMarkId", obj.ProductMarkingId);
    //        dynamicParameters.Add("@vchShapeCode", obj.vchShapeCode);
    //        dynamicParameters.Add("@vchProductCode", obj.ProductCode);
    //        // dynamicParameters.Add("@vchProductCode", obj.ProductCode)
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  int GroupMarkExists(AdminInfo obj)
    //{
    //    try
    //    {
    //        Int32 intReturnValue;
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("GroupMarkingName_DestinationExists_Get");
    //        dynamicParameters.Add("@intDestProjectId", obj.DestProjectId);
    //        dynamicParameters.Add("@vchDestGroupMarkingName", obj.DestGroupMarkingName);
    //        dynamicParameters.Add("@intStructureElementTypeId", obj.DestStructureElementTypeId);
    //        dynamicParameters.Add("@sitProductTypeId", obj.DestProductTypeId);
    //        DataAccess.DataAccess.db.AddOutParameter("@intOutput", 0);
    //        DataAccess.DataAccess.GetScalar(dbcom);
    //        intReturnValue = (dbcom.Parameters["@intOutput"].Value);
    //        return intReturnValue;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}
    //public  DataSet GetMinMaxPrcEnvelopValues(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("GetMinMaxPrcEnvelopValues_Get");
    //        dynamicParameters.Add("@intStructElemId", obj.StructElemId);

    //        DataAccess.DataAccess.GetScalar(dbcom);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }

    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public DataSet GetPinMasterDetails()
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("PinMasterDetails_Get");
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}


    //public int uspUpdatePinMaster(AdminInfo obj)
    //{
    //    int returnval;
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("UpdatePinMasterDetails");
    //        dynamicParameters.Add("@old_Norm", obj.norm);
    //        dynamicParameters.Add("@old_US", obj.us);
    //        dynamicParameters.Add("@old_Grade", obj.Grade);
    //        dynamicParameters.Add("@old_barDes", obj.barDes);
    //        dynamicParameters.Add("@old_pinStirrups", obj.pinStirrups);
    //        dynamicParameters.Add("@old_pinMainBars", obj.pinMainBars);
    //        dynamicParameters.Add("@old_pinHooks", obj.pinHooks);
    //        dynamicParameters.Add("@old_smLong", obj.smLong);
    //        dynamicParameters.Add("@old_smTrans", obj.smTrans);
    //        dynamicParameters.Add("@new_pinStirrups", obj.pinStirrups_new);
    //        returnval = DataAccess.DataAccess.ExecuteNonQuery(dbcom);
    //        return returnval;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}


    //public int uspDeletePinMaster(AdminInfo obj)
    //{
    //    int returnval;
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("DEletePinMasterDetails");
    //        dynamicParameters.Add("@del_Norm", obj.norm);
    //        dynamicParameters.Add("@del_US", obj.us);
    //        dynamicParameters.Add("@del_Grade", obj.Grade);
    //        dynamicParameters.Add("@del_barDes", obj.barDes);
    //        dynamicParameters.Add("@del_pinStirrups", obj.pinStirrups);
    //        dynamicParameters.Add("@del_pinMainBars", obj.pinMainBars);
    //        dynamicParameters.Add("@del_pinHooks", obj.pinHooks);
    //        dynamicParameters.Add("@del_smLong", obj.smLong);
    //        dynamicParameters.Add("@del_smTrans", obj.smTrans);

    //        returnval = DataAccess.DataAccess.ExecuteNonQuery(dbcom);
    //        return returnval;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public int uspInsertPinMaster(AdminInfo obj)
    //{
    //    int returnval;
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("InsertPinMasterDetails");
    //        dynamicParameters.Add("@ins_Norm", obj.norm);
    //        dynamicParameters.Add("@ins_Grade", obj.Grade);
    //        dynamicParameters.Add("@ins_barDes", obj.barDes);
    //        dynamicParameters.Add("@ins_pin", obj.pinStirrups);

    //        returnval = DataAccess.DataAccess.ExecuteNonQuery(dbcom);
    //        return returnval;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}


    //public DataSet GetShapeGroupDetails()
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ShapeGroupDetails_Get");
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public int uspUpdateShapeGroup(AdminInfo obj)
    //{
    //    int returnval;
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("UpdateShapeGroupDetails");
    //        dynamicParameters.Add("@old_Desc", obj.ShapeGroupDescription);
    //        dynamicParameters.Add("@old_Code", obj.Code);

    //        dynamicParameters.Add("@new_Desc", obj.ShapeGroupDescription_New);
    //        dynamicParameters.Add("@new_Code", obj.Code_new);

    //        dynamicParameters.Add("@username", obj.UserName);

    //        returnval = DataAccess.DataAccess.ExecuteNonQuery(dbcom);
    //        return returnval;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public int uspDeleteShapeGroup(AdminInfo obj)
    //{
    //    int returnval;
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("DeleteShapeGroupDetails");
    //        dynamicParameters.Add("@del_Desc", obj.ShapeGroupDescription);
    //        dynamicParameters.Add("@del_Code", obj.Code);

    //        returnval = DataAccess.DataAccess.ExecuteNonQuery(dbcom);
    //        return returnval;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public int uspInsertShapeGroup(AdminInfo obj)
    //{
    //    int returnval;
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("InsertShapeGroupDetails");
    //        dynamicParameters.Add("@desc", obj.ShapeGroupDescription);
    //        dynamicParameters.Add("@code", obj.Code);
    //        dynamicParameters.Add("@username", obj.UserName);

    //        returnval = DataAccess.DataAccess.ExecuteNonQuery(dbcom);
    //        return returnval;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}


    //public DataSet GetCouplerMaterialDetails()
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("CouplerMaterialDetails_Get");
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}


    //public int uspUpdateCouplerMaterial(AdminInfo obj)
    //{
    //    int returnval;
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("UpdateCouplerMaterialDetails");
    //        dynamicParameters.Add("@DiaPrefix", obj.DiaPrefix);
    //        dynamicParameters.Add("@old_CopLength", DbType.Decimal, obj.CouplerLength_old);
    //        dynamicParameters.Add("@new_CopLength", DbType.Decimal, obj.CouplerLength_new);
    //        returnval = DataAccess.DataAccess.ExecuteNonQuery(dbcom);
    //        return returnval;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public int uspDeleteCouplerMaterial(AdminInfo obj)
    //{
    //    int returnval;
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("DeleteCouplerMaterialDetails");
    //        dynamicParameters.Add("@DiaPrefix", obj.DiaPrefix);
    //        returnval = DataAccess.DataAccess.ExecuteNonQuery(dbcom);
    //        return returnval;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public int uspInsertCouplerMaterial(AdminInfo obj)
    //{
    //    int returnval;
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("InsertCouplerMaterialDetails");
    //        dynamicParameters.Add("@CouplerType", obj.CouplerType);
    //        dynamicParameters.Add("@Grade", obj.Grade);
    //        dynamicParameters.Add("@CopDiameter", DbType.Decimal, obj.CopDiameter);
    //        dynamicParameters.Add("@CopPrefix", obj.DiaPrefix);
    //        dynamicParameters.Add("@Coupler", obj.Coupler);
    //        dynamicParameters.Add("@Thread", obj.Thread);
    //        dynamicParameters.Add("@CopLength", DbType.Decimal, obj.CouplerLength_new);

    //        returnval = DataAccess.DataAccess.ExecuteNonQuery(dbcom);
    //        return returnval;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //// added for difot
    //public  int FindMaterialID(string matcode)
    //{
    //    try
    //    {
    //        Int32 intReturnValue;
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("FindSAPMaterialID");
    //        dynamicParameters.Add("@MaterialCode", matcode);

    //        intReturnValue = System.Convert.ToInt32(DataAccess.DataAccess.GetScalar(dbcom));
    //        return intReturnValue;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public DataSet LoadPostedByUsers(DateTime fromdate, DateTime todate)
    //{
    //    try
    //    {
    //        DataSet dt;
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("GetPostedByUsers");
    //        dynamicParameters.Add("@startdate", DbType.DateTime, fromdate);
    //        dynamicParameters.Add("@enddate", DbType.DateTime, todate);
    //        dt = DataAccess.DataAccess.GetDataSet(dbcom);
    //        return dt;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public DataSet LoadReleasedByUsers(DateTime fromdate, DateTime todate)
    //{
    //    try
    //    {
    //        DataSet dt;
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("GetReleasedByUsers");
    //        dynamicParameters.Add("@startdate", DbType.DateTime, fromdate);
    //        dynamicParameters.Add("@enddate", DbType.DateTime, todate);
    //        dt = DataAccess.DataAccess.GetDataSet(dbcom);
    //        return dt;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public DataSet GetTonnageReport(AdminInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("GetDataOnFilter");
    //        dynamicParameters.Add("@startdateinput", DbType.DateTime, obj.fromDateProperty);
    //        dynamicParameters.Add("@enddateinput", DbType.DateTime, obj.toDateProperty);
    //        dynamicParameters.Add("@vchUserNames", obj.userNameList);
    //        dynamicParameters.Add("@ReportType", obj.TonnageReportType);

    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //// Start:Added by Siddhi for Mesh Shape Code 
    //public DataSet CheckMeshShapeGroupExists(string meshShapeGroup)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("CheckMeshShapeGroupExists");
    //        dynamicParameters.Add("@meshShapeGroup", meshShapeGroup);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}
    //public DataSet CheckShapeExists(string shapeCode)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("CheckShapeExists");
    //        dynamicParameters.Add("@shapeCode", shapeCode);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}
}





