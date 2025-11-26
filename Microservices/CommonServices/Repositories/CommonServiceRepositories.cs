 using CommonServices.Context;
using CommonServices.Interfaces;
using CommonServices.Models;
using Microsoft.EntityFrameworkCore;
using CommonServices.Constants;
using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;
using CommonServices.Dtos;

namespace CommonServices.Repositories
{
    public class CommonServiceRepositories : ICommonService
    {
        private CommonServicesContext _dbContext;
        private readonly IConfiguration _configuration;      
        private readonly string connectionString;
        private readonly string STSconnectionString;

        public CommonServiceRepositories(CommonServicesContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;

            connectionString = _configuration.GetConnectionString(SystemConstant.DefaultDBConnection);
            STSconnectionString = _configuration.GetConnectionString(SystemConstant.STSDBConnection);
        }
       

        public async Task<List<CustomerMaster>> GetCustomerListAsync()
        {
            return await _dbContext.CustomerMaster.ToListAsync();
        }

        public async Task<IEnumerable<ProjectMaster>> GetProjectListAsync(string customerid)
        {
            //return await _dbContext.ProjectMaster.ToListAsync();
            IEnumerable<ProjectMaster> Projlist; //new IEnumerable<List<ProjectMaster>>();

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@intCustomerID", customerid.ToString());
                Projlist = sqlConnection.Query<ProjectMaster>(SystemConstant.Project_Get_new, dynamicParameters, commandType: CommandType.StoredProcedure);             
                sqlConnection.Close();

             
                return Projlist;


            }


        }

        public async Task<IEnumerable<ProjectMaster>> GetProjectListForcontractListAsync(int customerid)
        {           
            IEnumerable<ProjectMaster> Projlist;
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@intCustomerID", customerid);
                Projlist = sqlConnection.Query<ProjectMaster>(SystemConstant.ProjectcodeList, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();


                return Projlist;


            }


        }

        public async Task<IEnumerable<ProjectContractListDto>> GetProjectContractListAsync()
        {
            //return await _dbContext.ProjectMaster.ToListAsync();
            IEnumerable<ProjectContractListDto> ProjContractlist; //new IEnumerable<List<ProjectMaster>>();

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                

                ProjContractlist = sqlConnection.Query<ProjectContractListDto>(SystemConstant.Get_Project_Contract_List,  commandType: CommandType.StoredProcedure);
                sqlConnection.Close();


                return ProjContractlist;


            }


        }


        public async Task<IEnumerable<ProjectContractFilterDto>> GetProjectContractFilterAsync(int CustomerCode, string startDate, string endDate, int projectNo, string contractNo)
        {
            IEnumerable<ProjectContractFilterDto> ProjContractlist; //new IEnumerable<List<ProjectMaster>>();
            
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@CustomerCode", CustomerCode);
                dynamicParameters.Add("@startDate", startDate);
                dynamicParameters.Add("@enddate", endDate);
                dynamicParameters.Add("@intProjectId", projectNo);
                dynamicParameters.Add("@ContractCode", contractNo);

                ProjContractlist = sqlConnection.Query<ProjectContractFilterDto>(SystemConstant.GetProjectContractFilter, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return ProjContractlist;
                
            }


        }



        public async Task<IEnumerable<ContractDto>> GetContractListAsync(int projectid)
        {
            
            IEnumerable<ContractDto> ContractList;

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@Project_ID", projectid);
                ContractList =  sqlConnection.Query<ContractDto>(SystemConstant.GetContractByProject, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return  ContractList;


            }


        }

        public async Task<List<ProductType>> GetProductListAsync()
        {
            return await _dbContext.ProductType.ToListAsync();
        }

        public List<CustomerDto>GetESMCustomerList(bool isEsm)
        {

            IEnumerable<CustomerDto>customers;

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@IsESM", isEsm);
                customers = sqlConnection.Query<CustomerDto>(SystemConstant.usp_Customer_Get, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return customers.ToList();


            }


        }

        public List<getUsersDto> GetUsersList()
        {

            IEnumerable<getUsersDto> users;

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                
                users = sqlConnection.Query<getUsersDto>(SystemConstant.GetUsers, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return users.ToList();


            }


        }

        public List<EsmTonnageReportDto> GetEsmTonnageReport(string FromDate,string ToDate)
        {

            IEnumerable<EsmTonnageReportDto> esmTonnageReports;
            try 
            {

                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@FromDate", FromDate);
                    dynamicParameters.Add("@ToDate", ToDate);
                    esmTonnageReports = sqlConnection.Query<EsmTonnageReportDto>(SystemConstant.usp_ESMReleasedTonnageReport, dynamicParameters, commandType: CommandType.StoredProcedure);
                    sqlConnection.Close();
                    return esmTonnageReports.ToList();


                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
          


        }


        public List<TonnageReportDto> GetTonnageReport(string FromDate, string ToDate,string user,string rptType)
        {

            IEnumerable<TonnageReportDto> esmTonnageReports;
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@FromDate", FromDate);
                    dynamicParameters.Add("@ToDate", ToDate);
                    dynamicParameters.Add("@user", user);
                    dynamicParameters.Add("@rptType", rptType);
                    esmTonnageReports = sqlConnection.Query<TonnageReportDto>(SystemConstant.usp_TonnageReport, dynamicParameters, commandType: CommandType.StoredProcedure);
                    sqlConnection.Close();
                    return esmTonnageReports.ToList();


                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BPCOrderReportDto> GetBPCOrderEntryReport(string FromDate, string ToDate)
        {

            IEnumerable<BPCOrderReportDto> esmTonnageReports;
            try
            {

                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@FromDate", FromDate);
                    dynamicParameters.Add("@ToDate", ToDate);
                    esmTonnageReports = sqlConnection.Query<BPCOrderReportDto>(SystemConstant.usp_BPCOrderEntryReport, dynamicParameters, commandType: CommandType.StoredProcedure);
                    sqlConnection.Close();
                    return esmTonnageReports.ToList();


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }



        }



        public int GetUserIDByName(string name)
        {
            int Output = 0;
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {

                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@UserName", name);

                    Output = sqlConnection.QueryFirstOrDefault<int>(SystemConstant.GetUserID, dynamicParameters, commandType: CommandType.StoredProcedure);
                    //Output = dynamicParameters.Get<int>("@Output");
                    sqlConnection.Close();

                }

                return Output;


            }
            catch (Exception e)
            {
                return Output;
            }

        }


        public List<TonnageReportDto> GetTonnageReportByCustomerAndProject(string fromDate, string toDate, string projectCode, string rptType)
        {

            IEnumerable<TonnageReportDto> esmTonnageReports;
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@FromDate", fromDate);
                    dynamicParameters.Add("@ToDate", toDate);
                    dynamicParameters.Add("@ProjectCode", projectCode);
                    dynamicParameters.Add("@rptType", rptType);
                    esmTonnageReports = sqlConnection.Query<TonnageReportDto>(SystemConstant.usp_TonnageReport_ByProjectID, dynamicParameters, commandType: CommandType.StoredProcedure);
                    sqlConnection.Close();
                    return esmTonnageReports.ToList();


                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<CustomerMaster?> GetCustomerAsync(string customerId)
        {
            return await _dbContext.CustomerMaster
                                   .FirstOrDefaultAsync(c => c.vchCustomerNo == customerId);
        }

        public async Task<ProjectMaster?> GetProjectName(string ProjectCode)
        {
            return await _dbContext.ProjectMaster
                                   .FirstOrDefaultAsync(c => c.vchprojectcode == ProjectCode);
        }

        public async Task<IEnumerable<dynamic>> GetOrderStatus(string FromDate, string ToDate,string? ProductType=null)
        {
            IEnumerable<dynamic> Orderlist; //new IEnumerable<List<ProjectMaster>>();

            using (var sqlConnection = new SqlConnection(STSconnectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@FromDate", FromDate);
                dynamicParameters.Add("@ToDate", ToDate);
                dynamicParameters.Add("@ProductType", ProductType);
                Orderlist = sqlConnection.Query<dynamic>(SystemConstant.Get_OrderStatus, dynamicParameters, commandType: CommandType.StoredProcedure,commandTimeout:60);
                sqlConnection.Close();
                return Orderlist;
            }
        }

        public async Task<IEnumerable<dynamic>> GetMissingGIOrders(string FromDate, string ToDate, string? ProductType=null)
        {
            IEnumerable<dynamic> Orderlist; //new IEnumerable<List<ProjectMaster>>();

            using (var sqlConnection = new SqlConnection(STSconnectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@FromDate", FromDate);
                dynamicParameters.Add("@ToDate", ToDate);
                dynamicParameters.Add("@ProductType", ProductType);
                Orderlist = sqlConnection.Query<dynamic>(SystemConstant.Get_Missing_GI, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return Orderlist;

            }
        }


        public async Task<IEnumerable<dynamic>> GetProductType(string FromDate, string ToDate)
        {
            IEnumerable<dynamic> productlist; //new IEnumerable<List<ProjectMaster>>();

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@FromDate", FromDate);
                dynamicParameters.Add("@ToDate", ToDate);

                productlist = sqlConnection.Query<dynamic>(SystemConstant.GetProductType, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return productlist;

            }
        }

    }
}
