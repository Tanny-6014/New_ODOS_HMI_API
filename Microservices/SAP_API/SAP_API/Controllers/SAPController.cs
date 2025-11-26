using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Net;
using SAP.Middleware.Connector;
using SAP_API.SAPIntegration;
using System.Data;
using SAP_API.Models;
using System.Data.SqlClient;
using SAPConnector;
using Oracle.ManagedDataAccess.Client;
using System.ServiceModel;
using SAP_API.Modelss;
using Microsoft.AspNet.Identity;
using System.ServiceModel.Channels;
using System.Threading;
using System.Web.Mvc;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using NSAPConnector;

namespace SAP_API.Controllers
{
    [System.Web.Http.RoutePrefix("api/SAPAPI")]
    [ExceptionHandler]
    public class SAPController : ApiController
    {
        public const string SHIP_TO_PARTY_PARTNER_CODE = "('WE','S1','S2','S3','S4','S5','S6','S7','S8','S9','T1','T2','T3','T4','T5','T6','T7','T8','T9','U1')";
        public const string GET_SOR_NO_FROM_SAP = "N";
        //public string strServer = "DEV";
        //public string strClient = "600";


        public string strServer = "DEV";

        ///   public string strServer = "PRD";


        public string strClient = "600";

        static string strCOCode = "0640";
        public string strSalesOrg = "6401";
        public string strSalesExport = "6402";

        private string strSalesDocType = "ZCRO";
        private string strProdType = "BPC";

        OracleConnection cnCIS = new OracleConnection();
        OracleConnection cnIDB = new OracleConnection();
        SqlConnection cnNDS = new SqlConnection();

        SqlTransaction osqlTransNDS;
        OracleTransaction oracleTransIDB;
        OracleTransaction oracleTransCIS;

        private string strCIS_Connection = "";
        private string strNDS_Connection = "";
        private string strIDB_Connection = "";
        private string strSTS_Connection = "";
        private string strNGW_Connection = "";

        private string sCustomerCode = "";
        private string sProjectCode = "";
        private string sContractNo = "";
        private string sPONo = "";
        private string sPODate = "";

        private bool chkUrgOrd = false;
        private bool chkPremiumSvc = false;
        private bool chkCraneBooked = false;
        private bool chkBargeBooked = false;
        private bool chkPoliceEscort = false;
        private bool chkOnHoldInd = false; //
        private bool chkZeroTolInd = false;
        private bool chkCallBefDel = false;
        private bool chkConquasInd = false;
        private bool chkSpecialPassInd = false;
        private bool chkLorryCrane = false; //
        private bool chkLowBedVeh = false;
        private bool chk50TonVeh = false;
        private bool chkDoNotMix = false;
        private bool chkFabricateESM = false;
        private bool chkUnitMode = false; //
        private bool chkCarpet = false; //
        private string blankCouplerTyp = "N"; //

        private string strIntRemark = "";
        private string strExtRemark = "";
        private string strInvRemark = "";
        private string strCoupler = ""; //
        private string sUserID = "";
        private string sSubmittedBy = "";
        private string strMastReqDate = "";
        private string strMastReqDateFrm = "";
        private string strMastReqDateTo = "";
        private Double strEstTon = 0;

        private string strWBS1 = "";
        private string strWBS2 = "";
        private string strWBS3 = "";
        private string strWBS4 = " ";
        private string strWBS5 = " ";
        private string strPrjStage = "";

        private string strCABFormer = "";
        private string strVehicleType = "";

        private string strStruElement = " ";
        private string strBBSNo = "";
        private string strBBSDesc = "";

        private string couplerType = "0";

        private string strSAPMCode = "";

        private string strOrderSource = "";
        private string strVariousBar = " ";

        private List<int> sPostProjectID;
        private List<int> sPostWBSElementID;
        private List<int> sPostGroupMarkingID;
        private List<string> sPostBBSNo;
        private List<string> sPostSORNo;

        private DBContextModels db = new DBContextModels();
        SAPRFCConnector _mrSAPConnector = new SAPRFCConnector();
        string message;


        /// <summary>
        /// Get BatchDetails
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("GetBatchDetails")]
        public HttpResponseMessage GetBatchDetails(string matl, string batch)
        {
            _mrSAPConnector.GetConnection();
            IRfcFunction getDataSAP = SAPDATA.rfcRepository.CreateFunction("YMQMFM_QC01_BATCH_VALUES_READ");
            getDataSAP.SetValue("I_VAL_MATNR", matl);
            getDataSAP.SetValue("I_VAL_WERKS", "6400");
            getDataSAP.SetValue("I_VAL_CHARGE", batch);

            try
            {
                getDataSAP.Invoke(SAPDATA.rfcDestination);
            }
            catch (Exception ex)
            {
                message = "Record not exist ";
                message += ex.Message;
                return Request.CreateResponse(HttpStatusCode.NotFound, message);
            }

            IRfcTable IT_KNA1 = getDataSAP.GetTable("T_VAL_TAB");
            SAPResponse dsUniversal = new SAPResponse();
            DataTable dtKNA1 = dsUniversal.Tables["BatchDetails"];
            dtKNA1 = CreateDataTable(dtKNA1, IT_KNA1);

            //IRfcTable IT_KNA2 = getDataSAP.GetTable("T_CHAR_TAB");
            //SAPResponse dsUniversa2 = new SAPResponse();
            //DataTable dtKNA2 = dsUniversa2.Tables["DataTable1"];
            //dtKNA2 = CreateDataTable(dtKNA2, IT_KNA2);


            //IRfcTable IT_KNA3 = getDataSAP.GetTable("T_ATT_TAB");
            //SAPResponse dsUniversa3 = new SAPResponse();
            //DataTable dtKNA3 = dsUniversa3.Tables["BatchDetails"];
            //dtKNA3 = CreateDataTable(dtKNA3, IT_KNA3);


            List<Batch> mrs = new List<Batch>();
            Batch mr = new Batch();
            mr.BatchNumber = batch;
            mr.MATERIAL = matl;
            for (int i = 0; i < dtKNA1.Rows.Count; i++)
            {
                if (dtKNA1.Rows[i][1].ToString() == "Z_HEAT_NO")
                {
                    mr.HEATNO = dtKNA1.Rows[i][4].ToString();
                }
                else if (dtKNA1.Rows[i][1].ToString() == "Z_SHIFT")
                {
                    mr.Shift = dtKNA1.Rows[i][4].ToString();
                }
                else if (dtKNA1.Rows[i][1].ToString() == "Z_PROD_DATE")
                {
                    mr.Production_Date = dtKNA1.Rows[i][4].ToString();
                }
                else if (dtKNA1.Rows[i][1].ToString() == "Z_STANDARD")
                {
                    mr.STANDARD = dtKNA1.Rows[i][4].ToString();
                }
                else if (dtKNA1.Rows[i][1].ToString() == "Z_KG_PER_COIL")
                {
                    mr.KGPERCOIL = dtKNA1.Rows[i][4].ToString();
                }
                else if (dtKNA1.Rows[i][1].ToString() == "Z_MILL_SOURCE")
                {
                    mr.MILLSOURCE = dtKNA1.Rows[i][4].ToString();
                }

            }
             mrs.Add(mr);

            if (mrs == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Record not exist");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, mrs);
            }
        }

        //CAB Item CancelProcess
        //CancelProcess
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("CancelProcess")]
        //[Route("/CancelProcess/{CustomerCode}/{ProjectCode}/{ContractNo}/{JobID}/{StructureElement}/{ProdType}/{OrderSource}/{ScheduledProd}/{ActionType}")]
        public async Task<HttpResponseMessage> CancelProcessAsync(string CustomerCode,
            string ProjectCode,
            string ContractNo,
            int JobID,
            string StructureElement,
            string ProdType,
            string OrderSource,
            string ScheduledProd,
            string ActionType,
            string UserName)
        {
            bool lReturn = true;
            string lSQL = "";
            SqlCommand lCmd;
            SqlDataReader lRst;
            OracleDataReader lOrRst;

            int lGroupMarkingID = 0;
            int lSELevelID = 0;
            int lProjectID = 0;
            int lWBSElementID = 0;
            string lWBS1 = "";
            string lWBS2 = "";
            string lWBS3 = "";
            string lErrorMsg = "";

            string lSORs = "";
            string lGMs = "";
            int lOrderNumber = 0;
            int lPostHeaderID = 0;
            string lBBSNo = "";
            string lCurrentStatus = "";
            string lSAPSO = "";

            sUserID = UserName;//"jagdishH_ttl@natsteel.com.sg";//User.Identity.GetUserName().ToUpper();
            if (sUserID.IndexOf("@") > 0)
            {
                sUserID = sUserID.Substring(0, sUserID.IndexOf("@"));
            }

            if (CustomerCode != null && ProjectCode != null && ContractNo != null && ProdType != null && JobID > 0)
            {
                try
                {
                    //1. get Jobadvice ID or PostID if UX
                    //2. delete from CABProductMarking
                    //3. delete from CABACC_ProductMarking
                    //4. delete BBS 
                    //5. delete group marking

                    if (OrderSource == "UX" || OrderSource == "UXE" || OrderSource == "OL")
                    {
                        lOrderNumber = JobID;
                        var lHeader = db.OrderProject.Find(lOrderNumber);
                        var lSE = db.OrderProjectSE.Find(lOrderNumber, StructureElement, ProdType, ScheduledProd);
                        if (lHeader != null && lSE != null)
                        {
                            lCurrentStatus = lHeader.OrderStatus;
                            if (ScheduledProd != null && ScheduledProd == "Y")
                            {
                                lPostHeaderID = (int)(lSE.PostHeaderID == null ? 0 : lSE.PostHeaderID);

                                if (lPostHeaderID == 0)
                                {
                                    lErrorMsg = "Invalid posting header number. Please contact IT for the issue.";
                                    lReturn = false;
                                }
                            }
                            else
                            {
                                if (ProdType == "CAB")
                                {
                                    JobID = lSE.CABJobID;
                                }
                                else if (ProdType == "STANDARD-BAR")
                                {
                                    JobID = lSE.StdBarsJobID;
                                }
                                else if (ProdType == "STANDARD-MESH")
                                {
                                    JobID = lSE.StdMESHJobID;
                                }
                                else if (ProdType == "COIL" || ProdType == "COUPLER")
                                {
                                    JobID = lSE.CoilProdJobID;
                                }
                                else if (ProdType == "BPC")
                                {
                                    JobID = lSE.BPCJobID;
                                }
                                else if (ProdType == "STIRRUP-LINK-MESH" || ProdType == "COLUMN-LINK-MESH" || ProdType == "CUT-TO-SIZE-MESH")
                                {
                                    JobID = lSE.MESHJobID;
                                }
                                else if (ProdType == "PRE-CAGE" || ProdType == "CORE-CAGE")
                                {
                                    JobID = lSE.CageJobID;
                                }
                                else if (ProdType == "CARPET")
                                {
                                    JobID = lSE.CarpetJobID;
                                }
                                else
                                {
                                    JobID = 16900;//lSE.MESHJobID;//lSE.MESHJobID ajit
                                }
                            }

                            if (lSE.OrderStatus != "Reviewed" && lSE.OrderStatus != "Production")
                            {
                                lErrorMsg = "The order status had been changed, please refresh the list.";
                                lReturn = false;
                            }
                        }
                        else
                        {
                            lErrorMsg = "Invalid format of order detected. Please contact IT to resove the issue.";
                            lReturn = false;
                        }
                    }

                    if (lReturn == true)
                    {
                        //Process for scheduled order
                        //if (ScheduledProd != null && ScheduledProd == "Y" || ProdType == "PRE-CAGE" || ProdType == "STIRRUP-LINK-MESH" || ProdType == "COLUMN-LINK-MESH" || ProdType == "CUT-TO-SIZE-MESH")
                        if (ScheduledProd != null && ScheduledProd == "Y")
                        {
                            var lHeader = db.OrderProject.Find(lOrderNumber);
                            var lSE = db.OrderProjectSE.Find(lOrderNumber, StructureElement, ProdType, ScheduledProd);
                            if (lHeader != null && lSE != null)
                            {
                                lPostHeaderID = (int)(lSE.PostHeaderID == null ? 0 : lSE.PostHeaderID);
                                string lSOR = lSE.SAPSOR;
                                if (lPostHeaderID > 0 && lSOR != null && lSOR != "")
                                {
                                    lSAPSO = lSAPSO + lSOR;
                                    OpenNDSConnection(ref cnNDS);
                                    OpenIDBConnection(ref cnIDB);
                                    OpenCISConnection(ref cnCIS);
                                    if (cnNDS.State != ConnectionState.Open || cnIDB.State != ConnectionState.Open || cnCIS.State != ConnectionState.Open)
                                    {
                                        lErrorMsg = "Cannot open database error.";
                                        lReturn = false;
                                    }
                                    else
                                    {
                                        //get project id
                                        lProjectID = 0;
                                        lSQL = "SELECT P.intProjectId FROM dbo.SAPProjectMaster P WHERE P.vchProjectCode = '" + ProjectCode + "' ";


                                        lCmd = new SqlCommand(lSQL, cnNDS);
                                        lCmd.CommandTimeout = 1200;
                                        lRst = lCmd.ExecuteReader();
                                        if (lRst.HasRows)
                                        {
                                            lRst.Read();
                                            lProjectID = lRst.GetInt32(0);
                                        }
                                        lRst.Close();

                                        #region check release status and BBS Number and WBS Element ID
                                        var lStatus = 0;
                                        lWBSElementID = 0;

                                        lSQL = "SELECT R.tntStatusId, P.intWBSElementID, P.vchBBSNo " +
                                        "FROM dbo.BBSReleaseDetails R, " +
                                        "dbo.BBSPostHeader P " +
                                        "WHERE R.intPostHeaderid = P.intPostHeaderId " +
                                        //"AND P.intProjectId = " + lProjectID.ToString() + " " +
                                        "AND P.intPostHeaderId = " + lPostHeaderID.ToString() + " ";

                                        lCmd = new SqlCommand(lSQL, cnNDS);
                                        lCmd.CommandTimeout = 1200;
                                        lRst = lCmd.ExecuteReader();
                                        if (lRst.HasRows)
                                        {
                                            lRst.Read();
                                            lStatus = lRst.GetByte(0);
                                            lWBSElementID = lRst.GetInt32(1);
                                            lBBSNo = lRst.GetString(2);
                                        }
                                        lRst.Close();
                                        #endregion

                                        #region Cancel order in NDS and Change interface status in NDS
                                        if (lReturn == true)
                                        {
                                            if (lStatus == 12)  //Released 12
                                            {
                                                if (ProdType == "CAB")
                                                {
                                                    //Cancel Released SOR
                                                    lSQL = "dbo.BBSReleaseCAB_Insert ";
                                                    lCmd = new SqlCommand(lSQL, cnNDS);
                                                    lCmd.Transaction = osqlTransNDS;
                                                    lCmd.CommandTimeout = 1200;
                                                    lCmd.CommandType = CommandType.StoredProcedure;
                                                    lCmd.Parameters.Add("@ORD_REQ_NO", SqlDbType.Char);
                                                    lCmd.Parameters.Add("@IntProjectID", SqlDbType.Int);
                                                    lCmd.Parameters.Add("@IntWBSElementID", SqlDbType.Int);
                                                    lCmd.Parameters.Add("@BBS_NO", SqlDbType.Char);
                                                    lCmd.Parameters.Add("@chrBBSStatus", SqlDbType.Char);
                                                    lCmd.Parameters.Add("@UserID", SqlDbType.Int);

                                                    lCmd.Parameters["@ORD_REQ_NO"].Value = lSOR;
                                                    lCmd.Parameters["@IntProjectID"].Value = lProjectID;
                                                    lCmd.Parameters["@IntWBSElementID"].Value = lWBSElementID;
                                                    lCmd.Parameters["@BBS_NO"].Value = lBBSNo;
                                                    lCmd.Parameters["@chrBBSStatus"].Value = "C";
                                                    lCmd.Parameters["@UserID"].Value = 11;
                                                    lCmd.CommandTimeout = 1200;
                                                    lCmd.ExecuteNonQuery();
                                                }
                                                else
                                                {
                                                    //lSQL = "BBSReleaseBySOR_Insert ";
                                                    //lCmd = new SqlCommand(lSQL, cnNDS);
                                                    //lCmd.Transaction = osqlTransNDS;
                                                    //lCmd.CommandTimeout = 1200;
                                                    //lCmd.CommandType = CommandType.StoredProcedure;
                                                    //lCmd.Parameters.Add("@ORD_REQ_NO", SqlDbType.Char);
                                                    //lCmd.Parameters.Add("@IntProjectID", SqlDbType.Int);
                                                    //lCmd.Parameters.Add("@IntWBSElementID", SqlDbType.Int);
                                                    //lCmd.Parameters.Add("@BBS_NO", SqlDbType.Char);
                                                    //lCmd.Parameters.Add("@chrBBSStatus", SqlDbType.Char);
                                                    //lCmd.Parameters.Add("@UserID", SqlDbType.Int);

                                                    //lCmd.Parameters["@ORD_REQ_NO"].Value = lSOR;
                                                    //lCmd.Parameters["@IntProjectID"].Value = lProjectID;
                                                    //lCmd.Parameters["@IntWBSElementID"].Value = lWBSElementID;
                                                    //lCmd.Parameters["@BBS_NO"].Value = "&nbsp;"; //lBBSNo;
                                                    //lCmd.Parameters["@chrBBSStatus"].Value = "C";
                                                    //lCmd.Parameters["@UserID"].Value = 11;
                                                    //lCmd.CommandTimeout = 1200;
                                                    //lCmd.ExecuteNonQuery();

                                                    //Back to Posted
                                                    lSQL = "UPDATE dbo.SAP_WBS_SOR " +
                                                        "SET STATUS = 'Q', " +
                                                        "Error_log = 'ORDER CANCELLED', " +
                                                        "RELEASE_BY = 'DigiOS' " +
                                                        "WHERE ORD_REQ_NO = '" + lSOR + "' ";

                                                    lCmd = new SqlCommand(lSQL, cnNDS);
                                                    lCmd.CommandTimeout = 1200;
                                                    lCmd.Transaction = osqlTransNDS;
                                                    lCmd.ExecuteNonQuery();

                                                    lSQL = "DELETE FROM dbo.BBSReleaseDetails WHERE intPostHeaderid = " + lPostHeaderID.ToString() + " ";

                                                    lCmd = new SqlCommand(lSQL, cnNDS);
                                                    lCmd.CommandTimeout = 1200;
                                                    lCmd.Transaction = osqlTransNDS;
                                                    lCmd.ExecuteNonQuery();

                                                    //Unpost it
                                                    // var ndsClient = new NDSPosting.BBSPostingServiceClient();ajit
                                                    var binding = new CustomBinding();
                                                    var myTransport = new HttpTransportBindingElement();
                                                    var muEncoding = new BinaryMessageEncodingBindingElement();
                                                    EndpointAddress address;

                                                    binding = new CustomBinding();
                                                    //binding = new BasicHttpBinding();
                                                    binding.Name = "CustomBinding_IBBSPostingService";
                                                    binding.Elements.Add(muEncoding);
                                                    binding.Elements.Add(myTransport);

                                                    binding.OpenTimeout = new TimeSpan(0, 10, 0);
                                                    binding.CloseTimeout = new TimeSpan(0, 10, 0);
                                                    binding.SendTimeout = new TimeSpan(0, 10, 0);
                                                    binding.ReceiveTimeout = new TimeSpan(0, 10, 0);

                                                    //Specif (y the address to be used for the client.
                                                    //UAT server
                                                    if (strServer == "PRD")
                                                    {
                                                        //Production Server
                                                        address = new EndpointAddress("http://172.25.1.141:81/NDSWCF_PV/BBSPostingService.svc");
                                                    }
                                                    else
                                                    {
                                                        //address = new EndpointAddress("http://172.25.1.224:81/NDSWCF_CAB/BBSPostingService.svc");
                                                        address = new EndpointAddress("http://172.25.1.224:81/NDSWCF_CUBE/BBSPostingService.svc");
                                                    }

                                                    //Create a client that is configured with this address and binding.
                                                    // ndsClient = new NDSPosting.BBSPostingServiceClient(binding, address);commented ajit

                                                    // ndsClient.Open();
                                                    //  ndsClient.UnPostBBS(lPostHeaderID.ToString(), out lErrorMsg);

                                                    using (var client = new HttpClient())
                                                    {
                                                        try
                                                        {
                                                            // Define the API endpoint URL
                                                            string apiUrl = "https://localhost:5006/UnPostBBS_Update/" + lPostHeaderID; // Replace with your API URL

                                                            // Send an HTTP GET request
                                                            HttpResponseMessage response = await client.GetAsync(apiUrl);

                                                            // Check if the request was successful
                                                            if (response.IsSuccessStatusCode)
                                                            {
                                                                // Read the response content as a string
                                                                string responseContent = await response.Content.ReadAsStringAsync();

                                                                // Now you can work with the response content (e.g., parse JSON, process data, etc.)
                                                                Console.WriteLine(responseContent);
                                                            }
                                                            else
                                                            {
                                                                Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                                                            }
                                                        }
                                                        catch (HttpRequestException ex)
                                                        {
                                                            Console.WriteLine($"HTTP request error: {ex.Message}");
                                                        }
                                                    }
                                                    // ndsClient.Close();ajit

                                                }
                                            }

                                            
                                            lSQL = "UPDATE SAP_REQUEST_DETAILS SET STATUS = 'X' " +
                                                    "WHERE ORD_REQ_NO = '" + lSOR + "' ";

                                            lCmd = new SqlCommand(lSQL, cnNDS);
                                            lCmd.CommandTimeout = 1200;
                                            lCmd.ExecuteNonQuery();

                                            //delete from request header
                                            lSQL = "UPDATE SAP_REQUEST_HDR SET STATUS = 'X', PO_NUM = PO_NUM + '-CXL' " +
                                            "WHERE ORD_REQ_NO = '" + lSOR + "' ";

                                            lCmd = new SqlCommand(lSQL, cnNDS);
                                            lCmd.CommandTimeout = 1200;
                                            lCmd.ExecuteNonQuery();
                                        }
                                        #endregion

                                        oracleTransIDB = cnIDB.BeginTransaction(IsolationLevel.ReadCommitted);
                                        oracleTransCIS = cnCIS.BeginTransaction(IsolationLevel.ReadCommitted);

                                        //Added on 2019-05-07 as found duplicated record in IDB -- consult Soo Long
                                        #region Cancel SOR in IDB
                                        lSQL = "UPDATE ORDER_HEADER SET CANCELLATION_STATUS = 'C' " +
                                            "WHERE ORDER_REQUEST_NO = '" + lSOR + "' ";

                                        var lOracleCmd = new OracleCommand(lSQL, cnIDB);
                                        lOracleCmd.CommandTimeout = 1200;
                                        lOracleCmd.Transaction = oracleTransIDB;
                                        lOracleCmd.ExecuteNonQuery();

                                        lSQL = "UPDATE ORDER_ITEM SET CANCELLATION_STATUS = 'C' " +
                                        "WHERE ORDER_REQUEST_NO = '" + lSOR + "' ";

                                        lOracleCmd = new OracleCommand(lSQL, cnIDB);
                                        lOracleCmd.CommandTimeout = 1200;
                                        lOracleCmd.Transaction = oracleTransIDB;
                                        lOracleCmd.ExecuteNonQuery();

                                        #endregion

                                        #region Cancell SAP CIS
                                        if (lReturn == true)
                                        {
                                            //updade request header
                                            lSQL = "UPDATE SAPSR3.YMSDT_ORDER_HDR " +
                                            "SET STATUS = 'X', PO_NUMBER = PO_NUMBER || '-CXL' " +
                                            "WHERE ORDER_REQUEST_NO = '" + lSOR + "' " +
                                            "AND MANDT = '" + strClient + "' ";

                                            lOracleCmd = new OracleCommand(lSQL, cnCIS);
                                            lOracleCmd.CommandTimeout = 1200;
                                            lOracleCmd.Transaction = oracleTransCIS;
                                            lOracleCmd.ExecuteNonQuery();

                                            //updade request header
                                            lSQL = "UPDATE SAPSR3.YMSDT_REQ_DETAIL SET STATUS = 'X' " +
                                            "WHERE ORDER_REQUEST_NO = '" + lSOR + "' " +
                                            "AND MANDT = '" + strClient + "' ";

                                            lOracleCmd = new OracleCommand(lSQL, cnCIS);
                                            lOracleCmd.CommandTimeout = 1200;
                                            lOracleCmd.Transaction = oracleTransCIS;
                                            lOracleCmd.ExecuteNonQuery();

                                            // Cancel SAP SO if created
                                            string lSONo = "";
                                            lSQL = "SELECT VBELN " +
                                            "FROM SAPSR3.VBAK " +
                                            "WHERE MANDT = '" + strClient + "' " +
                                            "AND IHREZ = '" + lSOR + "' ";

                                            lOracleCmd.CommandText = lSQL;
                                            lOracleCmd.Connection = cnCIS;
                                            lOracleCmd.CommandTimeout = 1200;
                                            lOrRst = lOracleCmd.ExecuteReader();
                                            if (lOrRst.HasRows)
                                            {
                                                lOrRst.Read();
                                                lSONo = (lOrRst.GetString(0) == null ? "" : lOrRst.GetString(0));
                                            }
                                            lOrRst.Close();

                                            if (lSONo == "") //ajit !=
                                            {
                                                string lSOStatus = "";
                                                lSQL = "SELECT ABGRU " +
                                                "FROM SAPSR3.VBAP " +
                                                "WHERE MANDT = '" + strClient + "' " +
                                                "AND VBELN = '" + lSONo + "' " +
                                                "ORDER BY ABGRU ";

                                                lOracleCmd.CommandText = lSQL;
                                                lOracleCmd.Connection = cnCIS;
                                                lOracleCmd.CommandTimeout = 1200;
                                                lOrRst = lOracleCmd.ExecuteReader();
                                                if (lOrRst.HasRows)
                                                {
                                                    lOrRst.Read();
                                                    lSOStatus = (lOrRst.GetString(0) == null ? "" : lOrRst.GetString(0)).Trim();
                                                }
                                                lOrRst.Close();

                                                if (lSOStatus == "")
                                                {
                                                    CreateSOinSAP lSAP = new CreateSOinSAP();
                                                    //SAPConnector.CreateSOinSAP lSAP = new SAPConnector.CreateSOinSAP();
                                                    int lResult = lSAP.CancelSAPSO(CustomerCode, ProjectCode, JobID, lSE.PONumber, lBBSNo, lSONo);
                                                    //return no of items in the order
                                                    if (lResult < 0)
                                                    {
                                                        lReturn = false;
                                                        lErrorMsg = "SAP Error: cannot cancel SO in SAP.";
                                                    }
                                                    lSAP = null;

                                                    lSOStatus = "";
                                                    lSQL = "SELECT ABGRU " +
                                                    "FROM SAPSR3.VBAP " +
                                                    "WHERE MANDT = '" + strClient + "' " +
                                                    "AND VBELN = '" + lSONo + "' " +
                                                    "ORDER BY ABGRU ";

                                                    lOracleCmd.CommandText = lSQL;
                                                    lOracleCmd.Connection = cnCIS;
                                                    lOracleCmd.CommandTimeout = 1200;
                                                    lOrRst = lOracleCmd.ExecuteReader();
                                                    if (lOrRst.HasRows)
                                                    {
                                                        lOrRst.Read();
                                                        lSOStatus = (lOrRst.GetString(0) == null ? "" : lOrRst.GetString(0)).Trim();
                                                    }
                                                    lOrRst.Close();
                                                    if (lSOStatus == "")
                                                    {
                                                        lReturn = false;
                                                        lErrorMsg = "SAP Error: cannot cancel SO in SAP.";
                                                    }
                                                }

                                            }

                                        }
                                        #endregion


                                        if (lReturn == true)
                                        {
                                            if (oracleTransCIS != null) oracleTransCIS.Commit();
                                            if (oracleTransIDB != null) oracleTransIDB.Commit();
                                        }
                                        else
                                        {
                                            if (oracleTransCIS != null) oracleTransCIS.Rollback();
                                            if (oracleTransIDB != null) oracleTransIDB.Rollback();
                                        }
                                        CloseCISConnection(ref cnCIS);
                                        CloseNDSConnection(ref cnNDS);
                                        CloseIDBConnection(ref cnIDB);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (ProdType == "Rebar" || ProdType == "CAB")
                            {
                                //Check the order status in NDS
                                var lJob = db.JobAdvice.Find(CustomerCode, ProjectCode, JobID);
                                if (lJob == null)
                                {
                                    lReturn = false;
                                    lErrorMsg = "Cannot read job advice table.";
                                    //return Json((success: lReturn, message: lErrorMsg));
                                    //return Ok(lReturn);
                                }

                                if (StructureElement == null || StructureElement == "")
                                {
                                    StructureElement = "NONWBS";
                                }
                                if (ScheduledProd == null || ScheduledProd == "")
                                {
                                    ScheduledProd = "N";
                                }

                                var lProcess = db.Process.Find(CustomerCode, ProjectCode, lOrderNumber, StructureElement, ProdType, ScheduledProd);
                                if (lProcess == null)
                                {
                                    //lReturn = false;
                                    //lErrorMsg = "Cannot read process table.";
                                    //return Json(new { success = lReturn, message = lErrorMsg });
                                }
                                else
                                {
                                    lWBS1 = lProcess.WBS1;
                                    lWBS2 = lProcess.WBS2;
                                    lWBS3 = lProcess.WBS3;
                                }

                                if (lWBS1 == null) lWBS1 = "";
                                if (lWBS2 == null) lWBS2 = "";
                                if (lWBS3 == null) lWBS3 = "";

                                var lBBS = (from p in db.BBS
                                            where p.CustomerCode == CustomerCode &&
                                            p.ProjectCode == ProjectCode &&
                                            p.JobID == JobID
                                            select p).ToList();

                                if (lBBS.Count > 0)
                                {
                                    for (int i = 0; i < lBBS.Count; i++)
                                    {
                                        //Cancel SAP order if any
                                        if (lBBS[i].BBSSAPSO != null && lBBS[i].BBSSAPSO != "")
                                        {
                                            lSAPSO = lSAPSO + lBBS[i].BBSSAPSO;

                                            OpenCISConnection(ref cnCIS);
                                            if (cnCIS.State != ConnectionState.Open)
                                            {
                                                lReturn = false;
                                            }
                                            else
                                            {
                                                #region Cancel in SAP
                                                string lSOStatus = "";
                                                lSQL = "SELECT ABGRU " +
                                                "FROM SAPSR3.VBAP " +
                                                "WHERE MANDT = '" + strClient + "' " +
                                                "AND VBELN = '" + lBBS[i].BBSSAPSO + "' " +
                                                "ORDER BY ABGRU ";

                                                var lOracleCmd = new OracleCommand(lSQL, cnCIS);
                                                lOracleCmd.CommandTimeout = 1200;
                                                lOrRst = lOracleCmd.ExecuteReader();
                                                if (lOrRst.HasRows)
                                                {
                                                    lOrRst.Read();
                                                    lSOStatus = (lOrRst.GetString(0) == null ? "" : lOrRst.GetString(0)).Trim();
                                                }
                                                lOrRst.Close();

                                                if (lSOStatus == "")
                                                {
                                                    CreateSOinSAP lSAP = new CreateSOinSAP();
                                                    int lResult = lSAP.CancelSAPSO(CustomerCode, ProjectCode, JobID, lJob.PONumber, lBBS[i].BBSNo, lBBS[i].BBSSAPSO);
                                                    //return no of items in the order
                                                    if (lResult < 0)
                                                    {
                                                        lErrorMsg = "Cannot cancel SO in SAP.";
                                                        lReturn = false;
                                                        break;
                                                    }
                                                }

                                                lSOStatus = "";
                                                lSQL = "SELECT ABGRU " +
                                                "FROM SAPSR3.VBAP " +
                                                "WHERE MANDT = '" + strClient + "' " +
                                                "AND VBELN = '" + lBBS[i].BBSSAPSO + "' " +
                                                "ORDER BY ABGRU ";

                                                lOracleCmd.CommandText = lSQL;
                                                lOracleCmd.Connection = cnCIS;
                                                lOracleCmd.CommandTimeout = 1200;
                                                lOrRst = lOracleCmd.ExecuteReader();
                                                if (lOrRst.HasRows)
                                                {
                                                    lOrRst.Read();
                                                    lSOStatus = (lOrRst.GetString(0) == null ? "" : lOrRst.GetString(0)).Trim();
                                                }
                                                lOrRst.Close();
                                                if (lSOStatus == "")
                                                {
                                                    lReturn = false;
                                                    lErrorMsg = "SAP Error: cannot cancel SO in SAP.";
                                                }

                                                //lReturn = UpdateSAPHist(strClient, lBBS[i].BBSSAPSO, "", 0, 1, "U");
                                                #endregion

                                                #region Cancell SAP CIS
                                                //updade request header
                                                lSQL = "UPDATE SAPSR3.YMSDT_ORDER_HDR " +
                                                "SET STATUS = 'X', PO_NUMBER = PO_NUMBER || '-CXL' " +
                                                "WHERE ORDER_REQUEST_NO = '" + lBBS[i].BBSSAPSO + "' " +
                                                "AND MANDT = '" + strClient + "' ";

                                                lOracleCmd = new OracleCommand(lSQL, cnCIS);
                                                lOracleCmd.CommandTimeout = 1200;
                                                lOracleCmd.ExecuteNonQuery();

                                                //updade request header
                                                lSQL = "UPDATE SAPSR3.YMSDT_REQ_DETAIL SET STATUS = 'X' " +
                                                "WHERE ORDER_REQUEST_NO = '" + lBBS[i].BBSSAPSO + "' " +
                                                "AND MANDT = '" + strClient + "' ";

                                                lOracleCmd = new OracleCommand(lSQL, cnCIS);
                                                lOracleCmd.CommandTimeout = 1200;
                                                lOracleCmd.ExecuteNonQuery();
                                                #endregion
                                                CloseCISConnection(ref cnCIS);

                                                if (lReturn == true)
                                                {
                                                    OpenIDBConnection(ref cnIDB);
                                                    if (cnIDB.State != ConnectionState.Open)
                                                    {
                                                        lReturn = false;
                                                    }
                                                    else
                                                    {
                                                        if (lBBS[i].BBSSAPSO != null && lBBS[i].BBSSAPSO != "")
                                                        {
                                                            lSQL = "UPDATE ORDER_HEADER SET CANCELLATION_STATUS = 'C' " +
                                                            "WHERE ORDER_REQUEST_NO = '" + lBBS[i].BBSSAPSO + "' ";

                                                            lOracleCmd = new OracleCommand(lSQL, cnIDB);
                                                            lOracleCmd.CommandTimeout = 1200;
                                                            lOracleCmd.ExecuteNonQuery();

                                                            lSQL = "UPDATE ORDER_ITEM SET CANCELLATION_STATUS = 'C' " +
                                                            "WHERE ORDER_REQUEST_NO = '" + lBBS[i].BBSSAPSO + "' ";

                                                            lOracleCmd = new OracleCommand(lSQL, cnIDB);
                                                            lOracleCmd.CommandTimeout = 1200;
                                                            lOracleCmd.ExecuteNonQuery();
                                                        }
                                                        CloseIDBConnection(ref cnIDB);
                                                    }
                                                }
                                            }
                                        }

                                        //Cancel CAB and Coupler
                                        if ((lBBS[i].BBSNoNDS != null && lBBS[i].BBSNoNDS != "") || (lBBS[i].BBSNoNDSCoupler != null && lBBS[i].BBSNoNDSCoupler != ""))
                                        {
                                            OpenNDSConnection(ref cnNDS);
                                            if (cnNDS.State != ConnectionState.Open)
                                            {
                                                lReturn = false;
                                            }
                                            else
                                            {
                                                //get project id
                                                lProjectID = 0;
                                                //lSQL = "SELECT intProjectId FROM dbo.ProjectMaster WHERE vchProjectCode = '" + ProjectCode + "' ";
                                                lSQL = "SELECT P.intProjectId FROM dbo.SAPProjectMaster P WHERE P.vchProjectCode = '" + ProjectCode + "' ";
                                             

                                                lCmd = new SqlCommand(lSQL, cnNDS);
                                                lCmd.CommandTimeout = 1200;
                                                lRst = lCmd.ExecuteReader();
                                                if (lRst.HasRows)
                                                {
                                                    lRst.Read();
                                                    lProjectID = lRst.GetInt32(0);
                                                }
                                                lRst.Close();


                                                //check release status
                                                var lStatus = 0;
                                                var lStatusCoupler = 0;
                                                lWBSElementID = 0;

                                                if (lBBS[i].BBSNoNDS != null && lBBS[i].BBSNoNDS != "")
                                                {
                                                    lSQL = "SELECT R.tntStatusId, P.intWBSElementID " +
                                                    "FROM dbo.BBSReleaseDetails R, " +
                                                    "dbo.BBSPostHeader P, " +
                                                    "dbo.PostGroupMarkingDetails M, " +
                                                    "dbo.GroupMarkingDetails G, " +
                                                    "dbo.SELevelDetails S, " +
                                                    "dbo.ProductTypeMaster T " +
                                                    "WHERE R.intPostHeaderid = M.intPostHeaderId " +
                                                    "AND R.intPostHeaderid = P.intPostHeaderId " +
                                                    "AND M.intGroupMarkId = G.intGroupMarkId " +
                                                    "AND G.intGroupMarkId = S.intGroupMarkId " +
                                                    "AND S.sitProductTypeId = T.sitProductTypeID " +
                                                    "AND vchProductType = 'CAB' " +
                                                    "AND G.intProjectId = " + lProjectID.ToString() + " " +
                                                    "AND G.vchGroupMarkingName = '" + lBBS[i].BBSNoNDS + "' ";

                                                    lCmd = new SqlCommand(lSQL, cnNDS);
                                                    lCmd.CommandTimeout = 1200;
                                                    lRst = lCmd.ExecuteReader();
                                                    if (lRst.HasRows)
                                                    {
                                                        lRst.Read();
                                                        lStatus = lRst.GetByte(0);
                                                        lWBSElementID = lRst.GetInt32(1);
                                                    }
                                                    lRst.Close();
                                                    if (lStatus == 12)  //Released
                                                    {
                                                        //CloseNDSConnection(ref cnNDS);
                                                        //lReturn = false;

                                                        //Cancel Released SOR
                                                        lSQL = "dbo.BBSReleaseCAB_Insert ";
                                                        lCmd = new SqlCommand(lSQL, cnNDS);
                                                        //lCmd.Transaction = osqlTransNDS;
                                                        lCmd.CommandTimeout = 1200;
                                                        lCmd.CommandType = CommandType.StoredProcedure;
                                                        lCmd.Parameters.Add("@ORD_REQ_NO", SqlDbType.Char);
                                                        lCmd.Parameters.Add("@IntProjectID", SqlDbType.Int);
                                                        lCmd.Parameters.Add("@IntWBSElementID", SqlDbType.Int);
                                                        lCmd.Parameters.Add("@BBS_NO", SqlDbType.Char);
                                                        lCmd.Parameters.Add("@chrBBSStatus", SqlDbType.Char);
                                                        lCmd.Parameters.Add("@UserID", SqlDbType.Int);

                                                        lCmd.Parameters["@ORD_REQ_NO"].Value = lBBS[i].BBSSOR;
                                                        lCmd.Parameters["@IntProjectID"].Value = lProjectID;
                                                        lCmd.Parameters["@IntWBSElementID"].Value = lWBSElementID;
                                                        lCmd.Parameters["@BBS_NO"].Value = lBBS[i].BBSNoNDS;
                                                        lCmd.Parameters["@chrBBSStatus"].Value = "C";
                                                        lCmd.Parameters["@UserID"].Value = 11;
                                                        lCmd.CommandTimeout = 1200;
                                                        lCmd.ExecuteNonQuery();
                                                    }
                                                }

                                                if (lReturn == true && lBBS[i].BBSNoNDSCoupler != null && lBBS[i].BBSNoNDSCoupler != "")
                                                {
                                                    lSQL = "SELECT R.tntStatusId, P.intWBSElementID " +
                                                    "FROM dbo.BBSReleaseDetails R, " +
                                                    "dbo.BBSPostHeader P, " +
                                                    "dbo.PostGroupMarkingDetails M, " +
                                                    "dbo.GroupMarkingDetails G, " +
                                                    "dbo.SELevelDetails S, " +
                                                    "dbo.ProductTypeMaster T " +
                                                    "WHERE R.intPostHeaderid = M.intPostHeaderId " +
                                                    "AND R.intPostHeaderid = P.intPostHeaderId " +
                                                    "AND M.intGroupMarkId = G.intGroupMarkId " +
                                                    "AND G.intGroupMarkId = S.intGroupMarkId " +
                                                    "AND S.sitProductTypeId = T.sitProductTypeID " +
                                                    "AND vchProductType = 'CAB' " +
                                                    "AND G.intProjectId = " + lProjectID.ToString() + " " +
                                                    "AND G.vchGroupMarkingName = '" + lBBS[i].BBSNoNDSCoupler + "' ";

                                                    lCmd = new SqlCommand(lSQL, cnNDS);
                                                    lCmd.CommandTimeout = 1200;
                                                    lRst = lCmd.ExecuteReader();
                                                    if (lRst.HasRows)
                                                    {
                                                        lRst.Read();
                                                        lStatusCoupler = lRst.GetByte(0);
                                                        lWBSElementID = lRst.GetInt32(1);
                                                    }
                                                    lRst.Close();
                                                    if (lStatusCoupler == 12)  //Released
                                                    {
                                                        //CloseNDSConnection(ref cnNDS);
                                                        //lReturn = false;

                                                        //Cancel Released SOR
                                                        lSQL = "dbo.BBSReleaseCAB_Insert ";
                                                        lCmd = new SqlCommand(lSQL, cnNDS);
                                                        //lCmd.Transaction = osqlTransNDS;
                                                        lCmd.CommandTimeout = 1200;
                                                        lCmd.CommandType = CommandType.StoredProcedure;
                                                        lCmd.Parameters.Add("@ORD_REQ_NO", SqlDbType.Char);
                                                        lCmd.Parameters.Add("@IntProjectID", SqlDbType.Int);
                                                        lCmd.Parameters.Add("@IntWBSElementID", SqlDbType.Int);
                                                        lCmd.Parameters.Add("@BBS_NO", SqlDbType.Char);
                                                        lCmd.Parameters.Add("@chrBBSStatus", SqlDbType.Char);
                                                        lCmd.Parameters.Add("@UserID", SqlDbType.Int);

                                                        lCmd.Parameters["@ORD_REQ_NO"].Value = lBBS[i].BBSSORCoupler;
                                                        lCmd.Parameters["@IntProjectID"].Value = lProjectID;
                                                        lCmd.Parameters["@IntWBSElementID"].Value = lWBSElementID;
                                                        lCmd.Parameters["@BBS_NO"].Value = lBBS[i].BBSNoNDSCoupler;
                                                        lCmd.Parameters["@chrBBSStatus"].Value = "C";
                                                        lCmd.Parameters["@UserID"].Value = 11;
                                                        lCmd.CommandTimeout = 1200;
                                                        lCmd.ExecuteNonQuery();
                                                    }
                                                }

                                                if (lReturn == true)
                                                {
                                                    lWBSElementID = 0;
                                                    lSQL = "SELECT E.intWBSElementId from dbo.WBSElements E, dbo.WBS W, dbo.WBSElementsDetails D " +
                                                        "where e.intWBSId = W.intWBSId " +
                                                        "and E.intWBSElementId = D.intWBSElementId " +
                                                        "and W.intProjectid = " + lProjectID + " " +
                                                        "and vchWBS1 = '" + lWBS1 + "' " +
                                                        "and vchWBS2 = '" + lWBS2 + "' " +
                                                        "and vchWBS3 = '" + lWBS3 + "' " +
                                                        "and sitProductTypeId IN (SELECT sitProductTypeID " +
                                                        "FROM dbo.ProductTypeMaster WHERE vchProductType = 'CAB') " +
                                                        "and intStructureElementTypeId IN " +
                                                        "(SELECT intStructureElementTypeId " +
                                                        "FROM dbo.StructureElementMaster " +
                                                        "WHERE vchStructureElementType = '" + lBBS[i].BBSStrucElem + "' ) " +
                                                        "GROUP BY E.intWBSElementId ";

                                                    lCmd = new SqlCommand(lSQL, cnNDS);
                                                    lCmd.CommandTimeout = 1200;
                                                    lRst = lCmd.ExecuteReader();
                                                    if (lRst.HasRows)
                                                    {
                                                        lRst.Read();
                                                        lWBSElementID = (int)lRst.GetValue(0);
                                                    }
                                                    lRst.Close();
                                                }

                                                //if (lReturn == true && lBBS[i].BBSNoNDS != null && lBBS[i].BBSNoNDS != "" && lStatus != 14)  //Cancelled
                                                if (lReturn == true && lBBS[i].BBSNoNDS != null && lBBS[i].BBSNoNDS != "")  //Cancelled
                                                {
                                                    //delete from request details
                                                    lSQL = "UPDATE dbo.SAP_REQUEST_DETAILS SET STATUS = 'X' " +
                                                    "WHERE ORD_REQ_NO = '" + lBBS[i].BBSSOR + "' ";

                                                    lCmd = new SqlCommand(lSQL, cnNDS);
                                                    lCmd.CommandTimeout = 1200;
                                                    lCmd.ExecuteNonQuery();

                                                    //delete from request header
                                                    lSQL = "UPDATE dbo.SAP_REQUEST_HDR SET STATUS = 'X', PO_NUM = PO_NUM + '-CXL' " +
                                                    "WHERE ORD_REQ_NO = '" + lBBS[i].BBSSOR + "' ";

                                                    lCmd = new SqlCommand(lSQL, cnNDS);
                                                    lCmd.CommandTimeout = 1200;
                                                    lCmd.ExecuteNonQuery();

                                                    // get group marking ID and SE Level ID
                                                    lSQL = "SELECT G.intGroupMarkId, S.intSEDetailingId " +
                                                    "FROM dbo.GroupMarkingDetails G, dbo.SELevelDetails S  " +
                                                    "WHERE G.intGroupMarkId = S.intGroupMarkId " +
                                                    "AND G.intProjectid = " + lProjectID.ToString() + " " +
                                                    "and S.intStructureElementTypeId in " +
                                                    "(SELECT intStructureElementTypeId " +
                                                    "FROM dbo.StructureElementMaster " +
                                                    "WHERE vchStructureElementType = '" + lBBS[i].BBSStrucElem + "' ) " +
                                                    "and S.sitProductTypeId in " +
                                                    "(SELECT sitProductTypeID " +
                                                    "FROM dbo.ProductTypeMaster " +
                                                    "WHERE vchProductType = 'CAB' ) " +
                                                    "AND vchGroupMarkingName = '" + lBBS[i].BBSNoNDS + "' " +
                                                    "AND tntGroupRevNo = 0 ";

                                                    lCmd = new SqlCommand(lSQL, cnNDS);
                                                    lCmd.CommandTimeout = 1200;
                                                    lRst = lCmd.ExecuteReader();
                                                    if (lRst.HasRows)
                                                    {
                                                        lRst.Read();
                                                        lGroupMarkingID = lRst.GetInt32(0);
                                                        lSELevelID = lRst.GetInt32(1);
                                                    }

                                                    lRst.Close();

                                                    if (lGroupMarkingID > 0 && lSELevelID > 0)
                                                    {
                                                        //if (lStatus != 12 && lStatus != 14)
                                                        if (lStatus != 0)
                                                        {
                                                            //Remove BBS

                                                            //lSQL = "dbo.BBSPostingCAB_Delete ";
                                                            //lCmd = new SqlCommand(lSQL, cnNDS);
                                                            //lCmd.Transaction = osqlTransNDS;
                                                            //lCmd.CommandTimeout = 1200;
                                                            //lCmd.CommandType = CommandType.StoredProcedure;
                                                            //lCmd.Parameters.Add("@IntProjectID", SqlDbType.Int);
                                                            //lCmd.Parameters.Add("@IntWBSElementID", SqlDbType.Int);
                                                            //lCmd.Parameters.Add("@IntGroupMarkID", SqlDbType.Int);
                                                            //lCmd.Parameters.Add("@BBS_NO", SqlDbType.Char);

                                                            //lCmd.Parameters["@IntProjectID"].Value = lProjectID;
                                                            //lCmd.Parameters["@IntWBSElementID"].Value = lWBSElementID;
                                                            //lCmd.Parameters["@IntGroupMarkID"].Value = lGroupMarkingID;
                                                            //lCmd.Parameters["@BBS_NO"].Value = lBBS[i].BBSNoNDS;
                                                            //lCmd.CommandTimeout = 1200;
                                                            //lCmd.ExecuteNonQuery();

                                                            //delete CAB acc
                                                            lSQL = "DELETE FROM dbo.AccProductMarkingDetails " +
                                                            "WHERE intSEDetailingId = " + lSELevelID.ToString() + " ";

                                                            lCmd = new SqlCommand(lSQL, cnNDS);
                                                            lCmd.CommandTimeout = 1200;
                                                            lCmd.ExecuteNonQuery();

                                                            //delete from parameter details
                                                            lSQL = "DELETE FROM dbo.SHAPEDETAILINGTRANS_PARALLEL " +
                                                            "WHERE intShapeTransHeaderId IN " +
                                                            "(SELECT intShapeTransHeaderId FROM dbo.CABProductMarkingDetails " +
                                                            "WHERE intSEDetailingId = " + lSELevelID.ToString() + ") ";
                                                            lCmd = new SqlCommand(lSQL, cnNDS);
                                                            lCmd.CommandTimeout = 1200;
                                                            lCmd.ExecuteNonQuery();

                                                            //delete from shape trans header
                                                            lSQL = "DELETE FROM dbo.ShapeDetailingTrans " +
                                                            "WHERE intShapeTransHeaderId IN " +
                                                            "(SELECT intShapeTransHeaderId FROM dbo.CABProductMarkingDetails " +
                                                            "WHERE intSEDetailingId = " + lSELevelID.ToString() + ") ";
                                                            lCmd = new SqlCommand(lSQL, cnNDS);
                                                            lCmd.CommandTimeout = 1200;
                                                            lCmd.ExecuteNonQuery();

                                                            //delete from shape trans header
                                                            lSQL = "DELETE FROM dbo.ShapeDetailingTransHeader " +
                                                            "WHERE intShapeTransHeaderId IN " +
                                                            "(SELECT intShapeTransHeaderId FROM dbo.CABProductMarkingDetails " +
                                                            "WHERE intSEDetailingId = " + lSELevelID.ToString() + ") ";
                                                            lCmd = new SqlCommand(lSQL, cnNDS);
                                                            lCmd.CommandTimeout = 1200;
                                                            lCmd.ExecuteNonQuery();

                                                            //delete CAB product marking details
                                                            lSQL = "DELETE FROM dbo.CABProductMarkingDetails " +
                                                            "WHERE intSEDetailingId = " + lSELevelID.ToString() + " ";

                                                            lCmd = new SqlCommand(lSQL, cnNDS);
                                                            lCmd.CommandTimeout = 1200;
                                                            lCmd.ExecuteNonQuery();

                                                            //delete from group marking
                                                            lSQL = "DELETE FROM dbo.GroupMarkingDetails " +
                                                            "WHERE intGroupMarkId = " + lGroupMarkingID.ToString() + " ";

                                                            lCmd = new SqlCommand(lSQL, cnNDS);
                                                            lCmd.CommandTimeout = 1200;
                                                            lCmd.ExecuteNonQuery();

                                                            //delete from SE Level
                                                            lSQL = "DELETE FROM dbo.SELevelDetails " +
                                                            "WHERE intSEDetailingId = " + lSELevelID.ToString() + " ";

                                                            lCmd = new SqlCommand(lSQL, cnNDS);
                                                            lCmd.CommandTimeout = 1200;
                                                            lCmd.ExecuteNonQuery();

                                                            int lPostHDID = 0;
                                                            lSQL = "SELECT intPostHeaderId FROM dbo.POSTGROUPMARKINGDETAILS " +
                                                            "WHERE intGroupMarkId = " + lGroupMarkingID.ToString() + " ";
                                                            lCmd = new SqlCommand(lSQL, cnNDS);
                                                            lCmd.CommandTimeout = 1200;
                                                            lRst = lCmd.ExecuteReader();
                                                            if (lRst.HasRows)
                                                            {
                                                                lRst.Read();
                                                                lPostHDID = lRst.GetInt32(0);
                                                            }
                                                            lRst.Close();

                                                            //delete from group marking
                                                            lSQL = "DELETE FROM dbo.POSTGROUPMARKINGDETAILS " +
                                                            "WHERE intGroupMarkId = " + lGroupMarkingID.ToString() + " ";
                                                            lCmd = new SqlCommand(lSQL, cnNDS);
                                                            lCmd.CommandTimeout = 1200;
                                                            lCmd.ExecuteNonQuery();

                                                            //delete from posting header
                                                            lSQL = "DELETE FROM dbo.BBSPostHeader " +
                                                            "WHERE intPostHeaderId = " + lPostHDID.ToString() + " ";
                                                            lCmd = new SqlCommand(lSQL, cnNDS);
                                                            lCmd.CommandTimeout = 1200;
                                                            lCmd.ExecuteNonQuery();

                                                            //delete from BBS release
                                                            lSQL = "DELETE FROM dbo.BBSReleaseDetails " +
                                                            "WHERE intPostHeaderid = " + lPostHDID.ToString() + " ";
                                                            lCmd = new SqlCommand(lSQL, cnNDS);
                                                            lCmd.CommandTimeout = 1200;
                                                            lCmd.ExecuteNonQuery();
                                                        }


                                                    }
                                                }

                                                //if (lReturn == true && lBBS[i].BBSNoNDSCoupler != null && lBBS[i].BBSNoNDSCoupler != "" && lStatusCoupler != 14)  //Cancelled
                                                if (lReturn == true && lBBS[i].BBSNoNDSCoupler != null && lBBS[i].BBSNoNDSCoupler != "")  //Cancelled
                                                {
                                                    //delete from request details
                                                    lSQL = "UPDATE SAP_REQUEST_DETAILS SET STATUS = 'X' " +
                                                    "WHERE ORD_REQ_NO = '" + lBBS[i].BBSSORCoupler + "' ";

                                                    lCmd = new SqlCommand(lSQL, cnNDS);
                                                    lCmd.CommandTimeout = 1200;
                                                    lCmd.ExecuteNonQuery();

                                                    //delete from request header
                                                    lSQL = "UPDATE SAP_REQUEST_HDR SET STATUS = 'X', PO_NUM = PO_NUM + '-CXL' " +
                                                    "WHERE ORD_REQ_NO = '" + lBBS[i].BBSSORCoupler + "' ";

                                                    lCmd = new SqlCommand(lSQL, cnNDS);
                                                    lCmd.CommandTimeout = 1200;
                                                    lCmd.ExecuteNonQuery();

                                                    // get group marking ID and SE Level ID
                                                    lSQL = "SELECT G.intGroupMarkId, S.intSEDetailingId " +
                                                    "FROM dbo.GroupMarkingDetails G, dbo.SELevelDetails S  " +
                                                    "WHERE G.intGroupMarkId = S.intGroupMarkId " +
                                                    "AND G.intProjectid = " + lProjectID.ToString() + " " +
                                                    "and S.intStructureElementTypeId in " +
                                                    "(SELECT intStructureElementTypeId " +
                                                    "FROM dbo.StructureElementMaster " +
                                                    "WHERE vchStructureElementType = '" + lBBS[i].BBSStrucElem + "' ) " +
                                                    "and S.sitProductTypeId in " +
                                                    "(SELECT sitProductTypeID " +
                                                    "FROM dbo.ProductTypeMaster " +
                                                    "WHERE vchProductType = 'CAB' ) " +
                                                    "AND vchGroupMarkingName = '" + lBBS[i].BBSNoNDSCoupler + "' " +
                                                    "AND tntGroupRevNo = 0 ";

                                                    lCmd = new SqlCommand(lSQL, cnNDS);
                                                    lCmd.CommandTimeout = 1200;
                                                    lRst = lCmd.ExecuteReader();
                                                    if (lRst.HasRows)
                                                    {
                                                        lRst.Read();
                                                        lGroupMarkingID = lRst.GetInt32(0);
                                                        lSELevelID = lRst.GetInt32(1);
                                                    }

                                                    lRst.Close();

                                                    if (lGroupMarkingID > 0 && lSELevelID > 0)
                                                    {

                                                        //if (lStatusCoupler != 12 && lStatusCoupler != 14)
                                                        if (lStatusCoupler != 0)
                                                        {
                                                            //Remove BBS
                                                            //lSQL = "dbo.BBSPostingCAB_Delete ";
                                                            //lCmd = new SqlCommand(lSQL, cnNDS);
                                                            //lCmd.Transaction = osqlTransNDS;
                                                            //lCmd.CommandTimeout = 1200;
                                                            //lCmd.CommandType = CommandType.StoredProcedure;
                                                            //lCmd.Parameters.Add("@IntProjectID", SqlDbType.Int);
                                                            //lCmd.Parameters.Add("@IntWBSElementID", SqlDbType.Int);
                                                            //lCmd.Parameters.Add("@IntGroupMarkID", SqlDbType.Int);
                                                            //lCmd.Parameters.Add("@BBS_NO", SqlDbType.Char);

                                                            //lCmd.Parameters["@IntProjectID"].Value = lProjectID;
                                                            //lCmd.Parameters["@IntWBSElementID"].Value = lWBSElementID;
                                                            //lCmd.Parameters["@IntGroupMarkID"].Value = lGroupMarkingID;
                                                            //lCmd.Parameters["@BBS_NO"].Value = lBBS[i].BBSNoNDSCoupler;
                                                            //lCmd.CommandTimeout = 1200;
                                                            //lCmd.ExecuteNonQuery();

                                                            //delete CAB acc
                                                            lSQL = "DELETE FROM dbo.AccProductMarkingDetails " +
                                                            "WHERE intSEDetailingId = " + lSELevelID.ToString() + " ";

                                                            lCmd = new SqlCommand(lSQL, cnNDS);
                                                            lCmd.CommandTimeout = 1200;
                                                            lCmd.ExecuteNonQuery();

                                                            //delete from parameter details
                                                            lSQL = "DELETE FROM dbo.SHAPEDETAILINGTRANS_PARALLEL " +
                                                            "WHERE intShapeTransHeaderId IN " +
                                                            "(SELECT intShapeTransHeaderId FROM dbo.CABProductMarkingDetails " +
                                                            "WHERE intSEDetailingId = " + lSELevelID.ToString() + ") ";
                                                            lCmd = new SqlCommand(lSQL, cnNDS);
                                                            lCmd.CommandTimeout = 1200;
                                                            lCmd.ExecuteNonQuery();

                                                            //delete from shape trans header
                                                            lSQL = "DELETE FROM dbo.ShapeDetailingTrans " +
                                                            "WHERE intShapeTransHeaderId IN " +
                                                            "(SELECT intShapeTransHeaderId FROM dbo.CABProductMarkingDetails " +
                                                            "WHERE intSEDetailingId = " + lSELevelID.ToString() + ") ";
                                                            lCmd = new SqlCommand(lSQL, cnNDS);
                                                            lCmd.CommandTimeout = 1200;
                                                            lCmd.ExecuteNonQuery();

                                                            //delete from shape trans header
                                                            lSQL = "DELETE FROM dbo.ShapeDetailingTransHeader " +
                                                            "WHERE intShapeTransHeaderId IN " +
                                                            "(SELECT intShapeTransHeaderId FROM dbo.CABProductMarkingDetails " +
                                                            "WHERE intSEDetailingId = " + lSELevelID.ToString() + ") ";
                                                            lCmd = new SqlCommand(lSQL, cnNDS);
                                                            lCmd.CommandTimeout = 1200;
                                                            lCmd.ExecuteNonQuery();

                                                            //delete CAB product marking details
                                                            lSQL = "DELETE FROM dbo.CABProductMarkingDetails " +
                                                            "WHERE intSEDetailingId = " + lSELevelID.ToString() + " ";

                                                            lCmd = new SqlCommand(lSQL, cnNDS);
                                                            lCmd.CommandTimeout = 1200;
                                                            lCmd.ExecuteNonQuery();

                                                            //delete from group marking
                                                            lSQL = "DELETE FROM dbo.GroupMarkingDetails " +
                                                            "WHERE intGroupMarkId = " + lGroupMarkingID.ToString() + " ";

                                                            lCmd = new SqlCommand(lSQL, cnNDS);
                                                            lCmd.CommandTimeout = 1200;
                                                            lCmd.ExecuteNonQuery();

                                                            //delete from SE Level
                                                            lSQL = "DELETE FROM dbo.SELevelDetails " +
                                                            "WHERE intSEDetailingId = " + lSELevelID.ToString() + " ";

                                                            lCmd = new SqlCommand(lSQL, cnNDS);
                                                            lCmd.CommandTimeout = 1200;
                                                            lCmd.ExecuteNonQuery();

                                                            int lPostHDID = 0;
                                                            lSQL = "SELECT intPostHeaderId FROM dbo.POSTGROUPMARKINGDETAILS " +
                                                            "WHERE intGroupMarkId = " + lGroupMarkingID.ToString() + " ";
                                                            lCmd = new SqlCommand(lSQL, cnNDS);
                                                            lCmd.CommandTimeout = 1200;
                                                            lRst = lCmd.ExecuteReader();
                                                            if (lRst.HasRows)
                                                            {
                                                                lRst.Read();
                                                                lPostHDID = lRst.GetInt32(0);
                                                            }
                                                            lRst.Close();

                                                            //delete from group marking
                                                            lSQL = "DELETE FROM dbo.POSTGROUPMARKINGDETAILS " +
                                                            "WHERE intGroupMarkId = " + lGroupMarkingID.ToString() + " ";
                                                            lCmd = new SqlCommand(lSQL, cnNDS);
                                                            lCmd.CommandTimeout = 1200;
                                                            lCmd.ExecuteNonQuery();

                                                            //delete from posting header
                                                            lSQL = "DELETE FROM dbo.BBSPostHeader " +
                                                            "WHERE intPostHeaderId = " + lPostHDID.ToString() + " ";
                                                            lCmd = new SqlCommand(lSQL, cnNDS);
                                                            lCmd.CommandTimeout = 1200;
                                                            lCmd.ExecuteNonQuery();

                                                            //delete from BBS release
                                                            lSQL = "DELETE FROM dbo.BBSReleaseDetails " +
                                                            "WHERE intPostHeaderid = " + lPostHDID.ToString() + " ";
                                                            lCmd = new SqlCommand(lSQL, cnNDS);
                                                            lCmd.CommandTimeout = 1200;
                                                            lCmd.ExecuteNonQuery();
                                                        }


                                                    }
                                                }

                                                CloseNDSConnection(ref cnNDS);
                                            }

                                            //Added on 2019-05-07 as found duplicated record in IDB -- consult Soo Long
                                            if (lReturn == true)
                                            {
                                                OpenIDBConnection(ref cnIDB);
                                                if (cnIDB.State != ConnectionState.Open)
                                                {
                                                    lReturn = false;
                                                }
                                                else
                                                {
                                                    oracleTransIDB = cnIDB.BeginTransaction(IsolationLevel.ReadCommitted);
                                                    if (lBBS[i].BBSSOR != null && lBBS[i].BBSSOR != "")
                                                    {
                                                        lSQL = "UPDATE ORDER_HEADER SET CANCELLATION_STATUS = 'C' " +
                                                        "WHERE ORDER_REQUEST_NO = '" + lBBS[i].BBSSOR + "' ";

                                                        var lOracleCmd = new OracleCommand(lSQL, cnIDB);
                                                        lOracleCmd.CommandTimeout = 1200;
                                                        lOracleCmd.Transaction = oracleTransIDB;
                                                        lOracleCmd.ExecuteNonQuery();

                                                        lSQL = "UPDATE ORDER_ITEM SET CANCELLATION_STATUS = 'C' " +
                                                        "WHERE ORDER_REQUEST_NO = '" + lBBS[i].BBSSOR + "' ";

                                                        lOracleCmd = new OracleCommand(lSQL, cnIDB);
                                                        lOracleCmd.CommandTimeout = 1200;
                                                        lOracleCmd.Transaction = oracleTransIDB;
                                                        lOracleCmd.ExecuteNonQuery();
                                                    }
                                                    if (lBBS[i].BBSSORCoupler != null && lBBS[i].BBSSORCoupler != "")
                                                    {
                                                        lSQL = "UPDATE ORDER_HEADER SET CANCELLATION_STATUS = 'C' " +
                                                        "WHERE ORDER_REQUEST_NO = '" + lBBS[i].BBSSORCoupler + "' ";

                                                        var lOracleCmd = new OracleCommand(lSQL, cnIDB);
                                                        lOracleCmd.CommandTimeout = 1200;
                                                        lOracleCmd.Transaction = oracleTransIDB;
                                                        lOracleCmd.ExecuteNonQuery();

                                                        lSQL = "UPDATE ORDER_ITEM SET CANCELLATION_STATUS = 'C' " +
                                                        "WHERE ORDER_REQUEST_NO = '" + lBBS[i].BBSSORCoupler + "' ";

                                                        lOracleCmd = new OracleCommand(lSQL, cnIDB);
                                                        lOracleCmd.CommandTimeout = 1200;
                                                        lOracleCmd.Transaction = oracleTransIDB;
                                                        lOracleCmd.ExecuteNonQuery();
                                                    }
                                                    if (oracleTransIDB != null) oracleTransIDB.Commit();
                                                    CloseIDBConnection(ref cnIDB);
                                                }
                                            }

                                            if (lReturn == true)
                                            {
                                                OpenCISConnection(ref cnCIS);
                                                if (cnCIS.State != ConnectionState.Open)
                                                {
                                                    lReturn = false;
                                                }
                                                else
                                                {
                                                    oracleTransCIS = cnCIS.BeginTransaction(IsolationLevel.ReadCommitted);
                                                    if (lBBS[i].BBSSOR != null && lBBS[i].BBSSOR != "")
                                                    {
                                                        lSAPSO = lSAPSO + lBBS[i].BBSSOR;
                                                        //updade request header
                                                        lSQL = "UPDATE SAPSR3.YMSDT_ORDER_HDR " +
                                                        "SET STATUS = 'X', PO_NUMBER = PO_NUMBER || '-CXL' " +
                                                        "WHERE ORDER_REQUEST_NO = '" + lBBS[i].BBSSOR + "' " +
                                                        "AND MANDT = '" + strClient + "' ";

                                                        var lOracleCmd = new OracleCommand(lSQL, cnCIS);
                                                        lOracleCmd.CommandTimeout = 1200;
                                                        lOracleCmd.Transaction = oracleTransCIS;
                                                        lOracleCmd.ExecuteNonQuery();

                                                        //updade request header
                                                        lSQL = "UPDATE SAPSR3.YMSDT_REQ_DETAIL SET STATUS = 'X' " +
                                                        "WHERE ORDER_REQUEST_NO = '" + lBBS[i].BBSSOR + "' " +
                                                        "AND MANDT = '" + strClient + "' ";

                                                        lOracleCmd = new OracleCommand(lSQL, cnCIS);
                                                        lOracleCmd.CommandTimeout = 1200;
                                                        lOracleCmd.Transaction = oracleTransCIS;
                                                        lOracleCmd.ExecuteNonQuery();

                                                        // Cancel SAP SO if created
                                                        string lSONo = "";
                                                        lSQL = "SELECT VBELN " +
                                                        "FROM SAPSR3.VBAK " +
                                                        "WHERE MANDT = '" + strClient + "' " +
                                                        "AND IHREZ = '" + lBBS[i].BBSSOR + "' ";

                                                        lOracleCmd.CommandText = lSQL;
                                                        lOracleCmd.Connection = cnCIS;
                                                        lOracleCmd.CommandTimeout = 1200;
                                                        lOrRst = lOracleCmd.ExecuteReader();
                                                        if (lOrRst.HasRows)
                                                        {
                                                            lOrRst.Read();
                                                            lSONo = (lOrRst.GetString(0) == null ? "" : lOrRst.GetString(0));
                                                        }
                                                        lOrRst.Close();

                                                        if (lSONo == "")//ajit!=
                                                        {
                                                            string lSOStatus = "";
                                                            lSQL = "SELECT ABGRU " +
                                                            "FROM SAPSR3.VBAP " +
                                                            "WHERE MANDT = '" + strClient + "' " +
                                                            "AND VBELN = '" + lSONo + "' " +
                                                            "ORDER BY ABGRU ";

                                                            lOracleCmd.CommandText = lSQL;
                                                            lOracleCmd.Connection = cnCIS;
                                                            lOracleCmd.CommandTimeout = 1200;
                                                            lOrRst = lOracleCmd.ExecuteReader();
                                                            if (lOrRst.HasRows)
                                                            {
                                                                lOrRst.Read();
                                                                lSOStatus = (lOrRst.GetString(0) == null ? "" : lOrRst.GetString(0)).Trim();
                                                            }
                                                            lOrRst.Close();

                                                            if (lSOStatus == "")
                                                            {
                                                                CreateSOinSAP lSAP = new CreateSOinSAP();
                                                                int lResult = lSAP.CancelSAPSO(CustomerCode, ProjectCode, JobID, lJob.PONumber, lBBS[i].BBSNo, lSONo);
                                                                //return no of items in the order
                                                                if (lResult < 0)
                                                                {
                                                                    lErrorMsg = "Cannot cancel So in SAP.";
                                                                    lReturn = false;
                                                                    break;
                                                                }
                                                                lSAP = null;

                                                                lSOStatus = "";
                                                                lSQL = "SELECT ABGRU " +
                                                                "FROM SAPSR3.VBAP " +
                                                                "WHERE MANDT = '" + strClient + "' " +
                                                                "AND VBELN = '" + lSONo + "' " +
                                                                "ORDER BY ABGRU ";

                                                                lOracleCmd.CommandText = lSQL;
                                                                lOracleCmd.Connection = cnCIS;
                                                                lOracleCmd.CommandTimeout = 1200;
                                                                lOrRst = lOracleCmd.ExecuteReader();
                                                                if (lOrRst.HasRows)
                                                                {
                                                                    lOrRst.Read();
                                                                    lSOStatus = (lOrRst.GetString(0) == null ? "" : lOrRst.GetString(0)).Trim();
                                                                }
                                                                lOrRst.Close();
                                                                if (lSOStatus == "")
                                                                {
                                                                    lReturn = false;
                                                                    lErrorMsg = "SAP Error: cannot cancel SO in SAP.";
                                                                }

                                                            }

                                                        }

                                                    }

                                                    if (lBBS[i].BBSSORCoupler != null && lBBS[i].BBSSORCoupler != "")
                                                    {
                                                        lSAPSO = lSAPSO + lBBS[i].BBSSORCoupler;
                                                        //updade request header
                                                        lSQL = "UPDATE SAPSR3.YMSDT_ORDER_HDR " +
                                                        "SET STATUS = 'X', PO_NUMBER = PO_NUMBER || '-CXL' " +
                                                        "WHERE ORDER_REQUEST_NO = '" + lBBS[i].BBSSORCoupler + "' " +
                                                        "AND MANDT = '" + strClient + "' ";

                                                        var lOracleCmd = new OracleCommand(lSQL, cnCIS);
                                                        lOracleCmd.CommandTimeout = 1200;
                                                        lOracleCmd.Transaction = oracleTransCIS;
                                                        lOracleCmd.ExecuteNonQuery();

                                                        //updade request header
                                                        lSQL = "UPDATE SAPSR3.YMSDT_REQ_DETAIL SET STATUS = 'X' " +
                                                        "WHERE ORDER_REQUEST_NO = '" + lBBS[i].BBSSORCoupler + "' " +
                                                        "AND MANDT = '" + strClient + "' ";

                                                        lOracleCmd = new OracleCommand(lSQL, cnCIS);
                                                        lOracleCmd.CommandTimeout = 1200;
                                                        lOracleCmd.Transaction = oracleTransCIS;
                                                        lOracleCmd.ExecuteNonQuery();

                                                        // Cancel SAP SO if created
                                                        string lSONo = "";
                                                        lSQL = "SELECT VBELN " +
                                                        "FROM SAPSR3.VBAK " +
                                                        "WHERE MANDT = '" + strClient + "' " +
                                                        "AND IHREZ = '" + lBBS[i].BBSSORCoupler + "' ";

                                                        lOracleCmd.CommandText = lSQL;
                                                        lOracleCmd.Connection = cnCIS;
                                                        lOracleCmd.CommandTimeout = 1200;
                                                        lOrRst = lOracleCmd.ExecuteReader();
                                                        if (lOrRst.HasRows)
                                                        {
                                                            lOrRst.Read();
                                                            lSONo = (lOrRst.GetString(0) == null ? "" : lOrRst.GetString(0));
                                                        }
                                                        lOrRst.Close();

                                                        if (lSONo != "")
                                                        {
                                                            string lSOStatus = "";
                                                            lSQL = "SELECT ABGRU " +
                                                            "FROM SAPSR3.VBAP " +
                                                            "WHERE MANDT = '" + strClient + "' " +
                                                            "AND VBELN = '" + lSONo + "' " +
                                                            "ORDER BY ABGRU ";

                                                            lOracleCmd.CommandText = lSQL;
                                                            lOracleCmd.Connection = cnCIS;
                                                            lOracleCmd.CommandTimeout = 1200;
                                                            lOrRst = lOracleCmd.ExecuteReader();
                                                            if (lOrRst.HasRows)
                                                            {
                                                                lOrRst.Read();
                                                                lSOStatus = (lOrRst.GetString(0) == null ? "" : lOrRst.GetString(0)).Trim();
                                                            }
                                                            lOrRst.Close();

                                                            if (lSOStatus == "")
                                                            {
                                                                Thread.Sleep(300);
                                                                CreateSOinSAP lSAP = new CreateSOinSAP();
                                                                int lResult = lSAP.CancelSAPSO(CustomerCode, ProjectCode, JobID, lJob.PONumber, lBBS[i].BBSNo, lSONo);
                                                                //return no of items in the order
                                                                if (lResult < 0)
                                                                {
                                                                    lErrorMsg = "Cannot cancel So in SAP.";
                                                                    lReturn = false;
                                                                    break;
                                                                }
                                                                lSAP = null;

                                                                lSOStatus = "";
                                                                lSQL = "SELECT ABGRU " +
                                                                "FROM SAPSR3.VBAP " +
                                                                "WHERE MANDT = '" + strClient + "' " +
                                                                "AND VBELN = '" + lSONo + "' " +
                                                                "ORDER BY ABGRU ";

                                                                lOracleCmd.CommandText = lSQL;
                                                                lOracleCmd.Connection = cnCIS;
                                                                lOracleCmd.CommandTimeout = 1200;
                                                                lOrRst = lOracleCmd.ExecuteReader();
                                                                if (lOrRst.HasRows)
                                                                {
                                                                    lOrRst.Read();
                                                                    lSOStatus = (lOrRst.GetString(0) == null ? "" : lOrRst.GetString(0)).Trim();
                                                                }
                                                                lOrRst.Close();
                                                                if (lSOStatus == "")
                                                                {
                                                                    lReturn = false;
                                                                    lErrorMsg = "SAP Error: cannot cancel SO in SAP.";
                                                                }
                                                            }
                                                        }
                                                    }

                                                    if (oracleTransCIS != null) oracleTransCIS.Commit();
                                                    CloseCISConnection(ref cnCIS);
                                                }
                                            }
                                        }
                                    }
                                }

                                var lJobNew = lJob;
                                if (ActionType == "Cancel")
                                {
                                    lJobNew.OrderStatus = "Cancelled";
                                }
                                else
                                {
                                    lJobNew.OrderStatus = "Submitted";
                                }
                                lJobNew.UpdateDate = DateTime.Now;
                                db.Entry(lJob).CurrentValues.SetValues(lJobNew);
                            }
                            if (ProdType == "Standard MESH" || ProdType == "STANDARD-MESH" || ProdType == "STANDARD-BAR" || ProdType == "COIL" || ProdType == "COUPLER")
                            {
                                //Check the order status in NDS
                                var lStdSheetJob = db.StdSheetJobAdvice.Find(CustomerCode, ProjectCode, JobID);
                                if (lStdSheetJob != null && lStdSheetJob.SAPSONo != null && lStdSheetJob.SAPSONo != "")
                                {
                                    var lSOArr = lStdSheetJob.SAPSONo.Split(',');
                                    if (lSOArr.Length > 0)
                                    {
                                        for (int i = 0; i < lSOArr.Length; i++)
                                        {
                                            lSAPSO = lSAPSO + lSOArr[i];

                                            OpenCISConnection(ref cnCIS);
                                            if (cnCIS.State != ConnectionState.Open)
                                            {
                                                lReturn = false;
                                            }
                                            else
                                            {
                                                oracleTransCIS = cnCIS.BeginTransaction(IsolationLevel.ReadCommitted);
                                                string lSOStatus = "";
                                                lSQL = "SELECT ABGRU " +
                                                "FROM SAPSR3.VBAP " +
                                                "WHERE MANDT = '" + strClient + "' " +
                                                "AND VBELN = '" + lSOArr[i] + "' " +
                                                "ORDER BY ABGRU ";

                                                var lOracleCmd = new OracleCommand(lSQL, cnCIS);
                                                lOracleCmd.CommandText = lSQL;
                                                lOracleCmd.CommandTimeout = 1200;
                                                lOrRst = lOracleCmd.ExecuteReader();
                                                if (lOrRst.HasRows)
                                                {
                                                    lOrRst.Read();
                                                    lSOStatus = (lOrRst.GetString(0) == null ? "" : lOrRst.GetString(0)).Trim();
                                                }
                                                lOrRst.Close();

                                                if (lSOStatus == "")
                                                {
                                                    CreateSOinSAP lSAP = new CreateSOinSAP();
                                                    int lResult = lSAP.CancelSAPSO(CustomerCode, ProjectCode, JobID, lStdSheetJob.PONumber, " ", lSOArr[i]);
                                                    //return no of items in the order
                                                    if (lResult < 0)
                                                    {
                                                        lErrorMsg = "Cannot cancel SO in SAP.";
                                                        lReturn = false;
                                                    }
                                                    lSAP = null;

                                                    lSOStatus = "";
                                                    lSQL = "SELECT ABGRU " +
                                                    "FROM SAPSR3.VBAP " +
                                                    "WHERE MANDT = '" + strClient + "' " +
                                                    "AND VBELN = '" + lSOArr[i] + "' " +
                                                    "ORDER BY ABGRU ";

                                                    lOracleCmd.CommandText = lSQL;
                                                    lOracleCmd.Connection = cnCIS;
                                                    lOracleCmd.CommandTimeout = 1200;
                                                    lOrRst = lOracleCmd.ExecuteReader();
                                                    if (lOrRst.HasRows)
                                                    {
                                                        lOrRst.Read();
                                                        lSOStatus = (lOrRst.GetString(0) == null ? "" : lOrRst.GetString(0)).Trim();
                                                    }
                                                    lOrRst.Close();
                                                    if (lSOStatus == "")
                                                    {
                                                        lReturn = false;
                                                        lErrorMsg = "SAP Error: cannot cancel SO in SAP.";
                                                    }

                                                }
                                                if (lReturn == true)
                                                {
                                                    //lReturn = UpdateSAPHist(strClient, lSOArr[i], " ", 0, 1, "U");

                                                    #region Cancell SAP CIS
                                                    //updade request header
                                                    lSQL = "UPDATE SAPSR3.YMSDT_ORDER_HDR " +
                                                    "SET STATUS = 'X', PO_NUMBER = PO_NUMBER || '-CXL' " +
                                                    "WHERE ORDER_REQUEST_NO = '" + lSOArr[i] + "' " +
                                                    "AND MANDT = '" + strClient + "' ";

                                                    lOracleCmd = new OracleCommand(lSQL, cnCIS);
                                                    lOracleCmd.CommandTimeout = 1200;
                                                    lOracleCmd.Transaction = oracleTransIDB;
                                                    lOracleCmd.ExecuteNonQuery();

                                                    //updade request header
                                                    lSQL = "UPDATE SAPSR3.YMSDT_REQ_DETAIL SET STATUS = 'X' " +
                                                    "WHERE ORDER_REQUEST_NO = '" + lSOArr[i] + "' " +
                                                    "AND MANDT = '" + strClient + "' ";

                                                    lOracleCmd = new OracleCommand(lSQL, cnCIS);
                                                    lOracleCmd.CommandTimeout = 1200;
                                                    lOracleCmd.Transaction = oracleTransIDB;
                                                    lOracleCmd.ExecuteNonQuery();
                                                    #endregion
                                                }

                                                if (lReturn == true)
                                                {
                                                    if (oracleTransCIS != null) oracleTransCIS.Commit();
                                                }
                                                else
                                                {
                                                    if (oracleTransCIS != null) oracleTransCIS.Rollback();
                                                }
                                                CloseCISConnection(ref cnCIS);

                                                OpenIDBConnection(ref cnIDB);

                                                oracleTransIDB = cnIDB.BeginTransaction(IsolationLevel.ReadCommitted);
                                                //Added on 2019-05-07 as found duplicated record in IDB -- consult Soo Long
                                                #region Cancel SOR in IDB
                                                lSQL = "UPDATE ORDER_HEADER SET CANCELLATION_STATUS = 'C' " +
                                                    "WHERE ORDER_REQUEST_NO = '" + lSOArr[i] + "' ";

                                                lOracleCmd = new OracleCommand(lSQL, cnIDB);
                                                lOracleCmd.CommandTimeout = 1200;
                                                lOracleCmd.Transaction = oracleTransIDB;
                                                lOracleCmd.ExecuteNonQuery();

                                                lSQL = "UPDATE ORDER_ITEM SET CANCELLATION_STATUS = 'C' " +
                                                "WHERE ORDER_REQUEST_NO = '" + lSOArr[i] + "' ";

                                                lOracleCmd = new OracleCommand(lSQL, cnIDB);
                                                lOracleCmd.CommandTimeout = 1200;
                                                lOracleCmd.Transaction = oracleTransIDB;
                                                lOracleCmd.ExecuteNonQuery();

                                                #endregion
                                                if (lReturn == true)
                                                {
                                                    if (oracleTransIDB != null) oracleTransIDB.Commit();
                                                }
                                                else
                                                {
                                                    if (oracleTransIDB != null) oracleTransIDB.Rollback();
                                                }
                                                CloseIDBConnection(ref cnIDB);

                                            }
                                        }
                                    }
                                }

                                if (lReturn == true)
                                {
                                    var lJobNew = lStdSheetJob;
                                    if (ActionType == "Cancel")
                                    {
                                        lJobNew.OrderStatus = "Cancelled";
                                    }
                                    else
                                    {
                                        lJobNew.OrderStatus = "Submitted";
                                    }
                                    lJobNew.UpdateDate = DateTime.Now;
                                    db.Entry(lStdSheetJob).CurrentValues.SetValues(lJobNew);
                                }
                            }
                            if (ProdType == "MESH" || ProdType == "STIRRUP-LINK-MESH" || ProdType == "COLUMN-LINK-MESH" || ProdType == "CUT-TO-SIZE-MESH")
                            {
                                string lBBSNoHeader = "";
                                //Check the order status in NDS
                                var lJob = db.CTSMESHJobAdvice.Find(CustomerCode, ProjectCode, JobID);

                                if (StructureElement == null || StructureElement == "")
                                {
                                    StructureElement = "NONWBS";
                                }
                                if (ScheduledProd == null || ScheduledProd == "")
                                {
                                    ScheduledProd = "N";
                                }

                                var lProcess = db.Process.Find(CustomerCode, ProjectCode, lOrderNumber, StructureElement, ProdType, ScheduledProd);
                                if (lProcess == null)
                                {
                                    //lReturn = false;
                                    //lErrorMsg = "Cannot read process table.";
                                    //return Json(new { success = lReturn, message = lErrorMsg });
                                }
                                else
                                {
                                    lWBS1 = lProcess.WBS1;
                                    lWBS2 = lProcess.WBS2;
                                    lWBS3 = lProcess.WBS3;
                                }

                                if (lWBS1 == null) lWBS1 = "";
                                if (lWBS2 == null) lWBS2 = "";
                                if (lWBS3 == null) lWBS3 = "";

                                var lBBSStdSheet = (from p in db.CTSMESHBBS
                                                    where p.CustomerCode == CustomerCode &&
                                                    p.ProjectCode == ProjectCode &&
                                                    p.JobID == JobID &&
                                                    p.BBSTotalWT > 0 &&
                                                    p.BBSOrder == true &&
                                                    p.BBSID == 8
                                                    select p).ToList();
                                if (lBBSStdSheet.Count > 0)
                                {
                                    for (int i = 0; i < lBBSStdSheet.Count; i++)
                                    {
                                        if (lBBSStdSheet[i].BBSSOR != null && lBBSStdSheet[i].BBSSOR != "")
                                        {
                                            lSAPSO = lSAPSO + lBBSStdSheet[i].BBSSOR;

                                            OpenCISConnection(ref cnCIS);
                                            if (cnCIS.State != ConnectionState.Open)
                                            {
                                                lErrorMsg = "Cannot connect to SAP Oracle database";
                                                lReturn = false;
                                            }
                                            else
                                            {
                                                oracleTransCIS = cnCIS.BeginTransaction(IsolationLevel.ReadCommitted);
                                                string lSOStatus = "";
                                                lSQL = "SELECT ABGRU " +
                                                "FROM SAPSR3.VBAP " +
                                                "WHERE MANDT = '" + strClient + "' " +
                                                "AND VBELN = '" + lBBSStdSheet[i].BBSSOR + "' " +
                                                "ORDER BY ABGRU ";

                                                var lOracleCmd = new OracleCommand(lSQL, cnCIS);
                                                lOracleCmd.CommandText = lSQL;
                                                lOracleCmd.CommandTimeout = 1200;
                                                lOrRst = lOracleCmd.ExecuteReader();
                                                if (lOrRst.HasRows)
                                                {
                                                    lOrRst.Read();
                                                    lSOStatus = (lOrRst.GetString(0) == null ? "" : lOrRst.GetString(0)).Trim();
                                                }
                                                lOrRst.Close();

                                                if (lSOStatus == "")
                                                {
                                                    CreateSOinSAP lSAP = new CreateSOinSAP();
                                                    int lResult = lSAP.CancelSAPSO(CustomerCode, ProjectCode, JobID, lJob.PONumber, " ", lBBSStdSheet[i].BBSSOR);
                                                    //return no of items in the order
                                                    if (lResult < 0)
                                                    {
                                                        lReturn = false;
                                                        lErrorMsg = "Cancel order from SAP fialed. Please check the order in SAP.";
                                                        break;
                                                    }
                                                    lSAP = null;

                                                    lSOStatus = "";
                                                    lSQL = "SELECT ABGRU " +
                                                    "FROM SAPSR3.VBAP " +
                                                    "WHERE MANDT = '" + strClient + "' " +
                                                    "AND VBELN = '" + lBBSStdSheet[i].BBSSOR + "' " +
                                                    "ORDER BY ABGRU ";

                                                    lOracleCmd.CommandText = lSQL;
                                                    lOracleCmd.Connection = cnCIS;
                                                    lOracleCmd.CommandTimeout = 1200;
                                                    lOrRst = lOracleCmd.ExecuteReader();
                                                    if (lOrRst.HasRows)
                                                    {
                                                        lOrRst.Read();
                                                        lSOStatus = (lOrRst.GetString(0) == null ? "" : lOrRst.GetString(0)).Trim();
                                                    }
                                                    lOrRst.Close();
                                                    if (lSOStatus == "")
                                                    {
                                                        lReturn = false;
                                                        lErrorMsg = "SAP Error: cannot cancel SO in SAP.";
                                                    }
                                                }

                                                if (lReturn == true)
                                                {
                                                    //lReturn = UpdateSAPHist(strClient, lBBSStdSheet[i].BBSSOR, "", 0, 1, "U");

                                                    #region Cancell SAP CIS
                                                    //updade request header
                                                    lSQL = "UPDATE SAPSR3.YMSDT_ORDER_HDR " +
                                                    "SET STATUS = 'X', PO_NUMBER = PO_NUMBER || '-CXL' " +
                                                    "WHERE ORDER_REQUEST_NO = '" + lBBSStdSheet[i].BBSSOR + "' " +
                                                    "AND MANDT = '" + strClient + "' ";

                                                    lOracleCmd = new OracleCommand(lSQL, cnCIS);
                                                    lOracleCmd.CommandTimeout = 1200;
                                                    lOracleCmd.Transaction = oracleTransIDB;
                                                    lOracleCmd.ExecuteNonQuery();

                                                    //updade request header
                                                    lSQL = "UPDATE SAPSR3.YMSDT_REQ_DETAIL SET STATUS = 'X' " +
                                                    "WHERE ORDER_REQUEST_NO = '" + lBBSStdSheet[i].BBSSOR + "' " +
                                                    "AND MANDT = '" + strClient + "' ";

                                                    lOracleCmd = new OracleCommand(lSQL, cnCIS);
                                                    lOracleCmd.CommandTimeout = 1200;
                                                    lOracleCmd.Transaction = oracleTransIDB;
                                                    lOracleCmd.ExecuteNonQuery();
                                                    #endregion
                                                }

                                                if (lReturn == true)
                                                {
                                                    if (oracleTransCIS != null) oracleTransCIS.Commit();
                                                }
                                                else
                                                {
                                                    if (oracleTransCIS != null) oracleTransCIS.Rollback();
                                                }
                                                CloseCISConnection(ref cnCIS);

                                                OpenIDBConnection(ref cnIDB);

                                                oracleTransIDB = cnIDB.BeginTransaction(IsolationLevel.ReadCommitted);
                                                //Added on 2019-05-07 as found duplicated record in IDB -- consult Soo Long
                                                #region Cancel SOR in IDB
                                                lSQL = "UPDATE ORDER_HEADER SET CANCELLATION_STATUS = 'C' " +
                                                    "WHERE ORDER_REQUEST_NO = '" + lBBSStdSheet[i].BBSSOR + "' ";

                                                lOracleCmd = new OracleCommand(lSQL, cnIDB);
                                                lOracleCmd.CommandTimeout = 1200;
                                                lOracleCmd.Transaction = oracleTransIDB;
                                                lOracleCmd.ExecuteNonQuery();

                                                lSQL = "UPDATE ORDER_ITEM SET CANCELLATION_STATUS = 'C' " +
                                                "WHERE ORDER_REQUEST_NO = '" + lBBSStdSheet[i].BBSSOR + "' ";

                                                lOracleCmd = new OracleCommand(lSQL, cnIDB);
                                                lOracleCmd.CommandTimeout = 1200;
                                                lOracleCmd.Transaction = oracleTransIDB;
                                                lOracleCmd.ExecuteNonQuery();

                                                #endregion
                                                if (lReturn == true)
                                                {
                                                    if (oracleTransIDB != null) oracleTransIDB.Commit();
                                                }
                                                else
                                                {
                                                    if (oracleTransIDB != null) oracleTransIDB.Rollback();
                                                }
                                                CloseIDBConnection(ref cnIDB);
                                            }
                                        }
                                    }
                                }

                                ///////////////////////////////////
                                //Cancel order for customer order
                                //1. Remove Group marking from BBS Posting
                                //2. 
                                ///////////////////////////////////
                                var lBBSCust = (from p in db.CTSMESHBBS
                                                where p.CustomerCode == CustomerCode &&
                                                p.ProjectCode == ProjectCode &&
                                                p.JobID == JobID &&
                                                p.BBSTotalWT > 0
                                                select p).ToList();
                                if (lBBSCust.Count > 0)
                                {
                                    for (int i = 0; i < lBBSCust.Count; i++)
                                    {
                                        if (lBBSCust[i].BBSNDSPostID > 0)
                                        {
                                            strStruElement = lBBSCust[i].BBSStrucElem;
                                            if (strStruElement == null || strStruElement == "")
                                            {
                                                strStruElement = "Slab";
                                            }

                                            OpenNDSConnection(ref cnNDS);
                                            if (cnNDS.State != ConnectionState.Open)
                                            {
                                                lErrorMsg = "Cannot connect to NDS database";
                                                lReturn = false;
                                            }
                                            else
                                            {
                                                //get project id
                                                lProjectID = 0;
                                                //lSQL = "SELECT intProjectId FROM dbo.ProjectMaster WHERE vchProjectCode = '" + ProjectCode + "' ";
                                                lSQL = "SELECT P.intProjectId FROM dbo.SAPProjectMaster P WHERE P.vchProjectCode = '" + ProjectCode + "' ";
                        

                                                lCmd = new SqlCommand(lSQL, cnNDS);
                                                lCmd.CommandTimeout = 1200;
                                                lRst = lCmd.ExecuteReader();
                                                if (lRst.HasRows)
                                                {
                                                    lRst.Read();
                                                    lProjectID = lRst.GetInt32(0);
                                                }
                                                lRst.Close();

                                                //check release status
                                                var lStatus = 0;
                                                lWBSElementID = 0;

                                                lSQL = "SELECT R.tntStatusId, M.intWBSElementId, M.vchBBSNo " +
                                                "FROM dbo.BBSReleaseDetails R, " +
                                                "dbo.BBSPostHeader M " +
                                                "WHERE R.intPostHeaderid = M.intPostHeaderId " +
                                                "AND M.intPostHeaderId = " + lBBSCust[i].BBSNDSPostID.ToString() + " ";

                                                lCmd = new SqlCommand(lSQL, cnNDS);
                                                lCmd.CommandTimeout = 1200;
                                                lRst = lCmd.ExecuteReader();
                                                if (lRst.HasRows)
                                                {
                                                    lRst.Read();
                                                    lStatus = lRst.GetByte(0);
                                                    lWBSElementID = (int)lRst.GetValue(1);
                                                    lBBSNoHeader = (string)lRst.GetValue(2);
                                                }
                                                lRst.Close();

                                                if (lStatus == 12)  //Released
                                                {
                                                    //CloseNDSConnection(ref cnNDS);
                                                    //lErrorMsg = "The WBS already released. It cannot be cancelled. Please cancell the order using NDS first.";
                                                    //lReturn = false;

                                                    lSQL = "SELECT BBS_NO FROM dbo.SAP_REQUEST_DETAILS " +
                                                                "WHERE ORD_REQ_NO = '" + lBBSCust[i].BBSSOR + "' ";
                                                    lCmd = new SqlCommand(lSQL, cnNDS);
                                                    //lCmd.Transaction = osqlTransNDS;
                                                    lCmd.CommandTimeout = 1200;
                                                    lRst = lCmd.ExecuteReader();
                                                    if (lRst.HasRows)
                                                    {
                                                        lRst.Read();
                                                        lBBSNoHeader = (string)lRst.GetValue(0);
                                                    }
                                                    lRst.Close();

                                                    lSQL = "dbo.BBSReleaseBySOR_Insert ";
                                                    lCmd = new SqlCommand(lSQL, cnNDS);
                                                    lCmd.Transaction = osqlTransNDS;
                                                    lCmd.CommandTimeout = 1200;
                                                    lCmd.CommandType = CommandType.StoredProcedure;
                                                    lCmd.Parameters.Add("@ORD_REQ_NO", SqlDbType.Char);
                                                    lCmd.Parameters.Add("@IntProjectID", SqlDbType.Int);
                                                    lCmd.Parameters.Add("@IntWBSElementID", SqlDbType.Int);
                                                    lCmd.Parameters.Add("@BBS_NO", SqlDbType.Char);
                                                    lCmd.Parameters.Add("@chrBBSStatus", SqlDbType.Char);
                                                    lCmd.Parameters.Add("@UserID", SqlDbType.Int);

                                                    lCmd.Parameters["@ORD_REQ_NO"].Value = lBBSCust[i].BBSSOR;
                                                    lCmd.Parameters["@IntProjectID"].Value = lProjectID;
                                                    lCmd.Parameters["@IntWBSElementID"].Value = lWBSElementID;
                                                    lCmd.Parameters["@BBS_NO"].Value = lBBSNoHeader;
                                                    lCmd.Parameters["@chrBBSStatus"].Value = "C";
                                                    lCmd.Parameters["@UserID"].Value = 11;
                                                    lCmd.CommandTimeout = 1200;
                                                    lCmd.ExecuteNonQuery();
                                                }

                                                if (lReturn == true)
                                                {
                                                   // var lRet = RemoveGroupMarking(CustomerCode, ProjectCode, ContractNo, lBBSCust[i].BBSNDSPostID,
                                                    //strStruElement, "MSH", lBBSCust[i].BBSNDSGroupMark, 0);
                                                    //if (lRet < 0)
                                                    //{
                                                    //    lErrorMsg = "Error on removing group marking from BBS Posting";
                                                    //    lReturn = false;

                                                    //}ajit
                                                }

                                                if (lReturn == true && lBBSCust[i].BBSNDSGroupMark != null && lBBSCust[i].BBSNDSGroupMark != "" && lStatus != 14)  //Cancelled
                                                {
                                                    // get group marking ID and SE Level ID
                                                    lSQL = "SELECT G.intGroupMarkId, S.intSEDetailingId " +
                                                    "FROM dbo.GroupMarkingDetails G, dbo.SELevelDetails S  " +
                                                    "WHERE G.intGroupMarkId = S.intGroupMarkId " +
                                                    "AND G.intProjectid = " + lProjectID.ToString() + " " +
                                                    "and S.intStructureElementTypeId in " +
                                                    "(SELECT intStructureElementTypeId " +
                                                    "FROM dbo.StructureElementMaster " +
                                                    "WHERE vchStructureElementType = '" + strStruElement + "' ) " +
                                                    "and S.sitProductTypeId in " +
                                                    "(SELECT sitProductTypeID " +
                                                    "FROM dbo.ProductTypeMaster " +
                                                    "WHERE vchProductType = 'MSH' ) " +
                                                    "AND vchGroupMarkingName = '" + lBBSCust[i].BBSNDSGroupMark + "' " +
                                                    "AND tntGroupRevNo = 0 ";

                                                    lCmd = new SqlCommand(lSQL, cnNDS);
                                                    lCmd.CommandTimeout = 1200;
                                                    lRst = lCmd.ExecuteReader();
                                                    if (lRst.HasRows)
                                                    {
                                                        lRst.Read();
                                                        lGroupMarkingID = lRst.GetInt32(0);
                                                        lSELevelID = lRst.GetInt32(1);
                                                    }

                                                    lRst.Close();

                                                    if (lGroupMarkingID > 0 && lSELevelID > 0)
                                                    {
                                                        if (strStruElement.ToUpper() == "BEAM")
                                                        {
                                                            lSQL = "DELETE FROM ProductMarkingDetails WHERE intstructuremarkid IN " +
                                                                "(SELECT intstructuremarkid FROM StructureMarkingDetails " +
                                                                "WHERE intSEDetailingId = " + lSELevelID + ") ";
                                                            lCmd = new SqlCommand(lSQL, cnNDS);
                                                            lCmd.CommandTimeout = 1200;
                                                            lCmd.ExecuteNonQuery();

                                                            lSQL = "DELETE FROM StructureMarkingDetails " +
                                                                "WHERE intSEDetailingId = " + lSELevelID + "  ";
                                                            lCmd = new SqlCommand(lSQL, cnNDS);
                                                            lCmd.CommandTimeout = 1200;
                                                            lCmd.ExecuteNonQuery();
                                                        }
                                                        else if (strStruElement.ToUpper() == "COLUMN")
                                                        {
                                                            lSQL = "DELETE FROM dbo.ColumnCrossWireDetails " +
                                                                "WHERE intProductMarkId IN " +
                                                                "(SELECT intProductMarkId " +
                                                                "FROM ColumnProductMarkingDetails WHERE intstructuremarkid IN " +
                                                                "(SELECT intstructuremarkid FROM ColumnStructureMarkingDetails " +
                                                                "WHERE intSEDetailingId = " + lSELevelID + ")) ";
                                                            lCmd = new SqlCommand(lSQL, cnNDS);
                                                            lCmd.CommandTimeout = 1200;
                                                            lCmd.ExecuteNonQuery();

                                                            lSQL = "DELETE FROM ColumnProductMarkingDetails WHERE intstructuremarkid IN " +
                                                                "(SELECT intstructuremarkid FROM ColumnStructureMarkingDetails " +
                                                                "WHERE intSEDetailingId = " + lSELevelID + ") ";
                                                            lCmd = new SqlCommand(lSQL, cnNDS);
                                                            lCmd.CommandTimeout = 1200;
                                                            lCmd.ExecuteNonQuery();

                                                            lSQL = "DELETE FROM ColumnStructureMarkingDetails " +
                                                                "WHERE intSEDetailingId = " + lSELevelID + "  ";
                                                            lCmd = new SqlCommand(lSQL, cnNDS);
                                                            lCmd.CommandTimeout = 1200;
                                                            lCmd.ExecuteNonQuery();
                                                        }
                                                        else
                                                        {
                                                            lSQL = "DELETE FROM SHAPEDETAILINGTRANS WHERE intshapetransheaderid IN " +
                                                                "(SELECT intShapeTransHeaderId FROM meshproductmarkingdetails " +
                                                                "WHERE intstructuremarkid IN " +
                                                                "(SELECT intstructuremarkid FROM meshstructuremarkingdetails " +
                                                                "WHERE intSEDetailingId = " + lSELevelID + ")) ";
                                                            lCmd = new SqlCommand(lSQL, cnNDS);
                                                            lCmd.CommandTimeout = 1200;
                                                            lCmd.ExecuteNonQuery();

                                                            lSQL = "DELETE FROM ShapeDetailingTransHeader WHERE intshapetransheaderid IN " +
                                                                "(SELECT intShapeTransHeaderId FROM meshproductmarkingdetails " +
                                                                "WHERE intstructuremarkid IN " +
                                                                "(SELECT intstructuremarkid FROM meshstructuremarkingdetails " +
                                                                "WHERE intSEDetailingId = " + lSELevelID + ")) ";
                                                            lCmd = new SqlCommand(lSQL, cnNDS);
                                                            lCmd.CommandTimeout = 1200;
                                                            lCmd.ExecuteNonQuery();

                                                            lSQL = "DELETE FROM meshproductmarkingdetails WHERE intstructuremarkid IN " +
                                                                "(SELECT intstructuremarkid FROM meshstructuremarkingdetails " +
                                                                "WHERE intSEDetailingId = " + lSELevelID + ") ";
                                                            lCmd = new SqlCommand(lSQL, cnNDS);
                                                            lCmd.CommandTimeout = 1200;
                                                            lCmd.ExecuteNonQuery();

                                                            lSQL = "DELETE FROM meshstructuremarkingdetails " +
                                                                "WHERE intSEDetailingId = " + lSELevelID + "  ";
                                                            lCmd = new SqlCommand(lSQL, cnNDS);
                                                            lCmd.CommandTimeout = 1200;
                                                            lCmd.ExecuteNonQuery();
                                                        }
                                                        //delete from group marking
                                                        lSQL = "DELETE FROM dbo.GroupMarkingDetails " +
                                                        "WHERE intGroupMarkId = " + lGroupMarkingID.ToString() + " ";

                                                        lCmd = new SqlCommand(lSQL, cnNDS);
                                                        lCmd.CommandTimeout = 1200;
                                                        lCmd.ExecuteNonQuery();

                                                        //delete from SE Level
                                                        lSQL = "DELETE FROM dbo.SELevelDetails " +
                                                        "WHERE intSEDetailingId = " + lSELevelID.ToString() + " ";

                                                        lCmd = new SqlCommand(lSQL, cnNDS);
                                                        lCmd.CommandTimeout = 1200;
                                                        lCmd.ExecuteNonQuery();

                                                        //delete from request details
                                                        lSQL = "UPDATE SAP_REQUEST_DETAILS SET STATUS = 'X' " +
                                                        "WHERE ORD_REQ_NO = '" + lBBSCust[i].BBSSOR + "' ";

                                                        lCmd = new SqlCommand(lSQL, cnNDS);
                                                        lCmd.CommandTimeout = 1200;
                                                        lCmd.ExecuteNonQuery();

                                                        //delete from request header
                                                        lSQL = "UPDATE SAP_REQUEST_HDR SET STATUS = 'X', PO_NUM = PO_NUM + '-CXL' " +
                                                        "WHERE ORD_REQ_NO = '" + lBBSCust[i].BBSSOR + "' ";

                                                        lCmd = new SqlCommand(lSQL, cnNDS);
                                                        lCmd.CommandTimeout = 1200;
                                                        lCmd.ExecuteNonQuery();
                                                    }
                                                }
                                                CloseNDSConnection(ref cnNDS);
                                            }

                                            if (lReturn == true)
                                            {
                                                OpenIDBConnection(ref cnIDB);
                                                if (cnIDB.State != ConnectionState.Open)
                                                {
                                                    lReturn = false;
                                                }
                                                else
                                                {
                                                    if (lBBSCust[i].BBSSOR != null && lBBSCust[i].BBSSOR != "")
                                                    {
                                                        lSQL = "UPDATE ORDER_HEADER SET CANCELLATION_STATUS = 'C' " +
                                                        "WHERE ORDER_REQUEST_NO = '" + lBBSCust[i].BBSSOR + "' ";

                                                        var lOracleCmd = new OracleCommand(lSQL, cnIDB);
                                                        lOracleCmd.CommandTimeout = 1200;
                                                        lOracleCmd.ExecuteNonQuery();

                                                        lSQL = "UPDATE ORDER_ITEM SET CANCELLATION_STATUS = 'C' " +
                                                        "WHERE ORDER_REQUEST_NO = '" + lBBSCust[i].BBSSOR + "' ";

                                                        lOracleCmd = new OracleCommand(lSQL, cnIDB);
                                                        lOracleCmd.CommandTimeout = 1200;
                                                        lOracleCmd.ExecuteNonQuery();
                                                    }
                                                    CloseIDBConnection(ref cnIDB);
                                                }
                                            }

                                            if (lReturn == true)
                                            {
                                                OpenCISConnection(ref cnCIS);
                                                if (cnCIS.State != ConnectionState.Open)
                                                {
                                                    lReturn = false;
                                                }
                                                else
                                                {
                                                    if (lBBSCust[i].BBSSOR != null && lBBSCust[i].BBSSOR != "")
                                                    {
                                                        //updade request header
                                                        lSQL = "UPDATE SAPSR3.YMSDT_ORDER_HDR " +
                                                        "SET STATUS = 'X', PO_NUMBER = PO_NUMBER || '-CXL' " +
                                                        "WHERE ORDER_REQUEST_NO = '" + lBBSCust[i].BBSSOR + "' " +
                                                        "AND MANDT = '" + strClient + "' ";

                                                        var lOracleCmd = new OracleCommand(lSQL, cnCIS);
                                                        lOracleCmd.CommandTimeout = 1200;
                                                        lOracleCmd.ExecuteNonQuery();

                                                        //updade request header
                                                        lSQL = "UPDATE SAPSR3.YMSDT_REQ_DETAIL SET STATUS = 'X' " +
                                                        "WHERE ORDER_REQUEST_NO = '" + lBBSCust[i].BBSSOR + "' " +
                                                        "AND MANDT = '" + strClient + "' ";

                                                        lOracleCmd = new OracleCommand(lSQL, cnCIS);
                                                        lOracleCmd.CommandTimeout = 1200;
                                                        lOracleCmd.ExecuteNonQuery();

                                                        // Cancel SAP SO if created
                                                        string lSONo = "";
                                                        lSQL = "SELECT VBELN " +
                                                        "FROM SAPSR3.VBAK " +
                                                        "WHERE MANDT = '" + strClient + "' " +
                                                        "AND IHREZ = '" + lBBSCust[i].BBSSOR + "' ";

                                                        lOracleCmd.CommandText = lSQL;
                                                        lOracleCmd.Connection = cnCIS;
                                                        lOracleCmd.CommandTimeout = 1200;
                                                        lOrRst = lOracleCmd.ExecuteReader();
                                                        if (lOrRst.HasRows)
                                                        {
                                                            lOrRst.Read();
                                                            lSONo = (lOrRst.GetString(0) == null ? "" : lOrRst.GetString(0));
                                                        }
                                                        lOrRst.Close();

                                                        if (lSONo != "")
                                                        {
                                                            string lSOStatus = "";
                                                            lSQL = "SELECT ABGRU " +
                                                            "FROM SAPSR3.VBAP " +
                                                            "WHERE MANDT = '" + strClient + "' " +
                                                            "AND VBELN = '" + lSONo + "' " +
                                                            "ORDER BY ABGRU ";

                                                            lOracleCmd = new OracleCommand(lSQL, cnCIS);
                                                            lOracleCmd.CommandText = lSQL;
                                                            lOracleCmd.CommandTimeout = 1200;
                                                            lOrRst = lOracleCmd.ExecuteReader();
                                                            if (lOrRst.HasRows)
                                                            {
                                                                lOrRst.Read();
                                                                lSOStatus = (lOrRst.GetString(0) == null ? "" : lOrRst.GetString(0)).Trim();
                                                            }
                                                            lOrRst.Close();

                                                            if (lSOStatus == "")
                                                            {
                                                                CreateSOinSAP lSAP = new CreateSOinSAP();
                                                                int lResult = lSAP.CancelSAPSO(CustomerCode, ProjectCode, JobID, lJob.PONumber, lBBSNoHeader, lSONo);
                                                                //return no of items in the order
                                                                if (lResult < 0)
                                                                {
                                                                    lReturn = false;
                                                                    break;
                                                                }
                                                                lSAP = null;

                                                                lSOStatus = "";
                                                                lSQL = "SELECT ABGRU " +
                                                                "FROM SAPSR3.VBAP " +
                                                                "WHERE MANDT = '" + strClient + "' " +
                                                                "AND VBELN = '" + lSONo + "' " +
                                                                "ORDER BY ABGRU ";

                                                                lOracleCmd.CommandText = lSQL;
                                                                lOracleCmd.Connection = cnCIS;
                                                                lOracleCmd.CommandTimeout = 1200;
                                                                lOrRst = lOracleCmd.ExecuteReader();
                                                                if (lOrRst.HasRows)
                                                                {
                                                                    lOrRst.Read();
                                                                    lSOStatus = (lOrRst.GetString(0) == null ? "" : lOrRst.GetString(0)).Trim();
                                                                }
                                                                lOrRst.Close();
                                                                if (lSOStatus == "")
                                                                {
                                                                    lReturn = false;
                                                                    lErrorMsg = "SAP Error: cannot cancel SO in SAP.";
                                                                }
                                                            }
                                                        }

                                                    }
                                                    CloseCISConnection(ref cnCIS);
                                                }
                                            }
                                        }
                                    }
                                }

                                var lBBSNSH = (from p in db.CTSMESHBBSNSH
                                               where p.CustomerCode == CustomerCode &&
                                               p.ProjectCode == ProjectCode &&
                                               p.JobID == JobID &&
                                               p.BBSTotalWT > 0
                                               select p).ToList();
                                if (lBBSNSH.Count > 0)
                                {
                                    for (int i = 0; i < lBBSNSH.Count; i++)
                                    {
                                        //Check the same structure element with Customer records
                                        int lFound = 0;
                                        if (lBBSCust.Count > 0)
                                        {
                                            for (int j = 0; j < lBBSCust.Count; j++)
                                            {
                                                if (lBBSCust[j].BBSStrucElem == lBBSNSH[i].BBSStrucElem)
                                                {
                                                    lFound = 1;
                                                    break;
                                                }
                                            }
                                        }
                                        if (lFound == 1)
                                        {
                                            continue;
                                        }


                                        if (lBBSNSH[i].BBSNDSPostID > 0)
                                        {
                                            OpenNDSConnection(ref cnNDS);
                                            if (cnNDS.State != ConnectionState.Open)
                                            {
                                                lReturn = false;
                                            }
                                            else
                                            {
                                                //get project id
                                                lProjectID = 0;
                                                //lSQL = "SELECT intProjectId FROM dbo.ProjectMaster WHERE vchProjectCode = '" + ProjectCode + "' ";
                                                lSQL = "SELECT P.intProjectId FROM dbo.SAPProjectMaster P WHERE P.vchProjectCode = '" + ProjectCode + "' ";
  

                                                lCmd = new SqlCommand(lSQL, cnNDS);
                                                lCmd.CommandTimeout = 1200;
                                                lRst = lCmd.ExecuteReader();
                                                if (lRst.HasRows)
                                                {
                                                    lRst.Read();
                                                    lProjectID = lRst.GetInt32(0);
                                                }
                                                lRst.Close();


                                                //check release status
                                                var lStatus = 0;

                                                lSQL = "SELECT R.tntStatusId, M.intWBSElementId, M.vchBBSNo " +
                                                "FROM dbo.BBSReleaseDetails R, " +
                                                "dbo.BBSPostHeader M " +
                                                "WHERE R.intPostHeaderid = M.intPostHeaderId " +
                                                "AND M.intPostHeaderId = " + lBBSNSH[i].BBSNDSPostID.ToString() + " ";

                                                lCmd = new SqlCommand(lSQL, cnNDS);
                                                lCmd.CommandTimeout = 1200;
                                                lRst = lCmd.ExecuteReader();
                                                if (lRst.HasRows)
                                                {
                                                    lRst.Read();
                                                    lStatus = lRst.GetByte(0);
                                                    lWBSElementID = (int)lRst.GetValue(1);
                                                    lBBSNoHeader = (string)lRst.GetValue(2);
                                                }
                                                lRst.Close();
                                                if (lStatus == 12)  //Released
                                                {
                                                    //CloseNDSConnection(ref cnNDS);
                                                    //lReturn = false;

                                                    lSQL = "SELECT BBS_NO FROM dbo.SAP_REQUEST_DETAILS " +
                                                                "WHERE ORD_REQ_NO = '" + lBBSCust[i].BBSSOR + "' ";
                                                    lCmd = new SqlCommand(lSQL, cnNDS);
                                                    //lCmd.Transaction = osqlTransNDS;
                                                    lCmd.CommandTimeout = 1200;
                                                    lRst = lCmd.ExecuteReader();
                                                    if (lRst.HasRows)
                                                    {
                                                        lRst.Read();
                                                        lBBSNoHeader = (string)lRst.GetValue(0);
                                                    }
                                                    lRst.Close();

                                                    lSQL = "dbo.BBSReleaseBySOR_Insert ";
                                                    lCmd = new SqlCommand(lSQL, cnNDS);
                                                    lCmd.Transaction = osqlTransNDS;
                                                    lCmd.CommandTimeout = 1200;
                                                    lCmd.CommandType = CommandType.StoredProcedure;
                                                    lCmd.Parameters.Add("@ORD_REQ_NO", SqlDbType.Char);
                                                    lCmd.Parameters.Add("@IntProjectID", SqlDbType.Int);
                                                    lCmd.Parameters.Add("@IntWBSElementID", SqlDbType.Int);
                                                    lCmd.Parameters.Add("@BBS_NO", SqlDbType.Char);
                                                    lCmd.Parameters.Add("@chrBBSStatus", SqlDbType.Char);
                                                    lCmd.Parameters.Add("@UserID", SqlDbType.Int);

                                                    lCmd.Parameters["@ORD_REQ_NO"].Value = lBBSCust[i].BBSSOR;
                                                    lCmd.Parameters["@IntProjectID"].Value = lProjectID;
                                                    lCmd.Parameters["@IntWBSElementID"].Value = lWBSElementID;
                                                    lCmd.Parameters["@BBS_NO"].Value = lBBSNoHeader;
                                                    lCmd.Parameters["@chrBBSStatus"].Value = "C";
                                                    lCmd.Parameters["@UserID"].Value = 11;
                                                    lCmd.CommandTimeout = 1200;
                                                    lCmd.ExecuteNonQuery();
                                                }

                                                if (lReturn == true && lBBSNSH[i].BBSSOR != null && lBBSNSH[i].BBSSOR != "" && lStatus != 14)  //Cancelled
                                                {
                                                    //delete from request details
                                                    lSQL = "UPDATE SAP_REQUEST_DETAILS SET STATUS = 'X' " +
                                                    "WHERE ORD_REQ_NO = '" + lBBSNSH[i].BBSSOR + "' ";

                                                    lCmd = new SqlCommand(lSQL, cnNDS);
                                                    lCmd.CommandTimeout = 1200;
                                                    lCmd.ExecuteNonQuery();

                                                    //delete from request header
                                                    lSQL = "UPDATE SAP_REQUEST_HDR SET STATUS = 'X', PO_NUM = PO_NUM + '-CXL' " +
                                                    "WHERE ORD_REQ_NO = '" + lBBSNSH[i].BBSSOR + "' ";

                                                    lCmd = new SqlCommand(lSQL, cnNDS);
                                                    lCmd.CommandTimeout = 1200;
                                                    lCmd.ExecuteNonQuery();
                                                }

                                                CloseNDSConnection(ref cnNDS);
                                            }

                                            if (lReturn == true)
                                            {
                                                OpenIDBConnection(ref cnIDB);
                                                if (cnIDB.State != ConnectionState.Open)
                                                {
                                                    lReturn = false;
                                                }
                                                else
                                                {
                                                    if (lBBSNSH[i].BBSSOR != null && lBBSNSH[i].BBSSOR != "")
                                                    {
                                                        lSQL = "UPDATE ORDER_HEADER SET CANCELLATION_STATUS = 'C' " +
                                                        "WHERE ORDER_REQUEST_NO = '" + lBBSNSH[i].BBSSOR + "' ";

                                                        var lOracleCmd = new OracleCommand(lSQL, cnIDB);
                                                        lOracleCmd.CommandTimeout = 1200;
                                                        lOracleCmd.ExecuteNonQuery();

                                                        lSQL = "UPDATE ORDER_ITEM SET CANCELLATION_STATUS = 'C' " +
                                                        "WHERE ORDER_REQUEST_NO = '" + lBBSNSH[i].BBSSOR + "' ";

                                                        lOracleCmd = new OracleCommand(lSQL, cnIDB);
                                                        lOracleCmd.CommandTimeout = 1200;
                                                        lOracleCmd.ExecuteNonQuery();
                                                    }
                                                    CloseIDBConnection(ref cnIDB);
                                                }
                                            }

                                            if (lReturn == true)
                                            {
                                                OpenCISConnection(ref cnCIS);
                                                if (cnCIS.State != ConnectionState.Open)
                                                {
                                                    lReturn = false;
                                                }
                                                else
                                                {
                                                    if (lBBSNSH[i].BBSSOR != null && lBBSNSH[i].BBSSOR != "")
                                                    {
                                                        //updade request header
                                                        lSQL = "UPDATE SAPSR3.YMSDT_ORDER_HDR " +
                                                        "SET STATUS = 'X', PO_NUMBER = PO_NUMBER || '-CXL' " +
                                                        "WHERE ORDER_REQUEST_NO = '" + lBBSNSH[i].BBSSOR + "' " +
                                                        "AND MANDT = '" + strClient + "' ";

                                                        var lOracleCmd = new OracleCommand(lSQL, cnCIS);
                                                        lOracleCmd.CommandTimeout = 1200;
                                                        lOracleCmd.ExecuteNonQuery();

                                                        //updade request header
                                                        lSQL = "UPDATE SAPSR3.YMSDT_REQ_DETAIL SET STATUS = 'X' " +
                                                        "WHERE ORDER_REQUEST_NO = '" + lBBSNSH[i].BBSSOR + "' " +
                                                        "AND MANDT = '" + strClient + "' ";

                                                        lOracleCmd = new OracleCommand(lSQL, cnCIS);
                                                        lOracleCmd.CommandTimeout = 1200;
                                                        lOracleCmd.ExecuteNonQuery();

                                                        // Cancel SAP SO if created
                                                        string lSONo = "";
                                                        lSQL = "SELECT VBELN " +
                                                        "FROM SAPSR3.VBAK " +
                                                        "WHERE MANDT = '" + strClient + "' " +
                                                        "AND IHREZ = '" + lBBSCust[i].BBSSOR + "' ";

                                                        lOracleCmd.CommandText = lSQL;
                                                        lOracleCmd.Connection = cnCIS;
                                                        lOracleCmd.CommandTimeout = 1200;
                                                        lOrRst = lOracleCmd.ExecuteReader();
                                                        if (lOrRst.HasRows)
                                                        {
                                                            lOrRst.Read();
                                                            lSONo = (lOrRst.GetString(0) == null ? "" : lOrRst.GetString(0));
                                                        }
                                                        lOrRst.Close();

                                                        if (lSONo != "")
                                                        {
                                                            CreateSOinSAP lSAP = new CreateSOinSAP();
                                                            int lResult = lSAP.CancelSAPSO(CustomerCode, ProjectCode, JobID, lJob.PONumber, lBBSNoHeader, lSONo);
                                                            //return no of items in the order
                                                            if (lResult < 0)
                                                            {
                                                                lReturn = false;
                                                                break;
                                                            }
                                                        }

                                                    }
                                                    CloseCISConnection(ref cnCIS);
                                                }
                                            }
                                        }
                                    }
                                }
                                var lJobNew = lJob;
                                if (ActionType == "Cancel")
                                {
                                    lJobNew.OrderStatus = "Cancelled";
                                }
                                else
                                {
                                    lJobNew.OrderStatus = "Submitted";

                                }
                                lJobNew.UpdateDate = DateTime.Now;
                                db.Entry(lJob).CurrentValues.SetValues(lJobNew);
                            }
                            if (ProdType == "COMPONENT")
                            {
                                //Check the order status in NDS

                                if (StructureElement == null || StructureElement == "")
                                {
                                    StructureElement = "NONWBS";
                                }
                                if (ScheduledProd == null || ScheduledProd == "")
                                {
                                    ScheduledProd = "N";
                                }

                                var lSE = db.OrderProjectSE.Find(lOrderNumber, StructureElement, ProdType, ScheduledProd);

                                var lProcess = db.Process.Find(CustomerCode, ProjectCode, lOrderNumber, StructureElement, ProdType, ScheduledProd);
                                if (lProcess == null || lSE == null || lSE.OrderNumber == 0)
                                {
                                    //lReturn = false;
                                    //lErrorMsg = "Cannot read process table.";
                                    //return Json(new { success = lReturn, message = lErrorMsg });
                                }
                                else
                                {
                                    lWBS1 = lProcess.WBS1;
                                    lWBS2 = lProcess.WBS2;
                                    lWBS3 = lProcess.WBS3;
                                }

                                if (lWBS1 == null) lWBS1 = "";
                                if (lWBS2 == null) lWBS2 = "";
                                if (lWBS3 == null) lWBS3 = "";

                                var lSOR = (from p in db.ComponentSetOrder
                                            where p.CustomerCode == CustomerCode &&
                                            p.ProjectCode == ProjectCode &&
                                            p.OrderID == lOrderNumber
                                            select new
                                            {
                                                p.SAPSOR,
                                                p.PostHeaderID
                                            }
                                                    ).Distinct().ToList();

                                if (lSOR.Count > 0)
                                {
                                    for (int i = 0; i < lSOR.Count; i++)
                                    {
                                        if (lSOR[i].SAPSOR != null && lSOR[i].SAPSOR != "")
                                        {
                                            string lSOR1 = (string)lSOR[i].SAPSOR;

                                            OpenCISConnection(ref cnCIS);
                                            if (cnCIS.State != ConnectionState.Open)
                                            {
                                                lErrorMsg = "Cannot connect to SAP Oracle database";
                                                lReturn = false;
                                            }
                                            else
                                            {
                                                oracleTransCIS = cnCIS.BeginTransaction(IsolationLevel.ReadCommitted);

                                                #region Cancel SAP SO if any
                                                lSAPSO = "";

                                                lSQL = "SELECT sales_order " +
                                                "FROM sapsr3.ymsdt_order_hdr " +
                                                "WHERE MANDT = '" + strClient + "' " +
                                                "AND ORDER_REQUEST_NO = '" + lSOR1 + "' ";

                                                var lOracleCmd = new OracleCommand(lSQL, cnCIS);
                                                lOracleCmd.CommandText = lSQL;
                                                lOracleCmd.CommandTimeout = 1200;
                                                lOrRst = lOracleCmd.ExecuteReader();
                                                if (lOrRst.HasRows)
                                                {
                                                    lOrRst.Read();
                                                    lSAPSO = (lOrRst.GetString(0) == null ? "" : lOrRst.GetString(0)).Trim();
                                                }
                                                lOrRst.Close();

                                                if (lSAPSO != "")
                                                {
                                                    string lSOStatus = "";
                                                    lSQL = "SELECT ABGRU " +
                                                    "FROM SAPSR3.VBAP " +
                                                    "WHERE MANDT = '" + strClient + "' " +
                                                    "AND VBELN = '" + lSAPSO + "' " +
                                                    "ORDER BY ABGRU ";

                                                    lOracleCmd = new OracleCommand(lSQL, cnCIS);
                                                    lOracleCmd.CommandText = lSQL;
                                                    lOracleCmd.CommandTimeout = 1200;
                                                    lOrRst = lOracleCmd.ExecuteReader();
                                                    if (lOrRst.HasRows)
                                                    {
                                                        lOrRst.Read();
                                                        lSOStatus = (lOrRst.GetString(0) == null ? "" : lOrRst.GetString(0)).Trim();
                                                    }
                                                    lOrRst.Close();

                                                    if (lSOStatus == "")
                                                    {
                                                        CreateSOinSAP lSAP = new CreateSOinSAP();
                                                        int lResult = lSAP.CancelSAPSO(CustomerCode, ProjectCode, JobID, lSE.PONumber, " ", lSAPSO);
                                                        //return no of items in the order
                                                        if (lResult < 0)
                                                        {
                                                            lReturn = false;
                                                            lErrorMsg = "Cancel order from SAP fialed. Please check the order in SAP.";
                                                            break;
                                                        }
                                                        lSAP = null;

                                                        lSOStatus = "";
                                                        lSQL = "SELECT ABGRU " +
                                                        "FROM SAPSR3.VBAP " +
                                                        "WHERE MANDT = '" + strClient + "' " +
                                                        "AND VBELN = '" + lSAPSO + "' " +
                                                        "ORDER BY ABGRU ";

                                                        lOracleCmd.CommandText = lSQL;
                                                        lOracleCmd.Connection = cnCIS;
                                                        lOracleCmd.CommandTimeout = 1200;
                                                        lOrRst = lOracleCmd.ExecuteReader();
                                                        if (lOrRst.HasRows)
                                                        {
                                                            lOrRst.Read();
                                                            lSOStatus = (lOrRst.GetString(0) == null ? "" : lOrRst.GetString(0)).Trim();
                                                        }
                                                        lOrRst.Close();
                                                        if (lSOStatus == "")
                                                        {
                                                            lReturn = false;
                                                            lErrorMsg = "SAP Error: cannot cancel SO in SAP.";
                                                        }
                                                    }
                                                }
                                                #endregion

                                                #region Cancel Order in SAP Y
                                                if (lReturn == true)
                                                {
                                                    //lReturn = UpdateSAPHist(strClient, lBBSStdSheet[i].BBSSOR, "", 0, 1, "U");

                                                    #region Cancell SAP CIS
                                                    //updade request header
                                                    lSQL = "UPDATE SAPSR3.YMSDT_ORDER_HDR " +
                                                    "SET STATUS = 'X', PO_NUMBER = PO_NUMBER || '-CXL' " +
                                                    "WHERE ORDER_REQUEST_NO = '" + lSOR1 + "' " +
                                                    "AND MANDT = '" + strClient + "' ";

                                                    lOracleCmd = new OracleCommand(lSQL, cnCIS);
                                                    lOracleCmd.CommandTimeout = 1200;
                                                    lOracleCmd.Transaction = oracleTransIDB;
                                                    lOracleCmd.ExecuteNonQuery();

                                                    //updade request header
                                                    lSQL = "UPDATE SAPSR3.YMSDT_REQ_DETAIL SET STATUS = 'X' " +
                                                    "WHERE ORDER_REQUEST_NO = '" + lSOR1 + "' " +
                                                    "AND MANDT = '" + strClient + "' ";

                                                    lOracleCmd = new OracleCommand(lSQL, cnCIS);
                                                    lOracleCmd.CommandTimeout = 1200;
                                                    lOracleCmd.Transaction = oracleTransIDB;
                                                    lOracleCmd.ExecuteNonQuery();
                                                    #endregion
                                                }

                                                if (lReturn == true)
                                                {
                                                    if (oracleTransCIS != null) oracleTransCIS.Commit();
                                                }
                                                else
                                                {
                                                    if (oracleTransCIS != null) oracleTransCIS.Rollback();
                                                }
                                                CloseCISConnection(ref cnCIS);
                                                #endregion

                                                #region Cancel SO in IDB
                                                OpenIDBConnection(ref cnIDB);

                                                oracleTransIDB = cnIDB.BeginTransaction(IsolationLevel.ReadCommitted);
                                                //Added on 2019-05-07 as found duplicated record in IDB -- consult Soo Long
                                                #region Cancel SOR in IDB
                                                lSQL = "UPDATE ORDER_HEADER SET CANCELLATION_STATUS = 'C' " +
                                                    "WHERE ORDER_REQUEST_NO = '" + lSOR1 + "' ";

                                                lOracleCmd = new OracleCommand(lSQL, cnIDB);
                                                lOracleCmd.CommandTimeout = 1200;
                                                lOracleCmd.Transaction = oracleTransIDB;
                                                lOracleCmd.ExecuteNonQuery();

                                                lSQL = "UPDATE ORDER_ITEM SET CANCELLATION_STATUS = 'C' " +
                                                "WHERE ORDER_REQUEST_NO = '" + lSOR1 + "' ";

                                                lOracleCmd = new OracleCommand(lSQL, cnIDB);
                                                lOracleCmd.CommandTimeout = 1200;
                                                lOracleCmd.Transaction = oracleTransIDB;
                                                lOracleCmd.ExecuteNonQuery();

                                                #endregion
                                                if (lReturn == true)
                                                {
                                                    if (oracleTransIDB != null) oracleTransIDB.Commit();
                                                }
                                                else
                                                {
                                                    if (oracleTransIDB != null) oracleTransIDB.Rollback();
                                                }
                                                CloseIDBConnection(ref cnIDB);
                                                #endregion
                                            }


                                            #region Unpost and remove from posted list
                                            OpenNDSConnection(ref cnNDS);
                                            if (cnNDS.State != ConnectionState.Open)
                                            {
                                                lErrorMsg = "Cannot connect to NDS database";
                                                lReturn = false;
                                            }
                                            else
                                            {
                                                int lPostHerderID = (int)lSOR[i].PostHeaderID;

                                                //get project id
                                                lProjectID = 0;
                                                //check release status
                                                var lStatus = 0;
                                                lWBSElementID = 0;
                                                int lStructureElementID = 0;
                                                int lProductTypeID = 0;

                                                lSQL = "SELECT intProjectId, intWBSElementId, " +
                                                "intStructureElementTypeId, sitProductTypeId, vchBBSNo " +
                                                "FROM dbo.BBSPostHeader " +
                                                "WHERE intPostHeaderId = " + lPostHerderID.ToString() + " ";

                                                lCmd = new SqlCommand(lSQL, cnNDS);
                                                lCmd.CommandTimeout = 1200;
                                                lRst = lCmd.ExecuteReader();
                                                if (lRst.HasRows)
                                                {
                                                    if (lRst.Read())
                                                    {
                                                        lProjectID = lRst.GetValue(0) == DBNull.Value ? 0 : lRst.GetInt32(0);
                                                        lWBSElementID = lRst.GetValue(1) == DBNull.Value ? 0 : lRst.GetInt32(1);
                                                        lStructureElementID = lRst.GetValue(2) == DBNull.Value ? 0 : lRst.GetInt32(2);
                                                        lProductTypeID = lRst.GetValue(3) == DBNull.Value ? 0 : lRst.GetInt16(3);
                                                        lBBSNo = lRst.GetValue(4) == DBNull.Value ? "" : lRst.GetString(4);
                                                    }
                                                }
                                                lRst.Close();

                                                lSQL = "SELECT tntStatusId " +
                                                "FROM dbo.BBSReleaseDetails " +
                                                "WHERE intPostHeaderid = " + lPostHerderID.ToString() + " ";

                                                lCmd = new SqlCommand(lSQL, cnNDS);
                                                lCmd.CommandTimeout = 1200;
                                                lRst = lCmd.ExecuteReader();
                                                if (lRst.HasRows)
                                                {
                                                    lRst.Read();
                                                    lStatus = lRst.GetByte(0);
                                                }
                                                lRst.Close();

                                                //Cancel the released order
                                                if (lStatus == 12)  //Released
                                                {
                                                    //CloseNDSConnection(ref cnNDS);
                                                    //lErrorMsg = "The WBS already released. It cannot be cancelled. Please cancell the order using NDS first.";
                                                    //lReturn = false;

                                                    lSQL = "dbo.BBSReleaseBySOR_Insert ";
                                                    lCmd = new SqlCommand(lSQL, cnNDS);
                                                    lCmd.Transaction = osqlTransNDS;
                                                    lCmd.CommandTimeout = 1200;
                                                    lCmd.CommandType = CommandType.StoredProcedure;
                                                    lCmd.Parameters.Add("@ORD_REQ_NO", SqlDbType.Char);
                                                    lCmd.Parameters.Add("@IntProjectID", SqlDbType.Int);
                                                    lCmd.Parameters.Add("@IntWBSElementID", SqlDbType.Int);
                                                    lCmd.Parameters.Add("@BBS_NO", SqlDbType.Char);
                                                    lCmd.Parameters.Add("@chrBBSStatus", SqlDbType.Char);
                                                    lCmd.Parameters.Add("@UserID", SqlDbType.Int);

                                                    lCmd.Parameters["@ORD_REQ_NO"].Value = lSOR1;
                                                    lCmd.Parameters["@IntProjectID"].Value = lProjectID;
                                                    lCmd.Parameters["@IntWBSElementID"].Value = lWBSElementID;
                                                    lCmd.Parameters["@BBS_NO"].Value = lBBSNo;
                                                    lCmd.Parameters["@chrBBSStatus"].Value = "C";
                                                    lCmd.Parameters["@UserID"].Value = 11;
                                                    lCmd.CommandTimeout = 1200;
                                                    lCmd.ExecuteNonQuery();
                                                }

                                                //Remove assigned gruop marking (Components)
                                                if (lReturn == true)
                                                {
                                                    int lOutput = 0;

                                                    lSQL = "dbo.BBSPostStatus_Update ";
                                                    lCmd = new SqlCommand(lSQL, cnNDS);
                                                    lCmd.Transaction = osqlTransNDS;
                                                    lCmd.CommandTimeout = 1200;
                                                    lCmd.CommandType = CommandType.StoredProcedure;
                                                    lCmd.Parameters.Add("@intProjectId", SqlDbType.Int);
                                                    lCmd.Parameters.Add("@intWBSElementId", SqlDbType.Int);
                                                    lCmd.Parameters.Add("@intStructureElementTypeId", SqlDbType.Int);
                                                    lCmd.Parameters.Add("@sitProductTypeId", SqlDbType.SmallInt);
                                                    lCmd.Parameters.Add("@chrStatusId", SqlDbType.Char);
                                                    lCmd.Parameters.Add("@intUserID", SqlDbType.Int);
                                                    lCmd.Parameters.Add("@BBSNo", SqlDbType.Char);
                                                    lCmd.Parameters.Add("@Output", SqlDbType.Int).Direction = ParameterDirection.Output;

                                                    lCmd.Parameters["@IntProjectID"].Value = lProjectID;
                                                    lCmd.Parameters["@IntWBSElementID"].Value = lWBSElementID;
                                                    lCmd.Parameters["@intStructureElementTypeId"].Value = lStructureElementID;
                                                    lCmd.Parameters["@sitProductTypeId"].Value = lProductTypeID;
                                                    lCmd.Parameters["@chrStatusId"].Value = "RP";
                                                    lCmd.Parameters["@intUserID"].Value = 11;
                                                    lCmd.Parameters["@BBSNo"].Value = lBBSNo;
                                                    //lCmd.Parameters["@Output"].Value = lOutput;

                                                    lCmd.CommandTimeout = 1200;
                                                    lCmd.ExecuteNonQuery();

                                                    lOutput = Convert.ToInt32(lCmd.Parameters["@Output"].Value);

                                                    lSQL = "DELETE FROM dbo.PostGroupMarkingDetails " +
                                                    "WHERE intPostHeaderId = " + lPostHerderID.ToString() + " ";

                                                    lCmd = new SqlCommand(lSQL, cnNDS);
                                                    lCmd.CommandTimeout = 1200;
                                                    lCmd.ExecuteNonQuery();
                                                }

                                                //NDS SAP
                                                if (lReturn == true)
                                                {
                                                    //delete from request details
                                                    lSQL = "UPDATE SAP_REQUEST_DETAILS SET STATUS = 'X' " +
                                                    "WHERE ORD_REQ_NO = '" + lSOR1 + "' ";

                                                    lCmd = new SqlCommand(lSQL, cnNDS);
                                                    lCmd.CommandTimeout = 1200;
                                                    lCmd.ExecuteNonQuery();

                                                    //delete from request header
                                                    lSQL = "UPDATE SAP_REQUEST_HDR SET STATUS = 'X', PO_NUM = PO_NUM + '-CXL' " +
                                                    "WHERE ORD_REQ_NO = '" + lSOR1 + "' ";

                                                    lCmd = new SqlCommand(lSQL, cnNDS);
                                                    lCmd.CommandTimeout = 1200;
                                                    lCmd.ExecuteNonQuery();

                                                }
                                                CloseNDSConnection(ref cnNDS);
                                            }
                                            #endregion
                                        }
                                    }
                                }

                            }
                            if (ProdType == "BPC")
                            {
                                //Check the order status in NDS
                                var lJob = db.BPCJobAdvice.Find(CustomerCode, ProjectCode, false, JobID);

                                ///////////////////////////////////
                                //Cancel order for customer order
                                //1. Cancel SAP SO
                                //1. Remove Group marking from BBS Posting
                                //2. 
                                ///////////////////////////////////
                                var lDetailsProc = (from p in db.BPCDetailsProc
                                                    where p.CustomerCode == CustomerCode &&
                                                    p.ProjectCode == ProjectCode &&
                                                    p.JobID == JobID
                                                    orderby p.cage_id, p.load_id
                                                    select p).ToList();
                                if (lDetailsProc.Count > 0)
                                {
                                    for (int i = 0; i < lDetailsProc.Count; i++)
                                    {
                                        var lBPCDetails = db.BPCDetails.Find(CustomerCode, ProjectCode, false, JobID, lDetailsProc[i].cage_id);

                                        var lStatus = 0;
                                        if (lDetailsProc[i].nds_groupmarking.Length > 0)
                                        {
                                            OpenNDSConnection(ref cnNDS);
                                            if (cnNDS.State != ConnectionState.Open)
                                            {
                                                lErrorMsg = "Cannot connect to NDS database";
                                                lReturn = false;
                                            }
                                            else
                                            {
                                                if (lReturn == true)
                                                {
                                                    //get project id
                                                    lProjectID = 0;
                                                    lSQL = "SELECT P.intProjectId FROM dbo.SAPProjectMaster P WHERE P.vchProjectCode = '" + ProjectCode + "' ";

                                                    lCmd = new SqlCommand(lSQL, cnNDS);
                                                    lCmd.CommandTimeout = 1200;
                                                    lRst = lCmd.ExecuteReader();
                                                    if (lRst.HasRows)
                                                    {
                                                        lRst.Read();
                                                        lProjectID = lRst.GetInt32(0);
                                                    }
                                                    lRst.Close();

                                                    lWBSElementID = 0;
                                                    lSQL = "SELECT E.intWBSElementId from dbo.WBSElements E, dbo.WBS W, dbo.WBSElementsDetails D " +
                                                        "where e.intWBSId = W.intWBSId " +
                                                        "and E.intWBSElementId = D.intWBSElementId " +
                                                        "and W.intProjectid = " + lProjectID + " " +
                                                        "and vchWBS1 = '" + lDetailsProc[i].nds_wbs1 + "' " +
                                                        "and vchWBS2 = '" + lDetailsProc[i].nds_wbs2 + "' " +
                                                        "and vchWBS3 = '" + lDetailsProc[i].nds_wbs3 + "' " +
                                                        "and sitProductTypeId IN (SELECT sitProductTypeID " +
                                                        "FROM dbo.ProductTypeMaster WHERE vchProductType = 'BPC') " +
                                                        "and intStructureElementTypeId IN " +
                                                        "(SELECT intStructureElementTypeId " +
                                                        "FROM dbo.StructureElementMaster " +
                                                        "WHERE vchStructureElementType = 'Pile' ) " +
                                                        "GROUP BY E.intWBSElementId ";

                                                    lCmd = new SqlCommand(lSQL, cnNDS);
                                                    lCmd.CommandTimeout = 1200;
                                                    lRst = lCmd.ExecuteReader();
                                                    if (lRst.HasRows)
                                                    {
                                                        lRst.Read();
                                                        lWBSElementID = (int)lRst.GetValue(0);
                                                    }
                                                    lRst.Close();
                                                }

                                                //get PostheaderID
                                                int lPostID = 0;

                                                if (lReturn == true)
                                                {
                                                    lSQL = "SELECT intPostHeaderId " +
                                                        "FROM dbo.BBSPostHeader " +
                                                        "WHERE intProjectId = " + lProjectID + " " +
                                                        "AND intWBSElementId = " + lWBSElementID + " " +
                                                        "AND intStructureElementTypeId IN " +
                                                        "(SELECT intStructureElementTypeId " +
                                                        "FROM dbo.StructureElementMaster " +
                                                        "WHERE vchStructureElementType = 'Pile' ) " +
                                                        "AND sitProductTypeId IN (SELECT sitProductTypeID " +
                                                        "FROM dbo.ProductTypeMaster WHERE vchProductType = 'BPC')";

                                                    lCmd = new SqlCommand(lSQL, cnNDS);
                                                    lCmd.CommandTimeout = 1200;
                                                    lRst = lCmd.ExecuteReader();
                                                    if (lRst.HasRows)
                                                    {
                                                        lRst.Read();
                                                        lPostID = (int)lRst.GetValue(0);
                                                    }
                                                    lRst.Close();

                                                    //check release status

                                                    lSQL = "SELECT R.tntStatusId " +
                                                    "FROM dbo.BBSReleaseDetails R, " +
                                                    "dbo.PostGroupMarkingDetails M " +
                                                    "WHERE R.intPostHeaderid = M.intPostHeaderId " +
                                                    "AND M.intPostHeaderId = " + lPostID.ToString() + " ";

                                                    lCmd = new SqlCommand(lSQL, cnNDS);
                                                    lCmd.CommandTimeout = 1200;
                                                    lRst = lCmd.ExecuteReader();
                                                    if (lRst.HasRows)
                                                    {
                                                        lRst.Read();
                                                        lStatus = lRst.GetByte(0);
                                                    }
                                                    lRst.Close();

                                                    if (lStatus == 12)  //Released
                                                    {
                                                        //    CloseNDSConnection(ref cnNDS);
                                                        //    lErrorMsg = "The WBS already released. It cannot be cancelled. Please cancell the order using NDS first.";
                                                        //    lReturn = false;

                                                        //Cancel Released SOR

                                                        string lBBSNoHeader = "";

                                                        lSQL = "SELECT BBS_NO FROM dbo.SAP_REQUEST_DETAILS " +
                                                                    "WHERE ORD_REQ_NO = '" + lDetailsProc[i].sor_no + "' ";
                                                        lCmd = new SqlCommand(lSQL, cnNDS);
                                                        //lCmd.Transaction = osqlTransNDS;
                                                        lCmd.CommandTimeout = 1200;
                                                        lRst = lCmd.ExecuteReader();
                                                        if (lRst.HasRows)
                                                        {
                                                            lRst.Read();
                                                            lBBSNoHeader = (string)lRst.GetValue(0);
                                                        }
                                                        lRst.Close();

                                                        lSQL = "dbo.BBSReleaseBySOR_Insert ";
                                                        lCmd = new SqlCommand(lSQL, cnNDS);
                                                        lCmd.Transaction = osqlTransNDS;
                                                        lCmd.CommandTimeout = 1200;
                                                        lCmd.CommandType = CommandType.StoredProcedure;
                                                        lCmd.Parameters.Add("@ORD_REQ_NO", SqlDbType.Char);
                                                        lCmd.Parameters.Add("@IntProjectID", SqlDbType.Int);
                                                        lCmd.Parameters.Add("@IntWBSElementID", SqlDbType.Int);
                                                        lCmd.Parameters.Add("@BBS_NO", SqlDbType.Char);
                                                        lCmd.Parameters.Add("@chrBBSStatus", SqlDbType.Char);
                                                        lCmd.Parameters.Add("@UserID", SqlDbType.Int);

                                                        lCmd.Parameters["@ORD_REQ_NO"].Value = lDetailsProc[i].sor_no;
                                                        lCmd.Parameters["@IntProjectID"].Value = lProjectID;
                                                        lCmd.Parameters["@IntWBSElementID"].Value = lWBSElementID;
                                                        lCmd.Parameters["@BBS_NO"].Value = lBBSNoHeader;
                                                        lCmd.Parameters["@chrBBSStatus"].Value = "C";
                                                        lCmd.Parameters["@UserID"].Value = 11;
                                                        lCmd.CommandTimeout = 1200;
                                                        lCmd.ExecuteNonQuery();
                                                    }

                                                    //var lRet = RemoveGroupMarking(CustomerCode, ProjectCode, ContractNo, lPostID,
                                                    //            "Pile", "BPC", lDetailsProc[i].nds_groupmarking, 0);
                                                    //if (lRet < 0)
                                                    //{
                                                    //    lErrorMsg = "Error on removing group marking from BBS Posting";
                                                    //    lReturn = false;

                                                    //}
                                                }

                                                if (lReturn == true && lDetailsProc[i].nds_groupmarking != null && lDetailsProc[i].nds_groupmarking != "" && lStatus != 14
                                                    && (lGMs == "" || lGMs.IndexOf(lDetailsProc[i].nds_groupmarking) < 0))  //14 - Cancelled
                                                {
                                                    // get group marking ID and SE Level ID
                                                    lSQL = "SELECT G.intGroupMarkId, S.intSEDetailingId " +
                                                    "FROM dbo.GroupMarkingDetails G, dbo.SELevelDetails S  " +
                                                    "WHERE G.intGroupMarkId = S.intGroupMarkId " +
                                                    "AND G.intProjectid = " + lProjectID.ToString() + " " +
                                                    "and S.intStructureElementTypeId in " +
                                                    "(SELECT intStructureElementTypeId " +
                                                    "FROM dbo.StructureElementMaster " +
                                                    "WHERE vchStructureElementType = 'Pile' ) " +
                                                    "and S.sitProductTypeId in " +
                                                    "(SELECT sitProductTypeID " +
                                                    "FROM dbo.ProductTypeMaster " +
                                                    "WHERE vchProductType = 'BPC' ) " +
                                                    "AND vchGroupMarkingName = '" + lDetailsProc[i].nds_groupmarking + "' " +
                                                    "AND tntGroupRevNo = 0 ";

                                                    lCmd = new SqlCommand(lSQL, cnNDS);
                                                    lCmd.CommandTimeout = 1200;
                                                    lRst = lCmd.ExecuteReader();
                                                    if (lRst.HasRows)
                                                    {
                                                        lRst.Read();
                                                        lGroupMarkingID = lRst.GetInt32(0);
                                                        lSELevelID = lRst.GetInt32(1);
                                                    }

                                                    lRst.Close();

                                                    if (lGroupMarkingID > 0 && lSELevelID > 0)
                                                    {
                                                        //delete CAB acc
                                                        lSQL = "DELETE FROM dbo.AccProductMarkingDetails " +
                                                        "WHERE intSEDetailingId = " + lSELevelID.ToString() + " ";

                                                        lCmd = new SqlCommand(lSQL, cnNDS);
                                                        lCmd.CommandTimeout = 1200;
                                                        lCmd.ExecuteNonQuery();

                                                        //delete from parameter details
                                                        lSQL = "DELETE FROM dbo.SHAPEDETAILINGTRANS_PARALLEL " +
                                                        "WHERE intShapeTransHeaderId IN " +
                                                        "(SELECT intShapeTransHeaderId FROM dbo.CABProductMarkingDetails " +
                                                        "WHERE intSEDetailingId = " + lSELevelID.ToString() + ") ";
                                                        lCmd = new SqlCommand(lSQL, cnNDS);
                                                        lCmd.CommandTimeout = 1200;
                                                        lCmd.ExecuteNonQuery();

                                                        //delete from shape trans header
                                                        lSQL = "DELETE FROM dbo.ShapeDetailingTrans " +
                                                        "WHERE intShapeTransHeaderId IN " +
                                                        "(SELECT intShapeTransHeaderId FROM dbo.CABProductMarkingDetails " +
                                                        "WHERE intSEDetailingId = " + lSELevelID.ToString() + ") ";
                                                        lCmd = new SqlCommand(lSQL, cnNDS);
                                                        lCmd.CommandTimeout = 1200;
                                                        lCmd.ExecuteNonQuery();

                                                        //delete from shape trans header
                                                        lSQL = "DELETE FROM dbo.ShapeDetailingTransHeader " +
                                                        "WHERE intShapeTransHeaderId IN " +
                                                        "(SELECT intShapeTransHeaderId FROM dbo.CABProductMarkingDetails " +
                                                        "WHERE intSEDetailingId = " + lSELevelID.ToString() + ") ";
                                                        lCmd = new SqlCommand(lSQL, cnNDS);
                                                        lCmd.CommandTimeout = 1200;
                                                        lCmd.ExecuteNonQuery();

                                                        //delete CAB product marking details
                                                        lSQL = "DELETE FROM dbo.CABProductMarkingDetails " +
                                                        "WHERE intSEDetailingId = " + lSELevelID.ToString() + " ";
                                                        lCmd = new SqlCommand(lSQL, cnNDS);
                                                        lCmd.CommandTimeout = 1200;
                                                        lCmd.ExecuteNonQuery();

                                                        //delete BPC details
                                                        lSQL = "DELETE FROM dbo.BPCStructureMarkingDetails " +
                                                        "WHERE intSEDetailingId = " + lSELevelID.ToString() + " ";

                                                        lCmd = new SqlCommand(lSQL, cnNDS);
                                                        lCmd.CommandTimeout = 1200;
                                                        lCmd.ExecuteNonQuery();

                                                        //delete from group marking
                                                        lSQL = "DELETE FROM dbo.GroupMarkingDetails " +
                                                        "WHERE intGroupMarkId = " + lGroupMarkingID.ToString() + " ";

                                                        lCmd = new SqlCommand(lSQL, cnNDS);
                                                        lCmd.CommandTimeout = 1200;
                                                        lCmd.ExecuteNonQuery();

                                                        //delete from SE Level
                                                        lSQL = "DELETE FROM dbo.SELevelDetails " +
                                                        "WHERE intSEDetailingId = " + lSELevelID.ToString() + " ";

                                                        lCmd = new SqlCommand(lSQL, cnNDS);
                                                        lCmd.CommandTimeout = 1200;
                                                        lCmd.ExecuteNonQuery();

                                                        //delete from request details
                                                        lSQL = "UPDATE SAP_REQUEST_DETAILS SET STATUS = 'X' " +
                                                        "WHERE ORD_REQ_NO = '" + lDetailsProc[i].sor_no + "' ";

                                                        lCmd = new SqlCommand(lSQL, cnNDS);
                                                        lCmd.CommandTimeout = 1200;
                                                        lCmd.ExecuteNonQuery();

                                                        //delete from request header
                                                        lSQL = "UPDATE SAP_REQUEST_HDR SET STATUS = 'X', PO_NUM = PO_NUM + '-CXL' " +
                                                        "WHERE ORD_REQ_NO = '" + lDetailsProc[i].sor_no + "' ";

                                                        lCmd = new SqlCommand(lSQL, cnNDS);
                                                        lCmd.CommandTimeout = 1200;
                                                        lCmd.ExecuteNonQuery();
                                                    }
                                                    lGMs = lGMs + "," + lDetailsProc[i].nds_groupmarking;
                                                }
                                                CloseNDSConnection(ref cnNDS);
                                            }

                                            if (lReturn == true)
                                            {
                                                OpenIDBConnection(ref cnIDB);
                                                if (cnIDB.State != ConnectionState.Open)
                                                {
                                                    lReturn = false;
                                                }
                                                else
                                                {
                                                    if (lDetailsProc[i].sor_no != null && lDetailsProc[i].sor_no != "" && (lSORs == "" || lSORs.IndexOf(lDetailsProc[i].sor_no) < 0))
                                                    {
                                                        lSQL = "UPDATE ORDER_HEADER SET CANCELLATION_STATUS = 'C' " +
                                                        "WHERE ORDER_REQUEST_NO = '" + lDetailsProc[i].sor_no + "' ";

                                                        var lOracleCmd = new OracleCommand(lSQL, cnIDB);
                                                        lOracleCmd.CommandTimeout = 1200;
                                                        lOracleCmd.ExecuteNonQuery();

                                                        lSQL = "UPDATE ORDER_ITEM SET CANCELLATION_STATUS = 'C' " +
                                                        "WHERE ORDER_REQUEST_NO = '" + lDetailsProc[i].sor_no + "' ";

                                                        lOracleCmd = new OracleCommand(lSQL, cnIDB);
                                                        lOracleCmd.CommandTimeout = 1200;
                                                        lOracleCmd.ExecuteNonQuery();
                                                    }
                                                    CloseIDBConnection(ref cnIDB);
                                                }
                                            }

                                            if (lReturn == true)
                                            {
                                                OpenCISConnection(ref cnCIS);
                                                if (cnCIS.State != ConnectionState.Open)
                                                {
                                                    lReturn = false;
                                                }
                                                else
                                                {
                                                    if (lDetailsProc[i].sor_no != null && lDetailsProc[i].sor_no != "" && (lSORs == "" || lSORs.IndexOf(lDetailsProc[i].sor_no) < 0))
                                                    {
                                                        lSAPSO = lSAPSO + lDetailsProc[i].sor_no;
                                                        //updade request header
                                                        lSQL = "UPDATE SAPSR3.YMSDT_ORDER_HDR " +
                                                        "SET STATUS = 'X', PO_NUMBER = PO_NUMBER || '-CXL' " +
                                                        "WHERE ORDER_REQUEST_NO = '" + lDetailsProc[i].sor_no + "' " +
                                                        "AND MANDT = '" + strClient + "' ";

                                                        var lOracleCmd = new OracleCommand(lSQL, cnCIS);
                                                        lOracleCmd.CommandTimeout = 1200;
                                                        lOracleCmd.ExecuteNonQuery();

                                                        //updade request header
                                                        lSQL = "UPDATE SAPSR3.YMSDT_REQ_DETAIL SET STATUS = 'X' " +
                                                        "WHERE ORDER_REQUEST_NO = '" + lDetailsProc[i].sor_no + "' " +
                                                        "AND MANDT = '" + strClient + "' ";

                                                        lOracleCmd = new OracleCommand(lSQL, cnCIS);
                                                        lOracleCmd.CommandTimeout = 1200;
                                                        lOracleCmd.ExecuteNonQuery();

                                                        lSQL = "Commit ";

                                                        lOracleCmd = new OracleCommand(lSQL, cnCIS);
                                                        lOracleCmd.CommandTimeout = 1200;
                                                        lOracleCmd.ExecuteNonQuery();

                                                        // Cancel SAP SO if created
                                                        string lSONo = "";
                                                        lSQL = "SELECT VBELN " +
                                                        "FROM SAPSR3.VBAK " +
                                                        "WHERE MANDT = '" + strClient + "' " +
                                                        "AND IHREZ = '" + lDetailsProc[i].sor_no + "' ";

                                                        lOracleCmd.CommandText = lSQL;
                                                        lOracleCmd.Connection = cnCIS;
                                                        lOracleCmd.CommandTimeout = 1200;
                                                        lOrRst = lOracleCmd.ExecuteReader();
                                                        if (lOrRst.HasRows)
                                                        {
                                                            lOrRst.Read();
                                                            lSONo = (lOrRst.GetString(0) == null ? "" : lOrRst.GetString(0));
                                                        }
                                                        lOrRst.Close();

                                                        if (lSONo != "")
                                                        {
                                                            string lSOStatus = "";
                                                            lSQL = "SELECT ABGRU " +
                                                            "FROM SAPSR3.VBAP " +
                                                            "WHERE MANDT = '" + strClient + "' " +
                                                            "AND VBELN = '" + lSONo + "' " +
                                                            "ORDER BY ABGRU ";

                                                            lOracleCmd = new OracleCommand(lSQL, cnCIS);
                                                            lOracleCmd.CommandText = lSQL;
                                                            lOracleCmd.CommandTimeout = 1200;
                                                            lOrRst = lOracleCmd.ExecuteReader();
                                                            if (lOrRst.HasRows)
                                                            {
                                                                lOrRst.Read();
                                                                lSOStatus = (lOrRst.GetString(0) == null ? "" : lOrRst.GetString(0)).Trim();
                                                            }
                                                            lOrRst.Close();

                                                            if (lSOStatus == "")
                                                            {
                                                                CreateSOinSAP lSAP = new CreateSOinSAP();
                                                                int lResult = lSAP.CancelSAPSO(CustomerCode, ProjectCode, JobID, lJob.PONumber, lBPCDetails.bbs_no, lSONo);
                                                                //return no of items in the order
                                                                if (lResult < 0)
                                                                {
                                                                    lErrorMsg = "Cannot cancel So in SAP.";
                                                                    lReturn = false;
                                                                    break;
                                                                }
                                                                lSAP = null;

                                                                lSOStatus = "";
                                                                lSQL = "SELECT ABGRU " +
                                                                "FROM SAPSR3.VBAP " +
                                                                "WHERE MANDT = '" + strClient + "' " +
                                                                "AND VBELN = '" + lSONo + "' " +
                                                                "ORDER BY ABGRU ";

                                                                lOracleCmd.CommandText = lSQL;
                                                                lOracleCmd.Connection = cnCIS;
                                                                lOracleCmd.CommandTimeout = 1200;
                                                                lOrRst = lOracleCmd.ExecuteReader();
                                                                if (lOrRst.HasRows)
                                                                {
                                                                    lOrRst.Read();
                                                                    lSOStatus = (lOrRst.GetString(0) == null ? "" : lOrRst.GetString(0)).Trim();
                                                                }
                                                                lOrRst.Close();
                                                                if (lSOStatus == "")
                                                                {
                                                                    lReturn = false;
                                                                    lErrorMsg = "SAP Error: cannot cancel SO in SAP.";
                                                                }
                                                            }
                                                        }
                                                        lSORs = lSORs + "," + lDetailsProc[i].sor_no;
                                                    }
                                                    CloseCISConnection(ref cnCIS);
                                                }
                                            }
                                        }
                                    }
                                }

                                if (lReturn == true)
                                {
                                    var lJobNew = lJob;
                                    if (ActionType == "Cancel")
                                    {
                                        lJobNew.OrderStatus = "Cancelled";
                                    }
                                    else
                                    {
                                        lJobNew.OrderStatus = "Submitted";
                                    }
                                    lJobNew.UpdateDate = DateTime.Now;
                                    db.Entry(lJob).CurrentValues.SetValues(lJobNew);
                                }
                            }
                        }
                    }
                    if (lReturn != true)//lReturn == true ajit
                    {
                        if (StructureElement == null || StructureElement == "")
                        {
                            StructureElement = "NONWBS";
                        }
                        if (ScheduledProd == null || ScheduledProd == "")
                        {
                            ScheduledProd = "N";
                        }

                        var lProcess = db.Process.Find(CustomerCode, ProjectCode, lOrderNumber, StructureElement, ProdType, ScheduledProd);
                        if (lProcess != null)
                        {
                            if (lSAPSO.Length > 200)
                            {
                                lSAPSO = lSAPSO.Substring(0, 200);
                            }

                            var lProcessCancel = new ProcessCancelModels()
                            {
                                CustomerCode = lProcess.CustomerCode,
                                ProjectCode = lProcess.ProjectCode,
                                JobID = lProcess.JobID,
                                StructureElement = lProcess.StructureElement,
                                ProdType = lProcess.ProdType,
                                ScheduledProd = lProcess.ScheduledProd,
                                Contract = lProcess.Contract,
                                NonContract = lProcess.NonContract,
                                CashPayment = lProcess.CashPayment,
                                CABFormer = lProcess.CABFormer,
                                ShipToParty = lProcess.ShipToParty,
                                ProjectStage = lProcess.ProjectStage,
                                RequiredDateFrom = lProcess.RequiredDateFrom,
                                RequiredDateTo = lProcess.RequiredDateTo,
                                PONumber = lProcess.PONumber,
                                WBS1 = lProcess.WBS1,
                                WBS2 = lProcess.WBS2,
                                WBS3 = lProcess.WBS3,
                                TotalCABWeight = lProcess.TotalCABWeight,
                                TotalSTDWeight = lProcess.TotalSTDWeight,
                                TransportLimit = lProcess.TransportLimit,
                                VehicleType = lProcess.VehicleType,
                                Urgent = lProcess.Urgent,
                                Conquas = lProcess.Conquas,
                                Crane = lProcess.Crane,
                                Premium = lProcess.Premium,
                                ZeroTol = lProcess.ZeroTol,
                                CallDel = lProcess.CallDel,
                                SpecialPass = lProcess.SpecialPass,
                                LowBed = lProcess.LowBed,
                                Veh50Ton = lProcess.Veh50Ton,
                                Borge = lProcess.Borge,
                                PoliceEscort = lProcess.PoliceEscort,
                                DoNotMix = lProcess.DoNotMix,
                                TimeRange = lProcess.TimeRange,
                                IntRemarks = lProcess.IntRemarks,
                                ExtRemarks = lProcess.ExtRemarks,
                                OrderType = lProcess.OrderType,
                                UpdateDate = (DateTime)lProcess.UpdateDate,
                                ProcessedBy = lProcess.ProcessedBy,
                                CancelDate = DateTime.Now,
                                CancelledBy = User.Identity.GetUserName(),
                                SAPSO = lSAPSO,
                                FabricateESM = lProcess.FabricateESM
                            };

                            db.ProcessCancel.Add(lProcessCancel);

                            if (ActionType != "Cancel")
                            {
                                db.Process.Remove(lProcess);
                            }

                        }
                        db.SaveChanges();

                        //if (lReturn == true)
                        //{
                        //    cnNDS = new SqlConnection();
                        //    OpenNDSConnection(ref cnNDS);
                        //    var lUpdStatus = "Submitted";
                        //    if (ActionType == "Cancel")
                        //    {
                        //        lUpdStatus = "Cancelled";
                        //    }

                        //    updateHeaderStatus(sCustomerCode, sProjectCode, JobID, ProdType, lUpdStatus, cnNDS);
                        //    CloseNDSConnection(ref cnNDS);
                        //}

                        if (lReturn == true && (OrderSource == "UX" || OrderSource == "UXE" || OrderSource == "OL"))
                        {
                            try
                            {
                                var lHeader = db.OrderProject.Find(lOrderNumber);
                                var lSE = db.OrderProjectSE.Find(lOrderNumber, StructureElement, ProdType, ScheduledProd);

                                var lNewSE = lSE;
                                if (ActionType == "Cancel")
                                {
                                    lNewSE.OrderStatus = "Cancelled";
                                }
                                else
                                {
                                    lNewSE.OrderStatus = "Submitted";
                                }
                                lNewSE.ProcessBy = "";

                                //lNewSE.UpdateDate = DateTime.Now;

                                db.Entry(lSE).CurrentValues.SetValues(lNewSE);

                                var lSECheck = (from p in db.OrderProjectSE
                                                where p.OrderNumber == lOrderNumber
                                                select p).ToList();

                                //check whether have processed item
                                int lFound = 0;
                                decimal lTotalWt = 0;
                                int lCanCancel = 1;
                                if (lSECheck != null && lSECheck.Count > 0)
                                {
                                    for (int i = 0; i < lSECheck.Count; i++)
                                    {
                                        if ((lSECheck[i].StructureElement != StructureElement ||
                                            lSECheck[i].ProductType != ProdType ||
                                            lSECheck[i].ScheduledProd != ScheduledProd) &&
                                            lSECheck[i].OrderStatus != "Submitted" &&
                                            lSECheck[i].OrderStatus != "Created" &&
                                            lSECheck[i].OrderStatus != "Created*" &&
                                            lSECheck[i].OrderStatus != "Submitted*" &&
                                            lSECheck[i].OrderStatus != "Cancelled")
                                        {
                                            lFound = 1;
                                        }

                                        if ((lSECheck[i].StructureElement != StructureElement ||
                                            lSECheck[i].ProductType != ProdType ||
                                            lSECheck[i].ScheduledProd != ScheduledProd ||
                                            ActionType != "Cancel") &&
                                            lSECheck[i].OrderStatus != "Cancelled")
                                        {
                                            lTotalWt = lTotalWt + (lSECheck[i].TotalWeight == null ? 0 : (decimal)lSECheck[i].TotalWeight);
                                            lCanCancel = 0;
                                            //cannot cancel as there is items that are not ancelled
                                        }
                                    }
                                }

                                var lNewHeader = lHeader;
                                if (lFound == 0)
                                {
                                    if (ActionType == "Cancel" && lCanCancel == 1)
                                    {
                                        lNewHeader.OrderStatus = "Cancelled";
                                    }
                                    else
                                    {
                                        lNewHeader.OrderStatus = "Submitted";
                                    }
                                }

                                lNewHeader.TotalWeight = lTotalWt;
                                //lNewHeader.UpdateDate = DateTime.Now;

                                db.Entry(lHeader).CurrentValues.SetValues(lNewHeader);

                                db.SaveChanges();
                            }

                            catch (Exception ex)
                            {
                                lErrorMsg = "Error during update order status: " + ex.Message;
                                lReturn = false;
                                //SaveErrorMsg(ex.Message, ex.StackTrace);
                            }

                            var lExtEmail = 0;
                            var lStandardProd = 0;

                            var lSEa = (from p in db.OrderProjectSE
                                        where p.OrderNumber == lOrderNumber
                                        select p).ToList();

                            if (lSEa != null && lSEa.Count > 0)
                            {
                                for (int i = 0; i < lSEa.Count; i++)
                                {
                                    if (lSEa[i].ProductType == "STANDARD-MESH" ||
                                        lSEa[i].ProductType == "STANDARD-BAR" ||
                                        lSEa[i].ProductType == "COIL")
                                    {
                                        lStandardProd = 1;
                                    }
                                    if (lSEa[i].UpdateBy.Split('@')[1].ToLower() != "natsteel.com.sg")
                                    {
                                        lExtEmail = 1;
                                    }
                                }
                            }
                            //Send notification email.
                            if (lExtEmail == 1 || lStandardProd == 1)
                            {
                                //var lEmailObj = new SendGridEmail();//ajit
                                var lActionA = "Cancelled";
                                if (ActionType == "Withdraw")
                                {
                                    lActionA = "Withdraw";
                                }
                                //lEmailObj.sendOrderActionEmail(CustomerCode, ProjectCode, lOrderNumber, lActionA, lCurrentStatus, sUserID, 0, StructureElement, ProdType, ScheduledProd);
                                //lEmailObj = null; commented by ajit
                            }
                        }

                        if (OrderSource != "UX" && OrderSource != "UXE" && OrderSource != "OL")
                        {
                            int lExtEmail = 1;
                            if (ProdType == "Standard MESH")
                            {
                                var JobCheck = db.StdSheetJobAdvice.Find(CustomerCode, ProjectCode, JobID);
                                if (JobCheck != null)
                                {
                                    if (JobCheck.UpdateBy.Split('@')[1].ToLower() == "natsteel.com.sg")
                                    {
                                        lExtEmail = 0;
                                    }
                                }
                            }

                            if (lExtEmail == 1)
                            {
                                //Send prompting email
                                var lEmailContent = "";
                                var lEmailFrom = "";
                                var lEmailTo = "";
                                var lEmailCc = "";
                                var lEmailSubject = "";
                                string lVar1 = "";

                                if (ProdType == "Rebar")
                                {
                                    var JobContent = db.JobAdvice.Find(CustomerCode, ProjectCode, JobID);
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

                                        if (JobContent.ProjectStage == null) JobContent.ProjectStage = "";
                                        else JobContent.ProjectStage = JobContent.ProjectStage.Trim();

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

                                        if (JobContent.TransportLimit == null) JobContent.TransportLimit = "";
                                        else JobContent.TransportLimit = JobContent.TransportLimit.Trim();

                                        if (JobContent.TransportMode == null) JobContent.TransportMode = "";
                                        else JobContent.TransportMode = JobContent.TransportMode.Trim();

                                        if (JobContent.WBS1 == null) JobContent.WBS1 = "";
                                        else JobContent.WBS1 = JobContent.WBS1.Trim();

                                        if (JobContent.WBS2 == null) JobContent.WBS2 = "";
                                        else JobContent.WBS2 = JobContent.WBS2.Trim();

                                        if (JobContent.WBS3 == null) JobContent.WBS3 = "";
                                        else JobContent.WBS3 = JobContent.WBS3.Trim();
                                    }

                                    string lUserID = User.Identity.GetUserName().ToUpper();
                                    if (lUserID.IndexOf("@") > 0)
                                    {
                                        lUserID = lUserID.Substring(0, lUserID.IndexOf("@"));
                                    }
                                    lEmailContent = "<p align='center'>CANCELLED JOB ADVICE CAB/SB (取消工作通知 / 加工铁与标准直铁料表) - " + lUserID + "</p>";

                                    lEmailContent = lEmailContent + "<table border='1' width=100%><tr>";
                                    lEmailContent = lEmailContent + "<td width=20%>" + "Customer (客户名称)" + "</td>";

                                    CustomerModels lCustomer = db.Customer.Find(JobContent.CustomerCode);
                                    string lVar = "";
                                    if (lCustomer != null) lVar = lCustomer.CustomerName.Trim() + " (" + JobContent.CustomerCode.Trim() + ")";
                                    lEmailContent = lEmailContent + "<td>" + lVar + "</td></tr>";

                                    lEmailContent = lEmailContent + "<tr><td>" + "Project (工程项目)" + "</td>";

                                    lVar = "";
                                    var lProject = db.Project.Find(CustomerCode, ProjectCode);
                                    if (lProject != null) lVar = lProject.ProjectTitle.Trim() + " (" + JobContent.ProjectCode.Trim() + ")";
                                    lEmailContent = lEmailContent + "<td>" + lVar + "</td></tr></table>";

                                    lEmailContent = lEmailContent + "<table border='1' width=100%><tr>";

                                    lEmailContent = lEmailContent + "<td width=20%>" + "PO No.\n(客户订单号码)" + "</td>";
                                    lEmailContent = lEmailContent + "<td width=27%>" + JobContent.PONumber.Trim() + "</td>";
                                    lEmailContent = lEmailContent + "<td width=26%>" + "Order Date\n(订单日期)" + "</td>";
                                    lEmailContent = lEmailContent + "<td>" + String.Format("{0:yyyy-MM-dd}", JobContent.PODate) + "</td></tr>";

                                    lEmailContent = lEmailContent + "<tr><td width=20%>" + "Required Date\n(交货日期)" + "</td>";
                                    lEmailContent = lEmailContent + "<td width=27%>" + String.Format("{0:yyyy-MM-dd}", JobContent.RequiredDate) + "</td>";
                                    lEmailContent = lEmailContent + "<td width=26%>" + "Order Weight\n(订单重量)" + "</td>";
                                    lEmailContent = lEmailContent + "<td>" + ((decimal)JobContent.TotalWeight).ToString("F3") + " KG" + "</td></tr>";

                                    lEmailContent = lEmailContent + "<tr><td width=20%>" + "CAB Bars Weight\n(加工铁重量)" + "</td>";
                                    lEmailContent = lEmailContent + "<td width=27%>" + ((decimal)JobContent.TotalCABWeight).ToString("F3") + " KG" + "</td>";
                                    lEmailContent = lEmailContent + "<td width=26%>" + "SB Weight\n(标准直铁重量)" + "</td>";
                                    lEmailContent = lEmailContent + "<td>" + ((decimal)JobContent.TotalSTDWeight).ToString("F3") + " KG" + "</td></tr>";

                                    lEmailContent = lEmailContent + "<tr><td width=20%>" + "Transportation\n(运输工具)" + "</td>";
                                    lEmailContent = lEmailContent + "<td width=27%>" + JobContent.TransportMode + "</td>";
                                    lEmailContent = lEmailContent + "<td width=26%>" + "Project Stage\n(工程阶段)" + "</td>";
                                    lEmailContent = lEmailContent + "<td>" + JobContent.ProjectStage + "</td></tr></table>";

                                    lEmailContent = lEmailContent + "<table border='1' width=100%><tr>";
                                    lEmailContent = lEmailContent + "<td width='20%'>" + "Block (WBS1)\n(座号/大牌)" + "</td>";
                                    lEmailContent = lEmailContent + "<td width=15%>" + JobContent.WBS1.Trim() + "</td>";
                                    lEmailContent = lEmailContent + "<td width='17%'>" + "Storey (WBS2)\n(楼层)" + "</td>";
                                    lEmailContent = lEmailContent + "<td width=14%>" + JobContent.WBS2.Trim() + "</td>";
                                    lEmailContent = lEmailContent + "<td width=16%>" + "Part (WBS3)\n(分部)" + "</td>";
                                    lEmailContent = lEmailContent + "<td>" + JobContent.WBS3.Trim() + "</td></tr></table>";

                                    var lBBSContent = (from p in db.BBS
                                                       where p.CustomerCode == CustomerCode &&
                                                       p.ProjectCode == ProjectCode &&
                                                       p.JobID == JobID &&
                                                       p.BBSOrderCABWT + p.BBSOrderSTDWT > 0
                                                       orderby p.BBSID
                                                       select p).ToList();

                                    if (lBBSContent.Count > 0)
                                    {
                                        lEmailContent = lEmailContent + "<table border='1' width=100%><tr>";
                                        lEmailContent = lEmailContent + "<td width='14%'>" + "BBS No.\n(钢筋加工表)" + "</td>";
                                        lEmailContent = lEmailContent + "<td width='15%'>" + "BBS Description\n(具体描述)" + "</td>";
                                        lEmailContent = lEmailContent + "<td width='16%'>" + "Structure Element\n(构件)" + "</td>";
                                        lEmailContent = lEmailContent + "<td width='14%'>" + "CAB Weight\n(加工铁重量)" + "</td>";
                                        lEmailContent = lEmailContent + "<td width='14%'>" + "SB Weight\n(标准直铁重量)" + "</td>";
                                        lEmailContent = lEmailContent + "<td width='14%'>" + "Total Weight\n(总重量)" + "</td>";
                                        lEmailContent = lEmailContent + "<td>" + "SOR/SO Number\n(订单号码)" + "</td></tr>";

                                        for (int i = 0; i < lBBSContent.Count; i++)
                                        {
                                            lEmailContent = lEmailContent + "<tr><td><font color='blue'>" + lBBSContent[i].BBSNo + "</font></td>";
                                            lEmailContent = lEmailContent + "<td>" + lBBSContent[i].BBSDesc + "</td>";
                                            lEmailContent = lEmailContent + "<td>" + lBBSContent[i].BBSStrucElem + "</td>";
                                            if (lBBSContent[i].BBSOrderCABWT == 0) lVar = ""; else lVar = lBBSContent[i].BBSOrderCABWT.ToString("F3");
                                            lEmailContent = lEmailContent + "<td align='left'>" + lVar + "</td>";
                                            if (lBBSContent[i].BBSOrderSTDWT == 0) lVar = ""; else lVar = lBBSContent[i].BBSOrderSTDWT.ToString("F3");
                                            lEmailContent = lEmailContent + "<td align='left'>" + lVar + "</td>";
                                            lEmailContent = lEmailContent + "<td align='left'>" + (lBBSContent[i].BBSOrderCABWT + lBBSContent[i].BBSOrderSTDWT).ToString("F3") + "</td>";
                                            lVar = "";
                                            if (lBBSContent[i].BBSSAPSO != null)
                                            {
                                                if (lBBSContent[i].BBSSAPSO.Trim().Length > 0)
                                                {
                                                    if (lVar == "")
                                                    {
                                                        lVar = lBBSContent[i].BBSSAPSO.Trim();
                                                    }
                                                    else
                                                    {
                                                        lVar = lVar + "/ " + lBBSContent[i].BBSSAPSO.Trim();
                                                    }
                                                }
                                            }
                                            if (lBBSContent[i].BBSSOR != null)
                                            {
                                                if (lBBSContent[i].BBSSOR.Trim().Length > 0)
                                                {
                                                    if (lVar == "")
                                                    {
                                                        lVar = lBBSContent[i].BBSSOR.Trim();
                                                    }
                                                    else
                                                    {
                                                        lVar = lVar + "/ " + lBBSContent[i].BBSSOR.Trim();
                                                    }
                                                }
                                            }
                                            if (lBBSContent[i].BBSSORCoupler != null)
                                            {
                                                if (lBBSContent[i].BBSSORCoupler.Trim().Length > 0)
                                                {
                                                    if (lVar == "")
                                                    {
                                                        lVar = lBBSContent[i].BBSSORCoupler.Trim();
                                                    }
                                                    else
                                                    {
                                                        lVar = lVar + "/ " + lBBSContent[i].BBSSORCoupler.Trim();
                                                    }
                                                }
                                            }
                                            lEmailContent = lEmailContent + "<td align='left'>" + lVar + "</td></tr>";
                                        }
                                    }
                                    lEmailContent = lEmailContent + "</table>";

                                    lEmailContent = lEmailContent + "<table border='1' width=100%><tr>";

                                    lEmailContent = lEmailContent + "<td colspan='6'>" + "Site Contact (工地联系人)" + "</td></tr>";

                                    lEmailContent = lEmailContent + "<tr><td width='20%'>" + "Goods Receiver\n(收货人)" + "</td>";
                                    lEmailContent = lEmailContent + "<td width='15%'>" + (JobContent.SiteEngr_Name == null ? "" : JobContent.SiteEngr_Name.Trim()) + "</td>";
                                    lEmailContent = lEmailContent + "<td width='13%'>" + "H / P\n(手机号码)" + " </td>";
                                    lEmailContent = lEmailContent + "<td width='16%'>" + (JobContent.SiteEngr_HP == null ? "" : JobContent.SiteEngr_HP.Trim()) + "</td>";
                                    lEmailContent = lEmailContent + "<td width='13%'>" + "Email\n(电邮地址)" + "</td>";
                                    lEmailContent = lEmailContent + "<td>" + (JobContent.SiteEngr_Tel == null ? "" : JobContent.SiteEngr_Tel.Trim()) + "</td></tr>";

                                    lEmailContent = lEmailContent + "<tr><td width='20%'>" + "Site Contact\n(联系人)" + "</td>";
                                    lEmailContent = lEmailContent + "<td width='15%'>" + (JobContent.Scheduler_Name == null ? "" : JobContent.Scheduler_Name.Trim()) + "</td>";
                                    lEmailContent = lEmailContent + "<td width='13%'>" + "H / P\n(手机号码)" + " </td>";
                                    lEmailContent = lEmailContent + "<td width='16%'>" + (JobContent.Scheduler_HP == null ? "" : JobContent.Scheduler_HP.Trim()) + "</td>";
                                    lEmailContent = lEmailContent + "<td width='13%'>" + "Email\n(电邮地址)" + "</td>";
                                    lEmailContent = lEmailContent + "<td>" + (JobContent.Scheduler_Tel == null ? "" : JobContent.Scheduler_Tel.Trim()) + "</td></tr></table>";

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

                                    //lVar1 = JobContent.Scheduler_Tel.Trim();
                                    //if (lVar1 != "")
                                    //{
                                    //    if (lEmailTo == "") { lEmailTo = lVar1; }
                                    //    else { lEmailTo = lEmailTo + ";" + lVar1; }
                                    //}

                                    //lVar1 = JobContent.SiteEngr_Tel.Trim();
                                    //if (lVar1 != "")
                                    //{
                                    //    if (lEmailTo.IndexOf(lVar1) < 0)
                                    //    {
                                    //        if (lEmailTo == "") { lEmailTo = lVar1; }
                                    //        else { lEmailTo = lEmailTo + ";" + lVar1; }
                                    //    }
                                    //}

                                    // CC to Planning_mesh
                                    if (strServer == "DEV")
                                    {
                                        if (lEmailCc.Length > 0)
                                        {
                                            lEmailCc = lEmailCc + ";zbc@natsteel.com.sg";
                                        }
                                        else
                                        {
                                            lEmailCc = "zbc@natsteel.com.sg";
                                        }
                                    }
                                    else
                                    {
                                        if (lEmailCc.Length > 0)
                                        {
                                            if (ProdType == "Rebar")
                                            {
                                                //lEmailCc = lEmailCc + ";planning_cab@natsteel.com.sg";
                                            }
                                            if (ProdType == "Standard MESH" || ProdType == "MESH")
                                            {
                                                lEmailCc = lEmailCc + ";planning_mesh@natsteel.com.sg";
                                            }
                                            if (ProdType == "BPC" || ProdType == "PRC")
                                            {
                                                lEmailCc = lEmailCc + ";planning_cage@natsteel.com.sg";
                                            }

                                        }
                                        else
                                        {
                                            if (ProdType == "Rebar")
                                            {
                                                //lEmailCc = "planning_cab@natsteel.com.sg";
                                            }
                                            if (ProdType == "Standard MESH")
                                            {
                                                lEmailCc = "planning_mesh@natsteel.com.sg";
                                            }
                                            if (ProdType == "BPC" || ProdType == "PRC")
                                            {
                                                lEmailCc = "planning_cage@natsteel.com.sg";
                                            }
                                        }
                                    }
                                    lVar = "";
                                    if (lCustomer != null) lVar = lCustomer.CustomerName.Trim();
                                    lEmailSubject = lVar + " - " + JobContent.PONumber.Trim() + " - CAB/SB No. " + JobID.ToString() + " Cancelled";
                                    //lEmailSubject = "JOB ADVICE - " + lVar + " - CAB/SB No. " + JobID.ToString() + " (PO No.:" + JobContent.PONumber.Trim() + ") (工作通知 - 加工铁与标准直铁料表 第" + JobID.ToString() + "号)";
                                }
                                if (ProdType == "Standard MESH")
                                {
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

                                    string lUserID = User.Identity.GetUserName().ToUpper();
                                    if (lUserID.IndexOf("@") > 0)
                                    {
                                        lUserID = lUserID.Substring(0, lUserID.IndexOf("@"));
                                    }
                                    lEmailContent = "<p align='center'>CANCELLED JOB ADVICE - Standard Products (取消工作通知 / 标准产品订单) - " + lUserID + "</p>";

                                    lEmailContent = lEmailContent + "<table border='1' width=100%><tr>";
                                    lEmailContent = lEmailContent + "<td width=20%>" + "Customer (客户名称)" + "</td>";

                                    CustomerModels lCustomer = db.Customer.Find(JobContent.CustomerCode);
                                    string lVar = "";
                                    if (lCustomer != null) lVar = lCustomer.CustomerName.Trim() + " (" + JobContent.CustomerCode.Trim() + ")";
                                    lEmailContent = lEmailContent + "<td>" + lVar + "</td></tr>";

                                    lEmailContent = lEmailContent + "<tr><td>" + "Project (工程项目)" + "</td>";

                                    lVar = "";

                                    var lProject = db.Project.Find(CustomerCode, ProjectCode);
                                    if (lProject != null) lVar = lProject.ProjectTitle.Trim() + " (" + JobContent.ProjectCode.Trim() + ")";
                                    lEmailContent = lEmailContent + "<td>" + lVar + "</td></tr></table>";

                                    lEmailContent = lEmailContent + "<table border='1' width=100%><tr>";

                                    lEmailContent = lEmailContent + "<td width=20%>" + "PO No.\n(客户订单号码)" + "</td>";
                                    lEmailContent = lEmailContent + "<td width=27%>" + JobContent.PONumber.Trim() + "</td>";
                                    lEmailContent = lEmailContent + "<td width=26%>" + "Order Date\n(销售订单日期)" + "</td>";
                                    lEmailContent = lEmailContent + "<td>" + String.Format("{0:yyyy-MM-dd}", JobContent.PODate) + "</td></tr>";

                                    lEmailContent = lEmailContent + "<tr><td width=20%>" + "Required Date\n(交货日期)" + "</td>";
                                    lEmailContent = lEmailContent + "<td width=27%>" + String.Format("{0:yyyy-MM-dd}", JobContent.RequiredDate) + "</td>";
                                    lEmailContent = lEmailContent + "<td width=26%>" + "SO Number\n(订单号码)" + "</td>";
                                    lEmailContent = lEmailContent + "<td>" + JobContent.SAPSONo + "</td></tr>";

                                    lEmailContent = lEmailContent + "<tr><td width=20%>" + "Total Pieces\n(总件数)" + "</td>";
                                    lEmailContent = lEmailContent + "<td width=27%>" + ((int)JobContent.TotalPcs).ToString() + "</td>";
                                    lEmailContent = lEmailContent + "<td width=26%>" + "Total Weight\n(总重量)" + "</td>";
                                    lEmailContent = lEmailContent + "<td>" + ((decimal)JobContent.TotalWeight).ToString("F3") + " KG" + "</td></tr></table>";

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
                                        lEmailContent = lEmailContent + "<td width='5%'>" + "S/N\n序号" + "</td>";
                                        lEmailContent = lEmailContent + "<td width='10%' bgcolor='#ccffcc'>" + "Product Code\n产品代码" + "</td>";
                                        lEmailContent = lEmailContent + "<td width='8%'>" + "Main Length\n主筋长" + "</td>";
                                        lEmailContent = lEmailContent + "<td width='8%'>" + "Cross Length\n副筋长" + "</td>";
                                        lEmailContent = lEmailContent + "<td width='9%'>" + "MW Size\n主筋直径" + "</td>";
                                        lEmailContent = lEmailContent + "<td width='10%'>" + "MW Spacing\n主筋间距" + "</td>";
                                        lEmailContent = lEmailContent + "<td width='9%'>" + "CW Size\n副筋直径" + "</td>";
                                        lEmailContent = lEmailContent + "<td width='10%'>" + "CW Spacing\n副筋间距" + "</td>";
                                        lEmailContent = lEmailContent + "<td width='10%'>" + "Unit Weight\n单片重量" + "</td>";
                                        lEmailContent = lEmailContent + "<td width='10%' bgcolor='#ccffcc'>" + "Order Qty\n订购件数" + "</td>";
                                        lEmailContent = lEmailContent + "<td>" + "Total Weight\n总重量" + "</td>";
                                        lEmailContent = lEmailContent + "</td></tr>";

                                        for (int i = 0; i < lBBSContent.Count; i++)
                                        {
                                            lEmailContent = lEmailContent + "<tr><td align='center'> <font color='blue'>" + (i + 1).ToString() + "</font></td>";
                                            lEmailContent = lEmailContent + "<td align='center'><font color='blue' bgcolor='#ccffcc'>" + lBBSContent[i].sheet_name + "</font></td>";
                                            lEmailContent = lEmailContent + "<td align='center'><font color='blue'>" + lBBSContent[i].mw_length.ToString() + "</font></td>";
                                            lEmailContent = lEmailContent + "<td align='center'><font color='blue'>" + lBBSContent[i].cw_length.ToString() + "</font></td>";
                                            lEmailContent = lEmailContent + "<td align='center'><font color='blue'>" + lBBSContent[i].mw_size.ToString() + "</font></td>";
                                            lEmailContent = lEmailContent + "<td align='center'><font color='blue'>" + lBBSContent[i].mw_spacing.ToString() + "</font></td>";
                                            lEmailContent = lEmailContent + "<td align='center'><font color='blue'>" + lBBSContent[i].cw_size.ToString() + "</font></td>";
                                            lEmailContent = lEmailContent + "<td align='center'><font color='blue'>" + lBBSContent[i].cw_spacing.ToString() + "</font></td>";
                                            lEmailContent = lEmailContent + "<td align='right'><font color='blue'>" + lBBSContent[i].unit_weight.ToString("F3") + "</font></td>";
                                            if (lBBSContent[i].order_pcs == 0) lVar = ""; else lVar = lBBSContent[i].order_pcs.ToString();
                                            lEmailContent = lEmailContent + "<td align='center'><font color='blue' bgcolor='#ccffcc'><strong>" + lVar + "</strong></font></td>";
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

                                    lEmailContent = lEmailContent + "<td width='20%'>" + "Site Contact\n(联系人)" + "</td>";
                                    lEmailContent = lEmailContent + "<td width='15%'>" + JobContent.SiteEngr_Name.Trim() + "</td>";
                                    lEmailContent = lEmailContent + "<td width='13%'>" + "Handphone\n(手机号码)" + " </td>";
                                    lEmailContent = lEmailContent + "<td width='16%'>" + JobContent.SiteEngr_HP.Trim() + "</td>";
                                    lEmailContent = lEmailContent + "<td width='13%'>" + "Email\n(电邮地址)" + "</td>";
                                    lEmailContent = lEmailContent + "<td>" + JobContent.SiteEngr_Tel.Trim() + "</td></tr>";

                                    lEmailContent = lEmailContent + "<tr><td width='20%'>" + "Goods Receiver\n(收货人)" + "</td>";
                                    lEmailContent = lEmailContent + "<td width='15%'>" + JobContent.Scheduler_Name.Trim() + "</td>";
                                    lEmailContent = lEmailContent + "<td width='13%'>" + "Handphone\n(手机号码)" + " </td>";
                                    lEmailContent = lEmailContent + "<td width='16%'>" + JobContent.Scheduler_HP.Trim() + "</td>";
                                    lEmailContent = lEmailContent + "<td width='13%'>" + "Email\n(电邮地址)" + "</td>";
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

                                    //lVar1 = JobContent.Scheduler_Tel.Trim();
                                    //if (lVar1 != "")
                                    //{
                                    //    if (lEmailTo == "") { lEmailTo = lVar1; }
                                    //    else { lEmailTo = lEmailTo + ";" + lVar1; }
                                    //}

                                    //lVar1 = JobContent.SiteEngr_Tel.Trim();
                                    //if (lVar1 != "")
                                    //{
                                    //    if (lEmailTo.IndexOf(lVar1) < 0)
                                    //    {
                                    //        if (lEmailTo == "") { lEmailTo = lVar1; }
                                    //        else { lEmailTo = lEmailTo + ";" + lVar1; }
                                    //    }
                                    //}

                                    // CC to Planning_mesh
                                    if (strServer == "DEV")
                                    {
                                        if (lEmailCc.Length > 0)
                                        {
                                            lEmailCc = lEmailCc + ";zbc@natsteel.com.sg";
                                        }
                                        else
                                        {
                                            lEmailCc = "zbc@natsteel.com.sg";
                                        }
                                    }
                                    else
                                    {
                                        if (lEmailCc.Length > 0)
                                        {
                                            if (ProdType == "Rebar")
                                            {
                                                //lEmailCc = lEmailCc + ";planning_cab@natsteel.com.sg";
                                            }
                                            if (ProdType == "Standard MESH" || ProdType == "MESH")
                                            {
                                                lEmailCc = lEmailCc + ";planning_mesh@natsteel.com.sg";
                                            }
                                            if (ProdType == "BPC" || ProdType == "PRC")
                                            {
                                                lEmailCc = lEmailCc + ";planning_cage@natsteel.com.sg";
                                            }
                                        }
                                        else
                                        {
                                            if (ProdType == "Rebar")
                                            {
                                                //lEmailCc = "planning_cab@natsteel.com.sg";
                                            }
                                            if (ProdType == "Standard MESH")
                                            {
                                                lEmailCc = "planning_mesh@natsteel.com.sg";
                                            }
                                            if (ProdType == "BPC" || ProdType == "PRC")
                                            {
                                                lEmailCc = "planning_cage@natsteel.com.sg";
                                            }
                                        }
                                    }

                                    lVar = "";
                                    if (lCustomer != null) lVar = lCustomer.CustomerName.Trim();
                                    lEmailSubject = lVar + " - " + JobContent.PONumber.Trim() + " - Standard Products No. " + JobID.ToString() + " Cancelled ";
                                    //lEmailSubject = JobContent.PONumber.Trim() + " - " + lVar + " - MESH Standard Sheet No. " + JobID.ToString() + " Cancelled ";
                                    //lEmailSubject = "JOB ADVICE - " + lVar + " - CAB/SB No. " + JobID.ToString() + " (PO No.:" + JobContent.PONumber.Trim() + ") (工作通知 - 加工铁与标准直铁料表 第" + JobID.ToString() + "号)";


                                }
                                if (ProdType == "BPC")
                                {
                                    bool lTemplate = false;

                                    var JobContent = db.BPCJobAdvice.Find(CustomerCode, ProjectCode, lTemplate, JobID);
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

                                    lEmailContent = "<p align='center'>CANCELLED JOB ADVICE - Bored Pile Cage (取消工作通知 - 钻孔桩铁笼)</p>";

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

                                    var lOrcCmd = new OracleCommand();
                                    OracleDataReader lOrcRst;
                                    var lcisCon = new OracleConnection();

                                    if (lProcessObj.OpenCISConnection(ref lcisCon) == true)
                                    {

                                        lOrcCmd.CommandText = "SELECT BEZEI as DESCRIPTION " +
                                        "FROM SAPSR3.TMFGT WHERE MANDT = '" + lProcessObj.strClient + "' AND SPRAS ='E' " +
                                        "AND MFRGR = '" + JobContent.Transport + "' ";

                                        lOrcCmd.Connection = lcisCon;
                                        lOrcCmd.CommandTimeout = 300;
                                        lOrcRst = lOrcCmd.ExecuteReader();
                                        if (lOrcRst.HasRows)
                                        {
                                            if (lOrcRst.Read())
                                            {
                                                lVar = lOrcRst.GetString(0).Trim();
                                                lProcessObj.CloseCISConnection(ref lcisCon);
                                            }
                                        }
                                        lOrcRst.Close();
                                    }
                                    lOrcCmd = null;
                                    lcisCon = null;
                                    lOrcRst = null;
                                    lProcessObj = null;



                                    lEmailContent = lEmailContent + "<td>" + lVar + "</td></tr>";

                                    lEmailContent = lEmailContent + "</tr><td width=20%>" + "Total Pieces (总件数)" + "</td>";
                                    lEmailContent = lEmailContent + "<td width=27%>" + ((int)JobContent.TotalPcs).ToString() + "</td>";

                                    lEmailContent = lEmailContent + "<td width=26%>" + "Total Weight (总重量)" + "</td>";

                                    lEmailContent = lEmailContent + "<td>" + ((decimal)JobContent.TotalWeight).ToString("F3") + " KG" + "</td></tr></table>";

                                    lEmailContent = lEmailContent + "<table border='1' width=100%><tr>";
                                    lEmailContent = lEmailContent + "<td width='3%'>" + "S/N<br/>序号" + "</td>";
                                    lEmailContent = lEmailContent + "<td width='6%'>" + "Pile Diameter<br/>桩直径" + "</td>";
                                    lEmailContent = lEmailContent + "<td width='12%'>" + "Cage Type<br/>铁笼类型" + "</td>";
                                    lEmailContent = lEmailContent + "<td width='9%'>" + "No of Main Bars<br/>主筋数量" + "</td>";
                                    lEmailContent = lEmailContent + "<td width='7%'>" + "Main Bar Shape<br/>主筋图形" + "</td>";
                                    lEmailContent = lEmailContent + "<td width='5%'>" + "Cage Length<br/>铁笼长" + "</td>";
                                    lEmailContent = lEmailContent + "<td width='5%'>" + "Lap Length<br/>顶端重叠长" + "</td>";
                                    lEmailContent = lEmailContent + "<td width='14%'>" + "Link Dia-Spacing-Length<br/>弧状链的直径,间距与长度" + "</td>";
                                    lEmailContent = lEmailContent + "<td width='7%'>" + "End Length<br/>底端预留长度" + "</td>";
                                    lEmailContent = lEmailContent + "<td width='5%'>" + "Cage Qty<br/>铁笼件数" + "</td>";
                                    lEmailContent = lEmailContent + "<td width='11%'>" + "Combination of Cages<br/>上端, 中间还是低端铁笼" + "</td>";
                                    lEmailContent = lEmailContent + "<td width='6%'>" + "Weight(MT)<br/>重量(吨)" + "</td>";
                                    lEmailContent = lEmailContent + "<td width='5%'>" + "BBS No.<br/>铁笼号码" + "</td>";
                                    lEmailContent = lEmailContent + "<td>" + "Remarks<br/>备注" + "</td>";
                                    lEmailContent = lEmailContent + "</td></tr>";

                                    var lBBSContent = (from p in db.BPCDetails
                                                       where p.CustomerCode == CustomerCode &&
                                                       p.ProjectCode == ProjectCode &&
                                                       p.Template == false &&
                                                       p.JobID == JobID
                                                       orderby p.cage_id
                                                       select p).ToList();

                                    if (lBBSContent.Count > 0)
                                    {
                                        for (int i = 0; i < lBBSContent.Count; i++)
                                        {
                                            lEmailContent = lEmailContent + "<tr><td align='center'> <font color='blue'>" + (i + 1).ToString() + "</font></td>";
                                            lEmailContent = lEmailContent + "<td align='center'><font color='blue'>" + lBBSContent[i].pile_dia.ToString() + "</font></td>";
                                            //Cage Type
                                            lVar = "";
                                            var lPileType = lBBSContent[i].pile_type;
                                            var lMainBarArrange = lBBSContent[i].main_bar_arrange;
                                            var lMainBarType = lBBSContent[i].main_bar_type;
                                            if (lPileType == "Single-Layer")
                                            {
                                                if (lMainBarType == "Single")
                                                {
                                                    if (lMainBarArrange == "Single")
                                                    {
                                                        lVar = "Single Layer";
                                                    }
                                                    else if (lMainBarArrange == "Side-By-Side")
                                                    {
                                                        lVar = "Single Layer<br/>Side-By-Side Bundled Bars";
                                                    }
                                                    else if (lMainBarArrange == "In-Out")
                                                    {
                                                        lVar = "Single Layer<br/>In-Out Bundled Bars";
                                                    }
                                                    else
                                                    {
                                                        lVar = "Single Layer<br/>Complex Bundled Bars";
                                                    }
                                                }
                                                if (lMainBarType == "Mixed")
                                                {
                                                    if (lMainBarArrange == "Single")
                                                    {
                                                        lVar = "Single Layer<br/>Mixed Bars";
                                                    }
                                                    else if (lMainBarArrange == "Side-By-Side")
                                                    {
                                                        lVar = "Single Layer<br/>Side By Side Bundled<br/>Mixed Bars";
                                                    }
                                                    else if (lMainBarArrange == "In-Out")
                                                    {
                                                        lVar = "Single Layer<br/>In-Out Bundled<br/>Mixed Bars";
                                                    }
                                                    else
                                                    {
                                                        lVar = "Single Layer<br/>Complex Bundled<br/>Mixed Bars";
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (lMainBarArrange == "Single")
                                                {
                                                    lVar = "Double Layer";
                                                }
                                                else if (lMainBarArrange == "Side-By-Side")
                                                {
                                                    lVar = "Double Layer<br/>Side By Side Bundled Bars";
                                                }
                                                else
                                                {
                                                    lVar = "Double Layer<br/>Complex Bundled Bars";
                                                }

                                            }

                                            lEmailContent = lEmailContent + "<td align='center'><font color='blue'>" + lVar + "</font></td>";

                                            lVar = "";
                                            var lBarCTArr = lBBSContent[i].main_bar_ct.Split(',');
                                            var lBarDiaArr = lBBSContent[i].main_bar_dia.Split(',');
                                            var lBarType = lBBSContent[i].main_bar_grade.Trim();
                                            if (lBarCTArr.Length > 0 && lBarDiaArr.Length > 0)
                                            {
                                                lVar = lBarCTArr[0].Trim() + lBarType + lBarDiaArr[0].Trim();
                                            }
                                            if (lBarCTArr.Length > 1 && lBarDiaArr.Length > 1)
                                            {
                                                lVar = lVar + "<br/>" + lBarCTArr[1].Trim() + lBarType + lBarDiaArr[1].Trim();
                                            }

                                            lEmailContent = lEmailContent + "<td align='center'><font color='blue'>" + lVar + "</font></td>";
                                            lEmailContent = lEmailContent + "<td align='center'><font color='blue'>" + lBBSContent[i].main_bar_shape + "</font></td>";
                                            lEmailContent = lEmailContent + "<td align='center'><font color='blue'>" + lBBSContent[i].cage_length.ToString() + "</font></td>";
                                            lEmailContent = lEmailContent + "<td align='center'><font color='blue'>" + lBBSContent[i].lap_length.ToString() + "</font></td>";

                                            lVar = "";
                                            var lSLType = "";
                                            if (lBBSContent[i].spiral_link_type.Length >= 5)
                                            {
                                                if (lBBSContent[i].spiral_link_type.Substring(0, 5) == "Others")
                                                {
                                                    lSLType = "";
                                                }
                                                else if (lBBSContent[i].spiral_link_type.Substring(0, 4) == "Twin")
                                                {
                                                    lSLType = "2";
                                                }
                                            }
                                            var lSLSpacing = lBBSContent[i].spiral_link_spacing.Split(',');
                                            var lSLGrade = lBBSContent[i].spiral_link_grade.Trim();
                                            if (lSLSpacing.Length > 0 && lSLSpacing.Length > 0)
                                            {
                                                lVar = lSLType + lSLGrade + lBBSContent[i].sl1_dia + "-" + lSLSpacing[0] + "-" + lBBSContent[i].sl1_length;
                                            }
                                            if (lSLSpacing.Length > 1 && lSLSpacing.Length > 1)
                                            {
                                                lVar = lVar + "<br/>" + lSLType + lSLGrade + lBBSContent[i].sl2_dia + "-" + lSLSpacing[1] + "-" + lBBSContent[i].sl2_length;
                                            }
                                            if (lSLSpacing.Length > 2 && lSLSpacing.Length > 2)
                                            {
                                                lVar = lVar + "<br/>" + lSLType + lSLGrade + lBBSContent[i].sl3_dia + "-" + lSLSpacing[2] + "-" + lBBSContent[i].sl3_length;
                                            }

                                            lEmailContent = lEmailContent + "<td align='center'><font color='blue'>" + lVar + "</font></td>";
                                            lEmailContent = lEmailContent + "<td align='right'><font color='blue'>" + lBBSContent[i].end_length.ToString() + "</font></td>";
                                            lEmailContent = lEmailContent + "<td align='right'><font color='blue'>" + lBBSContent[i].cage_qty.ToString() + "</font></td>";
                                            lEmailContent = lEmailContent + "<td align='right'><font color='blue'>" + lBBSContent[i].cage_location + "</font></td>";
                                            lEmailContent = lEmailContent + "<td align='right'><font color='blue'>" + lBBSContent[i].cage_weight.ToString("F3") + "</font></td>";
                                            lEmailContent = lEmailContent + "<td align='right'><font color='blue'>" + lBBSContent[i].bbs_no + "</font></td>";
                                            lEmailContent = lEmailContent + "<td align='right'><font color='blue'>" + lBBSContent[i].cage_remarks + "</font></td>";
                                            lEmailContent = lEmailContent + "</tr>";
                                        }
                                    }
                                    lEmailContent = lEmailContent + "</table>";

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

                                    if (strServer == "DEV")
                                    {
                                        if (lEmailCc.Length > 0)
                                        {
                                            lEmailCc = lEmailCc + ";zbc@natsteel.com.sg";
                                        }
                                        else
                                        {
                                            lEmailCc = "zbc@natsteel.com.sg";
                                        }
                                    }
                                    else
                                    {
                                        if (lEmailCc.Length > 0)
                                        {
                                            if (ProdType == "Rebar")
                                            {
                                                //lEmailCc = lEmailCc + ";planning_cab@natsteel.com.sg";
                                            }
                                            if (ProdType == "Standard MESH" || ProdType == "MESH")
                                            {
                                                lEmailCc = lEmailCc + ";planning_mesh@natsteel.com.sg";
                                            }
                                            if (ProdType == "BPC" || ProdType == "PRC")
                                            {
                                                lEmailCc = lEmailCc + ";planning_cage@natsteel.com.sg";
                                            }

                                        }
                                        else
                                        {
                                            if (ProdType == "Rebar")
                                            {
                                                //lEmailCc = "planning_cab@natsteel.com.sg";
                                            }
                                            if (ProdType == "Standard MESH")
                                            {
                                                lEmailCc = "planning_mesh@natsteel.com.sg";
                                            }
                                            if (ProdType == "BPC" || ProdType == "PRC")
                                            {
                                                lEmailCc = "planning_cage@natsteel.com.sg";
                                            }
                                        }
                                    }

                                    lVar = "";
                                    if (lCustomer != null) lVar = lCustomer.CustomerName.Trim();

                                    lEmailSubject = lVar + " - " + JobContent.PONumber.Trim() + " - Bored Pile Cage No. " + JobID.ToString();

                                }

                                //// var lOESEmail = new SendGridEmail();

                                // string lEmailFromAddress = "eprompt@natsteel.com.sg";
                                // string lEmailFromName = "Digital Ordering Email Services";

                                // //dynamic response = lOESEmail.Execute(lEmailFromAddress, lEmailFromName, lEmailTo, lEmailCc, lEmailSubject, "text/html", lEmailContent);
                                // lOESEmail.Execute(lEmailFromAddress, lEmailFromName, lEmailTo, lEmailCc, lEmailSubject, "text/html", lEmailContent).Wait();
                                // lOESEmail = null;ajit
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    if (oracleTransCIS != null) oracleTransCIS.Rollback();
                    if (oracleTransIDB != null) oracleTransIDB.Rollback();
                    CloseNDSConnection(ref cnNDS);
                    CloseIDBConnection(ref cnIDB);
                    CloseCISConnection(ref cnCIS);
                    lErrorMsg = "Cancel error: " + ex.Message;
                    lReturn = false;
                    //SaveErrorMsg(ex.Message, ex.StackTrace);
                }
            }

            lCmd = null;
            lRst = null;

            //return Json((success: lReturn, message: lErrorMsg));
            // return Ok(lReturn);
            // return Request.CreateResponse(HttpStatusCode.OK);
            var result = Request.CreateResponse(HttpStatusCode.OK);
            result.Content = new StringContent(lReturn.ToString(), Encoding.UTF8, "application/json");
            return result;
        }


        public Boolean OpenNDSConnection(ref SqlConnection objConnection)
        {

            SetConnectString();

            CloseNDSConnection(ref objConnection);
            if (objConnection == null)
            {
                objConnection = new SqlConnection();
            }

            objConnection.ConnectionString = strNDS_Connection;
            //objConnection.ConnectionTimeout = 200;
            try
            {
                objConnection.Open();
            }
            catch (Exception ex)
            {
                //SaveErrorMsg(ex.Message, ex.StackTrace);
                string lErrorMsg = ex.Message;
                return false;
            }

            return true;
        }

        public Boolean CloseNDSConnection(ref SqlConnection objConnection)
        {
            if (objConnection != null)
            {
                if (objConnection.State == ConnectionState.Open)
                {
                    objConnection.Close();
                }
            }
            return true;
        }

        public Boolean SetConnectString()
        {
            if (strServer == "PRD")
            {
                strNDS_Connection = "Data Source=nsprddb10\\MSSQL2022;Initial Catalog=ODOS;User ID=ndswebapps; Password=DBAdmin4*NDS; MultipleActiveResultSets=false; Connection Timeout=120";
                strCIS_Connection = "Data Source=(DESCRIPTION = (ADDRESS = (COMMUNITY = SAP.WORLD)(PROTOCOL = TCP)(HOST = 172.25.101.1)(PORT = 1527)) (CONNECT_DATA = (SID = NSP) (SERVER = DEDICATED) (SERVICE_NAME = NSP)));Persist Security Info=True;User ID=ORAINTSAP;Password=ORAintsap01;Connect Timeout=300";
                strIDB_Connection = "Data Source=(DESCRIPTION = (ENABLE=BROKEN)(ADDRESS = (COMMUNITY = IDBPROD)(PROTOCOL = TCP)(HOST = nsprdscmlsnr.natsteel.corp)(PORT = 1521)) (CONNECT_DATA = (SERVER = DEDICATED) (SERVICE_NAME = IDBPROD)));Persist Security Info=True;User ID=SCM_ADMIN;Password=scm4admin; Connection Timeout=300";
                strSTS_Connection = "Data Source=NSDB11,1433;Initial Catalog=STS;User ID=stsapp; Password=stsapp123; Connection Timeout=120";
                strNGW_Connection = "Data Source=NSDB11,1433;Initial Catalog=NGWS;User ID=stsapp; Password=stsapp123; Connection Timeout=120";

                //strCIS_Connection = "Data Source=(DESCRIPTION = (ADDRESS = (COMMUNITY = SAP.WORLD)(PROTOCOL = TCP)(HOST = 10.168.101.177)(PORT = 1527)) (CONNECT_DATA = (SID = NSP) (SERVER = DEDICATED) (SERVICE_NAME = NSP)));Persist Security Info=True;User ID=ORAINTSAP;Password=ORAintsap01;";
                //strCIS_Connection = "Data Source=(DESCRIPTION = (ADDRESS = (COMMUNITY = SAP.WORLD)(PROTOCOL = TCP)(HOST = 172.25.101.1)(PORT = 1527)) (CONNECT_DATA = (SID = NSP) (SERVER = DEDICATED) (SERVICE_NAME = NSP)));Persist Security Info=True;User ID=ORAINTSAP;Password=ORAintsap01;Connect Timeout=300";
                //strNDS_Connection = "Provider=sqloledb; Data Source=NSDB10\\SQL2005_1,1433;Initial Catalog=NDSDB;User ID=ots_support; Password=ots1nprod";
                //strNDS_Connection = "Provider=sqloledb; Data Source=NSPRDDB10,1433;Initial Catalog=NDSDB;User ID=ots_support; Password=ots1nprod";
                //strNDS_Connection = "Data Source=NSPRDDB10,1433;Initial Catalog=NDSDB;User ID=ots_support; Password=ots1nprod; MultipleActiveResultSets=false; Connection Timeout=120";
                //strIDB_Connection = "Data Source=(DESCRIPTION = (ADDRESS = (COMMUNITY = IDBPROD)(PROTOCOL = TCP)(HOST = nsprdscmlsnr.natsteel.corp)(PORT = 1521)) (CONNECT_DATA = (SERVER = DEDICATED) (SERVICE_NAME = IDBPROD)));Persist Security Info=True;User ID=SCM_ADMIN;Password=scm4admin;";
                //strIDB_Connection = "Data Source=(DESCRIPTION = (ADDRESS = (COMMUNITY = IDBPROD)(PROTOCOL = TCP)(HOST = nsprdscmvdb01)(PORT = 1521)) (CONNECT_DATA = (SERVER = DEDICATED) (SERVICE_NAME = IDBPROD)));Persist Security Info=True;User ID=SCM_ADMIN;Password=scm4admin; Connection Timeout=300";
                //strIDB_Connection = "Data Source=(DESCRIPTION = (ENABLE=BROKEN)(ADDRESS = (COMMUNITY = IDBPROD)(PROTOCOL = TCP)(HOST = nsprdscmlsnr.natsteel.corp)(PORT = 1521)) (CONNECT_DATA = (SERVER = DEDICATED) (SERVICE_NAME = IDBPROD)));Persist Security Info=True;User ID=SCM_ADMIN;Password=scm4admin; Connection Timeout=300";
                //strSTS_Connection = "Data Source=NSDB11,1433;Initial Catalog=STS;User ID=stsapp; Password=stsapp123; Connection Timeout=120";
                //strNGW_Connection = "Data Source=NSDB11,1433;Initial Catalog=NGWS;User ID=stsapp; Password=stsapp123; Connection Timeout=120";

            }

            else
            {

                //strCIS_Connection = "Data Source=(DESCRIPTION = (ADDRESS_LIST = (ADDRESS = (COMMUNITY = SAP.WORLD)(PROTOCOL = TCP)(HOST = 10.168.101.5)(PORT = 1527)))(CONNECT_DATA = (SID = NSQ)(GLOBAL_NAME = NSQ.WORLD)));Persist Security Info=True;User ID=Sapsr3;Password=krishna123;";
                //Local
                strCIS_Connection = "Data Source=(DESCRIPTION = (ADDRESS_LIST = (ADDRESS = (COMMUNITY = SAP.WORLD)(PROTOCOL = TCP)(HOST = 172.25.101.5)(PORT = 1527)))(CONNECT_DATA = (SID = NSQ)(GLOBAL_NAME = NSQ.WORLD)));Persist Security Info=True;User ID=Sapsr3;Password=nspsap123;Connection Timeout=300";
                //Server
                //strCIS_Connection = "Data Source=(DESCRIPTION = (ADDRESS_LIST = (ADDRESS = (COMMUNITY = SAP.WORLD)(PROTOCOL = TCP)(HOST = 172.26.0.15)(PORT = 1527)))(CONNECT_DATA = (SID = NSQ)(GLOBAL_NAME = NSQ.WORLD)));Persist Security Info=True;User ID=Sapsr3;Password=krishna123;Connection Timeout=300";
                //strNDS_Connection = "Provider=sqloledb; Data Source=NSQADB4\\SQL2005_1;Initial Catalog=NDSPRD;User ID=ots_support; Password=ots1nqa";
                //strNDS_Connection = "Provider=sqloledb; Data Source=NSQADB5,1433;Initial Catalog=NDSPRD;User ID=ots_support; Password=ots1nqa";
                strNDS_Connection = "Data Source=NSQADB5\\MSSQL2022; Initial Catalog=ODOS;User ID=NDSWebApps; Password=NDS4DBAdmin*;MultipleActiveResultSets=false; Connection Timeout=120";
                strIDB_Connection = "Data Source=(DESCRIPTION = (ADDRESS = (COMMUNITY = SCMIDBQA)(PROTOCOL = TCP)(HOST = 172.18.1.134)(PORT = 1525)) (CONNECT_DATA = (SERVER = DEDICATED) (SERVICE_NAME = SCMIDBQA)));Persist Security Info=True;User ID=SCM_ADMIN;Password=scm4admin;Connection Timeout=300";
                strSTS_Connection = "Data Source=NSQADB2,1433;Initial Catalog=STS;User ID=stsapp; Password=stsapp123";
                strNGW_Connection = "Data Source=NSQADB2,1433;Initial Catalog=NGWS;User ID=stsapp; Password=stsapp123";

            }

            return true;
        }

        public Boolean OpenCISConnection(ref OracleConnection objConnection)
        {

            SetConnectString();

            //Environment.SetEnvironmentVariable("ORACLE_HOME ", ".");
            //Environment.SetEnvironmentVariable("NLS_LANG", "AMERICAN_AMERICA.AL32UTF8");

            //Environment.SetEnvironmentVariable("LD_LIBRARY_PATH", "C:\\instantclient_10_2");
            //Environment.SetEnvironmentVariable("TNS_ADMIN", "C:\\instantclient_10_2");

            CloseCISConnection(ref objConnection);
            if (objConnection == null)
            {
                objConnection = new OracleConnection();
            }

            objConnection.ConnectionString = strCIS_Connection;
            //objConnection.ConnectionTimeout = 200;
            try
            {
                objConnection.Open();
            }
            catch (Exception ex)
            {
                string lErrorMsg = ex.Message;
                //SaveErrorMsg(ex.Message, ex.StackTrace);
                return false;
            }

            return true;
        }

        public Boolean CloseCISConnection(ref OracleConnection objConnection)
        {
            if (objConnection != null)
            {
                if (objConnection.State == ConnectionState.Open)
                {
                    objConnection.Close();
                }
            }
            return true;
        }

        public Boolean OpenIDBConnection(ref OracleConnection objConnection)
        {

            SetConnectString();

            CloseIDBConnection(ref objConnection);
            if (objConnection == null)
            {
                objConnection = new OracleConnection();
            }

            objConnection.ConnectionString = strIDB_Connection;
            //objConnection.ConnectionTimeout = 200;
            try
            {
                objConnection.Open();
            }
            catch (Exception ex)
            {
                //SaveErrorMsg(ex.Message, ex.StackTrace);
                string lErrorMsg = ex.Message;
                return false;
            }

            return true;
        }

        public Boolean CloseIDBConnection(ref OracleConnection objConnection)
        {
            if (objConnection != null)
            {
                if (objConnection.State == ConnectionState.Open)
                {
                    objConnection.Close();
                }
            }
            return true;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("/RemoveGroupMarking/{pCustCode}/{pProject}/{pContract}/{pPostID}/{pStrucEle}/{pProdType}/{pGroupMarking}/{pRev}")]

        async Task<int> RemoveGroupMarking(string pCustCode, string pProject, string pContract, int pPostID,
string pStrucEle, string pProdType, string pGroupMarking, int pRev)
        {
            bool lSuccess = false;
            int lReturn;
            int lPostHeaderID, lStatus, liVar;
            int lUserID;
            string lModular;

            int lProjectID, lWBSEleID, lStruEleID, lProdTypeID;

            string lSQL;
            SqlCommand lCmd;
            SqlDataReader adoRst;

            string AmendDate, lErrorMsg;

            PostGroupMark ndsGM;
            string lBBSNo, lBBSDesc;
            int lGroupMarkId;

            int lRet1, lRet2;


            var myTransport = new HttpTransportBindingElement();
            var muEncoding = new BinaryMessageEncodingBindingElement();

            var binding = new CustomBinding();
            //var binding = new BasicHttpBinding();

            //Specify the address to be used for the client.

            EndpointAddress address;
            var ndsCAPCLinks = new List<CapClink>();
            List<CapClink> ndsCAPCLinkList;

            //Create a client that is configured with this address and binding.
            // var ndsClient = new NDSPosting.BBSPostingServiceClient();


            var ndsPostedGMs = new List<PostGroupMark>();

            string lProjectCode = "";

            lSuccess = true;
            lReturn = 0;
            AmendDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            try
            {
                lBBSNo = "";
                lPostHeaderID = 0;
                lStatus = 0;
                lProjectID = 0;
                lBBSDesc = "";
                lWBSEleID = 0;
                lStruEleID = 0;
                lProdTypeID = 0;
                //Get Project ID

                lProjectCode = pProject;
                //Get Project ID
                lSQL = "SELECT intProjectId FROM dbo.SAPProjectMaster P WHERE P.vchProjectCode = '" + pProject + "' ";
                lCmd = new SqlCommand(lSQL, cnNDS);
                lCmd.Transaction = osqlTransNDS;
                lCmd.CommandTimeout = 1200;
                adoRst = lCmd.ExecuteReader();
                if (adoRst.HasRows)
                {
                    adoRst.Read();
                    lProjectID = (int)adoRst.GetValue(0);
                }
                adoRst.Close();

                lUserID = 11;

                //string luserAddr = User.Identity.GetUserName();
                //if (luserAddr.IndexOf("@") > 0)
                //{
                //    luserAddr = luserAddr.Substring(0, luserAddr.IndexOf("@"));
                //}

                //lSQL = "SELECT intUserId FROM dbo.NDSUsers WHERE vchLoginId = '" + luserAddr + "' ";
                //lCmd = new SqlCommand(lSQL, cnNDS);
                //lCmd.CommandTimeout = 1200;
                //adoRst = lCmd.ExecuteReader();
                //if (adoRst.HasRows)
                //{
                //    if (adoRst.Read())
                //    {
                //        lUserID = adoRst.GetValue(0) == DBNull.Value ? 111 : adoRst.GetInt32(0);
                //    }
                //}

                lSQL = "SELECT intPostHeaderId,  tntStatusId, intWBSElementId, intStructureElementTypeId, " +
                "sitProductTypeId, vchBBSNo, BBS_DESC " +
                "FROM dbo.BBSPostHeader " +
                "where intPostHeaderid = " + pPostID.ToString() + "  ";

                lCmd = new SqlCommand(lSQL, cnNDS);
                lCmd.CommandTimeout = 1200;
                adoRst = lCmd.ExecuteReader();
                if (adoRst.HasRows)
                {
                    if (adoRst.Read())
                    {
                        lPostHeaderID = adoRst.GetValue(0) == DBNull.Value ? 0 : adoRst.GetInt32(0);
                        lStatus = adoRst.GetValue(1) == DBNull.Value ? 0 : adoRst.GetByte(1);
                        lWBSEleID = adoRst.GetValue(2) == DBNull.Value ? 0 : adoRst.GetInt32(2);
                        lStruEleID = adoRst.GetValue(3) == DBNull.Value ? 0 : adoRst.GetInt32(3);
                        lProdTypeID = adoRst.GetValue(4) == DBNull.Value ? 0 : adoRst.GetInt16(4);
                        lBBSNo = adoRst.GetValue(5) == DBNull.Value ? "" : adoRst.GetString(5);
                        lBBSDesc = adoRst.GetValue(6) == DBNull.Value ? "" : adoRst.GetString(6);
                    }
                }
                adoRst.Close();

                if (lBBSNo.Length == 0)
                {
                    //lReturn = -1;
                    //return lReturn;
                }

                lSQL = "SELECT tntStatusId FROM dbo.BBSReleaseDetails WHERE intPostHeaderid = " + lPostHeaderID + " ";

                lCmd = new SqlCommand(lSQL, cnNDS);
                lCmd.CommandTimeout = 1200;
                adoRst = lCmd.ExecuteReader();
                if (adoRst.HasRows)
                {
                    if (adoRst.Read())
                    {
                        liVar = adoRst.GetValue(0) == DBNull.Value ? 0 : adoRst.GetByte(0);
                        if (liVar == 12)
                        {
                            lStatus = liVar;
                        }

                    }
                }
                adoRst.Close();

                // Check the header ID
                if (lPostHeaderID == 0)
                {
                    //Released
                    lReturn = -1;
                    return lReturn;
                }

                if (lStatus == 12)
                {
                    //Released
                    lReturn = -1;
                    return lReturn;
                }

                lGroupMarkId = 0;
                lSQL = "SELECT G.intGroupMarkId " +
                    "FROM dbo.GroupMarkingDetails G, dbo.SELevelDetails S, dbo.SAPProjectMaster P   " +
                    "WHERE G.intGroupMarkId = S.intGroupMarkId " +
                    "AND G.intProjectid = P.intProjectid " +
                    "AND P.intProjectid = 0" + lProjectID + " " +
                    "and S.intStructureElementTypeId in " +
                    "(SELECT intStructureElementTypeId " +
                    "FROM dbo.StructureElementMaster " +
                    "WHERE vchStructureElementType = '" + pStrucEle + "' ) " +
                    "and S.sitProductTypeId in " +
                    "(SELECT sitProductTypeID " +
                    "FROM dbo.ProductTypeMaster " +
                    "WHERE vchProductType = '" + pProdType + "' ) " +
                    "AND vchGroupMarkingName = '" + pGroupMarking + "' " +
                    "AND tntGroupRevNo = " + pRev + " ";

                lCmd = new SqlCommand(lSQL, cnNDS);
                lCmd.CommandTimeout = 1200;
                adoRst = lCmd.ExecuteReader();
                if (adoRst.HasRows)
                {
                    if (adoRst.Read())
                    {
                        lGroupMarkId = adoRst.GetValue(0) == DBNull.Value ? 0 : adoRst.GetInt32(0);
                    }
                }
                adoRst.Close();

                if (lGroupMarkId == 0)
                {
                    //No group marking in NDS
                    //lReturn = -1;
                    //return lReturn;
                }

                ndsGM = new PostGroupMark();
                ndsGM.PostHeaderId = lPostHeaderID;
                ndsGM.BBSNo = lBBSNo;
                ndsGM.BBSRemarks = lBBSDesc;
                ndsGM.GroupMarkId = lGroupMarkId;
                ndsGM.GroupMarkingName = pGroupMarking;
                ndsGM.GroupMarkingRevisionNumber = pRev;
                ndsGM.GroupQty = 1;
                ndsGM.isModular = "N";
                ndsGM.ProjectId = lProjectID;
                ndsGM.Remarks = "";

                myTransport = new HttpTransportBindingElement();
                muEncoding = new BinaryMessageEncodingBindingElement();

                binding = new CustomBinding();
                //binding = new BasicHttpBinding();
                binding.Name = "CustomBinding_IBBSPostingService";
                binding.Elements.Add(muEncoding);
                binding.Elements.Add(myTransport);

                binding.OpenTimeout = new TimeSpan(0, 10, 0);
                binding.CloseTimeout = new TimeSpan(0, 10, 0);
                binding.SendTimeout = new TimeSpan(0, 10, 0);
                binding.ReceiveTimeout = new TimeSpan(0, 10, 0);


                //Specif (y the address to be used for the client.
                //UAT server
                if (strServer == "PRD")
                {
                    //Production Server
                    address = new EndpointAddress("http://172.25.1.141:81/NDSWCF_PV/BBSPostingService.svc");
                }
                else
                {
                    //address = new EndpointAddress("http://172.25.1.224:81/NDSWCF_CAB/BBSPostingService.svc");
                    address = new EndpointAddress("http://172.25.1.224:81/NDSWCF_CUBE/BBSPostingService.svc");
                }

                //Create a client that is configured with this address and binding.
                // ndsClient = new NDSPosting.BBSPostingServiceClient(binding, address);ajit

                //ndsClient.Open();ajit
                lErrorMsg = "";
                lModular = "N";

                if (lStatus == 3)
                {
                    //Posted
                    //lSuccess = ndsClient.UnPostBBS(lPostHeaderID.ToString(), out lErrorMsg);
                    using (var client = new HttpClient())
                    {
                        try
                        {
                            // Define the API endpoint URL
                            string apiUrl = "https://localhost:5006/UnPostBBS_Update/" + lPostHeaderID; // Replace with your API URL


                            // Send an HTTP GET request
                            HttpResponseMessage response = await client.GetAsync(apiUrl);

                            // Check if the request was successful
                            if (response.IsSuccessStatusCode)
                            {
                                // Read the response content as a string
                                string responseContent = await response.Content.ReadAsStringAsync();

                                // Now you can work with the response content (e.g., parse JSON, process data, etc.)
                                Console.WriteLine(responseContent);
                            }
                            else
                            {
                                Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                            }
                        }
                        catch (HttpRequestException ex)
                        {
                            Console.WriteLine($"HTTP request error: {ex.Message}");
                        }
                    }
                    if (lSuccess != true || lErrorMsg.Trim().Length > 0)
                    {
                        //if (ndsClient.State == System.ServiceModel.CommunicationState.Opened)
                        //{
                        //    ndsClient.Close();
                        //}//ajit
                        //MsgBox("WBS (" + pWBS1 + ", " + pWBS2 + ", " + pWBS3 + " had been posted, but the system cannot unpost it." + lErrorMsg)
                        lReturn = -1;
                        return lReturn;
                    }
                }

                if (lGroupMarkId > 0)
                {
                    // lSuccess = ndsClient.DeletePostGroupMarking(ndsGM, out lErrorMsg);//ajit
                    using (var client = new HttpClient())
                    {
                        try
                        {
                            // Define the API endpoint URL
                            string apiUrl = "https://localhost:5006/DeletePostingGroupMarkingDetail"; //+ lPostHeaderID / +lGroupMarkId; // Replace with your API URL

                            string apiUrlWithParameters = string.Format("{0}/{1}/{2}", apiUrl, lPostHeaderID, lGroupMarkId);
                            // Send an HTTP GET request
                            HttpResponseMessage response = await client.GetAsync(apiUrlWithParameters);

                            // Check if the request was successful
                            if (response.IsSuccessStatusCode)
                            {
                                // Read the response content as a string
                                string responseContent = await response.Content.ReadAsStringAsync();

                                // Now you can work with the response content (e.g., parse JSON, process data, etc.)
                                Console.WriteLine(responseContent);
                            }
                            else
                            {
                                Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                            }
                        }
                        catch (HttpRequestException ex)
                        {
                            Console.WriteLine($"HTTP request error: {ex.Message}");
                        }
                    }
                }

                int lLeft = 0;
                lSQL = "SELECT Count(*) " +
                "FROM dbo.BBSPostHeader H, dbo.PostGroupMarkingDetails G " +
                "WHERE H.intPostHeaderid = G.intPostHeaderId " +
                "AND H.intPostHeaderid = " + pPostID.ToString() + "  ";

                lCmd = new SqlCommand(lSQL, cnNDS);
                lCmd.CommandTimeout = 1200;
                adoRst = lCmd.ExecuteReader();
                if (adoRst.HasRows)
                {
                    if (adoRst.Read())
                    {
                        lLeft = adoRst.GetValue(0) == DBNull.Value ? 0 : adoRst.GetInt32(0);
                    }
                }
                adoRst.Close();//ajit

                if (lLeft > 0)
                {
                    //lSuccess = ndsClient.PostBBS(lPostHeaderID, lUserID, lModular, out lErrorMsg);
                    //if (lSuccess != true || lErrorMsg.Trim().Length > 0)
                    //{
                    //    if (ndsClient.State == System.ServiceModel.CommunicationState.Opened)
                    //    {
                    //        ndsClient.Close();
                    //    }
                    //    //MsgBox("Posting WBS (" + pWBS1 + ", " + pWBS2 + ", " + pWBS3 + " error: " + lErrorMsg)
                    //    lReturn = -1;
                    //    return lReturn;
                    //}

                    //ndsCAPCLinks = ndsClient.GetCapping(lPostHeaderID, out lErrorMsg).ToList();

                    using (var client = new HttpClient())
                    {
                        try
                        {
                            // Define the API endpoint URL
                            string apiUrl = "https://localhost:5006/GetCapping/" + lPostHeaderID; // Replace with your API URL

                            // Send an HTTP GET request
                            HttpResponseMessage response = await client.GetAsync(apiUrl);

                            // Check if the request was successful
                            if (response.IsSuccessStatusCode)
                            {
                                // Read the response content as a string

                                // ndsCAPCLinks = await response.Content.ReadAsStringAsync();
                                string jsonContent2 = response.Content.ReadAsStringAsync().Result;
                                //ndsCAPCLinks = JsonConvert.DeserializeObject<CapClink>(jsonContent2);
                                ndsCAPCLinks = JsonConvert.DeserializeObject<List<CapClink>>(jsonContent2);


                                // Now you can work with the response content (e.g., parse JSON, process data, etc.)
                                Console.WriteLine(ndsCAPCLinks);
                            }
                            else
                            {
                                Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                            }
                        }
                        catch (HttpRequestException ex)
                        {
                            Console.WriteLine($"HTTP request error: {ex.Message}");
                            //return BadRequest("Get Capping on WBS (" + pWBS1 + ", " + pWBS2 + ", " + pWBS3 + ") error: " + ex.Message)
                        }
                    }

                    //if (lErrorMsg.Trim().Length > 0)
                    //{
                    //    if (ndsClient.State == System.ServiceModel.CommunicationState.Opened)
                    //    {
                    //        ndsClient.Close();
                    //    }
                    //    //MsgBox("Get Capping on WBS (" + pWBS1 + ", " + pWBS2 + ", " + pWBS3 + ") error: " + lErrorMsg)
                    //    lReturn = -1;
                    //    return lReturn;
                    //}ajit

                    if (ndsCAPCLinks.Count > 0)
                    {
                        ndsCAPCLinkList = new List<CapClink>();
                        for (int i = 0; i < ndsCAPCLinks.Count; i++)
                        {
                            if (ndsCAPCLinks[i].Qty > 0)
                            {
                                ndsCAPCLinkList.Add(ndsCAPCLinks[i]);
                            }
                        }
                        ndsCAPCLinks = ndsCAPCLinkList;
                    }

                    if (ndsCAPCLinks.Count > 0)
                    {
                        lRet1 = 0;
                        lRet2 = 0;
                        //lSuccess = ndsClient.SaveCappingDetails(ndsCAPCLinks.ToArray(), lUserID, lWBSEleID, lStruEleID, lProdTypeID, lPostHeaderID, out lRet1, out lRet2, out lErrorMsg);
                        //if (lSuccess != true || lErrorMsg.Trim().Length > 0)
                        //{
                        //    if (ndsClient.State == System.ServiceModel.CommunicationState.Opened)
                        //    {
                        //        ndsClient.Close();
                        //    }
                        //    //MsgBox("Saving capping on WBS (" + pWBS1 + ", " + pWBS2 + ", " + pWBS3 + " error: " + lErrorMsg)
                        //    lReturn = -1;
                        //    return lReturn;
                        //}ajit

                        using (var client = new HttpClient())
                        {
                            try
                            {

                                string jsonData = JsonConvert.SerializeObject(ndsCAPCLinks);
                                StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                                // Define the API endpoint URL

                                string apiUrl = "https://localhost:5006/SaveCappingDetails";// Replace with your API URL
                                string apiUrlWithParameters = string.Format("{0}/{1}/{2}/{3}/{4}/{5}", apiUrl, lUserID, lWBSEleID, lStruEleID, lProdTypeID, lPostHeaderID);
                                // Send an HTTP GET request
                                HttpResponseMessage response = await client.PostAsync(apiUrlWithParameters, content);

                                // Check if the request was successful
                                if (response.IsSuccessStatusCode)
                                {
                                    // Read the response content as a string
                                    string responseContent = await response.Content.ReadAsStringAsync();

                                    // Now you can work with the response content (e.g., parse JSON, process data, etc.)
                                    Console.WriteLine(responseContent);
                                }
                                else
                                {
                                    Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                                }
                            }
                            catch (HttpRequestException ex)
                            {
                                Console.WriteLine($"HTTP request error: {ex.Message}");
                            }
                        }

                    }
                }
            }

            catch (Exception ex)
            {
                lReturn = -1;
                //SaveErrorMsg(ex.Message, ex.StackTrace);
            }


            myTransport = null;
            muEncoding = null;

            binding = null;
            address = null;
            //ndsClient = null;ajit

            return lReturn;
        }
        private static DataTable CreateDataTable(DataTable dt, IRfcTable rfcTable)
        {
            foreach (IRfcStructure row in rfcTable)
            {
                DataRow newRow = dt.NewRow();
                for (int element = 0; element < rfcTable.ElementCount; element++)
                {
                    RfcElementMetadata metadata = rfcTable.GetElementMetadata(element);
                    var nrow = newRow[element];
                    var rrow = row.GetString(metadata.Name);
                    newRow[element] = row.GetString(metadata.Name);

                }
                dt.Rows.Add(newRow);
            }

            return dt;

        }


        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("CancelSAPSO")]

        public int CancelSAPSO(CancelSAPSODto cancelSAPSODto)
        {
            string pCustomerCode = cancelSAPSODto.pCustomerCode;
            string pProjectCode = cancelSAPSODto.pProjectCode;
            int pJobID = cancelSAPSODto.pJobID;
            string pPONo = cancelSAPSODto.pPONo;
            string pBBSNo = cancelSAPSODto.pBBSNo;
            string pSAPSO = cancelSAPSODto.pSAPSO;
            //var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            //string connectionString = ConfigurationManager.ConnectionStrings["YourConnectionStringName"].ConnectionString;
            //var configuration = builder.Build();

            // Access a setting
            //string settingValue = ConfigurationManager.AppSettings["DEV"]; // You can also provide a default value in case the setting is not foundstring settingValue = ConfigurationManager.AppSettings["SettingName"] ?? "DefaultValue";
            //var sapDestinationConnectionString = configuration.GetConnectionString("SAPDestination");
            int lReturn = 0;

            if (pSAPSO.Length > 0)
            {
                var lConnect = new SapConnection("DEV");
                lConnect.Open();

                var lSAP = lConnect.Destination;

                try
                {
                    IRfcFunction setCommit = lSAP.Repository.CreateFunction("BAPI_TRANSACTION_COMMIT");

                    //get order item number
                    IRfcFunction getOrderAPI = lSAP.Repository.CreateFunction("BAPI_SALESORDER_GETSTATUS");
                    getOrderAPI.SetValue("SALESDOCUMENT", pSAPSO);
                    RfcSessionManager.BeginContext(lSAP);
                    getOrderAPI.Invoke(lSAP);
                    setCommit.Invoke(lSAP);
                    RfcSessionManager.EndContext(lSAP);

                    IRfcTable lItemsTable = getOrderAPI.GetTable("STATUSINFO");

                    IRfcFunction setOrderAPI = lSAP.Repository.CreateFunction("YSDBAPI_SALESORDER_CHANGE");

                    setOrderAPI.SetValue("SALESDOCUMENT", pSAPSO);

                    IRfcStructure SAPStrucOrder_Header_In = setOrderAPI.GetStructure("ORDER_HEADER_IN");

                    //Change PO NUmber
                    SAPStrucOrder_Header_In.SetValue("PURCH_NO_C", pPONo + "-CXL");

                    IRfcStructure salesHeaderINX = setOrderAPI.GetStructure("ORDER_HEADER_INX");
                    salesHeaderINX.SetValue("UPDATEFLAG", "U");
                    salesHeaderINX.SetValue("PURCH_NO_C", "X");

                    //IRfcTable SAPTableOrder_Ext_In = setOrderAPI.GetTable("EXTENSIONIN");

                    //SAPTableOrder_Ext_In.Append();
                    //SAPTableOrder_Ext_In.SetValue("STRUCTURE", "BAPE_VBAK");
                    //SAPTableOrder_Ext_In.SetValue("VALUEPART1",
                    //pSAPSO +                                                        //SO (10)
                    //"00000000000000000" +                                           //YTOT_GRAND (17)
                    //"00000000000000000" +                                           //YTOT_CAB (17)
                    //"00000000000000000" +                                           //YTOT_MESH (17)
                    //"00000000000000000");                                           //YTOT_REBAR (17)
                    ////"                 " +                                           //YTOT_BPC (17)
                    ////"                 " +                                           //YTOT_PRECAGE (17)
                    ////"                 " +                                           //YTOTAL_WR (17)
                    ////"                 " +                                           //YTOTAL_PCSTRAND (17)
                    ////"                " +                                            //YTOT_PCSTRVAL (16)
                    ////"                 " +                                           //YTOT_COLD_ROLL (17)
                    ////"                 " +                                           //YTOT_PRE_CUTWR (17)
                    ////"                " +                                            //YTOTAL_VALUE (16)
                    ////"      " +                                                      //YTOT_COUPLER (6)
                    ////"                " +                                            //YTOT_COUPVAL (16)
                    ////" " +
                    ////" " +
                    ////" " +
                    ////" " +
                    ////" " +
                    ////" ");

                    ////SAPTableOrder_Ext_In.SetValue("VALUEPART2",
                    ////    " " + //pVBAKExt.YPCSTRAND +
                    ////    " " + //pVBAKExt.YMAT_SOURCE +
                    ////    " " +                                       //YCAB_TYPE
                    ////    " " +                                       //YORD_TYPE
                    ////    "                                        " +    //YWBS1 (40)
                    ////    " " + //pVBAKExt.YCOLD_ROLL_WIRE +
                    ////    " " + //pVBAKExt.YPRE_CUT_WIRE +
                    ////    "                 " +                       //YTOT_BILLET(17)
                    ////    " " +                                       //YBILLET_IND
                    ////    " " +                                       //YCON_TYP
                    ////    "                 " +                       //YTOT_CAR(17)
                    ////    " " + //pVBAKExt.YCARPET_IND +
                    ////    " "); //pVBAKExt.YCSURCHARGE_IND);

                    //SAPTableOrder_Ext_In.Append();
                    //SAPTableOrder_Ext_In.SetValue("STRUCTURE", "BAPE_VBAKX");
                    //SAPTableOrder_Ext_In.SetValue("VALUEPART1", pSAPSO + "XXXX");
                    //    //" " +       //YTOT_BPC
                    //    //" " +       //YTOT_PRECAGE
                    //    //" " +       //YTOTAL_WR
                    //    //" " +       //YTOTAL_PCSTRAND
                    //    //" " +       //YTOT_PCSTRVAL
                    //    //" " +       //YTOT_COLD_ROLL
                    //    //" " +       //YTOT_PRE_CUTWR
                    //    //" " +       //YTOTAL_VALUE
                    //    //" " +       //YTOT_COUPLER
                    //    //" " +       //YTOT_COUPVAL
                    //    //" " +       //YWIREROD_IND
                    //    //" " +       //YREBAR_IND
                    //    //" " +       //YCAB_IND
                    //    //" " +       //YMESH_IND
                    //    //" " +       //YPRECAGE
                    //    //" " +       //YBPC_IND
                    //    //" " +       //YPCSTRAND
                    //    //" " +       //YMAT_SOURCE
                    //    //" " +       //YCAB_TYPE
                    //    //" " +       //YORD_TYPE
                    //    //" " +       //YWBS1
                    //    //" " +       //YCOLD_ROLL_WIRE
                    //    //" " +       //YPRE_CUT_WIRE
                    //    //" " +       //YTOT_BILLET
                    //    //" " +       //YBILLET_IND
                    //    //" " +       //YCON_TYP
                    //    //" " +       //YTOT_CAR
                    //    //" " +       //YCARPET_IND
                    //    //" "         //YCSURCHARGE_IND
                    //    //);


                    IRfcTable salesItems = setOrderAPI.GetTable("ORDER_ITEM_IN");
                    IRfcTable salesItemsINX = setOrderAPI.GetTable("ORDER_ITEM_INX");
                    IRfcTable lTextBBSNo = setOrderAPI.GetTable("ORDER_TEXT");

                    //IRfcTable Schedule_In = setOrderAPI.GetTable("SCHEDULE_LINES");
                    //IRfcTable Schedule_InX = setOrderAPI.GetTable("SCHEDULE_LINESX");

                    lReturn = lItemsTable.RowCount;

                    for (lItemsTable.CurrentIndex = 0; lItemsTable.CurrentIndex < lItemsTable.RowCount; ++(lItemsTable.CurrentIndex))
                    {
                        //Changed to Rejected order with reason 00 -- Assigned by the System (internal)
                        salesItems.Append();
                        salesItems.SetValue("ITM_NUMBER", lItemsTable.GetString("ITM_NUMBER"));
                        salesItems.SetValue("REASON_REJ", "00");

                        salesItemsINX.Append();
                        salesItemsINX.SetValue("UPDATEFLAG", "U");
                        salesItemsINX.SetValue("ITM_NUMBER", lItemsTable.GetString("ITM_NUMBER"));
                        salesItemsINX.SetValue("REASON_REJ", "X");

                        //Change BBS Number
                        lTextBBSNo.Append();
                        lTextBBSNo.SetValue("DOC_NUMBER", pSAPSO);
                        lTextBBSNo.SetValue("TEXT_ID", "Z106");
                        lTextBBSNo.SetValue("ITM_NUMBER", lItemsTable.GetString("ITM_NUMBER"));
                        lTextBBSNo.SetValue("LANGU", "EN");
                        lTextBBSNo.SetValue("TEXT_LINE", pBBSNo + "-CXL");

                        //Schedule_In.Append();
                        //Schedule_In.SetValue("ITM_NUMBER", lItemsTable.GetString("ITM_NUMBER"));
                        //Schedule_In.SetValue("SCHED_LINE", 1);
                        //Schedule_In.SetValue("REQ_QTY", 0);

                        //Schedule_InX.Append();
                        //Schedule_InX.SetValue("UPDATEFLAG", "U");
                        //Schedule_InX.SetValue("ITM_NUMBER", lItemsTable.GetString("ITM_NUMBER"));
                        //Schedule_InX.SetValue("SCHED_LINE", 1);
                        //Schedule_InX.SetValue("REQ_QTY", "X");

                        // exit if reach end of records as "for" statement error by increasing the currentIndex at end.
                        if (lItemsTable.CurrentIndex >= lItemsTable.RowCount - 1)
                        {
                            break;
                        }
                    }


                    RfcSessionManager.BeginContext(lSAP);
                    setOrderAPI.Invoke(lSAP);
                    setCommit.Invoke(lSAP);
                    RfcSessionManager.EndContext(lSAP);

                    String strReturnType;
                    String strReturnID;
                    String strReturnNumber;
                    String strReturnMessage;
                    String strReturnType2;
                    String strReturnID2;
                    String strReturnNumber2;
                    String strReturnMessage2;

                    IRfcTable SAPReturn = setOrderAPI.GetTable("RETURN");
                    if (SAPReturn.RowCount > 0)
                    {
                        strReturnType = SAPReturn[0].GetString("TYPE");
                        strReturnID = SAPReturn[0].GetString("ID");
                        strReturnNumber = SAPReturn[0].GetString("NUMBER");
                        strReturnMessage = SAPReturn[0].GetString("MESSAGE");
                    }
                    else
                    {
                        strReturnType = setOrderAPI.GetTable("RETURN").GetString("TYPE");
                        strReturnID = setOrderAPI.GetTable("RETURN").GetString("ID");
                        strReturnNumber = setOrderAPI.GetTable("RETURN").GetString("NUMBER");
                        strReturnMessage = setOrderAPI.GetTable("RETURN").GetString("MESSAGE");
                    }
                    strReturnType2 = setCommit.GetStructure("RETURN").GetString("TYPE");
                    strReturnID2 = setCommit.GetStructure("RETURN").GetString("ID");
                    strReturnNumber2 = setCommit.GetStructure("RETURN").GetString("NUMBER");
                    strReturnMessage2 = setCommit.GetStructure("RETURN").GetString("MESSAGE");

                    if (strReturnType == "E")
                    {
                        lReturn = -2;
                    }
                }

                catch (RfcCommunicationException e)
                {
                    lReturn = -1;
                }
                catch (RfcLogonException e)
                {
                    // user could not logon...
                    lReturn = -1;
                }
                catch (RfcAbapRuntimeException e)
                {
                    // serious problem on ABAP system side...
                    lReturn = -1;
                }
                catch (RfcAbapBaseException e)
                {
                    lReturn = -1;
                    // The function module returned an ABAP exception, an ABAP message
                    // or an ABAP class-based exception...
                }
                catch (Exception e)
                {
                    lReturn = -1;
                    // Other type of exception
                }
                finally
                {
                    lConnect.Dispose();
                    lSAP = null;
                    lConnect = null;
                }

                if (lReturn >= 0)
                {
                    System.Threading.Thread.Sleep(2000);
                }
            }
            return lReturn;
        }


        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("ChangeSAPSO")]
        public int ChangeSAPSO(string pCustomerCode, string pProjectCode, int pJobID,
    string pPONo, string pBBSNo, string pSAPSO, string pReqDate, string pInRemarks, string pExRemarks, string pInvRemarks,
    int ChReqDate, int ChPONumber, int ChBBSNo, int ChIntRemakrs, int ChExtRemakrs, int ChInvRemakrs)
        {
            int lReturn = 0;

            if (pSAPSO.Length > 0 && (ChPONumber == 1 || ChReqDate == 1 || ChBBSNo == 1 || ChIntRemakrs == 1 || ChExtRemakrs == 1 || ChInvRemakrs == 1))
            {
                var lConnect = new SapConnection("DEV");
                lConnect.Open();

                var lSAP = lConnect.Destination;

                try
                {
                    IRfcFunction setCommit = lSAP.Repository.CreateFunction("BAPI_TRANSACTION_COMMIT");

                    //get order item number
                    IRfcFunction getOrderAPI = lSAP.Repository.CreateFunction("BAPI_SALESORDER_GETSTATUS");
                    getOrderAPI.SetValue("SALESDOCUMENT", pSAPSO);
                    RfcSessionManager.BeginContext(lSAP);
                    getOrderAPI.Invoke(lSAP);
                    setCommit.Invoke(lSAP);
                    RfcSessionManager.EndContext(lSAP);

                    IRfcTable lItemsTable = getOrderAPI.GetTable("STATUSINFO");

                    IRfcFunction setOrderAPI = lSAP.Repository.CreateFunction("BAPI_SALESORDER_CHANGE");

                    setOrderAPI.SetValue("SALESDOCUMENT", pSAPSO);

                    IRfcStructure SAPStrucOrder_Header_In = setOrderAPI.GetStructure("ORDER_HEADER_IN");
                    IRfcStructure salesHeaderINX = setOrderAPI.GetStructure("ORDER_HEADER_INX");
                    salesHeaderINX.SetValue("UPDATEFLAG", "U");

                    //Change PO NUmber
                    if (ChPONumber == 1)
                    {
                        SAPStrucOrder_Header_In.SetValue("PURCH_NO_C", pPONo);
                        salesHeaderINX.SetValue("PURCH_NO_C", "X");
                    }

                    //Change Required date

                    if (ChReqDate == 1)
                    {
                        SAPStrucOrder_Header_In.SetValue("REQ_DATE_H", pReqDate);
                        //Change Required date
                        salesHeaderINX.SetValue("REQ_DATE_H", "X");
                    }

                    // Internal/External Remarks
                    if (ChIntRemakrs == 1 || ChExtRemakrs == 1 || ChInvRemakrs == 1)
                    {
                        IRfcTable lRemarks = setOrderAPI.GetTable("ORDER_TEXT");

                        if (ChIntRemakrs == 1)
                        {
                            lRemarks.Append();
                            lRemarks.SetValue("TEXT_ID", "Z010");
                            lRemarks.SetValue("LANGU", "EN");
                            lRemarks.SetValue("TEXT_LINE", pInRemarks);
                        }

                        if (ChExtRemakrs == 1)
                        {
                            lRemarks.Append();
                            lRemarks.SetValue("TEXT_ID", "Z011");
                            lRemarks.SetValue("LANGU", "EN");
                            lRemarks.SetValue("TEXT_LINE", pExRemarks);
                        }

                        if (ChInvRemakrs == 1)
                        {
                            lRemarks.Append();
                            lRemarks.SetValue("TEXT_ID", "Z013");
                            lRemarks.SetValue("LANGU", "EN");
                            lRemarks.SetValue("TEXT_LINE", pInvRemarks);
                        }
                    }

                    if (ChBBSNo == 1)
                    {
                        IRfcTable salesItems = setOrderAPI.GetTable("ORDER_ITEM_IN");
                        IRfcTable salesItemsINX = setOrderAPI.GetTable("ORDER_ITEM_INX");
                        IRfcTable lTextBBSNo = setOrderAPI.GetTable("ORDER_TEXT");

                        //IRfcTable Schedule_In = setOrderAPI.GetTable("SCHEDULE_LINES");
                        //IRfcTable Schedule_InX = setOrderAPI.GetTable("SCHEDULE_LINESX");

                        lReturn = lItemsTable.RowCount;

                        for (lItemsTable.CurrentIndex = 0; lItemsTable.CurrentIndex < lItemsTable.RowCount; ++(lItemsTable.CurrentIndex))
                        {
                            //Changed to Rejected order with reason 00 -- Assigned by the System (internal)
                            salesItems.Append();
                            salesItems.SetValue("ITM_NUMBER", lItemsTable.GetString("ITM_NUMBER"));

                            salesItemsINX.Append();
                            salesItemsINX.SetValue("UPDATEFLAG", "U");
                            salesItemsINX.SetValue("ITM_NUMBER", lItemsTable.GetString("ITM_NUMBER"));

                            //Change BBS Number
                            lTextBBSNo.Append();
                            lTextBBSNo.SetValue("DOC_NUMBER", pSAPSO);
                            lTextBBSNo.SetValue("TEXT_ID", "Z106");
                            lTextBBSNo.SetValue("ITM_NUMBER", lItemsTable.GetString("ITM_NUMBER"));
                            lTextBBSNo.SetValue("LANGU", "EN");
                            lTextBBSNo.SetValue("TEXT_LINE", pBBSNo);

                            //Schedule_In.Append();
                            //Schedule_In.SetValue("ITM_NUMBER", lItemsTable.GetString("ITM_NUMBER"));
                            //Schedule_In.SetValue("SCHED_LINE", 1);
                            //Schedule_In.SetValue("REQ_QTY", 0);

                            //Schedule_InX.Append();
                            //Schedule_InX.SetValue("UPDATEFLAG", "U");
                            //Schedule_InX.SetValue("ITM_NUMBER", lItemsTable.GetString("ITM_NUMBER"));
                            //Schedule_InX.SetValue("SCHED_LINE", 1);
                            //Schedule_InX.SetValue("REQ_QTY", "X");

                            // exit if reach end of records as "for" statement error by increasing the currentIndex at end.
                            if (lItemsTable.CurrentIndex >= lItemsTable.RowCount - 1)
                            {
                                break;
                            }
                        }
                    }

                    RfcSessionManager.BeginContext(lSAP);
                    setOrderAPI.Invoke(lSAP);
                    setCommit.Invoke(lSAP);
                    RfcSessionManager.EndContext(lSAP);

                    String strReturnType;
                    String strReturnID;
                    String strReturnNumber;
                    String strReturnMessage;
                    String strReturnType2;
                    String strReturnID2;
                    String strReturnNumber2;
                    String strReturnMessage2;

                    IRfcTable SAPReturn = setOrderAPI.GetTable("RETURN");
                    if (SAPReturn.RowCount > 0)
                    {
                        strReturnType = SAPReturn[0].GetString("TYPE");
                        strReturnID = SAPReturn[0].GetString("ID");
                        strReturnNumber = SAPReturn[0].GetString("NUMBER");
                        strReturnMessage = SAPReturn[0].GetString("MESSAGE");
                    }
                    else
                    {
                        strReturnType = setOrderAPI.GetTable("RETURN").GetString("TYPE");
                        strReturnID = setOrderAPI.GetTable("RETURN").GetString("ID");
                        strReturnNumber = setOrderAPI.GetTable("RETURN").GetString("NUMBER");
                        strReturnMessage = setOrderAPI.GetTable("RETURN").GetString("MESSAGE");
                    }
                    strReturnType2 = setCommit.GetStructure("RETURN").GetString("TYPE");
                    strReturnID2 = setCommit.GetStructure("RETURN").GetString("ID");
                    strReturnNumber2 = setCommit.GetStructure("RETURN").GetString("NUMBER");
                    strReturnMessage2 = setCommit.GetStructure("RETURN").GetString("MESSAGE");

                    if (strReturnType == "E")
                    {
                        lReturn = -2;
                    }
                }

                catch (RfcCommunicationException e)
                {
                    lReturn = -1;
                }
                catch (RfcLogonException e)
                {
                    // user could not logon...
                    lReturn = -1;
                }
                catch (RfcAbapRuntimeException e)
                {
                    // serious problem on ABAP system side...
                    lReturn = -1;
                }
                catch (RfcAbapBaseException e)
                {
                    lReturn = -1;
                    // The function module returned an ABAP exception, an ABAP message
                    // or an ABAP class-based exception...
                }
                catch (Exception e)
                {
                    lReturn = -1;
                    // Other type of exception
                }
                finally
                {
                    lConnect.Dispose();
                    lSAP = null;
                    lConnect = null;
                }
            }
            return lReturn;
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("CreateStdSheetSAPSO")]
        public string CreateStdSheetSAPSO(CreateStdSheetSAPSO createStdSheetSAPSO)
        {
            string lReturn = "";
            string lSAPMaterial = "";
            int lPieces = 0;

            string lCurrency = "";
            string lSQL = "";

            OracleCommand cmdOrdHdr = new OracleCommand();
            OracleDataReader lOrdRst;
            OracleConnection cnCIS = new OracleConnection();

            ProcessController lProcess = new ProcessController();


            lProcess.OpenCISConnection(ref cnCIS);

            lSQL = "SELECT WAERK " +
            "FROM SAPSR3.VBAK " +
            "WHERE MANDT = '" + lProcess.strClient + "' " +
            "AND VBELN = '" + createStdSheetSAPSO.pContractNo + "' ";

            cmdOrdHdr.CommandText = lSQL;
            cmdOrdHdr.Connection = cnCIS;
            cmdOrdHdr.CommandTimeout = 300;
            lOrdRst = cmdOrdHdr.ExecuteReader();
            if (lOrdRst.HasRows)
            {
                lOrdRst.Read();
                lCurrency = lOrdRst.GetString(0) == null ? "" : lOrdRst.GetString(0);
            }
            lOrdRst.Close();
            lProcess.CloseCISConnection(ref cnCIS);

            lCurrency = lCurrency.Trim();

            List<string> lSAPMaterialNo = new List<string>();
            List<int> lSAPPcs = new List<int>();

            //convert order to SAP material
            if (createStdSheetSAPSO.pBBSDetails.Count > 0)
            {
                for (var i = 0; i < createStdSheetSAPSO.pBBSDetails.Count; i++)
                {
                    lSAPMaterial = createStdSheetSAPSO.pBBSDetails[i].sap_mcode;
                    lPieces = createStdSheetSAPSO.pBBSDetails[i].order_pcs;
                    if (lSAPMaterial.Trim().Length > 0 && lPieces > 0)
                    {
                        lSAPMaterialNo.Add(lSAPMaterial);
                        lSAPPcs.Add(lPieces);
                    }
                }
            }
            

            if (lSAPMaterialNo.Count > 0)
            {
                // get Contract Infor.

                try
                {

                    var lConnect = new SapConnection("DEV");
                    lConnect.Open();

                    var lSAP = lConnect.Destination;
                    IRfcFunction setOrderAPI = lSAP.Repository.CreateFunction("BAPI_SALESORDER_CREATEFROMDAT2");
                    IRfcFunction setCommit = lSAP.Repository.CreateFunction("BAPI_TRANSACTION_COMMIT");

                    IRfcStructure SAPStrucOrder_Header_In = setOrderAPI.GetStructure("ORDER_HEADER_IN");

                    var lOrderType = lProcess.getSAPOrderType(createStdSheetSAPSO.pProjectCode, createStdSheetSAPSO.pContractNo, createStdSheetSAPSO.pPaymentType, "MTS");
                    SAPStrucOrder_Header_In.SetValue("DOC_TYPE", lOrderType);

                    var lSalesOrg = lProcess.getSAPSalesOrg(createStdSheetSAPSO.pProjectCode, createStdSheetSAPSO.pContractNo);
                    SAPStrucOrder_Header_In.SetValue("SALES_ORG", lSalesOrg);

                    SAPStrucOrder_Header_In.SetValue("DISTR_CHAN", "10");
                    SAPStrucOrder_Header_In.SetValue("DIVISION", "00");
                    SAPStrucOrder_Header_In.SetValue("REFDOC_CAT", "G");

                    SAPStrucOrder_Header_In.SetValue("REQ_DATE_H", createStdSheetSAPSO.pReqDate);

                    //if (pContractNo != "" && pPaymentType == "CREDIT")
                    if (createStdSheetSAPSO.pContractNo != "")
                    {
                        if (createStdSheetSAPSO.pPriceDate != "" && createStdSheetSAPSO.pPriceDate != "000000")
                        {
                            SAPStrucOrder_Header_In.SetValue("PRICE_DATE", createStdSheetSAPSO.pPriceDate);
                        }

                        if (createStdSheetSAPSO.pPaymentTerm != null && createStdSheetSAPSO.pPaymentTerm.Trim() != "")
                        {
                            SAPStrucOrder_Header_In.SetValue("PMNTTRMS", createStdSheetSAPSO.pPaymentTerm);
                        }

                        if (createStdSheetSAPSO.pPriceGroup != null && createStdSheetSAPSO.pPriceGroup.Trim() != "")
                        {
                            SAPStrucOrder_Header_In.SetValue("PRICE_GRP", createStdSheetSAPSO.pPriceGroup);
                        }

                        if (lCurrency != "")
                        {
                            SAPStrucOrder_Header_In.SetValue("CURRENCY", lCurrency);
                        }

                        if (createStdSheetSAPSO.pIncoTerms1 != null && createStdSheetSAPSO.pIncoTerms1.Trim() != "")
                        {
                            SAPStrucOrder_Header_In.SetValue("INCOTERMS1", createStdSheetSAPSO.pIncoTerms1);

                            if (createStdSheetSAPSO.pIncoTerms2 != null && createStdSheetSAPSO.pIncoTerms2.Trim() != "")
                            {
                                SAPStrucOrder_Header_In.SetValue("INCOTERMS2", createStdSheetSAPSO.pIncoTerms2);
                            }
                        }
                    }

                    //PO Number, PO Date
                    SAPStrucOrder_Header_In.SetValue("PURCH_NO_C", createStdSheetSAPSO.pPONo);
                    SAPStrucOrder_Header_In.SetValue("PURCH_DATE", createStdSheetSAPSO.pPODate);

                    //Contract Number
                    if (createStdSheetSAPSO.pContractNo != "")
                    {
                        SAPStrucOrder_Header_In.SetValue("REF_DOC", createStdSheetSAPSO.pContractNo);
                    }

                    //Order Source
                    string lOrderSource = "CUS-UX";
                    if (createStdSheetSAPSO.pUserID == null || createStdSheetSAPSO.pUserID.IndexOf("@") < 0 || createStdSheetSAPSO.pUserID.ToLower().IndexOf("@natsteel.com.sg") > 0)
                    {
                        lOrderSource = "NSH-UX";
                    }
                    SAPStrucOrder_Header_In.SetValue("NAME", lOrderSource);

                    // Internal/External Remarks
                    IRfcTable lRemarks = setOrderAPI.GetTable("ORDER_TEXT");

                    lRemarks.Append();
                    lRemarks.SetValue("TEXT_ID", "Z010");
                    lRemarks.SetValue("LANGU", "EN");
                    lRemarks.SetValue("TEXT_LINE", createStdSheetSAPSO.pInRemarks);

                    lRemarks.Append();
                    lRemarks.SetValue("TEXT_ID", "Z011");
                    lRemarks.SetValue("LANGU", "EN");
                    lRemarks.SetValue("TEXT_LINE", createStdSheetSAPSO.pExRemarks);

                    if (createStdSheetSAPSO.pContractNo == "")
                    {
                        lRemarks.Append();
                        lRemarks.SetValue("TEXT_ID", "Z013");
                        lRemarks.SetValue("LANGU", "EN");
                        lRemarks.SetValue("TEXT_LINE", createStdSheetSAPSO.pInvRemarks);
                    }

                    IRfcTable SAPTableOrder_Partners = setOrderAPI.GetTable("ORDER_PARTNERS");

                    SAPTableOrder_Partners.Append();
                    //Customer Code
                    SAPTableOrder_Partners.SetValue("PARTN_ROLE", "AG");
                    SAPTableOrder_Partners.SetValue("PARTN_NUMB", createStdSheetSAPSO.pCustomerCode);
                    SAPTableOrder_Partners.Append();
                    //Project code
                    SAPTableOrder_Partners.SetValue("PARTN_ROLE", "WE");
                    SAPTableOrder_Partners.SetValue("PARTN_NUMB", createStdSheetSAPSO.pProjectCode);


                    IRfcTable SAPTableOrder_Items_In = setOrderAPI.GetTable("ORDER_ITEMS_IN");
                    IRfcTable SAPTableOrder_Schedule_In = setOrderAPI.GetTable("ORDER_SCHEDULES_IN");

                    int lngTmpPosNr = 0;

                    for (int i = 0; i < lSAPMaterialNo.Count; i++)
                    {
                        lngTmpPosNr = (i + 1) * 10;
                        SAPTableOrder_Items_In.Append();
                        SAPTableOrder_Items_In.SetValue("ITM_NUMBER", lngTmpPosNr);
                        SAPTableOrder_Items_In.SetValue("MATERIAL", lSAPMaterialNo[i]);
                        if (createStdSheetSAPSO.pContractNo != "")
                        {
                            SAPTableOrder_Items_In.SetValue("REF_DOC", createStdSheetSAPSO.pContractNo);
                            SAPTableOrder_Items_In.SetValue("REF_DOC_IT", "000000");

                            SAPTableOrder_Items_In.SetValue("REF_DOC_CA", "G");
                        }


                        SAPTableOrder_Schedule_In.Append();
                        SAPTableOrder_Schedule_In.SetValue("ITM_NUMBER", lngTmpPosNr);
                        SAPTableOrder_Schedule_In.SetValue("SCHED_LINE", 1);
                        SAPTableOrder_Schedule_In.SetValue("REQ_QTY", lSAPPcs[i]);
                        SAPTableOrder_Schedule_In.SetValue("REQ_DATE", createStdSheetSAPSO.pReqDate);
                    }

                    IRfcTable SAPTableOrder_Ext_In = setOrderAPI.GetTable("EXTENSIONIN");

                    SAPTableOrder_Ext_In.Append();
                    SAPTableOrder_Ext_In.SetValue("STRUCTURE", "BAPE_VBAK");
                    if (createStdSheetSAPSO.pContractNo != "")
                    {
                        SAPTableOrder_Ext_In.SetValue("VALUEPART1",
                            "          " +                                                  //SO (10)
                            (createStdSheetSAPSO.pVBAKExt.YTOT_GRAND / 1000).ToString("0000000000000.000") +    //YTOT_GRAND (17)
                            "                 " +                                           //YTOT_CAB (17)
                            (createStdSheetSAPSO.pVBAKExt.YTOT_MESH / 1000).ToString("0000000000000.000") +     //YTOT_MESH (17)
                            "                 " +                                           //YTOT_REBAR (17)
                            "                 " +                                           //YTOT_BPC (17)
                            "                 " +                                           //YTOT_PRECAGE (17)
                            "                 " +                                           //YTOTAL_WR (17)
                            "                 " +                                           //YTOTAL_PCSTRAND (17)
                            "                " +                                            //YTOT_PCSTRVAL (16)
                            "                 " +                                           //YTOT_COLD_ROLL (17)
                            "                 " +                                           //YTOT_PRE_CUTWR (17)
                            "                " +                                            //YTOTAL_VALUE (16)
                            (createStdSheetSAPSO.pVBAKExt.YTOT_COUPLER).ToString("000000") +                    //YTOT_COUPLER (6)
                            "                " +                                            //YTOT_COUPVAL (16)
                            createStdSheetSAPSO.pVBAKExt.YWIREROD_IND +
                            createStdSheetSAPSO.pVBAKExt.YREBAR_IND +
                            createStdSheetSAPSO.pVBAKExt.YCAB_IND +
                            createStdSheetSAPSO.pVBAKExt.YMESH_IND +
                            createStdSheetSAPSO.pVBAKExt.YPRECAGE +
                            createStdSheetSAPSO.pVBAKExt.YBPC_IND);
                    }
                    else
                    {
                        SAPTableOrder_Ext_In.SetValue("VALUEPART1",
                            "          " +                                                  //SO (10)
                            "                 " +                                           //YTOT_GRAND (17)
                            "                 " +                                           //YTOT_CAB (17)
                            "                 " +                                           //YTOT_MESH (17)
                            "                 " +                                           //YTOT_REBAR (17)
                            "                 " +                                           //YTOT_BPC (17)
                            "                 " +                                           //YTOT_PRECAGE (17)
                            "                 " +                                           //YTOTAL_WR (17)
                            "                 " +                                           //YTOTAL_PCSTRAND (17)
                            "                " +                                            //YTOT_PCSTRVAL (16)
                            "                 " +                                           //YTOT_COLD_ROLL (17)
                            "                 " +                                           //YTOT_PRE_CUTWR (17)
                            "                " +                                            //YTOTAL_VALUE (16)
                            "      " +                                                      //YTOT_COUPLER (6)
                            "                " +                                            //YTOT_COUPVAL (16)
                            createStdSheetSAPSO.pVBAKExt.YWIREROD_IND +
                            createStdSheetSAPSO.pVBAKExt.YREBAR_IND +
                            createStdSheetSAPSO.pVBAKExt.YCAB_IND +
                            createStdSheetSAPSO.pVBAKExt.YMESH_IND +
                            createStdSheetSAPSO.pVBAKExt.YPRECAGE +
                            createStdSheetSAPSO.pVBAKExt.YBPC_IND);
                    }

                    SAPTableOrder_Ext_In.SetValue("VALUEPART2",
                        createStdSheetSAPSO.pVBAKExt.YPCSTRAND +
                        createStdSheetSAPSO.pVBAKExt.YMAT_SOURCE +
                        " " +                                       //YCAB_TYPE
                        " " +                                       //YORD_TYPE
                        "                                        " +    //YWBS1 (40)
                        createStdSheetSAPSO.pVBAKExt.YCOLD_ROLL_WIRE +
                        createStdSheetSAPSO.pVBAKExt.YPRE_CUT_WIRE +
                        "                 " +                       //YTOT_BILLET(17)
                        " " +                                       //YBILLET_IND
                        " " +                                       //YCON_TYP
                        "                 " +                       //YTOT_CAR(17)
                        createStdSheetSAPSO.pVBAKExt.YCARPET_IND +
                        createStdSheetSAPSO.pVBAKExt.YCSURCHARGE_IND); 


                    SAPTableOrder_Ext_In.Append();
                    SAPTableOrder_Ext_In.SetValue("STRUCTURE", "BAPE_VBAKX");
                    SAPTableOrder_Ext_In.SetValue("VALUEPART1", "          X X " +
                        " " +       //YTOT_BPC
                        " " +       //YTOT_PRECAGE
                        " " +       //YTOTAL_WR
                        " " +       //YTOTAL_PCSTRAND
                        " " +       //YTOT_PCSTRVAL
                        " " +       //YTOT_COLD_ROLL
                        " " +       //YTOT_PRE_CUTWR
                        " " +       //YTOTAL_VALUE
                        "X" +       //YTOT_COUPLER
                        " " +       //YTOT_COUPVAL
                        "X" +       //YWIREROD_IND
                        "X" +       //YREBAR_IND
                        "X" +       //YCAB_IND
                        "X" +       //YMESH_IND
                        "X" +       //YPRECAGE
                        "X" +       //YBPC_IND
                        "X" +       //YPCSTRAND
                        "X" +       //YMAT_SOURCE
                        " " +       //YCAB_TYPE
                        " " +       //YORD_TYPE
                        " " +       //YWBS1
                        "X" +       //YCOLD_ROLL_WIRE
                        "X" +       //YPRE_CUT_WIRE
                        " " +       //YTOT_BILLET
                        " " +       //YBILLET_IND
                        " " +       //YCON_TYP
                        " " +       //YTOT_CAR
                        "X" +       //YCARPET_IND
                        "X"         //YCSURCHARGE_IND
                        );

                    RfcSessionManager.BeginContext(lSAP);
                    setOrderAPI.Invoke(lSAP);
                    setCommit.Invoke(lSAP);
                    RfcSessionManager.EndContext(lSAP);

                    String strVBELN;
                    String strReturnType;
                    String strReturnID;
                    String strReturnNumber;
                    String strReturnMessage;
                    String strReturnType2;
                    String strReturnID2;
                    String strReturnNumber2;
                    String strReturnMessage2;

                    IRfcTable SAPReturn = setOrderAPI.GetTable("RETURN");
                    strVBELN = setOrderAPI.GetValue("SALESDOCUMENT").ToString();

                    strReturnMessage = "";
                    if (strVBELN.Length <= 8)
                    {
                        if (SAPReturn.RowCount > 0)
                        {
                            for (int i = 0; i < SAPReturn.RowCount; i++)
                            {
                                strReturnType = SAPReturn[i].GetString("TYPE");
                                strReturnID = SAPReturn[i].GetString("ID");
                                strReturnNumber = SAPReturn[i].GetString("NUMBER");
                                if (SAPReturn[i].GetString("MESSAGE") != null && SAPReturn[i].GetString("MESSAGE").ToUpper().IndexOf("SUCCESS") < 0)
                                {
                                    if (strReturnMessage == "")
                                    {
                                        strReturnMessage = SAPReturn[i].GetString("MESSAGE");
                                    }
                                    else
                                    {
                                        strReturnMessage = strReturnMessage + "\n" + SAPReturn[i].GetString("MESSAGE");
                                    }
                                }
                            }
                        }
                        else
                        {
                            strReturnType = setOrderAPI.GetTable("RETURN").GetString("TYPE");
                            strReturnID = setOrderAPI.GetTable("RETURN").GetString("ID");
                            strReturnNumber = setOrderAPI.GetTable("RETURN").GetString("NUMBER");
                            strReturnMessage = setOrderAPI.GetTable("RETURN").GetString("MESSAGE");
                        }
                    }

                    strReturnType2 = setCommit.GetStructure("RETURN").GetString("TYPE");
                    strReturnID2 = setCommit.GetStructure("RETURN").GetString("ID");
                    strReturnNumber2 = setCommit.GetStructure("RETURN").GetString("NUMBER");
                    strReturnMessage2 = setCommit.GetStructure("RETURN").GetString("MESSAGE");

                    if (strVBELN.Length > 8)
                    {
                        lReturn = strVBELN;
                    }
                    else
                    {
                        lReturn = strReturnMessage;
                    }

                    lConnect.Dispose();
                    lSAP = null;
                    lConnect = null;
                }

                catch (RfcCommunicationException e)
                {
                    lReturn = e.Message;
                }
                catch (RfcLogonException e)
                {
                    // user could not logon...
                    lReturn = e.Message;
                }
                catch (RfcAbapRuntimeException e)
                {
                    // serious problem on ABAP system side...
                    lReturn = e.Message;
                }
                catch (RfcAbapBaseException e)
                {
                    lReturn = e.Message;
                    // The function module returned an ABAP exception, an ABAP message
                    // or an ABAP class-based exception...
                }
                catch (Exception e)
                {
                    lReturn = e.Message;
                    // Other type of exception
                }
            }

            lProcess = null;

            return lReturn;
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("CreateStdProdSAPSO")]
        public string CreateStdProdSAPSO(CreateStdProdSAPSODto createStdProdSAPSODto)
        {
            string lReturn = "";
            List<string> lSAPCode;
            string lBarType = "";
            short lBarSize = 0;
            int lBarLength = 0;

            string lCurrency = "";
            string lSQL = "";

            OracleCommand cmdOrdHdr = new OracleCommand();
            OracleDataReader lOrdRst;
            OracleConnection cnCIS = new OracleConnection();

            ProcessController lProcess = new ProcessController();

            lProcess.OpenCISConnection(ref cnCIS);

            lSQL = "SELECT WAERK " +
            "FROM SAPSR3.VBAK " +
            "WHERE MANDT = '" + lProcess.strClient + "' " +
            "AND VBELN = '" + createStdProdSAPSODto.pContractNo + "' ";

            cmdOrdHdr.CommandText = lSQL;
            cmdOrdHdr.Connection = cnCIS;
            cmdOrdHdr.CommandTimeout = 300;
            lOrdRst = cmdOrdHdr.ExecuteReader();
            if (lOrdRst.HasRows)
            {
                lOrdRst.Read();
                lCurrency = lOrdRst.GetString(0) == null ? "" : lOrdRst.GetString(0);
            }
            lOrdRst.Close();
            lProcess.CloseCISConnection(ref cnCIS);

            lCurrency = lCurrency.Trim();

            List<string> lSAPMaterialNo = new List<string>();
            List<int> lSAPkg = new List<int>();

            if (createStdProdSAPSODto.pBBSDetails.Count > 0)
            {
                for (var i = 0; i < createStdProdSAPSODto.pBBSDetails.Count; i++)
                {
                    if (createStdProdSAPSODto.pBBSDetails[i].ProdCode != null && createStdProdSAPSODto.pBBSDetails[i].ProdCode != "" && (createStdProdSAPSODto.pBBSDetails[i].order_wt > 0 || createStdProdSAPSODto.pBBSDetails[i].order_pcs > 0))
                    {
                        if (createStdProdSAPSODto.pBBSDetails[i].ProdCode.Substring(0, 3) == "FUN" || createStdProdSAPSODto.pBBSDetails[i].ProdCode.Substring(0, 3) == "FUE")
                        {
                            lSAPMaterialNo.Add(createStdProdSAPSODto.pBBSDetails[i].ProdCode.Trim());
                            lSAPkg.Add((int)createStdProdSAPSODto.pBBSDetails[i].order_pcs);
                        }
                        else
                        {
                            lSAPMaterialNo.Add(createStdProdSAPSODto.pBBSDetails[i].ProdCode.Trim());
                            lSAPkg.Add((int)createStdProdSAPSODto.pBBSDetails[i].order_wt);
                        }
                    }
                }
            }

            if (lSAPMaterialNo.Count > 0)
            {
                // get Contract Infor.

                try
                {
                    var lConnect = new SapConnection("DEV");
                    lConnect.Open();

                    var lSAP = lConnect.Destination;
                    IRfcFunction setOrderAPI = lSAP.Repository.CreateFunction("BAPI_SALESORDER_CREATEFROMDAT2");
                    IRfcFunction setCommit = lSAP.Repository.CreateFunction("BAPI_TRANSACTION_COMMIT");

                    IRfcStructure SAPStrucOrder_Header_In = setOrderAPI.GetStructure("ORDER_HEADER_IN");

                    var lOrderType = lProcess.getSAPOrderType(createStdProdSAPSODto.pProjectCode, createStdProdSAPSODto.pContractNo, createStdProdSAPSODto.pPaymentType, "MTS");
                    SAPStrucOrder_Header_In.SetValue("DOC_TYPE", lOrderType);

                    var lSalesOrg = lProcess.getSAPSalesOrg(createStdProdSAPSODto.pProjectCode, createStdProdSAPSODto.pContractNo);
                    SAPStrucOrder_Header_In.SetValue("SALES_ORG", lSalesOrg);

                    SAPStrucOrder_Header_In.SetValue("DISTR_CHAN", "10");
                    SAPStrucOrder_Header_In.SetValue("DIVISION", "00");
                    SAPStrucOrder_Header_In.SetValue("REFDOC_CAT", "G");

                    SAPStrucOrder_Header_In.SetValue("REQ_DATE_H", createStdProdSAPSODto.pReqDate);

                    if (createStdProdSAPSODto.pContractNo != "" && createStdProdSAPSODto.pPaymentType == "CREDIT")
                    {
                        SAPStrucOrder_Header_In.SetValue("PRICE_DATE", createStdProdSAPSODto.pPriceDate);

                        if (createStdProdSAPSODto.pPaymentTerm != null && createStdProdSAPSODto.pPaymentTerm.Trim() != "")
                        {
                            SAPStrucOrder_Header_In.SetValue("PMNTTRMS", createStdProdSAPSODto.pPaymentTerm);
                        }

                        if (createStdProdSAPSODto.pPriceGroup != null && createStdProdSAPSODto.pPriceGroup.Trim() != "")
                        {
                            SAPStrucOrder_Header_In.SetValue("PRICE_GRP", createStdProdSAPSODto.pPriceGroup);
                        }

                        if (lCurrency != "")
                        {
                            SAPStrucOrder_Header_In.SetValue("CURRENCY", lCurrency);
                        }

                        if (createStdProdSAPSODto.pIncoTerms1 != null && createStdProdSAPSODto.pIncoTerms1.Trim() != "")
                        {
                            SAPStrucOrder_Header_In.SetValue("INCOTERMS1", createStdProdSAPSODto.pIncoTerms1);

                            if (createStdProdSAPSODto.pIncoTerms2 != null && createStdProdSAPSODto.pIncoTerms2.Trim() != "")
                            {
                                SAPStrucOrder_Header_In.SetValue("INCOTERMS2", createStdProdSAPSODto.pIncoTerms2);
                            }
                        }
                    }

                    //PO Number, PO Date
                    SAPStrucOrder_Header_In.SetValue("PURCH_NO_C", createStdProdSAPSODto.pPONo);
                    SAPStrucOrder_Header_In.SetValue("PURCH_DATE", createStdProdSAPSODto.pPODate);

                    //Contract Number
                    if (createStdProdSAPSODto.pContractNo != "")
                    {
                        SAPStrucOrder_Header_In.SetValue("REF_DOC", createStdProdSAPSODto.pContractNo);
                    }

                    //Order Source
                    string lOrderSource = "CUS-UX";
                    if (createStdProdSAPSODto.pUserID == null || createStdProdSAPSODto.pUserID.IndexOf("@") < 0 || createStdProdSAPSODto.pUserID.ToLower().IndexOf("@natsteel.com.sg") > 0)
                    {
                        lOrderSource = "NSH-UX";
                    }
                    SAPStrucOrder_Header_In.SetValue("NAME", lOrderSource);

                    // Internal/External Remarks
                    IRfcTable lRemarks = setOrderAPI.GetTable("ORDER_TEXT");

                    lRemarks.Append();
                    lRemarks.SetValue("TEXT_ID", "Z010");
                    lRemarks.SetValue("LANGU", "EN");
                    lRemarks.SetValue("TEXT_LINE", createStdProdSAPSODto.pInRemarks);

                    lRemarks.Append();
                    lRemarks.SetValue("TEXT_ID", "Z011");
                    lRemarks.SetValue("LANGU", "EN");
                    lRemarks.SetValue("TEXT_LINE", createStdProdSAPSODto.pExRemarks);

                    if (createStdProdSAPSODto.pContractNo == "")
                    {
                        lRemarks.Append();
                        lRemarks.SetValue("TEXT_ID", "Z013");
                        lRemarks.SetValue("LANGU", "EN");
                        lRemarks.SetValue("TEXT_LINE", createStdProdSAPSODto.pInvRemarks);
                    }

                    IRfcTable SAPTableOrder_Partners = setOrderAPI.GetTable("ORDER_PARTNERS");

                    SAPTableOrder_Partners.Append();
                    //Customer Code
                    SAPTableOrder_Partners.SetValue("PARTN_ROLE", "AG");
                    SAPTableOrder_Partners.SetValue("PARTN_NUMB", createStdProdSAPSODto.pCustomerCode);
                    SAPTableOrder_Partners.Append();
                    //Project code
                    SAPTableOrder_Partners.SetValue("PARTN_ROLE", "WE");
                    SAPTableOrder_Partners.SetValue("PARTN_NUMB", createStdProdSAPSODto.pProjectCode);


                    IRfcTable SAPTableOrder_Items_In = setOrderAPI.GetTable("ORDER_ITEMS_IN");
                    IRfcTable SAPTableOrder_Schedule_In = setOrderAPI.GetTable("ORDER_SCHEDULES_IN");

                    int lngTmpPosNr = 0;

                    for (int i = 0; i < lSAPMaterialNo.Count; i++)
                    {
                        lngTmpPosNr = (i + 1) * 10;
                        SAPTableOrder_Items_In.Append();
                        SAPTableOrder_Items_In.SetValue("ITM_NUMBER", lngTmpPosNr);
                        SAPTableOrder_Items_In.SetValue("MATERIAL", lSAPMaterialNo[i]);
                        if (createStdProdSAPSODto.pContractNo != "")
                        {
                            SAPTableOrder_Items_In.SetValue("REF_DOC", createStdProdSAPSODto.pContractNo);
                            SAPTableOrder_Items_In.SetValue("REF_DOC_IT", "000000");

                            SAPTableOrder_Items_In.SetValue("REF_DOC_CA", "G");
                        }

                        //BBS No, BBS Description
                        //SAPTableOrder_Items_In.SetValue("YBBS_NO", pBBSNo);
                        //SAPTableOrder_Items_In.SetValue("YBBS_DESCR", pBBSDesc);

                        lRemarks.Append();
                        lRemarks.SetValue("TEXT_ID", "Z106");
                        lRemarks.SetValue("ITM_NUMBER", lngTmpPosNr);
                        lRemarks.SetValue("LANGU", "EN");
                        lRemarks.SetValue("TEXT_LINE", createStdProdSAPSODto.pBBSNo);

                        lRemarks.Append();
                        lRemarks.SetValue("TEXT_ID", "Z107");
                        lRemarks.SetValue("ITM_NUMBER", lngTmpPosNr);
                        lRemarks.SetValue("LANGU", "EN");
                        lRemarks.SetValue("TEXT_LINE", createStdProdSAPSODto.pBBSDesc);

                        SAPTableOrder_Schedule_In.Append();
                        SAPTableOrder_Schedule_In.SetValue("ITM_NUMBER", lngTmpPosNr);
                        SAPTableOrder_Schedule_In.SetValue("SCHED_LINE", 1);
                        if (lSAPMaterialNo[i].Substring(0, 3) == "FUN" || lSAPMaterialNo[i].Substring(0, 3) == "FUE")
                        {
                            SAPTableOrder_Schedule_In.SetValue("REQ_QTY", lSAPkg[i]);
                        }
                        else
                        {
                            SAPTableOrder_Schedule_In.SetValue("REQ_QTY", (double)Math.Round((double)lSAPkg[i] / 1000, 3));
                        }
                        SAPTableOrder_Schedule_In.SetValue("REQ_DATE", createStdProdSAPSODto.pReqDate);
                    }

                    IRfcTable SAPTableOrder_Ext_In = setOrderAPI.GetTable("EXTENSIONIN");

                    SAPTableOrder_Ext_In.Append();
                    SAPTableOrder_Ext_In.SetValue("STRUCTURE", "BAPE_VBAK");
                    SAPTableOrder_Ext_In.SetValue("VALUEPART1",
                        "          " +                                                  //SO (10)
                        (createStdProdSAPSODto.pVBAKExt.YTOT_GRAND / 1000).ToString("0000000000000.000") +    //YTOT_GRAND (17)
                        "                 " +                                           //YTOT_CAB (17)
                        "                 " +                                           //YTOT_MESH (17)
                        (createStdProdSAPSODto.pVBAKExt.YTOT_REBAR / 1000).ToString("0000000000000.000") +    //YTOT_REBAR (17)
                        "                 " +                                           //YTOT_BPC (17)
                        "                 " +                                           //YTOT_PRECAGE (17)
                        (createStdProdSAPSODto.pVBAKExt.YTOT_WIROD / 1000).ToString("0000000000000.000") +    //YTOTAL_WR (17)
                        (createStdProdSAPSODto.pVBAKExt.YTOT_PCSTR / 1000).ToString("0000000000000.000") +    //YTOTAL_PCSTRAND (17)
                        "                " +                                            //YTOT_PCSTRVAL (16)
                        (createStdProdSAPSODto.pVBAKExt.YTOT_CRWIC / 1000).ToString("0000000000000.000") +    //YTOT_COLD_ROLL (17)
                        "                 " +                                           //YTOT_PRE_CUTWR (17)
                        "                " +                                            //YTOTAL_VALUE (16)
                        (createStdProdSAPSODto.pVBAKExt.YTOT_COUPLER).ToString("000000") +                    //YTOT_COUPLER (6)
                        "                " +                                            //YTOT_COUPVAL (16)
                        createStdProdSAPSODto.pVBAKExt.YWIREROD_IND +
                        createStdProdSAPSODto.pVBAKExt.YREBAR_IND +
                        createStdProdSAPSODto.pVBAKExt.YCAB_IND +
                        createStdProdSAPSODto.pVBAKExt.YMESH_IND +
                        createStdProdSAPSODto.pVBAKExt.YPRECAGE +
                        createStdProdSAPSODto.pVBAKExt.YBPC_IND);

                    SAPTableOrder_Ext_In.SetValue("VALUEPART2",
                        createStdProdSAPSODto.pVBAKExt.YPCSTRAND +
                        createStdProdSAPSODto.pVBAKExt.YMAT_SOURCE +
                        " " +                                       //YCAB_TYPE
                        " " +                                       //YORD_TYPE
                        "                                        " +    //YWBS1 (40)
                        createStdProdSAPSODto.pVBAKExt.YCOLD_ROLL_WIRE +
                        createStdProdSAPSODto.pVBAKExt.YPRE_CUT_WIRE +
                        "                 " +                       //YTOT_BILLET(17)
                        " " +                                       //YBILLET_IND
                        " " +                                       //YCON_TYP
                        "                 " +                       //YTOT_CAR(17)
                        createStdProdSAPSODto.pVBAKExt.YCARPET_IND +
                        createStdProdSAPSODto.pVBAKExt.YCSURCHARGE_IND);


                    SAPTableOrder_Ext_In.Append();
                    SAPTableOrder_Ext_In.SetValue("STRUCTURE", "BAPE_VBAKX");
                    SAPTableOrder_Ext_In.SetValue("VALUEPART1", "          X  X" +
                        " " +       //YTOT_BPC
                        " " +       //YTOT_PRECAGE
                        "X" +       //YTOTAL_WR
                        "X" +       //YTOTAL_PCSTRAND
                        " " +       //YTOT_PCSTRVAL
                        "X" +       //YTOT_COLD_ROLL
                        " " +       //YTOT_PRE_CUTWR
                        " " +       //YTOTAL_VALUE
                        "X" +       //YTOT_COUPLER
                        " " +       //YTOT_COUPVAL
                        "X" +       //YWIREROD_IND
                        "X" +       //YREBAR_IND
                        "X" +       //YCAB_IND
                        "X" +       //YMESH_IND
                        "X" +       //YPRECAGE
                        "X" +       //YBPC_IND
                        "X" +       //YPCSTRAND
                        "X" +       //YMAT_SOURCE
                        " " +       //YCAB_TYPE
                        " " +       //YORD_TYPE
                        " " +       //YWBS1
                        "X" +       //YCOLD_ROLL_WIRE
                        "X" +       //YPRE_CUT_WIRE
                        " " +       //YTOT_BILLET
                        " " +       //YBILLET_IND
                        " " +       //YCON_TYP
                        " " +       //YTOT_CAR
                        "X" +       //YCARPET_IND
                        "X"         //YCSURCHARGE_IND
                        );

                    RfcSessionManager.BeginContext(lSAP);
                    setOrderAPI.Invoke(lSAP);
                    setCommit.Invoke(lSAP);
                    RfcSessionManager.EndContext(lSAP);

                    String strVBELN;
                    String strReturnType;
                    String strReturnID;
                    String strReturnNumber;
                    String strReturnMessage;
                    String strReturnType2;
                    String strReturnID2;
                    String strReturnNumber2;
                    String strReturnMessage2;

                    IRfcTable SAPReturn = setOrderAPI.GetTable("RETURN");
                    strVBELN = setOrderAPI.GetValue("SALESDOCUMENT").ToString();

                    strReturnMessage = "";
                    if (strVBELN.Length <= 8)
                    {
                        if (SAPReturn.RowCount > 0)
                        {
                            for (int i = 0; i < SAPReturn.RowCount; i++)
                            {
                                strReturnType = SAPReturn[i].GetString("TYPE");
                                strReturnID = SAPReturn[i].GetString("ID");
                                strReturnNumber = SAPReturn[i].GetString("NUMBER");
                                if (SAPReturn[i].GetString("MESSAGE") != null && SAPReturn[i].GetString("MESSAGE").ToUpper().IndexOf("SUCCESS") < 0)
                                {
                                    if (strReturnMessage == "")
                                    {
                                        strReturnMessage = SAPReturn[i].GetString("MESSAGE");
                                    }
                                    else
                                    {
                                        strReturnMessage = strReturnMessage + "\n" + SAPReturn[i].GetString("MESSAGE");
                                    }
                                }
                            }
                        }
                        else
                        {
                            strReturnType = setOrderAPI.GetTable("RETURN").GetString("TYPE");
                            strReturnID = setOrderAPI.GetTable("RETURN").GetString("ID");
                            strReturnNumber = setOrderAPI.GetTable("RETURN").GetString("NUMBER");
                            strReturnMessage = setOrderAPI.GetTable("RETURN").GetString("MESSAGE");
                        }
                    }

                    strReturnType2 = setCommit.GetStructure("RETURN").GetString("TYPE");
                    strReturnID2 = setCommit.GetStructure("RETURN").GetString("ID");
                    strReturnNumber2 = setCommit.GetStructure("RETURN").GetString("NUMBER");
                    strReturnMessage2 = setCommit.GetStructure("RETURN").GetString("MESSAGE");

                    if (strVBELN.Length > 8)
                    {
                        lReturn = strVBELN;
                    }
                    else
                    {
                        lReturn = strReturnMessage;
                    }

                    lConnect.Dispose();
                    lSAP = null;
                    lConnect = null;
                }

                catch (RfcCommunicationException e)
                {
                    lReturn = e.Message;
                }
                catch (RfcLogonException e)
                {
                    // user could not logon...
                    lReturn = e.Message;
                }
                catch (RfcAbapRuntimeException e)
                {
                    // serious problem on ABAP system side...
                    lReturn = e.Message;
                }
                catch (RfcAbapBaseException e)
                {
                    lReturn = e.Message;
                    // The function module returned an ABAP exception, an ABAP message
                    // or an ABAP class-based exception...
                }
                catch (Exception e)
                {
                    lReturn = e.Message;
                    // Other type of exception
                }
            }

            lProcess = null;

            return lReturn;
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("CreateSAPSO")]
        public string CreateSAPSO(CreateSAPSOSAPAPIDto createSAPSOSAPAPIDto)
        {

            string lReturn = "";
            List<string> lSAPCode;
            string lBarType = "";
            short lBarSize = 0;
            int lBarLength = 0;
            string lCurrency = "";
            string lSQL = "";

            OracleCommand cmdOrdHdr = new OracleCommand();
            OracleDataReader lOrdRst;
            OracleConnection cnCIS = new OracleConnection();

            List<string> lSAPMaterialNo = new List<string>();
            List<int> lSAPkg = new List<int>();

            ProcessController lProcess = new ProcessController();

            lProcess.OpenCISConnection(ref cnCIS);

            lSQL = "SELECT WAERK " +
            "FROM SAPSR3.VBAK " +
            "WHERE MANDT = '" + lProcess.strClient + "' " +
            "AND VBELN = '" + createSAPSOSAPAPIDto.pContractNo + "' ";

            cmdOrdHdr.CommandText = lSQL;
            cmdOrdHdr.Connection = cnCIS;
            cmdOrdHdr.CommandTimeout = 300;
            lOrdRst = cmdOrdHdr.ExecuteReader();
            if (lOrdRst.HasRows)
            {
                lOrdRst.Read();
                lCurrency = lOrdRst.GetString(0) == null ? "" : lOrdRst.GetString(0);
            }
            lOrdRst.Close();
            lProcess.CloseCISConnection(ref cnCIS);

            lCurrency = lCurrency.Trim();

            //convert order to SAP material
            var lProdCode = (from p in db.ProdCode
                             orderby p.BarType, p.BarSize
                             select p).ToList();
            if (lProdCode.Count > 0 && createSAPSOSAPAPIDto.pBBSDetails.Count > 0)
            {
                for (var i = 0; i < createSAPSOSAPAPIDto.pBBSDetails.Count; i++)
                {
                    lSAPCode = new List<string>();
                    lBarType = createSAPSOSAPAPIDto.pBBSDetails[i].BarType;
                    lBarSize = (short)(createSAPSOSAPAPIDto.pBBSDetails[i].BarSize == null ? 0 : createSAPSOSAPAPIDto.pBBSDetails[i].BarSize);
                    lBarLength = (int)(createSAPSOSAPAPIDto.pBBSDetails[i].BarLength == null ? 0 : createSAPSOSAPAPIDto.pBBSDetails[i].BarLength);

                    lSAPCode = (from p in lProdCode
                                where p.BarType == lBarType &&
                                p.BarSize == lBarSize &&
                                p.BarLength == lBarLength
                                select p.ProdCodeSAP).ToList();
                    if (lSAPCode.Count > 0 && createSAPSOSAPAPIDto.pBBSDetails[i].BarTotalQty > 0)
                    {
                        if (lSAPCode[0].Length > 0 && createSAPSOSAPAPIDto.pBBSDetails[i].BarTotalQty > 0)
                        {
                            lSAPMaterialNo.Add(lSAPCode[0]);
                            lSAPkg.Add((int)createSAPSOSAPAPIDto.pBBSDetails[i].BarWeight);
                        }
                    }
                }
            }

            if (lSAPMaterialNo.Count > 0)
            {
                // get Contract Infor.

                try
                {
                    var lConnect = new SapConnection("DEV");
                    lConnect.Open();

                    var lSAP = lConnect.Destination;
                    IRfcFunction setOrderAPI = lSAP.Repository.CreateFunction("BAPI_SALESORDER_CREATEFROMDAT2");
                    IRfcFunction setCommit = lSAP.Repository.CreateFunction("BAPI_TRANSACTION_COMMIT");

                    IRfcStructure SAPStrucOrder_Header_In = setOrderAPI.GetStructure("ORDER_HEADER_IN");

                    var lOrderType = lProcess.getSAPOrderType(createSAPSOSAPAPIDto.pProjectCode, createSAPSOSAPAPIDto.pContractNo, createSAPSOSAPAPIDto.pPaymentType, "MTS");
                    SAPStrucOrder_Header_In.SetValue("DOC_TYPE", lOrderType);

                    var lSalesOrg = lProcess.getSAPSalesOrg(createSAPSOSAPAPIDto.pProjectCode, createSAPSOSAPAPIDto.pContractNo);
                    SAPStrucOrder_Header_In.SetValue("SALES_ORG", lSalesOrg);

                    SAPStrucOrder_Header_In.SetValue("DISTR_CHAN", "10");
                    SAPStrucOrder_Header_In.SetValue("DIVISION", "00");
                    SAPStrucOrder_Header_In.SetValue("REFDOC_CAT", "G");

                    SAPStrucOrder_Header_In.SetValue("REQ_DATE_H", createSAPSOSAPAPIDto.pReqDate);


                    if (createSAPSOSAPAPIDto.pContractNo != "" && createSAPSOSAPAPIDto.pPaymentType == "CREDIT")
                    {
                        SAPStrucOrder_Header_In.SetValue("PRICE_DATE", createSAPSOSAPAPIDto.pPriceDate);

                        if (lCurrency != "")
                        {
                            SAPStrucOrder_Header_In.SetValue("CURRENCY", lCurrency);
                        }

                        if (createSAPSOSAPAPIDto.pPaymentTerm != null && createSAPSOSAPAPIDto.pPaymentTerm.Trim() != "")
                        {
                            SAPStrucOrder_Header_In.SetValue("PMNTTRMS", createSAPSOSAPAPIDto.pPaymentTerm);
                        }

                        if (createSAPSOSAPAPIDto.pPriceGroup != null && createSAPSOSAPAPIDto.pPriceGroup.Trim() != "")
                        {
                            SAPStrucOrder_Header_In.SetValue("PRICE_GRP", createSAPSOSAPAPIDto.pPriceGroup);
                        }

                        if (createSAPSOSAPAPIDto.pIncoTerms1 != null && createSAPSOSAPAPIDto.pIncoTerms1.Trim() != "")
                        {
                            SAPStrucOrder_Header_In.SetValue("INCOTERMS1", createSAPSOSAPAPIDto.pIncoTerms1);

                            if (createSAPSOSAPAPIDto.pIncoTerms2 != null && createSAPSOSAPAPIDto.pIncoTerms2.Trim() != "")
                            {
                                SAPStrucOrder_Header_In.SetValue("INCOTERMS2", createSAPSOSAPAPIDto.pIncoTerms2);
                            }
                        }
                    }

                    //PO Number, PO Date
                    SAPStrucOrder_Header_In.SetValue("PURCH_NO_C", createSAPSOSAPAPIDto.pPONo);
                    SAPStrucOrder_Header_In.SetValue("PURCH_DATE", createSAPSOSAPAPIDto.pPODate);

                    //Contract Number
                    if (createSAPSOSAPAPIDto.pContractNo != "")
                    {
                        SAPStrucOrder_Header_In.SetValue("REF_DOC", createSAPSOSAPAPIDto.pContractNo);
                    }

                    //Order Source
                    string lOrderSource = "CUS-UX";
                    if (createSAPSOSAPAPIDto.pUserID == null || createSAPSOSAPAPIDto.pUserID.IndexOf("@") < 0 || createSAPSOSAPAPIDto.pUserID.ToLower().IndexOf("@natsteel.com.sg") > 0)
                    {
                        lOrderSource = "NSH-UX";
                    }
                    SAPStrucOrder_Header_In.SetValue("NAME", lOrderSource);

                    // Internal/External Remarks
                    IRfcTable lRemarks = setOrderAPI.GetTable("ORDER_TEXT");

                    lRemarks.Append();
                    lRemarks.SetValue("TEXT_ID", "Z010");
                    lRemarks.SetValue("LANGU", "EN");
                    lRemarks.SetValue("TEXT_LINE", createSAPSOSAPAPIDto.pInRemarks);

                    lRemarks.Append();
                    lRemarks.SetValue("TEXT_ID", "Z011");
                    lRemarks.SetValue("LANGU", "EN");
                    lRemarks.SetValue("TEXT_LINE", createSAPSOSAPAPIDto.pExRemarks);

                    if (createSAPSOSAPAPIDto.pContractNo == "")
                    {
                        lRemarks.Append();
                        lRemarks.SetValue("TEXT_ID", "Z013");
                        lRemarks.SetValue("LANGU", "EN");
                        lRemarks.SetValue("TEXT_LINE", createSAPSOSAPAPIDto.pInvRemarks);
                    }

                    IRfcTable SAPTableOrder_Partners = setOrderAPI.GetTable("ORDER_PARTNERS");

                    SAPTableOrder_Partners.Append();
                    //Customer Code
                    SAPTableOrder_Partners.SetValue("PARTN_ROLE", "AG");
                    SAPTableOrder_Partners.SetValue("PARTN_NUMB", createSAPSOSAPAPIDto.pCustomerCode);
                    SAPTableOrder_Partners.Append();
                    //Project code
                    SAPTableOrder_Partners.SetValue("PARTN_ROLE", "WE");
                    SAPTableOrder_Partners.SetValue("PARTN_NUMB", createSAPSOSAPAPIDto.pProjectCode);


                    IRfcTable SAPTableOrder_Items_In = setOrderAPI.GetTable("ORDER_ITEMS_IN");
                    IRfcTable SAPTableOrder_Schedule_In = setOrderAPI.GetTable("ORDER_SCHEDULES_IN");

                    int lngTmpPosNr = 0;

                    for (int i = 0; i < lSAPMaterialNo.Count; i++)
                    {
                        lngTmpPosNr = (i + 1) * 10;
                        SAPTableOrder_Items_In.Append();
                        SAPTableOrder_Items_In.SetValue("ITM_NUMBER", lngTmpPosNr);
                        SAPTableOrder_Items_In.SetValue("MATERIAL", lSAPMaterialNo[i]);
                        if (createSAPSOSAPAPIDto.pContractNo != "")
                        {
                            SAPTableOrder_Items_In.SetValue("REF_DOC", createSAPSOSAPAPIDto.pContractNo);
                            SAPTableOrder_Items_In.SetValue("REF_DOC_IT", "000000");

                            SAPTableOrder_Items_In.SetValue("REF_DOC_CA", "G");
                        }

                        //BBS No, BBS Description
                        //SAPTableOrder_Items_In.SetValue("YBBS_NO", pBBSNo);
                        //SAPTableOrder_Items_In.SetValue("YBBS_DESCR", pBBSDesc);

                        lRemarks.Append();
                        lRemarks.SetValue("TEXT_ID", "Z106");
                        lRemarks.SetValue("ITM_NUMBER", lngTmpPosNr);
                        lRemarks.SetValue("LANGU", "EN");
                        lRemarks.SetValue("TEXT_LINE", createSAPSOSAPAPIDto.pBBSNo);

                        lRemarks.Append();
                        lRemarks.SetValue("TEXT_ID", "Z107");
                        lRemarks.SetValue("ITM_NUMBER", lngTmpPosNr);
                        lRemarks.SetValue("LANGU", "EN");
                        lRemarks.SetValue("TEXT_LINE", createSAPSOSAPAPIDto.pBBSDesc);

                        SAPTableOrder_Schedule_In.Append();
                        SAPTableOrder_Schedule_In.SetValue("ITM_NUMBER", lngTmpPosNr);
                        SAPTableOrder_Schedule_In.SetValue("SCHED_LINE", 1);
                        SAPTableOrder_Schedule_In.SetValue("REQ_QTY", lSAPkg[i] / 1000);
                        SAPTableOrder_Schedule_In.SetValue("REQ_DATE", createSAPSOSAPAPIDto.pReqDate);
                    }

                    IRfcTable SAPTableOrder_Ext_In = setOrderAPI.GetTable("EXTENSIONIN");

                    SAPTableOrder_Ext_In.Append();
                    SAPTableOrder_Ext_In.SetValue("STRUCTURE", "BAPE_VBAK");
                    SAPTableOrder_Ext_In.SetValue("VALUEPART1",
                        "          " +                                                  //SO (10)
                        (createSAPSOSAPAPIDto.pVBAKExt.YTOT_GRAND / 1000).ToString("0000000000000.000") +    //YTOT_GRAND (17)
                        "                 " +                                           //YTOT_CAB (17)
                        "                 " +                                           //YTOT_MESH (17)
                        (createSAPSOSAPAPIDto.pVBAKExt.YTOT_REBAR / 1000).ToString("0000000000000.000") +    //YTOT_REBAR (17)
                        "                 " +                                           //YTOT_BPC (17)
                        "                 " +                                           //YTOT_PRECAGE (17)
                        "                 " +                                           //YTOTAL_WR (17)
                        "                 " +                                           //YTOTAL_PCSTRAND (17)
                        "                " +                                            //YTOT_PCSTRVAL (16)
                        "                 " +                                           //YTOT_COLD_ROLL (17)
                        "                 " +                                           //YTOT_PRE_CUTWR (17)
                        "                " +                                            //YTOTAL_VALUE (16)
                        "      " +                                                      //YTOT_COUPLER (6)
                        "                " +                                            //YTOT_COUPVAL (16)
                        createSAPSOSAPAPIDto.pVBAKExt.YWIREROD_IND +
                        createSAPSOSAPAPIDto.pVBAKExt.YREBAR_IND +
                        createSAPSOSAPAPIDto.pVBAKExt.YCAB_IND +
                        createSAPSOSAPAPIDto.pVBAKExt.YMESH_IND +
                        createSAPSOSAPAPIDto.pVBAKExt.YPRECAGE +
                        createSAPSOSAPAPIDto.pVBAKExt.YBPC_IND);

                    SAPTableOrder_Ext_In.SetValue("VALUEPART2",
                        createSAPSOSAPAPIDto.pVBAKExt.YPCSTRAND +
                        createSAPSOSAPAPIDto.pVBAKExt.YMAT_SOURCE +
                        " " +                                       //YCAB_TYPE
                        " " +                                       //YORD_TYPE
                        "                                        " +    //YWBS1 (40)
                        createSAPSOSAPAPIDto.pVBAKExt.YCOLD_ROLL_WIRE +
                        createSAPSOSAPAPIDto.pVBAKExt.YPRE_CUT_WIRE +
                        "                 " +                       //YTOT_BILLET(17)
                        " " +                                       //YBILLET_IND
                        " " +                                       //YCON_TYP
                        "                 " +                       //YTOT_CAR(17)
                        createSAPSOSAPAPIDto.pVBAKExt.YCARPET_IND +
                        createSAPSOSAPAPIDto.pVBAKExt.YCSURCHARGE_IND);


                    SAPTableOrder_Ext_In.Append();
                    SAPTableOrder_Ext_In.SetValue("STRUCTURE", "BAPE_VBAKX");
                    SAPTableOrder_Ext_In.SetValue("VALUEPART1", "          X  X" +
                        " " +       //YTOT_BPC
                        " " +       //YTOT_PRECAGE
                        " " +       //YTOTAL_WR
                        " " +       //YTOTAL_PCSTRAND
                        " " +       //YTOT_PCSTRVAL
                        " " +       //YTOT_COLD_ROLL
                        " " +       //YTOT_PRE_CUTWR
                        " " +       //YTOTAL_VALUE
                        " " +       //YTOT_COUPLER
                        " " +       //YTOT_COUPVAL
                        "X" +       //YWIREROD_IND
                        "X" +       //YREBAR_IND
                        "X" +       //YCAB_IND
                        "X" +       //YMESH_IND
                        "X" +       //YPRECAGE
                        "X" +       //YBPC_IND
                        "X" +       //YPCSTRAND
                        "X" +       //YMAT_SOURCE
                        " " +       //YCAB_TYPE
                        " " +       //YORD_TYPE
                        " " +       //YWBS1
                        "X" +       //YCOLD_ROLL_WIRE
                        "X" +       //YPRE_CUT_WIRE
                        " " +       //YTOT_BILLET
                        " " +       //YBILLET_IND
                        " " +       //YCON_TYP
                        " " +       //YTOT_CAR
                        "X" +       //YCARPET_IND
                        "X"         //YCSURCHARGE_IND
                        );

                    RfcSessionManager.BeginContext(lSAP);
                    setOrderAPI.Invoke(lSAP);
                    setCommit.Invoke(lSAP);
                    RfcSessionManager.EndContext(lSAP);

                    String strVBELN;
                    String strReturnType;
                    String strReturnID;
                    String strReturnNumber;
                    String strReturnMessage;
                    String strReturnType2;
                    String strReturnID2;
                    String strReturnNumber2;
                    String strReturnMessage2;

                    IRfcTable SAPReturn = setOrderAPI.GetTable("RETURN");
                    strVBELN = setOrderAPI.GetValue("SALESDOCUMENT").ToString();

                    strReturnMessage = "";
                    if (strVBELN.Length <= 8)
                    {
                        if (SAPReturn.RowCount > 0)
                        {
                            for (int i = 0; i < SAPReturn.RowCount; i++)
                            {
                                strReturnType = SAPReturn[i].GetString("TYPE");
                                strReturnID = SAPReturn[i].GetString("ID");
                                strReturnNumber = SAPReturn[i].GetString("NUMBER");
                                if (SAPReturn[i].GetString("MESSAGE") != null && SAPReturn[i].GetString("MESSAGE").ToUpper().IndexOf("SUCCESS") < 0)
                                {
                                    if (strReturnMessage == "")
                                    {
                                        strReturnMessage = SAPReturn[i].GetString("MESSAGE");
                                    }
                                    else
                                    {
                                        strReturnMessage = strReturnMessage + "\n" + SAPReturn[i].GetString("MESSAGE");
                                    }
                                }
                            }
                        }
                        else
                        {
                            strReturnType = setOrderAPI.GetTable("RETURN").GetString("TYPE");
                            strReturnID = setOrderAPI.GetTable("RETURN").GetString("ID");
                            strReturnNumber = setOrderAPI.GetTable("RETURN").GetString("NUMBER");
                            strReturnMessage = setOrderAPI.GetTable("RETURN").GetString("MESSAGE");
                        }
                    }
                    strReturnType2 = setCommit.GetStructure("RETURN").GetString("TYPE");
                    strReturnID2 = setCommit.GetStructure("RETURN").GetString("ID");
                    strReturnNumber2 = setCommit.GetStructure("RETURN").GetString("NUMBER");
                    strReturnMessage2 = setCommit.GetStructure("RETURN").GetString("MESSAGE");

                    if (strVBELN.Length > 8)
                    {
                        lReturn = strVBELN;
                    }
                    else
                    {
                        lReturn = strReturnMessage;
                    }

                    lConnect.Dispose();
                    lSAP = null;
                    lConnect = null;
                }

                catch (RfcCommunicationException e)
                {
                    lReturn = e.Message;
                }
                catch (RfcLogonException e)
                {
                    // user could not logon...
                    lReturn = e.Message;
                }
                catch (RfcAbapRuntimeException e)
                {
                    // serious problem on ABAP system side...
                    lReturn = e.Message;
                }
                catch (RfcAbapBaseException e)
                {
                    lReturn = e.Message;
                    // The function module returned an ABAP exception, an ABAP message
                    // or an ABAP class-based exception...
                }
                catch (Exception e)
                {
                    lReturn = e.Message;
                    // Other type of exception
                }
            }

            lProcess = null;

            return lReturn;
        }


        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("index1")]
        public int index1()
        {
            return 0;
        }
    }
}