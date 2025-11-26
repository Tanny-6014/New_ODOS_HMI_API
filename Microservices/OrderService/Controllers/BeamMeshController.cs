using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using OrderService.Models;
using Microsoft.AspNet.Identity;
using System.Data.SqlClient;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.IO;
using OrderService.Repositories;
using System.Globalization;
using System.Data.OleDb;
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
using System.Net.Http.Headers;

namespace OrderService.Controllers
{
   // [Authorize]
    public class BeamMeshController : Controller
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
        [Route("/Index_beam")]
        //[ValidateAntiForgeryToken]
        public ActionResult Index(string appCustomerCode, string appProjectCode, int appSelectedCount,
        string appSelectedSE, string appSelectedProd, string appSelectedPostID, string appSelectedScheduled,
        string appSelectedWBS1, string appSelectedWBS2, string appSelectedWBS3,
        string appSelectedWT, string appSelectedQty,
        string appWBS1S, string appWBS2S, string appWBS3S, string appOrderNoS,
        string appStructureElement, string appProductType, string appScheduledProd, int appPostID,
        string appOrderNo, string appWBS1, string appWBS2, string appWBS3)
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

            CustomerModels lCustomerModel = db.Customer.Find(appCustomerCode);

            ViewBag.CustomerSelection = new SelectList(new List<SelectListItem>
            { new SelectListItem
            {
                Value = appCustomerCode,
                Text = lCustomerModel == null? "":lCustomerModel.CustomerName
            }
            }, "Value", "Text");

            var lProjectModel = (from p in db.ProjectList
                                 where p.ProjectCode == appProjectCode
                                 select p).First();

            ViewBag.ProjectSelection = new SelectList(new List<SelectListItem>
            { new SelectListItem
            {
                Value = appProjectCode,
                Text = lProjectModel == null? "":lProjectModel.ProjectTitle
            }
            }, "Value", "Text");

            int lJobID = 0;
            string lOrderStatus = "New";
            string lTransport = "TR40/24";
            string lComponentName = "";

            int lOrderNoT = 0;
            int.TryParse(appOrderNo, out lOrderNoT);

            if (appScheduledProd != "E" && appScheduledProd != "S" && appScheduledProd != "W")
            {
                var lHeader = db.OrderProject.Find(lOrderNoT);
                if (lHeader != null)
                {
                    lOrderStatus = lHeader.OrderStatus;
                }
                if (lOrderStatus == null || lOrderStatus == "Reserved")
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
                    lJobID = lSE.MESHJobID;
                    lTransport = lSE.TransportMode;
                    if (lTransport == null || lTransport == "")
                    {
                        lTransport = "TR40/24";
                    }
                    lOrderStatus = lSE.OrderStatus;
                    if (lOrderStatus == null || lOrderStatus == "Reserved")
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
            }
            else
            {
                var lComp = db.OESComponent.Find(lOrderNoT);
                if (lComp != null)
                {
                    lJobID = (lComp.MeshJobID == null ? 0 : (int)lComp.MeshJobID);
                    lTransport = lComp.TransportMode;
                    lOrderStatus = lComp.Status;
                    if (lOrderStatus == null || lOrderStatus == "Reserved")
                    {
                        lOrderStatus = "New";
                    }
                    lComponentName = lComp.ComponentName;
                    //if (lUserName.Split('@').Length == 2)
                    //{
                    //    if (lOrderStatus == "Created*" && lUserName.Split('@')[1].ToLower().Trim() == "natsteel.com.sg")
                    //    {
                    //        lOrderStatus = "Created";
                    //    }
                    //}
                }
            }

            ////var lJobAdv = db.JobAdvice.Find(appCustomerCode, appProjectCode, lJobID);

            ////if (lJobAdv != null)
            ////{
            ////    lOrderStatus = lJobAdv.OrderStatus;
            ////}
            ////if (lOrderStatus == null || lOrderStatus == "Reserved")
            ////{
            ////    lOrderStatus = "New";
            ////}

            ViewBag.JobID = lJobID;

            ViewBag.OrderStatus = lOrderStatus;

            ViewBag.Transport = lTransport;

            ViewBag.ComponentName = lComponentName;

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
            ViewBag.OrderNo = appOrderNoS.Split(',');

            ViewBag.ScheduledProd = appScheduledProd;
            ViewBag.PostID = appPostID;

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

        [HttpGet]
        [Route("/Index_old_beam")]
        public ActionResult Index_old()
        {
            UserAccessController lUa = new UserAccessController();
            var lUserType = lUa.getUserType(User.Identity.GetUserName());
            var lGroupName = lUa.getGroupName(User.Identity.GetUserName());
            gUserType = lUserType;
            gGroupName = lGroupName;

            ViewBag.UserType = lUserType;
            lUa = null;

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
                content1.Insert(0, new CustomerModels
                {
                    CustomerCode = "",
                    CustomerName = ""
                });

                ViewBag.CustomerSelection = new SelectList(new List<SelectListItem>(content1.Select(h => new SelectListItem
                {
                    Value = h.CustomerCode.Trim(),
                    Text = h.CustomerName.Trim() + (h.CustomerCode.Trim() == "" ? "" : (" (" + h.CustomerCode.Trim() + ")"))
                })), "Value", "Text");

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
                        content = RemoveNonMESHProject(content);
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
            var lCustomerCK = Request.Cookies["nsh_digios_cust"];
            if (lCustomerCK != null)
            {
                //lCustomerCodeD = lCustomerCK.Value.ToString();
            }
            ViewBag.CustomerCodeCK = lCustomerCodeD;

            string lProjectCodeD = "";
            var lProjectCK = Request.Cookies["nsh_digios_proj"];
            if (lProjectCK != null)
            {
                //lProjectCodeD = lProjectCK.Value.ToString();
            }
            ViewBag.ProjectCodeCK = lProjectCodeD;

            return View(content.First());
        }


        [HttpPost]
        [Route("/uploadPODocs_beam")]
        // [ValidateAntiForgeryHeader]
        public ActionResult uploadPODocs()
        {
            //var lReturn = new CTSMESHPODocsFileModels();

            //try
            //{
            //    var lCustomerCode = Request.Form.Get("CustomerCode");
            //    var lProjectCode = Request.Form.Get("ProjectCode");
            //    int lJobID = 0;
            //    int lDocID = 0;
            //    int.TryParse(Request.Form.Get("JobID"), out lJobID);

            //    if (Request.Files.Count > 0)
            //    {
            //        HttpFileCollectionBase files = Request.Files;
            //        for (int i = 0; i < files.Count; i++)
            //        {
            //            HttpPostedFileBase file = files[i];

            //            var lFileName = file.FileName;
            //            if (lFileName.LastIndexOf("\\") >= 0)
            //            {
            //                lFileName = lFileName.Substring(lFileName.LastIndexOf("\\") + 1);
            //            }
            //            if (lFileName.LastIndexOf("/") >= 0)
            //            {
            //                lFileName = lFileName.Substring(lFileName.LastIndexOf("/") + 1);
            //            }

            //            byte[] pdfBytes = null;
            //            BinaryReader reader = new BinaryReader(file.InputStream);
            //            pdfBytes = reader.ReadBytes((int)file.ContentLength);

            //            lDocID = (from p in db.CTSMESHPODoc
            //                      where p.CustomerCode == lCustomerCode &&
            //                      p.ProjectCode == lProjectCode &&
            //                      p.JobID == lJobID
            //                      select p.PODocID).DefaultIfEmpty(0).Max();
            //            lDocID = lDocID + 1;

            //            var Content = new CTSMESHPODocsModels
            //            {
            //                CustomerCode = lCustomerCode,
            //                ProjectCode = lProjectCode,
            //                JobID = lJobID,
            //                PODocID = lDocID,
            //                FileName = lFileName,
            //                UpdatedDate = DateTime.Now,
            //                UpdatedBy = User.Identity.Name,
            //                PODoc = pdfBytes
            //            };
            //            db.CTSMESHPODoc.Add(Content);
            //            db.SaveChanges();

            //            var lPODocExists = db.CTSMESHPODoc.Where(
            //                                  p => p.CustomerCode == lCustomerCode &&
            //                                  p.ProjectCode == lProjectCode &&
            //                                  p.JobID == lJobID).Count();

            //            lReturn = new CTSMESHPODocsFileModels
            //            {
            //                PODocID = lDocID,
            //                FileName = lFileName,
            //                UpdatedDate = DateTime.Now.ToString("yyyy-mm-dd hh:mm:ss"),
            //                UpdatedBy = User.Identity.Name,
            //                FileSize = (pdfBytes.Length / 1000).ToString(),
            //                Exists = lPODocExists
            //            };
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ModelState.AddModelError(string.Empty, "Server Error: " + ex.Message);
            //}
            ////return Json(lReturn, JsonRequestBehavior.AllowGet);
            return Ok();
        }

        //download PO doc 
        [HttpPost]
        [Route("/deletePODocs_beam/{CustomerCode}/{ProjectCode}/{JobID}/{PODocID}")]
        //  [ValidateAntiForgeryHeader]
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
                    "DELETE FROM dbo.OESCTSMESHPODoc " +
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
            // return Json(lReturn, JsonRequestBehavior.AllowGet);
            return Ok(lReturn);
        }

        //download PO doc 
        [HttpPost]
        [Route("/downloadPODocs_beam/{CustomerCode}/{ProjectCode}/{JobID}/{PODocID}")]
        //  [ValidateAntiForgeryHeader]
        public ActionResult downloadPODocs(string CustomerCode, string ProjectCode, int JobID, int PODocID)
        {
            var content = db.CTSMESHPODoc.Find(CustomerCode, ProjectCode, JobID, PODocID);
            //return Json(content, JsonRequestBehavior.AllowGet);
            return Ok(content);
        }

        //check PO doc 
        [HttpPost]
        [Route("/checkPODocs_beam/{CustomerCode}/{ProjectCode}/{JobID}")]
        //  [ValidateAntiForgeryHeader]
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
                    "FROM dbo.OESCTSMESHPODoc " +
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
            //return Json(lReturn, JsonRequestBehavior.AllowGet);
            return Ok(lReturn);
        }

        [HttpGet]
        [Route("/getVarMaxValue_beam/{pValue}")]
        int getVarMaxValue(string pValue)
        {
            var rValue = 0;
            if (pValue != null)
            {
                if (!int.TryParse(pValue, out rValue))
                {
                    var lVarLen = pValue.Split('-');
                    if (lVarLen.Length == 2)
                    {
                        var lVar1 = 0;
                        var lVar2 = 0;
                        int.TryParse(lVarLen[0], out lVar1);
                        int.TryParse(lVarLen[1], out lVar2);
                        if (lVar1 > lVar2)
                        {
                            rValue = lVar1;
                        }
                        else
                        {
                            rValue = lVar2;
                        }
                    }
                    else
                    {
                        rValue = 0;
                    }
                }
            }
            return rValue;
        }

        [HttpGet]
        [Route("/getVarMinValue_beam/{pValue}")]
        int getVarMinValue(string pValue)
        {
            var rValue = 0;
            if (pValue != null)
            {
                if (!int.TryParse(pValue, out rValue))
                {
                    var lVarLen = pValue.Split('-');
                    if (lVarLen.Length == 2)
                    {
                        var lVar1 = 0;
                        var lVar2 = 0;
                        int.TryParse(lVarLen[0], out lVar1);
                        int.TryParse(lVarLen[1], out lVar2);
                        if (lVar1 > lVar2)
                        {
                            rValue = lVar2;
                        }
                        else
                        {
                            rValue = lVar1;
                        }
                    }
                    else
                    {
                        rValue = 0;
                    }
                }
            }
            return rValue;
        }

        [HttpGet]
        [Route("/isInvalidParameterCell_beam/{pShapeCode}/{pColumnName}/{pParameters}")]
        bool isInvalidParameterCell(string pShapeCode, string pColumnName, string pParameters)
        {
            var lReturn = false;
            if (pShapeCode != null && pShapeCode != "" && pParameters != null)
            {
                if ((pColumnName == "A" && pParameters.IndexOf("A") < 0) ||
                (pColumnName == "B" && pParameters.IndexOf("B") < 0) ||
                (pColumnName == "C" && pParameters.IndexOf("C") < 0) ||
                (pColumnName == "D" && pParameters.IndexOf("D") < 0) ||
                (pColumnName == "E" && pParameters.IndexOf("E") < 0) ||
                (pColumnName == "F" && pParameters.IndexOf("F") < 0) ||
                (pColumnName == "G" && pParameters.IndexOf("G") < 0) ||
                (pColumnName == "H" && pParameters.IndexOf("H") < 0) ||
                (pColumnName == "I" && pParameters.IndexOf("I") < 0) ||
                (pColumnName == "J" && pParameters.IndexOf("J") < 0) ||
                (pColumnName == "K" && pParameters.IndexOf("K") < 0) ||
                (pColumnName == "L" && pParameters.IndexOf("L") < 0) ||
                (pColumnName == "M" && pParameters.IndexOf("M") < 0) ||
                (pColumnName == "N" && pParameters.IndexOf("N") < 0) ||
                (pColumnName == "O" && pParameters.IndexOf("O") < 0) ||
                (pColumnName == "P" && pParameters.IndexOf("P") < 0) ||
                (pColumnName == "Q" && pParameters.IndexOf("Q") < 0) ||
                (pColumnName == "R" && pParameters.IndexOf("R") < 0) ||
                (pColumnName == "S" && pParameters.IndexOf("S") < 0) ||
                (pColumnName == "T" && pParameters.IndexOf("T") < 0) ||
                (pColumnName == "U" && pParameters.IndexOf("U") < 0) ||
                (pColumnName == "V" && pParameters.IndexOf("V") < 0) ||
                (pColumnName == "W" && pParameters.IndexOf("W") < 0) ||
                (pColumnName == "X" && pParameters.IndexOf("X") < 0) ||
                (pColumnName == "Y" && pParameters.IndexOf("Y") < 0) ||
                (pColumnName == "Z" && pParameters.IndexOf("Z") < 0))
                {
                    lReturn = true;
                }
            }
            return lReturn;
        }

        [HttpGet]
        [Route("/calTotalLen_beam/{pItem}/{pshapeLengthFormula}/{pValue}")]
        string calTotalLen(OrderDetailsModels pItem, string pshapeLengthFormula, string pValue)
        {
            var lItem = pItem;
            var lF = pshapeLengthFormula;
            var lResult = "0";
            var lVarMax = 0;
            var lVarMin = 0;
            if (lF != "")
            {
                if (lF.IndexOf("A") >= 0) if (lItem.A != null) lF = lF.Replace("A", getVarMaxValue(lItem.A.ToString()).ToString()); else lF = lF.Replace("A", "0");
                if (lF.IndexOf("B") >= 0) if (lItem.B != null) lF = lF.Replace("B", getVarMaxValue(lItem.B.ToString()).ToString()); else lF = lF.Replace("B", "0");
                if (lF.IndexOf("C") >= 0) if (lItem.C != null) lF = lF.Replace("C", getVarMaxValue(lItem.C.ToString()).ToString()); else lF = lF.Replace("C", "0");
                if (lF.IndexOf("D") >= 0) if (lItem.D != null) lF = lF.Replace("D", getVarMaxValue(lItem.D.ToString()).ToString()); else lF = lF.Replace("D", "0");
                if (lF.IndexOf("E") >= 0) if (lItem.E != null) lF = lF.Replace("E", getVarMaxValue(lItem.E.ToString()).ToString()); else lF = lF.Replace("E", "0");
                if (lF.IndexOf("F") >= 0) if (lItem.F != null) lF = lF.Replace("F", getVarMaxValue(lItem.F.ToString()).ToString()); else lF = lF.Replace("F", "0");
                if (lF.IndexOf("G") >= 0) if (lItem.G != null) lF = lF.Replace("G", getVarMaxValue(lItem.G.ToString()).ToString()); else lF = lF.Replace("G", "0");
                if (lF.IndexOf("H") >= 0) if (lItem.H != null) lF = lF.Replace("H", getVarMaxValue(lItem.H.ToString()).ToString()); else lF = lF.Replace("H", "0");
                if (lF.IndexOf("I") >= 0) if (lItem.I != null) lF = lF.Replace("I", getVarMaxValue(lItem.I.ToString()).ToString()); else lF = lF.Replace("I", "0");
                if (lF.IndexOf("J") >= 0) if (lItem.J != null) lF = lF.Replace("J", getVarMaxValue(lItem.J.ToString()).ToString()); else lF = lF.Replace("J", "0");
                if (lF.IndexOf("K") >= 0) if (lItem.K != null) lF = lF.Replace("K", getVarMaxValue(lItem.K.ToString()).ToString()); else lF = lF.Replace("K", "0");
                if (lF.IndexOf("L") >= 0) if (lItem.L != null) lF = lF.Replace("L", getVarMaxValue(lItem.L.ToString()).ToString()); else lF = lF.Replace("L", "0");
                if (lF.IndexOf("M") >= 0) if (lItem.M != null) lF = lF.Replace("M", getVarMaxValue(lItem.M.ToString()).ToString()); else lF = lF.Replace("M", "0");
                if (lF.IndexOf("N") >= 0) if (lItem.N != null) lF = lF.Replace("N", getVarMaxValue(lItem.N.ToString()).ToString()); else lF = lF.Replace("N", "0");
                if (lF.IndexOf("O") >= 0) if (lItem.O != null) lF = lF.Replace("O", getVarMaxValue(lItem.O.ToString()).ToString()); else lF = lF.Replace("O", "0");
                if (lF.IndexOf("P") >= 0) if (lItem.P != null) lF = lF.Replace("P", getVarMaxValue(lItem.P.ToString()).ToString()); else lF = lF.Replace("P", "0");
                if (lF.IndexOf("Q") >= 0) if (lItem.Q != null) lF = lF.Replace("Q", getVarMaxValue(lItem.Q.ToString()).ToString()); else lF = lF.Replace("Q", "0");
                if (lF.IndexOf("R") >= 0) if (lItem.R != null) lF = lF.Replace("R", getVarMaxValue(lItem.R.ToString()).ToString()); else lF = lF.Replace("R", "0");
                if (lF.IndexOf("S") >= 0) if (lItem.S != null) lF = lF.Replace("S", getVarMaxValue(lItem.S.ToString()).ToString()); else lF = lF.Replace("S", "0");
                if (lF.IndexOf("T") >= 0) if (lItem.T != null) lF = lF.Replace("T", getVarMaxValue(lItem.T.ToString()).ToString()); else lF = lF.Replace("T", "0");
                if (lF.IndexOf("U") >= 0) if (lItem.U != null) lF = lF.Replace("U", getVarMaxValue(lItem.U.ToString()).ToString()); else lF = lF.Replace("U", "0");
                if (lF.IndexOf("V") >= 0) if (lItem.V != null) lF = lF.Replace("V", getVarMaxValue(lItem.V.ToString()).ToString()); else lF = lF.Replace("V", "0");
                if (lF.IndexOf("W") >= 0) if (lItem.W != null) lF = lF.Replace("W", getVarMaxValue(lItem.W.ToString()).ToString()); else lF = lF.Replace("W", "0");
                if (lF.IndexOf("X") >= 0) if (lItem.X != null) lF = lF.Replace("X", getVarMaxValue(lItem.X.ToString()).ToString()); else lF = lF.Replace("X", "0");
                if (lF.IndexOf("Y") >= 0) if (lItem.Y != null) lF = lF.Replace("Y", getVarMaxValue(lItem.Y.ToString()).ToString()); else lF = lF.Replace("Y", "0");
                if (lF.IndexOf("Z") >= 0) if (lItem.Z != null) lF = lF.Replace("Z", getVarMaxValue(lItem.Z.ToString()).ToString()); else lF = lF.Replace("Z", "0");
                if (lF.IndexOf("math") >= 0)
                {
                    lF = lF.Replace("math.", "");
                }
                if (lF.IndexOf("pi") >= 0)
                {
                    lF = lF.Replace("pi", "Pi");
                }
                if (lF.IndexOf("sqrt") >= 0)
                {
                    lF = lF.Replace("sqrt", "Sqrt");
                }

                if (lF.IndexOf("sin") >= 0)
                {
                    lF = lF.Replace("sin", "Sin");
                }

                if (lF.IndexOf("cos") >= 0)
                {
                    lF = lF.Replace("cos", "Cos");
                }

                if (lF.IndexOf("max") >= 0)
                {
                    lF = lF.Replace("max", "Max");
                }

                if (lF.IndexOf("min") >= 0)
                {
                    lF = lF.Replace("min", "Min");
                }
                Expression lExp = new Expression(lF);

                if (!lExp.HasErrors())
                {
                    var lSResult = lExp.Evaluate().ToString();
                    if (lSResult.IndexOf('.') > 0) lSResult = lSResult.Substring(0, lSResult.IndexOf('.'));
                    int.TryParse(lSResult, out lVarMax);
                }

                lF = pshapeLengthFormula;
                if (lF.IndexOf("A") >= 0) if (lItem.A != null) lF = lF.Replace("A", getVarMinValue(lItem.A.ToString()).ToString()); else lF = lF.Replace("A", "0");
                if (lF.IndexOf("B") >= 0) if (lItem.B != null) lF = lF.Replace("B", getVarMinValue(lItem.B.ToString()).ToString()); else lF = lF.Replace("B", "0");
                if (lF.IndexOf("C") >= 0) if (lItem.C != null) lF = lF.Replace("C", getVarMinValue(lItem.C.ToString()).ToString()); else lF = lF.Replace("C", "0");
                if (lF.IndexOf("D") >= 0) if (lItem.D != null) lF = lF.Replace("D", getVarMinValue(lItem.D.ToString()).ToString()); else lF = lF.Replace("D", "0");
                if (lF.IndexOf("E") >= 0) if (lItem.E != null) lF = lF.Replace("E", getVarMinValue(lItem.E.ToString()).ToString()); else lF = lF.Replace("E", "0");
                if (lF.IndexOf("F") >= 0) if (lItem.F != null) lF = lF.Replace("F", getVarMinValue(lItem.F.ToString()).ToString()); else lF = lF.Replace("F", "0");
                if (lF.IndexOf("G") >= 0) if (lItem.G != null) lF = lF.Replace("G", getVarMinValue(lItem.G.ToString()).ToString()); else lF = lF.Replace("G", "0");
                if (lF.IndexOf("H") >= 0) if (lItem.H != null) lF = lF.Replace("H", getVarMinValue(lItem.H.ToString()).ToString()); else lF = lF.Replace("H", "0");
                if (lF.IndexOf("I") >= 0) if (lItem.I != null) lF = lF.Replace("I", getVarMinValue(lItem.I.ToString()).ToString()); else lF = lF.Replace("I", "0");
                if (lF.IndexOf("J") >= 0) if (lItem.J != null) lF = lF.Replace("J", getVarMinValue(lItem.J.ToString()).ToString()); else lF = lF.Replace("J", "0");
                if (lF.IndexOf("K") >= 0) if (lItem.K != null) lF = lF.Replace("K", getVarMinValue(lItem.K.ToString()).ToString()); else lF = lF.Replace("K", "0");
                if (lF.IndexOf("L") >= 0) if (lItem.L != null) lF = lF.Replace("L", getVarMinValue(lItem.L.ToString()).ToString()); else lF = lF.Replace("L", "0");
                if (lF.IndexOf("M") >= 0) if (lItem.M != null) lF = lF.Replace("M", getVarMinValue(lItem.M.ToString()).ToString()); else lF = lF.Replace("M", "0");
                if (lF.IndexOf("N") >= 0) if (lItem.N != null) lF = lF.Replace("N", getVarMinValue(lItem.N.ToString()).ToString()); else lF = lF.Replace("N", "0");
                if (lF.IndexOf("O") >= 0) if (lItem.O != null) lF = lF.Replace("O", getVarMinValue(lItem.O.ToString()).ToString()); else lF = lF.Replace("O", "0");
                if (lF.IndexOf("P") >= 0) if (lItem.P != null) lF = lF.Replace("P", getVarMinValue(lItem.P.ToString()).ToString()); else lF = lF.Replace("P", "0");
                if (lF.IndexOf("Q") >= 0) if (lItem.Q != null) lF = lF.Replace("Q", getVarMinValue(lItem.Q.ToString()).ToString()); else lF = lF.Replace("Q", "0");
                if (lF.IndexOf("R") >= 0) if (lItem.R != null) lF = lF.Replace("R", getVarMinValue(lItem.R.ToString()).ToString()); else lF = lF.Replace("R", "0");
                if (lF.IndexOf("S") >= 0) if (lItem.S != null) lF = lF.Replace("S", getVarMinValue(lItem.S.ToString()).ToString()); else lF = lF.Replace("S", "0");
                if (lF.IndexOf("T") >= 0) if (lItem.T != null) lF = lF.Replace("T", getVarMinValue(lItem.T.ToString()).ToString()); else lF = lF.Replace("T", "0");
                if (lF.IndexOf("U") >= 0) if (lItem.U != null) lF = lF.Replace("U", getVarMinValue(lItem.U.ToString()).ToString()); else lF = lF.Replace("U", "0");
                if (lF.IndexOf("V") >= 0) if (lItem.V != null) lF = lF.Replace("V", getVarMinValue(lItem.V.ToString()).ToString()); else lF = lF.Replace("V", "0");
                if (lF.IndexOf("W") >= 0) if (lItem.W != null) lF = lF.Replace("W", getVarMinValue(lItem.W.ToString()).ToString()); else lF = lF.Replace("W", "0");
                if (lF.IndexOf("X") >= 0) if (lItem.X != null) lF = lF.Replace("X", getVarMinValue(lItem.X.ToString()).ToString()); else lF = lF.Replace("X", "0");
                if (lF.IndexOf("Y") >= 0) if (lItem.Y != null) lF = lF.Replace("Y", getVarMinValue(lItem.Y.ToString()).ToString()); else lF = lF.Replace("Y", "0");
                if (lF.IndexOf("Z") >= 0) if (lItem.Z != null) lF = lF.Replace("Z", getVarMinValue(lItem.Z.ToString()).ToString()); else lF = lF.Replace("Z", "0");

                if (lF.IndexOf("math") >= 0)
                {
                    lF = lF.Replace("math.", "");
                }
                if (lF.IndexOf("pi") >= 0)
                {
                    lF = lF.Replace("pi", "PI");
                }
                if (lF.IndexOf("pi") >= 0)
                {
                    lF = lF.Replace("pi", "Pi");
                }
                if (lF.IndexOf("sqrt") >= 0)
                {
                    lF = lF.Replace("sqrt", "Sqrt");
                }

                if (lF.IndexOf("sin") >= 0)
                {
                    lF = lF.Replace("sin", "Sin");
                }

                if (lF.IndexOf("cos") >= 0)
                {
                    lF = lF.Replace("cos", "Cos");
                }

                if (lF.IndexOf("max") >= 0)
                {
                    lF = lF.Replace("max", "Max");
                }

                if (lF.IndexOf("min") >= 0)
                {
                    lF = lF.Replace("min", "Min");
                }

                lExp = new Expression(lF);
                if (!lExp.HasErrors())
                {
                    var lSResult = lExp.Evaluate().ToString();
                    if (lSResult.IndexOf('.') > 0) lSResult = lSResult.Substring(0, lSResult.IndexOf('.'));
                    int.TryParse(lSResult, out lVarMin);
                }

                if (lVarMin == lVarMax)
                {
                    lResult = lVarMax.ToString();
                }
                else
                {
                    lResult = lVarMin.ToString() + "-" + lVarMax.ToString();
                }
            }
            return lResult;
        }

        [HttpGet]
        [Route("/calWeight_beam/{pItem}")]
        double calWeight(OrderDetailsModels pItem)
        {
            int[] lDia = { 6, 8, 10, 12, 13, 16, 20, 24, 25, 28, 32, 36, 40, 50 };
            double[] lUnitWT = { 0.222, 0.395, 0.617, 0.888, 1.042, 1.579, 2.466, 3.699, 3.854, 4.834, 6.313, 7.769, 9.864, 15.413 };
            double lWeight = 0;
            double lKGM = 0;
            var lBarLength = pItem.BarLength;

            if (pItem.BarTotalQty != null && pItem.BarSize != null)
            {
                for (var i = 0; i < lDia.Length; i++)
                {
                    if (pItem.BarSize == lDia[i])
                    {
                        lKGM = lUnitWT[i];
                        break;
                    }
                }
                if (lKGM > 0)
                {
                    if (lBarLength != null)
                    {
                        if (lBarLength != null)
                        {
                            lWeight = Math.Round(lKGM * (double)pItem.BarTotalQty * (getVarMinValue(lBarLength.ToString()) + getVarMaxValue(lBarLength.ToString())) / 2) / 1000;
                        }
                    }
                    else
                    {
                        lWeight = Math.Round(lKGM * (int)pItem.BarTotalQty * (int)lBarLength) / 1000;
                    }
                }
            }

            return lWeight;
        }

        //[HttpPost]

        //[Route("/ExcelImport_beam/{CustomerCode}/{ProjectCode}/{JobID}/{BBSID}")]
        ////[ValidateAntiForgeryHeader]
        //public ActionResult ExcelImport(string CustomerCode, string ProjectCode, int JobID, int BBSID)
        //{
        //    try
        //    {
        //        string lUserType = "";
        //        if (gUserType != null && gUserType != "")
        //        {
        //            lUserType = gUserType;
        //        }
        //        else
        //        {
        //            UserAccessController lUa = new UserAccessController();
        //            lUserType = lUa.getUserType(User.Identity.GetUserName());
        //            ViewBag.UserType = lUserType;
        //            lUa = null;
        //        }

        //        //HttpPostedFileBase file = Request.Files["ExcelImport"];

        //        if (file.ContentLength > 0 && (Path.GetExtension(file.FileName) == ".xlsx" || Path.GetExtension(file.FileName) == ".xlsm"))
        //        {
        //            MemoryStream lStream = new MemoryStream();
        //            file.InputStream.CopyTo(lStream);
        //            ExcelPackage package = new ExcelPackage(lStream);

        //            ExcelWorksheet workSheet = package.Workbook.Worksheets.FirstOrDefault(f => f.View.TabSelected);
        //            DataTable lDataTable = new DataTable();

        //            if (Path.GetExtension(file.FileName) == ".xlsx" || Path.GetExtension(file.FileName) == ".xlsm")
        //            {
        //                if (package.Workbook.Worksheets.Count > 1)
        //                {
        //                    int lFound = 0;
        //                    int lCount = 0;

        //                    for (int i = 1; i <= package.Workbook.Worksheets.Count; i++)
        //                    {
        //                        if (package.Workbook.Worksheets[i].Name == "OrderSummary")
        //                        {
        //                            for (int j = 1; j < package.Workbook.Worksheets.Count; j++)
        //                            {
        //                                string lBBS = "";
        //                                if (package.Workbook.Worksheets[i].Dimension.End.Row >= 18 + j
        //                                    && package.Workbook.Worksheets[i].Dimension.End.Column >= 2
        //                                    && package.Workbook.Worksheets[i].Cells[18 + j, 2].Value != null)
        //                                {
        //                                    lBBS = package.Workbook.Worksheets[i].Cells[18 + j, 2].Value.ToString();
        //                                }
        //                                if (lBBS != null && lBBS != "" && package.Workbook.Worksheets["OrderDetail-" + lBBS] != null)
        //                                {
        //                                    workSheet = package.Workbook.Worksheets["OrderDetail-" + lBBS];
        //                                    lCount = lCount + 1;
        //                                    lFound = 1;
        //                                }

        //                            }
        //                            break;
        //                        }

        //                    }
        //                    if (lCount > 1)
        //                    {
        //                        lFound = 0;
        //                        var lBBSCon = db.BBS.Find(CustomerCode, ProjectCode, JobID, BBSID);
        //                        if (lBBSCon != null)
        //                        {
        //                            if (package.Workbook.Worksheets["OrderDetail-" + lBBSCon.BBSNo.Trim()] != null)
        //                            {
        //                                workSheet = package.Workbook.Worksheets["OrderDetail-" + lBBSCon.BBSNo.Trim()];
        //                                lFound = 1;
        //                            }
        //                        }

        //                        if (lFound == 0)
        //                        {
        //                            return Json(new { success = false, error = true, message = "Cannot find the BBS " + lBBSCon.BBSNo.Trim() }, JsonRequestBehavior.AllowGet);
        //                        }
        //                    }
        //                    if (lFound == 1)
        //                    {
        //                        workSheet.DeleteRow(1, 6);
        //                        if (workSheet.Dimension.End.Column >= 27)
        //                        {
        //                            workSheet.DeleteColumn(27, 1);
        //                        }
        //                    }
        //                }
        //            }

        //            //create columns
        //            foreach (var firstRowCell in workSheet.Cells[1, 1, 1, workSheet.Dimension.End.Column])
        //            {
        //                var lTitle = firstRowCell.Text;
        //                if (lTitle == null) { lTitle = "(No Title)"; }
        //                lDataTable.Columns.Add(lTitle, typeof(string));
        //            }

        //            for (var rowNumber = 1; rowNumber <= workSheet.Dimension.End.Row; rowNumber++)
        //            {
        //                var row = workSheet.Cells[rowNumber, 1, rowNumber, workSheet.Dimension.End.Column];
        //                while (row.Count() > lDataTable.Columns.Count)
        //                {
        //                    lDataTable.Columns.Add("(No Title)", typeof(string));
        //                }
        //                var newRow = lDataTable.NewRow();
        //                foreach (var cell in row)
        //                {
        //                    newRow[cell.Start.Column - 1] = cell.Text;
        //                }
        //                lDataTable.Rows.Add(newRow);
        //            }

        //            // Delete last white rows
        //            if (lDataTable.Rows.Count > 0)
        //            {
        //                for (int i = lDataTable.Rows.Count - 1; i >= 0; i--)
        //                {
        //                    var lFound = 0;
        //                    for (int j = 0; j < lDataTable.Columns.Count; j++)
        //                    {
        //                        if (lDataTable.Rows[i].ItemArray[j] != null)
        //                        {
        //                            if (lDataTable.Rows[i].ItemArray[j].ToString().Trim() != "" && lDataTable.Rows[i].ItemArray[j].ToString().Trim() != "0")
        //                            {
        //                                lFound = 1;
        //                                break;
        //                            }
        //                        }
        //                    }
        //                    if (lFound == 0)
        //                    {
        //                        lDataTable.Rows.RemoveAt(i);
        //                    }
        //                    else
        //                    {
        //                        break;
        //                    }
        //                }
        //            }


        //            if (lDataTable.Rows.Count > 0)
        //            {
        //                // clear null
        //                for (int i = 0; i < lDataTable.Rows.Count; i++)
        //                {
        //                    for (int j = 0; j < lDataTable.Columns.Count; j++)
        //                    {
        //                        if (lDataTable.Rows[i].ItemArray[j] == null)
        //                        {
        //                            lDataTable.Rows[i].ItemArray[j] = "";
        //                        }
        //                    }
        //                }
        //                //find the header row if it has
        //                var lColStart = 0;
        //                var lRowStart = 0;
        //                var lHeaderRows = 0;
        //                if (lDataTable.Rows.Count >= 2 && lDataTable.Columns.Count > 7)
        //                {
        //                    for (int i = 0; i < lDataTable.Rows.Count; i++)
        //                    {
        //                        for (int j = 0; j < lDataTable.Columns.Count - 1; j++)
        //                        {
        //                            if (lDataTable.Rows[i].ItemArray[j].ToString().Trim().ToUpper() == "A" && lDataTable.Rows[i].ItemArray[j + 1].ToString().Trim().ToUpper() == "B")
        //                            {
        //                                lRowStart = i + 1;
        //                                lColStart = j;
        //                                lHeaderRows = i;
        //                            }
        //                        }
        //                    }
        //                }

        //                // Checking the Imgage and taken out 
        //                if (lDataTable.Columns.Count > 9)
        //                {
        //                    var lFound = 0;
        //                    for (int i = lRowStart; i < lDataTable.Rows.Count; i++)
        //                    {
        //                        var liVar = 0;
        //                        if (lDataTable.Rows[i].ItemArray[8] != null &&
        //                        lDataTable.Rows[i].ItemArray[8].ToString().Trim() != "" &&
        //                        int.TryParse(lDataTable.Rows[i].ItemArray[8].ToString(), out liVar))
        //                        {
        //                            lFound = 1;
        //                            break;
        //                        }
        //                    }

        //                    if (lFound == 0)
        //                    {
        //                        lDataTable.Columns.RemoveAt(8);
        //                        if (lColStart > 8)
        //                        {
        //                            lColStart = lColStart - 1;
        //                        }
        //                    }
        //                }

        //                //prepare to insert record
        //                //1. get barID
        //                int? lBarID = 0;
        //                lBarID = db.OrderDetails.Where(z => z.CustomerCode == CustomerCode &&
        //                    z.ProjectCode == ProjectCode && z.JobID == JobID &&
        //                    z.BBSID == BBSID).Max(z => (int?)z.BarID);
        //                if (lBarID == null)
        //                {
        //                    lBarID = 0;
        //                }

        //                //2. get count of rows in the BBS
        //                int lRows = 0;
        //                lRows = db.OrderDetails.Count(z => z.CustomerCode == CustomerCode &&
        //                    z.ProjectCode == ProjectCode && z.JobID == JobID &&
        //                    z.BBSID == BBSID);

        //                for (int i = lRowStart; i < lDataTable.Rows.Count; i++)
        //                {
        //                    var lDia = ",10,13,16,20,24,25,28,32,36,40,50";
        //                    var lRDia = ",6,8,10,13,16,20";

        //                    lBarID = lBarID + 1;
        //                    lRows = lRows + 1;
        //                    int lSort = lRows * 1000;
        //                    string lBarType = lDataTable.Rows[i].ItemArray[2].ToString();
        //                    if (lBarType != null)
        //                    {
        //                        lBarType = lBarType.Trim();
        //                        if (lBarType != "" && lBarType != "T" && lBarType != "R" && lBarType != "H" && lBarType != "X") lBarType = "T";
        //                    }
        //                    var lBarDia = lDataTable.Rows[i].ItemArray[3].ToString();
        //                    lBarDia = lBarDia.Trim();
        //                    if (lBarType == "R")
        //                    {
        //                        if (lRDia.IndexOf(lBarDia) < 0) lBarDia = null;
        //                    }
        //                    else
        //                    {
        //                        if (lDia.IndexOf(lBarDia) < 0) lBarDia = null;
        //                    }

        //                    short lBarDiaInt = 0;
        //                    short.TryParse(lBarDia, out lBarDiaInt);

        //                    int lMQty = 0;
        //                    int.TryParse(lDataTable.Rows[i].ItemArray[4].ToString(), out lMQty);

        //                    int lEQty = 0;
        //                    int.TryParse(lDataTable.Rows[i].ItemArray[5].ToString(), out lEQty);

        //                    int lTQty = lMQty * lEQty;

        //                    var lShapeCode = lDataTable.Rows[i].ItemArray[7].ToString();
        //                    var lshapeParameters = "";
        //                    var lshapeLengthFormula = "";
        //                    var lshapeParaValidator = "";
        //                    var lshapeTransportValidator = "";
        //                    if (lShapeCode != null && lShapeCode.Trim() != "")
        //                    {
        //                        var lShapes = (from p in db.Shape where p.shapeCode == lShapeCode select p).ToList();
        //                        if (lShapes.Count == 0)
        //                        {
        //                            lShapeCode = "";
        //                        }
        //                        else
        //                        {
        //                            lshapeParameters = lShapes[0].shapeParameters;
        //                            lshapeLengthFormula = lShapes[0].shapeLengthFormula;
        //                            lshapeParaValidator = lShapes[0].shapeParaValidator;
        //                            lshapeTransportValidator = lShapes[0].shapeTransportValidator;
        //                        }
        //                    }

        //                    var orderDetailsModels = new OrderDetailsModels
        //                    {
        //                        CustomerCode = CustomerCode,
        //                        ProjectCode = ProjectCode,
        //                        JobID = JobID,
        //                        BBSID = BBSID,
        //                        BarID = (int)lBarID,
        //                        BarSort = lSort,
        //                        Cancelled = null,
        //                        BarCAB = true,
        //                        BarSTD = null,
        //                        ElementMark = lDataTable.Rows[i].ItemArray[0].ToString(),
        //                        BarMark = lDataTable.Rows[i].ItemArray[1].ToString(),
        //                        BarType = lBarType,
        //                        BarSize = lBarDiaInt == 0 ? (short?)null : lBarDiaInt,
        //                        BarMemberQty = lMQty == 0 ? (int?)null : lMQty,
        //                        BarEachQty = lEQty == 0 ? (int?)null : lEQty,
        //                        BarTotalQty = lTQty == 0 ? (int?)null : lTQty,
        //                        BarShapeCode = lShapeCode,
        //                        A = null,
        //                        B = null,
        //                        C = null,
        //                        D = null,
        //                        E = null,
        //                        F = null,
        //                        G = null,
        //                        H = null,
        //                        I = null,
        //                        J = null,
        //                        K = null,
        //                        L = null,
        //                        M = null,
        //                        N = null,
        //                        O = null,
        //                        P = null,
        //                        Q = null,
        //                        R = null,
        //                        S = null,
        //                        T = null,
        //                        U = null,
        //                        V = null,
        //                        W = null,
        //                        X = null,
        //                        Y = null,
        //                        Z = null,
        //                        A2 = null,
        //                        B2 = null,
        //                        C2 = null,
        //                        D2 = null,
        //                        E2 = null,
        //                        F2 = null,
        //                        G2 = null,
        //                        H2 = null,
        //                        I2 = null,
        //                        J2 = null,
        //                        K2 = null,
        //                        L2 = null,
        //                        M2 = null,
        //                        N2 = null,
        //                        O2 = null,
        //                        P2 = null,
        //                        Q2 = null,
        //                        R2 = null,
        //                        S2 = null,
        //                        T2 = null,
        //                        U2 = null,
        //                        V2 = null,
        //                        W2 = null,
        //                        X2 = null,
        //                        Y2 = null,
        //                        Z2 = null,
        //                        BarLength = null,
        //                        BarLength2 = null,
        //                        BarWeight = null,
        //                        Remarks = "",
        //                        shapeTransport = 0,
        //                        UpdateDate = DateTime.Now
        //                    };

        //                    if (lShapeCode.Trim() != "")
        //                    {
        //                        if (lRowStart > 0 && lColStart > 0)
        //                        {
        //                            //HAVE HEADER ROW
        //                            for (int j = lColStart; j < lDataTable.Columns.Count; j++)
        //                            {
        //                                string lValue = lDataTable.Rows[i].ItemArray[j].ToString();

        //                                if (isInvalidParameterCell(lShapeCode,
        //                                    lDataTable.Rows[lHeaderRows].ItemArray[j].ToString().Trim(),
        //                                    lshapeParameters) == true)
        //                                {
        //                                    lValue = null;
        //                                }

        //                                var lErrorMsg = "";
        //                                //if (isValidValue(lDataTable.Rows[lHeaderRows].ItemArray[j].ToString(),
        //                                //lshapeParameters,
        //                                //lshapeParaValidator,
        //                                //lBarDiaInt,
        //                                //lValue,
        //                                //orderDetailsModels,
        //                                //out lErrorMsg) == false)
        //                                //{
        //                                //    lValue = null;
        //                                //}

        //                                var lTotalLength = calTotalLen(
        //                                    orderDetailsModels,
        //                                    lshapeLengthFormula,
        //                                    lValue);
        //                                // validate the Total Length
        //                                var lMaxLength = getVarMaxValue(lTotalLength);
        //                                if (lMaxLength > 12000)
        //                                {
        //                                    lValue = null;
        //                                }
        //                                else
        //                                {
        //                                    orderDetailsModels.BarLength = getParaValue(lTotalLength, 1);
        //                                    orderDetailsModels.BarLength2 = getParaValue(lTotalLength, 2);
        //                                }

        //                                if (lDataTable.Rows[lHeaderRows].ItemArray[j].ToString() == "A") orderDetailsModels.A = getParaValue(lValue, 1);
        //                                if (lDataTable.Rows[lHeaderRows].ItemArray[j].ToString() == "B") orderDetailsModels.B = getParaValue(lValue, 1);
        //                                if (lDataTable.Rows[lHeaderRows].ItemArray[j].ToString() == "C") orderDetailsModels.C = getParaValue(lValue, 1);
        //                                if (lDataTable.Rows[lHeaderRows].ItemArray[j].ToString() == "D") orderDetailsModels.D = getParaValue(lValue, 1);
        //                                if (lDataTable.Rows[lHeaderRows].ItemArray[j].ToString() == "E") orderDetailsModels.E = getParaValue(lValue, 1);
        //                                if (lDataTable.Rows[lHeaderRows].ItemArray[j].ToString() == "F") orderDetailsModels.F = getParaValue(lValue, 1);
        //                                if (lDataTable.Rows[lHeaderRows].ItemArray[j].ToString() == "G") orderDetailsModels.G = getParaValue(lValue, 1);
        //                                if (lDataTable.Rows[lHeaderRows].ItemArray[j].ToString() == "H") orderDetailsModels.H = getParaValue(lValue, 1);
        //                                if (lDataTable.Rows[lHeaderRows].ItemArray[j].ToString() == "I") orderDetailsModels.I = getParaValue(lValue, 1);
        //                                if (lDataTable.Rows[lHeaderRows].ItemArray[j].ToString() == "J") orderDetailsModels.J = getParaValue(lValue, 1);
        //                                if (lDataTable.Rows[lHeaderRows].ItemArray[j].ToString() == "K") orderDetailsModels.K = getParaValue(lValue, 1);
        //                                if (lDataTable.Rows[lHeaderRows].ItemArray[j].ToString() == "L") orderDetailsModels.L = getParaValue(lValue, 1);
        //                                if (lDataTable.Rows[lHeaderRows].ItemArray[j].ToString() == "M") orderDetailsModels.M = getParaValue(lValue, 1);
        //                                if (lDataTable.Rows[lHeaderRows].ItemArray[j].ToString() == "N") orderDetailsModels.N = getParaValue(lValue, 1);
        //                                if (lDataTable.Rows[lHeaderRows].ItemArray[j].ToString() == "O") orderDetailsModels.O = getParaValue(lValue, 1);
        //                                if (lDataTable.Rows[lHeaderRows].ItemArray[j].ToString() == "P") orderDetailsModels.P = getParaValue(lValue, 1);
        //                                if (lDataTable.Rows[lHeaderRows].ItemArray[j].ToString() == "Q") orderDetailsModels.Q = getParaValue(lValue, 1);
        //                                if (lDataTable.Rows[lHeaderRows].ItemArray[j].ToString() == "R") orderDetailsModels.R = getParaValue(lValue, 1);
        //                                if (lDataTable.Rows[lHeaderRows].ItemArray[j].ToString() == "S") orderDetailsModels.S = getParaValue(lValue, 1);
        //                                if (lDataTable.Rows[lHeaderRows].ItemArray[j].ToString() == "T") orderDetailsModels.T = getParaValue(lValue, 1);
        //                                if (lDataTable.Rows[lHeaderRows].ItemArray[j].ToString() == "U") orderDetailsModels.U = getParaValue(lValue, 1);
        //                                if (lDataTable.Rows[lHeaderRows].ItemArray[j].ToString() == "V") orderDetailsModels.V = getParaValue(lValue, 1);
        //                                if (lDataTable.Rows[lHeaderRows].ItemArray[j].ToString() == "W") orderDetailsModels.W = getParaValue(lValue, 1);
        //                                if (lDataTable.Rows[lHeaderRows].ItemArray[j].ToString() == "X") orderDetailsModels.X = getParaValue(lValue, 1);
        //                                if (lDataTable.Rows[lHeaderRows].ItemArray[j].ToString() == "Y") orderDetailsModels.Y = getParaValue(lValue, 1);
        //                                if (lDataTable.Rows[lHeaderRows].ItemArray[j].ToString() == "Z") orderDetailsModels.Z = getParaValue(lValue, 1);

        //                                if (lDataTable.Rows[lHeaderRows].ItemArray[j].ToString() == "A") orderDetailsModels.A2 = getParaValue(lValue, 2);
        //                                if (lDataTable.Rows[lHeaderRows].ItemArray[j].ToString() == "B") orderDetailsModels.B2 = getParaValue(lValue, 2);
        //                                if (lDataTable.Rows[lHeaderRows].ItemArray[j].ToString() == "C") orderDetailsModels.C2 = getParaValue(lValue, 2);
        //                                if (lDataTable.Rows[lHeaderRows].ItemArray[j].ToString() == "D") orderDetailsModels.D2 = getParaValue(lValue, 2);
        //                                if (lDataTable.Rows[lHeaderRows].ItemArray[j].ToString() == "E") orderDetailsModels.E2 = getParaValue(lValue, 2);
        //                                if (lDataTable.Rows[lHeaderRows].ItemArray[j].ToString() == "F") orderDetailsModels.F2 = getParaValue(lValue, 2);
        //                                if (lDataTable.Rows[lHeaderRows].ItemArray[j].ToString() == "G") orderDetailsModels.G2 = getParaValue(lValue, 2);
        //                                if (lDataTable.Rows[lHeaderRows].ItemArray[j].ToString() == "H") orderDetailsModels.H2 = getParaValue(lValue, 2);
        //                                if (lDataTable.Rows[lHeaderRows].ItemArray[j].ToString() == "I") orderDetailsModels.I2 = getParaValue(lValue, 2);
        //                                if (lDataTable.Rows[lHeaderRows].ItemArray[j].ToString() == "J") orderDetailsModels.J2 = getParaValue(lValue, 2);
        //                                if (lDataTable.Rows[lHeaderRows].ItemArray[j].ToString() == "K") orderDetailsModels.K2 = getParaValue(lValue, 2);
        //                                if (lDataTable.Rows[lHeaderRows].ItemArray[j].ToString() == "L") orderDetailsModels.L2 = getParaValue(lValue, 2);
        //                                if (lDataTable.Rows[lHeaderRows].ItemArray[j].ToString() == "M") orderDetailsModels.M2 = getParaValue(lValue, 2);
        //                                if (lDataTable.Rows[lHeaderRows].ItemArray[j].ToString() == "N") orderDetailsModels.N2 = getParaValue(lValue, 2);
        //                                if (lDataTable.Rows[lHeaderRows].ItemArray[j].ToString() == "O") orderDetailsModels.O2 = getParaValue(lValue, 2);
        //                                if (lDataTable.Rows[lHeaderRows].ItemArray[j].ToString() == "P") orderDetailsModels.P2 = getParaValue(lValue, 2);
        //                                if (lDataTable.Rows[lHeaderRows].ItemArray[j].ToString() == "Q") orderDetailsModels.Q2 = getParaValue(lValue, 2);
        //                                if (lDataTable.Rows[lHeaderRows].ItemArray[j].ToString() == "R") orderDetailsModels.R2 = getParaValue(lValue, 2);
        //                                if (lDataTable.Rows[lHeaderRows].ItemArray[j].ToString() == "S") orderDetailsModels.S2 = getParaValue(lValue, 2);
        //                                if (lDataTable.Rows[lHeaderRows].ItemArray[j].ToString() == "T") orderDetailsModels.T2 = getParaValue(lValue, 2);
        //                                if (lDataTable.Rows[lHeaderRows].ItemArray[j].ToString() == "U") orderDetailsModels.U2 = getParaValue(lValue, 2);
        //                                if (lDataTable.Rows[lHeaderRows].ItemArray[j].ToString() == "V") orderDetailsModels.V2 = getParaValue(lValue, 2);
        //                                if (lDataTable.Rows[lHeaderRows].ItemArray[j].ToString() == "W") orderDetailsModels.W2 = getParaValue(lValue, 2);
        //                                if (lDataTable.Rows[lHeaderRows].ItemArray[j].ToString() == "X") orderDetailsModels.X2 = getParaValue(lValue, 2);
        //                                if (lDataTable.Rows[lHeaderRows].ItemArray[j].ToString() == "Y") orderDetailsModels.Y2 = getParaValue(lValue, 2);
        //                                if (lDataTable.Rows[lHeaderRows].ItemArray[j].ToString() == "Z") orderDetailsModels.Z2 = getParaValue(lValue, 2);
        //                            }
        //                        }
        //                        else
        //                        {
        //                            //WITHOUT HEADER ROW PARAMETER a STARTFROM 8
        //                            for (int j = 8; j < lDataTable.Columns.Count; j++)
        //                            {
        //                                var lValue = lDataTable.Rows[i].ItemArray[j].ToString();

        //                                if (isInvalidParameterCell(lShapeCode,
        //                                    ((char)(Encoding.ASCII.GetBytes("A")[0] + j - 8)).ToString(),
        //                                    lshapeParameters) == true)
        //                                {
        //                                    lValue = null;
        //                                }

        //                                var lErrorMsg = "";
        //                                //if (isValidValue(
        //                                //    ((char)(Encoding.ASCII.GetBytes("A")[0] + j - 8)).ToString(),
        //                                //    lshapeParameters,
        //                                //    lshapeParaValidator,
        //                                //    lBarDiaInt,
        //                                //    lValue,
        //                                //    orderDetailsModels,
        //                                //    out lErrorMsg) == false)
        //                                //{
        //                                //    lValue = null;
        //                                //}

        //                                var lTotalLength = calTotalLen(
        //                                    orderDetailsModels,
        //                                    lshapeLengthFormula,
        //                                    lValue);
        //                                // validate the Total Length
        //                                var lMaxLength = getVarMaxValue(lTotalLength);
        //                                if (lMaxLength > 12000)
        //                                {
        //                                    lValue = null;
        //                                }
        //                                else
        //                                {
        //                                    orderDetailsModels.BarLength = getParaValue(lTotalLength, 1);
        //                                    orderDetailsModels.BarLength2 = getParaValue(lTotalLength, 2);
        //                                }

        //                                if (j == 8) orderDetailsModels.A = getParaValue(lDataTable.Rows[i].ItemArray[j].ToString(), 1);
        //                                if (j == 9) orderDetailsModels.B = getParaValue(lDataTable.Rows[i].ItemArray[j].ToString(), 1);
        //                                if (j == 10) orderDetailsModels.C = getParaValue(lDataTable.Rows[i].ItemArray[j].ToString(), 1);
        //                                if (j == 11) orderDetailsModels.D = getParaValue(lDataTable.Rows[i].ItemArray[j].ToString(), 1);
        //                                if (j == 12) orderDetailsModels.E = getParaValue(lDataTable.Rows[i].ItemArray[j].ToString(), 1);
        //                                if (j == 13) orderDetailsModels.F = getParaValue(lDataTable.Rows[i].ItemArray[j].ToString(), 1);
        //                                if (j == 14) orderDetailsModels.G = getParaValue(lDataTable.Rows[i].ItemArray[j].ToString(), 1);
        //                                if (j == 15) orderDetailsModels.H = getParaValue(lDataTable.Rows[i].ItemArray[j].ToString(), 1);
        //                                if (j == 16) orderDetailsModels.I = getParaValue(lDataTable.Rows[i].ItemArray[j].ToString(), 1);
        //                                if (j == 17) orderDetailsModels.J = getParaValue(lDataTable.Rows[i].ItemArray[j].ToString(), 1);
        //                                if (j == 18) orderDetailsModels.K = getParaValue(lDataTable.Rows[i].ItemArray[j].ToString(), 1);
        //                                if (j == 19) orderDetailsModels.L = getParaValue(lDataTable.Rows[i].ItemArray[j].ToString(), 1);
        //                                if (j == 20) orderDetailsModels.M = getParaValue(lDataTable.Rows[i].ItemArray[j].ToString(), 1);
        //                                if (j == 21) orderDetailsModels.N = getParaValue(lDataTable.Rows[i].ItemArray[j].ToString(), 1);
        //                                if (j == 22) orderDetailsModels.O = getParaValue(lDataTable.Rows[i].ItemArray[j].ToString(), 1);
        //                                if (j == 23) orderDetailsModels.P = getParaValue(lDataTable.Rows[i].ItemArray[j].ToString(), 1);
        //                                if (j == 24) orderDetailsModels.Q = getParaValue(lDataTable.Rows[i].ItemArray[j].ToString(), 1);
        //                                if (j == 25) orderDetailsModels.R = getParaValue(lDataTable.Rows[i].ItemArray[j].ToString(), 1);
        //                                if (j == 26) orderDetailsModels.S = getParaValue(lDataTable.Rows[i].ItemArray[j].ToString(), 1);
        //                                if (j == 27) orderDetailsModels.T = getParaValue(lDataTable.Rows[i].ItemArray[j].ToString(), 1);
        //                                if (j == 28) orderDetailsModels.U = getParaValue(lDataTable.Rows[i].ItemArray[j].ToString(), 1);
        //                                if (j == 29) orderDetailsModels.V = getParaValue(lDataTable.Rows[i].ItemArray[j].ToString(), 1);
        //                                if (j == 30) orderDetailsModels.W = getParaValue(lDataTable.Rows[i].ItemArray[j].ToString(), 1);
        //                                if (j == 31) orderDetailsModels.X = getParaValue(lDataTable.Rows[i].ItemArray[j].ToString(), 1);
        //                                if (j == 32) orderDetailsModels.Y = getParaValue(lDataTable.Rows[i].ItemArray[j].ToString(), 1);
        //                                if (j == 33) orderDetailsModels.Z = getParaValue(lDataTable.Rows[i].ItemArray[j].ToString(), 1);

        //                                if (j == 8) orderDetailsModels.A2 = getParaValue(lDataTable.Rows[i].ItemArray[j].ToString(), 2);
        //                                if (j == 9) orderDetailsModels.B2 = getParaValue(lDataTable.Rows[i].ItemArray[j].ToString(), 2);
        //                                if (j == 10) orderDetailsModels.C2 = getParaValue(lDataTable.Rows[i].ItemArray[j].ToString(), 2);
        //                                if (j == 11) orderDetailsModels.D2 = getParaValue(lDataTable.Rows[i].ItemArray[j].ToString(), 2);
        //                                if (j == 12) orderDetailsModels.E2 = getParaValue(lDataTable.Rows[i].ItemArray[j].ToString(), 2);
        //                                if (j == 13) orderDetailsModels.F2 = getParaValue(lDataTable.Rows[i].ItemArray[j].ToString(), 2);
        //                                if (j == 14) orderDetailsModels.G2 = getParaValue(lDataTable.Rows[i].ItemArray[j].ToString(), 2);
        //                                if (j == 15) orderDetailsModels.H2 = getParaValue(lDataTable.Rows[i].ItemArray[j].ToString(), 2);
        //                                if (j == 16) orderDetailsModels.I2 = getParaValue(lDataTable.Rows[i].ItemArray[j].ToString(), 2);
        //                                if (j == 17) orderDetailsModels.J2 = getParaValue(lDataTable.Rows[i].ItemArray[j].ToString(), 2);
        //                                if (j == 18) orderDetailsModels.K2 = getParaValue(lDataTable.Rows[i].ItemArray[j].ToString(), 2);
        //                                if (j == 19) orderDetailsModels.L2 = getParaValue(lDataTable.Rows[i].ItemArray[j].ToString(), 2);
        //                                if (j == 20) orderDetailsModels.M2 = getParaValue(lDataTable.Rows[i].ItemArray[j].ToString(), 2);
        //                                if (j == 21) orderDetailsModels.N2 = getParaValue(lDataTable.Rows[i].ItemArray[j].ToString(), 2);
        //                                if (j == 22) orderDetailsModels.O2 = getParaValue(lDataTable.Rows[i].ItemArray[j].ToString(), 2);
        //                                if (j == 23) orderDetailsModels.P2 = getParaValue(lDataTable.Rows[i].ItemArray[j].ToString(), 2);
        //                                if (j == 24) orderDetailsModels.Q2 = getParaValue(lDataTable.Rows[i].ItemArray[j].ToString(), 2);
        //                                if (j == 25) orderDetailsModels.R2 = getParaValue(lDataTable.Rows[i].ItemArray[j].ToString(), 2);
        //                                if (j == 26) orderDetailsModels.S2 = getParaValue(lDataTable.Rows[i].ItemArray[j].ToString(), 2);
        //                                if (j == 27) orderDetailsModels.T2 = getParaValue(lDataTable.Rows[i].ItemArray[j].ToString(), 2);
        //                                if (j == 28) orderDetailsModels.U2 = getParaValue(lDataTable.Rows[i].ItemArray[j].ToString(), 2);
        //                                if (j == 29) orderDetailsModels.V2 = getParaValue(lDataTable.Rows[i].ItemArray[j].ToString(), 2);
        //                                if (j == 30) orderDetailsModels.W2 = getParaValue(lDataTable.Rows[i].ItemArray[j].ToString(), 2);
        //                                if (j == 31) orderDetailsModels.X2 = getParaValue(lDataTable.Rows[i].ItemArray[j].ToString(), 2);
        //                                if (j == 32) orderDetailsModels.Y2 = getParaValue(lDataTable.Rows[i].ItemArray[j].ToString(), 2);
        //                                if (j == 33) orderDetailsModels.Z2 = getParaValue(lDataTable.Rows[i].ItemArray[j].ToString(), 2);
        //                            }
        //                        }
        //                        double lWT = calWeight(orderDetailsModels);
        //                        if (lWT > 0)
        //                        {
        //                            orderDetailsModels.BarWeight = (decimal?)lWT;
        //                        }

        //                        //orderDetailsModels.shapeTransport = (byte)ValidTransport(orderDetailsModels, lshapeTransportValidator);

        //                    }
        //                    db.OrderDetails.Add(orderDetailsModels);

        //                }
        //                db.SaveChanges();

        //            }
        //        }
        //        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError(string.Empty, "Server Error: " + ex.Message);
        //        return Json(new { success = false, error = true, message = "Import Error: " + ex.Message }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        [HttpPost]
        [Route("/getWBS1_beam/{ProjectCode}")]
        //[ValidateAntiForgeryHeader]
        public ActionResult getWBS1(string ProjectCode)
        {
            var content = (from p in db.WBSList
                           where p.ProjectCode == ProjectCode && p.ProductType == "MSH"
                           select p.WBS1)
                           .Distinct()
                           .OrderBy(x => x)
                           .ToList();
            //return Json(content, JsonRequestBehavior.AllowGet);
            return Ok(content);
        }

        [HttpPost]

        [Route("/getWBS2_beam/{ProjectCode}/{WBS1}")]
        //  [ValidateAntiForgeryHeader]
        public ActionResult getWBS2(string ProjectCode, string WBS1)
        {
            var lReturn = new List<string>();
            var lCmd = new SqlCommand();
            var lNDSCon = new SqlConnection();
            SqlDataReader lRst;

            try
            {
                lCmd.CommandText =
                "SELECT WBS2 " +
                "FROM dbo.OESWBSList " +
                "WHERE ProjectCode = '" + ProjectCode + "' " +
                "AND WBS1 = '" + WBS1 + "' " +
                "AND ProductType = 'MSH' " +
                "GROUP BY WBS2 " +
                "Order By " +
                "(case when PATINDEX('B[0-9]%', WBS2) > 0 then cast(left(substring(WBS2, (PATINDEX('%[0-9]%',WBS2)),len(WBS2)), " +
                "PATINDEX('%[^0-9]%', substring(WBS2 + 'z', (PATINDEX('%[0-9]%', WBS2)), " +
                "len(WBS2)) + 'z') ) as int)    " +
                "else (case when PATINDEX('FDN[0-9]%', WBS2) > 0 then cast(left(substring(WBS2, (PATINDEX('%[0-9]%',WBS2)),len(WBS2)), " +
                "PATINDEX('%[^0-9]%', substring(WBS2, (PATINDEX('%[0-9]%', WBS2)), " +
                "len(WBS2)) + 'z') ) as int) else  " +
                "case when(PATINDEX('B%', WBS2) > 0 OR PATINDEX('FDN%', WBS2) > 0) then 98  else 99 end " +
                "end) end), " +
                " " +
                "(case when PATINDEX('[^0-9]%',WBS2) > 0 then WBS2 " +
                "else '' end), " +
                " " +
                "(CASE WHEN PATINDEX('%[0-9]%',WBS2) > 0 THEN cast(left(substring(WBS2, (PATINDEX('%[0-9]%',WBS2)),len(WBS2)), " +
                "PATINDEX('%[^0-9]%', substring(WBS2, (PATINDEX('%[0-9]%', WBS2) + 1), " +
                "len(WBS2)) + 'z') ) as int) " +
                "ELSE " +
                "cast(left(substring(WBS2, (PATINDEX('%[0-9]%', WBS2)), len(WBS2)), " +
                "PATINDEX('%[^0-9]%', substring(WBS2, (PATINDEX('%[0-9]%', WBS2)), " +
                "len(WBS2)) + 'z') - 1) as int) " +
                "END),  " +
                "WBS2 ";

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
                            var lWBS2 = lRst.GetValue(0) == DBNull.Value ? "" : lRst.GetString(0).Trim();
                            if (lWBS2.Length > 0)
                            {
                                lReturn.Add(lWBS2);
                            }
                        }
                    }
                    lRst.Close();

                    lProcessObj.CloseNDSConnection(ref lNDSCon);
                }

                lProcessObj = null;

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Server Error: " + ex.Message);
            }

            lCmd = null;
            lNDSCon = null;
            lRst = null;

            // return Json(lReturn, JsonRequestBehavior.AllowGet);
            return Ok(lReturn);
        }

        [HttpPost]
        [Route("/getWBS3_beam/{ProjectCode}/{WBS1}/{WBS2}")]
       // [ValidateAntiForgeryHeader]
        public ActionResult getWBS3(string ProjectCode, string WBS1, string WBS2)
        {
            var content = (from p in db.WBSList
                           where p.ProjectCode == ProjectCode
                           && p.WBS1 == WBS1
                           && p.WBS2 == WBS2
                           && p.ProductType == "MSH"
                           select p.WBS3)
                           .Distinct()
                           .OrderBy(x => x)
                           .ToList();

            // return Json(content, JsonRequestBehavior.AllowGet);
            return Ok(content);
        }

        [HttpPost]
        [Route("/getStrucElem_beam/{ProjectCode}/{pWBS1}/{pWBS2}/{pWBS3}")]
        //[ValidateAntiForgeryHeader]
        public ActionResult getStrucElem(string ProjectCode, string pWBS1, string pWBS2, string pWBS3)
        {
            var content1 = new List<string>();
            //if (pWBS2.Trim().Length > 0 && pWBS3.Trim().Length > 0)
            //{
            var content = (from p in db.WBSList
                           where p.ProjectCode == ProjectCode
                           && p.WBS1 == pWBS1
                           && p.WBS2 == pWBS2
                           && p.WBS3 == pWBS3
                           && p.ProductType == "MSH"
                           orderby p.StructureElem
                           select p.StructureElem).Distinct().ToList();
            return Ok(content);
           // return Json(content, JsonRequestBehavior.AllowGet);
            //}
            //else if (pWBS2.Trim().Length > 0 && pWBS3.Trim().Length == 0)
            //{
            //    var content = (from p in db.WBSList
            //                   where p.ProjectCode == ProjectCode
            //                   && p.WBS1 == pWBS1
            //                   && p.WBS2 == pWBS2
            //                   && p.ProductType == "MSH"
            //                   orderby p.StructureElem
            //                   select p.StructureElem).Distinct().ToList();
            //    return Json(content, JsonRequestBehavior.AllowGet);
            //}
            //else if (pWBS2.Trim().Length == 0 && pWBS3.Trim().Length == 0)
            //{
            //    var content = (from p in db.WBSList
            //                   where p.ProjectCode == ProjectCode
            //                   && p.WBS1 == pWBS1
            //                   && p.ProductType == "MSH"
            //                   orderby p.StructureElem
            //                   select p.StructureElem).Distinct().ToList();
            //    return Json(content, JsonRequestBehavior.AllowGet);
            //}

            //content1.Add(" ");
            //return Json(content1, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [Route("/sendOrderSubmittedEmail_beam/{CustomerCode}/{ProjectCode}/{JobID}")]
        //[ValidateAntiForgeryHeader]
        public ActionResult sendOrderSubmittedEmail(string CustomerCode, string ProjectCode, int JobID)
        {
            var lEmailContent = "";
            var lEmailFrom = "";
            var lEmailTo = "";
            var lEmailCc = "";
            var lEmailSubject = "";
            string lVar1 = "";

            var JobContent = db.CTSMESHJobAdvice.Find(CustomerCode, ProjectCode, JobID);
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

                if (JobContent.Remarks == null) JobContent.Remarks = "";
                else JobContent.Remarks = JobContent.Remarks.Trim();

                if (JobContent.DeliveryAddress == null) JobContent.DeliveryAddress = "";
                else JobContent.DeliveryAddress = JobContent.DeliveryAddress.Trim();

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

                if (JobContent.WBS1 == null) JobContent.WBS1 = "";
                else JobContent.WBS1 = JobContent.WBS1.Trim();

                if (JobContent.WBS2 == null) JobContent.WBS2 = "";
                else JobContent.WBS2 = JobContent.WBS2.Trim();

                if (JobContent.WBS3 == null) JobContent.WBS3 = "";
                else JobContent.WBS3 = JobContent.WBS3.Trim();
            }

            lEmailContent = "<p align='center'>JOB ADVICE - MESH (工作通知 - 网片料表)</p>";

            lEmailContent = lEmailContent + "<p align='left' style='font-size:15px'><a target='_top' href='https://oes.natsteel.com.sg/process?ccode=" + JobContent.CustomerCode
                + "&jcode=" + JobContent.ProjectCode + "&prodtype=Rebar" + "&jobid=" + JobContent.JobID.ToString()
                + "'>Click here to redirect to OES system to process it</a></p>";

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

            lEmailContent = lEmailContent + "<td width=20%>" + "PO No.\n(订单号码)" + "</td>";
            lEmailContent = lEmailContent + "<td width=27%>" + JobContent.PONumber.Trim() + "</td>";
            lEmailContent = lEmailContent + "<td width=26%>" + "Order Date\n(订单日期)" + "</td>";
            lEmailContent = lEmailContent + "<td>" + String.Format("{0:yyyy-MM-dd}", JobContent.PODate) + "</td></tr>";

            lEmailContent = lEmailContent + "<tr><td width=20%>" + "Required Date\n(交货日期)" + "</td>";
            lEmailContent = lEmailContent + "<td width=27%>" + String.Format("{0:yyyy-MM-dd}", JobContent.RequiredDate) + "</td>";
            lEmailContent = lEmailContent + "<td width=26%>" + "Order Weight\n(订单重量)" + "</td>";
            if (JobContent.TotalWeight > 0)
            {
                lEmailContent = lEmailContent + "<td>" + ((decimal)JobContent.TotalWeight).ToString("F3") + " KG" + "</td></tr>";
            }
            else
            {
                lEmailContent = lEmailContent + "<td>" + " " + "</td></tr>";
            }

            lEmailContent = lEmailContent + "</tr><td width=20%>" + "Order Pieces\n(订单总数)" + "</td>";
            if (JobContent.TotalPcs > 0)
            {
                lEmailContent = lEmailContent + "<td width=27%>" + JobContent.TotalPcs.ToString() + "</td>";
            }
            else
            {
                lEmailContent = lEmailContent + "<td width=27%> </td>";
            }
            lEmailContent = lEmailContent + "<td width=26%>" + "Transportation\n(运输工具)" + "</td>";
            lEmailContent = lEmailContent + "<td>" + JobContent.Transport + "</td></tr></table>";

            lEmailContent = lEmailContent + "<table border='1' width=100%><tr>";
            lEmailContent = lEmailContent + "<td width='20%'>" + "Block (WBS1)\n(座号/大牌)" + "</td>";
            lEmailContent = lEmailContent + "<td width=15%>" + JobContent.WBS1.Trim() + "</td>";
            lEmailContent = lEmailContent + "<td width='17%'>" + "Storey (WBS2)\n(楼层)" + "</td>";
            lEmailContent = lEmailContent + "<td width=14%>" + JobContent.WBS2.Trim() + "</td>";
            lEmailContent = lEmailContent + "<td width=16%>" + "Part (WBS3)\n(分部)" + "</td>";
            lEmailContent = lEmailContent + "<td>" + JobContent.WBS3.Trim() + "</td></tr></table>";

            var lBBSContentNSH = (from p in db.CTSMESHBBSNSH
                                  where p.CustomerCode == CustomerCode &&
                                  p.ProjectCode == ProjectCode &&
                                  p.JobID == JobID &&
                                  p.BBSOrder == true
                                  orderby p.BBSID
                                  select p).ToList();

            if (lBBSContentNSH.Count > 0)
            {
                lEmailContent = lEmailContent + "<table border='1' width=100%>";
                lEmailContent = lEmailContent + "<tr><td colspan='6'> <font color='blue'>MESH Scheduled by NatSteel\n(大众钢铁安排的铁网)</font></td><tr>";
                lEmailContent = lEmailContent + "<td width='20%'>" + "Product Category\n(产品类型)" + "</td>";
                lEmailContent = lEmailContent + "<td width='17%'>" + "Structure Element\n(构件)" + "</td>";
                lEmailContent = lEmailContent + "<td width='15%'>" + "Description\n(说明)" + "</td>";
                lEmailContent = lEmailContent + "<td width='14%'>" + "Total Pieces\n总件数" + "</td>";
                lEmailContent = lEmailContent + "<td width='16%'>" + "Total Weight\n总重量" + "</td>";
                lEmailContent = lEmailContent + "<td>" + "Posted Date\n(上载日期)" + "</td></tr>";

                for (int i = 0; i < lBBSContentNSH.Count; i++)
                {
                    lEmailContent = lEmailContent + "<tr><td> <font color='blue'>" + lBBSContentNSH[i].BBSProdCategory + "</font></td>";
                    lEmailContent = lEmailContent + "<td>" + lBBSContentNSH[i].BBSStrucElem + "</td>";
                    lEmailContent = lEmailContent + "<td>" + lBBSContentNSH[i].BBSDesc + "</td>";
                    if (lBBSContentNSH[i].BBSTotalPcs == 0) lVar = ""; else lVar = lBBSContentNSH[i].BBSTotalPcs.ToString("F0");
                    lEmailContent = lEmailContent + "<td align='left'>" + lVar + "</td>";
                    if (lBBSContentNSH[i].BBSTotalWT == 0) lVar = ""; else lVar = lBBSContentNSH[i].BBSTotalWT.ToString("F3");
                    lEmailContent = lEmailContent + "<td align='left'>" + lVar + "</td>";
                    lEmailContent = lEmailContent + "<td align='left'>" + lBBSContentNSH[i].BBSNDSPostDate.ToString("yyyy-MM-dd") + "</td></tr>";
                }
                lEmailContent = lEmailContent + "</table>";
            }

            var lBBSContent = (from p in db.CTSMESHBBS
                               where p.CustomerCode == CustomerCode &&
                               p.ProjectCode == ProjectCode &&
                               p.JobID == JobID &&
                               p.BBSOrder == true
                               orderby p.BBSID
                               select p).ToList();

            if (lBBSContent.Count > 0)
            {
                lEmailContent = lEmailContent + "<table border='1' width=100%>";
                lEmailContent = lEmailContent + "<tr><td colspan='6'> <font color='blue'>MESH Scheduled by Customer Self\n(客户自己安排的铁网)</font></td><tr>";
                lEmailContent = lEmailContent + "<td width='20%'>" + "Product Category\n(产品类型)" + "</td>";
                lEmailContent = lEmailContent + "<td width='17%'>" + "Structure Element\n(构件)" + "</td>";
                lEmailContent = lEmailContent + "<td width='15%'>" + "Description\n(说明)" + "</td>";
                lEmailContent = lEmailContent + "<td width='14%'>" + "Total Pieces\n总件数" + "</td>";
                lEmailContent = lEmailContent + "<td width='16%'>" + "Total Weight\n总重量" + "</td>";
                lEmailContent = lEmailContent + "<td>" + "Updated Date\n更新日期" + "</td></tr>";

                for (int i = 0; i < lBBSContent.Count; i++)
                {
                    lEmailContent = lEmailContent + "<tr><td> <font color='blue'>" + lBBSContent[i].BBSProdCategory + "</font></td>";
                    lEmailContent = lEmailContent + "<td>" + lBBSContent[i].BBSStrucElem + "</td>";
                    lEmailContent = lEmailContent + "<td>" + lBBSContent[i].BBSDesc + "</td>";
                    if (lBBSContent[i].BBSTotalPcs == 0) lVar = ""; else lVar = lBBSContent[i].BBSTotalPcs.ToString("F0");
                    lEmailContent = lEmailContent + "<td align='left'>" + lVar + "</td>";
                    if (lBBSContent[i].BBSTotalWT == 0) lVar = ""; else lVar = lBBSContent[i].BBSTotalWT.ToString("F3");
                    lEmailContent = lEmailContent + "<td align='left'>" + lVar + "</td>";
                    lEmailContent = lEmailContent + "<td align='left'>" + ((DateTime)lBBSContent[i].UpdateDate).ToString("yyyy-MM-dd") + "</td></tr>";
                }
                lEmailContent = lEmailContent + "</table>";
            }

            if (lBBSContentNSH.Count == 0 && lBBSContent.Count == 0)
            {
                lEmailContent = lEmailContent + "<table border='1' width=100%><tr>";
                lEmailContent = lEmailContent + "<tr><td> <font color='blue'>Order without details. </font></td></tr>";
                lEmailContent = lEmailContent + "</table>";
            }

            lEmailContent = lEmailContent + "<table border='1' width=100%>";
            lEmailContent = lEmailContent + "<tr><td width='20%'>" + "Delivery Address\n(送货地址)" + "</td>";
            lEmailContent = lEmailContent + "<td>" + JobContent.DeliveryAddress + "</td></tr>";
            lEmailContent = lEmailContent + "</table>";

            lEmailContent = lEmailContent + "<table border='1' width=100%>";
            lEmailContent = lEmailContent + "<tr><td width='20%'>" + "Special Remarks\n(特别备注)" + "</td>";
            lEmailContent = lEmailContent + "<td>" + JobContent.Remarks + "</td></tr>";
            lEmailContent = lEmailContent + "</table>";

            lEmailContent = lEmailContent + "<table border='1' width=100%>";
            lEmailContent = lEmailContent + "<tr><td width='20%'>" + "Site Contact\n(联系人)" + "</td>";
            lEmailContent = lEmailContent + "<td width='15%'>" + JobContent.Scheduler_Name.Trim() + "</td>";
            lEmailContent = lEmailContent + "<td width='13%'>" + "H / P\n(手机号码)" + " </td>";
            lEmailContent = lEmailContent + "<td width='16%'>" + JobContent.Scheduler_HP.Trim() + "</td>";
            lEmailContent = lEmailContent + "<td width='13%'>" + "Email\n(电邮地址)" + "</td>";
            lEmailContent = lEmailContent + "<td>" + JobContent.Scheduler_Tel.Trim() + "</td></tr>";

            lEmailContent = lEmailContent + "<tr><td width='20%'>" + "Goods Receiver\n(收货人)" + "</td>";
            lEmailContent = lEmailContent + "<td width='15%'>" + JobContent.SiteEngr_Name.Trim() + "</td>";
            lEmailContent = lEmailContent + "<td width='13%'>" + "H / P\n(手机号码)" + " </td>";
            lEmailContent = lEmailContent + "<td width='16%'>" + JobContent.SiteEngr_HP.Trim() + "</td>";
            lEmailContent = lEmailContent + "<td width='13%'>" + "Email\n(电邮地址)" + "</td>";
            lEmailContent = lEmailContent + "<td>" + JobContent.SiteEngr_Tel.Trim() + "</td></tr>";
            lEmailContent = lEmailContent + "</table>";

            lEmailContent = lEmailContent + "<table border='1' width=100%><tr>";

            lEmailContent = lEmailContent + "<td colspan='3'>" + "NatSteel Contacts (大众钢铁联系人) (Fax:62619133)" + "</td></tr>";

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
                    if (lEmailTo == "")
                    {
                        lEmailTo = lVar1;
                    }
                    else
                    {
                        lEmailTo = lEmailTo + ";" + lVar1;
                    }
                }

                lVar1 = JobContent.SiteEngr_Tel.Trim();
                if (lVar1.Length > 0 && lEmailCc.IndexOf(lVar1) < 0)
                {
                    if (lEmailTo == "")
                    {
                        lEmailTo = lVar1;
                    }
                    else
                    {
                        lEmailTo = lEmailTo + ";" + lVar1;
                    }
                }
            }

            lVar = "";
            if (lCustomer != null) lVar = lCustomer.CustomerName.Trim();

            lEmailSubject = JobContent.PONumber.Trim() + " - " + lVar + " - MESH No. " + JobID.ToString();

            var lOESEmail = new SendGridEmail();

            string lEmailFromAddress = "eprompt@natsteel.com.sg";
            string lEmailFromName = "OES Email Service";

            //dynamic response = lOESEmail.Execute(lEmailFromAddress, lEmailFromName, lEmailTo, lEmailCc, lEmailSubject, "text/html", lEmailContent);
            lOESEmail.Execute(lEmailFromAddress, lEmailFromName, lEmailTo, lEmailCc, lEmailSubject, "text/html", lEmailContent).Wait();
            lOESEmail = null;

            return Json(true);
        }

        //[HttpPost]
        //[Route("/getProject_beam/{CustomerCode}")]
        ////[ValidateAntiForgeryHeader]
        //public ActionResult getProject(string CustomerCode)
        //{
        //    string lUserType = "";
        //    string lGroupName = "";

        //    OracleDataReader lRst;
        //    var lCmd = new OracleCommand();
        //    var lcisCon = new OracleConnection();

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

        //    if (lUserType == "AD" || lUserType == "PM" || lUserType == "PA" || lUserType == "P1" || lUserType == "P2" || lUserType == "PU" || lUserType == "TE")
        //    {
        //        #region Get from SAP
        //        var lDa = new OracleDataAdapter();
        //        var lOCmd = new OracleCommand();
        //        var lDs = new DataSet();
        //        var lDStatus = new DataSet();
        //        var lOcisCon = new OracleConnection();
        //        var lProcess = new ProcessController();

        //        lOCmd.CommandText = "SELECT (NAME1 || NAME2) AS SHIP_TO_NAME,KUNNR AS SHIP_TO_PARTY FROM SAPSR3.KNA1 " +
        //        "WHERE KTOKD = 'Y001' AND MANDT = '" + lProcess.strClient + "' " +
        //        "AND KUNNR IN (SELECT KUNNR FROM SAPSR3.VBPA WHERE MANDT='" + lProcess.strClient + "' " +
        //        "AND VBELN IN (SELECT VBELN FROM SAPSR3.VBAK WHERE MANDT ='" + lProcess.strClient + "' " +
        //        "AND (VBELN like '102%' OR VBELN like '_102%') " +
        //        "AND ytot_mesh > 0 " +
        //        "AND KUNNR = '" + CustomerCode + "' AND TRVOG ='4' " +
        //        "AND to_date(GUEEN, 'yyyymmdd') >=  (SYSDATE - 180) )) " +
        //        "ORDER BY 1 ";

        //        if (lProcess.OpenCISConnection(ref lOcisCon) == true)
        //        {
        //            lOCmd.Connection = lOcisCon;
        //            lDa.SelectCommand = lOCmd;
        //            lDs.Clear();
        //            lDa.Fill(lDs);
        //            if (lDs.Tables[0].Rows.Count > 0)
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
        //            lProcess.CloseCISConnection(ref lOcisCon);
        //        }
        //        lDa = null;
        //        lOCmd = null;
        //        lDs = null;
        //        lDStatus = null;
        //        lOcisCon = null;
        //        lProcess = null;
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
        //            };
        //    }

        //    //  return Json(content, JsonRequestBehavior.AllowGet);
        //    return Ok(content);
        //}

        //[HttpGet]
        //[Route("/RemoveNonMESHProject_Oracle_beam/{pInputProj}")]
        //public List<ProjectListModels> RemoveNonMESHProject_Oracle(List<ProjectListModels> pInputProj)
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
        //                "FROM SAPSR3.VBAK K, SAPSR3.VBPA P " +
        //                "WHERE K.MANDT = '" + lProcessObj.strClient + "' " +
        //                "AND K.VKORG = '" + lProcessObj.strSalesOrg + "' " +
        //                "AND K.KUNNR = '" + pInputProj[i].CustomerCode + "' " +
        //                "AND K.TRVOG = '4' " +
        //                "AND K.ytot_mesh > 0 " +
        //                "AND K.MANDT = P.MANDT " +
        //                "AND K.VBELN = P.VBELN " +
        //                "AND K.VBELN like '102%' " +
        //                "AND P.VBELN like '102%' " +
        //                "AND P.KUNNR = '" + pInputProj[i].ProjectCode + "'";

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

        [HttpGet]

        [Route("/RemoveNonMESHProject_beam/{pInputProj}")]
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

        [HttpPost]

        [Route("/getProjectStage_beam")]
        //[ValidateAntiForgeryHeader]
        public ActionResult getProjectStage()
        {
            var content = (from p in db.ProjectStage
                           orderby p.prj_level_desc
                           select new
                           {
                               prj_level_cat = p.prj_level_cat.Trim(),
                               prj_level_desc = p.prj_level_desc.Trim()
                           }).ToList();

            return Ok(content);
            //return Json(content, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Route("/checkOrderStatus_beam/{CustomerCode}/{ProjectCode}/{JobID}")]
        //  [ValidateAntiForgeryHeader]
        public ActionResult checkOrderStatus(string CustomerCode, string ProjectCode, int JobID)
        {
            var lCheckBeam = (from p in db.CTSMESHBeamDetails
                              where p.CustomerCode == CustomerCode &&
                              p.ProjectCode == ProjectCode &&
                              p.JobID == JobID &&
                              p.MeshShapeCode != null &&
                              p.MeshShapeCode != "" &&
                              p.UpdateBy != User.Identity.Name
                              select p).ToList();



            if (lCheckBeam.Count > 0)
            {
                string lUserNameCur = User.Identity.GetUserName();
                string lSumissionCur = "No";
                string lEditableCur = "No";
                string lUserTypeCur = "PL";

                string lUserNamePre = User.Identity.Name;
                string lSumissionPre = "No";
                string lEditablePre = "No";
                string lUserTypePre = "PL";

                var lAccess = db.UserAccess.Find(lUserNameCur, "0000000000", "0000000000");
                if (lAccess != null)
                {
                    lUserTypeCur = (lAccess.UserType == null ? "CU" : lAccess.UserType.Trim());
                }

                lAccess = db.UserAccess.Find(lUserNamePre, "0000000000", "0000000000");
                if (lAccess != null)
                {
                    lUserTypePre = (lAccess.UserType == null ? "CU" : lAccess.UserType.Trim());
                }

                lAccess = db.UserAccess.Find(lUserNameCur, CustomerCode, ProjectCode);
                if (lAccess != null)
                {
                    lSumissionCur = (lAccess.OrderSubmission == null ? "No" : lAccess.OrderSubmission.Trim());

                    lEditableCur = (lAccess.OrderCreation == null ? "No" : lAccess.OrderCreation.Trim());
                }
                lAccess = db.UserAccess.Find(lUserNamePre, CustomerCode, ProjectCode);
                if (lAccess != null)
                {
                    lSumissionPre = (lAccess.OrderSubmission == null ? "No" : lAccess.OrderSubmission.Trim());

                    lEditablePre = (lAccess.OrderCreation == null ? "No" : lAccess.OrderCreation.Trim());
                }

                if ((lUserTypeCur == "CU" || lUserTypeCur == "CM" || lUserTypeCur == "CA") &&
                ((lUserTypePre == "CU" || lUserTypePre == "CM" || lUserTypePre == "CA") &&
                (lSumissionPre != "Yes" && lSumissionCur == "Yes") ||
                lEditableCur != "Yes" || lEditablePre != "Yes"))
                {
                    // return Json(new { success = true, error = false, message = User.Identity.GetUserName() }, JsonRequestBehavior.AllowGet);
                    return Ok();
                }
                else
                {
                    //return Json(new
                    //{
                    //    success = false,
                    //    error = true,
                    //    message = "The order has been processed by another person. Please click <New Order> button to create a new order."
                    //}, JsonRequestBehavior.AllowGet);
                    return Ok();
                }

            }
            else
            {
                //return Json(new { success = true, error = false, message = User.Identity.GetUserName() }, JsonRequestBehavior.AllowGet);
                return Ok();
            }
        }

//        [HttpPost]
//        [Route("/getProjectDetails_beam/{CustomerCode}/{ProjectCode}/{UserName}")]
//        //[ValidateAntiForgeryHeader]
//        public ActionResult getProjectDetails(string CustomerCode, string ProjectCode,string UserName)
//        {
//            var lCmd = new SqlCommand();
//            SqlDataReader lRst;
//            var lNDSCon = new SqlConnection();

//            string lUserType = "";

//            if (gUserType != null && gUserType != "")
//            {
//                lUserType = gUserType;
//            }
//            else
//            {
//                UserAccessController lUa = new UserAccessController();
//                lUserType = lUa.getUserType(UserName);
//                gUserType = lUserType;
//                lUa = null;
//            }

//            var content1 = new ProjectAccessModels();

//            if (lUserType == "CU" || lUserType == "CA" || lUserType == "CM")
//            {
//                lCmd.CommandText = "SELECT " +
//                "P.CustomerCode, " +
//                "P.ProjectCode, " +
//                "P.ProjectTitle, " +
//                "SiteEngr_Name, " +
//                "SiteEngr_HP, " +
//                "SiteEngr_Tel, " +
//                "Scheduler_Name, " +
//                "Scheduler_HP, " +
//                "Scheduler_Tel, " +
//                "Contact1, " +
//                "Contact2, " +
//                "Contact3, " +
//                "Contact4, " +
//                "Contact5, " +
//                "Contact6, " +
//                "Tel1, " +
//                "Tel2, " +
//                "Tel3, " +
//                "Tel4, " +
//                "Tel5, " +
//                "Tel6, " +
//                "Email1, " +
//                "Email2, " +
//                "Email3, " +
//                "Email4, " +
//                "Email5, " +
//                "Email6, " +
//                "MaxBarLength, " +
//                "OrderSubmission, " +
//                "OrderCreation " +
//                "FROM dbo.OESProject P, dbo.OESUserAccess U " +
//                "WHERE P.CustomerCode = U.CustomerCode " +
//                "AND P.ProjectCode = U.ProjectCode " +
//                "AND U.UserName = '" + User.Identity.Name + "' " +
//                "AND U.CustomerCode = '" + CustomerCode + "' " +
//                "AND U.ProjectCode = '" + ProjectCode + "' ";
//            }
//            else if (lUserType == "PL" || lUserType == "AD" || lUserType == "PM" || lUserType == "PA" || lUserType == "P1" || lUserType == "P2" || lUserType == "PU" || lUserType == "TE")
//            {
//                lCmd.CommandText = "SELECT " +
//                "CustomerCode, " +
//                "ProjectCode, " +
//                "ProjectTitle, " +
//                "SiteEngr_Name, " +
//                "SiteEngr_HP, " +
//                "SiteEngr_Tel, " +
//                "Scheduler_Name, " +
//                "Scheduler_HP, " +
//                "Scheduler_Tel, " +
//                "Contact1, " +
//                "Contact2, " +
//                "Contact3, " +
//                "Contact4, " +
//                "Contact5, " +
//                "Contact6, " +
//                "Tel1, " +
//                "Tel2, " +
//                "Tel3, " +
//                "Tel4, " +
//                "Tel5, " +
//                "Tel6, " +
//                "Email1, " +
//                "Email2, " +
//                "Email3, " +
//                "Email4, " +
//                "Email5, " +
//                "Email6, " +
//                "MaxBarLength, " +
//                "'Yes' as OrderSubmission, " +
//                "'Yes' as OrderCreation " +
//                "FROM dbo.OESProject P " +
//                "WHERE P.CustomerCode = '" + CustomerCode + "' " +
//                "AND P.ProjectCode = '" + ProjectCode + "' ";
//            }
//            else
//            {
//                lCmd.CommandText = "SELECT " +
//                "CustomerCode, " +
//                "ProjectCode, " +
//                "ProjectTitle, " +
//                "SiteEngr_Name, " +
//                "SiteEngr_HP, " +
//                "SiteEngr_Tel, " +
//                "Scheduler_Name, " +
//                "Scheduler_HP, " +
//                "Scheduler_Tel, " +
//                "Contact1, " +
//                "Contact2, " +
//                "Contact3, " +
//                "Contact4, " +
//                "Contact5, " +
//                "Contact6, " +
//                "Tel1, " +
//                "Tel2, " +
//                "Tel3, " +
//                "Tel4, " +
//                "Tel5, " +
//                "Tel6, " +
//                "Email1, " +
//                "Email2, " +
//                "Email3, " +
//                "Email4, " +
//                "Email5, " +
//                "Email6, " +
//                "MaxBarLength, " +
//                "'No' as OrderSubmission, " +
//                "'No' as OrderCreation " +
//                "FROM dbo.OESProject P " +
//                "WHERE P.CustomerCode = '" + CustomerCode + "' " +
//                "AND P.ProjectCode = '" + ProjectCode + "' ";
//            }

//            var lProcessObj = new ProcessController();
//            if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
//            {
//L1:

//                lCmd.Connection = lNDSCon;
//                lCmd.CommandTimeout = 300;
//                lRst = lCmd.ExecuteReader();
//                if (lRst.HasRows)
//                {
//                    if (lRst.Read())
//                    {
//                        content1 = new ProjectAccessModels
//                        {
//                            CustomerCode = lRst.GetValue(0) == DBNull.Value ? "" : lRst.GetString(0).Trim(),
//                            ProjectCode = lRst.GetValue(1) == DBNull.Value ? "" : lRst.GetString(1).Trim(),
//                            ProjectTitle = lRst.GetValue(2) == DBNull.Value ? "" : lRst.GetString(2).Trim(),
//                            SiteEngr_Name = lRst.GetValue(3) == DBNull.Value ? "" : lRst.GetString(3).Trim(),
//                            SiteEngr_HP = lRst.GetValue(4) == DBNull.Value ? "" : lRst.GetString(4).Trim(),
//                            SiteEngr_Tel = lRst.GetValue(5) == DBNull.Value ? "" : lRst.GetString(5).Trim(),
//                            Scheduler_Name = lRst.GetValue(6) == DBNull.Value ? "" : lRst.GetString(6).Trim(),
//                            Scheduler_HP = lRst.GetValue(7) == DBNull.Value ? "" : lRst.GetString(7).Trim(),
//                            Scheduler_Tel = lRst.GetValue(8) == DBNull.Value ? "" : lRst.GetString(8).Trim(),
//                            Contact1 = lRst.GetValue(9) == DBNull.Value ? "" : lRst.GetString(9).Trim(),
//                            Contact2 = lRst.GetValue(10) == DBNull.Value ? "" : lRst.GetString(10).Trim(),
//                            Contact3 = lRst.GetValue(11) == DBNull.Value ? "" : lRst.GetString(11).Trim(),
//                            Contact4 = lRst.GetValue(12) == DBNull.Value ? "" : lRst.GetString(12).Trim(),
//                            Contact5 = lRst.GetValue(13) == DBNull.Value ? "" : lRst.GetString(13).Trim(),
//                            Contact6 = lRst.GetValue(14) == DBNull.Value ? "" : lRst.GetString(14).Trim(),
//                            Tel1 = lRst.GetValue(15) == DBNull.Value ? "" : lRst.GetString(15).Trim(),
//                            Tel2 = lRst.GetValue(16) == DBNull.Value ? "" : lRst.GetString(16).Trim(),
//                            Tel3 = lRst.GetValue(17) == DBNull.Value ? "" : lRst.GetString(17).Trim(),
//                            Tel4 = lRst.GetValue(18) == DBNull.Value ? "" : lRst.GetString(18).Trim(),
//                            Tel5 = lRst.GetValue(19) == DBNull.Value ? "" : lRst.GetString(19).Trim(),
//                            Tel6 = lRst.GetValue(20) == DBNull.Value ? "" : lRst.GetString(20).Trim(),
//                            Email1 = lRst.GetValue(21) == DBNull.Value ? "" : lRst.GetString(21).Trim(),
//                            Email2 = lRst.GetValue(22) == DBNull.Value ? "" : lRst.GetString(22).Trim(),
//                            Email3 = lRst.GetValue(23) == DBNull.Value ? "" : lRst.GetString(23).Trim(),
//                            Email4 = lRst.GetValue(24) == DBNull.Value ? "" : lRst.GetString(24).Trim(),
//                            Email5 = lRst.GetValue(25) == DBNull.Value ? "" : lRst.GetString(25).Trim(),
//                            Email6 = lRst.GetValue(26) == DBNull.Value ? "" : lRst.GetString(26).Trim(),
//                            MaxBarLength = lRst.GetValue(27) == DBNull.Value ? 12000 : lRst.GetInt32(27),
//                            OrderSubmission = lRst.GetValue(28) == DBNull.Value ? "No" : lRst.GetString(28).Trim(),
//                            OrderCreation = lRst.GetValue(29) == DBNull.Value ? "No" : lRst.GetString(29).Trim()
//                        };
//                    }
//                }
//                lRst.Close();

//                if ((content1.CustomerCode == null || content1.ProjectCode == null) && CustomerCode != null && ProjectCode != null)
//                {
//                    if (CustomerCode.Trim().Length > 0 && ProjectCode.Trim().Length > 0)
//                    {
//                        createProject(CustomerCode, ProjectCode);
//                        goto L1;
//                    }
//                }

//                lCmd.CommandText =
//                "SELECT isNull(MAX(JobID), 0) FROM dbo.OESCTSMESHJobAdvice " +
//                "WHERE CustomerCode = '" + CustomerCode + "' " +
//                "AND ProjectCode = '" + ProjectCode + "' " +
//                "AND OrderStatus <> 'New' " +
//                "AND OrderStatus <> 'Created' " +
//                "AND (SiteEngr_Name > '' " +
//                "OR Scheduler_Name > '' " +
//                "OR DeliveryAddress > '') ";

//                int LastJobID = 0;

//                lCmd.Connection = lNDSCon;
//                lCmd.CommandTimeout = 300;
//                lRst = lCmd.ExecuteReader();
//                if (lRst.HasRows)
//                {
//                    if (lRst.Read())
//                    {
//                        LastJobID = lRst.GetInt32(0);
//                    }
//                }
//                lRst.Close();

//                if (LastJobID > 0)
//                {
//                    if (lUserType == "CU" || lUserType == "CA")
//                    {
//                        lCmd.CommandText =
//                        "SELECT isNull(MAX(JobID), 0) FROM dbo.OESCTSMESHJobAdvice " +
//                        "WHERE CustomerCode = '" + CustomerCode + "' " +
//                        "AND ProjectCode = '" + ProjectCode + "' " +
//                        "AND OrderStatus <> 'New' " +
//                        "AND OrderStatus <> 'Created' " +
//                        "AND UpdateBy = '" + User.Identity.GetUserName() + "' " +
//                        "AND(SiteEngr_Name > '' " +
//                        "OR Scheduler_Name > '' " +
//                        "OR DeliveryAddress > '') ";

//                        int LastMyJobID = 0;

//                        lCmd.Connection = lNDSCon;
//                        lCmd.CommandTimeout = 300;
//                        lRst = lCmd.ExecuteReader();
//                        if (lRst.HasRows)
//                        {
//                            if (lRst.Read())
//                            {
//                                LastMyJobID = lRst.GetInt32(0);
//                            }
//                        }
//                        lRst.Close();


//                        lCmd.CommandText =
//                        "SELECT isNull(SiteEngr_Name, ''), " +
//                        "isNull(SiteEngr_HP, ''), " +
//                        "isNull(SiteEngr_Tel, ''), " +
//                        "isNull(Scheduler_Name, ''), " +
//                        "isNull(Scheduler_HP, ''), " +
//                        "isNull(Scheduler_Tel, ''), " +
//                        "isNull(DeliveryAddress, '') " +
//                        "FROM dbo.OESCTSMESHJobAdvice " +
//                        "WHERE CustomerCode = '" + CustomerCode + "' " +
//                        "AND ProjectCode = '" + ProjectCode + "' " +
//                        "AND JobID = " + LastMyJobID.ToString() + " " +
//                        "AND UpdateBy = '" + User.Identity.GetUserName() + "' " +
//                        "UNION " +
//                        "SELECT isNull(SiteEngr_Name, ''), " +
//                        "isNull(SiteEngr_HP, ''), " +
//                        "isNull(SiteEngr_Tel, ''), " +
//                        "isNull(Scheduler_Name, ''), " +
//                        "isNull(Scheduler_HP, ''), " +
//                        "isNull(Scheduler_Tel, ''), " +
//                        "isNull(DeliveryAddress, '') " +
//                        "FROM dbo.OESCTSMESHJobAdvice " +
//                        "WHERE CustomerCode = '" + CustomerCode + "' " +
//                        "AND ProjectCode = '" + ProjectCode + "' " +
//                        "AND JobID = " + LastJobID.ToString() + " ";
//                    }
//                    else
//                    {
//                        lCmd.CommandText =
//                        "SELECT isNull(SiteEngr_Name,''), " +
//                        "isNull(SiteEngr_HP,''), " +
//                        "isNull(SiteEngr_Tel,''), " +
//                        "isNull(Scheduler_Name,''), " +
//                        "isNull(Scheduler_HP,''), " +
//                        "isNull(Scheduler_Tel,''), " +
//                        "isNull(DeliveryAddress, '') " +
//                        "FROM dbo.OESCTSMESHJobAdvice " +
//                        "WHERE CustomerCode = '" + CustomerCode + "' " +
//                        "AND ProjectCode = '" + ProjectCode + "' " +
//                        "AND JobID = " + LastJobID.ToString() + " ";
//                    }

//                    lCmd.Connection = lNDSCon;
//                    lCmd.CommandTimeout = 300;
//                    lRst = lCmd.ExecuteReader();
//                    if (lRst.HasRows)
//                    {
//                        if (lRst.Read())
//                        {
//                            content1.SiteEngr_Name = lRst.GetString(0);
//                            content1.SiteEngr_HP = lRst.GetString(1);
//                            content1.SiteEngr_Tel = lRst.GetString(2);
//                            content1.Scheduler_Name = lRst.GetString(3);
//                            content1.Scheduler_HP = lRst.GetString(4);
//                            content1.Scheduler_Tel = lRst.GetString(5);
//                        }
//                    }
//                    lRst.Close();

//                }

//                lProcessObj.CloseNDSConnection(ref lNDSCon);
//            }

//            lProcessObj = null;

//            //var content = db.Project.Find(CustomerCode, ProjectCode);
//            //var content1 = new ProjectAccessModels();

//            //if (content != null) {
//            //    if (lUserType == "CU")
//            //    {
//            //        var lAccess = db.UserAccess.Find(User.Identity.Name, CustomerCode, ProjectCode);
//            //        if (lAccess != null)
//            //        {
//            //            lSumission = lAccess.OrderSubmission.Trim();
//            //            lEditable = lAccess.OrderCreation.Trim();
//            //        }
//            //    }
//            //    content1 = new ProjectAccessModels
//            //    {
//            //        CustomerCode = content.CustomerCode,
//            //        ProjectCode = content.ProjectCode,
//            //        ProjectTitle = content.ProjectTitle,
//            //        SiteEngr_Name = content.SiteEngr_Name,
//            //        SiteEngr_HP = content.SiteEngr_HP,
//            //        SiteEngr_Tel = content.SiteEngr_Tel,
//            //        Scheduler_Name = content.Scheduler_Name,
//            //        Scheduler_HP = content.Scheduler_HP,
//            //        Scheduler_Tel = content.Scheduler_Tel,
//            //        Contact1 = content.Contact1,
//            //        Contact2 = content.Contact2,
//            //        Contact3 = content.Contact3,
//            //        Contact4 = content.Contact4,
//            //        Contact5 = content.Contact5,
//            //        Contact6 = content.Contact6,
//            //        Tel1 = content.Tel1,
//            //        Tel2 = content.Tel2,
//            //        Tel3 = content.Tel3,
//            //        Tel4 = content.Tel4,
//            //        Tel5 = content.Tel5,
//            //        Tel6 = content.Tel6,
//            //        Email1 = content.Email1,
//            //        Email2 = content.Email2,
//            //        Email3 = content.Email3,
//            //        Email4 = content.Email4,
//            //        Email5 = content.Email5,
//            //        Email6 = content.Email6,
//            //        OrderSubmission = lSumission,
//            //        OrderCreation = lEditable
//            //    };
//            //}

//            //set kookie for customer and project
//            //HttpCookie lCustCookies = new HttpCookie("nsh_digios_cust");
//            //lCustCookies.Value = CustomerCode;
//            //lCustCookies.Expires = DateTime.Now.AddDays(30);
//            //HttpContext.Response.Cookies.Remove("nsh_digios_cust");
//            //HttpContext.Response.SetCookie(lCustCookies);

//            //HttpCookie lProjCookies = new HttpCookie("nsh_digios_proj");
//            //lProjCookies.Value = ProjectCode;
//            //lProjCookies.Expires = DateTime.Now.AddDays(30);
//            //HttpContext.Response.Cookies.Remove("nsh_digios_proj");
//            //HttpContext.Response.SetCookie(lProjCookies);

//            //return Json(content1, JsonRequestBehavior.AllowGet);
//            return Ok(content1);
//        }


        [HttpPost]
        [Route("/getDeliveryAddress_beam/{CustomerCode}/{ProjectCode}/{WBS1}")]
        //[ValidateAntiForgeryHeader]
        public ActionResult getDeliveryAddress(string CustomerCode, string ProjectCode, string WBS1)
        {
            var lCmd = new SqlCommand();
            SqlDataReader lRst;
            var lNDSCon = new SqlConnection();
            var lErrorMsg = "";
            int lFound = 0;
            int? lJobID = 0;

            var lReturn = new
            {
                SiteEngr_Name = "",
                SiteEngr_HP = "",
                SiteEngr_Tel = "",
                Scheduler_Name = "",
                Scheduler_HP = "",
                Scheduler_Tel = "",
                DeliveryAddress = "",
                Remarks = ""
            };

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
                lUserType = lUa.getUserType(User.Identity.GetUserName());
                gUserType = lUserType;
                lUa = null;
            }
            if (lUserType == "TE" || lUserType == "CU" || lUserType == "CA" || lUserType == "CM" || lUserType == "PL" || lUserType == "AD" || lUserType == "PM" || lUserType == "PA" || lUserType == "P1" || lUserType == "P2" || lUserType == "PU")
            {
                var lAccess = db.UserAccess.Find(User.Identity.Name, CustomerCode, ProjectCode);
                if (lAccess != null)
                {
                    lSumission = lAccess.OrderSubmission.Trim();
                    lEditable = lAccess.OrderCreation.Trim();
                }
                if (lUserType == "TE" || lUserType == "PL" || lUserType == "AD" || lUserType == "PM" || lUserType == "PA" || lUserType == "P1" || lUserType == "P2" || lUserType == "PU")
                {
                    lEditable = "Yes";
                }

                if (lEditable == "Yes")
                {
                    var lProcessObj = new ProcessController();
                    if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
                    {
                        lCmd.CommandText =
                        "SELECT isNull(MAX(JobID), 0) FROM dbo.OESCTSMESHJobAdvice " +
                        "WHERE CustomerCode = '" + CustomerCode + "' " +
                        "AND ProjectCode = '" + ProjectCode + "' " +
                        "AND OrderStatus <> 'New' " +
                        "AND OrderStatus <> 'Created' " +
                        "AND WBS1 = '" + WBS1 + "' " +
                        "AND (SiteEngr_Name > '' " +
                        "OR Scheduler_Name > '' " +
                        "OR DeliveryAddress > '') ";

                        int lLastJobID = 0;

                        lCmd.Connection = lNDSCon;
                        lCmd.CommandTimeout = 300;
                        lRst = lCmd.ExecuteReader();
                        if (lRst.HasRows)
                        {
                            if (lRst.Read())
                            {
                                lLastJobID = lRst.GetInt32(0);
                            }
                        }
                        lRst.Close();

                        if (lLastJobID == 0)
                        {
                            lCmd.CommandText =
                                "SELECT isNull(MAX(JobID), 0) FROM dbo.OESCTSMESHJobAdvice " +
                                "WHERE CustomerCode = '" + CustomerCode + "' " +
                                "AND ProjectCode = '" + ProjectCode + "' " +
                                "AND OrderStatus <> 'New' " +
                                "AND OrderStatus <> 'Created' " +
                                "AND UpdateBy = '" + User.Identity.GetUserName() + "' " +
                                "AND(SiteEngr_Name > '' " +
                                "OR Scheduler_Name > '' " +
                                "OR DeliveryAddress > '') ";

                            lCmd.Connection = lNDSCon;
                            lCmd.CommandTimeout = 300;
                            lRst = lCmd.ExecuteReader();
                            if (lRst.HasRows)
                            {
                                if (lRst.Read())
                                {
                                    lLastJobID = lRst.GetInt32(0);
                                }
                            }
                            lRst.Close();
                        }

                        if (lLastJobID > 0)
                        {

                            lCmd.CommandText =
                            "SELECT isNull(SiteEngr_Name, ''), " +
                            "isNull(SiteEngr_HP, ''), " +
                            "isNull(SiteEngr_Tel, ''), " +
                            "isNull(Scheduler_Name, ''), " +
                            "isNull(Scheduler_HP, ''), " +
                            "isNull(Scheduler_Tel, ''), " +
                            "isNull(DeliveryAddress, ''), " +
                            "isNull(Remarks, '') " +
                            "FROM dbo.OESCTSMESHJobAdvice " +
                            "WHERE CustomerCode = '" + CustomerCode + "' " +
                            "AND ProjectCode = '" + ProjectCode + "' " +
                            "AND JobID = " + lLastJobID.ToString() + " ";

                            lCmd.Connection = lNDSCon;
                            lCmd.CommandTimeout = 300;
                            lRst = lCmd.ExecuteReader();
                            if (lRst.HasRows)
                            {
                                if (lRst.Read())
                                {
                                    lReturn = new
                                    {
                                        SiteEngr_Name = lRst.GetString(0),
                                        SiteEngr_HP = lRst.GetString(1),
                                        SiteEngr_Tel = lRst.GetString(2),
                                        Scheduler_Name = lRst.GetString(3),
                                        Scheduler_HP = lRst.GetString(4),
                                        Scheduler_Tel = lRst.GetString(5),
                                        DeliveryAddress = lRst.GetString(6),
                                        Remarks = lRst.GetString(7)
                                    };
                                }
                            }
                            lRst.Close();

                        }

                        lProcessObj.CloseNDSConnection(ref lNDSCon);
                    }

                    lProcessObj = null;
                }
            }

            //return Json(lReturn, JsonRequestBehavior.AllowGet);
            return Ok(lReturn);
        }

        //[HttpGet]
        //[Route("/getDeliveryAddress_beam/{CustomerCode}/{ProjectCode}")]
        //public void createProject(string CustomerCode, string ProjectCode)
        //{
        //    var lCISCon = new OracleConnection();
        //    var lOraCmd = new OracleCommand();
        //    OracleDataReader lOraRst;

        //    var projectModels = new ProjectModels();
        //    projectModels.CustomerCode = CustomerCode;
        //    projectModels.ProjectCode = ProjectCode;
        //    projectModels.MaxBarLength = 12000;
        //    projectModels.ProjectCAB = false;
        //    projectModels.ProjectMESH = false;
        //    projectModels.ProjectBPC = false;
        //    projectModels.ProjectCage = false;

        //    projectModels.EspliceN = false;
        //    projectModels.EspliceS = false;
        //    projectModels.Nsplice = false;

        //    var lProjType = getProjectType(projectModels.CustomerCode, projectModels.ProjectCode);
        //    var lProjTypeArr = lProjType.Split(',');
        //    if (lProjTypeArr.Length >= 4)
        //    {
        //        if (lProjTypeArr[0] != "0")
        //        {
        //            projectModels.ProjectCAB = true;

        //        }
        //        if (lProjTypeArr[1] != "0")
        //        {
        //            projectModels.ProjectMESH = true;

        //        }
        //        if (lProjTypeArr[2] != "0")
        //        {
        //            projectModels.ProjectBPC = true;

        //        }
        //        if (lProjTypeArr[3] != "0")
        //        {
        //            projectModels.ProjectCage = true;

        //        }
        //    }

        //    string lSegment = "";

        //    var lProcess = new ProcessController();
        //    if (lProcess.OpenCISConnection(ref lCISCon) == true)
        //    {
        //        lOraCmd.CommandText = "select NAME1, TELF1, SMTP_ADDR AS EMAIL " +
        //        "FROM SAPSR3.KNA1 left join SAPSR3.ADR6 ON SAPSR3.KNA1.MANDT = SAPSR3.ADR6.CLIENT " +
        //        "AND SAPSR3.KNA1.ADRNR = SAPSR3.ADR6.ADDRNUMBER " +
        //        "WHERE SAPSR3.KNA1.AUFSD <> '01' AND " +
        //        "(MANDT, KUNNR) in  " +
        //        "(SELECT MANDT, KUNNR FROM SAPSR3.VBPA WHERE MANDT = '600' " +
        //        "AND PARVW = 'Z3' AND VBELN = " +
        //        "(SELECT VBELN FROM " +
        //        "(SELECT NVL(VBELN,' ') as VBELN FROM SAPSR3.VBPA WHERE MANDT = '600' AND VBELN like '103%' and " +
        //        "KUNNR = '" + ProjectCode + "' " +
        //        "ORDER BY VBELN DESC)  " +
        //        "WHERE ROWNUM = 1 " +
        //        ")) ";

        //        lOraCmd.Connection = lCISCon;
        //        lOraCmd.CommandType = CommandType.Text;
        //        lOraCmd.CommandTimeout = 1200;
        //        lOraRst = lOraCmd.ExecuteReader();
        //        if (lOraRst.HasRows)
        //        {
        //            lOraRst.Read();
        //            projectModels.ProjectTitle = lOraRst.GetString(2);
        //            lSegment = lOraRst.GetString(1);
        //        }

        //        lOraRst.Close();

        //        if (lSegment != "")
        //        {
        //            var lUser = db.UserAccess.Find(lSegment, "0000000000", "0000000000");
        //            if (lUser != null)
        //            {
        //                lSegment = lUser.UserType;
        //                if (lSegment == "P1")
        //                {
        //                    lSegment = "S1";
        //                }
        //                else if (lSegment == "P2")
        //                {
        //                    lSegment = "S2";
        //                }
        //                else
        //                {
        //                    lSegment = "";
        //                }
        //            }
        //            else
        //            {
        //                lSegment = "";
        //            }
        //        }

        //        lOraCmd.CommandText = "SELECT (NAME1 || NAME2) AS SHIP_TO_NAME,KUNNR AS Name, BRAN2 " +
        //            "FROM SAPSR3.KNA1 " +
        //            "WHERE KTOKD = 'Y001' AND MANDT = '600' " +
        //            "AND KUNNR = '" + ProjectCode + "' ";

        //        lOraCmd.Connection = lCISCon;
        //        lOraCmd.CommandType = CommandType.Text;
        //        lOraCmd.CommandTimeout = 1200;
        //        lOraRst = lOraCmd.ExecuteReader();
        //        if (lOraRst.HasRows)
        //        {
        //            lOraRst.Read();
        //            projectModels.ProjectTitle = lOraRst.GetString(0);
        //            if (lSegment != "")
        //            {
        //                lSegment = lOraRst.GetString(1);
        //            }
        //        }

        //        lOraRst.Close();
        //        lProcess.CloseCISConnection(ref lCISCon);

        //    }

        //    lProcess = null;

        //    if (lSegment.Trim() == "S2")
        //    {
        //        projectModels.Contact1 = "Customer Support Group 3";
        //        projectModels.Email1 = "csd3@natsteel.com.sg";
        //    }
        //    else
        //    {
        //        projectModels.Contact1 = "Customer Support Group 1";
        //        projectModels.Email1 = "csd1@natsteel.com.sg";
        //    }
        //    projectModels.EmailDistribution = "";
        //    projectModels.AdvancedOrder = false;
        //    projectModels.UpdateDate = DateTime.Now;
        //    db.Project.Add(projectModels);
        //    db.SaveChanges();
        //}


        //[HttpGet]
        //[Route("/getProjectType_beam/{pCustomerCode}/{pProjectCode}")]
        //string getProjectType(string pCustomerCode, string pProjectCode)
        //{
        //    OracleDataReader lRst;
        //    var lCmd = new OracleCommand();
        //    var lcisCon = new OracleConnection();
        //    string lReturn = "0,0,0,0";

        //    var lProcessObj = new ProcessController();
        //    if (lProcessObj.OpenCISConnection(ref lcisCon) == true)
        //    {
        //        lCmd.CommandText = "SELECT NVL(SUM(K.ytot_cab + K.ytot_rebar),0), NVL(SUM(K.ytot_mesh),0), NVL(SUM(K.ytot_bpc),0), NVL(SUM(K.ytot_precage),0) " +
        //                "FROM SAPSR3.VBAK K, SAPSR3.VBPA P " +
        //                "WHERE K.MANDT = '" + lProcessObj.strClient + "' " +
        //                "AND (K.VBELN like '102%' OR K.VBELN like '_102%') " +
        //                "AND K.VKORG = '" + lProcessObj.strSalesOrg + "' " +
        //                "AND K.KUNNR = '" + pCustomerCode + "' " +
        //                "AND K.TRVOG = '4' " +
        //                "AND K.MANDT = P.MANDT " +
        //                "AND K.VBELN = P.VBELN " +
        //                "AND P.KUNNR = '" + pProjectCode + "' " +
        //                "AND to_date(K.GUEEN,'YYYYMMDD') >= (SYSDATE - 365) ";

        //        lCmd.Connection = lcisCon;
        //        lCmd.CommandTimeout = 1200;
        //        lRst = lCmd.ExecuteReader();
        //        if (lRst.HasRows)
        //        {
        //            if (lRst.Read())
        //            {
        //                lReturn = lRst.GetDecimal(0).ToString() + ","
        //                + lRst.GetDecimal(1).ToString() + ","
        //                + lRst.GetDecimal(2).ToString() + ","
        //                + lRst.GetDecimal(3).ToString();
        //            }
        //        }
        //        lRst.Close();

        //    }

        //    lProcessObj.CloseCISConnection(ref lcisCon);
        //    lCmd = null;
        //    lRst = null;
        //    lProcessObj = null;

        //    return lReturn;
        //}

        //[HttpPost]
        //[Route("/getOrderList_beam/{CustomerCode}/{ProjectCode}")]
        //// [ValidateAntiForgeryHeader]
        //public ActionResult getOrderList(string CustomerCode, string ProjectCode)
        //{
        //    createJobAdvice(CustomerCode, ProjectCode, false);
        //    updateOrderStatus(CustomerCode, ProjectCode);

        //    //var content = (from p in db.CTSMESHJobAdvice
        //    //               join s in db.CTSMESHPODoc
        //    //               on new {a=p.CustomerCode, b=p.ProjectCode, c=p.JobID, d=1 } equals
        //    //               new {a=s.CustomerCode, b=s.ProjectCode, c=s.JobID, d=s.PODocID } into s1
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

        //    var content = (from p in db.CTSMESHJobAdvice
        //                   let s = db.CTSMESHPODoc.Where(s2 => p.CustomerCode == s2.CustomerCode &&
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
        //                   ).Take(50).ToList();

        //    var content1 = new List<struOrderList>(content.Select(h => new struOrderList
        //    {
        //        OrderNo = h.JobID.ToString(),
        //        OrderDesc = h.JobID.ToString() + " PO:" + (h.PONumber == null ? "" : h.PONumber.Trim()) + " RD:"
        //        + (h.RequiredDate == null ? "" : ((DateTime)h.RequiredDate).ToString("yyyy-MM-dd"))
        //        + " Status:" + (h.OrderStatus == null ? "" : h.OrderStatus.Trim()
        //        + (h.FileName == null ? "" : " PO Attached"))
        //    }));

        //    // return Json(content1, JsonRequestBehavior.AllowGet);
        //    return Ok(content1);
        //}

        [HttpPost]
        [Route("/getCopyOrderList_beam/{CustomerCode}/{ProjectCode}/{CopyModel}")]
        //[ValidateAntiForgeryHeader]
        public ActionResult getCopyOrderList(string CustomerCode, string ProjectCode, string CopyModel)
        {
            var content1 = new List<struOrderList>();
            if (CopyModel == "All")
            {
                var content = (from p in db.CTSMESHJobAdvice
                               where p.CustomerCode == CustomerCode &&
                               p.ProjectCode == ProjectCode
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
                    + (h.RequiredDate == null ? "" : ((DateTime)h.RequiredDate).ToString("yyyy-MM-dd"))
                    + " Status:" + (h.OrderStatus == null ? "" : h.OrderStatus.Trim())
                }));
            }
            else if (CopyModel == "Orders")
            {
                var content = (from p in db.CTSMESHJobAdvice
                               where p.CustomerCode == CustomerCode &&
                               p.ProjectCode == ProjectCode &&
                               p.Model == false
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
                    + (h.RequiredDate == null ? "" : ((DateTime)h.RequiredDate).ToString("yyyy-MM-dd"))
                    + " Status:" + (h.OrderStatus == null ? "" : h.OrderStatus.Trim())
                }));

            }
            else
            {
                var content = (from p in db.CTSMESHJobAdvice
                               where p.CustomerCode == CustomerCode &&
                               p.ProjectCode == ProjectCode &&
                               p.Model == true
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

            //            return Json(content1, JsonRequestBehavior.AllowGet);
            return Ok(content1);
        }


        //[HttpGet]
        //[Route("/updateOrderStatus_beam/{pCustomerCode}/{pProjectCode}")]
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
        //    string lSOR = "";
        //    int lJOBID = 0;
        //    int lBBSID = 0;
        //    string lSQL = "";
        //    string lOutTime = "";

        //    var lProcessObj = new ProcessController();
        //    if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
        //    {
        //        lCmd.CommandText = "SELECT B.BBSSOR, B.JobID, B.BBSID " +
        //        "FROM dbo.OESCTSMESHBBS B, dbo.OESCTSMESHJobAdvice A " +
        //        "WHERE A.CustomerCode = '" + pCustomerCode + "' " +
        //        "AND A.ProjectCode = '" + pProjectCode + "' " +
        //        "AND A.CustomerCode = B.CustomerCode " +
        //        "AND A.ProjectCode = B.ProjectCode " +
        //        "AND A.JobID = B.JobID " +
        //        "AND B.BBSSOR > '' " +
        //        "AND A.OrderStatus = 'Processed' ";

        //        lCmd.Connection = lNDSCon;
        //        lCmd.CommandTimeout = 300;
        //        lRst = lCmd.ExecuteReader();
        //        if (lRst.HasRows)
        //        {
        //            if (lProcessObj.OpenCISConnection(ref lCISCon) == true)
        //            {
        //                while (lRst.Read())
        //                {
        //                    lSOR = lRst.GetValue(0) == DBNull.Value ? "" : lRst.GetString(0).Trim();
        //                    lJOBID = lRst.GetValue(1) == DBNull.Value ? 0 : lRst.GetInt32(1);
        //                    lBBSID = lRst.GetValue(2) == DBNull.Value ? 0 : lRst.GetInt32(2);

        //                    lSQL = "SELECT  " +
        //                    "NVL(MAX(WEIGH_OUT_TIME), ' ') " +
        //                    "FROM SAPSR3.YMPPT_LOAD_VEHIC V, " +
        //                    "SAPSR3.YMSDT_ORDER_HDR O " +
        //                    "WHERE O.MANDT = V.MANDT " +
        //                    "AND O.SALES_ORDER = V.VBELN " +
        //                    "AND O.MANDT = '" + lProcessObj.strClient + "' " +
        //                    "AND O.ORDER_REQUEST_NO = '" + lSOR + "' " +
        //                    "UNION " +
        //                    "SELECT  " +
        //                    "NVL(MAX(WEIGH_OUT_TIME), ' ') " +
        //                    "FROM SAPSR3.YMPPT_LOAD_VEHIC " +
        //                    "WHERE MANDT = '" + lProcessObj.strClient + "' " +
        //                    "AND VBELN = '" + lSOR + "' " +
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
        //                                lCmdUpdate.CommandText = "Update dbo.OESCTSMESHJobAdvice " +
        //                                "SET OrderStatus = 'Delivered' " +
        //                                "WHERE CustomerCode = '" + pCustomerCode + "' " +
        //                                "AND ProjectCode = '" + pProjectCode + "' " +
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
        //        // NSH scheduled BBS

        //        lRst.Close();

        //        lCmd.CommandText = "SELECT B.BBSSOR, B.JobID, B.BBSID " +
        //            "FROM dbo.OESCTSMESHBBSNSH B, dbo.OESCTSMESHJobAdvice A " +
        //            "WHERE A.CustomerCode = '" + pCustomerCode + "' " +
        //            "AND A.ProjectCode = '" + pProjectCode + "' " +
        //            "AND A.CustomerCode = B.CustomerCode " +
        //            "AND A.ProjectCode = B.ProjectCode " +
        //            "AND A.JobID = B.JobID " +
        //            "AND B.BBSSOR > '' " +
        //            "AND A.OrderStatus = 'Processed' ";

        //        lCmd.Connection = lNDSCon;
        //        lCmd.CommandTimeout = 300;
        //        lRst = lCmd.ExecuteReader();
        //        if (lRst.HasRows)
        //        {
        //            if (lProcessObj.OpenCISConnection(ref lCISCon) == true)
        //            {
        //                while (lRst.Read())
        //                {
        //                    lSOR = lRst.GetValue(0) == DBNull.Value ? "" : lRst.GetString(0).Trim();
        //                    lJOBID = lRst.GetValue(1) == DBNull.Value ? 0 : lRst.GetInt32(1);
        //                    lBBSID = lRst.GetValue(2) == DBNull.Value ? 0 : lRst.GetInt32(2);

        //                    lSQL = "SELECT  " +
        //                    "NVL(MAX(WEIGH_OUT_TIME), ' ') " +
        //                    "FROM SAPSR3.YMPPT_LOAD_VEHIC V, " +
        //                    "SAPSR3.YMSDT_ORDER_HDR O " +
        //                    "WHERE O.MANDT = V.MANDT " +
        //                    "AND O.SALES_ORDER = V.VBELN " +
        //                    "AND O.MANDT = '" + lProcessObj.strClient + "' " +
        //                    "AND O.ORDER_REQUEST_NO = '" + lSOR + "' " +
        //                    "UNION " +
        //                    "SELECT  " +
        //                    "NVL(MAX(WEIGH_OUT_TIME), ' ') " +
        //                    "FROM SAPSR3.YMPPT_LOAD_VEHIC " +
        //                    "WHERE MANDT = '" + lProcessObj.strClient + "' " +
        //                    "AND VBELN = '" + lSOR + "' " +
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
        //                                lCmdUpdate.CommandText = "Update dbo.OESCTSMESHJobAdvice " +
        //                                "SET OrderStatus = 'Delivered' " +
        //                                "WHERE CustomerCode = '" + pCustomerCode + "' " +
        //                                "AND ProjectCode = '" + pProjectCode + "' " +
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
        [Route("/createJobAdvice_beam/{pCustomerCode}/{pProjectCode}/{pClone}")]
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
                        lUserType = lUa.getUserType(User.Identity.GetUserName());
                        gUserType = lUserType;
                        lUa = null;
                    }
                    if (lUserType == "TE" || lUserType == "CU" || lUserType == "CA" || lUserType == "CM" || lUserType == "PL" || lUserType == "AD" || lUserType == "PM" || lUserType == "PA" || lUserType == "P1" || lUserType == "P2" || lUserType == "PU")
                    {
                        var lAccess = db.UserAccess.Find(User.Identity.Name, pCustomerCode, pProjectCode);
                        if (lAccess != null)
                        {
                            lSumission = lAccess.OrderSubmission.Trim();
                            lEditable = lAccess.OrderCreation.Trim();
                        }
                        if (lUserType == "TE" || lUserType == "PL" || lUserType == "AD" || lUserType == "PM" || lUserType == "PA" || lUserType == "P1" || lUserType == "P2" || lUserType == "PU")
                        {
                            lEditable = "Yes";
                        }

                        if (lEditable == "Yes")
                        {
                            lFound = 0;

                            lJobID = db.CTSMESHJobAdvice.Where(z => z.CustomerCode == pCustomerCode &&
                            z.ProjectCode == pProjectCode).Max(z => (int?)z.JobID);

                            if (lJobID != null)
                            {
                                lFound = 1;

                                var lCheckBeam = (from p in db.CTSMESHBeamDetails
                                                  where p.CustomerCode == pCustomerCode &&
                                                  p.ProjectCode == pProjectCode &&
                                                  p.JobID == lJobID &&
                                                  p.MeshShapeCode != null &&
                                                  p.MeshShapeCode != "" &&
                                                  p.UpdateBy != User.Identity.Name
                                                  select p).ToList();

                                var lCheckColumn = (from p in db.CTSMESHColumnDetails
                                                    where p.CustomerCode == pCustomerCode &&
                                                    p.ProjectCode == pProjectCode &&
                                                    p.JobID == lJobID &&
                                                    p.MeshShapeCode != null &&
                                                    p.MeshShapeCode != "" &&
                                                    p.UpdateBy != User.Identity.Name
                                                    select p).ToList();

                                var lCheckSlab = (from p in db.CTSMESHOthersDetails
                                                  where p.CustomerCode == pCustomerCode &&
                                                  p.ProjectCode == pProjectCode &&
                                                  p.JobID == lJobID &&
                                                  p.MeshShapeCode != null &&
                                                  p.MeshShapeCode != "" &&
                                                  p.UpdateBy != User.Identity.Name
                                                  select p).ToList();

                                var lCheckBBS = (from p in db.CTSMESHBBS
                                                 where p.CustomerCode == pCustomerCode &&
                                                 p.ProjectCode == pProjectCode &&
                                                 p.JobID == lJobID &&
                                                 ((p.BBSStrucElem == "Slab" &&
                                                 p.BBSOrder == false) ||
                                                 (p.BBSStrucElem != "Slab" &&
                                                 p.BBSOrder == true) ||
                                                 (p.BBSDesc != null && p.BBSDesc != "")) &&
                                                 p.UpdateBy != User.Identity.Name
                                                 select p).ToList();

                                var lCheckBBSNSH = (from p in db.CTSMESHBBSNSH
                                                    where p.CustomerCode == pCustomerCode &&
                                                    p.ProjectCode == pProjectCode &&
                                                    p.JobID == lJobID &&
                                                    p.BBSID > 0 &&
                                                    p.BBSOrder == true &&
                                                    p.UpdateBy != User.Identity.Name
                                                    select p).ToList();

                                var lCheckJob = db.CTSMESHJobAdvice.Find(pCustomerCode, pProjectCode, lJobID);

                                if (lCheckJob == null || (lCheckJob.OrderStatus != "New" && lCheckJob.OrderStatus != "Created") ||
                                    (lCheckJob.TotalWeight > 0 &&
                                    (lCheckJob.UpdateBy != User.Identity.GetUserName() ||
                                    pClone == true)) ||
                                    (lCheckJob.PONumber != null && lCheckJob.PONumber != "") ||
                                    lCheckBeam.Count > 0 ||
                                    lCheckColumn.Count > 0 ||
                                    lCheckSlab.Count > 0 ||
                                    lCheckBBS.Count > 0 ||
                                    lCheckBBSNSH.Count > 0)
                                {
                                    lFound = 0;
                                }
                            }

                            if (lFound == 0)
                            {

                                var lJobAdv = new CTSMESHJobAdviceModels();
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
                                lJobAdv.AutoWBS = true;
                                lJobAdv.WBS1 = "";
                                lJobAdv.WBS2 = "";
                                lJobAdv.WBS3 = "";
                                lJobAdv.Model = false;

                                lJobAdv.OrderSource = "WEB";

                                var lProj = db.Project.Find(pCustomerCode, pProjectCode);
                                if (lProj != null)
                                {
                                    var lProcessObj = new ProcessController();
                                    if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
                                    {
                                        lCmd.CommandText =
                                        "SELECT isNull(MAX(JobID), 0) FROM dbo.OESCTSMESHJobAdvice " +
                                        "WHERE CustomerCode = '" + pCustomerCode + "' " +
                                        "AND ProjectCode = '" + pProjectCode + "' " +
                                        "AND OrderStatus <> 'New' " +
                                        "AND OrderStatus <> 'Created' " +
                                        "AND OrderStatus is not NULL " +
                                        "AND OrderStatus <> '' " +
                                        "AND(SiteEngr_Name > '' " +
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
                                                "SELECT isNull(MAX(JobID), 0) FROM dbo.OESCTSMESHJobAdvice " +
                                                "WHERE CustomerCode = '" + pCustomerCode + "' " +
                                                "AND ProjectCode = '" + pProjectCode + "' " +
                                                "AND OrderStatus <> 'New' " +
                                                "AND OrderStatus <> 'Created' " +
                                                "AND OrderStatus is not NULL " +
                                                "AND OrderStatus <> '' " +
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
                                                "FROM dbo.OESCTSMESHJobAdvice " +
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
                                                "FROM dbo.OESCTSMESHJobAdvice " +
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
                                                "isNull(Scheduler_Tel,''), " +
                                                "isNull(DeliveryAddress, '') " +
                                                "FROM dbo.OESCTSMESHJobAdvice " +
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
                                lJobAdv.OrderStatus = "New";
                                lJobAdv.UpdateDate = DateTime.Now;
                                lJobAdv.UpdateBy = User.Identity.GetUserName();



                                var oldJobAdvice = db.CTSMESHJobAdvice.Find(lJobAdv.CustomerCode, lJobAdv.ProjectCode, lJobAdv.JobID);
                                if (oldJobAdvice == null)
                                {
                                    db.CTSMESHJobAdvice.Add(lJobAdv);
                                }
                                else
                                {
                                    db.Entry(oldJobAdvice).CurrentValues.SetValues(lJobAdv);
                                }

                                var lBBS1 = new CTSMESHBBSModels();
                                lBBS1.UpdateDate = DateTime.Now;
                                lBBS1.UpdateBy = User.Identity.GetUserName();
                                lBBS1.CustomerCode = pCustomerCode;
                                lBBS1.ProjectCode = pProjectCode;
                                lBBS1.JobID = lJobAdv.JobID;

                                lBBS1.BBSID = 1;
                                lBBS1.BBSProdCategory = "Beam Stirrup Cage";
                                lBBS1.BBSStrucElem = "Beam";
                                lBBS1.BBSDesc = "";
                                lBBS1.BBSDrawing = "N";
                                lBBS1.BBSDrawingRev = "";
                                lBBS1.BBSTotalPcs = 0;
                                lBBS1.BBSTotalWT = 0;
                                lBBS1.BBSNDSGroupMark = "";
                                lBBS1.BBSSOR = "";
                                lBBS1.BBSOrder = false;

                                var oldBBS1 = db.CTSMESHBBS.Find(pCustomerCode, pProjectCode, lJobAdv.JobID, 1);
                                if (oldBBS1 == null)
                                {
                                    db.CTSMESHBBS.Add(lBBS1);
                                }

                                var lBBS2 = new CTSMESHBBSModels();
                                lBBS2.UpdateDate = DateTime.Now;
                                lBBS2.UpdateBy = User.Identity.GetUserName();
                                lBBS2.CustomerCode = pCustomerCode;
                                lBBS2.ProjectCode = pProjectCode;
                                lBBS2.JobID = lJobAdv.JobID;

                                lBBS2.BBSID = 2;
                                lBBS2.BBSProdCategory = "Column Link Cage";
                                lBBS2.BBSStrucElem = "Column";
                                lBBS2.BBSDesc = "";
                                lBBS2.BBSDrawing = "N";
                                lBBS2.BBSDrawingRev = "";
                                lBBS2.BBSTotalPcs = 0;
                                lBBS2.BBSTotalWT = 0;
                                lBBS2.BBSNDSGroupMark = "";
                                lBBS2.BBSSOR = "";
                                lBBS2.BBSOrder = false;

                                var oldBBS2 = db.CTSMESHBBS.Find(pCustomerCode, pProjectCode, lJobAdv.JobID, 2);
                                if (oldBBS2 == null)
                                {
                                    db.CTSMESHBBS.Add(lBBS2);
                                }

                                var lBBS3 = new CTSMESHBBSModels();
                                lBBS3.UpdateDate = DateTime.Now;
                                lBBS3.UpdateBy = User.Identity.GetUserName();
                                lBBS3.CustomerCode = pCustomerCode;
                                lBBS3.ProjectCode = pProjectCode;
                                lBBS3.JobID = lJobAdv.JobID;

                                lBBS3.BBSID = 3;
                                lBBS3.BBSProdCategory = "CTS MESH";
                                lBBS3.BBSStrucElem = "Slab";
                                lBBS3.BBSDesc = "";
                                lBBS3.BBSDrawing = "N";
                                lBBS3.BBSDrawingRev = "";
                                lBBS3.BBSTotalPcs = 0;
                                lBBS3.BBSTotalWT = 0;
                                lBBS3.BBSNDSGroupMark = "";
                                lBBS3.BBSSOR = "";
                                lBBS3.BBSOrder = true;

                                var oldBBS3 = db.CTSMESHBBS.Find(pCustomerCode, pProjectCode, lJobAdv.JobID, 3);
                                if (oldBBS3 == null)
                                {
                                    db.CTSMESHBBS.Add(lBBS3);
                                }

                                var lBBS4 = new CTSMESHBBSModels();
                                lBBS4.UpdateDate = DateTime.Now;
                                lBBS4.UpdateBy = User.Identity.GetUserName();
                                lBBS4.CustomerCode = pCustomerCode;
                                lBBS4.ProjectCode = pProjectCode;
                                lBBS4.JobID = lJobAdv.JobID;

                                lBBS4.BBSID = 4;
                                lBBS4.BBSProdCategory = "CTS MESH";
                                lBBS4.BBSStrucElem = "Slab Bottom";
                                lBBS4.BBSDesc = "";
                                lBBS4.BBSDrawing = "N";
                                lBBS4.BBSDrawingRev = "";
                                lBBS4.BBSTotalPcs = 0;
                                lBBS4.BBSTotalWT = 0;
                                lBBS4.BBSNDSGroupMark = "";
                                lBBS4.BBSSOR = "";
                                lBBS4.BBSOrder = false;

                                var oldBBS4 = db.CTSMESHBBS.Find(pCustomerCode, pProjectCode, lJobAdv.JobID, 4);
                                if (oldBBS4 == null)
                                {
                                    db.CTSMESHBBS.Add(lBBS4);
                                }

                                var lBBS5 = new CTSMESHBBSModels();
                                lBBS5.UpdateDate = DateTime.Now;
                                lBBS5.UpdateBy = User.Identity.GetUserName();
                                lBBS5.CustomerCode = pCustomerCode;
                                lBBS5.ProjectCode = pProjectCode;
                                lBBS5.JobID = lJobAdv.JobID;

                                lBBS5.BBSID = 5;
                                lBBS5.BBSProdCategory = "CTS MESH";
                                lBBS5.BBSStrucElem = "Slab Top";
                                lBBS5.BBSDesc = "";
                                lBBS5.BBSDrawing = "N";
                                lBBS5.BBSDrawingRev = "";
                                lBBS5.BBSTotalPcs = 0;
                                lBBS5.BBSTotalWT = 0;
                                lBBS5.BBSNDSGroupMark = "";
                                lBBS5.BBSSOR = "";
                                lBBS5.BBSOrder = false;

                                var oldBBS5 = db.CTSMESHBBS.Find(pCustomerCode, pProjectCode, lJobAdv.JobID, 5);
                                if (oldBBS5 == null)
                                {
                                    db.CTSMESHBBS.Add(lBBS5);
                                }

                                var lBBS6 = new CTSMESHBBSModels();
                                lBBS6.UpdateDate = DateTime.Now;
                                lBBS6.UpdateBy = User.Identity.GetUserName();
                                lBBS6.CustomerCode = pCustomerCode;
                                lBBS6.ProjectCode = pProjectCode;
                                lBBS6.JobID = lJobAdv.JobID;

                                lBBS6.BBSID = 6;
                                lBBS6.BBSProdCategory = "CTS MESH";
                                lBBS6.BBSStrucElem = "Wall";
                                lBBS6.BBSDesc = "";
                                lBBS6.BBSDrawing = "N";
                                lBBS6.BBSDrawingRev = "";
                                lBBS6.BBSTotalPcs = 0;
                                lBBS6.BBSTotalWT = 0;
                                lBBS6.BBSNDSGroupMark = "";
                                lBBS6.BBSSOR = "";
                                lBBS6.BBSOrder = false;

                                var oldBBS6 = db.CTSMESHBBS.Find(pCustomerCode, pProjectCode, lJobAdv.JobID, 6);
                                if (oldBBS6 == null)
                                {
                                    db.CTSMESHBBS.Add(lBBS6);
                                }

                                var lBBS7 = new CTSMESHBBSModels();
                                lBBS7.UpdateDate = DateTime.Now;
                                lBBS7.UpdateBy = User.Identity.GetUserName();
                                lBBS7.CustomerCode = pCustomerCode;
                                lBBS7.ProjectCode = pProjectCode;
                                lBBS7.JobID = lJobAdv.JobID;

                                lBBS7.BBSID = 7;
                                lBBS7.BBSProdCategory = "CTS MESH";
                                lBBS7.BBSStrucElem = "Drain";
                                lBBS7.BBSDesc = "";
                                lBBS7.BBSDrawing = "N";
                                lBBS7.BBSDrawingRev = "";
                                lBBS7.BBSTotalPcs = 0;
                                lBBS7.BBSTotalWT = 0;
                                lBBS7.BBSNDSGroupMark = "";
                                lBBS7.BBSSOR = "";
                                lBBS7.BBSOrder = false;

                                var oldBBS7 = db.CTSMESHBBS.Find(pCustomerCode, pProjectCode, lJobAdv.JobID, 7);
                                if (oldBBS7 == null)
                                {
                                    db.CTSMESHBBS.Add(lBBS7);
                                }

                                var lBBS8 = new CTSMESHBBSModels();
                                lBBS8.UpdateDate = DateTime.Now;
                                lBBS8.UpdateBy = User.Identity.GetUserName();
                                lBBS8.CustomerCode = pCustomerCode;
                                lBBS8.ProjectCode = pProjectCode;
                                lBBS8.JobID = lJobAdv.JobID;

                                //lBBS8.BBSID = 8;
                                //lBBS8.BBSProdCategory = "Standard Sheet";
                                //lBBS8.BBSStrucElem = "";
                                //lBBS8.BBSDesc = "";
                                //lBBS8.BBSDrawing = "N";
                                //lBBS8.BBSDrawingRev = "";
                                //lBBS8.BBSTotalPcs = 0;
                                //lBBS8.BBSTotalWT = 0;
                                //lBBS8.BBSNDSGroupMark = "";
                                //lBBS8.BBSSOR = "";
                                //lBBS8.BBSOrder = false;

                                //var oldBBS8 = db.CTSMESHBBS.Find(pCustomerCode, pProjectCode, lJobAdv.JobID, 8);
                                //if (oldBBS8 == null)
                                //{
                                //    db.CTSMESHBBS.Add(lBBS8);
                                //}
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
        [Route("/getOrderListSearch_beam/{CustomerCode}/{pProjectCode}/{RequiredDateFrom}/{RequiredDateTo}/{PONo}/{BBSNo}/{Block}/{Storey}/{Part}")]
        //[ValidateAntiForgeryHeader]
        public ActionResult getOrderListSearch(string CustomerCode, string ProjectCode, string RequiredDateFrom, string RequiredDateTo, string PONo, string BBSNo, string Block, string Storey, string Part)
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
            if (BBSNo == null) BBSNo = "";
            if (Block == null) Block = "";
            if (Storey == null) Storey = "";
            if (Part == null) Part = "";
            PONo = PONo.Trim();
            BBSNo = BBSNo.Trim();
            Block = Block.Trim();
            Storey = Storey.Trim();
            Part = Part.Trim();
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

            if (BBSNo == null || BBSNo == "" || BBSNo.Trim().Length == 0)
            {
                lCmd.CommandText = " SELECT p.JobID, " +
                "isNull(p.PONumber,''), " +
                "p.RequiredDate, " +
                "isNull(p.OrderStatus,''), " +
                "isNull((SELECT Max(s.FileName) " +
                "FROM dbo.OESCTSMESHPODoc s " +
                "WHERE s.CustomerCode = p.CustomerCode " +
                "AND s.ProjectCode = p.ProjectCode " +
                "AND s.JobID = p.JobID),'') " +
                "FROM dbo.OESCTSMESHJobAdvice p " +
                "WHERE p.CustomerCode = '" + CustomerCode + "' " +
                "AND p.ProjectCode = '" + ProjectCode + "' " +
                "AND p.RequiredDate >= '" + RequiredDateFrom + "' " +
                "AND p.RequiredDate <= '" + RequiredDateTo + "' " +
                "AND (p.PONumber like '%" + PONo + "%' " +
                "OR '" + PONo + "' = '') " +
                "AND (p.WBS1 = '" + Block + "' " +
                "OR '" + Block + "' = '' ) " +
                "AND (p.WBS2 = '" + Storey + "' " +
                "OR '" + Storey + "' = '') " +
                "AND (p.WBS3 = '" + Part + "' " +
                "OR '" + Part + "' = '') " +
                "ORDER BY p.JobID DESC ";
            }
            else
            {
                lCmd.CommandText = " SELECT p.JobID, " +
                "isNull(p.PONumber,''), " +
                "p.RequiredDate, " +
                "isNull(p.OrderStatus,''), " +
                "isNull((SELECT Max(s.FileName) " +
                "FROM dbo.OESCTSMESHPODoc s " +
                "WHERE s.CustomerCode = p.CustomerCode " +
                "AND s.ProjectCode = p.ProjectCode " +
                "AND s.JobID = p.JobID),'') " +
                "FROM dbo.OESCTSMESHJobAdvice p " +
                "WHERE p.CustomerCode = '" + CustomerCode + "' " +
                "AND p.ProjectCode = '" + ProjectCode + "' " +
                "AND p.RequiredDate >= '" + RequiredDateFrom + "' " +
                "AND p.RequiredDate <= '" + RequiredDateTo + "' " +
                "AND EXISTS (SELECT b.JobID " +
                "FROM dbo.OESCTSMESHBBS b " +
                "WHERE b.CustomerCode = p.CustomerCode " +
                "AND b.ProjectCode = p.ProjectCode " +
                "AND b.JobID = p.JobID " +
                "AND b.BBSNo like '%" + BBSNo + "%') " +
                "AND (p.PONumber like '%" + PONo + "%' " +
                "OR '" + PONo + "' = '') " +
                "AND (p.WBS1 = '" + Block + "' " +
                "OR '" + Block + "' = '' ) " +
                "AND (p.WBS2 = '" + Storey + "' " +
                "OR '" + Storey + "' = '') " +
                "AND (p.WBS3 = '" + Part + "' " +
                "OR '" + Part + "' = '') " +
                "ORDER BY p.JobID DESC ";
            }

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
            //return Json(lReturn, JsonRequestBehavior.AllowGet);
            return Ok(lReturn);

        }

        //get Job Advice details
        [HttpPost]
        [Route("/getOrderDetails_beam/{CustomerCode}/{ProjectCode}/{JobID}")]
        //[ValidateAntiForgeryHeader]
        public ActionResult getOrderDetails(string CustomerCode, string ProjectCode, int JobID)
        {
            int lPODocExists = 0;

            string lSubmission = "No";
            string lEditable = "No";

            var lJob = db.CTSMESHJobAdvice.Find(CustomerCode, ProjectCode, JobID);
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
                if (lSubmission.Trim() == "Yes" || lEditable.Trim() == "Yes")
                {
                    checkUpdateBBSWT(CustomerCode, ProjectCode, JobID);
                }
            }

            //var content1 = db.CTSMESHPODoc.Find(CustomerCode, ProjectCode, JobID, 1);
            //if (content1 != null)
            //{
            //    if (content1.FileName != null)
            //    {
            //        if(content1.FileName.Trim().Length > 0)
            //        {
            //            lPODocExists = 1;
            //        }
            //    }
            //}

            lPODocExists = db.CTSMESHPODoc.Where(
                                  p => p.CustomerCode == CustomerCode &&
                                  p.ProjectCode == ProjectCode &&
                                  p.JobID == JobID).Count();

            var content = db.CTSMESHJobAdvice.Find(CustomerCode, ProjectCode, JobID);
            if (content != null)
            {
                if (content.CustomerCode == null) content.CustomerCode = "";
                else content.CustomerCode = content.CustomerCode.Trim();

                if (content.OrderStatus == null) content.OrderStatus = "";
                else content.OrderStatus = content.OrderStatus.Trim();

                if (content.PONumber == null) content.PONumber = "";
                else content.PONumber = content.PONumber.Trim();

                if (content.ProjectCode == null) content.ProjectCode = "";
                else content.ProjectCode = content.ProjectCode.Trim();

                if (content.Remarks == null) content.Remarks = "";
                else content.Remarks = content.Remarks.Trim();

                if (content.DeliveryAddress == null) content.DeliveryAddress = "";
                else content.DeliveryAddress = content.DeliveryAddress.Trim();

                if (content.Scheduler_HP == null) content.Scheduler_HP = "";
                else content.Scheduler_HP = content.Scheduler_HP.Trim();

                if (content.Scheduler_Name == null) content.Scheduler_Name = "";
                else content.Scheduler_Name = content.Scheduler_Name.Trim();

                if (content.Scheduler_Tel == null) content.Scheduler_Tel = "";
                else content.Scheduler_Tel = content.Scheduler_Tel.Trim();

                if (content.SiteEngr_HP == null) content.SiteEngr_HP = "";
                else content.SiteEngr_HP = content.SiteEngr_HP.Trim();

                if (content.SiteEngr_Name == null) content.SiteEngr_Name = "";
                else content.SiteEngr_Name = content.SiteEngr_Name.Trim();

                if (content.SiteEngr_Tel == null) content.SiteEngr_Tel = "";
                else content.SiteEngr_Tel = content.SiteEngr_Tel.Trim();

                if (content.Transport == null) content.Transport = "";
                else content.Transport = content.Transport.Trim();

                if (content.WBS1 == null) content.WBS1 = "";
                else content.WBS1 = content.WBS1.Trim();

                if (content.WBS2 == null) content.WBS2 = "";
                else content.WBS2 = content.WBS2.Trim();

                if (content.WBS3 == null) content.WBS3 = "";
                else content.WBS3 = content.WBS3.Trim();
            }
            var lReturn = new
            {
                CustomerCode = content.CustomerCode.Trim(),
                ProjectCode = content.ProjectCode.Trim(),
                JobID = content.JobID,
                PONumber = content.PONumber.Trim(),
                PODate = content.PODate,
                RequiredDate = content.RequiredDate,
                OrderStatus = content.OrderStatus.Trim(),
                TotalPcs = content.TotalPcs,
                TotalWeight = content.TotalWeight,
                Transport = content.Transport.Trim(),
                Scheduler_HP = content.Scheduler_HP.Trim(),
                Scheduler_Name = content.Scheduler_Name.Trim(),
                Scheduler_Tel = content.Scheduler_Tel.Trim(),
                SiteEngr_HP = content.SiteEngr_HP.Trim(),
                SiteEngr_Name = content.SiteEngr_Name.Trim(),
                SiteEngr_Tel = content.SiteEngr_Tel.Trim(),
                AutoWBS = content.AutoWBS,
                WBS1 = content.WBS1.Trim(),
                WBS2 = content.WBS2.Trim(),
                WBS3 = content.WBS3.Trim(),
                DeliveryAddress = content.DeliveryAddress.Trim(),
                Remarks = content.Remarks.Trim(),
                Model = content.Model,
                UpdateDate = content.UpdateDate,
                Exists = lPODocExists
            };
            content = null;
            //   return Json(lReturn, JsonRequestBehavior.AllowGet);
            return Ok(lReturn);
        }



        //[HttpPost]
        //[Route("/CreateNewShape_beam")]
        //// [ValidateAntiForgeryHeader]
        //public ActionResult CreateNewShape()
        //{
        //    try
        //    {
        //        var lCustomerCode = Request.Form.Get("CustomerCode");
        //        var lProjectCode = Request.Form.Get("ProjectCode");
        //        int lJobID = 0;
        //        int.TryParse(Request.Form.Get("JobID"), out lJobID);
        //        var lShapeCode = Request.Form.Get("shapeCode");
        //        var lShapeCategory = Request.Form.Get("shapeCategory");
        //        var lShapeParameters = Request.Form.Get("shapeParameters");
        //        var lShapeLengthFormula = Request.Form.Get("shapeLengthFormula");

        //        if (Request.Files.Count > 0)
        //        {
        //            HttpFileCollectionBase files = Request.Files;
        //            for (int i = 0; i < files.Count; i++)
        //            {
        //                HttpPostedFileBase file = files[i];

        //                byte[] imageBytes = null;
        //                BinaryReader reader = new BinaryReader(file.InputStream);
        //                imageBytes = reader.ReadBytes((int)file.ContentLength);

        //                if (GetImageFormat(imageBytes) == false)
        //                {
        //                    ModelState.AddModelError(string.Empty, "Invalid image file.");
        //                    return Json(new { success = false, responseText = "Error: Invalid image file (无效的图象文件)" }, JsonRequestBehavior.AllowGet);
        //                }

        //                var Content1 = db.CustomerShape.Find(lCustomerCode, lProjectCode, lShapeCode);
        //                if (Content1 != null)
        //                {
        //                    if (Content1.ShapeStatus == "Inactive")
        //                    {
        //                        return Json(new { success = false, responseText = "Error: Shape Code " + lShapeCode + " already be used and disabled as it is replaced by normal shape " + Content1.ReplacedBy + ". (图形码" + lShapeCode + "已使用并且被禁止因为它已被一般图形码" + Content1.ReplacedBy + "取代)" }, JsonRequestBehavior.AllowGet);
        //                    }
        //                    else
        //                    {
        //                        return Json(new { success = false, responseText = "Error: Shape Code " + lShapeCode + " already be used. (图形码" + lShapeCode + "已使用)" }, JsonRequestBehavior.AllowGet);
        //                    }
        //                }
        //                var Content = new CustomerShapeModels
        //                {
        //                    CustomerCode = lCustomerCode,
        //                    ProjectCode = lProjectCode,
        //                    shapeCode = lShapeCode,
        //                    shapeCategory = lShapeCategory,
        //                    shapeParameters = lShapeParameters,
        //                    shapeLengthFormula = lShapeLengthFormula,
        //                    shapeImage = imageBytes,
        //                    shapeCreated = DateTime.Now,
        //                    CreatedBy = User.Identity.Name,
        //                    ShapeStatus = "Active"
        //                };
        //                db.CustomerShape.Add(Content);
        //                db.SaveChanges();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError(string.Empty, "Error: " + ex.Message);
        //        return Json(new { success = false, responseText = "Error:" + ex.Message }, JsonRequestBehavior.AllowGet);
        //    }
        //    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        //}

        [HttpGet]
        [Route("/GetImageFormat_beam/{bytes}")]

        public bool GetImageFormat(byte[] bytes)
        {
            // see http://www.mikekunz.com/image_file_header.html  
            var bmp = Encoding.ASCII.GetBytes("BM");     // BMP
            var gif = Encoding.ASCII.GetBytes("GIF");    // GIF
            var png = new byte[] { 137, 80, 78, 71 };    // PNG
            var tiff = new byte[] { 73, 73, 42 };         // TIFF
            var tiff2 = new byte[] { 77, 77, 42 };         // TIFF
            var jpeg = new byte[] { 255, 216, 255, 224 }; // jpeg
            var jpeg2 = new byte[] { 255, 216, 255, 225 }; // jpeg canon

            if (bmp.SequenceEqual(bytes.Take(bmp.Length)))
                //return ImageFormat.bm;
                return false;

            if (gif.SequenceEqual(bytes.Take(gif.Length)))
                //return ImageFormat.gif;
                return true;

            if (png.SequenceEqual(bytes.Take(png.Length)))
                //return ImageFormat.png;
                return true;

            if (tiff.SequenceEqual(bytes.Take(tiff.Length)))
                //return ImageFormat.tiff;
                return true;

            if (tiff2.SequenceEqual(bytes.Take(tiff2.Length)))
                //return ImageFormat.tiff;
                return true;

            if (jpeg.SequenceEqual(bytes.Take(jpeg.Length)))
                //return ImageFormat.jpeg;
                return true;

            if (jpeg2.SequenceEqual(bytes.Take(jpeg2.Length)))
                //return ImageFormat.jpeg;
                return true;

            return false;
            //            return ImageFormat.unknown;
        }

        [HttpPost]
        [Route("/setCloneOrder_beam/{CustomerCode}/{ProjectCode}/{JobID}/{CloneNo}")]
        //[ValidateAntiForgeryHeader]
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

                        lJobID = db.CTSMESHJobAdvice.Where(z => z.CustomerCode == CustomerCode &&
                        z.ProjectCode == ProjectCode).Max(z => (int?)z.JobID);

                        var lJobNewOr = db.CTSMESHJobAdvice.Find(CustomerCode, ProjectCode, lJobID);
                        var lJobNew = lJobNewOr;
                        var lJobOld = db.CTSMESHJobAdvice.Find(CustomerCode, ProjectCode, JobID);
                        if (lJobOld != null && lJobNew != null)
                        {
                            lJobNew.OrderStatus = "New";
                            //lJobNew.PODate = lJobOld.PODate;
                            lJobNew.PODate = DateTime.Now;
                            lJobNew.PONumber = lJobOld.PONumber;
                            lJobNew.Remarks = lJobOld.Remarks;
                            lJobNew.DeliveryAddress = lJobOld.DeliveryAddress;
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

                        var lOld = (from p in db.CTSMESHStdSheetDetails
                                    where p.CustomerCode == CustomerCode &&
                                    p.ProjectCode == ProjectCode &&
                                    p.JobID == lJobID
                                    select p).ToList();
                        if (lOld != null && lOld.Count > 0)
                        {
                            db.CTSMESHStdSheetDetails.RemoveRange(lOld);
                        }

                        var lOldDet = (from p in db.CTSMESHStdSheetDetails
                                       where p.CustomerCode == CustomerCode &&
                                       p.ProjectCode == ProjectCode &&
                                       p.JobID == JobID
                                       select p).ToList();
                        if (lOldDet != null && lOldDet.Count > 0)
                        {
                            var lNewDet = new List<CTSMESHStdSheetDetailsModels>();
                            for (int j = 0; j < lOldDet.Count; j++)
                            {

                                lNewDet.Add(new CTSMESHStdSheetDetailsModels
                                {
                                    CustomerCode = lOldDet[j].CustomerCode,
                                    ProjectCode = lOldDet[j].ProjectCode,
                                    JobID = (int)lJobID,
                                    MeshID = lOldDet[j].MeshID,
                                    MeshSort = lOldDet[j].MeshSort,
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
                            db.CTSMESHStdSheetDetails.AddRange(lNewDet);
                        }

                        //Clone for Beam Products
                        var lOldBeam = (from p in db.CTSMESHBeamDetails
                                        where p.CustomerCode == CustomerCode &&
                                        p.ProjectCode == ProjectCode &&
                                        p.JobID == lJobID
                                        select p).ToList();
                        if (lOldBeam != null && lOldBeam.Count > 0)
                        {
                            db.CTSMESHBeamDetails.RemoveRange(lOldBeam);
                        }

                        var lOldDetBeam = (from p in db.CTSMESHBeamDetails
                                           where p.CustomerCode == CustomerCode &&
                                       p.ProjectCode == ProjectCode &&
                                       p.JobID == JobID
                                           select p).ToList();
                        if (lOldDetBeam != null && lOldDetBeam.Count > 0)
                        {
                            var lNewDetOther = new List<CTSMESHBeamDetailsModels>();
                            for (int j = 0; j < lOldDetBeam.Count; j++)
                            {
                                lNewDetOther.Add(new CTSMESHBeamDetailsModels
                                {
                                    CustomerCode = lOldDetBeam[j].CustomerCode,
                                    ProjectCode = lOldDetBeam[j].ProjectCode,
                                    JobID = (int)lJobID,
                                    BBSID = lOldDetBeam[j].BBSID,
                                    MeshID = lOldDetBeam[j].MeshID,
                                    MeshSort = lOldDetBeam[j].MeshSort,
                                    MeshMark = lOldDetBeam[j].MeshMark,
                                    MeshWidth = lOldDetBeam[j].MeshWidth,
                                    MeshDepth = lOldDetBeam[j].MeshDepth,
                                    MeshSlope = lOldDetBeam[j].MeshSlope,
                                    MeshProduct = lOldDetBeam[j].MeshProduct,
                                    MeshShapeCode = lOldDetBeam[j].MeshShapeCode,
                                    MeshTotalLinks = lOldDetBeam[j].MeshTotalLinks,
                                    MeshSpan = lOldDetBeam[j].MeshSpan,
                                    MeshMemberQty = lOldDetBeam[j].MeshMemberQty,
                                    MeshCapping = lOldDetBeam[j].MeshCapping,
                                    MeshCPProduct = lOldDetBeam[j].MeshCPProduct,
                                    A = lOldDetBeam[j].A,
                                    B = lOldDetBeam[j].B,
                                    C = lOldDetBeam[j].C,
                                    D = lOldDetBeam[j].D,
                                    E = lOldDetBeam[j].E,
                                    P = lOldDetBeam[j].P,
                                    Q = lOldDetBeam[j].Q,
                                    HOOK = lOldDetBeam[j].HOOK,
                                    LEG = lOldDetBeam[j].LEG,
                                    MeshTotalWT = lOldDetBeam[j].MeshTotalWT,
                                    Remarks = lOldDetBeam[j].Remarks,
                                    UpdateBy = User.Identity.GetUserName(),
                                    UpdateDate = DateTime.Now
                                });
                            }
                            db.CTSMESHBeamDetails.AddRange(lNewDetOther);
                        }

                        //Column Products
                        var lOldColumn = (from p in db.CTSMESHColumnDetails
                                          where p.CustomerCode == CustomerCode &&
                                          p.ProjectCode == ProjectCode &&
                                          p.JobID == lJobID
                                          select p).ToList();
                        if (lOldColumn != null && lOldColumn.Count > 0)
                        {
                            db.CTSMESHColumnDetails.RemoveRange(lOldColumn);
                        }

                        var lOldDetColumn = (from p in db.CTSMESHColumnDetails
                                             where p.CustomerCode == CustomerCode &&
                                         p.ProjectCode == ProjectCode &&
                                         p.JobID == JobID
                                             select p).ToList();
                        if (lOldDetColumn != null && lOldDetColumn.Count > 0)
                        {
                            var lNewDetColumn = new List<CTSMESHColumnDetailsModels>();
                            for (int j = 0; j < lOldDetColumn.Count; j++)
                            {
                                lNewDetColumn.Add(new CTSMESHColumnDetailsModels
                                {
                                    CustomerCode = lOldDetColumn[j].CustomerCode,
                                    ProjectCode = lOldDetColumn[j].ProjectCode,
                                    JobID = (int)lJobID,
                                    BBSID = lOldDetColumn[j].BBSID,
                                    MeshID = lOldDetColumn[j].MeshID,
                                    MeshSort = lOldDetColumn[j].MeshSort,
                                    MeshMark = lOldDetColumn[j].MeshMark,
                                    MeshWidth = lOldDetColumn[j].MeshWidth,
                                    MeshLength = lOldDetColumn[j].MeshLength,
                                    MeshHeight = lOldDetColumn[j].MeshHeight,
                                    MeshProduct = lOldDetColumn[j].MeshProduct,
                                    MeshShapeCode = lOldDetColumn[j].MeshShapeCode,
                                    MeshTotalLinks = lOldDetColumn[j].MeshTotalLinks,
                                    MeshCLinkProductAtLen = lOldDetColumn[j].MeshCLinkProductAtLen,
                                    MeshCLinkProductAtWidth = lOldDetColumn[j].MeshCLinkProductAtWidth,
                                    MeshCLinkRowsAtWidth = lOldDetColumn[j].MeshCLinkRowsAtWidth,
                                    MeshCLinkRowsAtLen = lOldDetColumn[j].MeshCLinkRowsAtLen,
                                    MeshMemberQty = lOldDetColumn[j].MeshMemberQty,
                                    A = lOldDetColumn[j].A,
                                    B = lOldDetColumn[j].B,
                                    C = lOldDetColumn[j].C,
                                    D = lOldDetColumn[j].D,
                                    E = lOldDetColumn[j].E,
                                    F = lOldDetColumn[j].F,
                                    P = lOldDetColumn[j].P,
                                    Q = lOldDetColumn[j].Q,
                                    LEG = lOldDetColumn[j].LEG,
                                    MeshTotalWT = lOldDetColumn[j].MeshTotalWT,
                                    Remarks = lOldDetColumn[j].Remarks,
                                    UpdateBy = User.Identity.GetUserName(),
                                    UpdateDate = DateTime.Now
                                });
                            }
                            db.CTSMESHColumnDetails.AddRange(lNewDetColumn);
                        }

                        //Other Products
                        var lOldOthers = (from p in db.CTSMESHOthersDetails
                                          where p.CustomerCode == CustomerCode &&
                                          p.ProjectCode == ProjectCode &&
                                          p.JobID == lJobID
                                          select p).ToList();
                        if (lOldOthers != null && lOldOthers.Count > 0)
                        {
                            db.CTSMESHOthersDetails.RemoveRange(lOldOthers);
                        }

                        var lOldDetOthers = (from p in db.CTSMESHOthersDetails
                                             where p.CustomerCode == CustomerCode &&
                                         p.ProjectCode == ProjectCode &&
                                         p.JobID == JobID
                                             select p).ToList();
                        if (lOldDetOthers != null && lOldDetOthers.Count > 0)
                        {
                            var lNewDetOthers = new List<CTSMESHOthersDetailsModels>();
                            for (int j = 0; j < lOldDetOthers.Count; j++)
                            {
                                lNewDetOthers.Add(new CTSMESHOthersDetailsModels
                                {
                                    CustomerCode = lOldDetOthers[j].CustomerCode,
                                    ProjectCode = lOldDetOthers[j].ProjectCode,
                                    JobID = (int)lJobID,
                                    BBSID = lOldDetOthers[j].BBSID,
                                    MeshID = lOldDetOthers[j].MeshID,
                                    MeshSort = lOldDetOthers[j].MeshSort,
                                    MeshMark = lOldDetOthers[j].MeshMark,
                                    MeshProduct = lOldDetOthers[j].MeshProduct,
                                    MeshMainLen = lOldDetOthers[j].MeshMainLen,
                                    MeshCrossLen = lOldDetOthers[j].MeshCrossLen,
                                    MeshMO1 = lOldDetOthers[j].MeshMO1,
                                    MeshMO2 = lOldDetOthers[j].MeshMO2,
                                    MeshCO1 = lOldDetOthers[j].MeshCO1,
                                    MeshCO2 = lOldDetOthers[j].MeshCO2,
                                    MeshShapeCode = lOldDetOthers[j].MeshShapeCode,
                                    MeshMemberQty = lOldDetOthers[j].MeshMemberQty,
                                    A = lOldDetOthers[j].A,
                                    B = lOldDetOthers[j].B,
                                    C = lOldDetOthers[j].C,
                                    D = lOldDetOthers[j].D,
                                    E = lOldDetOthers[j].E,
                                    F = lOldDetOthers[j].F,
                                    G = lOldDetOthers[j].G,
                                    H = lOldDetOthers[j].H,
                                    I = lOldDetOthers[j].I,
                                    J = lOldDetOthers[j].J,
                                    K = lOldDetOthers[j].K,
                                    L = lOldDetOthers[j].L,
                                    M = lOldDetOthers[j].M,
                                    N = lOldDetOthers[j].N,
                                    O = lOldDetOthers[j].O,
                                    P = lOldDetOthers[j].P,
                                    Q = lOldDetOthers[j].Q,
                                    R = lOldDetOthers[j].R,
                                    S = lOldDetOthers[j].S,
                                    T = lOldDetOthers[j].T,
                                    U = lOldDetOthers[j].U,
                                    V = lOldDetOthers[j].V,
                                    W = lOldDetOthers[j].W,
                                    X = lOldDetOthers[j].X,
                                    Y = lOldDetOthers[j].Y,
                                    Z = lOldDetOthers[j].Z,
                                    HOOK = lOldDetOthers[j].HOOK,
                                    MeshTotalWT = lOldDetOthers[j].MeshTotalWT,
                                    Remarks = lOldDetOthers[j].Remarks,
                                    UpdateBy = User.Identity.GetUserName(),
                                    UpdateDate = DateTime.Now
                                });
                            }
                            db.CTSMESHOthersDetails.AddRange(lNewDetOthers);
                        }

                        //bbs
                        var lOldBBSDel = (from p in db.CTSMESHBBS
                                          where p.CustomerCode == CustomerCode &&
                                          p.ProjectCode == ProjectCode &&
                                          p.JobID == lJobID
                                          select p).ToList();
                        if (lOldBBSDel != null && lOldBBSDel.Count > 0)
                        {
                            db.CTSMESHBBS.RemoveRange(lOldBBSDel);
                        }

                        var lOldBBS = (from p in db.CTSMESHBBS
                                       where p.CustomerCode == CustomerCode &&
                                   p.ProjectCode == ProjectCode &&
                                   p.JobID == JobID
                                       select p).ToList();
                        if (lOldBBS != null && lOldBBS.Count > 0)
                        {
                            var lNewBBS = new List<CTSMESHBBSModels>();
                            for (int j = 0; j < lOldBBS.Count; j++)
                            {
                                lNewBBS.Add(new CTSMESHBBSModels
                                {
                                    CustomerCode = lOldBBS[j].CustomerCode,
                                    ProjectCode = lOldBBS[j].ProjectCode,
                                    JobID = (int)lJobID,
                                    BBSID = lOldBBS[j].BBSID,
                                    BBSOrder = lOldBBS[j].BBSOrder,
                                    BBSProdCategory = lOldBBS[j].BBSProdCategory,
                                    BBSStrucElem = lOldBBS[j].BBSStrucElem,
                                    BBSDesc = lOldBBS[j].BBSDesc,
                                    BBSTotalPcs = lOldBBS[j].BBSTotalPcs,
                                    BBSTotalWT = lOldBBS[j].BBSTotalWT,
                                    BBSDrawing = lOldBBS[j].BBSDrawing,
                                    BBSDrawingRev = lOldBBS[j].BBSDrawingRev,
                                    UpdateBy = User.Identity.GetUserName(),
                                    UpdateDate = DateTime.Now
                                });
                            }
                            db.CTSMESHBBS.AddRange(lNewBBS);
                        }

                        //NSH bbs
                        var lOldBBSNSH = (from p in db.CTSMESHBBSNSH
                                          where p.CustomerCode == CustomerCode &&
                                      p.ProjectCode == ProjectCode &&
                                      p.JobID == JobID
                                          select p).ToList();
                        if (lOldBBSNSH != null && lOldBBSNSH.Count > 0)
                        {
                            var lNewBBSNSH = new List<CTSMESHBBSNSHModels>();
                            for (int j = 0; j < lOldBBSNSH.Count; j++)
                            {
                                lNewBBSNSH.Add(new CTSMESHBBSNSHModels
                                {
                                    CustomerCode = lOldBBSNSH[j].CustomerCode,
                                    ProjectCode = lOldBBSNSH[j].ProjectCode,
                                    JobID = (int)lJobID,
                                    BBSID = lOldBBSNSH[j].BBSID,
                                    BBSOrder = lOldBBSNSH[j].BBSOrder,
                                    BBSProdCategory = lOldBBSNSH[j].BBSProdCategory,
                                    BBSStrucElem = lOldBBSNSH[j].BBSStrucElem,
                                    BBSDesc = lOldBBSNSH[j].BBSDesc,
                                    BBSTotalPcs = lOldBBSNSH[j].BBSTotalPcs,
                                    BBSTotalWT = lOldBBSNSH[j].BBSTotalWT,
                                    BBSDrawing = lOldBBSNSH[j].BBSDrawing,
                                    BBSDrawingRev = lOldBBSNSH[j].BBSDrawingRev,
                                    UpdateBy = User.Identity.GetUserName(),
                                    UpdateDate = DateTime.Now
                                });
                            }
                            db.CTSMESHBBSNSH.AddRange(lNewBBSNSH);
                        }


                        db.SaveChanges();
                    }
                }
                //   return Json(new { success = true, responseText = "Successfully saved." }, JsonRequestBehavior.AllowGet);
                return Ok();
            }
            catch (Exception ex)
            {
                lErrorMsg = ex.Message;
            }
            // return Json(new { success = false, responseText = lErrorMsg }, JsonRequestBehavior.AllowGet);
            return Ok();
        }

        [HttpPost]
        [Route("/UpdateSUM_beam/{CustomerCode}/{ProjectCode}/{JobID}")]
        //[ValidateAntiForgeryHeader]
        public async Task<ActionResult> UpdateSUM(string CustomerCode, string ProjectCode, int JobID)
        {
            var lErrorMsg = "";
            int? lJobID = 0;
            try
            {
                var lTotalWT = await (from p in db.CTSMESHBeamDetails
                                      where p.CustomerCode == CustomerCode &&
                                      p.ProjectCode == ProjectCode &&
                                      p.JobID == JobID &&
                                      p.BBSID == 1
                                      select p.MeshTotalWT).SumAsync();

                var lTotalPCs = await (from p in db.CTSMESHBeamDetails
                                       where p.CustomerCode == CustomerCode &&
                                       p.ProjectCode == ProjectCode &&
                                       p.JobID == JobID &&
                                       p.BBSID == 1
                                       select p.MeshMemberQty).SumAsync();

                var oldJobAdvice = await db.CTSMESHJobAdvice.FindAsync(CustomerCode, ProjectCode, JobID);
                var lNewJobAdv = oldJobAdvice;

                lNewJobAdv.TotalWeight = (decimal)(lTotalWT == null ? 0 : lTotalWT);
                lNewJobAdv.TotalPcs = (int)(lTotalPCs == null ? 0 : lTotalPCs);

                db.Entry(oldJobAdvice).CurrentValues.SetValues(lNewJobAdv);

                var oldBBS = await db.CTSMESHBBS.FindAsync(CustomerCode, ProjectCode, JobID, 1);
                var lNewBBS = oldBBS;

                lNewBBS.BBSTotalWT = (decimal)(lTotalWT == null ? 0 : lTotalWT);
                lNewBBS.BBSTotalPcs = (int)(lTotalPCs == null ? 0 : lTotalPCs);

                db.Entry(oldBBS).CurrentValues.SetValues(lNewBBS);

                //update Master
                var lProjSE = await (from p in db.OrderProjectSE
                                     join m in db.OrderProject
                                     on p.OrderNumber equals m.OrderNumber
                                     where p.MESHJobID == JobID &&
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

                        lNewOrder.TotalWeight = (decimal)(lTotalWT == null ? 0 : lTotalWT);

                        db.Entry(lOldOrder).CurrentValues.SetValues(lNewOrder);
                    }

                    var lOldSE = await db.OrderProjectSE.FindAsync(lOrderNo, lStrEle, lProd, lScheduled);
                    if (lOldSE != null)
                    {
                        var lNewSE = lOldSE;

                        lNewSE.TotalWeight = (decimal)(lTotalWT == null ? 0 : lTotalWT);
                        lNewSE.TotalPCs = (lTotalPCs == null ? 0 : lTotalPCs);

                        db.Entry(lOldSE).CurrentValues.SetValues(lNewSE);
                    }

                }
                //Update Component
                var lComponent = await (from p in db.OESComponent
                                        where p.MeshJobID == JobID &&
                                         p.CustomerCode == CustomerCode &&
                                         p.ProjectCode == ProjectCode
                                        select p).ToListAsync();
                if (lComponent != null && lComponent.Count > 0)
                {
                    int lCompID = lComponent[0].ComponentID;
                    if (lCompID > 0)
                    {
                        var lOldCOM = await db.OESComponent.FindAsync(lCompID);
                        if (lOldCOM != null)
                        {
                            var lNewCOM = lOldCOM;

                            lNewCOM.TotalWeight = (decimal)(lTotalWT == null ? 0 : lTotalWT);
                            lNewCOM.TotalPCs = lTotalPCs == null ? 0 : lTotalPCs;

                            db.Entry(lOldCOM).CurrentValues.SetValues(lNewCOM);
                        }

                    }
                }

                int lRnt = await db.SaveChangesAsync();


                return Json(lNewJobAdv.JobID);

            }
            catch (Exception ex)
            {
                lErrorMsg = ex.Message;
                return Json(false);
            }
        }

        [HttpPost]
        [Route("/OrderWithdraw_beam/{CustomerCode}/{ProjectCode}/{JobID}")]
        //   [ValidateAntiForgeryHeader]
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
                    var oldJobAdvice = db.CTSMESHJobAdvice.Find(CustomerCode, ProjectCode, JobID);
                    if (oldJobAdvice != null)
                    {
                        if (oldJobAdvice.OrderStatus.Trim() == "Submitted")
                        {
                            var newJobAdvice = oldJobAdvice;
                            newJobAdvice.OrderStatus = "New";
                            newJobAdvice.UpdateDate = DateTime.Now;
                            newJobAdvice.UpdateBy = User.Identity.GetUserName();

                            db.Entry(oldJobAdvice).CurrentValues.SetValues(newJobAdvice);

                            var lBeamOld = (from p in db.CTSMESHBeamDetails
                                            where p.CustomerCode == CustomerCode &&
                                            p.ProjectCode == ProjectCode &&
                                            p.JobID == JobID &&
                                            p.UpdateBy != User.Identity.Name
                                            select p).ToList();

                            if (lBeamOld != null && lBeamOld.Count > 0)
                            {
                                for (int i = 0; i < lBeamOld.Count; i++)
                                {
                                    var lNewBeam = lBeamOld[i];
                                    lNewBeam.UpdateBy = User.Identity.GetUserName();

                                    db.Entry(lBeamOld[i]).CurrentValues.SetValues(lNewBeam);
                                }
                            }

                            var lColumnOld = (from p in db.CTSMESHColumnDetails
                                              where p.CustomerCode == CustomerCode &&
                                              p.ProjectCode == ProjectCode &&
                                              p.JobID == JobID &&
                                              p.UpdateBy != User.Identity.Name
                                              select p).ToList();

                            if (lColumnOld != null && lColumnOld.Count > 0)
                            {
                                for (int i = 0; i < lColumnOld.Count; i++)
                                {
                                    var lNewColumn = lColumnOld[i];
                                    lNewColumn.UpdateBy = User.Identity.GetUserName();

                                    db.Entry(lColumnOld[i]).CurrentValues.SetValues(lNewColumn);
                                }
                            }

                            var lSlabOld = (from p in db.CTSMESHOthersDetails
                                            where p.CustomerCode == CustomerCode &&
                                            p.ProjectCode == ProjectCode &&
                                            p.JobID == JobID &&
                                            p.UpdateBy != User.Identity.Name
                                            select p).ToList();

                            if (lSlabOld != null && lSlabOld.Count > 0)
                            {
                                for (int i = 0; i < lSlabOld.Count; i++)
                                {
                                    var lNewSlab = lSlabOld[i];
                                    lNewSlab.UpdateBy = User.Identity.GetUserName();

                                    db.Entry(lSlabOld[i]).CurrentValues.SetValues(lNewSlab);
                                }
                            }

                            var lBBSOld = (from p in db.CTSMESHBBS
                                           where p.CustomerCode == CustomerCode &&
                                           p.ProjectCode == ProjectCode &&
                                           p.JobID == JobID &&
                                           p.UpdateBy != User.Identity.Name
                                           select p).ToList();

                            if (lBBSOld != null && lBBSOld.Count > 0)
                            {
                                for (int i = 0; i < lBBSOld.Count; i++)
                                {
                                    var lNewBBS = lBBSOld[i];
                                    lNewBBS.UpdateBy = User.Identity.GetUserName();

                                    db.Entry(lBBSOld[i]).CurrentValues.SetValues(lNewBBS);
                                }
                            }

                            var lBBSOldNSH = (from p in db.CTSMESHBBSNSH
                                              where p.CustomerCode == CustomerCode &&
                                              p.ProjectCode == ProjectCode &&
                                              p.JobID == JobID &&
                                              p.UpdateBy != User.Identity.Name
                                              select p).ToList();

                            if (lBBSOldNSH != null && lBBSOldNSH.Count > 0)
                            {
                                for (int i = 0; i < lBBSOldNSH.Count; i++)
                                {
                                    var lNewBBSNSH = lBBSOldNSH[i];
                                    lNewBBSNSH.UpdateBy = User.Identity.GetUserName();

                                    db.Entry(lBBSOldNSH[i]).CurrentValues.SetValues(lNewBBSNSH);
                                }
                            }

                            db.SaveChanges();

                            var lEmailContent = "";
                            var lEmailFrom = "";
                            var lEmailTo = "";
                            var lEmailCc = "";
                            var lEmailSubject = "";
                            string lVar1 = "";

                            var JobContent = db.CTSMESHJobAdvice.Find(CustomerCode, ProjectCode, JobID);
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

                                if (JobContent.Remarks == null) JobContent.Remarks = "";
                                else JobContent.Remarks = JobContent.Remarks.Trim();

                                if (JobContent.DeliveryAddress == null) JobContent.DeliveryAddress = "";
                                else JobContent.DeliveryAddress = JobContent.DeliveryAddress.Trim();

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

                                if (JobContent.WBS1 == null) JobContent.WBS1 = "";
                                else JobContent.WBS1 = JobContent.WBS1.Trim();

                                if (JobContent.WBS2 == null) JobContent.WBS2 = "";
                                else JobContent.WBS2 = JobContent.WBS2.Trim();

                                if (JobContent.WBS3 == null) JobContent.WBS3 = "";
                                else JobContent.WBS3 = JobContent.WBS3.Trim();
                            }

                            lEmailContent = "<p align='center'>CANCEL JOB ADVICE - MESH (撤消工作通知 / 网片料表)</p>";

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

                            lEmailContent = lEmailContent + "<td width=20%>" + "PO No.\n(订单号码)" + "</td>";
                            lEmailContent = lEmailContent + "<td width=27%>" + JobContent.PONumber.Trim() + "</td>";
                            lEmailContent = lEmailContent + "<td width=26%>" + "Order Date\n(订单日期)" + "</td>";
                            lEmailContent = lEmailContent + "<td>" + String.Format("{0:yyyy-MM-dd}", JobContent.PODate) + "</td></tr>";

                            lEmailContent = lEmailContent + "<tr><td width=20%>" + "Required Date\n(交货日期)" + "</td>";
                            lEmailContent = lEmailContent + "<td width=27%>" + String.Format("{0:yyyy-MM-dd}", JobContent.RequiredDate) + "</td>";
                            lEmailContent = lEmailContent + "<td width=26%>" + "Order Weight\n(订单重量)" + "</td>";
                            if (JobContent.TotalWeight > 0)
                            {
                                lEmailContent = lEmailContent + "<td>" + ((decimal)JobContent.TotalWeight).ToString("F3") + " KG" + "</td></tr>";
                            }
                            else
                            {
                                lEmailContent = lEmailContent + "<td>" + " " + "</td></tr>";
                            }

                            lEmailContent = lEmailContent + "</tr><td width=20%>" + "Order Pieces\n(订单总数)" + "</td>";
                            if (JobContent.TotalPcs > 0)
                            {
                                lEmailContent = lEmailContent + "<td width=27%>" + JobContent.TotalPcs.ToString() + "</td>";
                            }
                            else
                            {
                                lEmailContent = lEmailContent + "<td width=27%> </td>";
                            }
                            lEmailContent = lEmailContent + "<td width=26%>" + "Transportation\n(运输工具)" + "</td>";
                            lEmailContent = lEmailContent + "<td>" + JobContent.Transport + "</td></tr></table>";

                            lEmailContent = lEmailContent + "<table border='1' width=100%><tr>";
                            lEmailContent = lEmailContent + "<td width='20%'>" + "Block (WBS1)\n(座号/大牌)" + "</td>";
                            lEmailContent = lEmailContent + "<td width=15%>" + JobContent.WBS1.Trim() + "</td>";
                            lEmailContent = lEmailContent + "<td width='17%'>" + "Storey (WBS2)\n(楼层)" + "</td>";
                            lEmailContent = lEmailContent + "<td width=14%>" + JobContent.WBS2.Trim() + "</td>";
                            lEmailContent = lEmailContent + "<td width=16%>" + "Part (WBS3)\n(分部)" + "</td>";
                            lEmailContent = lEmailContent + "<td>" + JobContent.WBS3.Trim() + "</td></tr></table>";

                            var lBBSContentNSH = (from p in db.CTSMESHBBSNSH
                                                  where p.CustomerCode == CustomerCode &&
                                                  p.ProjectCode == ProjectCode &&
                                                  p.JobID == JobID &&
                                                  p.BBSOrder == true
                                                  orderby p.BBSID
                                                  select p).ToList();

                            if (lBBSContentNSH.Count > 0)
                            {
                                lEmailContent = lEmailContent + "<table border='1' width=100%>";
                                lEmailContent = lEmailContent + "<tr><td colspan='6'> <font color='blue'>MESH Scheduled by NatSteel\n(大众钢铁安排的铁网)</font></td><tr>";
                                lEmailContent = lEmailContent + "<td width='20%'>" + "Product Category\n(产品类型)" + "</td>";
                                lEmailContent = lEmailContent + "<td width='17%'>" + "Structure Element\n(构件)" + "</td>";
                                lEmailContent = lEmailContent + "<td width='15%'>" + "Description\n(说明)" + "</td>";
                                lEmailContent = lEmailContent + "<td width='14%'>" + "Total Pieces\n总件数" + "</td>";
                                lEmailContent = lEmailContent + "<td width='16%'>" + "Total Weight\n总重量" + "</td>";
                                lEmailContent = lEmailContent + "<td>" + "Posted Date\n(上载日期)" + "</td></tr>";


                                for (int i = 0; i < lBBSContentNSH.Count; i++)
                                {
                                    lEmailContent = lEmailContent + "<tr><td> <font color='blue'>" + lBBSContentNSH[i].BBSProdCategory + "</font></td>";
                                    lEmailContent = lEmailContent + "<td>" + lBBSContentNSH[i].BBSStrucElem + "</td>";
                                    lEmailContent = lEmailContent + "<td>" + lBBSContentNSH[i].BBSDesc + "</td>";
                                    if (lBBSContentNSH[i].BBSTotalPcs == 0) lVar = ""; else lVar = lBBSContentNSH[i].BBSTotalPcs.ToString("F0");
                                    lEmailContent = lEmailContent + "<td align='left'>" + lVar + "</td>";
                                    if (lBBSContentNSH[i].BBSTotalWT == 0) lVar = ""; else lVar = lBBSContentNSH[i].BBSTotalWT.ToString("F3");
                                    lEmailContent = lEmailContent + "<td align='left'>" + lVar + "</td>";
                                    lEmailContent = lEmailContent + "<td align='left'>" + lBBSContentNSH[i].BBSNDSPostDate.ToString("yyyy-MM-dd") + "</td></tr>";
                                }
                                lEmailContent = lEmailContent + "</table>";
                            }

                            var lBBSContent = (from p in db.CTSMESHBBS
                                               where p.CustomerCode == CustomerCode &&
                                               p.ProjectCode == ProjectCode &&
                                               p.JobID == JobID &&
                                               p.BBSOrder == true
                                               orderby p.BBSID
                                               select p).ToList();

                            if (lBBSContent.Count > 0)
                            {
                                lEmailContent = lEmailContent + "<table border='1' width=100%>";
                                lEmailContent = lEmailContent + "<tr><td colspan='6'> <font color='blue'>MESH Scheduled by Customer Self\n(客户自己安排的铁网)</font></td><tr>";
                                lEmailContent = lEmailContent + "<td width='20%'>" + "Product Category\n(产品类型)" + "</td>";
                                lEmailContent = lEmailContent + "<td width='17%'>" + "Structure Element\n(构件)" + "</td>";
                                lEmailContent = lEmailContent + "<td width='15%'>" + "Description\n(说明)" + "</td>";
                                lEmailContent = lEmailContent + "<td width='14%'>" + "Total Pieces\n总件数" + "</td>";
                                lEmailContent = lEmailContent + "<td width='16%'>" + "Total Weight\n总重量" + "</td>";
                                lEmailContent = lEmailContent + "<td>" + "Updated Date\n更新日期" + "</td></tr>";

                                for (int i = 0; i < lBBSContent.Count; i++)
                                {
                                    lEmailContent = lEmailContent + "<tr><td> <font color='blue'>" + lBBSContent[i].BBSProdCategory + "</font></td>";
                                    lEmailContent = lEmailContent + "<td>" + lBBSContent[i].BBSStrucElem + "</td>";
                                    lEmailContent = lEmailContent + "<td>" + lBBSContent[i].BBSDesc + "</td>";
                                    if (lBBSContent[i].BBSTotalPcs == 0) lVar = ""; else lVar = lBBSContent[i].BBSTotalPcs.ToString("F0");
                                    lEmailContent = lEmailContent + "<td align='left'>" + lVar + "</td>";
                                    if (lBBSContent[i].BBSTotalWT == 0) lVar = ""; else lVar = lBBSContent[i].BBSTotalWT.ToString("F3");
                                    lEmailContent = lEmailContent + "<td align='left'>" + lVar + "</td>";
                                    lEmailContent = lEmailContent + "<td align='left'>" + ((DateTime)lBBSContent[i].UpdateDate).ToString("yyyy-MM-dd") + "</td></tr>";
                                }
                                lEmailContent = lEmailContent + "</table>";
                            }

                            if (lBBSContentNSH.Count == 0 && lBBSContent.Count == 0)
                            {
                                lEmailContent = lEmailContent + "<table border='1' width=100%><tr>";
                                lEmailContent = lEmailContent + "<tr><td> <font color='blue'>Order without details.\n(此订单无订单明细)</font></td></tr>";
                                lEmailContent = lEmailContent + "</table>";
                            }

                            lEmailContent = lEmailContent + "<table border='1' width=100%>";
                            lEmailContent = lEmailContent + "<tr><td width='20%'>" + "Delivery Address\n(送货地址)" + "</td>";
                            lEmailContent = lEmailContent + "<td>" + JobContent.DeliveryAddress + "</td></tr>";
                            lEmailContent = lEmailContent + "</table>";

                            lEmailContent = lEmailContent + "<table border='1' width=100%>";
                            lEmailContent = lEmailContent + "<tr><td width='20%'>" + "Special Remarks\n(特别备注)" + "</td>";
                            lEmailContent = lEmailContent + "<td>" + JobContent.Remarks + "</td></tr>";
                            lEmailContent = lEmailContent + "</table>";

                            lEmailContent = lEmailContent + "<table border='1' width=100%>";

                            lEmailContent = lEmailContent + "<tr><td width='20%'>" + "Site Contact\n(联系人)" + "</td>";
                            lEmailContent = lEmailContent + "<td width='15%'>" + JobContent.Scheduler_Name.Trim() + "</td>";
                            lEmailContent = lEmailContent + "<td width='13%'>" + "H / P\n(手机号码)" + " </td>";
                            lEmailContent = lEmailContent + "<td width='16%'>" + JobContent.Scheduler_HP.Trim() + "</td>";
                            lEmailContent = lEmailContent + "<td width='13%'>" + "Email\n(电邮地址)" + "</td>";
                            lEmailContent = lEmailContent + "<td>" + JobContent.Scheduler_Tel.Trim() + "</td></tr>";

                            lEmailContent = lEmailContent + "<tr><td width='20%'>" + "Goods Receiver\n(收货人)" + "</td>";
                            lEmailContent = lEmailContent + "<td width='15%'>" + JobContent.SiteEngr_Name.Trim() + "</td>";
                            lEmailContent = lEmailContent + "<td width='13%'>" + "H / P\n(手机号码)" + " </td>";
                            lEmailContent = lEmailContent + "<td width='16%'>" + JobContent.SiteEngr_HP.Trim() + "</td>";
                            lEmailContent = lEmailContent + "<td width='13%'>" + "Email\n(电邮地址)" + "</td>";
                            lEmailContent = lEmailContent + "<td>" + JobContent.SiteEngr_Tel.Trim() + "</td></tr>";
                            lEmailContent = lEmailContent + "</table>";

                            lEmailContent = lEmailContent + "<table border='1' width=100%><tr>";

                            lEmailContent = lEmailContent + "<td colspan='3'>" + "NatSteel Contacts (大众钢铁联系人) (Fax:62619133)" + "</td></tr>";

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
                                    if (lEmailTo == "")
                                    {
                                        lEmailTo = lVar1;
                                    }
                                    else
                                    {
                                        lEmailTo = lEmailTo + ";" + lVar1;
                                    }
                                }

                                lVar1 = JobContent.SiteEngr_Tel.Trim();
                                if (lVar1.Length > 0 && lEmailCc.IndexOf(lVar1) < 0)
                                {
                                    if (lEmailTo == "")
                                    {
                                        lEmailTo = lVar1;
                                    }
                                    else
                                    {
                                        lEmailTo = lEmailTo + ";" + lVar1;
                                    }
                                }
                            }

                            lVar = "";
                            if (lCustomer != null) lVar = lCustomer.CustomerName.Trim();

                            lEmailSubject = JobContent.PONumber.Trim() + " - " + lVar + " - MESH No. " + JobID.ToString();

                            var lOESEmail = new SendGridEmail();

                            string lEmailFromAddress = "eprompt@natsteel.com.sg";
                            string lEmailFromName = "OES Email Service";

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

        //List of Used BBS No
        [HttpPost]
        [Route("/ExportPONoToExcel_beam/{CustomerCode}/{ProjectCode}")]
        //[ValidateAntiForgeryHeader]
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
                    "FROM dbo.OESBBSSum B left outer join dbo.OESCTSMESHJobAdvice A " +
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

            //return Json(bExcel, JsonRequestBehavior.AllowGet);
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
                ws.Column(lColNo).Width = 16;       //"CAB PO WT\n剪弯铁料单重量";
                ws.Column(lColNo + 1).Width = 16;       //"CAB Delivery WT\n剪弯铁来货重量"
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
                ws.Cells[1, 1].Value = "PO List for Cut and Bend Bars\n剪弯铁订单列表";
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
                ws.Cells[lRowNo, 1].Value = "Cut & Bend Total (剪弯铁汇总):";
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
                ws.Cells[lRowNo, lColNo].Value = "CAB PO Weight\n剪弯铁料单重量";
                ws.Cells[lRowNo, lColNo + 1].Value = "CAB Delivery Weight\n剪弯铁来货重量";
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
                            ws.Cells[j + lRowNo, lColNo].Value = lReturn[i].CABOrderWT; //"CAB PO WT\n剪弯铁料单重量";
                            ws.Cells[j + lRowNo, lColNo + 1].Value = lReturn[i].CABDelWT; //"CAB Delivery WT\n剪弯铁来货重量";
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


        //List of raised PO
        //[HttpPost]
        //[Route("/usedPONoList_beam/{CustomerCode}/{ProjectCode}")]
        ////[ValidateAntiForgeryHeader]
        //public ActionResult usedPONoList(string CustomerCode, string ProjectCode)
        //{
        //    BBSSumProcess(CustomerCode, ProjectCode);

        //    var lCmd = new SqlCommand();
        //    SqlDataReader lRst;
        //    var lNDSCon = new SqlConnection();
        //    var lReturn = (new[]{ new
        //    {
        //        SSNNo = 0,
        //        PONo = "",
        //        POStatus = "",
        //        WBS1 = "",
        //        WBS2 = "",
        //        WBS3 = "",
        //        PODate = "",
        //        RequiredDate = "",
        //        DeliveryDate = "",
        //        CABOrderWT = "",
        //        CABDelWT = "",
        //        SBOrderWT = "",
        //        SBDelWT = "",
        //        MESHOrderWT = "",
        //        MESHDelWT = "",
        //        CAGEOrderWT = "",
        //        CAGEDelWT = "",
        //        PORemarks = "",
        //    }}).ToList();
        //    if (CustomerCode != null && ProjectCode != null)
        //    {
        //        if (CustomerCode.Length > 0 && ProjectCode.Length > 0)
        //        {
        //            lCmd.CommandText =
        //            "SELECT B.PONo, " +
        //            "B.WBS1, " +
        //            "B.WBS2, " +
        //            "B.WBS3, " +
        //            "B.PODate, " +
        //            "B.RequiredDate, " +
        //            "B.DeliveryDate, " +
        //            "SUM(B.CABOrderWT), " +
        //            "SUM(B.CABDeliveryWT), " +
        //            "SUM(B.SBOrderWT), " +
        //            "SUM(B.SBDeliveryWT), " +
        //            "SUM(B.MESHOrderWT), " +
        //            "SUM(B.MESHDeliveryWT), " +
        //            "SUM(B.CAGEOrderWT), " +
        //            "SUM(B.CAGEDeliveryWT), " +
        //            "A.OrderStatus, " +
        //            "A.Remarks, " +
        //            "MIN(B.SSNNo) " +
        //            "FROM dbo.OESBBSSum B left outer join dbo.OESCTSMESHJobAdvice A " +
        //            "ON B.CustomerCode = A.CustomerCode " +
        //            "AND B.ProjectCode = A.ProjectCode " +
        //            "AND B.JobID = A.JobID " +
        //            "WHERE B.CustomerCode = '" + CustomerCode + "' " +
        //            "AND B.ProjectCode = '" + ProjectCode + "' " +
        //            "GROUP BY " +
        //            "B.PONo, " +
        //            "B.WBS1, " +
        //            "B.WBS2, " +
        //            "B.WBS3, " +
        //            "B.PODate, " +
        //            "B.RequiredDate, " +
        //            "B.DeliveryDate, " +
        //            "A.OrderStatus, " +
        //            "A.Remarks " +
        //            "ORDER BY B.DeliveryDate DESC, B.PONo ";

        //            var lProcessObj = new ProcessController();
        //            if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
        //            {
        //                lCmd.Connection = lNDSCon;
        //                lCmd.CommandTimeout = 300;
        //                lRst = lCmd.ExecuteReader();
        //                if (lRst.HasRows)
        //                {
        //                    var lSNo = 0;
        //                    while (lRst.Read())
        //                    {
        //                        lSNo = lSNo + 1;
        //                        var lStatus = (lRst.GetValue(15) == DBNull.Value ? "" : lRst.GetString(15));
        //                        if (lRst.GetValue(6) != DBNull.Value)
        //                        {
        //                            lStatus = "Delivered";
        //                        }
        //                        lReturn.Add(new
        //                        {
        //                            SSNNo = lSNo,
        //                            PONo = lRst.GetString(0),
        //                            POStatus = lStatus,
        //                            WBS1 = lRst.GetString(1),
        //                            WBS2 = lRst.GetString(2),
        //                            WBS3 = lRst.GetString(3),
        //                            PODate = (lRst.GetValue(4) == DBNull.Value ? "" : lRst.GetDateTime(4).ToString("yyyy-MM-dd")),
        //                            RequiredDate = (lRst.GetValue(5) == DBNull.Value ? "" : lRst.GetDateTime(5).ToString("yyyy-MM-dd")),
        //                            DeliveryDate = (lRst.GetValue(6) == DBNull.Value ? "" : lRst.GetDateTime(6).ToString("yyyy-MM-dd")),
        //                            CABOrderWT = lRst.GetDecimal(7).ToString("###,###,###.000;(###,###.000); "),
        //                            CABDelWT = lRst.GetDecimal(8).ToString("###,###,###.000;(###,###.000); "),
        //                            SBOrderWT = lRst.GetDecimal(9).ToString("###,###,###.000;(###,###.000); "),
        //                            SBDelWT = lRst.GetDecimal(10).ToString("###,###,###.000;(###,###.000); "),
        //                            MESHOrderWT = lRst.GetDecimal(11).ToString("###,###,###.000;(###,###.000); "),
        //                            MESHDelWT = lRst.GetDecimal(12).ToString("###,###,###.000;(###,###.000); "),
        //                            CAGEOrderWT = lRst.GetDecimal(14).ToString("###,###,###.000;(###,###.000); "),
        //                            CAGEDelWT = lRst.GetDecimal(14).ToString("###,###,###.000;(###,###.000); "),
        //                            PORemarks = (lRst.GetValue(16) == DBNull.Value ? "" : lRst.GetString(16).Trim())
        //                        });
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

        //    if (lReturn.Count > 1)
        //    {
        //        lReturn.RemoveAt(0);
        //    }

        //    // return Json(lReturn, JsonRequestBehavior.AllowGet);
        //    return Ok(lReturn);
        //}

        //List of Used BBS No
        //[HttpPost]
        //[Route("/ExportBBSNoToExcel_beam/{CustomerCode/{ProjectCode}/{JobID}")]
        ////[ValidateAntiForgeryHeader]
        //public ActionResult ExportBBSNoToExcel(string CustomerCode, string ProjectCode, int JobID)
        //{
        //    decimal lCABDelTotal = 0;
        //    decimal lSBDelTotal = 0;
        //    decimal lMESHDelTotal = 0;
        //    decimal lCAGEDelTotal = 0;
        //    decimal lCABOrderTotal = 0;
        //    decimal lSBOrderTotal = 0;
        //    decimal lMESHOrderTotal = 0;
        //    decimal lCAGEOrderTotal = 0;
        //    int lColNo = 0;
        //    int lRowNo = 0;

        //    var lCmd = new SqlCommand();
        //    SqlDataReader lRst;
        //    var lNDSCon = new SqlConnection();
        //    var lCompanyName = db.Customer.Find(CustomerCode).CustomerName;
        //    var lProjectTitle = db.Project.Find(CustomerCode, ProjectCode).ProjectTitle;
        //    var lProjectCodeMESH = db.Project.Find(CustomerCode, ProjectCode).ProjectCodeMESH;
        //    var lProjectCodeCage = db.Project.Find(CustomerCode, ProjectCode).ProjectCodeCage;

        //    if (lProjectCodeMESH == null) lProjectCodeMESH = "";
        //    if (lProjectCodeCage == null) lProjectCodeCage = "";
        //    lProjectCodeMESH = lProjectCodeMESH.Trim();
        //    lProjectCodeCage = lProjectCodeCage.Trim();

        //    var lReturn = new List<BBSSumModels>();
        //    if (CustomerCode != null && ProjectCode != null)
        //    {
        //        if (CustomerCode.Length > 0 && ProjectCode.Length > 0)
        //        {
        //            lCmd.CommandText =
        //                "SELECT * FROM dbo.OESBBSSum " +
        //                "WHERE CustomerCode = '" + CustomerCode + "' " +
        //                "AND ProjectCode = '" + ProjectCode + "' " +
        //                "order by SSNNo ";

        //            var lProcessObj = new ProcessController();
        //            if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
        //            {
        //                lCmd.Connection = lNDSCon;
        //                lCmd.CommandTimeout = 300;
        //                lRst = lCmd.ExecuteReader();
        //                if (lRst.HasRows)
        //                {
        //                    while (lRst.Read())
        //                    {
        //                        lReturn.Add(new BBSSumModels()
        //                        {
        //                            SSNNo = lRst.GetInt32(2),
        //                            PONo = lRst.GetString(5),
        //                            BBSNo = lRst.GetString(6),
        //                            BBSDesc = lRst.GetString(7),
        //                            WBS1 = lRst.GetString(8),
        //                            WBS2 = lRst.GetString(9),
        //                            WBS3 = lRst.GetString(10),
        //                            StrucEle = lRst.GetString(11),
        //                            PODate = (lRst.GetValue(12) == DBNull.Value ? "" : lRst.GetDateTime(12).ToString("yyyy-MM-dd")),
        //                            RequiredDate = (lRst.GetValue(13) == DBNull.Value ? "" : lRst.GetDateTime(13).ToString("yyyy-MM-dd")),
        //                            DeliveryDate = (lRst.GetValue(14) == DBNull.Value ? "" : lRst.GetDateTime(14).ToString("yyyy-MM-dd")),
        //                            CABOrderWT = lRst.GetDecimal(15),
        //                            CABDelWT = lRst.GetDecimal(16),
        //                            SBOrderWT = lRst.GetDecimal(17),
        //                            SBDelWT = lRst.GetDecimal(18),
        //                            MESHOrderWT = lRst.GetDecimal(19),
        //                            MESHDelWT = lRst.GetDecimal(20),
        //                            CAGEOrderWT = lRst.GetDecimal(21),
        //                            CAGEDelWT = lRst.GetDecimal(22),
        //                            Remarks = lRst.GetString(24)
        //                        });
        //                        lCABOrderTotal = lCABOrderTotal + lRst.GetDecimal(15);
        //                        lCABDelTotal = lCABDelTotal + lRst.GetDecimal(16);
        //                        lSBOrderTotal = lSBOrderTotal + lRst.GetDecimal(17);
        //                        lSBDelTotal = lSBDelTotal + lRst.GetDecimal(18);
        //                        lMESHOrderTotal = lMESHOrderTotal + lRst.GetDecimal(19);
        //                        lMESHDelTotal = lMESHDelTotal + lRst.GetDecimal(20);
        //                        lCAGEOrderTotal = lCAGEOrderTotal + lRst.GetDecimal(21);
        //                        lCAGEDelTotal = lCAGEDelTotal + lRst.GetDecimal(22);
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

        //    if (lReturn.Count > 1)
        //    {
        //        lReturn.RemoveAt(0);
        //    }

        //    ExcelPackage package = new ExcelPackage();
        //    ExcelWorksheet ws = package.Workbook.Worksheets.Add("CAB");

        //    ExcelSheetGeneration("CAB",
        //        ref ws,
        //        ref lReturn,
        //        lCABDelTotal,
        //        lSBDelTotal,
        //        lMESHDelTotal,
        //        lCAGEDelTotal,
        //        lCABOrderTotal,
        //        lSBOrderTotal,
        //        lMESHOrderTotal,
        //        lCAGEOrderTotal,
        //        lCompanyName,
        //        lProjectTitle);

        //    if (lSBOrderTotal > 0 || lSBDelTotal > 0)
        //    {
        //        ExcelWorksheet wsSB = package.Workbook.Worksheets.Add("SB");

        //        ExcelSheetGeneration("SB",
        //            ref wsSB,
        //            ref lReturn,
        //            lCABDelTotal,
        //            lSBDelTotal,
        //            lMESHDelTotal,
        //            lCAGEDelTotal,
        //            lCABOrderTotal,
        //            lSBOrderTotal,
        //            lMESHOrderTotal,
        //            lCAGEOrderTotal,
        //            lCompanyName,
        //            lProjectTitle);
        //    }

        //    if (lMESHOrderTotal > 0 || lMESHDelTotal > 0)
        //    {
        //        ExcelWorksheet wsMESH = package.Workbook.Worksheets.Add("MESH");

        //        ExcelSheetGeneration("MESH",
        //            ref wsMESH,
        //            ref lReturn,
        //            lCABDelTotal,
        //            lSBDelTotal,
        //            lMESHDelTotal,
        //            lCAGEDelTotal,
        //            lCABOrderTotal,
        //            lSBOrderTotal,
        //            lMESHOrderTotal,
        //            lCAGEOrderTotal,
        //            lCompanyName,
        //            lProjectTitle);
        //    }

        //    if (lCAGEOrderTotal > 0 || lCAGEDelTotal > 0)
        //    {
        //        ExcelWorksheet wsCAGE = package.Workbook.Worksheets.Add("CAGE");

        //        ExcelSheetGeneration("CAGE",
        //            ref wsCAGE,
        //            ref lReturn,
        //            lCABDelTotal,
        //            lSBDelTotal,
        //            lMESHDelTotal,
        //            lCAGEDelTotal,
        //            lCABOrderTotal,
        //            lSBOrderTotal,
        //            lMESHOrderTotal,
        //            lCAGEOrderTotal,
        //            lCompanyName,
        //            lProjectTitle);
        //    }

        //    MemoryStream ms = new MemoryStream();
        //    package.SaveAs(ms);

        //    var bExcel = new byte[ms.Length];
        //    ms.Position = 0;
        //    ms.Read(bExcel, 0, bExcel.Length);

        //    //bPDF = ms.GetBuffer();
        //    ms.Flush();
        //    ms.Dispose();

        //    //            return Json(bExcel, JsonRequestBehavior.AllowGet);

        //    return Ok(bExcel);
        //}


        //[HttpGet]
        //[Route("/ExcelSheetGeneration_beam/{ProdType/{ProjectCode}/{JobID}")]
        //private void ExcelSheetGeneration(string ProdType,
        //    ref ExcelWorksheet ws,
        //    ref List<BBSSumModels> lReturn,
        //    decimal lCABDelTotal,
        //    decimal lSBDelTotal,
        //    decimal lMESHDelTotal,
        //    decimal lCAGEDelTotal,
        //    decimal lCABOrderTotal,
        //    decimal lSBOrderTotal,
        //    decimal lMESHOrderTotal,
        //    decimal lCAGEOrderTotal,
        //    string lCompanyName,
        //    string lProjectTitle
        //)
        //{
        //    int lColNo = 0;
        //    int lRowNo = 0;
        //    ws.Column(1).Width = 5;         //"SNo\n序号";
        //    ws.Column(2).Width = 15;        //"BBS No\n加工表号";
        //    ws.Column(3).Width = 37;        // "BBS Description\n施工部位";
        //    ws.Column(4).Width = 8;        //"WBS1\n楼座";
        //    ws.Column(5).Width = 8;        //"WBS2\n楼座";
        //    ws.Column(6).Width = 7;         //"WBS3\n楼座";
        //    ws.Column(7).Width = 10;        //"Structure Element\n构件"
        //    ws.Column(8).Width = 11;        //"PO Date\n订货日期";
        //    ws.Column(9).Width = 11;        //"Required Date\n到场日期";
        //    ws.Column(10).Width = 15;       //"Date Delivered\n实际到场日期"
        //    lColNo = 11;
        //    if (ProdType == "CAB")
        //    {
        //        ws.Column(lColNo).Width = 16;       //"CAB PO WT\n剪弯铁料单重量";
        //        ws.Column(lColNo + 1).Width = 16;       //"CAB Delivery WT\n剪弯铁来货重量"
        //        lColNo = lColNo + 2;
        //    }

        //    if (ProdType == "SB")
        //    {
        //        ws.Column(lColNo).Width = 16;       //"SB Delivery WT\n标直铁来货重量"
        //        ws.Column(lColNo + 1).Width = 16;       //"SB Delivery WT\n标直铁来货重量"
        //        lColNo = lColNo + 2;
        //    }

        //    if (ProdType == "MESH")
        //    {
        //        ws.Column(lColNo).Width = 15;       //"MESH Delivery WT\n标直铁来货重量"
        //        ws.Column(lColNo + 1).Width = 15;       //"MESH Delivery WT\n标直铁来货重量"
        //        lColNo = lColNo + 2;
        //    }

        //    if (ProdType == "CAGE")
        //    {
        //        ws.Column(lColNo).Width = 15;       //"CAGE Delivery WT\n标直铁来货重量"
        //        ws.Column(lColNo + 1).Width = 15;       //"CAGE Delivery WT\n标直铁来货重量"
        //        lColNo = lColNo + 2;
        //    }

        //    ws.Column(lColNo).Width = 20;       //"Remarks\n备注"

        //    string lColLetter = Convert.ToChar(64 + lColNo).ToString(); // A Ascii -- 65

        //    ws.Cells["A1:" + lColLetter + "1"].Merge = true;
        //    ws.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //    ws.Cells[1, 1].Style.Font.Bold = true;
        //    ws.Cells[1, 1].Style.Font.Size = 12;
        //    ws.Cells[1, 1].Style.WrapText = true;
        //    if (ProdType == "CAB")
        //    {
        //        ws.Cells[1, 1].Value = "BBS List for Cut and Bend Bars\n剪弯铁订单详细列表";
        //    }
        //    else if (ProdType == "SB")
        //    {
        //        ws.Cells[1, 1].Value = "BBS List for Standard Length Bars\n标准直铁订单详细列表";
        //    }
        //    else if (ProdType == "MESH")
        //    {
        //        ws.Cells[1, 1].Value = "BBS List for MESH\n铁网订单详细列表";
        //    }
        //    else if (ProdType == "CAGE")
        //    {
        //        ws.Cells[1, 1].Value = "BBS List for Cages\n铁笼铁订单详细列表";
        //    }



        //    ws.Row(1).Height = 30;

        //    ws.Cells["A2:C2"].Merge = true;
        //    ws.Cells[2, 1].Value = "Company Name (公司名称):";
        //    ws.Cells[2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    ws.Cells[2, 1].Style.Font.Bold = true;
        //    ws.Cells["D2:" + lColLetter + "2"].Merge = true;
        //    ws.Cells[2, 4].Value = lCompanyName;
        //    ws.Cells[2, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //    ws.Cells[2, 4].Style.Font.Bold = true;

        //    ws.Cells["A3:C3"].Merge = true;
        //    ws.Cells[3, 1].Value = "Project Title (工程项目名称):";
        //    ws.Cells[3, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //    ws.Cells[3, 1].Style.Font.Bold = true;

        //    ws.Cells["D3:" + lColLetter + "3"].Merge = true;
        //    ws.Cells[3, 4].Value = lProjectTitle;
        //    ws.Cells[3, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //    ws.Cells[3, 4].Style.Font.Bold = true;

        //    lRowNo = 4;

        //    if (ProdType == "CAB" && (lCABOrderTotal > 0 || lCABDelTotal > 0))
        //    {
        //        ws.Cells["A" + lRowNo.ToString() + ":C" + lRowNo.ToString() + ""].Merge = true;
        //        ws.Cells[lRowNo, 1].Value = "Cut & Bend Total (剪弯铁汇总):";
        //        ws.Cells[lRowNo, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //        ws.Cells[lRowNo, 1].Style.Font.Bold = true;

        //        ws.Cells["D" + lRowNo.ToString() + ":E" + lRowNo.ToString()].Merge = true;
        //        ws.Cells[lRowNo, 4].Value = lCABDelTotal;
        //        ws.Cells[lRowNo, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //        ws.Cells[lRowNo, 4].Style.Numberformat.Format = "###,###,###.000;(###,###,###.000); ";
        //        ws.Cells[lRowNo, 4].Style.Font.Bold = true;

        //        lRowNo = lRowNo + 1;
        //    }

        //    if ((ProdType == "CAB" || ProdType == "SB") && (lSBOrderTotal > 0 || lSBDelTotal > 0))
        //    {
        //        ws.Cells["A" + lRowNo.ToString() + ":C" + lRowNo.ToString() + ""].Merge = true;
        //        ws.Cells[lRowNo, 1].Value = "Standard Length Bars Total (标准直铁汇总):";
        //        ws.Cells[lRowNo, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //        ws.Cells[lRowNo, 1].Style.Font.Bold = true;

        //        ws.Cells["D" + lRowNo.ToString() + ":E" + lRowNo.ToString()].Merge = true;
        //        ws.Cells[lRowNo, 4].Value = lSBDelTotal;
        //        ws.Cells[lRowNo, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //        ws.Cells[lRowNo, 4].Style.Numberformat.Format = "###,###,###.000;(###,###,###.000); ";
        //        ws.Cells[lRowNo, 4].Style.Font.Bold = true;
        //        lRowNo = lRowNo + 1;
        //    }

        //    if ((ProdType == "CAB" || ProdType == "MESH") && (lMESHOrderTotal > 0 || lMESHDelTotal > 0))
        //    {
        //        ws.Cells["A" + lRowNo.ToString() + ":C" + lRowNo.ToString() + ""].Merge = true;
        //        ws.Cells[lRowNo, 1].Value = "MESH Total (铁网汇总):";
        //        ws.Cells[lRowNo, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //        ws.Cells[lRowNo, 1].Style.Font.Bold = true;

        //        ws.Cells["D" + lRowNo.ToString() + ":E" + lRowNo.ToString()].Merge = true;
        //        ws.Cells[lRowNo, 4].Value = lMESHDelTotal;
        //        ws.Cells[lRowNo, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //        ws.Cells[lRowNo, 4].Style.Numberformat.Format = "###,###,###.000;(###,###,###.000); ";
        //        ws.Cells[lRowNo, 4].Style.Font.Bold = true;
        //        lRowNo = lRowNo + 1;
        //    }

        //    if ((ProdType == "CAB" || ProdType == "CAGE") && (lCAGEOrderTotal > 0 || lCAGEDelTotal > 0))
        //    {
        //        ws.Cells["A" + lRowNo.ToString() + ":C" + lRowNo.ToString() + ""].Merge = true;
        //        ws.Cells[lRowNo, 1].Value = "Cage Total (铁笼汇总):";
        //        ws.Cells[lRowNo, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //        ws.Cells[lRowNo, 1].Style.Font.Bold = true;

        //        ws.Cells["D" + lRowNo.ToString() + ":E" + lRowNo.ToString()].Merge = true;
        //        ws.Cells[lRowNo, 4].Value = lCAGEDelTotal;
        //        ws.Cells[lRowNo, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //        ws.Cells[lRowNo, 4].Style.Numberformat.Format = "###,###,###.000;(###,###,###.000); ";
        //        ws.Cells[lRowNo, 4].Style.Font.Bold = true;
        //        lRowNo = lRowNo + 1;
        //    }

        //    if (lRowNo > 5)
        //    {
        //        ws.Cells["A" + lRowNo.ToString() + ":C" + lRowNo.ToString() + ""].Merge = true;
        //        ws.Cells[lRowNo, 1].Value = "Grand Total (总计):";
        //        ws.Cells[lRowNo, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //        ws.Cells[lRowNo, 1].Style.Font.Bold = true;

        //        ws.Cells["D" + lRowNo.ToString() + ":E" + lRowNo.ToString()].Merge = true;
        //        ws.Cells[lRowNo, 4].Value = lCABDelTotal + lSBDelTotal + lMESHDelTotal + lCAGEDelTotal;
        //        ws.Cells[lRowNo, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //        ws.Cells[lRowNo, 4].Style.Numberformat.Format = "###,###,###.000;(###,###,###.000); ";
        //        ws.Cells[lRowNo, 4].Style.Font.Bold = true;
        //        lRowNo = lRowNo + 1;
        //    }

        //    ws.Cells["F4" + ":" + lColLetter + (lRowNo - 1).ToString()].Merge = true;

        //    ExcelRange lrg = ws.Cells["A1:" + lColLetter + (lRowNo - 1).ToString()];
        //    lrg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        //    lrg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
        //    lrg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
        //    lrg.Style.Border.Right.Style = ExcelBorderStyle.Thin;

        //    ExcelRange lrg4 = ws.Cells["A" + lRowNo.ToString() + ":" + lColLetter + lRowNo.ToString() + ""];
        //    lrg4.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        //    lrg4.Style.Border.Top.Style = ExcelBorderStyle.Thin;
        //    lrg4.Style.Border.Left.Style = ExcelBorderStyle.Thin;
        //    lrg4.Style.Border.Right.Style = ExcelBorderStyle.Thin;

        //    lrg4.Style.Fill.PatternType = ExcelFillStyle.Solid;
        //    lrg4.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);

        //    lrg4.Style.WrapText = true;

        //    ws.Cells[lRowNo, 1].Value = "SNo\n序号";
        //    ws.Cells[lRowNo, 2].Value = "BBS No\n加工表号";
        //    ws.Cells[lRowNo, 3].Value = "BBS Description\n施工部位";
        //    ws.Cells[lRowNo, 4].Value = "WBS1\n楼座";
        //    ws.Cells[lRowNo, 5].Value = "WBS2\n楼层";
        //    ws.Cells[lRowNo, 6].Value = "WBS3\n分部";
        //    ws.Cells[lRowNo, 7].Value = "Structure Element\n构件";
        //    ws.Cells[lRowNo, 8].Value = "PO Date\n订货日期";
        //    ws.Cells[lRowNo, 9].Value = "Required Date\n到场日期";
        //    ws.Cells[lRowNo, 10].Value = "Date Delivered\n实际到场日期";

        //    lColNo = 11;
        //    if (ProdType == "CAB")
        //    {
        //        ws.Cells[lRowNo, lColNo].Value = "CAB PO Weight\n剪弯铁料单重量";
        //        ws.Cells[lRowNo, lColNo + 1].Value = "CAB Delivery Weight\n剪弯铁来货重量";
        //        lColNo = lColNo + 2;
        //    }
        //    if (ProdType == "SB")
        //    {
        //        ws.Cells[lRowNo, lColNo].Value = "Standard Bars PO Weight\n标直铁料单重量";
        //        ws.Cells[lRowNo, lColNo + 1].Value = "Standard Bars Delivery Weight\n标直铁来货重量";
        //        lColNo = lColNo + 2;
        //    }
        //    if (ProdType == "MESH")
        //    {
        //        ws.Cells[lRowNo, lColNo].Value = "MESH PO Weight\n铁网料单重量";
        //        ws.Cells[lRowNo, lColNo + 1].Value = "MESH Delivery Weight\n铁网来货重量";
        //        lColNo = lColNo + 2;
        //    }
        //    if (ProdType == "CAGE")
        //    {
        //        ws.Cells[lRowNo, lColNo].Value = "Cage PO Weight\n铁笼料单重量";
        //        ws.Cells[lRowNo, lColNo + 1].Value = "Cage Delivery Weight\n铁笼来货重量";
        //        lColNo = lColNo + 2;
        //    }
        //    ws.Cells[lRowNo, lColNo].Value = "Remarks\n备注";

        //    lRowNo = lRowNo + 1;

        //    if (lReturn.Count > 0)
        //    {
        //        var j = 0;
        //        for (int i = 0; i < lReturn.Count; i++)
        //        {
        //            if ((ProdType == "CAB" && (lReturn[i].CABOrderWT > 0 || lReturn[i].CABDelWT > 0))
        //                || (ProdType == "SB" && (lReturn[i].SBOrderWT > 0 || lReturn[i].SBDelWT > 0))
        //                || (ProdType == "MESH" && (lReturn[i].MESHOrderWT > 0 || lReturn[i].MESHDelWT > 0))
        //                || (ProdType == "CAGE" && (lReturn[i].CAGEOrderWT > 0 || lReturn[i].CAGEDelWT > 0))
        //                )
        //            {
        //                ws.Cells[j + lRowNo, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                ws.Cells[j + lRowNo, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                ws.Cells[j + lRowNo, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                ws.Cells[j + lRowNo, 3].Style.WrapText = true;
        //                ws.Cells[j + lRowNo, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                ws.Cells[j + lRowNo, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                ws.Cells[j + lRowNo, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                ws.Cells[j + lRowNo, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                ws.Cells[j + lRowNo, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                ws.Cells[j + lRowNo, 9].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                ws.Cells[j + lRowNo, 10].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                lColNo = 11;
        //                if (ProdType == "CAB" && (lReturn[i].CABOrderWT > 0 || lReturn[i].CABDelWT > 0))
        //                {
        //                    ws.Cells[j + lRowNo, lColNo].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                    ws.Cells[j + lRowNo, lColNo + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                    ws.Cells[j + lRowNo, lColNo].Style.Numberformat.Format = "###,###.000;(###,###.000); ";
        //                    ws.Cells[j + lRowNo, lColNo + 1].Style.Numberformat.Format = "###,###.000;(###,###.000); ";
        //                    lColNo = lColNo + 2;
        //                }

        //                if (ProdType == "SB" && (lReturn[i].SBOrderWT > 0 || lReturn[i].SBDelWT > 0))
        //                {
        //                    ws.Cells[j + lRowNo, lColNo].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                    ws.Cells[j + lRowNo, lColNo + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                    ws.Cells[j + lRowNo, lColNo].Style.Numberformat.Format = "###,###.000;(###,###.000); ";
        //                    ws.Cells[j + lRowNo, lColNo + 1].Style.Numberformat.Format = "###,###.000;(###,###.000); ";
        //                    lColNo = lColNo + 2;
        //                }

        //                if (ProdType == "MESH" && (lReturn[i].MESHOrderWT > 0 || lReturn[i].MESHDelWT > 0))
        //                {
        //                    ws.Cells[j + lRowNo, lColNo].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                    ws.Cells[j + lRowNo, lColNo + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                    ws.Cells[j + lRowNo, lColNo].Style.Numberformat.Format = "###,###.000;(###,###.000); ";
        //                    ws.Cells[j + lRowNo, lColNo + 1].Style.Numberformat.Format = "###,###.000;(###,###.000); ";
        //                    lColNo = lColNo + 2;
        //                }

        //                if (ProdType == "CAGE" && (lReturn[i].CAGEOrderWT > 0 || lReturn[i].CAGEDelWT > 0))
        //                {
        //                    ws.Cells[j + lRowNo, lColNo].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                    ws.Cells[j + lRowNo, lColNo + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                    ws.Cells[j + lRowNo, lColNo].Style.Numberformat.Format = "###,###.000;(###,###.000); ";
        //                    ws.Cells[j + lRowNo, lColNo + 1].Style.Numberformat.Format = "###,###.000;(###,###.000); ";
        //                    lColNo = lColNo + 2;
        //                }
        //                ws.Cells[j + lRowNo, lColNo].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        //                ws.Cells[j + lRowNo, lColNo].Style.WrapText = true;


        //                var lBBSDesc = lReturn[i].BBSDesc;
        //                if (lReturn[i].SBDelWT > 0 || (lReturn[i].SBOrderWT > 0 || lReturn[i].MESHDelWT > 0 || lReturn[i].MESHOrderWT > 0))
        //                {
        //                    lBBSDesc = lBBSDesc.Replace(", ", "\n");
        //                    lBBSDesc = lBBSDesc.Replace(",", "\n");
        //                }
        //                ws.Cells[j + lRowNo, 1].Value = j + 1;
        //                ws.Cells[j + lRowNo, 2].Value = lReturn[i].BBSNo; //"BBS No\n加工表号";
        //                ws.Cells[j + lRowNo, 3].Value = lBBSDesc; //"BBS Description\n施工部位";
        //                ws.Cells[j + lRowNo, 4].Value = lReturn[i].WBS1; //"WBS1\n楼座";
        //                ws.Cells[j + lRowNo, 5].Value = lReturn[i].WBS2; //"WBS2\n楼层";
        //                ws.Cells[j + lRowNo, 6].Value = lReturn[i].WBS3; //"WBS3\n分部";
        //                ws.Cells[j + lRowNo, 7].Value = lReturn[i].StrucEle; //"Structure Element\n构件";
        //                ws.Cells[j + lRowNo, 8].Value = lReturn[i].PODate; //"PO Date\n订货日期";
        //                ws.Cells[j + lRowNo, 9].Value = lReturn[i].RequiredDate; //"Required Date\n到场日期";
        //                ws.Cells[j + lRowNo, 10].Value = lReturn[i].DeliveryDate; //"Date Delivered\n实际到场日期";
        //                lColNo = 11;
        //                if (ProdType == "CAB" && (lReturn[i].CABOrderWT > 0 || lReturn[i].CABDelWT > 0))
        //                {
        //                    ws.Cells[j + lRowNo, lColNo].Value = lReturn[i].CABOrderWT; //"CAB PO WT\n剪弯铁料单重量";
        //                    ws.Cells[j + lRowNo, lColNo + 1].Value = lReturn[i].CABDelWT; //"CAB Delivery WT\n剪弯铁来货重量";
        //                    lColNo = lColNo + 2;
        //                }

        //                if (ProdType == "SB" && (lReturn[i].SBOrderWT > 0 || lReturn[i].SBDelWT > 0))
        //                {
        //                    ws.Cells[j + lRowNo, lColNo].Value = lReturn[i].SBOrderWT;
        //                    ws.Cells[j + lRowNo, lColNo + 1].Value = lReturn[i].SBDelWT;
        //                    lColNo = lColNo + 2;
        //                }

        //                if (ProdType == "MESH" && (lReturn[i].MESHOrderWT > 0 || lReturn[i].MESHDelWT > 0))
        //                {
        //                    ws.Cells[j + lRowNo, lColNo].Value = lReturn[i].MESHOrderWT;
        //                    ws.Cells[j + lRowNo, lColNo + 1].Value = lReturn[i].MESHDelWT;
        //                    lColNo = lColNo + 2;
        //                }

        //                if (ProdType == "CAGE" && (lReturn[i].CAGEOrderWT > 0 || lReturn[i].CAGEDelWT > 0))
        //                {
        //                    ws.Cells[j + lRowNo, lColNo].Value = lReturn[i].CAGEOrderWT;
        //                    ws.Cells[j + lRowNo, lColNo + 1].Value = lReturn[i].CAGEDelWT;
        //                    lColNo = lColNo + 2;
        //                }
        //                ws.Cells[j + lRowNo, lColNo].Value = lReturn[i].Remarks; //"Remarks\n备注";
        //                j = j + 1;
        //            }
        //        }
        //    }


        //}

        //List of Used BBS No
        //[HttpPost]
        //[Route("/usedBBSNoList_beam/{CustomerCode}/{ProjectCode}/{JobID}")]
        //// [ValidateAntiForgeryHeader]
        //public ActionResult usedBBSNoList(string CustomerCode, string ProjectCode, int JobID)
        //{
        //    BBSSumProcess(CustomerCode, ProjectCode);
        //    var lCmd = new SqlCommand();
        //    SqlDataReader lRst;
        //    int lSNo = 0;
        //    var lNDSCon = new SqlConnection();
        //    var lReturn = (new[]{ new
        //    {
        //        SSNNo = 0,
        //        PONo = "",
        //        BBSNo = "",
        //        BBSDesc = "",
        //        WBS1 = "",
        //        WBS2 = "",
        //        WBS3 = "",
        //        StrucEle = "",
        //        PODate = "",
        //        RequiredDate= "",
        //        DeliveryDate= "",
        //        CABOrderWT = "",
        //        CABDelWT = "",
        //        SBOrderWT = "",
        //        SBDelWT = "",
        //        MESHOrderWT = "",
        //        MESHDelWT = "",
        //        CAGEOrderWT = "",
        //        CAGEDelWT = "",
        //        Remarks = ""
        //    }}).ToList();
        //    if (CustomerCode != null && ProjectCode != null)
        //    {
        //        if (CustomerCode.Length > 0 && ProjectCode.Length > 0)
        //        {
        //            lCmd.CommandText =
        //                "SELECT * FROM dbo.OESBBSSum " +
        //                "WHERE CustomerCode = '" + CustomerCode + "' " +
        //                "AND ProjectCode = '" + ProjectCode + "' " +
        //                "order by DeliveryDate DESC, BBSNo ";

        //            var lProcessObj = new ProcessController();
        //            if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
        //            {
        //                lCmd.Connection = lNDSCon;
        //                lCmd.CommandTimeout = 300;
        //                lRst = lCmd.ExecuteReader();
        //                if (lRst.HasRows)
        //                {
        //                    while (lRst.Read())
        //                    {
        //                        lSNo = lSNo + 1;
        //                        lReturn.Add(new
        //                        {
        //                            SSNNo = lSNo,
        //                            PONo = lRst.GetString(5),
        //                            BBSNo = lRst.GetString(6),
        //                            BBSDesc = lRst.GetString(7),
        //                            WBS1 = lRst.GetString(8),
        //                            WBS2 = lRst.GetString(9),
        //                            WBS3 = lRst.GetString(10),
        //                            StrucEle = lRst.GetString(11),
        //                            PODate = (lRst.GetValue(12) == DBNull.Value ? "" : lRst.GetDateTime(12).ToString("yyyy-MM-dd")),
        //                            RequiredDate = (lRst.GetValue(13) == DBNull.Value ? "" : lRst.GetDateTime(13).ToString("yyyy-MM-dd")),
        //                            DeliveryDate = (lRst.GetValue(14) == DBNull.Value ? "" : lRst.GetDateTime(14).ToString("yyyy-MM-dd")),
        //                            CABOrderWT = lRst.GetDecimal(15).ToString("###,###.000;(###,###.000); "),
        //                            CABDelWT = lRst.GetDecimal(16).ToString("###,###.000;(###,###.000); "),
        //                            SBOrderWT = lRst.GetDecimal(17).ToString("###,###.000;(###,###.000); "),
        //                            SBDelWT = lRst.GetDecimal(18).ToString("###,###.000;(###,###.000); "),
        //                            MESHOrderWT = lRst.GetDecimal(19).ToString("###,###.000;(###,###.000); "),
        //                            MESHDelWT = lRst.GetDecimal(20).ToString("###,###.000;(###,###.000); "),
        //                            CAGEOrderWT = lRst.GetDecimal(21).ToString("###,###.000;(###,###.000); "),
        //                            CAGEDelWT = lRst.GetDecimal(22).ToString("###,###.000;(###,###.000); "),
        //                            Remarks = lRst.GetString(24)
        //                        });
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

        //    if (lReturn.Count > 1)
        //    {
        //        lReturn.RemoveAt(0);
        //    }

        //    //            return Json(lReturn, JsonRequestBehavior.AllowGet);
        //    return Ok(lReturn);
        //}

        [HttpGet]
        [Route("/BBSSumInsert_beam/{CustomerCode}/{ProjectCode}/{lNDSCon}/{lOraRst}/{lDTBackup}/{lSSNNo}")]

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
            ",'" + DateTime.ParseExact(lLoadDate, "yyyyMMdd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd") + "' " +
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

        //[HttpPost]
        //[Route("/BBSSumProcess_beam/{CustomerCode}/{ProjectCode}")]
        //// [ValidateAntiForgeryHeader]
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
        //                lSumUpdatedDate = lRst.GetDateTime(0);
        //            }
        //        }
        //        lRst.Close();

        //        if (lSumUpdatedDate < DateTime.Today)
        //        {

        //            //get last delevery datetime
        //            lCmd.CommandText = "SELECT isNull(Max(DeliveryDate),DateAdd(yyyy,-10,getDate())) FROM dbo.OESBBSSum " +
        //        "WHERE CustomerCode = '" + CustomerCode + "' " +
        //        "AND ProjectCode = '" + ProjectCode + "' ";

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
        //            "FROM dbo.OESCTSMESHJobAdvice A, dbo.OESBBS B " +
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
        //            "FROM dbo.OESCTSMESHJobAdvice A, dbo.OESBBS B " +
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

        //Check duplicated BBS No
        [HttpPost]
        [Route("/checkBBSNo_beam/{CustomerCode}/{ProjectCode}/{JobID}/{BBSNo}")]
        // [ValidateAntiForgeryHeader]
        public ActionResult checkBBSNo(string CustomerCode, string ProjectCode, int JobID, string BBSNo)
        {
            var lCmd = new SqlCommand();
            SqlDataReader lRst;
            var lNDSCon = new SqlConnection();
            var lReturn = "";
            var lBBSNoVar = "";
            if (BBSNo != null && ProjectCode != null)
            {
                if (BBSNo.Length > 0 && ProjectCode.Length > 0)
                {
                    var lBBSNo = "'" + BBSNo.Replace(",", "','") + "'";
                    //lCmd.CommandText =
                    //    "select vchGroupMarkingName from dbo.GroupMarkingDetails G , " +
                    //    "dbo.SELevelDetails S, " +
                    //    "dbo.ProjectMaster P, " +
                    //    "dbo.ProductTypeMaster T " +
                    //    "WHERE G.intGroupMarkId = S.intGroupMarkId " +
                    //    "AND S.sitProductTypeId = T.sitProductTypeID " +
                    //    "AND G.intProjectId = P.intProjectId " +
                    //    "AND vchProductType = 'CAB' " +
                    //    "AND vchProjectCode = '" + ProjectCode + "' " +
                    //    "AND vchGroupMarkingName in (" + lBBSNo + ") ";

                    lCmd.CommandText = "SELECT BBS_NO " +
                    "FROM dbo.SAP_REQUEST_DETAILS D, dbo.SAP_REQUEST_HDR H " +
                    "WHERE D.ORD_REQ_NO = H.ORD_REQ_NO " +
                    "AND H.PROJ_ID = '" + ProjectCode + "' " +
                    "AND BBS_NO in (" + lBBSNo + ") " +
                    "AND D.PROD_TYPE = 'CAB' " +
                    "AND D.STATUS <> 'X' " +
                    "AND H.STATUS <> 'X' ";

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
                                lBBSNoVar = lRst.GetString(0);
                                lBBSNoVar = lBBSNoVar.Trim();
                                if (lReturn == "") lReturn = lBBSNoVar;
                                else lReturn = lReturn + "," + lBBSNoVar;
                            }
                        }
                        lRst.Close();

                        if (lReturn == "")
                        {
                            lCmd = new SqlCommand();
                            lCmd.CommandText =
                                "SELECT BBSNo " +
                                "FROM dbo.OESBBS B, dbo.OESCTSMESHJobAdvice J " +
                                "WHERE B.CustomerCode = '" + CustomerCode + "' " +
                                "AND B.ProjectCode = '" + ProjectCode + "' " +
                                "AND J.CustomerCode = '" + CustomerCode + "' " +
                                "AND J.ProjectCode = '" + ProjectCode + "' " +
                                "AND B.JobID = J.JobID " +
                                "AND J.OrderStatus = 'Submitted' " +
                                "AND B.JobID <> " + JobID + " " +
                                "AND B.BBSNo in (" + lBBSNo + ") " +
                                "AND B.BBSNo is not null ";

                            lCmd.Connection = lNDSCon;
                            lCmd.CommandTimeout = 300;
                            lRst = lCmd.ExecuteReader();
                            if (lRst.HasRows)
                            {
                                while (lRst.Read())
                                {
                                    lBBSNoVar = lRst.GetString(0);
                                    lBBSNoVar = lBBSNoVar.Trim();
                                    if (lReturn == "") lReturn = lBBSNoVar;
                                    else lReturn = lReturn + "," + lBBSNoVar;
                                }
                            }
                            lRst.Close();
                        }
                        lProcessObj.CloseNDSConnection(ref lNDSCon);
                    }
                    lProcessObj = null;
                }

                if (lReturn == "")
                {
                    var laBBSNo = BBSNo.Split(',');
                    if (laBBSNo.Length > 1)
                    {
                        for (int i = 0; i < laBBSNo.Length - 1; i++)
                        {
                            for (int j = i + 1; j < laBBSNo.Length; j++)
                            {
                                if (laBBSNo[i] == laBBSNo[j])
                                {
                                    if (lReturn == "") lReturn = laBBSNo[i];
                                    else lReturn = lReturn + "," + laBBSNo[i];
                                }
                            }
                        }
                    }

                }

            }
            lCmd = null;
            lNDSCon = null;
            lRst = null;

            // return Json(lReturn, JsonRequestBehavior.AllowGet);
            return Ok(lReturn);
        }


        //generate BBS ID
        [HttpPost]
        [Route("/genBBSID_beam/{CustomerCode}/{ProjectCode}/{JobID}")]
        // [ValidateAntiForgeryHeader]
        public ActionResult genBBSID(string CustomerCode, string ProjectCode, int JobID)
        {
            var lErrorMsg = "";
            int? lBBSID = 1;

            try
            {
                lBBSID = (from p in db.CTSMESHBBS
                          where p.CustomerCode == CustomerCode &&
                          p.ProjectCode == ProjectCode &&
                          p.JobID == JobID
                          select p.BBSID).DefaultIfEmpty(0).Max();
                if (lBBSID == null)
                {
                    lBBSID = 1;
                }
                else
                {
                    lBBSID = lBBSID + 1;
                }

                var lBBS = new CTSMESHBBSModels
                {
                    CustomerCode = CustomerCode,
                    ProjectCode = ProjectCode,
                    JobID = JobID,
                    BBSID = (int)lBBSID,
                    BBSNDSPostID = 0,
                    BBSProdCategory = "Beam Stirrup Cage",
                    BBSStrucElem = "Beam",
                    BBSDesc = "",
                    BBSTotalPcs = 0,
                    BBSTotalWT = 0,
                    BBSDrawing = "N",
                    BBSDrawingRev = "",
                    BBSSOR = "",
                    BBSNDSGroupMark = "",
                    BBSOrder = false,
                    UpdateDate = DateTime.Now,
                    UpdateBy = User.Identity.GetUserName()
                };
                db.CTSMESHBBS.Add(lBBS);
                db.SaveChanges();

            }
            catch (Exception ex)
            {
                lErrorMsg = ex.Message;
            }
            //   return Json(lBBSID, JsonRequestBehavior.AllowGet);
            return Ok(lBBSID);
        }

        [HttpGet]
        [Route("/checkUpdateBBSWT_beam/{CustomerCode}/{ProjectCode}/{JobID}")]
        private int checkUpdateBBSWT(string CustomerCode, string ProjectCode, int JobID)
        {
            int lReturn = 0;
            int lTotalPcs = 0;
            decimal lTotalWT = 0;
            var lBBS = (from p in db.CTSMESHBBS
                        where p.CustomerCode == CustomerCode &&
                        p.ProjectCode == ProjectCode &&
                        p.JobID == JobID
                        select p).ToList();

            if (lBBS.Count > 0)
            {
                for (int i = 0; i < lBBS.Count; i++)
                {
                    if (lBBS[i].BBSID == 1)
                    {
                        int lBBSID = lBBS[i].BBSID;
                        int? lBBSPcs = (from p in db.CTSMESHBeamDetails
                                        where p.CustomerCode == CustomerCode &&
                                        p.ProjectCode == ProjectCode &&
                                        p.JobID == JobID &&
                                        p.BBSID == lBBSID
                                        select p.MeshMemberQty).Sum();

                        decimal? lBBSWeight = (from p in db.CTSMESHBeamDetails
                                               where p.CustomerCode == CustomerCode &&
                                               p.ProjectCode == ProjectCode &&
                                               p.JobID == JobID &&
                                               p.BBSID == lBBSID
                                               select p.MeshTotalWT).Sum();

                        if (lBBSPcs == null)
                        {
                            lBBSPcs = 0;
                        }
                        if (lBBSWeight == null)
                        {
                            lBBSWeight = 0;
                        }
                        if (lBBS[i].BBSOrder == true)
                        {
                            lTotalPcs = lTotalPcs + (int)lBBSPcs;
                            lTotalWT = lTotalWT + (decimal)lBBSWeight;
                        }

                        if (lBBSPcs != lBBS[i].BBSTotalPcs || lBBSWeight != lBBS[i].BBSTotalWT)
                        {
                            var oldBBS = db.CTSMESHBBS.Find(CustomerCode, ProjectCode, JobID, lBBSID);
                            if (oldBBS != null)
                            {
                                CTSMESHBBSModels lUpdateBBS = oldBBS;
                                lUpdateBBS.BBSTotalPcs = (int)lBBSPcs;
                                lUpdateBBS.BBSTotalWT = (decimal)lBBSWeight;
                                db.Entry(oldBBS).CurrentValues.SetValues(lUpdateBBS);
                            }
                        }
                    }

                    if (lBBS[i].BBSID == 2)
                    {
                        int lBBSID = lBBS[i].BBSID;
                        int? lBBSPcs = (from p in db.CTSMESHColumnDetails
                                        where p.CustomerCode == CustomerCode &&
                                        p.ProjectCode == ProjectCode &&
                                        p.JobID == JobID &&
                                        p.BBSID == lBBSID
                                        select p.MeshMemberQty).Sum();

                        decimal? lBBSWeight = (from p in db.CTSMESHColumnDetails
                                               where p.CustomerCode == CustomerCode &&
                                               p.ProjectCode == ProjectCode &&
                                               p.JobID == JobID &&
                                               p.BBSID == lBBSID
                                               select p.MeshTotalWT).Sum();

                        if (lBBSPcs == null)
                        {
                            lBBSPcs = 0;
                        }
                        if (lBBSWeight == null)
                        {
                            lBBSWeight = 0;
                        }
                        if (lBBS[i].BBSOrder == true)
                        {
                            lTotalPcs = lTotalPcs + (int)lBBSPcs;
                            lTotalWT = lTotalWT + (decimal)lBBSWeight;
                        }

                        if (lBBSPcs != lBBS[i].BBSTotalPcs || lBBSWeight != lBBS[i].BBSTotalWT)
                        {
                            var oldBBS = db.CTSMESHBBS.Find(CustomerCode, ProjectCode, JobID, lBBSID);
                            if (oldBBS != null)
                            {
                                CTSMESHBBSModels lUpdateBBS = oldBBS;
                                lUpdateBBS.BBSTotalPcs = (int)lBBSPcs;
                                lUpdateBBS.BBSTotalWT = (decimal)lBBSWeight;
                                db.Entry(oldBBS).CurrentValues.SetValues(lUpdateBBS);
                            }
                        }
                    }

                    if (lBBS[i].BBSID == 8)
                    {
                        int lBBSID = lBBS[i].BBSID;
                        int? lBBSPcs = (from p in db.CTSMESHStdSheetDetails
                                        where p.CustomerCode == CustomerCode &&
                                        p.ProjectCode == ProjectCode &&
                                        p.JobID == JobID &&
                                        p.BBSID == lBBSID
                                        select p.order_pcs).Sum();

                        decimal? lBBSWeight = (from p in db.CTSMESHStdSheetDetails
                                               where p.CustomerCode == CustomerCode &&
                                               p.ProjectCode == ProjectCode &&
                                               p.JobID == JobID &&
                                               p.BBSID == lBBSID
                                               select p.order_wt).Sum();

                        if (lBBSPcs == null)
                        {
                            lBBSPcs = 0;
                        }
                        if (lBBSWeight == null)
                        {
                            lBBSWeight = 0;
                        }
                        if (lBBS[i].BBSOrder == true)
                        {
                            lTotalPcs = lTotalPcs + (int)lBBSPcs;
                            lTotalWT = lTotalWT + (decimal)lBBSWeight;
                        }

                        if (lBBSPcs != lBBS[i].BBSTotalPcs || lBBSWeight != lBBS[i].BBSTotalWT)
                        {
                            var oldBBS = db.CTSMESHBBS.Find(CustomerCode, ProjectCode, JobID, lBBSID);
                            if (oldBBS != null)
                            {
                                CTSMESHBBSModels lUpdateBBS = oldBBS;
                                lUpdateBBS.BBSTotalPcs = (int)lBBSPcs;
                                lUpdateBBS.BBSTotalWT = (decimal)lBBSWeight;
                                db.Entry(oldBBS).CurrentValues.SetValues(lUpdateBBS);
                            }
                        }
                    }

                    if (lBBS[i].BBSID != 1 && lBBS[i].BBSID != 2 && lBBS[i].BBSID != 8)
                    {
                        int lBBSID = lBBS[i].BBSID;
                        int? lBBSPcs = (from p in db.CTSMESHOthersDetails
                                        where p.CustomerCode == CustomerCode &&
                                        p.ProjectCode == ProjectCode &&
                                        p.JobID == JobID &&
                                        p.BBSID == lBBSID
                                        select p.MeshMemberQty).Sum();

                        decimal? lBBSWeight = (from p in db.CTSMESHOthersDetails
                                               where p.CustomerCode == CustomerCode &&
                                               p.ProjectCode == ProjectCode &&
                                               p.JobID == JobID &&
                                               p.BBSID == lBBSID
                                               select p.MeshTotalWT).Sum();

                        if (lBBSPcs == null)
                        {
                            lBBSPcs = 0;
                        }
                        if (lBBSWeight == null)
                        {
                            lBBSWeight = 0;
                        }
                        if (lBBS[i].BBSOrder == true)
                        {
                            lTotalPcs = lTotalPcs + (int)lBBSPcs;
                            lTotalWT = lTotalWT + (decimal)lBBSWeight;
                        }

                        if (lBBSPcs != lBBS[i].BBSTotalPcs || lBBSWeight != lBBS[i].BBSTotalWT)
                        {
                            var oldBBS = db.CTSMESHBBS.Find(CustomerCode, ProjectCode, JobID, lBBSID);
                            if (oldBBS != null)
                            {
                                CTSMESHBBSModels lUpdateBBS = oldBBS;
                                lUpdateBBS.BBSTotalPcs = (int)lBBSPcs;
                                lUpdateBBS.BBSTotalWT = (decimal)lBBSWeight;
                                db.Entry(oldBBS).CurrentValues.SetValues(lUpdateBBS);
                            }
                        }
                    }
                }

                var lBBSNSH = (from p in db.CTSMESHBBSNSH
                               where p.CustomerCode == CustomerCode &&
                               p.ProjectCode == ProjectCode &&
                               p.JobID == JobID &&
                               p.BBSOrder == true
                               select p).ToList();

                if (lBBSNSH.Count > 0)
                {
                    for (int i = 0; i < lBBSNSH.Count; i++)
                    {
                        if (lBBSNSH[i].BBSTotalPcs > 0)
                        {
                            lTotalPcs = lTotalPcs + lBBSNSH[i].BBSTotalPcs;
                        }
                        if (lBBSNSH[i].BBSTotalWT > 0)
                        {
                            lTotalWT = lTotalWT + lBBSNSH[i].BBSTotalWT;
                        }
                    }
                }

                var oldJob = db.CTSMESHJobAdvice.Find(CustomerCode, ProjectCode, JobID);
                if (oldJob != null)
                {
                    if (lTotalPcs != oldJob.TotalPcs || lTotalWT != oldJob.TotalWeight)
                    {
                        CTSMESHJobAdviceModels lUpdateJob = oldJob;
                        lUpdateJob.TotalPcs = lTotalPcs;
                        lUpdateJob.TotalWeight = lTotalWT;
                        db.Entry(oldJob).CurrentValues.SetValues(lUpdateJob);
                    }
                }

                db.SaveChanges();

            }
            return lReturn;
        }

        //get BBS List for Copy BBS
        [HttpPost]
        [Route("/getBBSCopy_beam/{CustomerCode}/{ProjectCode}/{JobID}/{BBSID}")]
        //[ValidateAntiForgeryHeader]
        public ActionResult getBBSCopy(string CustomerCode, string ProjectCode, int JobID, int BBSID)
        {
            if (BBSID == 1 || BBSID == 2 || BBSID == 8)
            {
                var content = (from p in db.CTSMESHBBS
                               where p.CustomerCode == CustomerCode &&
                               p.ProjectCode == ProjectCode &&
                               p.JobID == JobID &&
                               p.BBSID == BBSID &&
                               p.BBSTotalWT > 0
                               orderby p.BBSID
                               select p).ToList();

                //                return Json(content, JsonRequestBehavior.AllowGet);
                return Ok(content);
            }
            else
            {
                var content = (from p in db.CTSMESHBBS
                               where p.CustomerCode == CustomerCode &&
                               p.ProjectCode == ProjectCode &&
                               p.JobID == JobID &&
                               p.BBSID != 1 &&
                               p.BBSID != 2 &&
                               p.BBSID != 8 &&
                               p.BBSTotalWT > 0
                               orderby p.BBSID
                               select p).ToList();

                //  return Json(content, JsonRequestBehavior.AllowGet);
                return Ok(content);
            }
        }

        //get BBS List at Order Summary
        [HttpPost]
        [Route("/getBBSCust_beam/{CustomerCode}/{ProjectCode}/{JobID}")]
        // [ValidateAntiForgeryHeader]
        public ActionResult getBBSCust(string CustomerCode, string ProjectCode, int JobID)
        {
            var content = (from p in db.CTSMESHBBS
                           where p.CustomerCode == CustomerCode &&
                           p.ProjectCode == ProjectCode &&
                           p.JobID == JobID
                           orderby p.BBSID
                           select p).ToList();

            if (content.Count > 0)
            {
                for (int i = 0; i < content.Count; i++)
                {
                    if (content[i].BBSStrucElem == "SLAB-B")
                    {
                        content[i].BBSStrucElem = "Slab Bottom";
                    }
                    if (content[i].BBSStrucElem == "SLAB-T")
                    {
                        content[i].BBSStrucElem = "Slab Top";
                    }
                }
            }
            // return Json(content, JsonRequestBehavior.AllowGet);
            return Ok(content);
        }

        //save BBS
        [HttpPost]
        [Route("/saveBBSCust_beam/{bbsModels}")]
        //[ValidateAntiForgeryHeader]
        public ActionResult saveBBSCust(CTSMESHBBSModels bbsModels)
        {
            var lErrorMsg = "";
            bbsModels.UpdateDate = DateTime.Now;
            try
            {
                if (bbsModels != null)
                {
                    if (bbsModels.CustomerCode == null)
                    {
                        bbsModels.CustomerCode = "";
                    }
                    if (bbsModels.CustomerCode.Length > 0)
                    {
                        bbsModels.BBSDrawing = (bbsModels.BBSDrawing != null && bbsModels.BBSDrawing.Length > 0 ? "Y" : "N");
                        bbsModels.UpdateBy = User.Identity.GetUserName();
                        bbsModels.UpdateDate = DateTime.Now;

                        if (bbsModels.BBSStrucElem == "Slab Bottom")
                        {
                            bbsModels.BBSStrucElem = "SLAB-B";
                        }
                        if (bbsModels.BBSStrucElem == "Slab Top")
                        {
                            bbsModels.BBSStrucElem = "SLAB-T";
                        }

                        var oldBBS = db.CTSMESHBBS.Find(bbsModels.CustomerCode, bbsModels.ProjectCode, bbsModels.JobID, bbsModels.BBSID);
                        if (oldBBS == null)
                        {
                            db.CTSMESHBBS.Add(bbsModels);
                        }
                        else
                        {
                            db.Entry(oldBBS).CurrentValues.SetValues(bbsModels);
                        }
                        db.SaveChanges();

                        return Json(true);
                    }
                }
            }
            catch (Exception ex)
            {
                lErrorMsg = ex.Message;
            }
            return View(bbsModels);
        }

        //ValidatePONumber
        [HttpPost]
        [Route("/ValidatePONumber_beam/{CustomerCode}/{ProjectCode}/{JobID}/{PONumber}")]
        // [ValidateAntiForgeryHeader]
        public ActionResult ValidatePONumber(string CustomerCode, string ProjectCode, int JobID, string PONumber)
        {
            var lErrorMsg = "";
            try
            {
                var lListOrder = "";
                var lJobAdvice = (from p in db.CTSMESHJobAdvice
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

                //return Json(new { success = true, responseText = lListOrder }, JsonRequestBehavior.AllowGet);
                return Ok();
            }
            catch (Exception ex)
            {
                lErrorMsg = ex.Message;
                // return Json(new { success = false, responseText = lErrorMsg }, JsonRequestBehavior.AllowGet);
                return Ok();
            }

        }

        // POST: Customer/deleteBBS/
        [HttpPost]
        [Route("/deleteBBSCust_beam/{CustomerCode}/{ProjectCode}/{JobID}/{BBSID}")]
        // [ValidateAntiForgeryHeader]
        public ActionResult deleteBBSCust(string CustomerCode, string ProjectCode, int JobID, int BBSID)
        {
            var lErrorMsg = "";
            try
            {
                var oldBBS = db.CTSMESHBBS.Find(CustomerCode, ProjectCode, JobID, BBSID);
                if (oldBBS != null)
                {
                    if (oldBBS.BBSProdCategory == "BEAM")
                    {
                        var oldOrderDetails = from p in db.CTSMESHBeamDetails
                                              where p.CustomerCode == CustomerCode &&
                                              p.ProjectCode == ProjectCode &&
                                              p.JobID == JobID &&
                                              p.BBSID == BBSID
                                              select p;

                        if (oldOrderDetails != null)
                        {
                            db.CTSMESHBeamDetails.RemoveRange(oldOrderDetails);
                        }
                    }

                    if (oldBBS.BBSProdCategory == "COLUMN")
                    {
                        var oldOrderDetails = from p in db.CTSMESHColumnDetails
                                              where p.CustomerCode == CustomerCode &&
                                              p.ProjectCode == ProjectCode &&
                                              p.JobID == JobID &&
                                              p.BBSID == BBSID
                                              select p;

                        if (oldOrderDetails != null)
                        {
                            db.CTSMESHColumnDetails.RemoveRange(oldOrderDetails);
                        }
                    }

                    if (oldBBS.BBSProdCategory == "STANDARD")
                    {
                        var oldOrderDetails = from p in db.CTSMESHStdSheetDetails
                                              where p.CustomerCode == CustomerCode &&
                                              p.ProjectCode == ProjectCode &&
                                              p.JobID == JobID &&
                                              p.BBSID == BBSID
                                              select p;

                        if (oldOrderDetails != null)
                        {
                            db.CTSMESHStdSheetDetails.RemoveRange(oldOrderDetails);
                        }
                    }

                    if (oldBBS.BBSProdCategory == "OTHERS")
                    {
                        var oldOrderDetails = from p in db.CTSMESHOthersDetails
                                              where p.CustomerCode == CustomerCode &&
                                              p.ProjectCode == ProjectCode &&
                                              p.JobID == JobID &&
                                              p.BBSID == BBSID
                                              select p;

                        if (oldOrderDetails != null)
                        {
                            db.CTSMESHOthersDetails.RemoveRange(oldOrderDetails);
                        }
                    }
                    db.CTSMESHBBS.Remove(oldBBS);
                    db.SaveChanges();
                }
                return Json(true);
            }
            catch (Exception ex)
            {
                lErrorMsg = ex.Message;
            }
            return Json(false);
        }

        // POST: Customer/deleteBBS/
        //[HttpPost]
        //[Route("/copyBBS_beam/{CustomerCode}/{ProjectCode}/{JobID}/{BBSID}")]
        ////[ValidateAntiForgeryHeader]
        //public ActionResult copyBBS(string SourceCustomerCode,
        //        string SourceProjectCode,
        //        int SourceOrderNo,
        //        int SourceBBSID,
        //        string DesCustomerCode,
        //        string DesProjectCode,
        //        int DesOrderNo,
        //        int DesBBSID,
        //        int DesCopyQty)
        //{
        //    var lMaxMESHID = 0;
        //    var lMaxMESHSort = 0;
        //    var lRemarks = "";

        //    var lJobSource = db.CTSMESHJobAdvice.Find(SourceCustomerCode, SourceProjectCode, SourceOrderNo);
        //    if (lJobSource != null)
        //    {
        //        if (DesCopyQty != 1)
        //        {
        //            lRemarks = DesCopyQty.ToString() + " x " + lJobSource.PONumber.Trim();
        //        }
        //        else
        //        {
        //            lRemarks = lJobSource.PONumber.Trim();
        //        }
        //    }
        //    //copy bars details;
        //    if (DesBBSID == 1)
        //    {
        //        lMaxMESHID = (from p in db.CTSMESHBeamDetails
        //                      where p.CustomerCode == DesCustomerCode &&
        //                      p.ProjectCode == DesProjectCode &&
        //                      p.JobID == DesOrderNo &&
        //                      p.BBSID == DesBBSID
        //                      select p.MeshID).DefaultIfEmpty(0).Max();

        //        lMaxMESHSort = (from p in db.CTSMESHBeamDetails
        //                        where p.CustomerCode == DesCustomerCode &&
        //                        p.ProjectCode == DesProjectCode &&
        //                        p.JobID == DesOrderNo &&
        //                        p.BBSID == DesBBSID
        //                        select p.MeshSort).DefaultIfEmpty(0).Max();

        //        var lBarsSource = (from p in db.CTSMESHBeamDetails
        //                           where p.CustomerCode == SourceCustomerCode &&
        //                           p.ProjectCode == SourceProjectCode &&
        //                           p.JobID == SourceOrderNo &&
        //                           p.BBSID == SourceBBSID
        //                           select p).ToList();

        //        lBarsSource = new List<CTSMESHBeamDetailsModels>
        //            (lBarsSource.Select(h => new CTSMESHBeamDetailsModels
        //            {
        //                CustomerCode = DesCustomerCode,
        //                ProjectCode = DesProjectCode,
        //                JobID = DesOrderNo,
        //                BBSID = DesBBSID,
        //                MeshID = lMaxMESHID + h.MeshID,
        //                MeshSort = lMaxMESHSort + h.MeshSort,
        //                MeshMark = h.MeshMark,
        //                MeshWidth = h.MeshWidth,
        //                MeshDepth = h.MeshWidth,
        //                MeshSlope = h.MeshSlope,
        //                MeshProduct = h.MeshProduct,
        //                MeshShapeCode = h.MeshShapeCode,
        //                MeshTotalLinks = h.MeshTotalLinks,
        //                MeshSpan = h.MeshSpan,
        //                MeshMemberQty = h.MeshMemberQty * DesCopyQty,
        //                MeshCapping = h.MeshCapping,
        //                MeshCPProduct = h.MeshCPProduct,
        //                A = h.A,
        //                B = h.B,
        //                C = h.C,
        //                D = h.D,
        //                E = h.E,
        //                P = h.P,
        //                Q = h.Q,
        //                HOOK = h.HOOK,
        //                LEG = h.LEG,
        //                MeshTotalWT = h.MeshTotalWT * DesCopyQty,
        //                Remarks = lRemarks,
        //                UpdateDate = DateTime.Now,
        //                UpdateBy = User.Identity.GetUserName()
        //            })).ToList();

        //        if (lBarsSource.Count() > 0)
        //        {
        //            foreach (CTSMESHBeamDetailsModels fNewItem in lBarsSource)
        //            {
        //                db.CTSMESHBeamDetails.Add(fNewItem);
        //            }
        //        }
        //        db.SaveChanges();
        //    }
        //    else if (DesBBSID == 2)
        //    {
        //        lMaxMESHID = (from p in db.CTSMESHColumnDetails
        //                      where p.CustomerCode == DesCustomerCode &&
        //                      p.ProjectCode == DesProjectCode &&
        //                      p.JobID == DesOrderNo &&
        //                      p.BBSID == DesBBSID
        //                      select p.MeshID).DefaultIfEmpty(0).Max();

        //        lMaxMESHSort = (from p in db.CTSMESHColumnDetails
        //                        where p.CustomerCode == DesCustomerCode &&
        //                        p.ProjectCode == DesProjectCode &&
        //                        p.JobID == DesOrderNo &&
        //                        p.BBSID == DesBBSID
        //                        select p.MeshSort).DefaultIfEmpty(0).Max();

        //        var lBarsSource = (from p in db.CTSMESHColumnDetails
        //                           where p.CustomerCode == SourceCustomerCode &&
        //                           p.ProjectCode == SourceProjectCode &&
        //                           p.JobID == SourceOrderNo &&
        //                           p.BBSID == SourceBBSID
        //                           select p).ToList();

        //        lBarsSource = new List<CTSMESHColumnDetailsModels>
        //            (lBarsSource.Select(h => new CTSMESHColumnDetailsModels
        //            {
        //                CustomerCode = DesCustomerCode,
        //                ProjectCode = DesProjectCode,
        //                JobID = DesOrderNo,
        //                BBSID = DesBBSID,
        //                MeshID = lMaxMESHID + h.MeshID,
        //                MeshSort = lMaxMESHSort + h.MeshSort,
        //                MeshMark = h.MeshMark,
        //                MeshWidth = h.MeshWidth,
        //                MeshLength = h.MeshLength,
        //                MeshProduct = h.MeshProduct,
        //                MeshShapeCode = h.MeshShapeCode,
        //                MeshTotalLinks = h.MeshTotalLinks,
        //                MeshHeight = h.MeshHeight,
        //                MeshMemberQty = h.MeshMemberQty * DesCopyQty,
        //                MeshCLinkRowsAtLen = h.MeshCLinkRowsAtLen,
        //                MeshCLinkProductAtLen = h.MeshCLinkProductAtLen,
        //                MeshCLinkRowsAtWidth = h.MeshCLinkRowsAtWidth,
        //                MeshCLinkProductAtWidth = h.MeshCLinkProductAtWidth,
        //                A = h.A,
        //                B = h.B,
        //                C = h.C,
        //                D = h.D,
        //                E = h.E,
        //                F = h.F,
        //                P = h.P,
        //                Q = h.Q,
        //                LEG = h.LEG,
        //                MeshTotalWT = h.MeshTotalWT * DesCopyQty,
        //                Remarks = lRemarks,
        //                UpdateDate = DateTime.Now,
        //                UpdateBy = User.Identity.GetUserName()
        //            })).ToList();

        //        if (lBarsSource.Count() > 0)
        //        {
        //            foreach (CTSMESHColumnDetailsModels fNewItem in lBarsSource)
        //            {
        //                db.CTSMESHColumnDetails.Add(fNewItem);
        //            }
        //        }
        //        db.SaveChanges();
        //    }
        //    else if (DesBBSID == 8)
        //    {
        //        lMaxMESHID = (from p in db.CTSMESHStdSheetDetails
        //                      where p.CustomerCode == DesCustomerCode &&
        //                      p.ProjectCode == DesProjectCode &&
        //                      p.JobID == DesOrderNo &&
        //                      p.BBSID == DesBBSID
        //                      select p.MeshID).DefaultIfEmpty(0).Max();

        //        lMaxMESHSort = (from p in db.CTSMESHStdSheetDetails
        //                        where p.CustomerCode == DesCustomerCode &&
        //                        p.ProjectCode == DesProjectCode &&
        //                        p.JobID == DesOrderNo &&
        //                        p.BBSID == DesBBSID
        //                        select p.MeshSort).DefaultIfEmpty(0).Max();

        //        var lBarsSource = (from p in db.CTSMESHStdSheetDetails
        //                           where p.CustomerCode == SourceCustomerCode &&
        //                           p.ProjectCode == SourceProjectCode &&
        //                           p.JobID == SourceOrderNo &&
        //                           p.BBSID == SourceBBSID
        //                           select p).ToList();

        //        lBarsSource = new List<CTSMESHStdSheetDetailsModels>
        //            (lBarsSource.Select(h => new CTSMESHStdSheetDetailsModels
        //            {
        //                CustomerCode = DesCustomerCode,
        //                ProjectCode = DesProjectCode,
        //                JobID = DesOrderNo,
        //                BBSID = DesBBSID,
        //                MeshID = lMaxMESHID + h.MeshID,
        //                MeshSort = lMaxMESHSort + h.MeshSort,
        //                std_type = h.std_type,
        //                mesh_series = h.mesh_series,
        //                sheet_name = h.sheet_name,
        //                mw_length = h.mw_length,
        //                mw_size = h.mw_size,
        //                mw_spacing = h.mw_spacing,
        //                mo1 = h.mo1,
        //                mo2 = h.mo2,
        //                cw_length = h.cw_length,
        //                cw_size = h.cw_size,
        //                cw_spacing = h.cw_spacing,
        //                co1 = h.co1,
        //                co2 = h.co2,
        //                unit_weight = h.unit_weight,
        //                order_pcs = h.order_pcs * DesCopyQty,
        //                order_wt = h.order_wt * DesCopyQty,
        //                sap_mcode = h.sap_mcode,
        //                UpdateDate = DateTime.Now,
        //                UpdateBy = User.Identity.GetUserName()
        //            })).ToList();

        //        if (lBarsSource.Count() > 0)
        //        {
        //            foreach (CTSMESHStdSheetDetailsModels fNewItem in lBarsSource)
        //            {
        //                db.CTSMESHStdSheetDetails.Add(fNewItem);
        //            }
        //        }
        //        db.SaveChanges();
        //    }
        //    else
        //    {
        //        lMaxMESHID = (from p in db.CTSMESHOthersDetails
        //                      where p.CustomerCode == DesCustomerCode &&
        //                      p.ProjectCode == DesProjectCode &&
        //                      p.JobID == DesOrderNo &&
        //                      p.BBSID == DesBBSID
        //                      select p.MeshID).DefaultIfEmpty(0).Max();

        //        lMaxMESHSort = (from p in db.CTSMESHOthersDetails
        //                        where p.CustomerCode == DesCustomerCode &&
        //                        p.ProjectCode == DesProjectCode &&
        //                        p.JobID == DesOrderNo &&
        //                        p.BBSID == DesBBSID
        //                        select p.MeshSort).DefaultIfEmpty(0).Max();

        //        var lBarsSource = (from p in db.CTSMESHOthersDetails
        //                           where p.CustomerCode == SourceCustomerCode &&
        //                           p.ProjectCode == SourceProjectCode &&
        //                           p.JobID == SourceOrderNo &&
        //                           p.BBSID == SourceBBSID
        //                           select p).ToList();

        //        lBarsSource = new List<CTSMESHOthersDetailsModels>
        //            (lBarsSource.Select(h => new CTSMESHOthersDetailsModels
        //            {
        //                CustomerCode = DesCustomerCode,
        //                ProjectCode = DesProjectCode,
        //                JobID = DesOrderNo,
        //                BBSID = DesBBSID,
        //                MeshID = lMaxMESHID + h.MeshID,
        //                MeshSort = lMaxMESHSort + h.MeshSort,
        //                MeshMark = h.MeshMark,
        //                MeshProduct = h.MeshProduct,
        //                MeshMainLen = h.MeshMainLen,
        //                MeshCrossLen = h.MeshCrossLen,
        //                MeshMO1 = h.MeshMO1,
        //                MeshMO2 = h.MeshMO2,
        //                MeshCO1 = h.MeshCO1,
        //                MeshCO2 = h.MeshCO2,
        //                MeshMemberQty = h.MeshMemberQty * DesCopyQty,
        //                MeshShapeCode = h.MeshShapeCode,
        //                A = h.A,
        //                B = h.B,
        //                C = h.C,
        //                D = h.D,
        //                E = h.E,
        //                F = h.F,
        //                G = h.G,
        //                H = h.H,
        //                I = h.I,
        //                J = h.J,
        //                K = h.K,
        //                L = h.L,
        //                M = h.M,
        //                N = h.N,
        //                O = h.O,
        //                P = h.P,
        //                Q = h.Q,
        //                R = h.R,
        //                S = h.S,
        //                T = h.T,
        //                U = h.U,
        //                V = h.V,
        //                W = h.W,
        //                X = h.X,
        //                Y = h.Y,
        //                Z = h.Z,
        //                HOOK = h.HOOK,
        //                MeshTotalWT = h.MeshTotalWT * DesCopyQty,
        //                Remarks = lRemarks,
        //                UpdateDate = DateTime.Now,
        //                UpdateBy = User.Identity.GetUserName()
        //            })).ToList();

        //        if (lBarsSource.Count() > 0)
        //        {
        //            foreach (CTSMESHOthersDetailsModels fNewItem in lBarsSource)
        //            {
        //                db.CTSMESHOthersDetails.Add(fNewItem);
        //            }
        //        }
        //        db.SaveChanges();
        //    }
        //    return Json(true);
        //}

        //get Beam Stirrup Cage List 
        [HttpGet]
        [Route("/getBeamDetailsNSH_beam/{CustomerCode}/{ProjectCode}/{PostID}")]
        //  [ValidateAntiForgeryHeader]
        public ActionResult getBeamDetailsNSH(string CustomerCode, string ProjectCode, int PostID)
        {
            var lBeamDetails = new List<CTSShapeBeamDetailsModels>();
            var lNewBeam = new CTSShapeBeamDetailsModels();

            var lCmd = new SqlCommand();
            var lNDSCon = new SqlConnection();
            SqlDataReader lRst;
            int lID = 0;
            string lPar = "";

            if (PostID > 0)
            {
                lCmd.CommandText =
                "SELECT S.vchStructureMarkingName as cage_mark, " +
                "P.numCageWidth, " +
                "P.numCageDepth, " +
                "P.numCageSlope, " +
                "M.vchProductCode, " +
                "(select chrShapeCode " +
                "FROM dbo.ShapeMaster " +
                "WHERE intShapeID = P.intShapeCodeId " +
                ") as ShapeCode, " +
                "P.intInvoiceMWQty, " +
                "P.numInvoiceCWLength, " +
                "P.intMemberQty * S.intMemberQty * G.tntGroupQty  as TotalQty, " +
                "S.bitIsCapping, " +
                "(select isNull(vchProductCode, '') " +
                "FROM dbo.ProductCodeMaster " +
                "WHERE intProductCodeId = S.intCappingProductCodeId " +
                ") as CappingProductCode, " +
                "P.ParamValues, " +
                "P.numTheoraticalWeight * P.intMemberQty * S.intMemberQty * G.tntGroupQty as InvoiceWeight, " +
                "(STUFF((SELECT CAST(',' + rtrim(chrParamName) AS VARCHAR(MAX)) " +
                "FROM dbo.ShapeParamDetails " +
                "WHERE(intShapeID = P.intShapeCodeId) " +
                "AND (bitEdit = 1 or bitVisible = 1) " +
                "FOR XML PATH('')), 1, 1, '')) AS ShapeParameters, " +
                "M.decMWDiameter, " +
                "M.intMWSpace, " +
                "M.decCWDiameter, " +
                "M.intCWSpace, " +
                "M.decWeightArea, " +
                "M.chrTwinIndicator " +
                "from dbo.StructureMarkingDetails S, " +
                "dbo.ProductMarkingDetails P, " +
                "dbo.SELevelDetails D, " +
                "dbo.PostGroupMarkingDetails G, " +
                "dbo.ProductCodeMaster M " +
                "WHERE S.intStructureMarkId = P.intStructureMarkId " +
                "AND S.intSEDetailingId = D.intSEDetailingId " +
                "AND D.intGroupMarkId = G.intGroupMarkId " +
                "AND M.intProductCodeId = P.intProductCode " +
                "AND G.intPostHeaderId = " + PostID + " " +
                "ORDER BY case when (PATINDEX('%[0-9]-%', S.vchStructureMarkingName + '1')) > 0 then left(S.vchStructureMarkingName,(PATINDEX('%[0-9]-%', S.vchStructureMarkingName + '1')-1)) " +
                "else case when (PATINDEX('%-%', S.vchStructureMarkingName + '1')) > 0 then left(S.vchStructureMarkingName,(PATINDEX('%-%', S.vchStructureMarkingName + '1')-1)) else '' end " +
                "end, " +
                " " +
                "case when PATINDEX('%-%',S.vchStructureMarkingName) <= PATINDEX('%[0-9]-%',S.vchStructureMarkingName) or PATINDEX('%[0-9]-%',S.vchStructureMarkingName) = 0 then 0 " +
                "else cast (substring(S.vchStructureMarkingName,(PATINDEX('%[0-9]-%',S.vchStructureMarkingName)), " +
                "(PATINDEX('%-%',S.vchStructureMarkingName) - PATINDEX('%[0-9]-%',S.vchStructureMarkingName))) as int) end,  " +
                " " +
                "CASE WHEN PATINDEX('%-%',S.vchStructureMarkingName) > 0 THEN cast (left(substring(S.vchStructureMarkingName,(PATINDEX('%-%',S.vchStructureMarkingName) + 1),len(S.vchStructureMarkingName)),  " +
                "PATINDEX('%[^0-9]%', substring(S.vchStructureMarkingName,(PATINDEX('%-%',S.vchStructureMarkingName) + 1),  " +
                "len(S.vchStructureMarkingName)) + 'z')-1) as int) " +
                "ELSE  " +
                "cast (left(substring(S.vchStructureMarkingName,(PATINDEX('%[0-9]%',S.vchStructureMarkingName)),len(S.vchStructureMarkingName)),  " +
                "PATINDEX('%[^0-9]%', substring(S.vchStructureMarkingName,(PATINDEX('%[0-9]%',S.vchStructureMarkingName)),  " +
                "len(S.vchStructureMarkingName)) + 'z')-1) as int) " +
                "END, S.vchStructureMarkingName ";


                var lProcessObj = new ProcessController();
                if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
                {
                    lCmd.Connection = lNDSCon;
                    lCmd.CommandTimeout = 300;
                    lRst = lCmd.ExecuteReader();
                    if (lRst.HasRows)
                    {
                        lID = 1;
                        while (lRst.Read())
                        {
                            lPar = lRst.GetValue(11) == DBNull.Value ? "" : lRst.GetString(11).Trim();
                            lNewBeam = new CTSShapeBeamDetailsModels
                            {
                                CustomerCode = CustomerCode,
                                ProjectCode = ProjectCode,
                                JobID = 0,
                                BBSID = 1,
                                MeshID = lID,
                                MeshSort = lID * 100,
                                MeshMark = lRst.GetString(0).Trim(),
                                MeshWidth = (int)lRst.GetDecimal(1),
                                MeshDepth = (int)lRst.GetDecimal(2),
                                MeshSlope = (int)lRst.GetDecimal(3),
                                MeshProduct = lRst.GetString(4).Trim(),
                                MeshShapeCode = lRst.GetString(5).Trim(),
                                MeshTotalLinks = lRst.GetInt32(6),
                                MeshSpan = (int)lRst.GetInt32(7),
                                MeshMemberQty = lRst.GetInt32(8),
                                MeshCapping = lRst.GetValue(9) == DBNull.Value ? false : lRst.GetBoolean(9),
                                MeshCPProduct = lRst.GetValue(10) == DBNull.Value ? "" : lRst.GetString(10).Trim(),
                                A = getParFrString(lPar, "A"),
                                B = getParFrString(lPar, "B"),
                                C = getParFrString(lPar, "C"),
                                D = getParFrString(lPar, "D"),
                                P = getParFrString(lPar, "P"),
                                Q = getParFrString(lPar, "Q"),
                                HOOK = getParFrString(lPar, "HOOK"),
                                LEG = getParFrString(lPar, "LEG"),
                                MeshTotalWT = lRst.GetDecimal(12),
                                MeshShapeParameters = lRst.GetString(13).Trim().ToUpper(),
                                ProdMWDia = lRst.GetDecimal(14),
                                ProdMWSpacing = lRst.GetInt32(15),
                                ProdCWDia = lRst.GetDecimal(16),
                                ProdCWSpacing = lRst.GetInt32(17),
                                ProdMass = lRst.GetDecimal(18),
                                ProdTwinInd = lRst.GetString(19).Trim()
                            };
                            lBeamDetails.Add(lNewBeam);
                            lID = lID + 1;
                        }
                    }
                    lRst.Close();

                    int lLinkPostID = 0;

                    lCmd.CommandText =
                    "SELECT intPostHeaderId FROM dbo.BBSPostHeader WHERE intRefPostheaderid = " + PostID + " ";

                    lCmd.Connection = lNDSCon;
                    lCmd.CommandTimeout = 300;
                    lRst = lCmd.ExecuteReader();
                    if (lRst.HasRows)
                    {
                        lID = 1;
                        if (lRst.Read())
                        {
                            lLinkPostID = lRst.GetValue(0) == DBNull.Value ? 0 : lRst.GetInt32(0);
                        }
                    }
                    lRst.Close();


                    lCmd.CommandText =
                    "SELECT S.vchStructureMarkingName as cage_mark, " +
                    "P.numCageWidth, " +
                    "P.numCageDepth, " +
                    "P.numCageSlope, " +
                    "M.vchProductCode, " +
                    "(select chrShapeCode " +
                    "FROM dbo.ShapeMaster " +
                    "WHERE intShapeID = P.intShapeCodeId " +
                    ") as ShapeCode, " +
                    "P.intInvoiceMWQty, " +
                    "P.numInvoiceCWLength, " +
                    "P.intMemberQty * G.tntGroupQty  as TotalQty, " +
                    "S.bitIsCapping, " +
                    "(select isNull(vchProductCode, '') " +
                    "FROM dbo.ProductCodeMaster " +
                    "WHERE intProductCodeId = S.intCappingProductCodeId " +
                    ") as CappingProductCode, " +
                    "P.ParamValues, " +
                    "P.numTheoraticalWeight * P.intMemberQty * G.tntGroupQty as InvoiceWeight, " +
                    "(STUFF((SELECT CAST(',' + rtrim(chrParamName) AS VARCHAR(MAX)) " +
                    "FROM dbo.ShapeParamDetails " +
                    "WHERE(intShapeID = P.intShapeCodeId) " +
                    "AND (bitEdit = 1 or bitVisible = 1) " +
                    "FOR XML PATH('')), 1, 1, '')) AS ShapeParameters, " +
                    "M.decMWDiameter, " +
                    "M.intMWSpace, " +
                    "M.decCWDiameter, " +
                    "M.intCWSpace, " +
                    "M.decWeightArea, " +
                    "M.chrTwinIndicator " +
                    "from dbo.StructureMarkingDetails S, " +
                    "dbo.ProductMarkingDetails P, " +
                    "dbo.SELevelDetails D, " +
                    "dbo.PostGroupMarkingDetails G, " +
                    "dbo.ProductCodeMaster M " +
                    "WHERE S.intStructureMarkId = P.intStructureMarkId " +
                    "AND S.intSEDetailingId = D.intSEDetailingId " +
                    "AND D.intGroupMarkId = G.intGroupMarkId " +
                    "AND M.intProductCodeId = P.intProductCode " +
                    "AND G.intPostHeaderId = " + lLinkPostID + " " +
                    "ORDER BY case when (PATINDEX('%[0-9]-%', S.vchStructureMarkingName + '1')) > 0 then left(S.vchStructureMarkingName,(PATINDEX('%[0-9]-%', S.vchStructureMarkingName + '1')-1)) " +
                    "else case when (PATINDEX('%-%', S.vchStructureMarkingName + '1')) > 0 then left(S.vchStructureMarkingName,(PATINDEX('%-%', S.vchStructureMarkingName + '1')-1)) else '' end " +
                    "end, " +
                    " " +
                    "case when PATINDEX('%-%',S.vchStructureMarkingName) <= PATINDEX('%[0-9]-%',S.vchStructureMarkingName) or PATINDEX('%[0-9]-%',S.vchStructureMarkingName) = 0 then 0 " +
                    "else cast (substring(S.vchStructureMarkingName,(PATINDEX('%[0-9]-%',S.vchStructureMarkingName)), " +
                    "(PATINDEX('%-%',S.vchStructureMarkingName) - PATINDEX('%[0-9]-%',S.vchStructureMarkingName))) as int) end,  " +
                    " " +
                    "CASE WHEN PATINDEX('%-%',S.vchStructureMarkingName) > 0 THEN cast (left(substring(S.vchStructureMarkingName,(PATINDEX('%-%',S.vchStructureMarkingName) + 1),len(S.vchStructureMarkingName)),  " +
                    "PATINDEX('%[^0-9]%', substring(S.vchStructureMarkingName,(PATINDEX('%-%',S.vchStructureMarkingName) + 1),  " +
                    "len(S.vchStructureMarkingName)) + 'z')-1) as int) " +
                    "ELSE  " +
                    "cast (left(substring(S.vchStructureMarkingName,(PATINDEX('%[0-9]%',S.vchStructureMarkingName)),len(S.vchStructureMarkingName)),  " +
                    "PATINDEX('%[^0-9]%', substring(S.vchStructureMarkingName,(PATINDEX('%[0-9]%',S.vchStructureMarkingName)),  " +
                    "len(S.vchStructureMarkingName)) + 'z')-1) as int) " +
                    "END, S.vchStructureMarkingName ";

                    lCmd.Connection = lNDSCon;
                    lCmd.CommandTimeout = 300;
                    lRst = lCmd.ExecuteReader();
                    if (lRst.HasRows)
                    {
                        lID = 1;
                        while (lRst.Read())
                        {
                            lPar = lRst.GetValue(11) == DBNull.Value ? "" : lRst.GetString(11).Trim();
                            lNewBeam = new CTSShapeBeamDetailsModels
                            {
                                CustomerCode = CustomerCode,
                                ProjectCode = ProjectCode,
                                JobID = 0,
                                BBSID = 1,
                                MeshID = lID,
                                MeshSort = lID * 100,
                                MeshMark = lRst.GetString(0).Trim(),
                                MeshWidth = (int)lRst.GetDecimal(1),
                                MeshDepth = (int)lRst.GetDecimal(2),
                                MeshSlope = (int)lRst.GetDecimal(3),
                                MeshProduct = lRst.GetString(4).Trim(),
                                MeshShapeCode = lRst.GetString(5).Trim(),
                                MeshTotalLinks = lRst.GetInt32(6),
                                MeshSpan = (int)lRst.GetInt32(7),
                                MeshMemberQty = lRst.GetInt32(8),
                                MeshCapping = lRst.GetValue(9) == DBNull.Value ? false : lRst.GetBoolean(9),
                                MeshCPProduct = lRst.GetValue(10) == DBNull.Value ? "" : lRst.GetString(10).Trim(),
                                A = getParFrString(lPar, "A"),
                                B = getParFrString(lPar, "B"),
                                C = getParFrString(lPar, "C"),
                                D = getParFrString(lPar, "D"),
                                P = getParFrString(lPar, "P"),
                                Q = getParFrString(lPar, "Q"),
                                HOOK = getParFrString(lPar, "HOOK"),
                                LEG = getParFrString(lPar, "LEG"),
                                MeshTotalWT = lRst.GetDecimal(12),
                                MeshShapeParameters = lRst.GetString(13).Trim().ToUpper(),
                                ProdMWDia = lRst.GetDecimal(14),
                                ProdMWSpacing = lRst.GetInt32(15),
                                ProdCWDia = lRst.GetDecimal(16),
                                ProdCWSpacing = lRst.GetInt32(17),
                                ProdMass = lRst.GetDecimal(18),
                                ProdTwinInd = lRst.GetString(19).Trim()
                            };
                            lBeamDetails.Add(lNewBeam);
                            lID = lID + 1;
                        }
                    }
                    lRst.Close();

                    lProcessObj.CloseNDSConnection(ref lNDSCon);
                }

                lProcessObj = null;
            }

            //  return Json(lBeamDetails, JsonRequestBehavior.AllowGet);
            return Ok(lBeamDetails);
        }

        //get Column Link Cage List 
        [HttpPost]
        [Route("/getColumnDetailsNSH_beam/{CustomerCode}/{ProjectCode}/{JobID}/{BBSID}/{BBSPostID}")]
        //    [ValidateAntiForgeryHeader]
        public ActionResult getColumnDetailsNSH(string CustomerCode, string ProjectCode, int JobID, int BBSID, int BBSPostID)
        {
            var lBeamDetails = new List<CTSShapeColumnDetailsModels>();
            var lNewBeam = new CTSShapeColumnDetailsModels();

            var lCmd = new SqlCommand();
            var lNDSCon = new SqlConnection();
            SqlDataReader lRst;
            int lID = 0;
            string lPar = "";

            if (BBSPostID > 0)
            {
                lCmd.CommandText =
                "SELECT S.vchStructureMarkingName, " +
                "P.intLinkWidth, " +
                "P.intLinkLength, " +
                "M.vchProductCode, " +
                "(select chrShapeCode " +
                "FROM dbo.ShapeMaster " +
                "WHERE intShapeID = P.intShapeCodeId " +
                ") as ShapeCode, " +
                "P.intNoofLinks, " +
                "P.numInvoiceCWLength, " +
                "P.intLinkQty * S.intMemberQty * tntGroupQty as TotalQty, " +
                "S.intRowsAtLength, " +
                "(select isNull(vchProductCode, '') " +
                "FROM dbo.ProductCodeMaster " +
                "WHERE intProductCodeId = S.intClinkProductCodeIdAtLength " +
                ") as CLinkProductCodeAtLength, " +
                "S.intRowsAtWidth, " +
                "(select isNull(vchProductCode, '') " +
                "FROM dbo.ProductCodeMaster " +
                "WHERE intProductCodeId = S.intClinkProductCodeIdAtWidth " +
                ") as CLinkProductCodeAtLength, " +
                "P.ParamValues, " +
                "P.numTheoraticalWeight * P.intLinkQty * S.intMemberQty * tntGroupQty as InvoiceWeight, " +
                "(STUFF((SELECT CAST(',' + rtrim(chrParamName) AS VARCHAR(MAX)) " +
                "FROM dbo.ShapeParamDetails " +
                "WHERE(intShapeID = P.intShapeCodeId) " +
                "AND (bitEdit = 1 or bitVisible = 1) " +
                "FOR XML PATH('')), 1, 1, '')) AS ShapeParameters, " +
                "M.decMWDiameter, " +
                "M.intMWSpace, " +
                "M.decCWDiameter, " +
                "M.intCWSpace, " +
                "M.decWeightArea, " +
                "M.chrTwinIndicator " +
                "from dbo.ColumnStructureMarkingDetails S, " +
                "dbo.ColumnProductMarkingDetails P, " +
                "dbo.SELevelDetails D, " +
                "dbo.PostGroupMarkingDetails G, " +
                "dbo.ProductCodeMaster M " +
                "WHERE S.intStructureMarkId = P.intStructureMarkId " +
                "AND S.intSEDetailingId = D.intSEDetailingId " +
                "AND D.intGroupMarkId = G.intGroupMarkId " +
                "AND M.intProductCodeId = P.intProductCodeId " +
                "AND G.intPostHeaderId = " + BBSPostID + " ";

                var lProcessObj = new ProcessController();
                if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
                {
                    lCmd.Connection = lNDSCon;
                    lCmd.CommandTimeout = 300;
                    lRst = lCmd.ExecuteReader();
                    if (lRst.HasRows)
                    {
                        lID = 1;
                        while (lRst.Read())
                        {
                            lPar = lRst.GetValue(12) == DBNull.Value ? "" : lRst.GetString(12).Trim();
                            lNewBeam = new CTSShapeColumnDetailsModels
                            {
                                CustomerCode = CustomerCode,
                                ProjectCode = ProjectCode,
                                JobID = JobID,
                                BBSID = BBSID,
                                MeshID = lID,
                                MeshSort = lID * 100,
                                MeshMark = lRst.GetString(0).Trim(),
                                MeshWidth = lRst.GetInt32(1),
                                MeshLength = lRst.GetInt32(2),
                                MeshProduct = lRst.GetString(3).Trim(),
                                MeshShapeCode = lRst.GetString(4).Trim(),
                                MeshTotalLinks = lRst.GetInt32(5),
                                MeshHeight = (int)lRst.GetDecimal(6),
                                MeshMemberQty = lRst.GetInt32(7),
                                MeshCLinkRowsAtLen = lRst.GetValue(8) == DBNull.Value ? 0 : lRst.GetInt32(8),
                                MeshCLinkProductAtLen = lRst.GetValue(8) == DBNull.Value ? "" : (lRst.GetInt32(8) > 0 ? lRst.GetString(9) : ""),
                                MeshCLinkRowsAtWidth = lRst.GetValue(10) == DBNull.Value ? 0 : lRst.GetInt32(10),
                                MeshCLinkProductAtWidth = lRst.GetValue(10) == DBNull.Value ? "" : (lRst.GetInt32(10) > 0 ? lRst.GetString(11) : ""),
                                A = getParFrString(lPar, "A"),
                                B = getParFrString(lPar, "B"),
                                C = getParFrString(lPar, "C"),
                                D = getParFrString(lPar, "D"),
                                E = getParFrString(lPar, "E"),
                                F = getParFrString(lPar, "F"),
                                P = getParFrString(lPar, "P"),
                                Q = getParFrString(lPar, "Q"),
                                LEG = getParFrString(lPar, "LEG"),
                                MeshTotalWT = lRst.GetDecimal(13),
                                MeshShapeParameters = lRst.GetString(14).ToUpper(),
                                ProdMWDia = lRst.GetDecimal(15),
                                ProdMWSpacing = lRst.GetInt32(16),
                                ProdCWDia = lRst.GetDecimal(17),
                                ProdCWSpacing = lRst.GetInt32(18),
                                ProdMass = lRst.GetDecimal(19),
                                ProdTwinInd = lRst.GetString(20).Trim()
                            };
                            lBeamDetails.Add(lNewBeam);
                            lID = lID + 1;
                        }
                    }
                    lRst.Close();

                    lProcessObj.CloseNDSConnection(ref lNDSCon);
                }

                lProcessObj = null;
            }

            // return Json(lBeamDetails, JsonRequestBehavior.AllowGet);
            return Ok(lBeamDetails);
        }

        //get Column Link Cage List 
        [HttpPost]
        [Route("/getOthersDetailsNSH_beam/{CustomerCode}/{ProjectCode}/{JobID}/{BBSID}/{BBSPostID}")]
        //[ValidateAntiForgeryHeader]
        public ActionResult getOthersDetailsNSH(string CustomerCode, string ProjectCode, int JobID, int BBSID, int BBSPostID)
        {
            var lBeamDetails = new List<CTSShapeOthersDetailsModels>();
            var lNewBeam = new CTSShapeOthersDetailsModels();

            var lCmd = new SqlCommand();
            var lNDSCon = new SqlConnection();
            SqlDataReader lRst;
            int lID = 0;
            string lPar = "";

            if (BBSPostID > 0)
            {
                lCmd.CommandText =
                "SELECT S.vchStructureMarkingName, " +
                "P.numInvoiceMWLength, " +
                "P.numInvoiceCWLength, " +
                "M.vchProductCode, " +
                "(select chrShapeCode " +
                "FROM dbo.ShapeMaster " +
                "WHERE intShapeID = P.intShapeCodeId " +
                ") as ShapeCode, " +
                "P.intMemberQty * S.intMemberQty * tntGroupQty as TotalQty, " +
                "P.intInvoiceMO1, " +
                "P.intInvoiceMO2, " +
                "P.intInvoiceCO1, " +
                "P.intInvoiceCO2, " +
                "P.ParamValues, " +
                "P.numTheoraticalWeight * P.intMemberQty * S.intMemberQty * tntGroupQty as InvoiceWeight, " +
                "(STUFF((SELECT CAST(',' + rtrim(chrParamName) AS VARCHAR(MAX)) " +
                "FROM dbo.ShapeParamDetails " +
                "WHERE(intShapeID = P.intShapeCodeId) " +
                "AND (bitEdit = 1 or bitVisible = 1) " +
                "FOR XML PATH('')), 1, 1, '')) AS ShapeParameters, " +
                "M.decMWDiameter, " +
                "M.intMWSpace, " +
                "M.decCWDiameter, " +
                "M.intCWSpace, " +
                "M.decWeightArea, " +
                "M.chrTwinIndicator " +
                "FROM dbo.MeshStructureMarkingDetails S, " +
                "dbo.MeshProductMarkingDetails P, " +
                "dbo.SELevelDetails D, " +
                "dbo.PostGroupMarkingDetails G, " +
                "dbo.ProductCodeMaster M " +
                "WHERE S.intStructureMarkId = P.intStructureMarkId " +
                "AND S.intSEDetailingId = D.intSEDetailingId " +
                "AND D.intGroupMarkId = G.intGroupMarkId " +
                "AND M.intProductCodeId = P.intProductCodeId " +
                "AND G.intPostHeaderId = " + BBSPostID + " ";

                var lProcessObj = new ProcessController();
                if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
                {
                    lCmd.Connection = lNDSCon;
                    lCmd.CommandTimeout = 300;
                    lRst = lCmd.ExecuteReader();
                    if (lRst.HasRows)
                    {
                        lID = 1;
                        while (lRst.Read())
                        {
                            lPar = lRst.GetValue(10) == DBNull.Value ? "" : lRst.GetString(10).Trim();
                            lNewBeam = new CTSShapeOthersDetailsModels
                            {
                                CustomerCode = CustomerCode,
                                ProjectCode = ProjectCode,
                                JobID = JobID,
                                BBSID = BBSID,
                                MeshID = lID,
                                MeshSort = lID * 100,
                                MeshMark = lRst.GetString(0).Trim(),
                                MeshMainLen = (int)Math.Round(lRst.GetDecimal(1), 0),
                                MeshCrossLen = (int)Math.Round(lRst.GetDecimal(2), 0),
                                MeshProduct = lRst.GetString(3).Trim(),
                                MeshShapeCode = lRst.GetString(4).Trim(),
                                MeshMemberQty = lRst.GetInt32(5),
                                MeshMO1 = lRst.GetInt32(6),
                                MeshMO2 = lRst.GetInt32(7),
                                MeshCO1 = lRst.GetInt32(8),
                                MeshCO2 = lRst.GetInt32(9),
                                A = getParFrString(lPar, "A"),
                                B = getParFrString(lPar, "B"),
                                C = getParFrString(lPar, "C"),
                                D = getParFrString(lPar, "D"),
                                E = getParFrString(lPar, "E"),
                                F = getParFrString(lPar, "F"),
                                G = getParFrString(lPar, "G"),
                                H = getParFrString(lPar, "H"),
                                I = getParFrString(lPar, "I"),
                                J = getParFrString(lPar, "J"),
                                K = getParFrString(lPar, "K"),
                                L = getParFrString(lPar, "L"),
                                M = getParFrString(lPar, "M"),
                                N = getParFrString(lPar, "N"),
                                O = getParFrString(lPar, "O"),
                                P = getParFrString(lPar, "P"),
                                Q = getParFrString(lPar, "Q"),
                                R = getParFrString(lPar, "R"),
                                S = getParFrString(lPar, "S"),
                                T = getParFrString(lPar, "T"),
                                U = getParFrString(lPar, "U"),
                                V = getParFrString(lPar, "V"),
                                W = getParFrString(lPar, "W"),
                                X = getParFrString(lPar, "X"),
                                Y = getParFrString(lPar, "Y"),
                                Z = getParFrString(lPar, "Z"),
                                HOOK = getParFrString(lPar, "HOOK"),
                                MeshTotalWT = lRst.GetDecimal(11),
                                MeshShapeParameters = lRst.GetString(12).ToUpper(),

                                ProdMWDia = lRst.GetDecimal(13),
                                ProdMWSpacing = lRst.GetInt32(14),
                                ProdCWDia = lRst.GetDecimal(15),
                                ProdCWSpacing = lRst.GetInt32(16),
                                ProdMass = lRst.GetDecimal(17),
                                ProdTwinInd = lRst.GetString(18).Trim()
                            };
                            lBeamDetails.Add(lNewBeam);
                            lID = lID + 1;
                        }
                    }
                    lRst.Close();

                    lProcessObj.CloseNDSConnection(ref lNDSCon);
                }

                lProcessObj = null;
            }

            // return Json(lBeamDetails, JsonRequestBehavior.AllowGet);
            return Ok(lBeamDetails);
        }

        // Search parameter value from string like "A:810;B:240;C810;Leg:110;Hook:60"
        [HttpGet]
        [Route("/getParFrString_beam/{pParameter}/{pLetter}")]
        private int? getParFrString(string pParameter, string pLetter)
        {
            int? lReturn = null;
            if (pParameter != null)
            {
                if (pParameter.Trim().Length > 0)
                {
                    var lArray = pParameter.ToUpper().Split(';');
                    if (lArray.Length > 0)
                    {
                        for (int i = 0; i < lArray.Length; i++)
                        {
                            if (lArray[i].IndexOf(":") > 0)
                            {
                                if (lArray[i].Substring(0, lArray[i].IndexOf(":")) == pLetter)
                                {
                                    string lVar = lArray[i].Substring(lArray[i].IndexOf(":") + 1);
                                    int liVar = 0;
                                    int.TryParse(lVar, out liVar);
                                    lReturn = liVar;
                                }
                            }
                        }
                    }
                }
            }
            return lReturn;
        }

        //get Bars List 
        //getOthersDetailsNSH
        [HttpGet]
        [Route("/getMeshBeamDetails_beam/{CustomerCode}/{ProjectCode}/{JobID}/{BBSID}")]
        // [ValidateAntiForgeryHeader]
        public ActionResult getMeshBeamDetails(string CustomerCode, string ProjectCode, int JobID, int BBSID)
        {
            var lBeamDetails = new List<CTSShapeBeamDetailsModels>();
            var lNewBeam = new CTSShapeBeamDetailsModels();

            var lCmd = new SqlCommand();
            var lNDSCon = new SqlConnection();
            SqlDataReader lRst;

            if (BBSID > 0)
            {
                lCmd.CommandText =
                "SELECT CustomerCode " +
                ",ProjectCode " +
                ",JobID " +
                ",BBSID " +
                ",MeshID " +
                ",MeshSort " +
                ",MeshMark " +
                ",MeshWidth " +
                ",MeshDepth " +
                ",MeshSlope " +
                ",MeshProduct " +
                ",MeshShapeCode " +
                ",MeshTotalLinks " +
                ",MeshSpan " +
                ",MeshMemberQty " +
                ",MeshCapping " +
                ",MeshCPProduct " +
                ",A " +
                ",B " +
                ",C " +
                ",D " +
                ",E " +
                ",P " +
                ",Q " +
                ",Hook " +
                ",Leg " +
                ",MeshTotalWT " +
                ",Remarks " +
                ",(STUFF((SELECT CAST(',' + rtrim(chrParamName) AS VARCHAR(MAX)) " +
                "FROM dbo.ShapeParamDetails P, dbo.ShapeMaster M " +
                "WHERE (P.intShapeID = M.intShapeID " +
                "AND chrShapeCode = D.MeshShapeCode) " +
                "AND (P.bitEdit = 1 OR bitVisible = 1) " +
                "ORDER BY UPPER(chrParamName) " +
                "FOR XML PATH('')), 1, 1, '')) AS ShapeParameters " +
                ",(STUFF((SELECT CAST(',' + rtrim(chrParamName) AS VARCHAR(MAX)) " +
                "FROM dbo.ShapeParamDetails P, dbo.ShapeMaster M " +
                "WHERE (P.intShapeID = M.intShapeID " +
                "AND chrShapeCode = D.MeshShapeCode) " +
                "AND P.bitEdit = 1 " +
                "ORDER BY UPPER(chrParamName) " +
                "FOR XML PATH('')), 1, 1, '')) AS EditParameters " +
                ",(STUFF((SELECT CAST(',' + rtrim(chrAngleType) AS VARCHAR(MAX)) " +
                "FROM dbo.ShapeParamDetails P, dbo.ShapeMaster M " +
                "WHERE (P.intShapeID = M.intShapeID " +
                "AND chrShapeCode = D.MeshShapeCode) " +
                "AND P.bitEdit = 1 " +
                "ORDER BY UPPER(chrParamName) " +
                "FOR XML PATH('')), 1, 1, '')) AS ShapeParaType " +
                ",(STUFF((SELECT CAST(',' + rtrim(convert(Varchar(10),intMinLen)) AS VARCHAR(MAX)) " +
                "FROM dbo.ShapeParamDetails P, dbo.ShapeMaster M " +
                "WHERE (P.intShapeID = M.intShapeID " +
                "AND chrShapeCode = D.MeshShapeCode) " +
                "AND P.bitEdit = 1 " +
                "ORDER BY UPPER(chrParamName) " +
                "FOR XML PATH('')), 1, 1, '')) AS ShapeMinLen " +
                ",(STUFF((SELECT CAST(',' + rtrim(convert(Varchar(10),intMaxLen)) AS VARCHAR(MAX)) " +
                "FROM dbo.ShapeParamDetails P, dbo.ShapeMaster M " +
                "WHERE (P.intShapeID = M.intShapeID " +
                "AND chrShapeCode = D.MeshShapeCode) " +
                "AND P.bitEdit = 1 " +
                "ORDER BY UPPER(chrParamName) " +
                "FOR XML PATH('')), 1, 1, '')) AS ShapeMaxLen " +
                ",(STUFF((SELECT CAST(',' + rtrim(chrWireType) AS VARCHAR(MAX)) " +
                "FROM dbo.ShapeParamDetails P, dbo.ShapeMaster M " +
                "WHERE (P.intShapeID = M.intShapeID " +
                "AND chrShapeCode = D.MeshShapeCode) " +
                "AND P.bitEdit = 1 " +
                "ORDER BY UPPER(chrParamName) " +
                "FOR XML PATH('')), 1, 1, '')) AS ShapeWireType, " +
                "M.decMWDiameter, " +
                "M.intMWSpace, " +
                "M.decCWDiameter, " +
                "M.intCWSpace, " +
                "M.decWeightArea, " +
                "M.intMinLinkFactor, " +
                "M.intMaxLinkFactor, " +
                "M.chrTwinIndicator, " +
                "MWLength, " +
                "MWBOM, " +
                "CWBOM, " +
                "SplitNotes " +
                "FROM dbo.OESCTSMESHBeamDetails D " +
                "LEFT OUTER JOIN dbo.ProductCodeMaster M " +
                "ON D.MeshProduct = M.vchProductCode " +
                "WHERE CustomerCode = '" + CustomerCode + "' " +
                "AND ProjectCode = '" + ProjectCode + "' " +
                "AND JobID = " + JobID.ToString() + " " +
                "AND BBSID = " + BBSID.ToString() + " " +
                "ORDER BY MeshSort ";

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
                            lNewBeam = new CTSShapeBeamDetailsModels
                            {
                                CustomerCode = lRst.GetString(0).Trim(),
                                ProjectCode = lRst.GetString(1).Trim(),
                                JobID = lRst.GetInt32(2),
                                BBSID = lRst.GetInt32(3),
                                MeshID = lRst.GetInt32(4),
                                MeshSort = lRst.GetInt32(5),
                                MeshMark = lRst.GetValue(6) == DBNull.Value ? "" : lRst.GetString(6).Trim(),
                                MeshWidth = lRst.GetValue(7) == DBNull.Value ? null : (int?)lRst.GetInt32(7),
                                MeshDepth = lRst.GetValue(8) == DBNull.Value ? null : (int?)lRst.GetInt32(8),
                                MeshSlope = lRst.GetValue(9) == DBNull.Value ? null : (int?)lRst.GetInt32(9),
                                MeshProduct = lRst.GetValue(10) == DBNull.Value ? "" : lRst.GetString(10).Trim(),
                                MeshShapeCode = lRst.GetValue(11) == DBNull.Value ? "" : lRst.GetString(11).Trim(),
                                MeshTotalLinks = lRst.GetValue(12) == DBNull.Value ? null : (int?)lRst.GetInt32(12),
                                MeshSpan = lRst.GetValue(13) == DBNull.Value ? null : (int?)lRst.GetInt32(13),
                                MeshMemberQty = lRst.GetValue(14) == DBNull.Value ? null : (int?)lRst.GetInt32(14),
                                MeshCapping = lRst.GetValue(15) == DBNull.Value ? false : lRst.GetBoolean(15),
                                MeshCPProduct = lRst.GetValue(16) == DBNull.Value ? "" : lRst.GetString(16).Trim(),
                                A = lRst.GetValue(17) == DBNull.Value ? null : (lRst.GetInt32(17) == 0 ? null : (int?)lRst.GetInt32(17)),
                                B = lRst.GetValue(18) == DBNull.Value ? null : (lRst.GetInt32(18) == 0 ? null : (int?)lRst.GetInt32(18)),
                                C = lRst.GetValue(19) == DBNull.Value ? null : (lRst.GetInt32(19) == 0 ? null : (int?)lRst.GetInt32(19)),
                                D = lRst.GetValue(20) == DBNull.Value ? null : (lRst.GetInt32(20) == 0 ? null : (int?)lRst.GetInt32(20)),
                                E = lRst.GetValue(21) == DBNull.Value ? null : (lRst.GetInt32(21) == 0 ? null : (int?)lRst.GetInt32(21)),
                                P = lRst.GetValue(22) == DBNull.Value ? null : (lRst.GetInt32(22) == 0 ? null : (int?)lRst.GetInt32(22)),
                                Q = lRst.GetValue(23) == DBNull.Value ? null : (lRst.GetInt32(23) == 0 ? null : (int?)lRst.GetInt32(23)),
                                HOOK = lRst.GetValue(24) == DBNull.Value ? null : (lRst.GetInt32(24) == 0 ? null : (int?)lRst.GetInt32(24)),
                                LEG = lRst.GetValue(25) == DBNull.Value ? null : (lRst.GetInt32(25) == 0 ? null : (int?)lRst.GetInt32(25)),
                                MeshTotalWT = lRst.GetValue(26) == DBNull.Value ? null : (decimal?)lRst.GetDecimal(26),
                                Remarks = lRst.GetValue(27) == DBNull.Value ? "" : lRst.GetString(27).Trim(),
                                MeshShapeParameters = lRst.GetValue(28) == DBNull.Value ? "" : lRst.GetString(28).Trim().ToUpper(),
                                MeshEditParameters = lRst.GetValue(29) == DBNull.Value ? "" : lRst.GetString(29).Trim().ToUpper(),
                                MeshShapeParamTypes = lRst.GetValue(30) == DBNull.Value ? "" : lRst.GetString(30).Trim(),
                                MeshShapeMinValues = lRst.GetValue(31) == DBNull.Value ? "" : lRst.GetString(31).Trim(),
                                MeshShapeMaxValues = lRst.GetValue(32) == DBNull.Value ? "" : lRst.GetString(32).Trim(),
                                MeshShapeWireTypes = lRst.GetValue(33) == DBNull.Value ? "" : lRst.GetString(33).Trim(),
                                ProdMWDia = lRst.GetValue(34) == DBNull.Value ? null : (decimal?)lRst.GetDecimal(34),
                                ProdMWSpacing = lRst.GetValue(35) == DBNull.Value ? null : (int?)lRst.GetInt32(35),
                                ProdCWDia = lRst.GetValue(36) == DBNull.Value ? null : (decimal?)lRst.GetDecimal(36),
                                ProdCWSpacing = lRst.GetValue(37) == DBNull.Value ? null : (int?)lRst.GetInt32(37),
                                ProdMass = lRst.GetValue(38) == DBNull.Value ? null : (decimal?)lRst.GetDecimal(38),
                                ProdMinFactor = lRst.GetValue(39) == DBNull.Value ? null : (int?)lRst.GetInt32(39),
                                ProdMaxFactor = lRst.GetValue(40) == DBNull.Value ? null : (int?)lRst.GetInt32(40),
                                ProdTwinInd = lRst.GetValue(41) == DBNull.Value ? "" : lRst.GetString(41).Trim(),
                                MWLength = lRst.GetValue(42) == DBNull.Value ? null : (int?)lRst.GetInt32(42),
                                MWBOM = lRst.GetValue(43) == DBNull.Value ? "" : lRst.GetString(43).Trim(),
                                CWBOM = lRst.GetValue(44) == DBNull.Value ? "" : lRst.GetString(44).Trim(),
                                SplitNotes = lRst.GetValue(45) == DBNull.Value ? "" : lRst.GetString(45).Trim()
                            };
                            lBeamDetails.Add(lNewBeam);
                        }
                    }
                    lRst.Close();

                    lProcessObj.CloseNDSConnection(ref lNDSCon);
                }

                lProcessObj = null;
            }

            // return Json(lBeamDetails, JsonRequestBehavior.AllowGet);
            return Ok(lBeamDetails);

        }

        //get Mesh List 
        [HttpPost]
        [Route("/getBeamProductCode_beam")]
        //[ValidateAntiForgeryHeader]
        public ActionResult getBeamProductCode()
        {
            var lBeamProduct = new List<CTSMeshProdcutCode>();
            CTSMeshProdcutCode lProductCode = new CTSMeshProdcutCode();
            var lCmd = new SqlCommand();
            var lNDSCon = new SqlConnection();
            SqlDataReader lRst;

            var lProcessObj = new ProcessController();
            if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
            {
                lCmd.CommandText =
                "select vchproductCode, decWeightArea " +
                "FROM dbo.ProductCodeMaster " +
                "WHERE vchproductCode like 'WS%' " +
                "AND vchproductCode NOT like '%CP' " +
                "AND vchproductCode NOT like '%D' " +
                "ORDER BY vchproductCode ";

                lCmd.Connection = lNDSCon;
                lCmd.CommandTimeout = 300;
                lRst = lCmd.ExecuteReader();
                if (lRst.HasRows)
                {
                    while (lRst.Read())
                    {
                        lProductCode = new CTSMeshProdcutCode
                        {
                            MeshProductCode = lRst.GetValue(0) == DBNull.Value ? "" : lRst.GetString(0).Trim(),
                            MeshWeightArea = lRst.GetValue(1) == DBNull.Value ? 0 : lRst.GetDecimal(1)
                        };
                        lBeamProduct.Add(lProductCode);
                    }
                }
                lRst.Close();

                lCmd.CommandText =
                "select vchproductCode, decWeightArea " +
                "FROM dbo.ProductCodeMaster " +
                "WHERE vchproductCode like 'WS%D' " +
                "ORDER BY vchproductCode ";

                lCmd.Connection = lNDSCon;
                lCmd.CommandTimeout = 300;
                lRst = lCmd.ExecuteReader();
                if (lRst.HasRows)
                {
                    while (lRst.Read())
                    {
                        lProductCode = new CTSMeshProdcutCode
                        {
                            MeshProductCode = lRst.GetValue(0) == DBNull.Value ? "" : lRst.GetString(0).Trim(),
                            MeshWeightArea = lRst.GetValue(1) == DBNull.Value ? 0 : lRst.GetDecimal(1)
                        };
                        lBeamProduct.Add(lProductCode);
                    }
                }
                lRst.Close();

                //lCmd.CommandText =
                //"select vchproductCode, decWeightArea " +
                //"FROM dbo.ProductCodeMaster " +
                //"WHERE vchproductCode like 'WS%CP' " +
                //"ORDER BY vchproductCode ";

                //lCmd.Connection = lNDSCon;
                //lCmd.CommandTimeout = 300;
                //lRst = lCmd.ExecuteReader();
                //if (lRst.HasRows)
                //{
                //    while (lRst.Read())
                //    {
                //        lProductCode = new CTSMeshProdcutCode
                //        {
                //            MeshProductCode = lRst.GetValue(0) == DBNull.Value ? "" : lRst.GetString(0).Trim(),
                //            MeshWeightArea = lRst.GetValue(1) == DBNull.Value ? 0 : lRst.GetDecimal(1)
                //        };
                //        lBeamProduct.Add(lProductCode);
                //    }
                //}
                //lRst.Close();

                lProcessObj.CloseNDSConnection(ref lNDSCon);
                lProcessObj = null;
            }

            //            return Json(lBeamProduct, JsonRequestBehavior.AllowGet);
            return Ok(lBeamProduct);
        }

        //get Mesh List 
        [HttpPost]
        [Route("/getCPProductCode_beam")]
        //  [ValidateAntiForgeryHeader]
        public ActionResult getCPProductCode()
        {
            var lCPProduct = "";
            var lCmd = new SqlCommand();
            var lNDSCon = new SqlConnection();
            SqlDataReader lRst;

            var lProcessObj = new ProcessController();
            if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
            {
                lCmd.CommandText =
                "SELECT vchproductCode " +
                "FROM dbo.ProductCodeMaster " +
                "WHERE vchproductCode like 'WS%CP' " +
                "AND tntStatusId = 1 " +
                "ORDER BY vchproductCode ";

                lCmd.Connection = lNDSCon;
                lCmd.CommandTimeout = 300;
                lRst = lCmd.ExecuteReader();
                if (lRst.HasRows)
                {
                    while (lRst.Read())
                    {
                        var lMeshProductCode = lRst.GetValue(0) == DBNull.Value ? "" : lRst.GetString(0).Trim();
                        if (lMeshProductCode.Trim().Length > 0)
                        {
                            lCPProduct = lCPProduct + "," + lMeshProductCode.Trim();
                        }
                    }
                }
                lRst.Close();

                lProcessObj.CloseNDSConnection(ref lNDSCon);
                lProcessObj = null;
            }

            // return Json(lCPProduct, JsonRequestBehavior.AllowGet);
            return Ok(lCPProduct);
        }

        //get Column Product Code and Weight/Area List 
        [HttpPost]
        [Route("/getColumnProductCode_beam")]
        //[ValidateAntiForgeryHeader]
        public ActionResult getColumnProductCode()
        {
            var lColumnProduct = new List<CTSMeshProdcutCode>();
            CTSMeshProdcutCode lProductCode = new CTSMeshProdcutCode();
            var lCmd = new SqlCommand();
            var lNDSCon = new SqlConnection();
            SqlDataReader lRst;

            var lProcessObj = new ProcessController();
            if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
            {
                lCmd.CommandText =
                "select vchproductCode, decWeightArea " +
                "FROM dbo.ProductCodeMaster " +
                "WHERE vchproductCode like 'WL%' " +
                "AND vchproductCode NOT like '%CL' " +
                "ORDER BY vchproductCode ";

                lCmd.Connection = lNDSCon;
                lCmd.CommandTimeout = 300;
                lRst = lCmd.ExecuteReader();
                if (lRst.HasRows)
                {
                    while (lRst.Read())
                    {
                        lProductCode = new CTSMeshProdcutCode
                        {
                            MeshProductCode = lRst.GetValue(0) == DBNull.Value ? "" : lRst.GetString(0).Trim(),
                            MeshWeightArea = lRst.GetValue(1) == DBNull.Value ? 0 : lRst.GetDecimal(1)
                        };
                        lColumnProduct.Add(lProductCode);
                    }
                }
                lRst.Close();

                lCmd.CommandText =
                "select vchproductCode, decWeightArea " +
                "FROM dbo.ProductCodeMaster " +
                "WHERE vchproductCode like 'WL%CL' " +
                "ORDER BY vchproductCode ";

                lCmd.Connection = lNDSCon;
                lCmd.CommandTimeout = 300;
                lRst = lCmd.ExecuteReader();
                if (lRst.HasRows)
                {
                    while (lRst.Read())
                    {
                        lProductCode = new CTSMeshProdcutCode
                        {
                            MeshProductCode = lRst.GetValue(0) == DBNull.Value ? "" : lRst.GetString(0).Trim(),
                            MeshWeightArea = lRst.GetValue(1) == DBNull.Value ? 0 : lRst.GetDecimal(1)
                        };
                        lColumnProduct.Add(lProductCode);
                    }
                }
                lRst.Close();

                lProcessObj.CloseNDSConnection(ref lNDSCon);
                lProcessObj = null;
            }

            //return Json(lColumnProduct, JsonRequestBehavior.AllowGet);
            return Ok(lColumnProduct);
        }

        //get Other Mesh Product Code and Weight/Area List 
        [HttpPost]
        [Route("/getOthersProductCode_beam")]
        //[ValidateAntiForgeryHeader]
        public ActionResult getOthersProductCode()
        {
            var lMESHProduct = new List<CTSMeshProdcutCode>();
            CTSMeshProdcutCode lProductCode = new CTSMeshProdcutCode();
            var lCmd = new SqlCommand();
            var lNDSCon = new SqlConnection();
            SqlDataReader lRst;

            var lProcessObj = new ProcessController();
            if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
            {
                lCmd.CommandText =
                "SELECT vchproductCode, decWeightArea " +
                "FROM dbo.ProductCodeMaster M, dbo.OESCTSMESHProdCode C " +
                "WHERE M.vchProductCode = C.ProdCode " +
                "AND C.ProdType = 'SLAB' " +
                "ORDER BY Class, intMWSpace desc, intCWSpace desc, decMWDiameter desc, decCWDiameter desc ";

                lCmd.Connection = lNDSCon;
                lCmd.CommandTimeout = 300;
                lRst = lCmd.ExecuteReader();
                if (lRst.HasRows)
                {
                    while (lRst.Read())
                    {
                        lProductCode = new CTSMeshProdcutCode
                        {
                            MeshProductCode = lRst.GetValue(0) == DBNull.Value ? "" : lRst.GetString(0).Trim(),
                            MeshWeightArea = lRst.GetValue(1) == DBNull.Value ? 0 : lRst.GetDecimal(1)
                        };
                        lMESHProduct.Add(lProductCode);
                    }
                }
                lRst.Close();

                //lCmd.CommandText =
                //"SELECT vchproductCode, decWeightArea " +
                //"FROM dbo.ProductCodeMaster " +
                //"WHERE (vchProductCode > 'WD1' " +
                //"AND vchProductCode < 'WD9') " +
                //"AND vchProductCode not like '%-50' " +
                //"AND vchProductCode not like '%-BT' " +
                //"AND len(vchProductCode) < 5 " +
                //"AND decMWDiameter <= 13 " +
                //"AND decCWDiameter <= 13 " +
                //"AND intMWSpace >= 100 " +
                //"AND intCWSpace >= 100 " +
                //"AND chrTwinIndicator = '0' " +
                //"ORDER BY intMWSpace, intCWSpace, decMWDiameter, decCWDiameter ";

                //lCmd.Connection = lNDSCon;
                //lCmd.CommandTimeout = 300;
                //lRst = lCmd.ExecuteReader();
                //if (lRst.HasRows)
                //{
                //    while (lRst.Read())
                //    {
                //        lProductCode = new CTSMeshProdcutCode
                //        {
                //            MeshProductCode = lRst.GetValue(0) == DBNull.Value ? "" : lRst.GetString(0).Trim(),
                //            MeshWeightArea = lRst.GetValue(1) == DBNull.Value ? 0 : lRst.GetDecimal(1)
                //        };
                //        lMESHProduct.Add(lProductCode);
                //    }
                //}
                //lRst.Close();

                //lCmd.CommandText =
                //"SELECT vchproductCode, decWeightArea " +
                //"FROM dbo.ProductCodeMaster " +
                //"WHERE (vchProductCode > 'WB1' " +
                //"AND vchProductCode < 'WB9') " +
                //"AND vchProductCode not like '%-50' " +
                //"AND vchProductCode not like '%-BT' " +
                //"AND vchProductCode not like '%A' " +
                //"AND len(vchProductCode) < 10 " +
                //"AND decMWDiameter <= 13 " +
                //"AND decCWDiameter <= 13 " +
                //"AND intMWSpace >= 100 " +
                //"AND intCWSpace >= 100 " +
                //"AND chrTwinIndicator = '0' " +
                //"ORDER BY intMWSpace, intCWSpace, decMWDiameter, decCWDiameter ";

                //lCmd.Connection = lNDSCon;
                //lCmd.CommandTimeout = 300;
                //lRst = lCmd.ExecuteReader();
                //if (lRst.HasRows)
                //{
                //    while (lRst.Read())
                //    {
                //        lProductCode = new CTSMeshProdcutCode
                //        {
                //            MeshProductCode = lRst.GetValue(0) == DBNull.Value ? "" : lRst.GetString(0).Trim(),
                //            MeshWeightArea = lRst.GetValue(1) == DBNull.Value ? 0 : lRst.GetDecimal(1)
                //        };
                //        lMESHProduct.Add(lProductCode);
                //    }
                //}
                //lRst.Close();

                //lCmd.CommandText =
                //"SELECT vchproductCode, decWeightArea " +
                //"FROM dbo.ProductCodeMaster " +
                //"WHERE (vchProductCode > 'WE1' " +
                //"AND vchProductCode < 'WE9') " +
                //"AND vchProductCode not like '%-50' " +
                //"AND vchProductCode not like '%-BT' " +
                //"AND vchProductCode not like '%A' " +
                //"AND len(vchProductCode) < 10 " +
                //"AND decMWDiameter <= 13 " +
                //"AND decCWDiameter <= 13 " +
                //"AND intMWSpace >= 100 " +
                //"AND intCWSpace >= 100 " +
                //"AND chrTwinIndicator = '0' " +
                //"ORDER BY intMWSpace, intCWSpace, decMWDiameter, decCWDiameter ";

                //lCmd.Connection = lNDSCon;
                //lCmd.CommandTimeout = 300;
                //lRst = lCmd.ExecuteReader();
                //if (lRst.HasRows)
                //{
                //    while (lRst.Read())
                //    {
                //        lProductCode = new CTSMeshProdcutCode
                //        {
                //            MeshProductCode = lRst.GetValue(0) == DBNull.Value ? "" : lRst.GetString(0).Trim(),
                //            MeshWeightArea = lRst.GetValue(1) == DBNull.Value ? 0 : lRst.GetDecimal(1)
                //        };
                //        lMESHProduct.Add(lProductCode);
                //    }
                //}
                //lRst.Close();

                //lCmd.CommandText =
                //"SELECT vchproductCode, decWeightArea " +
                //"FROM dbo.ProductCodeMaster " +
                //"WHERE (vchProductCode > 'WF1' " +
                //"AND vchProductCode < 'WF9') " +
                //"AND vchProductCode not like '%-50' " +
                //"AND vchProductCode not like '%-BT' " +
                //"AND vchProductCode not like '%A' " +
                //"AND len(vchProductCode) < 10 " +
                //"AND decMWDiameter <= 13 " +
                //"AND decCWDiameter <= 13 " +
                //"ORDER BY intMWSpace, intCWSpace, decMWDiameter, decCWDiameter ";

                //lCmd.Connection = lNDSCon;
                //lCmd.CommandTimeout = 300;
                //lRst = lCmd.ExecuteReader();
                //if (lRst.HasRows)
                //{
                //    while (lRst.Read())
                //    {
                //        lProductCode = new CTSMeshProdcutCode
                //        {
                //            MeshProductCode = lRst.GetValue(0) == DBNull.Value ? "" : lRst.GetString(0).Trim(),
                //            MeshWeightArea = lRst.GetValue(1) == DBNull.Value ? 0 : lRst.GetDecimal(1)
                //        };
                //        lMESHProduct.Add(lProductCode);
                //    }
                //}
                //lRst.Close();

                //lCmd.CommandText =
                //"SELECT vchproductCode, decWeightArea " +
                //"FROM dbo.ProductCodeMaster " +
                //"WHERE (vchProductCode > 'W2F1' " +
                //"AND vchProductCode < 'W2F9') " +
                //"AND vchProductCode not like '%-50' " +
                //"AND vchProductCode not like '%-BT' " +
                //"AND vchProductCode not like '%A' " +
                //"AND len(vchProductCode) < 10 " +
                //"AND decMWDiameter <= 13 " +
                //"AND decCWDiameter <= 13 " +
                //"ORDER BY intMWSpace, intCWSpace, decMWDiameter, decCWDiameter ";

                //lCmd.Connection = lNDSCon;
                //lCmd.CommandTimeout = 300;
                //lRst = lCmd.ExecuteReader();
                //if (lRst.HasRows)
                //{
                //    while (lRst.Read())
                //    {
                //        lProductCode = new CTSMeshProdcutCode
                //        {
                //            MeshProductCode = lRst.GetValue(0) == DBNull.Value ? "" : lRst.GetString(0).Trim(),
                //            MeshWeightArea = lRst.GetValue(1) == DBNull.Value ? 0 : lRst.GetDecimal(1)
                //        };
                //        lMESHProduct.Add(lProductCode);
                //    }
                //}
                //lRst.Close();


                //lCmd.CommandText =
                //"SELECT vchproductCode, decWeightArea " +
                //"FROM dbo.ProductCodeMaster " +
                //"WHERE ((vchProductCode > 'WAD1' " +
                //"AND vchProductCode < 'WAD9') " +
                //"OR (vchProductCode > 'WAE1' " +
                //"AND vchProductCode < 'WAE9') " +
                //"OR (vchProductCode > 'WDA1' " +
                //"AND vchProductCode < 'WDA9') " +
                //"OR (vchProductCode > 'WDE1' " +
                //"AND vchProductCode < 'WDE9') " +
                //"OR (vchProductCode > 'WEA1' " +
                //"AND vchProductCode < 'WEA9') " +
                //"OR (vchProductCode > 'WED1' " +
                //"AND vchProductCode < 'WED9')) " +
                //"AND vchProductCode not like '%-50' " +
                //"AND vchProductCode not like '%-BT' " +
                //"AND vchProductCode not like '%A' " +
                //"AND len(vchProductCode) < 10 " +
                //"AND decMWDiameter <= 13 " +
                //"AND decCWDiameter <= 13 " +
                //"AND intMWSpace >= 100 " +
                //"AND intCWSpace >= 100 " +
                //"AND chrTwinIndicator = '0' " +
                //"ORDER BY intMWSpace, intCWSpace, decMWDiameter, decCWDiameter ";

                //lCmd.Connection = lNDSCon;
                //lCmd.CommandTimeout = 300;
                //lRst = lCmd.ExecuteReader();
                //if (lRst.HasRows)
                //{
                //    while (lRst.Read())
                //    {
                //        lProductCode = new CTSMeshProdcutCode
                //        {
                //            MeshProductCode = lRst.GetValue(0) == DBNull.Value ? "" : lRst.GetString(0).Trim(),
                //            MeshWeightArea = lRst.GetValue(1) == DBNull.Value ? 0 : lRst.GetDecimal(1)
                //        };
                //        lMESHProduct.Add(lProductCode);
                //    }
                //}
                //lRst.Close();

                //lCmd.CommandText =
                //"SELECT vchproductCode, decWeightArea " +
                //"FROM dbo.ProductCodeMaster " +
                //"WHERE ((vchProductCode > 'WA1' " +
                //"AND vchProductCode < 'WA9') " +
                //"OR (vchProductCode > 'WAD1' " +
                //"AND vchProductCode < 'WAD9') " +
                //"OR (vchProductCode > 'WAE1' " +
                //"AND vchProductCode < 'WAE9') " +
                //"OR (vchProductCode > 'WB1' " +
                //"AND vchProductCode < 'WB9') " +
                //"OR (vchProductCode > 'WD1' " +
                //"AND vchProductCode < 'WD9') " +
                //"OR (vchProductCode > 'WDA1' " +
                //"AND vchProductCode < 'WDA9') " +
                //"OR (vchProductCode > 'WDE1' " +
                //"AND vchProductCode < 'WDE9') " +
                //"OR (vchProductCode > 'WE1' " +
                //"AND vchProductCode < 'WE9') " +
                //"OR (vchProductCode > 'WEA1' " +
                //"AND vchProductCode < 'WEA9') " +
                //"OR (vchProductCode > 'WED1' " +
                //"AND vchProductCode < 'WED9') " +
                //"OR (vchProductCode > 'WF1' " +
                //"AND vchProductCode < 'WF9')) " +
                //"AND vchProductCode not like '%-50' " +
                //"AND vchProductCode not like '%-BT' " +
                //"AND vchProductCode not like '%A' " +
                //"AND len(vchProductCode) < 10 " +
                //"AND decMWDiameter <= 13 " +
                //"AND decCWDiameter <= 13 " +
                //"AND intMWSpace >= 100 " +
                //"AND intCWSpace >= 100 " +
                //"AND chrTwinIndicator <> '0' " +
                //"ORDER BY intMWSpace, intCWSpace, decMWDiameter, decCWDiameter ";

                //lCmd.Connection = lNDSCon;
                //lCmd.CommandTimeout = 300;
                //lRst = lCmd.ExecuteReader();
                //if (lRst.HasRows)
                //{
                //    while (lRst.Read())
                //    {
                //        lProductCode = new CTSMeshProdcutCode
                //        {
                //            MeshProductCode = lRst.GetValue(0) == DBNull.Value ? "" : lRst.GetString(0).Trim(),
                //            MeshWeightArea = lRst.GetValue(1) == DBNull.Value ? 0 : lRst.GetDecimal(1)
                //        };
                //        lMESHProduct.Add(lProductCode);
                //    }
                //}
                //lRst.Close();

                //class B products
                //lCmd.CommandText =
                //"SELECT vchproductCode, decWeightArea " +
                //"FROM dbo.ProductCodeMaster " +
                //"WHERE vchProductCode like 'WH%' " +
                //"AND vchProductCode not like '%-50' " +
                //"AND vchProductCode not like '%-BT' " +
                //"AND len(vchProductCode) <= 12 " +
                //"AND decMWDiameter <= 13 " +
                //"AND decCWDiameter <= 13 " +
                //"AND intMWSpace >= 100 " +
                //"AND intCWSpace >= 100 " +
                //"AND chrTwinIndicator = '0' " +
                //"ORDER BY intMWSpace, intCWSpace, decMWDiameter, decCWDiameter ";

                //lCmd.Connection = lNDSCon;
                //lCmd.CommandTimeout = 300;
                //lRst = lCmd.ExecuteReader();
                //if (lRst.HasRows)
                //{
                //    while (lRst.Read())
                //    {
                //        lProductCode = new CTSMeshProdcutCode
                //        {
                //            MeshProductCode = lRst.GetValue(0) == DBNull.Value ? "" : lRst.GetString(0).Trim(),
                //            MeshWeightArea = lRst.GetValue(1) == DBNull.Value ? 0 : lRst.GetDecimal(1)
                //        };
                //        lMESHProduct.Add(lProductCode);
                //    }
                //}
                //lRst.Close();


                lProcessObj.CloseNDSConnection(ref lNDSCon);
                lProcessObj = null;
            }

            //return Json(lMESHProduct, JsonRequestBehavior.AllowGet);
            return Ok(lMESHProduct);
        }

        //get Beam shape List 
        [HttpPost]
        [Route("/getBeamShapeCode_beam")]
        //[ValidateAntiForgeryHeader]
        public ActionResult getBeamShapeCode()
        {
            var lBeamShape = new List<string>();
            string lShapeCode = "";
            var lCmd = new SqlCommand();
            var lNDSCon = new SqlConnection();
            SqlDataReader lRst;

            var lProcessObj = new ProcessController();
            if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
            {
                lCmd.CommandText =
                "SELECT chrShapeCode " +
                "FROM dbo.ShapeMaster " +
                "WHERE vchMeshShapeGroup like '%beam%' " +
                "AND chrShapeCode like 'C%' " +
                "AND chrShapeCode <> 'CP' " +
                "AND tntStatusID = 1 " +
                "ORDER BY chrShapeCode ";

                lCmd.Connection = lNDSCon;
                lCmd.CommandTimeout = 300;
                lRst = lCmd.ExecuteReader();
                if (lRst.HasRows)
                {
                    while (lRst.Read())
                    {
                        lShapeCode = lRst.GetValue(0) == DBNull.Value ? "" : lRst.GetString(0).Trim();
                        if (lShapeCode.Length > 0)
                        {
                            lBeamShape.Add(lShapeCode);
                        }
                    }
                }
                lRst.Close();

                lCmd.CommandText =
                "SELECT chrShapeCode " +
                "FROM dbo.ShapeMaster " +
                "WHERE vchMeshShapeGroup like '%beam%' " +
                "AND chrShapeCode like 'K%' " +
                "AND tntStatusID = 1 " +
                "ORDER BY chrShapeCode ";

                lCmd.Connection = lNDSCon;
                lCmd.CommandTimeout = 300;
                lRst = lCmd.ExecuteReader();
                if (lRst.HasRows)
                {
                    while (lRst.Read())
                    {
                        lShapeCode = lRst.GetValue(0) == DBNull.Value ? "" : lRst.GetString(0).Trim();
                        if (lShapeCode.Length > 0)
                        {
                            lBeamShape.Add(lShapeCode);
                        }
                    }
                }
                lRst.Close();

                lCmd.CommandText =
                "SELECT chrShapeCode " +
                "FROM dbo.ShapeMaster " +
                "WHERE vchMeshShapeGroup like '%beam%' " +
                "AND chrShapeCode like 'SC%' " +
                "AND tntStatusID = 1 " +
                "ORDER BY chrShapeCode ";

                lCmd.Connection = lNDSCon;
                lCmd.CommandTimeout = 300;
                lRst = lCmd.ExecuteReader();
                if (lRst.HasRows)
                {
                    while (lRst.Read())
                    {
                        lShapeCode = lRst.GetValue(0) == DBNull.Value ? "" : lRst.GetString(0).Trim();
                        if (lShapeCode.Length > 0)
                        {
                            lBeamShape.Add(lShapeCode);
                        }
                    }
                }
                lRst.Close();

                lCmd.CommandText =
                "SELECT chrShapeCode " +
                "FROM dbo.ShapeMaster " +
                "WHERE vchMeshShapeGroup like '%beam%' " +
                "AND chrShapeCode not like 'C%' " +
                "AND chrShapeCode not like 'K%' " +
                "AND chrShapeCode not like 'SC%' " +
                "AND tntStatusID = 1 " +
                "ORDER BY chrShapeCode ";

                lCmd.Connection = lNDSCon;
                lCmd.CommandTimeout = 300;
                lRst = lCmd.ExecuteReader();
                if (lRst.HasRows)
                {
                    while (lRst.Read())
                    {
                        lShapeCode = lRst.GetValue(0) == DBNull.Value ? "" : lRst.GetString(0).Trim();
                        if (lShapeCode.Length > 0)
                        {
                            lBeamShape.Add(lShapeCode);
                        }
                    }
                }
                lRst.Close();


                lProcessObj.CloseNDSConnection(ref lNDSCon);
                lProcessObj = null;
            }

            // return Json(lBeamShape, JsonRequestBehavior.AllowGet);
            return Ok(lBeamShape);
        }

        //get Column shape List 
        [HttpPost]
        [Route("/getColumnShapeCode_beam")]
        //[ValidateAntiForgeryHeader]
        public ActionResult getColumnShapeCode()
        {
            var lColumnShape = new List<string>();
            string lShapeCode = "";
            var lCmd = new SqlCommand();
            var lNDSCon = new SqlConnection();
            SqlDataReader lRst;

            var lProcessObj = new ProcessController();
            if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
            {
                lCmd.CommandText =
                "SELECT chrShapeCode " +
                "FROM dbo.ShapeMaster " +
                "WHERE vchMeshShapeGroup like '%column%' " +
                "AND chrShapeCode like 'C%' " +
                "AND chrShapeCode <> 'CL' " +
                "AND tntStatusID = 1 " +
                "ORDER BY chrShapeCode ";

                lCmd.Connection = lNDSCon;
                lCmd.CommandTimeout = 300;
                lRst = lCmd.ExecuteReader();
                if (lRst.HasRows)
                {
                    while (lRst.Read())
                    {
                        lShapeCode = lRst.GetValue(0) == DBNull.Value ? "" : lRst.GetString(0).Trim();
                        if (lShapeCode.Length > 0)
                        {
                            lColumnShape.Add(lShapeCode);
                        }
                    }
                }
                lRst.Close();

                lCmd.CommandText =
                "SELECT chrShapeCode " +
                "FROM dbo.ShapeMaster " +
                "WHERE vchMeshShapeGroup like '%column%' " +
                "AND chrShapeCode not like 'C%' " +
                "AND tntStatusID = 1 " +
                "ORDER BY chrShapeCode ";

                lCmd.Connection = lNDSCon;
                lCmd.CommandTimeout = 300;
                lRst = lCmd.ExecuteReader();
                if (lRst.HasRows)
                {
                    while (lRst.Read())
                    {
                        lShapeCode = lRst.GetValue(0) == DBNull.Value ? "" : lRst.GetString(0).Trim();
                        if (lShapeCode.Length > 0)
                        {
                            lColumnShape.Add(lShapeCode);
                        }
                    }
                }
                lRst.Close();


                lProcessObj.CloseNDSConnection(ref lNDSCon);
                lProcessObj = null;
            }

            //            return Json(lColumnShape, JsonRequestBehavior.AllowGet);
            return Ok(lColumnShape);
        }

        //get Others MESH shape List 
        [HttpPost]

        [Route("/getOthersShapeCode_beam")]
        // [ValidateAntiForgeryHeader]
        public ActionResult getOthersShapeCode()
        {
            var lColumnShape = new List<string>();
            string lShapeCode = "";
            var lCmd = new SqlCommand();
            var lNDSCon = new SqlConnection();
            SqlDataReader lRst;

            var lProcessObj = new ProcessController();
            if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
            {
                lCmd.CommandText =
                "SELECT chrShapeCode " +
                "FROM dbo.ShapeMaster " +
                "WHERE vchMeshShapeGroup not like '%beam%' " +
                "AND  vchMeshShapeGroup not like '%column%' " +
                "AND chrShapeCode not like 'A%' " +
                "AND chrShapeCode not like 'N%' " +
                "AND chrShapeCode not like 'P%' " +
                "AND chrShapeCode not like 'T%' " +
                "AND (chrShapeCode = '1M1' " +
                "OR chrShapeCode = '1MR1' " +
                "OR chrShapeCode = '2M1' " +
                "OR chrShapeCode = '2MR1' " +
                "OR chrShapeCode = '1C1' " +
                "OR chrShapeCode = '1CR1' " +
                "OR chrShapeCode = '2C1' " +
                "OR chrShapeCode = '2CR1' " +
                "OR chrShapeCode = 'F') " +
                "AND tntStatusID = 1 " +
                "ORDER BY chrShapeCode ";

                lCmd.Connection = lNDSCon;
                lCmd.CommandTimeout = 300;
                lRst = lCmd.ExecuteReader();
                if (lRst.HasRows)
                {
                    while (lRst.Read())
                    {
                        lShapeCode = lRst.GetValue(0) == DBNull.Value ? "" : lRst.GetString(0).Trim();
                        if (lShapeCode.Length > 0)
                        {
                            lColumnShape.Add(lShapeCode);
                        }
                    }
                }
                lRst.Close();

                lProcessObj.CloseNDSConnection(ref lNDSCon);
                lProcessObj = null;
            }

            //return Json(lColumnShape, JsonRequestBehavior.AllowGet);
            return Ok(lColumnShape);
        }

        //save Mesh - Beam Stirrup
        [HttpPost]
        [Route("/saveMeshBeamDetails_beam/{pBeamDetails}")]
        // [ValidateAntiForgeryHeader]
        public async Task<ActionResult> saveMeshBeamDetails(CTSMESHBeamDetailsModels pBeamDetails)
        {
            if (pBeamDetails.CustomerCode == null)
            {
                pBeamDetails.CustomerCode = "";
            }
            if (pBeamDetails.CustomerCode.Length > 0)
            {
                pBeamDetails.UpdateDate = DateTime.Now;
                pBeamDetails.UpdateBy = User.Identity.GetUserName();


                var oldBar = await db.CTSMESHBeamDetails.FindAsync(pBeamDetails.CustomerCode, pBeamDetails.ProjectCode, pBeamDetails.JobID, pBeamDetails.BBSID, pBeamDetails.MeshID);
                if (oldBar == null)
                {
                    db.CTSMESHBeamDetails.Add(pBeamDetails);
                }
                else
                {
                    db.Entry(oldBar).CurrentValues.SetValues(pBeamDetails);
                }
                int lRtn = await db.SaveChangesAsync();

                return Json(true);
            }
            return Json(false);
        }

        //save Mesh - Column Link
        [HttpPost]
        [Route("/saveMeshColumnDetails_beam/{pBeamDetails}")]
        // [ValidateAntiForgeryHeader]
        public ActionResult saveMeshColumnDetails(CTSMESHColumnDetailsModels pColumnDetails)
        {
            if (pColumnDetails.CustomerCode == null)
            {
                pColumnDetails.CustomerCode = "";
            }
            if (pColumnDetails.CustomerCode.Length > 0)
            {
                pColumnDetails.UpdateDate = DateTime.Now;
                pColumnDetails.UpdateBy = User.Identity.GetUserName();


                var oldBar = db.CTSMESHColumnDetails.Find(pColumnDetails.CustomerCode, pColumnDetails.ProjectCode, pColumnDetails.JobID, pColumnDetails.BBSID, pColumnDetails.MeshID);
                if (oldBar == null)
                {
                    db.CTSMESHColumnDetails.Add(pColumnDetails);
                }
                else
                {
                    db.Entry(oldBar).CurrentValues.SetValues(pColumnDetails);
                }
                db.SaveChanges();

                return Json(true);
            }
            return Json(false);
        }

        //save Mesh - Standard Sheet
        [HttpPost]
        [Route("/saveMeshStdSheetDetails_beam/{pStdSheetDetails}")]
        //[ValidateAntiForgeryHeader]
        public ActionResult saveMeshStdSheetDetails(List<CTSMESHStdSheetDetailsModels> pStdSheetDetails)
        {
            var lErrorMsg = "";
            try
            {
                if (pStdSheetDetails.Count > 0)
                {
                    db.Database.ExecuteSqlCommand("DELETE FROM OESCTSMESHStdSheetDetails " +
                    "WHERE CustomerCode = '" + pStdSheetDetails[0].CustomerCode + "' " +
                    "AND ProjectCode = '" + pStdSheetDetails[0].ProjectCode + "' " +
                    "AND JobID = " + pStdSheetDetails[0].JobID.ToString() + " " +
                    "AND BBSID = " + pStdSheetDetails[0].BBSID.ToString() + " ");

                    for (int i = 0; i < pStdSheetDetails.Count; i++)
                    {
                        pStdSheetDetails[i].UpdateBy = User.Identity.Name;
                        pStdSheetDetails[i].UpdateDate = DateTime.Now;
                        if (pStdSheetDetails[i].CustomerCode == null)
                        {
                            pStdSheetDetails[i].CustomerCode = "";
                        }
                        if (pStdSheetDetails[i].CustomerCode.Length > 0)
                        {
                            db.CTSMESHStdSheetDetails.Add(pStdSheetDetails[i]);
                        }
                    }
                    db.SaveChanges();

                    //return Json(new { success = true, responseText = "Successfully saved." }, JsonRequestBehavior.AllowGet);
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                lErrorMsg = ex.Message;
            }
            //return Json(new { success = false, responseText = lErrorMsg }, JsonRequestBehavior.AllowGet);
            return Ok();

        }

        //save Mesh - Standard Sheet
        [HttpPost]
        [Route("/saveMeshOthersDetails_beam/{pOthersDetails}")]
        //[ValidateAntiForgeryHeader]
        public ActionResult saveMeshOthersDetails(CTSMESHOthersDetailsModels pOthersDetails)
        {
            if (pOthersDetails.CustomerCode == null)
            {
                pOthersDetails.CustomerCode = "";
            }
            if (pOthersDetails.CustomerCode.Length > 0)
            {
                pOthersDetails.UpdateDate = DateTime.Now;
                pOthersDetails.UpdateBy = User.Identity.GetUserName();


                var oldBar = db.CTSMESHOthersDetails.Find(pOthersDetails.CustomerCode, pOthersDetails.ProjectCode, pOthersDetails.JobID, pOthersDetails.BBSID, pOthersDetails.MeshID);
                if (oldBar == null)
                {
                    db.CTSMESHOthersDetails.Add(pOthersDetails);
                }
                else
                {
                    db.Entry(oldBar).CurrentValues.SetValues(pOthersDetails);
                }
                db.SaveChanges();

                return Json(true);
            }
            return Json(false);
        }

        [HttpGet]
        [Route("/getParaValue_beam/{pParameter}/{pPos}")]
        private int? getParaValue(string pParameter, byte pPos)
        {
            int? lReturn = null;
            if (pParameter != null)
            {
                if (pParameter.Trim().Length > 0)
                {
                    int lVar = 0;
                    if (int.TryParse(pParameter, out lVar) == true && pPos == 1)
                    {
                        if (lVar > 0) lReturn = lVar;
                    }
                    else
                    {
                        var lArray = pParameter.Split('-');
                        if (lArray != null)
                        {
                            if (lArray.Length >= 2)
                            {
                                lVar = 0;
                                if (pPos == 1)
                                {
                                    if (int.TryParse(lArray[0], out lVar) == true)
                                    {
                                        if (lVar > 0) lReturn = lVar;
                                    }
                                }
                                if (pPos == 2)
                                {
                                    if (int.TryParse(lArray[1], out lVar) == true)
                                    {
                                        if (lVar > 0) lReturn = lVar;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return lReturn;
        }

        // delete mesh beam
        [HttpPost]
        [Route("/deleteMeshBeamDetails_beam/{CustomerCode}/{ProjectCode}/{JobID}/{BBSID}/{MeshID}")]
        //  [ValidateAntiForgeryHeader]
        public ActionResult deleteMeshBeamDetails(string CustomerCode, string ProjectCode, int JobID, int BBSID, int MeshID)
        {
            var oldBar = db.CTSMESHBeamDetails.Find(CustomerCode, ProjectCode, JobID, BBSID, MeshID);
            if (oldBar != null)
            {
                db.CTSMESHBeamDetails.Remove(oldBar);
                db.SaveChanges();
            }

            return Json(true);
            //return RedirectToAction("Index");
        }

        // delete mesh column
        [HttpPost]
        [Route("/deleteMeshColumnDetails_beam/{CustomerCode}/{ProjectCode}/{JobID}/{BBSID}/{MeshID}")]
        // [ValidateAntiForgeryHeader]
        public ActionResult deleteMeshColumnDetails(string CustomerCode, string ProjectCode, int JobID, int BBSID, int MeshID)
        {
            var oldBar = db.CTSMESHColumnDetails.Find(CustomerCode, ProjectCode, JobID, BBSID, MeshID);
            if (oldBar != null)
            {
                db.CTSMESHColumnDetails.Remove(oldBar);
                db.SaveChanges();
            }

            return Json(true);
            //return RedirectToAction("Index");
        }

        // delete a standrad sheet reord
        [HttpPost]
        [Route("/deleteMeshStdSheetDetails_beam/{CustomerCode}/{ProjectCode}/{JobID}/{BBSID}/{MeshID}")]
        //[ValidateAntiForgeryHeader]
        public ActionResult deleteMeshStdSheetDetails(string CustomerCode, string ProjectCode, int JobID, int BBSID, int MeshID)
        {
            var oldBar = db.CTSMESHStdSheetDetails.Find(CustomerCode, ProjectCode, JobID, BBSID, MeshID);
            if (oldBar != null)
            {
                db.CTSMESHStdSheetDetails.Remove(oldBar);
                db.SaveChanges();
            }

            return Json(true);
            //return RedirectToAction("Index");
        }

        // delete a standrad sheet reord
        [HttpPost]
        [Route("/deleteMeshOthersDetails_beam/{CustomerCode}/{ProjectCode}/{JobID}/{BBSID}/{MeshID}")]
        // [ValidateAntiForgeryHeader]
        public ActionResult deleteMeshOthersDetails(string CustomerCode, string ProjectCode, int JobID, int BBSID, int MeshID)
        {
            var oldBar = db.CTSMESHOthersDetails.Find(CustomerCode, ProjectCode, JobID, BBSID, MeshID);
            if (oldBar != null)
            {
                db.CTSMESHOthersDetails.Remove(oldBar);
                db.SaveChanges();
            }

            return Json(true);
            //return RedirectToAction("Index");
        }

        //getShapeImagesByCat
        [HttpGet]
        [Route("/getShapeImagesByCat_beam/{ShapeCategory}")]
        // [ValidateAntiForgeryHeader]
        public ActionResult getShapeImagesByCat(string ShapeCategory)
        {
            List<string> lShapeList = new List<string>();
            List<string> lShapeFileNameList = new List<string>();
            List<byte[]> lShapeImageList = new List<byte[]>();
            string lShapeCode = "";
            string lShapeFileName = "";
            byte[] lShapeImage;

            var lCmd = new SqlCommand();
            var lNDSCon = new SqlConnection();
            SqlDataReader lRst;

            if (ShapeCategory != null)
            {
                var lProcessObj = new ProcessController();
                if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
                {
                    if (ShapeCategory == "BEAM")
                    {
                        lCmd.CommandText =
                        "SELECT chrShapeCode, vchImage " +
                        "FROM dbo.ShapeMaster " +
                        "WHERE vchMeshShapeGroup like '%beam%' " +
                        "AND tntStatusID = 1 " +
                        "ORDER BY chrShapeCode ";
                    }
                    else if (ShapeCategory == "COLUMN")
                    {
                        lCmd.CommandText =
                        "SELECT chrShapeCode, vchImage " +
                        "FROM dbo.ShapeMaster " +
                        "WHERE vchMeshShapeGroup like '%column%' " +
                        "AND tntStatusID = 1 " +
                        "ORDER BY chrShapeCode ";
                    }
                    else
                    {
                        lCmd.CommandText =
                        "SELECT chrShapeCode, vchImage " +
                        "FROM dbo.ShapeMaster " +
                        "WHERE vchMeshShapeGroup not like '%beam%' " +
                        "AND  vchMeshShapeGroup not like '%column%' " +
                        "AND chrShapeCode not like 'A%' " +
                        "AND chrShapeCode not like 'N%' " +
                        "AND chrShapeCode not like 'P%' " +
                        "AND chrShapeCode not like 'T%' " +
                        "AND chrShapeCode not like 'CR%' " +
                        "AND tntStatusID = 1 " +
                        "AND vchMeshShapeGroup > '' " +
                        "AND vchImage > '' " +
                        "ORDER BY chrShapeCode ";
                    }

                    lCmd.Connection = lNDSCon;
                    lCmd.CommandTimeout = 300;
                    lRst = lCmd.ExecuteReader();
                    if (lRst.HasRows)
                    {
                        while (lRst.Read())
                        {
                            lShapeCode = lRst.GetValue(0) == DBNull.Value ? "" : lRst.GetString(0).Trim();
                            lShapeFileName = lRst.GetValue(1) == DBNull.Value ? "" : lRst.GetString(1).Trim();
                            if (lShapeCode.Length > 0)
                            {
                                lShapeList.Add(lShapeCode);
                                lShapeFileNameList.Add(lShapeFileName);
                            }
                        }
                    }
                    lRst.Close();

                    lProcessObj.CloseNDSConnection(ref lNDSCon);
                    lProcessObj = null;
                }

                if (lShapeList.Count > 0)
                {
                    for (int i = 0; i < lShapeList.Count; i++)
                    {
                        lShapeImage = new byte[0];
                        lShapeFileName = lShapeFileNameList[i];
                        if (lShapeFileName != null)
                        {
                            string lFullName = Path.Combine("D:\\images\\Shapes\\" + lShapeFileName);

                            if (System.IO.File.Exists(lFullName))
                            {
                                lShapeImage = System.IO.File.ReadAllBytes(lFullName);
                            }
                        }
                        lShapeImageList.Add(lShapeImage);
                    }
                }
            }

            var lReturn = new List<ShapeImageListModels>();

            if (lShapeList.Count > 0)
            {
                for (int i = 0; i < lShapeList.Count; i++)
                {
                    lReturn.Add(new ShapeImageListModels
                    {
                        shapeCode = lShapeList[i],
                        shapeImage = lShapeImageList[i]
                    });
                }
            }

            //return new JsonResult()
            //{
            //    Data = lReturn,
            //    MaxJsonLength = Int32.MaxValue,
            //    JsonRequestBehavior = JsonRequestBehavior.AllowGet
            //};
            return Ok(lReturn);
        }

        //get product information 
        [HttpGet]
        [Route("/getProductList_beam/{ProductCategory}")]
        //   [ValidateAntiForgeryHeader]
        public ActionResult getProductList(string ProductCategory)
        {
            var lCmd = new SqlCommand();
            var lNDSCon = new SqlConnection();
            SqlDataReader lRst;

            string lProdCode = "";
            decimal lMWDia = 0;
            int lMWSpacing = 0;
            decimal lCWDia = 0;
            int lCWSpacing = 0;
            decimal lMass = 0;
            string lTwinInd = "";
            List<Dictionary<string, object>> lReturn = new List<Dictionary<string, object>>();
            Dictionary<string, object> lDic = new Dictionary<string, object>();

            if (ProductCategory != null)
            {
                var lProcessObj = new ProcessController();
                if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
                {
                    if (ProductCategory == "BEAM")
                    {
                        lCmd.CommandText =
                        "SELECT " +
                        "vchProductCode " +
                        ",decMWDiameter " +
                        ",intMWSpace " +
                        ",decCWDiameter " +
                        ",intCWSpace " +
                        ",decWeightArea " +
                        ",chrTwinIndicator " +
                        "FROM dbo.ProductCodeMaster " +
                        "WHERE vchproductCode like 'WS%' " +
                        "AND tntStatusId = 1 " +
                        "ORDER BY vchproductCode ";
                    }
                    else if (ProductCategory == "COLUMN")
                    {
                        lCmd.CommandText =
                        "SELECT " +
                        "vchProductCode " +
                        ",decMWDiameter " +
                        ",intMWSpace " +
                        ",decCWDiameter " +
                        ",intCWSpace " +
                        ",decWeightArea " +
                        ",chrTwinIndicator " +
                        "FROM dbo.ProductCodeMaster " +
                        "WHERE vchproductCode like 'WL%' " +
                        "AND tntStatusId = 1 " +
                        "ORDER BY vchproductCode ";
                    }
                    else
                    {
                        lCmd.CommandText =
                        "SELECT " +
                        "vchProductCode " +
                        ",decMWDiameter " +
                        ",intMWSpace " +
                        ",decCWDiameter " +
                        ",intCWSpace " +
                        ",decWeightArea " +
                        ",chrTwinIndicator " +
                        "FROM dbo.ProductCodeMaster " +
                        "WHERE ((vchProductCode > 'WA1' " +
                        "AND vchProductCode < 'WA9') " +
                        "OR (vchProductCode > 'WAD1' " +
                        "AND vchProductCode < 'WAD9') " +
                        "OR (vchProductCode > 'WAE1' " +
                        "AND vchProductCode < 'WAE9') " +
                        "OR (vchProductCode > 'WB1' " +
                        "AND vchProductCode < 'WB9') " +
                        "OR (vchProductCode > 'WD1' " +
                        "AND vchProductCode < 'WD9' " +
                        "AND len(vchProductCode) < 5) " +
                        "OR (vchProductCode > 'WDA1' " +
                        "AND vchProductCode < 'WDA9') " +
                        "OR (vchProductCode > 'WDE1' " +
                        "AND vchProductCode < 'WDE9') " +
                        "OR (vchProductCode > 'WE1' " +
                        "AND vchProductCode < 'WE9') " +
                        "OR (vchProductCode > 'WEA1' " +
                        "AND vchProductCode < 'WEA9') " +
                        "OR (vchProductCode > 'WED1' " +
                        "AND vchProductCode < 'WED9') " +
                        "OR (vchProductCode > 'WF1' " +
                        "AND vchProductCode < 'WF9') " +
                        "OR vchProductCode like 'WH%') " +
                        "AND vchProductCode not like '%-50' " +
                        "AND vchProductCode not like '%-BT' " +
                        "AND vchProductCode not like '%A' " +
                        "AND len(vchProductCode) < 10 " +
                        "AND decMWDiameter <= 13 " +
                        "AND decCWDiameter <= 13 " +
                        "AND intMWSpace >= 100 " +
                        "AND intCWSpace >= 100 " +
                        "ORDER BY intMWSpace, intCWSpace, decMWDiameter, decCWDiameter ";
                    }

                    lCmd.Connection = lNDSCon;
                    lCmd.CommandTimeout = 300;
                    lRst = lCmd.ExecuteReader();
                    if (lRst.HasRows)
                    {
                        while (lRst.Read())
                        {
                            lProdCode = lRst.GetValue(0) == DBNull.Value ? "" : lRst.GetString(0).Trim();
                            lMWDia = lRst.GetValue(1) == DBNull.Value ? 0 : lRst.GetDecimal(1);
                            lMWSpacing = lRst.GetValue(2) == DBNull.Value ? 0 : lRst.GetInt32(2);
                            lCWDia = lRst.GetValue(3) == DBNull.Value ? 0 : lRst.GetDecimal(3);
                            lCWSpacing = lRst.GetValue(4) == DBNull.Value ? 0 : lRst.GetInt32(4);
                            lMass = lRst.GetValue(5) == DBNull.Value ? 0 : lRst.GetDecimal(5);
                            lTwinInd = lRst.GetValue(6) == DBNull.Value ? "" : lRst.GetString(6).Trim();

                            lDic = new Dictionary<string, object>();

                            lDic.Add("ProductCode", lProdCode);
                            lDic.Add("ProdMWDia", lMWDia);
                            lDic.Add("ProdMWSpacing", lMWSpacing);
                            lDic.Add("ProdCWDia", lCWDia);
                            lDic.Add("ProdCWSpacing", lCWSpacing);
                            lDic.Add("ProdMass", lMass);
                            lDic.Add("ProdTwinInd", lTwinInd);

                            lReturn.Add(lDic);
                        }
                    }
                    lRst.Close();

                    lProcessObj.CloseNDSConnection(ref lNDSCon);
                }

                lProcessObj = null;
            }


            // return Json(lReturn, JsonRequestBehavior.AllowGet);
            return Ok(lReturn);
        }

        //get product information 
        [HttpPost]
        [Route("/getProductInfo_beam/{ProductCode}")]
        //[ValidateAntiForgeryHeader]
        public ActionResult getProductInfo(string ProductCode)
        {
            var lCmd = new SqlCommand();
            var lNDSCon = new SqlConnection();
            SqlDataReader lRst;

            string lProdCode = "";
            decimal lMWDia = 0;
            int lMWSpacing = 0;
            decimal lCWDia = 0;
            int lCWSpacing = 0;
            decimal lMass = 0;
            int lMinFactor = 0;
            int lMaxFactor = 0;
            string lTwinInd = "";

            if (ProductCode != null)
            {

                var lProcessObj = new ProcessController();
                if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
                {
                    lCmd.CommandText =
                        "SELECT " +
                        "vchProductCode " +
                        ",decMWDiameter " +
                        ",intMWSpace " +
                        ",decCWDiameter " +
                        ",intCWSpace " +
                        ",decWeightArea " +
                        ",intMinLinkFactor " +
                        ",intMaxLinkFactor " +
                        ",chrTwinIndicator " +
                        "FROM dbo.ProductCodeMaster " +
                        "WHERE vchProductCode = '" + ProductCode + "' ";

                    lCmd.Connection = lNDSCon;
                    lCmd.CommandTimeout = 300;
                    lRst = lCmd.ExecuteReader();
                    if (lRst.HasRows)
                    {
                        if (lRst.Read())
                        {
                            lProdCode = lRst.GetValue(0) == DBNull.Value ? "" : lRst.GetString(0).Trim();
                            lMWDia = lRst.GetValue(1) == DBNull.Value ? 0 : lRst.GetDecimal(1);
                            lMWSpacing = lRst.GetValue(2) == DBNull.Value ? 0 : lRst.GetInt32(2);
                            lCWDia = lRst.GetValue(3) == DBNull.Value ? 0 : lRst.GetDecimal(3);
                            lCWSpacing = lRst.GetValue(4) == DBNull.Value ? 0 : lRst.GetInt32(4);
                            lMass = lRst.GetValue(5) == DBNull.Value ? 0 : lRst.GetDecimal(5);
                            lMinFactor = lRst.GetValue(6) == DBNull.Value ? 0 : lRst.GetInt32(6);
                            lMaxFactor = lRst.GetValue(7) == DBNull.Value ? 0 : lRst.GetInt32(7);
                            lTwinInd = lRst.GetValue(8) == DBNull.Value ? "" : lRst.GetString(8).Trim();
                        }
                    }
                    lRst.Close();


                    lProcessObj.CloseNDSConnection(ref lNDSCon);
                }

                lProcessObj = null;

            }

            var lProdRecord = new
            {
                ProductCode = lProdCode,
                ProdMWDia = lMWDia,
                ProdMWSpacing = lMWSpacing,
                ProdCWDia = lCWDia,
                ProdCWSpacing = lCWSpacing,
                ProdMass = lMass,
                ProdMinFactor = lMinFactor,
                ProdMaxFactor = lMaxFactor,
                ProdTwinInd = lTwinInd
            };

            //   return Json(lProdRecord, JsonRequestBehavior.AllowGet);
            return Ok(lProdRecord);
        }

        //get Shape information 
        [HttpPost]
        [Route("/getShapeInfo_beam/{CustomerCode}/{ProjectCode}/{JobID}/{ShapeCode}")]
        //  [ValidateAntiForgeryHeader]
        public ActionResult getShapeInfo(string CustomerCode, string ProjectCode, int JobID, string ShapeCode)
        {
            var lShapeRecord = new CTSMESHShapeModels();
            var lCmd = new SqlCommand();
            var lNDSCon = new SqlConnection();
            SqlDataReader lRst;

            int lShapeID = 0;
            string lShapeFileName = "";
            string lShapecategory = "";
            string lShapeParams = "";
            string lEditParams = "";
            string lMaxValues = "";
            string lMinValues = "";
            string lParamTypes = "";
            string lWireTypes = "";
            bool lCreepMO1 = false;
            bool lCreepCO1 = false;


            if (ShapeCode != null)
            {

                var lProcessObj = new ProcessController();
                if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
                {
                    lCmd.CommandText =
                    "SELECT intShapeID, vchImage, bitCreepDeductAtMo1, bitCreepDeductAtCo1 " +
                    "FROM dbo.ShapeMaster " +
                    "WHERE chrShapeCode = '" + ShapeCode + "' ";

                    lCmd.Connection = lNDSCon;
                    lCmd.CommandTimeout = 300;
                    lRst = lCmd.ExecuteReader();
                    if (lRst.HasRows)
                    {
                        if (lRst.Read())
                        {
                            lShapeID = lRst.GetValue(0) == DBNull.Value ? 0 : lRst.GetInt32(0);
                            lShapeFileName = lRst.GetValue(1) == DBNull.Value ? "" : lRst.GetString(1).Trim();
                            lCreepMO1 = lRst.GetValue(2) == DBNull.Value ? false : lRst.GetBoolean(2);
                            lCreepCO1 = lRst.GetValue(3) == DBNull.Value ? false : lRst.GetBoolean(3);
                        }
                    }
                    lRst.Close();

                    lCmd.CommandText =
                    "SELECT chrParamName, intMinLen, intMaxLen, chrAngleType, bitEdit, chrWireType " +
                    "FROM dbo.ShapeParamDetails " +
                    "WHERE intShapeID = " + lShapeID.ToString() + " " +
                    "AND (bitEdit = 1 " +
                    "OR bitVisible = 1) " +
                    "ORDER BY UPPER(chrParamName) ";

                    lCmd.Connection = lNDSCon;
                    lCmd.CommandTimeout = 300;
                    lRst = lCmd.ExecuteReader();
                    if (lRst.HasRows)
                    {
                        while (lRst.Read())
                        {
                            if (lShapeParams == "")
                            {
                                lShapeParams = lRst.GetValue(0) == DBNull.Value ? "" : lRst.GetString(0).Trim().ToUpper();
                            }
                            else
                            {
                                lShapeParams = lShapeParams + "," + (lRst.GetValue(0) == DBNull.Value ? "" : lRst.GetString(0).Trim().ToUpper());
                            }
                            if ((lRst.GetValue(4) == DBNull.Value ? false : lRst.GetBoolean(4)) == true)
                            {
                                if (lEditParams == "")
                                {
                                    lEditParams = lRst.GetValue(0) == DBNull.Value ? "" : lRst.GetString(0).Trim().ToUpper();
                                    lMinValues = (lRst.GetValue(1) == DBNull.Value ? 0 : lRst.GetInt32(1)).ToString();
                                    lMaxValues = (lRst.GetValue(2) == DBNull.Value ? 0 : lRst.GetInt32(2)).ToString();
                                    lParamTypes = lRst.GetValue(3) == DBNull.Value ? "" : lRst.GetString(3).Trim();
                                    lWireTypes = lRst.GetValue(5) == DBNull.Value ? "" : lRst.GetString(5).Trim();
                                }
                                else
                                {
                                    lEditParams = lEditParams + "," + (lRst.GetValue(0) == DBNull.Value ? "" : lRst.GetString(0).Trim().ToUpper());
                                    lMinValues = lMinValues + "," + (lRst.GetValue(1) == DBNull.Value ? 0 : lRst.GetInt32(1)).ToString();
                                    lMaxValues = lMaxValues + "," + (lRst.GetValue(2) == DBNull.Value ? 0 : lRst.GetInt32(2)).ToString();
                                    lParamTypes = lParamTypes + "," + (lRst.GetValue(3) == DBNull.Value ? "" : lRst.GetString(3).Trim());
                                    lWireTypes = lWireTypes + "," + (lRst.GetValue(5) == DBNull.Value ? "" : lRst.GetString(5).Trim());
                                }
                            }
                        }
                    }
                    lRst.Close();

                    lProcessObj.CloseNDSConnection(ref lNDSCon);
                }

                lProcessObj = null;

                string lFullName = Path.Combine("D:\\PROJECT_FOLDER\\Detailing - NDS\\One Detaling and Order System\\ODOS_API\\Microservices\\OrderService\\images\\Shapes" + lShapeFileName);
                System.IO.FileStream fs = System.IO.File.OpenRead(lFullName);
                byte[] lImage = new byte[fs.Length];
                int lByteCT = fs.Read(lImage, 0, lImage.Length);

                lShapeRecord = new CTSMESHShapeModels
                {
                    MeshShapeCode = ShapeCode,
                    MeshShapeCategory = lShapecategory,
                    MeshShapeMinValues = lMinValues,
                    MeshShapeMaxValues = lMaxValues,
                    MeshShapeParameters = lShapeParams,
                    MeshEditParameters = lEditParams,
                    MeshShapeParamTypes = lParamTypes,
                    MeshShapeWireTypes = lWireTypes,
                    MeshShapeImage = lImage,
                    MeshCreepMO1 = lCreepMO1,
                    MeshCreepCO1 = lCreepCO1,
                };
            }
            //return Json(lShapeRecord, JsonRequestBehavior.AllowGet);
            return Ok(lShapeRecord);
        }

        //get shape List 
        [HttpPost]
        [Route("/getShapeCodeList_beam/{CustomerCode}/{ProjectCode}/{CouplerType}")]
        //[ValidateAntiForgeryHeader]
        public ActionResult getShapeCodeList(string CustomerCode, string ProjectCode, string CouplerType)
        {
            var lCoupler = "ZZZNO";
            if (CouplerType == "N-Splice")
            {
                lCoupler = "N";
            }
            if (CouplerType == "E-Splice(N)" || CouplerType == "E-Splice(S)")
            {
                lCoupler = "E";
            }
            var content = (from p in db.Shape
                           where p.shapeStatus == "Active" &&
                           ((p.shapeCategory != "E" &&
                           p.shapeCategory != "N") ||
                           p.shapeCategory == lCoupler)
                           orderby p.shapeCode
                           select new { p.shapeCode, p.shapeCategory }).ToList();

            // return Json(content, JsonRequestBehavior.AllowGet);
            return Ok(content);
        }

        //get shape List 
        [HttpPost]
        [Route("/getCustomerShapeCodeList_beam/{CustomerCode}/{ProjectCode}")]
        // [ValidateAntiForgeryHeader]
        public ActionResult getCustomerShapeCodeList(string CustomerCode, string ProjectCode)
        {
            var content = (from p in db.CustomerShape
                           where p.CustomerCode == CustomerCode &&
                           p.ProjectCode == ProjectCode &&
                           p.ShapeStatus == "Active"
                           orderby p.shapeCode
                           select new { p.shapeCode, p.shapeCategory }).ToList();
            // return Json(content, JsonRequestBehavior.AllowGet);
            return Ok(content);
        }

        //Print Shapes List 
        [HttpPost]
        [Route("/printShapes_beam/{ShapeCategory}")]
        //  [ValidateAntiForgeryHeader]
        public ActionResult printShapes(string ShapeCategory)
        {
            Reports service = new Reports();
            var bPDF = service.rptMeshShapes(ShapeCategory);

            //return new JsonResult()
            //{
            //    Data = bPDF,
            //    MaxJsonLength = Int32.MaxValue,
            //    JsonRequestBehavior = JsonRequestBehavior.AllowGet
            //};
            return Ok();
        }

        //Print Shapes List 
        [HttpPost]
        [Route("/printProduct_beam/{ProductCategory}")]
        //[ValidateAntiForgeryHeader]
        public ActionResult printProduct(string ProductCategory)
        {
            Reports service = new Reports();
            var bPDF = service.rptMeshProduct(ProductCategory);

            //return new JsonResult()
            //{
            //    Data = bPDF,
            //    MaxJsonLength = Int32.MaxValue,
            //    JsonRequestBehavior = JsonRequestBehavior.AllowGet
            //};
            return Ok();
        }

        //get MESH List 
        [HttpPost]
        [Route("/printOrderDetail_beam/{CustomerCode}/{ProjectCode}/{JobID}")]
        //[ValidateAntiForgeryHeader]
        public ActionResult printOrderDetail(string CustomerCode, string ProjectCode, int JobID)
        {
            Reports service = new Reports();
            int lPage = 0;
            var bPDF = service.rptCTSMESHOrderDetails(CustomerCode, ProjectCode, JobID, "N", ref lPage);
            // bPDF = service.rptCTSMESHOrderDetails(CustomerCode, ProjectCode, JobID, "N", ref lPage);

            //var bPDF = service.rptOrderDetailsImage(CustomerCode, ProjectCode, JobID, "N", ref lPage);
            //bPDF = service.rptOrderDetailsImage(CustomerCode, ProjectCode, JobID, "N", ref lPage);

            // return Json(bPDF, JsonRequestBehavior.AllowGet);
            return Ok();
        }

        [HttpGet]
        [Route("/GetPDF_beam/{pHTML}")]
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

        //get BBS List at Order Summary
        [HttpPost]
        [Route("/getBBSNSH_beam/{CustomerCode}/{ProjectCode}/{JobID}/{WBS1}/{WBS2}/{WBS3}")]
        // [ValidateAntiForgeryHeader]
        public ActionResult getBBSNSH(string CustomerCode, string ProjectCode, int JobID, string WBS1, string WBS2, string WBS3)
        {
            var lReturn = new List<CTSMESHBBSNSHModels>();
            var lNewBBS = new CTSMESHBBSNSHModels();
            var lCmd = new SqlCommand();
            var lNDSCon = new SqlConnection();
            SqlDataReader lRst;
            string lProdCategory = "";
            string lStrucEle = "";
            int lID = 0;


            if (CustomerCode != null && ProjectCode != null)
            {
                if (CustomerCode.Length > 0 && ProjectCode.Length > 0)
                {
                    try
                    {
                        var lJobAdv = db.CTSMESHJobAdvice.Find(CustomerCode, ProjectCode, JobID);
                        if (lJobAdv != null)
                        {
                            if (lJobAdv.OrderStatus == "New" || lJobAdv.OrderStatus == "Created" || lJobAdv.OrderStatus == "" || lJobAdv.OrderStatus == null)
                            {
                                lCmd.CommandText =
                                "SELECT H.intPostHeaderId, " +
                                "BBS_DESC, " +
                                //"isNull(numPostedWeight,0) + isNull(numPostedCappingWeight,0) + isNull(numPostedClinkWeight,0) as PostedWeight, " +
                                //"isNull(intPostedQty,0) + isNull(intPostedCappingQty,0) + isNull(intPostedCLinkQty,0) as PostedQty, " +
                                "isNull(numPostedWeight,0) * 1000  as PostedWeight, " +
                                "isNull(intPostedQty,0) as PostedQty, " +
                                "vchStructureElementType, " +
                                "datPostedDate, " +
                                "(SELECT vchLoginId FROM dbo.NDSUsers WHERE intUserId = H.intPostedBy) " +
                                "from dbo.BBSPostHeader H, dbo.WBSElements E, dbo.WBS W, " +
                                "dbo.WBSElementsDetails D, dbo.SAPProjectMaster P, " +
                                "dbo.StructureElementMaster S " +
                                "where e.intWBSId = W.intWBSId " +
                                "and E.intWBSElementId = D.intWBSElementId " +
                                "and W.intProjectid = P.intProjectid " +
                                "and D.intStructureElementTypeId = S.intStructureElementTypeId " +
                                "and P.vchProjectCode = '" + ProjectCode + "' " +
                                "and D.intConfirm  = 1 " +
                                "and H.intWBSElementId = E.intWBSElementId " +
                                "and E.vchWBS1 = '" + WBS1 + "' " +
                                "and E.vchWBS2 = '" + WBS2 + "' " +
                                "and E.vchWBS3 = '" + WBS3 + "' " +
                                "and H.tntStatusID = 3 " +
                                "and H.numPostedWeight > 0 " +
                                "AND NOT EXISTS(SELECT BBSNDSPostID " +
                                "FROM dbo.OESCTSMESHBBS A, dbo.PostGroupMarkingDetails B " +
                                "WHERE A.BBSNDSPostID = H.intPostHeaderId " +
                                "AND B.intPostHeaderId = H.intPostHeaderId " +
                                "AND B.intPostHeaderId = A.BBSNDSPostID " +
                                "AND A.BBSNDSGroupMark = B.vchGroupMarkingName) " +
                                "AND NOT EXISTS ( SELECT R.intPostHeaderid FROM dbo.BBSReleaseDetails R  " +
                                "WHERE R.intPostHeaderid = H.intPostHeaderId  " +
                                "AND (R.tntStatusId = 12 OR R.tntStatusId = 14) ) " +
                                "and D.sitProductTypeId in " +
                                "(SELECT sitProductTypeID " +
                                "FROM dbo.ProductTypeMaster " +
                                "WHERE vchProductType = 'MSH' ) " +
                                "Order By datPostedDate ";

                                var lProcessObj = new ProcessController();
                                if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
                                {
                                    lCmd.Connection = lNDSCon;
                                    lCmd.CommandTimeout = 300;
                                    lRst = lCmd.ExecuteReader();
                                    if (lRst.HasRows)
                                    {
                                        lID = 1;
                                        while (lRst.Read())
                                        {
                                            lStrucEle = lRst.GetString(4).Trim();
                                            if (lStrucEle.ToUpper() == "BEAM")
                                            {
                                                lProdCategory = "Beam Stirrup Cage";
                                            }
                                            else if (lStrucEle.ToUpper() == "COLUMN")
                                            {
                                                lProdCategory = "Column Link Cage";
                                            }
                                            else
                                            {
                                                lProdCategory = "CTS MESH";
                                            }
                                            if (lStrucEle.ToUpper() == "SLAB-B")
                                            {
                                                lStrucEle = "Slab Bottom";
                                            }
                                            if (lStrucEle.ToUpper() == "SLAB-T")
                                            {
                                                lStrucEle = "Slab Top";
                                            }

                                            lNewBBS = new CTSMESHBBSNSHModels
                                            {
                                                BBSID = lID,
                                                BBSProdCategory = lProdCategory,
                                                BBSNDSPostID = lRst.GetInt32(0),
                                                BBSDesc = lRst.GetString(1).Trim(),
                                                BBSTotalWT = (decimal)lRst.GetDecimal(2),
                                                BBSTotalPcs = lRst.GetInt32(3),
                                                BBSStrucElem = lStrucEle,
                                                BBSNDSPostDate = lRst.GetDateTime(5),
                                                BBSNDSPostBy = lRst.GetString(6),
                                                BBSDrawing = "N",
                                                BBSDrawingRev = "",
                                                BBSNDSGroupMark = lRst.GetString(4).Trim(),
                                                BBSOrder = true
                                            };
                                            lReturn.Add(lNewBBS);
                                            lID = lID + 1;
                                        }
                                    }
                                    lRst.Close();

                                    if (lReturn.Count > 0)
                                    {
                                        for (int i = 0; i < lReturn.Count; i++)
                                        {
                                            //Check drawing
                                            lCmd.CommandText =
                                            "SELECT Count(DrawingID), isNull(Max(Revision),0) " +
                                            "FROM dbo.OESCTSMESHDrawings " +
                                            "WHERE CustomerCode = '" + CustomerCode + "' " +
                                            "AND ProjectCode = '" + ProjectCode + "' " +
                                            "AND ProdType = 'MSH' " +
                                            "AND StrucElem = '" + lReturn[i].BBSNDSGroupMark + "' " +
                                            "AND WBS1 = '" + WBS1 + "' " +
                                            "AND WBS2 = '" + WBS2 + "' " +
                                            "AND WBS3 = '" + WBS3 + "' ";

                                            lCmd.Connection = lNDSCon;
                                            lCmd.CommandTimeout = 300;
                                            lRst = lCmd.ExecuteReader();
                                            if (lRst.HasRows)
                                            {
                                                if (lRst.Read())
                                                {
                                                    if (lRst.GetInt32(0) > 0)
                                                    {
                                                        lReturn[i].BBSDrawing = lRst.GetInt32(0).ToString();
                                                        lReturn[i].BBSDrawingRev = lRst.GetInt32(1).ToString();
                                                    }
                                                }
                                            }

                                            lRst.Close();

                                            //get NDS Group Marking
                                            lCmd.CommandText =
                                            "SELECT vchGroupMarkingName " +
                                            "FROM dbo.PostGroupMarkingDetails " +
                                            "WHERE intPostHeaderID = " + lReturn[i].BBSNDSPostID + " ";

                                            lCmd.Connection = lNDSCon;
                                            lCmd.CommandTimeout = 300;
                                            lRst = lCmd.ExecuteReader();
                                            if (lRst.HasRows)
                                            {
                                                string lGroupMarking = "";
                                                while (lRst.Read())
                                                {
                                                    if (lGroupMarking.Length == 0)
                                                    {
                                                        lGroupMarking = lRst.GetString(0).Trim();
                                                    }
                                                    else
                                                    {
                                                        lGroupMarking = lGroupMarking + "," + lRst.GetString(0).Trim();
                                                    }
                                                }
                                                lReturn[i].BBSNDSGroupMark = lGroupMarking;

                                            }
                                            lRst.Close();
                                        }
                                    }
                                    lProcessObj.CloseNDSConnection(ref lNDSCon);
                                }

                                lProcessObj = null;
                            }
                            else
                            {
                                lReturn = (from p in db.CTSMESHBBSNSH
                                           where p.CustomerCode == CustomerCode &&
                                           p.ProjectCode == ProjectCode &&
                                           p.JobID == JobID
                                           orderby p.BBSID
                                           select p).ToList();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, "Server Error: " + ex.Message);
                    }

                    lCmd = null;
                    lNDSCon = null;
                    lRst = null;
                }

            }
            // return Json(lReturn, JsonRequestBehavior.AllowGet);
            return Ok(lReturn);
        }

        //get WBS Status data
        //[HttpPost]
        //[Route("/getScheduleData_beam/{CustomerCode}/{ProjectCode}/{ProdType}/{StruElem}/{WBS1}/{DataRange}/{Presentation}")]
        ////[ValidateAntiForgeryHeader]
        //public ActionResult getScheduleData(string CustomerCode, string ProjectCode, string ProdType, string StruElem, string WBS1, string DataRange, string Presentation)
        //{
        //    var lReturn = new List<WBSStatusModels>();
        //    var lNewWBS = new WBSStatusModels();
        //    var lCmd = new SqlCommand();
        //    var lNDSCon = new SqlConnection();
        //    SqlDataReader lRst;

        //    var lCISCon = new OracleConnection();
        //    var lOraCmd = new OracleCommand();
        //    OracleDataReader lOraRst;

        //    string lSelect = "";
        //    string lWBS3 = "";
        //    string lWBS3Group = "";
        //    string lWBS3Sub = "";
        //    string lWBS3Order = "";
        //    int lID = 0;

        //    if (StruElem == "Slab Top") StruElem = "SLAB-T";
        //    if (StruElem == "Slab Bottom") StruElem = "SLAB-B";

        //    if (CustomerCode != null && ProjectCode != null)
        //    {
        //        if (CustomerCode.Length > 0 && ProjectCode.Length > 0)
        //        {
        //            try
        //            {
        //                if (DataRange == "ALL")
        //                {
        //                    lSelect = " ";
        //                }
        //                else if (DataRange == "WBS1")
        //                {
        //                    lSelect = "and  E.vchWBS1 = '" + WBS1 + "' ";
        //                }
        //                else if (DataRange == "SE")
        //                {
        //                    lSelect = "and D.intStructureElementTypeId in " +
        //                    "(SELECT intStructureElementTypeId " +
        //                    "FROM dbo.StructureElementMaster " +
        //                    "WHERE vchStructureElementType = '" + StruElem + "') ";
        //                }
        //                else if (DataRange == "BOTH")
        //                {
        //                    lSelect = "and D.intStructureElementTypeId in " +
        //                    "(SELECT intStructureElementTypeId " +
        //                    "FROM dbo.StructureElementMaster " +
        //                    "WHERE vchStructureElementType = '" + StruElem + "') " +
        //                    "and  E.vchWBS1 = '" + WBS1 + "' ";
        //                }

        //                if (Presentation == "ALL")
        //                {
        //                    lWBS3 = "E.vchWBS3, ";
        //                    lWBS3Group = ", E.vchWBS3, R.intSalesOrderID, E.intWBSElementId, H.tntStatusId, R.tntStatusId ";
        //                    lWBS3Sub = "AND DR.WBS3 = E.vchWBS3 ";
        //                    lWBS3Order = "E.vchWBS3, ";
        //                }
        //                else if (Presentation == "MERGEWBS3")
        //                {
        //                    lWBS3 = "case when CHARINDEX('-', E.vchWBS3) > 1 then Substring(E.vchWBS3, 1, CHARINDEX('-', E.vchWBS3)-1) else E.vchWBS3 end, ";
        //                    lWBS3Group = ", case when CHARINDEX('-', E.vchWBS3) > 1 then Substring(E.vchWBS3, 1, CHARINDEX('-', E.vchWBS3)-1) else E.vchWBS3 end ";
        //                    lWBS3Sub = "and case when CHARINDEX('-', DR.WBS3) > 1 then Substring(DR.WBS3, 1, CHARINDEX('-', DR.WBS3)-1) else DR.WBS3 end = " +
        //                        "case when CHARINDEX('-', E.vchWBS3) > 1 then Substring(E.vchWBS3, 1, CHARINDEX('-', E.vchWBS3)-1) else E.vchWBS3 end ";
        //                    lWBS3Order = "case when CHARINDEX('-', E.vchWBS3) > 1 then Substring(E.vchWBS3, 1, CHARINDEX('-', E.vchWBS3)-1) else E.vchWBS3 end, ";
        //                }
        //                else if (Presentation == "SUMMARYWBS2")
        //                {
        //                    lWBS3 = "'Summary', ";
        //                    lWBS3Group = " ";
        //                    lWBS3Sub = " ";
        //                    lWBS3Order = " ";
        //                }

        //                lCmd.CommandText =
        //                "SELECT " +
        //                "(SELECT UPPER(vchStructureElementType) " +
        //                "FROM dbo.StructureElementMaster " +
        //                "WHERE intStructureElementTypeId = D.intStructureElementTypeId), " +
        //                "E.vchWBS1, " +
        //                "E.vchWBS2, " +
        //                lWBS3 +
        //                "isnull(Max(H.tntStatusId), 0), " +
        //                "isNull(Max(R.tntStatusId), 0), " +
        //                "SUM(isNull(numPostedWeight, 0) + isNull(numPostedCappingWeight, 0) + isNull(numPostedClinkWeight, 0)) as PostedWeight, " +
        //                "(SELECT COUNT(*) from dbo.OESCTSMESHDrawings DR, dbo.StructureElementMaster SE " +
        //                "WHERE DR.StrucElem = SE.vchStructureElementType " +
        //                "AND DR.ProjectCode = P.vchProjectCode " +
        //                "AND SE.intStructureElementTypeId = max(D.intStructureElementTypeId) " +
        //                "AND DR.ProdType = '" + ProdType + "' " +
        //                "AND DR.WBS1 = E.vchWBS1 " +
        //                "AND DR.WBS2 = E.vchWBS2 " +
        //                lWBS3Sub +
        //                ") AS DrawingCT, " +
        //                "(SELECT ord_req_no FROM dbo.SAP_REQUEST_HDR " +
        //                "WHERE IDENTITY_NO = max(R.intSalesOrderID) ) as SORNo " +
        //                "FROM dbo.WBSElements E, dbo.WBS W, " +
        //                "dbo.WBSElementsDetails D, dbo.SAPProjectMaster P, " +
        //                "dbo.BBSPostHeader H left outer join dbo.BBSReleaseDetails R  " +
        //                "ON H.intPostHeaderid = R.intPostHeaderid and R.tntStatusId <> 14 " +
        //                "WHERE e.intWBSId = W.intWBSId " +
        //                "AND H.intWBSElementId = E.intWBSElementId " +
        //                "and E.intWBSElementId = D.intWBSElementId " +
        //                "and W.intProjectid = P.intProjectid " +
        //                "and D.intStructureElementTypeId > 0 " +
        //                "AND (H.tntStatusId <> 3 " +
        //                "OR (H.tntStatusId = 3 " +
        //                "AND H.numPostedWeight > 0)) " +
        //                "AND NOT EXISTS(SELECT R1.intPostHeaderid FROM dbo.BBSReleaseDetails R1  " +
        //                "WHERE R1.intPostHeaderid = H.intPostHeaderId  AND  R1.tntStatusId = 14) " +
        //                "and D.intConfirm = 1 " +
        //                "and P.vchProjectCode = '" + ProjectCode + "' " +
        //                "and D.sitProductTypeId in " +
        //                "(SELECT sitProductTypeID " +
        //                "FROM dbo.ProductTypeMaster " +
        //                "WHERE vchProductType = '" + ProdType + "') " +
        //                lSelect +
        //                "GROUP BY P.vchProjectCode, " +
        //                "D.intStructureElementTypeId, " +
        //                "E.vchWBS1, " +
        //                "E.vchWBS2 " +
        //                lWBS3Group +
        //                "Order By vchWBS1, " +
        //                "(case when PATINDEX('B[0-9]%', vchWBS2) > 0 then cast(left(substring(vchWBS2, (PATINDEX('%[0-9]%',vchWBS2)),len(vchWBS2)), " +
        //                "PATINDEX('%[^0-9]%', substring(vchWBS2 + 'z', (PATINDEX('%[0-9]%', vchWBS2)), " +
        //                "len(vchWBS2)) + 'z') ) as int)    " +
        //                "else (case when PATINDEX('FDN[0-9]%', vchWBS2) > 0 then cast(left(substring(vchWBS2, (PATINDEX('%[0-9]%',vchWBS2)),len(vchWBS2)), " +
        //                "PATINDEX('%[^0-9]%', substring(vchWBS2, (PATINDEX('%[0-9]%', vchWBS2)), " +
        //                "len(vchWBS2)) + 'z') ) as int) else  " +
        //                "case when(PATINDEX('B%', vchWBS2) > 0 OR PATINDEX('FDN%', vchWBS2) > 0) then 98  else 99 end " +
        //                "end) end) DESC, " +
        //                " " +
        //                "(case when PATINDEX('[^0-9]%',vchWBS2) > 0 then vchWBS2 " +
        //                "else '' end) DESC,    " +
        //                " " +
        //                "(CASE WHEN PATINDEX('%[0-9]%',vchWBS2) > 0 THEN cast(left(substring(vchWBS2, (PATINDEX('%[0-9]%',vchWBS2)),len(vchWBS2)), " +
        //                "PATINDEX('%[^0-9]%', substring(vchWBS2, (PATINDEX('%[0-9]%', vchWBS2) + 1), " +
        //                "len(vchWBS2)) + 'z') ) as int)   " +
        //                "ELSE " +
        //                "cast(left(substring(vchWBS2, (PATINDEX('%[0-9]%', vchWBS2)), len(vchWBS2)), " +
        //                "PATINDEX('%[^0-9]%', substring(vchWBS2, (PATINDEX('%[0-9]%', vchWBS2)), " +
        //                "len(vchWBS2)) + 'z') - 1) as int) " +
        //                "END) DESC,  " +
        //                "vchWBS2 DESC, " +
        //                lWBS3Order +
        //                "1,  " +
        //                "5 DESC, 6 ";

        //                var lProcessObj = new ProcessController();
        //                if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
        //                {
        //                    lCmd.Connection = lNDSCon;
        //                    lCmd.CommandTimeout = 300;
        //                    lRst = lCmd.ExecuteReader();
        //                    if (lRst.HasRows)
        //                    {
        //                        lID = 1;
        //                        while (lRst.Read())
        //                        {
        //                            var lHeaderStatus = lRst.GetByte(4);
        //                            var lReleaseStatus = lRst.GetByte(5);
        //                            var lStatus = "Working";
        //                            if (lReleaseStatus == 12)
        //                            {
        //                                lStatus = "Ordered";
        //                            }
        //                            else
        //                            {
        //                                if (lHeaderStatus == 3)
        //                                {
        //                                    lStatus = "Ready";
        //                                }
        //                            }
        //                            lNewWBS = new WBSStatusModels
        //                            {
        //                                ProjectCode = ProjectCode,
        //                                WBS1 = lRst.GetValue(1) == DBNull.Value ? "" : lRst.GetString(1).Trim(),
        //                                WBS2 = lRst.GetValue(2) == DBNull.Value ? "" : lRst.GetString(2).Trim(),
        //                                WBS3 = lRst.GetValue(3) == DBNull.Value ? "" : lRst.GetString(3).Trim(),
        //                                ProductType = "MSH",
        //                                StructureElem = lRst.GetValue(0) == DBNull.Value ? "" : lRst.GetString(0).Trim(),
        //                                WBSStatus = lStatus,
        //                                WBSTonnage = lRst.GetValue(6) == DBNull.Value ? 0 : lRst.GetDecimal(6),
        //                                DrawingCount = lRst.GetValue(7) == DBNull.Value ? 0 : lRst.GetInt32(7),
        //                                SORNo = lRst.GetValue(8) == DBNull.Value ? "" : lRst.GetString(8).Trim()
        //                            };
        //                            lReturn.Add(lNewWBS);
        //                            lID = lID + 1;
        //                        }
        //                    }
        //                    lRst.Close();

        //                    lProcessObj.CloseNDSConnection(ref lNDSCon);
        //                }

        //                lProcessObj = null;

        //            }
        //            catch (Exception ex)
        //            {
        //                ModelState.AddModelError(string.Empty, "Server Error: " + ex.Message);
        //            }

        //            lCmd = null;
        //            lNDSCon = null;
        //            lRst = null;

        //            if (lReturn.Count > 0)
        //            {
        //                string lSORNo = "";
        //                string lLPStatus = "";
        //                string lStatus = "";
        //                int lLoop = 1;
        //                int lLeftCount = lReturn.Count;

        //                lLoop = (int)Math.Ceiling((double)(lLeftCount / 900)) + 1;
        //                var lProcessObj = new ProcessController();
        //                lProcessObj.OpenCISConnection(ref lCISCon);

        //                var lSQL = "SELECT O.ORDER_REQUEST_NO, " +
        //                "O.STATUS " +
        //                "FROM SAPSR3.YMSDT_ORDER_HDR O " +
        //                "WHERE O.MANDT = '" + lProcessObj.strClient + "' " +
        //                "AND O.PROJECT = '" + ProjectCode + "' " +
        //                "AND O.STATUS = 'X' " +
        //                "GROUP BY O.ORDER_REQUEST_NO, " +
        //                "O.STATUS ";

        //                lOraCmd.CommandText = lSQL;
        //                lOraCmd.Connection = lCISCon;
        //                lOraCmd.CommandTimeout = 300;
        //                lOraRst = lOraCmd.ExecuteReader();
        //                if (lOraRst.HasRows)
        //                {
        //                    while (lOraRst.Read())
        //                    {
        //                        lSORNo = lOraRst.GetString(0).Trim();
        //                        lStatus = lOraRst.GetString(1).Trim();

        //                        if (lStatus == "X")
        //                        {
        //                            for (int j = 0; j < lReturn.Count; j++)
        //                            {
        //                                if (lReturn[j].SORNo == lSORNo)
        //                                {
        //                                    lReturn.RemoveAt(j);
        //                                    break;
        //                                }
        //                            }
        //                        }
        //                    }
        //                }

        //                lSQL = "SELECT O.ORDER_REQUEST_NO, " +
        //                "NVL(MAX(LP_STATUS), ' '), " +
        //                "O.STATUS " +
        //                "FROM SAPSR3.YMPPT_LP_HDR H, " +
        //                "SAPSR3.YMPPT_LP_ITEM_C C, " +
        //                "SAPSR3.YMSDT_ORDER_HDR O " +
        //                "WHERE C.MANDT = H.MANDT " +
        //                "AND C.LOAD_NO = H.LOAD_NO " +
        //                "AND C.MANDT = O.MANDT " +
        //                "AND C.SALES_ORDER = O.SALES_ORDER " +
        //                "AND O.MANDT = '" + lProcessObj.strClient + "' " +
        //                "AND O.PROJECT = '" + ProjectCode + "' " +
        //                "AND C.PROD_GRP = '" + ProdType + "' " +
        //                "GROUP BY O.ORDER_REQUEST_NO, " +
        //                "O.STATUS ";

        //                lOraCmd.CommandText = lSQL;
        //                lOraCmd.Connection = lCISCon;
        //                lOraCmd.CommandTimeout = 300;
        //                lOraRst = lOraCmd.ExecuteReader();
        //                if (lOraRst.HasRows)
        //                {
        //                    while (lOraRst.Read())
        //                    {
        //                        lSORNo = lOraRst.GetString(0).Trim();
        //                        lLPStatus = lOraRst.GetString(1).Trim();
        //                        lStatus = lOraRst.GetString(2).Trim();

        //                        if (lStatus == "X")
        //                        {
        //                            for (int j = 0; j < lReturn.Count; j++)
        //                            {
        //                                if (lReturn[j].SORNo == lSORNo)
        //                                {
        //                                    lReturn.RemoveAt(j);
        //                                    break;
        //                                }
        //                            }
        //                        }
        //                        else if (lLPStatus == "D")
        //                        {
        //                            for (int j = 0; j < lReturn.Count; j++)
        //                            {
        //                                if (lReturn[j].SORNo == lSORNo)
        //                                {
        //                                    lReturn[j].WBSStatus = "Delivered";
        //                                    break;
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //                lOraRst.Close();

        //                var lSORIn = "";

        //                for (int j = 0; j < lReturn.Count; j++)
        //                {
        //                    if (lReturn[j].WBSStatus != "Delivered")
        //                    {
        //                        if (lReturn[j].SORNo.Trim().Length > 0)
        //                        {
        //                            if (lSORIn.Length > 0)
        //                            {
        //                                lSORIn = lSORIn + ",'" + lReturn[j].SORNo + "'";
        //                            }
        //                            else
        //                            {
        //                                lSORIn = "'" + lReturn[j].SORNo + "'";
        //                            }
        //                        }
        //                    }
        //                }

        //                if (lSORIn.Length > 0)
        //                {
        //                    lSQL = "SELECT O.ORDER_REQUEST_NO, " +
        //                    "NVL(MAX(LFSTK), ' ') " +
        //                    "FROM SAPSR3.VBUK H, " +
        //                    "SAPSR3.YMSDT_ORDER_HDR O " +
        //                    "WHERE H.MANDT = O.MANDT " +
        //                    "AND H.VBELN = O.SALES_ORDER " +
        //                    "AND O.MANDT = '" + lProcessObj.strClient + "' " +
        //                    "AND O.ORDER_REQUEST_NO IN (" + lSORIn + ") " +
        //                    "GROUP BY O.ORDER_REQUEST_NO ";

        //                    lOraCmd.CommandText = lSQL;
        //                    lOraCmd.Connection = lCISCon;
        //                    lOraCmd.CommandTimeout = 300;
        //                    lOraRst = lOraCmd.ExecuteReader();
        //                    if (lOraRst.HasRows)
        //                    {
        //                        while (lOraRst.Read())
        //                        {
        //                            lSORNo = lOraRst.GetString(0).Trim();
        //                            lStatus = lOraRst.GetString(1).Trim();
        //                            if (lStatus == "C")
        //                            {
        //                                if (lSORNo != "")
        //                                {
        //                                    for (int j = 0; j < lReturn.Count; j++)
        //                                    {
        //                                        if (lReturn[j].SORNo == lSORNo)
        //                                        {
        //                                            lReturn[j].WBSStatus = "Delivered";
        //                                            break;
        //                                        }
        //                                    }
        //                                }
        //                            }
        //                        }

        //                    }
        //                    lOraRst.Close();
        //                }

        //                lOraCmd = null;
        //                lOraRst = null;
        //                lProcessObj.CloseCISConnection(ref lCISCon);
        //                lProcessObj = null;
        //            }
        //        }

        //    }
        //    // return Json(lReturn, JsonRequestBehavior.AllowGet);
        //    return Ok(lReturn);
        //}

        //get BBS List at Order Summary
        //[HttpPost]
        //[Route("/getScheduleData_old_beam/{CustomerCode}/{ProjectCode}")]
        ////[ValidateAntiForgeryHeader]
        //public ActionResult getScheduleData_old(string CustomerCode, string ProjectCode)
        //{
        //    var lReturn = new List<WBSStatusModels>();
        //    var lNewWBS = new WBSStatusModels();
        //    var lCmd = new SqlCommand();
        //    var lNDSCon = new SqlConnection();
        //    SqlDataReader lRst;

        //    var lCISCon = new OracleConnection();
        //    var lOraCmd = new OracleCommand();
        //    OracleDataReader lOraRst;

        //    string lProdCategory = "";
        //    string lStrucEle = "";
        //    int lID = 0;


        //    if (CustomerCode != null && ProjectCode != null)
        //    {
        //        if (CustomerCode.Length > 0 && ProjectCode.Length > 0)
        //        {
        //            try
        //            {
        //                lCmd.CommandText =
        //                "SELECT " +
        //                "(SELECT vchStructureElementType " +
        //                "FROM dbo.StructureElementMaster " +
        //                "WHERE intStructureElementTypeId = D.intStructureElementTypeId), " +
        //                "E.vchWBS1, " +
        //                "E.vchWBS2, " +
        //                "E.vchWBS3, " +
        //                "isnull(Max(H.tntStatusId), 0), " +
        //                "isNull(Max(R.tntStatusId), 0), " +
        //                "SUM(isNull(numPostedWeight, 0) + isNull(numPostedCappingWeight, 0) + isNull(numPostedClinkWeight, 0)) as PostedWeight, " +
        //                "(SELECT COUNT(*) from dbo.OESCTSMESHDrawings DR, dbo.StructureElementMaster SE " +
        //                "WHERE DR.StrucElem = SE.vchStructureElementType " +
        //                "AND DR.ProjectCode = P.vchProjectCode " +
        //                "AND SE.intStructureElementTypeId = D.intStructureElementTypeId " +
        //                "AND DR.ProdType = 'MSH' " +
        //                "AND DR.WBS1 = E.vchWBS1 " +
        //                "AND DR.WBS2 = E.vchWBS2 " +
        //                "AND DR.WBS3 = E.vchWBS3 ) AS DrawingCT, " +
        //                "(SELECT ord_req_no FROM dbo.SAP_REQUEST_HDR " +
        //                "WHERE IDENTITY_NO = R.intSalesOrderID ) as SORNo " +
        //                "FROM dbo.WBSElements E, dbo.WBS W, " +
        //                "dbo.WBSElementsDetails D, dbo.SAPProjectMaster P, " +
        //                "dbo.BBSPostHeader H left outer join dbo.BBSReleaseDetails R  " +
        //                "ON H.intPostHeaderid = R.intPostHeaderid and R.tntStatusId <> 14 " +
        //                "WHERE e.intWBSId = W.intWBSId " +
        //                "AND H.intWBSElementId = E.intWBSElementId " +
        //                "and E.intWBSElementId = D.intWBSElementId " +
        //                "and W.intProjectid = P.intProjectid " +
        //                "and D.intStructureElementTypeId > 0 " +
        //                "AND (H.tntStatusId <> 3 " +
        //                "OR (H.tntStatusId = 3 " +
        //                "AND H.numPostedWeight > 0)) " +
        //                "AND NOT EXISTS(SELECT R1.intPostHeaderid FROM dbo.BBSReleaseDetails R1  " +
        //                "WHERE R1.intPostHeaderid = H.intPostHeaderId  AND  R1.tntStatusId = 14) " +
        //                "and D.intConfirm = 1 " +
        //                "and P.vchProjectCode = '" + ProjectCode + "' " +
        //                "and D.sitProductTypeId in  " +
        //                "(SELECT sitProductTypeID " +
        //                "FROM dbo.ProductTypeMaster " +
        //                "WHERE vchProductType = 'MSH') " +
        //                "GROUP BY P.vchProjectCode, " +
        //                "D.intStructureElementTypeId, " +
        //                "E.vchWBS1, " +
        //                "E.vchWBS2, " +
        //                "E.vchWBS3, " +
        //                "E.intWBSElementId, " +
        //                "H.tntStatusId, " +
        //                "R.tntStatusId, " +
        //                "R.intSalesOrderID " +
        //                "Order By vchWBS1, " +
        //                "(case when PATINDEX('B[0-9]%', vchWBS2) > 0 then cast(left(substring(vchWBS2, (PATINDEX('%[0-9]%',vchWBS2)),len(vchWBS2)), " +
        //                "PATINDEX('%[^0-9]%', substring(vchWBS2 + 'z', (PATINDEX('%[0-9]%', vchWBS2)), " +
        //                "len(vchWBS2)) + 'z') ) as int)    " +
        //                "else (case when PATINDEX('FDN[0-9]%', vchWBS2) > 0 then cast(left(substring(vchWBS2, (PATINDEX('%[0-9]%',vchWBS2)),len(vchWBS2)), " +
        //                "PATINDEX('%[^0-9]%', substring(vchWBS2, (PATINDEX('%[0-9]%', vchWBS2)), " +
        //                "len(vchWBS2)) + 'z') ) as int) else  " +
        //                "case when(PATINDEX('B%', vchWBS2) > 0 OR PATINDEX('FDN%', vchWBS2) > 0) then 98  else 99 end " +
        //                "end) end) DESC, " +
        //                " " +
        //                "(case when PATINDEX('[^0-9]%',vchWBS2) > 0 then vchWBS2 " +
        //                "else '' end) DESC,    " +
        //                " " +
        //                "(CASE WHEN PATINDEX('%[0-9]%',vchWBS2) > 0 THEN cast(left(substring(vchWBS2, (PATINDEX('%[0-9]%',vchWBS2)),len(vchWBS2)), " +
        //                "PATINDEX('%[^0-9]%', substring(vchWBS2, (PATINDEX('%[0-9]%', vchWBS2) + 1), " +
        //                "len(vchWBS2)) + 'z') ) as int)   " +
        //                "ELSE " +
        //                "cast(left(substring(vchWBS2, (PATINDEX('%[0-9]%', vchWBS2)), len(vchWBS2)), " +
        //                "PATINDEX('%[^0-9]%', substring(vchWBS2, (PATINDEX('%[0-9]%', vchWBS2)), " +
        //                "len(vchWBS2)) + 'z') - 1) as int) " +
        //                "END) DESC,  " +
        //                "vchWBS2 DESC, " +
        //                "vchWBS3, " +
        //                "1,  " +
        //                "5 DESC, 6 ";

        //                var lProcessObj = new ProcessController();
        //                if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
        //                {
        //                    lCmd.Connection = lNDSCon;
        //                    lCmd.CommandTimeout = 300;
        //                    lRst = lCmd.ExecuteReader();
        //                    if (lRst.HasRows)
        //                    {
        //                        lID = 1;
        //                        while (lRst.Read())
        //                        {
        //                            var lHeaderStatus = lRst.GetByte(4);
        //                            var lReleaseStatus = lRst.GetByte(5);
        //                            var lStatus = "Working";
        //                            if (lReleaseStatus == 12)
        //                            {
        //                                lStatus = "Ordered";
        //                            }
        //                            else
        //                            {
        //                                if (lHeaderStatus == 3)
        //                                {
        //                                    lStatus = "Ready";
        //                                }
        //                            }
        //                            lNewWBS = new WBSStatusModels
        //                            {
        //                                ProjectCode = ProjectCode,
        //                                WBS1 = lRst.GetValue(1) == DBNull.Value ? "" : lRst.GetString(1).Trim(),
        //                                WBS2 = lRst.GetValue(2) == DBNull.Value ? "" : lRst.GetString(2).Trim(),
        //                                WBS3 = lRst.GetValue(3) == DBNull.Value ? "" : lRst.GetString(3).Trim(),
        //                                ProductType = "MSH",
        //                                StructureElem = lRst.GetValue(0) == DBNull.Value ? "" : lRst.GetString(0).Trim(),
        //                                WBSStatus = lStatus,
        //                                WBSTonnage = lRst.GetValue(6) == DBNull.Value ? 0 : lRst.GetDecimal(6),
        //                                DrawingCount = lRst.GetValue(7) == DBNull.Value ? 0 : lRst.GetInt32(7),
        //                                SORNo = lRst.GetValue(8) == DBNull.Value ? "" : lRst.GetString(8)
        //                            };
        //                            lReturn.Add(lNewWBS);
        //                            lID = lID + 1;
        //                        }
        //                    }
        //                    lRst.Close();

        //                    lProcessObj.CloseNDSConnection(ref lNDSCon);
        //                }

        //                lProcessObj = null;

        //            }
        //            catch (Exception ex)
        //            {
        //                ModelState.AddModelError(string.Empty, "Server Error: " + ex.Message);
        //            }
        //            lCmd = null;
        //            lNDSCon = null;
        //            lRst = null;

        //            if (lReturn.Count > 0)
        //            {
        //                string lSORNoIn = "";
        //                string lSORNo = "";
        //                string lLPStatus = "";
        //                string lStatus = "";
        //                string lOutTime = "";
        //                string lSO = "";
        //                string lClient = "";
        //                int lLoop = 1;
        //                int lLeftCount = lReturn.Count;
        //                int lSn = 0;

        //                lLoop = (int)Math.Ceiling((double)(lLeftCount / 900)) + 1;
        //                var lProcessObj = new ProcessController();
        //                lProcessObj.OpenCISConnection(ref lCISCon);

        //                //for (int i = 0; i <= lLoop; i++) {
        //                //    if (lSn >= lReturn.Count)
        //                //    {
        //                //        break;
        //                //    }

        //                //    lSORNoIn = "";
        //                //    for (int j = 0; j < 900; j++)
        //                //    {
        //                //        if (lSn >= lReturn.Count)
        //                //        {
        //                //            break;
        //                //        }

        //                //        lSORNo = lReturn[lSn].SORNo.Trim();
        //                //        if (lSORNo.Length > 0)
        //                //        {
        //                //            if (lSORNoIn.Length > 0)
        //                //            {
        //                //                lSORNoIn = lSORNoIn + ",'" + lSORNo + "'";
        //                //            }
        //                //            else
        //                //            {
        //                //                lSORNoIn = "'" + lSORNo + "'";
        //                //            }
        //                //        }
        //                //        lSn = lSn + 1;
        //                //        lLeftCount = lLeftCount - 1;
        //                //    }

        //                //    if (lSORNoIn.Length > 0)
        //                //    {

        //                //        //var lSQL = "SELECT O.ORDER_REQUEST_NO, " +
        //                //        //"(SELECT NVL(MAX(WEIGH_OUT_TIME), ' ') " +
        //                //        //"FROM SAPSR3.YMPPT_LOAD_VEHIC V " +
        //                //        //"WHERE V.MANDT = O.MANDT " +
        //                //        //"AND V.VBELN = O.SALES_ORDER) as OutTime, " +
        //                //        //"O.STATUS " +
        //                //        //"FROM SAPSR3.YMSDT_ORDER_HDR O " +
        //                //        //"WHERE O.MANDT = '600' " +
        //                //        //"AND O.ORDER_REQUEST_NO IN (" + lSORNoIn + ") ";

        //                //        var lSQL = "SELECT O.ORDER_REQUEST_NO, " +
        //                //        "(SELECT NVL(MAX(LP_STATUS), ' ') " +
        //                //        "FROM SAPSR3.YMPPT_LP_HDR H, " +
        //                //        "SAPSR3.YMPPT_LP_ITEM_C C " +
        //                //        "WHERE C.MANDT = H.MANDT " +
        //                //        "AND C.LOAD_NO = H.LOAD_NO " +
        //                //        "AND C.MANDT = O.MANDT " +
        //                //        "AND C.SALES_ORDER = O.SALES_ORDER) as LP_STATUS, " +
        //                //        "(SELECT NVL(MAX(WEIGH_OUT_TIME), ' ') " +
        //                //        "FROM SAPSR3.YMPPT_LOAD_VEHIC V " +
        //                //        "WHERE V.MANDT = O.MANDT " +
        //                //        "AND V.VBELN = O.SALES_ORDER) as OutTime, " +
        //                //        "O.STATUS " +
        //                //        "FROM SAPSR3.YMSDT_ORDER_HDR O " +
        //                //        "WHERE O.MANDT = '600' " +
        //                //        "AND O.ORDER_REQUEST_NO IN (" + lSORNoIn + ") ";

        //                //        //+
        //                //        //"GROUP BY O.ORDER_REQUEST_NO, " +
        //                //        //"O.STATUS ";

        //                //        lOraCmd.CommandText = lSQL;
        //                //        lOraCmd.Connection = lCISCon;
        //                //        lOraCmd.CommandTimeout = 300;
        //                //        lOraRst = lOraCmd.ExecuteReader();
        //                //        if (lOraRst.HasRows)
        //                //        {
        //                //            while (lOraRst.Read())
        //                //            {
        //                //                lSORNo = lOraRst.GetString(0).Trim();
        //                //                lLPStatus = lOraRst.GetString(1).Trim();
        //                //                lOutTime = lOraRst.GetString(2).Trim();
        //                //                lStatus = lOraRst.GetString(3).Trim();
        //                //                if (lLPStatus == "D" || lOutTime.Length > 0 || lStatus == "X")
        //                //                {
        //                //                    for (int j = 0; j < lReturn.Count; j++)
        //                //                    {
        //                //                        if (lReturn[j].SORNo == lSORNo)
        //                //                        {
        //                //                            if (lStatus == "X")
        //                //                            {
        //                //                                lReturn.RemoveAt(j);
        //                //                            }
        //                //                            else
        //                //                            {
        //                //                                if (lLPStatus == "D" || lOutTime.Length > 0)
        //                //                                {
        //                //                                    lReturn[j].WBSStatus = "Delivered";
        //                //                                }
        //                //                            }
        //                //                            break;
        //                //                        }
        //                //                    }
        //                //                }
        //                //            }
        //                //        }

        //                //        lOraRst.Close();
        //                //    }
        //                //}
        //                var lSQL = "SELECT O.ORDER_REQUEST_NO, " +
        //                "O.STATUS " +
        //                "FROM SAPSR3.YMSDT_ORDER_HDR O " +
        //                "WHERE O.MANDT = '" + lProcessObj.strClient + "' " +
        //                "AND O.PROJECT = '" + ProjectCode + "' " +
        //                "AND O.STATUS = 'X' " +
        //                "GROUP BY O.ORDER_REQUEST_NO, " +
        //                "O.STATUS ";

        //                lOraCmd.CommandText = lSQL;
        //                lOraCmd.Connection = lCISCon;
        //                lOraCmd.CommandTimeout = 300;
        //                lOraRst = lOraCmd.ExecuteReader();
        //                if (lOraRst.HasRows)
        //                {
        //                    while (lOraRst.Read())
        //                    {
        //                        lSORNo = lOraRst.GetString(0).Trim();
        //                        lStatus = lOraRst.GetString(1).Trim();

        //                        if (lStatus == "X")
        //                        {
        //                            for (int j = 0; j < lReturn.Count; j++)
        //                            {
        //                                if (lReturn[j].SORNo == lSORNo)
        //                                {
        //                                    lReturn.RemoveAt(j);
        //                                    break;
        //                                }
        //                            }
        //                        }
        //                    }
        //                }

        //                lSQL = "SELECT O.ORDER_REQUEST_NO, " +
        //                "NVL(MAX(LP_STATUS), ' '), " +
        //                "O.STATUS " +
        //                "FROM SAPSR3.YMPPT_LP_HDR H, " +
        //                "SAPSR3.YMPPT_LP_ITEM_C C, " +
        //                "SAPSR3.YMSDT_ORDER_HDR O " +
        //                "WHERE C.MANDT = H.MANDT " +
        //                "AND C.LOAD_NO = H.LOAD_NO " +
        //                "AND C.MANDT = O.MANDT " +
        //                "AND C.SALES_ORDER = O.SALES_ORDER " +
        //                "AND O.MANDT = '" + lProcessObj.strClient + "' " +
        //                "AND O.PROJECT = '" + ProjectCode + "' " +
        //                "AND C.PROD_GRP = 'MSH' " +
        //                "GROUP BY O.ORDER_REQUEST_NO, " +
        //                "O.STATUS ";

        //                lOraCmd.CommandText = lSQL;
        //                lOraCmd.Connection = lCISCon;
        //                lOraCmd.CommandTimeout = 300;
        //                lOraRst = lOraCmd.ExecuteReader();
        //                if (lOraRst.HasRows)
        //                {
        //                    while (lOraRst.Read())
        //                    {
        //                        lSORNo = lOraRst.GetString(0).Trim();
        //                        lLPStatus = lOraRst.GetString(1).Trim();
        //                        lStatus = lOraRst.GetString(2).Trim();

        //                        if (lStatus == "X")
        //                        {
        //                            for (int j = 0; j < lReturn.Count; j++)
        //                            {
        //                                if (lReturn[j].SORNo == lSORNo)
        //                                {
        //                                    lReturn.RemoveAt(j);
        //                                    break;
        //                                }
        //                            }
        //                        }
        //                        else if (lLPStatus == "D")
        //                        {
        //                            for (int j = 0; j < lReturn.Count; j++)
        //                            {
        //                                if (lReturn[j].SORNo == lSORNo)
        //                                {
        //                                    lReturn[j].WBSStatus = "Delivered";
        //                                    break;
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //                lOraRst.Close();

        //                var lSORIn = "";

        //                //for (int j = 0; j < lReturn.Count; j++)
        //                //{
        //                //    if (lReturn[j].WBSStatus != "Delivered")
        //                //    {
        //                //        if (lReturn[j].SORNo.Trim().Length > 0)
        //                //        {
        //                //            if (lSORIn.Length > 0)
        //                //            {
        //                //                lSORIn = lSORIn + ",'" + lReturn[j].SORNo + "'";
        //                //            }
        //                //            else
        //                //            {
        //                //                lSORIn = "'" + lReturn[j].SORNo + "'";
        //                //            }
        //                //        }
        //                //    }
        //                //}

        //                //if (lSORIn.Length > 0)
        //                //{
        //                //    lSQL = "SELECT NVL(MAX(WEIGH_OUT_TIME), ' '), O.ORDER_REQUEST_NO " +
        //                //    "FROM SAPSR3.YMPPT_LOAD_VEHIC V, SAPSR3.YMSDT_ORDER_HDR O " +
        //                //    "WHERE V.MANDT = O.MANDT " +
        //                //    "AND V.VBELN = O.SALES_ORDER " +
        //                //    "AND O.MANDT = '" + lProcessObj.strClient + "' " +
        //                //    "AND O.ORDER_REQUEST_NO IN (" + lSORIn + ") " +
        //                //    "GROUP BY O.ORDER_REQUEST_NO ";

        //                //    lOraCmd.CommandText = lSQL;
        //                //    lOraCmd.Connection = lCISCon;
        //                //    lOraCmd.CommandTimeout = 300;
        //                //    lOraRst = lOraCmd.ExecuteReader();
        //                //    if (lOraRst.HasRows)
        //                //    {
        //                //        while (lOraRst.Read())
        //                //        {
        //                //            lOutTime = lOraRst.GetString(0).Trim();
        //                //            lSORNo = lOraRst.GetString(1).Trim();
        //                //            if (lOutTime != "")
        //                //            {
        //                //                if (lSORNo != "")
        //                //                {
        //                //                    for (int j = 0; j < lReturn.Count; j++)
        //                //                    {
        //                //                        if (lReturn[j].SORNo == lSORNo)
        //                //                        {
        //                //                            lReturn[j].WBSStatus = "Delivered";
        //                //                            break;
        //                //                        }
        //                //                    }
        //                //                }
        //                //            }
        //                //        }

        //                //    }
        //                //    lOraRst.Close();
        //                //}

        //                lSORIn = "";

        //                for (int j = 0; j < lReturn.Count; j++)
        //                {
        //                    if (lReturn[j].WBSStatus != "Delivered")
        //                    {
        //                        if (lReturn[j].SORNo.Trim().Length > 0)
        //                        {
        //                            if (lSORIn.Length > 0)
        //                            {
        //                                lSORIn = lSORIn + ",'" + lReturn[j].SORNo + "'";
        //                            }
        //                            else
        //                            {
        //                                lSORIn = "'" + lReturn[j].SORNo + "'";
        //                            }
        //                        }
        //                    }
        //                }

        //                if (lSORIn.Length > 0)
        //                {
        //                    lSQL = "SELECT O.ORDER_REQUEST_NO, " +
        //                    "NVL(MAX(LFSTK), ' ') " +
        //                    "FROM SAPSR3.VBUK H, " +
        //                    "SAPSR3.YMSDT_ORDER_HDR O " +
        //                    "WHERE H.MANDT = O.MANDT " +
        //                    "AND H.VBELN = O.SALES_ORDER " +
        //                    "AND O.MANDT = '" + lProcessObj.strClient + "' " +
        //                    "AND O.ORDER_REQUEST_NO IN (" + lSORIn + ") " +
        //                    "GROUP BY O.ORDER_REQUEST_NO ";

        //                    lOraCmd.CommandText = lSQL;
        //                    lOraCmd.Connection = lCISCon;
        //                    lOraCmd.CommandTimeout = 300;
        //                    lOraRst = lOraCmd.ExecuteReader();
        //                    if (lOraRst.HasRows)
        //                    {
        //                        while (lOraRst.Read())
        //                        {
        //                            lSORNo = lOraRst.GetString(0).Trim();
        //                            lStatus = lOraRst.GetString(1).Trim();
        //                            if (lStatus == "C")
        //                            {
        //                                if (lSORNo != "")
        //                                {
        //                                    for (int j = 0; j < lReturn.Count; j++)
        //                                    {
        //                                        if (lReturn[j].SORNo == lSORNo)
        //                                        {
        //                                            lReturn[j].WBSStatus = "Delivered";
        //                                            break;
        //                                        }
        //                                    }
        //                                }
        //                            }
        //                        }

        //                    }
        //                    lOraRst.Close();
        //                }

        //                lOraCmd = null;
        //                lOraRst = null;
        //                lProcessObj.CloseCISConnection(ref lCISCon);
        //                lProcessObj = null;
        //            }
        //        }

        //    }
        //    return Ok(lReturn);
        //   // return Json(lReturn, JsonRequestBehavior.AllowGet);
        //}

        //save BBS
        [HttpPost]
        [Route("/saveBBSNSH_beam/{CustomerCode}/{ProjectCode}/{JobID}/{bbsModels}")]
        // [ValidateAntiForgeryHeader]
        public ActionResult saveBBSNSH(string CustomerCode, string ProjectCode, int JobID, List<CTSMESHBBSNSHModels> bbsModels)
        {
            var lReturn = true;
            var lCmd = new SqlCommand();
            var lNDSCon = new SqlConnection();

            if (CustomerCode != null && ProjectCode != null)
            {
                if (CustomerCode.Length > 0 && ProjectCode.Length > 0)
                {
                    if (bbsModels != null)
                    {
                        if (bbsModels.Count > 0)
                        {
                            for (int i = 0; i < bbsModels.Count; i++)
                            {
                                bbsModels[i].CustomerCode = CustomerCode;
                                bbsModels[i].ProjectCode = ProjectCode;
                                bbsModels[i].JobID = JobID;
                                bbsModels[i].BBSID = i + 1;
                                bbsModels[i].BBSDrawing = (bbsModels[i].BBSDrawing != null && bbsModels[i].BBSDrawing.Length > 0 ? "Y" : "N");
                                bbsModels[i].UpdateBy = User.Identity.GetUserName();
                                bbsModels[i].UpdateDate = DateTime.Now;

                                if (bbsModels[i].BBSStrucElem == "Slab Bottom")
                                {
                                    bbsModels[i].BBSStrucElem = "SLAB-B";
                                }
                                if (bbsModels[i].BBSStrucElem == "Slab Top")
                                {
                                    bbsModels[i].BBSStrucElem = "SLAB-T";
                                }
                            }
                        }
                    }

                    lCmd.CommandText =
                    "DELETE FROM dbo.OESCTSMESHBBSNSH " +
                    "WHERE CustomerCode = '" + CustomerCode + "' " +
                    "AND ProjectCode = '" + ProjectCode + "' " +
                    "AND JobID = " + JobID + " ";

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

                        if (bbsModels != null)
                        {
                            if (bbsModels.Count > 0)
                            {
                                db.CTSMESHBBSNSH.AddRange(bbsModels);
                                db.SaveChanges();
                            }
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
            //return Json(lReturn, JsonRequestBehavior.AllowGet);
            return Ok(lReturn); 
        }

        [HttpGet]
        [Route("/Dispose_beam/{disposing}")]
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpGet]
        [Route("/ShowShapeList")]
        public ActionResult ShowSHapeList()
        {
            try
            {

                //var localFilePath = AppDomain.CurrentDomain.BaseDirectory; 
                //var directory = new DirectoryInfo(localFilePath);
                //var myFile = (from f in directory.GetFiles()
                //              orderby f.LastWriteTime descending
                //              select f).First();

                string fileName = "Shapes_List.pdf";// "cabnew.pdf";            

                var localFilePath = "D:\\ODOS\\PDF\\Shapes_List.pdf";

                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StreamContent(new FileStream(localFilePath, FileMode.Open, FileAccess.Read));
                response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                response.Content.Headers.ContentDisposition.FileName = fileName;
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                //response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue(System.Net.Mime.DispositionTypeNames.Inline)
                //{
                //    FileName = fileName
                //};
                return File(response.Content.ReadAsByteArrayAsync().Result, "application/pdf");
            }
            catch (Exception ex)
            {
                string errormsg = ex.Message;
                return Ok(errormsg);
            }



        }


        [HttpGet]
        [Route("/GetShapeCodeImage/ShapeCodeName")]
        public ActionResult GetShapeCodeImage(string ShapeCodeName)
        {
            try
            {

                //var localFilePath = AppDomain.CurrentDomain.BaseDirectory; 
                //var directory = new DirectoryInfo(localFilePath);
                //var myFile = (from f in directory.GetFiles()
                //              orderby f.LastWriteTime descending
                //              select f).First();

                string fileName = "Shapes_List.pdf";// "cabnew.pdf";            

                var localFilePath = "D:\\ODOS\\PDF\\Shapes_List.pdf";

                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StreamContent(new FileStream(localFilePath, FileMode.Open, FileAccess.Read));
                response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                response.Content.Headers.ContentDisposition.FileName = fileName;
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                //response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue(System.Net.Mime.DispositionTypeNames.Inline)
                //{
                //    FileName = fileName
                //};
                return File(response.Content.ReadAsByteArrayAsync().Result, "application/pdf");
            }
            catch (Exception ex)
            {
                string errormsg = ex.Message;
                return Ok(errormsg);
            }



        }




    }
}
