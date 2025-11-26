using AutoMapper;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.Data.SqlClient;
using Oracle.ManagedDataAccess.Client;
using OrderService.Dtos;
using OrderService.Interfaces;
using System.Net;
//using System.Web.Mvc;
using System.Globalization;
using System.Data;
using Microsoft.AspNet.Identity;
using System.Data.SqlClient;
using System.Net.Http.Headers;
using System.Xml.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Newtonsoft.Json;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeliveredOrdersController : Controller
    {
        private readonly IOrder _OrderRepository;
        private readonly IMapper _mapper;

        public DeliveredOrdersController(IOrder orderService, IMapper mapper)
        {
            _OrderRepository = orderService;
            _mapper = mapper;

        }

        //Not Used
        [HttpGet]
        [Route("/GetDeliveredGridList/{customerCode}/{projectCode}")]
        public async Task<IActionResult> GetDeliveredGridList(string customerCode, string projectCode)
        {
            IEnumerable<DeliveredGridListDto> orderServiceList = await _OrderRepository.GetDeliveredGridList(customerCode, projectCode);
            var result = _mapper.Map<List<DeliveredGridListDto>>(orderServiceList);
            return Ok(result);
        }

        [HttpPost]
        [Route("/getDeliveredOrders")]
        public async Task<ActionResult> getDeliveredOrders(DeliveredOrderDto deliveredOrderDto)

        //public async Task<ActionResult> getDeliveredOrders(string CustomerCode, string ProjectCode,
        //            string PONumber, string PODate, string DONumber, string RDate,
        //            string WBS1, string WBS2, string WBS3, bool AllProjects)
        {

            string CustomerCode = deliveredOrderDto.CustomerCode;
            string ProjectCode = deliveredOrderDto.ProjectCode;
            List<string> AddressCodes = deliveredOrderDto.AddressCode;
            bool AllProjects = deliveredOrderDto.AllProjects;
            string UserName = deliveredOrderDto.UserName;
            //set kookie for customer and project
            string lPODateFrom = "";
            string lPODateTo = "";
            string lRDateFrom = "";
            string lRDateTo = "";

            //if (PONumber == null) PONumber = "";
            //if (DONumber == null) DONumber = "";
            //if (PODate == null) PODate = "";
            //if (RDate == null) RDate = "";
            //if (WBS1 == null) WBS1 = "";
            //if (WBS2 == null) WBS2 = "";
            //if (WBS3 == null) WBS3 = "";
            //if (PODate.Trim().Length == 0 || PODate.IndexOf("to") <= 0)
            //{
            //    lPODateFrom = "1900-01-01 00:00:00";
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
                lPODateFrom = "1900-01-01 00:00:00";
            }
            if (DateTime.TryParse(lPODateTo, out lDateV) != true)
            {
                lPODateTo = "2200-01-01 00:00:00";
            }

            //if (RDate == null) RDate = "";
            //if (RDate.Trim().Length == 0 || RDate.IndexOf("to") <= 0)
            //{
            //    if (PONumber == "" && PODate == "" && RDate == "" &&
            //    WBS1 == "" && WBS2 == "" && WBS3 == "" && DONumber == "")
            //    {
            //        lRDateFrom = "2000-01-01 00:00:00";
            //        //lRDateFrom = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
            //        lRDateTo = "2200-01-01 23:59:59";
            //    }
            //    else
            //    {
            //        lRDateFrom = "2000-01-01 00:00:00";
            //        lRDateTo = "2200-01-01 23:59:59";
            //    }
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
                lRDateTo = "2200-01-01 23:59:59";
            }

            var lCmd = new SqlCommand();
            SqlDataReader lRst;
            var lNDSCon = new SqlConnection();
            var lReturn = (new[]{ new
            {
                PONo = "",
                WBS1 = "",
                WBS2 = "",
                WBS3 = "",
                StructureElement = "",
                ProdType = "",
                PODate = "",
                RequiredDate = "",
                OrderWeight = "",
                LoadQty = "",
                LoadWT = "",
                DeliveryDate = "",
                TransportMode = "",
                VehicleNo = "",
                OutTime = "",
                DigiOSID = 0,
                DONo = "",
                BBSNo = "",
                BBSDesc = "",
                CustomerCode = "",
                ProjectCode = "",
                ProjectTitle = "",
                PartialDelivery = false,
                SubmittedBy="",
                 Address = "",
    Gate = ""
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
                        var lUserType = lUa.getUserType(UserName);
                        var lGroupName = lUa.getGroupName(UserName);

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
                                    DeliveredOrderProcess(CustomerCode, lProjects[i].ProjectCode, UserName, lRDateFrom, lRDateTo);
                                    if (lProjectState == "")
                                    {
                                        lProjectState = "ProjectCode = '" + lProjects[i].ProjectCode + "' ";
                                    }
                                    else
                                    {
                                        lProjectState = lProjectState + " OR ProjectCode = '" + lProjects[i].ProjectCode + "' ";
                                    }
                                }
                            }
                        }

                        if (lProjectState == "")
                        {
                            lProjectState = "AND 1 = 2 ";
                        }
                        else
                        {
                            lProjectState = "AND (" + lProjectState + ") ";
                        }

                    }
                    else
                    {
                        DeliveredOrderProcess(CustomerCode, ProjectCode, UserName, lRDateFrom, lRDateTo);
                        lProjectState = "AND ProjectCode = '" + ProjectCode + "' ";
                    }

                    if (AddressCodes != null && AddressCodes.Any())
                    {
                        var validCodes = AddressCodes
                            .Where(code => !string.IsNullOrWhiteSpace(code))
                            .Distinct() // optional: remove duplicates
                            .ToList();

                        if (validCodes.Any())
                        {
                            lAddressState =
                                "AND EXISTS (SELECT 1 FROM dbo.OESProjOrder M " +
                                "WHERE M.OrderNumber = D.DigiOSID " +
                                "AND M.AddressCode IN ('" + string.Join("', '", validCodes) + "')) ";
                        }
                    }

                    //lCmd.CommandText =
                    //"SELECT  " +
                    //"PONo, " +
                    //"WBS1, " +
                    //"WBS2, " +
                    //"WBS3, " +
                    //"StructureElement, " +
                    //"ProductType, " +
                    //"PODate, " +
                    //"RequiredDate, " +
                    //"OrderWT, " +
                    //"LoadQty, " +
                    //"LoadWT, " +
                    //"Delivery_Date, " +
                    //"TransportMode, " +
                    //"Vehicle_No, " +
                    //"Vehicle_out_time, " +
                    //"DigiOSID, " +
                    //"DONo, " +
                    //"BBSNo, " +
                    //"(SELECT Max(BBSDesc) FROM dbo.OESBBS B, dbo.OESProjOrdersSE S " +
                    //"WHERE B.JobID = S.CABJobID " +
                    //"AND B.CustomerCode = D.CustomerCode " +
                    //"AND B.ProjectCode = D.ProjectCode " +
                    //"AND S.OrderNumber = D.DigiOSID " +
                    //"AND B.BBSNo = D.BBSNo " +
                    //") as BBSDesc, " +
                    //"CustomerCode, " +
                    //"ProjectCode, " +
                    //"(SELECT ProjectTitle FROM dbo.OESProject " +
                    //"WHERE CustomerCode = D.CustomerCode " +
                    //"AND ProjectCode = D.ProjectCode) as ProjectTitle, " +
                    //"PartialDelivery, " +
                    //"(SELECT case when M.OrderStatus = 'Sent' then M.SubmitBy else M.UpdateBy end " +
                    //"FROM dbo.OESProjOrder M WHERE M.OrderNumber = D.DigiOSID) as SubmittedBy " +
                    //"FROM dbo.OESDeliveredOrders D " +
                    //"WHERE CustomerCode = '" + CustomerCode + "' " +
                    //"" + lProjectState + " " +
                    ////"AND ((PODate >= '" + lPODateFrom + "' " +
                    ////"AND DATEADD(d,-1,PODate) < '" + lPODateTo + "') " +
                    ////"OR (PODate is null AND '" + lPODateTo + "' = '2200-01-01 00:00:00' )) " +
                    ////"AND ('YES' in " +
                    ////"(SELECT CASE WHEN item >= '" + lRDateFrom + "' " +
                    ////"AND item <= '" + lRDateTo + "' THEN 'YES' ELSE 'NO' END " +
                    ////"FROM dbo.SplitString(Delivery_Date, ',') ) " +
                    ////"OR (Delivery_Date is null AND '" + lRDateTo + "' = '2200-01-01 23:59:59' )) " +
                    ////"AND (PONo like '%" + PONumber + "%' " +
                    ////"OR  (PONo is null AND '" + PONumber + "' = '' )) " +
                    ////"AND (DONo like '%" + DONumber + "%' " +
                    ////"OR  (DONo is null AND '" + DONumber + "' = '' )) " +
                    ////"AND (WBS1 like '%" + WBS1 + "%' " +
                    ////"OR (WBS1 is null AND '" + WBS1 + "' = '' )) " +
                    ////"AND (WBS2 like '%" + WBS2 + "%' " +
                    ////"OR (WBS2 is null AND '" + WBS2 + "' = '' )) " +
                    ////"AND (WBS3 like '%" + WBS3 + "%' " +
                    ////"OR (WBS3 is null AND '" + WBS3 + "' = '' )) " +
                    //"ORDER BY " +
                    //"Delivery_Date DESC ";


                    lCmd.CommandText =
"SELECT  " +
"PONo, " +
"WBS1, " +
"WBS2, " +
"WBS3, " +
"StructureElement, " +
"ProductType, " +
"PODate, " +
"RequiredDate, " +
"OrderWT, " +
"LoadQty, " +
"LoadWT, " +
"Delivery_Date, " +
"TransportMode, " +
"Vehicle_No, " +
"Vehicle_out_time, " +
"DigiOSID, " +
"DONo, " +
"BBSNo, " +
"(SELECT Max(BBSDesc) FROM dbo.OESBBS B, dbo.OESProjOrdersSE S " +
"WHERE B.JobID = S.CABJobID " +
"AND B.CustomerCode = D.CustomerCode " +
"AND B.ProjectCode = D.ProjectCode " +
"AND S.OrderNumber = D.DigiOSID " +
"AND B.BBSNo = D.BBSNo " +
") as BBSDesc, " +
"CustomerCode, " +
"ProjectCode, " +
"(SELECT ProjectTitle FROM dbo.OESProject " +
"WHERE CustomerCode = D.CustomerCode " +
"AND ProjectCode = D.ProjectCode) as ProjectTitle, " +
"PartialDelivery, " +
"(SELECT CASE WHEN M.OrderStatus = 'Sent' THEN M.SubmitBy ELSE M.UpdateBy END " +
"FROM dbo.OESProjOrder M WHERE M.OrderNumber = D.DigiOSID) as SubmittedBy, " +
"(SELECT TOP 1 M.Address FROM dbo.OESProjOrder M WHERE M.OrderNumber = D.DigiOSID) as Address, " +
"(SELECT TOP 1 M.Gate FROM dbo.OESProjOrder M WHERE M.OrderNumber = D.DigiOSID) as Gate " +
"FROM dbo.OESDeliveredOrders D " +
"WHERE CustomerCode = '" + CustomerCode + "' " +
"" + lProjectState + " " +
"" + lAddressState + " " +
"ORDER BY Delivery_Date DESC ";


                    var lProcessObj = new ProcessController();
                    if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
                    {
                        lCmd.Connection = lNDSCon;
                        lCmd.CommandTimeout = 300;
                        lRst = await lCmd.ExecuteReaderAsync();
                        if (lRst.HasRows)
                        {
                            var lSNo = 0;
                            while (lRst.Read())
                            {
                                lSNo = lSNo + 1;
                                string lOutTime = lRst.GetValue(14) == DBNull.Value ? "" : lRst.GetString(14).Trim();

                                lOutTime = lOutTime.Trim();

                                var lOutTimeA = lOutTime.Split(',').ToList();
                                if (lOutTimeA.Count > 0)
                                {
                                    lOutTime = "";
                                    for (int i = 0; i < lOutTimeA.Count; i++)
                                    {
                                        lOutTimeA[i] = lOutTimeA[i].Trim();
                                        if (lOutTime == "")
                                        {
                                            lOutTime = (lOutTimeA[i].Length > 4 ? lOutTimeA[i].Substring(0, 2) + ":" + lOutTimeA[i].Substring(2, 2) : lOutTimeA[i]);
                                        }
                                        else
                                        {
                                            lOutTime = lOutTime + "," + (lOutTimeA[i].Length > 4 ? lOutTimeA[i].Substring(0, 2) + ":" + lOutTimeA[i].Substring(2, 2) : lOutTimeA[i]);
                                        }
                                    }
                                }

                                var lStruEle = (lRst.GetValue(4) == DBNull.Value ? "" : (lRst.GetString(4).Trim() == "NONWBS" ? "" : lRst.GetString(4).Trim()));
                                var lProdType = (lRst.GetValue(5) == DBNull.Value ? "" : lRst.GetString(5).Trim());

                                //Core Cage Product Type added at line 305
                                //if (lProdType == "CAR" && lStruEle == "COLUMN")
                                if ((lProdType == "CAR" || lProdType == "CORE") && lStruEle == "COLUMN")
                                {
                                    lProdType = "CORE-CAGE";
                                }
                                else if (lProdType == "CAR")
                                {
                                    lProdType = "CARPET";
                                }
                                else if (lProdType == "MSH")
                                {
                                    lProdType = "MESH";
                                }
                                else if (lProdType == "PRC")
                                {
                                    lProdType = "PRE-CAGE";
                                }

                                lReturn.Add(new
                                {
                                    PONo = (lRst.GetValue(0) == DBNull.Value ? "" : lRst.GetString(0).Trim()),
                                    WBS1 = (lRst.GetValue(1) == DBNull.Value ? "" : lRst.GetString(1).Trim()),
                                    WBS2 = (lRst.GetValue(2) == DBNull.Value ? "" : lRst.GetString(2).Trim()),
                                    WBS3 = (lRst.GetValue(3) == DBNull.Value ? "" : lRst.GetString(3).Trim()),
                                    StructureElement = lStruEle,
                                    ProdType = lProdType,
                                    PODate = (lRst.GetValue(6) == DBNull.Value ? "" : lRst.GetDateTime(6).ToString("yyyy-MM-dd")),
                                    RequiredDate = (lRst.GetValue(7) == DBNull.Value ? "" : lRst.GetDateTime(7).ToString("yyyy-MM-dd")),
                                    OrderWeight = (lRst.GetValue(8) == DBNull.Value ? "0.000" : lRst.GetDecimal(8).ToString("###,###,##0.000;(###,##0.000);")),
                                    LoadQty = (lRst.GetValue(9) == DBNull.Value ? "0" : lRst.GetInt32(9).ToString("#######0")),
                                    LoadWT = (lRst.GetValue(10) == DBNull.Value ? "0.000" : lRst.GetDecimal(10).ToString("###,###,##0.000;(###,##0.000);")),
                                    DeliveryDate = (lRst.GetValue(11) == DBNull.Value ? "" : lRst.GetString(11)),
                                    TransportMode = (lRst.GetValue(12) == DBNull.Value ? "" : lRst.GetString(12).Trim()),
                                    VehicleNo = (lRst.GetValue(13) == DBNull.Value ? "" : lRst.GetString(13).Trim()),
                                    OutTime = lOutTime,
                                    DigiOSID = (lRst.GetValue(15) == DBNull.Value ? 0 : lRst.GetInt32(15)),
                                    DONo = (lRst.GetValue(16) == DBNull.Value ? "" : lRst.GetString(16).Trim()),
                                    BBSNo = (lRst.GetValue(17) == DBNull.Value ? "" : lRst.GetString(17).Trim()),
                                    BBSDesc = WebUtility.HtmlDecode(lRst.GetValue(18) == DBNull.Value ? "" : lRst.GetString(18).Trim()),
                                    CustomerCode = (lRst.GetValue(19) == DBNull.Value ? "" : lRst.GetString(19).Trim()),
                                    ProjectCode = (lRst.GetValue(20) == DBNull.Value ? "" : lRst.GetString(20).Trim()),
                                    ProjectTitle = (lRst.GetValue(21) == DBNull.Value ? "" : lRst.GetString(21).Trim()),
                                    PartialDelivery = (lRst.GetValue(22) == DBNull.Value ? false : lRst.GetBoolean(22)),
                                    SubmittedBy = (lRst.GetValue(23) == DBNull.Value ? "" : lRst.GetString(23)),
                                    Address = (lRst.GetValue(24) == DBNull.Value ? "" : lRst.GetString(24).Trim()),
                                    Gate = (lRst.GetValue(25) == DBNull.Value ? "" : lRst.GetString(25).Trim())

                                });
                            }
                        }
                        lRst.Close();

                        lProcessObj.CloseNDSConnection(ref lNDSCon);
                    }
                    lProcessObj = null;
                }
            }
            lCmd = null;
            lNDSCon = null;
            lRst = null;


            return Ok(lReturn);
        }



        [HttpGet]
        [Route("/DeliveredOrderProcess")]
        public int DeliveredOrderProcess(string CustomerCode, string ProjectCode, string UserID, string DateFrom, string DateTo)
        {
            int lReturn = 0;
            string lSQL = "";

            string lSOR = "";
            List<string> lSORList = new List<string>();

            string lDeliveryDate = "";

            var lCmd = new SqlCommand();

            SqlDataReader lRst;
            var lNDSCon = new SqlConnection();

            //var lCISCon = new OracleConnection();
            //var lOraCmd = new OracleCommand();
            //OracleDataReader lOraRst;

            try
            {
                var lProcessObj = new ProcessController();
                if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
                {
                    //get last delevery datetime for multiple delivery per order.
                    lCmd.CommandText = "SELECT Max(Delivery_Date) " +
                    "FROM dbo.OESDeliveredOrders " +
                    "WHERE CustomerCode = '" + CustomerCode + "' " +
                    "AND ProjectCode = '" + ProjectCode + "' " +
                    "AND Delivery_Date > '" + DateTime.Now.AddMonths(-6).ToString("yyyyMMdd") + "'";

                    lCmd.Connection = lNDSCon;
                    lCmd.CommandTimeout = 300;
                    lRst = lCmd.ExecuteReader();
                    if (lRst.HasRows)
                    {
                        if (lRst.Read())
                        {
                            lDeliveryDate = lRst.GetValue(0) == DBNull.Value ? "" : lRst.GetString(0);
                        }
                    }
                    lRst.Close();

                    if (lDeliveryDate == null || lDeliveryDate == "" || lDeliveryDate.Length < 10)
                    {
                        lDeliveryDate = DateTime.Now.ToString("yyyy-MM-dd");
                    }

                    if (lDeliveryDate.Length > 10)
                    {
                        lDeliveryDate = lDeliveryDate.Substring(0, 10);
                    }

                    //delete record delivered today
                    lCmd.CommandText = "DELETE " +
                    "FROM dbo.OESDeliveredOrders " +
                    "WHERE CustomerCode = '" + CustomerCode + "' " +
                    "AND ProjectCode = '" + ProjectCode + "' " +
                    "AND Delivery_Date like '%" + lDeliveryDate + "%' ";

                    lCmd.Connection = lNDSCon;
                    lCmd.CommandTimeout = 300;
                    lCmd.ExecuteNonQuery();

                    lSOR = "";

                    lCmd.CommandText = "SELECT H.ORD_REQ_NO " +
                    "FROM dbo.SAP_REQUEST_HDR H, dbo.SAP_REQUEST_DETAILS D " +
                    "WHERE H.ORD_REQ_NO = D.ORD_REQ_NO " +
                    "AND H.PROJ_ID = '" + ProjectCode + "' " +
                    "AND H.STATUS <> 'X' " +
                    "AND D.STATUS <> 'X' " +
                    "AND D.REQ_DELI_DATE > '" + DateTime.Now.AddMonths(-6).ToString("yyyyMMdd") + "' " +
                    "AND NOT EXISTS (SELECT SOR FROM dbo.OESDeliveredOrders " +
                    "WHERE CustomerCode = '" + CustomerCode + "' " +
                    "AND ProjectCode = '" + ProjectCode + "'" +
                    "AND SOR = H.ORD_REQ_NO) " +
                    "UNION  " +
                    "SELECT SOR " +
                    "FROM dbo.OESDeliveredOrders " +
                    "WHERE CustomerCode = '" + CustomerCode + "' " +
                    "AND ProjectCode = '" + ProjectCode + "' " +
                    "AND PartialDelivery = 1 " +
                    "UNION  " +
                    "SELECT SAPSOR " +
                    "FROM dbo.OESProjOrdersSE S, OESProjOrder M " +
                    "WHERE M.OrderNumber = S.OrderNumber " +
                    "AND M.CustomerCode = '" + CustomerCode + "' " +
                    "AND M.ProjectCode = '" + ProjectCode + "' " +
                    "AND S.SAPSOR like '10%' " +
                    "AND NOT EXISTS (SELECT SOR FROM dbo.OESDeliveredOrders " +
                    "WHERE CustomerCode = '" + CustomerCode + "' " +
                    "AND ProjectCode = '" + ProjectCode + "'" +
                    "AND SOR = S.SAPSOR) " +
                    "UNION " +
                    "SELECT B.BBSSAPSO " +
                    "FROM dbo.OESProjOrdersSE S, OESProjOrder M, dbo.OESBBS B " +
                    "WHERE M.OrderNumber = S.OrderNumber " +
                    "AND M.CustomerCode = '" + CustomerCode + "' " +
                    "AND M.ProjectCode = '" + ProjectCode + "' " +
                    "AND B.CustomerCode = M.CustomerCode " +
                    "AND B.ProjectCode = M.ProjectCode " +
                    "AND B.JobID = S.CABJobID " +
                    "AND BBSSAPSO > '' " +
                    "AND NOT EXISTS(SELECT SOR FROM dbo.OESDeliveredOrders " +
                    "WHERE CustomerCode = '" + CustomerCode + "' " +
                    "AND ProjectCode = '" + ProjectCode + "' " +
                    "AND SOR = B.BBSSAPSO) ";


                    lCmd.Connection = lNDSCon;
                    lCmd.CommandTimeout = 300;
                    lRst = lCmd.ExecuteReader();

                    //if (lRst.HasRows)
                    //{
                    //    if (lRst.Read())
                    //    {
                    //        lDeliveryDate = lRst.GetDateTime(0);
                    //    }
                    //}
                    //lRst.Close();

                    //if (lDeliveryDate == null)
                    //{
                    //    lDeliveryDate = DateTime.Now.AddYears(-5);
                    //}

                    ////for manual Do may PGI late.
                    //lDeliveryDate = lDeliveryDate.AddDays(-5);

                    lSOR = "";
                    if (lRst.HasRows)
                    {
                        while (lRst.Read())
                        {
                            if (lRst.GetString(0) != null && lRst.GetString(0) != "")
                            {
                                if (lSOR == "")
                                {
                                    lSOR = lRst.GetString(0);
                                }
                                else
                                {
                                    lSOR = lSOR + "," + lRst.GetString(0);
                                }
                            }
                        }
                    }
                    lRst.Close();

                    //Group to 300
                    lSORList = new List<string>();
                    if (lSOR != "")
                    {
                        var lSORA = lSOR.Split(',').ToList();
                        lSOR = "";
                        if (lSORA.Count > 0)
                        {
                            int lCount = 0;
                            for (int i = 0; i < lSORA.Count; i++)
                            {
                                if (lSOR == "")
                                {
                                    //lSOR = "'" + lSORA[i] + "'";
                                    lSOR = lSORA[i];
                                }
                                else
                                {
                                    //lSOR = lSOR + "," + "'" + lSORA[i] + "'";
                                    lSOR = lSOR + "," + lSORA[i];
                                }
                                lCount = lCount + 1;
                                if (lCount > 300)
                                {
                                    lSORList.Add(lSOR);
                                    lSOR = "";
                                    lCount = 0;
                                }
                            }
                            if (lSOR != "")
                            {
                                lSORList.Add(lSOR);
                            }
                        }

                    }


                    //lProcessObj.OpenCISConnection(ref lCISCon);


                    DeliveredInsertDto tempObj = new DeliveredInsertDto();

                    tempObj.pSOR = lSORList;
                    tempObj.pCustomerCode = CustomerCode;
                    tempObj.pProjectCode = ProjectCode;
                    tempObj.pClient = lProcessObj.strClient;
                    tempObj.pUserID = UserID;
                    tempObj.pNDSCon = lNDSCon;
                    //tempObj.pCISCon = lCISCon;
                    tempObj.DateFrom = DateFrom;
                    tempObj.DateTo = DateTo;

                    DeliveredInsert(tempObj);

                    //lProcessObj.CloseCISConnection(ref lCISCon);
                }

                lProcessObj.CloseNDSConnection(ref lNDSCon);
                lProcessObj = null;
            }
            catch (Exception ex)
            {
                var lError = ex.Message;
            }

            lCmd = null;
            lRst = null;
            lNDSCon = null;

            //lOraCmd = null;
            //lCISCon = null;
            return lReturn;
        }

        [HttpPost]
        [Route("/DeliveredInsert")]
        public int DeliveredInsert([FromBody] DeliveredInsertDto deliveredInsertDto)
        {
            List<string> pSOR = deliveredInsertDto.pSOR;
            string pCustomerCode = deliveredInsertDto.pCustomerCode;
            string pProjectCode = deliveredInsertDto.pProjectCode;
            string pClient = deliveredInsertDto.pClient;
            string pUserID = deliveredInsertDto.pUserID;
            SqlConnection pNDSCon = deliveredInsertDto.pNDSCon;
            //OracleConnection pCISCon = deliveredInsertDto.pCISCon;
            string DateFrom = deliveredInsertDto.DateFrom;
            string DateTo = deliveredInsertDto.DateTo;
            int lReturn = 0;
            string lSQL = "";

            string lExists = "";
            string lSelect1 = "";
            string lSelect2 = "";

            string lPONo = "";
            string lBBSNo = "";
            string lPODate = "";
            string lWBS1 = "";
            string lWBS2 = "";
            string lWBS3 = "";
            string lStEle = "";
            string lProdType = "";
            string lReqDate = "";
            string lSONo = "";
            string lSOR = "";
            string lDONo = "";

            string lOrderWT = "0";
            string lLoadPcs = "0";
            string lLoadWT = "0";
            string lLoadNo = "";

            string lUXInd = "";

            string lTransportMode = "";
            string lVehicleNo = "";
            string lOutTime = "";

            string lActDelDate = "";

            int lPartialDelivery = 0;

            int lOrderID = 0;

            DateTime lDeliveryDate = new DateTime();

            lDeliveryDate = DateTime.Now.AddMonths(-6);

            var lCmd = new SqlCommand();
            var lHMICmd = new SqlCommand();
            var lDA = new SqlDataAdapter();

            SqlDataReader lRst;
            SqlDataReader lRstHMI;

            var lOraCmd = new OracleCommand();
            OracleDataReader lOraRst;


            List<DeliveredOrderHMIdto> deliveryDataHMI = new List<DeliveredOrderHMIdto>();
            //Core Cage Product Type added at line 641

            //lSelect1 = "" +
            //    "SELECT " +
            //    "M.SALES_ORDER, " +
            //    "NVL(D.WBS1, ' ') as WBS1, " +
            //    "NVL(D.WBS2, ' ') as WBS2, " +
            //    "NVL(D.WBS3, ' ') as WBS3,  " +
            //    "NVL(D.ST_ELEMENT_TYPE, ' ') as ST_ELE, " +
            //    //        "NVL(CASE WHEN D.PRODUCT_TYPE = 'CAR' AND D.ST_ELEMENT_TYPE = 'COLUMN' THEN 'CORE-CAGE' ELSE D.PRODUCT_TYPE END, ' ') as Prod_type, " +
            //    "NVL(CASE WHEN (D.PRODUCT_TYPE = 'CAR' or D.PRODUCT_TYPE ='CORE') AND D.ST_ELEMENT_TYPE = 'COLUMN' THEN 'CORE-CAGE' ELSE D.PRODUCT_TYPE END, ' ') as Prod_type, " +

            //    "M.PO_NUMBER, " +
            //    "M.CUST_ORDER_DATE, " +
            //    "M.REQD_DEL_DATE, " +           //8
            //    "M.FIRST_PROMISED_D, " +

            //    "(SELECT NVL(SUM(THEO_WEIGHT_KG), 0)/1000 " +
            //    "FROM SAPSR3.YMSDT_ORDER_ITEM " +
            //    "WHERE MANDT = M.MANDT " +
            //    "AND ORDER_REQUEST_NO = M.ORDER_REQUEST_NO " +
            //    "AND HG_ITEM_NO = '000000') as WT, " +  //10

            //    "(SELECT NVL(MAX(PROD_GRP), ' ') " +
            //    "FROM SAPSR3.YMSDT_ORDER_ITEM " +
            //    "WHERE MANDT = M.MANDT " +
            //    "AND ORDER_REQUEST_NO = M.ORDER_REQUEST_NO) as ProdType2, " +   //11

            //    "(SELECT LISTAGG(VEHICLE_TYPE, ',') Within Group(order by VEHICLE_TYPE) " +
            //    "FROM(SELECT VEHICLE_TYPE FROM SAPSR3.YMPPT_LOAD_VEHIC WHERE " +
            //    "VBELN = M.SALES_ORDER AND LOAD_DATE <= TO_CHAR(SYSDATE, 'YYYYMMDD') " +
            //    "GROUP BY VEHICLE_TYPE)  ) as TransportType, " + //12

            //    "(SELECT LISTAGG(VEHICLE_ID, ',') Within Group(order by VEHICLE_ID) " +
            //    "FROM(SELECT VEHICLE_ID FROM SAPSR3.YMPPT_LOAD_VEHIC WHERE " +
            //    "VBELN = M.SALES_ORDER  AND LOAD_DATE <= TO_CHAR(SYSDATE, 'YYYYMMDD') " +
            //    "GROUP BY VEHICLE_ID)  ) as VehicleNo, " +        //13

            //    "(SELECT LISTAGG(LOAD_DATE, ',') Within Group(order by LOAD_DATE) " +
            //    "FROM(SELECT LOAD_DATE FROM SAPSR3.YMPPT_LOAD_VEHIC WHERE " +
            //    "VBELN = M.SALES_ORDER  AND LOAD_DATE <= TO_CHAR(SYSDATE, 'YYYYMMDD') " +
            //    "GROUP BY LOAD_DATE)  ) as DeliveryDate, " +      //14

            //    "(SELECT LISTAGG(WEIGH_OUT_TIME, ',') Within Group(order by WEIGH_OUT_TIME) " +
            //    "FROM(SELECT WEIGH_OUT_TIME FROM SAPSR3.YMPPT_LOAD_VEHIC WHERE " +
            //    "VBELN = M.SALES_ORDER  AND LOAD_DATE <= TO_CHAR(SYSDATE, 'YYYYMMDD') " +
            //    "GROUP BY WEIGH_OUT_TIME)  ) as OutTime, " +      //15

            //    //"(SELECT NVL(SUM(DESP_LOAD_QNTY), 0) " +
            //    //"FROM SAPSR3.YMPPT_LP_ITEM_C LC, SAPSR3.YMPPT_LP_HDR LH " +
            //    //"WHERE LC.MANDT = LH.MANDT " +
            //    //"AND LC.LOAD_NO = LH.LOAD_NO " +
            //    //"AND SALES_ORDER = M.SALES_ORDER " +
            //    //"AND PLAN_DELIV_DATE <= TO_CHAR(SYSDATE, 'YYYYMMDD') ) as DeliveryWT, " +                //16

            //    "(SELECT Sum(TotalWT) " +
            //    "FROM (SELECT P.VBELN, ARKTX, " +
            //    "(CASE WHEN BEDAR_LF = 'MTS' THEN " +
            //    "(CASE WHEN K.LIFEX = ' ' THEN (SELECT CASE WHEN P.VRKME = 'ST' AND P.MATNR NOT LIKE 'FMS%' THEN SUM(MTS_BATCH_QTY) ELSE SUM(TO_NUMBER(CONCAT('0', NVL(NO_PIECES, 0)))) END " +
            //    "FROM sapsr3.ymppt_lp_mts_bch " +
            //    "WHERE MANDT = '600' " +
            //    "AND VBELN = P.VGBEL " +
            //    "AND POSNR = P.VGPOS " +
            //    "AND MTS_BATCH = P.CHARG) " +
            //    "ELSE (SELECT CASE WHEN P.VRKME = 'ST' AND P.MATNR NOT LIKE 'FMS%' THEN SUM(MTS_BATCH_QTY) ELSE SUM(TO_NUMBER(CONCAT('0', NVL(NO_PIECES, 0)))) END " +
            //    "FROM sapsr3.ymppt_lp_mts_bch " +
            //    "WHERE load_no = K.LIFEX " +
            //    "AND VBELN = P.VGBEL " +
            //    "AND POSNR = P.VGPOS " +
            //    "AND MTS_BATCH = P.CHARG) END ) " +
            //    "ELSE " +
            //    "CASE WHEN " +
            //    "(SELECT NVL(SUM(C.ATFLV), 0) FROM SAPSR3.AUSP C, SAPSR3.MCH1 M1 " +
            //    "WHERE C.MANDT = M1.MANDT " +
            //    "AND C.OBJEK = M1.CUOBJ_BM " +
            //    "AND M1.MANDT = '" + pClient + "' " +
            //    "AND M1.MATNR = P.MATNR " +
            //    "AND C.ATINN = '0000000102' " +
            //    "AND CHARG in P.CHARG) = 0 " +              //Sometimes no Chracteristic transferred
            //    "THEN " +
            //    "(SELECT SUM(TO_NUMBER(CONCAT('0', NVL(NO_PIECES, 0)))) " +
            //    "FROM SAPSR3.YMPPT_LP_ITEM_C " +
            //    "WHERE SALES_ORDER = P.VGBEL " +
            //    "AND CLBD_ITEM = P.VGPOS) " +
            //    "ELSE ( SELECT SUM(C.ATFLV) FROM SAPSR3.AUSP C, SAPSR3.MCH1 M1 " +
            //    "WHERE C.MANDT = M1.MANDT " +
            //    "AND C.OBJEK = M1.CUOBJ_BM " +
            //    "AND M1.MANDT = '" + pClient + "' " +
            //    "AND M1.MATNR = P.MATNR " +
            //    "AND C.ATINN = '0000000102' " +
            //    "AND CHARG in P.CHARG) " +
            //    "END * SUM(P.LFIMG) / " +
            //    "(SELECT SUM(LFIMG) FROM SAPSR3.LIPS WHERE VGBEL = P.VGBEL AND VGPOS = P.VGPOS AND CHARG = P.CHARG) " +
            //    "END) as TotalPcs, " +
            //    "SUM(CASE WHEN P.VRKME = 'ST' AND P.MATNR NOT LIKE 'FMS%' THEN 0 ELSE ROUND(P.NTGEW/1000, 3) END ) as TotalWT, " +
            //    "SUM(CASE WHEN P.VRKME = 'M2' THEN P.LFIMG ELSE 0 END) as TotalM2 " +
            //    "FROM SAPSR3.LIPS P, SAPSR3.LIKP K " +
            //    "WHERE P.MANDT = K.MANDT " +
            //    "AND P.VBELN = K.VBELN " +
            //    "AND P.UEPOS = '000000' " +
            //    "AND P.POSNR LIKE '9%' " +
            //    "AND K.KUNAG = M.KUNAG " +
            //    "AND K.KUNNR = M.KUNNR " +
            //    "AND P.VGBEL = M.SALES_ORDER " +
            //    "GROUP BY P.VBELN, ARKTX, P.VGBEL, P.VGPOS, BEDAR_LF, K.LIFEX, P.MATKL, P.VRKME, P.CHARG, P.MATNR) " +
            //    ") as DeliveryWT, " +

            //    //"(SELECT NVL(SUM(CASE WHEN TRIM(MATNR) like 'FRD_%_060' THEN ROUND(PLAN_LOAD_QNTY, 0) ELSE TO_NUMBER(CONCAT('0', NVL(NO_PIECES, 0))) END ), 0) " +
            //    //"FROM SAPSR3.YMPPT_LP_ITEM_C LC, SAPSR3.YMPPT_LP_HDR LH " +
            //    //"WHERE LC.MANDT = LH.MANDT " +
            //    //"AND LC.LOAD_NO = LH.LOAD_NO " +
            //    //"AND SALES_ORDER = M.SALES_ORDER " +
            //    //"AND DESP_LOAD_QNTY > 0 " +
            //    //"AND PLAN_DELIV_DATE <= TO_CHAR(SYSDATE, 'YYYYMMDD') ) as PCS, " +                       //17

            //    "(SELECT Sum(TotalPcs) " +
            //    "FROM (SELECT P.VBELN, ARKTX, " +
            //    "(CASE WHEN BEDAR_LF = 'MTS' THEN " +
            //    //"(CASE WHEN K.LIFEX = ' ' THEN (SELECT CASE WHEN P.VRKME = 'ST' AND P.MATNR NOT LIKE 'FMS%' THEN SUM(MTS_BATCH_QTY) ELSE SUM(TO_NUMBER(CONCAT('0', NVL(NO_PIECES, 0)))) END " +
            //    "(CASE WHEN K.LIFEX = ' ' THEN (SELECT CASE WHEN P.VRKME = 'ST' AND SUM(TO_NUMBER(CONCAT('0', NVL(NO_PIECES, 0)))) = 0 THEN SUM(MTS_BATCH_QTY) ELSE SUM(TO_NUMBER(CONCAT('0', NVL(NO_PIECES, 0)))) END " +
            //    "FROM sapsr3.ymppt_lp_mts_bch " +
            //    "WHERE MANDT = '600' " +
            //    "AND VBELN = P.VGBEL " +
            //    "AND POSNR = P.VGPOS " +
            //    "AND MTS_BATCH = P.CHARG) " +
            //    //"ELSE (SELECT CASE WHEN P.VRKME = 'ST' AND P.MATNR NOT LIKE 'FMS%' THEN SUM(MTS_BATCH_QTY) ELSE SUM(TO_NUMBER(CONCAT('0', NVL(NO_PIECES, 0)))) END " +
            //    "ELSE (SELECT CASE WHEN P.VRKME = 'ST' AND SUM(TO_NUMBER(CONCAT('0', NVL(NO_PIECES, 0)))) = 0 THEN SUM(MTS_BATCH_QTY) ELSE SUM(TO_NUMBER(CONCAT('0', NVL(NO_PIECES, 0)))) END " +
            //    "FROM sapsr3.ymppt_lp_mts_bch " +
            //    "WHERE load_no = K.LIFEX " +
            //    "AND VBELN = P.VGBEL " +
            //    "AND POSNR = P.VGPOS " +
            //    "AND MTS_BATCH = P.CHARG) END ) " +
            //    "ELSE " +
            //    "CASE WHEN " +
            //    "(SELECT NVL(SUM(C.ATFLV), 0) FROM SAPSR3.AUSP C, SAPSR3.MCH1 M1 " +
            //    "WHERE C.MANDT = M1.MANDT " +
            //    "AND C.OBJEK = M1.CUOBJ_BM " +
            //    "AND M1.MANDT = '" + pClient + "' " +
            //    "AND M1.MATNR = P.MATNR " +
            //    "AND C.ATINN = '0000000102' " +
            //    "AND CHARG in P.CHARG) = 0 " +              //Sometimes no Chracteristic transferred
            //    "THEN " +
            //    "(SELECT SUM(TO_NUMBER(CONCAT('0', NVL(NO_PIECES, 0)))) " +
            //    "FROM SAPSR3.YMPPT_LP_ITEM_C " +
            //    "WHERE SALES_ORDER = P.VGBEL " +
            //    "AND CLBD_ITEM = P.VGPOS) " +
            //    "ELSE ( SELECT SUM(C.ATFLV) FROM SAPSR3.AUSP C, SAPSR3.MCH1 M1 " +
            //    "WHERE C.MANDT = M1.MANDT " +
            //    "AND C.OBJEK = M1.CUOBJ_BM " +
            //    "AND M1.MANDT = '" + pClient + "' " +
            //    "AND M1.MATNR = P.MATNR " +
            //    "AND C.ATINN = '0000000102' " +
            //    "AND CHARG in P.CHARG) " +
            //    "END * SUM(P.LFIMG) / " +
            //    "(SELECT SUM(LFIMG) FROM SAPSR3.LIPS WHERE VGBEL = P.VGBEL AND VGPOS = P.VGPOS AND CHARG = P.CHARG) " +
            //    "END) as TotalPcs, " +
            //    "SUM(CASE WHEN P.VRKME = 'ST' AND P.MATNR NOT LIKE 'FMS%' THEN 0 ELSE ROUND(P.NTGEW/1000, 3) END ) as TotalWT, " +
            //    "SUM(CASE WHEN P.VRKME = 'M2' THEN P.LFIMG ELSE 0 END) as TotalM2 " +
            //    "FROM SAPSR3.LIPS P, SAPSR3.LIKP K " +
            //    "WHERE P.MANDT = K.MANDT " +
            //    "AND P.VBELN = K.VBELN " +
            //    "AND P.UEPOS = '000000' " +
            //    "AND P.POSNR LIKE '9%' " +
            //    "AND K.KUNAG = M.KUNAG " +
            //    "AND K.KUNNR = M.KUNNR " +
            //    "AND P.VGBEL = M.SALES_ORDER " +
            //    "GROUP BY P.VBELN, ARKTX, P.VGBEL, P.VGPOS, BEDAR_LF, K.LIFEX, P.MATKL, P.VRKME, P.CHARG, P.MATNR) " +
            //    ") as PCS, " +

            //    "(SELECT LISTAGG(VEHICLE_TYPE, ',') Within Group(order by VEHICLE_TYPE) " +
            //    "FROM (SELECT VEHICLE_TYPE FROM SAPSR3.YMPPT_LP_ITEM_C LC, SAPSR3.YMPPT_LP_HDR LH " +
            //    "WHERE LC.MANDT = LH.MANDT " +
            //    "AND LC.LOAD_NO = LH.LOAD_NO " +
            //    "AND SALES_ORDER = M.SALES_ORDER AND PLAN_DELIV_DATE <= TO_CHAR(SYSDATE, 'YYYYMMDD') " +
            //    "AND DESP_LOAD_QNTY > 0 " +
            //    "GROUP BY VEHICLE_TYPE)  ) as TransportType2, " +   //18

            //    "(SELECT LISTAGG(TRAILER_NO, ',') Within Group(order by TRAILER_NO) " +
            //    "FROM (SELECT TRAILER_NO FROM SAPSR3.YMPPT_LP_ITEM_C LC, SAPSR3.YMPPT_LP_HDR LH " +
            //    "WHERE LC.MANDT = LH.MANDT " +
            //    "AND LC.LOAD_NO = LH.LOAD_NO " +
            //    "AND SALES_ORDER = M.SALES_ORDER AND PLAN_DELIV_DATE <= TO_CHAR(SYSDATE, 'YYYYMMDD') " +
            //    "AND DESP_LOAD_QNTY > 0 " +
            //    "GROUP BY TRAILER_NO)  ) as VehicleNo2, " +         //19

            //    "(SELECT LISTAGG(wadat_ist, ',') Within Group(order by wadat_ist) " +
            //    "FROM (SELECT K.wadat_ist FROM SAPSR3.LIPS P, SAPSR3.LIKP K WHERE P.MANDT = K.MANDT " +
            //    "AND P.VBELN = K.VBELN AND P.VGBEL = M.SALES_ORDER AND K.wadat_ist > '00000000' " +
            //    "GROUP BY K.wadat_ist)  ) as DeliveryDate2, " +     //20

            //    "(SELECT LISTAGG(LOAD_NO, ',') Within Group(order by LOAD_NO) " +
            //    "FROM (SELECT LH.LOAD_NO FROM SAPSR3.YMPPT_LP_ITEM_C LC, SAPSR3.YMPPT_LP_HDR LH " +
            //    "WHERE LC.MANDT = LH.MANDT " +
            //    "AND LC.LOAD_NO = LH.LOAD_NO " +
            //    "AND SALES_ORDER = M.SALES_ORDER AND PLAN_DELIV_DATE <= TO_CHAR(SYSDATE, 'YYYYMMDD') " +
            //    "AND DESP_LOAD_QNTY > 0 " +
            //    "GROUP BY LH.LOAD_NO)  ) as LoadNo, " +                 //21

            //    "M.ORDER_GROUP_ID, " +                                                                 //22
            //    "M.order_request_no, " +

            //    "(SELECT LISTAGG(VBELN, ',') Within Group(order by VBELN) " +
            //    "FROM (SELECT P.VBELN FROM SAPSR3.LIPS P " +
            //    "WHERE P.VGBEL = M.SALES_ORDER " +
            //    "GROUP BY P.VBELN)  ) as DeliveryNo, " +     //24

            //    "D.BBS_NO, " +                               //25

            //    "(SELECT COUNT(*) FROM SAPSR3.VBAP A " +
            //    "WHERE A.UEPOS = '000000' " +
            //    "AND A.VBELN = M.SALES_ORDER) - " +
            //    "(SELECT COUNT(*) FROM SAPSR3.VBUP U, SAPSR3.VBAP A " +
            //    "WHERE U.VBELN = A.VBELN " +
            //    "AND U.POSNR = A.POSNR " +
            //    "AND A.UEPOS = '000000' " +
            //    "AND U.VBELN = M.SALES_ORDER AND LFSTA = 'C') as PartialDel, " +  //26

            //    "(SELECT COUNT(*) FROM SAPSR3.YMPPT_LP_ITEM_C C1 " +
            //    "WHERE SALES_ORDER = M.SALES_ORDER " +
            //    "AND LOAD_DATE <= TO_CHAR(SYSDATE + 30, 'YYYYMMDD') " +
            //"AND (C1.STATUS = 'A' OR C1.STATUS = 'Y') " +
            //"AND (NOT EXISTS " +
            //"(SELECT K.VBELN FROM SAPSR3.LIPS P, SAPSR3.LIKP K " +
            //"WHERE P.MANDT = K.MANDT " +
            //"AND P.VBELN = K.VBELN " +
            //"AND P.VGBEL = C1.SALES_ORDER " +
            //"AND P.VGPOS = C1.CLBD_ITEM " +
            //"AND K.wadat_ist > '00000000') " +
            //"OR EXISTS " +
            //"(SELECT P.VBELN FROM SAPSR3.VBUP P " +
            //"WHERE P.MANDT = C1.MANDT " +
            //"AND P.VBELN = C1.SALES_ORDER " +
            //"AND P.POSNR = C1.CLBD_ITEM " +
            //"AND P.LFSTA = 'B')) " +
            //    "AND NOT EXISTS (SELECT LOAD_NO " +
            //    "FROM SAPSR3.YMPPT_LOAD_VEHIC " +
            //    "WHERE LOAD_NO = C1.LOAD_NO AND VBELN = M.SALES_ORDER)) as LeftMultiLoad " +   //27

            //    "FROM SAPSR3.YMSDT_ORDER_HDR M LEFT OUTER JOIN SAPSR3.YMSDT_REQ_DETAIL D " +
            //    "ON M.order_request_no = D.order_request_no ";
            ////"WHERE M.MANDT = '" + pClient + "' " +
            ////"AND M.PROJECT = '" + pProjectCode + "' ";


            lSelect1 = "SELECT " +
                        "M.SALES_ORDER, " +
                        "ISNULL(D.WBS1, ' ') AS WBS1, " +
                        "ISNULL(D.WBS2, ' ') AS WBS2, " +
                        "ISNULL(D.WBS3, ' ') AS WBS3, " +
                        "ISNULL(D.ST_ELEMENT_TYPE, ' ') AS ST_ELE, " +
                        "ISNULL(CASE WHEN (D.PRODUCT_TYPE = 'CAR' OR D.PRODUCT_TYPE = 'CORE') AND D.ST_ELEMENT_TYPE = 'COLUMN' THEN 'CORE-CAGE' ELSE D.PRODUCT_TYPE END, ' ') AS Prod_type, " +
                        "M.PO_NUMBER, " +
                        "M.CUST_ORDER_DATE, " +
                        "M.REQD_DEL_DATE, " +
                        "M.FIRST_PROMISED_D, " +

                        "(SELECT ISNULL(SUM(CAST(THEO_WGT AS FLOAT)), 0)/1000 FROM OesOrderItemHMI WHERE ORDER_NO = M.SALES_ORDER) AS WT, " +
                        "(SELECT ISNULL(MAX(PROD_TYPE), ' ') FROM OESOrderItemHMI WHERE ORDER_NO = M.SALES_ORDER) AS ProdType2, " +

                        "'BLANK' AS TransportType, " +

                        "(SELECT STRING_AGG(Vehicle_No, ',') WITHIN GROUP (ORDER BY Vehicle_No) " +
                        "FROM (SELECT Vehicle_No FROM HMIDONoDetails WHERE SALES_ORDER = M.SALES_ORDER AND TRY_CONVERT(date, Act_Del_Date, 103) <= CAST(GETDATE() AS date) GROUP BY Vehicle_No) AS SubQ) AS VehicleNo, " +

                        "(SELECT STRING_AGG(CONVERT(varchar, TRY_CONVERT(date, Act_Del_Date, 103), 112), ',') WITHIN GROUP (ORDER BY TRY_CONVERT(date, Act_Del_Date, 103)) AS DeliveryDate " +
                        "FROM (SELECT Act_Del_Date FROM HMIDONoDetails WHERE SALES_ORDER = M.SALES_ORDER AND TRY_CONVERT(date, Act_Del_Date, 103) <= CAST(GETDATE() AS date) GROUP BY Act_Del_Date) AS SubQ) AS DeliveryDate, " +

                        "(SELECT STRING_AGG(CONVERT(varchar, out_time, 23), ',') WITHIN GROUP (ORDER BY out_time) " +
                        "FROM (SELECT out_time FROM HMIDONoDetails WHERE SALES_ORDER = M.SALES_ORDER AND TRY_CONVERT(date, Act_Del_Date, 103) <= CAST(GETDATE() AS date) GROUP BY out_time) AS SubQ) AS OutTime, " +

                        "(SELECT SUM(CAST(LOAD_WT AS FLOAT)) FROM HMILoadDetails WHERE SALES_ORDER = M.SALES_ORDER GROUP BY SALES_ORDER) AS DeliveryWT, " +
                        "(SELECT SUM(CAST(LOAD_PCS AS FLOAT)) FROM HMILoadDetails WHERE SALES_ORDER = M.SALES_ORDER GROUP BY SALES_ORDER) AS PCS, " +

                        "'' AS TransportType2, " +
                        "'' AS VehicleNo2, " +
                        "'' AS DeliveryDate2, " +

                        "(SELECT STRING_AGG(LOAD_NO, ',') WITHIN GROUP (ORDER BY LOAD_NO) " +
                        "FROM (SELECT LOAD_NO FROM HMILoadDetails LD, HMIDONoDetails DN WHERE LD.SALES_ORDER = DN.SALES_ORDER AND LD.SALES_ORDER = M.SALES_ORDER AND TRY_CONVERT(date, DN.Act_Del_Date, 103) <= CAST(GETDATE() AS date) GROUP BY LOAD_NO) AS SubQ) AS LoadNo, " +

                        "M.ORDER_GROUP_ID, " +
                        "M.order_request_no, " +

                        "(SELECT STRING_AGG(DO_NO, ',') WITHIN GROUP (ORDER BY DO_NO) " +
                        "FROM (SELECT DO_NO FROM HMIDONoDetails WHERE SALES_ORDER = M.SALES_ORDER GROUP BY DO_NO) AS SubQ) AS DeliveryNo, " +

                        "D.BBS_NO, " +

                        "(SELECT COUNT(*) FROM DeliveredOrderdetailsHMI WHERE PARTIAL_DEL_IND <> 'Completed' " +
                        "AND SALES_ORDER IN (SELECT SALES_ORDER FROM HMILoadDetails WHERE SALES_ORDER = M.SALES_ORDER)) AS PartialDel, " +

                        "0 AS LeftMultiLoad " +

                        "FROM OesOrderHeaderHMI M " +
                        "LEFT OUTER JOIN OesRequestDetailsHMI D ON M.order_request_no = D.order_request_no";


            ////"AND REQD_DEL_DATE <= '" + lDeliveryDate.ToString("yyyy-MM-dd") + "' " +
            //lSelect2 = " AND M.SALES_ORDER <> ' ' " +
            //"AND M.SALES_ORDER is NOT Null " +
            //"AND ( ( EXISTS " +
            //"(SELECT P.VGBEL FROM SAPSR3.LIPS P, SAPSR3.LIKP K " +
            //"WHERE P.MANDT = K.MANDT " +
            //"AND P.VBELN = K.VBELN " +
            //"AND P.VGBEL = M.SALES_ORDER " +
            //"AND K.wadat_ist > '00000000' ) " +
            ////"AND K.LFDAT <= TO_CHAR(SYSDATE, 'YYYYMMDD') ) " +
            //"AND EXISTS " +
            //"(SELECT SALES_ORDER FROM SAPSR3.YMPPT_LP_ITEM_C " +
            //"WHERE SALES_ORDER = M.SALES_ORDER AND DELIVERY_DATE < TO_CHAR(SYSDATE, 'YYYYMMDD') " +
            //"AND DESP_LOAD_QNTY >= 0 ) ) " +
            //"OR EXISTS " +
            //"(SELECT VBELN FROM SAPSR3.YMPPT_LOAD_VEHIC WHERE " +
            //"VBELN = M.SALES_ORDER  AND LOAD_DATE <= TO_CHAR(SYSDATE, 'YYYYMMDD')) ) ";


            lSelect2 = "AND M.SALES_ORDER <> ' ' " +
                        "AND M.SALES_ORDER IS NOT NULL " +
                        "AND ( " +
                        "    (EXISTS ( " +
                        "        SELECT SALES_ORDER " +
                        "        FROM DeliveredOrderdetailsHMI " +
                        "        WHERE SALES_ORDER = M.SALES_ORDER) " +

                        "    AND EXISTS ( " +
                        "        SELECT SALES_ORDER " +
                        "        FROM HMIDONoDetails " +
                        "        WHERE SALES_ORDER = M.SALES_ORDER " +
                        "        AND TRY_CONVERT(DATE, Act_Del_Date, 103) <= CAST(GETDATE() AS DATE)) " +
                        "    ) " +

                        "OR EXISTS ( " +
                        "    SELECT SALES_ORDER " +
                        "    FROM HMILoadDetails " +
                        "    WHERE SALES_ORDER = M.SALES_ORDER " +
                        "    AND LOAD_NO IN ( " +
                        "        SELECT LOAD_NO " +
                        "        FROM SalesOrderloading " +
                        "        WHERE INPUT_CODE<>'D' AND TRY_CONVERT(DATE, PLAN_SHIPPING_DATE, 103) <= CAST(GETDATE() AS DATE)) " +
                        ") " +
                        ") ";


            lExists = "";
            if (pSOR != null && pSOR.Count > 0)
            {
                for (int i = 0; i < pSOR.Count; i++)
                {
                    if (i == 0)
                    {
                        //lSQL = lSelect1 + " WHERE exists(select str from (SELECT distinct trim(regexp_substr(str, '[^,]+', 1, level)) str " +
                        //    " FROM(SELECT '" + pSOR[i] + "' str from dual  ) t CONNECT BY instr(str, ',', 1, level - 1) > 0) " +
                        //    "WHERE str = M.order_request_no ) " + lSelect2;
                        ////lExists = "AND M.order_request_no NOT IN (" + pSOR[i] + ") ";

                        string formattedSOR = pSOR[i].Replace(",", "','"); // Split correctly
                        lSQL = lSelect1 + " WHERE M.order_request_no IN ('" + formattedSOR + "') " + lSelect2;
                    }
                    else
                    {
                        ////lExists = lExists + "AND M.order_request_no NOT IN (" + pSOR[i] + ") ";
                        //lSQL = lSQL + " UNION " + lSelect1 + " WHERE exists(select str from (SELECT distinct trim(regexp_substr(str, '[^,]+', 1, level)) str " +
                        //    "FROM(SELECT '" + pSOR[i] + "' str from dual  ) t CONNECT BY instr(str, ',', 1, level - 1) > 0) " +
                        //    "WHERE str = M.order_request_no ) " + lSelect2;

                        string formattedSOR = pSOR[i].Replace(",", "','"); // Split correctly
                        lSQL = lSQL + " UNION " + lSelect1 + " WHERE M.order_request_no IN ('" + formattedSOR + "') " + lSelect2;

                    }
                }
            }


            //"(SELECT LISTAGG(DELIVERY_DATE, ',') Within Group(order by DELIVERY_DATE) " +
            //"FROM (SELECT DELIVERY_DATE FROM SAPSR3.YMPPT_LP_ITEM_C LC " +
            //"WHERE SALES_ORDER = M.SALES_ORDER AND DELIVERY_DATE <= TO_CHAR(SYSDATE, 'YYYYMMDD') " +
            //"AND DESP_LOAD_QNTY > 0 " +
            //"GROUP BY DELIVERY_DATE)  ) as DeliveryDate2, " +     //20


            //lOraCmd.CommandText = lSQL;
            //lOraCmd.Connection = pCISCon;
            //lOraCmd.CommandTimeout = 300;
            //lOraRst = lOraCmd.ExecuteReader();
            //if (lOraRst.HasRows)

            lHMICmd.CommandText = lSQL;
            lHMICmd.Connection = pNDSCon;
            lHMICmd.CommandTimeout = 300;
            lRstHMI = lHMICmd.ExecuteReader();
            if (lRstHMI.HasRows)
            {
                while (lRstHMI.Read())
                {
                    lSONo = (lRstHMI.GetValue(0) == DBNull.Value ? "" : lRstHMI.GetString(0).Trim());
                    lWBS1 = (lRstHMI.GetValue(1) == DBNull.Value ? "" : lRstHMI.GetString(1).Trim());
                    lWBS2 = (lRstHMI.GetValue(2) == DBNull.Value ? "" : lRstHMI.GetString(2).Trim());
                    lWBS3 = (lRstHMI.GetValue(3) == DBNull.Value ? "" : lRstHMI.GetString(3).Trim());
                    lStEle = (lRstHMI.GetValue(4) == DBNull.Value ? "" : lRstHMI.GetString(4).Trim());
                    lPONo = (lRstHMI.GetValue(6) == DBNull.Value ? "" : lRstHMI.GetString(6).Trim());

                    if (lPONo.Length > 50)
                    {
                        lPONo = lPONo.Substring(0, 50);
                    }

                    lPODate = (lRstHMI.GetValue(7) == DBNull.Value ? "" : lRstHMI.GetString(7));

                    if (lPODate == "00000000")
                    {
                        lPODate = "";
                    }
                    else
                    {
                        lPODate = String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lPODate, "yyyyMMdd", CultureInfo.InvariantCulture));
                    }

                    lReqDate = (lRstHMI.GetValue(9) == DBNull.Value ? "" : lRstHMI.GetString(9).Trim());
                    if (lReqDate == "")
                    {
                        lReqDate = (lRstHMI.GetValue(8) == DBNull.Value ? "" : lRstHMI.GetString(8).Trim());
                        if (lReqDate != "")
                        {
                            lReqDate = String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lReqDate, "yyyyMMdd", CultureInfo.InvariantCulture));
                        }
                    }
                    else
                    {
                        if (lReqDate.Length > 10)
                        {
                            lReqDate = lReqDate.Substring(0, 10);
                        }
                        lReqDate = String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lReqDate, "yyyy-MM-dd", CultureInfo.InvariantCulture));
                    }

                    lProdType = (lRstHMI.GetValue(5) == DBNull.Value ? "" : lRstHMI.GetString(5).Trim());
                    if (lProdType == "")
                    {
                        var lProdType2 = (lRstHMI.GetValue(11) == DBNull.Value ? "" : lRstHMI.GetString(11).Trim());
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
                        else if (lProdType2 == "COUESP")
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

                    lOrderWT = lRstHMI.GetValue(10) == DBNull.Value ? "0.000" : lRstHMI.GetDouble(10).ToString("########0.000");

                    lTransportMode = (lRstHMI.GetValue(12) == DBNull.Value ? "" : lRstHMI.GetString(12).Trim());
                    if (lTransportMode == "")
                    {
                        lTransportMode = (lRstHMI.GetValue(18) == DBNull.Value ? "" : lRstHMI.GetString(18).Trim());
                    }
                    lTransportMode = lTransportMode.IndexOf(",") == 0 ? lTransportMode.Substring(1) : lTransportMode;

                    lVehicleNo = (lRstHMI.GetValue(13) == DBNull.Value ? "" : lRstHMI.GetString(13).Trim());
                    if (lVehicleNo == "")
                    {
                        //lVehicleNo = (lRstHMI.GetValue(19) == DBNull.Value ? "" : lRstHMI.GetString(19).Trim());
                    }
                    lVehicleNo = lVehicleNo.IndexOf(",") == 0 ? lVehicleNo.Substring(1) : lVehicleNo;

                    lActDelDate = (lRstHMI.GetValue(14) == DBNull.Value ? "" : lRstHMI.GetString(14).Trim());
                    if (lActDelDate == "")
                    {
                        lActDelDate = lRstHMI.GetValue(20) == DBNull.Value ? "" : lRstHMI.GetString(20).Trim();
                    }
                    lActDelDate = lActDelDate.IndexOf(",") == 0 ? lActDelDate.Substring(1) : lActDelDate;

                    var lDelDateA = lActDelDate.Split(',');
                    lActDelDate = "";
                    if (lDelDateA.Length > 0)
                    {
                        for (int i = 0; i < lDelDateA.Length; i++)
                        {
                            if (lDelDateA[i].Length == 8)
                            {
                                if (lActDelDate == "")
                                {
                                    lActDelDate = String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lDelDateA[i], "yyyyMMdd", CultureInfo.InvariantCulture));
                                }
                                else
                                {
                                    lActDelDate = lActDelDate + "," + String.Format("{0:yyyy-MM-dd}", DateTime.ParseExact(lDelDateA[i], "yyyyMMdd", CultureInfo.InvariantCulture));
                                }
                            }
                        }
                    }

                    lOutTime = (lRstHMI.GetValue(15) == DBNull.Value ? "" : lRstHMI.GetString(15).Trim());
                    lOutTime = lOutTime.IndexOf(",") == 0 ? lOutTime.Substring(1) : lOutTime;

                    lLoadWT = lRstHMI.GetValue(16) == DBNull.Value ? "0.000" : lRstHMI.GetDouble(16).ToString("########0.000");

                    lLoadPcs = lRstHMI.GetValue(17) == DBNull.Value ? "0" : lRstHMI.GetDouble(17).ToString();

                    if (lLoadPcs == "0" || lLoadPcs == "")
                    {
                        // Sales uom ST, Pcs will be in WT, such as MESH
                        lLoadPcs = lLoadWT;
                        lLoadWT = lOrderWT;

                    }
                    else
                    {
                        //if (lLoadWT != lOrderWT)
                        //{
                        //    lLoadWT = lOrderWT;
                        //}
                    }
                    lLoadNo = lRstHMI.GetValue(21) == DBNull.Value ? "0" : lRstHMI.GetString(21).Trim();
                    lLoadNo = lLoadNo.IndexOf(",") == 0 ? lLoadNo.Substring(1) : lLoadNo;

                    lUXInd = lRstHMI.GetValue(22) == DBNull.Value ? "0" : lRstHMI.GetString(22).Trim();

                    lSOR = lRstHMI.GetValue(23) == DBNull.Value ? "" : lRstHMI.GetString(23).Trim();

                    lDONo = lRstHMI.GetValue(24) == DBNull.Value ? "" : lRstHMI.GetString(24).Trim();

                    lBBSNo = lRstHMI.GetValue(25) == DBNull.Value ? "" : lRstHMI.GetString(25).Trim();

                    lPartialDelivery = lRstHMI.GetValue(26) == DBNull.Value ? 0 : lRstHMI.GetInt32(26);

                    var lLeftMultiLoad = lRstHMI.GetValue(27) == DBNull.Value ? 0 : lRstHMI.GetInt32(27);

                    if (lLeftMultiLoad > 0 && lVehicleNo != "" && lPartialDelivery == 0)
                    {
                        lPartialDelivery = 1;
                    }

                    DeliveredOrderHMIdto temp = new DeliveredOrderHMIdto { };
                    temp.lSONo = lSONo;
                    temp.lWBS1 = lWBS1;
                    temp.lWBS2 = lWBS2;
                    temp.lWBS3 = lWBS3;
                    temp.lStEle = lStEle;
                    temp.lPONo = lPONo;
                    temp.lPODate = lPODate;
                    temp.lReqDate = lReqDate;
                    temp.lProdType = lProdType;
                    temp.lOrderWT = lOrderWT;
                    temp.lTransportMode = lTransportMode;
                    temp.lVehicleNo = lVehicleNo;
                    temp.lActDelDate = lActDelDate;
                    temp.lOutTime = lOutTime;
                    temp.lLoadWT = lLoadWT;
                    temp.lLoadPcs = lLoadPcs;
                    temp.lLoadNo = lLoadNo;
                    temp.lUXInd = lUXInd;
                    temp.lSOR = lSOR;
                    temp.lDONo = lDONo;
                    temp.lBBSNo = lBBSNo;
                    temp.lPartialDelivery = lPartialDelivery;
                    temp.lLeftMultiLoad = lLeftMultiLoad;

                    deliveryDataHMI.Add(temp);


                }
            }
            lRstHMI.Close();


            for (int i = 0; i < deliveryDataHMI.Count; i++)
            {
                DeliveredOrderHMIdto lItem = deliveryDataHMI[i];
                lOrderID = 0;

                if (lItem.lUXInd == "CUS-UX" || lItem.lUXInd == "NSH-UX" || lItem.lUXInd == "CUS-UXE" || lItem.lUXInd == "NSH-UXE" || lItem.lSOR.Substring(0, 3) == "103")
                {
                    int lJobID = 0;

                    #region Check CAB
                    lSQL = "SELECT JobID " +
                    "FROM dbo.OESBBS " +
                    "WHERE CustomerCode = '" + pCustomerCode + "' " +
                    "AND ProjectCode = '" + pProjectCode + "' " +
                    "AND (BBSSOR = '" + lItem.lSOR + "' " +
                    "OR BBSSAPSO = '" + lItem.lSOR + "' " +
                    "OR BBSSORCoupler = '" + lItem.lSOR + "')";

                    lCmd.CommandText = lSQL;
                    lCmd.Connection = pNDSCon;
                    lCmd.CommandTimeout = 300;
                    lRst = lCmd.ExecuteReader();
                    if (lRst.HasRows)
                    {
                        if (lRst.Read())
                        {
                            lJobID = lRst.GetInt32(0);
                        }
                    }
                    lRst.Close();

                    if (lJobID > 0)
                    {
                        lSQL = "SELECT S.OrderNumber " +
                        "FROM dbo.OESProjOrdersSE S, dbo.OESProjOrder M " +
                        "WHERE S.OrderNumber = M.OrderNumber " +
                        "AND M.CustomerCode = '" + pCustomerCode + "' " +
                        "AND M.ProjectCode = '" + pProjectCode + "' " +
                        "AND S.CABJobID = " + lJobID.ToString() + " ";

                        lCmd.CommandText = lSQL;
                        lCmd.Connection = pNDSCon;
                        lCmd.CommandTimeout = 300;
                        lRst = lCmd.ExecuteReader();
                        if (lRst.HasRows)
                        {
                            if (lRst.Read())
                            {
                                lOrderID = lRst.GetInt32(0);
                            }
                        }
                        lRst.Close();
                    }
                    #endregion

                    #region Check Standard Product
                    if (lJobID == 0)
                    {
                        lSQL = "SELECT JobID " +
                        "FROM dbo.OESStdSheetJobAdvice " +
                        "WHERE CustomerCode = '" + pCustomerCode + "' " +
                        "AND ProjectCode = '" + pProjectCode + "' " +
                        "AND SAPSONo = '" + lItem.lSOR + "' ";

                        lCmd.CommandText = lSQL;
                        lCmd.Connection = pNDSCon;
                        lCmd.CommandTimeout = 300;
                        lRst = lCmd.ExecuteReader();
                        if (lRst.HasRows)
                        {
                            if (lRst.Read())
                            {
                                lJobID = lRst.GetInt32(0);
                            }
                        }
                        lRst.Close();

                        if (lJobID > 0)
                        {
                            lSQL = "SELECT S.OrderNumber " +
                            "FROM dbo.OESProjOrdersSE S, dbo.OESProjOrder M " +
                            "WHERE S.OrderNumber = M.OrderNumber " +
                            "AND M.CustomerCode = '" + pCustomerCode + "' " +
                            "AND M.ProjectCode = '" + pProjectCode + "' " +
                            "AND ( S.StdBarsJobID = " + lJobID.ToString() + " " +
                            "OR S.StdMESHJobID  = " + lJobID.ToString() + " " +
                            "OR S.CoilProdJobID  = " + lJobID.ToString() + ") ";

                            lCmd.CommandText = lSQL;
                            lCmd.Connection = pNDSCon;
                            lCmd.CommandTimeout = 300;
                            lRst = lCmd.ExecuteReader();
                            if (lRst.HasRows)
                            {
                                if (lRst.Read())
                                {
                                    lOrderID = lRst.GetInt32(0);
                                }
                            }
                            lRst.Close();
                        }
                    }
                    #endregion

                    #region Check BPC
                    if (lJobID == 0)
                    {
                        lSQL = "SELECT JobID " +
                        "FROM dbo.OESBPCDetailsProc " +
                        "WHERE CustomerCode = '" + pCustomerCode + "' " +
                        "AND ProjectCode = '" + pProjectCode + "' " +
                        "AND sor_no = '" + lItem.lSOR + "' ";

                        lCmd.CommandText = lSQL;
                        lCmd.Connection = pNDSCon;
                        lCmd.CommandTimeout = 300;
                        lRst = lCmd.ExecuteReader();
                        if (lRst.HasRows)
                        {
                            if (lRst.Read())
                            {
                                lJobID = lRst.GetInt32(0);
                            }
                        }
                        lRst.Close();

                        if (lJobID > 0)
                        {
                            lSQL = "SELECT S.OrderNumber " +
                            "FROM dbo.OESProjOrdersSE S, dbo.OESProjOrder M " +
                            "WHERE S.OrderNumber = M.OrderNumber " +
                            "AND M.CustomerCode = '" + pCustomerCode + "' " +
                            "AND M.ProjectCode = '" + pProjectCode + "' " +
                            "AND S.BPCJobID = " + lJobID.ToString() + " ";

                            lCmd.CommandText = lSQL;
                            lCmd.Connection = pNDSCon;
                            lCmd.CommandTimeout = 300;
                            lRst = lCmd.ExecuteReader();
                            if (lRst.HasRows)
                            {
                                if (lRst.Read())
                                {
                                    lOrderID = lRst.GetInt32(0);
                                }
                            }
                            lRst.Close();
                        }
                    }
                    #endregion

                    #region Check in UX
                    if (lJobID == 0)
                    {
                        lSQL = "SELECT S.OrderNumber " +
                        "FROM dbo.OESProjOrdersSE S, dbo.OESProjOrder M " +
                        "WHERE S.OrderNumber = M.OrderNumber " +
                        "AND M.CustomerCode = '" + pCustomerCode + "' " +
                        "AND M.ProjectCode = '" + pProjectCode + "' " +
                        "AND S.SAPSOR like '%" + lItem.lSOR + "%' ";

                        lCmd.CommandText = lSQL;
                        lCmd.Connection = pNDSCon;
                        lCmd.CommandTimeout = 300;
                        lRst = lCmd.ExecuteReader();
                        if (lRst.HasRows)
                        {
                            if (lRst.Read())
                            {
                                lOrderID = lRst.GetInt32(0);
                            }
                        }
                        lRst.Close();
                    }
                    #endregion

                    if (lOrderID > 0)
                    {
                        lSQL = "UPDATE dbo.OESProjOrdersSE " +
                        "SET OrderStatus = 'Delivered' " +
                        "WHERE OrderNumber = " + lOrderID.ToString() + " AND SAPSOR like '%" + lSOR + "%' ";

                        lCmd.CommandText = lSQL;
                        lCmd.Connection = pNDSCon;
                        lCmd.CommandTimeout = 300;
                        lCmd.ExecuteNonQuery();

                        lSQL = "UPDATE dbo.OESProjOrder " +
                        "SET OrderStatus = 'Delivered' " +
                        "WHERE OrderNumber = " + lOrderID.ToString() + " ";

                        lCmd.CommandText = lSQL;
                        lCmd.Connection = pNDSCon;
                        lCmd.CommandTimeout = 300;
                        lCmd.ExecuteNonQuery();
                    }

                }

                // Check existing
                var lOldSO = "";
                lCmd.CommandText = "SELECT SONumber " +
                "FROM dbo.OESDeliveredOrders " +
                "WHERE CustomerCode = '" + pCustomerCode + "' " +
                "AND ProjectCode = '" + pProjectCode + "' " +
                "AND SONumber = '" + lItem.lSONo + "' ";

                lCmd.Connection = pNDSCon;
                lCmd.CommandTimeout = 300;
                lRst = lCmd.ExecuteReader();
                if (lRst.HasRows)
                {
                    if (lRst.Read())
                    {
                        lOldSO = lRst.GetString(0);
                    }
                }
                lRst.Close();

                if (lOldSO != "")
                {
                    lCmd.CommandText = "DELETE " +
                    "FROM dbo.OESDeliveredOrders " +
                    "WHERE CustomerCode = '" + pCustomerCode + "' " +
                    "AND ProjectCode = '" + pProjectCode + "' " +
                    "AND SONumber = '" + lItem.lSONo + "' ";

                    lCmd.Connection = pNDSCon;
                    lCmd.CommandTimeout = 300;
                    lCmd.ExecuteNonQuery();
                }

                lSQL = "INSERT INTO dbo.OESDeliveredOrders " +
                "(CustomerCode " +
                ", ProjectCode " +
                ", SONumber " +
                ", SOR " +
                ", LoadNo " +
                ", PONo " +
                ", BBSNo " +
                ", WBS1 " +
                ", WBS2 " +
                ", WBS3 " +
                ", StructureElement " +
                ", ProductType " +
                ", PODate " +
                ", RequiredDate " +
                ", OrderWT " +
                ", LoadQty " +
                ", LoadWT " +
                ", Delivery_Date " +
                ", TransportMode " +
                ", Vehicle_No " +
                ", Vehicle_out_time " +
                ", DONo" +
                ", DigiOSID " +
                ", PartialDelivery " +
                ", UpdateDate " +
                ", UpdateBy) " +
                "VALUES " +
                "('" + pCustomerCode + "' " +
                ",'" + pProjectCode + "' " +
                ",'" + lItem.lSONo + "' " +
                ",'" + lItem.lSOR + "' " +
                ",'" + lItem.lLoadNo + "' " +
                ",'" + lItem.lPONo + "' " +
                ",'" + lItem.lBBSNo + "' " +
                ",'" + lItem.lWBS1 + "' " +
                ",'" + lItem.lWBS2 + "' " +
                ",'" + lItem.lWBS3 + "' " +
                ",'" + lItem.lStEle + "' " +
                ",'" + lItem.lProdType + "' " +
                ",'" + lItem.lPODate + "' " +
                ",'" + lItem.lReqDate + "' " +
                ",0" + lItem.lOrderWT + " " +
                ",0" + lItem.lLoadPcs + " " +
                ",0" + lItem.lLoadWT + " " +
                ",'" + lItem.lActDelDate + "' " +
                ",'" + lItem.lTransportMode + "' " +
                ",'" + lItem.lVehicleNo + "' " +
                ",'" + lItem.lOutTime + "' " +
                ",'" + lItem.lDONo + "' " +
                ",0" + lOrderID + " " +
                "," + lItem.lPartialDelivery.ToString() + " " +
                ",'" + DateTime.Now.ToString("yyyy-MM-dd") + "' " +
                ",'" + pUserID + "') ";

                lCmd.CommandText = lSQL;
                lCmd.Connection = pNDSCon;
                lCmd.CommandTimeout = 300;
                lCmd.ExecuteNonQuery();
            }


            lCmd = null;
            lHMICmd = null;
            lRst = null;

            lOraCmd = null;
            lOraRst = null;
            lRstHMI = null;

            return lReturn;
        }

        [HttpGet]
        [Route("/getDocumentIndex/{CustomerCode}/{ProjectCode}/{DONumber}/{DocumentType}/{DODate}")]
        public async Task<ActionResult> getDocumentIndex(string CustomerCode, string ProjectCode, string DONumber, string DocumentType, string? DODate)
        {
            bool lReturn = true;
            string lErrorMsg = "";
            string lToken = "";
            string lRet = "";
            string lNewGenURL = "https://natsteel.newgenbpmcloud.com/WebServiceCall/CallWebService";
            string lInputXml = "<InputCriteria><CustomerId>" + CustomerCode + "</CustomerId>" +
            "<ProjectId>" + ProjectCode + "</ProjectId>" +
            //"<FromDate>00-00-0000</FromDate>" +
            //"<ToDate>00-00-0000</ToDate>" +
            "<DocumentDetails>" +
            "<DocumentType>" + DocumentType + "</DocumentType>" +
            "<ReferenceNo>" + DONumber + "</ReferenceNo>" +
            "</DocumentDetails>" +
            "</InputCriteria>";

            if (DocumentType == "Mill Certificate" && DODate != null && DODate != "")
            {
                var lDate = DateTime.Now;
                var lDateFr = DateTime.Now;
                var lDateTo = DateTime.Now;

                if (DODate.IndexOf(",") > 0)
                {
                    var DODateArr = DODate.Split(',');
                    if (DODateArr.Length > 0)
                    {
                        if (DateTime.TryParse(DODateArr[0], out lDate))
                        {
                            lDateFr = lDate;
                            lDateTo = lDate;
                        }
                        for (int i = 0; i < DODateArr.Length; i++)
                        {
                            if (DateTime.TryParse(DODateArr[i], out lDate))
                            {
                                if (lDate < lDateFr)
                                {
                                    lDateFr = lDate;
                                }
                                if (lDate > lDateTo)
                                {
                                    lDateTo = lDate;
                                }
                            }
                        }
                    }
                    lDateFr = lDateFr.AddDays(-10);
                    lDateTo = lDateTo.AddDays(+10);
                }
                else
                {
                    if (DateTime.TryParse(DODate, out lDate))
                    {
                        lDateFr = lDate;
                        lDateTo = lDate;
                    }
                    lDateFr = lDateFr.AddDays(-10);
                    lDateTo = lDateTo.AddDays(+10);
                }

                lInputXml = "<InputCriteria><CustomerId>" + CustomerCode + "</CustomerId>" +
                "<ProjectId>" + ProjectCode + "</ProjectId>" +
                "<FromDate>" + lDateFr.ToString("dd-MM-yyyy") + "</FromDate>" +
                "<ToDate>" + lDateTo.ToString("dd-MM-yyyy") + "</ToDate>" +
                "<DocumentDetails>" +
                "<DocumentType>" + DocumentType + "</DocumentType>" +
                "<ReferenceNo></ReferenceNo>" +
                "</DocumentDetails>" +
                "</InputCriteria>";
            }

            string lURlFull = lNewGenURL + "?InputXML=" + lInputXml + "&RequestType=advanced";

            HttpClient lClient = new HttpClient();

            lClient.BaseAddress = new Uri(lURlFull);

            lClient.DefaultRequestHeaders.Accept.Clear();
            lClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (lToken.Length > 0)
            {
                lClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + lToken);
            }

            try
            {
                HttpResponseMessage lResponse = await lClient.PostAsXmlAsync("", "");
                if (lResponse.IsSuccessStatusCode)
                {
                    lRet = await lResponse.Content.ReadAsStringAsync();
                }

            }
            catch (Exception e)
            {
                lErrorMsg = e.Message;
                lReturn = false;
            }

            if (lReturn == true)
            {

                List<DocumentDetails> lRetDocs = new List<DocumentDetails>();
                TextReader sr = new StringReader(lRet);
                XDocument doc = XDocument.Load(sr);
                lRetDocs = doc.Descendants("DocumentDetails").Select(x => new DocumentDetails()
                {
                    DocumentIndex = x.Element("DocumentIndex").Value,
                    DocumentName = x.Element("DocumentName").Value,
                    DocumentType = x.Element("DocumentType").Value,
                    Extension = x.Element("Extension").Value,
                    ImageIndex = x.Element("ImageIndex").Value,
                    InsertDate = x.Element("InsertDate").Value
                }).ToList();

                //OutputCriteria lJsonObj = null;

                //try
                //{
                //    using (TextReader sr = new StringReader(lRet))
                //    {
                //        var serializer = new System.Xml.Serialization.XmlSerializer(typeof(OutputCriteria));
                //        lJsonObj = (OutputCriteria)serializer.Deserialize(sr);
                //    }
                //}
                //catch { }

                //if (lJsonObj != null && lJsonObj.Type != null && lJsonObj.Type == "S" && lJsonObj.DocumentDetails != null)
                //{
                //    lRetDocs.Add(lJsonObj.DocumentDetails);
                //}
                //else
                //{
                //    try
                //    {
                //        OutputCriteriaMultiple lJsonObjM = null;
                //        using (TextReader sr = new StringReader(lRet))
                //        {
                //            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(OutputCriteriaMultiple));
                //            lJsonObjM = (OutputCriteriaMultiple)serializer.Deserialize(sr);
                //        }
                //        if (lJsonObjM != null && lJsonObjM.Type != null && lJsonObjM.Type == "S" && lJsonObjM.DocumentDetails != null)
                //        {
                //            lRetDocs.AddRange(lJsonObjM.DocumentDetails);
                //        }
                //    }
                //    catch { }
                //}

                if (lRetDocs != null && lRetDocs.Count > 0)
                {
                    if (DocumentType == "Mill Certificate")
                    {
                        List<DocumentDetails> lRetMillCertDocs = new List<DocumentDetails>();
                        for (int i = 0; i < lRetDocs.Count; i++)
                        {
                            if (lRetDocs[i].DocumentName.IndexOf(DONumber) >= 0)
                            {
                                lRetMillCertDocs.Add(lRetDocs[i]);
                            }
                        }
                        if (lRetMillCertDocs.Count > 0)
                        {
                            return Json(new { success = lReturn, message = lRetMillCertDocs });
                        }
                        else
                        {
                            lReturn = false;
                            lErrorMsg = "The Document has not generated yet.";
                            return Json(new { success = lReturn, message = lErrorMsg });
                        }
                    }
                    else
                    {
                        return Json(new { success = lReturn, message = lRetDocs });
                    }
                }
                else
                {
                    lReturn = false;
                    lErrorMsg = "The Document has not generated yet.";
                    return Json(new { success = lReturn, message = lErrorMsg });
                }
            }
            else
            {
                return Json(new { success = lReturn, message = lErrorMsg });
            }

            //return Json(lJson, JsonRequestBehavior.AllowGet);
        }


        public class DocumentDetails
        {
            public string DocumentIndex { get; set; }
            public string DocumentName { get; set; }
            public string DocumentType { get; set; }
            public string Extension { get; set; }
            public string ImageIndex { get; set; }
            public string InsertDate { get; set; }
        }


        [HttpGet]
        [Route("/getDOMaterial_Delivered/{CustomerCode}/{ProjectCode}/{DONumbers}/{DODate}")]
        public async Task<ActionResult> getDOMaterial(string CustomerCode, string ProjectCode, string DONumbers, string DODate)
        {
            string lErrorMsg = "Got DO details successfully.";

            //var lCISCon = new OracleConnection();
            //var lOraCmd = new OracleCommand();
            //OracleDataReader lOraRst;


            var lNDSCon = new SqlConnection();
            var lSQLCmd = new SqlCommand();
            SqlDataReader lSQLRst;

            string lSQL = "";
            int lSuccess = 1;

            CustomerCode = CustomerCode.Trim();
            ProjectCode = ProjectCode.Trim();

            var lReturn = (new[]{ new
            {
                DONumber = "",
                Product = "",
                TotalPCs = 0,
                TotalTonnage = (decimal)0,
                TotalM2 = (decimal)0,
                SignedDO = 0,
                DODetails = 0,
                MillCert = 0
            }}).ToList();

            string lProdDesc = "";
            int lTotalPCs = 0;
            decimal lTotalWT = 0;
            decimal lTotalM2 = 0;
            string lProdType = "";

            lReturn.RemoveAt(0);

            try
            {
                if (DONumbers != null && DONumbers != "")
                {
                    var lDONo = "";
                    var lDOList = DONumbers.Split(',').ToList();

                    //if (lDOList != null && lDOList.Count > 0)
                    //{
                    //    for (int i = 0; i < lDOList.Count; i++)
                    //    {
                    //        if (lDOList[i] != null && lDOList[i].Trim().Length > 0)
                    //        {
                    //            if (lDONo == "")
                    //            {
                    //                lDONo = "'" + lDOList[i].Trim() + "'";
                    //            }
                    //            else
                    //            {
                    //                lDONo = lDONo + ",'" + lDOList[i].Trim() + "'";

                    //            }
                    //        }

                    //    }
                    //}

                    //if (lDONo == "")
                    //{
                    //    lDONo = "''";
                    //}

                    var lDBConn = new ProcessController();
                    //if (lDBConn.OpenCISConnection(ref lCISCon) != true)
                    if (lDBConn.OpenNDSConnection(ref lNDSCon) != true)
                    {
                        lErrorMsg = "Cannot open database";
                        lSuccess = 0;
                    }

                    for (int i = 0; i < lDOList.Count; i++)
                    {
                        lDONo = lDOList[i];

                        if (lDONo != null && lDONo.Trim() != "")
                        {
                            lDONo = lDONo.Trim();
                            var lDocSignedDO = 0;
                            var lDocDODetail = 0;
                            var lDocMillCert = 0;

                            var lDocIndexJ = new List<DocumentDetails>();
                            TestDocumentIndexDto obj = new TestDocumentIndexDto();
                            obj.CustomerCode = CustomerCode;
                            obj.ProjectCode = ProjectCode;
                            obj.DONumber = lDONo;
                            obj.DocumentType = "Signed DO";
                            obj.DODate = DODate;
                            lDocIndexJ = await testDocumentIndex(obj); //Commented for HMI
                            if (lDocIndexJ.Count > 0)
                            {
                                lDocSignedDO = 1;
                            }

                            TestDocumentIndexDto obj1 = new TestDocumentIndexDto();
                            obj1.CustomerCode = CustomerCode;
                            obj1.ProjectCode = ProjectCode;
                            obj1.DONumber = lDONo;
                            obj1.DocumentType = "DO Detail";
                            obj1.DODate = DODate;
                            lDocIndexJ = await testDocumentIndex(obj1); //Commented for HMI
                            if (lDocIndexJ.Count > 0)
                            {
                                lDocDODetail = 1;
                            }

                            TestDocumentIndexDto obj2 = new TestDocumentIndexDto();
                            obj2.CustomerCode = CustomerCode;
                            obj2.ProjectCode = ProjectCode;
                            obj2.DONumber = lDONo;
                            obj2.DocumentType = "Mill Certificate";
                            obj2.DODate = DODate;
                            lDocIndexJ = await testDocumentIndex(obj2); //Commented for HMI
                            if (lDocIndexJ.Count > 0)
                            {
                                lDocMillCert = 1;
                            }

                            //lSQL =
                            //"SELECT VBELN, " +
                            //"ARKTX, " +
                            //"Sum(TotalPcs), " +
                            //"Sum(TotalWT), Sum(TotalM2) " +
                            //"FROM (SELECT P.VBELN, ARKTX, " +
                            //"(CASE WHEN BEDAR_LF = 'MTS' THEN " +
                            ////"(CASE WHEN K.LIFEX = ' ' THEN (SELECT CASE WHEN P.VRKME = 'ST' AND P.MATNR NOT LIKE 'FMS%' THEN SUM(MTS_BATCH_QTY) ELSE SUM(TO_NUMBER(CONCAT('0', NVL(NO_PIECES, 0)))) END " +
                            //"(CASE WHEN K.LIFEX = ' ' THEN (SELECT CASE WHEN P.VRKME = 'ST' AND SUM(TO_NUMBER(CONCAT('0', NVL(NO_PIECES, 0)))) = 0 THEN SUM(MTS_BATCH_QTY) ELSE SUM(TO_NUMBER(CONCAT('0', NVL(NO_PIECES, 0)))) END " +
                            //"FROM sapsr3.ymppt_lp_mts_bch " +
                            //"WHERE MANDT = '600' " +
                            //"AND VBELN = P.VGBEL " +
                            //"AND POSNR = P.VGPOS " +
                            //"AND MTS_BATCH = P.CHARG) " +
                            ////"ELSE (SELECT CASE WHEN P.VRKME = 'ST' AND P.MATNR NOT LIKE 'FMS%' THEN SUM(MTS_BATCH_QTY) ELSE SUM(TO_NUMBER(CONCAT('0', NVL(NO_PIECES, 0)))) END " +
                            //"ELSE (SELECT CASE WHEN P.VRKME = 'ST' AND SUM(TO_NUMBER(CONCAT('0', NVL(NO_PIECES, 0)))) = 0 THEN SUM(MTS_BATCH_QTY) ELSE SUM(TO_NUMBER(CONCAT('0', NVL(NO_PIECES, 0)))) END " +
                            //"FROM sapsr3.ymppt_lp_mts_bch " +
                            //"WHERE load_no = K.LIFEX " +
                            //"AND VBELN = P.VGBEL " +
                            //"AND POSNR = P.VGPOS " +
                            //"AND MTS_BATCH = P.CHARG) END ) " +
                            //"ELSE " +
                            //"CASE WHEN " +
                            //"(SELECT NVL(SUM(C.ATFLV), 0) FROM SAPSR3.AUSP C, SAPSR3.MCH1 M " +
                            //"WHERE C.MANDT = M.MANDT " +
                            //"AND C.OBJEK = M.CUOBJ_BM " +
                            //"AND M.MANDT = '600' " +
                            //"AND M.MATNR = P.MATNR " +
                            //"AND C.ATINN = '0000000102' " +
                            //"AND CHARG in P.CHARG) = 0 " +              //Sometimes no Chracteristic transferred
                            //"THEN " +
                            //"(SELECT SUM(TO_NUMBER(CONCAT('0', NVL(NO_PIECES, 0)))) " +
                            //"FROM SAPSR3.YMPPT_LP_ITEM_C " +
                            //"WHERE SALES_ORDER = P.VGBEL " +
                            //"AND CLBD_ITEM = P.VGPOS) " +
                            //"* SUM(P.LFIMG) / " +
                            //"(SELECT SUM(LFIMG) FROM SAPSR3.LIPS WHERE VGBEL = P.VGBEL AND VGPOS = P.VGPOS AND CHARG = P.CHARG) " +
                            //"ELSE ( SELECT SUM(C.ATFLV) FROM SAPSR3.AUSP C, SAPSR3.MCH1 M " +
                            //"WHERE C.MANDT = M.MANDT " +
                            //"AND C.OBJEK = M.CUOBJ_BM " +
                            //"AND M.MANDT = '600' " +
                            //"AND M.MATNR = P.MATNR " +
                            //"AND C.ATINN = '0000000102' " +
                            //"AND CHARG in P.CHARG) " +
                            //"END " +
                            //"END) as TotalPcs, " +
                            //"SUM(CASE WHEN P.VRKME = 'ST' AND P.MATNR NOT LIKE 'FMS%' THEN 0 ELSE ROUND(P.NTGEW/1000, 3) END ) as TotalWT, " +
                            //"SUM(CASE WHEN P.VRKME = 'M2' THEN P.LFIMG ELSE 0 END) as TotalM2 " +
                            //"FROM SAPSR3.LIPS P, SAPSR3.LIKP K " +
                            //"WHERE P.MANDT = K.MANDT " +
                            //"AND P.VBELN = K.VBELN " +
                            //"AND P.UEPOS = '000000' " +
                            //"AND P.POSNR LIKE '9%' " +
                            //"AND K.KUNAG = '" + CustomerCode + "' " +
                            //"AND K.KUNNR = '" + ProjectCode + "' " +
                            //"AND P.VBELN = '" + lDONo.Trim() + "' " +
                            //"GROUP BY P.VBELN, ARKTX, P.VGBEL, P.VGPOS, BEDAR_LF, K.LIFEX, P.MATKL, P.VRKME, P.CHARG, P.MATNR) " +
                            //"GROUP BY VBELN, ARKTX " +
                            //"ORDER BY VBELN, ARKTX ";


                            lSQL = "SELECT  DONumber,  " +
                                "ProductCodeDesc AS Product,  " +
                                "CAST(TotalPcs AS INT) AS Pieces,  " +
                                "CAST(TotalWT AS DECIMAL(18, 2)) AS Weight,  " +
                                "CAST(TotalM2 AS DECIMAL(18, 2)) AS SquareMeter " +
                                "FROM SystemDeliveryDetails " +
                                "WHERE DONumber = '" + lDONo.Trim() + "' ";

                            lSQLCmd.CommandText = lSQL;
                            lSQLCmd.Connection = lNDSCon;
                            lSQLCmd.CommandTimeout = 300;
                            lSQLRst = lSQLCmd.ExecuteReader();
                            if (lSQLRst.HasRows)
                            {
                                while (lSQLRst.Read())
                                {
                                    lProdDesc = lSQLRst.GetString(1).Trim();
                                    if (lProdDesc.IndexOf(" - ") > 0)
                                    {
                                        lProdDesc = lProdDesc.Substring(0, lProdDesc.IndexOf(" - "));
                                    }

                                    lTotalPCs = (lSQLRst.GetValue(2) == DBNull.Value ? 0 : lSQLRst.GetInt32(2));
                                    lTotalWT = (lSQLRst.GetValue(3) == DBNull.Value ? 0 : lSQLRst.GetDecimal(3));
                                    lTotalM2 = (lSQLRst.GetValue(4) == DBNull.Value ? 0 : lSQLRst.GetDecimal(4));

                                    if (lProdDesc != null && lProdDesc.Trim().Length > 0 && (lTotalPCs > 0 || lTotalWT > 0 || lTotalM2 > 0))
                                    {
                                        lReturn.Add(new
                                        {
                                            DONumber = lSQLRst.GetString(0).Trim(),
                                            Product = lProdDesc,
                                            TotalPCs = lTotalPCs,
                                            TotalTonnage = Math.Round(lTotalWT, 3),
                                            TotalM2 = Math.Round(lTotalM2, 3),
                                            SignedDO = lDocSignedDO,
                                            DODetails = lDocDODetail,
                                            MillCert = lDocMillCert
                                        });
                                    }
                                }
                            }
                            lSQLRst.Close();
                        }
                    }

                    //lDBConn.CloseCISConnection(ref lCISCon);
                    lDBConn.CloseNDSConnection(ref lNDSCon);
                    lDBConn = null;

                }
            }
            catch (Exception ex)
            {
                lErrorMsg = "Process Error: " + ex.Message;
                lSuccess = 0;
            }


            return Ok(lReturn);

        }

        //[HttpPost]
        //[Route("/testDocumentIndex")]
        //public async Task<List<DocumentDetails>> testDocumentIndex(TestDocumentIndexDto testDocumentIndexDto)
        //{
        //    string CustomerCode = testDocumentIndexDto.CustomerCode;
        //    string ProjectCode = testDocumentIndexDto.ProjectCode;
        //    string DONumber = testDocumentIndexDto.DONumber;
        //    string DocumentType = testDocumentIndexDto.DocumentType;
        //    string DODate = testDocumentIndexDto.DODate;
        //    var lRetNone = new List<DocumentDetails>();
        //    try
        //    {
        //        bool lReturn = true;
        //        string lErrorMsg = "";
        //        string lToken = "";
        //        string lRet = "";


        //        string lProdDashURL = "https://prodash.natsteel.com.sg:8080/project-document/v1/share-point/document";

        //        string FinalapiUrl = $"{lProdDashURL}/{CustomerCode}/{ProjectCode}/do/{DONumber}/{DocumentType}";

        //        HttpClient lClient = new HttpClient();

        //        lClient.BaseAddress = new Uri(FinalapiUrl);

        //        lClient.Timeout = TimeSpan.FromMinutes(1);

        //        lClient.DefaultRequestHeaders.Accept.Clear();
        //        lClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //        if (lToken.Length > 0)
        //        {
        //            lClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + lToken);
        //        }

        //        try
        //        {
        //            var client = new HttpClient();
        //            HttpResponseMessage response = await client.GetAsync(FinalapiUrl);

        //            if (response.IsSuccessStatusCode)
        //            {
        //                lRet = await response.Content.ReadAsStringAsync();
        //            }
        //            else
        //            {
        //                lErrorMsg = "Cannot connect to Document Portal";
        //                lReturn = false;
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            lErrorMsg = e.Message;
        //            lReturn = false;
        //        }
        //        lClient.Dispose();

        //        if (lReturn == true)
        //        {
        //            List<DocumentDetails> lRetDocs = new List<DocumentDetails>();


        //            List<DocumentDetails> documents = JsonConvert.DeserializeObject<List<DocumentDetails>>(lRet);

        //            if (DocumentType == "Mill Certificate")
        //            {
        //                DocumentType = "MillCert";
        //            }

        //            lRetDocs = documents.Where(p => p.DocumentName.Contains(DocumentType)).ToList();

        //            if (lRetDocs != null && lRetDocs.Count > 0)
        //            {
        //                if (DocumentType == "MillCert")
        //                {
        //                    List<DocumentDetails> lRetMillCertDocs = new List<DocumentDetails>();
        //                    for (int i = 0; i < lRetDocs.Count; i++)
        //                    {
        //                        if (lRetDocs[i].DocumentName.IndexOf(DONumber) >= 0)
        //                        {
        //                            lRetMillCertDocs.Add(lRetDocs[i]);
        //                        }
        //                    }
        //                    return lRetMillCertDocs;
        //                }
        //                else
        //                {
        //                    return lRetDocs;
        //                }
        //            }
        //            else
        //            {
        //                return lRetNone;
        //            }
        //        }
        //        else
        //        {
        //            return lRetNone;
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        return lRetNone;
        //    }
        //}

        [HttpGet]
        [Route("/downloadDocument/{DocumentIndex}/{FileName}/{FileType}")]
        public ActionResult downloadDocument(string DocumentIndex, string FileName, string FileType)
        {
            bool lReturn = true;
            string lErrorMsg = "";
            string lToken = "";
            string lNewGenURL = "https://natsteel.newgenbpmcloud.com/WebServiceCall/Download?InputXML=" + DocumentIndex + "&RequestType=download&ViewOption=N";
            byte[] lbPDF = new byte[] { };
            //HttpClient lClient = new HttpClient();
            WebClient lClient = new WebClient();
            try
            {
                lbPDF = lClient.DownloadData(lNewGenURL);
                string lCP = lClient.ResponseHeaders["Content-Disposition"];

                string lFileName = FileName;
                string lContentType = FileType;
                //if (lCP != null)
                //{
                //    var lDisposition = ContentDispositionHeaderValue.Parse(lCP);
                //    if (lDisposition != null && lDisposition.FileName != null)
                //    {
                //        lFileName = lDisposition.FileName.Replace("\"", "");
                //    }
                //}
                using (MemoryStream memoryStream = new MemoryStream(lbPDF))
                {
                    HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                    response.Content = new StreamContent(memoryStream);
                    response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                    response.Content.Headers.ContentDisposition.FileName = lFileName;
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

                    return File(response.Content.ReadAsByteArrayAsync().Result, "application/pdf");
                }

                return Ok(lbPDF);

            }
            catch (Exception e)
            {
                lErrorMsg = e.Message;
                lReturn = false;
            }


            return Ok(new { success = lReturn, message = lErrorMsg });

        }

        [HttpPost]
        [Route("/exportDeliveredOrdersToExcel")]
        public async Task<ActionResult> exportDeliveredOrdersToExcel()
        {

            //set kookie for customer and project
            string CustomerCode = Request.Form["CustomerCode"];
            string temp = Request.Form["ProjectCodes"];
            string tempAddress = Request.Form["AddressCodes"];
            string lUserName = Request.Form["UserName"];
            List<string> ProjectCodes = temp.Split(',').ToList();
            List<string> AddressCodes = tempAddress.Split(',').ToList();
            string RDate = Request.Form["RDate"];
            string lDeliveryDateFrom = "";
            string lDeliveryDateTo = "";
            string AllProjectsFlag = Request.Form["AllProjectsFlag"];
            bool AllProjects = AllProjectsFlag == "Y" ? true : false;


            string PONumber = ""; string PODate = ""; string DONumber = "";
            string WBS1 = ""; string WBS2 = ""; string WBS3 = ""; 
            string lPODateFrom = "";
            string lPODateTo = "";
            string lRDateFrom = "";
            string lRDateTo = "";

            if (PONumber == null) PONumber = "";
            if (DONumber == null) DONumber = "";
            if (PODate == null) PODate = "";
            if (RDate == null) RDate = "";
            if (WBS1 == null) WBS1 = "";
            if (WBS2 == null) WBS2 = "";
            if (WBS3 == null) WBS3 = "";

            // Delivery Date Range Filter (RDate)
            if (RDate == null) RDate = "";
            if (RDate.Trim().Length == 0 || RDate.IndexOf("to") <= 0)
            {
                lDeliveryDateFrom = "2000-01-01 00:00:00";
                lDeliveryDateTo = "2200-01-01 23:59:59";
            }
            else
            {
                lDeliveryDateFrom = RDate.Substring(0, RDate.IndexOf("to")).Trim();
                lDeliveryDateTo = RDate.Substring(RDate.IndexOf("to") + 2).Trim();
            }

            DateTime tmpDate;

            if (!DateTime.TryParse(lDeliveryDateFrom, out tmpDate))
                lDeliveryDateFrom = "2000-01-01 00:00:00";

            if (!DateTime.TryParse(lDeliveryDateTo, out tmpDate))
                lDeliveryDateTo = "2200-01-01 23:59:59";



            if (PODate.Trim().Length == 0 || PODate.IndexOf("to") <= 0)
            {
                lPODateFrom = "1900-01-01 00:00:00";
                lPODateTo = "2200-01-01 23:59:59";
            }
            else
            {
                lPODateFrom = PODate.Substring(0, PODate.IndexOf("to")).Trim();
                lPODateTo = PODate.Substring(PODate.IndexOf("to") + 2).Trim();
            }
            DateTime lDateV = new DateTime();
            if (DateTime.TryParse(lPODateFrom, out lDateV) != true)
            {
                lPODateFrom = "1900-01-01 00:00:00";
            }
            if (DateTime.TryParse(lPODateTo, out lDateV) != true)
            {
                lPODateTo = "2200-01-01 23:59:59";
            }

            if (RDate == null) RDate = "";
            if (RDate.Trim().Length == 0 || RDate.IndexOf("to") <= 0)
            {
                lRDateFrom = "2000-01-01 00:00:00";
                lRDateTo = "2200-01-01 23:59:59";
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
                lRDateTo = "2200-01-01 23:59:59";
            }

            ExcelPackage package = new ExcelPackage();
            ExcelWorksheet ws = package.Workbook.Worksheets.Add("Delivered Order List");

            int lRowNo = 1;
            ws.Column(1).Width = 5;         //"SNo\n序号";
            ws.Column(2).Width = 15;        //"WBS1\n楼座";
            ws.Column(3).Width = 10;        //"WBS2\n楼座";
            ws.Column(4).Width = 7;         //"WBS3\n楼座";
            ws.Column(5).Width = 14;        //"Structure Element";
            ws.Column(6).Width = 14;        //"Prod Type\n加工表号";
            ws.Column(7).Width = 22;        //"PO No\n加工表号";
            ws.Column(8).Width = 11;        //"PO Date\n订货日期";
            ws.Column(9).Width = 17;        //"Delivery Date\n到场日期";
            ws.Column(10).Width = 15;        //"Transport Mode\n运输类型";
            ws.Column(11).Width = 15;        //"Delivery Qty\n送货数量";
            ws.Column(12).Width = 15;        //"Delivery Weight\n送货重量";
            ws.Column(13).Width = 20;        //"Vehicle No.\n货车号码";
            ws.Column(14).Width = 16;        //"Vehicle Out Time\n货车离厂时间";
            ws.Column(15).Width = 20;        //DO No";
            ws.Column(16).Width = 20;        //BBS No";
            ws.Column(17).Width = 30;        //BBS Description";
            ws.Column(18).Width = 31;        //Address";
            ws.Column(19).Width = 32;        //Gate";


            ws.Row(2).Height = 30;
            ws.Cells[lRowNo, 1].Value = "SNo\n序号";
            ws.Cells[lRowNo, 2].Value = "WBS1\n楼座";
            ws.Cells[lRowNo, 3].Value = "WBS2\n楼层";
            ws.Cells[lRowNo, 4].Value = "WBS3\n分部";
            ws.Cells[lRowNo, 5].Value = "Structure Element\n构件";
            ws.Cells[lRowNo, 6].Value = "Product Type\n产品类型";
            ws.Cells[lRowNo, 7].Value = "PO No\n订单号码";
            ws.Cells[lRowNo, 8].Value = "PO Date\n订货日期";
            ws.Cells[lRowNo, 9].Value = "Delivery Date\n到场日期";
            ws.Cells[lRowNo, 10].Value = "Transport Mode\n运输类型";
            ws.Cells[lRowNo, 11].Value = "Delivery Qty\n送货数量";
            ws.Cells[lRowNo, 12].Value = "Delivery Weight\n送货重量 (MT)";
            ws.Cells[lRowNo, 13].Value = "Vehicle No.\n货车号码";
            ws.Cells[lRowNo, 14].Value = "Vehicle Out Time\n货车离厂时间";
            ws.Cells[lRowNo, 15].Value = "Delivery No\n交货编号";
            ws.Cells[lRowNo, 16].Value = "BBS No\n钢筋加工号码";
            ws.Cells[lRowNo, 17].Value = "BBS Description\n钢筋加工描述";
            ws.Cells[lRowNo, 18].Value = "Project Title\n项目名称";
            ws.Cells[lRowNo, 19].Value = "Submitted By\n提交者";
            ws.Cells[lRowNo, 20].Value = "Address\n地址";
            ws.Cells[lRowNo, 21].Value = "Gate\n门";

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
            ws.Cells[lRowNo, 16].Style.WrapText = true;
            ws.Cells[lRowNo, 17].Style.WrapText = true;
            ws.Cells[lRowNo, 18].Style.WrapText = true;
            ws.Cells[lRowNo, 19].Style.WrapText = true;
            ws.Cells[lRowNo, 20].Style.WrapText = true;
            ws.Cells[lRowNo, 21].Style.WrapText = true;

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
            ws.Cells[lRowNo, 16].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[lRowNo, 17].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[lRowNo, 18].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[lRowNo, 19].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[lRowNo, 20].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[lRowNo, 21].Style.Border.BorderAround(ExcelBorderStyle.Thin);

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
            ws.Cells[lRowNo, 16].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[lRowNo, 17].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[lRowNo, 18].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[lRowNo, 19].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[lRowNo, 20].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[lRowNo, 21].Style.Fill.PatternType = ExcelFillStyle.Solid;


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
            ws.Cells[lRowNo, 16].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));
            ws.Cells[lRowNo, 17].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));
            ws.Cells[lRowNo, 18].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));
            ws.Cells[lRowNo, 19].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));
            ws.Cells[lRowNo, 20].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));
            ws.Cells[lRowNo, 21].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));

            lRowNo = 2;

            var lCmd = new SqlCommand();
            SqlDataReader lRst;
            var lNDSCon = new SqlConnection();
            var lReturn = (new[]{ new
            {
                PONo = "",
                WBS1 = "",
                WBS2 = "",
                WBS3 = "",
                StructureElement = "",
                ProdType = "",
                PODate = "",
                RequiredDate = "",
                OrderWeight = "",
                DeliveryDate = "",
                TransportMode = "",
                LoadQty = "",
                LoadWT = "",
                VehicleNo = "",
                OutTime = "",
                DONo = "",
                BBSNo= "",
                BBSDesc = "",
                ProjectTitle="",
                SubmittedBy="",
                Address = "",
                Gate = ""
            }}).ToList();
            var j = 0;
            if (CustomerCode != null && ProjectCodes != null)

                for (int x = 0; x < ProjectCodes.Count(); x++)
                {
                    string ProjectCode = ProjectCodes[x];
                    

                    if (CustomerCode.Length > 0 && ProjectCode.Length > 0)
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
                                        DeliveredOrderProcess(CustomerCode, lProjects[i].ProjectCode, lUserName, lRDateFrom, lRDateTo);
                                        if (lProjectState == "")
                                        {
                                            lProjectState = "ProjectCode = '" + lProjects[i].ProjectCode + "' ";
                                        }
                                        else
                                        {
                                            lProjectState = lProjectState + " OR ProjectCode = '" + lProjects[i].ProjectCode + "' ";
                                        }
                                    }
                                }
                            }

                            if (lProjectState == "")
                            {
                                lProjectState = "AND 1 = 2 ";
                            }
                            else
                            {
                                lProjectState = "AND (" + lProjectState + ") ";
                            }
                        }
                        else
                        {
                            lProjectState = "AND ProjectCode = '" + ProjectCode + "' ";
                        }

                        if (AddressCodes != null && AddressCodes.Any())
                        {
                            var validCodes = AddressCodes
                                .Where(code => !string.IsNullOrWhiteSpace(code))
                                .Distinct() // optional: remove duplicates
                                .ToList();

                            if (validCodes.Any())
                            {
                                lAddressState =
                                    "AND EXISTS (SELECT 1 FROM dbo.OESProjOrder M " +
                                    "WHERE M.OrderNumber = D.DigiOSID " +
                                    "AND M.AddressCode IN ('" + string.Join("', '", validCodes) + "')) ";
                            }
                        }

                        lCmd.CommandText =
"SELECT  " +
"PONo, " +
"WBS1, " +
"WBS2, " +
"WBS3, " +
"StructureElement, " +
"ProductType, " +
"PODate, " +
"RequiredDate, " +
"OrderWT, " +
"LoadQty, " +
"LoadWT, " +
"Delivery_Date, " +
"TransportMode, " +
"Vehicle_No, " +
"Vehicle_out_time, " +

"DONo, " +
"BBSNo, " +
"(SELECT Max(BBSDesc) FROM dbo.OESBBS B, dbo.OESProjOrdersSE S " +
"WHERE B.JobID = S.CABJobID " +
"AND B.CustomerCode = D.CustomerCode " +
"AND B.ProjectCode = D.ProjectCode " +
"AND S.OrderNumber = D.DigiOSID " +
"AND B.BBSNo = D.BBSNo " +
") as BBSDesc, " +
"CustomerCode, " +
"ProjectCode, " +
"(SELECT ProjectTitle FROM dbo.OESProject " +
"WHERE CustomerCode = D.CustomerCode " +
"AND ProjectCode = D.ProjectCode) as ProjectTitle, " +
"PartialDelivery, " +
"(SELECT CASE WHEN M.OrderStatus = 'Sent' THEN M.SubmitBy ELSE M.UpdateBy END " +
"FROM dbo.OESProjOrder M WHERE M.OrderNumber = D.DigiOSID) as SubmittedBy, " +
"(SELECT TOP 1 M.Address FROM dbo.OESProjOrder M WHERE M.OrderNumber = D.DigiOSID) as Address, " +
"(SELECT TOP 1 M.Gate FROM dbo.OESProjOrder M WHERE M.OrderNumber = D.DigiOSID) as Gate " +
"FROM dbo.OESDeliveredOrders D " +
"WHERE CustomerCode = '" + CustomerCode + "' " +
"" + lProjectState + " " +
"" + lAddressState + " " +
"AND Delivery_Date >= '" + lDeliveryDateFrom + "' " +
"AND Delivery_Date <= '" + lDeliveryDateTo + "' " +
"ORDER BY Delivery_Date DESC ";


                        var lProcessObj = new ProcessController();
                        if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
                        {
                            lCmd.Connection = lNDSCon;
                            lCmd.CommandTimeout = 300;
                            lRst = await lCmd.ExecuteReaderAsync();
                            if (lRst.HasRows)
                            {
                                while (lRst.Read())
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
                                    ws.Cells[j + lRowNo, 16].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    ws.Cells[j + lRowNo, 17].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    ws.Cells[j + lRowNo, 18].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    ws.Cells[j + lRowNo, 19].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    ws.Cells[j + lRowNo, 20].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    ws.Cells[j + lRowNo, 21].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                                    ws.Cells[j + lRowNo, 1].Value = j + 1;
                                    ws.Cells[j + lRowNo, 2].Value = (lRst.GetValue(1) == DBNull.Value ? "" : lRst.GetString(1).Trim()); //"WBS1\n楼座";
                                    ws.Cells[j + lRowNo, 3].Value = (lRst.GetValue(2) == DBNull.Value ? "" : lRst.GetString(2).Trim()); //"WBS2\n楼层";
                                    ws.Cells[j + lRowNo, 4].Value = (lRst.GetValue(3) == DBNull.Value ? "" : lRst.GetString(3).Trim()); //"WBS3\n分部";
                                    ws.Cells[j + lRowNo, 5].Value = (lRst.GetValue(4) == DBNull.Value ? "" : lRst.GetString(4).Trim()); //"Structure Element\n分部";
                                    ws.Cells[j + lRowNo, 6].Value = (lRst.GetValue(5) == DBNull.Value ? "" : lRst.GetString(5)); //"Product Type\n订货日期";

                                    ws.Cells[j + lRowNo, 7].Value = (lRst.GetValue(0) == DBNull.Value ? "" : lRst.GetString(0).Trim()); //"PO No\n加工表号";

                                    var lValue = (lRst.GetValue(6) == DBNull.Value ? "" : lRst.GetDateTime(6).ToString("yyyy-MM-dd")); //"PO Date\n订货日期";
                                    var lDateTValue = DateTime.Now;
                                    if (DateTime.TryParse(lValue, out lDateTValue))
                                    {
                                        ws.Cells[j + lRowNo, 8].Value = lDateTValue;
                                        ws.Cells[j + lRowNo, 8].Style.Numberformat.Format = "yyyy-mm-dd"; ;
                                    }
                                    else
                                    {
                                        ws.Cells[j + lRowNo, 8].Value = lValue;
                                    }
                                    //ws.Cells[j + lRowNo, 8].Value = (lRst.GetValue(6) == DBNull.Value ? "" : lRst.GetDateTime(6).ToString("yyyy-MM-dd")); //"PO Date\n订货日期";

                                    lValue = (lRst.GetValue(11) == DBNull.Value ? "" : lRst.GetString(11)); //Delivery Date\n到场日期
                                    if (DateTime.TryParse(lValue, out lDateTValue))
                                    {
                                        ws.Cells[j + lRowNo, 9].Value = lDateTValue;
                                        ws.Cells[j + lRowNo, 9].Style.Numberformat.Format = "yyyy-mm-dd"; ;
                                    }
                                    else
                                    {
                                        ws.Cells[j + lRowNo, 9].Value = lValue;
                                    }

                                    //ws.Cells[j + lRowNo, 9].Value = (lRst.GetValue(11) == DBNull.Value ? "" : lRst.GetString(11)); //Delivery Date\n到场日期
                                    ws.Cells[j + lRowNo, 10].Value = (lRst.GetValue(12) == DBNull.Value ? "" : lRst.GetString(12)); //Transport Mode\n运输类型
                                    ws.Cells[j + lRowNo, 11].Value = (lRst.GetValue(9) == DBNull.Value ? 0 : lRst.GetInt32(9)); //load Qty\n送货
                                    ws.Cells[j + lRowNo, 12].Value = (lRst.GetValue(10) == DBNull.Value ? (decimal)0 : lRst.GetDecimal(10)); //load Weight\n送货重量
                                    ws.Cells[j + lRowNo, 12].Style.Numberformat.Format = "#,###,##0.000";

                                    ws.Cells[j + lRowNo, 13].Value = (lRst.GetValue(13) == DBNull.Value ? "" : lRst.GetString(13)); //Vehicle No.\n货车号码

                                    string lOutTime = lRst.GetValue(14) == DBNull.Value ? "" : lRst.GetString(14).Trim();

                                    lOutTime = lOutTime.Trim();

                                    var lOutTimeA = lOutTime.Split(',').ToList();
                                    if (lOutTimeA.Count > 0)
                                    {
                                        lOutTime = "";
                                        for (int i = 0; i < lOutTimeA.Count; i++)
                                        {
                                            lOutTimeA[i] = lOutTimeA[i].Trim();
                                            if (lOutTime == "")
                                            {
                                                lOutTime = (lOutTimeA[i].Length > 4 ? lOutTimeA[i].Substring(0, 2) + ":" + lOutTimeA[i].Substring(2, 2) : lOutTimeA[i]);
                                            }
                                            else
                                            {
                                                lOutTime = lOutTime + "," + (lOutTimeA[i].Length > 4 ? lOutTimeA[i].Substring(0, 2) + ":" + lOutTimeA[i].Substring(2, 2) : lOutTimeA[i]);
                                            }
                                        }
                                    }

                                    ws.Cells[j + lRowNo, 14].Value = lOutTime; //Vehicle Out Time\n货车离厂时间
                                    ws.Cells[j + lRowNo, 15].Value = (lRst.GetValue(15) == DBNull.Value ? "" : lRst.GetString(15).Trim()); //Do No

                                    ws.Cells[j + lRowNo, 16].Value = (lRst.GetValue(16) == DBNull.Value ? "" : lRst.GetString(16).Trim()); //BBS Number 钢筋加工号码
                                    ws.Cells[j + lRowNo, 17].Value = (lRst.GetValue(17) == DBNull.Value ? "" : lRst.GetString(17).Trim()); //BBS Description 钢筋加工描述
                                    ws.Cells[j + lRowNo, 18].Value = (lRst.GetValue(20) == DBNull.Value ? "" : lRst.GetString(20).Trim()); //Address
                                    ws.Cells[j + lRowNo, 19].Value = (lRst.GetValue(22) == DBNull.Value ? "" : lRst.GetString(22).Trim()); //Gate
                                    ws.Cells[j + lRowNo, 20].Value = (lRst.GetValue(23) == DBNull.Value ? "" : lRst.GetString(23).Trim()); //Gate
                                    ws.Cells[j + lRowNo, 21].Value = (lRst.GetValue(24) == DBNull.Value ? "" : lRst.GetString(24).Trim()); //Gate


                                    ws.Cells[j + lRowNo, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    ws.Cells[j + lRowNo, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    ws.Cells[j + lRowNo, 11].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                    ws.Cells[j + lRowNo, 12].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                    ws.Cells[j + lRowNo, 14].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                    ws.Cells[j + lRowNo, 15].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                    ws.Cells[j + lRowNo, 16].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                    ws.Cells[j + lRowNo, 17].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                    ws.Cells[j + lRowNo, 18].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                    ws.Cells[j + lRowNo, 19].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                    ws.Cells[j + lRowNo, 20].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; 
                                    ws.Cells[j + lRowNo, 21].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                    j = j + 1;


                                }
                            }
                            lRst.Close();

                            lProcessObj.CloseNDSConnection(ref lNDSCon);
                        }
                        lProcessObj = null;
                    }
                }
            {

            }
            lCmd = null;
            lNDSCon = null;
            lRst = null;

            //MemoryStream ms = new MemoryStream();
            //package.SaveAs(ms);

            //var bExcel = new byte[ms.Length];
            //ms.Position = 0;
            //ms.Read(bExcel, 0, bExcel.Length);

            ////bPDF = ms.GetBuffer();
            //ms.Flush();
            //ms.Dispose();

            //return Json(bExcel, JsonRequestBehavior.AllowGet);

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
        }


        [HttpPost]
        [Route("/testDocumentIndex")]
        public async Task<List<DocumentDetails>> testDocumentIndex([FromBody]TestDocumentIndexDto testDocumentIndexDto)
        {
            string CustomerCode = testDocumentIndexDto.CustomerCode;
            string ProjectCode = testDocumentIndexDto.ProjectCode;
            string DONumber = testDocumentIndexDto.DONumber;
            string DocumentType = testDocumentIndexDto.DocumentType;
            string DODate = testDocumentIndexDto.DODate;
            var lRetNone = new List<DocumentDetails>();
            try
            {
                bool lReturn = true;
                string lErrorMsg = "";
                string lToken = "";
                string lRet = "";


                //string lProdDashURL = "https://prodash.natsteel.com.sg:8080/project-document/v1/share-point/document";
                string lProdDashURL = "https://devappui.natsteel.com.sg:8080/project-document/v1/share-point/document";

                //string FinalapiUrl = $"{lProdDashURL}/{CustomerCode}/{ProjectCode}/do/{DONumber}/{DocumentType}";

                string FinalapiUrl = $"{lProdDashURL}/{CustomerCode}/{ProjectCode}/do/{DONumber}";


                HttpClient lClient = new HttpClient();

                lClient.BaseAddress = new Uri(FinalapiUrl);

                lClient.Timeout = TimeSpan.FromMinutes(1);

                lClient.DefaultRequestHeaders.Accept.Clear();
                lClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (lToken.Length > 0)
                {
                    lClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + lToken);
                }

                try
                {
                    var client = new HttpClient();
                    HttpResponseMessage response = await client.GetAsync(FinalapiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        lRet = await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        lErrorMsg = "Cannot connect to Document Portal";
                        lReturn = false;
                    }
                }
                catch (Exception e)
                {
                    lErrorMsg = e.Message;
                    lReturn = false;
                }
                lClient.Dispose();

                if (lReturn == true)
                {
                    List<DocumentDetails> lRetDocs = new List<DocumentDetails>();


                    List<DocumentDetails> documents = JsonConvert.DeserializeObject<List<DocumentDetails>>(lRet);

                    if (DocumentType == "Mill Certificate")
                    {
                        DocumentType = "MillCert";
                    }

                    lRetDocs = documents.Where(p => p.DocumentName.Contains(DocumentType)).ToList();

                    if (lRetDocs != null && lRetDocs.Count > 0)
                    {
                        if (DocumentType == "MillCert")
                        {
                            List<DocumentDetails> lRetMillCertDocs = new List<DocumentDetails>();
                            for (int i = 0; i < lRetDocs.Count; i++)
                            {
                                if (lRetDocs[i].DocumentName.IndexOf(DONumber) >= 0)
                                {
                                    lRetMillCertDocs.Add(lRetDocs[i]);
                                }
                            }
                            return lRetMillCertDocs;
                        }
                        else
                        {
                            return lRetDocs;
                        }
                    }
                    else
                    {
                        return lRetNone;
                    }
                }
                else
                {
                    return lRetNone;
                }
            }
            catch (Exception)
            {

                return lRetNone;
            }
        }
    }
}
