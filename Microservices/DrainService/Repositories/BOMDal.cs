using System.Data;
using System.Data.SqlClient;
using DrainService.Interfaces;
using System.Configuration;
using Dapper;
using DrainService.Constants;

namespace DrainService.Repositories
{
    public class BOMDal :BOMAbs
    {
        private Int32 intReturnValue;
        private readonly IConfiguration _configuration;
        private string connectionString;

        public BOMDal(IConfiguration configuration)
        {
            //_dbContext = dbContext;
            _configuration = configuration;

            connectionString = _configuration.GetConnectionString(SystemConstants.DefaultDBConnection);
        }

        public BOMDal()
        {
        }

        //public  DataSet GetBomUserid(NDSBLL.WBSInfo objWbsInfo)
        //{
        //    try
        //    {
        //        dbcom = DataAccess.DataAccess.db.GetStoredProcCommand("WBSTypeCreationMaster_Insert");
        //        dynamicParameters.Add("@vchWBSType",objWbsInfo.Output);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public DataSet GetBOMHeader(BOMInfo obj)
        {
            try
            {
                DataSet ds = new DataSet();
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intProductMarkingId",obj.ProductMarkingId);
                    dynamicParameters.Add("@nvchBOMType",obj.BOMType);
                    dynamicParameters.Add("@strStructureElement",obj.StructureElement);
                    ds = sqlConnection.QueryFirstOrDefault<DataSet>(SystemConstants.BOMHeader_Select, dynamicParameters, commandType: CommandType.StoredProcedure);
                    return ds;

                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public  DataSet GetBOM(BOMInfo obj)
        {
            try
            {
                DataSet ds = new DataSet();
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intProductMarkingId", obj.ProductMarkingId);
                    dynamicParameters.Add("@nvchBOMType", obj.BOMType);
                    dynamicParameters.Add("@strStructureElement", obj.StructureElement);
                    ds = sqlConnection.QueryFirstOrDefault<DataSet>(SystemConstants.BOM_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                    return ds;

                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public  DataSet GetRawMaterial()
        {
            try
            {
                DataSet ds = new DataSet();
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    
                    ds = sqlConnection.QueryFirstOrDefault<DataSet>(SystemConstants.RawMaterial_Get, commandType: CommandType.StoredProcedure);
                    return ds;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public  int InsertBOMDetails(BOMInfo obj)
        {
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@strBOMType", obj.BOMType);
                    dynamicParameters.Add("@intProductMarkingId", obj.ProductMarkingId);
                    dynamicParameters.Add("@intDetailingBOMDetailId", obj.DetailingBOMDetailId);
                    dynamicParameters.Add("@strStructureElement", obj.StructureElement);
                    dynamicParameters.Add("@chrWireType", obj.WireType);
                    dynamicParameters.Add("@strLineNo", obj.LineNo);
                    dynamicParameters.Add("@strStartPos", obj.StartPosition);
                    dynamicParameters.Add("@strNoPths", obj.NoOfPitch);
                    dynamicParameters.Add("@strWireSpace", obj.WireSpace);
                    dynamicParameters.Add("@strWireLen", obj.WireLength);
                    dynamicParameters.Add("@strWireDia", obj.WireDiameter);
                    dynamicParameters.Add("@strRawMaterial", obj.RawMaterial);
                    dynamicParameters.Add("@strRepFrom", obj.RepFrom);
                    dynamicParameters.Add("@strRepTo", obj.RepTo);
                    dynamicParameters.Add("@strRep", obj.Rep);
                    dynamicParameters.Add("@bitTwinWire",obj.TwinWire);
                    dynamicParameters.Add("@intMO1", obj.MO1);
                    dynamicParameters.Add("@intMO2", obj.MO2);
                    dynamicParameters.Add("@intCO1", obj.CO1);
                    dynamicParameters.Add("@intCO2", obj.CO2);
                    dynamicParameters.Add("@intUserId", obj.UserId);
                    dynamicParameters.Add("@Output", null, dbType: DbType.Int32, ParameterDirection.Output);
                    intReturnValue = sqlConnection.QueryFirstOrDefault<int>(SystemConstants.BOM_Insert, dynamicParameters, commandType: CommandType.StoredProcedure);
                    intReturnValue = dynamicParameters.Get<int>("@Output");
                    return intReturnValue;

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public  int DeleteBOMDetails(BOMInfo obj)
        {
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intDetailingBOMDetailId", obj.DetailingBOMDetailId);
                    dynamicParameters.Add("@Output",0);
                    intReturnValue = sqlConnection.QueryFirstOrDefault<int>(SystemConstants.BOM_Delete, dynamicParameters, commandType: CommandType.StoredProcedure);
                    intReturnValue = dynamicParameters.Get<int>("@Output");
                    return intReturnValue;

                }
              
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public  int InsertBOMHeaderDetails(BOMInfo obj)
        {
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intProductMarkingId", obj.ProductMarkingId);
                    dynamicParameters.Add("@intMO1", obj.MO1);
                    dynamicParameters.Add("@intMO2", obj.MO2);
                    dynamicParameters.Add("@intCO1", obj.CO1);
                    dynamicParameters.Add("@intCO2", obj.CO2);
                    dynamicParameters.Add("@nvchBOMType", obj.BOMType);
                    dynamicParameters.Add("@intUserId", obj.UserId);
                    dynamicParameters.Add("@strStructureElement", obj.StructureElement);
                    dynamicParameters.Add("@Output", null, dbType: DbType.Int32, ParameterDirection.Output);
                    intReturnValue = sqlConnection.QueryFirstOrDefault<int>(SystemConstants.BOMHeaderDetails_Update, dynamicParameters, commandType: CommandType.StoredProcedure);
                    //intReturnValue = dynamicParameters.Get<int>("@Output");
                    return intReturnValue;

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public  DataSet GetWireType(BOMInfo obj)
        {
            try
            {
                DataSet ds = new DataSet();
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intProductMarkingId", obj.ProductMarkingId);
                    ds = sqlConnection.QueryFirstOrDefault<DataSet>(SystemConstants.WireType_Get,dynamicParameters ,commandType: CommandType.StoredProcedure);
                    return ds;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public  int BOMCheck(BOMInfo obj)
        {
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intProductMarkingId", obj.ProductMarkingId);
                    dynamicParameters.Add("@nvchBOMType", obj.BOMType);
                    dynamicParameters.Add("@strStructureElement", obj.StructureElement);
                    dynamicParameters.Add("@Output", null, dbType: DbType.Int32, ParameterDirection.Output);
                    intReturnValue = sqlConnection.QueryFirstOrDefault<int>(SystemConstants.BOMData_Check, dynamicParameters, commandType: CommandType.StoredProcedure);
                    intReturnValue = dynamicParameters.Get<int>("@Output");
                    return intReturnValue;

                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public  SqlDataReader GetWireDiameter(BOMInfo obj)
        {
            SqlDataReader sqlDataReader = null;
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@chrWireType", obj.WireType);
                    dynamicParameters.Add("@intProductCodeId", obj.RawMaterial);
                    sqlDataReader = sqlConnection.QueryFirstOrDefault<SqlDataReader>(SystemConstants.WireDiameter_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                    return sqlDataReader;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public  DataSet GetBOMDrawingHeader(BOMInfo obj)
        {
            try
            {
                DataSet ds = new DataSet();
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intProductMarkingId", obj.ProductMarkingId);
                    dynamicParameters.Add("@strStructureElement", obj.StructureElement);
                    ds = sqlConnection.QueryFirstOrDefault<DataSet>(SystemConstants.BOMDrawingHeader_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                    return ds;

                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public  int IsBendingCheckRequired(BOMInfo obj)
        {
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@PRODUCTMARKID", obj.ProductMarkingId);
                    dynamicParameters.Add("@STRUCTUREELEMENTTYPE", obj.StructureElement);
                    dynamicParameters.Add("@Output", null, dbType: DbType.Int32, ParameterDirection.Output);
                    intReturnValue = sqlConnection.QueryFirstOrDefault<int>(SystemConstants.usp_IsBendingCheckRequiredForProductionBOM_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                    intReturnValue = dynamicParameters.Get<int>("@Output");
                    return intReturnValue;

                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public  int UpdateProductionBOMDetails(BOMInfo obj)
        {
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intProductMarkingId", obj.ProductMarkingId);
                    dynamicParameters.Add("@strStructureElement", obj.StructureElement);
                    dynamicParameters.Add("@intUserId", obj.UserId);
                    dynamicParameters.Add("@Output", null, dbType: DbType.Int32, ParameterDirection.Output);
                    intReturnValue = sqlConnection.QueryFirstOrDefault<int>(SystemConstants.GeneratePrdBOMFromDetailing_Insert, dynamicParameters, commandType: CommandType.StoredProcedure);
                    intReturnValue = dynamicParameters.Get<int>("@Output");
                    return intReturnValue;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public  SqlDataReader GetBOMEditStatus(BOMInfo obj)
        {
            try
            {
                SqlDataReader sqlDataReader = null;
                
                    using (var sqlConnection = new SqlConnection(connectionString))
                    {
                        sqlConnection.Open();
                        var dynamicParameters = new DynamicParameters();
                        dynamicParameters.Add("@intProductMarkId", obj.ProductMarkingId);
                        dynamicParameters.Add("@chrBOMType", obj.BOMType);
                        dynamicParameters.Add("@vchWireType ", obj.WireType);
                        dynamicParameters.Add("@strStructureElement", obj.StructureElement);
                        sqlDataReader = sqlConnection.QueryFirstOrDefault<SqlDataReader>(SystemConstants.BOMEditStatus_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                        return sqlDataReader;
                    }
                
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        // -------------------------------- For Bending Check --------------------------------- 
        public  DataSet GetDoubleBend(BOMInfo obj)
        {
            try
            {
                DataSet ds = new DataSet();
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intProductMarkingId", obj.ProductMarkingId);
                    ds = sqlConnection.QueryFirstOrDefault<DataSet>(SystemConstants.BendType_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                    return ds;

                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public  DataSet GetBendingCheck(BOMInfo obj)
        {
            try
            {
                DataSet ds = new DataSet();
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intProductMarkingId", obj.ProductMarkingId);
                    dynamicParameters.Add("@intShapeId", obj.ShapeId);
                    dynamicParameters.Add("@strStructureelementType", obj.StructureElement);
                    ds = sqlConnection.QueryFirstOrDefault<DataSet>(SystemConstants.BendingCheck_Select, dynamicParameters, commandType: CommandType.StoredProcedure);
                    return ds;

                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public  int GetOverHang(BOMInfo obj)
        {
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intProductMarkingId", obj.ProductMarkingId);
                    dynamicParameters.Add("@strStructureElement", obj.StructureElement);
                    // dynamicParameters.Add("@strShape",obj.Shape)
                    dynamicParameters.Add("@strMOH1", obj.MOH1);
                    dynamicParameters.Add("@strMOH2", obj.MOH2);
                    dynamicParameters.Add("@strCOH1", obj.COH1);
                    dynamicParameters.Add("@strCOH2", obj.COH2);
                    dynamicParameters.Add("@strOverHang", obj.OverHang);
                    dynamicParameters.Add("@intOutput", null, dbType: DbType.Int32, ParameterDirection.Output);
                    intReturnValue = sqlConnection.QueryFirstOrDefault<int>(SystemConstants.OverHang_Insert, dynamicParameters, commandType: CommandType.StoredProcedure);
                    intReturnValue = dynamicParameters.Get<int>("@intOutput");
                    return intReturnValue;

                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public  int GetSpaceShift(BOMInfo obj)
        {
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intProductMarkingId", obj.ProductMarkingId);
                    dynamicParameters.Add("@strStructureElement", obj.StructureElement);
                    dynamicParameters.Add("@strShape", obj.Shape);
                    dynamicParameters.Add("@strParametersForCWSpace", obj.ParametersForCWSpace);
                    dynamicParameters.Add("@strParametersForMWSpace", obj.ParametersForMWSpace);
                    dynamicParameters.Add("@strMOH1", obj.MOH1);
                    dynamicParameters.Add("@strMOH2", obj.MOH2);
                    dynamicParameters.Add("@strCOH1", obj.COH1);
                    dynamicParameters.Add("@strCOH2", obj.COH2);
                    dynamicParameters.Add("@strOverHang", obj.OverHang);
                    dynamicParameters.Add("@intOutput", null, dbType: DbType.Int32, ParameterDirection.Output);
                    intReturnValue = sqlConnection.QueryFirstOrDefault<int>(SystemConstants.SpaceShift_Insert, dynamicParameters, commandType: CommandType.StoredProcedure);
                    intReturnValue = dynamicParameters.Get<int>("@intOutput");
                    return intReturnValue;

                }
                
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public  int GetWireRemovalPass(BOMInfo obj)
        {
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intProductMarkingId", obj.ProductMarkingId);
                    dynamicParameters.Add("@strStructureElement", obj.StructureElement);
                    dynamicParameters.Add("@strShape", obj.Shape);
                    dynamicParameters.Add("@strParametersForCWSpace", obj.ParametersForCWSpace);
                    dynamicParameters.Add("@strParametersForMWSpace", obj.ParametersForMWSpace);
                    dynamicParameters.Add("@strOverHang", obj.OverHang);
                    dynamicParameters.Add("@intOutput", null, dbType: DbType.Int32, ParameterDirection.Output);
                    intReturnValue = sqlConnection.QueryFirstOrDefault<int>(SystemConstants.WireRemovalPass_Insert, dynamicParameters, commandType: CommandType.StoredProcedure);
                    intReturnValue = dynamicParameters.Get<int>("@intOutput");
                    return intReturnValue;

                }
                
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public  int GetWireRemovalFail(BOMInfo obj)
        {
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intProductMarkingId", obj.ProductMarkingId);
                    dynamicParameters.Add("@strStructureElement", obj.StructureElement);
                    dynamicParameters.Add("@strShape", obj.Shape);
                    dynamicParameters.Add("@strWireForCW", obj.ParametersForCWSpace);
                    dynamicParameters.Add("@strWireForMW", obj.ParametersForMWSpace);
                    dynamicParameters.Add("@strTotalSpace", obj.TotalSpace);
                    dynamicParameters.Add("@strOverHang", obj.OverHang);
                    dynamicParameters.Add("@intOutput", null, dbType: DbType.Int32, ParameterDirection.Output);
                    intReturnValue = sqlConnection.QueryFirstOrDefault<int>(SystemConstants.WireRemovalFail_Insert, dynamicParameters, commandType: CommandType.StoredProcedure);
                    intReturnValue = dynamicParameters.Get<int>("@intOutput");
                    return intReturnValue;

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public  DataSet GetProductionMWLength(BOMInfo obj)
        {
            try
            {
                DataSet ds = new DataSet();
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intProductMarkingId", obj.ProductMarkingId);

                    ds = sqlConnection.QueryFirstOrDefault<DataSet>(SystemConstants.BCProdMWLength_Select, dynamicParameters, commandType: CommandType.StoredProcedure);
                    return ds;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     ''' ''validate user
        ///     ''' </summary>
        ///     ''' <param name="obj"></param>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public  string[] ValidateUser(BOMInfo obj)
        {
            try
            {

                string[] strResult = new string[4];

                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@vchLoginId", obj.LoginId);
                    dynamicParameters.Add("@vchFormName", obj.FormName);
                    dynamicParameters.Add("@bitReadAccess", DbType.Boolean, 0);
                    dynamicParameters.Add("@bitWriteAccess", DbType.Boolean, 0);
                    dynamicParameters.Add("@intUserId", DbType.Int64, 0);
                    dynamicParameters.Add("@vchEmailId", 100);

                    sqlConnection.Query<string>(SystemConstants.UserRights_Validate, dynamicParameters, commandType: CommandType.StoredProcedure);

                    strResult[0] = dynamicParameters.Get<string>("@bitReadAccess");
                    strResult[1] = dynamicParameters.Get<string>("@bitWriteAccess");
                    strResult[2] = dynamicParameters.Get<string>("@intUserId");
                    strResult[3] = dynamicParameters.Get<string>("@vchEmailId");
                    sqlConnection.Close();

                    return strResult;

                }
                   
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // ------------------------------Get BOM Details For PDF ------------------------------------------------------
        public  DataSet GetBOMDetailsForPDF(BOMInfo obj)
        {
            try
            {
                DataSet ds = new DataSet();
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intProductMarkingId", obj.ProductMarkingId);
                    dynamicParameters.Add("@nvchBOMType", obj.BOMType);
                    dynamicParameters.Add("@strStructureElement", obj.StructureElement);
                    ds = sqlConnection.QueryFirstOrDefault<DataSet>(SystemConstants.BOMPdf_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                    return ds;

                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // -------------Update PDF path in respective ProductMarking tables for Beam, Column and Slab-----------------

        public  DataSet UpdateBOMPDF(BOMInfo obj)
        {
            try
            {
                DataSet ds = new DataSet();
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intProductMarkingId", obj.ProductMarkingId);
                    dynamicParameters.Add("@strStructureElement", obj.StructureElement);
                    dynamicParameters.Add("@BOMPDFPath", obj.BOMPDFPath);
                    ds = sqlConnection.QueryFirstOrDefault<DataSet>(SystemConstants.BOMPdfPath_Update, dynamicParameters, commandType: CommandType.StoredProcedure);
                    return ds;

                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
