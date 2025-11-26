using Microsoft.AspNet.Identity;
using Microsoft.SharePoint.Client;
using SAP_API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Net.Mime;
using Oracle.ManagedDataAccess.Client;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Principal;
//using System.Web.Http.Cors;

namespace SAP_API.Controllers
{
    [System.Web.Http.RoutePrefix("api/SharePointAPI")]
   // [EnableCors(origins: "https://devodos.natsteel.com.sg", headers: "*", methods: "*")]
    [ExceptionHandler]
    public class SharePointController : ApiController

    {
        private DBContextModels db = new DBContextModels();
        public const string gSiteURL = "https://natsteel.sharepoint.com/sites/DigiOSDocs";
        // GET: SharePoint
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("ConnectionSP")]
        public int ConnectionSP()
        {
            string gSiteURL = "https://natsteel.sharepoint.com/sites/DigiOSDocs";
            string userName = "DigiOSNoReply@natsteel.com.sg";
            string password = "NatSteel@123B";
            var securePassword = new SecureString();
            foreach (char c in password)
            {
                securePassword.AppendChar(c);
            }
            string lCustomerName = "Demo";
            string lProjectTitle = "subfolder";
            //ClientContext clientContext = O365LogOn();
            //Microsoft.SharePoint.Client.List docLib = clientContext.Web.Lists.GetByTitle("Documents");
            //clientContext.Load(docLib);
            //clientContext.ExecuteQueryAsync();
            using (var clientContext = new ClientContext("https://natsteel.sharepoint.com/sites/DigiOSDocs"))
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                Microsoft.SharePoint.Client.List docLib = clientContext.Web.Lists.GetByTitle("Documents");
                //HttpPostedFileBase file = files[i];

                //var lFileName = file.FileName;
                clientContext.Credentials = new SharePointOnlineCredentials(userName, securePassword);

                if (FolderExists(clientContext, "/sites/DigiOSDocs/Shared Documents/DrawingsLib/" + lCustomerName) == false)
                {
                    var fld1 = docLib.RootFolder.Folders.GetByUrl("/sites/DigiOSDocs/Shared Documents/DrawingsLib");
                    //var fld1 = docLib.RootFolder;
                    var fld2 = fld1.Folders.Add(lCustomerName);
                    var fld3 = fld2.Folders.Add(lProjectTitle);
                    fld2.Update();
                    fld3.Update();
                    clientContext.ExecuteQuery();

                }
                else if (FolderExists(clientContext, "/sites/DigiOSDocs/Shared Documents/DrawingsLib/" + lCustomerName + "/" + lProjectTitle) == false)
                {
                    var fld1 = docLib.RootFolder.Folders.GetByUrl("/sites/DigiOSDocs/Shared Documents/DrawingsLib");
                    var fld2 = fld1.Folders.Add(lCustomerName);
                    var fld3 = fld2.Folders.Add(lProjectTitle);
                    fld3.Update();
                    clientContext.ExecuteQuery();
                }

                FileCreationInformation createFile = new FileCreationInformation();

                //var lFileURL = gSiteURL + "/Shared Documents/DrawingsLib/" + lCustomerName + "/" + lProjectTitle + "/" + "";
                string fileUrl = gSiteURL + "/Shared Documents/DrawingsLib/" + lCustomerName + "/" + lProjectTitle + "/" + "STDMESH3.pdf";

                //createFile.Content = pdfBytes;
                //createFile.ContentStream = file.InputStream;
                string localFilePath = @"D:\LocalPath\" + "STDMESH3.pdf"; // Replace with your local file path

                using (FileStream fs = new FileStream(localFilePath, FileMode.Open))
                {
                    FileCreationInformation fileInfo = new FileCreationInformation
                    {
                        Url = fileUrl,
                        Overwrite = true,
                        ContentStream = fs
                    };

                    Microsoft.SharePoint.Client.File uploadFile = docLib.RootFolder.Files.Add(fileInfo);
                    clientContext.Load(uploadFile);
                    clientContext.ExecuteQuery();

                    Console.WriteLine("File uploaded: ");
                }

            }


            return 0;
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("uploadDrawingFiles")]
        //[ValidateAntiForgeryHeader]
        public IHttpActionResult uploadDrawingFiles()
        {
            var httpRequest = HttpContext.Current.Request;
            var lCustomerCode = httpRequest.Form["Customer"];
            var lProjectCode = httpRequest.Form["Project"];
            var UserName= httpRequest.Form["UserName"];

            var lDrawingIDList = new List<int>();
            string lSQL = "";
            var lCmd = new SqlCommand();
            var lNDSCon = new SqlConnection();
            var lDa = new SqlDataAdapter();
            var lDs = new DataSet();

            //       UserAccessController lUa = new UserAccessController();
            //       var lUserType = lUa.getUserType(User.Identity.GetUserName());



            var lUserType = "AD";



            //       lUa = null;
            SqlDataReader lRst;
            string lSPFileName = "";
            int lFound = 0;

            var Content = new DrawingModels();
            try
            {
                int lRevision = 0;
                int lDrawingID = 0;
                int lOrderNumber = 0;
                int.TryParse(httpRequest.Form["OrderNumber"], out lOrderNumber);//306095;
                int.TryParse(httpRequest.Form["Revision"], out lRevision);
                //var lDrawingNo = Request.Form.Get("DrawingNo").Trim();
                //var lRemarks = Request.Form.Get("Remarks").Trim();
                //var lWBS1 = Request.Form.Get("WBS1").Trim();
                //var lWBS2 = Request.Form.Get("WBS2").Trim();
                //var lWBS3 = Request.Form.Get("WBS3").Trim();
                //var lProdType = Request.Form.Get("Prodtype").Trim();
                //var lStructureElement = Request.Form.Get("StructureElement").Trim();
                //var lUploadType = Request.Form.Get("UploadType").Trim();    // O:Overwrite; R-Revision
                //var lScheduledProd = Request.Form.Get("ScheduledProd");

                var lDrawingNo = httpRequest.Form["DrawingNo"];// "121-3";
                var lRemarks = httpRequest.Form["Remarks"];// "test";
                var lWBS1 = httpRequest.Form["WBS1"];//"100A";
                var lWBS2 = httpRequest.Form["WBS2"];//"1";
                var lWBS3 = httpRequest.Form["WBS3"];//"A";
                var lProdType = httpRequest.Form["ProdType"];//"CAB";
                var lStructureElement = httpRequest.Form["StructureElement"];// "BEAM";
                var lUploadType = httpRequest.Form["UploadType"];//"R";
                var lScheduledProd = httpRequest.Form["ScheduledProd"];//"N";

                if (lScheduledProd == null)
                {
                    lScheduledProd = "";
                }
                lScheduledProd = lScheduledProd.Trim();


                if (lScheduledProd == "")
                {
                    lScheduledProd = "N";
                }

                if (httpRequest.Files.Count > 0)
                {
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                    ClientContext lClient = O365LogOn();
                    Microsoft.SharePoint.Client.List docLib = lClient.Web.Lists.GetByTitle("Documents");
                    lClient.Load(docLib);
                    lClient.ExecuteQuery();
                    var lProcessObj = new ProcessController();
                    lProcessObj.OpenNDSConnection(ref lNDSCon);
                    HttpFileCollection files = httpRequest.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        lRevision = 0;
                        HttpPostedFile file = files[i];
                        var lFileName = file.FileName;

                        if (lFileName.LastIndexOf("\\") >= 0)
                        {
                            lFileName = lFileName.Substring(lFileName.LastIndexOf("\\") + 1);
                        }
                        if (lFileName.LastIndexOf("/") >= 0)
                        {
                            lFileName = lFileName.Substring(lFileName.LastIndexOf("/") + 1);
                        }

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
                                "UpdatedBy = '" + UserName + "' " +
                                "WHERE CustomerCode = '" + lCustomerCode + "' " +
                                "AND ProjectCode = '" + lProjectCode + "' " +
                                "AND FileName = N'" + lFileName + "' ";
                            }
                            else
                            {
                                lSQL = "UPDATE dbo.OESDrawings " +
                                "SET Revision = " + lRevision.ToString() + ", " +
                                "UpdatedDate = getDate(), " +
                                "UpdatedBy = '" + UserName + "' " +
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
                        string lProjectTitle = getProjectName(lCustomerCode, lProjectCode, UserName);
                        lCustomerName = removeSpecialChar(lCustomerName);
                        lProjectTitle = removeSpecialChar(lProjectTitle);
                        lFileName = removeSpecialChar(lFileName);

                        //lCustomerCode = "Demo";
                        //lProjectTitle = "Subfolder";

                        if (FolderExists(lClient, "/sites/DigiOSDocs/Shared Documents/DrawingsLib/" + lCustomerName) == false)
                        {
                            var fld1 = docLib.RootFolder.Folders.GetByUrl("/sites/DigiOSDocs/Shared Documents/DrawingsLib");
                            //var fld1 = docLib.RootFolder;
                            var fld2 = fld1.Folders.Add(lCustomerName);
                            var fld3 = fld2.Folders.Add(lProjectTitle);
                            fld2.Update();
                            fld3.Update();
                            lClient.ExecuteQuery();
                        }
                        else if (FolderExists(lClient, "/sites/DigiOSDocs/Shared Documents/DrawingsLib/" + lCustomerName + "/" + lProjectTitle) == false)
                        {
                            var fld1 = docLib.RootFolder.Folders.GetByUrl("/sites/DigiOSDocs/Shared Documents/DrawingsLib");
                            var fld2 = fld1.Folders.Add(lCustomerName);
                            var fld3 = fld2.Folders.Add(lProjectTitle);
                            fld3.Update();
                            lClient.ExecuteQuery();
                        }

                        FileCreationInformation createFile = new FileCreationInformation();
                        var lFileURL = gSiteURL + "/Shared Documents/DrawingsLib/" + lCustomerName + "/" + lProjectTitle + "/" + lSPFileName;

                        //createFile.Content = pdfBytes;
                        createFile.ContentStream = file.InputStream;
                        createFile.Url = lFileURL;
                        createFile.Overwrite = true;
                        Microsoft.SharePoint.Client.File newFile = docLib.RootFolder.Files.Add(createFile);


                        newFile.CheckOut();

                        newFile.ListItemAllFields.Update();
                        newFile.CheckIn(User.Identity.Name, CheckinType.MajorCheckIn);
                        lClient.ExecuteQuery();

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
                                UpdatedBy = UserName//User.Identity.Name
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
                                ",'" + UserName + "') ";

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
                                ",'" + UserName + "') ";
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
                            //"AND (CASE  " +
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
                            //    for (int j = 0; j < lDs.Tables[0].Rows.Count; j++)
                            //    {
                            //        var lProductType = lDs.Tables[0].Rows[j].ItemArray[0].ToString();
                            //        var lStructureEle = lDs.Tables[0].Rows[j].ItemArray[1].ToString();
                            //        var lWBS1A = lDs.Tables[0].Rows[j].ItemArray[2].ToString();
                            //        var lWBS2A = (int)lDs.Tables[0].Rows[j].ItemArray[3];

                            //        if (lWBS2A > 0)
                            //        {
                            //            lSQL = "UPDATE dbo.OESDrawingsWBS " +
                            //            "SET Revision = " + lRevision.ToString() + " " +
                            //            "WHERE DrawingID = " + Content.DrawingID.ToString() + " " +
                            //            "AND (CASE  " +
                            //            "when ProductType = 'MESH' then 'MSH' " +
                            //            "when ProductType = 'CUT-TO-SIZE-MESH' then 'MSH' " +
                            //            "when ProductType = 'STIRRUP-LINK-MESH' then 'MSH' " +
                            //            "when ProductType = 'COLUMN-LINK-MESH' then 'MSH' " +
                            //            "when ProductType = 'PRE-CAGE' then 'PRC' " +
                            //            "when ProductType = 'CORE-CAGE' then 'CAR' " +
                            //            "when ProductType = 'CARPET' then 'CAR' " +
                            //            "else ProductType END) = '" + lProductType + "' " +
                            //            "AND StructureElement = '" + lStructureEle + "' " +
                            //            "AND WBS1 = '" + lWBS1A + "' " +
                            //            "AND ((WBS2 > '" + lWBS2A.ToString() + "' " +
                            //            "AND LEN(WBS2) = LEN('" + lWBS2A.ToString() + "')) " +
                            //            "OR  LEN(WBS2) > LEN('" + lWBS2A.ToString() + "')) " +
                            //            "AND isNumeric(WBS2) = 1 " +
                            //            "AND NOT EXISTS(SELECT B.intWBSElementId " +
                            //            "FROM dbo.WBSElements W, " +
                            //            "dbo.WBSElementsDetails D, " +
                            //            "dbo.StructureElementMaster S, " +
                            //            "dbo.ProductTypeMaster P, " +
                            //            "dbo.BBSPostHeader B, " +
                            //            "dbo.BBSReleaseDetails R, " +
                            //            "dbo.ProjectMaster J " +
                            //            "WHERE B.intWBSElementId = W.intWBSElementId " +
                            //            "AND D.intWBSElementId = W.intWBSElementId " +
                            //            "AND D.sitProductTypeId = P.sitProductTypeID " +
                            //            "AND B.sitProductTypeId = P.sitProductTypeID " +
                            //            "AND D.intStructureElementTypeId = S.intStructureElementTypeId " +
                            //            "AND J.intProjectId = B.intProjectId " +
                            //            "AND R.intPostHeaderid = B.intPostHeaderId " +
                            //            "AND J.vchProjectCode = '" + lProjectCode + "' " +
                            //            "AND R.tntStatusId = 12 " +
                            //            "AND P.vchProductType = '" + lProductType + "' " +
                            //            "AND S.vchStructureElementType = '" + lStructureEle + "' " +
                            //            "AND W.vchWBS2 NOT LIKE 'B%' " +
                            //            "AND W.vchWBS2 NOT LIKE 'FDN%' " +
                            //            "AND W.vchWBS1 = dbo.OESDrawingsWBS.WBS1 " +
                            //            "AND W.vchWBS2 = dbo.OESDrawingsWBS.WBS2 " +
                            //            "AND W.vchWBS3 = dbo.OESDrawingsWBS.WBS3) ";

                            //            lCmd.CommandText = lSQL;
                            //            lCmd.Connection = lNDSCon;
                            //            lCmd.CommandTimeout = 300;
                            //            lCmd.ExecuteNonQuery();
                            //        }
                            //    }
                            //}

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
    "(CASE  " +
    "when dbo.OESDrawingsWBS.ProductType = 'MESH' then 'MSH' " +
    "when dbo.OESDrawingsWBS.ProductType = 'CUT-TO-SIZE-MESH' then 'MSH' " +
    "when dbo.OESDrawingsWBS.ProductType = 'STIRRUP-LINK-MESH' then 'MSH' " +
    "when dbo.OESDrawingsWBS.ProductType = 'COLUMN-LINK-MESH' then 'MSH' " +
    "when dbo.OESDrawingsWBS.ProductType = 'PRE-CAGE' then 'PRC' " +
    "when dbo.OESDrawingsWBS.ProductType = 'CORE-CAGE' then 'CAR' " +
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

            var lReturn = lSaved;
            //lReturn.MaxJsonLength = 50000000;

            return Ok(lReturn);
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
                context.ExecuteQuery();

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
            //var lUserType = lUa.getUserType(pUserID);
            var lGroupName = "Dev";//lUa.getGroupName(pUserID);
            lUa = null;

            string lProjectTitle = "";

            var lSharedAPI = new SharedAPIController();

            lProjectTitle = "";
            lProjectTitle = getProjectTitle(pCustomerCode, pProjectCode, pUserID, lGroupName);

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


        public string getProjectTitle(string pCustomerCode, string pProjectCode, string pUserType, string pGroupName)
        {
            string lUserType = pUserType;
            string lGroupName = pGroupName;
            string lErrorMsg = "";
            string lReturn = "";

            try
            {
                if (lUserType == "PL" || lUserType == "AD" || lUserType == "PM" || lUserType == "PA" || lUserType == "P1" || lUserType == "P2" || lUserType == "PU" || lUserType == "TE")
                {
                    lReturn = (from p in db.ProjectList
                               where p.CustomerCode == pCustomerCode &&
                               p.ProjectCode == pProjectCode
                               select p.ProjectTitle).FirstOrDefault();

                    if (lReturn == null || lReturn == "")
                    {
                        //for Spot order
                        lReturn = (from p in db.ProjectList
                                   where p.CustomerCode == "0000000000" &&
                                   p.ProjectCode == pProjectCode
                                   select p.ProjectTitle).FirstOrDefault();

                        if (lReturn == null || lReturn == "")
                        {
                            //Create Project
                            //var lProjectObj = new OrderDetailsController();
                            // var lProjectObj=createProject(pCustomerCode, pProjectCode);
                            //lProjectObj = null;
                        }

                        #region Get from SAP
                        OracleDataReader lRst;
                        var lOCmd = new OracleCommand();
                        var lOcisCon = new OracleConnection();
                        var lProcess = new ProcessController();

                        lOCmd.CommandText = "SELECT (NAME1 || NAME2) AS SHIP_TO_NAME FROM SAPSR3.KNA1 " +
                        "WHERE KTOKD = 'Y001' AND MANDT ='" + lProcess.strClient + "' " +
                        "AND KUNNR = '" + pProjectCode + "' ";

                        if (lProcess.OpenCISConnection(ref lOcisCon) == true)
                        {
                            lOCmd.Connection = lOcisCon;
                            lOCmd.CommandTimeout = 300;
                            lRst = lOCmd.ExecuteReader();
                            if (lRst.HasRows)
                            {
                                if (lRst.Read())
                                {
                                    lReturn = lRst.GetString(0);
                                }
                            }
                            lRst.Close();

                            lProcess.CloseCISConnection(ref lOcisCon);

                        }
                        lOCmd = null;
                        lOcisCon = null;
                        lProcess = null;
                    }
                    #endregion
                }
                else
                {
                    lReturn = (from p in db.ProjectList
                               where p.CustomerCode == pCustomerCode &&
                               p.ProjectCode == pProjectCode
                               select p.ProjectTitle).FirstOrDefault();
                }

            }
            catch (Exception e)
            {
                lErrorMsg = e.Message;
            }

            return lReturn;
        }
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
                    SecureString securestring = new SecureString();
                    lPassWD.ToCharArray().ToList().ForEach(s => securestring.AppendChar(s));
                    clientContext.Credentials = new SharePointOnlineCredentials(lUserName, securestring);
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

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("deleteDrawing/{CustomerCode}/{ProjectCode}/{DrawingID}")]
        public IHttpActionResult deleteDrawing(string CustomerCode, string ProjectCode, int DrawingID)

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

                        ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

                        ClientContext lClient = O365LogOn();

                        Microsoft.SharePoint.Client.List docLib = lClient.Web.Lists.GetByTitle("Documents");

                        lClient.Load(docLib);

                        lClient.ExecuteQuery();

                        string lCustomerName = getCustomerName(lCustomerCode);

                        string lProjectTitle = getProjectName(lCustomerCode, lProjectCode, "");

                        lCustomerName = removeSpecialChar(lCustomerName);

                        lProjectTitle = removeSpecialChar(lProjectTitle);

                        var lFileUrl = "/sites/DigiOSDocs/Shared Documents/DrawingsLib/" + lCustomerName + "/" + lProjectTitle + "/" + lFileName;

                        if (FileExists(lClient, lFileUrl) == true)

                        {

                            var lFileToDelete = lClient.Web.GetFileByServerRelativeUrl(lFileUrl);

                            lFileToDelete.DeleteObject();

                            lClient.ExecuteQuery();

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

                                lClient.ExecuteQuery();

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

            return Ok(lReturn);

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
                context.ExecuteQuery();

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

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("downloadDrawings/{ddCustomerCode}/{ddProjectCode}/{ddFileName}/{ddRevision}")]
        public byte[] downloadDrawings(string ddCustomerCode, string ddProjectCode, string ddFileName, int ddRevision)
        {
            try
            {
                string lCustomerName = getCustomerName(ddCustomerCode);
                string lProjectTitle = getProjectName(ddCustomerCode, ddProjectCode, User.Identity.GetUserName());
                lCustomerName = removeSpecialChar(lCustomerName);
                lProjectTitle = removeSpecialChar(lProjectTitle);

                //string lFileName = Server.HtmlDecode(ddFileName);
                string lFileName = System.Web.HttpUtility.HtmlDecode(ddFileName);
                //lFileName = removeSpecialChar(lFileName);

                if (lFileName.LastIndexOf(".") >= 0)
                {
                    lFileName = lFileName.Substring(0, lFileName.LastIndexOf(".")) + "-R" + ddRevision.ToString() + "" + lFileName.Substring(lFileName.LastIndexOf("."));
                }
                else
                {
                    lFileName = lFileName + "-R" + ddRevision.ToString();
                }

                string lServerRelative = "/sites/DigiOSDocs/Shared Documents/DrawingsLib/" + lCustomerName + "/" + lProjectTitle + "/" + lFileName;

                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                ClientContext lClient = O365LogOn();
                Microsoft.SharePoint.Client.List docLib = lClient.Web.Lists.GetByTitle("Documents");
                lClient.Load(docLib);
                lClient.ExecuteQuery();

                var file = lClient.Web.GetFileByServerRelativeUrl(lServerRelative);
                lClient.Load(file);
                lClient.ExecuteQuery();
                ClientResult<Stream> streamResult = file.OpenBinaryStream();
                lClient.ExecuteQuery();

                var lMemoryStream = new MemoryStream();
                streamResult.Value.CopyTo(lMemoryStream);
                var lFileBinary = lMemoryStream.ToArray();

                System.Net.Mime.ContentDisposition cd = new System.Net.Mime.ContentDisposition
                {
                    FileName = lFileName,
                    Inline = false
                };

                string contentType = MimeMapping.GetMimeMapping(lFileName);
                //Response.AppendHeader("Access-Control-Allow-Origin", "*");
                //Response.AppendHeader("Access-Control-Allow-Methods", "POST, GET, OPTIONS");
                //Response.AppendHeader("Access-Control-Allow-Headers", "X-PINGOTHER, Content-Type");
                //return File(lFileBinary, contentType, lFileName);
                // string fileName = "abc.pdf";

                // Create a MemoryStream from the byte array
                //using (MemoryStream memoryStream = new MemoryStream(lFileBinary))
                //{
                //    HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                //    response.Content = new StreamContent(memoryStream);
                //    response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                //    response.Content.Headers.ContentDisposition.FileName = lFileName;
                //    response.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                //    //return response.Content.ReadAsByteArrayAsync().Result;
                //    //return response.Content.ReadAsByteArrayAsync().Result;
                //    //return (File(lFileBinary, contentType, lFileName));
                //    return lFileBinary;
                //}
                //return Json(FileBinary = lFileBinary, contentType = contentType, FileName = lFileName);
                //return Json(new { FileBinary = lFileBinary, contentType = contentType, FileName = lFileName });
                //var lReturn= Json(new { FileBinary = lFileBinary, contentType = contentType, FileName = lFileName });
                //var result = Request.CreateResponse(HttpStatusCode.OK);
                //result.Content = new StringContent(lReturn.ToString(), Encoding.UTF8, "application/json");
                return lFileBinary;

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Server Error: " + ex.Message);
            }
            //var lReturn = Json(imageDataURL, JsonRequestBehavior.AllowGet);
            //lReturn.MaxJsonLength = 80000000;
            return null;

        }



        //[System.Web.Http.Route("ShowDir/{ddCustomerCode}/{ddProjectCode}/{ddFileName}/{ddRevision}")]
        //public IHttpActionResult ShowDir(string ddCustomerCode, string ddProjectCode, string ddFileName, int ddRevision)


        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("ShowDirView")]
        public HttpResponseMessage ShowDirView(ShowDirDownloadPostDto showDirDownloadPostDto)

        {
            //ddCustomerCode = "0001101170";
            //ddProjectCode = "0000113012";
            //ddFileName = "CAB-BBS769-R0 (2)-R0.pdf";
            //ddRevision = 0;

            string ddCustomerCode = showDirDownloadPostDto.ddCustomerCode;
            string ddProjectCode = showDirDownloadPostDto.ddProjectCode;
            string ddFileName = showDirDownloadPostDto.ddFileName;
            int ddRevision = showDirDownloadPostDto.ddRevision;
            string UserType = showDirDownloadPostDto.UserType;

            string lCustomerName = getCustomerName(ddCustomerCode);
            string lProjectTitle = getProjectName(ddCustomerCode, ddProjectCode, UserType);
            lCustomerName = removeSpecialChar(lCustomerName);
            lProjectTitle = removeSpecialChar(lProjectTitle);
            //string lFileName = Server.HtmlDecode(ddFileName);

            string lFileName = System.Web.HttpUtility.HtmlDecode(ddFileName);
            //lFileName = removeSpecialChar(lFileName);

            if (lFileName.LastIndexOf(".") >= 0)
            {
                lFileName = lFileName.Substring(0, lFileName.LastIndexOf(".")) + "-R" + ddRevision.ToString() + "" + lFileName.Substring(lFileName.LastIndexOf("."));
            }

            else

            {

                lFileName = lFileName + "-R" + ddRevision.ToString();

            }

            string lServerRelative = "/sites/DigiOSDocs/Shared Documents/DrawingsLib/" + lCustomerName + "/" + lProjectTitle + "/" + lFileName;

            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

            ClientContext lClient = O365LogOn();

            Microsoft.SharePoint.Client.List docLib = lClient.Web.Lists.GetByTitle("Documents");

            lClient.Load(docLib);

            lClient.ExecuteQuery();

            var file = lClient.Web.GetFileByServerRelativeUrl(lServerRelative);

            lClient.Load(file);

            lClient.ExecuteQuery();

            ClientResult<Stream> streamResult = file.OpenBinaryStream();

            lClient.ExecuteQuery();

            var lMemoryStream = new MemoryStream();

            streamResult.Value.CopyTo(lMemoryStream);

            var lFileBinary = lMemoryStream.ToArray();
            //  string fileName = lFileName;
            string contentType = MimeMapping.GetMimeMapping(lFileName);
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            MemoryStream memoryStream = new MemoryStream(lFileBinary);
            StreamContent streamContent = new StreamContent(memoryStream);
            response.Content = streamContent;
            response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = lFileName;
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue(System.Net.Mime.DispositionTypeNames.Inline)
            {
                FileName = lFileName
            };


            return response;
            ////return Ok(lFileBinary);
            //var result = Request.CreateResponse(HttpStatusCode.OK);
            //result.Content = new StringContent(lFileBinary.ToString(), Encoding.UTF8, "application/json");
            //return Ok(result);

        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("ShowDirDownload")]
        public HttpResponseMessage ShowDirDownload(ShowDirDownloadPostDto showDirDownloadPostDto)
        {
            //ddCustomerCode = "0001101170";
            //ddProjectCode = "0000113012";
            //ddFileName = "CAB-BBS769-R0 (2)-R0.pdf";
            //ddRevision = 0;
            string ddCustomerCode = showDirDownloadPostDto.ddCustomerCode;
            string ddProjectCode = showDirDownloadPostDto.ddProjectCode;
            string ddFileName = showDirDownloadPostDto.ddFileName;
            int ddRevision = showDirDownloadPostDto.ddRevision;
            string UserType = showDirDownloadPostDto.UserType;

            string lCustomerName = getCustomerName(ddCustomerCode);
            string lProjectTitle = getProjectName(ddCustomerCode, ddProjectCode, UserType);
            lCustomerName = removeSpecialChar(lCustomerName);
            lProjectTitle = removeSpecialChar(lProjectTitle);
            //string lFileName = Server.HtmlDecode(ddFileName);

            string lFileName = System.Web.HttpUtility.HtmlDecode(ddFileName);
            //lFileName = removeSpecialChar(lFileName);

            if (lFileName.LastIndexOf(".") >= 0)
            {
                lFileName = lFileName.Substring(0, lFileName.LastIndexOf(".")) + "-R" + ddRevision.ToString() + "" + lFileName.Substring(lFileName.LastIndexOf("."));
            }

            else

            {

                lFileName = lFileName + "-R" + ddRevision.ToString();

            }

            string lServerRelative = "/sites/DigiOSDocs/Shared Documents/DrawingsLib/" + lCustomerName + "/" + lProjectTitle + "/" + lFileName;

            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

            ClientContext lClient = O365LogOn();

            Microsoft.SharePoint.Client.List docLib = lClient.Web.Lists.GetByTitle("Documents");

            lClient.Load(docLib);

            lClient.ExecuteQuery();

            var file = lClient.Web.GetFileByServerRelativeUrl(lServerRelative);

            lClient.Load(file);

            lClient.ExecuteQuery();

            ClientResult<Stream> streamResult = file.OpenBinaryStream();

            lClient.ExecuteQuery();

            var lMemoryStream = new MemoryStream();

            streamResult.Value.CopyTo(lMemoryStream);

            var lFileBinary = lMemoryStream.ToArray();
            //  string fileName = lFileName;
            string contentType = MimeMapping.GetMimeMapping(lFileName);
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            MemoryStream memoryStream = new MemoryStream(lFileBinary);
            StreamContent streamContent = new StreamContent(memoryStream);
            response.Content = streamContent;
            response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = lFileName;
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            //response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue(System.Net.Mime.DispositionTypeNames.Inline)
            //{
            //    FileName = lFileName
            //};


            return response;
            ////return Ok(lFileBinary);
            //var result = Request.CreateResponse(HttpStatusCode.OK);
            //result.Content = new StringContent(lFileBinary.ToString(), Encoding.UTF8, "application/json");
            //return Ok(result);

        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("getDrawing")]
        public HttpResponseMessage getDrawing(getDrawingDto getDrawingDto)
        {
            //byte[] lFileBinary1 = null;
            //try
            //{
            string ddCustomerCode = getDrawingDto.ddCustomerCode;
            string ddProjectCode = getDrawingDto.ddProjectCode;
            string ddFileName = getDrawingDto.ddFileName;
            int ddRevision=getDrawingDto.ddRevision;
            string ddUserID=getDrawingDto.ddUserID;
                string lCustomerName = getCustomerName(ddCustomerCode);
                string lProjectTitle = getProjectName(ddCustomerCode, ddProjectCode, ddUserID);
                lCustomerName = removeSpecialChar(lCustomerName);
                lProjectTitle = removeSpecialChar(lProjectTitle);

                string lFileName = HttpUtility.HtmlDecode(ddFileName);

                if (lFileName.LastIndexOf(".") >= 0)
                {
                    lFileName = lFileName.Substring(0, lFileName.LastIndexOf(".")) + "-R" + ddRevision.ToString() + "" + lFileName.Substring(lFileName.LastIndexOf("."));
                }
                else
                {
                    lFileName = lFileName + "-R" + ddRevision.ToString();
                }

                string lServerRelative = "/sites/DigiOSDocs/Shared Documents/DrawingsLib/" + lCustomerName + "/" + lProjectTitle + "/" + lFileName;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            ClientContext lClient = O365LogOn();
                Microsoft.SharePoint.Client.List docLib = lClient.Web.Lists.GetByTitle("Documents");
                lClient.Load(docLib);
                lClient.ExecuteQueryAsync();

                var file = lClient.Web.GetFileByServerRelativeUrl(lServerRelative);
                lClient.Load(file);
                lClient.ExecuteQuery();
                ClientResult<Stream> streamResult = file.OpenBinaryStream();
                lClient.ExecuteQuery();

                //var lMemoryStream = new MemoryStream();
                //streamResult.Value.CopyTo(lMemoryStream);
                //lFileBinary = lMemoryStream.ToArray();
                var lMemoryStream = new MemoryStream();

                streamResult.Value.CopyTo(lMemoryStream);

                var lFileBinary2 = lMemoryStream.ToArray();
                //  string fileName = lFileName;
                string contentType = MimeMapping.GetMimeMapping(lFileName);
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                MemoryStream memoryStream = new MemoryStream(lFileBinary2);
                StreamContent streamContent = new StreamContent(memoryStream);
                response.Content = streamContent;
                response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                response.Content.Headers.ContentDisposition.FileName = lFileName;
                response.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                return response;
            //}
            //catch (Exception ex)
            //{
            //    //lFileBinary2 = null;
            //}
            //return lFileBinary;
            
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("ActionRR")]
        public IHttpActionResult ActionRR()
        {
            return Ok();
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("ActionRR2")]
        public IHttpActionResult ActionRR2()
        {
            var test = "hello";
            return Ok(test);
        }

        #region Impersionation global variables
        public const int LOGON32_LOGON_INTERACTIVE = 2;
        public const int LOGON32_PROVIDER_DEFAULT = 0;

        WindowsImpersonationContext impersonationContext;

        [DllImport("advapi32.dll")]
        public static extern int LogonUserA(String lpszUserName,
            String lpszDomain,
            String lpszPassword,
            int dwLogonType,
            int dwLogonProvider,
            ref IntPtr phToken);
        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int DuplicateToken(IntPtr hToken,
            int impersonationLevel,
            ref IntPtr hNewToken);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool RevertToSelf();

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern bool CloseHandle(IntPtr handle);
        #endregion
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("ImpersonateUser")]
       
        public bool Action2()
        {
            var domain = "NATSTEELCORP";
            var userName = "DigiOSNoReply";
            var password = "NatSteel@123B";
            WindowsIdentity tempWindowsIdentity;
            IntPtr token = IntPtr.Zero;
            IntPtr tokenDuplicate = IntPtr.Zero;

            if (RevertToSelf())
            {
                if (LogonUserA(userName, domain, password, LOGON32_LOGON_INTERACTIVE,
                    LOGON32_PROVIDER_DEFAULT, ref token) != 0)
                {
                    if (DuplicateToken(token, 2, ref tokenDuplicate) != 0)
                    {
                        tempWindowsIdentity = new WindowsIdentity(tokenDuplicate);
                        WindowsImpersonationContext impersonationContext = tempWindowsIdentity.Impersonate();
                        if (impersonationContext != null)
                        {
                            CloseHandle(token);
                            CloseHandle(tokenDuplicate);
                            return true;
                        }
                    }
                }
            }
            if (token != IntPtr.Zero)
                CloseHandle(token);
            if (tokenDuplicate != IntPtr.Zero)
                CloseHandle(tokenDuplicate);
            return false;
        }

        //WindowsImpersonationContext impersonationContext;

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("undoImpersonation")]
        private void ABCD()
        {
            impersonationContext.Undo();
        }

        
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("viewDrawing")]
        public byte[] viewDrawing(int DrawingID, int Revision)
        {

            string imageDataURL = "";

            try
            {
                var Content = (from p in db.Drawings
                               where p.DrawingID == DrawingID
                               select p).ToList();

                if (Content != null && Content.Count > 0)
                {
                    string lCustomerName = getCustomerName(Content[0].CustomerCode);
                    string lProjectTitle = getProjectName(Content[0].CustomerCode, Content[0].ProjectCode, User.Identity.GetUserName());
                    lCustomerName = removeSpecialChar(lCustomerName);
                    lProjectTitle = removeSpecialChar(lProjectTitle);

                    string lFileName = Content[0].FileName;

                    if (lFileName.LastIndexOf(".") >= 0)
                    {
                        lFileName = lFileName.Substring(0, lFileName.LastIndexOf(".")) + "-R" + Revision.ToString() + "" + lFileName.Substring(lFileName.LastIndexOf("."));
                    }
                    else
                    {
                        lFileName = lFileName + "-R" + Revision.ToString();
                    }

                    //string lServerRelative = gSiteURL + "/Shared Documents/DrawingsLib/" + lCustomerName + "/" + lProjectTitle + "/" + Content[0].FileName;
                    string lServerRelative = "/sites/DigiOSDocs/Shared Documents/DrawingsLib/" + lCustomerName + "/" + lProjectTitle + "/" + lFileName;

                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
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

                    //var result = Convert.ToBase64String(lFileBinary);


                    System.Net.Mime.ContentDisposition cd = new System.Net.Mime.ContentDisposition
                    {
                        FileName = lFileName,
                        Inline = false
                    };

                    string contentType = MimeMapping.GetMimeMapping(lFileName);
                    //var provider = new FileExtensionContentTypeProvider();
                    //if (!provider.TryGetContentType(lFileName, out string contentType))
                    //{
                    //    contentType = "application/octet-stream"; // Default content type if not found
                    //}

                    //Response.AppendHeader("Content-Disposition", cd.ToString());

                    //Response.Headers.Add("X-Content-Type-Options", "nosniff");

                    //Response.AppendHeader("Content-Disposition", "inline; filename=" + Content[0].FileName + "");
                    //Response.AppendHeader("Content-Type", contentType);

                    //Response.AppendHeader("Content-Disposition", "attachment;filename=" + Content[0].FileName + "; size=" + lFileBinary.Length.ToString());
                    //Response.ContentType = contentType;
                    //var readStream = System.IO.File.ReadAllBytes(filePath);
                    //return File(lFileBinary, contentType, Content[0].FileName);
                    //Response.AppendHeader("Access-Control-Allow-Origin", "*");
                    //Response.AppendHeader("Access-Control-Allow-Methods", "POST, GET, OPTIONS");
                    //Response.AppendHeader("Access-Control-Allow-Headers", "X-PINGOTHER, Content-Type");
                    //return File(lFileBinary, contentType, Content[0].FileName);
                    //return File(lFileBinary, "application/pdf", lFileName);
                    return lFileBinary;

                    //return new FileContentResult(lFileBinary, contentType);

                    //direct attach
                    //Response.Clear();
                    //Response.ClearHeaders();
                    //Response.ClearContent();
                    //Response.AddHeader("Content-Disposition", "attachment; filename=" + Content[0].FileName + "; size=" + lMemoryStream.Length.ToString());
                    //Response.ContentType = contentType;
                    //Response.AddHeader("Content-Type", contentType);
                    //int bufferSize = 65536;
                    //byte[] byteBuffer = new byte[bufferSize];

                    //while (lMemoryStream.Read(byteBuffer, 0, byteBuffer.Length) > 0)
                    //{
                    //    Response.BinaryWrite(lMemoryStream.ToArray());
                    //}

                    //Response.BinaryWrite(lFileBinary);

                    //Response.AddHeader("Content-Disposition", "attachment; filename=\"" + Content[0].FileName + "\"; size=" + lMemoryStream.Length.ToString());
                    //Response.ContentType = contentType;
                    //Response.AddHeader("Content-Type", contentType);

                    //Response.Flush();

                    //try
                    //{

                    //    var LengthToRead = lMemoryStream.Length;
                    //    int bytesToRead = 65536;

                    //    //Indicate the type of data being sent
                    //    Response.ContentType = "application/octet-stream";

                    //    //Name the file 
                    //    Response.AddHeader("Content-Disposition", "attachment; filename=\"Content[0].FileName\"");
                    //    Response.AddHeader("Content-Length", LengthToRead.ToString());

                    //    int length;
                    //    lMemoryStream.Position = 0;
                    //    do
                    //    {
                    //        // Verify that the client is connected.
                    //        if (Response.IsClientConnected)
                    //        {
                    //            byte[] buffer = new Byte[bytesToRead];

                    //            // Read data into the buffer.
                    //            length = lMemoryStream.Read(buffer, 0, bytesToRead);

                    //            // and write it out to the response's output stream
                    //            Response.OutputStream.Write(buffer, 0, length);

                    //            // Flush the data
                    //            Response.Flush();

                    //            //Clear the buffer
                    //            LengthToRead = LengthToRead - length;
                    //        }
                    //        else
                    //        {
                    //            // cancel the download if client has disconnected
                    //            LengthToRead = -1;
                    //        }
                    //    } while (LengthToRead > 0); //Repeat until no data is read

                    //}
                    //finally
                    //{
                    //    if (lMemoryStream != null)
                    //    {
                    //        //Close the input stream                   
                    //        lMemoryStream.Close();
                    //    }
                    //    Response.End();
                    //    Response.Close();
                    //}

                    //return View("Failed");

                    //return null;
                    //return File(lFileBinary, System.Net.Mime.MediaTypeNames.Application.Octet, Content[0].FileName);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Server Error: " + ex.Message);
            }
            //var lReturn = Json(imageDataURL, JsonRequestBehavior.AllowGet);
            //lReturn.MaxJsonLength = 80000000;
            return null;
            //return File(lFileBinary, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

        }



        [System.Web.Http.HttpGet]

        [System.Web.Http.Route("deleteDrawing_wbsPosting/{CustomerCode}/{ProjectCode}/{DrawingID}")]

        public IHttpActionResult deleteDrawing_wbsPosting(string CustomerCode, string ProjectCode, int DrawingID)

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

                    "FROM dbo.OESDrawings_posting " +

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

                        ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

                        ClientContext lClient = O365LogOn();

                        Microsoft.SharePoint.Client.List docLib = lClient.Web.Lists.GetByTitle("Documents");

                        lClient.Load(docLib);

                        lClient.ExecuteQuery();

                        string lCustomerName = getCustomerName(lCustomerCode);

                        string lProjectTitle = getProjectName(lCustomerCode, lProjectCode, "");

                        lCustomerName = removeSpecialChar(lCustomerName);

                        lProjectTitle = removeSpecialChar(lProjectTitle);

                        var lFileUrl = "/sites/DigiOSDocs/Shared Documents/DrawingsLib/" + lCustomerName + "/" + lProjectTitle + "/" + lFileName;

                        if (FileExists(lClient, lFileUrl) == true)

                        {

                            var lFileToDelete = lClient.Web.GetFileByServerRelativeUrl(lFileUrl);

                            lFileToDelete.DeleteObject();

                            lClient.ExecuteQuery();

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

                                lClient.ExecuteQuery();

                            }

                        }

                        lSQL = "DELETE " +

                        "FROM dbo.OESDrawingsOrder_posting " +

                        "WHERE DrawingID = " + DrawingID.ToString() + " " +

                        "AND Revision = " + lRevision.ToString() + " ";

                        lCmd.CommandText = lSQL;

                        lCmd.Connection = lNDSCon;

                        lCmd.CommandTimeout = 300;

                        lCmd.ExecuteNonQuery();


                        if (lRevision == 0)

                        {

                            lSQL = "DELETE " +

                            "FROM dbo.OESDrawingsWBS_posting " +

                            "WHERE DrawingID = " + DrawingID.ToString() + " " +

                            "AND Revision = " + lRevision.ToString() + " ";

                            lCmd.CommandText = lSQL;

                            lCmd.Connection = lNDSCon;

                            lCmd.CommandTimeout = 300;

                            lCmd.ExecuteNonQuery();

                            lSQL = "DELETE " +

                            "FROM dbo.OESDrawings_posting " +

                            "WHERE DrawingID = " + DrawingID + " ";

                            lCmd.CommandText = lSQL;

                            lCmd.Connection = lNDSCon;

                            lCmd.CommandTimeout = 300;

                            lCmd.ExecuteNonQuery();

                        }

                        else

                        {

                            lRevision = lRevision - 1;

                            lSQL = "UPDATE dbo.OESDrawings_posting " +

                            "SET Revision = " + lRevision.ToString() + " " +

                            "WHERE DrawingID = " + DrawingID + " ";

                            lCmd.CommandText = lSQL;

                            lCmd.Connection = lNDSCon;

                            lCmd.CommandTimeout = 300;

                            lCmd.ExecuteNonQuery();

                            lSQL = "UPDATE dbo.OESDrawingsWBS_posting " +

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

            return Ok(lReturn);

        }


        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("uploadDrawingFiles_2")]
        public IHttpActionResult uploadDrawingFiles_2()
        {
            var httpRequest = HttpContext.Current.Request;
            var lCustomerCode = httpRequest.Form["Customer"];
            var lProjectCode = httpRequest.Form["Project"];
            var UserName = httpRequest.Form["UserName"];

            var lDrawingIDList = new List<int>();
            string lSQL = "";
            var lCmd = new SqlCommand();
            var lNDSCon = new SqlConnection();
            var lDa = new SqlDataAdapter();
            var lDs = new DataSet();


            var lUserType = "AD";

            SqlDataReader lRst;
            string lSPFileName = "";
            int lFound = 0;

            var Content = new Drawing_postingModels();
            try
            {
                int lRevision = 0;
                int lDrawingID = 0;
                int lOrderNumber = 0;
                int.TryParse(httpRequest.Form["OrderNumber"], out lOrderNumber);//306095;
                int.TryParse(httpRequest.Form["Revision"], out lRevision);

                var lDrawingNo = httpRequest.Form["DrawingNo"];// "121-3";
                var lRemarks = httpRequest.Form["Remarks"];// "test";
                var lWBS1 = httpRequest.Form["WBS1"];//"100A";
                var lWBS2 = httpRequest.Form["WBS2"];//"1";
                var lWBS3 = httpRequest.Form["WBS3"];//"A";
                var lProdType = httpRequest.Form["ProdType"];//"CAB";
                var lStructureElement = httpRequest.Form["StructureElement"];// "BEAM";
                var lUploadType = httpRequest.Form["UploadType"];//"R";
                var lScheduledProd = httpRequest.Form["ScheduledProd"];//"N";
                var IsDuplicate = httpRequest.Form["IsDuplicate"];//"N";
                var FileSumittedBy = httpRequest.Form["FileSumittedBy"];//"N";

                if (lScheduledProd == null)
                {
                    lScheduledProd = "";
                }
                lScheduledProd = lScheduledProd.Trim();


                if (lScheduledProd == "")
                {
                    lScheduledProd = "N";
                }

                if (httpRequest.Files.Count > 0)
                {
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                    ClientContext lClient = O365LogOn();
                    Microsoft.SharePoint.Client.List docLib = lClient.Web.Lists.GetByTitle("Documents");
                    lClient.Load(docLib);
                    lClient.ExecuteQuery();
                    var lProcessObj = new ProcessController();
                    lProcessObj.OpenNDSConnection(ref lNDSCon);
                    HttpFileCollection files = httpRequest.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        lRevision = 0;
                        HttpPostedFile file = files[i];
                        var lFileName = file.FileName;

                        if (lFileName.LastIndexOf("\\") >= 0)
                        {
                            lFileName = lFileName.Substring(lFileName.LastIndexOf("\\") + 1);
                        }
                        if (lFileName.LastIndexOf("/") >= 0)
                        {
                            lFileName = lFileName.Substring(lFileName.LastIndexOf("/") + 1);
                        }

                        lFileName = System.Web.HttpUtility.HtmlDecode(lFileName);
                        lFileName = removeSpecialChar(lFileName);
                        RegexOptions options = RegexOptions.None;
                        Regex regex = new Regex("[ ]{2,}", options);
                        lFileName = regex.Replace(lFileName, " ");

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
                        "FROM dbo.OESDrawings_posting " +
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
                                lSQL = "UPDATE dbo.OESDrawings_posting " +
                                "SET Revision = " + lRevision.ToString() + ", " +
                                "DrawingNo = '" + lDrawingNo + "', " +
                                "Remarks = '" + lRemarks + "', " +
                                "UpdatedDate = getDate(), " +
                                "UpdatedBy = '" + UserName + "' " +
                                "WHERE CustomerCode = '" + lCustomerCode + "' " +
                                "AND ProjectCode = '" + lProjectCode + "' " +
                                "AND FileName = N'" + lFileName + "' ";
                            }
                            else
                            {
                                lSQL = "UPDATE dbo.OESDrawings_posting " +
                                "SET Revision = " + lRevision.ToString() + ", " +
                                "UpdatedDate = getDate(), " +
                                "UpdatedBy = '" + UserName + "' " +
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
                        string lProjectTitle = getProjectName(lCustomerCode, lProjectCode, UserName);
                        lCustomerName = removeSpecialChar(lCustomerName);
                        lProjectTitle = removeSpecialChar(lProjectTitle);
                        lFileName = removeSpecialChar(lFileName);

                        if (FolderExists(lClient, "/sites/DigiOSDocs/Shared Documents/DrawingsLib/" + lCustomerName) == false)
                        {
                            var fld1 = docLib.RootFolder.Folders.GetByUrl("/sites/DigiOSDocs/Shared Documents/DrawingsLib");
                            //var fld1 = docLib.RootFolder;
                            var fld2 = fld1.Folders.Add(lCustomerName);
                            var fld3 = fld2.Folders.Add(lProjectTitle);
                            fld2.Update();
                            fld3.Update();
                            lClient.ExecuteQuery();
                        }
                        else if (FolderExists(lClient, "/sites/DigiOSDocs/Shared Documents/DrawingsLib/" + lCustomerName + "/" + lProjectTitle) == false)
                        {
                            var fld1 = docLib.RootFolder.Folders.GetByUrl("/sites/DigiOSDocs/Shared Documents/DrawingsLib");
                            var fld2 = fld1.Folders.Add(lCustomerName);
                            var fld3 = fld2.Folders.Add(lProjectTitle);
                            fld3.Update();
                            lClient.ExecuteQuery();
                        }

                        FileCreationInformation createFile = new FileCreationInformation();
                        var lFileURL = gSiteURL + "/Shared Documents/DrawingsLib/" + lCustomerName + "/" + lProjectTitle + "/" + lSPFileName;

                        //createFile.Content = pdfBytes;
                        createFile.ContentStream = file.InputStream;
                        createFile.Url = lFileURL;
                        createFile.Overwrite = true;
                        Microsoft.SharePoint.Client.File newFile = docLib.RootFolder.Files.Add(createFile);


                        newFile.CheckOut();

                        newFile.ListItemAllFields.Update();
                        newFile.CheckIn(User.Identity.Name, CheckinType.MajorCheckIn);
                        lClient.ExecuteQuery();

                        //register
                        Content = db.Drawings_posting.Find(lCustomerCode, lProjectCode, lFileName);
                        if (Content == null || Content.DrawingID == 0)
                        {
                            Content = new Drawing_postingModels
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
                                UpdatedBy = UserName//User.Identity.Name
                            };
                            db.Drawings_posting.Add(Content);
                            db.SaveChanges();

                            Content = db.Drawings_posting.Find(lCustomerCode, lProjectCode, lFileName);
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
                            "FROM dbo.OESDrawingsWBS_posting " +
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
                                lSQL = "INSERT INTO dbo.OESDrawingsWBS_posting " +
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
                                ",'" + UserName + "') ";

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
                            "FROM dbo.OESDrawingsOrder_posting " +
                            "WHERE DrawingID = " + Content.DrawingID.ToString() + " " +
                            "AND Revision = " + lRevision + " " +
                            "AND WBSElementId = " + lOrderNumber + " " +
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
                                var DetailerSubmissionCount = 0;
                                var CustomerSubmissionCount = 0;                               
                                var maxCount = 0;
                                var type = FileSumittedBy == "Detailer" ? "DetailerSubmissionCount" : "CustomerSubmissionCount";
                                if (IsDuplicate== "1")
                                {

                                    string SQLquery = "select Max(" + type + ") as MaxCount from OESDrawingsOrder_Posting where WBSElementId=" + lOrderNumber ;
                                   
                                    lCmd.CommandText = SQLquery;
                                    lCmd.Connection = lNDSCon;
                                    lCmd.CommandTimeout = 300;
                                    lRst = lCmd.ExecuteReader();
                                    if (lRst.HasRows)
                                    {
                                        while (lRst.Read())
                                        {
                                            // Get the value from the column
                                            maxCount = lRst["MaxCount"] != DBNull.Value ? Convert.ToInt32(lRst["MaxCount"]) : 0;
                                          
                                        }
                                    }

                                }
                                lRst.Close();
                                maxCount += 1;

                                lSQL = "INSERT INTO dbo.OESDrawingsOrder_posting " +
                                "(DrawingID " +
                                ", Revision " +
                                ", WBSElementId" +
                                ", ProductType " +
                                ", StructureElement " +
                                ", ScheduledProd " +
                                ", UpdatedDate " +
                                ", FileSubmitedBy " +
                                ", Remark " +
                                "," + type +
                                ", UpdatedBy) " +
                                "VALUES " +
                                "(" + Content.DrawingID.ToString() + " " +
                                ", " + lRevision + " " +
                                ", " + lOrderNumber + " " +
                                ",'" + lProdType + "' " +
                                ",'" + lStructureElement + "' " +
                                ",'" + lScheduledProd + "' " +
                                ",getDate() " +
                                 ",'" + FileSumittedBy + "' " +
                                 ",'" + lRemarks + "' " +
                                  ",'" + maxCount + "' " +
                                ",'" + UserName + "') ";
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

                            lSQL = "UPDATE dbo.OESDrawingsWBS_posting " +
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
    "when dbo.OESDrawingsWBS_posting.ProductType = 'MESH' then 'MSH' " +
    "when dbo.OESDrawingsWBS_posting.ProductType = 'CUT-TO-SIZE-MESH' then 'MSH' " +
    "when dbo.OESDrawingsWBS_posting.ProductType = 'STIRRUP-LINK-MESH' then 'MSH' " +
    "when dbo.OESDrawingsWBS_posting.ProductType = 'COLUMN-LINK-MESH' then 'MSH' " +
    "when dbo.OESDrawingsWBS_posting.ProductType = 'PRE-CAGE' then 'PRC' " +
    "when dbo.OESDrawingsWBS_posting.ProductType = 'CORE-CAGE' then 'CAR' " +
    "when dbo.OESDrawingsWBS_posting.ProductType = 'CARPET' then 'CAR' " +
    "else dbo.OESDrawingsWBS_posting.ProductType END) " +
    "AND S.vchStructureElementType = dbo.OESDrawingsWBS_posting.StructureElement " +
    "AND W.vchWBS1 = dbo.OESDrawingsWBS_posting.WBS1 " +
    "AND W.vchWBS2 = dbo.OESDrawingsWBS_posting.WBS2 " +
    "AND W.vchWBS3 = dbo.OESDrawingsWBS_posting.WBS3) ";

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


            var lSaved = (from p in db.Drawings
                          where p.CustomerCode == lCustomerCode &&
                          p.ProjectCode == lProjectCode &&
                          lDrawingIDList.Contains(p.DrawingID)
                          orderby p.DrawingID
                          select p).ToList();

            var lReturn = lDrawingIDList;

            return Ok(lReturn);
        }



    }
}