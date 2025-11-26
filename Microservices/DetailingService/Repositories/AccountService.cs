using DetailingService.Context;
using DetailingService.Interfaces;
using DetailingService.Constants;
using Dapper;
using DetailingService.Dtos;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DetailingService.Repositories
{
    public class AccountService : IAccountService
    {
        private DetailingApplicationContext _dbContext;
        private readonly IConfiguration _configuration;
        //private string connectionString = "Server=nsprddb10\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=ndswebapps;Password=DBAdmin4*NDS;Connection Timeout=36000000";
        private string connectionString = "Server=NSQADB5\\MSSQL2022;Database=ODOS;TrustServerCertificate=True;MultipleActiveResultSets=true;User=MES_USER;Password=Mes@123;Connection Timeout=36000000";

        public AccountService(DetailingApplicationContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString(SystemConstant.DefaultDBConnection);
        }


        public List<DetailingFormDto> GetDetailingForm(string Username)
        {
            try
            {
                //return await _dbContext.ProjectMaster.ToListAsync();
                IEnumerable<DetailingFormDto> result; //new IEnumerable<List<ProjectMaster>>();
                List<DetailingFormDto> result2; //new IEnumerable<List<ProjectMaster>>();

                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@vchLoginId", Username);
                    result = sqlConnection.Query<DetailingFormDto>(SystemConstant.DetailingForm, dynamicParameters, commandType: CommandType.StoredProcedure);
                    sqlConnection.Close();
                    result2 = result.ToList();
                    return result2;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
