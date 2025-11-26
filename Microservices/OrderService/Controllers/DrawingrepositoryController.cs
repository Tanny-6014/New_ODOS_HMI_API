using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.IO;
using System.Web;
//using System.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using OrderService.Models;

//using AntiForgeryHeader.Helper;
using Microsoft.AspNet.Identity;
using System.Data.SqlClient;
//using iTextSharp.text;
//using iTextSharp.text.html.simpleparser;
//using iTextSharp.text.pdf;
using System.IO;
using OrderService.Repositories;
using System.Globalization;
using OfficeOpenXml;
using OfficeOpenXml.Style;
//using NCalc;
using System.Text;
using Oracle.ManagedDataAccess.Client;
using System.ServiceModel;
//using System.ServiceModel.Channels;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;

using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Security;

//using Xbim.Ifc;
//using Xbim.IO;
//using Xbim.ModelGeometry.Scene;

using System.Text.RegularExpressions;
using Microsoft.Exchange.WebServices.Data;
using Microsoft.AspNetCore.Authorization;
using OrderService.Dtos;
using Azure;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.Extensions.Primitives;
using SharpCompress.Common;
using Microsoft.AspNetCore.Http;


namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DrawingrepositoryController : Controller
    {
        public string gUserType = "";
        public string gGroupName = "";
        public const string gSiteURL = "https://natsteel.sharepoint.com/sites/DigiOSDocs";

        private DBContextModels db = new DBContextModels();

        ////[ValidateAntiForgeryToken]
        //public ActionResult Drawings(string rCustomerCode, string rProjectCode)
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
        //    var lUserType = lUa.getUserType(User.Identity.GetUserName());
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

        //    return View();
        //}

        [HttpPost]
        [Route("/getDrawingList")]
        public ActionResult getDrawingList(DrawingList drawingList)
        {
            var lReturn = (new[]{ new
            {
                DrawingID = 0,
                DrawingNo = "",
                FileName = "",
                Remarks = "",
                UpdatedBy = "",
                UpdatedDate = DateTime.Now,
                Revision = 0,
                NoAssign = 0
            }}).ToList();

            if (lReturn.Count > 0)
            {
                lReturn.RemoveAt(0);
            }

            var lDa = new SqlDataAdapter();
            var lCmd = new SqlCommand();
            var lDs = new DataSet();
            var cnNDS = new SqlConnection();

            string lSQL = "";

            string lCagetoryCond = "";
            if (drawingList.Category != null)
            {
                if (drawingList.Category.ToUpper() == "NSH DRAWINGS")
                {
                    lCagetoryCond = "AND NOT EXISTS (SELECT DrawingID FROM dbo.OESDrawingsOrder WHERE DrawingID = P.DrawingID) ";
                }
                else if (drawingList.Category.ToUpper() == "ORDER DOCS")
                {
                    //lCagetoryCond = "AND EXISTS (SELECT DrawingID FROM dbo.OESDrawingsOrder WHERE DrawingID = P.DrawingID) AND (P.FileName NOT like '%.xlsx' AND P.FileName NOT like '%.xlsm' AND P.FileName NOT like '%.xls' AND P.FileName NOT like '%.msg') ";
                    lCagetoryCond = "AND EXISTS (SELECT DrawingID FROM dbo.OESDrawingsOrder WHERE DrawingID = P.DrawingID) ";
                }
            }

            if (lCagetoryCond == "")
            {
                lCagetoryCond = " AND 1 = 1 ";
            }

            string lProdTypeCond = "";
            if (drawingList.ProductType != null && drawingList.ProductType.Count > 0)
            {
                for (int i = 0; i < drawingList.ProductType.Count; i++)
                {
                    if (drawingList.ProductType[i] != null && drawingList.ProductType[i].Trim() != "")
                    {
                        if (lProdTypeCond == "")
                        {
                            lProdTypeCond = "(CASE when W.ProductType = 'CUT-TO-SIZE-MESH' then 'Mesh' when W.ProductType = 'STIRRUP-LINK-MESH' then 'Mesh' when W.ProductType = 'COLUMN-LINK-MESH' then 'MESH' else W.ProductType END) = '" + drawingList.ProductType[i] + "' ";
                        }
                        else
                        {
                            lProdTypeCond = lProdTypeCond + " OR (CASE when W.ProductType = 'CUT-TO-SIZE-MESH' then 'Mesh' when W.ProductType = 'STIRRUP-LINK-MESH' then 'Mesh' when W.ProductType = 'COLUMN-LINK-MESH' then 'MESH' else W.ProductType END) = '" + drawingList.ProductType[i] + "' ";
                        }
                    }
                }
            }
            if (lProdTypeCond == "")
            {
                lProdTypeCond = " 1 = 1 ";
            }

            var lStruEle = "";
            if (drawingList.StructureElement != null && drawingList.StructureElement.Count > 0)
            {
                for (int i = 0; i < drawingList.StructureElement.Count; i++)
                {
                    if (drawingList.StructureElement[i] != null && drawingList.StructureElement[i].Trim() != "")
                    {
                        if (lStruEle == "")
                        {
                            lStruEle = "StructureElement = '" + drawingList.StructureElement[i] + "' ";
                        }
                        else
                        {
                            lStruEle = lStruEle + " OR StructureElement = '" + drawingList.StructureElement[i] + "' ";
                        }
                    }
                }
            }
            if (lStruEle == "")
            {
                lStruEle = " 1 = 1 ";
            }

            if (drawingList.WBS1 == null)
            {
                drawingList.WBS1 = new List<string>();
            }
            if (drawingList.WBS2 == null)
            {
                drawingList.WBS2 = new List<string>();
            }
            if (drawingList.WBS3 == null)
            {
                drawingList.WBS3 = new List<string>();
            }
            if (drawingList.StructureElement == null)
            {
                drawingList.StructureElement = new List<string>();
            }
            if (drawingList.ProductType == null)
            {
                drawingList.ProductType = new List<string>();
            }

            if ((drawingList.ProductType.Count == 0 || (drawingList.ProductType.Count == 1 && drawingList.ProductType[0].Trim() == "")) &&
                (drawingList.StructureElement.Count == 0 || (drawingList.StructureElement.Count == 1 && drawingList.StructureElement[0].Trim() == "")))
            {
                var lProcess = new ProcessController();
                lProcess.OpenNDSConnection(ref cnNDS);
                if (cnNDS.State == ConnectionState.Open)
                {
                    lSQL = "SELECT " +
                    "P.DrawingID, " +
                    "P.DrawingNo, " +
                    "P.FileName, " +
                    "P.Remarks, " +
                    "P.UpdatedBy, " +
                    "P.UpdatedDate, " +
                    "P.Revision, " +
                    "(SELECT COUNT(DrawingID) " +
                    "FROM dbo.OESDrawingsWBS " +
                    "WHERE DrawingID = P.DrawingID) " +
                    "FROM dbo.OESDrawings P " +
                    "WHERE P.CustomerCode = '" + drawingList.CustomerCode + "' " +
                    "AND P.ProjectCode = '" + drawingList.ProjectCode + "' " +
                    lCagetoryCond +
                    //"AND NOT EXISTS ( SELECT DrawingID " +
                    //"FROM dbo.OESDrawingsWBS " +
                    //"WHERE DrawingID = P.DrawingID " +
                    //"AND Revision = P.Revision) " +
                    "GROUP BY " +
                    "P.DrawingID, " +
                    "P.DrawingNo, " +
                    "P.FileName, " +
                    "P.Remarks, " +
                    "P.UpdatedBy, " +
                    "P.UpdatedDate, " +
                    "P.Revision " +
                    "ORDER BY " +
                    "P.FileName ";

                    lCmd.CommandType = CommandType.Text;
                    lCmd.CommandText = lSQL;
                    lCmd.Connection = cnNDS;
                    lDa.SelectCommand = lCmd;
                    lDs = new DataSet();
                    lDa.Fill(lDs);
                    if (lDs.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < lDs.Tables[0].Rows.Count; i++)
                        {
                            lReturn.Add(new
                            {
                                DrawingID = (int)lDs.Tables[0].Rows[i].ItemArray[0],
                                DrawingNo = lDs.Tables[0].Rows[i].ItemArray[1].ToString().Trim(),
                                FileName = lDs.Tables[0].Rows[i].ItemArray[2].ToString().Trim(),
                                Remarks = lDs.Tables[0].Rows[i].ItemArray[3].ToString().Trim(),
                                UpdatedBy = lDs.Tables[0].Rows[i].ItemArray[4].ToString().Trim(),
                                UpdatedDate = (DateTime)lDs.Tables[0].Rows[i].ItemArray[5],
                                Revision = (int)lDs.Tables[0].Rows[i].ItemArray[6],
                                NoAssign = (int)lDs.Tables[0].Rows[i].ItemArray[7]
                            });
                        }
                    }
                    lProcess.CloseNDSConnection(ref cnNDS);
                }

            }
            else
            {
                string lWBS1Cond = "";
                string lWBS2Cond = "";
                string lWBS3Cond = "";

                if (drawingList.WBS1 != null && drawingList.WBS1.Count > 0)
                {
                    for (int i = 0; i < drawingList.WBS1.Count; i++)
                    {
                        if (drawingList.WBS1[i] != null && drawingList.WBS1[i].Trim() != "")
                        {
                            if (lWBS1Cond == "")
                            {
                                lWBS1Cond = "W.WBS1 = '" + drawingList.WBS1[i] + "' ";
                            }
                            else
                            {
                                lWBS1Cond = lWBS1Cond + " OR W.WBS1 = '" + drawingList.WBS1[i] + "' ";
                            }
                        }
                    }
                }
                if (lWBS1Cond == "")
                {
                    lWBS1Cond = " 1 = 1 ";
                }

                if (drawingList.WBS2 != null && drawingList.WBS2.Count > 0)
                {
                    for (int i = 0; i < drawingList.WBS2.Count; i++)
                    {
                        if (drawingList.WBS2[i] != null && drawingList.WBS2[i].Trim() != "")
                        {
                            if (lWBS2Cond == "")
                            {
                                lWBS2Cond = "W.WBS2 = '" + drawingList.WBS2[i] + "' ";
                            }
                            else
                            {
                                lWBS2Cond = lWBS2Cond + " OR W.WBS2 = '" + drawingList.WBS2[i] + "' ";
                            }
                        }
                    }
                }
                if (lWBS2Cond == "")
                {
                    lWBS2Cond = " 1 = 1 ";
                }

                if (drawingList.WBS3 != null && drawingList.WBS3.Count > 0)
                {
                    for (int i = 0; i < drawingList.WBS3.Count; i++)
                    {
                        if (drawingList.WBS3[i] != null && drawingList.WBS3[i].Trim() != "")
                        {
                            if (lWBS3Cond == "")
                            {
                                lWBS3Cond = "W.WBS3 = '" + drawingList.WBS3[i] + "' ";
                            }
                            else
                            {
                                lWBS3Cond = lWBS3Cond + " OR W.WBS3 = '" + drawingList.WBS3[i] + "' ";
                            }
                        }
                    }
                }
                if (lWBS3Cond == "")
                {
                    lWBS3Cond = " 1 = 1 ";
                }

                // WBS1 only
                lSQL = "SELECT " +
                        "P.DrawingID, " +
                        "P.DrawingNo, " +
                        "P.FileName, " +
                        "P.Remarks, " +
                        "P.UpdatedBy, " +
                        "P.UpdatedDate, " +
                        "P.Revision, " +
                        "(SELECT COUNT(DrawingID) " +
                        "FROM dbo.OESDrawingsWBS " +
                        "WHERE DrawingID = P.DrawingID) " +
                        "FROM dbo.OESDrawings P, dbo.OESDrawingsWBS W " +
                        "WHERE P.DrawingID = W.DrawingID " +
                        "AND P.CustomerCode = '" + drawingList.CustomerCode + "' " +
                        "AND P.ProjectCode = '" + drawingList.ProjectCode + "' " +
                        "AND ( " + lProdTypeCond + " ) " +
                        "AND ( " + lStruEle + " ) " +
                        "AND ( " + lWBS1Cond + " ) " +
                        "AND ( " + lWBS2Cond + " ) " +
                        "AND ( " + lWBS3Cond + " ) " +
                        lCagetoryCond +
                        "GROUP BY " +
                        "P.DrawingID, " +
                        "P.DrawingNo, " +
                        "P.FileName, " +
                        "P.Remarks, " +
                        "P.UpdatedBy, " +
                        "P.UpdatedDate, " +
                        "P.Revision " +
                        "ORDER BY " +
                        "P.FileName, " +
                        "P.Revision ";

                var lProcess = new ProcessController();
                lProcess.OpenNDSConnection(ref cnNDS);
                if (cnNDS.State == ConnectionState.Open)
                {
                    lCmd.CommandType = CommandType.Text;
                    lCmd.CommandText = lSQL;
                    lCmd.Connection = cnNDS;
                    lDa.SelectCommand = lCmd;
                    lDs = new DataSet();
                    lDa.Fill(lDs);
                    if (lDs.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < lDs.Tables[0].Rows.Count; i++)
                        {
                            lReturn.Add(new
                            {
                                DrawingID = (int)lDs.Tables[0].Rows[i].ItemArray[0],
                                DrawingNo = lDs.Tables[0].Rows[i].ItemArray[1].ToString().Trim(),
                                FileName = lDs.Tables[0].Rows[i].ItemArray[2].ToString().Trim(),
                                Remarks = lDs.Tables[0].Rows[i].ItemArray[3].ToString().Trim(),
                                UpdatedBy = lDs.Tables[0].Rows[i].ItemArray[4].ToString().Trim(),
                                UpdatedDate = (DateTime)lDs.Tables[0].Rows[i].ItemArray[5],
                                Revision = (int)lDs.Tables[0].Rows[i].ItemArray[6],
                                NoAssign = (int)lDs.Tables[0].Rows[i].ItemArray[7]
                            });
                        }
                    }
                    lProcess.CloseNDSConnection(ref cnNDS);
                }


            }
            //return Ok(lReturn);
            return Ok(lReturn);

        }

        [HttpGet]
        [Route("/searchDrawingList/{CustomerCode}/{ProjectCode}/{FileName}/{DrawingNo}/{UpdateBy}/{UpdateDateFr}/{UpdateDateTo}")]
        public ActionResult searchDrawingList(string CustomerCode, string ProjectCode,
            string FileName, string DrawingNo, string UpdateBy, string UpdateDateFr, string UpdateDateTo)
        {
            var lReturn = (new[]{ new
            {
                DrawingID = 0,
                DrawingNo = "",
                FileName = "",
                Remarks = "",
                UpdatedBy = "",
                UpdatedDate = DateTime.Now,
                Revision = 0,
                NoAssign = 0
            }}).ToList();

            if (lReturn.Count > 0)
            {
                lReturn.RemoveAt(0);
            }

            var lDa = new SqlDataAdapter();
            var lCmd = new SqlCommand();
            var lDs = new DataSet();
            var cnNDS = new SqlConnection();

            string lSQL = "";

            if (UpdateDateFr == null) UpdateDateFr = "";
            if (UpdateDateFr.Trim().Length == 0)
            {
                UpdateDateFr = "2000-01-01";
            }
            if (UpdateDateTo == null) UpdateDateTo = "";
            if (UpdateDateTo.Trim().Length == 0)
            {
                UpdateDateTo = "2100-01-01";
            }

            // WBS1 only
            lSQL = "SELECT " +
            "P.DrawingID, " +
            "P.DrawingNo, " +
            "P.FileName, " +
            "P.Remarks, " +
            "P.UpdatedBy, " +
            "P.UpdatedDate, " +
            "P.Revision, " +
            "(SELECT COUNT(DrawingID) " +
            "FROM dbo.OESDrawingsWBS " +
            "WHERE DrawingID = P.DrawingID) " +
            "FROM dbo.OESDrawings P LEFT OUTER JOIN dbo.OESDrawingsWBS W " +
            "ON P.DrawingID = W.DrawingID " +
            "WHERE P.CustomerCode = '" + CustomerCode + "' " +
            "AND P.ProjectCode = '" + ProjectCode + "' " +
            "AND P.FileName like N'%" + FileName + "%' " +
            "AND P.DrawingNo like '%" + DrawingNo + "%' " +
            "AND P.UpdatedBy like '%" + UpdateBy + "%' " +
            "AND P.UpdatedDate >= '" + UpdateDateFr + "' " +
            "AND P.UpdatedDate <= '" + UpdateDateTo + "' " +
            "GROUP BY " +
            "P.DrawingID, " +
            "P.DrawingNo, " +
            "P.FileName, " +
            "P.Remarks, " +
            "P.UpdatedBy, " +
            "P.UpdatedDate, " +
            "P.Revision " +
            "ORDER BY " +
            "P.FileName ";

            var lProcess = new ProcessController();
            lProcess.OpenNDSConnection(ref cnNDS);
            if (cnNDS.State == ConnectionState.Open)
            {
                lCmd.CommandType = CommandType.Text;
                lCmd.CommandText = lSQL;
                lCmd.Connection = cnNDS;
                lDa.SelectCommand = lCmd;
                lDs = new DataSet();
                lDa.Fill(lDs);
                if (lDs.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < lDs.Tables[0].Rows.Count; i++)
                    {
                        lReturn.Add(new
                        {
                            DrawingID = (int)lDs.Tables[0].Rows[i].ItemArray[0],
                            DrawingNo = lDs.Tables[0].Rows[i].ItemArray[1].ToString().Trim(),
                            FileName = lDs.Tables[0].Rows[i].ItemArray[2].ToString().Trim(),
                            Remarks = lDs.Tables[0].Rows[i].ItemArray[3].ToString().Trim(),
                            UpdatedBy = lDs.Tables[0].Rows[i].ItemArray[4].ToString().Trim(),
                            UpdatedDate = (DateTime)lDs.Tables[0].Rows[i].ItemArray[5],
                            Revision = (int)lDs.Tables[0].Rows[i].ItemArray[6],
                            NoAssign = (int)lDs.Tables[0].Rows[i].ItemArray[7]
                        });
                    }
                }
                lProcess.CloseNDSConnection(ref cnNDS);
            }
            return Ok(lReturn);
        }


        [HttpPost]
        [Route("/UploadTest")]
        public async Task<ActionResult> UploadTestAsync()
        {
           string SharePointSiteUrl = "https://natsteel.sharepoint.com/:f:/r/sites/DigiOSDocs/Shared%20Documents/DrawingsLib?csf=1&web=1&e=4b3XMN";
           //string SharePointDocumentLibrary = "Shared Documents";
           string FileName = "STDMESH-0000111909112.pdf";
           string FilePath = "D:\\STDMESH-0000111909112.pdf";
           string UserName = "DigiOSNoReply@natsteel.com.sg";
           string Password = "NatSteel@123B";

            using (var client = new HttpClient())
            {
                var authInfo = Convert.ToBase64String(System.Text.Encoding.Default.GetBytes($"{UserName}:{Password}"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authInfo);

               var endpoint = $"{SharePointSiteUrl}/{FileName}";



                using (var fileStream = System.IO.File.OpenRead(FilePath))
                {
                    var content = new StreamContent(fileStream);
                    var response = await client.PutAsync(endpoint, content);

                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("File uploaded successfully.");
                    }
                    else
                    {
                        return Ok(response);
                        Console.WriteLine(response);
                        Console.WriteLine("Failed to upload the file.");
                    }
                }
            }

            return Ok();
        }

        //[HttpPost]
        ////[ValidateAntiForgeryHeader]
        //public ActionResult getProject(string CustomerCode)
        //{
        //    string lUserType = "";
        //    string lGroupName = "";

        //    OracleDataReader lRst;
        //    var lCmd = new OracleCommand();
        //    var lcisCon = new OracleConnection();

        //    UserAccessController lUa = new UserAccessController();
        //    lUserType = lUa.getUserType(User.Identity.GetUserName());
        //    lGroupName = lUa.getGroupName(User.Identity.GetUserName());

        //    lUa = null;

        //    SharedAPIController lBackEnd = new SharedAPIController();

        //    var lProject = lBackEnd.getProject(CustomerCode, lUserType, lGroupName);

        //    //return Json(lProject, JsonRequestBehavior.AllowGet);
        //    return Ok(lProject);

        //}

        //private static ClientContext LogOn(string userName, string password, Uri url)
        //{
        //    ClientContext clientContext = null;
        //    ClientContext ctx;
        //    try
        //    {
        //        clientContext = new ClientContext(url);

        //        // Condition to check whether the user name is null or empty.
        //        if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
        //        {
        //            SecureString securestring = new SecureString();
        //            password.ToCharArray().ToList().ForEach(s => securestring.AppendChar(s));
        //            clientContext.Credentials = new System.Net.NetworkCredential(userName, securestring);
        //            clientContext.ExecuteQuery();
        //        }
        //        else
        //        {
        //            clientContext.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;
        //            clientContext.ExecuteQuery();
        //        }

        //        ctx = clientContext;
        //    }
        //    finally
        //    {
        //        if (clientContext != null)
        //        {
        //            clientContext.Dispose();
        //        }
        //    }

        //    return ctx;
        //}

        private static ClientContext O365LogOn()
        {
            //string URL = "https://login.microsoftonline.com/common/oauth2/token";

            string lUserName = "DigiOSNoReply@natsteel.com.sg";
            string lPassWD = "NatSteel@123B";

            //string lUserName = "@natsteel.com.sg";
            //string lPassWD = "qdbdxlfxjbdfxjjz";

            ClientContext clientContext = null;
            ClientContext ctx = null;
            try
            {
                clientContext = new ClientContext(new Uri(gSiteURL));

                // Condition to check whether the user name is null or empty.
                if (!string.IsNullOrEmpty(lUserName) && !string.IsNullOrEmpty(lPassWD))
                {
                    //SecureString securestring = new SecureString();
                    //lPassWD.ToCharArray().ToList().ForEach(s => securestring.AppendChar(s));                

                    clientContext.Credentials = new SharePointOnlineCredentials(lUserName, lPassWD);
                    //clientContext.ExecuteQuery();
                }
                else
                {
                    clientContext.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;
                    //clientContext.ExecuteQuery();
                }
                ctx = clientContext;
            }
            finally
            {
                if (clientContext != null)
                {
                    clientContext.Dispose();
                }
            }
            return ctx;
        }

        //private static ClientContext ExtranetLogOn(string userName, string password, Uri url)
        //{
        //    ClientContext clientContext = null;
        //    ClientContext ctx;
        //    try
        //    {
        //        clientContext = new ClientContext(url);

        //        // Condition to check whether the user name is null or empty.
        //        if (!string.IsNullOrEmpty(userName))
        //        {
        //            NetworkCredential networkCredential = new NetworkCredential(userName, password);
        //            CredentialCache cc = new CredentialCache();
        //            cc.Add(url, "NTLM", networkCredential);
        //            clientContext.Credentials = cc;
        //            clientContext.ExecuteQuery();
        //        }
        //        else
        //        {
        //            CredentialCache cc = new CredentialCache();
        //            cc.Add(url, "NTLM", System.Net.CredentialCache.DefaultNetworkCredentials);
        //            clientContext.Credentials = cc;
        //            clientContext.ExecuteQuery();
        //        }
        //        ctx = clientContext;
        //    }
        //    finally
        //    {
        //        if (clientContext != null)
        //        {
        //            clientContext.Dispose();
        //        }
        //    }
        //    return ctx;
        //}

        bool SPLogout()
        {
            //GET https://login.microsoftonline.com/common/oauth2/logout?
            bool lReturn = false;
            using (var client = new HttpClient())
            {
                //setup client
                client.BaseAddress = new Uri("https://login.microsoftonline.com/common/oauth2/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //setup login data
                var formContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("post_logout_redirect_uri", ""),
                });

                //send request
                try
                {
                    HttpResponseMessage responseMessage = client.GetAsync("/logout").Result;

                    //get access token from response body
                    var responseJson = responseMessage.Content.ReadAsStringAsync().Result;
                    var jObject = JObject.Parse(responseJson);
                    if (jObject != null)
                    {
                        lReturn = true;
                    }
                }
                catch (Exception ex)
                {
                    string lErrorMsg = ex.Message;
                }

            }
            return lReturn;
        }

        string getCustomerName(string pCustomerCode)
        {
            string lCustomerName = "";

            var lCustomer = db.Customer.Find(pCustomerCode);
            if (lCustomer != null)
            {
                lCustomerName = lCustomer.CustomerName;
            }
            if (lCustomerName == null)
            {
                lCustomerName = "";
            }
            lCustomerName = lCustomerName.Trim();
            lCustomerName = removeSpecialChar(lCustomerName);
            //Sharepoint maximum folder/file nme length:260 
            if (lCustomerName.Length > 100)
            {
                lCustomerName = lCustomerName.Substring(0, 100).Trim();
            }
            lCustomerName = lCustomerName + " (" + pCustomerCode + ")";

            return lCustomerName;
        }

        string getProjectName(string pCustomerCode, string pProjectCode, string pUserID)
        {
            UserAccessController lUa = new UserAccessController();
            //var lUserType = lUa.getUserType(User.Identity.GetUserName());
            //var lGroupName = lUa.getGroupName(User.Identity.GetUserName());
            var lUserType = lUa.getUserType(pUserID);
            var lGroupName = lUa.getGroupName(pUserID);
            lUa = null;

            string lProjectTitle = "";

            var lSharedAPI = new SharedAPIController();

            //lProjectTitle = lSharedAPI.getProjectTitle(pCustomerCode, pProjectCode, lUserType, lGroupName);  //commented tempoaray

            //var lProject = (from p in db.ProjectList
            //                  where p.ProjectCode == pProjectCode
            //                  orderby p.ProjectTitle descending
            //                  select p).First();

            //if (lProject != null)
            //{
            //    lProjectTitle = lProject.ProjectTitle;
            //}

            if (lProjectTitle == null)
            {
                lProjectTitle = "";
            }
            lProjectTitle = lProjectTitle.Trim();
            lProjectTitle = removeSpecialChar(lProjectTitle);

            //Sharepoint maximum folder/file nme length:260 
            if (lProjectTitle.Length > 100)
            {
                lProjectTitle = lProjectTitle.Substring(0, 100).Trim();
            }
            lProjectTitle = lProjectTitle + " (" + pProjectCode + ")";

            return lProjectTitle;
        }

        string removeSpecialChar(string pInput)
        {
            string lOutput = pInput;

            lOutput = lOutput.Replace("*", "");
            lOutput = lOutput.Replace(":", "");
            lOutput = lOutput.Replace("<", "");
            lOutput = lOutput.Replace(">", "");
            lOutput = lOutput.Replace("?", "");
            lOutput = lOutput.Replace("/", "");
            lOutput = lOutput.Replace("\\", "");
            lOutput = lOutput.Replace("|", "");
            lOutput = lOutput.Replace("'", "");
            lOutput = lOutput.Replace("\"", "");
            lOutput = lOutput.Replace("#", "");
            lOutput = lOutput.Replace("%", "");

            return lOutput;
        }

        public static bool FolderExists(ClientContext context, Microsoft.SharePoint.Client.List list, string folderName)
        {
            string url = list.RootFolder.ServerRelativeUrl + "/" + folderName;
            return FolderExists(context, url);
        }

        public static bool FolderExists(ClientContext context, Microsoft.SharePoint.Client.Folder currentListFolder, string folderName)
        {
            string url = currentListFolder.ServerRelativeUrl + "/" + folderName;
            return FolderExists(context, url);
        }

        private static bool FolderExists(ClientContext context, string url)
        {
            var folder = context.Web.GetFolderByServerRelativeUrl(url);
            context.Load(folder, f => f.Exists);
            try
            {
                context.ExecuteQueryAsync();

                if (folder.Exists)
                {
                    return true;
                }
                return false;
            }
            //catch (ServerUnauthorizedAccessException uae)
            //{
            //    Trace.WriteLine($"You are not allowed to access this folder");
            //    throw Exception;
            //}
            catch (Exception ex)
            {
                return false;
            }
        }

        //public static bool FileExists(ClientContext context, Microsoft.SharePoint.Client.List list, string fileName)
        //{
        //    string url = list.RootFolder.ServerRelativeUrl + "/" + fileName;
        //    return FileExists(context, url);
        //}

        //public static bool FileExists(ClientContext context, Microsoft.SharePoint.Client.Folder currentListFolder, string fileName)
        //{
        //    string url = currentListFolder.ServerRelativeUrl + "/" + fileName;
        //    return FileExists(context, url);
        //}

        //private static bool FileExists(ClientContext context, string url)
        //{
        //    var file = context.Web.GetFileByServerRelativeUrl(url);
        //    context.Load(file, f => f.Exists);
        //    try
        //    {
        //        context.ExecuteQuery();

        //        if (file.Exists)
        //        {
        //            return true;
        //        }
        //        return false;
        //    }
        //    //catch (ServerUnauthorizedAccessException uae)
        //    //{
        //    //    Trace.WriteLine($"You are not allowed to access this file");
        //    //    throw;
        //    //}
        //    catch (Exception ex)
        //    {
        //        //Trace.WriteLine($"Could not find file {url}");
        //        return false;
        //    }
        //}

        [HttpPost]
        //[ValidateAntiForgeryHeader]
        [Route("/drawing")]
        public ActionResult uploadDrawingFiles()
        {
            var lCustomerCode = "0001101200";
            var lProjectCode = "0000113013";
            var lDrawingIDList = new List<int>();
            string lSQL = "";
            var lCmd = new SqlCommand();
            var lNDSCon = new SqlConnection();
            var lDa = new SqlDataAdapter();
            var lDs = new DataSet();

            UserAccessController lUa = new UserAccessController();
            var lUserType = lUa.getUserType(User.Identity.GetUserName());
            lUa = null;

            SqlDataReader lRst;
            string lSPFileName = "";
            int lFound = 0;

            var Content = new DrawingModels();
            try
            {
                int lRevision = 0;
                int lDrawingID = 0;
                int lOrderNumber = 294389;
                int.TryParse("", out lRevision);
                var lDrawingNo = "Test 1200";
                var lRemarks = "Test 1200 file";
                var lWBS1 = "BEAMMESH";
                var lWBS2 = "1";
                var lWBS3 = "BeamMesh P1";
                var lProdType = "CAB";
                var lStructureElement = "";
                var lUploadType = "BEAM";    // O:Overwrite; R-Revision
                var lScheduledProd = "R";
                int.TryParse("N", out lOrderNumber);

                if (lScheduledProd == null)
                {
                    lScheduledProd = "";
                }
                lScheduledProd = lScheduledProd.Trim();

                if (lScheduledProd == "")
                {
                    lScheduledProd = "N";
                }

                byte[] file1Content = Encoding.UTF8.GetBytes("This is the content of file 1.");
                byte[] file2Content = Encoding.UTF8.GetBytes("This is the content of file 2.");


                // Create a custom IFormFile implementation
                var formFile1 = new FormFile(new MemoryStream(file1Content), 0, file1Content.Length, "file1", "file1.txt");
                var formFile2 = new FormFile(new MemoryStream(file2Content), 0, file2Content.Length, "file2", "file2.txt");

                // Create a custom IFormCollection with hardcoded files
                var file1 = new Microsoft.AspNetCore.Http.FormCollection(new Dictionary<string, StringValues>(), new FormFileCollection
                    {
                        formFile1,
                        formFile2
                    });


                if (file1.Files.Count > 0)
                {
                    ClientContext lClient = O365LogOn();
                    Microsoft.SharePoint.Client.List docLib = lClient.Web.Lists.GetByTitle("Documents");
                    lClient.Load(docLib);
                    lClient.ExecuteQueryAsync();

                    var lProcessObj = new ProcessController();
                    lProcessObj.OpenNDSConnection(ref lNDSCon);
                   
                    IFormCollection files = file1;
                    for (int i = 0; i < files.Files.Count; i++)
                    {
                        lRevision = 0;
                    
                        IFormFile file = files.Files[i];

                        var lFileName = "STDMESH";

                        if (lFileName.LastIndexOf("\\") >= 0)
                        {
                            lFileName = lFileName.Substring(lFileName.LastIndexOf("\\") + 1);
                        }
                        if (lFileName.LastIndexOf("/") >= 0)
                        {
                            lFileName = lFileName.Substring(lFileName.LastIndexOf("/") + 1);
                        }
                        //lFileName = Server.HtmlDecode(lFileName);

                        lFileName = System.Web.HttpUtility.HtmlDecode(lFileName);
                        lFileName = removeSpecialChar(lFileName);

                        RegexOptions options = RegexOptions.None;
                        Regex regex = new Regex("[ ]{2,}", options);
                        lFileName = regex.Replace(lFileName, " ");

                        //byte[] pdfBytes = null;
                        //BinaryReader reader = new BinaryReader(file.InputStream);
                        //pdfBytes = reader.ReadBytes((int)file.ContentLength);

                        // take out -R0 -- -R99
                        if (lUserType == "TE")
                        {
                            lFileName = Regex.Replace(lFileName, @"-R\d+,", ",");
                        }

                        if (lUserType == "TE" && lFileName.LastIndexOf("-R") > 0)
                        {
                            int lExt = lFileName.LastIndexOf(".");
                            if (lExt > 0)
                            {
                                int lRevTestInt = 0;
                                string lRevTestChr = "";
                                int lRevPost = lFileName.LastIndexOf("-R");
                                lRevTestChr = lFileName.Substring(lRevPost, lExt - lRevPost);

                                if (lRevTestChr == "-R0" || (lRevTestChr.Length > 2 && lRevTestChr.Length < 6 && int.TryParse(lRevTestChr.Substring(2), out lRevTestInt) == true))
                                {
                                    lFileName = lFileName.Substring(0, lRevPost) + lFileName.Substring(lExt);
                                }
                            }
                        }

                        if (lFileName.Length > 100)
                        {
                            int lExt = lFileName.LastIndexOf(".");
                            if (lExt > 0)
                            {
                                lFileName = lFileName.Substring(0, 90) + lFileName.Substring(lExt);
                            }
                            else
                            {
                                lFileName = lFileName.Substring(0, 100);
                            }
                        }


                        lFound = 0;
                        lSQL = "SELECT Revision " +
                        "FROM dbo.OESDrawings " +
                        "WHERE CustomerCode = '" + lCustomerCode + "' " +
                        "AND ProjectCode = '" + lProjectCode + "' " +
                        "AND FileName = N'" + lFileName + "' ";

                        lCmd.CommandText = lSQL;
                        lCmd.Connection = lNDSCon;
                        lCmd.CommandTimeout = 300;
                        lRst = lCmd.ExecuteReader();
                        if (lRst.HasRows)
                        {
                            if (lRst.Read())
                            {
                                lRevision = lRst.GetInt32(0);
                                lFound = 1;
                            }
                        }
                        lRst.Close();


                        if (lUploadType == "R" && lFound == 1)
                        {
                            lRevision = lRevision + 1;
                            if (lDrawingNo != null && lRemarks != null && (lDrawingNo != "" || lRemarks != ""))
                            {
                                lSQL = "UPDATE dbo.OESDrawings " +
                                "SET Revision = " + lRevision.ToString() + ", " +
                                "DrawingNo = '" + lDrawingNo + "', " +
                                "Remarks = '" + lRemarks + "', " +
                                "UpdatedDate = getDate(), " +
                                "UpdatedBy = '" + User.Identity.Name + "' " +
                                "WHERE CustomerCode = '" + lCustomerCode + "' " +
                                "AND ProjectCode = '" + lProjectCode + "' " +
                                "AND FileName = N'" + lFileName + "' ";
                            }
                            else
                            {
                                lSQL = "UPDATE dbo.OESDrawings " +
                                "SET Revision = " + lRevision.ToString() + ", " +
                                "UpdatedDate = getDate(), " +
                                "UpdatedBy = '" + User.Identity.Name + "' " +
                                "WHERE CustomerCode = '" + lCustomerCode + "' " +
                                "AND ProjectCode = '" + lProjectCode + "' " +
                                "AND FileName = N'" + lFileName + "' ";
                            }
                            lCmd.CommandText = lSQL;
                            lCmd.Connection = lNDSCon;
                            lCmd.CommandTimeout = 300;
                            lCmd.ExecuteNonQuery();

                        }

                        if (lFileName.LastIndexOf(".") >= 0)
                        {
                            lSPFileName = lFileName.Substring(0, lFileName.LastIndexOf(".")) + "-R" + lRevision.ToString() + "" + lFileName.Substring(lFileName.LastIndexOf("."));
                        }
                        else
                        {
                            lSPFileName = lFileName + "-R" + lRevision.ToString();
                        }

                        string lCustomerName = getCustomerName(lCustomerCode);
                        string lProjectTitle = getProjectName(lCustomerCode, lProjectCode, User.Identity.GetUserName());
                        lCustomerName = removeSpecialChar(lCustomerName);
                        lProjectTitle = removeSpecialChar(lProjectTitle);
                        lFileName = removeSpecialChar(lFileName);
                        lCustomerName = "Customer";
                        lProjectTitle="Project";

                        if (FolderExists(lClient, "/sites/DigiOSDocs/Shared Documents/DrawingsLib/" + lCustomerName) == false)
                        {
                            var fld1 = docLib.RootFolder.Folders.GetByUrl("/sites/DigiOSDocs/Shared Documents/DrawingsLib");
                            //var fld1 = docLib.RootFolder;
                            var fld2 = fld1.Folders.Add(lCustomerName);
                            var fld3 = fld2.Folders.Add(lProjectTitle);
                            fld2.Update();
                            fld3.Update();
                            lClient.ExecuteQueryAsync();

                        }
                        else if (FolderExists(lClient, "/sites/DigiOSDocs/Shared Documents/DrawingsLib/" + lCustomerName + "/" + lProjectTitle) == false)
                        {
                            var fld1 = docLib.RootFolder.Folders.GetByUrl("/sites/DigiOSDocs/Shared Documents/DrawingsLib");
                            var fld2 = fld1.Folders.Add(lCustomerName);
                            var fld3 = fld2.Folders.Add(lProjectTitle);
                            fld3.Update();
                            lClient.ExecuteQueryAsync();
                        }

                        FileCreationInformation createFile = new FileCreationInformation();

                        var lFileURL = gSiteURL + "/Shared Documents/DrawingsLib/" + lCustomerName + "/" + lProjectTitle + "/" + lFileName;


                        string fileName = Path.GetFileName(gSiteURL);
                        string fileUrl = gSiteURL + "/Shared Documents/DrawingsLib/"+ lCustomerName + "/" + lProjectTitle + "/" + lFileName;
                        string localFilePath = @"D:\LocalPath\" + "STDMESH2.pdf"; // Replace with your local file path

                        using (FileStream fs = new FileStream(localFilePath, FileMode.Open))
                        {
                            FileCreationInformation fileInfo = new FileCreationInformation
                            {
                                Url = fileUrl,
                                Overwrite = true,
                                ContentStream = fs
                            };

                            Microsoft.SharePoint.Client.File uploadFile = docLib.RootFolder.Files.Add(fileInfo);
                            lClient.Load(uploadFile);
                            lClient.ExecuteQueryAsync();

                            Console.WriteLine("File uploaded: " + fileName);



                            using (var memoryStream = new MemoryStream())
                            {
                                //fs.CopyToAsync(memoryStream);
                                //createFile.ContentStream = memoryStream;
                                createFile.Url = lFileURL;
                                createFile.Overwrite = true;
                                createFile.Content = System.IO.File.ReadAllBytes(localFilePath);
                                Microsoft.SharePoint.Client.File newFile = docLib.RootFolder.Files.Add(createFile);

                                // Check out the file
                                newFile.CheckOut();

                                // Update metadata for the file if needed
                                // newFile.ListItemAllFields["FieldName"] = "Value";
                                // newFile.ListItemAllFields.Update();

                                // Check in the file
                                newFile.CheckIn("DigiOSNoReply@natsteel.com.sg", CheckinType.MajorCheckIn);

                                lClient.ExecuteQueryAsync();

                                

                                // Open the form file's read stream and copy it to the memory stream
                                //file.OpenReadStream().CopyTo(memoryStream);

                                //// Set the content of 'createFile' to the MemoryStream
                                //createFile.ContentStream = memoryStream;
                                //createFile.Url = lFileURL;
                                //createFile.Overwrite = true;
                                //Microsoft.SharePoint.Client.File newFile = docLib.RootFolder.Files.Add(createFile);


                                //newFile.CheckOut();

                                //newFile.ListItemAllFields.Update();

                                //newFile.CheckIn("DigiOSNoReply@natsteel.com.sg", CheckinType.MajorCheckIn);

                                //lClient.ExecuteQueryAsync();

                            }

                        }

                        //createFile.Content = pdfBytes;
                        //using (var memoryStream = new MemoryStream())
                        //{
                        //    file.CopyTo(memoryStream);
                        //    createFile.ContentStream = memoryStream;
                        //    createFile.Url = lFileURL;
                        //    createFile.Overwrite = true;
                        //    Microsoft.SharePoint.Client.File newFile = docLib.RootFolder.Files.Add(createFile);

                        //    // Check out the file
                        //    newFile.CheckOut();

                        //    // Update metadata for the file if needed
                        //    // newFile.ListItemAllFields["FieldName"] = "Value";
                        //    // newFile.ListItemAllFields.Update();

                        //    // Check in the file
                        //    newFile.CheckIn("DigiOSNoReply@natsteel.com.sg", CheckinType.MajorCheckIn);

                        //    lClient.ExecuteQueryAsync();

                        //    // Open the form file's read stream and copy it to the memory stream
                        //    //file.OpenReadStream().CopyTo(memoryStream);

                        //    //// Set the content of 'createFile' to the MemoryStream
                        //    //createFile.ContentStream = memoryStream;
                        //    //createFile.Url = lFileURL;
                        //    //createFile.Overwrite = true;
                        //    //Microsoft.SharePoint.Client.File newFile = docLib.RootFolder.Files.Add(createFile);


                        //    //newFile.CheckOut();

                        //    //newFile.ListItemAllFields.Update();

                        //    //newFile.CheckIn("DigiOSNoReply@natsteel.com.sg", CheckinType.MajorCheckIn);

                        //    //lClient.ExecuteQueryAsync();

                        //}
                        //createFile.ContentStream = file.InputStream;

                        //register
                        Content = db.Drawings.Find(lCustomerCode, lProjectCode, lFileName);
                        if (Content == null || Content.DrawingID == 0)
                        {
                            Content = new DrawingModels
                            {
                                CustomerCode = lCustomerCode,
                                ProjectCode = lProjectCode,
                                FileName = lFileName,
                                DrawingID = lDrawingID,
                                DrawingNo = lDrawingNo,
                                Revision = lRevision,
                                Remarks = lRemarks,
                                Status = "Active",
                                UpdatedDate = DateTime.Now,
                                UpdatedBy = User.Identity.Name
                            };
                            db.Drawings.Add(Content);
                            db.SaveChanges();

                            Content = db.Drawings.Find(lCustomerCode, lProjectCode, lFileName);
                        }

                        lDrawingIDList.Add(Content.DrawingID);

                        //Assign WBS
                        if (lWBS1 == null)
                        {
                            lWBS1 = "";
                        }
                        if (lWBS2 == null)
                        {
                            lWBS2 = "";
                        }
                        if (lWBS3 == null)
                        {
                            lWBS3 = "";
                        }
                        if (lProdType == null)
                        {
                            lProdType = "";
                        }
                        if (lStructureElement == null)
                        {
                            lStructureElement = "";
                        }
                        lWBS1 = lWBS1.Trim();
                        lWBS2 = lWBS2.Trim();
                        lWBS3 = lWBS3.Trim();
                        lProdType = lProdType.Trim();
                        lStructureElement = lStructureElement.Trim();

                        if (lWBS1 != "" && lWBS2 != "" && lProdType != "" && lStructureElement != "")
                        {
                            lFound = 0;

                            lSQL = "SELECT DrawingID " +
                            "FROM dbo.OESDrawingsWBS " +
                            "WHERE DrawingID = " + Content.DrawingID.ToString() + " " +
                            //"AND Revision = " + lRevision + " " +
                            "AND ProductType = '" + lProdType + "' " +
                            "AND StructureElement = '" + lStructureElement + "' " +
                            "AND WBS1 = '" + lWBS1 + "' " +
                            "AND WBS2 = '" + lWBS2 + "' " +
                            "AND WBS3 = '" + lWBS3 + "' ";

                            lCmd.CommandText = lSQL;
                            lCmd.Connection = lNDSCon;
                            lCmd.CommandTimeout = 300;
                            lRst = lCmd.ExecuteReader();
                            if (lRst.HasRows)
                            {
                                lFound = 1;
                            }
                            lRst.Close();

                            if (lFound == 0)
                            {
                                lSQL = "INSERT INTO dbo.OESDrawingsWBS " +
                                    "(DrawingID " +
                                    ", Revision " +
                                    ", ProductType " +
                                    ", StructureElement " +
                                    ", WBS1 " +
                                    ", WBS2 " +
                                    ", WBS3 " +
                                    ", UpdatedDate " +
                                    ", UpdatedBy) " +
                                    "VALUES " +
                                    "(" + Content.DrawingID.ToString() + " " +
                                    ", " + lRevision + " " +
                                    ",'" + lProdType + "' " +
                                    ",'" + lStructureElement + "' " +
                                    ",'" + lWBS1 + "' " +
                                    ",'" + lWBS2 + "' " +
                                    ",'" + lWBS3 + "' " +
                                    ",getDate() " +
                                    ",'" + User.Identity.Name + "') ";

                                lCmd.CommandText = lSQL;
                                lCmd.Connection = lNDSCon;
                                lCmd.CommandTimeout = 300;
                                lCmd.ExecuteNonQuery();
                            }
                        }

                        if (lOrderNumber > 0 && lProdType != "" && lStructureElement != "")
                        {
                            lFound = 0;

                            lSQL = "SELECT DrawingID " +
                            "FROM dbo.OESDrawingsOrder " +
                            "WHERE DrawingID = " + Content.DrawingID.ToString() + " " +
                            "AND Revision = " + lRevision + " " +
                            "AND OrderNumber = " + lOrderNumber + " " +
                            "AND ProductType = '" + lProdType + "' " +
                            "AND StructureElement = '" + lStructureElement + "' " +
                            "AND scheduledProd = '" + lScheduledProd + "' ";

                            lCmd.CommandText = lSQL;
                            lCmd.Connection = lNDSCon;
                            lCmd.CommandTimeout = 300;
                            lRst = lCmd.ExecuteReader();
                            if (lRst.HasRows)
                            {
                                lFound = 1;
                            }
                            lRst.Close();

                            if (lFound == 0)
                            {
                                lSQL = "INSERT INTO dbo.OESDrawingsOrder " +
                                    "(DrawingID " +
                                    ", Revision " +
                                    ", OrderNumber" +
                                    ", ProductType " +
                                    ", StructureElement " +
                                    ", ScheduledProd " +
                                    ", UpdatedDate " +
                                    ", UpdatedBy) " +
                                    "VALUES " +
                                    "(" + Content.DrawingID.ToString() + " " +
                                    ", " + lRevision + " " +
                                    ", " + lOrderNumber + " " +
                                    ",'" + lProdType + "' " +
                                    ",'" + lStructureElement + "' " +
                                    ",'" + lScheduledProd + "' " +
                                    ",getDate() " +
                                    ",'" + User.Identity.Name + "') ";

                                lCmd.CommandText = lSQL;
                                lCmd.Connection = lNDSCon;
                                lCmd.CommandTimeout = 300;
                                lCmd.ExecuteNonQuery();
                            }
                        }

                        //
                        if (lUploadType == "R" && lRevision > 0)
                        {
                            lFound = 0;


                            //lSQL = "SELECT " +
                            //"P.vchProductType, " +
                            //"S.vchStructureElementType, " +
                            //"W.vchWBS1, " +
                            //"isNull(MAX(convert(int, cast(vchWBS2 as numeric(10,3)))), 0) as WBS2 " +
                            //"FROM dbo.WBS U, " +
                            //"dbo.WBSElements W, " +
                            //"dbo.WBSElementsDetails D, " +
                            //"dbo.StructureElementMaster S, " +
                            //"dbo.ProductTypeMaster P, " +
                            //"dbo.ProjectMaster J " +
                            //"WHERE U.intWBSID = W.intWBSID " +
                            //"AND D.intWBSElementId = W.intWBSElementId " +
                            //"AND D.sitProductTypeId = P.sitProductTypeID " +
                            //"AND D.intStructureElementTypeId = S.intStructureElementTypeId " +
                            //"AND J.intProjectId = U.intProjectId " +
                            //"AND J.vchProjectCode = '" + lProjectCode + "' " +
                            //"AND EXISTS(SELECT R.intPostHeaderid " +
                            //"FROM dbo.BBSReleaseDetails R, dbo.BBSPostHeader B " +
                            //"WHERE R.intPostHeaderid = B.intPostHeaderId " +
                            //"AND B.intWBSElementId = W.intWBSElementId " +
                            //"AND R.tntStatusId = 12) " +
                            //"AND W.vchWBS2 NOT LIKE 'B%' " +
                            //"AND W.vchWBS2 NOT LIKE 'FDN%' " +
                            //"AND P.vchProductType <> 'BPC' " +
                            //"AND W.tntStatusId = 1 " +      
                            //"AND D.intConfirm = 1 " +
                            //"AND isNumeric(vchWBS2) = 1 " +
                            //"AND EXISTS ( SELECT DW.DrawingID FROM dbo.OESDrawingsWBS DW, dbo.OESDrawings HW " +
                            //"WHERE HW.DrawingID = " + Content.DrawingID.ToString() + " " +
                            //"AND HW.DrawingID = DW.DrawingID " +
                            //"AND HW.CustomerCode = '" + lCustomerCode + "' " +
                            //"AND HW.ProjectCode = '" + lProjectCode + "' " +
                            //"AND (CASE  " +
                            //"when DW.ProductType = 'MESH' then 'MSH' " +
                            //"when DW.ProductType = 'CUT-TO-SIZE-MESH' then 'MSH' " +
                            //"when DW.ProductType = 'STIRRUP-LINK-MESH' then 'MSH' " +
                            //"when DW.ProductType = 'COLUMN-LINK-MESH' then 'MSH' " +
                            //"when DW.ProductType = 'PRE-CAGE' then 'PRC' " +
                            //"when DW.ProductType = 'CORE-CAGE' then 'CAR' " +
                            //"when DW.ProductType = 'CARPET' then 'CAR' " +
                            //"else DW.ProductType END) = P.vchProductType " +
                            //"AND DW.StructureElement = S.vchStructureElementType " +
                            //"AND WBS1 = W.vchWBS1 " +
                            //"AND WBS2 = W.vchWBS2 " +
                            //"AND WBS3 = W.vchWBS3) " +
                            //"GROUP BY " +
                            //"P.vchProductType, " +
                            //"S.vchStructureElementType, " +
                            //"W.vchWBS1 " +
                            //"ORDER BY " +
                            //"P.vchProductType, " +
                            //"S.vchStructureElementType, " +
                            //"W.vchWBS1 ";

                            //lCmd.CommandType = CommandType.Text;
                            //lCmd.CommandText = lSQL;
                            //lCmd.CommandTimeout = 300;
                            //lCmd.Connection = lNDSCon;
                            //lDa.SelectCommand = lCmd;
                            //lDs = new DataSet();
                            //lDa.Fill(lDs);
                            //if (lDs.Tables[0].Rows.Count > 0)
                            //{
                            //    for (int j = 0; j < lDs.Tables[0].Rows.Count; j++)
                            //    {
                            //        var lProductType = lDs.Tables[0].Rows[j].ItemArray[0].ToString();
                            //        var lStructureEle = lDs.Tables[0].Rows[j].ItemArray[1].ToString();
                            //        var lWBS1A = lDs.Tables[0].Rows[j].ItemArray[2].ToString();
                            //        var lWBS2A = (int)lDs.Tables[0].Rows[j].ItemArray[3];

                            //        if (lWBS2A > 0)
                            //        {
                            //            lSQL = "UPDATE dbo.OESDrawingsWBS " +
                            //            "SET Revision = " + lRevision.ToString() + " " +
                            //            "WHERE DrawingID = " + Content.DrawingID.ToString() + " " +
                            //            "AND (CASE  " +
                            //            "when ProductType = 'MESH' then 'MSH' " +
                            //            "when ProductType = 'CUT-TO-SIZE-MESH' then 'MSH' " +
                            //            "when ProductType = 'STIRRUP-LINK-MESH' then 'MSH' " +
                            //            "when ProductType = 'COLUMN-LINK-MESH' then 'MSH' " +
                            //            "when ProductType = 'PRE-CAGE' then 'PRC' " +
                            //            "when ProductType = 'CORE-CAGE' then 'CAR' " +
                            //            "when ProductType = 'CARPET' then 'CAR' " +
                            //            "else ProductType END) = '" + lProductType + "' " +
                            //            "AND StructureElement = '" + lStructureEle + "' " +
                            //            "AND WBS1 = '" + lWBS1A + "' " +
                            //            "AND ((WBS2 > '" + lWBS2A.ToString() + "' " +
                            //            "AND LEN(WBS2) = LEN('" + lWBS2A.ToString() + "')) " +
                            //            "OR  LEN(WBS2) > LEN('" + lWBS2A.ToString() + "')) " +
                            //            "AND isNumeric(WBS2) = 1 " +
                            //            "AND NOT EXISTS(SELECT B.intWBSElementId " +
                            //            "FROM dbo.WBSElements W, " +
                            //            "dbo.WBSElementsDetails D, " +
                            //            "dbo.StructureElementMaster S, " +
                            //            "dbo.ProductTypeMaster P, " +
                            //            "dbo.BBSPostHeader B, " +
                            //            "dbo.BBSReleaseDetails R, " +
                            //            "dbo.ProjectMaster J " +
                            //            "WHERE B.intWBSElementId = W.intWBSElementId " +
                            //            "AND D.intWBSElementId = W.intWBSElementId " +
                            //            "AND D.sitProductTypeId = P.sitProductTypeID " +
                            //            "AND B.sitProductTypeId = P.sitProductTypeID " +
                            //            "AND D.intStructureElementTypeId = S.intStructureElementTypeId " +
                            //            "AND J.intProjectId = B.intProjectId " +
                            //            "AND R.intPostHeaderid = B.intPostHeaderId " +
                            //            "AND J.vchProjectCode = '" + lProjectCode + "' " +
                            //            "AND R.tntStatusId = 12 " +
                            //            "AND P.vchProductType = '" + lProductType + "' " +
                            //            "AND S.vchStructureElementType = '" + lStructureEle + "' " +
                            //            "AND W.vchWBS2 NOT LIKE 'B%' " +
                            //            "AND W.vchWBS2 NOT LIKE 'FDN%' " +
                            //            "AND W.vchWBS1 = dbo.OESDrawingsWBS.WBS1 " +
                            //            "AND W.vchWBS2 = dbo.OESDrawingsWBS.WBS2 " +
                            //            "AND W.vchWBS3 = dbo.OESDrawingsWBS.WBS3) ";

                            //            lCmd.CommandText = lSQL;
                            //            lCmd.Connection = lNDSCon;
                            //            lCmd.CommandTimeout = 300;
                            //            lCmd.ExecuteNonQuery();
                            //        }
                            //    }
                            //}

                            //Core Cage Product Type added at line 1619 Chetan

                            lSQL = "UPDATE dbo.OESDrawingsWBS " +
                            "SET Revision = " + lRevision.ToString() + " " +
                            "WHERE DrawingID = " + Content.DrawingID.ToString() + " " +
                            "AND NOT EXISTS(SELECT B.intWBSElementId " +
                            "FROM dbo.WBSElements W, " +
                            "dbo.WBSElementsDetails D, " +
                            "dbo.StructureElementMaster S, " +
                            "dbo.ProductTypeMaster P, " +
                            "dbo.BBSPostHeader B, " +
                            "dbo.BBSReleaseDetails R, " +
                            "dbo.SAPProjectMaster J " +
                            "WHERE B.intWBSElementId = W.intWBSElementId " +
                            "AND D.intWBSElementId = W.intWBSElementId " +
                            "AND D.sitProductTypeId = P.sitProductTypeID " +
                            "AND B.sitProductTypeId = P.sitProductTypeID " +
                            "AND D.intStructureElementTypeId = S.intStructureElementTypeId " +
                            "AND J.intProjectId = B.intProjectId " +
                            "AND R.intPostHeaderid = B.intPostHeaderId " +
                            "AND J.vchProjectCode = '" + lProjectCode + "' " +
                            "AND R.tntStatusId = 12 " +
                            "AND P.vchProductType = " +
                            "(CASE  " +
                            "when dbo.OESDrawingsWBS.ProductType = 'MESH' then 'MSH' " +
                            "when dbo.OESDrawingsWBS.ProductType = 'CUT-TO-SIZE-MESH' then 'MSH' " +
                            "when dbo.OESDrawingsWBS.ProductType = 'STIRRUP-LINK-MESH' then 'MSH' " +
                            "when dbo.OESDrawingsWBS.ProductType = 'COLUMN-LINK-MESH' then 'MSH' " +
                            "when dbo.OESDrawingsWBS.ProductType = 'PRE-CAGE' then 'PRC' " +
                            //"when dbo.OESDrawingsWBS.ProductType = 'CORE-CAGE' then 'CAR' " +
                            "when dbo.OESDrawingsWBS.ProductType = 'CORE-CAGE' then 'CORE' " +

                            "when dbo.OESDrawingsWBS.ProductType = 'CARPET' then 'CAR' " +
                            "else dbo.OESDrawingsWBS.ProductType END) " +
                            "AND S.vchStructureElementType = dbo.OESDrawingsWBS.StructureElement " +
                            "AND W.vchWBS1 = dbo.OESDrawingsWBS.WBS1 " +
                            "AND W.vchWBS2 = dbo.OESDrawingsWBS.WBS2 " +
                            "AND W.vchWBS3 = dbo.OESDrawingsWBS.WBS3) ";

                            lCmd.CommandText = lSQL;
                            lCmd.Connection = lNDSCon;
                            lCmd.CommandTimeout = 300;
                            lCmd.ExecuteNonQuery();

                        }
                    }

                    lProcessObj.CloseNDSConnection(ref lNDSCon);
                    lProcessObj = null;
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Server Error: " + ex.Message);
            }

            //ClientContext lClient = O365LogOn();

            var lSaved = (from p in db.Drawings
                          where p.CustomerCode == lCustomerCode &&
                          p.ProjectCode == lProjectCode &&
                          lDrawingIDList.Contains(p.DrawingID)
                          orderby p.DrawingID
                          select p).ToList();

            var lReturn = "";
            //var lReturn = Json(lSaved, JsonRequestBehavior.AllowGet); commented by vw
            //lReturn.MaxJsonLength = 50000000;

            return Ok(lReturn);
        }

        //  [HttpPost]
        //[ValidateAntiForgeryHeader]
        //public ActionResult deleteDrawing_backup(int DrawingID)
        //{
        //    var lCustomerCode = "";
        //    var lProjectCode = "";
        //    var lFileName = "";
        //    var lRevision = 0;

        //    string lSQL = "";
        //    var lCmd = new SqlCommand();
        //    var lNDSCon = new SqlConnection();
        //    SqlDataReader lRst;
        //    bool lReturn = true;
        //    string lErrorMsg = "";

        //    var Content = new DrawingModels();
        //    try
        //    {
        //        var lDrawing = (from p in db.Drawings
        //                        where p.DrawingID == DrawingID
        //                        select p).ToList();

        //        if (lDrawing != null && lDrawing.Count > 0)
        //        {
        //            lCustomerCode = lDrawing[0].CustomerCode;
        //            lProjectCode = lDrawing[0].ProjectCode;
        //            lFileName = lDrawing[0].FileName;
        //            lRevision = lDrawing[0].Revision;
        //        }

        //        if (lFileName.LastIndexOf(".") >= 0)
        //        {
        //            lFileName = lFileName.Substring(0, lFileName.LastIndexOf(".")) + "-R" + lRevision.ToString() + "" + lFileName.Substring(lFileName.LastIndexOf("."));
        //        }
        //        else
        //        {
        //            lFileName = lFileName + "-R" + lRevision.ToString();
        //        }

        //        var lOrder = (from p in db.DrawingsOrder
        //                      where p.DrawingID == DrawingID
        //                      select p).ToList();
        //        if (lOrder != null && lOrder.Count > 0)
        //        {
        //            lReturn = false;
        //            lErrorMsg = "Not allow to delete the drawing as there are orders already using it.";
        //            return Json(new { Success = lReturn, Message = lErrorMsg }, JsonRequestBehavior.AllowGet);
        //        }

        //        ClientContext lClient = O365LogOn();
        //        Microsoft.SharePoint.Client.List docLib = lClient.Web.Lists.GetByTitle("Documents");
        //        lClient.Load(docLib);
        //        lClient.ExecuteQuery();

        //        string lCustomerName = getCustomerName(lCustomerCode);
        //        string lProjectTitle = getProjectName(lCustomerCode, lProjectCode, User.Identity.GetUserName());
        //        lCustomerName = removeSpecialChar(lCustomerName);
        //        lProjectTitle = removeSpecialChar(lProjectTitle);

        //        string filePath = "/sites/DigiOSDocs/Shared Documents/DrawingsLib/" + lCustomerName + "/" + lProjectTitle + "/" + lFileName;

        //        var file = lClient.Web.GetFileByServerRelativeUrl(filePath);
        //        lClient.Load(file, f => f.Exists);
        //        file.DeleteObject();
        //        lClient.ExecuteQuery();

        //        var lProcessObj = new ProcessController();
        //        lProcessObj.OpenNDSConnection(ref lNDSCon);

        //        if (lRevision == 0)
        //        {
        //            lSQL = "DELETE FROM dbo.OESDrawings " +
        //            "where DrawingID = " + DrawingID + " ";

        //            lCmd.CommandText = lSQL;
        //            lCmd.Connection = lNDSCon;
        //            lCmd.CommandTimeout = 300;
        //            lCmd.ExecuteNonQuery();
        //        }
        //        else
        //        {
        //            lRevision = lRevision - 1;
        //            lSQL = "Update dbo.OESDrawings " +
        //            "SET Revision = " + lRevision.ToString() + " " +
        //            "where DrawingID = " + DrawingID + " ";

        //            lCmd.CommandText = lSQL;
        //            lCmd.Connection = lNDSCon;
        //            lCmd.CommandTimeout = 300;
        //            lCmd.ExecuteNonQuery();
        //        }

        //        lProcessObj.CloseNDSConnection(ref lNDSCon);
        //        lProcessObj = null;

        //    }
        //    catch (Exception ex)
        //    {
        //        lReturn = false;
        //        lErrorMsg = ex.Message;
        //    }

        //    return Json(new { success = lReturn, message = lErrorMsg }, JsonRequestBehavior.AllowGet);
        //}

        [HttpGet]
        [Route("/getWBSList/{CustomerCode}/{ProjectCode}/{DrawingID}/{Revision}")]
        public ActionResult getWBSList(string CustomerCode, string ProjectCode, int DrawingID, int Revision)
        {
            var lDa = new SqlDataAdapter();
            var lCmd = new SqlCommand();
            var lDs = new DataSet();
            var cnNDS = new SqlConnection();
            string lSQL = "";
            var lUserName = User.Identity.GetUserName().Split('@');

            var lReturn = (new[]{ new
            {
                DrawingID = 0,
                Revision= 0,
                ProductType = "",
                StructureElement = "",
                WBS1 = "",
                WBS2 = "",
                WBS3 = "",
                Status = "",
                UpdateBy = "",
                UpdateDate = DateTime.Now,
                ProductTypeDis = ""
            }
            }).ToList();

            if (lReturn.Count > 0)
            {
                lReturn.RemoveAt(0);
            }

            //Core Cage Product Type added at line 1843 Chetan


            if (lUserName.Length == 2 && lUserName[1].ToLower().Trim() == "natsteel.com.sg")
            {
                lSQL = "SELECT " +
                "O.DrawingID, " +
                "O.Revision, " +
                "O.ProductType, " +
                "O.StructureElement, " +
                "O.WBS1, " +
                "O.WBS2, " +
                "O.WBS3, " +
                "isNull((SELECT isNULl(N.vchStatus, 'Prepared') " +
                "FROM dbo.BBSReleaseDetails R, dbo.NDSStatusMaster N " +
                "WHERE R.tntStatusId = N.tntStatusId " +
                "AND R.intPostHeaderid = MAX(B.intPostHeaderId)), 'Prepared') as ReleaseStatus, " +
                "O.UpdatedBy, " +
                "O.UpdatedDate, " +
                "(CASE  " +
                "when O.ProductType = 'CUT-TO-SIZE-MESH' then 'MESH' " +
                "when O.ProductType = 'STIRRUP-LINK-MESH' then 'MESH' " +
                "when O.ProductType = 'COLUMN-LINK-MESH' then 'MESH' " +
                "else O.ProductType END) as ProductTypeDis " +
                "FROM dbo.OESDrawingsWBS O, " +
                "dbo.WBSElements W, " +
                "dbo.WBSElementsDetails D, " +
                "dbo.StructureElementMaster S, " +
                "dbo.ProductTypeMaster P, " +
                "dbo.BBSPostHeader B, " +
                "dbo.OESDrawings M, " +
                "dbo.SAPProjectMaster J " +
                "WHERE B.intWBSElementId = W.intWBSElementId " +
                "AND D.intWBSElementId = W.intWBSElementId " +
                "AND D.sitProductTypeId = P.sitProductTypeID " +
                "AND B.sitProductTypeId = P.sitProductTypeID " +
                "AND D.intStructureElementTypeId = S.intStructureElementTypeId " +
                "AND P.vchProductType = " +
                "(CASE  " +
                "when O.ProductType = 'MESH' then 'MSH' " +
                "when O.ProductType = 'CUT-TO-SIZE-MESH' then 'MSH' " +
                "when O.ProductType = 'STIRRUP-LINK-MESH' then 'MSH' " +
                "when O.ProductType = 'COLUMN-LINK-MESH' then 'MSH' " +
                "when O.ProductType = 'PRE-CAGE' then 'PRC' " +
                // "when O.ProductType = 'CORE-CAGE' then 'CAR' " +
                 "when O.ProductType = 'CORE-CAGE' then 'CORE' " +
                "when O.ProductType = 'CARPET' then 'CAR' " +
                "else O.ProductType END) " +
                "AND S.vchStructureElementType = O.StructureElement " +
                "AND J.intProjectId = B.intProjectId " +
                "AND J.vchProjectCode = M.ProjectCode " +
                "AND M.DrawingID = O.DrawingID " +
                "AND O.WBS1 = W.vchWBS1 " +
                "AND O.WBS2 = W.vchWBS2 " +
                "AND O.WBS3 = W.vchWBS3 " +
                "AND M.ProjectCode = '" + ProjectCode + "' " +
                "AND O.DrawingID = " + DrawingID + " " +
                //"AND O.Revision = " + Revision + " " +
                "GROUP BY " +
                "O.DrawingID, " +
                "O.Revision, " +
                "O.ProductType, " +
                "O.StructureElement, " +
                "O.WBS1, " +
                "O.WBS2, " +
                "O.WBS3, " +
                "O.UpdatedBy, " +
                "O.UpdatedDate " +
                "ORDER BY " +
                "O.ProductType, " +
                "O.StructureElement, " +
                "O.WBS1, " +
                "LEN(O.WBS2), O.WBS2, " +
                "O.WBS3 ";
            }
            else
            {
                //Core Cage Product Type added at line 1916 Chetan

                lSQL = "SELECT " +
                "O.DrawingID, " +
                "O.Revision, " +
                "O.ProductType, " +
                "O.StructureElement, " +
                "O.WBS1, " +
                "O.WBS2, " +
                "O.WBS3, " +
                "isNull((SELECT isNULl(N.vchStatus, 'Prepared') " +
                "FROM dbo.BBSReleaseDetails R, dbo.NDSStatusMaster N " +
                "WHERE R.tntStatusId = N.tntStatusId " +
                "AND R.intPostHeaderid = MAX(B.intPostHeaderId)), 'Prepared') as ReleaseStatus, " +
                "O.UpdatedBy, " +
                "O.UpdatedDate, " +
                "(CASE  " +
                "when O.ProductType = 'CUT-TO-SIZE-MESH' then 'MESH' " +
                "when O.ProductType = 'STIRRUP-LINK-MESH' then 'MESH' " +
                "when O.ProductType = 'COLUMN-LINK-MESH' then 'MESH' " +
                "else O.ProductType END) as ProductTypeDis " +
                "FROM dbo.OESDrawingsWBS O, " +
                "dbo.WBSElements W, " +
                "dbo.WBSElementsDetails D, " +
                "dbo.StructureElementMaster S, " +
                "dbo.ProductTypeMaster P, " +
                "dbo.BBSPostHeader B, " +
                "dbo.OESDrawings M, " +
                "dbo.SAPProjectMaster J " +
                "WHERE B.intWBSElementId = W.intWBSElementId " +
                "AND D.intWBSElementId = W.intWBSElementId " +
                "AND D.sitProductTypeId = P.sitProductTypeID " +
                "AND B.sitProductTypeId = P.sitProductTypeID " +
                "AND D.intStructureElementTypeId = S.intStructureElementTypeId " +
                "AND P.vchProductType = " +
                "(CASE  " +
                "when O.ProductType = 'MESH' then 'MSH' " +
                "when O.ProductType = 'CUT-TO-SIZE-MESH' then 'MSH' " +
                "when O.ProductType = 'STIRRUP-LINK-MESH' then 'MSH' " +
                "when O.ProductType = 'COLUMN-LINK-MESH' then 'MSH' " +
                //"when O.ProductType = 'PRE-CAGE' then 'PRC' " +
                "when O.ProductType = 'CORE-CAGE' then 'CORE' " +
                "when O.ProductType = 'CARPET' then 'CAR' " +
                "else O.ProductType END) " +
                "AND S.vchStructureElementType = O.StructureElement " +
                "AND J.intProjectId = B.intProjectId " +
                "AND J.vchProjectCode = M.ProjectCode " +
                "AND M.DrawingID = O.DrawingID " +
                "AND O.WBS1 = W.vchWBS1 " +
                "AND O.WBS2 = W.vchWBS2 " +
                "AND O.WBS3 = W.vchWBS3 " +
                "AND M.ProjectCode = '" + ProjectCode + "' " +
                "AND O.DrawingID = " + DrawingID + " " +
                //"AND O.Revision = " + Revision + " " +
                "GROUP BY " +
                "O.DrawingID, " +
                "O.Revision, " +
                "O.ProductType, " +
                "O.StructureElement, " +
                "O.WBS1, " +
                "O.WBS2, " +
                "O.WBS3, " +
                "O.UpdatedBy, " +
                "O.UpdatedDate " +
                "ORDER BY " +
                "O.ProductType, " +
                "O.StructureElement, " +
                "O.WBS1, " +
                "LEN(O.WBS2), O.WBS2, " +
                "O.WBS3 ";
            }
            var lProcess = new ProcessController();
            lProcess.OpenNDSConnection(ref cnNDS);
            if (cnNDS.State == ConnectionState.Open)
            {
                lCmd.CommandType = CommandType.Text;
                lCmd.CommandText = lSQL;
                lCmd.Connection = cnNDS;
                lDa.SelectCommand = lCmd;
                lDs = new DataSet();
                lDa.Fill(lDs);
                if (lDs.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < lDs.Tables[0].Rows.Count; i++)
                    {
                        lReturn.Add(new
                        {
                            DrawingID = (int)lDs.Tables[0].Rows[i].ItemArray[0],
                            Revision = (int)lDs.Tables[0].Rows[i].ItemArray[1],
                            ProductType = lDs.Tables[0].Rows[i].ItemArray[2].ToString().Trim(),
                            StructureElement = lDs.Tables[0].Rows[i].ItemArray[3].ToString().Trim(),
                            WBS1 = lDs.Tables[0].Rows[i].ItemArray[4].ToString().Trim(),
                            WBS2 = lDs.Tables[0].Rows[i].ItemArray[5].ToString().Trim(),
                            WBS3 = lDs.Tables[0].Rows[i].ItemArray[6].ToString().Trim(),
                            Status = lDs.Tables[0].Rows[i].ItemArray[7].ToString().Trim(),
                            UpdateBy = lDs.Tables[0].Rows[i].ItemArray[8].ToString().Trim(),
                            UpdateDate = (DateTime)lDs.Tables[0].Rows[i].ItemArray[9],
                            ProductTypeDis = lDs.Tables[0].Rows[i].ItemArray[10].ToString().Trim()
                        });
                    }
                }
                lProcess.CloseNDSConnection(ref cnNDS);
            }

            return Ok(lReturn);
        }

        [HttpPost]
        [Route("/chkFileExist")]
        public ActionResult chkFileExist(FileExsistDto fileExsistDto)
        {
            var lReturn = 0;
            string lSQL = "";
            var lCmd = new SqlCommand();
            var lNDSCon = new SqlConnection();
            SqlDataReader lRst;

            if (fileExsistDto.FileName != null && fileExsistDto.FileName.Count > 0)
            {
                var lCond = "";
                for (int i = 0; i < fileExsistDto.FileName.Count; i++)
                {
                    if (fileExsistDto.FileName[i] != null && fileExsistDto.FileName[i].Trim() != "")
                    {
                        if (lCond == "")
                        {
                            lCond = " FileName = '" + fileExsistDto.FileName[i] + "' ";
                        }
                        else
                        {
                            lCond = lCond + " OR FileName = '" + fileExsistDto.FileName[i] + "' ";
                        }
                    }
                }

                if (lCond != "")
                {
                    var lProcessObj = new ProcessController();
                    lProcessObj.OpenNDSConnection(ref lNDSCon);

                    try
                    {
                        lSQL = "SELECT COUNT(*) FROM dbo.OESDrawings " +
                        "WHERE CustomerCode = '" + fileExsistDto.CustomerCode + "' " +
                        "AND ProjectCode = '" + fileExsistDto.ProjectCode + "' " +
                        "AND (" + lCond + ") ";

                        lCmd.CommandText = lSQL;
                        lCmd.Connection = lNDSCon;
                        lCmd.CommandTimeout = 300;
                        lRst = lCmd.ExecuteReader();

                        if (lRst.HasRows)
                        {
                            if (lRst.Read())
                            {
                                lReturn = lRst.GetInt32(0);
                            }
                        }
                        lRst.Close();

                        lProcessObj.CloseNDSConnection(ref lNDSCon);
                        lProcessObj = null;

                    }
                    catch (Exception ex)
                    {
                        lReturn = 0;
                    }
                }
            }

            return Ok(lReturn);
        }

        [HttpPost]
        [Route("/checkOrder/{DrawingID}/{Revision}")]
        public ActionResult checkOrder(int DrawingID, int Revision)
        {
            var lList = (from p in db.DrawingsOrder
                         join
                         s in db.OrderProjectSE
                         on new { a = p.OrderNumber, b = p.ProductType, c = p.StructureElement, d = p.ScheduledProd }
                         equals new { a = s.OrderNumber, b = s.ProductType, c = s.StructureElement, d = s.ScheduledProd }
                         join
                         t in db.OrderProject on s.OrderNumber equals t.OrderNumber
                         where p.DrawingID == DrawingID &&
                         p.Revision == Revision &&
                         s.OrderStatus != "Deleted" &&
                         s.OrderStatus != "Cancelled" &&
                         t.OrderStatus != "Deleted" &&
                         t.OrderStatus != "Cancelled"
                         select p.DrawingID).Count();


            //return Json(lList, JsonRequestBehavior.AllowGet);
            return Ok(lList);

        }

        [HttpPost]
        [Route("/getOrderList/{DrawingID}/{Revision}")]
        public ActionResult getOrderList(int DrawingID, int Revision)
        {
            var lList = (from p in db.DrawingsOrder
                         join
                         s in db.OrderProjectSE
                         on new { a = p.OrderNumber, b = p.ProductType, c = p.StructureElement, d = p.ScheduledProd }
                         equals new { a = s.OrderNumber, b = s.ProductType, c = s.StructureElement, d = s.ScheduledProd }
                         join
                         t in db.OrderProject on s.OrderNumber equals t.OrderNumber
                         where p.DrawingID == DrawingID &&
                         p.Revision == Revision &&
                         s.OrderStatus != "Deleted" &&
                         s.OrderStatus != "Cancelled" &&
                         s.OrderStatus != "Created" &&
                         t.OrderStatus != "Deleted" &&
                         t.OrderStatus != "Cancelled"
                         select
                         new
                         {
                             p.OrderNumber,
                             p.ProductType,
                             p.StructureElement,
                             p.ScheduledProd,
                             s.PONumber,
                             s.RequiredDate,
                             s.TotalWeight,
                             s.OrderStatus,
                             s.UpdateBy,
                             s.UpdateDate
                         }).ToList();

            // return Json(lList, JsonRequestBehavior.AllowGet);
            return Ok(lList);
        }

        //[HttpPost]
        ////[ValidateAntiForgeryHeader]
        //public ActionResult getWexBim(int DrawingID, int Revision)
        //{
        //    try
        //    {
        //        var Content = (from p in db.Drawings
        //                       where p.DrawingID == DrawingID
        //                       select p).ToList();

        //        if (Content != null && Content.Count > 0)
        //        {
        //            string lCustomerName = getCustomerName(Content[0].CustomerCode);
        //            string lProjectTitle = getProjectName(Content[0].CustomerCode, Content[0].ProjectCode, User.Identity.GetUserName());
        //            lCustomerName = removeSpecialChar(lCustomerName);
        //            lProjectTitle = removeSpecialChar(lProjectTitle);

        //            string lFileName = Content[0].FileName;
        //            string lWexBIMFile = "";

        //            if (lFileName.LastIndexOf(".") > 0)
        //            {
        //                lFileName = lFileName.Substring(0, lFileName.LastIndexOf(".")) + "-R" + Revision.ToString() + lFileName.Substring(lFileName.LastIndexOf("."));
        //            }
        //            else
        //            {
        //                lFileName = lFileName + "-R" + Revision.ToString();
        //            }

        //            if (lFileName.LastIndexOf(".") > 0)
        //            {
        //                lWexBIMFile = lFileName.Substring(0, lFileName.LastIndexOf(".")) + ".wexbim";
        //            }
        //            else
        //            {
        //                lWexBIMFile = lFileName + ".wexbim";
        //                lFileName = lFileName + ".ifc";
        //            }

        //            //string lServerRelative = gSiteURL + "/Shared Documents/DrawingsLib/" + lCustomerName + "/" + lProjectTitle + "/" + Content[0].FileName;
        //            string lServerRelative = "/sites/DigiOSDocs/Shared Documents/DrawingsLib/" + lCustomerName + "/" + lProjectTitle + "/" + lWexBIMFile;

        //            ClientContext lClient = O365LogOn();
        //            Microsoft.SharePoint.Client.List docLib = lClient.Web.Lists.GetByTitle("Documents");
        //            lClient.Load(docLib);
        //            var file = lClient.Web.GetFileByServerRelativeUrl(lServerRelative);
        //            lClient.Load(file, f => f.Exists); // Only load the Exists property
        //            lClient.ExecuteQuery();

        //            byte[] lFileBinary = new byte[] { };

        //            if (file.Exists == true)
        //            {
        //                lClient.Load(file);
        //                lClient.ExecuteQuery();
        //                ClientResult<Stream> streamResult = file.OpenBinaryStream();
        //                lClient.ExecuteQuery();

        //                var lMemoryStream = new MemoryStream();
        //                streamResult.Value.CopyTo(lMemoryStream);

        //                lMemoryStream.Position = 0;
        //                lFileBinary = lMemoryStream.GetBuffer();
        //            }
        //            else
        //            {
        //                lServerRelative = "/sites/DigiOSDocs/Shared Documents/DrawingsLib/" + lCustomerName + "/" + lProjectTitle + "/" + lFileName;

        //                file = lClient.Web.GetFileByServerRelativeUrl(lServerRelative);
        //                lClient.Load(file);
        //                lClient.ExecuteQuery();

        //                ClientResult<Stream> streamResult = file.OpenBinaryStream();
        //                lClient.ExecuteQuery();

        //                var lMemoryStream = new MemoryStream();
        //                streamResult.Value.CopyTo(lMemoryStream);

        //                lMemoryStream.Position = 0;

        //                var lDataType = Xbim.IO.StorageType.Ifc;
        //                if (lFileName.LastIndexOf(".") > 0)
        //                {
        //                    var lExt = lFileName.Substring(lFileName.LastIndexOf(".") + 1);
        //                    if (lExt.ToUpper() == "IFCZIP")
        //                    {
        //                        lDataType = Xbim.IO.StorageType.IfcZip;
        //                    }
        //                    else if (lExt.ToUpper() == "IFCXML")
        //                    {
        //                        lDataType = Xbim.IO.StorageType.IfcXml;
        //                    }
        //                }

        //                var model = IfcStore.Open(lMemoryStream, lDataType, Xbim.Common.Step21.XbimSchemaVersion.Ifc2X3, XbimModelType.MemoryModel);

        //                model.ModelFactors.Precision = 0.1;
        //                model.ModelFactors.ProfileDefLevelOfDetail = 0;
        //                model.ModelFactors.DeflectionTolerance = 0.5;
        //                model.ModelFactors.DeflectionAngle = 0.5;

        //                //var model = IfcStore.Open("D:\\temp\\Revit Rebar.ifc");
        //                using (var lWexBimStream = new MemoryStream())
        //                {
        //                    var context = new Xbim3DModelContext(model);
        //                    context.CreateContext();

        //                    using (BinaryWriter lBinaryWriter = new BinaryWriter(lWexBimStream))
        //                    {

        //                        //lWexBimStream.Capacity = 90000000;
        //                        lBinaryWriter.Seek(0, SeekOrigin.Begin);

        //                        model.SaveAsWexBim(lBinaryWriter);

        //                        FileCreationInformation createFile = new FileCreationInformation();
        //                        lWexBimStream.Position = 0;
        //                        createFile.ContentStream = lWexBimStream;
        //                        //createFile.Url = siteUrl + "/Documents/Drawings/" + lCustomerName + "/" + lProjectTitle + "/" + lFileName;
        //                        lServerRelative = "/sites/DigiOSDocs/Shared Documents/DrawingsLib/" + lCustomerName + "/" + lProjectTitle + "/" + lWexBIMFile;
        //                        createFile.Url = lServerRelative;
        //                        createFile.Overwrite = true;
        //                        Microsoft.SharePoint.Client.File newFile = docLib.RootFolder.Files.Add(createFile);

        //                        newFile.CheckOut();

        //                        newFile.ListItemAllFields.Update();

        //                        newFile.CheckIn(User.Identity.Name, CheckinType.MajorCheckIn);

        //                        lClient.ExecuteQuery();

        //                        lFileBinary = lWexBimStream.GetBuffer();

        //                        lWexBimStream.Flush();

        //                        lBinaryWriter.Close();

        //                    }
        //                }
        //            }

        //            System.Net.Mime.ContentDisposition cd = new System.Net.Mime.ContentDisposition
        //            {
        //                FileName = lWexBIMFile,
        //                Inline = false
        //            };

        //            string contentType = MimeMapping.GetMimeMapping(lWexBIMFile);

        //            //Response.AppendHeader("Content-Disposition", cd.ToString());

        //            return File(lFileBinary, "application/octet-stream", lWexBIMFile);
        //            //return File(binary, "application/octet-stream", wexBim);


        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError(string.Empty, "Server Error: " + ex.Message);
        //    }
        //    //var lReturn = Json(imageDataURL, JsonRequestBehavior.AllowGet);
        //    //lReturn.MaxJsonLength = 80000000;
        //    return null;
        //    //return File(lFileBinary, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

        //}

        ////[ValidateAntiForgeryHeader]
        //public ActionResult viewDrawing(int DrawingID, int Revision)
        //{
        //    //int lDrawingID = 0;
        //    //if (DrawingID.IndexOf(".") > 0)
        //    //{
        //    //    DrawingID = DrawingID.Substring(0, DrawingID.IndexOf("."));
        //    //    int.TryParse(DrawingID, out lDrawingID);
        //    //}
        //    string imageDataURL = "";

        //    try
        //    {
        //        var Content = (from p in db.Drawings
        //                       where p.DrawingID == DrawingID
        //                       select p).ToList();

        //        if (Content != null && Content.Count > 0)
        //        {
        //            string lCustomerName = getCustomerName(Content[0].CustomerCode);
        //            string lProjectTitle = getProjectName(Content[0].CustomerCode, Content[0].ProjectCode, User.Identity.GetUserName());
        //            lCustomerName = removeSpecialChar(lCustomerName);
        //            lProjectTitle = removeSpecialChar(lProjectTitle);

        //            string lFileName = Content[0].FileName;

        //            if (lFileName.LastIndexOf(".") >= 0)
        //            {
        //                lFileName = lFileName.Substring(0, lFileName.LastIndexOf(".")) + "-R" + Revision.ToString() + "" + lFileName.Substring(lFileName.LastIndexOf("."));
        //            }
        //            else
        //            {
        //                lFileName = lFileName + "-R" + Revision.ToString();
        //            }

        //            //string lServerRelative = gSiteURL + "/Shared Documents/DrawingsLib/" + lCustomerName + "/" + lProjectTitle + "/" + Content[0].FileName;
        //            string lServerRelative = "/sites/DigiOSDocs/Shared Documents/DrawingsLib/" + lCustomerName + "/" + lProjectTitle + "/" + lFileName;

        //            ClientContext lClient = O365LogOn();
        //            Microsoft.SharePoint.Client.List docLib = lClient.Web.Lists.GetByTitle("Documents");
        //            lClient.Load(docLib);
        //            lClient.ExecuteQuery();

        //            var file = lClient.Web.GetFileByServerRelativeUrl(lServerRelative);
        //            lClient.Load(file);
        //            lClient.ExecuteQuery();
        //            ClientResult<Stream> streamResult = file.OpenBinaryStream();
        //            lClient.ExecuteQuery();

        //            var lMemoryStream = new MemoryStream();
        //            streamResult.Value.CopyTo(lMemoryStream);
        //            var lFileBinary = lMemoryStream.ToArray();

        //            //var result = Convert.ToBase64String(lFileBinary);


        //            System.Net.Mime.ContentDisposition cd = new System.Net.Mime.ContentDisposition
        //            {
        //                FileName = lFileName,
        //                Inline = false
        //            };

        //            string contentType = MimeMapping.GetMimeMapping(lFileName);

        //            //Response.AppendHeader("Content-Disposition", cd.ToString());

        //            //Response.Headers.Add("X-Content-Type-Options", "nosniff");

        //            //Response.AppendHeader("Content-Disposition", "inline; filename=" + Content[0].FileName + "");
        //            //Response.AppendHeader("Content-Type", contentType);

        //            //Response.AppendHeader("Content-Disposition", "attachment;filename=" + Content[0].FileName + "; size=" + lFileBinary.Length.ToString());
        //            //Response.ContentType = contentType;
        //            //var readStream = System.IO.File.ReadAllBytes(filePath);
        //            //return File(lFileBinary, contentType, Content[0].FileName);
        //            Response.AppendHeader("Access-Control-Allow-Origin", "*");
        //            Response.AppendHeader("Access-Control-Allow-Methods", "POST, GET, OPTIONS");
        //            Response.AppendHeader("Access-Control-Allow-Headers", "X-PINGOTHER, Content-Type");
        //            //return File(lFileBinary, contentType, Content[0].FileName);
        //            //return File(lFileBinary, "application/pdf", lFileName);
        //            return File(lFileBinary, contentType, lFileName);

        //            //return new FileContentResult(lFileBinary, contentType);

        //            //direct attach
        //            //Response.Clear();
        //            //Response.ClearHeaders();
        //            //Response.ClearContent();
        //            //Response.AddHeader("Content-Disposition", "attachment; filename=" + Content[0].FileName + "; size=" + lMemoryStream.Length.ToString());
        //            //Response.ContentType = contentType;
        //            //Response.AddHeader("Content-Type", contentType);
        //            //int bufferSize = 65536;
        //            //byte[] byteBuffer = new byte[bufferSize];

        //            //while (lMemoryStream.Read(byteBuffer, 0, byteBuffer.Length) > 0)
        //            //{
        //            //    Response.BinaryWrite(lMemoryStream.ToArray());
        //            //}

        //            //Response.BinaryWrite(lFileBinary);

        //            //Response.AddHeader("Content-Disposition", "attachment; filename=\"" + Content[0].FileName + "\"; size=" + lMemoryStream.Length.ToString());
        //            //Response.ContentType = contentType;
        //            //Response.AddHeader("Content-Type", contentType);

        //            //Response.Flush();

        //            //try
        //            //{

        //            //    var LengthToRead = lMemoryStream.Length;
        //            //    int bytesToRead = 65536;

        //            //    //Indicate the type of data being sent
        //            //    Response.ContentType = "application/octet-stream";

        //            //    //Name the file 
        //            //    Response.AddHeader("Content-Disposition", "attachment; filename=\"Content[0].FileName\"");
        //            //    Response.AddHeader("Content-Length", LengthToRead.ToString());

        //            //    int length;
        //            //    lMemoryStream.Position = 0;
        //            //    do
        //            //    {
        //            //        // Verify that the client is connected.
        //            //        if (Response.IsClientConnected)
        //            //        {
        //            //            byte[] buffer = new Byte[bytesToRead];

        //            //            // Read data into the buffer.
        //            //            length = lMemoryStream.Read(buffer, 0, bytesToRead);

        //            //            // and write it out to the response's output stream
        //            //            Response.OutputStream.Write(buffer, 0, length);

        //            //            // Flush the data
        //            //            Response.Flush();

        //            //            //Clear the buffer
        //            //            LengthToRead = LengthToRead - length;
        //            //        }
        //            //        else
        //            //        {
        //            //            // cancel the download if client has disconnected
        //            //            LengthToRead = -1;
        //            //        }
        //            //    } while (LengthToRead > 0); //Repeat until no data is read

        //            //}
        //            //finally
        //            //{
        //            //    if (lMemoryStream != null)
        //            //    {
        //            //        //Close the input stream                   
        //            //        lMemoryStream.Close();
        //            //    }
        //            //    Response.End();
        //            //    Response.Close();
        //            //}

        //            //return View("Failed");

        //            //return null;
        //            //return File(lFileBinary, System.Net.Mime.MediaTypeNames.Application.Octet, Content[0].FileName);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError(string.Empty, "Server Error: " + ex.Message);
        //    }
        //    //var lReturn = Json(imageDataURL, JsonRequestBehavior.AllowGet);
        //    //lReturn.MaxJsonLength = 80000000;
        //    return null;
        //    //return File(lFileBinary, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

        //}

        //[ValidateAntiForgeryHeader]
        //public ActionResult viewPDFDrawing(int DrawingID, int Revision)
        //{
        //    string imageDataURL = "";

        //    try
        //    {
        //        var Content = (from p in db.Drawings
        //                       where p.DrawingID == DrawingID
        //                       select p).ToList();

        //        if (Content != null && Content.Count > 0)
        //        {
        //            string lCustomerName = getCustomerName(Content[0].CustomerCode);
        //            string lProjectTitle = getProjectName(Content[0].CustomerCode, Content[0].ProjectCode, User.Identity.GetUserName());
        //            lCustomerName = removeSpecialChar(lCustomerName);
        //            lProjectTitle = removeSpecialChar(lProjectTitle);

        //            string lFileName = Content[0].FileName;

        //            if (lFileName.LastIndexOf(".") >= 0)
        //            {
        //                lFileName = lFileName.Substring(0, lFileName.LastIndexOf(".")) + "-R" + Revision.ToString() + "" + lFileName.Substring(lFileName.LastIndexOf("."));
        //            }
        //            else
        //            {
        //                lFileName = lFileName + "-R" + Revision.ToString();
        //            }

        //            //string lServerRelative = gSiteURL + "/Shared Documents/DrawingsLib/" + lCustomerName + "/" + lProjectTitle + "/" + Content[0].FileName;
        //            string lServerRelative = "/sites/DigiOSDocs/Shared Documents/DrawingsLib/" + lCustomerName + "/" + lProjectTitle + "/" + lFileName;

        //            ClientContext lClient = O365LogOn();
        //            Microsoft.SharePoint.Client.List docLib = lClient.Web.Lists.GetByTitle("Documents");
        //            lClient.Load(docLib);
        //            lClient.ExecuteQuery();

        //            var file = lClient.Web.GetFileByServerRelativeUrl(lServerRelative);
        //            lClient.Load(file);
        //            lClient.ExecuteQuery();
        //            ClientResult<Stream> streamResult = file.OpenBinaryStream();
        //            lClient.ExecuteQuery();

        //            var lMemoryStream = new MemoryStream();
        //            streamResult.Value.CopyTo(lMemoryStream);
        //            var lFileBinary = lMemoryStream.ToArray();

        //            var result = Convert.ToBase64String(lFileBinary);

        //            imageDataURL = string.Format("data:application/pdf;base64,{0}", result);

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError(string.Empty, "Server Error: " + ex.Message);
        //    }

        //    var lReturn = Json(imageDataURL, JsonRequestBehavior.AllowGet);
        //    lReturn.MaxJsonLength = 800000000;
        //    return lReturn;

        //}

        //[HttpPost]
        //[ValidateAntiForgeryHeader]
        //public ActionResult AssigDrawing(int DrawingID, int Revision, List<string> WBS1, List<string> WBS2, List<string> WBS3,
        //    List<string> ProductType, List<string> StructureElement)
        //{
        //    string lSQL = "";
        //    var lCmd = new SqlCommand();
        //    var lNDSCon = new SqlConnection();
        //    SqlDataReader lRst;
        //    bool lReturn = true;
        //    string lErrorMsg = "";

        //    if (WBS3 == null)
        //    {
        //        WBS3 = new List<string>();
        //    }
        //    if (WBS3.Count == 0)
        //    {
        //        WBS3.Add("");
        //    }

        //    if (DrawingID > 0 && Revision >= 0 && ProductType != null &&
        //        WBS1 != null && WBS2 != null && StructureElement != null &&
        //        WBS1.Count > 0 && WBS2.Count > 0 && StructureElement.Count > 0 && ProductType.Count > 0)
        //    {
        //        try
        //        {
        //            var lProcessObj = new ProcessController();
        //            lProcessObj.OpenNDSConnection(ref lNDSCon);

        //            for (int i = 0; i < WBS1.Count; i++)
        //            {
        //                for (int j = 0; j < WBS2.Count; j++)
        //                {
        //                    for (int k = 0; k < WBS3.Count; k++)
        //                    {
        //                        for (int m = 0; m < StructureElement.Count; m++)
        //                        {
        //                            for (int n = 0; n < ProductType.Count; n++)
        //                            {
        //                                int lFound = 0;
        //                                lSQL = "SELECT DrawingID " +
        //                                "FROM dbo.OESDrawingsWBS " +
        //                                "WHERE DrawingID = " + DrawingID + " " +
        //                                //"AND Revision = " + Revision + " " +
        //                                "AND ProductType = '" + ProductType[n] + "' " +
        //                                "AND StructureElement = '" + StructureElement[m] + "' " +
        //                                "AND WBS1 = '" + WBS1[i] + "' " +
        //                                "AND WBS2 = '" + WBS2[j] + "' " +
        //                                "AND WBS3 = '" + WBS3[k] + "' ";

        //                                lCmd.CommandText = lSQL;
        //                                lCmd.Connection = lNDSCon;
        //                                lCmd.CommandTimeout = 300;
        //                                lRst = lCmd.ExecuteReader();
        //                                if (lRst.HasRows)
        //                                {
        //                                    lFound = 1;
        //                                }
        //                                lRst.Close();
        //                                if (lFound == 0)
        //                                {
        //                                    lSQL = "INSERT INTO dbo.OESDrawingsWBS " +
        //                                        "(DrawingID " +
        //                                        ", Revision " +
        //                                        ", ProductType " +
        //                                        ", StructureElement " +
        //                                        ", WBS1 " +
        //                                        ", WBS2 " +
        //                                        ", WBS3 " +
        //                                        ", UpdatedDate " +
        //                                        ", UpdatedBy) " +
        //                                        "VALUES " +
        //                                        "(" + DrawingID.ToString() + " " +
        //                                        ", " + Revision + " " +
        //                                        ",'" + ProductType[n] + "' " +
        //                                        ",'" + StructureElement[m] + "' " +
        //                                        ",'" + WBS1[i] + "' " +
        //                                        ",'" + WBS2[j] + "' " +
        //                                        ",'" + WBS3[k] + "' " +
        //                                        ",getDate() " +
        //                                        ",'" + User.Identity.Name + "') ";

        //                                    lCmd.CommandText = lSQL;
        //                                    lCmd.Connection = lNDSCon;
        //                                    lCmd.CommandTimeout = 300;
        //                                    lCmd.ExecuteNonQuery();
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            lProcessObj.CloseNDSConnection(ref lNDSCon);
        //            lProcessObj = null;

        //        }
        //        catch (Exception ex)
        //        {
        //            lReturn = false;
        //            lErrorMsg = ex.Message;
        //        }
        //    }

        //    return Json(new { success = lReturn, message = lErrorMsg }, JsonRequestBehavior.AllowGet);
        //}

        //[HttpPost]
        ////[ValidateAntiForgeryHeader]
        //public ActionResult UnassigDrawing(List<int> DrawingID, List<int> Revision, List<string> WBS1, List<string> WBS2, List<string> WBS3,
        //    List<string> ProductType, List<string> StructureElement)
        //{
        //    string lSQL = "";
        //    var lCmd = new SqlCommand();
        //    var lNDSCon = new SqlConnection();
        //    SqlDataReader lRst;
        //    bool lReturn = true;
        //    string lErrorMsg = "";

        //    if (WBS3 == null)
        //    {
        //        WBS3 = new List<string>();
        //    }
        //    if (WBS3.Count == 0)
        //    {
        //        WBS3.Add("");
        //    }

        //    if (DrawingID != null && Revision != null && ProductType != null &&
        //        WBS1 != null && WBS2 != null && StructureElement != null &&
        //        DrawingID.Count > 0 && Revision.Count > 0 && ProductType.Count > 0 &&
        //        WBS1.Count > 0 && WBS2.Count > 0 && StructureElement.Count > 0)
        //    {
        //        try
        //        {
        //            var lProcessObj = new ProcessController();
        //            lProcessObj.OpenNDSConnection(ref lNDSCon);

        //            for (int i = 0; i < DrawingID.Count; i++)
        //            {
        //                int lFound = 0;
        //                lSQL = "SELECT DrawingID " +
        //                    "FROM dbo.OESDrawingsWBS " +
        //                    "WHERE DrawingID = " + DrawingID[i] + " " +
        //                    "AND Revision = " + Revision[i] + " " +
        //                    "AND ProductType = '" + ProductType[i] + "' " +
        //                    "AND StructureElement = '" + StructureElement[i] + "' " +
        //                    "AND WBS1 = '" + WBS1[i] + "' " +
        //                    "AND WBS2 = '" + WBS2[i] + "' " +
        //                    "AND WBS3 = '" + WBS3[i] + "' ";

        //                lCmd.CommandText = lSQL;
        //                lCmd.Connection = lNDSCon;
        //                lCmd.CommandTimeout = 300;
        //                lRst = lCmd.ExecuteReader();
        //                if (lRst.HasRows)
        //                {
        //                    lFound = 1;
        //                }
        //                lRst.Close();
        //                if (lFound == 1)
        //                {
        //                    lSQL = "DELETE " +
        //                        "FROM dbo.OESDrawingsWBS " +
        //                        "WHERE DrawingID = " + DrawingID[i] + " " +
        //                        "AND ProductType = '" + ProductType[i] + "' " +
        //                        "AND StructureElement = '" + StructureElement[i] + "' " +
        //                        "AND WBS1 = '" + WBS1[i] + "' " +
        //                        "AND WBS2 = '" + WBS2[i] + "' " +
        //                        "AND WBS3 = '" + WBS3[i] + "' ";

        //                    lCmd.CommandText = lSQL;
        //                    lCmd.Connection = lNDSCon;
        //                    lCmd.CommandTimeout = 300;
        //                    lCmd.ExecuteNonQuery();
        //                }
        //            }
        //            lProcessObj.CloseNDSConnection(ref lNDSCon);
        //            lProcessObj = null;
        //        }
        //        catch (Exception ex)
        //        {
        //            lReturn = false;
        //            lErrorMsg = ex.Message;
        //        }
        //    }

        //    return Json(new { success = lReturn, message = lErrorMsg }, JsonRequestBehavior.AllowGet);
        //}

        //[HttpPost]
        ////[ValidateAntiForgeryHeader]
        //public ActionResult deleteDrawingOrder(int OrderNuber, String StructureElement, string ProductType, string ScheduledProd, string CustomerCode, string ProjectCode, int DrawingID, int Revision)
        //{
        //    string lSQL = "";
        //    var lCmd = new SqlCommand();
        //    var lNDSCon = new SqlConnection();
        //    SqlDataReader lRst;
        //    int lReturn = 0;
        //    string lErrorMsg = "";

        //    int lOtherPreRev = 0;
        //    int lOtherCurrRev = 0;

        //    int lFound = 0;
        //    var lRevision = 0;
        //    lRevision = Revision;

        //    if (DrawingID > 0)
        //    {
        //        try
        //        {
        //            var lProcessObj = new ProcessController();
        //            lProcessObj.OpenNDSConnection(ref lNDSCon);

        //            var lRevision1 = lRevision - 1;

        //            if (lRevision1 >= 0)
        //            {
        //                lSQL = "SELECT Revision " +
        //                "FROM dbo.OESDrawingsOrder " +
        //                "WHERE DrawingID = " + DrawingID.ToString() + "  " +
        //                "AND Revision = " + lRevision1.ToString() + " " +
        //                "AND (OrderNumber <> " + OrderNuber.ToString() + " " +
        //                "OR StructureElement <> '" + StructureElement + "' " +
        //                "OR ProductType <> '" + ProductType + "' " +
        //                "OR ScheduledProd <> '" + ScheduledProd + "') ";

        //                lCmd.CommandText = lSQL;
        //                lCmd.Connection = lNDSCon;
        //                lCmd.CommandTimeout = 300;
        //                lRst = lCmd.ExecuteReader();
        //                if (lRst.HasRows)
        //                {
        //                    if (lRst.Read())
        //                    {
        //                        lOtherPreRev = 1;
        //                    }
        //                }
        //                lRst.Close();
        //            }

        //            lSQL = "SELECT Revision " +
        //            "FROM dbo.OESDrawingsOrder " +
        //            "WHERE DrawingID = " + DrawingID.ToString() + "  " +
        //            "AND Revision = " + Revision.ToString() + " " +
        //            "AND OrderNumber = " + OrderNuber.ToString() + " " +
        //            "AND StructureElement = '" + StructureElement + "' " +
        //            "AND ProductType = '" + ProductType + "' " +
        //            "AND ScheduledProd = '" + ScheduledProd + "' ";

        //            lCmd.CommandText = lSQL;
        //            lCmd.Connection = lNDSCon;
        //            lCmd.CommandTimeout = 300;
        //            lRst = lCmd.ExecuteReader();
        //            if (lRst.HasRows)
        //            {
        //                if (lRst.Read())
        //                {
        //                    lFound = 1;
        //                }
        //            }
        //            lRst.Close();

        //            if (lFound == 1)
        //            {
        //                if (lOtherPreRev == 0 && lRevision1 >= 0)
        //                {
        //                    lSQL = "UPDATE " +
        //                    "dbo.OESDrawingsOrder " +
        //                    "SET Revision = " + lRevision1.ToString() + " " +
        //                    "WHERE DrawingID = " + DrawingID.ToString() + "  " +
        //                    "AND Revision = " + Revision.ToString() + " " +
        //                    "AND OrderNumber = " + OrderNuber.ToString() + " " +
        //                    "AND StructureElement = '" + StructureElement + "' " +
        //                    "AND ProductType = '" + ProductType + "' " +
        //                    "AND ScheduledProd = '" + ScheduledProd + "' ";
        //                }
        //                else
        //                {
        //                    lSQL = "DELETE " +
        //                    "FROM dbo.OESDrawingsOrder " +
        //                    "WHERE DrawingID = " + DrawingID.ToString() + "  " +
        //                    "AND Revision = " + Revision.ToString() + " " +
        //                    "AND OrderNumber = " + OrderNuber.ToString() + " " +
        //                    "AND StructureElement = '" + StructureElement + "' " +
        //                    "AND ProductType = '" + ProductType + "' " +
        //                    "AND ScheduledProd = '" + ScheduledProd + "' ";
        //                }

        //                lCmd.CommandText = lSQL;
        //                lCmd.Connection = lNDSCon;
        //                lCmd.CommandTimeout = 300;
        //                lCmd.ExecuteNonQuery();

        //                lSQL = "SELECT Revision " +
        //                "FROM dbo.OESDrawingsOrder " +
        //                "WHERE DrawingID = " + DrawingID.ToString() + "  " +
        //                "AND Revision = " + Revision.ToString() + " ";

        //                lCmd.CommandText = lSQL;
        //                lCmd.Connection = lNDSCon;
        //                lCmd.CommandTimeout = 300;
        //                lRst = lCmd.ExecuteReader();
        //                if (lRst.HasRows)
        //                {
        //                    if (lRst.Read())
        //                    {
        //                        lOtherCurrRev = 1;
        //                    }
        //                }
        //                lRst.Close();

        //            }

        //            lProcessObj.CloseNDSConnection(ref lNDSCon);
        //            lProcessObj = null;
        //        }
        //        catch (Exception ex)
        //        {
        //            lReturn = -1;
        //            lErrorMsg = ex.Message;
        //        }
        //    }

        //    if (lFound == 1)
        //    {
        //        if (lOtherCurrRev == 1)
        //        {
        //            lReturn = 1;
        //        }
        //        else
        //        {
        //            if (lOtherPreRev == 1)
        //            {
        //                lReturn = 2;
        //            }
        //            else
        //            {
        //                lReturn = 3;
        //            }
        //        }
        //    }
        //    //return code:-1 - error 
        //    //0 - Not found 
        //    //1 - Have other Order using the drawing, no delete
        //    //2 - Delete the order, but no previous revision
        //    //3 - Delete the order and havee previous revision if current revision > 0;

        //    return Json(new { success = lReturn, message = lErrorMsg }, JsonRequestBehavior.AllowGet);
        //}

        //[HttpPost]
        ////[ValidateAntiForgeryHeader]
        //public ActionResult deleteDrawing(string CustomerCode, string ProjectCode, int DrawingID)
        //{
        //    string lSQL = "";
        //    var lCmd = new SqlCommand();
        //    var lNDSCon = new SqlConnection();
        //    SqlDataReader lRst;
        //    bool lReturn = true;
        //    string lErrorMsg = "";


        //    if (DrawingID > 0)
        //    {
        //        try
        //        {
        //            var lProcessObj = new ProcessController();
        //            lProcessObj.OpenNDSConnection(ref lNDSCon);

        //            int lFound = 0;
        //            var lCustomerCode = "";
        //            var lProjectCode = "";
        //            var lFileName = "";
        //            var lRevision = 0;

        //            lSQL = "SELECT CustomerCode, ProjectCode, FileName, Revision " +
        //            "FROM dbo.OESDrawings " +
        //            "WHERE DrawingID = " + DrawingID + " ";

        //            lCmd.CommandText = lSQL;
        //            lCmd.Connection = lNDSCon;
        //            lCmd.CommandTimeout = 300;
        //            lRst = lCmd.ExecuteReader();
        //            if (lRst.HasRows)
        //            {
        //                if (lRst.Read())
        //                {
        //                    lCustomerCode = lRst.GetString(0);
        //                    lProjectCode = lRst.GetString(1);
        //                    lFileName = lRst.GetString(2);
        //                    lRevision = lRst.GetInt32(3);
        //                    lFound = 1;
        //                }
        //            }
        //            lRst.Close();

        //            if (lFileName.LastIndexOf(".") >= 0)
        //            {
        //                lFileName = lFileName.Substring(0, lFileName.LastIndexOf(".")) + "-R" + lRevision.ToString() + "" + lFileName.Substring(lFileName.LastIndexOf("."));
        //            }
        //            else
        //            {
        //                lFileName = lFileName + "-R" + lRevision.ToString();
        //            }

        //            if (lFound == 1 && lCustomerCode == CustomerCode && lProjectCode == ProjectCode)
        //            {
        //                ClientContext lClient = O365LogOn();
        //                Microsoft.SharePoint.Client.List docLib = lClient.Web.Lists.GetByTitle("Documents");
        //                lClient.Load(docLib);
        //                lClient.ExecuteQuery();

        //                string lCustomerName = getCustomerName(lCustomerCode);
        //                string lProjectTitle = getProjectName(lCustomerCode, lProjectCode, User.Identity.GetUserName());
        //                lCustomerName = removeSpecialChar(lCustomerName);
        //                lProjectTitle = removeSpecialChar(lProjectTitle);

        //                var lFileUrl = "/sites/DigiOSDocs/Shared Documents/DrawingsLib/" + lCustomerName + "/" + lProjectTitle + "/" + lFileName;
        //                if (FileExists(lClient, lFileUrl) == true)
        //                {

        //                    var lFileToDelete = lClient.Web.GetFileByServerRelativeUrl(lFileUrl);

        //                    lFileToDelete.DeleteObject();

        //                    lClient.ExecuteQuery();
        //                }

        //                //delete wexbim file if any
        //                var lExt = lFileName.Substring(lFileName.LastIndexOf(".") + 1);
        //                if (lExt.ToLower() == "ifc" || lExt.ToLower() == "ifczip" || lExt.ToLower() == "ifcxml")
        //                {
        //                    var lFileUrlIFC = "/sites/DigiOSDocs/Shared Documents/DrawingsLib/" + lCustomerName + "/" + lProjectTitle + "/" + lFileName.Substring(0, lFileName.LastIndexOf(".")) + ".wexbim";
        //                    if (FileExists(lClient, lFileUrlIFC) == true)
        //                    {

        //                        var lFileToDelete = lClient.Web.GetFileByServerRelativeUrl(lFileUrlIFC);

        //                        lFileToDelete.DeleteObject();

        //                        lClient.ExecuteQuery();
        //                    }

        //                }

        //                lSQL = "DELETE " +
        //                "FROM dbo.OESDrawingsOrder " +
        //                "WHERE DrawingID = " + DrawingID.ToString() + " " +
        //                "AND Revision = " + lRevision.ToString() + " ";

        //                lCmd.CommandText = lSQL;
        //                lCmd.Connection = lNDSCon;
        //                lCmd.CommandTimeout = 300;
        //                lCmd.ExecuteNonQuery();


        //                if (lRevision == 0)
        //                {
        //                    lSQL = "DELETE " +
        //                    "FROM dbo.OESDrawingsWBS " +
        //                    "WHERE DrawingID = " + DrawingID.ToString() + " " +
        //                    "AND Revision = " + lRevision.ToString() + " ";

        //                    lCmd.CommandText = lSQL;
        //                    lCmd.Connection = lNDSCon;
        //                    lCmd.CommandTimeout = 300;
        //                    lCmd.ExecuteNonQuery();

        //                    lSQL = "DELETE " +
        //                    "FROM dbo.OESDrawings " +
        //                    "WHERE DrawingID = " + DrawingID + " ";

        //                    lCmd.CommandText = lSQL;
        //                    lCmd.Connection = lNDSCon;
        //                    lCmd.CommandTimeout = 300;
        //                    lCmd.ExecuteNonQuery();
        //                }
        //                else
        //                {
        //                    lRevision = lRevision - 1;

        //                    lSQL = "UPDATE dbo.OESDrawings " +
        //                    "SET Revision = " + lRevision.ToString() + " " +
        //                    "WHERE DrawingID = " + DrawingID + " ";

        //                    lCmd.CommandText = lSQL;
        //                    lCmd.Connection = lNDSCon;
        //                    lCmd.CommandTimeout = 300;
        //                    lCmd.ExecuteNonQuery();

        //                    lSQL = "UPDATE dbo.OESDrawingsWBS " +
        //                    "SET Revision = " + lRevision.ToString() + " " +
        //                    "WHERE DrawingID = " + DrawingID + " " +
        //                    "AND Revision = " + (lRevision + 1).ToString() + " ";

        //                    lCmd.CommandText = lSQL;
        //                    lCmd.Connection = lNDSCon;
        //                    lCmd.CommandTimeout = 300;
        //                    lCmd.ExecuteNonQuery();

        //                }
        //            }

        //            lProcessObj.CloseNDSConnection(ref lNDSCon);
        //            lProcessObj = null;
        //        }
        //        catch (Exception ex)
        //        {
        //            lReturn = false;
        //            lErrorMsg = ex.Message;
        //        }
        //    }

        //    return Json(new { success = lReturn, message = lErrorMsg }, JsonRequestBehavior.AllowGet);
        //}

        

        //[HttpPost]
        ////[ValidateAntiForgeryHeader]
        //public ActionResult getFilesList(string CustomerCode, string ProjectCode)
        //{
        //    ClientContext lClient = O365LogOn();

        //    string lCustomerName = "";
        //    string lProjectTitle = "";

        //    var lCustomer = db.Customer.Find(CustomerCode);
        //    if (lCustomer != null)
        //    {
        //        lCustomerName = lCustomer.CustomerName;
        //    }
        //    if (lCustomerName == null)
        //    {
        //        lCustomerName = "";
        //    }
        //    lCustomerName = lCustomerName.Trim();
        //    //Sharepoint maximum folder/file nme length:260 
        //    if (lCustomerName.Length > 200)
        //    {
        //        lCustomerName = lCustomerName.Substring(0, 200).Trim();
        //    }
        //    lCustomerName = lCustomerName + "(" + CustomerCode + ")";

        //    var lProject = db.Project.Find(CustomerCode, ProjectCode);
        //    if (lProject != null)
        //    {
        //        lProjectTitle = lProject.ProjectTitle;
        //    }
        //    if (lProjectTitle == null)
        //    {
        //        lProjectTitle = "";
        //    }
        //    lProjectTitle = lProjectTitle.Trim();
        //    //Sharepoint maximum folder/file nme length:260 
        //    if (lProjectTitle.Length > 200)
        //    {
        //        lProjectTitle = lProjectTitle.Substring(0, 200).Trim();
        //    }
        //    lProjectTitle = lProjectTitle + "(" + ProjectCode + ")";

        //    Web lWebsite = lClient.Web;

        //    string folderUrl = "/sites/learning/Shared Documents";
        //    // checks folder from site level 
        //    //bool folderExists = lClient.Web.Lists.DoesFolderExists(folderUrl); 
        //    // Output if (folderExists

        //    //lClient.Load(lWebsite.Lists,
        //    //             lists => lists.eq.Include(list => list.Title, list => list.Id, list => list.ItemCount));

        //    //ListCollection lCollList = lWebsite.Lists;

        //    Microsoft.SharePoint.Client.List targetList = lClient.Web.Lists.GetByTitle("Documents");

        //    // This method only gets the folders which are on top level of the list/library
        //    FolderCollection oFolderCollection = targetList.RootFolder.Folders;

        //    // Load folder collection
        //    lClient.Load(oFolderCollection);

        //    //Microsoft.SharePoint.Client.List lDocList = lWebsite.Lists.GetByTitle("Documents");

        //    //    CamlQuery camlQuery = new CamlQuery();
        //    //    camlQuery.ViewXml = "<View><Query><Where>" +
        //    //    "<Contains><FieldRef Name=\"Title\" /> < Value Type = \"Text\" > discussion session </ Value > " +
        //    //    "</Contains></Where></Query><RowLimit>50</RowLimit></View>";
        //    //ListItemCollection lCompanyList = lDocList.GetItems(camlQuery);

        //    //Microsoft.SharePoint.Client.List lOrderList = lCollList.GetByTitle("OrderDocuments");
        //    //Microsoft.SharePoint.Client.List lCompanyList = lOrderList.GetByTitle(CustomerCode);
        //    //Microsoft.SharePoint.Client.List lProjectList = lCompanyList.GetByTitle(ProjectCode);

        //    //CamlQuery camlQuery = new CamlQuery();
        //    //camlQuery.ViewXml = "<View><Query><Where>" +
        //    //"<Contains><FieldRef Name=\"Conference\" /> < Value Type = \"Note\" > discussion session </ Value > " + 
        //    //"</Contains></Where></Query><RowLimit>50</RowLimit></View>";

        //    //ListItemCollection lCompanyList = lOrderList.GetItems(camlQuery);

        //    //clientContext.Load(collListItem,
        //    //    items => items.IncludeWithDefaultProperties(
        //    //    item => item.DisplayName));

        //    lClient.ExecuteQuery();


        //    //Microsoft.SharePoint.Client.List lCompanyList = lClient.Web.Lists.GetByTitle(CustomerCode);


        //    return Json(lProject, JsonRequestBehavior.AllowGet);
        //}

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

    
        [HttpPost]
        [Route("/getAssignStrEle/{ProjectCode}")]
        public ActionResult getAssignStrEle(string ProjectCode)
        {
            var lDa = new SqlDataAdapter();
            var lCmd = new SqlCommand();
            var lDs = new DataSet();
            var cnNDS = new SqlConnection();

            var lSQL = "";

            var lStruEle = new List<string>();

            var lProcess = new ProcessController();
            lProcess.OpenNDSConnection(ref cnNDS);
            if (cnNDS.State == ConnectionState.Open)
            {
                lSQL = "SELECT vchStructureElementType " +
                    "FROM dbo.StructureElementMaster " +
                    "WHERE tntStatusId = 1 " +
                    "AND vchStructureElementType <> 'SCAB1' " +
                    "ORDER BY vchStructureElementType ";

                lCmd.CommandType = CommandType.Text;
                lCmd.CommandText = lSQL;
                lCmd.Connection = cnNDS;
                lDa.SelectCommand = lCmd;
                lDs = new DataSet();
                lDa.Fill(lDs);
                if (lDs.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < lDs.Tables[0].Rows.Count; i++)
                    {
                        lStruEle.Add(lDs.Tables[0].Rows[i].ItemArray[0].ToString().Trim());
                    }
                }
                lProcess.CloseNDSConnection(ref cnNDS);
            }

            lDa = null;
            lCmd = null;
            lDs = null;
            cnNDS = null;

           // return Json(lStruEle, JsonRequestBehavior.AllowGet);
            return Ok(lStruEle);

        }

        //[HttpPost]
        ////[ValidateAntiForgeryHeader]
        //[ValidateAntiForgeryToken]
        //public ActionResult downloadDrawings(string ddCustomerCode, string ddProjectCode, string ddFileName, int ddRevision)
        //{
        //    try
        //    {
        //        string lCustomerName = getCustomerName(ddCustomerCode);
        //        string lProjectTitle = getProjectName(ddCustomerCode, ddProjectCode, User.Identity.GetUserName());
        //        lCustomerName = removeSpecialChar(lCustomerName);
        //        lProjectTitle = removeSpecialChar(lProjectTitle);

        //        string lFileName = Server.HtmlDecode(ddFileName);

        //        //lFileName = removeSpecialChar(lFileName);

        //        if (lFileName.LastIndexOf(".") >= 0)
        //        {
        //            lFileName = lFileName.Substring(0, lFileName.LastIndexOf(".")) + "-R" + ddRevision.ToString() + "" + lFileName.Substring(lFileName.LastIndexOf("."));
        //        }
        //        else
        //        {
        //            lFileName = lFileName + "-R" + ddRevision.ToString();
        //        }

        //        string lServerRelative = "/sites/DigiOSDocs/Shared Documents/DrawingsLib/" + lCustomerName + "/" + lProjectTitle + "/" + lFileName;

        //        ClientContext lClient = O365LogOn();
        //        Microsoft.SharePoint.Client.List docLib = lClient.Web.Lists.GetByTitle("Documents");
        //        lClient.Load(docLib);
        //        lClient.ExecuteQuery();

        //        var file = lClient.Web.GetFileByServerRelativeUrl(lServerRelative);
        //        lClient.Load(file);
        //        lClient.ExecuteQuery();
        //        ClientResult<Stream> streamResult = file.OpenBinaryStream();
        //        lClient.ExecuteQuery();

        //        var lMemoryStream = new MemoryStream();
        //        streamResult.Value.CopyTo(lMemoryStream);
        //        var lFileBinary = lMemoryStream.ToArray();

        //        System.Net.Mime.ContentDisposition cd = new System.Net.Mime.ContentDisposition
        //        {
        //            FileName = lFileName,
        //            Inline = false
        //        };

        //        string contentType = MimeMapping.GetMimeMapping(lFileName);
        //        Response.AppendHeader("Access-Control-Allow-Origin", "*");
        //        Response.AppendHeader("Access-Control-Allow-Methods", "POST, GET, OPTIONS");
        //        Response.AppendHeader("Access-Control-Allow-Headers", "X-PINGOTHER, Content-Type");
        //        return File(lFileBinary, contentType, lFileName);

        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError(string.Empty, "Server Error: " + ex.Message);
        //    }
        //    //var lReturn = Json(imageDataURL, JsonRequestBehavior.AllowGet);
        //    //lReturn.MaxJsonLength = 80000000;
        //    return null;
        //}

        //public byte[] getDrawing(string ddCustomerCode, string ddProjectCode, string ddFileName, int ddRevision, string ddUserID)
        //{
        //    byte[] lFileBinary = null;
        //    try
        //    {
        //        string lCustomerName = getCustomerName(ddCustomerCode);
        //        string lProjectTitle = getProjectName(ddCustomerCode, ddProjectCode, ddUserID);
        //        lCustomerName = removeSpecialChar(lCustomerName);
        //        lProjectTitle = removeSpecialChar(lProjectTitle);

        //        string lFileName = HttpUtility.HtmlDecode(ddFileName);

        //        if (lFileName.LastIndexOf(".") >= 0)
        //        {
        //            lFileName = lFileName.Substring(0, lFileName.LastIndexOf(".")) + "-R" + ddRevision.ToString() + "" + lFileName.Substring(lFileName.LastIndexOf("."));
        //        }
        //        else
        //        {
        //            lFileName = lFileName + "-R" + ddRevision.ToString();
        //        }

        //        string lServerRelative = "/sites/DigiOSDocs/Shared Documents/DrawingsLib/" + lCustomerName + "/" + lProjectTitle + "/" + lFileName;

        //        ClientContext lClient = O365LogOn();
        //        Microsoft.SharePoint.Client.List docLib = lClient.Web.Lists.GetByTitle("Documents");
        //        lClient.Load(docLib);
        //        lClient.ExecuteQueryAsync();

        //        var file = lClient.Web.GetFileByServerRelativeUrl(lServerRelative);
        //        lClient.Load(file);
        //        lClient.ExecuteQueryAsync();
        //        ClientResult<Stream> streamResult = file.OpenBinaryStream();
        //        lClient.ExecuteQueryAsync();

        //        var lMemoryStream = new MemoryStream();
        //        streamResult.Value.CopyTo(lMemoryStream);
        //        lFileBinary = lMemoryStream.ToArray();
        //    }
        //    catch (Exception ex)
        //    {
        //        lFileBinary = null;
        //    }
        //    return lFileBinary;
        //}


        [HttpGet]
        [Route("/PrintDrawings/{CustomerCode}/{ProjectCode}/{FileName}/{Revision}")]
        public ActionResult PrintDrawings(string CustomerCode, string ProjectCode, string FileName, int Revision)
        {
            try
            {
                string lCustomerName = getCustomerName(CustomerCode);
                string lProjectTitle = getProjectName(CustomerCode, ProjectCode, User.Identity.GetUserName());
                lCustomerName = removeSpecialChar(lCustomerName);
                lProjectTitle = removeSpecialChar(lProjectTitle);


                //string lFileName = Server.HtmlDecode(FileName);//commented by ajit
                string lFileName=System.Web.HttpUtility.HtmlDecode(FileName);

                if (lFileName.LastIndexOf(".") >= 0)
                {
                    lFileName = lFileName.Substring(0, lFileName.LastIndexOf(".")) + "-R" + Revision.ToString() + "" + lFileName.Substring(lFileName.LastIndexOf("."));
                }
                else
                {
                    lFileName = lFileName + "-R" + Revision.ToString();
                }

                string lServerRelative = "/sites/DigiOSDocs/Shared Documents/DrawingsLib/" + lCustomerName + "/" + lProjectTitle + "/" + lFileName;

                ClientContext lClient = O365LogOn();
                Microsoft.SharePoint.Client.List docLib = lClient.Web.Lists.GetByTitle("Documents");
                lClient.Load(docLib);
                lClient.ExecuteQueryAsync();

                var file = lClient.Web.GetFileByServerRelativeUrl(lServerRelative);
                lClient.Load(file);
                lClient.ExecuteQueryAsync();
                ClientResult<Stream> streamResult = file.OpenBinaryStream();
                lClient.ExecuteQueryAsync();

                var lMemoryStream = new MemoryStream();
                streamResult.Value.CopyTo(lMemoryStream);
                var lFileBinary = lMemoryStream.ToArray();

                var lReturn = Ok(lFileBinary);
              //  lReturn.MaxJsonLength = 80000000;
                return lReturn;

                //return Json(lFileBinary, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Server Error: " + ex.Message);
            }
            //var lReturn = Json(imageDataURL, JsonRequestBehavior.AllowGet);
            //lReturn.MaxJsonLength = 80000000;
            return null;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        [HttpGet]
        [Route("/deleteDrawingOrder/{OrderNuber}/{StructureElement}/{ProductType}/{ScheduledProd}/{CustomerCode}/{ProjectCode}/{DrawingID}/{Revision}")]
        public ActionResult deleteDrawingOrder(int OrderNuber, String StructureElement, string ProductType, string ScheduledProd, string CustomerCode, string ProjectCode, int DrawingID, int Revision)
        {
            string lSQL = "";
            var lCmd = new SqlCommand();
            var lNDSCon = new SqlConnection();
            SqlDataReader lRst;
            int lReturn = 0;
            string lErrorMsg = "";

            int lOtherPreRev = 0;
            int lOtherCurrRev = 0;

            int lFound = 0;
            var lRevision = 0;
            lRevision = Revision;

            if (DrawingID > 0)
            {
                try
                {
                    var lProcessObj = new ProcessController();
                    lProcessObj.OpenNDSConnection(ref lNDSCon);

                    var lRevision1 = lRevision - 1;

                    if (lRevision1 >= 0)
                    {
                        lSQL = "SELECT Revision " +
                        "FROM dbo.OESDrawingsOrder " +
                        "WHERE DrawingID = " + DrawingID.ToString() + "  " +
                        "AND Revision = " + lRevision1.ToString() + " " +
                        "AND (OrderNumber <> " + OrderNuber.ToString() + " " +
                        "OR StructureElement <> '" + StructureElement + "' " +
                        "OR ProductType <> '" + ProductType + "' " +
                        "OR ScheduledProd <> '" + ScheduledProd + "') ";

                        lCmd.CommandText = lSQL;
                        lCmd.Connection = lNDSCon;
                        lCmd.CommandTimeout = 300;
                        lRst = lCmd.ExecuteReader();
                        if (lRst.HasRows)
                        {
                            if (lRst.Read())
                            {
                                lOtherPreRev = 1;
                            }
                        }
                        lRst.Close();
                    }

                    lSQL = "SELECT Revision " +
                    "FROM dbo.OESDrawingsOrder " +
                    "WHERE DrawingID = " + DrawingID.ToString() + "  " +
                    "AND Revision = " + Revision.ToString() + " " +
                    "AND OrderNumber = " + OrderNuber.ToString() + " " +
                    "AND StructureElement = '" + StructureElement + "' " +
                    "AND ProductType = '" + ProductType + "' " +
                    "AND ScheduledProd = '" + ScheduledProd + "' ";

                    lCmd.CommandText = lSQL;
                    lCmd.Connection = lNDSCon;
                    lCmd.CommandTimeout = 300;
                    lRst = lCmd.ExecuteReader();
                    if (lRst.HasRows)
                    {
                        if (lRst.Read())
                        {
                            lFound = 1;
                        }
                    }
                    lRst.Close();

                    if (lFound == 1)
                    {
                        if (lOtherPreRev == 0 && lRevision1 >= 0)
                        {
                            lSQL = "UPDATE " +
                            "dbo.OESDrawingsOrder " +
                            "SET Revision = " + lRevision1.ToString() + " " +
                            "WHERE DrawingID = " + DrawingID.ToString() + "  " +
                            "AND Revision = " + Revision.ToString() + " " +
                            "AND OrderNumber = " + OrderNuber.ToString() + " " +
                            "AND StructureElement = '" + StructureElement + "' " +
                            "AND ProductType = '" + ProductType + "' " +
                            "AND ScheduledProd = '" + ScheduledProd + "' ";
                        }
                        else
                        {
                            lSQL = "DELETE " +
                            "FROM dbo.OESDrawingsOrder " +
                            "WHERE DrawingID = " + DrawingID.ToString() + "  " +
                            "AND Revision = " + Revision.ToString() + " " +
                            "AND OrderNumber = " + OrderNuber.ToString() + " " +
                            "AND StructureElement = '" + StructureElement + "' " +
                            "AND ProductType = '" + ProductType + "' " +
                            "AND ScheduledProd = '" + ScheduledProd + "' ";
                        }

                        lCmd.CommandText = lSQL;
                        lCmd.Connection = lNDSCon;
                        lCmd.CommandTimeout = 300;
                        lCmd.ExecuteNonQuery();

                        lSQL = "SELECT Revision " +
                        "FROM dbo.OESDrawingsOrder " +
                        "WHERE DrawingID = " + DrawingID.ToString() + "  " +
                        "AND Revision = " + Revision.ToString() + " ";

                        lCmd.CommandText = lSQL;
                        lCmd.Connection = lNDSCon;
                        lCmd.CommandTimeout = 300;
                        lRst = lCmd.ExecuteReader();
                        if (lRst.HasRows)
                        {
                            if (lRst.Read())
                            {
                                lOtherCurrRev = 1;
                            }
                        }
                        lRst.Close();

                    }

                    lProcessObj.CloseNDSConnection(ref lNDSCon);
                    lProcessObj = null;
                }
                catch (Exception ex)
                {
                    lReturn = -1;
                    lErrorMsg = ex.Message;
                }
            }

            if (lFound == 1)
            {
                if (lOtherCurrRev == 1)
                {
                    lReturn = 1;
                }
                else
                {
                    if (lOtherPreRev == 1)
                    {
                        lReturn = 2;
                    }
                    else
                    {
                        lReturn = 3;
                    }
                }
            }
            //return code:-1 - error 
            //0 - Not found 
            //1 - Have other Order using the drawing, no delete
            //2 - Delete the order, but no previous revision
            //3 - Delete the order and havee previous revision if current revision > 0;

            return Ok(new { success = lReturn, message = lErrorMsg });
        }

        [HttpGet]
        [Route("/deleteDrawing/{CustomerCode}/{ProjectCode}/{DrawingID}")]
        public ActionResult deleteDrawing(string CustomerCode, string ProjectCode, int DrawingID)
        {
            string lSQL = "";
            var lCmd = new SqlCommand();
            var lNDSCon = new SqlConnection();
            SqlDataReader lRst;
            bool lReturn = true;
            string lErrorMsg = "";


            if (DrawingID > 0)
            {
                try
                {
                    var lProcessObj = new ProcessController();
                    lProcessObj.OpenNDSConnection(ref lNDSCon);

                    int lFound = 0;
                    var lCustomerCode = "";
                    var lProjectCode = "";
                    var lFileName = "";
                    var lRevision = 0;

                    lSQL = "SELECT CustomerCode, ProjectCode, FileName, Revision " +
                    "FROM dbo.OESDrawings " +
                    "WHERE DrawingID = " + DrawingID + " ";

                    lCmd.CommandText = lSQL;
                    lCmd.Connection = lNDSCon;
                    lCmd.CommandTimeout = 300;
                    lRst = lCmd.ExecuteReader();
                    if (lRst.HasRows)
                    {
                        if (lRst.Read())
                        {
                            lCustomerCode = lRst.GetString(0);
                            lProjectCode = lRst.GetString(1);
                            lFileName = lRst.GetString(2);
                            lRevision = lRst.GetInt32(3);
                            lFound = 1;
                        }
                    }
                    lRst.Close();

                    if (lFileName.LastIndexOf(".") >= 0)
                    {
                        lFileName = lFileName.Substring(0, lFileName.LastIndexOf(".")) + "-R" + lRevision.ToString() + "" + lFileName.Substring(lFileName.LastIndexOf("."));
                    }
                    else
                    {
                        lFileName = lFileName + "-R" + lRevision.ToString();
                    }

                    if (lFound == 1 && lCustomerCode == CustomerCode && lProjectCode == ProjectCode)
                    {
                        ClientContext lClient = O365LogOn();
                        Microsoft.SharePoint.Client.List docLib = lClient.Web.Lists.GetByTitle("Documents");
                        lClient.Load(docLib);
                        lClient.ExecuteQueryAsync();

                        string lCustomerName = getCustomerName(lCustomerCode);
                        string lProjectTitle = getProjectName(lCustomerCode, lProjectCode, User.Identity.GetUserName());
                        lCustomerName = removeSpecialChar(lCustomerName);
                        lProjectTitle = removeSpecialChar(lProjectTitle);

                        var lFileUrl = "/sites/DigiOSDocs/Shared Documents/DrawingsLib/" + lCustomerName + "/" + lProjectTitle + "/" + lFileName;
                        if (FileExists(lClient, lFileUrl) == true)
                        {

                            var lFileToDelete = lClient.Web.GetFileByServerRelativeUrl(lFileUrl);

                            lFileToDelete.DeleteObject();

                            lClient.ExecuteQueryAsync();
                        }

                        //delete wexbim file if any
                        var lExt = lFileName.Substring(lFileName.LastIndexOf(".") + 1);
                        if (lExt.ToLower() == "ifc" || lExt.ToLower() == "ifczip" || lExt.ToLower() == "ifcxml")
                        {
                            var lFileUrlIFC = "/sites/DigiOSDocs/Shared Documents/DrawingsLib/" + lCustomerName + "/" + lProjectTitle + "/" + lFileName.Substring(0, lFileName.LastIndexOf(".")) + ".wexbim";
                            if (FileExists(lClient, lFileUrlIFC) == true)
                            {

                                var lFileToDelete = lClient.Web.GetFileByServerRelativeUrl(lFileUrlIFC);

                                lFileToDelete.DeleteObject();

                                lClient.ExecuteQueryAsync();
                            }

                        }

                        lSQL = "DELETE " +
                        "FROM dbo.OESDrawingsOrder " +
                        "WHERE DrawingID = " + DrawingID.ToString() + " " +
                        "AND Revision = " + lRevision.ToString() + " ";

                        lCmd.CommandText = lSQL;
                        lCmd.Connection = lNDSCon;
                        lCmd.CommandTimeout = 300;
                        lCmd.ExecuteNonQuery();


                        if (lRevision == 0)
                        {
                            lSQL = "DELETE " +
                            "FROM dbo.OESDrawingsWBS " +
                            "WHERE DrawingID = " + DrawingID.ToString() + " " +
                            "AND Revision = " + lRevision.ToString() + " ";

                            lCmd.CommandText = lSQL;
                            lCmd.Connection = lNDSCon;
                            lCmd.CommandTimeout = 300;
                            lCmd.ExecuteNonQuery();

                            lSQL = "DELETE " +
                            "FROM dbo.OESDrawings " +
                            "WHERE DrawingID = " + DrawingID + " ";

                            lCmd.CommandText = lSQL;
                            lCmd.Connection = lNDSCon;
                            lCmd.CommandTimeout = 300;
                            lCmd.ExecuteNonQuery();
                        }
                        else
                        {
                            lRevision = lRevision - 1;

                            lSQL = "UPDATE dbo.OESDrawings " +
                            "SET Revision = " + lRevision.ToString() + " " +
                            "WHERE DrawingID = " + DrawingID + " ";

                            lCmd.CommandText = lSQL;
                            lCmd.Connection = lNDSCon;
                            lCmd.CommandTimeout = 300;
                            lCmd.ExecuteNonQuery();

                            lSQL = "UPDATE dbo.OESDrawingsWBS " +
                            "SET Revision = " + lRevision.ToString() + " " +
                            "WHERE DrawingID = " + DrawingID + " " +
                            "AND Revision = " + (lRevision + 1).ToString() + " ";

                            lCmd.CommandText = lSQL;
                            lCmd.Connection = lNDSCon;
                            lCmd.CommandTimeout = 300;
                            lCmd.ExecuteNonQuery();

                        }
                    }

                    lProcessObj.CloseNDSConnection(ref lNDSCon);
                    lProcessObj = null;
                }
                catch (Exception ex)
                {
                    lReturn = false;
                    lErrorMsg = ex.Message;
                }
            }

            return Ok(new { success = lReturn, message = lErrorMsg });
        }

        public static bool FileExists(ClientContext context, Microsoft.SharePoint.Client.List list, string fileName)
        {
            string url = list.RootFolder.ServerRelativeUrl + "/" + fileName;
            return FileExists(context, url);
        }

        public static bool FileExists(ClientContext context, Microsoft.SharePoint.Client.Folder currentListFolder, string fileName)
        {
            string url = currentListFolder.ServerRelativeUrl + "/" + fileName;
            return FileExists(context, url);
        }

        private static bool FileExists(ClientContext context, string url)
        {
            var file = context.Web.GetFileByServerRelativeUrl(url);
            context.Load(file, f => f.Exists);
            try
            {
                context.ExecuteQueryAsync();

                if (file.Exists)
                {
                    return true;
                }
                return false;
            }
            //catch (ServerUnauthorizedAccessException uae)
            //{
            //    Trace.WriteLine($"You are not allowed to access this file");
            //    throw;
            //}
            catch (Exception ex)
            {
                //Trace.WriteLine($"Could not find file {url}");
                return false;
            }
        }
        
        [HttpPost]
        [Route("/modifyDrawing")]
        public ActionResult modifyDrawing(ModifyDrawingDto modifyDrawing)
        {

            int DrawingID = modifyDrawing.DrawingID;
            string DrawingNo = modifyDrawing.DrawingNo;
            string Remarks = modifyDrawing.Remarks;
            string lSQL = "";
            var lCmd = new SqlCommand();
            var lNDSCon = new SqlConnection();
            SqlDataReader lRst;
            bool lReturn = true;
            string lErrorMsg = "";

            try
            {

                var lProcessObj = new ProcessController();
                lProcessObj.OpenNDSConnection(ref lNDSCon);

                DrawingNo = DrawingNo.Replace("'", "''");
                Remarks = Remarks.Replace("'", "''");

                lSQL = "UPDATE dbo.OESDrawings " +
                "SET DrawingNo = '" + DrawingNo + "', " +
                "Remarks = '" + Remarks + "' " +
                "WHERE DrawingID = " + DrawingID + " ";

                lCmd.CommandText = lSQL;
                lCmd.Connection = lNDSCon;
                lCmd.CommandTimeout = 300;
                lRst = lCmd.ExecuteReader();

                lRst.Close();

                lProcessObj.CloseNDSConnection(ref lNDSCon);
                lProcessObj = null;

            }
            catch (Exception ex)
            {
                lReturn = false;
                lErrorMsg = ex.Message;
            }

            return Ok(lReturn);
        }
    }
}
