using System;
using System.Collections.Generic;
using System.Data;
using UtilityService.Dtos;
using UtilityService.Context;
using UtilityService.Interface;
//using UtilitiesService.Models;
using Microsoft.EntityFrameworkCore;
using UtilityService.Constants;
using Microsoft.Data.SqlClient;
using Dapper;

public class UtilitiesDal:IUtilities
{

    private UtilityServiceContext _dbContext;
    private readonly IConfiguration _configuration;
    private string connectionString;

    public UtilitiesDal(UtilityServiceContext dbContext, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _configuration = configuration;

        connectionString = _configuration.GetConnectionString(SystemConstant.DefaultDBConnection);
    }


    //public async Task<int> InsertCopyDetailing(UtilitiesInfo obj)
    //{
    //    Int32 intReturnValue = 0;
    //    IEnumerable<UtilitiesInfo> addMeshDtos;
    //    try
    //    {
    //        using (var sqlConnection = new SqlConnection(connectionString))
    //        {

    //            sqlConnection.Open();
    //            var dynamicParameters = new DynamicParameters();
    //            dynamicParameters.Add("@intStructureElementTypeIdSource", obj.StructureElement);
    //            dynamicParameters.Add("@sitProductTypeIdSource", obj.ProductTypeId);
    //            dynamicParameters.Add("@intProjectIdSource", obj.ProjectId);
    //            dynamicParameters.Add("@intWBSTypeIdSource", obj.WBSTypeId);
    //            dynamicParameters.Add("@vchWBS1Source", obj.WBS1);
    //            dynamicParameters.Add("@vchWBS2Source", obj.WBS2);
    //            dynamicParameters.Add("@intStructureElementTypeIdDestination", obj.StructureElementDest);
    //            dynamicParameters.Add("@sitProductTypeIdDestination", obj.ProductTypeIdDest);
    //            dynamicParameters.Add("@intProjectIdDestination", obj.ProjectIdDest);
    //            dynamicParameters.Add("@intWBSTypeIdDestination ", obj.WBSTypeIdDest);
    //            dynamicParameters.Add("@vchWBS1Destination", obj.WBS1Dest);
    //            dynamicParameters.Add("@vchWBS2Destination", obj.WBS2Dest);
    //            dynamicParameters.Add("@vchWBSElementIdSource", obj.StrWBSElementSource);
    //            dynamicParameters.Add("@vchWBSElementIdDest", obj.StrWBSElementDest);
    //            dynamicParameters.Add("@intUserId", obj.UserId);
    //            dynamicParameters.Add("@Output", null, dbType: DbType.Int32, ParameterDirection.Output);

    //            sqlConnection.Query<UtilitiesInfo>(SystemConstant.CopyDetailing_Insert, dynamicParameters, commandType: CommandType.StoredProcedure);

    //            //addMeshDtos = sqlConnection.QueryFirstOrDefault(SystemConstant.CopyDetailing_Insert, dynamicParameters, commandType: CommandType.StoredProcedure);
    //            sqlConnection.Close();


    //        }

    //        return intReturnValue;

    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetWBSId(UtilitiesInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("CopyWBSElementID_Select");
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@intProjectId", DbType.Int32, obj.ProjectId);
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@intWBSTypeId", DbType.Int32, obj.WBSTypeId);
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@vchWBS1", DbType.String, obj.WBS1);
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@vchWBS2", DbType.String, obj.WBS2);
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@intStructureElementTypeIdSource", DbType.Int32, obj.StructureElement);
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@sitProductTypeIdSource", DbType.Int32, obj.ProductTypeId);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetCopyWBSId(UtilitiesInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("CopyWBSElementID_Get");
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@intProjectId", DbType.Int32, obj.ProjectId);
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@intWBSTypeId", DbType.Int32, obj.WBSTypeId);
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@vchWBS1", DbType.String, obj.WBS1);
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@vchWBS2", DbType.String, obj.WBS2);
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@vchWBS3", DbType.String, obj.WBS3);
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@intStructureElementTypeIdSource", DbType.Int32, obj.StructureElement);
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@sitProductTypeIdSource", DbType.Int32, obj.ProductTypeId);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
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
    //        throw ex;
    //    }
    //}

    //public  DataSet GetSapContractConvert()
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("SapContractConvert_Get");
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}
    //public  int InsertSapContractConvert(UtilitiesInfo obj)
    //{
    //    Int32 intReturnValue;
    //    // Dim intAffectedRows As Int32
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("SapContractConvert_Insert");
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@intSAPContractNo", DbType.Int32, obj.SapContractNo);
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@chrStandardType", DbType.String, obj.StandardType);
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@vchStandardCode", DbType.String, obj.StandardCode);
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@tntStatus", DbType.Int32, obj.Status);
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@intUserId", DbType.Int32, obj.UserId);
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@intCustomerCode", DbType.String, obj.CustomerCode);
    //        DataAccess.DataAccess.db.AddOutParameter(dbcom, "@Output", DbType.Int32, 0);
    //        DataAccess.DataAccess.GetScalar(dbcom);
    //        // intAffectedRows = DataAccess.DataAccess.ExecuteNonQuery(dbcom)
    //        // If intAffectedRows <= 0 Then
    //        // intReturnValue = -1
    //        // Else
    //        // intReturnValue = (dbcom.Parameters("@Output").Value)
    //        // End If
    //        intReturnValue = (dbcom.Parameters["@Output"].Value);
    //        return intReturnValue;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}
    //public  DataSet GetSapProjectConvert()
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("SapProjectConvert_Get");
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}
    //public  int InsertSapProjectConvert(UtilitiesInfo obj)
    //{
    //    Int32 intReturnValue;
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("SapProjectConvert_Insert");
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@intSAPProjectNo", DbType.Int32, obj.SapProjectId);
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@intContractId", DbType.Int32, obj.Contract);
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@vchProjectAbbr", DbType.String, obj.ProjectAbbr);
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@intProjectTypeId", DbType.Int32, obj.ProjectType);
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@intMarketSector", DbType.Int32, obj.MarkerSector);
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@tntStatus", DbType.Int32, obj.Status);
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@intUserId", DbType.Int32, obj.UserId);
    //        DataAccess.DataAccess.db.AddOutParameter(dbcom, "@Output", DbType.Int32, 0);
    //        DataAccess.DataAccess.GetScalar(dbcom);
    //        intReturnValue = (dbcom.Parameters["@Output"].Value);
    //        return intReturnValue;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}
    //public  DataSet GetContract(UtilitiesInfo obj)
    //{
    //    try
    //    {
    //        SqlCacheDependency SqlDep;
    //        DataSet dsCache = (DataSet)HttpRuntime.Cache["CacheContract"];
    //        if (dsCache == null)
    //        {
    //            SqlDep = new SqlCacheDependency("NDSCaching", "contractmaster");
    //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("Contract_Get");
    //            dsCache = DataAccess.DataAccess.GetDataSet(dbcom);
    //            HttpRuntime.Cache.Insert("CacheContract", dsCache, SqlDep);
    //        }

    //        DataView dvcacheContract;
    //        dvcacheContract = dsCache.Tables[0].DefaultView;
    //        dvcacheContract.RowFilter = "intCustomerCode = " + obj.CustomerCode;

    //        DataSet dsfilterContract = new DataSet();
    //        dsfilterContract.Tables.Add(dvcacheContract.ToTable());
    //        return dsfilterContract;
    //    }
    //    // dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("Contract_Get")
    //    // DataAccess.DataAccess.db.AddInParameter(dbcom, "@intCustomerCode", DbType.Int32, obj.CustomerCode)
    //    // Return DataAccess.DataAccess.GetDataSet(dbcom)
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    public async Task<IEnumerable<getCopyParameterSetDto>> CopyProjectParameterSetGet(int ProjectId, int ProductTypeId, int MeshFlag, string SElement)
    {
        //List<UtilitiesInfo> utilitiesInfos = new List<UtilitiesInfo> { };
       
        IEnumerable<getCopyParameterSetDto> getCopyParameterSetDtos;
        try
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {

                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@intProjectId",ProjectId);
                dynamicParameters.Add("@sitProductTypeL2Id",ProductTypeId);
                dynamicParameters.Add("@MeshFlag",MeshFlag);
                dynamicParameters.Add("@vchStructureElementType",SElement);

                getCopyParameterSetDtos = sqlConnection.Query<getCopyParameterSetDto>(SystemConstant.CopyProjectParameterSet_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return getCopyParameterSetDtos;
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
       
    }
    //public  DataSet CopyProjectParamLabelGet(UtilitiesInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ProjectParemeterSetIncre_Get_PV"); // modified for ship to party
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@intProjectId", DbType.Int32, obj.ProjectId);
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@sitProductTypeL2Id", DbType.Int32, obj.ProductTypeId);
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@tntParamSetNumber", DbType.Int32, obj.ParamSetNumber);
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@intDestProjectId", DbType.Int32, obj.ProjectIdDest);
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@sitDestProductTypeId", DbType.Int32, obj.ProductTypeIdDest);
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@intMeshFlag", DbType.Int32, obj.MeshFlag);

    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    public async Task<int> InsertCopyProjectParameter(InsertCopyProjectParamDto obj)
    {
        int Output = 0;
       
            using (var sqlConnection = new SqlConnection(connectionString))
            {

                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@tntParamSetNumberSource",obj.tntParamSetNumberSource);
                dynamicParameters.Add("@intProjectIdDest", obj.intProjectIdDest);
                dynamicParameters.Add("@intParameteSetDest", obj.intParameteSetDest);
                dynamicParameters.Add("@intUserId", obj.intUserId);
                
                dynamicParameters.Add("@OUTPUT", null, dbType: DbType.Int32, ParameterDirection.Output);
                sqlConnection.Query<int>(SystemConstant.CopyProjectParameter_Insert, dynamicParameters, commandType: CommandType.StoredProcedure);
               
                Output = dynamicParameters.Get<int>("@OUTPUT");
                sqlConnection.Close();
             
               
            }

            return Output;

    }

    //public  System.Data.DataSet GetNonEstimatedStorey(UtilitiesInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("WBSStoreyNonEstimated_Get");
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@intStructureElementTypeId", DbType.Int32, obj.StructureElement);
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@sitProductTypeId", DbType.Int32, obj.ProductTypeId);
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@intProjectId", DbType.Int32, obj.ProjectId);
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@intWBSTypeId", DbType.Int32, obj.WBSTypeId);
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@vchWBS1", DbType.String, obj.WBS1);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  System.Data.DataSet GetEstimatedStorey(UtilitiesInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("WBSStoreyEstimated_Get");
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@intStructureElementTypeId", DbType.Int32, obj.StructureElement);
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@sitProductTypeId", DbType.Int32, obj.ProductTypeId);
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@intProjectId", DbType.Int32, obj.ProjectId);
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@intWBSTypeId", DbType.Int32, obj.WBSTypeId);
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@vchWBS1", DbType.String, obj.WBS1);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  System.Data.DataSet GetNonEstimatedStoreyDest(UtilitiesInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("WBSStoreyNonEstimatedForDest_Get");
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@intStructureElementTypeId", DbType.Int32, obj.StructureElement);
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@sitProductTypeId", DbType.Int32, obj.ProductTypeId);
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@intProjectId", DbType.Int32, obj.ProjectId);
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@intWBSTypeId", DbType.Int32, obj.WBSTypeId);
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@vchWBS1", DbType.String, obj.WBS1);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  System.Data.DataSet GetEstimatedStoreyDest(UtilitiesInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("WBSStoreyEstimatedDest_Get");
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@intStructureElementTypeId", DbType.Int32, obj.StructureElement);
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@sitProductTypeId", DbType.Int32, obj.ProductTypeId);
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@intProjectId", DbType.Int32, obj.ProjectId);
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@intWBSTypeId", DbType.Int32, obj.WBSTypeId);
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@vchWBS1", DbType.String, obj.WBS1);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public  DataSet GetEstimatedWBS1(UtilitiesInfo obj)
    //{
    //    try
    //    {
    //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("EstimatedWBS1_Get");
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@intStructureElementTypeId", DbType.Int32, obj.StructureElement);
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@sitProductTypeId", DbType.Int32, obj.ProductTypeId);
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@intProjectId", DbType.Int32, obj.ProjectId);
    //        DataAccess.DataAccess.db.AddInParameter(dbcom, "@intWBSTypeId", DbType.Int32, obj.WBSTypeId);
    //        return DataAccess.DataAccess.GetDataSet(dbcom);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}


}
