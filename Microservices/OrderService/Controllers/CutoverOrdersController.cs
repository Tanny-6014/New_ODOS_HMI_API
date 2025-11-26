using AutoMapper;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.Data.SqlClient;
using OrderService.Dtos;
using OrderService.Interfaces;
using Oracle.ManagedDataAccess.Client;
using Microsoft.AspNet.Identity;
using System.Data.SqlClient;
using System.Net;
using System.Globalization;
using OrderService.Models;
using SixLabors.ImageSharp.ColorSpaces;
using OrderService.Repositories;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.Security.Policy;
using System.Data;
using MongoDB.Bson;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using static OrderService.Controllers.ProcessController;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CutoverOrdersController : Controller
    {
        public string strServer = "DEV";
        private readonly IOrder _OrderRepository;
        private readonly IMapper _mapper;

        private DBContextModels db = new DBContextModels();

        private string strCIS_Connection = "";
        private string strNDS_Connection = "";
        private string strIDB_Connection = "";
        private string strSTS_Connection = "";
        private string strNGW_Connection = "";

        public CutoverOrdersController(IOrder orderService, IMapper mapper)
        {
            _OrderRepository = orderService;
            _mapper = mapper;

        }




        [HttpGet]
        [Route("/CutoverCase2Update/{CaseScenario}")]
        public async Task<ActionResult> CutoverCase2Update(int CaseScenario)
        {
            ProcessController lprocess = new ProcessController();
            // Create SQL connection object
            var cnNDS = new SqlConnection();

            // This list will hold the final result (OrderNumber details)
            var orderNumbers = new List<GetOrderNumberUpdateDto>();
            bool cutoverResult = false;


            try
            {
                // Open custom connection (you already have OpenNDSConnection method)
                lprocess.OpenNDSConnection(ref cnNDS);

                if (cnNDS.State == ConnectionState.Open)
                {
                    // SQL query: Joins OrderUpdate with other related tables
                    // to fetch required details
                    var lSQL = $@"
                SELECT 
                    opj.OrderNumber,
                    opj.CustomerCode,
                    opj.ProjectCode,
                    oesd.StructureElement,
                    oesd.ProductType,
                    oesd.ScheduledProd,
                    opj.OrderSource,
                    op.Contract,
                    oesd.SAPSOR,
                    opj.TransportMode,
                    opj.PONumber
                FROM OrderUpdateCase2Custover ou
                INNER JOIN OESProjOrder opj 
                    ON ou.OrderNumber = opj.OrderNumber
                INNER JOIN OESProjOrdersSE oesd 
                    ON ou.OrderNumber = oesd.OrderNumber
                INNER JOIN OESProcess op 
                    ON ou.OrderNumber = op.JobID
                WHERE ou.Status = 0 and ou.CaseScenario={CaseScenario}";

                    // Execute the SQL command
                    using (var lCmd = new SqlCommand(lSQL, cnNDS))
                    using (var reader = lCmd.ExecuteReader())
                    {
                        // Loop through each row returned by SQL
                        while (reader.Read())
                        {
                            // Create DTO object and map SQL fields
                            var tempObj = new GetOrderNumberUpdateDto
                            {
                                // OrderNumber is INT → handle DBNull safely
                                JobID = reader.IsDBNull(reader.GetOrdinal("OrderNumber"))
                                                    ? 0
                                                    : reader.GetInt32(reader.GetOrdinal("OrderNumber")),

                                // Map all string fields (use Trim() to avoid extra spaces)
                                CustomerCode = reader["CustomerCode"].ToString().Trim(),
                                ProjectCode = reader["ProjectCode"].ToString().Trim(),
                                StructureElement = reader["StructureElement"].ToString().Trim(),
                                ProdType = reader["ProductType"].ToString().Trim(),
                                ScheduledProd = reader["ScheduledProd"].ToString().Trim(),
                                OrderSource = reader["OrderSource"].ToString().Trim(),
                                ContractNo = reader["Contract"].ToString().Trim(),
                                SAPSOR = reader["SAPSOR"].ToString().Trim(),
                                TransportMode = reader["TransportMode"].ToString().Trim(),
                                PONumber = reader["PONumber"].ToString().Trim(),
                                // Custom logic fields
                                ActionType = "Withdraw",
                                UserName = string.Empty
                            };

                            // Add to list
                            orderNumbers.Add(tempObj);
                        }
                    }

                    var lUpcomingObj = new UpcomingOrdersController();

                    // Process each order number for Withdraw logic
                    foreach (var tempObj in orderNumbers)
                    {
                        #region get data from SAP using tempObj SAPSOR
                        //var results = new List<YmsdtOrderHdrDto>();
                        //var pCISCon = new OracleConnection();

                        //string lSQL1 = "SELECT * FROM YMSDT_ORDER_HDR where order_request_no=" + tempObj.SAPSOR + " ";

                        //using (var lOraCmd = new OracleCommand(lSQL1, pCISCon))
                        //{
                        //    //lOraCmd.Parameters.Add(new OracleParameter(":orderNo", tempObj.SAPSOR));
                        //    lOraCmd.CommandTimeout = 300;

                        //    using (var lOraRst = lOraCmd.ExecuteReader())
                        //    {
                        //        while (lOraRst.Read())
                        //        {
                        //            var obj = new YmsdtOrderHdrDto();

                        //            for (int i = 0; i < lOraRst.FieldCount; i++)
                        //            {
                        //                var colName = lOraRst.GetName(i);
                        //                var val = lOraRst.IsDBNull(i) ? "" : lOraRst.GetValue(i).ToString().Trim();

                        //                var prop = typeof(YmsdtOrderHdrDto).GetProperty(colName);
                        //                if (prop != null && prop.CanWrite)
                        //                {
                        //                    prop.SetValue(obj, val);
                        //                }
                        //            }

                        //            results.Add(obj);
                        //        }
                        //        lOraRst.Close();
                        //    }

                        //}

                        #endregion

                        #region Get Submitted Order Data from OesProcess
                        //List<ProcessModels> lProcessList = new List<ProcessModels>();
                        List<ProcessModels> lProcessList = new List<ProcessModels>();

                        if (cnNDS.State == ConnectionState.Open)
                        {
                            lSQL = "select * from oesprocess where jobid = " + tempObj.JobID + " ";
                            // Execute the SQL command
                            using (var lCmd = new SqlCommand(lSQL, cnNDS))
                            using (var reader = lCmd.ExecuteReader())
                            {
                                // Loop through each row returned by SQL
                                while (reader.Read())
                                {
                                    var lProcess = new ProcessModels();

                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        var colName = reader.GetName(i);
                                        //var val = reader.IsDBNull(i) ? "" : reader.GetValue(i).ToString().Trim();
                                        var val = reader.IsDBNull(i) ? null : reader.GetValue(i);

                                        var prop = typeof(ProcessModels).GetProperty(colName);
                                        if (prop != null && prop.CanWrite)
                                        {
                                            prop.SetValue(lProcess, val);
                                        }
                                    }

                                    lProcessList.Add(lProcess);
                                }
                            }
                        }
                        #endregion

                        #region Step 2: Withdraw order using cutover api
                        var result = await lprocess.CancelProcessCase2Cutover(
                            tempObj.CustomerCode,
                            tempObj.ProjectCode,
                            tempObj.ContractNo,
                            tempObj.JobID,
                            tempObj.StructureElement,
                            tempObj.ProdType,
                            tempObj.OrderSource,
                            tempObj.ScheduledProd,
                            tempObj.ActionType,
                            tempObj.UserName
                        );

                        var okResult = result as OkObjectResult;
                        var jsonStr = JsonConvert.SerializeObject(okResult.Value);
                        dynamic resp = JsonConvert.DeserializeObject<dynamic>(jsonStr);
                        bool withdrawSuccess = (bool)resp.Value.success;
                        #endregion

                        if (withdrawSuccess)
                        {
                            #region Step 3: Update cust proj from cutover sp
                            var UpdateResult = await GetIncomingUpdateData(new List<string> { tempObj.JobID.ToString() });
                            var okUpdateResult = UpdateResult as OkObjectResult;
                            var jsonokUpdateStr = JsonConvert.SerializeObject(okUpdateResult.Value);
                            dynamic UpdateResponse = JsonConvert.DeserializeObject<dynamic>(jsonokUpdateStr);
                            bool updateSuccess = (bool)UpdateResponse.result;
                            #endregion

                            if (updateSuccess)
                            {
                                #region step 4: Submit it form cutover api

                                #region step 4.1: Get Updated customer/project
                                string NewCustomerCode = "";
                                string NewProjectCode = "";
                                string poDate = "";
                                lSQL = $@" SELECT CustomerCode, ProjectCode, PODate FROM OESProjOrder WHERE OrderNumber = {tempObj.JobID} ";

                                // Execute the SQL command
                                using (var lCmd = new SqlCommand(lSQL, cnNDS))
                                using (var reader = lCmd.ExecuteReader())
                                {
                                    // Loop through each row returned by SQL
                                    while (reader.Read())
                                    {
                                        // Map all string fields (use Trim() to avoid extra spaces)
                                        NewCustomerCode = reader["CustomerCode"].ToString().Trim();
                                        NewProjectCode = reader["ProjectCode"].ToString().Trim();
                                        poDate = ((DateTime)reader["PODate"]).ToString("yyyy-MM-dd");
                                    }
                                }
                                #endregion

                                #region step 4.2: Get Contract number
                                GetContractlistDto GetContractlist = new GetContractlistDto
                                {
                                    CustomerCode = NewCustomerCode,
                                    ProjectCode = NewProjectCode,
                                    ProdType = tempObj.ProdType,
                                    ProdTypeL2 = tempObj.ProdType,
                                    OrderNumber = tempObj.JobID,
                                    StructureElement = tempObj.StructureElement
                                };
                                List<string> ContractList = lprocess.getContractList(GetContractlist);
                                #endregion

                                if (ContractList.Count > 0)
                                {
                                    string lContractNo = ContractList[0].Length > 10 ? ContractList[0].Substring(0, 10) : "";

                                    if (lContractNo != "")
                                    {
                                        // Add a Contract Check

                                        var contractResult = await lprocess.checkContract(lContractNo);
                                        var jsonResult = (JsonResult)contractResult;

                                        // Convert the Value to JSON, then deserialize into dynamic
                                        dynamic lConCheck = JsonConvert.DeserializeObject<dynamic>(
                                            JsonConvert.SerializeObject(jsonResult.Value)
                                        );
                                        bool noExpired = (bool)lConCheck.noexpired;

                                        if (noExpired == false)
                                        {
                                            // NOTE: Log an error 
                                            await LogError_CutoverActivity(tempObj.JobID, 3, "Expired Contract"); // Expired Contract 
                                            continue;
                                        }

                                        #region step 4.2: Get Ship to Party Address
                                        var shipResult = await lprocess.GetShipToParty(NewProjectCode, lContractNo);
                                        var okShipResult = shipResult as OkObjectResult;
                                        var jsonShipStr = JsonConvert.SerializeObject(okShipResult.Value);
                                        dynamic lShipToAddressList = JsonConvert.DeserializeObject<dynamic>(jsonShipStr);

                                        string lShipToAddress = "";

                                        for (int i = 0; i < lShipToAddressList.Count; i++)
                                        {
                                            string lTempValue = (string)lShipToAddressList[i];
                                            if (lTempValue.Substring(lTempValue.Length - 7, 6) == NewProjectCode)
                                            {
                                                lShipToAddress = lShipToAddressList[i];
                                            }
                                            //var hdr = results.First();  // take the first record
                                        }
                                        #endregion

                                        var lProcess = new ProcessModels();
                                        if (lProcessList.Count == 1)
                                        {
                                            lProcess = lProcessList.First();
                                        }
                                        else
                                        {
                                            lProcess = lProcessList.First();
                                        }

                                        UserAccessController lUa = new UserAccessController();
                                        var lUserType = lUa.getUserType(lProcess.ProcessedBy);

                                        SubmitProcessDto submitProcess = new SubmitProcessDto
                                        {
                                            CustomerCode = NewCustomerCode,
                                            ProjectCode = NewProjectCode,
                                            ContractNo = lContractNo,
                                            ProdType = lProcess.ProdType,
                                            JobID = tempObj.JobID,
                                            CashPayment = (lProcess.CashPayment == true) ? "Y" : "N",
                                            CABFormer = lProcess.CABFormer,
                                            ShipToParty = lShipToAddress,
                                            ProjectStage = lProcess.ProjectStage,
                                            ReqDateFrom = lProcess.RequiredDateFrom?.ToString("yyyy-MM-dd"),
                                            ReqDateTo = lProcess.RequiredDateTo?.ToString("yyyy-MM-dd"),
                                            PONumber = lProcess.PONumber,
                                            PODate = poDate,
                                            WBS1 = lProcess.WBS1,
                                            WBS2 = lProcess.WBS2,
                                            WBS3 = lProcess.WBS3,
                                            VehicleType = lProcess.VehicleType,
                                            UrgentOrder = lProcess.Urgent,
                                            Conquas = lProcess.Conquas,
                                            Crane = lProcess.Crane,
                                            PremiumService = lProcess.Premium,
                                            ZeroTol = lProcess.ZeroTol,
                                            CallBDel = lProcess.CallDel,
                                            DoNotMix = (lProcess.DoNotMix == true) ? true : false,
                                            SpecialPass = lProcess.SpecialPass,
                                            VehLowBed = lProcess.LowBed,
                                            Veh50Ton = lProcess.Veh50Ton,
                                            Borge = lProcess.Borge,
                                            PoliceEscort = lProcess.PoliceEscort,
                                            TimeRange = lProcess.TimeRange,
                                            IntRemarks = lProcess.IntRemarks,
                                            ExtRemarks = lProcess.ExtRemarks,
                                            OrderSource = tempObj.OrderSource,
                                            StructureElement = tempObj.StructureElement,
                                            ScheduledProd = tempObj.ScheduledProd,
                                            OrderType = lProcess.OrderType,
                                            InvRemarks = lProcess.InvRemarks,
                                            FabricateESM = false,
                                            UserName = lProcess.ProcessedBy,
                                            UserType = lUserType,
                                            IsGreensteel = "N"
                                        };

                                        dynamic submitResult = await lprocess.SubmitProcess(submitProcess);
                                        var jsonSubmit = (JsonResult)submitResult;

                                        // Convert the Value to JSON, then deserialize into dynamic
                                        dynamic submitResponse = JsonConvert.DeserializeObject<dynamic>(
                                            JsonConvert.SerializeObject(jsonSubmit.Value)
                                        );
                                        bool submitSuccess = (bool)submitResponse.success;
                                        string submitMessage = submitResponse.emessage;

                                        if (submitSuccess)
                                        {
                                            await LogError_CutoverActivity(tempObj.JobID, 1, "Submit Successful"); // Submit Successful
                                            cutoverResult = true;
                                        }
                                        else
                                        {
                                            // NOTE: Update an error log that Order not submitted.
                                            await LogError_CutoverActivity(tempObj.JobID, 4, submitMessage);
                                        }
                                    }
                                    else
                                    {
                                        // NOTE: Update an error log that contract not available.
                                        await LogError_CutoverActivity(tempObj.JobID, 5, "contract not available");
                                    }
                                }
                                else
                                {
                                    // NOTE: Update an error log that contract not available.
                                    await LogError_CutoverActivity(tempObj.JobID, 6, "contract not available");
                                }

                                #endregion
                            }
                            else
                            {
                                // NOTE: Update an error log that Cust/Proj not updated properly.
                                await LogError_CutoverActivity(tempObj.JobID, 7, "Cust/Proj not updated properly");
                            }

                            // step 5: update remarks from cutover api
                        }
                        else
                        {
                            // NOTE: Update an error log that Error in Order Withdraw.
                            await LogError_CutoverActivity(tempObj.JobID, 8, "Error in Order Withdraw");
                        }
                    }
                }

                #region Call the SP to get the orders to Auto-Release
                if(cutoverResult == true && CaseScenario == 5)
                {
                    Insert_AutoReleaseOrdersData();
                }
                #endregion

                // ✅ Return as JSON response
                return Ok(new
                {
                    result = cutoverResult
                });
            }
            catch (Exception ex)
            {
                // Return error as 500 response if something fails
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
            finally
            {
                // Always close the connection (even on error)
                lprocess.CloseNDSConnection(ref cnNDS);
            }
        }


        [HttpPost]
        [Route("/LogError_CutoverActivity")]
        public async Task<ActionResult> LogError_CutoverActivity(int orderNumber, int statusCode, string remarks)
        {
            ProcessController lprocess = new ProcessController();
            var cnNDS = new SqlConnection();
            try
            {
                lprocess.OpenNDSConnection(ref cnNDS);

                if (cnNDS.State == ConnectionState.Open)
                {

                    var sql = $@"
                    UPDATE OrderUpdateCase2Custover
                    SET Status = {statusCode} , Remark = '{remarks}'
                    WHERE OrderNumber = {orderNumber} ";

                    using var cmd = new SqlCommand(sql, cnNDS);

                    var rowsAffected = await cmd.ExecuteNonQueryAsync();

                    lprocess.CloseNDSConnection(ref cnNDS);
                    cnNDS = null;
                    if (rowsAffected > 0)
                        return Ok(new { Success = true, Message = "Status updated successfully." });
                    else
                        return NotFound(new { Success = false, Message = "Order not found." });
                }
                return Ok(new { Success = false, Message = "Connection not established." });
            }
            catch (Exception ex)
            {
                // Log the exception here
                return StatusCode(500, new { Success = false, Message = "Internal server error", Error = ex.Message });
            }
            finally
            {
                if (cnNDS != null)
                {
                    lprocess.CloseNDSConnection(ref cnNDS);
                }
            }
        }

        [HttpGet]
        [Route("/GetIncomingUpdateData/{orderNumber}")]
        public async Task<IActionResult> GetIncomingUpdateData(List<string> orderNumber)
        {
            bool lResult = false;
            for (int i = 0; i < orderNumber.Count; i++)
            {
                if (_OrderRepository == null) throw new Exception("_OrderRepository is null");
                if (orderNumber == null) throw new Exception("orderNumber is null");
                if (i >= orderNumber.Count) throw new Exception("Index out of range");

                var response = await _OrderRepository.GetUpdateIncomingData(orderNumber[i]);

                lResult = response;
                if (response == false)
                {
                    //log the order number in temp table
                }
            }
            return Ok(new { result = lResult, response = "success" });

        }


        [HttpGet]
        [Route("/getIncomingRD_CutOver")]
        public async Task<List<string>> getIncomingRDCutOverAsync(string OrderStatus, bool Forecast, string UserName)
        {
            UserAccessController lUa = new UserAccessController();
            ProcessController lprocess = new ProcessController();
            var lUserName = UserName;//"Vishal.Wani@natsteel.com.sg";
            var lUserType = UserName;//"Vishal.Wani@natsteel.com.sg";
            var lGroupName = lUa.getGroupName(lUserType);
            ViewBag.UserType = lUserType;
            lUa = null;

            string lSQL = "";
            var lDa = new SqlDataAdapter();
            var lCmd = new SqlCommand();
            var lDs = new DataSet();
            var lNDSCon = new SqlConnection();

            var content = new List<JobAdviceListModels>();
            var lCancelledDate = DateTime.Now.AddDays(-30);
            var lDate = DateTime.Now.AddDays(-3);

            if (OrderStatus == "CREATING")
            {
                lSQL = "SELECT " +
                "S.ProductType as ProdType, " +
                //"S.AdditionalRemark as AdditionalRemark, " +
                "P.CustomerCode, " +
                "P.ProjectCode, " +
                "(SELECT MAX(cust.CustomerName) FROM OESCustomerMaster cust " +
                "WHERE cust.CustomerCode = P.CustomerCode) as CustomerName, " +
                "(SELECT MAX(proj.ProjectTitle) FROM OESProjectList proj " +
                "WHERE proj.ProjectCode = P.ProjectCode) as ProjectTitle, " +
                "P.OrderNumber, " +
                "S.PONumber, " +
                "S.PODate, " +
                "S.RequiredDate, " +
                "'' as ProjectStage, " +
                "p.Remarks, " +
                "S.OrderStatus, " +
                "CASE WHEN S.ProductType = 'CAB' THEN " +
                "(SELECT isNull(TotalCABWeight,0)/1000 FROM dbo.OESJobAdvice " +
                "WHERE CustomerCode = P.CustomerCode " +
                "AND ProjectCode = P.ProjectCode " +
                "AND JobID = S.CABJobID) " +
                "ELSE 0 END as TotalCABWeight, " +
                "CASE WHEN S.ProductType = 'CAB' THEN " +
                "(SELECT IsNull(TotalSTDWeight,0)/1000 FROM dbo.OESJobAdvice " +
                "WHERE CustomerCode = P.CustomerCode " +
                "AND ProjectCode = P.ProjectCode " +
                "AND JobID = S.CABJobID) " +
                "ELSE S.TotalPCs END as TotalSTDWeight, " +
                "isNull(S.TotalWeight,0)/1000, " +
                "S.TransportMode as TransportLimit, " +
                "S.TransportMode as TransportMode, " +
                "S.UpdateDate, " +
                "isNULL(P.WBS1, '') as WBS1, " +
                "isNULL(P.WBS2, '') as WBS2, " +
                "isNULL(P.WBS3, '') as WBS3, " +
                "CASE WHEN S.ProductType = 'BPC' THEN " +
                "isNull(STUFF((SELECT  ',' + sor_no " +
                "FROM dbo.OESBPCDetailsProc " +
                "WHERE CustomerCode = p.CustomerCode " +
                "AND ProjectCode = p.ProjectCode " +
                "AND JobID = S.BPCJobID " +
                "GROUP BY sor_no " +
                "ORDER BY sor_no " +
                "FOR XML PATH('')), 1, 1, ''),'') " +
                "ELSE S.SAPSOR END as SAPSONo, " +
                "P.UpdateBy, " +
                "(CASE WHEN S.CABJobID > 0 THEN " +
                "(CASE WHEN " +
                "(SELECT isNull(sum(BarTotalQty), 0) FROM dbo.OESOrderDetails " +
                " WHERE CustomerCode = P.CustomerCode " +
                "AND ProjectCode = P.ProjectCode " +
                "AND JobID = S.CABJobID " +
                "AND Cancelled is null " +
                "AND BarCAB = '1' " +
                "AND (BarShapeCode like 'C%' " +
                "OR BarShapeCode like 'P%' " +
                "OR BarShapeCode like 'N%' " +
                "OR BarShapeCode like 'S%' " +
                "OR BarShapeCode like 'H%' " +
                "OR BarShapeCode like 'I%' " +
                "OR BarShapeCode like 'J%' " +
                "OR BarShapeCode like 'K%' " +
                "OR BarShapeCode like 'L%')) > 0 AND " +
                "(SELECT isNull(sum(BarTotalQty), 0) FROM dbo.OESOrderDetails " +
                "WHERE CustomerCode = P.CustomerCode " +
                "AND ProjectCode = P.ProjectCode " +
                "AND JobID = S.CABJobID " +
                "AND Cancelled is null " +
                "AND BarCAB = '1' " +
                "AND BarShapeCode not like 'C%' " +
                "AND BarShapeCode not like 'P%' " +
                "AND BarShapeCode not like 'N%' " +
                "AND BarShapeCode not like 'S%' " +
                "AND BarShapeCode not like 'H%' " +
                "AND BarShapeCode not like 'I%' " +
                "AND BarShapeCode not like 'J%' " +
                "AND BarShapeCode not like 'K%' " +
                "AND BarShapeCode not like 'L%') > 0 " +
                "THEN 'CAB/COUPLER' " +
                "WHEN (SELECT isNull(sum(BarTotalQty), 0) FROM dbo.OESOrderDetails " +
                "WHERE CustomerCode = P.CustomerCode " +
                "AND ProjectCode = P.ProjectCode " +
                "AND JobID = S.CABJobID " +
                "AND Cancelled is null " +
                "AND BarCAB = '1' " +
                "AND(BarShapeCode like 'C%' " +
                "OR BarShapeCode like 'P%' " +
                "OR BarShapeCode like 'N%' " +
                "OR BarShapeCode like 'S%' " +
                "OR BarShapeCode like 'H%' " +
                "OR BarShapeCode like 'I%' " +
                "OR BarShapeCode like 'J%' " +
                "OR BarShapeCode like 'K%' " +
                "OR BarShapeCode like 'L%')) > 0 " +
                "THEN 'COUPLER' " +
                "WHEN (SELECT isNull(sum(BarTotalQty), 0) FROM dbo.OESOrderDetails " +
                "WHERE CustomerCode = P.CustomerCode " +
                "AND ProjectCode = P.ProjectCode " +
                "AND JobID = S.CABJobID " +
                "AND Cancelled is null " +
                "AND BarCAB = '1') > 0 " +
                "THEN 'CAB' " +
                "WHEN (SELECT isNull(sum(BarTotalQty), 0) FROM dbo.OESOrderDetails " +
                "WHERE CustomerCode = P.CustomerCode " +
                "AND ProjectCode = P.ProjectCode " +
                "AND JobID = S.CABJobID " +
                "AND Cancelled is null " +
                "AND BarSTD = '1') > 0 " +
                "THEN 'STANDARD-BAR' " +
                "ELSE S.ProductType " +
                "END ) " +

                "WHEN S.StdBarsJobID > 0 THEN 'STANDARD-BAR' " +
                "WHEN S.StdMESHJobID > 0 THEN 'STANDARD-MESH' " +
                "WHEN S.CoilProdJobID > 0 THEN " +
                "(SELECT MAX(ProdType) " +
                "FROM dbo.OESStdProdDetails " +
                "WHERE CustomerCode = p.CustomerCode " +
                "AND ProjectCode = p.ProjectCode " +
                "and JobID = S.CoilProdJobID ) " +
                "ELSE S.ProductType " +
                "END " +
                ") as ProdTypeDis, " +
                "DeliveryAddress, " +
                "SiteEngr_Name, " +
                "SiteEngr_HP, " +
                "Scheduler_Name, " +
                "Scheduler_HP, " +
                "CASE WHEN S.StructureElement = 'NONWBS' THEN '' ELSE S.StructureElement END as SEDis, " +
                "'UX' as OrderSource, " +
                "S.StructureElement, " +
                "S.ScheduledProd, " +
                "(SELECT ProjectIncharge FROM dbo.OESProjectIncharges WHERE CustomerCode = P.CustomerCode AND ProjectCode = P.ProjectCode) as ProjectIncharge, " +
                "(SELECT DetailingIncharge FROM dbo.OESProjectIncharges WHERE CustomerCode = P.CustomerCode AND ProjectCode = P.ProjectCode) as DetailingIncharge, " +
                "isNull(STUFF((SELECT  ',' + BBSNO " +
                "FROM dbo.OESBBS " +
                "WHERE CustomerCode = p.CustomerCode " +
                "AND ProjectCode = p.ProjectCode " +
                "AND JobID = S.CABJobID " +
                "ORDER BY BBSNO " +
                "FOR XML PATH('')), 1, 1, ''),'') AS BBSNo, " +
                "S.PostHeaderID, " +
                "S.OrigReqDate, " +
                "(SELECT COUNT(DrawingID) " +
                "FROM dbo.OESDrawingsOrder " +
                "WHERE OrderNumber = S.OrderNumber " +
                "AND StructureElement = S.StructureElement " +
                "AND ProductType = S.ProductType " +
                "AND ScheduledProd = S.ScheduledProd) as AttachedNO, " +
                "isNull(STUFF((SELECT  ',' + UpdateBy " +
                "FROM dbo.OESOrderDetails " +
                "WHERE CustomerCode = p.CustomerCode " +
                "AND ProjectCode = p.ProjectCode " +
                "AND JobID = S.CABJobID " +
                "GROUP BY UpdateBy " +
                "ORDER BY UpdateBy " +
                "FOR XML PATH('')), 1, 1, ''),'') AS UpdateBy, " +
                "(SELECT isNull(MAX(PaymentType),'') FROM dbo.OESProject " +
                "WHERE CustomerCode = P.CustomerCode " +
                "AND ProjectCode = P.ProjectCode) as OrderType, " +
                "S.ProcessDate, " +
                "S.AdditionalRemark " +
                "FROM dbo.OESProjOrdersSE S, " +
                "dbo.OESProjOrder P " +
                "WHERE S.OrderNumber = P.OrderNumber " +
                "AND ((P.OrderStatus = 'Created*' " +
                "AND S.OrderStatus = 'Created*') " +
                "OR (P.OrderStatus = 'Submitted*' " +
                "AND S.OrderStatus = 'Submitted*')) " +
                //"and s.UpdateDate >'2024-03-08 00:00:00.000' " +
                "Order by 12 desc, 4, 5, 9 desc ";

            }
            else if (OrderStatus == "DETAILING")
            {
                string lMJ = "";
                if (lUserType == "MJ")
                {
                    var lUserSName = lUserName.Trim();
                    if (lUserSName.IndexOf("@") > 0)
                    {
                        lUserSName = lUserSName.Substring(0, lUserSName.IndexOf("@"));
                    }

                    lMJ = "AND P.ProjectCode in ( " +
                    "SELECT ProjectCode FROM dbo.OESProjectIncharges " +
                    "WHERE ((',' + Ltrim(RTRIM(DetailingIncharge)) + ',') LIKE '%," + lUserSName + ",%' " +
                    "OR (',' + Ltrim(RTRIM(DetailingIncharge)) + ',') LIKE '%," + lUserSName + " ,%' " +
                    "OR (',' + Ltrim(RTRIM(DetailingIncharge)) + ',') LIKE '%, " + lUserSName + ",%' " +
                    "OR (',' + Ltrim(RTRIM(DetailingIncharge)) + ',') LIKE '%, " + lUserSName + " ,%' " +
                    "OR (';' + Ltrim(RTRIM(DetailingIncharge)) + ';') LIKE '%;" + lUserSName + ";%' " +
                    "OR (';' + Ltrim(RTRIM(DetailingIncharge)) + ';') LIKE '%;" + lUserSName + " ;%' " +
                    "OR (';' + Ltrim(RTRIM(DetailingIncharge)) + ';') LIKE '%; " + lUserSName + ";%' " +
                    "OR (';' + Ltrim(RTRIM(DetailingIncharge)) + ';') LIKE '%; " + lUserSName + " ;%')) ";
                }

                lSQL = "SELECT " +
                "S.ProductType as ProdType, " +
                // "S.AdditionalRemark as AdditionalRemark, " +
                "P.CustomerCode, " +
                "P.ProjectCode, " +
                "(SELECT MAX(cust.CustomerName) FROM OESCustomerMaster cust " +
                "WHERE cust.CustomerCode = P.CustomerCode) as CustomerName, " +
                "(SELECT MAX(proj.ProjectTitle) FROM OESProjectList proj " +
                "WHERE proj.ProjectCode = P.ProjectCode) as ProjectTitle, " +
                "P.OrderNumber, " +
                "S.PONumber, " +
                "S.PODate, " +
                "S.RequiredDate, " +
                "'' as ProjectStage, " +
                "p.Remarks, " +
                "S.OrderStatus, " +
                "CASE WHEN S.ProductType = 'CAB' THEN " +
                "(SELECT isNull(TotalCABWeight,0)/1000 FROM dbo.OESJobAdvice " +
                "WHERE CustomerCode = P.CustomerCode " +
                "AND ProjectCode = P.ProjectCode " +
                "AND JobID = S.CABJobID) " +
                "ELSE 0 END as TotalCABWeight, " +
                "CASE WHEN S.ProductType = 'CAB' THEN " +
                "(SELECT IsNull(TotalSTDWeight,0)/1000 FROM dbo.OESJobAdvice " +
                "WHERE CustomerCode = P.CustomerCode " +
                "AND ProjectCode = P.ProjectCode " +
                "AND JobID = S.CABJobID) " +
                "ELSE S.TotalPCs END as TotalSTDWeight, " +
                "isNull(S.TotalWeight,0)/1000, " +
                "S.TransportMode as TransportLimit, " +
                "S.TransportMode as TransportMode, " +
                "S.UpdateDate, " +
                "isNULL(P.WBS1, '') as WBS1, " +
                "isNULL(P.WBS2, '') as WBS2, " +
                "isNULL(P.WBS3, '') as WBS3, " +
                "S.SAPSOR as SAPSONo, " +
                "P.UpdateBy, " +
                "(CASE WHEN S.CABJobID > 0 THEN " +
                "(CASE WHEN " +
                "(SELECT isNull(sum(BarTotalQty), 0) FROM dbo.OESOrderDetails " +
                " WHERE CustomerCode = P.CustomerCode " +
                "AND ProjectCode = P.ProjectCode " +
                "AND JobID = S.CABJobID " +
                "AND Cancelled is null " +
                "AND BarCAB = '1' " +
                "AND(BarShapeCode like 'C%' " +
                "OR BarShapeCode like 'P%' " +
                "OR BarShapeCode like 'N%' " +
                "OR BarShapeCode like 'S%' " +
                "OR BarShapeCode like 'H%' " +
                "OR BarShapeCode like 'I%' " +
                "OR BarShapeCode like 'J%' " +
                "OR BarShapeCode like 'K%' " +
                "OR BarShapeCode like 'L%')) > 0 AND " +
                "(SELECT isNull(sum(BarTotalQty), 0) FROM dbo.OESOrderDetails " +
                "WHERE CustomerCode = P.CustomerCode " +
                "AND ProjectCode = P.ProjectCode " +
                "AND JobID = S.CABJobID " +
                "AND Cancelled is null " +
                "AND BarCAB = '1' " +
                "AND BarShapeCode not like 'C%' " +
                "AND BarShapeCode not like 'P%' " +
                "AND BarShapeCode not like 'N%' " +
                "AND BarShapeCode not like 'S%' " +
                "AND BarShapeCode not like 'H%' " +
                "AND BarShapeCode not like 'I%' " +
                "AND BarShapeCode not like 'J%' " +
                "AND BarShapeCode not like 'K%' " +
                "AND BarShapeCode not like 'L%') > 0 " +
                "THEN 'CAB/COUPLER' " +
                "WHEN (SELECT isNull(sum(BarTotalQty), 0) FROM dbo.OESOrderDetails " +
                "WHERE CustomerCode = P.CustomerCode " +
                "AND ProjectCode = P.ProjectCode " +
                "AND JobID = S.CABJobID " +
                "AND Cancelled is null " +
                "AND BarCAB = '1' " +
                "AND(BarShapeCode like 'C%' " +
                "OR BarShapeCode like 'P%' " +
                "OR BarShapeCode like 'N%' " +
                "OR BarShapeCode like 'S%' " +
                "OR BarShapeCode like 'H%' " +
                "OR BarShapeCode like 'I%' " +
                "OR BarShapeCode like 'J%' " +
                "OR BarShapeCode like 'K%' " +
                "OR BarShapeCode like 'L%')) > 0 " +
                "THEN 'COUPLER' " +
                "WHEN (SELECT isNull(sum(BarTotalQty), 0) FROM dbo.OESOrderDetails " +
                "WHERE CustomerCode = P.CustomerCode " +
                "AND ProjectCode = P.ProjectCode " +
                "AND JobID = S.CABJobID " +
                "AND Cancelled is null " +
                "AND BarCAB = '1') > 0 " +
                "THEN 'CAB' " +
                "WHEN (SELECT isNull(sum(BarTotalQty), 0) FROM dbo.OESOrderDetails " +
                "WHERE CustomerCode = P.CustomerCode " +
                "AND ProjectCode = P.ProjectCode " +
                "AND JobID = S.CABJobID " +
                "AND Cancelled is null " +
                "AND BarSTD = '1') > 0 " +
                "THEN 'STANDARD-BAR' " +
                "ELSE S.ProductType " +
                "END ) " +

                "WHEN S.StdBarsJobID > 0 THEN 'STANDARD-BAR' " +
                "WHEN S.StdMESHJobID > 0 THEN 'STANDARD-MESH' " +
                "WHEN S.CoilProdJobID > 0 THEN " +
                "(SELECT MAX(ProdType) " +
                "FROM dbo.OESStdProdDetails " +
                "WHERE CustomerCode = p.CustomerCode " +
                "AND ProjectCode = p.ProjectCode " +
                "and JobID = S.CoilProdJobID ) " +
                "ELSE S.ProductType " +
                "END " +
                ") as ProdTypeDis, " +
                "DeliveryAddress, " +
                "SiteEngr_Name, " +
                "SiteEngr_HP, " +
                "Scheduler_Name, " +
                "Scheduler_HP, " +
                "CASE WHEN S.StructureElement = 'NONWBS' THEN '' ELSE S.StructureElement END as SEDis, " +
                "'UX' as OrderSource, " +
                "S.StructureElement, " +
                "S.ScheduledProd, " +
                "(SELECT ProjectIncharge FROM dbo.OESProjectIncharges WHERE CustomerCode = P.CustomerCode AND ProjectCode = P.ProjectCode) as ProjectIncharge, " +
                "(SELECT DetailingIncharge FROM dbo.OESProjectIncharges WHERE CustomerCode = P.CustomerCode AND ProjectCode = P.ProjectCode) as DetailingIncharge, " +
                "isNull(STUFF((SELECT  ',' + BBSNO " +
                "FROM dbo.OESBBS " +
                "WHERE CustomerCode = p.CustomerCode " +
                "AND ProjectCode = p.ProjectCode " +
                "AND JobID = S.CABJobID " +
                "ORDER BY BBSNO " +
                "FOR XML PATH('')), 1, 1, ''),'') AS BBSNo, " +
                "S.PostHeaderID, " +
                "S.OrigReqDate, " +
                "(SELECT COUNT(DrawingID) " +
                "FROM dbo.OESDrawingsOrder " +
                "WHERE OrderNumber = S.OrderNumber " +
                "AND StructureElement = S.StructureElement " +
                "AND ProductType = S.ProductType " +
                "AND ScheduledProd = S.ScheduledProd) as AttachedNO, " +
                "'' AS UpdateBy, " +
                "(SELECT isNull(MAX(PaymentType),'') FROM dbo.OESProject " +
                "WHERE CustomerCode = P.CustomerCode " +
                "AND ProjectCode = P.ProjectCode) as OrderType, " +
                "S.ProcessDate, " +

"S.AdditionalRemark " +
"FROM dbo.OESProjOrdersSE S, " +
                "dbo.OESProjOrder P " +
                "WHERE S.OrderNumber = P.OrderNumber " +
                //"AND S.ScheduledProd = 'Y' " +
                "AND (S.PostHeaderID > 0 ) " +
                "AND NOT EXISTS( SELECT intReleaseId FROM dbo.BBSReleaseDetails " +
                "WHERE intPostHeaderid = S.PostHeaderID AND tntStatusId = 12) " +
                "AND S.OrderStatus = 'Reviewed' " + lMJ +
                //"and s.UpdateDate >'2024-03-08 00:00:00.000' " +
                "Order by 12 desc, 4, 5, 9 desc ";
            }
            else
            {
                string lStatus1 = "Created*";   //Created* (UX)
                string lStatus2 = "Submitted";  //Submitted
                string lStatus3 = "Submitted*";      //Cancelled
                string lStatus4 = "Processed";  //Processed (OES)
                string lStatus5 = "Reviewed";   //Reviewed (UX)
                string lStatus6 = "Production";   //Production (UX)
                string lCond = "";
                string lCondUX = "";
                if (OrderStatus == "INCOMING")
                {
                    lStatus1 = "DUMMY";
                    lStatus2 = "Submitted";
                    lStatus3 = "DUMMY";
                    lStatus4 = "DUMMY";
                    lStatus5 = "DUMMY";
                    lStatus6 = "DUMMY";

                    lCond = "AND p.OrderStatus = 'Submitted' ";
                    lCondUX = "AND S.OrderStatus = 'Submitted' ";
                }
                else if (OrderStatus == "PROCESSING")
                {
                    string lMJ = "";
                    if (lUserType == "MJ")
                    {
                        var lUserSName = lUserName.Trim();
                        if (lUserSName.IndexOf("@") > 0)
                        {
                            lUserSName = lUserSName.Substring(0, lUserSName.IndexOf("@"));
                        }

                        lMJ = "AND P.ProjectCode in ( " +
                        "SELECT ProjectCode FROM dbo.OESProjectIncharges " +
                        "WHERE ((',' + Ltrim(RTRIM(DetailingIncharge)) + ',') LIKE '%," + lUserSName + ",%' " +
                        "OR (',' + Ltrim(RTRIM(DetailingIncharge)) + ',') LIKE '%," + lUserSName + " ,%' " +
                        "OR (',' + Ltrim(RTRIM(DetailingIncharge)) + ',') LIKE '%, " + lUserSName + ",%' " +
                        "OR (',' + Ltrim(RTRIM(DetailingIncharge)) + ',') LIKE '%, " + lUserSName + " ,%' " +
                        "OR (';' + Ltrim(RTRIM(DetailingIncharge)) + ';') LIKE '%;" + lUserSName + ";%' " +
                        "OR (';' + Ltrim(RTRIM(DetailingIncharge)) + ';') LIKE '%;" + lUserSName + " ;%' " +
                        "OR (';' + Ltrim(RTRIM(DetailingIncharge)) + ';') LIKE '%; " + lUserSName + ";%' " +
                        "OR (';' + Ltrim(RTRIM(DetailingIncharge)) + ';') LIKE '%; " + lUserSName + " ;%')) ";
                    }

                    lStatus1 = "DUMMY";
                    lStatus2 = "DUMMY";
                    lStatus3 = "DUMMY";
                    lStatus4 = "Production";
                    lStatus5 = "Reviewed";
                    lStatus6 = "DUMMY";

                    lCond = "AND ((p.OrderStatus = '" + lStatus4 + "' " +
                    "OR p.OrderStatus = '" + lStatus5 + "') " +
                    "AND S.ProcessDate >= '2020-01-01' " +
                    "AND S.ProcessDate >= '" + lDate.ToString("yyyy-MM-dd") + "' " +
                    ") ";

                    //lCondUX = "AND ((S.OrderStatus = '" + lStatus4 + "' " +
                    //"OR S.OrderStatus = '" + lStatus5 + "') " +
                    //"AND (p.UpdateDate >= '" + lDate.ToString("yyyy-MM-dd") + "' " +
                    //"AND (S.PostHeaderID = 0 " +
                    //"OR (S.PostHeaderID > 0 " +
                    //"AND EXISTS ( SELECT intReleaseId FROM dbo.BBSReleaseDetails " +
                    //"WHERE intPostHeaderid = S.PostHeaderID AND tntStatusId = 12 ) ) ) " +
                    //") ";

                    lCondUX = "AND ((S.OrderStatus = '" + lStatus4 + "' " +
                    "OR S.OrderStatus = '" + lStatus5 + "') " +
                    "AND ((S.ProcessDate >= '" + lDate.ToString("yyyy-MM-dd") + "' " +
                    "AND S.PostHeaderID = 0 ) " +
                    "OR (S.PostHeaderID > 0 " +
                    "AND EXISTS ( SELECT intReleaseId FROM dbo.BBSReleaseDetails " +
                    "WHERE intPostHeaderid = S.PostHeaderID AND tntStatusId = 12 " +
                    "AND datReleasedDate >= '" + lDate.ToString("yyyy-MM-dd") + "' ) ) ) " +
                    ") " + lMJ + " ";
                }
                else if (OrderStatus == "PRODUCTION")
                {
                    lStatus1 = "DUMMY";
                    lStatus2 = "DUMMY";
                    lStatus3 = "DUMMY";
                    lStatus4 = "DUMMY";
                    lStatus5 = "DUMMY";
                    lStatus6 = "Production";

                    lCond = "AND (p.OrderStatus = '" + lStatus6 + "' " +
                    "AND p.UpdateDate >= '" + lDate.ToString("yyyy-MM-dd") + "') ";

                    lCondUX = "AND (S.OrderStatus = '" + lStatus6 + "' " +
                    //"AND p.UpdateDate >= '" + lDate.ToString("yyyy-MM-dd") + "' " +
                    ") ";
                }
                else if (OrderStatus == "CANCELLED")
                {
                    lStatus1 = "DUMMY";
                    lStatus2 = "DUMMY";
                    lStatus3 = "Cancelled";
                    lStatus4 = "DUMMY";
                    lStatus5 = "DUMMY";
                    lStatus6 = "DUMMY";

                    lCond = "AND (p.OrderStatus = 'Cancelled' " +
                    "AND p.UpdateDate >= '" + lCancelledDate.ToString("yyyy-MM-dd") + "') ";

                    lCondUX = "AND (S.OrderStatus = 'Cancelled' " +
                    "AND p.UpdateDate >= '" + lCancelledDate.ToString("yyyy-MM-dd") + "') ";
                }
                else if (OrderStatus == "ALL")
                {
                    lStatus1 = "Created*";
                    lStatus2 = "Submitted";
                    lStatus3 = "Submitted*";
                    lStatus4 = "Processed";
                    lStatus5 = "Reviewed";
                    lStatus6 = "Production";
                    //lStatus6 = "DUMMY";
                    lCond = "AND ( (p.OrderStatus = '" + lStatus1 + "' " +
                    "OR p.OrderStatus = '" + lStatus2 + "' " +
                    //"OR p.OrderStatus = '" + lStatus3 + "' " +
                    "OR p.OrderStatus = '" + lStatus4 + "' " +
                    "OR p.OrderStatus = '" + lStatus5 + "' " +
                    "OR p.OrderStatus = '" + lStatus6 + "') " +
                    "AND p.UpdateDate >= '2020-01-01' " +
                    ") ";

                    lCondUX = "AND (S.OrderStatus = '" + lStatus1 + "' " +
                    "OR S.OrderStatus = '" + lStatus2 + "' " +
                    "OR S.OrderStatus = '" + lStatus3 + "' " +
                    "OR S.OrderStatus = '" + lStatus4 + "' " +
                    "OR S.OrderStatus = '" + lStatus5 + "' " +
                    "OR S.OrderStatus = '" + lStatus6 + "') ";
                }

                lSQL = "SELECT " +
                     "S.ProductType as ProdType, " +
                     "P.CustomerCode, " +
                     "P.ProjectCode, " +
                     "(SELECT MAX(cust.CustomerName) FROM OESCustomerMaster cust " +
                     "WHERE cust.CustomerCode = P.CustomerCode) as CustomerName, " +
                     "(SELECT MAX(proj.ProjectTitle) FROM OESProjectList proj " +
                     "WHERE proj.ProjectCode = P.ProjectCode) as ProjectTitle, " +
                     "P.OrderNumber, " +
                     "S.PONumber, " +
                     "S.PODate, " +
                     "S.RequiredDate, " +
                     "'' as ProjectStage, " +
                     "p.Remarks, " +
                     "S.OrderStatus, " +
                     "CASE WHEN S.ProductType = 'CAB' THEN " +
                     "(SELECT isNull(TotalCABWeight,0)/1000 FROM dbo.OESJobAdvice " +
                     "WHERE CustomerCode = P.CustomerCode " +
                     "AND ProjectCode = P.ProjectCode " +
                     "AND JobID = S.CABJobID) " +
                     "ELSE 0 END as TotalCABWeight, " +
                     "CASE WHEN S.ProductType = 'CAB' THEN " +
                     "(SELECT isNull(TotalSTDWeight,0)/1000 FROM dbo.OESJobAdvice " +
                     "WHERE CustomerCode = P.CustomerCode " +
                     "AND ProjectCode = P.ProjectCode " +
                     "AND JobID = S.CABJobID) " +
                     "ELSE S.TotalPCs END as TotalSTDWeight, " +
                     "isNull(S.TotalWeight,0)/1000, " +
                     "S.TransportMode as TransportLimit, " +
                     "S.TransportMode as TransportMode, " +
                     "S.UpdateDate, " +
                     "isNULL(P.WBS1, '') as WBS1, " +
                     "isNULL(P.WBS2, '') as WBS2, " +
                     "isNULL(P.WBS3, '') as WBS3, " +
                     "CASE WHEN S.ProductType = 'BPC' AND S.ScheduledProd <> 'Y' THEN " +
                     "isNull(STUFF((SELECT  ',' + sor_no " +
                     "FROM dbo.OESBPCDetailsProc " +
                     "WHERE CustomerCode = p.CustomerCode " +
                     "AND ProjectCode = p.ProjectCode " +
                     "AND JobID = S.BPCJobID " +
                     "GROUP BY sor_no " +
                     "ORDER BY sor_no " +
                     "FOR XML PATH('')), 1, 1, ''),'') " +
                     "ELSE S.SAPSOR END as SAPSONo, " +
                     "P.UpdateBy, " +
                     "(CASE WHEN S.CABJobID > 0 THEN " +
                     "(CASE WHEN " +
                     "(SELECT isNull(sum(BarTotalQty), 0) FROM dbo.OESOrderDetails " +
                     " WHERE CustomerCode = P.CustomerCode " +
                     "AND ProjectCode = P.ProjectCode " +
                     "AND JobID = S.CABJobID " +
                     "AND Cancelled is null " +
                     "AND BarCAB = '1' " +
                     "AND(BarShapeCode like 'C%' " +
                     "OR BarShapeCode like 'P%' " +
                     "OR BarShapeCode like 'N%' " +
                     "OR BarShapeCode like 'S%' " +
                     "OR BarShapeCode like 'H%' " +
                     "OR BarShapeCode like 'I%' " +
                     "OR BarShapeCode like 'J%' " +
                     "OR BarShapeCode like 'K%' " +
                     "OR BarShapeCode like 'L%')) > 0 AND " +
                     "(SELECT isNull(sum(BarTotalQty), 0) FROM dbo.OESOrderDetails " +
                     "WHERE CustomerCode = P.CustomerCode " +
                     "AND ProjectCode = P.ProjectCode " +
                     "AND JobID = S.CABJobID " +
                     "AND Cancelled is null " +
                     "AND BarCAB = '1' " +
                     "AND BarShapeCode not like 'C%' " +
                     "AND BarShapeCode not like 'P%' " +
                     "AND BarShapeCode not like 'N%' " +
                     "AND BarShapeCode not like 'S%' " +
                     "AND BarShapeCode not like 'H%' " +
                     "AND BarShapeCode not like 'I%' " +
                     "AND BarShapeCode not like 'J%' " +
                     "AND BarShapeCode not like 'K%' " +
                     "AND BarShapeCode not like 'L%') > 0 " +
                     "THEN 'CAB/COUPLER' " +
                     "WHEN (SELECT isNull(sum(BarTotalQty), 0) FROM dbo.OESOrderDetails " +
                     "WHERE CustomerCode = P.CustomerCode " +
                     "AND ProjectCode = P.ProjectCode " +
                     "AND JobID = S.CABJobID " +
                     "AND Cancelled is null " +
                     "AND BarCAB = '1' " +
                     "AND(BarShapeCode like 'C%' " +
                     "OR BarShapeCode like 'P%' " +
                     "OR BarShapeCode like 'N%' " +
                     "OR BarShapeCode like 'S%' " +
                     "OR BarShapeCode like 'H%' " +
                     "OR BarShapeCode like 'I%' " +
                     "OR BarShapeCode like 'J%' " +
                     "OR BarShapeCode like 'K%' " +
                     "OR BarShapeCode like 'L%')) > 0 " +
                     "THEN 'COUPLER' " +
                     "WHEN (SELECT isNull(sum(BarTotalQty), 0) FROM dbo.OESOrderDetails " +
                     "WHERE CustomerCode = P.CustomerCode " +
                     "AND ProjectCode = P.ProjectCode " +
                     "AND JobID = S.CABJobID " +
                     "AND Cancelled is null " +
                     "AND BarCAB = '1') > 0 " +
                     "THEN 'CAB' " +
                     "WHEN (SELECT isNull(sum(BarTotalQty), 0) FROM dbo.OESOrderDetails " +
                     "WHERE CustomerCode = P.CustomerCode " +
                     "AND ProjectCode = P.ProjectCode " +
                     "AND JobID = S.CABJobID " +
                     "AND Cancelled is null " +
                     "AND BarSTD = '1') > 0 " +
                     "THEN 'STANDARD-BAR' " +
                     "ELSE S.ProductType " +
                     "END ) " +

                     "WHEN S.StdBarsJobID > 0 THEN 'STANDARD-BAR' " +
                     "WHEN S.StdMESHJobID > 0 THEN 'STANDARD-MESH' " +
                     "WHEN S.CoilProdJobID > 0 THEN " +
                     "(SELECT MAX(ProdType) " +
                     "FROM dbo.OESStdProdDetails " +
                     "WHERE CustomerCode = p.CustomerCode " +
                     "AND ProjectCode = p.ProjectCode " +
                     "and JobID = S.CoilProdJobID ) " +
                     "ELSE S.ProductType " +
                     "END " +
                     ") as ProdTypeDis, " +
                     "DeliveryAddress, " +
                     "SiteEngr_Name, " +
                     "SiteEngr_HP, " +
                     "Scheduler_Name, " +
                     "Scheduler_HP, " +
                     "CASE WHEN S.StructureElement = 'NONWBS' THEN '' ELSE S.StructureElement END as SEDis, " +
                     "'UX' as OrderSource, " +
                     "S.StructureElement, " +
                     "S.ScheduledProd, " +
                     "(SELECT ProjectIncharge FROM dbo.OESProjectIncharges WHERE CustomerCode = P.CustomerCode AND ProjectCode = P.ProjectCode) as ProjectIncharge, " +
                     "(SELECT DetailingIncharge FROM dbo.OESProjectIncharges WHERE CustomerCode = P.CustomerCode AND ProjectCode = P.ProjectCode) as DetailingIncharge, " +
                     "isNull(STUFF((SELECT  ',' + BBSNO " +
                     "FROM dbo.OESBBS " +
                     "WHERE CustomerCode = p.CustomerCode " +
                     "AND ProjectCode = p.ProjectCode " +
                     "AND JobID = S.CABJobID " +
                     "ORDER BY BBSNO " +
                     "FOR XML PATH('')), 1, 1, ''),'') AS BBSNo, " +
                     "S.PostHeaderID, " +
                     "S.OrigReqDate, " +
                     "(SELECT COUNT(DrawingID) " +
                     "FROM dbo.OESDrawingsOrder " +
                     "WHERE OrderNumber = S.OrderNumber " +
                     "AND StructureElement = S.StructureElement " +
                     "AND ProductType = S.ProductType " +
                     "AND ScheduledProd = S.ScheduledProd) as AttachedNO, " +
                     "isNull(STUFF((SELECT  ',' + UpdateBy " +
                     "FROM dbo.OESOrderDetails " +
                     "WHERE CustomerCode = p.CustomerCode " +
                     "AND ProjectCode = p.ProjectCode " +
                     "AND JobID = S.CABJobID " +
                     "GROUP BY UpdateBy " +
                     "ORDER BY UpdateBy " +
                     "FOR XML PATH('')), 1, 1, ''),'') + " +
                     "isNull(STUFF((SELECT  ',' + UpdateBy " +
                     "FROM dbo.OESBPCDetails " +
                     "WHERE CustomerCode = p.CustomerCode " +
                     "AND ProjectCode = p.ProjectCode " +
                     "AND JobID = S.BPCJobID " +
                     "GROUP BY UpdateBy " +
                     "ORDER BY UpdateBy " +
                     "FOR XML PATH('')), 1, 1, ''),'') AS UpdateBy, " +
                     "(SELECT isNull(MAX(PaymentType),'') FROM dbo.OESProject " +
                     "WHERE CustomerCode = P.CustomerCode " +
                     "AND ProjectCode = P.ProjectCode) as OrderType, " +
                     "S.ProcessDate ," +
                     "S.AdditionalRemark " +
                     "FROM dbo.OESProjOrdersSE S, " +
                     "dbo.OESProjOrder P " +
                     "WHERE S.OrderNumber = P.OrderNumber " +
                     lCondUX +
                     "Order by 12 desc, 4, 5, 9 desc, 7, 6 ";


                //"Order by p.OrderStatus desc, cust.CustomerName, proj.ProjectTitle, p.RequiredDate desc, POnumber, OrderNumber ";

            }

            if (lprocess.OpenNDSConnection(ref lNDSCon) == true)
            {
                try
                {
                    lCmd.CommandText = lSQL;
                    lCmd.Connection = lNDSCon;
                    lCmd.CommandType = CommandType.Text;
                    lCmd.CommandTimeout = 1200;
                    lDa = new SqlDataAdapter(lCmd);
                    lDa.Fill(lDs);
                    if (lDs.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < lDs.Tables[0].Rows.Count; i++)
                        {
                            content.Add
                                (new JobAdviceListModels
                                {
                                    ProdType = (string)lDs.Tables[0].Rows[i].ItemArray[0],
                                    CustomerCode = (string)lDs.Tables[0].Rows[i].ItemArray[1],
                                    ProjectCode = (string)lDs.Tables[0].Rows[i].ItemArray[2],
                                    //CustomerName = (string)lDs.Tables[0].Rows[i].ItemArray[3],
                                    CustomerName = DbNullHelper.HandleDbNull(lDs.Tables[0].Rows[i].ItemArray[3], string.Empty),

                                    //ProjectTitle = lDs.Tables[0].Rows[i].ItemArray[4] == DBNull.Value ? string.Empty : (string)lDs.Tables[0].Rows[i].ItemArray[4],
                                    ProjectTitle = lDs.Tables[0].Rows[i].ItemArray[4] == DBNull.Value ? string.Empty : (string)lDs.Tables[0].Rows[i].ItemArray[4],
                                    JobID = (int)lDs.Tables[0].Rows[i].ItemArray[5],
                                    JobIDDis = (int)lDs.Tables[0].Rows[i].ItemArray[5],
                                    //PONumber = lDs.Tables[0].Rows[i].ItemArray[6] == DBNull.Value ? string.Empty : (string)lDs.Tables[0].Rows[i].ItemArray[6],
                                    PONumber = Convert.IsDBNull(lDs.Tables[0].Rows[i].ItemArray[6]) ? string.Empty : (string)lDs.Tables[0].Rows[i].ItemArray[6],
                                    //PODate = lDs.Tables[0].Rows[i].ItemArray[7] == DBNull.Value ? (new DateTime(2020, 1, 1)) : (DateTime)lDs.Tables[0].Rows[i].ItemArray[7],
                                    PODate = Convert.IsDBNull(lDs.Tables[0].Rows[i].ItemArray[7]) ? new DateTime(2020, 1, 1) : (DateTime)lDs.Tables[0].Rows[i].ItemArray[7],
                                    RequiredDate = lDs.Tables[0].Rows[i].ItemArray[8] == DBNull.Value ? (new DateTime(2020, 1, 1)) : (DateTime)lDs.Tables[0].Rows[i].ItemArray[8],
                                    ProjectStage = lDs.Tables[0].Rows[i].ItemArray[9] == DBNull.Value ? string.Empty : (string)lDs.Tables[0].Rows[i].ItemArray[9],
                                    Remarks = lDs.Tables[0].Rows[i].ItemArray[10] == DBNull.Value ? string.Empty : (string)lDs.Tables[0].Rows[i].ItemArray[10],
                                    OrderStatus = lDs.Tables[0].Rows[i].ItemArray[11] == DBNull.Value ? string.Empty : (string)lDs.Tables[0].Rows[i].ItemArray[11],
                                    TotalCABWeight = (decimal)(lDs.Tables[0].Rows[i].ItemArray[12] == DBNull.Value ? (decimal)0 : lDs.Tables[0].Rows[i].ItemArray[12]),
                                    TotalSTDWeight = (decimal)(lDs.Tables[0].Rows[i].ItemArray[13] == DBNull.Value ? (decimal)0 : lDs.Tables[0].Rows[i].ItemArray[13]),
                                    TotalWeight = (decimal)(lDs.Tables[0].Rows[i].ItemArray[14] == DBNull.Value ? (decimal)0 : lDs.Tables[0].Rows[i].ItemArray[14]),
                                    TransportLimit = lDs.Tables[0].Rows[i].ItemArray[15] == DBNull.Value ? string.Empty : (string)lDs.Tables[0].Rows[i].ItemArray[15],
                                    TransportMode = lDs.Tables[0].Rows[i].ItemArray[16] == DBNull.Value ? string.Empty : (string)lDs.Tables[0].Rows[i].ItemArray[16],
                                    UpdateDate = lDs.Tables[0].Rows[i].ItemArray[17] == DBNull.Value ? (new DateTime(2020, 1, 1)) : (DateTime)lDs.Tables[0].Rows[i].ItemArray[17],
                                    WBS1 = lDs.Tables[0].Rows[i].ItemArray[18] == DBNull.Value ? string.Empty : (string)lDs.Tables[0].Rows[i].ItemArray[18],
                                    WBS2 = lDs.Tables[0].Rows[i].ItemArray[19] == DBNull.Value ? string.Empty : (string)lDs.Tables[0].Rows[i].ItemArray[19],
                                    WBS3 = lDs.Tables[0].Rows[i].ItemArray[20] == DBNull.Value ? string.Empty : (string)lDs.Tables[0].Rows[i].ItemArray[20],
                                    SORNo = lDs.Tables[0].Rows[i].ItemArray[21] == DBNull.Value ? string.Empty : (string)lDs.Tables[0].Rows[i].ItemArray[21],
                                    SORNoDis = lDs.Tables[0].Rows[i].ItemArray[21] == DBNull.Value ? string.Empty : (string)lDs.Tables[0].Rows[i].ItemArray[21],
                                    UpdateBy = lDs.Tables[0].Rows[i].ItemArray[22] == DBNull.Value ? string.Empty : (string)lDs.Tables[0].Rows[i].ItemArray[22],
                                    ProdTypeDis = lDs.Tables[0].Rows[i].ItemArray[23] == DBNull.Value ? string.Empty : (string)lDs.Tables[0].Rows[i].ItemArray[23],
                                    DeliveryAddress = lDs.Tables[0].Rows[i].ItemArray[24] == DBNull.Value ? string.Empty : (string)lDs.Tables[0].Rows[i].ItemArray[24],
                                    SiteEngr_Name = lDs.Tables[0].Rows[i].ItemArray[25] == DBNull.Value ? string.Empty : (string)lDs.Tables[0].Rows[i].ItemArray[25],
                                    SiteEngr_HP = lDs.Tables[0].Rows[i].ItemArray[26] == DBNull.Value ? string.Empty : (string)lDs.Tables[0].Rows[i].ItemArray[26],
                                    Scheduler_Name = lDs.Tables[0].Rows[i].ItemArray[27] == DBNull.Value ? string.Empty : (string)lDs.Tables[0].Rows[i].ItemArray[27],
                                    Scheduler_HP = lDs.Tables[0].Rows[i].ItemArray[28] == DBNull.Value ? string.Empty : (string)lDs.Tables[0].Rows[i].ItemArray[28],
                                    StructureElementDis = (lDs.Tables[0].Rows[i].ItemArray[29] == DBNull.Value || (string)lDs.Tables[0].Rows[i].ItemArray[29] == "0" || (string)lDs.Tables[0].Rows[i].ItemArray[29] == "" ? "" : lDs.Tables[0].Rows[i].ItemArray[29].ToString()),
                                    OrderSource = lDs.Tables[0].Rows[i].ItemArray[30] == DBNull.Value ? string.Empty : (string)lDs.Tables[0].Rows[i].ItemArray[30],
                                    StructureElement = lDs.Tables[0].Rows[i].ItemArray[31] == DBNull.Value ? string.Empty : (string)lDs.Tables[0].Rows[i].ItemArray[31],
                                    ScheduledProd = lDs.Tables[0].Rows[i].ItemArray[32] == DBNull.Value ? "N" : (string)lDs.Tables[0].Rows[i].ItemArray[32],
                                    ProjectIncharge = lDs.Tables[0].Rows[i].ItemArray[33] == DBNull.Value ? string.Empty : (string)lDs.Tables[0].Rows[i].ItemArray[33],
                                    DetailingIncharge = lDs.Tables[0].Rows[i].ItemArray[34] == DBNull.Value ? string.Empty : (string)lDs.Tables[0].Rows[i].ItemArray[34],
                                    BBSNo = lDs.Tables[0].Rows[i].ItemArray[35] == DBNull.Value ? string.Empty : (string)lDs.Tables[0].Rows[i].ItemArray[35],
                                    PostHeaderID = lDs.Tables[0].Rows[i].ItemArray[36] == DBNull.Value ? 0 : (Int32)lDs.Tables[0].Rows[i].ItemArray[36],
                                    OrigReqDate = lDs.Tables[0].Rows[i].ItemArray[37] == DBNull.Value ? (lDs.Tables[0].Rows[i].ItemArray[8] == DBNull.Value ? (new DateTime(2020, 1, 1)) : (DateTime)lDs.Tables[0].Rows[i].ItemArray[8]) : (DateTime?)lDs.Tables[0].Rows[i].ItemArray[37],
                                    AttachedNo = (int)lDs.Tables[0].Rows[i].ItemArray[38],
                                    DataEntryBy = lDs.Tables[0].Rows[i].ItemArray[39] == DBNull.Value ? string.Empty : (string)lDs.Tables[0].Rows[i].ItemArray[39],
                                    OrderType = lDs.Tables[0].Rows[i].ItemArray[40] == DBNull.Value ? string.Empty : (string)lDs.Tables[0].Rows[i].ItemArray[40],
                                    ProcessDate = lDs.Tables[0].Rows[i].ItemArray[41] == DBNull.Value ? string.Empty : ((DateTime)lDs.Tables[0].Rows[i].ItemArray[41]).ToString("yyyy-MM-dd"),
                                    //AdditionalRemark = (string)lDs.Tables[42].Rows[i].ItemArray[42],
                                    AdditionalRemark = lDs.Tables[0].Rows[i].ItemArray[42] == DBNull.Value ? string.Empty : (string)lDs.Tables[0].Rows[i].ItemArray[42],

                                    ForecastDate = "",
                                    PlanDelDate = "",
                                    ConfirmedDelDate = "",
                                    SAPSONo = "",
                                    PMDRemarks = "",
                                    PMDRemarksHis = "",
                                    TECHRemarks = "",
                                    TECHRemarksHis = "",
                                    RunNo = ""
                                });
                        }
                    }

                    content = lprocess.AppendRemarks(content, lNDSCon);

                    lprocess.CloseNDSConnection(ref lNDSCon);
                }
                catch (Exception ex)
                {
                    var lErrorMsg = ex.Message;
                    lprocess.SaveErrorMsg(ex.Message, ex.StackTrace);
                }
            }

            lDa = null;
            lCmd = null;
            lDs = null;
            lNDSCon = null;

            var lReturn = lprocess.checkCancelledOrders(content);

            //lReturn = AddProjectIncharge(lReturn);

            lReturn = lprocess.AddConfirmedDelDate(lReturn, Forecast);

            if (lReturn.Count > 0)
            {
                //add spacing for SO/SNo

                for (int i = 0; i < lReturn.Count; i++)
                {
                    lReturn[i].SORNo = lReturn[i].SORNo.Replace(",", ", ");
                    lReturn[i].SAPSONo = lReturn[i].SAPSONo.Replace(",", ", ");
                }
            }

            var finalOrderNumbers = lReturn.Select(x => x.JobID.ToString()).ToList();


            // Process in batches of 50
            int batchSize = 50;
            for (int i = 0; i < finalOrderNumbers.Count; i += batchSize)
            {
                var batch = finalOrderNumbers.Skip(i).Take(batchSize).ToList();

                //Call service method(async)
                await GetIncomingUpdateData(batch);
            }
            return finalOrderNumbers;
        }

        [HttpGet]
        [Route("/Insert_AutoReleaseOrdersData")]
        public async Task<IActionResult> Insert_AutoReleaseOrdersData()
        {
            bool lResult = false;
            
            if (_OrderRepository == null) throw new Exception("_OrderRepository is null");

            var response = await _OrderRepository.InserAutoReleaseData();

            lResult = response;
            if (response == false)
            {
                //log the order number in temp table
            }
            
            return Ok(new { result = lResult, response = "success" });

        }
    }
}
