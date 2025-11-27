using Newtonsoft.Json.Linq;
using OrderService.Context;
using System.Net.Http;
using System.Net.Http.Headers;
using OrderService.Constants;
using OrderService.Models;
using OrderService.Context;
using System.Data.SqlClient;
using Dapper;
using Microsoft.EntityFrameworkCore;
using OrderService.Interfaces;
using System.Data.Entity;
using System.Text;
using Newtonsoft.Json;
using System.Data;
using System.Net;
using Oracle.ManagedDataAccess.Client;
using OrderService.Controllers;
using System.Globalization;

namespace OrderService.Repositories
{
    public class InterfaceRepository:IInterface
    {
        OracleTransaction oracleTransIDB;
        private static string _accessToken;
        private static string _refreshToken;
        private static DateTime _accessTokenIssuedAt;
        private static DateTime _refreshTokenIssuedAt;
        private readonly HttpClient client;
        private IDbContextFactory<OrderContext> _contextFactory;
        private readonly IConfiguration _configuration;
        private string connectionString;
        // private string strIDB_Connection = "Data Source=(DESCRIPTION = (ENABLE=BROKEN)(ADDRESS = (COMMUNITY = IDBPROD)(PROTOCOL = TCP)(HOST = nsprdscmlsnr.natsteel.corp)(PORT = 1521)) (CONNECT_DATA = (SERVER = DEDICATED) (SERVICE_NAME = IDBPROD)));Persist Security Info=True;User ID=SCM_ADMIN;Password=scm4admin; Connection Timeout=300";
        // private string connectionStringPROD= "Server=NSPRDDB19\\MSSQL2022;Database=ODOS;User ID=ndswebapps; Password=DBAdmin4*NDS; TrustServerCertificate=True;MultipleActiveResultSets=true";
         public InterfaceRepository(IDbContextFactory<OrderContext> contextFactory, IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _contextFactory = contextFactory;
            _configuration = configuration;

            connectionString = _configuration.GetConnectionString(SystemConstant.DefaultDBConnection);
            client = httpClientFactory.CreateClient();   ////////////////// initialize this in constructor
        }


        
        public static async Task LoginAsync()
        {
            using var client = new HttpClient();

            //var response = await client.PostAsJsonAsync("http://10.72.181.2/api/auth/login", new
            var response = await client.PostAsJsonAsync("http://172.20.0.5/api/auth/login", new
            {
                username = "Snehal.Kulkarni@tatatechnologies.com",
                password = "Snehal.Kulkarni",
                scanLogin = false
            });

            if (!response.IsSuccessStatusCode)
                throw new Exception("Login failed: " + await response.Content.ReadAsStringAsync());

            var result = await response.Content.ReadFromJsonAsync<TokenResponse>();

            _accessToken = result.token;
            _refreshToken = result.renewToken;
            _accessTokenIssuedAt = DateTime.UtcNow;
            _refreshTokenIssuedAt = DateTime.UtcNow;
        }

        public static async Task RenewTokenAsync()
        {
            using var client = new HttpClient();

            //var response = await client.PostAsJsonAsync("http://10.72.181.2/api/auth/renew-token", new
            var response = await client.PostAsJsonAsync("http://172.20.0.5/api/auth/renew-token", new
            {
                username = "Snehal.Kulkarni@tatatechnologies.com",
                renewToken = _refreshToken
            });

            if (!response.IsSuccessStatusCode)
            {
                _accessToken = null;
                _refreshToken = null;
                throw new Exception("Token renewal failed: " + await response.Content.ReadAsStringAsync());
            }

            var result = await response.Content.ReadFromJsonAsync<TokenResponse>();

            _accessToken = result.token;
            _refreshToken = result.renewToken;
            _accessTokenIssuedAt = DateTime.UtcNow;
            _refreshTokenIssuedAt = DateTime.UtcNow;
        }

        public static async Task<string> GetAccessToken()
        {
            if (string.IsNullOrEmpty(_accessToken) || (DateTime.UtcNow - _accessTokenIssuedAt).TotalMinutes > 25)
            {
                if (!string.IsNullOrEmpty(_refreshToken) && (DateTime.UtcNow - _refreshTokenIssuedAt).TotalDays < 3)
                {
                    await RenewTokenAsync();
                }
                else
                {
                    await LoginAsync();
                }
            }

            return _accessToken;
        }


        public async Task<string> GetIDFromCodename(bool hasExtendedInfo, string codename)
        {

            //var token = await GetAccessToken();

            var client = new HttpClient();
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            client.DefaultRequestHeaders.Add("x-api-key", "6B29FC40-CA47-1067-B31D-00DD010662DA");
            //var url = $"http://10.72.181.2/api/public/run-order?has-extended-info={hasExtendedInfo.ToString().ToLower()}&code-name={Uri.EscapeDataString(codename ?? string.Empty)}";
            var url = $"http://172.20.0.5/api/public/run-order?has-extended-info={hasExtendedInfo.ToString().ToLower()}&code-name={Uri.EscapeDataString(codename ?? string.Empty)}";

            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {

                throw new Exception($"API call failed: {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync();

            var res = JArray.Parse(content);
            if (res == null || !res.Any())
            {
                return "0";
            }

            var id = res.First?["id"]?.ToString();

            return string.IsNullOrEmpty(id) ? "0" : id;
        }


        //public async Task<(bool IsSuccess, string Message)> Delete_OrderAssignmentData(string orderRequestNo,string WithdrawBy)
        public async Task<(bool IsSuccess, string Message)> Delete_OrderAssignmentData(string orderRequestNo)
        {
                try
                {
                    using var connection = new SqlConnection(connectionString);
                    await connection.OpenAsync();

                    var headerQuery = @"SELECT order_no
                             FROM SCM_Order_Header
                             where AssignedTo='SCHNELL'
                             and SchnellStatus='Y' and order_request_no =@orderRequestNo";

                    var headers = (await connection.QueryAsync<dynamic>(headerQuery, new { orderRequestNo })).ToList();

                    string Id = await GetIDFromCodename(false, (string)headers.First().order_no);

                    if (headers == null || headers.Count == 0)
                    {
                        throw new Exception("No matching order found.");
                    }
                    else
                    {
                        if (Id == "0")
                        {
                            throw new Exception("No order found.");
                        }

                    }

                    //var token = await GetAccessToken();
                    using var client = new HttpClient();
                    client.DefaultRequestHeaders.Add("x-api-key", "6B29FC40-CA47-1067-B31D-00DD010662DA");
                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                //string url = $"http://10.72.181.2/api/public/run-order/{Id}";
                string url = $"http://172.20.0.5/api/public/run-order/{Id}";

                var response = await client.DeleteAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    await using var context = _contextFactory.CreateDbContext();
                
                        var orderRequestNoParam = new Microsoft.Data.SqlClient.SqlParameter("@OrderRequestNo", orderRequestNo);
                        //var WithdrawbyParam = new Microsoft.Data.SqlClient.SqlParameter("@WithdrawBy", WithdrawBy);
                        var outputParam = new Microsoft.Data.SqlClient.SqlParameter
                        {
                            ParameterName = "@Output",
                            SqlDbType = System.Data.SqlDbType.Int,
                            Direction = System.Data.ParameterDirection.Output
                        };

                        await context.Database.ExecuteSqlRawAsync(
                            //"EXEC [dbo].[Save_OrderWithdraw] @OrderRequestNo,@WithdrawBy, @Output OUTPUT",
                            //orderRequestNoParam, WithdrawbyParam, outputParam
                            "EXEC [dbo].[Save_OrderWithdraw] @OrderRequestNo, @Output OUTPUT",
                            orderRequestNoParam, outputParam
                        );

                        int result = (int)outputParam.Value;

                        await context.Database.ExecuteSqlRawAsync(
                                            @"UPDATE scm_order_header
                                           SET SchnellStatus = 'D',SchnellLastModifiedDate=GETDATE()
                                           WHERE  order_request_no = {0}", orderRequestNo);
                }
                else
                {
                    string error = await response.Content.ReadAsStringAsync();
                    return (false, $"Failed to delete order: {error}");
                }
                return (true, "Deletion Completed.");
                }
                catch (Exception ex)
                {
                    return (false, $"Exception occurred: {ex.Message}");
                }
            }

        public async Task<(bool IsSuccess, string Message)> Delete_ODOS_Scnhell_Data(string orderNo)
        {
            try
            {
                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();

                var headerQuery = @"SELECT order_no
                             FROM SCM_Order_Header
                             where AssignedTo='SCHNELL'
                             and SchnellStatus='Y' and order_no =@orderNo";

                var headers = (await connection.QueryAsync<dynamic>(headerQuery, new { orderNo })).ToList();

                //string Id = await GetIDFromCodename(false, (string)headers.First().order_no);

                string Id;

                if (headers == null || headers.Count == 0)
                {
                    //throw new Exception("No matching order found.");
                    return (false, "No matching order found.");
                }
                else
                {
                    Id = await GetIDFromCodename(false, (string)headers.First().order_no);
                    if (Id == "0")
                    {
                        //throw new Exception("No order found.");
                        return (false, "No order found.");
                    }
                }

                //var token = await GetAccessToken();
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("x-api-key", "6B29FC40-CA47-1067-B31D-00DD010662DA");
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                //string url = $"http://10.72.181.2/api/public/run-order/{Id}";
                string url = $"http://172.20.0.5/api/public/run-order/{Id}";

                var response = await client.DeleteAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    await using var context = _contextFactory.CreateDbContext();
                    await context.Database.ExecuteSqlRawAsync(
                                @"UPDATE scm_order_header
                               SET SchnellStatus = 'D',SchnellLastModifiedDate=GETDATE(),AssignedTo='CANCEL',WithdrawDate =  CONVERT(varchar(23), GETDATE(), 121)
                               WHERE  order_no = {0}", orderNo);
                }
                else
                {
                    string error = await response.Content.ReadAsStringAsync();
                    return (false, $"Failed to delete order: {error}");
                }


                return (true, "Deletion Completed.");
            }
            catch (Exception ex)
            {
                return (false, $"Exception occurred: {ex.Message}");
            }
        }

        public async Task<string> GetCodename(int id, bool hasExtendedInfo)
        {
            //var token = await GetAccessToken();
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("x-api-key", "6B29FC40-CA47-1067-B31D-00DD010662DA");
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //var url = $"http://10.72.181.2/api/public/run-order/{id}?has-extended-info={hasExtendedInfo.ToString().ToLower()}";
            var url = $"http://172.20.0.5/api/public/run-order/{id}?has-extended-info={hasExtendedInfo.ToString().ToLower()}";
            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {

                throw new Exception($"API call failed: {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync();


            var jObject = Newtonsoft.Json.Linq.JObject.Parse(content);
            var codename = jObject["codename"]?.ToString();

            return codename;
        }

        private DateTime? TryParseDate(object value)
        {
            if (value == null) return null;

            var s = value.ToString().Trim();
            if (string.IsNullOrEmpty(s) || s == "00000000")
                return null;

            if (DateTime.TryParse(s, out var result))
                return result;

            return null;
        }
        private int TryParseInt(object value)
        {

            if (value == null || value == DBNull.Value)
                return 0;

            if (decimal.TryParse(value.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal decResult))
                return (int)Math.Round(decResult);

            return 0;

        }

        private static float TryParseFloat(object input)
        {
            return float.TryParse(Convert.ToString(input), out var result) ? result : 0;
        }

        private ExtendedHeader MapHeaderToExtended(dynamic h)
        {
            return new ExtendedHeader
            {
                OrderNo = h.order_no?.Trim(),
                CustCode = h.cust_code?.Trim(),
                CustName = h.cust_name?.Trim(),
                ContractNo = h.contract_no?.Trim(),
                ProjNo = h.proj_no?.Trim(),
                ProjName = h.proj_name?.Trim(),
                CustPoNo = h.cust_po_no?.Trim(),
                CustOrderDate = ConvertDateFormat(h.cust_order_date),
                ReqDelDateOriginal = ConvertDateFormat(h.req_del_date_original),
                ReqDelDateRevised = ConvertDateFormat(h.req_del_date_revised),
                ActualDelDate = string.IsNullOrWhiteSpace(h.actual_del_date) ? null : h.actual_del_date.Trim(),
                OrderType = h.order_type?.Trim(),
                CustClass = h.cust_class?.Trim(),
                ProjSegment = h.proj_segment?.Trim(),
                ProjSubSegment = h.proj_sub_segment?.Trim(),
                JobSiteLocation = h.job_site_location?.Trim(),
                ForecastOrderNo = h.forecast_order_no?.Trim(),
                AccountMgr = h.account_mgr?.Trim(),
                SegmentMgr = h.segment_mgr?.Trim(),
                ProjCoordinator = h.proj_coordinator?.Trim(),
                ProductType = h.product_type?.Trim(),
                VehicleType = h.vehicle_type?.Trim(),
                OrderPieces = TryParseInt(h.order_pieces),
                DeliveredPieces = TryParseInt(h.delivered_pieces),
                CreditStatus = h.credit_status?.Trim(),
                ApprovalStatus = h.approval_status?.Trim(),
                DeliveryStatus = h.delivery_status?.Trim(),
                CancellationStatus = h.cancellation_status?.Trim(),
                MatSourceInd = h.mat_source_ind?.Trim(),
                UrgentOrderInd = h.urgent_order_in?.Trim(),
                AllowPartialDeliveryInd = h.allow_partial_delivery_ind?.Trim(),
                ForecastOrderInd = h.forecast_order_ind?.Trim(),
                LocalOrderInd = h.local_order_ind?.Trim(),
                MtoInd = h.mto_ind?.Trim(),
                SelfCollectInd = h.self_collect_ind?.Trim(),
                PremiumServiceInd = h.premium_service_ind?.Trim(),
                CraneBookedInd = h.crane_booked_ind?.Trim(),
                BargeBookedInd = h.barge_booked_ind?.Trim(),
                PoliceEscortInd = h.police_escort_ind?.Trim(),
                CompanyCode = h.company_code?.Trim(),
                NoOfItems = TryParseInt(h.no_of_items),
                IntRemark = h.int_remark?.Trim(),
                ExtRemark = h.ext_remark?.Trim(),
                Remarks = h.remarks?.Trim(),
                OrderGroupId = h.order_group_id?.Trim(),
                OnHoldInd = false,
                FirstPromisedDate = DateTime.UtcNow,
                ContractualLeadtimeInd = false,
                TimeStampInsert = h.time_stamp,
                NeedsLorryCraneInd = false,
                ZeroToleranceInd = false,
                CallBefDelInd = false,
                ConquasInd = false,
                SpecialPassInd = false,
                DoNotMixInd = false,
                GroupMemberCount = TryParseInt(h.group_member_count),
                LowBedVehicleAllowed = h.low_bed_vehicle_allowed?.Trim(),
                FiftyTonVehicleAllowed = h.fiftyton_vehicle_allowed?.Trim(),
                PhysicalProjNo = h.physical_proj_no?.Trim(),
                PhysicalDeliveryProjNo = h.physical_delivery_proj_no?.Trim(),
                Wbs1 = "",
                Wbs2 = "",
                Wbs3 = "",
                Bbs = "",
                StElement = "",
                VariousBar = false,
                Spare1 = "",
                Spare2 = "",
                Spare3 = "",
                Spare4 = "",
                Spare5 = ""
            };

        }

        private ExtendedItem MapItemToExtended(dynamic i)
        {
            return new ExtendedItem
            {
                Cliente = "",
                OrderNo = i.order_no?.Trim(),
                ItemNo = i.item_no?.Trim(),
                ParentItemNo = i.parent_item_no?.Trim(),
                SapOrderItem = i.sap_order_item?.Trim(),
                MaterialNo = i.material_no?.Trim(),
                ProductHierarchy = i.product_hierarchy?.Trim(),
                OrderQty = (int)i.order_pieces,
                SalesUom = i.sales_uom?.Trim(),
                FgProductionWtKg = (float)i.fg_production_wt_kg,
                ComponentProductionWtKg = (float)i.component_production_wt_kg,
                ProductMarking = i.product_marking?.Trim()?.Length > 10 ? i.product_marking.Trim().Substring(0, 10) : i.product_marking?.Trim(),
                ComponentPrefix = i.component_prefix?.Trim(),
                OrderPieces = i.order_pieces,
                DeliveredPieces = i.delivered_pieces,
                ActualDelDate = TryParseDate(i.actual_del_date),
                DeliveryStatus = i.delivery_status?.Trim(),
                CancellationStatus = i.cancellation_status?.Trim(),
                EnvLength = TryParseInt(i.env_length),
                EnvWidth = TryParseInt(i.env_width),
                EnvHeight = TryParseInt(i.env_height),
                Wbs1 = i.wbs1?.Trim(),
                Wbs2 = i.wbs2?.Trim(),
                Wbs3 = i.wbs3?.Trim(),
                Wbs4 = i.wbs4?.Trim(),
                Wbs5 = i.wbs5?.Trim(),
                ProjStage = i.proj_stage?.Trim(),
                BbsNo = i.bbs_no?.Trim(),
                BbsDesc = i.bbs_desc?.Trim(),
                NeedsGalvanizingInd = i.needs_galvanizing_ind?.Trim(),
                NeedsProductionInd = i.needs_production_ind?.Trim(),
                CabSpiralPitch = TryParseInt(i.cab_spiral_pitch),
                CabFleetPitch = TryParseInt(i.cab_fleet_pitch),
                CabBarGrade = i.cab_bar_grade?.Trim(),
                CabDia = (int)i.cab_dia,
                CabBvbs = i.cab_bvbs?.Trim(),
                CabCutLength = TryParseInt(i.cab_cut_length),
                CabCouplerEndNo = TryParseInt(i.cab_coupler_end_no),
                CabFormerSize = TryParseInt(i.cab_former_size),
                CabShapeGroup = i.cab_shape_group?.Trim(),
                ShapeCode = i.shape_code?.Trim(),
                CabCouEndType1 = i.cab_cou_end_type1?.Trim(),
                CabCouEndType2 = i.cab_cou_end_type2?.Trim(),
                CabNoOfBend = TryParseInt(i.cab_no_of_bend),
                Remarks = i.remarks?.Trim(),
                StElement = i.st_element?.Trim(),
                QtyInBaseUom = TryParseInt(i.qty_in_base_uom),
                BaseUom = i.base_uom?.Trim(),
                DeliveryQty = 0,
                CabMainBarAngle = TryParseInt(i.cab_main_bar_angle),
                InvLenInMm = TryParseInt(i.inv_len_in_mm),
                GradeDia = i.grade_dia?.Trim(),
                SapProductGroup = i.sap_product_group?.Trim(),
                CabStandardEndNo = TryParseInt(i.cab_standard_end_no),
                CabFormerType = TryParseInt(i.cab_former_type),
                ProductType = i.product_type?.Trim(),
                CabExtendedEndNo = TryParseInt(i.cab_extended_end_no),
                CabStudEndNo = TryParseInt(i.cab_stud_end_no),
                BbsPageNo = TryParseInt(i.bbs_page_no),
                IdenticalHashcode = TryParseInt(i.identical_hashcode),
                SimilarMainbarHashcode = TryParseInt(i.similar_mainbar_hashcode),
                SimilarExtlinkHashcode = TryParseInt(i.similar_extlink_hashcode),
                BendingHr = "",
                Spare1 = "",
                Spare2 = "",
                Spare3 = "",
                Spare4 = "",
                Spare5 = ""
            };
        }

        public async Task<(bool IsSuccess, string Message)> Update_OrderAssignmentData(string OrderRequestNo)
        {
            try
            {
                //string result = await GetCodename(id, false);
                using var connection = new SqlConnection(connectionString);
                //await connection.OpenAsync();
                //var headerQuery = @"
                //SELECT 
                //  h.order_request_no, h.order_no, h.cust_code, h.cust_name, h.contract_no, h.proj_no, 
                //  h.proj_name, h.cust_po_no, h.cust_order_date, h.req_del_date_fr AS req_del_date_original,
                //  h.req_del_date_to AS req_del_date_revised, h.actual_del_date, h.order_type,
                //  h.cust_class, h.proj_segment, h.proj_sub_segment, h.job_site_location, h.forecast_order_no,
                //  h.account_mgr, h.segment_mgr, h.proj_coordinator, h.product_type, h.vehicle_type,
                //  h.order_pieces, h.delivered_pieces, h.credit_status, h.approval_status, h.delivery_status,
                //  h.cancellation_status, h.mat_source_ind, h.urgent_order_ind, h.allow_partial_delivery_ind,
                //  h.forecast_order_ind, h.local_order_ind, h.mto_ind, h.self_collect_ind, h.premium_service_ind,
                //  h.crane_booked_ind, h.barge_booked_ind, h.police_escort_ind, h.company_code, h.no_of_items,
                //  h.int_remark, h.ext_remark, h.remarks, h.order_group_id, h.group_member_count,
                //  h.low_bed_vehicle_allowed, h.fiftyton_vehicle_allowed, h.physical_proj_no,
                //  h.physical_delivery_proj_no
                //FROM SCM_Order_Header h
                //WHERE h.AssignedTo = 'SCHNELL'and h.order_no=@orderNo";

                //var headers = await connection.QueryAsync<dynamic>(headerQuery, new { orderNo = orderNo });

                var headers = await connection.QueryAsync<dynamic>(
                    SystemConstant.GetOrderUpdateddata_IDB,
                    new { OrderRequestNo = OrderRequestNo },
                    commandType: CommandType.StoredProcedure
                );

                //var token = await GetAccessToken();
                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("x-api-key", "6B29FC40-CA47-1067-B31D-00DD010662DA");
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                foreach (var h in headers)
                {
                    // var orderNo = (string)h.order_no;
                    var orderRequestNo = (string)h.order_request_no;

                    var itemQuery = @"SELECT oi.*
                FROM SCM_Order_Item oi
                CROSS APPLY (
                    SELECT MAX(TRY_CAST(
                        REPLACE(value, 'l', '') AS INT
                    )) AS MaxSegLength
                    FROM STRING_SPLIT(oi.CAB_BVBS, '@')
                    WHERE value LIKE 'l%'
                ) seg
                WHERE oi.CAB_DIA > 16
                    AND seg.MaxSegLength > 1800
                    AND oi.order_request_no = @orderRequestNo 
                ORDER BY oi.item_no;";
                    var items = await connection.QueryAsync<dynamic>(itemQuery, new { orderRequestNo });

                    var order = new OrderAssignment
                    {
                        Codename = h.order_no,
                        Description = "",
                        ShippingAt = DateTime.Now,
                        Position = 0,
                        Extended = MapHeaderToExtended(h),
                        //Items = items.Select(i => new Item
                        //{

                        //    Tag = i.TagNo,
                        //    Part = "",
                        //    Collection = "",
                        //    AggregationCode = 0,
                        //    BvbsString = i.cab_bvbs,
                        //    Replaced = false,
                        //    ExtraInfo = "",
                        //    Position = 0,
                        //    Diameter = (int)i.cab_dia,
                        //    Quantity = (int)i.order_pieces,
                        //    Length = i.cab_cut_length,
                        //    MachineId = null,
                        //    BundleTypeId = 1,
                        //    LoadNo = "",
                        //    LayerNo = 0,
                        //    Extended = MapItemToExtended(i)
                        //}).ToList()
                    };


                    string id = await GetIDFromCodename(false, h.order_no);

                    var jsonBody = JsonConvert.SerializeObject(order);
                    var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                    // File.WriteAllText(@"C:\Logs\order-payload.json", jsonBody);

                    //string url = $"http://10.72.181.2/api/public/run-order/{id}";
                    string url = $"http://172.20.0.5/api/public/run-order/{id}";

                    var response = await client.PutAsJsonAsync(url, order);


                    if (!response.IsSuccessStatusCode)
                    {
                        string error = await response.Content.ReadAsStringAsync();
                       // File.WriteAllText(@"C:\Logs\order-error.json", error);
                        return (false, $"Failed to post order: {error}");
                    }

                    return (true, "Order updated successfully.");
                }

                return (false, "No orders processed.");
            }
            catch (Exception ex)
            {
                return (false, $"Exception occurred: {ex.Message}");
            }
        }

        public async Task<(bool IsSuccess, string Message)> Update_OrderAssignmentData(string orderNo,int LayerNo,string LoadNo)
        {
            try
            {
                //string result = await GetCodename(id, false);
                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();
                var headerQuery = @"
                SELECT 
                  h.order_request_no, h.order_no, h.cust_code, h.cust_name, h.contract_no, h.proj_no, 
                  h.proj_name, h.cust_po_no, h.cust_order_date, h.req_del_date_fr AS req_del_date_original,
                  h.req_del_date_to AS req_del_date_revised, h.actual_del_date, h.order_type,
                  h.cust_class, h.proj_segment, h.proj_sub_segment, h.job_site_location, h.forecast_order_no,
                  h.account_mgr, h.segment_mgr, h.proj_coordinator, h.product_type, h.vehicle_type,
                  h.order_pieces, h.delivered_pieces, h.credit_status, h.approval_status, h.delivery_status,
                  h.cancellation_status, h.mat_source_ind, h.urgent_order_ind, h.allow_partial_delivery_ind,
                  h.forecast_order_ind, h.local_order_ind, h.mto_ind, h.self_collect_ind, h.premium_service_ind,
                  h.crane_booked_ind, h.barge_booked_ind, h.police_escort_ind, h.company_code, h.no_of_items,
                  h.int_remark, h.ext_remark, h.remarks, h.order_group_id, h.group_member_count,
                  h.low_bed_vehicle_allowed, h.fiftyton_vehicle_allowed, h.physical_proj_no,
                  h.physical_delivery_proj_no
                FROM SCM_Order_Header h
                WHERE h.AssignedTo = 'SCHNELL'and h.order_no=@orderNo";

                var headers = await connection.QueryAsync<dynamic>(headerQuery, new { orderNo = orderNo });

                //var token = await GetAccessToken();
                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("x-api-key", "6B29FC40-CA47-1067-B31D-00DD010662DA");
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                foreach (var h in headers)
                {
                    // var orderNo = (string)h.order_no;
                    var orderRequestNo = (string)h.order_request_no;

                    //var itemQuery = @"SELECT * FROM SCM_Order_Item WHERE  CAB_DIA>=20 and order_request_no = @orderRequestNo ORDER BY item_no";

                    var itemQuery = @"SELECT oi.*
                FROM SCM_Order_Item oi
                CROSS APPLY (
                    SELECT MAX(TRY_CAST(
                        REPLACE(value, 'l', '') AS INT
                    )) AS MaxSegLength
                    FROM STRING_SPLIT(oi.CAB_BVBS, '@')
                    WHERE value LIKE 'l%'
                ) seg
                WHERE oi.CAB_DIA > 16
                    AND seg.MaxSegLength > 1800
                    AND oi.order_request_no = @orderRequestNo 
                ORDER BY oi.item_no;";

                    var items = await connection.QueryAsync<dynamic>(itemQuery, new { orderRequestNo });

                    var order = new OrderAssignment
                    {
                        Codename = orderNo,
                        Description = "",
                        ShippingAt = DateTime.Now,
                        Position = 0,
                        Paused = false,
                        Completed = false,
                        Extended = MapHeaderToExtended(h),
                        Items = items.Select(i => new Item
                        {

                            Tag = h.order_no + i.item_no.Trim(),
                            Part = "",
                            Collection = "",
                            AggregationCode = 0,
                            BvbsString = i.cab_bvbs,
                            Replaced = false,
                            ExtraInfo = "",
                            Position = 0,
                            Diameter = (int)i.cab_dia,
                            Quantity = (int)i.order_qty,
                            Length = i.cab_cut_length,
                            MachineId = null,
                            BundleTypeId = 1,
                            LoadNo = LoadNo,
                            LayerNo = LayerNo,
                            Extended = MapItemToExtended(i)
                        }).ToList()
                    };


                    string id = await GetIDFromCodename(false, orderNo);

                    var jsonBody = JsonConvert.SerializeObject(order);
                    var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                    File.WriteAllText(@"C:\Logs\order-payload.json", jsonBody);

                    //string url = $"http://10.72.181.2/api/public/run-order/{id}";
                    string url = $"http://172.20.0.5/api/public/run-order/{id}";

                    var response = await client.PutAsJsonAsync(url, order);


                    if (!response.IsSuccessStatusCode)
                    {
                        string error = await response.Content.ReadAsStringAsync();
                        File.WriteAllText(@"C:\Logs\order-error.json", error);
                        return (false, $"Failed to post order: {error}");
                    }

                    return (true, "Order updated successfully.");
                }

                return (false, "No orders processed.");
            }
            catch (Exception ex)
            {
                return (false, $"Exception occurred: {ex.Message}");
            }
        }

        public static string ConvertDateFormat(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return null;


            var parts = input.Split('.', 2);

            if (parts.Length == 2)
            {
                var timePart = parts[1].Replace('.', ':');
                return $"{parts[0]}.{timePart}";
            }


            return input;
        }

        public async Task<(bool IsSuccess, string Message)> Update_OrderAssignmentData_Schedule()
        {
            try
            {
                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();
                var lProcessObj = new ProcessController();
                OracleConnection cnIDB = new OracleConnection();
                lProcessObj.OpenIDBConnection(ref cnIDB); 

                using var cmdIDB = cnIDB.CreateCommand();
                cmdIDB.CommandTimeout = 15000;


                List<string> orderRows = new List<string>();
                
                string orderQuery = @"
                SELECT ORDER_NO
                FROM (
                SELECT DISTINCT ORDER_NO
                FROM SCM_ADMIN.tbl_order_item_load_schnell
                WHERE (STATUS = 'A' OR STATUS = 'E')
                ) t
                WHERE ROWNUM <= 50";

                cmdIDB.CommandText = orderQuery;

               
                using (var reader = await cmdIDB.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        orderRows.Add(reader["ORDER_NO"].ToString());
                    }
                }

                if (orderRows.Count == 0)
                {
                    throw new Exception("No orders present in IDB.");
                }

                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("x-api-key", "6B29FC40-CA47-1067-B31D-00DD010662DA");


                foreach (var orderNo in orderRows)
                {

                    try
                    {
                        cmdIDB.CommandText = @"
                        SELECT ITEM_NO, LOAD_NO, LAYER_NO, CONFIRMED_DATE_STRING, STATUS
                        FROM SCM_ADMIN.tbl_order_item_load_schnell
                        WHERE (STATUS ='A' OR STATUS = 'E')
                          AND ORDER_NO = :orderNo";

                        // clear old params first
                        cmdIDB.Parameters.Clear();
                        cmdIDB.Parameters.Add(new OracleParameter("orderNo", orderNo));

                        List<IDB_LoadItem> items = new List<IDB_LoadItem>();
                        using (var reader = await cmdIDB.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                items.Add(new IDB_LoadItem
                                {
                                    ITEM_NO = reader["ITEM_NO"].ToString(),
                                    LOAD_NO = reader["LOAD_NO"].ToString(),
                                    LAYER_NO = reader["LAYER_NO"].ToString(),
                                    CONFIRMED_DATE_STRING = reader["CONFIRMED_DATE_STRING"].ToString()
                                });
                            }
                        }

                        if (items.Count == 0)
                        {
                            continue;
                        }
                        // check if header exists 
                        var headerQuery = @"
                       SELECT h.order_request_no, h.order_no
                       FROM SCM_Order_Header h
                       WHERE h.AssignedTo = 'SCHNELL'
                       AND h.SchnellStatus = 'Y'
                       AND h.order_no = @orderNo";

                        var headers = (await connection.QueryAsync<dynamic>(headerQuery, new { orderNo })).ToList();

                        //var orderRequestNo = (string)headers.First().order_request_no;

                        string id = await GetIDFromCodename(false, orderNo);
                        if (headers.Count > 0 && id != "0")
                        {

                            //var client = new HttpClient();


                            //client.DefaultRequestHeaders.Add("x-api-key", "6B29FC40-CA47-1067-B31D-00DD010662DA");
                            //string fetchOrderDetails = $"http://10.72.181.2/api/public/run-order/{id}?has-extended-info=true";
                            string fetchOrderDetails = $"http://172.20.0.5/api/public/run-order/{id}?has-extended-info=true";


                            var getResponse = await client.GetStringAsync(fetchOrderDetails);

                            var apiResponse = JsonConvert.DeserializeObject<OrderAssignment_Get>(getResponse);
                            var jsonFormatted = JsonConvert.SerializeObject(apiResponse, Formatting.Indented);
                            File.WriteAllText(@"C:\Logs\api-response-parsed.json", jsonFormatted);

                            var mappedOrder = new OrderAssignment_Update
                            {
                                Codename = apiResponse.Codename,
                                Description = apiResponse.Description,
                                ShippingAt = items
                                    .Select(d =>
                                        string.IsNullOrWhiteSpace(d.CONFIRMED_DATE_STRING)
                                            ? (DateTime?)null
                                            : DateTime.SpecifyKind(
                                                DateTime.ParseExact(
                                                    d.CONFIRMED_DATE_STRING,
                                                    "yyyy-MM-dd.HH.mm.ss",
                                                    CultureInfo.InvariantCulture
                                                ),
                                                DateTimeKind.Utc
                                            )
                                    )
                                    .FirstOrDefault(),
                                Position = apiResponse.Position,
                                Paused = apiResponse.Paused,
                                Completed = apiResponse.Completed,
                                Extended = new ExtendedHeader_U
                                {
                                    OrderNo = apiResponse.Extended.OrderNo,
                                    CustCode = apiResponse.Extended.CustCode,
                                    CustName = apiResponse.Extended.CustName,
                                    ContractNo = apiResponse.Extended.ContractNo,
                                    ProjNo = apiResponse.Extended.ProjNo,
                                    ProjName = apiResponse.Extended.ProjName,
                                    CustPoNo = apiResponse.Extended.CustPoNo,
                                    CustOrderDate = apiResponse.Extended.CustOrderDate,
                                    ReqDelDateOriginal = apiResponse.Extended.ReqDelDateOriginal,
                                    ReqDelDateRevised = apiResponse.Extended.ReqDelDateRevised,
                                    PromisedDelDate = items
                                    .Select(d =>
                                        string.IsNullOrWhiteSpace(d.CONFIRMED_DATE_STRING)
                                            ? (DateTime?)null
                                            : DateTime.SpecifyKind(
                                                DateTime.ParseExact(
                                                    d.CONFIRMED_DATE_STRING,
                                                    "yyyy-MM-dd.HH.mm.ss",
                                                    CultureInfo.InvariantCulture
                                                ),
                                                DateTimeKind.Utc
                                            )
                                    )
                                    .FirstOrDefault(),
                                    ActualDelDate = apiResponse.Extended.ActualDelDate,
                                    OrderType = apiResponse.Extended.OrderType,
                                    CustClass = apiResponse.Extended.CustClass,
                                    ProjSegment = apiResponse.Extended.ProjSegment,
                                    ProjSubSegment = apiResponse.Extended.ProjSubSegment,
                                    JobSiteLocation = apiResponse.Extended.JobSiteLocation,
                                    ForecastOrderNo = apiResponse.Extended.ForecastOrderNo,
                                    AccountMgr = apiResponse.Extended.AccountMgr,
                                    SegmentMgr = apiResponse.Extended.SegmentMgr,
                                    ProjCoordinator = apiResponse.Extended.ProjCoordinator,
                                    ProductType = apiResponse.Extended.ProductType,
                                    VehicleType = apiResponse.Extended.VehicleType,
                                    OrderPieces = apiResponse.Extended.OrderPieces,
                                    DeliveredPieces = apiResponse.Extended.DeliveredPieces,
                                    CreditStatus = apiResponse.Extended.CreditStatus,
                                    ApprovalStatus = apiResponse.Extended.ApprovalStatus,
                                    DeliveryStatus = apiResponse.Extended.DeliveryStatus,
                                    CancellationStatus = apiResponse.Extended.CancellationStatus,
                                    MatSourceInd = apiResponse.Extended.MatSourceInd,
                                    UrgentOrderInd = apiResponse.Extended.UrgentOrderInd,
                                    AllowPartialDeliveryInd = apiResponse.Extended.AllowPartialDeliveryInd,
                                    ForecastOrderInd = apiResponse.Extended.ForecastOrderInd,
                                    LocalOrderInd = apiResponse.Extended.LocalOrderInd,
                                    MtoInd = apiResponse.Extended.MtoInd,
                                    SelfCollectInd = apiResponse.Extended.SelfCollectInd,
                                    PremiumServiceInd = apiResponse.Extended.PremiumServiceInd,
                                    CraneBookedInd = apiResponse.Extended.CraneBookedInd,
                                    BargeBookedInd = apiResponse.Extended.BargeBookedInd,
                                    PoliceEscortInd = apiResponse.Extended.PoliceEscortInd,
                                    CompanyCode = apiResponse.Extended.CompanyCode,
                                    NoOfItems = apiResponse.Extended.NoOfItems,
                                    IntRemark = apiResponse.Extended.IntRemark,
                                    ExtRemark = apiResponse.Extended.ExtRemark,
                                    Remarks = apiResponse.Extended.Remarks,
                                    OrderGroupId = apiResponse.Extended.OrderGroupId,
                                    OnHoldInd = apiResponse.Extended.OnHoldInd,
                                    FirstPromisedDate = apiResponse.Extended.FirstPromisedDate,
                                    ContractualLeadtimeInd = apiResponse.Extended.ContractualLeadtimeInd,
                                    TimeStampUpdate = apiResponse.Items.FirstOrDefault()?.Extended.TimeStampUpdate,
                                    TimeStampInsert = apiResponse.Items.FirstOrDefault()?.Extended.TimeStampInsert,
                                    NeedsLorryCraneInd = apiResponse.Extended.NeedsLorryCraneInd,
                                    ZeroToleranceInd = apiResponse.Extended.ZeroToleranceInd,
                                    CallBefDelInd = apiResponse.Extended.CallBefDelInd,
                                    ConquasInd = apiResponse.Extended.ConquasInd,
                                    SpecialPassInd = apiResponse.Extended.SpecialPassInd,
                                    DoNotMixInd = apiResponse.Extended.DoNotMixInd,
                                    GroupMemberCount = apiResponse.Extended.GroupMemberCount,
                                    LowBedVehicleAllowed = apiResponse.Extended.LowBedVehicleAllowed,
                                    FiftyTonVehicleAllowed = apiResponse.Extended.FiftyTonVehicleAllowed,
                                    HeightControlInd = apiResponse.Extended.HeightControlInd,
                                    PhysicalProjNo = apiResponse.Extended.PhysicalProjNo,
                                    PhysicalDeliveryProjNo = apiResponse.Extended.PhysicalDeliveryProjNo,
                                    Wbs1 = apiResponse.Extended.Wbs1,
                                    Wbs2 = apiResponse.Extended.Wbs2,
                                    Wbs3 = apiResponse.Extended.Wbs3,
                                    Bbs = apiResponse.Extended.Bbs,
                                    StElement = apiResponse.Extended.StElement,
                                    VariousBar = apiResponse.Extended.VariousBar,
                                    Spare1 = apiResponse.Extended.Spare1,
                                    Spare2 = apiResponse.Extended.Spare2,
                                    Spare3 = apiResponse.Extended.Spare3,
                                    Spare4 = apiResponse.Extended.Spare4,
                                    Spare5 = apiResponse.Extended.Spare5,
                                },

                                Items = apiResponse.Items.Select(apiItem =>
                                {
                                    var dbMatch = items.FirstOrDefault(d => d.ITEM_NO.PadLeft(6, '0') == apiItem.Extended.ItemNo.PadLeft(6, '0'));



                                    return new Item_U
                                    {
                                        Tag = apiItem.Tag,
                                        Part = apiItem.Part,
                                        Collection = apiItem.Collection,
                                        AggregationCode = apiItem.AggregationCode,
                                        BvbsString = apiItem.BvbsString,
                                        Replaced = apiItem.Replaced,
                                        ExtraInfo = apiItem.ExtraInfo,
                                        Position = apiItem.Position,
                                        Diameter = TryParseInt(apiItem.Extended.CabDia),
                                        Length = apiItem.Length,
                                        Quantity = TryParseInt(apiItem.Quantity),
                                        MachineId = apiItem.MachineId,
                                        BundleTypeId = apiItem.BundleTypeId,
                                        LoadNo = dbMatch?.LOAD_NO.ToString(),
                                        LayerNo = TryParseInt(dbMatch?.LAYER_NO),
                                        Extended = new ExtendedItem_U
                                        {
                                            Cliente = apiItem.Extended.Cliente,
                                            OrderNo = apiItem.Extended.OrderNo,
                                            ItemNo = apiItem.Extended.ItemNo,
                                            ParentItemNo = apiItem.Extended.ParentItemNo,
                                            SapOrderItem = apiItem.Extended.SapOrderItem,
                                            MaterialNo = apiItem.Extended.MaterialNo,
                                            ProductHierarchy = apiItem.Extended.ProductHierarchy,
                                            OrderQty = TryParseInt(apiItem.Extended.OrderQty),
                                            SalesUom = apiItem.Extended.SalesUom,
                                            FgProductionWtKg = TryParseFloat(apiItem.Extended.FgProductionWtKg),
                                            ComponentProductionWtKg = TryParseFloat(apiItem.Extended.ComponentProductionWtKg),
                                            ProductMarking = apiItem.Extended.ProductMarking,
                                            ComponentPrefix = apiItem.Extended.ComponentPrefix,
                                            OrderPieces = TryParseInt(apiItem.Extended.OrderPieces),
                                            DeliveredPieces = TryParseInt(apiItem.Extended.DeliveredPieces),
                                            ActualDelDate = apiItem.Extended.ActualDelDate,
                                            DeliveryStatus = apiItem.Extended.DeliveryStatus,
                                            CancellationStatus = apiItem.Extended.CancellationStatus,
                                            EnvLength = TryParseInt(apiItem.Extended.EnvLength),
                                            EnvWidth = TryParseInt(apiItem.Extended.EnvWidth),
                                            EnvHeight = TryParseInt(apiItem.Extended.EnvHeight),
                                            Wbs1 = apiItem.Extended.Wbs1,
                                            Wbs2 = apiItem.Extended.Wbs2,
                                            Wbs3 = apiItem.Extended.Wbs3,
                                            Wbs4 = apiItem.Extended.Wbs4,
                                            Wbs5 = apiItem.Extended.Wbs5,
                                            ProjStage = apiItem.Extended.ProjStage,
                                            BbsNo = apiItem.Extended.BbsNo,
                                            BbsDesc = apiItem.Extended.BbsDesc,
                                            NeedsGalvanizingInd = apiItem.Extended.NeedsGalvanizingInd,
                                            NeedsProductionInd = apiItem.Extended.NeedsProductionInd,
                                            CabSpiralPitch = TryParseInt(apiItem.Extended.CabSpiralPitch),
                                            CabFleetPitch = TryParseInt(apiItem.Extended.CabFleetPitch),
                                            CabBarGrade = apiItem.Extended.CabBarGrade,
                                            CabDia = TryParseInt(apiItem.Extended.CabDia),
                                            CabBvbs = apiItem.Extended.CabBvbs,
                                            CabCutLength = TryParseInt(apiItem.Extended.CabCutLength),
                                            CabCouplerEndNo = TryParseInt(apiItem.Extended.CabCouplerEndNo),
                                            CabFormerSize = TryParseInt(apiItem.Extended.CabFormerSize),
                                            CabShapeGroup = apiItem.Extended.CabShapeGroup,
                                            ShapeCode = apiItem.Extended.ShapeCode,
                                            CabCouEndType1 = apiItem.Extended.CabCouEndType1,
                                            CabCouEndType2 = apiItem.Extended.CabCouEndType2,
                                            CabNoOfBend = TryParseInt(apiItem.Extended.CabNoOfBend),
                                            Remarks = apiItem.Extended.Remarks,
                                            StElement = apiItem.Extended.StElement,
                                            QtyInBaseUom = TryParseInt(apiItem.Extended.QtyInBaseUom),
                                            BaseUom = apiItem.Extended.BaseUom,
                                            DeliveryQty = TryParseFloat(apiItem.Extended.DeliveryQty),
                                            CabMainBarAngle = TryParseInt(apiItem.Extended.CabMainBarAngle),
                                            InvLenInMm = TryParseInt(apiItem.Extended.InvLenInMm),
                                            GradeDia = apiItem.Extended.GradeDia,
                                            SapProductGroup = apiItem.Extended.SapProductGroup,
                                            CabStandardEndNo = TryParseInt(apiItem.Extended.CabStandardEndNo),
                                            CabFormerType = TryParseInt(apiItem.Extended.CabFormerType),
                                            ProductType = apiItem.Extended.ProductType,
                                            CabExtendedEndNo = TryParseInt(apiItem.Extended.CabExtendedEndNo),
                                            CabStudEndNo = TryParseInt(apiItem.Extended.CabStudEndNo),
                                            BbsPageNo = TryParseInt(apiItem.Extended.BbsPageNo),
                                            IdenticalHashcode = apiItem.Extended.IdenticalHashcode,
                                            SimilarMainbarHashcode = apiItem.Extended.SimilarMainbarHashcode,
                                            SimilarExtlinkHashcode = apiItem.Extended.SimilarExtlinkHashcode,
                                            BendingHr = apiItem.Extended.BendingHr,
                                            Spare1 = apiItem.Extended.Spare1,
                                            Spare2 = apiItem.Extended.Spare2,
                                            Spare3 = apiItem.Extended.Spare3,
                                            Spare4 = apiItem.Extended.Spare4,
                                            Spare5 = apiItem.Extended.Spare5
                                        }
                                    };
                                }).ToList()
                            };


                            var settings = new JsonSerializerSettings
                            {
                                FloatFormatHandling = FloatFormatHandling.DefaultValue,
                                FloatParseHandling = FloatParseHandling.Double,
                                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                                DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind

                            };


                            var jsonBody = JsonConvert.SerializeObject(mappedOrder, settings);
                            File.WriteAllText(@"C:\Logs\order-payload.json", jsonBody);
                            //var clientInstance = new HttpClient();
                            //client.DefaultRequestHeaders.Add("x-api-key", "6B29FC40-CA47-1067-B31D-00DD010662DA");

                            //string url = $"http://10.72.181.2/api/public/run-order/{id}?raw-import=false";
                            string url = $"http://172.20.0.5/api/public/run-order/{id}?raw-import=false";

                            var response = await client.PutAsync(url, new StringContent(jsonBody, Encoding.UTF8, "application/json"));
                            
                            


                            if (response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.Conflict)
                            {

                                var lSQL = "UPDATE SCM_ADMIN.tbl_order_item_load_schnell" +
                                " SET STATUS = 'Y' WHERE ORDER_NO = '" + orderNo + "' ";
                                var lOracleCmd = new OracleCommand(lSQL, cnIDB);

                                lOracleCmd.CommandTimeout = 1200;
                                lOracleCmd.Transaction = oracleTransIDB;
                                lOracleCmd.ExecuteNonQuery();

                                await _contextFactory.CreateDbContext().Database.ExecuteSqlRawAsync(
                                    @"UPDATE scm_order_header
                               SET SchnellUpdateStatus = 'Y', SchnellLastModifiedDate = GETDATE()
                               WHERE order_no = {0}", orderNo);

                            }
                            else if (response.StatusCode == HttpStatusCode.Forbidden)
                            {
                                return (false, "Forbidden");
                            }
                            else
                            {

                                var lSQL = "UPDATE SCM_ADMIN.tbl_order_item_load_schnell" +
                                  " SET STATUS = 'E' WHERE ORDER_NO = '" + orderNo + "' ";
                                var lOracleCmd = new OracleCommand(lSQL, cnIDB);

                                lOracleCmd.CommandTimeout = 1200;
                                lOracleCmd.Transaction = oracleTransIDB;
                                lOracleCmd.ExecuteNonQuery();

                                string error = await response.Content.ReadAsStringAsync();
                                await _contextFactory.CreateDbContext().Database.ExecuteSqlRawAsync(
                                    @"UPDATE scm_order_header
                               SET SchnellUpdateStatus = 'E', SchnellLastModifiedDate = GETDATE()
                               WHERE order_no = {0}", orderNo);


                            }
                        }
                        else

                        {


                            var lSQL = "UPDATE SCM_ADMIN.tbl_order_item_load_schnell" +
                                " SET STATUS = 'X' WHERE ORDER_NO = '" + orderNo + "' ";
                            var lOracleCmd = new OracleCommand(lSQL, cnIDB);
                        
                            lOracleCmd.CommandTimeout = 1200;
                            lOracleCmd.Transaction = oracleTransIDB;
                            lOracleCmd.ExecuteNonQuery();
                            
                        }
                    }
                    catch (Exception ex)
                    {
                        File.AppendAllText(@"C:\Logs\UpdateOrders.log",
                            $"Order {orderNo} failed: {ex.Message}\n");
                        // optionally mark the order as 'E' in Oracle so next run can retry
                    }
                }
                return (true, "All orders processed successfully.");
            }
            catch (Exception ex)
            {
                return (false, $"Exception occurred: {ex.Message}");
            }
        }


        public async Task<(bool IsSuccess, string Message)> POST_OrderAssignmentData()
        {
            try
            {
                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();

                                // ---------------- Header query  to get the orders----------------
                var headerQuery = @"
                    SELECT 
                    h.order_request_no, h.order_no, h.cust_code, h.cust_name, h.contract_no, h.proj_no, 
                    h.proj_name, h.cust_po_no, h.cust_order_date, h.req_del_date_fr AS req_del_date_original,
                    h.req_del_date_to AS req_del_date_revised, h.actual_del_date, h.order_type,
                    h.cust_class, h.proj_segment, h.proj_sub_segment, h.job_site_location, h.forecast_order_no,
                    h.account_mgr, h.segment_mgr, h.proj_coordinator, h.product_type, h.vehicle_type,
                    h.order_pieces, h.delivered_pieces, h.credit_status, h.approval_status, h.delivery_status,
                    h.cancellation_status, h.mat_source_ind, h.urgent_order_ind, h.allow_partial_delivery_ind,
                    h.forecast_order_ind, h.local_order_ind, h.mto_ind, h.self_collect_ind, h.premium_service_ind,
                    h.crane_booked_ind, h.barge_booked_ind, h.police_escort_ind, h.company_code, h.no_of_items,
                    h.int_remark, h.ext_remark, h.remarks, h.order_group_id, h.group_member_count,
                    h.low_bed_vehicle_allowed, h.fiftyton_vehicle_allowed, h.physical_proj_no,
                    h.physical_delivery_proj_no
                    FROM SCM_Order_Header h
                    WHERE h.AssignedTo = 'SCHNELL' AND (h.SchnellStatus IS NULL OR h.SchnellStatus = 'E')";
                //HERE h.AssignedTo = 'SCHNELL'AND h.order_no = '1031787396'




                var headers = (await connection.QueryAsync<dynamic>(headerQuery)).ToList();
                if (headers == null || headers.Count == 0)
                    throw new Exception("No matching order headers found.");

                foreach (var h in headers)
                {
                    var orderNo = (string)h.order_no;
                    var orderRequestNo = (string)h.order_request_no;

                    // ---------------- Item query ----------------

                    var items = (await connection.QueryAsync<dynamic>(
                            "GetShearBendItems",
                            new { orderRequestNo },
                            commandType: System.Data.CommandType.StoredProcedure
                        )).ToList();

                    //if (items.Count == 0)
                    //{
                    //    await _dbcontext.Database.ExecuteSqlRawAsync(
                    //        @"UPDATE scm_order_header
                    //  SET SchnellStatus = 'E', SchnellLastModifiedDate = GETDATE()
                    //  WHERE order_no = {0}", orderNo);
                    //    continue;
                    //}

                    // ---------------- Tag generation ----------------
                    var year = DateTime.Now.Year % 100;
                    var prefix = $"S{year}";

                    var lastTag = await connection.ExecuteScalarAsync<string>(
                        @"SELECT TOP 1 TagNo 
                  FROM SCM_Order_Item 
                  WHERE TagNo LIKE @prefix + '%' 
                  ORDER BY TagNo DESC",
                        new { prefix }
                    );

                    int counter = 1;
                    if (!string.IsNullOrEmpty(lastTag) && lastTag.Length > 3)
                    {
                        var numberPart = lastTag.Substring(3);
                        if (int.TryParse(numberPart, out var n))
                            counter = n + 1;
                    }

                    foreach (var i in items)
                    {
                        if (string.IsNullOrEmpty(i.TagNo))
                        {
                            string newTag = $"{prefix}{counter:D7}";
                            counter++;

                            i.TagNo = newTag;
                            await connection.ExecuteAsync(
                                @"UPDATE SCM_Order_Item 
                          SET TagNo = @TagNo 
                          WHERE order_request_no = @OrderRequestNo 
                            AND item_no = @ItemNo",
                                new { TagNo = newTag, OrderRequestNo = orderRequestNo, ItemNo = i.item_no }
                            );
                        }
                    }




                    var order = new OrderAssignment
                    {
                        Codename = orderNo,
                        Description = items.Max(i => i.bbs_no)?.Trim(),  //new change
                        ShippingAt = DateTime.Now,
                        Position = 0,
                        Paused = false,
                        Completed = false,
                        Extended = MapHeaderToExtended_Insert(h),
                        Items = items.Select(i =>
                        {

                            return new Item
                            {
                                Tag = i.TagNo,
                                Part = "",
                                Collection = "",
                                AggregationCode = null,
                                BvbsString = i.cab_bvbs,
                                Replaced = false,
                                ExtraInfo = "",
                                Position = 0,
                                Diameter = (int)i.cab_dia,
                                Quantity = (int)i.order_pieces,
                                Length = i.cab_cut_length,
                                MachineId = null,
                                BundleTypeId = 1,
                                LoadNo = "",
                                LayerNo = 0,
                                Extended = MapItemToExtended_Insert(i, orderNo)
                            };
                        }).ToList()
                    };

                    var settings = new JsonSerializerSettings
                    {
                        FloatFormatHandling = FloatFormatHandling.DefaultValue,
                        FloatParseHandling = FloatParseHandling.Double,
                        DateFormatHandling = DateFormatHandling.IsoDateFormat,
                        DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                        Converters =
                         {
                            new Newtonsoft.Json.Converters.IsoDateTimeConverter
                            {
                                DateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fffZ"
                            }
                         }
                    };

                    var jsonBody = JsonConvert.SerializeObject(order, settings);
                    //File.WriteAllText(@"C:\Logs\order-payload.json", jsonBody);


                    // ---------------- API call ----------------
                    var client = new HttpClient();
                    client.DefaultRequestHeaders.Add("x-api-key", "6B29FC40-CA47-1067-B31D-00DD010662DA");

                    //string apiUrl = "http://10.72.181.2/api/public/run-order?raw-import=true";
                    string apiUrl = "http://172.20.0.5/api/public/run-order?raw-import=true";
                    var response = await client.PostAsync(apiUrl, new StringContent(jsonBody, Encoding.UTF8, "application/json"));
                    string hOrderNo = (string)h.order_no;

                    if (response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.Conflict)
                    {
                        await _contextFactory.CreateDbContext().Database.ExecuteSqlRawAsync(
                            @"UPDATE scm_order_header
                      SET SchnellStatus = 'Y', SchnellLastModifiedDate = GETDATE()
                      WHERE order_no = {0}", hOrderNo);
                    }
                    else if (response.StatusCode == HttpStatusCode.Forbidden)
                    {
                        return (false, "Forbidden");
                    }
                    else
                    {
                        string error = await response.Content.ReadAsStringAsync();
                         await _contextFactory.CreateDbContext().Database.ExecuteSqlRawAsync(
                            @"UPDATE scm_order_header
                      SET SchnellStatus = 'E', SchnellLastModifiedDate = GETDATE()
                      WHERE order_no = {0}", hOrderNo);

                        return (false, error);


                    }
                }

                return (true, "All orders inserted successfully.");
            }
            catch (Exception ex)
            {
                return (false, $"Exception occurred: {ex.Message}");
            }
        }

        private ExtendedHeader MapHeaderToExtended_Insert(dynamic h)
        {
            return new ExtendedHeader
            {
                OrderNo = h.order_no?.Trim(),
                CustCode = h.cust_code?.Trim(),
                CustName = h.cust_name?.Trim(),
                ContractNo = h.contract_no?.Trim(),
                ProjNo = h.proj_no?.Trim(),
                ProjName = h.proj_name?.Trim(),
                CustPoNo = h.cust_po_no?.Trim(),
                CustOrderDate = ConvertDateFormat(h.cust_order_date),
                ReqDelDateOriginal = ConvertDateFormat(h.req_del_date_original),
                ReqDelDateRevised = ConvertDateFormat(h.req_del_date_revised),
                ActualDelDate = string.IsNullOrWhiteSpace(h.actual_del_date) ? null : h.actual_del_date.Trim(),
                OrderType = h.order_type?.Trim(),
                CustClass = h.cust_class?.Trim(),
                ProjSegment = h.proj_segment?.Trim(),
                ProjSubSegment = h.proj_sub_segment?.Trim(),
                JobSiteLocation = h.job_site_location?.Trim(),
                ForecastOrderNo = h.forecast_order_no?.Trim(),
                AccountMgr = h.account_mgr?.Trim(),
                SegmentMgr = h.segment_mgr?.Trim(),
                ProjCoordinator = h.proj_coordinator?.Trim(),
                ProductType = h.product_type?.Trim(),
                VehicleType = h.vehicle_type?.Trim(),
                OrderPieces = TryParseInt(h.order_pieces),
                DeliveredPieces = TryParseInt(h.delivered_pieces),
                CreditStatus = h.credit_status?.Trim(),
                ApprovalStatus = h.approval_status?.Trim(),
                DeliveryStatus = h.delivery_status?.Trim(),
                CancellationStatus = h.cancellation_status?.Trim(),
                MatSourceInd = h.mat_source_ind?.Trim(),
                UrgentOrderInd = h.urgent_order_in?.Trim(),
                AllowPartialDeliveryInd = h.allow_partial_delivery_ind?.Trim(),
                ForecastOrderInd = h.forecast_order_ind?.Trim(),
                LocalOrderInd = h.local_order_ind?.Trim(),
                MtoInd = h.mto_ind?.Trim(),
                SelfCollectInd = h.self_collect_ind?.Trim(),
                PremiumServiceInd = h.premium_service_ind?.Trim(),
                CraneBookedInd = h.crane_booked_ind?.Trim(),
                BargeBookedInd = h.barge_booked_ind?.Trim(),
                PoliceEscortInd = h.police_escort_ind?.Trim(),
                CompanyCode = h.company_code?.Trim(),
                NoOfItems = TryParseInt(h.no_of_items),
                IntRemark = h.int_remark?.Trim(),
                ExtRemark = h.ext_remark?.Trim(),
                Remarks = h.remarks?.Trim(),
                OrderGroupId = h.order_group_id?.Trim(),
                OnHoldInd = false,
                FirstPromisedDate = DateTime.Now,
                ContractualLeadtimeInd = false,
                TimeStampInsert = h.time_stamp,
                NeedsLorryCraneInd = false,
                ZeroToleranceInd = false,
                CallBefDelInd = false,
                ConquasInd = false,
                SpecialPassInd = false,
                DoNotMixInd = false,
                GroupMemberCount = TryParseInt(h.group_member_count),
                LowBedVehicleAllowed = h.low_bed_vehicle_allowed?.Trim(),
                FiftyTonVehicleAllowed = h.fiftyton_vehicle_allowed?.Trim(),
                PhysicalProjNo = h.physical_proj_no?.Trim(),
                PhysicalDeliveryProjNo = h.physical_delivery_proj_no?.Trim(),
                Wbs1 = "",
                Wbs2 = "",
                Wbs3 = "",
                Bbs = "",
                StElement = "",
                VariousBar = false,
                Spare1 = "",
                Spare2 = "",
                Spare3 = "",
                Spare4 = "",
                Spare5 = ""
            };

        }
        private ExtendedItem MapItemToExtended_Insert(dynamic i, string orderNo)
        {
            return new ExtendedItem
            {
                Cliente = "",
                OrderNo = orderNo,
                ItemNo = i.item_no?.Trim(),
                ParentItemNo = i.parent_item_no?.Trim(),
                SapOrderItem = i.sap_order_item?.Trim(),
                MaterialNo = i.material_no?.Trim(),
                ProductHierarchy = i.product_hierarchy?.Trim(),
                OrderQty = (int)i.order_pieces,
                SalesUom = i.sales_uom?.Trim(),
                FgProductionWtKg = (float)i.fg_production_wt_kg,
                ComponentProductionWtKg = (float)i.component_production_wt_kg,
                ProductMarking = i.product_marking?.Trim()?.Length > 10 ? i.product_marking.Trim().Substring(0, 10) : i.product_marking?.Trim(),
                ComponentPrefix = i.component_prefix?.Trim(),
                OrderPieces = i.order_pieces,
                DeliveredPieces = i.delivered_pieces,
                ActualDelDate = string.IsNullOrWhiteSpace(i.actual_del_date) ? null : i.actual_del_date.Trim(),
                DeliveryStatus = i.delivery_status?.Trim(),
                CancellationStatus = i.cancellation_status?.Trim(),
                EnvLength = TryParseInt(i.env_length),
                EnvWidth = TryParseInt(i.env_width),
                EnvHeight = TryParseInt(i.env_height),
                Wbs1 = i.wbs1?.Trim(),
                Wbs2 = i.wbs2?.Trim(),
                Wbs3 = i.wbs3?.Trim(),
                Wbs4 = i.wbs4?.Trim(),
                Wbs5 = i.wbs5?.Trim(),
                ProjStage = i.proj_stage?.Trim(),
                BbsNo = i.bbs_no?.Trim(),
                BbsDesc = i.bbs_desc?.Trim(),
                NeedsGalvanizingInd = i.needs_galvanizing_ind?.Trim(),
                NeedsProductionInd = i.needs_production_ind?.Trim(),
                CabSpiralPitch = TryParseInt(i.cab_spiral_pitch),
                CabFleetPitch = TryParseInt(i.cab_fleet_pitch),
                CabBarGrade = i.cab_bar_grade?.Trim(),
                CabDia = (int)i.cab_dia,
                CabBvbs = i.cab_bvbs?.Trim(),
                CabCutLength = TryParseInt(i.cab_cut_length),
                CabCouplerEndNo = TryParseInt(i.cab_coupler_end_no),
                CabFormerSize = TryParseInt(i.cab_former_size),
                CabShapeGroup = i.cab_shape_group?.Trim(),
                ShapeCode = i.shape_code?.Trim(),
                CabCouEndType1 = i.cab_cou_end_type1?.Trim(),
                CabCouEndType2 = i.cab_cou_end_type2?.Trim(),
                CabNoOfBend = TryParseInt(i.cab_no_of_bend),
                Remarks = i.remarks?.Trim(),
                StElement = i.st_element?.Trim(),
                QtyInBaseUom = TryParseInt(i.qty_in_base_uom),
                BaseUom = i.base_uom?.Trim(),
                DeliveryQty = 0,
                CabMainBarAngle = TryParseInt(i.cab_main_bar_angle),
                InvLenInMm = TryParseInt(i.inv_len_in_mm),
                GradeDia = i.grade_dia?.Trim(),
                SapProductGroup = i.sap_product_group?.Trim(),
                CabStandardEndNo = TryParseInt(i.cab_standard_end_no),
                CabFormerType = TryParseInt(i.cab_former_type),
                ProductType = i.product_type?.Trim(),
                CabExtendedEndNo = TryParseInt(i.cab_extended_end_no),
                CabStudEndNo = TryParseInt(i.cab_stud_end_no),
                BbsPageNo = TryParseInt(i.bbs_page_no),
                IdenticalHashcode = TryParseInt(i.identical_hashcode),
                SimilarMainbarHashcode = TryParseInt(i.similar_mainbar_hashcode),
                SimilarExtlinkHashcode = TryParseInt(i.similar_extlink_hashcode),
                BendingHr = "",
                Spare1 = i.product_marking?.Trim(),
                Spare2 = "",
                Spare3 = "",
                Spare4 = "",
                Spare5 = ""
            };
        }

    }

}
