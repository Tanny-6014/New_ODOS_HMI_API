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
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Net.Http.Headers;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CancelledOrdersController : Controller
    {
        private readonly IOrder _OrderRepository;
        private readonly IMapper _mapper;

        private DBContextModels db = new DBContextModels();

        public CancelledOrdersController(IOrder orderService, IMapper mapper)
        {
            _OrderRepository = orderService;
            _mapper = mapper;

        }

        [HttpGet]
        [Route("/getCancelledOrders/{CustomerCode}/{ProjectCode}/{AllProjects}/{UserName}")]
        public async Task<ActionResult> getCancelledOrders(string CustomerCode, string ProjectCode, bool AllProjects,string UserName)
        {
            string lSQL = "";
            //set kookie for customer and project
            string lPODateFrom = "";
            string lPODateTo = "";
            string lRDateFrom = "";
            string lRDateTo = "";

            //if (PODate == null) PODate = "";
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
                BBSNo = "",
                BBSDesc = "",
                CustomerCode = "",
                ProjectCode = "",
                ProjectTitle = "",
                Confirmed = 0
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
                    if (AllProjects == true)
                    {
                        UserAccessController lUa = new UserAccessController();
                        var lUserType =lUa.getUserType(UserName);
                        var lGroupName =lUa.getGroupName(UserName);

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

                    #region Retrieve Data
                    lSQL =
                    "SELECT " +
                    "M.OrderNumber, " +
                    "M.WBS1, " +
                    "M.WBS2, " +
                    "M.WBS3, " +
                    "(STUFF( " +
                    "(SELECT ',' + isNull(StructureElement,'') " +
                    "FROM dbo.OESProjOrdersSE " +
                    "WHERE OrderNumber = M.OrderNumber " +
                    "GROUP BY StructureElement " +
                    "FOR XML PATH('')), 1, 1, ''))   " +
                    "AS StructureElement, " +       //4
                    "(STUFF( " +
                    "(SELECT ',' + isNull(ProductType,'') " +
                    "FROM dbo.OESProjOrdersSE " +
                    "WHERE OrderNumber = M.OrderNumber " +
                    "GROUP BY ProductType " +
                    "FOR XML PATH('')), 1, 1, ''))   " +
                    "AS ProductType, " +            //5
                    "(STUFF( " +
                    "(SELECT ',' + isNull(PONumber,'') " +
                    "FROM dbo.OESProjOrdersSE " +
                    "WHERE OrderNumber = M.OrderNumber " +
                    "GROUP BY PONumber " +
                    "FOR XML PATH('')), 1, 1, ''))   " +
                    "AS PONumber, " +               //6
                    "(STUFF( " +
                    "(SELECT ',' + isNull(convert(varchar(10), PODate, 120),'') " +
                    "FROM dbo.OESProjOrdersSE " +
                    "WHERE OrderNumber = M.OrderNumber " +
                    "GROUP BY convert(varchar(10), PODate, 120) " +
                    "FOR XML PATH('')), 1, 1, ''))   " +
                    "AS PODate, " +                 //7
                    "(STUFF( " +
                    "(SELECT ',' + isNull(convert(varchar(10), RequiredDate, 120),'') " +
                    "FROM dbo.OESProjOrdersSE " +
                    "WHERE OrderNumber = M.OrderNumber " +
                    "GROUP BY convert(varchar(10), RequiredDate, 120) " +
                    "FOR XML PATH('')), 1, 1, ''))   " +
                    "AS RequiredDate, " +           //8
                    "SUM(S.TotalWeight), " +             //9
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
                    "(SELECT ProjectTitle FROM dbo.OESProject " +
                    "WHERE CustomerCode = M.CustomerCode " +
                    "AND ProjectCode = M.ProjectCode) as ProjectTitle " +
                    "FROM dbo.OESProjOrder M, dbo.OESProjOrdersSE S " +
                    "WHERE M.OrderNumber = S.OrderNumber " +
                    "AND M.CustomerCode = '" + CustomerCode + "' " +
                    "AND M.ProjectCode IN (" + lProjectState + ") " +
                    "AND M.OrderStatus = 'Cancelled' " +
                    "AND NOT EXISTS (SELECT OrderNumber FROM dbo.OESProjOrder M1 " +
                    "WHERE OrderNumber <> M.OrderNumber AND PONumber = M.PONumber " +
                    "AND M1.OrderStatus <> 'Created' AND M1.OrderStatus <> 'New' " +
                    "AND M1.OrderStatus <> 'Sent') " +
                    "AND NOT EXISTS (SELECT ORD_REQ_NO FROM dbo.SAP_REQUEST_HDR " +
                    "WHERE PO_NUM = M.PONumber AND STATUS <> 'X') " +
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
                    //"AND M.UpdateDate >= '2019-07-01' " +
                    "GROUP BY " +
                    "M.CustomerCode, " +
                    "M.ProjectCode, " +
                    "S.CABJobID, " +
                    "S.StdMESHJobID, " +
                    "S.StdBarsJobID, " +
                    "S.CoilProdJobID, " +
                    "S.BPCJobID, " +
                    "M.OrderNumber, " +
                    "M.WBS1, " +
                    "M.WBS2, " +
                    "M.WBS3, " +
                    "M.TotalWeight, " +
                    "M.UpdateBy, " +
                    "M.SubmitBy, " +
                    "M.OrderStatus, " +
                    "S.SAPSOR " +
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
                                if (lUXSOR != "")
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
                                    BBSNo = (lRst.GetValue(13) == DBNull.Value ? "" : lRst.GetString(13).Trim()),
                                    BBSDesc = (lRst.GetValue(14) == DBNull.Value ? "" : lRst.GetString(14).Trim()),
                                    CustomerCode = (lRst.GetValue(16) == DBNull.Value ? "" : lRst.GetString(16).Trim()),
                                    ProjectCode = (lRst.GetValue(17) == DBNull.Value ? "" : lRst.GetString(17).Trim()),
                                    ProjectTitle = (lRst.GetValue(18) == DBNull.Value ? "" : lRst.GetString(18).Trim()),
                                    Confirmed = 0
                                });
                            }
                        }
                        lRst.Close();

                        lProcessObj.CloseNDSConnection(ref lNDSCon);
                    }

                    #endregion
                }
            }
            lCmd = null;
            lNDSCon = null;
            lRst = null;



            lReturn = (from p in lReturn
                       orderby p.RequiredDate descending
                       select p).ToList();


            return Ok(lReturn);
        }


        [HttpPost]
        [Route("/exportCancelledOrdersToExcel")]
        public async Task<ActionResult> exportCancelledOrdersToExcel()
        {

            string CustomerCode = Request.Form["CustomerCode"];
            string temp = Request.Form["ProjectCodes"];
            string lUserName = Request.Form["UserName"];
            List<string> ProjectCodes = temp.Split(',').ToList();



            string PONumber = ""; string PODate = ""; string RDate = "";
            string WBS1 = ""; string WBS2 = ""; string WBS3 = ""; bool AllProjects = false;
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
            ExcelWorksheet ws = package.Workbook.Worksheets.Add("Cancelled Order List");

            int lRowNo = 1;
            // ws.Column(1).Width = 5;         //"SNo\n序号";
            ws.Column(1).Width = 12;        //"Order No\n加工表号";
            ws.Column(2).Width = 10;        //"WBS1\n楼座";
            ws.Column(3).Width = 10;        //"WBS2\n楼座";
            ws.Column(4).Width = 7;         //"WBS3\n楼座";
            ws.Column(5).Width = 14;        //"Structure Element";
            ws.Column(6).Width = 14;        //"PO Type";
            ws.Column(7).Width = 14;        //"PO Number";
            ws.Column(8).Width = 14;        //"BBS Number";
            ws.Column(9).Width = 25;        //"BBS Desc";
            ws.Column(10).Width = 11;        //"PO Date\n订货日期";
            ws.Column(11).Width = 15;        //"Required Date\n到场日期";
            ws.Column(12).Width = 16;       //"Order WT\n料单重量";
            ws.Column(13).Width = 17;        //"Submitted By\n到场日期";

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

            lRowNo = 2;
            var lCmd = new SqlCommand();
            SqlDataReader lRst;
            var lNDSCon = new SqlConnection();

            var lCISCon = new OracleConnection();
            var lOraCmd = new OracleCommand();
            OracleDataReader lOraRst;

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
                            BBSNo = "",
                            BBSDesc = "",
                            Confirmed = 0
                        }}).ToList();

            if (CustomerCode != null && ProjectCodes != null)
            {
                for (int x = 0; x < ProjectCodes.Count(); x++)
                {
                    string ProjectCode = ProjectCodes[x];
                    if (CustomerCode.Length > 0 && ProjectCode.Length > 0)
                    {
                        if (CustomerCode != null && ProjectCode != null)
                        {
                            if (CustomerCode.Length > 0 && ProjectCode.Length > 0)
                            {
                                var lProjectState = "";
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


                                #region Retrieve Data
                                //lSQL =
                                //"SELECT " +
                                //"M.OrderNumber, " +
                                //"M.WBS1, " +
                                //"M.WBS2, " +
                                //"M.WBS3, " +
                                //"(STUFF( " +
                                //"(SELECT ',' + isNull(StructureElement,'') " +
                                //"FROM dbo.OESProjOrdersSE " +
                                //"WHERE OrderNumber = M.OrderNumber " +
                                //"GROUP BY StructureElement " +
                                //"FOR XML PATH('')), 1, 1, ''))   " +
                                //"AS StructureElement, " +
                                //"(STUFF( " +
                                //"(SELECT ',' + isNull(ProductType,'') " +
                                //"FROM dbo.OESProjOrdersSE " +
                                //"WHERE OrderNumber = M.OrderNumber " +
                                //"GROUP BY ProductType " +
                                //"FOR XML PATH('')), 1, 1, ''))   " +
                                //"AS ProductType, " +
                                //"(STUFF( " +
                                //"(SELECT ',' + isNull(PONumber,'') " +
                                //"FROM dbo.OESProjOrdersSE " +
                                //"WHERE OrderNumber = M.OrderNumber " +
                                //"GROUP BY PONumber " +
                                //"FOR XML PATH('')), 1, 1, ''))   " +
                                //"AS PONumber, " +
                                //"(STUFF( " +
                                //"(SELECT ',' + isNull(convert(varchar(10), PODate, 120),'') " +
                                //"FROM dbo.OESProjOrdersSE " +
                                //"WHERE OrderNumber = M.OrderNumber " +
                                //"GROUP BY convert(varchar(10), PODate, 120) " +
                                //"FOR XML PATH('')), 1, 1, ''))   " +
                                //"AS PODate, " +
                                //"(STUFF( " +
                                //"(SELECT ',' + isNull(convert(varchar(10), RequiredDate, 120),'') " +
                                //"FROM dbo.OESProjOrdersSE " +
                                //"WHERE OrderNumber = M.OrderNumber " +
                                //"GROUP BY convert(varchar(10), RequiredDate, 120) " +
                                //"FOR XML PATH('')), 1, 1, ''))   " +
                                //"AS RequiredDate, " +
                                //"M.TotalWeight, " +
                                //"case when M.OrderStatus = 'Sent' then M.SubmitBy else M.UpdateBy end as UpdateBy, " +
                                //"M.OrderStatus, " +
                                //"(STUFF ( " +
                                //" (" +
                                //" SELECT ',' + SAPSONo FROM " +

                                //" (SELECT SAPSONo as SAPSONo " +
                                //" FROM dbo.OESStdSheetJobAdvice " +
                                //" WHERE CustomerCode = M.CustomerCode AND " +
                                //" ProjectCode = M.ProjectCode AND " +
                                //"(JobID = S.StdMESHJobID " +
                                //"OR JobID = S.StdBarsJobID " +
                                //"OR JobID = S.CoilProdJobID) " +
                                //" GROUP BY SAPSONo " +

                                //" Union " +
                                //" select BBSSOR as SAPSONo " +
                                //" FROM  dbo.OESBBS " +
                                //" WHERE CustomerCode = M.CustomerCode AND " +
                                //" ProjectCode = M.ProjectCode AND " +
                                //" JobID = S.CABJobID " +
                                //" GROUP BY BBSSOR " +

                                //" Union " +
                                //" select BBSSORCoupler as SAPSONo " +
                                //" FROM  dbo.OESBBS " +
                                //" WHERE CustomerCode = M.CustomerCode AND " +
                                //" ProjectCode = M.ProjectCode AND " +
                                //" JobID = S.CABJobID " +
                                //" GROUP BY BBSSORCoupler " +

                                //" Union " +
                                //" select BBSSAPSO as SAPSONo " +
                                //" FROM  dbo.OESBBS " +
                                //" WHERE CustomerCode = M.CustomerCode AND " +
                                //" ProjectCode = M.ProjectCode AND " +
                                //" JobID = S.CABJobID " +
                                //" GROUP BY BBSSAPSO " +

                                //" Union " +
                                //" select sor_no as SAPSONo " +
                                //" FROM  dbo.OESBPCDetailsProc " +
                                //" WHERE CustomerCode = M.CustomerCode AND " +
                                //" ProjectCode = M.ProjectCode AND " +
                                //" JobID = S.BPCJobID " +
                                //" GROUP BY sor_no " +

                                //" ) as k " +
                                //" FOR XML PATH('')), 1, 1, '')) " +
                                //" AS SOR, " +
                                //"(STUFF( " +
                                //"(SELECT ',' + isNull(BBSNo, '') " +
                                //"FROM  dbo.OESBBS " +
                                //"WHERE CustomerCode =  M.CustomerCode " +
                                //"AND ProjectCode = M.ProjectCode " +
                                //"AND JobID = S.CABJobID " +
                                //"GROUP BY BBSNo FOR XML PATH('')), 1, 1, '')) as BBSNo, " +
                                //"(STUFF( " +
                                //"(SELECT ',' + isNull(BBSDesc, '') " +
                                //"FROM  dbo.OESBBS " +
                                //"WHERE CustomerCode =  M.CustomerCode " +
                                //"AND ProjectCode = M.ProjectCode " +
                                //"AND JobID = S.CABJobID " +
                                //"GROUP BY BBSDesc FOR XML PATH('')), 1, 1, '')) as BBSDesc, " +
                                //"S.SAPSOR as UXSOR " +
                                //"FROM dbo.OESProjOrder M, dbo.OESProjOrdersSE S " +
                                //"WHERE M.OrderNumber = S.OrderNumber " +
                                //"AND M.CustomerCode = '" + CustomerCode + "' " +
                                //"AND M.ProjectCode IN (" + lProjectState + ") " +
                                //"AND M.OrderStatus = 'Cancelled' " +
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
                                //"GROUP BY " +
                                //"M.CustomerCode, " +
                                //"M.ProjectCode, " +
                                //"S.CABJobID, " +
                                //"S.StdMESHJobID, " +
                                //"S.StdBarsJobID, " +
                                //"S.CoilProdJobID, " +
                                //"S.BPCJobID, " +
                                //"M.OrderNumber, " +
                                //"M.WBS1, " +
                                //"M.WBS2, " +
                                //"M.WBS3, " +
                                //"M.TotalWeight, " +
                                //"M.UpdateBy, " +
                                //"M.SubmitBy, " +
                                //"M.OrderStatus, " +
                                //"S.SAPSOR " +
                                //"ORDER BY " +
                                //"M.OrderNumber DESC ";

                                lSQL =
                    "SELECT " +
                    "M.OrderNumber, " +
                    "M.WBS1, " +
                    "M.WBS2, " +
                    "M.WBS3, " +
                    "(STUFF( " +
                    "(SELECT ',' + isNull(StructureElement,'') " +
                    "FROM dbo.OESProjOrdersSE " +
                    "WHERE OrderNumber = M.OrderNumber " +
                    "GROUP BY StructureElement " +
                    "FOR XML PATH('')), 1, 1, ''))   " +
                    "AS StructureElement, " +       //4
                    "(STUFF( " +
                    "(SELECT ',' + isNull(ProductType,'') " +
                    "FROM dbo.OESProjOrdersSE " +
                    "WHERE OrderNumber = M.OrderNumber " +
                    "GROUP BY ProductType " +
                    "FOR XML PATH('')), 1, 1, ''))   " +
                    "AS ProductType, " +            //5
                    "(STUFF( " +
                    "(SELECT ',' + isNull(PONumber,'') " +
                    "FROM dbo.OESProjOrdersSE " +
                    "WHERE OrderNumber = M.OrderNumber " +
                    "GROUP BY PONumber " +
                    "FOR XML PATH('')), 1, 1, ''))   " +
                    "AS PONumber, " +               //6
                    "(STUFF( " +
                    "(SELECT ',' + isNull(convert(varchar(10), PODate, 120),'') " +
                    "FROM dbo.OESProjOrdersSE " +
                    "WHERE OrderNumber = M.OrderNumber " +
                    "GROUP BY convert(varchar(10), PODate, 120) " +
                    "FOR XML PATH('')), 1, 1, ''))   " +
                    "AS PODate, " +                 //7
                    "(STUFF( " +
                    "(SELECT ',' + isNull(convert(varchar(10), RequiredDate, 120),'') " +
                    "FROM dbo.OESProjOrdersSE " +
                    "WHERE OrderNumber = M.OrderNumber " +
                    "GROUP BY convert(varchar(10), RequiredDate, 120) " +
                    "FOR XML PATH('')), 1, 1, ''))   " +
                    "AS RequiredDate, " +           //8
                    "SUM(S.TotalWeight), " +             //9
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
                    "(SELECT ProjectTitle FROM dbo.OESProject " +
                    "WHERE CustomerCode = M.CustomerCode " +
                    "AND ProjectCode = M.ProjectCode) as ProjectTitle " +
                    "FROM dbo.OESProjOrder M, dbo.OESProjOrdersSE S " +
                    "WHERE M.OrderNumber = S.OrderNumber " +
                    "AND M.CustomerCode = '" + CustomerCode + "' " +
                    "AND M.ProjectCode IN (" + lProjectState + ") " +
                    "AND M.OrderStatus = 'Cancelled' " +
                    "AND NOT EXISTS (SELECT OrderNumber FROM dbo.OESProjOrder M1 " +
                    "WHERE OrderNumber <> M.OrderNumber AND PONumber = M.PONumber " +
                    "AND M1.OrderStatus <> 'Created' AND M1.OrderStatus <> 'New' " +
                    "AND M1.OrderStatus <> 'Sent') " +
                    "AND NOT EXISTS (SELECT ORD_REQ_NO FROM dbo.SAP_REQUEST_HDR " +
                    "WHERE PO_NUM = M.PONumber AND STATUS <> 'X') " +
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
                    //"AND M.UpdateDate >= '2019-07-01' " +
                    "GROUP BY " +
                    "M.CustomerCode, " +
                    "M.ProjectCode, " +
                    "S.CABJobID, " +
                    "S.StdMESHJobID, " +
                    "S.StdBarsJobID, " +
                    "S.CoilProdJobID, " +
                    "S.BPCJobID, " +
                    "M.OrderNumber, " +
                    "M.WBS1, " +
                    "M.WBS2, " +
                    "M.WBS3, " +
                    "M.TotalWeight, " +
                    "M.UpdateBy, " +
                    "M.SubmitBy, " +
                    "M.OrderStatus, " +
                    "S.SAPSOR " +
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
                                            if (lUXSOR != "")
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
                                                BBSNo = (lRst.GetValue(13) == DBNull.Value ? "" : lRst.GetString(13).Trim()),
                                                BBSDesc = (lRst.GetValue(14) == DBNull.Value ? "" : lRst.GetString(14).Trim()),
                                                Confirmed = 0
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

                                #endregion


                            }
                        }


                    }
                }

            }

            lCmd = null;
            lNDSCon = null;
            lRst = null;

            lOraCmd = null;
            lCISCon = null;
            lOraRst = null;

            if (lReturn.Count > 1)
            {
                lReturn.RemoveAt(0);
            }

            lReturn = (from p in lReturn
                       orderby p.RequiredDate descending
                       select p).ToList();

            var j = 0;
            if (lReturn.Count > 0)
            {
                

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

                    ws.Cells[j + lRowNo, 10].Value = lReturn[i].PODate; //"PO Date\n订货日期";
                    ws.Cells[j + lRowNo, 11].Value = lReturn[i].RequiredDate; //"Required Date\n到场日期";
                    decimal lWT = 0;
                    decimal.TryParse(lReturn[i].OrderWeight, out lWT);

                    ws.Cells[j + lRowNo, 12].Value = lWT;//"Total weight\n订货日期";
                    ws.Cells[j + lRowNo, 12].Style.Numberformat.Format = "#,###,##0.000";
                    ws.Cells[j + lRowNo, 13].Value = lReturn[i].SubmittedBy; //"Updated by\n订货日期";
                    j = j + 1;

                }
            }

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
    }
}
