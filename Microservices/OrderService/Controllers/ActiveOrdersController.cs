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
using System.Net.Http.Headers;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using Microsoft.AspNetCore.Authorization;

//test

namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class ActiveOrdersController : Controller
    {
        private readonly IOrder _OrderRepository;
        private readonly IMapper _mapper;

        private DBContextModels db = new DBContextModels();

        //[HttpGet]
        //[Route("/ActiveOrdersIndex/{rCustomerCode}/{rProjectCode}")]
        //public ActionResult Index(string rCustomerCode, string rProjectCode)
        //{
        //    if (rCustomerCode == null)
        //    {
        //        rCustomerCode = "";
        //    }
        //    rCustomerCode = rCustomerCode.Trim();

        //    if (rProjectCode == null)
        //    {
        //        rProjectCode = "";
        //    }
        //    rProjectCode = rProjectCode.Trim();

        //    UserAccessController lUa = new UserAccessController();
        //    var lUserType = "AD";//lUa.getUserType(User.Identity.GetUserName());//commented by ajit New
        //    var lGroupName = lUa.getGroupName(User.Identity.GetUserName());

        //    ViewBag.UserType = lUserType;

        //    string lUserName = User.Identity.GetUserName();
        //    //if (lUserName.IndexOf("@") > 0)
        //    //{
        //    //    lUserName = lUserName.Substring(0, lUserName.IndexOf("@"));
        //    //}
        //    ViewBag.UserName = lUserName;

        //    lUa = null;

        //    SharedAPIController lBackEnd = new SharedAPIController();

        //    var lCustSelectList = lBackEnd.getCustomerSelectList(rCustomerCode, lUserType, lGroupName);

        //    ViewBag.CustomerSelection = lCustSelectList;

        //    if (rCustomerCode.Length == 0)
        //    {
        //        rCustomerCode = lCustSelectList.First().Value;
        //        if (rCustomerCode == null)
        //        {
        //            rCustomerCode = "";
        //        }
        //    }

        //    var lProjSelectList = lBackEnd.getProjectSelectList(rCustomerCode, rProjectCode, lUserType, lGroupName);
        //    ViewBag.ProjectSelection = lProjSelectList;

        //    if (rProjectCode.Length == 0)
        //    {
        //        rProjectCode = lProjSelectList.First().Value;
        //        if (rProjectCode == null)
        //        {
        //            rProjectCode = "";
        //        }
        //    }

        //    lBackEnd = null;

        //    var lSubmission = "No";
        //    var lEditable = "No";

        //    //get Access right;
        //    if (lUserType == "CU" || lUserType == "CA" || lUserType == "CM")
        //    {
        //        var lAccess = db.UserAccess.Find(User.Identity.Name, rCustomerCode, rProjectCode);
        //        if (lAccess != null)
        //        {
        //            lSubmission = lAccess.OrderSubmission.Trim();
        //            lEditable = lAccess.OrderCreation.Trim();
        //        }
        //    }
        //    else
        //    {
        //        if (lUserType == "TE" || lUserType == "PL" || lUserType == "AD" || lUserType == "PM" || lUserType == "PA" || lUserType == "P1" || lUserType == "P2" || lUserType == "PU")
        //        {
        //            lSubmission = "Yes";
        //            lEditable = "Yes";
        //        }
        //    }

        //    ViewBag.Submission = lSubmission;
        //    ViewBag.Editable = lEditable;

        //    ViewBag.AlertMessage = new List<string>();
        //    //var lSharedPrg = new SharedAPIController();
        //    //ViewBag.AlertMessage = lSharedPrg.getAlertMessage(rCustomerCode, rProjectCode, lUserName, lSubmission, lEditable);
        //    //lSharedPrg = null;

        //    return Ok(lSubmission);
        //}


        //[ValidateAntiForgeryToken]

        [HttpGet]
        [Route("/ActiveIndex/{rCustomerCode}/{rProjectCode}")]
        public ActionResult ActiveIndex(string rCustomerCode, string rProjectCode)
        {
            if (rCustomerCode == null)
            {
                rCustomerCode = "";
            }
            rCustomerCode = rCustomerCode.Trim();

            if (rProjectCode == null)
            {
                rProjectCode = "";
            }
            rProjectCode = rProjectCode.Trim();
            List<RoleCheckDto> lRolecheck = new List<RoleCheckDto>();

            UserAccessController lUa = new UserAccessController();
            var lUserType = lUa.getUserType(User.Identity.GetUserName());
            var lGroupName = lUa.getGroupName(User.Identity.GetUserName());

            ViewBag.UserType = lUserType;

            string lUserName = User.Identity.GetUserName();
            //if (lUserName.IndexOf("@") > 0)
            //{
            //    lUserName = lUserName.Substring(0, lUserName.IndexOf("@"));
            //}
            ViewBag.UserName = lUserName;

            lUa = null;

            SharedAPIController lBackEnd = new SharedAPIController();

            var lCustSelectList = lBackEnd.getCustomerSelectList(rCustomerCode, lUserType, lGroupName);

            ViewBag.CustomerSelection = lCustSelectList;

            if (rCustomerCode.Length == 0)
            {
                rCustomerCode = lCustSelectList.First().Value;
                if (rCustomerCode == null)
                {
                    rCustomerCode = "";
                }
            }

            var lProjSelectList = lBackEnd.getProjectSelectList(rCustomerCode, rProjectCode, lUserType, lGroupName);
            ViewBag.ProjectSelection = lProjSelectList;

            if (rProjectCode.Length == 0)
            {
                rProjectCode = lProjSelectList.First().Value;
                if (rProjectCode == null)
                {
                    rProjectCode = "";
                }
            }

            lBackEnd = null;



            var lSubmission = "No";
            var lEditable = "No";

            //get Access right;
            if (lUserType == "CU" || lUserType == "CA" || lUserType == "CM")
            {
                var lAccess = db.UserAccess.Find(User.Identity.Name, rCustomerCode, rProjectCode);
                if (lAccess != null)
                {
                    lSubmission = lAccess.OrderSubmission.Trim();
                    lEditable = lAccess.OrderCreation.Trim();
                }
            }
            else
            {
                if (lUserType == "TE" || lUserType == "PL" || lUserType == "AD" || lUserType == "PM" || lUserType == "PA" || lUserType == "P1" || lUserType == "P2" || lUserType == "PU")
                {
                    lSubmission = "Yes";
                    lEditable = "Yes";
                }
            }

            ViewBag.Submission = lSubmission;
            ViewBag.Editable = lEditable;



            ViewBag.AlertMessage = new List<string>();
            //var lSharedPrg = new SharedAPIController();
            //ViewBag.AlertMessage = lSharedPrg.getAlertMessage(rCustomerCode, rProjectCode, lUserName, lSubmission, lEditable);
            //lSharedPrg = null;
            lRolecheck.Add(new RoleCheckDto
            {
                Submission = lSubmission,
                Editable = lEditable

            }
                       );
            var result = lRolecheck;
            return Ok(result);
            //return Ok();
        }
        public ActiveOrdersController(IOrder orderService, IMapper mapper)
        {
            _OrderRepository = orderService;
            _mapper = mapper;

        }


        [HttpGet]
        [Route("/GetGridList/{customerCode}/{projectCode}")]
        public async Task<IActionResult> GetGridList(string customerCode, string projectCode)
        {
            IEnumerable<ActiveGridListDto> orderServiceList = await _OrderRepository.GetActiveGridList(customerCode, projectCode);
            var result = _mapper.Map<List<ActiveGridListDto>>(orderServiceList);
            return Ok(result);
        }



        [HttpPost]
        [Route("/getActiveOrders")]
        public async Task<ActionResult> getActiveOrders(ActiveOrdersDto activeOrdersDto)

        //public async Task<ActionResult> getActiveOrders(string CustomerCode, string ProjectCode,
        //               string PONumber, string PODate, string RDate,
        //               string WBS1, string WBS2, string WBS3, bool AllProjects)
        {
            //UpdateStatus(CustomerCode, ProjectCode, User.Identity.GetUserName());

            try
            {
                string CustomerCode = activeOrdersDto.CustomerCode;
                string ProjectCode = activeOrdersDto.ProjectCode;
                List<string> AddressCodes = activeOrdersDto.AddressCode;
                string pUserName = activeOrdersDto.UserName;
                bool AllProjects = activeOrdersDto.AllProjects;
                string lSQL = "";
                //set kookie for customer and project
                string lPODateFrom = "";
                string lPODateTo = "";
                string lRDateFrom = "";
                string lRDateTo = "";

                string lPODateFrom_O = "";
                string lPODateTo_O = "";
                string lRDateFrom_O = "";
                string lRDateTo_O = "";


                //  (PODate == null) PODate = "";
                //if (PODate.Trim().Length == 0 || PODate.IndexOf("to") <= 0)
                //{
                //    lPODateFrom = "2000-01-01 00:00:00";
                //    lPODateTo = "2200-01-01 00:00:00";
                //}
                //else
                //{
                //    lPODateFrom = PODate.Substring(0, PODate.IndexOf("to")).Trim();
                //    lPODateTo = PODate.Substring(PODate.IndexOf("to") + 2).Trim();
                //}
                DateTime lDateV = new DateTime();
                if (DateTime.TryParse(lPODateFrom, out lDateV) != true)
                {
                    lPODateFrom = "2000-01-01 00:00:00";
                }
                if (DateTime.TryParse(lPODateTo, out lDateV) != true)
                {
                    lPODateTo = "2200-01-01 00:00:00";
                }

                //if (RDate == null) RDate = "";
                //if (RDate.Trim().Length == 0 || RDate.IndexOf("to") <= 0)
                //{
                //    lRDateFrom = "2000-01-01 00:00:00";
                //    lRDateTo = "2200-01-01 00:00:00";
                //}
                //else
                //{
                //    lRDateFrom = RDate.Substring(0, RDate.IndexOf("to")).Trim();
                //    lRDateTo = RDate.Substring(RDate.IndexOf("to") + 2).Trim();
                //}

                lDateV = new DateTime();
                if (DateTime.TryParse(lRDateFrom, out lDateV) != true)
                {
                    lRDateFrom = "2000-01-01 00:00:00";
                }
                if (DateTime.TryParse(lRDateTo, out lDateV) != true)
                {
                    lRDateTo = "2200-01-01 00:00:00";
                }

                var lCmd = new SqlCommand();
                SqlDataReader lRst;
                var lNDSCon = new SqlConnection();

                var lIDBCon = new OracleConnection();
                var lIDBCmd = new OracleCommand();
                OracleDataReader lOraRst;
                //var lCISCon = new OracleConnection();
                //var lOraCmd = new OracleCommand();
                //OracleDataReader lOraRst;

                var lReturn = (new[]{ new
            {
                SSNNo = 0,
                OrderNo = "0",
                WBS1 = "",
                WBS2 = "",
                WBS3 = "",
                StructureElement = "",
                ProdType = "",
                PONo = "",
                PODate = "",
                RequiredDate = "",
                OrderWeight = "",
                SubmittedBy = "",
                OrderStatus = "",
                OrderSource = "",
                SONo = "",
                SORNo = "",
                BBSNo = "",
                BBSDesc = "",
                CustomerCode = "",
                ProjectCode = "",
                ProjectTitle = "",
                DataEnteredBy = "",
                Confirmed = 0,
                PlanDeliveryDate = "",
                SubmitBy="",
                                        Address = "",       // <-- added
    Gate = ""         // <-- added
            }}).ToList();

                if (lReturn.Count > 0)
                {
                    lReturn.RemoveAt(0);
                }

                if (CustomerCode != null && ProjectCode != null)
                {
                    if (CustomerCode.Length > 0 && ProjectCode.Length > 0)
                    {
                        var lProjectState = "";
                        var lAddressState = "";
                        if (AllProjects == true)
                        {
                            UserAccessController lUa = new UserAccessController();
                            var lUserType = lUa.getUserType(pUserName);
                            var lGroupName = lUa.getGroupName(pUserName);

                            lUa = null;

                            SharedAPIController lBackEnd = new SharedAPIController();

                            var lProjects = lBackEnd.getProject(CustomerCode, lUserType, lGroupName);
                            lBackEnd = null;

                            if (lProjects != null && lProjects.Count > 0)
                            {
                                for (int i = 0; i < lProjects.Count; i++)
                                {
                                    if (lProjects[i] != null && lProjects[i].ProjectCode != null && lProjects[i].ProjectCode.Trim().Length > 0)
                                    {
                                        /// int lRtn = await UpdateStatus(CustomerCode, lProjects[i].ProjectCode);

                                        if (lProjectState == "")
                                        {
                                            lProjectState = "'" + lProjects[i].ProjectCode + "' ";
                                        }
                                        else
                                        {
                                            lProjectState = lProjectState + ",'" + lProjects[i].ProjectCode + "' ";
                                        }
                                    }
                                }
                            }

                            if (lProjectState == "")
                            {
                                lProjectState = "'' ";
                            }
                        }
                        else
                        {
                            lProjectState = "'" + ProjectCode + "' ";
                        }

                        if (AddressCodes != null && AddressCodes.Any())
                        {
                            var validCodes = AddressCodes
                                .Where(code => !string.IsNullOrWhiteSpace(code))
                                .Distinct() // optional: remove duplicates
                                .ToList();

                            if (validCodes.Any())
                            {
                                lAddressState = "AND M.AddressCode IN ('" + string.Join("', '", validCodes) + "')";
                            }
                            else
                            {
                                // ✅ All codes were null/empty/whitespace
                                lAddressState = "AND (M.AddressCode = '' OR M.AddressCode IS NULL)";
                            }
                        }
                        else
                        {
                            // ✅ AddressCodes list itself is null or empty
                            lAddressState = "AND (M.AddressCode = '' OR M.AddressCode IS NULL)";
                        }



                        #region Retrieve Data
                        lSQL =
                        "SELECT " +
                        "M.OrderNumber, " +
                        "M.WBS1, " +
                        "M.WBS2, " +
                        "M.WBS3, " +
                        "S.StructureElement, " +
                        "S.ProductType, " +
                        "S.PONumber, " +

                        "isNull(convert(varchar(10), S.PODate, 120),'') AS PODate, " + //7
                        "isNull(convert(varchar(10), S.RequiredDate, 120),'') AS RequiredDate, " + //8
                        "M.TotalWeight, " +             //9
                        "case when M.OrderStatus = 'Sent' then M.SubmitBy else M.UpdateBy end as UpdateBy, " +
                        "M.OrderStatus, " +
                        "(STUFF ( " +
                        " (" +
                        " SELECT ',' + SAPSONo FROM " +

                        " (SELECT SAPSONo as SAPSONo " +
                        " FROM dbo.OESStdSheetJobAdvice " +
                        " WHERE CustomerCode = M.CustomerCode AND " +
                        " ProjectCode = M.ProjectCode AND " +
                        "(JobID = S.StdMESHJobID " +
                        "OR JobID = S.StdBarsJobID " +
                        "OR JobID = S.CoilProdJobID) " +
                        " GROUP BY SAPSONo " +

                        " Union " +
                        " select BBSSOR as SAPSONo " +
                        " FROM  dbo.OESBBS " +
                        " WHERE CustomerCode = M.CustomerCode AND " +
                        " ProjectCode = M.ProjectCode AND " +
                        " JobID = S.CABJobID " +
                        " GROUP BY BBSSOR " +

                        " Union " +
                        " select BBSSORCoupler as SAPSONo " +
                        " FROM  dbo.OESBBS " +
                        " WHERE CustomerCode = M.CustomerCode AND " +
                        " ProjectCode = M.ProjectCode AND " +
                        " JobID = S.CABJobID " +
                        " GROUP BY BBSSORCoupler " +

                        " Union " +
                        " select BBSSAPSO as SAPSONo " +
                        " FROM  dbo.OESBBS " +
                        " WHERE CustomerCode = M.CustomerCode AND " +
                        " ProjectCode = M.ProjectCode AND " +
                        " JobID = S.CABJobID " +
                        " GROUP BY BBSSAPSO " +

                        " Union " +
                        " select sor_no as SAPSONo " +
                        " FROM  dbo.OESBPCDetailsProc " +
                        " WHERE CustomerCode = M.CustomerCode AND " +
                        " ProjectCode = M.ProjectCode AND " +
                        " JobID = S.BPCJobID " +
                        " GROUP BY sor_no " +

                        " ) as k " +
                        " FOR XML PATH('')), 1, 1, '')) " +
                        " AS SOR, " +
                        "(STUFF( " +
                        "(SELECT ',' + isNull(BBSNo, '') " +
                        "FROM  dbo.OESBBS " +
                        "WHERE CustomerCode =  M.CustomerCode " +
                        "AND ProjectCode = M.ProjectCode " +
                        "AND JobID = S.CABJobID " +
                        "GROUP BY BBSNo FOR XML PATH('')), 1, 1, '')) as BBSNo, " +
                        "(STUFF( " +
                        "(SELECT ',' + isNull(BBSDesc, '') " +
                        "FROM  dbo.OESBBS " +
                        "WHERE CustomerCode =  M.CustomerCode " +
                        "AND ProjectCode = M.ProjectCode " +
                        "AND JobID = S.CABJobID " +
                        "GROUP BY BBSDesc FOR XML PATH('')), 1, 1, '')) as BBSDesc, " +
                        "S.SAPSOR as UXSOR, " +
                        "M.CustomerCode, " +
                        "M.ProjectCode, " +
                        "M.SubmitBy, " +
                        "(SELECT ProjectTitle FROM dbo.OESProject " +
                        "WHERE CustomerCode = M.CustomerCode " +
                        "AND ProjectCode = M.ProjectCode) as ProjectTitle, " +
                        "CASE WHEN S.ProductType = 'CAB' THEN " +
                        "isNull(STUFF((SELECT  ',' + UpdateBy " +
                        "FROM dbo.OESOrderDetails " +
                        "WHERE CustomerCode = M.CustomerCode " +
                        "AND ProjectCode = M.ProjectCode " +
                        "AND JobID = S.CABJobID " +
                        "GROUP BY UpdateBy " +
                        "ORDER BY UpdateBy " +
                        "FOR XML PATH('')), 1, 1, ''),'') " +
                        "WHEN S.ProductType = 'CUT-TO-SIZE-MESH' THEN " +
                        "isNull(STUFF((SELECT  ',' + UpdateBy " +
                        "FROM dbo.OESCTSMESHOthersDetails " +
                        "WHERE CustomerCode = M.CustomerCode " +
                        "AND ProjectCode = M.ProjectCode " +
                        "AND JobID = S.MESHJobID " +
                        "GROUP BY UpdateBy " +
                        "ORDER BY UpdateBy " +
                        "FOR XML PATH('')), 1, 1, ''),'') " +
                        "WHEN S.ProductType = 'STANDARD-MESH' THEN " +
                        "isNull(STUFF((SELECT  ',' + UpdateBy " +
                        "FROM dbo.OESStdSheetDetails " +
                        "WHERE CustomerCode = M.CustomerCode " +
                        "AND ProjectCode = M.ProjectCode " +
                        "AND JobID = S.StdMESHJobID " +
                        "GROUP BY UpdateBy " +
                        "ORDER BY UpdateBy " +
                        "FOR XML PATH('')), 1, 1, ''),'') " +
                        "WHEN S.ProductType = 'STANDARD-BAR' THEN " +
                        "isNull(STUFF((SELECT  ',' + UpdateBy " +
                        "FROM dbo.OESStdProdDetails " +
                        "WHERE CustomerCode = M.CustomerCode " +
                        "AND ProjectCode = M.ProjectCode " +
                        "AND JobID = S.StdBarsJobID " +
                        "GROUP BY UpdateBy " +
                        "ORDER BY UpdateBy " +
                        "FOR XML PATH('')), 1, 1, ''),'') " +
                        "WHEN S.ProductType = 'BPC' THEN " +
                        "isNull(STUFF((SELECT  ',' + UpdateBy " +
                        "FROM dbo.OESBPCDetails " +
                        "WHERE CustomerCode = M.CustomerCode " +
                        "AND ProjectCode = M.ProjectCode " +
                        "AND Template = 0 " +
                        "AND JobID = S.BPCJobID " +
                        "GROUP BY UpdateBy " +
                        "ORDER BY UpdateBy " +
                        "FOR XML PATH('')), 1, 1, ''),'') " +
                        "WHEN S.ProductType = 'COIL' OR S.ProductType = 'COUPLER' THEN " +
                        "isNull(STUFF((SELECT  ',' + UpdateBy " +
                        "FROM dbo.OESStdProdDetails " +
                        "WHERE CustomerCode = M.CustomerCode " +
                        "AND ProjectCode = M.ProjectCode " +
                        "AND JobID = S.CoilProdJobID " +
                        "GROUP BY UpdateBy " +
                        "ORDER BY UpdateBy " +
                        "FOR XML PATH('')), 1, 1, ''),'') " +
                        "ELSE '' END as DataEnteredBy, " +
    // ✅ Newly added columns:
    "ISNULL(M.Address, ' ') AS Address, " +
    "ISNULL(M.Gate, ' ') AS Gate " +
                        "FROM dbo.OESProjOrder M, dbo.OESProjOrdersSE S " +
                        "WHERE M.OrderNumber = S.OrderNumber " +
                        "AND M.CustomerCode = '" + CustomerCode + "' " +
                        "AND M.ProjectCode IN (" + lProjectState + ") " +
                        " " + lAddressState +  // ✅ fixed line
                        "AND M.OrderStatus is not NULL " +
                        "AND M.OrderStatus <> 'New' " +
                        "AND M.OrderStatus <> 'Created' " +
                        "AND M.OrderStatus <> 'Cancelled' " +
                        "AND M.OrderStatus <> 'Deleted' " +
                        "AND M.OrderStatus <> 'Delivered' " +
                        //"AND ((S.PODate >= '" + lPODateFrom + "' " +
                        //"AND DATEADD(d,-1,S.PODate) < '" + lPODateTo + "') " +
                        //"OR (S.PODate is null AND '" + lPODateTo + "' = '2200-01-01 00:00:00' )) " +
                        //"AND ((S.RequiredDate >= '" + lRDateFrom + "' " +
                        //"AND DATEADD(d,-1,S.RequiredDate) < '" + lRDateTo + "') " +
                        //"OR (S.RequiredDate is null AND '" + lRDateTo + "' = '2200-01-01 00:00:00' )) " +
                        //"AND (S.PONumber like '%" + PONumber + "%' " +
                        //"OR  (S.PONumber is null AND '" + PONumber + "' = '' )) " +
                        //"AND (M.WBS1 like '%" + WBS1 + "%' " +
                        //"OR (M.WBS1 is null AND '" + WBS1 + "' = '' )) " +
                        //"AND (M.WBS2 like '%" + WBS2 + "%' " +
                        //"OR (M.WBS2 is null AND '" + WBS2 + "' = '' )) " +
                        //"AND (M.WBS3 like '%" + WBS3 + "%' " +
                        //"OR (M.WBS3 is null AND '" + WBS3 + "' = '' )) " +
                        ////"AND M.UpdateDate >= '2019-07-01' " +
                        "GROUP BY " +
                        "M.CustomerCode, " +
                        "M.ProjectCode, " +
                        "S.CABJobID, " +
                        "S.MESHJobID, " +
                        "S.StdMESHJobID, " +
                        "S.StdBarsJobID, " +
                        "S.CoilProdJobID, " +
                        "S.BPCJobID, " +
                        "S.StructureElement, " +
                        "S.ProductType, " +
                        "S.ScheduledProd, " +
                        "S.PONumber, " +

                        "S.PODate, " +
                        "S.RequiredDate, " +
                        "M.OrderNumber, " +
                        "M.WBS1, " +
                        "M.WBS2, " +
                        "M.WBS3, " +
                        "M.TotalWeight, " +
                        "M.UpdateBy, " +
                        "M.OrderStatus, " +
                        "S.SAPSOR, " +
                        "M.SubmitBy, " +
    // ✅ Also added in GROUP BY
    "M.Address, " +
    "M.Gate " +
                        "ORDER BY " +
                        "M.OrderNumber DESC ";

                        var lProcessObj = new ProcessController();
                        if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
                        {
                            lCmd.CommandText = lSQL;
                            lCmd.Connection = lNDSCon;
                            lCmd.CommandTimeout = 300;
                            lRst = await lCmd.ExecuteReaderAsync();
                            if (lRst.HasRows)
                            {
                                var lSNo = 0;
                                while (lRst.Read())
                                {
                                    lSNo = lSNo + 1;
                                    var lOrderStatus = (lRst.GetValue(11) == DBNull.Value ? "" : lRst.GetString(11).Trim());
                                    if (lOrderStatus == "Sent")
                                    {
                                        lOrderStatus = "Pending Approval";
                                    }
                                    if (lOrderStatus == "Submitted")
                                    {
                                        lOrderStatus = "Submitted to NSH";
                                    }
                                    var lUXSOR = lRst.GetValue(15) == DBNull.Value ? "" : lRst.GetString(15).Trim();
                                    var lMySOR = lRst.GetValue(12) == DBNull.Value ? "" : lRst.GetString(12).Trim();
                                    if (lUXSOR != "" && lUXSOR != lMySOR)
                                    {
                                        if (lMySOR == "")
                                        {
                                            lMySOR = lUXSOR;
                                        }
                                        else
                                        {
                                            lMySOR = lMySOR = "," + lUXSOR;
                                        }

                                    }
                                    lReturn.Add(new
                                    {
                                        SSNNo = lSNo,
                                        OrderNo = lRst.GetInt32(0).ToString(),
                                        WBS1 = (lRst.GetValue(1) == DBNull.Value ? "" : lRst.GetString(1).Trim()),
                                        WBS2 = (lRst.GetValue(2) == DBNull.Value ? "" : lRst.GetString(2).Trim()),
                                        WBS3 = (lRst.GetValue(3) == DBNull.Value ? "" : lRst.GetString(3).Trim()),
                                        StructureElement = lRst.GetValue(4) == DBNull.Value ? "" : (lRst.GetString(4).Trim() == "NONWBS" ? "" : lRst.GetString(4).Trim()),
                                        ProdType = (lRst.GetValue(5) == DBNull.Value ? "" : lRst.GetString(5).Trim()),
                                        PONo = (lRst.GetValue(6) == DBNull.Value ? "" : (lRst.GetString(6).Trim().Length > 0 && lRst.GetString(6).Trim().Substring(0, 1) == "," ? lRst.GetString(6).Trim().Substring(1) : lRst.GetString(6).Trim())),
                                        PODate = (lRst.GetValue(7) == DBNull.Value ? "" : lRst.GetString(7)),
                                        RequiredDate = lRst.GetValue(8) == DBNull.Value ? "" : (lRst.GetString(8).Trim().Length > 0 && lRst.GetString(8).Substring(0, 1) == "," ? lRst.GetString(8).Substring(1) : lRst.GetString(8)),
                                        // OrderWeight = (lRst.GetValue(9) == DBNull.Value ? "0.000" : (lRst.GetDouble(9) / 1000).ToString("###,###,##0.000;(###,##0.000); ")),
                                        OrderWeight = (lRst.IsDBNull(9) ? "0.000" : ((decimal)lRst.GetDecimal(9) / 1000).ToString("###,###,##0.000;(###,##0.000); ")),
                                        SubmittedBy = (lRst.GetValue(10) == DBNull.Value ? "" : lRst.GetString(10).Trim()),
                                        OrderStatus = lOrderStatus,
                                        OrderSource = "DIGIOS",
                                        SONo = lMySOR,
                                        SORNo = lMySOR,
                                        BBSNo = (lRst.GetValue(13) == DBNull.Value ? "" : lRst.GetString(13).Trim()),
                                        BBSDesc = WebUtility.HtmlDecode(lRst.GetValue(14) == DBNull.Value ? "" : lRst.GetString(14).Trim()),
                                        CustomerCode = (lRst.GetValue(16) == DBNull.Value ? "" : lRst.GetString(16).Trim()),
                                        ProjectCode = (lRst.GetValue(17) == DBNull.Value ? "" : lRst.GetString(17).Trim()),
                                        ProjectTitle = (lRst.GetValue(19) == DBNull.Value ? "" : lRst.GetString(19).Trim()),
                                        DataEnteredBy = (lRst.GetValue(20) == DBNull.Value ? "" : lRst.GetString(20).Trim()),
                                        Confirmed = 0,
                                        PlanDeliveryDate = "",
                                        SubmitBy = (lRst.GetValue(18) == DBNull.Value ? "" : lRst.GetString(18).Trim()),
                                        Address = (lRst.GetValue(21) == DBNull.Value ? "" : lRst.GetString(21).Trim()),   // <-- new index 21
                                        Gate = (lRst.GetValue(22) == DBNull.Value ? "" : lRst.GetString(22).Trim())      // <-- new index 22
                                    });
                                }
                            }
                            lRst.Close();

                            lProcessObj.CloseNDSConnection(ref lNDSCon);
                        }

                        string lSOR = "";
                        if (lReturn.Count > 0)
                        {
                            for (int i = 0; i < lReturn.Count; i++)
                            {
                                if (lReturn[i].SONo != null && lReturn[i].SONo != "")
                                {
                                    if (lSOR == "")
                                    {
                                        lSOR = lReturn[i].SONo;
                                    }
                                    else
                                    {
                                        lSOR = lSOR + "," + lReturn[i].SONo;
                                    }
                                }
                            }
                        }

                        if (lSOR != "")
                        {
                            var lSORA = lSOR.Split(',').ToList();
                            lSOR = "";
                            if (lSORA.Count > 0)
                            {
                                for (int i = 0; i < lSORA.Count; i++)
                                {
                                    if (lSOR == "")
                                    {
                                        lSOR = "'" + lSORA[i] + "'";
                                    }
                                    else
                                    {
                                        lSOR = lSOR + "," + "'" + lSORA[i] + "'";
                                    }
                                }
                            }
                        }

                        if (lSOR == "")
                        {
                            lSOR = "' '";
                        }

                        //take out duplicated record
                        if (lReturn.Count > 0)
                        {
                            for (int i = lReturn.Count - 1; i > 0; i--)
                            {
                                if (lReturn[i].OrderNo == lReturn[i - 1].OrderNo)
                                {
                                    lReturn[i - 1] = new
                                    {
                                        SSNNo = lReturn[i - 1].SSNNo,
                                        OrderNo = lReturn[i - 1].OrderNo,
                                        WBS1 = lReturn[i - 1].WBS1,
                                        WBS2 = lReturn[i - 1].WBS2,
                                        WBS3 = lReturn[i - 1].WBS3,
                                        StructureElement = (lReturn[i - 1].StructureElement == null || lReturn[i - 1].StructureElement.Trim() == "") ? lReturn[i].StructureElement : ((lReturn[i].StructureElement != null && lReturn[i].StructureElement.Trim() != "" && lReturn[i].StructureElement != lReturn[i - 1].StructureElement) ? (lReturn[i - 1].StructureElement + "," + lReturn[i].StructureElement) : lReturn[i].StructureElement),
                                        ProdType = (lReturn[i - 1].ProdType == null || lReturn[i - 1].ProdType.Trim() == "") ? lReturn[i].ProdType : ((lReturn[i].ProdType != null && lReturn[i].ProdType.Trim() != "" && lReturn[i].ProdType != lReturn[i - 1].ProdType) ? (lReturn[i - 1].ProdType + "," + lReturn[i].ProdType) : lReturn[i].ProdType),
                                        PONo = (lReturn[i - 1].PONo == null || lReturn[i - 1].PONo.Trim() == "") ? lReturn[i].PONo : ((lReturn[i].PONo != null && lReturn[i].PONo.Trim() != "" && lReturn[i].PONo != lReturn[i - 1].PONo) ? (lReturn[i - 1].PONo + "," + lReturn[i].PONo) : lReturn[i].PONo),
                                        PODate = (lReturn[i - 1].PODate == null || lReturn[i - 1].PODate.Trim() == "") ? lReturn[i].PODate : ((lReturn[i].PODate != null && lReturn[i].PODate.Trim() != "" && lReturn[i].PODate != lReturn[i - 1].PODate) ? (lReturn[i - 1].PODate + "," + lReturn[i].PODate) : lReturn[i].PODate),
                                        RequiredDate = (lReturn[i - 1].RequiredDate == null || lReturn[i - 1].RequiredDate.Trim() == "") ? lReturn[i].RequiredDate : ((lReturn[i].RequiredDate != null && lReturn[i].RequiredDate.Trim() != "" && lReturn[i].RequiredDate != lReturn[i - 1].RequiredDate) ? (lReturn[i - 1].RequiredDate + "," + lReturn[i].RequiredDate) : lReturn[i].RequiredDate),
                                        OrderWeight = lReturn[i - 1].OrderWeight,
                                        SubmittedBy = lReturn[i - 1].SubmittedBy,
                                        OrderStatus = lReturn[i - 1].OrderStatus,
                                        OrderSource = lReturn[i - 1].OrderSource,
                                        SONo = (lReturn[i - 1].SONo == null || lReturn[i - 1].SONo.Trim() == "") ? lReturn[i].SONo : ((lReturn[i].SONo != null && lReturn[i].SONo.Trim() != "" && lReturn[i].SONo != lReturn[i - 1].SONo) ? (lReturn[i - 1].SONo + "," + lReturn[i].SONo) : lReturn[i].SONo),
                                        SORNo = (lReturn[i - 1].SORNo == null || lReturn[i - 1].SORNo.Trim() == "") ? lReturn[i].SORNo : ((lReturn[i].SORNo != null && lReturn[i].SORNo.Trim() != "" && lReturn[i].SORNo != lReturn[i - 1].SORNo) ? (lReturn[i - 1].SORNo + "," + lReturn[i].SORNo) : lReturn[i].SORNo),
                                        BBSNo = (lReturn[i - 1].BBSNo == null || lReturn[i - 1].BBSNo.Trim() == "") ? lReturn[i].BBSNo : ((lReturn[i].BBSNo != null && lReturn[i].BBSNo.Trim() != "" && lReturn[i].BBSNo != lReturn[i - 1].BBSNo) ? (lReturn[i - 1].BBSNo + "," + lReturn[i].BBSNo) : lReturn[i].BBSNo),
                                        BBSDesc = (lReturn[i - 1].BBSDesc == null || lReturn[i - 1].BBSDesc.Trim() == "") ? lReturn[i].BBSDesc : ((lReturn[i].BBSDesc != null && lReturn[i].BBSDesc.Trim() != "" && lReturn[i].BBSDesc != lReturn[i - 1].BBSDesc) ? (lReturn[i - 1].BBSDesc + "," + lReturn[i].BBSDesc) : lReturn[i].BBSDesc),
                                        CustomerCode = lReturn[i - 1].CustomerCode,
                                        ProjectCode = lReturn[i - 1].ProjectCode,
                                        ProjectTitle = lReturn[i - 1].ProjectTitle,
                                        DataEnteredBy = lReturn[i - 1].DataEnteredBy,
                                        Confirmed = lReturn[i - 1].Confirmed,
                                        PlanDeliveryDate = lReturn[i - 1].PlanDeliveryDate,
                                        SubmitBy = lReturn[i - 1].SubmitBy,
                                        Address = lReturn[i - 1].Address,
                                        Gate = lReturn[i - 1].Gate
                                    };

                                    lReturn.RemoveAt(i);
                                }
                            }
                        }

                        // lProcessObj.OpenCISConnection(ref lCISCon);
                        lProcessObj.OpenNDSConnection(ref lNDSCon);

                        #region Check from CIS Plan Delivery Date
                        if (lSOR != "' '" && lSOR != "''")
                        {
                            var lSORList = new List<string>();
                            if (lSOR != "")
                            {
                                var lSORA = lSOR.Split(',').ToList();
                                var lSOR1 = "";
                                if (lSORA.Count > 0)
                                {
                                    int lCount = 0;
                                    for (int i = 0; i < lSORA.Count; i++)
                                    {
                                        if (lSOR1 == "")
                                        {
                                            //lSOR1 = "'" + lSORA[i] + "'";
                                            lSOR1 = lSORA[i];
                                        }
                                        else
                                        {
                                            //lSOR1 = lSOR1 + "," + "'" + lSORA[i] + "'";
                                            lSOR1 = lSOR1 + "," + lSORA[i];
                                        }
                                        lCount = lCount + 1;
                                        if (lCount > 300)
                                        {
                                            lSORList.Add(lSOR1);
                                            lSOR1 = "";
                                            lCount = 0;
                                        }
                                    }
                                    if (lSOR1 != "")
                                    {
                                        lSORList.Add(lSOR1);
                                    }
                                }

                            }

                            for (int k = 0; k < lSORList.Count; k++)
                            {
                                //lSQL = "SELECT " +
                                //"NVL(M.ORDER_REQUEST_NO, ' '), " +
                                //"M.REQ_DAT_TO, " +                                  //1. Revised Req date
                                //"M.FIRST_PROMISED_D, " +                            //2. First Promised date
                                //                                                    //"(SELECT NVL(MAX(conf_del_date), ' ') " +           
                                //                                                    //"FROM SAPSR3.YMSDT_ORDER_ITEM " +
                                //                                                    //"WHERE MANDT = M.MANDT " +
                                //                                                    //"AND ORDER_REQUEST_NO = M.ORDER_REQUEST_NO) as Conf_del_date,  " + //3. Confirmed Del from CIS
                                //"' ', " +
                                ////"(SELECT MIN(PLAN_DELIV_DATE) FROM SAPSR3.YMPPT_LP_ITEM_C LC, SAPSR3.YMPPT_LP_HDR LH " +
                                ////"WHERE LC.MANDT = LH.MANDT " +
                                ////"AND LC.LOAD_NO = LH.LOAD_NO " +
                                ////"AND SALES_ORDER = M.SALES_ORDER) as plan_del_date, " +         //4. Confirmed Del from Planning
                                //"(SELECT MIN(LOAD_DATE) FROM SAPSR3.YMPPT_LP_ITEM_C LC, SAPSR3.YMPPT_LP_HDR LH " +
                                //"WHERE LC.MANDT = LH.MANDT AND LC.LOAD_NO = LH.LOAD_NO AND LH.VEHICLE_TYPE <> 'MC' " +
                                //"AND SALES_ORDER = M.SALES_ORDER) as plan_del_date, " +         //4. Confirmed Del from Planning
                                //"(SELECT MIN(K.LFDAT) FROM SAPSR3.LIPS P, SAPSR3.LIKP K " +
                                //"WHERE P.MANDT = K.MANDT " +
                                //"AND P.VBELN = K.VBELN " +
                                //"AND P.VGBEL = M.SALES_ORDER) as plan_del_conf, " +              //5. Confirmed Del from SAP
                                //"D.BBS_NO " +
                                //"FROM SAPSR3.YMSDT_ORDER_HDR M LEFT OUTER JOIN SAPSR3.YMSDT_REQ_DETAIL D " +
                                //"ON M.order_request_no = D.order_request_no " +
                                //"WHERE M.MANDT = '" + lProcessObj.strClient + "' " +
                                //"AND M.PROJECT IN (" + lProjectState + ") " +
                                //"AND M.REQD_DEL_DATE >= to_char(sysdate - 180, 'yyyymmdd') " +
                                //"AND M.SALES_ORDER NOT IN " +
                                //"(SELECT VBELN FROM  SAPSR3.VBUK " +
                                //"WHERE VBELN = M.SALES_ORDER AND ABSTK = 'C') " +
                                //"AND M.ORDER_REQUEST_NO IN " +
                                //"(" + lSORList[k] + ") ";

                                //lSQL = "SELECT M.ORDER_REQUEST_NO, M.REQ_DAT_TO, M.FIRST_PROMISED_D,'' ,(select min(PLAN_SHIPPING_DATE)  from SalesOrderloading sol " +
                                //    "inner join HMILoadDetails hmi on sol.LOAD_NO=hmi.LOAD_NO where hmi.SALES_ORDER=m.sales_order) as plan_del_date,(select min(confirmdeldate)" +
                                //    " from HMIOrderHeaderDetails where OrderNo=m.sales_order) AS plan_del_conf,D.BBS_NO FROM OesOrderHeaderHMI M LEFT OUTER JOIN  Oesrequestdetailshmi D " +
                                //    "ON M.order_request_no = D.order_request_no WHERE M.MANDT = '600' AND M.PROJECT IN (" + lProjectState + ") AND M.REQD_DEL_DATE >= GETDATE() - 180 " +
                                //    "AND M.Order_request_no in(" + lSORList[k] + ")";

                                lSQL = "SELECT " +
                                       "ISNULL(M.ORDER_REQUEST_NO, ' ') AS ORDER_REQUEST_NO, " +
                                       "M.REQ_DAT_TO, " +                                                               //1. Revised Req date
                                       "M.FIRST_PROMISED_D, " +                                                         //2. First Promised date
                                       "' ' AS UNUSED_COLUMN, " +
                                       "(SELECT TOP 1 CONVERT(varchar, CONVERT(date, PLAN_SHIPPING_DATE, 103), 112) " +
                                           "FROM SalesOrderloading sol " +
                                           "INNER JOIN HMILoadDetails hmi ON sol.LOAD_NO = hmi.LOAD_NO " +
                                           "WHERE hmi.SALES_ORDER = M.SALES_ORDER) AS PLAN_DEL_DATE, " +                //3. Confirmed Del from HMI (Shipping Date)
                                       "(SELECT CONVERT(varchar, CONVERT(date, MIN(confirmdeldate), 103), 112) " +
                                           "FROM HMIOrderHeaderDetails " +
                                           "WHERE OrderNo = M.SALES_ORDER) AS PLAN_DEL_CONF, " +                        //4. Confirmed Del from HMI
                                       "D.BBS_NO " +                                                                    //5. BBS Number
                                       "FROM OesOrderHeaderHMI M " +
                                       "LEFT OUTER JOIN OESRequestDetailsHMI D ON M.ORDER_REQUEST_NO = D.ORDER_REQUEST_NO " +
                                       "WHERE M.MANDT = '" + lProcessObj.strClient + "' " +
                                       "AND M.PROJECT IN (" + lProjectState + ") " +
                                       "AND M.REQD_DEL_DATE >= GETDATE() - 180 " +
                                       "AND M.STATUS <> 'X' " +
                                       "AND M.ORDER_REQUEST_NO IN (" + lSORList[k] + ")";


                                lCmd.CommandText = lSQL;
                                lCmd.Connection = lNDSCon;
                                lCmd.CommandTimeout = 300;
                                lRst = await lCmd.ExecuteReaderAsync();
                                if (lRst.HasRows)
                                {
                                    while (lRst.Read())
                                    {
                                        int lConfirmed = 0;
                                        var lSORNo = (lRst.GetValue(0) == DBNull.Value ? "" : lRst.GetString(0));

                                        //var lReqDate = (lOraRst.GetValue(2) == DBNull.Value ? "" : lOraRst.GetString(2).Trim());
                                        //if (lReqDate == "")
                                        //{
                                        var lReqDate = (lRst.GetValue(1) == DBNull.Value ? "" : lRst.GetString(1).Trim());
                                        //}
                                        //else
                                        //{
                                        //    if (lReqDate.Length > 10)
                                        //    {
                                        //        lReqDate = lReqDate.Substring(0, 10);
                                        //    }
                                        //    lReqDate = String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lReqDate, "yyyy-MM-dd", CultureInfo.InvariantCulture));
                                        //}

                                        //var lReqDateConf = (lOraRst.GetValue(3) == DBNull.Value ? "" : lOraRst.GetString(3).Trim());
                                        //if (lReqDateConf != null && lReqDateConf != "" && lReqDateConf != "20500101" &&
                                        //    DateTime.ParseExact(lReqDateConf, "yyyyMMdd", CultureInfo.InvariantCulture) > DateTime.Now.AddMonths(-6) &&
                                        //    DateTime.ParseExact(lReqDateConf, "yyyyMMdd", CultureInfo.InvariantCulture) < DateTime.Now.AddMonths(6))
                                        //{
                                        //    lReqDate = String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lReqDateConf, "yyyyMMdd", CultureInfo.InvariantCulture));
                                        //}

                                        var lBBSNo = (lRst.GetValue(6) == DBNull.Value ? "" : lRst.GetString(6).Trim());
                                        var lPlanDelDate = (lRst.GetValue(4) == DBNull.Value ? "" : lRst.GetString(4).Trim());
                                        if (lPlanDelDate != null && lPlanDelDate != "")
                                        {
                                            if (String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lPlanDelDate, "yyyyMMdd", CultureInfo.InvariantCulture))
                                                == (String.Format("{0:yyyy-MM-dd}", DateTime.Now)))
                                            {
                                                lConfirmed = 1;
                                            }
                                            else if (String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lPlanDelDate, "yyyyMMdd", CultureInfo.InvariantCulture))
                                                == (String.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(1))))
                                            {
                                                lConfirmed = 2;
                                            }
                                            else
                                            {
                                                lConfirmed = 3;
                                            }

                                            if (DateTime.ParseExact(lPlanDelDate, "yyyyMMdd", CultureInfo.InvariantCulture) >= DateTime.Now.AddMonths(6))
                                            {
                                                lPlanDelDate = "";
                                            }
                                            else
                                            {
                                                lPlanDelDate = String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lPlanDelDate, "yyyyMMdd", CultureInfo.InvariantCulture));
                                            }
                                        }
                                        else
                                        {
                                            lPlanDelDate = "";
                                        }

                                        //var lPlanDelConfDate = (lOraRst.GetValue(5) == DBNull.Value ? "" : lOraRst.GetString(5).Trim());
                                        //if (lPlanDelConfDate != null && lPlanDelConfDate != "")
                                        //{
                                        //    if (String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lPlanDelConfDate, "yyyyMMdd", CultureInfo.InvariantCulture))
                                        //        == (String.Format("{0:yyyy-MM-dd}", DateTime.Now)))
                                        //    {
                                        //        lConfirmed = 1;
                                        //    }
                                        //    else if (String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lPlanDelConfDate, "yyyyMMdd", CultureInfo.InvariantCulture))
                                        //        == (String.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(1))))
                                        //    {
                                        //        lConfirmed = 2;
                                        //    }
                                        //    else
                                        //    {
                                        //        lConfirmed = 3;
                                        //    }
                                        //    //lReqDate = String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lPlanDelConfDate, "yyyyMMdd", CultureInfo.InvariantCulture));
                                        //}

                                        if (lReqDate.Length > 10)
                                        {
                                            lReqDate = lReqDate.Substring(0, 10);
                                        }

                                        if (lSORNo != null && lSORNo != "" && lReturn.Count > 0 && lReqDate != null && lReqDate != "" &&
                                        DateTime.ParseExact(lReqDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) > DateTime.Now.AddMonths(-6) &&
                                        DateTime.ParseExact(lReqDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) < DateTime.Now.AddMonths(6))
                                        {
                                            for (int i = 0; i < lReturn.Count; i++)
                                            {
                                                if (lReturn[i].SONo.IndexOf(lSORNo) >= 0)
                                                {
                                                    var lSONOList = lReturn[i].SONo.Split(',').ToList();
                                                    var lReqDateList = lReturn[i].RequiredDate.Split(',').ToList();
                                                    if (lSONOList.Count > 0)
                                                    {
                                                        for (int j = 0; j < lSONOList.Count; j++)
                                                        {
                                                            if (lSONOList[j] == lSORNo)
                                                            {
                                                                if (lReqDateList.Count > j)
                                                                {
                                                                    lReqDateList[j] = lReqDate;
                                                                }
                                                                else
                                                                {
                                                                    lReqDateList.Add(lReqDate);
                                                                }
                                                            }

                                                        }
                                                    }
                                                    var lNewReqDate = "";
                                                    if (lReqDateList.Count > 0)
                                                    {
                                                        for (int j = 0; j < lReqDateList.Count; j++)
                                                        {
                                                            if (lNewReqDate == "")
                                                            {
                                                                lNewReqDate = lReqDateList[j];
                                                            }
                                                            else
                                                            {
                                                                if (lNewReqDate.IndexOf(lReqDateList[j]) < 0)
                                                                {
                                                                    lNewReqDate = lNewReqDate + "," + lReqDateList[j];
                                                                }
                                                            }
                                                        }
                                                    }
                                                    if (lNewReqDate == "")
                                                    {
                                                        lNewReqDate = lReqDate;
                                                    }

                                                    var lDeliveryDate = lReturn[i].PlanDeliveryDate;
                                                    if (lDeliveryDate == null)
                                                    {
                                                        lDeliveryDate = "";
                                                    }
                                                    //lDeliveryDate = (lReturn[i].PlanDeliveryDate == null || lReturn[i].PlanDeliveryDate.Trim() == "") ? lPlanDelDate : ((lPlanDelDate != null && lPlanDelDate != "" && lReturn[i].PlanDeliveryDate.IndexOf(lPlanDelDate) < 0) ? lReturn[i].PlanDeliveryDate + "," + lPlanDelDate : lReturn[i].PlanDeliveryDate);

                                                    if (lPlanDelDate != null && lPlanDelDate != "")
                                                    {
                                                        if (lDeliveryDate.Trim() == "")
                                                        {
                                                            if (lSORNo.Substring(0, 3) == "103")
                                                            {
                                                                if (lReturn[i].SONo.LastIndexOf(",") > 0 && (lReturn[i].SONo.Substring(0, 1) == "8" || lReturn[i].SONo.IndexOf(",8") >= 0))
                                                                {
                                                                    lDeliveryDate = lPlanDelDate + "(SB)";
                                                                }
                                                                else
                                                                {
                                                                    lDeliveryDate = lPlanDelDate;
                                                                }
                                                            }
                                                            else if (lReturn[i].SONo.LastIndexOf(",") > 0 && (lReturn[i].SONo.Substring(0, 3) == "103" || lReturn[i].SONo.IndexOf(",103") > 0))
                                                            {
                                                                if (lBBSNo.IndexOf("-COUPLER") > 0 || lBBSNo.IndexOf("-CP") > 0)
                                                                {
                                                                    lDeliveryDate = lPlanDelDate + "(CP)";
                                                                }
                                                                else
                                                                {
                                                                    lDeliveryDate = lPlanDelDate + "(CAB)";
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (lReturn[i].SONo.LastIndexOf(",") > 0 && (lReturn[i].BBSNo.IndexOf("-COUPLER") > 0 || lReturn[i].BBSNo.IndexOf("-CP") > 0))
                                                                {
                                                                    if (lBBSNo.IndexOf("-COUPLER") > 0 || lBBSNo.IndexOf("-CP") > 0)
                                                                    {
                                                                        lDeliveryDate = lPlanDelDate + "(CP)";
                                                                    }
                                                                    else
                                                                    {
                                                                        lDeliveryDate = lPlanDelDate + "(CAB)";
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    lDeliveryDate = lPlanDelDate;
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (lDeliveryDate.IndexOf(lPlanDelDate) < 0)
                                                            {
                                                                if (lSORNo.Substring(0, 3) == "103")
                                                                {
                                                                    if (lReturn[i].SONo.LastIndexOf(",") > 0 && (lReturn[i].SONo.Substring(0, 1) == "8" || lReturn[i].SONo.IndexOf(",8") >= 0))
                                                                    {
                                                                        lDeliveryDate = lDeliveryDate + "," + lPlanDelDate + "(SB)";
                                                                    }
                                                                    else
                                                                    {
                                                                        lDeliveryDate = lDeliveryDate + "," + lPlanDelDate;
                                                                    }
                                                                }
                                                                else if (lReturn[i].SONo.LastIndexOf(",") > 0 && (lReturn[i].SONo.Substring(0, 3) == "103" || lReturn[i].SONo.IndexOf(",103") > 0))
                                                                {
                                                                    if (lBBSNo.IndexOf("-COUPLER") > 0 || lBBSNo.IndexOf("-CP") > 0)
                                                                    {
                                                                        lDeliveryDate = lDeliveryDate + "," + lPlanDelDate + "(CP)";
                                                                    }
                                                                    else
                                                                    {
                                                                        lDeliveryDate = lDeliveryDate + "," + lPlanDelDate + "(CAB)";
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (lReturn[i].SONo.LastIndexOf(",") > 0 && (lReturn[i].BBSNo.IndexOf("-COUPLER") > 0 || lReturn[i].BBSNo.IndexOf("-CP") > 0))
                                                                    {
                                                                        if (lBBSNo.IndexOf("-COUPLER") > 0 || lBBSNo.IndexOf("-CP") > 0)
                                                                        {
                                                                            lDeliveryDate = lDeliveryDate + "," + lPlanDelDate + "(CP)";
                                                                        }
                                                                        else
                                                                        {
                                                                            lDeliveryDate = lDeliveryDate + "," + lPlanDelDate + "(CAB)";
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        lDeliveryDate = lDeliveryDate + "," + lPlanDelDate;
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (lDeliveryDate.IndexOf(",") < 0 && lDeliveryDate.IndexOf("(") > 0)
                                                                {
                                                                    lDeliveryDate = lDeliveryDate.Substring(0, lDeliveryDate.IndexOf("("));
                                                                }
                                                            }
                                                        }
                                                    }

                                                    lReturn[i] = new
                                                    {
                                                        SSNNo = lReturn[i].SSNNo,
                                                        OrderNo = lReturn[i].OrderNo,
                                                        WBS1 = lReturn[i].WBS1,
                                                        WBS2 = lReturn[i].WBS2,
                                                        WBS3 = lReturn[i].WBS3,
                                                        StructureElement = lReturn[i].StructureElement,
                                                        ProdType = lReturn[i].ProdType,
                                                        PONo = lReturn[i].PONo,
                                                        PODate = lReturn[i].PODate,
                                                        RequiredDate = lNewReqDate,
                                                        OrderWeight = lReturn[i].OrderWeight,
                                                        SubmittedBy = lReturn[i].SubmittedBy,
                                                        OrderStatus = lReturn[i].OrderStatus,
                                                        OrderSource = lReturn[i].OrderSource,
                                                        SONo = lReturn[i].SONo,
                                                        SORNo = lReturn[i].SORNo,
                                                        BBSNo = lReturn[i].BBSNo,
                                                        BBSDesc = WebUtility.HtmlDecode(lReturn[i].BBSDesc),
                                                        CustomerCode = lReturn[i].CustomerCode,
                                                        ProjectCode = lReturn[i].ProjectCode,
                                                        ProjectTitle = lReturn[i].ProjectTitle,
                                                        DataEnteredBy = lReturn[i].DataEnteredBy,
                                                        Confirmed = lReturn[i].Confirmed == 1 ? 1 : (lConfirmed == 1 ? 1 : (lReturn[i].Confirmed == 2 ? 2 : lConfirmed)),
                                                        PlanDeliveryDate = lDeliveryDate,
                                                        SubmitBy = lReturn[i].SubmitBy,
                                                        Address = lReturn[i].Address,
                                                        Gate = lReturn[i].Gate
                                                    };
                                                    break;
                                                }
                                            }
                                        }

                                    }
                                }
                                lRst.Close();
                            }
                            lProcessObj.CloseNDSConnection(ref lNDSCon);
                        }
                        #endregion

                        lProcessObj.OpenNDSConnection(ref lNDSCon);
                        #region Check Individual SO, but Order Delivered 
                        lPODateFrom_O = lPODateFrom.Replace("-", "");
                        lPODateTo_O = lPODateTo.Replace("-", "");
                        lRDateFrom_O = lRDateFrom.Replace("-", "");
                        lRDateTo_O = lRDateTo.Replace("-", "");

                        var lBalSNo = 0;

                        //lSQL = "SELECT NVL(D.WBS1, ' ') as WBS1, " +
                        //"NVL(D.WBS2, ' ') as WBS2, " +
                        //"NVL(D.WBS3, ' ') as WBS3,  " +
                        //"NVL(D.ST_ELEMENT_TYPE, ' ') as ST_ELE,  " +
                        //"NVL(D.PRODUCT_TYPE, ' ') as Prod_type, " +
                        //"M.PO_NUMBER, " +
                        //"M.CUST_ORDER_DATE, " +
                        //"M.REQ_DAT_TO, " +
                        //"M.FIRST_PROMISED_D, " +
                        //"(SELECT NVL(SUM(THEO_WEIGHT_KG), 0)/1000 " +
                        //"FROM SAPSR3.YMSDT_ORDER_ITEM " +
                        //"WHERE MANDT = M.MANDT " +
                        //"AND ORDER_REQUEST_NO = M.ORDER_REQUEST_NO " +
                        //"AND HG_ITEM_NO = '000000') as WT, " +
                        //"'' as SubumittedBY, " +
                        //"'Reviewed', " +
                        //"(SELECT NVL(MAX(PROD_GRP), ' ') " +
                        //"FROM SAPSR3.YMSDT_ORDER_ITEM " +
                        //"WHERE MANDT = M.MANDT " +
                        //"AND ORDER_REQUEST_NO = M.ORDER_REQUEST_NO) as ProdType2,  " +
                        ////"(SELECT NVL(MAX(conf_del_date), ' ') " +
                        ////"FROM SAPSR3.YMSDT_ORDER_ITEM " +
                        ////"WHERE MANDT = M.MANDT " +
                        ////"AND ORDER_REQUEST_NO = M.ORDER_REQUEST_NO) as Conf_del_date,  " +       //13 Conf_del_date
                        //"' ', " +
                        //"NVL(M.SALES_ORDER, ' '), " +                                                       //14 Sales Order no
                        //"(SELECT MIN(PLAN_DELIV_DATE) FROM SAPSR3.YMPPT_LP_ITEM_C LC, SAPSR3.YMPPT_LP_HDR LH " +
                        //"WHERE LC.MANDT = LH.MANDT " +
                        //"AND LC.LOAD_NO = LH.LOAD_NO AND LH.VEHICLE_TYPE <> 'MC' " +
                        //"AND SALES_ORDER = M.SALES_ORDER) as plan_del_date, " +                      //15 Plan del date
                        //"(SELECT MIN(K.LFDAT) FROM SAPSR3.LIPS P, SAPSR3.LIKP K " +
                        //"WHERE P.MANDT = K.MANDT " +
                        //"AND P.VBELN = K.VBELN " +
                        //"AND P.VGBEL = M.SALES_ORDER) as plan_del_conf, " +                          // 16 Plan conf del date
                        //"M.ORDER_REQUEST_NO, " +                                                     // 17 SOR
                        //"D.BBS_NO, " +
                        //"D.BBS_DESC, " +
                        //"M.KUNAG, " +
                        //"M.KUNNR, " +
                        //"M.NAME_WE " +
                        //"FROM SAPSR3.YMSDT_ORDER_HDR M LEFT OUTER JOIN SAPSR3.YMSDT_REQ_DETAIL D  " +
                        //"ON M.MANDT = D.MANDT AND M.order_request_no = D.order_request_no " +
                        //"WHERE M.MANDT = '" + lProcessObj.strClient + "' " +
                        //"AND M.KUNAG = '" + CustomerCode + "' " +
                        //"AND M.PROJECT IN (" + lProjectState + ") " +
                        //"AND M.STATUS <> 'X' " +
                        //"AND (EXISTS (SELECT VBELN FROM SAPSR3.VBAK WHERE VBELN = M.SALES_ORDER) " +
                        //"OR M.SALES_ORDER is NULL " +
                        //"OR M.SALES_ORDER = ' ') " +
                        //"AND M.REQD_DEL_DATE >= to_char(sysdate - 180, 'yyyymmdd') " +
                        //"AND M.SALES_ORDER NOT IN " +
                        //"(SELECT VBELN FROM  SAPSR3.VBUK " +
                        //"WHERE VBELN = M.SALES_ORDER AND ABSTK = 'C') " +
                        ////"AND M.ORDER_REQUEST_NO NOT IN " +
                        ////"(" + lSOR + ") " +
                        //"AND NOT EXISTS " +
                        //"(SELECT P.VGBEL FROM SAPSR3.LIPS P, SAPSR3.LIKP K " +
                        //"WHERE P.MANDT = K.MANDT " +
                        //"AND P.VBELN = K.VBELN " +
                        //"AND P.VGBEL = M.SALES_ORDER " +
                        //"AND K.wadat_ist > '00000000' ) " +
                        ////"AND K.LFDAT < TO_CHAR(SYSDATE, 'YYYYMMDD') ) " +
                        //"AND NOT EXISTS " +
                        //"(SELECT VBELN FROM SAPSR3.YMPPT_LOAD_VEHIC WHERE " +
                        //"MANDT = M.MANDT AND VBELN = M.SALES_ORDER  AND LOAD_DATE <= TO_CHAR(SYSDATE, 'YYYYMMDD')) " +
                        ////"AND (M.PO_NUMBER like '%" + PONumber + "%' " +
                        ////"OR M.PO_NUMBER = ' ' ) " +
                        //"AND ((M.CUST_ORDER_DATE >= '" + lPODateFrom_O + "' " +
                        //"AND M.CUST_ORDER_DATE <= '" + lPODateTo_O + "') " +
                        //"OR M.CUST_ORDER_DATE is NULL  " +
                        //"OR M.CUST_ORDER_DATE = ' ' ) " +
                        //"AND ((M.REQD_DEL_DATE >= '" + lRDateFrom_O + "' " +
                        //"AND M.REQD_DEL_DATE <= '" + lRDateTo_O + "') " +
                        //"OR M.REQD_DEL_DATE is NULL  " +
                        //"OR M.REQD_DEL_DATE = ' ' ) " +
                        ////"AND (D.WBS1 like '%" + WBS1 + "%' " +
                        ////"OR (D.WBS1 is null AND '" + WBS1 + "' = ' ' )) " +
                        ////"AND (D.WBS2 like '%" + WBS2 + "%' " +
                        ////"OR (D.WBS2 is null AND '" + WBS2 + "' = ' ' )) " +
                        ////"AND (D.WBS3 like '%" + WBS3 + "%' " +
                        ////"OR (D.WBS3 is null AND '" + WBS3 + "' = ' ' )) " +
                        //"AND (D.STATUS <> 'X' " +
                        //"OR D.STATUS is NULL) " +
                        //"ORDER BY 7 ";


                        //                    lSQL = "SELECT " +
                        //                            "    ISNULL(D.WBS1, ' ') AS WBS1, " +
                        //                            "    ISNULL(D.WBS2, ' ') AS WBS2, " +
                        //                            "    ISNULL(D.WBS3, ' ') AS WBS3, " +
                        //                            "    ISNULL(D.ST_ELEMENT_TYPE, ' ') AS ST_ELE, " +
                        //                            "    ISNULL(D.PRODUCT_TYPE, ' ') AS Prod_type, " +
                        //                            "    M.PO_NUMBER, " +
                        //                            "    M.CUST_ORDER_DATE, " +
                        //                            "    M.REQ_DAT_TO, " +
                        //                            "    M.FIRST_PROMISED_D, " +
                        //                            "    (SELECT ISNULL(SUM(CAST(THEO_WGT AS FLOAT)), 0)/1000 " +
                        //                            "     FROM OESOrderItemHMI " +
                        //                            "     WHERE ORDER_NO = M.SALES_ORDER) AS WT, " +
                        //                            "    '' AS SubumittedBY, " +
                        //                            "    'Reviewed' AS ReviewStatus, " +
                        //                            "    (SELECT ISNULL(MAX(PROD_TYPE), ' ') " +
                        //                            "     FROM OESOrderItemHMI " +
                        //                            "     WHERE ORDER_NO = M.SALES_ORDER) AS ProdType2, " +
                        //                            "    ' ' AS Placeholder, " +
                        //                            "    ISNULL(M.SALES_ORDER, ' ') AS SALES_ORDER, " +
                        //                            "    (SELECT TOP 1 CONVERT(varchar, CONVERT(date, PLAN_SHIPPING_DATE, 103), 112) " +
                        //                            "     FROM salesorderloading sl " +
                        //                            "     INNER JOIN hmiloaddetails ld ON ld.load_no = sl.load_no " +
                        //                            "     WHERE ld.sales_order = M.sales_order) AS PLAN_DEL_DATE, " +
                        //                            "    (SELECT CONVERT(varchar, CONVERT(date, MIN(confirmdeldate), 103), 112) " +
                        //                            "     FROM HMIOrderHeaderDetails " +
                        //                            "     WHERE OrderNo = M.sales_order) AS PLAN_DEL_CONF, " +
                        //                            "    M.ORDER_REQUEST_NO, " +
                        //                            "    D.BBS_NO, " +
                        //                            "    D.BBS_DESC, " +
                        //                            "    M.KUNAG, " +
                        //                            "    M.KUNNR, " +
                        //                            "    M.NAME_WE " +
                        //                            "    ISNULL(P.AddressCode, '') AS AddressCode " +        // ✅ NEW COLUMN FROM OesProjOrder
                        //                            "FROM OesOrderHeaderHMI M " +
                        //                            "LEFT OUTER JOIN OesRequestDetailsHMI D " +
                        //                            "    ON M.MANDT = D.MANDT AND M.ORDER_REQUEST_NO = D.ORDER_REQUEST_NO " +
                        //                              "LEFT JOIN OesProjOrder P " +                           // ✅ NEW JOIN
                        //"    ON P.OrderNumber = M.ODOS_ID " +
                        //                            "WHERE M.MANDT = '" + lProcessObj.strClient + "' " +
                        //                            "AND M.KUNAG = '" + CustomerCode + "' " +
                        //                            "AND M.PROJECT IN (" + lProjectState + ") " +
                        //                            "AND " + lAddressState +  // ✅ fixed line
                        //                            "AND M.STATUS <> 'X' " +
                        //                            "AND M.REQD_DEL_DATE >= GETDATE() - 180 " +
                        //                            "AND NOT EXISTS ( " +
                        //                            "    SELECT SALES_ORDER FROM DeliveredOrderdetailsHMI " +
                        //                            "    WHERE PARTIAL_DEL_IND = 'Completed' " +
                        //                            "    AND SALES_ORDER IN (SELECT SALES_ORDER FROM HMILoadDetails WHERE SALES_ORDER = M.SALES_ORDER) " +
                        //                            ") " +
                        //                            "AND NOT EXISTS ( " +
                        //                            "    SELECT LOAD_NO FROM SALESORDERLOADING " +
                        //                            "    WHERE TRY_CONVERT(DATETIME, PLAN_SHIPPING_DATE, 103) <= GETDATE() " +
                        //                            "    AND LOAD_NO IN (SELECT LOAD_NO FROM HMILoadDetails WHERE SALES_ORDER = M.SALES_ORDER) " +
                        //                            ") " +
                        //                            "AND ((M.CUST_ORDER_DATE BETWEEN '" + lPODateFrom_O + "' AND '" + lPODateTo_O + "') " +
                        //                            "     OR M.CUST_ORDER_DATE IS NULL " +
                        //                            "     OR M.CUST_ORDER_DATE = ' ') " +
                        //                            "AND ((M.REQD_DEL_DATE BETWEEN '" + lRDateFrom_O + "' AND '" + lRDateTo_O + "') " +
                        //                            "     OR M.REQD_DEL_DATE IS NULL " +
                        //                            "     OR M.REQD_DEL_DATE = ' ') " +
                        //                            "AND (D.STATUS <> 'X' OR D.STATUS IS NULL) " +
                        //                            "ORDER BY 7";

                        lSQL = "SELECT " +
"    ISNULL(D.WBS1, ' ') AS WBS1, " +
"    ISNULL(D.WBS2, ' ') AS WBS2, " +
"    ISNULL(D.WBS3, ' ') AS WBS3, " +
"    ISNULL(D.ST_ELEMENT_TYPE, ' ') AS ST_ELE, " +
"    ISNULL(D.PRODUCT_TYPE, ' ') AS Prod_type, " +
"    P.PO_NUMBER, " +
"    P.CUST_ORDER_DATE, " +
"    P.REQ_DAT_TO, " +
"    P.FIRST_PROMISED_D, " +
"    (SELECT ISNULL(SUM(CAST(THEO_WGT AS FLOAT)), 0)/1000 " +
"       FROM OESOrderItemHMI " +
"       WHERE ORDER_NO = P.SALES_ORDER) AS WT, " +
"    '' AS SubumittedBY, " +
"    'Reviewed' AS ReviewStatus, " +
"    (SELECT ISNULL(MAX(PROD_TYPE), ' ') " +
"       FROM OESOrderItemHMI " +
"       WHERE ORDER_NO = P.SALES_ORDER) AS ProdType2, " +
"    ' ' AS Placeholder, " +
"    ISNULL(P.SALES_ORDER, ' ') AS SALES_ORDER, " +
"    (SELECT TOP 1 CONVERT(varchar, CONVERT(date, PLAN_SHIPPING_DATE, 103), 112) " +
"       FROM salesorderloading sl " +
"       INNER JOIN hmiloaddetails ld ON ld.load_no = sl.load_no " +
"       WHERE ld.sales_order = P.sales_order) AS PLAN_DEL_DATE, " +
"    (SELECT CONVERT(varchar, CONVERT(date, MIN(confirmdeldate), 103), 112) " +
"       FROM HMIOrderHeaderDetails " +
"       WHERE OrderNo = P.sales_order) AS PLAN_DEL_CONF, " +
"    P.ORDER_REQUEST_NO, " +
"    D.BBS_NO, " +
"    D.BBS_DESC, " +
"    P.KUNAG, " +
"    P.KUNNR, " +
"    P.NAME_WE, " +
"    ISNULL(M.AddressCode, ' ') AS AddressCode, " +   // ✅ now from OesProjOrder (alias M)
"    ISNULL(M.Gate, ' ') AS Gate " +   // ✅ now from OesProjOrder (alias M)
"FROM OesProjOrder M " +
"LEFT JOIN OesOrderHeaderHMI P " +
   " ON P.ODOS_ID = M.OrderNumber " +

"LEFT JOIN OesRequestDetailsHMI D " +
"    ON P.MANDT = D.MANDT " +
 "   AND P.ORDER_REQUEST_NO = D.ORDER_REQUEST_NO " +
"WHERE P.MANDT = '" + lProcessObj.strClient + "' " +
"AND P.KUNAG = '" + CustomerCode + "' " +
"AND P.PROJECT IN (" + lProjectState + ") " +
" " + lAddressState + " " +
"AND P.STATUS <> 'X' " +
"AND P.REQD_DEL_DATE >= GETDATE() - 180 " +
"AND NOT EXISTS ( " +
"    SELECT SALES_ORDER FROM DeliveredOrderdetailsHMI " +
"    WHERE PARTIAL_DEL_IND = 'Completed' " +
"    AND SALES_ORDER IN (SELECT SALES_ORDER FROM HMILoadDetails WHERE SALES_ORDER = P.SALES_ORDER) " +
") " +
"AND NOT EXISTS ( " +
"    SELECT LOAD_NO FROM SALESORDERLOADING " +
"    WHERE TRY_CONVERT(DATETIME, PLAN_SHIPPING_DATE, 103) <= GETDATE() " +
"    AND LOAD_NO IN (SELECT LOAD_NO FROM HMILoadDetails WHERE SALES_ORDER = P.SALES_ORDER) " +
") " +
"AND ((P.CUST_ORDER_DATE BETWEEN '" + lPODateFrom_O + "' AND '" + lPODateTo_O + "') " +
"     OR P.CUST_ORDER_DATE IS NULL " +
"     OR P.CUST_ORDER_DATE = ' ') " +
"AND ((P.REQD_DEL_DATE BETWEEN '" + lRDateFrom_O + "' AND '" + lRDateTo_O + "') " +
"     OR P.REQD_DEL_DATE IS NULL " +
"     OR P.REQD_DEL_DATE = ' ') " +
"AND (D.STATUS <> 'X' OR D.STATUS IS NULL) " +
"ORDER BY 7";



                        lCmd.CommandText = lSQL;
                        lCmd.Connection = lNDSCon;
                        lCmd.CommandTimeout = 300;
                        lRst = await lCmd.ExecuteReaderAsync();
                        if (lRst.HasRows)
                        {
                            while (lRst.Read())
                            {
                                var lBBSNo = lRst.GetValue(18) == DBNull.Value ? "" : lRst.GetString(18);
                                var lBBSDesc = WebUtility.HtmlDecode(lRst.GetValue(19) == DBNull.Value ? "" : lRst.GetString(19));
                                var lSORNo = lRst.GetString(17).Trim();
                                if (lSOR.IndexOf(lSORNo) < 0)
                                {
                                    lBalSNo = lBalSNo + 1;

                                    int lConfirmed = 0;

                                    var lPODate = (lRst.GetValue(6) == DBNull.Value ? "" : lRst.GetString(6));
                                    lPODate = String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lPODate, "yyyyMMdd", CultureInfo.InvariantCulture));

                                    //var lReqDate = (lOraRst.GetValue(8) == DBNull.Value ? "" : lOraRst.GetString(8).Trim());
                                    //if (lReqDate == "")
                                    //{
                                    var lReqDate = (lRst.GetValue(7) == DBNull.Value ? "" : lRst.GetString(7).Trim());
                                    //} else
                                    //{
                                    //    if (lReqDate.Length > 10)
                                    //    {
                                    //        lReqDate = lReqDate.Substring(0, 10);
                                    //    }
                                    //    lReqDate = String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lReqDate, "yyyy-MM-dd", CultureInfo.InvariantCulture));
                                    //}

                                    //var lReqDateConf = (lOraRst.GetValue(13) == DBNull.Value ? "" : lOraRst.GetString(13).Trim());
                                    //if (lReqDateConf != null && lReqDateConf != "")
                                    //{
                                    //    lReqDate = String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lReqDateConf, "yyyyMMdd", CultureInfo.InvariantCulture));
                                    //}

                                    var lPlanDelDate = (lRst.GetValue(15) == DBNull.Value ? "" : lRst.GetString(15).Trim());
                                    if (lPlanDelDate != null && lPlanDelDate != "" && lPlanDelDate != "20500101")
                                    {
                                        if (String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lPlanDelDate, "yyyyMMdd", CultureInfo.InvariantCulture))
                                            == (String.Format("{0:yyyy-MM-dd}", DateTime.Now)))
                                        {
                                            lConfirmed = 1;
                                        }
                                        else if (String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lPlanDelDate, "yyyyMMdd", CultureInfo.InvariantCulture))
                                            == (String.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(1))))
                                        {
                                            lConfirmed = 2;
                                        }
                                        else
                                        {
                                            lConfirmed = 3;
                                        }
                                        lPlanDelDate = String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lPlanDelDate, "yyyyMMdd", CultureInfo.InvariantCulture));
                                    }
                                    else
                                    {
                                        lPlanDelDate = "";
                                    }

                                    //var lPlanDelConfDate = (lOraRst.GetValue(16) == DBNull.Value ? "" : lOraRst.GetString(16).Trim());
                                    //if (lPlanDelConfDate != null && lPlanDelConfDate != "")
                                    //{
                                    //    if (String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lPlanDelConfDate, "yyyyMMdd", CultureInfo.InvariantCulture))
                                    //        == (String.Format("{0:yyyy-MM-dd}", DateTime.Now)))
                                    //    {
                                    //        lConfirmed = 1;
                                    //    }
                                    //    else if (String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lPlanDelConfDate, "yyyyMMdd", CultureInfo.InvariantCulture))
                                    //        == (String.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(1))))
                                    //    {
                                    //        lConfirmed = 2;
                                    //    }
                                    //    else
                                    //    {
                                    //        lConfirmed = 3;
                                    //    }
                                    //    //lReqDate = String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lPlanDelConfDate, "yyyyMMdd", CultureInfo.InvariantCulture));
                                    //}

                                    if (lReqDate.Length > 10)
                                    {
                                        lReqDate = lReqDate.Substring(0, 10);
                                    }

                                    var lProdType = (lRst.GetValue(4) == DBNull.Value ? "" : lRst.GetString(4).Trim());
                                    if (lProdType.Trim() == "")
                                    {
                                        var lProdType2 = (lRst.GetValue(12) == DBNull.Value ? "" : lRst.GetString(12).Trim());
                                        if (lProdType2 == "MSHSTM")
                                        {
                                            lProdType = "STANDARD-MESH";
                                        }
                                        else if (lProdType2 == "BARDBX" || lProdType2 == "BARRBX")
                                        {
                                            lProdType = "STANDARD-BAR";
                                        }
                                        else if (lProdType2 == "BARDBI")
                                        {
                                            lProdType = "DBIC";
                                        }
                                        else if (lProdType2 == "BARRBI")
                                        {
                                            lProdType = "RBIC";
                                        }
                                        else if (lProdType2 == "COUNSP")
                                        {
                                            lProdType = "COUPLER";
                                        }
                                        else if (lProdType2 == "CRWWPR")
                                        {
                                            lProdType = "Cold Rolled Wire";
                                        }
                                        else if (lProdType2 == "PCSPCS")
                                        {
                                            lProdType = "PC Strand";
                                        }
                                        else if (lProdType2 == "WRDWRD")
                                        {
                                            lProdType = "Wire Rod";
                                        }
                                        else
                                        {
                                            lProdType = lProdType2;
                                        }
                                    }
                                    lReturn.Add(new
                                    {
                                        SSNNo = lBalSNo,
                                        OrderNo = "N" + lBalSNo.ToString(),
                                        WBS1 = (lRst.GetValue(0) == DBNull.Value ? "" : lRst.GetString(0).Trim()),
                                        WBS2 = (lRst.GetValue(1) == DBNull.Value ? "" : lRst.GetString(1).Trim()),
                                        WBS3 = (lRst.GetValue(2) == DBNull.Value ? "" : lRst.GetString(2).Trim()),
                                        StructureElement = lRst.GetValue(3) == DBNull.Value ? "" : (lRst.GetString(3).Trim() == "NONWBS" ? "" : lRst.GetString(3).Trim()),
                                        ProdType = lProdType,
                                        PONo = (lRst.GetValue(5) == DBNull.Value ? "" : lRst.GetString(5).Trim()),
                                        PODate = lPODate,
                                        RequiredDate = lReqDate,
                                        OrderWeight = (lRst.GetValue(9) == DBNull.Value ? "0.000" : lRst.GetDouble(9).ToString("###,###,##0.000;(###,##0.000); ")),
                                        SubmittedBy = (lRst.GetValue(10) == DBNull.Value ? "" : lRst.GetString(10).Trim()),
                                        OrderStatus = "Reviewed",
                                        OrderSource = "SAP",
                                        SONo = (lRst.GetValue(14) == DBNull.Value || lRst.GetString(14) == "") ? lSORNo : lRst.GetString(14).Trim(),
                                        SORNo = lSORNo,
                                        BBSNo = lBBSNo,
                                        BBSDesc = lBBSDesc,
                                        CustomerCode = (lRst.GetValue(20) == DBNull.Value ? "" : lRst.GetString(20).Trim()),
                                        ProjectCode = (lRst.GetValue(21) == DBNull.Value ? "" : lRst.GetString(21).Trim()),
                                        ProjectTitle = (lRst.GetValue(22) == DBNull.Value ? "" : lRst.GetString(22).Trim()),
                                        DataEnteredBy = "",
                                        Confirmed = lConfirmed,
                                        PlanDeliveryDate = lPlanDelDate,
                                        SubmitBy = (lRst.GetValue(18) == DBNull.Value ? "" : lRst.GetString(18).Trim()),
                                        Address = (lRst.GetValue(23) == DBNull.Value ? "" : lRst.GetString(23).Trim()),
                                        Gate = (lRst.GetValue(24) == DBNull.Value ? "" : lRst.GetString(24).Trim())
                                    });

                                }
                            }
                        }
                        lRst.Close();
                        lProcessObj.CloseNDSConnection(ref lNDSCon);
                        #endregion


                        #region Check Partial Delivery
                        lProcessObj.OpenNDSConnection(ref lNDSCon);
                        lBalSNo = 0;

                        //lSQL = "SELECT NVL(D.WBS1, ' ') as WBS1, " +
                        //"NVL(D.WBS2, ' ') as WBS2, " +
                        //"NVL(D.WBS3, ' ') as WBS3,  " +
                        //"NVL(D.ST_ELEMENT_TYPE, ' ') as ST_ELE,  " +
                        //"NVL(D.PRODUCT_TYPE, ' ') as Prod_type, " +
                        //"M.PO_NUMBER, " +
                        //"M.CUST_ORDER_DATE, " +
                        //"M.REQ_DAT_TO, " +
                        //"M.FIRST_PROMISED_D, " +

                        //"(SELECT NVL(SUM(CASE WHEN T1.MEINS = 'ST' AND T1.KWMENG > 0 THEN T1.THEO_WEIGHT_KG * C.NO_PIECES / T1.KWMENG ELSE T1.THEO_WEIGHT_KG END), 0)/1000 " +
                        //"FROM SAPSR3.YMPPT_LP_ITEM_C C, SAPSR3.YMSDT_ORDER_ITEM T1 " +
                        //"WHERE C.SALES_ORDER = M.SALES_ORDER " +
                        //"AND (C.STATUS = 'A' OR C.STATUS = 'Y') " +
                        //"AND T1.MANDT = M.MANDT " +
                        //"AND T1.ORDER_REQUEST_NO = M.ORDER_REQUEST_NO " +
                        //"AND T1.HG_ITEM_NO = '000000' " +
                        //"AND T1.ORDER_ITEM = C.ORDER_ITEM " +
                        //"AND NOT EXISTS( " +
                        //"SELECT K.VBELN FROM SAPSR3.LIPS P, SAPSR3.LIKP K  " +
                        //"WHERE P.MANDT = K.MANDT " +
                        //"AND P.VBELN = K.VBELN " +
                        //"AND P.VGBEL = M.SALES_ORDER " +
                        //"AND K.MANDT = C.MANDT " +
                        //"AND K.LIFEX = C.LOAD_NO " +
                        //"AND K.wadat_ist > '00000000') " +
                        //"AND NOT EXISTS " +
                        //"(SELECT LOAD_NO FROM SAPSR3.YMPPT_LOAD_VEHIC WHERE " +
                        //"MANDT = C.MANDT AND LOAD_NO = C.LOAD_NO AND LOAD_DATE <= TO_CHAR(SYSDATE, 'YYYYMMDD')) ) as WT, " +

                        //"'' as SubumittedBY, " +
                        //"'Production', " +
                        //"(SELECT NVL(MAX(PROD_GRP), ' ') " +
                        //"FROM SAPSR3.YMSDT_ORDER_ITEM " +
                        //"WHERE MANDT = M.MANDT " +
                        //"AND ORDER_REQUEST_NO = M.ORDER_REQUEST_NO) as ProdType2,  " +
                        //"' ', " +
                        //"NVL(M.SALES_ORDER, ' '), " +                                                       //14 Sales Order no
                        //"(SELECT MIN(PLAN_DELIV_DATE) FROM SAPSR3.YMPPT_LP_ITEM_C LC, SAPSR3.YMPPT_LP_HDR LH " +
                        //"WHERE LC.MANDT = LH.MANDT " +
                        //"AND LC.LOAD_NO = LH.LOAD_NO AND LH.VEHICLE_TYPE <> 'MC' " +
                        //"AND SALES_ORDER = M.SALES_ORDER " +
                        //"AND (LC.STATUS = 'A' OR LC.STATUS = 'Y') " +
                        //"AND NOT EXISTS( " +
                        //"SELECT K.VBELN FROM SAPSR3.LIPS P, SAPSR3.LIKP K  " +
                        //"WHERE P.MANDT = K.MANDT " +
                        //"AND P.VBELN = K.VBELN " +
                        //"AND P.VGBEL = M.SALES_ORDER " +
                        //"AND K.MANDT = LC.MANDT " +
                        //"AND K.LIFEX = LC.LOAD_NO " +
                        //"AND K.wadat_ist > '00000000') " +
                        //"AND NOT EXISTS " +
                        //"(SELECT LOAD_NO FROM SAPSR3.YMPPT_LOAD_VEHIC WHERE " +
                        //"MANDT = LC.MANDT AND LOAD_NO = LC.LOAD_NO AND LOAD_DATE <= TO_CHAR(SYSDATE, 'YYYYMMDD')) " +
                        //") as plan_del_date, " +                      //15 Plan del date
                        //"(SELECT MIN(K.LFDAT) FROM SAPSR3.LIPS P, SAPSR3.LIKP K " +
                        //"WHERE P.MANDT = K.MANDT " +
                        //"AND P.VBELN = K.VBELN " +
                        //"AND P.VGBEL = M.SALES_ORDER) as plan_del_conf, " +                          // 16 Plan conf del date
                        //"M.ORDER_REQUEST_NO, " +                                                     // 17 SOR
                        //"D.BBS_NO, " +
                        //"D.BBS_DESC, " +
                        //"M.KUNAG, " +
                        //"M.KUNNR, " +
                        //"M.NAME_WE " +
                        //"FROM SAPSR3.YMSDT_ORDER_HDR M LEFT OUTER JOIN SAPSR3.YMSDT_REQ_DETAIL D  " +
                        //"ON M.MANDT = D.MANDT AND M.order_request_no = D.order_request_no " +
                        //"WHERE M.MANDT = '" + lProcessObj.strClient + "' " +
                        //"AND M.KUNAG = '" + CustomerCode + "' " +
                        //"AND M.PROJECT IN (" + lProjectState + ") " +
                        //"AND M.STATUS <> 'X' " +
                        //"AND (EXISTS (SELECT VBELN FROM SAPSR3.VBAK WHERE VBELN = M.SALES_ORDER) " +
                        //"OR M.SALES_ORDER is NULL " +
                        //"OR M.SALES_ORDER = ' ') " +
                        //"AND M.REQD_DEL_DATE >= to_char(sysdate - 180, 'yyyymmdd') " +
                        //"AND M.SALES_ORDER NOT IN " +
                        //"(SELECT VBELN FROM  SAPSR3.VBUK " +
                        //"WHERE MANDT = M.MANDT AND VBELN = M.SALES_ORDER AND ABSTK = 'C') " +
                        ////"AND M.ORDER_REQUEST_NO NOT IN " +
                        ////"(" + lSOR + ") " +
                        //"AND EXISTS " +
                        //"(SELECT LOAD_NO FROM SAPSR3.YMPPT_LP_ITEM_C C " +
                        //"WHERE C.SALES_ORDER = M.SALES_ORDER " +
                        //"AND (C.STATUS = 'A' OR C.STATUS = 'Y') " +
                        //"AND LOAD_DATE >= TO_CHAR(SYSDATE - 10, 'YYYYMMDD') " +
                        //"AND EXISTS " +
                        //"(SELECT LOAD_NO FROM SAPSR3.YMPPT_LOAD_VEHIC WHERE " +
                        //"MANDT = C.MANDT AND LOAD_NO = C.LOAD_NO ) ) " +
                        //"AND EXISTS " +
                        //"(SELECT LOAD_NO FROM SAPSR3.YMPPT_LP_ITEM_C C " +
                        //"WHERE C.SALES_ORDER = M.SALES_ORDER " +
                        //"AND (C.STATUS = 'A' OR C.STATUS = 'Y') " +
                        //"AND LOAD_DATE <= TO_CHAR(SYSDATE + 30, 'YYYYMMDD') " +
                        //"AND (NOT EXISTS " +
                        //"(SELECT K.VBELN FROM SAPSR3.LIPS P, SAPSR3.LIKP K " +
                        //"WHERE P.MANDT = K.MANDT " +
                        //"AND P.VBELN = K.VBELN " +
                        //"AND P.VGBEL = C.SALES_ORDER " +
                        //"AND P.VGPOS = C.CLBD_ITEM " +
                        //"AND K.wadat_ist > '00000000') " +
                        //"OR EXISTS " +
                        //"(SELECT P.VBELN FROM SAPSR3.VBUP P " +
                        //"WHERE P.MANDT = C.MANDT " +
                        //"AND P.VBELN = C.SALES_ORDER " +
                        //"AND P.POSNR = C.CLBD_ITEM " +
                        //"AND P.LFSTA = 'B')) " +

                        //"AND NOT EXISTS " +
                        //"(SELECT LOAD_NO FROM SAPSR3.YMPPT_LOAD_VEHIC WHERE " +
                        //"MANDT = C.MANDT AND LOAD_NO = C.LOAD_NO AND LOAD_DATE <= TO_CHAR(SYSDATE, 'YYYYMMDD') ) ) " +
                        ////"AND (M.PO_NUMBER like '%" + PONumber + "%' " +
                        ////"OR M.PO_NUMBER = ' ' ) " +
                        //"AND ((M.CUST_ORDER_DATE >= '" + lPODateFrom_O + "' " +
                        //"AND M.CUST_ORDER_DATE <= '" + lPODateTo_O + "') " +
                        //"OR M.CUST_ORDER_DATE is NULL  " +
                        //"OR M.CUST_ORDER_DATE = ' ' ) " +
                        //"AND ((M.REQD_DEL_DATE >= '" + lRDateFrom_O + "' " +
                        //"AND M.REQD_DEL_DATE <= '" + lRDateTo_O + "') " +
                        //"OR M.REQD_DEL_DATE is NULL  " +
                        //"OR M.REQD_DEL_DATE = ' ' ) " +
                        ////"AND (D.WBS1 like '%" + WBS1 + "%' " +
                        ////"OR (D.WBS1 is null AND '" + WBS1 + "' = ' ' )) " +
                        ////"AND (D.WBS2 like '%" + WBS2 + "%' " +
                        ////"OR (D.WBS2 is null AND '" + WBS2 + "' = ' ' )) " +
                        ////"AND (D.WBS3 like '%" + WBS3 + "%' " +
                        ////"OR (D.WBS3 is null AND '" + WBS3 + "' = ' ' )) " +
                        //"AND (D.STATUS <> 'X' " +
                        //"OR D.STATUS is NULL) " +
                        //"ORDER BY 7 ";

                        lSQL = "SELECT " +
                                "D.WBS1, " +
                                "D.WBS2, " +
                                "D.WBS3, " +
                                "D.ST_ELEMENT_TYPE, " +
                                "D.PRODUCT_TYPE, " +
                                "M.PO_NUMBER, " +
                                "M.CUST_ORDER_DATE, " +
                                "M.REQ_DAT_TO, " +
                                "M.FIRST_PROMISED_D, " +
                                "(SELECT ISNULL(SUM(CAST(THEO_WGT AS FLOAT)), 0)/1000 " +
                                " FROM OESOrderItemHMI WHERE ORDER_NO = M.SALES_ORDER) AS WT, " +
                                "'' AS SubumittedBY, " +
                                "'Production', " +
                                "(SELECT ISNULL(MAX(PROD_TYPE), ' ') FROM OESOrderItemHMI WHERE ORDER_NO = M.SALES_ORDER) AS ProdType2, " +
                                "' ', " +
                                "M.SALES_ORDER, " +
                                "(SELECT TOP 1 CONVERT(varchar, CONVERT(date, PLAN_SHIPPING_DATE, 103), 112) " +
                                " FROM salesorderloading sl " +
                                " INNER JOIN hmiloaddetails ld ON ld.load_no = sl.load_no " +
                                " WHERE ld.sales_order = M.sales_order) AS PLAN_DEL_DATE, " +
                                "(SELECT CONVERT(varchar, CONVERT(date, MIN(confirmdeldate), 103), 112) FROM HMIOrderHeaderDetails WHERE OrderNo = M.sales_order) AS PLAN_DEL_CONF, " +
                                "M.ORDER_REQUEST_NO, " +
                                "D.BBS_NO, " +
                                "D.BBS_DESC, " +
                                "M.KUNAG, " +
                                "M.KUNNR, " +
                                "M.NAME_WE " +

                                "FROM OesOrderHeaderHMI M " +
                                "LEFT OUTER JOIN OesRequestDetailsHMI D " +
                                "ON M.MANDT = D.MANDT AND M.ORDER_REQUEST_NO = D.ORDER_REQUEST_NO " +
                                "WHERE M.MANDT = '" + lProcessObj.strClient + "' " +
                                "AND M.KUNAG = '" + CustomerCode + "' " +
                                "AND M.PROJECT IN (" + lProjectState + ") " +
                                "AND M.REQD_DEL_DATE >= GETDATE() - 180 " +
                                "AND M.STATUS <> 'X' " +

                                "AND EXISTS ( " +
                                " SELECT LOAD_NO FROM SALESORDERLOADING " +
                                " WHERE TRY_CONVERT(DATETIME, PLAN_SHIPPING_DATE, 103) <= GETDATE() - 10 " +
                                " AND LOAD_NO IN (SELECT LOAD_NO FROM HMILoadDetails WHERE SALES_ORDER = M.SALES_ORDER) " +
                                ") " +

                                "AND EXISTS ( " +
                                " SELECT SALES_ORDER FROM DeliveredOrderdetailsHMI " +
                                " WHERE PARTIAL_DEL_IND <> 'Completed' " +
                                " AND LOAD_NO IN ( " +
                                "     SELECT LOAD_NO FROM SALESORDERLOADING " +
                                "     WHERE TRY_CONVERT(DATETIME, PLAN_SHIPPING_DATE, 103) <= GETDATE() + 30 " +
                                "     AND LOAD_NO IN (SELECT LOAD_NO FROM HMILoadDetails WHERE SALES_ORDER = M.SALES_ORDER) " +
                                " )) " +

                                "AND ( " +
                                " (M.CUST_ORDER_DATE >= '" + lPODateFrom_O + "' AND M.CUST_ORDER_DATE <= '" + lPODateTo_O + "') " +
                                " OR M.CUST_ORDER_DATE IS NULL " +
                                " OR M.CUST_ORDER_DATE = ' ' " +
                                ") " +

                                "AND ( " +
                                " (M.REQD_DEL_DATE >= '" + lRDateFrom_O + "' AND M.REQD_DEL_DATE <= '" + lRDateTo_O + "') " +
                                " OR M.REQD_DEL_DATE IS NULL " +
                                " OR M.REQD_DEL_DATE = ' ' " +
                                ") " +

                                "AND (D.STATUS <> 'X' OR D.STATUS IS NULL)";


                        lCmd.CommandText = lSQL;
                        lCmd.Connection = lNDSCon;
                        lCmd.CommandTimeout = 300;
                        lRst = await lCmd.ExecuteReaderAsync();
                        if (lRst.HasRows)
                        {
                            while (lRst.Read())
                            {
                                var lBBSNo = lRst.GetValue(18) == DBNull.Value ? "" : lRst.GetString(18);
                                var lBBSDesc = WebUtility.HtmlDecode(lRst.GetValue(19) == DBNull.Value ? "" : lRst.GetString(19));
                                var lSORNo = lRst.GetString(17).Trim();
                                int lInd = lReturn.FindIndex(f => f.SONo == lSORNo);
                                if (lInd < 0)
                                {
                                    lBalSNo = lBalSNo + 1;

                                    int lConfirmed = 0;

                                    var lPODate = (lRst.GetValue(6) == DBNull.Value ? "" : lRst.GetString(6));
                                    lPODate = String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lPODate, "yyyyMMdd", CultureInfo.InvariantCulture));

                                    //var lReqDate = (lRst.GetValue(8) == DBNull.Value ? "" : lRst.GetString(8).Trim());
                                    //if (lReqDate == "")
                                    //{
                                    var lReqDate = (lRst.GetValue(7) == DBNull.Value ? "" : lRst.GetString(7).Trim());
                                    //} else
                                    //{
                                    //    if (lReqDate.Length > 10)
                                    //    {
                                    //        lReqDate = lReqDate.Substring(0, 10);
                                    //    }
                                    //    lReqDate = String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lReqDate, "yyyy-MM-dd", CultureInfo.InvariantCulture));
                                    //}

                                    //var lReqDateConf = (lRst.GetValue(13) == DBNull.Value ? "" : lRst.GetString(13).Trim());
                                    //if (lReqDateConf != null && lReqDateConf != "")
                                    //{
                                    //    lReqDate = String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lReqDateConf, "yyyyMMdd", CultureInfo.InvariantCulture));
                                    //}

                                    var lPlanDelDate = (lRst.GetValue(15) == DBNull.Value ? "" : lRst.GetString(15).Trim());
                                    if (lPlanDelDate != null && lPlanDelDate != "" && lPlanDelDate != "20500101")
                                    {
                                        if (String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lPlanDelDate, "yyyyMMdd", CultureInfo.InvariantCulture))
                                            == (String.Format("{0:yyyy-MM-dd}", DateTime.Now)))
                                        {
                                            lConfirmed = 1;
                                        }
                                        else if (String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lPlanDelDate, "yyyyMMdd", CultureInfo.InvariantCulture))
                                            == (String.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(1))))
                                        {
                                            lConfirmed = 2;
                                        }
                                        else
                                        {
                                            lConfirmed = 3;
                                        }
                                        lPlanDelDate = String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lPlanDelDate, "yyyyMMdd", CultureInfo.InvariantCulture));
                                    }
                                    else
                                    {
                                        lPlanDelDate = "";
                                    }

                                    //var lPlanDelConfDate = (lOraRst.GetValue(16) == DBNull.Value ? "" : lOraRst.GetString(16).Trim());
                                    //if (lPlanDelConfDate != null && lPlanDelConfDate != "")
                                    //{
                                    //    if (String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lPlanDelConfDate, "yyyyMMdd", CultureInfo.InvariantCulture))
                                    //        == (String.Format("{0:yyyy-MM-dd}", DateTime.Now)))
                                    //    {
                                    //        lConfirmed = 1;
                                    //    }
                                    //    else if (String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lPlanDelConfDate, "yyyyMMdd", CultureInfo.InvariantCulture))
                                    //        == (String.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(1))))
                                    //    {
                                    //        lConfirmed = 2;
                                    //    }
                                    //    else
                                    //    {
                                    //        lConfirmed = 3;
                                    //    }
                                    //    //lReqDate = String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lPlanDelConfDate, "yyyyMMdd", CultureInfo.InvariantCulture));
                                    //}

                                    if (lReqDate.Length > 10)
                                    {
                                        lReqDate = lReqDate.Substring(0, 10);
                                    }

                                    var lProdType = (lRst.GetValue(4) == DBNull.Value ? "" : lRst.GetString(4).Trim());
                                    if (lProdType.Trim() == "")
                                    {
                                        var lProdType2 = (lRst.GetValue(12) == DBNull.Value ? "" : lRst.GetString(12).Trim());
                                        if (lProdType2 == "MSHSTM")
                                        {
                                            lProdType = "STANDARD-MESH";
                                        }
                                        else if (lProdType2 == "BARDBX" || lProdType2 == "BARRBX")
                                        {
                                            lProdType = "STANDARD-BAR";
                                        }
                                        else if (lProdType2 == "BARDBI")
                                        {
                                            lProdType = "DBIC";
                                        }
                                        else if (lProdType2 == "BARRBI")
                                        {
                                            lProdType = "RBIC";
                                        }
                                        else if (lProdType2 == "COUNSP")
                                        {
                                            lProdType = "COUPLER";
                                        }
                                        else if (lProdType2 == "CRWWPR")
                                        {
                                            lProdType = "Cold Rolled Wire";
                                        }
                                        else if (lProdType2 == "PCSPCS")
                                        {
                                            lProdType = "PC Strand";
                                        }
                                        else if (lProdType2 == "WRDWRD")
                                        {
                                            lProdType = "Wire Rod";
                                        }
                                        else
                                        {
                                            lProdType = lProdType2;
                                        }
                                    }
                                    lReturn.Add(new
                                    {
                                        SSNNo = lBalSNo,
                                        OrderNo = "P" + lBalSNo.ToString(),
                                        WBS1 = (lRst.GetValue(0) == DBNull.Value ? "" : lRst.GetString(0).Trim()),
                                        WBS2 = (lRst.GetValue(1) == DBNull.Value ? "" : lRst.GetString(1).Trim()),
                                        WBS3 = (lRst.GetValue(2) == DBNull.Value ? "" : lRst.GetString(2).Trim()),
                                        StructureElement = lRst.GetValue(3) == DBNull.Value ? "" : (lRst.GetString(3).Trim() == "NONWBS" ? "" : lRst.GetString(3).Trim()),
                                        ProdType = lProdType,
                                        PONo = (lRst.GetValue(5) == DBNull.Value ? "" : lRst.GetString(5).Trim()),
                                        PODate = lPODate,
                                        RequiredDate = lReqDate,
                                        OrderWeight = (lRst.GetValue(9) == DBNull.Value ? "0.000" :
                   decimal.TryParse(lRst.GetValue(9).ToString(), out var decVal) ?
                   decVal.ToString("###,###,##0.000;(###,##0.000); ") : "0.000"),
                                        SubmittedBy = (lRst.GetValue(10) == DBNull.Value ? "" : lRst.GetString(10).Trim()),
                                        OrderStatus = lSORNo.Substring(0, 1) == "1" ? "Reviewed" : "Production",
                                        OrderSource = "SAP",
                                        SONo = lRst.GetValue(14) == DBNull.Value ? "" : lRst.GetString(14).Trim(),
                                        SORNo = lSORNo,
                                        BBSNo = lBBSNo,
                                        BBSDesc = lBBSDesc,
                                        CustomerCode = (lRst.GetValue(20) == DBNull.Value ? "" : lRst.GetString(20).Trim()),
                                        ProjectCode = (lRst.GetValue(21) == DBNull.Value ? "" : lRst.GetString(21).Trim()),
                                        ProjectTitle = (lRst.GetValue(22) == DBNull.Value ? "" : lRst.GetString(22).Trim()),
                                        DataEnteredBy = "",
                                        Confirmed = lConfirmed,
                                        PlanDeliveryDate = lPlanDelDate,
                                        SubmitBy = (lRst.GetValue(18) == DBNull.Value ? "" : lRst.GetString(18).Trim()),
                                        Address = (lRst.GetValue(19) == DBNull.Value ? "" : lRst.GetString(19).Trim()),
                                        Gate = (lRst.GetValue(20) == DBNull.Value ? "" : lRst.GetString(20).Trim())
                                    });

                                }
                            }
                        }
                        lRst.Close();
                        lProcessObj.CloseNDSConnection(ref lNDSCon);
                        #endregion

                        //Check Production Status
                        var lSTSCon = new SqlConnection();
                        var lSOs = "";
                        var lTagSO = new List<string>();

                        try
                        {
                            if (lProcessObj.OpenSTSConnection(ref lSTSCon) == true)
                            {

                                #region CAB
                                if (lReturn.Count > 0)
                                {
                                    for (int i = 0; i < lReturn.Count; i++)
                                    {
                                        if (lReturn[i].SONo != null && lReturn[i].SONo != "" &&
                                            lReturn[i].OrderSource == "SAP" &&
                                            lReturn[i].ProdType == "CAB")
                                        {
                                            if (lSOs == "")
                                            {
                                                lSOs = "'" + lReturn[i].SONo + "'";
                                            }
                                            else
                                            {
                                                lSOs = lSOs + "," + "'" + lReturn[i].SONo + "'";
                                            }
                                        }
                                    }
                                }

                                if (lSOs != null && lSOs != "")
                                {
                                    lSQL = "SELECT ORDER_NO " +
                                    "FROM dbo.tbl_scm_sts_cab_print_tag " +
                                    "WHERE ORDER_NO IN (" + lSOs + ") " +
                                    "GROUP BY ORDER_NO ";

                                    lCmd.CommandText = lSQL;
                                    lCmd.Connection = lSTSCon;
                                    lCmd.CommandTimeout = 300;
                                    lRst = lCmd.ExecuteReader();
                                    if (lRst.HasRows)
                                    {
                                        while (lRst.Read())
                                        {
                                            if (lRst.GetString(0).Trim() != "")
                                            {
                                                lTagSO.Add(lRst.GetString(0).Trim());
                                            }

                                        }
                                    }
                                    lRst.Close();

                                    for (int i = 0; i < lTagSO.Count; i++)
                                    {
                                        for (int j = 0; j < lReturn.Count; j++)
                                        {
                                            if (lTagSO[i] == lReturn[j].SONo)
                                            {
                                                lReturn.Insert(j + 1, new
                                                {
                                                    SSNNo = lReturn[j].SSNNo,
                                                    OrderNo = lReturn[j].OrderNo,
                                                    WBS1 = lReturn[j].WBS1,
                                                    WBS2 = lReturn[j].WBS2,
                                                    WBS3 = lReturn[j].WBS3,
                                                    StructureElement = lReturn[j].StructureElement,
                                                    ProdType = lReturn[j].ProdType,
                                                    PONo = lReturn[j].PONo,
                                                    PODate = lReturn[j].PODate,
                                                    RequiredDate = lReturn[j].RequiredDate,
                                                    OrderWeight = lReturn[j].OrderWeight,
                                                    SubmittedBy = lReturn[j].SubmittedBy,
                                                    OrderStatus = "Production",
                                                    OrderSource = lReturn[j].OrderSource,
                                                    SONo = lReturn[j].SONo,
                                                    SORNo = lReturn[j].SORNo,
                                                    BBSNo = lReturn[j].BBSNo,
                                                    BBSDesc = lReturn[j].BBSDesc,
                                                    CustomerCode = lReturn[j].CustomerCode,
                                                    ProjectCode = lReturn[j].ProjectCode,
                                                    ProjectTitle = lReturn[j].ProjectTitle,
                                                    DataEnteredBy = lReturn[j].DataEnteredBy,
                                                    Confirmed = lReturn[j].Confirmed,
                                                    PlanDeliveryDate = lReturn[j].PlanDeliveryDate,
                                                    SubmitBy = lReturn[j].SubmitBy,
                                                    Address = lReturn[j].Address,
                                                    Gate = lReturn[j].Gate
                                                });
                                                lReturn.RemoveAt(j);
                                                break;
                                            }
                                        }
                                    }
                                }
                                #endregion

                                #region MESH
                                lSOs = "";
                                lTagSO = new List<string>();

                                if (lReturn.Count > 0)
                                {
                                    for (int i = 0; i < lReturn.Count; i++)
                                    {
                                        if (lReturn[i].SONo != null && lReturn[i].SONo != "" &&
                                            lReturn[i].OrderSource == "SAP" &&
                                            (lReturn[i].ProdType == "CUT-TO-SIZE-MESH" ||
                                            lReturn[i].ProdType == "STIRRUP-LINK-MESH" ||
                                            lReturn[i].ProdType == "COLUMN-LINK-MESH")
                                            )
                                        {
                                            if (lSOs == "")
                                            {
                                                lSOs = "'" + lReturn[i].SONo + "'";
                                            }
                                            else
                                            {
                                                lSOs = lSOs + "," + "'" + lReturn[i].SONo + "'";
                                            }
                                        }
                                    }
                                }

                                if (lSOs != null && lSOs != "")
                                {
                                    lSQL = "SELECT MSD_PRT_SO_NO " +
                                    "FROM dbo.tbl_scm_sts_mesh_print_tag " +
                                    "WHERE MSD_PRT_SO_NO IN (" + lSOs + ") " +
                                    "GROUP BY MSD_PRT_SO_NO ";

                                    lCmd.CommandText = lSQL;
                                    lCmd.Connection = lSTSCon;
                                    lCmd.CommandTimeout = 300;
                                    lRst = lCmd.ExecuteReader();
                                    if (lRst.HasRows)
                                    {
                                        while (lRst.Read())
                                        {
                                            if (lRst.GetString(0).Trim() != "")
                                            {
                                                lTagSO.Add(lRst.GetString(0).Trim());
                                            }

                                        }
                                    }
                                    lRst.Close();

                                    for (int i = 0; i < lTagSO.Count; i++)
                                    {
                                        for (int j = 0; j < lReturn.Count; j++)
                                        {
                                            if (lTagSO[i] == lReturn[j].SONo)
                                            {
                                                lReturn.Insert(j + 1, new
                                                {
                                                    SSNNo = lReturn[j].SSNNo,
                                                    OrderNo = lReturn[j].OrderNo,
                                                    WBS1 = lReturn[j].WBS1,
                                                    WBS2 = lReturn[j].WBS2,
                                                    WBS3 = lReturn[j].WBS3,
                                                    StructureElement = lReturn[j].StructureElement,
                                                    ProdType = lReturn[j].ProdType,
                                                    PONo = lReturn[j].PONo,
                                                    PODate = lReturn[j].PODate,
                                                    RequiredDate = lReturn[j].RequiredDate,
                                                    OrderWeight = lReturn[j].OrderWeight,
                                                    SubmittedBy = lReturn[j].SubmittedBy,
                                                    OrderStatus = "Production",
                                                    OrderSource = lReturn[j].OrderSource,
                                                    SONo = lReturn[j].SONo,
                                                    SORNo = lReturn[j].SORNo,
                                                    BBSNo = lReturn[j].BBSNo,
                                                    BBSDesc = lReturn[j].BBSDesc,
                                                    CustomerCode = lReturn[j].CustomerCode,
                                                    ProjectCode = lReturn[j].ProjectCode,
                                                    ProjectTitle = lReturn[j].ProjectTitle,
                                                    DataEnteredBy = lReturn[j].DataEnteredBy,
                                                    Confirmed = lReturn[j].Confirmed,
                                                    PlanDeliveryDate = lReturn[j].PlanDeliveryDate,
                                                    SubmitBy = lReturn[j].SubmitBy,
                                                    Address = lReturn[j].Address,
                                                    Gate = lReturn[j].Gate
                                                });
                                                lReturn.RemoveAt(j);
                                                break;
                                            }

                                        }
                                    }
                                }
                                #endregion

                                #region PRE-CAGE
                                lSOs = "";
                                lTagSO = new List<string>();

                                if (lReturn.Count > 0)
                                {
                                    for (int i = 0; i < lReturn.Count; i++)
                                    {
                                        if (lReturn[i].SONo != null && lReturn[i].SONo != "" &&
                                            lReturn[i].OrderSource == "SAP" &&
                                            (lReturn[i].ProdType == "CORE-CAGE" ||
                                            lReturn[i].ProdType == "PRE-CAGE")
                                            )
                                        {
                                            if (lSOs == "")
                                            {
                                                lSOs = "'" + lReturn[i].SONo + "'";
                                            }
                                            else
                                            {
                                                lSOs = lSOs + "," + "'" + lReturn[i].SONo + "'";
                                            }
                                        }
                                    }
                                }

                                if (lSOs != null && lSOs != "")
                                {
                                    lSQL = "SELECT SO_NO " +
                                    "FROM dbo.tbl_scm_sts_prc_print_tag " +
                                    "WHERE SO_NO IN (" + lSOs + ") " +
                                    "GROUP BY SO_NO ";

                                    lCmd.CommandText = lSQL;
                                    lCmd.Connection = lSTSCon;
                                    lCmd.CommandTimeout = 300;
                                    lRst = lCmd.ExecuteReader();
                                    if (lRst.HasRows)
                                    {
                                        while (lRst.Read())
                                        {
                                            if (lRst.GetString(0).Trim() != "")
                                            {
                                                lTagSO.Add(lRst.GetString(0).Trim());
                                            }

                                        }
                                    }
                                    lRst.Close();

                                    for (int i = 0; i < lTagSO.Count; i++)
                                    {
                                        for (int j = 0; j < lReturn.Count; j++)
                                        {
                                            if (lTagSO[i] == lReturn[j].SONo)
                                            {
                                                lReturn.Insert(j + 1, new
                                                {
                                                    SSNNo = lReturn[j].SSNNo,
                                                    OrderNo = lReturn[j].OrderNo,
                                                    WBS1 = lReturn[j].WBS1,
                                                    WBS2 = lReturn[j].WBS2,
                                                    WBS3 = lReturn[j].WBS3,
                                                    StructureElement = lReturn[j].StructureElement,
                                                    ProdType = lReturn[j].ProdType,
                                                    PONo = lReturn[j].PONo,
                                                    PODate = lReturn[j].PODate,
                                                    RequiredDate = lReturn[j].RequiredDate,
                                                    OrderWeight = lReturn[j].OrderWeight,
                                                    SubmittedBy = lReturn[j].SubmittedBy,
                                                    OrderStatus = "Production",
                                                    OrderSource = lReturn[j].OrderSource,
                                                    SONo = lReturn[j].SONo,
                                                    SORNo = lReturn[j].SORNo,
                                                    BBSNo = lReturn[j].BBSNo,
                                                    BBSDesc = lReturn[j].BBSDesc,
                                                    CustomerCode = lReturn[j].CustomerCode,
                                                    ProjectCode = lReturn[j].ProjectCode,
                                                    ProjectTitle = lReturn[j].ProjectTitle,
                                                    DataEnteredBy = lReturn[j].DataEnteredBy,
                                                    Confirmed = lReturn[j].Confirmed,
                                                    PlanDeliveryDate = lReturn[j].PlanDeliveryDate,
                                                    SubmitBy = lReturn[j].SubmitBy,
                                                    Address = lReturn[j].Address,
                                                    Gate = lReturn[j].Gate
                                                });
                                                lReturn.RemoveAt(j);
                                                break;
                                            }

                                        }
                                    }
                                }
                                #endregion

                                #region BPC
                                lSOs = "";
                                lTagSO = new List<string>();

                                if (lReturn.Count > 0)
                                {
                                    for (int i = 0; i < lReturn.Count; i++)
                                    {
                                        if (lReturn[i].SONo != null && lReturn[i].SONo != "" &&
                                            lReturn[i].OrderSource == "SAP" &&
                                            lReturn[i].ProdType == "BPC")
                                        {

                                            if (lSOs == "")
                                            {
                                                lSOs = "'" + lReturn[i].SONo + "'";
                                            }
                                            else
                                            {
                                                lSOs = lSOs + "," + "'" + lReturn[i].SONo + "'";
                                            }
                                        }
                                    }
                                }

                                if (lSOs != null && lSOs != "")
                                {
                                    lSQL = "SELECT fld_bpc_so_no " +
                                    "FROM dbo.tbl_scm_sts_bpc_print_tag " +
                                    "WHERE fld_bpc_so_no IN (" + lSOs + ") " +
                                    "GROUP BY fld_bpc_so_no ";

                                    lCmd.CommandText = lSQL;
                                    lCmd.Connection = lSTSCon;
                                    lCmd.CommandTimeout = 300;
                                    lRst = lCmd.ExecuteReader();
                                    if (lRst.HasRows)
                                    {
                                        while (lRst.Read())
                                        {
                                            if (lRst.GetString(0).Trim() != "")
                                            {
                                                lTagSO.Add(lRst.GetString(0).Trim());
                                            }

                                        }
                                    }
                                    lRst.Close();

                                    for (int i = 0; i < lTagSO.Count; i++)
                                    {
                                        for (int j = 0; j < lReturn.Count; j++)
                                        {
                                            if (lTagSO[i] == lReturn[j].SONo)
                                            {
                                                lReturn.Insert(j + 1, new
                                                {
                                                    SSNNo = lReturn[j].SSNNo,
                                                    OrderNo = lReturn[j].OrderNo,
                                                    WBS1 = lReturn[j].WBS1,
                                                    WBS2 = lReturn[j].WBS2,
                                                    WBS3 = lReturn[j].WBS3,
                                                    StructureElement = lReturn[j].StructureElement,
                                                    ProdType = lReturn[j].ProdType,
                                                    PONo = lReturn[j].PONo,
                                                    PODate = lReturn[j].PODate,
                                                    RequiredDate = lReturn[j].RequiredDate,
                                                    OrderWeight = lReturn[j].OrderWeight,
                                                    SubmittedBy = lReturn[j].SubmittedBy,
                                                    OrderStatus = "Production",
                                                    OrderSource = lReturn[j].OrderSource,
                                                    SONo = lReturn[j].SONo,
                                                    SORNo = lReturn[j].SORNo,
                                                    BBSNo = lReturn[j].BBSNo,
                                                    BBSDesc = lReturn[j].BBSDesc,
                                                    CustomerCode = lReturn[j].CustomerCode,
                                                    ProjectCode = lReturn[j].ProjectCode,
                                                    ProjectTitle = lReturn[j].ProjectTitle,
                                                    DataEnteredBy = lReturn[j].DataEnteredBy,
                                                    Confirmed = lReturn[j].Confirmed,
                                                    PlanDeliveryDate = lReturn[j].PlanDeliveryDate,
                                                    SubmitBy = lReturn[j].SubmitBy,
                                                    Address = lReturn[j].Address,
                                                    Gate = lReturn[j].Gate
                                                });
                                                lReturn.RemoveAt(j);
                                                break;
                                            }

                                        }
                                    }
                                }
                                #endregion

                                #region CARPET
                                lSOs = "";
                                lTagSO = new List<string>();

                                if (lReturn.Count > 0)
                                {
                                    for (int i = 0; i < lReturn.Count; i++)
                                    {
                                        if (lReturn[i].SONo != null && lReturn[i].SONo != "" &&
                                            lReturn[i].OrderSource == "SAP" &&
                                            lReturn[i].ProdType == "CARPET")
                                        {
                                            if (lSOs == "")
                                            {
                                                lSOs = "'" + lReturn[i].SONo + "'";
                                            }
                                            else
                                            {
                                                lSOs = lSOs + "," + "'" + lReturn[i].SONo + "'";
                                            }
                                        }
                                    }
                                }

                                if (lSOs != null && lSOs != "")
                                {
                                    lSQL = "SELECT SO_NO " +
                                    "FROM dbo.tbl_scm_sts_carpet_print_tag " +
                                    "WHERE SO_NO IN (" + lSOs + ") " +
                                    "GROUP BY SO_NO ";

                                    lCmd.CommandText = lSQL;
                                    lCmd.Connection = lSTSCon;
                                    lCmd.CommandTimeout = 300;
                                    lRst = lCmd.ExecuteReader();
                                    if (lRst.HasRows)
                                    {
                                        while (lRst.Read())
                                        {
                                            if (lRst.GetString(0).Trim() != "")
                                            {
                                                lTagSO.Add(lRst.GetString(0).Trim());
                                            }

                                        }
                                    }
                                    lRst.Close();

                                    for (int i = 0; i < lTagSO.Count; i++)
                                    {
                                        for (int j = 0; j < lReturn.Count; j++)
                                        {
                                            if (lTagSO[i] == lReturn[j].SONo)
                                            {
                                                lReturn.Insert(j + 1, new
                                                {
                                                    SSNNo = lReturn[j].SSNNo,
                                                    OrderNo = lReturn[j].OrderNo,
                                                    WBS1 = lReturn[j].WBS1,
                                                    WBS2 = lReturn[j].WBS2,
                                                    WBS3 = lReturn[j].WBS3,
                                                    StructureElement = lReturn[j].StructureElement,
                                                    ProdType = lReturn[j].ProdType,
                                                    PONo = lReturn[j].PONo,
                                                    PODate = lReturn[j].PODate,
                                                    RequiredDate = lReturn[j].RequiredDate,
                                                    OrderWeight = lReturn[j].OrderWeight,
                                                    SubmittedBy = lReturn[j].SubmittedBy,
                                                    OrderStatus = "Production",
                                                    OrderSource = lReturn[j].OrderSource,
                                                    SONo = lReturn[j].SONo,
                                                    SORNo = lReturn[j].SORNo,
                                                    BBSNo = lReturn[j].BBSNo,
                                                    BBSDesc = lReturn[j].BBSDesc,
                                                    CustomerCode = lReturn[j].CustomerCode,
                                                    ProjectCode = lReturn[j].ProjectCode,
                                                    ProjectTitle = lReturn[j].ProjectTitle,
                                                    DataEnteredBy = lReturn[j].DataEnteredBy,
                                                    Confirmed = lReturn[j].Confirmed,
                                                    PlanDeliveryDate = lReturn[j].PlanDeliveryDate,
                                                    SubmitBy = lReturn[j].SubmitBy,
                                                    Address = lReturn[j].Address,
                                                    Gate = lReturn[j].Gate
                                                });
                                                lReturn.RemoveAt(j);
                                                break;
                                            }

                                        }
                                    }
                                }
                                #endregion

                                lProcessObj.CloseSTSConnection(ref lSTSCon);
                            }
                        }
                        catch (Exception ex)
                        {
                            lProcessObj.SaveErrorMsg(ex.Message, ex.StackTrace);
                            string lerrorMsg = ex.Message;
                        }


                        //lProcessObj.CloseCISConnection(ref lCISCon);

                        #region Check Plan Delivery Date From IDB
                        if (lReturn.Count > 0)
                        {
                            var lSONos = "";
                            for (int i = 0; i < lReturn.Count; i++)
                            {
                                var lSOARR = lReturn[i].SORNo.Split(',');
                                if (lSOARR.Length > 0 && (lReturn[i].PlanDeliveryDate == null || lReturn[i].PlanDeliveryDate == ""))
                                    for (int j = 0; j < lSOARR.Length; j++)
                                    {
                                        if (lSOARR[j].Trim().Length > 0)
                                        {
                                            if (lSONos == "")
                                            {
                                                lSONos = "'" + lSOARR[j].Trim() + "'";
                                            }
                                            else
                                            {
                                                lSONos = lSONos + ",'" + lSOARR[j].Trim() + "'";
                                            }
                                        }
                                    }
                            }
                            if (lSONos != "")
                            {
                                try
                                {
                                    lProcessObj.OpenIDBConnection(ref lIDBCon);

                                    var lSORList = new List<string>();

                                    var lSORA = lSONos.Split(',').ToList();
                                    var lSOR1 = "";
                                    if (lSORA.Count > 0)
                                    {
                                        int lCount = 0;
                                        for (int i = 0; i < lSORA.Count; i++)
                                        {
                                            if (lSOR1 == "")
                                            {
                                                //lSOR1 = "'" + lSORA[i] + "'";
                                                lSOR1 = lSORA[i];
                                            }
                                            else
                                            {
                                                //lSOR1 = lSOR1 + "," + "'" + lSORA[i] + "'";
                                                lSOR1 = lSOR1 + "," + lSORA[i];
                                            }
                                            lCount = lCount + 1;
                                            if (lCount > 300)
                                            {
                                                lSORList.Add(lSOR1);
                                                lSOR1 = "";
                                                lCount = 0;
                                            }
                                        }
                                        if (lSOR1 != "")
                                        {
                                            lSORList.Add(lSOR1);
                                        }
                                    }

                                    for (int k = 0; k < lSORList.Count; k++)
                                    {
                                        if (lSORList[k] != null && lSORList[k] != "" && lSORList[k] != " " && lSORList[k] != "''" && lSORList[k] != "' '")
                                        {
                                            // Plan Delivery date

                                            var lDelSOR = "";
                                            if (lReturn.Count > 0)
                                            {
                                                for (int j = 0; j < lReturn.Count; j++)
                                                {
                                                    if (lReturn[j].PlanDeliveryDate != null && lReturn[j].PlanDeliveryDate.Trim() != "")
                                                    {
                                                        var lSOAR = lReturn[j].SORNo.Split(',');
                                                        if (lSOAR.Length > 0)
                                                        {
                                                            for (int m = 0; m < lSOAR.Length; m++)
                                                            {
                                                                lDelSOR = lDelSOR + ",'" + lSOAR[m] + "'";
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            if (lDelSOR.Length > 0)
                                            {
                                                lDelSOR = lDelSOR.Substring(1);
                                            }

                                            if (lDelSOR.Length > 0)
                                            {
                                                lSQL = "SELECT A.ORDER_REQUEST_NO, Max(A.CONFIRMED_DATE), H.BBS " +      //PLANNED_DATE
                                                "FROM SALES_ORDER_PLANNING A, ORDER_HEADER H " +
                                                "WHERE A.ORDER_REQUEST_NO = H.ORDER_REQUEST_NO " +
                                                "AND A.ORDER_REQUEST_NO IN (" + lSORList[k] + ") " +
                                                "AND not exists (select ORDER_REQUEST_NO " +
                                                "FROM SALES_ORDER_PLANNING " +
                                                "WHERE ORDER_REQUEST_NO = A.ORDER_REQUEST_NO " +
                                                "AND ORDER_REQUEST_NO IN (" + lDelSOR + ") ) " +
                                                "GROUP BY A.ORDER_REQUEST_NO, H.BBS ";
                                            }
                                            else
                                            {
                                                lSQL = "SELECT A.ORDER_REQUEST_NO, Max(A.CONFIRMED_DATE), H.BBS " +      //PLANNED_DATE
                                                "FROM SALES_ORDER_PLANNING A, ORDER_HEADER H " +
                                                "WHERE A.ORDER_REQUEST_NO = H.ORDER_REQUEST_NO " +
                                                "AND A.ORDER_REQUEST_NO IN (" + lSORList[k] + ") " +
                                                "GROUP BY A.ORDER_REQUEST_NO, H.BBS ";
                                            }

                                            lIDBCmd.CommandText = lSQL;
                                            lIDBCmd.Connection = lIDBCon;
                                            lIDBCmd.CommandTimeout = 300;
                                            lOraRst = (OracleDataReader)await lIDBCmd.ExecuteReaderAsync();
                                            if (lOraRst.HasRows)
                                            {
                                                while (lOraRst.Read())
                                                {
                                                    var lOrderNo = lOraRst.GetValue(0) == DBNull.Value ? "" : lOraRst.GetString(0).Trim();
                                                    var lPlanDate = (DateTime?)lOraRst.GetValue(1);
                                                    var lBBSNo = lOraRst.GetValue(2) == DBNull.Value ? "" : lOraRst.GetString(2).Trim();
                                                    if (lPlanDate != null)
                                                    {
                                                        if ((DateTime)lPlanDate > DateTime.Now.AddYears(1))
                                                        {
                                                            lPlanDate = null;
                                                        }
                                                    }
                                                    if (lOrderNo != "")
                                                    {
                                                        var lConfirmed = 1;

                                                        if (lPlanDate != null)
                                                        {
                                                            if (String.Format("{0:yyyy-MM-dd}", lPlanDate)
                                                                == String.Format("{0:yyyy-MM-dd}", DateTime.Now))
                                                            {
                                                                lConfirmed = 1;
                                                            }
                                                            else if (String.Format("{0:yyyy-MM-dd}", lPlanDate)
                                                                == (String.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(1))))
                                                            {
                                                                lConfirmed = 2;
                                                            }
                                                            else
                                                            {
                                                                lConfirmed = 3;
                                                            }
                                                        }

                                                        var lPlanDateStr = lPlanDate == null ? "" : ((DateTime)lPlanDate).ToString("yyyy-MM-dd");
                                                        for (int j = 0; j < lReturn.Count; j++)
                                                        {
                                                            if (lReturn[j].SORNo.IndexOf(lOrderNo) >= 0)
                                                            {
                                                                //added for specify the product
                                                                var lPlanDelDate = lPlanDateStr;
                                                                var lDeliveryDate = lReturn[j].PlanDeliveryDate;

                                                                if (lDeliveryDate == null)
                                                                {
                                                                    lDeliveryDate = "";
                                                                }

                                                                if (lPlanDelDate != null && lPlanDelDate != "")
                                                                {
                                                                    if (lDeliveryDate.Trim() == "")
                                                                    {
                                                                        if (lOrderNo.Substring(0, 3) == "103")
                                                                        {
                                                                            if (lReturn[j].SONo.LastIndexOf(",") > 0 && (lReturn[j].SONo.Substring(0, 1) == "8" || lReturn[j].SONo.IndexOf(",8") >= 0))
                                                                            {
                                                                                lDeliveryDate = lPlanDelDate + "(SB)";
                                                                            }
                                                                            else
                                                                            {
                                                                                lDeliveryDate = lPlanDelDate;
                                                                            }
                                                                        }
                                                                        else if (lReturn[j].SONo.LastIndexOf(",") > 0 && (lReturn[j].SONo.Substring(0, 3) == "103" || lReturn[j].SONo.IndexOf(",103") > 0))
                                                                        {
                                                                            if (lBBSNo.IndexOf("-COUPLER") > 0 || lBBSNo.IndexOf("-CP") > 0)
                                                                            {
                                                                                lDeliveryDate = lPlanDelDate + "(CP)";
                                                                            }
                                                                            else
                                                                            {
                                                                                lDeliveryDate = lPlanDelDate + "(CAB)";
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            if (lReturn[j].SONo.LastIndexOf(",") > 0 && (lReturn[j].BBSNo.IndexOf("-COUPLER") > 0 || lReturn[j].BBSNo.IndexOf("-CP") > 0))
                                                                            {
                                                                                if (lBBSNo.IndexOf("-COUPLER") > 0 || lBBSNo.IndexOf("-CP") > 0)
                                                                                {
                                                                                    lDeliveryDate = lPlanDelDate + "(CP)";
                                                                                }
                                                                                else
                                                                                {
                                                                                    lDeliveryDate = lPlanDelDate + "(CAB)";
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                lDeliveryDate = lPlanDelDate;
                                                                            }
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (lDeliveryDate.IndexOf(lPlanDelDate) < 0)
                                                                        {
                                                                            if (lOrderNo.Substring(0, 3) == "103")
                                                                            {
                                                                                if (lReturn[j].SONo.LastIndexOf(",") > 0 && (lReturn[j].SONo.Substring(0, 1) == "8" || lReturn[j].SONo.IndexOf(",8") >= 0))
                                                                                {
                                                                                    lDeliveryDate = lDeliveryDate + "," + lPlanDelDate + "(SB)";
                                                                                }
                                                                                else
                                                                                {
                                                                                    lDeliveryDate = lDeliveryDate + "," + lPlanDelDate;
                                                                                }
                                                                            }
                                                                            else if (lReturn[j].SONo.LastIndexOf(",") > 0 && (lReturn[j].SONo.Substring(0, 3) == "103" || lReturn[j].SONo.IndexOf(",103") > 0))
                                                                            {
                                                                                if (lBBSNo.IndexOf("-COUPLER") > 0 || lBBSNo.IndexOf("-CP") > 0)
                                                                                {
                                                                                    lDeliveryDate = lDeliveryDate + "," + lPlanDelDate + "(CP)";
                                                                                }
                                                                                else
                                                                                {
                                                                                    lDeliveryDate = lDeliveryDate + "," + lPlanDelDate + "(CAB)";
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                if (lReturn[j].SONo.LastIndexOf(",") > 0 && (lReturn[j].BBSNo.IndexOf("-COUPLER") > 0 || lReturn[j].BBSNo.IndexOf("-CP") > 0))
                                                                                {
                                                                                    if (lBBSNo.IndexOf("-COUPLER") > 0 || lBBSNo.IndexOf("-CP") > 0)
                                                                                    {
                                                                                        lDeliveryDate = lDeliveryDate + "," + lPlanDelDate + "(CP)";
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        lDeliveryDate = lDeliveryDate + "," + lPlanDelDate + "(CAB)";
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    lDeliveryDate = lDeliveryDate + "," + lPlanDelDate;
                                                                                }
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            if (lDeliveryDate.IndexOf(",") < 0 && lDeliveryDate.IndexOf("(") > 0)
                                                                            {
                                                                                lDeliveryDate = lDeliveryDate.Substring(0, lDeliveryDate.IndexOf("("));
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                //PlanDeliveryDate = (lReturn[j].PlanDeliveryDate == null || lReturn[j].PlanDeliveryDate.Trim() == "") ? lPlanDateStr : ((lPlanDateStr != null && lPlanDateStr.Trim() != "" && lReturn[j].PlanDeliveryDate.IndexOf(lPlanDateStr) < 0) ? (lReturn[j].PlanDeliveryDate + "," + lPlanDateStr) : lReturn[j].PlanDeliveryDate)
                                                                //end

                                                                lReturn[j] = new
                                                                {
                                                                    SSNNo = lReturn[j].SSNNo,
                                                                    OrderNo = lReturn[j].OrderNo,
                                                                    WBS1 = lReturn[j].WBS1,
                                                                    WBS2 = lReturn[j].WBS2,
                                                                    WBS3 = lReturn[j].WBS3,
                                                                    StructureElement = lReturn[j].StructureElement,
                                                                    ProdType = lReturn[j].ProdType,
                                                                    PONo = lReturn[j].PONo,
                                                                    PODate = lReturn[j].PODate,
                                                                    RequiredDate = lReturn[j].RequiredDate,
                                                                    OrderWeight = lReturn[j].OrderWeight,
                                                                    SubmittedBy = lReturn[j].SubmittedBy,
                                                                    OrderStatus = lReturn[j].OrderStatus,
                                                                    OrderSource = lReturn[j].OrderSource,
                                                                    SONo = lReturn[j].SONo,
                                                                    SORNo = lReturn[j].SORNo,
                                                                    BBSNo = lReturn[j].BBSNo,
                                                                    BBSDesc = lReturn[j].BBSDesc,
                                                                    CustomerCode = lReturn[j].CustomerCode,
                                                                    ProjectCode = lReturn[j].ProjectCode,
                                                                    ProjectTitle = lReturn[j].ProjectTitle,
                                                                    DataEnteredBy = lReturn[j].DataEnteredBy,
                                                                    Confirmed = lReturn[j].Confirmed == 1 ? 1 : (lConfirmed == 1 ? 1 : (lReturn[j].Confirmed == 2 ? 2 : lConfirmed)),
                                                                    PlanDeliveryDate = lDeliveryDate,
                                                                    SubmitBy = lReturn[j].SubmitBy,
                                                                    Address = lReturn[j].Address,
                                                                    Gate = lReturn[j].Gate
                                                                };

                                                                break;
                                                            }
                                                        }

                                                    }
                                                }
                                            }

                                            lOraRst.Close();

                                        }
                                    }

                                    lProcessObj.CloseIDBConnection(ref lIDBCon);
                                }
                                catch (Exception ex)
                                {

                                }
                            }
                        }

                        #endregion

                        lProcessObj = null;

                        #endregion
                    }
                }
                lCmd = null;
                lNDSCon = null;
                lRst = null;

                lIDBCmd = null;
                lIDBCon = null;
                lOraRst = null;


                lReturn = (from p in lReturn
                           orderby p.RequiredDate descending
                           select p).ToList();

                // put the Pending Approval first


                if (lReturn.Count > 0)
                {
                    var lPendingA = lReturn.ToArray();

                    lReturn.CopyTo(lPendingA);

                    var lPendingAppr = lPendingA.ToList();

                    lPendingAppr.Clear();

                    for (int i = lReturn.Count - 1; i >= 0; i--)
                    {
                        if (lReturn[i].OrderStatus == "Pending Approval")
                        {
                            lPendingAppr.Add(lReturn[i]);
                            lReturn.RemoveAt(i);
                        }

                    }

                    if (lPendingAppr.Count > 0)
                    {
                        for (int i = 0; i < lPendingAppr.Count; i++)
                        {
                            lReturn.Insert(0, lPendingAppr[i]);
                        }
                    }
                }


                // return Json(lReturn, System.Web.Mvc.JsonRequestBehavior.AllowGet);
                #region Update Wt and Qty in oes

                lCmd = new SqlCommand();
                lNDSCon = new SqlConnection();
                SqlDataReader lRst1;
                var lProcessObj1 = new ProcessController();
                if (lProcessObj1.OpenNDSConnection(ref lNDSCon) == true)
                {
                    for (int i = 0; i < lReturn.Count; i++)
                    {
                        var lItem = lReturn[i];
                        if (lItem.ProdType.Trim() == "STIRRUP-LINK-MESH" || lItem.ProdType.Trim() == "PRE-CAGE" || lItem.ProdType.Trim() == "CUT-TO-SIZE-MESH" || lItem.ProdType.Trim() == "CARPET" || lItem.ProdType.Trim() == "COLUMN-LINK-MESH" || lItem.ProdType.Trim() == "ACS" || lItem.ProdType.Trim() == "BPC" || lItem.ProdType.Trim() == "CORE-CAGE")
                        {
                            decimal lWtObj = 0;
                            decimal lCappingWtObj = 0;
                            decimal lClinkWtObj = 0;

                            var lQtyObj = 0;
                            var lCappingQtyObj = 0;
                            var lClinkQtyObj = 0;

                            lSQL = "SELECT numPostedWeight,numPostedCappingWeight,numPostedClinkWeight,intPostedQty,intPostedCappingQty,intPostedCLinkQty FROM BBSPostHeader WHERE INTPostHeaderID in" +
                                " (select PostHeaderID from OESProjOrdersSE where OrderNumber='" + lReturn[i].OrderNo + "') ";

                            lCmd.CommandText = lSQL;
                            lCmd.Connection = lNDSCon;
                            lCmd.CommandTimeout = 300;
                            lRst1 = lCmd.ExecuteReader();
                            if (lRst1.HasRows)
                            {
                                while (lRst1.Read())
                                {
                                    lWtObj = lRst1.GetValue(0) == DBNull.Value ? 0 : lRst1.GetDecimal(0);
                                    lCappingWtObj = lRst1.GetValue(1) == DBNull.Value ? 0 : lRst1.GetDecimal(1);
                                    lClinkWtObj = lRst1.GetValue(2) == DBNull.Value ? 0 : lRst1.GetDecimal(2);
                                    lQtyObj = lRst1.GetValue(3) == DBNull.Value ? 0 : lRst1.GetInt32(3);
                                    lCappingQtyObj = lRst1.GetValue(4) == DBNull.Value ? 0 : lRst1.GetInt32(4);
                                    lClinkQtyObj = lRst1.GetValue(5) == DBNull.Value ? 0 : lRst1.GetInt32(5);
                                    lWtObj = lWtObj + lCappingWtObj + lClinkWtObj;
                                    lQtyObj = lQtyObj + lCappingQtyObj + lClinkQtyObj;
                                }

                            }
                            lRst1.Close();
                            if (lWtObj != 0)
                            {
                                decimal orderWeight = 0;
                                bool isWeightValid = decimal.TryParse(lReturn[i].OrderWeight, out orderWeight);

                                if (!isWeightValid || orderWeight == 0 || orderWeight != lWtObj)
                                {
                                    lSQL = "update dbo.OESProjOrder SET TotalWeight=(" + lWtObj + "*1000)   where OrderNumber=" + lReturn[i].OrderNo + "";
                                    lCmd.CommandText = lSQL;
                                    lCmd.Connection = lNDSCon;
                                    lCmd.CommandTimeout = 300;
                                    lCmd.ExecuteNonQuery();

                                    lSQL = "update dbo.OESProjOrdersSE SET TotalWeight=(" + lWtObj + "*1000), TotalPCs='" + lQtyObj + "'   where OrderNumber=" + lReturn[i].OrderNo + "";
                                    lCmd.CommandText = lSQL;
                                    lCmd.Connection = lNDSCon;
                                    lCmd.CommandTimeout = 300;
                                    lCmd.ExecuteNonQuery();
                                }
                            }
                        }

                    }
                    lProcessObj1.CloseNDSConnection(ref lNDSCon);
                }
                lCmd = null;
                lNDSCon = null;
                lRst1 = null;


                #endregion
                return Ok(lReturn);
            }
            catch (Exception ex)
            {
                var lMsg = ex.Message;
                var lProcessObj1 = new ProcessController();
                lProcessObj1.SaveErrorMsg(ex.Message, ex.StackTrace);
                return BadRequest("error: " + ex.Message);
            }
        }


        async Task<int> UpdateStatus(string CustomerCode, string ProjectCode)
        {
            int lRtn = 0;
            string lSQL = "";
            var lCmd = new SqlCommand();
            SqlDataReader lRst;
            var lNDSCon = new SqlConnection();


            var lCISCon = new OracleConnection();
            var lIDBCon = new OracleConnection();
            var lOraCmd = new OracleCommand();
            OracleDataReader lOraRst;

            var lReturn = (new[]{ new
            {
                SSNNo = 0,
                OrderNo = 0,
                StructureElement = "",
                ProdType = "",
                SONo = "",
                ScheduledProd = ""
            }}).ToList();

            if (lReturn.Count > 0)
            {
                lReturn.RemoveAt(0);
            }

            try
            {
                lSQL =
            "SELECT " +
            "M.OrderNumber, " +
            "S.StructureElement, " +
            "S.ProductType, " +
            "(STUFF ( " +
            " (" +
            " SELECT ',' + SAPSONo FROM " +

            " (SELECT SAPSONo as SAPSONo " +
            " FROM dbo.OESStdSheetJobAdvice " +
            " WHERE CustomerCode = M.CustomerCode AND " +
            " ProjectCode = M.ProjectCode AND " +
            "(JobID = S.StdMESHJobID " +
            "OR JobID = S.StdBarsJobID " +
            "OR JobID = S.CoilProdJobID) " +
            " GROUP BY SAPSONo " +

            " Union " +
            " select BBSSOR as SAPSONo " +
            " FROM  dbo.OESBBS " +
            " WHERE CustomerCode = M.CustomerCode AND " +
            " ProjectCode = M.ProjectCode AND " +
            "JobID = S.CABJobID " +
            " GROUP BY BBSSOR " +

            " Union " +
            " select BBSSORCoupler as SAPSONo " +
            " FROM  dbo.OESBBS " +
            " WHERE CustomerCode = M.CustomerCode AND " +
            " ProjectCode = M.ProjectCode AND " +
            "JobID = S.CABJobID " +
            " GROUP BY BBSSORCoupler " +

            " Union " +
            " select BBSSAPSO as SAPSONo " +
            " FROM  dbo.OESBBS " +
            " WHERE CustomerCode = M.CustomerCode AND " +
            " ProjectCode = M.ProjectCode AND " +
            "JobID = S.CABJobID " +
            " GROUP BY BBSSAPSO " +

            " Union " +
            "SELECT SAPSOR as SAPSONo " +
            "FROM dbo.OESProjOrdersSE " +
            "WHERE OrderNumber = M.OrderNumber " +
            "AND StructureElement = S.StructureElement " +
            "AND ProductType = S.ProductType " +
            "AND ScheduledProd = S.ScheduledProd " +
            "AND SAPSOR IS NOT NULL " +
            "AND SAPSOR > '' " +
            " GROUP BY SAPSOR " +

            " ) as k " +
            " FOR XML PATH('')), 1, 1, '')) " +
            " AS SOR, " +
            "S.ScheduledProd " +
            "FROM dbo.OESProjOrder M, dbo.OESProjOrdersSE S " +
            "WHERE M.OrderNumber = S.OrderNumber " +
            "AND M.CustomerCode = '" + CustomerCode + "' " +
            "AND M.ProjectCode = '" + ProjectCode + "' " +
            "AND M.OrderStatus is not NULL " +
            "AND M.OrderStatus <> 'New' " +
            "AND M.OrderStatus <> 'Created' " +
            "AND M.OrderStatus <> 'Cancelled' " +
            "AND M.OrderStatus <> 'Deleted' " +
            "AND M.OrderStatus <> 'Delivered' " +
            "GROUP BY " +
            "M.CustomerCode, " +
            "M.ProjectCode, " +
            "S.StructureElement, " +
            "S.ProductType, " +
            "S.CABJobID, " +
            "S.StdMESHJobID, " +
            "S.StdBarsJobID, " +
            "S.CoilProdJobID, " +
            "S.ScheduledProd, " +
            "M.OrderNumber " +
            "ORDER BY " +
            "M.OrderNumber DESC ";

                var lProcessObj = new ProcessController();
                if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
                {
                    lCmd.CommandText = lSQL;
                    lCmd.Connection = lNDSCon;
                    lCmd.CommandTimeout = 300;
                    lRst = await lCmd.ExecuteReaderAsync();
                    if (lRst.HasRows)
                    {
                        var lSNo = 0;
                        while (lRst.Read())
                        {
                            lSNo = lSNo + 1;
                            lReturn.Add(new
                            {
                                SSNNo = lSNo,
                                OrderNo = lRst.GetInt32(0),
                                StructureElement = lRst.GetValue(1) == DBNull.Value ? "" : lRst.GetString(1).Trim(),
                                ProdType = (lRst.GetValue(2) == DBNull.Value ? "" : lRst.GetString(2).Trim()),
                                SONo = (lRst.GetValue(3) == DBNull.Value ? "" : lRst.GetString(3).Trim()),
                                ScheduledProd = (lRst.GetValue(4) == DBNull.Value ? "N" : lRst.GetString(4).Trim())
                            });
                        }
                    }
                    lRst.Close();

                    lProcessObj.CloseNDSConnection(ref lNDSCon);
                }

                string lSOR = "";
                if (lReturn.Count > 0)
                {
                    for (int i = 0; i < lReturn.Count; i++)
                    {
                        if (lReturn[i].SONo != null && lReturn[i].SONo != "")
                        {
                            if (lSOR == "")
                            {
                                lSOR = lReturn[i].SONo;
                            }
                            else
                            {
                                lSOR = lSOR + "," + lReturn[i].SONo;
                            }
                        }
                    }
                }

                if (lSOR != "")
                {
                    var lSORA = lSOR.Split(',').ToList();
                    lSOR = "";
                    if (lSORA.Count > 0)
                    {
                        for (int i = 0; i < lSORA.Count; i++)
                        {
                            if (lSOR == "")
                            {
                                lSOR = "'" + lSORA[i] + "'";
                            }
                            else
                            {
                                lSOR = lSOR + "," + "'" + lSORA[i] + "'";
                            }
                        }
                    }
                }

                if (lSOR == "")
                {
                    lSOR = "' '";
                }

                //take out duplicated record
                if (lReturn.Count > 0)
                {
                    for (int i = lReturn.Count - 1; i > 0; i--)
                    {
                        if (lReturn[i].OrderNo == lReturn[i - 1].OrderNo)
                        {
                            lReturn.RemoveAt(i);
                        }
                    }
                }

                lProcessObj.OpenNDSConnection(ref lNDSCon);

                //Set Cancelled Status

                //List<string> lCancelSORs = new List<string>();

                //lSQL = "SELECT " +
                //"NVL(M.ORDER_REQUEST_NO, ' ') " +                                                       
                //"FROM SAPSR3.YMSDT_ORDER_HDR M LEFT OUTER JOIN SAPSR3.YMSDT_REQ_DETAIL D  " +
                //"ON M.order_request_no = D.order_request_no " +
                //"WHERE M.MANDT = '" + lProcessObj.strClient + "' " +
                //"AND M.PROJECT = '" + ProjectCode + "' " +
                //"AND M.REQD_DEL_DATE >= to_char(sysdate - 100, 'yyyymmdd') " +
                //"AND M.ORDER_REQUEST_NO IN " +
                //"(" + lSOR + ") " +
                //"AND M.SALES_ORDER IS NOT NULL AND M.SALES_ORDER <> ' ' " +
                //"AND (M.SALES_ORDER IN " +
                //"(SELECT VBELN FROM  SAPSR3.VBUK " +
                //"WHERE VBELN = M.SALES_ORDER AND ABSTK = 'C') " +
                //"OR M.STATUS = 'X') " +
                //"ORDER BY 1 ";

                //lOraCmd.CommandText = lSQL;
                //lOraCmd.Connection = lCISCon;
                //lOraCmd.CommandTimeout = 300;
                //lOraRst = lOraCmd.ExecuteReader();
                //if (lOraRst.HasRows)
                //{
                //    while (lOraRst.Read())
                //    {
                //        if (lOraRst.GetString(0).Trim() != "")
                //        {
                //            lCancelSORs.Add(lOraRst.GetString(0).Trim());
                //        }

                //    }
                //}
                //lOraRst.Close();
                //if (lCancelSORs.Count > 0)
                //{
                //    int lCount = 0;
                //    for (int i = 0; i < lCancelSORs.Count; i++)
                //    {
                //        if (lCancelSORs[i] != "")
                //        {
                //            for (int j = 0; j < lReturn.Count; j++)
                //            {
                //                if (lReturn[j].SONo.IndexOf(lCancelSORs[i]) >= 0)
                //                {
                //                    var lOrderNo = lReturn[j].OrderNo;
                //                    var lStrEle = lReturn[j].StructureElement;
                //                    var lProdType = lReturn[j].ProdType;
                //                    var lSE = db.OrderProjectSE.Find(lOrderNo, lStrEle, lProdType);
                //                    if (lSE != null)
                //                    {
                //                        var lSENew = lSE;
                //                        lSENew.OrderStatus = "Cancelled";
                //                        db.Entry(lSE).CurrentValues.SetValues(lSENew);
                //                    }
                //                    lCount = lCount + 1;
                //                }
                //            }
                //        }
                //    }
                //    if (lCount > 0)
                //    {
                //        db.SaveChanges();
                //    }

                //    //check and Cancel Header
                //    var lHead = (from p in db.OrderProjectSE
                //                 join m in db.OrderProject
                //                 on p.OrderNumber equals m.OrderNumber
                //                 where m.OrderStatus != "Delivered" &&
                //                 m.OrderStatus != "Cancelled" &&
                //                 p.OrderStatus == "Cancelled"
                //                 select p).ToList();
                //    if (lHead != null && lHead.Count > 0)
                //    {
                //        lCount = 0;
                //        for (int i = 0; i < lHead.Count; i ++)
                //        {
                //            var lOrderNo = lHead[i].OrderNumber;

                //            var lSEc = (from p in db.OrderProjectSE
                //                        where p.OrderNumber == lOrderNo &&
                //                        p.TotalWeight > 0 &&
                //                        p.OrderStatus != "Delivered" &&
                //                        p.OrderStatus != "Cancelled"
                //                        select p).ToList();
                //            if (lSEc == null || lSEc.Count ==0)
                //            {
                //                var lHeader = db.OrderProject.Find(lOrderNo);
                //                if (lHeader != null)
                //                {
                //                    var lHeaderNew = lHeader;
                //                    lHeaderNew.OrderStatus = "Cancelled";
                //                    db.Entry(lHeader).CurrentValues.SetValues(lHeaderNew);
                //                }
                //            }

                //        }
                //        if (lCount > 0)
                //        {
                //            db.SaveChanges();
                //        }
                //    }

                //}

                // Set Production status
                string lIDBSOs = "";

                //lSQL = "SELECT NVL(M.SALES_ORDER, ' ') " +
                //"FROM SAPSR3.YMSDT_ORDER_HDR M LEFT OUTER JOIN SAPSR3.YMSDT_REQ_DETAIL D  " +
                //"ON M.order_request_no = D.order_request_no " +
                //"WHERE M.MANDT = '" + lProcessObj.strClient + "' " +
                //"AND M.PROJECT = '" + ProjectCode + "' " +
                ////                "AND M.REQD_DEL_DATE >= to_char(sysdate - 30, 'yyyymmdd') " +
                //"AND M.ORDER_REQUEST_NO IN " +
                //"(" + lSOR + ") " +
                //"AND M.SALES_ORDER IS NOT NULL AND M.SALES_ORDER <> ' ' " +
                //"AND (M.SALES_ORDER NOT IN " +
                //"(SELECT P.VGBEL FROM SAPSR3.LIPS P, SAPSR3.LIKP K " +
                //"WHERE P.MANDT = K.MANDT " +
                //"AND P.VBELN = K.VBELN " +
                //"AND P.VGBEL = M.SALES_ORDER " +
                //"AND K.wadat_ist > '00000000' ) " +
                ////"AND K.LFDAT < TO_CHAR(SYSDATE, 'YYYYMMDD') ) " +
                //"AND M.SALES_ORDER NOT IN " +
                //"(SELECT VBELN FROM SAPSR3.YMPPT_LOAD_VEHIC WHERE " +
                //"VBELN = M.SALES_ORDER  AND LOAD_DATE <= TO_CHAR(SYSDATE, 'YYYYMMDD'))) " +
                //"AND (D.STATUS <> 'X' " +
                //"OR D.STATUS is NULL) " +
                //"ORDER BY 1 ";

                lSQL =
       "SELECT ISNULL(M.SALES_ORDER, ' ') " +
       "FROM OesOrderHeaderHMI M " +
       "     LEFT OUTER JOIN OesRequestDetailsHMI D " +
       "     ON M.order_request_no = D.order_request_no " +
       "WHERE M.MANDT = '" + lProcessObj.strClient + "' " +
       "  AND M.PROJECT = '" + ProjectCode + "' " +
       "  AND M.ORDER_REQUEST_NO IN (" + lSOR + ") " +
       "  AND M.SALES_ORDER IS NOT NULL " +
       "  AND M.SALES_ORDER <> ' ' " +
       "  AND M.SALES_ORDER NOT IN ( " +
       "          SELECT DISTINCT sl.ORDER_NO " +
       "          FROM SalesOrderloading sl " +
       "               LEFT JOIN SalesOrderItem_loading sll " +
       "               ON sl.LOAD_NO = sll.LOAD_NO " +
       "          WHERE sl.ORDER_NO = M.SALES_ORDER " +
       "            AND TRY_CONVERT(date, PLAN_SHIPPING_DATE, 103) <= CAST(GETDATE() AS date) " +
       "      ) " +
       "  AND (M.STATUS <> 'X' OR M.STATUS IS NULL) " +
       "ORDER BY 1;";



                lCmd.CommandText = lSQL;
                lCmd.Connection = lNDSCon;
                lCmd.CommandTimeout = 300;
                lRst = (SqlDataReader)await lCmd.ExecuteReaderAsync();
                if (lRst.HasRows)
                {
                    while (lRst.Read())
                    {
                        if (lRst.GetString(0).Trim() != "")
                        {
                            if (lIDBSOs == "")
                            {
                                lIDBSOs = "'" + lRst.GetString(0).Trim() + "'";
                            }
                            else
                            {
                                lIDBSOs = lIDBSOs + ",'" + lRst.GetString(0).Trim() + "'";
                            }
                        }

                    }
                }
                lRst.Close();

                //Check IDB for Production
                List<string> lSOs = new List<string>();
                if (lIDBSOs != "")
                {
                    lProcessObj.OpenIDBConnection(ref lIDBCon);

                    lSQL = "SELECT MSD_PRT_SO_NO " +
                    "FROM MESH_PRINT_TAG " +
                    "WHERE MSD_PRT_SO_NO IN (" + lIDBSOs + ") " +
                    "GROUP BY MSD_PRT_SO_NO " +
                    "UNION " +
                    "SELECT ORDER_NO " +
                    "FROM cab_print_tag " +
                    "WHERE ORDER_NO IN (" + lIDBSOs + ") " +
                    "GROUP BY ORDER_NO " +
                    "UNION " +
                    "SELECT SO_NO " +
                    "FROM prc_print_tag " +
                    "WHERE SO_NO IN (" + lIDBSOs + ") " +
                    "GROUP BY SO_NO " +
                    "UNION " +
                    "SELECT SO_NO " +
                    "FROM bpc_print_tag " +
                    "WHERE SO_NO IN (" + lIDBSOs + ") " +
                    "GROUP BY SO_NO ";

                    lOraCmd.CommandText = lSQL;
                    lOraCmd.Connection = lIDBCon;
                    lOraCmd.CommandTimeout = 300;
                    lOraRst = (OracleDataReader)await lOraCmd.ExecuteReaderAsync();
                    if (lOraRst.HasRows)
                    {
                        while (lOraRst.Read())
                        {
                            if (lOraRst.GetString(0).Trim() != "")
                            {
                                lSOs.Add(lOraRst.GetString(0).Trim());
                            }

                        }
                    }
                    lOraRst.Close();


                    lProcessObj.CloseIDBConnection(ref lIDBCon);

                    //convert to SOR from SO
                    lIDBSOs = "";
                    if (lSOs.Count > 0)
                    {
                        for (int i = 0; i < lSOs.Count; i++)
                        {
                            if (lIDBSOs == "")
                            {
                                lIDBSOs = "'" + lSOs[i] + "'";
                            }
                            else
                            {
                                lIDBSOs = lIDBSOs + ",'" + lSOs[i] + "'";
                            }
                        }

                        lSOs = new List<string>();

                        lSQL = "SELECT " +
                        "NVL(M.ORDER_REQUEST_NO, ' ') " +
                        "FROM OesOrderHeaderHMI M " +
                        "WHERE M.MANDT = '" + lProcessObj.strClient + "' " +
                        "AND M.PROJECT = '" + ProjectCode + "' " +
                        "AND M.SALES_ORDER IN " +
                        "(" + lIDBSOs + ") ";

                        lCmd.CommandText = lSQL;
                        lCmd.Connection = lNDSCon;
                        lCmd.CommandTimeout = 300;
                        lRst = (SqlDataReader)await lCmd.ExecuteReaderAsync();
                        if (lRst.HasRows)
                        {
                            while (lRst.Read())
                            {
                                if (lRst.GetString(0).Trim() != "")
                                {
                                    lSOs.Add(lRst.GetString(0).Trim());
                                }

                            }
                        }
                        lRst.Close();
                    }

                    if (lSOs.Count > 0)
                    {
                        int lCount = 0;
                        for (int i = 0; i < lSOs.Count; i++)
                        {
                            if (lSOs[i] != "")
                            {
                                for (int j = 0; j < lReturn.Count; j++)
                                {
                                    if (lReturn[j].SONo.IndexOf(lSOs[i]) >= 0)
                                    {
                                        int lOrderNo = lReturn[j].OrderNo;
                                        var lStrEle = lReturn[j].StructureElement;
                                        var lProdType = lReturn[j].ProdType;
                                        var lScheduledProd = lReturn[j].ScheduledProd;
                                        var lHeader = db.OrderProject.Find(lOrderNo);
                                        if (lHeader != null)
                                        {
                                            var lHeaderNew = lHeader;
                                            if (lHeaderNew.OrderStatus != "Production")
                                            {
                                                lHeaderNew.OrderStatus = "Production";
                                                db.Entry(lHeader).CurrentValues.SetValues(lHeaderNew);
                                                lCount = lCount + 1;
                                            }
                                        }

                                        var lSE = db.OrderProjectSE.Find(lOrderNo, lStrEle, lProdType, lScheduledProd);
                                        if (lSE != null)
                                        {
                                            var lSENew = lSE;
                                            if (lSENew.OrderStatus != "Production")
                                            {
                                                lSENew.OrderStatus = "Production";
                                                db.Entry(lSE).CurrentValues.SetValues(lSENew);
                                                lCount = lCount + 1;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (lCount > 0)
                        {
                            db.SaveChanges();
                        }
                    }
                }

                // Set Delivery status
                lSOs = new List<string>();

                //lSQL = "SELECT NVL(M.ORDER_REQUEST_NO, ' ') " +
                //"FROM SAPSR3.YMSDT_ORDER_HDR M LEFT OUTER JOIN SAPSR3.YMSDT_REQ_DETAIL D  " +
                //"ON M.order_request_no = D.order_request_no " +
                //"WHERE M.MANDT = '" + lProcessObj.strClient + "' " +
                //"AND M.PROJECT = '" + ProjectCode + "' " +
                ////                "AND M.REQD_DEL_DATE >= to_char(sysdate - 100, 'yyyymmdd') " +
                //"AND M.ORDER_REQUEST_NO IN " +
                //"(" + lSOR + ") " +
                //"AND M.SALES_ORDER IS NOT NULL AND M.SALES_ORDER <> ' ' " +
                //"AND ((M.SALES_ORDER IN " +
                //"(SELECT P.VGBEL FROM SAPSR3.LIPS P, SAPSR3.LIKP K " +
                //"WHERE P.MANDT = K.MANDT " +
                //"AND P.VBELN = K.VBELN " +
                //"AND P.VGBEL = M.SALES_ORDER " +
                //"AND K.wadat_ist > '00000000' ) " +
                ////"AND K.LFDAT < TO_CHAR(SYSDATE, 'YYYYMMDD') ) " +
                //"AND M.SALES_ORDER IN " +
                //"(SELECT SALES_ORDER FROM SAPSR3.YMPPT_LP_ITEM_C " +
                //"WHERE SALES_ORDER = M.SALES_ORDER AND DELIVERY_DATE < TO_CHAR(SYSDATE, 'YYYYMMDD') " +
                //"AND DESP_LOAD_QNTY > 0 ) ) " +
                //"OR M.SALES_ORDER IN " +
                //"(SELECT VBELN FROM SAPSR3.YMPPT_LOAD_VEHIC WHERE " +
                //"VBELN = M.SALES_ORDER  AND LOAD_DATE <= TO_CHAR(SYSDATE, 'YYYYMMDD'))) " +
                //"AND (D.STATUS <> 'X' " +
                //"OR D.STATUS is NULL) " +
                //"ORDER BY 1 ";

                lSQL =
                    "SELECT ISNULL(M.ORDER_REQUEST_NO, ' ') " +
                    "FROM OesOrderHeaderHMI M " +
                    "     LEFT OUTER JOIN OesRequestDetailsHMI D " +
                    "     ON M.order_request_no = D.order_request_no " +
                    "WHERE M.MANDT = '" + lProcessObj.strClient + "' " +
                    "  AND M.PROJECT = '" + ProjectCode + "' " +
                    "  AND M.ORDER_REQUEST_NO IN (" + lSOR + ") " +
                    "  AND M.SALES_ORDER IS NOT NULL " +
                    "  AND M.SALES_ORDER <> ' ' " +
                    "  AND ( " +
                    "        M.SALES_ORDER IN ( " +
                    "              SELECT SALES_ORDER " +
                    "              FROM HMIDONoDetails " +
                    "              WHERE TRY_CONVERT(date, Act_Del_Date, 103) <= CAST(GETDATE() AS date) " +
                    "                AND SALES_ORDER = M.SALES_ORDER " +
                    "        ) " +
                    "        OR M.SALES_ORDER IN ( " +
                    "              SELECT DISTINCT sl.ORDER_NO " +
                    "              FROM SalesOrderloading sl " +
                    "                   LEFT JOIN SalesOrderItem_loading sll " +
                    "                   ON sl.LOAD_NO = sll.LOAD_NO " +
                    "              WHERE sl.ORDER_NO = M.SALES_ORDER " +
                    "                AND TRY_CONVERT(date, PLAN_SHIPPING_DATE, 103) <= CAST(GETDATE() AS date) " +
                    "        ) " +
                    "      ) " +
                    "  AND (D.STATUS <> 'X' OR D.STATUS IS NULL) " +
                    "ORDER BY 1;";



                lCmd.CommandText = lSQL;
                lCmd.Connection = lNDSCon;
                lCmd.CommandTimeout = 300;
                lRst = (SqlDataReader)await lCmd.ExecuteReaderAsync();
                if (lRst.HasRows)
                {
                    while (lRst.Read())
                    {
                        if (lRst.GetString(0).Trim() != "")
                        {
                            lSOs.Add(lRst.GetString(0).Trim());
                        }

                    }
                }
                lRst.Close();

                lProcessObj.CloseNDSConnection(ref lNDSCon);

                if (lSOs.Count > 0)
                {
                    int lCount = 0;
                    for (int i = 0; i < lSOs.Count; i++)
                    {
                        if (lSOs[i] != "")
                        {
                            for (int j = 0; j < lReturn.Count; j++)
                            {
                                if (lReturn[j].SONo.IndexOf(lSOs[i]) >= 0)
                                {
                                    int lOrderNo = lReturn[j].OrderNo;
                                    var lStrEle = lReturn[j].StructureElement;
                                    var lProdType = lReturn[j].ProdType;
                                    var lScheduleProd = lReturn[j].ScheduledProd;
                                    var lHeader = db.OrderProject.Find(lOrderNo);
                                    if (lHeader != null)
                                    {
                                        var lHeaderNew = lHeader;
                                        lHeaderNew.OrderStatus = "Delivered";
                                        db.Entry(lHeader).CurrentValues.SetValues(lHeaderNew);
                                    }

                                    var lSE = db.OrderProjectSE.Find(lOrderNo, lStrEle, lProdType, lScheduleProd);
                                    if (lSE != null)
                                    {
                                        var lSENew = lSE;
                                        lSENew.OrderStatus = "Delivered";
                                        db.Entry(lSE).CurrentValues.SetValues(lSENew);
                                    }
                                    lCount = lCount + 1;
                                }
                            }
                        }
                    }
                    if (lCount > 0)
                    {
                        db.SaveChanges();
                    }
                }

            }
            catch (Exception ex)
            {
                lRtn = -1;
            }

            return lRtn;
        }




        [HttpPost]
        [Route("/exportActiveOrdersToExcel")]
        public async Task<ActionResult> exportActiveOrdersToExcel()
        {
            try
            {


                //exportActiveOrdersToExcelDto exportActiveOrdersToExcelDto = new exportActiveOrdersToExcelDto();

                //CustomerCode = HttpContext.Request.Form["CustomerCode"];
                //ProjectCode = HttpContext.Request.Form["ProjectCode"];
                //PONumber = HttpContext.Request.Form["PONumber"];
                //PODate = HttpContext.Request.Form["PODate"];
                //RDate = HttpContext.Request.Form["RDate"];
                //WBS1 = HttpContext.Request.Form["WBS1"];
                //WBS2 = HttpContext.Request.Form["WBS2"];
                //WBS3 = HttpContext.Request.Form["WBS3"];
                // AllProjects = HttpContext.Request.Form["AllProjects"];


                // Get form data
                string CustomerCode = Request.Form["CustomerCode"];
                string temp = Request.Form["ProjectCodes"];
                string tempAddress = Request.Form["AddressCodes"];
                string lUserName = Request.Form["UserName"];
                string allProjectsStr = Request.Form["AllProjects"];

                List<string> ProjectCodes = !string.IsNullOrEmpty(temp) ? temp.Split(',').ToList() : new List<string>();
                List<string> AddressCodes = tempAddress.Split(',').ToList();
                bool AllProjects = bool.TryParse(allProjectsStr, out bool allProjects) ? allProjects : false;
                string RDate = "";



                string PONumber = ""; string PODate = ""; string DONumber = "";
                string WBS1 = ""; string WBS2 = ""; string WBS3 = ""; //bool AllProjects = false;



                var lUserID = lUserName;
                string lSQL = "";
                //set kookie for customer and project
                string lPODateFrom = "";
                string lPODateTo = "";
                string lRDateFrom = "";
                string lRDateTo = "";

                string lPODateFrom_O = "";
                string lPODateTo_O = "";
                string lRDateFrom_O = "";
                string lRDateTo_O = "";

                if (PODate == null) PODate = "";
                if (PODate.Trim().Length == 0 || PODate.IndexOf("to") <= 0)
                {
                    lPODateFrom = "2000-01-01 00:00:00";
                    lPODateTo = "2200-01-01 00:00:00";
                }
                else
                {
                    lPODateFrom = PODate.Substring(0, PODate.IndexOf("to")).Trim();
                    lPODateTo = PODate.Substring(PODate.IndexOf("to") + 2).Trim();
                }
                DateTime lDateV = new DateTime();
                if (DateTime.TryParse(lPODateFrom, out lDateV) != true)
                {
                    lPODateFrom = "2000-01-01 00:00:00";
                }
                if (DateTime.TryParse(lPODateTo, out lDateV) != true)
                {
                    lPODateTo = "2200-01-01 00:00:00";
                }

                if (RDate == null) RDate = "";
                if (RDate.Trim().Length == 0 || RDate.IndexOf("to") <= 0)
                {
                    lRDateFrom = "2000-01-01 00:00:00";
                    lRDateTo = "2200-01-01 00:00:00";
                }
                else
                {
                    lRDateFrom = RDate.Substring(0, RDate.IndexOf("to")).Trim();
                    lRDateTo = RDate.Substring(RDate.IndexOf("to") + 2).Trim();
                }

                lDateV = new DateTime();
                if (DateTime.TryParse(lRDateFrom, out lDateV) != true)
                {
                    lRDateFrom = "2000-01-01 00:00:00";
                }
                if (DateTime.TryParse(lRDateTo, out lDateV) != true)
                {
                    lRDateTo = "2200-01-01 00:00:00";
                }

                ExcelPackage package = new ExcelPackage();
                ExcelWorksheet ws = package.Workbook.Worksheets.Add("Active Order List");

                int lRowNo = 1;
                // ws.Column(1).Width = 5;         //"SNo\n序号";
                ws.Column(1).Width = 12;        //"Order No\n加工表号";
                ws.Column(2).Width = 10;        //"WBS1\n楼座";
                ws.Column(3).Width = 10;        //"WBS2\n楼座";
                ws.Column(4).Width = 7;         //"WBS3\n楼座";
                ws.Column(5).Width = 17;        //"Structure Element";
                ws.Column(6).Width = 14;        //"PO Type";
                ws.Column(7).Width = 14;        //"PO Number";
                ws.Column(8).Width = 14;        //"BBS Number";
                ws.Column(9).Width = 25;        //"BBS Desc";
                ws.Column(10).Width = 11;        //"PO Date\n订货日期";
                ws.Column(11).Width = 15;        //"Required Date\n到场日期";
                ws.Column(12).Width = 16;       //"Order WT\n料单重量";
                ws.Column(13).Width = 17;        //"Submitted By";
                ws.Column(14).Width = 17;        //"Created By";
                ws.Column(15).Width = 15;        //"Order status";
                                                 //if (lUserID.Split('@').Length == 2 && lUserID.Split('@')[1].ToLower() == "natsteel.com.sg")
                                                 //{
                ws.Column(16).Width = 17;        //"Plan Delivery Date\n";
                ws.Column(17).Width = 18;        //"Address\n";
                ws.Column(18).Width = 19;        //"Gate";
                ws.Column(19).Width = 20;        // "Gate\n门"
                                                 //}
                ws.Row(1).Height = 30;
                //ws.Cells[lRowNo, 1].Value = "SNo\n序号";
                ws.Cells[lRowNo, 1].Value = "SNo\n序号";  //"Order No\n订单号码";
                ws.Cells[lRowNo, 2].Value = "WBS1\n楼座";
                ws.Cells[lRowNo, 3].Value = "WBS2\n楼层";
                ws.Cells[lRowNo, 4].Value = "WBS3\n分部";
                ws.Cells[lRowNo, 5].Value = "Structure Element\n构件";
                ws.Cells[lRowNo, 6].Value = "Product Type\n产品类型";
                ws.Cells[lRowNo, 7].Value = "PO Number\n订单号码";
                ws.Cells[lRowNo, 8].Value = "BBS Number\n钢筋加工号码";
                ws.Cells[lRowNo, 9].Value = "BBS Description\n钢筋加工描述";
                ws.Cells[lRowNo, 10].Value = "PO Date\n订货日期";
                ws.Cells[lRowNo, 11].Value = "Required Date\n到场日期";
                ws.Cells[lRowNo, 12].Value = "Order Weight\n订单重量 (MT)";
                ws.Cells[lRowNo, 13].Value = "Submitted By\n提交人";
                ws.Cells[lRowNo, 14].Value = "Created By\n创建者";
                ws.Cells[lRowNo, 15].Value = "Order Status\n订单状态";

                //if (lUserID.Split('@').Length == 2 && lUserID.Split('@')[1].ToLower() == "natsteel.com.sg")
                //{
                ws.Cells[lRowNo, 16].Value = "Plan Delivery Date\n计划到场日期";
                ws.Cells[lRowNo, 17].Value = " Project Title\n工程项目";
                //}
                ws.Cells[lRowNo, 18].Value = "Address\n地址";
                ws.Cells[lRowNo, 19].Value = "Gate\n门";

                ws.Cells[lRowNo, 1].Style.WrapText = true;
                ws.Cells[lRowNo, 2].Style.WrapText = true;
                ws.Cells[lRowNo, 3].Style.WrapText = true;
                ws.Cells[lRowNo, 4].Style.WrapText = true;
                ws.Cells[lRowNo, 5].Style.WrapText = true;
                ws.Cells[lRowNo, 6].Style.WrapText = true;
                ws.Cells[lRowNo, 7].Style.WrapText = true;
                ws.Cells[lRowNo, 8].Style.WrapText = true;
                ws.Cells[lRowNo, 9].Style.WrapText = true;
                ws.Cells[lRowNo, 10].Style.WrapText = true;
                ws.Cells[lRowNo, 11].Style.WrapText = true;
                ws.Cells[lRowNo, 12].Style.WrapText = true;
                ws.Cells[lRowNo, 13].Style.WrapText = true;
                ws.Cells[lRowNo, 14].Style.WrapText = true;
                ws.Cells[lRowNo, 15].Style.WrapText = true;

                //if (lUserID.Split('@').Length == 2 && lUserID.Split('@')[1].ToLower() == "natsteel.com.sg")
                //{
                ws.Cells[lRowNo, 16].Style.WrapText = true;
                //}
                ws.Cells[lRowNo, 17].Style.WrapText = true;
                ws.Cells[lRowNo, 18].Style.WrapText = true;
                ws.Cells[lRowNo, 19].Style.WrapText = true;

                ws.Cells[lRowNo, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[lRowNo, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[lRowNo, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[lRowNo, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[lRowNo, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[lRowNo, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[lRowNo, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[lRowNo, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[lRowNo, 9].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[lRowNo, 10].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[lRowNo, 11].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[lRowNo, 12].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[lRowNo, 13].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[lRowNo, 14].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[lRowNo, 15].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                //if (lUserID.Split('@').Length == 2 && lUserID.Split('@')[1].ToLower() == "natsteel.com.sg")
                //{
                ws.Cells[lRowNo, 16].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                //}
                ws.Cells[lRowNo, 17].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[lRowNo, 18].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[lRowNo, 19].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                ws.Cells[lRowNo, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[lRowNo, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[lRowNo, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[lRowNo, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[lRowNo, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[lRowNo, 6].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[lRowNo, 7].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[lRowNo, 8].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[lRowNo, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[lRowNo, 10].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[lRowNo, 11].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[lRowNo, 12].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[lRowNo, 13].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[lRowNo, 14].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[lRowNo, 15].Style.Fill.PatternType = ExcelFillStyle.Solid;
                //if (lUserID.Split('@').Length == 2 && lUserID.Split('@')[1].ToLower() == "natsteel.com.sg")
                //{
                ws.Cells[lRowNo, 16].Style.Fill.PatternType = ExcelFillStyle.Solid;
                //}
                ws.Cells[lRowNo, 17].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[lRowNo, 18].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[lRowNo, 19].Style.Fill.PatternType = ExcelFillStyle.Solid;

                ws.Cells[lRowNo, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));
                ws.Cells[lRowNo, 2].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));
                ws.Cells[lRowNo, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));
                ws.Cells[lRowNo, 4].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));
                ws.Cells[lRowNo, 5].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));
                ws.Cells[lRowNo, 6].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));
                ws.Cells[lRowNo, 7].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));
                ws.Cells[lRowNo, 8].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));
                ws.Cells[lRowNo, 9].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));
                ws.Cells[lRowNo, 10].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));
                ws.Cells[lRowNo, 11].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));
                ws.Cells[lRowNo, 12].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));
                ws.Cells[lRowNo, 13].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));
                ws.Cells[lRowNo, 14].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));
                ws.Cells[lRowNo, 15].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));

                //if (lUserID.Split('@').Length == 2 && lUserID.Split('@')[1].ToLower() == "natsteel.com.sg")
                //{
                ws.Cells[lRowNo, 16].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));
                //}
                ws.Cells[lRowNo, 17].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));
                ws.Cells[lRowNo, 18].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));
                ws.Cells[lRowNo, 19].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));

                lRowNo = 2;
                var lCmd = new SqlCommand();
                SqlDataReader lRst;
                var lNDSCon = new SqlConnection();

                var lCISCon = new OracleConnection();
                var lOraCmd = new OracleCommand();
                OracleDataReader lOraRst;

                var lIDBCon = new OracleConnection();
                var lIDBCmd = new OracleCommand();


                var lReturn = (new[]{ new
                        {
                            SSNNo = 0,
                            OrderNo = "0",
                            WBS1 = "",
                            WBS2 = "",
                            WBS3 = "",
                            StructureElement = "",
                            ProdType = "",
                            PONo = "",
                            PODate = "",
                            RequiredDate = "",
                            OrderWeight = "",
                            SubmittedBy = "",
                            OrderStatus = "",
                            OrderSource = "",
                            SONo = "",
                            SORNo = "",
                            BBSNo = "",
                            BBSDesc = "",
                            CustomerCode = "",
                            ProjectCode = "",
                            DataEnteredBy = "",
                            Confirmed = 0,
                            PlanDeliveryDate = "",
                             ProjectTitle = "",
                            Address = "",
                            Gate = ""
                        }}).ToList();
                if (CustomerCode != null && ProjectCodes != null)
                {
                    for (int x = 0; x < ProjectCodes.Count(); x++)
                    {
                        string lProjectCode = ProjectCodes[x];

                        if (CustomerCode.Length > 0 && lProjectCode.Length > 0)
                        {
                            if (CustomerCode != null && lProjectCode != null)
                            {
                                if (CustomerCode.Length > 0 && lProjectCode.Length > 0)
                                {
                                    var lProjectState = "";
                                    var lAddressState = "";
                                    if (AllProjects == true)
                                    {
                                        UserAccessController lUa = new UserAccessController();
                                        var lUserType = lUa.getUserType(lUserName);
                                        var lGroupName = lUa.getGroupName(lUserName);

                                        lUa = null;

                                        SharedAPIController lBackEnd = new SharedAPIController();

                                        var lProjects = lBackEnd.getProject(CustomerCode, lUserType, lGroupName);
                                        lBackEnd = null;

                                        if (lProjects != null && lProjects.Count > 0)
                                        {
                                            for (int i = 0; i < lProjects.Count; i++)
                                            {
                                                if (lProjects[i] != null && lProjects[i].ProjectCode != null && lProjects[i].ProjectCode.Trim().Length > 0)
                                                {
                                                    int lRtn = await UpdateStatus(CustomerCode, lProjects[i].ProjectCode);

                                                    if (lProjectState == "")
                                                    {
                                                        lProjectState = "'" + lProjects[i].ProjectCode + "' ";
                                                    }
                                                    else
                                                    {
                                                        lProjectState = lProjectState + ",'" + lProjects[i].ProjectCode + "' ";
                                                    }
                                                }
                                            }
                                        }

                                        if (lProjectState == "")
                                        {
                                            lProjectState = "'' ";
                                        }
                                    }
                                    else
                                    {
                                        lProjectState = "'" + lProjectCode + "' ";
                                    }


                                    if (AddressCodes != null && AddressCodes.Any())
                                    {
                                        var validCodes = AddressCodes
                                            .Where(code => !string.IsNullOrWhiteSpace(code))
                                            .Distinct() // optional: remove duplicates
                                            .ToList();

                                        if (validCodes.Any())
                                        {
                                            lAddressState = "AND M.AddressCode IN ('" + string.Join("', '", validCodes) + "')";
                                        }
                                        else
                                        {
                                            // ✅ All codes were null/empty/whitespace
                                            lAddressState = "AND (M.AddressCode = '' OR M.AddressCode IS NULL)";
                                        }
                                    }
                                    else
                                    {
                                        // ✅ AddressCodes list itself is null or empty
                                        lAddressState = "AND (M.AddressCode = '' OR M.AddressCode IS NULL)";
                                    }

                                    #region Retrieve Data

                                    lSQL =
                        "SELECT " +
                        "M.OrderNumber, " +
                        "M.WBS1, " +
                        "M.WBS2, " +
                        "M.WBS3, " +
                        "S.StructureElement, " +
                        "S.ProductType, " +
                        "S.PONumber, " +

                        "isNull(convert(varchar(10), S.PODate, 120),'') AS PODate, " + //7
                        "isNull(convert(varchar(10), S.RequiredDate, 120),'') AS RequiredDate, " + //8
                        "M.TotalWeight, " +             //9
                        "case when M.OrderStatus = 'Sent' then M.SubmitBy else M.UpdateBy end as UpdateBy, " +
                        "M.OrderStatus, " +
                        "(STUFF ( " +
                        " (" +
                        " SELECT ',' + SAPSONo FROM " +

                        " (SELECT SAPSONo as SAPSONo " +
                        " FROM dbo.OESStdSheetJobAdvice " +
                        " WHERE CustomerCode = M.CustomerCode AND " +
                        " ProjectCode = M.ProjectCode AND " +
                        "(JobID = S.StdMESHJobID " +
                        "OR JobID = S.StdBarsJobID " +
                        "OR JobID = S.CoilProdJobID) " +
                        " GROUP BY SAPSONo " +

                        " Union " +
                        " select BBSSOR as SAPSONo " +
                        " FROM  dbo.OESBBS " +
                        " WHERE CustomerCode = M.CustomerCode AND " +
                        " ProjectCode = M.ProjectCode AND " +
                        " JobID = S.CABJobID " +
                        " GROUP BY BBSSOR " +

                        " Union " +
                        " select BBSSORCoupler as SAPSONo " +
                        " FROM  dbo.OESBBS " +
                        " WHERE CustomerCode = M.CustomerCode AND " +
                        " ProjectCode = M.ProjectCode AND " +
                        " JobID = S.CABJobID " +
                        " GROUP BY BBSSORCoupler " +

                        " Union " +
                        " select BBSSAPSO as SAPSONo " +
                        " FROM  dbo.OESBBS " +
                        " WHERE CustomerCode = M.CustomerCode AND " +
                        " ProjectCode = M.ProjectCode AND " +
                        " JobID = S.CABJobID " +
                        " GROUP BY BBSSAPSO " +

                        " Union " +
                        " select sor_no as SAPSONo " +
                        " FROM  dbo.OESBPCDetailsProc " +
                        " WHERE CustomerCode = M.CustomerCode AND " +
                        " ProjectCode = M.ProjectCode AND " +
                        " JobID = S.BPCJobID " +
                        " GROUP BY sor_no " +

                        " ) as k " +
                        " FOR XML PATH('')), 1, 1, '')) " +
                        " AS SOR, " +
                        "(STUFF( " +
                        "(SELECT ',' + isNull(BBSNo, '') " +
                        "FROM  dbo.OESBBS " +
                        "WHERE CustomerCode =  M.CustomerCode " +
                        "AND ProjectCode = M.ProjectCode " +
                        "AND JobID = S.CABJobID " +
                        "GROUP BY BBSNo FOR XML PATH('')), 1, 1, '')) as BBSNo, " +
                        "(STUFF( " +
                        "(SELECT ',' + isNull(BBSDesc, '') " +
                        "FROM  dbo.OESBBS " +
                        "WHERE CustomerCode =  M.CustomerCode " +
                        "AND ProjectCode = M.ProjectCode " +
                        "AND JobID = S.CABJobID " +
                        "GROUP BY BBSDesc FOR XML PATH('')), 1, 1, '')) as BBSDesc, " +
                        "S.SAPSOR as UXSOR, " +
                        "M.CustomerCode, " +
                        "M.ProjectCode, " +
                        "M.SubmitBy, " +
                        "(SELECT ProjectTitle FROM dbo.OESProject " +
                        "WHERE CustomerCode = M.CustomerCode " +
                        "AND ProjectCode = M.ProjectCode) as ProjectTitle, " +
                        "CASE WHEN S.ProductType = 'CAB' THEN " +
                        "isNull(STUFF((SELECT  ',' + UpdateBy " +
                        "FROM dbo.OESOrderDetails " +
                        "WHERE CustomerCode = M.CustomerCode " +
                        "AND ProjectCode = M.ProjectCode " +
                        "AND JobID = S.CABJobID " +
                        "GROUP BY UpdateBy " +
                        "ORDER BY UpdateBy " +
                        "FOR XML PATH('')), 1, 1, ''),'') " +
                        "WHEN S.ProductType = 'CUT-TO-SIZE-MESH' THEN " +
                        "isNull(STUFF((SELECT  ',' + UpdateBy " +
                        "FROM dbo.OESCTSMESHOthersDetails " +
                        "WHERE CustomerCode = M.CustomerCode " +
                        "AND ProjectCode = M.ProjectCode " +
                        "AND JobID = S.MESHJobID " +
                        "GROUP BY UpdateBy " +
                        "ORDER BY UpdateBy " +
                        "FOR XML PATH('')), 1, 1, ''),'') " +
                        "WHEN S.ProductType = 'STANDARD-MESH' THEN " +
                        "isNull(STUFF((SELECT  ',' + UpdateBy " +
                        "FROM dbo.OESStdSheetDetails " +
                        "WHERE CustomerCode = M.CustomerCode " +
                        "AND ProjectCode = M.ProjectCode " +
                        "AND JobID = S.StdMESHJobID " +
                        "GROUP BY UpdateBy " +
                        "ORDER BY UpdateBy " +
                        "FOR XML PATH('')), 1, 1, ''),'') " +
                        "WHEN S.ProductType = 'STANDARD-BAR' THEN " +
                        "isNull(STUFF((SELECT  ',' + UpdateBy " +
                        "FROM dbo.OESStdProdDetails " +
                        "WHERE CustomerCode = M.CustomerCode " +
                        "AND ProjectCode = M.ProjectCode " +
                        "AND JobID = S.StdBarsJobID " +
                        "GROUP BY UpdateBy " +
                        "ORDER BY UpdateBy " +
                        "FOR XML PATH('')), 1, 1, ''),'') " +
                        "WHEN S.ProductType = 'BPC' THEN " +
                        "isNull(STUFF((SELECT  ',' + UpdateBy " +
                        "FROM dbo.OESBPCDetails " +
                        "WHERE CustomerCode = M.CustomerCode " +
                        "AND ProjectCode = M.ProjectCode " +
                        "AND Template = 0 " +
                        "AND JobID = S.BPCJobID " +
                        "GROUP BY UpdateBy " +
                        "ORDER BY UpdateBy " +
                        "FOR XML PATH('')), 1, 1, ''),'') " +
                        "WHEN S.ProductType = 'COIL' OR S.ProductType = 'COUPLER' THEN " +
                        "isNull(STUFF((SELECT  ',' + UpdateBy " +
                        "FROM dbo.OESStdProdDetails " +
                        "WHERE CustomerCode = M.CustomerCode " +
                        "AND ProjectCode = M.ProjectCode " +
                        "AND JobID = S.CoilProdJobID " +
                        "GROUP BY UpdateBy " +
                        "ORDER BY UpdateBy " +
                        "FOR XML PATH('')), 1, 1, ''),'') " +
                        "ELSE '' END as DataEnteredBy, " +
    // ✅ Newly added columns:
    "ISNULL(M.Address, ' ') AS Address, " +
    "ISNULL(M.Gate, ' ') AS Gate " +
                        "FROM dbo.OESProjOrder M, dbo.OESProjOrdersSE S " +
                        "WHERE M.OrderNumber = S.OrderNumber " +
                        "AND M.CustomerCode = '" + CustomerCode + "' " +
                        "AND M.ProjectCode IN (" + lProjectState + ") " +
                        "" + lAddressState + " " +
                        "AND M.OrderStatus is not NULL " +
                        "AND M.OrderStatus <> 'New' " +
                        "AND M.OrderStatus <> 'Created' " +
                        "AND M.OrderStatus <> 'Cancelled' " +
                        "AND M.OrderStatus <> 'Deleted' " +
                        "AND M.OrderStatus <> 'Delivered' " +
                        //"AND ((S.PODate >= '" + lPODateFrom + "' " +
                        //"AND DATEADD(d,-1,S.PODate) < '" + lPODateTo + "') " +
                        //"OR (S.PODate is null AND '" + lPODateTo + "' = '2200-01-01 00:00:00' )) " +
                        //"AND ((S.RequiredDate >= '" + lRDateFrom + "' " +
                        //"AND DATEADD(d,-1,S.RequiredDate) < '" + lRDateTo + "') " +
                        //"OR (S.RequiredDate is null AND '" + lRDateTo + "' = '2200-01-01 00:00:00' )) " +
                        //"AND (S.PONumber like '%" + PONumber + "%' " +
                        //"OR  (S.PONumber is null AND '" + PONumber + "' = '' )) " +
                        //"AND (M.WBS1 like '%" + WBS1 + "%' " +
                        //"OR (M.WBS1 is null AND '" + WBS1 + "' = '' )) " +
                        //"AND (M.WBS2 like '%" + WBS2 + "%' " +
                        //"OR (M.WBS2 is null AND '" + WBS2 + "' = '' )) " +
                        //"AND (M.WBS3 like '%" + WBS3 + "%' " +
                        //"OR (M.WBS3 is null AND '" + WBS3 + "' = '' )) " +
                        ////"AND M.UpdateDate >= '2019-07-01' " +
                        "GROUP BY " +
                        "M.CustomerCode, " +
                        "M.ProjectCode, " +
                        "S.CABJobID, " +
                        "S.MESHJobID, " +
                        "S.StdMESHJobID, " +
                        "S.StdBarsJobID, " +
                        "S.CoilProdJobID, " +
                        "S.BPCJobID, " +
                        "S.StructureElement, " +
                        "S.ProductType, " +
                        "S.ScheduledProd, " +
                        "S.PONumber, " +

                        "S.PODate, " +
                        "S.RequiredDate, " +
                        "M.OrderNumber, " +
                        "M.WBS1, " +
                        "M.WBS2, " +
                        "M.WBS3, " +
                        "M.TotalWeight, " +
                        "M.UpdateBy, " +
                        "M.OrderStatus, " +
                        "S.SAPSOR, " +
                        "M.SubmitBy, " +
    // ✅ Also added in GROUP BY
    "M.Address, " +
    "M.Gate " +
                        "ORDER BY " +
                        "M.OrderNumber DESC ";

                                    var lProcessObj = new ProcessController();
                                    if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
                                    {
                                        lCmd.CommandText = lSQL;
                                        lCmd.Connection = lNDSCon;
                                        lCmd.CommandTimeout = 300;
                                        lRst = await lCmd.ExecuteReaderAsync();
                                        if (lRst.HasRows)
                                        {
                                            var lSNo = 0;
                                            while (lRst.Read())
                                            {
                                                lSNo = lSNo + 1;
                                                var lOrderStatus = (lRst.GetValue(11) == DBNull.Value ? "" : lRst.GetString(11).Trim());
                                                if (lOrderStatus == "Sent")
                                                {
                                                    lOrderStatus = "Pending Approval";
                                                }
                                                if (lOrderStatus == "Submitted")
                                                {
                                                    lOrderStatus = "Submitted to NSH";
                                                }
                                                var lUXSOR = lRst.GetValue(15) == DBNull.Value ? "" : lRst.GetString(15).Trim();
                                                var lMySOR = lRst.GetValue(12) == DBNull.Value ? "" : lRst.GetString(12).Trim();
                                                if (lUXSOR != "" && lUXSOR != lMySOR)
                                                {
                                                    if (lMySOR == "")
                                                    {
                                                        lMySOR = lUXSOR;
                                                    }
                                                    else
                                                    {
                                                        lMySOR = lMySOR = "," + lUXSOR;
                                                    }

                                                }
                                                lReturn.Add(new
                                                {
                                                    SSNNo = lSNo,
                                                    OrderNo = lRst.GetInt32(0).ToString(),
                                                    WBS1 = (lRst.GetValue(1) == DBNull.Value ? "" : lRst.GetString(1).Trim()),
                                                    WBS2 = (lRst.GetValue(2) == DBNull.Value ? "" : lRst.GetString(2).Trim()),
                                                    WBS3 = (lRst.GetValue(3) == DBNull.Value ? "" : lRst.GetString(3).Trim()),
                                                    StructureElement = lRst.GetValue(4) == DBNull.Value ? "" : (lRst.GetString(4).Trim() == "NONWBS" ? "" : lRst.GetString(4).Trim()),
                                                    ProdType = (lRst.GetValue(5) == DBNull.Value ? "" : lRst.GetString(5).Trim()),
                                                    PONo = (lRst.GetValue(6) == DBNull.Value ? "" : (lRst.GetString(6).Trim().Length > 0 && lRst.GetString(6).Trim().Substring(0, 1) == "," ? lRst.GetString(6).Trim().Substring(1) : lRst.GetString(6).Trim())),
                                                    PODate = (lRst.GetValue(7) == DBNull.Value ? "" : lRst.GetString(7)),
                                                    RequiredDate = lRst.GetValue(8) == DBNull.Value ? "" : (lRst.GetString(8).Trim().Length > 0 && lRst.GetString(8).Substring(0, 1) == "," ? lRst.GetString(8).Substring(1) : lRst.GetString(8)),
                                                    OrderWeight = (lRst.GetValue(9) == DBNull.Value ? "0.000" : (lRst.GetDecimal(9) / 1000).ToString("###,###,##0.000;(###,##0.000); ")),
                                                    SubmittedBy = (lRst.GetValue(10) == DBNull.Value ? "" : lRst.GetString(10).Trim()),
                                                    OrderStatus = lOrderStatus,
                                                    OrderSource = "DIGIOS",
                                                    SONo = lMySOR,
                                                    SORNo = lMySOR,
                                                    BBSNo = (lRst.GetValue(13) == DBNull.Value ? "" : lRst.GetString(13).Trim()),
                                                    BBSDesc = WebUtility.HtmlDecode(lRst.GetValue(14) == DBNull.Value ? "" : lRst.GetString(14).Trim()),
                                                    CustomerCode = (lRst.GetValue(16) == DBNull.Value ? "" : lRst.GetString(16).Trim()),
                                                    ProjectCode = (lRst.GetValue(17) == DBNull.Value ? "" : lRst.GetString(17).Trim()),
                                                    DataEnteredBy = (lRst.GetValue(20) == DBNull.Value ? "" : lRst.GetString(20).Trim()),
                                                    Confirmed = 0,
                                                    PlanDeliveryDate = "",
                                                    ProjectTitle = (lRst.GetValue(19) == DBNull.Value ? "" : lRst.GetString(19).Trim()),
                                                    Address = (lRst.GetValue(21) == DBNull.Value ? "" : lRst.GetString(21).Trim()),
                                                    Gate = (lRst.GetValue(22) == DBNull.Value ? "" : lRst.GetString(22).Trim()),

                                                });
                                            }
                                        }
                                        lRst.Close();

                                        lProcessObj.CloseNDSConnection(ref lNDSCon);
                                    }

                                    string lSOR = "";
                                    if (lReturn.Count > 0)
                                    {
                                        for (int i = 0; i < lReturn.Count; i++)
                                        {
                                            if (lReturn[i].SONo != null && lReturn[i].SONo != "")
                                            {
                                                if (lSOR == "")
                                                {
                                                    lSOR = lReturn[i].SONo;
                                                }
                                                else
                                                {
                                                    lSOR = lSOR + "," + lReturn[i].SONo;
                                                }
                                            }
                                        }
                                    }

                                    if (lSOR != "")
                                    {
                                        var lSORA = lSOR.Split(',').ToList();
                                        lSOR = "";
                                        if (lSORA.Count > 0)
                                        {
                                            for (int i = 0; i < lSORA.Count; i++)
                                            {
                                                if (lSOR == "")
                                                {
                                                    lSOR = "'" + lSORA[i] + "'";
                                                }
                                                else
                                                {
                                                    lSOR = lSOR + "," + "'" + lSORA[i] + "'";
                                                }
                                            }
                                        }
                                    }

                                    if (lSOR == "")
                                    {
                                        lSOR = "' '";
                                    }

                                    //take out duplicated record
                                    if (lReturn.Count > 0)
                                    {
                                        for (int i = lReturn.Count - 1; i > 0; i--)
                                        {
                                            if (lReturn[i].OrderNo == lReturn[i - 1].OrderNo)
                                            {
                                                lReturn[i - 1] = new
                                                {
                                                    SSNNo = lReturn[i - 1].SSNNo,
                                                    OrderNo = lReturn[i - 1].OrderNo,
                                                    WBS1 = lReturn[i - 1].WBS1,
                                                    WBS2 = lReturn[i - 1].WBS2,
                                                    WBS3 = lReturn[i - 1].WBS3,
                                                    StructureElement = (lReturn[i - 1].StructureElement == null || lReturn[i - 1].StructureElement.Trim() == "") ? lReturn[i].StructureElement : ((lReturn[i].StructureElement != null && lReturn[i].StructureElement.Trim() != "" && lReturn[i].StructureElement != lReturn[i - 1].StructureElement) ? (lReturn[i - 1].StructureElement + "," + lReturn[i].StructureElement) : lReturn[i].StructureElement),
                                                    ProdType = (lReturn[i - 1].ProdType == null || lReturn[i - 1].ProdType.Trim() == "") ? lReturn[i].ProdType : ((lReturn[i].ProdType != null && lReturn[i].ProdType.Trim() != "" && lReturn[i].ProdType != lReturn[i - 1].ProdType) ? (lReturn[i - 1].ProdType + "," + lReturn[i].ProdType) : lReturn[i].ProdType),
                                                    PONo = (lReturn[i - 1].PONo == null || lReturn[i - 1].PONo.Trim() == "") ? lReturn[i].PONo : ((lReturn[i].PONo != null && lReturn[i].PONo.Trim() != "" && lReturn[i].PONo != lReturn[i - 1].PONo) ? (lReturn[i - 1].PONo + "," + lReturn[i].PONo) : lReturn[i].PONo),
                                                    PODate = (lReturn[i - 1].PODate == null || lReturn[i - 1].PODate.Trim() == "") ? lReturn[i].PODate : ((lReturn[i].PODate != null && lReturn[i].PODate.Trim() != "" && lReturn[i].PODate != lReturn[i - 1].PODate) ? (lReturn[i - 1].PODate + "," + lReturn[i].PODate) : lReturn[i].PODate),
                                                    RequiredDate = (lReturn[i - 1].RequiredDate == null || lReturn[i - 1].RequiredDate.Trim() == "") ? lReturn[i].RequiredDate : ((lReturn[i].RequiredDate != null && lReturn[i].RequiredDate.Trim() != "" && lReturn[i].RequiredDate != lReturn[i - 1].RequiredDate) ? (lReturn[i - 1].RequiredDate + "," + lReturn[i].RequiredDate) : lReturn[i].RequiredDate),
                                                    OrderWeight = lReturn[i - 1].OrderWeight,
                                                    SubmittedBy = lReturn[i - 1].SubmittedBy,
                                                    OrderStatus = lReturn[i - 1].OrderStatus,
                                                    OrderSource = lReturn[i - 1].OrderSource,
                                                    SONo = (lReturn[i - 1].SONo == null || lReturn[i - 1].SONo.Trim() == "") ? lReturn[i].SONo : ((lReturn[i].SONo != null && lReturn[i].SONo.Trim() != "" && lReturn[i].SONo != lReturn[i - 1].SONo) ? (lReturn[i - 1].SONo + "," + lReturn[i].SONo) : lReturn[i].SONo),
                                                    SORNo = (lReturn[i - 1].SORNo == null || lReturn[i - 1].SORNo.Trim() == "") ? lReturn[i].SORNo : ((lReturn[i].SORNo != null && lReturn[i].SORNo.Trim() != "" && lReturn[i].SORNo != lReturn[i - 1].SORNo) ? (lReturn[i - 1].SORNo + "," + lReturn[i].SORNo) : lReturn[i].SORNo),
                                                    BBSNo = (lReturn[i - 1].BBSNo == null || lReturn[i - 1].BBSNo.Trim() == "") ? lReturn[i].BBSNo : ((lReturn[i].BBSNo != null && lReturn[i].BBSNo.Trim() != "" && lReturn[i].BBSNo != lReturn[i - 1].BBSNo) ? (lReturn[i - 1].BBSNo + "," + lReturn[i].BBSNo) : lReturn[i].BBSNo),
                                                    BBSDesc = (lReturn[i - 1].BBSDesc == null || lReturn[i - 1].BBSDesc.Trim() == "") ? lReturn[i].BBSDesc : ((lReturn[i].BBSDesc != null && lReturn[i].BBSDesc.Trim() != "" && lReturn[i].BBSDesc != lReturn[i - 1].BBSDesc) ? (lReturn[i - 1].BBSDesc + "," + lReturn[i].BBSDesc) : lReturn[i].BBSDesc),
                                                    CustomerCode = lReturn[i - 1].CustomerCode,
                                                    ProjectCode = lReturn[i - 1].ProjectCode,
                                                    DataEnteredBy = lReturn[i - 1].DataEnteredBy,
                                                    Confirmed = lReturn[i - 1].Confirmed,
                                                    PlanDeliveryDate = lReturn[i - 1].PlanDeliveryDate,
                                                    ProjectTitle = lReturn[i - 1].ProjectTitle,
                                                    Address = lReturn[i - 1].Address,
                                                    Gate = lReturn[i - 1].Gate
                                                };

                                                lReturn.RemoveAt(i);
                                            }
                                        }
                                    }

                                    lProcessObj.OpenNDSConnection(ref lNDSCon);

                                    #region Check from CIS Plan Delivery Date
                                    if (lSOR != "' '" && lSOR != "''")
                                    {
                                        var lSORList = new List<string>();
                                        if (lSOR != "")
                                        {
                                            var lSORA = lSOR.Split(',').ToList();
                                            var lSOR1 = "";
                                            if (lSORA.Count > 0)
                                            {
                                                int lCount = 0;
                                                for (int i = 0; i < lSORA.Count; i++)
                                                {
                                                    if (lSOR1 == "")
                                                    {
                                                        //lSOR1 = "'" + lSORA[i] + "'";
                                                        lSOR1 = lSORA[i];
                                                    }
                                                    else
                                                    {
                                                        //lSOR1 = lSOR1 + "," + "'" + lSORA[i] + "'";
                                                        lSOR1 = lSOR1 + "," + lSORA[i];
                                                    }
                                                    lCount = lCount + 1;
                                                    if (lCount > 300)
                                                    {
                                                        lSORList.Add(lSOR1);
                                                        lSOR1 = "";
                                                        lCount = 0;
                                                    }
                                                }
                                                if (lSOR1 != "")
                                                {
                                                    lSORList.Add(lSOR1);
                                                }
                                            }

                                        }

                                        for (int k = 0; k < lSORList.Count; k++)
                                        {
                                            //lSQL = "SELECT " +
                                            //"NVL(M.ORDER_REQUEST_NO, ' '), " +
                                            //"M.REQ_DAT_TO, " +                                  //1. Revised Req date
                                            //"M.FIRST_PROMISED_D, " +                            //2. First Promised date
                                            //                                                    //"(SELECT NVL(MAX(conf_del_date), ' ') " +           
                                            //                                                    //"FROM SAPSR3.YMSDT_ORDER_ITEM " +
                                            //                                                    //"WHERE MANDT = M.MANDT " +
                                            //                                                    //"AND ORDER_REQUEST_NO = M.ORDER_REQUEST_NO) as Conf_del_date,  " + //3. Confirmed Del from CIS
                                            //"' ', " +
                                            ////"(SELECT MIN(PLAN_DELIV_DATE) FROM SAPSR3.YMPPT_LP_ITEM_C LC, SAPSR3.YMPPT_LP_HDR LH " +
                                            ////"WHERE LC.MANDT = LH.MANDT " +
                                            ////"AND LC.LOAD_NO = LH.LOAD_NO " +
                                            ////"AND SALES_ORDER = M.SALES_ORDER) as plan_del_date, " +         //4. Confirmed Del from Planning
                                            //"(SELECT MIN(LOAD_DATE) FROM SAPSR3.YMPPT_LP_ITEM_C LC, SAPSR3.YMPPT_LP_HDR LH " +
                                            //"WHERE LC.MANDT = LH.MANDT AND LC.LOAD_NO = LH.LOAD_NO AND LH.VEHICLE_TYPE <> 'MC' " +
                                            //"AND SALES_ORDER = M.SALES_ORDER) as plan_del_date, " +         //4. Confirmed Del from Planning
                                            //"(SELECT MIN(K.LFDAT) FROM SAPSR3.LIPS P, SAPSR3.LIKP K " +
                                            //"WHERE P.MANDT = K.MANDT " +
                                            //"AND P.VBELN = K.VBELN " +
                                            //"AND P.VGBEL = M.SALES_ORDER) as plan_del_conf, " +              //5. Confirmed Del from SAP
                                            //"D.BBS_NO " +
                                            //"FROM SAPSR3.YMSDT_ORDER_HDR M LEFT OUTER JOIN SAPSR3.YMSDT_REQ_DETAIL D " +
                                            //"ON M.order_request_no = D.order_request_no " +
                                            //"WHERE M.MANDT = '" + lProcessObj.strClient + "' " +
                                            //"AND M.PROJECT IN (" + lProjectState + ") " +
                                            //"AND M.REQD_DEL_DATE >= to_char(sysdate - 180, 'yyyymmdd') " +
                                            //"AND M.SALES_ORDER NOT IN " +
                                            //"(SELECT VBELN FROM  SAPSR3.VBUK " +
                                            //"WHERE VBELN = M.SALES_ORDER AND ABSTK = 'C') " +
                                            //"AND M.ORDER_REQUEST_NO IN " +
                                            //"(" + lSORList[k] + ") ";

                                            //lOraCmd.CommandText = lSQL;
                                            //lOraCmd.Connection = lCISCon;
                                            //lOraCmd.CommandTimeout = 300;
                                            //lOraRst = (OracleDataReader)await lOraCmd.ExecuteReaderAsync();
                                            lSQL = "SELECT " +
                                    "ISNULL(M.ORDER_REQUEST_NO, ' ') AS ORDER_REQUEST_NO, " +
                                    "M.REQ_DAT_TO, " +                                                               //1. Revised Req date
                                    "M.FIRST_PROMISED_D, " +                                                         //2. First Promised date
                                    "' ' AS UNUSED_COLUMN, " +
                                    "(SELECT TOP 1 CONVERT(varchar, CONVERT(date, PLAN_SHIPPING_DATE, 103), 112) " +
                                        "FROM SalesOrderloading sol " +
                                        "INNER JOIN HMILoadDetails hmi ON sol.LOAD_NO = hmi.LOAD_NO " +
                                        "WHERE hmi.SALES_ORDER = M.SALES_ORDER) AS PLAN_DEL_DATE, " +                //3. Confirmed Del from HMI (Shipping Date)
                                    "(SELECT CONVERT(varchar, CONVERT(date, MIN(confirmdeldate), 103), 112) " +
                                        "FROM HMIOrderHeaderDetails " +
                                        "WHERE OrderNo = M.SALES_ORDER) AS PLAN_DEL_CONF, " +                        //4. Confirmed Del from HMI
                                    "D.BBS_NO " +                                                                    //5. BBS Number
                                    "FROM OesOrderHeaderHMI M " +
                                    "LEFT OUTER JOIN OESRequestDetailsHMI D ON M.ORDER_REQUEST_NO = D.ORDER_REQUEST_NO " +
                                    "WHERE M.MANDT = '" + lProcessObj.strClient + "' " +
                                    "AND M.PROJECT IN (" + lProjectState + ") " +
                                    "AND M.REQD_DEL_DATE >= GETDATE() - 180 " +
                                    "AND M.STATUS <> 'X' " +
                                    "AND M.ORDER_REQUEST_NO IN (" + lSORList[k] + ")";


                                            lCmd.CommandText = lSQL;
                                            lCmd.Connection = lNDSCon;
                                            lCmd.CommandTimeout = 300;
                                            lRst = (SqlDataReader)await lCmd.ExecuteReaderAsync();
                                            if (lRst.HasRows)
                                            {
                                                while (lRst.Read())
                                                {
                                                    int lConfirmed = 0;
                                                    var lSORNo = (lRst.GetValue(0) == DBNull.Value ? "" : lRst.GetString(0));

                                                    //var lReqDate = (lRst.GetValue(2) == DBNull.Value ? "" : lRst.GetString(2).Trim());
                                                    //if (lReqDate == "")
                                                    //{
                                                    var lReqDate = (lRst.GetValue(1) == DBNull.Value ? "" : lRst.GetString(1).Trim());
                                                    //}
                                                    //else
                                                    //{
                                                    //    if (lReqDate.Length > 10)
                                                    //    {
                                                    //        lReqDate = lReqDate.Substring(0, 10);
                                                    //    }
                                                    //    lReqDate = String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lReqDate, "yyyy-MM-dd", CultureInfo.InvariantCulture));
                                                    //}

                                                    //var lReqDateConf = (lOraRst.GetValue(3) == DBNull.Value ? "" : lOraRst.GetString(3).Trim());
                                                    //if (lReqDateConf != null && lReqDateConf != "" && lReqDateConf != "20500101" &&
                                                    //    DateTime.ParseExact(lReqDateConf, "yyyyMMdd", CultureInfo.InvariantCulture) > DateTime.Now.AddMonths(-6) &&
                                                    //    DateTime.ParseExact(lReqDateConf, "yyyyMMdd", CultureInfo.InvariantCulture) < DateTime.Now.AddMonths(6))
                                                    //{
                                                    //    lReqDate = String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lReqDateConf, "yyyyMMdd", CultureInfo.InvariantCulture));
                                                    //}

                                                    var lBBSNo = (lRst.GetValue(6) == DBNull.Value ? "" : lRst.GetString(6).Trim());
                                                    var lPlanDelDate = (lRst.GetValue(4) == DBNull.Value ? "" : lRst.GetString(4).Trim());
                                                    if (lPlanDelDate != null && lPlanDelDate != "")
                                                    {
                                                        if (String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lPlanDelDate, "yyyyMMdd", CultureInfo.InvariantCulture))
                                                            == (String.Format("{0:yyyy-MM-dd}", DateTime.Now)))
                                                        {
                                                            lConfirmed = 1;
                                                        }
                                                        else if (String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lPlanDelDate, "yyyyMMdd", CultureInfo.InvariantCulture))
                                                            == (String.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(1))))
                                                        {
                                                            lConfirmed = 2;
                                                        }
                                                        else
                                                        {
                                                            lConfirmed = 3;
                                                        }

                                                        if (DateTime.ParseExact(lPlanDelDate, "yyyyMMdd", CultureInfo.InvariantCulture) >= DateTime.Now.AddMonths(6))
                                                        {
                                                            lPlanDelDate = "";
                                                        }
                                                        else
                                                        {
                                                            lPlanDelDate = String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lPlanDelDate, "yyyyMMdd", CultureInfo.InvariantCulture));
                                                        }
                                                    }
                                                    else
                                                    {
                                                        lPlanDelDate = "";
                                                    }

                                                    //var lPlanDelConfDate = (lOraRst.GetValue(5) == DBNull.Value ? "" : lOraRst.GetString(5).Trim());
                                                    //if (lPlanDelConfDate != null && lPlanDelConfDate != "")
                                                    //{
                                                    //    if (String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lPlanDelConfDate, "yyyyMMdd", CultureInfo.InvariantCulture))
                                                    //        == (String.Format("{0:yyyy-MM-dd}", DateTime.Now)))
                                                    //    {
                                                    //        lConfirmed = 1;
                                                    //    }
                                                    //    else if (String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lPlanDelConfDate, "yyyyMMdd", CultureInfo.InvariantCulture))
                                                    //        == (String.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(1))))
                                                    //    {
                                                    //        lConfirmed = 2;
                                                    //    }
                                                    //    else
                                                    //    {
                                                    //        lConfirmed = 3;
                                                    //    }
                                                    //    //lReqDate = String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lPlanDelConfDate, "yyyyMMdd", CultureInfo.InvariantCulture));
                                                    //}

                                                    if (lReqDate.Length > 10)
                                                    {
                                                        lReqDate = lReqDate.Substring(0, 10);
                                                    }

                                                    if (lSORNo != null && lSORNo != "" && lReturn.Count > 0 && lReqDate != null && lReqDate != "" &&
                                                    DateTime.ParseExact(lReqDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) > DateTime.Now.AddMonths(-6) &&
                                                    DateTime.ParseExact(lReqDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) < DateTime.Now.AddMonths(6))
                                                    {
                                                        for (int i = 0; i < lReturn.Count; i++)
                                                        {
                                                            if (lReturn[i].SONo.IndexOf(lSORNo) >= 0)
                                                            {
                                                                var lSONOList = lReturn[i].SONo.Split(',').ToList();
                                                                var lReqDateList = lReturn[i].RequiredDate.Split(',').ToList();
                                                                if (lSONOList.Count > 0)
                                                                {
                                                                    for (int j = 0; j < lSONOList.Count; j++)
                                                                    {
                                                                        if (lSONOList[j] == lSORNo)
                                                                        {
                                                                            if (lReqDateList.Count > j)
                                                                            {
                                                                                lReqDateList[j] = lReqDate;
                                                                            }
                                                                            else
                                                                            {
                                                                                lReqDateList.Add(lReqDate);
                                                                            }
                                                                        }

                                                                    }
                                                                }
                                                                var lNewReqDate = "";
                                                                if (lReqDateList.Count > 0)
                                                                {
                                                                    for (int j = 0; j < lReqDateList.Count; j++)
                                                                    {
                                                                        if (lNewReqDate == "")
                                                                        {
                                                                            lNewReqDate = lReqDateList[j];
                                                                        }
                                                                        else
                                                                        {
                                                                            if (lNewReqDate.IndexOf(lReqDateList[j]) < 0)
                                                                            {
                                                                                lNewReqDate = lNewReqDate + "," + lReqDateList[j];
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                if (lNewReqDate == "")
                                                                {
                                                                    lNewReqDate = lReqDate;
                                                                }

                                                                var lDeliveryDate = lReturn[i].PlanDeliveryDate;
                                                                if (lDeliveryDate == null)
                                                                {
                                                                    lDeliveryDate = "";
                                                                }
                                                                //lDeliveryDate = (lReturn[i].PlanDeliveryDate == null || lReturn[i].PlanDeliveryDate.Trim() == "") ? lPlanDelDate : ((lPlanDelDate != null && lPlanDelDate != "" && lReturn[i].PlanDeliveryDate.IndexOf(lPlanDelDate) < 0) ? lReturn[i].PlanDeliveryDate + "," + lPlanDelDate : lReturn[i].PlanDeliveryDate);

                                                                if (lPlanDelDate != null && lPlanDelDate != "")
                                                                {
                                                                    if (lDeliveryDate.Trim() == "")
                                                                    {
                                                                        if (lSORNo.Substring(0, 3) == "103")
                                                                        {
                                                                            if (lReturn[i].SONo.LastIndexOf(",") > 0 && (lReturn[i].SONo.Substring(0, 1) == "8" || lReturn[i].SONo.IndexOf(",8") >= 0))
                                                                            {
                                                                                lDeliveryDate = lPlanDelDate + "(SB)";
                                                                            }
                                                                            else
                                                                            {
                                                                                lDeliveryDate = lPlanDelDate;
                                                                            }
                                                                        }
                                                                        else if (lReturn[i].SONo.LastIndexOf(",") > 0 && (lReturn[i].SONo.Substring(0, 3) == "103" || lReturn[i].SONo.IndexOf(",103") > 0))
                                                                        {
                                                                            if (lBBSNo.IndexOf("-COUPLER") > 0 || lBBSNo.IndexOf("-CP") > 0)
                                                                            {
                                                                                lDeliveryDate = lPlanDelDate + "(CP)";
                                                                            }
                                                                            else
                                                                            {
                                                                                lDeliveryDate = lPlanDelDate + "(CAB)";
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            if (lReturn[i].SONo.LastIndexOf(",") > 0 && (lReturn[i].BBSNo.IndexOf("-COUPLER") > 0 || lReturn[i].BBSNo.IndexOf("-CP") > 0))
                                                                            {
                                                                                if (lBBSNo.IndexOf("-COUPLER") > 0 || lBBSNo.IndexOf("-CP") > 0)
                                                                                {
                                                                                    lDeliveryDate = lPlanDelDate + "(CP)";
                                                                                }
                                                                                else
                                                                                {
                                                                                    lDeliveryDate = lPlanDelDate + "(CAB)";
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                lDeliveryDate = lPlanDelDate;
                                                                            }
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (lDeliveryDate.IndexOf(lPlanDelDate) < 0)
                                                                        {
                                                                            if (lSORNo.Substring(0, 3) == "103")
                                                                            {
                                                                                if (lReturn[i].SONo.LastIndexOf(",") > 0 && (lReturn[i].SONo.Substring(0, 1) == "8" || lReturn[i].SONo.IndexOf(",8") >= 0))
                                                                                {
                                                                                    lDeliveryDate = lDeliveryDate + "," + lPlanDelDate + "(SB)";
                                                                                }
                                                                                else
                                                                                {
                                                                                    lDeliveryDate = lDeliveryDate + "," + lPlanDelDate;
                                                                                }
                                                                            }
                                                                            else if (lReturn[i].SONo.LastIndexOf(",") > 0 && (lReturn[i].SONo.Substring(0, 3) == "103" || lReturn[i].SONo.IndexOf(",103") > 0))
                                                                            {
                                                                                if (lBBSNo.IndexOf("-COUPLER") > 0 || lBBSNo.IndexOf("-CP") > 0)
                                                                                {
                                                                                    lDeliveryDate = lDeliveryDate + "," + lPlanDelDate + "(CP)";
                                                                                }
                                                                                else
                                                                                {
                                                                                    lDeliveryDate = lDeliveryDate + "," + lPlanDelDate + "(CAB)";
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                if (lReturn[i].SONo.LastIndexOf(",") > 0 && (lReturn[i].BBSNo.IndexOf("-COUPLER") > 0 || lReturn[i].BBSNo.IndexOf("-CP") > 0))
                                                                                {
                                                                                    if (lBBSNo.IndexOf("-COUPLER") > 0 || lBBSNo.IndexOf("-CP") > 0)
                                                                                    {
                                                                                        lDeliveryDate = lDeliveryDate + "," + lPlanDelDate + "(CP)";
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        lDeliveryDate = lDeliveryDate + "," + lPlanDelDate + "(CAB)";
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    lDeliveryDate = lDeliveryDate + "," + lPlanDelDate;
                                                                                }
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            if (lDeliveryDate.IndexOf(",") < 0 && lDeliveryDate.IndexOf("(") > 0)
                                                                            {
                                                                                lDeliveryDate = lDeliveryDate.Substring(0, lDeliveryDate.IndexOf("("));
                                                                            }
                                                                        }
                                                                    }
                                                                }

                                                                lReturn[i] = new
                                                                {
                                                                    SSNNo = lReturn[i].SSNNo,
                                                                    OrderNo = lReturn[i].OrderNo,
                                                                    WBS1 = lReturn[i].WBS1,
                                                                    WBS2 = lReturn[i].WBS2,
                                                                    WBS3 = lReturn[i].WBS3,
                                                                    StructureElement = lReturn[i].StructureElement,
                                                                    ProdType = lReturn[i].ProdType,
                                                                    PONo = lReturn[i].PONo,
                                                                    PODate = lReturn[i].PODate,
                                                                    RequiredDate = lNewReqDate,
                                                                    OrderWeight = lReturn[i].OrderWeight,
                                                                    SubmittedBy = lReturn[i].SubmittedBy,
                                                                    OrderStatus = lReturn[i].OrderStatus,
                                                                    OrderSource = lReturn[i].OrderSource,
                                                                    SONo = lReturn[i].SONo,
                                                                    SORNo = lReturn[i].SORNo,
                                                                    BBSNo = lReturn[i].BBSNo,
                                                                    BBSDesc = WebUtility.HtmlDecode(lReturn[i].BBSDesc),
                                                                    CustomerCode = lReturn[i].CustomerCode,
                                                                    ProjectCode = lReturn[i].ProjectCode,
                                                                    DataEnteredBy = lReturn[i].DataEnteredBy,
                                                                    Confirmed = lReturn[i].Confirmed == 1 ? 1 : (lConfirmed == 1 ? 1 : (lReturn[i].Confirmed == 2 ? 2 : lConfirmed)),
                                                                    PlanDeliveryDate = lDeliveryDate,
                                                                    ProjectTitle = lReturn[i].ProjectTitle,
                                                                    Address = lReturn[i].Address,
                                                                    Gate = lReturn[i].Gate
                                                                };
                                                                break;
                                                            }
                                                        }
                                                    }

                                                }
                                            }
                                            lRst.Close();
                                        }
                                    }
                                    #endregion

                                    #region Check Individual SO, but Order Delivered 
                                    lPODateFrom_O = lPODateFrom.Replace("-", "");
                                    lPODateTo_O = lPODateTo.Replace("-", "");
                                    lRDateFrom_O = lRDateFrom.Replace("-", "");
                                    lRDateTo_O = lRDateTo.Replace("-", "");

                                    var lBalSNo = 0;

                                    lSQL = "SELECT " +
"    ISNULL(D.WBS1, ' ') AS WBS1, " +
"    ISNULL(D.WBS2, ' ') AS WBS2, " +
"    ISNULL(D.WBS3, ' ') AS WBS3, " +
"    ISNULL(D.ST_ELEMENT_TYPE, ' ') AS ST_ELE, " +
"    ISNULL(D.PRODUCT_TYPE, ' ') AS Prod_type, " +
"    P.PO_NUMBER, " +
"    P.CUST_ORDER_DATE, " +
"    P.REQ_DAT_TO, " +
"    P.FIRST_PROMISED_D, " +
"    (SELECT ISNULL(SUM(CAST(THEO_WGT AS FLOAT)), 0)/1000 " +
"       FROM OESOrderItemHMI " +
"       WHERE ORDER_NO = P.SALES_ORDER) AS WT, " +
"    '' AS SubumittedBY, " +
"    'Reviewed' AS ReviewStatus, " +
"    (SELECT ISNULL(MAX(PROD_TYPE), ' ') " +
"       FROM OESOrderItemHMI " +
"       WHERE ORDER_NO = P.SALES_ORDER) AS ProdType2, " +
"    ' ' AS Placeholder, " +
"    ISNULL(P.SALES_ORDER, ' ') AS SALES_ORDER, " +
"    (SELECT TOP 1 CONVERT(varchar, CONVERT(date, PLAN_SHIPPING_DATE, 103), 112) " +
"       FROM salesorderloading sl " +
"       INNER JOIN hmiloaddetails ld ON ld.load_no = sl.load_no " +
"       WHERE ld.sales_order = P.sales_order) AS PLAN_DEL_DATE, " +
"    (SELECT CONVERT(varchar, CONVERT(date, MIN(confirmdeldate), 103), 112) " +
"       FROM HMIOrderHeaderDetails " +
"       WHERE OrderNo = P.sales_order) AS PLAN_DEL_CONF, " +
"    P.ORDER_REQUEST_NO, " +
"    D.BBS_NO, " +
"    D.BBS_DESC, " +
"    P.KUNAG, " +
"    P.KUNNR, " +
"    P.NAME_WE, " +
 "(SELECT ProjectTitle FROM dbo.OESProject " +
                        "WHERE CustomerCode = M.CustomerCode " +
                        "AND ProjectCode = M.ProjectCode) as ProjectTitle, " +
"    ISNULL(M.AddressCode, '') AS AddressCode, " +   // ✅ now from OesProjOrder (alias M)
"    ISNULL(M.Gate, '') AS Gate " +   // ✅ now from OesProjOrder (alias M)

"FROM OesProjOrder M " +
"LEFT JOIN OesOrderHeaderHMI P " +
" ON P.ODOS_ID = M.OrderNumber " +

"LEFT JOIN OesRequestDetailsHMI D " +
"    ON P.MANDT = D.MANDT " +
"   AND P.ORDER_REQUEST_NO = D.ORDER_REQUEST_NO " +
"WHERE P.MANDT = '" + lProcessObj.strClient + "' " +
"AND P.KUNAG = '" + CustomerCode + "' " +
"AND P.PROJECT IN (" + lProjectState + ") " +
" " + lAddressState + " " +
"AND P.STATUS <> 'X' " +
"AND P.REQD_DEL_DATE >= GETDATE() - 180 " +
"AND NOT EXISTS ( " +
"    SELECT SALES_ORDER FROM DeliveredOrderdetailsHMI " +
"    WHERE PARTIAL_DEL_IND = 'Completed' " +
"    AND SALES_ORDER IN (SELECT SALES_ORDER FROM HMILoadDetails WHERE SALES_ORDER = P.SALES_ORDER) " +
") " +
"AND NOT EXISTS ( " +
"    SELECT LOAD_NO FROM SALESORDERLOADING " +
"    WHERE TRY_CONVERT(DATETIME, PLAN_SHIPPING_DATE, 103) <= GETDATE() " +
"    AND LOAD_NO IN (SELECT LOAD_NO FROM HMILoadDetails WHERE SALES_ORDER = P.SALES_ORDER) " +
") " +
"AND ((P.CUST_ORDER_DATE BETWEEN '" + lPODateFrom_O + "' AND '" + lPODateTo_O + "') " +
"     OR P.CUST_ORDER_DATE IS NULL " +
"     OR P.CUST_ORDER_DATE = ' ') " +
"AND ((P.REQD_DEL_DATE BETWEEN '" + lRDateFrom_O + "' AND '" + lRDateTo_O + "') " +
"     OR P.REQD_DEL_DATE IS NULL " +
"     OR P.REQD_DEL_DATE = ' ') " +
"AND (D.STATUS <> 'X' OR D.STATUS IS NULL) " +
"ORDER BY 7";



                                    lCmd.CommandText = lSQL;
                                    lCmd.Connection = lNDSCon;
                                    lCmd.CommandTimeout = 300;
                                    lRst = (SqlDataReader)await lCmd.ExecuteReaderAsync();
                                    if (lRst.HasRows)
                                    {
                                        while (lRst.Read())
                                        {
                                            var lBBSNo = lRst.GetValue(18) == DBNull.Value ? "" : lRst.GetString(18);
                                            var lBBSDesc = WebUtility.HtmlDecode(lRst.GetValue(19) == DBNull.Value ? "" : lRst.GetString(19));
                                            var lSORNo = lRst.GetString(17).Trim();
                                            if (lSOR.IndexOf(lSORNo) < 0)
                                            {
                                                lBalSNo = lBalSNo + 1;

                                                int lConfirmed = 0;

                                                var lPODate = (lRst.GetValue(6) == DBNull.Value ? "" : lRst.GetString(6));
                                                lPODate = String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lPODate, "yyyyMMdd", CultureInfo.InvariantCulture));

                                                //var lReqDate = (lOraRst.GetValue(8) == DBNull.Value ? "" : lOraRst.GetString(8).Trim());
                                                //if (lReqDate == "")
                                                //{
                                                var lReqDate = (lRst.GetValue(7) == DBNull.Value ? "" : lRst.GetString(7).Trim());
                                                //} else
                                                //{
                                                //    if (lReqDate.Length > 10)
                                                //    {
                                                //        lReqDate = lReqDate.Substring(0, 10);
                                                //    }
                                                //    lReqDate = String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lReqDate, "yyyy-MM-dd", CultureInfo.InvariantCulture));
                                                //}

                                                //var lReqDateConf = (lOraRst.GetValue(13) == DBNull.Value ? "" : lOraRst.GetString(13).Trim());
                                                //if (lReqDateConf != null && lReqDateConf != "")
                                                //{
                                                //    lReqDate = String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lReqDateConf, "yyyyMMdd", CultureInfo.InvariantCulture));
                                                //}

                                                var lPlanDelDate = (lRst.GetValue(15) == DBNull.Value ? "" : lRst.GetString(15).Trim());
                                                if (lPlanDelDate != null && lPlanDelDate != "" && lPlanDelDate != "20500101")
                                                {
                                                    if (String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lPlanDelDate, "yyyyMMdd", CultureInfo.InvariantCulture))
                                                        == (String.Format("{0:yyyy-MM-dd}", DateTime.Now)))
                                                    {
                                                        lConfirmed = 1;
                                                    }
                                                    else if (String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lPlanDelDate, "yyyyMMdd", CultureInfo.InvariantCulture))
                                                        == (String.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(1))))
                                                    {
                                                        lConfirmed = 2;
                                                    }
                                                    else
                                                    {
                                                        lConfirmed = 3;
                                                    }
                                                    lPlanDelDate = String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lPlanDelDate, "yyyyMMdd", CultureInfo.InvariantCulture));
                                                }
                                                else
                                                {
                                                    lPlanDelDate = "";
                                                }

                                                //var lPlanDelConfDate = (lOraRst.GetValue(16) == DBNull.Value ? "" : lOraRst.GetString(16).Trim());
                                                //if (lPlanDelConfDate != null && lPlanDelConfDate != "")
                                                //{
                                                //    if (String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lPlanDelConfDate, "yyyyMMdd", CultureInfo.InvariantCulture))
                                                //        == (String.Format("{0:yyyy-MM-dd}", DateTime.Now)))
                                                //    {
                                                //        lConfirmed = 1;
                                                //    }
                                                //    else if (String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lPlanDelConfDate, "yyyyMMdd", CultureInfo.InvariantCulture))
                                                //        == (String.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(1))))
                                                //    {
                                                //        lConfirmed = 2;
                                                //    }
                                                //    else
                                                //    {
                                                //        lConfirmed = 3;
                                                //    }
                                                //    //lReqDate = String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lPlanDelConfDate, "yyyyMMdd", CultureInfo.InvariantCulture));
                                                //}

                                                if (lReqDate.Length > 10)
                                                {
                                                    lReqDate = lReqDate.Substring(0, 10);
                                                }

                                                var lProdType = (lRst.GetValue(4) == DBNull.Value ? "" : lRst.GetString(4).Trim());
                                                if (lProdType.Trim() == "")
                                                {
                                                    var lProdType2 = (lRst.GetValue(12) == DBNull.Value ? "" : lRst.GetString(12).Trim());
                                                    if (lProdType2 == "MSHSTM")
                                                    {
                                                        lProdType = "STANDARD-MESH";
                                                    }
                                                    else if (lProdType2 == "BARDBX" || lProdType2 == "BARRBX")
                                                    {
                                                        lProdType = "STANDARD-BAR";
                                                    }
                                                    else if (lProdType2 == "BARDBI")
                                                    {
                                                        lProdType = "DBIC";
                                                    }
                                                    else if (lProdType2 == "BARRBI")
                                                    {
                                                        lProdType = "RBIC";
                                                    }
                                                    else if (lProdType2 == "COUNSP")
                                                    {
                                                        lProdType = "COUPLER";
                                                    }
                                                    else if (lProdType2 == "CRWWPR")
                                                    {
                                                        lProdType = "Cold Rolled Wire";
                                                    }
                                                    else if (lProdType2 == "PCSPCS")
                                                    {
                                                        lProdType = "PC Strand";
                                                    }
                                                    else if (lProdType2 == "WRDWRD")
                                                    {
                                                        lProdType = "Wire Rod";
                                                    }
                                                    else
                                                    {
                                                        lProdType = lProdType2;
                                                    }
                                                }
                                                lReturn.Add(new
                                                {
                                                    SSNNo = lBalSNo,
                                                    OrderNo = "N" + lBalSNo.ToString(),
                                                    WBS1 = (lRst.GetValue(0) == DBNull.Value ? "" : lRst.GetString(0).Trim()),
                                                    WBS2 = (lRst.GetValue(1) == DBNull.Value ? "" : lRst.GetString(1).Trim()),
                                                    WBS3 = (lRst.GetValue(2) == DBNull.Value ? "" : lRst.GetString(2).Trim()),
                                                    StructureElement = lRst.GetValue(3) == DBNull.Value ? "" : (lRst.GetString(3).Trim() == "NONWBS" ? "" : lRst.GetString(3).Trim()),
                                                    ProdType = lProdType,
                                                    PONo = (lRst.GetValue(5) == DBNull.Value ? "" : lRst.GetString(5).Trim()),
                                                    PODate = lPODate,
                                                    RequiredDate = lReqDate,
                                                    OrderWeight = (lRst.GetValue(9) == DBNull.Value ? "0.000" : lRst.GetDouble(9).ToString("###,###,##0.000;(###,##0.000); ")),
                                                    SubmittedBy = (lRst.GetValue(10) == DBNull.Value ? "" : lRst.GetString(10).Trim()),
                                                    OrderStatus = "Reviewed",
                                                    OrderSource = "SAP",
                                                    SONo = (lRst.GetValue(14) == DBNull.Value || lRst.GetString(14) == "") ? lSORNo : lRst.GetString(14).Trim(),
                                                    SORNo = lSORNo,
                                                    BBSNo = lBBSNo,
                                                    BBSDesc = lBBSDesc,
                                                    CustomerCode = (lRst.GetValue(20) == DBNull.Value ? "" : lRst.GetString(20).Trim()),
                                                    ProjectCode = (lRst.GetValue(21) == DBNull.Value ? "" : lRst.GetString(21).Trim()),
                                                    DataEnteredBy = "",
                                                    Confirmed = lConfirmed,
                                                    PlanDeliveryDate = lPlanDelDate,
                                                    ProjectTitle = (lRst.GetValue(23) == DBNull.Value ? "" : lRst.GetString(23).Trim()),
                                                    Address = (lRst.GetValue(24) == DBNull.Value ? "" : lRst.GetString(24).Trim()),
                                                    Gate = (lRst.GetValue(25) == DBNull.Value ? "" : lRst.GetString(25).Trim())
                                                    //SubmitBy = (lRst.GetValue(18) == DBNull.Value ? "" : lRst.GetString(18).Trim())
                                                });

                                            }
                                        }
                                    }
                                    lRst.Close();
                                    #endregion


                                    #region Check Partial Delivery
                                    lBalSNo = 0;

                                    lSQL = "SELECT " +
                                                                    "D.WBS1, " +
                                                                    "D.WBS2, " +
                                                                    "D.WBS3, " +
                                                                    "D.ST_ELEMENT_TYPE, " +
                                                                    "D.PRODUCT_TYPE, " +
                                                                    "M.PO_NUMBER, " +
                                                                    "M.CUST_ORDER_DATE, " +
                                                                    "M.REQ_DAT_TO, " +
                                                                    "M.FIRST_PROMISED_D, " +
                                                                    "(SELECT ISNULL(SUM(CAST(THEO_WGT AS FLOAT)), 0)/1000 " +
                                                                    " FROM OESOrderItemHMI WHERE ORDER_NO = M.SALES_ORDER) AS WT, " +
                                                                    "'' AS SubumittedBY, " +
                                                                    "'Production', " +
                                                                    "(SELECT ISNULL(MAX(PROD_TYPE), ' ') FROM OESOrderItemHMI WHERE ORDER_NO = M.SALES_ORDER) AS ProdType2, " +
                                                                    "' ', " +
                                                                    "M.SALES_ORDER, " +
                                                                    "(SELECT TOP 1 CONVERT(varchar, CONVERT(date, PLAN_SHIPPING_DATE, 103), 112) " +
                                                                    " FROM salesorderloading sl " +
                                                                    " INNER JOIN hmiloaddetails ld ON ld.load_no = sl.load_no " +
                                                                    " WHERE ld.sales_order = M.sales_order) AS PLAN_DEL_DATE, " +
                                                                    "(SELECT CONVERT(varchar, CONVERT(date, MIN(confirmdeldate), 103), 112) FROM HMIOrderHeaderDetails WHERE OrderNo = M.sales_order) AS PLAN_DEL_CONF, " +
                                                                    "M.ORDER_REQUEST_NO, " +
                                                                    "D.BBS_NO, " +
                                                                    "D.BBS_DESC, " +
                                                                    "M.KUNAG, " +
                                                                    "M.KUNNR, " +
                                                                    "M.NAME_WE " +

                                                                    "FROM OesOrderHeaderHMI M " +
                                                                    "LEFT OUTER JOIN OesRequestDetailsHMI D " +
                                                                    "ON M.MANDT = D.MANDT AND M.ORDER_REQUEST_NO = D.ORDER_REQUEST_NO " +
                                                                    "WHERE M.MANDT = '" + lProcessObj.strClient + "' " +
                                                                    "AND M.KUNAG = '" + CustomerCode + "' " +
                                                                    "AND M.PROJECT IN (" + lProjectState + ") " +
                                                                    "AND M.REQD_DEL_DATE >= GETDATE() - 180 " +
                                                                    "AND M.STATUS <> 'X' " +

                                                                    "AND EXISTS ( " +
                                                                    " SELECT LOAD_NO FROM SALESORDERLOADING " +
                                                                    " WHERE TRY_CONVERT(DATETIME, PLAN_SHIPPING_DATE, 103) <= GETDATE() - 10 " +
                                                                    " AND LOAD_NO IN (SELECT LOAD_NO FROM HMILoadDetails WHERE SALES_ORDER = M.SALES_ORDER) " +
                                                                    ") " +

                                                                    "AND EXISTS ( " +
                                                                    " SELECT SALES_ORDER FROM DeliveredOrderdetailsHMI " +
                                                                    " WHERE PARTIAL_DEL_IND <> 'Completed' " +
                                                                    " AND LOAD_NO IN ( " +
                                                                    "     SELECT LOAD_NO FROM SALESORDERLOADING " +
                                                                    "     WHERE TRY_CONVERT(DATETIME, PLAN_SHIPPING_DATE, 103) <= GETDATE() + 30 " +
                                                                    "     AND LOAD_NO IN (SELECT LOAD_NO FROM HMILoadDetails WHERE SALES_ORDER = M.SALES_ORDER) " +
                                                                    " )) " +

                                                                    "AND ( " +
                                                                    " (M.CUST_ORDER_DATE >= '" + lPODateFrom_O + "' AND M.CUST_ORDER_DATE <= '" + lPODateTo_O + "') " +
                                                                    " OR M.CUST_ORDER_DATE IS NULL " +
                                                                    " OR M.CUST_ORDER_DATE = ' ' " +
                                                                    ") " +

                                                                    "AND ( " +
                                                                    " (M.REQD_DEL_DATE >= '" + lRDateFrom_O + "' AND M.REQD_DEL_DATE <= '" + lRDateTo_O + "') " +
                                                                    " OR M.REQD_DEL_DATE IS NULL " +
                                                                    " OR M.REQD_DEL_DATE = ' ' " +
                                                                    ") " +

                                                                    "AND (D.STATUS <> 'X' OR D.STATUS IS NULL)";


                                    lCmd.CommandText = lSQL;
                                    lCmd.Connection = lNDSCon;
                                    lCmd.CommandTimeout = 300;
                                    lRst = (SqlDataReader)await lCmd.ExecuteReaderAsync();
                                    if (lRst.HasRows)
                                    {
                                        while (lRst.Read())
                                        {
                                            var lBBSNo = lRst.GetValue(18) == DBNull.Value ? "" : lRst.GetString(18);
                                            var lBBSDesc = WebUtility.HtmlDecode(lRst.GetValue(19) == DBNull.Value ? "" : lRst.GetString(19));
                                            var lSORNo = lRst.GetString(17).Trim();
                                            int lInd = lReturn.FindIndex(f => f.SONo == lSORNo);
                                            if (lInd < 0)
                                            {
                                                lBalSNo = lBalSNo + 1;

                                                int lConfirmed = 0;

                                                var lPODate = (lRst.GetValue(6) == DBNull.Value ? "" : lRst.GetString(6));
                                                lPODate = String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lPODate, "yyyyMMdd", CultureInfo.InvariantCulture));

                                                //var lReqDate = (lRst.GetValue(8) == DBNull.Value ? "" : lRst.GetString(8).Trim());
                                                //if (lReqDate == "")
                                                //{
                                                var lReqDate = (lRst.GetValue(7) == DBNull.Value ? "" : lRst.GetString(7).Trim());
                                                //} else
                                                //{
                                                //    if (lReqDate.Length > 10)
                                                //    {
                                                //        lReqDate = lReqDate.Substring(0, 10);
                                                //    }
                                                //    lReqDate = String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lReqDate, "yyyy-MM-dd", CultureInfo.InvariantCulture));
                                                //}

                                                //var lReqDateConf = (lRst.GetValue(13) == DBNull.Value ? "" : lRst.GetString(13).Trim());
                                                //if (lReqDateConf != null && lReqDateConf != "")
                                                //{
                                                //    lReqDate = String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lReqDateConf, "yyyyMMdd", CultureInfo.InvariantCulture));
                                                //}

                                                var lPlanDelDate = (lRst.GetValue(15) == DBNull.Value ? "" : lRst.GetString(15).Trim());
                                                if (lPlanDelDate != null && lPlanDelDate != "" && lPlanDelDate != "20500101")
                                                {
                                                    if (String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lPlanDelDate, "yyyyMMdd", CultureInfo.InvariantCulture))
                                                        == (String.Format("{0:yyyy-MM-dd}", DateTime.Now)))
                                                    {
                                                        lConfirmed = 1;
                                                    }
                                                    else if (String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lPlanDelDate, "yyyyMMdd", CultureInfo.InvariantCulture))
                                                        == (String.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(1))))
                                                    {
                                                        lConfirmed = 2;
                                                    }
                                                    else
                                                    {
                                                        lConfirmed = 3;
                                                    }
                                                    lPlanDelDate = String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lPlanDelDate, "yyyyMMdd", CultureInfo.InvariantCulture));
                                                }
                                                else
                                                {
                                                    lPlanDelDate = "";
                                                }

                                                //var lPlanDelConfDate = (lRst.GetValue(16) == DBNull.Value ? "" : lRst.GetString(16).Trim());
                                                //if (lPlanDelConfDate != null && lPlanDelConfDate != "")
                                                //{
                                                //    if (String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lPlanDelConfDate, "yyyyMMdd", CultureInfo.InvariantCulture))
                                                //        == (String.Format("{0:yyyy-MM-dd}", DateTime.Now)))
                                                //    {
                                                //        lConfirmed = 1;
                                                //    }
                                                //    else if (String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lPlanDelConfDate, "yyyyMMdd", CultureInfo.InvariantCulture))
                                                //        == (String.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(1))))
                                                //    {
                                                //        lConfirmed = 2;
                                                //    }
                                                //    else
                                                //    {
                                                //        lConfirmed = 3;
                                                //    }
                                                //    //lReqDate = String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lPlanDelConfDate, "yyyyMMdd", CultureInfo.InvariantCulture));
                                                //}

                                                if (lReqDate.Length > 10)
                                                {
                                                    lReqDate = lReqDate.Substring(0, 10);
                                                }

                                                var lProdType = (lRst.GetValue(4) == DBNull.Value ? "" : lRst.GetString(4).Trim());
                                                if (lProdType.Trim() == "")
                                                {
                                                    var lProdType2 = (lRst.GetValue(12) == DBNull.Value ? "" : lRst.GetString(12).Trim());
                                                    if (lProdType2 == "MSHSTM")
                                                    {
                                                        lProdType = "STANDARD-MESH";
                                                    }
                                                    else if (lProdType2 == "BARDBX" || lProdType2 == "BARRBX")
                                                    {
                                                        lProdType = "STANDARD-BAR";
                                                    }
                                                    else if (lProdType2 == "BARDBI")
                                                    {
                                                        lProdType = "DBIC";
                                                    }
                                                    else if (lProdType2 == "BARRBI")
                                                    {
                                                        lProdType = "RBIC";
                                                    }
                                                    else if (lProdType2 == "COUNSP")
                                                    {
                                                        lProdType = "COUPLER";
                                                    }
                                                    else if (lProdType2 == "CRWWPR")
                                                    {
                                                        lProdType = "Cold Rolled Wire";
                                                    }
                                                    else if (lProdType2 == "PCSPCS")
                                                    {
                                                        lProdType = "PC Strand";
                                                    }
                                                    else if (lProdType2 == "WRDWRD")
                                                    {
                                                        lProdType = "Wire Rod";
                                                    }
                                                    else
                                                    {
                                                        lProdType = lProdType2;
                                                    }
                                                }
                                                lReturn.Add(new
                                                {
                                                    SSNNo = lBalSNo,
                                                    OrderNo = "P" + lBalSNo.ToString(),
                                                    WBS1 = (lRst.GetValue(0) == DBNull.Value ? "" : lRst.GetString(0).Trim()),
                                                    WBS2 = (lRst.GetValue(1) == DBNull.Value ? "" : lRst.GetString(1).Trim()),
                                                    WBS3 = (lRst.GetValue(2) == DBNull.Value ? "" : lRst.GetString(2).Trim()),
                                                    StructureElement = lRst.GetValue(3) == DBNull.Value ? "" : (lRst.GetString(3).Trim() == "NONWBS" ? "" : lRst.GetString(3).Trim()),
                                                    ProdType = lProdType,
                                                    PONo = (lRst.GetValue(5) == DBNull.Value ? "" : lRst.GetString(5).Trim()),
                                                    PODate = lPODate,
                                                    RequiredDate = lReqDate,
                                                    OrderWeight = (lRst.GetValue(9) == DBNull.Value ? "0.000" : lRst.GetDecimal(9).ToString("###,###,##0.000;(###,##0.000); ")),
                                                    SubmittedBy = (lRst.GetValue(10) == DBNull.Value ? "" : lRst.GetString(10).Trim()),
                                                    OrderStatus = lSORNo.Substring(0, 1) == "1" ? "Reviewed" : "Production",
                                                    OrderSource = "SAP",
                                                    SONo = lRst.GetValue(14) == DBNull.Value ? "" : lRst.GetString(14).Trim(),
                                                    SORNo = lSORNo,
                                                    BBSNo = lBBSNo,
                                                    BBSDesc = lBBSDesc,
                                                    CustomerCode = (lRst.GetValue(20) == DBNull.Value ? "" : lRst.GetString(20).Trim()),
                                                    ProjectCode = (lRst.GetValue(21) == DBNull.Value ? "" : lRst.GetString(21).Trim()),
                                                    DataEnteredBy = "",
                                                    Confirmed = lConfirmed,
                                                    PlanDeliveryDate = lPlanDelDate,
                                                    ProjectTitle = (lRst.GetValue(22) == DBNull.Value ? "" : lRst.GetString(22).Trim()),
                                                    Address = (lRst.GetValue(24) == DBNull.Value ? "" : lRst.GetString(24).Trim()),
                                                    Gate = (lRst.GetValue(25) == DBNull.Value ? "" : lRst.GetString(25).Trim())
                                                });

                                            }
                                        }
                                    }
                                    lRst.Close();
                                    #endregion

                                    //Check Production Status
                                    var lSTSCon = new SqlConnection();
                                    var lSOs = "";
                                    var lTagSO = new List<string>();

                                    try
                                    {
                                        if (lProcessObj.OpenSTSConnection(ref lSTSCon) == true)
                                        {

                                            #region CAB
                                            if (lReturn.Count > 0)
                                            {
                                                for (int i = 0; i < lReturn.Count; i++)
                                                {
                                                    if (lReturn[i].SONo != null && lReturn[i].SONo != "" &&
                                                        lReturn[i].OrderSource == "SAP" &&
                                                        lReturn[i].ProdType == "CAB")
                                                    {
                                                        if (lSOs == "")
                                                        {
                                                            lSOs = "'" + lReturn[i].SONo + "'";
                                                        }
                                                        else
                                                        {
                                                            lSOs = lSOs + "," + "'" + lReturn[i].SONo + "'";
                                                        }
                                                    }
                                                }
                                            }

                                            if (lSOs != null && lSOs != "")
                                            {
                                                lSQL = "SELECT ORDER_NO " +
                                                "FROM dbo.tbl_scm_sts_cab_print_tag " +
                                                "WHERE ORDER_NO IN (" + lSOs + ") " +
                                                "GROUP BY ORDER_NO ";

                                                lCmd.CommandText = lSQL;
                                                lCmd.Connection = lSTSCon;
                                                lCmd.CommandTimeout = 300;
                                                lRst = lCmd.ExecuteReader();
                                                if (lRst.HasRows)
                                                {
                                                    while (lRst.Read())
                                                    {
                                                        if (lRst.GetString(0).Trim() != "")
                                                        {
                                                            lTagSO.Add(lRst.GetString(0).Trim());
                                                        }

                                                    }
                                                }
                                                lRst.Close();

                                                for (int i = 0; i < lTagSO.Count; i++)
                                                {
                                                    for (int j = 0; j < lReturn.Count; j++)
                                                    {
                                                        if (lTagSO[i] == lReturn[j].SONo)
                                                        {
                                                            lReturn.Insert(j + 1, new
                                                            {
                                                                SSNNo = lReturn[j].SSNNo,
                                                                OrderNo = lReturn[j].OrderNo,
                                                                WBS1 = lReturn[j].WBS1,
                                                                WBS2 = lReturn[j].WBS2,
                                                                WBS3 = lReturn[j].WBS3,
                                                                StructureElement = lReturn[j].StructureElement,
                                                                ProdType = lReturn[j].ProdType,
                                                                PONo = lReturn[j].PONo,
                                                                PODate = lReturn[j].PODate,
                                                                RequiredDate = lReturn[j].RequiredDate,
                                                                OrderWeight = lReturn[j].OrderWeight,
                                                                SubmittedBy = lReturn[j].SubmittedBy,
                                                                OrderStatus = "Production",
                                                                OrderSource = lReturn[j].OrderSource,
                                                                SONo = lReturn[j].SONo,
                                                                SORNo = lReturn[j].SORNo,
                                                                BBSNo = lReturn[j].BBSNo,
                                                                BBSDesc = lReturn[j].BBSDesc,
                                                                CustomerCode = lReturn[j].CustomerCode,
                                                                ProjectCode = lReturn[j].ProjectCode,
                                                                DataEnteredBy = lReturn[j].DataEnteredBy,
                                                                Confirmed = lReturn[j].Confirmed,
                                                                PlanDeliveryDate = lReturn[j].PlanDeliveryDate,
                                                                ProjectTitle = lReturn[j].ProjectTitle,
                                                                Address = lReturn[j].Address,
                                                                Gate = lReturn[j].Gate
                                                            });
                                                            lReturn.RemoveAt(j);
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                            #endregion

                                            #region MESH
                                            lSOs = "";
                                            lTagSO = new List<string>();

                                            if (lReturn.Count > 0)
                                            {
                                                for (int i = 0; i < lReturn.Count; i++)
                                                {
                                                    if (lReturn[i].SONo != null && lReturn[i].SONo != "" &&
                                                        lReturn[i].OrderSource == "SAP" &&
                                                        (lReturn[i].ProdType == "CUT-TO-SIZE-MESH" ||
                                                        lReturn[i].ProdType == "STIRRUP-LINK-MESH" ||
                                                        lReturn[i].ProdType == "COLUMN-LINK-MESH")
                                                        )
                                                    {
                                                        if (lSOs == "")
                                                        {
                                                            lSOs = "'" + lReturn[i].SONo + "'";
                                                        }
                                                        else
                                                        {
                                                            lSOs = lSOs + "," + "'" + lReturn[i].SONo + "'";
                                                        }
                                                    }
                                                }
                                            }

                                            if (lSOs != null && lSOs != "")
                                            {
                                                lSQL = "SELECT MSD_PRT_SO_NO " +
                                                "FROM dbo.tbl_scm_sts_mesh_print_tag " +
                                                "WHERE MSD_PRT_SO_NO IN (" + lSOs + ") " +
                                                "GROUP BY MSD_PRT_SO_NO ";

                                                lCmd.CommandText = lSQL;
                                                lCmd.Connection = lSTSCon;
                                                lCmd.CommandTimeout = 300;
                                                lRst = lCmd.ExecuteReader();
                                                if (lRst.HasRows)
                                                {
                                                    while (lRst.Read())
                                                    {
                                                        if (lRst.GetString(0).Trim() != "")
                                                        {
                                                            lTagSO.Add(lRst.GetString(0).Trim());
                                                        }

                                                    }
                                                }
                                                lRst.Close();

                                                for (int i = 0; i < lTagSO.Count; i++)
                                                {
                                                    for (int j = 0; j < lReturn.Count; j++)
                                                    {
                                                        if (lTagSO[i] == lReturn[j].SONo)
                                                        {
                                                            lReturn.Insert(j + 1, new
                                                            {
                                                                SSNNo = lReturn[j].SSNNo,
                                                                OrderNo = lReturn[j].OrderNo,
                                                                WBS1 = lReturn[j].WBS1,
                                                                WBS2 = lReturn[j].WBS2,
                                                                WBS3 = lReturn[j].WBS3,
                                                                StructureElement = lReturn[j].StructureElement,
                                                                ProdType = lReturn[j].ProdType,
                                                                PONo = lReturn[j].PONo,
                                                                PODate = lReturn[j].PODate,
                                                                RequiredDate = lReturn[j].RequiredDate,
                                                                OrderWeight = lReturn[j].OrderWeight,
                                                                SubmittedBy = lReturn[j].SubmittedBy,
                                                                OrderStatus = "Production",
                                                                OrderSource = lReturn[j].OrderSource,
                                                                SONo = lReturn[j].SONo,
                                                                SORNo = lReturn[j].SORNo,
                                                                BBSNo = lReturn[j].BBSNo,
                                                                BBSDesc = lReturn[j].BBSDesc,
                                                                CustomerCode = lReturn[j].CustomerCode,
                                                                ProjectCode = lReturn[j].ProjectCode,
                                                                DataEnteredBy = lReturn[j].DataEnteredBy,
                                                                Confirmed = lReturn[j].Confirmed,
                                                                PlanDeliveryDate = lReturn[j].PlanDeliveryDate,
                                                                ProjectTitle = lReturn[j].ProjectTitle,
                                                                Address = lReturn[j].Address,
                                                                Gate = lReturn[j].Gate
                                                            });
                                                            lReturn.RemoveAt(j);
                                                            break;
                                                        }

                                                    }
                                                }
                                            }
                                            #endregion

                                            #region PRE-CAGE
                                            lSOs = "";
                                            lTagSO = new List<string>();

                                            if (lReturn.Count > 0)
                                            {
                                                for (int i = 0; i < lReturn.Count; i++)
                                                {
                                                    if (lReturn[i].SONo != null && lReturn[i].SONo != "" &&
                                                        lReturn[i].OrderSource == "SAP" &&
                                                        (lReturn[i].ProdType == "CORE-CAGE" ||
                                                        lReturn[i].ProdType == "PRE-CAGE")
                                                        )
                                                    {
                                                        if (lSOs == "")
                                                        {
                                                            lSOs = "'" + lReturn[i].SONo + "'";
                                                        }
                                                        else
                                                        {
                                                            lSOs = lSOs + "," + "'" + lReturn[i].SONo + "'";
                                                        }
                                                    }
                                                }
                                            }

                                            if (lSOs != null && lSOs != "")
                                            {
                                                lSQL = "SELECT SO_NO " +
                                                "FROM dbo.tbl_scm_sts_prc_print_tag " +
                                                "WHERE SO_NO IN (" + lSOs + ") " +
                                                "GROUP BY SO_NO ";

                                                lCmd.CommandText = lSQL;
                                                lCmd.Connection = lSTSCon;
                                                lCmd.CommandTimeout = 300;
                                                lRst = lCmd.ExecuteReader();
                                                if (lRst.HasRows)
                                                {
                                                    while (lRst.Read())
                                                    {
                                                        if (lRst.GetString(0).Trim() != "")
                                                        {
                                                            lTagSO.Add(lRst.GetString(0).Trim());
                                                        }

                                                    }
                                                }
                                                lRst.Close();

                                                for (int i = 0; i < lTagSO.Count; i++)
                                                {
                                                    for (int j = 0; j < lReturn.Count; j++)
                                                    {
                                                        if (lTagSO[i] == lReturn[j].SONo)
                                                        {
                                                            lReturn.Insert(j + 1, new
                                                            {
                                                                SSNNo = lReturn[j].SSNNo,
                                                                OrderNo = lReturn[j].OrderNo,
                                                                WBS1 = lReturn[j].WBS1,
                                                                WBS2 = lReturn[j].WBS2,
                                                                WBS3 = lReturn[j].WBS3,
                                                                StructureElement = lReturn[j].StructureElement,
                                                                ProdType = lReturn[j].ProdType,
                                                                PONo = lReturn[j].PONo,
                                                                PODate = lReturn[j].PODate,
                                                                RequiredDate = lReturn[j].RequiredDate,
                                                                OrderWeight = lReturn[j].OrderWeight,
                                                                SubmittedBy = lReturn[j].SubmittedBy,
                                                                OrderStatus = "Production",
                                                                OrderSource = lReturn[j].OrderSource,
                                                                SONo = lReturn[j].SONo,
                                                                SORNo = lReturn[j].SORNo,
                                                                BBSNo = lReturn[j].BBSNo,
                                                                BBSDesc = lReturn[j].BBSDesc,
                                                                CustomerCode = lReturn[j].CustomerCode,
                                                                ProjectCode = lReturn[j].ProjectCode,
                                                                DataEnteredBy = lReturn[j].DataEnteredBy,
                                                                Confirmed = lReturn[j].Confirmed,
                                                                PlanDeliveryDate = lReturn[j].PlanDeliveryDate,
                                                                ProjectTitle = lReturn[j].ProjectTitle,
                                                                Address = lReturn[j].Address,
                                                                Gate = lReturn[j].Gate
                                                            });
                                                            lReturn.RemoveAt(j);
                                                            break;
                                                        }

                                                    }
                                                }
                                            }
                                            #endregion

                                            #region BPC
                                            lSOs = "";
                                            lTagSO = new List<string>();

                                            if (lReturn.Count > 0)
                                            {
                                                for (int i = 0; i < lReturn.Count; i++)
                                                {
                                                    if (lReturn[i].SONo != null && lReturn[i].SONo != "" &&
                                                        lReturn[i].OrderSource == "SAP" &&
                                                        lReturn[i].ProdType == "BPC")
                                                    {

                                                        if (lSOs == "")
                                                        {
                                                            lSOs = "'" + lReturn[i].SONo + "'";
                                                        }
                                                        else
                                                        {
                                                            lSOs = lSOs + "," + "'" + lReturn[i].SONo + "'";
                                                        }
                                                    }
                                                }
                                            }

                                            if (lSOs != null && lSOs != "")
                                            {
                                                lSQL = "SELECT fld_bpc_so_no " +
                                                "FROM dbo.tbl_scm_sts_bpc_print_tag " +
                                                "WHERE fld_bpc_so_no IN (" + lSOs + ") " +
                                                "GROUP BY fld_bpc_so_no ";

                                                lCmd.CommandText = lSQL;
                                                lCmd.Connection = lSTSCon;
                                                lCmd.CommandTimeout = 300;
                                                lRst = lCmd.ExecuteReader();
                                                if (lRst.HasRows)
                                                {
                                                    while (lRst.Read())
                                                    {
                                                        if (lRst.GetString(0).Trim() != "")
                                                        {
                                                            lTagSO.Add(lRst.GetString(0).Trim());
                                                        }

                                                    }
                                                }
                                                lRst.Close();

                                                for (int i = 0; i < lTagSO.Count; i++)
                                                {
                                                    for (int j = 0; j < lReturn.Count; j++)
                                                    {
                                                        if (lTagSO[i] == lReturn[j].SONo)
                                                        {
                                                            lReturn.Insert(j + 1, new
                                                            {
                                                                SSNNo = lReturn[j].SSNNo,
                                                                OrderNo = lReturn[j].OrderNo,
                                                                WBS1 = lReturn[j].WBS1,
                                                                WBS2 = lReturn[j].WBS2,
                                                                WBS3 = lReturn[j].WBS3,
                                                                StructureElement = lReturn[j].StructureElement,
                                                                ProdType = lReturn[j].ProdType,
                                                                PONo = lReturn[j].PONo,
                                                                PODate = lReturn[j].PODate,
                                                                RequiredDate = lReturn[j].RequiredDate,
                                                                OrderWeight = lReturn[j].OrderWeight,
                                                                SubmittedBy = lReturn[j].SubmittedBy,
                                                                OrderStatus = "Production",
                                                                OrderSource = lReturn[j].OrderSource,
                                                                SONo = lReturn[j].SONo,
                                                                SORNo = lReturn[j].SORNo,
                                                                BBSNo = lReturn[j].BBSNo,
                                                                BBSDesc = lReturn[j].BBSDesc,
                                                                CustomerCode = lReturn[j].CustomerCode,
                                                                ProjectCode = lReturn[j].ProjectCode,
                                                                DataEnteredBy = lReturn[j].DataEnteredBy,
                                                                Confirmed = lReturn[j].Confirmed,
                                                                PlanDeliveryDate = lReturn[j].PlanDeliveryDate,
                                                                ProjectTitle = lReturn[j].ProjectTitle,
                                                                Address = lReturn[j].Address,
                                                                Gate = lReturn[j].Gate
                                                            });
                                                            lReturn.RemoveAt(j);
                                                            break;
                                                        }

                                                    }
                                                }
                                            }
                                            #endregion

                                            #region CARPET
                                            lSOs = "";
                                            lTagSO = new List<string>();

                                            if (lReturn.Count > 0)
                                            {
                                                for (int i = 0; i < lReturn.Count; i++)
                                                {
                                                    if (lReturn[i].SONo != null && lReturn[i].SONo != "" &&
                                                        lReturn[i].OrderSource == "SAP" &&
                                                        lReturn[i].ProdType == "CARPET")
                                                    {
                                                        if (lSOs == "")
                                                        {
                                                            lSOs = "'" + lReturn[i].SONo + "'";
                                                        }
                                                        else
                                                        {
                                                            lSOs = lSOs + "," + "'" + lReturn[i].SONo + "'";
                                                        }
                                                    }
                                                }
                                            }

                                            if (lSOs != null && lSOs != "")
                                            {
                                                lSQL = "SELECT SO_NO " +
                                                "FROM dbo.tbl_scm_sts_carpet_print_tag " +
                                                "WHERE SO_NO IN (" + lSOs + ") " +
                                                "GROUP BY SO_NO ";

                                                lCmd.CommandText = lSQL;
                                                lCmd.Connection = lSTSCon;
                                                lCmd.CommandTimeout = 300;
                                                lRst = lCmd.ExecuteReader();
                                                if (lRst.HasRows)
                                                {
                                                    while (lRst.Read())
                                                    {
                                                        if (lRst.GetString(0).Trim() != "")
                                                        {
                                                            lTagSO.Add(lRst.GetString(0).Trim());
                                                        }

                                                    }
                                                }
                                                lRst.Close();

                                                for (int i = 0; i < lTagSO.Count; i++)
                                                {
                                                    for (int j = 0; j < lReturn.Count; j++)
                                                    {
                                                        if (lTagSO[i] == lReturn[j].SONo)
                                                        {
                                                            lReturn.Insert(j + 1, new
                                                            {
                                                                SSNNo = lReturn[j].SSNNo,
                                                                OrderNo = lReturn[j].OrderNo,
                                                                WBS1 = lReturn[j].WBS1,
                                                                WBS2 = lReturn[j].WBS2,
                                                                WBS3 = lReturn[j].WBS3,
                                                                StructureElement = lReturn[j].StructureElement,
                                                                ProdType = lReturn[j].ProdType,
                                                                PONo = lReturn[j].PONo,
                                                                PODate = lReturn[j].PODate,
                                                                RequiredDate = lReturn[j].RequiredDate,
                                                                OrderWeight = lReturn[j].OrderWeight,
                                                                SubmittedBy = lReturn[j].SubmittedBy,
                                                                OrderStatus = "Production",
                                                                OrderSource = lReturn[j].OrderSource,
                                                                SONo = lReturn[j].SONo,
                                                                SORNo = lReturn[j].SORNo,
                                                                BBSNo = lReturn[j].BBSNo,
                                                                BBSDesc = lReturn[j].BBSDesc,
                                                                CustomerCode = lReturn[j].CustomerCode,
                                                                ProjectCode = lReturn[j].ProjectCode,
                                                                DataEnteredBy = lReturn[j].DataEnteredBy,
                                                                Confirmed = lReturn[j].Confirmed,
                                                                PlanDeliveryDate = lReturn[j].PlanDeliveryDate,
                                                                ProjectTitle = lReturn[j].ProjectTitle,
                                                                Address = lReturn[j].Address,
                                                                Gate = lReturn[j].Gate
                                                            });
                                                            lReturn.RemoveAt(j);
                                                            break;
                                                        }

                                                    }
                                                }
                                            }
                                            #endregion

                                            lProcessObj.CloseSTSConnection(ref lSTSCon);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        lProcessObj.SaveErrorMsg(ex.Message, ex.StackTrace);
                                        string lerrorMsg = ex.Message;
                                    }


                                    lProcessObj.CloseNDSConnection(ref lNDSCon);

                                    #region Check Plan Delivery Date From IDB
                                    if (lReturn.Count > 0)
                                    {
                                        var lSONos = "";
                                        for (int i = 0; i < lReturn.Count; i++)
                                        {
                                            var lSOARR = lReturn[i].SORNo.Split(',');
                                            if (lSOARR.Length > 0 && (lReturn[i].PlanDeliveryDate == null || lReturn[i].PlanDeliveryDate == ""))
                                                for (int j = 0; j < lSOARR.Length; j++)
                                                {
                                                    if (lSOARR[j].Trim().Length > 0)
                                                    {
                                                        if (lSONos == "")
                                                        {
                                                            lSONos = "'" + lSOARR[j].Trim() + "'";
                                                        }
                                                        else
                                                        {
                                                            lSONos = lSONos + ",'" + lSOARR[j].Trim() + "'";
                                                        }
                                                    }
                                                }
                                        }
                                        if (lSONos != "")
                                        {
                                            try
                                            {
                                                lProcessObj.OpenIDBConnection(ref lCISCon);

                                                var lSORList = new List<string>();

                                                var lSORA = lSONos.Split(',').ToList();
                                                var lSOR1 = "";
                                                if (lSORA.Count > 0)
                                                {
                                                    int lCount = 0;
                                                    for (int i = 0; i < lSORA.Count; i++)
                                                    {
                                                        if (lSOR1 == "")
                                                        {
                                                            //lSOR1 = "'" + lSORA[i] + "'";
                                                            lSOR1 = lSORA[i];
                                                        }
                                                        else
                                                        {
                                                            //lSOR1 = lSOR1 + "," + "'" + lSORA[i] + "'";
                                                            lSOR1 = lSOR1 + "," + lSORA[i];
                                                        }
                                                        lCount = lCount + 1;
                                                        if (lCount > 300)
                                                        {
                                                            lSORList.Add(lSOR1);
                                                            lSOR1 = "";
                                                            lCount = 0;
                                                        }
                                                    }
                                                    if (lSOR1 != "")
                                                    {
                                                        lSORList.Add(lSOR1);
                                                    }
                                                }

                                                for (int k = 0; k < lSORList.Count; k++)
                                                {
                                                    if (lSORList[k] != null && lSORList[k] != "" && lSORList[k] != " " && lSORList[k] != "''" && lSORList[k] != "' '")
                                                    {
                                                        // Plan Delivery date

                                                        var lDelSOR = "";
                                                        if (lReturn.Count > 0)
                                                        {
                                                            for (int j = 0; j < lReturn.Count; j++)
                                                            {
                                                                if (lReturn[j].PlanDeliveryDate != null && lReturn[j].PlanDeliveryDate.Trim() != "")
                                                                {
                                                                    var lSOAR = lReturn[j].SORNo.Split(',');
                                                                    if (lSOAR.Length > 0)
                                                                    {
                                                                        for (int m = 0; m < lSOAR.Length; m++)
                                                                        {
                                                                            lDelSOR = lDelSOR + ",'" + lSOAR[m] + "'";
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        if (lDelSOR.Length > 0)
                                                        {
                                                            lDelSOR = lDelSOR.Substring(1);
                                                        }

                                                        if (lDelSOR.Length > 0)
                                                        {
                                                            lSQL = "SELECT A.ORDER_REQUEST_NO, Max(A.CONFIRMED_DATE), H.BBS " +      //PLANNED_DATE
                                                            "FROM SALES_ORDER_PLANNING A, ORDER_HEADER H " +
                                                            "WHERE A.ORDER_REQUEST_NO = H.ORDER_REQUEST_NO " +
                                                            "AND A.ORDER_REQUEST_NO IN (" + lSORList[k] + ") " +
                                                            "AND not exists (select ORDER_REQUEST_NO " +
                                                            "FROM SALES_ORDER_PLANNING " +
                                                            "WHERE ORDER_REQUEST_NO = A.ORDER_REQUEST_NO " +
                                                            "AND ORDER_REQUEST_NO IN (" + lDelSOR + ") ) " +
                                                            "GROUP BY A.ORDER_REQUEST_NO, H.BBS ";
                                                        }
                                                        else
                                                        {
                                                            lSQL = "SELECT A.ORDER_REQUEST_NO, Max(A.CONFIRMED_DATE), H.BBS " +      //PLANNED_DATE
                                                            "FROM SALES_ORDER_PLANNING A, ORDER_HEADER H " +
                                                            "WHERE A.ORDER_REQUEST_NO = H.ORDER_REQUEST_NO " +
                                                            "AND A.ORDER_REQUEST_NO IN (" + lSORList[k] + ") " +
                                                            "GROUP BY A.ORDER_REQUEST_NO, H.BBS ";
                                                        }

                                                        lIDBCmd.CommandText = lSQL;
                                                        lIDBCmd.Connection = lIDBCon;
                                                        lIDBCmd.CommandTimeout = 300;
                                                        lOraRst = (OracleDataReader)await lIDBCmd.ExecuteReaderAsync();
                                                        if (lOraRst.HasRows)
                                                        {
                                                            while (lOraRst.Read())
                                                            {
                                                                var lOrderNo = lOraRst.GetValue(0) == DBNull.Value ? "" : lOraRst.GetString(0).Trim();
                                                                var lPlanDate = (DateTime?)lOraRst.GetValue(1);
                                                                var lBBSNo = lOraRst.GetValue(2) == DBNull.Value ? "" : lOraRst.GetString(2).Trim();
                                                                if (lPlanDate != null)
                                                                {
                                                                    if ((DateTime)lPlanDate > DateTime.Now.AddYears(1))
                                                                    {
                                                                        lPlanDate = null;
                                                                    }
                                                                }
                                                                if (lOrderNo != "")
                                                                {
                                                                    var lConfirmed = 1;

                                                                    if (lPlanDate != null)
                                                                    {
                                                                        if (String.Format("{0:yyyy-MM-dd}", lPlanDate)
                                                                            == String.Format("{0:yyyy-MM-dd}", DateTime.Now))
                                                                        {
                                                                            lConfirmed = 1;
                                                                        }
                                                                        else if (String.Format("{0:yyyy-MM-dd}", lPlanDate)
                                                                            == (String.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(1))))
                                                                        {
                                                                            lConfirmed = 2;
                                                                        }
                                                                        else
                                                                        {
                                                                            lConfirmed = 3;
                                                                        }
                                                                    }

                                                                    var lPlanDateStr = lPlanDate == null ? "" : ((DateTime)lPlanDate).ToString("yyyy-MM-dd");
                                                                    for (int j = 0; j < lReturn.Count; j++)
                                                                    {
                                                                        if (lReturn[j].SORNo.IndexOf(lOrderNo) >= 0)
                                                                        {
                                                                            //added for specify the product
                                                                            var lPlanDelDate = lPlanDateStr;
                                                                            var lDeliveryDate = lReturn[j].PlanDeliveryDate;

                                                                            if (lDeliveryDate == null)
                                                                            {
                                                                                lDeliveryDate = "";
                                                                            }

                                                                            if (lPlanDelDate != null && lPlanDelDate != "")
                                                                            {
                                                                                if (lDeliveryDate.Trim() == "")
                                                                                {
                                                                                    if (lOrderNo.Substring(0, 3) == "103")
                                                                                    {
                                                                                        if (lReturn[j].SONo.LastIndexOf(",") > 0 && (lReturn[j].SONo.Substring(0, 1) == "8" || lReturn[j].SONo.IndexOf(",8") >= 0))
                                                                                        {
                                                                                            lDeliveryDate = lPlanDelDate + "(SB)";
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            lDeliveryDate = lPlanDelDate;
                                                                                        }
                                                                                    }
                                                                                    else if (lReturn[j].SONo.LastIndexOf(",") > 0 && (lReturn[j].SONo.Substring(0, 3) == "103" || lReturn[j].SONo.IndexOf(",103") > 0))
                                                                                    {
                                                                                        if (lBBSNo.IndexOf("-COUPLER") > 0 || lBBSNo.IndexOf("-CP") > 0)
                                                                                        {
                                                                                            lDeliveryDate = lPlanDelDate + "(CP)";
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            lDeliveryDate = lPlanDelDate + "(CAB)";
                                                                                        }
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        if (lReturn[j].SONo.LastIndexOf(",") > 0 && (lReturn[j].BBSNo.IndexOf("-COUPLER") > 0 || lReturn[j].BBSNo.IndexOf("-CP") > 0))
                                                                                        {
                                                                                            if (lBBSNo.IndexOf("-COUPLER") > 0 || lBBSNo.IndexOf("-CP") > 0)
                                                                                            {
                                                                                                lDeliveryDate = lPlanDelDate + "(CP)";
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                lDeliveryDate = lPlanDelDate + "(CAB)";
                                                                                            }
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            lDeliveryDate = lPlanDelDate;
                                                                                        }
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    if (lDeliveryDate.IndexOf(lPlanDelDate) < 0)
                                                                                    {
                                                                                        if (lOrderNo.Substring(0, 3) == "103")
                                                                                        {
                                                                                            if (lReturn[j].SONo.LastIndexOf(",") > 0 && (lReturn[j].SONo.Substring(0, 1) == "8" || lReturn[j].SONo.IndexOf(",8") >= 0))
                                                                                            {
                                                                                                lDeliveryDate = lDeliveryDate + "," + lPlanDelDate + "(SB)";
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                lDeliveryDate = lDeliveryDate + "," + lPlanDelDate;
                                                                                            }
                                                                                        }
                                                                                        else if (lReturn[j].SONo.LastIndexOf(",") > 0 && (lReturn[j].SONo.Substring(0, 3) == "103" || lReturn[j].SONo.IndexOf(",103") > 0))
                                                                                        {
                                                                                            if (lBBSNo.IndexOf("-COUPLER") > 0 || lBBSNo.IndexOf("-CP") > 0)
                                                                                            {
                                                                                                lDeliveryDate = lDeliveryDate + "," + lPlanDelDate + "(CP)";
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                lDeliveryDate = lDeliveryDate + "," + lPlanDelDate + "(CAB)";
                                                                                            }
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            if (lReturn[j].SONo.LastIndexOf(",") > 0 && (lReturn[j].BBSNo.IndexOf("-COUPLER") > 0 || lReturn[j].BBSNo.IndexOf("-CP") > 0))
                                                                                            {
                                                                                                if (lBBSNo.IndexOf("-COUPLER") > 0 || lBBSNo.IndexOf("-CP") > 0)
                                                                                                {
                                                                                                    lDeliveryDate = lDeliveryDate + "," + lPlanDelDate + "(CP)";
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    lDeliveryDate = lDeliveryDate + "," + lPlanDelDate + "(CAB)";
                                                                                                }
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                lDeliveryDate = lDeliveryDate + "," + lPlanDelDate;
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        if (lDeliveryDate.IndexOf(",") < 0 && lDeliveryDate.IndexOf("(") > 0)
                                                                                        {
                                                                                            lDeliveryDate = lDeliveryDate.Substring(0, lDeliveryDate.IndexOf("("));
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                            //PlanDeliveryDate = (lReturn[j].PlanDeliveryDate == null || lReturn[j].PlanDeliveryDate.Trim() == "") ? lPlanDateStr : ((lPlanDateStr != null && lPlanDateStr.Trim() != "" && lReturn[j].PlanDeliveryDate.IndexOf(lPlanDateStr) < 0) ? (lReturn[j].PlanDeliveryDate + "," + lPlanDateStr) : lReturn[j].PlanDeliveryDate)
                                                                            //end

                                                                            lReturn[j] = new
                                                                            {
                                                                                SSNNo = lReturn[j].SSNNo,
                                                                                OrderNo = lReturn[j].OrderNo,
                                                                                WBS1 = lReturn[j].WBS1,
                                                                                WBS2 = lReturn[j].WBS2,
                                                                                WBS3 = lReturn[j].WBS3,
                                                                                StructureElement = lReturn[j].StructureElement,
                                                                                ProdType = lReturn[j].ProdType,
                                                                                PONo = lReturn[j].PONo,
                                                                                PODate = lReturn[j].PODate,
                                                                                RequiredDate = lReturn[j].RequiredDate,
                                                                                OrderWeight = lReturn[j].OrderWeight,
                                                                                SubmittedBy = lReturn[j].SubmittedBy,
                                                                                OrderStatus = lReturn[j].OrderStatus,
                                                                                OrderSource = lReturn[j].OrderSource,
                                                                                SONo = lReturn[j].SONo,
                                                                                SORNo = lReturn[j].SORNo,
                                                                                BBSNo = lReturn[j].BBSNo,
                                                                                BBSDesc = lReturn[j].BBSDesc,
                                                                                CustomerCode = lReturn[j].CustomerCode,
                                                                                ProjectCode = lReturn[j].ProjectCode,
                                                                                DataEnteredBy = lReturn[j].DataEnteredBy,
                                                                                Confirmed = lReturn[j].Confirmed == 1 ? 1 : (lConfirmed == 1 ? 1 : (lReturn[j].Confirmed == 2 ? 2 : lConfirmed)),
                                                                                PlanDeliveryDate = lDeliveryDate,
                                                                                ProjectTitle = lReturn[j].ProjectTitle,
                                                                                Address = lReturn[j].Address,
                                                                                Gate = lReturn[j].Gate
                                                                            };

                                                                            break;
                                                                        }
                                                                    }

                                                                }
                                                            }
                                                        }

                                                        lOraRst.Close();

                                                    }
                                                }

                                                lProcessObj.CloseIDBConnection(ref lCISCon);
                                            }
                                            catch (Exception ex)
                                            {

                                            }
                                        }
                                    }

                                    #endregion

                                    lProcessObj = null;

                                    #endregion
                                }
                            }

                        }
                    }
                    if (lReturn.Count > 1)
                    {
                        lReturn.RemoveAt(0);
                    }

                    lReturn = (from p in lReturn
                               orderby p.RequiredDate descending
                               select p).ToList();

                    // put the Pending Approval first


                    if (lReturn.Count > 0)
                    {
                        var lPendingA = lReturn.ToArray();

                        lReturn.CopyTo(lPendingA);

                        var lPendingAppr = lPendingA.ToList();

                        lPendingAppr.Clear();

                        for (int i = lReturn.Count - 1; i >= 0; i--)
                        {
                            if (lReturn[i].OrderStatus == "Pending Approval")
                            {
                                lPendingAppr.Add(lReturn[i]);
                                lReturn.RemoveAt(i);
                            }

                        }

                        if (lPendingAppr.Count > 0)
                        {
                            for (int i = 0; i < lPendingAppr.Count; i++)
                            {
                                lReturn.Insert(0, lPendingAppr[i]);
                            }
                        }


                    }
                    if (lReturn.Count > 0)
                    {
                        var j = 0;

                        //loop thru the resultset to print the header dynamically
                        /*  int row = 1;
                          ws.Cells[1, 1].Value = "SNo\n序号";
                          ws.Cells[lRowNo, 1].Style.WrapText = true;
                          ws.Cells[lRowNo, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                          for (int i = 0; i < lRst.FieldCount; i++)
                          {

                             ws.Cells[row, i+2].Value = lRst.GetName(i);
                             ws.Cells[row, i+2].Style.WrapText = true;
                             ws.Cells[row, i+2].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                          }*/
                        for (int i = 0; i < lReturn.Count; i++)
                        {
                            ws.Cells[j + lRowNo, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            ws.Cells[j + lRowNo, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            ws.Cells[j + lRowNo, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            ws.Cells[j + lRowNo, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            ws.Cells[j + lRowNo, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            ws.Cells[j + lRowNo, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            ws.Cells[j + lRowNo, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            ws.Cells[j + lRowNo, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            ws.Cells[j + lRowNo, 9].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            ws.Cells[j + lRowNo, 10].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            ws.Cells[j + lRowNo, 11].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            ws.Cells[j + lRowNo, 12].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            ws.Cells[j + lRowNo, 13].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            ws.Cells[j + lRowNo, 14].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            ws.Cells[j + lRowNo, 15].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                            //if (lUserID.Split('@').Length == 2 && lUserID.Split('@')[1].ToLower() == "natsteel.com.sg")
                            //{
                            ws.Cells[j + lRowNo, 16].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            ws.Cells[j + lRowNo, 17].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            ws.Cells[j + lRowNo, 18].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            ws.Cells[j + lRowNo, 19].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            //}

                            //ws.Cells[j + lRowNo, 1].Value = j + 1;
                            ws.Cells[j + lRowNo, 1].Value = lReturn[i].OrderNo; //"Order No\n加工表号";
                            ws.Cells[j + lRowNo, 2].Value = lReturn[i].WBS1; //"WBS1\n楼座";
                            ws.Cells[j + lRowNo, 3].Value = lReturn[i].WBS2; //"WBS2\n楼层";
                            ws.Cells[j + lRowNo, 4].Value = lReturn[i].WBS3; //"WBS3\n分部";
                            ws.Cells[j + lRowNo, 5].Value = lReturn[i].StructureElement; //"Structure Element\n分部

                            ws.Cells[j + lRowNo, 6].Value = lReturn[i].ProdType; //"Product Type\n订货日期";
                            ws.Cells[j + lRowNo, 7].Value = lReturn[i].PONo;
                            ws.Cells[j + lRowNo, 8].Value = lReturn[i].BBSNo;
                            ws.Cells[j + lRowNo, 9].Value = lReturn[i].BBSDesc;

                            var lValue = lReturn[i].PODate;
                            DateTime lDateTValue = DateTime.Now;
                            if (DateTime.TryParse(lValue, out lDateTValue))
                            {
                                ws.Cells[j + lRowNo, 10].Value = lDateTValue;
                                ws.Cells[j + lRowNo, 10].Style.Numberformat.Format = "yyyy-mm-dd"; ;
                            }
                            else
                            {
                                ws.Cells[j + lRowNo, 10].Value = lValue;
                            }
                            //ws.Cells[j + lRowNo, 10].Value = lReturn[i].PODate; //"PO Date\n订货日期";
                            lValue = lReturn[i].RequiredDate;
                            lDateTValue = DateTime.Now;
                            if (DateTime.TryParse(lValue, out lDateTValue))
                            {
                                ws.Cells[j + lRowNo, 11].Value = lDateTValue;
                                ws.Cells[j + lRowNo, 11].Style.Numberformat.Format = "yyyy-mm-dd"; ;
                            }
                            else
                            {
                                ws.Cells[j + lRowNo, 11].Value = lValue;
                            }
                            //ws.Cells[j + lRowNo, 11].Value = lReturn[i].RequiredDate; //"Required Date\n到场日期";
                            decimal lWT = 0;
                            decimal.TryParse(lReturn[i].OrderWeight, out lWT);

                            ws.Cells[j + lRowNo, 12].Value = lWT;//"Total weight\n订货日期";
                            ws.Cells[j + lRowNo, 12].Style.Numberformat.Format = "#,###,##0.000";
                            ws.Cells[j + lRowNo, 13].Value = lReturn[i].SubmittedBy; //"Updated by\n订货日期";
                            ws.Cells[j + lRowNo, 14].Value = lReturn[i].DataEnteredBy;
                            ws.Cells[j + lRowNo, 15].Value = lReturn[i].OrderStatus;

                            //if (lUserID.Split('@').Length == 2 && lUserID.Split('@')[1].ToLower() == "natsteel.com.sg")
                            //{
                            lValue = lReturn[i].PlanDeliveryDate;
                            lDateTValue = DateTime.Now;
                            if (DateTime.TryParse(lValue, out lDateTValue))
                            {
                                ws.Cells[j + lRowNo, 16].Value = lDateTValue;
                                ws.Cells[j + lRowNo, 16].Style.Numberformat.Format = "yyyy-mm-dd"; ;
                            }
                            else
                            {
                                ws.Cells[j + lRowNo, 16].Value = lValue;
                            }
                            ws.Cells[j + lRowNo, 17].Value = lReturn[i].ProjectTitle;
                            ws.Cells[j + lRowNo, 18].Value = lReturn[i].Address;
                            ws.Cells[j + lRowNo, 19].Value = lReturn[i].Gate;
                            //}

                            j = j + 1;

                        }
                    }
                    lCmd = null;
                    lNDSCon = null;
                    lRst = null;

                    lOraCmd = null;
                    lCISCon = null;
                    lOraRst = null;

                }

                MemoryStream ms = new MemoryStream();
                package.SaveAs(ms);

                var bExcel = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(bExcel, 0, bExcel.Length);

                //bPDF = ms.GetBuffer();
                ms.Flush();
                ms.Dispose();

                var fileName = "abc";
                using (MemoryStream memoryStream = new MemoryStream(bExcel)) // Replace bExcelData with your Excel data byte array
                {
                    HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                    response.Content = new StreamContent(memoryStream);
                    // Set content disposition with the appropriate file name and extension for Excel
                    response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                    response.Content.Headers.ContentDisposition.FileName = fileName + ".xlsx"; // Change the file extension to .xlsx for Excel
                                                                                               // Set the content type to Excel
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"); // Use the appropriate MIME type for Excel
                    return File(response.Content.ReadAsByteArrayAsync().Result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"); // Use the appropriate MIME type for Excel
                }


                //return Json(bExcel, JsonRequestBehavior.AllowGet);
            }


            catch (Exception ex)
            {

                throw ex;

            }
        }


//[HttpGet]
//[Route("/getActiveOrders_Upcoming/{CustomerCode}/{ProjectCode}/{AllProjects}")]
//public async Task<ActionResult> getActiveOrders_Upcoming(string CustomerCode, string ProjectCode, bool AllProjects)
//{
//    //UpdateStatus(CustomerCode, ProjectCode, User.Identity.GetUserName());

//    string lSQL = "";
//    //set kookie for customer and project
//    string lPODateFrom = "";
//    string lPODateTo = "";
//    string lRDateFrom = "";
//    string lRDateTo = "";

//    string lPODateFrom_O = "";
//    string lPODateTo_O = "";
//    string lRDateFrom_O = "";
//    string lRDateTo_O = "";


//    //  (PODate == null) PODate = "";
//    //if (PODate.Trim().Length == 0 || PODate.IndexOf("to") <= 0)
//    //{
//    //    lPODateFrom = "2000-01-01 00:00:00";
//    //    lPODateTo = "2200-01-01 00:00:00";
//    //}
//    //else
//    //{
//    //    lPODateFrom = PODate.Substring(0, PODate.IndexOf("to")).Trim();
//    //    lPODateTo = PODate.Substring(PODate.IndexOf("to") + 2).Trim();
//    //}
//    DateTime lDateV = new DateTime();
//    if (DateTime.TryParse(lPODateFrom, out lDateV) != true)
//    {
//        lPODateFrom = "2000-01-01 00:00:00";
//    }
//    if (DateTime.TryParse(lPODateTo, out lDateV) != true)
//    {
//        lPODateTo = "2200-01-01 00:00:00";
//    }

//    //if (RDate == null) RDate = "";
//    //if (RDate.Trim().Length == 0 || RDate.IndexOf("to") <= 0)
//    //{
//    //    lRDateFrom = "2000-01-01 00:00:00";
//    //    lRDateTo = "2200-01-01 00:00:00";
//    //}
//    //else
//    //{
//    //    lRDateFrom = RDate.Substring(0, RDate.IndexOf("to")).Trim();
//    //    lRDateTo = RDate.Substring(RDate.IndexOf("to") + 2).Trim();
//    //}

//    lDateV = new DateTime();
//    if (DateTime.TryParse(lRDateFrom, out lDateV) != true)
//    {
//        lRDateFrom = "2000-01-01 00:00:00";
//    }
//    if (DateTime.TryParse(lRDateTo, out lDateV) != true)
//    {
//        lRDateTo = "2200-01-01 00:00:00";
//    }

//    var lCmd = new SqlCommand();
//    SqlDataReader lRst;
//    var lNDSCon = new SqlConnection();


//    var lCISCon = new OracleConnection();
//    var lOraCmd = new OracleCommand();
//    OracleDataReader lOraRst;

//    var lReturn = (new[]{ new
//    {
//        SSNNo = 0,
//        OrderNo = "0",
//        WBS1 = "",
//        WBS2 = "",
//        WBS3 = "",
//        StructureElement = "",
//        ProdType = "",
//        PONo = "",
//        PODate = "",
//        RequiredDate = "",
//        OrderWeight = "",
//        SubmittedBy = "",
//        OrderStatus = "",
//        OrderSource = "",
//        SONo = "",
//        SORNo = "",
//        BBSNo = "",
//        BBSDesc = "",
//        CustomerCode = "",
//        ProjectCode = "",
//        ProjectTitle = "",
//        DataEnteredBy = "",
//        Confirmed = 0,
//        PlanDeliveryDate = "",
//        SubmitBy=""
//    }}).ToList();

//    if (lReturn.Count > 0)
//    {
//        lReturn.RemoveAt(0);
//    }

//    if (CustomerCode != null && ProjectCode != null)
//    {
//        if (CustomerCode.Length > 0 && ProjectCode.Length > 0)
//        {
//            var lProjectState = "";
//            if (AllProjects == true)
//            {
//                UserAccessController lUa = new UserAccessController();
//                var lUserType = lUa.getUserType(User.Identity.GetUserName());
//                var lGroupName = lUa.getGroupName(User.Identity.GetUserName());

//                lUa = null;

//                SharedAPIController lBackEnd = new SharedAPIController();

//                var lProjects = lBackEnd.getProject(CustomerCode, lUserType, lGroupName);
//                lBackEnd = null;

//                if (lProjects != null && lProjects.Count > 0)
//                {
//                    for (int i = 0; i < lProjects.Count; i++)
//                    {
//                        if (lProjects[i] != null && lProjects[i].ProjectCode != null && lProjects[i].ProjectCode.Trim().Length > 0)
//                        {
//                            /// int lRtn = await UpdateStatus(CustomerCode, lProjects[i].ProjectCode);

//                            if (lProjectState == "")
//                            {
//                                lProjectState = "'" + lProjects[i].ProjectCode + "' ";
//                            }
//                            else
//                            {
//                                lProjectState = lProjectState + ",'" + lProjects[i].ProjectCode + "' ";
//                            }
//                        }
//                    }
//                }

//                if (lProjectState == "")
//                {
//                    lProjectState = "'' ";
//                }
//            }
//            else
//            {
//                lProjectState = "'" + ProjectCode + "' ";
//            }

//            #region Retrieve Data
//            lSQL =
//            "SELECT " +
//            "M.OrderNumber, " +
//            "M.WBS1, " +
//            "M.WBS2, " +
//            "M.WBS3, " +
//            "S.StructureElement, " +
//            "S.ProductType, " +
//            "S.PONumber, " +

//            "isNull(convert(varchar(10), S.PODate, 120),'') AS PODate, " + //7
//            "isNull(convert(varchar(10), S.RequiredDate, 120),'') AS RequiredDate, " + //8
//            "M.TotalWeight, " +             //9
//            "case when M.OrderStatus = 'Sent' then M.SubmitBy else M.UpdateBy end as UpdateBy, " +
//            "M.OrderStatus, " +
//            "(STUFF ( " +
//            " (" +
//            " SELECT ',' + SAPSONo FROM " +

//            " (SELECT SAPSONo as SAPSONo " +
//            " FROM dbo.OESStdSheetJobAdvice " +
//            " WHERE CustomerCode = M.CustomerCode AND " +
//            " ProjectCode = M.ProjectCode AND " +
//            "(JobID = S.StdMESHJobID " +
//            "OR JobID = S.StdBarsJobID " +
//            "OR JobID = S.CoilProdJobID) " +
//            " GROUP BY SAPSONo " +

//            " Union " +
//            " select BBSSOR as SAPSONo " +
//            " FROM  dbo.OESBBS " +
//            " WHERE CustomerCode = M.CustomerCode AND " +
//            " ProjectCode = M.ProjectCode AND " +
//            " JobID = S.CABJobID " +
//            " GROUP BY BBSSOR " +

//            " Union " +
//            " select BBSSORCoupler as SAPSONo " +
//            " FROM  dbo.OESBBS " +
//            " WHERE CustomerCode = M.CustomerCode AND " +
//            " ProjectCode = M.ProjectCode AND " +
//            " JobID = S.CABJobID " +
//            " GROUP BY BBSSORCoupler " +

//            " Union " +
//            " select BBSSAPSO as SAPSONo " +
//            " FROM  dbo.OESBBS " +
//            " WHERE CustomerCode = M.CustomerCode AND " +
//            " ProjectCode = M.ProjectCode AND " +
//            " JobID = S.CABJobID " +
//            " GROUP BY BBSSAPSO " +

//            " Union " +
//            " select sor_no as SAPSONo " +
//            " FROM  dbo.OESBPCDetailsProc " +
//            " WHERE CustomerCode = M.CustomerCode AND " +
//            " ProjectCode = M.ProjectCode AND " +
//            " JobID = S.BPCJobID " +
//            " GROUP BY sor_no " +

//            " ) as k " +
//            " FOR XML PATH('')), 1, 1, '')) " +
//            " AS SOR, " +
//            "(STUFF( " +
//            "(SELECT ',' + isNull(BBSNo, '') " +
//            "FROM  dbo.OESBBS " +
//            "WHERE CustomerCode =  M.CustomerCode " +
//            "AND ProjectCode = M.ProjectCode " +
//            "AND JobID = S.CABJobID " +
//            "GROUP BY BBSNo FOR XML PATH('')), 1, 1, '')) as BBSNo, " +
//            "(STUFF( " +
//            "(SELECT ',' + isNull(BBSDesc, '') " +
//            "FROM  dbo.OESBBS " +
//            "WHERE CustomerCode =  M.CustomerCode " +
//            "AND ProjectCode = M.ProjectCode " +
//            "AND JobID = S.CABJobID " +
//            "GROUP BY BBSDesc FOR XML PATH('')), 1, 1, '')) as BBSDesc, " +
//            "S.SAPSOR as UXSOR, " +
//            "M.CustomerCode, " +
//            "M.ProjectCode, " +
//            "M.SubmitBy, " +
//            "(SELECT ProjectTitle FROM dbo.OESProject " +
//            "WHERE CustomerCode = M.CustomerCode " +
//            "AND ProjectCode = M.ProjectCode) as ProjectTitle, " +
//            "CASE WHEN S.ProductType = 'CAB' THEN " +
//            "isNull(STUFF((SELECT  ',' + UpdateBy " +
//            "FROM dbo.OESOrderDetails " +
//            "WHERE CustomerCode = M.CustomerCode " +
//            "AND ProjectCode = M.ProjectCode " +
//            "AND JobID = S.CABJobID " +
//            "GROUP BY UpdateBy " +
//            "ORDER BY UpdateBy " +
//            "FOR XML PATH('')), 1, 1, ''),'') " +
//            "WHEN S.ProductType = 'CUT-TO-SIZE-MESH' THEN " +
//            "isNull(STUFF((SELECT  ',' + UpdateBy " +
//            "FROM dbo.OESCTSMESHOthersDetails " +
//            "WHERE CustomerCode = M.CustomerCode " +
//            "AND ProjectCode = M.ProjectCode " +
//            "AND JobID = S.MESHJobID " +
//            "GROUP BY UpdateBy " +
//            "ORDER BY UpdateBy " +
//            "FOR XML PATH('')), 1, 1, ''),'') " +
//            "WHEN S.ProductType = 'STANDARD-MESH' THEN " +
//            "isNull(STUFF((SELECT  ',' + UpdateBy " +
//            "FROM dbo.OESStdSheetDetails " +
//            "WHERE CustomerCode = M.CustomerCode " +
//            "AND ProjectCode = M.ProjectCode " +
//            "AND JobID = S.StdMESHJobID " +
//            "GROUP BY UpdateBy " +
//            "ORDER BY UpdateBy " +
//            "FOR XML PATH('')), 1, 1, ''),'') " +
//            "WHEN S.ProductType = 'STANDARD-BAR' THEN " +
//            "isNull(STUFF((SELECT  ',' + UpdateBy " +
//            "FROM dbo.OESStdProdDetails " +
//            "WHERE CustomerCode = M.CustomerCode " +
//            "AND ProjectCode = M.ProjectCode " +
//            "AND JobID = S.StdBarsJobID " +
//            "GROUP BY UpdateBy " +
//            "ORDER BY UpdateBy " +
//            "FOR XML PATH('')), 1, 1, ''),'') " +
//            "WHEN S.ProductType = 'BPC' THEN " +
//            "isNull(STUFF((SELECT  ',' + UpdateBy " +
//            "FROM dbo.OESBPCDetails " +
//            "WHERE CustomerCode = M.CustomerCode " +
//            "AND ProjectCode = M.ProjectCode " +
//            "AND Template = 0 " +
//            "AND JobID = S.BPCJobID " +
//            "GROUP BY UpdateBy " +
//            "ORDER BY UpdateBy " +
//            "FOR XML PATH('')), 1, 1, ''),'') " +
//            "WHEN S.ProductType = 'COIL' OR S.ProductType = 'COUPLER' THEN " +
//            "isNull(STUFF((SELECT  ',' + UpdateBy " +
//            "FROM dbo.OESStdProdDetails " +
//            "WHERE CustomerCode = M.CustomerCode " +
//            "AND ProjectCode = M.ProjectCode " +
//            "AND JobID = S.CoilProdJobID " +
//            "GROUP BY UpdateBy " +
//            "ORDER BY UpdateBy " +
//            "FOR XML PATH('')), 1, 1, ''),'') " +
//            "ELSE '' END as DataEnteredBy " +
//            "FROM dbo.OESProjOrder M, dbo.OESProjOrdersSE S " +
//            "WHERE M.OrderNumber = S.OrderNumber " +
//            "AND M.CustomerCode = '" + CustomerCode + "' " +
//            "AND M.ProjectCode IN (" + lProjectState + ") " +
//            "AND M.OrderStatus is not NULL " +
//            "AND M.OrderStatus <> 'New' " +
//            "AND M.OrderStatus <> 'Created' " +
//            "AND M.OrderStatus <> 'Cancelled' " +
//            "AND M.OrderStatus <> 'Deleted' " +
//            "AND M.OrderStatus <> 'Delivered' " +
//            //"AND ((S.PODate >= '" + lPODateFrom + "' " +
//            //"AND DATEADD(d,-1,S.PODate) < '" + lPODateTo + "') " +
//            //"OR (S.PODate is null AND '" + lPODateTo + "' = '2200-01-01 00:00:00' )) " +
//            //"AND ((S.RequiredDate >= '" + lRDateFrom + "' " +
//            //"AND DATEADD(d,-1,S.RequiredDate) < '" + lRDateTo + "') " +
//            //"OR (S.RequiredDate is null AND '" + lRDateTo + "' = '2200-01-01 00:00:00' )) " +
//            //"AND (S.PONumber like '%" + PONumber + "%' " +
//            //"OR  (S.PONumber is null AND '" + PONumber + "' = '' )) " +
//            //"AND (M.WBS1 like '%" + WBS1 + "%' " +
//            //"OR (M.WBS1 is null AND '" + WBS1 + "' = '' )) " +
//            //"AND (M.WBS2 like '%" + WBS2 + "%' " +
//            //"OR (M.WBS2 is null AND '" + WBS2 + "' = '' )) " +
//            //"AND (M.WBS3 like '%" + WBS3 + "%' " +
//            //"OR (M.WBS3 is null AND '" + WBS3 + "' = '' )) " +
//            ////"AND M.UpdateDate >= '2019-07-01' " +
//            "GROUP BY " +
//            "M.CustomerCode, " +
//            "M.ProjectCode, " +
//            "S.CABJobID, " +
//            "S.MESHJobID, " +
//            "S.StdMESHJobID, " +
//            "S.StdBarsJobID, " +
//            "S.CoilProdJobID, " +
//            "S.BPCJobID, " +
//            "S.StructureElement, " +
//            "S.ProductType, " +
//            "S.ScheduledProd, " +
//            "S.PONumber, " +

//            "S.PODate, " +
//            "S.RequiredDate, " +
//            "M.OrderNumber, " +
//            "M.WBS1, " +
//            "M.WBS2, " +
//            "M.WBS3, " +
//            "M.TotalWeight, " +
//            "M.UpdateBy, " +
//            "M.OrderStatus, " +
//            "S.SAPSOR, " +
//            "M.SubmitBy " +
//            "ORDER BY " +
//            "M.OrderNumber DESC ";

//            var lProcessObj = new ProcessController();
//            if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
//            {
//                lCmd.CommandText = lSQL;
//                lCmd.Connection = lNDSCon;
//                lCmd.CommandTimeout = 300;
//                lRst = await lCmd.ExecuteReaderAsync();
//                if (lRst.HasRows)
//                {
//                    var lSNo = 0;
//                    while (lRst.Read())
//                    {
//                        lSNo = lSNo + 1;
//                        var lOrderStatus = (lRst.GetValue(11) == DBNull.Value ? "" : lRst.GetString(11).Trim());
//                        if (lOrderStatus == "Sent")
//                        {
//                            lOrderStatus = "Pending Approval";
//                        }
//                        if (lOrderStatus == "Submitted")
//                        {
//                            lOrderStatus = "Submitted to NSH";
//                        }
//                        var lUXSOR = lRst.GetValue(15) == DBNull.Value ? "" : lRst.GetString(15).Trim();
//                        var lMySOR = lRst.GetValue(12) == DBNull.Value ? "" : lRst.GetString(12).Trim();
//                        if (lUXSOR != "" && lUXSOR != lMySOR)
//                        {
//                            if (lMySOR == "")
//                            {
//                                lMySOR = lUXSOR;
//                            }
//                            else
//                            {
//                                lMySOR = lMySOR = "," + lUXSOR;
//                            }

//                        }
//                        lReturn.Add(new
//                        {
//                            SSNNo = lSNo,
//                            OrderNo = lRst.GetInt32(0).ToString(),
//                            WBS1 = (lRst.GetValue(1) == DBNull.Value ? "" : lRst.GetString(1).Trim()),
//                            WBS2 = (lRst.GetValue(2) == DBNull.Value ? "" : lRst.GetString(2).Trim()),
//                            WBS3 = (lRst.GetValue(3) == DBNull.Value ? "" : lRst.GetString(3).Trim()),
//                            StructureElement = lRst.GetValue(4) == DBNull.Value ? "" : (lRst.GetString(4).Trim() == "NONWBS" ? "" : lRst.GetString(4).Trim()),
//                            ProdType = (lRst.GetValue(5) == DBNull.Value ? "" : lRst.GetString(5).Trim()),
//                            PONo = (lRst.GetValue(6) == DBNull.Value ? "" : (lRst.GetString(6).Trim().Length > 0 && lRst.GetString(6).Trim().Substring(0, 1) == "," ? lRst.GetString(6).Trim().Substring(1) : lRst.GetString(6).Trim())),
//                            PODate = (lRst.GetValue(7) == DBNull.Value ? "" : lRst.GetString(7)),
//                            RequiredDate = lRst.GetValue(8) == DBNull.Value ? "" : (lRst.GetString(8).Trim().Length > 0 && lRst.GetString(8).Substring(0, 1) == "," ? lRst.GetString(8).Substring(1) : lRst.GetString(8)),
//                            OrderWeight = (lRst.GetValue(9) == DBNull.Value ? "0.000" : (lRst.GetDecimal(9) / 1000).ToString("###,###,##0.000;(###,##0.000); ")),
//                            SubmittedBy = (lRst.GetValue(10) == DBNull.Value ? "" : lRst.GetString(10).Trim()),
//                            OrderStatus = lOrderStatus,
//                            OrderSource = "DIGIOS",
//                            SONo = lMySOR,
//                            SORNo = lMySOR,
//                            BBSNo = (lRst.GetValue(13) == DBNull.Value ? "" : lRst.GetString(13).Trim()),
//                            BBSDesc = WebUtility.HtmlDecode(lRst.GetValue(14) == DBNull.Value ? "" : lRst.GetString(14).Trim()),
//                            CustomerCode = (lRst.GetValue(16) == DBNull.Value ? "" : lRst.GetString(16).Trim()),
//                            ProjectCode = (lRst.GetValue(17) == DBNull.Value ? "" : lRst.GetString(17).Trim()),
//                            ProjectTitle = (lRst.GetValue(19) == DBNull.Value ? "" : lRst.GetString(19).Trim()),
//                            DataEnteredBy = (lRst.GetValue(20) == DBNull.Value ? "" : lRst.GetString(20).Trim()),
//                            Confirmed = 0,
//                            PlanDeliveryDate = "",
//                            SubmitBy = (lRst.GetValue(18) == DBNull.Value ? "" : lRst.GetString(18).Trim())
//                        });
//                    }
//                }
//                lRst.Close();

//                lProcessObj.CloseNDSConnection(ref lNDSCon);
//            }

//            string lSOR = "";
//            if (lReturn.Count > 0)
//            {
//                for (int i = 0; i < lReturn.Count; i++)
//                {
//                    if (lReturn[i].SONo != null && lReturn[i].SONo != "")
//                    {
//                        if (lSOR == "")
//                        {
//                            lSOR = lReturn[i].SONo;
//                        }
//                        else
//                        {
//                            lSOR = lSOR + "," + lReturn[i].SONo;
//                        }
//                    }
//                }
//            }

//            if (lSOR != "")
//            {
//                var lSORA = lSOR.Split(',').ToList();
//                lSOR = "";
//                if (lSORA.Count > 0)
//                {
//                    for (int i = 0; i < lSORA.Count; i++)
//                    {
//                        if (lSOR == "")
//                        {
//                            lSOR = "'" + lSORA[i] + "'";
//                        }
//                        else
//                        {
//                            lSOR = lSOR + "," + "'" + lSORA[i] + "'";
//                        }
//                    }
//                }
//            }

//            if (lSOR == "")
//            {
//                lSOR = "' '";
//            }

//            //take out duplicated record
//            if (lReturn.Count > 0)
//            {
//                for (int i = lReturn.Count - 1; i > 0; i--)
//                {
//                    if (lReturn[i].OrderNo == lReturn[i - 1].OrderNo)
//                    {
//                        lReturn[i - 1] = new
//                        {
//                            SSNNo = lReturn[i - 1].SSNNo,
//                            OrderNo = lReturn[i - 1].OrderNo,
//                            WBS1 = lReturn[i - 1].WBS1,
//                            WBS2 = lReturn[i - 1].WBS2,
//                            WBS3 = lReturn[i - 1].WBS3,
//                            StructureElement = (lReturn[i - 1].StructureElement == null || lReturn[i - 1].StructureElement.Trim() == "") ? lReturn[i].StructureElement : ((lReturn[i].StructureElement != null && lReturn[i].StructureElement.Trim() != "" && lReturn[i].StructureElement != lReturn[i - 1].StructureElement) ? (lReturn[i - 1].StructureElement + "," + lReturn[i].StructureElement) : lReturn[i].StructureElement),
//                            ProdType = (lReturn[i - 1].ProdType == null || lReturn[i - 1].ProdType.Trim() == "") ? lReturn[i].ProdType : ((lReturn[i].ProdType != null && lReturn[i].ProdType.Trim() != "" && lReturn[i].ProdType != lReturn[i - 1].ProdType) ? (lReturn[i - 1].ProdType + "," + lReturn[i].ProdType) : lReturn[i].ProdType),
//                            PONo = (lReturn[i - 1].PONo == null || lReturn[i - 1].PONo.Trim() == "") ? lReturn[i].PONo : ((lReturn[i].PONo != null && lReturn[i].PONo.Trim() != "" && lReturn[i].PONo != lReturn[i - 1].PONo) ? (lReturn[i - 1].PONo + "," + lReturn[i].PONo) : lReturn[i].PONo),
//                            PODate = (lReturn[i - 1].PODate == null || lReturn[i - 1].PODate.Trim() == "") ? lReturn[i].PODate : ((lReturn[i].PODate != null && lReturn[i].PODate.Trim() != "" && lReturn[i].PODate != lReturn[i - 1].PODate) ? (lReturn[i - 1].PODate + "," + lReturn[i].PODate) : lReturn[i].PODate),
//                            RequiredDate = (lReturn[i - 1].RequiredDate == null || lReturn[i - 1].RequiredDate.Trim() == "") ? lReturn[i].RequiredDate : ((lReturn[i].RequiredDate != null && lReturn[i].RequiredDate.Trim() != "" && lReturn[i].RequiredDate != lReturn[i - 1].RequiredDate) ? (lReturn[i - 1].RequiredDate + "," + lReturn[i].RequiredDate) : lReturn[i].RequiredDate),
//                            OrderWeight = lReturn[i - 1].OrderWeight,
//                            SubmittedBy = lReturn[i - 1].SubmittedBy,
//                            OrderStatus = lReturn[i - 1].OrderStatus,
//                            OrderSource = lReturn[i - 1].OrderSource,
//                            SONo = (lReturn[i - 1].SONo == null || lReturn[i - 1].SONo.Trim() == "") ? lReturn[i].SONo : ((lReturn[i].SONo != null && lReturn[i].SONo.Trim() != "" && lReturn[i].SONo != lReturn[i - 1].SONo) ? (lReturn[i - 1].SONo + "," + lReturn[i].SONo) : lReturn[i].SONo),
//                            SORNo = (lReturn[i - 1].SORNo == null || lReturn[i - 1].SORNo.Trim() == "") ? lReturn[i].SORNo : ((lReturn[i].SORNo != null && lReturn[i].SORNo.Trim() != "" && lReturn[i].SORNo != lReturn[i - 1].SORNo) ? (lReturn[i - 1].SORNo + "," + lReturn[i].SORNo) : lReturn[i].SORNo),
//                            BBSNo = (lReturn[i - 1].BBSNo == null || lReturn[i - 1].BBSNo.Trim() == "") ? lReturn[i].BBSNo : ((lReturn[i].BBSNo != null && lReturn[i].BBSNo.Trim() != "" && lReturn[i].BBSNo != lReturn[i - 1].BBSNo) ? (lReturn[i - 1].BBSNo + "," + lReturn[i].BBSNo) : lReturn[i].BBSNo),
//                            BBSDesc = (lReturn[i - 1].BBSDesc == null || lReturn[i - 1].BBSDesc.Trim() == "") ? lReturn[i].BBSDesc : ((lReturn[i].BBSDesc != null && lReturn[i].BBSDesc.Trim() != "" && lReturn[i].BBSDesc != lReturn[i - 1].BBSDesc) ? (lReturn[i - 1].BBSDesc + "," + lReturn[i].BBSDesc) : lReturn[i].BBSDesc),
//                            CustomerCode = lReturn[i - 1].CustomerCode,
//                            ProjectCode = lReturn[i - 1].ProjectCode,
//                            ProjectTitle = lReturn[i - 1].ProjectTitle,
//                            DataEnteredBy = lReturn[i - 1].DataEnteredBy,
//                            Confirmed = lReturn[i - 1].Confirmed,
//                            PlanDeliveryDate = lReturn[i - 1].PlanDeliveryDate,
//                            SubmitBy = lReturn[i - 1].SubmitBy
//                        };

//                        lReturn.RemoveAt(i);
//                    }
//                }
//            }

//            //lProcessObj.OpenCISConnection(ref lCISCon);

//            //#region Check from CIS Plan Delivery Date
//            //if (lSOR != "' '" && lSOR != "''")
//            //{
//            //    var lSORList = new List<string>();
//            //    if (lSOR != "")
//            //    {
//            //        var lSORA = lSOR.Split(',').ToList();
//            //        var lSOR1 = "";
//            //        if (lSORA.Count > 0)
//            //        {
//            //            int lCount = 0;
//            //            for (int i = 0; i < lSORA.Count; i++)
//            //            {
//            //                if (lSOR1 == "")
//            //                {
//            //                    //lSOR1 = "'" + lSORA[i] + "'";
//            //                    lSOR1 = lSORA[i];
//            //                }
//            //                else
//            //                {
//            //                    //lSOR1 = lSOR1 + "," + "'" + lSORA[i] + "'";
//            //                    lSOR1 = lSOR1 + "," + lSORA[i];
//            //                }
//            //                lCount = lCount + 1;
//            //                if (lCount > 300)
//            //                {
//            //                    lSORList.Add(lSOR1);
//            //                    lSOR1 = "";
//            //                    lCount = 0;
//            //                }
//            //            }
//            //            if (lSOR1 != "")
//            //            {
//            //                lSORList.Add(lSOR1);
//            //            }
//            //        }

//            //    }

//            //    for (int k = 0; k < lSORList.Count; k++)
//            //    {
//            //        lSQL = "SELECT " +
//            //        "NVL(M.ORDER_REQUEST_NO, ' '), " +
//            //        "M.REQ_DAT_TO, " +                                  //1. Revised Req date
//            //        "M.FIRST_PROMISED_D, " +                            //2. First Promised date
//            //                                                            //"(SELECT NVL(MAX(conf_del_date), ' ') " +           
//            //                                                            //"FROM SAPSR3.YMSDT_ORDER_ITEM " +
//            //                                                            //"WHERE MANDT = M.MANDT " +
//            //                                                            //"AND ORDER_REQUEST_NO = M.ORDER_REQUEST_NO) as Conf_del_date,  " + //3. Confirmed Del from CIS
//            //        "' ', " +
//            //        //"(SELECT MIN(PLAN_DELIV_DATE) FROM SAPSR3.YMPPT_LP_ITEM_C LC, SAPSR3.YMPPT_LP_HDR LH " +
//            //        //"WHERE LC.MANDT = LH.MANDT " +
//            //        //"AND LC.LOAD_NO = LH.LOAD_NO " +
//            //        //"AND SALES_ORDER = M.SALES_ORDER) as plan_del_date, " +         //4. Confirmed Del from Planning
//            //        "(SELECT MIN(LOAD_DATE) FROM SAPSR3.YMPPT_LP_ITEM_C LC, SAPSR3.YMPPT_LP_HDR LH " +
//            //        "WHERE LC.MANDT = LH.MANDT AND LC.LOAD_NO = LH.LOAD_NO AND LH.VEHICLE_TYPE <> 'MC' " +
//            //        "AND SALES_ORDER = M.SALES_ORDER) as plan_del_date, " +         //4. Confirmed Del from Planning
//            //        "(SELECT MIN(K.LFDAT) FROM SAPSR3.LIPS P, SAPSR3.LIKP K " +
//            //        "WHERE P.MANDT = K.MANDT " +
//            //        "AND P.VBELN = K.VBELN " +
//            //        "AND P.VGBEL = M.SALES_ORDER) as plan_del_conf, " +              //5. Confirmed Del from SAP
//            //        "D.BBS_NO " +
//            //        "FROM SAPSR3.YMSDT_ORDER_HDR M LEFT OUTER JOIN SAPSR3.YMSDT_REQ_DETAIL D " +
//            //        "ON M.order_request_no = D.order_request_no " +
//            //        "WHERE M.MANDT = '" + lProcessObj.strClient + "' " +
//            //        "AND M.PROJECT IN (" + lProjectState + ") " +
//            //        "AND M.REQD_DEL_DATE >= to_char(sysdate - 180, 'yyyymmdd') " +
//            //        "AND M.SALES_ORDER NOT IN " +
//            //        "(SELECT VBELN FROM  SAPSR3.VBUK " +
//            //        "WHERE VBELN = M.SALES_ORDER AND ABSTK = 'C') " +
//            //        "AND M.ORDER_REQUEST_NO IN " +
//            //        "(" + lSORList[k] + ") ";

//            //        lOraCmd.CommandText = lSQL;
//            //        lOraCmd.Connection = lCISCon;
//            //        lOraCmd.CommandTimeout = 300;
//            //        lOraRst = (OracleDataReader)await lOraCmd.ExecuteReaderAsync();
//            //        if (lOraRst.HasRows)
//            //        {
//            //            while (lOraRst.Read())
//            //            {
//            //                int lConfirmed = 0;
//            //                var lSORNo = (lOraRst.GetValue(0) == DBNull.Value ? "" : lOraRst.GetString(0));

//            //                //var lReqDate = (lOraRst.GetValue(2) == DBNull.Value ? "" : lOraRst.GetString(2).Trim());
//            //                //if (lReqDate == "")
//            //                //{
//            //                var lReqDate = (lOraRst.GetValue(1) == DBNull.Value ? "" : lOraRst.GetString(1).Trim());
//            //                //}
//            //                //else
//            //                //{
//            //                //    if (lReqDate.Length > 10)
//            //                //    {
//            //                //        lReqDate = lReqDate.Substring(0, 10);
//            //                //    }
//            //                //    lReqDate = String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lReqDate, "yyyy-MM-dd", CultureInfo.InvariantCulture));
//            //                //}

//            //                //var lReqDateConf = (lOraRst.GetValue(3) == DBNull.Value ? "" : lOraRst.GetString(3).Trim());
//            //                //if (lReqDateConf != null && lReqDateConf != "" && lReqDateConf != "20500101" &&
//            //                //    DateTime.ParseExact(lReqDateConf, "yyyyMMdd", CultureInfo.InvariantCulture) > DateTime.Now.AddMonths(-6) &&
//            //                //    DateTime.ParseExact(lReqDateConf, "yyyyMMdd", CultureInfo.InvariantCulture) < DateTime.Now.AddMonths(6))
//            //                //{
//            //                //    lReqDate = String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lReqDateConf, "yyyyMMdd", CultureInfo.InvariantCulture));
//            //                //}

//            //                var lBBSNo = (lOraRst.GetValue(6) == DBNull.Value ? "" : lOraRst.GetString(6).Trim());
//            //                var lPlanDelDate = (lOraRst.GetValue(4) == DBNull.Value ? "" : lOraRst.GetString(4).Trim());
//            //                if (lPlanDelDate != null && lPlanDelDate != "")
//            //                {
//            //                    if (String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lPlanDelDate, "yyyyMMdd", CultureInfo.InvariantCulture))
//            //                        == (String.Format("{0:yyyy-MM-dd}", DateTime.Now)))
//            //                    {
//            //                        lConfirmed = 1;
//            //                    }
//            //                    else if (String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lPlanDelDate, "yyyyMMdd", CultureInfo.InvariantCulture))
//            //                        == (String.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(1))))
//            //                    {
//            //                        lConfirmed = 2;
//            //                    }
//            //                    else
//            //                    {
//            //                        lConfirmed = 3;
//            //                    }

//            //                    if (DateTime.ParseExact(lPlanDelDate, "yyyyMMdd", CultureInfo.InvariantCulture) >= DateTime.Now.AddMonths(6))
//            //                    {
//            //                        lPlanDelDate = "";
//            //                    }
//            //                    else
//            //                    {
//            //                        lPlanDelDate = String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lPlanDelDate, "yyyyMMdd", CultureInfo.InvariantCulture));
//            //                    }
//            //                }
//            //                else
//            //                {
//            //                    lPlanDelDate = "";
//            //                }

//            //                //var lPlanDelConfDate = (lOraRst.GetValue(5) == DBNull.Value ? "" : lOraRst.GetString(5).Trim());
//            //                //if (lPlanDelConfDate != null && lPlanDelConfDate != "")
//            //                //{
//            //                //    if (String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lPlanDelConfDate, "yyyyMMdd", CultureInfo.InvariantCulture))
//            //                //        == (String.Format("{0:yyyy-MM-dd}", DateTime.Now)))
//            //                //    {
//            //                //        lConfirmed = 1;
//            //                //    }
//            //                //    else if (String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lPlanDelConfDate, "yyyyMMdd", CultureInfo.InvariantCulture))
//            //                //        == (String.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(1))))
//            //                //    {
//            //                //        lConfirmed = 2;
//            //                //    }
//            //                //    else
//            //                //    {
//            //                //        lConfirmed = 3;
//            //                //    }
//            //                //    //lReqDate = String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lPlanDelConfDate, "yyyyMMdd", CultureInfo.InvariantCulture));
//            //                //}

//            //                if (lReqDate.Length > 10)
//            //                {
//            //                    lReqDate = lReqDate.Substring(0, 10);
//            //                }

//            //                if (lSORNo != null && lSORNo != "" && lReturn.Count > 0 && lReqDate != null && lReqDate != "" &&
//            //                DateTime.ParseExact(lReqDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) > DateTime.Now.AddMonths(-6) &&
//            //                DateTime.ParseExact(lReqDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) < DateTime.Now.AddMonths(6))
//            //                {
//            //                    for (int i = 0; i < lReturn.Count; i++)
//            //                    {
//            //                        if (lReturn[i].SONo.IndexOf(lSORNo) >= 0)
//            //                        {
//            //                            var lSONOList = lReturn[i].SONo.Split(',').ToList();
//            //                            var lReqDateList = lReturn[i].RequiredDate.Split(',').ToList();
//            //                            if (lSONOList.Count > 0)
//            //                            {
//            //                                for (int j = 0; j < lSONOList.Count; j++)
//            //                                {
//            //                                    if (lSONOList[j] == lSORNo)
//            //                                    {
//            //                                        if (lReqDateList.Count > j)
//            //                                        {
//            //                                            lReqDateList[j] = lReqDate;
//            //                                        }
//            //                                        else
//            //                                        {
//            //                                            lReqDateList.Add(lReqDate);
//            //                                        }
//            //                                    }

//            //                                }
//            //                            }
//            //                            var lNewReqDate = "";
//            //                            if (lReqDateList.Count > 0)
//            //                            {
//            //                                for (int j = 0; j < lReqDateList.Count; j++)
//            //                                {
//            //                                    if (lNewReqDate == "")
//            //                                    {
//            //                                        lNewReqDate = lReqDateList[j];
//            //                                    }
//            //                                    else
//            //                                    {
//            //                                        if (lNewReqDate.IndexOf(lReqDateList[j]) < 0)
//            //                                        {
//            //                                            lNewReqDate = lNewReqDate + "," + lReqDateList[j];
//            //                                        }
//            //                                    }
//            //                                }
//            //                            }
//            //                            if (lNewReqDate == "")
//            //                            {
//            //                                lNewReqDate = lReqDate;
//            //                            }

//            //                            var lDeliveryDate = lReturn[i].PlanDeliveryDate;
//            //                            if (lDeliveryDate == null)
//            //                            {
//            //                                lDeliveryDate = "";
//            //                            }
//            //                            //lDeliveryDate = (lReturn[i].PlanDeliveryDate == null || lReturn[i].PlanDeliveryDate.Trim() == "") ? lPlanDelDate : ((lPlanDelDate != null && lPlanDelDate != "" && lReturn[i].PlanDeliveryDate.IndexOf(lPlanDelDate) < 0) ? lReturn[i].PlanDeliveryDate + "," + lPlanDelDate : lReturn[i].PlanDeliveryDate);

//            //                            if (lPlanDelDate != null && lPlanDelDate != "")
//            //                            {
//            //                                if (lDeliveryDate.Trim() == "")
//            //                                {
//            //                                    if (lSORNo.Substring(0, 3) == "103")
//            //                                    {
//            //                                        if (lReturn[i].SONo.LastIndexOf(",") > 0 && (lReturn[i].SONo.Substring(0, 1) == "8" || lReturn[i].SONo.IndexOf(",8") >= 0))
//            //                                        {
//            //                                            lDeliveryDate = lPlanDelDate + "(SB)";
//            //                                        }
//            //                                        else
//            //                                        {
//            //                                            lDeliveryDate = lPlanDelDate;
//            //                                        }
//            //                                    }
//            //                                    else if (lReturn[i].SONo.LastIndexOf(",") > 0 && (lReturn[i].SONo.Substring(0, 3) == "103" || lReturn[i].SONo.IndexOf(",103") > 0))
//            //                                    {
//            //                                        if (lBBSNo.IndexOf("-COUPLER") > 0 || lBBSNo.IndexOf("-CP") > 0)
//            //                                        {
//            //                                            lDeliveryDate = lPlanDelDate + "(CP)";
//            //                                        }
//            //                                        else
//            //                                        {
//            //                                            lDeliveryDate = lPlanDelDate + "(CAB)";
//            //                                        }
//            //                                    }
//            //                                    else
//            //                                    {
//            //                                        if (lReturn[i].SONo.LastIndexOf(",") > 0 && (lReturn[i].BBSNo.IndexOf("-COUPLER") > 0 || lReturn[i].BBSNo.IndexOf("-CP") > 0))
//            //                                        {
//            //                                            if (lBBSNo.IndexOf("-COUPLER") > 0 || lBBSNo.IndexOf("-CP") > 0)
//            //                                            {
//            //                                                lDeliveryDate = lPlanDelDate + "(CP)";
//            //                                            }
//            //                                            else
//            //                                            {
//            //                                                lDeliveryDate = lPlanDelDate + "(CAB)";
//            //                                            }
//            //                                        }
//            //                                        else
//            //                                        {
//            //                                            lDeliveryDate = lPlanDelDate;
//            //                                        }
//            //                                    }
//            //                                }
//            //                                else
//            //                                {
//            //                                    if (lDeliveryDate.IndexOf(lPlanDelDate) < 0)
//            //                                    {
//            //                                        if (lSORNo.Substring(0, 3) == "103")
//            //                                        {
//            //                                            if (lReturn[i].SONo.LastIndexOf(",") > 0 && (lReturn[i].SONo.Substring(0, 1) == "8" || lReturn[i].SONo.IndexOf(",8") >= 0))
//            //                                            {
//            //                                                lDeliveryDate = lDeliveryDate + "," + lPlanDelDate + "(SB)";
//            //                                            }
//            //                                            else
//            //                                            {
//            //                                                lDeliveryDate = lDeliveryDate + "," + lPlanDelDate;
//            //                                            }
//            //                                        }
//            //                                        else if (lReturn[i].SONo.LastIndexOf(",") > 0 && (lReturn[i].SONo.Substring(0, 3) == "103" || lReturn[i].SONo.IndexOf(",103") > 0))
//            //                                        {
//            //                                            if (lBBSNo.IndexOf("-COUPLER") > 0 || lBBSNo.IndexOf("-CP") > 0)
//            //                                            {
//            //                                                lDeliveryDate = lDeliveryDate + "," + lPlanDelDate + "(CP)";
//            //                                            }
//            //                                            else
//            //                                            {
//            //                                                lDeliveryDate = lDeliveryDate + "," + lPlanDelDate + "(CAB)";
//            //                                            }
//            //                                        }
//            //                                        else
//            //                                        {
//            //                                            if (lReturn[i].SONo.LastIndexOf(",") > 0 && (lReturn[i].BBSNo.IndexOf("-COUPLER") > 0 || lReturn[i].BBSNo.IndexOf("-CP") > 0))
//            //                                            {
//            //                                                if (lBBSNo.IndexOf("-COUPLER") > 0 || lBBSNo.IndexOf("-CP") > 0)
//            //                                                {
//            //                                                    lDeliveryDate = lDeliveryDate + "," + lPlanDelDate + "(CP)";
//            //                                                }
//            //                                                else
//            //                                                {
//            //                                                    lDeliveryDate = lDeliveryDate + "," + lPlanDelDate + "(CAB)";
//            //                                                }
//            //                                            }
//            //                                            else
//            //                                            {
//            //                                                lDeliveryDate = lDeliveryDate + "," + lPlanDelDate;
//            //                                            }
//            //                                        }
//            //                                    }
//            //                                    else
//            //                                    {
//            //                                        if (lDeliveryDate.IndexOf(",") < 0 && lDeliveryDate.IndexOf("(") > 0)
//            //                                        {
//            //                                            lDeliveryDate = lDeliveryDate.Substring(0, lDeliveryDate.IndexOf("("));
//            //                                        }
//            //                                    }
//            //                                }
//            //                            }

//            //                            lReturn[i] = new
//            //                            {
//            //                                SSNNo = lReturn[i].SSNNo,
//            //                                OrderNo = lReturn[i].OrderNo,
//            //                                WBS1 = lReturn[i].WBS1,
//            //                                WBS2 = lReturn[i].WBS2,
//            //                                WBS3 = lReturn[i].WBS3,
//            //                                StructureElement = lReturn[i].StructureElement,
//            //                                ProdType = lReturn[i].ProdType,
//            //                                PONo = lReturn[i].PONo,
//            //                                PODate = lReturn[i].PODate,
//            //                                RequiredDate = lNewReqDate,
//            //                                OrderWeight = lReturn[i].OrderWeight,
//            //                                SubmittedBy = lReturn[i].SubmittedBy,
//            //                                OrderStatus = lReturn[i].OrderStatus,
//            //                                OrderSource = lReturn[i].OrderSource,
//            //                                SONo = lReturn[i].SONo,
//            //                                SORNo = lReturn[i].SORNo,
//            //                                BBSNo = lReturn[i].BBSNo,
//            //                                BBSDesc = WebUtility.HtmlDecode(lReturn[i].BBSDesc),
//            //                                CustomerCode = lReturn[i].CustomerCode,
//            //                                ProjectCode = lReturn[i].ProjectCode,
//            //                                ProjectTitle = lReturn[i].ProjectTitle,
//            //                                DataEnteredBy = lReturn[i].DataEnteredBy,
//            //                                Confirmed = lReturn[i].Confirmed == 1 ? 1 : (lConfirmed == 1 ? 1 : (lReturn[i].Confirmed == 2 ? 2 : lConfirmed)),
//            //                                PlanDeliveryDate = lDeliveryDate,
//            //                                SubmitBy = lReturn[i].SubmitBy
//            //                            };
//            //                            break;
//            //                        }
//            //                    }
//            //                }

//            //            }
//            //        }
//            //        lOraRst.Close();
//            //    }
//            //}
//            //#endregion
//            lProcessObj.OpenNDSConnection(ref lNDSCon);

//            #region Check from CIS Plan Delivery Date
//            if (lSOR != "' '" && lSOR != "''")
//            {
//                var lSORList = new List<string>();
//                if (lSOR != "")
//                {
//                    var lSORA = lSOR.Split(',').ToList();
//                    var lSOR1 = "";
//                    if (lSORA.Count > 0)
//                    {
//                        int lCount = 0;
//                        for (int i = 0; i < lSORA.Count; i++)
//                        {
//                            if (lSOR1 == "")
//                            {
//                                //lSOR1 = "'" + lSORA[i] + "'";
//                                lSOR1 = lSORA[i];
//                            }
//                            else
//                            {
//                                //lSOR1 = lSOR1 + "," + "'" + lSORA[i] + "'";
//                                lSOR1 = lSOR1 + "," + lSORA[i];
//                            }
//                            lCount = lCount + 1;
//                            if (lCount > 300)
//                            {
//                                lSORList.Add(lSOR1);
//                                lSOR1 = "";
//                                lCount = 0;
//                            }
//                        }
//                        if (lSOR1 != "")
//                        {
//                            lSORList.Add(lSOR1);
//                        }
//                    }

//                }

//                for (int k = 0; k < lSORList.Count; k++)
//                {
//                    //lSQL = "SELECT " +
//                    //"NVL(M.ORDER_REQUEST_NO, ' '), " +
//                    //"M.REQ_DAT_TO, " +                                  //1. Revised Req date
//                    //"M.FIRST_PROMISED_D, " +                            //2. First Promised date
//                    //                                                    //"(SELECT NVL(MAX(conf_del_date), ' ') " +           
//                    //                                                    //"FROM SAPSR3.YMSDT_ORDER_ITEM " +
//                    //                                                    //"WHERE MANDT = M.MANDT " +
//                    //                                                    //"AND ORDER_REQUEST_NO = M.ORDER_REQUEST_NO) as Conf_del_date,  " + //3. Confirmed Del from CIS
//                    //"' ', " +
//                    ////"(SELECT MIN(PLAN_DELIV_DATE) FROM SAPSR3.YMPPT_LP_ITEM_C LC, SAPSR3.YMPPT_LP_HDR LH " +
//                    ////"WHERE LC.MANDT = LH.MANDT " +
//                    ////"AND LC.LOAD_NO = LH.LOAD_NO " +
//                    ////"AND SALES_ORDER = M.SALES_ORDER) as plan_del_date, " +         //4. Confirmed Del from Planning
//                    //"(SELECT MIN(LOAD_DATE) FROM SAPSR3.YMPPT_LP_ITEM_C LC, SAPSR3.YMPPT_LP_HDR LH " +
//                    //"WHERE LC.MANDT = LH.MANDT AND LC.LOAD_NO = LH.LOAD_NO AND LH.VEHICLE_TYPE <> 'MC' " +
//                    //"AND SALES_ORDER = M.SALES_ORDER) as plan_del_date, " +         //4. Confirmed Del from Planning
//                    //"(SELECT MIN(K.LFDAT) FROM SAPSR3.LIPS P, SAPSR3.LIKP K " +
//                    //"WHERE P.MANDT = K.MANDT " +
//                    //"AND P.VBELN = K.VBELN " +
//                    //"AND P.VGBEL = M.SALES_ORDER) as plan_del_conf, " +              //5. Confirmed Del from SAP
//                    //"D.BBS_NO " +
//                    //"FROM SAPSR3.YMSDT_ORDER_HDR M LEFT OUTER JOIN SAPSR3.YMSDT_REQ_DETAIL D " +
//                    //"ON M.order_request_no = D.order_request_no " +
//                    //"WHERE M.MANDT = '" + lProcessObj.strClient + "' " +
//                    //"AND M.PROJECT IN (" + lProjectState + ") " +
//                    //"AND M.REQD_DEL_DATE >= to_char(sysdate - 180, 'yyyymmdd') " +
//                    //"AND M.SALES_ORDER NOT IN " +
//                    //"(SELECT VBELN FROM  SAPSR3.VBUK " +
//                    //"WHERE VBELN = M.SALES_ORDER AND ABSTK = 'C') " +
//                    //"AND M.ORDER_REQUEST_NO IN " +
//                    //"(" + lSORList[k] + ") ";

//                    //lSQL = "SELECT M.ORDER_REQUEST_NO, M.REQ_DAT_TO, M.FIRST_PROMISED_D,'' ,(select min(PLAN_SHIPPING_DATE)  from SalesOrderloading sol " +
//                    //    "inner join HMILoadDetails hmi on sol.LOAD_NO=hmi.LOAD_NO where hmi.SALES_ORDER=m.sales_order) as plan_del_date,(select min(confirmdeldate)" +
//                    //    " from HMIOrderHeaderDetails where OrderNo=m.sales_order) AS plan_del_conf,D.BBS_NO FROM OesOrderHeaderHMI M LEFT OUTER JOIN  Oesrequestdetailshmi D " +
//                    //    "ON M.order_request_no = D.order_request_no WHERE M.MANDT = '600' AND M.PROJECT IN (" + lProjectState + ") AND M.REQD_DEL_DATE >= GETDATE() - 180 " +
//                    //    "AND M.Order_request_no in(" + lSORList[k] + ")";

//                    lSQL = "SELECT " +
//                           "ISNULL(M.ORDER_REQUEST_NO, ' ') AS ORDER_REQUEST_NO, " +
//                           "M.REQ_DAT_TO, " +                                                               //1. Revised Req date
//                           "M.FIRST_PROMISED_D, " +                                                         //2. First Promised date
//                           "' ' AS UNUSED_COLUMN, " +
//                           "(SELECT TOP 1 CONVERT(varchar, CONVERT(date, PLAN_SHIPPING_DATE, 103), 112) " +
//                               "FROM SalesOrderloading sol " +
//                               "INNER JOIN HMILoadDetails hmi ON sol.LOAD_NO = hmi.LOAD_NO " +
//                               "WHERE hmi.SALES_ORDER = M.SALES_ORDER) AS PLAN_DEL_DATE, " +                //3. Confirmed Del from HMI (Shipping Date)
//                           "(SELECT CONVERT(varchar, CONVERT(date, MIN(confirmdeldate), 103), 112) " +
//                               "FROM HMIOrderHeaderDetails " +
//                               "WHERE OrderNo = M.SALES_ORDER) AS PLAN_DEL_CONF, " +                        //4. Confirmed Del from HMI
//                           "D.BBS_NO " +                                                                    //5. BBS Number
//                           "FROM OesOrderHeaderHMI M " +
//                           "LEFT OUTER JOIN OESRequestDetailsHMI D ON M.ORDER_REQUEST_NO = D.ORDER_REQUEST_NO " +
//                           "WHERE M.MANDT = '" + lProcessObj.strClient + "' " +
//                           "AND M.PROJECT IN (" + lProjectState + ") " +
//                           "AND M.REQD_DEL_DATE >= GETDATE() - 180 " +
//                           "AND M.STATUS <> 'X' " +
//                           "AND M.ORDER_REQUEST_NO IN (" + lSORList[k] + ")";


//                    lCmd.CommandText = lSQL;
//                    lCmd.Connection = lNDSCon;
//                    lCmd.CommandTimeout = 300;
//                    lRst = await lCmd.ExecuteReaderAsync();
//                    if (lRst.HasRows)
//                    {
//                        while (lRst.Read())
//                        {
//                            int lConfirmed = 0;
//                            var lSORNo = (lRst.GetValue(0) == DBNull.Value ? "" : lRst.GetString(0));

//                            //var lReqDate = (lOraRst.GetValue(2) == DBNull.Value ? "" : lOraRst.GetString(2).Trim());
//                            //if (lReqDate == "")
//                            //{
//                            var lReqDate = (lRst.GetValue(1) == DBNull.Value ? "" : lRst.GetString(1).Trim());
//                            //}
//                            //else
//                            //{
//                            //    if (lReqDate.Length > 10)
//                            //    {
//                            //        lReqDate = lReqDate.Substring(0, 10);
//                            //    }
//                            //    lReqDate = String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lReqDate, "yyyy-MM-dd", CultureInfo.InvariantCulture));
//                            //}

//                            //var lReqDateConf = (lOraRst.GetValue(3) == DBNull.Value ? "" : lOraRst.GetString(3).Trim());
//                            //if (lReqDateConf != null && lReqDateConf != "" && lReqDateConf != "20500101" &&
//                            //    DateTime.ParseExact(lReqDateConf, "yyyyMMdd", CultureInfo.InvariantCulture) > DateTime.Now.AddMonths(-6) &&
//                            //    DateTime.ParseExact(lReqDateConf, "yyyyMMdd", CultureInfo.InvariantCulture) < DateTime.Now.AddMonths(6))
//                            //{
//                            //    lReqDate = String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lReqDateConf, "yyyyMMdd", CultureInfo.InvariantCulture));
//                            //}

//                            var lBBSNo = (lRst.GetValue(6) == DBNull.Value ? "" : lRst.GetString(6).Trim());
//                            var lPlanDelDate = (lRst.GetValue(4) == DBNull.Value ? "" : lRst.GetString(4).Trim());
//                            if (lPlanDelDate != null && lPlanDelDate != "")
//                            {
//                                if (String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lPlanDelDate, "yyyyMMdd", CultureInfo.InvariantCulture))
//                                    == (String.Format("{0:yyyy-MM-dd}", DateTime.Now)))
//                                {
//                                    lConfirmed = 1;
//                                }
//                                else if (String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lPlanDelDate, "yyyyMMdd", CultureInfo.InvariantCulture))
//                                    == (String.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(1))))
//                                {
//                                    lConfirmed = 2;
//                                }
//                                else
//                                {
//                                    lConfirmed = 3;
//                                }

//                                if (DateTime.ParseExact(lPlanDelDate, "yyyyMMdd", CultureInfo.InvariantCulture) >= DateTime.Now.AddMonths(6))
//                                {
//                                    lPlanDelDate = "";
//                                }
//                                else
//                                {
//                                    lPlanDelDate = String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lPlanDelDate, "yyyyMMdd", CultureInfo.InvariantCulture));
//                                }
//                            }
//                            else
//                            {
//                                lPlanDelDate = "";
//                            }

//                            //var lPlanDelConfDate = (lOraRst.GetValue(5) == DBNull.Value ? "" : lOraRst.GetString(5).Trim());
//                            //if (lPlanDelConfDate != null && lPlanDelConfDate != "")
//                            //{
//                            //    if (String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lPlanDelConfDate, "yyyyMMdd", CultureInfo.InvariantCulture))
//                            //        == (String.Format("{0:yyyy-MM-dd}", DateTime.Now)))
//                            //    {
//                            //        lConfirmed = 1;
//                            //    }
//                            //    else if (String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lPlanDelConfDate, "yyyyMMdd", CultureInfo.InvariantCulture))
//                            //        == (String.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(1))))
//                            //    {
//                            //        lConfirmed = 2;
//                            //    }
//                            //    else
//                            //    {
//                            //        lConfirmed = 3;
//                            //    }
//                            //    //lReqDate = String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lPlanDelConfDate, "yyyyMMdd", CultureInfo.InvariantCulture));
//                            //}

//                            if (lReqDate.Length > 10)
//                            {
//                                lReqDate = lReqDate.Substring(0, 10);
//                            }

//                            if (lSORNo != null && lSORNo != "" && lReturn.Count > 0 && lReqDate != null && lReqDate != "" &&
//                            DateTime.ParseExact(lReqDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) > DateTime.Now.AddMonths(-6) &&
//                            DateTime.ParseExact(lReqDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) < DateTime.Now.AddMonths(6))
//                            {
//                                for (int i = 0; i < lReturn.Count; i++)
//                                {
//                                    if (lReturn[i].SONo.IndexOf(lSORNo) >= 0)
//                                    {
//                                        var lSONOList = lReturn[i].SONo.Split(',').ToList();
//                                        var lReqDateList = lReturn[i].RequiredDate.Split(',').ToList();
//                                        if (lSONOList.Count > 0)
//                                        {
//                                            for (int j = 0; j < lSONOList.Count; j++)
//                                            {
//                                                if (lSONOList[j] == lSORNo)
//                                                {
//                                                    if (lReqDateList.Count > j)
//                                                    {
//                                                        lReqDateList[j] = lReqDate;
//                                                    }
//                                                    else
//                                                    {
//                                                        lReqDateList.Add(lReqDate);
//                                                    }
//                                                }

//                                            }
//                                        }
//                                        var lNewReqDate = "";
//                                        if (lReqDateList.Count > 0)
//                                        {
//                                            for (int j = 0; j < lReqDateList.Count; j++)
//                                            {
//                                                if (lNewReqDate == "")
//                                                {
//                                                    lNewReqDate = lReqDateList[j];
//                                                }
//                                                else
//                                                {
//                                                    if (lNewReqDate.IndexOf(lReqDateList[j]) < 0)
//                                                    {
//                                                        lNewReqDate = lNewReqDate + "," + lReqDateList[j];
//                                                    }
//                                                }
//                                            }
//                                        }
//                                        if (lNewReqDate == "")
//                                        {
//                                            lNewReqDate = lReqDate;
//                                        }

//                                        var lDeliveryDate = lReturn[i].PlanDeliveryDate;
//                                        if (lDeliveryDate == null)
//                                        {
//                                            lDeliveryDate = "";
//                                        }
//                                        //lDeliveryDate = (lReturn[i].PlanDeliveryDate == null || lReturn[i].PlanDeliveryDate.Trim() == "") ? lPlanDelDate : ((lPlanDelDate != null && lPlanDelDate != "" && lReturn[i].PlanDeliveryDate.IndexOf(lPlanDelDate) < 0) ? lReturn[i].PlanDeliveryDate + "," + lPlanDelDate : lReturn[i].PlanDeliveryDate);

//                                        if (lPlanDelDate != null && lPlanDelDate != "")
//                                        {
//                                            if (lDeliveryDate.Trim() == "")
//                                            {
//                                                if (lSORNo.Substring(0, 3) == "103")
//                                                {
//                                                    if (lReturn[i].SONo.LastIndexOf(",") > 0 && (lReturn[i].SONo.Substring(0, 1) == "8" || lReturn[i].SONo.IndexOf(",8") >= 0))
//                                                    {
//                                                        lDeliveryDate = lPlanDelDate + "(SB)";
//                                                    }
//                                                    else
//                                                    {
//                                                        lDeliveryDate = lPlanDelDate;
//                                                    }
//                                                }
//                                                else if (lReturn[i].SONo.LastIndexOf(",") > 0 && (lReturn[i].SONo.Substring(0, 3) == "103" || lReturn[i].SONo.IndexOf(",103") > 0))
//                                                {
//                                                    if (lBBSNo.IndexOf("-COUPLER") > 0 || lBBSNo.IndexOf("-CP") > 0)
//                                                    {
//                                                        lDeliveryDate = lPlanDelDate + "(CP)";
//                                                    }
//                                                    else
//                                                    {
//                                                        lDeliveryDate = lPlanDelDate + "(CAB)";
//                                                    }
//                                                }
//                                                else
//                                                {
//                                                    if (lReturn[i].SONo.LastIndexOf(",") > 0 && (lReturn[i].BBSNo.IndexOf("-COUPLER") > 0 || lReturn[i].BBSNo.IndexOf("-CP") > 0))
//                                                    {
//                                                        if (lBBSNo.IndexOf("-COUPLER") > 0 || lBBSNo.IndexOf("-CP") > 0)
//                                                        {
//                                                            lDeliveryDate = lPlanDelDate + "(CP)";
//                                                        }
//                                                        else
//                                                        {
//                                                            lDeliveryDate = lPlanDelDate + "(CAB)";
//                                                        }
//                                                    }
//                                                    else
//                                                    {
//                                                        lDeliveryDate = lPlanDelDate;
//                                                    }
//                                                }
//                                            }
//                                            else
//                                            {
//                                                if (lDeliveryDate.IndexOf(lPlanDelDate) < 0)
//                                                {
//                                                    if (lSORNo.Substring(0, 3) == "103")
//                                                    {
//                                                        if (lReturn[i].SONo.LastIndexOf(",") > 0 && (lReturn[i].SONo.Substring(0, 1) == "8" || lReturn[i].SONo.IndexOf(",8") >= 0))
//                                                        {
//                                                            lDeliveryDate = lDeliveryDate + "," + lPlanDelDate + "(SB)";
//                                                        }
//                                                        else
//                                                        {
//                                                            lDeliveryDate = lDeliveryDate + "," + lPlanDelDate;
//                                                        }
//                                                    }
//                                                    else if (lReturn[i].SONo.LastIndexOf(",") > 0 && (lReturn[i].SONo.Substring(0, 3) == "103" || lReturn[i].SONo.IndexOf(",103") > 0))
//                                                    {
//                                                        if (lBBSNo.IndexOf("-COUPLER") > 0 || lBBSNo.IndexOf("-CP") > 0)
//                                                        {
//                                                            lDeliveryDate = lDeliveryDate + "," + lPlanDelDate + "(CP)";
//                                                        }
//                                                        else
//                                                        {
//                                                            lDeliveryDate = lDeliveryDate + "," + lPlanDelDate + "(CAB)";
//                                                        }
//                                                    }
//                                                    else
//                                                    {
//                                                        if (lReturn[i].SONo.LastIndexOf(",") > 0 && (lReturn[i].BBSNo.IndexOf("-COUPLER") > 0 || lReturn[i].BBSNo.IndexOf("-CP") > 0))
//                                                        {
//                                                            if (lBBSNo.IndexOf("-COUPLER") > 0 || lBBSNo.IndexOf("-CP") > 0)
//                                                            {
//                                                                lDeliveryDate = lDeliveryDate + "," + lPlanDelDate + "(CP)";
//                                                            }
//                                                            else
//                                                            {
//                                                                lDeliveryDate = lDeliveryDate + "," + lPlanDelDate + "(CAB)";
//                                                            }
//                                                        }
//                                                        else
//                                                        {
//                                                            lDeliveryDate = lDeliveryDate + "," + lPlanDelDate;
//                                                        }
//                                                    }
//                                                }
//                                                else
//                                                {
//                                                    if (lDeliveryDate.IndexOf(",") < 0 && lDeliveryDate.IndexOf("(") > 0)
//                                                    {
//                                                        lDeliveryDate = lDeliveryDate.Substring(0, lDeliveryDate.IndexOf("("));
//                                                    }
//                                                }
//                                            }
//                                        }

//                                        lReturn[i] = new
//                                        {
//                                            SSNNo = lReturn[i].SSNNo,
//                                            OrderNo = lReturn[i].OrderNo,
//                                            WBS1 = lReturn[i].WBS1,
//                                            WBS2 = lReturn[i].WBS2,
//                                            WBS3 = lReturn[i].WBS3,
//                                            StructureElement = lReturn[i].StructureElement,
//                                            ProdType = lReturn[i].ProdType,
//                                            PONo = lReturn[i].PONo,
//                                            PODate = lReturn[i].PODate,
//                                            RequiredDate = lNewReqDate,
//                                            OrderWeight = lReturn[i].OrderWeight,
//                                            SubmittedBy = lReturn[i].SubmittedBy,
//                                            OrderStatus = lReturn[i].OrderStatus,
//                                            OrderSource = lReturn[i].OrderSource,
//                                            SONo = lReturn[i].SONo,
//                                            SORNo = lReturn[i].SORNo,
//                                            BBSNo = lReturn[i].BBSNo,
//                                            BBSDesc = WebUtility.HtmlDecode(lReturn[i].BBSDesc),
//                                            CustomerCode = lReturn[i].CustomerCode,
//                                            ProjectCode = lReturn[i].ProjectCode,
//                                            ProjectTitle = lReturn[i].ProjectTitle,
//                                            DataEnteredBy = lReturn[i].DataEnteredBy,
//                                            Confirmed = lReturn[i].Confirmed == 1 ? 1 : (lConfirmed == 1 ? 1 : (lReturn[i].Confirmed == 2 ? 2 : lConfirmed)),
//                                            PlanDeliveryDate = lDeliveryDate,
//                                            SubmitBy = lReturn[i].SubmitBy
//                                        };
//                                        break;
//                                    }
//                                }
//                            }

//                        }
//                    }
//                    lRst.Close();
//                    lProcessObj.CloseNDSConnection(ref lNDSCon);
//                }
//            }
//            #endregion


//            #region Check Individual SO, but Order Delivered 
//            lPODateFrom_O = lPODateFrom.Replace("-", "");
//            lPODateTo_O = lPODateTo.Replace("-", "");
//            lRDateFrom_O = lRDateFrom.Replace("-", "");
//            lRDateTo_O = lRDateTo.Replace("-", "");

//            var lBalSNo = 0;

//            lSQL = "SELECT NVL(D.WBS1, ' ') as WBS1, " +
//            "NVL(D.WBS2, ' ') as WBS2, " +
//            "NVL(D.WBS3, ' ') as WBS3,  " +
//            "NVL(D.ST_ELEMENT_TYPE, ' ') as ST_ELE,  " +
//            "NVL(D.PRODUCT_TYPE, ' ') as Prod_type, " +
//            "M.PO_NUMBER, " +
//            "M.CUST_ORDER_DATE, " +
//            "M.REQ_DAT_TO, " +
//            "M.FIRST_PROMISED_D, " +
//            "(SELECT NVL(SUM(THEO_WEIGHT_KG), 0)/1000 " +
//            "FROM SAPSR3.YMSDT_ORDER_ITEM " +
//            "WHERE MANDT = M.MANDT " +
//            "AND ORDER_REQUEST_NO = M.ORDER_REQUEST_NO " +
//            "AND HG_ITEM_NO = '000000') as WT, " +
//            "'' as SubumittedBY, " +
//            "'Reviewed', " +
//            "(SELECT NVL(MAX(PROD_GRP), ' ') " +
//            "FROM SAPSR3.YMSDT_ORDER_ITEM " +
//            "WHERE MANDT = M.MANDT " +
//            "AND ORDER_REQUEST_NO = M.ORDER_REQUEST_NO) as ProdType2,  " +
//            //"(SELECT NVL(MAX(conf_del_date), ' ') " +
//            //"FROM SAPSR3.YMSDT_ORDER_ITEM " +
//            //"WHERE MANDT = M.MANDT " +
//            //"AND ORDER_REQUEST_NO = M.ORDER_REQUEST_NO) as Conf_del_date,  " +       //13 Conf_del_date
//            "' ', " +
//            "NVL(M.SALES_ORDER, ' '), " +                                                       //14 Sales Order no
//            "(SELECT MIN(PLAN_DELIV_DATE) FROM SAPSR3.YMPPT_LP_ITEM_C LC, SAPSR3.YMPPT_LP_HDR LH " +
//            "WHERE LC.MANDT = LH.MANDT " +
//            "AND LC.LOAD_NO = LH.LOAD_NO AND LH.VEHICLE_TYPE <> 'MC' " +
//            "AND SALES_ORDER = M.SALES_ORDER) as plan_del_date, " +                      //15 Plan del date
//            "(SELECT MIN(K.LFDAT) FROM SAPSR3.LIPS P, SAPSR3.LIKP K " +
//            "WHERE P.MANDT = K.MANDT " +
//            "AND P.VBELN = K.VBELN " +
//            "AND P.VGBEL = M.SALES_ORDER) as plan_del_conf, " +                          // 16 Plan conf del date
//            "M.ORDER_REQUEST_NO, " +                                                     // 17 SOR
//            "D.BBS_NO, " +
//            "D.BBS_DESC, " +
//            "M.KUNAG, " +
//            "M.KUNNR, " +
//            "M.NAME_WE " +
//            "FROM SAPSR3.YMSDT_ORDER_HDR M LEFT OUTER JOIN SAPSR3.YMSDT_REQ_DETAIL D  " +
//            "ON M.MANDT = D.MANDT AND M.order_request_no = D.order_request_no " +
//            "WHERE M.MANDT = '" + lProcessObj.strClient + "' " +
//            "AND M.KUNAG = '" + CustomerCode + "' " +
//            "AND M.PROJECT IN (" + lProjectState + ") " +
//            "AND M.STATUS <> 'X' " +
//            "AND (EXISTS (SELECT VBELN FROM SAPSR3.VBAK WHERE VBELN = M.SALES_ORDER) " +
//            "OR M.SALES_ORDER is NULL " +
//            "OR M.SALES_ORDER = ' ') " +
//            "AND M.REQD_DEL_DATE >= to_char(sysdate - 180, 'yyyymmdd') " +
//            "AND M.SALES_ORDER NOT IN " +
//            "(SELECT VBELN FROM  SAPSR3.VBUK " +
//            "WHERE VBELN = M.SALES_ORDER AND ABSTK = 'C') " +
//            //"AND M.ORDER_REQUEST_NO NOT IN " +
//            //"(" + lSOR + ") " +
//            "AND NOT EXISTS " +
//            "(SELECT P.VGBEL FROM SAPSR3.LIPS P, SAPSR3.LIKP K " +
//            "WHERE P.MANDT = K.MANDT " +
//            "AND P.VBELN = K.VBELN " +
//            "AND P.VGBEL = M.SALES_ORDER " +
//            "AND K.wadat_ist > '00000000' ) " +
//            //"AND K.LFDAT < TO_CHAR(SYSDATE, 'YYYYMMDD') ) " +
//            "AND NOT EXISTS " +
//            "(SELECT VBELN FROM SAPSR3.YMPPT_LOAD_VEHIC WHERE " +
//            "MANDT = M.MANDT AND VBELN = M.SALES_ORDER  AND LOAD_DATE <= TO_CHAR(SYSDATE, 'YYYYMMDD')) " +
//            //"AND (M.PO_NUMBER like '%" + PONumber + "%' " +
//            //"OR M.PO_NUMBER = ' ' ) " +
//            "AND ((M.CUST_ORDER_DATE >= '" + lPODateFrom_O + "' " +
//            "AND M.CUST_ORDER_DATE <= '" + lPODateTo_O + "') " +
//            "OR M.CUST_ORDER_DATE is NULL  " +
//            "OR M.CUST_ORDER_DATE = ' ' ) " +
//            "AND ((M.REQD_DEL_DATE >= '" + lRDateFrom_O + "' " +
//            "AND M.REQD_DEL_DATE <= '" + lRDateTo_O + "') " +
//            "OR M.REQD_DEL_DATE is NULL  " +
//            "OR M.REQD_DEL_DATE = ' ' ) " +
//            //"AND (D.WBS1 like '%" + WBS1 + "%' " +
//            //"OR (D.WBS1 is null AND '" + WBS1 + "' = ' ' )) " +
//            //"AND (D.WBS2 like '%" + WBS2 + "%' " +
//            //"OR (D.WBS2 is null AND '" + WBS2 + "' = ' ' )) " +
//            //"AND (D.WBS3 like '%" + WBS3 + "%' " +
//            //"OR (D.WBS3 is null AND '" + WBS3 + "' = ' ' )) " +
//            "AND (D.STATUS <> 'X' " +
//            "OR D.STATUS is NULL) " +
//            "ORDER BY 7 ";

//            lOraCmd.CommandText = lSQL;
//            lOraCmd.Connection = lCISCon;
//            lOraCmd.CommandTimeout = 300;
//            lOraRst = (OracleDataReader)await lOraCmd.ExecuteReaderAsync();
//            if (lOraRst.HasRows)
//            {
//                while (lOraRst.Read())
//                {
//                    var lBBSNo = lOraRst.GetValue(18) == DBNull.Value ? "" : lOraRst.GetString(18);
//                    var lBBSDesc = WebUtility.HtmlDecode(lOraRst.GetValue(19) == DBNull.Value ? "" : lOraRst.GetString(19));
//                    var lSORNo = lOraRst.GetString(17).Trim();
//                    if (lSOR.IndexOf(lSORNo) < 0)
//                    {
//                        lBalSNo = lBalSNo + 1;

//                        int lConfirmed = 0;

//                        var lPODate = (lOraRst.GetValue(6) == DBNull.Value ? "" : lOraRst.GetString(6));
//                        lPODate = String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lPODate, "yyyyMMdd", CultureInfo.InvariantCulture));

//                        //var lReqDate = (lOraRst.GetValue(8) == DBNull.Value ? "" : lOraRst.GetString(8).Trim());
//                        //if (lReqDate == "")
//                        //{
//                        var lReqDate = (lOraRst.GetValue(7) == DBNull.Value ? "" : lOraRst.GetString(7).Trim());
//                        //} else
//                        //{
//                        //    if (lReqDate.Length > 10)
//                        //    {
//                        //        lReqDate = lReqDate.Substring(0, 10);
//                        //    }
//                        //    lReqDate = String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lReqDate, "yyyy-MM-dd", CultureInfo.InvariantCulture));
//                        //}

//                        //var lReqDateConf = (lOraRst.GetValue(13) == DBNull.Value ? "" : lOraRst.GetString(13).Trim());
//                        //if (lReqDateConf != null && lReqDateConf != "")
//                        //{
//                        //    lReqDate = String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lReqDateConf, "yyyyMMdd", CultureInfo.InvariantCulture));
//                        //}

//                        var lPlanDelDate = (lOraRst.GetValue(15) == DBNull.Value ? "" : lOraRst.GetString(15).Trim());
//                        if (lPlanDelDate != null && lPlanDelDate != "" && lPlanDelDate != "20500101")
//                        {
//                            if (String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lPlanDelDate, "yyyyMMdd", CultureInfo.InvariantCulture))
//                                == (String.Format("{0:yyyy-MM-dd}", DateTime.Now)))
//                            {
//                                lConfirmed = 1;
//                            }
//                            else if (String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lPlanDelDate, "yyyyMMdd", CultureInfo.InvariantCulture))
//                                == (String.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(1))))
//                            {
//                                lConfirmed = 2;
//                            }
//                            else
//                            {
//                                lConfirmed = 3;
//                            }
//                            lPlanDelDate = String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lPlanDelDate, "yyyyMMdd", CultureInfo.InvariantCulture));
//                        }
//                        else
//                        {
//                            lPlanDelDate = "";
//                        }

//                        //var lPlanDelConfDate = (lOraRst.GetValue(16) == DBNull.Value ? "" : lOraRst.GetString(16).Trim());
//                        //if (lPlanDelConfDate != null && lPlanDelConfDate != "")
//                        //{
//                        //    if (String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lPlanDelConfDate, "yyyyMMdd", CultureInfo.InvariantCulture))
//                        //        == (String.Format("{0:yyyy-MM-dd}", DateTime.Now)))
//                        //    {
//                        //        lConfirmed = 1;
//                        //    }
//                        //    else if (String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lPlanDelConfDate, "yyyyMMdd", CultureInfo.InvariantCulture))
//                        //        == (String.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(1))))
//                        //    {
//                        //        lConfirmed = 2;
//                        //    }
//                        //    else
//                        //    {
//                        //        lConfirmed = 3;
//                        //    }
//                        //    //lReqDate = String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lPlanDelConfDate, "yyyyMMdd", CultureInfo.InvariantCulture));
//                        //}

//                        if (lReqDate.Length > 10)
//                        {
//                            lReqDate = lReqDate.Substring(0, 10);
//                        }

//                        var lProdType = (lOraRst.GetValue(4) == DBNull.Value ? "" : lOraRst.GetString(4).Trim());
//                        if (lProdType.Trim() == "")
//                        {
//                            var lProdType2 = (lOraRst.GetValue(12) == DBNull.Value ? "" : lOraRst.GetString(12).Trim());
//                            if (lProdType2 == "MSHSTM")
//                            {
//                                lProdType = "STANDARD-MESH";
//                            }
//                            else if (lProdType2 == "BARDBX" || lProdType2 == "BARRBX")
//                            {
//                                lProdType = "STANDARD-BAR";
//                            }
//                            else if (lProdType2 == "BARDBI")
//                            {
//                                lProdType = "DBIC";
//                            }
//                            else if (lProdType2 == "BARRBI")
//                            {
//                                lProdType = "RBIC";
//                            }
//                            else if (lProdType2 == "COUNSP")
//                            {
//                                lProdType = "COUPLER";
//                            }
//                            else if (lProdType2 == "CRWWPR")
//                            {
//                                lProdType = "Cold Rolled Wire";
//                            }
//                            else if (lProdType2 == "PCSPCS")
//                            {
//                                lProdType = "PC Strand";
//                            }
//                            else if (lProdType2 == "WRDWRD")
//                            {
//                                lProdType = "Wire Rod";
//                            }
//                            else
//                            {
//                                lProdType = lProdType2;
//                            }
//                        }
//                        lReturn.Add(new
//                        {
//                            SSNNo = lBalSNo,
//                            OrderNo = "N" + lBalSNo.ToString(),
//                            WBS1 = (lOraRst.GetValue(0) == DBNull.Value ? "" : lOraRst.GetString(0).Trim()),
//                            WBS2 = (lOraRst.GetValue(1) == DBNull.Value ? "" : lOraRst.GetString(1).Trim()),
//                            WBS3 = (lOraRst.GetValue(2) == DBNull.Value ? "" : lOraRst.GetString(2).Trim()),
//                            StructureElement = lOraRst.GetValue(3) == DBNull.Value ? "" : (lOraRst.GetString(3).Trim() == "NONWBS" ? "" : lOraRst.GetString(3).Trim()),
//                            ProdType = lProdType,
//                            PONo = (lOraRst.GetValue(5) == DBNull.Value ? "" : lOraRst.GetString(5).Trim()),
//                            PODate = lPODate,
//                            RequiredDate = lReqDate,
//                            OrderWeight = (lOraRst.GetValue(9) == DBNull.Value ? "0.000" : lOraRst.GetDecimal(9).ToString("###,###,##0.000;(###,##0.000); ")),
//                            SubmittedBy = (lOraRst.GetValue(10) == DBNull.Value ? "" : lOraRst.GetString(10).Trim()),
//                            OrderStatus = "Reviewed",
//                            OrderSource = "SAP",
//                            SONo = (lOraRst.GetValue(14) == DBNull.Value || lOraRst.GetString(14) == "") ? lSORNo : lOraRst.GetString(14).Trim(),
//                            SORNo = lSORNo,
//                            BBSNo = lBBSNo,
//                            BBSDesc = lBBSDesc,
//                            CustomerCode = (lOraRst.GetValue(20) == DBNull.Value ? "" : lOraRst.GetString(20).Trim()),
//                            ProjectCode = (lOraRst.GetValue(21) == DBNull.Value ? "" : lOraRst.GetString(21).Trim()),
//                            ProjectTitle = (lOraRst.GetValue(22) == DBNull.Value ? "" : lOraRst.GetString(22).Trim()),
//                            DataEnteredBy = "",
//                            Confirmed = lConfirmed,
//                            PlanDeliveryDate = lPlanDelDate,
//                            SubmitBy = (lOraRst.GetValue(18) == DBNull.Value ? "" : lOraRst.GetString(18).Trim())
//                        });

//                    }
//                }
//            }
//            lOraRst.Close();
//            #endregion


//            #region Check Partial Delivery
//            lBalSNo = 0;

//            lSQL = "SELECT NVL(D.WBS1, ' ') as WBS1, " +
//            "NVL(D.WBS2, ' ') as WBS2, " +
//            "NVL(D.WBS3, ' ') as WBS3,  " +
//            "NVL(D.ST_ELEMENT_TYPE, ' ') as ST_ELE,  " +
//            "NVL(D.PRODUCT_TYPE, ' ') as Prod_type, " +
//            "M.PO_NUMBER, " +
//            "M.CUST_ORDER_DATE, " +
//            "M.REQ_DAT_TO, " +
//            "M.FIRST_PROMISED_D, " +

//            "(SELECT NVL(SUM(CASE WHEN T1.MEINS = 'ST' AND T1.KWMENG > 0 THEN T1.THEO_WEIGHT_KG * C.NO_PIECES / T1.KWMENG ELSE T1.THEO_WEIGHT_KG END), 0)/1000 " +
//            "FROM SAPSR3.YMPPT_LP_ITEM_C C, SAPSR3.YMSDT_ORDER_ITEM T1 " +
//            "WHERE C.SALES_ORDER = M.SALES_ORDER " +
//            "AND (C.STATUS = 'A' OR C.STATUS = 'Y') " +
//            "AND T1.MANDT = M.MANDT " +
//            "AND T1.ORDER_REQUEST_NO = M.ORDER_REQUEST_NO " +
//            "AND T1.HG_ITEM_NO = '000000' " +
//            "AND T1.ORDER_ITEM = C.ORDER_ITEM " +
//            "AND NOT EXISTS( " +
//            "SELECT K.VBELN FROM SAPSR3.LIPS P, SAPSR3.LIKP K  " +
//            "WHERE P.MANDT = K.MANDT " +
//            "AND P.VBELN = K.VBELN " +
//            "AND P.VGBEL = M.SALES_ORDER " +
//            "AND K.MANDT = C.MANDT " +
//            "AND K.LIFEX = C.LOAD_NO " +
//            "AND K.wadat_ist > '00000000') " +
//            "AND NOT EXISTS " +
//            "(SELECT LOAD_NO FROM SAPSR3.YMPPT_LOAD_VEHIC WHERE " +
//            "MANDT = C.MANDT AND LOAD_NO = C.LOAD_NO AND LOAD_DATE <= TO_CHAR(SYSDATE, 'YYYYMMDD')) ) as WT, " +

//            "'' as SubumittedBY, " +
//            "'Production', " +
//            "(SELECT NVL(MAX(PROD_GRP), ' ') " +
//            "FROM SAPSR3.YMSDT_ORDER_ITEM " +
//            "WHERE MANDT = M.MANDT " +
//            "AND ORDER_REQUEST_NO = M.ORDER_REQUEST_NO) as ProdType2,  " +
//            "' ', " +
//            "NVL(M.SALES_ORDER, ' '), " +                                                       //14 Sales Order no
//            "(SELECT MIN(PLAN_DELIV_DATE) FROM SAPSR3.YMPPT_LP_ITEM_C LC, SAPSR3.YMPPT_LP_HDR LH " +
//            "WHERE LC.MANDT = LH.MANDT " +
//            "AND LC.LOAD_NO = LH.LOAD_NO AND LH.VEHICLE_TYPE <> 'MC' " +
//            "AND SALES_ORDER = M.SALES_ORDER " +
//            "AND (LC.STATUS = 'A' OR LC.STATUS = 'Y') " +
//            "AND NOT EXISTS( " +
//            "SELECT K.VBELN FROM SAPSR3.LIPS P, SAPSR3.LIKP K  " +
//            "WHERE P.MANDT = K.MANDT " +
//            "AND P.VBELN = K.VBELN " +
//            "AND P.VGBEL = M.SALES_ORDER " +
//            "AND K.MANDT = LC.MANDT " +
//            "AND K.LIFEX = LC.LOAD_NO " +
//            "AND K.wadat_ist > '00000000') " +
//            "AND NOT EXISTS " +
//            "(SELECT LOAD_NO FROM SAPSR3.YMPPT_LOAD_VEHIC WHERE " +
//            "MANDT = LC.MANDT AND LOAD_NO = LC.LOAD_NO AND LOAD_DATE <= TO_CHAR(SYSDATE, 'YYYYMMDD')) " +
//            ") as plan_del_date, " +                      //15 Plan del date
//            "(SELECT MIN(K.LFDAT) FROM SAPSR3.LIPS P, SAPSR3.LIKP K " +
//            "WHERE P.MANDT = K.MANDT " +
//            "AND P.VBELN = K.VBELN " +
//            "AND P.VGBEL = M.SALES_ORDER) as plan_del_conf, " +                          // 16 Plan conf del date
//            "M.ORDER_REQUEST_NO, " +                                                     // 17 SOR
//            "D.BBS_NO, " +
//            "D.BBS_DESC, " +
//            "M.KUNAG, " +
//            "M.KUNNR, " +
//            "M.NAME_WE " +
//            "FROM SAPSR3.YMSDT_ORDER_HDR M LEFT OUTER JOIN SAPSR3.YMSDT_REQ_DETAIL D  " +
//            "ON M.MANDT = D.MANDT AND M.order_request_no = D.order_request_no " +
//            "WHERE M.MANDT = '" + lProcessObj.strClient + "' " +
//            "AND M.KUNAG = '" + CustomerCode + "' " +
//            "AND M.PROJECT IN (" + lProjectState + ") " +
//            "AND M.STATUS <> 'X' " +
//            "AND (EXISTS (SELECT VBELN FROM SAPSR3.VBAK WHERE VBELN = M.SALES_ORDER) " +
//            "OR M.SALES_ORDER is NULL " +
//            "OR M.SALES_ORDER = ' ') " +
//            "AND M.REQD_DEL_DATE >= to_char(sysdate - 180, 'yyyymmdd') " +
//            "AND M.SALES_ORDER NOT IN " +
//            "(SELECT VBELN FROM  SAPSR3.VBUK " +
//            "WHERE MANDT = M.MANDT AND VBELN = M.SALES_ORDER AND ABSTK = 'C') " +
//            //"AND M.ORDER_REQUEST_NO NOT IN " +
//            //"(" + lSOR + ") " +
//            "AND EXISTS " +
//            "(SELECT LOAD_NO FROM SAPSR3.YMPPT_LP_ITEM_C C " +
//            "WHERE C.SALES_ORDER = M.SALES_ORDER " +
//            "AND (C.STATUS = 'A' OR C.STATUS = 'Y') " +
//            "AND LOAD_DATE >= TO_CHAR(SYSDATE - 10, 'YYYYMMDD') " +
//            "AND EXISTS " +
//            "(SELECT LOAD_NO FROM SAPSR3.YMPPT_LOAD_VEHIC WHERE " +
//            "MANDT = C.MANDT AND LOAD_NO = C.LOAD_NO ) ) " +
//            "AND EXISTS " +
//            "(SELECT LOAD_NO FROM SAPSR3.YMPPT_LP_ITEM_C C " +
//            "WHERE C.SALES_ORDER = M.SALES_ORDER " +
//            "AND (C.STATUS = 'A' OR C.STATUS = 'Y') " +
//            "AND LOAD_DATE <= TO_CHAR(SYSDATE + 30, 'YYYYMMDD') " +
//            "AND (NOT EXISTS " +
//            "(SELECT K.VBELN FROM SAPSR3.LIPS P, SAPSR3.LIKP K " +
//            "WHERE P.MANDT = K.MANDT " +
//            "AND P.VBELN = K.VBELN " +
//            "AND P.VGBEL = C.SALES_ORDER " +
//            "AND P.VGPOS = C.CLBD_ITEM " +
//            "AND K.wadat_ist > '00000000') " +
//            "OR EXISTS " +
//            "(SELECT P.VBELN FROM SAPSR3.VBUP P " +
//            "WHERE P.MANDT = C.MANDT " +
//            "AND P.VBELN = C.SALES_ORDER " +
//            "AND P.POSNR = C.CLBD_ITEM " +
//            "AND P.LFSTA = 'B')) " +

//            "AND NOT EXISTS " +
//            "(SELECT LOAD_NO FROM SAPSR3.YMPPT_LOAD_VEHIC WHERE " +
//            "MANDT = C.MANDT AND LOAD_NO = C.LOAD_NO AND LOAD_DATE <= TO_CHAR(SYSDATE, 'YYYYMMDD') ) ) " +
//            //"AND (M.PO_NUMBER like '%" + PONumber + "%' " +
//            //"OR M.PO_NUMBER = ' ' ) " +
//            "AND ((M.CUST_ORDER_DATE >= '" + lPODateFrom_O + "' " +
//            "AND M.CUST_ORDER_DATE <= '" + lPODateTo_O + "') " +
//            "OR M.CUST_ORDER_DATE is NULL  " +
//            "OR M.CUST_ORDER_DATE = ' ' ) " +
//            "AND ((M.REQD_DEL_DATE >= '" + lRDateFrom_O + "' " +
//            "AND M.REQD_DEL_DATE <= '" + lRDateTo_O + "') " +
//            "OR M.REQD_DEL_DATE is NULL  " +
//            "OR M.REQD_DEL_DATE = ' ' ) " +
//            //"AND (D.WBS1 like '%" + WBS1 + "%' " +
//            //"OR (D.WBS1 is null AND '" + WBS1 + "' = ' ' )) " +
//            //"AND (D.WBS2 like '%" + WBS2 + "%' " +
//            //"OR (D.WBS2 is null AND '" + WBS2 + "' = ' ' )) " +
//            //"AND (D.WBS3 like '%" + WBS3 + "%' " +
//            //"OR (D.WBS3 is null AND '" + WBS3 + "' = ' ' )) " +
//            "AND (D.STATUS <> 'X' " +
//            "OR D.STATUS is NULL) " +
//            "ORDER BY 7 ";

//            lOraCmd.CommandText = lSQL;
//            lOraCmd.Connection = lCISCon;
//            lOraCmd.CommandTimeout = 300;
//            lOraRst = (OracleDataReader)await lOraCmd.ExecuteReaderAsync();
//            if (lOraRst.HasRows)
//            {
//                while (lOraRst.Read())
//                {
//                    var lBBSNo = lOraRst.GetValue(18) == DBNull.Value ? "" : lOraRst.GetString(18);
//                    var lBBSDesc = WebUtility.HtmlDecode(lOraRst.GetValue(19) == DBNull.Value ? "" : lOraRst.GetString(19));
//                    var lSORNo = lOraRst.GetString(17).Trim();
//                    int lInd = lReturn.FindIndex(f => f.SONo == lSORNo);
//                    if (lInd < 0)
//                    {
//                        lBalSNo = lBalSNo + 1;

//                        int lConfirmed = 0;

//                        var lPODate = (lOraRst.GetValue(6) == DBNull.Value ? "" : lOraRst.GetString(6));
//                        lPODate = String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lPODate, "yyyyMMdd", CultureInfo.InvariantCulture));

//                        //var lReqDate = (lOraRst.GetValue(8) == DBNull.Value ? "" : lOraRst.GetString(8).Trim());
//                        //if (lReqDate == "")
//                        //{
//                        var lReqDate = (lOraRst.GetValue(7) == DBNull.Value ? "" : lOraRst.GetString(7).Trim());
//                        //} else
//                        //{
//                        //    if (lReqDate.Length > 10)
//                        //    {
//                        //        lReqDate = lReqDate.Substring(0, 10);
//                        //    }
//                        //    lReqDate = String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lReqDate, "yyyy-MM-dd", CultureInfo.InvariantCulture));
//                        //}

//                        //var lReqDateConf = (lOraRst.GetValue(13) == DBNull.Value ? "" : lOraRst.GetString(13).Trim());
//                        //if (lReqDateConf != null && lReqDateConf != "")
//                        //{
//                        //    lReqDate = String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lReqDateConf, "yyyyMMdd", CultureInfo.InvariantCulture));
//                        //}

//                        var lPlanDelDate = (lOraRst.GetValue(15) == DBNull.Value ? "" : lOraRst.GetString(15).Trim());
//                        if (lPlanDelDate != null && lPlanDelDate != "" && lPlanDelDate != "20500101")
//                        {
//                            if (String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lPlanDelDate, "yyyyMMdd", CultureInfo.InvariantCulture))
//                                == (String.Format("{0:yyyy-MM-dd}", DateTime.Now)))
//                            {
//                                lConfirmed = 1;
//                            }
//                            else if (String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lPlanDelDate, "yyyyMMdd", CultureInfo.InvariantCulture))
//                                == (String.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(1))))
//                            {
//                                lConfirmed = 2;
//                            }
//                            else
//                            {
//                                lConfirmed = 3;
//                            }
//                            lPlanDelDate = String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lPlanDelDate, "yyyyMMdd", CultureInfo.InvariantCulture));
//                        }
//                        else
//                        {
//                            lPlanDelDate = "";
//                        }

//                        //var lPlanDelConfDate = (lOraRst.GetValue(16) == DBNull.Value ? "" : lOraRst.GetString(16).Trim());
//                        //if (lPlanDelConfDate != null && lPlanDelConfDate != "")
//                        //{
//                        //    if (String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lPlanDelConfDate, "yyyyMMdd", CultureInfo.InvariantCulture))
//                        //        == (String.Format("{0:yyyy-MM-dd}", DateTime.Now)))
//                        //    {
//                        //        lConfirmed = 1;
//                        //    }
//                        //    else if (String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lPlanDelConfDate, "yyyyMMdd", CultureInfo.InvariantCulture))
//                        //        == (String.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(1))))
//                        //    {
//                        //        lConfirmed = 2;
//                        //    }
//                        //    else
//                        //    {
//                        //        lConfirmed = 3;
//                        //    }
//                        //    //lReqDate = String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lPlanDelConfDate, "yyyyMMdd", CultureInfo.InvariantCulture));
//                        //}

//                        if (lReqDate.Length > 10)
//                        {
//                            lReqDate = lReqDate.Substring(0, 10);
//                        }

//                        var lProdType = (lOraRst.GetValue(4) == DBNull.Value ? "" : lOraRst.GetString(4).Trim());
//                        if (lProdType.Trim() == "")
//                        {
//                            var lProdType2 = (lOraRst.GetValue(12) == DBNull.Value ? "" : lOraRst.GetString(12).Trim());
//                            if (lProdType2 == "MSHSTM")
//                            {
//                                lProdType = "STANDARD-MESH";
//                            }
//                            else if (lProdType2 == "BARDBX" || lProdType2 == "BARRBX")
//                            {
//                                lProdType = "STANDARD-BAR";
//                            }
//                            else if (lProdType2 == "BARDBI")
//                            {
//                                lProdType = "DBIC";
//                            }
//                            else if (lProdType2 == "BARRBI")
//                            {
//                                lProdType = "RBIC";
//                            }
//                            else if (lProdType2 == "COUNSP")
//                            {
//                                lProdType = "COUPLER";
//                            }
//                            else if (lProdType2 == "CRWWPR")
//                            {
//                                lProdType = "Cold Rolled Wire";
//                            }
//                            else if (lProdType2 == "PCSPCS")
//                            {
//                                lProdType = "PC Strand";
//                            }
//                            else if (lProdType2 == "WRDWRD")
//                            {
//                                lProdType = "Wire Rod";
//                            }
//                            else
//                            {
//                                lProdType = lProdType2;
//                            }
//                        }
//                        lReturn.Add(new
//                        {
//                            SSNNo = lBalSNo,
//                            OrderNo = "P" + lBalSNo.ToString(),
//                            WBS1 = (lOraRst.GetValue(0) == DBNull.Value ? "" : lOraRst.GetString(0).Trim()),
//                            WBS2 = (lOraRst.GetValue(1) == DBNull.Value ? "" : lOraRst.GetString(1).Trim()),
//                            WBS3 = (lOraRst.GetValue(2) == DBNull.Value ? "" : lOraRst.GetString(2).Trim()),
//                            StructureElement = lOraRst.GetValue(3) == DBNull.Value ? "" : (lOraRst.GetString(3).Trim() == "NONWBS" ? "" : lOraRst.GetString(3).Trim()),
//                            ProdType = lProdType,
//                            PONo = (lOraRst.GetValue(5) == DBNull.Value ? "" : lOraRst.GetString(5).Trim()),
//                            PODate = lPODate,
//                            RequiredDate = lReqDate,
//                            OrderWeight = (lOraRst.GetValue(9) == DBNull.Value ? "0.000" : lOraRst.GetDecimal(9).ToString("###,###,##0.000;(###,##0.000); ")),
//                            SubmittedBy = (lOraRst.GetValue(10) == DBNull.Value ? "" : lOraRst.GetString(10).Trim()),
//                            OrderStatus = lSORNo.Substring(0, 1) == "1" ? "Reviewed" : "Production",
//                            OrderSource = "SAP",
//                            SONo = lOraRst.GetValue(14) == DBNull.Value ? "" : lOraRst.GetString(14).Trim(),
//                            SORNo = lSORNo,
//                            BBSNo = lBBSNo,
//                            BBSDesc = lBBSDesc,
//                            CustomerCode = (lOraRst.GetValue(20) == DBNull.Value ? "" : lOraRst.GetString(20).Trim()),
//                            ProjectCode = (lOraRst.GetValue(21) == DBNull.Value ? "" : lOraRst.GetString(21).Trim()),
//                            ProjectTitle = (lOraRst.GetValue(22) == DBNull.Value ? "" : lOraRst.GetString(22).Trim()),
//                            DataEnteredBy = "",
//                            Confirmed = lConfirmed,
//                            PlanDeliveryDate = lPlanDelDate,
//                            SubmitBy = (lOraRst.GetValue(18) == DBNull.Value ? "" : lOraRst.GetString(18).Trim())
//                        });

//                    }
//                }
//            }
//            lOraRst.Close();
//            #endregion

//            //Check Production Status
//            var lSTSCon = new SqlConnection();
//            var lSOs = "";
//            var lTagSO = new List<string>();

//            try
//            {
//                if (lProcessObj.OpenSTSConnection(ref lSTSCon) == true)
//                {

//                    #region CAB
//                    if (lReturn.Count > 0)
//                    {
//                        for (int i = 0; i < lReturn.Count; i++)
//                        {
//                            if (lReturn[i].SONo != null && lReturn[i].SONo != "" &&
//                                lReturn[i].OrderSource == "SAP" &&
//                                lReturn[i].ProdType == "CAB")
//                            {
//                                if (lSOs == "")
//                                {
//                                    lSOs = "'" + lReturn[i].SONo + "'";
//                                }
//                                else
//                                {
//                                    lSOs = lSOs + "," + "'" + lReturn[i].SONo + "'";
//                                }
//                            }
//                        }
//                    }

//                    if (lSOs != null && lSOs != "")
//                    {
//                        lSQL = "SELECT ORDER_NO " +
//                        "FROM dbo.tbl_scm_sts_cab_print_tag " +
//                        "WHERE ORDER_NO IN (" + lSOs + ") " +
//                        "GROUP BY ORDER_NO ";

//                        lCmd.CommandText = lSQL;
//                        lCmd.Connection = lSTSCon;
//                        lCmd.CommandTimeout = 300;
//                        lRst = lCmd.ExecuteReader();
//                        if (lRst.HasRows)
//                        {
//                            while (lRst.Read())
//                            {
//                                if (lRst.GetString(0).Trim() != "")
//                                {
//                                    lTagSO.Add(lRst.GetString(0).Trim());
//                                }

//                            }
//                        }
//                        lRst.Close();

//                        for (int i = 0; i < lTagSO.Count; i++)
//                        {
//                            for (int j = 0; j < lReturn.Count; j++)
//                            {
//                                if (lTagSO[i] == lReturn[j].SONo)
//                                {
//                                    lReturn.Insert(j + 1, new
//                                    {
//                                        SSNNo = lReturn[j].SSNNo,
//                                        OrderNo = lReturn[j].OrderNo,
//                                        WBS1 = lReturn[j].WBS1,
//                                        WBS2 = lReturn[j].WBS2,
//                                        WBS3 = lReturn[j].WBS3,
//                                        StructureElement = lReturn[j].StructureElement,
//                                        ProdType = lReturn[j].ProdType,
//                                        PONo = lReturn[j].PONo,
//                                        PODate = lReturn[j].PODate,
//                                        RequiredDate = lReturn[j].RequiredDate,
//                                        OrderWeight = lReturn[j].OrderWeight,
//                                        SubmittedBy = lReturn[j].SubmittedBy,
//                                        OrderStatus = "Production",
//                                        OrderSource = lReturn[j].OrderSource,
//                                        SONo = lReturn[j].SONo,
//                                        SORNo = lReturn[j].SORNo,
//                                        BBSNo = lReturn[j].BBSNo,
//                                        BBSDesc = lReturn[j].BBSDesc,
//                                        CustomerCode = lReturn[j].CustomerCode,
//                                        ProjectCode = lReturn[j].ProjectCode,
//                                        ProjectTitle = lReturn[j].ProjectTitle,
//                                        DataEnteredBy = lReturn[j].DataEnteredBy,
//                                        Confirmed = lReturn[j].Confirmed,
//                                        PlanDeliveryDate = lReturn[j].PlanDeliveryDate,
//                                        SubmitBy = lReturn[j].SubmitBy
//                                    });
//                                    lReturn.RemoveAt(j);
//                                    break;
//                                }
//                            }
//                        }
//                    }
//                    #endregion

//                    #region MESH
//                    lSOs = "";
//                    lTagSO = new List<string>();

//                    if (lReturn.Count > 0)
//                    {
//                        for (int i = 0; i < lReturn.Count; i++)
//                        {
//                            if (lReturn[i].SONo != null && lReturn[i].SONo != "" &&
//                                lReturn[i].OrderSource == "SAP" &&
//                                (lReturn[i].ProdType == "CUT-TO-SIZE-MESH" ||
//                                lReturn[i].ProdType == "STIRRUP-LINK-MESH" ||
//                                lReturn[i].ProdType == "COLUMN-LINK-MESH")
//                                )
//                            {
//                                if (lSOs == "")
//                                {
//                                    lSOs = "'" + lReturn[i].SONo + "'";
//                                }
//                                else
//                                {
//                                    lSOs = lSOs + "," + "'" + lReturn[i].SONo + "'";
//                                }
//                            }
//                        }
//                    }

//                    if (lSOs != null && lSOs != "")
//                    {
//                        lSQL = "SELECT MSD_PRT_SO_NO " +
//                        "FROM dbo.tbl_scm_sts_mesh_print_tag " +
//                        "WHERE MSD_PRT_SO_NO IN (" + lSOs + ") " +
//                        "GROUP BY MSD_PRT_SO_NO ";

//                        lCmd.CommandText = lSQL;
//                        lCmd.Connection = lSTSCon;
//                        lCmd.CommandTimeout = 300;
//                        lRst = lCmd.ExecuteReader();
//                        if (lRst.HasRows)
//                        {
//                            while (lRst.Read())
//                            {
//                                if (lRst.GetString(0).Trim() != "")
//                                {
//                                    lTagSO.Add(lRst.GetString(0).Trim());
//                                }

//                            }
//                        }
//                        lRst.Close();

//                        for (int i = 0; i < lTagSO.Count; i++)
//                        {
//                            for (int j = 0; j < lReturn.Count; j++)
//                            {
//                                if (lTagSO[i] == lReturn[j].SONo)
//                                {
//                                    lReturn.Insert(j + 1, new
//                                    {
//                                        SSNNo = lReturn[j].SSNNo,
//                                        OrderNo = lReturn[j].OrderNo,
//                                        WBS1 = lReturn[j].WBS1,
//                                        WBS2 = lReturn[j].WBS2,
//                                        WBS3 = lReturn[j].WBS3,
//                                        StructureElement = lReturn[j].StructureElement,
//                                        ProdType = lReturn[j].ProdType,
//                                        PONo = lReturn[j].PONo,
//                                        PODate = lReturn[j].PODate,
//                                        RequiredDate = lReturn[j].RequiredDate,
//                                        OrderWeight = lReturn[j].OrderWeight,
//                                        SubmittedBy = lReturn[j].SubmittedBy,
//                                        OrderStatus = "Production",
//                                        OrderSource = lReturn[j].OrderSource,
//                                        SONo = lReturn[j].SONo,
//                                        SORNo = lReturn[j].SORNo,
//                                        BBSNo = lReturn[j].BBSNo,
//                                        BBSDesc = lReturn[j].BBSDesc,
//                                        CustomerCode = lReturn[j].CustomerCode,
//                                        ProjectCode = lReturn[j].ProjectCode,
//                                        ProjectTitle = lReturn[j].ProjectTitle,
//                                        DataEnteredBy = lReturn[j].DataEnteredBy,
//                                        Confirmed = lReturn[j].Confirmed,
//                                        PlanDeliveryDate = lReturn[j].PlanDeliveryDate,
//                                        SubmitBy = lReturn[j].SubmitBy
//                                    });
//                                    lReturn.RemoveAt(j);
//                                    break;
//                                }

//                            }
//                        }
//                    }
//                    #endregion

//                    #region PRE-CAGE
//                    lSOs = "";
//                    lTagSO = new List<string>();

//                    if (lReturn.Count > 0)
//                    {
//                        for (int i = 0; i < lReturn.Count; i++)
//                        {
//                            if (lReturn[i].SONo != null && lReturn[i].SONo != "" &&
//                                lReturn[i].OrderSource == "SAP" &&
//                                (lReturn[i].ProdType == "CORE-CAGE" ||
//                                lReturn[i].ProdType == "PRE-CAGE")
//                                )
//                            {
//                                if (lSOs == "")
//                                {
//                                    lSOs = "'" + lReturn[i].SONo + "'";
//                                }
//                                else
//                                {
//                                    lSOs = lSOs + "," + "'" + lReturn[i].SONo + "'";
//                                }
//                            }
//                        }
//                    }

//                    if (lSOs != null && lSOs != "")
//                    {
//                        lSQL = "SELECT SO_NO " +
//                        "FROM dbo.tbl_scm_sts_prc_print_tag " +
//                        "WHERE SO_NO IN (" + lSOs + ") " +
//                        "GROUP BY SO_NO ";

//                        lCmd.CommandText = lSQL;
//                        lCmd.Connection = lSTSCon;
//                        lCmd.CommandTimeout = 300;
//                        lRst = lCmd.ExecuteReader();
//                        if (lRst.HasRows)
//                        {
//                            while (lRst.Read())
//                            {
//                                if (lRst.GetString(0).Trim() != "")
//                                {
//                                    lTagSO.Add(lRst.GetString(0).Trim());
//                                }

//                            }
//                        }
//                        lRst.Close();

//                        for (int i = 0; i < lTagSO.Count; i++)
//                        {
//                            for (int j = 0; j < lReturn.Count; j++)
//                            {
//                                if (lTagSO[i] == lReturn[j].SONo)
//                                {
//                                    lReturn.Insert(j + 1, new
//                                    {
//                                        SSNNo = lReturn[j].SSNNo,
//                                        OrderNo = lReturn[j].OrderNo,
//                                        WBS1 = lReturn[j].WBS1,
//                                        WBS2 = lReturn[j].WBS2,
//                                        WBS3 = lReturn[j].WBS3,
//                                        StructureElement = lReturn[j].StructureElement,
//                                        ProdType = lReturn[j].ProdType,
//                                        PONo = lReturn[j].PONo,
//                                        PODate = lReturn[j].PODate,
//                                        RequiredDate = lReturn[j].RequiredDate,
//                                        OrderWeight = lReturn[j].OrderWeight,
//                                        SubmittedBy = lReturn[j].SubmittedBy,
//                                        OrderStatus = "Production",
//                                        OrderSource = lReturn[j].OrderSource,
//                                        SONo = lReturn[j].SONo,
//                                        SORNo = lReturn[j].SORNo,
//                                        BBSNo = lReturn[j].BBSNo,
//                                        BBSDesc = lReturn[j].BBSDesc,
//                                        CustomerCode = lReturn[j].CustomerCode,
//                                        ProjectCode = lReturn[j].ProjectCode,
//                                        ProjectTitle = lReturn[j].ProjectTitle,
//                                        DataEnteredBy = lReturn[j].DataEnteredBy,
//                                        Confirmed = lReturn[j].Confirmed,
//                                        PlanDeliveryDate = lReturn[j].PlanDeliveryDate,
//                                        SubmitBy = lReturn[j].SubmitBy
//                                    });
//                                    lReturn.RemoveAt(j);
//                                    break;
//                                }

//                            }
//                        }
//                    }
//                    #endregion

//                    #region BPC
//                    lSOs = "";
//                    lTagSO = new List<string>();

//                    if (lReturn.Count > 0)
//                    {
//                        for (int i = 0; i < lReturn.Count; i++)
//                        {
//                            if (lReturn[i].SONo != null && lReturn[i].SONo != "" &&
//                                lReturn[i].OrderSource == "SAP" &&
//                                lReturn[i].ProdType == "BPC")
//                            {

//                                if (lSOs == "")
//                                {
//                                    lSOs = "'" + lReturn[i].SONo + "'";
//                                }
//                                else
//                                {
//                                    lSOs = lSOs + "," + "'" + lReturn[i].SONo + "'";
//                                }
//                            }
//                        }
//                    }

//                    if (lSOs != null && lSOs != "")
//                    {
//                        lSQL = "SELECT fld_bpc_so_no " +
//                        "FROM dbo.tbl_scm_sts_bpc_print_tag " +
//                        "WHERE fld_bpc_so_no IN (" + lSOs + ") " +
//                        "GROUP BY fld_bpc_so_no ";

//                        lCmd.CommandText = lSQL;
//                        lCmd.Connection = lSTSCon;
//                        lCmd.CommandTimeout = 300;
//                        lRst = lCmd.ExecuteReader();
//                        if (lRst.HasRows)
//                        {
//                            while (lRst.Read())
//                            {
//                                if (lRst.GetString(0).Trim() != "")
//                                {
//                                    lTagSO.Add(lRst.GetString(0).Trim());
//                                }

//                            }
//                        }
//                        lRst.Close();

//                        for (int i = 0; i < lTagSO.Count; i++)
//                        {
//                            for (int j = 0; j < lReturn.Count; j++)
//                            {
//                                if (lTagSO[i] == lReturn[j].SONo)
//                                {
//                                    lReturn.Insert(j + 1, new
//                                    {
//                                        SSNNo = lReturn[j].SSNNo,
//                                        OrderNo = lReturn[j].OrderNo,
//                                        WBS1 = lReturn[j].WBS1,
//                                        WBS2 = lReturn[j].WBS2,
//                                        WBS3 = lReturn[j].WBS3,
//                                        StructureElement = lReturn[j].StructureElement,
//                                        ProdType = lReturn[j].ProdType,
//                                        PONo = lReturn[j].PONo,
//                                        PODate = lReturn[j].PODate,
//                                        RequiredDate = lReturn[j].RequiredDate,
//                                        OrderWeight = lReturn[j].OrderWeight,
//                                        SubmittedBy = lReturn[j].SubmittedBy,
//                                        OrderStatus = "Production",
//                                        OrderSource = lReturn[j].OrderSource,
//                                        SONo = lReturn[j].SONo,
//                                        SORNo = lReturn[j].SORNo,
//                                        BBSNo = lReturn[j].BBSNo,
//                                        BBSDesc = lReturn[j].BBSDesc,
//                                        CustomerCode = lReturn[j].CustomerCode,
//                                        ProjectCode = lReturn[j].ProjectCode,
//                                        ProjectTitle = lReturn[j].ProjectTitle,
//                                        DataEnteredBy = lReturn[j].DataEnteredBy,
//                                        Confirmed = lReturn[j].Confirmed,
//                                        PlanDeliveryDate = lReturn[j].PlanDeliveryDate,
//                                        SubmitBy = lReturn[j].SubmitBy
//                                    });
//                                    lReturn.RemoveAt(j);
//                                    break;
//                                }

//                            }
//                        }
//                    }
//                    #endregion

//                    #region CARPET
//                    lSOs = "";
//                    lTagSO = new List<string>();

//                    if (lReturn.Count > 0)
//                    {
//                        for (int i = 0; i < lReturn.Count; i++)
//                        {
//                            if (lReturn[i].SONo != null && lReturn[i].SONo != "" &&
//                                lReturn[i].OrderSource == "SAP" &&
//                                lReturn[i].ProdType == "CARPET")
//                            {
//                                if (lSOs == "")
//                                {
//                                    lSOs = "'" + lReturn[i].SONo + "'";
//                                }
//                                else
//                                {
//                                    lSOs = lSOs + "," + "'" + lReturn[i].SONo + "'";
//                                }
//                            }
//                        }
//                    }

//                    if (lSOs != null && lSOs != "")
//                    {
//                        lSQL = "SELECT SO_NO " +
//                        "FROM dbo.tbl_scm_sts_carpet_print_tag " +
//                        "WHERE SO_NO IN (" + lSOs + ") " +
//                        "GROUP BY SO_NO ";

//                        lCmd.CommandText = lSQL;
//                        lCmd.Connection = lSTSCon;
//                        lCmd.CommandTimeout = 300;
//                        lRst = lCmd.ExecuteReader();
//                        if (lRst.HasRows)
//                        {
//                            while (lRst.Read())
//                            {
//                                if (lRst.GetString(0).Trim() != "")
//                                {
//                                    lTagSO.Add(lRst.GetString(0).Trim());
//                                }

//                            }
//                        }
//                        lRst.Close();

//                        for (int i = 0; i < lTagSO.Count; i++)
//                        {
//                            for (int j = 0; j < lReturn.Count; j++)
//                            {
//                                if (lTagSO[i] == lReturn[j].SONo)
//                                {
//                                    lReturn.Insert(j + 1, new
//                                    {
//                                        SSNNo = lReturn[j].SSNNo,
//                                        OrderNo = lReturn[j].OrderNo,
//                                        WBS1 = lReturn[j].WBS1,
//                                        WBS2 = lReturn[j].WBS2,
//                                        WBS3 = lReturn[j].WBS3,
//                                        StructureElement = lReturn[j].StructureElement,
//                                        ProdType = lReturn[j].ProdType,
//                                        PONo = lReturn[j].PONo,
//                                        PODate = lReturn[j].PODate,
//                                        RequiredDate = lReturn[j].RequiredDate,
//                                        OrderWeight = lReturn[j].OrderWeight,
//                                        SubmittedBy = lReturn[j].SubmittedBy,
//                                        OrderStatus = "Production",
//                                        OrderSource = lReturn[j].OrderSource,
//                                        SONo = lReturn[j].SONo,
//                                        SORNo = lReturn[j].SORNo,
//                                        BBSNo = lReturn[j].BBSNo,
//                                        BBSDesc = lReturn[j].BBSDesc,
//                                        CustomerCode = lReturn[j].CustomerCode,
//                                        ProjectCode = lReturn[j].ProjectCode,
//                                        ProjectTitle = lReturn[j].ProjectTitle,
//                                        DataEnteredBy = lReturn[j].DataEnteredBy,
//                                        Confirmed = lReturn[j].Confirmed,
//                                        PlanDeliveryDate = lReturn[j].PlanDeliveryDate,
//                                        SubmitBy = lReturn[j].SubmitBy
//                                    });
//                                    lReturn.RemoveAt(j);
//                                    break;
//                                }

//                            }
//                        }
//                    }
//                    #endregion

//                    lProcessObj.CloseSTSConnection(ref lSTSCon);
//                }
//            }
//            catch (Exception ex)
//            {
//                lProcessObj.SaveErrorMsg(ex.Message, ex.StackTrace);
//                string lerrorMsg = ex.Message;
//            }


//            lProcessObj.CloseCISConnection(ref lCISCon);

//            #region Check Plan Delivery Date From IDB
//            if (lReturn.Count > 0)
//            {
//                var lSONos = "";
//                for (int i = 0; i < lReturn.Count; i++)
//                {
//                    var lSOARR = lReturn[i].SORNo.Split(',');
//                    if (lSOARR.Length > 0 && (lReturn[i].PlanDeliveryDate == null || lReturn[i].PlanDeliveryDate == ""))
//                        for (int j = 0; j < lSOARR.Length; j++)
//                        {
//                            if (lSOARR[j].Trim().Length > 0)
//                            {
//                                if (lSONos == "")
//                                {
//                                    lSONos = "'" + lSOARR[j].Trim() + "'";
//                                }
//                                else
//                                {
//                                    lSONos = lSONos + ",'" + lSOARR[j].Trim() + "'";
//                                }
//                            }
//                        }
//                }
//                if (lSONos != "")
//                {
//                    try
//                    {
//                        lProcessObj.OpenIDBConnection(ref lCISCon);

//                        var lSORList = new List<string>();

//                        var lSORA = lSONos.Split(',').ToList();
//                        var lSOR1 = "";
//                        if (lSORA.Count > 0)
//                        {
//                            int lCount = 0;
//                            for (int i = 0; i < lSORA.Count; i++)
//                            {
//                                if (lSOR1 == "")
//                                {
//                                    //lSOR1 = "'" + lSORA[i] + "'";
//                                    lSOR1 = lSORA[i];
//                                }
//                                else
//                                {
//                                    //lSOR1 = lSOR1 + "," + "'" + lSORA[i] + "'";
//                                    lSOR1 = lSOR1 + "," + lSORA[i];
//                                }
//                                lCount = lCount + 1;
//                                if (lCount > 300)
//                                {
//                                    lSORList.Add(lSOR1);
//                                    lSOR1 = "";
//                                    lCount = 0;
//                                }
//                            }
//                            if (lSOR1 != "")
//                            {
//                                lSORList.Add(lSOR1);
//                            }
//                        }

//                        for (int k = 0; k < lSORList.Count; k++)
//                        {
//                            if (lSORList[k] != null && lSORList[k] != "" && lSORList[k] != " " && lSORList[k] != "''" && lSORList[k] != "' '")
//                            {
//                                // Plan Delivery date

//                                var lDelSOR = "";
//                                if (lReturn.Count > 0)
//                                {
//                                    for (int j = 0; j < lReturn.Count; j++)
//                                    {
//                                        if (lReturn[j].PlanDeliveryDate != null && lReturn[j].PlanDeliveryDate.Trim() != "")
//                                        {
//                                            var lSOAR = lReturn[j].SORNo.Split(',');
//                                            if (lSOAR.Length > 0)
//                                            {
//                                                for (int m = 0; m < lSOAR.Length; m++)
//                                                {
//                                                    lDelSOR = lDelSOR + ",'" + lSOAR[m] + "'";
//                                                }
//                                            }
//                                        }
//                                    }
//                                }
//                                if (lDelSOR.Length > 0)
//                                {
//                                    lDelSOR = lDelSOR.Substring(1);
//                                }

//                                if (lDelSOR.Length > 0)
//                                {
//                                    lSQL = "SELECT A.ORDER_REQUEST_NO, Max(A.CONFIRMED_DATE), H.BBS " +      //PLANNED_DATE
//                                    "FROM SALES_ORDER_PLANNING A, ORDER_HEADER H " +
//                                    "WHERE A.ORDER_REQUEST_NO = H.ORDER_REQUEST_NO " +
//                                    "AND A.ORDER_REQUEST_NO IN (" + lSORList[k] + ") " +
//                                    "AND not exists (select ORDER_REQUEST_NO " +
//                                    "FROM SALES_ORDER_PLANNING " +
//                                    "WHERE ORDER_REQUEST_NO = A.ORDER_REQUEST_NO " +
//                                    "AND ORDER_REQUEST_NO IN (" + lDelSOR + ") ) " +
//                                    "GROUP BY A.ORDER_REQUEST_NO, H.BBS ";
//                                }
//                                else
//                                {
//                                    lSQL = "SELECT A.ORDER_REQUEST_NO, Max(A.CONFIRMED_DATE), H.BBS " +      //PLANNED_DATE
//                                    "FROM SALES_ORDER_PLANNING A, ORDER_HEADER H " +
//                                    "WHERE A.ORDER_REQUEST_NO = H.ORDER_REQUEST_NO " +
//                                    "AND A.ORDER_REQUEST_NO IN (" + lSORList[k] + ") " +
//                                    "GROUP BY A.ORDER_REQUEST_NO, H.BBS ";
//                                }

//                                lOraCmd.CommandText = lSQL;
//                                lOraCmd.Connection = lCISCon;
//                                lOraCmd.CommandTimeout = 300;
//                                lOraRst = (OracleDataReader)await lOraCmd.ExecuteReaderAsync();
//                                if (lOraRst.HasRows)
//                                {
//                                    while (lOraRst.Read())
//                                    {
//                                        var lOrderNo = lOraRst.GetValue(0) == DBNull.Value ? "" : lOraRst.GetString(0).Trim();
//                                        var lPlanDate = (DateTime?)lOraRst.GetValue(1);
//                                        var lBBSNo = lOraRst.GetValue(2) == DBNull.Value ? "" : lOraRst.GetString(2).Trim();
//                                        if (lPlanDate != null)
//                                        {
//                                            if ((DateTime)lPlanDate > DateTime.Now.AddYears(1))
//                                            {
//                                                lPlanDate = null;
//                                            }
//                                        }
//                                        if (lOrderNo != "")
//                                        {
//                                            var lConfirmed = 1;

//                                            if (lPlanDate != null)
//                                            {
//                                                if (String.Format("{0:yyyy-MM-dd}", lPlanDate)
//                                                    == String.Format("{0:yyyy-MM-dd}", DateTime.Now))
//                                                {
//                                                    lConfirmed = 1;
//                                                }
//                                                else if (String.Format("{0:yyyy-MM-dd}", lPlanDate)
//                                                    == (String.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(1))))
//                                                {
//                                                    lConfirmed = 2;
//                                                }
//                                                else
//                                                {
//                                                    lConfirmed = 3;
//                                                }
//                                            }

//                                            var lPlanDateStr = lPlanDate == null ? "" : ((DateTime)lPlanDate).ToString("yyyy-MM-dd");
//                                            for (int j = 0; j < lReturn.Count; j++)
//                                            {
//                                                if (lReturn[j].SORNo.IndexOf(lOrderNo) >= 0)
//                                                {
//                                                    //added for specify the product
//                                                    var lPlanDelDate = lPlanDateStr;
//                                                    var lDeliveryDate = lReturn[j].PlanDeliveryDate;

//                                                    if (lDeliveryDate == null)
//                                                    {
//                                                        lDeliveryDate = "";
//                                                    }

//                                                    if (lPlanDelDate != null && lPlanDelDate != "")
//                                                    {
//                                                        if (lDeliveryDate.Trim() == "")
//                                                        {
//                                                            if (lOrderNo.Substring(0, 3) == "103")
//                                                            {
//                                                                if (lReturn[j].SONo.LastIndexOf(",") > 0 && (lReturn[j].SONo.Substring(0, 1) == "8" || lReturn[j].SONo.IndexOf(",8") >= 0))
//                                                                {
//                                                                    lDeliveryDate = lPlanDelDate + "(SB)";
//                                                                }
//                                                                else
//                                                                {
//                                                                    lDeliveryDate = lPlanDelDate;
//                                                                }
//                                                            }
//                                                            else if (lReturn[j].SONo.LastIndexOf(",") > 0 && (lReturn[j].SONo.Substring(0, 3) == "103" || lReturn[j].SONo.IndexOf(",103") > 0))
//                                                            {
//                                                                if (lBBSNo.IndexOf("-COUPLER") > 0 || lBBSNo.IndexOf("-CP") > 0)
//                                                                {
//                                                                    lDeliveryDate = lPlanDelDate + "(CP)";
//                                                                }
//                                                                else
//                                                                {
//                                                                    lDeliveryDate = lPlanDelDate + "(CAB)";
//                                                                }
//                                                            }
//                                                            else
//                                                            {
//                                                                if (lReturn[j].SONo.LastIndexOf(",") > 0 && (lReturn[j].BBSNo.IndexOf("-COUPLER") > 0 || lReturn[j].BBSNo.IndexOf("-CP") > 0))
//                                                                {
//                                                                    if (lBBSNo.IndexOf("-COUPLER") > 0 || lBBSNo.IndexOf("-CP") > 0)
//                                                                    {
//                                                                        lDeliveryDate = lPlanDelDate + "(CP)";
//                                                                    }
//                                                                    else
//                                                                    {
//                                                                        lDeliveryDate = lPlanDelDate + "(CAB)";
//                                                                    }
//                                                                }
//                                                                else
//                                                                {
//                                                                    lDeliveryDate = lPlanDelDate;
//                                                                }
//                                                            }
//                                                        }
//                                                        else
//                                                        {
//                                                            if (lDeliveryDate.IndexOf(lPlanDelDate) < 0)
//                                                            {
//                                                                if (lOrderNo.Substring(0, 3) == "103")
//                                                                {
//                                                                    if (lReturn[j].SONo.LastIndexOf(",") > 0 && (lReturn[j].SONo.Substring(0, 1) == "8" || lReturn[j].SONo.IndexOf(",8") >= 0))
//                                                                    {
//                                                                        lDeliveryDate = lDeliveryDate + "," + lPlanDelDate + "(SB)";
//                                                                    }
//                                                                    else
//                                                                    {
//                                                                        lDeliveryDate = lDeliveryDate + "," + lPlanDelDate;
//                                                                    }
//                                                                }
//                                                                else if (lReturn[j].SONo.LastIndexOf(",") > 0 && (lReturn[j].SONo.Substring(0, 3) == "103" || lReturn[j].SONo.IndexOf(",103") > 0))
//                                                                {
//                                                                    if (lBBSNo.IndexOf("-COUPLER") > 0 || lBBSNo.IndexOf("-CP") > 0)
//                                                                    {
//                                                                        lDeliveryDate = lDeliveryDate + "," + lPlanDelDate + "(CP)";
//                                                                    }
//                                                                    else
//                                                                    {
//                                                                        lDeliveryDate = lDeliveryDate + "," + lPlanDelDate + "(CAB)";
//                                                                    }
//                                                                }
//                                                                else
//                                                                {
//                                                                    if (lReturn[j].SONo.LastIndexOf(",") > 0 && (lReturn[j].BBSNo.IndexOf("-COUPLER") > 0 || lReturn[j].BBSNo.IndexOf("-CP") > 0))
//                                                                    {
//                                                                        if (lBBSNo.IndexOf("-COUPLER") > 0 || lBBSNo.IndexOf("-CP") > 0)
//                                                                        {
//                                                                            lDeliveryDate = lDeliveryDate + "," + lPlanDelDate + "(CP)";
//                                                                        }
//                                                                        else
//                                                                        {
//                                                                            lDeliveryDate = lDeliveryDate + "," + lPlanDelDate + "(CAB)";
//                                                                        }
//                                                                    }
//                                                                    else
//                                                                    {
//                                                                        lDeliveryDate = lDeliveryDate + "," + lPlanDelDate;
//                                                                    }
//                                                                }
//                                                            }
//                                                            else
//                                                            {
//                                                                if (lDeliveryDate.IndexOf(",") < 0 && lDeliveryDate.IndexOf("(") > 0)
//                                                                {
//                                                                    lDeliveryDate = lDeliveryDate.Substring(0, lDeliveryDate.IndexOf("("));
//                                                                }
//                                                            }
//                                                        }
//                                                    }
//                                                    //PlanDeliveryDate = (lReturn[j].PlanDeliveryDate == null || lReturn[j].PlanDeliveryDate.Trim() == "") ? lPlanDateStr : ((lPlanDateStr != null && lPlanDateStr.Trim() != "" && lReturn[j].PlanDeliveryDate.IndexOf(lPlanDateStr) < 0) ? (lReturn[j].PlanDeliveryDate + "," + lPlanDateStr) : lReturn[j].PlanDeliveryDate)
//                                                    //end

//                                                    lReturn[j] = new
//                                                    {
//                                                        SSNNo = lReturn[j].SSNNo,
//                                                        OrderNo = lReturn[j].OrderNo,
//                                                        WBS1 = lReturn[j].WBS1,
//                                                        WBS2 = lReturn[j].WBS2,
//                                                        WBS3 = lReturn[j].WBS3,
//                                                        StructureElement = lReturn[j].StructureElement,
//                                                        ProdType = lReturn[j].ProdType,
//                                                        PONo = lReturn[j].PONo,
//                                                        PODate = lReturn[j].PODate,
//                                                        RequiredDate = lReturn[j].RequiredDate,
//                                                        OrderWeight = lReturn[j].OrderWeight,
//                                                        SubmittedBy = lReturn[j].SubmittedBy,
//                                                        OrderStatus = lReturn[j].OrderStatus,
//                                                        OrderSource = lReturn[j].OrderSource,
//                                                        SONo = lReturn[j].SONo,
//                                                        SORNo = lReturn[j].SORNo,
//                                                        BBSNo = lReturn[j].BBSNo,
//                                                        BBSDesc = lReturn[j].BBSDesc,
//                                                        CustomerCode = lReturn[j].CustomerCode,
//                                                        ProjectCode = lReturn[j].ProjectCode,
//                                                        ProjectTitle = lReturn[j].ProjectTitle,
//                                                        DataEnteredBy = lReturn[j].DataEnteredBy,
//                                                        Confirmed = lReturn[j].Confirmed == 1 ? 1 : (lConfirmed == 1 ? 1 : (lReturn[j].Confirmed == 2 ? 2 : lConfirmed)),
//                                                        PlanDeliveryDate = lDeliveryDate,
//                                                        SubmitBy = lReturn[j].SubmitBy
//                                                    };

//                                                    break;
//                                                }
//                                            }

//                                        }
//                                    }
//                                }

//                                lOraRst.Close();

//                            }
//                        }

//                        lProcessObj.CloseIDBConnection(ref lCISCon);
//                    }
//                    catch (Exception ex)
//                    {

//                    }
//                }
//            }

//            #endregion

//            lProcessObj = null;

//            #endregion
//        }
//    }
//    lCmd = null;
//    lNDSCon = null;
//    lRst = null;

//    lOraCmd = null;
//    lCISCon = null;
//    lOraRst = null;


//    lReturn = (from p in lReturn
//               orderby p.RequiredDate descending
//               select p).ToList();

//    // put the Pending Approval first


//    if (lReturn.Count > 0)
//    {
//        var lPendingA = lReturn.ToArray();

//        lReturn.CopyTo(lPendingA);

//        var lPendingAppr = lPendingA.ToList();

//        lPendingAppr.Clear();

//        for (int i = lReturn.Count - 1; i >= 0; i--)
//        {
//            if (lReturn[i].OrderStatus == "Pending Approval")
//            {
//                lPendingAppr.Add(lReturn[i]);
//                lReturn.RemoveAt(i);
//            }

//        }

//        if (lPendingAppr.Count > 0)
//        {
//            for (int i = 0; i < lPendingAppr.Count; i++)
//            {
//                lReturn.Insert(0, lPendingAppr[i]);
//            }
//        }
//    }


//    // return Json(lReturn, System.Web.Mvc.JsonRequestBehavior.AllowGet);

//    return Ok(lReturn);
//}


[HttpGet]
[Route("/ActiveOrderPopup/{PoNumber}")]
[AllowAnonymous]
public IActionResult GetCombinedData(string PoNumber)
{
    var sors = new List<string>();
    var result = new List<object>();

    try
    {
        // Step 1: Fetch SORs from SQL Server
        var lCmd = new SqlCommand();
        var lNDSCon = new SqlConnection();
        SqlDataReader lRst;

        lCmd.CommandText =
        "SELECT p.ordernumber, s.producttype AS prodtype, s.ponumber, p.customercode, p.projectcode, " +
        "CASE WHEN S.ProductType = 'BPC' AND S.ScheduledProd <> 'Y' THEN " +
        "isNull(STUFF((SELECT ',' + sor_no FROM dbo.OESBPCDetailsProc " +
        "WHERE CustomerCode = p.CustomerCode AND ProjectCode = p.ProjectCode AND JobID = S.BPCJobID " +
        "GROUP BY sor_no ORDER BY sor_no FOR XML PATH('')), 1, 1, ''),'') " +
        "WHEN S.ProductType = 'CAB' AND S.ScheduledProd <> 'Y' THEN " +
        "isNull(STUFF((SELECT CASE WHEN BBSSOR <> '' THEN ',' + BBSSOR ELSE '' END + " +
        "CASE WHEN BBSSAPSO <> '' THEN ',' + BBSSAPSO ELSE '' END + " +
        "CASE WHEN BBSSORCoupler <> '' THEN ',' + BBSSORCoupler ELSE '' END " +
        "FROM dbo.OESBBS " +
        "WHERE CustomerCode = p.CustomerCode AND ProjectCode = p.ProjectCode AND JobID = S.CABJobID " +
        "GROUP BY BBSSOR, BBSSAPSO, BBSSORCoupler ORDER BY BBSSOR, BBSSAPSO, BBSSORCoupler FOR XML PATH('')), 1, 1, ''),'') " +
        "ELSE S.SAPSOR END AS SOR " +
        "FROM dbo.OESProjOrdersSE S, dbo.OESProjOrder P " +
        "WHERE s.ordernumber = p.ordernumber " +
        "AND ((p.orderstatus IN ('Submitted', 'Created*', 'Submitted*', 'Cancelled', 'Processed', 'Reviewed', 'Production', 'Delivered')) " +
        "AND (s.ponumber = @PoNumber) AND S.ProductType IN ('BPC', 'PRC'))";
        lCmd.CommandText = "SELECT ORDER_REQUEST_NO as SOR FROM OesOrderHeaderHMI where PO_NUMBER=@PoNumber AND STATUS<>'X'";
        lCmd.Parameters.AddWithValue("@PoNumber", PoNumber);

        var lProcessObj = new ProcessController();
        if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
        {
            lCmd.Connection = lNDSCon;
            lCmd.CommandTimeout = 300;
            lRst = lCmd.ExecuteReader();

            while (lRst.Read())
            {
                var sor = lRst["SOR"]?.ToString()?.Trim();
                if (!string.IsNullOrEmpty(sor))
                {
                    sors.AddRange(sor.Split(',')); // Collect SORs
                }
            }

            lRst.Close();
            lProcessObj.CloseNDSConnection(ref lNDSCon);
        }

        if (sors.Count == 0)
        {
            return Ok(new { status = "NoSORsFound", message = "No SORs found for the selected PO number." });
        }

        // Step 2: Pass SORs to Query 2
        //var lCmd2 = new SqlCommand();
        // var lNDSCon = new SqlConnection();

        string sorList = string.Join("','", sors.Distinct().Select(s => s.Replace("'", "''")));
        sorList = $"'{sorList}'";
        lCmd.CommandText = $@"
  SELECT
  rdtl.bbs_no AS bbsno,
  rdtl.wbs1 + '/ ' + rdtl.wbs2 + ' / ' + rdtl.wbs3 AS WBS,


  ROUND(
    (SELECT ISNULL(SUM(CAST(i.THEO_WGT AS FLOAT)), 0) / 1000
     FROM OesOrderItemHMI i
     WHERE i.ORDER_NO = h.SALES_ORDER), 2
  ) AS total_mt_sap,

  CAST((
    SELECT FORMAT(
      ISNULL(SUM(CAST(ORDER_PCS AS FLOAT)), 0) / 1000, 
      '#,##0'
    )
    FROM OesOrderItemHMI i
    WHERE i.ORDER_NO = h.SALES_ORDER
  ) AS INT) AS totalpcs_sapy,

  h.Vehicle_type,ld.LOAD_NO
FROM OesOrderHeaderHMI h
LEFT JOIN OesRequestDetailsHMI rdtl 
  ON rdtl.order_request_no = h.order_request_no
 left join HMILoadDetails ld on ld.SALES_ORDER=h.SALES_ORDER
WHERE h.order_request_no IN ({sorList});
";

        if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
        {
            lCmd.Connection = lNDSCon;
            lCmd.CommandTimeout = 300;
            var lRst2 = lCmd.ExecuteReader();

            while (lRst2.Read())
            {
                result.Add(new
                {
                    BBSNo = lRst2["bbsno"]?.ToString()?.Trim(),
                    WBS = lRst2["WBS"]?.ToString()?.Trim(),
                    TotalMtSAP = lRst2["total_mt_sap"]?.ToString()?.Trim(),
                    TotalPcsSAP = lRst2["totalpcs_sapy"]?.ToString()?.Trim(),
                    Vehicle_type = lRst2["Vehicle_type"]?.ToString()?.Trim(),
                    LOAD_NO = lRst2["LOAD_NO"]?.ToString()?.Trim()
                });
            }

            lRst2.Close();
            lProcessObj.CloseNDSConnection(ref lNDSCon);
        }

        if (result.Count == 0)
        {
            return Ok(new { status = "NoDetailsFound", message = "SORs retrieved, but no detail records were found in Oracle." });
        }


        return Ok(new { status = "Success", data = result });
    }
    catch (Exception ex)
    {
        return StatusCode(500, $"Server Error: {ex.Message}");
    }
}


        //[HttpGet]
        //[Route("/TesFun")]
        //public IActionResult TesFun()
        //{
        //    var lNDSCon = new SqlConnection();
        //    string lSQL = "";
        //    var lCmd = new SqlCommand();
        //    SqlDataReader lRst;
        //    var LReturn

        //    var lProcessObj1 = new ProcessController();
        //    if (lProcessObj1.OpenNDSConnection(ref lNDSCon) == true)
        //    {
        //        for (int i = 0; i < lReturn.Count; i++)
        //        {
        //            if (Convert.ToDecimal(lReturn[i].OrderWeight) == 0)
        //            {
        //                decimal lWtObj = 0;
        //                var lQtyObj = 0;
        //                lSQL = "SELECT numPostedWeight,intPostedQty FROM BBSPostHeader WHERE INTPostHeaderID in" +
        //                                    " (select PostHeaderID from OESProjOrdersSE where OrderNumber='" + lReturn[i].OrderNo + "') ";

        //                lCmd.CommandText = lSQL;
        //                lCmd.Connection = lNDSCon;
        //                lCmd.CommandTimeout = 300;
        //                lRst = lCmd.ExecuteReader();
        //                if (lRst.HasRows)
        //                {
        //                    while (lRst.Read())
        //                    {
        //                        lWtObj = lRst.GetValue(0) == DBNull.Value ? 0 : lRst.GetInt32(0);
        //                        lQtyObj = lRst.GetValue(1) == DBNull.Value ? 0 : lRst.GetInt32(1);

        //                    }

        //                }
        //                lRst.Close();
        //                if (lWtObj != 0)
        //                {
        //                    lSQL = "update dbo.OESProjOrders SET TotalWeight=('" + lWtObj + "'*1000)   where OrderNumber='" + lReturn[i].OrderNo + "'";
        //                    lCmd.CommandText = lSQL;
        //                    lCmd.Connection = lNDSCon;
        //                    lCmd.CommandTimeout = 300;
        //                    lCmd.ExecuteNonQuery();

        //                    lSQL = "update dbo.OESProjOrdersSE SET TotalWeight=('" + lWtObj + "'*1000), TotalPCs='" + lQtyObj + "'   where OrderNumber='" + lReturn[i].OrderNo + "'";
        //                    lCmd.CommandText = lSQL;
        //                    lCmd.Connection = lNDSCon;
        //                    lCmd.CommandTimeout = 300;
        //                    lCmd.ExecuteNonQuery();
        //                }
        //            }
        //        }
        //        lProcessObj1.CloseNDSConnection(ref lNDSCon);
        //    }
        //    return Ok();


        //}

    }
}
