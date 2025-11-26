using Microsoft.VisualBasic;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using ShapeCodeService.Interfaces;
using ShapeCodeService.Constants;
using Dapper;
using ShapeCodeService.Dtos;
using ShapeCodeService.Models;
using static Dapper.SqlMapper;

namespace ShapeCodeService.Repositories
{
    public class CABDAL : ICABDAL
    {

        private readonly IConfiguration _configuration;
        private string connectionString = "Server=NSQADB5\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=MES_USER;Password=Mes@123;Connection Timeout=36000000";
        //private string connectionString = "Server=nsprddb10\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=ndswebapps;Password=DBAdmin4*NDS;Connection Timeout=36000000";
        public CABDAL()
        {
        }

        //    public int InsUpdWBSDetails(CABInfo obj)
        //    {
        //        Int32 intReturnValue = 0;
        //        try
        //        {

        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BCDWBSGroupInsUpd");
        //            dynamicParameters.Add("@vchWBSElementId", obj.vchWBSElementIds);
        //            dynamicParameters.Add("@intProjectid", obj.intProjectId);
        //            dynamicParameters.Add("@intGroupMarkid", obj.intGroupMarkId);
        //            dynamicParameters.Add("@Output", 0);

        //            intReturnValue = DataAccess.DataAccess.GetScalar(dbcom); // (dbcom.Parameters("@Output").Value)
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        return intReturnValue;
        //    }
        //    public DataSet GetSAPMaterialId(CABInfo obj)
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


        //            var strStructureElementType = obj.vchStructureElementType.ToLower;
        //            if (strStructureElementType == "slab")
        //                strStructureElementType = "wall";

        //            if (!obj.vchStructureElementType == null)
        //            {
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
        //        // dynamicParameters.Add("@vchStructureElement",  obj.vchStructureElementType)
        //        // Return DataAccess.DataAccess.GetDataSet(dbcom)
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }
        //    public DataSet GetPRCDetails(CABInfo obj)
        //    {
        //        try
        //        {
        //            DataSet ds = new DataSet();
        //            using (var sqlConnection = new SqlConnection(connectionString))
        //            {
        //                sqlConnection.Open();
        //                var dynamicParameters = new DynamicParameters();
        //                dynamicParameters.Add("@intSEDetailingId", obj.SEDetailingId);
        //                dynamicParameters.Add("@bitParentFlag", obj.ParentFlag);
        //                ds = (DataSet)sqlConnection.Query<DataSet>(SystemConstants.BCDPRCDetails_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
        //                return ds;

        //            }

        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public DataSet GetCABTransportMode()
        //    {
        //        try
        //        {
        //            DataSet ds = new DataSet();
        //            using (var sqlConnection = new SqlConnection(connectionString))
        //            {
        //                sqlConnection.Open();

        //                ds = (DataSet)sqlConnection.Query<DataSet>(SystemConstants.CABTransportMode_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
        //                return ds;

        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;

        //        }
        //    }

        //    public DataSet GetCABTransportCheck()
        //    {
        //        try
        //        {
        //            DataSet ds = new DataSet();
        //            using (var sqlConnection = new SqlConnection(connectionString))
        //            {
        //                sqlConnection.Open();

        //                ds = (DataSet)sqlConnection.Query<DataSet>(SystemConstants.CAB_TransportMaster_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
        //                return ds;

        //            }

        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public DataSet GetStructureType(CABInfo obj)
        //    {
        //        try
        //        {
        //            DataSet ds = new DataSet();
        //            using (var sqlConnection = new SqlConnection(connectionString))
        //            {
        //                sqlConnection.Open();
        //                var dynamicParameters = new DynamicParameters();
        //                dynamicParameters.Add("@intStructureElementTypeId", obj.intStructureElementTypeId);

        //                ds = (DataSet)sqlConnection.Query<DataSet>(SystemConstants.StructureElementType_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
        //                return ds;

        //            }


        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }


        //    public DataSet GetAccProductMarkingDetails(CABInfo obj)
        //    {
        //        try
        //        {
        //            DataSet ds = new DataSet();
        //            using (var sqlConnection = new SqlConnection(connectionString))
        //            {
        //                sqlConnection.Open();
        //                var dynamicParameters = new DynamicParameters();
        //                // dynamicParameters.Add("@intSEDetailingId", obj.SEDetailingId)
        //                // dynamicParameters.Add("@intStructureMarkId", obj.intStructureMarkId)
        //                // dynamicParameters.Add("@intGroupMarkingID", obj.intGroupMarkId)
        //                dynamicParameters.Add("@intSEDetailingId", obj.SEDetailingId); // ' july 20 
        //                dynamicParameters.Add("@intStructureMarkId", obj.intStructureMarkId);
        //                ds = (DataSet)sqlConnection.Query<DataSet>(SystemConstants.CABACC_ProductMarkingDetails_GET, dynamicParameters, commandType: CommandType.StoredProcedure);
        //                return ds;

        //            }

        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public DataSet GetAccSAPMaterialDetails(CABInfo obj)
        //    {
        //        try
        //        {
        //            DataSet ds = new DataSet();
        //            using (var sqlConnection = new SqlConnection(connectionString))
        //            {
        //                sqlConnection.Open();
        //                var dynamicParameters = new DynamicParameters();
        //                dynamicParameters.Add("@vchSAPCode", obj.SAPMaterialCode);

        //                ds = (DataSet)sqlConnection.Query<DataSet>(SystemConstants.ACC_SAPMaterial_GET, dynamicParameters, commandType: CommandType.StoredProcedure);
        //                return ds;

        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }


        //    public DataSet GetHeaderInfo(CABInfo obj)
        //    {
        //        try
        //        {
        //            DataSet ds = new DataSet();
        //            using (var sqlConnection = new SqlConnection(connectionString))
        //            {
        //                sqlConnection.Open();
        //                var dynamicParameters = new DynamicParameters();
        //                dynamicParameters.Add("@intProjectId", obj.intProjectId);
        //                ds = (DataSet)sqlConnection.Query<DataSet>(SystemConstants.BCDHeaderInfo_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
        //                return ds;

        //            }

        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;

        //        }
        //    }


        //    public DataSet GetProductAccessoriesDetail(CABInfo obj)
        //    {
        //        try
        //        {
        //            SqlCacheDependency SqlDep;
        //            DataSet dsCache = (DataSet)HttpRuntime.Cache["Cacheshapemaster"];
        //            if (dsCache == null)
        //            {
        //                SqlDep = new SqlCacheDependency("NDSCaching", "sapmaterialmaster");
        //                dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ACC_ProductMarkingDetails_GET");
        //                dsCache = DataAccess.DataAccess.GetDataSet(dbcom);
        //                HttpRuntime.Cache.Insert("Cacheshapemaster", dsCache, SqlDep);
        //            }
        //            return dsCache;
        //        }
        //        // dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ACC_ProductMarkingDetails_GET")
        //        // dynamicParameters.Add("@intSEDetailingId", obj.SEDetailingId)
        //        // dynamicParameters.Add("@intStructureMarkId", obj.intStructureMarkId)
        //        // Return DataAccess.DataAccess.GetDataSet(dbcom)
        //        catch (Exception ex)
        //        {
        //            throw ex;

        //        }
        //    }

        //    public DataSet GetAccSAPDetails(CABInfo obj)
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

        //    public DataSet GetGroupMark(CABInfo obj)
        //    {
        //        try
        //        {
        //            DataSet ds = new DataSet();
        //            using (var sqlConnection = new SqlConnection(connectionString))
        //            {
        //                sqlConnection.Open();
        //                var dynamicParameters = new DynamicParameters();
        //                dynamicParameters.Add("@intProjectID", obj.intProjectId);
        //                ds = (DataSet)sqlConnection.Query<DataSet>(SystemConstants.BCDGroupMark_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
        //                return ds;

        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;

        //        }
        //    }


        //    public DataSet GetConsignmentType(CABInfo obj)
        //    {
        //        try
        //        {
        //            DataSet ds = new DataSet();
        //            using (var sqlConnection = new SqlConnection(connectionString))
        //            {
        //                sqlConnection.Open();
        //                var dynamicParameters = new DynamicParameters();
        //                // dynamicParameters.Add("@intProjectID", obj.intProjectId)
        //                ds = (DataSet)sqlConnection.Query<DataSet>(SystemConstants.BCDConsignment_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
        //                return ds;

        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;

        //        }
        //    }

        //    public DataSet GetWBSList(CABInfo obj)
        //    {
        //        try
        //        {
        //            DataSet ds = new DataSet();
        //            using (var sqlConnection = new SqlConnection(connectionString))
        //            {
        //                sqlConnection.Open();
        //                var dynamicParameters = new DynamicParameters();
        //                dynamicParameters.Add("@vchInputValueParameter", obj.vchInputValueParameter);
        //                ds = (DataSet)sqlConnection.Query<DataSet>(SystemConstants.BCDWBSList_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
        //                return ds;

        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;

        //        }
        //    }

        //    public DataSet GetWBSListColumn(CABInfo obj)
        //    {
        //        try
        //        {
        //            DataSet ds = new DataSet();
        //            using (var sqlConnection = new SqlConnection(connectionString))
        //            {
        //                sqlConnection.Open();
        //                var dynamicParameters = new DynamicParameters();
        //                dynamicParameters.Add("@vchInputValueParameter", obj.vchInputValueParameter);

        //                ds = (DataSet)sqlConnection.Query<DataSet>(SystemConstants.BCDWBSListColumn_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
        //                return ds;

        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;

        //        }
        //    }

        //    public int GetGroupMarkIsExist(CABInfo obj)
        //    {
        //        try
        //        {
        //            int strReturnValue;

        //            using (var sqlConnection = new SqlConnection(connectionString))
        //            {
        //                sqlConnection.Open();
        //                var dynamicParameters = new DynamicParameters();
        //                dynamicParameters.Add("@vchGroupMarkingName", obj.vchGroupMarkingName);
        //                dynamicParameters.Add("@intProject", obj.intProjectId);
        //                dynamicParameters.Add("@vchStructureElement", obj.vchStructureElementType);
        //                dynamicParameters.Add("@vchProductType", obj.vchProductType);
        //                strReturnValue = sqlConnection.QueryFirstOrDefault<int>(SystemConstants.BCDChkGrpMarkExist_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
        //                return strReturnValue;

        //            }


        //            return (int)Interaction.IIf(strReturnValue.Equals("") | strReturnValue.Equals(null), -1, (int)strReturnValue);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public int GetStructElementIsExist(CABInfo obj)
        //    {
        //        try
        //        {
        //            string strReturnValue;
        //            DataSet ds = new DataSet();
        //            using (var sqlConnection = new SqlConnection(connectionString))
        //            {
        //                sqlConnection.Open();
        //                var dynamicParameters = new DynamicParameters();
        //                dynamicParameters.Add("@intGroupMarkingId", obj.intGroupMarkId);
        //                dynamicParameters.Add("@vchStructureElementName", obj.vchStructureElementType);
        //                strReturnValue = sqlConnection.QueryFirstOrDefault<string>(SystemConstants.BCDChkStructElementExist_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
        //                return Convert.ToInt32(strReturnValue);

        //            }

        //            return (int)Interaction.IIf(strReturnValue.Equals(""), -1, strReturnValue);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }


        //    public DataSet GetPageInfoFromGroupMarkID(CABInfo obj)
        //    {
        //        try
        //        {
        //            DataSet ds = new DataSet();
        //            using (var sqlConnection = new SqlConnection(connectionString))
        //            {
        //                sqlConnection.Open();
        //                var dynamicParameters = new DynamicParameters();
        //                dynamicParameters.Add("@intGroupMarkId", obj.intGroupMarkId);

        //                ds = (DataSet)sqlConnection.Query<DataSet>(SystemConstants.BeamGroupMarkingDetails_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
        //                return ds;

        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }


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


        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public DataSet GetWBSElementByGroupMarkId(CABInfo obj)
        //    {
        //        try
        //        {
        //            DataSet ds = new DataSet();
        //            using (var sqlConnection = new SqlConnection(connectionString))
        //            {
        //                sqlConnection.Open();
        //                var dynamicParameters = new DynamicParameters();
        //                dynamicParameters.Add("@intGroupMarkId", obj.intGroupMarkId);

        //                ds = (DataSet)sqlConnection.Query<DataSet>(SystemConstants.BCDWBSElementByGroupMarkId_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
        //                return ds;

        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public DataSet GetCABDetails(CABInfo obj)
        //    {
        //        try
        //        {
        //            DataSet ds = new DataSet();
        //            using (var sqlConnection = new SqlConnection(connectionString))
        //            {
        //                sqlConnection.Open();
        //                var dynamicParameters = new DynamicParameters();
        //                dynamicParameters.Add("@intSEDetailingId", obj.SEDetailingId);
        //                dynamicParameters.Add("@intStructureMarkId", obj.intStructureMarkId);
        //                ds = (DataSet)sqlConnection.Query<DataSet>(SystemConstants.CABCAB_ProductMarkingDetails_GET, dynamicParameters, commandType: CommandType.StoredProcedure);
        //                return ds;

        //            }

        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;

        //        }
        //    }


        //    public int UpdateBeamCageAccess(CABInfo obj)
        //    {
        //        try
        //        {
        //            int intReturnValue;
        //            using (var sqlConnection = new SqlConnection(connectionString))
        //            {
        //                sqlConnection.Open();
        //                var dynamicParameters = new DynamicParameters();
        //                dynamicParameters.Add("@intAccessoriesId", obj.intAccessoriesId);
        //                dynamicParameters.Add("@intItemNo", obj.intItemNo);
        //                dynamicParameters.Add("@vchMark", obj.vchMark);
        //                dynamicParameters.Add("@vchProductCode", obj.vchProductCode);
        //                dynamicParameters.Add("@intQty", obj.intQty);
        //                dynamicParameters.Add("@vchUOM", obj.vchUOM);
        //                dynamicParameters.Add("@numWeight", obj.numWeight);
        //                dynamicParameters.Add("@vchBillType", obj.vchBillType);
        //                dynamicParameters.Add("@vchShape", obj.vchShape);
        //                dynamicParameters.Add("@intParentProductMarkid", obj.ParentProductId);

        //                intReturnValue = sqlConnection.QueryFirstOrDefault<int>(SystemConstants.BCDProductAccessorDetails_Update, dynamicParameters, commandType: CommandType.StoredProcedure);
        //                return intReturnValue;

        //            }

        //        }

        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public int InsertBeamCageAccess(CABInfo obj)
        //    {
        //        try
        //        {
        //            int intReturnValue;
        //            using (var sqlConnection = new SqlConnection(connectionString))
        //            {
        //                sqlConnection.Open();
        //                var dynamicParameters = new DynamicParameters();
        //                dynamicParameters.Add("@intSEDetailingId", obj.SEDetailingId);
        //                dynamicParameters.Add("@intStructureMarkId", obj.intStructureMarkId);

        //                dynamicParameters.Add("@intItemNo", obj.intItemNo);
        //                dynamicParameters.Add("@vchMark", obj.vchMark);
        //                dynamicParameters.Add("@vchProductCode", obj.vchProductCode);
        //                dynamicParameters.Add("@intQty", obj.intQty);
        //                dynamicParameters.Add("@vchUOM", obj.vchUOM);
        //                dynamicParameters.Add("@numWeight", obj.numWeight);
        //                dynamicParameters.Add("@vchBillType", obj.vchBillType);
        //                dynamicParameters.Add("@vchShape", obj.vchShape);
        //                dynamicParameters.Add("@intParentProductMarkid", obj.ParentProductId);
        //                intReturnValue = sqlConnection.QueryFirstOrDefault<int>(SystemConstants.BCDProductAccessorDetails_Insert, dynamicParameters, commandType: CommandType.StoredProcedure);
        //                return intReturnValue;

        //            }

        //        }

        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public int InsertAccessories(CABInfo obj)
        //    {
        //        try
        //        {
        //            int intReturnValue;

        //            using (var sqlConnection = new SqlConnection(connectionString))
        //            {
        //                sqlConnection.Open();
        //                var dynamicParameters = new DynamicParameters();
        //                dynamicParameters.Add("@vchAccProductMarkingName", obj.vchProductMarkingName);
        //                dynamicParameters.Add("@intQty", obj.intQty);
        //                // dynamicParameters.Add("@intStructureMarkID",  obj.intStructureMarkId)
        //                // dynamicParameters.Add("@intSEDetailing", obj.SEDetailingId)
        //                dynamicParameters.Add("@intSEDetailing", obj.intSELevelDetailsID);
        //                dynamicParameters.Add("@intNoOfPieces", obj.intNoEach);
        //                dynamicParameters.Add("@vchSAPCode", obj.SAPMaterialCode);
        //                dynamicParameters.Add("@vchCABProductMarkName", obj.vchBarMark);
        //                // dynamicParameters.Add("@vchStructureMarkName",  obj.vchStructureMarkingName)
        //                dynamicParameters.Add("@vchProductType", obj.vchStructureElementType);
        //                dynamicParameters.Add("@intGroupMarkingID", obj.intGroupMarkId);
        //                dynamicParameters.Add("@bitIscoupler", obj.IsbitCoupler);
        //                dynamicParameters.Add("@numExternalLength", obj.numExternalLength);
        //                dynamicParameters.Add("@numExternalWidth", obj.numExternalWidth);
        //                dynamicParameters.Add("@numExternalHeight", obj.numExternalHeight);
        //                dynamicParameters.Add("@numInvoiceWeightPerPC", obj.numInvoiceWeightPerPC);
        //                dynamicParameters.Add("@numLength", obj.numLength);
        //                intReturnValue = sqlConnection.QueryFirstOrDefault<int>(SystemConstants.CABACC_ProductMarkingDetails_Insert, dynamicParameters, commandType: CommandType.StoredProcedure);
        //                return intReturnValue;

        //            }

        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public int DeleteAccessories(CABInfo obj)
        //    {
        //        try
        //        {
        //            int intReturnValue;

        //            using (var sqlConnection = new SqlConnection(connectionString))
        //            {
        //                sqlConnection.Open();
        //                var dynamicParameters = new DynamicParameters();
        //                dynamicParameters.Add("@intSEDetailing", obj.intSELevelDetailsID);

        //                intReturnValue = sqlConnection.QueryFirstOrDefault<int>(SystemConstants.CABACC_ProductMarkingDetails_Delete, dynamicParameters, commandType: CommandType.StoredProcedure);
        //                return intReturnValue;

        //            }

        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public int InsertGroupMarking(CABInfo obj)
        //    {
        //        try
        //        {
        //            int intReturnValue;
        //            using (var sqlConnection = new SqlConnection(connectionString))
        //            {
        //                sqlConnection.Open();
        //                var dynamicParameters = new DynamicParameters();
        //                dynamicParameters.Add("@intGroupMarkId", obj.intGroupMarkId);
        //                dynamicParameters.Add("@tntGroupRevNo", obj.tntGroupRevNo);
        //                dynamicParameters.Add("@intProjectId", obj.intProjectId);
        //                dynamicParameters.Add("@intWBSTypeId", obj.intWBSTypeId);
        //                dynamicParameters.Add("@intStructureElementTypeId", obj.intStructureElementTypeId);
        //                dynamicParameters.Add("@sitProductTypeId", obj.sitProductTypeId);
        //                dynamicParameters.Add("@intConsignmentType", obj.intConsignmentType);
        //                dynamicParameters.Add("@vchGroupMarkingName", obj.vchGroupMarkingName);
        //                dynamicParameters.Add("@tntParamSetNumber", obj.tntParamSetNumber);
        //                dynamicParameters.Add("@vchRemarks", obj.vchRemarks);
        //                dynamicParameters.Add("@vchCopyFrom", obj.vchCopyFrom);
        //                dynamicParameters.Add("@vchDrawingReference", obj.vchDrawingReference);
        //                dynamicParameters.Add("@chrDrawingVersion", obj.chrDrawingVersion);
        //                dynamicParameters.Add("@vchDrawingRemarks", obj.vchDrawingRemarks);
        //                dynamicParameters.Add("@tntStatusId", obj.tntStatusId);
        //                dynamicParameters.Add("@bitParentFlag", obj.ParentFlag);
        //                dynamicParameters.Add("@tntTransportId", obj.intTransport);
        //                dynamicParameters.Add("@intCreatedUId", obj.intUserid);
        //                intReturnValue = sqlConnection.QueryFirstOrDefault<int>(SystemConstants.CABGroupMarking_Insert, dynamicParameters, commandType: CommandType.StoredProcedure);
        //                return intReturnValue;

        //            }

        //        }

        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public int InsertWbsDetailing(CABInfo obj)
        //    {
        //        try
        //        {
        //            int intReturnValue;
        //            using (var sqlConnection = new SqlConnection(connectionString))
        //            {
        //                sqlConnection.Open();
        //                var dynamicParameters = new DynamicParameters();
        //                dynamicParameters.Add("@intGroupMarkId", obj.intGroupMarkId);
        //                dynamicParameters.Add("@tntGroupRevNo", obj.tntGroupRevNo);
        //                dynamicParameters.Add("@intWBSElementId", obj.intWBSElementId);
        //                dynamicParameters.Add("@vchDrawingReference", obj.vchDrawingReference);
        //                dynamicParameters.Add("@chrDrawingVersion", obj.chrDrawingVersion);

        //                dynamicParameters.Add("@vchDrawingRemarks", obj.vchDrawingRemarks);
        //                dynamicParameters.Add("@tntStatusId", obj.tntStatusId);
        //                intReturnValue = sqlConnection.QueryFirstOrDefault<int>(SystemConstants.BCDWBSDetails_Insert, dynamicParameters, commandType: CommandType.StoredProcedure);
        //                return intReturnValue;

        //            }


        //        }

        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public int DeleteWbsDetailing(CABInfo obj)
        //    {
        //        try
        //        {
        //            int intReturnValue;

        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BCDWBSList_Delete");
        //            dynamicParameters.Add("@intGroupMarkId", obj.intGroupMarkId);

        //            intReturnValue = DataAccess.DataAccess.ExecuteNonQuery(dbcom);
        //            return intReturnValue;
        //        }

        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public string[] GetGroupMarkingByID(CABInfo obj)
        //    {
        //        try
        //        {
        //            string[] strReturnValue = new string[3];

        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BCDGroupMarkNameById_Get");
        //            dynamicParameters.Add("@intGroupMarkId", obj.intGroupMarkId);

        //            dynamicParameters.Add("@vchGroupMarkingName", 100);
        //            dynamicParameters.Add("@tntGroupRevno", 0);
        //            dynamicParameters.Add("@vchCopyFrom", 200);

        //            DataAccess.DataAccess.GetScalar(dbcom);

        //            // strReturnValue(0) = (dbcom.Parameters("@vchGroupMarkingName").Value)
        //            strReturnValue[0] = Interaction.IIf(Information.IsDBNull(dbcom.Parameters["@vchGroupMarkingName"].Value), "", dbcom.Parameters["@vchGroupMarkingName"].Value);
        //            strReturnValue[1] = Interaction.IIf(Information.IsDBNull(dbcom.Parameters["@tntGroupRevno"].Value), "0", dbcom.Parameters["@tntGroupRevno"].Value);
        //            strReturnValue[2] = Interaction.IIf(Information.IsDBNull(dbcom.Parameters["@vchCopyFrom"].Value), "N/A", dbcom.Parameters["@vchCopyFrom"].Value);
        //            // strReturnValue(3) = IIf(IsDBNull(dbcom.Parameters("@vchRemarks").Value), " ", dbcom.Parameters("@vchRemarks").Value)

        //            return strReturnValue;
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }


        //    public DataSet GetTransportMode(CABInfo obj)
        //    {
        //        try
        //        {
        //            int intReturnValue = 0;
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("TransportMode_Get");
        //            dynamicParameters.Add("@intGroupMarkId", obj.intGroupMarkId);
        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public int InsertBeamPRCDetails(CABInfo obj)
        //    {
        //        try
        //        {
        //            int intReturnValue;
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BCDPRCDetails_Insert");

        //            dynamicParameters.Add("@intSEDetailingId", obj.SEDetailingId);  // ' modfied here 
        //            dynamicParameters.Add("@vchStructureMarkingName", obj.vchStructureMarkingName);


        //            dynamicParameters.Add("@numBeamWidth", obj.numBeamWidth);
        //            dynamicParameters.Add("@numBeamDepth", obj.numBeamDepth);
        //            dynamicParameters.Add("@intClearSpan", obj.intClearSpan);

        //            dynamicParameters.Add("@intSAPMaterialcodeId", obj.SAPMAterialCodeid);
        //            dynamicParameters.Add("@bitAssIndicator", obj.bitAssemblyIndicator);
        //            dynamicParameters.Add("@vchRemarks", obj.vchRemarks);
        //            dynamicParameters.Add("@intStructureMarkid", obj.intStructureMarkId);
        //            // intReturnValue = CType(DataAccess.DataAccess.GetScalar(dbcom), Int32)

        //            DataAccess.DataAccess.GetScalar(dbcom);
        //            intReturnValue = (dbcom.Parameters["@intStructureMarkid"].Value);
        //            return intReturnValue;
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    /// <summary>
        //    ///     ''' 
        //    ///     ''' </summary>
        //    ///     ''' <param name="obj"></param>
        //    ///     ''' <returns></returns>
        //    ///     ''' <remarks></remarks>


        //    public int DelCabAccess(CABInfo obj)
        //    {
        //        try
        //        {
        //            int intReturnValue;
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BCDBeamCageAccessories_Del");
        //            dynamicParameters.Add("@intAccProductMarkID", obj.intAccessoriesId);
        //            intReturnValue = DataAccess.DataAccess.GetScalar(dbcom);
        //            return intReturnValue;
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }




        //    public DataSet GetGroupMarkingDrawingReference(CABInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("GroupMarkingDrawingReference_Get");
        //            dynamicParameters.Add("@intgroupmarkingdrawingid", obj.intGroupMarkId);
        //            dynamicParameters.Add("@intProjectId", obj.intProjectId);
        //            dynamicParameters.Add("@intStructureElementTypeId", obj.intStructureElementTypeId);
        //            dynamicParameters.Add("@intProductTypeId", obj.sitProductTypeId);


        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;

        //        }
        //    }
        //    public DataSet GetWBSDrawingReference(CABInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("WBSDrawingReference_Get");
        //            dynamicParameters.Add("@intWBSElementId", obj.intWBSElementId);
        //            dynamicParameters.Add("@intProjectId", obj.intProjectId);
        //            dynamicParameters.Add("@intGroupMarkId", obj.intGroupMarkId);
        //            dynamicParameters.Add("@intStructureElementTypeId", obj.intStructureElementTypeId);
        //            dynamicParameters.Add("@intProductTypeId", obj.sitProductTypeId);


        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;

        //        }
        //    }



        //    public int InsertGroupMarkingDrawingHistory(CABInfo obj)
        //    {
        //        try
        //        {
        //            int intReturnValue;

        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("GroupMarkingDrawingHistory_Insert");
        //            dynamicParameters.Add("@intGroupMarkId", obj.intGroupMarkId);
        //            dynamicParameters.Add("@intProjectId", obj.intProjectId);
        //            dynamicParameters.Add("@intStructureElementTypeId", obj.intStructureElementTypeId);
        //            dynamicParameters.Add("@intProductTypeId", obj.sitProductTypeId);
        //            dynamicParameters.Add("@tntGroupRevNo", obj.tntGroupRevNo);
        //            dynamicParameters.Add("@vchDrawingReference", obj.vchDrawingReference);
        //            dynamicParameters.Add("@chrDrawingVersion", obj.chrDrawingVersion);
        //            dynamicParameters.Add("@vchDrawingRemarks", obj.vchDrawingRemarks);
        //            dynamicParameters.Add("@intUploadedUid", obj.intUploadedUid);


        //            intReturnValue = System.Convert.ToInt32(DataAccess.DataAccess.GetScalar(dbcom));
        //            return intReturnValue;
        //        }

        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }
        //    public int InsertWBSDrawingHistory(CABInfo obj)
        //    {
        //        try
        //        {
        //            int intReturnValue;

        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("WBSDrawingHistory_Insert");
        //            dynamicParameters.Add("@vchWBSElementIds", obj.vchWBSElementIds);
        //            dynamicParameters.Add("@vchUploadedFiles", obj.vchUploadedFiles);
        //            dynamicParameters.Add("@vchVersionNos", obj.vchUploadedFileVersions);
        //            dynamicParameters.Add("@vchFileRemarks", obj.vchFileRemarks);
        //            dynamicParameters.Add("@intUploadedUid", obj.intUploadedUid);
        //            dynamicParameters.Add("@intGroupMarkId", obj.intGroupMarkId);
        //            dynamicParameters.Add("@intProjectId", obj.intProjectId);
        //            dynamicParameters.Add("@intStructureElementTypeId", obj.intStructureElementTypeId);
        //            dynamicParameters.Add("@intProductTypeId", obj.sitProductTypeId);

        //            intReturnValue = System.Convert.ToInt32(DataAccess.DataAccess.GetScalar(dbcom));
        //            return intReturnValue;
        //        }

        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }




        //    public int InsertColumnPRCDetails(CABInfo obj)
        //    {
        //        try
        //        {
        //            int intReturnValue;
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ColumnPRCDetails_Insert");

        //            dynamicParameters.Add("@intSEDetailingId", obj.SEDetailingId);  // ' modfied here 
        //            dynamicParameters.Add("@vchStructureMarkingName", obj.vchStructureMarkingName);
        //            dynamicParameters.Add("@intSAPMaterialcodeId", obj.SAPMAterialCodeid);

        //            dynamicParameters.Add("@numColumnWidth", obj.numColumnWidth);
        //            dynamicParameters.Add("@numColumnLength", obj.numColumnLength);
        //            dynamicParameters.Add("@numColumnHeight", obj.numColumnHeight);
        //            dynamicParameters.Add("@bitAssIndicator", obj.bitAssemblyIndicator);
        //            dynamicParameters.Add("@vchRemarks", obj.vchRemarks);

        //            dynamicParameters.Add("@intStructureMarkid", obj.intStructureMarkId);
        //            // intReturnValue = CType(DataAccess.DataAccess.GetScalar(dbcom), Int32)

        //            DataAccess.DataAccess.GetScalar(dbcom);
        //            intReturnValue = (dbcom.Parameters["@intStructureMarkid"].Value);
        //            return intReturnValue;
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }
        //    public int InsertSlabPRCDetails(CABInfo obj)
        //    {
        //        try
        //        {
        //            int intReturnValue;
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("SWPRCDetails_Insert");

        //            dynamicParameters.Add("@intSEDetailingId", obj.SEDetailingId);  // ' modfied here 
        //            dynamicParameters.Add("@vchStructureMarkingName", obj.vchStructureMarkingName);
        //            dynamicParameters.Add("@intSAPMaterialcodeId", obj.SAPMAterialCodeid);

        //            dynamicParameters.Add("@numSlabWidth", obj.numSlabwidth);
        //            dynamicParameters.Add("@numSlabDepth", obj.numSlabDepth);
        //            dynamicParameters.Add("@numSlabLength", obj.numSlabLength);
        //            dynamicParameters.Add("@bitAssIndicator", obj.bitAssemblyIndicator);
        //            dynamicParameters.Add("@vchRemarks", obj.vchRemarks);
        //            dynamicParameters.Add("@intStructureMarkid", obj.intStructureMarkId);
        //            // intReturnValue = CType(DataAccess.DataAccess.GetScalar(dbcom), Int32)

        //            DataAccess.DataAccess.GetScalar(dbcom);
        //            intReturnValue = (dbcom.Parameters["@intStructureMarkid"].Value);
        //            return intReturnValue;
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }
        //    public DataSet GetWBSElementByGroupMarkIdColumn(CABInfo obj)
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


        //    public System.Data.DataSet GetShapePathDetails(CABInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ShapePathDetails_Get");
        //            dynamicParameters.Add("@intSEDetailingID", obj.intSELevelDetailsID);
        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public DataSet GetArmaInputInfo(int intSEDetailingID)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("AplusInput_Get");
        //            dynamicParameters.Add("@intSEDetailingID", intSEDetailingID);
        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }
        //    public DataTable GetCAB_BarmarkCount(int intSEDetailingID, int intGroupMarkID, int intStructureMarkID, int intProductTypeID, int intArmaid)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("CAB_BarMarkCount");
        //            dynamicParameters.Add("@intProductTypeID", intProductTypeID);
        //            dynamicParameters.Add("@intSEDetailingID", intSEDetailingID);
        //            dynamicParameters.Add("@intStructureMarkID", intStructureMarkID);
        //            dynamicParameters.Add("@intGroupMarkID", intGroupMarkID);
        //            dynamicParameters.Add("@intArmaID", intArmaid);
        //            return DataAccess.DataAccess.GetDataSet(dbcom).Tables(0);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public void InsertArmaTraceInfo(int intArmaID, string CopyQuery, string Result, DateTime SendTimeStamp, DateTime ReceiveTimeStamp)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ArmaTraceInfo_Insert");
        //            dynamicParameters.Add("intArmaID", intArmaID);
        //            dynamicParameters.Add("CopyQuery", CopyQuery);
        //            dynamicParameters.Add("Result", Result);
        //            dynamicParameters.Add("SendTimeStamp", DbType.DateTime, SendTimeStamp);
        //            dynamicParameters.Add("ReceiveTimeStamp", DbType.DateTime, ReceiveTimeStamp);
        //            DataAccess.DataAccess.ExecuteNonQuery(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }
        public bool SaveCABShapeDetails(string ShapeCode, string BVBS, int NoofBends, string ShapeIndice, string ShapeGroup, bool BarToBar, string ShapeSurCharge, Int16 intCatalog, string chrVersion)
        {
            try
            {

                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@ShapeCode", ShapeCode);
                    dynamicParameters.Add("@BVBS", BVBS);
                    dynamicParameters.Add("@NoofBends",NoofBends);
                    dynamicParameters.Add("@ShapeGroup", ShapeGroup);
                    dynamicParameters.Add("@BarToBar", BarToBar.ToString());
                    dynamicParameters.Add("@ShapeSurcharge", ShapeSurCharge);
                    dynamicParameters.Add("@ShapeINDICE", ShapeIndice);
                    dynamicParameters.Add("@sitCatalog", intCatalog);
                    dynamicParameters.Add("@chrVersion", chrVersion);
                    sqlConnection.Query<bool>(SystemConstants.CABShape_InsUpd, dynamicParameters, commandType: CommandType.StoredProcedure);
                    sqlConnection.Close();
                    return true;

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<getShapeDetailsDto> GetExistingCABShapeDetails(Int16 intCatalog)
        {
            try
            {
                DataSet ds = new DataSet();
                IEnumerable<getShapeDetailsDto> cABInfos;
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@sitCatalog",intCatalog);

                    cABInfos = sqlConnection.Query<getShapeDetailsDto>(SystemConstants.CABShapeDetails_Get_cube, dynamicParameters, commandType: CommandType.StoredProcedure);
                    sqlConnection.Close();
                    return cABInfos.AsList();

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //    public void UpdateCABCoords(CABInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("CABShapeDetails_Update");
        //            dynamicParameters.Add("@intShapeID", obj.intShapeId);
        //            dynamicParameters.Add("@chrVersion", obj.vchStructureVersionNos);
        //            dynamicParameters.Add("@intX", obj.intX);
        //            dynamicParameters.Add("@intY", obj.intY);
        //            dynamicParameters.Add("@intZ", obj.intZ);
        //            DataAccess.DataAccess.ExecuteNonQuery(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public void DeleteCABShape(CABInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("CABShapeStatus_Delete");
        //            dynamicParameters.Add("@intShapeID", obj.intShapeId);
        //            DataAccess.DataAccess.ExecuteNonQuery(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    // Public s Function GetShapeCoordinates(ByVal intShapeID As Int32) As System.Data.DataSet
        //    // Try
        //    // dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("CABShapeDetails_Get")
        //    // dynamicParameters.Add("@intShapeID", intShapeID)
        //    // Return DataAccess.DataAccess.GetDataSet(dbcom)
        //    // Catch ex As Exception
        //    // Throw ex
        //    // End Try
        //    // End Function
        public List<GetShapeCoordinatesDto> GetShapeCoordinates(string StrShapeID)
        {
            try
            {
                DataSet ds = new DataSet();
                IEnumerable<GetShapeCoordinatesDto>getShapeCoordinates = new List<GetShapeCoordinatesDto>();
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@StrShapeID", StrShapeID);
                    getShapeCoordinates = sqlConnection.Query<GetShapeCoordinatesDto>(SystemConstants.CABShapeCoords_get_cube_New, dynamicParameters, commandType: CommandType.StoredProcedure);

                    //SqlCommand cmd = new SqlCommand(SystemConstants.CABShapeCoords_get_cube, sqlConnection);
                    //cmd.CommandType = CommandType.StoredProcedure;
                    //dynamicParameters.Add("@StrShapeID", StrShapeID);
                    //SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    //adapter.Fill(ds);

                    //if (ds != null && ds.Tables.Count != 0)
                    //{
                    //    foreach (DataRowView drvBorePileParameterSet in ds.Tables[0].DefaultView)
                    //    {
                    //        CABInfo borePilePS = new CABInfo
                    //        {
                    //            ParameterSetNo = Convert.ToInt32(drvBorePileParameterSet["INTPARAMETESET"]),
                    //            ParameterSetId = Convert.ToInt32(drvBorePileParameterSet["TNTPARAMSETNUMBER"]),
                    //            ProductType = drvBorePileParameterSet["VCHPRODUCTTYPE"].ToString(),
                    //            ProductTypeL2Id = Convert.ToInt32(drvBorePileParameterSet["SITPRODUCTTYPEL2ID"]),
                    //            TransportModeId = Convert.ToInt32(drvBorePileParameterSet["TNTTRANSPORTMODEID"]),
                    //            LapLength = Convert.ToInt32(drvBorePileParameterSet["INTLAPLENGTH"]),
                    //            EndLength = Convert.ToInt32(drvBorePileParameterSet["INTENDLENGTH"]),
                    //            AdjFactor = Convert.ToInt32(drvBorePileParameterSet["INTADJFACTOR"]),
                    //            CoverToLink = Convert.ToInt32(drvBorePileParameterSet["INTCOVERTOLINK"]),
                    //            ParameterType = drvBorePileParameterSet["VCHPARAMETERTYPE"].ToString(),
                    //            Description = drvBorePileParameterSet["VCHDESCRIPTION"].ToString()
                    //        };
                    //        listBorePilePS.Add(borePilePS);
                    //    }
                    //}


                }
                //return listBorePilePS;
                return getShapeCoordinates.ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public byte[] GetShapeImage(string StrShapeID)
        {
            try
            {
                byte[] image = null;
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@SHAPECODE", StrShapeID);


                    image = sqlConnection.QueryFirstOrDefault<byte[]>(
                           SystemConstants.USP_GET_SHAPE_IMAGE,
                           dynamicParameters,
                           commandType: CommandType.StoredProcedure);
                    sqlConnection.Close();

                    //SqlDataReader reader =DataAccess.ExecuteReader(image);

                    //if (reader.HasRows)
                    //{
                    //    while (reader.Read())
                    //        image = (byte[])reader[0];
                    //}
                    return image;


                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<GetAllShapeImgDto> GetAllShapeImage()
        {
            try
            {
                byte[] image = null;
                IEnumerable<GetAllShapeImgDto> shapeImages;
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    //dynamicParameters.Add("@SHAPECODE", StrShapeID);


                    shapeImages=sqlConnection.Query<GetAllShapeImgDto>(SystemConstants.USP_GET_SHAPE_IMAGE_ALL,dynamicParameters,commandType: CommandType.StoredProcedure);
                    sqlConnection.Close();
                    return shapeImages.ToList();


                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<getShapeDetailsDto>GetCABShapeCatalogDetails()
        {
            try
            {
                DataSet dataSet = new DataSet();
                IEnumerable<getShapeDetailsDto> getCatalog;
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();

                    getCatalog = sqlConnection.Query<getShapeDetailsDto>(SystemConstants.CABShapeCatalogDetails_Get, commandType: CommandType.StoredProcedure);
                    sqlConnection.Close();
                    return getCatalog.ToList();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //    public int GetArmaIDPileStruct(int intStructID, string StructureElement)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ArmaIDonStructID_Get");
        //            dynamicParameters.Add("intStructureMarkID", intStructID);
        //            dynamicParameters.Add("vchStructureElement", StructureElement);
        //            return DataAccess.DataAccess.GetScalar(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }
        //    public DataSet GetSAPMaterialStructure(CABInfo obj)
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
        //    public DataSet PopulatePRCDefault(CABInfo obj)
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
        public bool UpdateShapeParameters(UpdateShapeParamDto shapeParamDto)
        {
            try
            {
                DataSet ds = new DataSet();
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@strShapeID", shapeParamDto.StrShapeID);
                    dynamicParameters.Add("@ParameterSeq", shapeParamDto.ParameterSeq);
                    dynamicParameters.Add("@ParameterName", shapeParamDto.ParameterName);
                    dynamicParameters.Add("@XCoor", shapeParamDto.XCoor);
                    dynamicParameters.Add("@YCoor", shapeParamDto.YCoor);
                    dynamicParameters.Add("@ZCoor", shapeParamDto.ZCoor);
                    dynamicParameters.Add("@Visible", shapeParamDto.Visible);
                    if (shapeParamDto.symmetricTo.Trim() == "")
                    {
                        dynamicParameters.Add("@SymmetricTo",null);
                    }
                    else
                    {
                        dynamicParameters.Add("@SymmetricTo", shapeParamDto.symmetricTo);
                    }

                    if (shapeParamDto.Formula.Trim() == "")
                    {
                        dynamicParameters.Add("@Formula", null);
                    }
                    else
                    {
                        dynamicParameters.Add("@Formula", shapeParamDto.Formula);
                    }

                    dynamicParameters.Add("@Range", shapeParamDto.Range);

                    if (shapeParamDto.HAFormula.Trim() == "")
                    {
                        dynamicParameters.Add("@HAFormula", null);

                    }
                    else
                    {
                        dynamicParameters.Add("@HAFormula",shapeParamDto.HAFormula);

                    }
                    if (shapeParamDto.OAFormula.Trim() == "")
                    {
                        dynamicParameters.Add("@OAFormula",null);

                    }
                    else
                    {
                        dynamicParameters.Add("@OAFormula",shapeParamDto.OAFormula);

                    }

                    sqlConnection.Query<bool>(SystemConstants.UpdateShapeParameters_cube, dynamicParameters, commandType: CommandType.StoredProcedure);
                    sqlConnection.Close();
                    return true;

                }
                
             
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //    public DataSet GetLockNutForCouplerType(CABInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ACC_LOCKNUT_SAPMaterial_GET_BY_COUPLERTYPE");
        //            dynamicParameters.Add("@COUPLERTYPE", obj.SAPMaterialCode);
        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }
        //    // ' Added By AK - CAB Shape Parameter Validation CR - 29-Jun-2018

        //public bool BBSPostingGetDataDiscrepancy(int GroupMarkId)
        //{
        //    bool IsDataDiscrepancy;
        //    int res;
        //    DataSet ds;
        //    //DataTable res;
        //    int DD;
        //    try
        //    {
        //        using (var sqlConnection = new SqlConnection(connectionString))
        //        {

        //            sqlConnection.Open();
        //            var dynamicParameters = new DynamicParameters();
        //            dynamicParameters.Add("@INTGROUPMARKID", GroupMarkId); 
        //            res =sqlConnection.QueryFirstOrDefault<int>(SystemConstants.usp_CABDetailingDataDescrepancy_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
        //            sqlConnection.Close();

        //            //res = ds.Tables[0];
        //            //DD = Convert.ToInt32(res.Rows[0]["Result"]);

        //            if (res > 0)
        //                IsDataDiscrepancy = true;
        //            else
        //                IsDataDiscrepancy = false;

        //        }




        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return IsDataDiscrepancy;
        //}

        //    // Added By KA - NEW CAB Posting Page - 26-Nov-2010
        //public List<LoadSORDto> BBSPostingCABSOR_Get(CABInfo obj)
        //{
        //    try
        //    {
        //        IEnumerable<LoadSORDto> loadSORDtos;
        //        using (var sqlConnection = new SqlConnection(connectionString))
        //        {

        //            sqlConnection.Open();
        //            var dynamicParameters = new DynamicParameters();
        //            dynamicParameters.Add("@ORD_REQ_NO", obj.ORD_REQ_NO);
        //            loadSORDtos = sqlConnection.Query<LoadSORDto>(SystemConstants.BBSPostingCABSOR_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
        //            sqlConnection.Close();
        //            return loadSORDtos.ToList();

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //    // Added By KA - NEW CAB Posting Page - 26-Nov-2010
        //    // Public s Function ESM_BBSPostingCABSOR_Get(ByVal obj As CABInfo) As DataSet
        //    // Try
        //    // dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ESM_BBSPostingCABSOR_Get")
        //    // dynamicParameters.Add("@ORD_REQ_NO",  obj.ORD_REQ_NO)
        //    // Return DataAccess.DataAccess.GetDataSet(dbcom)
        //    // Catch ex As Exception
        //    // Throw ex
        //    // End Try
        //    // End Function

        //    // Added By KA - NEW CAB Posting Page - 26-Nov-2010
        //public List<LoadSORDto>BBSPostingCAB_Get(CABInfo obj)
        //{
        //    try
        //    {
        //        IEnumerable<LoadSORDto> loadSORDtos;
        //        using (var sqlConnection = new SqlConnection(connectionString))
        //        {

        //            sqlConnection.Open();
        //            var dynamicParameters = new DynamicParameters();
        //            dynamicParameters.Add("@ORD_REQ_NO", obj.ORD_REQ_NO);
        //            loadSORDtos = sqlConnection.Query<LoadSORDto>(SystemConstants.BBSPostingCAB_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
        //            sqlConnection.Close();
        //            return loadSORDtos.ToList();

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //    // Added By KA - NEW CAB Posting Page - 3-Apr-2012
        //    public DataSet BBSPostingCAB_Get_Range(CABInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("usp_BBSPostingCAB_Get_Range");
        //            dynamicParameters.Add("@ORD_REQ_NO_FROM", obj.FROM_SOR);
        //            dynamicParameters.Add("@ORD_REQ_NO_TO", obj.TO_SOR);
        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }
        //public List<LoadSORDto>ESM_BBSPostingCAB_Get_Range(CABInfo obj)
        //{
        //    try
        //    {
        //        IEnumerable<LoadSORDto> loadSORDtos;
        //        using (var sqlConnection = new SqlConnection(connectionString))
        //        {

        //            sqlConnection.Open();
        //            var dynamicParameters = new DynamicParameters();
        //            dynamicParameters.Add("@ORD_REQ_NO_FROM", obj.FROM_SOR);
        //            dynamicParameters.Add("@ORD_REQ_NO_TO", obj.TO_SOR);
        //            loadSORDtos = sqlConnection.Query<LoadSORDto>(SystemConstants.usp_ESM_BBSPostingCAB_Get_Range, dynamicParameters, commandType: CommandType.StoredProcedure);
        //            sqlConnection.Close();
        //            return loadSORDtos.ToList();

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public List<BBSPostingCABGMDto> BBSPostingCABGM_Get(CABInfo obj)
        //{
        //    try
        //    {
        //        IEnumerable<BBSPostingCABGMDto> loadSORDtos;
        //        using (var sqlConnection = new SqlConnection(connectionString))
        //        {

        //            sqlConnection.Open();
        //            var dynamicParameters = new DynamicParameters();
        //            dynamicParameters.Add("@IntProjectID", obj.intProjectId);
        //            dynamicParameters.Add("@BBS_NO", obj.BBS_NO);
        //            loadSORDtos = sqlConnection.Query<BBSPostingCABGMDto>(SystemConstants.BBSPostingCABGM_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
        //            sqlConnection.Close();
        //            return loadSORDtos.ToList();

        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public int BBSPostingCAB_Post(BBSCABPostDto obj)
        //{
        //    int intReturnValue;
        //    try
        //    {
        //        obj.UserID = GetUserIDByName(obj.Username);

        //        using (var sqlConnection = new SqlConnection(connectionString))
        //        {

        //            sqlConnection.Open();
        //            var dynamicParameters = new DynamicParameters();
        //            dynamicParameters.Add("@intProjectID", obj.IntProjectID);
        //            dynamicParameters.Add("@intWBSElementID", obj.IntWBSElementID);
        //            dynamicParameters.Add("@intGroupMarkID", obj.IntGroupMarkID);
        //            dynamicParameters.Add("@BBS_NO", obj.BBS_NO);
        //            dynamicParameters.Add("@UserID", obj.UserID);
        //            intReturnValue = sqlConnection.QueryFirstOrDefault<int>(SystemConstants.BBSPostingCAB_Insert, dynamicParameters, commandType: CommandType.StoredProcedure);
        //            sqlConnection.Close();
        //            return intReturnValue;

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return intReturnValue;
        //}

        //public int BBSPostingCAB_Unpost(CABInfo obj)
        //{
        //    int intReturnValue;
        //    try
        //    {

        //        using (var sqlConnection = new SqlConnection(connectionString))
        //        {

        //            sqlConnection.Open();
        //            var dynamicParameters = new DynamicParameters();
        //            dynamicParameters.Add("@intProjectID", obj.intProjectId);
        //            dynamicParameters.Add("@intWBSElementID", obj.intWBSElementId);
        //            dynamicParameters.Add("@intGroupMarkID", obj.intGroupMarkId);
        //            dynamicParameters.Add("@BBS_NO", obj.BBS_NO);
        //            intReturnValue = sqlConnection.QueryFirstOrDefault<int>(SystemConstants.BBSPostingCAB_Delete, dynamicParameters, commandType: CommandType.StoredProcedure);
        //            sqlConnection.Close();

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return intReturnValue;
        //}
        //    // Added By KA - NEW CAB Posting Page - 26-Nov-2010
        //    public System.Data.DataSet GetArmaInfoByArmaID(int intArmaID)
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
        //    // Added By KA - NEW CAB Posting Page - 26-Nov-2010
        //public List<GroupMarkIdGetDto>GroupMarkIdCAB_Get(GroupMarkIdCABDto obj)
        //{
        //    int intResultValue;
        //    IEnumerable<GroupMarkIdGetDto> groupMarkIdGets;
        //    try
        //    {
        //        obj.intUserId = GetUserIDByName(obj.Username);
        //        using (var sqlConnection = new SqlConnection(connectionString))
        //        {

        //            sqlConnection.Open();
        //            var dynamicParameters = new DynamicParameters();
        //            dynamicParameters.Add("@intProjectID", obj.intProjectId);
        //            dynamicParameters.Add("@intWBSElementID", obj.intWBSElementId);
        //            dynamicParameters.Add("@vchStructureElementName", obj.vchStructureElementName);
        //            dynamicParameters.Add("@vchGroupMarkingName", obj.BBS_NO);
        //            dynamicParameters.Add("@intUserId", obj.intUserId);
        //            groupMarkIdGets = sqlConnection.Query<GroupMarkIdGetDto>(SystemConstants.GroupMarkIdCAB_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
        //            sqlConnection.Close();

        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return groupMarkIdGets.ToList();
        //}
        //    Added By KA - NEW feature to release in CAB Posting Page - 19-Jan-2011
        //    public int BBSReleaseCAB_Insert(CABInfo obj)
        //    {
        //        string intReturnValue;
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BBSReleaseCAB_Insert");
        //            dynamicParameters.Add("@ORD_REQ_NO", obj.ORD_REQ_NO);
        //            dynamicParameters.Add("@intWBSElementID", obj.intWBSElementId);
        //            dynamicParameters.Add("@intProjectID", obj.intProjectId);
        //            dynamicParameters.Add("@BBS_NO", obj.BBS_NO);
        //            dynamicParameters.Add("@chrBBSStatus", obj.chrBBSStatus);
        //            dynamicParameters.Add("@UserID", obj.intUserid);
        //            intReturnValue = DataAccess.DataAccess.GetScalar(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        return intReturnValue;
        //    }

        //    public int BBSReleaseBySOR_Insert(CABInfo obj)
        //    {
        //        string intReturnValue;
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BBSReleaseBySOR_Insert");
        //            dynamicParameters.Add("@ORD_REQ_NO", obj.ORD_REQ_NO);
        //            dynamicParameters.Add("@intWBSElementID", obj.intWBSElementId);
        //            dynamicParameters.Add("@intProjectID", obj.intProjectId);
        //            dynamicParameters.Add("@BBS_NO", obj.BBS_NO);
        //            dynamicParameters.Add("@chrBBSStatus", obj.chrBBSStatus);
        //            dynamicParameters.Add("@UserID", obj.intUserid);
        //            intReturnValue = DataAccess.DataAccess.GetScalar(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        return intReturnValue;
        //    }


        public int InsertCabShapeMaster(string ShapeID,string createdUser,string description)
        {
            int intReturnValue;
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@ShapeId", ShapeID);
                    dynamicParameters.Add("@user_ID", createdUser);
                    dynamicParameters.Add("@description", description);
                    intReturnValue = sqlConnection.QueryFirstOrDefault<int>(SystemConstants.CABShapeMaster_Insert_cube, dynamicParameters, commandType: CommandType.StoredProcedure);
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


        public int InsertCabShapeDetails(CABInfo obj)
        {
            string intReturnValue;
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@ShapeId", obj.ShapeID);
                    intReturnValue = sqlConnection.QueryFirstOrDefault<string>(SystemConstants.CABShapeDetails_Insert_cube, dynamicParameters, commandType: CommandType.StoredProcedure);
                    sqlConnection.Close();
                    return Convert.ToInt32(intReturnValue);

                }
               
            }

            catch (Exception ex)
            {
                throw ex;
            }
            return Convert.ToInt32(intReturnValue);
        }


        public int DeleteCabShapeMaster(CABInfo obj)
        {
            string intReturnValue;
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@ShapeId", obj.ShapeID);
                    intReturnValue = sqlConnection.QueryFirstOrDefault<string>(SystemConstants.CABShapeMaster_Delete_cube, dynamicParameters, commandType: CommandType.StoredProcedure);
                    sqlConnection.Close();
                    return Convert.ToInt32(intReturnValue);

                }
              
            }

            catch (Exception ex)
            {
                throw ex;
            }
            return Convert.ToInt32(intReturnValue);
        }


        public int DeleteCabShapeDetails(CABInfo obj)
        {
            string intReturnValue;
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@ShapeId", obj.ShapeID);
                    intReturnValue = sqlConnection.QueryFirstOrDefault<string>(SystemConstants.CABShapeDetails_Delete_cube, dynamicParameters, commandType: CommandType.StoredProcedure);
                    sqlConnection.Close();
                    return Convert.ToInt32(intReturnValue);

                }
              
            }

            catch (Exception ex)
            {
                throw ex;
            }
            return Convert.ToInt32(intReturnValue);
        }

        //    public int CheckShapeExists(CABInfo obj)
        //    {
        //        try
        //        {
        //            int strReturnValue;
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("usp_CheckShapeExists_cube");
        //            dynamicParameters.Add("@ShapeId", obj.ShapeID);
        //            dynamicParameters.Add("@bitEdit", 32);
        //            DataAccess.DataAccess.ExecuteNonQuery(dbcom);
        //            strReturnValue = DataAccess.DataAccess.db.GetParameterValue("@bitEdit");
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public DataSet SelectExitingShapeDetails(CABInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ExistingShape_Get_cube");
        //            dynamicParameters.Add("@ShapeID", obj.CSM_SHAPE_ID);
        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }


        //    public int InsertShapeDetailsTMast(CABInfo obj)
        //    {
        //        string intReturnValue;
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("CABShapeDetailsTMast_Insert");
        //            dynamicParameters.Add("@ShapeId", obj.CSM_SHAPE_ID);
        //            dynamicParameters.Add("@Category", obj.CSC_CAT_DESC);
        //            dynamicParameters.Add("@xcord", obj.CSM_X_COR);
        //            dynamicParameters.Add("@ycord", obj.CSM_Y_COR);
        //            dynamicParameters.Add("@noBends", obj.CSM_NO_BEND);
        //            dynamicParameters.Add("@NoArc", obj.CSM_NO_ARC);
        //            dynamicParameters.Add("@unit", obj.CSM_SCALE);
        //            dynamicParameters.Add("@userID", obj.createdUser);
        //            // dynamicParameters.Add("@Pic", DbType.Byte, imagedata)
        //            // dynamicParameters.Add("@Pic", DbType.Byte, imagedata)
        //            intReturnValue = DataAccess.DataAccess.ExecuteNonQuery(dbcom);
        //        }

        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        return intReturnValue;
        //    }

        //    public int InsertShapeDetailsTDetails(CABInfo obj)
        //    {
        //        string intReturnValue;
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("CABShapeDetailsTDetails_Insert");

        //            dynamicParameters.Add("@Seq_no", obj.CSD_SEQ_NO);
        //            dynamicParameters.Add("@ShapeID", obj.CSD_SHAPE_ID);
        //            dynamicParameters.Add("@Type", obj.CSD_TYPE);
        //            dynamicParameters.Add("@end_x", obj.CSD_END_POINT_X);
        //            dynamicParameters.Add("@end_y", obj.CSD_END_POINT_Y);
        //            dynamicParameters.Add("@inputtype", obj.CSD_INPUT_TYPE);
        //            dynamicParameters.Add("@par1", obj.CSD_PAR1);
        //            dynamicParameters.Add("@par2", obj.CSD_PAR2);
        //            dynamicParameters.Add("@par3", obj.CSD_PAR3);
        //            dynamicParameters.Add("@arc_dir", obj.CSD_ARC_DIR);
        //            dynamicParameters.Add("@userID", obj.createdUser);

        //            intReturnValue = DataAccess.DataAccess.ExecuteNonQuery(dbcom);
        //        }

        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        return intReturnValue;
        //    }

        //    public System.Data.DataSet ExistingShapeTDetails(CABInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ExistingShapeDetails3D_Get");
        //            dynamicParameters.Add("@shapeID", obj.CSD_SHAPE_ID);
        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }



        //    public DataSet CABShapeProperties(CABInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("uspCABShapeProperties");
        //            dynamicParameters.Add("@ShapeID", obj.CSM_SHAPE_ID);
        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    // Public Function InsertCouplerCordinates(ByVal startX As Integer, ByVal startY As Integer, ByVal stopX As Integer, ByVal stopY As Integer, ByVal rowid As Integer, ByVal type As String, ByVal coupActX As Integer, ByVal coupActY As Integer) As Integer
        //    // Dim intReturnValue As String
        //    // Try
        //    // dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("CouplerCoordinates_Insert")
        //    // dynamicParameters.Add("@startX", startX)
        //    // dynamicParameters.Add("@startY", startY)
        //    // dynamicParameters.Add("@stopX", stopX)
        //    // dynamicParameters.Add("@stopY", stopY)
        //    // dynamicParameters.Add("@rowid", rowid)
        //    // dynamicParameters.Add("@type",  type)
        //    // dynamicParameters.Add("@coupActX", coupActX)
        //    // dynamicParameters.Add("@coupActY", coupActY)
        //    // ' intReturnValue = DataAccess.DataAccess.GetScalar(dbcom)
        //    // intReturnValue = DataAccess.DataAccess.ExecuteNonQuery(dbcom)

        //    // Catch ex As Exception
        //    // Throw ex
        //    // End Try
        //    // Return intReturnValue
        //    // End Function



        //    /// <summary>
        //    ///     ''' Method to get the bbs for copy bbs functionality
        //    ///     ''' </summary>
        //    ///     ''' <param name="prijectId"></param>
        //    ///     ''' <returns></returns>
        //    ///     ''' <remarks></remarks>
        //    public System.Data.DataSet GetBBS(int prijectId)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("GET_BBS_CUBE");
        //            dynamicParameters.Add("@PROJECTID", prijectId);
        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    /// <summary>
        //    ///     ''' Method to copy BBS from source to target.
        //    ///     ''' </summary>
        //    ///     ''' <param name="bbsSource"></param>
        //    ///     ''' <param name="bbsTarget"></param>
        //    ///     ''' <param name="pojId"></param>
        //    ///     ''' <param name="wbsId"></param>
        //    ///     ''' <param name="vchStructureElementType"></param>
        //    ///     ''' <returns></returns>
        //    ///     ''' <remarks></remarks>
        //    public int CopyBBS(string bbsSource, string bbsTarget, int pojId, int wbsId, string vchStructureElementType)
        //    {
        //        int intReturnValue;
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("COPY_BBS_INSERT_CUBE");
        //            dynamicParameters.Add("@BBSSOURCE", bbsSource.Trim());
        //            dynamicParameters.Add("@BBSTARGET", bbsTarget.Trim());
        //            dynamicParameters.Add("@intProjectID", pojId);
        //            dynamicParameters.Add("@intWBSElementID", wbsId);
        //            dynamicParameters.Add("@vchStructureElementName", vchStructureElementType);
        //            dynamicParameters.Add("@ROWCOUNT", 100);
        //            intReturnValue = DataAccess.DataAccess.ExecuteNonQuery(dbcom);
        //            intReturnValue = dbcom.Parameters["@ROWCOUNT"].Value;
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        return intReturnValue;
        //    }

        public bool ImportImageToDB(ImportImageDto importImage)
        {
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@Pic", importImage.imag);
                    dynamicParameters.Add("@ShapeId", importImage.shapeCode.Trim());

                    sqlConnection.Query<bool>(SystemConstants.USP_ImportImageToDB, dynamicParameters, commandType: CommandType.StoredProcedure);
                    sqlConnection.Close();
                    return true;

                }
                //var sqlConnection = new SqlConnection(connectionString);
                //string query = "UPDATE t_cab_shape_mast set CSM_SHAPE_IMAGE = @Pic where CSM_SHAPE_ID = @ShapeId";

                //using (SqlCommand command = new SqlCommand(query,sqlConnection))
                //{
                //    sqlConnection.Open();
                //    command.Parameters.AddWithValue("@Pic", importImage.imag);
                //    command.Parameters.AddWithValue("@ShapeId", importImage.shapeCode);

                //    command.ExecuteNonQuery();
                //    return true;
                //}

            }
            catch (Exception ex)
            {
                return false;
            }
        }


        //    /// <summary>
        //    ///     ''' Method to delete the shape code.
        //    ///     ''' </summary>
        //    ///     ''' <param name="shapeCode"></param>
        //    ///     ''' <returns></returns>
        //    ///     ''' <remarks></remarks>
        public bool DeleteShapeCode(string shapeCode)
        {
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@SHAPECODE", shapeCode.Trim());
                    sqlConnection.Query<bool>(SystemConstants.USP_SHAPE_DELETE_cube, dynamicParameters, commandType: CommandType.StoredProcedure);
                    sqlConnection.Close();
                    return true;

                }
                
                
            }
            catch (Exception ex)
            {
                return false;
            }
        }



        //    public DataSet GetBBSName(CABInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("BBS_NAME_GET_OES");
        //            dynamicParameters.Add("@ORD_REQ_NO", obj.ORD_REQ_NO);
        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    public int InsertCabShapeMasterSpecial(CABInfo obj)
        //    {
        //        string intReturnValue;
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("CABShapeMasterSpecial_Insert_cube");
        //            dynamicParameters.Add("@ShapeId", obj.ShapeID);
        //            dynamicParameters.Add("@user_ID", obj.createdUser);

        //            // intReturnValue = DataAccess.DataAccess.GetScalar(dbcom)
        //            intReturnValue = DataAccess.DataAccess.ExecuteNonQuery(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        return intReturnValue;
        //    }

        //    public int InsertCabShapeDetailsSpecial(CABInfo obj, string match, int SeqNo)
        //    {
        //        string intReturnValue;
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("CABShapeDetailsSpecial_Insert_cube");
        //            dynamicParameters.Add("@ShapeId", obj.ShapeID);
        //            dynamicParameters.Add("@user_ID", obj.createdUser);
        //            dynamicParameters.Add("@match", match);
        //            dynamicParameters.Add("@SeqNo", SeqNo);
        //            // intReturnValue = DataAccess.DataAccess.GetScalar(dbcom)
        //            intReturnValue = DataAccess.DataAccess.ExecuteNonQuery(dbcom);
        //        }

        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        return intReturnValue;
        //    }

        public int GetShapeEditable(string shapeEdit)
        {
            try
            {
                int isEditable = 0;
                DataSet ds = new DataSet();
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@ShapeCode", shapeEdit);
                    isEditable = sqlConnection.QueryFirstOrDefault<int>(SystemConstants.ShapeEditable_Get_cube, dynamicParameters, commandType: CommandType.StoredProcedure);
                    sqlConnection.Close();
                    return isEditable;

                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //    // Added by panchatapa for CHM114465630 (Shape Group)
        public int InsertShapeGroupDetails(addShapeGrpDetailDto obj)
        {
            string intReturnValue;
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@ShapeCode", obj.ShapeID);
                    dynamicParameters.Add("@ShapeGroup", obj.ShapeGroup);
                    dynamicParameters.Add("@Coupler", obj.Coupler);
                    dynamicParameters.Add("@Stud", obj.Stud);
                    dynamicParameters.Add("@Thread", obj.Thread);
                    dynamicParameters.Add("@Locknut", obj.Locknut);
                    dynamicParameters.Add("@ShapePrefix", obj.ShapePrefix);
                    dynamicParameters.Add("@CouplerType", obj.CouplerType);
                    dynamicParameters.Add("@Flag", obj.Flag);
                    intReturnValue = sqlConnection.QueryFirstOrDefault<string>(SystemConstants.InsertShapeGroup, dynamicParameters, commandType: CommandType.StoredProcedure);
                    sqlConnection.Close();
                    return Convert.ToInt32(intReturnValue);

                }
             
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //    // Added by panchatapa for CHM114465630 (Shape Group)

        //    // START:ADDED BY SIDDHI FOR ESM BY SIDDHI

        //public int ESM_BBSReleaseCAB_Insert(BBSCABReleaseDto obj)
        //{
        //    int intReturnValue;
        //    try
        //    {
        //        obj.UserID = GetUserIDByName(obj.Username);

        //        using (var sqlConnection = new SqlConnection(connectionString))
        //        {

        //            sqlConnection.Open();
        //            var dynamicParameters = new DynamicParameters();
        //            dynamicParameters.Add("@ORD_REQ_NO",obj.ORD_REQ_NO);
        //            dynamicParameters.Add("@intWBSElementID",obj.intWBSElementId);
        //            dynamicParameters.Add("@intProjectID",obj.intProjectId);
        //            dynamicParameters.Add("@BBS_NO",obj.BBS_NO);
        //            dynamicParameters.Add("@chrBBSStatus",obj.chrBBSStatus);
        //            dynamicParameters.Add("@UserID",obj.UserID);
        //            intReturnValue = sqlConnection.QueryFirstOrDefault<int>(SystemConstants.ESM_BBSReleaseCAB_Insert, dynamicParameters, commandType: CommandType.StoredProcedure);
        //            sqlConnection.Close();

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return intReturnValue;
        //}

        //public List<LoadSORDto> ESM_BBSPostingCABSOR_Get(CABInfo obj)
        //{
        //    try
        //    {
        //        IEnumerable<LoadSORDto>loadSORDtos;
        //        using (var sqlConnection = new SqlConnection(connectionString))
        //        {

        //            sqlConnection.Open();
        //            var dynamicParameters = new DynamicParameters();
        //            dynamicParameters.Add("@ORD_REQ_NO", obj.ORD_REQ_NO);
        //            loadSORDtos = sqlConnection.Query<LoadSORDto>(SystemConstants.ESM_BBSPostingCABSOR_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
        //            sqlConnection.Close();
        //            return loadSORDtos.ToList();

        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //// END
        //    public DataSet DownloadData(CABInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ESM_CABDownload");
        //            dynamicParameters.Add("@TRACK_NO", obj.ORD_REQ_NO);
        //            // dynamicParameters.Add("@TRACK_NO_TO",  obj.TO_SOR)
        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    // Start:Added for report
        //    public DataSet Report_DownloadData(string prodType, string monthYear)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("Report_Download");
        //            dynamicParameters.Add("@PROD_TYPE", prodType);
        //            dynamicParameters.Add("@MONTH_YEAR", monthYear);
        //            // dynamicParameters.Add("@TRACK_NO_TO",  obj.TO_SOR)
        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //    // End


      

        //    // START: ADDED BY SIDDHI FOR ESM ORDER TRACKER ENHANCEMENT CR
        //    public void ESMTrackerDataDownload(string source, string strFrom, string strTo)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("usp_ESM_ExportTrackingDetails_Get");
        //            dynamicParameters.Add("@Filter", source);
        //            dynamicParameters.Add("@From", strFrom);
        //            dynamicParameters.Add("@To", strTo);
        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }


        //    public DataSet ESM_SOR_Get(CABInfo obj)
        //    {
        //        try
        //        {
        //            dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("ESM_SOR_Get");
        //            dynamicParameters.Add("@ORD_REQ_NO", obj.ORD_REQ_NO);
        //            return DataAccess.DataAccess.GetDataSet(dbcom);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }



        //public int GetUserIDByName(string name)
        //{
        //    int Output = 0;
        //    try
        //    {
        //        using (var sqlConnection = new SqlConnection(connectionString))
        //        {

        //            sqlConnection.Open();
        //            var dynamicParameters = new DynamicParameters();
        //            dynamicParameters.Add("@UserName", name);

        //            Output = sqlConnection.QueryFirstOrDefault<int>(SystemConstants.GetUserID, dynamicParameters, commandType: CommandType.StoredProcedure);
        //            //Output = dynamicParameters.Get<int>("@Output");
        //            sqlConnection.Close();

        //        }

        //        return Output;


        //    }
        //    catch (Exception e)
        //    {
        //        return Output;
        //    }

        //}


        public List<GridListDto> ShapeCodeDetails_Grid(string shapeId)
        {
            try
            {
                DataSet dataSet = new DataSet();
                IEnumerable<GridListDto> gridLists;
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@ShapeId", shapeId);

                    gridLists = sqlConnection.Query<GridListDto>(SystemConstants.USP_GET_ShapeCodeDetails_Grid,dynamicParameters, commandType: CommandType.StoredProcedure);
                    sqlConnection.Close();
                    return gridLists.ToList();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool ShapeCodeDetails_Insert(List<GridListDto> ShapeTableData)
        {
            try
            {
                DataSet dataSet = new DataSet();
                IEnumerable<GridListDto> gridLists;
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();

                    var dynamicParameters_Delete = new DynamicParameters();
                    dynamicParameters_Delete.Add("@ShapeId", ShapeTableData[0].ShapeId);
                    sqlConnection.QueryFirstOrDefault<string>(SystemConstants.USP_Delete_ShapeCodeDetails_Grid, dynamicParameters_Delete, commandType: CommandType.StoredProcedure);

                    foreach (var gridListDto in ShapeTableData)
                    {
                        var dynamicParameters = new DynamicParameters();
                        dynamicParameters.Add("@CSD_SEQ_NO", gridListDto.SeqNo);
                        dynamicParameters.Add("@CSD_SHAPE_ID", gridListDto.ShapeId);
                        dynamicParameters.Add("@CSD_TYPE", gridListDto.GeometryType);
                        dynamicParameters.Add("@CSD_PAR1_CAP", gridListDto.CSD_PAR1_CAP);
                        dynamicParameters.Add("@CSD_PAR2_CAP", gridListDto.CSD_PAR2_CAP);
                        dynamicParameters.Add("@CSD_PAR3_CAP", gridListDto.CSD_PAR3_CAP);
                        dynamicParameters.Add("@CSD_END_POINT_X", gridListDto.CSD_END_POINT_X);
                        dynamicParameters.Add("@CSD_END_POINT_Y", gridListDto.CSD_END_POINT_Y);
                        dynamicParameters.Add("@CSD_TXT_X", gridListDto.CSD_TXT_X);
                        dynamicParameters.Add("@CSD_TXT_Y", gridListDto.CSD_TXT_Y);
                        dynamicParameters.Add("@CSD_INPUT_TYPE", gridListDto.Type);
                        dynamicParameters.Add("@CSD_PAR1", gridListDto.Length);
                        dynamicParameters.Add("@CSD_PAR2", gridListDto.Angle);
                        dynamicParameters.Add("@CSD_PAR3", gridListDto.Length2);
                        dynamicParameters.Add("@CSD_MATCH_PAR1", gridListDto.CSD_MATCH_PAR2);
                        dynamicParameters.Add("@CSD_MATCH_PAR2", gridListDto.CSD_MATCH_PAR2);
                        dynamicParameters.Add("@CSD_MATCH_PAR3", gridListDto.CSD_MATCH_PAR2 );
                        dynamicParameters.Add("@CSD_ARC_DIR", gridListDto.CSD_ARC_DIR);
                        dynamicParameters.Add("@CSD_CRT_BY", gridListDto.CSD_CRT_BY);
                        dynamicParameters.Add("@CSD_CRT_ON", gridListDto.CSD_CRT_ON);
                        dynamicParameters.Add("@CSD_UPD_BY", gridListDto.CSD_UPD_BY);
                        dynamicParameters.Add("@CSD_UPD_ON", gridListDto.CSD_UPD_ON);
                        dynamicParameters.Add("@CSD_HEIGHT_DIST", gridListDto.CSD_HEIGHT_DIST);
                        dynamicParameters.Add("@CSD_VIEW", gridListDto.CSD_VIEW);
                        dynamicParameters.Add("@CSD_VISIBLE", gridListDto.Visible);
                        dynamicParameters.Add("@CSD_ProdLength", gridListDto.CSD_ProdLength);


                        sqlConnection.Query<GridListDto>(SystemConstants.USP_INSERT_ShapeCodeDetails_Grid, dynamicParameters, commandType: CommandType.StoredProcedure);
                    }
                        sqlConnection.Close();

                    return true;

                }
            }
            catch (Exception ex)
            {
                return false;
                throw ex;

            }
        }

        // START: ADDED BY Tanmay FOR CAB SHAPE ACTIVE INACTIVE
        public bool CABShape_Status_Update(string StrShapeID, int status,out string errorMessage)
        {
            try
            {

                using(var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@STRSHAPEID", StrShapeID);
                    dynamicParameters.Add("@status", status);
                    sqlConnection.Query<int>(SystemConstants.CABShapeStatus_Update, dynamicParameters, commandType: CommandType.StoredProcedure);
                    errorMessage = "";
                    return true;
                }
               
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }
        // End


    }

}


