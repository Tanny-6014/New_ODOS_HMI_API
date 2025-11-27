using Dapper;
using MongoDB.Driver.Core.Configuration;
using OrderService.Constants;
using OrderService.Dtos;
using OrderService.Interfaces;
using OrderService.NDSPosting;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;


namespace OrderService.Repositories
{
    public class PrecastService : IPrecastService
    {
        private string connectionString = "Server=NSPRDDB19\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=ndswebapps;Password=DBAdmin4*NDS;Connection Timeout=36000000";
        //private string connectionString = "Server=NSPRDDB19\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=ndswebapps;Password=DBAdmin4*NDS;Connection Timeout=36000000";

        public PrecastService()
        {
        }

        #region PrecastAPI
        public List<getPrecastDto> GetPrecastDetails()
        {
            List<getPrecastDto> records = new List<getPrecastDto>();

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();

                records = (List<getPrecastDto>)sqlConnection.Query<getPrecastDto>(SystemConstant.usp_getPrecastDetails, commandType: CommandType.StoredProcedure);


            }
            return records;
        }



        public int InsertPrecastDetails(getPrecastDto getPrecast)
        {

            int addedPrecastID = 0;


            try
            {
                using (var sqlconnection = new SqlConnection(connectionString))
                {
                    sqlconnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@Precast_ID",null);
                    dynamicParameters.Add("@CustomerCode", getPrecast.CustomerCode);
                    dynamicParameters.Add("@ProjectCode", getPrecast.ProjectCode);
                    dynamicParameters.Add("@ComponentMarking", getPrecast.ComponentMarking);
                    dynamicParameters.Add("@Block", getPrecast.Block);
                    dynamicParameters.Add("@Level", getPrecast.Level);
                    dynamicParameters.Add("@Qty", getPrecast.Qty);
                    dynamicParameters.Add("@Remark", getPrecast.Remark);
                    dynamicParameters.Add("@PageNo", getPrecast.PageNo);
                    dynamicParameters.Add("@StructureElement", getPrecast.StructureElement);
                    dynamicParameters.Add("@CreatedBy", getPrecast.CreatedBy);
                    dynamicParameters.Add("@ModifiedBy",null);

                    dynamicParameters.Add("@Output", null, dbType: DbType.Int32, ParameterDirection.Output);

                    sqlconnection.QueryFirstOrDefault<int>(SystemConstant.usp_PrecastDetaildInsert, dynamicParameters, commandType: CommandType.StoredProcedure);

                    addedPrecastID = dynamicParameters.Get<int>("Output");
                    sqlconnection.Close();

                    return addedPrecastID;


                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public int UpdatePrecastDetails(getPrecastDto getPrecast)
        {

            int retunvalue = 0;


            try
            {
                using (var sqlconnection = new SqlConnection(connectionString))
                {
                    sqlconnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@Precast_ID", getPrecast.Precast_ID);
                    dynamicParameters.Add("@CustomerCode", getPrecast.CustomerCode);
                    dynamicParameters.Add("@ProjectCode", getPrecast.ProjectCode);
                    dynamicParameters.Add("@ComponentMarking", getPrecast.ComponentMarking);
                    dynamicParameters.Add("@Block", getPrecast.Block);
                    dynamicParameters.Add("@Level", getPrecast.Level);
                    dynamicParameters.Add("@Qty", getPrecast.Qty);
                    dynamicParameters.Add("@Remark", getPrecast.Remark);
                    dynamicParameters.Add("@PageNo", getPrecast.PageNo);
                    dynamicParameters.Add("@StructureElement", getPrecast.StructureElement);
                    dynamicParameters.Add("@CreatedBy", null);
                    dynamicParameters.Add("@ModifiedBy", getPrecast.ModifiedBy);

                    dynamicParameters.Add("@Output", null, dbType: DbType.Int32, ParameterDirection.Output);

                    sqlconnection.QueryFirstOrDefault<int>(SystemConstant.usp_PrecastDetaildInsert, dynamicParameters, commandType: CommandType.StoredProcedure);

                    retunvalue = dynamicParameters.Get<int>("Output");
                    sqlconnection.Close();

                    return retunvalue;


                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }


public int UpdatePrecastFlag(int PostheaderID, int flag)

        {

            int retunvalue = 0;


            try

            {

                using (var sqlconnection = new SqlConnection(connectionString))

                {

                    sqlconnection.Open();

                    var dynamicParameters = new DynamicParameters();

                    dynamicParameters.Add("@PostHeaderID", PostheaderID);

                    dynamicParameters.Add("@flag", flag);

                    sqlconnection.QueryFirstOrDefault<int>(SystemConstant.usp_UpdatePrecastFlag, dynamicParameters, commandType: CommandType.StoredProcedure);

                    sqlconnection.Close();

                    return retunvalue;

                }

            }

            catch (Exception ex)

            {

                throw ex;

            }

            finally

            {

            }

        }


        public bool DeletePrecastDetailsById(int precastId)

        {

            using (SqlConnection conn = new SqlConnection(connectionString))

            {

                using (SqlCommand cmd = new SqlCommand("[dbo].[DeletePrecastDetailsById]", conn))

                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Precast_ID", precastId);

                    conn.Open();

                    int rowsdeleted = cmd.ExecuteNonQuery();

                    return rowsdeleted > 0; // Return true if a record was deleted

                }

            }

        }


        #endregion


        #region BarShapeCode
        public List<string> GetDistinctBarShapeCodes()
        {
            string query = "SELECT distinct BarShapeCode FROM OESBPCCageBars";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                return conn.Query<string>(query).AsList(); // Executes the query and returns a list of strings
            }
        }

        public int UpdateBarShapeCodeDetails(getBarShapeCodeDto getBarShape)
        {
            int intReturnValue = 0;
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {


                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@CustomerCode", getBarShape.CustomerCode);
                    dynamicParameters.Add("@ProjectCode", getBarShape.ProjectCode);
                    dynamicParameters.Add("@JobID", getBarShape.JobID);
                    dynamicParameters.Add("@CageID", getBarShape.CageID);
                    dynamicParameters.Add("@BarID", getBarShape.BarID);
                    dynamicParameters.Add("@BarMark", getBarShape.BarMark);
                    dynamicParameters.Add("@BarType", getBarShape.BarType);
                    dynamicParameters.Add("@BarSize", getBarShape.BarSize);
                    dynamicParameters.Add("@BarTotalQty", getBarShape.BarTotalQty);
                    dynamicParameters.Add("@BarShapeCode", getBarShape.BarShapeCode);
                    dynamicParameters.Add("@A", getBarShape.A);
                    dynamicParameters.Add("@B", getBarShape.B);
                    dynamicParameters.Add("@C", getBarShape.C);
                    dynamicParameters.Add("@D", getBarShape.D);
                    dynamicParameters.Add("@E", getBarShape.E);
                    dynamicParameters.Add("@F", getBarShape.F);
                    dynamicParameters.Add("@G", getBarShape.G);
                    dynamicParameters.Add("@BarLength", getBarShape.BarLength);
                    dynamicParameters.Add("@BarWeight", getBarShape.BarWeight);
                    dynamicParameters.Add("@Remarks", getBarShape.Remarks);
                    dynamicParameters.Add("@output", null, dbType: DbType.Int32, ParameterDirection.Output);

                    sqlConnection.QueryFirstOrDefault<int>(SystemConstant.BarIdDetails_Update, dynamicParameters, commandType: CommandType.StoredProcedure);

                    intReturnValue = dynamicParameters.Get<int>("output");

                    return intReturnValue;

                }
            }
            catch (Exception ex)
            {
            }
            return intReturnValue;

        }

        #endregion

        #region BPCJobAdviceAPI

        public List<BPCJobAdviceDto> GetBPCJobAdviceDetails(string CustomerCode, string ProjectCode, int JobId)

        {

            IEnumerable<BPCJobAdviceDto> bPCJobAdvices = null;

            using (var sqlConnection = new SqlConnection(connectionString))

            {

                try

                {

                    sqlConnection.Open();

                }

                catch (SqlException ex)

                {

                    Console.WriteLine("SQL Connection Error: " + ex.Message);

                }

                var dynamicParameters = new DynamicParameters();

                dynamicParameters.Add("@CustomerCode", CustomerCode);

                dynamicParameters.Add("@ProjectCode", ProjectCode);

                dynamicParameters.Add("@JobId", JobId);

                bPCJobAdvices = sqlConnection.Query<BPCJobAdviceDto>(SystemConstant.usp_getBPCJobAdviceDetails, dynamicParameters, commandType: CommandType.StoredProcedure);

                //records = (List<BPCJobAdviceDto>)sqlConnection.Query<BPCJobAdviceDto>(SystemConstant.usp_getBPCJobAdviceDetails, dynamicParameters, commandType: CommandType.StoredProcedure);


            }

            return bPCJobAdvices.ToList();

        }

        public int InsertBPCJobAdviceDetails(BPCJobAdviceInsertDto bPCJob, int UserId)

        {

            int InsertedId = 0;

            try

            {

                using (var sqlconnection = new SqlConnection(connectionString))

                {

                    sqlconnection.Open();

                    var dynamicParameters = new DynamicParameters();

                    dynamicParameters.Add("@CustomerCode", bPCJob.CustomerCode);

                    dynamicParameters.Add("@ProjectCode", bPCJob.ProjectCode);

                    dynamicParameters.Add("@PileDia", bPCJob.PileDia);

                    dynamicParameters.Add("@NoofMainBar", bPCJob.NoofMainBar);

                    dynamicParameters.Add("@CageLength", bPCJob.CageLength);

                    dynamicParameters.Add("@L1", bPCJob.L1);

                    dynamicParameters.Add("@NOs", bPCJob.NOs);

                    dynamicParameters.Add("@Dia", bPCJob.Dia);

                    dynamicParameters.Add("@Spacing", bPCJob.Spacing);

                    dynamicParameters.Add("@L2", bPCJob.L2);

                    dynamicParameters.Add("@StraightCrank", bPCJob.StraightCrank);

                    dynamicParameters.Add("@Qty", bPCJob.Qty);

                    dynamicParameters.Add("@TypeOfCages", bPCJob.TypeOfCages);

                    dynamicParameters.Add("@EstimationWt", bPCJob.EstimationWt);

                    dynamicParameters.Add("@Remark", bPCJob.Remark);

                    dynamicParameters.Add("@JobID", bPCJob.JobId);

                    dynamicParameters.Add("@CreatedBy", UserId);

                    dynamicParameters.Add("@ConcreteCover", bPCJob.Concrete_cover);
                    dynamicParameters.Add("@PileType", bPCJob.PileType);
                    dynamicParameters.Add("@DeliveryDate", bPCJob.DeliveryDate);

                    dynamicParameters.Add("@Output", null, dbType: DbType.Int32, ParameterDirection.Output);

                    sqlconnection.QueryFirstOrDefault<int>(SystemConstant.usp_InsertBPCJobAdviceDetails, dynamicParameters, commandType: CommandType.StoredProcedure);

                    InsertedId = dynamicParameters.Get<int>("Output");

                    sqlconnection.Close();

                    return InsertedId;


                }

            }

            catch (Exception ex)

            {

                throw ex;

            }

            finally

            {

            }

        }


        public int UpdateBPCJobAdviceDetails(BPCJobAdviceInsertDto bPCJob, int UserId)

        {

            int retunvalue = 0;

            try

            {

                using (var sqlconnection = new SqlConnection(connectionString))

                {

                    sqlconnection.Open();

                    var dynamicParameters = new DynamicParameters();

                    dynamicParameters.Add("@bpc_ID", bPCJob.bpc_ID);

                    //dynamicParameters.Add("@CustomerCode", bPCJob.CustomerCode);

                    //dynamicParameters.Add("@ProjectCode", bPCJob.ProjectCode);

                    dynamicParameters.Add("@PileName", bPCJob.PileDia);

                    dynamicParameters.Add("@NoofMainBar", bPCJob.NoofMainBar);

                    dynamicParameters.Add("@CageLength", bPCJob.CageLength);

                    dynamicParameters.Add("@L1", bPCJob.L1);

                    dynamicParameters.Add("@NOs", bPCJob.NOs);

                    dynamicParameters.Add("@Dia", bPCJob.Dia);

                    dynamicParameters.Add("@Spacing", bPCJob.Spacing);

                    dynamicParameters.Add("@L2", bPCJob.L2);

                    dynamicParameters.Add("@StraightCrank", bPCJob.StraightCrank);

                    dynamicParameters.Add("@Qty", bPCJob.Qty);

                    dynamicParameters.Add("@TypeOfCages", bPCJob.TypeOfCages);

                    dynamicParameters.Add("@EstimationWt", bPCJob.EstimationWt);

                    dynamicParameters.Add("@Remark", bPCJob.Remark);

                    dynamicParameters.Add("@ModifiedBy", UserId);

                    dynamicParameters.Add("@ConcreteCover", bPCJob.Concrete_cover);
                    dynamicParameters.Add("@PileType", bPCJob.PileType);
                    dynamicParameters.Add("@DeliveryDate", bPCJob.DeliveryDate);

                    dynamicParameters.Add("@Output", null, dbType: DbType.Int32, ParameterDirection.Output);

                    sqlconnection.QueryFirstOrDefault<int>(SystemConstant.usp_UpdateBPCJobAdviceDetails, dynamicParameters, commandType: CommandType.StoredProcedure);

                    retunvalue = dynamicParameters.Get<int>("Output");

                    sqlconnection.Close();

                    return retunvalue;

                }

            }

            catch (Exception ex)

            {

                throw ex;

            }

            finally

            {

            }

        }


        public bool DeleteBPCJobAdviceDetails(int BpcJobID)

        {

            using (SqlConnection conn = new SqlConnection(connectionString))

            {

                using (SqlCommand cmd = new SqlCommand("[dbo].[DeleteBPCJobAdviceDetails]", conn))

                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@bpc_ID", BpcJobID);

                    conn.Open();

                    int rowsdeleted = cmd.ExecuteNonQuery();

                    return rowsdeleted > 0;

                }

            }

        }


        #endregion


        public async Task<bool?> GetIsPrecast(string customerCode, string projectCode)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"SELECT IsPrecast FROM [dbo].[PrecastCustomerProjectMaster]
                                 WHERE CustomerCode = @CustomerCode AND ProjectCode = @ProjectCode";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CustomerCode", customerCode);
                        cmd.Parameters.AddWithValue("@ProjectCode", projectCode);

                        await conn.OpenAsync();
                        var result = await cmd.ExecuteScalarAsync();

                        if (result != null && bool.TryParse(result.ToString(), out bool isPrecast))
                        {
                            return isPrecast;
                        }

                        return false;
                    }
                }
            }
            catch(Exception ex)
            {
                throw ;
            }
          
        }

    }


}
