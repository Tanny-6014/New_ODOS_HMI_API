using DrainService.Interfaces;
using DrainService.Repositories;
using DrainService.Dtos;
using System.Data;
using Microsoft.VisualBasic;
using System.Configuration;
using System.Data.SqlClient;
using DrainService.Constants;
using Dapper;
using static System.Net.Mime.MediaTypeNames;

namespace DrainService.Repositories
{
    public class DetailDal : IDetailDal
    {

        
        // private DrainServiceContext _dbContext;
        private readonly IConfiguration _configuration;
        private string connectionString = "Server=NSPRDDB19\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=ndswebapps;Password=DBAdmin4*NDS;Connection Timeout=36000000";
        //private string connectionString = "Server=NSPRDDB19\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=ndswebapps;Password=DBAdmin4*NDS;Connection Timeout=36000000";

        //public DetailDal(IConfiguration configuration)
        //{
        //    //_dbContext = dbContext;
        //    _configuration = configuration;

        //    connectionString = _configuration.GetConnectionString(SystemConstants.DefaultDBConnection);
        //}

        public DetailDal()
        {
        }

        #region Beam Cage

          //    public int InsUpdWBSDetails(DetailInfo obj)
        //    {
        //        Int32 intReturnValue = 0;
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BCDWBSGroupInsUpd");
        //            dynamicParameters.Add("@vchWBSElementId", obj.vchWBSElementIds);
        //            dynamicParameters.Add("@intProjectid", obj.intProjectId);
        //            dynamicParameters.Add("@intGroupMarkid", obj.intGroupMarkId);
        //            DataAccess.DataAccess.db.AddOutParameter("@Output", 0);

        //            intReturnValue = DataAccess.DataAccess.GetScalar(dbcom); // (dbcom.Parameters("@Output").Value)
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        return intReturnValue;
        //    }
          
          #region Get

        //    /// <summary>
        //    ///     ''' 
        //    ///     ''' </summary>
        //    ///     ''' <param name="obj"></param>
        //    ///     ''' <returns></returns>
        //    ///     ''' <remarks></remarks>
        //    ///     '''
        //    public DataSet GetSAPMaterialId(DetailInfo obj)
        //    {
        //        try
        //        {
        //            SqlCacheDependency SqlDep;
        //            DataSet dsCache = (DataSet)HttpRuntime.Cache["CacheSAPMaterialId"];
        //            if (dsCache == null)
        //            {
        //                SqlDep = new SqlCacheDependency("NDSCaching", "sapmaterialmaster");
        //                dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BCDSAPMaterialId_cache_Get");
        //                dsCache = DataAccess.DataAccess.GetDataSet(dbcom);
        //                HttpRuntime.Cache.Insert("CacheSAPMaterialId", dsCache, SqlDep);
        //            }

        //            DataView dvCacheSAPMaterialId;
        //            string strMiscFilter = ConfigurationManager.AppSettings.Get("vchMisc");
        //            string strPrecageFilter = ConfigurationManager.AppSettings.Get("vchPrecage");
        //            string strCoreCageFilter = ConfigurationManager.AppSettings.Get("vchCoreCage");
        //            dvCacheSAPMaterialId = dsCache.Tables[0].DefaultView;

        //            if (!obj.vchStructureElementType == null)
        //            {
        //                var strStructureElementType = obj.vchStructureElementType.ToLower;
        //                if (strStructureElementType == "slab")
        //                    strStructureElementType = "wall";

        //                if (obj.vchStructureElementType.ToLower.ToString() == "column")
        //                    dvCacheSAPMaterialId.RowFilter = "Material LIKE '" + strPrecageFilter + "' AND (MaterialDescription LIKE '%" + strStructureElementType + "%' OR MaterialDescription LIKE " + "'" + strMiscFilter + "'" + " OR MaterialDescription LIKE " + "'" + strCoreCageFilter + "'" + ") AND (StructureElement IS NOT NULL OR StructureElement <> '')";
        //                else
        //                    dvCacheSAPMaterialId.RowFilter = "(Material LIKE '" + strPrecageFilter + "' AND MaterialDescription LIKE '%" + strStructureElementType + "%' OR MaterialDescription LIKE " + "'" + strMiscFilter + "'" + ") AND (StructureElement IS NOT NULL OR StructureElement <> '')";
        //            }

        //            DataSet dsCacheSAPMaterialId = new DataSet();
        //            dsCacheSAPMaterialId.Tables.Add(dvCacheSAPMaterialId.ToTable());
        //            return dsCacheSAPMaterialId;
        //        }
        //        // dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BCDSAPMaterialId_cache_Get")
        //        // dynamicParameters.Add("@vchStructureElement", obj.vchStructureElementType)
        //        // Return DataAccess.DataAccess.GetDataSet(dbcom)
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }


        //    public DataSet GetPRCDetails(DetailInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BCDPRCDetails_Get");
        //            dynamicParameters.Add("@intSEDetailingId",obj.SEDetailingId);
        //            dynamicParameters.Add("@bitParentFlag",obj.ParentFlag);
        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public DataSet GetAccProductMarkingDetails(DetailInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ACC_ProductMarkingDetails_GET");
        //            dynamicParameters.Add("@intSEDetailingId", obj.SEDetailingId);
        //            dynamicParameters.Add("@intStructureMarkId", obj.intStructureMarkId);
        //            dynamicParameters.Add("@vchStructureElement", obj.vchStructureElementType);

        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public DataSet GetAccSAPMaterialDetails(DetailInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ACC_SAPMaterial_GET");
        //            dynamicParameters.Add("@vchSAPCode", obj.SAPMaterialCode);
        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public int GetCreepLength(DetailInfo obj)
        //    {
        //        string strReturnValue = "";
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BCDCreepLength_Get");
        //            dynamicParameters.Add("@intProductCodeid",obj.intProductCodeId);
        //            dynamicParameters.Add("@intShapeid",obj.intShapeCodeId);
        //            dynamicParameters.Add("@numOutput",0);
        //            strReturnValue = DataAccess.DataAccess.GetScalar(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        return Interaction.IIf(strReturnValue.Equals(""), -1, (int)strReturnValue);
        //    }
        public List<GetHeaderInfoDto> GetHeaderInfo(DetailInfo obj)
        {
            try
            {
                DataSet ds = new DataSet();
                IEnumerable<GetHeaderInfoDto> getHeaderInfos;
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intProjectId", obj.intProjectId);
                    getHeaderInfos = sqlConnection.Query<GetHeaderInfoDto>(SystemConstants.BCDHeaderInfo_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

                    return getHeaderInfos.ToList();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //    public DataSet GetParameterInfo(DetailInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BCDParameterInfo_Get");
        //            dynamicParameters.Add("@tntParamSetNumber",obj.tntParamSetNumber);

        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public DataSet GetStructureMarking(DetailInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BCDStructureMarking_Get");
        //            dynamicParameters.Add("@vchGroupMarkingName", obj.vchGroupMarkingName);
        //            dynamicParameters.Add("@intProjectId", obj.intProjectId);
        //            dynamicParameters.Add("@inSEDetailingId", obj.SEDetailingId);
        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public DataSet GetProductMarking(DetailInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BCDProductMarking_Get");
        //            dynamicParameters.Add("@intStructureMarkId", obj.intStructureMarkId);
        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public DataSet ParameterInfoByPrjID_Get(DetailInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BCDParameterInfoByPrjID_Get");
        //            dynamicParameters.Add("@intProjectID", obj.intProjectId);
        //            dynamicParameters.Add("@sitProductTypeId", obj.sitProductTypeId);

        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public DataSet ParameterInfoByPrjIDColumn_Get(DetailInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BCDParameterInfoByPrjIDColumn_Get");
        //            dynamicParameters.Add("@intProjectID", obj.intProjectId);
        //            dynamicParameters.Add("@sitProductTypeId", obj.sitProductTypeId);
        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public DataSet GetWBSElements(DetailInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BCDWBSElements_Get");
        //            dynamicParameters.Add("@intProjectID", obj.intProjectId);
        //            dynamicParameters.Add("@vchWBSType", obj.vchWBSType);
        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public DataSet GetProductTypeByStructElement(DetailInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BCDProductTypeByStructElm_Get");
        //            dynamicParameters.Add("@vchStructureElementType", obj.vchStructureElementType);
        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public DataSet GetProductCABDetail(DetailInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BCDProductCABDetail_Get");
        //            dynamicParameters.Add("@intSEDetailingId",obj.SEDetailingId);
        //            dynamicParameters.Add("@intStructureMarkId", obj.intStructureMarkId);
        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public DataSet GetProductAccessoriesDetail(DetailInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ACC_ProductMarkingDetails_GET");
        //            dynamicParameters.Add("@intSEDetailingId", obj.SEDetailingId);
        //            dynamicParameters.Add("@intStructureMarkId", obj.intStructureMarkId);
        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public DataSet GetAccSAPDetails(DetailInfo obj)
        //    {
        //        try
        //        {
        //            SqlCacheDependency SqlDep;
        //            DataSet dsCache = (DataSet)HttpRuntime.Cache["CacheSAPMaterialId"];
        //            if (dsCache == null)
        //            {
        //                SqlDep = new SqlCacheDependency("NDSCaching", "sapmaterialmaster");
        //                dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BCDSAPMaterialId_cache_Get");
        //                dsCache = DataAccess.DataAccess.GetDataSet(dbcom);
        //                HttpRuntime.Cache.Insert("CacheSAPMaterialId", dsCache, SqlDep);
        //            }

        //            DataView dvCacheAccSAPDetails;
        //            string strRMStock = ConfigurationManager.AppSettings.Get("vchRmStock");
        //            dvCacheAccSAPDetails = dsCache.Tables[0].DefaultView;
        //            dvCacheAccSAPDetails.RowFilter = "(RM = " + strRMStock + " Or Stock = " + strRMStock + ") AND (StructureElement IS NOT NULL OR StructureElement <> '')";

        //            DataSet dsCacheAccSAPDetails = new DataSet();
        //            dsCacheAccSAPDetails.Tables.Add(dvCacheAccSAPDetails.ToTable());
        //            return dsCacheAccSAPDetails;
        //        }
        //        // dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("Acc_SapMaterialList_Get")
        //        // Return DataAccess.DataAccess.GetDataSet(dbcom)
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        public DataSet GetGroupMark(DetailInfo obj)
        {
            try
            {
                DataSet ds = new DataSet();
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intProjectID", obj.intProjectId);
                    ds = (DataSet)sqlConnection.Query<DataSet>(SystemConstants.BCDGroupMark_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                    return ds;

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //    public DataSet GetConsignmentType(DetailInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BCDConsignment_Get");
        //            // dynamicParameters.Add("@intProjectID", obj.intProjectId)
        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        public DataSet GetWBSList(DetailInfo obj)
        {
            try
            {
                DataSet dataSet = new DataSet();
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@vchInputValueParameter", obj.vchInputValueParameter);
                    dataSet = (DataSet)sqlConnection.Query<DataSet>(SystemConstants.BCDWBSList_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                    sqlConnection.Close();
                    return dataSet;

                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetWBSListColumn(DetailInfo obj)
        {
            try
            {
                DataSet dataSet = new DataSet();
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@vchInputValueParameter", obj.vchInputValueParameter);
                    dataSet = (DataSet)sqlConnection.Query<DataSet>(SystemConstants.BCDWBSListColumn_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                    sqlConnection.Close();
                    return dataSet;

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetGroupMarkIsExist(DetailInfo obj)
        {
            string strReturnValue = "";
            try
            {
                DataSet ds = new DataSet();
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@vchGroupMarkingName", obj.vchGroupMarkingName);
                    dynamicParameters.Add("@intProject", obj.intProjectId);
                    dynamicParameters.Add("@vchStructureElement", obj.vchStructureElementType);
                    dynamicParameters.Add("@vchProductType", obj.vchProductType);
                    strReturnValue = sqlConnection.QueryFirstOrDefault<string>(SystemConstants.BCDChkGrpMarkExist_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                    return Convert.ToInt32(strReturnValue);

                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return (int)Interaction.IIf(strReturnValue.Equals(""), -1, strReturnValue);
        }

        //    public int GetGroupMarkNameIsExistnew(DetailInfo obj)
        //    {
        //        string strReturnValue = "";
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BCDIsGrpMarkExist_Get");
        //            dynamicParameters.Add("@vchGroupMarkingName", obj.vchGroupMarkingName);
        //            dynamicParameters.Add("@intGroupRev",obj.tntGroupRevNo);
        //            dynamicParameters.Add("@intProject",obj.intProjectId);
        //            dynamicParameters.Add("@vchStructureElement", obj.vchStructureElementType);
        //            strReturnValue = DataAccess.DataAccess.GetScalar(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        return Interaction.IIf(strReturnValue.Equals(""), -1, (int)strReturnValue);
        //    }

        public int GetStructElementIsExist(int GroupMarkingId, string StructureElementName)
        {
            int intReturnValue;
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intGroupMarkingId", GroupMarkingId);
                    dynamicParameters.Add("@vchStructureElementName", StructureElementName);
                    intReturnValue = sqlConnection.QueryFirstOrDefault<int>(SystemConstants.BCDChkStructElementExist_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                    return intReturnValue;

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return (int)Interaction.IIf(intReturnValue.Equals(""), -1, intReturnValue);
        }

        //    public DataSet GetBeamProduct(DetailInfo obj)
        //    {
        //        try
        //        {
        //            SqlCacheDependency SqlDep;
        //            // ''''''''''''''''''''''''''''''''''''''''''''product types
        //            DataSet dsCacheProductTypes = (DataSet)HttpRuntime.Cache["CacheProductType"];
        //            if (dsCacheProductTypes == null)
        //            {
        //                SqlDep = new SqlCacheDependency("NDSCaching", "producttypemaster");
        //                dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ProductCode_CacheProductType_Get");
        //                dsCacheProductTypes = DataAccess.DataAccess.GetDataSet(dbcom);
        //                HttpRuntime.Cache.Insert("CacheProductType", dsCacheProductTypes, SqlDep);
        //            }
        //            // prc
        //            int intPrcProductType = System.Convert.ToInt32(dsCacheProductTypes.Tables[0].Rows[1]["sitProducTTypeid"]);
        //            // msh
        //            int intMshProductType = System.Convert.ToInt32(dsCacheProductTypes.Tables[0].Rows[0]["sitProducTTypeid"]);
        //            // '''''''''''''''''''''''''''''''''''''''''''''

        //            // '''product code for slab & others (beam,column)
        //            DataSet dsCacheProductCodeSlab = (DataSet)HttpRuntime.Cache["CacheProductCodeSlab"];
        //            DataSet dsCacheProductCodeOthers = (DataSet)HttpRuntime.Cache["CacheProductCodeOthers"];
        //            int intStructureElementTypeId = System.Convert.ToInt32(ConfigurationManager.AppSettings.Get("intStructureElementTypeId"));
        //            if ((obj.intStructureElementTypeId == intStructureElementTypeId))
        //            {
        //                if (dsCacheProductCodeSlab == null)
        //                {
        //                    SqlDep = new SqlCacheDependency("NDSCaching", "productcodemaster");
        //                    dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ProductCode_CacheSlab_Get");
        //                    dsCacheProductCodeSlab = DataAccess.DataAccess.GetDataSet(dbcom);
        //                    HttpRuntime.Cache.Insert("CacheProductCodeSlab", dsCacheProductCodeSlab, SqlDep);
        //                }
        //                return dsCacheProductCodeSlab;
        //            }
        //            else
        //            {
        //                if (dsCacheProductCodeOthers == null)
        //                {
        //                    SqlDep = new SqlCacheDependency("NDSCaching", "productcodemaster");
        //                    dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ProductCode_Cache_Get");
        //                    dsCacheProductCodeOthers = DataAccess.DataAccess.GetDataSet(dbcom);
        //                    HttpRuntime.Cache.Insert("CacheProductCodeOthers", dsCacheProductCodeOthers, SqlDep);
        //                }

        //                DataView dvProductCodeOthers;
        //                string strCP = ConfigurationManager.AppSettings.Get("strCP");
        //                string strCL = ConfigurationManager.AppSettings.Get("strCL");
        //                dvProductCodeOthers = dsCacheProductCodeOthers.Tables[0].DefaultView;
        //                int intProductTypeID;
        //                if ((obj.sitProductTypeId == intPrcProductType))
        //                    intProductTypeID = intMshProductType;
        //                else
        //                    intProductTypeID = obj.sitProductTypeId;
        //                dvProductCodeOthers.RowFilter = "intStructureElementTypeID = " + obj.intStructureElementTypeId + " AND sitProductTypeID = " + intProductTypeID + " AND (vchProductCode not like '" + strCP + "' and vchproductcode not like '" + strCL + "')";

        //                DataSet dsProductCodeOthers = new DataSet();
        //                dsProductCodeOthers.Tables.Add(dvProductCodeOthers.ToTable());
        //                return dsProductCodeOthers;
        //            }
        //        }
        //        // dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ProductCode_Get")
        //        // dynamicParameters.Add("@intStructureElementTypeId", obj.intStructureElementTypeId)
        //        // dynamicParameters.Add("@sitProductTypeId", obj.sitProductTypeId)
        //        // Return DataAccess.DataAccess.GetDataSet(dbcom)
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public DataSet GetBeamShape(DetailInfo obj)
        //    {
        //        try
        //        {
        //            SqlCacheDependency SqlDep;
        //            DataSet dsCache = (DataSet)HttpRuntime.Cache["CacheBCDShapeCode"];
        //            if (dsCache == null)
        //            {
        //                SqlDep = new SqlCacheDependency("NDSCaching", "shapemaster");
        //                dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BCDShapeCode_Cache_Get_Beam");
        //                dsCache = DataAccess.DataAccess.GetDataSet(dbcom);
        //                HttpRuntime.Cache.Insert("CacheBCDShapeCode", dsCache, SqlDep);
        //            }

        //            DataView dvBCDShapeCode;
        //            string strBCDShapeCodeFilter = ConfigurationManager.AppSettings.Get("vchMeshShapeGroup");
        //            dvBCDShapeCode = dsCache.Tables[0].DefaultView;
        //            dvBCDShapeCode.RowFilter = "vchMeshShapeGroup = '" + strBCDShapeCodeFilter + "'"; // ORDER BY chrShapeCode ASC"

        //            DataSet dsBCDShapeCode = new DataSet();
        //            dsBCDShapeCode.Tables.Add(dvBCDShapeCode.ToTable());
        //            return dsBCDShapeCode;
        //        }
        //        // dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BCDShapeCode_Get_Beam")
        //        // Return DataAccess.DataAccess.GetDataSet(dbcom)
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public DataSet GetCappingProduct(DetailInfo obj)
        //    {
        //        try
        //        {
        //            SqlCacheDependency SqlDep;
        //            DataSet dsCache = (DataSet)HttpRuntime.Cache["CacheCapProduct"];
        //            if (dsCache == null)
        //            {
        //                SqlDep = new SqlCacheDependency("NDSCaching", "productcodemaster");
        //                dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("CapProductCode_Cache_Get");
        //                dsCache = DataAccess.DataAccess.GetDataSet(dbcom);
        //                HttpRuntime.Cache.Insert("CacheCapProduct", dsCache, SqlDep);
        //            }

        //            DataView dvCacheCapProduct;
        //            string strProductTypeFilter = ConfigurationManager.AppSettings.Get("vchCAPProductType");
        //            dvCacheCapProduct = dsCache.Tables[0].DefaultView;
        //            dvCacheCapProduct.RowFilter = "vchProductType = '" + strProductTypeFilter + "'"; // order by vchProductCode"

        //            DataSet dsCacheCapProduct = new DataSet();
        //            dsCacheCapProduct.Tables.Add(dvCacheCapProduct.ToTable());
        //            return dsCacheCapProduct;
        //        }
        //        // dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("CapProductCode_Get")
        //        // Return DataAccess.DataAccess.GetDataSet(dbcom)
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public DataSet GetCappingShape(DetailInfo obj)
        //    {
        //        try
        //        {
        //            SqlCacheDependency SqlDep;
        //            DataSet dsCache = (DataSet)HttpRuntime.Cache["CacheBCDShapeCode"];
        //            if (dsCache == null)
        //            {
        //                SqlDep = new SqlCacheDependency("NDSCaching", "shapemaster");
        //                dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BCDShapeCode_Cache_Get_Beam");
        //                dsCache = DataAccess.DataAccess.GetDataSet(dbcom);
        //                HttpRuntime.Cache.Insert("CacheBCDShapeCode", dsCache, SqlDep);
        //            }

        //            DataView dvBCDShapeCode;
        //            string strBCDShapeCodeFilter = ConfigurationManager.AppSettings.Get("vchCAPShapeGroup");
        //            dvBCDShapeCode = dsCache.Tables[0].DefaultView;
        //            dvBCDShapeCode.RowFilter = "chrShapeCode = '" + strBCDShapeCodeFilter + "' "; // ORDER BY chrShapeCode ASC"

        //            DataSet dsBCDShapeCode = new DataSet();
        //            dsBCDShapeCode.Tables.Add(dvBCDShapeCode.ToTable());
        //            return dsBCDShapeCode;
        //        }
        //        // dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BCDShapeCode_Get_Capping")
        //        // Return DataAccess.DataAccess.GetDataSet(dbcom)
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public DataSet GetTcPrmFrmPrdCodeMaster(DetailInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BCDTcPrmFrmPrdMaster_Get");
        //            dynamicParameters.Add("@intProductCodeId", obj.intProductCodeId);
        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public DataSet GetTCPrmFrmShapeParm(DetailInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BCDTcPrmFrmShapeParm_Get");
        //            dynamicParameters.Add("@intShapeCodeId", obj.intShapeCodeId);
        //            dynamicParameters.Add("@tntParamSetNumber", obj.tntParamSetNumber);
        //            dynamicParameters.Add("@intShapeTransHeaderId", obj.intShapeTransHeaderId);
        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public DataSet GetTCPrmFrmBeamParm(DetailInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BCDTcPrmBeamFrmParm_Get");
        //            dynamicParameters.Add("@intProductCodeId", obj.intProductCodeId);
        //            dynamicParameters.Add("@tntParamSetNumber", obj.tntParamSetNumber);
        //            dynamicParameters.Add("@intShapeCodeId", obj.intShapeCodeId);
        //            dynamicParameters.Add("@intShapeTransHeaderId", obj.intShapeTransHeaderId);
        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }
        //    public DataSet GetTCPrmFrmParameterSet(DetailInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BCDTcPrmFrmParamSet_Get");
        //            dynamicParameters.Add("@tntParamSetNumber", obj.tntParamSetNumber);
        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        public int GetRoundOffValue(DetailInfo obj)
        {
            int strReturnValue;
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@sitProductTypeId", obj.sitProductTypeId);
                    dynamicParameters.Add("@intStructureElementTypeId", obj.intStructureElementTypeId);
                    dynamicParameters.Add("@intConsignmentType", obj.intConsignmentType);
                    dynamicParameters.Add("@decMWDiameter", obj.numDiameter);
                    dynamicParameters.Add("@numMWProductionLength", obj.numProductionMWLength);
                    strReturnValue = sqlConnection.QueryFirstOrDefault<int>(SystemConstants.BCDRndOff_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                    return strReturnValue;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return (int)Interaction.IIf(strReturnValue.Equals(""), -1, strReturnValue);
        }

        public DataSet GetPageInfoFromGroupMarkID(DetailInfo obj)
        {
            try
            {
                DataSet ds = new DataSet();
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intGroupMarkId", obj.intGroupMarkId);

                    ds = (DataSet)sqlConnection.Query<DataSet>(SystemConstants.BeamGroupMarkingDetails_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                    return ds;

                }



            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //    public DataSet GetProductType()
        //    {
        //        try
        //        {
        //            SqlCacheDependency SqlDep;
        //            DataSet dsCache = (DataSet)HttpRuntime.Cache["CacheWBSProduct"];
        //            if (dsCache == null)
        //            {
        //                SqlDep = new SqlCacheDependency("NDSCaching", "producttypemaster");
        //                dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("WBSProductType_Get");
        //                dsCache = DataAccess.DataAccess.GetDataSet(dbcom);
        //                HttpRuntime.Cache.Insert("CacheWBSProduct", dsCache, SqlDep);
        //            }
        //            return dsCache;
        //        }
        //        // dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("WBSProductType_Get")
        //        // Return DataAccess.DataAccess.GetDataSet(dbcom)
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    // Public s Function GetShapeParamByShapeTrans(ByVal obj As DetailInfo) As DataSet
        //    // Try
        //    // dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ShapeCode_LegHook_Get")
        //    // dynamicParameters.Add("@intShapeTransHeaderId", obj.intShapeTransHeaderId)
        //    // Return DataAccess.DataAccess.GetDataSet(dbcom)
        //    // Catch ex As Exception
        //    // Throw ex
        //    // End Try
        //    // End Function

        //    public DataSet GetShapeParamByShapeTrans(DetailInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BeamShapeCodevalues_Get");
        //            dynamicParameters.Add("@intShapeTransHeaderId", obj.intShapeTransHeaderId);
        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        public List<GetWBSElementByIdDto>GetWBSElementByGroupMarkId(DetailInfo obj)
        {
            try
            {
                DataSet dataSet = new DataSet();
                IEnumerable<GetWBSElementByIdDto> getWBSElements;
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intGroupMarkId", obj.intGroupMarkId);

                    getWBSElements = sqlConnection.Query<GetWBSElementByIdDto>(SystemConstants.BCDWBSElementByGroupMarkId_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                    sqlConnection.Close();
                    return getWBSElements.ToList();

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<DrainParamDepthValues_GetNewDto> DrainParamDepthValues_Get(DetailInfo obj)
        {
            IEnumerable<DrainParamDepthValues_GetNewDto> drainParamDepths;

            List<DrainParamDepthValues_GetNewDto> drainParamDepths_list = new List<DrainParamDepthValues_GetNewDto>();
            DataSet ds = new DataSet();
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intGroupMarkID", obj.intGroupMarkId);
                    dynamicParameters.Add("@intParamSet", obj.tntParamSetNumber);
                    drainParamDepths = sqlConnection.Query<DrainParamDepthValues_GetNewDto>(SystemConstants.DrainParamDepthValues_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

                    drainParamDepths_list = drainParamDepths.ToList();

                    SqlCommand cmd = new SqlCommand(SystemConstants.DrainParamDepthValues_Get, sqlConnection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@intGroupMarkID", obj.intGroupMarkId));
                    cmd.Parameters.Add(new SqlParameter("@intParamSet", obj.tntParamSetNumber));


                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                    adapter.Fill(ds);

                    int Count = 0;

                    string TempLayer = "";

                    drainParamDepths_list[0].vchDrainLayer = new List<string>();

                    if (ds != null && ds.Tables.Count != 0)
                    {
                        foreach (DataRowView drvBeam in ds.Tables[1].DefaultView)
                        {
                            TempLayer = (drvBeam["vchDrainLayer"]).ToString();

                            drainParamDepths_list[0].vchDrainLayer.Add(TempLayer);


                        }
                    }


                    return drainParamDepths_list;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //    public DataSet DrainProjectParamDetails_Get(DetailInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ProjectParameterDrainDetails_Get");
        //            dynamicParameters.Add("@tntParamSetNumber", obj.tntParamSetNumber);
        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public int CappingProductSelectbyPrdCode_Get(DetailInfo obj)
        //    {
        //        string strReturnValue = "";
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BCDCappingProductbyPrdCode_Get");
        //            dynamicParameters.Add("@intProductCodeId",obj.intProductCodeId);
        //            dynamicParameters.Add("@tntParamSetNumber", obj.tntParamSetNumber);
        //            strReturnValue = DataAccess.DataAccess.GetScalar(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        return Interaction.IIf(strReturnValue.Equals(""), -1, (int)strReturnValue);
        //    }

        //    public DataSet GetParentProductMarkid(DetailInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BCDParentProductMarkId_Get");
        //            dynamicParameters.Add("@intstructureMarkid",obj.intStructureMarkId);
        //            dynamicParameters.Add("@vchParentStructureElement", obj.vchStructureElementType);
        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }
        //    public DataSet GetParentStructureMarkid(DetailInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BCDParentStructureMarkId_Get");
        //            dynamicParameters.Add("@intSEDetailingId",obj.SEDetailingId);
        //            dynamicParameters.Add("@vchParentStructureElement", obj.vchStructureElementType);
        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public DataSet GetCABProductCodeID(DetailInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("CABProductCodeMaster_Get");
        //            dynamicParameters.Add("@chrGradeType", obj.chrGradeType);
        //            dynamicParameters.Add("@intDiameter", obj.intDiameter);
        //            dynamicParameters.Add("@bitCouplerIndicator", DbType.Byte, obj.bitCouplerIndicator);
        //            dynamicParameters.Add("@vchCouplerType", obj.vchCouplerType1);

        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        public List<GetCABDetailsDto> GetCABDetails(DetailInfo obj)
        {
            try
            {
                IEnumerable<GetCABDetailsDto>getCABDetails;
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intSEDetailingId", obj.SEDetailingId);
                    dynamicParameters.Add("@intStructureMarkId", obj.intStructureMarkId);
                    getCABDetails = sqlConnection.Query<GetCABDetailsDto>(SystemConstants.CAB_ProductMarkingDetails_GET, dynamicParameters, commandType: CommandType.StoredProcedure);
                    sqlConnection.Close();
                    return getCABDetails.ToList();

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #region Transport Check

        public DataSet GetTcTransChTCMaster(DetailInfo obj)
        {
            try
            {
                DataSet ds = new DataSet();
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    SqlCommand cmd = new SqlCommand(SystemConstants.BCDTcTransChTCMaster_Get, sqlConnection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@tntParamSetNumber", obj.tntParamSetNumber));


                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(ds);


                    //ds = (DataSet)sqlConnection.Query<DataSet>(SystemConstants.BCDTcTransChTCMaster_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                    return ds;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion

        #endregion
         
          #region Update

        //    /// <summary>
        //    ///     ''' 
        //    ///     ''' </summary>
        //    ///     ''' <param name="obj"></param>
        //    ///     ''' <returns></returns>
        //    ///     ''' <remarks></remarks>
        //    ///     '''
        //    public int UpdateStructureMarking(DetailInfo obj)
        //    {
        //        try
        //        {
        //            int intReturnValue;
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BCDStructureMarking_Update");

        //            dynamicParameters.Add("@intStructureMarkId", obj.intStructureMarkId);
        //            // dynamicParameters.Add("@tntStructureRevNo",obj.tntStructureRevNo)
        //            // dynamicParameters.Add("@intDetailingHeaderId ", obj.intDetailingHeaderId)
        //            // dynamicParameters.Add("@intGroupMarkId", obj.intGroupMarkId)
        //            // dynamicParameters.Add("@tntGroupRevNo",obj.tntGroupRevNo)
        //            // dynamicParameters.Add("@intConsignmentType", obj.intConsignmentType)
        //            dynamicParameters.Add("@vchStructureMarkingName", obj.vchStructureMarkingName);


        //            dynamicParameters.Add("@tntParamSetNumber",obj.tntParamSetNumber);
        //            dynamicParameters.Add("@intMemberQty", obj.intMemberQty);
        //            dynamicParameters.Add("@bitSimilarStructure",obj.bitSimilarStructure);
        //            dynamicParameters.Add("@vchSimilarTo", obj.vchSimilarTo);
        //            dynamicParameters.Add("@numBeamWidth", obj.numBeamWidth);
        //            dynamicParameters.Add("@numBeamDepth",obj.numBeamDepth);
        //            dynamicParameters.Add("@numBeamSlope",obj.numBeamSlope);
        //            dynamicParameters.Add("@numCageWidth",obj.numCageWidth);
        //            dynamicParameters.Add("@numCageDepth",obj.numCageDepth);
        //            dynamicParameters.Add("@numCageSlope",obj.numCageSlope);
        //            dynamicParameters.Add("@intClearSpan", obj.intClearSpan);
        //            dynamicParameters.Add("@intTotalStirrups", obj.intTotalStirrups);
        //            dynamicParameters.Add("@intBeamProductCodeId", obj.intBeamProductCodeId);
        //            dynamicParameters.Add("@intBeamShapeId", obj.intBeamShapeId);
        //            dynamicParameters.Add("@bitIsCapping",obj.bitIsCapping);
        //            dynamicParameters.Add("@intCappingProductCodeId", obj.intCappingProductCodeId);
        //            dynamicParameters.Add("@intCappingShapeCodeId", obj.intCappingShapeCodeId);
        //            // dynamicParameters.Add("@numArea",obj.numArea)
        //            // dynamicParameters.Add("@intTotalQty", obj.intTotalQty)
        //            // dynamicParameters.Add("@intTotalBend", obj.intTotalBend)
        //            // dynamicParameters.Add("@numTheoreticTonnage",obj.numTheoreticTonnage)
        //            // dynamicParameters.Add("@numNetTonnage",obj.numNetTonnage)
        //            dynamicParameters.Add("@bitBendingCheck",obj.bitBendingCheck);
        //            dynamicParameters.Add("@bitCoat",obj.bitCoat);
        //            dynamicParameters.Add("@bitMachineCheck",obj.bitMachineCheck);
        //            dynamicParameters.Add("@bitTransportCheck",obj.bitTransportCheck);
        //            // dynamicParameters.Add("@vchDrawingReference", obj.vchDrawingReference)
        //            // dynamicParameters.Add("@chrDrawingVersion", DbType.StringFixedLength, obj.chrDrawingVersion)
        //            // dynamicParameters.Add("@vchDrawingRemarks", obj.vchDrawingRemarks)
        //            // dynamicParameters.Add("@tntStatusId",obj.tntStatusId)
        //            dynamicParameters.Add("@intShapeTransHeaderId", obj.intShapeTransHeaderId);
        //            dynamicParameters.Add("@vchTactonConfigurationState", obj.vchTactonConfigurationState);
        //            dynamicParameters.Add("@intParentStructureid", obj.ParentStructureId);
        //            dynamicParameters.Add("@intSAPMaterialcodeId",obj.SAPMAterialCodeid);
        //            dynamicParameters.Add("@chProduceIndicator", obj.ProduceIndicator);
        //            dynamicParameters.Add("@intPinSize", obj.sitPinSize);
        //            dynamicParameters.Add("@intuserID", obj.intUserid);
        //            intReturnValue = DataAccess.DataAccess.ExecuteNonQuery(dbcom);
        //            return intReturnValue;
        //        }

        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public int UpdateBeamCageCAB(DetailInfo obj)
        //    {
        //        try
        //        {
        //            int intReturnValue;
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BCDProductCABDetails_Update");
        //            dynamicParameters.Add("@intCABId", obj.intCABId);

        //            dynamicParameters.Add("@vchBarMark", obj.vchBarMark);
        //            dynamicParameters.Add("@vchGDia", obj.vchGDia);
        //            dynamicParameters.Add("@intNoOfMBars", obj.intNoOfMBars);
        //            dynamicParameters.Add("@intNoEach", obj.intNoEach);
        //            dynamicParameters.Add("@intTotalQty", obj.intTotalQty);
        //            dynamicParameters.Add("@intShapeId", obj.intShapeId);
        //            dynamicParameters.Add("@sitPinSize",obj.sitPinSize);
        //            dynamicParameters.Add("@numCutLength",obj.numCutLength);
        //            dynamicParameters.Add("@numInvoiceLength",obj.numInvoiceLength);
        //            dynamicParameters.Add("@numCutWeight",obj.numCutWeight);
        //            dynamicParameters.Add("@numInvoiceWeight",obj.numInvoiceWeight);
        //            dynamicParameters.Add("@intBBSPage", obj.intBBSPage);
        //            dynamicParameters.Add("@vchRemarks", obj.vchRemarks);

        //            intReturnValue = DataAccess.DataAccess.ExecuteNonQuery(dbcom);
        //            return intReturnValue;
        //        }

        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public int UpdateBeamCageAccess(DetailInfo obj)
        //    {
        //        int intReturnValue;
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BCDProductAccessorDetails_Update");
        //            dynamicParameters.Add("@intAccessoriesId", obj.intAccessoriesId);

        //            dynamicParameters.Add("@intItemNo", obj.intItemNo);
        //            dynamicParameters.Add("@vchMark", obj.vchMark);
        //            dynamicParameters.Add("@vchProductCode", obj.vchProductCode);
        //            dynamicParameters.Add("@intQty", obj.intQty);
        //            dynamicParameters.Add("@vchUOM", obj.vchUOM);
        //            dynamicParameters.Add("@numWeight",obj.numWeight);
        //            dynamicParameters.Add("@vchBillType", obj.vchBillType);
        //            dynamicParameters.Add("@vchShape", obj.vchShape);
        //            dynamicParameters.Add("@intParentProductMarkid",obj.ParentProductId);

        //            intReturnValue = DataAccess.DataAccess.ExecuteNonQuery(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        return intReturnValue;
        //    }

        #endregion
         
          #region Insert

        //    /// <summary>
        //    ///     ''' 
        //    ///     ''' </summary>
        //    ///     ''' <param name="obj"></param>
        //    ///     ''' <returns></returns>
        //    ///     ''' <remarks></remarks>
        //    ///     '''
        //    public int InsertStructureMarking(DetailInfo obj)
        //    {
        //        int intReturnValue;
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BCDStructureMarking_Insert");
        //            // dynamicParameters.Add("@tntStructureRevNo",obj.tntStructureRevNo)
        //            // dynamicParameters.Add("@intDetailingHeaderId ", obj.intDetailingHeaderId)
        //            dynamicParameters.Add("@intSEDetailingId", obj.SEDetailingId);  // ' modfied here 
        //                                                                                                                   // dynamicParameters.Add("@tntGroupRevNo",obj.tntGroupRevNo)
        //                                                                                                                   // dynamicParameters.Add("@intConsignmentType", obj.intConsignmentType)
        //            dynamicParameters.Add("@vchStructureMarkingName", obj.vchStructureMarkingName);

        //            dynamicParameters.Add("@tntParamSetNumber",obj.tntParamSetNumber);
        //            dynamicParameters.Add("@intMemberQty", obj.intMemberQty);
        //            dynamicParameters.Add("@bitSimilarStructure",obj.bitSimilarStructure);
        //            dynamicParameters.Add("@vchSimilarTo", obj.vchSimilarTo);
        //            dynamicParameters.Add("@numBeamWidth", obj.numBeamWidth);
        //            dynamicParameters.Add("@numBeamDepth",obj.numBeamDepth);
        //            dynamicParameters.Add("@numBeamSlope",obj.numBeamSlope);
        //            // dynamicParameters.Add("@numCageWidth",obj.numCageWidth)
        //            // dynamicParameters.Add("@numCageDepth",obj.numCageDepth)
        //            dynamicParameters.Add("@numCageSlope",obj.numCageSlope);
        //            dynamicParameters.Add("@intClearSpan", obj.intClearSpan);
        //            dynamicParameters.Add("@intTotalStirrups", obj.intTotalStirrups);
        //            dynamicParameters.Add("@intBeamProductCodeId", obj.intBeamProductCodeId);
        //            dynamicParameters.Add("@intBeamShapeId", obj.intBeamShapeId);
        //            dynamicParameters.Add("@bitIsCapping",obj.bitIsCapping);
        //            dynamicParameters.Add("@intCappingProductCodeId", obj.intCappingProductCodeId);
        //            dynamicParameters.Add("@intCappingShapeCodeId", obj.intCappingShapeCodeId);
        //            // dynamicParameters.Add("@numArea",obj.numArea)
        //            // dynamicParameters.Add("@intTotalQty", obj.intTotalQty)
        //            // dynamicParameters.Add("@intTotalBend", obj.intTotalBend)
        //            // dynamicParameters.Add("@numTheoreticTonnage",obj.numTheoreticTonnage)
        //            // dynamicParameters.Add("@numNetTonnage",obj.numNetTonnage)
        //            dynamicParameters.Add("@bitBendingCheck",obj.bitBendingCheck);
        //            dynamicParameters.Add("@bitCoat",obj.bitCoat);

        //            dynamicParameters.Add("@bitMachineCheck",obj.bitMachineCheck);
        //            dynamicParameters.Add("@bitTransportCheck",obj.bitTransportCheck);
        //            // dynamicParameters.Add("@vchDrawingReference", obj.vchDrawingReference)
        //            // dynamicParameters.Add("@chrDrawingVersion", DbType.StringFixedLength, obj.chrDrawingVersion)
        //            // dynamicParameters.Add("@vchDrawingRemarks", obj.vchDrawingRemarks)
        //            // dynamicParameters.Add("@tntStatusId",obj.tntStatusId)
        //            dynamicParameters.Add("@intShapeTransHeaderId", obj.intShapeTransHeaderId);
        //            dynamicParameters.Add("@vchTactonConfigurationState", obj.vchTactonConfigurationState);
        //            dynamicParameters.Add("@intParentStructureId", obj.ParentStructureId);
        //            dynamicParameters.Add("@intSAPMaterialcodeId",obj.SAPMAterialCodeid);
        //            dynamicParameters.Add("@intPinSize", obj.intPinSizeId);
        //            dynamicParameters.Add("@chProduceIndicator", obj.ProduceIndicator);
        //            dynamicParameters.Add("@intuserID", obj.intUserid);
        //            intReturnValue = System.Convert.ToInt32(DataAccess.DataAccess.GetScalar(dbcom));
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        return intReturnValue;
        //    }

        //    public int InsertBeamCageCAB(DetailInfo obj)
        //    {
        //        int intReturnValue;
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BCDProductCABDetails_Insert");

        //            dynamicParameters.Add("@intStructureMarkId", obj.intStructureMarkId);
        //            dynamicParameters.Add("@intSEDetailingId", obj.SEDetailingId);

        //            dynamicParameters.Add("@vchBarMark", obj.vchBarMark);
        //            dynamicParameters.Add("@vchGDia", obj.vchGDia);
        //            dynamicParameters.Add("@intNoOfMBars", obj.intNoOfMBars);
        //            dynamicParameters.Add("@intNoEach", obj.intNoEach);
        //            dynamicParameters.Add("@intTotalQty", obj.intTotalQty);
        //            dynamicParameters.Add("@intShapeId", obj.intShapeId);
        //            dynamicParameters.Add("@sitPinSize",obj.sitPinSize);
        //            dynamicParameters.Add("@numCutLength",obj.numCutLength);
        //            dynamicParameters.Add("@numInvoiceLength",obj.numInvoiceLength);
        //            dynamicParameters.Add("@numCutWeight",obj.numCutWeight);
        //            dynamicParameters.Add("@numInvoiceWeight",obj.numInvoiceWeight);
        //            dynamicParameters.Add("@intBBSPage", obj.intBBSPage);
        //            dynamicParameters.Add("@vchRemarks", obj.vchRemarks);

        //            intReturnValue = DataAccess.DataAccess.ExecuteNonQuery(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        return intReturnValue;
        //    }

        //    public int InsertBeamCageAccess(DetailInfo obj)
        //    {
        //        int intReturnValue;
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BCDProductAccessorDetails_Insert");

        //            dynamicParameters.Add("@intSEDetailingId", obj.SEDetailingId);
        //            dynamicParameters.Add("@intStructureMarkId", obj.intStructureMarkId);

        //            dynamicParameters.Add("@intItemNo", obj.intItemNo);
        //            dynamicParameters.Add("@vchMark", obj.vchMark);
        //            dynamicParameters.Add("@vchProductCode", obj.vchProductCode);
        //            dynamicParameters.Add("@intQty", obj.intQty);
        //            dynamicParameters.Add("@vchUOM", obj.vchUOM);
        //            dynamicParameters.Add("@numWeight",obj.numWeight);
        //            dynamicParameters.Add("@vchBillType", obj.vchBillType);
        //            dynamicParameters.Add("@vchShape", obj.vchShape);
        //            dynamicParameters.Add("@intParentProductMarkid",obj.ParentProductId);
        //            intReturnValue = DataAccess.DataAccess.ExecuteNonQuery(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        return intReturnValue;
        //    }

        //    public int InsertAccessories(DetailInfo obj)
        //    {
        //        int intReturnValue;
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ACC_ProductMarkingDetails_Insert");

        //            dynamicParameters.Add("@vchAccProductMarkingName", obj.vchProductMarkingName);
        //            dynamicParameters.Add("@intQty", obj.intQty);
        //            dynamicParameters.Add("@intStructureMarkID", obj.intStructureMarkId);
        //            dynamicParameters.Add("@intSEDetailing", obj.SEDetailingId);
        //            dynamicParameters.Add("@intNoOfPieces", obj.intNoEach);
        //            dynamicParameters.Add("@vchSAPCode", obj.SAPMaterialCode);
        //            dynamicParameters.Add("@vchCABProductMarkName", obj.vchBarMark);
        //            dynamicParameters.Add("@vchStructureMarkName", obj.vchStructureMarkingName);
        //            dynamicParameters.Add("@vchProductType", obj.vchStructureElementType);
        //            dynamicParameters.Add("@intGroupMarkId", obj.intGroupMarkId);
        //            dynamicParameters.Add("@bitIscoupler",obj.IsbitCoupler);
        //            dynamicParameters.Add("@numExternalLength", obj.numExternalLength);
        //            dynamicParameters.Add("@numExternalWidth", obj.numExternalWidth);
        //            dynamicParameters.Add("@numExternalHeight", obj.numExternalHeight);
        //            dynamicParameters.Add("@numInvoiceWeightPerPC", DbType.Double, obj.numInvoiceWeightPerPC);
        //            dynamicParameters.Add("@numLength", obj.numLength);
        //            intReturnValue = DataAccess.DataAccess.ExecuteNonQuery(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        return intReturnValue;
        //    }

        public int InsertGroupMarking(DetailInfo obj)
        {
            int intReturnValue;
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intGroupMarkId", obj.intGroupMarkId);
                    dynamicParameters.Add("@tntGroupRevNo", obj.tntGroupRevNo);
                    dynamicParameters.Add("@intProjectId", obj.intProjectId);
                    dynamicParameters.Add("@intWBSTypeId", obj.intWBSTypeId);
                    dynamicParameters.Add("@intStructureElementTypeId", obj.intStructureElementTypeId);
                    dynamicParameters.Add("@sitProductTypeId", obj.sitProductTypeId);
                    dynamicParameters.Add("@intConsignmentType", obj.intConsignmentType);
                    dynamicParameters.Add("@vchGroupMarkingName", obj.vchGroupMarkingName);
                    dynamicParameters.Add("@tntParamSetNumber", obj.tntParamSetNumber);
                    dynamicParameters.Add("@vchRemarks", obj.vchRemarks);
                    dynamicParameters.Add("@vchCopyFrom", obj.vchCopyFrom);
                    dynamicParameters.Add("@vchDrawingReference", obj.vchDrawingReference);
                    dynamicParameters.Add("@chrDrawingVersion", obj.chrDrawingVersion);
                    dynamicParameters.Add("@vchDrawingRemarks", obj.vchDrawingRemarks);
                    dynamicParameters.Add("@tntStatusId", obj.tntStatusId);
                    dynamicParameters.Add("@bitParentFlag", obj.ParentFlag);
                    dynamicParameters.Add("@intCreatedUId", obj.intUserid);
                    intReturnValue = sqlConnection.QueryFirstOrDefault<int>(SystemConstants.BCDGroupMarking_Insert, dynamicParameters, commandType: CommandType.StoredProcedure);

                    return intReturnValue;

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return intReturnValue;
        }

        public int InsertWbsDetailing(DetailInfo obj)
        {
            int intReturnValue;
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intGroupMarkId", obj.intGroupMarkId);
                    dynamicParameters.Add("@tntGroupRevNo", obj.tntGroupRevNo);
                    dynamicParameters.Add("@intWBSElementId", obj.intWBSElementId);
                    dynamicParameters.Add("@vchDrawingReference", obj.vchDrawingReference);
                    dynamicParameters.Add("@chrDrawingVersion", obj.chrDrawingVersion);

                    dynamicParameters.Add("@vchDrawingRemarks", obj.vchDrawingRemarks);
                    dynamicParameters.Add("@tntStatusId", obj.tntStatusId);
                    intReturnValue = sqlConnection.QueryFirstOrDefault<int>(SystemConstants.BCDWBSDetails_Insert, dynamicParameters, commandType: CommandType.StoredProcedure);

                    return intReturnValue;

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return intReturnValue;
        }

        //public int InsertProductMarking(DetailInfo obj)
        //{
        //    int intReturnValue;
        //    try
        //    {
        //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ProductMarkingDetails_Insert");

        //        // db.AddInParameter("@intGroupMarkId", obj.intGroupMarkId)
        //        db.AddInParameter("@intProjectID", obj.intProjectId);
        //        db.AddInParameter("@intStructureMarkId", obj.intStructureMarkId);
        //        db.AddInParameter("@vchProductMarkingName", obj.vchProductMarkingName);
        //        db.AddInParameter("@numCageWidth", obj.numCageWidth);
        //        db.AddInParameter("@numCageDepth", obj.numCageDepth);
        //        db.AddInParameter("@numCageSlope", obj.numCageSlope);
        //        db.AddInParameter("@numBeamWidth", obj.numBeamWidth);
        //        db.AddInParameter("@numBeamDepth", obj.numBeamDepth);
        //        db.AddInParameter("@numInvoiceMWLength", obj.numInvoiceMWLength);
        //        db.AddInParameter("@numInvoiceCWLength", obj.numInvoiceCWLength);
        //        db.AddInParameter("@intShapeCodeId", obj.intShapeCodeId);

        //        db.AddInParameter("@intProductCode", obj.intProductCode);
        //        db.AddInParameter("@intMemberQty", obj.intMemberQty);
        //        db.AddInParameter("@intCutQty", obj.intCutQty);

        //        db.AddInParameter("@intInvoiceMWQty", obj.intInvoiceMWQty);
        //        db.AddInParameter("@intInvoiceCWQty", obj.intInvoiceCWQty);
        //        db.AddInParameter("@numInvoiceMWWeight", obj.numInvoiceMWWeight);
        //        db.AddInParameter("@numInvoiceCWWeight", obj.numInvoiceCWWeight);
        //        db.AddInParameter("@numTheoraticalWeight", obj.numTheoraticalWeight);
        //        db.AddInParameter("@intMO1", obj.intMO1);
        //        db.AddInParameter("@intMO2", obj.intMO2);
        //        db.AddInParameter("@intCO1", obj.intCO1);
        //        db.AddInParameter("@intCO2", obj.intCO2);


        //        db.AddInParameter("@intProductionMO1", obj.intProductionMO1);
        //        db.AddInParameter("@intProductionMO2", obj.intProductionMO2);
        //        db.AddInParameter("@intProductionCO1", obj.intProductionCO1);
        //        db.AddInParameter("@intProductionCO2", obj.intProductionCO2);

        //        db.AddInParameter("@numProductionMWLength", obj.numProductionMWLength);
        //        db.AddInParameter("@numProductionCWLength", obj.numProductionCWLength);
        //        db.AddInParameter("@intProductionCWQty", obj.intProductionCWQty);
        //        db.AddInParameter("@bitBendIndicator", obj.bitBendIndicator);
        //        db.AddInParameter("@bitCoatingIndicator", obj.bitCoatingIndicator);
        //        db.AddInParameter("@numInvoiceArea", obj.numInvoiceArea);
        //        db.AddInParameter("@numInvoiceWeight", obj.numInvoiceWeight);
        //        db.AddInParameter("@numMWSpacing", obj.numMWSpacing);
        //        db.AddInParameter("@numCWSpacing", obj.numCWSpacing);
        //        db.AddInParameter("@vchTransportCheckResult", obj.vchTransportCheckResult);
        //        db.AddInParameter("@ParamValues", obj.ParamValues);
        //        db.AddInParameter("@BendingPos", obj.BendingPos);
        //        db.AddInParameter("@intParentProductId", obj.ParentProductId);
        //        db.AddInParameter("@sitPinSizeId", obj.sitPinSize);
        //        db.AddInParameter("@bitMachineCheckIndicator", obj.bitMachineCheckIndicator);
        //        db.AddInParameter("@vchSafeState", obj.vchTactonConfigurationState);
        //        db.AddInParameter("@intuserID", obj.intUserid);
        //        db.AddInParameter("@vchMWBVBSString", obj.vchMWBVBSString);
        //        db.AddInParameter("@vchCWBVBSString", obj.vchCWBVBSString);

        //        intReturnValue = DataAccess.DataAccess.ExecuteNonQuery(dbcom);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return intReturnValue;
        //}

        public int DeleteWbsDetailing(DetailInfo obj)
        {
            int intReturnValue;
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intGroupMarkId", obj.intGroupMarkId);

                    intReturnValue = sqlConnection.QueryFirstOrDefault<int>(SystemConstants.BCDWBSList_Delete, dynamicParameters, commandType: CommandType.StoredProcedure);

                    sqlConnection.Close();
                    return intReturnValue;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return intReturnValue;
        }

        //public string[] GetGroupMarkingByID(DetailInfo obj)
        //{
        //    string[] strReturnValue = new string[4];
        //    try
        //    {
        //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BCDGroupMarkNameById_Get");
        //        dynamicParameters.Add("@intGroupMarkId", obj.intGroupMarkId);

        //        DataAccess.DataAccess.db.AddOutParameter("@vchGroupMarkingName", 100);
        //        DataAccess.DataAccess.db.AddOutParameter("@tntGroupRevno", 0);
        //        DataAccess.DataAccess.db.AddOutParameter("@vchCopyFrom", 200);
        //        DataAccess.DataAccess.db.AddOutParameter("@vchRemarks", 200);
        //        DataAccess.DataAccess.GetScalar(dbcom);

        //        strReturnValue[0] = Interaction.IIf(Information.IsDBNull(dbcom.Parameters["@vchGroupMarkingName"].Value), "", dbcom.Parameters["@vchGroupMarkingName"].Value);
        //        strReturnValue[1] = Interaction.IIf(Information.IsDBNull(dbcom.Parameters["@tntGroupRevno"].Value), "0", dbcom.Parameters["@tntGroupRevno"].Value);
        //        strReturnValue[2] = Interaction.IIf(Information.IsDBNull(dbcom.Parameters["@vchCopyFrom"].Value), "N/A", dbcom.Parameters["@vchCopyFrom"].Value);
        //        strReturnValue[3] = Interaction.IIf(Information.IsDBNull(dbcom.Parameters["@vchRemarks"].Value), "", dbcom.Parameters["@vchRemarks"].Value);
        //    }
        //    // strReturnValue(3) = IIf(IsDBNull(dbcom.Parameters("@vchRemarks").Value), "", dbcom.Parameters("@vchRemarks").Value)

        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return strReturnValue;
        //}


        //public int InsertBeamPRCDetails(DetailInfo obj)
        //{
        //    int intReturnValue;
        //    try
        //    {
        //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BCDPRCDetails_Insert");

        //        dynamicParameters.Add("@intSEDetailingId", obj.SEDetailingId);  // ' modfied here 
        //        dynamicParameters.Add("@vchStructureMarkingName", obj.vchStructureMarkingName);

        //        dynamicParameters.Add("@numBeamWidth", obj.numBeamWidth);
        //        dynamicParameters.Add("@numBeamDepth", obj.numBeamDepth);
        //        dynamicParameters.Add("@intClearSpan", obj.intClearSpan);

        //        dynamicParameters.Add("@intSAPMaterialcodeId", obj.SAPMAterialCodeid);
        //        dynamicParameters.Add("@bitAssIndicator", obj.bitAssemblyIndicator);
        //        dynamicParameters.Add("@vchRemarks", obj.vchRemarks);

        //        DataAccess.DataAccess.db.AddOutParameter("@intStructureMarkid", obj.intStructureMarkId);
        //        // intReturnValue = CType(DataAccess.DataAccess.GetScalar(dbcom), Int32)

        //        DataAccess.DataAccess.GetScalar(dbcom);
        //        intReturnValue = (dbcom.Parameters["@intStructureMarkid"].Value);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return intReturnValue;
        //}


        #endregion
         
          #region Delete


        ///// <summary>
        /////     ''' 
        /////     ''' </summary>
        /////     ''' <param name="obj"></param>
        /////     ''' <returns></returns>
        /////     ''' <remarks></remarks>
        /////     '''
        //public int DelStructureMarking(DetailInfo obj)
        //{
        //    int intReturnValue;
        //    try
        //    {
        //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BCDStructureMarking_Del");
        //        dynamicParameters.Add("@intStructureMarkId", obj.intStructureMarkId);
        //        intReturnValue = DataAccess.DataAccess.GetScalar(dbcom);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return intReturnValue;
        //}

        //public int DelSWStructureMarking(DetailInfo obj)
        //{
        //    int intReturnValue;
        //    try
        //    {
        //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("SWStructureMarking_Del");
        //        dynamicParameters.Add("@intStructureMarkId", obj.intStructureMarkId);
        //        intReturnValue = DataAccess.DataAccess.GetScalar(dbcom);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return intReturnValue;
        //}

        public int DrainStructureMarking_Del(DetailInfo obj)
        {
            int intReturnValue;
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intStructureMarkId", obj.intStructureMarkId);
                    intReturnValue = sqlConnection.QueryFirstOrDefault<int>(SystemConstants.DrainStructureMarking_Del, dynamicParameters, commandType: CommandType.StoredProcedure);

                    sqlConnection.Close();
                    return intReturnValue;

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return intReturnValue;
        }

        //    public int DelSWProductMarking(DetailInfo obj)
        //    {
        //        int intReturnValue;
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("SWProductMarking_Del");
        //            dynamicParameters.Add("@intMeshProductMarkId", obj.intProductMarkId);
        //            dynamicParameters.Add("@intStructureMarkId", obj.intStructureMarkId);
        //            intReturnValue = DataAccess.DataAccess.GetScalar(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        return intReturnValue;
        //    }

        //    public int DelCABProductMarkingDetails(DetailInfo obj)
        //    {
        //        int intReturnValue;
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("SWProductMarking_Del");
        //            dynamicParameters.Add("@intSEDetailingId", obj.SEDetailingId);
        //            dynamicParameters.Add("@intStructureMarkId", obj.intStructureMarkId);
        //            intReturnValue = DataAccess.DataAccess.GetScalar(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        return intReturnValue;
        //    }

        //    public int DelBeamCageCAB(DetailInfo obj)
        //    {
        //        int intReturnValue;
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BCDBeamCageCAB_Del");
        //            dynamicParameters.Add("@intCABId", obj.intCABId);
        //            intReturnValue = DataAccess.DataAccess.GetScalar(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        return intReturnValue;
        //    }

        //    public int DelBeamCageAccess(DetailInfo obj)
        //    {
        //        int intReturnValue;
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BCDBeamCageAccessories_Del");
        //            dynamicParameters.Add("@intAccProductMarkID", obj.intAccessoriesId);
        //            intReturnValue = DataAccess.DataAccess.GetScalar(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        return intReturnValue;
        //    }

        #endregion
         
          #region Drawing & Version

             #region Get

        //    public DataSet GetGroupMarkingDrawingReference(DetailInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("GroupMarkingDrawingReference_Get");
        //            dynamicParameters.Add("@intgroupmarkingdrawingid", obj.intGroupMarkId);
        //            dynamicParameters.Add("@intProjectId", obj.intProjectId);
        //            dynamicParameters.Add("@intStructureElementTypeId", obj.intStructureElementTypeId);
        //            dynamicParameters.Add("@intProductTypeId", obj.intProductTypeId);

        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }
        //    public DataSet GetWBSDrawingReference(DetailInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("WBSDrawingReference_Get");
        //            dynamicParameters.Add("@intWBSElementId", obj.intWBSElementId);
        //            dynamicParameters.Add("@intProjectId", obj.intProjectId);
        //            dynamicParameters.Add("@intGroupMarkId", obj.intGroupMarkId);
        //            dynamicParameters.Add("@intStructureElementTypeId", obj.intStructureElementTypeId);
        //            dynamicParameters.Add("@intProductTypeId", obj.intProductTypeId);

        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public DataSet GetStructureMarkingDrawingReference(DetailInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("StructureMarkingDrawingReference_Get");
        //            dynamicParameters.Add("@intStructureMarkingId", obj.intStructureMarkId);
        //            dynamicParameters.Add("@intProjectId", obj.intProjectId);
        //            dynamicParameters.Add("@intGroupMarkId", obj.intGroupMarkId);
        //            dynamicParameters.Add("@intStructureElementTypeId", obj.intStructureElementTypeId);
        //            dynamicParameters.Add("@intProductTypeId", obj.intProductTypeId);

        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        #endregion
             
             #region Insert

        //    public int InsertGroupMarkingDrawingHistory(DetailInfo obj)
        //    {
        //        int intReturnValue;
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("GroupMarkingDrawingHistory_Insert");
        //            dynamicParameters.Add("@intGroupMarkId", obj.intGroupMarkId);
        //            dynamicParameters.Add("@intProjectId", obj.intProjectId);
        //            dynamicParameters.Add("@intStructureElementTypeId", obj.intStructureElementTypeId);
        //            dynamicParameters.Add("@intProductTypeId", obj.intProductTypeId);
        //            dynamicParameters.Add("@tntGroupRevNo", obj.tntGroupRevNo);
        //            dynamicParameters.Add("@vchDrawingReference", obj.vchDrawingReference);
        //            dynamicParameters.Add("@chrDrawingVersion", obj.chrDrawingVersion);
        //            dynamicParameters.Add("@vchDrawingRemarks", obj.vchDrawingRemarks);
        //            dynamicParameters.Add("@intUploadedUid", obj.intUploadedUid);

        //            intReturnValue = System.Convert.ToInt32(DataAccess.DataAccess.GetScalar(dbcom));
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        return intReturnValue;
        //    }


        //    public int InsertWBSDrawingHistory(DetailInfo obj)
        //    {
        //        int intReturnValue;
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("WBSDrawingHistory_Insert");
        //            dynamicParameters.Add("@vchWBSElementIds", obj.vchWBSElementIds);
        //            dynamicParameters.Add("@vchUploadedFiles", obj.vchUploadedFiles);
        //            dynamicParameters.Add("@vchVersionNos", obj.vchUploadedFileVersions);
        //            dynamicParameters.Add("@vchFileRemarks", obj.vchFileRemarks);
        //            dynamicParameters.Add("@intUploadedUid", obj.intUploadedUid);
        //            dynamicParameters.Add("@intGroupMarkId", obj.intGroupMarkId);
        //            dynamicParameters.Add("@intProjectId", obj.intProjectId);
        //            dynamicParameters.Add("@intStructureElementTypeId", obj.intStructureElementTypeId);
        //            dynamicParameters.Add("@intProductTypeId", obj.intProductTypeId);

        //            intReturnValue = System.Convert.ToInt32(DataAccess.DataAccess.GetScalar(dbcom));
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        return intReturnValue;
        //    }
        //    public int InsertStructureMarkingDrawingHistory(DetailInfo obj)
        //    {
        //        int intReturnValue;
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("StructureMarkingDrawingHistory_Insert");
        //            dynamicParameters.Add("@vchStructureMarkingIds", obj.vchStructureMarkingIds);
        //            dynamicParameters.Add("@vchStructureRevNos", obj.vchStructureRevNos);
        //            dynamicParameters.Add("@vchVersionNos", obj.vchStructureVersionNos);
        //            dynamicParameters.Add("@vchUploadedFiles", obj.vchSMUploadedFiles);
        //            dynamicParameters.Add("@vchFileRemarks", obj.vchSMFileRemarks);
        //            dynamicParameters.Add("@intUploadedUid", obj.intUploadedUid);
        //            dynamicParameters.Add("@intProjectId", obj.intProjectId);
        //            dynamicParameters.Add("@intGroupMarkId", obj.intGroupMarkId);
        //            dynamicParameters.Add("@intStructureElementTypeId", obj.intStructureElementTypeId);
        //            dynamicParameters.Add("@intProductTypeId", obj.intProductTypeId);

        //            intReturnValue = System.Convert.ToInt32(DataAccess.DataAccess.GetScalar(dbcom));
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        return intReturnValue;
        //    }

        #endregion
             

        #endregion

        #endregion

        #region Column Cage

        //    public int InsertColumnPRCDetails(DetailInfo obj)
        //    {
        //        int intReturnValue;
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ColumnPRCDetails_Insert");

        //            dynamicParameters.Add("@intSEDetailingId", obj.SEDetailingId);  // ' modfied here 
        //            dynamicParameters.Add("@vchStructureMarkingName", obj.vchStructureMarkingName);
        //            dynamicParameters.Add("@intSAPMaterialcodeId",obj.SAPMAterialCodeid);

        //            dynamicParameters.Add("@numColumnWidth",obj.numColumnWidth);
        //            dynamicParameters.Add("@numColumnLength",obj.numColumnLength);
        //            dynamicParameters.Add("@numColumnHeight",obj.numColumnHeight);

        //            dynamicParameters.Add("@bitAssIndicator",obj.bitAssemblyIndicator);
        //            dynamicParameters.Add("@vchRemarks", obj.vchRemarks);
        //            DataAccess.DataAccess.db.AddOutParameter("@intStructureMarkid",obj.intStructureMarkId);

        //            DataAccess.DataAccess.GetScalar(dbcom);
        //            intReturnValue = (dbcom.Parameters["@intStructureMarkid"].Value);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        return intReturnValue;
        //    }
        //    public DataSet GetWBSElementByGroupMarkIdColumn(DetailInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BCDWBSElementByGroupMarkIdColumn_Get");
        //            dynamicParameters.Add("@intGroupMarkId", obj.intGroupMarkId);
        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }
        //    public int GetCLinkProductSelectbyPrdCode(DetailInfo obj)
        //    {
        //        string strReturnValue = "";
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BCDCLinkProductbyPrdCode_Get");
        //            dynamicParameters.Add("@intProductCodeId",obj.intProductCodeId);
        //            dynamicParameters.Add("@tntParamSetNumber", obj.tntParamSetNumber);

        //            strReturnValue = DataAccess.DataAccess.GetScalar(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        return Interaction.IIf(strReturnValue.Equals(""), -1, (int)strReturnValue);
        //    }

        //    public DataSet GetProductTypeColumn(DetailInfo obj)
        //    {
        //        try
        //        {
        //            // Dim SqlDep As SqlCacheDependency
        //            // '''''''''''''''''''''''''''''''''''''''''''''product types
        //            // Dim dsCacheProductTypes As DataSet = CType(HttpRuntime.Cache("CacheProductType"), DataSet)
        //            // If dsCacheProductTypes Is Nothing Then
        //            // SqlDep = New SqlCacheDependency("NDSCaching", "producttypemaster")
        //            // dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ProductCode_CacheProductType_Get")
        //            // dsCacheProductTypes = DataAccess.DataAccess.GetDataSet(dbcom)
        //            // HttpRuntime.Cache.Insert("CacheProductType", dsCacheProductTypes, SqlDep)
        //            // End If
        //            // 'prc
        //            // Dim intPrcProductType As Integer = CInt(dsCacheProductTypes.Tables(0).Rows(1)("sitProducTTypeid"))
        //            // 'msh
        //            // Dim intMshProductType As Integer = CInt(dsCacheProductTypes.Tables(0).Rows(0)("sitProducTTypeid"))
        //            // ''''''''''''''''''''''''''''''''''''''''''''''

        //            // ''''product code for column
        //            // Dim dsCacheProductCodeOthers As DataSet = CType(HttpRuntime.Cache("CacheProductCodeOthers"), DataSet)
        //            // If dsCacheProductCodeOthers Is Nothing Then
        //            // SqlDep = New SqlCacheDependency("NDSCaching", "productcodemaster")
        //            // dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ProductCode_Cache_Get")
        //            // dsCacheProductCodeOthers = DataAccess.DataAccess.GetDataSet(dbcom)
        //            // HttpRuntime.Cache.Insert("CacheProductCodeOthers", dsCacheProductCodeOthers, SqlDep)
        //            // End If

        //            // Dim dvProductCodeOthers As DataView
        //            // Dim strCP As String = ConfigurationManager.AppSettings.Get("strCP")
        //            // Dim strCL As String = ConfigurationManager.AppSettings.Get("strCL")
        //            // dvProductCodeOthers = dsCacheProductCodeOthers.Tables(0).DefaultView
        //            // Dim intProductTypeID As Integer
        //            // If (obj.sitProductTypeId = intPrcProductType) Then
        //            // intProductTypeID = intMshProductType
        //            // Else
        //            // intProductTypeID = obj.sitProductTypeId
        //            // End If
        //            // dvProductCodeOthers.RowFilter = "intStructureElementTypeID = " & obj.intStructureElementTypeId & " AND sitProductTypeID = " & intProductTypeID & " AND (vchProductCode not like '" & strCP & "' and vchproductcode not like '" & strCL & "')"

        //            // Dim dsProductCodeOthers As New DataSet
        //            // dsProductCodeOthers.Tables.Add(dvProductCodeOthers.ToTable())
        //            // Return dsProductCodeOthers

        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ProductCodeColumn_Get");
        //            dynamicParameters.Add("@intStructureElementTypeId", obj.intStructureElementTypeId);
        //            dynamicParameters.Add("@sitProductTypeId", obj.sitProductTypeId);
        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public DataSet GetShapeColumn()
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BCDShapeCode_Get_Column");
        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public DataSet GetShapeCLink()
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BCDShapeCode_Get_Clink");
        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }
        //    public DataSet GetProductTypeCLink()
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ClinkProductCode_Get");
        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        #region ColumnDelete
        //    public int DelStructureMarkingColumn(DetailInfo obj)
        //    {
        //        int intReturnValue;
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ColumnStructureMarking_Del");
        //            dynamicParameters.Add("@intStructureMarkId", obj.intStructureMarkId);
        //            intReturnValue = DataAccess.DataAccess.GetScalar(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        return intReturnValue;
        //    }
        #endregion

        #region ColumnInsert

        //    public int InsertProductMarkingColumn(DetailInfo obj)
        //    {
        //        int intReturnValue;
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ColumnProductMarkingDetails_Insert");

        //            // db.AddInParameter("@intGroupMarkId", obj.intGroupMarkId)
        //            db.AddInParameter("@intProjectID", obj.intProjectId);
        //            db.AddInParameter("@intStructureMarkId", obj.intStructureMarkId);
        //            db.AddInParameter("@vchProductMarkingName", obj.vchProductMarkingName);
        //            db.AddInParameter("@numLinkWidth", obj.numLinkwidth);
        //            db.AddInParameter("@numLinklength", obj.numLinkLength);
        //            db.AddInParameter("@intLinkQty", obj.intLinkQty);  // ' need to modify tolinkQty
        //            db.AddInParameter("@intnoofLinks", obj.intNoofLinks);
        //            db.AddInParameter("@intTotalLinks", obj.intTotalLinks);
        //            db.AddInParameter("@numInvoiceMWLength", obj.numInvoiceMWLength);
        //            db.AddInParameter("@numInvoiceCWLength", obj.numInvoiceCWLength);
        //            db.AddInParameter("@intShapeCodeId", obj.intShapeCodeId);

        //            db.AddInParameter("@intProductCodeId", obj.intProductCode);
        //            db.AddInParameter("@intInvoiceMWQty", obj.intInvoiceMWQty);
        //            db.AddInParameter("@intInvoiceCWQty", obj.intInvoiceCWQty);
        //            db.AddInParameter("@numProductionMWQty",obj.intProductionMWQty);
        //            db.AddInParameter("@numProductionCWQty",obj.intProductionCWQty);
        //            db.AddInParameter("@numInvoiceMWWeight", obj.numInvoiceMWWeight);
        //            db.AddInParameter("@numInvoiceCWWeight", obj.numInvoiceCWWeight);
        //            db.AddInParameter("@numProductionMWWeight", obj.numProductionMWWeight);
        //            db.AddInParameter("@numProductionCWWeight", obj.numProductionCWWeight);
        //            db.AddInParameter("@numInvoiceWeight", obj.numInvoiceWeight);
        //            db.AddInParameter("@numProductionWeight", obj.numProductionWeight);
        //            db.AddInParameter("@numMWSpacing", obj.numMWSpacing);
        //            db.AddInParameter("@numTheoWeight", obj.numTheoreticTonnage);
        //            db.AddInParameter("@numInvoiceArea", obj.numInvoiceArea);
        //            // db.AddInParameter("@numCWSpacing", obj.numCWSpacing)

        //            db.AddInParameter("@intMO1", obj.intMO1);
        //            db.AddInParameter("@intMO2", obj.intMO2);
        //            db.AddInParameter("@intCO1", obj.intCO1);
        //            db.AddInParameter("@intCO2", obj.intCO2);

        //            db.AddInParameter("@intProductionMO1", obj.intProductionMO1);
        //            db.AddInParameter("@intProductionMO2", obj.intProductionMO2);
        //            db.AddInParameter("@intProductionCO1", obj.intProductionCO1);
        //            db.AddInParameter("@intProductionCO2", obj.intProductionCO2);

        //            db.AddInParameter("@numProductionMWLength", obj.numProductionMWLength);
        //            db.AddInParameter("@numProductionCWLength", obj.numProductionCWLength);
        //            db.AddInParameter("@bitBendIndicator", obj.bitBendIndicator);
        //            db.AddInParameter("@bitCoatingIndicator", obj.bitCoatingIndicator);
        //            db.AddInParameter("@intPinSizeId", obj.intPinSizeId);
        //            db.AddInParameter("@bitShiftIndicator", obj.bitShiftIndicator);
        //            // ' need to pass  conversion factor , Generation status,
        //            db.AddInParameter("@chrCalculationIndicator", obj.chrCalculationIndicator);
        //            db.AddInParameter("@bitMachineCheckIndicator", obj.bitMachineCheckIndicator);
        //            db.AddInParameter("@intUserUid", obj.intUserid);
        //            db.AddInParameter("@bitTransportIndicator", obj.bitTransportCheck);
        //            db.AddInParameter("@vchTransportResult", obj.strTransportResult);
        //            db.AddInParameter("@vchBendResult", obj.strBendResult);

        //            db.AddInParameter("@vchParamValues", obj.ParamValues);
        //            db.AddInParameter("@vchBendingPos", obj.BendingPos);
        //            db.AddInParameter("@vchCWSpacing", obj.strCWSpacing);
        //            db.AddInParameter("@intParentProductId", obj.ParentProductId);
        //            db.AddInParameter("@vchSafeState", obj.vchTactonConfigurationState);
        //            db.AddInParameter("@vchMWBVBSString", obj.vchMWBVBSString);
        //            db.AddInParameter("@vchCWBVBSString", obj.vchCWBVBSString);

        //            intReturnValue = DataAccess.DataAccess.ExecuteNonQuery(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        return intReturnValue;
        //    }

        //    public int InsertStructureMarkingColumn(DetailInfo obj)
        //    {
        //        int intReturnValue;
        //        try
        //        {
        //            object SMColLock = new object();
        //            lock (SMColLock)
        //            {
        //                dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ColumnStructureMarking_Insert");

        //                // dynamicParameters.Add("@tntStructureRevNo",obj.tntStructureRevNo)

        //                dynamicParameters.Add("@intSEDetailingId", obj.SEDetailingId);
        //                dynamicParameters.Add("@intConsignmentType", obj.intConsignmentType);
        //                dynamicParameters.Add("@vchStructureMarkingName", obj.vchStructureMarkingName);


        //                dynamicParameters.Add("@tntParamSetNumber",obj.tntParamSetNumber);
        //                dynamicParameters.Add("@intMemberQty", obj.intMemberQty);
        //                dynamicParameters.Add("@bitSimilarStructure",obj.bitSimilarStructure);
        //                dynamicParameters.Add("@vchSimilarTo", obj.vchSimilarTo);
        //                // '''''''''''''''''''''''''''''''''''''' insert from update '''''''''''''''

        //                dynamicParameters.Add("@numcolumnWidth",obj.numColumnWidth);
        //                dynamicParameters.Add("@numColumnLength",obj.numColumnLength);
        //                dynamicParameters.Add("@numColumnHeight",obj.numColumnHeight);
        //                dynamicParameters.Add("@numLinkWidth",obj.numLinkwidth);
        //                dynamicParameters.Add("@numLinkLength",obj.numLinkLength);
        //                dynamicParameters.Add("@intNoofLinks", obj.intNoofLinks);


        //                dynamicParameters.Add("@intRowsAtLength", obj.intRowsAtLen);
        //                dynamicParameters.Add("@intRowsAtWidth", obj.intRowsAtWidth);
        //                dynamicParameters.Add("@intColumnProductCodeId", obj.intProductCodeId);
        //                dynamicParameters.Add("@intColumnShapeId", obj.intShapeCodeId);
        //                dynamicParameters.Add("@bitIsCLink",obj.bitIsCLink);
        //                dynamicParameters.Add("@intCLinkProductCodeIdAtLen", obj.intCLinkProdCodeIdAtlen);
        //                dynamicParameters.Add("@intCLinkProductCodeIdAtWidth", obj.intClinkProdCodeIdAtWidth);

        //                dynamicParameters.Add("@intCLinkShapeCodeIdAtLen", obj.intCLinkShapeCodeIdAtlen);
        //                dynamicParameters.Add("@intCLinkShapeCodeIdAtWidth", obj.intClinkShapeCodeIdAtWidth);

        //                dynamicParameters.Add("@intClinkQtyAtLength", obj.intClinkQtyAtLen);
        //                dynamicParameters.Add("@intClinkQtyAtWidth", obj.intClinkQtyAtWidth);

        //                dynamicParameters.Add("@numArea",obj.numArea);
        //                dynamicParameters.Add("@intTotalQty", obj.intTotalQty);
        //                dynamicParameters.Add("@intTotalBend", obj.intTotalBend);
        //                dynamicParameters.Add("@numTheoreticTonnage",obj.numTheoreticTonnage);
        //                dynamicParameters.Add("@numNetTonnage",obj.numNetTonnage);
        //                dynamicParameters.Add("@bitBendingCheck",obj.bitBendingCheck);
        //                dynamicParameters.Add("@bitCoat",obj.bitCoat);
        //                dynamicParameters.Add("@bitMachineCheck",obj.bitMachineCheckIndicator);
        //                dynamicParameters.Add("@bitTransportCheck",obj.bitTransportCheck);
        //                dynamicParameters.Add("@vchDrawingReference", obj.vchDrawingReference);
        //                dynamicParameters.Add("@chrDrawingVersion", DbType.StringFixedLength, obj.chrDrawingVersion);
        //                dynamicParameters.Add("@vchDrawingRemarks", obj.vchDrawingRemarks);
        //                dynamicParameters.Add("@tntStatusId",obj.tntStatusId);

        //                dynamicParameters.Add("@vchCLOnly",obj.bitCLOnly);
        //                dynamicParameters.Add("@vchWBSElementID", obj.strWBSElementId);
        //                dynamicParameters.Add("@intShapeTransHeaderId", obj.intShapeTransHeaderId);
        //                dynamicParameters.Add("@vchTactonConfigurationState", obj.vchTactonConfigurationState);
        //                dynamicParameters.Add("@intParentStructureid", obj.ParentStructureId);
        //                dynamicParameters.Add("@intSAPMaterialcodeId",obj.SAPMAterialCodeid);
        //                dynamicParameters.Add("@intPinsize", obj.sitPinSize);
        //                dynamicParameters.Add("@chProduceIndicator", obj.ProduceIndicator);
        //                dynamicParameters.Add("@intuserID", obj.intUserid);
        //                intReturnValue = System.Convert.ToInt32(DataAccess.DataAccess.GetScalar(dbcom));
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        return intReturnValue;
        //    }
        #endregion

        #region ColumnUpdate


        //    public int UpdateStructureMarkingColumn(DetailInfo obj)
        //    {
        //        int intReturnValue;
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ColumnStructureMarking_Update");

        //            dynamicParameters.Add("@intStructureMarkId", obj.intStructureMarkId);
        //            // dynamicParameters.Add("@tntStructureRevNo",obj.tntStructureRevNo)
        //            dynamicParameters.Add("@intSEDetailingId", obj.SEDetailingId);
        //            dynamicParameters.Add("@intConsignmentType", obj.intConsignmentType);
        //            dynamicParameters.Add("@vchStructureMarkingName", obj.vchStructureMarkingName);

        //            dynamicParameters.Add("@tntParamSetNumber",obj.tntParamSetNumber);
        //            dynamicParameters.Add("@intMemberQty", obj.intMemberQty);
        //            dynamicParameters.Add("@bitSimilarStructure",obj.bitSimilarStructure);
        //            dynamicParameters.Add("@vchSimilarTo", obj.vchSimilarTo);
        //            dynamicParameters.Add("@numcolumnWidth",obj.numColumnWidth);
        //            dynamicParameters.Add("@numColumnLength",obj.numColumnLength);
        //            dynamicParameters.Add("@numColumnHeight",obj.numColumnHeight);
        //            dynamicParameters.Add("@numLinkWidth",obj.numLinkwidth);
        //            dynamicParameters.Add("@numLinkLength",obj.numLinkLength);
        //            dynamicParameters.Add("@intNoofLinks", obj.intNoofLinks);

        //            dynamicParameters.Add("@intRowsAtLength", obj.intRowsAtLen);
        //            dynamicParameters.Add("@intRowsAtWidth", obj.intRowsAtWidth);
        //            dynamicParameters.Add("@intColumnProductCodeId", obj.intProductCodeId);
        //            dynamicParameters.Add("@intColumnShapeId", obj.intShapeCodeId);
        //            dynamicParameters.Add("@bitIsCLink",obj.bitIsCLink);
        //            dynamicParameters.Add("@intCLinkProductCodeIdAtLen", obj.intCLinkProdCodeIdAtlen);
        //            dynamicParameters.Add("@intCLinkProductCodeIdAtWidth", obj.intClinkProdCodeIdAtWidth);

        //            dynamicParameters.Add("@intCLinkShapeCodeIdAtLen", obj.intCLinkShapeCodeIdAtlen);
        //            dynamicParameters.Add("@intCLinkShapeCodeIdAtWidth", obj.intClinkShapeCodeIdAtWidth);

        //            dynamicParameters.Add("@intClinkQtyAtLength", obj.intClinkQtyAtLen);
        //            dynamicParameters.Add("@intClinkQtyAtWidth", obj.intClinkQtyAtWidth);

        //            dynamicParameters.Add("@numArea",obj.numArea);
        //            dynamicParameters.Add("@intTotalQty", obj.intTotalQty);
        //            dynamicParameters.Add("@intTotalBend", obj.intTotalBend);
        //            dynamicParameters.Add("@numTheoreticTonnage",obj.numTheoreticTonnage);
        //            dynamicParameters.Add("@numNetTonnage",obj.numNetTonnage);
        //            dynamicParameters.Add("@bitBendingCheck",obj.bitBendingCheck);
        //            dynamicParameters.Add("@bitCoat",obj.bitCoat);
        //            dynamicParameters.Add("@bitMachineCheck",obj.bitMachineCheckIndicator);
        //            dynamicParameters.Add("@bitTransportCheck",obj.bitTransportCheck);
        //            dynamicParameters.Add("@vchDrawingReference", obj.vchDrawingReference);
        //            dynamicParameters.Add("@chrDrawingVersion", DbType.StringFixedLength, obj.chrDrawingVersion);
        //            dynamicParameters.Add("@vchDrawingRemarks", obj.vchDrawingRemarks);
        //            dynamicParameters.Add("@tntStatusId",obj.tntStatusId);

        //            dynamicParameters.Add("@vchCLOnly",obj.bitCLOnly);
        //            dynamicParameters.Add("@vchWBSElementID", obj.strWBSElementId);
        //            dynamicParameters.Add("@intShapeTransHeaderId", obj.intShapeTransHeaderId);
        //            dynamicParameters.Add("@vchTactonConfigurationState", obj.vchTactonConfigurationState);
        //            dynamicParameters.Add("@intParentStructureid", obj.ParentStructureId);
        //            dynamicParameters.Add("@intSAPMaterialcodeId",obj.SAPMAterialCodeid);
        //            dynamicParameters.Add("@intPinSize", obj.intPinSizeId);
        //            dynamicParameters.Add("@chProduceIndicator", obj.ProduceIndicator);

        //            dynamicParameters.Add("@intuserID", obj.intUserid);
        //            intReturnValue = DataAccess.DataAccess.ExecuteNonQuery(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        return intReturnValue;
        //    }
        #endregion

        #region ColumnGet

        //    public DataSet GetStructureMarkingColumn(DetailInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("columnStructureMarking_Get");
        //            dynamicParameters.Add("@vchGroupMarkingName", obj.vchGroupMarkingName);
        //            dynamicParameters.Add("@intProjectId", obj.intProjectId);
        //            dynamicParameters.Add("@intSEDetailingId", obj.SEDetailingId);
        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }
        //    public DataSet GetProductMarkingColumn(DetailInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ColumnProductMarking_Get");
        //            dynamicParameters.Add("@intStructureMarkId", obj.intStructureMarkId);
        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }
        //    public int GetNoofCW(DetailInfo obj)
        //    {
        //        int intReturnValue;
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BCDCrossWire_Get");
        //            dynamicParameters.Add("@intProductId",obj.intProductCodeId);
        //            dynamicParameters.Add("@intMWLen ",obj.intMwLength);
        //            intReturnValue = DataAccess.DataAccess.GetScalar(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        return intReturnValue;
        //    }

        #endregion

        #region Drawing & Version

        #region Get

        //    public DataSet GetColumnWBSDrawingReference(DetailInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ColumnWBSDrawingReference_Get");
        //            dynamicParameters.Add("@intWBSElementId", obj.intWBSElementId);
        //            dynamicParameters.Add("@intGroupMarkId", obj.intGroupMarkId);
        //            dynamicParameters.Add("@intProjectId", obj.intProjectId);
        //            dynamicParameters.Add("@intProductTypeId", obj.intProductTypeId);

        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public DataSet GetColumnStructureMarkingDrawingReference(DetailInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ColumnStructureMarkingDrawingReference_Get");
        //            dynamicParameters.Add("@intStructureMarkingId", obj.intStructureMarkId);
        //            dynamicParameters.Add("@intGroupMarkId", obj.intGroupMarkId);
        //            dynamicParameters.Add("@intProjectId", obj.intProjectId);
        //            dynamicParameters.Add("@intProductTypeId", obj.intProductTypeId);

        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        #endregion

        #region Insert

        //    public int InsertColumnWBSDrawingHistory(DetailInfo obj)
        //    {
        //        int intReturnValue;
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ColumnWBSDrawingHistory_Insert");
        //            dynamicParameters.Add("@vchWBSElementIds", obj.vchWBSElementIds);
        //            dynamicParameters.Add("@vchUploadedFiles", obj.vchUploadedFiles);
        //            dynamicParameters.Add("@vchVersionNos", obj.vchUploadedFileVersions);
        //            dynamicParameters.Add("@vchFileRemarks", obj.vchFileRemarks);
        //            dynamicParameters.Add("@intUploadedUid", obj.intUploadedUid);
        //            dynamicParameters.Add("@intGroupMarkId", obj.intGroupMarkId);
        //            dynamicParameters.Add("@intProjectId", obj.intProjectId);
        //            dynamicParameters.Add("@intProductTypeId", obj.intProductTypeId);

        //            intReturnValue = System.Convert.ToInt32(DataAccess.DataAccess.GetScalar(dbcom));
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        return intReturnValue;
        //    }

        //    public int InsertColumnStructureMarkingDrawingHistory(DetailInfo obj)
        //    {
        //        int intReturnValue;
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ColumnStructureMarkingDrawingHistory_Insert");
        //            dynamicParameters.Add("@vchStructureMarkingIds", obj.vchStructureMarkingIds);
        //            dynamicParameters.Add("@vchStructureRevNos", obj.vchStructureRevNos);
        //            dynamicParameters.Add("@vchVersionNos", obj.vchStructureVersionNos);
        //            dynamicParameters.Add("@vchUploadedFiles", obj.vchSMUploadedFiles);
        //            dynamicParameters.Add("@vchFileRemarks", obj.vchSMFileRemarks);
        //            dynamicParameters.Add("@intUploadedUid", obj.intUploadedUid);
        //            dynamicParameters.Add("@intGroupMarkId", obj.intGroupMarkId);
        //            dynamicParameters.Add("@intProjectId", obj.intProjectId);
        //            dynamicParameters.Add("@intProductTypeId", obj.intProductTypeId);

        //            intReturnValue = System.Convert.ToInt32(DataAccess.DataAccess.GetScalar(dbcom));
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        return intReturnValue;
        //    }

        #endregion

        #endregion


        #endregion

        #region Slab Wall

        //    /// <summary>
        //    ///     ''' 
        //    ///     ''' </summary>
        //    ///     ''' <param name="obj"></param>
        //    ///     ''' <returns></returns>
        //    ///     ''' <remarks></remarks>
        //    ///     '''
        //    public int InsertSlabPRCDetails(DetailInfo obj)
        //    {
        //        int intReturnValue;
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("SWPRCDetails_Insert");

        //            dynamicParameters.Add("@intSEDetailingId", obj.SEDetailingId);  // ' modfied here 
        //            dynamicParameters.Add("@vchStructureMarkingName", obj.vchStructureMarkingName);
        //            dynamicParameters.Add("@intSAPMaterialcodeId",obj.SAPMAterialCodeid);

        //            dynamicParameters.Add("@numSlabWidth",obj.numSlabwidth);
        //            dynamicParameters.Add("@numSlabDepth",obj.numSlabDepth);
        //            dynamicParameters.Add("@numSlabLength",obj.numSlabLength);
        //            dynamicParameters.Add("@bitAssIndicator",obj.bitAssemblyIndicator);
        //            dynamicParameters.Add("@vchRemarks", obj.vchRemarks);
        //            DataAccess.DataAccess.db.AddOutParameter("@intStructureMarkid",obj.intStructureMarkId);
        //            // intReturnValue = CType(DataAccess.DataAccess.GetScalar(dbcom), Int32)

        //            DataAccess.DataAccess.GetScalar(dbcom);
        //            intReturnValue = (dbcom.Parameters["@intStructureMarkid"].Value);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        return intReturnValue;
        //    }

        //    public SqlDataReader GetShapeGroup(DetailInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ShapeGroup_Get");
        //            dynamicParameters.Add("@intShapeId", obj.intShapeId);
        //            return DataAccess.DataAccess.ExecuteReader(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public DataSet SWParameterInfoByPrjID_Get(DetailInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("SWParameterInfoByPrjID_Get");
        //            dynamicParameters.Add("@intProjectID", obj.intProjectId);
        //            dynamicParameters.Add("@sitProductTypeId", obj.sitProductTypeId);
        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        public List<DrainParameterSetByPrjIDDto> DrainParameterSetByPrjID_Get(DetailInfo obj)
        {
            IEnumerable<DrainParameterSetByPrjIDDto> drainParameterSetByPrjIDs;
            DataSet dataSet = new DataSet();
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intProjectId", obj.intProjectId);
                    drainParameterSetByPrjIDs = sqlConnection.Query<DrainParameterSetByPrjIDDto>(SystemConstants.DrainParameterInfoByPrjID_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                    return drainParameterSetByPrjIDs.ToList();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public DataSet DrainProductMarkingID_Get(DetailInfo obj)
        //{
        //    try
        //    {
        //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("DrainProductMarkingID_Get");
        //        dynamicParameters.Add("@intDrainStructureMarkingId", obj.intDrainStructureMarkId);
        //        return DataAccess.DataAccess.GetDataSet(dbcom);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //    public DataSet SWProjectParamOH_Get(DetailInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("SWProjectParamOH_Get");
        //            dynamicParameters.Add("@tntParamSetNumber", obj.tntParamSetNumber);
        //            dynamicParameters.Add("@intProjectId", obj.intProjectId);
        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public DataSet SWTransportMode_Get(DetailInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("SWTransportMode_Get");
        //            dynamicParameters.Add("@intParamSetNumber", obj.tntParamSetNumber);
        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        public List<Get_ParameterSet> DrainParamInfo_Get(DetailInfo obj)
        {
            IEnumerable<Get_ParameterSet> getParamSet;
            try
            {
                //dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("DrainParamInfo_Get");
                //dynamicParameters.Add("@intParamSetNumber", obj.tntParamSetNumber);
                //return DataAccess.DataAccess.GetDataSet(dbcom);

                using (var sqlConnection = new SqlConnection(connectionString))
                {


                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intParamSetNumber", obj.tntParamSetNumber);


                    getParamSet = sqlConnection.Query<Get_ParameterSet>(SystemConstants.DrainParamInfo_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                    sqlConnection.Close();
                    return getParamSet.ToList();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet DrainOverHangs_Get(DetailInfo obj)
        {
            try
            {
                DataSet ds = new DataSet();
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    //sqlConnection.Open();
                    //var dynamicParameters = new DynamicParameters();
                    //dynamicParameters.Add("@intMWSpace", obj.intMWSpacing);
                    //dynamicParameters.Add("@intCWSpace", obj.intCWSpacing);
                    //dynamicParameters.Add("@tntParamSetNumber", obj.tntParamSetNumber);
                    //dynamicParameters.Add("@chrShapeCode", obj.chrShapeCode);

                    //ds = sqlConnection.QueryFirstOrDefault<DataSet>(SystemConstants.DrainOverHangs_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                    SqlCommand cmd = new SqlCommand(SystemConstants.DrainOverHangs_Get, sqlConnection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@intMWSpace", obj.intMWSpacing));
                    cmd.Parameters.Add(new SqlParameter("@intCWSpace", obj.intCWSpacing));
                    cmd.Parameters.Add(new SqlParameter("@chrShapeCode", obj.chrShapeCode));

                    cmd.Parameters.Add(new SqlParameter("@tntParamSetNumber", obj.tntParamSetNumber));

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(ds);
                    return ds;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //    public DataSet SWProductCode_Mesh_Get(DetailInfo obj)
        //    {
        //        try
        //        {
        //            SqlCacheDependency SqlDep;
        //            DataSet dsCache = (DataSet)HttpRuntime.Cache["CacheSlabProduct"];
        //            if (dsCache == null)
        //            {
        //                SqlDep = new SqlCacheDependency("NDSCaching", "productcodemaster");
        //                dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ProductCodeMesh_Get");
        //                dynamicParameters.Add("@sitProductTypeId", obj.intProjectTypeId);
        //                dsCache = DataAccess.DataAccess.GetDataSet(dbcom);
        //                HttpRuntime.Cache.Insert("CacheSlabProduct", dsCache, SqlDep);
        //            }
        //            return dsCache;
        //        }
        //        // dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ProductCodeMesh_Get")
        //        // dynamicParameters.Add("@sitProductTypeId", obj.intProjectTypeId)
        //        // Return DataAccess.DataAccess.GetDataSet(dbcom)
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public DataSet SWProjectParamLap_Get(DetailInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("SWProjectParamLap_Get");
        //            dynamicParameters.Add("@tntParamSetNumber", obj.tntParamSetNumber);
        //            dynamicParameters.Add("@intProjectId", obj.intProjectId);
        //            dynamicParameters.Add("@intProductCodeID", obj.intProductCodeId);
        //            dynamicParameters.Add("@intConsignmentId", obj.intConsignmentType);
        //            dynamicParameters.Add("@intStructureElementTypeId", obj.intStructureElementTypeId);
        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public DataSet SWProjectTypeID_Get(DetailInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ProjectType_Get");
        //            dynamicParameters.Add("@intProjectId", obj.intProjectId);
        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public DataSet SWShapeParam_Get(DetailInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("SWShapeCode_Get");
        //            dynamicParameters.Add("@intShapeId", obj.intShapeId);
        //            dynamicParameters.Add("@intShapeTransHeaderId", obj.intShapeTransHeaderId);
        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public DataSet SW_MWCW_Lap_Get(DetailInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("SWLapping_Get");
        //            dynamicParameters.Add("@sitProductTypeId",obj.sitProductTypeId);
        //            dynamicParameters.Add("@intSETypeId", obj.intStructureElementTypeId);
        //            dynamicParameters.Add("@intProductCodeId", obj.intProductCodeId);
        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public DataSet SWParam_Get(DetailInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ProjParamValues_Get");
        //            dynamicParameters.Add("@intProductTypeID",obj.sitProductTypeId);
        //            dynamicParameters.Add("@intStructureElementTypeID", obj.intStructureElementTypeId);
        //            dynamicParameters.Add("@intProductCodeId", obj.intProductCodeId);
        //            dynamicParameters.Add("@intParamSetID", obj.intParamSetID);
        //            dynamicParameters.Add("@intWBSElementID", obj.intWBSElementId);
        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public DataSet SWPCMSpacing_Get(DetailInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("SWProduct_Code_Spacing_Get");
        //            dynamicParameters.Add("@intProductCodeId", obj.intProductCodeId);
        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public int SWShapeIdByCode_Get(DetailInfo obj)
        //    {
        //        int intShapeID;
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("SWShapeIdByCode_Get");
        //            dynamicParameters.Add("@chrShapeCode", obj.chrShapeCode);
        //            intShapeID = DataAccess.DataAccess.GetScalar(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        return intShapeID;
        //    }

        //    public DataSet SWPrefMeshMaster_Get(DetailInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("PreferredMeshData_Get");
        //            dynamicParameters.Add("@intProductCodeId", obj.intProductCodeId);
        //            dynamicParameters.Add("@intProjectTypeId", obj.intProjectTypeId);
        //            dynamicParameters.Add("@intMWSpace", obj.intMWSpacing);
        //            dynamicParameters.Add("@decMWLength", obj.decMWLength);
        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public DataSet SWProjectOH_Get(DetailInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("SWProjectOH_Get");
        //            dynamicParameters.Add("@decMWLength", obj.decMWLength);
        //            dynamicParameters.Add("@decCWLength", obj.decCWLength);
        //            dynamicParameters.Add("@intMWSpace", obj.intMWSpacing);
        //            dynamicParameters.Add("@intCWSpace", obj.intCWSpacing);
        //            dynamicParameters.Add("@tntParamSetNumber", obj.tntParamSetNumber);
        //            dynamicParameters.Add("@intProjectId", obj.intProjectId);
        //            dynamicParameters.Add("@intStructureElementId", obj.intStructureElementTypeId);
        //            dynamicParameters.Add("@sitProductTypeId", obj.sitProductTypeId);
        //            dynamicParameters.Add("@intConsignmentTypeId", obj.intConsignmentType);
        //            dynamicParameters.Add("@chrShapeCode", IIf(!obj.chrShapeCode == null, obj.chrShapeCode, ""));
        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public DataSet SWStructureMarking_Get(DetailInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("SWStructureMarking_Get");
        //            dynamicParameters.Add("@vchGroupMarkingName", obj.vchGroupMarkingName);
        //            dynamicParameters.Add("@intProjectId", obj.intProjectId);
        //            dynamicParameters.Add("@inSEDetailingId", obj.SEDetailingId); // -->Azeem
        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }
        //    // Drain
        public List<DrainStructureMarkingDto> DrainStructureMarking_Get(DetailInfo obj)
        {
            try
            {
                IEnumerable<DrainStructureMarkingDto> drainStructureMarkings;
                DataSet dsProjectParamDrain = new DataSet();

                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@vchGroupMarkingName", obj.vchGroupMarkingName);
                    dynamicParameters.Add("@intProjectId", obj.intProjectId);
                    dynamicParameters.Add("@intSEDetailingId", obj.SEDetailingId);
                    dynamicParameters.Add("@intGroupMarkID", obj.intGroupMarkId);

                    drainStructureMarkings = sqlConnection.Query<DrainStructureMarkingDto>(SystemConstants.DrainStructureMarking_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                    sqlConnection.Close();
                    return drainStructureMarkings.ToList();

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<GetDrainProdMarkDto> DrainProductMarkingDetails_Get(DetailInfo obj)
        {
            try
            {
                IEnumerable<GetDrainProdMarkDto> getDrainProdMarks;
                DataSet dsProduct = new DataSet();
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intStructureMarkId", obj.intStructureMarkId);
                    getDrainProdMarks = sqlConnection.Query<GetDrainProdMarkDto>(SystemConstants.DrainProductMarking_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                    return getDrainProdMarks.ToList();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //    public DataSet SWProductMarking_Get(DetailInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("SWProductMarking_Get");
        //            dynamicParameters.Add("@intStructureMarkId", obj.intStructureMarkId);
        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public int SWProductMarkingDetails_Del(DetailInfo obj)
        //    {
        //        int intReturnValue;
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("SWProductMarkingDetails_Del");
        //            dynamicParameters.Add("@intStructureMarkId", obj.intStructureMarkId);
        //            intReturnValue = System.Convert.ToInt32(DataAccess.DataAccess.GetScalar(dbcom));
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        return intReturnValue;
        //    }
        //    public int BCDProductMarkingDetails_Del(DetailInfo obj)
        //    {
        //        int intReturnValue;
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BCDProductMarkingDetails_Del");
        //            dynamicParameters.Add("@intStructureMarkId", obj.intStructureMarkId);
        //            dynamicParameters.Add("@vchStructureElement", obj.vchStructureElementType);
        //            intReturnValue = System.Convert.ToInt32(DataAccess.DataAccess.GetScalar(dbcom));
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        return intReturnValue;
        //    }
        //    public DataSet SWShapeCode_Get_Mesh(DetailInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("SWShapeCode_Get_Mesh");
        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public DataSet Pinsize_Get(DetailInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("PinSize_Get");
        //            dynamicParameters.Add("@intContractID", obj.intContractID);
        //            dynamicParameters.Add("@intProductCodeID", obj.intProductCodeId);
        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        public int DrainStructureMarking_InsUpd(AddDrainStructMarkingDto obj,out string errorMessage)
        {
            int intReturnValue=0;
            errorMessage = "";
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intStructureMarkId", obj.intStructureMarkId);
                    dynamicParameters.Add("@intSEDetailingID", obj.intSEDetailingID);
                    dynamicParameters.Add("@tntStructureRevNo", obj.tntStructureRevNo);
                    dynamicParameters.Add("@vchStructureMarkingName", obj.vchStructureMarkingName);
                    dynamicParameters.Add("@intGroupMarkId", obj.intGroupMarkId);
                    dynamicParameters.Add("@tntGroupRevNo", obj.tntGroupRevNo);
                    dynamicParameters.Add("@intParamSetNumber", obj.intParamSetNumber);
                    dynamicParameters.Add("@decStartChainage", obj.decStartChainage);
                    dynamicParameters.Add("@decEndChainage", obj.decEndChainage);
                    dynamicParameters.Add("@decDistance", obj.decDistance);
                    dynamicParameters.Add("@decStartTopLevel", obj.decStartTopLevel);
                    dynamicParameters.Add("@decEndTopLevel", obj.decEndTopLevel);
                    dynamicParameters.Add("@decStartInvertLevel", obj.decStartInvertLevel);
                    dynamicParameters.Add("@decEndInvertLevel", obj.decEndInvertLevel);
                    dynamicParameters.Add("@decStartHeight", obj.decStartHeight);
                    dynamicParameters.Add("@decEndHeight", obj.decEndHeight);
                    dynamicParameters.Add("@decStartDepth", obj.decStartDepth);
                    dynamicParameters.Add("@decEndDepth", obj.decEndDepth);
                    dynamicParameters.Add("@bitCascade", obj.bitCascade);
                    dynamicParameters.Add("@intCascadeNo", obj.intCascadeNo);
                    dynamicParameters.Add("@decCascadeDropHeight", obj.decCascadeDropHeight);
                    dynamicParameters.Add("@decCascadeWidth", obj.decCascadeWidth);
                    dynamicParameters.Add("@decCascadeCWLength", obj.decCascadeCWLength);
                    dynamicParameters.Add("@bitCrossLenInner", obj.bitCrossLenInner);
                    dynamicParameters.Add("@bitCrossLenOuter", obj.bitCrossLenOuter);
                    dynamicParameters.Add("@bitCrossLenSlab", obj.bitCrossLenSlab);
                    dynamicParameters.Add("@bitCrossLenBase", obj.bitCrossLenBase);
                    dynamicParameters.Add("@decCrossLenInner", obj.decCrossLenInner);
                    dynamicParameters.Add("@decCrossLenOuter", obj.decCrossLenOuter);
                    dynamicParameters.Add("@decCrossLenSlab", obj.decCrossLenSlab);
                    dynamicParameters.Add("@decCrossLenBase", obj.decCrossLenBase);
                    dynamicParameters.Add("@bitCoatingIndicator", obj.bitCoatingIndicator);
                    dynamicParameters.Add("@bitBendingCheck", obj.bitBendingCheck);
                    dynamicParameters.Add("@bitMachineCheck", obj.bitMachineCheck);
                    dynamicParameters.Add("@bitTransportCheck", obj.bitTransportCheck);
                    dynamicParameters.Add("@intMemberQty", obj.intMemberQty);
                    dynamicParameters.Add("@tntStatusId", obj.tntStatusId);
                    dynamicParameters.Add("@vchDrawingReference", obj.vchDrawingReference);
                    dynamicParameters.Add("@chrDrawingVersion", obj.chrDrawingVersion);
                    dynamicParameters.Add("@vchDrawingRemarks", obj.vchDrawingRemarks);
                    dynamicParameters.Add("@bitAssemblyIndicator", obj.bitAssemblyIndicator);
                    dynamicParameters.Add("@intUserID", obj.intUserID);
                    dynamicParameters.Add("@nvchProduceIndicator", obj.nvchProduceIndicator);


                    intReturnValue = sqlConnection.QueryFirstOrDefault<int>(SystemConstants.DrainStructureMarking_InsUpd, dynamicParameters, commandType: CommandType.StoredProcedure);
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

        //    public int SWStructureMarking_InsUpd(DetailInfo obj)
        //    {
        //        int intReturnValue;
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("SWStructureMarking_InsUpd");

        //            dynamicParameters.Add("@intStructureMarkId", obj.intStructureMarkId);
        //            dynamicParameters.Add("@tntStructureRevNo",obj.tntStructureRevNo);
        //            dynamicParameters.Add("@intSEDetailingId", obj.SEDetailingId);
        //            dynamicParameters.Add("@intParentStructureMarkId", obj.ParentStructureId);
        //            dynamicParameters.Add("@intConsignmentType", obj.intConsignmentType);
        //            dynamicParameters.Add("@intProductCodeId", obj.intProductCodeId);
        //            dynamicParameters.Add("@intShapeId", obj.intShapeId);
        //            dynamicParameters.Add("@vchStructureMarkingName", obj.vchStructureMarkingName);
        //            dynamicParameters.Add("@tntParamSetNumber", obj.tntParamSetNumber);
        //            dynamicParameters.Add("@bitSimilarStructure",obj.bitSimilarStructure);
        //            dynamicParameters.Add("@vchSimilarTo", obj.vchSimilarTo);
        //            dynamicParameters.Add("@decTotalMeshMainLength",obj.decTotalMeshMainLength);
        //            dynamicParameters.Add("@decTotalMeshCrossLength",obj.decTotalMeshCrossLength);
        //            dynamicParameters.Add("@intMemberQty", obj.intMemberQty);
        //            dynamicParameters.Add("@numArea",obj.numArea);
        //            dynamicParameters.Add("@intTotalQty", obj.intTotalQty);
        //            dynamicParameters.Add("@intTotalBend", obj.intTotalBend);
        //            dynamicParameters.Add("@numTheoreticTonnage",obj.numTheoreticTonnage);
        //            dynamicParameters.Add("@numNetTonnage",obj.numNetTonnage);
        //            dynamicParameters.Add("@vchDrawingReference", obj.vchDrawingReference);
        //            dynamicParameters.Add("@chrDrawingVersion", obj.chrDrawingVersion);
        //            dynamicParameters.Add("@vchDrawingRemarks", obj.vchDrawingRemarks);
        //            dynamicParameters.Add("@vchTactonConfigurationState", obj.vchTactonConfigurationState);
        //            dynamicParameters.Add("@tntStatusId",obj.tntStatusId);
        //            dynamicParameters.Add("@bitCoat",obj.bitCoat);
        //            dynamicParameters.Add("@bitBendingCheck",obj.bitBendingCheck);
        //            dynamicParameters.Add("@bitMachineCheck",obj.bitMachineCheck);
        //            dynamicParameters.Add("@bitTransportCheck",obj.bitTransportCheck);
        //            dynamicParameters.Add("@intSAPMaterialCodeId", obj.SAPMAterialCodeid);
        //            dynamicParameters.Add("@bitSingleMesh",obj.bitSingleMesh);
        //            dynamicParameters.Add("@chProduceIndicator", obj.ProduceIndicator);
        //            dynamicParameters.Add("@intuserID", obj.intUserid);

        //            intReturnValue = System.Convert.ToInt32(DataAccess.DataAccess.GetScalar(dbcom));
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        return intReturnValue;
        //    }

        //    public int SWProductMarking_InsUpd(DetailInfo obj)
        //    {
        //        int intReturnValue;
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("SWProductMarkingDetails_InsUpd");

        //            dynamicParameters.Add("@intStructureMarkId", obj.intStructureMarkId);
        //            dynamicParameters.Add("@tntStructureMarkRevNo",obj.tntStructureMarkRevNo);
        //            dynamicParameters.Add("@intProductCodeId", obj.intProductCodeId);
        //            dynamicParameters.Add("@vchProductMarkingName", obj.vchProductMarkingName);
        //            dynamicParameters.Add("@intShapeCodeId", obj.intShapeCodeId);
        //            dynamicParameters.Add("@numInvoiceMWLength",obj.numInvoiceMWLength);
        //            dynamicParameters.Add("@numInvoiceCWLength",obj.numInvoiceCWLength);
        //            dynamicParameters.Add("@intInvoiceMainQty", obj.intInvoiceMainQty);
        //            dynamicParameters.Add("@intInvoiceCrossQty", obj.intInvoiceCrossQty);
        //            dynamicParameters.Add("@intInvoiceTotalQty", obj.intInvoiceTotalQty);
        //            dynamicParameters.Add("@numProductionMWLength",obj.numProductionMWLength);
        //            dynamicParameters.Add("@numProductionCWLength",obj.numProductionCWLength);
        //            dynamicParameters.Add("@intProductionMainQty", obj.intProductionMainQty);
        //            dynamicParameters.Add("@intProductionCrossQty", obj.intProductionCrossQty);
        //            dynamicParameters.Add("@intProductionTotalQty", obj.intProductionTotalQty);
        //            dynamicParameters.Add("@intInvoiceMO1", obj.intInvoiceMO1);
        //            dynamicParameters.Add("@intInvoiceMO2", obj.intInvoiceMO2);
        //            dynamicParameters.Add("@intInvoiceCO1", obj.intInvoiceCO1);
        //            dynamicParameters.Add("@intInvoiceCO2", obj.intInvoiceCO2);
        //            dynamicParameters.Add("@intProductionMO1", obj.intProductionMO1);
        //            dynamicParameters.Add("@intProductionMO2", obj.intProductionMO2);
        //            dynamicParameters.Add("@intProductionCO1", obj.intProductionCO1);
        //            dynamicParameters.Add("@intProductionCO2", obj.intProductionCO2);
        //            dynamicParameters.Add("@intMWSpacing", obj.intMWSpacing);
        //            dynamicParameters.Add("@intCWSpacing", obj.intCWSpacing);
        //            dynamicParameters.Add("@sitPinSize",obj.sitPinSize);
        //            dynamicParameters.Add("@bitCoatingIndicator",obj.bitCoatingIndicator);
        //            dynamicParameters.Add("@numConversionFactor",obj.numConversionFactor);
        //            dynamicParameters.Add("@vchShapeSurcharge", obj.vchShapeSurcharge);
        //            dynamicParameters.Add("@bitBendIndicator",obj.bitBendIndicator);
        //            dynamicParameters.Add("@chrCalculationIndicator", obj.chrCalculationIndicator);
        //            dynamicParameters.Add("@tntGenerationStatus",obj.tntGenerationStatus);
        //            dynamicParameters.Add("@bitMachineCheckIndicator",obj.bitMachineCheckIndicator);
        //            dynamicParameters.Add("@vchTransportCheckResult", obj.vchTransportCheckResult);
        //            dynamicParameters.Add("@bitTransportIndicator",obj.bitTransportIndicator);
        //            dynamicParameters.Add("@vchBendCheckResult", obj.vchBendCheckResult);
        //            dynamicParameters.Add("@xmlResult", obj.xmlResult);
        //            dynamicParameters.Add("@vchFilePath", obj.vchFilePath);
        //            dynamicParameters.Add("@numInvoiceMWWeight",obj.numInvoiceMWWeight);
        //            dynamicParameters.Add("@numInvoiceCWWeight",obj.numInvoiceCWWeight);
        //            dynamicParameters.Add("@numInvoiceArea",obj.numInvoiceArea);
        //            dynamicParameters.Add("@numTheoraticalWeight",obj.numTheoraticalWeight);
        //            dynamicParameters.Add("@numNetWeight",obj.numNetWeight);
        //            dynamicParameters.Add("@intMemberQty", obj.intMemberQty);
        //            dynamicParameters.Add("@numProductionMWWeight",obj.numProductionMWWeight);
        //            dynamicParameters.Add("@numProductionCWWeight",obj.numProductionCWWeight);
        //            dynamicParameters.Add("@numProductionWeight",obj.numProductionWeight);
        //            dynamicParameters.Add("@ParamValues", obj.ParamValues);
        //            dynamicParameters.Add("@BendingPos", obj.BendingPos);
        //            dynamicParameters.Add("@intShapeTransHeaderId", obj.intShapeTransHeaderId);

        //            dynamicParameters.Add("@intEnvelopLength", obj.intEnvelopeLength);
        //            dynamicParameters.Add("@intEnvelopWidth", obj.intEnvelopewidth);
        //            dynamicParameters.Add("@intEnvelopHeight", obj.intEnvelopeHeight);
        //            dynamicParameters.Add("@intStructureElementId", obj.intStructureElementTypeId);
        //            dynamicParameters.Add("@intProductMarkId", obj.intProductMarkId);
        //            dynamicParameters.Add("@vchMCResult", obj.vchMachineResult);
        //            dynamicParameters.Add("@vchMWBVBSString", obj.vchMWBVBSString);
        //            dynamicParameters.Add("@vchCWBVBSString", obj.vchCWBVBSString);
        //            dynamicParameters.Add("@intuserID", obj.intUserid);
        //            dynamicParameters.Add("@chProduceIndicator", obj.ProduceIndicator);
        //            intReturnValue = System.Convert.ToInt32(DataAccess.DataAccess.GetScalar(dbcom));
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        return intReturnValue;
        //    }

        public int UpdateDrainProductMarking(DetailInfo obj)
        {
            int intReturnValue = 0;
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intStructureMarkId", obj.intStructureMarkId);
                    dynamicParameters.Add("@intInvoiceMainQty", obj.intInvoiceMainQty);
                    dynamicParameters.Add("@intInvoiceCrossQty", obj.intInvoiceCrossQty);
                    dynamicParameters.Add("@numProductionMWLength", obj.numProductionMWLength);
                    dynamicParameters.Add("@numProductionCWLength", obj.numProductionCWLength);
                    dynamicParameters.Add("@intProductionMainQty", obj.intProductionMainQty);
                    dynamicParameters.Add("@intProductionCrossQty", obj.intProductionCrossQty);

                    dynamicParameters.Add("@intInvoiceMO1", obj.intInvoiceMO1);
                    dynamicParameters.Add("@intInvoiceMO2", obj.intInvoiceMO2);
                    dynamicParameters.Add("@intInvoiceCO1", obj.intInvoiceCO1);
                    dynamicParameters.Add("@intInvoiceCO2", obj.intInvoiceCO2);
                    dynamicParameters.Add("@intProductionMO1", obj.intProductionMO1);
                    dynamicParameters.Add("@intProductionMO2", obj.intProductionMO2);
                    dynamicParameters.Add("@intProductionCO1", obj.intProductionCO1);
                    dynamicParameters.Add("@intProductionCO2", obj.intProductionCO2);

                    dynamicParameters.Add("@bitBendIndicator", obj.bitBendIndicator);

                    dynamicParameters.Add("@tntGenerationStatus", obj.tntGenerationStatus);
                    dynamicParameters.Add("@bitMachineCheckIndicator", obj.bitMachineCheckIndicator);
                    dynamicParameters.Add("@vchTransportCheckResult", obj.vchTransportCheckResult);
                    dynamicParameters.Add("@bitTransportIndicator", obj.bitTransportIndicator);
                    dynamicParameters.Add("@vchBendCheckResult", obj.vchBendCheckResult);
                    dynamicParameters.Add("@xmlResult", obj.xmlResult);

                    dynamicParameters.Add("@numTheoraticalWeight", obj.numTheoraticalWeight);
                    dynamicParameters.Add("@numNetWeight", obj.numNetWeight);

                    dynamicParameters.Add("@numProductionWeight", obj.numProductionWeight);

                    dynamicParameters.Add("@intEnvelopLength", obj.intEnvelopeLength);
                    dynamicParameters.Add("@intEnvelopWidth", obj.intEnvelopewidth);
                    dynamicParameters.Add("@intEnvelopHeight", obj.intEnvelopeHeight);

                    dynamicParameters.Add("@intMeshProductMarkId", obj.intMeshProductMarkId);
                    dynamicParameters.Add("@intDrainStructureMarkID", obj.intDrainStructureMarkId);
                    dynamicParameters.Add("@vchMWBVBSString", obj.vchMWBVBSString);
                    dynamicParameters.Add("@vchCWBVBSString", obj.vchCWBVBSString);
                    dynamicParameters.Add("@intuserID", obj.intUserid);
                    dynamicParameters.Add("@nvchProduceIndicator", obj.nvchProduceIndicator);
                    dynamicParameters.Add("@totalqty", obj.intInvoiceTotalQty);
                    intReturnValue = sqlConnection.QueryFirstOrDefault<int>(SystemConstants.DrainProductMarkingDetails_Update, dynamicParameters, commandType: CommandType.StoredProcedure);

                    return intReturnValue;

                }
            }
            catch (Exception ex)
            {
            }
            return intReturnValue;

        }

        public int InsertShapeValFrmTC(DetailInfo obj)
        {
            int intReturnValue = 0;
            try
            {

                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intShapeTransHeaderId", obj.intShapeTransHeaderId);
                    dynamicParameters.Add("@intShapeId", obj.intShapeId);
                    dynamicParameters.Add("@vchShapeDescription", obj.vchShapeDescription);
                    dynamicParameters.Add("@nvchParamValues", obj.nvchParamValues);
                    dynamicParameters.Add("@intUserId", obj.intUserid);
                    dynamicParameters.Add("@Output", null, dbType: DbType.Int32, ParameterDirection.Output);
                    intReturnValue = sqlConnection.QueryFirstOrDefault<int>(SystemConstants.ShapeCode_Insert, dynamicParameters, commandType: CommandType.StoredProcedure);
                    //intReturnValue = dynamicParameters.Get<int>("@Output");
                    return intReturnValue;

                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return intReturnValue;
        }

        public int DrainProductMarkingDetails_InsUpd(DetailInfo obj)
        {
            int intReturnValue;
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();

                    dynamicParameters.Add("@intStructureMarkId", obj.intStructureMarkId);
                    dynamicParameters.Add("@tntStructureMarkRevNo", obj.tntStructureMarkRevNo);
                    dynamicParameters.Add("@intProductCodeId", obj.intProductCodeId);
                    dynamicParameters.Add("@vchProductMarkingName", obj.vchProductMarkingName);
                    dynamicParameters.Add("@intShapeCodeId", obj.intShapeCodeId);
                    dynamicParameters.Add("@numInvoiceMWLength", obj.numInvoiceMWLength);
                    dynamicParameters.Add("@numInvoiceCWLength", obj.numInvoiceCWLength);
                    dynamicParameters.Add("@intInvoiceMainQty", obj.intInvoiceMainQty);
                    dynamicParameters.Add("@intInvoiceCrossQty", obj.intInvoiceCrossQty);
                    dynamicParameters.Add("@intInvoiceTotalQty", obj.intInvoiceTotalQty);
                    dynamicParameters.Add("@numProductionMWLength", obj.numProductionMWLength);
                    dynamicParameters.Add("@numProductionCWLength", obj.numProductionCWLength);
                    dynamicParameters.Add("@intProductionMainQty", obj.intProductionMainQty);
                    dynamicParameters.Add("@intProductionCrossQty", obj.intProductionCrossQty);
                    dynamicParameters.Add("@intProductionTotalQty", obj.intProductionTotalQty);
                    dynamicParameters.Add("@intInvoiceMO1", obj.intInvoiceMO1);
                    dynamicParameters.Add("@intInvoiceMO2", obj.intInvoiceMO2);
                    dynamicParameters.Add("@intInvoiceCO1", obj.intInvoiceCO1);
                    dynamicParameters.Add("@intInvoiceCO2", obj.intInvoiceCO2);
                    dynamicParameters.Add("@intProductionMO1", obj.intProductionMO1);
                    dynamicParameters.Add("@intProductionMO2", obj.intProductionMO2);
                    dynamicParameters.Add("@intProductionCO1", obj.intProductionCO1);
                    dynamicParameters.Add("@intProductionCO2", obj.intProductionCO2);
                    dynamicParameters.Add("@intMWSpacing", obj.intMWSpacing);
                    dynamicParameters.Add("@intCWSpacing", obj.intCWSpacing);
                    dynamicParameters.Add("@sitPinSize", obj.sitPinSize);
                    dynamicParameters.Add("@bitCoatingIndicator", obj.bitCoatingIndicator);
                    dynamicParameters.Add("@numConversionFactor", obj.numConversionFactor);
                    dynamicParameters.Add("@vchShapeSurcharge", obj.vchShapeSurcharge);
                    dynamicParameters.Add("@bitBendIndicator", obj.bitBendIndicator);
                    dynamicParameters.Add("@chrCalculationIndicator", obj.chrCalculationIndicator);
                    dynamicParameters.Add("@tntGenerationStatus", obj.tntGenerationStatus);
                    dynamicParameters.Add("@bitMachineCheckIndicator", obj.bitMachineCheckIndicator);
                    dynamicParameters.Add("@vchTransportCheckResult", obj.vchTransportCheckResult);
                    dynamicParameters.Add("@bitTransportIndicator", obj.bitTransportIndicator);
                    dynamicParameters.Add("@vchBendCheckResult", obj.vchBendCheckResult);
                    dynamicParameters.Add("@xmlResult", obj.xmlResult);
                    dynamicParameters.Add("@vchFilePath", obj.vchFilePath);
                    dynamicParameters.Add("@numInvoiceMWWeight", obj.numInvoiceMWWeight);
                    dynamicParameters.Add("@numInvoiceCWWeight", obj.numInvoiceCWWeight);
                    dynamicParameters.Add("@numInvoiceArea", obj.numInvoiceArea);
                    dynamicParameters.Add("@numTheoraticalWeight", obj.numTheoraticalWeight);
                    dynamicParameters.Add("@numNetWeight", obj.numNetWeight);
                    dynamicParameters.Add("@intMemberQty", obj.intMemberQty);
                    dynamicParameters.Add("@numProductionMWWeight", obj.numProductionMWWeight);
                    dynamicParameters.Add("@numProductionCWWeight", obj.numProductionCWWeight);
                    dynamicParameters.Add("@numProductionWeight", obj.numProductionWeight);
                    dynamicParameters.Add("@ParamValues", obj.ParamValues);
                    dynamicParameters.Add("@BendingPos", obj.BendingPos);
                    dynamicParameters.Add("@intShapeTransHeaderId", obj.intShapeTransHeaderId);
                    dynamicParameters.Add("@intEnvelopLength", obj.intEnvelopeLength);
                    dynamicParameters.Add("@intEnvelopWidth", obj.intEnvelopewidth);
                    dynamicParameters.Add("@intEnvelopHeight", obj.intEnvelopeHeight);
                    // dynamicParameters.Add("@intStructureElementId", obj.intStructureElementTypeId)
                    dynamicParameters.Add("@intProductMarkId", obj.intProductMarkId);
                    dynamicParameters.Add("@intDrainStructureMarkID", obj.intDrainStructureMarkId);
                    dynamicParameters.Add("@tntDrainLayerId", obj.tntLayer);
                    dynamicParameters.Add("@decStartChainage", obj.decStartChainage);
                    dynamicParameters.Add("@decEndChainage", obj.decEndChainage);
                    dynamicParameters.Add("@vchMWBVBSString", obj.vchMWBVBSString);
                    dynamicParameters.Add("@vchCWBVBSString", obj.vchCWBVBSString);
                    dynamicParameters.Add("@intuserID", obj.intUserid);
                    dynamicParameters.Add("@nvchProduceIndicator", obj.nvchProduceIndicator);
                    //dynamicParameters.Add("@Output", null, dbType: DbType.Int32, ParameterDirection.Output);
                    intReturnValue = sqlConnection.QueryFirstOrDefault<int>(SystemConstants.DrainProductMarkingDetails_InsUpd, dynamicParameters, commandType: CommandType.StoredProcedure);

                    return intReturnValue;

                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return intReturnValue;
        }

        //    public DataSet GetAutoCABdata(DetailInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("MeshAutoCABPrepare");
        //            dynamicParameters.Add("@MeshproductCodeID", obj.intProductCodeId);
        //            dynamicParameters.Add("@MWLength", obj.numInvoiceMWLength);
        //            dynamicParameters.Add("@CWLength", obj.numInvoiceCWLength);
        //            dynamicParameters.Add("@MWCnt", obj.intInvoiceMainQty);
        //            dynamicParameters.Add("@CWCnt", obj.intInvoiceCrossQty);
        //            dynamicParameters.Add("@StructureMarkID", obj.intStructureMarkId);
        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }
        //    public DataSet ImportSlabMachineCheck_Get()
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ImportSlabMachineCheck_Get");

        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }
        //    public DataSet SWStructureProduct_Get(DetailInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("SWStructureProduct_Get");
        //            dynamicParameters.Add("@intGroupMarkid", obj.intGroupMarkId);

        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //        }
        //    }


        #region Drawing & Version

        #region Get
        //    public DataSet GetMeshWBSDrawingReference(DetailInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("MeshWBSDrawingReference_Get");
        //            dynamicParameters.Add("@intWBSElementId", obj.intWBSElementId);
        //            dynamicParameters.Add("@intGroupMarkId", obj.intGroupMarkId);
        //            dynamicParameters.Add("@intProjectId", obj.intProjectId);
        //            dynamicParameters.Add("@intProductTypeId", obj.intProductTypeId);


        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public DataSet GetDrainWBSDrawingReference(DetailInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("DrainWBSDrawingReference_Get");
        //            dynamicParameters.Add("@intWBSElementId", obj.intWBSElementId);
        //            dynamicParameters.Add("@intGroupMarkId", obj.intGroupMarkId);
        //            dynamicParameters.Add("@intProjectId", obj.intProjectId);
        //            dynamicParameters.Add("@intProductTypeId", obj.intProductTypeId);


        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public DataSet GetMeshStructureMarkingDrawingReference(DetailInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("MeshStructureMarkingDrawingReference_Get");
        //            dynamicParameters.Add("@intStructureMarkingId", obj.intStructureMarkId);
        //            dynamicParameters.Add("@intGroupMarkId", obj.intGroupMarkId);
        //            dynamicParameters.Add("@intProjectId", obj.intProjectId);
        //            dynamicParameters.Add("@intProductTypeId", obj.intProductTypeId);

        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public DataSet GetDrainStructureMarkingDrawingReference(DetailInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("DrainStructureMarkingDrawingReference_Get");
        //            dynamicParameters.Add("@intStructureMarkingId", obj.intStructureMarkId);
        //            dynamicParameters.Add("@intGroupMarkId", obj.intGroupMarkId);
        //            dynamicParameters.Add("@intProjectId", obj.intProjectId);
        //            dynamicParameters.Add("@intProductTypeId", obj.intProductTypeId);

        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        #endregion

        #region Insert

        //    public int InsertMeshWBSDrawingHistory(DetailInfo obj)
        //    {
        //        int intReturnValue;
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("MeshWBSDrawingHistory_Insert");
        //            dynamicParameters.Add("@vchWBSElementIds", obj.vchWBSElementIds);
        //            dynamicParameters.Add("@vchUploadedFiles", obj.vchUploadedFiles);
        //            dynamicParameters.Add("@vchVersionNos", obj.vchUploadedFileVersions);
        //            dynamicParameters.Add("@vchFileRemarks", obj.vchFileRemarks);
        //            dynamicParameters.Add("@intUploadedUid", obj.intUploadedUid);
        //            dynamicParameters.Add("@intGroupMarkId", obj.intGroupMarkId);
        //            dynamicParameters.Add("@intProjectId", obj.intProjectId);
        //            dynamicParameters.Add("@intProductTypeId", obj.intProductTypeId);

        //            intReturnValue = System.Convert.ToInt32(DataAccess.DataAccess.GetScalar(dbcom));
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        return intReturnValue;
        //    }

        //    public int InsertDrainWBSDrawingHistory(DetailInfo obj)
        //    {
        //        int intReturnValue;
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("DrainWBSDrawingHistory_Insert");
        //            dynamicParameters.Add("@vchWBSElementIds", obj.vchWBSElementIds);
        //            dynamicParameters.Add("@vchUploadedFiles", obj.vchUploadedFiles);
        //            dynamicParameters.Add("@vchVersionNos", obj.vchUploadedFileVersions);
        //            dynamicParameters.Add("@vchFileRemarks", obj.vchFileRemarks);
        //            dynamicParameters.Add("@intUploadedUid", obj.intUploadedUid);
        //            dynamicParameters.Add("@intGroupMarkId", obj.intGroupMarkId);
        //            dynamicParameters.Add("@intProjectId", obj.intProjectId);
        //            dynamicParameters.Add("@intProductTypeId", obj.intProductTypeId);

        //            intReturnValue = System.Convert.ToInt32(DataAccess.DataAccess.GetScalar(dbcom));
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        return intReturnValue;
        //    }
        //    public int InsertMeshStructureMarkingDrawingHistory(DetailInfo obj)
        //    {
        //        int intReturnValue;
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("MeshStructureMarkingDrawingHistory_Insert");
        //            dynamicParameters.Add("@vchStructureMarkingIds", obj.vchStructureMarkingIds);
        //            dynamicParameters.Add("@vchStructureRevNos", obj.vchStructureRevNos);
        //            dynamicParameters.Add("@vchVersionNos", obj.vchStructureVersionNos);
        //            dynamicParameters.Add("@vchUploadedFiles", obj.vchSMUploadedFiles);
        //            dynamicParameters.Add("@vchFileRemarks", obj.vchSMFileRemarks);
        //            dynamicParameters.Add("@intUploadedUid", obj.intUploadedUid);
        //            dynamicParameters.Add("@intGroupMarkId", obj.intGroupMarkId);
        //            dynamicParameters.Add("@intProjectId", obj.intProjectId);
        //            dynamicParameters.Add("@intProductTypeId", obj.intProductTypeId);

        //            intReturnValue = System.Convert.ToInt32(DataAccess.DataAccess.GetScalar(dbcom));
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        return intReturnValue;
        //    }

        //    public int InsertDrainStructureMarkingDrawingHistory(DetailInfo obj)
        //    {
        //        int intReturnValue;
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("DrainStructureMarkingDrawingHistory_Insert");
        //            dynamicParameters.Add("@vchStructureMarkingIds", obj.vchStructureMarkingIds);
        //            dynamicParameters.Add("@vchStructureRevNos", obj.vchStructureRevNos);
        //            dynamicParameters.Add("@vchVersionNos", obj.vchStructureVersionNos);
        //            dynamicParameters.Add("@vchUploadedFiles", obj.vchSMUploadedFiles);
        //            dynamicParameters.Add("@vchFileRemarks", obj.vchSMFileRemarks);
        //            dynamicParameters.Add("@intUploadedUid", obj.intUploadedUid);
        //            dynamicParameters.Add("@intGroupMarkId", obj.intGroupMarkId);
        //            dynamicParameters.Add("@intProjectId", obj.intProjectId);
        //            dynamicParameters.Add("@intProductTypeId", obj.intProductTypeId);

        //            intReturnValue = System.Convert.ToInt32(DataAccess.DataAccess.GetScalar(dbcom));
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        return intReturnValue;
        //    }

        #endregion

        #endregion

        #endregion

        #region Machine Check

        //    public DataSet GetMachineContraint(DetailInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("MachineContraint_Get");
        //            dynamicParameters.Add("@SitProductTypeID ",obj.sitProductTypeId);
        //            dynamicParameters.Add("@intProdMo1 ",obj.intProductionMO1);
        //            dynamicParameters.Add("@intProdMo2 ",obj.intProductionMO2);
        //            dynamicParameters.Add("@intProdCo1 ",obj.intProductionCO1);
        //            dynamicParameters.Add("@intProdCo2 ",obj.intProductionCO2);
        //            dynamicParameters.Add("@numMWSpacing ",obj.numMWSpacing);
        //            dynamicParameters.Add("@numCWSpacing ",obj.numCWSpacing);
        //            dynamicParameters.Add("@numProdMWLength ",obj.numProductionMWLength);
        //            dynamicParameters.Add("@numProdCWLength ",obj.numProductionCWLength);
        //            dynamicParameters.Add("@intShapeCodeID ",obj.intShapeCodeId);
        //            // ' need to add more parameters
        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        public DataSet GetMachineLimits(string chrShapeCode)
        {
            try
            {
                DataSet dsMachineLimits = new DataSet();

                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@chrShapeCode", chrShapeCode);

                    dsMachineLimits = (DataSet)sqlConnection.Query<DataSet>(SystemConstants.TMCProgamInput_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                    sqlConnection.Close();
                    return dsMachineLimits;

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetMachineCheckValues(DetailInfo obj)
        {
            try
            {
                DataSet dsMachineCheckValues = new DataSet();

                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@chrShapeCode", obj.chrShapeCode);
                    dynamicParameters.Add("@intProductCodeID", obj.intProductCodeId);

                    SqlCommand cmd = new SqlCommand(SystemConstants.MachineCheck_Get, sqlConnection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@chrShapeCode", obj.chrShapeCode));
                    cmd.Parameters.Add(new SqlParameter("@intProductCodeID", obj.intProductCodeId));


                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dsMachineCheckValues);

                    //dsMachineCheckValues = (DataSet)sqlConnection.Query<DataSet>(SystemConstants.MachineCheck_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                    sqlConnection.Close();
                    return dsMachineCheckValues;

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion

        #region Bore Pile

           #region GET

        public List<BorePileParamInfoDto>GetBorePileParameterInfo(DetailInfo obj)
        {
            try
            {
               
                IEnumerable<BorePileParamInfoDto> borePileParamInfos;
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@tntParamSetNumber", obj.tntParamSetNumber);

                    borePileParamInfos = sqlConnection.Query<BorePileParamInfoDto>(SystemConstants.BPCParameterInfo_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                    return borePileParamInfos.ToList();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<GetSchnellMachineConfig> GetSchnellMachineConfig(DetailInfo obj)
        {
            IEnumerable<GetSchnellMachineConfig> bOMHeaderDtos;
            GetSchnellMachineConfig BOMServiceHeader = new GetSchnellMachineConfig();
            List<GetSchnellMachineConfig> listHeaderDetails =new List<GetSchnellMachineConfig>();
            List<GetSchnellMachineConfig> listBomheader1 = new List<GetSchnellMachineConfig> { };
           
            //listHeaderDetails = new List<GetSchnellMachineConfig> { };
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    DataSet dsGetBomHeader = new DataSet();

                    SqlCommand cmd = new SqlCommand(SystemConstants.BPCSchnellMachineConfig_Get, sqlConnection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@intNoOfMainBar", obj.intNoOfMainBar));
                    cmd.Parameters.Add(new SqlParameter("@holetoholedia", obj.intHoletoHoleDia));
                    cmd.Parameters.Add(new SqlParameter("@vchTemplateCode", obj.vchTemplateCode));
                   
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dsGetBomHeader);

                    if (dsGetBomHeader != null && dsGetBomHeader.Tables.Count != 0)
                    {
                        foreach (DataRowView drvBeam in dsGetBomHeader.Tables[0].DefaultView)
                        {
                            BOMServiceHeader.intTemplateID = Convert.ToInt32(drvBeam["intTemplateID"]);
                            BOMServiceHeader.vchTemplateName = (drvBeam["vchTemplateName"]).ToString();
                            BOMServiceHeader.intNoOfHole = Convert.ToInt32(drvBeam["intNoOfHole"]);
                           
                        }
                       
                    }

                    if (dsGetBomHeader != null && dsGetBomHeader.Tables.Count != 0)
                    {
                        foreach (DataRowView drvBeam in dsGetBomHeader.Tables[0].DefaultView)
                        {
                            BOMServiceHeader.sitBarSeqNo = Convert.ToInt32(drvBeam["sitBarSeqNo"]);
                            BOMServiceHeader.intHolePosition = Convert.ToInt32(drvBeam["intHolePosition"]);
                        }

                    }

                    listBomheader1.Add(BOMServiceHeader);
                  

                }
               

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return listBomheader1;


        }

        //public DataSet GetSchnellMachineConfig(DetailInfo obj)
        //{
        //    try
        //    {
        //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BPCSchnellMachineConfig_Get");
        //        dynamicParameters.Add("@intNoOfMainBar", obj.intNoOfMainBar);
        //        dynamicParameters.Add("@holetoholedia", obj.intHoletoHoleDia);
        //        dynamicParameters.Add("@vchTemplateCode", obj.vchTemplateCode);
        //        return DataAccess.DataAccess.GetDataSet(dbcom);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public List<BorePileParamInfoDto> GetBorePileParameterInfoByPrjId(DetailInfo obj)
        {
            try
            {
                DataSet dsProduct = new DataSet();
                IEnumerable<BorePileParamInfoDto> borePileParamInfos;
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intProjectId", obj.intProjectId);
                    dynamicParameters.Add("@intProductTypeId", obj.sitProductTypeId);
                    borePileParamInfos = sqlConnection.Query<BorePileParamInfoDto>(SystemConstants.BPCParameterInfoByPrjID_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                    return borePileParamInfos.ToList();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //    public DataSet getProductsDetailForXML(DetailInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BPCProductDetailsForXML_Get");
        //            dynamicParameters.Add("@intGroupMarkId", obj.intGroupMarkIdXML);
        //            dynamicParameters.Add("@intStructureMarkingId", obj.intStructureMarkingId);
        //            dynamicParameters.Add("@intSEDetailingId", obj.intSEDetailingIdXML);
        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }
        //    // Modified by Abhijeet for grade diameter sorting.
        
        public List<GetBorePilePopulateMethodsDto> GetBorePilePopulateMethods(string strType, int intProductL2Id, string strMainBarCode)
        {
            try
            {
                IEnumerable<GetBorePilePopulateMethodsDto> borePilePopulateMethods;
                DataSet dataSet = new DataSet();
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@vchType", strType);
                    dynamicParameters.Add("@sitProductTypeL2Id", intProductL2Id);
                    dynamicParameters.Add("@vchMainBarPatternCode", strMainBarCode);
                    borePilePopulateMethods = sqlConnection.Query<GetBorePilePopulateMethodsDto>(SystemConstants.BPCPopulateDetails_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                    return borePilePopulateMethods.ToList();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<GetCentralizerDto>GetPopulateCentralizer()
        {
            try
            {
                IEnumerable<GetCentralizerDto> getCentralizers;
                
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();

                    getCentralizers = sqlConnection.Query<GetCentralizerDto>(SystemConstants.BPCPopulateCentralizer_Get, commandType: CommandType.StoredProcedure);
                    return getCentralizers.ToList();
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<GetBPMainBarPatternDetailDto> GetBorePileMainBarPatternDetails(DetailInfo obj)
        {
            try
            {
                IEnumerable<GetBPMainBarPatternDetailDto> getBPMainBarPatternDetails;
                DataSet dataSet = new DataSet();
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@vchMainBarPatternCode", obj.vchMainBarPattern);

                    getBPMainBarPatternDetails = sqlConnection.Query<GetBPMainBarPatternDetailDto>(SystemConstants.BPCMainBarPatternDetails_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                    return getBPMainBarPatternDetails.ToList();
                }

                
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<GetBPElevationPatternDetails>GetBorePileElevationPatternDetails(DetailInfo obj)
        {
            try
            {
                IEnumerable<GetBPElevationPatternDetails>getBPElevationPatterns;
                DataSet dataSet = new DataSet();
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@vchElevationCode", obj.vchElevationPattern);

                    getBPElevationPatterns = sqlConnection.Query<GetBPElevationPatternDetails>(SystemConstants.BPCElevationPatternDetails_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                    return getBPElevationPatterns.ToList();
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<GetBorePilStructureMarkingDto> GetBorePilStructureMarkingDetails(DetailInfo obj)
        {
            try
            {
                IEnumerable<GetBorePilStructureMarkingDto> getBorePilStructureMarkings;

                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@vchGroupMarkingName", obj.vchGroupMarkingName);
                    dynamicParameters.Add("@intProjectId", obj.intProjectId);
                    dynamicParameters.Add("@intSEDetailingId", obj.intSELevelDetailsID);

                    getBorePilStructureMarkings = sqlConnection.Query<GetBorePilStructureMarkingDto>(SystemConstants.BPCStructureMarking_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                    return getBorePilStructureMarkings.ToList();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<StiffenerDetailsDto>getStiffenerDetails(DetailInfo obj)
        {
            try
            {
                IEnumerable<StiffenerDetailsDto>stiffenerDetails;

                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intCageLength", obj.intCageLength);
                    dynamicParameters.Add("@intPileDia", obj.intPileDia);

                    stiffenerDetails = sqlConnection.Query<StiffenerDetailsDto>(SystemConstants.BPCStiffnerDetails_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                    return stiffenerDetails.ToList();
                }
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<getAdditionalSpiralDetails>getAdditionalSpiralDetails(DetailInfo obj)
        {
            try
            {
                IEnumerable<getAdditionalSpiralDetails> getAdditionalSpirals;

                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intCageLength", obj.intCageLength);
                    dynamicParameters.Add("@intPileDia", obj.intPileDia);

                    getAdditionalSpirals = sqlConnection.Query<getAdditionalSpiralDetails>(SystemConstants.BPCAddSpiralDetails_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                    return getAdditionalSpirals.ToList();
                }
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //    public DataSet GetBorePileProductsListingByStructId(DetailInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BPCProductMarking_Get");
        //            dynamicParameters.Add("@intStructId", obj.intStructureMarkId);
        //            dynamicParameters.Add("@intGroupMarkId", obj.intGroupMarkId);
        //            dynamicParameters.Add("@intSELevelDetailsID", obj.intSELevelDetailsID);

        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }
        
        public List<getAdditionalSpiralDetails>GetBPCPileDiaDependentValue(DetailInfo obj)
        {
            try
            {
                IEnumerable<getAdditionalSpiralDetails> getAdditionalSpirals;
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intPileDia", obj.intPileDia);

                    getAdditionalSpirals = sqlConnection.Query<getAdditionalSpiralDetails>(SystemConstants.BPCPileDiaDependentValue_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                    return getAdditionalSpirals.ToList();
                }
              
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<GetSchnellMachineConfig>GetTemplateForSchnellMachine(DetailInfo obj)
        {
            try
            {
                IEnumerable<GetSchnellMachineConfig> gettemplateforSchnell;
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@holetoholedia", obj.intHoletoHoleDia);
                    dynamicParameters.Add("@intNoOfMainBar", obj.intNoOfMainBarLayer1);

                    gettemplateforSchnell =sqlConnection.Query<GetSchnellMachineConfig>(SystemConstants.BPCPopulateTemplateForSchnell_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                    sqlConnection.Close();
                    return gettemplateforSchnell.ToList();

                }
             
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string getCoverCodeForStrucMarking(DetailInfo obj)
        {
            string strReturnValue="";
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intCover", obj.intCoverLink);
                    dynamicParameters.Add("@vchCoverCode", 5);
                    strReturnValue = sqlConnection.QueryFirstOrDefault<string>(SystemConstants.BPCCovercodeForStrucMarking_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                    //strReturnValue = dynamicParameters.Get<string>("@vchCoverCode");

                    return strReturnValue;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string getBarMarkPrefix(DetailInfo obj)
        {
            string strReturnValue = "";
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@vchBarMarkDescription", obj.vchBarMarkDescription);
                    dynamicParameters.Add("@vchElevationCode", obj.vchSLPattern);
                    //dynamicParameters.Add("@vchBarMarkPrefix", 10);
                    dynamicParameters.Add("@vchBarMarkPrefix", null, dbType: DbType.Int32, ParameterDirection.Output);

                    sqlConnection.QueryFirstOrDefault<string>(SystemConstants.BPCBarMarkPrefix_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                    
                    strReturnValue = dynamicParameters.Get<string>("@vchBarMarkPrefix");
                  
                    return strReturnValue;

                }

             
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        //    public int getElevtnPatnLinkDiaForStrucMarking(DetailInfo obj)
        //    {
        //        Int32 intReturnValue;
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BPCElevtnPatnLinkDiaForStrucMarking_Get");
        //            dynamicParameters.Add("@vchElevationCode", obj.vchElevationCode);
        //            DataAccess.DataAccess.db.AddOutParameter("@tntNoOfPitch", 0);
        //            DataAccess.DataAccess.GetScalar(dbcom);
        //            intReturnValue = (dbcom.Parameters["@tntNoOfPitch"].Value);
        //            return intReturnValue;
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        public string getBPCMaterialCodeIDForStrucMarking(DetailInfo obj)
        {
            string strReturnValue = "";
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@vchMainBarPattern", obj.vchMainBarPattern);
                    dynamicParameters.Add("@vchElevationPattern", obj.vchElevationPattern);
                    dynamicParameters.Add("@numSmallerMainBarLength", obj.intSmallerMainBarLength);
                    dynamicParameters.Add("@numPileDia", obj.numPileDia);
                    dynamicParameters.Add("@bitCoat", obj.bitCoat);
                    dynamicParameters.Add("@sitProductTypeL2Id", obj.sitProductTypeId); // added for CHG0032240 by debarati
                    //dynamicParameters.Add("@MaterialCodeID", 10);
                    dynamicParameters.Add("@MaterialCodeID", null, dbType: DbType.Int32, ParameterDirection.Output);

                    sqlConnection.QueryFirstOrDefault<string>(SystemConstants.BPCMaterialCodeID_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

                    //strReturnValue = dynamicParameters.Get<string>("@MaterialCodeID");

                    int a = dynamicParameters.Get<int>("@MaterialCodeID");


                    strReturnValue = a.ToString();

                    return strReturnValue;

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //    public string getWBSforDrawingXML(DetailInfo obj)
        //    {
        //        string strReturnValue = "";
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BPCWBSforDrawingXML_Get");
        //            dynamicParameters.Add("@intWBSElementId", obj.intWBSElementIdXML);
        //            DataAccess.DataAccess.db.AddOutParameter("@vchWBSDetail", 50);
        //            DataAccess.DataAccess.GetScalar(dbcom);
        //            strReturnValue = (dbcom.Parameters["@vchWBSDetail"].Value);
        //            return strReturnValue;
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }
        //    public string getDrawingPathForBPC(DetailInfo obj)
        //    {
        //        string strReturnValue = "";
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BPCPDFDrawingPath_Get");
        //            dynamicParameters.Add("@intStructureMarkId", obj.intStructureMarkingId);
        //            DataAccess.DataAccess.db.AddOutParameter("@vchDrawingPath", 300);
        //            DataAccess.DataAccess.GetScalar(dbcom);
        //            strReturnValue = (dbcom.Parameters["@vchDrawingPath"].Value);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        return strReturnValue;
        //    }

        public List<GetCageNotesDto> GetCageNotesPopulateMethods()
        {
            try
            {
               
                IEnumerable<GetCageNotesDto> getCageNotes;
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    getCageNotes = sqlConnection.Query<GetCageNotesDto>(SystemConstants.BPCPopulateCageNotes_Get, commandType: CommandType.StoredProcedure);
                    return getCageNotes.ToList();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<GetSchnellMachineConfig>GetSchnellTemplates()
        {
            try
            {
                IEnumerable<GetSchnellMachineConfig>getSchnellTemplates;
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    getSchnellTemplates = sqlConnection.Query<GetSchnellMachineConfig>(SystemConstants.BPCPopulateSchnellTemplate_Get, commandType: CommandType.StoredProcedure);
                    return getSchnellTemplates.ToList();
                }
            
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<List<GetSchnellMachineConfig>>GetSchnellConfiguration(DetailInfo obj)
        {
            try
            {
                DataSet ds = new DataSet();
                IEnumerable<GetSchnellMachineConfig> getSchnellMachines;
                List<GetSchnellMachineConfig> drainParamDepths_list = new List<GetSchnellMachineConfig>();
                List<List<GetSchnellMachineConfig>> Final_List = new List<List<GetSchnellMachineConfig>>();


                

                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    //var dynamicParameters = new DynamicParameters();
                    //dynamicParameters.Add("@intNoOfMainBar", obj.intNoOfMainBar);
                    //dynamicParameters.Add("@vchTemplateCode", obj.vchTemplateCode);
                    //getSchnellMachines = sqlConnection.Query<GetSchnellMachineConfig>(SystemConstants.BPCSchnellConfiguration_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

                    //drainParamDepths_list = getSchnellMachines.ToList();

                    SqlCommand cmd = new SqlCommand(SystemConstants.BPCSchnellConfiguration_Get, sqlConnection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@intNoOfMainBar", obj.intNoOfMainBar));
                    cmd.Parameters.Add(new SqlParameter("@vchTemplateCode", obj.vchTemplateCode));


                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                    adapter.Fill(ds);

                    if (ds != null && ds.Tables.Count != 0)
                    {
                        foreach (DataRowView drvTracking in ds.Tables[0].DefaultView)
                        {
                            GetSchnellMachineConfig groupMark = new GetSchnellMachineConfig
                            {
                                intTemplateID=Convert.ToInt32(drvTracking["intTemplateID"]),
                                sitBarSeqNo = Convert.ToInt32(drvTracking["sitBarSeqNo"]),
                                intHolePosition = Convert.ToInt32(drvTracking["intHolePosition"]),
                         
   
                            };
                            drainParamDepths_list.Add(groupMark);
                        }

                        Final_List.Add(drainParamDepths_list);

                        drainParamDepths_list = new List<GetSchnellMachineConfig>();

                        foreach (DataRowView drvTracking in ds.Tables[1].DefaultView)
                        {
                            GetSchnellMachineConfig groupMark = new GetSchnellMachineConfig
                            {
                                intTemplateID = Convert.ToInt32(drvTracking["intTemplateID"]),
                                vchTemplateName = Convert.ToString(drvTracking["vchTemplateName"]),
                                intNoOfHole = Convert.ToInt32(drvTracking["intNoOfHole"]),

        
                            };
                            drainParamDepths_list.Add(groupMark);

                        }

                        Final_List.Add(drainParamDepths_list);


                        drainParamDepths_list = new List<GetSchnellMachineConfig>();


                        foreach (DataRowView drvTracking in ds.Tables[2].DefaultView)
                        {
                            GetSchnellMachineConfig groupMark = new GetSchnellMachineConfig
                            {
                                sitBarSeqNo = Convert.ToInt32(drvTracking["sitBarSeqNo"]),
                                intHolePosition = Convert.ToInt32(drvTracking["intHolePosition"]),                     

                            };
                            drainParamDepths_list.Add(groupMark);

                        }


                        Final_List.Add(drainParamDepths_list);

                    }


                    return Final_List;

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        //    public DataSet GetSchnellDetails(DetailInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BPCSchnellDetails_Get");
        //            dynamicParameters.Add("@intNoOfBars", obj.intNoOfMainBar);
        //            dynamicParameters.Add("@intTemplateId", obj.intTemplateID);
        //            dynamicParameters.Add("@intNoOfHoles", obj.intNoOfMBars);
        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }


        //    public DataSet GetSchnellTemplate(DetailInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BPCSchnellTemplate_Get");
        //            dynamicParameters.Add("@holetoholedia", obj.intHoletoHoleDia);
        //            dynamicParameters.Add("@intMainBarSize", obj.intMainBarSize);
        //            dynamicParameters.Add("@intNoOfMainBars", obj.intNoOfMainBar);
        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public DataSet GetBPCDetailsForDrawing(DetailInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BPCDetailsForDrawing_Get");
        //            dynamicParameters.Add("@intWBSElementId", obj.intWBSElementIdXML);
        //            dynamicParameters.Add("@intGroupMarkId", obj.intGroupMarkIdXML);
        //            dynamicParameters.Add("@intStructureMarkingId", obj.intStructureMarkingId);
        //            dynamicParameters.Add("@intSEDetailingId", obj.intSEDetailingIdXML);
        //            dynamicParameters.Add("@vchElevationPattern", obj.vchElevationPattern);
        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public DataSet GetSchnellBarsForPDF(DetailInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BPCSchnellBarsForPDF_Get");
        //            dynamicParameters.Add("@intSchnellId", obj.intTemplateID);
        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    // export for BPC 
        //    public DataSet GetBPCExportDetails(DetailInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BPCExportDetails_Get");
        //            dynamicParameters.Add("@intGroupMarkId", obj.intGroupMarkId);
        //            dynamicParameters.Add("@intSEDetailingId", obj.intSELevelDetailsID);
        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        public List<BPCMEPDetailsDto>GetMEPDetailsForBPCDrawing(DetailInfo obj)
        {
            try
            {
                IEnumerable<BPCMEPDetailsDto>bPCMEPDetails;
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intMEPConfigId", obj.intMEPConfigId);

                    bPCMEPDetails= sqlConnection.Query<BPCMEPDetailsDto>(SystemConstants.BPCMEPDetailsForDrawing_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                    return bPCMEPDetails.ToList();
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

           #region Insert

        public int InsertBPCStructureMarkingDetails(InsertBPCStructureMarkingDto obj,out string errorMessage)
        {
            int intReturnValue=0;
            errorMessage = "";
            try
            {

                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();

                    dynamicParameters.Add("@intSEDetailingId", obj.intSEDetailingId);
                    dynamicParameters.Add("@vchStructureMarkingName", obj.vchStructureMarkingName);
                    dynamicParameters.Add("@tntStructureRevNo", obj.tntStructureRevNo);
                    dynamicParameters.Add("@intSAPMaterialCodeId", obj.intSAPMaterialCodeId);
                    dynamicParameters.Add("@intGroupMarkId", obj.intGroupMarkId);
                    dynamicParameters.Add("@tntGroupRevNo", obj.tntGroupRevNo);
                    dynamicParameters.Add("@tntParamSetNumber", obj.tntParamSetNumber);
                    dynamicParameters.Add("@intNoofWeldPoints", obj.intNoofWeldPoints);
                    dynamicParameters.Add("@intShenellTemplateId", obj.intShenellTemplateId);
                    dynamicParameters.Add("@vchShenellAlternateTemplates", obj.vchShenellAlternateTemplates);
                    dynamicParameters.Add("@numPileDia", obj.numPileDia);
                    dynamicParameters.Add("@numCageLength", obj.numCageLength);
                    dynamicParameters.Add("@numCageDia", obj.numCageDia);
                    dynamicParameters.Add("@vchFabricationType", obj.vchFabricationType);
                    dynamicParameters.Add("@vchMainBarPattern", obj.vchMainBarPattern);
                    dynamicParameters.Add("@vchElevationPattern", obj.vchElevationPattern);
                    dynamicParameters.Add("@tntNumOfMainBar", obj.tntNumOfMainBar);
                    dynamicParameters.Add("@tntNumOfLinksatStart", obj.tntNumOfLinksatStart);
                    dynamicParameters.Add("@tntNumOfLinksatEnd", obj.tntNumOfLinksatEnd);
                    dynamicParameters.Add("@tntAdditionalSpiral", obj.tntAdditionalSpiral);
                    dynamicParameters.Add("@tntNumberOfStiffnerOrCentralizer", obj.tntNumberOfStiffnerOrCentralizer);
                    dynamicParameters.Add("@tntNumOfSquareStiffner", obj.tntNumOfSquareStiffner);
                    dynamicParameters.Add("@tntNumOfCircularRings", obj.tntNumOfCircularRings);
                    dynamicParameters.Add("@intLapLength", obj.intLapLength);
                    dynamicParameters.Add("@intEndLength", obj.intEndLength);
                    dynamicParameters.Add("@intCoverToLink", obj.intCoverToLink);
                    dynamicParameters.Add("@intAjustmentFactor", obj.intAjustmentFactor);
                    dynamicParameters.Add("@bitCageNoteChangeIndicator", obj.bitCageNoteChangeIndicator);
                    dynamicParameters.Add("@tntCageNoteId", obj.tntCageNoteId);
                    dynamicParameters.Add("@vchCageNote", obj.vchCageNote);
                    dynamicParameters.Add("@MachineTypeId", obj.MachineTypeId);
                    dynamicParameters.Add("@intMemberQty", obj.intMemberQty);
                    dynamicParameters.Add("@numNetWeight", obj.numNetWeight);
                    dynamicParameters.Add("@numActualWeight", obj.numActualWeight);
                    dynamicParameters.Add("@vchDrawingReference", obj.vchDrawingReference);
                    dynamicParameters.Add("@chrDrawingVersion", obj.chrDrawingVersion);
                    dynamicParameters.Add("@vchDrawingRemarks", obj.vchDrawingRemarks);
                    dynamicParameters.Add("@vchTactonConfigurationState", obj.vchTactonConfigurationState);
                    dynamicParameters.Add("@vchHoleConfigurationXML", obj.vchHoleConfigurationXML);
                    dynamicParameters.Add("@vchTactonXmlResult", obj.vchTactonXmlResult);
                    dynamicParameters.Add("@bitAssembly", obj.bitAssembly);
                    dynamicParameters.Add("@bitGenerate3DforPreview", obj.bitGenerate3DforPreview);
                    dynamicParameters.Add("@nvchEdwgPath", obj.nvchEdwgPath);
                    dynamicParameters.Add("@chrGeneration3DStatus", obj.chrGeneration3DStatus);
                    dynamicParameters.Add("@vchOutputDrawingRef", obj.vchOutputDrawingRef);
                    dynamicParameters.Add("@bitCoat", obj.bitCoat);
                    dynamicParameters.Add("@vchMainBarDia", obj.vchMainBarDia);
                    dynamicParameters.Add("@vchLinkDia", obj.vchLinkDia);
                    dynamicParameters.Add("@vchLinkPitch", obj.vchLinkPitch);
                    dynamicParameters.Add("@tntStatusId", obj.tntStatusId);
                    dynamicParameters.Add("@intCreatedUId", obj.intCreatedUId);
                    dynamicParameters.Add("@intUploadedUId", obj.intUploadedUId);
                    dynamicParameters.Add("@vchPDFPath", obj.vchPDFPath);
                    //dynamicParameters.Add("@indicator", obj.FondIndicator);

                    dynamicParameters.Add("@output", null, dbType: DbType.Int32, ParameterDirection.Output);

                    sqlConnection.QueryFirstOrDefault<int>(SystemConstants.BPCStructureMarking_Insert, dynamicParameters, commandType: CommandType.StoredProcedure);

                    intReturnValue = dynamicParameters.Get<int>("@output");
                    sqlConnection.Close();

                    return intReturnValue;

                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return intReturnValue;
               
            }
        }

        public int[] InsertBPCProjectParameterSet(BPCProjectParamInsertDto obj, out string errorMessage)
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
                    dynamicParameters.Add("@intLapLength", obj.intLapLength);
                    dynamicParameters.Add("@intEndLength", obj.intEndLength);
                    dynamicParameters.Add("@intAdjFactor", obj.intAdjFactor);
                    dynamicParameters.Add("@intCoverToLink", obj.intCoverToLink);
                    dynamicParameters.Add("@tntStatusId", obj.tntStatusId);
                    dynamicParameters.Add("@intParameterSet", obj.intParameterSet);
                    dynamicParameters.Add("@vchParameterType", obj.vchParameterType);
                    dynamicParameters.Add("@bitStructureMarkingLevel", obj.bitStructureMarkingLevel);
                    dynamicParameters.Add("@intUserId", obj.intuserid);
                    dynamicParameters.Add("@vchDescription", obj.vchDescription);
                    //dynamicParameters.Add("@Output", 0);
                    dynamicParameters.Add("@Output", null, dbType: DbType.Int32, ParameterDirection.Output);
                    //dynamicParameters.Add("@intScopeIdentity", 0);
                    dynamicParameters.Add("@intScopeIdentity", null, dbType: DbType.Int32, ParameterDirection.Output);

                    sqlConnection.QueryFirstOrDefault<int>(SystemConstants.BPCProjectParameter_Insert, dynamicParameters, commandType: CommandType.StoredProcedure);
                    intResult[0] = dynamicParameters.Get<int>("@Output");
                    intResult[1] = dynamicParameters.Get<int>("@intScopeIdentity");
                    sqlConnection.Close();
                    return intResult;
                }
            }
            catch (Exception ex)
            {
               errorMessage = ex.Message;
                return intResult;
            }
        }

        public int InsertBorePileProducts(DetailInfo obj)
        {
            int intReturnValue;
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intStructId", obj.intStructureMarkId);
                    dynamicParameters.Add("@@Output", null, dbType: DbType.Int32, ParameterDirection.Output);

                    sqlConnection.QueryFirstOrDefault<int>(SystemConstants.BPCProductMarking_Insert, dynamicParameters, commandType: CommandType.StoredProcedure);

                    intReturnValue = dynamicParameters.Get<int>("@@Output");
                    sqlConnection.Close();

                    return intReturnValue;

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int InsertAccessoriesCentralizer(InsertAccessoriesCentralizerDto obj, out string errorMessage)
        {
            int intReturnValue=0;
            errorMessage = "";
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intSeleveldetailsId", obj.intSELevelDetailsID);
                    dynamicParameters.Add("@intGroupMarkId", obj.intGroupMarkId);
                    dynamicParameters.Add("@intStructId", obj.intStructureMarkId);
                    dynamicParameters.Add("@vchBarMarkName", obj.vchBarMark);
                    dynamicParameters.Add("@vchSapMaterialCode", obj.vchSapMaterialCode);
                    dynamicParameters.Add("@intQty", obj.intQty);
                    dynamicParameters.Add("@intOutput", null, dbType: DbType.Int32, ParameterDirection.Output);

                    sqlConnection.QueryFirstOrDefault<int>(SystemConstants.AccessoriesCentralizer_Insert, dynamicParameters, commandType: CommandType.StoredProcedure);

                    intReturnValue = dynamicParameters.Get<int>("@intOutput");
                    sqlConnection.Close();

                    return intReturnValue;

                }

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return intReturnValue;  
            }
        }


        #endregion

           #region Update

        public int UpdateBPCStructureMarkingDetails(UpdateBPCStructureMarkingDto obj,out string errorMessage)
        {
            int intReturnValue=0;
            errorMessage = "";
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intStructureMarkId", obj.intStructureMarkId);
                    dynamicParameters.Add("@vchStructureMarkingName", obj.vchStructureMarkingName);
                    dynamicParameters.Add("@tntStructureRevNo", obj.tntStructureRevNo);
                    dynamicParameters.Add("@intSAPMaterialCodeId", obj.intBPCMaterialCodeID);
                    dynamicParameters.Add("@intGroupMarkId", obj.intGroupMarkId);
                    dynamicParameters.Add("@tntGroupRevNo", obj.tntGroupRevNo);
                    dynamicParameters.Add("@tntParamSetNumber", obj.tntParamSetNumber);
                    dynamicParameters.Add("@tntCageSeqNo", obj.intCageSeqNo);
                    dynamicParameters.Add("@intNoofWeldPoints", obj.intNoofWeldPoints);
                    dynamicParameters.Add("@intShenellTemplateId", obj.intShenellTemplateId);
                    dynamicParameters.Add("@vchShenellAlternateTemplates", obj.vchShenellAlternateTemplates);
                    dynamicParameters.Add("@numPileDia", obj.numPileDia);
                    dynamicParameters.Add("@numCageLength", obj.numCageLength);
                    dynamicParameters.Add("@numCageDia", obj.intCageDia);
                    dynamicParameters.Add("@vchFabricationType", obj.vchFabricationType);
                    dynamicParameters.Add("@vchMainBarPattern", obj.vchMainBarPattern);
                    dynamicParameters.Add("@vchElevationPattern", obj.vchElevationPattern);
                    dynamicParameters.Add("@tntNumOfMainBar", obj.tntNumOfMainBar);
                    dynamicParameters.Add("@tntNumOfLinksatStart", obj.tntNumOfLinksatStart);
                    dynamicParameters.Add("@tntNumOfLinksatEnd", obj.tntNumOfLinksatEnd);
                    dynamicParameters.Add("@tntAdditionalSpiral", obj.tntAdditionalSpiral);
                    dynamicParameters.Add("@tntNumberOfStiffnerOrCentralizer", obj.tntNumberOfStiffnerOrCentralizer);
                    dynamicParameters.Add("@tntNumOfSquareStiffner", obj.tntNumOfSquareStiffner);
                    dynamicParameters.Add("@tntNumOfCircularRings", obj.tntNumOfCircularRings);
                    dynamicParameters.Add("@intLapLength", obj.intLapLength);
                    dynamicParameters.Add("@intEndLength", obj.intEndLength);
                    dynamicParameters.Add("@intCoverToLink", obj.intCoverToLink);
                    dynamicParameters.Add("@intAjustmentFactor", obj.intAjustmentFactor);
                    dynamicParameters.Add("@bitCageNoteChangeIndicator", obj.bitCageNoteChangeIndicator);
                    dynamicParameters.Add("@tntCageNoteId", obj.tntCageNoteId);
                    dynamicParameters.Add("@vchCageNote", obj.vchCageNote);
                    dynamicParameters.Add("@MachineTypeId", obj.intMachineTypeId);
                    dynamicParameters.Add("@intMemberQty", obj.intMemberQty);
                    dynamicParameters.Add("@numNetWeight", obj.numNetWeight);
                    dynamicParameters.Add("@numActualWeight", obj.numStructActualWeight);
                    dynamicParameters.Add("@vchDrawingReference", obj.vchDrawingReference);
                    dynamicParameters.Add("@chrDrawingVersion", obj.chrDrawingVersion);
                    dynamicParameters.Add("@vchDrawingRemarks", obj.vchDrawingRemarks);
                    dynamicParameters.Add("@vchTactonConfigurationState", obj.vchTactonConfigurationState);
                    dynamicParameters.Add("@vchHoleConfigurationXML", obj.vchHoleConfigurationXML);
                    dynamicParameters.Add("@vchTactonXmlResult", obj.vchTactonXmlResult);
                    dynamicParameters.Add("@bitAssembly", obj.bitAssemblyIndicator);
                    dynamicParameters.Add("@bitGenerate3DforPreview", obj.bitGenerate3DforPreview);
                    dynamicParameters.Add("@nvchEdwgPath", obj.vch3DImageRef);
                    dynamicParameters.Add("@chrGeneration3DStatus", obj.chrGeneration3DStatus);
                    dynamicParameters.Add("@vchOutputDrawingRef", obj.vchOutputDrawingRef);
                    dynamicParameters.Add("@bitCoat", obj.bitCoat);
                    dynamicParameters.Add("@vchMainBarDia", obj.vchMainBarDia);
                    dynamicParameters.Add("@vchLinkDia", obj.vchLinkDia);
                    dynamicParameters.Add("@vchLinkPitch", obj.vchLinkPitch);
                    dynamicParameters.Add("@tntStatusId", obj.tntStatusId);
                    dynamicParameters.Add("@intCreatedUId", obj.intCreatedUID);
                    dynamicParameters.Add("@intUploadedUId", obj.intUpdatedUID);
                    dynamicParameters.Add("@vchPDFPath", obj.vchBPCPdfPath);
                    //dynamicParameters.Add("@indicator", obj.FondIndicator);

                    dynamicParameters.Add("@output", null, dbType: DbType.Int32, ParameterDirection.Output);

                    sqlConnection.QueryFirstOrDefault<int>(SystemConstants.BPCStructureMarking_Update, dynamicParameters, commandType: CommandType.StoredProcedure);

                    intReturnValue = dynamicParameters.Get<int>("@output");
                    sqlConnection.Close();
                    return intReturnValue;


                }

            }
            catch (Exception ex)
            {
                errorMessage=ex.Message;
                return intReturnValue;
            }
        }

        public int UpdateBPCMainBarPattern(UpdateBPCMainBarDto obj, out string errorMessage)
        {
            int intReturnValue=0;
            errorMessage = "";
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intMainBarPatternID", obj.intMainBarPatternID);
                    dynamicParameters.Add("@intNoBarSize", obj.intNoBarSize);
                    dynamicParameters.Add("@intNoBarLength", obj.intNoBarLength);
                    dynamicParameters.Add("@intStatusId", obj.intStatusId);
                    dynamicParameters.Add("@intUId", obj.intCreatedUID);
                    dynamicParameters.Add("@output", null, dbType: DbType.Int32, ParameterDirection.Output);

                    sqlConnection.QueryFirstOrDefault<int>(SystemConstants.BPCMainBarPatternDetails_Update, dynamicParameters, commandType: CommandType.StoredProcedure);

                    intReturnValue = dynamicParameters.Get<int>("@output");
                    sqlConnection.Close();
                    return intReturnValue;


                }

                
            }
            catch (Exception ex)
            {
               errorMessage = ex.Message;
                return intReturnValue;
            }
        }

        public int UpdateBarPostionForSchnelllMcn(SnlMcnBarPositonsUpdateDto obj, out string errorMessage)
        {
            int intReturnValue=0;
            errorMessage = "";
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@vchTemplateCode", obj.vchTemplateCode);
                    dynamicParameters.Add("@sitBarSeqNo", obj.sitBarSeqNo);
                    dynamicParameters.Add("@intHolePosition", obj.intHolePosition);
                    dynamicParameters.Add("@intNumberOfBars", obj.intNumberOfBars);
                    dynamicParameters.Add("@intTemplateId", obj.intTemplateId);
                    dynamicParameters.Add("@output", null, dbType: DbType.Int32, ParameterDirection.Output);

                    sqlConnection.QueryFirstOrDefault<int>(SystemConstants.BPCSnlMcnBarPositons_Update, dynamicParameters, commandType: CommandType.StoredProcedure);

                    intReturnValue = dynamicParameters.Get<int>("@output");
                    sqlConnection.Close();
                    return intReturnValue;


                }
              
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return intReturnValue;
            }
        }

        public int UpdateBPCElevationPattern(BPCElevationUpdateDto obj, out string errorMessage)
        {
            int intReturnValue=0;
            errorMessage = "";
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intElevationPatternID", obj.intElevationId);
                    dynamicParameters.Add("@intNoOfSpiralLinkSize", obj.intNoOfSpiralLinkSize);
                    dynamicParameters.Add("@intNoOfSpiralLinkPitch", obj.intNoOfSpiralLinkPitch);
                    dynamicParameters.Add("@intNoOfSpiralLinkLength", obj.intNoOfSpiralLinkLength);
                    dynamicParameters.Add("@intStatusId", obj.intStatusId);
                    dynamicParameters.Add("@intUId", obj.intCreatedUID);
                    dynamicParameters.Add("@output", null, dbType: DbType.Int32, ParameterDirection.Output);

                    sqlConnection.QueryFirstOrDefault<int>(SystemConstants.BPCElevationPatternDetails_Update, dynamicParameters, commandType: CommandType.StoredProcedure);

                    intReturnValue = dynamicParameters.Get<int>("@output");
                    sqlConnection.Close();
                    return intReturnValue;


                }
         
            }
            catch (Exception ex)
            {
                errorMessage=ex.Message;
               
            }
            return intReturnValue;
        }

        public int UpdateBorePileProducts(BPCProductMarkingUpdateDto obj,out string errorMessage)
        {
            int intReturnValue=0;
            errorMessage = "";
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intCABProductMarkID", obj.intCABProductMarkID);
                    dynamicParameters.Add("@tntLayer", obj.tntLayer);
                    dynamicParameters.Add("@chrGenerationStatus", obj.chrGenerationStatus);
                    dynamicParameters.Add("@bitAssemblyIndicator", obj.bitAssemblyIndicator);
                    dynamicParameters.Add("@bitMachineIndicator", obj.bitMachineIndicator);
                    dynamicParameters.Add("@output", null, dbType: DbType.Int32, ParameterDirection.Output);

                    sqlConnection.QueryFirstOrDefault<int>(SystemConstants.BPCProductMarking_Update, dynamicParameters, commandType: CommandType.StoredProcedure);

                    intReturnValue = dynamicParameters.Get<int>("@output");
                    sqlConnection.Close();
                    return intReturnValue;


                }

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return intReturnValue;
            }
        }

        //    public int UpdateBPCPDFPath(DetailInfo obj)
        //    {
        //        int intReturnValue;
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BPCPDFPath_Update");
        //            dynamicParameters.Add("@intStructureMarkingId", obj.intStructureMarkId);
        //            dynamicParameters.Add("@intGroupMarkingId", obj.intGroupMarkId);
        //            dynamicParameters.Add("@SEDetailingId", obj.intSELevelDetailsID);
        //            dynamicParameters.Add("@vchPDFPath", obj.vchBPCPdfPath);
        //            dynamicParameters.Add("@intRevNo", obj.tntStructureRevNo);
        //            DataAccess.DataAccess.db.AddOutParameter("@output", 0);
        //            DataAccess.DataAccess.GetScalar(dbcom);
        //            intReturnValue = (dbcom.Parameters["@output"].Value);
        //            return intReturnValue;
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public int UpdateBPCDWGPath(DetailInfo obj)
        //    {
        //        int intReturnValue;
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BPCUpdateDWGPath");
        //            dynamicParameters.Add("@intStructureMarkId", obj.intStructureMarkId);
        //            dynamicParameters.Add("@vchDWGPath", obj.vchDWGPath);
        //            DataAccess.DataAccess.db.AddOutParameter("@output", 0);
        //            DataAccess.DataAccess.GetScalar(dbcom);
        //            intReturnValue = (dbcom.Parameters["@output"].Value);
        //            return intReturnValue;
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public int UpdateBPCDetailingXML(DetailInfo obj)
        //    {
        //        int intReturnValue;
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BPCDetailingXML_Update");
        //            dynamicParameters.Add("@intStructureMarkId", obj.intStructureMarkId);
        //            dynamicParameters.Add("@vchBPCDetalingXML", obj.vchBPCDetailingXML);
        //            DataAccess.DataAccess.db.AddOutParameter("@output", 0);
        //            DataAccess.DataAccess.GetScalar(dbcom);
        //            intReturnValue = (dbcom.Parameters["@output"].Value);
        //            return intReturnValue;
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }


        #endregion

           #region Delete

        public int DeleteBPCStructureMarkingDetails(DetailInfo obj)
        {
            int intReturnValue;
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intStructMarkId", obj.intStructureMarkId);
                    dynamicParameters.Add("@tntCageSeqNo", obj.intCageSeqNo);
                    dynamicParameters.Add("@intGroupMarkId", obj.intGroupMarkId);
                    dynamicParameters.Add("@output", 0);
                    sqlConnection.QueryFirstOrDefault<int>(SystemConstants.BPCStructureMarking_Delete, dynamicParameters, commandType: CommandType.StoredProcedure);

                    intReturnValue = dynamicParameters.Get<int>("@output");
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

           #region Drawing & Version

        //populateWBSUploadedDrawing :
        //    public DataSet GetBPCWBSDrawingReference(DetailInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BPCWBSDrawingReference_Get");
        //            dynamicParameters.Add("@intWBSElementId", obj.intWBSElementId);
        //            dynamicParameters.Add("@intProjectId", obj.intProjectId);
        //            dynamicParameters.Add("@intGroupMarkId", obj.intGroupMarkId);
        //            dynamicParameters.Add("@intProductTypeId", obj.intProductTypeId);

        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //InsertWBSDrawingHistory
        //    public int InsertBPCWBSDrawingHistory(DetailInfo obj)
        //    {
        //        try
        //        {
        //            int intReturnValue;

        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BPCWBSDrawingHistory_Insert");
        //            dynamicParameters.Add("@vchWBSElementIds", obj.vchWBSElementIds);
        //            dynamicParameters.Add("@vchUploadedFiles", obj.vchUploadedFiles);
        //            dynamicParameters.Add("@vchVersionNos", obj.vchUploadedFileVersions);
        //            dynamicParameters.Add("@vchFileRemarks", obj.vchFileRemarks);
        //            dynamicParameters.Add("@intProjectId", obj.intProjectId);
        //            dynamicParameters.Add("@intGroupMarkId", obj.intGroupMarkId);
        //            dynamicParameters.Add("@intProductTypeId", obj.intProductTypeId);
        //            dynamicParameters.Add("@intUploadedUid", obj.intUploadedUid);

        //            intReturnValue = System.Convert.ToInt32(DataAccess.DataAccess.GetScalar(dbcom));
        //            return intReturnValue;
        //        }

        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public DataSet GetBPCSMDrawingReference(DetailInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BPCStructureMarkingDrawingReference_Get");
        //            dynamicParameters.Add("@intStructId", obj.intStructureMarkId);
        //            dynamicParameters.Add("@intProjectId", obj.intProjectId);
        //            dynamicParameters.Add("@intGroupMarkId", obj.intGroupMarkId);
        //            dynamicParameters.Add("@intProductTypeId", obj.intProductTypeId);

        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //populateSMUploadedDrawing
        //    public DataSet GetBPCSMUploadedDrawingReference(DetailInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BPCSMUploadedDrawingRef_Get");
        //            dynamicParameters.Add("@intStructId", obj.intStructureMarkId);
        //            dynamicParameters.Add("@intGroupMarkId", obj.intGroupMarkId);

        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //InsertSMDrawingHistory
        //    public int InsertBPCSMUploadedDrawing(DetailInfo obj)
        //    {
        //        int intReturnValue;

        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BPCSMUploadDrawing_Insert");
        //            dynamicParameters.Add("@vchStructIds", obj.vchStructureMarkingIds);
        //            dynamicParameters.Add("@vchUploadedFiles", obj.vchUploadedFiles);
        //            dynamicParameters.Add("@vchVersionNos", obj.vchUploadedFileVersions);
        //            dynamicParameters.Add("@vchFileRemarks", obj.vchFileRemarks);
        //            // dynamicParameters.Add("@intProjectId", obj.intProjectId)
        //            dynamicParameters.Add("@intGroupMarkId", obj.intGroupMarkId);
        //            // dynamicParameters.Add("@intProductTypeId", obj.intProductTypeId)
        //            dynamicParameters.Add("@intUploadedUid", obj.intUploadedUid);

        //            intReturnValue = System.Convert.ToInt32(DataAccess.DataAccess.GetScalar(dbcom));
        //            return intReturnValue;
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }
        //    public int InsertBPCSMDrawingHistory(DetailInfo obj)
        //    {
        //        int intReturnValue;

        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BPCStructureMarkingDrawingHistory_Insert");
        //            dynamicParameters.Add("@vchStructIds", obj.vchStructureMarkingIds);
        //            dynamicParameters.Add("@vchUploadedFiles", obj.vchUploadedFiles);
        //            // dynamicParameters.Add("@vchVersionNos", obj.vchUploadedFileVersions)
        //            dynamicParameters.Add("@vchFileRemarks", obj.vchFileRemarks);
        //            dynamicParameters.Add("@intProjectId", obj.intProjectId);
        //            dynamicParameters.Add("@intGroupMarkId", obj.intGroupMarkId);
        //            dynamicParameters.Add("@intProductTypeId", obj.intProductTypeId);
        //            dynamicParameters.Add("@intUploadedUid", obj.intUploadedUid);

        //            intReturnValue = System.Convert.ToInt32(DataAccess.DataAccess.GetScalar(dbcom));
        //            return intReturnValue;
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        #endregion


        #endregion

        #region Common

        //    public System.Data.DataSet GetArmaDetailsOnArmaID(int intArmaID)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ArmaInfoOnArmaID_Get");
        //            dynamicParameters.Add("@intArmaID", intArmaID);
        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }
        //    public int InsertArmaInfo(string vchFolder, int intNumber, string vchDnumber, string branch, int intRelease, string vchJobTitle, string vchElement, string Structural, int intQty)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ArmaPlusInfo_Insert");
        //            dynamicParameters.Add("@vchFolder", vchFolder);
        //            dynamicParameters.Add("@vchFolder", DbType.Int16, intNumber);
        //            dynamicParameters.Add("@vchFolder", vchDnumber);
        //            dynamicParameters.Add("@vchFolder", branch);
        //            dynamicParameters.Add("@vchFolder", DbType.Int16, intRelease);
        //            dynamicParameters.Add("@vchFolder", vchJobTitle);
        //            dynamicParameters.Add("@vchFolder", vchElement);
        //            dynamicParameters.Add("@vchFolder", Structural);
        //            dynamicParameters.Add("@vchFolder", DbType.Int16, intQty);
        //            return DataAccess.DataAccess.GetScalar(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }
        //    public DataSet GetExportDetails(DetailInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ExportDetails_Get");
        //            dynamicParameters.Add("@intGroupMarkId", obj.intGroupMarkId);
        //            dynamicParameters.Add("@intSEDetailingId", obj.intSELevelDetailsID);
        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public DataSet GetProductsDetailForPDFUpdate(DetailInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ProductDetailsForPDFUpdate_Get");
        //            dynamicParameters.Add("@vchStructureElement", obj.vchStructureElementType);

        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }



        #endregion

        //    public DataSet GetStructureElement()
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("StructureElementMap_Get");

        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public DataSet GetSAPMaterialStructure(DetailInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("SAPMAterialStructure_Get");
        //            dynamicParameters.Add("@intStructureElementid", obj.intStructureElementTypeId);

        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }
        //    public string GetParentStructureTab(DetailInfo obj)
        //    {
        //        string strReturnValue = "";
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ParentStructuretab_Get");

        //            dynamicParameters.Add("@intGroupMarkId",obj.intGroupMarkId);
        //            dynamicParameters.Add("@intSedetailingId",obj.SEDetailingId);

        //            strReturnValue = DataAccess.DataAccess.GetScalar(dbcom);
        //            return strReturnValue;
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }
        //    public DataSet PopulatePRCDefault(DetailInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("PopulatePRCDefault");
        //            dynamicParameters.Add("@intStructureElementid", obj.intStructureElementTypeId);

        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        public List<DrainParamDepthValues_GetNewDto> DrainParamDepthValues_GetNew(DetailInfo obj)
        {
            try
            {
                IEnumerable<DrainParamDepthValues_GetNewDto> drainParamDepthValues;
                DataSet dsProjectParamDrain = new DataSet();

                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intGroupMarkID", obj.intGroupMarkId);
                    dynamicParameters.Add("@intParamSet", obj.tntParamSetNumber);
                    dynamicParameters.Add("@intDrainWidth", obj.intDrainWidth);
                    dynamicParameters.Add("@vchDrainType", obj.vchDrainType);

                    drainParamDepthValues =sqlConnection.Query<DrainParamDepthValues_GetNewDto>(SystemConstants.DrainParamDepthValues_Get_New, dynamicParameters, commandType: CommandType.StoredProcedure);
                    sqlConnection.Close();
                    return drainParamDepthValues.ToList();

                }
                
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
      
        public int DrainProductMarking_Del(DetailInfo obj)
        {
            int intReturnValue;
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intDrainProductMarkId", obj.intProductMarkId);
                    intReturnValue = sqlConnection.QueryFirstOrDefault<int>(SystemConstants.DrainProductMarkingDetails_Del, dynamicParameters, commandType: CommandType.StoredProcedure);
                    sqlConnection.Close();
                    return intReturnValue;

                }
                 
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return intReturnValue;
        }
      
        //remaining
        public string DrainWireDiameter_Get(DetailInfo obj)
        {
            string intReturnValue;
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@vchProductCode",obj.vchProductCode);
                    intReturnValue = sqlConnection.QueryFirstOrDefault<string>(SystemConstants.DrainWireDiameter_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                    sqlConnection.Close();
                    return intReturnValue;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return intReturnValue;
        }
     
        public DataSet DrainProjectParamDetails_GetNew(DetailInfo obj)
        {
            try
            {
                DataSet dsProjectParamDrain = new DataSet();

                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    //sqlConnection.Open();
                    //var dynamicParameters = new DynamicParameters();
                    //dynamicParameters.Add("@tntParamSetNumber", obj.tntParamSetNumber);
                    //dynamicParameters.Add("@intDrainWidth", obj.intDrainWidth);
                    //dynamicParameters.Add("@vchDrainType", obj.vchDrainType);

                    //dsProjectParamDrain = (DataSet)sqlConnection.Query<DataSet>(SystemConstants.ProjectParameterDrainDetails_Get_New, dynamicParameters, commandType: CommandType.StoredProcedure);

                    SqlCommand cmd = new SqlCommand(SystemConstants.ProjectParameterDrainDetails_Get_New, sqlConnection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@tntParamSetNumber", obj.tntParamSetNumber));
                    cmd.Parameters.Add(new SqlParameter("@intDrainWidth", obj.intDrainWidth));
                    cmd.Parameters.Add(new SqlParameter("@vchDrainType", obj.vchDrainType));

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dsProjectParamDrain);
                    sqlConnection.Close();
                    return dsProjectParamDrain;

                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int CheckDrainGroupMarking(DetailInfo obj)
        {
            int intReturnValue;
            try
            {

                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intParameterSet", obj.tntParamSetNumber);
                    dynamicParameters.Add("@intWidth", obj.intDrainWidth);
                    dynamicParameters.Add("@vchDrainType", obj.vchDrainType);
                    //dynamicParameters.Add("@intOut", 0);
                    dynamicParameters.Add("@intOut", null, dbType: DbType.Int32, ParameterDirection.Output);
                   
                    sqlConnection.QueryFirstOrDefault<int>(SystemConstants.DrainGroupMark_Check, dynamicParameters, commandType: CommandType.StoredProcedure);
                    intReturnValue = dynamicParameters.Get<int>("@intOut");
                    sqlConnection.Close();
                    return intReturnValue;

                }
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return intReturnValue;
        }
       
        public List<GetSWShapeCodeDto> SWShapeCode_Get_Drain(DetailInfo obj)
        {
            try
            {
                DataSet dsProjectParamDrain = new DataSet();
                IEnumerable<GetSWShapeCodeDto> getSWShapeCodeDtos;
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();

                    getSWShapeCodeDtos = sqlConnection.Query<GetSWShapeCodeDto>(SystemConstants.SWShapeCode_Get_Drain, dynamicParameters, commandType: CommandType.StoredProcedure);
                    sqlConnection.Close();
                    return getSWShapeCodeDtos.ToList();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Public s Function SWShapeCode_Get_Drain(ByVal obj As DetailInfo) As DataSet
        // Try
        // dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("SWShapeCode_Get_Mesh")
        // Return DataAccess.DataAccess.GetDataSet(dbcom)
        // Catch ex As Exception
        // Throw ex
        // End Try
        // End Function

        // Public s Function SWProductCode_Drain_Get(ByVal obj As DetailInfo) As DataSet
        // Try
        // dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("OthProductCodeDrain_Get")
        // dynamicParameters.Add("@sitProductTypeId", obj.intProjectTypeId)
        // dynamicParameters.Add("@vchProductCode", obj.vchProductCode)
        // Return DataAccess.DataAccess.GetDataSet(dbcom)
        // Catch ex As Exception
        // Throw ex
        // End Try
        // End Function
        // Public s Function OthProductCode_Get(ByVal obj As DetailInfo) As DataSet
        // Try
        // dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("OthProductCode_Get")
        // dynamicParameters.Add("@vchProductCode", obj.vchProductCode)
        // Return DataAccess.DataAccess.GetDataSet(dbcom)
        // Catch ex As Exception
        // Throw ex
        // End Try
        // End Function
        // Public s Function OthDrainOverHangs_Get(ByVal obj As DetailInfo) As DataSet
        // Try
        // dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("OthDrainOverHangs_Get")
        // dynamicParameters.Add("@intMWSpace", obj.intMWSpacing)
        // dynamicParameters.Add("@intCWSpace", obj.intCWSpacing)
        // dynamicParameters.Add("@chrShapeCode", obj.chrShapeCode)
        // Return DataAccess.DataAccess.GetDataSet(dbcom)
        // Catch ex As Exception
        // Throw ex
        // End Try
        // End Function

        // Public s Function OtherDrainProductMarkingDetails_Insert(ByVal obj As DetailInfo) As Integer
        // Dim intReturnValue As Integer
        // Try
        // dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("Oth_DrainProductMarkingDetails_Insert")
        // dynamicParameters.Add("@intStructureMarkId", obj.intStructureMarkId)
        // dynamicParameters.Add("@tntStructureMarkRevNo",obj.tntStructureMarkRevNo)
        // dynamicParameters.Add("@intProductCodeId", obj.intProductCodeId)
        // dynamicParameters.Add("@vchProductMarkingName", obj.vchProductMarkingName)
        // dynamicParameters.Add("@intShapeCodeId", obj.intShapeCodeId)
        // dynamicParameters.Add("@numInvoiceMWLength",obj.numInvoiceMWLength)
        // dynamicParameters.Add("@numInvoiceCWLength",obj.numInvoiceCWLength)
        // dynamicParameters.Add("@intInvoiceMainQty", obj.intInvoiceMainQty)
        // dynamicParameters.Add("@intInvoiceCrossQty", obj.intInvoiceCrossQty)
        // dynamicParameters.Add("@intInvoiceTotalQty", obj.intInvoiceTotalQty)
        // dynamicParameters.Add("@numProductionMWLength",obj.numProductionMWLength)
        // dynamicParameters.Add("@numProductionCWLength",obj.numProductionCWLength)
        // dynamicParameters.Add("@intProductionMainQty", obj.intProductionMainQty)
        // dynamicParameters.Add("@intProductionCrossQty", obj.intProductionCrossQty)
        // dynamicParameters.Add("@intProductionTotalQty", obj.intProductionTotalQty)
        // dynamicParameters.Add("@intInvoiceMO1", obj.intInvoiceMO1)
        // dynamicParameters.Add("@intInvoiceMO2", obj.intInvoiceMO2)
        // dynamicParameters.Add("@intInvoiceCO1", obj.intInvoiceCO1)
        // dynamicParameters.Add("@intInvoiceCO2", obj.intInvoiceCO2)
        // dynamicParameters.Add("@intProductionMO1", obj.intProductionMO1)
        // dynamicParameters.Add("@intProductionMO2", obj.intProductionMO2)
        // dynamicParameters.Add("@intProductionCO1", obj.intProductionCO1)
        // dynamicParameters.Add("@intProductionCO2", obj.intProductionCO2)
        // dynamicParameters.Add("@intMWSpacing", obj.intMWSpacing)
        // dynamicParameters.Add("@intCWSpacing", obj.intCWSpacing)
        // dynamicParameters.Add("@sitPinSize",obj.sitPinSize)
        // dynamicParameters.Add("@bitCoatingIndicator",obj.bitCoatingIndicator)
        // dynamicParameters.Add("@numConversionFactor",obj.numConversionFactor)
        // dynamicParameters.Add("@vchShapeSurcharge", obj.vchShapeSurcharge)
        // dynamicParameters.Add("@bitBendIndicator",obj.bitBendIndicator)
        // dynamicParameters.Add("@chrCalculationIndicator", obj.chrCalculationIndicator)
        // dynamicParameters.Add("@tntGenerationStatus",obj.tntGenerationStatus)
        // dynamicParameters.Add("@bitMachineCheckIndicator",obj.bitMachineCheckIndicator)
        // dynamicParameters.Add("@vchTransportCheckResult", obj.vchTransportCheckResult)
        // dynamicParameters.Add("@bitTransportIndicator",obj.bitTransportIndicator)
        // dynamicParameters.Add("@vchBendCheckResult", obj.vchBendCheckResult)
        // dynamicParameters.Add("@xmlResult", obj.xmlResult)
        // dynamicParameters.Add("@vchFilePath", obj.vchFilePath)
        // dynamicParameters.Add("@numInvoiceMWWeight",obj.numInvoiceMWWeight)
        // dynamicParameters.Add("@numInvoiceCWWeight",obj.numInvoiceCWWeight)
        // dynamicParameters.Add("@numInvoiceArea",obj.numInvoiceArea)
        // dynamicParameters.Add("@numTheoraticalWeight",obj.numTheoraticalWeight)
        // dynamicParameters.Add("@numNetWeight",obj.numNetWeight)
        // dynamicParameters.Add("@intMemberQty", obj.intMemberQty)
        // dynamicParameters.Add("@numProductionMWWeight",obj.numProductionMWWeight)
        // dynamicParameters.Add("@numProductionCWWeight",obj.numProductionCWWeight)
        // dynamicParameters.Add("@numProductionWeight",obj.numProductionWeight)
        // dynamicParameters.Add("@ParamValues", obj.ParamValues)
        // dynamicParameters.Add("@BendingPos", obj.BendingPos)
        // dynamicParameters.Add("@intShapeTransHeaderId", obj.intShapeTransHeaderId)
        // dynamicParameters.Add("@intEnvelopLength", obj.intEnvelopeLength)
        // dynamicParameters.Add("@intEnvelopWidth", obj.intEnvelopewidth)
        // dynamicParameters.Add("@intEnvelopHeight", obj.intEnvelopeHeight)
        // dynamicParameters.Add("@intStructureElementId", obj.intStructureElementTypeId)
        // dynamicParameters.Add("@intProductMarkId", obj.intProductMarkId)
        // dynamicParameters.Add("@intDrainStructureMarkID", obj.intDrainStructureMarkId)
        // 'dynamicParameters.Add("@tntDrainLayerId", DbType.Int16, obj.tntLayer)
        // 'dynamicParameters.Add("@decStartChainage",obj.decStartChainage)
        // 'dynamicParameters.Add("@decEndChainage",obj.decEndChainage)
        // dynamicParameters.Add("@vchMWBVBSString", obj.vchMWBVBSString)
        // dynamicParameters.Add("@vchCWBVBSString", obj.vchCWBVBSString)
        // dynamicParameters.Add("@intuserID", obj.intUserid)

        // intReturnValue = CType(DataAccess.DataAccess.GetScalar(dbcom), Int32)
        // Catch ex As Exception
        // Throw ex
        // End Try
        // Return intReturnValue
        // End Function

        public int DeleteAccessories(DetailInfo obj)
        {
            try
            {
                int intReturnValue;
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intSEDetailing", obj.intSELevelDetailsID);

                    intReturnValue = sqlConnection.QueryFirstOrDefault<int>(SystemConstants.CABACC_ProductMarkingDetails_Delete, dynamicParameters, commandType: CommandType.StoredProcedure);

                    sqlConnection.Close();
                    return intReturnValue;

                }
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<GetSWProductCodeDto> SWProductCode_Drain_Get(DetailInfo obj)
        {
            try
            {
                IEnumerable<GetSWProductCodeDto> getSWProducts;
                DataSet dsProjectParamDrain = new DataSet();
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    // dynamicParameters.Add("@sitProductTypeId", obj.intProjectTypeId)
                    dynamicParameters.Add("@vchProductCode",obj.vchProductCode);

                    getSWProducts = sqlConnection.Query<GetSWProductCodeDto>(SystemConstants.OthProductCodeDrain_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                    sqlConnection.Close();
                    return getSWProducts.ToList();

                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<DrainProductCodeDetailsDto> OthProductCode_Get(DetailInfo obj)
        {
            try
            {
                IEnumerable<DrainProductCodeDetailsDto> OthProductCode;
                DataSet dsProjectParamDrain = new DataSet();
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@vchProductCode",obj.vchProductCode);

                    OthProductCode = sqlConnection.Query<DrainProductCodeDetailsDto>(SystemConstants.OthProductCode_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                    sqlConnection.Close();
                    return OthProductCode.ToList();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       
        public List<GetOthDrainOverHangsDto> OthDrainOverHangs_Get(DetailInfo obj)
        {
            try
            {
                IEnumerable<GetOthDrainOverHangsDto> getOthDrainOvers;
                DataSet dsProjectParamDrain = new DataSet();
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intMWSpace", obj.intMWSpacing);
                    dynamicParameters.Add("@intCWSpace", obj.intCWSpacing);
                    dynamicParameters.Add("@chrShapeCode", obj.chrShapeCode);
                    getOthDrainOvers = sqlConnection.Query<GetOthDrainOverHangsDto>(SystemConstants.OthDrainOverHangs_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                    sqlConnection.Close();
                    return getOthDrainOvers.ToList();

                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //remaining 
        public int OtherDrainProductMarkingDetails_Update(DetailInfo obj)
        {
            int intReturnValue;
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intStructureMarkId", obj.intStructureMarkId);
                    dynamicParameters.Add("@tntStructureMarkRevNo", obj.tntStructureMarkRevNo);
                    dynamicParameters.Add("@intProductCodeId", obj.intProductCodeId);
                    dynamicParameters.Add("@vchProductMarkingName", obj.vchProductMarkingName);
                    dynamicParameters.Add("@intShapeCodeId",obj.intShapeCodeId);
                    dynamicParameters.Add("@numInvoiceMWLength",obj.numInvoiceMWLength);
                    dynamicParameters.Add("@numInvoiceCWLength",obj.numInvoiceCWLength);
                    dynamicParameters.Add("@intInvoiceMainQty",obj.intInvoiceMainQty);
                    dynamicParameters.Add("@intInvoiceCrossQty",obj.intInvoiceCrossQty);
                    dynamicParameters.Add("@intInvoiceTotalQty",obj.intInvoiceTotalQty);
                    dynamicParameters.Add("@numProductionMWLength",obj.numProductionMWLength);
                    dynamicParameters.Add("@numProductionCWLength",obj.numProductionCWLength);
                    dynamicParameters.Add("@intProductionMainQty",obj.intProductionMainQty);
                    dynamicParameters.Add("@intProductionCrossQty",obj.intProductionCrossQty);
                    dynamicParameters.Add("@intProductionTotalQty",obj.intProductionTotalQty);
                    dynamicParameters.Add("@intInvoiceMO1",obj.intInvoiceMO1);
                    dynamicParameters.Add("@intInvoiceMO2",obj.intInvoiceMO2);
                    dynamicParameters.Add("@intInvoiceCO1",obj.intInvoiceCO1);
                    dynamicParameters.Add("@intInvoiceCO2",obj.intInvoiceCO2);
                    dynamicParameters.Add("@intProductionMO1",obj.intProductionMO1);
                    dynamicParameters.Add("@intProductionMO2",obj.intProductionMO2);
                    dynamicParameters.Add("@intProductionCO1",obj.intProductionCO1);
                    dynamicParameters.Add("@intProductionCO2",obj.intProductionCO2);
                    dynamicParameters.Add("@intMWSpacing",obj.intMWSpacing);
                    dynamicParameters.Add("@intCWSpacing",obj.intCWSpacing);
                    dynamicParameters.Add("@sitPinSize",obj.sitPinSize);
                    dynamicParameters.Add("@bitCoatingIndicator",obj.bitCoatingIndicator);
                    dynamicParameters.Add("@numConversionFactor",obj.numConversionFactor);
                    dynamicParameters.Add("@vchShapeSurcharge",obj.vchShapeSurcharge);
                    dynamicParameters.Add("@bitBendIndicator",obj.bitBendIndicator);
                    dynamicParameters.Add("@chrCalculationIndicator",obj.chrCalculationIndicator);
                    dynamicParameters.Add("@tntGenerationStatus",obj.tntGenerationStatus);
                    dynamicParameters.Add("@bitMachineCheckIndicator",obj.bitMachineCheckIndicator);
                    dynamicParameters.Add("@vchTransportCheckResult",obj.vchTransportCheckResult);
                    dynamicParameters.Add("@bitTransportIndicator",obj.bitTransportIndicator);
                    dynamicParameters.Add("@vchBendCheckResult",obj.vchBendCheckResult);
                    dynamicParameters.Add("@xmlResult",obj.xmlResult);
                    dynamicParameters.Add("@vchFilePath",obj.vchFilePath);
                    dynamicParameters.Add("@numInvoiceMWWeight",obj.numInvoiceMWWeight);
                    dynamicParameters.Add("@numInvoiceCWWeight",obj.numInvoiceCWWeight);
                    dynamicParameters.Add("@numInvoiceArea",obj.numInvoiceArea);
                    dynamicParameters.Add("@numTheoraticalWeight",obj.numTheoraticalWeight);
                    dynamicParameters.Add("@numNetWeight",obj.numNetWeight);
                    dynamicParameters.Add("@intMemberQty",obj.intMemberQty);
                    dynamicParameters.Add("@numProductionMWWeight",obj.numProductionMWWeight);
                    dynamicParameters.Add("@numProductionCWWeight",obj.numProductionCWWeight);
                    dynamicParameters.Add("@numProductionWeight",obj.numProductionWeight);
                    dynamicParameters.Add("@ParamValues",obj.ParamValues);
                    dynamicParameters.Add("@BendingPos",obj.BendingPos);
                    dynamicParameters.Add("@intShapeTransHeaderId",obj.intShapeTransHeaderId);
                    dynamicParameters.Add("@intEnvelopLength",obj.intEnvelopeLength);
                    dynamicParameters.Add("@intEnvelopWidth",obj.intEnvelopewidth);
                    dynamicParameters.Add("@intEnvelopHeight",obj.intEnvelopeHeight);
                    // dynamicParameters.Add("@intStructureElementId", obj.intStructureElementTypeId)
                    dynamicParameters.Add("@intProductMarkId",obj.intProductMarkId);
                    dynamicParameters.Add("@intDrainStructureMarkID",obj.intDrainStructureMarkId);
                    dynamicParameters.Add("@vchMWBVBSString",obj.vchMWBVBSString);
                    dynamicParameters.Add("@vchCWBVBSString",obj.vchCWBVBSString);
                    dynamicParameters.Add("@intuserID",obj.intUserid);
                    dynamicParameters.Add("@nvchProduceIndicator",obj.nvchProduceIndicator);

                    intReturnValue = sqlConnection.QueryFirstOrDefault<int>(SystemConstants.Oth_DrainProductMarkingDetails_Update, dynamicParameters, commandType: CommandType.StoredProcedure);
                    sqlConnection.Close();
                    return intReturnValue;

                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return intReturnValue;
        }

        public int OtherDrainProductMarkingDetails_Insert(DetailInfo obj)
        {
            int intReturnValue;
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();

                    dynamicParameters.Add("@intStructureMarkId", obj.intStructureMarkId);
                    dynamicParameters.Add("@tntStructureMarkRevNo",obj.tntStructureMarkRevNo);
                    dynamicParameters.Add("@intProductCodeId",obj.intProductCodeId);
                    dynamicParameters.Add("@vchProductMarkingName",obj.vchProductMarkingName);
                    dynamicParameters.Add("@intShapeCodeId",obj.intShapeCodeId);
                    dynamicParameters.Add("@numInvoiceMWLength",obj.numInvoiceMWLength);
                    dynamicParameters.Add("@numInvoiceCWLength",obj.numInvoiceCWLength);
                    dynamicParameters.Add("@intInvoiceMainQty",obj.intInvoiceMainQty);
                    dynamicParameters.Add("@intInvoiceCrossQty",obj.intInvoiceCrossQty);
                    dynamicParameters.Add("@intInvoiceTotalQty",obj.intInvoiceTotalQty);
                    dynamicParameters.Add("@numProductionMWLength",obj.numProductionMWLength);
                    dynamicParameters.Add("@numProductionCWLength",obj.numProductionCWLength);
                    dynamicParameters.Add("@intProductionMainQty",obj.intProductionMainQty);
                    dynamicParameters.Add("@intProductionCrossQty",obj.intProductionCrossQty);
                    dynamicParameters.Add("@intProductionTotalQty",obj.intProductionTotalQty);
                    dynamicParameters.Add("@intInvoiceMO1",obj.intInvoiceMO1);
                    dynamicParameters.Add("@intInvoiceMO2",obj.intInvoiceMO2);
                    dynamicParameters.Add("@intInvoiceCO1",obj.intInvoiceCO1);
                    dynamicParameters.Add("@intInvoiceCO2",obj.intInvoiceCO2);
                    dynamicParameters.Add("@intProductionMO1",obj.intProductionMO1);
                    dynamicParameters.Add("@intProductionMO2",obj.intProductionMO2);
                    dynamicParameters.Add("@intProductionCO1",obj.intProductionCO1);
                    dynamicParameters.Add("@intProductionCO2",obj.intProductionCO2);
                    dynamicParameters.Add("@intMWSpacing",obj.intMWSpacing);
                    dynamicParameters.Add("@intCWSpacing",obj.intCWSpacing);
                    dynamicParameters.Add("@sitPinSize",obj.sitPinSize);
                    dynamicParameters.Add("@bitCoatingIndicator",obj.bitCoatingIndicator);
                    dynamicParameters.Add("@numConversionFactor",obj.numConversionFactor);
                    dynamicParameters.Add("@vchShapeSurcharge", obj.vchShapeSurcharge);
                    dynamicParameters.Add("@bitBendIndicator",obj.bitBendIndicator);
                    dynamicParameters.Add("@chrCalculationIndicator", obj.chrCalculationIndicator);
                    dynamicParameters.Add("@tntGenerationStatus",obj.tntGenerationStatus);
                    dynamicParameters.Add("@bitMachineCheckIndicator",obj.bitMachineCheckIndicator);
                    dynamicParameters.Add("@vchTransportCheckResult", obj.vchTransportCheckResult);
                    dynamicParameters.Add("@bitTransportIndicator",obj.bitTransportIndicator);
                    dynamicParameters.Add("@vchBendCheckResult", obj.vchBendCheckResult);
                    dynamicParameters.Add("@xmlResult", obj.xmlResult);
                    dynamicParameters.Add("@vchFilePath", obj.vchFilePath);
                    dynamicParameters.Add("@numInvoiceMWWeight",obj.numInvoiceMWWeight);
                    dynamicParameters.Add("@numInvoiceCWWeight",obj.numInvoiceCWWeight);
                    dynamicParameters.Add("@numInvoiceArea",obj.numInvoiceArea);
                    dynamicParameters.Add("@numTheoraticalWeight",obj.numTheoraticalWeight);
                    dynamicParameters.Add("@numNetWeight",obj.numNetWeight);
                    dynamicParameters.Add("@intMemberQty", obj.intMemberQty);
                    dynamicParameters.Add("@numProductionMWWeight",obj.numProductionMWWeight);
                    dynamicParameters.Add("@numProductionCWWeight",obj.numProductionCWWeight);
                    dynamicParameters.Add("@numProductionWeight",obj.numProductionWeight);
                    dynamicParameters.Add("@ParamValues", obj.ParamValues);
                    dynamicParameters.Add("@BendingPos", obj.BendingPos);
                    dynamicParameters.Add("@intShapeTransHeaderId",obj.intShapeTransHeaderId);
                    dynamicParameters.Add("@intEnvelopLength",obj.intEnvelopeLength);
                    dynamicParameters.Add("@intEnvelopWidth",obj.intEnvelopewidth);
                    dynamicParameters.Add("@intEnvelopHeight", obj.intEnvelopeHeight);
                    // dynamicParameters.Add("@intStructureElementId", obj.intStructureElementTypeId)
                    dynamicParameters.Add("@intProductMarkId", obj.intProductMarkId);
                    dynamicParameters.Add("@intDrainStructureMarkID",obj.intDrainStructureMarkId);
                    // dynamicParameters.Add("@tntDrainLayerId", DbType.Int16, obj.tntLayer)
                    // dynamicParameters.Add("@decStartChainage",obj.decStartChainage)
                    // dynamicParameters.Add("@decEndChainage",obj.decEndChainage)
                    dynamicParameters.Add("@vchMWBVBSString", obj.vchMWBVBSString);
                    dynamicParameters.Add("@vchCWBVBSString", obj.vchCWBVBSString);
                    dynamicParameters.Add("@intuserID",obj.intUserid);
                    dynamicParameters.Add("@nvchProduceIndicator", obj.nvchProduceIndicator);

                    intReturnValue = sqlConnection.QueryFirstOrDefault<int>(SystemConstants.Oth_DrainProductMarkingDetails_Insert, dynamicParameters, commandType: CommandType.StoredProcedure);
                    sqlConnection.Close();
                    return intReturnValue;

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return intReturnValue;
        }

        public int DrainOth_InsertShapeDetails(DetailInfo obj)
        {
            int intReturnValue = 0;
            try
            {

                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intShapeTransHeaderId", obj.intShapeTransHeaderId);
                    dynamicParameters.Add("@intShapeId", obj.intShapeId);
                    dynamicParameters.Add("@vchSequence", obj.vchSequence);
                    dynamicParameters.Add("@vchCriticalIndicator", obj.vchCriticalIndicator);
                    dynamicParameters.Add("@vchParamValues", obj.nvchParamValues);
                    dynamicParameters.Add("@intUserId", obj.intUserid);
                    dynamicParameters.Add("@intOutput", 0);
                    intReturnValue = sqlConnection.QueryFirstOrDefault<int>(SystemConstants.DrainOth_ShapeCodeDetails_Insert, dynamicParameters, commandType: CommandType.StoredProcedure);
                    //intReturnValue = (dbcom.Parameters["@intOut"].Value);

                    sqlConnection.Close();
                    return intReturnValue;

                }
                
            
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return intReturnValue;
        }

        public List<OtherProductMarkingDetailsDto> DrainOtherProductMarkingDetails_Get(DetailInfo obj)
        {
            try
            {
                IEnumerable<OtherProductMarkingDetailsDto> otherProductMarkings;
                DataSet dsProjectParamDrain = new DataSet();
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intStructureMarkId", obj.intStructureMarkId);

                    otherProductMarkings = sqlConnection.Query<OtherProductMarkingDetailsDto>(SystemConstants.DrainOth_ProductMarking_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                    sqlConnection.Close();
                    return otherProductMarkings.ToList();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       
        public List<PRCEnvelopeDetailsDto> PRCEnvelopeDetails_Get(DetailInfo obj)
        {
            try
            {
                IEnumerable<PRCEnvelopeDetailsDto> pRCEnvelopeDetails;
                DataSet dsProjectParamDrain = new DataSet();
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intStructureElementTypeId", obj.intStructureElementTypeId);

                    pRCEnvelopeDetails =sqlConnection.Query<PRCEnvelopeDetailsDto>(SystemConstants.PRCEnvelopeDetailsMaster_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                    sqlConnection.Close();
                    return pRCEnvelopeDetails.ToList();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetPostedWBSForGroupMark(DetailInfo obj)
        {
            try
            {
                DataSet dsProjectParamDrain = new DataSet();
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intGroupMarkId", obj.intGroupMarkId);
                    dynamicParameters.Add("@intStructureElementTypeId", obj.intStructureElementTypeId);
                    dynamicParameters.Add("@sitProductTypeId", obj.sitProductTypeId);

                    dsProjectParamDrain = (DataSet)sqlConnection.Query<DataSet>(SystemConstants.WBSAttachedWithGroupMarking_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                    sqlConnection.Close();
                    return dsProjectParamDrain;

                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet DrainParamReport_Get(DetailInfo obj)
        {
            try
            {
                DataSet dsProjectParamDrain = new DataSet();
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@tntParamSetNumber", obj.intParameterSet);

                    dsProjectParamDrain = (DataSet)sqlConnection.Query<DataSet>(SystemConstants.DrainParamReport_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                    sqlConnection.Close();
                    return dsProjectParamDrain;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // ' Abhijeet >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        public DataSet DrainProductMark_Get(DetailInfo obj)
        {
            IEnumerable<GetDrainProdMarkDto> getDrainProdMarkId;
            DataSet ds = new DataSet();
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intStructureMarkId", obj.intStructureMarkId);
                    ds = (DataSet)sqlConnection.Query<GetDrainProdMarkDto>(SystemConstants.DrainProductMarkingID_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                    sqlConnection.Close();
                    return ds;

                }
              
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }


        public int UpdateStructureMarkingDetails(int structureMarkID, string structuremarkingname, out string errormsg)
        {
            int intReturnValue=0;
            errormsg = "";
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intStructureMarkID", structureMarkID);
                    dynamicParameters.Add("@vchStructureMarkName", structuremarkingname);
                    dynamicParameters.Add("@intOutput", null, dbType: DbType.Int32, ParameterDirection.Output);
                    sqlConnection.QueryFirstOrDefault<int>(SystemConstants.Update_MeshStructureMark, dynamicParameters, commandType: CommandType.StoredProcedure);

                    intReturnValue = dynamicParameters.Get<int>("@intOutput");
                    sqlConnection.Close();
                    return intReturnValue;

                }


            }
            catch (Exception ex)
            {
                errormsg = "Failed to update the marking name";
                return intReturnValue;
            }
        }


    }

}
