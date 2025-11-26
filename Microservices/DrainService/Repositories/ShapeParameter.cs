using Dapper;
using DrainService.Constants;
using System.Data;
using System.Data.SqlClient;

namespace DrainService.Repositories
{
    public class ShapeParameter
    {
        private readonly IConfiguration _configuration;
        private string connectionString;
       
        #region Variables

        public ShapeCode Shape { get; set; }
        public int ShapeId { get; set; }
        public string ParameterName { get; set; }
        public int ParameterValue { get; set; }
        public string CriticalIndiacator { get; set; }
        public int SequenceNumber { get; set; }
        public string ShapeCodeImage { get; set; }
        public bool EditFlag { get; set; }
        public bool VisibleFlag { get; set; }
        public string symmetricIndex { get; set; }
        public string HeghtAngleFormula { get; set; }
        public string HeghtSuceedAngleFormula { get; set; }
        // Property for CABITEM - Added by Lakshmi
        public String Parameter { get; set; }
        public string Value { get; set; }
        public bool IsVisible { get; set; }
        public string AngleType { get; set; }
        public string WireType { get; set; }
        public string OHDtls { get; set; }
        public string PrintFlag { get; set; }
        public Int32 PrintValue { get; set; }
        public int EvenMo1 { get; set; }
        public int EvenMo2 { get; set; }
        public int OddMo1 { get; set; }
        public int OddMo2 { get; set; }
        public int EvenCo1 { get; set; }
        public int EvenCo2 { get; set; }
        public int OddCo1 { get; set; }
        public int OddCo2 { get; set; }
        public bool OHIndicator { get; set; }
        public int AngleDir { get; set; }
        public string Angle { get; set; }
        public bool IsParamUsed { get; set; }

        public int IntXCord { get; set; }
        public int IntYCord { get; set; }
        public int IntZCord { get; set; }
        public string CustFormula { get; set; }
        public string OffsetAngleFormula { get; set; }
        public string OffsetSuceedAngleFormula { get; set; }
        public String ParameterValueCab { get; set; }
        public string CouplerType { get; set; }
        public string ParameterView { get; set; }

        // Added for CAB Shape Param validation CR : Round Off handle Logic
        public int RoundOffRange { get; set; }

        #endregion


        public ShapeParameter()
        {

        }

        public ShapeParameter(IConfiguration configuration)
        {
           // _dbContext = dbContext;
            _configuration = configuration;

            connectionString = _configuration.GetConnectionString(SystemConstants.DefaultDBConnection);
        }

        #region start for dwall prc report
        /// <summary>
        /// Prepare data for Dwall report
        /// </summary>
        /// <param name="groupmarkid"></param>
        public void insertcabproductmark(int groupmarkid)
        {


            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();

                    dynamicParameters.Add("@groupmarkid", groupmarkid);
                    //dbManager.ExecuteScalar(CommandType.StoredProcedure, "usp_CubeShapeParameter_Insert_forGm");
                    sqlConnection.Query<int>(SystemConstants.CubeShapeParameter_Insert_forGm, dynamicParameters, commandType: CommandType.StoredProcedure);



                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Get shape parameter for CAB
        /// </summary>
        /// <param name="groupmarkid"></param>
        /// <returns></returns>
		public DataSet getcabproductmark(int groupmarkid)
        {

            DataSet ds = new DataSet();
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@groupmarkid", groupmarkid);
                    // ds = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_CubeShapeParameter_GET_forgm");
                    ds = (DataSet)sqlConnection.Query<DataSet>(SystemConstants.CubeShapeParameter_GET_forgm, dynamicParameters, commandType: CommandType.StoredProcedure);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ds;
        }
        #endregion

        /// <summary>
        /// To Save Shape Parameters.
        /// </summary>
        /// <param name="intShapeTransHeaderId"></param>
        /// <returns></returns>  
        public bool Save(int intShapeTransHeaderId)
        {

            bool isSuccess = false;
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@INTSHAPETRANSHEADERID", intShapeTransHeaderId);
                    dynamicParameters.Add("@VCHSEGMENTPARAMETER", this.Parameter);
                    dynamicParameters.Add("@INTSEGMENTVALUE", this.Value);
                    dynamicParameters.Add("@BITPRINTTAG", this.PrintValue);
                    dynamicParameters.Add("@INTPARAMSEQ", this.SequenceNumber);
                    //dbManager.ExecuteScalar(CommandType.StoredProcedure, "USP_CABSHAPEPARAMETER_PARALLEL_INSERT");
                    sqlConnection.Query<int>(SystemConstants.CABSHAPEPARAMETER_PARALLEL_INSERT, dynamicParameters, commandType: CommandType.StoredProcedure);


                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isSuccess;
        }
    }
}
