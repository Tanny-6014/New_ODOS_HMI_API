
using Dapper;
using Microsoft.Data.SqlClient;
using OrderService.Constants;
using OrderService.Context;
using OrderService.Dtos;
using OrderService.Interfaces;
using System;
using System.Data;
using System.Net.Http;


namespace OrderService.Repositories
{
    public class OrderRepositories : IOrder
    {
        private OrderContext _dbContext;
        private readonly IConfiguration _configuration;
        private string connectionString;

        public OrderRepositories(OrderContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;

            connectionString = _configuration.GetConnectionString(SystemConstant.DefaultDBConnection);
        }



        public async Task<IEnumerable<ProjectListDto>> GetProjectAsync(string CustomerCode)
        {
            IEnumerable<ProjectListDto> Projlist; //new IEnumerable<List<ProjectMaster>>();

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@pCustomerCode", CustomerCode);

                Projlist = sqlConnection.Query<ProjectListDto>(SystemConstant.ddl_ProjectList, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return Projlist;

            }


        }

        public async Task<IEnumerable<CustomerListDto>> GetCustomerAsync(string UserName)
        {
            IEnumerable<CustomerListDto> Customerlist; //new IEnumerable<List<ProjectMaster>>();

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@UserName", UserName);

                Customerlist = sqlConnection.Query<CustomerListDto>(SystemConstant.ddl_CustomerList, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return Customerlist;

            }


        }

        public async Task<IEnumerable<ActiveGridListDto>> GetActiveGridList(string customerCode, string projectCode)
        {
            IEnumerable<ActiveGridListDto> Customerlist; //new IEnumerable<List<ProjectMaster>>();

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@pCustomerCode", customerCode);
                dynamicParameters.Add("@pProjectCode", projectCode);
                Customerlist = sqlConnection.Query<ActiveGridListDto>(SystemConstant.getActiveOrder_GridList, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return Customerlist;

            }


        }

        public async Task<IEnumerable<DeliveredGridListDto>> GetDeliveredGridList(string customerCode, string projectCode)
        {
            IEnumerable<DeliveredGridListDto> Customerlist; //new IEnumerable<List<ProjectMaster>>();

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@pCustomerCode", customerCode);
                dynamicParameters.Add("@pProjectCode", projectCode);
                Customerlist = sqlConnection.Query<DeliveredGridListDto>(SystemConstant.getDeliveredOrder_GridList, dynamicParameters, commandType: CommandType.StoredProcedure);
                sqlConnection.Close();
                return Customerlist;

            }


        }

        public async Task<bool> DeleteUpcomingOrder(string OrderNumber)
        {
            IEnumerable<UpcomingOrderDto> Customerlist; //new IEnumerable<List<ProjectMaster>>();

            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@pOrderNumber", OrderNumber);
                    Customerlist = sqlConnection.Query<UpcomingOrderDto>(SystemConstant.DeleteUpcomingOrder, dynamicParameters, commandType: CommandType.StoredProcedure);
                    sqlConnection.Close();
                    return true;

                }
            }
            catch (Exception ex)
            {
                return false;
                //throw ex;
                
            }


        }

        public async Task<bool> ConvertedUpcomingOrder(string pOrderNumber, string nOrderNumber,string LoginUser)
        {
            IEnumerable<UpcomingOrderDto> Customerlist; //new IEnumerable<List<ProjectMaster>>();

            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@pOrderNumber", pOrderNumber);
                    dynamicParameters.Add("@nOrderNumber", nOrderNumber);
                    dynamicParameters.Add("@nLoginUser", LoginUser);
                    Customerlist = sqlConnection.Query<UpcomingOrderDto>(SystemConstant.ConvertedUpcomingOrder, dynamicParameters, commandType: CommandType.StoredProcedure);
                    sqlConnection.Close();
                    return true;

                }
            }
            catch (Exception ex)
            {
                return false;
                //throw ex;

            }


        }

        public async Task<IEnumerable<UpcomingJobAdviceListDto>> LoadDataFor_UpcomingOrder(string customerCode, string projectCode)
        {
            IEnumerable<UpcomingJobAdviceListDto> Customerlist; //new IEnumerable<List<ProjectMaster>>();

            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@CustomerCode", customerCode);
                    dynamicParameters.Add("@ProjectCode", projectCode);
                    Customerlist = sqlConnection.Query<UpcomingJobAdviceListDto>(SystemConstant.LoadData_ForUpcomingOrder, dynamicParameters, commandType: CommandType.StoredProcedure);
                    sqlConnection.Close();
                    return Customerlist;

                }
            }
            catch (Exception ex)
            {
                //return false;
                throw ex;

            }


        }
        public async Task<IEnumerable<UpcomingJobAdviceListDto>> InsertIntoUpcomingOrderData(UpcomingJobAdviceListDto insertDataUpcomingOrdersDto)
        {
            IEnumerable<UpcomingJobAdviceListDto> Customerlist;
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@OrderNumber", insertDataUpcomingOrdersDto.OrderNumber);
                    dynamicParameters.Add("@OrderType", insertDataUpcomingOrdersDto.OrderType);
                    dynamicParameters.Add("@CustomerCode",insertDataUpcomingOrdersDto.CustomerCode);
                    dynamicParameters.Add("@ProjectCode", insertDataUpcomingOrdersDto.ProjectCode);
                    dynamicParameters.Add("@WBS1", insertDataUpcomingOrdersDto.WBS1);
                    dynamicParameters.Add("@WBS2", insertDataUpcomingOrdersDto.WBS2);
                    dynamicParameters.Add("@WBS3", insertDataUpcomingOrdersDto.WBS3);
                    dynamicParameters.Add("@StructureElement", insertDataUpcomingOrdersDto.StructureElement);
                    dynamicParameters.Add("@ProductType", insertDataUpcomingOrdersDto.ProductType);
                    dynamicParameters.Add("@ForecastDate", insertDataUpcomingOrdersDto.ForecastDate);
                    dynamicParameters.Add("@DeliveryDate", insertDataUpcomingOrdersDto.DeliveryDate);
                    dynamicParameters.Add("@LowerPONumber", insertDataUpcomingOrdersDto.LowerPONumber);
                    dynamicParameters.Add("@BBSNo", insertDataUpcomingOrdersDto.BBSNo);
                    dynamicParameters.Add("@BBSDesc", insertDataUpcomingOrdersDto.BBSDesc);
                    dynamicParameters.Add("@FloorTonnage", insertDataUpcomingOrdersDto.FloorTonnage);
                    dynamicParameters.Add("@ConvertedOrderNo", null);
                    dynamicParameters.Add("@OrderStatus", "Y");
                    dynamicParameters.Add("@NotifiedByEmail", null);
                    dynamicParameters.Add("@NotifiedEmailId", null);
                    dynamicParameters.Add("@NotifiedEmailDate", null);
             
                    Customerlist = sqlConnection.Query<UpcomingJobAdviceListDto>(SystemConstant.InsertData, dynamicParameters, commandType: CommandType.StoredProcedure);
                    sqlConnection.Close();
                    return Customerlist;

                }
            }
            catch (Exception ex)
            {
                //return false;
                throw ex;

            }


        }


        public async Task<bool> DeletedSubmittedUpcomingOrder(string pOrderNumber)
        {
            IEnumerable<UpcomingOrderDto> Customerlist; //new IEnumerable<List<ProjectMaster>>();

            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@COrderNumber", pOrderNumber);
                    Customerlist = sqlConnection.Query<UpcomingOrderDto>(SystemConstant.DeleteSubmittedUpcomingOrder, dynamicParameters, commandType: CommandType.StoredProcedure);
                    sqlConnection.Close();
                    return true;

                }
            }
            catch (Exception ex)
            {
                return false;
                //throw ex;

            }


        }


        public async Task<IEnumerable<OESUpcomingNotificationMailDto>> UpcomingNotificationMail(string pOrderNumber)
        {
            IEnumerable<OESUpcomingNotificationMailDto> Customerlist; //new IEnumerable<List<ProjectMaster>>();

            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@OrderNumber", pOrderNumber);
                    Customerlist = sqlConnection.Query<OESUpcomingNotificationMailDto>(SystemConstant.UpcomingNotification, dynamicParameters, commandType: CommandType.StoredProcedure);
                    sqlConnection.Close();
                    return Customerlist;

                }
            }
            catch (Exception ex)
            {
                //return false;
                throw ex;

            }


        }

        public async Task<bool> ResetUpcomingOrder(int pOrderNo)

        {

            IEnumerable<UpcomingOrderDto> Customerlist; //new IEnumerable<List<ProjectMaster>>();

            try

            {

                using (var sqlConnection = new SqlConnection(connectionString))

                {

                    sqlConnection.Open();

                    var dynamicParameters = new DynamicParameters();

                    dynamicParameters.Add("@pOrderNo", pOrderNo);

                    Customerlist = sqlConnection.Query<UpcomingOrderDto>(SystemConstant.ResetConvertedOrder, dynamicParameters, commandType: CommandType.StoredProcedure);

                    sqlConnection.Close();

                    return true;

                }

            }

            catch (Exception ex)

            {

                return false;

                //throw ex;

            }

        }


        public async Task<bool> GetUpdateIncomingData(string orderNumber)
        {
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    await sqlConnection.OpenAsync();

                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@ORDER_NUMBER", orderNumber);

                    var successFlag = await sqlConnection.QuerySingleAsync<int>(
                        SystemConstant.usp_UpdateIncomingData,
                        dynamicParameters,
                        commandType: CommandType.StoredProcedure);

                    return successFlag == 1;
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error updating incoming data: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> InserAutoReleaseData()
        {
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    await sqlConnection.OpenAsync();

                    var dynamicParameters = new DynamicParameters();

                    var result = await sqlConnection.QuerySingleAsync<InsertAutoReleaseDto>(
                            SystemConstant.usp_InsertAutoReleaseCutoverData,
                            dynamicParameters,
                            commandType: CommandType.StoredProcedure);

                    // You can now access:
                    bool isSuccess = result.SuccessFlag == 1;
                    int insertedCount = result.InsertedRows;

                    return isSuccess;
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error in inserting data: {ex.Message}");
                return false;
            }
        }

    }
}
