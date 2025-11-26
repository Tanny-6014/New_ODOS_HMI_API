using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNet.Identity;
using System.Data.SqlClient;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.IO;

using System.Globalization;
using System.Data.OleDb;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using NCalc;
using System.Text;
using Oracle.ManagedDataAccess.Client;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using OrderService.Controllers;
using OrderService.Models;
using OrderService.Repositories;
using System.Net.Http.Headers;


namespace OrderService.Controllers
{
  
    public class CarpetController : Controller
    {
        public string gUserType = "";
        public string gGroupName = "";

        struct struOrderList
        {
            public string OrderNo;
            public string OrderDesc;
        };

        private readonly IWebHostEnvironment _env;

        public CarpetController(IWebHostEnvironment env)
        {
            _env = env;
        }



        [HttpGet]
        [Route("/getProjectDetails_Carpet/{CustomerCode}/{ProjectCode}/{UserName}")]
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
                "MaxBarLength, " +
                "OrderSubmission, " +
                "OrderCreation " +
                "FROM dbo.OESProject P, dbo.OESUserAccess U " +
                "WHERE P.CustomerCode = U.CustomerCode " +
                "AND P.ProjectCode = U.ProjectCode " +
                "AND U.UserName = '" + UserName + "' " +
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
                "MaxBarLength, " +
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
                "MaxBarLength, " +
                "'No' as OrderSubmission, " +
                "'No' as OrderCreation " +
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
                            MaxBarLength = lRst.GetValue(27) == DBNull.Value ? 12000 : lRst.GetInt32(27),
                            OrderSubmission = lRst.GetValue(28) == DBNull.Value ? "No" : lRst.GetString(28).Trim(),
                            OrderCreation = lRst.GetValue(29) == DBNull.Value ? "No" : lRst.GetString(29).Trim()
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
                        goto L1;
                    }
                }

                lCmd.CommandText =
                "SELECT isNull(MAX(JobID), 0) FROM dbo.OESCTSMESHJobAdvice " +
                "WHERE CustomerCode = '" + CustomerCode + "' " +
                "AND ProjectCode = '" + ProjectCode + "' " +
                "AND OrderStatus <> 'New' " +
                "AND OrderStatus <> 'Created' " +
                "AND (SiteEngr_Name > '' " +
                "OR Scheduler_Name > '' " +
                "OR DeliveryAddress > '') ";

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
                        "WHERE CustomerCode = '" + CustomerCode + "' " +
                        "AND ProjectCode = '" + ProjectCode + "' " +
                        "AND OrderStatus <> 'New' " +
                        "AND OrderStatus <> 'Created' " +
                        "AND UpdateBy = '" + UserName + "' " +
                        "AND(SiteEngr_Name > '' " +
                        "OR Scheduler_Name > '' " +
                        "OR DeliveryAddress > '') ";

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
                        "WHERE CustomerCode = '" + CustomerCode + "' " +
                        "AND ProjectCode = '" + ProjectCode + "' " +
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
                        "FROM dbo.OESCTSMESHJobAdvice " +
                        "WHERE CustomerCode = '" + CustomerCode + "' " +
                        "AND ProjectCode = '" + ProjectCode + "' " +
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
                        "WHERE CustomerCode = '" + CustomerCode + "' " +
                        "AND ProjectCode = '" + ProjectCode + "' " +
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

            lProcessObj = null;

            //return Json(content1, JsonRequestBehavior.AllowGet);
            return Ok(content1);
        }


        //get Column Link Cage List 
        [HttpGet]
        [Route("/getCarpetDetailsNSH/{CustomerCode}/{ProjectCode}/{PostID}")]
        public ActionResult getCarpetDetailsNSH(string CustomerCode, string ProjectCode, int PostID)
        {
            var lBeamDetails = new List<CTSShapeOthersDetailsModels>();
            var lNewBeam = new CTSShapeOthersDetailsModels();

            var lCmd = new SqlCommand();
            var lNDSCon = new SqlConnection();
            SqlDataReader lRst;
            int lID = 0;
            string lPar = "";

            if (PostID > 0)
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
                "FROM dbo.CarpetStructureMarkingDetails S, " +
                "dbo.MeshProductMarkingDetails P, " +
                "dbo.SELevelDetails D, " +
                "dbo.PostGroupMarkingDetails G, " +
                "dbo.ProductCodeMaster M " +
                "WHERE S.intStructureMarkId = P.intCarpetStructureMarkID " +
                "AND S.intSEDetailingId = D.intSEDetailingId " +
                "AND D.intGroupMarkId = G.intGroupMarkId " +
                "AND M.intProductCodeId = P.intProductCodeId " +
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
                            lPar = lRst.GetValue(10) == DBNull.Value ? "" : lRst.GetString(10).Trim();
                            lNewBeam = new CTSShapeOthersDetailsModels
                            {
                                CustomerCode = CustomerCode,
                                ProjectCode = ProjectCode,
                                JobID = 0,
                                BBSID = 1,
                                MeshID = lID,
                                MeshSort = lID * 100,
                                MeshMark = lRst.GetString(0).Trim(),
                                MeshMainLen = (int)Math.Round(lRst.GetDecimal(1), 0),
                                MeshCrossLen = (int)Math.Round(lRst.GetDecimal(2), 0),
                                MeshProduct = lRst.GetString(3).Trim(),
                                MeshShapeCode = lRst.GetValue(4) == DBNull.Value ? "" : lRst.GetString(4).Trim(),
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
                                MeshShapeParameters = lRst.GetValue(12) == DBNull.Value ? "" : lRst.GetString(12).ToUpper(),

                                ProdMWDia = lRst.GetValue(13) == DBNull.Value ? 0 : lRst.GetDecimal(13),
                                ProdMWSpacing = lRst.GetValue(14) == DBNull.Value ? 0 : lRst.GetInt32(14),
                                ProdCWDia = lRst.GetValue(15) == DBNull.Value ? 0 : lRst.GetDecimal(15),
                                ProdCWSpacing = lRst.GetValue(16) == DBNull.Value ? 0 : lRst.GetInt32(16),
                                ProdMass = lRst.GetValue(17) == DBNull.Value ? 0 : lRst.GetDecimal(17),
                                ProdTwinInd = lRst.GetValue(18) == DBNull.Value ? "" : lRst.GetString(18).Trim()
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

            //return Json(lBeamDetails, JsonRequestBehavior.AllowGet);
            return Ok(lBeamDetails);
        }

        // Search parameter value from string like "A:810;B:240;C810;Leg:110;Hook:60"
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

        //get Shape information 
        [HttpGet]
        [Route("/getShapeInfo_Carpet/{CustomerCode}/{ProjectCode}/{JobID}/{ShapeCode}")]
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

                try
                {
                    //string lFullName = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Content/images/Shapes/"), lShapeFileName);
                    //string lFullName = Path.Combine("C:\\inetpub\\wwwroot\\ODOSMicroServices\\OrderService\\images\\" + lShapeFileName);
                    //System.IO.FileStream fs = System.IO.File.OpenRead(lFullName);
                    //byte[] lImage = new byte[fs.Length];
                    //int lByteCT = fs.Read(lImage, 0, lImage.Length);

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
                        MeshShapeImage = null,
                        MeshCreepMO1 = lCreepMO1,
                        MeshCreepCO1 = lCreepCO1,
                    };
                }
                catch (Exception ex)
                {
                    var lErroeMsg = ex.Message;
                }
            }
            return Json(lShapeRecord);
        }

        //getShapeImagesByCat
        [HttpGet]
        [Route("/getShapeImagesByCarpet/{ShapeCategory}")]
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
                    if (ShapeCategory == "CARPET")
                    {
                        lCmd.CommandText =
                        "SELECT chrShapeCode, vchImage " +
                        "FROM dbo.ShapeMaster " +
                        "WHERE vchMeshShapeGroup = 'A1' " +
                        "OR chrShapeCode = 'N' ";
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
                            string lFullName = Path.Combine("C:\\inetpub\\wwwroot\\ODOSMicroServices\\OrderService\\images\\", lShapeFileName);

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
        [Route("/getProductListCarpet/{ProductCategory}")]
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
            string lDesc = "";

            List<Dictionary<string, object>> lReturn = new List<Dictionary<string, object>>();
            Dictionary<string, object> lDic = new Dictionary<string, object>();

            if (ProductCategory != null)
            {
                var lProcessObj = new ProcessController();
                if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
                {
                    if (ProductCategory == "CARPET")
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
                        ",vchProductDescription " +
                        "FROM dbo.ProductCodeMaster " +
                        "WHERE vchProductCode like 'C%' " +
                        "AND intmaterialTypeID = 11 " +
                        "AND tntStatusId = 1 " +
                        "ORDER BY decMWLength Desc, intShapeCodeID, decMWDiameter, intMWSpace ";
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
                        ",vchProductDescription" +
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
                            lDesc = lRst.GetValue(7) == DBNull.Value ? "" : lRst.GetString(7).Trim();

                            lDic = new Dictionary<string, object>();

                            lDic.Add("ProductCode", lProdCode);
                            lDic.Add("ProdMWDia", lMWDia);
                            lDic.Add("ProdMWSpacing", lMWSpacing);
                            lDic.Add("ProdCWDia", lCWDia);
                            lDic.Add("ProdCWSpacing", lCWSpacing);
                            lDic.Add("ProdMass", lMass);
                            lDic.Add("ProdTwinInd", lTwinInd);
                            lDic.Add("ProdDesc", lDesc);

                            lReturn.Add(lDic);
                        }
                    }
                    lRst.Close();

                    lProcessObj.CloseNDSConnection(ref lNDSCon);
                }

                lProcessObj = null;
            }


            return Ok(lReturn);
        }


        //Print Shapes List 
        [HttpGet]
        [Route("/printShapesCarpet/{ShapeCategory}")]
        public ActionResult printShapes(string ShapeCategory)
        {
            byte[] lbPDF = new byte[] { };
            Reports service = new Reports();
            var bPDF = service.rptMeshShapes(ShapeCategory);
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
            //return new JsonResult()
            //{
            //    Data = bPDF,
            //    MaxJsonLength = Int32.MaxValue,
            //    JsonRequestBehavior = JsonRequestBehavior.AllowGet
            //};
            return Ok(lbPDF);
        }

        //Print Shapes List 
        [HttpGet]
        [Route("/printProductCarpet/{ProductCategory}")]
        public ActionResult printProduct(string ProductCategory)
        {
            byte[] lbPDF = new byte[] { };
            Reports service = new Reports();
            var bPDF = service.rptMeshProduct(ProductCategory);
            string fileName = "abc.pdf";
            using (MemoryStream memoryStream = new MemoryStream(bPDF))
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StreamContent(memoryStream);
                response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                response.Content.Headers.ContentDisposition.FileName = fileName;
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

                return File(response.Content.ReadAsByteArrayAsync().Result, "application/pdf");
            }
            return Ok(lbPDF);
            //return new JsonResult()
            //{
            //    Data = bPDF,
            //    MaxJsonLength = Int32.MaxValue,
            //    JsonRequestBehavior = JsonRequestBehavior.AllowGet
            //};
        }
    }
}
