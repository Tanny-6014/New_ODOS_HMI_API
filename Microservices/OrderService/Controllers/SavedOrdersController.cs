using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
//using System.Web.Mvc;
//using Microsoft.AspNetCore;
using OrderService.Models;
//using AntiForgeryHeader.Helper;
using Microsoft.AspNet.Identity;
//using SAP.Middleware.Connector;
//using OrderService.NSAPConnector;
//using System.Data.OleDb;
//using System.Data.OracleClient;
//using Oracle.DataAccess.Client;
using System.Data.SqlClient;
using Oracle.ManagedDataAccess.Client;
using System.Text;
using System.Globalization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using OrderService.Repositories;
//using OrderService.NSAPConnector;
//using SAP.Middleware.Connector;
using System.Runtime.InteropServices;
using System.Security.Principal;
//using System.Web.Security;
using System.Threading.Tasks;
using System.Threading;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;
//using Microsoft.Ajax.Utilities;

using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using Microsoft.AspNetCore.Mvc;
using OrderService.Dtos;
//using System.ServiceModel.Channels;
//using SAPConnector;
using Newtonsoft.Json;
using System.Net.Http.Json;
using Azure;
//using Apitron.PDF.Kit.FixedLayout.Content;

using System.Web.Helpers;
using SAPConnector;
using OrderService.NDSBeam;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Rendering;





namespace OrderService.Controllers
{
    //[Authorize]
    public class SavedOrdersController : Controller
    {

        public string gUserType = "";
        public string gGroupName = "";

        struct struOrderList
        {
            public string OrderNo;
            public string OrderDesc;
        };

        private DBContextModels db = new DBContextModels();

        [HttpGet]
        [Route("/Index/{rCustomerCode}/{rProjectCode}")]
        public ActionResult Index(string rCustomerCode, string rProjectCode)
        {
            var lCmd = new SqlCommand();
            SqlDataReader lRst;
            var lNDSCon = new SqlConnection();
            string lSQL = "";

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

            try
            {
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

                string lLeadTimeProdType = "";
                string lLeadTime = "";
                string lHolidays = "";

                var lProcessObj = new ProcessController();
                lProcessObj.OpenNDSConnection(ref lNDSCon);

                lSQL = "SELECT ProductType, LeadTime FROM dbo.OESLeadTime ORDER BY ProductType ";
                lCmd.CommandText = lSQL;
                lCmd.Connection = lNDSCon;
                lCmd.CommandTimeout = 300;
                lRst = lCmd.ExecuteReader();
                if (lRst.HasRows)
                {
                    while (lRst.Read())
                    {
                        lLeadTimeProdType = lLeadTimeProdType + "," + lRst.GetString(0);
                        lLeadTime = lLeadTime + "," + lRst.GetInt32(1).ToString();
                    }
                    lLeadTimeProdType = lLeadTimeProdType.Substring(1);
                    lLeadTime = lLeadTime.Substring(1);
                }
                lRst.Close();

                lSQL = "SELECT Holiday FROM dbo.OESPublicHolidays ORDER BY Holiday ";
                lCmd.CommandText = lSQL;
                lCmd.Connection = lNDSCon;
                lCmd.CommandTimeout = 300;
                lRst = lCmd.ExecuteReader();
                if (lRst.HasRows)
                {
                    while (lRst.Read())
                    {
                        lHolidays = lHolidays + "," + lRst.GetString(0);
                    }
                    lHolidays = lHolidays.Substring(1);
                }
                lRst.Close();

                lProcessObj.CloseNDSConnection(ref lNDSCon);
                lProcessObj = null;

                ViewBag.Submission = lSubmission;
                ViewBag.Editable = lEditable;

                ViewBag.LeadTimeProdType = lLeadTimeProdType;
                ViewBag.LeadTime = lLeadTime;
                ViewBag.Holidays = lHolidays;

                ViewBag.AlertMessage = new List<string>();
                //var lSharedPrg = new SharedAPIController();
                //ViewBag.AlertMessage = lSharedPrg.getAlertMessage(rCustomerCode, rProjectCode, lUserName, lSubmission, lEditable);
                //lSharedPrg = null;
            }
            catch (Exception e)
            {
                var lErrorMsg = e.Message;
            }
            return View();
        }

        [HttpPost]
        [Route("/BatchChangeStatus_Data/{UserName}")]
        public async Task<ActionResult> BatchChangeStatus([FromBody] BatchChangeDto batchChangeDto, string UserName)
        {
            //OrderStatus 1. Submitted, 2. Sent 
            //Process Shared or Exclusive request
            try
            {
                if (batchChangeDto.pOrderStatus == "Shared" || batchChangeDto.pOrderStatus == "Exclusive")
                {
                    if (batchChangeDto.pOrderNo.Count > 0)
                    {
                        bool lShared = true;
                        if (batchChangeDto.pOrderStatus == "Exclusive")
                        {
                            lShared = false;
                        }
                        for (int i = 0; i < batchChangeDto.pOrderNo.Count; i++)
                        {
                            var lHeader = db.OrderProject.Find(batchChangeDto.pOrderNo[i]);
                            var lNewHeader = lHeader;
                            lNewHeader.OrderShared = lShared;
                            lNewHeader.UpdateDate = DateTime.Now;
                            lNewHeader.UpdateBy = UserName;
                            db.Entry(lHeader).CurrentValues.SetValues(lNewHeader);
                            db.SaveChanges();

                            if (batchChangeDto.pOrderStatus == "Shared")
                            {
                                var lUserID = "Ajit.Kamble@tatatechnologies.com";//UserName;
                                if (lUserID.Split('@')[1].ToLower() != "natsteel.com.sg" &&
                                lUserID.Split('@')[1].ToLower() != "easteel-services.com")
                                {
                                    var lEmailObj = new SendGridEmail();
                                    //lEmailObj.sendOrderActionEmail(lHeader.CustomerCode, lHeader.ProjectCode, lHeader.OrderNumber, batchChangeDto.pOrderStatus, lHeader.OrderStatus, lUserID, 1, "", "", "");//commented by ajit
                                    lEmailObj = null;

                                }

                            }
                        }
                    }
                }
                else
                {
                    if (batchChangeDto.pOrderStatus == "Submitted" || batchChangeDto.pOrderStatus == "Sent")
                    {
                        if (batchChangeDto.pOrderNo.Count > 0)
                        {
                            for (int i = 0; i < batchChangeDto.pOrderNo.Count; i++)
                            {
                                int lOrderNo = batchChangeDto.pOrderNo[i];
                                var lSE = (from p in db.OrderProjectSE
                                           where p.OrderNumber == lOrderNo
                                           select p).ToList();
                                if (lSE != null && lSE.Count > 0)
                                {
                                    for (int j = 0; j < lSE.Count; j++)
                                    {
                                        if (lSE[j].ProductType != "BPC" && lSE[j].ScheduledProd == "N" && lSE[j].TotalWeight == 0)
                                        {
                                            return Ok(new
                                            {
                                                success = false,
                                                responseText = "Incompleted order has been found (Ref No:" + batchChangeDto.pOrderNo + "). Please enter the order detail before submit it. \n\n发现未完成的订单. 请输入订单的数据再提交."
                                            });
                                        }
                                    }
                                }
                            }
                        }

                        if (batchChangeDto.pOrderNo.Count > 0)
                        {
                            for (int i = 0; i < batchChangeDto.pOrderNo.Count; i++)
                            {

                                int lCAB = 0;
                                int lOrderNo = batchChangeDto.pOrderNo[i];
                                var lSE = (from p in db.OrderProjectSE
                                           where p.OrderNumber == lOrderNo
                                           select p).ToList();
                                if (lSE != null && lSE.Count > 0)
                                {
                                    for (int j = 0; j < lSE.Count; j++)
                                    {
                                        UpdateWT(lSE[j].ProductType, lOrderNo);
                                        if (lSE[j].ProductType == "CAB")
                                        {
                                            lCAB = 1;
                                            break;
                                        }
                                    }
                                }
                                if (lCAB == 1)
                                {
                                    var lSharedAPI = new SharedAPIController();
                                    string lCheckResult = lSharedAPI.checkCABDetails(lOrderNo, UserName);
                                    if (lCheckResult != "")
                                    {
                                        return Ok(new
                                        {
                                            success = false,
                                            responseText = "Please settle Cut & Bend Order (Ref No:" + batchChangeDto.pOrderNo[i] + ") issue before submit the order: \n\n" + lCheckResult
                                        });
                                    }
                                }
                              
                            }
                        }
                    }

                    OrderProcessAPIController lOrderProc = new OrderProcessAPIController();

                    if (batchChangeDto.pOrderNo.Count > 0)
                    {
                        for (int i = 0; i < batchChangeDto.pOrderNo.Count; i++)
                        {
                            var lReturnMsg = lOrderProc.StatusProcess(batchChangeDto.pCustomerCode[i], batchChangeDto.pProjectCode[i], batchChangeDto.pOrderNo[i], batchChangeDto.pOrderStatus, UserName);

                            if (lReturnMsg != "")
                            {
                                return Ok(new
                                {
                                    success = false,
                                    responseText = lReturnMsg
                                });

                            }
                        }
                    }
                    lOrderProc = null;
                }
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    success = false,
                    responseText = "Error:" + ex.Message
                });
            }

            return Ok(new
            {
                success = true,
            });

        }


        [HttpGet]
        [Route("/getProject/{CustomerCode}/{UserName}")]
        public ActionResult getProject(string CustomerCode, string UserName)
        {
            string lUserType = "";
            string lGroupName = "";

            OracleDataReader lRst;
            var lCmd = new OracleCommand();
            var lcisCon = new OracleConnection();

            UserAccessController lUa = new UserAccessController();
            lUserType = lUa.getUserType(UserName);
            lGroupName = lUa.getGroupName(UserName);

            lUa = null;

            SharedAPIController lBackEnd = new SharedAPIController();

            var lProject = lBackEnd.getProject(CustomerCode, lUserType, lGroupName);

            return Ok(lProject);
        }


        //List of Used BBS No
        [HttpGet]
        [Route("/ExportPONoToExcel/{CustomerCode}/{ProjectCode}")]
        public ActionResult ExportPONoToExcel(string CustomerCode, string ProjectCode)
        {
            decimal lCABDelTotal = 0;
            decimal lSBDelTotal = 0;
            decimal lMESHDelTotal = 0;
            decimal lCAGEDelTotal = 0;
            decimal lCABOrderTotal = 0;
            decimal lSBOrderTotal = 0;
            decimal lMESHOrderTotal = 0;
            decimal lCAGEOrderTotal = 0;

            var lCompanyName = db.Customer.Find(CustomerCode).CustomerName;
            var lProjectTitle = db.Project.Find(CustomerCode, ProjectCode).ProjectTitle;

            var lCmd = new SqlCommand();
            SqlDataReader lRst;
            var lNDSCon = new SqlConnection();
            var lReturn = new List<POSumModels>();
            if (CustomerCode != null && ProjectCode != null)
            {
                if (CustomerCode.Length > 0 && ProjectCode.Length > 0)
                {
                    lCmd.CommandText =
                    "SELECT B.PONo, " +
                    "B.WBS1, " +
                    "B.WBS2, " +
                    "B.WBS3, " +
                    "B.PODate, " +
                    "B.RequiredDate, " +
                    "B.DeliveryDate, " +
                    "SUM(B.CABOrderWT), " +
                    "SUM(B.CABDeliveryWT), " +
                    "SUM(B.SBOrderWT), " +
                    "SUM(B.SBDeliveryWT), " +
                    "SUM(B.MESHOrderWT), " +
                    "SUM(B.MESHDeliveryWT), " +
                    "SUM(B.CAGEOrderWT), " +
                    "SUM(B.CAGEDeliveryWT), " +
                    "A.OrderStatus, " +
                    "A.Remarks, " +
                    "MIN(B.SSNNo) " +
                    "FROM dbo.OESBBSSum B left outer join dbo.OESJobAdvice A " +
                    "ON B.CustomerCode = A.CustomerCode " +
                    "AND B.ProjectCode = A.ProjectCode " +
                    "AND B.JobID = A.JobID " +
                    "WHERE B.CustomerCode = '" + CustomerCode + "' " +
                    "AND B.ProjectCode = '" + ProjectCode + "' " +
                    "GROUP BY " +
                    "B.PONo, " +
                    "B.WBS1, " +
                    "B.WBS2, " +
                    "B.WBS3, " +
                    "B.PODate, " +
                    "B.RequiredDate, " +
                    "B.DeliveryDate, " +
                    "A.OrderStatus, " +
                    "A.Remarks " +
                    "ORDER BY 18 ";

                    var lProcessObj = new ProcessController();
                    if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
                    {
                        lCmd.Connection = lNDSCon;
                        lCmd.CommandTimeout = 300;
                        lRst = lCmd.ExecuteReader();
                        if (lRst.HasRows)
                        {
                            var lSNo = 0;
                            while (lRst.Read())
                            {
                                lSNo = lSNo + 1;
                                var lStatus = (lRst.GetValue(15) == DBNull.Value ? "" : lRst.GetString(15));
                                if (lRst.GetValue(6) != DBNull.Value)
                                {
                                    lStatus = "Delivered";
                                }
                                lReturn.Add(new POSumModels()
                                {
                                    SSNNo = lSNo,
                                    PONo = lRst.GetString(0),
                                    WBS1 = lRst.GetString(1),
                                    WBS2 = lRst.GetString(2),
                                    WBS3 = lRst.GetString(3),
                                    PODate = (lRst.GetValue(4) == DBNull.Value ? "" : lRst.GetDateTime(4).ToString("yyyy-MM-dd")),
                                    RequiredDate = (lRst.GetValue(5) == DBNull.Value ? "" : lRst.GetDateTime(5).ToString("yyyy-MM-dd")),
                                    DeliveryDate = (lRst.GetValue(6) == DBNull.Value ? "" : lRst.GetDateTime(6).ToString("yyyy-MM-dd")),
                                    CABOrderWT = lRst.GetDecimal(7),
                                    CABDelWT = lRst.GetDecimal(8),
                                    SBOrderWT = lRst.GetDecimal(9),
                                    SBDelWT = lRst.GetDecimal(10),
                                    MESHOrderWT = lRst.GetDecimal(11),
                                    MESHDelWT = lRst.GetDecimal(12),
                                    CAGEOrderWT = lRst.GetDecimal(13),
                                    CAGEDelWT = lRst.GetDecimal(14),
                                    POStatus = lStatus,
                                    PORemarks = (lRst.GetValue(16) == DBNull.Value ? "" : lRst.GetString(16).Trim())
                                });

                                lCABOrderTotal = lCABOrderTotal + lRst.GetDecimal(7);
                                lCABDelTotal = lCABDelTotal + lRst.GetDecimal(8);
                                lSBOrderTotal = lSBOrderTotal + lRst.GetDecimal(9);
                                lSBDelTotal = lSBDelTotal + lRst.GetDecimal(10);
                                lMESHOrderTotal = lMESHOrderTotal + lRst.GetDecimal(11);
                                lMESHDelTotal = lMESHDelTotal + lRst.GetDecimal(12);
                                lCAGEOrderTotal = lCAGEOrderTotal + lRst.GetDecimal(13);
                                lCAGEDelTotal = lCAGEDelTotal + lRst.GetDecimal(14);
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

            ExcelPackage package = new ExcelPackage();
            ExcelWorksheet ws = package.Workbook.Worksheets.Add("CAB");

            ExcelSheetGenerationPO("CAB",
                ref ws,
                ref lReturn,
                lCABDelTotal,
                lSBDelTotal,
                lMESHDelTotal,
                lCAGEDelTotal,
                lCABOrderTotal,
                lSBOrderTotal,
                lMESHOrderTotal,
                lCAGEOrderTotal,
                lCompanyName,
                lProjectTitle);

            if (lSBOrderTotal > 0 || lSBDelTotal > 0)
            {
                ExcelWorksheet wsSB = package.Workbook.Worksheets.Add("SB");

                ExcelSheetGenerationPO("SB",
                    ref wsSB,
                    ref lReturn,
                    lCABDelTotal,
                    lSBDelTotal,
                    lMESHDelTotal,
                    lCAGEDelTotal,
                    lCABOrderTotal,
                    lSBOrderTotal,
                    lMESHOrderTotal,
                    lCAGEOrderTotal,
                    lCompanyName,
                    lProjectTitle);
            }

            if (lMESHOrderTotal > 0 || lMESHDelTotal > 0)
            {
                ExcelWorksheet wsMESH = package.Workbook.Worksheets.Add("MESH");

                ExcelSheetGenerationPO("MESH",
                    ref wsMESH,
                    ref lReturn,
                    lCABDelTotal,
                    lSBDelTotal,
                    lMESHDelTotal,
                    lCAGEDelTotal,
                    lCABOrderTotal,
                    lSBOrderTotal,
                    lMESHOrderTotal,
                    lCAGEOrderTotal,
                    lCompanyName,
                    lProjectTitle);
            }

            if (lCAGEOrderTotal > 0 || lCAGEDelTotal > 0)
            {
                ExcelWorksheet wsCAGE = package.Workbook.Worksheets.Add("CAGE");

                ExcelSheetGenerationPO("CAGE",
                    ref wsCAGE,
                    ref lReturn,
                    lCABDelTotal,
                    lSBDelTotal,
                    lMESHDelTotal,
                    lCAGEDelTotal,
                    lCABOrderTotal,
                    lSBOrderTotal,
                    lMESHOrderTotal,
                    lCAGEOrderTotal,
                    lCompanyName,
                    lProjectTitle);
            }

            MemoryStream ms = new MemoryStream();
            package.SaveAs(ms);

            var bExcel = new byte[ms.Length];
            ms.Position = 0;
            ms.Read(bExcel, 0, bExcel.Length);

            //bPDF = ms.GetBuffer();
            ms.Flush();
            ms.Dispose();

            return Ok(bExcel);
        }

        private void ExcelSheetGenerationPO(string ProdType,
            ref ExcelWorksheet ws,
            ref List<POSumModels> lReturn,
            decimal lCABDelTotal,
            decimal lSBDelTotal,
            decimal lMESHDelTotal,
            decimal lCAGEDelTotal,
            decimal lCABOrderTotal,
            decimal lSBOrderTotal,
            decimal lMESHOrderTotal,
            decimal lCAGEOrderTotal,
            string lCompanyName,
            string lProjectTitle
        )
        {
            int lColNo = 0;
            int lRowNo = 0;
            ws.Column(1).Width = 5;         //"SNo\n序号";
            ws.Column(2).Width = 12;        //"PO No\n加工表号";
            ws.Column(3).Width = 15;       //"Status\nStatus"
            ws.Column(4).Width = 10;        //"WBS1\n楼座";
            ws.Column(5).Width = 10;        //"WBS2\n楼座";
            ws.Column(6).Width = 7;         //"WBS3\n楼座";
            ws.Column(7).Width = 11;        //"PO Date\n订货日期";
            ws.Column(8).Width = 15;        //"Required Date\n到场日期";
            ws.Column(9).Width = 15;       //"Date Delivered\n实际到场日期"

            lColNo = 10;
            if (ProdType == "CAB")
            {
                ws.Column(lColNo).Width = 16;       //"CAB PO WT\n加工铁料单重量";
                ws.Column(lColNo + 1).Width = 16;       //"CAB Delivery WT\n加工铁来货重量"
                lColNo = lColNo + 2;
            }

            if (ProdType == "SB")
            {
                ws.Column(lColNo).Width = 16;       //"SB Delivery WT\n标直铁来货重量"
                ws.Column(lColNo + 1).Width = 16;       //"SB Delivery WT\n标直铁来货重量"
                lColNo = lColNo + 2;
            }

            if (ProdType == "MESH")
            {
                ws.Column(lColNo).Width = 15;       //"MESH Delivery WT\n标直铁来货重量"
                ws.Column(lColNo + 1).Width = 15;       //"MESH Delivery WT\n标直铁来货重量"
                lColNo = lColNo + 2;
            }

            if (ProdType == "CAGE")
            {
                ws.Column(lColNo).Width = 15;       //"CAGE Delivery WT\n标直铁来货重量"
                ws.Column(lColNo + 1).Width = 15;       //"CAGE Delivery WT\n标直铁来货重量"
                lColNo = lColNo + 2;
            }

            ws.Column(lColNo).Width = 20;       //"Remarks\n备注"

            string lColLetter = Convert.ToChar(64 + lColNo).ToString(); // A Ascii -- 65

            ws.Cells["A1:" + lColLetter + "1"].Merge = true;
            ws.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells[1, 1].Style.Font.Bold = true;
            ws.Cells[1, 1].Style.Font.Size = 12;
            ws.Cells[1, 1].Style.WrapText = true;
            if (ProdType == "CAB")
            {
                ws.Cells[1, 1].Value = "PO List for Cut and Bend Bars\n加工铁订单列表";
            }
            else if (ProdType == "SB")
            {
                ws.Cells[1, 1].Value = "PO List for Standard Length Bars\n标准直铁订单列表";
            }
            else if (ProdType == "MESH")
            {
                ws.Cells[1, 1].Value = "PO List for MESH\n铁网订单列表";
            }
            else if (ProdType == "CAGE")
            {
                ws.Cells[1, 1].Value = "PO List for Cages\n铁笼铁订单列表";
            }



            ws.Row(1).Height = 30;

            ws.Cells["A2:C2"].Merge = true;
            ws.Cells[2, 1].Value = "Company Name (公司名称):";
            ws.Cells[2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            ws.Cells[2, 1].Style.Font.Bold = true;
            ws.Cells["D2:" + lColLetter + "2"].Merge = true;
            ws.Cells[2, 4].Value = lCompanyName;
            ws.Cells[2, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            ws.Cells[2, 4].Style.Font.Bold = true;

            ws.Cells["A3:C3"].Merge = true;
            ws.Cells[3, 1].Value = "Project Title (工程项目名称):";
            ws.Cells[3, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            ws.Cells[3, 1].Style.Font.Bold = true;

            ws.Cells["D3:" + lColLetter + "3"].Merge = true;
            ws.Cells[3, 4].Value = lProjectTitle;
            ws.Cells[3, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            ws.Cells[3, 4].Style.Font.Bold = true;

            lRowNo = 4;

            if (ProdType == "CAB" && (lCABOrderTotal > 0 || lCABDelTotal > 0))
            {
                ws.Cells["A" + lRowNo.ToString() + ":C" + lRowNo.ToString() + ""].Merge = true;
                ws.Cells[lRowNo, 1].Value = "Cut & Bend Total (加工铁汇总):";
                ws.Cells[lRowNo, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells[lRowNo, 1].Style.Font.Bold = true;

                ws.Cells["D" + lRowNo.ToString() + ":E" + lRowNo.ToString()].Merge = true;
                ws.Cells[lRowNo, 4].Value = lCABDelTotal;
                ws.Cells[lRowNo, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells[lRowNo, 4].Style.Numberformat.Format = "###,###,###.000;(###,###,###.000); ";
                ws.Cells[lRowNo, 4].Style.Font.Bold = true;

                lRowNo = lRowNo + 1;
            }

            if ((ProdType == "CAB" || ProdType == "SB") && (lSBOrderTotal > 0 || lSBDelTotal > 0))
            {
                ws.Cells["A" + lRowNo.ToString() + ":C" + lRowNo.ToString() + ""].Merge = true;
                ws.Cells[lRowNo, 1].Value = "Standard Length Bars Total (标准直铁汇总):";
                ws.Cells[lRowNo, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells[lRowNo, 1].Style.Font.Bold = true;

                ws.Cells["D" + lRowNo.ToString() + ":E" + lRowNo.ToString()].Merge = true;
                ws.Cells[lRowNo, 4].Value = lSBDelTotal;
                ws.Cells[lRowNo, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells[lRowNo, 4].Style.Numberformat.Format = "###,###,###.000;(###,###,###.000); ";
                ws.Cells[lRowNo, 4].Style.Font.Bold = true;
                lRowNo = lRowNo + 1;
            }

            if ((ProdType == "CAB" || ProdType == "MESH") && (lMESHOrderTotal > 0 || lMESHDelTotal > 0))
            {
                ws.Cells["A" + lRowNo.ToString() + ":C" + lRowNo.ToString() + ""].Merge = true;
                ws.Cells[lRowNo, 1].Value = "MESH Total (铁网汇总):";
                ws.Cells[lRowNo, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells[lRowNo, 1].Style.Font.Bold = true;

                ws.Cells["D" + lRowNo.ToString() + ":E" + lRowNo.ToString()].Merge = true;
                ws.Cells[lRowNo, 4].Value = lMESHDelTotal;
                ws.Cells[lRowNo, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells[lRowNo, 4].Style.Numberformat.Format = "###,###,###.000;(###,###,###.000); ";
                ws.Cells[lRowNo, 4].Style.Font.Bold = true;
                lRowNo = lRowNo + 1;
            }

            if ((ProdType == "CAB" || ProdType == "CAGE") && (lCAGEOrderTotal > 0 || lCAGEDelTotal > 0))
            {
                ws.Cells["A" + lRowNo.ToString() + ":C" + lRowNo.ToString() + ""].Merge = true;
                ws.Cells[lRowNo, 1].Value = "Cage Total (铁笼汇总):";
                ws.Cells[lRowNo, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells[lRowNo, 1].Style.Font.Bold = true;

                ws.Cells["D" + lRowNo.ToString() + ":E" + lRowNo.ToString()].Merge = true;
                ws.Cells[lRowNo, 4].Value = lCAGEDelTotal;
                ws.Cells[lRowNo, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells[lRowNo, 4].Style.Numberformat.Format = "###,###,###.000;(###,###,###.000); ";
                ws.Cells[lRowNo, 4].Style.Font.Bold = true;
                lRowNo = lRowNo + 1;
            }

            if (lRowNo > 5)
            {
                ws.Cells["A" + lRowNo.ToString() + ":C" + lRowNo.ToString() + ""].Merge = true;
                ws.Cells[lRowNo, 1].Value = "Grand Total (总计):";
                ws.Cells[lRowNo, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells[lRowNo, 1].Style.Font.Bold = true;

                ws.Cells["D" + lRowNo.ToString() + ":E" + lRowNo.ToString()].Merge = true;
                ws.Cells[lRowNo, 4].Value = lCABDelTotal + lSBDelTotal + lMESHDelTotal + lCAGEDelTotal;
                ws.Cells[lRowNo, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells[lRowNo, 4].Style.Numberformat.Format = "###,###,###.000;(###,###,###.000); ";
                ws.Cells[lRowNo, 4].Style.Font.Bold = true;
                lRowNo = lRowNo + 1;
            }

            ws.Cells["F4" + ":" + lColLetter + (lRowNo - 1).ToString()].Merge = true;

            ExcelRange lrg = ws.Cells["A1:" + lColLetter + (lRowNo - 1).ToString()];
            lrg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            lrg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            lrg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            lrg.Style.Border.Right.Style = ExcelBorderStyle.Thin;

            ExcelRange lrg4 = ws.Cells["A" + lRowNo.ToString() + ":" + lColLetter + lRowNo.ToString() + ""];
            lrg4.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            lrg4.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            lrg4.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            lrg4.Style.Border.Right.Style = ExcelBorderStyle.Thin;

            lrg4.Style.Fill.PatternType = ExcelFillStyle.Solid;
            lrg4.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);

            lrg4.Style.WrapText = true;

            ws.Cells[lRowNo, 1].Value = "SNo\n序号";
            ws.Cells[lRowNo, 2].Value = "PO No\n订单号码";
            ws.Cells[lRowNo, 3].Value = "PO Status\n订单状态";
            ws.Cells[lRowNo, 4].Value = "WBS1\n楼座";
            ws.Cells[lRowNo, 5].Value = "WBS2\n楼层";
            ws.Cells[lRowNo, 6].Value = "WBS3\n分部";
            ws.Cells[lRowNo, 7].Value = "PO Date\n订货日期";
            ws.Cells[lRowNo, 8].Value = "Required Date\n到场日期";
            ws.Cells[lRowNo, 9].Value = "Date Delivered\n实际到场日期";

            lColNo = 10;
            if (ProdType == "CAB")
            {
                ws.Cells[lRowNo, lColNo].Value = "CAB PO Weight\n加工铁料单重量";
                ws.Cells[lRowNo, lColNo + 1].Value = "CAB Delivery Weight\n加工铁来货重量";
                lColNo = lColNo + 2;
            }
            if (ProdType == "SB")
            {
                ws.Cells[lRowNo, lColNo].Value = "Standard Bars PO Weight\n标直铁料单重量";
                ws.Cells[lRowNo, lColNo + 1].Value = "Standard Bars Delivery Weight\n标直铁来货重量";
                lColNo = lColNo + 2;
            }
            if (ProdType == "MESH")
            {
                ws.Cells[lRowNo, lColNo].Value = "MESH PO Weight\n铁网料单重量";
                ws.Cells[lRowNo, lColNo + 1].Value = "MESH Delivery Weight\n铁网来货重量";
                lColNo = lColNo + 2;
            }
            if (ProdType == "CAGE")
            {
                ws.Cells[lRowNo, lColNo].Value = "Cage PO Weight\n铁笼料单重量";
                ws.Cells[lRowNo, lColNo + 1].Value = "Cage Delivery Weight\n铁笼来货重量";
                lColNo = lColNo + 2;
            }
            ws.Cells[lRowNo, lColNo].Value = "Remarks\n备注";

            lRowNo = lRowNo + 1;

            if (lReturn.Count > 0)
            {
                var j = 0;
                for (int i = 0; i < lReturn.Count; i++)
                {
                    if ((ProdType == "CAB" && (lReturn[i].CABOrderWT > 0 || lReturn[i].CABDelWT > 0))
                        || (ProdType == "SB" && (lReturn[i].SBOrderWT > 0 || lReturn[i].SBDelWT > 0))
                        || (ProdType == "MESH" && (lReturn[i].MESHOrderWT > 0 || lReturn[i].MESHDelWT > 0))
                        || (ProdType == "CAGE" && (lReturn[i].CAGEOrderWT > 0 || lReturn[i].CAGEDelWT > 0))
                        )
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
                        lColNo = 10;
                        if (ProdType == "CAB" && (lReturn[i].CABOrderWT > 0 || lReturn[i].CABDelWT > 0))
                        {
                            ws.Cells[j + lRowNo, lColNo].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            ws.Cells[j + lRowNo, lColNo + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            ws.Cells[j + lRowNo, lColNo].Style.Numberformat.Format = "###,###.000;(###,###.000); ";
                            ws.Cells[j + lRowNo, lColNo + 1].Style.Numberformat.Format = "###,###.000;(###,###.000); ";
                            lColNo = lColNo + 2;
                        }

                        if (ProdType == "SB" && (lReturn[i].SBOrderWT > 0 || lReturn[i].SBDelWT > 0))
                        {
                            ws.Cells[j + lRowNo, lColNo].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            ws.Cells[j + lRowNo, lColNo + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            ws.Cells[j + lRowNo, lColNo].Style.Numberformat.Format = "###,###.000;(###,###.000); ";
                            ws.Cells[j + lRowNo, lColNo + 1].Style.Numberformat.Format = "###,###.000;(###,###.000); ";
                            lColNo = lColNo + 2;
                        }

                        if (ProdType == "MESH" && (lReturn[i].MESHOrderWT > 0 || lReturn[i].MESHDelWT > 0))
                        {
                            ws.Cells[j + lRowNo, lColNo].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            ws.Cells[j + lRowNo, lColNo + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            ws.Cells[j + lRowNo, lColNo].Style.Numberformat.Format = "###,###.000;(###,###.000); ";
                            ws.Cells[j + lRowNo, lColNo + 1].Style.Numberformat.Format = "###,###.000;(###,###.000); ";
                            lColNo = lColNo + 2;
                        }

                        if (ProdType == "CAGE" && (lReturn[i].CAGEOrderWT > 0 || lReturn[i].CAGEDelWT > 0))
                        {
                            ws.Cells[j + lRowNo, lColNo].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            ws.Cells[j + lRowNo, lColNo + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            ws.Cells[j + lRowNo, lColNo].Style.Numberformat.Format = "###,###.000;(###,###.000); ";
                            ws.Cells[j + lRowNo, lColNo + 1].Style.Numberformat.Format = "###,###.000;(###,###.000); ";
                            lColNo = lColNo + 2;
                        }
                        ws.Cells[j + lRowNo, lColNo].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[j + lRowNo, lColNo].Style.WrapText = true;

                        ws.Cells[j + lRowNo, 1].Value = j + 1;
                        ws.Cells[j + lRowNo, 2].Value = lReturn[i].PONo; //"PO No\n加工表号";
                        ws.Cells[j + lRowNo, 3].Value = lReturn[i].POStatus;
                        ws.Cells[j + lRowNo, 4].Value = lReturn[i].WBS1; //"WBS1\n楼座";
                        ws.Cells[j + lRowNo, 5].Value = lReturn[i].WBS2; //"WBS2\n楼层";
                        ws.Cells[j + lRowNo, 6].Value = lReturn[i].WBS3; //"WBS3\n分部";
                        ws.Cells[j + lRowNo, 7].Value = lReturn[i].PODate; //"PO Date\n订货日期";
                        ws.Cells[j + lRowNo, 8].Value = lReturn[i].RequiredDate; //"Required Date\n到场日期";
                        ws.Cells[j + lRowNo, 9].Value = lReturn[i].DeliveryDate; //"Date Delivered\n实际到场日期";
                        lColNo = 10;
                        if (ProdType == "CAB" && (lReturn[i].CABOrderWT > 0 || lReturn[i].CABDelWT > 0))
                        {
                            ws.Cells[j + lRowNo, lColNo].Value = lReturn[i].CABOrderWT; //"CAB PO WT\n加工铁料单重量";
                            ws.Cells[j + lRowNo, lColNo + 1].Value = lReturn[i].CABDelWT; //"CAB Delivery WT\n加工铁来货重量";
                            lColNo = lColNo + 2;
                        }

                        if (ProdType == "SB" && (lReturn[i].SBOrderWT > 0 || lReturn[i].SBDelWT > 0))
                        {
                            ws.Cells[j + lRowNo, lColNo].Value = lReturn[i].SBOrderWT;
                            ws.Cells[j + lRowNo, lColNo + 1].Value = lReturn[i].SBDelWT;
                            lColNo = lColNo + 2;
                        }

                        if (ProdType == "MESH" && (lReturn[i].MESHOrderWT > 0 || lReturn[i].MESHDelWT > 0))
                        {
                            ws.Cells[j + lRowNo, lColNo].Value = lReturn[i].MESHOrderWT;
                            ws.Cells[j + lRowNo, lColNo + 1].Value = lReturn[i].MESHDelWT;
                            lColNo = lColNo + 2;
                        }

                        if (ProdType == "CAGE" && (lReturn[i].CAGEOrderWT > 0 || lReturn[i].CAGEDelWT > 0))
                        {
                            ws.Cells[j + lRowNo, lColNo].Value = lReturn[i].CAGEOrderWT;
                            ws.Cells[j + lRowNo, lColNo + 1].Value = lReturn[i].CAGEDelWT;
                            lColNo = lColNo + 2;
                        }
                        ws.Cells[j + lRowNo, lColNo].Value = lReturn[i].PORemarks; //"Remarks\n备注";
                        j = j + 1;
                    }
                }
            }


        }


        //Check duplicated BBS No
        [HttpGet]
        [Route("/SaveBBSSum/{CustomerCode}/{ProjectCode}/{PONo}/{BBSNo}/{Remarks}")]
        public ActionResult SaveBBSSum(string CustomerCode, string ProjectCode, string PONo, string BBSNo, string Remarks)
        {
            var lCmd = new SqlCommand();
            var lNDSCon = new SqlConnection();
            var lReturn = "";
            if (BBSNo != null && ProjectCode != null)
            {
                if (BBSNo.Length > 0 && ProjectCode.Length > 0)
                {
                    var lRemarks = Remarks.Replace("'", "''");
                    lCmd.CommandText =
                        "UPDATE dbo.OESBBSSum " +
                        "SET Remarks = '" + lRemarks + "' " +
                        "WHERE CustomerCode = '" + CustomerCode + "' " +
                        "AND ProjectCode = '" + ProjectCode + "' " +
                        "AND PONo = '" + PONo + "' " +
                        "AND BBSNo = '" + BBSNo + "' ";

                    var lProcessObj = new ProcessController();
                    if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
                    {
                        lCmd.Connection = lNDSCon;
                        lCmd.CommandTimeout = 300;
                        lCmd.ExecuteNonQuery();
                        lProcessObj.CloseNDSConnection(ref lNDSCon);
                    }
                    lProcessObj = null;
                }
            }
            lCmd = null;
            lNDSCon = null;

            return Ok(lReturn);
        }


        //List of Used BBS No
        [HttpGet]
        [Route("/ExportBBSNoToExcel/{CustomerCode}/{ProjectCode}")]
        public ActionResult ExportBBSNoToExcel(string CustomerCode, string ProjectCode)
        {
            decimal lCABDelTotal = 0;
            decimal lSBDelTotal = 0;
            decimal lMESHDelTotal = 0;
            decimal lCAGEDelTotal = 0;
            decimal lCABOrderTotal = 0;
            decimal lSBOrderTotal = 0;
            decimal lMESHOrderTotal = 0;
            decimal lCAGEOrderTotal = 0;
            int lColNo = 0;
            int lRowNo = 0;

            var lCmd = new SqlCommand();
            SqlDataReader lRst;
            var lNDSCon = new SqlConnection();
            var lCompanyName = db.Customer.Find(CustomerCode).CustomerName;
            var lProjectTitle = db.Project.Find(CustomerCode, ProjectCode).ProjectTitle;
            var lProjectCodeMESH = db.Project.Find(CustomerCode, ProjectCode).ProjectCodeMESH;
            var lProjectCodeCage = db.Project.Find(CustomerCode, ProjectCode).ProjectCodeCage;

            if (lProjectCodeMESH == null) lProjectCodeMESH = "";
            if (lProjectCodeCage == null) lProjectCodeCage = "";
            lProjectCodeMESH = lProjectCodeMESH.Trim();
            lProjectCodeCage = lProjectCodeCage.Trim();

            var lReturn = new List<BBSSumModels>();
            if (CustomerCode != null && ProjectCode != null)
            {
                if (CustomerCode.Length > 0 && ProjectCode.Length > 0)
                {
                    lCmd.CommandText =
                        "SELECT * FROM dbo.OESBBSSum " +
                        "WHERE CustomerCode = '" + CustomerCode + "' " +
                        "AND ProjectCode = '" + ProjectCode + "' " +
                        "order by SSNNo ";

                    var lProcessObj = new ProcessController();
                    if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
                    {
                        lCmd.Connection = lNDSCon;
                        lCmd.CommandTimeout = 300;
                        lRst = lCmd.ExecuteReader();
                        if (lRst.HasRows)
                        {
                            while (lRst.Read())
                            {
                                lReturn.Add(new BBSSumModels()
                                {
                                    SSNNo = lRst.GetInt32(2),
                                    PONo = lRst.GetString(5),
                                    BBSNo = lRst.GetString(6),
                                    BBSDesc = lRst.GetString(7),
                                    WBS1 = lRst.GetString(8),
                                    WBS2 = lRst.GetString(9),
                                    WBS3 = lRst.GetString(10),
                                    StrucEle = lRst.GetString(11),
                                    PODate = (lRst.GetValue(12) == DBNull.Value ? "" : lRst.GetDateTime(12).ToString("yyyy-MM-dd")),
                                    RequiredDate = (lRst.GetValue(13) == DBNull.Value ? "" : lRst.GetDateTime(13).ToString("yyyy-MM-dd")),
                                    DeliveryDate = (lRst.GetValue(14) == DBNull.Value ? "" : lRst.GetDateTime(14).ToString("yyyy-MM-dd")),
                                    CABOrderWT = lRst.GetDecimal(15),
                                    CABDelWT = lRst.GetDecimal(16),
                                    SBOrderWT = lRst.GetDecimal(17),
                                    SBDelWT = lRst.GetDecimal(18),
                                    MESHOrderWT = lRst.GetDecimal(19),
                                    MESHDelWT = lRst.GetDecimal(20),
                                    CAGEOrderWT = lRst.GetDecimal(21),
                                    CAGEDelWT = lRst.GetDecimal(22),
                                    Remarks = lRst.GetString(24)
                                });
                                lCABOrderTotal = lCABOrderTotal + lRst.GetDecimal(15);
                                lCABDelTotal = lCABDelTotal + lRst.GetDecimal(16);
                                lSBOrderTotal = lSBOrderTotal + lRst.GetDecimal(17);
                                lSBDelTotal = lSBDelTotal + lRst.GetDecimal(18);
                                lMESHOrderTotal = lMESHOrderTotal + lRst.GetDecimal(19);
                                lMESHDelTotal = lMESHDelTotal + lRst.GetDecimal(20);
                                lCAGEOrderTotal = lCAGEOrderTotal + lRst.GetDecimal(21);
                                lCAGEDelTotal = lCAGEDelTotal + lRst.GetDecimal(22);
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

            if (lReturn.Count > 1)
            {
                lReturn.RemoveAt(0);
            }

            ExcelPackage package = new ExcelPackage();
            ExcelWorksheet ws = package.Workbook.Worksheets.Add("CAB");

            ExcelSheetGeneration("CAB",
                ref ws,
                ref lReturn,
                lCABDelTotal,
                lSBDelTotal,
                lMESHDelTotal,
                lCAGEDelTotal,
                lCABOrderTotal,
                lSBOrderTotal,
                lMESHOrderTotal,
                lCAGEOrderTotal,
                lCompanyName,
                lProjectTitle);

            if (lSBOrderTotal > 0 || lSBDelTotal > 0)
            {
                ExcelWorksheet wsSB = package.Workbook.Worksheets.Add("SB");

                ExcelSheetGeneration("SB",
                    ref wsSB,
                    ref lReturn,
                    lCABDelTotal,
                    lSBDelTotal,
                    lMESHDelTotal,
                    lCAGEDelTotal,
                    lCABOrderTotal,
                    lSBOrderTotal,
                    lMESHOrderTotal,
                    lCAGEOrderTotal,
                    lCompanyName,
                    lProjectTitle);
            }

            if (lMESHOrderTotal > 0 || lMESHDelTotal > 0)
            {
                ExcelWorksheet wsMESH = package.Workbook.Worksheets.Add("MESH");

                ExcelSheetGeneration("MESH",
                    ref wsMESH,
                    ref lReturn,
                    lCABDelTotal,
                    lSBDelTotal,
                    lMESHDelTotal,
                    lCAGEDelTotal,
                    lCABOrderTotal,
                    lSBOrderTotal,
                    lMESHOrderTotal,
                    lCAGEOrderTotal,
                    lCompanyName,
                    lProjectTitle);
            }

            if (lCAGEOrderTotal > 0 || lCAGEDelTotal > 0)
            {
                ExcelWorksheet wsCAGE = package.Workbook.Worksheets.Add("CAGE");

                ExcelSheetGeneration("CAGE",
                    ref wsCAGE,
                    ref lReturn,
                    lCABDelTotal,
                    lSBDelTotal,
                    lMESHDelTotal,
                    lCAGEDelTotal,
                    lCABOrderTotal,
                    lSBOrderTotal,
                    lMESHOrderTotal,
                    lCAGEOrderTotal,
                    lCompanyName,
                    lProjectTitle);
            }

            MemoryStream ms = new MemoryStream();
            package.SaveAs(ms);

            var bExcel = new byte[ms.Length];
            ms.Position = 0;
            ms.Read(bExcel, 0, bExcel.Length);

            //bPDF = ms.GetBuffer();
            ms.Flush();
            ms.Dispose();

            return Ok(bExcel);
        }

        private void ExcelSheetGeneration(string ProdType,
            ref ExcelWorksheet ws,
            ref List<BBSSumModels> lReturn,
            decimal lCABDelTotal,
            decimal lSBDelTotal,
            decimal lMESHDelTotal,
            decimal lCAGEDelTotal,
            decimal lCABOrderTotal,
            decimal lSBOrderTotal,
            decimal lMESHOrderTotal,
            decimal lCAGEOrderTotal,
            string lCompanyName,
            string lProjectTitle
        )
        {
            int lColNo = 0;
            int lRowNo = 0;
            ws.Column(1).Width = 5;         //"SNo\n序号";
            ws.Column(2).Width = 15;        //"BBS No\n加工表号";
            ws.Column(3).Width = 37;        // "BBS Description\n施工部位";
            ws.Column(4).Width = 8;        //"WBS1\n楼座";
            ws.Column(5).Width = 8;        //"WBS2\n楼座";
            ws.Column(6).Width = 7;         //"WBS3\n楼座";
            ws.Column(7).Width = 10;        //"Structure Element\n构件"
            ws.Column(8).Width = 11;        //"PO Date\n订货日期";
            ws.Column(9).Width = 11;        //"Required Date\n到场日期";
            ws.Column(10).Width = 15;       //"Date Delivered\n实际到场日期"
            lColNo = 11;
            if (ProdType == "CAB")
            {
                ws.Column(lColNo).Width = 16;       //"CAB PO WT\n加工铁料单重量";
                ws.Column(lColNo + 1).Width = 16;       //"CAB Delivery WT\n加工铁来货重量"
                lColNo = lColNo + 2;
            }

            if (ProdType == "SB")
            {
                ws.Column(lColNo).Width = 16;       //"SB Delivery WT\n标直铁来货重量"
                ws.Column(lColNo + 1).Width = 16;       //"SB Delivery WT\n标直铁来货重量"
                lColNo = lColNo + 2;
            }

            if (ProdType == "MESH")
            {
                ws.Column(lColNo).Width = 15;       //"MESH Delivery WT\n标直铁来货重量"
                ws.Column(lColNo + 1).Width = 15;       //"MESH Delivery WT\n标直铁来货重量"
                lColNo = lColNo + 2;
            }

            if (ProdType == "CAGE")
            {
                ws.Column(lColNo).Width = 15;       //"CAGE Delivery WT\n标直铁来货重量"
                ws.Column(lColNo + 1).Width = 15;       //"CAGE Delivery WT\n标直铁来货重量"
                lColNo = lColNo + 2;
            }

            ws.Column(lColNo).Width = 20;       //"Remarks\n备注"

            string lColLetter = Convert.ToChar(64 + lColNo).ToString(); // A Ascii -- 65

            ws.Cells["A1:" + lColLetter + "1"].Merge = true;
            ws.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells[1, 1].Style.Font.Bold = true;
            ws.Cells[1, 1].Style.Font.Size = 12;
            ws.Cells[1, 1].Style.WrapText = true;
            if (ProdType == "CAB")
            {
                ws.Cells[1, 1].Value = "BBS List for Cut and Bend Bars\n加工铁订单详细列表";
            }
            else if (ProdType == "SB")
            {
                ws.Cells[1, 1].Value = "BBS List for Standard Length Bars\n标准直铁订单详细列表";
            }
            else if (ProdType == "MESH")
            {
                ws.Cells[1, 1].Value = "BBS List for MESH\n铁网订单详细列表";
            }
            else if (ProdType == "CAGE")
            {
                ws.Cells[1, 1].Value = "BBS List for Cages\n铁笼铁订单详细列表";
            }



            ws.Row(1).Height = 30;

            ws.Cells["A2:C2"].Merge = true;
            ws.Cells[2, 1].Value = "Company Name (公司名称):";
            ws.Cells[2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            ws.Cells[2, 1].Style.Font.Bold = true;
            ws.Cells["D2:" + lColLetter + "2"].Merge = true;
            ws.Cells[2, 4].Value = lCompanyName;
            ws.Cells[2, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            ws.Cells[2, 4].Style.Font.Bold = true;

            ws.Cells["A3:C3"].Merge = true;
            ws.Cells[3, 1].Value = "Project Title (工程项目名称):";
            ws.Cells[3, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            ws.Cells[3, 1].Style.Font.Bold = true;

            ws.Cells["D3:" + lColLetter + "3"].Merge = true;
            ws.Cells[3, 4].Value = lProjectTitle;
            ws.Cells[3, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            ws.Cells[3, 4].Style.Font.Bold = true;

            lRowNo = 4;

            if (ProdType == "CAB" && (lCABOrderTotal > 0 || lCABDelTotal > 0))
            {
                ws.Cells["A" + lRowNo.ToString() + ":C" + lRowNo.ToString() + ""].Merge = true;
                ws.Cells[lRowNo, 1].Value = "Cut & Bend Total (加工铁汇总):";
                ws.Cells[lRowNo, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells[lRowNo, 1].Style.Font.Bold = true;

                ws.Cells["D" + lRowNo.ToString() + ":E" + lRowNo.ToString()].Merge = true;
                ws.Cells[lRowNo, 4].Value = lCABDelTotal;
                ws.Cells[lRowNo, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells[lRowNo, 4].Style.Numberformat.Format = "###,###,###.000;(###,###,###.000); ";
                ws.Cells[lRowNo, 4].Style.Font.Bold = true;

                lRowNo = lRowNo + 1;
            }

            if ((ProdType == "CAB" || ProdType == "SB") && (lSBOrderTotal > 0 || lSBDelTotal > 0))
            {
                ws.Cells["A" + lRowNo.ToString() + ":C" + lRowNo.ToString() + ""].Merge = true;
                ws.Cells[lRowNo, 1].Value = "Standard Length Bars Total (标准直铁汇总):";
                ws.Cells[lRowNo, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells[lRowNo, 1].Style.Font.Bold = true;

                ws.Cells["D" + lRowNo.ToString() + ":E" + lRowNo.ToString()].Merge = true;
                ws.Cells[lRowNo, 4].Value = lSBDelTotal;
                ws.Cells[lRowNo, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells[lRowNo, 4].Style.Numberformat.Format = "###,###,###.000;(###,###,###.000); ";
                ws.Cells[lRowNo, 4].Style.Font.Bold = true;
                lRowNo = lRowNo + 1;
            }

            if ((ProdType == "CAB" || ProdType == "MESH") && (lMESHOrderTotal > 0 || lMESHDelTotal > 0))
            {
                ws.Cells["A" + lRowNo.ToString() + ":C" + lRowNo.ToString() + ""].Merge = true;
                ws.Cells[lRowNo, 1].Value = "MESH Total (铁网汇总):";
                ws.Cells[lRowNo, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells[lRowNo, 1].Style.Font.Bold = true;

                ws.Cells["D" + lRowNo.ToString() + ":E" + lRowNo.ToString()].Merge = true;
                ws.Cells[lRowNo, 4].Value = lMESHDelTotal;
                ws.Cells[lRowNo, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells[lRowNo, 4].Style.Numberformat.Format = "###,###,###.000;(###,###,###.000); ";
                ws.Cells[lRowNo, 4].Style.Font.Bold = true;
                lRowNo = lRowNo + 1;
            }

            if ((ProdType == "CAB" || ProdType == "CAGE") && (lCAGEOrderTotal > 0 || lCAGEDelTotal > 0))
            {
                ws.Cells["A" + lRowNo.ToString() + ":C" + lRowNo.ToString() + ""].Merge = true;
                ws.Cells[lRowNo, 1].Value = "Cage Total (铁笼汇总):";
                ws.Cells[lRowNo, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells[lRowNo, 1].Style.Font.Bold = true;

                ws.Cells["D" + lRowNo.ToString() + ":E" + lRowNo.ToString()].Merge = true;
                ws.Cells[lRowNo, 4].Value = lCAGEDelTotal;
                ws.Cells[lRowNo, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells[lRowNo, 4].Style.Numberformat.Format = "###,###,###.000;(###,###,###.000); ";
                ws.Cells[lRowNo, 4].Style.Font.Bold = true;
                lRowNo = lRowNo + 1;
            }

            if (lRowNo > 5)
            {
                ws.Cells["A" + lRowNo.ToString() + ":C" + lRowNo.ToString() + ""].Merge = true;
                ws.Cells[lRowNo, 1].Value = "Grand Total (总计):";
                ws.Cells[lRowNo, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells[lRowNo, 1].Style.Font.Bold = true;

                ws.Cells["D" + lRowNo.ToString() + ":E" + lRowNo.ToString()].Merge = true;
                ws.Cells[lRowNo, 4].Value = lCABDelTotal + lSBDelTotal + lMESHDelTotal + lCAGEDelTotal;
                ws.Cells[lRowNo, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells[lRowNo, 4].Style.Numberformat.Format = "###,###,###.000;(###,###,###.000); ";
                ws.Cells[lRowNo, 4].Style.Font.Bold = true;
                lRowNo = lRowNo + 1;
            }

            ws.Cells["F4" + ":" + lColLetter + (lRowNo - 1).ToString()].Merge = true;

            ExcelRange lrg = ws.Cells["A1:" + lColLetter + (lRowNo - 1).ToString()];
            lrg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            lrg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            lrg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            lrg.Style.Border.Right.Style = ExcelBorderStyle.Thin;

            ExcelRange lrg4 = ws.Cells["A" + lRowNo.ToString() + ":" + lColLetter + lRowNo.ToString() + ""];
            lrg4.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            lrg4.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            lrg4.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            lrg4.Style.Border.Right.Style = ExcelBorderStyle.Thin;

            lrg4.Style.Fill.PatternType = ExcelFillStyle.Solid;
            lrg4.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);

            lrg4.Style.WrapText = true;

            ws.Cells[lRowNo, 1].Value = "SNo\n序号";
            ws.Cells[lRowNo, 2].Value = "BBS No\n加工表号";
            ws.Cells[lRowNo, 3].Value = "BBS Description\n施工部位";
            ws.Cells[lRowNo, 4].Value = "WBS1\n楼座";
            ws.Cells[lRowNo, 5].Value = "WBS2\n楼层";
            ws.Cells[lRowNo, 6].Value = "WBS3\n分部";
            ws.Cells[lRowNo, 7].Value = "Structure Element\n构件";
            ws.Cells[lRowNo, 8].Value = "PO Date\n订货日期";
            ws.Cells[lRowNo, 9].Value = "Required Date\n到场日期";
            ws.Cells[lRowNo, 10].Value = "Date Delivered\n实际到场日期";

            lColNo = 11;
            if (ProdType == "CAB")
            {
                ws.Cells[lRowNo, lColNo].Value = "CAB PO Weight\n加工铁料单重量";
                ws.Cells[lRowNo, lColNo + 1].Value = "CAB Delivery Weight\n加工铁来货重量";
                lColNo = lColNo + 2;
            }
            if (ProdType == "SB")
            {
                ws.Cells[lRowNo, lColNo].Value = "Standard Bars PO Weight\n标直铁料单重量";
                ws.Cells[lRowNo, lColNo + 1].Value = "Standard Bars Delivery Weight\n标直铁来货重量";
                lColNo = lColNo + 2;
            }
            if (ProdType == "MESH")
            {
                ws.Cells[lRowNo, lColNo].Value = "MESH PO Weight\n铁网料单重量";
                ws.Cells[lRowNo, lColNo + 1].Value = "MESH Delivery Weight\n铁网来货重量";
                lColNo = lColNo + 2;
            }
            if (ProdType == "CAGE")
            {
                ws.Cells[lRowNo, lColNo].Value = "Cage PO Weight\n铁笼料单重量";
                ws.Cells[lRowNo, lColNo + 1].Value = "Cage Delivery Weight\n铁笼来货重量";
                lColNo = lColNo + 2;
            }
            ws.Cells[lRowNo, lColNo].Value = "Remarks\n备注";

            lRowNo = lRowNo + 1;

            if (lReturn.Count > 0)
            {
                var j = 0;
                for (int i = 0; i < lReturn.Count; i++)
                {
                    if ((ProdType == "CAB" && (lReturn[i].CABOrderWT > 0 || lReturn[i].CABDelWT > 0))
                        || (ProdType == "SB" && (lReturn[i].SBOrderWT > 0 || lReturn[i].SBDelWT > 0))
                        || (ProdType == "MESH" && (lReturn[i].MESHOrderWT > 0 || lReturn[i].MESHDelWT > 0))
                        || (ProdType == "CAGE" && (lReturn[i].CAGEOrderWT > 0 || lReturn[i].CAGEDelWT > 0))
                        )
                    {
                        ws.Cells[j + lRowNo, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[j + lRowNo, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[j + lRowNo, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[j + lRowNo, 3].Style.WrapText = true;
                        ws.Cells[j + lRowNo, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[j + lRowNo, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[j + lRowNo, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[j + lRowNo, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[j + lRowNo, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[j + lRowNo, 9].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[j + lRowNo, 10].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        lColNo = 11;
                        if (ProdType == "CAB" && (lReturn[i].CABOrderWT > 0 || lReturn[i].CABDelWT > 0))
                        {
                            ws.Cells[j + lRowNo, lColNo].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            ws.Cells[j + lRowNo, lColNo + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            ws.Cells[j + lRowNo, lColNo].Style.Numberformat.Format = "###,###.000;(###,###.000); ";
                            ws.Cells[j + lRowNo, lColNo + 1].Style.Numberformat.Format = "###,###.000;(###,###.000); ";
                            lColNo = lColNo + 2;
                        }

                        if (ProdType == "SB" && (lReturn[i].SBOrderWT > 0 || lReturn[i].SBDelWT > 0))
                        {
                            ws.Cells[j + lRowNo, lColNo].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            ws.Cells[j + lRowNo, lColNo + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            ws.Cells[j + lRowNo, lColNo].Style.Numberformat.Format = "###,###.000;(###,###.000); ";
                            ws.Cells[j + lRowNo, lColNo + 1].Style.Numberformat.Format = "###,###.000;(###,###.000); ";
                            lColNo = lColNo + 2;
                        }

                        if (ProdType == "MESH" && (lReturn[i].MESHOrderWT > 0 || lReturn[i].MESHDelWT > 0))
                        {
                            ws.Cells[j + lRowNo, lColNo].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            ws.Cells[j + lRowNo, lColNo + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            ws.Cells[j + lRowNo, lColNo].Style.Numberformat.Format = "###,###.000;(###,###.000); ";
                            ws.Cells[j + lRowNo, lColNo + 1].Style.Numberformat.Format = "###,###.000;(###,###.000); ";
                            lColNo = lColNo + 2;
                        }

                        if (ProdType == "CAGE" && (lReturn[i].CAGEOrderWT > 0 || lReturn[i].CAGEDelWT > 0))
                        {
                            ws.Cells[j + lRowNo, lColNo].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            ws.Cells[j + lRowNo, lColNo + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            ws.Cells[j + lRowNo, lColNo].Style.Numberformat.Format = "###,###.000;(###,###.000); ";
                            ws.Cells[j + lRowNo, lColNo + 1].Style.Numberformat.Format = "###,###.000;(###,###.000); ";
                            lColNo = lColNo + 2;
                        }
                        ws.Cells[j + lRowNo, lColNo].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[j + lRowNo, lColNo].Style.WrapText = true;


                        var lBBSDesc = lReturn[i].BBSDesc;
                        if (lReturn[i].SBDelWT > 0 || (lReturn[i].SBOrderWT > 0 || lReturn[i].MESHDelWT > 0 || lReturn[i].MESHOrderWT > 0))
                        {
                            lBBSDesc = lBBSDesc.Replace(", ", "\n");
                            lBBSDesc = lBBSDesc.Replace(",", "\n");
                        }
                        ws.Cells[j + lRowNo, 1].Value = j + 1;
                        ws.Cells[j + lRowNo, 2].Value = lReturn[i].BBSNo; //"BBS No\n加工表号";
                        ws.Cells[j + lRowNo, 3].Value = lBBSDesc; //"BBS Description\n施工部位";
                        ws.Cells[j + lRowNo, 4].Value = lReturn[i].WBS1; //"WBS1\n楼座";
                        ws.Cells[j + lRowNo, 5].Value = lReturn[i].WBS2; //"WBS2\n楼层";
                        ws.Cells[j + lRowNo, 6].Value = lReturn[i].WBS3; //"WBS3\n分部";
                        ws.Cells[j + lRowNo, 7].Value = lReturn[i].StrucEle; //"Structure Element\n构件";
                        ws.Cells[j + lRowNo, 8].Value = lReturn[i].PODate; //"PO Date\n订货日期";
                        ws.Cells[j + lRowNo, 9].Value = lReturn[i].RequiredDate; //"Required Date\n到场日期";
                        ws.Cells[j + lRowNo, 10].Value = lReturn[i].DeliveryDate; //"Date Delivered\n实际到场日期";
                        lColNo = 11;
                        if (ProdType == "CAB" && (lReturn[i].CABOrderWT > 0 || lReturn[i].CABDelWT > 0))
                        {
                            ws.Cells[j + lRowNo, lColNo].Value = lReturn[i].CABOrderWT; //"CAB PO WT\n加工铁料单重量";
                            ws.Cells[j + lRowNo, lColNo + 1].Value = lReturn[i].CABDelWT; //"CAB Delivery WT\n加工铁来货重量";
                            lColNo = lColNo + 2;
                        }

                        if (ProdType == "SB" && (lReturn[i].SBOrderWT > 0 || lReturn[i].SBDelWT > 0))
                        {
                            ws.Cells[j + lRowNo, lColNo].Value = lReturn[i].SBOrderWT;
                            ws.Cells[j + lRowNo, lColNo + 1].Value = lReturn[i].SBDelWT;
                            lColNo = lColNo + 2;
                        }

                        if (ProdType == "MESH" && (lReturn[i].MESHOrderWT > 0 || lReturn[i].MESHDelWT > 0))
                        {
                            ws.Cells[j + lRowNo, lColNo].Value = lReturn[i].MESHOrderWT;
                            ws.Cells[j + lRowNo, lColNo + 1].Value = lReturn[i].MESHDelWT;
                            lColNo = lColNo + 2;
                        }

                        if (ProdType == "CAGE" && (lReturn[i].CAGEOrderWT > 0 || lReturn[i].CAGEDelWT > 0))
                        {
                            ws.Cells[j + lRowNo, lColNo].Value = lReturn[i].CAGEOrderWT;
                            ws.Cells[j + lRowNo, lColNo + 1].Value = lReturn[i].CAGEDelWT;
                            lColNo = lColNo + 2;
                        }
                        ws.Cells[j + lRowNo, lColNo].Value = lReturn[i].Remarks; //"Remarks\n备注";
                        j = j + 1;
                    }
                }
            }


        }

        private int BBSSumInsert(string CustomerCode, string ProjectCode, SqlConnection lNDSCon, OracleDataReader lOraRst, DataTable lDTOrder, DataTable lDTBackup, int lSSNNo)
        {
            int lReturn = 0;
            string lSQL = "";

            string lPONo = "";
            string lPODate = "";
            string lWBS1 = "";
            string lWBS2 = "";
            string lWBS3 = "";
            string lStEle = "";
            string lProdType = "";
            string lBBSNo = "";
            string lBBSDesc = "";
            string lReqDate = "";
            string lLoadDate = "";
            string lSONo = "";
            decimal lDelQty = 0;

            decimal lCABDelQty = 0;
            decimal lSBDelQty = 0;
            decimal lMESHDelQty = 0;
            decimal lCAGEDelQty = 0;

            decimal lCABPOQty = 0;
            decimal lSBPOQty = 0;
            decimal lMESHPOQty = 0;
            decimal lCAGEPOQty = 0;

            string lRemarks = "";

            int lJobID = 0;
            int lBBSID = 0;

            var lCmd = new SqlCommand();

            lPONo = (lOraRst.GetString(0) == null ? "" : lOraRst.GetString(0).Trim());
            lPODate = (lOraRst.GetString(1) == null ? "20000101" : lOraRst.GetString(1).Trim());
            lWBS1 = (lOraRst.GetString(2) == null ? "" : lOraRst.GetString(2).Trim());
            lWBS2 = (lOraRst.GetString(3) == null ? "" : lOraRst.GetString(3).Trim());
            lWBS3 = (lOraRst.GetString(4) == null ? "" : lOraRst.GetString(4).Trim());
            lStEle = (lOraRst.GetString(5) == null ? "" : lOraRst.GetString(5).Trim());
            lProdType = (lOraRst.GetString(6) == null ? "" : lOraRst.GetString(6).Trim());
            lBBSNo = (lOraRst.GetString(7) == null ? "" : lOraRst.GetString(7).Trim());
            lBBSDesc = (lOraRst.GetString(8) == null ? "" : lOraRst.GetString(8).Trim());
            lReqDate = (lOraRst.GetString(9) == null ? "20000101" : lOraRst.GetString(9).Trim());
            lLoadDate = (lOraRst.GetString(10) == null ? "20000101" : lOraRst.GetString(10).Trim());
            lDelQty = lOraRst.GetDecimal(11);
            lSONo = (lOraRst.GetString(12) == null ? "" : lOraRst.GetString(12).Trim());

            if (lPODate == "") { lPODate = "20000101"; }
            if (lReqDate == "") { lReqDate = "20000101"; }
            if (lLoadDate == "") { lLoadDate = "20000101"; }

            lCABDelQty = 0;
            lSBDelQty = 0;
            lMESHDelQty = 0;
            lCAGEDelQty = 0;

            lCABPOQty = 0;
            lSBPOQty = 0;
            lMESHPOQty = 0;
            lCAGEPOQty = 0;

            if (lProdType == "CAB")
            {
                lCABDelQty = lDelQty;
            }
            if (lProdType == "SB")
            {
                lSBDelQty = lDelQty;
            }
            if (lProdType == "MSH")
            {
                lMESHDelQty = lDelQty;
            }
            if (lProdType == "PRC")
            {
                lCAGEDelQty = lDelQty;
            }


            DataRow[] lOrder = lDTOrder.Select("PONumber='" + lPONo + "' AND BBSNoNDS='" + lBBSNo + "'");
            if (lOrder == null || lOrder.Length == 0)
            {
                lOrder = lDTOrder.Select("PONumber='" + lPONo + "' AND BBSNo='" + lBBSNo + "'");
            }

            lJobID = 0;
            lBBSID = 0;
            if (lOrder != null && lOrder.Length > 0)
            {
                lJobID = (int)lOrder[0].ItemArray[2];
                lBBSID = (int)lOrder[0].ItemArray[11];
                lCABPOQty = (decimal)lOrder[0].ItemArray[15];
                lSBPOQty = (decimal)lOrder[0].ItemArray[16];
                if (lWBS1.Length == 0 && lOrder[0].ItemArray[8] != DBNull.Value)
                {
                    lWBS1 = (string)lOrder[0].ItemArray[8];
                }
                if (lWBS2.Length == 0 && lOrder[0].ItemArray[9] != DBNull.Value)
                {
                    lWBS2 = (string)lOrder[0].ItemArray[9];
                }
                if (lWBS3.Length == 0 && lOrder[0].ItemArray[10] != DBNull.Value)
                {
                    lWBS3 = (string)lOrder[0].ItemArray[10];
                }
                if (lStEle.Length == 0 && lOrder[0].ItemArray[14] != DBNull.Value)
                {
                    lStEle = (string)lOrder[0].ItemArray[14];
                }
            }

            if (lCABDelQty > 0 && lCABPOQty == 0)
            {
                lCABPOQty = lCABDelQty;
            }
            if (lSBDelQty > 0 && lSBPOQty == 0)
            {
                lSBPOQty = lSBDelQty;
            }
            if (lMESHDelQty > 0 && lMESHPOQty == 0)
            {
                lMESHPOQty = lMESHDelQty;
            }
            if (lCAGEDelQty > 0 && lCAGEPOQty == 0)
            {
                lCAGEPOQty = lCAGEDelQty;
            }

            lOrder = lDTBackup.Select("PONo='" + lPONo + "' AND BBSNo='" + lBBSNo + "'");
            if (lOrder != null && lOrder.Length > 0)
            {
                lRemarks = (string)lOrder[0].ItemArray[24];
                lRemarks = lRemarks.Replace("'", "''");
            }

            lBBSDesc = lBBSDesc.Replace("'", "''");
            if (lBBSDesc.Length > 500)
            {
                lBBSDesc = lBBSDesc.Substring(0, 500);
            }

            lSQL = "INSERT INTO dbo.OESBBSSum " +
            "(CustomerCode " +
            ", ProjectCode " +
            ", SSNNo " +
            ", JobID " +
            ", BBSID " +
            ", PONo " +
            ", BBSNo " +
            ", BBSDesc " +
            ", WBS1 " +
            ", WBS2 " +
            ", WBS3 " +
            ", STElement " +
            ", PODate " +
            ", RequiredDate " +
            ", DeliveryDate " +
            ", CABOrderWT " +
            ", CABDeliveryWT " +
            ", SBOrderWT " +
            ", SBDeliveryWT " +
            ", MESHOrderWT " +
            ", MESHDeliveryWT " +
            ", CAGEOrderWT " +
            ", CAGEDeliveryWT " +
            ", SONo " +
            ", Remarks) " +
            "VALUES " +
            "('" + CustomerCode + "' " +
            ",'" + ProjectCode + "' " +
            "," + lSSNNo.ToString() + " " +
            "," + lJobID.ToString() + " " +
            "," + lBBSID.ToString() + " " +
            ",'" + lPONo + "' " +
            ",'" + lBBSNo + "' " +
            ",'" + lBBSDesc + "' " +
            ",'" + lWBS1 + "' " +
            ",'" + lWBS2 + "' " +
            ",'" + lWBS3 + "' " +
            ",'" + lStEle + "' " +
            ",'" + DateTime.ParseExact(lPODate, "yyyyMMdd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd") + "' " +
            ",'" + DateTime.ParseExact(lReqDate, "yyyyMMdd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd") + "' " +
            ",'" + ((lLoadDate == "00000000" || lLoadDate == null || lLoadDate == "" || lLoadDate == " ") ? "" : DateTime.ParseExact(lLoadDate, "yyyyMMdd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd")) + "' " +
            "," + lCABPOQty.ToString() + " " +
            "," + lCABDelQty.ToString() + " " +
            "," + lSBPOQty.ToString() + " " +
            "," + lSBDelQty.ToString() + " " +
            "," + lMESHPOQty.ToString() + " " +
            "," + lMESHDelQty.ToString() + " " +
            "," + lCAGEPOQty.ToString() + " " +
            "," + lCAGEDelQty.ToString() + " " +
            ",'" + lSONo + "' " +
            ",'" + lRemarks + "') ";

            lCmd = new SqlCommand();
            lCmd.CommandText = lSQL;
            lCmd.Connection = lNDSCon;
            lCmd.CommandTimeout = 300;
            lCmd.ExecuteNonQuery();


            return lReturn;

        }

        //[HttpGet]
        //[Route("/BBSSumProcess/{CustomerCode}/{ProjectCode}")]
        //public int BBSSumProcess(string CustomerCode, string ProjectCode)
        //{
        //    int lReturn = 0;
        //    int lSSNNo = 0;
        //    string lSQL = "";

        //    string lPONo = "";
        //    string lPODate = "";
        //    string lWBS1 = "";
        //    string lWBS2 = "";
        //    string lWBS3 = "";
        //    string lStEle = "";
        //    string lProdType = "";
        //    string lBBSNo = "";
        //    string lBBSDesc = "";
        //    string lReqDate = "";
        //    string lLoadDate = "";
        //    string lSONo = "";
        //    decimal lDelQty = 0;

        //    decimal lCABDelQty = 0;
        //    decimal lSBDelQty = 0;
        //    decimal lMESHDelQty = 0;
        //    decimal lCAGEDelQty = 0;

        //    decimal lCABPOQty = 0;
        //    decimal lSBPOQty = 0;
        //    decimal lMESHPOQty = 0;
        //    decimal lCAGEPOQty = 0;

        //    string lRemarks = "";

        //    int lJobID = 0;
        //    int lBBSID = 0;

        //    var lProjectContent = db.Project.Find(CustomerCode, ProjectCode);
        //    string lProjectCodeMESH = lProjectContent.ProjectCodeMESH;
        //    string lProjectCodeCage = lProjectContent.ProjectCodeCage;
        //    if (lProjectCodeMESH == null) lProjectCodeMESH = "";
        //    if (lProjectCodeCage == null) lProjectCodeCage = "";
        //    lProjectCodeMESH = lProjectCodeMESH.Trim();
        //    lProjectCodeCage = lProjectCodeCage.Trim();

        //    DataTable lDTBackup = new DataTable();
        //    DataTable lDTOrder = new DataTable();

        //    DateTime lDeliveryDate = new DateTime();
        //    DateTime lRequiredDate = new DateTime();
        //    DateTime lSumUpdatedDate = new DateTime();

        //    var lCmd = new SqlCommand();
        //    var lDA = new SqlDataAdapter();

        //    SqlDataReader lRst;
        //    var lNDSCon = new SqlConnection();

        //    var lCISCon = new OracleConnection();
        //    var lOraCmd = new OracleCommand();
        //    OracleDataReader lOraRst;

        //    var lProcessObj = new ProcessController();
        //    if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
        //    {
        //        //Chech process time
        //        lCmd.CommandText = "SELECT SumUpdatedDate FROM dbo.OESProject " +
        //        "WHERE CustomerCode = '" + CustomerCode + "' " +
        //        "AND ProjectCode = '" + ProjectCode + "' ";

        //        lCmd.Connection = lNDSCon;
        //        lCmd.CommandTimeout = 300;
        //        lRst = lCmd.ExecuteReader();
        //        if (lRst.HasRows)
        //        {
        //            if (lRst.Read())
        //            {
        //                lSumUpdatedDate = lRst.GetValue(0) == DBNull.Value ? new DateTime(2010, 1, 1) : lRst.GetDateTime(0);
        //            }
        //        }
        //        lRst.Close();

        //        if (lSumUpdatedDate < DateTime.Today)
        //        {

        //            //get last delevery datetime
        //            lCmd.CommandText = "SELECT isNull(Max(DeliveryDate),DateAdd(yyyy,-10,getDate())) FROM dbo.OESBBSSum " +
        //            "WHERE CustomerCode = '" + CustomerCode + "' " +
        //            "AND ProjectCode = '" + ProjectCode + "' ";

        //            lCmd.Connection = lNDSCon;
        //            lCmd.CommandTimeout = 300;
        //            lRst = lCmd.ExecuteReader();
        //            if (lRst.HasRows)
        //            {
        //                if (lRst.Read())
        //                {
        //                    lDeliveryDate = lRst.GetDateTime(0);
        //                }
        //            }
        //            lRst.Close();

        //            //Backup the data 
        //            lCmd.CommandText = "SELECT * FROM dbo.OESBBSSum " +
        //            "WHERE CustomerCode = '" + CustomerCode + "' " +
        //            "AND ProjectCode = '" + ProjectCode + "' " +
        //            "AND (DeliveryDate  >= '" + lDeliveryDate.ToString("yyyy-MM-dd") + "' " +
        //            "OR DeliveryDate is null) ";

        //            lCmd.Connection = lNDSCon;
        //            lCmd.CommandTimeout = 300;
        //            lDA = new SqlDataAdapter();
        //            lDA.SelectCommand = lCmd;
        //            lDTBackup = new DataTable();
        //            lDA.Fill(lDTBackup);

        //            //Delete
        //            lCmd.CommandText = "DELETE FROM dbo.OESBBSSum " +
        //            "WHERE CustomerCode = '" + CustomerCode + "' " +
        //            "AND ProjectCode = '" + ProjectCode + "' " +
        //            "AND (DeliveryDate  >= '" + lDeliveryDate.ToString("yyyy-MM-dd") + "' " +
        //            "OR DeliveryDate is null) ";

        //            lCmd.Connection = lNDSCon;
        //            lCmd.CommandTimeout = 300;
        //            lCmd.ExecuteNonQuery();


        //            //get last required date
        //            lCmd.CommandText = "SELECT isNull(Max(RequiredDate),DateAdd(yyyy,-10,getDate())) FROM dbo.OESBBSSum " +
        //            "WHERE CustomerCode = '" + CustomerCode + "' " +
        //            "AND ProjectCode = '" + ProjectCode + "' ";

        //            lCmd.Connection = lNDSCon;
        //            lCmd.CommandTimeout = 300;
        //            lRst = lCmd.ExecuteReader();
        //            if (lRst.HasRows)
        //            {
        //                if (lRst.Read())
        //                {
        //                    lRequiredDate = lRst.GetDateTime(0);
        //                }
        //            }
        //            lRst.Close();

        //            // get Record from Order
        //            lCmd.CommandText = "SELECT A.CustomerCode, " +
        //            "A.ProjectCode, " +
        //            "A.JobID, " +
        //            "PONumber, " +
        //            "PODate, " +
        //            "RequiredDate, " +
        //            "TotalCABWeight, " +
        //            "TotalSTDWeight, " +
        //            "WBS1, " +
        //            "WBS2, " +
        //            "WBS3, " +
        //            "BBSID, " +
        //            "BBSNo, " +
        //            "BBSDesc, " +
        //            "BBSStrucElem, " +
        //            "BBSOrderCABWT, " +
        //            "BBSOrderSTDWT, " +
        //            "BBSSOR, " +
        //            "BBSNoNDS " +
        //            "FROM dbo.OESJobAdvice A, dbo.OESBBS B " +
        //            "WHERE A.CustomerCode = B.CustomerCode " +
        //            "AND A.ProjectCode = B.ProjectCode " +
        //            "AND A.JobID = B.JobID " +
        //            "AND A.CustomerCode = '" + CustomerCode + "' " +
        //            "AND A.ProjectCode = '" + ProjectCode + "' " +
        //            "AND A.RequiredDate >= '" + lRequiredDate.ToString("yyyy-MM-dd") + "' ";

        //            lCmd.Connection = lNDSCon;
        //            lCmd.CommandTimeout = 300;
        //            lDA = new SqlDataAdapter();
        //            lDA.SelectCommand = lCmd;
        //            lDTOrder = new DataTable();
        //            lDA.Fill(lDTOrder);


        //            //get last SSNNo
        //            lCmd.CommandText = "SELECT isNull(Max(SSNNo),0) FROM dbo.OESBBSSum " +
        //            "WHERE CustomerCode = '" + CustomerCode + "' " +
        //            "AND ProjectCode = '" + ProjectCode + "' ";

        //            lCmd.Connection = lNDSCon;
        //            lCmd.CommandTimeout = 300;
        //            lRst = lCmd.ExecuteReader();
        //            if (lRst.HasRows)
        //            {
        //                if (lRst.Read())
        //                {
        //                    lSSNNo = lRst.GetInt32(0);
        //                }
        //            }
        //            lRst.Close();

        //            lProcessObj.OpenCISConnection(ref lCISCon);

        //            //CAB 
        //            //lSQL = "SELECT /*+ INDEX_SS(S \"LIPS~Y01\") INDEX_SS(D) */ " +
        //            lSQL = "SELECT /*+ INDEX_SS(S) INDEX_SS(D) */ " +
        //            "H.PO_NUMBER, " +
        //            "H.CUST_ORDER_DATE, " +
        //            "D.WBS1, " +
        //            "D.WBS2, " +
        //            "D.WBS3, " +
        //            "D.ST_ELEMENT_TYPE, " +
        //            "D.PRODUCT_TYPE, " +
        //            "D.BBS_NO, " +
        //            "D.BBS_DESC, " +
        //            "P.WADAT, " +
        //            "MIN(P.WADAT_IST) as LOAD_DATE, " +
        //            "SUM(S.NTGEW) as DEL_QTY, " +
        //            "H.SALES_ORDER " +
        //            "FROM SAPSR3.YMSDT_REQ_DETAIL D, " +
        //            "SAPSR3.YMSDT_ORDER_HDR H, " +
        //            "SAPSR3.lips S, " +
        //            "SAPSR3.likp P " +
        //            "WHERE H.ORDER_REQUEST_NO = D.ORDER_REQUEST_NO " +
        //            "AND S.VGBEL = H.SALES_ORDER " +
        //            "AND P.VBELN = S.VBELN " +
        //            "AND to_date(P.WADAT_IST,'yyyymmdd') >= to_date(H.CUST_ORDER_DATE,'yyyymmdd')-100 " +
        //            "AND SUBSTR(P.WADAT_IST,5,2) > 0 " +
        //            "AND SUBSTR(H.CUST_ORDER_DATE,5,2) > 0 " +
        //            "AND H.MANDT = '" + lProcessObj.strClient + "' " +
        //            "AND H.PROJECT = '" + ProjectCode + "' " +
        //            "AND H.STATUS <> 'X' " +
        //            "AND D.STATUS <> 'X' " +
        //            "AND P.WADAT_IST >= '" + lDeliveryDate.ToString("yyyyMMdd") + "' " +
        //            "GROUP BY " +
        //            "S.VBELN, " +
        //            "H.SALES_ORDER, " +
        //            "H.PO_NUMBER, " +
        //            "H.CUST_ORDER_DATE, " +
        //            "D.WBS1, " +
        //            "D.WBS2, " +
        //            "D.WBS3, " +
        //            "D.ST_ELEMENT_TYPE, " +
        //            "D.PRODUCT_TYPE, " +
        //            "D.BBS_NO, " +
        //            "D.BBS_DESC, " +
        //            "P.WADAT " +
        //             "ORDER BY " +
        //             "11, " +
        //             "8 ";

        //            lOraCmd.CommandText = lSQL;
        //            lOraCmd.Connection = lCISCon;
        //            lOraCmd.CommandTimeout = 300;
        //            lOraRst = lOraCmd.ExecuteReader();
        //            if (lOraRst.HasRows)
        //            {
        //                while (lOraRst.Read())
        //                {
        //                    lSSNNo = lSSNNo + 1;
        //                    BBSSumInsert(CustomerCode, ProjectCode, lNDSCon, lOraRst, lDTOrder, lDTBackup, lSSNNo);
        //                }
        //            }
        //            lOraRst.Close();

        //            lSQL = " " +
        //            "SELECT /*+ INDEX_SS(S) INDEX_SS(P) INDEX_SS(K) */ " +
        //            "H.PO_NUMBER, " +
        //            "H.CUST_ORDER_DATE, " +
        //            "' ', " +
        //            "' ', " +
        //            "' ', " +
        //            "' ', " +
        //            "'SB', " +
        //            "K.BSTNK, " +
        //            "listagg(to_char(Round(S.NTGEW/2000)) || ' X ' ||S.ARKTX, ', ') within group (order by S.ARKTX), " +
        //            "P.WADAT, " +
        //            "MIN(P.WADAT_IST) as LOAD_DATE, " +
        //            "SUM(S.NTGEW) as DEL_QTY, " +
        //            "H.SALES_ORDER " +
        //            "FROM SAPSR3.YMSDT_ORDER_HDR H, " +
        //            "SAPSR3.vbak K, " +
        //            "SAPSR3.lips S, " +
        //            "SAPSR3.likp P " +
        //            "WHERE S.VGBEL = H.SALES_ORDER " +
        //            "AND P.VBELN = S.VBELN " +
        //            "AND K.VBELN = H.SALES_ORDER " +
        //            "AND to_date(P.WADAT_IST,'yyyymmdd') >= to_date(H.CUST_ORDER_DATE,'yyyymmdd')-100 " +
        //            "AND SUBSTR(P.WADAT_IST,5,2) > 0 " +
        //            "AND SUBSTR(H.CUST_ORDER_DATE,5,2) > 0 " +
        //            "AND H.MANDT = '" + lProcessObj.strClient + "' " +
        //            "AND H.PROJECT = '" + ProjectCode + "' " +
        //            "AND S.NTGEW > 0 " +
        //            "AND NOT EXISTS (SELECT ORDER_REQUEST_NO " +
        //            "FROM SAPSR3.YMSDT_REQ_DETAIL " +
        //            "WHERE ORDER_REQUEST_NO = H.ORDER_REQUEST_NO) " +
        //            "AND H.STATUS <> 'X' " +
        //            "AND P.WADAT_IST >= '" + lDeliveryDate.ToString("yyyyMMdd") + "' " +
        //            "GROUP BY " +
        //            "S.VBELN, " +
        //            "H.SALES_ORDER, " +
        //            "H.PO_NUMBER, " +
        //            "H.CUST_ORDER_DATE, " +
        //            "K.BSTNK, " +
        //            "P.WADAT " +
        //            "ORDER BY " +
        //            "11, " +
        //            "8 ";

        //            lOraCmd.CommandText = lSQL;
        //            lOraCmd.Connection = lCISCon;
        //            lOraCmd.CommandTimeout = 300;
        //            lOraRst = lOraCmd.ExecuteReader();
        //            if (lOraRst.HasRows)
        //            {
        //                while (lOraRst.Read())
        //                {
        //                    lSSNNo = lSSNNo + 1;
        //                    BBSSumInsert(CustomerCode, ProjectCode, lNDSCon, lOraRst, lDTOrder, lDTBackup, lSSNNo);
        //                }
        //            }
        //            lOraRst.Close();

        //            if (lProjectCodeMESH != "")
        //            {
        //                lSQL = " " +
        //                "SELECT /*+ INDEX_SS(S) INDEX_SS(D) */ " +
        //                "H.PO_NUMBER, " +
        //                "H.CUST_ORDER_DATE, " +
        //                "D.WBS1, " +
        //                "D.WBS2, " +
        //                "D.WBS3, " +
        //                "D.ST_ELEMENT_TYPE, " +
        //                "D.PRODUCT_TYPE, " +
        //                "H.PO_NUMBER, " +
        //                "' ', " +
        //                "P.WADAT, " +
        //                "MIN(P.WADAT_IST) as LOAD_DATE, " +
        //                "SUM(S.NTGEW) as DEL_QTY, " +
        //                "H.SALES_ORDER " +
        //                "FROM SAPSR3.YMSDT_REQ_DETAIL D, " +
        //                "SAPSR3.YMSDT_ORDER_HDR H, " +
        //                "SAPSR3.lips S, " +
        //                "SAPSR3.likp P " +
        //                "WHERE H.ORDER_REQUEST_NO = D.ORDER_REQUEST_NO " +
        //                "AND S.VGBEL = H.SALES_ORDER " +
        //                "AND P.VBELN = S.VBELN " +
        //                "AND to_date(P.WADAT_IST,'yyyymmdd') >= to_date(H.CUST_ORDER_DATE,'yyyymmdd')-100 " +
        //                "AND SUBSTR(P.WADAT_IST,5,2) > 0 " +
        //                "AND SUBSTR(H.CUST_ORDER_DATE,5,2) > 0 " +
        //                "AND H.MANDT = '" + lProcessObj.strClient + "' " +
        //                "AND H.PROJECT = '" + lProjectCodeMESH + "' " +
        //                "AND H.STATUS <> 'X' " +
        //                "AND D.STATUS <> 'X' " +
        //                "AND P.WADAT_IST >= '" + lDeliveryDate.ToString("yyyyMMdd") + "' " +
        //                "GROUP BY " +
        //                "S.VBELN, " +
        //                "H.SALES_ORDER, " +
        //                "H.PO_NUMBER, " +
        //                "H.CUST_ORDER_DATE, " +
        //                "D.WBS1, " +
        //                "D.WBS2, " +
        //                "D.WBS3, " +
        //                "D.ST_ELEMENT_TYPE, " +
        //                "D.PRODUCT_TYPE, " +
        //                "P.WADAT " +
        //                "ORDER BY " +
        //                "11, " +
        //                "8 ";

        //                lOraCmd.CommandText = lSQL;
        //                lOraCmd.Connection = lCISCon;
        //                lOraCmd.CommandTimeout = 300;
        //                lOraRst = lOraCmd.ExecuteReader();
        //                if (lOraRst.HasRows)
        //                {
        //                    while (lOraRst.Read())
        //                    {
        //                        lSSNNo = lSSNNo + 1;
        //                        BBSSumInsert(CustomerCode, ProjectCode, lNDSCon, lOraRst, lDTOrder, lDTBackup, lSSNNo);
        //                    }
        //                }
        //                lOraRst.Close();

        //                lSQL = " " +
        //                "SELECT /*+ INDEX_SS(S) INDEX_SS(P) INDEX_SS(K) */ " +
        //                "H.PO_NUMBER, " +
        //                "H.CUST_ORDER_DATE, " +
        //                "' ', " +
        //                "' ', " +
        //                "' ', " +
        //                "' ', " +
        //                "'MSH', " +
        //                "K.BSTNK, " +
        //                "listagg(to_char(S.LFIMG) || ' X ' ||S.ARKTX, ', ') within group (order by S.ARKTX), " +
        //                "P.WADAT, " +
        //                "MIN(P.WADAT_IST) as LOAD_DATE, " +
        //                "SUM(S.NTGEW) as DEL_QTY, " +
        //                "H.SALES_ORDER " +
        //                "FROM SAPSR3.YMSDT_ORDER_HDR H, " +
        //                "SAPSR3.vbak K, " +
        //                "SAPSR3.lips S, " +
        //                "SAPSR3.likp P " +
        //                "WHERE S.VGBEL = H.SALES_ORDER " +
        //                "AND P.VBELN = S.VBELN " +
        //                "AND K.VBELN = H.SALES_ORDER " +
        //                "AND to_date(P.WADAT_IST,'yyyymmdd') >= to_date(H.CUST_ORDER_DATE,'yyyymmdd')-100 " +
        //                "AND SUBSTR(P.WADAT_IST,5,2) > 0 " +
        //                "AND SUBSTR(H.CUST_ORDER_DATE,5,2) > 0 " +
        //                "AND H.MANDT = '" + lProcessObj.strClient + "' " +
        //                "AND H.PROJECT = '" + lProjectCodeMESH + "' " +
        //                "AND S.NTGEW > 0 " +
        //                "AND NOT EXISTS (SELECT ORDER_REQUEST_NO " +
        //                "FROM SAPSR3.YMSDT_REQ_DETAIL " +
        //                "WHERE ORDER_REQUEST_NO = H.ORDER_REQUEST_NO) " +
        //                "AND H.STATUS <> 'X' " +
        //                "AND P.WADAT_IST >= '" + lDeliveryDate.ToString("yyyyMMdd") + "' " +
        //                "GROUP BY " +
        //                "S.VBELN, " +
        //                "H.SALES_ORDER, " +
        //                "H.PO_NUMBER, " +
        //                "H.CUST_ORDER_DATE, " +
        //                "K.BSTNK, " +
        //                "P.WADAT " +
        //                "ORDER BY " +
        //                "11, " +
        //                "8 ";

        //                lOraCmd.CommandText = lSQL;
        //                lOraCmd.Connection = lCISCon;
        //                lOraCmd.CommandTimeout = 300;
        //                lOraRst = lOraCmd.ExecuteReader();
        //                if (lOraRst.HasRows)
        //                {
        //                    while (lOraRst.Read())
        //                    {
        //                        lSSNNo = lSSNNo + 1;
        //                        BBSSumInsert(CustomerCode, ProjectCode, lNDSCon, lOraRst, lDTOrder, lDTBackup, lSSNNo);
        //                    }
        //                }
        //                lOraRst.Close();
        //            }

        //            if (lProjectCodeCage != "")
        //            {
        //                lSQL = " " +
        //                "SELECT /*+ INDEX_SS(S) INDEX_SS(D) */ " +
        //                "H.PO_NUMBER, " +
        //                "H.CUST_ORDER_DATE, " +
        //                "D.WBS1, " +
        //                "D.WBS2, " +
        //                "D.WBS3, " +
        //                "D.ST_ELEMENT_TYPE, " +
        //                "D.PRODUCT_TYPE, " +
        //                "D.BBS_NO, " +
        //                "D.BBS_DESC, " +
        //                "P.WADAT, " +
        //                "MIN(P.WADAT_IST) as LOAD_DATE, " +
        //                "SUM(S.NTGEW) as DEL_QTY, " +
        //                "H.SALES_ORDER " +
        //                "FROM SAPSR3.YMSDT_REQ_DETAIL D, " +
        //                "SAPSR3.YMSDT_ORDER_HDR H, " +
        //                "SAPSR3.lips S, " +
        //                "SAPSR3.likp P " +
        //                "WHERE H.ORDER_REQUEST_NO = D.ORDER_REQUEST_NO " +
        //                "AND S.VGBEL = H.SALES_ORDER " +
        //                "AND P.VBELN = S.VBELN " +
        //                "AND to_date(P.WADAT_IST,'yyyymmdd') >= to_date(H.CUST_ORDER_DATE,'yyyymmdd')-100 " +
        //                "AND SUBSTR(P.WADAT_IST,5,2) > 0 " +
        //                "AND SUBSTR(H.CUST_ORDER_DATE,5,2) > 0 " +
        //                "AND H.MANDT = '" + lProcessObj.strClient + "' " +
        //                "AND H.PROJECT = '" + lProjectCodeCage + "' " +
        //                "AND H.STATUS <> 'X' " +
        //                "AND D.STATUS <> 'X' " +
        //                "AND P.WADAT_IST >= '" + lDeliveryDate.ToString("yyyyMMdd") + "' " +
        //                "GROUP BY " +
        //                "S.VBELN, " +
        //                "H.SALES_ORDER, " +
        //                "H.PO_NUMBER, " +
        //                "H.CUST_ORDER_DATE, " +
        //                "D.WBS1, " +
        //                "D.WBS2, " +
        //                "D.WBS3, " +
        //                "D.ST_ELEMENT_TYPE, " +
        //                "D.PRODUCT_TYPE, " +
        //                "D.BBS_NO, " +
        //                "D.BBS_DESC, " +
        //                "P.WADAT " +
        //                "ORDER BY " +
        //                "11, " +
        //                "8 ";

        //                lOraCmd.CommandText = lSQL;
        //                lOraCmd.Connection = lCISCon;
        //                lOraCmd.CommandTimeout = 300;
        //                lOraRst = lOraCmd.ExecuteReader();
        //                if (lOraRst.HasRows)
        //                {
        //                    while (lOraRst.Read())
        //                    {
        //                        lSSNNo = lSSNNo + 1;
        //                        BBSSumInsert(CustomerCode, ProjectCode, lNDSCon, lOraRst, lDTOrder, lDTBackup, lSSNNo);
        //                    }
        //                }
        //                lOraRst.Close();
        //            }

        //            //get have not scheduled order

        //            lSQL = "SELECT /*+ INDEX_SS(S \"LIPS~Y01\") INDEX_SS(D) */ " +
        //            "H.PO_NUMBER, " +
        //            "H.CUST_ORDER_DATE, " +
        //            "D.WBS1, " +
        //            "D.WBS2, " +
        //            "D.WBS3, " +
        //            "D.ST_ELEMENT_TYPE, " +
        //            "D.PRODUCT_TYPE, " +
        //            "D.BBS_NO, " +
        //            "D.BBS_DESC, " +
        //            "H.REQD_DEL_DATE, " +
        //            "MIN(P.WADAT_IST) as LOAD_DATE, " +
        //            "SUM(S.NTGEW) as DEL_QTY, " +
        //            "H.SALES_ORDER " +
        //            "FROM SAPSR3.YMSDT_REQ_DETAIL D, " +
        //            "SAPSR3.YMSDT_ORDER_HDR H, " +
        //            "SAPSR3.lips S, " +
        //            "SAPSR3.likp P " +
        //            "WHERE H.ORDER_REQUEST_NO = D.ORDER_REQUEST_NO " +
        //            "AND S.VGBEL = H.SALES_ORDER " +
        //            "AND S.VGBEL > ' ' " +
        //            "AND P.VBELN = S.VBELN " +
        //            "AND P.WADAT_IST <= to_char(to_date(H.CUST_ORDER_DATE,'yyyymmdd')-300,'yyyymmdd') " +
        //            "AND SUBSTR(H.CUST_ORDER_DATE,5,2) > 0 " +
        //            "AND H.MANDT = '" + lProcessObj.strClient + "' " +
        //            "AND H.PROJECT = '" + ProjectCode + "' " +
        //            "AND H.STATUS <> 'X' " +
        //            "AND D.STATUS <> 'X' " +
        //            "GROUP BY " +
        //            "S.VBELN, " +
        //            "H.SALES_ORDER, " +
        //            "H.PO_NUMBER, " +
        //            "H.CUST_ORDER_DATE, " +
        //            "D.WBS1, " +
        //            "D.WBS2, " +
        //            "D.WBS3, " +
        //            "D.ST_ELEMENT_TYPE, " +
        //            "D.PRODUCT_TYPE, " +
        //            "D.BBS_NO, " +
        //            "D.BBS_DESC, " +
        //            "H.REQD_DEL_DATE " +
        //            "ORDER BY " +
        //            "10, " +
        //            "8 ";
        //            lOraCmd.CommandText = lSQL;
        //            lOraCmd.Connection = lCISCon;
        //            lOraCmd.CommandTimeout = 300;
        //            lOraRst = lOraCmd.ExecuteReader();
        //            if (lOraRst.HasRows)
        //            {
        //                while (lOraRst.Read())
        //                {
        //                    lSSNNo = lSSNNo + 1;
        //                    BBSSumInsert(CustomerCode, ProjectCode, lNDSCon, lOraRst, lDTOrder, lDTBackup, lSSNNo);
        //                }
        //            }
        //            lOraRst.Close();

        //            lSQL = " " +
        //            "SELECT /*+ INDEX_SS(D) INDEX_SS(P) */ " +
        //            "H.PO_NUMBER, " +
        //            "H.CUST_ORDER_DATE, " +
        //            "D.WBS1, " +
        //            "D.WBS2, " +
        //            "D.WBS3, " +
        //            "D.ST_ELEMENT_TYPE, " +
        //            "D.PRODUCT_TYPE, " +
        //            "D.BBS_NO, " +
        //            "D.BBS_DESC, " +
        //            "H.REQD_DEL_DATE, " +
        //            "' ', " +
        //            "SUM(P.NTGEW) as DEL_QTY, " +
        //            "H.SALES_ORDER " +
        //            "FROM SAPSR3.YMSDT_REQ_DETAIL D, " +
        //            "SAPSR3.YMSDT_ORDER_HDR H, " +
        //            "SAPSR3.vbak K, SAPSR3.vbap P " +
        //            "WHERE H.ORDER_REQUEST_NO = D.ORDER_REQUEST_NO " +
        //            "AND K.VBELN = H.SALES_ORDER " +
        //            "AND P.VBELN = K.VBELN " +
        //            "AND NOT EXISTS (SELECT /*+ INDEX_SS(S \"LIPS~Y01\") */ S.VGBEL FROM SAPSR3.lips S WHERE S.VGBEL = H.SALES_ORDER) " +
        //            "AND H.MANDT = '" + lProcessObj.strClient + "' " +
        //            "AND H.PROJECT = '" + ProjectCode + "' " +
        //            "AND H.STATUS <> 'X' " +
        //            "AND D.STATUS <> 'X' " +
        //            "GROUP BY " +
        //            "H.SALES_ORDER, " +
        //            "H.PO_NUMBER, " +
        //            "H.CUST_ORDER_DATE, " +
        //            "D.WBS1, " +
        //            "D.WBS2, " +
        //            "D.WBS3, " +
        //            "D.ST_ELEMENT_TYPE, " +
        //            "D.PRODUCT_TYPE, " +
        //            "D.BBS_NO, " +
        //            "D.BBS_DESC, " +
        //            "H.REQD_DEL_DATE " +
        //            "ORDER BY " +
        //            "10, " +
        //            "8 ";
        //            lOraCmd.CommandText = lSQL;
        //            lOraCmd.Connection = lCISCon;
        //            lOraCmd.CommandTimeout = 300;
        //            lOraRst = lOraCmd.ExecuteReader();
        //            if (lOraRst.HasRows)
        //            {
        //                while (lOraRst.Read())
        //                {
        //                    lSSNNo = lSSNNo + 1;
        //                    BBSSumInsert(CustomerCode, ProjectCode, lNDSCon, lOraRst, lDTOrder, lDTBackup, lSSNNo);
        //                }
        //            }
        //            lOraRst.Close();

        //            lSQL = " " +
        //            "SELECT /*+ INDEX_SS(S \"LIPS~Y01\") INDEX_SS(P) INDEX_SS(K) */ " +
        //            "H.PO_NUMBER, " +
        //            "H.CUST_ORDER_DATE, " +
        //            "' ', " +
        //            "' ', " +
        //            "' ', " +
        //            "' ', " +
        //            "'SB', " +
        //            "K.BSTNK, " +
        //            "listagg(to_char(Round(S.NTGEW/2000)) || ' X ' ||S.ARKTX, ', ') within group (order by S.ARKTX), " +
        //            "H.REQD_DEL_DATE, " +
        //            "MIN(P.WADAT_IST) as LOAD_DATE, " +
        //            "SUM(S.NTGEW) as DEL_QTY, " +
        //            "H.SALES_ORDER " +
        //            "FROM SAPSR3.YMSDT_ORDER_HDR H, " +
        //            "SAPSR3.vbak K, " +
        //            "SAPSR3.lips S, " +
        //            "SAPSR3.likp P " +
        //            "WHERE S.VGBEL = H.SALES_ORDER " +
        //            "AND P.VBELN = S.VBELN " +
        //            "AND S.VGBEL > ' ' " +
        //            "AND K.VBELN = H.SALES_ORDER " +
        //            "AND P.WADAT_IST <= to_char(to_date(H.CUST_ORDER_DATE,'yyyymmdd')-300,'yyyymmdd') " +
        //            "AND SUBSTR(H.CUST_ORDER_DATE,5,2) > 0 " +
        //            "AND H.MANDT = '" + lProcessObj.strClient + "' " +
        //            "AND H.PROJECT = '" + ProjectCode + "' " +
        //            "AND S.NTGEW > 0 " +
        //            "AND NOT EXISTS (SELECT ORDER_REQUEST_NO " +
        //            "FROM SAPSR3.YMSDT_REQ_DETAIL " +
        //            "WHERE ORDER_REQUEST_NO = H.ORDER_REQUEST_NO) " +
        //            "AND H.STATUS <> 'X' " +
        //            "GROUP BY " +
        //            "S.VBELN, " +
        //            "H.SALES_ORDER, " +
        //            "H.PO_NUMBER, " +
        //            "H.CUST_ORDER_DATE, " +
        //            "K.BSTNK, " +
        //            "H.REQD_DEL_DATE " +
        //            "ORDER BY " +
        //            "10, " +
        //            "8 ";
        //            lOraCmd.CommandText = lSQL;
        //            lOraCmd.Connection = lCISCon;
        //            lOraCmd.CommandTimeout = 300;
        //            lOraRst = lOraCmd.ExecuteReader();
        //            if (lOraRst.HasRows)
        //            {
        //                while (lOraRst.Read())
        //                {
        //                    lSSNNo = lSSNNo + 1;
        //                    BBSSumInsert(CustomerCode, ProjectCode, lNDSCon, lOraRst, lDTOrder, lDTBackup, lSSNNo);
        //                }
        //            }
        //            lOraRst.Close();

        //            lSQL = " " +
        //            "SELECT /*+ INDEX_SS(S \"LIPS~Y01\") INDEX_SS(K) */ " +
        //            "H.PO_NUMBER, " +
        //            "H.CUST_ORDER_DATE, " +
        //            "' ', " +
        //            "' ', " +
        //            "' ', " +
        //            "' ', " +
        //            "'SB', " +
        //            "K.BSTNK, " +
        //            "listagg(to_char(Round(P.NTGEW/2000)) || ' X ' ||P.ARKTX, ', ') within group (order by P.ARKTX), " +
        //            "H.REQD_DEL_DATE, " +
        //            "' ' as LOAD_DATE, " +
        //            "SUM(P.NTGEW) as DEL_QTY, " +
        //            "H.SALES_ORDER " +
        //            "FROM SAPSR3.YMSDT_ORDER_HDR H, " +
        //            "SAPSR3.vbak K, SAPSR3.vbap P " +
        //            "WHERE K.VBELN = H.SALES_ORDER " +
        //            "AND K.VBELN = P.VBELN " +
        //            "AND NOT EXISTS (SELECT /*+ INDEX_SS(S \"LIPS~Y01\") */ S.VGBEL FROM SAPSR3.lips S WHERE S.VGBEL = H.SALES_ORDER) " +
        //            "AND H.MANDT = '" + lProcessObj.strClient + "' " +
        //            "AND H.PROJECT = '" + ProjectCode + "' " +
        //            "AND NOT EXISTS (SELECT ORDER_REQUEST_NO " +
        //            "FROM SAPSR3.YMSDT_REQ_DETAIL " +
        //            "WHERE ORDER_REQUEST_NO = H.ORDER_REQUEST_NO) " +
        //            "AND H.STATUS <> 'X' " +
        //            "GROUP BY " +
        //            "K.VBELN, " +
        //            "H.SALES_ORDER, " +
        //            "H.PO_NUMBER, " +
        //            "H.CUST_ORDER_DATE, " +
        //            "K.BSTNK, " +
        //            "H.REQD_DEL_DATE " +
        //            "ORDER BY " +
        //            "10, " +
        //            "8 ";
        //            lOraCmd.CommandText = lSQL;
        //            lOraCmd.Connection = lCISCon;
        //            lOraCmd.CommandTimeout = 300;
        //            lOraRst = lOraCmd.ExecuteReader();
        //            if (lOraRst.HasRows)
        //            {
        //                while (lOraRst.Read())
        //                {
        //                    lSSNNo = lSSNNo + 1;
        //                    BBSSumInsert(CustomerCode, ProjectCode, lNDSCon, lOraRst, lDTOrder, lDTBackup, lSSNNo);
        //                }
        //            }
        //            lOraRst.Close();

        //            if (lProjectCodeMESH != "")
        //            {
        //                lSQL = " " +
        //                "SELECT /*+ INDEX_SS(S \"LIPS~Y01\") INDEX_SS(D) */ " +
        //                "H.PO_NUMBER, " +
        //                "H.CUST_ORDER_DATE, " +
        //                "D.WBS1, " +
        //                "D.WBS2, " +
        //                "D.WBS3, " +
        //                "D.ST_ELEMENT_TYPE, " +
        //                "D.PRODUCT_TYPE, " +
        //                "H.PO_NUMBER, " +
        //                "' ', " +
        //                "H.REQD_DEL_DATE, " +
        //                "MIN(P.WADAT_IST) as LOAD_DATE, " +
        //                "SUM(S.NTGEW) as DEL_QTY, " +
        //                "H.SALES_ORDER " +
        //                "FROM SAPSR3.YMSDT_REQ_DETAIL D, " +
        //                "SAPSR3.YMSDT_ORDER_HDR H, " +
        //                "SAPSR3.lips S, " +
        //                "SAPSR3.likp P " +
        //                "WHERE H.ORDER_REQUEST_NO = D.ORDER_REQUEST_NO " +
        //                "AND S.VGBEL = H.SALES_ORDER " +
        //                "AND S.VGBEL > ' ' " +
        //                "AND P.VBELN = S.VBELN " +
        //                "AND P.WADAT_IST <= to_char(to_date(H.CUST_ORDER_DATE,'yyyymmdd')-300,'yyyymmdd') " +
        //                "AND SUBSTR(H.CUST_ORDER_DATE,5,2) > 0 " +
        //                "AND H.MANDT = '" + lProcessObj.strClient + "' " +
        //                "AND H.PROJECT = '" + lProjectCodeMESH + "' " +
        //                "AND H.STATUS <> 'X' " +
        //                "AND D.STATUS <> 'X' " +
        //                "GROUP BY " +
        //                "S.VBELN, " +
        //                "H.SALES_ORDER, " +
        //                "H.PO_NUMBER, " +
        //                "H.CUST_ORDER_DATE, " +
        //                "D.WBS1, " +
        //                "D.WBS2, " +
        //                "D.WBS3, " +
        //                "D.ST_ELEMENT_TYPE, " +
        //                "D.PRODUCT_TYPE, " +
        //                "H.REQD_DEL_DATE " +
        //                "ORDER BY " +
        //                "10, " +
        //                "8 ";
        //                lOraCmd.CommandText = lSQL;
        //                lOraCmd.Connection = lCISCon;
        //                lOraCmd.CommandTimeout = 300;
        //                lOraRst = lOraCmd.ExecuteReader();
        //                if (lOraRst.HasRows)
        //                {
        //                    while (lOraRst.Read())
        //                    {
        //                        lSSNNo = lSSNNo + 1;
        //                        BBSSumInsert(CustomerCode, ProjectCode, lNDSCon, lOraRst, lDTOrder, lDTBackup, lSSNNo);
        //                    }
        //                }
        //                lOraRst.Close();

        //                //have not scheduled for delivery
        //                lSQL = " " +
        //                "SELECT /*+ INDEX_SS(D) INDEX_SS(P) */ " +
        //                "H.PO_NUMBER, " +
        //                "H.CUST_ORDER_DATE, " +
        //                "D.WBS1, " +
        //                "D.WBS2, " +
        //                "D.WBS3, " +
        //                "D.ST_ELEMENT_TYPE, " +
        //                "D.PRODUCT_TYPE, " +
        //                "H.PO_NUMBER, " +
        //                "' ', " +
        //                "H.REQD_DEL_DATE, " +
        //                "' ' as LOAD_DATE, " +
        //                "SUM(P.NTGEW) as DEL_QTY, " +
        //                "H.SALES_ORDER " +
        //                "FROM SAPSR3.YMSDT_REQ_DETAIL D, " +
        //                "SAPSR3.YMSDT_ORDER_HDR H, " +
        //                "SAPSR3.vbak K, " +
        //                "SAPSR3.vbap P " +
        //                "WHERE H.ORDER_REQUEST_NO = D.ORDER_REQUEST_NO " +
        //                "AND K.VBELN = H.SALES_ORDER " +
        //                "AND P.VBELN = K.VBELN " +
        //                "AND NOT EXISTS (SELECT /*+ INDEX_SS(S \"LIPS~Y01\") */ S.VGBEL FROM SAPSR3.lips S WHERE S.VGBEL = H.SALES_ORDER) " +
        //                "AND H.MANDT = '" + lProcessObj.strClient + "' " +
        //                "AND H.PROJECT = '" + lProjectCodeMESH + "' " +
        //                "AND H.STATUS <> 'X' " +
        //                "AND D.STATUS <> 'X' " +
        //                "GROUP BY " +
        //                "H.SALES_ORDER, " +
        //                "H.PO_NUMBER, " +
        //                "H.CUST_ORDER_DATE, " +
        //                "D.WBS1, " +
        //                "D.WBS2, " +
        //                "D.WBS3, " +
        //                "D.ST_ELEMENT_TYPE, " +
        //                "D.PRODUCT_TYPE, " +
        //                "H.REQD_DEL_DATE " +
        //                "ORDER BY " +
        //                "10, " +
        //                "8 ";
        //                lOraCmd.CommandText = lSQL;
        //                lOraCmd.Connection = lCISCon;
        //                lOraCmd.CommandTimeout = 300;
        //                lOraRst = lOraCmd.ExecuteReader();
        //                if (lOraRst.HasRows)
        //                {
        //                    while (lOraRst.Read())
        //                    {
        //                        lSSNNo = lSSNNo + 1;
        //                        BBSSumInsert(CustomerCode, ProjectCode, lNDSCon, lOraRst, lDTOrder, lDTBackup, lSSNNo);
        //                    }
        //                }
        //                lOraRst.Close();
        //            }

        //            if (lProjectCodeCage != "")
        //            {
        //                lSQL = " " +
        //                "SELECT /*+ INDEX_SS(S \"LIPS~Y01\") INDEX_SS(D) */ " +
        //                "H.PO_NUMBER, " +
        //                "H.CUST_ORDER_DATE, " +
        //                "D.WBS1, " +
        //                "D.WBS2, " +
        //                "D.WBS3, " +
        //                "D.ST_ELEMENT_TYPE, " +
        //                "D.PRODUCT_TYPE, " +
        //                "D.BBS_NO, " +
        //                "D.BBS_DESC, " +
        //                "H.REQD_DEL_DATE, " +
        //                "MIN(P.WADAT_IST) as LOAD_DATE, " +
        //                "SUM(S.NTGEW) as DEL_QTY, " +
        //                "H.SALES_ORDER " +
        //                "FROM SAPSR3.YMSDT_REQ_DETAIL D, " +
        //                "SAPSR3.YMSDT_ORDER_HDR H, " +
        //                "SAPSR3.lips S, " +
        //                "SAPSR3.likp P " +
        //                "WHERE H.ORDER_REQUEST_NO = D.ORDER_REQUEST_NO " +
        //                "AND S.VGBEL = H.SALES_ORDER " +
        //                "AND S.VGBEL > ' ' " +
        //                "AND P.VBELN = S.VBELN " +
        //                "AND P.WADAT_IST <= to_char(to_date(H.CUST_ORDER_DATE,'yyyymmdd')-300,'yyyymmdd') " +
        //                "AND SUBSTR(H.CUST_ORDER_DATE,5,2) > 0 " +
        //                "AND H.MANDT = '" + lProcessObj.strClient + "' " +
        //                "AND H.PROJECT = '" + lProjectCodeCage + "' " +
        //                "AND H.STATUS <> 'X' " +
        //                "AND D.STATUS <> 'X' " +
        //                "GROUP BY " +
        //                "S.VBELN, " +
        //                "H.SALES_ORDER, " +
        //                "H.PO_NUMBER, " +
        //                "H.CUST_ORDER_DATE, " +
        //                "D.WBS1, " +
        //                "D.WBS2, " +
        //                "D.WBS3, " +
        //                "D.ST_ELEMENT_TYPE, " +
        //                "D.PRODUCT_TYPE, " +
        //                "D.BBS_NO, " +
        //                "D.BBS_DESC, " +
        //                "H.REQD_DEL_DATE " +
        //                "ORDER BY " +
        //                "10, " +
        //                "8 ";
        //                lOraCmd.CommandText = lSQL;
        //                lOraCmd.Connection = lCISCon;
        //                lOraCmd.CommandTimeout = 300;
        //                lOraRst = lOraCmd.ExecuteReader();
        //                if (lOraRst.HasRows)
        //                {
        //                    while (lOraRst.Read())
        //                    {
        //                        lSSNNo = lSSNNo + 1;
        //                        BBSSumInsert(CustomerCode, ProjectCode, lNDSCon, lOraRst, lDTOrder, lDTBackup, lSSNNo);
        //                    }
        //                }
        //                lOraRst.Close();

        //                //have not scheduled for delivery
        //                lSQL = " " +
        //                "SELECT /*+ INDEX_SS(D) INDEX_SS(P) */ " +
        //                "H.PO_NUMBER, " +
        //                "H.CUST_ORDER_DATE, " +
        //                "D.WBS1, " +
        //                "D.WBS2, " +
        //                "D.WBS3, " +
        //                "D.ST_ELEMENT_TYPE, " +
        //                "D.PRODUCT_TYPE, " +
        //                "D.BBS_NO, " +
        //                "D.BBS_DESC, " +
        //                "H.REQD_DEL_DATE, " +
        //                "' ', " +
        //                "SUM(P.NTGEW) as DEL_QTY, " +
        //                "H.SALES_ORDER " +
        //                "FROM SAPSR3.YMSDT_REQ_DETAIL D, " +
        //                "SAPSR3.YMSDT_ORDER_HDR H, " +
        //                "SAPSR3.vbak K, " +
        //                "SAPSR3.vbap P " +
        //                "WHERE H.ORDER_REQUEST_NO = D.ORDER_REQUEST_NO " +
        //                "AND K.VBELN = H.SALES_ORDER " +
        //                "AND P.VBELN = K.VBELN " +
        //                "AND NOT EXISTS (SELECT /*+ INDEX_SS(S \"LIPS~Y01\") */ S.VGBEL FROM SAPSR3.lips S WHERE S.VGBEL = H.SALES_ORDER) " +
        //                "AND H.MANDT = '" + lProcessObj.strClient + "' " +
        //                "AND H.PROJECT = '" + lProjectCodeCage + "' " +
        //                "AND H.STATUS <> 'X' " +
        //                "AND D.STATUS <> 'X' " +
        //                "GROUP BY " +
        //                "H.SALES_ORDER, " +
        //                "H.PO_NUMBER, " +
        //                "H.CUST_ORDER_DATE, " +
        //                "D.WBS1, " +
        //                "D.WBS2, " +
        //                "D.WBS3, " +
        //                "D.ST_ELEMENT_TYPE, " +
        //                "D.PRODUCT_TYPE, " +
        //                "D.BBS_NO, " +
        //                "D.BBS_DESC, " +
        //                "H.REQD_DEL_DATE " +
        //                "ORDER BY " +
        //                "10, " +
        //                "8 ";

        //                lOraCmd.CommandText = lSQL;
        //                lOraCmd.Connection = lCISCon;
        //                lOraCmd.CommandTimeout = 300;
        //                lOraRst = lOraCmd.ExecuteReader();
        //                if (lOraRst.HasRows)
        //                {
        //                    while (lOraRst.Read())
        //                    {
        //                        lSSNNo = lSSNNo + 1;
        //                        BBSSumInsert(CustomerCode, ProjectCode, lNDSCon, lOraRst, lDTOrder, lDTBackup, lSSNNo);
        //                    }
        //                }
        //                lOraRst.Close();
        //            }

        //            // get Record from Order
        //            lCmd.CommandText = "SELECT A.CustomerCode, " +
        //            "A.ProjectCode, " +
        //            "A.JobID, " +
        //            "PONumber, " +
        //            "PODate, " +
        //            "RequiredDate, " +
        //            "TotalCABWeight, " +
        //            "TotalSTDWeight, " +
        //            "WBS1, " +
        //            "WBS2, " +
        //            "WBS3, " +
        //            "BBSID, " +
        //            "BBSNo, " +
        //            "BBSDesc, " +
        //            "BBSStrucElem, " +
        //            "BBSOrderCABWT, " +
        //            "BBSOrderSTDWT, " +
        //            "BBSSOR, " +
        //            "BBSNoNDS " +
        //            "FROM dbo.OESJobAdvice A, dbo.OESBBS B " +
        //            "WHERE A.CustomerCode = B.CustomerCode " +
        //            "AND A.ProjectCode = B.ProjectCode " +
        //            "AND A.JobID = B.JobID " +
        //            "AND A.CustomerCode = '" + CustomerCode + "' " +
        //            "AND A.ProjectCode = '" + ProjectCode + "' " +
        //            "AND Requireddate > ' ' " +
        //            "AND PONumber > ' ' " +
        //            "AND NOT EXISTS (SELECT BBSNO FROM dbo.OESBBSSum " +
        //            "WHERE PONo = A.PONumber " +
        //            "AND BBSNo = B.BBSNo)  ";


        //            lCmd.Connection = lNDSCon;
        //            lCmd.CommandTimeout = 300;
        //            lDA = new SqlDataAdapter();
        //            lDA.SelectCommand = lCmd;
        //            lDTOrder = new DataTable();
        //            lDA.Fill(lDTOrder);

        //            if (lDTOrder.Rows.Count > 0)
        //            {
        //                for (int i = 0; i < lDTOrder.Rows.Count; i++)
        //                {
        //                    lPONo = (lDTOrder.Rows[i].ItemArray[3] == null ? "" : lDTOrder.Rows[i].ItemArray[3].ToString().Trim());
        //                    lPODate = (lDTOrder.Rows[i].ItemArray[4] == null ? "2000-01-01" : ((DateTime)lDTOrder.Rows[i].ItemArray[4]).ToString("yyyy-MM-dd"));
        //                    lWBS1 = (lDTOrder.Rows[i].ItemArray[8] == null ? "" : lDTOrder.Rows[i].ItemArray[8].ToString().Trim());
        //                    lWBS2 = (lDTOrder.Rows[i].ItemArray[9] == null ? "" : lDTOrder.Rows[i].ItemArray[9].ToString().Trim());
        //                    lWBS3 = (lDTOrder.Rows[i].ItemArray[10] == null ? "" : lDTOrder.Rows[i].ItemArray[10].ToString().Trim());
        //                    lStEle = (lDTOrder.Rows[i].ItemArray[14] == null ? "" : lDTOrder.Rows[i].ItemArray[14].ToString().Trim());
        //                    lBBSNo = (lDTOrder.Rows[i].ItemArray[12] == null ? "" : lDTOrder.Rows[i].ItemArray[12].ToString().Trim());
        //                    lBBSDesc = (lDTOrder.Rows[i].ItemArray[13] == null ? "" : lDTOrder.Rows[i].ItemArray[13].ToString().Trim());
        //                    lReqDate = (lDTOrder.Rows[i].ItemArray[5] == null ? "2000-01-01" : ((DateTime)lDTOrder.Rows[i].ItemArray[5]).ToString("yyyy-MM-dd"));
        //                    lLoadDate = null;

        //                    lJobID = (int)lDTOrder.Rows[i].ItemArray[2];
        //                    lBBSID = (int)lDTOrder.Rows[i].ItemArray[11];
        //                    lCABPOQty = (decimal)lDTOrder.Rows[i].ItemArray[15];
        //                    lSBPOQty = (decimal)lDTOrder.Rows[i].ItemArray[16];

        //                    lDelQty = 0;
        //                    lSONo = "";

        //                    lCABDelQty = 0;
        //                    lSBDelQty = 0;
        //                    lMESHDelQty = 0;
        //                    if (lCABPOQty > 0 && lSBPOQty > 0)
        //                    {
        //                        lProdType = "CAB/SB";
        //                    }
        //                    else if (lCABPOQty > 0)
        //                    {
        //                        lProdType = "CAB";
        //                    }
        //                    else if (lSBPOQty > 0)
        //                    {
        //                        lProdType = "SB";
        //                    }

        //                    lSSNNo = lSSNNo + 1;

        //                    DataRow[] lOrder = lDTBackup.Select("PONo='" + lPONo + "' AND BBSNo='" + lBBSNo + "'");
        //                    if (lOrder != null && lOrder.Length > 0)
        //                    {
        //                        lRemarks = (string)lOrder[0].ItemArray[24];
        //                        lRemarks = lRemarks.Replace("'", "''");
        //                    }

        //                    lBBSDesc = lBBSDesc.Replace("'", "''");

        //                    lSQL = "INSERT INTO dbo.OESBBSSum " +
        //                    "(CustomerCode " +
        //                    ", ProjectCode " +
        //                    ", SSNNo " +
        //                    ", JobID " +
        //                    ", BBSID " +
        //                    ", PONo " +
        //                    ", BBSNo " +
        //                    ", BBSDesc " +
        //                    ", WBS1 " +
        //                    ", WBS2 " +
        //                    ", WBS3 " +
        //                    ", STElement " +
        //                    ", PODate " +
        //                    ", RequiredDate " +
        //                    ", DeliveryDate " +
        //                    ", CABOrderWT " +
        //                    ", CABDeliveryWT " +
        //                    ", SBOrderWT " +
        //                    ", SBDeliveryWT " +
        //                    ", MESHOrderWT " +
        //                    ", MESHDeliveryWT " +
        //                    ", CAGEOrderWT " +
        //                    ", CAGEDeliveryWT " +
        //                    ", SONo " +
        //                    ", Remarks) " +
        //                    "VALUES " +
        //                    "('" + CustomerCode + "' " +
        //                    ",'" + ProjectCode + "' " +
        //                    "," + lSSNNo.ToString() + " " +
        //                    "," + lJobID.ToString() + " " +
        //                    "," + lBBSID.ToString() + " " +
        //                    ",'" + lPONo + "' " +
        //                    ",'" + lBBSNo + "' " +
        //                    ",'" + lBBSDesc + "' " +
        //                    ",'" + lWBS1 + "' " +
        //                    ",'" + lWBS2 + "' " +
        //                    ",'" + lWBS3 + "' " +
        //                    ",'" + lStEle + "' " +
        //                    ",'" + lPODate + "' " +
        //                    ",'" + lReqDate + "' " +
        //                    ",null " +
        //                    "," + lCABPOQty.ToString() + " " +
        //                    "," + lCABDelQty.ToString() + " " +
        //                    "," + lSBPOQty.ToString() + " " +
        //                    "," + lSBDelQty.ToString() + " " +
        //                    ",0" +
        //                    ",0 " +
        //                    ",0" +
        //                    ",0 " +
        //                    ",'" + lSONo + "' " +
        //                    ",'" + lRemarks + "') ";

        //                    lCmd = new SqlCommand();
        //                    lCmd.CommandText = lSQL;
        //                    lCmd.Connection = lNDSCon;
        //                    lCmd.CommandTimeout = 300;
        //                    lCmd.ExecuteNonQuery();
        //                }
        //            }
        //            lSQL = "UPDATE dbo.OESProject SET SumUpdatedDate = getDate() " +
        //            "WHERE CustomerCode = '" + CustomerCode + "' " +
        //            "AND ProjectCode = '" + ProjectCode + "' ";

        //            lCmd = new SqlCommand();
        //            lCmd.CommandText = lSQL;
        //            lCmd.Connection = lNDSCon;
        //            lCmd.CommandTimeout = 300;
        //            lCmd.ExecuteNonQuery();
        //        }

        //        lProcessObj.CloseNDSConnection(ref lNDSCon);
        //        lProcessObj.CloseCISConnection(ref lCISCon);
        //    }

        //    lProcessObj = null;
        //    lCmd = null;
        //    lRst = null;
        //    lNDSCon = null;

        //    lOraCmd = null;
        //    lOraRst = null;
        //    lCISCon = null;
        //    return lReturn;
        //}

        decimal UpdateOrderWT(string pCustomerCode, string pProjectCode, int pOrderNo)
        {
            decimal lTotalWT = 0;

            var lOrderSE = (from p in db.OrderProjectSE
                            where p.OrderNumber == pOrderNo
                            select p).ToList();

            for (int i = 0; i < lOrderSE.Count; i++)
            {
                if (lOrderSE[i].CABJobID > 0)
                {
                    int lJobID = lOrderSE[i].CABJobID;
                    var lProdWT = (from p in db.OrderDetails
                                   where p.CustomerCode == pCustomerCode &&
                                   p.ProjectCode == pProjectCode &&
                                   p.JobID == lJobID &&
                                   p.Cancelled == null
                                   select p.BarWeight)
                               .DefaultIfEmpty(0)
                               .Sum();

                    if (lProdWT == null) lProdWT = 0;

                    //save WT back to SE
                    if (lOrderSE[i].TotalWeight != lProdWT)
                    {
                        var lNewCart = lOrderSE[i];
                        lNewCart.TotalWeight = lProdWT;
                        db.Entry(lOrderSE[i]).CurrentValues.SetValues(lNewCart);
                    }
                    lTotalWT = lTotalWT + (decimal)lProdWT;
                }
                else if (lOrderSE[i].MESHJobID > 0)
                {
                    int lJobID = lOrderSE[i].MESHJobID;
                    var lProdWT = (from p in db.CTSMESHOthersDetails
                                   where p.CustomerCode == pCustomerCode &&
                                   p.ProjectCode == pProjectCode &&
                                   p.JobID == lJobID
                                   select p.MeshTotalWT)
                               .DefaultIfEmpty(0)
                               .Sum();

                    if (lProdWT == null) lProdWT = 0;

                    //save WT back to SE
                    if (lOrderSE[i].TotalWeight != lProdWT)
                    {
                        var lNewCart = lOrderSE[i];
                        lNewCart.TotalWeight = lProdWT;
                        db.Entry(lOrderSE[i]).CurrentValues.SetValues(lNewCart);
                    }
                    lTotalWT = lTotalWT + (decimal)lProdWT;

                }
                else if (lOrderSE[i].BPCJobID > 0)
                {
                    int lJobID = lOrderSE[i].BPCJobID;
                    var lProdWT = (from p in db.BPCJobAdvice
                                   where p.CustomerCode == pCustomerCode &&
                                   p.ProjectCode == pProjectCode &&
                                   p.JobID == lJobID
                                   select p.TotalWeight)
                               .DefaultIfEmpty(0)
                               .Sum();

                    //save WT back to SE
                    if (lOrderSE[i].TotalWeight != lProdWT)
                    {
                        var lNewCart = lOrderSE[i];
                        lNewCart.TotalWeight = lProdWT;
                        db.Entry(lOrderSE[i]).CurrentValues.SetValues(lNewCart);
                    }
                    lTotalWT = lTotalWT + (decimal)lProdWT;
                }
                else if (lOrderSE[i].CageJobID > 0)
                {
                    int lJobID = lOrderSE[i].CageJobID;
                    var lProdWT = (from p in db.PRCJobAdvice
                                   where p.CustomerCode == pCustomerCode &&
                                   p.ProjectCode == pProjectCode &&
                                   p.JobID == lJobID
                                   select p.TotalWeight)
                               .DefaultIfEmpty(0)
                               .Sum();

                    //save WT back to SE
                    if (lOrderSE[i].TotalWeight != lProdWT)
                    {
                        var lNewCart = lOrderSE[i];
                        lNewCart.TotalWeight = lProdWT;
                        db.Entry(lOrderSE[i]).CurrentValues.SetValues(lNewCart);
                    }
                    lTotalWT = lTotalWT + (decimal)lProdWT;
                }
                else if (lOrderSE[i].CoilProdJobID > 0)
                {
                    int lJobID = lOrderSE[i].CoilProdJobID;
                    var lProdWT = (from p in db.StdProdDetails
                                   where p.CustomerCode == pCustomerCode &&
                                   p.ProjectCode == pProjectCode &&
                                   p.JobID == lJobID &&
                                   p.ProdType != "BAR12" &&
                                   p.ProdType != "BAR14" &&
                                   p.ProdType != "BAR15" &&
                                   p.ProdType != "500M" &&
                                   p.ProdType != "BAR6"
                                   select p.order_wt)
                               .DefaultIfEmpty(0)
                               .Sum();

                    //save WT back to SE
                    if (lOrderSE[i].TotalWeight != lProdWT)
                    {
                        var lNewCart = lOrderSE[i];
                        lNewCart.TotalWeight = lProdWT;
                        db.Entry(lOrderSE[i]).CurrentValues.SetValues(lNewCart);
                    }
                    lTotalWT = lTotalWT + (decimal)lProdWT;
                }
                else if (lOrderSE[i].StdBarsJobID > 0)
                {
                    int lJobID = lOrderSE[i].StdBarsJobID;
                    var lProdWT = (from p in db.StdProdDetails
                                   where p.CustomerCode == pCustomerCode &&
                                   p.ProjectCode == pProjectCode &&
                                   p.JobID == lJobID &&
                                   (p.ProdType == "BAR12" ||
                                   p.ProdType == "BAR14" ||
                                   p.ProdType == "BAR15" ||
                                   p.ProdType == "500M" ||
                                   p.ProdType == "BAR6")
                                   select p.order_wt)
                               .DefaultIfEmpty(0)
                               .Sum();

                    //save WT back to SE
                    if (lOrderSE[i].TotalWeight != lProdWT)
                    {
                        var lNewCart = lOrderSE[i];
                        lNewCart.TotalWeight = lProdWT;
                        db.Entry(lOrderSE[i]).CurrentValues.SetValues(lNewCart);
                    }
                    lTotalWT = lTotalWT + (decimal)lProdWT;
                }
                else if (lOrderSE[i].StdMESHJobID > 0)
                {
                    int lJobID = lOrderSE[i].StdMESHJobID;

                    var lProdWT = (from p in db.StdSheetDetails
                                   where p.CustomerCode == pCustomerCode &&
                                   p.ProjectCode == pProjectCode &&
                                   p.JobID == lJobID
                                   select p.order_wt)
                               .DefaultIfEmpty(0)
                               .Sum();

                    //save WT back to SE
                    if (lOrderSE[i].TotalWeight != lProdWT)
                    {
                        var lNewCart = lOrderSE[i];
                        lNewCart.TotalWeight = lProdWT;
                        db.Entry(lOrderSE[i]).CurrentValues.SetValues(lNewCart);
                    }
                    lTotalWT = lTotalWT + (decimal)lProdWT;
                }
            }

            if (lTotalWT > 0)
            {
                var lUXHeader = db.OrderProject.Find(pOrderNo);
                if (lUXHeader != null && lUXHeader.OrderNumber > 0)
                {
                    if (lUXHeader.TotalWeight != lTotalWT)
                    {
                        var lNewHd = lUXHeader;
                        lNewHd.TotalWeight = lTotalWT;
                        db.Entry(lUXHeader).CurrentValues.SetValues(lNewHd);
                    }
                }
                db.SaveChanges();
            }
            return lTotalWT;

        }

        //List of raised PO
        [HttpPost]
        [Route("/getSavedOrders")]
        public async Task<ActionResult> getSavedOrders([FromBody] SavedOrdersDto savedOrdersDto)
        {
            string lPODateFrom = "";
            string lPODateTo = "";
            string lRDateFrom = "";
            string lRDateTo = "";

            UserAccessController lUa = new UserAccessController();
            var lUserType = lUa.getUserType(User.Identity.GetUserName());
            var lGroupName = lUa.getGroupName(User.Identity.GetUserName());

            ViewBag.UserType = lUserType;

            string lUserName = User.Identity.GetUserName();

            lUa = null;

            if (savedOrdersDto.PODate == null) savedOrdersDto.PODate = "";
            if (savedOrdersDto.PODate.Trim().Length == 0 || savedOrdersDto.PODate.IndexOf("to") <= 0)
            {
                lPODateFrom = "2000-01-01 00:00:00";
                lPODateTo = "2200-01-01 00:00:00";
            }
            else
            {
                lPODateFrom = savedOrdersDto.PODate.Substring(0, savedOrdersDto.PODate.IndexOf("to")).Trim();
                lPODateTo = savedOrdersDto.PODate.Substring(savedOrdersDto.PODate.IndexOf("to") + 2).Trim();
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

            if (savedOrdersDto.RDate == null) savedOrdersDto.RDate = "";
            if (savedOrdersDto.RDate.Trim().Length == 0 || savedOrdersDto.RDate.IndexOf("to") <= 0)
            {
                lRDateFrom = "2000-01-01 00:00:00";
                lRDateTo = "2200-01-01 23:59:59";
            }
            else
            {
                lRDateFrom = savedOrdersDto.RDate.Substring(0, savedOrdersDto.RDate.IndexOf("to")).Trim();
                lRDateTo = savedOrdersDto.RDate.Substring(savedOrdersDto.RDate.IndexOf("to") + 2).Trim();
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
                lRDateTo = "2200-01-01 23:59:59";
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
                UpdateBy = "",
                OrderShared = false,
                ScheduledProd = "N",
                BBSNo = "",
                BBSDesc = "",
                CustomerCode = "",
                ProjectCode = "",
                ProjectTitle = ""
            }}).ToList();

            if (lReturn.Count > 0)
            {
                lReturn.RemoveAt(0);
            }

            if (savedOrdersDto.CustomerCode != null && savedOrdersDto.ProjectCode != null)
            {
                if (savedOrdersDto.CustomerCode.Length > 0 && savedOrdersDto.ProjectCode.Length > 0)
                {
                    var lProjectState = "";
                    if (savedOrdersDto.AllProjects == true)
                    {
                        SharedAPIController lBackEnd = new SharedAPIController();

                        var lProjects = lBackEnd.getProject(savedOrdersDto.CustomerCode, lUserType, lGroupName);
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
                        lProjectState = "AND M.ProjectCode = '" + savedOrdersDto.ProjectCode + "' ";
                    }


                    lCmd.CommandText =
                        "SELECT " +
                        "M.OrderNumber, " +
                        "M.WBS1, " +
                        "M.WBS2, " +
                        "M.WBS3, " +
                        "(STUFF( " +
                        "(SELECT ',' + left(StructureElement + ProductType, len(StructureElement)) " +
                        "FROM dbo.OESProjOrdersSE " +
                        "WHERE OrderNumber = M.OrderNumber " +
                        "GROUP BY StructureElement, ProductType " +
                        "FOR XML PATH('')), 1, 1, ''))   " +
                        "AS StructureElement, " +
                        "(STUFF( " +
                        "(SELECT ',' + left(ProductType + StructureElement, len(ProductType)) " +
                        "FROM dbo.OESProjOrdersSE " +
                        "WHERE OrderNumber = M.OrderNumber " +
                        "GROUP BY ProductType, StructureElement " +
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
                        "(SELECT ',' + left(convert(varchar(10), RequiredDate, 120) + ProductType + StructureElement, 10) " +
                        "FROM dbo.OESProjOrdersSE " +
                        "WHERE OrderNumber = M.OrderNumber " +
                        "GROUP BY convert(varchar(10), RequiredDate, 120), ProductType, StructureElement " +
                        "FOR XML PATH('')), 1, 1, ''))   " +
                        "AS RequiredDate, " +
                        "M.TotalWeight, " +
                        "M.UpdateBy, " +
                        "M.OrderShared, " +
                        "MIN(S.ScheduledProd), " +
                        "(STUFF( " +
                        "(SELECT ',' + rtrim(ltrim(BBSNo)) " +
                        "FROM dbo.OESBBS " +
                        "WHERE CustomerCode = M.CustomerCode " +
                        "AND ProjectCode = M.ProjectCode AND JobID > 0 " +
                        "AND JobID in (select CABJobID from dbo.OESProjOrdersSE " +
                        "WHERE OrderNumber = S.OrderNumber group by CABJobID) " +
                        "GROUP BY BBSNo " +
                        "FOR XML PATH('')), 1, 1, ''))   " +
                        "AS BBSNo, " +
                        "(STUFF( " +
                        "(SELECT ',' + rtrim(ltrim(BBSDesc)) " +
                        "FROM dbo.OESBBS " +
                        "WHERE CustomerCode = M.CustomerCode " +
                        "AND ProjectCode = M.ProjectCode AND JobID > 0 " +
                        "AND JobID in (select CABJobID from dbo.OESProjOrdersSE " +
                        "WHERE OrderNumber = S.OrderNumber group by CABJobID) " +
                        "GROUP BY BBSDesc " +
                        "FOR XML PATH('')), 1, 1, ''))   " +
                        "AS BBSDesc, " +
                        "M.CustomerCode, " +
                        "M.ProjectCode, " +
                        "(SELECT ProjectTitle FROM dbo.OESProject " +
                        "WHERE CustomerCode = M.CustomerCode " +
                        "AND ProjectCode = M.ProjectCode) as ProjectTitle " +
                        "FROM dbo.OESProjOrder M, dbo.OESProjOrdersSE S " +
                        "WHERE M.OrderNumber = S.OrderNumber " +
                        "AND M.CustomerCode = '" + savedOrdersDto.CustomerCode + "' " +
                        "" + lProjectState + " " +
                        "AND (M.OrderStatus is NULL " +
                        "OR M.OrderStatus = 'New' " +
                        "OR M.OrderStatus = 'Created' " +
                        "OR M.OrderStatus = 'Reserved') " +
                        "AND ((S.UpdateDate >= '" + lPODateFrom + "' " +
                        "AND DATEADD(d,-1,S.UpdateDate) < '" + lPODateTo + "') " +
                        "OR (S.UpdateDate is null AND '" + lPODateTo + "' = '2200-01-01 00:00:00' )) " +
                        "AND ((S.RequiredDate >= '" + lRDateFrom + "' " +
                        "AND DATEADD(d,-1,S.RequiredDate) < '" + lRDateTo + "') " +
                        "OR (S.RequiredDate is null AND '" + lRDateTo + "' = '2200-01-01 23:59:59' )) " +
                        "AND (S.PONumber like '%" + savedOrdersDto.PONumber + "%' " +
                        "OR  (S.PONumber is null AND '" + savedOrdersDto.PONumber + "' = '' )) " +
                        "AND (M.WBS1 like '%" + savedOrdersDto.WBS1 + "%' " +
                        "OR (M.WBS1 is null AND '" + savedOrdersDto.WBS1 + "' = '' )) " +
                        "AND (M.WBS2 like '%" + savedOrdersDto.WBS2 + "%' " +
                        "OR (M.WBS2 is null AND '" + savedOrdersDto.WBS2 + "' = '' )) " +
                        "AND (M.WBS3 like '%" + savedOrdersDto.WBS3 + "%' " +
                        "OR (M.WBS3 is null AND '" + savedOrdersDto.WBS3 + "' = '' )) " +
                        "AND (M.UpdateBy = '" + User.Identity.GetUserName() + "' " +
                        "OR M.OrderShared = 1 ) " +
                        //"AND M.UpdateDate >= '2019-07-01' " +
                        "GROUP BY " +
                        "M.OrderNumber, " +
                        "S.OrderNumber, " +
                        "M.WBS1, " +
                        "M.WBS2, " +
                        "M.WBS3, " +
                        "M.UpdateDate, " +
                        "M.TotalWeight, " +
                        "M.UpdateBy, " +
                        "M.OrderShared, " +
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
                                int lOrderNo = lRst.GetInt32(0);
                                decimal lOrderWT = lRst.GetValue(9) == DBNull.Value ? 0 : (decimal)(lRst.GetDecimal(9) / 1000);
                                if (lOrderWT == 0)
                                {
                                    lOrderWT = UpdateOrderWT(savedOrdersDto.CustomerCode, savedOrdersDto.ProjectCode, lOrderNo) / 1000;
                                }

                                if (lOrderWT > 0 && lOrderWT < (decimal)0.001)
                                {
                                    lOrderWT = (decimal)0.001;
                                }

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
                                    RequiredDate = (lRst.GetValue(8) == DBNull.Value ? "" : (lRst.GetString(8).Trim().Length > 0 && lRst.GetString(8).Substring(0, 1) == "," ? lRst.GetString(8).Substring(1) : lRst.GetString(8))),
                                    OrderWeight = lOrderWT.ToString("###,###,##0.000;(###,##0.000);"),
                                    UpdateBy = (lRst.GetValue(10) == DBNull.Value ? "" : lRst.GetString(10).Trim()),
                                    OrderShared = (lRst.GetValue(11) == DBNull.Value ? false : (bool)lRst.GetBoolean(11)),
                                    ScheduledProd = (lRst.GetValue(12) == DBNull.Value ? "N" : lRst.GetString(12).Trim()),
                                    BBSNo = (lRst.GetValue(13) == DBNull.Value ? "" : lRst.GetString(13).Trim()),
                                    BBSDesc = System.Net.WebUtility.HtmlDecode(lRst.GetValue(14) == DBNull.Value ? "" : lRst.GetString(14).Trim()),
                                    CustomerCode = (lRst.GetValue(15) == DBNull.Value ? "" : lRst.GetString(15).Trim()),
                                    ProjectCode = (lRst.GetValue(16) == DBNull.Value ? "" : lRst.GetString(16).Trim()),
                                    ProjectTitle = (lRst.GetValue(17) == DBNull.Value ? "" : lRst.GetString(17).Trim())
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


        //To implement to export saved order to excel
        //Commented by ajit
        //public async Task<ActionResult> exportSavedOrdersToExcel(string CustomerCode, string ProjectCode,
        //                string PONumber, string PODate, string RDate,
        //                string WBS1, string WBS2, string WBS3, bool AllProjects)
        //{
        //    string lPODateFrom = "";
        //    string lPODateTo = "";
        //    string lRDateFrom = "";
        //    string lRDateTo = "";

        //    UserAccessController lUa = new UserAccessController();
        //    var lUserType = lUa.getUserType(User.Identity.GetUserName());
        //    var lGroupName = lUa.getGroupName(User.Identity.GetUserName());

        //    ViewBag.UserType = lUserType;

        //    string lUserName = User.Identity.GetUserName();

        //    lUa = null;

        //    if (PODate == null) PODate = "";
        //    if (PODate.Trim().Length == 0 || PODate.IndexOf("to") <= 0)
        //    {
        //        lPODateFrom = "2000-01-01 00:00:00";
        //        lPODateTo = "2200-01-01 23:59:59";
        //    }
        //    else
        //    {
        //        lPODateFrom = PODate.Substring(0, PODate.IndexOf("to")).Trim();
        //        lPODateTo = PODate.Substring(PODate.IndexOf("to") + 2).Trim();
        //    }

        //    lPODateFrom = lPODateFrom + " 00:00:00";
        //    lPODateTo = lPODateTo + " 23:59:59";

        //    DateTime lDateV = new DateTime();
        //    if (DateTime.TryParse(lPODateFrom, out lDateV) != true)
        //    {
        //        lPODateFrom = "2000-01-01 00:00:00";
        //    }
        //    if (DateTime.TryParse(lPODateTo, out lDateV) != true)
        //    {
        //        lPODateTo = "2200-01-01 23:59:59";
        //    }

        //    if (RDate == null) RDate = "";
        //    if (RDate.Trim().Length == 0 || RDate.IndexOf("to") <= 0)
        //    {
        //        lRDateFrom = "2000-01-01 00:00:00";
        //        lRDateTo = "2200-01-01 23:59:59";
        //    }
        //    else
        //    {
        //        lRDateFrom = RDate.Substring(0, RDate.IndexOf("to")).Trim();
        //        lRDateTo = RDate.Substring(RDate.IndexOf("to") + 2).Trim();
        //    }

        //    lRDateFrom = lRDateFrom + " 00:00:00";
        //    lRDateTo = lRDateTo + " 00:00:00";

        //    lDateV = new DateTime();
        //    if (DateTime.TryParse(lRDateFrom, out lDateV) != true)
        //    {
        //        lRDateFrom = "2000-01-01 00:00:00";
        //    }
        //    if (DateTime.TryParse(lRDateTo, out lDateV) != true)
        //    {
        //        lRDateTo = "2200-01-01 23:59:59";
        //    }


        //    ExcelPackage package = new ExcelPackage();
        //    ExcelWorksheet ws = package.Workbook.Worksheets.Add("Saved Order List");

        //    int lRowNo = 1;
        //    //ws.Column(1).Width = 5;         //"SNo\n序号";
        //    ws.Column(1).Width = 12;        //"Order No\n加工表号";
        //    ws.Column(2).Width = 10;        //"WBS1\n楼座";
        //    ws.Column(3).Width = 10;        //"WBS2\n楼座";
        //    ws.Column(4).Width = 7;         //"WBS3\n楼座";
        //    ws.Column(5).Width = 14;        //"Structure Element";
        //    ws.Column(6).Width = 14;        //"Prod Type";
        //    ws.Column(7).Width = 14;        //"PO Number";
        //    ws.Column(8).Width = 14;        //"BBS Number";
        //    ws.Column(9).Width = 20;        //"BBS Desc";
        //    ws.Column(10).Width = 11;        //"Update Date\n订货日期";
        //    ws.Column(11).Width = 15;        //"Required Date\n到场日期";
        //    ws.Column(12).Width = 16;       //"Order WT\n料单重量";

        //    ws.Row(1).Height = 30;
        //    ws.Cells[lRowNo, 1].Value = "SNo\n序号";
        //    //ws.Cells[lRowNo, 2].Value = "Order No\n订单号码";
        //    ws.Cells[lRowNo, 2].Value = "WBS1\n楼座";
        //    ws.Cells[lRowNo, 3].Value = "WBS2\n楼层";
        //    ws.Cells[lRowNo, 4].Value = "WBS3\n分部";
        //    ws.Cells[lRowNo, 5].Value = "Structure Element\n构件";
        //    ws.Cells[lRowNo, 6].Value = "Product Type\n产品类型";
        //    ws.Cells[lRowNo, 7].Value = "PO Number\n订单号码";
        //    ws.Cells[lRowNo, 8].Value = "BBS Number\n钢筋加工号码";
        //    ws.Cells[lRowNo, 9].Value = "BBS Description\n钢筋加工描述";
        //    ws.Cells[lRowNo, 10].Value = "Date Created\n创建日期";
        //    ws.Cells[lRowNo, 11].Value = "Required Date\n到场日期";
        //    ws.Cells[lRowNo, 12].Value = "Order Weight\n料单重量 (MT)";

        //    ws.Cells[lRowNo, 1].Style.WrapText = true;
        //    ws.Cells[lRowNo, 2].Style.WrapText = true;
        //    ws.Cells[lRowNo, 3].Style.WrapText = true;
        //    ws.Cells[lRowNo, 4].Style.WrapText = true;
        //    ws.Cells[lRowNo, 5].Style.WrapText = true;
        //    ws.Cells[lRowNo, 6].Style.WrapText = true;
        //    ws.Cells[lRowNo, 7].Style.WrapText = true;
        //    ws.Cells[lRowNo, 8].Style.WrapText = true;
        //    ws.Cells[lRowNo, 9].Style.WrapText = true;
        //    ws.Cells[lRowNo, 10].Style.WrapText = true;
        //    ws.Cells[lRowNo, 11].Style.WrapText = true;
        //    ws.Cells[lRowNo, 12].Style.WrapText = true;

        //    ws.Cells[lRowNo, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //    ws.Cells[lRowNo, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //    ws.Cells[lRowNo, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //    ws.Cells[lRowNo, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //    ws.Cells[lRowNo, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //    ws.Cells[lRowNo, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //    ws.Cells[lRowNo, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //    ws.Cells[lRowNo, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //    ws.Cells[lRowNo, 9].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //    ws.Cells[lRowNo, 10].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //    ws.Cells[lRowNo, 11].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //    ws.Cells[lRowNo, 12].Style.Border.BorderAround(ExcelBorderStyle.Thin);

        //    ws.Cells[lRowNo, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //    ws.Cells[lRowNo, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //    ws.Cells[lRowNo, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //    ws.Cells[lRowNo, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //    ws.Cells[lRowNo, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //    ws.Cells[lRowNo, 6].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //    ws.Cells[lRowNo, 7].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //    ws.Cells[lRowNo, 8].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //    ws.Cells[lRowNo, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //    ws.Cells[lRowNo, 10].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //    ws.Cells[lRowNo, 11].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //    ws.Cells[lRowNo, 12].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //    //ws.Cells[lRowNo, 13].Style.Fill.PatternType = ExcelFillStyle.Solid;

        //    ws.Cells[lRowNo, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));
        //    ws.Cells[lRowNo, 2].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));
        //    ws.Cells[lRowNo, 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));
        //    ws.Cells[lRowNo, 4].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));
        //    ws.Cells[lRowNo, 5].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));
        //    ws.Cells[lRowNo, 6].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));
        //    ws.Cells[lRowNo, 7].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));
        //    ws.Cells[lRowNo, 8].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));
        //    ws.Cells[lRowNo, 9].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));
        //    ws.Cells[lRowNo, 10].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));
        //    ws.Cells[lRowNo, 11].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));
        //    ws.Cells[lRowNo, 12].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));
        //    //ws.Cells[lRowNo, 13].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));

        //    lRowNo = 2;
        //    var lCmd = new SqlCommand();
        //    SqlDataReader lRst;
        //    var lNDSCon = new SqlConnection();

        //    if (CustomerCode != null && ProjectCode != null)
        //    {
        //        if (CustomerCode.Length > 0 && ProjectCode.Length > 0)
        //        {
        //            var lProjectState = "";
        //            if (AllProjects == true)
        //            {
        //                SharedAPIController lBackEnd = new SharedAPIController();

        //                var lProjects = lBackEnd.getProject(CustomerCode, lUserType, lGroupName);
        //                lBackEnd = null;

        //                if (lProjects != null && lProjects.Count > 0)
        //                {
        //                    for (int i = 0; i < lProjects.Count; i++)
        //                    {
        //                        if (lProjects[i] != null && lProjects[i].ProjectCode != null && lProjects[i].ProjectCode.Trim().Length > 0)
        //                        {
        //                            if (lProjectState == "")
        //                            {
        //                                lProjectState = "M.ProjectCode = '" + lProjects[i].ProjectCode + "' ";
        //                            }
        //                            else
        //                            {
        //                                lProjectState = lProjectState + " OR M.ProjectCode = '" + lProjects[i].ProjectCode + "' ";
        //                            }
        //                        }
        //                    }
        //                }

        //                if (lProjectState == "")
        //                {
        //                    lProjectState = "AND 1 = 2 ";
        //                }
        //                else
        //                {
        //                    lProjectState = "AND (" + lProjectState + ") ";
        //                }

        //            }
        //            else
        //            {
        //                lProjectState = "AND M.ProjectCode = '" + ProjectCode + "' ";
        //            }


        //            lCmd.CommandText =
        //            "SELECT " +
        //            "M.OrderNumber, " +
        //            "M.WBS1, " +
        //            "M.WBS2, " +
        //            "M.WBS3, " +
        //            "(STUFF( " +
        //            "(SELECT ',' + StructureElement " +
        //            "FROM dbo.OESProjOrdersSE " +
        //            "WHERE OrderNumber = M.OrderNumber " +
        //            "GROUP BY StructureElement " +
        //            "FOR XML PATH('')), 1, 1, ''))   " +
        //            "AS StructureElement, " +
        //            "(STUFF( " +
        //            "(SELECT ',' + ProductType " +
        //            "FROM dbo.OESProjOrdersSE " +
        //            "WHERE OrderNumber = M.OrderNumber " +
        //            "GROUP BY ProductType " +
        //            "FOR XML PATH('')), 1, 1, ''))   " +
        //            "AS ProductType, " +
        //            "(STUFF( " +
        //            "(SELECT ',' + PONumber " +
        //            "FROM dbo.OESProjOrdersSE " +
        //            "WHERE OrderNumber = M.OrderNumber " +
        //            "GROUP BY PONumber " +
        //            "FOR XML PATH('')), 1, 1, ''))   " +
        //            "AS PONumber, " +
        //            "M.UpdateDate,  " +
        //            "(STUFF( " +
        //            "(SELECT ',' + convert(varchar(10), RequiredDate, 120) " +
        //            "FROM dbo.OESProjOrdersSE " +
        //            "WHERE OrderNumber = M.OrderNumber " +
        //            "GROUP BY convert(varchar(10), RequiredDate, 120) " +
        //            "FOR XML PATH('')), 1, 1, ''))   " +
        //            "AS RequiredDate, " +
        //            "M.TotalWeight, " +
        //            "M.UpdateBy, " +
        //            "M.OrderShared, " +
        //            "MIN(S.ScheduledProd), " +
        //            "(STUFF( " +
        //            "(SELECT ',' + rtrim(ltrim(BBSNo)) " +
        //            "FROM dbo.OESBBS " +
        //            "WHERE CustomerCode = M.CustomerCode " +
        //            "AND ProjectCode = M.ProjectCode AND JobID > 0 " +
        //            "AND JobID in (select CABJobID from dbo.OESProjOrdersSE " +
        //            "WHERE OrderNumber = S.OrderNumber group by CABJobID) " +
        //            "GROUP BY BBSNo " +
        //            "FOR XML PATH('')), 1, 1, ''))   " +
        //            "AS BBSNo, " +
        //            "(STUFF( " +
        //            "(SELECT ',' + rtrim(ltrim(BBSDesc)) " +
        //            "FROM dbo.OESBBS " +
        //            "WHERE CustomerCode = M.CustomerCode " +
        //            "AND ProjectCode = M.ProjectCode AND JobID > 0 " +
        //            "AND JobID in (select CABJobID from dbo.OESProjOrdersSE " +
        //            "WHERE OrderNumber = S.OrderNumber group by CABJobID) " +
        //            "GROUP BY BBSDesc " +
        //            "FOR XML PATH('')), 1, 1, ''))   " +
        //            "AS BBSDesc " +
        //            "FROM dbo.OESProjOrder M, dbo.OESProjOrdersSE S " +
        //            "WHERE M.OrderNumber = S.OrderNumber " +
        //            "AND M.CustomerCode = '" + CustomerCode + "' " +
        //            "" + lProjectState + " " +
        //            "AND (M.OrderStatus is NULL " +
        //            "OR M.OrderStatus = 'New' " +
        //            "OR M.OrderStatus = 'Created' " +
        //            "OR M.OrderStatus = 'Reserved') " +
        //            "AND ((S.UpdateDate >= '" + lPODateFrom + "' " +
        //            "AND DATEADD(d,-1,S.UpdateDate) < '" + lPODateTo + "') " +
        //            "OR (S.UpdateDate is null AND '" + lPODateTo + "' = '2200-01-01 00:00:00' )) " +
        //            "AND ((S.RequiredDate >= '" + lRDateFrom + "' " +
        //            "AND DATEADD(d,-1,S.RequiredDate) < '" + lRDateTo + "') " +
        //            "OR (S.RequiredDate is null AND '" + lRDateTo + "' = '2200-01-01 23:59:59' )) " +
        //            "AND (S.PONumber like '%" + PONumber + "%' " +
        //            "OR  (S.PONumber is null AND '" + PONumber + "' = '' )) " +
        //            "AND (M.WBS1 like '%" + WBS1 + "%' " +
        //            "OR (M.WBS1 is null AND '" + WBS1 + "' = '' )) " +
        //            "AND (M.WBS2 like '%" + WBS2 + "%' " +
        //            "OR (M.WBS2 is null AND '" + WBS2 + "' = '' )) " +
        //            "AND (M.WBS3 like '%" + WBS3 + "%' " +
        //            "OR (M.WBS3 is null AND '" + WBS3 + "' = '' )) " +
        //            "AND (M.UpdateBy = '" + User.Identity.GetUserName() + "' " +
        //            "OR M.OrderShared = 1 ) " +
        //            //"AND M.UpdateDate >= '2019-07-01' " +
        //            "GROUP BY " +
        //            "M.OrderNumber, " +
        //            "S.OrderNumber, " +
        //            "M.WBS1, " +
        //            "M.WBS2, " +
        //            "M.WBS3, " +
        //            "M.UpdateDate, " +
        //            "M.TotalWeight, " +
        //            "M.UpdateBy, " +
        //            "M.OrderShared, " +
        //            "M.CustomerCode, " +
        //            "M.ProjectCode " +
        //            "ORDER BY " +
        //            "M.OrderNumber DESC ";

        //            var lProcessObj = new ProcessController();
        //            if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
        //            {
        //                lCmd.Connection = lNDSCon;
        //                lCmd.CommandTimeout = 300;
        //                lRst = await lCmd.ExecuteReaderAsync();
        //                if (lRst.HasRows)
        //                {
        //                    var j = 0;
        //                    while (lRst.Read())
        //                    {
        //                        //ws.Cells[j + lRowNo, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                        ws.Cells[j + lRowNo, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                        ws.Cells[j + lRowNo, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                        ws.Cells[j + lRowNo, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                        ws.Cells[j + lRowNo, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                        ws.Cells[j + lRowNo, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                        ws.Cells[j + lRowNo, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                        ws.Cells[j + lRowNo, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                        ws.Cells[j + lRowNo, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                        ws.Cells[j + lRowNo, 9].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                        ws.Cells[j + lRowNo, 10].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                        ws.Cells[j + lRowNo, 11].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                        ws.Cells[j + lRowNo, 12].Style.Border.BorderAround(ExcelBorderStyle.Thin);


        //                        //ws.Cells[j + lRowNo, 1].Value = j + 1;
        //                        ws.Cells[j + lRowNo, 1].Value = (lRst.GetValue(0) == DBNull.Value ? "" : lRst.GetInt32(0).ToString().Trim()); //"Order No\n加工表号";
        //                        ws.Cells[j + lRowNo, 2].Value = (lRst.GetValue(1) == DBNull.Value ? "" : lRst.GetString(1).Trim()); //"WBS1\n楼座";
        //                        ws.Cells[j + lRowNo, 3].Value = (lRst.GetValue(2) == DBNull.Value ? "" : lRst.GetString(2).Trim()); //"WBS2\n楼层";
        //                        ws.Cells[j + lRowNo, 4].Value = (lRst.GetValue(3) == DBNull.Value ? "" : lRst.GetString(3).Trim()); //"WBS3\n分部";
        //                        ws.Cells[j + lRowNo, 5].Value = (lRst.GetValue(4) == DBNull.Value ? "" : (lRst.GetString(4).Trim() == "NONWBS" ? "" : lRst.GetString(4).Trim())); //"Structure Element\n分部

        //                        ws.Cells[j + lRowNo, 6].Value = (lRst.GetValue(5) == DBNull.Value ? "" : lRst.GetString(5)); //"Product Type\n订货日期";
        //                        ws.Cells[j + lRowNo, 7].Value = lRst.GetValue(6) == DBNull.Value ? "" : (lRst.GetString(6).Trim().Length > 0 && lRst.GetString(6).Trim().Substring(0, 1) == "," ? lRst.GetString(6).Trim().Substring(1) : lRst.GetString(6).Trim()); //"Product No\n订货日期";

        //                        ws.Cells[j + lRowNo, 8].Value = (lRst.GetValue(13) == DBNull.Value ? "" : lRst.GetString(13).Trim());
        //                        ws.Cells[j + lRowNo, 9].Value = WebUtility.HtmlDecode(lRst.GetValue(14) == DBNull.Value ? "" : lRst.GetString(14).Trim());

        //                        ws.Cells[j + lRowNo, 10].Value = (lRst.GetValue(7) == DBNull.Value ? "" : lRst.GetDateTime(7).ToString("yyyy-MM-dd")); //"Update Date\n订货日期";

        //                        ws.Cells[j + lRowNo, 11].Value = (lRst.GetValue(8) == DBNull.Value ? "" : (lRst.GetString(8).Substring(0, 1) == "," ? lRst.GetString(8).Substring(1) : lRst.GetString(8))); //"Required Date\n到场日期";
        //                        ws.Cells[j + lRowNo, 12].Value = (lRst.GetValue(9) == DBNull.Value ? (decimal)0 : (lRst.GetDecimal(9) / 1000));//"Total weight\n订货日期";
        //                        ws.Cells[j + lRowNo, 12].Style.Numberformat.Format = "#,###,##0.000";
        //                        j = j + 1;

        //                    }
        //                }
        //                lRst.Close();

        //                lProcessObj.CloseNDSConnection(ref lNDSCon);
        //            }
        //            lProcessObj = null;
        //        }
        //    }
        //    lCmd = null;
        //    lNDSCon = null;
        //    lRst = null;

        //    MemoryStream ms = new MemoryStream();
        //    package.SaveAs(ms);

        //    var bExcel = new byte[ms.Length];
        //    ms.Position = 0;
        //    ms.Read(bExcel, 0, bExcel.Length);

        //    //bPDF = ms.GetBuffer();
        //    ms.Flush();
        //    ms.Dispose();

        //    return Ok(bExcel);
        //}

        //Export order Status to Excal
        //Commented by ajit
        //[HttpPost]

        //public ActionResult ExportOrderStatusToExcel(string CustomerCode, string ProjectCode,
        //                    string PONumber, string PODateFrom, string PODateTo,
        //                    string RDateFrom, string RDateTo)
        //{
        //    //set kookie for customer and project
        //    HttpCookie lCustCookies = new HttpCookie("nsh_digios_cust");
        //    lCustCookies.Value = CustomerCode;
        //    lCustCookies.Expires = DateTime.Now.AddDays(30);
        //    HttpContext.Response.Cookies.Remove("nsh_digios_cust");
        //    HttpContext.Response.SetCookie(lCustCookies);

        //    HttpCookie lProjCookies = new HttpCookie("nsh_digios_proj");
        //    lProjCookies.Value = ProjectCode;
        //    lProjCookies.Expires = DateTime.Now.AddDays(30);
        //    HttpContext.Response.Cookies.Remove("nsh_digios_proj");
        //    HttpContext.Response.SetCookie(lProjCookies);

        //    if (PODateFrom == null) PODateFrom = "";
        //    if (PODateFrom.Trim().Length == 0)
        //    {
        //        PODateFrom = "2000-01-01";
        //    }
        //    if (PODateTo == null) PODateTo = "";
        //    if (PODateTo.Trim().Length == 0)
        //    {
        //        PODateTo = "2200-01-01";
        //    }

        //    if (RDateFrom == null) RDateFrom = "";
        //    if (RDateFrom.Trim().Length == 0)
        //    {
        //        RDateFrom = "2000-01-01";
        //    }
        //    if (RDateTo == null) RDateTo = "";
        //    if (RDateTo.Trim().Length == 0)
        //    {
        //        RDateTo = "2200-01-01";
        //    }

        //    ExcelPackage package = new ExcelPackage();
        //    ExcelWorksheet ws = package.Workbook.Worksheets.Add("Order Status");

        //    var lCompanyName = "";
        //    var lProjectTitle = "";

        //    lCompanyName = db.Customer.Find(CustomerCode).CustomerName;
        //    lProjectTitle = db.Project.Find(CustomerCode, ProjectCode).ProjectTitle;

        //    int lRowNo = 0;
        //    ws.Column(1).Width = 5;         //"SNo\n序号";
        //    ws.Column(2).Width = 12;        //"PO No\n加工表号";
        //    ws.Column(3).Width = 16;       //"Status\nStatus"
        //    ws.Column(4).Width = 14;        //"Prod Type\n加工表号";
        //    ws.Column(5).Width = 10;        //"WBS1\n楼座";
        //    ws.Column(6).Width = 10;        //"WBS2\n楼座";
        //    ws.Column(7).Width = 7;         //"WBS3\n楼座";
        //    ws.Column(8).Width = 14;        //"Structure Element";
        //    ws.Column(9).Width = 11;        //"PO Date\n订货日期";
        //    ws.Column(10).Width = 15;        //"Required Date\n到场日期";
        //    ws.Column(11).Width = 16;       //"Order WT\n料单重量";
        //    ws.Column(12).Width = 17;        //"Plan Delivery Date\n到场日期";
        //    ws.Column(13).Width = 15;        //"Transport Mode\n运输类型";
        //    ws.Column(14).Width = 15;        //"Delivery Qty\n送货数量";
        //    ws.Column(15).Width = 15;        //"Delivery Weight\n送货重量";
        //    ws.Column(16).Width = 15;        //"Vehicle No.\n货车号码";
        //    ws.Column(17).Width = 16;        //"Vehicle Out Time\n货车离厂时间";

        //    ws.Cells["A1:P1"].Merge = true;
        //    ws.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    ws.Cells[1, 1].Style.Font.Bold = true;
        //    ws.Cells[1, 1].Style.Font.Size = 12;
        //    ws.Cells[1, 1].Style.WrapText = true;
        //    ws.Cells[1, 1].Value = "Order Status\n订单状况";

        //    ws.Row(1).Height = 30;

        //    ws.Cells["A2:C2"].Merge = true;
        //    ws.Cells[2, 1].Value = "Company Name (公司名称):";
        //    ws.Cells[2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    ws.Cells[2, 1].Style.Font.Bold = true;
        //    ws.Cells["D2:P2"].Merge = true;
        //    ws.Cells[2, 4].Value = lCompanyName;
        //    ws.Cells[2, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //    ws.Cells[2, 4].Style.Font.Bold = true;

        //    ws.Cells["A3:C3"].Merge = true;
        //    ws.Cells[3, 1].Value = "Project Title (工程项目名称):";
        //    ws.Cells[3, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    ws.Cells[3, 1].Style.Font.Bold = true;

        //    ws.Cells["D3:P3"].Merge = true;
        //    ws.Cells[3, 4].Value = lProjectTitle;
        //    ws.Cells[3, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //    ws.Cells[3, 4].Style.Font.Bold = true;

        //    lRowNo = 4;

        //    ws.Row(4).Height = 30;
        //    ws.Cells[lRowNo, 1].Value = "SNo\n序号";
        //    ws.Cells[lRowNo, 2].Value = "PO No\n订单号码";
        //    ws.Cells[lRowNo, 3].Value = "Order Status\n订单状况";
        //    ws.Cells[lRowNo, 4].Value = "Product Type\n产品类型";
        //    ws.Cells[lRowNo, 5].Value = "WBS1\n楼座";
        //    ws.Cells[lRowNo, 6].Value = "WBS2\n楼层";
        //    ws.Cells[lRowNo, 7].Value = "WBS3\n分部";
        //    ws.Cells[lRowNo, 8].Value = "Structure Element\n构件";
        //    ws.Cells[lRowNo, 9].Value = "PO Date\n订货日期";
        //    ws.Cells[lRowNo, 10].Value = "Required Date\n到场日期";
        //    ws.Cells[lRowNo, 11].Value = "Order Weight\n料单重量 (KG)";
        //    ws.Cells[lRowNo, 12].Value = "Delivery Date\n到场日期";
        //    ws.Cells[lRowNo, 13].Value = "Transport Mode\n运输类型";
        //    ws.Cells[lRowNo, 14].Value = "Delivery Qty\n送货数量";
        //    ws.Cells[lRowNo, 15].Value = "Delivery Weight\n送货重量 (MT)";
        //    ws.Cells[lRowNo, 16].Value = "Vehicle No.\n货车号码";
        //    ws.Cells[lRowNo, 17].Value = "Vehicle Out Time\n货车离厂时间";

        //    ws.Cells[lRowNo, 1].Style.WrapText = true;
        //    ws.Cells[lRowNo, 2].Style.WrapText = true;
        //    ws.Cells[lRowNo, 3].Style.WrapText = true;
        //    ws.Cells[lRowNo, 4].Style.WrapText = true;
        //    ws.Cells[lRowNo, 5].Style.WrapText = true;
        //    ws.Cells[lRowNo, 6].Style.WrapText = true;
        //    ws.Cells[lRowNo, 7].Style.WrapText = true;
        //    ws.Cells[lRowNo, 8].Style.WrapText = true;
        //    ws.Cells[lRowNo, 9].Style.WrapText = true;
        //    ws.Cells[lRowNo, 10].Style.WrapText = true;
        //    ws.Cells[lRowNo, 11].Style.WrapText = true;
        //    ws.Cells[lRowNo, 12].Style.WrapText = true;
        //    ws.Cells[lRowNo, 13].Style.WrapText = true;
        //    ws.Cells[lRowNo, 14].Style.WrapText = true;
        //    ws.Cells[lRowNo, 15].Style.WrapText = true;
        //    ws.Cells[lRowNo, 16].Style.WrapText = true;
        //    ws.Cells[lRowNo, 17].Style.WrapText = true;

        //    ws.Cells[lRowNo, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //    ws.Cells[lRowNo, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //    ws.Cells[lRowNo, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //    ws.Cells[lRowNo, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //    ws.Cells[lRowNo, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //    ws.Cells[lRowNo, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //    ws.Cells[lRowNo, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //    ws.Cells[lRowNo, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //    ws.Cells[lRowNo, 9].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //    ws.Cells[lRowNo, 10].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //    ws.Cells[lRowNo, 11].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //    ws.Cells[lRowNo, 12].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //    ws.Cells[lRowNo, 13].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //    ws.Cells[lRowNo, 14].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //    ws.Cells[lRowNo, 15].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //    ws.Cells[lRowNo, 16].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //    ws.Cells[lRowNo, 17].Style.Border.BorderAround(ExcelBorderStyle.Thin);

        //    lRowNo = 5;

        //    var lStatus = "";
        //    var lCmd = new SqlCommand();
        //    SqlDataReader lRst;
        //    var lNDSCon = new SqlConnection();
        //    var lReturn = (new[]{ new
        //    {
        //        SSNNo = 0,
        //        ProdType = "",
        //        PONo = "",
        //        WBS1 = "",
        //        WBS2 = "",
        //        WBS3 = "",
        //        StructureElement = "",
        //        PODate = "",
        //        RequiredDate = "",
        //        OrderWeight = "",
        //        DeliveryDate = "",
        //        TransportMode = "",
        //        LoadQty = "",
        //        LoadWT = "",
        //        DeliveryStatus = "",
        //        VehicleNo = "",
        //        OutTime = ""
        //    }}).ToList();
        //    if (CustomerCode != null && ProjectCode != null)
        //    {
        //        if (CustomerCode.Length > 0 && ProjectCode.Length > 0)
        //        {

        //            lCmd.CommandText =
        //            "SELECT " +
        //            "(CASE WHEN " +
        //            "(SELECT isNull(sum(BarTotalQty), 0) FROM dbo.OESOrderDetails " +
        //            " WHERE CustomerCode = A.CustomerCode " +
        //            "AND ProjectCode = A.ProjectCode " +
        //            "AND JobID = A.JobID " +
        //            "AND Cancelled is null " +
        //            "AND BarCAB = '1' " +
        //            "AND(BarShapeCode like 'C%' " +
        //            "OR BarShapeCode like 'P%' " +
        //            "OR BarShapeCode like 'N%' " +
        //            "OR BarShapeCode like 'S%' " +
        //            "OR BarShapeCode like 'H%' " +
        //            "OR BarShapeCode like 'I%' " +
        //            "OR BarShapeCode like 'J%' " +
        //            "OR BarShapeCode like 'K%' " +
        //            "OR BarShapeCode like 'L%')) > 0 AND " +
        //            "(SELECT isNull(sum(BarTotalQty), 0) FROM dbo.OESOrderDetails " +
        //            "WHERE CustomerCode = A.CustomerCode " +
        //            "AND ProjectCode = A.ProjectCode " +
        //            "AND JobID = A.JobID " +
        //            "AND Cancelled is null " +
        //            "AND BarCAB = '1' " +
        //            "AND BarShapeCode not like 'C%' " +
        //            "AND BarShapeCode not like 'P%' " +
        //            "AND BarShapeCode not like 'N%' " +
        //            "AND BarShapeCode not like 'S%' " +
        //            "AND BarShapeCode not like 'H%' " +
        //            "AND BarShapeCode not like 'I%' " +
        //            "AND BarShapeCode not like 'J%' " +
        //            "AND BarShapeCode not like 'K%' " +
        //            "AND BarShapeCode not like 'L%') > 0 " +
        //            "THEN 'CAB/' + A.CouplerType " +
        //            "WHEN (SELECT isNull(sum(BarTotalQty), 0) FROM dbo.OESOrderDetails " +
        //            "WHERE CustomerCode = A.CustomerCode " +
        //            "AND ProjectCode = A.ProjectCode " +
        //            "AND JobID = A.JobID " +
        //            "AND Cancelled is null " +
        //            "AND BarCAB = '1' " +
        //            "AND(BarShapeCode like 'C%' " +
        //            "OR BarShapeCode like 'P%' " +
        //            "OR BarShapeCode like 'N%' " +
        //            "OR BarShapeCode like 'S%' " +
        //            "OR BarShapeCode like 'H%' " +
        //            "OR BarShapeCode like 'I%' " +
        //            "OR BarShapeCode like 'J%' " +
        //            "OR BarShapeCode like 'K%' " +
        //            "OR BarShapeCode like 'L%')) > 0 " +
        //            "THEN A.CouplerType " +
        //            "WHEN (SELECT isNull(sum(BarTotalQty), 0) FROM dbo.OESOrderDetails " +
        //            "WHERE CustomerCode = A.CustomerCode " +
        //            "AND ProjectCode = A.ProjectCode " +
        //            "AND JobID = A.JobID " +
        //            "AND Cancelled is null " +
        //            "AND BarCAB = '1') > 0 " +
        //            "THEN 'CAB' " +
        //            "ELSE 'Standard Bar' " +
        //            "END " +
        //            ") as ProdType, " +
        //            "PONumber, " +
        //            "WBS1, " +
        //            "WBS2, " +
        //            "WBS3, " +
        //            "(SELECT STUFF((SELECT ',' + BBSStrucElem from dbo.OESBBS " +
        //            "WHERE CustomerCode = A.CustomerCode " +
        //            "AND ProjectCode = A.ProjectCode " +
        //            "AND JobID = A.JobID " +
        //            "GROUP BY BBSStrucElem " +
        //            "for xml path('')),1,1,'')) as StructureEle, " +
        //            "PODate, " +
        //            "RequiredDate, " +
        //            "TotalWeight, " +
        //            "MIN(Delivery_Date), " +
        //            "(select vchTransportDescription from dbo.TransportMaster where tntStatusID = 1 and vchTransportMode = MAX(A.TransportMode)), " +
        //            "SUM(LoadQty), " +
        //            "SUM(LoadWT), " +
        //            "MAX(Delivery_Status), " +
        //            "MAX(Vehicle_No), " +
        //            "MAX(Vehicle_out_time), " +
        //            "A.OrderStatus " +
        //            "FROM dbo.OESJobAdvice A LEFT OUTER JOIN dbo.OESDelivery D " +
        //            "ON A.CustomerCode = D.CustomerCode " +
        //            "AND A.ProjectCode = D.ProjectCode " +
        //            "AND A.JobID = D.JobID " +
        //            "WHERE A.CustomerCode = '" + CustomerCode + "' " +
        //            "AND A.ProjectCode = '" + ProjectCode + "' " +
        //            "AND A.PODate >= '" + PODateFrom + "' " +
        //            "AND A.PODate <= '" + PODateTo + "' " +
        //            "AND A.RequiredDate >= '" + RDateFrom + "' " +
        //            "AND A.RequiredDate <= '" + RDateTo + "' " +
        //            "AND A.PONumber like '%" + PONumber + "%' " +
        //            "AND A.OrderStatus <> 'New' " +
        //            "AND A.OrderStatus <> 'Created' " +
        //            "GROUP BY " +
        //            "A.CustomerCode, " +
        //            "A.ProjectCode, " +
        //            "A.JobID, " +
        //            "A.CouplerType, " +
        //            "PONumber, " +
        //            "WBS1, " +
        //            "WBS2, " +
        //            "WBS3, " +
        //            "PODate, " +
        //            "RequiredDate, " +
        //            "TotalWeight, " +
        //            "A.OrderStatus " +

        //            "UNION " +
        //            "SELECT " +
        //            "isNULL((CASE WHEN( " +
        //            "(SELECT COUNT(*) " +
        //            "FROM dbo.OESStdProdDetails " +
        //            "WHERE CustomerCode = A.CustomerCode " +
        //            "AND ProjectCode = A.ProjectCode " +
        //            "and JobID = A.JobID) > 0) THEN " +
        //            "(SELECT DISTINCT 'Standard MESH/' " +
        //            "FROM dbo.OESStdSheetDetails " +
        //            "WHERE CustomerCode = A.CustomerCode " +
        //            "AND ProjectCode = A.ProjectCode " +
        //            "and JobID = A.JobID) " +
        //            "ELSE " +
        //            "(SELECT DISTINCT 'Standard MESH' " +
        //            "FROM dbo.OESStdSheetDetails " +
        //            "WHERE CustomerCode = A.CustomerCode " +
        //            "AND ProjectCode = A.ProjectCode " +
        //            "and JobID = A.JobID) END), '') + " +
        //            "isNULL((SELECT STUFF(( " +
        //            "SELECT '/' + ProdType " +
        //            "FROM dbo.OESStdProdDetails " +
        //            "WHERE CustomerCode = A.CustomerCode " +
        //            "AND ProjectCode = A.ProjectCode " +
        //            "and JobID = A.JobID " +
        //            "GROUP BY ProdType " +
        //            "FOR XML PATH('') ), 1,1,'')), '') as ProdType, " +
        //            "PONumber, " +
        //            "'', " +
        //            "'', " +
        //            "'', " +
        //            "'', " +
        //            "PODate, " +
        //            "RequiredDate, " +
        //            "TotalWeight, " +
        //            "MIN(Delivery_Date), " +
        //            "(select vchTransportDescription from dbo.TransportMaster where tntStatusID = 1 and vchTransportMode = MAX(A.Transport)), " +
        //            "SUM(LoadQty), " +
        //            "SUM(LoadWT), " +
        //            "MAX(Delivery_Status), " +
        //            "MAX(Vehicle_No), " +
        //            "MAX(Vehicle_out_time), " +
        //            "A.OrderStatus " +
        //            "FROM dbo.OESStdSheetJobAdvice A LEFT OUTER JOIN dbo.OESStdSheetDelivery D " +
        //            "ON A.CustomerCode = D.CustomerCode " +
        //            "AND A.ProjectCode = D.ProjectCode " +
        //            "AND A.JobID = D.JobID " +
        //            "WHERE A.CustomerCode = '" + CustomerCode + "' " +
        //            "AND A.ProjectCode = '" + ProjectCode + "' " +
        //            "AND A.PODate >= '" + PODateFrom + "' " +
        //            "AND A.PODate <= '" + PODateTo + "' " +
        //            "AND A.RequiredDate >= '" + RDateFrom + "' " +
        //            "AND A.RequiredDate <= '" + RDateTo + "' " +
        //            "AND A.PONumber like '%" + PONumber + "%' " +
        //            "AND A.OrderStatus <> 'New' " +
        //            "AND A.OrderStatus <> 'Created' " +
        //            "GROUP BY " +
        //            "A.CustomerCode, " +
        //            "A.ProjectCode, " +
        //            "A.JobID, " +
        //            "PONumber, " +
        //            "PODate, " +
        //            "RequiredDate, " +
        //            "TotalWeight, " +
        //            "A.OrderStatus " +

        //            "UNION " +
        //            "SELECT " +
        //            "'BPC' as ProdType, " +
        //            "PONumber, " +
        //            "'', " +
        //            "'', " +
        //            "'', " +
        //            "'Pile', " +
        //            "PODate, " +
        //            "RequiredDate, " +
        //            "TotalWeight, " +
        //            "MIN(Delivery_Date), " +
        //            "(select vchTransportDescription from dbo.TransportMaster where tntStatusID = 1 and vchTransportMode = MAX(A.Transport)), " +
        //            "SUM(LoadQty), " +
        //            "SUM(LoadWT), " +
        //            "MAX(Delivery_Status), " +
        //            "MAX(Vehicle_No), " +
        //            "MAX(Vehicle_out_time), " +
        //            "A.OrderStatus " +
        //            "FROM dbo.OESBPCJobAdvice A LEFT OUTER JOIN dbo.OESBPCDelivery D " +
        //            "ON A.CustomerCode = D.CustomerCode " +
        //            "AND A.ProjectCode = D.ProjectCode " +
        //            "AND A.JobID = D.JobID " +
        //            "WHERE A.CustomerCode = '" + CustomerCode + "' " +
        //            "AND A.ProjectCode = '" + ProjectCode + "' " +
        //            "AND A.Template = 0 " +
        //            "AND A.PODate >= '" + PODateFrom + "' " +
        //            "AND A.PODate <= '" + PODateTo + "' " +
        //            "AND A.RequiredDate >= '" + RDateFrom + "' " +
        //            "AND A.RequiredDate <= '" + RDateTo + "' " +
        //            "AND A.PONumber like '%" + PONumber + "%' " +
        //            "AND A.OrderStatus <> 'New' " +
        //            "AND A.OrderStatus <> 'Created' " +
        //            "GROUP BY " +
        //            "A.CustomerCode, " +
        //            "A.ProjectCode, " +
        //            "A.JobID, " +
        //            "PONumber, " +
        //            "PODate, " +
        //            "RequiredDate, " +
        //            "TotalWeight, " +
        //            "A.OrderStatus " +

        //            "UNION " +
        //            "SELECT " +
        //            "'CTS MESH' as ProdType, " +
        //            "PONumber, " +
        //            "WBS1, " +
        //            "WBS2, " +
        //            "WBS3, " +
        //            "(SELECT STUFF((SELECT ',' + BBSStrucElem from dbo.OESCTSMESHBBS " +
        //            "WHERE CustomerCode = A.CustomerCode " +
        //            "AND ProjectCode = A.ProjectCode " +
        //            "AND JobID = A.JobID " +
        //            "AND BBSOrder = 1 " +
        //            "GROUP BY BBSStrucElem " +
        //            "for xml path('')),1,1,'')) as StructureEle, " +
        //            "PODate, " +
        //            "A.RequiredDate, " +
        //            "SUM(A.TotalWeight), " +
        //            "Delivery_Date, " +
        //            "isNull((select vchTransportDescription from dbo.TransportMaster where tntStatusID = 1 and vchTransportMode = MAX(A.Transport)),''), " +
        //            "SUM(LoadQty), " +
        //            "SUM(LoadWT), " +
        //            "Delivery_Status, " +
        //            "Vehicle_No, " +
        //            "Vehicle_out_time, " +
        //            "A.OrderStatus " +
        //            "FROM dbo.OESCTSMESHJobAdvice A LEFT OUTER JOIN dbo.OESCTSMESHDelivery D " +
        //            "ON A.CustomerCode = D.CustomerCode " +
        //            "AND A.ProjectCode = D.ProjectCode " +
        //            "AND A.JobID = D.JobID " +
        //            "WHERE A.CustomerCode = '" + CustomerCode + "' " +
        //            "AND A.ProjectCode = '" + ProjectCode + "' " +
        //            "AND A.PODate >= '" + PODateFrom + "' " +
        //            "AND A.PODate <= '" + PODateTo + "' " +
        //            "AND A.RequiredDate >= '" + RDateFrom + "' " +
        //            "AND A.RequiredDate <= '" + RDateTo + "' " +
        //            "AND A.PONumber like '%" + PONumber + "%' " +
        //            "AND A.OrderStatus <> 'New' " +
        //            "AND A.OrderStatus <> 'Created' " +
        //            "GROUP BY " +
        //            "A.CustomerCode, " +
        //            "A.ProjectCode, " +
        //            "A.JobID, " +
        //            "PONumber, " +
        //            "WBS1, " +
        //            "WBS2, " +
        //            "WBS3, " +
        //            "PODate, " +
        //            "A.RequiredDate, " +
        //            "Delivery_Date, " +
        //            "Delivery_Status, " +
        //            "Vehicle_No, " +
        //            "Vehicle_out_time, " +
        //            "A.OrderStatus " +

        //            "ORDER BY 7, 2 ";

        //            var lProcessObj = new ProcessController();
        //            if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
        //            {
        //                lCmd.Connection = lNDSCon;
        //                lCmd.CommandTimeout = 300;
        //                lRst = lCmd.ExecuteReader();
        //                if (lRst.HasRows)
        //                {
        //                    var j = 0;
        //                    while (lRst.Read())
        //                    {
        //                        lStatus = lRst.GetValue(13) == DBNull.Value ? "" : lRst.GetString(13);
        //                        if (lStatus == "")
        //                        {
        //                            if ((lRst.GetValue(16) == DBNull.Value ? "" : lRst.GetString(16)) == "Processed")
        //                            {
        //                                lStatus = "Order Received";
        //                            }
        //                            else if ((lRst.GetValue(16) == DBNull.Value ? "" : lRst.GetString(16)) == "Cancelled")
        //                            {
        //                                lStatus = "Order Cancelled";
        //                            }
        //                            else
        //                            {
        //                                lStatus = "Order Submitted";
        //                            }
        //                        }

        //                        ws.Cells[j + lRowNo, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                        ws.Cells[j + lRowNo, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                        ws.Cells[j + lRowNo, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                        ws.Cells[j + lRowNo, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                        ws.Cells[j + lRowNo, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                        ws.Cells[j + lRowNo, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                        ws.Cells[j + lRowNo, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                        ws.Cells[j + lRowNo, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                        ws.Cells[j + lRowNo, 9].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                        ws.Cells[j + lRowNo, 10].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                        ws.Cells[j + lRowNo, 11].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                        ws.Cells[j + lRowNo, 12].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                        ws.Cells[j + lRowNo, 13].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                        ws.Cells[j + lRowNo, 14].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                        ws.Cells[j + lRowNo, 15].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                        ws.Cells[j + lRowNo, 16].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                        ws.Cells[j + lRowNo, 17].Style.Border.BorderAround(ExcelBorderStyle.Thin);

        //                        ws.Cells[j + lRowNo, 1].Value = j + 1;
        //                        ws.Cells[j + lRowNo, 2].Value = (lRst.GetValue(1) == DBNull.Value ? "" : lRst.GetString(1).Trim()); //"PO No\n加工表号";
        //                        ws.Cells[j + lRowNo, 3].Value = lStatus;
        //                        ws.Cells[j + lRowNo, 4].Value = lRst.GetString(0);
        //                        ws.Cells[j + lRowNo, 5].Value = (lRst.GetValue(2) == DBNull.Value ? "" : lRst.GetString(2).Trim()); //"WBS1\n楼座";
        //                        ws.Cells[j + lRowNo, 6].Value = (lRst.GetValue(3) == DBNull.Value ? "" : lRst.GetString(3).Trim()); //"WBS2\n楼层";
        //                        ws.Cells[j + lRowNo, 7].Value = (lRst.GetValue(4) == DBNull.Value ? "" : lRst.GetString(4).Trim()); //"WBS3\n分部";
        //                        ws.Cells[j + lRowNo, 8].Value = (lRst.GetValue(5) == DBNull.Value ? "" : lRst.GetString(5).Trim()); //"Structure Element\n分部";
        //                        ws.Cells[j + lRowNo, 9].Value = (lRst.GetValue(6) == DBNull.Value ? "" : lRst.GetDateTime(6).ToString("yyyy-MM-dd")); //"PO Date\n订货日期";
        //                        ws.Cells[j + lRowNo, 10].Value = (lRst.GetValue(7) == DBNull.Value ? "" : lRst.GetDateTime(7).ToString("yyyy-MM-dd")); //"Required Date\n到场日期";
        //                        ws.Cells[j + lRowNo, 11].Value = lRst.GetDecimal(8).ToString("###,###,###.000;(###,###.000); ");
        //                        ws.Cells[j + lRowNo, 12].Value = (lRst.GetValue(9) == DBNull.Value ? "" : lRst.GetDateTime(9).ToString("yyyy-MM-dd")); //Plan Delivery Date\n到场日期
        //                        ws.Cells[j + lRowNo, 13].Value = (lRst.GetValue(10) == DBNull.Value ? "" : lRst.GetString(10)); //Transport Mode\n运输类型
        //                        ws.Cells[j + lRowNo, 14].Value = (lRst.GetValue(11) == DBNull.Value ? "" : lRst.GetInt32(11).ToString()); //Delivery Qty\n送货数量
        //                        ws.Cells[j + lRowNo, 15].Value = (lRst.GetValue(12) == DBNull.Value ? "" : lRst.GetDecimal(12).ToString("###,###,###.000;(###,###.000); ")); //Delivery Weight\n送货重量
        //                        ws.Cells[j + lRowNo, 16].Value = (lRst.GetValue(14) == DBNull.Value ? "" : lRst.GetString(14)); //Vehicle No.\n货车号码
        //                        ws.Cells[j + lRowNo, 17].Value = (lRst.GetValue(15) == DBNull.Value ? "" : lRst.GetString(15)); //Vehicle Out Time\n货车离厂时间

        //                        ws.Cells[j + lRowNo, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                        ws.Cells[j + lRowNo, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                        ws.Cells[j + lRowNo, 11].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //                        ws.Cells[j + lRowNo, 12].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                        ws.Cells[j + lRowNo, 14].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //                        ws.Cells[j + lRowNo, 15].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

        //                        j = j + 1;

        //                    }
        //                }
        //                lRst.Close();

        //                lProcessObj.CloseNDSConnection(ref lNDSCon);
        //            }
        //            lProcessObj = null;
        //        }
        //    }
        //    lCmd = null;
        //    lNDSCon = null;
        //    lRst = null;

        //    MemoryStream ms = new MemoryStream();
        //    package.SaveAs(ms);

        //    var bExcel = new byte[ms.Length];
        //    ms.Position = 0;
        //    ms.Read(bExcel, 0, bExcel.Length);

        //    //bPDF = ms.GetBuffer();
        //    ms.Flush();
        //    ms.Dispose();

        //    return Json(bExcel);
        //}

        //Commented by ajit
        //public byte[] GetPDF(string pHTML)
        //{
        //    byte[] bPDF = null;

        //    MemoryStream ms = new MemoryStream();
        //    TextReader txtReader = new StringReader(pHTML);

        //    // 1: create object of a itextsharp document class  
        //    Document doc = new Document(PageSize.A4, 25, 25, 25, 25);

        //    // 2: we create a itextsharp pdfwriter that listens to the document and directs a XML-stream to a file  
        //    PdfWriter oPdfWriter = PdfWriter.GetInstance(doc, ms);

        //    // 3: we create a worker parse the document  
        //    HTMLWorker htmlWorker = new HTMLWorker(doc);

        //    // 4: we open document and start the worker on the document  
        //    doc.Open();
        //    htmlWorker.StartDocument();


        //    // 5: parse the html into the document  
        //    htmlWorker.Parse(txtReader);

        //    // 6: close the document and the worker  
        //    htmlWorker.EndDocument();
        //    htmlWorker.Close();
        //    doc.Close();

        //    bPDF = ms.ToArray();

        //    return bPDF;
        //}


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        [HttpGet]
        [Route("/UpdateWT/{productType}/{OrderNo}")]
        public async Task<ActionResult> UpdateWT(string productType, int OrderNo)
        {
            try
            {
                string lSQL = "";
                var lCmd = new SqlCommand();
                var lNDSCon = new SqlConnection();
                SqlDataReader lRst1;
                var lProcessObj1 = new ProcessController();
                if (lProcessObj1.OpenNDSConnection(ref lNDSCon) == true)
                {
                    //if (productType.Trim() == "STIRRUP-LINK-MESH" || productType.Trim() == "PRE-CAGE" || productType.Trim() == "CUT-TO-SIZE-MESH" || productType.Trim() == "CARPET" || productType.Trim() == "COLUMN-LINK-MESH" || productType.Trim() == "ACS" || productType.Trim() == "BPC" || productType.Trim() == "CORE-CAGE")

                    if (productType.Trim() == "CAB")
                    {
                        decimal lWtObj = 0;

                        lSQL = "select sum(Barweight) from oesorderdetails od inner join oesprojorder po on od.CustomerCode = po.CustomerCode and " +
                        "od.ProjectCode = po.ProjectCode inner join OESProjOrdersSE pse on po.OrderNumber = pse.OrderNumber and pse.CABJobID = " +
                        "od.JobID where po.OrderNumber = " + OrderNo + " and od.Cancelled is null group by od.CustomerCode, od.ProjectCode, od.jobid";

                        lCmd.CommandText = lSQL;
                        lCmd.Connection = lNDSCon;
                        lCmd.CommandTimeout = 300;
                        lRst1 = lCmd.ExecuteReader();
                        if (lRst1.HasRows)
                        {
                            while (lRst1.Read())
                            {
                                lWtObj = lRst1.GetValue(0) == DBNull.Value ? 0 : lRst1.GetDecimal(0);
                            }

                        }
                        lRst1.Close();
                        if (lWtObj != 0)
                        {
                            lSQL = "update dbo.OESProjOrder SET TotalWeight=(" + lWtObj + ")   where OrderNumber=" + OrderNo + "";
                            lCmd.CommandText = lSQL;
                            lCmd.Connection = lNDSCon;
                            lCmd.CommandTimeout = 300;
                            lCmd.ExecuteNonQuery();

                            lSQL = "update dbo.OESProjOrdersSE SET TotalWeight=(" + lWtObj + ") where OrderNumber=" + OrderNo + "";
                            lCmd.CommandText = lSQL;
                            lCmd.Connection = lNDSCon;
                            lCmd.CommandTimeout = 300;
                            lCmd.ExecuteNonQuery();
                        }
                    }


                    lProcessObj1.CloseNDSConnection(ref lNDSCon);
                }
                lCmd = null;
                lNDSCon = null;
                lRst1 = null;
                return Ok(true);
            }
            catch (Exception ex)
            {

                return Ok(false);
            }
        }
    }
}





