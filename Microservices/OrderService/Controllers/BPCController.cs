//using System;
using Apitron.PDF.Kit.FixedLayout.Content;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
//using AntiForgeryHeader.Helper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NCalc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Oracle.ManagedDataAccess.Client;
using OrderService.Context;
using OrderService.Dtos;
//using System.Web;
//using System.Web.Mvc;
using OrderService.Models;
using OrderService.Repositories;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlBuilder;

namespace OrderService.Controllers
{
    //[Authorize]
    public class BPCController : Controller 
    {
        public string gUserType = "";
        public string gGroupName = "";



        struct struOrderList
        {
            public string OrderNo;
            public string OrderDesc;
        };
        PrecastService precastService = new PrecastService();
        private DBContextModels db = new DBContextModels();
        string mbshapeCode;
        int mbB = 0;
        int mbC = 0;

        //[ValidateAntiForgeryToken]
        [HttpGet]
        [Route("/getShapeInfo_bpc")]
        public ActionResult Index(string appCustomerCode, string appProjectCode, int appSelectedCount,
        string appSelectedSE, string appSelectedProd, string appSelectedPostID, string appSelectedScheduled,
        string appSelectedWBS1, string appSelectedWBS2, string appSelectedWBS3,
        string appSelectedWT, string appSelectedQty,
        string appWBS1S, string appWBS2S, string appWBS3S, string appOrderNoS,
        string appStructureElement, string appProductType, string appScheduledProd, int appPostID,
        string appOrderNo, string appWBS1, string appWBS2, string appWBS3, string UserName)
        {
            UserAccessController lUa = new UserAccessController();
            var lUserType = lUa.getUserType(UserName);
            var lGroupName = lUa.getGroupName(UserName);

            ViewBag.UserType = lUserType;

            string lUserName = UserName;//"Vishalw_ttl@natsteel.com.sg";

            //if (lUserName.IndexOf("@") > 0)
            //{
            //    lUserName = lUserName.Substring(0, lUserName.IndexOf("@"));
            //}

            ViewBag.UserName = lUserName;

            string lUserDomain = "";
            if (lUserName.IndexOf("@") > 0)
            {
                lUserDomain = lUserName.Split('@')[1].ToLower();
            }
            ViewBag.Domain = lUserDomain == null ? "" : lUserDomain.ToLower();

            lUa = null;

            CustomerModels lCustomerModel = db.Customer.Find(appCustomerCode);

            ViewBag.CustomerSelection = new SelectList(new List<SelectListItem>
            { new SelectListItem
            {
                Value = appCustomerCode,
                Text = lCustomerModel == null? "":lCustomerModel.CustomerName
            }
            }, "Value", "Text");

            //var lProjectModel = (from p in db.ProjectList
            //                     where p.ProjectCode == appProjectCode
            //                     select p).First();

            var lSharedAPI = new SharedAPIController();

            ViewBag.ProjectSelection = new SelectList(new List<SelectListItem>
            { new SelectListItem
            {
                Value = appProjectCode,
                //Text = lProjectModel == null? "":lProjectModel.ProjectTitle
                Text = lSharedAPI.getProjectTitle(appCustomerCode, appProjectCode, lUserType, lGroupName)
            }
            }, "Value", "Text");

            lSharedAPI = null;

            int lJobID = 0;
            string lOrderStatus = "New";
            string lPONo = "";

            int lOrderNoT = 0;
            int.TryParse(appOrderNo, out lOrderNoT);

            var lSE = db.OrderProjectSE.Find(lOrderNoT, appStructureElement, appProductType, appScheduledProd);
            if (lSE != null)
            {
                lJobID = lSE.BPCJobID;
                lPONo = lSE.PONumber == null ? "" : lSE.PONumber;
            }

            var lJobAdv = db.BPCJobAdvice.Find(appCustomerCode, appProjectCode, false, lJobID);

            var lMain = db.OrderProject.Find(lOrderNoT);

            if (lMain != null)
            {
                lOrderStatus = lMain.OrderStatus;
            }
            if (lOrderStatus == null || lOrderStatus == "Reserved")
            {
                lOrderStatus = "New";
            }
            if (lUserName.Split('@').Length == 2)
            {
                if ((lOrderStatus == "Created*" || lOrderStatus == "Submitted*") && lUserName.Split('@')[1].ToLower().Trim() == "natsteel.com.sg")
                {
                    lOrderStatus = "Created";
                }
            }

            ViewBag.JobID = lJobID;

            ViewBag.PONo = lPONo;

            ViewBag.OrderStatus = lOrderStatus;

            ViewBag.SelectedCount = appSelectedCount;
            ViewBag.SelectedSE = appSelectedSE.Split(',');
            ViewBag.SelectedProd = appSelectedProd.Split(',');
            ViewBag.SelectedPostID = appSelectedPostID.Split(',');
            ViewBag.SelectedScheduled = appSelectedScheduled.Split(',');

            ViewBag.SelectedWT = appSelectedWT == null ? new string[] { } : appSelectedWT.Split(',');
            ViewBag.SelectedQty = appSelectedQty == null ? new string[] { } : appSelectedQty.Split(',');

            ViewBag.SelectedWBS1 = appSelectedWBS1.Split(',');
            ViewBag.SelectedWBS2 = appSelectedWBS2.Split(',');
            ViewBag.SelectedWBS3 = appSelectedWBS3.Split(',');

            ViewBag.WBS1 = appWBS1S.Split(',');
            ViewBag.WBS2 = appWBS2S.Split(',');
            ViewBag.WBS3 = appWBS3S.Split(',');

            ViewBag.WBS1T = appWBS1;
            ViewBag.WBS2T = appWBS2;
            ViewBag.WBS3T = appWBS3;
            ViewBag.OrderNo = appOrderNo;

            ViewBag.StandbyOrder = appScheduledProd;
            ViewBag.PostID = appPostID;
            ViewBag.StructureElement = appStructureElement;

            var lSubmission = "No";
            var lEditable = "No";

            //get Access right;
            if (lUserType == "CU" || lUserType == "CA" || lUserType == "CM")
            {
                var lAccess = db.UserAccess.Find(User.Identity.Name, appCustomerCode, appProjectCode);
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
            //ViewBag.AlertMessage = lSharedPrg.getAlertMessage(appCustomerCode, appProjectCode, lUserName, lSubmission, lEditable);
            //lSharedPrg = null;

            return View();
        }


        // GET: Customer
        [Route("Index_bpc")]
        [HttpGet]
        public ActionResult Index_bk()
        {
            UserAccessController lUa = new UserAccessController();
            var lUserType = lUa.getUserType("Vishalw_ttl@natsteel.com.sg");
            var lGroupName = lUa.getGroupName("Vishalw_ttl@natsteel.com.sg");

            gUserType = lUserType;
            gGroupName = lGroupName;

            ViewBag.UserType = lUserType;
            lUa = null;

            if (lUserType == "")
            {
                //return RedirectToAction("Index", "Home");
            }

            List<ProjectListModels> content = new List<ProjectListModels> {
                new ProjectListModels
                {
                    CustomerCode = "",
                    ProjectCode = "",
                    ProjectTitle = ""
                } };

            if (lUserType == "PL" || lUserType == "AD" || lUserType == "PM" || lUserType == "PA" || lUserType == "P1" || lUserType == "P2" || lUserType == "PU" || lUserType == "TE")
            {
                var content1 = (from p in db.Customer
                                where !p.CustomerName.StartsWith("(D)") &&
                                !p.CustomerName.Contains("DONOT USE") &&
                                !p.CustomerName.Contains("DONT USE") &&
                                !p.CustomerName.StartsWith("-CANCEL-") &&
                                !p.CustomerCode.Trim().Equals("") &&
                                !p.CustomerName.Trim().Equals("") &&
                                !p.CustomerName.Trim().Equals(".")
                                orderby p.CustomerName
                                select p
                               ).ToList();

                if (lUserType == "TE" || lUserType == "AD")
                {
                    var lStandardLib = new CustomerModels();
                    lStandardLib.CustomerCode = "0000000000";
                    lStandardLib.CustomerName = "COMMON BORED PILE CAGE";

                    content1.Insert(0, lStandardLib);
                }

                content1.Insert(0, new CustomerModels
                {
                    CustomerCode = "",
                    CustomerName = ""
                });


                ViewBag.CustomerSelection = new SelectList(new List<SelectListItem>(content1.Select(h => new SelectListItem
                {
                    Value = h.CustomerCode.Trim(),
                    Text = h.CustomerName.Trim() + ((h.CustomerCode.Trim() == "" || h.CustomerCode.Trim() == "0000000000") ? "" : (" (" + h.CustomerCode.Trim() + ")"))
                })), "Value", "Text");

                //if (content1.Count() > 0)
                //{
                //    var lCustomerCode = content1.First().CustomerCode;
                //    content = (from p in db.ProjectList
                //               where p.CustomerCode == lCustomerCode &&
                //               (from m in db.Project
                //                where m.CustomerCode == lCustomerCode
                //                select m.ProjectCode).Contains(p.ProjectCode)
                //               orderby p.ProjectTitle
                //               select p).ToList();
                //    content = content.OrderBy(o => o.ProjectTitle).ToList();

                //    if (content.Count() > 0)
                //    {
                //        content = RemoveNonBPCProject(content);
                //    }
                //}

                //if (lUserType == "TE" || lUserType == "AD")
                //{
                //    var lStandardLib = new ProjectListModels();
                //    lStandardLib.CustomerCode = "0000000000";
                //    lStandardLib.ProjectCode = "0000000000";
                //    lStandardLib.ProjectTitle = "NatSteel Standard BPC Library";

                //    content.Insert(0, lStandardLib);
                //}

                if (content.Count() == 0)
                {
                    content = new List<ProjectListModels> {
                        new ProjectListModels
                        {
                            CustomerCode = "",
                            ProjectCode = "",
                            ProjectTitle = ""
                        }
                    };
                }

                ViewBag.ProjectSelection = new SelectList(new List<SelectListItem>(content.Select(h => new SelectListItem
                {
                    Value = h.ProjectCode.Trim(),
                    //Text = h.ProjectTitle + "(" + h.ProjectCode + ")"
                    Text = h.ProjectTitle.Trim()
                })), "Value", "Text");
            }
            else
            {

                var content2 = (from p in db.Customer
                                where (from u in db.UserAccess
                                       where u.UserName == lGroupName &&
                                       p.CustomerCode != "0000000000"
                                       select u.CustomerCode).Contains(p.CustomerCode)
                                orderby p.CustomerName
                                select p).ToList();

                var content3 = new List<CustomerModels>(content2.Select(h => new CustomerModels
                {
                    CustomerCode = h.CustomerCode.Trim(),
                    CustomerName = h.CustomerName.Trim()
                }));
                content3 = content3.GroupBy(o => o.CustomerCode).Select(x => x.First()).ToList();

                ViewBag.CustomerSelection = new SelectList(new List<SelectListItem>(content3.Select(h => new SelectListItem
                {
                    Value = h.CustomerCode,
                    Text = h.CustomerName
                })), "Value", "Text");

                if (content2.Count() > 0)
                {
                    var lCustomerCode = content2.First().CustomerCode.Substring(0, 10);

                    content = (from p in db.ProjectList
                               where p.CustomerCode == lCustomerCode &&
                               (from u in db.UserAccess
                                where u.UserName == lGroupName &&
                                p.CustomerCode == lCustomerCode
                                select u.ProjectCode).Contains(p.ProjectCode)
                               orderby p.ProjectTitle
                               select p).ToList();

                    if (content.Count() > 0)
                    {
                        content = RemoveNonBPCProject(content);
                    }
                }

                if (content.Count() == 0)
                {
                    content = new List<ProjectListModels> {
                        new ProjectListModels
                        {
                            CustomerCode = "",
                            ProjectCode = "",
                            ProjectTitle = ""
                        }
                    };
                }

                ViewBag.ProjectSelection = new SelectList(new List<SelectListItem>(content.Select(h => new SelectListItem
                {
                    Value = h.ProjectCode,
                    Text = h.ProjectTitle
                })), "Value", "Text");

            }

            string lCustomerCodeD = "";
            //var lCustomerCK = Request.Cookies["nsh_digios_cust"]; vw
            var lCustomerCK = "";
            if (lCustomerCK != null)
            {
                //lCustomerCodeD = lCustomerCK.Value.ToString(); vw
            }
            ViewBag.CustomerCodeCK = lCustomerCodeD;

            string lProjectCodeD = "";
            //var lProjectCK = Request.Cookies["nsh_digios_proj"];
            var lProjectCK = "";
            if (lProjectCK != null)
            {
                //lProjectCodeD = lProjectCK.Value.ToString(); vw
            }
            ViewBag.ProjectCodeCK = lProjectCodeD;

            return View(content.First());
        }

        //[HttpPost]
        //[ValidateAntiForgeryHeader]
        //public ActionResult uploadPODocs()
        //{
        //    var lReturn = new CTSMESHPODocsFileModels();

        //    try
        //    {
        //        //var lCustomerCode = Request.Form.Get("CustomerCode");
        //        var lCustomerCode = "";
        //        //var lProjectCode = Request.Form.Get("ProjectCode");
        //        var lProjectCode = "";
        //        int lJobID = 0;
        //        int lDocID = 0;
        //        //int.TryParse(Request.Form.Get("JobID"), out lJobID); vw

        //        if (Request.Files.Count > 0)
        //        {
        //            HttpFileCollectionBase files = Request.Files;
        //            for (int i = 0; i < files.Count; i++)
        //            {
        //                HttpPostedFileBase file = files[i];

        //                var lFileName = file.FileName;
        //                if (lFileName.LastIndexOf("\\") >= 0)
        //                {
        //                    lFileName = lFileName.Substring(lFileName.LastIndexOf("\\") + 1);
        //                }
        //                if (lFileName.LastIndexOf("/") >= 0)
        //                {
        //                    lFileName = lFileName.Substring(lFileName.LastIndexOf("/") + 1);
        //                }

        //                byte[] pdfBytes = null;
        //                BinaryReader reader = new BinaryReader(file.InputStream);
        //                pdfBytes = reader.ReadBytes((int)file.ContentLength);

        //                lDocID = (from p in db.BPCPODoc
        //                          where p.CustomerCode == lCustomerCode &&
        //                          p.ProjectCode == lProjectCode &&
        //                          p.JobID == lJobID
        //                          select p.PODocID).DefaultIfEmpty(0).Max();
        //                lDocID = lDocID + 1;

        //                var Content = new BPCPODocsModels
        //                {
        //                    CustomerCode = lCustomerCode,
        //                    ProjectCode = lProjectCode,
        //                    JobID = lJobID,
        //                    PODocID = lDocID,
        //                    FileName = lFileName,
        //                    UpdatedDate = DateTime.Now,
        //                    UpdatedBy = User.Identity.Name,
        //                    PODoc = pdfBytes
        //                };
        //                db.BPCPODoc.Add(Content);
        //                db.SaveChanges();

        //                var lPODocExists = db.BPCPODoc.Where(
        //                                      p => p.CustomerCode == lCustomerCode &&
        //                                      p.ProjectCode == lProjectCode &&
        //                                      p.JobID == lJobID).Count();

        //                lReturn = new CTSMESHPODocsFileModels
        //                {
        //                    PODocID = lDocID,
        //                    FileName = lFileName,
        //                    UpdatedDate = DateTime.Now.ToString("yyyy-mm-dd hh:mm:ss"),
        //                    UpdatedBy = User.Identity.Name,
        //                    FileSize = (pdfBytes.Length / 1000).ToString(),
        //                    Exists = lPODocExists
        //                };
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError(string.Empty, "Server Error: " + ex.Message);
        //    }
        //    return Json(lReturn, JsonRequestBehavior.AllowGet);
        //}

        //download PO doc 
        [HttpGet]
        [Route("/deletePODocs_bpc/{CustomerCode}/{ProjectCode}/{JobID}/{PODocID}")]
        //[ValidateAntiForgeryHeader]
        public ActionResult deletePODocs(string CustomerCode, string ProjectCode, int JobID, int PODocID)
        {
            var lReturn = true;
            var lCmd = new SqlCommand();
            var lNDSCon = new SqlConnection();

            if (CustomerCode != null && ProjectCode != null)
            {
                if (CustomerCode.Length > 0 && ProjectCode.Length > 0)
                {
                    lCmd.CommandText =
                    "DELETE FROM dbo.OESBPCPODoc " +
                    "WHERE CustomerCode = '" + CustomerCode + "' " +
                    "AND ProjectCode = '" + ProjectCode + "' " +
                    "AND JobID = " + JobID + " " +
                    "AND PODocID = " + PODocID + " ";

                    var lProcessObj = new ProcessController();
                    try
                    {
                        if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
                        {
                            lCmd.Connection = lNDSCon;
                            lCmd.CommandTimeout = 300;
                            lCmd.ExecuteNonQuery();

                            lProcessObj.CloseNDSConnection(ref lNDSCon);
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, "Server Error: " + ex.Message);
                        lReturn = false;
                    }
                    lProcessObj = null;
                }
            }
            lCmd = null;
            lNDSCon = null;
            return Ok(lReturn);
        }

        //download PO doc 
        [HttpGet]
        [Route("/downloadPODocs_bpc/{CustomerCode}/{ProjectCode}/{JobID}/{PODocID}")]
        //[ValidateAntiForgeryHeader]
        public ActionResult downloadPODocs(string CustomerCode, string ProjectCode, int JobID, int PODocID)
        {
            var content = db.BPCPODoc.Find(CustomerCode, ProjectCode, JobID, PODocID);
            return Ok(content);
        }

        //check PO doc 
        [HttpGet]
        [Route("/checkPODocs_bpc/{CustomerCode}/{ProjectCode}/{JobID}")]
        //[ValidateAntiForgeryHeader]
        public ActionResult checkPODocs(string CustomerCode, string ProjectCode, int JobID)
        {
            var lCmd = new SqlCommand();
            SqlDataReader lRst;
            var lNDSCon = new SqlConnection();
            var lReturn = new List<CTSMESHPODocsFileModels>();
            var lRecord = new CTSMESHPODocsFileModels();

            if (CustomerCode != null && ProjectCode != null)
            {
                if (CustomerCode.Length > 0 && ProjectCode.Length > 0)
                {
                    lCmd.CommandText =
                    "SELECT PODocID, " +
                    "FileName, " +
                    "Convert(varchar, UpdatedDate, 120), " +
                    "UpdatedBy, " +
                    "str(DataLength(PODoc)/1000) " +
                    "FROM dbo.OESBPCPODoc " +
                    "WHERE CustomerCode = '" + CustomerCode + "' " +
                    "AND ProjectCode = '" + ProjectCode + "' " +
                    "AND JobID = " + JobID + " " +
                    "ORDER BY PODocID ";

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
                                lRecord = new CTSMESHPODocsFileModels
                                {
                                    PODocID = lRst.GetInt32(0),
                                    FileName = lRst.GetString(1).Trim(),
                                    UpdatedDate = lRst.GetString(2).Trim(),
                                    UpdatedBy = lRst.GetString(3).Trim(),
                                    FileSize = lRst.GetString(4).Trim(),
                                };
                                lReturn.Add(lRecord);
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

        //[HttpGet]
        //[Route("/sendOrderSubmittedEmail_bpc/{CustomerCode}/{ProjectCode}/{JobID}")]
        ////[ValidateAntiForgeryHeader]
        //public ActionResult sendOrderSubmittedEmail(string CustomerCode, string ProjectCode, int JobID)
        //{
        //    var lEmailContent = "";
        //    var lEmailFrom = "";
        //    var lEmailTo = "";
        //    var lEmailCc = "";
        //    var lEmailSubject = "";
        //    string lVar1 = "";
        //    bool lTemplate = false;

        //    var JobContent = db.BPCJobAdvice.Find(CustomerCode, ProjectCode, lTemplate, JobID);
        //    if (JobContent != null)
        //    {
        //        if (JobContent.CustomerCode == null) JobContent.CustomerCode = "";
        //        else JobContent.CustomerCode = JobContent.CustomerCode.Trim();

        //        if (JobContent.OrderStatus == null) JobContent.OrderStatus = "";
        //        else JobContent.OrderStatus = JobContent.OrderStatus.Trim();

        //        if (JobContent.PONumber == null) JobContent.PONumber = "";
        //        else JobContent.PONumber = JobContent.PONumber.Trim();

        //        if (JobContent.ProjectCode == null) JobContent.ProjectCode = "";
        //        else JobContent.ProjectCode = JobContent.ProjectCode.Trim();

        //        if (JobContent.DeliveryAddress == null) JobContent.DeliveryAddress = "";
        //        else JobContent.DeliveryAddress = JobContent.DeliveryAddress.Trim();

        //        if (JobContent.Remarks == null) JobContent.Remarks = "";
        //        else JobContent.Remarks = JobContent.Remarks.Trim();

        //        if (JobContent.Scheduler_HP == null) JobContent.Scheduler_HP = "";
        //        else JobContent.Scheduler_HP = JobContent.Scheduler_HP.Trim();

        //        if (JobContent.Scheduler_Name == null) JobContent.Scheduler_Name = "";
        //        else JobContent.Scheduler_Name = JobContent.Scheduler_Name.Trim();

        //        if (JobContent.Scheduler_Tel == null) JobContent.Scheduler_Tel = "";
        //        else JobContent.Scheduler_Tel = JobContent.Scheduler_Tel.Trim();

        //        if (JobContent.SiteEngr_HP == null) JobContent.SiteEngr_HP = "";
        //        else JobContent.SiteEngr_HP = JobContent.SiteEngr_HP.Trim();

        //        if (JobContent.SiteEngr_Name == null) JobContent.SiteEngr_Name = "";
        //        else JobContent.SiteEngr_Name = JobContent.SiteEngr_Name.Trim();

        //        if (JobContent.SiteEngr_Tel == null) JobContent.SiteEngr_Tel = "";
        //        else JobContent.SiteEngr_Tel = JobContent.SiteEngr_Tel.Trim();
        //    }

        //    lEmailContent = "<p align='center'>JOB ADVICE - Bored Pile Cage (工作通知 - 钻孔桩铁笼)</p>";

        //    lEmailContent = lEmailContent + "<p align='left' style='font-size:15px'><a target='_top' href='https://oes.natsteel.com.sg/process?ccode=" + JobContent.CustomerCode
        //        + "&jcode=" + JobContent.ProjectCode + "&prodtype=BPC" + "&jobid=" + JobContent.JobID.ToString()
        //        + "'>Click here to redirect to digital Ordering System to process it</a></p>";

        //    lEmailContent = lEmailContent + "<table border='1' width=100%><tr>";
        //    lEmailContent = lEmailContent + "<td width=20%>" + "Customer (客户名称)" + "</td>";

        //    CustomerModels lCustomer = db.Customer.Find(JobContent.CustomerCode);
        //    string lVar = "";
        //    if (lCustomer != null) lVar = lCustomer.CustomerName.Trim() + " (" + JobContent.CustomerCode.Trim() + ")";
        //    lEmailContent = lEmailContent + "<td>" + lVar + "</td></tr>";

        //    lEmailContent = lEmailContent + "<tr><td>" + "Project (工程项目)" + "</td>";

        //    var lProject = (from p in db.ProjectList
        //                    where p.ProjectCode == ProjectCode
        //                    select p).First();
        //    lVar = "";
        //    if (lProject != null) lVar = lProject.ProjectTitle.Trim() + " (" + JobContent.ProjectCode.Trim() + ")";
        //    lEmailContent = lEmailContent + "<td>" + lVar + "</td></tr></table>";

        //    lEmailContent = lEmailContent + "<table border='1' width=100%><tr>";

        //    lEmailContent = lEmailContent + "<td width=20%>" + "PO No. (订单号码)" + "</td>";
        //    lEmailContent = lEmailContent + "<td width=27%>" + JobContent.PONumber.Trim() + "</td>";
        //    lEmailContent = lEmailContent + "<td width=26%>" + "Order Date (订单日期)" + "</td>";
        //    lEmailContent = lEmailContent + "<td>" + String.Format("{0:yyyy-MM-dd}", JobContent.PODate) + "</td></tr>";

        //    lEmailContent = lEmailContent + "</tr><td width=20%>" + "Required Date (交货日期)" + "</td>";
        //    lEmailContent = lEmailContent + "<td width=27%>" + String.Format("{0:yyyy-MM-dd}", JobContent.RequiredDate) + "</td>";

        //    lEmailContent = lEmailContent + "<td width=26%>" + "Transport Mode (运输工具)" + "</td>";

        //    lVar = "";
        //    var lProcessObj = new ProcessController();

        //    var lCmd = new OracleCommand();
        //    OracleDataReader lRst;
        //    var lcisCon = new OracleConnection();

        //    if (lProcessObj.OpenCISConnection(ref lcisCon) == true)
        //    {

        //        lCmd.CommandText = "SELECT BEZEI as DESCRIPTION " +
        //        "FROM SAPSR3.TMFGT WHERE MANDT = '" + lProcessObj.strClient + "' AND SPRAS ='E' " +
        //        "AND MFRGR = '" + JobContent.Transport + "' ";

        //        lCmd.Connection = lcisCon;
        //        lCmd.CommandTimeout = 300;
        //        lRst = lCmd.ExecuteReader();
        //        if (lRst.HasRows)
        //        {
        //            if (lRst.Read())
        //            {
        //                lVar = lRst.GetString(0).Trim();
        //                lProcessObj.CloseCISConnection(ref lcisCon);
        //            }
        //        }
        //        lRst.Close();
        //    }
        //    lCmd = null;
        //    lcisCon = null;
        //    lRst = null;
        //    lProcessObj = null;



        //    lEmailContent = lEmailContent + "<td>" + lVar + "</td></tr>";

        //    lEmailContent = lEmailContent + "</tr><td width=20%>" + "Total Pieces (总件数)" + "</td>";
        //    lEmailContent = lEmailContent + "<td width=27%>" + ((int)JobContent.TotalPcs).ToString() + "</td>";

        //    lEmailContent = lEmailContent + "<td width=26%>" + "Total Weight (总重量)" + "</td>";

        //    lEmailContent = lEmailContent + "<td>" + ((decimal)JobContent.TotalWeight).ToString("F3") + " KG" + "</td></tr></table>";

        //    lEmailContent = lEmailContent + "<table border='1' width=100%><tr>";
        //    lEmailContent = lEmailContent + "<td width=20%>" + "Delivery Address (送货地址)" + "</td>";

        //    lVar = "&nbsp;";
        //    if (JobContent.DeliveryAddress != null) lVar = JobContent.DeliveryAddress.Trim();
        //    lEmailContent = lEmailContent + "<td>" + lVar + "</td></tr>";

        //    lEmailContent = lEmailContent + "<tr><td>" + "Remarks (备注)" + "</td>";

        //    lVar = "&nbsp;";
        //    if (JobContent.Remarks != null) lVar = JobContent.Remarks.Trim();
        //    lEmailContent = lEmailContent + "<td>" + lVar + "</td></tr></table>";

        //    lEmailContent = lEmailContent + "<table border='1' width=100%><tr>";
        //    lEmailContent = lEmailContent + "<td width='3%'>" + "S/N<br/>序号" + "</td>";
        //    lEmailContent = lEmailContent + "<td width='6%'>" + "Pile Diameter<br/>桩直径" + "</td>";
        //    lEmailContent = lEmailContent + "<td width='12%'>" + "Cage Type<br/>铁笼类型" + "</td>";
        //    lEmailContent = lEmailContent + "<td width='9%'>" + "No of Main Bars<br/>主筋数量" + "</td>";
        //    lEmailContent = lEmailContent + "<td width='7%'>" + "Main Bar Shape<br/>主筋图形" + "</td>";
        //    lEmailContent = lEmailContent + "<td width='5%'>" + "Cage Length<br/>铁笼长" + "</td>";
        //    lEmailContent = lEmailContent + "<td width='5%'>" + "Lap Length<br/>顶端重叠长" + "</td>";
        //    lEmailContent = lEmailContent + "<td width='14%'>" + "Link Dia-Spacing-Length<br/>弧状链的直径,间距与长度" + "</td>";
        //    lEmailContent = lEmailContent + "<td width='7%'>" + "End Length<br/>底端预留长度" + "</td>";
        //    lEmailContent = lEmailContent + "<td width='5%'>" + "Cage Qty<br/>铁笼件数" + "</td>";
        //    lEmailContent = lEmailContent + "<td width='11%'>" + "Combination of Cages<br/>上端, 中间还是低端铁笼" + "</td>";
        //    lEmailContent = lEmailContent + "<td width='6%'>" + "Weight(MT)<br/>重量(吨)" + "</td>";
        //    lEmailContent = lEmailContent + "<td width='5%'>" + "Per Set<br/>每组笼数" + "</td>";
        //    lEmailContent = lEmailContent + "<td width='5%'>" + "Cage Mark<br/>铁笼号码" + "</td>";
        //    lEmailContent = lEmailContent + "<td>" + "Remarks<br/>备注" + "</td>";
        //    lEmailContent = lEmailContent + "</td></tr>";

        //    var lBBSContent = (from p in db.BPCDetails
        //                       where p.CustomerCode == CustomerCode &&
        //                       p.ProjectCode == ProjectCode &&
        //                       p.Template == false &&
        //                       p.JobID == JobID
        //                       orderby p.cage_id
        //                       select p).ToList();

        //    if (lBBSContent.Count > 0)
        //    {
        //        for (int i = 0; i < lBBSContent.Count; i++)
        //        {
        //            lEmailContent = lEmailContent + "<tr><td align='center'> <font color='blue'>" + (i + 1).ToString() + "</font></td>";
        //            lEmailContent = lEmailContent + "<td align='center'><font color='blue'>" + lBBSContent[i].pile_dia.ToString() + "</font></td>";
        //            //Cage Type
        //            lVar = "";
        //            var lPileType = lBBSContent[i].pile_type;
        //            var lMainBarArrange = lBBSContent[i].main_bar_arrange;
        //            var lMainBarType = lBBSContent[i].main_bar_type;
        //            if (lPileType == "Single-Layer")
        //            {
        //                if (lMainBarType == "Single")
        //                {
        //                    if (lMainBarArrange == "Single")
        //                    {
        //                        lVar = "Single Layer";
        //                    }
        //                    else if (lMainBarArrange == "Side-By-Side")
        //                    {
        //                        lVar = "Single Layer<br/>Side-By-Side Bundled Bars";
        //                    }
        //                    else if (lMainBarArrange == "In-Out")
        //                    {
        //                        lVar = "Single Layer<br/>In-Out Bundled Bars";
        //                    }
        //                    else
        //                    {
        //                        lVar = "Single Layer<br/>Complex Bundled Bars";
        //                    }
        //                }
        //                if (lMainBarType == "Mixed")
        //                {
        //                    if (lMainBarArrange == "Single")
        //                    {
        //                        lVar = "Single Layer<br/>Mixed Bars";
        //                    }
        //                    else if (lMainBarArrange == "Side-By-Side")
        //                    {
        //                        lVar = "Single Layer<br/>Side By Side Bundled<br/>Mixed Bars";
        //                    }
        //                    else if (lMainBarArrange == "In-Out")
        //                    {
        //                        lVar = "Single Layer<br/>In-Out Bundled<br/>Mixed Bars";
        //                    }
        //                    else
        //                    {
        //                        lVar = "Single Layer<br/>Complex Bundled<br/>Mixed Bars";
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                if (lMainBarArrange == "Single")
        //                {
        //                    lVar = "Double Layer";
        //                }
        //                else if (lMainBarArrange == "Side-By-Side")
        //                {
        //                    lVar = "Double Layer<br/>Side By Side Bundled Bars";
        //                }
        //                else
        //                {
        //                    lVar = "Double Layer<br/>Complex Bundled Bars";
        //                }

        //            }

        //            lEmailContent = lEmailContent + "<td align='center'><font color='blue'>" + lVar + "</font></td>";

        //            lVar = "";
        //            var lBarCTArr = lBBSContent[i].main_bar_ct.Split(',');
        //            var lBarDiaArr = lBBSContent[i].main_bar_dia.Split(',');
        //            var lBarType = lBBSContent[i].main_bar_grade.Trim();
        //            if (lBarCTArr.Length > 0 && lBarDiaArr.Length > 0)
        //            {
        //                lVar = lBarCTArr[0].Trim() + lBarType + lBarDiaArr[0].Trim();
        //            }
        //            if (lBarCTArr.Length > 1 && lBarDiaArr.Length > 1)
        //            {
        //                lVar = lVar + "<br/>" + lBarCTArr[1].Trim() + lBarType + lBarDiaArr[1].Trim();
        //            }

        //            lEmailContent = lEmailContent + "<td align='center'><font color='blue'>" + lVar + "</font></td>";
        //            lEmailContent = lEmailContent + "<td align='center'><font color='blue'>" + lBBSContent[i].main_bar_shape + "</font></td>";
        //            lEmailContent = lEmailContent + "<td align='center'><font color='blue'>" + lBBSContent[i].cage_length.ToString() + "</font></td>";
        //            lEmailContent = lEmailContent + "<td align='center'><font color='blue'>" + lBBSContent[i].lap_length.ToString() + "</font></td>";

        //            lVar = "";
        //            var lSLType = "";
        //            if (lBBSContent[i].spiral_link_type.Length >= 5)
        //            {
        //                if (lBBSContent[i].spiral_link_type.Substring(0, 5) == "Others")
        //                {
        //                    lSLType = "";
        //                }
        //                else if (lBBSContent[i].spiral_link_type.Substring(0, 4) == "Twin")
        //                {
        //                    lSLType = "2";
        //                }
        //            }
        //            var lSLSpacing = lBBSContent[i].spiral_link_spacing.Split(',');
        //            var lSLGrade = lBBSContent[i].spiral_link_grade.Trim();
        //            if (lSLSpacing.Length > 0 && lSLSpacing.Length > 0)
        //            {
        //                lVar = lSLType + lSLGrade + lBBSContent[i].sl1_dia + "-" + lSLSpacing[0] + "-" + lBBSContent[i].sl1_length;
        //            }
        //            if (lSLSpacing.Length > 1 && lSLSpacing.Length > 1)
        //            {
        //                if (lBBSContent[i].spiral_link_type == "Twin-Single")
        //                {
        //                    lVar = lVar + "<br/>" + "" + lSLGrade + lBBSContent[i].sl2_dia + "-" + lSLSpacing[1] + "-" + lBBSContent[i].sl2_length;
        //                }
        //                else
        //                {
        //                    lVar = lVar + "<br/>" + lSLType + lSLGrade + lBBSContent[i].sl2_dia + "-" + lSLSpacing[1] + "-" + lBBSContent[i].sl2_length;
        //                }
        //            }
        //            if (lSLSpacing.Length > 2 && lSLSpacing.Length > 2)
        //            {
        //                lVar = lVar + "<br/>" + lSLType + lSLGrade + lBBSContent[i].sl3_dia + "-" + lSLSpacing[2] + "-" + lBBSContent[i].sl3_length;
        //            }

        //            lEmailContent = lEmailContent + "<td align='center'><font color='blue'>" + lVar + "</font></td>";
        //            lEmailContent = lEmailContent + "<td align='right'><font color='blue'>" + lBBSContent[i].end_length.ToString() + "</font></td>";
        //            lEmailContent = lEmailContent + "<td align='right'><font color='blue'>" + lBBSContent[i].cage_qty.ToString() + "</font></td>";
        //            lEmailContent = lEmailContent + "<td align='right'><font color='blue'>" + lBBSContent[i].cage_location + "</font></td>";
        //            lEmailContent = lEmailContent + "<td align='right'><font color='blue'>" + lBBSContent[i].cage_weight.ToString("F3") + "</font></td>";
        //            lEmailContent = lEmailContent + "<td align='right'><font color='blue'>" + lBBSContent[i].per_set.ToString() + "</font></td>";
        //            lEmailContent = lEmailContent + "<td align='right'><font color='blue'>" + lBBSContent[i].bbs_no + "</font></td>";
        //            lEmailContent = lEmailContent + "<td align='right'><font color='blue'>" + lBBSContent[i].cage_remarks + "</font></td>";
        //            lEmailContent = lEmailContent + "</tr>";
        //        }
        //    }
        //    lEmailContent = lEmailContent + "</table>";

        //    lEmailContent = lEmailContent + "<table border='1' width=100%><tr>";

        //    lEmailContent = lEmailContent + "<td width='20%'>" + "Site Contact (联系人)" + "</td>";
        //    lEmailContent = lEmailContent + "<td width='15%'>" + JobContent.SiteEngr_Name.Trim() + "</td>";
        //    lEmailContent = lEmailContent + "<td width='13%'>" + "Handphone (手机号码)" + " </td>";
        //    lEmailContent = lEmailContent + "<td width='16%'>" + JobContent.SiteEngr_HP.Trim() + "</td>";
        //    lEmailContent = lEmailContent + "<td width='13%'>" + "Email (电邮地址)" + "</td>";
        //    lEmailContent = lEmailContent + "<td>" + JobContent.SiteEngr_Tel.Trim() + "</td></tr>";

        //    lEmailContent = lEmailContent + "<tr><td width='20%'>" + "Goods Receiver (收货人)" + "</td>";
        //    lEmailContent = lEmailContent + "<td width='15%'>" + JobContent.Scheduler_Name.Trim() + "</td>";
        //    lEmailContent = lEmailContent + "<td width='13%'>" + "Handphone (手机号码)" + " </td>";
        //    lEmailContent = lEmailContent + "<td width='16%'>" + JobContent.Scheduler_HP.Trim() + "</td>";
        //    lEmailContent = lEmailContent + "<td width='13%'>" + "Email (电邮地址)" + "</td>";
        //    lEmailContent = lEmailContent + "<td>" + JobContent.Scheduler_Tel.Trim() + "</td></tr></table>";

        //    lEmailContent = lEmailContent + "<table border='1' width=100%><tr>";

        //    lEmailContent = lEmailContent + "<td colspan='3'>" + "NatSteel Contacts (大众钢铁联系人) (Fax:62619133/62665153)" + "</td></tr>";

        //    lEmailContent = lEmailContent + "<tr><td width='20%'>" + "Name (姓名)" + "</td>";
        //    lEmailContent = lEmailContent + "<td width='15%'>" + "Contact Numbers (联系电话)" + "</td>";
        //    lEmailContent = lEmailContent + "<td width='13%'>" + "Email Address (电邮地址)" + " </td></tr>";


        //    var lProjContent = db.Project.Find(CustomerCode, ProjectCode);

        //    if (lProjContent.Contact1 != null)
        //    {
        //        if (lProjContent.Contact1.Trim().Length > 0)
        //        {
        //            lEmailContent = lEmailContent + "<tr><td>" + lProjContent.Contact1.Trim() + "</td>";
        //            lVar1 = "";
        //            if (lProjContent.Tel1 != null) if (lProjContent.Tel1.Trim().Length > 0) lVar1 = lProjContent.Tel1.Trim();
        //            lEmailContent = lEmailContent + "<td>" + lVar1 + "</td>";

        //            lVar1 = "";
        //            if (lProjContent.Email1 != null) if (lProjContent.Email1.Trim().Length > 0) lVar1 = lProjContent.Email1.Trim();
        //            lEmailContent = lEmailContent + "<td>" + lVar1 + " </td></tr>";
        //            if (lVar1 != "") if (lEmailTo == "") lEmailTo = lVar1; else lEmailTo = lEmailTo + ";" + lVar1;
        //        }
        //    }

        //    if (lProjContent.Contact2 != null)
        //    {
        //        if (lProjContent.Contact2.Trim().Length > 0)
        //        {
        //            lEmailContent = lEmailContent + "<tr><td>" + lProjContent.Contact2.Trim() + "</td>";
        //            lVar1 = "";
        //            if (lProjContent.Tel2 != null) if (lProjContent.Tel2.Trim().Length > 0) lVar1 = lProjContent.Tel2.Trim();
        //            lEmailContent = lEmailContent + "<td>" + lVar1 + "</td>";

        //            lVar1 = "";
        //            if (lProjContent.Email2 != null) if (lProjContent.Email2.Trim().Length > 0) lVar1 = lProjContent.Email2.Trim();
        //            lEmailContent = lEmailContent + "<td>" + lVar1 + " </td></tr>";
        //            if (lVar1 != "") if (lEmailTo == "") lEmailTo = lVar1; else lEmailTo = lEmailTo + ";" + lVar1;
        //        }
        //    }

        //    if (lProjContent.Contact3 != null)
        //    {
        //        if (lProjContent.Contact3.Trim().Length > 0)
        //        {
        //            lEmailContent = lEmailContent + "<tr><td>" + lProjContent.Contact3.Trim() + "</td>";
        //            lVar1 = "";
        //            if (lProjContent.Tel3 != null) if (lProjContent.Tel3.Trim().Length > 0) lVar1 = lProjContent.Tel3.Trim();
        //            lEmailContent = lEmailContent + "<td>" + lVar1 + "</td>";

        //            lVar1 = "";
        //            if (lProjContent.Email3 != null) if (lProjContent.Email3.Trim().Length > 0) lVar1 = lProjContent.Email3.Trim();
        //            lEmailContent = lEmailContent + "<td>" + lVar1 + " </td></tr>";
        //            if (lVar1 != "") if (lEmailTo == "") lEmailTo = lVar1; else lEmailTo = lEmailTo + ";" + lVar1;
        //        }
        //    }

        //    if (lProjContent.Contact4 != null)
        //    {
        //        if (lProjContent.Contact4.Trim().Length > 0)
        //        {
        //            lEmailContent = lEmailContent + "<tr><td>" + lProjContent.Contact4.Trim() + "</td>";
        //            lVar1 = "";
        //            if (lProjContent.Tel4 != null) if (lProjContent.Tel4.Trim().Length > 0) lVar1 = lProjContent.Tel4.Trim();
        //            lEmailContent = lEmailContent + "<td>" + lVar1 + "</td>";

        //            lVar1 = "";
        //            if (lProjContent.Email4 != null) if (lProjContent.Email4.Trim().Length > 0) lVar1 = lProjContent.Email4.Trim();
        //            lEmailContent = lEmailContent + "<td>" + lVar1 + " </td></tr>";
        //            if (lVar1 != "") if (lEmailTo == "") lEmailTo = lVar1; else lEmailTo = lEmailTo + ";" + lVar1;
        //        }
        //    }

        //    if (lProjContent.Contact5 != null)
        //    {
        //        if (lProjContent.Contact5.Trim().Length > 0)
        //        {
        //            lEmailContent = lEmailContent + "<tr><td>" + lProjContent.Contact5.Trim() + "</td>";
        //            lVar1 = "";
        //            if (lProjContent.Tel5 != null) if (lProjContent.Tel5.Trim().Length > 0) lVar1 = lProjContent.Tel5.Trim();
        //            lEmailContent = lEmailContent + "<td>" + lVar1 + "</td>";

        //            lVar1 = "";
        //            if (lProjContent.Email5 != null) if (lProjContent.Email5.Trim().Length > 0) lVar1 = lProjContent.Email5.Trim();
        //            lEmailContent = lEmailContent + "<td>" + lVar1 + " </td></tr>";
        //            if (lVar1 != "") if (lEmailTo == "") lEmailTo = lVar1; else lEmailTo = lEmailTo + ";" + lVar1;
        //        }
        //    }

        //    if (lProjContent.Contact6 != null)
        //    {
        //        if (lProjContent.Contact6.Trim().Length > 0)
        //        {
        //            lEmailContent = lEmailContent + "<tr><td>" + lProjContent.Contact6.Trim() + "</td>";
        //            lVar1 = "";
        //            if (lProjContent.Tel6 != null) if (lProjContent.Tel6.Trim().Length > 0) lVar1 = lProjContent.Tel6.Trim();
        //            lEmailContent = lEmailContent + "<td>" + lVar1 + "</td>";

        //            lVar1 = "";
        //            if (lProjContent.Email6 != null) if (lProjContent.Email6.Trim().Length > 0) lVar1 = lProjContent.Email6.Trim();
        //            lEmailContent = lEmailContent + "<td>" + lVar1 + " </td></tr>";
        //            if (lVar1 != "") if (lEmailTo == "") lEmailTo = lVar1; else lEmailTo = lEmailTo + ";" + lVar1;
        //        }
        //    }
        //    lEmailContent = lEmailContent + "</table>";

        //    // Scheduler email
        //    if (JobContent != null)
        //    {
        //        lVar1 = JobContent.Scheduler_Tel.Trim();
        //        if (lVar1.Length > 0 && lEmailCc.IndexOf(lVar1) < 0 && lEmailTo.IndexOf(lVar1) < 0)
        //        {
        //            if (lEmailTo == "")
        //            {
        //                lEmailTo = lVar1;
        //            }
        //            else
        //            {
        //                lEmailTo = lEmailTo + ";" + lVar1;
        //            }
        //        }

        //        lVar1 = JobContent.SiteEngr_Tel.Trim();
        //        if (lVar1.Length > 0 && lEmailCc.IndexOf(lVar1) < 0 && lEmailTo.IndexOf(lVar1) < 0)
        //        {
        //            if (lEmailTo == "")
        //            {
        //                lEmailTo = lVar1;
        //            }
        //            else
        //            {
        //                lEmailTo = lEmailTo + ";" + lVar1;
        //            }
        //        }

        //        if (JobContent.UpdateBy != null)
        //        {
        //            lVar1 = JobContent.UpdateBy.Trim();
        //            if (lVar1 != "" && lEmailCc.IndexOf(lVar1) < 0)
        //            {
        //                if (lEmailCc == "") { lEmailCc = lVar1; }
        //                else { lEmailCc = lEmailCc + ";" + lVar1; }
        //            }
        //        }
        //    }

        //    lVar = "";
        //    if (lCustomer != null) lVar = lCustomer.CustomerName.Trim();

        //    lEmailSubject = lVar + " - " + JobContent.PONumber.Trim() + " - Bored Pile Cage No. " + JobID.ToString();

        //    var lOESEmail = new SendGridEmail();

        //    string lEmailFromAddress = "eprompt@natsteel.com.sg";
        //    string lEmailFromName = "Digital Ordering Email Services";

        //    //dynamic response = lOESEmail.Execute(lEmailFromAddress, lEmailFromName, lEmailTo, lEmailCc, lEmailSubject, "text/html", lEmailContent);
        //    lOESEmail.Execute(lEmailFromAddress, lEmailFromName, lEmailTo, lEmailCc, lEmailSubject, "text/html", lEmailContent).Wait();
        //    lOESEmail = null;

        //    //lock the library

        //    var lOrderDet = (from p in db.BPCDetails
        //                     from s in db.BPCJobAdvice
        //                     where p.CustomerCode == CustomerCode &&
        //                       p.ProjectCode == ProjectCode &&
        //                       p.Template == false &&
        //                       p.JobID == JobID &&
        //                       p.copyfrom_project == ProjectCode &&
        //                       p.copyfrom_template == true &&
        //                       s.CustomerCode == CustomerCode &&
        //                       s.ProjectCode == p.copyfrom_project &&
        //                       s.JobID == p.copyfrom_jobid &&
        //                       s.Template == true &&
        //                       (s.OrderStatus == "New" ||
        //                       s.OrderStatus == "Created")
        //                     select s).ToList();
        //    if (lOrderDet != null && lOrderDet.Count > 0)
        //    {
        //        for (int i = 0; i < lOrderDet.Count; i++)
        //        {
        //            int lOldJobID = lOrderDet[i].JobID;
        //            var lOldRec = db.BPCJobAdvice.Find(CustomerCode, ProjectCode, true, lOldJobID);
        //            if (lOldRec != null && lOldRec.CustomerCode != null)
        //            {
        //                var lNewRec = lOldRec;
        //                lNewRec.OrderStatus = "OnHold";
        //                db.Entry(lOldRec).CurrentValues.SetValues(lNewRec);
        //            }
        //        }
        //        db.SaveChanges();
        //    }

        //    var lOrderCommon = (from p in db.BPCDetails
        //                        from s in db.BPCJobAdvice
        //                        where p.CustomerCode == CustomerCode &&
        //                          p.ProjectCode == ProjectCode &&
        //                          p.Template == false &&
        //                          p.JobID == JobID &&
        //                          p.copyfrom_project == "0000000000" &&
        //                          s.CustomerCode == "0000000000" &&
        //                          s.ProjectCode == p.copyfrom_project &&
        //                          s.JobID == p.copyfrom_jobid &&
        //                          (s.OrderStatus == "New" ||
        //                          s.OrderStatus == "Created")
        //                        select s).ToList();
        //    if (lOrderCommon != null && lOrderCommon.Count > 0)
        //    {
        //        for (int i = 0; i < lOrderCommon.Count; i++)
        //        {
        //            int lOldJobID = lOrderCommon[i].JobID;
        //            var lOldRec = db.BPCJobAdvice.Find("0000000000", "0000000000", false, lOldJobID);
        //            if (lOldRec == null || lOldRec.CustomerCode != null || lOldRec.CustomerCode != "0000000000")
        //            {
        //                lOldRec = db.BPCJobAdvice.Find("0000000000", "0000000000", true, lOldJobID);
        //            }

        //            if (lOldRec != null && lOldRec.CustomerCode != null && lOldRec.CustomerCode == "0000000000")
        //            {
        //                var lNewRec = lOldRec;
        //                lNewRec.OrderStatus = "OnHold";
        //                db.Entry(lOldRec).CurrentValues.SetValues(lNewRec);
        //            }
        //        }
        //        db.SaveChanges();
        //    }

        //    return Json(true);
        //}

        //[HttpGet]
        //[Route("/getProject_bpc/{CustomerCode}/{UserName}")]
        ////[ValidateAntiForgeryHeader]
        //public ActionResult getProject(string CustomerCode, string UserName)
        //{
        //    string lUserType = "";
        //    string lGroupName = "";

        //    if (gUserType != null && gUserType != "" && gGroupName != null && gGroupName != "")
        //    {
        //        lUserType = gUserType;
        //        lGroupName = gGroupName;
        //    }
        //    else
        //    {
        //        UserAccessController lUa = new UserAccessController();
        //        lUserType = lUa.getUserType(UserName);
        //        lGroupName = lUa.getGroupName(UserName);
        //        gUserType = lUserType;
        //        gGroupName = lGroupName;
        //        lUa = null;
        //    }

        //    List<ProjectListModels> content = new List<ProjectListModels> {
        //        new ProjectListModels
        //        {
        //            CustomerCode = "",
        //            ProjectCode = "",
        //            ProjectTitle = ""
        //        } };

        //    if (lUserType == "PL" || lUserType == "AD" || lUserType == "PM" || lUserType == "PA" || lUserType == "P1" || lUserType == "P2" || lUserType == "PU" || lUserType == "TE")
        //    {
        //        //content = (from p in db.ProjectList
        //        //           where p.CustomerCode == CustomerCode &&
        //        //           (from m in db.Project
        //        //            where m.CustomerCode == CustomerCode
        //        //            select m.ProjectCode).Contains(p.ProjectCode)
        //        //           orderby p.ProjectTitle
        //        //           select p).ToList();

        //        content = new List<ProjectListModels>();

        //        //var lDa = new OracleDataAdapter();
        //        //var lCmd = new OracleCommand();
        //        //var lDs = new DataSet();
        //        //var lDStatus = new DataSet();
        //        //var lcisCon = new OracleConnection();
        //        //var lProcess = new ProcessController();

        //        //lCmd.CommandText = "SELECT (NAME1 || NAME2) AS SHIP_TO_NAME,KUNNR AS SHIP_TO_PARTY FROM SAPSR3.KNA1 " +
        //        //"WHERE KTOKD  = 'Y001' AND MANDT ='" + lProcess.strClient + "' " +
        //        //"AND KUNNR IN (SELECT KUNNR FROM SAPSR3.VBPA WHERE MANDT='" + lProcess.strClient + "' " +
        //        //"AND VBELN IN (SELECT VBELN FROM SAPSR3.VBAK WHERE MANDT ='" + lProcess.strClient + "' " +
        //        //"AND VBELN like '102%' " +
        //        //"AND ytot_bpc > 0 " +
        //        //"AND KUNNR ='" + CustomerCode + "' AND TRVOG ='4' " +
        //        //"AND to_date(GUEEN, 'yyyymmdd') >=  (SYSDATE - 180) )) " +
        //        //"ORDER BY 1 ";

        //        //if (lProcess.OpenCISConnection(ref lcisCon) == true)
        //        //{
        //        //    lCmd.Connection = lcisCon;
        //        //    lDa.SelectCommand = lCmd;
        //        //    lDs.Clear();
        //        //    lDa.Fill(lDs);
        //        //    if (lDs.Tables[0].Rows.Count > 0)
        //        //        for (int i = 0; i < lDs.Tables[0].Rows.Count; i++)
        //        //        {
        //        //            string lName = ((string)lDs.Tables[0].Rows[i].ItemArray[0]).Trim();
        //        //            string lCode = ((string)lDs.Tables[0].Rows[i].ItemArray[1]).Trim();
        //        //            content.Add(new ProjectListModels
        //        //            {
        //        //                CustomerCode = CustomerCode,
        //        //                ProjectCode = lCode,
        //        //                ProjectTitle = lName
        //        //            });
        //        //        }
        //        //    lProcess.CloseCISConnection(ref lcisCon);
        //        //}
        //        //lDa = null;
        //        //lCmd = null;
        //        //lDs = null;
        //        //lDStatus = null;
        //        //lcisCon = null;
        //        //lProcess = null;
        //        content = new List<ProjectListModels>();

        //        if (lUserType == "AD" || lUserType == "PM" || lUserType == "PA" || lUserType == "P1" || lUserType == "P2" || lUserType == "PU" || lUserType == "TE")
        //        {
        //            #region Get from SAP
        //            var lDa = new OracleDataAdapter();
        //            var lOCmd = new OracleCommand();
        //            var lDs = new DataSet();
        //            var lDStatus = new DataSet();
        //            var lOcisCon = new OracleConnection();
        //            var lProcess = new ProcessController();

        //            lOCmd.CommandText = "SELECT (NAME1 || NAME2) AS SHIP_TO_NAME,KUNNR AS SHIP_TO_PARTY FROM SAPSR3.KNA1 " +
        //            "WHERE KTOKD = 'Y001' AND MANDT ='" + lProcess.strClient + "' " +
        //            "AND KUNNR IN (SELECT KUNNR FROM SAPSR3.VBPA WHERE MANDT='" + lProcess.strClient + "' " +
        //            "AND VBELN IN (SELECT VBELN FROM SAPSR3.VBAK WHERE MANDT ='" + lProcess.strClient + "' " +
        //            "AND (VBELN like '102%' OR VBELN like '_102%') " +
        //            "AND ytot_bpc > 0 " +
        //            "AND KUNNR ='" + CustomerCode + "' AND TRVOG ='4' " +
        //            "AND to_date(GUEEN, 'yyyymmdd') >=  (SYSDATE - 180) )) " +
        //            "ORDER BY 1 ";

        //            if (lProcess.OpenCISConnection(ref lOcisCon) == true)
        //            {
        //                lOCmd.Connection = lOcisCon;
        //                lDa.SelectCommand = lOCmd;
        //                lDs.Clear();
        //                lDa.Fill(lDs);
        //                if (lDs.Tables[0].Rows.Count > 0)
        //                    for (int i = 0; i < lDs.Tables[0].Rows.Count; i++)
        //                    {
        //                        string lName = ((string)lDs.Tables[0].Rows[i].ItemArray[0]).Trim() + "(" + ((string)lDs.Tables[0].Rows[i].ItemArray[1]).Trim() + ")";
        //                        string lCode = ((string)lDs.Tables[0].Rows[i].ItemArray[1]).Trim();
        //                        content.Add(new ProjectListModels
        //                        {
        //                            CustomerCode = CustomerCode,
        //                            ProjectCode = lCode,
        //                            ProjectTitle = lName
        //                        });
        //                    }
        //                lProcess.CloseCISConnection(ref lOcisCon);
        //            }
        //            lDa = null;
        //            lOCmd = null;
        //            lDs = null;
        //            lDStatus = null;
        //            lOcisCon = null;
        //            lProcess = null;
        //            #endregion
        //        }
        //        else
        //        {
        //            #region Get from NDS
        //            var lDa = new SqlDataAdapter();
        //            var lOCmd = new SqlCommand();
        //            var lDs = new DataSet();
        //            var lDStatus = new DataSet();
        //            var lOcisCon = new SqlConnection();
        //            var lProcess = new ProcessController();

        //            lOCmd.CommandText = "SELECT P.vchProjectCode, P.vchProjectName " +
        //            "FROM dbo.SAPProjectMaster P, " +
        //            //    "dbo.ContractMaster C, " +
        //            "dbo.CustomerMaster A, " +
        //            "dbo.ContractProductMapping M " +
        //            //"WHERE P.intContractID = C.intContractID " +
        //            //"AND A.intCustomerCode = C.intCustomerCode " +
        //            //"AND C.vchContractNumber = M.VBELN " +
        //            "WHERE M.mandt = '" + lProcess.strClient + "' " +
        //            "AND ytot_bpc > 0 " +
        //            "AND datEndDate > DATEADD(dd, -180, getDate()) " +
        //            "AND vchCustomerNo = '" + CustomerCode + "' " +
        //            "GROUP BY P.vchProjectCode, P.vchProjectName " +
        //            "ORDER BY P.vchProjectName ";

        //            if (lProcess.OpenNDSConnection(ref lOcisCon) == true)
        //            {
        //                lOCmd.Connection = lOcisCon;
        //                lDa.SelectCommand = lOCmd;
        //                lDs.Clear();
        //                lDa.Fill(lDs);
        //                if (lDs.Tables[0].Rows.Count > 0)
        //                    for (int i = 0; i < lDs.Tables[0].Rows.Count; i++)
        //                    {
        //                        string lCode = ((string)lDs.Tables[0].Rows[i].ItemArray[0]).Trim();
        //                        string lName = ((string)lDs.Tables[0].Rows[i].ItemArray[1]).Trim() + "(" + ((string)lDs.Tables[0].Rows[i].ItemArray[0]).Trim() + ")";
        //                        content.Add(new ProjectListModels
        //                        {
        //                            CustomerCode = CustomerCode,
        //                            ProjectCode = lCode,
        //                            ProjectTitle = lName
        //                        });
        //                    }
        //                lProcess.CloseNDSConnection(ref lOcisCon);
        //            }
        //            lDa = null;
        //            lOCmd = null;
        //            lDs = null;
        //            lDStatus = null;
        //            lOcisCon = null;
        //            lProcess = null;
        //            #endregion
        //        }

        //    }
        //    else
        //    {
        //        content = (from p in db.ProjectList
        //                   where p.CustomerCode == CustomerCode &&
        //                   (from u in db.UserAccess
        //                    where u.UserName == lGroupName &&
        //                    u.CustomerCode == CustomerCode
        //                    select u.ProjectCode).Contains(p.ProjectCode)
        //                   orderby p.ProjectTitle
        //                   select p).ToList();
        //        if (content.Count() > 0)
        //        {
        //            content = RemoveNonBPCProject(content);
        //        }

        //    }


        //    if (CustomerCode == "0000000000" && (lUserType == "TE" || lUserType == "AD"))
        //    {
        //        var lStandardLib = new ProjectListModels();
        //        lStandardLib.CustomerCode = "0000000000";
        //        lStandardLib.ProjectCode = "0000000000";
        //        lStandardLib.ProjectTitle = "COMMON BORED PILE CAGE";

        //        content.Insert(0, lStandardLib);
        //    }

        //    if (content.Count() == 0)
        //    {
        //        content = new List<ProjectListModels> {
        //                new ProjectListModels
        //                {
        //                    CustomerCode = "",
        //                    ProjectCode = "",
        //                    ProjectTitle = ""
        //                }
        //        };
        //    }

        //    return Ok(content);
        //}

        //[HttpPost]
        ////[ValidateAntiForgeryHeader]
        //public ActionResult getCustomer()
        //{
        //    string lCustomerCode = "";
        //    UserAccessController lUa = new UserAccessController();
        //    var lUserType = lUa.getUserType("Vishalw_ttl@natsteel.com.sg");
        //    var lGroupName = lUa.getGroupName("Vishalw_ttl@natsteel.com.sg");

        //    ViewBag.UserType = lUserType;

        //    string lUserName = "Vishalw_ttl@natsteel.com.sg";
        //    //if (lUserName.IndexOf("@") > 0)
        //    //{
        //    //    lUserName = lUserName.Substring(0, lUserName.IndexOf("@"));
        //    //}
        //    ViewBag.UserName = lUserName;

        //    lUa = null;

        //    SharedAPIController lBackEnd = new SharedAPIController();

        //    var lCustomer = lBackEnd.getCustomer(lUserType, lGroupName);

        //    lBackEnd = null;

        //    return Json(lCustomer, JsonRequestBehavior.AllowGet);
        //}

        //[HttpPost]
        //[Route("/RemoveNonBPCProject_Oracle_bpc")]
        //List<ProjectListModels> RemoveNonBPCProject_Oracle(List<ProjectListModels> pInputProj)
        //{
        //    OracleDataReader lRst;
        //    var lCmd = new OracleCommand();
        //    var lcisCon = new OracleConnection();

        //    if (pInputProj.Count() > 0)
        //    {
        //        var lProcessObj = new ProcessController();
        //        if (lProcessObj.OpenCISConnection(ref lcisCon) == true)
        //        {
        //            for (int i = pInputProj.Count() - 1; i >= 0; i--)
        //            {
        //                lCmd.CommandText = "SELECT K.VBELN AS CONTRACT_NO , K.GUEBG AS VALID_FROM, " +
        //                "K.GUEEN AS VALID_TO " +
        //                "/*+ index(idx_kunnr) */ " +
        //                "FROM SAPSR3.VBAK K, SAPSR3.VBPA P " +
        //                "WHERE K.MANDT = '" + lProcessObj.strClient + "' " +
        //                "AND P.MANDT = '" + lProcessObj.strClient + "' " +
        //                "AND K.VKORG = '" + lProcessObj.strSalesOrg + "' " +
        //                "AND K.KUNNR = '" + pInputProj[i].CustomerCode + "' " +
        //                "AND K.TRVOG = '4' " +
        //                "AND K.ytot_bpc > 0 " +
        //                "AND k.VBELN = P.VBELN " +
        //                "AND P.KUNNR = '" + pInputProj[i].ProjectCode + "' ";

        //                lCmd.Connection = lcisCon;
        //                lCmd.CommandTimeout = 1200;
        //                lRst = lCmd.ExecuteReader();
        //                if (!lRst.HasRows)
        //                {
        //                    pInputProj.RemoveAt(i);
        //                }
        //                lRst.Close();
        //            }
        //        }

        //        lProcessObj.CloseCISConnection(ref lcisCon);
        //        lCmd = null;
        //        lRst = null;
        //        lProcessObj = null;
        //    }

        //    return pInputProj;
        //}

        [HttpPost]
        [Route("/RemoveNonBPCProject_bpc")]
        public List<ProjectListModels> RemoveNonBPCProject(List<ProjectListModels> pInputProj)
        {
            var lCmd = new SqlCommand();
            SqlDataReader lRst;
            var lNDSCon = new SqlConnection();

            if (pInputProj.Count() > 0)
            {
                var lProcessObj = new ProcessController();
                if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
                {
                    for (int i = pInputProj.Count() - 1; i >= 0; i--)
                    {
                        lCmd.CommandText =
                        "SELECT isNull(ProjectMESH,0) " +
                        "from dbo.OESProject " +
                        "WHERE CustomerCode = '" + pInputProj[i].CustomerCode + "' " +
                        "AND ProjectCode = '" + pInputProj[i].ProjectCode + "' " +
                        "AND ProjectBPC = 1 ";

                        lCmd.Connection = lNDSCon;
                        lCmd.CommandTimeout = 1200;
                        lRst = lCmd.ExecuteReader();
                        if (!lRst.HasRows)
                        {
                            pInputProj.RemoveAt(i);
                        }
                        lRst.Close();
                    }
                }

                lProcessObj.CloseNDSConnection(ref lNDSCon);
                lCmd = null;
                lRst = null;
                lProcessObj = null;
            }

            return pInputProj;
        }

        [HttpGet]
        //   [ValidateAntiForgeryHeader]
        [Route("/getProjectDetails_bpc/{CustomerCode}/{ProjectCode}/{UserName}")]
        public ActionResult getProjectDetails(string CustomerCode, string ProjectCode, string UserName)
        {
            var lCmd = new SqlCommand();
            SqlDataReader lRst;
            var lNDSCon = new SqlConnection();

            string lUserType = "";

            if (gUserType != null && gUserType != "")
            {
                lUserType = gUserType;
            }
            else
            {
                UserAccessController lUa = new UserAccessController();
                lUserType = lUa.getUserType(UserName);
                gUserType = lUserType;
                lUa = null;
            }

            var content1 = new ProjectAccessModels();

            if (lUserType == "CU" || lUserType == "CA")
            {
                lCmd.CommandText = "SELECT " +
                "P.CustomerCode, " +
                "P.ProjectCode, " +
                "P.ProjectTitle, " +
                "SiteEngr_Name, " +
                "SiteEngr_HP, " +
                "SiteEngr_Tel, " +
                "Scheduler_Name, " +
                "Scheduler_HP, " +
                "Scheduler_Tel, " +
                "Contact1, " +
                "Contact2, " +
                "Contact3, " +
                "Contact4, " +
                "Contact5, " +
                "Contact6, " +
                "Tel1, " +
                "Tel2, " +
                "Tel3, " +
                "Tel4, " +
                "Tel5, " +
                "Tel6, " +
                "Email1, " +
                "Email2, " +
                "Email3, " +
                "Email4, " +
                "Email5, " +
                "Email6, " +
                "OrderSubmission, " +
                "OrderCreation, " +
                "bpc_template_editable, " +
                "bpc_change_cagedata, " +
                "bpc_order_misccages " +
                "FROM dbo.OESProject P, dbo.OESUserAccess U " +
                "WHERE P.CustomerCode = U.CustomerCode " +
                "AND P.ProjectCode = U.ProjectCode " +
                "AND U.UserName = '" + User.Identity.Name + "' " +
                "AND U.CustomerCode = '" + CustomerCode + "' " +
                "AND U.ProjectCode = '" + ProjectCode + "' ";
            }
            else if (lUserType == "PL" || lUserType == "AD" || lUserType == "PM" || lUserType == "PA" || lUserType == "P1" || lUserType == "P2" || lUserType == "PU" || lUserType == "TE")
            {
                lCmd.CommandText = "SELECT " +
                "CustomerCode, " +
                "ProjectCode, " +
                "ProjectTitle, " +
                "SiteEngr_Name, " +
                "SiteEngr_HP, " +
                "SiteEngr_Tel, " +
                "Scheduler_Name, " +
                "Scheduler_HP, " +
                "Scheduler_Tel, " +
                "Contact1, " +
                "Contact2, " +
                "Contact3, " +
                "Contact4, " +
                "Contact5, " +
                "Contact6, " +
                "Tel1, " +
                "Tel2, " +
                "Tel3, " +
                "Tel4, " +
                "Tel5, " +
                "Tel6, " +
                "Email1, " +
                "Email2, " +
                "Email3, " +
                "Email4, " +
                "Email5, " +
                "Email6, " +
                "'Yes' as OrderSubmission, " +
                "'Yes' as OrderCreation, " +
                "CAST(1 as bit) as bpc_template_editable, " +
                "CAST(1 as bit) as bpc_change_cagedata, " +
                "CAST(1 as bit) as bpc_order_misccages " +
                "FROM dbo.OESProject P " +
                "WHERE P.CustomerCode = '" + CustomerCode + "' " +
                "AND P.ProjectCode = '" + ProjectCode + "' ";
            }
            else
            {
                lCmd.CommandText = "SELECT " +
                "CustomerCode, " +
                "ProjectCode, " +
                "ProjectTitle, " +
                "SiteEngr_Name, " +
                "SiteEngr_HP, " +
                "SiteEngr_Tel, " +
                "Scheduler_Name, " +
                "Scheduler_HP, " +
                "Scheduler_Tel, " +
                "Contact1, " +
                "Contact2, " +
                "Contact3, " +
                "Contact4, " +
                "Contact5, " +
                "Contact6, " +
                "Tel1, " +
                "Tel2, " +
                "Tel3, " +
                "Tel4, " +
                "Tel5, " +
                "Tel6, " +
                "Email1, " +
                "Email2, " +
                "Email3, " +
                "Email4, " +
                "Email5, " +
                "Email6, " +
                "'No' as OrderSubmission, " +
                "'No' as OrderCreation, " +
                "CAST(0 as bit) as bpc_template_editable, " +
                "CAST(0 as bit) as bpc_change_cagedata, " +
                "CAST(0 as bit) as bpc_order_misccages " +
                "FROM dbo.OESProject P " +
                "WHERE P.CustomerCode = '" + CustomerCode + "' " +
                "AND P.ProjectCode = '" + ProjectCode + "' ";
            }

            var lProcessObj = new ProcessController();
            if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
            {
            L1:

                lCmd.Connection = lNDSCon;
                lCmd.CommandTimeout = 300;
                lRst = lCmd.ExecuteReader();
                if (lRst.HasRows)
                {
                    if (lRst.Read())
                    {
                        content1 = new ProjectAccessModels
                        {
                            CustomerCode = lRst.GetValue(0) == DBNull.Value ? "" : lRst.GetString(0).Trim(),
                            ProjectCode = lRst.GetValue(1) == DBNull.Value ? "" : lRst.GetString(1).Trim(),
                            ProjectTitle = lRst.GetValue(2) == DBNull.Value ? "" : lRst.GetString(2).Trim(),
                            SiteEngr_Name = lRst.GetValue(3) == DBNull.Value ? "" : lRst.GetString(3).Trim(),
                            SiteEngr_HP = lRst.GetValue(4) == DBNull.Value ? "" : lRst.GetString(4).Trim(),
                            SiteEngr_Tel = lRst.GetValue(5) == DBNull.Value ? "" : lRst.GetString(5).Trim(),
                            Scheduler_Name = lRst.GetValue(6) == DBNull.Value ? "" : lRst.GetString(6).Trim(),
                            Scheduler_HP = lRst.GetValue(7) == DBNull.Value ? "" : lRst.GetString(7).Trim(),
                            Scheduler_Tel = lRst.GetValue(8) == DBNull.Value ? "" : lRst.GetString(8).Trim(),
                            Contact1 = lRst.GetValue(9) == DBNull.Value ? "" : lRst.GetString(9).Trim(),
                            Contact2 = lRst.GetValue(10) == DBNull.Value ? "" : lRst.GetString(10).Trim(),
                            Contact3 = lRst.GetValue(11) == DBNull.Value ? "" : lRst.GetString(11).Trim(),
                            Contact4 = lRst.GetValue(12) == DBNull.Value ? "" : lRst.GetString(12).Trim(),
                            Contact5 = lRst.GetValue(13) == DBNull.Value ? "" : lRst.GetString(13).Trim(),
                            Contact6 = lRst.GetValue(14) == DBNull.Value ? "" : lRst.GetString(14).Trim(),
                            Tel1 = lRst.GetValue(15) == DBNull.Value ? "" : lRst.GetString(15).Trim(),
                            Tel2 = lRst.GetValue(16) == DBNull.Value ? "" : lRst.GetString(16).Trim(),
                            Tel3 = lRst.GetValue(17) == DBNull.Value ? "" : lRst.GetString(17).Trim(),
                            Tel4 = lRst.GetValue(18) == DBNull.Value ? "" : lRst.GetString(18).Trim(),
                            Tel5 = lRst.GetValue(19) == DBNull.Value ? "" : lRst.GetString(19).Trim(),
                            Tel6 = lRst.GetValue(20) == DBNull.Value ? "" : lRst.GetString(20).Trim(),
                            Email1 = lRst.GetValue(21) == DBNull.Value ? "" : lRst.GetString(21).Trim(),
                            Email2 = lRst.GetValue(22) == DBNull.Value ? "" : lRst.GetString(22).Trim(),
                            Email3 = lRst.GetValue(23) == DBNull.Value ? "" : lRst.GetString(23).Trim(),
                            Email4 = lRst.GetValue(24) == DBNull.Value ? "" : lRst.GetString(24).Trim(),
                            Email5 = lRst.GetValue(25) == DBNull.Value ? "" : lRst.GetString(25).Trim(),
                            Email6 = lRst.GetValue(26) == DBNull.Value ? "" : lRst.GetString(26).Trim(),
                            OrderSubmission = lRst.GetValue(27) == DBNull.Value ? "No" : lRst.GetString(27).Trim(),
                            OrderCreation = lRst.GetValue(28) == DBNull.Value ? "No" : lRst.GetString(28).Trim(),
                            bpc_template_editable = lRst.GetValue(29) == DBNull.Value ? false : lRst.GetBoolean(29),
                            bpc_change_cagedata = lRst.GetValue(30) == DBNull.Value ? false : lRst.GetBoolean(30),
                            bpc_order_misccages = lRst.GetValue(31) == DBNull.Value ? false : lRst.GetBoolean(31)
                        };
                    }
                }
                lRst.Close();

                if ((content1.CustomerCode == null || content1.ProjectCode == null) & CustomerCode != null && ProjectCode != null)
                {
                    if (CustomerCode.Trim().Length > 0 && ProjectCode.Trim().Length > 0)
                    {
                        var lProjectObj = new OrderDetailsController();
                        lProjectObj.createProject(CustomerCode, ProjectCode);
                        lProjectObj = null;
                        goto L1;
                    }
                }

                lCmd.CommandText =
                "SELECT isNull(MAX(JobID), 0) FROM dbo.OESBPCJobAdvice " +
                "WHERE CustomerCode = '" + CustomerCode + "' " +
                "AND ProjectCode = '" + ProjectCode + "' " +
                "AND Template = 0 " +
                "AND OrderStatus <> 'New' " +
                "AND OrderStatus <> 'Created' " +
                "AND OrderStatus is not NULL " +
                "AND OrderStatus <> '' " +
                "AND(SiteEngr_Name > '' " +
                "OR Scheduler_Name > '') ";

                int LastJobID = 0;

                lCmd.Connection = lNDSCon;
                lCmd.CommandTimeout = 300;
                lRst = lCmd.ExecuteReader();
                if (lRst.HasRows)
                {
                    if (lRst.Read())
                    {
                        LastJobID = lRst.GetInt32(0);
                    }
                }
                lRst.Close();

                if (LastJobID > 0)
                {
                    if (lUserType == "CU" || lUserType == "CA")
                    {
                        lCmd.CommandText =
                        "SELECT isNull(MAX(JobID), 0) FROM dbo.OESBPCJobAdvice " +
                        "WHERE CustomerCode = '" + CustomerCode + "' " +
                        "AND ProjectCode = '" + ProjectCode + "' " +
                        "AND Template = 0 " +
                        "AND OrderStatus <> 'New' " +
                        "AND OrderStatus <> 'Created' " +
                        "AND OrderStatus is not NULL " +
                        "AND OrderStatus <> '' " +
                        "AND UpdateBy = '" + UserName + "' " +
                        "AND(SiteEngr_Name > '' " +
                        "OR Scheduler_Name > '') ";

                        int LastMyJobID = 0;

                        lCmd.Connection = lNDSCon;
                        lCmd.CommandTimeout = 300;
                        lRst = lCmd.ExecuteReader();
                        if (lRst.HasRows)
                        {
                            if (lRst.Read())
                            {
                                LastMyJobID = lRst.GetInt32(0);
                            }
                        }
                        lRst.Close();


                        lCmd.CommandText =
                        "SELECT isNull(SiteEngr_Name, ''), " +
                        "isNull(SiteEngr_HP, ''), " +
                        "isNull(SiteEngr_Tel, ''), " +
                        "isNull(Scheduler_Name, ''), " +
                        "isNull(Scheduler_HP, ''), " +
                        "isNull(Scheduler_Tel, '') " +
                        "FROM dbo.OESBPCJobAdvice " +
                        "WHERE CustomerCode = '" + CustomerCode + "' " +
                        "AND ProjectCode = '" + ProjectCode + "' " +
                        "AND Template = 0 " +
                        "AND JobID = " + LastMyJobID.ToString() + " " +
                        "AND UpdateBy = '" + UserName + "' " +
                        "UNION " +
                        "SELECT isNull(SiteEngr_Name, ''), " +
                        "isNull(SiteEngr_HP, ''), " +
                        "isNull(SiteEngr_Tel, ''), " +
                        "isNull(Scheduler_Name, ''), " +
                        "isNull(Scheduler_HP, ''), " +
                        "isNull(Scheduler_Tel, '') " +
                        "FROM dbo.OESBPCJobAdvice " +
                        "WHERE CustomerCode = '" + CustomerCode + "' " +
                        "AND ProjectCode = '" + ProjectCode + "' " +
                        "AND Template = 0 " +
                        "AND JobID = " + LastJobID.ToString() + " ";
                    }
                    else
                    {
                        lCmd.CommandText =
                        "SELECT isNull(SiteEngr_Name,''), " +
                        "isNull(SiteEngr_HP,''), " +
                        "isNull(SiteEngr_Tel,''), " +
                        "isNull(Scheduler_Name,''), " +
                        "isNull(Scheduler_HP,''), " +
                        "isNull(Scheduler_Tel,'') " +
                        "FROM dbo.OESBPCJobAdvice " +
                        "WHERE CustomerCode = '" + CustomerCode + "' " +
                        "AND ProjectCode = '" + ProjectCode + "' " +
                        "AND Template = 0 " +
                        "AND JobID = " + LastJobID.ToString() + " ";
                    }

                    lCmd.Connection = lNDSCon;
                    lCmd.CommandTimeout = 300;
                    lRst = lCmd.ExecuteReader();
                    if (lRst.HasRows)
                    {
                        if (lRst.Read())
                        {
                            content1.SiteEngr_Name = lRst.GetString(0);
                            content1.SiteEngr_HP = lRst.GetString(1);
                            content1.SiteEngr_Tel = lRst.GetString(2);
                            content1.Scheduler_Name = lRst.GetString(3);
                            content1.Scheduler_HP = lRst.GetString(4);
                            content1.Scheduler_Tel = lRst.GetString(5);
                        }
                    }
                    lRst.Close();

                }

                lProcessObj.CloseNDSConnection(ref lNDSCon);
            }


            //var lProcessObj = new ProcessController();
            //if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
            //{
            //    lCmd.Connection = lNDSCon;
            //    lCmd.CommandTimeout = 300;
            //    lRst = lCmd.ExecuteReader();
            //    if (lRst.HasRows)
            //    {
            //        if (lRst.Read())
            //        {
            //            content1 = new ProjectAccessModels
            //            {
            //                CustomerCode = lRst.GetValue(0) == DBNull.Value ? "" : lRst.GetString(0).Trim(),
            //                ProjectCode = lRst.GetValue(1) == DBNull.Value ? "" : lRst.GetString(1).Trim(),
            //                ProjectTitle = lRst.GetValue(2) == DBNull.Value ? "" : lRst.GetString(2).Trim(),
            //                SiteEngr_Name = lRst.GetValue(3) == DBNull.Value ? "" : lRst.GetString(3).Trim(),
            //                SiteEngr_HP = lRst.GetValue(4) == DBNull.Value ? "" : lRst.GetString(4).Trim(),
            //                SiteEngr_Tel = lRst.GetValue(5) == DBNull.Value ? "" : lRst.GetString(5).Trim(),
            //                Scheduler_Name = lRst.GetValue(6) == DBNull.Value ? "" : lRst.GetString(6).Trim(),
            //                Scheduler_HP = lRst.GetValue(7) == DBNull.Value ? "" : lRst.GetString(7).Trim(),
            //                Scheduler_Tel = lRst.GetValue(8) == DBNull.Value ? "" : lRst.GetString(8).Trim(),
            //                Contact1 = lRst.GetValue(9) == DBNull.Value ? "" : lRst.GetString(9).Trim(),
            //                Contact2 = lRst.GetValue(10) == DBNull.Value ? "" : lRst.GetString(10).Trim(),
            //                Contact3 = lRst.GetValue(11) == DBNull.Value ? "" : lRst.GetString(11).Trim(),
            //                Contact4 = lRst.GetValue(12) == DBNull.Value ? "" : lRst.GetString(12).Trim(),
            //                Contact5 = lRst.GetValue(13) == DBNull.Value ? "" : lRst.GetString(13).Trim(),
            //                Contact6 = lRst.GetValue(14) == DBNull.Value ? "" : lRst.GetString(14).Trim(),
            //                Tel1 = lRst.GetValue(15) == DBNull.Value ? "" : lRst.GetString(15).Trim(),
            //                Tel2 = lRst.GetValue(16) == DBNull.Value ? "" : lRst.GetString(16).Trim(),
            //                Tel3 = lRst.GetValue(17) == DBNull.Value ? "" : lRst.GetString(17).Trim(),
            //                Tel4 = lRst.GetValue(18) == DBNull.Value ? "" : lRst.GetString(18).Trim(),
            //                Tel5 = lRst.GetValue(19) == DBNull.Value ? "" : lRst.GetString(19).Trim(),
            //                Tel6 = lRst.GetValue(20) == DBNull.Value ? "" : lRst.GetString(20).Trim(),
            //                Email1 = lRst.GetValue(21) == DBNull.Value ? "" : lRst.GetString(21).Trim(),
            //                Email2 = lRst.GetValue(22) == DBNull.Value ? "" : lRst.GetString(22).Trim(),
            //                Email3 = lRst.GetValue(23) == DBNull.Value ? "" : lRst.GetString(23).Trim(),
            //                Email4 = lRst.GetValue(24) == DBNull.Value ? "" : lRst.GetString(24).Trim(),
            //                Email5 = lRst.GetValue(25) == DBNull.Value ? "" : lRst.GetString(25).Trim(),
            //                Email6 = lRst.GetValue(26) == DBNull.Value ? "" : lRst.GetString(26).Trim(),
            //                OrderSubmission = lRst.GetValue(27) == DBNull.Value ? "No" : lRst.GetString(27).Trim(),
            //                OrderCreation = lRst.GetValue(28) == DBNull.Value ? "No" : lRst.GetString(28).Trim()
            //            };
            //        }
            //    }
            //    lRst.Close();

            //    lProcessObj.CloseNDSConnection(ref lNDSCon);
            //}

            //lProcessObj = null;

            if ((lUserType == "TE" || lUserType == "AD") && CustomerCode == "0000000000" && ProjectCode == "0000000000")
            {
                content1 = new ProjectAccessModels
                {
                    CustomerCode = CustomerCode,
                    ProjectCode = ProjectCode,
                    ProjectTitle = "COMMON BORED PILE CAGE",
                    SiteEngr_Name = "",
                    SiteEngr_HP = "",
                    SiteEngr_Tel = "",
                    Scheduler_Name = "",
                    Scheduler_HP = "",
                    Scheduler_Tel = "",
                    Contact1 = "",
                    Contact2 = "",
                    Contact3 = "",
                    Contact4 = "",
                    Contact5 = "",
                    Contact6 = "",
                    Tel1 = "",
                    Tel2 = "",
                    Tel3 = "",
                    Tel4 = "",
                    Tel5 = "",
                    Tel6 = "",
                    Email1 = "",
                    Email2 = "",
                    Email3 = "",
                    Email4 = "",
                    Email5 = "",
                    Email6 = "",
                    OrderSubmission = "No",
                    OrderCreation = "Yes",
                    bpc_template_editable = true,
                    bpc_change_cagedata = true,
                    bpc_order_misccages = true
                };
            }


            //var content = db.Project.Find(CustomerCode, ProjectCode);
            //var content1 = new ProjectAccessModels();

            //if (content != null) {
            //    if (lUserType == "CU")
            //    {
            //        var lAccess = db.UserAccess.Find(User.Identity.Name, CustomerCode, ProjectCode);
            //        if (lAccess != null)
            //        {
            //            lSumission = lAccess.OrderSubmission.Trim();
            //            lEditable = lAccess.OrderCreation.Trim();
            //        }
            //    }
            //    content1 = new ProjectAccessModels
            //    {
            //        CustomerCode = content.CustomerCode,
            //        ProjectCode = content.ProjectCode,
            //        ProjectTitle = content.ProjectTitle,
            //        SiteEngr_Name = content.SiteEngr_Name,
            //        SiteEngr_HP = content.SiteEngr_HP,
            //        SiteEngr_Tel = content.SiteEngr_Tel,
            //        Scheduler_Name = content.Scheduler_Name,
            //        Scheduler_HP = content.Scheduler_HP,
            //        Scheduler_Tel = content.Scheduler_Tel,
            //        Contact1 = content.Contact1,
            //        Contact2 = content.Contact2,
            //        Contact3 = content.Contact3,
            //        Contact4 = content.Contact4,
            //        Contact5 = content.Contact5,
            //        Contact6 = content.Contact6,
            //        Tel1 = content.Tel1,
            //        Tel2 = content.Tel2,
            //        Tel3 = content.Tel3,
            //        Tel4 = content.Tel4,
            //        Tel5 = content.Tel5,
            //        Tel6 = content.Tel6,
            //        Email1 = content.Email1,
            //        Email2 = content.Email2,
            //        Email3 = content.Email3,
            //        Email4 = content.Email4,
            //        Email5 = content.Email5,
            //        Email6 = content.Email6,
            //        OrderSubmission = lSumission,
            //        OrderCreation = lEditable
            //    };
            //}

            //set kookie for customer and project
            //HttpCookie lCustCookies = new HttpCookie("nsh_digios_cust");
            //lCustCookies.Value = CustomerCode;
            //lCustCookies.Expires = DateTime.Now.AddDays(30);
            //HttpContext.Response.Cookies.Remove("nsh_digios_cust");
            //HttpContext.Response.SetCookie(lCustCookies);

            //HttpCookie lProjCookies = new HttpCookie("nsh_digios_proj");
            //lProjCookies.Value = ProjectCode;
            //lProjCookies.Expires = DateTime.Now.AddDays(30);
            //HttpContext.Response.Cookies.Remove("nsh_digios_proj");
            //HttpContext.Response.SetCookie(lProjCookies);

            //return Json(content1, JsonRequestBehavior.AllowGet);
            return Ok(content1);

        }
        [HttpPost]
        [Route("/getSupportBarSetting_bpc")]
        //[ValidateAntiForgeryHeader]
        public ActionResult getSupportBarSetting()
        {
            var lCmd = new SqlCommand();
            SqlDataReader lRst;
            var lNDSCon = new SqlConnection();
            var lReturn = (new[]{ new
            {
                pile_dia = 0,
                mainbar_size_fr = 0,
                mainbar_size_to = 0,
                mainbar_qty_fr = 0,
                link_dia_to = 0,
                extra_supportbar_dia = 0
            }}).ToList();


            var lProcessObj = new ProcessController();
            if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
            {
                lCmd.CommandText = "SELECT pile_dia " +
                ",mainbar_size_fr " +
                ",mainbar_size_to " +
                ",mainbar_qty_fr " +
                ",link_dia_to " +
                ",extra_supportbar_dia " +
                "FROM dbo.OESBPCExtraSupportBars " +
                "ORDER BY pile_dia " +
                ",mainbar_size_fr " +
                ",mainbar_size_to " +
                ",mainbar_qty_fr " +
                ",link_dia_to ";

                lCmd.Connection = lNDSCon;
                lCmd.CommandTimeout = 300;
                lRst = lCmd.ExecuteReader();
                if (lRst.HasRows)
                {
                    while (lRst.Read())
                    {
                        lReturn.Add(new
                        {
                            pile_dia = lRst.GetValue(0) == DBNull.Value ? 0 : lRst.GetInt32(0),
                            mainbar_size_fr = lRst.GetValue(1) == DBNull.Value ? 0 : lRst.GetInt32(1),
                            mainbar_size_to = lRst.GetValue(2) == DBNull.Value ? 0 : lRst.GetInt32(2),
                            mainbar_qty_fr = lRst.GetValue(3) == DBNull.Value ? 0 : lRst.GetInt32(3),
                            link_dia_to = lRst.GetValue(4) == DBNull.Value ? 0 : lRst.GetInt32(4),
                            extra_supportbar_dia = lRst.GetValue(5) == DBNull.Value ? 0 : lRst.GetInt32(5),
                        });
                    }
                }
                lRst.Close();

                lProcessObj.CloseNDSConnection(ref lNDSCon);
            }
            lProcessObj = null;

            return Ok(lReturn);
        }

        //[HttpGet]
        //[Route("/getOrderList_bpc/{CustomerCode}/{ProjectCode}/{Template}/{UserName}")]
        ////[ValidateAntiForgeryHeader]
        //public ActionResult getOrderList(string CustomerCode, string ProjectCode, bool Template, string UserName)
        //{
        //    createJobAdvice(CustomerCode, ProjectCode, Template, false, UserName);
        //    if (Template == false)
        //    {
        //        updateOrderStatus(CustomerCode, ProjectCode);
        //    }

        //    //var content = (from p in db.BPCJobAdvice
        //    //               join s in db.BPCPODoc
        //    //               on new { a = p.CustomerCode, b = p.ProjectCode, c = p.JobID } equals
        //    //               new { a = s.CustomerCode, b = s.ProjectCode, c = s.JobID } into s1
        //    //               from d in s1.DefaultIfEmpty()
        //    //               where p.CustomerCode == CustomerCode &&
        //    //               p.ProjectCode == ProjectCode
        //    //               orderby p.JobID descending
        //    //               select new
        //    //               {
        //    //                   p.JobID,
        //    //                   p.PONumber,
        //    //                   p.RequiredDate,
        //    //                   p.OrderStatus,
        //    //                   d.FileName
        //    //               }
        //    //               ).Take(50).ToList();

        //    var content = (from p in db.BPCJobAdvice
        //                   let s = db.BPCPODoc.Where(s2 => p.CustomerCode == s2.CustomerCode &&
        //                   p.ProjectCode == s2.ProjectCode &&
        //                   p.JobID == s2.JobID).FirstOrDefault()
        //                   where p.CustomerCode == CustomerCode &&
        //                   p.ProjectCode == ProjectCode &&
        //                   p.Template == Template
        //                   orderby p.JobID descending
        //                   select new
        //                   {
        //                       JobID = p.JobID,
        //                       PONumber = p.PONumber,
        //                       RequiredDate = p.RequiredDate,
        //                       OrderStatus = p.OrderStatus,
        //                       FileName = s.FileName
        //                   }
        //       ).Take(50).ToList();

        //    var content1 = new List<struOrderList>(content.Select(h => new struOrderList
        //    {
        //        OrderNo = h.JobID.ToString(),
        //        OrderDesc = ((CustomerCode == "0000000000" || Template == true) ?
        //        h.JobID.ToString() + " ID:" + (h.PONumber == null ? "" : h.PONumber.Trim()) :
        //        (h.JobID.ToString() + " PO:" + (h.PONumber == null ? "" : h.PONumber.Trim()) + " RD:"
        //        + (h.RequiredDate == null ? "" : h.RequiredDate.ToString("yyyy-MM-dd"))
        //        + " Status:" + (h.OrderStatus == null ? "" : h.OrderStatus.Trim()
        //        + (h.FileName == null ? "" : " PO Attached")))
        //        )

        //    }));

        //    return Ok(content1);
        //}

        //[HttpGet]
        //[Route("/updateOrderStatus_bpc/{CustomerCode}/{ProjectCode}")]
        //int updateOrderStatus(string pCustomerCode, string pProjectCode)
        //{
        //    var lCmd = new SqlCommand();
        //    var lCmdUpdate = new SqlCommand();

        //    SqlDataReader lRst;
        //    var lNDSCon = new SqlConnection();

        //    var lCISCon = new OracleConnection();
        //    var lOraCmd = new OracleCommand();
        //    OracleDataReader lOraRst;
        //    int lReturn = 0;
        //    string lSAPSO = "";
        //    int lJOBID = 0;
        //    string lSQL = "";
        //    string lOutTime = "";

        //    var lProcessObj = new ProcessController();
        //    if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
        //    {
        //        lCmd.CommandText = "SELECT L.sap_so, L.JobID " +
        //        "FROM dbo.OESBPCJobAdvice J, dbo.OESBPCLoad L " +
        //        "WHERE J.CustomerCode = L.CustomerCode " +
        //        "AND J.ProjectCode = L.ProjectCode " +
        //        "AND Template = 0 " +
        //        "AND J.JobID = L.JobID " +
        //        "AND J.CustomerCode = '" + pCustomerCode + "' " +
        //        "AND J.ProjectCode = '" + pProjectCode + "' " +
        //        "AND L.sap_so > '' " +
        //        "AND J.OrderStatus = 'Processed' ";

        //        lCmd.Connection = lNDSCon;
        //        lCmd.CommandTimeout = 300;
        //        lRst = lCmd.ExecuteReader();
        //        if (lRst.HasRows)
        //        {
        //            if (lProcessObj.OpenCISConnection(ref lCISCon) == true)
        //            {
        //                while (lRst.Read())
        //                {
        //                    lSAPSO = lRst.GetValue(0) == DBNull.Value ? "" : lRst.GetString(0).Trim();
        //                    lJOBID = lRst.GetValue(1) == DBNull.Value ? 0 : lRst.GetInt32(1);

        //                    lSQL = "SELECT  " +
        //                    "NVL(MAX(WEIGH_OUT_TIME), ' ') " +
        //                    "FROM SAPSR3.YMPPT_LOAD_VEHIC " +
        //                    "WHERE MANDT = '" + lProcessObj.strClient + "' " +
        //                    "AND VBELN = '" + lSAPSO + "' " +
        //                    "ORDER BY 1 DESC ";

        //                    lOraCmd.CommandText = lSQL;
        //                    lOraCmd.Connection = lCISCon;
        //                    lOraCmd.CommandTimeout = 300;
        //                    lOraRst = lOraCmd.ExecuteReader();
        //                    if (lOraRst.HasRows)
        //                    {
        //                        if (lOraRst.Read())
        //                        {
        //                            lOutTime = lOraRst.GetString(0).Trim();

        //                            if (lOutTime != "")
        //                            {
        //                                lCmdUpdate.CommandText = "Update dbo.OESBPCJobAdvice " +
        //                                "SET OrderStatus = 'Delivered' " +
        //                                "WHERE CustomerCode = '" + pCustomerCode + "' " +
        //                                "AND ProjectCode = '" + pProjectCode + "' " +
        //                                "AND Template = 0 " +
        //                                "AND JobID = " + lJOBID.ToString() + " ";

        //                                lCmdUpdate.Connection = lNDSCon;
        //                                lCmdUpdate.CommandTimeout = 300;
        //                                lCmdUpdate.ExecuteNonQuery();
        //                            }
        //                        }
        //                    }
        //                    lOraRst.Close();
        //                }
        //                lProcessObj.CloseCISConnection(ref lCISCon);
        //            }
        //        }
        //        lProcessObj.CloseNDSConnection(ref lNDSCon);
        //    }
        //    lProcessObj = null;

        //    return lReturn;
        //}

        [HttpGet]
        [Route("/createJobAdvice_bpc/{CustomerCode}/{ProjectCode}/{pTemplate}/{pClone}/{UserName}")]
        public int createJobAdvice(string pCustomerCode, string pProjectCode, bool pTemplate, bool pClone, string UserName)
        {
            var lCmd = new SqlCommand();
            SqlDataReader lRst;
            var lNDSCon = new SqlConnection();
            var lErrorMsg = "";
            int lFound = 0;
            int? lJobID = 0;
            try
            {
                if (pCustomerCode == null)
                {
                    pCustomerCode = "";
                }
                if (pProjectCode == null)
                {
                    pProjectCode = "";
                }
                pCustomerCode = pCustomerCode.Trim();
                pProjectCode = pProjectCode.Trim();
                if (pCustomerCode.Length > 0 && pProjectCode.Length > 0)
                {
                    var lSumission = "No";
                    var lEditable = "No";

                    string lUserType = "";
                    if (gUserType != null && gUserType != "")
                    {
                        lUserType = gUserType;
                    }
                    else
                    {
                        UserAccessController lUa = new UserAccessController();
                        lUserType = lUa.getUserType(UserName);
                        gUserType = lUserType;
                        lUa = null;
                    }

                    //if (lUserType == "CU" || lUserType == "CA" || lUserType == "TE")
                    //{
                    //    var lAccess = db.UserAccess.Find(User.Identity.Name, pCustomerCode, pProjectCode);
                    //    if (lAccess != null)
                    //    {
                    //        lSumission = lAccess.OrderSubmission.Trim();
                    //        lEditable = lAccess.OrderCreation.Trim();
                    //    }
                    //    if (lEditable == "Yes" || lUserType == "TE" || lUserType == "AD")
                    //    {
                    if (lUserType == "CU" || lUserType == "CA" || lUserType == "PL" || lUserType == "AD" || lUserType == "PM" || lUserType == "PA" || lUserType == "P1" || lUserType == "P2" || lUserType == "PU" || lUserType == "TE")
                    {
                        var lAccess = db.UserAccess.Find(User.Identity.Name, pCustomerCode, pProjectCode);
                        if (lAccess != null)
                        {
                            lSumission = lAccess.OrderSubmission.Trim();
                            lEditable = lAccess.OrderCreation.Trim();
                        }
                        if (lUserType == "PL" || lUserType == "AD" || lUserType == "PM" || lUserType == "PA" || lUserType == "P1" || lUserType == "P2" || lUserType == "PU" || lUserType == "TE")
                        {
                            lEditable = "Yes";
                        }

                        if (lEditable == "Yes")
                        {
                            lJobID = db.BPCJobAdvice.Where(z => z.CustomerCode == pCustomerCode &&
                            z.ProjectCode == pProjectCode &&
                            z.Template == pTemplate).Max(z => (int?)z.JobID);

                            lFound = 0;
                            if (lJobID != null)
                            {
                                lFound = 1;
                                var lCheckSheetDet = (from p in db.BPCDetails
                                                      where p.CustomerCode == pCustomerCode &&
                                                      p.ProjectCode == pProjectCode &&
                                                      p.Template == pTemplate &&
                                                      p.JobID == lJobID &&
                                                      p.cage_qty > 0 &&
                                                      p.UpdateBy != User.Identity.Name
                                                      select p).ToList();

                                var lCheckJob = db.BPCJobAdvice.Find(pCustomerCode, pProjectCode, pTemplate, lJobID);

                                if (lCheckJob == null || (lCheckJob.OrderStatus != "New" && lCheckJob.OrderStatus != "Created" && lCheckJob.OrderStatus != "OnHold") ||
                                    lCheckSheetDet.Count > 0 ||
                                    (lCheckJob.PONumber != null && lCheckJob.PONumber != "") ||
                                    (lCheckJob.TotalWeight > 0 &&
                                    (lCheckJob.UpdateBy != UserName ||
                                    lCheckJob.Template == true ||
                                    pClone == true)))
                                {
                                    lFound = 0;
                                }

                            }


                            if (lFound == 0)
                            {

                                var lJobAdv = new BPCJobAdviceModels();
                                lJobAdv.CustomerCode = pCustomerCode;
                                lJobAdv.ProjectCode = pProjectCode;
                                lJobAdv.Template = pTemplate;
                                if (lJobID == null)
                                {
                                    lJobAdv.JobID = 1;
                                }
                                else
                                {
                                    lJobAdv.JobID = (int)lJobID + 1;
                                }

                                lJobAdv.PODate = DateTime.Now;
                                lJobAdv.RequiredDate = DateTime.Now.AddDays(10);
                                lJobAdv.TotalPcs = 0;
                                lJobAdv.TotalWeight = 0;

                                lJobAdv.Transport = "TR40/24";
                                lJobAdv.OrderStatus = "New";
                                lJobAdv.cover_to_link = 75;
                                lJobAdv.OrderSource = "WEB";
                                lJobAdv.DeliveryAddress = "";
                                lJobAdv.Remarks = "";

                                var lProj = db.Project.Find(pCustomerCode, pProjectCode);
                                if (lProj != null)
                                {
                                    var lProcessObj = new ProcessController();
                                    if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
                                    {
                                        lCmd.CommandText =
                                        "SELECT isNull(MAX(JobID), 0) FROM dbo.OESBPCJobAdvice " +
                                        "WHERE CustomerCode = '" + pCustomerCode + "' " +
                                        "AND ProjectCode = '" + pProjectCode + "' " +
                                        "AND Template = " + (pTemplate == true ? 1 : 0) + " " +
                                        "AND OrderStatus <> 'New' " +
                                        "AND OrderStatus <> 'Created' " +
                                        "AND OrderStatus is not null " +
                                        "AND OrderStatus <> '' " +
                                        "AND (SiteEngr_Name > '' " +
                                        "OR Scheduler_Name > '' " +
                                        "OR DeliveryAddress > '' ) ";

                                        int LastJobID = 0;

                                        lCmd.Connection = lNDSCon;
                                        lCmd.CommandTimeout = 300;
                                        lRst = lCmd.ExecuteReader();
                                        if (lRst.HasRows)
                                        {
                                            if (lRst.Read())
                                            {
                                                LastJobID = lRst.GetInt32(0);
                                            }
                                        }
                                        lRst.Close();

                                        if (LastJobID > 0)
                                        {
                                            if (lUserType == "CU" || lUserType == "CA")
                                            {
                                                lCmd.CommandText =
                                                "SELECT isNull(MAX(JobID), 0) FROM dbo.OESBPCJobAdvice " +
                                                "WHERE CustomerCode = '" + pCustomerCode + "' " +
                                                "AND Template = " + (pTemplate == true ? 1 : 0) + " " +
                                                "AND ProjectCode = '" + pProjectCode + "' " +
                                                "AND OrderStatus <> 'New' " +
                                                "AND OrderStatus <> 'Created' " +
                                                "AND OrderStatus is not null " +
                                                "AND OrderStatus <> '' " +
                                                "AND UpdateBy = '" + UserName + "' " +
                                                "AND (SiteEngr_Name > '' " +
                                                "OR Scheduler_Name > '' " +
                                                "OR DeliveryAddress > '' ) ";

                                                int LastMyJobID = 0;

                                                lCmd.Connection = lNDSCon;
                                                lCmd.CommandTimeout = 300;
                                                lRst = lCmd.ExecuteReader();
                                                if (lRst.HasRows)
                                                {
                                                    if (lRst.Read())
                                                    {
                                                        LastMyJobID = lRst.GetInt32(0);
                                                    }
                                                }
                                                lRst.Close();


                                                lCmd.CommandText =
                                                "SELECT isNull(SiteEngr_Name, ''), " +
                                                "isNull(SiteEngr_HP, ''), " +
                                                "isNull(SiteEngr_Tel, ''), " +
                                                "isNull(Scheduler_Name, ''), " +
                                                "isNull(Scheduler_HP, ''), " +
                                                "isNull(Scheduler_Tel, ''), " +
                                                "isNull(DeliveryAddress, '') " +
                                                "FROM dbo.OESBPCJobAdvice " +
                                                "WHERE CustomerCode = '" + pCustomerCode + "' " +
                                                "AND ProjectCode = '" + pProjectCode + "' " +
                                                "AND Template = " + (pTemplate == true ? 1 : 0) + " " +
                                                "AND JobID = " + LastMyJobID.ToString() + " " +
                                                "AND UpdateBy = '" + UserName + "' " +
                                                "UNION " +
                                                "SELECT isNull(SiteEngr_Name, ''), " +
                                                "isNull(SiteEngr_HP, ''), " +
                                                "isNull(SiteEngr_Tel, ''), " +
                                                "isNull(Scheduler_Name, ''), " +
                                                "isNull(Scheduler_HP, ''), " +
                                                "isNull(Scheduler_Tel, ''), " +
                                                "isNull(DeliveryAddress, '') " +
                                                "FROM dbo.OESBPCJobAdvice " +
                                                "WHERE CustomerCode = '" + pCustomerCode + "' " +
                                                "AND ProjectCode = '" + pProjectCode + "' " +
                                                "AND Template = " + (pTemplate == true ? 1 : 0) + " " +
                                                "AND JobID = " + LastJobID.ToString() + " ";
                                            }
                                            else
                                            {
                                                lCmd.CommandText =
                                                "SELECT isNull(SiteEngr_Name,''), " +
                                                "isNull(SiteEngr_HP,''), " +
                                                "isNull(SiteEngr_Tel,''), " +
                                                "isNull(Scheduler_Name,''), " +
                                                "isNull(Scheduler_HP,''), " +
                                                "isNull(Scheduler_Tel, ''), " +
                                                "isNull(DeliveryAddress, '') " +
                                                "FROM dbo.OESBPCJobAdvice " +
                                                "WHERE CustomerCode = '" + pCustomerCode + "' " +
                                                "AND ProjectCode = '" + pProjectCode + "' " +
                                                "AND Template = " + (pTemplate == true ? 1 : 0) + " " +
                                                "AND JobID = " + LastJobID.ToString() + " ";
                                            }

                                            lCmd.Connection = lNDSCon;
                                            lCmd.CommandTimeout = 300;
                                            lRst = lCmd.ExecuteReader();
                                            if (lRst.HasRows)
                                            {
                                                if (lRst.Read())
                                                {
                                                    lJobAdv.SiteEngr_Name = lRst.GetString(0);
                                                    lJobAdv.SiteEngr_HP = lRst.GetString(1);
                                                    lJobAdv.SiteEngr_Tel = lRst.GetString(2);
                                                    lJobAdv.Scheduler_Name = lRst.GetString(3);
                                                    lJobAdv.Scheduler_HP = lRst.GetString(4);
                                                    lJobAdv.Scheduler_Tel = lRst.GetString(5);
                                                    lJobAdv.DeliveryAddress = lRst.GetString(6);
                                                }
                                            }
                                            else
                                            {
                                                lJobAdv.Scheduler_Name = lProj.Scheduler_Name;
                                                lJobAdv.Scheduler_HP = lProj.Scheduler_HP;
                                                lJobAdv.Scheduler_Tel = lProj.Scheduler_Tel;
                                                lJobAdv.SiteEngr_Name = lProj.SiteEngr_Name;
                                                lJobAdv.SiteEngr_HP = lProj.SiteEngr_HP;
                                                lJobAdv.SiteEngr_Tel = lProj.SiteEngr_Tel;
                                                lJobAdv.DeliveryAddress = "";
                                            }
                                            lRst.Close();
                                        }
                                        lProcessObj.CloseNDSConnection(ref lNDSCon);
                                    }

                                    lCmd = null;
                                    lRst = null;
                                    lProcessObj = null;
                                }

                                lJobAdv.PONumber = "";
                                lJobAdv.UpdateDate = DateTime.Now;

                                lJobAdv.UpdateBy = UserName;//"Vishalw_ttl@natsteel.com.sg";

                                var oldJobAdvice = db.BPCJobAdvice.Find(lJobAdv.CustomerCode, lJobAdv.ProjectCode, pTemplate, lJobAdv.JobID);
                                if (oldJobAdvice == null)
                                {
                                    db.BPCJobAdvice.Add(lJobAdv);
                                }
                                else
                                {
                                    db.Entry(oldJobAdvice).CurrentValues.SetValues(lJobAdv);
                                }

                                db.SaveChanges();
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                lErrorMsg = ex.Message;
                return 0;
            }
            return 0;
        }


        [HttpPost]
        [Route("/getOrderListSearch_bpc/{CustomerCode}/{ProjectCode}/{Template}/pClone/RequiredDateFrom/RequiredDateTo/PONo")]
        //[ValidateAntiForgeryHeader]
        public ActionResult getOrderListSearch(string CustomerCode, string ProjectCode, bool Template, string RequiredDateFrom, string RequiredDateTo, string PONo)
        {
            var lCmd = new SqlCommand();
            SqlDataReader lRst;
            var lNDSCon = new SqlConnection();
            var lReturn = new List<struOrderList>();

            if (RequiredDateFrom == null) RequiredDateFrom = "";
            if (RequiredDateFrom.Trim().Length == 0)
            {
                RequiredDateFrom = "2000-01-01";
            }
            if (RequiredDateTo == null) RequiredDateTo = "";
            if (RequiredDateTo.Trim().Length == 0)
            {
                RequiredDateTo = "2100-01-01";
            }
            if (PONo == null) PONo = "";
            PONo = PONo.Trim();
            //DateTime lRequiredDateFrom = DateTime.ParseExact(RequiredDateFrom, "yyyy-MM-dd",CultureInfo.InvariantCulture);
            //DateTime lRequiredDateTo = DateTime.ParseExact(RequiredDateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            //var content = new List<JobAdviceModels>();
            //var content = (from p in db.JobAdvice
            //           join s in db.PODoc
            //           on new { a = p.CustomerCode, b = p.ProjectCode, c = p.JobID } equals
            //           new { a = s.CustomerCode, b = s.ProjectCode, c = s.JobID } 
            //           where p.CustomerCode == "" &&
            //           p.ProjectCode == "" 
            //           select new
            //           {
            //               p.JobID,
            //               p.PONumber,
            //               p.RequiredDate,
            //               p.OrderStatus,
            //               s.FileName
            //           }
            //           ).ToList();

            //if (BBSNo == null || BBSNo == "" || BBSNo.Trim().Length == 0)
            //{
            //content = (from p in db.JobAdvice
            //           where p.CustomerCode == CustomerCode &&
            //           p.ProjectCode == ProjectCode &&
            //           p.RequiredDate >= lRequiredDateFrom &&
            //           p.RequiredDate <= lRequiredDateTo &&
            //           p.PONumber.Contains(PONo)
            //           orderby p.JobID descending
            //           select p).Take(50).ToList();

            //var content = (from p in db.JobAdvice
            //               join s in db.PODoc
            //               on new { a = p.CustomerCode, b = p.ProjectCode, c = p.JobID } equals
            //               new { a = s.CustomerCode, b = s.ProjectCode, c = s.JobID } into s1
            //               from d in s1.DefaultIfEmpty()
            //               where p.CustomerCode == CustomerCode &&
            //               p.ProjectCode == ProjectCode &&
            //               p.RequiredDate >= lRequiredDateFrom &&
            //               p.RequiredDate <= lRequiredDateTo &&
            //               (p.PONumber.Contains(PONo) ||
            //               PONo == "") &&
            //               (p.WBS1 == Block ||
            //               Block == "") &&
            //               (p.WBS2 == Storey ||
            //               Storey == "") &&
            //               (p.WBS3 == Part ||
            //               Part == "")
            //               orderby p.JobID descending
            //               select new
            //               {
            //                   p.JobID,
            //                   p.PONumber,
            //                   p.RequiredDate,
            //                   p.OrderStatus,
            //                   d.FileName
            //               }
            //               ).Take(50).ToList();

            //var content1 = new List<struOrderList>(content.Select(h => new struOrderList
            //{
            //    OrderNo = h.JobID.ToString(),
            //    OrderDesc = h.JobID.ToString() + " PO:" + h.PONumber.Trim()
            //    + " RD:" + h.RequiredDate.ToString("yyyy-MM-dd")
            //    + " Status:" + h.OrderStatus.Trim()
            //    + (h.FileName == null ? "" : " PO Attached")
            //}));



            //return Json(content1, JsonRequestBehavior.AllowGet);
            //}
            //else
            //{
            //    var contentBBS = (from p in db.JobAdvice
            //               join bbs in db.BBS on p.JobID equals bbs.JobID
            //               where p.CustomerCode == CustomerCode &&
            //               p.ProjectCode == ProjectCode &&
            //               bbs.CustomerCode == CustomerCode &&
            //               bbs.ProjectCode == ProjectCode &&
            //               p.RequiredDate >= lRequiredDateFrom &&
            //               p.RequiredDate <= lRequiredDateTo &&
            //               (p.PONumber.Contains(PONo) ||
            //               PONo == "") &&
            //               (p.WBS1 == Block ||
            //               Block == "") &&
            //               (p.WBS2 == Storey ||
            //               Storey == "") &&
            //               (p.WBS3 == Part ||
            //               Part == "") &&
            //               bbs.BBSNo.Contains(BBSNo)
            //               orderby p.JobID descending
            //               select p).Distinct().Take(50).ToList();

            //    var content = (from p in contentBBS
            //               join s in db.PODoc
            //               on new { a = p.CustomerCode, b = p.ProjectCode, c = p.JobID } equals
            //               new { a = s.CustomerCode, b = s.ProjectCode, c = s.JobID } into s1
            //               from d in s1.DefaultIfEmpty()
            //               where p.CustomerCode == CustomerCode &&
            //               p.ProjectCode == ProjectCode &&
            //               p.RequiredDate >= lRequiredDateFrom &&
            //               p.RequiredDate <= lRequiredDateTo &&
            //               (p.PONumber.Contains(PONo) ||
            //               PONo == "") &&
            //               (p.WBS1 == Block ||
            //               Block == "") &&
            //               (p.WBS2 == Storey ||
            //               Storey == "") &&
            //               (p.WBS3 == Part ||
            //               Part == "")
            //               orderby p.JobID descending
            //               select new
            //               {
            //                   p.JobID,
            //                   p.PONumber,
            //                   p.RequiredDate,
            //                   p.OrderStatus,
            //                   d.FileName
            //               }
            //               ).Take(50).ToList();

            //    var content1 = new List<struOrderList>(content.Select(h => new struOrderList
            //    {
            //        OrderNo = h.JobID.ToString(),
            //        OrderDesc = h.JobID.ToString() + " PO:" + h.PONumber.Trim()
            //        + " RD:" + h.RequiredDate.ToString("yyyy-MM-dd")
            //        + " Status:" + h.OrderStatus.Trim()
            //        + (h.FileName == null ? "" : " PO Attached")
            //    }));

            //    return Json(content1, JsonRequestBehavior.AllowGet);
            //}

            lCmd.CommandText = " SELECT p.JobID, " +
            "isNull(p.PONumber,''), " +
            "p.RequiredDate, " +
            "isNull(p.OrderStatus,''), " +
            "isNull((SELECT Max(s.FileName) " +
            "FROM dbo.OESBPCPODoc s " +
            "WHERE s.CustomerCode = p.CustomerCode " +
            "AND s.ProjectCode = p.ProjectCode " +
            "AND s.JobID = p.JobID),'') " +
            "FROM dbo.OESBPCJobAdvice p " +
            "WHERE p.CustomerCode = '" + CustomerCode + "' " +
            "AND p.ProjectCode = '" + ProjectCode + "' " +
            "AND p.Template = " + (Template == false ? 0 : 1) + " " +
            "AND p.RequiredDate >= '" + RequiredDateFrom + "' " +
            "AND p.RequiredDate <= '" + RequiredDateTo + "' " +
            "AND (p.PONumber like '%" + PONo + "%' " +
            "OR '" + PONo + "' = '') " +
            "ORDER BY p.JobID DESC ";

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
                        lReturn.Add(new struOrderList()
                        {
                            OrderNo = lRst.GetInt32(0).ToString(),
                            OrderDesc = ((CustomerCode == "0000000000" || Template == true) ?
                                lRst.GetInt32(0).ToString() + " ID:" + (lRst.GetValue(1) == null ? "" : lRst.GetString(1).Trim()) :
                                (lRst.GetInt32(0).ToString() + " PO:" + (lRst.GetValue(1) == null ? "" : lRst.GetString(1).Trim()) + " RD:"
                                + (lRst.GetValue(2) == null ? "" : lRst.GetDateTime(2).ToString("yyyy-MM-dd"))
                                + " Status:" + (lRst.GetValue(3) == null ? "" : lRst.GetString(3).Trim()
                                + (lRst.GetString(4) == null || lRst.GetString(4).Trim() == "" ? "" : " PO Attached")))
                                )
                        });
                    }
                }
                lRst.Close();

                lProcessObj.CloseNDSConnection(ref lNDSCon);
            }

            lProcessObj = null;
            lCmd = null;
            lNDSCon = null;
            lRst = null;
            return Ok(lReturn);

        }

        //get Job Advice details
        [HttpGet]
        //[ValidateAntiForgeryHeader]
        [Route("/getOrderDetails_bpc/{CustomerCode}/{ProjectCode}/{Template}/{JobID}")]
        public ActionResult getOrderDetails(string CustomerCode, string ProjectCode, bool Template, int JobID)
        {
            int lPODocExists = 0;

            string lSubmission = "No";
            string lEditable = "No";

            var lJob = db.BPCJobAdvice.Find(CustomerCode, ProjectCode, Template, JobID);
            if (lJob != null && (lJob.OrderStatus == "New" || lJob.OrderStatus == "Created"))
            {
                var lAccess = db.UserAccess.Find(User.Identity.Name, CustomerCode, ProjectCode);
                if (lAccess != null)
                {
                    lSubmission = lAccess.OrderSubmission.Trim();
                    lEditable = lAccess.OrderCreation.Trim();
                }
                lSubmission = (lSubmission == null ? "" : lSubmission);
                lEditable = (lEditable == null ? "" : lEditable);
            }

            //var content1 = db.BPCPODoc.Find(CustomerCode, ProjectCode, JobID);
            //if (content1 != null)
            //{
            //    if (content1.FileName != null)
            //    {
            //        if (content1.FileName.Trim().Length > 0)
            //        {
            //            lPODocExists = 1;
            //        }
            //    }
            //}

            lPODocExists = db.BPCPODoc.Where(
                                  p => p.CustomerCode == CustomerCode &&
                                  p.ProjectCode == ProjectCode &&
                                  p.JobID == JobID).Count();

            if (lJob != null)
            {
                if (lJob.CustomerCode == null) lJob.CustomerCode = "";
                else lJob.CustomerCode = lJob.CustomerCode.Trim();

                if (lJob.OrderStatus == null) lJob.OrderStatus = "";
                else lJob.OrderStatus = lJob.OrderStatus.Trim();

                if (lJob.PONumber == null) lJob.PONumber = "";
                else lJob.PONumber = lJob.PONumber.Trim();

                if (lJob.ProjectCode == null) lJob.ProjectCode = "";
                else lJob.ProjectCode = lJob.ProjectCode.Trim();

                if (lJob.Transport == null) lJob.Transport = "";
                else lJob.Transport = lJob.Transport.Trim();

                if (lJob.DeliveryAddress == null) lJob.DeliveryAddress = "";
                else lJob.DeliveryAddress = lJob.DeliveryAddress.Trim();

                if (lJob.Remarks == null) lJob.Remarks = "";
                else lJob.Remarks = lJob.Remarks.Trim();

                if (lJob.Scheduler_HP == null) lJob.Scheduler_HP = "";
                else lJob.Scheduler_HP = lJob.Scheduler_HP.Trim();

                if (lJob.Scheduler_Name == null) lJob.Scheduler_Name = "";
                else lJob.Scheduler_Name = lJob.Scheduler_Name.Trim();

                if (lJob.Scheduler_Tel == null) lJob.Scheduler_Tel = "";
                else lJob.Scheduler_Tel = lJob.Scheduler_Tel.Trim();

                if (lJob.SiteEngr_HP == null) lJob.SiteEngr_HP = "";
                else lJob.SiteEngr_HP = lJob.SiteEngr_HP.Trim();

                if (lJob.SiteEngr_Name == null) lJob.SiteEngr_Name = "";
                else lJob.SiteEngr_Name = lJob.SiteEngr_Name.Trim();

                if (lJob.SiteEngr_Tel == null) lJob.SiteEngr_Tel = "";
                else lJob.SiteEngr_Tel = lJob.SiteEngr_Tel.Trim();
            }
            var lReturn = new
            {
                CustomerCode = lJob.CustomerCode.Trim(),
                ProjectCode = lJob.ProjectCode.Trim(),
                Template = lJob.Template,
                JobID = lJob.JobID,
                PONumber = lJob.PONumber.Trim(),
                PODate = lJob.PODate,
                RequiredDate = lJob.RequiredDate,
                Transport = lJob.Transport,
                OrderStatus = lJob.OrderStatus.Trim(),
                TotalPcs = lJob.TotalPcs,
                TotalWeight = lJob.TotalWeight,
                Scheduler_HP = lJob.Scheduler_HP.Trim(),
                Scheduler_Name = lJob.Scheduler_Name.Trim(),
                Scheduler_Tel = lJob.Scheduler_Tel.Trim(),
                SiteEngr_HP = lJob.SiteEngr_HP.Trim(),
                SiteEngr_Name = lJob.SiteEngr_Name.Trim(),
                SiteEngr_Tel = lJob.SiteEngr_Tel.Trim(),
                DeliveryAddress = lJob.DeliveryAddress.Trim(),
                Remarks = lJob.Remarks.Trim(),
                cover_to_link = lJob.cover_to_link,
                underload_ct = lJob.underload_ct,
                pile_category = lJob.pile_category,
                UpdateDate = lJob.UpdateDate,
                Exists = lPODocExists
            };
            lJob = null;
            return Ok(lReturn);
        }

        //get Bars List 
        [HttpGet]
        [Route("/getRebarData_bpc/{CustomerCode}/{ProjectCode}/{JobID}/CageID/{Template}")]
        //[ValidateAntiForgeryHeader]
        public ActionResult getRebarData(string CustomerCode, string ProjectCode, int JobID, int CageID, bool Template)
        {
            var lReturn = (new[]{ new
            {
                BarID = 0,
                BarMark = "",
                BarType ="",
                BarSize = 0,
                BarTotalQty = 0,
                BarShapeCode = "",
                //BarShape = new byte[]{0},
                A = 0,
                B = 0,
                C = 0,
                D = 0,
                E = 0,
                F = 0,
                G = 0,
                BarLength = 0,
                BarWeight = (decimal)0,
                Remarks = ""
            }}).ToList();


            var lOrderDetails = (from p in db.BPCCageBars
                                 where p.CustomerCode == CustomerCode &&
                                 p.ProjectCode == ProjectCode &&
                                 p.Template == Template &&
                                 p.JobID == JobID &&
                                 p.CageID == CageID
                                 orderby p.BarSort
                                 select p).ToList();

            if (lOrderDetails.Count > 0)
            {
                for (int j = 0; j < lOrderDetails.Count; j++)
                {
                    var lVar = lOrderDetails[j].BarShapeCode.Trim();

                    var lImage = db.Shape.Find(lVar);

                    lReturn.Add(new
                    {
                        BarID = j + 1,
                        BarMark = lOrderDetails[j].BarMark,
                        BarType = lOrderDetails[j].BarType,
                        BarSize = (int)lOrderDetails[j].BarSize,
                        BarTotalQty = (int)lOrderDetails[j].BarTotalQty,
                        BarShapeCode = lOrderDetails[j].BarShapeCode,
                        //BarShape = lImage.shapeImage,
                        A = (int)lOrderDetails[j].A,
                        B = (int)lOrderDetails[j].B,
                        C = (int)lOrderDetails[j].C,
                        D = (int)lOrderDetails[j].D,
                        E = (int)lOrderDetails[j].E,
                        F = (int)lOrderDetails[j].F,
                        G = (int)lOrderDetails[j].G,
                        BarLength = (int)lOrderDetails[j].BarLength,
                        BarWeight = (decimal)lOrderDetails[j].BarWeight,
                        Remarks = lOrderDetails[j].Remarks
                    });

                }
            }


            if (lReturn.Count > 0)
            {
                lReturn.RemoveAt(0);
            }

            return Ok(lReturn);
        }

        [HttpGet]
        [Route("/getBPCConfig_bpc/{CustomerCode}/{ProjectCode}")]
        //[ValidateAntiForgeryHeader]
        public ActionResult getBPCConfig(string CustomerCode, string ProjectCode)
        {
            var lErrorMsg = "";
            var lResult = false;
            try
            {
                if (CustomerCode != null && CustomerCode.Length > 0 && ProjectCode != null && ProjectCode.Length > 0)
                {

                    var oldProj = db.Project.Find(CustomerCode, ProjectCode);
                    if (oldProj != null)
                    {
                        lResult = (oldProj.bpc_spiral_lapping == null ? false : (bool)oldProj.bpc_spiral_lapping);
                    }
                }
                //return Json(new { success = true, responseText = "Successfully saved.", result = lResult }, JsonRequestBehavior.AllowGet);
                return Ok(lResult);

            }
            catch (Exception ex)
            {
                lErrorMsg = ex.Message;
            }
            //return Json(new { success = false, responseText = lErrorMsg, result = false }, JsonRequestBehavior.AllowGet);
            return Ok();
        }

        [HttpGet]
        [Route("/SaveBPCConfig_bpc/{CustomerCode}/{ProjectCode}/{SLLapping}")]
        //[ValidateAntiForgeryHeader]
        public ActionResult SaveBPCConfig(string CustomerCode, string ProjectCode, bool SLLapping)
        {
            var lErrorMsg = "";
            try
            {
                if (CustomerCode != null && CustomerCode.Length > 0 && ProjectCode != null && ProjectCode.Length > 0)
                {

                    var oldProj = db.Project.Find(CustomerCode, ProjectCode);
                    if (oldProj != null)
                    {
                        var lNewProj = oldProj;
                        lNewProj.bpc_spiral_lapping = SLLapping;
                        db.Entry(oldProj).CurrentValues.SetValues(lNewProj);
                    }
                    db.SaveChanges();


                    //return Json(new { success = true, responseText = "Successfully saved." }, JsonRequestBehavior.AllowGet);
                    return Ok(true);
                }

            }
            catch (Exception ex)
            {
                lErrorMsg = ex.Message;
            }
            //return Json(new { success = false, responseText = lErrorMsg }, JsonRequestBehavior.AllowGet);
            return Ok();
        }

        // POST: OrderDetails/SaveJobAdvice
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        
        [HttpPost]
        [Route("/SaveJobAdvice_bpc")]
        //[ValidateAntiForgeryHeader]
        public async Task<ActionResult> SaveJobAdvice([FromBody] BPCJobAdviceModels jobAdviceModels)
        {
            var lErrorMsg = "";
            int? lJobID = 0;
            try
            {

                jobAdviceModels.UpdateDate = DateTime.Now;
                //jobAdviceModels.UpdateBy = ;//"Vishalw_ttl@natsteel.com.sg";
                jobAdviceModels.PODate = DateTime.Now;

                if (jobAdviceModels.CustomerCode == null)
                {
                    jobAdviceModels.CustomerCode = "";
                }
                if (jobAdviceModels.CustomerCode.Length > 0)
                {
                    if (jobAdviceModels.JobID == 0)
                    {
                        lJobID = await db.BPCJobAdvice.Where(z => z.CustomerCode == jobAdviceModels.CustomerCode &&
                        z.ProjectCode == jobAdviceModels.ProjectCode &&
                        z.Template == jobAdviceModels.Template).MaxAsync(z => (int?)z.JobID);
                        if (lJobID == null)
                        {
                            jobAdviceModels.JobID = 1;
                        }
                        else
                        {
                            jobAdviceModels.JobID = (int)lJobID + 1;
                        }
                    }
                    if (jobAdviceModels.RequiredDate == null)
                    {
                        jobAdviceModels.RequiredDate = DateTime.Now.AddDays(5);
                    }
                    if (jobAdviceModels.RequiredDate < DateTime.Now.AddDays(-365))
                    {
                        jobAdviceModels.RequiredDate = DateTime.Now.AddDays(5);
                    }
                    jobAdviceModels.RequiredDate = jobAdviceModels.RequiredDate.Date;

                    var oldJobAdvice = await db.BPCJobAdvice.FindAsync(jobAdviceModels.CustomerCode, jobAdviceModels.ProjectCode, jobAdviceModels.Template, jobAdviceModels.JobID);
                    if (oldJobAdvice == null)
                    {
                        db.BPCJobAdvice.Add(jobAdviceModels);
                    }
                    else
                    {
                        var lNewJobAdv = oldJobAdvice;
                        lNewJobAdv.cover_to_link = jobAdviceModels.cover_to_link;
                        lNewJobAdv.pile_category = jobAdviceModels.pile_category;
                        lNewJobAdv.redesign_req = jobAdviceModels.redesign_req;
                        lNewJobAdv.underload_ct = jobAdviceModels.underload_ct;
                        lNewJobAdv.TotalPcs = jobAdviceModels.TotalPcs;
                        lNewJobAdv.TotalWeight = jobAdviceModels.TotalWeight;
                        if (jobAdviceModels.Template == true)
                        {
                            lNewJobAdv.PONumber = jobAdviceModels.PONumber;
                        }
                        db.Entry(oldJobAdvice).CurrentValues.SetValues(lNewJobAdv);
                    }

                    //update Master

                    var lCustomerCode = oldJobAdvice.CustomerCode;
                    var lProjectCode = oldJobAdvice.ProjectCode;
                    var myJobID = oldJobAdvice.JobID;
                    var lTotalWT = oldJobAdvice.TotalWeight * 1000;

                    var lProjSE = await (from p in db.OrderProjectSE
                                         join s in db.OrderProject on p.OrderNumber equals s.OrderNumber
                                         where p.BPCJobID == myJobID &&
                                         s.CustomerCode == lCustomerCode &&
                                         s.ProjectCode == lProjectCode
                                         select p).ToListAsync();
                    if (lProjSE != null && lProjSE.Count > 0)
                    {
                        int lOrderNo = lProjSE[0].OrderNumber;
                        string lStrEle = lProjSE[0].StructureElement;
                        string lProd = lProjSE[0].ProductType;

                        var lOldOrder = await db.OrderProject.FindAsync(lOrderNo);

                        if (lOldOrder != null)
                        {
                            var lNewOrder = lOldOrder;

                            lNewOrder.TotalWeight = lTotalWT;

                            db.Entry(lOldOrder).CurrentValues.SetValues(lNewOrder);
                        }

                        var lOldSE = await db.OrderProjectSE.FindAsync(lOrderNo, lStrEle, lProd, "N");
                        if (lOldSE != null)
                        {
                            var lNewSE = lOldSE;

                            lNewSE.TotalWeight = lTotalWT;
                            lNewSE.TotalPCs = jobAdviceModels.TotalPcs;

                            db.Entry(lOldSE).CurrentValues.SetValues(lNewSE);
                        }

                    }

                    db.SaveChanges();

                    //return Json(new { success = true, responseText = "Successfully saved." }, JsonRequestBehavior.AllowGet);
                    return Ok(true);
                }

            }
            catch (Exception ex)
            {
                lErrorMsg = ex.Message;
            }
            //return Json(new { success = false, responseText = lErrorMsg }, JsonRequestBehavior.AllowGet);
            return Ok();
        }

        //[HttpGet]
        //[Route("/OrderWithdraw_bpc/{CustomerCode}/{ProjectCode}/{JobID}/{UserName}")]
        ////[ValidateAntiForgeryHeader]
        //public ActionResult OrderWithdraw(string CustomerCode, string ProjectCode, int JobID, string UserName)
        //{
        //    var lErrorMsg = "";
        //    try
        //    {
        //        if (CustomerCode == null)
        //        {
        //            CustomerCode = "";
        //        }
        //        if (CustomerCode.Length > 0)
        //        {
        //            var lTemplate = false;
        //            var oldJobAdvice = db.BPCJobAdvice.Find(CustomerCode, ProjectCode, lTemplate, JobID);
        //            if (oldJobAdvice != null)
        //            {
        //                if (oldJobAdvice.OrderStatus.Trim() == "Submitted")
        //                {
        //                    var newJobAdvice = oldJobAdvice;
        //                    newJobAdvice.OrderStatus = "New";
        //                    newJobAdvice.UpdateDate = DateTime.Now;
        //                    newJobAdvice.UpdateBy = UserName;//"Vishalw_ttl@natsteel.com.sg";

        //                    db.Entry(oldJobAdvice).CurrentValues.SetValues(newJobAdvice);

        //                    var lDetOld = (from p in db.BPCDetails
        //                                   where p.CustomerCode == CustomerCode &&
        //                                   p.ProjectCode == ProjectCode &&
        //                                   p.JobID == JobID &&
        //                                   p.Template == lTemplate &&
        //                                   p.UpdateBy != UserName//User.Identity.Name
        //                                   select p).ToList();

        //                    if (lDetOld != null && lDetOld.Count > 0)
        //                    {
        //                        for (int i = 0; i < lDetOld.Count; i++)
        //                        {
        //                            var lNewDet = lDetOld[i];
        //                            lNewDet.UpdateBy = UserName;//"Vishalw_ttl@natsteel.com.sg";

        //                            db.Entry(lDetOld[i]).CurrentValues.SetValues(lNewDet);

        //                        }
        //                    }

        //                    db.SaveChanges();


        //                    var lEmailContent = "";
        //                    var lEmailFrom = "";
        //                    var lEmailTo = "";
        //                    var lEmailCc = "";
        //                    var lEmailSubject = "";
        //                    string lVar1 = "";

        //                    var JobContent = db.BPCJobAdvice.Find(CustomerCode, ProjectCode, lTemplate, JobID);
        //                    if (JobContent != null)
        //                    {
        //                        if (JobContent.CustomerCode == null) JobContent.CustomerCode = "";
        //                        else JobContent.CustomerCode = JobContent.CustomerCode.Trim();

        //                        if (JobContent.OrderStatus == null) JobContent.OrderStatus = "";
        //                        else JobContent.OrderStatus = JobContent.OrderStatus.Trim();

        //                        if (JobContent.PONumber == null) JobContent.PONumber = "";
        //                        else JobContent.PONumber = JobContent.PONumber.Trim();

        //                        if (JobContent.ProjectCode == null) JobContent.ProjectCode = "";
        //                        else JobContent.ProjectCode = JobContent.ProjectCode.Trim();

        //                        if (JobContent.DeliveryAddress == null) JobContent.DeliveryAddress = "";
        //                        else JobContent.DeliveryAddress = JobContent.DeliveryAddress.Trim();

        //                        if (JobContent.Remarks == null) JobContent.Remarks = "";
        //                        else JobContent.Remarks = JobContent.Remarks.Trim();

        //                        if (JobContent.Scheduler_HP == null) JobContent.Scheduler_HP = "";
        //                        else JobContent.Scheduler_HP = JobContent.Scheduler_HP.Trim();

        //                        if (JobContent.Scheduler_Name == null) JobContent.Scheduler_Name = "";
        //                        else JobContent.Scheduler_Name = JobContent.Scheduler_Name.Trim();

        //                        if (JobContent.Scheduler_Tel == null) JobContent.Scheduler_Tel = "";
        //                        else JobContent.Scheduler_Tel = JobContent.Scheduler_Tel.Trim();

        //                        if (JobContent.SiteEngr_HP == null) JobContent.SiteEngr_HP = "";
        //                        else JobContent.SiteEngr_HP = JobContent.SiteEngr_HP.Trim();

        //                        if (JobContent.SiteEngr_Name == null) JobContent.SiteEngr_Name = "";
        //                        else JobContent.SiteEngr_Name = JobContent.SiteEngr_Name.Trim();

        //                        if (JobContent.SiteEngr_Tel == null) JobContent.SiteEngr_Tel = "";
        //                        else JobContent.SiteEngr_Tel = JobContent.SiteEngr_Tel.Trim();
        //                    }

        //                    lEmailContent = "<p align='center'>CANCEL JOB ADVICE - Bored Pile Cage (撤消工作通知 - 钻孔桩铁笼)</p>";

        //                    lEmailContent = lEmailContent + "<table border='1' width=100%><tr>";
        //                    lEmailContent = lEmailContent + "<td width=20%>" + "Customer (客户名称)" + "</td>";

        //                    CustomerModels lCustomer = db.Customer.Find(JobContent.CustomerCode);
        //                    string lVar = "";
        //                    if (lCustomer != null) lVar = lCustomer.CustomerName.Trim() + " (" + JobContent.CustomerCode.Trim() + ")";
        //                    lEmailContent = lEmailContent + "<td>" + lVar + "</td></tr>";

        //                    lEmailContent = lEmailContent + "<tr><td>" + "Project (工程项目)" + "</td>";

        //                    var lProject = (from p in db.ProjectList
        //                                    where p.ProjectCode == ProjectCode
        //                                    select p).First();
        //                    lVar = "";
        //                    if (lProject != null) lVar = lProject.ProjectTitle.Trim() + " (" + JobContent.ProjectCode.Trim() + ")";
        //                    lEmailContent = lEmailContent + "<td>" + lVar + "</td></tr></table>";

        //                    lEmailContent = lEmailContent + "<table border='1' width=100%><tr>";

        //                    lEmailContent = lEmailContent + "<td width=20%>" + "PO No. (订单号码)" + "</td>";
        //                    lEmailContent = lEmailContent + "<td width=27%>" + JobContent.PONumber.Trim() + "</td>";
        //                    lEmailContent = lEmailContent + "<td width=26%>" + "Order Date (订单日期)" + "</td>";
        //                    lEmailContent = lEmailContent + "<td>" + String.Format("{0:yyyy-MM-dd}", JobContent.PODate) + "</td></tr>";

        //                    lEmailContent = lEmailContent + "</tr><td width=20%>" + "Required Date (交货日期)" + "</td>";
        //                    lEmailContent = lEmailContent + "<td width=27%>" + String.Format("{0:yyyy-MM-dd}", JobContent.RequiredDate) + "</td>";

        //                    lEmailContent = lEmailContent + "<td width=26%>" + "Transport Mode 运输工具)" + "</td>";

        //                    lVar = "";
        //                    var lProcessObj = new ProcessController();

        //                    var lCmd = new OracleCommand();
        //                    OracleDataReader lRst;
        //                    var lcisCon = new OracleConnection();

        //                    if (lProcessObj.OpenCISConnection(ref lcisCon) == true)
        //                    {

        //                        lCmd.CommandText = "SELECT BEZEI as DESCRIPTION " +
        //                        "FROM SAPSR3.TMFGT WHERE MANDT = '" + lProcessObj.strClient + "' AND SPRAS ='E' " +
        //                        "AND MFRGR = '" + JobContent.Transport + "' ";

        //                        lCmd.Connection = lcisCon;
        //                        lCmd.CommandTimeout = 300;
        //                        lRst = lCmd.ExecuteReader();
        //                        if (lRst.HasRows)
        //                        {
        //                            if (lRst.Read())
        //                            {
        //                                lVar = lRst.GetString(0).Trim();
        //                                lProcessObj.CloseCISConnection(ref lcisCon);
        //                            }
        //                        }
        //                        lRst.Close();
        //                    }
        //                    lCmd = null;
        //                    lcisCon = null;
        //                    lRst = null;
        //                    lProcessObj = null;

        //                    lEmailContent = lEmailContent + "<td>" + lVar + "</td></tr>";

        //                    lEmailContent = lEmailContent + "</tr><td width=20%>" + "Total Pieces (总件数)" + "</td>";
        //                    lEmailContent = lEmailContent + "<td width=27%>" + ((int)JobContent.TotalPcs).ToString() + "</td>";

        //                    lEmailContent = lEmailContent + "<td width=26%>" + "Total Weight (总重量)" + "</td>";

        //                    lEmailContent = lEmailContent + "<td>" + ((decimal)JobContent.TotalWeight).ToString("F3") + " KG" + "</td></tr></table>";

        //                    lEmailContent = lEmailContent + "<table border='1' width=100%><tr>";
        //                    lEmailContent = lEmailContent + "<td width=20%>" + "Delivery Address (送货地址)" + "</td>";

        //                    lVar = "&nbsp;";
        //                    if (JobContent.DeliveryAddress != null) lVar = JobContent.DeliveryAddress.Trim();
        //                    lEmailContent = lEmailContent + "<td>" + lVar + "</td></tr>";

        //                    lEmailContent = lEmailContent + "<tr><td>" + "Remarks (备注)" + "</td>";

        //                    lVar = "&nbsp;";
        //                    if (JobContent.Remarks != null) lVar = JobContent.Remarks.Trim();
        //                    lEmailContent = lEmailContent + "<td>" + lVar + "</td></tr></table>";

        //                    lEmailContent = lEmailContent + "<table border='1' width=100%><tr>";
        //                    lEmailContent = lEmailContent + "<td width='3%'>" + "S/N<br/>序号" + "</td>";
        //                    lEmailContent = lEmailContent + "<td width='6%'>" + "Pile Diameter<br/>桩直径" + "</td>";
        //                    lEmailContent = lEmailContent + "<td width='12%'>" + "Cage Type<br/>铁笼类型" + "</td>";
        //                    lEmailContent = lEmailContent + "<td width='9%'>" + "No of Main Bars<br/>主筋数量" + "</td>";
        //                    lEmailContent = lEmailContent + "<td width='7%'>" + "Main Bar Shape<br/>主筋图形" + "</td>";
        //                    lEmailContent = lEmailContent + "<td width='5%'>" + "Cage Length<br/>铁笼长" + "</td>";
        //                    lEmailContent = lEmailContent + "<td width='5%'>" + "Lap Length<br/>顶端重叠长" + "</td>";
        //                    lEmailContent = lEmailContent + "<td width='14%'>" + "Link Dia-Spacing-Length<br/>弧状链的直径,间距与长度" + "</td>";
        //                    lEmailContent = lEmailContent + "<td width='7%'>" + "End Length<br/>底端预留长度" + "</td>";
        //                    lEmailContent = lEmailContent + "<td width='5%'>" + "Cage Qty<br/>铁笼件数" + "</td>";
        //                    lEmailContent = lEmailContent + "<td width='11%'>" + "Combination of Cages<br/>上端, 中间还是低端铁笼" + "</td>";
        //                    lEmailContent = lEmailContent + "<td width='6%'>" + "Weight(MT)<br/>重量(吨)" + "</td>";
        //                    lEmailContent = lEmailContent + "<td width='5%'>" + "Per Set<br/>每组笼数" + "</td>";
        //                    lEmailContent = lEmailContent + "<td width='5%'>" + "Cage Mark<br/>铁笼号码" + "</td>";
        //                    lEmailContent = lEmailContent + "<td>" + "Remarks<br/>备注" + "</td>";
        //                    lEmailContent = lEmailContent + "</td></tr>";

        //                    var lBBSContent = (from p in db.BPCDetails
        //                                       where p.CustomerCode == CustomerCode &&
        //                                       p.ProjectCode == ProjectCode &&
        //                                       p.Template == false &&
        //                                       p.JobID == JobID
        //                                       orderby p.cage_id
        //                                       select p).ToList();

        //                    if (lBBSContent.Count > 0)
        //                    {
        //                        for (int i = 0; i < lBBSContent.Count; i++)
        //                        {
        //                            lEmailContent = lEmailContent + "<tr><td align='center'> <font color='blue'>" + (i + 1).ToString() + "</font></td>";
        //                            lEmailContent = lEmailContent + "<td align='center'><font color='blue'>" + lBBSContent[i].pile_dia.ToString() + "</font></td>";
        //                            //Cage Type
        //                            lVar = "";
        //                            var lPileType = lBBSContent[i].pile_type;
        //                            var lMainBarArrange = lBBSContent[i].main_bar_arrange;
        //                            var lMainBarType = lBBSContent[i].main_bar_type;
        //                            if (lPileType == "Single-Layer")
        //                            {
        //                                if (lMainBarType == "Single")
        //                                {
        //                                    if (lMainBarArrange == "Single")
        //                                    {
        //                                        lVar = "Single Layer";
        //                                    }
        //                                    else if (lMainBarArrange == "Side-By-Side")
        //                                    {
        //                                        lVar = "Single Layer<br/>Side-By-Side Bundled Bars";
        //                                    }
        //                                    else if (lMainBarArrange == "In-Out")
        //                                    {
        //                                        lVar = "Single Layer<br/>In-Out Bundled Bars";
        //                                    }
        //                                    else
        //                                    {
        //                                        lVar = "Single Layer<br/>Complex Bundled Bars";
        //                                    }
        //                                }
        //                                if (lMainBarType == "Mixed")
        //                                {
        //                                    if (lMainBarArrange == "Single")
        //                                    {
        //                                        lVar = "Single Layer<br/>Mixed Bars";
        //                                    }
        //                                    else if (lMainBarArrange == "Side-By-Side")
        //                                    {
        //                                        lVar = "Single Layer<br/>Side By Side Bundled<br/>Mixed Bars";
        //                                    }
        //                                    else if (lMainBarArrange == "In-Out")
        //                                    {
        //                                        lVar = "Single Layer<br/>In-Out Bundled<br/>Mixed Bars";
        //                                    }
        //                                    else
        //                                    {
        //                                        lVar = "Single Layer<br/>Complex Bundled<br/>Mixed Bars";
        //                                    }
        //                                }
        //                            }
        //                            else
        //                            {
        //                                if (lMainBarArrange == "Single")
        //                                {
        //                                    lVar = "Double Layer";
        //                                }
        //                                else if (lMainBarArrange == "Side-By-Side")
        //                                {
        //                                    lVar = "Double Layer<br/>Side By Side Bundled Bars";
        //                                }
        //                                else
        //                                {
        //                                    lVar = "Double Layer<br/>Complex Bundled Bars";
        //                                }

        //                            }

        //                            lEmailContent = lEmailContent + "<td align='center'><font color='blue'>" + lVar + "</font></td>";

        //                            lVar = "";
        //                            var lBarCTArr = lBBSContent[i].main_bar_ct.Split(',');
        //                            var lBarDiaArr = lBBSContent[i].main_bar_dia.Split(',');
        //                            var lBarType = lBBSContent[i].main_bar_grade.Trim();
        //                            if (lBarCTArr.Length > 0 && lBarDiaArr.Length > 0)
        //                            {
        //                                lVar = lBarCTArr[0].Trim() + lBarType + lBarDiaArr[0].Trim();
        //                            }
        //                            if (lBarCTArr.Length > 1 && lBarDiaArr.Length > 1)
        //                            {
        //                                lVar = lVar + "<br/>" + lBarCTArr[1].Trim() + lBarType + lBarDiaArr[1].Trim();
        //                            }

        //                            lEmailContent = lEmailContent + "<td align='center'><font color='blue'>" + lVar + "</font></td>";
        //                            lEmailContent = lEmailContent + "<td align='center'><font color='blue'>" + lBBSContent[i].main_bar_shape + "</font></td>";
        //                            lEmailContent = lEmailContent + "<td align='center'><font color='blue'>" + lBBSContent[i].cage_length.ToString() + "</font></td>";
        //                            lEmailContent = lEmailContent + "<td align='center'><font color='blue'>" + lBBSContent[i].lap_length.ToString() + "</font></td>";

        //                            lVar = "";
        //                            var lSLType = "";
        //                            if (lBBSContent[i].spiral_link_type.Length >= 5)
        //                            {
        //                                if (lBBSContent[i].spiral_link_type.Substring(0, 5) == "Others")
        //                                {
        //                                    lSLType = "";
        //                                }
        //                                else if (lBBSContent[i].spiral_link_type.Substring(0, 4) == "Twin")
        //                                {
        //                                    lSLType = "2";
        //                                }
        //                            }
        //                            var lSLSpacing = lBBSContent[i].spiral_link_spacing.Split(',');
        //                            var lSLGrade = lBBSContent[i].spiral_link_grade.Trim();
        //                            if (lSLSpacing.Length > 0 && lSLSpacing.Length > 0)
        //                            {
        //                                lVar = lSLType + lSLGrade + lBBSContent[i].sl1_dia + "-" + lSLSpacing[0] + "-" + lBBSContent[i].sl1_length;
        //                            }
        //                            if (lSLSpacing.Length > 1 && lSLSpacing.Length > 1)
        //                            {
        //                                if (lBBSContent[i].spiral_link_type == "Twin-Single")
        //                                {
        //                                    lVar = lVar + "<br/>" + "" + lSLGrade + lBBSContent[i].sl2_dia + "-" + lSLSpacing[1] + "-" + lBBSContent[i].sl2_length;
        //                                }
        //                                else
        //                                {
        //                                    lVar = lVar + "<br/>" + lSLType + lSLGrade + lBBSContent[i].sl2_dia + "-" + lSLSpacing[1] + "-" + lBBSContent[i].sl2_length;
        //                                }
        //                            }
        //                            if (lSLSpacing.Length > 2 && lSLSpacing.Length > 2)
        //                            {
        //                                lVar = lVar + "<br/>" + lSLType + lSLGrade + lBBSContent[i].sl3_dia + "-" + lSLSpacing[2] + "-" + lBBSContent[i].sl3_length;
        //                            }

        //                            lEmailContent = lEmailContent + "<td align='center'><font color='blue'>" + lVar + "</font></td>";
        //                            lEmailContent = lEmailContent + "<td align='right'><font color='blue'>" + lBBSContent[i].end_length.ToString() + "</font></td>";
        //                            lEmailContent = lEmailContent + "<td align='right'><font color='blue'>" + lBBSContent[i].cage_qty.ToString() + "</font></td>";
        //                            lEmailContent = lEmailContent + "<td align='right'><font color='blue'>" + lBBSContent[i].cage_location + "</font></td>";
        //                            lEmailContent = lEmailContent + "<td align='right'><font color='blue'>" + lBBSContent[i].cage_weight.ToString("F3") + "</font></td>";
        //                            lEmailContent = lEmailContent + "<td align='right'><font color='blue'>" + lBBSContent[i].per_set.ToString() + "</font></td>";
        //                            lEmailContent = lEmailContent + "<td align='right'><font color='blue'>" + lBBSContent[i].bbs_no + "</font></td>";
        //                            lEmailContent = lEmailContent + "<td align='right'><font color='blue'>" + lBBSContent[i].cage_remarks + "</font></td>";
        //                            lEmailContent = lEmailContent + "</tr>";
        //                        }
        //                    }
        //                    lEmailContent = lEmailContent + "</table>";

        //                    lEmailContent = lEmailContent + "<table border='1' width=100%><tr>";

        //                    lEmailContent = lEmailContent + "<td width='20%'>" + "Site Contact (联系人)" + "</td>";
        //                    lEmailContent = lEmailContent + "<td width='15%'>" + JobContent.SiteEngr_Name.Trim() + "</td>";
        //                    lEmailContent = lEmailContent + "<td width='13%'>" + "handphone (手机号码)" + " </td>";
        //                    lEmailContent = lEmailContent + "<td width='16%'>" + JobContent.SiteEngr_HP.Trim() + "</td>";
        //                    lEmailContent = lEmailContent + "<td width='13%'>" + "Email (电邮地址)" + "</td>";
        //                    lEmailContent = lEmailContent + "<td>" + JobContent.SiteEngr_Tel.Trim() + "</td></tr>";

        //                    lEmailContent = lEmailContent + "<tr><td width='20%'>" + "Goods Receiver (收货人)" + "</td>";
        //                    lEmailContent = lEmailContent + "<td width='15%'>" + JobContent.Scheduler_Name.Trim() + "</td>";
        //                    lEmailContent = lEmailContent + "<td width='13%'>" + "Handphone (手机号码)" + " </td>";
        //                    lEmailContent = lEmailContent + "<td width='16%'>" + JobContent.Scheduler_HP.Trim() + "</td>";
        //                    lEmailContent = lEmailContent + "<td width='13%'>" + "Email (电邮地址)" + "</td>";
        //                    lEmailContent = lEmailContent + "<td>" + JobContent.Scheduler_Tel.Trim() + "</td></tr></table>";

        //                    lEmailContent = lEmailContent + "<table border='1' width=100%><tr>";

        //                    lEmailContent = lEmailContent + "<td colspan='3'>" + "NatSteel Contacts (大众钢铁联系人) (Fax:62619133/62665153)" + "</td></tr>";

        //                    lEmailContent = lEmailContent + "<tr><td width='20%'>" + "Name (姓名)" + "</td>";
        //                    lEmailContent = lEmailContent + "<td width='15%'>" + "Contact Numbers (联系电话)" + "</td>";
        //                    lEmailContent = lEmailContent + "<td width='13%'>" + "Email Address (电邮地址)" + " </td></tr>";

        //                    var lProjContent = db.Project.Find(CustomerCode, ProjectCode);

        //                    if (lProjContent.Contact1 != null)
        //                    {
        //                        if (lProjContent.Contact1.Trim().Length > 0)
        //                        {
        //                            lEmailContent = lEmailContent + "<tr><td>" + lProjContent.Contact1.Trim() + "</td>";
        //                            lVar1 = "";
        //                            if (lProjContent.Tel1 != null) if (lProjContent.Tel1.Trim().Length > 0) lVar1 = lProjContent.Tel1.Trim();
        //                            lEmailContent = lEmailContent + "<td>" + lVar1 + "</td>";

        //                            lVar1 = "";
        //                            if (lProjContent.Email1 != null) if (lProjContent.Email1.Trim().Length > 0) lVar1 = lProjContent.Email1.Trim();
        //                            lEmailContent = lEmailContent + "<td>" + lVar1 + " </td></tr>";
        //                            if (lVar1 != "") if (lEmailTo == "") lEmailTo = lVar1; else lEmailTo = lEmailTo + ";" + lVar1;
        //                        }
        //                    }

        //                    if (lProjContent.Contact2 != null)
        //                    {
        //                        if (lProjContent.Contact2.Trim().Length > 0)
        //                        {
        //                            lEmailContent = lEmailContent + "<tr><td>" + lProjContent.Contact2.Trim() + "</td>";
        //                            lVar1 = "";
        //                            if (lProjContent.Tel2 != null) if (lProjContent.Tel2.Trim().Length > 0) lVar1 = lProjContent.Tel2.Trim();
        //                            lEmailContent = lEmailContent + "<td>" + lVar1 + "</td>";

        //                            lVar1 = "";
        //                            if (lProjContent.Email2 != null) if (lProjContent.Email2.Trim().Length > 0) lVar1 = lProjContent.Email2.Trim();
        //                            lEmailContent = lEmailContent + "<td>" + lVar1 + " </td></tr>";
        //                            if (lVar1 != "") if (lEmailTo == "") lEmailTo = lVar1; else lEmailTo = lEmailTo + ";" + lVar1;
        //                        }
        //                    }

        //                    if (lProjContent.Contact3 != null)
        //                    {
        //                        if (lProjContent.Contact3.Trim().Length > 0)
        //                        {
        //                            lEmailContent = lEmailContent + "<tr><td>" + lProjContent.Contact3.Trim() + "</td>";
        //                            lVar1 = "";
        //                            if (lProjContent.Tel3 != null) if (lProjContent.Tel3.Trim().Length > 0) lVar1 = lProjContent.Tel3.Trim();
        //                            lEmailContent = lEmailContent + "<td>" + lVar1 + "</td>";

        //                            lVar1 = "";
        //                            if (lProjContent.Email3 != null) if (lProjContent.Email3.Trim().Length > 0) lVar1 = lProjContent.Email3.Trim();
        //                            lEmailContent = lEmailContent + "<td>" + lVar1 + " </td></tr>";
        //                            if (lVar1 != "") if (lEmailTo == "") lEmailTo = lVar1; else lEmailTo = lEmailTo + ";" + lVar1;
        //                        }
        //                    }

        //                    if (lProjContent.Contact4 != null)
        //                    {
        //                        if (lProjContent.Contact4.Trim().Length > 0)
        //                        {
        //                            lEmailContent = lEmailContent + "<tr><td>" + lProjContent.Contact4.Trim() + "</td>";
        //                            lVar1 = "";
        //                            if (lProjContent.Tel4 != null) if (lProjContent.Tel4.Trim().Length > 0) lVar1 = lProjContent.Tel4.Trim();
        //                            lEmailContent = lEmailContent + "<td>" + lVar1 + "</td>";

        //                            lVar1 = "";
        //                            if (lProjContent.Email4 != null) if (lProjContent.Email4.Trim().Length > 0) lVar1 = lProjContent.Email4.Trim();
        //                            lEmailContent = lEmailContent + "<td>" + lVar1 + " </td></tr>";
        //                            if (lVar1 != "") if (lEmailTo == "") lEmailTo = lVar1; else lEmailTo = lEmailTo + ";" + lVar1;
        //                        }
        //                    }

        //                    if (lProjContent.Contact5 != null)
        //                    {
        //                        if (lProjContent.Contact5.Trim().Length > 0)
        //                        {
        //                            lEmailContent = lEmailContent + "<tr><td>" + lProjContent.Contact5.Trim() + "</td>";
        //                            lVar1 = "";
        //                            if (lProjContent.Tel5 != null) if (lProjContent.Tel5.Trim().Length > 0) lVar1 = lProjContent.Tel5.Trim();
        //                            lEmailContent = lEmailContent + "<td>" + lVar1 + "</td>";

        //                            lVar1 = "";
        //                            if (lProjContent.Email5 != null) if (lProjContent.Email5.Trim().Length > 0) lVar1 = lProjContent.Email5.Trim();
        //                            lEmailContent = lEmailContent + "<td>" + lVar1 + " </td></tr>";
        //                            if (lVar1 != "") if (lEmailTo == "") lEmailTo = lVar1; else lEmailTo = lEmailTo + ";" + lVar1;
        //                        }
        //                    }

        //                    if (lProjContent.Contact6 != null)
        //                    {
        //                        if (lProjContent.Contact6.Trim().Length > 0)
        //                        {
        //                            lEmailContent = lEmailContent + "<tr><td>" + lProjContent.Contact6.Trim() + "</td>";
        //                            lVar1 = "";
        //                            if (lProjContent.Tel6 != null) if (lProjContent.Tel6.Trim().Length > 0) lVar1 = lProjContent.Tel6.Trim();
        //                            lEmailContent = lEmailContent + "<td>" + lVar1 + "</td>";

        //                            lVar1 = "";
        //                            if (lProjContent.Email6 != null) if (lProjContent.Email6.Trim().Length > 0) lVar1 = lProjContent.Email6.Trim();
        //                            lEmailContent = lEmailContent + "<td>" + lVar1 + " </td></tr>";
        //                            if (lVar1 != "") if (lEmailTo == "") lEmailTo = lVar1; else lEmailTo = lEmailTo + ";" + lVar1;
        //                        }
        //                    }
        //                    lEmailContent = lEmailContent + "</table>";

        //                    lVar1 = JobContent.Scheduler_Tel.Trim();
        //                    if (lVar1 != "")
        //                    {
        //                        if (lEmailTo == "") { lEmailTo = lVar1; }
        //                        else { lEmailTo = lEmailTo + ";" + lVar1; }
        //                    }

        //                    lVar1 = JobContent.SiteEngr_Tel.Trim();
        //                    if (lVar1 != "" && lEmailTo.IndexOf(lVar1) < 0)
        //                    {
        //                        if (lEmailTo == "") { lEmailTo = lVar1; }
        //                        else { lEmailTo = lEmailTo + ";" + lVar1; }
        //                    }

        //                    if (JobContent.UpdateBy != null)
        //                    {
        //                        lVar1 = JobContent.UpdateBy.Trim();
        //                        if (lVar1 != "" && lEmailCc.IndexOf(lVar1) < 0)
        //                        {
        //                            if (lEmailCc == "") { lEmailCc = lVar1; }
        //                            else { lEmailCc = lEmailCc + ";" + lVar1; }
        //                        }
        //                    }

        //                    lVar = "";
        //                    if (lCustomer != null) lVar = lCustomer.CustomerName.Trim();

        //                    lEmailSubject = lVar + " - " + JobContent.PONumber.Trim() + " - Bored Pile Cage No. " + JobID.ToString();

        //                    var lOESEmail = new SendGridEmail();

        //                    string lEmailFromAddress = "eprompt@natsteel.com.sg";
        //                    string lEmailFromName = "Digital Ordering Email Services";

        //                    //dynamic response = lOESEmail.Execute(lEmailFromAddress, lEmailFromName, lEmailTo, lEmailCc, lEmailSubject, "text/html", lEmailContent);
        //                    lOESEmail.Execute(lEmailFromAddress, lEmailFromName, lEmailTo, lEmailCc, lEmailSubject, "text/html", lEmailContent).Wait();
        //                    lOESEmail = null;

        //                    return Json(true);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        lErrorMsg = ex.Message;
        //        return Json(false);
        //    }
        //    return Json(false);
        //}

        //get BPC List of NSH Library
        [HttpGet]
        [Route("/getLibraryData_bpc/{SearchType}/{SearchString}")]
        //[ValidateAntiForgeryHeader]getRebarData_bpc
        public ActionResult getLibraryData(string SearchType, string SearchString)
        {
            int lSearchPileDia = 0;
            string lSearchBarDia = "";
            string lSearchBarQty = "";

            int.TryParse(SearchString, out lSearchPileDia);

            lSearchBarDia = SearchString;
            if (lSearchBarDia == null) lSearchBarDia = "";
            lSearchBarDia = lSearchBarDia.Trim();

            lSearchBarQty = SearchString;
            if (lSearchBarQty == null) lSearchBarQty = "";
            lSearchBarQty = lSearchBarQty.Trim();

            if (lSearchPileDia == 0 && lSearchBarDia == "" && lSearchBarQty == "")
            {
                SearchType = "ALL";
            }
            var content = (from p in db.BPCDetails
                           join s in db.BPCJobAdvice
                           on new { a = p.CustomerCode, b = p.ProjectCode, c = p.Template, d = p.JobID } equals
                           new { a = s.CustomerCode, b = s.ProjectCode, c = s.Template, d = s.JobID }
                           where s.CustomerCode == "0000000000" &&
                           s.ProjectCode == "0000000000" &&
                           p.CustomerCode == "0000000000" &&
                           p.ProjectCode == "0000000000" &&
                           s.OrderStatus != "Deleted" &&
                           (p.pile_dia == lSearchPileDia ||
                           SearchType != "PILEDIA") &&
                           (p.main_bar_dia == lSearchBarDia ||
                           SearchType != "BARDIA") &&
                           (p.main_bar_ct == lSearchBarQty ||
                           SearchType != "BARQTY")
                           orderby s.PONumber, p.cage_id
                           select new
                           {
                               s.JobID,
                               s.PONumber,
                               s.cover_to_link,
                               p.CustomerCode,
                               p.ProjectCode,
                               p.Template,
                               p.cage_id,
                               p.pile_type,
                               p.pile_dia,
                               p.cage_dia,
                               p.main_bar_arrange,
                               p.main_bar_type,
                               p.main_bar_ct,
                               p.main_bar_shape,
                               p.main_bar_grade,
                               p.main_bar_dia,
                               p.main_bar_topjoin,
                               p.main_bar_endjoin,
                               p.cage_length,
                               p.spiral_link_type,
                               p.spiral_link_grade,
                               p.spiral_link_dia,
                               p.spiral_link_spacing,
                               p.lap_length,
                               p.end_length,
                               p.cage_location,
                               p.rings_start,
                               p.rings_end,
                               p.rings_addn_no,
                               p.rings_addn_member,
                               p.coupler_top,
                               p.coupler_end,
                               p.no_of_sr,
                               p.sr_grade,
                               p.sr_dia,
                               p.sr_dia_add,
                               p.sr1_location,
                               p.sr2_location,
                               p.sr3_location,
                               p.sr4_location,
                               p.sr5_location,
                               p.crank_height_top,
                               p.crank_height_end,
                               p.crank2_height_top,
                               p.crank2_height_end,
                               p.sl1_length,
                               p.sl2_length,
                               p.sl3_length,
                               p.sl1_dia,
                               p.sl2_dia,
                               p.sl3_dia,
                               p.total_sl_length,
                               p.no_of_cr_top,
                               p.cr_spacing_top,
                               p.no_of_cr_end,
                               p.cr_spacing_end,
                               p.cr_end_remarks,
                               p.extra_support_bar_ind,
                               p.extra_support_bar_dia,
                               p.extra_cr_no,
                               p.extra_cr_loc,
                               p.extra_cr_dia,
                               p.mainbar_length_2layer,
                               p.mainbar_location_2layer,
                               p.per_set,
                               p.bbs_no,
                               p.cage_qty,
                               p.cage_weight,
                               p.cage_remarks,
                               p.set_code
                           }
                           ).ToList();

            return Ok(content);
        }

        //get BPC List of NSH Library
        [HttpGet]
        [Route("/getTemplateData_bpc/{CustomerCode}/{ProjectCode}/{PileDia}/{BarDia}/{BarQty}")]
        //[ValidateAntiForgeryHeader]
        public ActionResult getTemplateData(string CustomerCode, string ProjectCode, string PileDia, string BarDia, string BarQty)
        {
            int lSearchPileDia = 0;
            string lSearchBarDia = "";
            string lSearchBarQty = "";

            if (PileDia == null || PileDia == "null")
            {
                PileDia = "";
            }
            PileDia = PileDia.Trim();

            if (BarDia == null || BarDia == "null")
            {
                BarDia = "";
            }
            if (BarQty == null || BarQty == "null")
            {
                BarQty = "";
            }
            int.TryParse(PileDia, out lSearchPileDia);

            lSearchBarDia = BarDia;
            lSearchBarDia = lSearchBarDia.Trim();

            lSearchBarQty = BarQty;
            lSearchBarQty = lSearchBarQty.Trim();


            if (CustomerCode != null && ProjectCode != null && CustomerCode != "" && ProjectCode != "")
            {
                var content = (from p in db.BPCDetails
                               join s in db.BPCJobAdvice
                               on new { a = p.CustomerCode, b = p.ProjectCode, c = p.Template, d = p.JobID } equals
                               new { a = s.CustomerCode, b = s.ProjectCode, c = s.Template, d = s.JobID }
                               where s.CustomerCode == CustomerCode &&
                               s.ProjectCode == ProjectCode &&
                               p.CustomerCode == CustomerCode &&
                               p.ProjectCode == ProjectCode &&
                               s.OrderStatus != "Deleted" &&
                               p.Template == true &&
                               (p.pile_dia == lSearchPileDia ||
                               PileDia == "") &&
                               (p.main_bar_dia == lSearchBarDia ||
                               lSearchBarDia == "") &&
                               (p.main_bar_ct == lSearchBarQty ||
                               lSearchBarQty == "")
                               orderby p.pile_dia, p.set_code, p.cage_id
                               select new
                               {
                                   s.JobID,
                                   s.PONumber,
                                   s.cover_to_link,
                                   p.CustomerCode,
                                   p.ProjectCode,
                                   p.Template,
                                   p.cage_id,
                                   p.pile_type,
                                   p.pile_dia,
                                   p.cage_dia,
                                   p.main_bar_arrange,
                                   p.main_bar_type,
                                   p.main_bar_ct,
                                   p.main_bar_shape,
                                   p.main_bar_grade,
                                   p.main_bar_dia,
                                   p.main_bar_topjoin,
                                   p.main_bar_endjoin,
                                   p.cage_length,
                                   p.spiral_link_type,
                                   p.spiral_link_grade,
                                   p.spiral_link_dia,
                                   p.spiral_link_spacing,
                                   p.lap_length,
                                   p.end_length,
                                   p.cage_location,
                                   p.rings_start,
                                   p.rings_end,
                                   p.rings_addn_no,
                                   p.rings_addn_member,
                                   p.coupler_top,
                                   p.coupler_end,
                                   p.no_of_sr,
                                   p.sr_grade,
                                   p.sr_dia,
                                   p.sr_dia_add,
                                   p.sr1_location,
                                   p.sr2_location,
                                   p.sr3_location,
                                   p.sr4_location,
                                   p.sr5_location,
                                   p.crank_height_top,
                                   p.crank_height_end,
                                   p.crank2_height_top,
                                   p.crank2_height_end,
                                   p.sl1_length,
                                   p.sl2_length,
                                   p.sl3_length,
                                   p.sl1_dia,
                                   p.sl2_dia,
                                   p.sl3_dia,
                                   p.total_sl_length,
                                   p.no_of_cr_top,
                                   p.cr_spacing_top,
                                   p.no_of_cr_end,
                                   p.cr_spacing_end,

                                   p.cr_end_remarks,
                                   p.extra_support_bar_ind,
                                   p.extra_support_bar_dia,
                                   p.extra_cr_no,
                                   p.extra_cr_loc,
                                   p.extra_cr_dia,
                                   p.mainbar_length_2layer,
                                   p.mainbar_location_2layer,
                                   p.per_set,
                                   p.bbs_no,
                                   p.cage_qty,
                                   p.cage_weight,
                                   p.cage_remarks,
                                   p.set_code,
                                   p.pdf_remark,
                                   p.lminTop,
                                   p.lminEnd,
                                   p.vchCustomizeBarsJSON,
                                   p.cr_ring_type,
                                   p.cr_bundle_side,
                                   p.mainbar_position_2layer,
                                   p.BPC_Type,
                                   p.cr_link_lapping,
                                   p.sr_link_lapping,
                                   p.twopcs_stiffener

                               }
                        ).ToList();
                return Ok(content);
            }

            return Ok("");
        }

        //get BBS List at Order Summary
        [HttpGet]
        [Route("/getBBS_bpc/{CustomerCode}/{ProjectCode}/{Template}/{JobID}")]
        //[ValidateAntiForgeryHeader]
        public ActionResult getBBS(string CustomerCode, string ProjectCode, bool Template, int JobID)
        {

            var content = (from p in db.BPCDetails
                           where p.CustomerCode == CustomerCode &&
                           p.ProjectCode == ProjectCode &&
                           p.Template == Template &&
                           p.JobID == JobID
                           orderby p.cage_id
                           select p).ToList();

            return Ok(content);
        }

        [HttpGet]
        [Route("/getCopyOrderList_bpc/{CustomerCode}/{ProjectCode}/{CopyModel}")]
        //[ValidateAntiForgeryHeader]
        public ActionResult getCopyOrderList(string CustomerCode, string ProjectCode, string CopyModel)
        {
            var content1 = new List<struOrderList>();
            if (CopyModel == "All")
            {
                var content = (from p in db.BPCJobAdvice
                               where p.CustomerCode == CustomerCode &&
                               p.ProjectCode == ProjectCode &&
                               p.Template == false
                               orderby p.JobID descending
                               select new
                               {
                                   JobID = p.JobID,
                                   PONumber = p.PONumber,
                                   RequiredDate = p.RequiredDate,
                                   OrderStatus = p.OrderStatus,
                               }
                               ).ToList();

                content1 = new List<struOrderList>(content.Select(h => new struOrderList
                {
                    OrderNo = h.JobID.ToString(),
                    OrderDesc = h.JobID.ToString() + " PO:" + (h.PONumber == null ? "" : h.PONumber.Trim()) + " RD:"
                    + (h.RequiredDate == null ? "" : h.RequiredDate.ToString("yyyy-MM-dd"))
                    + " Status:" + (h.OrderStatus == null ? "" : h.OrderStatus.Trim())
                }));
            }
            else if (CopyModel == "Order History")
            {
                var content = (from p in db.BPCJobAdvice
                               where p.CustomerCode == CustomerCode &&
                               p.ProjectCode == ProjectCode &&
                               p.Template == false &&
                               p.PONumber != null &&
                               p.PONumber != ""
                               orderby p.JobID descending
                               select new
                               {
                                   JobID = p.JobID,
                                   PONumber = p.PONumber,
                                   RequiredDate = p.RequiredDate,
                                   OrderStatus = p.OrderStatus,
                               }
                               ).ToList();

                content1 = new List<struOrderList>(content.Select(h => new struOrderList
                {
                    OrderNo = h.JobID.ToString(),
                    OrderDesc = h.JobID.ToString() + " PO:" + (h.PONumber == null ? "" : h.PONumber.Trim()) + " RD:"
                    + (h.RequiredDate == null ? "" : h.RequiredDate.ToString("yyyy-MM-dd"))
                    + " Status:" + (h.OrderStatus == null ? "" : h.OrderStatus.Trim())
                }));

            }
            else if (CopyModel == "Customer Template")
            {
                var content = (from p in db.BPCJobAdvice
                               where p.CustomerCode == CustomerCode &&
                               p.ProjectCode == ProjectCode &&
                               p.Template == true &&
                               p.PONumber != null &&
                               p.PONumber != ""
                               orderby p.JobID descending
                               select new
                               {
                                   JobID = p.JobID,
                                   PONumber = p.PONumber,
                                   RequiredDate = p.RequiredDate,
                                   OrderStatus = p.OrderStatus,
                               }
                               ).ToList();

                content1 = new List<struOrderList>(content.Select(h => new struOrderList
                {
                    OrderNo = h.JobID.ToString(),
                    OrderDesc = h.PONumber == null ? "" : h.PONumber.Trim()
                }));
            }
            else if (CopyModel == "COMMON BORED PILE CAGE")
            {
                var content = (from p in db.BPCJobAdvice
                               where p.CustomerCode == "0000000000" &&
                               p.ProjectCode == "0000000000" &&
                               p.PONumber != null &&
                               p.PONumber != ""
                               orderby p.JobID descending
                               select new
                               {
                                   JobID = p.JobID,
                                   PONumber = p.PONumber,
                                   RequiredDate = p.RequiredDate,
                                   OrderStatus = p.OrderStatus,
                               }
                               ).ToList();

                content1 = new List<struOrderList>(content.Select(h => new struOrderList
                {
                    OrderNo = h.JobID.ToString(),
                    OrderDesc = h.JobID.ToString() + " - " + (h.PONumber == null ? "" : h.PONumber.Trim())
                }));
            }

            return Ok(content1);
        }

        [HttpGet]
        [Route("/getCopyOrderDetails_bpc/{CustomerCode}/{ProjectCode}/CopyModel/{JobID}")]
        //[ValidateAntiForgeryHeader]
        public ActionResult getCopyOrderDetails(string CustomerCode, string ProjectCode, string CopyModel, int JobID)
        {
            bool lTemplate = false;
            if (CopyModel == "Customer Template")
            {
                lTemplate = true;
            }

            var content1 = new List<BPCDetailsModels>();
            if (CopyModel == "COMMON BORED PILE CAGE")
            {
                content1 = (from p in db.BPCDetails
                            where p.CustomerCode == "0000000000" &&
                            p.ProjectCode == "0000000000" &&
                            p.JobID == JobID
                            orderby p.cage_id
                            select p
                            ).ToList();

            }
            else
            {
                content1 = (from p in db.BPCDetails
                            where p.CustomerCode == CustomerCode &&
                            p.ProjectCode == ProjectCode &&
                            p.Template == lTemplate &&
                            p.JobID == JobID
                            orderby p.cage_id
                            select p
                            ).ToList();

            }

            return Ok(content1);
        }

        [HttpGet]
        [Route("/setCloneOrder_bpc/{CustomerCode}/{ProjectCode}/{JobID}/{Template}/{CloneNo}/{UserName}")]
        //[ValidateAntiForgeryHeader]
        public ActionResult setCloneOrder(string CustomerCode, string ProjectCode, int JobID, bool Template, int CloneNo, string UserName)
        {
            var lErrorMsg = "";
            int? lJobID = 0;
            try
            {
                if (CloneNo > 0)
                {
                    for (int i = 0; i < CloneNo; i++)
                    {
                        createJobAdvice(CustomerCode, ProjectCode, Template, true, UserName);

                        lJobID = db.BPCJobAdvice.Where(z => z.CustomerCode == CustomerCode &&
                        z.ProjectCode == ProjectCode && z.Template == Template).Max(z => (int?)z.JobID);

                        //create JobAdvice
                        var lJobNewOr = db.BPCJobAdvice.Find(CustomerCode, ProjectCode, Template, lJobID);
                        var lJobNew = lJobNewOr;
                        var lJobOld = db.BPCJobAdvice.Find(CustomerCode, ProjectCode, Template, JobID);
                        if (lJobOld != null && lJobNew != null)
                        {
                            lJobNew.cover_to_link = lJobOld.cover_to_link;
                            lJobNew.OrderSource = lJobOld.OrderSource;
                            lJobNew.pile_category = lJobOld.pile_category;
                            lJobNew.redesign_req = lJobOld.redesign_req;
                            lJobNew.TotalPcs = lJobOld.TotalPcs;
                            lJobNew.Transport = lJobOld.Transport;
                            lJobNew.underload_ct = lJobOld.underload_ct;

                            lJobNew.OrderStatus = "New";
                            //lJobNew.PODate = lJobOld.PODate;
                            lJobNew.PODate = DateTime.Now;
                            lJobNew.PONumber = lJobOld.PONumber;
                            lJobNew.DeliveryAddress = lJobOld.DeliveryAddress;
                            lJobNew.Remarks = lJobOld.Remarks;
                            lJobNew.RequiredDate = lJobOld.RequiredDate;
                            lJobNew.Scheduler_HP = lJobOld.Scheduler_HP;
                            lJobNew.Scheduler_Name = lJobOld.Scheduler_Name;
                            lJobNew.Scheduler_Tel = lJobOld.Scheduler_Tel;
                            lJobNew.SiteEngr_HP = lJobOld.SiteEngr_HP;
                            lJobNew.SiteEngr_Name = lJobOld.SiteEngr_Name;
                            lJobNew.SiteEngr_Tel = lJobOld.SiteEngr_Tel;
                            lJobNew.OrderSource = lJobOld.OrderSource;
                            lJobNew.TotalWeight = lJobOld.TotalWeight;
                            lJobNew.UpdateBy = UserName;//"Vishalw_ttl@natsteel.com.sg";
                            lJobNew.UpdateDate = DateTime.Now;
                            db.Entry(lJobNewOr).CurrentValues.SetValues(lJobNew);
                        }

                        //Clone BPC Details

                        var lBBSDel = (from p in db.BPCDetails
                                       where p.CustomerCode == CustomerCode &&
                                       p.ProjectCode == ProjectCode &&
                                       p.Template == Template &&
                                       p.JobID == lJobID
                                       select p).ToList();
                        if (lBBSDel != null && lBBSDel.Count > 0)
                        {
                            db.BPCDetails.RemoveRange(lBBSDel);
                        }

                        var lBBSOld = (from p in db.BPCDetails
                                       where p.CustomerCode == CustomerCode &&
                                       p.ProjectCode == ProjectCode &&
                                       p.Template == Template &&
                                       p.JobID == JobID
                                       select p).ToList();
                        if (lBBSOld != null && lBBSOld.Count > 0)
                        {
                            var lBBSNew = new List<BPCDetailsModels>();
                            for (int j = 0; j < lBBSOld.Count; j++)
                            {

                                lBBSNew.Add(new BPCDetailsModels
                                {
                                    CustomerCode = lBBSOld[j].CustomerCode,
                                    ProjectCode = lBBSOld[j].ProjectCode,
                                    Template = lBBSOld[j].Template,
                                    JobID = (int)lJobID,
                                    cage_id = lBBSOld[j].cage_id,
                                    pile_type = lBBSOld[j].pile_type,
                                    pile_dia = lBBSOld[j].pile_dia,
                                    cage_dia = lBBSOld[j].cage_dia,
                                    main_bar_arrange = lBBSOld[j].main_bar_arrange,
                                    main_bar_type = lBBSOld[j].main_bar_type,
                                    main_bar_ct = lBBSOld[j].main_bar_ct,
                                    main_bar_shape = lBBSOld[j].main_bar_shape,
                                    main_bar_grade = lBBSOld[j].main_bar_grade,
                                    main_bar_dia = lBBSOld[j].main_bar_dia,
                                    main_bar_topjoin = lBBSOld[j].main_bar_topjoin,
                                    main_bar_endjoin = lBBSOld[j].main_bar_endjoin,
                                    cage_length = lBBSOld[j].cage_length,
                                    spiral_link_type = lBBSOld[j].spiral_link_type,
                                    spiral_link_grade = lBBSOld[j].spiral_link_grade,
                                    spiral_link_dia = lBBSOld[j].spiral_link_dia,
                                    spiral_link_spacing = lBBSOld[j].spiral_link_spacing,
                                    lap_length = lBBSOld[j].lap_length,
                                    end_length = lBBSOld[j].end_length,
                                    cage_location = lBBSOld[j].cage_location,
                                    rings_start = lBBSOld[j].rings_start,
                                    rings_end = lBBSOld[j].rings_end,
                                    rings_addn_no = lBBSOld[j].rings_addn_no,
                                    rings_addn_member = lBBSOld[j].rings_addn_member,
                                    coupler_top = lBBSOld[j].coupler_top,
                                    coupler_end = lBBSOld[j].coupler_end,
                                    no_of_sr = lBBSOld[j].no_of_sr,
                                    sr_grade = lBBSOld[j].sr_grade,
                                    sr_dia = lBBSOld[j].sr_dia,
                                    sr_dia_add = lBBSOld[j].sr_dia_add,
                                    sr1_location = lBBSOld[j].sr1_location,
                                    sr2_location = lBBSOld[j].sr2_location,
                                    sr3_location = lBBSOld[j].sr3_location,
                                    sr4_location = lBBSOld[j].sr4_location,
                                    sr5_location = lBBSOld[j].sr5_location,
                                    crank_height_top = lBBSOld[j].crank_height_top,
                                    crank_height_end = lBBSOld[j].crank_height_end,
                                    crank2_height_top = lBBSOld[j].crank2_height_top,
                                    crank2_height_end = lBBSOld[j].crank2_height_end,
                                    sl1_length = lBBSOld[j].sl1_length,
                                    sl2_length = lBBSOld[j].sl2_length,
                                    sl3_length = lBBSOld[j].sl3_length,
                                    sl1_dia = lBBSOld[j].sl1_dia,
                                    sl2_dia = lBBSOld[j].sl2_dia,
                                    sl3_dia = lBBSOld[j].sl3_dia,
                                    total_sl_length = lBBSOld[j].total_sl_length,
                                    no_of_cr_top = lBBSOld[j].no_of_cr_top,
                                    cr_spacing_top = lBBSOld[j].cr_spacing_top,
                                    no_of_cr_end = lBBSOld[j].no_of_cr_end,
                                    cr_spacing_end = lBBSOld[j].cr_spacing_end,
                                    cr_end_remarks = lBBSOld[j].cr_end_remarks,
                                    extra_support_bar_ind = lBBSOld[j].extra_support_bar_ind,
                                    extra_support_bar_dia = lBBSOld[j].extra_support_bar_dia,
                                    extra_cr_no = lBBSOld[j].extra_cr_no,
                                    extra_cr_loc = lBBSOld[j].extra_cr_loc,
                                    extra_cr_dia = lBBSOld[j].extra_cr_dia,
                                    mainbar_length_2layer = lBBSOld[j].mainbar_length_2layer,
                                    mainbar_location_2layer = lBBSOld[j].mainbar_location_2layer,

                                    per_set = lBBSOld[j].per_set,
                                    bbs_no = lBBSOld[j].bbs_no,
                                    cage_qty = lBBSOld[j].cage_qty,
                                    cage_weight = lBBSOld[j].cage_weight,
                                    cage_remarks = lBBSOld[j].cage_remarks,
                                    set_code = lBBSOld[j].set_code,
                                    nds_groupmarking = lBBSOld[j].nds_groupmarking,
                                    nds_wbs1 = lBBSOld[j].nds_wbs1,
                                    nds_wbs2 = lBBSOld[j].nds_wbs2,
                                    nds_wbs3 = lBBSOld[j].nds_wbs3,
                                    sor_no = lBBSOld[j].sor_no,
                                    sap_mcode = lBBSOld[j].sap_mcode,
                                    copyfrom_project = lBBSOld[j].copyfrom_project,
                                    copyfrom_template = lBBSOld[j].copyfrom_template,
                                    copyfrom_jobid = lBBSOld[j].copyfrom_jobid,
                                    copyfrom_ponumber = lBBSOld[j].copyfrom_ponumber,
                                    sr_link_lapping = lBBSOld[j].sr_link_lapping ?? 10,
                                    cr_link_lapping = lBBSOld[j].cr_link_lapping ?? 51,
                                    UpdateDate = DateTime.Now,
                                    UpdateBy = UserName//"Vishalw_ttl@natsteel.com.sg"
                                });
                            }
                            db.BPCDetails.AddRange(lBBSNew);
                        }

                        //Clone BPC Template

                        var lTempDel = (from p in db.BPCTemplate
                                        where p.CustomerCode == CustomerCode &&
                                        p.ProjectCode == ProjectCode &&
                                        p.Template == Template &&
                                        p.JobID == lJobID
                                        select p).ToList();
                        if (lTempDel != null && lTempDel.Count > 0)
                        {
                            db.BPCTemplate.RemoveRange(lTempDel);
                        }

                        var lTempOld = (from p in db.BPCTemplate
                                        where p.CustomerCode == CustomerCode &&
                                       p.ProjectCode == ProjectCode &&
                                       p.Template == Template &&
                                       p.JobID == JobID
                                        select p).ToList();
                        if (lTempOld != null && lTempOld.Count > 0)
                        {
                            var lTempNew = new List<BPCTemplateModels>();
                            for (int j = 0; j < lTempOld.Count; j++)
                            {

                                lTempNew.Add(new BPCTemplateModels
                                {
                                    CustomerCode = lTempOld[j].CustomerCode,
                                    ProjectCode = lTempOld[j].ProjectCode,
                                    Template = lTempOld[j].Template,
                                    JobID = (int)lJobID,
                                    CageID = lTempOld[j].CageID,
                                    TemplateID = lTempOld[j].TemplateID,
                                    template_code = lTempOld[j].template_code,
                                    pile_dia = lTempOld[j].pile_dia,
                                    cage_dia = lTempOld[j].cage_dia,
                                    no_of_holes = lTempOld[j].no_of_holes,
                                    cover = lTempOld[j].cover,
                                    bundled = lTempOld[j].bundled,
                                    UpdateDate = DateTime.Now,
                                    UpdatedBy = UserName//"Vishalw_ttl@natsteel.com.sg"
                                });
                            }
                            db.BPCTemplate.AddRange(lTempNew);
                        }


                        //Clone BPC Bars

                        var lBarsDel = (from p in db.BPCCageBars
                                        where p.CustomerCode == CustomerCode &&
                                        p.ProjectCode == ProjectCode &&
                                        p.Template == Template &&
                                        p.JobID == lJobID
                                        select p).ToList();
                        if (lBarsDel != null && lBarsDel.Count > 0)
                        {
                            db.BPCCageBars.RemoveRange(lBarsDel);
                        }

                        var lBarsOld = (from p in db.BPCCageBars
                                        where p.CustomerCode == CustomerCode &&
                                        p.ProjectCode == ProjectCode &&
                                        p.Template == Template &&
                                        p.JobID == JobID
                                        select p).ToList();
                        if (lBarsOld != null && lBarsOld.Count > 0)
                        {
                            var lBarsNew = new List<BPCCageBarsModels>();
                            for (int j = 0; j < lBarsOld.Count; j++)
                            {
                                lBarsNew.Add(new BPCCageBarsModels
                                {
                                    CustomerCode = lBarsOld[j].CustomerCode,
                                    ProjectCode = lBarsOld[j].ProjectCode,
                                    Template = lBarsOld[j].Template,
                                    JobID = (int)lJobID,
                                    CageID = lBarsOld[j].CageID,
                                    BarID = lBarsOld[j].BarID,
                                    BarSort = lBarsOld[j].BarSort,
                                    ElementMark = lBarsOld[j].ElementMark,
                                    BarMark = lBarsOld[j].BarMark,
                                    BarType = lBarsOld[j].BarType,
                                    BarSize = lBarsOld[j].BarSize,
                                    BarMemberQty = lBarsOld[j].BarMemberQty,
                                    BarEachQty = lBarsOld[j].BarEachQty,
                                    BarTotalQty = lBarsOld[j].BarTotalQty,
                                    BarShapeCode = lBarsOld[j].BarShapeCode,
                                    A = lBarsOld[j].A,
                                    B = lBarsOld[j].B,
                                    C = lBarsOld[j].C,
                                    D = lBarsOld[j].D,
                                    E = lBarsOld[j].E,
                                    F = lBarsOld[j].F,
                                    G = lBarsOld[j].G,
                                    H = lBarsOld[j].H,
                                    I = lBarsOld[j].I,
                                    J = lBarsOld[j].J,
                                    K = lBarsOld[j].K,
                                    L = lBarsOld[j].L,
                                    M = lBarsOld[j].M,
                                    N = lBarsOld[j].N,
                                    O = lBarsOld[j].O,
                                    P = lBarsOld[j].P,
                                    Q = lBarsOld[j].Q,
                                    R = lBarsOld[j].R,
                                    S = lBarsOld[j].S,
                                    T = lBarsOld[j].T,
                                    U = lBarsOld[j].U,
                                    V = lBarsOld[j].V,
                                    W = lBarsOld[j].W,
                                    X = lBarsOld[j].X,
                                    Y = lBarsOld[j].Y,
                                    Z = lBarsOld[j].Z,
                                    BarLength = lBarsOld[j].BarLength,
                                    BarWeight = lBarsOld[j].BarWeight,
                                    Remarks = lBarsOld[j].Remarks,
                                    PinSize = lBarsOld[j].PinSize,
                                    UpdateDate = DateTime.Now,
                                    isCABEdit = lBarsOld[j].isCABEdit
                                });
                            }
                            db.BPCCageBars.AddRange(lBarsNew);
                        }
                        db.SaveChanges();
                    }
                }
                //return Json(new { success = true, responseText = "Successfully saved." }, JsonRequestBehavior.AllowGet);\
                return Ok();
            }
            catch (Exception ex)
            {
                lErrorMsg = ex.Message;
            }
            //return Json(new { success = false, responseText = lErrorMsg }, JsonRequestBehavior.AllowGet);
            return Ok();
        }


        // POST: Customer/copyBBS/
        [HttpGet]
        [Route("/copyBPCLibrary_bpc/{SourceCustomerCode}/{SourceProjectCode}/{SourceJobIDs}/{DesCustomerCode}/{DesProjectCode}/{UserName}")]
        //[ValidateAntiForgeryHeader]
        public ActionResult copyBPCLibrary(string SourceCustomerCode,
                string SourceProjectCode,
                string SourceJobIDs,
                string DesCustomerCode,
                string DesProjectCode,
                string UserName)
        {
            var lTemplate = true;

            var lJobIDList = SourceJobIDs.Split(',').ToList();

            try
            {
                if (lJobIDList.Count > 0)
                {
                    var lNewJobID = (from p in db.BPCJobAdvice
                                     where p.CustomerCode == DesCustomerCode &&
                                     p.ProjectCode == DesProjectCode &&
                                     p.Template == lTemplate
                                     select p.JobID).DefaultIfEmpty(0).Max();

                    for (int i = 0; i < lJobIDList.Count; i++)
                    {
                        int lJobIDSource = 0;
                        int.TryParse(lJobIDList[i], out lJobIDSource);
                        if (lJobIDSource > 0)
                        {
                            var lJobSource = db.BPCJobAdvice.Find(SourceCustomerCode, SourceProjectCode, lTemplate, lJobIDSource);
                            if (lJobSource != null)
                            {
                                var lBPCDetails = db.BPCDetails.Find(SourceCustomerCode, SourceProjectCode, lTemplate, lJobIDSource, 1);
                                if (lBPCDetails != null)
                                {
                                    // start copy

                                    lNewJobID = lNewJobID + 1;

                                    var lJobDesUpdate = new BPCJobAdviceModels();
                                    lJobDesUpdate.CustomerCode = DesCustomerCode;
                                    lJobDesUpdate.ProjectCode = DesProjectCode;
                                    lJobDesUpdate.Template = true;
                                    lJobDesUpdate.JobID = lNewJobID;
                                    lJobDesUpdate.PONumber = lJobSource.PONumber;
                                    lJobDesUpdate.PODate = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                                    lJobDesUpdate.RequiredDate = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                                    lJobDesUpdate.cover_to_link = lJobSource.cover_to_link;
                                    lJobDesUpdate.UpdateDate = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                                    lJobDesUpdate.UpdateBy = UserName;//"Vishalw_ttl@natsteel.com.sg";
                                    db.BPCJobAdvice.Add(lJobDesUpdate);

                                    var lBPCDesUpdate = new BPCDetailsModels();

                                    lBPCDesUpdate.CustomerCode = DesCustomerCode;
                                    lBPCDesUpdate.ProjectCode = DesProjectCode;
                                    lBPCDesUpdate.Template = true;
                                    lBPCDesUpdate.JobID = lNewJobID;
                                    lBPCDesUpdate.cage_id = 1;
                                    lBPCDesUpdate.copyfrom_project = SourceProjectCode;
                                    lBPCDesUpdate.copyfrom_template = lBPCDetails.Template;
                                    lBPCDesUpdate.copyfrom_jobid = lJobIDSource;
                                    lBPCDesUpdate.copyfrom_ponumber = lJobSource.PONumber;
                                    lBPCDesUpdate.UpdateDate = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                                    lBPCDesUpdate.UpdateBy = UserName;//"Vishalw_ttl@natsteel.com.sg";

                                    lBPCDesUpdate.pile_type = lBPCDetails.pile_type;
                                    lBPCDesUpdate.pile_dia = lBPCDetails.pile_dia;
                                    lBPCDesUpdate.cage_dia = lBPCDetails.cage_dia;
                                    lBPCDesUpdate.main_bar_arrange = lBPCDetails.main_bar_arrange;
                                    lBPCDesUpdate.main_bar_type = lBPCDetails.main_bar_type;
                                    lBPCDesUpdate.main_bar_ct = lBPCDetails.main_bar_ct;
                                    lBPCDesUpdate.main_bar_shape = lBPCDetails.main_bar_shape;
                                    lBPCDesUpdate.main_bar_grade = lBPCDetails.main_bar_grade;
                                    lBPCDesUpdate.main_bar_dia = lBPCDetails.main_bar_dia;
                                    lBPCDesUpdate.main_bar_topjoin = lBPCDetails.main_bar_topjoin;
                                    lBPCDesUpdate.main_bar_endjoin = lBPCDetails.main_bar_endjoin;
                                    lBPCDesUpdate.cage_length = lBPCDetails.cage_length;
                                    lBPCDesUpdate.spiral_link_type = lBPCDetails.spiral_link_type;
                                    lBPCDesUpdate.spiral_link_grade = lBPCDetails.spiral_link_grade;
                                    lBPCDesUpdate.spiral_link_dia = lBPCDetails.spiral_link_dia;
                                    lBPCDesUpdate.spiral_link_spacing = lBPCDetails.spiral_link_spacing;
                                    lBPCDesUpdate.lap_length = lBPCDetails.lap_length;
                                    lBPCDesUpdate.end_length = lBPCDetails.end_length;
                                    lBPCDesUpdate.cage_location = lBPCDetails.cage_location;

                                    lBPCDesUpdate.rings_start = lBPCDetails.rings_start;
                                    lBPCDesUpdate.rings_end = lBPCDetails.rings_end;
                                    lBPCDesUpdate.rings_addn_no = lBPCDetails.rings_addn_no;
                                    lBPCDesUpdate.rings_addn_member = lBPCDetails.rings_addn_member;
                                    lBPCDesUpdate.coupler_top = lBPCDetails.coupler_top;
                                    lBPCDesUpdate.coupler_end = lBPCDetails.coupler_end;
                                    lBPCDesUpdate.no_of_sr = lBPCDetails.no_of_sr;
                                    lBPCDesUpdate.sr_grade = lBPCDetails.sr_grade;
                                    lBPCDesUpdate.sr_dia = lBPCDetails.sr_dia;
                                    lBPCDesUpdate.sr_dia_add = lBPCDetails.sr_dia_add;
                                    lBPCDesUpdate.sr1_location = lBPCDetails.sr1_location;
                                    lBPCDesUpdate.sr2_location = lBPCDetails.sr2_location;
                                    lBPCDesUpdate.sr3_location = lBPCDetails.sr3_location;
                                    lBPCDesUpdate.sr4_location = lBPCDetails.sr4_location;
                                    lBPCDesUpdate.sr5_location = lBPCDetails.sr5_location;
                                    lBPCDesUpdate.crank_height_top = lBPCDetails.crank_height_top;
                                    lBPCDesUpdate.crank_height_end = lBPCDetails.crank_height_end;
                                    lBPCDesUpdate.crank2_height_top = lBPCDetails.crank2_height_top;
                                    lBPCDesUpdate.crank2_height_end = lBPCDetails.crank2_height_end;
                                    lBPCDesUpdate.sl1_length = lBPCDetails.sl1_length;
                                    lBPCDesUpdate.sl2_length = lBPCDetails.sl2_length;
                                    lBPCDesUpdate.sl3_length = lBPCDetails.sl3_length;
                                    lBPCDesUpdate.sl1_dia = lBPCDetails.sl1_dia;
                                    lBPCDesUpdate.sl2_dia = lBPCDetails.sl2_dia;
                                    lBPCDesUpdate.sl3_dia = lBPCDetails.sl3_dia;
                                    lBPCDesUpdate.total_sl_length = lBPCDetails.total_sl_length;
                                    lBPCDesUpdate.no_of_cr_top = lBPCDetails.no_of_cr_top;
                                    lBPCDesUpdate.cr_spacing_top = lBPCDetails.cr_spacing_top;
                                    lBPCDesUpdate.no_of_cr_end = lBPCDetails.no_of_cr_end;
                                    lBPCDesUpdate.cr_spacing_end = lBPCDetails.cr_spacing_end;
                                    lBPCDesUpdate.cr_end_remarks = lBPCDetails.cr_end_remarks;
                                    lBPCDesUpdate.extra_support_bar_ind = lBPCDetails.extra_support_bar_ind;
                                    lBPCDesUpdate.extra_support_bar_dia = lBPCDetails.extra_support_bar_dia;
                                    lBPCDesUpdate.extra_cr_no = lBPCDetails.extra_cr_no;
                                    lBPCDesUpdate.extra_cr_loc = lBPCDetails.extra_cr_loc;
                                    lBPCDesUpdate.extra_cr_dia = lBPCDetails.extra_cr_dia;
                                    lBPCDesUpdate.mainbar_length_2layer = lBPCDetails.mainbar_length_2layer;
                                    lBPCDesUpdate.mainbar_location_2layer = lBPCDetails.mainbar_location_2layer;

                                    lBPCDesUpdate.per_set = lBPCDetails.per_set;
                                    lBPCDesUpdate.bbs_no = lBPCDetails.bbs_no;
                                    lBPCDesUpdate.cage_qty = lBPCDetails.cage_qty;
                                    lBPCDesUpdate.cage_weight = lBPCDetails.cage_weight;
                                    lBPCDesUpdate.cage_remarks = "";
                                    lBPCDesUpdate.set_code = lBPCDetails.set_code;
                                    lBPCDesUpdate.cr_link_lapping = lBPCDetails.cr_link_lapping ?? 51;
                                    lBPCDesUpdate.sr_link_lapping = lBPCDetails.sr_link_lapping ?? 10;

                                    var lBPCDetList = new List<BPCDetailsModels>();
                                    lBPCDetList.Add(lBPCDesUpdate);
                                    lBPCDesUpdate.set_code = genBPCSetCode(lBPCDesUpdate, lBPCDetList, 0);

                                    lBPCDesUpdate.sap_mcode = genBPCSAPMaterialCode(lBPCDesUpdate);

                                    db.BPCDetails.Add(lBPCDesUpdate);

                                    List<BPCCageBarsModels> lRebarDet = SaveRebarDetails(lBPCDesUpdate);
                                    if (lRebarDet.Count > 0)
                                    {
                                        db.BPCCageBars.AddRange(lRebarDet);
                                    }

                                    List<BPCTemplateModels> lTempDet = AssignTemplates(lBPCDesUpdate);
                                    if (lTempDet.Count > 0)
                                    {
                                        db.BPCTemplate.AddRange(lTempDet);
                                    }

                                    db.SaveChanges();

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var lError = ex.Message;
                return Ok(false);
            }
            return Ok(true);
        }

        [HttpGet]
        [Route("/copyBPCLibrary_HMI_Projectwise/{SourceCustomerCode}/{SourceProjectCode}/{DesCustomerCode}/{DesProjectCode}/{UserName}")]
        //[ValidateAntiForgeryHeader]
        public ActionResult copyBPCLibraryHMIProjectwise(string SourceCustomerCode,
                string SourceProjectCode,
                string DesCustomerCode,
                string DesProjectCode,
                string UserName)
        {
            var lTemplate = true;
            var lSOurceMaxJobID = (from p in db.BPCJobAdvice
                                   where p.CustomerCode == SourceCustomerCode &&
                                   p.ProjectCode == SourceProjectCode &&
                                   p.Template == lTemplate
                                   select p.JobID).DefaultIfEmpty(0).Max();

            List<int> lJobIDList = Enumerable.Range(1, lSOurceMaxJobID).ToList();
            try
            {
                if (lJobIDList.Count > 0)
                {
                    var lNewJobID = (from p in db.BPCJobAdvice
                                     where p.CustomerCode == DesCustomerCode &&
                                     p.ProjectCode == DesProjectCode &&
                                     p.Template == lTemplate
                                     select p.JobID).DefaultIfEmpty(0).Max();

                    for (int i = 0; i < lJobIDList.Count; i++)
                    {
                        int lJobIDSource = lJobIDList[i];
                        if (lJobIDSource > 0)
                        {
                            var lJobSource = db.BPCJobAdvice.Find(SourceCustomerCode, SourceProjectCode, lTemplate, lJobIDSource);
                            if (lJobSource != null)
                            {
                                var lBPCDetails = db.BPCDetails.Find(SourceCustomerCode, SourceProjectCode, lTemplate, lJobIDSource, 1);
                                if (lBPCDetails != null)
                                {
                                    // start copy

                                    lNewJobID = lNewJobID + 1;

                                    var lJobDesUpdate = new BPCJobAdviceModels();
                                    lJobDesUpdate.CustomerCode = DesCustomerCode;
                                    lJobDesUpdate.ProjectCode = DesProjectCode;
                                    lJobDesUpdate.Template = true;
                                    lJobDesUpdate.JobID = lNewJobID;
                                    lJobDesUpdate.PONumber = lJobSource.PONumber;
                                    lJobDesUpdate.PODate = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                                    lJobDesUpdate.RequiredDate = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                                    lJobDesUpdate.cover_to_link = lJobSource.cover_to_link;
                                    lJobDesUpdate.UpdateDate = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                                    lJobDesUpdate.UpdateBy = UserName;//"Vishalw_ttl@natsteel.com.sg";
                                    db.BPCJobAdvice.Add(lJobDesUpdate);

                                    var lBPCDesUpdate = new BPCDetailsModels();

                                    lBPCDesUpdate.CustomerCode = DesCustomerCode;
                                    lBPCDesUpdate.ProjectCode = DesProjectCode;
                                    lBPCDesUpdate.Template = true;
                                    lBPCDesUpdate.JobID = lNewJobID;
                                    lBPCDesUpdate.cage_id = 1;
                                    lBPCDesUpdate.copyfrom_project = SourceProjectCode;
                                    lBPCDesUpdate.copyfrom_template = lBPCDetails.Template;
                                    lBPCDesUpdate.copyfrom_jobid = lJobIDSource;
                                    lBPCDesUpdate.copyfrom_ponumber = lJobSource.PONumber;
                                    lBPCDesUpdate.UpdateDate = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                                    lBPCDesUpdate.UpdateBy = UserName;//"Vishalw_ttl@natsteel.com.sg";

                                    lBPCDesUpdate.pile_type = lBPCDetails.pile_type;
                                    lBPCDesUpdate.pile_dia = lBPCDetails.pile_dia;
                                    lBPCDesUpdate.cage_dia = lBPCDetails.cage_dia;
                                    lBPCDesUpdate.main_bar_arrange = lBPCDetails.main_bar_arrange;
                                    lBPCDesUpdate.main_bar_type = lBPCDetails.main_bar_type;
                                    lBPCDesUpdate.main_bar_ct = lBPCDetails.main_bar_ct;
                                    lBPCDesUpdate.main_bar_shape = lBPCDetails.main_bar_shape;
                                    lBPCDesUpdate.main_bar_grade = lBPCDetails.main_bar_grade;
                                    lBPCDesUpdate.main_bar_dia = lBPCDetails.main_bar_dia;
                                    lBPCDesUpdate.main_bar_topjoin = lBPCDetails.main_bar_topjoin;
                                    lBPCDesUpdate.main_bar_endjoin = lBPCDetails.main_bar_endjoin;
                                    lBPCDesUpdate.cage_length = lBPCDetails.cage_length;
                                    lBPCDesUpdate.spiral_link_type = lBPCDetails.spiral_link_type;
                                    lBPCDesUpdate.spiral_link_grade = lBPCDetails.spiral_link_grade;
                                    lBPCDesUpdate.spiral_link_dia = lBPCDetails.spiral_link_dia;
                                    lBPCDesUpdate.spiral_link_spacing = lBPCDetails.spiral_link_spacing;
                                    lBPCDesUpdate.lap_length = lBPCDetails.lap_length;
                                    lBPCDesUpdate.end_length = lBPCDetails.end_length;
                                    lBPCDesUpdate.cage_location = lBPCDetails.cage_location;

                                    lBPCDesUpdate.rings_start = lBPCDetails.rings_start;
                                    lBPCDesUpdate.rings_end = lBPCDetails.rings_end;
                                    lBPCDesUpdate.rings_addn_no = lBPCDetails.rings_addn_no;
                                    lBPCDesUpdate.rings_addn_member = lBPCDetails.rings_addn_member;
                                    lBPCDesUpdate.coupler_top = lBPCDetails.coupler_top;
                                    lBPCDesUpdate.coupler_end = lBPCDetails.coupler_end;
                                    lBPCDesUpdate.no_of_sr = lBPCDetails.no_of_sr;
                                    lBPCDesUpdate.sr_grade = lBPCDetails.sr_grade;
                                    lBPCDesUpdate.sr_dia = lBPCDetails.sr_dia;
                                    lBPCDesUpdate.sr_dia_add = lBPCDetails.sr_dia_add;
                                    lBPCDesUpdate.sr1_location = lBPCDetails.sr1_location;
                                    lBPCDesUpdate.sr2_location = lBPCDetails.sr2_location;
                                    lBPCDesUpdate.sr3_location = lBPCDetails.sr3_location;
                                    lBPCDesUpdate.sr4_location = lBPCDetails.sr4_location;
                                    lBPCDesUpdate.sr5_location = lBPCDetails.sr5_location;
                                    lBPCDesUpdate.crank_height_top = lBPCDetails.crank_height_top;
                                    lBPCDesUpdate.crank_height_end = lBPCDetails.crank_height_end;
                                    lBPCDesUpdate.crank2_height_top = lBPCDetails.crank2_height_top;
                                    lBPCDesUpdate.crank2_height_end = lBPCDetails.crank2_height_end;
                                    lBPCDesUpdate.sl1_length = lBPCDetails.sl1_length;
                                    lBPCDesUpdate.sl2_length = lBPCDetails.sl2_length;
                                    lBPCDesUpdate.sl3_length = lBPCDetails.sl3_length;
                                    lBPCDesUpdate.sl1_dia = lBPCDetails.sl1_dia;
                                    lBPCDesUpdate.sl2_dia = lBPCDetails.sl2_dia;
                                    lBPCDesUpdate.sl3_dia = lBPCDetails.sl3_dia;
                                    lBPCDesUpdate.total_sl_length = lBPCDetails.total_sl_length;
                                    lBPCDesUpdate.no_of_cr_top = lBPCDetails.no_of_cr_top;
                                    lBPCDesUpdate.cr_spacing_top = lBPCDetails.cr_spacing_top;
                                    lBPCDesUpdate.no_of_cr_end = lBPCDetails.no_of_cr_end;
                                    lBPCDesUpdate.cr_spacing_end = lBPCDetails.cr_spacing_end;
                                    lBPCDesUpdate.cr_end_remarks = lBPCDetails.cr_end_remarks;
                                    lBPCDesUpdate.extra_support_bar_ind = lBPCDetails.extra_support_bar_ind;
                                    lBPCDesUpdate.extra_support_bar_dia = lBPCDetails.extra_support_bar_dia;
                                    lBPCDesUpdate.extra_cr_no = lBPCDetails.extra_cr_no;
                                    lBPCDesUpdate.extra_cr_loc = lBPCDetails.extra_cr_loc;
                                    lBPCDesUpdate.extra_cr_dia = lBPCDetails.extra_cr_dia;
                                    lBPCDesUpdate.mainbar_length_2layer = lBPCDetails.mainbar_length_2layer;
                                    lBPCDesUpdate.mainbar_location_2layer = lBPCDetails.mainbar_location_2layer;

                                    lBPCDesUpdate.per_set = lBPCDetails.per_set;
                                    lBPCDesUpdate.bbs_no = lBPCDetails.bbs_no;
                                    lBPCDesUpdate.cage_qty = lBPCDetails.cage_qty;
                                    lBPCDesUpdate.cage_weight = lBPCDetails.cage_weight;
                                    lBPCDesUpdate.cage_remarks = "";
                                    lBPCDesUpdate.set_code = lBPCDetails.set_code;
                                    lBPCDesUpdate.cr_link_lapping = lBPCDetails.cr_link_lapping ?? 51;
                                    lBPCDesUpdate.sr_link_lapping = lBPCDetails.sr_link_lapping ?? 10;

                                    var lBPCDetList = new List<BPCDetailsModels>();
                                    lBPCDetList.Add(lBPCDesUpdate);
                                    lBPCDesUpdate.set_code = genBPCSetCode(lBPCDesUpdate, lBPCDetList, 0);

                                    lBPCDesUpdate.sap_mcode = genBPCSAPMaterialCodeHMI(lBPCDesUpdate);

                                    db.BPCDetails.Add(lBPCDesUpdate);

                                    List<BPCCageBarsModels> lRebarDet = SaveRebarDetails(lBPCDesUpdate);
                                    if (lRebarDet.Count > 0)
                                    {
                                        db.BPCCageBars.AddRange(lRebarDet);
                                    }

                                    List<BPCTemplateModels> lTempDet = AssignTemplates(lBPCDesUpdate);
                                    if (lTempDet.Count > 0)
                                    {
                                        db.BPCTemplate.AddRange(lTempDet);
                                    }

                                    db.SaveChanges();

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var lError = ex.Message;
                return Ok(false);
            }
            return Ok(true);
        }


        // POST: Customer/copyBBS/
        [HttpGet]
        [Route("/copyBPCLibrary_bpc/{SourceCustomerCode}/{SourceProjectCode}/{SourceTemplate}/{SourceOrderNo}/{DesCustomerCode}/{DesProjectCode}/{DesTemplate}/{DesOrderNo}/{DesCopyQty}/{CopyModel}/{UserName}")]
        //[ValidateAntiForgeryHeader]
        public  ActionResult copyBBS(string SourceCustomerCode,
                string SourceProjectCode,
                bool SourceTemplate,
                int SourceOrderNo,
                string DesCustomerCode,
                string DesProjectCode,
                bool DesTemplate,
                int DesOrderNo,
                int DesCopyQty,
                string CopyModel,
                string UserName)
        {
            var lMaxBPCID = 0;
            var lRemarks = "";
            int lTotalQty = 0;
            decimal lTotalWT = 0;
            string lCopyFromPO = "";
            if (CopyModel == "COMMON BORED PILE CAGE")
            {
                SourceCustomerCode = "0000000000";
                SourceProjectCode = "0000000000";
            }
            var lJobSource = db.BPCJobAdvice.Find(SourceCustomerCode, SourceProjectCode, SourceTemplate, SourceOrderNo);
            if (lJobSource != null)
            {
                if (DesCopyQty != 1)
                {
                    lCopyFromPO = DesCopyQty.ToString() + " x " + lJobSource.PONumber.Trim();
                }
                else
                {
                    lCopyFromPO = lJobSource.PONumber.Trim();
                }

                lMaxBPCID = (from p in db.BPCDetails
                             where p.CustomerCode == DesCustomerCode &&
                             p.ProjectCode == DesProjectCode &&
                             p.Template == DesTemplate &&
                             p.JobID == DesOrderNo
                             select p.cage_id).DefaultIfEmpty(0).Max();

                //copy BPC details;
                var lBPCSource = (from p in db.BPCDetails
                                  where p.CustomerCode == SourceCustomerCode &&
                                  p.ProjectCode == SourceProjectCode &&
                                  p.Template == SourceTemplate &&
                                  p.JobID == SourceOrderNo
                                  select p).ToList();

                var lBPCNewDes = new List<BPCDetailsModels>
                    (lBPCSource.Select(h => new BPCDetailsModels
                    {
                        CustomerCode = DesCustomerCode,
                        ProjectCode = DesProjectCode,
                        Template = DesTemplate,
                        JobID = DesOrderNo,
                        cage_id = lMaxBPCID + h.cage_id,
                        pile_type = h.pile_type,
                        pile_dia = h.pile_dia,
                        cage_dia = h.cage_dia,
                        main_bar_arrange = h.main_bar_arrange,
                        main_bar_type = h.main_bar_type,
                        main_bar_ct = h.main_bar_ct,
                        main_bar_shape = h.main_bar_shape,
                        main_bar_grade = h.main_bar_grade,
                        main_bar_dia = h.main_bar_dia,
                        main_bar_topjoin = h.main_bar_topjoin,
                        main_bar_endjoin = h.main_bar_endjoin,
                        cage_length = h.cage_length,
                        spiral_link_type = h.spiral_link_type,
                        spiral_link_grade = h.spiral_link_grade,
                        spiral_link_dia = h.spiral_link_dia,
                        spiral_link_spacing = h.spiral_link_spacing,
                        lap_length = h.lap_length,
                        end_length = h.end_length,
                        cage_location = h.cage_location,

                        rings_start = h.rings_start,
                        rings_end = h.rings_end,
                        rings_addn_no = h.rings_addn_no,
                        rings_addn_member = h.rings_addn_member,
                        coupler_top = h.coupler_top,
                        coupler_end = h.coupler_end,
                        no_of_sr = h.no_of_sr,
                        sr_grade = h.sr_grade,
                        sr_dia = h.sr_dia,
                        sr_dia_add = h.sr_dia_add,
                        sr1_location = h.sr1_location,
                        sr2_location = h.sr2_location,
                        sr3_location = h.sr3_location,
                        sr4_location = h.sr4_location,
                        sr5_location = h.sr5_location,
                        crank_height_top = h.crank_height_top,
                        crank_height_end = h.crank_height_end,
                        crank2_height_top = h.crank2_height_top,
                        crank2_height_end = h.crank2_height_end,
                        sl1_length = h.sl1_length,
                        sl2_length = h.sl2_length,
                        sl3_length = h.sl3_length,
                        sl1_dia = h.sl1_dia,
                        sl2_dia = h.sl2_dia,
                        sl3_dia = h.sl3_dia,
                        total_sl_length = h.total_sl_length,
                        no_of_cr_top = h.no_of_cr_top,
                        cr_spacing_top = h.cr_spacing_top,
                        no_of_cr_end = h.no_of_cr_end,
                        cr_spacing_end = h.cr_spacing_end,
                        cr_end_remarks = h.cr_end_remarks,
                        extra_support_bar_ind = h.extra_support_bar_ind,
                        extra_support_bar_dia = h.extra_support_bar_dia,
                        extra_cr_no = h.extra_cr_no,
                        extra_cr_loc = h.extra_cr_loc,
                        extra_cr_dia = h.extra_cr_dia,
                        mainbar_length_2layer = h.mainbar_length_2layer,
                        mainbar_location_2layer = h.mainbar_location_2layer,

                        per_set = h.per_set,
                        bbs_no = h.bbs_no,
                        cage_qty = h.cage_qty * DesCopyQty,
                        cage_weight = h.cage_weight * DesCopyQty,
                        cage_remarks = "",
                        set_code = "",

                        copyfrom_project = SourceProjectCode,
                        copyfrom_template = SourceTemplate,
                        copyfrom_jobid = SourceOrderNo,
                        copyfrom_ponumber = lCopyFromPO,
                        cr_link_lapping = h.cr_link_lapping ?? 51,
                        sr_link_lapping = h.sr_link_lapping ?? 10,

                        UpdateDate = DateTime.Now,
                        UpdateBy = UserName//"Vishalw_ttl@natsteel.com.sg"
                    })).ToList();

                for (int i = 0; i < lBPCNewDes.Count; i++)
                {
                    lTotalQty = lTotalQty + lBPCNewDes[i].cage_qty;
                    lTotalWT = lTotalWT + lBPCNewDes[i].cage_weight;

                    lBPCNewDes[i].set_code = genBPCSetCode(lBPCNewDes[i], lBPCNewDes, i);
                    lBPCNewDes[i].UpdateBy = UserName;//User.Identity.Name;
                    lBPCNewDes[i].UpdateDate = DateTime.Now;
                    if (lBPCNewDes[i].CustomerCode == null)
                    {
                        lBPCNewDes[i].CustomerCode = "";
                    }
                    if (lBPCNewDes[i].CustomerCode.Length > 0)
                    {
                        db.BPCDetails.Add(lBPCNewDes[i]);
                    }

                    List<BPCCageBarsModels> lRebarDet = SaveRebarDetails(lBPCNewDes[i]);
                    if (lRebarDet.Count > 0)
                    {
                        db.BPCCageBars.AddRange(lRebarDet);
                    }

                    List<BPCTemplateModels> lTempDet = AssignTemplates(lBPCNewDes[i]);
                    if (lTempDet.Count > 0)
                    {
                        db.BPCTemplate.AddRange(lTempDet);
                    }

                    lBPCNewDes[i].set_code = genBPCSetCode(lBPCNewDes[i], lBPCNewDes, i);
                }

                var lJobDes = db.BPCJobAdvice.Find(DesCustomerCode, DesProjectCode, DesTemplate, DesOrderNo);
                var lJobDesUpdate = lJobDes;
                lJobDesUpdate.TotalPcs = lJobDesUpdate.TotalPcs + lTotalQty;
                lJobDesUpdate.TotalWeight = lJobDesUpdate.TotalWeight + lTotalWT;

                db.Entry(lJobDes).CurrentValues.SetValues(lJobDesUpdate);

                db.SaveChanges();
            }

            return Ok(true);
        }

        //save BBS
        [HttpPost]
        [Route("/saveBBS_bpc/{SourceCustomerCode}/{ProjectCode}/{Template}/{JobID}")]
        //[ValidateAntiForgeryHeader]
        public async Task<ActionResult> saveBBS(string CustomerCode, string ProjectCode, bool Template, int JobID, [FromBody] SAveBBSDto sAveBBSDto)
        {
            var lErrorMsg = "Successfully saved.";
            var lLib = true;
            var lReturn = true;
            var lTotalWT = (decimal)0;
            var lTotalOrderWT = (decimal)0;
            
            try
            {
                if (Template == false)
                {
                    var lStatus = db.Database.SqlQuery<string>("SELECT S.OrderStatus " +
                    "FROM dbo.OESProjOrder P, dbo.OESProjOrdersSE S " +
                    "WHERE P.OrderNumber = S.OrderNumber " +
                    "AND P.CustomerCode = '" + CustomerCode + "' " +
                    "AND P.ProjectCode = '" + ProjectCode + "' " +
                    "AND S.BPCJobID = " + JobID.ToString() + " ").ToList();

                    if (lStatus != null && lStatus.Count > 0 && lStatus[0] != "New" && lStatus[0] != "Created" && lStatus[0] != "Created*" && lStatus[0] != "Submitted*")
                    {
                        lErrorMsg = "The order had been submitted already.";
                        lReturn = false;
                        //return Json(new { success = lReturn, responseText = lErrorMsg }, JsonRequestBehavior.AllowGet);
                        return Ok();
                    }

                }

                db.Database.ExecuteSqlCommand("DELETE FROM OESBPCDetails " +
                "WHERE CustomerCode = '" + CustomerCode + "' " +
                "AND ProjectCode = '" + ProjectCode + "' " +
                "AND Template = " + (Template == true ? 1 : 0) + " " +
                "AND JobID = " + JobID.ToString() + " ");

                db.Database.ExecuteSqlCommand("DELETE FROM OESBPCCageBars " +
                "WHERE CustomerCode = '" + CustomerCode + "' " +
                "AND ProjectCode = '" + ProjectCode + "' " +
                "AND Template = " + (Template == true ? 1 : 0) + " " +
                "AND JobID = " + JobID.ToString() + " ");

                db.Database.ExecuteSqlCommand("DELETE FROM OESBPCTemplates " +
                "WHERE CustomerCode = '" + CustomerCode + "' " +
                "AND ProjectCode = '" + ProjectCode + "' " +
                "AND Template = " + (Template == true ? 1 : 0) + " " +
                "AND JobID = " + JobID.ToString() + " ");

                var lJobAdvice = db.BPCJobAdvice.Find(CustomerCode, ProjectCode, Template, JobID);

                if (sAveBBSDto.BPCModels != null && sAveBBSDto.BPCModels.Count > 0)
                {

                    for (int i = 0; i < sAveBBSDto.BPCModels.Count; i++)
                    {
                        lTotalWT = 0;
                        sAveBBSDto.BPCModels[i].cage_id = i + 1;
                        //sAveBBSDto.BPCModels[i].UpdateBy = User.Identity.Name;
                        sAveBBSDto.BPCModels[i].UpdateDate = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                        sAveBBSDto.BPCModels[i].Template = Template;
                        sAveBBSDto.BPCModels[i].BPC_Type = sAveBBSDto.BPCModels[i].BPC_Type ?? "FBP";
                        if (sAveBBSDto.BPCModels[i].CustomerCode == null)
                        {
                            sAveBBSDto.BPCModels[i].CustomerCode = "";
                        }

                        if (sAveBBSDto.BPCModels[i].cage_dia == 0 && lJobAdvice != null && lJobAdvice.cover_to_link != 0)
                        {
                            sAveBBSDto.BPCModels[i].cage_dia = sAveBBSDto.BPCModels[i].pile_dia - 2 * lJobAdvice.cover_to_link;
                        }
                        List<BPCCageBarsModels> lRebarDet = SaveRebarDetails(sAveBBSDto.BPCModels[i]);
                        if (lRebarDet.Count > 0)
                        {
                            db.BPCCageBars.AddRange(lRebarDet);

                            for (int j = 0; j < lRebarDet.Count; j++)
                            {
                                if (lRebarDet[j].BarWeight != null && lRebarDet[j].BarWeight > 0 && sAveBBSDto.BPCModels[i].cage_qty > 0)
                                {
                                    lTotalWT = lTotalWT + (decimal)lRebarDet[j].BarWeight * sAveBBSDto.BPCModels[i].cage_qty;
                                }
                            }
                        }

                        lTotalOrderWT = lTotalOrderWT + lTotalWT;

                        sAveBBSDto.BPCModels[i].set_code = genBPCSetCode(sAveBBSDto.BPCModels[i], sAveBBSDto.BPCModels, i);
                        sAveBBSDto.BPCModels[i].sap_mcode = genBPCSAPMaterialCode(sAveBBSDto.BPCModels[i]);

                        if (sAveBBSDto.BPCModels[i].CustomerCode.Length > 0)
                        {
                            sAveBBSDto.BPCModels[i].cage_weight = Math.Round(lTotalWT) / 1000;
                            db.BPCDetails.Add(sAveBBSDto.BPCModels[i]);
                        }

                        List<BPCTemplateModels> lTempDet = AssignTemplates(sAveBBSDto.BPCModels[i]);
                        if (lTempDet.Count > 0)
                        {
                            db.BPCTemplate.AddRange(lTempDet);
                        }
                       
                    }

                    var lOldJobAdvice = db.BPCJobAdvice.Find(CustomerCode, ProjectCode, Template, JobID);
                    var lNewJobAdvice = lOldJobAdvice;

                    lNewJobAdvice.TotalWeight = Math.Round(lTotalOrderWT) / 1000;
                    db.Entry(lOldJobAdvice).CurrentValues.SetValues(lNewJobAdvice);

                    db.SaveChanges();
                    for (int i = 0; i < sAveBBSDto.BPCModels.Count; i++)
                    {
                        try
                        {
                            await SaveBPCWithJobID(sAveBBSDto.BPCModels[i].CustomerCode,
                        sAveBBSDto.BPCModels[i].ProjectCode, sAveBBSDto.BPCModels[i].JobID,
                        sAveBBSDto.BPCModels[i].cage_id, sAveBBSDto.BPCModels[i].set_code);
                        }
                        catch(Exception ex)
                        {
                            Console.WriteLine("Error");
                        }

                     
                    }
                }
            }
            catch (Exception ex)
            {
                lErrorMsg = ex.Message;
                lReturn = false;
            }
            //return Json(new { success = lReturn, responseText = lErrorMsg }, JsonRequestBehavior.AllowGet);
            return Ok(lReturn);
        }

        [HttpPost]
        [Route("/saveLibData_bpc/{CustomerCode}/{ProjectCode}/{JobID}/{LinkToCover}/{CageName}/{UserName}")]
        //[ValidateAntiForgeryHeader]
        public ActionResult saveLibData([FromBody] BPCDetailsModels BPCModels, string CustomerCode, string ProjectCode, int JobID, int LinkToCover, string CageName, string UserName)
        {
            var lErrorMsg = "Successfully saved.";
            var lReturn = true;
            var lLib = true;
            var lCageID = 1;
            var lTotalWT = (decimal)0;
            var lTotalOrderWT = (decimal)0;
            bool isCabEdit = false;
            BPCModels.sr_link_lapping = BPCModels.sr_link_lapping ?? 10;
            BPCModels.cr_link_lapping = BPCModels.cr_link_lapping ?? 51;
            //IActionResult result = GetIsCABEdit(CustomerCode, ProjectCode, BPCModels.Template,JobID, BPCModels.cage_id);
            bool isSuccess = false;
            List<BPCCageBarsModels> lRebarDet = new List<BPCCageBarsModels>();

            var lRebarDet_new = getRebarData(CustomerCode, ProjectCode, JobID, BPCModels.cage_dia, BPCModels.Template);

            if (lRebarDet_new is OkObjectResult okResult)
            {
                var list = okResult.Value as IEnumerable<dynamic>;
                var list2 = list.ToList();

                if (list2 != null)
                {
                    for (int i = 0; i < list2.Count(); i++)
                    {
                        lRebarDet[i].BarWeight = list2[i].BarWeight;
                    }
                }
            }

            isCabEdit = db.BPCCageBars

                .Where(bar => bar.CustomerCode == CustomerCode &&

                              bar.ProjectCode == ProjectCode &&

                              bar.Template == BPCModels.Template &&

                              bar.JobID == JobID &&

                              bar.CageID == BPCModels.cage_id)

                .Count(bar => (bool)bar.isCABEdit) > 0 ? true : false;




            if (CustomerCode != null && ProjectCode != null && BPCModels != null)
            {
                try
                {
                    //Verify and Gen JobID
                    var lJobAdvice = db.BPCJobAdvice.Find(CustomerCode, ProjectCode, lLib, JobID);
                    if (lJobAdvice == null)
                    {
                        var lJobID = db.BPCJobAdvice.Where(z => z.CustomerCode == CustomerCode &&
                        z.ProjectCode == ProjectCode &&
                        z.Template == lLib).Max(z => (int?)z.JobID);

                        if (lJobID == null)
                        {
                            lJobID = 1;
                        }
                        else
                        {
                            lJobID = lJobID + 1;
                        }

                        JobID = (int)lJobID;
                    }


                    if (BPCModels != null && BPCModels.cage_dia > 0)
                    {
                        if (isCabEdit == false)
                        {
                            db.Database.ExecuteSqlCommand("DELETE FROM OESBPCCageBars " +
                       "WHERE CustomerCode = '" + CustomerCode + "' " +
                       "AND ProjectCode = '" + ProjectCode + "' " +
                       "AND Template = " + (lLib == true ? 1 : 0) + " " +
                       "AND JobID = " + JobID.ToString() + " ");
                        }


                        db.Database.ExecuteSqlCommand("DELETE FROM OESBPCTemplates " +
                        "WHERE CustomerCode = '" + CustomerCode + "' " +
                        "AND ProjectCode = '" + ProjectCode + "' " +
                        "AND Template = " + (lLib == true ? 1 : 0) + " " +
                        "AND JobID = " + JobID.ToString() + " ");

                        lTotalWT = 0;

                        if (BPCModels.CustomerCode == null || BPCModels.CustomerCode.Trim() == "")
                        {
                            BPCModels.CustomerCode = CustomerCode;
                        }

                        if (BPCModels.ProjectCode == null || BPCModels.ProjectCode.Trim() == "")
                        {
                            BPCModels.ProjectCode = ProjectCode;
                        }
                        BPCModels.Template = lLib;
                        BPCModels.JobID = JobID;
                        BPCModels.cage_id = 1;
                        BPCModels.cage_qty = 1;
                        BPCModels.BPC_Type=BPCModels.BPC_Type ?? "FBP";
                        BPCModels.UpdateBy = User.Identity.Name;
                        BPCModels.UpdateDate = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                        if (BPCModels.cage_dia == 0 && LinkToCover > 0)
                        {
                            BPCModels.cage_dia = BPCModels.pile_dia - 2 * LinkToCover;
                        }

                        List<BPCDetailsModels> lBPSList = new List<BPCDetailsModels>();
                        lBPSList.Add(BPCModels);

                        BPCModels.set_code = genBPCSetCode(BPCModels, lBPSList, 1);
                        //check repeated setcode in the project
                        var lSetCode = BPCModels.set_code;
                        if (lSetCode != null && lSetCode.Trim().Length > 0)
                        {
                            var lRSet = (from p in db.BPCDetails
                                         where p.CustomerCode == CustomerCode &&
                                         p.ProjectCode == ProjectCode &&
                                         p.Template == lLib &&
                                         p.JobID != JobID &&
                                         p.set_code == lSetCode
                                         select p).ToList();


                            while (lRSet != null && lRSet.Count > 0)
                            {
                                // increment the set code
                                lSetCode = IncrementSetCode(lSetCode);

                                // recheck with updated code
                                lRSet = (from p in db.BPCDetails
                                         where p.CustomerCode == CustomerCode &&
                                               p.ProjectCode == ProjectCode &&
                                               p.Template == lLib &&
                                               p.JobID != JobID &&
                                               p.set_code == lSetCode
                                         select p).ToList();
                            }
                            if (lRSet != null && lRSet.Count > 0)
                            {
                                lErrorMsg = "Found repeated set code. Please change the setcode and save again. ";
                                lReturn = false;
                                JobID = 0;
                            }
                            else
                            {
                                BPCModels.set_code = lSetCode;
                            }

                            }

                        if (lReturn == true)
                        {
                            //List<BPCCageBarsModels> lRebarDet = new List<BPCCageBarsModels>();
                            if (isCabEdit == false)
                            {
                                lRebarDet = SaveRebarDetails(BPCModels);
                            }
                            //else
                            //{
                            //    var lRebarDet_new = getRebarData(CustomerCode, ProjectCode, JobID, BPCModels.cage_dia, BPCModels.Template);

                            //    if (lRebarDet_new is OkObjectResult okResult)
                            //    {
                            //        var list = okResult.Value as IEnumerable<dynamic>;
                            //        var list2 = list.ToList();

                            //        if (list2 != null)
                            //        {
                            //            for (int i =0;i< list2.Count();i++)
                            //            {
                            //                lRebarDet[i].BarWeight = list2[i].BarWeight;
                            //            }
                            //        }
                            //    }



                            //}
                            if (lRebarDet.Count > 0)
                            {
                                if (isCabEdit == false)
                                {
                                    db.BPCCageBars.AddRange(lRebarDet);
                                }

                                for (int j = 0; j < lRebarDet.Count; j++)
                                {
                                    if (lRebarDet[j].BarWeight != null && lRebarDet[j].BarWeight > 0 && BPCModels.cage_qty > 0)
                                    {
                                        lTotalWT = lTotalWT + (decimal)lRebarDet[j].BarWeight * BPCModels.cage_qty;
                                    }
                                }
                            }

                            lTotalOrderWT = lTotalOrderWT + lTotalWT;


                            BPCModels.sap_mcode = genBPCSAPMaterialCode(BPCModels);

                            BPCModels.cage_weight = Math.Round(lTotalWT) / 1000;

                            //Save Templates
                            List<BPCTemplateModels> lTempDet = AssignTemplates(BPCModels);
                            if (lTempDet.Count > 0)
                            {
                                db.BPCTemplate.AddRange(lTempDet);
                            }

                            // save Cage Details
                            var lCage = db.BPCDetails.Find(CustomerCode, ProjectCode, lLib, JobID, lCageID);
                            if (lCage == null)
                            {
                                db.BPCDetails.Add(BPCModels);
                            }
                            else
                            {
                                db.Entry(lCage).CurrentValues.SetValues(BPCModels);

                            }

                            if (lJobAdvice == null)
                            {
                                var lJobAdv = new BPCJobAdviceModels();
                                lJobAdv.CustomerCode = CustomerCode;
                                lJobAdv.ProjectCode = ProjectCode;
                                lJobAdv.Template = lLib;
                                lJobAdv.JobID = JobID;

                                lJobAdv.PODate = DateTime.Now;
                                lJobAdv.RequiredDate = DateTime.Now.AddDays(10);
                                lJobAdv.TotalPcs = 0;
                                lJobAdv.TotalWeight = Math.Round(lTotalOrderWT) / 1000;

                                lJobAdv.Transport = "TR40/24";
                                lJobAdv.OrderStatus = "New";
                                lJobAdv.cover_to_link = LinkToCover;
                                lJobAdv.OrderSource = "UX";
                                lJobAdv.DeliveryAddress = "";
                                lJobAdv.Remarks = "";
                                lJobAdv.PONumber = CageName;
                                lJobAdv.UpdateDate = DateTime.Now;

                                lJobAdv.UpdateBy = UserName;//"Vishalw_ttl@natsteel.com.sg";

                                db.BPCJobAdvice.Add(lJobAdv);

                            }
                            else
                            {
                                var lNewJobAdvice = lJobAdvice;

                                lNewJobAdvice.cover_to_link = LinkToCover;
                                lNewJobAdvice.PONumber = CageName;

                                lNewJobAdvice.TotalWeight = Math.Round(lTotalOrderWT) / 1000;
                                db.Entry(lJobAdvice).CurrentValues.SetValues(lNewJobAdvice);
                            }

                            db.SaveChanges();

                            if (BPCModels.cage_dia < 350 && BPCModels.pile_type != "Micro-Pile")
                            {
                                lErrorMsg = "Found cage diameter less than 350mmm. Please change cover or pile diameter and save again. ";
                                lReturn = false;
                                JobID = 0;
                            }
                        }
                    }


                }
                catch (Exception ex)
                {
                    lErrorMsg = ex.Message;
                    lReturn = false;
                    JobID = 0;
                }
            }
            //return Json(new { Success = lReturn, jobID = JobID, set_code = BPCModels.set_code, responseText = lErrorMsg }, JsonRequestBehavior.AllowGet);
            return Ok(new { JobID, set_code = BPCModels.set_code, response = lReturn, responseText = lErrorMsg });
        }

        string IncrementSetCode(string setCode)
        {
            if (string.IsNullOrEmpty(setCode) || setCode.Length < 3)
                return setCode;

            string prefix = setCode.Substring(0, setCode.Length - 3); // before last 3 chars
            string suffix = setCode.Substring(setCode.Length - 3);    // last 3 chars

            // suffix example: A01, AA1, AB1, AC1 ...
            char first = suffix[0]; // 'A'
            char middle = suffix[1]; // '0' or 'A', 'B', etc.
            char last = suffix[2];   // '1'

            // Increment the middle character
            if (middle == '0')
                middle = 'A';
            else if (middle >= 'A' && middle < 'Z')
                middle = (char)(middle + 1);
            else if (middle == 'Z')
                middle = 'A'; // wrap around if needed

            // Build the new suffix
            suffix = $"{first}{middle}{last}";

            return prefix + suffix;
        }


        //save BBS
        [HttpGet]
        [Route("/deleteLibItem_bpc/{CustomerCode}/{ProjectCode}/{JobID}")]
        //[ValidateAntiForgeryHeader]
        public ActionResult deleteLibItem(string CustomerCode, string ProjectCode, int JobID)
        {
            var lErrorMsg = "Successfully saved.";
            var lReturn = true;
            var lTemplate = true;

            try
            {
                db.Database.ExecuteSqlCommand("DELETE FROM OESBPCDetails " +
                "WHERE CustomerCode = '" + CustomerCode + "' " +
                "AND ProjectCode = '" + ProjectCode + "' " +
                "AND Template = " + (lTemplate == true ? 1 : 0) + " " +
                "AND JobID = " + JobID.ToString() + " ");

                db.Database.ExecuteSqlCommand("DELETE FROM OESBPCCageBars " +
                "WHERE CustomerCode = '" + CustomerCode + "' " +
                "AND ProjectCode = '" + ProjectCode + "' " +
                "AND Template = " + (lTemplate == true ? 1 : 0) + " " +
                "AND JobID = " + JobID.ToString() + " ");

                db.Database.ExecuteSqlCommand("DELETE FROM OESBPCTemplates " +
                "WHERE CustomerCode = '" + CustomerCode + "' " +
                "AND ProjectCode = '" + ProjectCode + "' " +
                "AND Template = " + (lTemplate == true ? 1 : 0) + " " +
                "AND JobID = " + JobID.ToString() + " ");

                db.Database.ExecuteSqlCommand("DELETE FROM OESBPCJobAdvice " +
                "WHERE CustomerCode = '" + CustomerCode + "' " +
                "AND ProjectCode = '" + ProjectCode + "' " +
                "AND Template = " + (lTemplate == true ? 1 : 0) + " " +
                "AND JobID = " + JobID.ToString() + " ");

            }
            catch (Exception ex)
            {
                lErrorMsg = ex.Message;
                lReturn = false;
            }

            //return Json(new { success = lReturn, responseText = lErrorMsg }, JsonRequestBehavior.AllowGet);
            return Ok(lReturn);
        }

        [HttpPost]
        [Route("/genBPCSAPMaterialCode_bpc")]
        string genBPCSAPMaterialCode(BPCDetailsModels pBPCDetails)
        {
            // get SAP Material FFO_BM ID
            pBPCDetails.sr_link_lapping = pBPCDetails.sr_link_lapping ?? 10;
            pBPCDetails.cr_link_lapping = pBPCDetails.cr_link_lapping ?? 51;
            string lDia = pBPCDetails.pile_dia.ToString();
            while (lDia.Length < 4)
            {
                lDia = "0" + lDia;
            }

            string lStd = "N";
            int liVar = 0;
            int.TryParse(pBPCDetails.cage_length, out liVar);
            if (liVar == 12000)
            {
                lStd = "S";
            }

            string lBendBar = "S";
            if (pBPCDetails.main_bar_shape == "Crank-Top" || pBPCDetails.main_bar_shape == "Crank" || pBPCDetails.main_bar_shape == "Crank-End")
            {
                lBendBar = "B";
            }
            else if (pBPCDetails.main_bar_shape == "Crank-Both")
            {
                lBendBar = "X";
            }

            string lType = "FOC";
            if (pBPCDetails.pile_type == "Micro-Pile")
            {
                lType = "FOM";
            }
            else
            {
                if (pBPCDetails.coupler_end == "No-Coupler" && pBPCDetails.coupler_top == "No-Coupler")
                {
                    if (pBPCDetails.pile_type == "Single-Layer" && pBPCDetails.main_bar_arrange == "Single")
                    {
                        lType = "FOC";
                    }
                    else
                    {
                        lType = "FOL";
                    }

                }
                else
                {
                    lType = "FOS";
                }
            }
            string lSAPMaterail = lType + "_" + lStd + "_" + lDia.Substring(0, 2) + lBendBar;

            return lSAPMaterail;
        }

        [HttpPost]
        [Route("/genBPCSAPMaterialCode_bpcHMI")]
        string genBPCSAPMaterialCodeHMI(BPCDetailsModels pBPCDetails)
        {
            var SAPMaterialCode = "BPC_";
            string BPCType = pBPCDetails.BPC_Type==null?"FBP": pBPCDetails.BPC_Type;
            SAPMaterialCode += BPCType;
            string Layer = pBPCDetails.pile_type== "Single-Layer" ? "1" : "2";
            SAPMaterialCode += Layer;
            if (pBPCDetails.coupler_end != "No-Coupler" && pBPCDetails.coupler_top != "No-Coupler")
            {
                SAPMaterialCode += "C";
            }        

           return SAPMaterialCode;
        }


        [HttpGet]
        [Route("/genBPCSetCode_bpc/{pID}")]
        string genBPCSetCode(BPCDetailsModels pBPCdetails, List<BPCDetailsModels> pAllBPCdetails, int pID)
        {
            pBPCdetails.sr_link_lapping = pBPCdetails.sr_link_lapping ?? 10;
            pBPCdetails.cr_link_lapping = pBPCdetails.cr_link_lapping ?? 51;
            string lSetCode = "";
            var lCoverCode = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "P" };
            var lCover = new int[] { 30, 35, 40, 45, 50, 55, 60, 65, 70, 75, 80, 85, 90, 95, 100 };
            string lPileDia1 = "000";
            string lCover2 = "A";
            string lMainBarsCT3 = "16";
            string lMainBarGrade4 = "H";
            string lMainBarDia5 = "32";
            string lLinkGrade6 = "H";
            string lLinkDia7 = "10";
            string lLinkSpacing8 = "20";
            string lCageLength9 = "12";
            string lPattern10 = "A";
            string lSeq11 = "00";
            int liVar = 0;
            string lsVar = "";

            var lCmd = new SqlCommand();
            SqlDataReader lRst;
            var lNDSCon = new SqlConnection();

            //Pile Dia
            liVar = (int)Math.Round((double)pBPCdetails.pile_dia / 10);
            lPileDia1 = liVar.ToString("D3");

            //Cover
            liVar = (int)Math.Round((double)(pBPCdetails.pile_dia - pBPCdetails.cage_dia) / 2);
            for (int i = 0; i < lCover.Length; i++)
            {
                if (lCover[i] == liVar)
                {
                    lCover2 = lCoverCode[i];
                    break;
                }
            }

            //Main Bar
            var lPileType = pBPCdetails.pile_type;
            var lMainBarType = pBPCdetails.main_bar_type;
            var lMainBarArrange = pBPCdetails.main_bar_arrange;

            if (lPileType == "Single-Layer")
            {
                if (lMainBarType == "Single")
                {
                    if (lMainBarArrange == "Single")
                    {
                        //lVar = "Single Layer\rSeparated Bars";
                        liVar = 0;
                        int.TryParse(pBPCdetails.main_bar_ct, out liVar);
                        lMainBarsCT3 = liVar.ToString("D2");
                        lMainBarGrade4 = pBPCdetails.main_bar_grade.Trim();
                        lMainBarDia5 = (pBPCdetails.main_bar_dia.Split(','))[0].Trim();
                    }
                    else if (lMainBarArrange == "Side-By-Side")
                    {
                        //lVar = "Single Layer\rSide-By-Side Bundled Bars";
                        liVar = 0;
                        int.TryParse(pBPCdetails.main_bar_ct, out liVar);
                        lMainBarsCT3 = liVar.ToString("D2");
                        lMainBarGrade4 = "M";
                        lMainBarDia5 = "04";
                    }
                    else if (lMainBarArrange == "In-Out")
                    {
                        //lVar = "Single Layer\rIn-Out Bundled Bars";
                        liVar = 0;
                        int.TryParse(pBPCdetails.main_bar_ct, out liVar);
                        lMainBarsCT3 = liVar.ToString("D2");
                        lMainBarGrade4 = "M";
                        lMainBarDia5 = "03";
                    }
                    else
                    {
                        //lVar = "Single Layer\rComplex Bundled Bars";
                        liVar = 0;
                        int.TryParse(pBPCdetails.main_bar_ct, out liVar);
                        lMainBarsCT3 = liVar.ToString("D2");
                        lMainBarGrade4 = pBPCdetails.main_bar_grade.Trim();
                        lMainBarDia5 = (pBPCdetails.main_bar_dia.Split(','))[0].Trim();
                    }
                }
                if (lMainBarType == "Mixed")
                {
                    if (lMainBarArrange == "Single")
                    {
                        //lVar = "Single Layer\rMixed Bars";
                        var lCRArr = pBPCdetails.main_bar_ct.Split(',');
                        int lBarCT = 0;
                        if (lCRArr.Length > 0)
                        {
                            liVar = 0;
                            int.TryParse(lCRArr[0], out liVar);
                            lBarCT = lBarCT + liVar;
                        }
                        if (lCRArr.Length > 1)
                        {
                            liVar = 0;
                            int.TryParse(lCRArr[1], out liVar);
                            lBarCT = lBarCT + liVar;
                        }
                        lMainBarsCT3 = lBarCT.ToString("D2");
                        lMainBarGrade4 = pBPCdetails.main_bar_grade.Trim();
                        lMainBarDia5 = (pBPCdetails.main_bar_dia.Split(','))[0].Trim();
                    }
                    else if (lMainBarArrange == "Side-By-Side")
                    {
                        //lVar = "Single Layer\rSide By Side Bundled\rMixed Bars";
                        var lCRArr = pBPCdetails.main_bar_ct.Split(',');
                        int lBarCT = 0;
                        if (lCRArr.Length > 0)
                        {
                            liVar = 0;
                            int.TryParse(lCRArr[0], out liVar);
                            lBarCT = lBarCT + liVar;
                        }
                        if (lCRArr.Length > 1)
                        {
                            liVar = 0;
                            int.TryParse(lCRArr[1], out liVar);
                            lBarCT = lBarCT + liVar;
                        }
                        lMainBarsCT3 = lBarCT.ToString("D2");
                        lMainBarGrade4 = "M";
                        lMainBarDia5 = "04";
                    }
                    else if (lMainBarArrange == "In-Out")
                    {
                        //lVar = "Single Layer\rIn-Out Bundled\rMixed Bars";
                        var lCRArr = pBPCdetails.main_bar_ct.Split(',');
                        int lBarCT = 0;
                        if (lCRArr.Length > 0)
                        {
                            liVar = 0;
                            int.TryParse(lCRArr[0], out liVar);
                            lBarCT = lBarCT + liVar;
                        }
                        if (lCRArr.Length > 1)
                        {
                            liVar = 0;
                            int.TryParse(lCRArr[1], out liVar);
                            lBarCT = lBarCT + liVar;
                        }
                        lMainBarsCT3 = lBarCT.ToString("D2");
                        lMainBarGrade4 = "M";
                        lMainBarDia5 = "03";
                    }
                    else
                    {
                        //lVar = "Single Layer\rComplex Bundled\rMixed Bars";
                    }
                }
            }
            else if (lPileType == "Micro-Pile")
            {
                liVar = 0;
                int.TryParse(pBPCdetails.main_bar_ct, out liVar);
                lMainBarsCT3 = liVar.ToString("D2");
                lMainBarGrade4 = "M";
                lMainBarDia5 = "09";
            }
            else
            {
                if (lMainBarArrange == "Single")
                {
                    //lVar = "Double Layer\rSeparated Bars";
                    var lCRArr = pBPCdetails.main_bar_ct.Split(',');
                    int lBarCT = 0;
                    if (lCRArr.Length > 0)
                    {
                        liVar = 0;
                        int.TryParse(lCRArr[0], out liVar);
                        lBarCT = lBarCT + liVar;
                    }
                    if (lCRArr.Length > 1)
                    {
                        liVar = 0;
                        int.TryParse(lCRArr[1], out liVar);
                        lBarCT = lBarCT + liVar;
                    }
                    lMainBarsCT3 = lBarCT.ToString("D2");
                    var lBarDiaArr = pBPCdetails.main_bar_dia.Split(',');
                    int lBarDia1 = 0;
                    int lBarDia2 = 0;
                    if (lBarDiaArr.Length > 0)
                    {
                        int.TryParse(lBarDiaArr[0], out lBarDia1);
                    }
                    if (lBarDiaArr.Length > 1)
                    {
                        int.TryParse(lBarDiaArr[1], out lBarDia2);
                    }
                    if (lBarDia1 == lBarDia2)
                    {
                        lMainBarGrade4 = "M";
                        lMainBarDia5 = "07";
                    }
                    else
                    {
                        lMainBarGrade4 = "M";
                        lMainBarDia5 = "08";
                    }

                }
                else if (lMainBarArrange == "Side-By-Side")
                {
                    //lVar = "Double Layer\rSide By Side Bundled Bars";
                    var lCRArr = pBPCdetails.main_bar_ct.Split(',');
                    int lBarCT = 0;
                    if (lCRArr.Length > 0)
                    {
                        liVar = 0;
                        int.TryParse(lCRArr[0], out liVar);
                        lBarCT = lBarCT + liVar;
                    }
                    if (lCRArr.Length > 1)
                    {
                        liVar = 0;
                        int.TryParse(lCRArr[1], out liVar);
                        lBarCT = lBarCT + liVar;
                    }
                    lMainBarsCT3 = lBarCT.ToString("D2");
                    var lBarDiaArr = pBPCdetails.main_bar_dia.Split(',');
                    int lBarDia1 = 0;
                    int lBarDia2 = 0;
                    if (lBarDiaArr.Length > 0)
                    {
                        int.TryParse(lBarDiaArr[0], out lBarDia1);
                    }
                    if (lBarDiaArr.Length > 1)
                    {
                        int.TryParse(lBarDiaArr[1], out lBarDia2);
                    }
                    if (lBarDia1 == lBarDia2)
                    {
                        lMainBarGrade4 = "M";
                        lMainBarDia5 = "05";
                    }
                    else
                    {
                        lMainBarGrade4 = "M";
                        lMainBarDia5 = "06";
                    }
                }
                else
                {
                    //lVar = "Double Layer\rComplex Bundled Bars";
                    var lCRArr = pBPCdetails.main_bar_ct.Split(',');
                    int lBarCT = 0;
                    if (lCRArr.Length > 0)
                    {
                        liVar = 0;
                        int.TryParse(lCRArr[0], out liVar);
                        lBarCT = lBarCT + liVar;
                    }
                    if (lCRArr.Length > 1)
                    {
                        liVar = 0;
                        int.TryParse(lCRArr[1], out liVar);
                        lBarCT = lBarCT + liVar;
                    }
                    lMainBarsCT3 = lBarCT.ToString("D2");
                    var lBarDiaArr = pBPCdetails.main_bar_dia.Split(',');
                    int lBarDia1 = 0;
                    int lBarDia2 = 0;
                    if (lBarDiaArr.Length > 0)
                    {
                        int.TryParse(lBarDiaArr[0], out lBarDia1);
                    }
                    if (lBarDiaArr.Length > 1)
                    {
                        int.TryParse(lBarDiaArr[1], out lBarDia2);
                    }
                    if (lBarDia1 >= lBarDia2)
                    {
                        lMainBarGrade4 = pBPCdetails.main_bar_grade.Trim();
                        lMainBarDia5 = lBarDia1.ToString("D2");
                    }
                    else
                    {
                        lMainBarGrade4 = pBPCdetails.main_bar_grade.Trim();
                        lMainBarDia5 = lBarDia2.ToString("D2");
                    }
                }

            }

            // Spiral Link

            var lMainBarShape = pBPCdetails.main_bar_shape.Trim();
            var lSLType = pBPCdetails.spiral_link_type.Trim();
            var lSLGrade = pBPCdetails.spiral_link_grade.Trim();
            var lSLDia = pBPCdetails.spiral_link_dia.Trim();
            var lSLSpacing = pBPCdetails.spiral_link_spacing.Trim();
            var lCouplerEnd = pBPCdetails.coupler_end;
            var lCouplerTop = pBPCdetails.coupler_top;

            if (lCouplerEnd == null) lCouplerEnd = "";
            if (lCouplerTop == null) lCouplerTop = "";
            lCouplerEnd = lCouplerEnd.Trim();
            lCouplerTop = lCouplerTop.Trim();

            if (lPileType == "Micro-Pile")
            {
                lLinkGrade6 = lSLGrade;
                lLinkDia7 = lSLDia;

                liVar = 0;
                int.TryParse(lSLSpacing, out liVar);
                lLinkSpacing8 = ((int)Math.Round((double)liVar / 10)).ToString("D2");
            }
            else
            {
                if ((lCouplerEnd == "" || lCouplerEnd == "No-Coupler") && (lCouplerTop == "" || lCouplerTop == "No-Coupler"))
                {
                    if (lSLType.IndexOf("Twin") >= 0)
                    {
                        lLinkGrade6 = "S";
                        lLinkDia7 = "LC";
                        lLinkSpacing8 = "13";
                    }
                    else
                    {
                        if (lMainBarShape == "Straight")
                        {
                            if (lSLType == "1 Spacing")
                            {
                                lLinkGrade6 = lSLGrade;
                                lLinkDia7 = lSLDia;

                                liVar = 0;
                                int.TryParse(lSLSpacing, out liVar);
                                lLinkSpacing8 = ((int)Math.Round((double)liVar / 10)).ToString("D2");
                            }
                            else
                            {
                                lLinkGrade6 = "S";
                                lLinkDia7 = "LS";
                                lLinkSpacing8 = "02";
                            }

                        }
                        else if (lMainBarShape == "Crank-Both")
                        {
                            if (lSLType == "1 Spacing")
                            {
                                lLinkGrade6 = "S";
                                lLinkDia7 = "LC";
                                lLinkSpacing8 = "05";
                            }
                            else
                            {
                                lLinkGrade6 = "S";
                                lLinkDia7 = "LC";
                                lLinkSpacing8 = "06";
                            }

                        }
                        else
                        {
                            if (lSLType == "1 Spacing")
                            {
                                //lLinkGrade6 = "S";
                                //lLinkDia7 = "LC";
                                //lLinkSpacing8 = "03";

                                lLinkGrade6 = lSLGrade;
                                lLinkDia7 = lSLDia;

                                liVar = 0;
                                int.TryParse(lSLSpacing, out liVar);
                                lLinkSpacing8 = ((int)Math.Round((double)liVar / 10)).ToString("D2");
                            }
                            else
                            {
                                lLinkGrade6 = "S";
                                lLinkDia7 = "LC";
                                lLinkSpacing8 = "04";
                            }
                        }
                    }
                }
                else
                {
                    // with vertical coupler
                    if (lMainBarShape == "Straight")
                    {
                        lLinkGrade6 = "S";
                        lLinkDia7 = "LS";
                        lLinkSpacing8 = "20";
                    }
                    else if (lMainBarShape == "Crank-Both")
                    {
                        lLinkGrade6 = "S";
                        lLinkDia7 = "LX";
                        lLinkSpacing8 = "22";
                    }
                    else
                    {
                        lLinkGrade6 = "S";
                        lLinkDia7 = "LC";
                        lLinkSpacing8 = "21";
                    }
                }
            }
            //Cage Length
            lCageLength9 = pBPCdetails.cage_length.Trim().Substring(0, 2);

            //Pattern
            var lStartlength = pBPCdetails.lap_length;
            var lEndlength = pBPCdetails.end_length;

            if (lMainBarShape == "Straight")
            {
                if (lStartlength > 700 && lEndlength > 700)
                {
                    lPattern10 = "B";
                }
                else
                {
                    lPattern10 = "A";
                }
            }
            else
            {
                lPattern10 = "C";
            }

            // Cage Seq
            //lSeq11 = pBPCdetails.cage_id.ToString("D2");


            var lProcessObj = new ProcessController();
            if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
            {
                lSetCode = lPileDia1 + lCover2 + lMainBarsCT3 + lMainBarGrade4 + lMainBarDia5 +
                                lLinkGrade6 + lLinkDia7 + lLinkSpacing8 + "-" +
                                lCageLength9 + lPattern10;

                lCmd.CommandText =
                "SELECT set_code, lap_length, end_length " +
                "FROM dbo.OESBPCDetails " +
                "WHERE CustomerCode = '" + pBPCdetails.CustomerCode + "' " +
                "AND ProjectCode = '" + pBPCdetails.ProjectCode + "' " +
                "AND Template = 0 " +
                "AND set_code like '" + lSetCode + "%' " +
                "AND lap_length = " + pBPCdetails.lap_length.ToString() + " " +
                "AND end_length = " + pBPCdetails.end_length.ToString() + " " +
                "AND cage_length = " + pBPCdetails.cage_length.ToString() + " " +
                "GROUP BY set_code, lap_length, end_length " +
                "ORDER BY set_code ";

                liVar = 0;
                lCmd.Connection = lNDSCon;
                lCmd.CommandTimeout = 300;
                lRst = lCmd.ExecuteReader();
                if (lRst.HasRows)
                {
                    if (lRst.Read())
                    {
                        liVar = 1;
                        lSetCode = lRst.GetString(0).Trim();
                    }
                }
                lRst.Close();
                if (liVar == 0)
                {
                    lsVar = "";

                    lCmd.CommandText =
                    "SELECT isNULL(MAX(set_code),'') " +
                    "FROM dbo.OESBPCDetails " +
                    "WHERE CustomerCode = '" + pBPCdetails.CustomerCode + "' " +
                    "AND ProjectCode = '" + pBPCdetails.ProjectCode + "' " +
                    "AND Template = 0 " +
                    "AND set_code like '" + lSetCode + "%' ";

                    lCmd.Connection = lNDSCon;
                    lCmd.CommandTimeout = 300;
                    lRst = lCmd.ExecuteReader();
                    if (lRst.HasRows)
                    {
                        if (lRst.Read())
                        {
                            lsVar = lRst.GetString(0).Trim();
                        }
                    }
                    if (lsVar == "" || lsVar.Length < 20)
                    {
                        lSeq11 = "01";
                    }
                    else
                    {
                        liVar = 0;
                        int.TryParse(lsVar.Substring(18, 2), out liVar);
                        liVar = liVar + 1;
                        lSeq11 = liVar.ToString("D2");
                    }

                    lSetCode = lPileDia1 + lCover2 + lMainBarsCT3 + lMainBarGrade4 + lMainBarDia5 +
                        lLinkGrade6 + lLinkDia7 + lLinkSpacing8 + "-" +
                        lCageLength9 + lPattern10 + lSeq11;
                }

                lProcessObj.CloseNDSConnection(ref lNDSCon);

                //Check internal Set Code
                if (pAllBPCdetails.Count > 1)
                {
                    for (int i = 0; i < pAllBPCdetails.Count; i++)
                    {
                        if (i != pID && pAllBPCdetails[i].set_code != null && pAllBPCdetails[i].set_code != "" && pAllBPCdetails[i].set_code.Length >= 20)
                        {
                            if (lSetCode == pAllBPCdetails[i].set_code &&
                            (pBPCdetails.lap_length != pAllBPCdetails[i].lap_length ||
                            pBPCdetails.end_length != pAllBPCdetails[i].end_length ||
                            pBPCdetails.cage_length != pAllBPCdetails[i].cage_length ||
                            pBPCdetails.cage_location != pAllBPCdetails[i].cage_location))
                            {
                                liVar = 0;
                                int.TryParse(pAllBPCdetails[i].set_code.Substring(18, 2), out liVar);
                                liVar = liVar + 1;
                                lSeq11 = liVar.ToString("D2");

                                lSetCode = lPileDia1 + lCover2 + lMainBarsCT3 + lMainBarGrade4 + lMainBarDia5 +
                                    lLinkGrade6 + lLinkDia7 + lLinkSpacing8 + "-" +
                                    lCageLength9 + lPattern10 + lSeq11;

                            }
                        }
                    }
                }
            }
            if (pBPCdetails.set_code != null && pBPCdetails.set_code != "" && pBPCdetails.set_code.Trim().Length >= 2)
            {
                pBPCdetails.set_code = pBPCdetails.set_code.Trim();
                string lOldSetCodeLast = pBPCdetails.set_code.Substring(pBPCdetails.set_code.Length - 2);
                int lOldSetCodeNum = 0;
                int lNewSetCodeNum = 0;
                int.TryParse(lOldSetCodeLast, out lOldSetCodeNum);
                int.TryParse(lSeq11, out lNewSetCodeNum);

                if (lOldSetCodeNum == 0)
                {
                    lSetCode = lPileDia1 + lCover2 + lMainBarsCT3 + lMainBarGrade4 + lMainBarDia5 +
                        lLinkGrade6 + lLinkDia7 + lLinkSpacing8 + "-" +
                        lCageLength9 + lPattern10 + lOldSetCodeLast;
                }
                else
                {
                    if (lOldSetCodeNum > 0 && lOldSetCodeNum > lNewSetCodeNum)
                    {
                        lSetCode = lPileDia1 + lCover2 + lMainBarsCT3 + lMainBarGrade4 + lMainBarDia5 +
                            lLinkGrade6 + lLinkDia7 + lLinkSpacing8 + "-" +
                            lCageLength9 + lPattern10 + lOldSetCodeLast;
                    }
                }
            }
            lProcessObj = null;

            return lSetCode;
        }

        [HttpPost]
        [Route("/SaveRebarDetails_bpc")]
        List<BPCCageBarsModels> SaveRebarDetails(BPCDetailsModels pBPCModels)
        {
            var lRebarList = new List<BPCCageBarsModels>();
            var lRebar = new BPCCageBarsModels();
            int lBarID = 1;
            int lBarSort = 1;

            var lProject = db.Project.Find(pBPCModels.CustomerCode, pBPCModels.ProjectCode);

            //Main Bar Details
            var lPileType = pBPCModels.pile_type;
            var lCageDia = pBPCModels.cage_dia;
            var lMainBarType = pBPCModels.main_bar_type;
            var lMainBarArrange = pBPCModels.main_bar_arrange;
            var lMainBarCT = pBPCModels.main_bar_ct;
            var lMainBarShape = pBPCModels.main_bar_shape;
            var lMainBarDia = pBPCModels.main_bar_dia;
            var lCageLength = pBPCModels.cage_length;
            var lTopCrankHT = pBPCModels.crank_height_top;
            var lEndCrankHT = pBPCModels.crank_height_end;
            var lTopCrank2HT = pBPCModels.crank2_height_top;
            var lEndCrank2HT = pBPCModels.crank2_height_end;
            var lMainBarLenLayer2 = pBPCModels.mainbar_length_2layer;
            var PdfRemark = pBPCModels.pdf_remark;
            pBPCModels.sr_link_lapping = pBPCModels.sr_link_lapping ?? 10;
            pBPCModels.cr_link_lapping = pBPCModels.cr_link_lapping ?? 51;
            var sr_link_lapping = pBPCModels.sr_link_lapping ?? 10;
            var cr_link_lapping = pBPCModels.cr_link_lapping ?? 51;

            if ((lPileType == "Single-Layer" || lPileType == "Micro-Pile") && lMainBarType == "Single")
            {
                if (lMainBarArrange == "In-Out" || lMainBarArrange == "Side-By-Side")
                {
                    var lMainBarLen = lCageLength;

                    int l2MainBarLen = int.Parse(pBPCModels.cage_length);
                    if (pBPCModels.mainbar_length_2layer > 0 && pBPCModels.mainbar_length_2layer < l2MainBarLen)
                    {
                        l2MainBarLen = pBPCModels.mainbar_length_2layer;
                    }

                    if (lMainBarShape == "Crank-Top" || lMainBarShape == "Crank")
                    {
                        lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "041",
                                   "MB", "MB", int.Parse(pBPCModels.main_bar_dia), int.Parse(pBPCModels.cage_length), int.Parse(pBPCModels.main_bar_ct) / 2,
                                   pBPCModels.lap_length, 10 * int.Parse(pBPCModels.main_bar_dia),
                                   int.Parse(pBPCModels.cage_length) - pBPCModels.lap_length - 10 * int.Parse(pBPCModels.main_bar_dia),
                                   pBPCModels.crank_height_top, 0, 0, 0);
                        lRebarList.Add(lRebar);
                        lBarID++;
                        lBarSort++;

                        //Second layer
                        if (l2MainBarLen != int.Parse(pBPCModels.cage_length))
                        {
                            if (pBPCModels.mainbar_location_2layer >= pBPCModels.lap_length)
                            {
                                lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "020",
                                           "MM", "MM", int.Parse(pBPCModels.main_bar_dia), l2MainBarLen, int.Parse(pBPCModels.main_bar_ct) / 2,
                                           l2MainBarLen,
                                           0, 0, 0, 0, 0, 0);
                            }
                            else
                            {
                                lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "041",
                                           "MM", "MM", int.Parse(pBPCModels.main_bar_dia), l2MainBarLen, int.Parse(pBPCModels.main_bar_ct) / 2,
                                           pBPCModels.lap_length - pBPCModels.mainbar_location_2layer, 10 * int.Parse(pBPCModels.main_bar_dia),
                                           l2MainBarLen - pBPCModels.mainbar_location_2layer - pBPCModels.lap_length - 10 * int.Parse(pBPCModels.main_bar_dia),
                                           pBPCModels.crank_height_top, 0, 0, 0);
                            }
                        }
                        else
                        {
                            lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "041",
                                       "MM", "MM", int.Parse(pBPCModels.main_bar_dia), int.Parse(pBPCModels.cage_length), int.Parse(pBPCModels.main_bar_ct) / 2,
                                       pBPCModels.lap_length, 10 * int.Parse(pBPCModels.main_bar_dia),
                                       int.Parse(pBPCModels.cage_length) - pBPCModels.lap_length - 10 * int.Parse(pBPCModels.main_bar_dia),
                                       pBPCModels.crank_height_top, 0, 0, 0);
                        }
                        lRebarList.Add(lRebar);
                        lBarID++;
                        lBarSort++;
                    }
                    else if (lMainBarShape == "Crank-End")
                    {
                        lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "041",
                                   "MB", "MB", int.Parse(pBPCModels.main_bar_dia), int.Parse(pBPCModels.cage_length), int.Parse(pBPCModels.main_bar_ct) / 2,
                                   pBPCModels.end_length, 10 * int.Parse(pBPCModels.main_bar_dia),
                                   int.Parse(pBPCModels.cage_length) - pBPCModels.end_length - 10 * int.Parse(pBPCModels.main_bar_dia),
                                   pBPCModels.crank_height_end, 0, 0, 0);
                        lRebarList.Add(lRebar);
                        lBarID++;
                        lBarSort++;

                        //Second layer
                        if (l2MainBarLen != int.Parse(pBPCModels.cage_length))
                        {
                            if (pBPCModels.mainbar_location_2layer + l2MainBarLen <= int.Parse(pBPCModels.cage_length) - pBPCModels.end_length)
                            {
                                lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "020",
                                           "MM", "MM", int.Parse(pBPCModels.main_bar_dia), l2MainBarLen, int.Parse(pBPCModels.main_bar_ct) / 2,
                                           l2MainBarLen,
                                           0, 0, 0, 0, 0, 0);
                            }
                            else
                            {
                                lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "041",
                                           "MM", "MM", int.Parse(pBPCModels.main_bar_dia), l2MainBarLen, int.Parse(pBPCModels.main_bar_ct) / 2,
                                           pBPCModels.end_length - (int.Parse(pBPCModels.cage_length) - pBPCModels.mainbar_location_2layer - l2MainBarLen), 10 * int.Parse(pBPCModels.main_bar_dia),
                                           int.Parse(pBPCModels.cage_length) - pBPCModels.end_length - pBPCModels.mainbar_location_2layer - 10 * int.Parse(pBPCModels.main_bar_dia),
                                           pBPCModels.crank_height_top, 0, 0, 0);
                            }
                        }
                        else
                        {
                            lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "041",
                                   "MM", "MM", int.Parse(pBPCModels.main_bar_dia), l2MainBarLen, int.Parse(pBPCModels.main_bar_ct) / 2,
                                   pBPCModels.end_length, 10 * int.Parse(pBPCModels.main_bar_dia),
                                   l2MainBarLen - pBPCModels.end_length - 10 * int.Parse(pBPCModels.main_bar_dia),
                                   pBPCModels.crank_height_end, 0, 0, 0);
                        }
                        lRebarList.Add(lRebar);
                        lBarID++;
                        lBarSort++;
                    }
                    else if (lMainBarShape == "Crank-Both")
                    {
                        if (pBPCModels.crank_height_top == pBPCModels.crank_height_end)
                        {
                            lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "043",
                                       "MB", "MB", int.Parse(pBPCModels.main_bar_dia), int.Parse(pBPCModels.cage_length), int.Parse(pBPCModels.main_bar_ct) / 2,
                                       pBPCModels.lap_length, 10 * int.Parse(pBPCModels.main_bar_dia),
                                       int.Parse(pBPCModels.cage_length) - pBPCModels.lap_length - 2 * 10 * int.Parse(pBPCModels.main_bar_dia) - pBPCModels.end_length,
                                       pBPCModels.crank_height_top,
                                       pBPCModels.end_length,
                                       (int)Math.Round(180 / Math.PI * Math.Asin((double)(pBPCModels.crank_height_top - int.Parse(pBPCModels.main_bar_dia)) / (10 * int.Parse(pBPCModels.main_bar_dia)))),
                                       0);
                            lRebarList.Add(lRebar);
                            lBarID++;
                            lBarSort++;

                            //Second layer
                            if (l2MainBarLen != int.Parse(pBPCModels.cage_length))
                            {
                                if (pBPCModels.mainbar_location_2layer >= pBPCModels.lap_length && pBPCModels.mainbar_location_2layer + l2MainBarLen <= int.Parse(pBPCModels.cage_length) - pBPCModels.end_length)
                                {
                                    lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "020",
                                               "MM", "MM", int.Parse(pBPCModels.main_bar_dia), l2MainBarLen, int.Parse(pBPCModels.main_bar_ct) / 2,
                                               l2MainBarLen,
                                               0, 0, 0, 0, 0, 0);
                                }
                                else if (pBPCModels.mainbar_location_2layer + l2MainBarLen <= int.Parse(pBPCModels.cage_length) - pBPCModels.end_length)
                                {
                                    //crank at top
                                    lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "041",
                                               "MM", "MM", int.Parse(pBPCModels.main_bar_dia), l2MainBarLen, int.Parse(pBPCModels.main_bar_ct) / 2,
                                               pBPCModels.lap_length - pBPCModels.mainbar_location_2layer, 10 * int.Parse(pBPCModels.main_bar_dia),
                                               l2MainBarLen - pBPCModels.mainbar_location_2layer - pBPCModels.lap_length - 10 * int.Parse(pBPCModels.main_bar_dia),
                                               pBPCModels.crank_height_top, 0, 0, 0);
                                }
                                else if (pBPCModels.mainbar_location_2layer >= pBPCModels.lap_length && pBPCModels.mainbar_location_2layer + l2MainBarLen <= int.Parse(pBPCModels.cage_length) - pBPCModels.end_length)
                                {
                                    // crank at END
                                    lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "041",
                                               "MM", "MM", int.Parse(pBPCModels.main_bar_dia), l2MainBarLen, int.Parse(pBPCModels.main_bar_ct) / 2,
                                               pBPCModels.end_length - (int.Parse(pBPCModels.cage_length) - pBPCModels.mainbar_location_2layer - l2MainBarLen), 10 * int.Parse(pBPCModels.main_bar_dia),
                                               int.Parse(pBPCModels.cage_length) - pBPCModels.end_length - pBPCModels.mainbar_location_2layer - 10 * int.Parse(pBPCModels.main_bar_dia),
                                               pBPCModels.crank_height_top, 0, 0, 0);
                                }
                                else
                                {
                                    //crank at both
                                    lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "043",
                                           "MM", "MM", int.Parse(pBPCModels.main_bar_dia), l2MainBarLen, int.Parse(pBPCModels.main_bar_ct) / 2,
                                           pBPCModels.lap_length - pBPCModels.mainbar_location_2layer, 10 * int.Parse(pBPCModels.main_bar_dia),
                                           int.Parse(pBPCModels.cage_length) - pBPCModels.lap_length - 2 * 10 * int.Parse(pBPCModels.main_bar_dia) - pBPCModels.end_length,
                                           pBPCModels.crank_height_top,
                                           pBPCModels.end_length,
                                           (int)Math.Round(180 / Math.PI * Math.Asin((double)(pBPCModels.crank_height_top - int.Parse(pBPCModels.main_bar_dia)) / (10 * int.Parse(pBPCModels.main_bar_dia)))),
                                           0);
                                }
                            }
                            else
                            {
                                lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "043",
                                       "MM", "MM", int.Parse(pBPCModels.main_bar_dia), int.Parse(pBPCModels.cage_length), int.Parse(pBPCModels.main_bar_ct) / 2,
                                       pBPCModels.lap_length, 10 * int.Parse(pBPCModels.main_bar_dia),
                                       int.Parse(pBPCModels.cage_length) - pBPCModels.lap_length - 2 * 10 * int.Parse(pBPCModels.main_bar_dia) - pBPCModels.end_length,
                                       pBPCModels.crank_height_top,
                                       pBPCModels.end_length,
                                       (int)Math.Round(180 / Math.PI * Math.Asin((double)(pBPCModels.crank_height_top - int.Parse(pBPCModels.main_bar_dia)) / (10 * int.Parse(pBPCModels.main_bar_dia)))),
                                       0);
                            }
                            lRebarList.Add(lRebar);
                            lBarID++;
                            lBarSort++;
                        }
                        else
                        {
                            lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "43B",
                                       "MB", "MB", int.Parse(pBPCModels.main_bar_dia), int.Parse(pBPCModels.cage_length), int.Parse(pBPCModels.main_bar_ct) / 2,
                                       pBPCModels.lap_length,
                                       10 * int.Parse(pBPCModels.main_bar_dia),
                                       int.Parse(pBPCModels.cage_length) - pBPCModels.lap_length - 2 * 10 * int.Parse(pBPCModels.main_bar_dia) - pBPCModels.end_length,
                                       10 * int.Parse(pBPCModels.main_bar_dia),
                                       pBPCModels.end_length,
                                       pBPCModels.crank_height_top,
                                       pBPCModels.crank_height_end);
                            lRebarList.Add(lRebar);
                            lBarID++;
                            lBarSort++;

                            lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "43B",
                                       "MM", "MM", int.Parse(pBPCModels.main_bar_dia), int.Parse(pBPCModels.cage_length), int.Parse(pBPCModels.main_bar_ct) / 2,
                                       pBPCModels.lap_length,
                                       10 * int.Parse(pBPCModels.main_bar_dia),
                                       int.Parse(pBPCModels.cage_length) - pBPCModels.lap_length - 2 * 10 * int.Parse(pBPCModels.main_bar_dia) - pBPCModels.end_length,
                                       10 * int.Parse(pBPCModels.main_bar_dia),
                                       pBPCModels.end_length,
                                       pBPCModels.crank_height_top,
                                       pBPCModels.crank_height_end);
                            lRebarList.Add(lRebar);
                            lBarID++;
                            lBarSort++;
                        }
                    }
                    else
                    {
                        lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "020",
                                   "MB", "MB", int.Parse(pBPCModels.main_bar_dia), int.Parse(pBPCModels.cage_length), int.Parse(pBPCModels.main_bar_ct) / 2,
                                   int.Parse(pBPCModels.cage_length),
                                   0, 0, 0, 0, 0, 0);
                        lRebarList.Add(lRebar);

                        lBarID++;
                        lBarSort++;

                        lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "020",
                                   "MM", "MM", int.Parse(pBPCModels.main_bar_dia), l2MainBarLen, int.Parse(pBPCModels.main_bar_ct) / 2,
                                   l2MainBarLen,
                                   0, 0, 0, 0, 0, 0);
                        lRebarList.Add(lRebar);

                        lBarID++;
                        lBarSort++;
                    }

                }
                else
                {
                    var lMainBarLen = lCageLength;
                    if (lMainBarShape == "Crank-Top" || lMainBarShape == "Crank")
                    {
                        lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "041",
                                   "MB", "MB", int.Parse(pBPCModels.main_bar_dia), int.Parse(pBPCModels.cage_length), int.Parse(pBPCModels.main_bar_ct),
                                   pBPCModels.lap_length, 10 * int.Parse(pBPCModels.main_bar_dia),
                                   int.Parse(pBPCModels.cage_length) - pBPCModels.lap_length - 10 * int.Parse(pBPCModels.main_bar_dia),
                                   pBPCModels.crank_height_top, 0, 0, 0);
                        lRebarList.Add(lRebar);
                        lBarID++;
                        lBarSort++;
                    }
                    else if (lMainBarShape == "Crank-End")
                    {
                        lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "041",
                                   "MB", "MB", int.Parse(pBPCModels.main_bar_dia), int.Parse(pBPCModels.cage_length), int.Parse(pBPCModels.main_bar_ct),
                                   pBPCModels.end_length, 10 * int.Parse(pBPCModels.main_bar_dia),
                                   int.Parse(pBPCModels.cage_length) - pBPCModels.end_length - 10 * int.Parse(pBPCModels.main_bar_dia),
                                   pBPCModels.crank_height_end, 0, 0, 0);
                        lRebarList.Add(lRebar);
                        lBarID++;
                        lBarSort++;
                    }
                    else if (lMainBarShape == "Crank-Both")
                    {
                        if (pBPCModels.crank_height_top == pBPCModels.crank_height_end)
                        {
                            lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "043",
                                       "MB", "MB", int.Parse(pBPCModels.main_bar_dia), int.Parse(pBPCModels.cage_length), int.Parse(pBPCModels.main_bar_ct),
                                       pBPCModels.lap_length, 10 * int.Parse(pBPCModels.main_bar_dia),
                                       int.Parse(pBPCModels.cage_length) - pBPCModels.lap_length - 2 * 10 * int.Parse(pBPCModels.main_bar_dia) - pBPCModels.end_length,
                                       pBPCModels.crank_height_top,
                                       pBPCModels.end_length,
                                       (int)Math.Round(180 / Math.PI * Math.Asin((double)(pBPCModels.crank_height_top - int.Parse(pBPCModels.main_bar_dia)) / (10 * int.Parse(pBPCModels.main_bar_dia)))),
                                       0);
                            lRebarList.Add(lRebar);
                        }
                        else
                        {
                            lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "43B",
                                       "MB", "MB", int.Parse(pBPCModels.main_bar_dia), int.Parse(pBPCModels.cage_length), int.Parse(pBPCModels.main_bar_ct),
                                       pBPCModels.lap_length,
                                       10 * int.Parse(pBPCModels.main_bar_dia),
                                       int.Parse(pBPCModels.cage_length) - pBPCModels.lap_length - 2 * 10 * int.Parse(pBPCModels.main_bar_dia) - pBPCModels.end_length,
                                       10 * int.Parse(pBPCModels.main_bar_dia),
                                       pBPCModels.end_length,
                                       pBPCModels.crank_height_top,
                                       pBPCModels.crank_height_end);
                            lRebarList.Add(lRebar);
                        }
                        lBarID++;
                        lBarSort++;
                    }
                    else
                    {
                        lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "020",
                                   "MB", "MB", int.Parse(pBPCModels.main_bar_dia), int.Parse(pBPCModels.cage_length), int.Parse(pBPCModels.main_bar_ct),
                                   int.Parse(pBPCModels.cage_length),
                                   0, 0, 0, 0, 0, 0);
                        lRebarList.Add(lRebar);

                        lBarID++;
                        lBarSort++;
                    }
                }

            }
            else if (lPileType == "Single-Layer" && lMainBarType == "Mixed")
            {
                var lMainbarDiaArr = lMainBarDia.Split(',');
                var lMainbarCTArr = lMainBarCT.Split(',');
                var lMainBarLen = int.Parse(lCageLength);

                var lMainBarCT1 = 0;
                var lMainBarCT2 = 0;
                var lMainBarDia1 = 0;
                var lMainBarDia2 = 0;
                var lMainBarDiaMax = 0;

                var lCrankHeightTop1 = 0;
                var lCrankHeightTop2 = 0;
                var lCrankHeightEnd1 = 0;
                var lCrankHeightEnd2 = 0;

                if (lMainbarDiaArr.Length > 0 && lMainbarCTArr.Length > 0)
                {
                    lMainBarCT1 = int.Parse(lMainbarCTArr[0]);
                    lMainBarDia1 = int.Parse(lMainbarDiaArr[0]);
                }
                if (lMainbarDiaArr.Length > 1 && lMainbarCTArr.Length > 1)
                {
                    lMainBarCT2 = int.Parse(lMainbarCTArr[1]);
                    lMainBarDia2 = int.Parse(lMainbarDiaArr[1]);
                }

                lMainBarDiaMax = lMainBarDia1;
                if (lMainBarDia2 > lMainBarDiaMax)
                {
                    lMainBarDiaMax = lMainBarDia2;
                    lMainBarDia2 = lMainBarDia1;
                    lMainBarDia1 = lMainBarDiaMax;
                    var lCTvar = lMainBarCT2;
                    lMainBarCT2 = lMainBarCT1;
                    lMainBarCT1 = lCTvar;
                }

                lCrankHeightTop1 = pBPCModels.crank_height_top;
                lCrankHeightTop2 = pBPCModels.crank2_height_top == null ? (pBPCModels.crank_height_top > 0 ? pBPCModels.crank_height_top - (lMainBarDia1 - lMainBarDia2) : 0) : (int)pBPCModels.crank2_height_top;
                lCrankHeightEnd1 = pBPCModels.crank_height_end;
                lCrankHeightEnd2 = pBPCModels.crank2_height_end == null ? (pBPCModels.crank_height_end > 0 ? pBPCModels.crank_height_end - (lMainBarDia1 - lMainBarDia2) : 0) : (int)pBPCModels.crank2_height_end;

                if (lMainBarArrange == "In-Out" || lMainBarArrange == "Side-By-Side")
                {
                    int l2MainBarLen = int.Parse(pBPCModels.cage_length);
                    if (pBPCModels.mainbar_length_2layer > 0 && pBPCModels.mainbar_length_2layer < l2MainBarLen)
                    {
                        l2MainBarLen = pBPCModels.mainbar_length_2layer;
                    }

                    if (pBPCModels.bundle_same_type == "Y")
                    {
                        if (lMainBarShape == "Crank-Top" || lMainBarShape == "Crank")
                        {
                            lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "041",
                                       "MB", "MB1", lMainBarDia1, lMainBarLen, lMainBarCT1 / 2,
                                       pBPCModels.lap_length, 10 * lMainBarDia1,
                                       lMainBarLen - pBPCModels.lap_length - 10 * lMainBarDia1,
                                       lCrankHeightTop1, 0, 0, 0);
                            lRebarList.Add(lRebar);
                            lBarID++;
                            lBarSort++;

                            lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "041",
                                       "MB", "MB2", lMainBarDia2, lMainBarLen, lMainBarCT2 / 2,
                                       pBPCModels.lap_length, 10 * lMainBarDia2,
                                       lMainBarLen - pBPCModels.lap_length - 10 * lMainBarDia2,
                                      lCrankHeightTop2, 0, 0, 0);
                            lRebarList.Add(lRebar);
                            lBarID++;
                            lBarSort++;

                            //Second layer
                            //First main bars
                            if (l2MainBarLen != int.Parse(pBPCModels.cage_length))
                            {
                                if (pBPCModels.mainbar_location_2layer >= pBPCModels.lap_length)
                                {
                                    lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "020",
                                               "MM", "MM1", lMainBarDia1, l2MainBarLen, lMainBarCT1 / 2,
                                               l2MainBarLen,
                                               0, 0, 0, 0, 0, 0);
                                }
                                else
                                {
                                    lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "041",
                                               "MM", "MM1", lMainBarDia1, l2MainBarLen, lMainBarCT1 / 2,
                                               pBPCModels.lap_length - pBPCModels.mainbar_location_2layer, 10 * lMainBarDia1,
                                               l2MainBarLen - pBPCModels.mainbar_location_2layer - pBPCModels.lap_length - 10 * lMainBarDia1,
                                               lCrankHeightTop1, 0, 0, 0);
                                }
                            }
                            else
                            {
                                lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "041",
                                           "MM", "MM1", lMainBarDia1, int.Parse(pBPCModels.cage_length), lMainBarCT1 / 2,
                                           pBPCModels.lap_length, 10 * lMainBarDia1,
                                           int.Parse(pBPCModels.cage_length) - pBPCModels.lap_length - 10 * lMainBarDia1,
                                           lCrankHeightTop1, 0, 0, 0);
                            }
                            lRebarList.Add(lRebar);
                            lBarID++;
                            lBarSort++;

                            //Second layer
                            //Second main bars
                            if (l2MainBarLen != int.Parse(pBPCModels.cage_length))
                            {
                                if (pBPCModels.mainbar_location_2layer >= pBPCModels.lap_length)
                                {
                                    lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "020",
                                               "MM", "MM2", lMainBarDia2, l2MainBarLen, lMainBarCT2 / 2,
                                               l2MainBarLen,
                                               0, 0, 0, 0, 0, 0);
                                }
                                else
                                {
                                    lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "041",
                                               "MM", "MM2", lMainBarDia2, l2MainBarLen, lMainBarCT2 / 2,
                                               pBPCModels.lap_length - pBPCModels.mainbar_location_2layer, 10 * lMainBarDia1,
                                               l2MainBarLen + pBPCModels.mainbar_location_2layer - pBPCModels.lap_length - 10 * lMainBarDia1,
                                               lCrankHeightTop2, 0, 0, 0);
                                }
                            }
                            else
                            {
                                lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "041",
                                           "MM", "MM2", lMainBarDia2, int.Parse(pBPCModels.cage_length), lMainBarCT2 / 2,
                                           pBPCModels.lap_length, 10 * lMainBarDia1,
                                           int.Parse(pBPCModels.cage_length) - pBPCModels.lap_length - 10 * lMainBarDia1,
                                           lCrankHeightTop2, 0, 0, 0);
                            }
                            lRebarList.Add(lRebar);
                            lBarID++;
                            lBarSort++;
                        }
                        else if (lMainBarShape == "Crank-End")
                        {
                            lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "041",
                                       "MB", "MB1", lMainBarDia1, lMainBarLen, lMainBarCT1 / 2,
                                       pBPCModels.end_length, 10 * lMainBarDia1,
                                       lMainBarLen - pBPCModels.end_length - 10 * lMainBarDia1,
                                       lCrankHeightEnd1, 0, 0, 0);
                            lRebarList.Add(lRebar);
                            lBarID++;
                            lBarSort++;

                            lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "041",
                                       "MB", "MB2", lMainBarDia2, lMainBarLen, lMainBarCT2 / 2,
                                       pBPCModels.end_length, 10 * lMainBarDia2,
                                       lMainBarLen - pBPCModels.end_length - 10 * lMainBarDia2,
                                       lCrankHeightEnd2, 0, 0, 0);
                            lRebarList.Add(lRebar);
                            lBarID++;
                            lBarSort++;

                            //Second layer
                            //First Main bars
                            if (l2MainBarLen != int.Parse(pBPCModels.cage_length))
                            {
                                if (pBPCModels.mainbar_location_2layer + l2MainBarLen <= int.Parse(pBPCModels.cage_length) - pBPCModels.end_length)
                                {
                                    lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "020",
                                               "MM", "MM1", lMainBarDia1, l2MainBarLen, lMainBarCT1 / 2,
                                               l2MainBarLen,
                                               0, 0, 0, 0, 0, 0);
                                }
                                else
                                {
                                    lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "041",
                                               "MM", "MM1", lMainBarDia1, l2MainBarLen, lMainBarCT1 / 2,
                                               pBPCModels.end_length - (int.Parse(pBPCModels.cage_length) - pBPCModels.mainbar_location_2layer - l2MainBarLen), 10 * lMainBarDia1,
                                               int.Parse(pBPCModels.cage_length) - pBPCModels.end_length - pBPCModels.mainbar_location_2layer - 10 * lMainBarDia1,
                                               lCrankHeightEnd1, 0, 0, 0);
                                }
                            }
                            else
                            {
                                lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "041",
                                       "MM", "MM1", lMainBarDia1, l2MainBarLen, lMainBarCT1 / 2,
                                       pBPCModels.end_length, 10 * lMainBarDia1,
                                       int.Parse(pBPCModels.cage_length) - pBPCModels.end_length - 10 * lMainBarDia1,
                                       lCrankHeightEnd1, 0, 0, 0);
                            }
                            lRebarList.Add(lRebar);
                            lBarID++;
                            lBarSort++;

                            //Second layer
                            //Second Main bars
                            if (l2MainBarLen != int.Parse(pBPCModels.cage_length))
                            {
                                if (pBPCModels.mainbar_location_2layer + l2MainBarLen <= int.Parse(pBPCModels.cage_length) - pBPCModels.end_length)
                                {
                                    lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "020",
                                               "MM", "MM2", lMainBarDia2, l2MainBarLen, lMainBarCT2 / 2,
                                               l2MainBarLen,
                                               0, 0, 0, 0, 0, 0);
                                }
                                else
                                {
                                    lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "041",
                                               "MM", "MM2", lMainBarDia2, l2MainBarLen, lMainBarCT2 / 2,
                                               pBPCModels.end_length - (int.Parse(pBPCModels.cage_length) - pBPCModels.mainbar_location_2layer - l2MainBarLen), 10 * lMainBarDia1,
                                               int.Parse(pBPCModels.cage_length) - pBPCModels.end_length - pBPCModels.mainbar_location_2layer - 10 * lMainBarDia1,
                                               lCrankHeightEnd2, 0, 0, 0);
                                }
                            }
                            else
                            {
                                lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "041",
                                       "MM", "MM2", lMainBarDia2, l2MainBarLen, lMainBarCT2 / 2,
                                       pBPCModels.end_length, 10 * lMainBarDia1,
                                       int.Parse(pBPCModels.cage_length) - pBPCModels.end_length - 10 * lMainBarDia1,
                                       lCrankHeightEnd2, 0, 0, 0);
                            }
                            lRebarList.Add(lRebar);
                            lBarID++;
                            lBarSort++;
                        }
                        else if (lMainBarShape == "Crank-Both")
                        {
                            if (pBPCModels.crank_height_top == pBPCModels.crank_height_end)
                            {
                                lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "043",
                                           "MB", "MB1", lMainBarDia1, lMainBarLen, lMainBarCT1 / 2,
                                           pBPCModels.lap_length, 10 * lMainBarDia1,
                                           lMainBarLen - pBPCModels.lap_length - 2 * 10 * lMainBarDia1 - pBPCModels.end_length,
                                           lCrankHeightTop1,
                                           pBPCModels.end_length,
                                           (int)Math.Round(180 / Math.PI * Math.Asin((double)(lCrankHeightTop1 - lMainBarDia1) / (10 * lMainBarDia1))),
                                           0);
                                lRebarList.Add(lRebar);
                                lBarID++;
                                lBarSort++;

                                lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "043",
                                           "MB", "MB2", lMainBarDia2, lMainBarLen, lMainBarCT2 / 2,
                                           pBPCModels.lap_length, 10 * lMainBarDia2,
                                           lMainBarLen - pBPCModels.lap_length - 2 * 10 * lMainBarDia2 - pBPCModels.end_length,
                                           lCrankHeightTop2,
                                           pBPCModels.end_length,
                                           (int)Math.Round(180 / Math.PI * Math.Asin((double)(lCrankHeightTop2 - lMainBarDia2) / (10 * lMainBarDia2))),
                                           0);
                                lRebarList.Add(lRebar);
                                lBarID++;
                                lBarSort++;

                                //Second layer
                                // First main bar
                                if (l2MainBarLen != int.Parse(pBPCModels.cage_length))
                                {
                                    if (pBPCModels.mainbar_location_2layer >= pBPCModels.lap_length && pBPCModels.mainbar_location_2layer + l2MainBarLen <= int.Parse(pBPCModels.cage_length) - pBPCModels.end_length)
                                    {
                                        lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "020",
                                                   "MM", "MM1", lMainBarDia1, l2MainBarLen, lMainBarCT1 / 2,
                                                   l2MainBarLen,
                                                   0, 0, 0, 0, 0, 0);
                                    }
                                    else if (pBPCModels.mainbar_location_2layer + l2MainBarLen <= int.Parse(pBPCModels.cage_length) - pBPCModels.end_length)
                                    {
                                        //crank at top
                                        lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "041",
                                                   "MM", "MM1", lMainBarDia1, l2MainBarLen, lMainBarCT1 / 2,
                                                   pBPCModels.lap_length - pBPCModels.mainbar_location_2layer, 10 * lMainBarDia1,
                                                   l2MainBarLen - pBPCModels.mainbar_location_2layer - pBPCModels.lap_length - 10 * lMainBarDia1,
                                                   lCrankHeightTop1, 0, 0, 0);
                                    }
                                    else if (pBPCModels.mainbar_location_2layer >= pBPCModels.lap_length && pBPCModels.mainbar_location_2layer + l2MainBarLen <= int.Parse(pBPCModels.cage_length) - pBPCModels.end_length)
                                    {
                                        // crank at END
                                        lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "041",
                                                   "MM", "MM1", lMainBarDia1, l2MainBarLen, lMainBarCT1 / 2,
                                                   pBPCModels.end_length - (int.Parse(pBPCModels.cage_length) - pBPCModels.mainbar_location_2layer - l2MainBarLen), 10 * lMainBarDia1,
                                                   int.Parse(pBPCModels.cage_length) - pBPCModels.end_length - pBPCModels.mainbar_location_2layer - 10 * lMainBarDia1,
                                                   lCrankHeightTop1, 0, 0, 0);
                                    }
                                    else
                                    {
                                        //crank at both
                                        lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "043",
                                               "MM", "MM1", lMainBarDia1, l2MainBarLen, lMainBarCT1 / 2,
                                               pBPCModels.lap_length - pBPCModels.mainbar_location_2layer, 10 * lMainBarDia1,
                                               int.Parse(pBPCModels.cage_length) - pBPCModels.lap_length - 2 * 10 * lMainBarDia1 - pBPCModels.end_length,
                                               lCrankHeightTop1,
                                               pBPCModels.end_length,
                                               (int)Math.Round(180 / Math.PI * Math.Asin((double)(lCrankHeightTop1 - lMainBarDia1) / (10 * lMainBarDia1))),
                                               0);
                                    }
                                }
                                else
                                {
                                    lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "043",
                                           "MM", "MM1", lMainBarDia1, int.Parse(pBPCModels.cage_length), lMainBarCT1 / 2,
                                           pBPCModels.lap_length, 10 * lMainBarDia1,
                                           int.Parse(pBPCModels.cage_length) - pBPCModels.lap_length - 2 * 10 * lMainBarDia1 - pBPCModels.end_length,
                                           lCrankHeightTop1,
                                           pBPCModels.end_length,
                                           (int)Math.Round(180 / Math.PI * Math.Asin((double)(lCrankHeightTop1 - lMainBarDia1) / (10 * lMainBarDia1))),
                                           0);
                                }
                                lRebarList.Add(lRebar);
                                lBarID++;
                                lBarSort++;

                                //Second layer
                                // Scond main bar
                                if (l2MainBarLen != int.Parse(pBPCModels.cage_length))
                                {
                                    if (pBPCModels.mainbar_location_2layer >= pBPCModels.lap_length && pBPCModels.mainbar_location_2layer + l2MainBarLen <= int.Parse(pBPCModels.cage_length) - pBPCModels.end_length)
                                    {
                                        lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "020",
                                                   "MM", "MM2", lMainBarDia2, l2MainBarLen, lMainBarCT2 / 2,
                                                   l2MainBarLen,
                                                   0, 0, 0, 0, 0, 0);
                                    }
                                    else if (pBPCModels.mainbar_location_2layer + l2MainBarLen <= int.Parse(pBPCModels.cage_length) - pBPCModels.end_length)
                                    {
                                        //crank at top
                                        lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "041",
                                                   "MM", "MM2", lMainBarDia2, l2MainBarLen, lMainBarCT2 / 2,
                                                   pBPCModels.lap_length - pBPCModels.mainbar_location_2layer, 10 * lMainBarDia1,
                                                   l2MainBarLen - pBPCModels.mainbar_location_2layer - pBPCModels.lap_length - 10 * lMainBarDia1,
                                                   lCrankHeightTop2, 0, 0, 0);
                                    }
                                    else if (pBPCModels.mainbar_location_2layer >= pBPCModels.lap_length && pBPCModels.mainbar_location_2layer + l2MainBarLen <= int.Parse(pBPCModels.cage_length) - pBPCModels.end_length)
                                    {
                                        // crank at END
                                        lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "041",
                                                   "MM", "MM2", lMainBarDia2, l2MainBarLen, lMainBarCT2 / 2,
                                                   pBPCModels.end_length - (int.Parse(pBPCModels.cage_length) - pBPCModels.mainbar_location_2layer - l2MainBarLen), 10 * lMainBarDia1,
                                                   int.Parse(pBPCModels.cage_length) - pBPCModels.end_length - pBPCModels.mainbar_location_2layer - 10 * lMainBarDia1,
                                                   lCrankHeightTop2, 0, 0, 0);
                                    }
                                    else
                                    {
                                        //crank at both
                                        lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "043",
                                               "MM", "MM2", lMainBarDia2, l2MainBarLen, lMainBarCT2 / 2,
                                               pBPCModels.lap_length - pBPCModels.mainbar_location_2layer, 10 * lMainBarDia2,
                                               int.Parse(pBPCModels.cage_length) - pBPCModels.lap_length - 2 * 10 * lMainBarDia2 - pBPCModels.end_length,
                                               lCrankHeightTop2,
                                               pBPCModels.end_length,
                                               (int)Math.Round(180 / Math.PI * Math.Asin((double)(lCrankHeightTop2 - lMainBarDia2) / (10 * lMainBarDia2))),
                                               0);
                                    }
                                }
                                else
                                {
                                    lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "043",
                                           "MM", "MM2", lMainBarDia2, int.Parse(pBPCModels.cage_length), lMainBarCT2 / 2,
                                           pBPCModels.lap_length, 10 * lMainBarDia2,
                                           int.Parse(pBPCModels.cage_length) - pBPCModels.lap_length - 2 * 10 * lMainBarDia2 - pBPCModels.end_length,
                                           lCrankHeightTop2,
                                           pBPCModels.end_length,
                                           (int)Math.Round(180 / Math.PI * Math.Asin((double)(lCrankHeightTop2 - lMainBarDia2) / (10 * lMainBarDia2))),
                                           0);
                                }
                                lRebarList.Add(lRebar);
                                lBarID++;
                                lBarSort++;

                            }
                            else
                            {
                                lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "43B",
                                           "MB", "MB1", lMainBarDia1, lMainBarLen, lMainBarCT1 / 2,
                                           pBPCModels.lap_length,
                                           10 * lMainBarDia1,
                                           lMainBarLen - pBPCModels.lap_length - 2 * 10 * lMainBarDia1 - pBPCModels.end_length,
                                           10 * lMainBarDia1,
                                           pBPCModels.end_length,
                                           lCrankHeightTop1,
                                           lCrankHeightEnd1);
                                lRebarList.Add(lRebar);
                                lBarID++;
                                lBarSort++;

                                lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "43B",
                                           "MB", "MB2", lMainBarDia2, lMainBarLen, lMainBarCT2 / 2,
                                           pBPCModels.lap_length,
                                           10 * lMainBarDia2,
                                           lMainBarLen - pBPCModels.lap_length - 2 * 10 * lMainBarDia2 - pBPCModels.end_length,
                                           10 * lMainBarDia2,
                                           pBPCModels.end_length,
                                           lCrankHeightTop2,
                                           lCrankHeightEnd2);
                                lRebarList.Add(lRebar);
                                lBarID++;
                                lBarSort++;

                                lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "43B",
                                           "MM", "MM1", lMainBarDia1, lMainBarLen, lMainBarCT1 / 2,
                                           pBPCModels.lap_length,
                                           10 * lMainBarDia1,
                                           lMainBarLen - pBPCModels.lap_length - 2 * 10 * lMainBarDia1 - pBPCModels.end_length,
                                           10 * lMainBarDia1,
                                           pBPCModels.end_length,
                                           lCrankHeightTop1,
                                           lCrankHeightEnd1);
                                lRebarList.Add(lRebar);
                                lBarID++;
                                lBarSort++;

                                lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "43B",
                                           "MM", "MM2", lMainBarDia2, lMainBarLen, lMainBarCT2 / 2,
                                           pBPCModels.lap_length,
                                           10 * lMainBarDia2,
                                           lMainBarLen - pBPCModels.lap_length - 2 * 10 * lMainBarDia2 - pBPCModels.end_length,
                                           10 * lMainBarDia2,
                                           pBPCModels.end_length,
                                           lCrankHeightTop2,
                                           lCrankHeightEnd2);
                                lRebarList.Add(lRebar);
                                lBarID++;
                                lBarSort++;
                            }
                        }
                        else
                        {
                            lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "020",
                                       "MB", "MB1", lMainBarDia1, lMainBarLen, lMainBarCT1 / 2,
                                       int.Parse(pBPCModels.cage_length),
                                       0, 0, 0, 0, 0, 0);
                            lRebarList.Add(lRebar);
                            lBarID++;
                            lBarSort++;

                            lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "020",
                                       "MB", "MB2", lMainBarDia2, lMainBarLen, lMainBarCT2 / 2,
                                       int.Parse(pBPCModels.cage_length),
                                       0, 0, 0, 0, 0, 0);
                            lRebarList.Add(lRebar);
                            lBarID++;
                            lBarSort++;

                            lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "020",
                                       "MM", "MM1", lMainBarDia1, l2MainBarLen, lMainBarCT1 / 2,
                                       l2MainBarLen,
                                       0, 0, 0, 0, 0, 0);
                            lRebarList.Add(lRebar);
                            lBarID++;
                            lBarSort++;

                            lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "020",
                                       "MM", "MM2", lMainBarDia2, l2MainBarLen, lMainBarCT2 / 2,
                                       l2MainBarLen,
                                       0, 0, 0, 0, 0, 0);
                            lRebarList.Add(lRebar);
                            lBarID++;
                            lBarSort++;
                        }
                    }
                    else
                    {
                        //Main bar1 first layer, Main bar2 second layer
                        if (lMainBarShape == "Crank-Top" || lMainBarShape == "Crank")
                        {
                            lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "041",
                                       "MB", "MB", lMainBarDia1, lMainBarLen, lMainBarCT1,
                                       pBPCModels.lap_length, 10 * lMainBarDia1,
                                       lMainBarLen - pBPCModels.lap_length - 10 * lMainBarDia1,
                                       lCrankHeightTop1, 0, 0, 0);
                            lRebarList.Add(lRebar);
                            lBarID++;
                            lBarSort++;

                            //Second layer
                            //Second main bars
                            if (l2MainBarLen != int.Parse(pBPCModels.cage_length))
                            {
                                if (pBPCModels.mainbar_location_2layer >= pBPCModels.lap_length)
                                {
                                    lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "020",
                                               "MM", "MM", lMainBarDia2, l2MainBarLen, lMainBarCT2,
                                               l2MainBarLen,
                                               0, 0, 0, 0, 0, 0);
                                }
                                else
                                {
                                    lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "041",
                                               "MM", "MM", lMainBarDia2, l2MainBarLen, lMainBarCT2,
                                               pBPCModels.lap_length - pBPCModels.mainbar_location_2layer, 10 * lMainBarDia1,
                                               l2MainBarLen - pBPCModels.mainbar_location_2layer - pBPCModels.lap_length - 10 * lMainBarDia1,
                                               lCrankHeightTop2, 0, 0, 0);
                                }
                            }
                            else
                            {
                                lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "041",
                                           "MM", "MM", lMainBarDia2, int.Parse(pBPCModels.cage_length), lMainBarCT2,
                                           pBPCModels.lap_length, 10 * lMainBarDia1,
                                           int.Parse(pBPCModels.cage_length) - pBPCModels.lap_length - 10 * lMainBarDia1,
                                           lCrankHeightTop2, 0, 0, 0);
                            }
                            lRebarList.Add(lRebar);
                            lBarID++;
                            lBarSort++;
                        }
                        else if (lMainBarShape == "Crank-End")
                        {
                            lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "041",
                                       "MB", "MB", lMainBarDia1, lMainBarLen, lMainBarCT1,
                                       pBPCModels.end_length, 10 * lMainBarDia1,
                                       lMainBarLen - pBPCModels.end_length - 10 * lMainBarDia1,
                                       lCrankHeightEnd1, 0, 0, 0);
                            lRebarList.Add(lRebar);
                            lBarID++;
                            lBarSort++;

                            //Second layer
                            //Second Main bars
                            if (l2MainBarLen != int.Parse(pBPCModels.cage_length))
                            {
                                if (pBPCModels.mainbar_location_2layer + l2MainBarLen <= int.Parse(pBPCModels.cage_length) - pBPCModels.end_length)
                                {
                                    lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "020",
                                               "MM", "MM", lMainBarDia2, l2MainBarLen, lMainBarCT2,
                                               l2MainBarLen,
                                               0, 0, 0, 0, 0, 0);
                                }
                                else
                                {
                                    lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "041",
                                               "MM", "MM", lMainBarDia2, l2MainBarLen, lMainBarCT2,
                                               pBPCModels.end_length - (int.Parse(pBPCModels.cage_length) - pBPCModels.mainbar_location_2layer - l2MainBarLen), 10 * lMainBarDia1,
                                               int.Parse(pBPCModels.cage_length) - pBPCModels.end_length - pBPCModels.mainbar_location_2layer - 10 * lMainBarDia1,
                                               lCrankHeightEnd2, 0, 0, 0);
                                }
                            }
                            else
                            {
                                lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "041",
                                       "MM", "MM", lMainBarDia2, l2MainBarLen, lMainBarCT2,
                                       pBPCModels.end_length, 10 * lMainBarDia1,
                                       int.Parse(pBPCModels.cage_length) - pBPCModels.end_length - 10 * lMainBarDia1,
                                       lCrankHeightEnd2, 0, 0, 0);
                            }
                            lRebarList.Add(lRebar);
                            lBarID++;
                            lBarSort++;
                        }
                        else if (lMainBarShape == "Crank-Both")
                        {
                            if (pBPCModels.crank_height_top == pBPCModels.crank_height_end)
                            {
                                lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "043",
                                           "MB", "MB", lMainBarDia1, lMainBarLen, lMainBarCT1,
                                           pBPCModels.lap_length, 10 * lMainBarDia1,
                                           lMainBarLen - pBPCModels.lap_length - 2 * 10 * lMainBarDia1 - pBPCModels.end_length,
                                           lCrankHeightTop1,
                                           pBPCModels.end_length,
                                           (int)Math.Round(180 / Math.PI * Math.Asin((double)(lCrankHeightTop1 - lMainBarDia1) / (10 * lMainBarDia1))),
                                           0);
                                lRebarList.Add(lRebar);
                                lBarID++;
                                lBarSort++;

                                //Second layer
                                // Scond main bar
                                if (l2MainBarLen != int.Parse(pBPCModels.cage_length))
                                {
                                    if (pBPCModels.mainbar_location_2layer >= pBPCModels.lap_length && pBPCModels.mainbar_location_2layer + l2MainBarLen <= int.Parse(pBPCModels.cage_length) - pBPCModels.end_length)
                                    {
                                        lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "020",
                                                   "MM", "MM", lMainBarDia2, l2MainBarLen, lMainBarCT2,
                                                   l2MainBarLen,
                                                   0, 0, 0, 0, 0, 0);
                                    }
                                    else if (pBPCModels.mainbar_location_2layer + l2MainBarLen <= int.Parse(pBPCModels.cage_length) - pBPCModels.end_length)
                                    {
                                        //crank at top
                                        lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "041",
                                                   "MM", "MM", lMainBarDia2, l2MainBarLen, lMainBarCT2,
                                                   pBPCModels.lap_length - pBPCModels.mainbar_location_2layer, 10 * lMainBarDia1,
                                                   l2MainBarLen - pBPCModels.mainbar_location_2layer - pBPCModels.lap_length - 10 * lMainBarDia1,
                                                   lCrankHeightTop2, 0, 0, 0);
                                    }
                                    else if (pBPCModels.mainbar_location_2layer >= pBPCModels.lap_length && pBPCModels.mainbar_location_2layer + l2MainBarLen <= int.Parse(pBPCModels.cage_length) - pBPCModels.end_length)
                                    {
                                        // crank at END
                                        lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "041",
                                                   "MM", "MM", lMainBarDia2, l2MainBarLen, lMainBarCT2,
                                                   pBPCModels.end_length - (int.Parse(pBPCModels.cage_length) - pBPCModels.mainbar_location_2layer - l2MainBarLen), 10 * lMainBarDia1,
                                                   int.Parse(pBPCModels.cage_length) - pBPCModels.end_length - pBPCModels.mainbar_location_2layer - 10 * lMainBarDia1,
                                                   lCrankHeightEnd2, 0, 0, 0);
                                    }
                                    else
                                    {
                                        //crank at both
                                        lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "043",
                                               "MM", "MM", lMainBarDia2, l2MainBarLen, lMainBarCT2,
                                               pBPCModels.lap_length - pBPCModels.mainbar_location_2layer, 10 * lMainBarDia2,
                                               int.Parse(pBPCModels.cage_length) - pBPCModels.lap_length - 2 * 10 * lMainBarDia2 - pBPCModels.end_length,
                                               lCrankHeightTop2,
                                               pBPCModels.end_length,
                                               (int)Math.Round(180 / Math.PI * Math.Asin((double)(lCrankHeightTop2 - lMainBarDia2) / (10 * lMainBarDia2))),
                                               0);
                                    }
                                }
                                else
                                {
                                    lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "043",
                                           "MM", "MM", lMainBarDia2, int.Parse(pBPCModels.cage_length), lMainBarCT2,
                                           pBPCModels.lap_length, 10 * lMainBarDia2,
                                           int.Parse(pBPCModels.cage_length) - pBPCModels.lap_length - 2 * 10 * lMainBarDia2 - pBPCModels.end_length,
                                           lCrankHeightTop2,
                                           pBPCModels.end_length,
                                           (int)Math.Round(180 / Math.PI * Math.Asin((double)(lCrankHeightTop2 - lMainBarDia2) / (10 * lMainBarDia2))),
                                           0);
                                }
                                lRebarList.Add(lRebar);
                                lBarID++;
                                lBarSort++;

                            }
                            else
                            {
                                lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "43B",
                                           "MB", "MB1", lMainBarDia1, lMainBarLen, lMainBarCT1 / 2,
                                           pBPCModels.lap_length,
                                           10 * lMainBarDia1,
                                           lMainBarLen - pBPCModels.lap_length - 2 * 10 * lMainBarDia1 - pBPCModels.end_length,
                                           10 * lMainBarDia1,
                                           pBPCModels.end_length,
                                           lCrankHeightTop1,
                                           lCrankHeightEnd1);
                                lRebarList.Add(lRebar);
                                lBarID++;
                                lBarSort++;

                                lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "43B",
                                           "MB", "MB2", lMainBarDia2, lMainBarLen, lMainBarCT2 / 2,
                                           pBPCModels.lap_length,
                                           10 * lMainBarDia2,
                                           lMainBarLen - pBPCModels.lap_length - 2 * 10 * lMainBarDia2 - pBPCModels.end_length,
                                           10 * lMainBarDia2,
                                           pBPCModels.end_length,
                                           lCrankHeightTop2,
                                           lCrankHeightEnd2);
                                lRebarList.Add(lRebar);
                                lBarID++;
                                lBarSort++;

                                lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "43B",
                                           "MM", "MM1", lMainBarDia1, lMainBarLen, lMainBarCT1 / 2,
                                           pBPCModels.lap_length,
                                           10 * lMainBarDia1,
                                           lMainBarLen - pBPCModels.lap_length - 2 * 10 * lMainBarDia1 - pBPCModels.end_length,
                                           10 * lMainBarDia1,
                                           pBPCModels.end_length,
                                           lCrankHeightTop1,
                                           lCrankHeightEnd1);
                                lRebarList.Add(lRebar);
                                lBarID++;
                                lBarSort++;

                                lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "43B",
                                           "MM", "MM2", lMainBarDia2, lMainBarLen, lMainBarCT2 / 2,
                                           pBPCModels.lap_length,
                                           10 * lMainBarDia2,
                                           lMainBarLen - pBPCModels.lap_length - 2 * 10 * lMainBarDia2 - pBPCModels.end_length,
                                           10 * lMainBarDia2,
                                           pBPCModels.end_length,
                                           lCrankHeightTop2,
                                           lCrankHeightEnd2);
                                lRebarList.Add(lRebar);
                                lBarID++;
                                lBarSort++;
                            }
                        }
                        else
                        {
                            lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "020",
                                       "MB", "MB", lMainBarDia1, lMainBarLen, lMainBarCT1,
                                       int.Parse(pBPCModels.cage_length),
                                       0, 0, 0, 0, 0, 0);
                            lRebarList.Add(lRebar);
                            lBarID++;
                            lBarSort++;

                            lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "020",
                                       "MM", "MM", lMainBarDia2, l2MainBarLen, lMainBarCT2,
                                       l2MainBarLen,
                                       0, 0, 0, 0, 0, 0);
                            lRebarList.Add(lRebar);
                            lBarID++;
                            lBarSort++;
                        }
                    }
                }
                else
                {
                    //For backup no more such case
                    if (lMainBarDia1 == lMainBarDia2)
                    {
                        int lMaxCT = lMainBarCT1;
                        if (lMaxCT < lMainBarCT2)
                        {
                            lMainBarCT1 = lMainBarCT2;
                            lMainBarCT2 = lMaxCT;
                        }
                        if (lMainBarShape == "Crank-Top" || lMainBarShape == "Crank")
                        {
                            lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "041",
                                       "MB", "MB", lMainBarDia1, lMainBarLen, lMainBarCT1,
                                       pBPCModels.lap_length, 10 * lMainBarDia1,
                                       lMainBarLen - pBPCModels.lap_length - 10 * lMainBarDia1,
                                       lCrankHeightTop1, 0, 0, 0);
                            lRebarList.Add(lRebar);
                            lBarID++;
                            lBarSort++;

                            lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "041",
                                       "MM", "MM", lMainBarDia2, lMainBarLen, lMainBarCT2,
                                       pBPCModels.lap_length, 10 * lMainBarDia2,
                                       lMainBarLen - pBPCModels.lap_length - 10 * lMainBarDia2,
                                       lCrankHeightTop2, 0, 0, 0);
                            lRebarList.Add(lRebar);
                            lBarID++;
                            lBarSort++;
                        }
                        else if (lMainBarShape == "Crank-End")
                        {
                            lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "041",
                                       "MB", "MB", lMainBarDia1, lMainBarLen, lMainBarCT1,
                                       pBPCModels.end_length, 10 * lMainBarDia1,
                                       lMainBarLen - pBPCModels.end_length - 10 * lMainBarDia1,
                                       lCrankHeightEnd1, 0, 0, 0);
                            lRebarList.Add(lRebar);
                            lBarID++;
                            lBarSort++;

                            lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "041",
                                       "MM", "MM", lMainBarDia2, lMainBarLen, lMainBarCT2,
                                       pBPCModels.end_length, 10 * lMainBarDia2,
                                       lMainBarLen - pBPCModels.end_length - 10 * lMainBarDia2,
                                       lCrankHeightEnd2, 0, 0, 0);
                            lRebarList.Add(lRebar);
                            lBarID++;
                            lBarSort++;
                        }
                        else if (lMainBarShape == "Crank-Both")
                        {
                            if (pBPCModels.crank_height_top == pBPCModels.crank_height_end)
                            {
                                lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "043",
                                           "MB", "MB", lMainBarDia1, lMainBarLen, lMainBarCT1,
                                           pBPCModels.lap_length, 10 * lMainBarDia1,
                                           lMainBarLen - pBPCModels.lap_length - 2 * 10 * lMainBarDia1 - pBPCModels.end_length,
                                           lCrankHeightTop1,
                                           pBPCModels.end_length,
                                           (int)Math.Round(180 / Math.PI * Math.Asin((double)(lCrankHeightTop1 - lMainBarDia1) / (10 * lMainBarDia1))),
                                           0);
                                lRebarList.Add(lRebar);
                                lBarID++;
                                lBarSort++;

                                lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "043",
                                           "MM", "MM", lMainBarDia2, lMainBarLen, lMainBarCT2,
                                           pBPCModels.lap_length, 10 * lMainBarDia2,
                                           lMainBarLen - pBPCModels.lap_length - 2 * 10 * lMainBarDia2 - pBPCModels.end_length,
                                           lCrankHeightTop2,
                                           pBPCModels.end_length,
                                           (int)Math.Round(180 / Math.PI * Math.Asin((double)(lCrankHeightTop2 - lMainBarDia2) / (10 * lMainBarDia2))),
                                           0);
                                lRebarList.Add(lRebar);
                                lBarID++;
                                lBarSort++;
                            }
                            else
                            {
                                lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "43B",
                                           "MB", "MB", lMainBarDia1, lMainBarLen, lMainBarCT1,
                                           pBPCModels.lap_length,
                                           10 * lMainBarDia1,
                                           lMainBarLen - pBPCModels.lap_length - 2 * 10 * lMainBarDia1 - pBPCModels.end_length,
                                           10 * lMainBarDia1,
                                           pBPCModels.end_length,
                                           lCrankHeightTop1,
                                           lCrankHeightEnd1);
                                lRebarList.Add(lRebar);
                                lBarID++;
                                lBarSort++;

                                lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "43B",
                                           "MM", "MM", lMainBarDia2, lMainBarLen, lMainBarCT2,
                                           pBPCModels.lap_length,
                                           10 * lMainBarDia2,
                                           lMainBarLen - pBPCModels.lap_length - 2 * 10 * lMainBarDia2 - pBPCModels.end_length,
                                           10 * lMainBarDia2,
                                           pBPCModels.end_length,
                                           lCrankHeightTop2,
                                           lCrankHeightEnd2);
                                lRebarList.Add(lRebar);
                                lBarID++;
                                lBarSort++;
                            }
                        }
                        else
                        {
                            lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "020",
                                       "MB", "MB", lMainBarDia1, lMainBarLen, lMainBarCT1,
                                       int.Parse(pBPCModels.cage_length),
                                       0, 0, 0, 0, 0, 0);
                            lRebarList.Add(lRebar);
                            lBarID++;
                            lBarSort++;

                            lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "020",
                                       "MM", "MM", lMainBarDia2, lMainBarLen, lMainBarCT2,
                                       int.Parse(pBPCModels.cage_length),
                                       0, 0, 0, 0, 0, 0);
                            lRebarList.Add(lRebar);
                            lBarID++;
                            lBarSort++;
                        }
                    }
                    else
                    {
                        if (lMainBarShape == "Crank-Top" || lMainBarShape == "Crank")
                        {
                            lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "041",
                                       "MB", "MB1", lMainBarDia1, lMainBarLen, lMainBarCT1,
                                       pBPCModels.lap_length, 10 * lMainBarDia1,
                                       lMainBarLen - pBPCModels.lap_length - 10 * lMainBarDia1,
                                       lCrankHeightTop1, 0, 0, 0);
                            lRebarList.Add(lRebar);
                            lBarID++;
                            lBarSort++;

                            lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "041",
                                       "MB", "MB2", lMainBarDia2, lMainBarLen, lMainBarCT2,
                                       pBPCModels.lap_length, 10 * lMainBarDia2,
                                       lMainBarLen - pBPCModels.lap_length - 10 * lMainBarDia2,
                                       lCrankHeightTop2, 0, 0, 0);
                            lRebarList.Add(lRebar);
                            lBarID++;
                            lBarSort++;
                        }
                        else if (lMainBarShape == "Crank-End")
                        {
                            lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "041",
                                       "MB", "MB1", lMainBarDia1, lMainBarLen, lMainBarCT1,
                                       pBPCModels.end_length, 10 * lMainBarDia1,
                                       lMainBarLen - pBPCModels.end_length - 10 * lMainBarDia1,
                                       lCrankHeightEnd1, 0, 0, 0);
                            lRebarList.Add(lRebar);
                            lBarID++;
                            lBarSort++;

                            lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "041",
                                       "MB", "MB2", lMainBarDia2, lMainBarLen, lMainBarCT2,
                                       pBPCModels.end_length, 10 * lMainBarDia2,
                                       lMainBarLen - pBPCModels.end_length - 10 * lMainBarDia2,
                                       lCrankHeightEnd2, 0, 0, 0);
                            lRebarList.Add(lRebar);
                            lBarID++;
                            lBarSort++;
                        }
                        else if (lMainBarShape == "Crank-Both")
                        {
                            if (pBPCModels.crank_height_top == pBPCModels.crank_height_end)
                            {
                                lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "043",
                                           "MB", "MB1", lMainBarDia1, lMainBarLen, lMainBarCT1,
                                           pBPCModels.lap_length, 10 * lMainBarDia1,
                                           lMainBarLen - pBPCModels.lap_length - 2 * 10 * lMainBarDia1 - pBPCModels.end_length,
                                           lCrankHeightTop1,
                                           pBPCModels.end_length,
                                           (int)Math.Round(180 / Math.PI * Math.Asin((double)(lCrankHeightTop1 - lMainBarDia1) / (10 * lMainBarDia1))),
                                           0);
                                lRebarList.Add(lRebar);
                                lBarID++;
                                lBarSort++;

                                lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "043",
                                           "MB", "MB2", lMainBarDia2, lMainBarLen, lMainBarCT2,
                                           pBPCModels.lap_length, 10 * lMainBarDia2,
                                           lMainBarLen - pBPCModels.lap_length - 2 * 10 * lMainBarDia2 - pBPCModels.end_length,
                                           lCrankHeightTop2,
                                           pBPCModels.end_length,
                                           (int)Math.Round(180 / Math.PI * Math.Asin((double)(lCrankHeightTop2 - lMainBarDia2) / (10 * lMainBarDia2))),
                                           0);
                                lRebarList.Add(lRebar);
                                lBarID++;
                                lBarSort++;
                            }
                            else
                            {
                                lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "43B",
                                           "MB", "MB1", lMainBarDia1, lMainBarLen, lMainBarCT1,
                                           pBPCModels.lap_length,
                                           10 * lMainBarDia1,
                                           lMainBarLen - pBPCModels.lap_length - 2 * 10 * lMainBarDia1 - pBPCModels.end_length,
                                           10 * lMainBarDia1,
                                           pBPCModels.end_length,
                                           lCrankHeightTop1,
                                           lCrankHeightEnd1);
                                lRebarList.Add(lRebar);
                                lBarID++;
                                lBarSort++;

                                lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "43B",
                                           "MB", "MB2", lMainBarDia2, lMainBarLen, lMainBarCT2,
                                           pBPCModels.lap_length,
                                           10 * lMainBarDia2,
                                           lMainBarLen - pBPCModels.lap_length - 2 * 10 * lMainBarDia2 - pBPCModels.end_length,
                                           10 * lMainBarDia2,
                                           pBPCModels.end_length,
                                           lCrankHeightTop2,
                                           lCrankHeightEnd2);
                                lRebarList.Add(lRebar);
                                lBarID++;
                                lBarSort++;
                            }
                        }
                        else
                        {
                            lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "020",
                                       "MB", "MB1", lMainBarDia1, lMainBarLen, lMainBarCT1,
                                       int.Parse(pBPCModels.cage_length),
                                       0, 0, 0, 0, 0, 0);
                            lRebarList.Add(lRebar);
                            lBarID++;
                            lBarSort++;

                            lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "020",
                                       "MB", "MB2", lMainBarDia2, lMainBarLen, lMainBarCT2,
                                       int.Parse(pBPCModels.cage_length),
                                       0, 0, 0, 0, 0, 0);
                            lRebarList.Add(lRebar);
                            lBarID++;
                            lBarSort++;
                        }
                    }
                }
            }
            else if (lPileType == "Double-Layer")
            {
                var lMainbarDiaArr = lMainBarDia.Split(',');
                var lMainbarCTArr = lMainBarCT.Split(',');
                var lMainBarLen = lCageLength;
                if (lMainbarDiaArr.Length > 0 && lMainbarCTArr.Length > 0)
                {
                    int lMainBarDia1 = int.Parse(lMainbarDiaArr[0]);
                    int lMainBarCT1 = int.Parse(lMainbarCTArr[0]);

                    if (lMainBarShape == "Crank-Top" || lMainBarShape == "Crank")
                    {
                        lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "041",
                                   "MB", "MB", lMainBarDia1, int.Parse(lMainBarLen), lMainBarCT1,
                                   pBPCModels.lap_length, 10 * lMainBarDia1,
                                   int.Parse(lMainBarLen) - pBPCModels.lap_length - 10 * lMainBarDia1,
                                   pBPCModels.crank_height_top, 0, 0, 0);
                        lRebarList.Add(lRebar);
                        lBarID++;
                        lBarSort++;
                    }
                    else if (lMainBarShape == "Crank-End")
                    {
                        lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "041",
                                   "MB", "MB", lMainBarDia1, int.Parse(lMainBarLen), lMainBarCT1,
                                   pBPCModels.end_length, 10 * lMainBarDia1,
                                   int.Parse(lMainBarLen) - pBPCModels.end_length - 10 * lMainBarDia1,
                                   pBPCModels.crank_height_end, 0, 0, 0);
                        lRebarList.Add(lRebar);
                        lBarID++;
                        lBarSort++;
                    }
                    else if (lMainBarShape == "Crank-Both")
                    {
                        if (pBPCModels.crank_height_top == pBPCModels.crank_height_end)
                        {
                            lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "043",
                                       "MB", "MB", lMainBarDia1, int.Parse(lMainBarLen), lMainBarCT1,
                                       pBPCModels.lap_length, 10 * lMainBarDia1,
                                       int.Parse(lMainBarLen) - pBPCModels.lap_length - 2 * 10 * lMainBarDia1 - pBPCModels.end_length,
                                       pBPCModels.crank_height_top,
                                       pBPCModels.end_length,
                                       (int)Math.Round(180 / Math.PI * Math.Asin((double)(pBPCModels.crank_height_top - lMainBarDia1) / (10 * lMainBarDia1))),
                                       0);
                            lRebarList.Add(lRebar);
                        }
                        else
                        {
                            lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "43B",
                                       "MB", "MB", lMainBarDia1, int.Parse(lMainBarLen), lMainBarCT1,
                                       pBPCModels.lap_length,
                                       10 * lMainBarDia1,
                                       int.Parse(lMainBarLen) - pBPCModels.lap_length - 2 * 10 * lMainBarDia1 - pBPCModels.end_length,
                                       10 * lMainBarDia1,
                                       pBPCModels.end_length,
                                       pBPCModels.crank_height_top,
                                       pBPCModels.crank_height_end);
                            lRebarList.Add(lRebar);
                        }
                        lBarID++;
                        lBarSort++;
                    }
                    else
                    {
                        lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "020",
                                   "MB", "MB", lMainBarDia1, int.Parse(lMainBarLen), lMainBarCT1,
                                   int.Parse(pBPCModels.cage_length),
                                   0, 0, 0, 0, 0, 0);
                        lRebarList.Add(lRebar);

                        lBarID++;
                        lBarSort++;
                    }
                }

                //Manual Main Bars
                if (lMainbarDiaArr.Length > 1 && lMainbarCTArr.Length > 1)
                {

                    int lMainBarDia2 = int.Parse(lMainbarDiaArr[1]);
                    int lMainBarCT2 = int.Parse(lMainbarCTArr[1]);

                    if (lMainBarShape == "Crank-Top" || lMainBarShape == "Crank")
                    {
                        lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "041",
                                   "MM", "MM", lMainBarDia2, pBPCModels.mainbar_length_2layer, lMainBarCT2,
                                   pBPCModels.lap_length, 10 * lMainBarDia2,
                                   pBPCModels.mainbar_length_2layer - pBPCModels.lap_length - 10 * lMainBarDia2,
                                   pBPCModels.crank_height_top, 0, 0, 0);
                        lRebarList.Add(lRebar);
                        lBarID++;
                        lBarSort++;
                    }
                    else
                    {
                        lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "020",
                                 "MM", "MM", lMainBarDia2, pBPCModels.mainbar_length_2layer, lMainBarCT2,
                                 pBPCModels.mainbar_length_2layer,
                                 0, 0, 0, 0, 0, 0);
                        lRebarList.Add(lRebar);

                        lBarID++;
                        lBarSort++;
                    }

                  
                }
            }

            //Spiral Link 
            var lSLType = pBPCModels.spiral_link_type;
            var lSLSpacing = pBPCModels.spiral_link_spacing;
            var lLapLength = pBPCModels.lap_length;
            var lEndLength = pBPCModels.end_length;

            var lStartRings = pBPCModels.rings_start;
            var lEndRings = pBPCModels.rings_end;
            var lAddnRingNo = pBPCModels.rings_addn_no;
            var lAddnRingSet = pBPCModels.rings_addn_member;
            var lSL1Length = pBPCModels.sl1_length;
            var lSL2Length = pBPCModels.sl2_length;
            var lSL3Length = pBPCModels.sl3_length;
            var lSL1Dia = pBPCModels.sl1_dia;
            var lSL2Dia = pBPCModels.sl2_dia;
            var lSL3Dia = pBPCModels.sl3_dia;


            int lSLLappingRound = 0;
            bool lSlLapping = false;
            if (lProject != null && lProject.bpc_spiral_lapping != null && lProject.bpc_spiral_lapping == true)
            {
                lSlLapping = true;
            }

            if (lSLType == "1 Spacing")
            {
                int lTotalLength = (int)Math.Round(((double)lSL1Length / int.Parse(lSLSpacing) + lStartRings + lEndRings + lAddnRingNo * lAddnRingSet) *
                    Math.Sqrt(Math.Pow(pBPCModels.cage_dia * Math.PI, 2) + Math.Pow(int.Parse(lSLSpacing), 2)));

                if (lSlLapping == true && lTotalLength > 0)
                {
                    lSLLappingRound = (int)Math.Round((double)(Math.Ceiling((double)lTotalLength / 12000) - 1) * 51 * lSL1Dia / Math.Sqrt(Math.Pow(pBPCModels.cage_dia * Math.PI, 2) + Math.Pow(int.Parse(lSLSpacing), 2)));
                    lTotalLength = lTotalLength + (int)Math.Round((double)lSLLappingRound * Math.Sqrt(Math.Pow(pBPCModels.cage_dia * Math.PI, 2) + Math.Pow(int.Parse(lSLSpacing), 2)));
                }

                lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "R7A",
                           "SL", "SL", lSL1Dia, lTotalLength, 1,
                           pBPCModels.cage_dia, int.Parse(lSLSpacing), lSL1Length,
                           lStartRings + lEndRings + lAddnRingNo * lAddnRingSet + lSLLappingRound, 0, 0, 0);
                lRebarList.Add(lRebar);
                lBarID++;
                lBarSort++;
            }
            else if (lSLType == "2 Spacing")
            {
                var lSpacingArr = lSLSpacing.Split(',');
                var lChangeAddRing = 0;
                if (lSL1Dia != lSL2Dia)
                {
                    lChangeAddRing = 2;
                }
                if (lSpacingArr.Length > 0)
                {
                    int lTotalLength = (int)Math.Round(((double)lSL1Length / int.Parse(lSpacingArr[0]) + lStartRings + lChangeAddRing + lAddnRingNo *
                        Math.Round((double)lAddnRingSet * lSL1Length / (lSL1Length + lSL2Length))) *
                    Math.Sqrt(Math.Pow(pBPCModels.cage_dia * Math.PI, 2) + Math.Pow(int.Parse(lSpacingArr[0]), 2)));

                    if (lSlLapping == true && lTotalLength > 0)
                    {
                        lSLLappingRound = (int)Math.Round((double)(Math.Ceiling((double)lTotalLength / 12000) - 1) * 51 * lSL1Dia / Math.Sqrt(Math.Pow(pBPCModels.cage_dia * Math.PI, 2) + Math.Pow(int.Parse(lSpacingArr[0]), 2)));
                        lTotalLength = lTotalLength + (int)Math.Round((double)lSLLappingRound * Math.Sqrt(Math.Pow(pBPCModels.cage_dia * Math.PI, 2) + Math.Pow(int.Parse(lSpacingArr[0]), 2)));
                    }

                    lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "R7A",
                               "SL", "SL1", lSL1Dia, lTotalLength, 1,
                               pBPCModels.cage_dia, int.Parse(lSpacingArr[0]), lSL1Length,
                               lStartRings + lChangeAddRing + lAddnRingNo *
                               (int)Math.Round((double)lAddnRingSet * lSL1Length / (lSL1Length + lSL2Length)) + lSLLappingRound,
                               0, 0, 0);
                    lRebarList.Add(lRebar);
                    lBarID++;
                    lBarSort++;
                }
                if (lSpacingArr.Length > 1)
                {
                    int lTotalLength = (int)Math.Round(((double)lSL2Length / int.Parse(lSpacingArr[1]) + lEndRings + lChangeAddRing + lAddnRingNo *
                        (lAddnRingSet - Math.Round((double)lAddnRingSet * lSL1Length / (lSL1Length + lSL2Length)))) *
                    Math.Sqrt(Math.Pow(pBPCModels.cage_dia * Math.PI, 2) + Math.Pow(int.Parse(lSpacingArr[1]), 2)));

                    if (lSlLapping == true && lTotalLength > 0)
                    {
                        lSLLappingRound = (int)Math.Round((double)(Math.Ceiling((double)lTotalLength / 12000) - 1) * 51 * lSL1Dia / Math.Sqrt(Math.Pow(pBPCModels.cage_dia * Math.PI, 2) + Math.Pow(int.Parse(lSpacingArr[1]), 2)));
                        lTotalLength = lTotalLength + (int)Math.Round((double)lSLLappingRound * Math.Sqrt(Math.Pow(pBPCModels.cage_dia * Math.PI, 2) + Math.Pow(int.Parse(lSpacingArr[1]), 2)));
                    }

                    lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "R7A",
                               "SL", "SL2", lSL2Dia, lTotalLength, 1,
                               pBPCModels.cage_dia, int.Parse(lSpacingArr[1]), lSL2Length,
                               lEndRings + lChangeAddRing + lAddnRingNo *
                               (lAddnRingSet - (int)Math.Round((double)lAddnRingSet * lSL1Length / (lSL1Length + lSL2Length))) + lSLLappingRound,
                               0, 0, 0);
                    lRebarList.Add(lRebar);
                    lBarID++;
                    lBarSort++;
                }
            }
            else if (lSLType == "3 Spacing")
            {
                var lSpacingArr = lSLSpacing.Split(',');
                var lChangeAddRing1 = 0;
                var lChangeAddRing2 = 0;
                if (lSL1Dia != lSL2Dia)
                {
                    lChangeAddRing1 = 2;
                }
                if (lSL2Dia != lSL3Dia)
                {
                    lChangeAddRing2 = 2;
                }
                if (lSpacingArr.Length > 0)
                {
                    int lTotalLength = (int)Math.Round(((double)lSL1Length / int.Parse(lSpacingArr[0]) + lStartRings + lChangeAddRing1 + lAddnRingNo *
                        Math.Round((double)lAddnRingSet * lSL1Length / (lSL1Length + lSL2Length + lSL3Length))) *
                    Math.Sqrt(Math.Pow(pBPCModels.cage_dia * Math.PI, 2) + Math.Pow(int.Parse(lSpacingArr[0]), 2)));

                    if (lSlLapping == true && lTotalLength > 0)
                    {
                        lSLLappingRound = (int)Math.Round((double)(Math.Ceiling((double)lTotalLength / 12000) - 1) * 51 * lSL1Dia / Math.Sqrt(Math.Pow(pBPCModels.cage_dia * Math.PI, 2) + Math.Pow(int.Parse(lSpacingArr[0]), 2)));
                        lTotalLength = lTotalLength + (int)Math.Round((double)lSLLappingRound * Math.Sqrt(Math.Pow(pBPCModels.cage_dia * Math.PI, 2) + Math.Pow(int.Parse(lSpacingArr[0]), 2)));
                    }

                    lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "R7A",
                               "SL", "SL1", lSL1Dia, lTotalLength, 1,
                               pBPCModels.cage_dia, int.Parse(lSpacingArr[0]), lSL1Length,
                               lStartRings + lChangeAddRing1 + lAddnRingNo *
                               (int)Math.Round((double)lAddnRingSet * lSL1Length / (lSL1Length + lSL2Length + lSL3Length)) + lSLLappingRound,
                               0, 0, 0);
                    lRebarList.Add(lRebar);
                    lBarID++;
                    lBarSort++;
                }
                if (lSpacingArr.Length > 1)
                {
                    int lTotalLength = (int)Math.Round(((double)lSL2Length / int.Parse(lSpacingArr[1]) + lChangeAddRing1 + lChangeAddRing2 + lAddnRingNo *
                        (Math.Round((double)lAddnRingSet * lSL2Length / (lSL1Length + lSL2Length + lSL3Length)))) *
                    Math.Sqrt(Math.Pow(pBPCModels.cage_dia * Math.PI, 2) + Math.Pow(int.Parse(lSpacingArr[1]), 2)));

                    if (lSlLapping == true && lTotalLength > 0)
                    {
                        lSLLappingRound = (int)Math.Round((double)(Math.Ceiling((double)lTotalLength / 12000) - 1) * 51 * lSL1Dia / Math.Sqrt(Math.Pow(pBPCModels.cage_dia * Math.PI, 2) + Math.Pow(int.Parse(lSpacingArr[1]), 2)));
                        lTotalLength = lTotalLength + (int)Math.Round((double)lSLLappingRound * Math.Sqrt(Math.Pow(pBPCModels.cage_dia * Math.PI, 2) + Math.Pow(int.Parse(lSpacingArr[1]), 2)));
                    }

                    lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "R7A",
                               "SL", "SL2", lSL2Dia, lTotalLength, 1,
                               pBPCModels.cage_dia, int.Parse(lSpacingArr[1]), lSL2Length,
                               lChangeAddRing1 + lChangeAddRing2 + lAddnRingNo *
                               (int)(Math.Round((double)lAddnRingSet * lSL2Length / (lSL1Length + lSL2Length + lSL3Length))) + lSLLappingRound,
                               0, 0, 0);
                    lRebarList.Add(lRebar);
                    lBarID++;
                    lBarSort++;
                }
                if (lSpacingArr.Length > 2)
                {
                    int lTotalLength = (int)Math.Round(((double)lSL3Length / int.Parse(lSpacingArr[2]) + lEndRings + lChangeAddRing2 + lAddnRingNo * (lAddnRingSet -
                        Math.Round((double)lAddnRingSet * lSL1Length / (lSL1Length + lSL2Length + lSL3Length)) -
                        Math.Round((double)lAddnRingSet * lSL2Length / (lSL1Length + lSL2Length + lSL3Length))
                        )) *
                    Math.Sqrt(Math.Pow(pBPCModels.cage_dia * Math.PI, 2) + Math.Pow(int.Parse(lSpacingArr[2]), 2)));

                    if (lSlLapping == true && lTotalLength > 0)
                    {
                        lSLLappingRound = (int)Math.Round((double)(Math.Ceiling((double)lTotalLength / 12000) - 1) * 51 * lSL1Dia / Math.Sqrt(Math.Pow(pBPCModels.cage_dia * Math.PI, 2) + Math.Pow(int.Parse(lSpacingArr[2]), 2)));
                        lTotalLength = lTotalLength + (int)Math.Round((double)lSLLappingRound * Math.Sqrt(Math.Pow(pBPCModels.cage_dia * Math.PI, 2) + Math.Pow(int.Parse(lSpacingArr[2]), 2)));
                    }

                    lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "R7A",
                               "SL", "SL3", lSL3Dia, lTotalLength, 1,
                               pBPCModels.cage_dia, int.Parse(lSpacingArr[2]), lSL3Length,
                               lEndRings + lChangeAddRing2 + lAddnRingNo * (lAddnRingSet -
                               (int)Math.Round((double)lAddnRingSet * lSL1Length / (lSL1Length + lSL2Length + lSL3Length)) -
                               (int)Math.Round((double)lAddnRingSet * lSL2Length / (lSL1Length + lSL2Length + lSL3Length))) + lSLLappingRound,
                                0, 0, 0);
                    lRebarList.Add(lRebar);
                    lBarID++;
                    lBarSort++;
                }
            }
            else if (lSLType == "Twin 1 Spacing")
            {
                int lTotalLength = (int)(((double)lSL1Length / int.Parse(lSLSpacing) + lStartRings + lEndRings + lAddnRingNo * lAddnRingSet) *
                    Math.Sqrt(Math.Pow(pBPCModels.cage_dia * Math.PI, 2) + Math.Pow(int.Parse(lSLSpacing), 2)));

                if (lSlLapping == true && lTotalLength > 0)
                {
                    lSLLappingRound = (int)Math.Round((double)(Math.Ceiling((double)lTotalLength / 12000) - 1) * 51 * lSL1Dia / Math.Sqrt(Math.Pow(pBPCModels.cage_dia * Math.PI, 2) + Math.Pow(int.Parse(lSLSpacing), 2)));
                    lTotalLength = lTotalLength + (int)Math.Round((double)lSLLappingRound * Math.Sqrt(Math.Pow(pBPCModels.cage_dia * Math.PI, 2) + Math.Pow(int.Parse(lSLSpacing), 2)));
                }

                lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "R7A",
                           "SL", "SL", lSL1Dia, lTotalLength, 2,
                           pBPCModels.cage_dia, int.Parse(lSLSpacing), lSL1Length,
                           lStartRings + lEndRings + lAddnRingNo * lAddnRingSet + lSLLappingRound,
                           0, 0, 0);
                lRebarList.Add(lRebar);
                lBarID++;
                lBarSort++;
            }
            else if (lSLType == "Twin 2 Spacing")
            {
                var lSpacingArr = lSLSpacing.Split(',');
                var lChangeAddRing = 0;
                if (lSL1Dia != lSL2Dia)
                {
                    lChangeAddRing = 2;
                }
                if (lSpacingArr.Length > 0)
                {
                    int lTotalLength = (int)(((double)lSL1Length / int.Parse(lSpacingArr[0]) + lStartRings + lChangeAddRing + lAddnRingNo *
                        Math.Round((double)lAddnRingSet * lSL1Length / (lSL1Length + lSL2Length))) *
                    Math.Sqrt(Math.Pow(pBPCModels.cage_dia * Math.PI, 2) + Math.Pow(int.Parse(lSpacingArr[0]), 2)));

                    if (lSlLapping == true && lTotalLength > 0)
                    {
                        lSLLappingRound = (int)Math.Round((double)(Math.Ceiling((double)lTotalLength / 12000) - 1) * 51 * lSL1Dia / Math.Sqrt(Math.Pow(pBPCModels.cage_dia * Math.PI, 2) + Math.Pow(int.Parse(lSpacingArr[0]), 2)));
                        lTotalLength = lTotalLength + (int)Math.Round((double)lSLLappingRound * Math.Sqrt(Math.Pow(pBPCModels.cage_dia * Math.PI, 2) + Math.Pow(int.Parse(lSpacingArr[0]), 2)));
                    }

                    lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "R7A",
                               "SL", "SL1", lSL1Dia, lTotalLength, 2,
                               pBPCModels.cage_dia, int.Parse(lSpacingArr[0]), lSL1Length,
                               lStartRings + lChangeAddRing + lAddnRingNo *
                               (int)Math.Round((double)lAddnRingSet * lSL1Length / (lSL1Length + lSL2Length)) + lSLLappingRound,
                               0, 0, 0);
                    lRebarList.Add(lRebar);
                    lBarID++;
                    lBarSort++;
                }
                if (lSpacingArr.Length > 1)
                {
                    int lTotalLength = (int)(((double)lSL2Length / int.Parse(lSpacingArr[1]) + lEndRings + lChangeAddRing + lAddnRingNo *
                        (lAddnRingSet - Math.Round((double)lAddnRingSet * lSL1Length / (lSL1Length + lSL2Length)))) *
                    Math.Sqrt(Math.Pow(pBPCModels.cage_dia * Math.PI, 2) + Math.Pow(int.Parse(lSpacingArr[1]), 2)));

                    if (lSlLapping == true && lTotalLength > 0)
                    {
                        lSLLappingRound = (int)Math.Round((double)(Math.Ceiling((double)lTotalLength / 12000) - 1) * 51 * lSL1Dia / Math.Sqrt(Math.Pow(pBPCModels.cage_dia * Math.PI, 2) + Math.Pow(int.Parse(lSpacingArr[1]), 2)));
                        lTotalLength = lTotalLength + (int)Math.Round((double)lSLLappingRound * Math.Sqrt(Math.Pow(pBPCModels.cage_dia * Math.PI, 2) + Math.Pow(int.Parse(lSpacingArr[1]), 2)));
                    }

                    lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "R7A",
                               "SL", "SL2", lSL2Dia, lTotalLength, 2,
                               pBPCModels.cage_dia, int.Parse(lSpacingArr[1]), lSL2Length,
                               lEndRings + lChangeAddRing + lAddnRingNo *
                               (lAddnRingSet - (int)Math.Round((double)lAddnRingSet * lSL1Length / (lSL1Length + lSL2Length))) + lSLLappingRound,
                               0, 0, 0);
                    lRebarList.Add(lRebar);
                    lBarID++;
                    lBarSort++;
                }
            }
            else if (lSLType == "Twin 3 Spacing")
            {
                var lSpacingArr = lSLSpacing.Split(',');
                var lChangeAddRing1 = 0;
                var lChangeAddRing2 = 0;
                if (lSL1Dia != lSL2Dia)
                {
                    lChangeAddRing1 = 2;
                }
                if (lSL2Dia != lSL3Dia)
                {
                    lChangeAddRing2 = 2;
                }
                if (lSpacingArr.Length > 0)
                {
                    int lTotalLength = (int)(((double)lSL1Length / int.Parse(lSpacingArr[0]) + lStartRings + lChangeAddRing1 +
                        lAddnRingNo * Math.Round((double)lAddnRingSet * lSL1Length / (lSL1Length + lSL2Length + lSL3Length))) *
                    Math.Sqrt(Math.Pow(pBPCModels.cage_dia * Math.PI, 2) + Math.Pow(int.Parse(lSpacingArr[0]), 2)));

                    if (lSlLapping == true && lTotalLength > 0)
                    {
                        lSLLappingRound = (int)Math.Round((double)(Math.Ceiling((double)lTotalLength / 12000) - 1) * 51 * lSL1Dia / Math.Sqrt(Math.Pow(pBPCModels.cage_dia * Math.PI, 2) + Math.Pow(int.Parse(lSpacingArr[0]), 2)));
                        lTotalLength = lTotalLength + (int)Math.Round((double)lSLLappingRound * Math.Sqrt(Math.Pow(pBPCModels.cage_dia * Math.PI, 2) + Math.Pow(int.Parse(lSpacingArr[0]), 2)));
                    }

                    lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "R7A",
                               "SL", "SL1", lSL1Dia, lTotalLength, 2,
                               pBPCModels.cage_dia, int.Parse(lSpacingArr[0]), lSL1Length,
                               lStartRings + lChangeAddRing1 +
                               (int)lAddnRingNo * (int)Math.Round((double)lAddnRingSet * lSL1Length / (lSL1Length + lSL2Length + lSL3Length)) + lSLLappingRound,
                               0, 0, 0);
                    lRebarList.Add(lRebar);
                    lBarID++;
                    lBarSort++;
                }
                if (lSpacingArr.Length > 1)
                {
                    int lTotalLength = (int)(((double)lSL2Length / int.Parse(lSpacingArr[1]) + lChangeAddRing1 + lChangeAddRing2 +
                        lAddnRingNo * (Math.Round((double)lAddnRingSet * lSL2Length / (lSL1Length + lSL2Length + lSL3Length)))) *
                    Math.Sqrt(Math.Pow(pBPCModels.cage_dia * Math.PI, 2) + Math.Pow(int.Parse(lSpacingArr[1]), 2)));

                    if (lSlLapping == true && lTotalLength > 0)
                    {
                        lSLLappingRound = (int)Math.Round((double)(Math.Ceiling((double)lTotalLength / 12000) - 1) * 51 * lSL1Dia / Math.Sqrt(Math.Pow(pBPCModels.cage_dia * Math.PI, 2) + Math.Pow(int.Parse(lSpacingArr[1]), 2)));
                        lTotalLength = lTotalLength + (int)Math.Round((double)lSLLappingRound * Math.Sqrt(Math.Pow(pBPCModels.cage_dia * Math.PI, 2) + Math.Pow(int.Parse(lSpacingArr[1]), 2)));
                    }

                    lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "R7A",
                               "SL", "SL2", lSL2Dia, lTotalLength, 2,
                               pBPCModels.cage_dia, int.Parse(lSpacingArr[1]), lSL2Length,
                               lChangeAddRing1 + lChangeAddRing2 +
                               lAddnRingNo * (int)(Math.Round((double)lAddnRingSet * lSL2Length / (lSL1Length + lSL2Length + lSL3Length))) + lSLLappingRound,
                               0, 0, 0);
                    lRebarList.Add(lRebar);
                    lBarID++;
                    lBarSort++;
                }
                if (lSpacingArr.Length > 2)
                {
                    int lTotalLength = (int)(((double)lSL3Length / int.Parse(lSpacingArr[2]) + lEndRings + lChangeAddRing2 + lAddnRingNo * (lAddnRingSet -
                        Math.Round((double)lAddnRingSet * lSL1Length / (lSL1Length + lSL2Length + lSL3Length)) -
                        Math.Round((double)lAddnRingSet * lSL2Length / (lSL1Length + lSL2Length + lSL3Length)))) *
                    Math.Sqrt(Math.Pow(pBPCModels.cage_dia * Math.PI, 2) + Math.Pow(int.Parse(lSpacingArr[2]), 2)));

                    if (lSlLapping == true && lTotalLength > 0)
                    {
                        lSLLappingRound = (int)Math.Round((double)(Math.Ceiling((double)lTotalLength / 12000) - 1) * 51 * lSL1Dia / Math.Sqrt(Math.Pow(pBPCModels.cage_dia * Math.PI, 2) + Math.Pow(int.Parse(lSpacingArr[2]), 2)));
                        lTotalLength = lTotalLength + (int)Math.Round((double)lSLLappingRound * Math.Sqrt(Math.Pow(pBPCModels.cage_dia * Math.PI, 2) + Math.Pow(int.Parse(lSpacingArr[2]), 2)));
                    }

                    lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "R7A",
                               "SL", "SL3", lSL3Dia, lTotalLength, 2,
                               pBPCModels.cage_dia, int.Parse(lSpacingArr[2]), lSL3Length,
                               lEndRings + lChangeAddRing2 + lAddnRingNo * (lAddnRingSet -
                               (int)Math.Round((double)lAddnRingSet * lSL1Length / (lSL1Length + lSL2Length + lSL3Length)) -
                               (int)Math.Round((double)lAddnRingSet * lSL2Length / (lSL1Length + lSL2Length + lSL3Length))) + lSLLappingRound,
                               0, 0, 0);
                    lRebarList.Add(lRebar);
                    lBarID++;
                    lBarSort++;
                }
            }
            else if (lSLType == "Single-Twin")
            {
                var lSpacingArr = lSLSpacing.Split(',');
                var lChangeAddRing = 0;
                if (lSL1Dia != lSL2Dia)
                {
                    lChangeAddRing = 2;
                }
                if (lSpacingArr.Length > 0)
                {
                    int lTotalLength = (int)Math.Round(((double)lSL1Length / int.Parse(lSpacingArr[0]) + lStartRings + lChangeAddRing + lAddnRingNo *
                        Math.Round((double)lAddnRingSet * lSL1Length / (lSL1Length + lSL2Length))) *
                    Math.Sqrt(Math.Pow(pBPCModels.cage_dia * Math.PI, 2) + Math.Pow(int.Parse(lSpacingArr[0]), 2)));

                    if (lSlLapping == true && lTotalLength > 0)
                    {
                        lSLLappingRound = (int)Math.Round((double)(Math.Ceiling((double)lTotalLength / 12000) - 1) * 51 * lSL1Dia / Math.Sqrt(Math.Pow(pBPCModels.cage_dia * Math.PI, 2) + Math.Pow(int.Parse(lSpacingArr[0]), 2)));
                        lTotalLength = lTotalLength + (int)Math.Round((double)lSLLappingRound * Math.Sqrt(Math.Pow(pBPCModels.cage_dia * Math.PI, 2) + Math.Pow(int.Parse(lSpacingArr[0]), 2)));
                    }

                    lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "R7A",
                               "SL", "SL1", lSL1Dia, lTotalLength, 1,
                               pBPCModels.cage_dia, int.Parse(lSpacingArr[0]), lSL1Length,
                               lStartRings + lChangeAddRing + lAddnRingNo *
                               (int)Math.Round((double)lAddnRingSet * lSL1Length / (lSL1Length + lSL2Length)) + lSLLappingRound,
                               0, 0, 0);
                    lRebarList.Add(lRebar);
                    lBarID++;
                    lBarSort++;
                }
                if (lSpacingArr.Length > 1)
                {
                    int lTotalLength = (int)(((double)lSL2Length / int.Parse(lSpacingArr[1]) + lEndRings + lChangeAddRing + lAddnRingNo *
                        (lAddnRingSet - Math.Round((double)lAddnRingSet * lSL1Length / (lSL1Length + lSL2Length)))) *
                    Math.Sqrt(Math.Pow(pBPCModels.cage_dia * Math.PI, 2) + Math.Pow(int.Parse(lSpacingArr[1]), 2)));

                    if (lSlLapping == true && lTotalLength > 0)
                    {
                        lSLLappingRound = (int)Math.Round((double)(Math.Ceiling((double)lTotalLength / 12000) - 1) * 51 * lSL1Dia / Math.Sqrt(Math.Pow(pBPCModels.cage_dia * Math.PI, 2) + Math.Pow(int.Parse(lSpacingArr[1]), 2)));
                        lTotalLength = lTotalLength + (int)Math.Round((double)lSLLappingRound * Math.Sqrt(Math.Pow(pBPCModels.cage_dia * Math.PI, 2) + Math.Pow(int.Parse(lSpacingArr[1]), 2)));
                    }

                    lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "R7A",
                               "SL", "SL2", lSL2Dia, lTotalLength, 2,
                               pBPCModels.cage_dia, int.Parse(lSpacingArr[1]), lSL2Length,
                               lEndRings + lChangeAddRing + lAddnRingNo *
                               (lAddnRingSet - (int)Math.Round((double)lAddnRingSet * lSL1Length / (lSL1Length + lSL2Length))) + lSLLappingRound,
                               0, 0, 0);
                    lRebarList.Add(lRebar);
                    lBarID++;
                    lBarSort++;
                }
            }
            else if (lSLType == "Twin-Single")
            {
                var lSpacingArr = lSLSpacing.Split(',');
                var lChangeAddRing = 0;
                if (lSL1Dia != lSL2Dia)
                {
                    lChangeAddRing = 2;
                }
                if (lSpacingArr.Length > 0)
                {
                    int lTotalLength = (int)(((double)lSL1Length / int.Parse(lSpacingArr[0]) + lStartRings + lChangeAddRing + lAddnRingNo *
                        Math.Round((double)lAddnRingSet * lSL1Length / (lSL1Length + lSL2Length))) *
                    Math.Sqrt(Math.Pow(pBPCModels.cage_dia * Math.PI, 2) + Math.Pow(int.Parse(lSpacingArr[0]), 2)));

                    if (lSlLapping == true && lTotalLength > 0)
                    {
                        lSLLappingRound = (int)Math.Round((double)(Math.Ceiling((double)lTotalLength / 12000) - 1) * 51 * lSL1Dia / Math.Sqrt(Math.Pow(pBPCModels.cage_dia * Math.PI, 2) + Math.Pow(int.Parse(lSpacingArr[0]), 2)));
                        lTotalLength = lTotalLength + (int)Math.Round((double)lSLLappingRound * Math.Sqrt(Math.Pow(pBPCModels.cage_dia * Math.PI, 2) + Math.Pow(int.Parse(lSpacingArr[0]), 2)));
                    }

                    lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "R7A",
                               "SL", "SL1", lSL1Dia, lTotalLength, 2,
                               pBPCModels.cage_dia, int.Parse(lSpacingArr[0]), lSL1Length,
                               lStartRings + lChangeAddRing + lAddnRingNo *
                               (int)Math.Round((double)lAddnRingSet * lSL1Length / (lSL1Length + lSL2Length)) + lSLLappingRound,
                               0, 0, 0);
                    lRebarList.Add(lRebar);
                    lBarID++;
                    lBarSort++;
                }
                if (lSpacingArr.Length > 1)
                {
                    int lTotalLength = (int)Math.Round(((double)lSL2Length / int.Parse(lSpacingArr[1]) + lEndRings + lChangeAddRing + lAddnRingNo *
                        (lAddnRingSet - Math.Round((double)lAddnRingSet * lSL1Length / (lSL1Length + lSL2Length)))) *
                    Math.Sqrt(Math.Pow(pBPCModels.cage_dia * Math.PI, 2) + Math.Pow(int.Parse(lSpacingArr[1]), 2)));

                    if (lSlLapping == true && lTotalLength > 0)
                    {
                        lSLLappingRound = (int)Math.Round((double)(Math.Ceiling((double)lTotalLength / 12000) - 1) * 51 * lSL1Dia / Math.Sqrt(Math.Pow(pBPCModels.cage_dia * Math.PI, 2) + Math.Pow(int.Parse(lSpacingArr[1]), 2)));
                        lTotalLength = lTotalLength + (int)Math.Round((double)lSLLappingRound * Math.Sqrt(Math.Pow(pBPCModels.cage_dia * Math.PI, 2) + Math.Pow(int.Parse(lSpacingArr[1]), 2)));
                    }

                    lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "R7A",
                               "SL", "SL2", lSL2Dia, lTotalLength, 1,
                               pBPCModels.cage_dia, int.Parse(lSpacingArr[1]), lSL2Length,
                               lEndRings + lChangeAddRing + lAddnRingNo *
                               (lAddnRingSet - (int)Math.Round((double)lAddnRingSet * lSL1Length / (lSL1Length + lSL2Length))) + lSLLappingRound,
                               0, 0, 0);
                    lRebarList.Add(lRebar);
                    lBarID++;
                    lBarSort++;
                }
            }

            if (lPileType == "Micro-Pile")
            {
                return lRebarList;
            }

            // Stiffener Rings 
            var lSupportBar = pBPCModels.extra_support_bar_ind == null ? "None" : pBPCModels.extra_support_bar_ind;
            int lSupportBarDia = pBPCModels.extra_support_bar_dia == null ? 0 : (int)pBPCModels.extra_support_bar_dia;

            var lNoOfSR = pBPCModels.no_of_sr;
            var lSRDia = pBPCModels.sr_dia;
            var lSRDiaAdd = pBPCModels.sr_dia_add;
            var lSLDia = pBPCModels.sl1_dia;
            if (pBPCModels.sl2_dia > lSLDia)
            {
                lSLDia = pBPCModels.sl2_dia;
            }
            if (pBPCModels.sl3_dia > lSLDia)
            {
                lSLDia = pBPCModels.sl3_dia;
            }

            if (lSRDiaAdd == null)
            {
                lSRDiaAdd = 0;
            }
            int lLapSR = 0;
            int lEndSR = 0;

            if (lNoOfSR == 5 && pBPCModels.sr5_location > 0 && int.Parse(pBPCModels.cage_length) - pBPCModels.sr5_location < lEndLength)
            {
                lEndSR = 1;
                lNoOfSR = lNoOfSR - 1;
            }

            if (lNoOfSR == 4 && pBPCModels.sr4_location > 0 && int.Parse(pBPCModels.cage_length) - pBPCModels.sr4_location < lEndLength)
            {
                lEndSR = 1;
                lNoOfSR = lNoOfSR - 1;
            }

            if (lNoOfSR == 3 && pBPCModels.sr3_location > 0 && int.Parse(pBPCModels.cage_length) - pBPCModels.sr3_location < lEndLength)
            {
                lEndSR = 1;
                lNoOfSR = lNoOfSR - 1;
            }

            if (lNoOfSR > 1 && pBPCModels.sr1_location < lLapLength)
            {
                lLapSR = 1;
                lNoOfSR = lNoOfSR - 1;
            }

            if (lPileType == "Single-Layer" && lMainBarType == "Single")
            {
                if (pBPCModels.main_bar_arrange == "In-Out")
                {
                    int lTotalLength = (int)Math.Round((double)(lCageDia - 2 * lSLDia - 4 * int.Parse(lMainBarDia)) * Math.PI + sr_link_lapping * lSRDia);
                    string lBarMark = "SR";
                    if (lLapSR > 0 || lEndSR > 0 || ((lSupportBar == "Cross" || lSupportBar == "Square") && lSupportBarDia > 0))
                    {
                        lBarMark = "SR1";
                    }
                    lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "R88",
                               "SR", lBarMark, lSRDia, lTotalLength, lNoOfSR,
                               0, 0,
                               lCageDia - 2 * lSLDia - 4 * int.Parse(lMainBarDia),
                               sr_link_lapping * lSRDia,
                               0, 0, 0);
                    lRebarList.Add(lRebar);
                    lBarID++;
                    lBarSort++;

                    //additional SR
                    if (lLapSR > 0)
                    {
                        var lNoOfSRa = 1;
                        if (lMainBarShape == "Crank-Top" || lMainBarShape == "Crank" || lMainBarShape == "Crank-Both")
                        {
                            if (pBPCModels.sr1_location > pBPCModels.mainbar_location_2layer)
                            {
                                lSRDia = 13;
                                if (lSRDiaAdd != 0)
                                {
                                    lSRDia = (int)lSRDiaAdd;
                                }
                                lTotalLength = (int)Math.Round((double)(lCageDia - 2 * lSLDia - 2 * int.Parse(lMainBarDia) - 2 * lEndCrankHT) * Math.PI + sr_link_lapping * lSRDia);
                                lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "R88",
                                       "SR", "SR2", lSRDia, lTotalLength, lNoOfSRa,
                                       0, 0,
                                       lCageDia - 2 * lSLDia - 2 * int.Parse(lMainBarDia) - 2 * lEndCrankHT,
                                       sr_link_lapping * lSRDia,
                                       0, 0, 0);
                                lRebarList.Add(lRebar);
                                lBarID++;
                                lBarSort++;
                            }
                            else
                            {
                                lSRDia = 13;
                                if (lSRDiaAdd != 0)
                                {
                                    lSRDia = (int)lSRDiaAdd;
                                }
                                lTotalLength = (int)Math.Round((double)(lCageDia - 2 * lSLDia - 2 * lEndCrankHT) * Math.PI + sr_link_lapping * lSRDia);
                                lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "R88",
                                               "SR", "SR2", lSRDia, lTotalLength, lNoOfSRa,
                                               0, 0,
                                               lCageDia - 2 * lSLDia - 2 * lEndCrankHT,
                                               sr_link_lapping * lSRDia,
                                               0, 0, 0);
                                lRebarList.Add(lRebar);
                                lBarID++;
                                lBarSort++;
                            }
                        }
                        else
                        {
                            if (pBPCModels.sr1_location > pBPCModels.mainbar_location_2layer)
                            {
                                lSRDia = 13;
                                if (lSRDiaAdd != 0)
                                {
                                    lSRDia = (int)lSRDiaAdd;
                                }
                                lTotalLength = (int)Math.Round((double)(lCageDia - 2 * lSLDia - 4 * int.Parse(lMainBarDia)) * Math.PI + sr_link_lapping * lSRDia);
                                lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "R88",
                                       "SR", "SR2", lSRDia, lTotalLength, lNoOfSRa,
                                       0, 0,
                                       lCageDia - 2 * lSLDia - 4 * int.Parse(lMainBarDia),
                                       sr_link_lapping * lSRDia,
                                       0, 0, 0);
                                lRebarList.Add(lRebar);
                                lBarID++;
                                lBarSort++;
                            }
                            else
                            {
                                lSRDia = 13;
                                if (lSRDiaAdd != 0)
                                {
                                    lSRDia = (int)lSRDiaAdd;
                                }
                                lTotalLength = (int)Math.Round((double)(lCageDia - 2 * lSLDia - 2 * int.Parse(lMainBarDia)) * Math.PI + sr_link_lapping * lSRDia);
                                lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "R88",
                                               "SR", "SR2", lSRDia, lTotalLength, lNoOfSR,
                                               0, 0,
                                               lCageDia - 2 * lSLDia - 2 * int.Parse(lMainBarDia),
                                               sr_link_lapping * lSRDia,
                                               0, 0, 0);
                                lRebarList.Add(lRebar);
                                lBarID++;
                                lBarSort++;
                            }
                        }

                    }

                    if (lEndSR > 0)
                    {
                        var lNoOfSRb = 1;
                        lBarMark = "SR2";
                        if (lLapSR > 0)
                        {
                            lBarMark = "SR3";
                        }
                        int lLastSRLoc = pBPCModels.sr5_location;
                        if (lLastSRLoc == 0) lLastSRLoc = pBPCModels.sr4_location;
                        if (lLastSRLoc == 0) lLastSRLoc = pBPCModels.sr3_location;
                        if (lLastSRLoc == 0) lLastSRLoc = pBPCModels.sr2_location;
                        if (lLastSRLoc == 0) lLastSRLoc = pBPCModels.sr1_location;
                        if (lLastSRLoc > pBPCModels.mainbar_location_2layer + pBPCModels.mainbar_length_2layer)
                        {
                            if (lMainBarShape == "Crank-End" || lMainBarShape == "Crank-Both")
                            {
                                lSRDia = 13;
                                if (lSRDiaAdd != 0)
                                {
                                    lSRDia = (int)lSRDiaAdd;
                                }
                                lTotalLength = (int)Math.Round((double)(lCageDia - 2 * lSLDia - 2 * int.Parse(lMainBarDia) - 2 * lEndCrankHT) * Math.PI + sr_link_lapping * lSRDia);
                                lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "R88",
                                       "SR", lBarMark, lSRDia, lTotalLength, lNoOfSRb,
                                       0, 0,
                                       lCageDia - 2 * lSLDia - 2 * int.Parse(lMainBarDia) - 2 * lEndCrankHT,
                                       sr_link_lapping * lSRDia,
                                       0, 0, 0);
                                lRebarList.Add(lRebar);
                                lBarID++;
                                lBarSort++;
                            }
                            else
                            {
                                lSRDia = 13;
                                if (lSRDiaAdd != 0)
                                {
                                    lSRDia = (int)lSRDiaAdd;
                                }
                                lTotalLength = (int)Math.Round((double)(lCageDia - 2 * lSLDia - 4 * int.Parse(lMainBarDia)) * Math.PI + sr_link_lapping * lSRDia);
                                lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "R88",
                                       "SR", lBarMark, lSRDia, lTotalLength, lNoOfSRb,
                                       0, 0,
                                       lCageDia - 2 * lSLDia - 4 * int.Parse(lMainBarDia),
                                       sr_link_lapping * lSRDia,
                                       0, 0, 0);
                                lRebarList.Add(lRebar);
                                lBarID++;
                                lBarSort++;
                            }
                        }
                        else
                        {
                            if (lMainBarShape == "Crank-End" || lMainBarShape == "Crank-Both")
                            {
                                lSRDia = 13;
                                if (lSRDiaAdd != 0)
                                {
                                    lSRDia = (int)lSRDiaAdd;
                                }
                                lTotalLength = (int)Math.Round((double)(lCageDia - 2 * lSLDia - 2 * lEndCrankHT) * Math.PI + sr_link_lapping * lSRDia);
                                lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "R88",
                                               "SR", lBarMark, lSRDia, lTotalLength, lNoOfSRb,
                                               0, 0,
                                               lCageDia - 2 * lSLDia - 2 * lEndCrankHT,
                                               sr_link_lapping * lSRDia,
                                               0, 0, 0);
                                lRebarList.Add(lRebar);
                                lBarID++;
                                lBarSort++;
                            }
                            else
                            {
                                lSRDia = 13;
                                if (lSRDiaAdd != 0)
                                {
                                    lSRDia = (int)lSRDiaAdd;
                                }
                                lTotalLength = (int)Math.Round((double)(lCageDia - 2 * lSLDia - 2 * int.Parse(lMainBarDia)) * Math.PI + sr_link_lapping * lSRDia);
                                lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "R88",
                                               "SR", lBarMark, lSRDia, lTotalLength, lNoOfSRb,
                                               0, 0,
                                               lCageDia - 2 * lSLDia - 2 * int.Parse(lMainBarDia),
                                               sr_link_lapping * lSRDia,
                                               0, 0, 0);
                                lRebarList.Add(lRebar);
                                lBarID++;
                                lBarSort++;
                            }

                        }
                    }

                }
                else
                {
                    int lTotalLength = (int)Math.Round((double)(lCageDia - 2 * lSLDia - 2 * int.Parse(lMainBarDia)) * Math.PI + sr_link_lapping * lSRDia);
                    string lBarMark = "SR";
                    if (lLapSR > 0 || lEndSR > 0 || ((lSupportBar == "Cross" || lSupportBar == "Square") && lSupportBarDia > 0))
                    {
                        lBarMark = "SR1";
                    }
                    lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "R88",
                               "SR", lBarMark, lSRDia, lTotalLength, lNoOfSR,
                               0, 0,
                               lCageDia - 2 * lSLDia - 2 * int.Parse(lMainBarDia),
                               sr_link_lapping * lSRDia,
                               0, 0, 0);
                    lRebarList.Add(lRebar);
                    lBarID++;
                    lBarSort++;
                    //additional SR
                    if (lLapSR > 0)
                    {
                        var lNoOfSRc = 1;
                        if (lMainBarShape == "Crank-Top" || lMainBarShape == "Crank" || lMainBarShape == "Crank-Both")
                        {
                            lSRDia = 13;
                            if (lSRDiaAdd != 0)
                            {
                                lSRDia = (int)lSRDiaAdd;
                            }
                            lTotalLength = (int)Math.Round((double)(lCageDia - 2 * lSLDia - 2 * lTopCrankHT) * Math.PI + sr_link_lapping * lSRDia);
                            lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "R88",
                                   "SR", "SR2", lSRDia, lTotalLength, lNoOfSRc,
                                   0, 0,
                                   lCageDia - 2 * lSLDia - 2 * lTopCrankHT,
                                   sr_link_lapping * lSRDia,
                                   0, 0, 0);
                            lRebarList.Add(lRebar);
                            lBarID++;
                            lBarSort++;
                        }
                        else
                        {
                            lSRDia = 13;
                            if (lSRDiaAdd != 0)
                            {
                                lSRDia = (int)lSRDiaAdd;
                            }
                            lTotalLength = (int)Math.Round((double)(lCageDia - 2 * lSLDia - 2 * int.Parse(lMainBarDia)) * Math.PI + sr_link_lapping * lSRDia);
                            lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "R88",
                                   "SR", "SR2", lSRDia, lTotalLength, lNoOfSRc,
                                   0, 0,
                                   lCageDia - 2 * lSLDia - 2 * int.Parse(lMainBarDia),
                                   sr_link_lapping * lSRDia,
                                   0, 0, 0);
                            lRebarList.Add(lRebar);
                            lBarID++;
                            lBarSort++;
                        }

                    }

                    if (lEndSR > 0)
                    {
                        lBarMark = "SR2";
                        if (lLapSR > 0)
                        {
                            lBarMark = "SR3";
                        }
                        var lNoOfSRd = 1;
                        if (lMainBarShape == "Crank-End" || lMainBarShape == "Crank-Both")
                        {
                            lSRDia = 13;
                            if (lSRDiaAdd != 0)
                            {
                                lSRDia = (int)lSRDiaAdd;
                            }
                            lTotalLength = (int)Math.Round((double)(lCageDia - 2 * lSLDia - 2 * lEndCrankHT) * Math.PI + sr_link_lapping * lSRDia);
                            lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "R88",
                                   "SR", lBarMark, lSRDia, lTotalLength, lNoOfSRd,
                                   0, 0,
                                   lCageDia - 2 * lSLDia - 2 * lEndCrankHT,
                                   sr_link_lapping * lSRDia,
                                   0, 0, 0);
                            lRebarList.Add(lRebar);
                            lBarID++;
                            lBarSort++;
                        }
                        else
                        {
                            lSRDia = 13;
                            if (lSRDiaAdd != 0)
                            {
                                lSRDia = (int)lSRDiaAdd;
                            }
                            lTotalLength = (int)Math.Round((double)(lCageDia - 2 * lSLDia - 2 * int.Parse(lMainBarDia)) * Math.PI + sr_link_lapping * lSRDia);
                            lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "R88",
                                   "SR", lBarMark, lSRDia, lTotalLength, lNoOfSRd,
                                   0, 0,
                                   lCageDia - 2 * lSLDia - 2 * int.Parse(lMainBarDia),
                                   sr_link_lapping * lSRDia,
                                   0, 0, 0);
                            lRebarList.Add(lRebar);
                            lBarID++;
                            lBarSort++;
                        }
                    }
                }
            }
            else if (lPileType == "Single-Layer" && lMainBarType == "Mixed")
            {
                var lMainbarDiaArr = lMainBarDia.Split(',');
                var lMainbarCTArr = lMainBarCT.Split(',');

                if (lMainbarDiaArr.Length > 1 && lMainbarCTArr.Length > 1)
                {
                    var lMainBarDia1 = int.Parse(lMainbarDiaArr[0]);
                    var lMainBarDia2 = int.Parse(lMainbarDiaArr[1]);

                    if (pBPCModels.main_bar_arrange == "In-Out")
                    {
                        if (pBPCModels.bundle_same_type != "Y")
                        {
                            int lTotalLength = (int)Math.Round((double)(lCageDia - 2 * lSLDia - 2 * (lMainBarDia1 + lMainBarDia2)) * Math.PI + sr_link_lapping * lSRDia);
                            string lBarMark = "SR";
                            if (lLapSR > 0 || lEndSR > 0 || ((lSupportBar == "Cross" || lSupportBar == "Square") && lSupportBarDia > 0))
                            {
                                lBarMark = "SR1";
                            }
                            lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "R88",
                                   "SR", lBarMark, lSRDia, lTotalLength, lNoOfSR,
                                   0, 0,
                                   lCageDia - 2 * lSLDia - 2 * (lMainBarDia1 + lMainBarDia2),
                                   sr_link_lapping * lSRDia,
                                   0, 0, 0);
                            lRebarList.Add(lRebar);
                            lBarID++;
                            lBarSort++;

                            //additional SR
                            if (lLapSR > 0)
                            {
                                var lNoOfSRa = 1;
                                if (lMainBarShape == "Crank-Top" || lMainBarShape == "Crank" || lMainBarShape == "Crank-Both")
                                {
                                    if (pBPCModels.sr1_location > pBPCModels.mainbar_location_2layer)
                                    {
                                        lSRDia = 13;
                                        if (lSRDiaAdd != 0)
                                        {
                                            lSRDia = (int)lSRDiaAdd;
                                        }
                                        lTotalLength = (int)Math.Round((double)(lCageDia - 2 * lSLDia - 2 * lTopCrankHT - 2 * Math.Min(lMainBarDia1, lMainBarDia2)) * Math.PI + sr_link_lapping * lSRDia);
                                        lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "R88",
                                               "SR", "SR2", lSRDia, lTotalLength, lNoOfSRa,
                                               0, 0,
                                               lCageDia - 2 * lSLDia - 2 * lTopCrankHT - 2 * Math.Min(lMainBarDia1, lMainBarDia2),
                                               sr_link_lapping * lSRDia,
                                               0, 0, 0);
                                        lRebarList.Add(lRebar);
                                        lBarID++;
                                        lBarSort++;
                                    }
                                    else
                                    {
                                        lSRDia = 13;
                                        if (lSRDiaAdd != 0)
                                        {
                                            lSRDia = (int)lSRDiaAdd;
                                        }
                                        lTotalLength = (int)Math.Round((double)(lCageDia - 2 * lSLDia - 2 * lTopCrankHT) * Math.PI + sr_link_lapping * lSRDia);
                                        lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "R88",
                                                       "SR", "SR2", lSRDia, lTotalLength, lNoOfSRa,
                                                       0, 0,
                                                       lCageDia - 2 * lSLDia - 2 * lTopCrankHT,
                                                       sr_link_lapping * lSRDia,
                                                       0, 0, 0);
                                        lRebarList.Add(lRebar);
                                        lBarID++;
                                        lBarSort++;
                                    }
                                }
                                else
                                {
                                    if (pBPCModels.sr1_location > pBPCModels.mainbar_location_2layer)
                                    {
                                        lSRDia = 13;
                                        if (lSRDiaAdd != 0)
                                        {
                                            lSRDia = (int)lSRDiaAdd;
                                        }
                                        //Commented lTopCrankHT for SR calculations to match SR1 and SR2 
                                        // lTotalLength = (int)Math.Round((double)(lCageDia - 2 * lSLDia - 2 * lTopCrankHT - 2 * Math.Min(lMainBarDia1, lMainBarDia2)) * Math.PI + 10 * lSRDia);
                                        lTotalLength = (int)Math.Round((double)(lCageDia - 2 * lSLDia - 2 * Math.Min(lMainBarDia1, lMainBarDia2)) * Math.PI + sr_link_lapping * lSRDia);

                                        lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "R88",
                                               "SR", "SR2", lSRDia, lTotalLength, lNoOfSRa,
                                               0, 0,
                                                //Commented lTopCrankHT for SR calculations to match SR1 and SR2
                                                //lCageDia - 2 * lSLDia - 2 * lTopCrankHT - 2 * Math.Min(lMainBarDia1, lMainBarDia2),
                                                lCageDia - 2 * lSLDia - 2 * Math.Min(lMainBarDia1, lMainBarDia2),
                                               10 * lSRDia,
                                               0, 0, 0);
                                        lRebarList.Add(lRebar);
                                        lBarID++;
                                        lBarSort++;
                                    }
                                    else
                                    {
                                        lSRDia = 13;
                                        if (lSRDiaAdd != 0)
                                        {
                                            lSRDia = (int)lSRDiaAdd;
                                        }
                                        //Commented lTopCrankHT for SR calculations to match SR1 and SR2
                                        // lTotalLength = (int)Math.Round((double)(lCageDia - 2 * lSLDia - 2 * lTopCrankHT) * Math.PI + 10 * lSRDia);
                                        lTotalLength = (int)Math.Round((double)(lCageDia - 2 * lSLDia) * Math.PI + sr_link_lapping * lSRDia);

                                        lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "R88",
                                                       "SR", "SR2", lSRDia, lTotalLength, lNoOfSRa,
                                                       0, 0,
                                                       //Commented lTopCrankHT for SR calculations to match SR1 and SR2
                                                       // lCageDia - 2 * lSLDia - 2 * lTopCrankHT,
                                                       lCageDia - 2 * lSLDia,

                                                       sr_link_lapping * lSRDia,
                                                       0, 0, 0);
                                        lRebarList.Add(lRebar);
                                        lBarID++;
                                        lBarSort++;
                                    }
                                }

                            }

                            if (lEndSR > 0)
                            {
                                var lNoOfSRb = 1;
                                lBarMark = "SR2";
                                if (lLapSR > 0)
                                {
                                    lBarMark = "SR3";
                                }
                                int lLastSRLoc = pBPCModels.sr5_location;
                                if (lLastSRLoc == 0) lLastSRLoc = pBPCModels.sr4_location;
                                if (lLastSRLoc == 0) lLastSRLoc = pBPCModels.sr3_location;
                                if (lLastSRLoc == 0) lLastSRLoc = pBPCModels.sr2_location;
                                if (lLastSRLoc == 0) lLastSRLoc = pBPCModels.sr1_location;
                                if (lLastSRLoc > pBPCModels.mainbar_location_2layer + pBPCModels.mainbar_length_2layer)
                                {
                                    if (lMainBarShape == "Crank-End" || lMainBarShape == "Crank-Both")
                                    {
                                        lSRDia = 13;
                                        if (lSRDiaAdd != 0)
                                        {
                                            lSRDia = (int)lSRDiaAdd;
                                        }
                                        lTotalLength = (int)Math.Round((double)(lCageDia - 2 * lSLDia - 2 * lTopCrankHT - 2 * Math.Min(lMainBarDia1, lMainBarDia2)) * Math.PI + sr_link_lapping * lSRDia);
                                        lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "R88",
                                               "SR", lBarMark, lSRDia, lTotalLength, lNoOfSRb,
                                               0, 0,
                                               lCageDia - 2 * lSLDia - 2 * lTopCrankHT - 2 * Math.Min(lMainBarDia1, lMainBarDia2),
                                               sr_link_lapping * lSRDia,
                                               0, 0, 0);
                                        lRebarList.Add(lRebar);
                                        lBarID++;
                                        lBarSort++;
                                    }
                                    else
                                    {
                                        lSRDia = 13;
                                        if (lSRDiaAdd != 0)
                                        {
                                            lSRDia = (int)lSRDiaAdd;
                                        }
                                        lTotalLength = (int)Math.Round((double)(lCageDia - 2 * lSLDia - 2 * (lMainBarDia1 + lMainBarDia2)) * Math.PI + sr_link_lapping * lSRDia);
                                        lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "R88",
                                               "SR", lBarMark, lSRDia, lTotalLength, lNoOfSRb,
                                               0, 0,
                                               lCageDia - 2 * lSLDia - 2 * (lMainBarDia1 + lMainBarDia2),
                                               sr_link_lapping * lSRDia,
                                               0, 0, 0);
                                        lRebarList.Add(lRebar);
                                        lBarID++;
                                        lBarSort++;
                                    }
                                }
                                else
                                {
                                    if (lMainBarShape == "Crank-End" || lMainBarShape == "Crank-Both")
                                    {
                                        lSRDia = 13;
                                        if (lSRDiaAdd != 0)
                                        {
                                            lSRDia = (int)lSRDiaAdd;
                                        }
                                        lTotalLength = (int)Math.Round((double)(lCageDia - 2 * lSLDia - 2 * lTopCrankHT) * Math.PI + sr_link_lapping * lSRDia);
                                        lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "R88",
                                                       "SR", lBarMark, lSRDia, lTotalLength, lNoOfSRb,
                                                       0, 0,
                                                       lCageDia - 2 * lSLDia - 2 * lTopCrankHT,
                                                       sr_link_lapping * lSRDia,
                                                       0, 0, 0);
                                        lRebarList.Add(lRebar);
                                        lBarID++;
                                        lBarSort++;
                                    }
                                    else
                                    {
                                        lSRDia = 13;
                                        if (lSRDiaAdd != 0)
                                        {
                                            lSRDia = (int)lSRDiaAdd;
                                        }
                                        lTotalLength = (int)Math.Round((double)(lCageDia - 2 * lSLDia - 2 * lMainBarDia1) * Math.PI + sr_link_lapping * lSRDia);
                                        lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "R88",
                                                       "SR", lBarMark, lSRDia, lTotalLength, lNoOfSRb,
                                                       0, 0,
                                                       lCageDia - 2 * lSLDia - 2 * lMainBarDia1,
                                                       sr_link_lapping * lSRDia,
                                                       0, 0, 0);
                                        lRebarList.Add(lRebar);
                                        lBarID++;
                                        lBarSort++;
                                    }

                                }
                            }
                        }
                        else
                        {
                            var lMainBarMax = lMainBarDia1;
                            if (lMainBarDia2 > lMainBarMax)
                            {
                                lMainBarMax = lMainBarDia2;
                            }
                            int lTotalLength = (int)Math.Round((double)(lCageDia - 2 * lSLDia - 4 * lMainBarMax) * Math.PI + sr_link_lapping * lSRDia);
                            string lBarMark = "SR";
                            if (lLapSR > 0 || lEndSR > 0 || ((lSupportBar == "Cross" || lSupportBar == "Square") && lSupportBarDia > 0))
                            {
                                lBarMark = "SR1";
                            }
                            lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "R88",
                                       "SR", lBarMark, lSRDia, lTotalLength, lNoOfSR,
                                       0, 0,
                                       lCageDia - 2 * lSLDia - 4 * lMainBarMax,
                                       sr_link_lapping * lSRDia,
                                       0, 0, 0);
                            lRebarList.Add(lRebar);
                            lBarID++;
                            lBarSort++;

                            //additional SR
                            if (lLapSR > 0)
                            {
                                var lNoOfSRc = 1;
                                if (lMainBarShape == "Crank-Top" || lMainBarShape == "Crank" || lMainBarShape == "Crank-Both")
                                {
                                    if (pBPCModels.sr1_location > pBPCModels.mainbar_location_2layer)
                                    {
                                        lSRDia = 13;
                                        if (lSRDiaAdd != 0)
                                        {
                                            lSRDia = (int)lSRDiaAdd;
                                        }
                                        lTotalLength = (int)Math.Round((double)(lCageDia - 2 * lSLDia - 2 * lTopCrankHT - 2 * lMainBarMax) * Math.PI + sr_link_lapping * lSRDia);
                                        lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "R88",
                                               "SR", "SR2", lSRDia, lTotalLength, lNoOfSRc,
                                               0, 0,
                                               lCageDia - 2 * lSLDia - 2 * lTopCrankHT - 2 * lMainBarMax,
                                               sr_link_lapping * lSRDia,
                                               0, 0, 0);
                                        lRebarList.Add(lRebar);
                                        lBarID++;
                                        lBarSort++;
                                    }
                                    else
                                    {
                                        lSRDia = 13;
                                        if (lSRDiaAdd != 0)
                                        {
                                            lSRDia = (int)lSRDiaAdd;
                                        }
                                        lTotalLength = (int)Math.Round((double)(lCageDia - 2 * lSLDia - 2 * lTopCrankHT) * Math.PI + sr_link_lapping * lSRDia);
                                        lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "R88",
                                                       "SR", "SR2", lSRDia, lTotalLength, lNoOfSRc,
                                                       0, 0,
                                                       lCageDia - 2 * lSLDia - 2 * lTopCrankHT,
                                                       sr_link_lapping * lSRDia,
                                                       0, 0, 0);
                                        lRebarList.Add(lRebar);
                                        lBarID++;
                                        lBarSort++;
                                    }
                                }
                                else
                                {
                                    if (pBPCModels.sr1_location > pBPCModels.mainbar_location_2layer)
                                    {
                                        lSRDia = 13;
                                        if (lSRDiaAdd != 0)
                                        {
                                            lSRDia = (int)lSRDiaAdd;
                                        }
                                        lTotalLength = (int)Math.Round((double)(lCageDia - 2 * lSLDia - 4 * lMainBarMax) * Math.PI + sr_link_lapping * lSRDia);
                                        lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "R88",
                                               "SR", "SR2", lSRDia, lTotalLength, lNoOfSRc,
                                               0, 0,
                                               lCageDia - 2 * lSLDia - 4 * lMainBarMax,
                                               sr_link_lapping * lSRDia,
                                               0, 0, 0);
                                        lRebarList.Add(lRebar);
                                        lBarID++;
                                        lBarSort++;
                                    }
                                    else
                                    {
                                        lSRDia = 13;
                                        if (lSRDiaAdd != 0)
                                        {
                                            lSRDia = (int)lSRDiaAdd;
                                        }
                                        lTotalLength = (int)Math.Round((double)(lCageDia - 2 * lSLDia - 2 * lMainBarMax) * Math.PI + sr_link_lapping * lSRDia);
                                        lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "R88",
                                                       "SR", "SR2", lSRDia, lTotalLength, lNoOfSRc,
                                                       0, 0,
                                                       lCageDia - 2 * lSLDia - 2 * lMainBarMax,
                                                       sr_link_lapping * lSRDia,
                                                       0, 0, 0);
                                        lRebarList.Add(lRebar);
                                        lBarID++;
                                        lBarSort++;
                                    }
                                }

                            }

                            if (lEndSR > 0)
                            {
                                var lNoOfSRd = 1;
                                lBarMark = "SR2";
                                if (lLapSR > 0)
                                {
                                    lBarMark = "SR3";
                                }
                                int lLastSRLoc = pBPCModels.sr5_location;
                                if (lLastSRLoc == 0) lLastSRLoc = pBPCModels.sr4_location;
                                if (lLastSRLoc == 0) lLastSRLoc = pBPCModels.sr3_location;
                                if (lLastSRLoc == 0) lLastSRLoc = pBPCModels.sr2_location;
                                if (lLastSRLoc == 0) lLastSRLoc = pBPCModels.sr1_location;
                                if (lLastSRLoc > pBPCModels.mainbar_location_2layer + pBPCModels.mainbar_length_2layer)
                                {
                                    if (lMainBarShape == "Crank-End" || lMainBarShape == "Crank-Both")
                                    {
                                        lSRDia = 13;
                                        if (lSRDiaAdd != 0)
                                        {
                                            lSRDia = (int)lSRDiaAdd;
                                        }
                                        lTotalLength = (int)Math.Round((double)(lCageDia - 2 * lSLDia - 4 * lMainBarMax - 2 * 10 * lMainBarMax) * Math.PI + sr_link_lapping * lSRDia);
                                        lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "R88",
                                               "SR", lBarMark, lSRDia, lTotalLength, lNoOfSRd,
                                               0, 0,
                                               lCageDia - 2 * lSLDia - 4 * lMainBarMax - 2 * 10 * lMainBarMax,
                                               sr_link_lapping * lSRDia,
                                               0, 0, 0);
                                        lRebarList.Add(lRebar);
                                        lBarID++;
                                        lBarSort++;
                                    }
                                    else
                                    {
                                        lSRDia = 13;
                                        if (lSRDiaAdd != 0)
                                        {
                                            lSRDia = (int)lSRDiaAdd;
                                        }
                                        lTotalLength = (int)Math.Round((double)(lCageDia - 2 * lSLDia - 4 * lMainBarMax) * Math.PI + sr_link_lapping * lSRDia);
                                        lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "R88",
                                               "SR", lBarMark, lSRDia, lTotalLength, lNoOfSRd,
                                               0, 0,
                                               lCageDia - 2 * lSLDia - 4 * lMainBarMax,
                                               sr_link_lapping * lSRDia,
                                               0, 0, 0);
                                        lRebarList.Add(lRebar);
                                        lBarID++;
                                        lBarSort++;
                                    }
                                }
                                else
                                {
                                    if (lMainBarShape == "Crank-End" || lMainBarShape == "Crank-Both")
                                    {
                                        lSRDia = 13;
                                        if (lSRDiaAdd != 0)
                                        {
                                            lSRDia = (int)lSRDiaAdd;
                                        }
                                        lTotalLength = (int)Math.Round((double)(lCageDia - 2 * lSLDia - 2 * lMainBarMax - 2 * 10 * lMainBarMax) * Math.PI + sr_link_lapping * lSRDia);
                                        lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "R88",
                                                       "SR", lBarMark, lSRDia, lTotalLength, lNoOfSRd,
                                                       0, 0,
                                                       lCageDia - 2 * lSLDia - 2 * lMainBarMax - 2 * 10 * lMainBarMax,
                                                       sr_link_lapping * lSRDia,
                                                       0, 0, 0);
                                        lRebarList.Add(lRebar);
                                        lBarID++;
                                        lBarSort++;
                                    }
                                    else
                                    {
                                        lSRDia = 13;
                                        if (lSRDiaAdd != 0)
                                        {
                                            lSRDia = (int)lSRDiaAdd;
                                        }
                                        lTotalLength = (int)Math.Round((double)(lCageDia - 2 * lSLDia - 2 * lMainBarMax) * Math.PI + sr_link_lapping * lSRDia);
                                        lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "R88",
                                                       "SR", lBarMark, lSRDia, lTotalLength, lNoOfSRd,
                                                       0, 0,
                                                       lCageDia - 2 * lSLDia - 2 * lMainBarMax,
                                                       sr_link_lapping * lSRDia,
                                                       0, 0, 0);
                                        lRebarList.Add(lRebar);
                                        lBarID++;
                                        lBarSort++;
                                    }

                                }
                            }

                        }
                    }
                    else
                    {
                        var lMainBarMax = lMainBarDia1;
                        if (lMainBarDia2 > lMainBarMax)
                        {
                            lMainBarMax = lMainBarDia2;
                        }
                        int lTotalLength = (int)Math.Round((double)(lCageDia - 2 * lSLDia - 2 * lMainBarMax) * Math.PI + sr_link_lapping * lSRDia);
                        string lBarMark = "SR";
                        if (lLapSR > 0 || lEndSR > 0 || ((lSupportBar == "Cross" || lSupportBar == "Square") && lSupportBarDia > 0))
                        {
                            lBarMark = "SR1";
                        }
                        lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "R88",
                                   "SR", lBarMark, lSRDia, lTotalLength, lNoOfSR,
                                   0, 0,
                                   lCageDia - 2 * lSLDia - 2 * lMainBarMax,
                                   sr_link_lapping * lSRDia,
                                   0, 0, 0);
                        lRebarList.Add(lRebar);
                        lBarID++;
                        lBarSort++;

                        //additional SR
                        if (lLapSR > 0)
                        {
                            var lNoOfSRa = 1;
                            if (lMainBarShape == "Crank-Top" || lMainBarShape == "Crank" || lMainBarShape == "Crank-Both")
                            {
                                lSRDia = 13;
                                if (lSRDiaAdd != 0)
                                {
                                    lSRDia = (int)lSRDiaAdd;
                                }
                                lTotalLength = (int)Math.Round((double)(lCageDia - 2 * lSLDia - 2 * lTopCrankHT) * Math.PI + sr_link_lapping * lSRDia);
                                lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "R88",
                                       "SR", "SR2", lSRDia, lTotalLength, lNoOfSRa,
                                       0, 0,
                                       lCageDia - 2 * lSLDia - 2 * lTopCrankHT,
                                       sr_link_lapping * lSRDia,
                                       0, 0, 0);
                                lRebarList.Add(lRebar);
                                lBarID++;
                                lBarSort++;
                            }
                            else
                            {
                                lSRDia = 13;
                                if (lSRDiaAdd != 0)
                                {
                                    lSRDia = (int)lSRDiaAdd;
                                }
                                lTotalLength = (int)Math.Round((double)(lCageDia - 2 * lSLDia - 2 * lMainBarMax) * Math.PI + sr_link_lapping * lSRDia);
                                lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "R88",
                                       "SR", "SR2", lSRDia, lTotalLength, lNoOfSRa,
                                       0, 0,
                                       lCageDia - 2 * lSLDia - 2 * lMainBarMax,
                                       sr_link_lapping * lSRDia,
                                       0, 0, 0);
                                lRebarList.Add(lRebar);
                                lBarID++;
                                lBarSort++;
                            }

                        }

                        if (lEndSR > 0)
                        {
                            var lNoOfSRb = 1;
                            lBarMark = "SR2";
                            if (lLapSR > 0)
                            {
                                lBarMark = "SR3";
                            }
                            if (lMainBarShape == "Crank-End" || lMainBarShape == "Crank-Both")
                            {
                                lSRDia = 13;
                                if (lSRDiaAdd != 0)
                                {
                                    lSRDia = (int)lSRDiaAdd;
                                }
                                lTotalLength = (int)Math.Round((double)(lCageDia - 2 * lSLDia - 2 * lTopCrankHT) * Math.PI + sr_link_lapping * lSRDia);
                                //lTotalLength = (int)Math.Round((double)(lCageDia - 2 * lSLDia - 2 * lMainBarMax - 2 * 10 * lMainBarMax) * Math.PI + 10 * lSRDia);
                                lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "R88",
                                       "SR", lBarMark, lSRDia, lTotalLength, lNoOfSRb,
                                       0, 0,
                                       lCageDia - 2 * lSLDia - 2 * lTopCrankHT,
                                       sr_link_lapping * lSRDia,
                                       0, 0, 0);
                                lRebarList.Add(lRebar);
                                lBarID++;
                                lBarSort++;
                            }
                            else
                            {
                                lSRDia = 13;
                                if (lSRDiaAdd != 0)
                                {
                                    lSRDia = (int)lSRDiaAdd;
                                }
                                lTotalLength = (int)Math.Round((double)(lCageDia - 2 * lSLDia - 2 * lMainBarMax) * Math.PI + sr_link_lapping * lSRDia);
                                lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "R88",
                                       "SR", lBarMark, lSRDia, lTotalLength, lNoOfSRb,
                                       0, 0,
                                       lCageDia - 2 * lSLDia - 2 * lMainBarMax,
                                       sr_link_lapping * lSRDia,
                                       0, 0, 0);
                                lRebarList.Add(lRebar);
                                lBarID++;
                                lBarSort++;
                            }
                        }

                    }

                }
            }
            else if (lPileType == "Double-Layer")
            {
                var lMainbarDiaArr = lMainBarDia.Split(',');
                var lMainbarCTArr = lMainBarCT.Split(',');
                if (lMainbarDiaArr.Length > 1 && lMainbarCTArr.Length > 1)
                {
                    var lMainBarDia1 = int.Parse(lMainbarDiaArr[0]);
                    var lMainBarDia2 = int.Parse(lMainbarDiaArr[1]);

                    //First SR 
                    int lTotalLength = (int)Math.Round((double)(lCageDia - 2 * lSLDia - 2 * lMainBarDia1) * Math.PI + sr_link_lapping * lSRDia);
                    lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "R88",
                               "SR", "SR1", lSRDia, lTotalLength, lNoOfSR,
                               0, 0,
                               lCageDia - 2 * lSLDia - 2 * lMainBarDia1,
                               sr_link_lapping * lSRDia,
                               0, 0, 0);
                    lRebarList.Add(lRebar);
                    lBarID++;
                    lBarSort++;

                    //Second Layer SR
                    lTotalLength = (int)Math.Round((double)(lCageDia - 2 * lSLDia - 2 * lMainBarDia1 - 2 * lSRDia - 2 * lMainBarDia2) * Math.PI + sr_link_lapping * lSRDia);
                    lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "R88",
                               "SR", "SR2", lSRDia, lTotalLength, lNoOfSR,
                               0, 0,
                               lCageDia - 2 * lSLDia - 2 * lMainBarDia1 - 2 * lSRDia - 2 * lMainBarDia2,
                               sr_link_lapping * lSRDia,
                               0, 0, 0);
                    lRebarList.Add(lRebar);
                    lBarID++;
                    lBarSort++;
                }
            }

            // Circular Rings
            var lCRTop = pBPCModels.no_of_cr_top;
            var lCREnd = pBPCModels.no_of_cr_end;
            int lExtraCRNo = pBPCModels.extra_cr_no == null ? 0 : (int)pBPCModels.extra_cr_no;
            int lExtraCRLoc = pBPCModels.extra_cr_loc == null ? 0 : (int)pBPCModels.extra_cr_loc;
            int lExtraCRDia = pBPCModels.extra_cr_dia == null ? 0 : (int)pBPCModels.extra_cr_dia;

            if (lSLType.IndexOf("Twin") >= 0)
            {
                if (lSLType == "Single-Twin")
                {
                    lCREnd = lCREnd * 2;
                }
                else if (lSLType == "Twin-Single")
                {
                    lCRTop = lCRTop * 2;
                }
                else
                {
                    lCRTop = lCRTop * 2;
                    lCREnd = lCREnd * 2;
                }
            }

            var lCRNo = 0;
            if (lCRTop + lCREnd > 0)
            {
                if (lCRTop > 0 && lCREnd > 0)
                {
                    if (lSL1Dia > 0 && lSL3Dia > 0 && lSL1Dia != lSL3Dia)
                    {
                        int lTotalLength = (int)Math.Round((double)(lCageDia * Math.PI + cr_link_lapping * lSL1Dia));
                        lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "R88",
                                   "CR", "CR1", lSL1Dia, lTotalLength, lCRTop,
                                   0, 0,
                                   lCageDia,
                                   cr_link_lapping * lSL1Dia,
                                   0, 0, 0);
                        lRebarList.Add(lRebar);
                        lBarID++;
                        lBarSort++;
                        lCRNo++;

                        lTotalLength = (int)Math.Round((double)(lCageDia * Math.PI + cr_link_lapping * lSL3Dia));
                        lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "R88",
                                   "CR", "CR2", lSL3Dia, lTotalLength, lCREnd,
                                   0, 0,
                                   lCageDia,
                                   cr_link_lapping * lSL3Dia,
                                   0, 0, 0);
                        lRebarList.Add(lRebar);
                        lBarID++;
                        lBarSort++;
                        lCRNo++;
                    }
                    else if (lSL1Dia > 0 && lSL2Dia > 0 && lSL3Dia == 0 && lSL1Dia != lSL2Dia)
                    {
                        int lTotalLength = (int)Math.Round((double)(lCageDia * Math.PI + cr_link_lapping * lSL1Dia));
                        lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "R88",
                                   "CR", "CR1", lSL1Dia, lTotalLength, lCRTop,
                                   0, 0,
                                   lCageDia,
                                   cr_link_lapping * lSL1Dia,
                                   0, 0, 0);
                        lRebarList.Add(lRebar);
                        lBarID++;
                        lBarSort++;
                        lCRNo++;

                        lTotalLength = (int)Math.Round((double)(lCageDia * Math.PI + cr_link_lapping * lSL2Dia));
                        lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "R88",
                                   "CR", "CR2", lSL2Dia, lTotalLength, lCREnd,
                                   0, 0,
                                   lCageDia,
                                   cr_link_lapping * lSL2Dia,
                                   0, 0, 0);
                        lRebarList.Add(lRebar);
                        lBarID++;
                        lBarSort++;
                        lCRNo++;
                    }
                    else
                    {
                        var lCRMark = "CR";
                        if (lExtraCRNo > 0 && lExtraCRDia > 0)
                        {
                            lCRMark = "CR1";
                        }
                        var lTotalLength = (int)Math.Round((double)(lCageDia * Math.PI + cr_link_lapping * lSL1Dia));
                        lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "R88",
                                   "CR", lCRMark, lSL1Dia, lTotalLength, lCRTop + lCREnd,
                                   0, 0,
                                   lCageDia,
                                   cr_link_lapping * lSL1Dia,
                                   0, 0, 0);
                        lRebarList.Add(lRebar);
                        lBarID++;
                        lBarSort++;
                        lCRNo++;
                    }
                }
                else
                {
                    var lCRMark = "CR";
                    if (lExtraCRNo > 0 && lExtraCRDia > 0)
                    {
                        lCRMark = "CR1";
                    }
                    var lTotalLength = (int)Math.Round((double)(lCageDia * Math.PI + cr_link_lapping * lSL1Dia));
                    lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "R88",
                               "CR", lCRMark, lSL1Dia, lTotalLength, lCRTop + lCREnd,
                               0, 0,
                               lCageDia,
                               cr_link_lapping * lSL1Dia,
                               0, 0, 0);
                    lRebarList.Add(lRebar);
                    lBarID++;
                    lBarSort++;
                    lCRNo++;
                }
            }

            //Extra Circular Rings
            if (lExtraCRNo > 0 && lExtraCRDia > 0)
            {
                lCRNo++;
                var lCRMark = "CR" + lCRNo.ToString();
                if (lCRNo == 1)
                {
                    lCRMark = "CR";
                }
                var lCrankHA = 0;
                if ((lMainBarShape == "Crank-Top" || lMainBarShape == "Crank" || lMainBarShape == "Crank-Both") &&
                    lExtraCRLoc <= lLapLength)
                {
                    lCrankHA = lTopCrankHT;
                }
                if (lMainBarShape == "Crank-Both" && lExtraCRLoc >= int.Parse(lCageLength) - lEndLength)
                {
                    lCrankHA = lTopCrankHT;
                }

                var lTotalLength = (int)Math.Round((double)((lCageDia - 2 * lCrankHA) * Math.PI + cr_link_lapping * lSL1Dia));
                lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "R88",
                           "CR", lCRMark, lExtraCRDia, lTotalLength, lExtraCRNo,
                           0, 0,
                           lCageDia - 2 * lCrankHA,
                           cr_link_lapping * lSL1Dia,
                           0, 0, 0);
                lRebarList.Add(lRebar);
                lBarID++;
                lBarSort++;
            }

            #region Extra Support Bars 
            var lSBSNo = 2;
            if (lLapSR > 0)
            {
                lSBSNo = lSBSNo + 1;
            }
            if (lEndSR > 0)
            {
                lSBSNo = lSBSNo + 1;
            }

            if ((lSupportBar == "Cross" || lSupportBar == "Square") && lSupportBarDia > 0)
            {
                int lSTBarQty = 4;
                if (lNoOfSR == 1)
                {
                    lSTBarQty = 2;
                }
                if (lSupportBar == "Square")
                {
                    lSTBarQty = 8;
                    if (lNoOfSR == 1)
                    {
                        lSTBarQty = 4;
                    }

                }

                int lBarLength = lCageDia;
                if (lSupportBar == "Square")
                {
                    lBarLength = (int)Math.Round((double)lCageDia * 1.414 / 2);
                }

                string lBarMark = "SR" + lSBSNo;
                lSBSNo = lSBSNo + 1;
                lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "020",
                           "SR", lBarMark, lSupportBarDia, lBarLength,
                           lSTBarQty,
                           lBarLength,
                           0, 0, 0, 0, 0, 0);
                lRebarList.Add(lRebar);
                lBarID++;
                lBarSort++;

                #region All Extra Support Bar are Same length = Cage Dia
                /*
                if (lPileType == "Single-Layer" && lMainBarType == "Single")
                {
                    if (pBPCModels.main_bar_arrange == "In-Out")
                    {
                        int lBarLength = lCageDia - 2 * lSLDia - 4 * int.Parse(lMainBarDia);
                        if (lSupportBar == "Square")
                        {
                        }

                        string lBarMark = "SR" + lSBSNo;
                        lSBSNo = lSBSNo + 1;
                        lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "020",
                                   "SR", lBarMark, lSupportBarDia, lCageDia - 2 * lSLDia - 4 * int.Parse(lMainBarDia),
                                   lSTBarQty,
                                   lCageDia - 2 * lSLDia - 4 * int.Parse(lMainBarDia),
                                   0, 0, 0, 0, 0, 0);
                        lRebarList.Add(lRebar);
                        lBarID++;
                        lBarSort++;
                    }
                    else
                    {
                        string lBarMark = "SR" + lSBSNo;
                        lSBSNo = lSBSNo + 1;
                        lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "020",
                                   "SR", lBarMark, lSupportBarDia, lCageDia - 2 * lSLDia - 2 * int.Parse(lMainBarDia),
                                   lSTBarQty,
                                   lCageDia - 2 * lSLDia - 2 * int.Parse(lMainBarDia),
                                   0, 0, 0, 0, 0, 0);
                        lRebarList.Add(lRebar);
                        lBarID++;
                        lBarSort++;
                    }
                }
                else if (lPileType == "Single-Layer" && lMainBarType == "Mixed")
                {
                    var lMainbarDiaArr = lMainBarDia.Split(',');
                    var lMainbarCTArr = lMainBarCT.Split(',');

                    if (lMainbarDiaArr.Length > 1 && lMainbarCTArr.Length > 1)
                    {
                        var lMainBarDia1 = int.Parse(lMainbarDiaArr[0]);
                        var lMainBarDia2 = int.Parse(lMainbarDiaArr[1]);

                        if (pBPCModels.main_bar_arrange == "In-Out")
                        {
                            if (pBPCModels.bundle_same_type != "Y")
                            {
                                string lBarMark = "SR" + lSBSNo;
                                lSBSNo = lSBSNo + 1;
                                lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "020",
                                           "SR", lBarMark, lSupportBarDia, lCageDia - 2 * lSLDia - 2 * (lMainBarDia1 + lMainBarDia2),
                                           lSTBarQty,
                                           lCageDia - 2 * lSLDia - 2 * (lMainBarDia1 + lMainBarDia2),
                                           0, 0, 0, 0, 0, 0);
                                lRebarList.Add(lRebar);
                                lBarID++;
                                lBarSort++;
                            }
                            else
                            {
                                var lMainBarMax = lMainBarDia1;
                                if (lMainBarDia2 > lMainBarMax)
                                {
                                    lMainBarMax = lMainBarDia2;
                                }

                                string lBarMark = "SR" + lSBSNo;
                                lSBSNo = lSBSNo + 1; 

                                lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "020",
                                           "SR", lBarMark, lSupportBarDia, lCageDia - 2 * lSLDia - 4 * lMainBarMax,
                                           lSTBarQty,
                                           lCageDia - 2 * lSLDia - 4 * lMainBarMax,
                                           0, 0, 0, 0, 0, 0);
                                lRebarList.Add(lRebar);
                                lBarID++;
                                lBarSort++;
                            }
                        }
                        else
                        {
                            var lMainBarMax = lMainBarDia1;
                            if (lMainBarDia2 > lMainBarMax)
                            {
                                lMainBarMax = lMainBarDia2;
                            }

                            string lBarMark = "SR" + lSBSNo;
                            lSBSNo = lSBSNo + 1;
                            lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "020",
                                       "SR", lBarMark, lSupportBarDia, lCageDia - 2 * lSLDia - 2 * lMainBarMax,
                                       lSTBarQty,
                                       lCageDia - 2 * lSLDia - 2 * lMainBarMax,
                                       0, 0, 0, 0, 0, 0);
                            lRebarList.Add(lRebar);
                            lBarID++;
                            lBarSort++;
                        }

                    }
                }
                else if (lPileType == "Double-Layer")
                {
                    var lMainbarDiaArr = lMainBarDia.Split(',');
                    var lMainbarCTArr = lMainBarCT.Split(',');
                    if (lMainbarDiaArr.Length > 1 && lMainbarCTArr.Length > 1)
                    {
                        var lMainBarDia1 = int.Parse(lMainbarDiaArr[0]);
                        var lMainBarDia2 = int.Parse(lMainbarDiaArr[1]);

                        string lBarMark = "SR" + lSBSNo;
                        lSBSNo = lSBSNo + 1;
                        lRebar = SaveMainBar(pBPCModels, lBarID, lBarSort, "020",
                                   "SR", lBarMark, lSupportBarDia, lCageDia - 2 * lSLDia - 2 * lMainBarDia1 - 2 * lSRDia - 2 * lMainBarDia2,
                                   lSTBarQty,
                                   lCageDia - 2 * lSLDia - 2 * lMainBarDia1 - 2 * lSRDia - 2 * lMainBarDia2,
                                   0, 0, 0, 0, 0, 0);
                        lRebarList.Add(lRebar);
                        lBarID++;
                        lBarSort++;

                    }
                }
                */
                #endregion
            }

            #endregion

            return lRebarList;

        }

        [HttpGet]
        [Route("/SaveMainBar_bpc/{pBarID}/{pSortID}/{pShapeCode}/{pElemMark}/{pBarMark}/{pBarDia}/{pBarLength}/{pBarQty}/{pA}/{pB}/{pC}/{pD}/{pE}/{pF}/{pG}")]
        BPCCageBarsModels SaveMainBar(BPCDetailsModels pBPCModels, int pBarID, int pSortID, string pShapeCode,
            string pElemMark, string pBarMark, int pBarDia, int pBarLength, int pBarQty,
            int pA, int pB, int pC, int pD, int pE, int pF, int pG)
        {
            //pA = (int)Math.Round((double)pA / 5) * 5;
            //pB = (int)Math.Round((double)pB / 5) * 5;
            //pC = (int)Math.Round((double)pC / 5) * 5;
            //pD = (int)Math.Round((double)pD / 5) * 5;
            //pE = (int)Math.Round((double)pE / 5) * 5;
            //pF = (int)Math.Round((double)pF / 5) * 5;
            //pG = (int)Math.Round((double)pG / 5) * 5;
            pBPCModels.sr_link_lapping = pBPCModels.sr_link_lapping ?? 10;
            pBPCModels.cr_link_lapping = pBPCModels.cr_link_lapping ?? 51;
            if ((pBPCModels.main_bar_arrange == "Side-By-Side" || pBPCModels.main_bar_arrange == "In-Out") && pBPCModels.pile_type == "Double-Layer")
            {
                if (pElemMark == "MB")
                {
                    mbB = pB;
                    mbC = pC;
                    mbshapeCode = pShapeCode;
                }

                if (pElemMark == "MM" && mbshapeCode == "041" && pShapeCode == "041")
                {

                    if (pB < mbB)
                    {
                        pB = mbB;
                        pC = mbC;
                    }

                }
            }


            var lMainBars = new BPCCageBarsModels();
            lMainBars.CustomerCode = pBPCModels.CustomerCode;
            lMainBars.ProjectCode = pBPCModels.ProjectCode;
            lMainBars.Template = pBPCModels.Template;
            lMainBars.JobID = pBPCModels.JobID;
            lMainBars.CageID = pBPCModels.cage_id;
            lMainBars.BarID = pBarID;
            lMainBars.BarSort = pSortID;

            lMainBars.ElementMark = pElemMark;
            lMainBars.BarMark = pBarMark;
            if (pElemMark == "MB" || pElemMark == "MM")
            {
                lMainBars.BarType = pBPCModels.main_bar_grade;
            }
            else
            {
                lMainBars.BarType = pBPCModels.spiral_link_grade;
            }
            lMainBars.BarSize = (short)pBarDia;
            lMainBars.BarMemberQty = 1;
            lMainBars.BarEachQty = pBarQty;
            lMainBars.BarTotalQty = pBarQty;
            lMainBars.BarLength = pBarLength;
            lMainBars.PinSize = 0;
            lMainBars.Remarks = "";
            lMainBars.UpdateDate = DateTime.Now;
            lMainBars.BarShapeCode = pShapeCode;
            lMainBars.A = pA;
            lMainBars.B = pB;
            lMainBars.C = pC;
            lMainBars.D = pD;
            lMainBars.E = pE;
            lMainBars.F = pF;
            lMainBars.G = pG;

            lMainBars.BarWeight = (decimal)(pBarQty * getRebarWeight(pBarDia, pBarLength));

            if (pShapeCode == "020" && pElemMark == "MB")
            {
                if (pBPCModels.coupler_top == "No-Coupler" && pBPCModels.coupler_end == "No-Coupler")
                {
                    lMainBars.BarShapeCode = "020";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "No-Coupler" && pBPCModels.coupler_end == "Esplice-Standard-Coupler")
                {
                    lMainBars.BarShapeCode = "C20";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "No-Coupler" && pBPCModels.coupler_end == "Esplice-Standard-Stud")
                {
                    lMainBars.BarShapeCode = "S20";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "No-Coupler" && pBPCModels.coupler_end == "Esplice-Extended-Coupler")
                {
                    lMainBars.BarShapeCode = "P20";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "No-Coupler" && pBPCModels.coupler_end == "Esplice-Extended-Stud")
                {
                    lMainBars.BarShapeCode = "N20";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "No-Coupler" && pBPCModels.coupler_end == "Nsplice-Standard-Coupler")
                {
                    lMainBars.BarShapeCode = "J20";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "No-Coupler" && pBPCModels.coupler_end == "Nsplice-Standard-Stud")
                {
                    lMainBars.BarShapeCode = "K20";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "No-Coupler" && pBPCModels.coupler_end == "Nsplice-Extended-Coupler")
                {
                    lMainBars.BarShapeCode = "H20";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "No-Coupler" && pBPCModels.coupler_end == "Nsplice-Extended-Stud")
                {
                    lMainBars.BarShapeCode = "I20";
                    lMainBars.A = pA;
                }
                //Set 2
                if (pBPCModels.coupler_top == "Esplice-Standard-Coupler" && pBPCModels.coupler_end == "No-Coupler")
                {
                    lMainBars.BarShapeCode = "C20";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "Esplice-Standard-Coupler" && pBPCModels.coupler_end == "Esplice-Standard-Coupler")
                {
                    lMainBars.BarShapeCode = "C1C";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "Esplice-Standard-Coupler" && pBPCModels.coupler_end == "Esplice-Standard-Stud")
                {
                    lMainBars.BarShapeCode = "S1C";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "Esplice-Standard-Coupler" && pBPCModels.coupler_end == "Esplice-Extended-Coupler")
                {
                    lMainBars.BarShapeCode = "C1P";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "Esplice-Standard-Coupler" && pBPCModels.coupler_end == "Esplice-Extended-Stud")
                {
                    // No shape created for C1N
                    lMainBars.BarShapeCode = "020";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "Esplice-Standard-Coupler" && pBPCModels.coupler_end == "Nsplice-Standard-Coupler")
                {
                    //No Link with Esplice and Nsplice
                    lMainBars.BarShapeCode = "020";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "Esplice-Standard-Coupler" && pBPCModels.coupler_end == "Nsplice-Standard-Stud")
                {
                    //No Link with Esplice and Nsplice
                    lMainBars.BarShapeCode = "020";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "Esplice-Standard-Coupler" && pBPCModels.coupler_end == "Nsplice-Extended-Coupler")
                {
                    //No Link with Esplice and Nsplice
                    lMainBars.BarShapeCode = "020";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "Esplice-Standard-Coupler" && pBPCModels.coupler_end == "Nsplice-Extended-Stud")
                {
                    //No Link with Esplice and Nsplice
                    lMainBars.BarShapeCode = "020";
                    lMainBars.A = pA;
                }
                //Set 3
                if (pBPCModels.coupler_top == "Esplice-Standard-Stud" && pBPCModels.coupler_end == "No-Coupler")
                {
                    lMainBars.BarShapeCode = "S20";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "Esplice-Standard-Stud" && pBPCModels.coupler_end == "Esplice-Standard-Coupler")
                {
                    lMainBars.BarShapeCode = "S1C";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "Esplice-Standard-Stud" && pBPCModels.coupler_end == "Esplice-Standard-Stud")
                {
                    lMainBars.BarShapeCode = "S1S";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "Esplice-Standard-Stud" && pBPCModels.coupler_end == "Esplice-Extended-Coupler")
                {
                    lMainBars.BarShapeCode = "S1P";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "Esplice-Standard-Stud" && pBPCModels.coupler_end == "Esplice-Extended-Stud")
                {
                    lMainBars.BarShapeCode = "N1S";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "Esplice-Standard-Stud" && pBPCModels.coupler_end == "Nsplice-Standard-Coupler")
                {
                    lMainBars.BarShapeCode = "S1C";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "Esplice-Standard-Stud" && pBPCModels.coupler_end == "Nsplice-Standard-Stud")
                {
                    lMainBars.BarShapeCode = "S1S";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "Esplice-Standard-Stud" && pBPCModels.coupler_end == "Nsplice-Extended-Coupler")
                {
                    lMainBars.BarShapeCode = "S1P";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "Esplice-Standard-Stud" && pBPCModels.coupler_end == "Nsplice-Extended-Stud")
                {
                    lMainBars.BarShapeCode = "N1S";
                    lMainBars.A = pA;
                }
                //Set 4
                if (pBPCModels.coupler_top == "Esplice-Extended-Coupler" && pBPCModels.coupler_end == "No-Coupler")
                {
                    lMainBars.BarShapeCode = "P20";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "Esplice-Extended-Coupler" && pBPCModels.coupler_end == "Esplice-Standard-Coupler")
                {
                    lMainBars.BarShapeCode = "C1P";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "Esplice-Extended-Coupler" && pBPCModels.coupler_end == "Esplice-Standard-Stud")
                {
                    lMainBars.BarShapeCode = "S1P";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "Esplice-Extended-Coupler" && pBPCModels.coupler_end == "Esplice-Extended-Coupler")
                {
                    lMainBars.BarShapeCode = "P1P";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "Esplice-Extended-Coupler" && pBPCModels.coupler_end == "Esplice-Extended-Stud")
                {
                    lMainBars.BarShapeCode = "N1P";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "Esplice-Extended-Coupler" && pBPCModels.coupler_end == "Nsplice-Standard-Coupler")
                {
                    //No couple code available
                    lMainBars.BarShapeCode = "020";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "Esplice-Extended-Coupler" && pBPCModels.coupler_end == "Nsplice-Standard-Stud")
                {
                    //No couple code available
                    lMainBars.BarShapeCode = "020";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "Esplice-Extended-Coupler" && pBPCModels.coupler_end == "Nsplice-Extended-Coupler")
                {
                    //No couple code available
                    lMainBars.BarShapeCode = "020";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "Esplice-Extended-Coupler" && pBPCModels.coupler_end == "Nsplice-Extended-Stud")
                {
                    //No couple code available
                    lMainBars.BarShapeCode = "020";
                    lMainBars.A = pA;
                }
                //Set 5
                if (pBPCModels.coupler_top == "Esplice-Extended-Stud" && pBPCModels.coupler_end == "No-Coupler")
                {
                    lMainBars.BarShapeCode = "N20";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "Esplice-Extended-Stud" && pBPCModels.coupler_end == "Esplice-Standard-Coupler")
                {
                    //No couple code available FOR C1N
                    lMainBars.BarShapeCode = "020";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "Esplice-Extended-Stud" && pBPCModels.coupler_end == "Esplice-Standard-Stud")
                {
                    lMainBars.BarShapeCode = "N1S";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "Esplice-Extended-Stud" && pBPCModels.coupler_end == "Esplice-Extended-Coupler")
                {
                    lMainBars.BarShapeCode = "N1P";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "Esplice-Extended-Stud" && pBPCModels.coupler_end == "Esplice-Extended-Stud")
                {
                    //nO SHAPE N1N
                    lMainBars.BarShapeCode = "020";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "Esplice-Extended-Stud" && pBPCModels.coupler_end == "Nsplice-Standard-Coupler")
                {
                    // NO SHAPE FOR CONNECT ESPLICE AND NSPLICE
                    lMainBars.BarShapeCode = "020";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "Esplice-Extended-Stud" && pBPCModels.coupler_end == "Nsplice-Standard-Stud")
                {
                    // NO SHAPE FOR CONNECT ESPLICE AND NSPLICE
                    lMainBars.BarShapeCode = "020";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "Esplice-Extended-Stud" && pBPCModels.coupler_end == "Nsplice-Extended-Coupler")
                {
                    // NO SHAPE FOR CONNECT ESPLICE AND NSPLICE
                    lMainBars.BarShapeCode = "020";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "Esplice-Extended-Stud" && pBPCModels.coupler_end == "Nsplice-Extended-Stud")
                {
                    // NO SHAPE FOR CONNECT ESPLICE AND NSPLICE
                    lMainBars.BarShapeCode = "020";
                    lMainBars.A = pA;
                }
                //Set 6
                if (pBPCModels.coupler_top == "Nsplice-Standard-Coupler" && pBPCModels.coupler_end == "No-Coupler")
                {
                    lMainBars.BarShapeCode = "J20";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "Nsplice-Standard-Coupler" && pBPCModels.coupler_end == "Esplice-Standard-Coupler")
                {
                    //No shape code available for Esplice and Nsplice
                    lMainBars.BarShapeCode = "020";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "Nsplice-Standard-Coupler" && pBPCModels.coupler_end == "Esplice-Standard-Stud")
                {
                    //No shape code available for Esplice and Nsplice
                    lMainBars.BarShapeCode = "020";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "Nsplice-Standard-Coupler" && pBPCModels.coupler_end == "Esplice-Extended-Coupler")
                {
                    //No shape code available for Esplice and Nsplice
                    lMainBars.BarShapeCode = "020";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "Nsplice-Standard-Coupler" && pBPCModels.coupler_end == "Esplice-Extended-Stud")
                {
                    //No shape code available for Esplice and Nsplice
                    lMainBars.BarShapeCode = "020";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "Nsplice-Standard-Coupler" && pBPCModels.coupler_end == "Nsplice-Standard-Coupler")
                {
                    lMainBars.BarShapeCode = "J1J";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "Nsplice-Standard-Coupler" && pBPCModels.coupler_end == "Nsplice-Standard-Stud")
                {
                    lMainBars.BarShapeCode = "J1K";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "Nsplice-Standard-Coupler" && pBPCModels.coupler_end == "Nsplice-Extended-Coupler")
                {
                    lMainBars.BarShapeCode = "H1J";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "Nsplice-Standard-Coupler" && pBPCModels.coupler_end == "Nsplice-Extended-Stud")
                {
                    lMainBars.BarShapeCode = "J1I";
                    lMainBars.A = pA;
                }
                //Set 7
                if (pBPCModels.coupler_top == "Nsplice-Standard-Stud" && pBPCModels.coupler_end == "No-Coupler")
                {
                    lMainBars.BarShapeCode = "K20";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "Nsplice-Standard-Stud" && pBPCModels.coupler_end == "Esplice-Standard-Coupler")
                {
                    //No shape code available for Esplice and Nsplice
                    lMainBars.BarShapeCode = "020";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "Nsplice-Standard-Stud" && pBPCModels.coupler_end == "Esplice-Standard-Stud")
                {
                    //No shape code available for Esplice and Nsplice
                    lMainBars.BarShapeCode = "020";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "Nsplice-Standard-Stud" && pBPCModels.coupler_end == "Esplice-Extended-Coupler")
                {
                    //No shape code available for Esplice and Nsplice
                    lMainBars.BarShapeCode = "020";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "Nsplice-Standard-Stud" && pBPCModels.coupler_end == "Esplice-Extended-Stud")
                {
                    //No shape code available for Esplice and Nsplice
                    lMainBars.BarShapeCode = "020";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "Nsplice-Standard-Stud" && pBPCModels.coupler_end == "Nsplice-Standard-Coupler")
                {
                    lMainBars.BarShapeCode = "J1K";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "Nsplice-Standard-Stud" && pBPCModels.coupler_end == "Nsplice-Standard-Stud")
                {
                    lMainBars.BarShapeCode = "K1K";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "Nsplice-Standard-Stud" && pBPCModels.coupler_end == "Nsplice-Extended-Coupler")
                {
                    lMainBars.BarShapeCode = "K1H";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "Nsplice-Standard-Stud" && pBPCModels.coupler_end == "Nsplice-Extended-Stud")
                {
                    lMainBars.BarShapeCode = "K1I";
                    lMainBars.A = pA;
                }
                //Set 8
                if (pBPCModels.coupler_top == "Nsplice-Extended-Coupler" && pBPCModels.coupler_end == "No-Coupler")
                {
                    lMainBars.BarShapeCode = "H20";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "Nsplice-Extended-Coupler" && pBPCModels.coupler_end == "Esplice-Standard-Coupler")
                {
                    //No shape code available for Esplice and Nsplice
                    lMainBars.BarShapeCode = "020";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "Nsplice-Extended-Coupler" && pBPCModels.coupler_end == "Esplice-Standard-Stud")
                {
                    //No shape code available for Esplice and Nsplice
                    lMainBars.BarShapeCode = "020";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "Nsplice-Extended-Coupler" && pBPCModels.coupler_end == "Esplice-Extended-Coupler")
                {
                    //No shape code available for Esplice and Nsplice
                    lMainBars.BarShapeCode = "020";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "Nsplice-Extended-Coupler" && pBPCModels.coupler_end == "Esplice-Extended-Stud")
                {
                    //No shape code available for Esplice and Nsplice
                    lMainBars.BarShapeCode = "020";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "Nsplice-Extended-Coupler" && pBPCModels.coupler_end == "Nsplice-Standard-Coupler")
                {
                    lMainBars.BarShapeCode = "H1J";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "Nsplice-Extended-Coupler" && pBPCModels.coupler_end == "Nsplice-Standard-Stud")
                {
                    lMainBars.BarShapeCode = "K1H";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "Nsplice-Extended-Coupler" && pBPCModels.coupler_end == "Nsplice-Extended-Coupler")
                {
                    lMainBars.BarShapeCode = "H1H";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "Nsplice-Extended-Coupler" && pBPCModels.coupler_end == "Nsplice-Extended-Stud")
                {
                    lMainBars.BarShapeCode = "I1H";
                    lMainBars.A = pA;
                }
                //Set 9
                if (pBPCModels.coupler_top == "Nsplice-Extended-Stud" && pBPCModels.coupler_end == "No-Coupler")
                {
                    lMainBars.BarShapeCode = "I20";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "Nsplice-Extended-Stud" && pBPCModels.coupler_end == "Esplice-Standard-Coupler")
                {
                    //No shape code available for Esplice and Nsplice
                    lMainBars.BarShapeCode = "020";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "Nsplice-Extended-Stud" && pBPCModels.coupler_end == "Esplice-Standard-Stud")
                {
                    //No shape code available for Esplice and Nsplice
                    lMainBars.BarShapeCode = "020";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "Nsplice-Extended-Stud" && pBPCModels.coupler_end == "Esplice-Extended-Coupler")
                {
                    //No shape code available for Esplice and Nsplice
                    lMainBars.BarShapeCode = "020";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "Nsplice-Extended-Stud" && pBPCModels.coupler_end == "Esplice-Extended-Stud")
                {
                    //No shape code available for Esplice and Nsplice
                    lMainBars.BarShapeCode = "020";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "Nsplice-Extended-Stud" && pBPCModels.coupler_end == "Nsplice-Standard-Coupler")
                {
                    lMainBars.BarShapeCode = "J1I";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "Nsplice-Extended-Stud" && pBPCModels.coupler_end == "Nsplice-Standard-Stud")
                {
                    lMainBars.BarShapeCode = "K1I";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "Nsplice-Extended-Stud" && pBPCModels.coupler_end == "Nsplice-Extended-Coupler")
                {
                    lMainBars.BarShapeCode = "I1H";
                    lMainBars.A = pA;
                }
                else if (pBPCModels.coupler_top == "Nsplice-Extended-Stud" && pBPCModels.coupler_end == "Nsplice-Extended-Stud")
                {
                    lMainBars.BarShapeCode = "I1I";
                    lMainBars.A = pA;
                }
            }
            if (pShapeCode == "041" && pElemMark == "MB")
            {
                if (pBPCModels.coupler_top == "No-Coupler" && pBPCModels.coupler_end == "No-Coupler")
                {
                    lMainBars.BarShapeCode = "041";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "No-Coupler" && pBPCModels.coupler_end == "Esplice-Standard-Coupler")
                {
                    lMainBars.BarShapeCode = "C41";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "No-Coupler" && pBPCModels.coupler_end == "Esplice-Standard-Stud")
                {
                    lMainBars.BarShapeCode = "S41";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "No-Coupler" && pBPCModels.coupler_end == "Esplice-Extended-Coupler")
                {
                    lMainBars.BarShapeCode = "P41";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "No-Coupler" && pBPCModels.coupler_end == "Esplice-Extended-Stud")
                {
                    //No shape code svailable for N41
                    lMainBars.BarShapeCode = "041";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "No-Coupler" && pBPCModels.coupler_end == "Nsplice-Standard-Coupler")
                {
                    lMainBars.BarShapeCode = "J41";
                    lMainBars.A = pC;
                    lMainBars.B = pB;
                    lMainBars.C = pA;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "No-Coupler" && pBPCModels.coupler_end == "Nsplice-Standard-Stud")
                {
                    lMainBars.BarShapeCode = "K41";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "No-Coupler" && pBPCModels.coupler_end == "Nsplice-Extended-Coupler")
                {
                    lMainBars.BarShapeCode = "H41";
                    lMainBars.A = pC;
                    lMainBars.B = pB;
                    lMainBars.C = pA;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "No-Coupler" && pBPCModels.coupler_end == "Nsplice-Extended-Stud")
                {
                    lMainBars.BarShapeCode = "I41";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                //Set 2
                if (pBPCModels.coupler_top == "Esplice-Standard-Coupler" && pBPCModels.coupler_end == "No-Coupler")
                {
                    lMainBars.BarShapeCode = "C41";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "Esplice-Standard-Coupler" && pBPCModels.coupler_end == "Esplice-Standard-Coupler")
                {
                    lMainBars.BarShapeCode = "041";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "Esplice-Standard-Coupler" && pBPCModels.coupler_end == "Esplice-Standard-Stud")
                {
                    lMainBars.BarShapeCode = "041";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "Esplice-Standard-Coupler" && pBPCModels.coupler_end == "Esplice-Extended-Coupler")
                {
                    lMainBars.BarShapeCode = "041";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "Esplice-Standard-Coupler" && pBPCModels.coupler_end == "Esplice-Extended-Stud")
                {
                    lMainBars.BarShapeCode = "041";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "Esplice-Standard-Coupler" && pBPCModels.coupler_end == "Nsplice-Standard-Coupler")
                {
                    lMainBars.BarShapeCode = "041";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "Esplice-Standard-Coupler" && pBPCModels.coupler_end == "Nsplice-Standard-Stud")
                {
                    lMainBars.BarShapeCode = "041";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "Esplice-Standard-Coupler" && pBPCModels.coupler_end == "Nsplice-Extended-Coupler")
                {
                    lMainBars.BarShapeCode = "041";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "Esplice-Standard-Coupler" && pBPCModels.coupler_end == "Nsplice-Extended-Stud")
                {
                    lMainBars.BarShapeCode = "041";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                //Set 3
                if (pBPCModels.coupler_top == "Esplice-Standard-Stud" && pBPCModels.coupler_end == "No-Coupler")
                {
                    lMainBars.BarShapeCode = "S41";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "Esplice-Standard-Stud" && pBPCModels.coupler_end == "Esplice-Standard-Coupler")
                {
                    lMainBars.BarShapeCode = "041";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "Esplice-Standard-Stud" && pBPCModels.coupler_end == "Esplice-Standard-Stud")
                {
                    lMainBars.BarShapeCode = "041";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "Esplice-Standard-Stud" && pBPCModels.coupler_end == "Esplice-Extended-Coupler")
                {
                    lMainBars.BarShapeCode = "041";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "Esplice-Standard-Stud" && pBPCModels.coupler_end == "Esplice-Extended-Stud")
                {
                    lMainBars.BarShapeCode = "041";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "Esplice-Standard-Stud" && pBPCModels.coupler_end == "Nsplice-Standard-Coupler")
                {
                    lMainBars.BarShapeCode = "041";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "Esplice-Standard-Stud" && pBPCModels.coupler_end == "Nsplice-Standard-Stud")
                {
                    lMainBars.BarShapeCode = "041";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "Esplice-Standard-Stud" && pBPCModels.coupler_end == "Nsplice-Extended-Coupler")
                {
                    lMainBars.BarShapeCode = "041";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "Esplice-Standard-Stud" && pBPCModels.coupler_end == "Nsplice-Extended-Stud")
                {
                    lMainBars.BarShapeCode = "041";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                //Set 4
                if (pBPCModels.coupler_top == "Esplice-Extended-Coupler" && pBPCModels.coupler_end == "No-Coupler")
                {
                    lMainBars.BarShapeCode = "P41";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "Esplice-Extended-Coupler" && pBPCModels.coupler_end == "Esplice-Standard-Coupler")
                {
                    lMainBars.BarShapeCode = "041";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "Esplice-Extended-Coupler" && pBPCModels.coupler_end == "Esplice-Standard-Stud")
                {
                    lMainBars.BarShapeCode = "041";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "Esplice-Extended-Coupler" && pBPCModels.coupler_end == "Esplice-Extended-Coupler")
                {
                    lMainBars.BarShapeCode = "041";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "Esplice-Extended-Coupler" && pBPCModels.coupler_end == "Esplice-Extended-Stud")
                {
                    lMainBars.BarShapeCode = "041";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "Esplice-Extended-Coupler" && pBPCModels.coupler_end == "Nsplice-Standard-Coupler")
                {
                    lMainBars.BarShapeCode = "041";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "Esplice-Extended-Coupler" && pBPCModels.coupler_end == "Nsplice-Standard-Stud")
                {
                    lMainBars.BarShapeCode = "041";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "Esplice-Extended-Coupler" && pBPCModels.coupler_end == "Nsplice-Extended-Coupler")
                {
                    lMainBars.BarShapeCode = "041";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "Esplice-Extended-Coupler" && pBPCModels.coupler_end == "Nsplice-Extended-Stud")
                {
                    lMainBars.BarShapeCode = "041";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                //Set 5
                if (pBPCModels.coupler_top == "Esplice-Extended-Stud" && pBPCModels.coupler_end == "No-Coupler")
                {
                    lMainBars.BarShapeCode = "041";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "Esplice-Extended-Stud" && pBPCModels.coupler_end == "Esplice-Standard-Coupler")
                {
                    lMainBars.BarShapeCode = "041";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "Esplice-Extended-Stud" && pBPCModels.coupler_end == "Esplice-Standard-Stud")
                {
                    lMainBars.BarShapeCode = "041";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "Esplice-Extended-Stud" && pBPCModels.coupler_end == "Esplice-Extended-Coupler")
                {
                    lMainBars.BarShapeCode = "041";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "Esplice-Extended-Stud" && pBPCModels.coupler_end == "Esplice-Extended-Stud")
                {
                    lMainBars.BarShapeCode = "041";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "Esplice-Extended-Stud" && pBPCModels.coupler_end == "Nsplice-Standard-Coupler")
                {
                    lMainBars.BarShapeCode = "041";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "Esplice-Extended-Stud" && pBPCModels.coupler_end == "Nsplice-Standard-Stud")
                {
                    lMainBars.BarShapeCode = "041";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "Esplice-Extended-Stud" && pBPCModels.coupler_end == "Nsplice-Extended-Coupler")
                {
                    lMainBars.BarShapeCode = "041";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "Esplice-Extended-Stud" && pBPCModels.coupler_end == "Nsplice-Extended-Stud")
                {
                    lMainBars.BarShapeCode = "041";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                //Set 6
                if (pBPCModels.coupler_top == "Nsplice-Standard-Coupler" && pBPCModels.coupler_end == "No-Coupler")
                {
                    lMainBars.BarShapeCode = "J41";
                    lMainBars.A = pC;
                    lMainBars.B = pB;
                    lMainBars.C = pA;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "Nsplice-Standard-Coupler" && pBPCModels.coupler_end == "Esplice-Standard-Coupler")
                {
                    lMainBars.BarShapeCode = "041";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "Nsplice-Standard-Coupler" && pBPCModels.coupler_end == "Esplice-Standard-Stud")
                {
                    lMainBars.BarShapeCode = "041";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "Nsplice-Standard-Coupler" && pBPCModels.coupler_end == "Esplice-Extended-Coupler")
                {
                    lMainBars.BarShapeCode = "041";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "Nsplice-Standard-Coupler" && pBPCModels.coupler_end == "Esplice-Extended-Stud")
                {
                    lMainBars.BarShapeCode = "041";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "Nsplice-Standard-Coupler" && pBPCModels.coupler_end == "Nsplice-Standard-Coupler")
                {
                    lMainBars.BarShapeCode = "041";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "Nsplice-Standard-Coupler" && pBPCModels.coupler_end == "Nsplice-Standard-Stud")
                {
                    lMainBars.BarShapeCode = "041";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "Nsplice-Standard-Coupler" && pBPCModels.coupler_end == "Nsplice-Extended-Coupler")
                {
                    lMainBars.BarShapeCode = "041";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "Nsplice-Standard-Coupler" && pBPCModels.coupler_end == "Nsplice-Extended-Stud")
                {
                    lMainBars.BarShapeCode = "041";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                //Set 7
                if (pBPCModels.coupler_top == "Nsplice-Standard-Stud" && pBPCModels.coupler_end == "No-Coupler")
                {
                    lMainBars.BarShapeCode = "K41";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "Nsplice-Standard-Stud" && pBPCModels.coupler_end == "Esplice-Standard-Coupler")
                {
                    lMainBars.BarShapeCode = "041";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "Nsplice-Standard-Stud" && pBPCModels.coupler_end == "Esplice-Standard-Stud")
                {
                    lMainBars.BarShapeCode = "041";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "Nsplice-Standard-Stud" && pBPCModels.coupler_end == "Esplice-Extended-Coupler")
                {
                    lMainBars.BarShapeCode = "041";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "Nsplice-Standard-Stud" && pBPCModels.coupler_end == "Esplice-Extended-Stud")
                {
                    lMainBars.BarShapeCode = "041";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "Nsplice-Standard-Stud" && pBPCModels.coupler_end == "Nsplice-Standard-Coupler")
                {
                    lMainBars.BarShapeCode = "041";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "Nsplice-Standard-Stud" && pBPCModels.coupler_end == "Nsplice-Standard-Stud")
                {
                    lMainBars.BarShapeCode = "041";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "Nsplice-Standard-Stud" && pBPCModels.coupler_end == "Nsplice-Extended-Coupler")
                {
                    lMainBars.BarShapeCode = "041";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "Nsplice-Standard-Stud" && pBPCModels.coupler_end == "Nsplice-Extended-Stud")
                {
                    lMainBars.BarShapeCode = "041";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                //Set 8
                if (pBPCModels.coupler_top == "Nsplice-Extended-Coupler" && pBPCModels.coupler_end == "No-Coupler")
                {
                    lMainBars.BarShapeCode = "H41";
                    lMainBars.A = pC;
                    lMainBars.B = pB;
                    lMainBars.C = pA;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "Nsplice-Extended-Coupler" && pBPCModels.coupler_end == "Esplice-Standard-Coupler")
                {
                    lMainBars.BarShapeCode = "041";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "Nsplice-Extended-Coupler" && pBPCModels.coupler_end == "Esplice-Standard-Stud")
                {
                    lMainBars.BarShapeCode = "041";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "Nsplice-Extended-Coupler" && pBPCModels.coupler_end == "Esplice-Extended-Coupler")
                {
                    lMainBars.BarShapeCode = "041";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "Nsplice-Extended-Coupler" && pBPCModels.coupler_end == "Esplice-Extended-Stud")
                {
                    lMainBars.BarShapeCode = "041";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "Nsplice-Extended-Coupler" && pBPCModels.coupler_end == "Nsplice-Standard-Coupler")
                {
                    lMainBars.BarShapeCode = "041";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "Nsplice-Extended-Coupler" && pBPCModels.coupler_end == "Nsplice-Standard-Stud")
                {
                    lMainBars.BarShapeCode = "041";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "Nsplice-Extended-Coupler" && pBPCModels.coupler_end == "Nsplice-Extended-Coupler")
                {
                    lMainBars.BarShapeCode = "041";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "Nsplice-Extended-Coupler" && pBPCModels.coupler_end == "Nsplice-Extended-Stud")
                {
                    lMainBars.BarShapeCode = "041";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                //Set 9
                if (pBPCModels.coupler_top == "Nsplice-Extended-Stud" && pBPCModels.coupler_end == "No-Coupler")
                {
                    lMainBars.BarShapeCode = "I41";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "Nsplice-Extended-Stud" && pBPCModels.coupler_end == "Esplice-Standard-Coupler")
                {
                    lMainBars.BarShapeCode = "041";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "Nsplice-Extended-Stud" && pBPCModels.coupler_end == "Esplice-Standard-Stud")
                {
                    lMainBars.BarShapeCode = "041";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "Nsplice-Extended-Stud" && pBPCModels.coupler_end == "Esplice-Extended-Coupler")
                {
                    lMainBars.BarShapeCode = "041";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "Nsplice-Extended-Stud" && pBPCModels.coupler_end == "Esplice-Extended-Stud")
                {
                    lMainBars.BarShapeCode = "041";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "Nsplice-Extended-Stud" && pBPCModels.coupler_end == "Nsplice-Standard-Coupler")
                {
                    lMainBars.BarShapeCode = "041";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "Nsplice-Extended-Stud" && pBPCModels.coupler_end == "Nsplice-Standard-Stud")
                {
                    lMainBars.BarShapeCode = "041";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "Nsplice-Extended-Stud" && pBPCModels.coupler_end == "Nsplice-Extended-Coupler")
                {
                    lMainBars.BarShapeCode = "041";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
                else if (pBPCModels.coupler_top == "Nsplice-Extended-Stud" && pBPCModels.coupler_end == "Nsplice-Extended-Stud")
                {
                    lMainBars.BarShapeCode = "041";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                }
            }
            if (pShapeCode == "043" && pElemMark == "MB")
            {
                if (pBPCModels.coupler_top == "No-Coupler" && pBPCModels.coupler_end == "No-Coupler")
                {
                    lMainBars.BarShapeCode = "043";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                    lMainBars.E = pE;
                    lMainBars.F = pF;
                }
                else if (pBPCModels.coupler_top == "No-Coupler" && pBPCModels.coupler_end == "Esplice-Standard-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "No-Coupler" && pBPCModels.coupler_end == "Esplice-Standard-Stud")
                {
                }
                else if (pBPCModels.coupler_top == "No-Coupler" && pBPCModels.coupler_end == "Esplice-Extended-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "No-Coupler" && pBPCModels.coupler_end == "Esplice-Extended-Stud")
                {
                }
                else if (pBPCModels.coupler_top == "No-Coupler" && pBPCModels.coupler_end == "Nsplice-Standard-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "No-Coupler" && pBPCModels.coupler_end == "Nsplice-Standard-Stud")
                {
                }
                else if (pBPCModels.coupler_top == "No-Coupler" && pBPCModels.coupler_end == "Nsplice-Extended-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "No-Coupler" && pBPCModels.coupler_end == "Nsplice-Extended-Stud")
                {
                }
                //Set 2
                if (pBPCModels.coupler_top == "Esplice-Standard-Coupler" && pBPCModels.coupler_end == "No-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Esplice-Standard-Coupler" && pBPCModels.coupler_end == "Esplice-Standard-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Esplice-Standard-Coupler" && pBPCModels.coupler_end == "Esplice-Standard-Stud")
                {
                }
                else if (pBPCModels.coupler_top == "Esplice-Standard-Coupler" && pBPCModels.coupler_end == "Esplice-Extended-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Esplice-Standard-Coupler" && pBPCModels.coupler_end == "Esplice-Extended-Stud")
                {
                }
                else if (pBPCModels.coupler_top == "Esplice-Standard-Coupler" && pBPCModels.coupler_end == "Nsplice-Standard-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Esplice-Standard-Coupler" && pBPCModels.coupler_end == "Nsplice-Standard-Stud")
                {
                }
                else if (pBPCModels.coupler_top == "Esplice-Standard-Coupler" && pBPCModels.coupler_end == "Nsplice-Extended-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Esplice-Standard-Coupler" && pBPCModels.coupler_end == "Nsplice-Extended-Stud")
                {
                }
                //Set 3
                if (pBPCModels.coupler_top == "Esplice-Standard-Stud" && pBPCModels.coupler_end == "No-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Esplice-Standard-Stud" && pBPCModels.coupler_end == "Esplice-Standard-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Esplice-Standard-Stud" && pBPCModels.coupler_end == "Esplice-Standard-Stud")
                {
                }
                else if (pBPCModels.coupler_top == "Esplice-Standard-Stud" && pBPCModels.coupler_end == "Esplice-Extended-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Esplice-Standard-Stud" && pBPCModels.coupler_end == "Esplice-Extended-Stud")
                {
                }
                else if (pBPCModels.coupler_top == "Esplice-Standard-Stud" && pBPCModels.coupler_end == "Nsplice-Standard-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Esplice-Standard-Stud" && pBPCModels.coupler_end == "Nsplice-Standard-Stud")
                {
                }
                else if (pBPCModels.coupler_top == "Esplice-Standard-Stud" && pBPCModels.coupler_end == "Nsplice-Extended-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Esplice-Standard-Stud" && pBPCModels.coupler_end == "Nsplice-Extended-Stud")
                {
                }
                //Set 4
                if (pBPCModels.coupler_top == "Esplice-Extended-Coupler" && pBPCModels.coupler_end == "No-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Esplice-Extended-Coupler" && pBPCModels.coupler_end == "Esplice-Standard-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Esplice-Extended-Coupler" && pBPCModels.coupler_end == "Esplice-Standard-Stud")
                {
                }
                else if (pBPCModels.coupler_top == "Esplice-Extended-Coupler" && pBPCModels.coupler_end == "Esplice-Extended-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Esplice-Extended-Coupler" && pBPCModels.coupler_end == "Esplice-Extended-Stud")
                {
                }
                else if (pBPCModels.coupler_top == "Esplice-Extended-Coupler" && pBPCModels.coupler_end == "Nsplice-Standard-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Esplice-Extended-Coupler" && pBPCModels.coupler_end == "Nsplice-Standard-Stud")
                {
                }
                else if (pBPCModels.coupler_top == "Esplice-Extended-Coupler" && pBPCModels.coupler_end == "Nsplice-Extended-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Esplice-Extended-Coupler" && pBPCModels.coupler_end == "Nsplice-Extended-Stud")
                {
                }
                //Set 5
                if (pBPCModels.coupler_top == "Esplice-Extended-Stud" && pBPCModels.coupler_end == "No-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Esplice-Extended-Stud" && pBPCModels.coupler_end == "Esplice-Standard-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Esplice-Extended-Stud" && pBPCModels.coupler_end == "Esplice-Standard-Stud")
                {
                }
                else if (pBPCModels.coupler_top == "Esplice-Extended-Stud" && pBPCModels.coupler_end == "Esplice-Extended-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Esplice-Extended-Stud" && pBPCModels.coupler_end == "Esplice-Extended-Stud")
                {
                }
                else if (pBPCModels.coupler_top == "Esplice-Extended-Stud" && pBPCModels.coupler_end == "Nsplice-Standard-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Esplice-Extended-Stud" && pBPCModels.coupler_end == "Nsplice-Standard-Stud")
                {
                }
                else if (pBPCModels.coupler_top == "Esplice-Extended-Stud" && pBPCModels.coupler_end == "Nsplice-Extended-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Esplice-Extended-Stud" && pBPCModels.coupler_end == "Nsplice-Extended-Stud")
                {
                }
                //Set 6
                if (pBPCModels.coupler_top == "Nsplice-Standard-Coupler" && pBPCModels.coupler_end == "No-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Nsplice-Standard-Coupler" && pBPCModels.coupler_end == "Esplice-Standard-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Nsplice-Standard-Coupler" && pBPCModels.coupler_end == "Esplice-Standard-Stud")
                {
                }
                else if (pBPCModels.coupler_top == "Nsplice-Standard-Coupler" && pBPCModels.coupler_end == "Esplice-Extended-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Nsplice-Standard-Coupler" && pBPCModels.coupler_end == "Esplice-Extended-Stud")
                {
                }
                else if (pBPCModels.coupler_top == "Nsplice-Standard-Coupler" && pBPCModels.coupler_end == "Nsplice-Standard-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Nsplice-Standard-Coupler" && pBPCModels.coupler_end == "Nsplice-Standard-Stud")
                {
                }
                else if (pBPCModels.coupler_top == "Nsplice-Standard-Coupler" && pBPCModels.coupler_end == "Nsplice-Extended-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Nsplice-Standard-Coupler" && pBPCModels.coupler_end == "Nsplice-Extended-Stud")
                {
                }
                //Set 7
                if (pBPCModels.coupler_top == "Nsplice-Standard-Stud" && pBPCModels.coupler_end == "No-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Nsplice-Standard-Stud" && pBPCModels.coupler_end == "Esplice-Standard-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Nsplice-Standard-Stud" && pBPCModels.coupler_end == "Esplice-Standard-Stud")
                {
                }
                else if (pBPCModels.coupler_top == "Nsplice-Standard-Stud" && pBPCModels.coupler_end == "Esplice-Extended-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Nsplice-Standard-Stud" && pBPCModels.coupler_end == "Esplice-Extended-Stud")
                {
                }
                else if (pBPCModels.coupler_top == "Nsplice-Standard-Stud" && pBPCModels.coupler_end == "Nsplice-Standard-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Nsplice-Standard-Stud" && pBPCModels.coupler_end == "Nsplice-Standard-Stud")
                {
                }
                else if (pBPCModels.coupler_top == "Nsplice-Standard-Stud" && pBPCModels.coupler_end == "Nsplice-Extended-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Nsplice-Standard-Stud" && pBPCModels.coupler_end == "Nsplice-Extended-Stud")
                {
                }
                //Set 8
                if (pBPCModels.coupler_top == "Nsplice-Extended-Coupler" && pBPCModels.coupler_end == "No-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Nsplice-Extended-Coupler" && pBPCModels.coupler_end == "Esplice-Standard-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Nsplice-Extended-Coupler" && pBPCModels.coupler_end == "Esplice-Standard-Stud")
                {
                }
                else if (pBPCModels.coupler_top == "Nsplice-Extended-Coupler" && pBPCModels.coupler_end == "Esplice-Extended-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Nsplice-Extended-Coupler" && pBPCModels.coupler_end == "Esplice-Extended-Stud")
                {
                }
                else if (pBPCModels.coupler_top == "Nsplice-Extended-Coupler" && pBPCModels.coupler_end == "Nsplice-Standard-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Nsplice-Extended-Coupler" && pBPCModels.coupler_end == "Nsplice-Standard-Stud")
                {
                }
                else if (pBPCModels.coupler_top == "Nsplice-Extended-Coupler" && pBPCModels.coupler_end == "Nsplice-Extended-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Nsplice-Extended-Coupler" && pBPCModels.coupler_end == "Nsplice-Extended-Stud")
                {
                }
                //Set 9
                if (pBPCModels.coupler_top == "Nsplice-Extended-Stud" && pBPCModels.coupler_end == "No-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Nsplice-Extended-Stud" && pBPCModels.coupler_end == "Esplice-Standard-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Nsplice-Extended-Stud" && pBPCModels.coupler_end == "Esplice-Standard-Stud")
                {
                }
                else if (pBPCModels.coupler_top == "Nsplice-Extended-Stud" && pBPCModels.coupler_end == "Esplice-Extended-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Nsplice-Extended-Stud" && pBPCModels.coupler_end == "Esplice-Extended-Stud")
                {
                }
                else if (pBPCModels.coupler_top == "Nsplice-Extended-Stud" && pBPCModels.coupler_end == "Nsplice-Standard-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Nsplice-Extended-Stud" && pBPCModels.coupler_end == "Nsplice-Standard-Stud")
                {
                }
                else if (pBPCModels.coupler_top == "Nsplice-Extended-Stud" && pBPCModels.coupler_end == "Nsplice-Extended-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Nsplice-Extended-Stud" && pBPCModels.coupler_end == "Nsplice-Extended-Stud")
                {
                }
            }

            if (pShapeCode == "43B" && pElemMark == "MB")
            {
                if (pBPCModels.coupler_top == "No-Coupler" && pBPCModels.coupler_end == "No-Coupler")
                {
                    lMainBars.BarShapeCode = "43B";
                    lMainBars.A = pA;
                    lMainBars.B = pB;
                    lMainBars.C = pC;
                    lMainBars.D = pD;
                    lMainBars.E = pE;
                    lMainBars.F = pF;
                    lMainBars.G = pG;
                }
                else if (pBPCModels.coupler_top == "No-Coupler" && pBPCModels.coupler_end == "Esplice-Standard-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "No-Coupler" && pBPCModels.coupler_end == "Esplice-Standard-Stud")
                {
                }
                else if (pBPCModels.coupler_top == "No-Coupler" && pBPCModels.coupler_end == "Esplice-Extended-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "No-Coupler" && pBPCModels.coupler_end == "Esplice-Extended-Stud")
                {
                }
                else if (pBPCModels.coupler_top == "No-Coupler" && pBPCModels.coupler_end == "Nsplice-Standard-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "No-Coupler" && pBPCModels.coupler_end == "Nsplice-Standard-Stud")
                {
                }
                else if (pBPCModels.coupler_top == "No-Coupler" && pBPCModels.coupler_end == "Nsplice-Extended-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "No-Coupler" && pBPCModels.coupler_end == "Nsplice-Extended-Stud")
                {
                }
                //Set 2
                if (pBPCModels.coupler_top == "Esplice-Standard-Coupler" && pBPCModels.coupler_end == "No-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Esplice-Standard-Coupler" && pBPCModels.coupler_end == "Esplice-Standard-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Esplice-Standard-Coupler" && pBPCModels.coupler_end == "Esplice-Standard-Stud")
                {
                }
                else if (pBPCModels.coupler_top == "Esplice-Standard-Coupler" && pBPCModels.coupler_end == "Esplice-Extended-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Esplice-Standard-Coupler" && pBPCModels.coupler_end == "Esplice-Extended-Stud")
                {
                }
                else if (pBPCModels.coupler_top == "Esplice-Standard-Coupler" && pBPCModels.coupler_end == "Nsplice-Standard-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Esplice-Standard-Coupler" && pBPCModels.coupler_end == "Nsplice-Standard-Stud")
                {
                }
                else if (pBPCModels.coupler_top == "Esplice-Standard-Coupler" && pBPCModels.coupler_end == "Nsplice-Extended-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Esplice-Standard-Coupler" && pBPCModels.coupler_end == "Nsplice-Extended-Stud")
                {
                }
                //Set 3
                if (pBPCModels.coupler_top == "Esplice-Standard-Stud" && pBPCModels.coupler_end == "No-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Esplice-Standard-Stud" && pBPCModels.coupler_end == "Esplice-Standard-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Esplice-Standard-Stud" && pBPCModels.coupler_end == "Esplice-Standard-Stud")
                {
                }
                else if (pBPCModels.coupler_top == "Esplice-Standard-Stud" && pBPCModels.coupler_end == "Esplice-Extended-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Esplice-Standard-Stud" && pBPCModels.coupler_end == "Esplice-Extended-Stud")
                {
                }
                else if (pBPCModels.coupler_top == "Esplice-Standard-Stud" && pBPCModels.coupler_end == "Nsplice-Standard-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Esplice-Standard-Stud" && pBPCModels.coupler_end == "Nsplice-Standard-Stud")
                {
                }
                else if (pBPCModels.coupler_top == "Esplice-Standard-Stud" && pBPCModels.coupler_end == "Nsplice-Extended-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Esplice-Standard-Stud" && pBPCModels.coupler_end == "Nsplice-Extended-Stud")
                {
                }
                //Set 4
                if (pBPCModels.coupler_top == "Esplice-Extended-Coupler" && pBPCModels.coupler_end == "No-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Esplice-Extended-Coupler" && pBPCModels.coupler_end == "Esplice-Standard-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Esplice-Extended-Coupler" && pBPCModels.coupler_end == "Esplice-Standard-Stud")
                {
                }
                else if (pBPCModels.coupler_top == "Esplice-Extended-Coupler" && pBPCModels.coupler_end == "Esplice-Extended-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Esplice-Extended-Coupler" && pBPCModels.coupler_end == "Esplice-Extended-Stud")
                {
                }
                else if (pBPCModels.coupler_top == "Esplice-Extended-Coupler" && pBPCModels.coupler_end == "Nsplice-Standard-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Esplice-Extended-Coupler" && pBPCModels.coupler_end == "Nsplice-Standard-Stud")
                {
                }
                else if (pBPCModels.coupler_top == "Esplice-Extended-Coupler" && pBPCModels.coupler_end == "Nsplice-Extended-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Esplice-Extended-Coupler" && pBPCModels.coupler_end == "Nsplice-Extended-Stud")
                {
                }
                //Set 5
                if (pBPCModels.coupler_top == "Esplice-Extended-Stud" && pBPCModels.coupler_end == "No-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Esplice-Extended-Stud" && pBPCModels.coupler_end == "Esplice-Standard-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Esplice-Extended-Stud" && pBPCModels.coupler_end == "Esplice-Standard-Stud")
                {
                }
                else if (pBPCModels.coupler_top == "Esplice-Extended-Stud" && pBPCModels.coupler_end == "Esplice-Extended-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Esplice-Extended-Stud" && pBPCModels.coupler_end == "Esplice-Extended-Stud")
                {
                }
                else if (pBPCModels.coupler_top == "Esplice-Extended-Stud" && pBPCModels.coupler_end == "Nsplice-Standard-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Esplice-Extended-Stud" && pBPCModels.coupler_end == "Nsplice-Standard-Stud")
                {
                }
                else if (pBPCModels.coupler_top == "Esplice-Extended-Stud" && pBPCModels.coupler_end == "Nsplice-Extended-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Esplice-Extended-Stud" && pBPCModels.coupler_end == "Nsplice-Extended-Stud")
                {
                }
                //Set 6
                if (pBPCModels.coupler_top == "Nsplice-Standard-Coupler" && pBPCModels.coupler_end == "No-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Nsplice-Standard-Coupler" && pBPCModels.coupler_end == "Esplice-Standard-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Nsplice-Standard-Coupler" && pBPCModels.coupler_end == "Esplice-Standard-Stud")
                {
                }
                else if (pBPCModels.coupler_top == "Nsplice-Standard-Coupler" && pBPCModels.coupler_end == "Esplice-Extended-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Nsplice-Standard-Coupler" && pBPCModels.coupler_end == "Esplice-Extended-Stud")
                {
                }
                else if (pBPCModels.coupler_top == "Nsplice-Standard-Coupler" && pBPCModels.coupler_end == "Nsplice-Standard-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Nsplice-Standard-Coupler" && pBPCModels.coupler_end == "Nsplice-Standard-Stud")
                {
                }
                else if (pBPCModels.coupler_top == "Nsplice-Standard-Coupler" && pBPCModels.coupler_end == "Nsplice-Extended-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Nsplice-Standard-Coupler" && pBPCModels.coupler_end == "Nsplice-Extended-Stud")
                {
                }
                //Set 7
                if (pBPCModels.coupler_top == "Nsplice-Standard-Stud" && pBPCModels.coupler_end == "No-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Nsplice-Standard-Stud" && pBPCModels.coupler_end == "Esplice-Standard-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Nsplice-Standard-Stud" && pBPCModels.coupler_end == "Esplice-Standard-Stud")
                {
                }
                else if (pBPCModels.coupler_top == "Nsplice-Standard-Stud" && pBPCModels.coupler_end == "Esplice-Extended-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Nsplice-Standard-Stud" && pBPCModels.coupler_end == "Esplice-Extended-Stud")
                {
                }
                else if (pBPCModels.coupler_top == "Nsplice-Standard-Stud" && pBPCModels.coupler_end == "Nsplice-Standard-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Nsplice-Standard-Stud" && pBPCModels.coupler_end == "Nsplice-Standard-Stud")
                {
                }
                else if (pBPCModels.coupler_top == "Nsplice-Standard-Stud" && pBPCModels.coupler_end == "Nsplice-Extended-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Nsplice-Standard-Stud" && pBPCModels.coupler_end == "Nsplice-Extended-Stud")
                {
                }
                //Set 8
                if (pBPCModels.coupler_top == "Nsplice-Extended-Coupler" && pBPCModels.coupler_end == "No-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Nsplice-Extended-Coupler" && pBPCModels.coupler_end == "Esplice-Standard-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Nsplice-Extended-Coupler" && pBPCModels.coupler_end == "Esplice-Standard-Stud")
                {
                }
                else if (pBPCModels.coupler_top == "Nsplice-Extended-Coupler" && pBPCModels.coupler_end == "Esplice-Extended-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Nsplice-Extended-Coupler" && pBPCModels.coupler_end == "Esplice-Extended-Stud")
                {
                }
                else if (pBPCModels.coupler_top == "Nsplice-Extended-Coupler" && pBPCModels.coupler_end == "Nsplice-Standard-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Nsplice-Extended-Coupler" && pBPCModels.coupler_end == "Nsplice-Standard-Stud")
                {
                }
                else if (pBPCModels.coupler_top == "Nsplice-Extended-Coupler" && pBPCModels.coupler_end == "Nsplice-Extended-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Nsplice-Extended-Coupler" && pBPCModels.coupler_end == "Nsplice-Extended-Stud")
                {
                }
                //Set 9
                if (pBPCModels.coupler_top == "Nsplice-Extended-Stud" && pBPCModels.coupler_end == "No-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Nsplice-Extended-Stud" && pBPCModels.coupler_end == "Esplice-Standard-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Nsplice-Extended-Stud" && pBPCModels.coupler_end == "Esplice-Standard-Stud")
                {
                }
                else if (pBPCModels.coupler_top == "Nsplice-Extended-Stud" && pBPCModels.coupler_end == "Esplice-Extended-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Nsplice-Extended-Stud" && pBPCModels.coupler_end == "Esplice-Extended-Stud")
                {
                }
                else if (pBPCModels.coupler_top == "Nsplice-Extended-Stud" && pBPCModels.coupler_end == "Nsplice-Standard-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Nsplice-Extended-Stud" && pBPCModels.coupler_end == "Nsplice-Standard-Stud")
                {
                }
                else if (pBPCModels.coupler_top == "Nsplice-Extended-Stud" && pBPCModels.coupler_end == "Nsplice-Extended-Coupler")
                {
                }
                else if (pBPCModels.coupler_top == "Nsplice-Extended-Stud" && pBPCModels.coupler_end == "Nsplice-Extended-Stud")
                {
                }
            }
            return lMainBars;

        }

        [HttpGet]
        [Route("/getRebarWeight_bpc/{pDia}/{pLength}")]
        double getRebarWeight(int pDia, int pLength)
        {
            var lStdDia = new int[] { 6, 8, 10, 12, 13, 16, 20, 24, 25, 28, 32, 36, 40, 50 };
            var lStdUnitWT = new double[] { 0.222, 0.395, 0.617, 0.888, 1.042, 1.579, 2.466, 3.699, 3.854, 4.834, 6.313, 7.769, 9.864, 15.413 };
            double lWeight = 0;

            double lUnitWT = 0;

            if (pDia > 0 && pLength > 0)
            {
                for (var i = 0; i < lStdDia.Length; i++)
                {
                    if (pDia == lStdDia[i])
                    {
                        lUnitWT = lStdUnitWT[i];
                        break;
                    }
                }
            }

            if (lUnitWT > 0)
            {
                lWeight = lUnitWT * pLength / 1000;
            }

            return lWeight;

        }

        [HttpPost]
        [Route("/AssignTemplates_bpc")]
        List<BPCTemplateModels> AssignTemplates(BPCDetailsModels pBPCModels)
        {
            var lPileType = pBPCModels.pile_type;
            var lPileDia = pBPCModels.pile_dia;
            var lCageDia = pBPCModels.cage_dia;
            var lMainBarArrange = pBPCModels.main_bar_arrange;
            var lMainBarType = pBPCModels.main_bar_type;
            var lMainBarCT = pBPCModels.main_bar_ct;
            var lMainBarDiaS = pBPCModels.main_bar_dia;
            var lCover = (int)Math.Round((double)(lPileDia - lCageDia) / 2);
            var lSameBarBundle = pBPCModels.bundle_same_type;
            var lNoOfHoles = 0;
            var lBundled = 0;
            var lMainBarDia = 0;
            int lTemplateID = 1;
            var ProjectCode = pBPCModels.ProjectCode;
            pBPCModels.sr_link_lapping = pBPCModels.sr_link_lapping ?? 10;
            pBPCModels.cr_link_lapping = pBPCModels.cr_link_lapping ?? 51;
            if (lSameBarBundle == null || lSameBarBundle == "")
            {
                lSameBarBundle = "N";
            }
            if (lPileType == "Single-Layer" && lMainBarType == "Single")
            {
                if (lMainBarArrange == "Single")
                {
                    lNoOfHoles = int.Parse(lMainBarCT);
                    lBundled = 0;
                    lMainBarDia = int.Parse(lMainBarDiaS);
                }
                else if (lMainBarArrange == "Side-By-Side")
                {
                    lNoOfHoles = (int)Math.Floor((double)int.Parse(lMainBarCT) / 2);
                    lBundled = 0;
                    lMainBarDia = int.Parse(lMainBarDiaS);
                }
                else if (lMainBarArrange == "In-Out")
                {
                    lNoOfHoles = (int)Math.Floor((double)int.Parse(lMainBarCT) / 2);
                    lBundled = 0;
                    lMainBarDia = int.Parse(lMainBarDiaS);
                }
            }
            else if (lPileType == "Single-Layer" && lMainBarType == "Mixed")
            {

                var lMainBarDiaArr = lMainBarDiaS.Split(',');
                if (lMainBarDiaArr.Length > 1)
                {
                    if (int.Parse(lMainBarDiaArr[0]) >= int.Parse(lMainBarDiaArr[1]))
                    {
                        lMainBarDia = int.Parse(lMainBarDiaArr[0]);
                    }
                    else
                    {
                        lMainBarDia = int.Parse(lMainBarDiaArr[1]);
                    }
                }

                var lMainbarCTArr = lMainBarCT.Split(',');
                if (lMainbarCTArr.Length > 1)
                {
                    if (lMainBarArrange == "Single")
                    {
                        lNoOfHoles = int.Parse(lMainbarCTArr[0]) + int.Parse(lMainbarCTArr[1]);
                        lBundled = 0;
                    }
                    else if (lMainBarArrange == "Side-By-Side")
                    {
                        if (lSameBarBundle != "Y")
                        {
                            lNoOfHoles = int.Parse(lMainbarCTArr[0]);
                            if (lNoOfHoles < int.Parse(lMainbarCTArr[1]))
                            {
                                lNoOfHoles = int.Parse(lMainbarCTArr[1]);
                            }
                            lBundled = 0;
                        }
                        else
                        {
                            lNoOfHoles = (int)Math.Floor((double)(int.Parse(lMainbarCTArr[0]) + int.Parse(lMainbarCTArr[1])) / 2);
                            lBundled = 0;
                        }
                    }
                    else if (lMainBarArrange == "In-Out")
                    {
                        if (lSameBarBundle != "Y")
                        {
                            lNoOfHoles = int.Parse(lMainbarCTArr[0]);
                            if (lNoOfHoles < int.Parse(lMainbarCTArr[1]))
                            {
                                lNoOfHoles = int.Parse(lMainbarCTArr[1]);
                            }
                            lBundled = 0;
                        }
                        else
                        {
                            lNoOfHoles = (int)Math.Floor((double)(int.Parse(lMainbarCTArr[0]) + int.Parse(lMainbarCTArr[1])) / 2);
                            lBundled = 0;
                        }
                    }
                }

            }
            else if (lPileType == "Double-Layer")
            {
                var lMainbarCTArr = lMainBarCT.Split(',');
                if (lMainbarCTArr.Length > 1)
                {
                    if (lMainBarArrange == "Single")
                    {
                        lNoOfHoles = int.Parse(lMainbarCTArr[0]);
                        lBundled = 0;
                    }
                    else if (lMainBarArrange == "Side-By-Side")
                    {
                        lNoOfHoles = (int)Math.Floor((double)(int.Parse(lMainbarCTArr[0]) / 2));
                        lBundled = 1;
                    }
                    else if (lMainBarArrange == "In-Out")
                    {
                        lNoOfHoles = (int)Math.Floor((double)(int.Parse(lMainbarCTArr[0]) / 2));
                        lBundled = 0;
                    }
                }
                var lMainBarDiaArr = lMainBarDiaS.Split(',');
                if (lMainBarDiaArr.Length > 0)
                {
                    lMainBarDia = int.Parse(lMainBarDiaArr[0]);
                }

            }

            var lCmd = new SqlCommand();
            SqlDataReader lRst;
            var lNDSCon = new SqlConnection();
            var lRecord = new BPCTemplateModels();

            var lRecordList = new List<BPCTemplateModels>();

            lCmd.CommandText =
            "SELECT top 5 template_code " +
            ",pile_dia " +
            ",no_of_holes " +
            ",cover " +
            ",bundled " +
            ",priority " +
            "FROM dbo.OESBPCTemplatesMaster " +
            "WHERE pile_dia = " + lPileDia.ToString() + " " +
            "AND no_of_holes = " + lNoOfHoles.ToString() + " " +
            "AND cover = " + lCover.ToString() + " " +
            "AND (bundled = 1 OR bundled = " + lBundled.ToString() + ") " +
            "AND MainBarMax >= " + lMainBarDia.ToString() + " " +
            "ORDER BY priority ";

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
                        lRecord = new BPCTemplateModels
                        {
                            CustomerCode = pBPCModels.CustomerCode,
                            ProjectCode = pBPCModels.ProjectCode,
                            Template = pBPCModels.Template,
                            JobID = pBPCModels.JobID,
                            CageID = pBPCModels.cage_id,
                            TemplateID = lTemplateID,
                            template_code = lRst.GetString(0).Trim(),
                            pile_dia = lRst.GetInt32(1),
                            cage_dia = lRst.GetInt32(1) - 2 * lRst.GetInt32(3),
                            no_of_holes = lRst.GetInt32(2),
                            cover = lRst.GetInt32(3),
                            bundled = lRst.GetInt32(4),
                            UpdatedBy = "Vishalw_ttl@natsteel.com.sg",
                            UpdateDate = DateTime.Now
                        };

                        lTemplateID++;

                        lRecordList.Add(lRecord);
                    }
                }
                lRst.Close();

                if (lTemplateID < 5)
                {
                    lCmd.CommandText =
                    "SELECT top 5 template_code " +
                    ",pile_dia " +
                    ",no_of_holes " +
                    ",cover " +
                    ",bundled " +
                    ",priority " +
                    "FROM dbo.OESBPCTemplatesMaster " +
                    "WHERE pile_dia = " + lPileDia.ToString() + " " +
                    "AND no_of_holes = " + (2 * lNoOfHoles).ToString() + " " +
                    "AND cover = " + lCover.ToString() + " " +
                    "AND bundled = " + lBundled.ToString() + " " +
                    "ORDER BY priority ";

                    lRst = lCmd.ExecuteReader();
                    if (lRst.HasRows)
                    {
                        while (lRst.Read())
                        {
                            var lTemplateCode = lRst.GetString(0).Trim();
                            var lFound = 0;
                            if (lRecordList.Count > 0)
                            {
                                for (int i = 0; i < lRecordList.Count; i++)
                                {
                                    if (lRecordList[i].template_code == lTemplateCode)
                                    {
                                        lFound = 1;
                                        break;
                                    }
                                }
                            }
                            if (lFound == 0)
                            {
                                lRecord = new BPCTemplateModels
                                {
                                    CustomerCode = pBPCModels.CustomerCode,
                                    ProjectCode = pBPCModels.ProjectCode,
                                    Template = pBPCModels.Template,
                                    JobID = pBPCModels.JobID,
                                    CageID = pBPCModels.cage_id,
                                    TemplateID = lTemplateID,
                                    template_code = lRst.GetString(0).Trim(),
                                    pile_dia = lRst.GetInt32(1),
                                    cage_dia = lRst.GetInt32(1) - 2 * lRst.GetInt32(3),
                                    no_of_holes = lRst.GetInt32(2),
                                    cover = lRst.GetInt32(3),
                                    bundled = lRst.GetInt32(4),
                                    UpdatedBy = "Vishalw_ttl@natsteel.com.sg",
                                    UpdateDate = DateTime.Now
                                };

                                lTemplateID++;

                                lRecordList.Add(lRecord);
                            }
                            if (lTemplateID > 5)
                            {
                                break;
                            }
                        }
                    }
                    lRst.Close();
                }

                if (lTemplateID < 5)
                {
                    lRecord = new BPCTemplateModels
                    {
                        CustomerCode = pBPCModels.CustomerCode,
                        ProjectCode = pBPCModels.ProjectCode,
                        Template = pBPCModels.Template,
                        JobID = pBPCModels.JobID,
                        CageID = pBPCModels.cage_id,
                        TemplateID = lTemplateID,
                        template_code = "SCMEP",
                        pile_dia = lPileDia,
                        cage_dia = lPileDia - 2 * lCover,
                        no_of_holes = lNoOfHoles,
                        cover = lCover,
                        bundled = 0,
                        UpdatedBy = "Vishalw_ttl@natsteel.com.sg",
                        UpdateDate = DateTime.Now
                    };

                    lTemplateID++;

                    lRecordList.Add(lRecord);

                }
                lProcessObj.CloseNDSConnection(ref lNDSCon);
            }
            lProcessObj = null;
            lCmd = null;
            lNDSCon = null;
            lRst = null;

            if (lRecordList.Count <= 1 && pBPCModels.Template == false)
            {
                var lEmailSubject = "No Template Found for BPC Set Code: " + pBPCModels.set_code + " ";

                var lOESEmail = new SendGridEmail();

                //var lEmailTo = "zbc@natsteel.com.sg";
                //var lEmailCc = "aaronzhaobc@gmail.com";

                var lEmailTo = "wl@natsteel.com.sg;heng@natsteel.com.sg";
                var lEmailCc = "ongsh@natsteel.com.sg";
                var lEmailContent = "";

                lEmailContent = "<p align='center'>No Template Found for BPC Set Code : " + pBPCModels.set_code + "</p>";

                lEmailContent = lEmailContent + "<table border='1' width=100%><tr>";
                lEmailContent = lEmailContent + "<td width=20%>" + "Customer " + "</td>";

                CustomerModels lCustomerUX = db.Customer.Find(pBPCModels.CustomerCode);
                string lVar2 = "";
                if (lCustomerUX != null) lVar2 = lCustomerUX.CustomerName.Trim() + " (" + pBPCModels.CustomerCode.Trim() + ")";
                lEmailContent = lEmailContent + "<td>" + lVar2 + "</td></tr>";

                lEmailContent = lEmailContent + "<tr><td>" + "Project " + "</td>";

                var lProjectUX = (from p in db.ProjectList
                                  where p.ProjectCode == ProjectCode
                                  select p).First();
                lVar2 = "";
                if (lProjectUX != null) lVar2 = lProjectUX.ProjectTitle.Trim() + " (" + pBPCModels.ProjectCode.Trim() + ")";
                lEmailContent = lEmailContent + "<td>" + lVar2 + "</td></tr>";

                lEmailContent = lEmailContent + "<tr><td>" + "Cage Diameter " + "</td>";

                lVar2 = "";
                lVar2 = pBPCModels.cage_dia.ToString();
                lEmailContent = lEmailContent + "<td>" + lVar2 + "</td></tr>";

                lEmailContent = lEmailContent + "<tr><td>" + "Cage Length " + "</td>";

                lVar2 = "";
                if (pBPCModels.cage_length != null) lVar2 = pBPCModels.cage_length.Trim();
                lEmailContent = lEmailContent + "<td>" + lVar2 + "</td></tr>";

                lEmailContent = lEmailContent + "<tr><td>" + "Set Code " + "</td>";

                lVar2 = "";
                if (pBPCModels.set_code != null) lVar2 = pBPCModels.set_code.Trim();
                lEmailContent = lEmailContent + "<td>" + lVar2 + "</td></tr></table>";

                string lEmailFromAddress = "eprompt@natsteel.com.sg";
                string lEmailFromName = "Digital Ordering Email Services";
                //lEmailTo = "aaronzhaobc@gmail.com";
                //dynamic response = lOESEmail.Execute(lEmailFromAddress, lEmailFromName, lEmailTo, lEmailCc, lEmailSubject, "text/html", lEmailContent);
                lOESEmail.Execute(lEmailFromAddress, lEmailFromName, lEmailTo, lEmailCc, lEmailSubject, "text/html", lEmailContent).Wait();

                lOESEmail = null;
            }

            return lRecordList;

        }

        //ValidatePONumber
        [HttpGet]
        [Route("/ValidatePONumber_bpc/{CustomerCode}/{ProjectCode}/{Template}/{JobID}/PONumber")]
        //[ValidateAntiForgeryHeader]
        public ActionResult ValidatePONumber(string CustomerCode, string ProjectCode, bool Template, int JobID, string PONumber)
        {
            var lErrorMsg = "";
            try
            {
                var lListOrder = "";
                var lJobAdvice = (from p in db.BPCJobAdvice
                                  where p.CustomerCode == CustomerCode &&
                                  p.ProjectCode == ProjectCode &&
                                  p.Template == Template &&
                                  p.JobID != JobID &&
                                  p.PONumber == PONumber
                                  select p
                                       ).ToList();

                if (lJobAdvice.Count > 0)
                {
                    for (int i = 0; i < lJobAdvice.Count; i++)
                    {
                        if (lListOrder.Length == 0)
                        {
                            lListOrder = "JobAdvice ID : " + lJobAdvice[i].JobID.ToString();
                        }
                        else
                        {
                            lListOrder = lListOrder + ", " + lJobAdvice[i].JobID.ToString();
                        }
                    }
                }

                //return Json(new { success = true, responseText = lListOrder }, JsonRequestBehavior.AllowGet);
                return Ok();
            }
            catch (Exception ex)
            {
                lErrorMsg = ex.Message;
                //return Json(new { success = false, responseText = lErrorMsg }, JsonRequestBehavior.AllowGet);
                return Ok();
            }

        }

        //get Bars List 
        [HttpGet]
        [Route("/printOrderDetail_bpc/{CustomerCode}/{ProjectCode}/{Template}/{JobID}/{UserName}")]
        //[ValidateAntiForgeryHeader]
        public  ActionResult printOrderDetail(string CustomerCode, string ProjectCode, bool Template, int JobID, string UserName)
        {
            Reports service = new Reports();
            int lPage = 0;
            string lUserType = "";

            if (gUserType != null && gUserType != "")
            {
                lUserType = gUserType;
            }
            else
            {
                UserAccessController lUa = new UserAccessController();
                lUserType = lUa.getUserType(UserName);
                gUserType = lUserType;
                lUa = null;
            }

            if (lUserType == "CA" || lUserType == "CU")
            {
                var bPDF =  service.rptBPCOrderDetails(CustomerCode, ProjectCode, Template, JobID, "N", 0, ref lPage, UserName);

                string fileName = "abc.pdf";

                // Create a MemoryStream from the byte array
                using (MemoryStream memoryStream = new MemoryStream(bPDF))
                {
                    HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                    response.Content = new StreamContent(memoryStream);
                    response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                    response.Content.Headers.ContentDisposition.FileName = fileName;
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

                    return File(response.Content.ReadAsByteArrayAsync().Result, "application/pdf");
                }
            }
            else
            {
                var bPDF =  service.rptBPCOrderDetails(CustomerCode, ProjectCode, Template, JobID, "Y", 0, ref lPage, UserName);

                string fileName = "abc.pdf";

                // Create a MemoryStream from the byte array
                using (MemoryStream memoryStream = new MemoryStream(bPDF))
                {
                    HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                    response.Content = new StreamContent(memoryStream);
                    response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                    response.Content.Headers.ContentDisposition.FileName = fileName;
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

                    return File(response.Content.ReadAsByteArrayAsync().Result, "application/pdf");
                }
            }

        }

        //get Bars List 
        [HttpGet]
        [Route("/printLib_bpc/{CustomerCode}/{ProjectCode}/{Template}/{JobID}/{UserName}")]
        //[ValidateAntiForgeryHeader]
        public ActionResult printLib(string CustomerCode, string ProjectCode, bool Template, int JobID, string UserName)
        {
            Reports service = new Reports();
            int lPage = 0;
            string lUserType = "";

            if (gUserType != null && gUserType != "")
            {
                lUserType = gUserType;
            }
            else
            {
                UserAccessController lUa = new UserAccessController();
                lUserType = lUa.getUserType(User.Identity.Name);
                gUserType = lUserType;
                lUa = null;
            }

            var bPDF = service.rptBPCOrderDetails(CustomerCode, ProjectCode, Template, JobID, "N", 0, ref lPage, UserName);
            //bPDF = service.rptBPCOrderDetails(CustomerCode, ProjectCode, Template, JobID, "N", 0, ref lPage, "Vishalw_ttl@natsteel.com.sg");

            string fileName = "abc.pdf";

            // Create a MemoryStream from the byte array
            using (MemoryStream memoryStream = new MemoryStream(bPDF))
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StreamContent(memoryStream);
                response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                response.Content.Headers.ContentDisposition.FileName = fileName;
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

                return File(response.Content.ReadAsByteArrayAsync().Result, "application/pdf");
            }

            //var result = Convert.ToBase64String(bPDF);
            //string imageDataURL = string.Format("data:application/pdf;base64,{0}", result);

            ////Response.Clear();
            ////Response.AddHeader("Content-Disposition", "inline; filename = BPC_Library.pdf");
            ////Response.AddHeader("Content-Type", "application/pdf");
            //////Response.ClearHeaders();
            ////Response.AddHeader("Content-Length", imageDataURL);

            ////commented by vishal wani

            //ByteArrayToBase64 drawingDetailsBase64 = new ByteArrayToBase64();
            //drawingDetailsBase64.Base64 = imageDataURL;

            //return Ok(drawingDetailsBase64);
        }

        [HttpGet]
        [Route("/GetPDF_bpc/{pHTML}")]
        public byte[] GetPDF(string pHTML)
        {
            byte[] bPDF = null;

            MemoryStream ms = new MemoryStream();
            TextReader txtReader = new StringReader(pHTML);

            // 1: create object of a itextsharp document class  
            Document doc = new Document(PageSize.A4, 25, 25, 25, 25);

            // 2: we create a itextsharp pdfwriter that listens to the document and directs a XML-stream to a file  
            PdfWriter oPdfWriter = PdfWriter.GetInstance(doc, ms);

            // 3: we create a worker parse the document  
            HTMLWorker htmlWorker = new HTMLWorker(doc);

            // 4: we open document and start the worker on the document  
            doc.Open();
            htmlWorker.StartDocument();


            // 5: parse the html into the document  
            htmlWorker.Parse(txtReader);

            // 6: close the document and the worker  
            htmlWorker.EndDocument();
            htmlWorker.Close();
            doc.Close();

            bPDF = ms.ToArray();

            return bPDF;
        }

        //get Bars List 
        [HttpGet]

        [Route("/SaveDrawing_bpc/{CustomerCode}/{ProjectCode}/{JobID}")]
        //[ValidateAntiForgeryHeader]
        public ActionResult SaveDrawing(string CustomerCode, string ProjectCode, int JobID)
        {
            string lReturn = "OK";
            var lProcess = new ProcessController();

            lProcess.SaveBPCDrawing(CustomerCode, ProjectCode, JobID);

            return Ok(lReturn);

        }


        [HttpGet]
        [Route("/Dispose_bpc/disposing")]
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpGet]
        [Route("/checktableforUserEntry/{CustomerCode}/{ProjectCode}/{JobID}/{UserName}")]
        public ActionResult checktableforUserEntry(string CustomerCode, string ProjectCode, int JobID, string UserName)
        {
            var lReturn = "";
            var lSuccess = false;
            var lCmd = new SqlCommand();
            SqlDataReader lRst;
            var lNDSCon = new SqlConnection();
            string lErrorMsg = "";
            string lSQL = "";


            var lProcessObj = new ProcessController();
            if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
            {
                string lUpdatedBy = "";
                lSQL = "SELECT isnull(MIN(UpdateBy), '') " +
                "FROM dbo.OESBPCDetails " +
                "WHERE CustomerCode = '" + CustomerCode + "' " +
                "AND ProjectCode = '" + ProjectCode + "' " +
                "AND JobID = " + JobID.ToString() + " " +
                "AND UpdateDate >= DateAdd(ss,-300,getDate()) " +
                "AND UpdateBy <> '" + UserName + "' ";

                lCmd.Connection = lNDSCon;
                lCmd.CommandText = lSQL;
                lCmd.CommandTimeout = 1200;
                lRst = lCmd.ExecuteReader();
                if (lRst.HasRows)
                {
                    lRst.Read();
                    lUpdatedBy = lRst.GetString(0).Trim();
                }
                lRst.Close();

                lProcessObj.CloseNDSConnection(ref lNDSCon);

                if (lUpdatedBy != "")
                {
                    lSuccess = true;
                    lReturn = "For your informaion, " + lUpdatedBy + " is updating the BPC details now. If you want to change the BPC data, please contact him/her for the updating status.";
                }
            }
            return Json(new { Success = lSuccess, Message = lReturn });

        }

        #region BPCJobAdvice --BY Vidhya

        [HttpGet]

        [Route("/GetBPCJobAdviceDetails/{CustomerCode}/{ProjectCode}/{JobId}")]

        public async Task<IActionResult> GetBPCJobAdviceDetails(string CustomerCode, string ProjectCode, int JobId)

        {

            string errorMessage = "";

            var result = precastService.GetBPCJobAdviceDetails(CustomerCode, ProjectCode, JobId);

            return Ok(result);

        }

        [HttpPost]

        [Route("/InsertBPCJobAdviceDetails/{userId}")]

        public async Task<IActionResult> InsertBPCJobAdviceDetails([FromBody] BPCJobAdviceInsertDto jobAdviceInsertDto, int UserId)

        {

            string errorMessage = "";

            var result = precastService.InsertBPCJobAdviceDetails(jobAdviceInsertDto, UserId);

            return Ok(result);

        }

        [HttpPost]

        [Route("/UpdateBPCJobAdviceDetails/{UserId}")]

        public async Task<IActionResult> UpdateBPCJobAdviceDetails([FromBody] BPCJobAdviceInsertDto getPrecastDto, int UserId)

        {

            string errorMessage = "";

            var result = precastService.UpdateBPCJobAdviceDetails(getPrecastDto, UserId);

            return Ok(result);

        }

        [HttpDelete]

        [Route("/DeleteBPCJobAdviceDetails/{BpcJobID}")]

        public IActionResult DeleteBPCJobAdviceDetails(int BpcJobID)

        {

            try

            {

                PrecastService precastService = new PrecastService();

                bool isDeleted = precastService.DeleteBPCJobAdviceDetails(BpcJobID);

                if (isDeleted)

                {

                    return Ok(isDeleted);

                }

                else

                {

                    return NotFound("No record found with the specified Precast_ID.");

                }

            }

            catch (Exception ex)

            {

                return BadRequest();

            }

        }


        #endregion

        [HttpGet("GetIsCABEdit")]

        public IActionResult GetIsCABEdit(string customerCode, string projectCode, bool template, int jobId, int cageId)

        {

            try

            {

                //var result = db.BPCCageBars

                //    .Where(bar => bar.CustomerCode == customerCode &&

                //                  bar.ProjectCode == projectCode &&

                //                  bar.Template == template &&

                //                  bar.JobID == jobId &&

                //                  bar.CageID == cageId)

                //    .Select(bar => bar.isCABEdit) // 

                //    .ToList();

                var result = db.BPCCageBars

                 .Where(bar => bar.CustomerCode == customerCode &&

                               bar.ProjectCode == projectCode &&

                               bar.Template == template &&

                               bar.JobID == jobId &&

                               bar.CageID == cageId)

                 .Count(bar => (bool)bar.isCABEdit) > 0 ? "TRUE" : "FALSE";

                if (result.Any())

                {

                    if (result == "TRUE")

                    {

                        return Ok(true); // Return the list of values

                    }

                    else

                    {

                        return Ok(false);

                    }

                }

                else

                {

                    return NotFound("No data found.");

                }

            }

            catch (Exception ex)

            {

                return StatusCode(500, $"Internal server error: {ex.Message}");

            }

        }

        [HttpPost]
        [Route("/updateMainBar_Customization")]
        public ActionResult updateMainBar_Customization([FromBody] updateCustomizationDto customizationDto)
        {
            if (customizationDto == null)
            {
                return BadRequest("Invalid data.");
            }
            try
            {
                var record = db.BPCDetails

                    .Where(o => o.CustomerCode == customizationDto.CustomerCode
                           && o.ProjectCode == customizationDto.ProjectCode
                           && o.Template == customizationDto.Template
                           && o.JobID == customizationDto.JobId
                           && o.cage_id == customizationDto.CageId)
                    .FirstOrDefault();
                // If record is found, update it
                if (record != null)
                {
                    record.vchCustomizeBarsJSON = customizationDto.CustomizeBarsJSON;
                    record.main_bar_ct = customizationDto.main_bar_ct;
                    // Save changes to the database
                    db.SaveChanges();
                    return Ok(new { Success = true, Message = "OESBPCDetails updated successfully." });
                }
                else
                {
                    return NotFound(new { Success = false, Message = "Record not found." });
                }
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = "Error: " + ex.Message });
            }
        }

        [HttpPost]
        [Route("/updateStiffenerRing")] 
        public ActionResult UpdateStiffenerRing([FromBody] StiffenerRingDto stiffenerRing)
        {
            var lSuccess = false; //Added
            var lReturn = "";
            var lCmd = new SqlCommand();
            SqlDataReader lRst;
            var lNDSCon = new SqlConnection();
            string lErrorMsg = "";
            string lSQL = "";
            try
            {
                // Open database connection
                var lProcessObj = new ProcessController();
                if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
                {
                    // Prepare SQL to execute the stored procedure
                    lCmd.Connection = lNDSCon;
                    lCmd.CommandText = "usp_UpdateStiffenerRing";
                    lCmd.CommandType = CommandType.StoredProcedure;
                    // Add parameters for stored procedure
                    lCmd.Parameters.AddWithValue("@CustomerCode", stiffenerRing.CustomerCode);
                    lCmd.Parameters.AddWithValue("@ProjectCode", stiffenerRing.ProjectCode);
                    lCmd.Parameters.AddWithValue("@Template", stiffenerRing.Template);
                    lCmd.Parameters.AddWithValue("@JobId", stiffenerRing.JobId);
                    lCmd.Parameters.AddWithValue("@CageId", stiffenerRing.CageId);
                    lCmd.Parameters.AddWithValue("@sr1_location", stiffenerRing.Sr1Location);
                    lCmd.Parameters.AddWithValue("@sr2_location", stiffenerRing.Sr2Location);
                    lCmd.Parameters.AddWithValue("@sr3_location", stiffenerRing.Sr3Location);
                    lCmd.Parameters.AddWithValue("@sr4_location", stiffenerRing.Sr4Location);
                    lCmd.Parameters.AddWithValue("@sr5_location", stiffenerRing.Sr5Location);
                    //lCmd.Parameters.AddWithValue("@no_of_sr", stiffenerRing.NoOfSr);
                    lCmd.Parameters.AddWithValue("@lminTop", stiffenerRing.LminTop);
                    lCmd.Parameters.AddWithValue("@lminEnd", stiffenerRing.LminEnd);
                    lCmd.Parameters.AddWithValue("@rings_start", stiffenerRing.rings_start);
                    lCmd.Parameters.AddWithValue("@no_of_sr", stiffenerRing.no_of_sr);
                    var rowsAffected = lCmd.ExecuteNonQuery(); // Executes the stored procedure
                    lProcessObj.CloseNDSConnection(ref lNDSCon);

                    if (rowsAffected > 0)
                    {
                        lSuccess = true;
                        lReturn = "Stiffener Ring updated successfully.";
                    }
                    else
                    {
                        lSuccess = false;
                        lReturn = "No records were updated.";
                    }
                }
            }
            catch (Exception ex)
            {
                lSuccess = false;
                lReturn = "Error: " + ex.Message;
            }
            return Json(new { Success = lSuccess, Message = lReturn });
        }

        [HttpPost]
        [Route("/UploadBPCImage")]
        public async Task<IActionResult> UploadBPCImage([FromForm] UploadImageDto uploadImageDto)
        {
            // Convert file to bytes
            try
            {
                
                using var memoryStream = new MemoryStream();
                await uploadImageDto.PlanView.CopyToAsync(memoryStream);
                byte[] imagePlanView = memoryStream.ToArray();
                using var memoryStream_New = new MemoryStream();

                await uploadImageDto.ElevatedView.CopyToAsync(memoryStream_New);
                byte[] imagePElevatedView = memoryStream_New.ToArray();


                // Find the record you want to attach the image to
                var record = await db.BPCDetails
                    .FirstOrDefaultAsync(r => r.CustomerCode == uploadImageDto.CustomerCode
                                           && r.ProjectCode == uploadImageDto.ProjectCode
                                           && r.JobID == uploadImageDto.JobID
                                           && r.cage_id == uploadImageDto.CageID
                                           && r.Template == uploadImageDto.Template);

                if (record == null)
                    return BadRequest(Json(new { Success = false, textMessage = "Record Not Found" }));

                // Save image bytes to VARBINARY(MAX) column
                record.ElevatedView = imagePElevatedView;
                record.PlanView = imagePlanView;
                await db.SaveChangesAsync();

                return Ok(Json(new { Success = true, textMessage = "Image Uploaded Successfully" }));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); ;
            }
        }

        [HttpGet]
        [Route("SaveBPCWithJobID")]
        public async Task<IActionResult> SaveBPCWithJobID(string customerCode ,string ProjectCode,int DestJobID ,int DestCageID, string SetCode)
        {
            try
            {
                var record = await db.BPCDetails.
                    FirstOrDefaultAsync(b => b.CustomerCode == customerCode &&
                    b.ProjectCode == ProjectCode &&
                    b.set_code == SetCode &&
                    b.Template == true
                    );

                if (record == null)
                    return BadRequest(Json(new { success = false, responseText = "No Record Found" }));
                if(record.ElevatedView==null || record.PlanView==null)
                    return BadRequest(Json(new { success = false, responseText = "No Image Found" }));


                byte[] ElevatedView = record.ElevatedView;
                byte[] PlanView = record.PlanView;

                var recordtoSave = await db.BPCDetails.
                 FirstOrDefaultAsync(b => b.CustomerCode == customerCode &&
                 b.ProjectCode == ProjectCode &&
                 b.cage_id == DestCageID &&
                 b.JobID ==DestJobID &&
                 b.Template == false
                 );
                recordtoSave.ElevatedView = ElevatedView;
                recordtoSave.PlanView = PlanView;
                await db.SaveChangesAsync();

                return Ok(Json(new { success = true, responseText = "Images Updated Successfully" }));


            }
            catch (Exception ex)
            {
                return BadRequest(Json(new { success = false, responseText = $"Images no Updated Successfully because  {ex.Message}" }));

            }
        }




    }
}
