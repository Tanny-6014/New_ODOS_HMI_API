using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
//using System.Web;
//using System.Web.Mvc;
using OrderService.Models;
//using AntiForgeryHeader.Helper;
using Microsoft.AspNet.Identity;
using System.Data.SqlClient;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.IO;
using OrderService.Repositories;
using System.Globalization;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using NCalc;
using System.Text;
using Oracle.ManagedDataAccess.Client;
using System.ServiceModel;
//using System.ServiceModel.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OrderService.Dtos;
using System.Net.Http.Headers;

namespace OrderService.Controllers
{
 //   [Authorize]
    public class CouplerHeadController : Controller
    {
        public string gUserType = "";
        public string gGroupName = "";

        struct struOrderList
        {
            public string OrderNo;
            public string OrderDesc;
        };

        private DBContextModels db = new DBContextModels();

        //[ValidateAntiForgeryToken]
        [HttpGet]
        [Route("/Index_couler/{appCustomerCode}/{appProjectCode}/{Username}/{appProductType}/{appStructureElement}/{appScheduledProd}/{appWBS1}/{appWBS2}/{appWBS3}/{appOrderNo}/{PostID}")]
        public ActionResult Index(string appCustomerCode, string appProjectCode, string Username, string appProductType, string appStructureElement, string appScheduledProd, string appWBS1, string appWBS2, string appWBS3, string appOrderNo, int PostID)
        {
            UserAccessController lUa = new UserAccessController();
            var lUserType = lUa.getUserType(User.Identity.GetUserName());
            var lGroupName = lUa.getGroupName(User.Identity.GetUserName());

//            ViewBag.UserType = lUserType;

            string lUserName = User.Identity.GetUserName();

            //if (lUserName.IndexOf("@") > 0)
            //{
            //    lUserName = lUserName.Substring(0, lUserName.IndexOf("@"));
            //}

            //ViewBag.UserName = lUserName;

            var lReturn = (new[]{ new
            {
                SSNNo = 0,
                PostHeaderID = 0,
                WBS1 = "",
                WBS2 = "",
                WBS3 = "",
                //WBS1T="",
                //WBS2T="",
                //WBS3T="",
                //BBSDesc = "",
                StructEle = "",
                ProductType = "",
                TotalPCs = "",
                TotalWeight = "",
                //PostedBy = "",
                //PostedDate = "",
                Status = "",
                SelectedCount=0,
                //SelectedSE="",
                //StandbyOrder="",
                OrderNo="",
                lsubmission="",
                leditable="",
                JobID=0

            }}).ToList();

            lReturn.RemoveAt(0);


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

            int lOrderNoT = 0;
            int.TryParse(appOrderNo, out lOrderNoT);

            var lMain = db.OrderProject.Find(lOrderNoT);

            if (lMain != null)
            {
                lOrderStatus = lMain.OrderStatus;
            }
            if (lOrderStatus == null)
            {
                lOrderStatus = "New";
            }
            if (lUserName.Split('@').Length == 2)
            {
                if (lOrderStatus == "Created*" && lUserName.Split('@')[1].ToLower().Trim() == "natsteel.com.sg")
                {
                    lOrderStatus = "Created";
                }
            }

            var lSE = db.OrderProjectSE.Find(lOrderNoT, appStructureElement, appProductType, appScheduledProd);
            if (lSE != null)
            {
                lJobID = lSE.CoilProdJobID;

                lOrderStatus = lSE.OrderStatus;
                if (lOrderStatus == null)
                {
                    lOrderStatus = "New";
                }
                if (lUserName.Split('@').Length == 2)
                {
                    if (lOrderStatus == "Created*" && lUserName.Split('@')[1].ToLower().Trim() == "natsteel.com.sg")
                    {
                        lOrderStatus = "Created";
                    }
                }
            }

            var lJobAdv = db.StdSheetJobAdvice.Find(appCustomerCode, appProjectCode, lJobID);


            var lProjectDetail = db.Project.Find(appCustomerCode, appProjectCode);

            //ViewBag.JobID = lJobID;

            //ViewBag.OrderStatus = lOrderStatus;

            //ViewBag.MaxBarLength = lProjectDetail.MaxBarLength;

            //ViewBag.SelectedCount = appSelectedCount;
            //ViewBag.SelectedSE = appSelectedSE.Split(',');
            //ViewBag.SelectedProd = appSelectedProd.Split(',');
            //ViewBag.SelectedPostID = appSelectedPostID.Split(',');
            //ViewBag.SelectedScheduled = appSelectedScheduled.Split(',');

            //ViewBag.SelectedWT = appSelectedWT == null ? new string[] { } : appSelectedWT.Split(',');
            //ViewBag.SelectedQty = appSelectedQty == null ? new string[] { } : appSelectedQty.Split(',');

            //ViewBag.SelectedWBS1 = appSelectedWBS1.Split(',');
            //ViewBag.SelectedWBS2 = appSelectedWBS2.Split(',');
            //ViewBag.SelectedWBS3 = appSelectedWBS3.Split(',');

            //ViewBag.WBS1 = appWBS1S.Split(',');
            //ViewBag.WBS2 = appWBS2S.Split(',');
            //ViewBag.WBS3 = appWBS3S.Split(',');

            //ViewBag.WBS1T = appWBS1;
            //ViewBag.WBS2T = appWBS2;
            //ViewBag.WBS3T = appWBS3;
            //ViewBag.OrderNo = appOrderNoS.Split(',');

            //ViewBag.StandbyOrder = appScheduledProd;
            //ViewBag.PostID = appPostID;

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

            //ViewBag.Submission = lSubmission;
            //ViewBag.Editable = lEditable;

            //ViewBag.AlertMessage = new List<string>();
            //var lSharedPrg = new SharedAPIController();
            //ViewBag.AlertMessage = lSharedPrg.getAlertMessage(appCustomerCode, appProjectCode, lUserName, lSubmission, lEditable);
            //lSharedPrg = null;

            lReturn.Add(new
            {
                SSNNo = 0,
                PostHeaderID = PostID,
                WBS1 = appWBS1,
                WBS2 = appWBS2,
                WBS3 = appWBS3,
                //WBS1T = "",
                //WBS2T = "",
                //WBS3T = "",
                //BBSDesc = "",
                StructEle = appStructureElement,
                ProductType = appProductType,
                TotalPCs = lSE.TotalWeight.ToString(),
                TotalWeight = lSE.TotalWeight.ToString(),
                //PostedBy = "",
                //PostedDate = "",
                Status = lOrderStatus,
                SelectedCount = 0,
                //SelectedSE = "",
                //StandbyOrder = "",
                OrderNo = appOrderNo,
                lsubmission = lSubmission,
                leditable = lEditable,
                JobID = lJobID

            });



            return Ok(lReturn);
        }

        [HttpGet]
        [Route("/getStandardProducts_coupler/{ProdCategory}")]
        //[ValidateAntiForgeryHeader]
        public ActionResult getStandardProducts(string ProdCategory)
        {
            var lReturn = new List<StdProductsModels>();

            try
            {
                lReturn = (from p in db.StdProducts
                           where p.ProdType == ProdCategory
                           orderby p.Grade, p.ProdCode
                           select p
                    ).ToList();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Server Error: " + ex.Message);
            }
            return Ok(lReturn);
        }

        //[HttpPost]
        ////[ValidateAntiForgeryHeader]
        //[Route("/checkOrderStatus_coupler/{CustomerCode}/{ProjectCode}/{JobID}")]
        //public ActionResult checkOrderStatus(string CustomerCode, string ProjectCode, int JobID)
        //{

        //    var lCheckStdDet = (from p in db.StdProdDetails
        //                        where p.CustomerCode == CustomerCode &&
        //                        p.ProjectCode == ProjectCode &&
        //                        p.JobID == JobID &&
        //                        p.order_pcs > 0 &&
        //                        p.UpdateBy != User.Identity.Name
        //                        select p).ToList();

        //    var lCheckJob = db.StdSheetJobAdvice.Find(CustomerCode, ProjectCode, JobID);
        //    if (lCheckJob == null || (lCheckJob.OrderStatus != "New" && lCheckJob.OrderStatus != "Created") ||
        //        lCheckStdDet.Count > 0 ||
        //        ((lCheckJob.TotalPcs > 0 ||
        //        (lCheckJob.PONumber != null && lCheckJob.PONumber != "")) &&
        //        lCheckJob.UpdateBy != User.Identity.GetUserName()))
        //    {
        //        string lUserNameCur = User.Identity.GetUserName();
        //        string lSubmissionCur = "No";
        //        string lEditableCur = "No";
        //        string lUserTypeCur = "PL";

        //        string lUserNamePre = lCheckJob.UpdateBy;
        //        string lSubmissionPre = "No";
        //        string lEditablePre = "No";
        //        string lUserTypePre = "PL";

        //        var lAccess = db.UserAccess.Find(lUserNameCur, "0000000000", "0000000000");
        //        if (lAccess != null)
        //        {
        //            lUserTypeCur = (lAccess.UserType == null ? "CU" : lAccess.UserType.Trim());
        //        }

        //        lAccess = db.UserAccess.Find(lUserNamePre, "0000000000", "0000000000");
        //        if (lAccess != null)
        //        {
        //            lUserTypePre = (lAccess.UserType == null ? "CU" : lAccess.UserType.Trim());
        //        }

        //        lAccess = db.UserAccess.Find(lUserNameCur, CustomerCode, ProjectCode);
        //        if (lAccess != null)
        //        {
        //            lSubmissionCur = (lAccess.OrderSubmission == null ? "No" : lAccess.OrderSubmission.Trim());

        //            lEditableCur = (lAccess.OrderCreation == null ? "No" : lAccess.OrderCreation.Trim());
        //        }
        //        lAccess = db.UserAccess.Find(lUserNamePre, CustomerCode, ProjectCode);
        //        if (lAccess != null)
        //        {
        //            lSubmissionPre = (lAccess.OrderSubmission == null ? "No" : lAccess.OrderSubmission.Trim());

        //            lEditablePre = (lAccess.OrderCreation == null ? "No" : lAccess.OrderCreation.Trim());
        //        }

        //        if ((lUserTypeCur == "CU" || lUserTypeCur == "CM" || lUserTypeCur == "CA") &&
        //        ((lUserTypePre == "CU" || lUserTypePre == "CM" || lUserTypePre == "CA") &&
        //        (lSubmissionPre != "Yes" && lSubmissionCur == "Yes") ||
        //        lEditableCur != "Yes" || lEditablePre != "Yes"))
        //        {
        //            //return Json(new { success = true, error = false, message = lCheckJob.UpdateBy }, JsonRequestBehavior.AllowGet);
        //            return Ok();
        //        }
        //        else
        //        {
        //            return Json(new
        //            {
        //                success = false,
        //                error = true,
        //                message = "The order has been processed by another person. Please click <New Order> button to create a new order."
        //            }, System.Web.Mvc.JsonRequestBehavior.AllowGet);
        //        }

        //    }
        //    else
        //    {
        //        //return Json(new { success = true, error = false, message = lCheckJob.UpdateBy }, JsonRequestBehavior.AllowGet);
        //        return Ok();
        //    }
        //}

        [HttpPost]
        // [ValidateAntiForgeryHeader]
        [Route("/uploadPODocs_coupler")]
        public ActionResult uploadPODocs()
        {
            var lReturn = new CTSMESHPODocsFileModels();

            try
            {
                //var lCustomerCode = Request.Form.Get("CustomerCode");
                //var lProjectCode = Request.Form.Get("ProjectCode");
                //int lJobID = 0;
                //int lDocID = 0;
                //int.TryParse(Request.Form.Get("JobID"), out lJobID);

                //if (Request.Files.Count > 0)
                //{
                //    HttpFileCollectionBase files = Request.Files;
                //    for (int i = 0; i < files.Count; i++)
                //    {
                //        HttpPostedFileBase file = files[i];

                //        var lFileName = file.FileName;
                //        if (lFileName.LastIndexOf("\\") >= 0)
                //        {
                //            lFileName = lFileName.Substring(lFileName.LastIndexOf("\\") + 1);
                //        }
                //        if (lFileName.LastIndexOf("/") >= 0)
                //        {
                //            lFileName = lFileName.Substring(lFileName.LastIndexOf("/") + 1);
                //        }

                //        byte[] pdfBytes = null;
                //        BinaryReader reader = new BinaryReader(file.InputStream);
                //        pdfBytes = reader.ReadBytes((int)file.ContentLength);

                //        lDocID = (from p in db.StdSheetPODoc
                //                  where p.CustomerCode == lCustomerCode &&
                //                  p.ProjectCode == lProjectCode &&
                //                  p.JobID == lJobID
                //                  select p.PODocID).DefaultIfEmpty(0).Max();
                //        lDocID = lDocID + 1;

                //        var Content = new StdSheetPODocsModels
                //        {
                //            CustomerCode = lCustomerCode,
                //            ProjectCode = lProjectCode,
                //            JobID = lJobID,
                //            PODocID = lDocID,
                //            FileName = lFileName,
                //            UpdatedDate = DateTime.Now,
                //            UpdatedBy = User.Identity.Name,
                //            PODoc = pdfBytes
                //        };
                //        db.StdSheetPODoc.Add(Content);
                //        db.SaveChanges();

                //        var lPODocExists = db.StdSheetPODoc.Where(
                //                              p => p.CustomerCode == lCustomerCode &&
                //                              p.ProjectCode == lProjectCode &&
                //                              p.JobID == lJobID).Count();

                //        lReturn = new CTSMESHPODocsFileModels
                //        {
                //            PODocID = lDocID,
                //            FileName = lFileName,
                //            UpdatedDate = DateTime.Now.ToString("yyyy-mm-dd hh:mm:ss"),
                //            UpdatedBy = User.Identity.Name,
                //            FileSize = (pdfBytes.Length / 1000).ToString(),
                //            Exists = lPODocExists
                //        };
                //    }
                //}
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Server Error: " + ex.Message);
            }
            return Ok(lReturn);
        }

        //download PO doc 
        [HttpPost]
        // [ValidateAntiForgeryHeader]
        [Route("/deletePODocs_coupler/{CustomerCode}/{ProjectCode}/{JobID}/{PODocID}")]
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
                    "DELETE FROM dbo.OESStdSheetPODoc " +
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
        [HttpPost]
        [Route("/downloadPODocs_coupler/{CustomerCode}/{ProjectCode}/{JobID}/{PODocID}")]
        // [ValidateAntiForgeryHeader]
        public ActionResult downloadPODocs(string CustomerCode, string ProjectCode, int JobID, int PODocID)
        {
            var content = db.StdSheetPODoc.Find(CustomerCode, ProjectCode, JobID, PODocID);
            //return Json(content, JsonRequestBehavior.AllowGet);
            return Ok(content);
        }

        //check PO doc 
        [HttpPost]
        [Route("/checkPODocs_coupler/{CustomerCode}/{ProjectCode}/{JobID}")]
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
                    "FROM dbo.OESStdSheetPODoc " +
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

        [HttpPost]
        // [ValidateAntiForgeryHeader]
        [Route("/sendOrderSubmittedEmail_coupler/{CustomerCode}/{ProjectCode}/{JobID}")]
        public ActionResult sendOrderSubmittedEmail(string CustomerCode, string ProjectCode, int JobID)
        {
            var lEmailContent = "";
            var lEmailFrom = "";
            var lEmailTo = "";
            var lEmailCc = "";
            var lEmailSubject = "";
            string lVar1 = "";

            var lCoilPrd = (from p in db.StdProdDetails
                            where p.CustomerCode == CustomerCode &&
                            p.ProjectCode == ProjectCode &&
                            p.JobID == JobID
                            orderby p.SSID
                            select p).ToList();

            if ((CustomerCode == "0001101449" || CustomerCode == "0001101431") && lCoilPrd.Count > 0)
            {
                return Json(true);
            }


            var JobContent = db.StdSheetJobAdvice.Find(CustomerCode, ProjectCode, JobID);
            if (JobContent != null)
            {
                if (JobContent.CustomerCode == null) JobContent.CustomerCode = "";
                else JobContent.CustomerCode = JobContent.CustomerCode.Trim();

                if (JobContent.OrderStatus == null) JobContent.OrderStatus = "";
                else JobContent.OrderStatus = JobContent.OrderStatus.Trim();

                if (JobContent.PONumber == null) JobContent.PONumber = "";
                else JobContent.PONumber = JobContent.PONumber.Trim();

                if (JobContent.ProjectCode == null) JobContent.ProjectCode = "";
                else JobContent.ProjectCode = JobContent.ProjectCode.Trim();

                if (JobContent.DeliveryAddress == null) JobContent.DeliveryAddress = "";
                else JobContent.DeliveryAddress = JobContent.DeliveryAddress.Trim();

                if (JobContent.Remarks == null) JobContent.Remarks = "";
                else JobContent.Remarks = JobContent.Remarks.Trim();

                if (JobContent.Scheduler_HP == null) JobContent.Scheduler_HP = "";
                else JobContent.Scheduler_HP = JobContent.Scheduler_HP.Trim();

                if (JobContent.Scheduler_Name == null) JobContent.Scheduler_Name = "";
                else JobContent.Scheduler_Name = JobContent.Scheduler_Name.Trim();

                if (JobContent.Scheduler_Tel == null) JobContent.Scheduler_Tel = "";
                else JobContent.Scheduler_Tel = JobContent.Scheduler_Tel.Trim();

                if (JobContent.SiteEngr_HP == null) JobContent.SiteEngr_HP = "";
                else JobContent.SiteEngr_HP = JobContent.SiteEngr_HP.Trim();

                if (JobContent.SiteEngr_Name == null) JobContent.SiteEngr_Name = "";
                else JobContent.SiteEngr_Name = JobContent.SiteEngr_Name.Trim();

                if (JobContent.SiteEngr_Tel == null) JobContent.SiteEngr_Tel = "";
                else JobContent.SiteEngr_Tel = JobContent.SiteEngr_Tel.Trim();
            }

            lEmailContent = "<p align='center'>JOB ADVICE - Standard Products (工作通知 - 标准产品)</p>";

            lEmailContent = lEmailContent + "<p align='left' style='font-size:15px'><a target='_top' href='https://oes.natsteel.com.sg/process?ccode=" + JobContent.CustomerCode
                + "&jcode=" + JobContent.ProjectCode + "&prodtype=Standard%20MESH" + "&jobid=" + JobContent.JobID.ToString()
                + "'>Click here to redirect to Digital Ordering System to process it</a></p>";

            lEmailContent = lEmailContent + "<table border='1' width=100%><tr>";
            lEmailContent = lEmailContent + "<td width=20%>" + "Customer (客户名称)" + "</td>";

            CustomerModels lCustomer = db.Customer.Find(JobContent.CustomerCode);
            string lVar = "";
            if (lCustomer != null) lVar = lCustomer.CustomerName.Trim() + " (" + JobContent.CustomerCode.Trim() + ")";
            lEmailContent = lEmailContent + "<td>" + lVar + "</td></tr>";

            lEmailContent = lEmailContent + "<tr><td>" + "Project (工程项目)" + "</td>";

            var lProject = (from p in db.ProjectList
                            where p.ProjectCode == ProjectCode
                            select p).First();
            lVar = "";
            if (lProject != null) lVar = lProject.ProjectTitle.Trim() + " (" + JobContent.ProjectCode.Trim() + ")";
            lEmailContent = lEmailContent + "<td>" + lVar + "</td></tr></table>";

            lEmailContent = lEmailContent + "<table border='1' width=100%><tr>";

            lEmailContent = lEmailContent + "<td width=20%>" + "PO No. (订单号码)" + "</td>";
            lEmailContent = lEmailContent + "<td width=27%>" + JobContent.PONumber.Trim() + "</td>";
            lEmailContent = lEmailContent + "<td width=26%>" + "Order Date (订单日期)" + "</td>";
            lEmailContent = lEmailContent + "<td>" + String.Format("{0:yyyy-MM-dd}", JobContent.PODate) + "</td></tr>";

            lEmailContent = lEmailContent + "</tr><td width=20%>" + "Required Date (交货日期)" + "</td>";
            lEmailContent = lEmailContent + "<td width=27%>" + String.Format("{0:yyyy-MM-dd}", JobContent.RequiredDate) + "</td>";

            lEmailContent = lEmailContent + "<td width=26%>" + "Transport Mode (运输工具)" + "</td>";

            lVar = "";
            var lProcessObj = new ProcessController();

            //var lCmd = new OracleCommand();
            //OracleDataReader lRst;
            //var lcisCon = new OracleConnection();

            //if (lProcessObj.OpenCISConnection(ref lcisCon) == true)
            //{

            //    lCmd.CommandText = "SELECT BEZEI as DESCRIPTION " +
            //    "FROM SAPSR3.TMFGT WHERE MANDT = '" + lProcessObj.strClient + "' AND SPRAS ='E' " +
            //    "AND MFRGR = '" + JobContent.Transport + "' ";

            //    lCmd.Connection = lcisCon;
            //    lCmd.CommandTimeout = 300;
            //    lRst = lCmd.ExecuteReader();
            //    if (lRst.HasRows)
            //    {
            //        if (lRst.Read())
            //        {
            //            lVar = lRst.GetString(0).Trim();
            //            lProcessObj.CloseCISConnection(ref lcisCon);
            //        }
            //    }
            //    lRst.Close();
            //}
            //lCmd = null;
            //lcisCon = null;
            //lRst = null;
            //lProcessObj = null;

            var lCmd = new SqlCommand();
            SqlDataReader lRst;
            var lNDSCon = new SqlConnection();

            if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
            {

                lCmd.CommandText = "select vchTransportDescription " +
                "from dbo.TransportMaster " +
                "where vchTransportMode = '" + JobContent.Transport + "' ";

                lCmd.Connection = lNDSCon;
                lCmd.CommandTimeout = 300;
                lRst = lCmd.ExecuteReader();
                if (lRst.HasRows)
                {
                    if (lRst.Read())
                    {
                        lVar = lRst.GetString(0).Trim();
                        lProcessObj.CloseNDSConnection(ref lNDSCon);
                    }
                }
                lRst.Close();
            }
            lCmd = null;
            lNDSCon = null;
            lRst = null;


            lEmailContent = lEmailContent + "<td>" + lVar + "</td></tr>";

            lEmailContent = lEmailContent + "</tr><td width=20%>" + "Total Pieces (总件数)" + "</td>";
            lEmailContent = lEmailContent + "<td width=27%>" + ((int)JobContent.TotalPcs).ToString() + "</td>";

            lEmailContent = lEmailContent + "<td width=26%>" + "Total Weight (总重量)" + "</td>";

            lEmailContent = lEmailContent + "<td>" + ((decimal)JobContent.TotalWeight).ToString("F3") + " KG" + "</td></tr></table>";

            lEmailContent = lEmailContent + "<table border='1' width=100%><tr>";
            lEmailContent = lEmailContent + "<td width=20%>Delivery Address(送货地址)</td>";

            lVar = "&nbsp;";
            if (JobContent.DeliveryAddress != null) lVar = JobContent.DeliveryAddress.Trim();
            lEmailContent = lEmailContent + "<td>" + lVar + "</td></tr>";

            lEmailContent = lEmailContent + "<tr><td>" + "Remarks (备注)" + "</td>";

            lVar = "&nbsp;";
            if (JobContent.DeliveryAddress != null) lVar = JobContent.DeliveryAddress.Trim();
            lEmailContent = lEmailContent + "<td>" + lVar + "</td></tr></table>";

            var lBBSContent = (from p in db.StdSheetDetails
                               where p.CustomerCode == CustomerCode &&
                               p.ProjectCode == ProjectCode &&
                               p.JobID == JobID
                               orderby p.SheetSort
                               select p).ToList();

            if (lBBSContent.Count > 0)
            {
                lEmailContent = lEmailContent + "<table border='1' width=100%><tr>";
                lEmailContent = lEmailContent + "<td colspan='11'>" + "MESH Standard Sheet (Class A)(标准铁网)" + "</td></tr><tr>";
                lEmailContent = lEmailContent + "<td width='5%'>" + "S/N<br/>序号" + "</td>";
                lEmailContent = lEmailContent + "<td width='10%'>" + "Product Code<br/>产品代码" + "</td>";
                lEmailContent = lEmailContent + "<td width='8%'>" + "Main Length<br/>主筋长" + "</td>";
                lEmailContent = lEmailContent + "<td width='8%'>" + "Cross Length<br/>副筋长" + "</td>";
                lEmailContent = lEmailContent + "<td width='9%'>" + "MW Size<br/>主筋直径" + "</td>";
                lEmailContent = lEmailContent + "<td width='10%'>" + "MW Spacing<br/>主筋间距" + "</td>";
                lEmailContent = lEmailContent + "<td width='9%'>" + "CW Size<br/>副筋直径" + "</td>";
                lEmailContent = lEmailContent + "<td width='10%'>" + "CW Spacing<br/>副筋间距" + "</td>";
                lEmailContent = lEmailContent + "<td width='10%'>" + "Unit Weight<br/>单片重量" + "</td>";
                lEmailContent = lEmailContent + "<td width='10%'>" + "Order Qty<br/>订购件数" + "</td>";
                lEmailContent = lEmailContent + "<td>" + "Total Weight<br/>总重量" + "</td>";
                lEmailContent = lEmailContent + "</td></tr>";


                for (int i = 0; i < lBBSContent.Count; i++)
                {
                    lEmailContent = lEmailContent + "<tr><td align='center'> <font color='blue'>" + (i + 1).ToString() + "</font></td>";
                    lEmailContent = lEmailContent + "<td align='center'><font color='blue'>" + lBBSContent[i].sheet_name + "</font></td>";
                    lEmailContent = lEmailContent + "<td align='center'><font color='blue'>" + lBBSContent[i].mw_length.ToString() + "</font></td>";
                    lEmailContent = lEmailContent + "<td align='center'><font color='blue'>" + lBBSContent[i].cw_length.ToString() + "</font></td>";
                    lEmailContent = lEmailContent + "<td align='center'><font color='blue'>" + lBBSContent[i].mw_size.ToString() + "</font></td>";
                    lEmailContent = lEmailContent + "<td align='center'><font color='blue'>" + lBBSContent[i].mw_spacing.ToString() + "</font></td>";
                    lEmailContent = lEmailContent + "<td align='center'><font color='blue'>" + lBBSContent[i].cw_size.ToString() + "</font></td>";
                    lEmailContent = lEmailContent + "<td align='center'><font color='blue'>" + lBBSContent[i].cw_spacing.ToString() + "</font></td>";
                    lEmailContent = lEmailContent + "<td align='right'><font color='blue'>" + lBBSContent[i].unit_weight.ToString("F3") + "</font></td>";
                    if (lBBSContent[i].order_pcs == 0) lVar = ""; else lVar = lBBSContent[i].order_pcs.ToString();
                    lEmailContent = lEmailContent + "<td align='center'><font color='blue'><strong>" + lVar + "</strong></font></td>";
                    if (lBBSContent[i].order_wt == 0) lVar = ""; else lVar = lBBSContent[i].order_wt.ToString("F3");
                    lEmailContent = lEmailContent + "<td align='right'><font color='blue'>" + lVar + "</font></td>";
                    lEmailContent = lEmailContent + "</tr>";
                }
                lEmailContent = lEmailContent + "</table>";
            }

            var lStdContent = (from p in db.StdProdDetails
                               where p.CustomerCode == CustomerCode &&
                               p.ProjectCode == ProjectCode &&
                               p.JobID == JobID
                               orderby p.SSID
                               select p).ToList();

            if (lStdContent.Count > 0)
            {
                lEmailContent = lEmailContent + "<table border='1' width=100%><tr>";
                lEmailContent = lEmailContent + "<td colspan='11'>" + "Standard Products (标准产品)" + "</td></tr><tr>";
                lEmailContent = lEmailContent + "<td width='5%'>" + "S/N<br/>序号" + "</td>";
                lEmailContent = lEmailContent + "<td width='15%'>" + "Product Code<br/>产品代码" + "</td>";
                lEmailContent = lEmailContent + "<td width='30%'>" + "Product Description<br/>产品概述" + "</td>";
                lEmailContent = lEmailContent + "<td width='10%'>" + "Diameter<br/>直径" + "</td>";
                lEmailContent = lEmailContent + "<td width='10%'>" + "Grade<br/>类型" + "</td>";
                lEmailContent = lEmailContent + "<td width='10%'>" + "Unit Weight<br/>单位重量" + "</td>";
                lEmailContent = lEmailContent + "<td width='10%'>" + "Order Qty<br/>订购件数" + "</td>";
                lEmailContent = lEmailContent + "<td>" + "Total Weight<br/>总重量" + "</td>";
                lEmailContent = lEmailContent + "</td></tr>";


                for (int i = 0; i < lStdContent.Count; i++)
                {
                    lEmailContent = lEmailContent + "<tr><td align='center'> <font color='blue'>" + (i + 1).ToString() + "</font></td>";
                    lEmailContent = lEmailContent + "<td align='center'><font color='blue'><strong>" + lStdContent[i].ProdCode + "</strong></font></td>";
                    lEmailContent = lEmailContent + "<td align='center'><font color='blue'>" + lStdContent[i].ProdDesc + "</font></td>";
                    lEmailContent = lEmailContent + "<td align='center'><font color='blue'>" + lStdContent[i].Diameter.ToString() + "</font></td>";
                    lEmailContent = lEmailContent + "<td align='center'><font color='blue'>" + lStdContent[i].Grade + "</font></td>";
                    lEmailContent = lEmailContent + "<td align='right'><font color='blue'>" + lStdContent[i].UnitWT.ToString("F0") + "</font></td>";
                    if (lStdContent[i].order_pcs == 0) lVar = ""; else lVar = lStdContent[i].order_pcs.ToString();
                    lEmailContent = lEmailContent + "<td align='center'><font color='blue'><strong>" + lVar + "</strong></font></td>";
                    if (lStdContent[i].order_wt == 0) lVar = ""; else lVar = lStdContent[i].order_wt.ToString("F0");
                    lEmailContent = lEmailContent + "<td align='right'><font color='blue'>" + lVar + "</font></td>";
                    lEmailContent = lEmailContent + "</tr>";
                }
                lEmailContent = lEmailContent + "</table>";
            }

            lEmailContent = lEmailContent + "<table border='1' width=100%><tr>";

            lEmailContent = lEmailContent + "<td width='20%'>" + "Site Contact (联系人)" + "</td>";
            lEmailContent = lEmailContent + "<td width='15%'>" + JobContent.SiteEngr_Name.Trim() + "</td>";
            lEmailContent = lEmailContent + "<td width='13%'>" + "Handphone (手机号码)" + " </td>";
            lEmailContent = lEmailContent + "<td width='16%'>" + JobContent.SiteEngr_HP.Trim() + "</td>";
            lEmailContent = lEmailContent + "<td width='13%'>" + "Email (电邮地址)" + "</td>";
            lEmailContent = lEmailContent + "<td>" + JobContent.SiteEngr_Tel.Trim() + "</td></tr>";

            lEmailContent = lEmailContent + "<tr><td width='20%'>" + "Goods Receiver (收货人)" + "</td>";
            lEmailContent = lEmailContent + "<td width='15%'>" + JobContent.Scheduler_Name.Trim() + "</td>";
            lEmailContent = lEmailContent + "<td width='13%'>" + "Handphone (手机号码)" + " </td>";
            lEmailContent = lEmailContent + "<td width='16%'>" + JobContent.Scheduler_HP.Trim() + "</td>";
            lEmailContent = lEmailContent + "<td width='13%'>" + "Email (电邮地址)" + "</td>";
            lEmailContent = lEmailContent + "<td>" + JobContent.Scheduler_Tel.Trim() + "</td></tr></table>";

            lEmailContent = lEmailContent + "<table border='1' width=100%><tr>";

            lEmailContent = lEmailContent + "<td colspan='3'>" + "NatSteel Contacts (大众钢铁联系人) (Fax:62619133/62665153)" + "</td></tr>";

            lEmailContent = lEmailContent + "<tr><td width='20%'>" + "Name (姓名)" + "</td>";
            lEmailContent = lEmailContent + "<td width='15%'>" + "Contact Numbers (联系电话)" + "</td>";
            lEmailContent = lEmailContent + "<td width='13%'>" + "Email Address (电邮地址)" + " </td></tr>";


            var lProjContent = db.Project.Find(CustomerCode, ProjectCode);

            if (lProjContent.Contact1 != null)
            {
                if (lProjContent.Contact1.Trim().Length > 0)
                {
                    lEmailContent = lEmailContent + "<tr><td>" + lProjContent.Contact1.Trim() + "</td>";
                    lVar1 = "";
                    if (lProjContent.Tel1 != null) if (lProjContent.Tel1.Trim().Length > 0) lVar1 = lProjContent.Tel1.Trim();
                    lEmailContent = lEmailContent + "<td>" + lVar1 + "</td>";

                    lVar1 = "";
                    if (lProjContent.Email1 != null) if (lProjContent.Email1.Trim().Length > 0) lVar1 = lProjContent.Email1.Trim();
                    lEmailContent = lEmailContent + "<td>" + lVar1 + " </td></tr>";
                    if (lVar1 != "") if (lEmailTo == "") lEmailTo = lVar1; else lEmailTo = lEmailTo + ";" + lVar1;
                }
            }

            if (lProjContent.Contact2 != null)
            {
                if (lProjContent.Contact2.Trim().Length > 0)
                {
                    lEmailContent = lEmailContent + "<tr><td>" + lProjContent.Contact2.Trim() + "</td>";
                    lVar1 = "";
                    if (lProjContent.Tel2 != null) if (lProjContent.Tel2.Trim().Length > 0) lVar1 = lProjContent.Tel2.Trim();
                    lEmailContent = lEmailContent + "<td>" + lVar1 + "</td>";

                    lVar1 = "";
                    if (lProjContent.Email2 != null) if (lProjContent.Email2.Trim().Length > 0) lVar1 = lProjContent.Email2.Trim();
                    lEmailContent = lEmailContent + "<td>" + lVar1 + " </td></tr>";
                    if (lVar1 != "") if (lEmailTo == "") lEmailTo = lVar1; else lEmailTo = lEmailTo + ";" + lVar1;
                }
            }

            if (lProjContent.Contact3 != null)
            {
                if (lProjContent.Contact3.Trim().Length > 0)
                {
                    lEmailContent = lEmailContent + "<tr><td>" + lProjContent.Contact3.Trim() + "</td>";
                    lVar1 = "";
                    if (lProjContent.Tel3 != null) if (lProjContent.Tel3.Trim().Length > 0) lVar1 = lProjContent.Tel3.Trim();
                    lEmailContent = lEmailContent + "<td>" + lVar1 + "</td>";

                    lVar1 = "";
                    if (lProjContent.Email3 != null) if (lProjContent.Email3.Trim().Length > 0) lVar1 = lProjContent.Email3.Trim();
                    lEmailContent = lEmailContent + "<td>" + lVar1 + " </td></tr>";
                    if (lVar1 != "") if (lEmailTo == "") lEmailTo = lVar1; else lEmailTo = lEmailTo + ";" + lVar1;
                }
            }

            if (lProjContent.Contact4 != null)
            {
                if (lProjContent.Contact4.Trim().Length > 0)
                {
                    lEmailContent = lEmailContent + "<tr><td>" + lProjContent.Contact4.Trim() + "</td>";
                    lVar1 = "";
                    if (lProjContent.Tel4 != null) if (lProjContent.Tel4.Trim().Length > 0) lVar1 = lProjContent.Tel4.Trim();
                    lEmailContent = lEmailContent + "<td>" + lVar1 + "</td>";

                    lVar1 = "";
                    if (lProjContent.Email4 != null) if (lProjContent.Email4.Trim().Length > 0) lVar1 = lProjContent.Email4.Trim();
                    lEmailContent = lEmailContent + "<td>" + lVar1 + " </td></tr>";
                    if (lVar1 != "") if (lEmailTo == "") lEmailTo = lVar1; else lEmailTo = lEmailTo + ";" + lVar1;
                }
            }

            if (lProjContent.Contact5 != null)
            {
                if (lProjContent.Contact5.Trim().Length > 0)
                {
                    lEmailContent = lEmailContent + "<tr><td>" + lProjContent.Contact5.Trim() + "</td>";
                    lVar1 = "";
                    if (lProjContent.Tel5 != null) if (lProjContent.Tel5.Trim().Length > 0) lVar1 = lProjContent.Tel5.Trim();
                    lEmailContent = lEmailContent + "<td>" + lVar1 + "</td>";

                    lVar1 = "";
                    if (lProjContent.Email5 != null) if (lProjContent.Email5.Trim().Length > 0) lVar1 = lProjContent.Email5.Trim();
                    lEmailContent = lEmailContent + "<td>" + lVar1 + " </td></tr>";
                    if (lVar1 != "") if (lEmailTo == "") lEmailTo = lVar1; else lEmailTo = lEmailTo + ";" + lVar1;
                }
            }

            if (lProjContent.Contact6 != null)
            {
                if (lProjContent.Contact6.Trim().Length > 0)
                {
                    lEmailContent = lEmailContent + "<tr><td>" + lProjContent.Contact6.Trim() + "</td>";
                    lVar1 = "";
                    if (lProjContent.Tel6 != null) if (lProjContent.Tel6.Trim().Length > 0) lVar1 = lProjContent.Tel6.Trim();
                    lEmailContent = lEmailContent + "<td>" + lVar1 + "</td>";

                    lVar1 = "";
                    if (lProjContent.Email6 != null) if (lProjContent.Email6.Trim().Length > 0) lVar1 = lProjContent.Email6.Trim();
                    lEmailContent = lEmailContent + "<td>" + lVar1 + " </td></tr>";
                    if (lVar1 != "") if (lEmailTo == "") lEmailTo = lVar1; else lEmailTo = lEmailTo + ";" + lVar1;
                }
            }
            lEmailContent = lEmailContent + "</table>";

            // Scheduler email
            if (JobContent != null)
            {
                lVar1 = JobContent.Scheduler_Tel.Trim();
                if (lVar1.Length > 0)
                {
                    if (lEmailCc == "")
                    {
                        lEmailCc = lVar1;
                    }
                    else
                    {
                        lEmailCc = lEmailCc + ";" + lVar1;
                    }
                }

                lVar1 = JobContent.SiteEngr_Tel.Trim();
                if (lVar1.Length > 0 && lEmailCc.IndexOf(lVar1) < 0)
                {
                    if (lEmailCc == "")
                    {
                        lEmailCc = lVar1;
                    }
                    else
                    {
                        lEmailCc = lEmailCc + ";" + lVar1;
                    }
                }
            }

            if (lProjContent.EmailDistribution != null)
            {
                lVar1 = lProjContent.EmailDistribution.Trim();
                if (lVar1.Length > 0 && lEmailCc.IndexOf(lVar1) < 0)
                {
                    if (lEmailCc == "")
                    {
                        lEmailCc = lVar1;
                    }
                    else
                    {
                        lEmailCc = lEmailCc + ";" + lVar1;
                    }
                }
            }

            if (JobContent.UpdateBy != null)
            {
                lVar1 = JobContent.UpdateBy.Trim();
                if (lVar1 != "" && lEmailCc.IndexOf(lVar1) < 0)
                {
                    if (lEmailCc == "") { lEmailCc = lVar1; }
                    else { lEmailCc = lEmailCc + ";" + lVar1; }
                }
            }

            lVar = "";
            if (lCustomer != null) lVar = lCustomer.CustomerName.Trim();

            lEmailSubject = lVar + " - " + JobContent.PONumber.Trim() + " - Standard Products No. " + JobID.ToString();

            var lOESEmail = new SendGridEmail();

            string lEmailFromAddress = "eprompt@natsteel.com.sg";
            string lEmailFromName = "Digital Ordering Email Services";

            //dynamic response = lOESEmail.Execute(lEmailFromAddress, lEmailFromName, lEmailTo, lEmailCc, lEmailSubject, "text/html", lEmailContent);
            lOESEmail.Execute(lEmailFromAddress, lEmailFromName, lEmailTo, lEmailCc, lEmailSubject, "text/html", lEmailContent).Wait();
            lOESEmail = null;

            return Json(true);
        }

        //[HttpPost]

        //[Route("/getProject_coupler/{CustomerCode}")]
        ////[ValidateAntiForgeryHeader]
        //public ActionResult getProject(string CustomerCode)
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
        //        lUserType = lUa.getUserType(User.Identity.GetUserName());
        //        lGroupName = lUa.getGroupName(User.Identity.GetUserName());
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

        //        // try the sql.

        //        //content = db.Database.SqlQuery<ProjectListModels>(
        //        //    "SELECT * FROM OESProjectList P " +
        //        //    "WHERE P.CustomerCode = @cust " +
        //        //    "ORDER BY ProjectTitle ", new SqlParameter("cust", CustomerCode)).ToList();

        //        content = new List<ProjectListModels>();

        //        #region get from SAP
        //        var lDa = new OracleDataAdapter();
        //        var lCmd = new OracleCommand();
        //        var lDs = new DataSet();
        //        var lDStatus = new DataSet();
        //        var lcisCon = new OracleConnection();
        //        var lProcess = new ProcessController();

        //        lCmd.CommandText = "SELECT (NAME1 || NAME2) AS SHIP_TO_NAME,KUNNR AS SHIP_TO_PARTY FROM SAPSR3.KNA1 " +
        //        "WHERE KTOKD  = 'Y001' AND MANDT ='" + lProcess.strClient + "' " +
        //        "AND KUNNR IN (SELECT KUNNR FROM SAPSR3.VBPA WHERE MANDT='" + lProcess.strClient + "' " +
        //        "AND VBELN IN (SELECT VBELN FROM SAPSR3.VBAK WHERE MANDT ='" + lProcess.strClient + "' " +
        //        "AND (VBELN like '102%'  OR VBELN like '_102%') " +
        //        "AND (ytot_mesh > 0 OR ytot_rebar > 0 OR ytot_cold_roll > 0 OR ytotal_WR > 0 OR ytot_pre_cutwr > 0 OR ytotal_pcstrand > 0) " +
        //        "AND KUNNR ='" + CustomerCode + "' AND TRVOG ='4' " +
        //        "AND to_date(GUEEN, 'yyyymmdd') >=  (SYSDATE - 180) )) " +
        //        "ORDER BY 1 ";

        //        if (lProcess.OpenCISConnection(ref lcisCon) == true)
        //        {
        //            lCmd.Connection = lcisCon;
        //            lDa.SelectCommand = lCmd;
        //            lDs.Clear();
        //            lDa.Fill(lDs);
        //            if (lDs.Tables[0].Rows.Count == 0)
        //                lCmd.CommandText = "SELECT (NAME1 || NAME2) AS SHIP_TO_NAME,KUNNR AS SHIP_TO_PARTY FROM SAPSR3.KNA1 " +
        //                "WHERE KTOKD  = 'Y001' AND MANDT ='" + lProcess.strClient + "' " +
        //                "AND KUNNR IN (SELECT KUNNR FROM SAPSR3.VBPA WHERE MANDT='" + lProcess.strClient + "' " +
        //                "AND VBELN IN (SELECT VBELN FROM SAPSR3.VBAK WHERE MANDT ='" + lProcess.strClient + "' " +
        //                "AND (VBELN like '102%' OR VBELN like '_102%') " +
        //                "AND (ytot_cab > 0 OR ytot_rebar > 0 ) " +
        //                "AND KUNNR ='" + CustomerCode + "' AND TRVOG ='4' " +
        //                "AND to_date(GUEEN, 'yyyymmdd') >=  (SYSDATE - 180) )) " +
        //                "ORDER BY 1 ";
        //            lCmd.Connection = lcisCon;
        //            lDa.SelectCommand = lCmd;
        //            lDs.Clear();
        //            lDa.Fill(lDs);

        //            if (lDs.Tables[0].Rows.Count > 0)
        //            {
        //                for (int i = 0; i < lDs.Tables[0].Rows.Count; i++)
        //                {
        //                    string lName = ((string)lDs.Tables[0].Rows[i].ItemArray[0]).Trim();
        //                    string lCode = ((string)lDs.Tables[0].Rows[i].ItemArray[1]).Trim();
        //                    content.Add(new ProjectListModels
        //                    {
        //                        CustomerCode = CustomerCode,
        //                        ProjectCode = lCode,
        //                        ProjectTitle = lName
        //                    });
        //                }
        //            }
        //            lProcess.CloseCISConnection(ref lcisCon);
        //        }
        //        lDa = null;
        //        lCmd = null;
        //        lDs = null;
        //        lDStatus = null;
        //        lcisCon = null;
        //        lProcess = null;
        //        #endregion

        //        #region Get from NDS
        //        //var lDa = new SqlDataAdapter();
        //        //var lOCmd = new SqlCommand();
        //        //var lDs = new DataSet();
        //        //var lDStatus = new DataSet();
        //        //var lOcisCon = new SqlConnection();
        //        //var lProcess = new ProcessController();

        //        //lOCmd.CommandText = "SELECT P.vchProjectCode, P.vchProjectName " +
        //        //"FROM dbo.ProjectMaster P, " +
        //        //"dbo.ContractMaster C, " +
        //        //"dbo.CustomerMaster A, " +
        //        //"dbo.ContractProductMapping M " +
        //        //"WHERE P.intContractID = C.intContractID " +
        //        //"AND A.intCustomerCode = C.intCustomerCode " +
        //        //"AND C.vchContractNumber = M.VBELN " +
        //        //"AND M.mandt = '" + lProcess.strClient + "' " +
        //        //"AND ytot_mesh > 0 " +
        //        //"AND datEndDate > DATEADD(dd, -180, getDate()) " +
        //        //"AND vchCustomerNo = '" + CustomerCode + "' " +
        //        //"GROUP BY P.vchProjectCode, P.vchProjectName " +
        //        //"ORDER BY P.vchProjectName ";

        //        //if (lProcess.OpenNDSConnection(ref lOcisCon) == true)
        //        //{
        //        //    lOCmd.Connection = lOcisCon;
        //        //    lDa.SelectCommand = lOCmd;
        //        //    lDs.Clear();
        //        //    lDa.Fill(lDs);
        //        //    if (lDs.Tables[0].Rows.Count > 0)
        //        //        for (int i = 0; i < lDs.Tables[0].Rows.Count; i++)
        //        //        {
        //        //            string lCode = ((string)lDs.Tables[0].Rows[i].ItemArray[0]).Trim();
        //        //            string lName = ((string)lDs.Tables[0].Rows[i].ItemArray[1]).Trim();
        //        //            content.Add(new ProjectListModels
        //        //            {
        //        //                CustomerCode = CustomerCode,
        //        //                ProjectCode = lCode,
        //        //                ProjectTitle = lName
        //        //            });
        //        //        }
        //        //    lProcess.CloseNDSConnection(ref lOcisCon);
        //        //}
        //        //lDa = null;
        //        //lOCmd = null;
        //        //lDs = null;
        //        //lDStatus = null;
        //        //lOcisCon = null;
        //        //lProcess = null;
        //        #endregion

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
        //            content = RemoveNonMESHProject(content);
        //        }

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

        //    //return Json(content, JsonRequestBehavior.AllowGet);
        //    return Ok();
        //}

        //[HttpGet]
        //[Route("/RemoveNonMESHProject_Oracle_coupler/{pInputProj}")]
        //List<ProjectListModels> RemoveNonMESHProject_Oracle(List<ProjectListModels> pInputProj)
        //{
        //    List<ProjectListModels> lReturn = new List<ProjectListModels>();
        //    string lProjectCode = "";

        //    OracleDataReader lRst;
        //    var lCmd = new OracleCommand();
        //    var lcisCon = new OracleConnection();

        //    if (pInputProj.Count() > 0)
        //    {
        //        var lProcessObj = new ProcessController();
        //        if (lProcessObj.OpenCISConnection(ref lcisCon) == true)
        //        {
        //            var lSQL = "";
        //            for (int i = 0; i < pInputProj.Count(); i++)
        //            {
        //                if (lSQL == "")
        //                {
        //                    lSQL = " P.KUNNR = '" + pInputProj[i].ProjectCode + "' ";
        //                }
        //                else
        //                {
        //                    lSQL = lSQL + " OR " + "P.KUNNR = '" + pInputProj[i].ProjectCode + "' ";
        //                }
        //            }

        //            lCmd.CommandText = "SELECT distinct P.KUNNR " +
        //            "FROM SAPSR3.VBAK K, SAPSR3.VBPA P " +
        //            "WHERE K.MANDT = '" + lProcessObj.strClient + "' " +
        //            "AND (K.VBELN like '102%' OR K.VBELN like '_102%') " +
        //            "AND P.MANDT = '" + lProcessObj.strClient + "' " +
        //            "AND K.VKORG = '" + lProcessObj.strSalesOrg + "' " +
        //            "AND K.KUNNR = '" + pInputProj[0].CustomerCode + "' " +
        //            "AND K.TRVOG = '4' " +
        //            "AND K.ytot_mesh > 0 " +
        //            "AND k.VBELN = P.VBELN " +
        //            "AND ( " + lSQL + " ) ";

        //            lCmd.Connection = lcisCon;
        //            lCmd.CommandTimeout = 1200;
        //            lRst = lCmd.ExecuteReader();
        //            if (lRst.HasRows)
        //            {
        //                while (lRst.Read())
        //                {
        //                    lProjectCode = lRst.GetString(0);

        //                    for (int i = 0; i < pInputProj.Count(); i++)
        //                    {
        //                        if (pInputProj[i].ProjectCode.Trim() == lProjectCode.Trim())
        //                        {
        //                            lReturn.Add(pInputProj[i]);
        //                        }
        //                    }

        //                }
        //            }
        //            lRst.Close();
        //        }

        //        lProcessObj.CloseCISConnection(ref lcisCon);
        //        lCmd = null;
        //        lRst = null;
        //        lProcessObj = null;
        //    }

        //    return lReturn;
        //}

        [HttpGet]
        [Route("/RemoveNonMESHProject_coupler/{pInputProj}")]
        public List<ProjectListModels> RemoveNonMESHProject(List<ProjectListModels> pInputProj)
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
                        "AND ProjectMESH = 1 ";

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
        [Route("/getProjectDetails_coupler/{CustomerCode}/{ProjectCode}/{UserName}")]
        //[ValidateAntiForgeryHeader]
        public ActionResult getProjectDetails(string CustomerCode, string ProjectCode,string UserName)
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

            if (lUserType == "CU" || lUserType == "CA" || lUserType == "CM")
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
                "OrderCreation " +
                "FROM dbo.OESProject P, dbo.OESUserAccess U " +
                "WHERE P.CustomerCode = U.CustomerCode " +
                "AND P.ProjectCode = U.ProjectCode " +
                "AND U.UserName = '" + User.Identity.Name + "' " +
                "AND U.CustomerCode = '" + CustomerCode + "' " +
                "AND U.ProjectCode = '" + ProjectCode + "' ";
            }
            else if (lUserType == "TE" || lUserType == "PL" || lUserType == "AD" || lUserType == "PM" || lUserType == "PA" || lUserType == "P1" || lUserType == "P2" || lUserType == "PU")
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
                "'Yes' as OrderCreation " +
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
                "'No' as OrderCreation " +
                "FROM dbo.OESProject P " +
                "WHERE P.CustomerCode = '" + CustomerCode + "' " +
                "AND P.ProjectCode = '" + ProjectCode + "' ";
            }

            var lProcessObj = new ProcessController();
            if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
            {
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
                            OrderCreation = lRst.GetValue(28) == DBNull.Value ? "No" : lRst.GetString(28).Trim()
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
                                    OrderCreation = lRst.GetValue(28) == DBNull.Value ? "No" : lRst.GetString(28).Trim()
                                };
                            }
                        }
                        lRst.Close();

                    }
                }

                lProcessObj.CloseNDSConnection(ref lNDSCon);
            }


            lProcessObj = null;

            //return Json(content1, JsonRequestBehavior.AllowGet);
            return Ok(content1);
        }

        [HttpPost]
        [Route("/getProductTypeList_coupler")]
        // [ValidateAntiForgeryHeader]
        public ActionResult getProductTypeList()
        {
            var lCmd = new SqlCommand();
            SqlDataReader lRst;
            var lNDSCon = new SqlConnection();
            var lReturn = (new[] { new {
                ProdTypeCode = "",
                ProdTypeDesc = ""
            }
            }).ToList();

            lCmd.CommandText = " SELECT ProdCategory, ProdTypeCode, ProdTypeDesc " +
            "FROM dbo.OESStdProdType WHERE ProdTypeCode = '1' OR ProdTypeCode = '2' " +
            "ORDER BY ProdTypeSort ";

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
                        lReturn.Add(new
                        {
                            ProdTypeCode = lRst.GetString(0).Trim() + "-" + lRst.GetString(1).Trim(),
                            ProdTypeDesc = lRst.GetString(2).Trim()
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
            lReturn.RemoveAt(0);

            //return Json(lReturn, JsonRequestBehavior.AllowGet);
            return Ok(lReturn);
        }

        //[HttpPost]
        //[Route("/getOrderList_coupler/{CustomerCode}/{ProjectCode}")]
        ////[ValidateAntiForgeryHeader]
        //public ActionResult getOrderList(string CustomerCode, string ProjectCode)
        //{
        //    createJobAdvice(CustomerCode, ProjectCode, false);
        //    updateOrderStatus(CustomerCode, ProjectCode);

        //    //var content = (from p in db.StdSheetJobAdvice
        //    //               join s in db.StdSheetPODoc
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

        //    var content = (from p in db.StdSheetJobAdvice
        //                   let s = db.StdSheetPODoc.Where(s2 => p.CustomerCode == s2.CustomerCode &&
        //                   p.ProjectCode == s2.ProjectCode &&
        //                   p.JobID == s2.JobID).FirstOrDefault()
        //                   where p.CustomerCode == CustomerCode &&
        //                   p.ProjectCode == ProjectCode
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
        //        OrderDesc = h.JobID.ToString() + " PO:" + (h.PONumber == null ? "" : h.PONumber.Trim()) + " RD:"
        //        + (h.RequiredDate == null ? "" : ((DateTime)h.RequiredDate).ToString("yyyy-MM-dd"))
        //        + " Status:" + (h.OrderStatus == null ? "" : h.OrderStatus.Trim()
        //        + (h.FileName == null ? "" : " PO Attached"))
        //    }));

        //    //   return Json(content1, JsonRequestBehavior.AllowGet);
        //    return Ok(content1);
        //}

        //[HttpGet]
        //[Route("/checkOrderStatus_coupler/{pCustomerCode}/{pProjectCode}")]
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

        //    try
        //    {
        //        var lProcessObj = new ProcessController();
        //        if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
        //        {
        //            lCmd.CommandText = "SELECT SAPSONo, JobID " +
        //            "FROM dbo.OESStdSheetJobAdvice " +
        //            "WHERE CustomerCode = '" + pCustomerCode + "' " +
        //            "AND ProjectCode = '" + pProjectCode + "' " +
        //            "AND SAPSONo > '' " +
        //            "AND OrderStatus = 'Processed' ";

        //            lCmd.Connection = lNDSCon;
        //            lCmd.CommandTimeout = 300;
        //            lRst = lCmd.ExecuteReader();
        //            if (lRst.HasRows)
        //            {
        //                if (lProcessObj.OpenCISConnection(ref lCISCon) == true)
        //                {
        //                    while (lRst.Read())
        //                    {
        //                        lSAPSO = lRst.GetValue(0) == DBNull.Value ? "" : lRst.GetString(0).Trim();
        //                        lJOBID = lRst.GetValue(1) == DBNull.Value ? 0 : lRst.GetInt32(1);

        //                        lSQL = "SELECT  " +
        //                        "NVL(MAX(WEIGH_OUT_TIME), ' ') " +
        //                        "FROM SAPSR3.YMPPT_LOAD_VEHIC " +
        //                        "WHERE MANDT = '" + lProcessObj.strClient + "' " +
        //                        "AND VBELN = '" + lSAPSO + "' " +
        //                        "ORDER BY 1 DESC ";

        //                        lOraCmd.CommandText = lSQL;
        //                        lOraCmd.Connection = lCISCon;
        //                        lOraCmd.CommandTimeout = 300;
        //                        lOraRst = lOraCmd.ExecuteReader();
        //                        if (lOraRst.HasRows)
        //                        {
        //                            if (lOraRst.Read())
        //                            {
        //                                lOutTime = lOraRst.GetString(0).Trim();

        //                                if (lOutTime != "")
        //                                {
        //                                    lCmdUpdate.CommandText = "Update dbo.OESStdSheetJobAdvice " +
        //                                    "SET OrderStatus = 'Delivered' " +
        //                                    "WHERE CustomerCode = '" + pCustomerCode + "' " +
        //                                    "AND ProjectCode = '" + pProjectCode + "' " +
        //                                    "AND JobID = " + lJOBID.ToString() + " ";

        //                                    lCmdUpdate.Connection = lNDSCon;
        //                                    lCmdUpdate.CommandTimeout = 300;
        //                                    lCmdUpdate.ExecuteNonQuery();
        //                                }
        //                            }
        //                        }
        //                        lOraRst.Close();
        //                    }
        //                    lProcessObj.CloseCISConnection(ref lCISCon);
        //                }
        //            }
        //            lProcessObj.CloseNDSConnection(ref lNDSCon);
        //        }
        //        lProcessObj = null;
        //    }
        //    catch (Exception e)
        //    {
        //    }
        //    return lReturn;
        //}

        [HttpGet]
        [Route("/createJobAdvice_coupler/{pCustomerCode}/{pProjectCode}/{pClone}")]
        public int createJobAdvice(string pCustomerCode, string pProjectCode, bool pClone)
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
                    var lSubmission = "No";
                    var lEditable = "No";

                    string lUserType = "";
                    if (gUserType != null && gUserType != "")
                    {
                        lUserType = gUserType;
                    }
                    else
                    {
                        UserAccessController lUa = new UserAccessController();
                        lUserType = lUa.getUserType(User.Identity.GetUserName());
                        gUserType = lUserType;
                        lUa = null;
                    }

                    if (lUserType == "CU" || lUserType == "CA" || lUserType == "PL" || lUserType == "AD" || lUserType == "PM" || lUserType == "PA" || lUserType == "P1" || lUserType == "P2" || lUserType == "PU")
                    {
                        var lAccess = db.UserAccess.Find(User.Identity.Name, pCustomerCode, pProjectCode);
                        if (lAccess != null)
                        {
                            lSubmission = lAccess.OrderSubmission.Trim();
                            lEditable = lAccess.OrderCreation.Trim();
                        }
                        if (lUserType == "PL" || lUserType == "AD" || lUserType == "PM" || lUserType == "PA" || lUserType == "P1" || lUserType == "P2" || lUserType == "PU")
                        {
                            lEditable = "Yes";
                        }

                        if (lEditable == "Yes")
                        {
                            lFound = 0;

                            lJobID = db.StdSheetJobAdvice.Where(z => z.CustomerCode == pCustomerCode &&
                            z.ProjectCode == pProjectCode).Max(z => (int?)z.JobID);

                            if (lJobID != null)
                            {
                                lFound = 1;

                                var lCheckStdDet = (from p in db.StdProdDetails
                                                    where p.CustomerCode == pCustomerCode &&
                                                    p.ProjectCode == pProjectCode &&
                                                    p.JobID == lJobID &&
                                                    p.order_pcs > 0 &&
                                                    p.UpdateBy != User.Identity.Name
                                                    select p).ToList();

                                var lCheckSheetDet = (from p in db.StdSheetDetails
                                                      where p.CustomerCode == pCustomerCode &&
                                                      p.ProjectCode == pProjectCode &&
                                                      p.JobID == lJobID &&
                                                      p.order_pcs > 0 &&
                                                      p.UpdateBy != User.Identity.Name
                                                      select p).ToList();

                                var lCheckJob = db.StdSheetJobAdvice.Find(pCustomerCode, pProjectCode, lJobID);

                                if (lCheckJob == null || (lCheckJob.OrderStatus != "New" && lCheckJob.OrderStatus != "Created") ||
                                    lCheckStdDet.Count > 0 || lCheckSheetDet.Count > 0 ||
                                    (lCheckJob.PONumber != null && lCheckJob.PONumber != "") ||
                                    (lCheckJob.TotalPcs > 0 &&
                                    (lCheckJob.UpdateBy != User.Identity.GetUserName() ||
                                    pClone == true)))
                                {
                                    lFound = 0;
                                }
                            }

                            if (lFound == 0)
                            {

                                var lJobAdv = new StdSheetJobAdviceModels();
                                lJobAdv.CustomerCode = pCustomerCode;
                                lJobAdv.ProjectCode = pProjectCode;

                                if (lJobID == null)
                                {
                                    lJobAdv.JobID = 1;
                                }
                                else
                                {
                                    lJobAdv.JobID = (int)lJobID + 1;
                                }

                                lJobAdv.PODate = DateTime.Now;
                                lJobAdv.RequiredDate = DateTime.Now.AddDays(5);
                                lJobAdv.TotalPcs = 0;
                                lJobAdv.TotalWeight = 0;

                                lJobAdv.Transport = "HC";
                                lJobAdv.OrderStatus = "New";
                                lJobAdv.DeliveryAddress = "";
                                lJobAdv.Remarks = "";

                                var lProj = db.Project.Find(pCustomerCode, pProjectCode);
                                if (lProj != null)
                                {
                                    var lProcessObj = new ProcessController();
                                    if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
                                    {
                                        lCmd.CommandText =
                                        "SELECT isNull(MAX(JobID), 0) FROM dbo.OESStdSheetJobAdvice " +
                                        "WHERE CustomerCode = '" + pCustomerCode + "' " +
                                        "AND ProjectCode = '" + pProjectCode + "' " +
                                        "AND OrderStatus <> 'New' " +
                                        "AND OrderStatus <> 'Created' " +
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
                                                "SELECT isNull(MAX(JobID), 0) FROM dbo.OESStdSheetJobAdvice " +
                                                "WHERE CustomerCode = '" + pCustomerCode + "' " +
                                                "AND ProjectCode = '" + pProjectCode + "' " +
                                                "AND OrderStatus <> 'New' " +
                                                "AND OrderStatus <> 'Created' " +
                                                "AND UpdateBy = '" + User.Identity.GetUserName() + "' " +
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
                                                "FROM dbo.OESStdSheetJobAdvice " +
                                                "WHERE CustomerCode = '" + pCustomerCode + "' " +
                                                "AND ProjectCode = '" + pProjectCode + "' " +
                                                "AND JobID = " + LastMyJobID.ToString() + " " +
                                                "AND UpdateBy = '" + User.Identity.GetUserName() + "' " +
                                                "UNION " +
                                                "SELECT isNull(SiteEngr_Name, ''), " +
                                                "isNull(SiteEngr_HP, ''), " +
                                                "isNull(SiteEngr_Tel, ''), " +
                                                "isNull(Scheduler_Name, ''), " +
                                                "isNull(Scheduler_HP, ''), " +
                                                "isNull(Scheduler_Tel, ''), " +
                                                "isNull(DeliveryAddress, '') " +
                                                "FROM dbo.OESStdSheetJobAdvice " +
                                                "WHERE CustomerCode = '" + pCustomerCode + "' " +
                                                "AND ProjectCode = '" + pProjectCode + "' " +
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
                                                "FROM dbo.OESStdSheetJobAdvice " +
                                                "WHERE CustomerCode = '" + pCustomerCode + "' " +
                                                "AND ProjectCode = '" + pProjectCode + "' " +
                                                "AND JobID = " + LastJobID.ToString() + " ";
                                            }

                                            lCmd.Connection = lNDSCon;
                                            lCmd.CommandTimeout = 300;
                                            lRst = lCmd.ExecuteReader();
                                            if (lRst.HasRows)
                                            {
                                                if (lRst.Read())
                                                {
                                                    lJobAdv.Scheduler_Name = lRst.GetString(0);
                                                    lJobAdv.Scheduler_HP = lRst.GetString(1);
                                                    lJobAdv.Scheduler_Tel = lRst.GetString(2);
                                                    lJobAdv.SiteEngr_Name = lRst.GetString(3);
                                                    lJobAdv.SiteEngr_HP = lRst.GetString(4);
                                                    lJobAdv.SiteEngr_Tel = lRst.GetString(5);
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

                                lJobAdv.UpdateBy = User.Identity.GetUserName();

                                var oldJobAdvice = db.StdSheetJobAdvice.Find(lJobAdv.CustomerCode, lJobAdv.ProjectCode, lJobAdv.JobID);
                                if (oldJobAdvice == null)
                                {
                                    db.StdSheetJobAdvice.Add(lJobAdv);
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
        [Route("/getOrderListSearch_coupler/{CustomerCode}/{ProjectCode}/{RequiredDateFrom}/{RequiredDateTo}/{PONo}")]
        //[ValidateAntiForgeryHeader]
        public ActionResult getOrderListSearch(string CustomerCode, string ProjectCode, string RequiredDateFrom, string RequiredDateTo, string PONo)
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
            "FROM dbo.OESStdSheetPODoc s " +
            "WHERE s.CustomerCode = p.CustomerCode " +
            "AND s.ProjectCode = p.ProjectCode " +
            "AND s.JobID = p.JobID),'') " +
            "FROM dbo.OESStdSheetJobAdvice p " +
            "WHERE p.CustomerCode = '" + CustomerCode + "' " +
            "AND p.ProjectCode = '" + ProjectCode + "' " +
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
                            OrderDesc = lRst.GetInt32(0).ToString() + " PO:" + lRst.GetString(1).Trim()
                            + " RD:" + lRst.GetDateTime(2).ToString("yyyy-MM-dd")
                            + " Status:" + lRst.GetString(3).Trim()
                            + (lRst.GetString(4) == null || lRst.GetString(4).Trim() == "" ? "" : " PO Attached")
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
            // return Json(lReturn, JsonRequestBehavior.AllowGet);
            return Ok(lReturn);
        }

        //get Job Advice details
        [HttpPost]
        [Route("/getOrderDetails_coupler/{CustomerCode}/{ProjectCode}/{JobID}")]
        //[ValidateAntiForgeryHeader]
        public ActionResult getOrderDetails(string CustomerCode, string ProjectCode, int JobID)
        {
            int lPODocExists = 0;

            string lSubmission = "No";
            string lEditable = "No";

            var lJob = db.StdSheetJobAdvice.Find(CustomerCode, ProjectCode, JobID);
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

            //var content1 = db.StdSheetPODoc.Find(CustomerCode, ProjectCode, JobID);
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

            lPODocExists = db.StdSheetPODoc.Where(
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
                UpdateDate = lJob.UpdateDate,
                Exists = lPODocExists
            };
            lJob = null;
            // return Json(lReturn, JsonRequestBehavior.AllowGet);
            return Ok(lReturn);
        }


        [HttpPost]
        [Route("/setCloneOrder_coupler/{CustomerCode}/{ProjectCode}/{JobID}/{CloneNo}")]
        // [ValidateAntiForgeryHeader]
        public ActionResult setCloneOrder(string CustomerCode, string ProjectCode, int JobID, int CloneNo)
        {
            var lErrorMsg = "";
            int? lJobID = 0;
            try
            {
                if (CloneNo > 0)
                {
                    for (int i = 0; i < CloneNo; i++)
                    {
                        createJobAdvice(CustomerCode, ProjectCode, true);

                        lJobID = db.StdSheetJobAdvice.Where(z => z.CustomerCode == CustomerCode &&
                        z.ProjectCode == ProjectCode).Max(z => (int?)z.JobID);

                        var lJobNewOr = db.StdSheetJobAdvice.Find(CustomerCode, ProjectCode, lJobID);
                        var lJobNew = lJobNewOr;
                        var lJobOld = db.StdSheetJobAdvice.Find(CustomerCode, ProjectCode, JobID);
                        if (lJobOld != null && lJobNew != null)
                        {
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
                            lJobNew.TotalPcs = lJobOld.TotalPcs;
                            lJobNew.TotalWeight = lJobOld.TotalWeight;
                            lJobNew.Transport = lJobOld.Transport;
                            lJobNew.UpdateBy = User.Identity.GetUserName();
                            lJobNew.UpdateDate = DateTime.Now;
                            db.Entry(lJobNewOr).CurrentValues.SetValues(lJobNew);
                        }

                        var lOld = (from p in db.StdSheetDetails
                                    where p.CustomerCode == CustomerCode &&
                                    p.ProjectCode == ProjectCode &&
                                    p.JobID == lJobID
                                    select p).ToList();
                        if (lOld != null && lOld.Count > 0)
                        {
                            db.StdSheetDetails.RemoveRange(lOld);
                        }

                        var lOldDet = (from p in db.StdSheetDetails
                                       where p.CustomerCode == CustomerCode &&
                                       p.ProjectCode == ProjectCode &&
                                       p.JobID == JobID
                                       select p).ToList();
                        if (lOldDet != null && lOldDet.Count > 0)
                        {
                            var lNewDet = new List<StdSheetDetailsModels>();
                            for (int j = 0; j < lOldDet.Count; j++)
                            {

                                lNewDet.Add(new StdSheetDetailsModels
                                {
                                    CustomerCode = lOldDet[j].CustomerCode,
                                    ProjectCode = lOldDet[j].ProjectCode,
                                    JobID = (int)lJobID,
                                    SheetID = lOldDet[j].SheetID,
                                    SheetSort = lOldDet[j].SheetSort,
                                    std_type = lOldDet[j].std_type,
                                    mesh_series = lOldDet[j].mesh_series,
                                    sheet_name = lOldDet[j].sheet_name,
                                    mw_length = lOldDet[j].mw_length,
                                    mw_size = lOldDet[j].mw_size,
                                    mw_spacing = lOldDet[j].mw_spacing,
                                    mo1 = lOldDet[j].mo1,
                                    mo2 = lOldDet[j].mo2,
                                    cw_length = lOldDet[j].cw_length,
                                    cw_size = lOldDet[j].cw_size,
                                    cw_spacing = lOldDet[j].cw_spacing,
                                    co1 = lOldDet[j].co1,
                                    co2 = lOldDet[j].co2,
                                    unit_weight = lOldDet[j].unit_weight,
                                    order_pcs = lOldDet[j].order_pcs,
                                    order_wt = lOldDet[j].order_wt,
                                    sap_mcode = lOldDet[j].sap_mcode,
                                    UpdateBy = User.Identity.GetUserName(),
                                    UpdateDate = DateTime.Now
                                });
                            }
                            db.StdSheetDetails.AddRange(lNewDet);
                        }

                        //Clone for other standard products
                        var lOldOther = (from p in db.StdProdDetails
                                         where p.CustomerCode == CustomerCode &&
                                         p.ProjectCode == ProjectCode &&
                                         p.JobID == lJobID
                                         select p).ToList();
                        if (lOldOther != null && lOldOther.Count > 0)
                        {
                            db.StdProdDetails.RemoveRange(lOldOther);
                        }

                        var lOldDetOther = (from p in db.StdProdDetails
                                            where p.CustomerCode == CustomerCode &&
                                       p.ProjectCode == ProjectCode &&
                                       p.JobID == JobID
                                            select p).ToList();
                        if (lOldDetOther != null && lOldDetOther.Count > 0)
                        {
                            var lNewDetOther = new List<StdProdDetailsModels>();
                            for (int j = 0; j < lOldDetOther.Count; j++)
                            {

                                lNewDetOther.Add(new StdProdDetailsModels
                                {
                                    CustomerCode = lOldDetOther[j].CustomerCode,
                                    ProjectCode = lOldDetOther[j].ProjectCode,
                                    JobID = (int)lJobID,
                                    ProdCode = lOldDetOther[j].ProdCode,
                                    SSID = lOldDetOther[j].SSID,
                                    ProdType = lOldDetOther[j].ProdType,
                                    ProdDesc = lOldDetOther[j].ProdDesc,
                                    Diameter = lOldDetOther[j].Diameter,
                                    Grade = lOldDetOther[j].Grade,
                                    UnitWT = lOldDetOther[j].UnitWT,
                                    order_pcs = lOldDetOther[j].order_pcs,
                                    order_wt = lOldDetOther[j].order_wt,
                                    UpdateBy = User.Identity.GetUserName(),
                                    UpdateDate = DateTime.Now
                                });
                            }
                            db.StdProdDetails.AddRange(lNewDetOther);
                        }
                        db.SaveChanges();
                    }
                }
                //return Json(new { success = true, responseText = "Successfully saved." }, JsonRequestBehavior.AllowGet);
                return Ok();
            }
            catch (Exception ex)
            {
                lErrorMsg = ex.Message;
            }
            //  return Json(new { success = false, responseText = lErrorMsg }, JsonRequestBehavior.AllowGet);
            return Ok();
        }

        // POST: OrderDetails/SaveJobAdvice
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpGet]
        [Route("/SaveJobAdvice_coupler/{CustomerCode}/{ProjectCode}/{JobID}/{OrderStatus}/{TotalPcs}/{TotalWeight}")]
        //[ValidateAntiForgeryHeader]
        public async Task<ActionResult> SaveJobAdvice(string CustomerCode, string ProjectCode, int JobID, string OrderStatus, int? TotalPcs, decimal? TotalWeight)
        {
            var lErrorMsg = "";
            string Message = "";
            try
            {

                var oldJobAdvice = await db.StdSheetJobAdvice.FindAsync(CustomerCode, ProjectCode, JobID);
                if (oldJobAdvice != null)
                {
                    var newJobAdvice = oldJobAdvice;
                    newJobAdvice.TotalPcs = (int)(TotalPcs == null ? 0 : TotalPcs); ;
                    newJobAdvice.TotalWeight = (decimal)(TotalWeight == null ? 0 : TotalWeight);
                    db.Entry(oldJobAdvice).CurrentValues.SetValues(newJobAdvice);
                }

                //update Master
                var lProjSE = await (from p in db.OrderProjectSE
                                     join m in db.OrderProject
                                     on p.OrderNumber equals m.OrderNumber
                                     where p.CoilProdJobID == JobID &&
                                     m.CustomerCode == CustomerCode &&
                                     m.ProjectCode == ProjectCode
                                     select p).ToListAsync();
                if (lProjSE != null && lProjSE.Count > 0)
                {
                    int lOrderNo = lProjSE[0].OrderNumber;
                    string lStrEle = lProjSE[0].StructureElement;
                    string lProd = lProjSE[0].ProductType;
                    string lScheduled = lProjSE[0].ScheduledProd;

                    if (lScheduled == null || lScheduled == "")
                    {
                        lScheduled = "N";
                    }

                    var lOldOrder = await db.OrderProject.FindAsync(lOrderNo);

                    if (lOldOrder != null)
                    {
                        var lNewOrder = lOldOrder;

                        lNewOrder.TotalWeight = (decimal)(TotalWeight == null ? 0 : TotalWeight);

                        db.Entry(lOldOrder).CurrentValues.SetValues(lNewOrder);
                    }

                    var lOldSE = await db.OrderProjectSE.FindAsync(lOrderNo, lStrEle, lProd, lScheduled);
                    if (lOldSE != null)
                    {
                        var lNewSE = lOldSE;

                        lNewSE.TotalWeight = (decimal)(TotalWeight == null ? 0 : TotalWeight);
                        lNewSE.TotalPCs = TotalPcs == null ? 0 : TotalPcs;

                        db.Entry(lOldSE).CurrentValues.SetValues(lNewSE);
                    }

                }
                int lRtn = await db.SaveChangesAsync();

                // return Json(new { success = true, responseText = "Successfully saved." }, JsonRequestBehavior.AllowGet);
                Message = "Data has been saved successfully.";
                return Ok(new { Message, response = "success" });

            }
            catch (Exception ex)
            {
                lErrorMsg = ex.Message;
            }
            //return Json(new { success = false, responseText = lErrorMsg }, JsonRequestBehavior.AllowGet);
            Message = lErrorMsg;
            return Ok(new { Message, response = "failure" });
        }

        [HttpPost]
        [Route("/OrderWithdraw_coupler/{CustomerCode}/{ProjectCode}/{JobID}")]
        //[ValidateAntiForgeryHeader]
        public ActionResult OrderWithdraw(string CustomerCode, string ProjectCode, int JobID)
        {
            var lErrorMsg = "";
            try
            {
                if (CustomerCode == null)
                {
                    CustomerCode = "";
                }
                if (CustomerCode.Length > 0)
                {
                    var oldJobAdvice = db.StdSheetJobAdvice.Find(CustomerCode, ProjectCode, JobID);
                    if (oldJobAdvice != null)
                    {
                        if (oldJobAdvice.OrderStatus.Trim() == "Submitted")
                        {
                            var newJobAdvice = oldJobAdvice;
                            newJobAdvice.OrderStatus = "New";
                            newJobAdvice.UpdateDate = DateTime.Now;
                            newJobAdvice.UpdateBy = User.Identity.GetUserName();

                            db.Entry(oldJobAdvice).CurrentValues.SetValues(newJobAdvice);

                            var lStdMESHOld = (from p in db.StdSheetDetails
                                               where p.CustomerCode == CustomerCode &&
                                               p.ProjectCode == ProjectCode &&
                                               p.JobID == JobID &&
                                               p.UpdateBy != User.Identity.Name
                                               select p).ToList();

                            if (lStdMESHOld != null && lStdMESHOld.Count > 0)
                            {
                                for (int i = 0; i < lStdMESHOld.Count; i++)
                                {
                                    var lNewStdMESH = lStdMESHOld[i];
                                    lNewStdMESH.UpdateBy = User.Identity.GetUserName();

                                    db.Entry(lStdMESHOld[i]).CurrentValues.SetValues(lNewStdMESH);

                                }
                            }

                            var lStdCoilOld = (from p in db.StdProdDetails
                                               where p.CustomerCode == CustomerCode &&
                                               p.ProjectCode == ProjectCode &&
                                               p.JobID == JobID &&
                                               p.UpdateBy != User.Identity.Name
                                               select p).ToList();

                            if (lStdCoilOld != null && lStdCoilOld.Count > 0)
                            {
                                for (int i = 0; i < lStdCoilOld.Count; i++)
                                {
                                    var lNewCoil = lStdCoilOld[i];
                                    lNewCoil.UpdateBy = User.Identity.GetUserName();

                                    db.Entry(lStdCoilOld[i]).CurrentValues.SetValues(lNewCoil);

                                }
                            }


                            db.SaveChanges();

                            var lEmailContent = "";
                            var lEmailFrom = "";
                            var lEmailTo = "";
                            var lEmailCc = "";
                            var lEmailSubject = "";
                            string lVar1 = "";

                            var JobContent = db.StdSheetJobAdvice.Find(CustomerCode, ProjectCode, JobID);
                            if (JobContent != null)
                            {
                                if (JobContent.CustomerCode == null) JobContent.CustomerCode = "";
                                else JobContent.CustomerCode = JobContent.CustomerCode.Trim();

                                if (JobContent.OrderStatus == null) JobContent.OrderStatus = "";
                                else JobContent.OrderStatus = JobContent.OrderStatus.Trim();

                                if (JobContent.PONumber == null) JobContent.PONumber = "";
                                else JobContent.PONumber = JobContent.PONumber.Trim();

                                if (JobContent.ProjectCode == null) JobContent.ProjectCode = "";
                                else JobContent.ProjectCode = JobContent.ProjectCode.Trim();

                                if (JobContent.DeliveryAddress == null) JobContent.DeliveryAddress = "";
                                else JobContent.DeliveryAddress = JobContent.DeliveryAddress.Trim();

                                if (JobContent.Remarks == null) JobContent.Remarks = "";
                                else JobContent.Remarks = JobContent.Remarks.Trim();

                                if (JobContent.Scheduler_HP == null) JobContent.Scheduler_HP = "";
                                else JobContent.Scheduler_HP = JobContent.Scheduler_HP.Trim();

                                if (JobContent.Scheduler_Name == null) JobContent.Scheduler_Name = "";
                                else JobContent.Scheduler_Name = JobContent.Scheduler_Name.Trim();

                                if (JobContent.Scheduler_Tel == null) JobContent.Scheduler_Tel = "";
                                else JobContent.Scheduler_Tel = JobContent.Scheduler_Tel.Trim();

                                if (JobContent.SiteEngr_HP == null) JobContent.SiteEngr_HP = "";
                                else JobContent.SiteEngr_HP = JobContent.SiteEngr_HP.Trim();

                                if (JobContent.SiteEngr_Name == null) JobContent.SiteEngr_Name = "";
                                else JobContent.SiteEngr_Name = JobContent.SiteEngr_Name.Trim();

                                if (JobContent.SiteEngr_Tel == null) JobContent.SiteEngr_Tel = "";
                                else JobContent.SiteEngr_Tel = JobContent.SiteEngr_Tel.Trim();
                            }

                            lEmailContent = "<p align='center'>CANCEL JOB ADVICE - Standard Products (撤消工作通知 - 标准产品)</p>";

                            lEmailContent = lEmailContent + "<table border='1' width=100%><tr>";
                            lEmailContent = lEmailContent + "<td width=20%>" + "Customer (客户名称)" + "</td>";

                            CustomerModels lCustomer = db.Customer.Find(JobContent.CustomerCode);
                            string lVar = "";
                            if (lCustomer != null) lVar = lCustomer.CustomerName.Trim() + " (" + JobContent.CustomerCode.Trim() + ")";
                            lEmailContent = lEmailContent + "<td>" + lVar + "</td></tr>";

                            lEmailContent = lEmailContent + "<tr><td>" + "Project (工程项目)" + "</td>";

                            var lProject = (from p in db.ProjectList
                                            where p.ProjectCode == ProjectCode
                                            select p).First();
                            lVar = "";
                            if (lProject != null) lVar = lProject.ProjectTitle.Trim() + " (" + JobContent.ProjectCode.Trim() + ")";
                            lEmailContent = lEmailContent + "<td>" + lVar + "</td></tr></table>";

                            lEmailContent = lEmailContent + "<table border='1' width=100%><tr>";

                            lEmailContent = lEmailContent + "<td width=20%>" + "PO No. (订单号码)" + "</td>";
                            lEmailContent = lEmailContent + "<td width=27%>" + JobContent.PONumber.Trim() + "</td>";
                            lEmailContent = lEmailContent + "<td width=26%>" + "Order Date (订单日期)" + "</td>";
                            lEmailContent = lEmailContent + "<td>" + String.Format("{0:yyyy-MM-dd}", JobContent.PODate) + "</td></tr>";

                            lEmailContent = lEmailContent + "</tr><td width=20%>" + "Required Date (交货日期)" + "</td>";
                            lEmailContent = lEmailContent + "<td width=27%>" + String.Format("{0:yyyy-MM-dd}", JobContent.RequiredDate) + "</td>";

                            lEmailContent = lEmailContent + "<td width=26%>" + "Transport Mode 运输工具)" + "</td>";

                            lVar = "";
                            var lProcessObj = new ProcessController();

                            //var lCmd = new OracleCommand();
                            //OracleDataReader lRst;
                            //var lcisCon = new OracleConnection();

                            //if (lProcessObj.OpenCISConnection(ref lcisCon) == true)
                            //{

                            //    lCmd.CommandText = "SELECT BEZEI as DESCRIPTION " +
                            //    "FROM SAPSR3.TMFGT WHERE MANDT = '" + lProcessObj.strClient + "' AND SPRAS ='E' " +
                            //    "AND MFRGR = '" + JobContent.Transport + "' ";

                            //    lCmd.Connection = lcisCon;
                            //    lCmd.CommandTimeout = 300;
                            //    lRst = lCmd.ExecuteReader();
                            //    if (lRst.HasRows)
                            //    {
                            //        if (lRst.Read())
                            //        {
                            //            lVar = lRst.GetString(0).Trim();
                            //            lProcessObj.CloseCISConnection(ref lcisCon);
                            //        }
                            //    }
                            //    lRst.Close();
                            //}
                            //lCmd = null;
                            //lcisCon = null;
                            //lRst = null;

                            var lCmd = new SqlCommand();
                            SqlDataReader lRst;
                            var lNDSCon = new SqlConnection();

                            if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
                            {

                                lCmd.CommandText = "select vchTransportDescription " +
                                "from dbo.TransportMaster " +
                                "where vchTransportMode = '" + JobContent.Transport + "' ";

                                lCmd.Connection = lNDSCon;
                                lCmd.CommandTimeout = 300;
                                lRst = lCmd.ExecuteReader();
                                if (lRst.HasRows)
                                {
                                    if (lRst.Read())
                                    {
                                        lVar = lRst.GetString(0).Trim();
                                        lProcessObj.CloseNDSConnection(ref lNDSCon);
                                    }
                                }
                                lRst.Close();
                            }
                            lCmd = null;
                            lNDSCon = null;
                            lRst = null;

                            lProcessObj = null;

                            lEmailContent = lEmailContent + "<td>" + lVar + "</td></tr>";

                            lEmailContent = lEmailContent + "</tr><td width=20%>" + "Total Pieces (总件数)" + "</td>";
                            lEmailContent = lEmailContent + "<td width=27%>" + ((int)JobContent.TotalPcs).ToString() + "</td>";

                            lEmailContent = lEmailContent + "<td width=26%>" + "Total Weight (总重量)" + "</td>";

                            lEmailContent = lEmailContent + "<td>" + ((decimal)JobContent.TotalWeight).ToString("F3") + " KG" + "</td></tr></table>";

                            lEmailContent = lEmailContent + "<table border='1' width=100%><tr>";
                            lEmailContent = lEmailContent + "<td width=20%>Delivery Address(送货地址)</td>";

                            lVar = "&nbsp;";
                            if (JobContent.DeliveryAddress != null) lVar = JobContent.DeliveryAddress.Trim();
                            lEmailContent = lEmailContent + "<td>" + lVar + "</td></tr>";

                            lEmailContent = lEmailContent + "<tr><td>" + "Remarks (备注)" + "</td>";

                            lVar = "&nbsp;";
                            if (JobContent.DeliveryAddress != null) lVar = JobContent.DeliveryAddress.Trim();
                            lEmailContent = lEmailContent + "<td>" + lVar + "</td></tr></table>";

                            var lBBSContent = (from p in db.StdSheetDetails
                                               where p.CustomerCode == CustomerCode &&
                                               p.ProjectCode == ProjectCode &&
                                               p.JobID == JobID
                                               orderby p.SheetSort
                                               select p).ToList();

                            if (lBBSContent.Count > 0)
                            {
                                lEmailContent = lEmailContent + "<table border='1' width=100%><tr>";
                                lEmailContent = lEmailContent + "<td colspan='11'>" + "MESH Standard Sheet (Class A)(标准铁网)" + "</td></tr><tr>";
                                lEmailContent = lEmailContent + "<td width='5%'>" + "S/N<br/>序号" + "</td>";
                                lEmailContent = lEmailContent + "<td width='10%'>" + "Product Code<br/>产品代码" + "</td>";
                                lEmailContent = lEmailContent + "<td width='8%'>" + "Main Length<br/>主筋长" + "</td>";
                                lEmailContent = lEmailContent + "<td width='8%'>" + "Cross Length<br/>副筋长" + "</td>";
                                lEmailContent = lEmailContent + "<td width='9%'>" + "MW Size<br/>主筋直径" + "</td>";
                                lEmailContent = lEmailContent + "<td width='10%'>" + "MW Spacing<br/>主筋间距" + "</td>";
                                lEmailContent = lEmailContent + "<td width='9%'>" + "CW Size<br/>副筋直径" + "</td>";
                                lEmailContent = lEmailContent + "<td width='10%'>" + "CW Spacing<br/>副筋间距" + "</td>";
                                lEmailContent = lEmailContent + "<td width='10%'>" + "Unit Weight<br/>单片重量" + "</td>";
                                lEmailContent = lEmailContent + "<td width='10%'>" + "Order Qty<br/>订购件数" + "</td>";
                                lEmailContent = lEmailContent + "<td>" + "Total Weight<br/>总重量" + "</td>";
                                lEmailContent = lEmailContent + "</td></tr>";


                                for (int i = 0; i < lBBSContent.Count; i++)
                                {
                                    lEmailContent = lEmailContent + "<tr><td align='center'> <font color='blue'>" + (i + 1).ToString() + "</font></td>";
                                    lEmailContent = lEmailContent + "<td align='center'><font color='blue'>" + lBBSContent[i].sheet_name + "</font></td>";
                                    lEmailContent = lEmailContent + "<td align='center'><font color='blue'>" + lBBSContent[i].mw_length.ToString() + "</font></td>";
                                    lEmailContent = lEmailContent + "<td align='center'><font color='blue'>" + lBBSContent[i].cw_length.ToString() + "</font></td>";
                                    lEmailContent = lEmailContent + "<td align='center'><font color='blue'>" + lBBSContent[i].mw_size.ToString() + "</font></td>";
                                    lEmailContent = lEmailContent + "<td align='center'><font color='blue'>" + lBBSContent[i].mw_spacing.ToString() + "</font></td>";
                                    lEmailContent = lEmailContent + "<td align='center'><font color='blue'>" + lBBSContent[i].cw_size.ToString() + "</font></td>";
                                    lEmailContent = lEmailContent + "<td align='center'><font color='blue'>" + lBBSContent[i].cw_spacing.ToString() + "</font></td>";
                                    lEmailContent = lEmailContent + "<td align='right'><font color='blue'>" + lBBSContent[i].unit_weight.ToString("F3") + "</font></td>";
                                    if (lBBSContent[i].order_pcs == 0) lVar = ""; else lVar = lBBSContent[i].order_pcs.ToString();
                                    lEmailContent = lEmailContent + "<td align='center'><font color='blue'><strong>" + lVar + "</strong></font></td>";
                                    if (lBBSContent[i].order_wt == 0) lVar = ""; else lVar = lBBSContent[i].order_wt.ToString("F3");
                                    lEmailContent = lEmailContent + "<td align='right'><font color='blue'>" + lVar + "</font></td>";
                                    lEmailContent = lEmailContent + "</tr>";
                                }
                                lEmailContent = lEmailContent + "</table>";
                            }

                            var lStdContent = (from p in db.StdProdDetails
                                               where p.CustomerCode == CustomerCode &&
                                               p.ProjectCode == ProjectCode &&
                                               p.JobID == JobID
                                               orderby p.SSID
                                               select p).ToList();

                            if (lStdContent.Count > 0)
                            {
                                lEmailContent = lEmailContent + "<table border='1' width=100%><tr>";
                                lEmailContent = lEmailContent + "<td colspan='11'>" + "Standard Products (标准产品)" + "</td></tr><tr>";
                                lEmailContent = lEmailContent + "<td width='5%'>" + "S/N<br/>序号" + "</td>";
                                lEmailContent = lEmailContent + "<td width='15%'>" + "Product Code<br/>产品代码" + "</td>";
                                lEmailContent = lEmailContent + "<td width='30%'>" + "Product Description<br/>产品概述" + "</td>";
                                lEmailContent = lEmailContent + "<td width='10%'>" + "Diameter<br/>直径" + "</td>";
                                lEmailContent = lEmailContent + "<td width='10%'>" + "Grade<br/>类型" + "</td>";
                                lEmailContent = lEmailContent + "<td width='10%'>" + "Unit Weight<br/>单位重量" + "</td>";
                                lEmailContent = lEmailContent + "<td width='10%'>" + "Order Qty<br/>订购件数" + "</td>";
                                lEmailContent = lEmailContent + "<td>" + "Total Weight<br/>总重量" + "</td>";
                                lEmailContent = lEmailContent + "</td></tr>";


                                for (int i = 0; i < lStdContent.Count; i++)
                                {
                                    lEmailContent = lEmailContent + "<tr><td align='center'> <font color='blue'>" + (i + 1).ToString() + "</font></td>";
                                    lEmailContent = lEmailContent + "<td align='center'><font color='blue'><strong>" + lStdContent[i].ProdCode + "</strong></font></td>";
                                    lEmailContent = lEmailContent + "<td align='center'><font color='blue'>" + lStdContent[i].ProdDesc + "</font></td>";
                                    lEmailContent = lEmailContent + "<td align='center'><font color='blue'>" + lStdContent[i].Diameter.ToString() + "</font></td>";
                                    lEmailContent = lEmailContent + "<td align='center'><font color='blue'>" + lStdContent[i].Grade + "</font></td>";
                                    lEmailContent = lEmailContent + "<td align='right'><font color='blue'>" + lStdContent[i].UnitWT.ToString("F0") + "</font></td>";
                                    if (lStdContent[i].order_pcs == 0) lVar = ""; else lVar = lStdContent[i].order_pcs.ToString();
                                    lEmailContent = lEmailContent + "<td align='center'><font color='blue'><strong>" + lVar + "</strong></font></td>";
                                    if (lStdContent[i].order_wt == 0) lVar = ""; else lVar = lStdContent[i].order_wt.ToString("F0");
                                    lEmailContent = lEmailContent + "<td align='right'><font color='blue'>" + lVar + "</font></td>";
                                    lEmailContent = lEmailContent + "</tr>";
                                }
                                lEmailContent = lEmailContent + "</table>";
                            }

                            lEmailContent = lEmailContent + "<table border='1' width=100%><tr>";

                            lEmailContent = lEmailContent + "<td width='20%'>" + "Site Contact (联系人)" + "</td>";
                            lEmailContent = lEmailContent + "<td width='15%'>" + JobContent.SiteEngr_Name.Trim() + "</td>";
                            lEmailContent = lEmailContent + "<td width='13%'>" + "handphone (手机号码)" + " </td>";
                            lEmailContent = lEmailContent + "<td width='16%'>" + JobContent.SiteEngr_HP.Trim() + "</td>";
                            lEmailContent = lEmailContent + "<td width='13%'>" + "Email (电邮地址)" + "</td>";
                            lEmailContent = lEmailContent + "<td>" + JobContent.SiteEngr_Tel.Trim() + "</td></tr>";

                            lEmailContent = lEmailContent + "<tr><td width='20%'>" + "Goods Receiver (收货人)" + "</td>";
                            lEmailContent = lEmailContent + "<td width='15%'>" + JobContent.Scheduler_Name.Trim() + "</td>";
                            lEmailContent = lEmailContent + "<td width='13%'>" + "Handphone (手机号码)" + " </td>";
                            lEmailContent = lEmailContent + "<td width='16%'>" + JobContent.Scheduler_HP.Trim() + "</td>";
                            lEmailContent = lEmailContent + "<td width='13%'>" + "Email (电邮地址)" + "</td>";
                            lEmailContent = lEmailContent + "<td>" + JobContent.Scheduler_Tel.Trim() + "</td></tr></table>";

                            lEmailContent = lEmailContent + "<table border='1' width=100%><tr>";

                            lEmailContent = lEmailContent + "<td colspan='3'>" + "NatSteel Contacts (大众钢铁联系人) (Fax:62619133/62665153)" + "</td></tr>";

                            lEmailContent = lEmailContent + "<tr><td width='20%'>" + "Name (姓名)" + "</td>";
                            lEmailContent = lEmailContent + "<td width='15%'>" + "Contact Numbers (联系电话)" + "</td>";
                            lEmailContent = lEmailContent + "<td width='13%'>" + "Email Address (电邮地址)" + " </td></tr>";

                            var lProjContent = db.Project.Find(CustomerCode, ProjectCode);

                            if (lProjContent.Contact1 != null)
                            {
                                if (lProjContent.Contact1.Trim().Length > 0)
                                {
                                    lEmailContent = lEmailContent + "<tr><td>" + lProjContent.Contact1.Trim() + "</td>";
                                    lVar1 = "";
                                    if (lProjContent.Tel1 != null) if (lProjContent.Tel1.Trim().Length > 0) lVar1 = lProjContent.Tel1.Trim();
                                    lEmailContent = lEmailContent + "<td>" + lVar1 + "</td>";

                                    lVar1 = "";
                                    if (lProjContent.Email1 != null) if (lProjContent.Email1.Trim().Length > 0) lVar1 = lProjContent.Email1.Trim();
                                    lEmailContent = lEmailContent + "<td>" + lVar1 + " </td></tr>";
                                    if (lVar1 != "") if (lEmailTo == "") lEmailTo = lVar1; else lEmailTo = lEmailTo + ";" + lVar1;
                                }
                            }

                            if (lProjContent.Contact2 != null)
                            {
                                if (lProjContent.Contact2.Trim().Length > 0)
                                {
                                    lEmailContent = lEmailContent + "<tr><td>" + lProjContent.Contact2.Trim() + "</td>";
                                    lVar1 = "";
                                    if (lProjContent.Tel2 != null) if (lProjContent.Tel2.Trim().Length > 0) lVar1 = lProjContent.Tel2.Trim();
                                    lEmailContent = lEmailContent + "<td>" + lVar1 + "</td>";

                                    lVar1 = "";
                                    if (lProjContent.Email2 != null) if (lProjContent.Email2.Trim().Length > 0) lVar1 = lProjContent.Email2.Trim();
                                    lEmailContent = lEmailContent + "<td>" + lVar1 + " </td></tr>";
                                    if (lVar1 != "") if (lEmailTo == "") lEmailTo = lVar1; else lEmailTo = lEmailTo + ";" + lVar1;
                                }
                            }

                            if (lProjContent.Contact3 != null)
                            {
                                if (lProjContent.Contact3.Trim().Length > 0)
                                {
                                    lEmailContent = lEmailContent + "<tr><td>" + lProjContent.Contact3.Trim() + "</td>";
                                    lVar1 = "";
                                    if (lProjContent.Tel3 != null) if (lProjContent.Tel3.Trim().Length > 0) lVar1 = lProjContent.Tel3.Trim();
                                    lEmailContent = lEmailContent + "<td>" + lVar1 + "</td>";

                                    lVar1 = "";
                                    if (lProjContent.Email3 != null) if (lProjContent.Email3.Trim().Length > 0) lVar1 = lProjContent.Email3.Trim();
                                    lEmailContent = lEmailContent + "<td>" + lVar1 + " </td></tr>";
                                    if (lVar1 != "") if (lEmailTo == "") lEmailTo = lVar1; else lEmailTo = lEmailTo + ";" + lVar1;
                                }
                            }

                            if (lProjContent.Contact4 != null)
                            {
                                if (lProjContent.Contact4.Trim().Length > 0)
                                {
                                    lEmailContent = lEmailContent + "<tr><td>" + lProjContent.Contact4.Trim() + "</td>";
                                    lVar1 = "";
                                    if (lProjContent.Tel4 != null) if (lProjContent.Tel4.Trim().Length > 0) lVar1 = lProjContent.Tel4.Trim();
                                    lEmailContent = lEmailContent + "<td>" + lVar1 + "</td>";

                                    lVar1 = "";
                                    if (lProjContent.Email4 != null) if (lProjContent.Email4.Trim().Length > 0) lVar1 = lProjContent.Email4.Trim();
                                    lEmailContent = lEmailContent + "<td>" + lVar1 + " </td></tr>";
                                    if (lVar1 != "") if (lEmailTo == "") lEmailTo = lVar1; else lEmailTo = lEmailTo + ";" + lVar1;
                                }
                            }

                            if (lProjContent.Contact5 != null)
                            {
                                if (lProjContent.Contact5.Trim().Length > 0)
                                {
                                    lEmailContent = lEmailContent + "<tr><td>" + lProjContent.Contact5.Trim() + "</td>";
                                    lVar1 = "";
                                    if (lProjContent.Tel5 != null) if (lProjContent.Tel5.Trim().Length > 0) lVar1 = lProjContent.Tel5.Trim();
                                    lEmailContent = lEmailContent + "<td>" + lVar1 + "</td>";

                                    lVar1 = "";
                                    if (lProjContent.Email5 != null) if (lProjContent.Email5.Trim().Length > 0) lVar1 = lProjContent.Email5.Trim();
                                    lEmailContent = lEmailContent + "<td>" + lVar1 + " </td></tr>";
                                    if (lVar1 != "") if (lEmailTo == "") lEmailTo = lVar1; else lEmailTo = lEmailTo + ";" + lVar1;
                                }
                            }

                            if (lProjContent.Contact6 != null)
                            {
                                if (lProjContent.Contact6.Trim().Length > 0)
                                {
                                    lEmailContent = lEmailContent + "<tr><td>" + lProjContent.Contact6.Trim() + "</td>";
                                    lVar1 = "";
                                    if (lProjContent.Tel6 != null) if (lProjContent.Tel6.Trim().Length > 0) lVar1 = lProjContent.Tel6.Trim();
                                    lEmailContent = lEmailContent + "<td>" + lVar1 + "</td>";

                                    lVar1 = "";
                                    if (lProjContent.Email6 != null) if (lProjContent.Email6.Trim().Length > 0) lVar1 = lProjContent.Email6.Trim();
                                    lEmailContent = lEmailContent + "<td>" + lVar1 + " </td></tr>";
                                    if (lVar1 != "") if (lEmailTo == "") lEmailTo = lVar1; else lEmailTo = lEmailTo + ";" + lVar1;
                                }
                            }
                            lEmailContent = lEmailContent + "</table>";

                            lVar1 = JobContent.Scheduler_Tel.Trim();
                            if (lVar1 != "")
                            {
                                if (lEmailTo == "") { lEmailTo = lVar1; }
                                else { lEmailTo = lEmailTo + ";" + lVar1; }
                            }

                            lVar1 = JobContent.SiteEngr_Tel.Trim();
                            if (lVar1 != "" && lEmailTo.IndexOf(lVar1) < 0)
                            {
                                if (lEmailTo == "") { lEmailTo = lVar1; }
                                else { lEmailTo = lEmailTo + ";" + lVar1; }
                            }

                            if (JobContent.UpdateBy != null)
                            {
                                lVar1 = JobContent.UpdateBy.Trim();
                                if (lVar1 != "" && lEmailCc.IndexOf(lVar1) < 0)
                                {
                                    if (lEmailCc == "") { lEmailCc = lVar1; }
                                    else { lEmailCc = lEmailCc + ";" + lVar1; }
                                }
                            }

                            lVar = "";
                            if (lCustomer != null) lVar = lCustomer.CustomerName.Trim();

                            lEmailSubject = lVar + " - " + JobContent.PONumber.Trim() + " - Standard Products No. " + JobID.ToString();
                            //lEmailSubject = JobContent.PONumber.Trim() + " - " + lVar + " - MESH Standard Sheet (Class A) No. " + JobID.ToString();

                            var lOESEmail = new SendGridEmail();

                            string lEmailFromAddress = "eprompt@natsteel.com.sg";
                            string lEmailFromName = "Digital Ordering Email Services";

                            //dynamic response = lOESEmail.Execute(lEmailFromAddress, lEmailFromName, lEmailTo, lEmailCc, lEmailSubject, "text/html", lEmailContent);
                            lOESEmail.Execute(lEmailFromAddress, lEmailFromName, lEmailTo, lEmailCc, lEmailSubject, "text/html", lEmailContent).Wait();
                            lOESEmail = null;

                            return Json(true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lErrorMsg = ex.Message;
                return Json(false);
            }
            return Json(false);
        }



        //get BBS List at Order Summary
        [HttpPost]
        [Route("/getBBS_coupler/{CustomerCode}/{ProjectCode}/{JobID}")]
        //[ValidateAntiForgeryHeader]
        public ActionResult getBBS(string CustomerCode, string ProjectCode, int JobID)
        {

            var contentStd = (from p in db.StdProdDetails
                              where p.CustomerCode == CustomerCode &&
                              p.ProjectCode == ProjectCode &&
                              p.JobID == JobID
                              orderby p.SSID
                              select p).ToList();
            //return Json(new { STDPROD = contentStd }, JsonRequestBehavior.AllowGet);
            return Ok(contentStd);
        }

        //save BBS
        [HttpPost]
        [Route("/saveBBS_coupler")]

        //[ValidateAntiForgeryHeader]
        public ActionResult saveBBS([FromBody] CouplerHead couplerHead)
        {
            var lErrorMsg = "";
            string Message = "";
            try
            {
                db.Database.ExecuteSqlCommand("DELETE FROM OESStdProdDetails " +
                "WHERE CustomerCode = '" + couplerHead.CustomerCode + "' " +
                "AND ProjectCode = '" + couplerHead.ProjectCode + "' " +
                "AND JobID = " + couplerHead.JobID.ToString() + " ");

                if (couplerHead.StdProdDetails != null && couplerHead.StdProdDetails.Count > 0)
                {
                    for (int i = 0; i < couplerHead.StdProdDetails.Count; i++)
                    {
                        couplerHead.StdProdDetails[i].SSID = i + 1;
                        couplerHead.StdProdDetails[i].UpdateBy = User.Identity.Name;
                        couplerHead.StdProdDetails[i].UpdateDate = DateTime.Now;
                        couplerHead.StdProdDetails[i].CustomerCode = couplerHead.CustomerCode;
                        couplerHead.StdProdDetails[i].ProjectCode = couplerHead.ProjectCode;
                        couplerHead.StdProdDetails[i].JobID = couplerHead.JobID;
                        db.StdProdDetails.Add(couplerHead.StdProdDetails[i]);
                    }
                }
                db.SaveChanges();
                // return Json(new { success = true, responseText = "Successfully saved." }, JsonRequestBehavior.AllowGet);
                Message = "Data has been saved successfully.";
                return Ok(new { Message, response = "success" });
            }
            catch (Exception ex)
            {
                lErrorMsg = ex.Message;
            }
            //return Json(new { success = false, responseText = lErrorMsg }, JsonRequestBehavior.AllowGet);
            Message = lErrorMsg;
            return Ok(new { Message, response = "failure" });
        }

        //ValidatePONumber
        [HttpPost]
        [Route("/ValidatePONumber_coupler/{CustomerCode}/{ProjectCode}/{JobID}/{PONumber}")]
        //[ValidateAntiForgeryHeader]
        public ActionResult ValidatePONumber(string CustomerCode, string ProjectCode, int JobID, string PONumber)
        {
            var lErrorMsg = "";
            try
            {
                var lListOrder = "";
                var lJobAdvice = (from p in db.StdSheetJobAdvice
                                  where p.CustomerCode == CustomerCode &&
                                  p.ProjectCode == ProjectCode &&
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

                //   return Json(new { success = true, responseText = lListOrder }, JsonRequestBehavior.AllowGet);
                return Ok();
            }
            catch (Exception ex)
            {
                lErrorMsg = ex.Message;
                // return Json(new { success = false, responseText = lErrorMsg }, JsonRequestBehavior.AllowGet);
                return Ok();
            }

        }

        //get Bars List 
        [HttpGet]
        //[ValidateAntiForgeryHeader]
        [Route("/printOrderDetail_coupler/{CustomerCode}/{ProjectCode}/{JobID}")]
        public ActionResult printOrderDetail(string CustomerCode, string ProjectCode, int JobID)
        {
            Reports service = new Reports();
            int lPage = 0;
            var bPDF = service.rptStdMeshOrderDetails(CustomerCode, ProjectCode, JobID, "N", ref lPage);
            bPDF = service.rptStdMeshOrderDetails(CustomerCode, ProjectCode, JobID, "N", ref lPage);

            //var bPDF = service.rptOrderDetailsImage(CustomerCode, ProjectCode, JobID, "N", ref lPage);
            //bPDF = service.rptOrderDetailsImage(CustomerCode, ProjectCode, JobID, "N", ref lPage);

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

        [HttpGet]
        [Route("/GetPDF_coupler/{pHTML}")]
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


        [HttpGet]
        [Route("/Dispose_coupler/{disposing}")]
       
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}