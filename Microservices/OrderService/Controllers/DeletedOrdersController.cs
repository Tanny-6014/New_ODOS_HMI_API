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
    public class DeletedOrdersController : Controller
    {
        private readonly IOrder _OrderRepository;
        private readonly IMapper _mapper;

        private DBContextModels db = new DBContextModels();


        public DeletedOrdersController(IOrder orderService, IMapper mapper)
        {
            _OrderRepository = orderService;
            _mapper = mapper;

        }

        //List of raised PO
        [HttpGet]
        [Route("/getDeletedOrders/{CustomerCode}/{ProjectCode}/{AllProjects}/{UserName}")]
        public async Task<ActionResult> getDeletedOrders(string CustomerCode, string ProjectCode,bool AllProjects, string UserName)
        {
            string lPODateFrom = "";
            string lPODateTo = "";
            string lRDateFrom = "";
            string lRDateTo = "";

            UserAccessController lUa = new UserAccessController();
            var lUserType = lUa.getUserType(UserName);
            var lGroupName = lUa.getGroupName(UserName);

          //  ViewBag.UserType = lUserType;

            string lUserName = UserName;

            lUa = null;

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

            lPODateFrom = lPODateFrom + " 00:00:00";
            lPODateTo = lPODateTo + " 23:59:59";

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

            lRDateFrom = lRDateFrom + " 00:00:00";
            lRDateTo = lRDateTo + " 00:00:00";

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
                OrderNo = 0,
                WBS1 = "",
                WBS2 = "",
                WBS3 = "",
                StructureElement = "",
                ProdType = "",
                PONo = "",
                UpdateDate = "",
                RequiredDate = "",
                OrderWeight = "",
                CustomerCode = "",
                ProjectCode = "",
                ProjectTitle = ""
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
                                        lProjectState = "M.ProjectCode = '" + lProjects[i].ProjectCode + "' ";
                                    }
                                    else
                                    {
                                        lProjectState = lProjectState + " OR M.ProjectCode = '" + lProjects[i].ProjectCode + "' ";
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
                        lProjectState = "AND M.ProjectCode = '" + ProjectCode + "' ";
                    }


                    lCmd.CommandText =
                    "SELECT " +
                    "M.OrderNumber, " +
                    "M.WBS1, " +
                    "M.WBS2, " +
                    "M.WBS3, " +
                    "(STUFF( " +
                    "(SELECT ',' + StructureElement " +
                    "FROM dbo.OESProjOrdersSE " +
                    "WHERE OrderNumber = M.OrderNumber " +
                    "GROUP BY StructureElement " +
                    "FOR XML PATH('')), 1, 1, ''))   " +
                    "AS StructureElement, " +
                    "(STUFF( " +
                    "(SELECT ',' + ProductType " +
                    "FROM dbo.OESProjOrdersSE " +
                    "WHERE OrderNumber = M.OrderNumber " +
                    "GROUP BY ProductType " +
                    "FOR XML PATH('')), 1, 1, ''))   " +
                    "AS ProductType, " +
                    "(STUFF( " +
                    "(SELECT ',' + PONumber " +
                    "FROM dbo.OESProjOrdersSE " +
                    "WHERE OrderNumber = M.OrderNumber " +
                    "GROUP BY PONumber " +
                    "FOR XML PATH('')), 1, 1, ''))   " +
                    "AS PONumber, " +
                    "M.UpdateDate,  " +
                    "(STUFF( " +
                    "(SELECT ',' + convert(varchar(10), RequiredDate, 120) " +
                    "FROM dbo.OESProjOrdersSE " +
                    "WHERE OrderNumber = M.OrderNumber " +
                    "GROUP BY convert(varchar(10), RequiredDate, 120) " +
                    "FOR XML PATH('')), 1, 1, ''))   " +
                    "AS RequiredDate, " +
                    "M.TotalWeight, " +
                    "M.CustomerCode, " +
                    "M.ProjectCode, " +
                    "(SELECT ProjectTitle FROM dbo.OESProject " +
                    "WHERE CustomerCode = M.CustomerCode " +
                    "AND ProjectCode = M.ProjectCode) as ProjectTitle " +
                    "FROM dbo.OESProjOrder M, dbo.OESProjOrdersSE S " +
                    "WHERE M.OrderNumber = S.OrderNumber " +
                    "AND M.CustomerCode = '" + CustomerCode + "' " +
                    "" + lProjectState + " " +
                    "AND M.OrderStatus = 'Deleted' " +
                    //"AND ((S.UpdateDate >= '" + lPODateFrom + "' " +
                    //"AND DATEADD(d,-1,S.UpdateDate) < '" + lPODateTo + "') " +
                    //"OR (S.UpdateDate is null AND '" + lPODateTo + "' = '2200-01-01 00:00:00' )) " +
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
                    "AND M.UpdateBy = '" + UserName + "' " +
                    "AND M.UpdateDate >= DateAdd(dd, -2, getDate()) " +
                    "GROUP BY " +
                    "M.OrderNumber, " +
                    "M.WBS1, " +
                    "M.WBS2, " +
                    "M.WBS3, " +
                    "M.UpdateDate, " +
                    "M.TotalWeight, " +
                    "M.CustomerCode, " +
                    "M.ProjectCode " +
                    "ORDER BY " +
                    "M.OrderNumber DESC ";

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
                                lReturn.Add(new
                                {
                                    SSNNo = lSNo,
                                    OrderNo = lRst.GetInt32(0),
                                    WBS1 = (lRst.GetValue(1) == DBNull.Value ? "" : lRst.GetString(1).Trim()),
                                    WBS2 = (lRst.GetValue(2) == DBNull.Value ? "" : lRst.GetString(2).Trim()),
                                    WBS3 = (lRst.GetValue(3) == DBNull.Value ? "" : lRst.GetString(3).Trim()),
                                    StructureElement = (lRst.GetValue(4) == DBNull.Value ? "" : (lRst.GetString(4).Trim() == "NONWBS" ? "" : lRst.GetString(4).Trim())),
                                    ProdType = (lRst.GetValue(5) == DBNull.Value ? "" : lRst.GetString(5).Trim()),
                                    PONo = (lRst.GetValue(6) == DBNull.Value ? "" : (lRst.GetString(6).Trim().Length > 0 && lRst.GetString(6).Trim().Substring(0, 1) == "," ? lRst.GetString(6).Trim().Substring(1) : lRst.GetString(6).Trim())),
                                    UpdateDate = (lRst.GetValue(7) == DBNull.Value ? "" : lRst.GetDateTime(7).ToString("yyyy-MM-dd")),
                                    RequiredDate = (lRst.GetValue(8) == DBNull.Value ? "" : (lRst.GetString(8).Substring(0, 1) == "," ? lRst.GetString(8).Substring(1) : lRst.GetString(8))),
                                    OrderWeight = (lRst.GetValue(9) == DBNull.Value ? "0.000" : (lRst.GetDecimal(9) / 1000).ToString("###,###,##0.000;(###,##0.000);")),
                                    CustomerCode = (lRst.GetValue(10) == DBNull.Value ? "" : lRst.GetString(10).Trim()),
                                    ProjectCode = (lRst.GetValue(11) == DBNull.Value ? "" : lRst.GetString(11).Trim()),
                                    ProjectTitle = (lRst.GetValue(12) == DBNull.Value ? "" : lRst.GetString(12).Trim())
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

        [HttpPost]

        [Route("/BatchChangeStatus")]
        public async Task<ActionResult> BatchChangeStatus([FromBody]  DeleteOrder deleteOrder)
        {
            //OrderStatus 1. Submitted, 2. Sent 4. Delete 
            OrderProcessAPIController lOrderProc = new OrderProcessAPIController();
            try
            {



                if (deleteOrder.pOrderNo.Count > 0)
                {
                    for (int i = 0; i < deleteOrder.pOrderNo.Count; i++)
                    {
                        var lReturnMsg = lOrderProc.StatusProcess(deleteOrder.pCustomerCode[i], deleteOrder.pProjectCode[i], deleteOrder.pOrderNo[i], deleteOrder.pOrderStatus, deleteOrder.UserName);//User.Identity.GetUserName() 1 added by ajit

                        if (lReturnMsg != "")
                        {
                            //return Json(new
                            //{
                            //    success = false,
                            //    responseText = lReturnMsg
                            //}, JsonRequestBehavior.AllowGet);
                            return Ok(lReturnMsg);

                        }
                    }
                }
                lOrderProc = null;

            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    success = false,
                    responseText = "Error:" + ex.Message
                });
                throw ex;
            }
            //return Ok(lOrderProc);

            return Ok(new { success = true });
        }


        [HttpPost]
        [Route("/exportDeletedOrdersToExcel")]
        public async Task<ActionResult> exportDeletedOrdersToExcel()
        {
            string CustomerCode = Request.Form["CustomerCode"];
            string temp = Request.Form["ProjectCodes"];
            string lUserName = Request.Form["UserName"];
            List<string> ProjectCodes = temp.Split(',').ToList();

            string PONumber = "";
            string PODate = "";
            string RDate = "";
            string WBS1 = "";
            string WBS2 = "";
            string WBS3 = "";
            bool AllProjects = false;
            string lPODateFrom = "";
            string lPODateTo = "";
            string lRDateFrom = "";
            string lRDateTo = "";

            UserAccessController lUa = new UserAccessController();
            var lUserType = lUa.getUserType(lUserName);
            var lGroupName = lUa.getGroupName(lUserName);

            ViewBag.UserType = lUserType;

            //string lUserName = lUserName;

            lUa = null;

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

            lPODateFrom = lPODateFrom + " 00:00:00";
            lPODateTo = lPODateTo + " 23:59:59";

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

            lRDateFrom = lRDateFrom + " 00:00:00";
            lRDateTo = lRDateTo + " 00:00:00";

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
            ExcelWorksheet ws = package.Workbook.Worksheets.Add("Deleted Order List");

            int lRowNo = 1;
            //ws.Column(1).Width = 5;         //"SNo\n序号";
            ws.Column(1).Width = 12;        //"Order No\n加工表号";
            ws.Column(2).Width = 10;        //"WBS1\n楼座";
            ws.Column(3).Width = 10;        //"WBS2\n楼座";
            ws.Column(4).Width = 7;         //"WBS3\n楼座";
            ws.Column(5).Width = 14;        //"Structure Element";
            ws.Column(6).Width = 14;        //"PO Type";
            ws.Column(7).Width = 14;        //"PO Number";
            ws.Column(8).Width = 11;        //"Update Date\n订货日期";
            ws.Column(9).Width = 15;        //"Required Date\n到场日期";
            ws.Column(10).Width = 16;       //"Order WT\n料单重量";

            ws.Row(1).Height = 30;
            ws.Cells[lRowNo, 1].Value = "SNo\n序号";
            //ws.Cells[lRowNo, 2].Value = "Order No\n订单号码";
            ws.Cells[lRowNo, 2].Value = "WBS1\n楼座";
            ws.Cells[lRowNo, 3].Value = "WBS2\n楼层";
            ws.Cells[lRowNo, 4].Value = "WBS3\n分部";
            ws.Cells[lRowNo, 5].Value = "Structure Element\n构件";
            ws.Cells[lRowNo, 6].Value = "Product Type\n产品类型";
            ws.Cells[lRowNo, 7].Value = "PO Number\n订单号码";
            ws.Cells[lRowNo, 8].Value = "Update Date\n修改日期";
            ws.Cells[lRowNo, 9].Value = "Required Date\n到场日期";
            ws.Cells[lRowNo, 10].Value = "Order Weight\n料单重量 (MT)";

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
            //ws.Cells[lRowNo, 11].Style.WrapText = true;

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
            //ws.Cells[lRowNo, 11].Style.Border.BorderAround(ExcelBorderStyle.Thin);

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
            //ws.Cells[lRowNo, 11].Style.Fill.PatternType = ExcelFillStyle.Solid;
            //ws.Cells[lRowNo, 12].Style.Fill.PatternType = ExcelFillStyle.Solid;
            //ws.Cells[lRowNo, 13].Style.Fill.PatternType = ExcelFillStyle.Solid;

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
            //ws.Cells[lRowNo, 11].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));
            //ws.Cells[lRowNo, 12].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));
            //ws.Cells[lRowNo, 13].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));

            lRowNo = 2;
            var lCmd = new SqlCommand();
            SqlDataReader lRst;
            var lNDSCon = new SqlConnection();

            if (CustomerCode != null && ProjectCodes != null)
            {
                var j = 0;
                for (int x = 0; x < ProjectCodes.Count(); x++)
                {
                    string ProjectCode = ProjectCodes[x];
                    if (CustomerCode.Length > 0 && ProjectCode.Length > 0)
                    {

                        var lProjectState = "";
                        if (AllProjects == true)
                        {
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
                                            lProjectState = "M.ProjectCode = '" + lProjects[i].ProjectCode + "' ";
                                        }
                                        else
                                        {
                                            lProjectState = lProjectState + " OR M.ProjectCode = '" + lProjects[i].ProjectCode + "' ";
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
                            lProjectState = "AND M.ProjectCode = '" + ProjectCode + "' ";
                        }


                        lCmd.CommandText =
                        "SELECT " +
                        "M.OrderNumber, " +
                        "M.WBS1, " +
                        "M.WBS2, " +
                        "M.WBS3, " +
                        "(STUFF( " +
                        "(SELECT ', ' + StructureElement " +
                        "FROM dbo.OESProjOrdersSE " +
                        "WHERE OrderNumber = M.OrderNumber " +
                        "GROUP BY StructureElement " +
                        "FOR XML PATH('')), 1, 1, ''))   " +
                        "AS StructureElement, " +
                        "(STUFF( " +
                        "(SELECT ', ' + ProductType " +
                        "FROM dbo.OESProjOrdersSE " +
                        "WHERE OrderNumber = M.OrderNumber " +
                        "GROUP BY ProductType " +
                        "FOR XML PATH('')), 1, 1, ''))   " +
                        "AS ProductType, " +
                        "(STUFF( " +
                        "(SELECT ', ' + PONumber " +
                        "FROM dbo.OESProjOrdersSE " +
                        "WHERE OrderNumber = M.OrderNumber " +
                        "GROUP BY PONumber " +
                        "FOR XML PATH('')), 1, 1, ''))   " +
                        "AS PONumber, " +
                        "M.UpdateDate,  " +
                        "(STUFF( " +
                        "(SELECT ', ' + convert(varchar(10), RequiredDate, 120) " +
                        "FROM dbo.OESProjOrdersSE " +
                        "WHERE OrderNumber = M.OrderNumber " +
                        "GROUP BY convert(varchar(10), RequiredDate, 120) " +
                        "FOR XML PATH('')), 1, 1, ''))   " +
                        "AS RequiredDate, " +
                        "M.TotalWeight " +
                        "FROM dbo.OESProjOrder M, dbo.OESProjOrdersSE S " +
                        "WHERE M.OrderNumber = S.OrderNumber " +
                        "AND M.CustomerCode = '" + CustomerCode + "' " +
                        "" + lProjectState + " " +
                        "AND M.OrderStatus = 'Deleted' " +
                        "AND ((S.PODate >= '" + lPODateFrom + "' " +
                        "AND DATEADD(d,-1,S.PODate) < '" + lPODateTo + "') " +
                        "OR (S.PODate is null AND '" + lPODateTo + "' = '2200-01-01 00:00:00' )) " +
                        "AND ((S.RequiredDate >= '" + lRDateFrom + "' " +
                        "AND DATEADD(d,-1,S.RequiredDate) < '" + lRDateTo + "') " +
                        "OR (S.RequiredDate is null AND '" + lRDateTo + "' = '2200-01-01 00:00:00' )) " +
                        "AND (S.PONumber like '%" + PONumber + "%' " +
                        "OR  (S.PONumber is null AND '" + PONumber + "' = '' )) " +
                        "AND (M.WBS1 like '%" + WBS1 + "%' " +
                        "OR (M.WBS1 is null AND '" + WBS1 + "' = '' )) " +
                        "AND (M.WBS2 like '%" + WBS2 + "%' " +
                        "OR (M.WBS2 is null AND '" + WBS2 + "' = '' )) " +
                        "AND (M.WBS3 like '%" + WBS3 + "%' " +
                        "OR (M.WBS3 is null AND '" + WBS3 + "' = '' )) " +
                        "AND M.UpdateBy = '" + lUserName + "' " +
                        "AND M.UpdateDate >= DateAdd(dd, -2, getDate()) " +
                        "GROUP BY " +
                        "M.OrderNumber, " +
                        "M.WBS1, " +
                        "M.WBS2, " +
                        "M.WBS3, " +
                        "M.UpdateDate, " +
                        "M.TotalWeight " +
                        "ORDER BY " +
                        "M.OrderNumber DESC ";

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
                                    //ws.Cells[j + lRowNo, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
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


                                    //ws.Cells[j + lRowNo, 1].Value = j + 1;
                                    ws.Cells[j + lRowNo, 1].Value = (lRst.GetValue(0) == DBNull.Value ? "" : lRst.GetInt32(0).ToString().Trim()); //"Order No\n加工表号";
                                    ws.Cells[j + lRowNo, 2].Value = (lRst.GetValue(1) == DBNull.Value ? "" : lRst.GetString(1).Trim()); //"WBS1\n楼座";
                                    ws.Cells[j + lRowNo, 3].Value = (lRst.GetValue(2) == DBNull.Value ? "" : lRst.GetString(2).Trim()); //"WBS2\n楼层";
                                    ws.Cells[j + lRowNo, 4].Value = (lRst.GetValue(3) == DBNull.Value ? "" : lRst.GetString(3).Trim()); //"WBS3\n分部";
                                    ws.Cells[j + lRowNo, 5].Value = (lRst.GetValue(4) == DBNull.Value ? "" : (lRst.GetString(4).Trim() == "NONWBS" ? "" : lRst.GetString(4).Trim())); //"Structure Element\n分部

                                    ws.Cells[j + lRowNo, 6].Value = (lRst.GetValue(5) == DBNull.Value ? "" : lRst.GetString(5)); //"Product Type\n订货日期";

                                    var lValue = lRst.GetValue(6) == DBNull.Value ? "" : (lRst.GetString(6).Trim().Length > 0 && lRst.GetString(6).Trim().Substring(0, 1) == "," ? lRst.GetString(6).Trim().Substring(1) : lRst.GetString(6).Trim()); //"Product No\n订货日期";
                                    var lDateTValue = DateTime.Now;
                                    if (DateTime.TryParse(lValue, out lDateTValue))
                                    {
                                        ws.Cells[j + lRowNo, 7].Value = lDateTValue;
                                        ws.Cells[j + lRowNo, 7].Style.Numberformat.Format = "yyyy-mm-dd"; ;
                                    }
                                    else
                                    {
                                        ws.Cells[j + lRowNo, 7].Value = lValue;
                                    }
                                    //ws.Cells[j + lRowNo, 7].Value = lRst.GetValue(6) == DBNull.Value ? "" : (lRst.GetString(6).Trim().Length > 0 && lRst.GetString(6).Trim().Substring(0, 1) == "," ? lRst.GetString(6).Trim().Substring(1) : lRst.GetString(6).Trim()); //"Product No\n订货日期";

                                    lValue = (lRst.GetValue(7) == DBNull.Value ? "" : lRst.GetDateTime(7).ToString("yyyy-MM-dd")); //"Update Date\n订货日期";
                                    if (DateTime.TryParse(lValue, out lDateTValue))
                                    {
                                        ws.Cells[j + lRowNo, 8].Value = lDateTValue;
                                        ws.Cells[j + lRowNo, 8].Style.Numberformat.Format = "yyyy-mm-dd"; ;
                                    }
                                    else
                                    {
                                        ws.Cells[j + lRowNo, 8].Value = lValue;
                                    }
                                    //ws.Cells[j + lRowNo, 8].Value = (lRst.GetValue(7) == DBNull.Value ? "" : lRst.GetDateTime(7).ToString("yyyy-MM-dd")); //"Update Date\n订货日期";

                                    ws.Cells[j + lRowNo, 9].Value = (lRst.GetValue(8) == DBNull.Value ? "" : (lRst.GetString(8).Substring(0, 1) == "," ? lRst.GetString(8).Substring(1) : lRst.GetString(8))); //"Required Date\n到场日期";
                                    ws.Cells[j + lRowNo, 10].Value = (lRst.GetValue(9) == DBNull.Value ? (decimal)0 : (lRst.GetDecimal(9) / 1000));//"Total weight\n订货日期";
                                    ws.Cells[j + lRowNo, 10].Style.Numberformat.Format = "#,###,##0.000";

                                    j = j + 1;

                                }
                            }
                            lRst.Close();

                            lProcessObj.CloseNDSConnection(ref lNDSCon);
                        }
                        lProcessObj = null;
                    }
                }

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

    }
}
