using Dapper;
using DetailingService.Constants;
using DetailingService.Context;
using DetailingService.Dtos;
using Microsoft.Data.SqlClient;
using SharedCache.WinServiceCommon.Provider.Cache;
using System.Data;

namespace DetailingService.Repositories
{
    public class PreferredMesh
    {
        private DetailingApplicationContext _dbContext;
        private readonly IConfiguration _configuration;

        //private string connectionString = "Server=nsprddb10\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=ndswebapps;Password=DBAdmin4*NDS;Connection Timeout=36000000";
        private string connectionString = "Server=NSQADB5\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=MES_USER;Password=Mes@123;Connection Timeout=36000000";

        #region
        public int MainWireSpacing { get; set; }
        public int ProjectTypeId { get; set; }
        public int MWMinLength { get; set; }
        public int MWMaxLength { get; set; }
        public int MWStep { get; set; }
        public int MWExcessLap { get; set; }
        public int CWMinLength { get; set; }
        public int CWMaxLength { get; set; }
        public int CWStep { get; set; }
        public int CWExcessLap { get; set; }
        #endregion


        public PreferredMesh()
        {

        }
        public PreferredMesh(DetailingApplicationContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;

            connectionString = _configuration.GetConnectionString(SystemConstant.DefaultDBConnection);
        }

        public List<PreferredMesh> PreferredMesh_Get()
        {
           // DBManager dbManager = new DBManager();
            List<PreferredMesh> listPreferredMesh = new List<PreferredMesh>();
            DataSet dsPreferredMesh = new DataSet();
            try
            {
                //if (IndexusDistributionCache.SharedCache.Get("PreferredMeshCache") == null)
                // {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    // dsPreferredMesh = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_PreferredMesh_Get");
                    dsPreferredMesh = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.PreferredMesh_Get, commandType: CommandType.StoredProcedure);


                    if (dsPreferredMesh != null && dsPreferredMesh.Tables.Count != 0)
                    {
                        foreach (DataRowView drvPreferredMesh in dsPreferredMesh.Tables[0].DefaultView)
                        {
                            PreferredMesh preferredmesh = new PreferredMesh
                            {
                                MainWireSpacing = Convert.ToInt32(drvPreferredMesh["INTMWSPACE"]),
                                ProjectTypeId = Convert.ToInt32(drvPreferredMesh["INTPROJECTTYPEID"]),
                                MWMinLength = Convert.ToInt32(drvPreferredMesh["DECMWMINLENGTH"]),
                                MWMaxLength = Convert.ToInt32(drvPreferredMesh["DECMWMAXLENGTH"]),
                                MWStep = Convert.ToInt32(drvPreferredMesh["TNTMWSTEP"]),
                                MWExcessLap = Convert.ToInt32(drvPreferredMesh["INTMWEXCESSLAP"]),
                                CWMinLength = Convert.ToInt32(drvPreferredMesh["DECCWMINLENGTH"]),
                                CWMaxLength = Convert.ToInt32(drvPreferredMesh["DECCWMAXLENGTH"]),
                                CWStep = Convert.ToInt32(drvPreferredMesh["TNTCWSTEP"]),
                                CWExcessLap = Convert.ToInt32(drvPreferredMesh["INTCWEXCESSLAP"])
                            };
                            listPreferredMesh.Add(preferredmesh);
                        }
                        // IndexusDistributionCache.SharedCache.Add("PreferredMeshCache", listPreferredMesh, DateTime.Today.AddMinutes(Convert.ToInt32(ConfigurationManager.AppSettings.Get("cacheTimeOut"))));
                        //}
                        //else
                        //{
                        //    listPreferredMesh = IndexusDistributionCache.SharedCache.Get("PreferredMeshCache") as List<PreferredMesh>;
                        //}
                    }
                }
            
            }
            catch (Exception)
            {
                throw;
            }
            //finally
            //{
            //    dbManager.Dispose();
            //}
            return listPreferredMesh;
        }

        public List<PreferredMesh> PreferredMeshData_Get(int productCodeId, int projectTypeId, int mwSpacing, int mwLength)
        {
            //DBManager dbManager = new DBManager();
            List<PreferredMesh> listPreferredMesh = new List<PreferredMesh>();
            DataSet dsPreferredMesh = new DataSet();
            IEnumerable<PreferredMeshDto> listPreferredMeshDto;
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@intProductCodeId", productCodeId);
                    dynamicParameters.Add("@intProjectTypeId", projectTypeId);
                    dynamicParameters.Add("@intMWSpace", mwSpacing);
                    dynamicParameters.Add("@decMWLength", mwLength);
                    // dsPreferredMesh = dbManager.ExecuteDataSet(CommandType.StoredProcedure, "usp_PreferredMeshData_Get");
                    //dsPreferredMesh = (DataSet)sqlConnection.Query<DataSet>(SystemConstant.PreferredMeshData_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                    listPreferredMeshDto = sqlConnection.Query<PreferredMeshDto>(SystemConstant.PreferredMeshData_Get, dynamicParameters, commandType: CommandType.StoredProcedure);

                    if (listPreferredMeshDto.Count() > 0)
                    {
                        DataTable dt = ConvertToDataTable.ToDataTable(listPreferredMeshDto);
                        dsPreferredMesh.Tables.Add(dt);
                    }
                        if (dsPreferredMesh != null && dsPreferredMesh.Tables.Count != 0)
                    {
                        foreach (DataRowView drvPreferredMesh in dsPreferredMesh.Tables[0].DefaultView)
                        {
                            PreferredMesh preferredmesh = new PreferredMesh
                            {
                                MainWireSpacing = Convert.ToInt32(drvPreferredMesh["INTMWSPACE"]),
                                ProjectTypeId = Convert.ToInt32(drvPreferredMesh["INTPROJECTTYPEID"]),
                                MWMinLength = Convert.ToInt32(drvPreferredMesh["DECMWMINLENGTH"]),
                                MWMaxLength = Convert.ToInt32(drvPreferredMesh["DECMWMAXLENGTH"]),
                                MWStep = Convert.ToInt32(drvPreferredMesh["TNTMWSTEP"]),
                                MWExcessLap = Convert.ToInt32(drvPreferredMesh["INTMWEXCESSLAP"]),
                                CWMinLength = Convert.ToInt32(drvPreferredMesh["DECCWMINLENGTH"]),
                                CWMaxLength = Convert.ToInt32(drvPreferredMesh["DECCWMAXLENGTH"]),
                                CWStep = Convert.ToInt32(drvPreferredMesh["TNTCWSTEP"]),
                                CWExcessLap = Convert.ToInt32(drvPreferredMesh["INTCWEXCESSLAP"])
                            };
                            listPreferredMesh.Add(preferredmesh);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            
            return listPreferredMesh;
        }
    }
}
