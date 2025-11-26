using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
//using Microsoft.Data.SqlClient;
using OrderService.Dtos;
using OrderService.Interfaces;
using Oracle.ManagedDataAccess.Client;
using Microsoft.AspNet.Identity;
using System.Data.SqlClient;
using System.Net;
using System.Globalization;
using OrderService.Models;
using System.Web;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Text;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
//using RestSharp;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderAmendmentController : ControllerBase
    {
        public string gUserType = "";
        public string gGroupName = "";
        private string UserType;

        private readonly IOrder _OrderRepository;
        private readonly IMapper _mapper;

        private DBContextModels db = new DBContextModels();


        public OrderAmendmentController(IOrder orderService, IMapper mapper)
        {
            _OrderRepository = orderService;
            _mapper = mapper;

        }


        #region Main functionality



        [HttpGet]
        [Route("/getProject_Amendment/{CustomerCode}/{UserName}")]
        //[ValidateAntiForgeryHeader]
        public ActionResult getProject(string CustomerCode,string UserName)
        {
            string lUserType = "";
            string lGroupName = "";
            //OracleDataReader lRst;
            var lDa = new SqlDataAdapter();
            var lCmd = new SqlCommand();
            var lDs = new DataSet();
            var cnNDS = new SqlConnection();
            if (gUserType != null && gUserType != "" && gGroupName != null && gGroupName != "")
            {
                lUserType = gUserType;
                lGroupName = gGroupName;
            }
            else
            {
                UserAccessController lUa = new UserAccessController();
                lUserType = lUa.getUserType(UserName);
                lGroupName = lUa.getGroupName(UserName);
                gUserType = lUserType;
                gGroupName = lGroupName;
                lUa = null;
            }
            List<ProjectListModels> content = new List<ProjectListModels> {
                new ProjectListModels
                {
                    CustomerCode = "",
                    ProjectCode = "",
                    ProjectTitle = " -- Select Project -- "
                } };
            if (lUserType == "PL" || lUserType == "AD" || lUserType == "PM" || lUserType == "PA" || lUserType == "P1" || lUserType == "P2" || lUserType == "PU" || lUserType == "TE")
            {
                content = new List<ProjectListModels>();
                content.Add(new ProjectListModels
                {
                    CustomerCode = "",
                    ProjectCode = "",
                    ProjectTitle = " -- Select Project -- "
                });


                #region Get from SAP

              
                var lProcessObj = new ProcessController();
                OracleDataAdapter oda = new OracleDataAdapter();


                string SHIP_TO_PARTY_PARTNER_CODE = "('WE','S1','S2','S3','S4','S5','S6','S7','S8','S9','T1','T2','T3','T4','T5','T6','T7','T8','T9','U1')";

                lCmd.CommandText = "select project_name,hpm.project_code  from HMIContractMaster hcm " +
                    "inner join HMIContractProject hcp " +
                    "on hcm.CONTRACT_NO=hcp.CONTRACT_NO " +
                    "inner join HMIProjectMaster hpm " +
                    "on hcp.PROJECT_CODE=hpm.project_code " +
                    "where hcm.CUST_ID='"+ CustomerCode + "'";

                if (lProcessObj.OpenNDSConnection(ref cnNDS) == true)
                {
                    lCmd.Connection = cnNDS;
                    lDa.SelectCommand = lCmd;
                    lDs.Clear();
                    lDa.Fill(lDs);
                    if (lDs.Tables[0].Rows.Count > 0)
                        for (int i = 0; i < lDs.Tables[0].Rows.Count; i++)
                        {
                            string lName = ((string)lDs.Tables[0].Rows[i].ItemArray[0]).Trim();
                            string lCode = ((string)lDs.Tables[0].Rows[i].ItemArray[1]).Trim();
                            content.Add(new ProjectListModels
                            {
                                CustomerCode = CustomerCode,
                                ProjectCode = lCode,
                                ProjectTitle = lName
                            });
                        }
                    lProcessObj.CloseNDSConnection(ref cnNDS);
                }
                lDa = null;
                lCmd = null;
                lDs = null;
                cnNDS = null;
                lProcessObj = null;
                #endregion


                //#region Get from NDS
                //var lDa = new OleDbDataAdapter();
                //var lOCmd = new OleDbCommand();
                //var lDs = new DataSet();
                //var lDStatus = new DataSet();
                //var lOcisCon = new OleDbConnection();
                //var lProcess = new ProcessController();
                //lOCmd.CommandText = "SELECT P.vchProjectCode, P.vchProjectName " +
                //"FROM dbo.ProjectMaster P, " +
                //"dbo.ContractMaster C, " +
                //"dbo.CustomerMaster A, " +
                //"dbo.ContractProductMapping M " +
                //"WHERE P.intContractID = C.intContractID " +
                //"AND A.intCustomerCode = C.intCustomerCode " +
                //"AND C.vchContractNumber = M.VBELN " +
                //"AND M.mandt = '" + lProcess.strClient + "' " +
                //"AND datEndDate > DATEADD(dd, -180, getDate()) " +
                //"AND vchCustomerNo = '" + CustomerCode + "' " +
                //"GROUP BY P.vchProjectCode, P.vchProjectName " +
                //"ORDER BY P.vchProjectName ";
                //if (lProcess.OpenNDSConnection(ref lOcisCon) == true)
                //{
                //    lOCmd.Connection = lOcisCon;
                //    lDa.SelectCommand = lOCmd;
                //    lDs.Clear();
                //    lDa.Fill(lDs);
                //    if (lDs.Tables[0].Rows.Count > 0)
                //        for (int i = 0; i < lDs.Tables[0].Rows.Count; i++)
                //        {
                //            string lCode = ((string)lDs.Tables[0].Rows[i].ItemArray[0]).Trim();
                //            string lName = ((string)lDs.Tables[0].Rows[i].ItemArray[1]).Trim();
                //            content.Add(new ProjectListModels
                //            {
                //                CustomerCode = CustomerCode,
                //                ProjectCode = lCode,
                //                ProjectTitle = lName
                //            });
                //        }
                //    lProcess.CloseNDSConnection(ref lOcisCon);
                //}
                //lDa = null;
                //lOCmd = null;
                //lDs = null;
                //lDStatus = null;
                //lOcisCon = null;
                //lProcess = null;
                //#endregion
            }
            else
            {
                content = (from p in db.ProjectList
                           where p.CustomerCode == CustomerCode &&
                           (from u in db.UserAccess
                            where u.UserName == lGroupName &&
                            u.CustomerCode == CustomerCode
                            select u.ProjectCode).Contains(p.ProjectCode)
                           orderby p.ProjectTitle
                           select p).ToList();
            }
            if (content.Count() == 0)
            {
                content = new List<ProjectListModels> {
                        new ProjectListModels
                        {
                            CustomerCode = "",
                            ProjectCode = "",
                            ProjectTitle = " -- Select Project -- "
                        }
                    };
            }
            return Ok(content);
            //return Json(content, JsonRequestBehavior.AllowGet); //commented by vidya
        }


        [HttpPost]
        [Route("/OrdersForAmendment/{RDateFrom}/{RDateTo}/{Rev_required_Conf_date_from}/{Rev_required_Conf_date_to}/{Rev_Req_Confirmed_Date_Search_Range}")]
        //[ValidateAntiForgeryHeader]       
        //public ActionResult OrdersForAmendment(string CustomerCode, string ProjectCode, string PODateFrom, string PODateTo, string RDateFrom, string RDateTo, string SORNofrom, string SORNoTo, string SONofrom, string SONoTo, string WBS1, string WBS2, string WBS3, string SearchOptions, string txtSearch)
        //public ActionResult OrdersForAmendment(string CustomerCode, string ProjectCode, string RDateFrom, string RDateTo, string SORNofrom, string SORNoTo, string WBS1, string WBS2, string WBS3, string SearchOptions, string txtSearch, string SO_SOR_Search_Range, string Req_PO_Date_Search_Range, string Rev_required_Conf_date_from, string Rev_required_Conf_date_to, string Rev_Req_Confirmed_Date_Search_Range, string SearchProducts, string SearchOptionsByDesignation, string lSearchByDesignation)
        public ActionResult OrdersForAmendment(OrdersForAmendmentDto ordersForAmendmentDto, string RDateFrom, string RDateTo, string Rev_required_Conf_date_from, string Rev_required_Conf_date_to, string Rev_Req_Confirmed_Date_Search_Range)
        {
            RDateFrom = ordersForAmendmentDto.RDateFrom;
            RDateTo = ordersForAmendmentDto.RDateTo;
            Rev_required_Conf_date_from = ordersForAmendmentDto.Rev_required_Conf_date_from;
            Rev_required_Conf_date_to = ordersForAmendmentDto.Rev_required_Conf_date_to;

            var lReturn = (new[]{ new
                {
                    SrNo = 0,
                    SONo = "",
                    Customer = "",
                    Project = "",
                    ContractNo = "",
                    ProjCoord = "",
                    ReqDateFr = "",
                    ReqDateTo = "",
                    ConfirmedDate = "",
                    HallAssignment = "",
                    OnHold = "",
                    LorryCrane = "",
                    DoNotMix = "",
                    CallBefDelivery = "",
                    SpecialPass = "",
                    BargeBooked = "",
                    CraneBooked = "",
                    PoliceEscort = "",
                    PremiumService = "",
                    UrgentOrder = "",
                    ConquasOrder = "",
                    ZeroTolerance = "",
                    LowBedVehicleAllowed = "",
                    FiftyTonVehicleAllowed = "",
                    UnitMode = "",
                    //ProjectCastingDate = "",
                    //MustHaveDate = "",
                    SalesOrder = "",
                    SORNo = "",
                    GroupID = "",
                    PONumber = "",
                    PODate = "",
                    BBSNo = "",
                    //LOADNO = "",
                    ProductType = "",
                    IntRemark = "",
                    ExtRemark = "",
                    STELEMENTTYPE = "",
                    WBS1 = "",
                    WBS2 = "",
                    WBS3 = "",
                    STATUS = "",
                    WT_DATE = "",
                    SOR_STATUS = "",
                    USERID = "",
                    DELIVERY_STATUS = "",
                    Total_Tonnage = "",
                    //Total_Tonnage = 0.000,
                    LP_No_of_Pieces = ""
                }}).ToList();
            DataSet ds = new DataSet();

            try
            {
                Getdata(ordersForAmendmentDto.CustomerCode, ordersForAmendmentDto.ProjectCode, ref RDateFrom, ref RDateTo, ordersForAmendmentDto.SORNofrom, ordersForAmendmentDto.SORNoTo, ordersForAmendmentDto.WBS1, ordersForAmendmentDto.WBS2, ordersForAmendmentDto.WBS3, ordersForAmendmentDto.SearchOptions, ordersForAmendmentDto.txtSearch, ordersForAmendmentDto.SO_SOR_Search_Range, ordersForAmendmentDto.Req_PO_Date_Search_Range, ref Rev_required_Conf_date_from, ref Rev_required_Conf_date_to, Rev_Req_Confirmed_Date_Search_Range, ordersForAmendmentDto.SearchProducts, ds, ordersForAmendmentDto.SearchOptionsByDesignation, ordersForAmendmentDto.lSearchByDesignation);
                int serial = 1;
                if (ds != null && ds.Tables.Count > 0)
                {
                    //var lCmd = new OracleCommand();
                    //var lcisCon = new OracleConnection();

                    var lCmd = new SqlCommand();
                    var lndsCon = new SqlConnection();

                    var lProcessObj = new ProcessController();

                    //OracleDataAdapter oda = new OracleDataAdapter();
                    SqlDataAdapter oda = new SqlDataAdapter();

                    DataSet dsLoads = new DataSet();
                    DataSet dsLPPieces = new DataSet();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        String strLPPieces = string.Empty;
                        //try
                        //{
                        //    dsLoads = new DataSet();
                        //    lCmd.CommandText = "select distinct load_no  FROM SAPSR3.YMPPT_LP_BATCH_C  WHERE MANDT='" + lProcessObj.strClient + "'"
                        //        + " AND sales_order = '" + Convert.ToString(dr["SALES_ORDER"]) + "' AND status<> 'X'";
                        //    lProcessObj.OpenCISConnection(ref lcisCon);
                        //    oda.SelectCommand = lCmd;
                        //    lCmd.Connection = lcisCon;
                        //    lCmd.CommandTimeout = 15000;
                        //    oda.Fill(dsLoads);
                        //    lProcessObj.CloseCISConnection(ref lcisCon);
                        //    if (dsLoads != null && dsLoads.Tables.Count > 0)
                        //    {
                        //        dsLPPieces = new DataSet();
                        //        foreach (DataRow drLoads in dsLoads.Tables[0].Rows)
                        //        {
                        //            lCmd.CommandText = "select sum(b.no_pieces) as deli_pcs from SAPSR3.YMPPT_LOAD_VEHIC a,SAPSR3.YMPPT_LP_BATCH_C b WHERE b.MANDT='" + lProcessObj.strClient + "'"
                        //                + " AND b.sales_order = '" + Convert.ToString(dr["SALES_ORDER"]) + "' AND b.load_no = '" + Convert.ToString(drLoads["Load_no"]) + "'"
                        //                + " AND b.status <> 'X' and a.vbeln = b.sales_order and a.POSNR = b.order_item and a.load_no = b.load_no "
                        //                + " and a.tallied_batch = b.tallied_batch  and a.delivery_no = b.delivery_no "
                        //                + " group by a.vbeln,a.load_no order by a.vbeln,a.load_no";
                        //            lProcessObj.OpenCISConnection(ref lcisCon);
                        //            oda.SelectCommand = lCmd;
                        //            lCmd.Connection = lcisCon;
                        //            lCmd.CommandTimeout = 15000;
                        //            oda.Fill(dsLPPieces);
                        //            lProcessObj.CloseCISConnection(ref lcisCon);
                        //            if (dsLPPieces != null && dsLPPieces.Tables.Count > 0)
                        //            {
                        //                if (dsLPPieces.Tables[0].Rows.Count > 0)
                        //                {
                        //                    strLPPieces = strLPPieces + " " + Convert.ToString(drLoads["Load_no"]) + "(" + Convert.ToString(dsLPPieces.Tables[0].Rows[0][0]) + "),";
                        //                }
                        //            }
                        //        }
                        //    }
                        //}

                        try
                        {
                            dsLoads = new DataSet();

                            // Step 1: Fetch distinct load numbers
                            lCmd.CommandText = "SELECT DISTINCT LOAD_NO FROM HMILoadDetails WHERE SALES_ORDER = '" + Convert.ToString(dr["SALES_ORDER"]) + "'";
                            lProcessObj.OpenNDSConnection(ref lndsCon);
                            oda.SelectCommand = lCmd;
                            lCmd.Connection = lndsCon;
                            lCmd.CommandTimeout = 15000;
                            oda.Fill(dsLoads);
                            lProcessObj.CloseNDSConnection(ref lndsCon);

                            // Step 2: Process each load to get delivered pieces
                            if (dsLoads != null && dsLoads.Tables.Count > 0)
                            {
                                dsLPPieces = new DataSet();

                                foreach (DataRow drLoads in dsLoads.Tables[0].Rows)
                                {
                                    string salesOrder = Convert.ToString(dr["SALES_ORDER"]);
                                    string loadNo = Convert.ToString(drLoads["LOAD_NO"]);

                                    lCmd.CommandText = "SELECT SALES_ORDER, LOAD_NO, SUM(Load_Pcs) AS deli_pcs " +
                                                       "FROM HMILoadDetails " +
                                                       "WHERE SALES_ORDER = '" + salesOrder + "' AND LOAD_NO = '" + loadNo + "' " +
                                                       "GROUP BY SALES_ORDER, LOAD_NO " +
                                                       "ORDER BY SALES_ORDER, LOAD_NO";

                                    lProcessObj.OpenNDSConnection(ref lndsCon);
                                    oda.SelectCommand = lCmd;
                                    lCmd.Connection = lndsCon;
                                    lCmd.CommandTimeout = 15000;
                                    oda.Fill(dsLPPieces);
                                    lProcessObj.CloseNDSConnection(ref lndsCon);

                                    if (dsLPPieces != null && dsLPPieces.Tables.Count > 0 && dsLPPieces.Tables[0].Rows.Count > 0)
                                    {
                                        string pcs = Convert.ToString(dsLPPieces.Tables[0].Rows[0]["deli_pcs"]);
                                        strLPPieces += " " + loadNo + "(" + pcs + "),";
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                        lReturn.Add(new
                        {
                            SrNo = serial,
                            SONo = Convert.ToString(dr["SALES_ORDER"]),
                            Customer = Convert.ToString(dr["Customer"]),
                            Project = Convert.ToString(dr["Project"]),
                            ContractNo = Convert.ToString(dr["Contract"]),
                            ProjCoord = Convert.ToString(dr["PROJ_COORD"]),
                            //ReqDateFr = Convert.ToString(dr["REQ_DAT_FRM"]),
                            ReqDateFr = Convert.ToString(dr["REQ_DAT_FRM"]),
                            //ReqDateFr = (!string.IsNullOrEmpty(Convert.ToString(dr["REQ_DAT_FRM"]))) ? Convert.ToDateTime(dr["REQ_DAT_FRM"]).ToShortDateString() : "",
                            //ReqDateTo = Convert.ToString(dr["REQ_DAT_TO"]),
                            //ReqDateTo = (SearchProducts == "MTS" ? null : Convert.ToString(dr["REQ_DAT_TO_STR"])), //Commented by aishwarya--CHM127362420 - Sorting enhancement for Multiple order amendment form in Digios
                            ReqDateTo = Convert.ToString(dr["REQ_DAT_TO"]),
                            //ReqDateTo = (!string.IsNullOrEmpty(Convert.ToString(dr["REQ_DAT_TO"]))) ? Convert.ToDateTime(dr["REQ_DAT_TO"]).ToShortDateString() : "",
                            ConfirmedDate = Convert.ToString(dr["CONF_DEL_DATE"]),
                            //ConfirmedDate = (!string.IsNullOrEmpty(Convert.ToString(dr["CONF_DEL_DATE"]))) ? Convert.ToDateTime(dr["CONF_DEL_DATE"]).ToShortDateString() : "",
                            HallAssignment = Convert.ToString(dr["Hall_Assignment"]),
                            OnHold = Convert.ToString(dr["ON_HOLD_IND"]),
                            LorryCrane = Convert.ToString(dr["LORRY_CRANE_IND"]),
                            DoNotMix = Convert.ToString(dr["DO_NOT_MIX_IND"]),
                            CallBefDelivery = Convert.ToString(dr["CALL_BEF_DEL_IND"]),
                            SpecialPass = Convert.ToString(dr["SPECIAL_PASS_IND"]),
                            BargeBooked = Convert.ToString(dr["BRG_BKD_IND"]),
                            CraneBooked = Convert.ToString(dr["CRN_BKD_IND"]),
                            PoliceEscort = Convert.ToString(dr["POL_ESC_IND"]),
                            PremiumService = Convert.ToString(dr["PRM_SVC_IND"]),
                            UrgentOrder = Convert.ToString(dr["URG_ORD_IND"]),
                            ConquasOrder = Convert.ToString(dr["CONQUAS_IND"]),
                            ZeroTolerance = Convert.ToString(dr["ZERO_TOLERANCE_I"]),
                            LowBedVehicleAllowed = Convert.ToString(dr["LOW_BED_VEH_IND"]),
                            FiftyTonVehicleAllowed = Convert.ToString(dr["TON50_VEH_IND"]),
                            UnitMode = Convert.ToString(dr["UNIT_MODE_IND"]),
                            //ProjectCastingDate = Convert.ToString(dr["PRJ_CST_DATE"]),
                            //MustHaveDate = Convert.ToString(dr["MUST_HAVE_DATE"]),
                            SalesOrder = Convert.ToString(dr["Sales_Order"]),
                            SORNo = Convert.ToString(dr["SOR_No"]),
                            GroupID = Convert.ToString(dr["ORDER_GROUP_ID"]),
                            PONumber = Convert.ToString(dr["PO_Number"]),
                            PODate = Convert.ToString(dr["PO_Date"]),
                            BBSNo = Convert.ToString(dr["BBS_No"]),
                            //LOADNO = Convert.ToString(dr["LOAD_NO"]),
                            ProductType = Convert.ToString(dr["PRODUCT_TYPE"]),
                            IntRemark = Convert.ToString(dr["Int_Remark"]),
                            ExtRemark = Convert.ToString(dr["Ext_Remark"]),
                            STELEMENTTYPE = Convert.ToString(dr["ST_ELEMENT_TYPE"]),
                            WBS1 = Convert.ToString(dr["WBS1"]),
                            WBS2 = Convert.ToString(dr["WBS2"]),
                            WBS3 = Convert.ToString(dr["WBS3"]),
                            STATUS = Convert.ToString(dr["STATUS"]),
                            WT_DATE = Convert.ToString(dr["WT_DATE"]),
                            SOR_STATUS = Convert.ToString(dr["SOR_STATUS"]),
                            USERID = Convert.ToString(dr["USERID"]),
                            DELIVERY_STATUS = Convert.ToString(dr["DELIVERY_STATUS"]),
                            Total_Tonnage = Convert.ToString(dr["Total_Tonnage"]),
                            //Total_Tonnage = (!String.IsNullOrEmpty(Convert.ToString(dr["Total_Tonnage"])) ? Convert.ToDouble(dr["Total_Tonnage"]) : 0),
                            LP_No_of_Pieces = ((strLPPieces != null && strLPPieces != "") ? strLPPieces.Remove(strLPPieces.Length - 1) : strLPPieces)//Convert.ToString(dr["LP_No_of_Pieces"])
                        });
                        serial = serial + 1;
                    }
                    lProcessObj = null;
                    lCmd = null;
                    lndsCon = null;
                    oda = null;
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                //if (lReturn.Count > 1)
                //{
                lReturn.RemoveAt(0);
                //}
            }
            //new { success = ViewBag.Counter }
            //return Json(new { response = lReturn, data = Grand_Total_Tonnage }, JsonRequestBehavior.AllowGet);
            //return Json(lReturn, JsonRequestBehavior.AllowGet);
            return Ok(lReturn);
        }

        private void Getdata(string CustomerCode, string ProjectCode, ref string RDateFrom, ref string RDateTo, string SORNofrom, string SORNoTo, string WBS1, string WBS2, string WBS3, string SearchOptions, string txtSearch, string SO_SOR_Search_Range, string Req_PO_Date_Search_Range, ref string Rev_required_Conf_date_from, ref string Rev_required_Conf_date_to, string Rev_Req_Confirmed_Date_Search_Range, string SearchProducts, DataSet ds, string SearchOptionsByDesignation, string lSearchByDesignation)
        {
            #region default Setting
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
            //if (!string.IsNullOrEmpty(PODateFrom) || !string.IsNullOrEmpty(PODateTo))
            //{
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
            //}
            if (!string.IsNullOrEmpty(RDateFrom) || !string.IsNullOrEmpty(RDateTo))
            {
                if (RDateFrom == null) RDateFrom = "";
                if (RDateFrom.Trim().Length == 0)
                {
                    RDateFrom = "2000-01-01";
                }
                if (RDateTo == null) RDateTo = "";
                if (RDateTo.Trim().Length == 0)
                {
                    RDateTo = "2200-01-01";
                }
            }
            if (!string.IsNullOrEmpty(Rev_required_Conf_date_from) || !string.IsNullOrEmpty(Rev_required_Conf_date_to))
            {
                if (Rev_required_Conf_date_from == null) Rev_required_Conf_date_from = "";
                if (Rev_required_Conf_date_from.Trim().Length == 0)
                {
                    Rev_required_Conf_date_from = "2000-01-01";
                }
                if (Rev_required_Conf_date_to == null) Rev_required_Conf_date_to = "";
                if (Rev_required_Conf_date_to.Trim().Length == 0)
                {
                    Rev_required_Conf_date_to = "2200-01-01";
                }
            }
            #endregion default Setting
            #region Commented Code
            //DataTable dtSalesOrders = new DataTable();
            //if (SearchOptions != "0")
            //{
            //    if (SearchOptions == "5" || SearchOptions == "6")
            //    {
            //        OracleConnection cnIDB = new OracleConnection();    //// 'SCM IDB
            //        OracleCommand cmdIDB = new OracleCommand(); ////'SCM IDB
            //        lProcessObj.OpenIDBConnection(ref cnIDB);
            //        switch (SearchOptions)
            //        {
            //            case "5":  //// Pile Number
            //                cmdIDB.CommandText = "select distinct order_no from order_item where BPC_PILE_NO LIKE '%" + txtSearch + "%' ";
            //                break;
            //            case "6":    //// Panel Number
            //                cmdIDB.CommandText = "select distinct order_no from order_item where PRC_PANEL_NO LIKE '%" + txtSearch + "%' ";
            //                break;
            //            default:
            //                break;
            //        }
            //        oda.SelectCommand = cmdIDB;
            //        cmdIDB.Connection = cnIDB;
            //        cmdIDB.CommandTimeout = 15000;
            //        oda.Fill(dtSalesOrders);
            //        lProcessObj.CloseIDBConnection(ref cnIDB);
            //        cmdIDB = null;
            //        cnIDB = null;
            //    }
            //}
            //lCmd.CommandText = "select distinct sales_order from SAPSR3.YMPPT_LP_ITEM_C" +
            //    //" where mandt ='" + strClientNo + "' and LOAD_NO ='" + strLoadNo + "' +
            //    " order by sales_order";
            #endregion Commented Code
            try
            {
                //var lCmd = new OracleCommand();
                //var lcisCon = new OracleConnection();
                //OracleDataAdapter oda = new OracleDataAdapter();


                var lCmd = new SqlCommand();
                var lndsCon = new SqlConnection();
                SqlDataAdapter oda = new SqlDataAdapter();


                var lProcessObj = new ProcessController();
                
                lCmd.CommandText =
                        "SELECT DISTINCT  " +
                        " H.NAME_AG AS CUSTOMER, " +
                        " H.NAME_WE AS PROJECT, " +
                        " H.CONTRACT, " +
                        " (SELECT TOP 1 PM.project_leader  " +
                        " FROM HMIProjectMaster PM " +
                        " INNER JOIN HMIContractProject CP ON CP.PROJECT_CODE = PM.project_code " +
                        " WHERE PM.project_code = H.KUNNR) AS PROJ_COORD, " +
                        " H.REQ_DAT_FRM, " +
                        " CASE WHEN H.REQ_DAT_FRM IS NOT NULL AND H.REQ_DAT_FRM <> ' ' " +
                        " THEN TRY_CAST(H.REQ_DAT_FRM AS DATETIME) " +
                        " END AS REQ_DAT_FRM_DATE, " +
                        " CASE WHEN H.REQ_DAT_FRM IS NOT NULL AND H.REQ_DAT_FRM <> ' ' AND H.REQ_DAT_FRM <> '00000000' " +
                        " THEN FORMAT(TRY_CAST(H.REQ_DAT_FRM AS DATETIME), 'yyyy/MM/dd') " +
                        " END AS REQ_DAT_FRM_STR, " +
                        " H.REQ_DAT_TO, " +
                        " CASE WHEN H.REQ_DAT_TO IS NOT NULL AND H.REQ_DAT_TO <> ' ' AND H.REQ_DAT_TO <> '00000000' " +
                        " THEN FORMAT(TRY_CAST(H.REQ_DAT_TO AS DATETIME), 'yyyy/MM/dd') " +
                        " END AS REQ_DAT_TO_STR, " +
                        " CASE WHEN H.STATUS IN ('C','S','Z') THEN ( " +
                        " SELECT FORMAT(TRY_CAST(MAX(ACT_DEL_DATE) AS DATE), 'yyyy/MM/dd') " +
                        " FROM DeliveredOrderdetailsHMI  " +
                        " WHERE SALES_ORDER = H.SALES_ORDER " +
                        " AND LEN(ACT_DEL_DATE) = 8 " +
                        " AND ACT_DEL_DATE <> '00000000' " +
                        " AND ACT_DEL_DATE NOT LIKE '%-%') " +
                        " END AS CONF_DEL_DATE, " +
                        " H.ON_HOLD_IND, " +
                        " H.ZERO_TOLERANCE_I, " +
                        " H.CALL_BEF_DEL_IND, " +
                        " H.CONQUAS_IND, " +
                        " H.SPECIAL_PASS_IND, " +
                        " H.DO_NOT_MIX_IND, " +
                        " H.LORRY_CRANE_IND, " +
                        " H.URG_ORD_IND, " +
                        " H.PRM_SVC_IND, " +
                        " H.CRN_BKD_IND, " +
                        " H.BRG_BKD_IND, " +
                        " H.POL_ESC_IND, " +
                        " H.WBS1 AS HALL_ASSIGNMENT, " +
                        " H.LOW_BED_VEH_IND, " +
                        " H.TON50_VEH_IND, " +
                        " H.UNIT_MODE_IND, " +
                        " H.SALES_ORDER, " +
                        " H.ORDER_REQUEST_NO AS SOR_NO, " +
                        " H.ORDER_GROUP_ID, " +
                        " H.PO_NUMBER, " +
                        " C.PRODUCT_TYPE, " +
                        " H.Int_Remark, " +
                        " H.Ext_Remark, " +
                        " C.ST_ELEMENT_TYPE, " +
                        " C.WBS1, " +
                        " C.WBS2, " +
                        " C.WBS3, " +
                        " C.BBS_No, " +
                        " CASE WHEN H.CUST_ORDER_DATE <> '00000000' AND H.CUST_ORDER_DATE <> ' ' " +
                        " THEN FORMAT(TRY_CAST(H.CUST_ORDER_DATE AS DATE), 'yyyy/MM/dd') " +
                        " END AS PO_DATE, " +
                        " CASE WHEN H.UDATE = '00000000' THEN NULL " +
                        " ELSE CAST(FORMAT(TRY_CAST(H.UDATE AS DATE), 'yyyy-MM-dd') + ' ' + FORMAT(TRY_CAST(H.UTIME AS TIME), 'HH:mm:ss') AS DATETIME) " +
                        " END AS UDATETIME, " +
                        " H.STATUS, " +
                        " H.ORDER_REQUEST_NO, " +
                        " H.STATUS AS SOR_STATUS, " +
                        " CASE WHEN product_marking <> ' ' THEN product_marking ELSE H.USERID END AS USERID, " +
                        " CASE WHEN HMLD.SALES_ORDER IS NULL THEN 'Pending' " +
                        " WHEN D.PARTIAL_DEL_IND = 'Completed' THEN 'Complete' " +
                        " ELSE 'Partial'  END AS DELIVERY_STATUS," +
                        " (SELECT FORMAT(SUM(Theo_Weight_kg) / 1000.0, '#,##0.000') " +
                        " FROM sap_order_item OI " +
                        " WHERE MANDT = '" + lProcessObj.strClient + "' " +
                        " AND OI.ORD_REQ_NO = H.ORDER_REQUEST_NO " +
                        " AND hg_item_no = '000000' " +
                        " AND H.STATUS <> 'X') AS Total_Tonnage, " +
                        " (SELECT SUM(TRY_CAST(No_Pieces AS DECIMAL(18, 2))) " +
                        " FROM sap_order_item OI " +
                        " WHERE MANDT = '" + lProcessObj.strClient + "' " +
                        " AND OI.ORD_REQ_NO = H.ORDER_REQUEST_NO " +
                        " AND OI.STATUS <> 'X') AS LP_No_of_Pieces, " +
                        " CASE WHEN ( " +
                        " SELECT MAX(PLAN_SHIPPING_DATE) " +
                        " FROM SalesOrderloading " +
                        " INNER JOIN HMILoadDetails ON HMILoadDetails.LOAD_NO = SalesOrderloading.LOAD_NO " +
                        " WHERE HMILoadDetails.SALES_ORDER = H.SALES_ORDER " +
                        " AND HMILoadDetails.SALES_ORDER <> ' ') <> '00000000' " +
                        " THEN ( " +
                        " SELECT TOP 1 FORMAT( " +
                        " CAST(FORMAT(TRY_CAST(SalesOrderloading.PLAN_SHIPPING_DATE AS DATE), 'yyyy-MM-dd') + ' ' + FORMAT(TRY_CAST(SalesOrderloading.PLAN_SHIPPING_TIME AS TIME), 'HH:mm:ss') AS DATETIME), " +
                        " 'yyyy/MM/dd') " +
                        " FROM SalesOrderloading " +
                        " INNER JOIN HMILoadDetails ON HMILoadDetails.LOAD_NO = SalesOrderloading.LOAD_NO " +
                        " WHERE HMILoadDetails.SALES_ORDER = H.SALES_ORDER " +
                        " AND HMILoadDetails.SALES_ORDER <> ' ' " +
                        " ORDER BY SalesOrderloading.PLAN_SHIPPING_DATE DESC, SalesOrderloading.PLAN_SHIPPING_TIME DESC) " +
                        " END AS WT_DATE " +
                        " FROM OesOrderHeaderHMI H " +
                        " LEFT JOIN OESRequestDetailsHMI C ON C.ORDER_REQUEST_NO = H.ORDER_REQUEST_NO " +
                        " LEFT JOIN DeliveredOrderdetailsHMI D ON H.SALES_ORDER = D.SALES_ORDER " +
                        " LEFT JOIN HMILoadDetails HMLD ON H.SALES_ORDER = HMLD.SALES_ORDER";


                //lCmd.CommandText = "select DISTINCT H.NAME_AG as CUSTOMER, H.NAME_WE AS PROJECT, H.CONTRACT, "
                //+ " (select NAME1 from SAPSR3.KNA1 WHERE MANDT = '" + lProcessObj.strClient + "' AND KUNNR = (SELECT KUNN2 FROM SAPSR3.KNVP WHERE MANDT = '" + lProcessObj.strClient + "' AND PARVW = 'Z3' AND KUNNR = H.KUNNR)) AS PROJ_COORD, "
                //////+ " TO_CHAR(TO_DATE(H.REQD_DEL_DATE, 'YYYYMMDD'), 'YYYY - MM - DD') AS REQD_DEL_DATE_STR, TO_DATE(H.REQD_DEL_DATE, 'YYYYMMDD') AS REQD_DEL_DATE, "
                //+ " H.REQ_DAT_FRM, (case when H.REQ_DAT_FRM is not null and H.REQ_DAT_FRM <> ' ' then to_date(H.REQ_DAT_FRM, 'yyyy-mm-dd.hh24.mi.ss') end) as REQ_DAT_FRM_DATE, "
                ////+ " (case when H.REQ_DAT_FRM is not null and H.REQ_DAT_FRM <> ' ' then TO_CHAR(to_date(H.REQ_DAT_FRM, 'yyyy-mm-dd.hh24.mi.ss'), 'DD/MM/YYYY') end) as REQ_DAT_FRM_STR, "
                ////+ " (case when H.REQ_DAT_FRM is not null and H.REQ_DAT_FRM <> ' ' and H.REQ_DAT_FRM <> '00000000' then TO_CHAR(to_date(H.REQ_DAT_FRM, 'yyyy-mm-dd.hh24.mi.ss'), 'DD/MM/YYYY') end) as REQ_DAT_FRM_STR, " //Modified by aishwarya
                //+ " (case when H.REQ_DAT_FRM is not null and H.REQ_DAT_FRM <> ' ' and H.REQ_DAT_FRM <> '00000000' then TO_CHAR(to_date(H.REQ_DAT_FRM, 'yyyy-mm-dd.hh24.mi.ss'), 'YYYY/MM/DD') end) as REQ_DAT_FRM_STR, "
                //+ " H.REQ_DAT_TO, " ////(case when H.REQ_DAT_TO is not null and H.REQ_DAT_TO <> ' ' then to_date(H.REQ_DAT_TO, 'yyyy-mm-dd.hh24.mi.ss') end) as REQ_DAT_TO_DATE, "
                ////+ " (case when H.REQ_DAT_TO is not null and H.REQ_DAT_TO <> ' ' and H.REQ_DAT_TO <> '00000000' then TO_CHAR(to_date(H.REQ_DAT_TO, 'yyyy-mm-dd.hh24.mi.ss'), 'DD/MM/YYYY') end) as REQ_DAT_TO_STR, "
                //+ " (case when H.REQ_DAT_TO is not null and H.REQ_DAT_TO <> ' ' and H.REQ_DAT_TO <> '00000000' then TO_CHAR(to_date(H.REQ_DAT_TO, 'yyyy-mm-dd.hh24.mi.ss'), 'YYYY/MM/DD') end) as REQ_DAT_TO_STR, "
                ////+ " (case when H.REQ_DAT_TO is not null and H.REQ_DAT_TO <> ' ' then TO_CHAR(to_date(H.REQ_DAT_TO, 'yyyy-mm-dd.hh24.mi.ss'), 'DD/MM/YYYY') end) as REQ_DAT_TO_STR, "
                ////+ " (case when H.STATUS IN ('C','S','Z') THEN (select TO_CHAR(to_date(max(conf_del_date), 'yyyymmdd'), 'DD/MM/YYYY') from SAPSR3.YMSDT_ORDER_ITEM where order_request_no = H.ORDER_REQUEST_NO AND LENGTH(conf_del_date)=8 AND conf_del_date <> '00000000' AND conf_del_date NOT LIKE '%-%') END) AS CONF_DEL_DATE, "
                //+ " (case when H.STATUS IN ('C','S','Z') THEN (select TO_CHAR(to_date(max(conf_del_date), 'yyyymmdd'), 'YYYY/MM/DD') from SAPSR3.YMSDT_ORDER_ITEM where order_request_no = H.ORDER_REQUEST_NO AND LENGTH(conf_del_date)=8 AND conf_del_date <> '00000000' AND conf_del_date NOT LIKE '%-%') END) AS CONF_DEL_DATE, "
                //+ " H.ON_HOLD_IND, H.ZERO_TOLERANCE_I, H.CALL_BEF_DEL_IND, H.CONQUAS_IND, H.SPECIAL_PASS_IND, H.DO_NOT_MIX_IND, H.LORRY_CRANE_IND, H.URG_ORD_IND, "
                //+ " H.PRM_SVC_IND, H.CRN_BKD_IND, H.BRG_BKD_IND, H.POL_ESC_IND, H.WBS1 as HALL_ASSIGNMENT, H.LOW_BED_VEH_IND, H.TON50_VEH_IND, H.UNIT_MODE_IND, "
                //////+ " (case when trim(H.PRJ_CST_DAT) is not null then to_date(H.PRJ_CST_DAT, 'yyyymmdd') end) as PRJ_CST_DAT, "
                //////+ " (case when trim(H.PRJ_CST_DAT) is not null then TO_CHAR(to_date(H.PRJ_CST_DAT, 'yyyymmdd'),'DD/MM/YYYY') end) as PRJ_CST_DATE, "
                //////+ " (case when trim(H.MUST_HAVE_DAT) is not null then to_date(H.MUST_HAVE_DAT, 'yyyymmdd') end) as MUST_HAVE_DAT, "
                //////+ " (case when trim(H.MUST_HAVE_DAT) is not null then TO_CHAR(to_date(H.MUST_HAVE_DAT, 'yyyymmdd'),'DD/MM/YYYY') end) as MUST_HAVE_DATE, "
                ////////+ " H.SALES_ORDER, H.ORDER_REQUEST_NO AS SOR_NO, H.ORDER_GROUP_ID, H.PO_Number, IC.BBS_No, IC.LOAD_NO, H.Product_Type, H.Int_Remark, H.Ext_Remark, "
                //+ " H.SALES_ORDER, H.ORDER_REQUEST_NO AS SOR_NO, H.ORDER_GROUP_ID, H.PO_Number, "////H.Product_Type," 
                ////+ " (select Product_Type from SAPSR3.YMSDT_REQ_DETAIL where ORDER_REQUEST_NO = H.ORDER_REQUEST_NO) as Product_Type, " +
                //+ " C.Product_Type, " +
                //" H.Int_Remark, H.Ext_Remark, " + " C.ST_ELEMENT_TYPE, C.WBS1, C.WBS2, C.WBS3, C.BBS_No, " +
                ////+ " H.ST_ELEMENT_TYPE, H.WBS1, H.WBS2, H.WBS3, H.BBS_No, " +
                //////+ " (select ST_ELEMENT_TYPE from SAPSR3.YMSDT_REQ_DETAIL where ORDER_REQUEST_NO = H.ORDER_REQUEST_NO) as ST_ELEMENT_TYPE, "
                //////+ " (select WBS1 from SAPSR3.YMSDT_REQ_DETAIL where ORDER_REQUEST_NO = H.ORDER_REQUEST_NO) as WBS1, "
                //////+ " (select WBS2 from SAPSR3.YMSDT_REQ_DETAIL where ORDER_REQUEST_NO = H.ORDER_REQUEST_NO) as WBS2, "
                //////+ " (select WBS3 from SAPSR3.YMSDT_REQ_DETAIL where ORDER_REQUEST_NO = H.ORDER_REQUEST_NO) as WBS3, "
                //////+ " (select BBS_No from SAPSR3.YMSDT_REQ_DETAIL where ORDER_REQUEST_NO = H.ORDER_REQUEST_NO) as BBS_No, " +
                ////////+ " C.ST_ELEMENT_TYPE, C.WBS1, C.WBS2, C.WBS3, C.BBS_No, " +
                ////"(case when H.CUST_ORDER_DATE <> '00000000' AND H.CUST_ORDER_DATE <> ' ' then TO_CHAR(to_date(H.CUST_ORDER_DATE, 'YYYYMMDD'), 'DD/MM/YYYY') end) as PO_DATE, " //Modified by aishwarya
                // "(case when H.CUST_ORDER_DATE <> '00000000' AND H.CUST_ORDER_DATE <> ' ' then TO_CHAR(to_date(H.CUST_ORDER_DATE, 'YYYYMMDD'), 'YYYY/MM/DD') end) as PO_DATE, "
                ////"(case when H.CUST_ORDER_DATE <> '00000000' AND H.CUST_ORDER_DATE <> ' ' then TO_CHAR(to_date(H.CUST_ORDER_DATE, 'YYYYMMDD'), 'DD/MM/YYYY') end) as PO_DATE, "
                //////+ " H.UDATE, H.UTIME, "
                //+ " CASE WHEN H.UDATE = '00000000' THEN NULL ELSE TO_DATE(CONCAT(TO_CHAR(TO_DATE(H.UDATE, 'YYYYMMDD'), 'YYYY-MM-DD '), TO_CHAR(TO_DATE(H.UTIME, 'HH24MISS'), 'HH24:MI:SS')), 'YYYY-MM-DD HH24:MI:SS') END AS UDATETIME, H.STATUS, "
                //+ " CASE WHEN ( SELECT MAX(LOAD_DATE) FROM SAPSR3.YMPPT_LOAD_VEHIC WHERE MANDT = '" + lProcessObj.strClient + "' AND VBELN = H.SALES_ORDER "
                //+ " AND VBELN <> ' ') <> '00000000' THEN TO_CHAR(to_date((SELECT MAX(LOAD_DATE || WEIGH_OUT_TIME) FROM SAPSR3.YMPPT_LOAD_VEHIC "
                //+ " WHERE MANDT = '" + lProcessObj.strClient + "' "
                //+ " AND VBELN = H.SALES_ORDER AND VBELN <> ' '), 'yyyymmddhh24miss'), 'YYYY/MM/DD') "
                //+ " END AS WT_DATE,"
                //+ " H.ORDER_REQUEST_NO,H.STATUS AS SOR_STATUS, "
                //+ " (CASE WHEN product_marking <> ' ' THEN product_marking  ELSE H.USERID END ) AS USERID, "
                //+ " CASE (SELECT COUNT(*) FROM (SELECT VBELN,LFSTA FROM SAPSR3.VBUP WHERE ( "
                //+ " VBELN,POSNR) IN (SELECT VBELN ,POSNR FROM SAPSR3.VBAP WHERE UEPOS = '000000'  )) "
                //+ " WHERE LFSTA = 'C' AND VBELN = H.SALES_ORDER) WHEN (SELECT COUNT(*) FROM SAPSR3.VBAP WHERE UEPOS = '000000' "
                //+ " AND VBELN = H.SALES_ORDER HAVING COUNT(*) > 0 ) THEN 'Complete' WHEN 0 THEN CASE WHEN ( "
                //+ " SELECT COUNT(*) FROM (SELECT VBELN,LFSTA FROM SAPSR3.VBUP WHERE ( VBELN ,POSNR  ) IN ( SELECT VBELN ,POSNR "
                //+ " FROM SAPSR3.VBAP WHERE UEPOS = '000000' ) ) WHERE LFSTA = 'B' AND VBELN = H.SALES_ORDER ) = 0 THEN 'Pending' "
                //+ " ELSE 'Partial' END ELSE 'Partial' END AS DELIVERY_STATUS "
                //+ " , (SELECT to_char(Sum(Theo_Weight_kg) / 1000, '999,999,990.999') FROM SAPSR3.YMSDT_ORDER_ITEM OI WHERE MANDT = '" + lProcessObj.strClient + "'  AND OI.Order_Request_No = H.ORDER_REQUEST_NO AND hg_item_no ='000000' AND H.STATUS <> 'X') AS Total_Tonnage"
                //+ " , (SELECT Sum(No_Pieces) FROM  SAPSR3.YMSDT_ORDER_ITEM OI WHERE MANDT = '" + lProcessObj.strClient + "' AND OI.Order_Request_No = H.ORDER_REQUEST_NO AND H.STATUS <> 'X') AS LP_No_of_Pieces "
                ////+ " from SAPSR3.YMSDT_REQ_DETAIL C INNER JOIN SAPSR3.YMSDT_ORDER_HDR H ON C.ORDER_REQUEST_NO = H.ORDER_REQUEST_NO ";
                //+ " from SAPSR3.YMSDT_ORDER_HDR H LEFT JOIN SAPSR3.YMSDT_REQ_DETAIL C ON C.ORDER_REQUEST_NO = H.ORDER_REQUEST_NO  ";

                ////if (SearchOptions == "4" || SearchOptions == "7")

                //Updated HMI Code
                if (SearchOptions == "4")
                {
                    lCmd.CommandText = lCmd.CommandText + " INNER JOIN  HMILoadDetails HLD ON HLD.SALES_ORDER = H.SALES_ORDER ";
                }
                else if (!string.IsNullOrEmpty(SORNofrom))
                {
                    if (SO_SOR_Search_Range == "3")
                    {
                        lCmd.CommandText = lCmd.CommandText + " INNER JOIN  HMILoadDetails HLD ON HLD.SALES_ORDER = H.SALES_ORDER ";
                    }
                }
                else if (!string.IsNullOrEmpty(SORNoTo))
                {
                    if (SO_SOR_Search_Range == "3")
                    {
                        lCmd.CommandText = lCmd.CommandText + " INNER JOIN  HMILoadDetails HLD ON HLD.SALES_ORDER = H.SALES_ORDER ";
                    }
                }
                lCmd.CommandText = lCmd.CommandText + " where 1 = 1 "; //// AND H.NDS_SALE_ORDER = '' ";

                if (SearchProducts == "MTS")
                {
                    lCmd.CommandText = lCmd.CommandText + " AND H.Status LIKE  'Z'";
                }

                if (SearchProducts != "0" && SearchProducts != "MTS")
                {
                    lCmd.CommandText = lCmd.CommandText + " AND C.Product_Type LIKE  '" + SearchProducts + "'";
                }
                if (!string.IsNullOrEmpty(CustomerCode))
                {
                    lCmd.CommandText = lCmd.CommandText + " AND H.KUNAG =  '" + CustomerCode + "'";
                }
                if (!string.IsNullOrEmpty(ProjectCode))
                {
                    lCmd.CommandText = lCmd.CommandText + " AND H.PROJECT =  '" + ProjectCode + "'";
                }
                if (!string.IsNullOrEmpty(RDateFrom) || !string.IsNullOrEmpty(RDateTo))
                {
                    if (Req_PO_Date_Search_Range == "1")
                    {
                        lCmd.CommandText = lCmd.CommandText + " AND (FORMAT(TRY_CAST(H.REQD_DEL_DATE AS DATETIME), 'yyyy/MM/dd') BETWEEN FORMAT(TRY_CAST('" + RDateFrom.Replace("/", "") + "' AS DATETIME), 'yyyy/MM/dd') AND FORMAT(TRY_CAST('" + RDateTo.Replace("/", "") + "' AS DATETIME), 'yyyy/MM/dd')) ";
                    }
                    else if (Req_PO_Date_Search_Range == "2")
                    {
                        lCmd.CommandText = lCmd.CommandText + " AND (H.CUST_ORDER_DATE >= '" + RDateFrom.Replace("/", "") + "' AND  H.CUST_ORDER_DATE <= '" + RDateTo.Replace("/", "") + "') ";
                    }
                }
                if (!string.IsNullOrEmpty(Rev_required_Conf_date_from) || !string.IsNullOrEmpty(Rev_required_Conf_date_to))
                {
                    if (Rev_Req_Confirmed_Date_Search_Range == "1")
                    {
                        lCmd.CommandText = lCmd.CommandText + " AND (H.REQ_DAT_TO <> ' ' AND H.REQ_DAT_TO is not null  AND  H.REQ_DAT_TO  BETWEEN '" + Rev_required_Conf_date_from.Replace("/", "-") + ".00.00.00'  AND  '" + Rev_required_Conf_date_to.Replace("/", "-") + ".23.59.59') ";
                    }
                    else if (Rev_Req_Confirmed_Date_Search_Range == "2")
                    {
                        //lCmd.CommandText = lCmd.CommandText + " AND ((select to_date(max(conf_del_date), 'yyyymmdd') from SAPSR3.YMSDT_ORDER_ITEM where order_request_no = H.ORDER_REQUEST_NO AND LENGTH(conf_del_date)=8 AND conf_del_date <> '00000000' AND conf_del_date NOT LIKE '%-%')  BETWEEN TO_DATE('" + Rev_required_Conf_date_from.Replace("/", "") + "', 'YYYYMMDD') AND TO_DATE('" + Rev_required_Conf_date_to.Replace("/", "") + "', 'YYYYMMDD')) ";
                    }
                }
                if (!string.IsNullOrEmpty(SORNofrom))
                {
                    if (SO_SOR_Search_Range == "1")
                    {
                        lCmd.CommandText = lCmd.CommandText + " AND H.ORDER_REQUEST_NO >= '" + SORNofrom.Trim() + "' ";
                    }
                    else if (SO_SOR_Search_Range == "2")
                    {
                        lCmd.CommandText = lCmd.CommandText + " AND H.SALES_ORDER >= '" + SORNofrom.Trim() + "' ";
                    }
                    else if (SO_SOR_Search_Range == "3")
                    {
                        lCmd.CommandText = lCmd.CommandText + " AND HLD.LOAD_NO >= '" + SORNofrom.Trim() + "' ";
                    }
                }
                if (!string.IsNullOrEmpty(SORNoTo))
                {
                    if (SO_SOR_Search_Range == "1")
                    {
                        lCmd.CommandText = lCmd.CommandText + " AND H.ORDER_REQUEST_NO <= '" + SORNoTo.Trim() + "' ";
                    }
                    else if (SO_SOR_Search_Range == "2")
                    {
                        lCmd.CommandText = lCmd.CommandText + " AND H.SALES_ORDER <= '" + SORNoTo.Trim() + "' ";
                    }
                    else if (SO_SOR_Search_Range == "3")
                    {
                        lCmd.CommandText = lCmd.CommandText + " AND HLD.LOAD_NO <= '" + SORNoTo.Trim() + "' ";
                    }
                }

                if (!string.IsNullOrEmpty(WBS1))
                {
                    lCmd.CommandText = lCmd.CommandText + " AND C.WBS1 LIKE '%" + WBS1.ToUpper() + "%' ";
                }
                if (!string.IsNullOrEmpty(WBS2))
                {
                    lCmd.CommandText = lCmd.CommandText + " AND C.WBS2 LIKE '%" + WBS2.ToUpper() + "%' ";
                }
                if (!string.IsNullOrEmpty(WBS3))
                {
                    lCmd.CommandText = lCmd.CommandText + " AND C.WBS3 LIKE '%" + WBS3.ToUpper() + "%' ";
                }
                if (SearchOptions != "0")
                {
                    switch (SearchOptions)
                    {
                        case "1":   //// PO Number
                            lCmd.CommandText = lCmd.CommandText + " AND H.PO_NUMBER LIKE '%" + txtSearch.Trim() + "%' ";
                            break;
                        case "2": //// SO Number
                            lCmd.CommandText = lCmd.CommandText + " AND H.SALES_ORDER LIKE '%" + txtSearch.Trim() + "%' ";
                            break;
                        case "3":   //// SOR Number
                            lCmd.CommandText = lCmd.CommandText + " AND H.ORDER_REQUEST_NO LIKE  '%" + txtSearch.Trim() + "%' ";
                            break;
                        case "4": //// Load Number
                            lCmd.CommandText = lCmd.CommandText + " AND HLD.LOAD_NO LIKE '%" + txtSearch.Trim() + "%'";
                            //lCmd.CommandText = lCmd.CommandText + " AND (SELECT Sum(No_Pieces) FROM SAPSR3.YMSDT_ORDER_ITEM OI WHERE MANDT = '" + lProcessObj.strClient + "' AND OI.Order_Request_No = H.ORDER_REQUEST_NO AND H.STATUS <> 'X') = '" + txtSearch + "' ";
                            break;
                        //case "5":  //// Pile Number
                        //    if (dtSalesOrders != null && dtSalesOrders.Rows.Count > 0)
                        //    {
                        //        string SOs = "";
                        //        foreach (DataRow dr in dtSalesOrders.Rows)
                        //        {
                        //            SOs = SOs + "'" + dr[0] + "',";
                        //        }
                        //        lCmd.CommandText = lCmd.CommandText + " AND H.SALES_ORDER IN (" + SOs.Remove(SOs.Length - 1, 1) + ") ";
                        //    }
                        //    break;
                        //case "6":    //// Panel Number
                        //    if (dtSalesOrders != null && dtSalesOrders.Rows.Count > 0)
                        //    {
                        //        string SOs = "";
                        //        foreach (DataRow dr in dtSalesOrders.Rows)
                        //        {
                        //            SOs = SOs + "'" + dr[0] + "',";
                        //        }
                        //        lCmd.CommandText = lCmd.CommandText + " AND H.SALES_ORDER IN (" + SOs.Remove(SOs.Length - 1, 1) + ") ";
                        //    }
                        //    break;
                        case "7":    //// BBS Number
                            lCmd.CommandText = lCmd.CommandText + " AND C.BBS_NO LIKE '%" + txtSearch.Trim() + "%'";
                            break;
                        default:
                            lCmd.CommandText = lCmd.CommandText + " AND 1 = 1 ";
                            break;
                    }
                }
                if (!string.IsNullOrEmpty(SearchOptionsByDesignation) && SearchOptionsByDesignation != "0")
                {
                    switch (SearchOptionsByDesignation)
                    {
                        case "1":   //// Project Coordinator Name
                            //lCmd.CommandText = lCmd.CommandText + " AND (select NAME1 from SAPSR3.KNA1 WHERE MANDT = '" + lProcessObj.strClient + "'" +
                            //    " AND KUNNR = (SELECT KUNN2 FROM SAPSR3.KNVP WHERE MANDT = '" + lProcessObj.strClient + "' AND PARVW = 'Z3' AND KUNNR = H.KUNNR)) LIKE '%" + lSearchByDesignation + "%' ";
                            
                            lCmd.CommandText = lCmd.CommandText + " AND (SELECT PM.project_leader FROM HMIProjectMaster PM INNER JOIN HMIContractProject CP ON " +
                                "CP.PROJECT_CODE = PM.project_code  WHERE CP.PROJECT_CODE = H.KUNNR) LIKE '%" + lSearchByDesignation + "%' ";


                            break;
                        case "2": //// Segment Manager
                            lCmd.CommandText = lCmd.CommandText + " AND H.KUNNR LIKE '%" + lSearchByDesignation + "%' ";
                            break;
                        case "3":   //// Account Manager
                            lCmd.CommandText = lCmd.CommandText + " AND H.KUNNR LIKE  '%" + lSearchByDesignation + "%' ";
                            break;
                        default:
                            lCmd.CommandText = lCmd.CommandText + " AND 1 = 1 ";
                            break;
                    }
                }

                lCmd.CommandText = lCmd.CommandText + "  ORDER BY UDATETIME DESC ";
                lProcessObj.OpenNDSConnection(ref lndsCon);
                oda.SelectCommand = lCmd;
                lCmd.Connection = lndsCon;
                lCmd.CommandTimeout = 15000;
                oda.Fill(ds);
                lProcessObj.CloseNDSConnection(ref lndsCon);
                lProcessObj = null;
                lCmd = null;
                lndsCon = null;
                oda = null;
            }
            catch (Exception ex)
            {
            }
        }

        [HttpPost]
        [Route("/Amendment_Indicators")]
        public ActionResult Amendment_Indicators(AmendmentIndicatorsDto amendmentIndicatorsDto)
        {
            string strReturn = "";
            bool status = true;
            var lCmd = new OracleCommand();
            var lcisCon = new OracleConnection();
            var lProcessObj = new ProcessController();
            OracleConnection cnIDB = new OracleConnection();    //// 'SCM IDB
            OracleCommand cmdIDB = new OracleCommand(); ////'SCM IDB
            SqlConnection sqlIDB = new SqlConnection();
            var lNDSCmd = new SqlCommand();
            var lNDSCon = new SqlConnection();
            var oNDSda = new SqlDataAdapter();
            try
            {
                //string strSqlUpdateOrderChangeHistory = "";   //Update order change hisgory in DSS
                //lProcessObj.OpenCISConnection(ref lcisCon);
                lProcessObj.OpenIDBConnection(ref cnIDB);
                lProcessObj.OpenNDSConnection(ref lNDSCon);
                int iCounterOrderChangeHistory = 0;
                string[] SORNumbers = amendmentIndicatorsDto.SORNumbers_to_amend.Split(',');
                DateTime strCurrentTimeStamp = DateTime.Now;
                lCmd.Connection = lcisCon;
                lCmd.CommandTimeout = 15000;
                cmdIDB.Connection = cnIDB;
                cmdIDB.CommandTimeout = 15000;
                /////ORDER_REQUEST_NO === SONumber
                foreach (string SORNumber in SORNumbers)
                {
                    if (!string.IsNullOrEmpty(SORNumber) && SORNumber != "")
                    {
                        /////'Added for CHG0031726
                        /////'ymsdt_order_hdr @nshoracis
                        lNDSCmd.CommandText = "update ORderheaderhmi " ////& strDSSDBLink & _
                            + " SET URG_ORD_IND ='" + (amendmentIndicatorsDto.Chk_Urgent_Order ? "Y" : "N") + "', "
                            + " PRM_SVC_IND ='" + (amendmentIndicatorsDto.Chk_Premium_Service ? "Y" : "N") + "', "
                            + " CRN_BKD_IND ='" + (amendmentIndicatorsDto.Chk_Crane_Booked_Onsite ? "Y" : "N") + "', "
                            + " BRG_BKD_IND ='" + (amendmentIndicatorsDto.Chk_Barge_Booked ? "Y" : "N") + "', "
                            + " POL_ESC_IND ='" + (amendmentIndicatorsDto.Chk_Police_Escort_Service ? "Y" : "N") + "', "
                            + " ZERO_TOLERANCE_I ='" + (amendmentIndicatorsDto.Chk_Zero_Tolerance ? "Y" : "N") + "', "
                            + " CALL_BEF_DEL_IND ='" + (amendmentIndicatorsDto.Chk_Call_Before_Delivery ? "Y" : "N") + "', "
                            + " CONQUAS_IND ='" + (amendmentIndicatorsDto.Chk_Conquas_Order ? "Y" : "N") + "', "
                            + " SPECIAL_PASS_IND ='" + (amendmentIndicatorsDto.Chk_Special_Pass ? "Y" : "N") + "', "
                            + " DO_NOT_MIX_IND ='" + (amendmentIndicatorsDto.Chk_Do_Not_Mix ? "Y" : "N") + "', "
                            + " LORRY_CRANE_IND ='" + (amendmentIndicatorsDto.Chk_Lorry_Crane ? "Y" : "N") + "', "
                            + " LOW_BED_VEH_IND ='" + (amendmentIndicatorsDto.Chk_Low_Bed_Vehicle_Allowed ? "Y" : "N") + "', "
                            + " TON50_VEH_IND ='" + (amendmentIndicatorsDto.Chk_50_Ton_Vehicle_Allowed ? "Y" : "N") + "', "
                            + " ON_HOLD_IND ='" + (amendmentIndicatorsDto.Chk_Order_OnHold ? "Y" : "N") + "', "
                            + " UNIT_MODE_IND ='" + (amendmentIndicatorsDto.Chk_Unit_Mode ? "Y" : "N") + "' "
                            + ", UDATE = (SELECT TO_CHAR(SYSDATE, 'YYYYMMDD') FROM DUAL) "
                            + ", UTIME = (SELECT TO_CHAR(SYSDATE, 'HH24MISS') FROM DUAL) "
                            + " WHERE MANDT = '" + lProcessObj.strClient + "' AND ORDER_REQUEST_NO = '" + SORNumber + "'";
                        lNDSCmd.ExecuteNonQuery();
                        /////'START SCM IDB Amending Indicators for selected orders
                        cmdIDB.CommandText = "update order_header "
                            + " SET urgent_order_ind ='" + (amendmentIndicatorsDto.Chk_Urgent_Order ? "Y" : "N") + "', "
                            + " premium_service_ind ='" + (amendmentIndicatorsDto.Chk_Premium_Service ? "Y" : "N") + "', "
                            + " crane_booked_ind ='" + (amendmentIndicatorsDto.Chk_Crane_Booked_Onsite ? "Y" : "N") + "', "
                            + " barge_booked_ind ='" + (amendmentIndicatorsDto.Chk_Barge_Booked ? "Y" : "N") + "', "
                            + " police_escort_ind ='" + (amendmentIndicatorsDto.Chk_Police_Escort_Service ? "Y" : "N") + "', "
                            + " zero_tolerance_ind ='" + (amendmentIndicatorsDto.Chk_Zero_Tolerance ? "Y" : "N") + "', "
                            + " CALL_BEF_DEL_IND ='" + (amendmentIndicatorsDto.Chk_Call_Before_Delivery ? "Y" : "N") + "', "
                            + " CONQUAS_IND ='" + (amendmentIndicatorsDto.Chk_Conquas_Order ? "Y" : "N") + "', "
                            + " SPECIAL_PASS_IND ='" + (amendmentIndicatorsDto.Chk_Special_Pass ? "Y" : "N") + "', "
                            + " DO_NOT_MIX_IND ='" + (amendmentIndicatorsDto.Chk_Do_Not_Mix ? "Y" : "N") + "', "
                            + " needs_lorry_crane_ind ='" + (amendmentIndicatorsDto.Chk_Lorry_Crane ? "Y" : "N") + "', "
                            + " low_bed_vehicle_allowed ='" + (amendmentIndicatorsDto.Chk_Low_Bed_Vehicle_Allowed ? "Y" : "N") + "', "
                            + " fiftyton_vehicle_allowed ='" + (amendmentIndicatorsDto.Chk_50_Ton_Vehicle_Allowed ? "Y" : "N") + "', "
                            + " ON_HOLD_IND ='" + (amendmentIndicatorsDto.Chk_Order_OnHold ? "Y" : "N") + "', "
                            + " unit_tag_mode_ind ='" + (amendmentIndicatorsDto.Chk_Unit_Mode ? "Y" : "N") + "' "
                            + " where ORDER_REQUEST_NO ='" + SORNumber + "'";
                        cmdIDB.ExecuteNonQuery();
                        /////'END SCM IDB Amending Indicators for selected orders
                        iCounterOrderChangeHistory += 1;
                        /////'update SAP Y table,when on-hold reason with REJECT ORDER, system shall set reqd_del_date to 2060-01-01      
                        if (amendmentIndicatorsDto.Chk_Order_OnHold)
                        {
                            /////'ymsdt_order_ITEM @nshoracis
                            //lNDSCmd.CommandText = "update SAPSR3.YMSDT_ORDER_ITEM " ////& strDSSDBLink & _
                            //    + " SET CONF_DEL_DATE ='20500101'";
                            //if (amendmentIndicatorsDto.txtReason_Change_Order_OnHold.Trim() == "CUSTOMER - REJECT ORDER")
                            //{
                            //    lNDSCmd.CommandText = lNDSCmd.CommandText + ", reqd_del_date='20600101'";
                            //}
                            //lNDSCmd.CommandText = lNDSCmd.CommandText + " WHERE MANDT = '" + lProcessObj.strClient + "' AND ORDER_REQUEST_NO = '" + SORNumber + "'";
                            //lNDSCmd.ExecuteNonQuery();
                            /////'ymsdt_order_hdr @nshoracis
                            if (amendmentIndicatorsDto.txtReason_Change_Order_OnHold.Trim() == "CUSTOMER - REJECT ORDER")
                            {
                                lNDSCmd.CommandText = "update ORderheaderhmi " ////& strDSSDBLink & _                                                               
                                    + " SET reqd_del_date='20600101', REQ_DAT_FRM ='2060-01-01.08.00.00', REQ_DAT_TO ='2060-01-01.17.00.00'  "
                                    //+ ", UDATE = (SELECT TO_CHAR(TO_DATE(SYSDATE, 'YYYY-MM-DD HH24:MI:SS'), 'YYYYMMDD') FROM DUAL) "
                                    //+ ", UTIME = (SELECT TO_CHAR(TO_DATE(SYSDATE, 'YYYY-MM-DD HH24:MI:SS'), 'HH24MISS') FROM DUAL) "
                                    + " WHERE MANDT = '" + lProcessObj.strClient + "' AND ORDER_REQUEST_NO = '" + SORNumber + "'";
                                lNDSCmd.ExecuteNonQuery();
                            }
                            //kunal commented
                            /////'18 Dec 2011, reset confirmed_date_string in sales_order_planning in SCM IDB to MaxDateTime
                            cmdIDB.CommandText = "update sales_order_planning set confirmed_date_string ='MaxDateTime',"
                                                + " confirmation_remark='CONFIRMED DATE STR RESET TO MAXDATETIME BY INTERFACE' "
                                                + " where ORDER_REQUEST_NO ='" + SORNumber + "'";
                            cmdIDB.ExecuteNonQuery();
                            ////'18 Dec 2011, Reset confirmed_date_string in sales_order_planning in SCM IDB to MaxDateTime
                        }
                        /////'Update order change history
                        //strSqlUpdateOrderChangeHistory = "insert into SAPSR3.YMSDT_ORD_CLOG " +
                        //         "(och_timestamp_ch, och_sales_order, och_ord_item, och_par_chng,  och_old_value, och_new_value, och_chng_rsn, och_chng_userid)" +
                        //         " values ('" + strCurrentTimeStamp + "'," + "'" + SORNumber + "'," + "0," + "'PRJ CST DAT','"
                        //         + Project_Casting_Date_Change.Replace("-", "") + "'," + "'" + Project_Casting_Date_Change.Replace("-", "") + "'," + "'NA'," + "'DigiOS')";
                        //strSqlUpdateOrderChangeHistory = "insert into SAPSR3.YMSDT_ORD_CLOG " +
                        //         "(och_timestamp_ch, och_sales_order, och_ord_item, och_par_chng,  och_old_value, och_new_value, och_chng_rsn, och_chng_userid)" +
                        //         " values ('" + strCurrentTimeStamp + "'," + "'" + SORNumber + "'," + "0," + "' MUST HAVE DATE','"
                        //         + Must_Have_Date_Change.Replace("-", "") + "'," + "'" + Must_Have_Date_Change.Replace("-", "") + "'," + "'NA'," + "'DigiOS')";
                        //lCmd.CommandText = strSqlUpdateOrderChangeHistory;
                        //lCmd.ExecuteNonQuery();
                    }
                }
                lProcessObj.CloseIDBConnection(ref cnIDB);
                //lProcessObj.CloseCISConnection(ref lcisCon);
                lProcessObj.CloseNDSConnection(ref lNDSCon);
                strReturn = Convert.ToString("Indicators for " + iCounterOrderChangeHistory + " order(s) amended successfully.");
            }
            catch (Exception ex)
            {
                strReturn = "Error occurred while amending indicators, Please try again.!!!";
                status = false;
            }
            finally
            {
                cmdIDB = null;
                cnIDB = null;
                lCmd = null;
                lcisCon = null;
                lNDSCon = null;
            }
            //return Json(strReturn, JsonRequestBehavior.AllowGet); 
            var data = new { Message = strReturn, Status = status };
            return Ok(data);
        }

        [HttpPost]
        [Route("/Amendment_Required_Dates")]
        public ActionResult Amendment_Required_Dates([FromBody]AmendmentRequiredDateDto amendmentRequiredDateDto)
        {
            string strReturn = "";
            bool status = true;
            var lCmd = new OracleCommand();
            var lcisCon = new OracleConnection();
            var lProcessObj = new ProcessController();
            OracleConnection cnIDB = new OracleConnection();    //// 'SCM IDB
            OracleCommand cmdIDB = new OracleCommand(); ////'SCM IDB
            SqlConnection sqlIDB = new SqlConnection();
            var lNDSCmd = new SqlCommand();
            var lNDSCon = new SqlConnection();
            var oNDSda = new SqlDataAdapter();
            var lProcess = new ProcessController();

            string IsGreensteel = "";

                try
            {
                //// SAP Y table
                //// SCM IDB
                string strSqlUpdateOrderChangeHistory = "";
                //Update order change hisgory in DSS
                //string strSqlUpdateSCMIDB = "";              
                //Update SCM IDB. if this step fails, it will be sync by order status interface program
                //lProcessObj.OpenCISConnection(ref lcisCon);
                /////Dim myTransIDB As OracleClient.OracleTransaction
                lProcessObj.OpenIDBConnection(ref cnIDB);

                lProcess.OpenNDSConnection(ref lNDSCon);
                int iCounterOrderChangeHistory = 0;
                string[] SORNumbers = amendmentRequiredDateDto.SORNumbers_to_amend.Split(',');
                DateTime strCurrentTimeStamp = DateTime.Now;
                //string strOrderChangeTimestamp = "";
                lCmd.Connection = lcisCon;
                lCmd.CommandTimeout = 15000;
                cmdIDB.Connection = cnIDB;
                cmdIDB.CommandTimeout = 15000;

                lNDSCmd.Connection = lNDSCon;
                lNDSCmd.CommandTimeout = 15000;
              

                /////ORDER_REQUEST_NO === SONumber
                foreach (string SORNumber in SORNumbers)
                {
                    if (!string.IsNullOrEmpty(SORNumber) && SORNumber != "")
                    {
                        /////ymsdt_order_hdr @nshoracis
                        lNDSCmd.CommandText = "update OesOrderHeaderHMI SET ";////& strDSSDBLink & _
                        //if (!string.IsNullOrEmpty(amendmentRequiredDateDto.required_date_from.Trim()))
                        //{
                        //    ////lCmd.CommandText = lCmd.CommandText + " REQD_DEL_DATE ='" + required_date_from.Replace("-", "") + "', REQ_DAT_FRM = '" + required_date_from.Replace("-", "");
                        //    //lCmd.CommandText = lCmd.CommandText + " REQD_DEL_DATE ='" + required_date_from.Replace("/", "")

                        //    //lCmd.CommandText = lCmd.CommandText + " REQD_DEL_DATE ='" + required_date_from.Replace("/", "").Substring(4) + required_date_from.Replace("/", "").Substring(2, 2) + required_date_from.Replace("/", "").Substring(0, 2)
                        //    //    + "', REQ_DAT_FRM = '" + required_date_from.Replace("/", "").Substring(4) + "-" + required_date_from.Replace("/", "").Substring(2, 2) + "-" + required_date_from.Replace("/", "").Substring(0, 2) + ".08.00.00";

                        //    //lCmd.CommandText = lCmd.CommandText + " REQD_DEL_DATE ='" + required_date_from.Replace("/", "-")
                        //    //    + "', REQ_DAT_FRM = '" + required_date_from.Replace("/", "-") + ".08.00.00";

                        //    lNDSCmd.CommandText = lNDSCmd.CommandText + " REQD_DEL_DATE ='" + amendmentRequiredDateDto.required_date_from.Replace("/", "")
                        //        + "', REQ_DAT_FRM = '" + amendmentRequiredDateDto.required_date_from.Replace("/", "-") + ".08.00.00";
                        //}

                        //+ "', REQ_DAT_FRM = '" + required_date_from + ".08.00.00"
                        if (!string.IsNullOrEmpty(amendmentRequiredDateDto.required_date_to.Trim()))
                        {
                            //if (!string.IsNullOrEmpty(amendmentRequiredDateDto.required_date_from.Trim()))
                            //{
                            //    lNDSCmd.CommandText = lNDSCmd.CommandText + "', ";
                            //}
                            ////lCmd.CommandText = lCmd.CommandText + " REQ_DAT_To = '" + required_date_to.Replace("-", "");
                            //lCmd.CommandText = lCmd.CommandText + " REQ_DAT_To = '" + required_date_to + ".17.00.00";
                            //lCmd.CommandText = lCmd.CommandText + " REQ_DAT_To = '" + required_date_to.Replace("/", "").Substring(4) + "-" + required_date_to.Replace("/", "").Substring(2, 2) + "-" + required_date_to.Replace("/", "").Substring(0, 2) + ".17.00.00";
                            //lCmd.CommandText = lCmd.CommandText + " REQ_DAT_To = '" + required_date_to.Replace("/", "-") + ".17.00.00";
                            lNDSCmd.CommandText = lNDSCmd.CommandText + " REQ_DAT_To = '" + amendmentRequiredDateDto.required_date_to.Replace("/", "-") + ".17.00.00";
                        }
                        lNDSCmd.CommandText += "', " +
                          "UDATE = CONVERT(VARCHAR(8), GETDATE(), 112), " +  // Format: YYYYMMDD
                          "UTIME = REPLACE(CONVERT(VARCHAR(8), GETDATE(), 108), ':', '') " +  // Format: HHMMSS
                          "WHERE MANDT = '" + lProcessObj.strClient + "' AND ORDER_REQUEST_NO = '" + SORNumber + "'";

                        lNDSCmd.ExecuteNonQuery();  //To change- AIshwarya


                        // Update Date in NDS Tables 
                        if (!string.IsNullOrEmpty(amendmentRequiredDateDto.required_date_to.Trim()))
                        {
                            lNDSCmd.CommandText = " UPDATE OESProjOrder SET RequiredDate=TRY_CONVERT(DATETIME, '" + amendmentRequiredDateDto.required_date_to + "', 111) " +
                                "WHERE OrderNumber IN ( SELECT ODOS_ID FROM OesOrderHeaderHMI WHERE MANDT='" + lProcessObj.strClient + "' AND ORDER_REQUEST_NO = '" + SORNumber + "' ) ";
                            lNDSCmd.ExecuteNonQuery();


                            lNDSCmd.CommandText = " UPDATE OESProjOrdersSE SET RequiredDate=TRY_CONVERT(DATETIME, '" + amendmentRequiredDateDto.required_date_to + "', 111) " +
                               "WHERE OrderNumber IN ( SELECT ODOS_ID FROM OesOrderHeaderHMI WHERE MANDT='" + lProcessObj.strClient + "' AND ORDER_REQUEST_NO = '" + SORNumber + "' ) ";
                            lNDSCmd.ExecuteNonQuery();
                        }



                        /////'ymsdt_order_item @nshoracis
                        ////'' ''For checking
                        ////''cmdCIS.CommandText = "update " & strTableName_YMSDT_ORDER_ITEM & "  "
                        ////'also update conf_del_date field in order item table
                        ////' as requested by Soo Loke by email on 16 Dec 2012
                        //if (!string.IsNullOrEmpty(amendmentRequiredDateDto.required_date_from.Trim()))
                        //{
                        //    lCmd.CommandText = "update SAPSR3.YMSDT_ORDER_ITEM" ////& strDSSDBLink & _
                        //    ////+ " SET REQD_DEL_DATE ='" + required_date_from.Replace("-", "")
                        //    //+ " SET REQD_DEL_DATE ='" + required_date_from.Replace("/", "")
                        //    //+ " SET REQD_DEL_DATE ='" + required_date_from.Replace("-", "").Substring(4) + required_date_from.Replace("/", "").Substring(2, 2) + required_date_from.Replace("/", "").Substring(0, 2)
                        //    + " SET REQD_DEL_DATE ='" + amendmentRequiredDateDto.required_date_from.Replace("/", "")
                        //    ////+ "', CONF_DEL_DATE = '" + required_date_from.Replace("-", "")   /// Commented on 06/11/2019 for Confirmed delivery date should not be updated
                        //    + "' WHERE MANDT = '" + lProcessObj.strClient + "' AND ORDER_REQUEST_NO = '" + SORNumber + "'";
                        //    lCmd.ExecuteNonQuery();  //To change- AIshwarya
                        //    //////'update SCM IDB                       
                        //    ////cmdIDB.CommandText = "update order_header set req_del_date_fr = '" + required_date_from.Replace("-", "")
                        //    //cmdIDB.CommandText = "update order_header set req_del_date_fr = '" + required_date_from
                        //    //        + "' where ORDER_REQUEST_NO ='" + SORNumber + "'";
                        //    //cmdIDB.ExecuteNonQuery();
                        //}
                        //////'update SCM IDB 
                        cmdIDB.CommandText = "update order_header set ";
                        //if (!string.IsNullOrEmpty(amendmentRequiredDateDto.required_date_from.Trim()))
                        //{
                        //    //cmdIDB.CommandText = cmdIDB.CommandText + " req_del_date_fr ='" + required_date_from + ".08.00.00";
                        //    // cmdIDB.CommandText = cmdIDB.CommandText + " req_del_date_fr ='" + required_date_from.Replace("/", "").Substring(4) + "-" + required_date_from.Replace("/", "").Substring(2, 2) + "-" + required_date_from.Replace("/", "").Substring(0, 2) + ".08.00.00";
                        //    cmdIDB.CommandText = cmdIDB.CommandText + " req_del_date_fr ='" + amendmentRequiredDateDto.required_date_from.Replace("/", "-") + ".08.00.00";

                        //}
                        if (!string.IsNullOrEmpty(amendmentRequiredDateDto.required_date_to.Trim()))
                        {
                            //if (!string.IsNullOrEmpty(amendmentRequiredDateDto.required_date_from.Trim()))
                            //{
                            //    cmdIDB.CommandText = cmdIDB.CommandText + "', ";
                            //}
                            //cmdIDB.CommandText = cmdIDB.CommandText + " REQ_DEL_DATE_TO = '" + required_date_to + ".17.00.00";
                            // cmdIDB.CommandText = cmdIDB.CommandText + " REQ_DEL_DATE_TO = '" + required_date_to.Replace("/", "").Substring(4) + "-" + required_date_to.Replace("/", "").Substring(2, 2) + "-" + required_date_to.Replace("/", "").Substring(0, 2) + ".17.00.00";

                            cmdIDB.CommandText = cmdIDB.CommandText + " REQ_DEL_DATE_TO = '" + amendmentRequiredDateDto.required_date_to.Replace("/", "-") + ".17.00.00";
                        }
                        cmdIDB.CommandText = cmdIDB.CommandText + "' WHERE ORDER_REQUEST_NO = '" + SORNumber + "'";
                        cmdIDB.ExecuteNonQuery();//To change- AIshwarya
                        //// Start Commented on 06/11/2019 for Confirmed delivery date should not be updated
                        ///////'18 Dec 2011, reset confirmed_date_string in sales_order_planning in SCM IDB to MaxDateTime
                        //cmdIDB.CommandText = "update sales_order_planning set confirmed_date_string ='MaxDateTime',"
                        //                    + " confirmation_remark='CONFIRMED DATE STR RESET TO MAXDATETIME BY INTERFACE' "
                        //                    + " where ORDER_REQUEST_NO ='" + SORNumber + "'";
                        //cmdIDB.ExecuteNonQuery();
                        //////'18 Dec 2011, Reset confirmed_date_string in sales_order_planning in SCM IDB to MaxDateTime
                        //// end Commented on 06/11/2019 for Confirmed delivery date should not be updated
                        ////'update order change history table
                        iCounterOrderChangeHistory += 1;
                        //strOrderChangeTimestamp = strCurrentTimeStamp.ToString().Substring(0, 20);// + (900000 + iCounterOrderChangeHistory);
                        if (!string.IsNullOrEmpty(amendmentRequiredDateDto.required_date_from.Trim()) && !string.IsNullOrEmpty(amendmentRequiredDateDto.required_date_to.Trim()))
                        {
                            string[] splitArr = amendmentRequiredDateDto.required_date_from.Split('/');
                            string converteddateFrom = splitArr[2] + "/" + splitArr[1] + "/" + splitArr[0];
                            string[] splitArr1 = amendmentRequiredDateDto.required_date_to.Split('/');
                            string converteddateTo = splitArr1[2] + "/" + splitArr1[1] + "/" + splitArr1[0];
                            strSqlUpdateOrderChangeHistory = "insert into OrderChangeHistoryHMI " +
                               "(och_timestamp_ch, och_sales_order, och_ord_item, och_par_chng,  och_old_value, och_new_value, och_chng_rsn, och_chng_userid)" +
                               " values ('" + strCurrentTimeStamp + "', '" + SORNumber + "', 0, 'REQ DATE FR','" + converteddateFrom + ".08.00.00"//.Replace("-", "")
                                                                                                                                                  //+ "', '" + required_date_to.Replace("-", "") + "', '" + Reason_Change_Req_date + "', 'DigiOS')";
                               + "', '" + converteddateTo + "', '" + amendmentRequiredDateDto.Reason_Change_Req_date + "', 'DigiOS')";
                        }
                        else if (!string.IsNullOrEmpty(amendmentRequiredDateDto.required_date_from.Trim()))
                        {
                            string[] splitArr = amendmentRequiredDateDto.required_date_from.Split('/');
                            string converteddateFrom = splitArr[2] + "/" + splitArr[1] + "/" + splitArr[0];

                            strSqlUpdateOrderChangeHistory = "insert into OrderChangeHistoryHMI " +
                                "(och_timestamp_ch, och_sales_order, och_ord_item, och_par_chng,  och_old_value, och_new_value, och_chng_rsn, och_chng_userid)" +
                                " values ('" + strCurrentTimeStamp + "', '" + SORNumber + "', 0, 'REQ DATE FR','" + converteddateFrom + ".08.00.00"//.Replace("-", "")
                                                                                                                                                   //+ "', '" + required_date_from.Replace("-", "") + "', '" + Reason_Change_Req_date + "', 'DigiOS')";
                                + "', '" + converteddateFrom + "', '" + amendmentRequiredDateDto.Reason_Change_Req_date + "', 'DigiOS')";
                        }
                        else if (!string.IsNullOrEmpty(amendmentRequiredDateDto.required_date_to.Trim()))
                        {
                            string[] splitArr1 = amendmentRequiredDateDto.required_date_to.Split('/');
                            string converteddateTo = splitArr1[2] + "/" + splitArr1[1] + "/" + splitArr1[0];
                            strSqlUpdateOrderChangeHistory = "insert into OrderChangeHistoryHMI " +
                                "(och_timestamp_ch, och_sales_order, och_ord_item, och_par_chng,  och_old_value, och_new_value, och_chng_rsn, och_chng_userid)" +
                                " values ('" + strCurrentTimeStamp + "', '" + SORNumber + "', 0, 'REQ DATE FR','" + converteddateTo + ".17.00.00"//.Replace("-", "")
                                                                                                                                                 //+ "', '" + required_date_to.Replace("-", "") + "', '" + Reason_Change_Req_date + "', 'DigiOS')";
                                + "', '" + converteddateTo + "', '" + amendmentRequiredDateDto.Reason_Change_Req_date + "', 'DigiOS')";
                        }
                        lNDSCmd.CommandText = strSqlUpdateOrderChangeHistory;
                        lNDSCmd.ExecuteNonQuery(); //To change- AIshwarya
                    }
                }
                lProcessObj.CloseIDBConnection(ref cnIDB);
                //lProcessObj.CloseCISConnection(ref lcisCon); 
                lProcessObj.CloseNDSConnection(ref lNDSCon);
                strReturn = Convert.ToString("Required date(s) for " + iCounterOrderChangeHistory + " Order(s) amended successfully.");
            }
            catch (Exception ex)
            {
                strReturn = "Error occurred while amending required date(s), Please try again.!!!";
                status = false;
            }
            finally
            {
                cmdIDB = null;
                cnIDB = null;
                lCmd = null;
                lcisCon = null;
                lNDSCon = null;
            }
            // //return Json(strReturn, JsonRequestBehavior.AllowGet);  //commented by vidya
            var data = new { Message = strReturn, Status = status };
            return Ok(data);
        }

        //Export order Status to Excal

        [HttpPost]
        //[ValidateAntiForgeryHeader]
        [Route("/ExportOrderStatusToExcel")]
        public ActionResult ExportOrderStatusToExcel(string CustomerCode, string ProjectCode, string RDateFrom, string RDateTo, string SORNofrom, string SORNoTo, string WBS1, string WBS2, string WBS3, string SearchOptions, string txtSearch, string SO_SOR_Search_Range, string Req_PO_Date_Search_Range, string Rev_required_Conf_date_from, string Rev_required_Conf_date_to, string Rev_Req_Confirmed_Date_Search_Range, string SearchProducts, string SearchOptionsByDesignation, string lSearchByDesignation)
        {
            #region Default Settings
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
            if (RDateFrom == null) RDateFrom = "";
            if (RDateFrom.Trim().Length == 0)
            {
                RDateFrom = "2000-01-01";
            }
            if (RDateTo == null) RDateTo = "";
            if (RDateTo.Trim().Length == 0)
            {
                RDateTo = "2200-01-01";
            }
            if (!string.IsNullOrEmpty(Rev_required_Conf_date_from) || !string.IsNullOrEmpty(Rev_required_Conf_date_to))
            {
                if (Rev_required_Conf_date_from == null) Rev_required_Conf_date_from = "";
                if (Rev_required_Conf_date_from.Trim().Length == 0)
                {
                    Rev_required_Conf_date_from = "2000-01-01";
                }
                if (Rev_required_Conf_date_to == null) Rev_required_Conf_date_to = "";
                if (Rev_required_Conf_date_to.Trim().Length == 0)
                {
                    Rev_required_Conf_date_to = "2200-01-01";
                }
            }
            #endregion Default Settings
            #region Excel Settings
            ExcelPackage package = new ExcelPackage();
            ExcelWorksheet ws = package.Workbook.Worksheets.Add("Order Status");
            var lCompanyName = "";
            var lProjectTitle = "";
            try
            {
                if (!string.IsNullOrEmpty(CustomerCode))
                {
                    lCompanyName = db.Customer.Find(CustomerCode).CustomerName;
                }
                if (!string.IsNullOrEmpty(ProjectCode))
                {
                    lProjectTitle = db.Project.Find(CustomerCode, ProjectCode).ProjectTitle;
                }
            }
            catch (Exception ex)
            {
            }
            int lRowNo = 0;
            ws.Column(1).Width = 5;         //"SNo\n序号";
            ws.Column(3).Width = 15;        //"SOR No";
            ws.Column(2).Width = 15;        //"SO No";
            ws.Column(7).Width = 15;        //"PO No\n加工表号";
            ws.Column(16).Width = 10;       //"BBS No";
            ws.Column(35).Width = 17;       // "Total Tonnage"
            ws.Column(13).Width = 20;       //"Internal Remark"
            ws.Column(8).Width = 17;        //"PO Date\n订货日期";
            ws.Column(10).Width = 17;       //"Required Date\n到场日期";
            ws.Column(11).Width = 17;       //"Rev Req Date"
            ws.Column(9).Width = 17;        //"Confirmed Date"
            ws.Column(17).Width = 10;       //"WBS1\n楼座";
            ws.Column(18).Width = 10;       //"WBS2\n楼座";
            ws.Column(19).Width = 10;       //"WBS3\n楼座";
            ws.Column(42).Width = 17;       // LP_No_of_Pieces
            ws.Column(38).Width = 17;       // "WT_DATE";
            ws.Column(41).Width = 17;       // "DELIVERY_STATUS
            ws.Column(14).Width = 14;       //"Prod Type\n加工表号";
            ws.Column(15).Width = 14;       //"Structure Element";
            ws.Column(4).Width = 15;        //"Contract No";
            ws.Column(5).Width = 25;        //"Customer";
            ws.Column(6).Width = 25;        //"Project";
            ws.Column(12).Width = 20;       //"External Remark"
            ws.Column(20).Width = 16;       //"Proj Coord";
            ws.Column(40).Width = 17;       // "USERID"
            ws.Column(39).Width = 17;       // "SOR_STATUS"
            ws.Column(37).Width = 15;       //"Group ID";
            ws.Column(26).Width = 10;       //"On Hold";

            ws.Column(21).Width = 10;       //"Urgent Order";
            ws.Column(22).Width = 10;       //"Premium Service";
            ws.Column(23).Width = 10;       //"Crane Booked";
            ws.Column(24).Width = 10;       //"Barge Booked";
            ws.Column(25).Width = 10;       //"Police Escort";
            ws.Column(27).Width = 10;       //"Zero Tolerance";
            ws.Column(28).Width = 10;       //"Call Before Delivery";
            ws.Column(29).Width = 10;       //"Conquas Order";
            ws.Column(30).Width = 10;       //"Special Pass";
            ws.Column(31).Width = 10;       //"Do Not Mix";
            ws.Column(32).Width = 10;       //"Lorry Crane";
            ws.Column(33).Width = 10;       //"Low Bed Vehicle Allowed";
            ws.Column(34).Width = 10;       //"50 Ton Vehicle Allowed";
            ws.Column(36).Width = 15;       //"Unit Mode";
                                            //ws.Column(35).Width = 17;   //"Project Casting Date";
                                            //ws.Column(36).Width = 17;   //"Must Have Date";

            ws.Cells["A1:P1"].Merge = true;
            ws.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells[1, 1].Style.Font.Bold = true;
            ws.Cells[1, 1].Style.Font.Size = 12;
            ws.Cells[1, 1].Style.WrapText = true;
            ws.Cells[1, 1].Value = "Multiple Order Amendment";
            ws.Row(1).Height = 30;
            ws.Cells["A2:C2"].Merge = true;
            ws.Cells[2, 1].Value = "Company Name :";
            ws.Cells[2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            ws.Cells[2, 1].Style.Font.Bold = true;
            ws.Cells["D2:P2"].Merge = true;
            ws.Cells[2, 4].Value = lCompanyName;
            ws.Cells[2, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            ws.Cells[2, 4].Style.Font.Bold = true;
            ws.Cells["A3:C3"].Merge = true;
            ws.Cells[3, 1].Value = "Project Title :";
            ws.Cells[3, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            ws.Cells[3, 1].Style.Font.Bold = true;
            ws.Cells["D3:P3"].Merge = true;
            ws.Cells[3, 4].Value = lProjectTitle;
            ws.Cells[3, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            ws.Cells[3, 4].Style.Font.Bold = true;
            lRowNo = 4;
            ws.Row(4).Height = 30;
            ws.Cells[lRowNo, 1].Value = "SNo";               //"SNo\n序号";
            ws.Cells[lRowNo, 2].Value = "SOR No";             //"SOR No";
            ws.Cells[lRowNo, 3].Value = "SO No";              //"SO No";
            ws.Cells[lRowNo, 4].Value = "PO No";               //"PO No\n加工表号";
            ws.Cells[lRowNo, 5].Value = "BBS No";                  //"BBS No";
            ws.Cells[lRowNo, 6].Value = "Total Tonnage";     // Total_Tonnage   
            ws.Cells[lRowNo, 7].Value = "Internal Remark";     //"Internal Remark"
            ws.Cells[lRowNo, 8].Value = "PO Date";             //"PO Date\n订货日期";
            ws.Cells[lRowNo, 9].Value = "Required Date";      //"Required Date\n到场日期";
            ws.Cells[lRowNo, 10].Value = "Rev Req Date";        //"Rev Req Date";
            ws.Cells[lRowNo, 11].Value = "Confirmed Date";      //"Confirmed Date"
            ws.Cells[lRowNo, 12].Value = "WBS1";              //"WBS1\n楼座";
            ws.Cells[lRowNo, 13].Value = "WBS2";               //"WBS2\n楼座";
            ws.Cells[lRowNo, 14].Value = "WBS3";               //"WBS3\n楼座";
            ws.Cells[lRowNo, 15].Value = "LP No of Pieces";        // LP_No_of_Pieces
            ws.Cells[lRowNo, 16].Value = "WT Date";                //"WT_DATE";
            ws.Cells[lRowNo, 17].Value = "Delivery Status";        //"DELIVERY_STATUS
            ws.Cells[lRowNo, 18].Value = "Prod Type";         //"Prod Type\n加工表号";
            ws.Cells[lRowNo, 19].Value = "Structure Element";       //"Structure Element";
            ws.Cells[lRowNo, 20].Value = "Contract No";        //"Contract No";
            ws.Cells[lRowNo, 21].Value = "Customer";           //"Customer";
            ws.Cells[lRowNo, 22].Value = "Project";            //"Project";
            ws.Cells[lRowNo, 23].Value = "External Remark";     //"External Remark"
            ws.Cells[lRowNo, 24].Value = "Proj Coord";               //"Proj Coord";
            ws.Cells[lRowNo, 25].Value = "User ID";                //"USERID"
            ws.Cells[lRowNo, 26].Value = "SOR Staus";              //"SOR_STATUS"

            ws.Cells[lRowNo, 27].Value = "Group ID";                //"Group ID";
            ws.Cells[lRowNo, 28].Value = "On Hold";              //"On Hold";
            ws.Cells[lRowNo, 29].Value = "Urgent Order";             //"Urgent Order";
            ws.Cells[lRowNo, 30].Value = "Premium Service";          //"Premium Service";
            ws.Cells[lRowNo, 31].Value = "Crane Booked";             //"Crane Booked";
            ws.Cells[lRowNo, 32].Value = "Barge Booked";             //"Barge Booked";
            ws.Cells[lRowNo, 33].Value = "Police Escort";        //"Police Escort";
            ws.Cells[lRowNo, 34].Value = "Zero Tolerance";      //"Zero Tolerance";
            ws.Cells[lRowNo, 35].Value = "Call Before Delivery";    //"Call Before Delivery";
            ws.Cells[lRowNo, 36].Value = "Conquas Order";           //"Conquas Order";
            ws.Cells[lRowNo, 37].Value = "Special Pass";            //"Special Pass";
            ws.Cells[lRowNo, 38].Value = "Do Not Mix";              //"Do Not Mix";
            ws.Cells[lRowNo, 39].Value = "Lorry Crane";             //"Lorry Crane";
            ws.Cells[lRowNo, 40].Value = "Low Bed Vehicle Allowed"; //"Low Bed Vehicle Allowed";
            ws.Cells[lRowNo, 41].Value = "50 Ton Vehicle Allowed";  //"50 Ton Vehicle Allowed";
            ws.Cells[lRowNo, 42].Value = "Unit Mode";               //"Unit Mode";
            //ws.Cells[lRowNo, 35].Value = "Project Casting Date";    //"Project Casting Date";
            //ws.Cells[lRowNo, 36].Value = "Must Have Date";          //"Must Have Date";

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
            ws.Cells[lRowNo, 14].Style.WrapText = true;
            ws.Cells[lRowNo, 15].Style.WrapText = true;
            ws.Cells[lRowNo, 16].Style.WrapText = true;
            ws.Cells[lRowNo, 17].Style.WrapText = true;
            ws.Cells[lRowNo, 18].Style.WrapText = true;
            ws.Cells[lRowNo, 19].Style.WrapText = true;
            ws.Cells[lRowNo, 20].Style.WrapText = true;
            ws.Cells[lRowNo, 21].Style.WrapText = true;
            ws.Cells[lRowNo, 22].Style.WrapText = true;
            ws.Cells[lRowNo, 23].Style.WrapText = true;
            ws.Cells[lRowNo, 24].Style.WrapText = true;
            ws.Cells[lRowNo, 25].Style.WrapText = true;
            ws.Cells[lRowNo, 26].Style.WrapText = true;
            ws.Cells[lRowNo, 27].Style.WrapText = true;
            ws.Cells[lRowNo, 28].Style.WrapText = true;
            ws.Cells[lRowNo, 29].Style.WrapText = true;
            ws.Cells[lRowNo, 30].Style.WrapText = true;
            ws.Cells[lRowNo, 31].Style.WrapText = true;
            ws.Cells[lRowNo, 32].Style.WrapText = true;
            ws.Cells[lRowNo, 33].Style.WrapText = true;
            ws.Cells[lRowNo, 34].Style.WrapText = true;
            //ws.Cells[lRowNo, 35].Style.WrapText = true;
            ws.Cells[lRowNo, 35].Style.WrapText = true;
            //ws.Cells[lRowNo, 36].Style.WrapText = true;
            ws.Cells[lRowNo, 36].Style.WrapText = true;
            ws.Cells[lRowNo, 37].Style.WrapText = true;
            ws.Cells[lRowNo, 38].Style.WrapText = true;
            ws.Cells[lRowNo, 39].Style.WrapText = true;
            ws.Cells[lRowNo, 40].Style.WrapText = true;
            ws.Cells[lRowNo, 41].Style.WrapText = true;
            ws.Cells[lRowNo, 42].Style.WrapText = true;

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
            ws.Cells[lRowNo, 14].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[lRowNo, 15].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[lRowNo, 16].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[lRowNo, 17].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[lRowNo, 18].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[lRowNo, 19].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[lRowNo, 20].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[lRowNo, 21].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[lRowNo, 22].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[lRowNo, 23].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[lRowNo, 24].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[lRowNo, 25].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[lRowNo, 26].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[lRowNo, 27].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[lRowNo, 28].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[lRowNo, 29].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[lRowNo, 30].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[lRowNo, 31].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[lRowNo, 32].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[lRowNo, 33].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[lRowNo, 34].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //ws.Cells[lRowNo, 35].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[lRowNo, 35].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            //ws.Cells[lRowNo, 36].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[lRowNo, 36].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[lRowNo, 37].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[lRowNo, 38].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[lRowNo, 39].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[lRowNo, 40].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[lRowNo, 41].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[lRowNo, 42].Style.Border.BorderAround(ExcelBorderStyle.Thin);

            #endregion Excel Settings
            lRowNo = 5;
            DataSet ds = new DataSet();
            Getdata(CustomerCode, ProjectCode, ref RDateFrom, ref RDateTo, SORNofrom, SORNoTo, WBS1, WBS2, WBS3, SearchOptions, txtSearch, SO_SOR_Search_Range, Req_PO_Date_Search_Range, ref Rev_required_Conf_date_from, ref Rev_required_Conf_date_to, Rev_Req_Confirmed_Date_Search_Range, SearchProducts, ds, SearchOptionsByDesignation, lSearchByDesignation);
            var j = 0;

            //var lCmd = new OracleCommand();
            //var lcisCon = new OracleConnection();
            //OracleDataAdapter oda = new OracleDataAdapter();

            var lCmd = new SqlCommand();
            var lndsCon = new SqlConnection();
            SqlDataAdapter oda = new SqlDataAdapter();

            var lProcessObj = new ProcessController();
            DataSet dsLoads = new DataSet();
            DataSet dsLPPieces = new DataSet();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                String strLPPieces = string.Empty;
                //try
                //{
                //    dsLoads = new DataSet();
                //    lCmd.CommandText = "select distinct load_no  FROM SAPSR3.YMPPT_LP_BATCH_C  WHERE MANDT='" + lProcessObj.strClient + "'"
                //        + " AND sales_order = '" + Convert.ToString(dr["SALES_ORDER"]) + "' AND status<> 'X'";
                //    lProcessObj.OpenCISConnection(ref lcisCon);
                //    oda.SelectCommand = lCmd;
                //    lCmd.Connection = lcisCon;
                //    lCmd.CommandTimeout = 15000;
                //    oda.Fill(dsLoads);
                //    lProcessObj.CloseCISConnection(ref lcisCon);

                //    if (dsLoads != null && dsLoads.Tables.Count > 0)
                //    {
                //        dsLPPieces = new DataSet();
                //        foreach (DataRow drLoads in dsLoads.Tables[0].Rows)
                //        {
                //            lCmd.CommandText = "select sum(b.no_pieces) as deli_pcs from SAPSR3.YMPPT_LOAD_VEHIC a,SAPSR3.YMPPT_LP_BATCH_C b WHERE b.MANDT='" + lProcessObj.strClient + "'"
                //                + " AND b.sales_order = '" + Convert.ToString(dr["SALES_ORDER"]) + "' AND b.load_no = '" + Convert.ToString(drLoads["Load_no"]) + "'"
                //                + " AND b.status <> 'X' and a.vbeln = b.sales_order and a.POSNR = b.order_item and a.load_no = b.load_no "
                //                + " and a.tallied_batch = b.tallied_batch  and a.delivery_no = b.delivery_no "
                //                + " group by a.vbeln,a.load_no order by a.vbeln,a.load_no";
                //            lProcessObj.OpenCISConnection(ref lcisCon);
                //            oda.SelectCommand = lCmd;
                //            lCmd.Connection = lcisCon;
                //            lCmd.CommandTimeout = 15000;
                //            oda.Fill(dsLPPieces);
                //            lProcessObj.CloseCISConnection(ref lcisCon);
                //            if (dsLPPieces != null && dsLPPieces.Tables.Count > 0)
                //            {
                //                if (dsLPPieces.Tables[0].Rows.Count > 0)
                //                {
                //                    strLPPieces = strLPPieces + " " + Convert.ToString(drLoads["Load_no"]) + "(" + Convert.ToString(dsLPPieces.Tables[0].Rows[0][0]) + "),";
                //                }
                //            }
                //        }
                //    }
                //}
                try
                {
                    dsLoads = new DataSet();

                    // Step 1: Fetch distinct load numbers
                    lCmd.CommandText = "SELECT DISTINCT LOAD_NO FROM HMILoadDetails WHERE SALES_ORDER = '" + Convert.ToString(dr["SALES_ORDER"]) + "'";
                    lProcessObj.OpenNDSConnection(ref lndsCon);
                    oda.SelectCommand = lCmd;
                    lCmd.Connection = lndsCon;
                    lCmd.CommandTimeout = 15000;
                    oda.Fill(dsLoads);
                    lProcessObj.CloseNDSConnection(ref lndsCon);

                    // Step 2: Process each load to get delivered pieces
                    if (dsLoads != null && dsLoads.Tables.Count > 0)
                    {
                        dsLPPieces = new DataSet();

                        foreach (DataRow drLoads in dsLoads.Tables[0].Rows)
                        {
                            string salesOrder = Convert.ToString(dr["SALES_ORDER"]);
                            string loadNo = Convert.ToString(drLoads["LOAD_NO"]);

                            lCmd.CommandText = "SELECT SALES_ORDER, LOAD_NO, SUM(Load_Pcs) AS deli_pcs " +
                                               "FROM HMILoadDetails " +
                                               "WHERE SALES_ORDER = '" + salesOrder + "' AND LOAD_NO = '" + loadNo + "' " +
                                               "GROUP BY SALES_ORDER, LOAD_NO " +
                                               "ORDER BY SALES_ORDER, LOAD_NO";

                            lProcessObj.OpenNDSConnection(ref lndsCon);
                            oda.SelectCommand = lCmd;
                            lCmd.Connection = lndsCon;
                            lCmd.CommandTimeout = 15000;
                            oda.Fill(dsLPPieces);
                            lProcessObj.CloseNDSConnection(ref lndsCon);

                            if (dsLPPieces != null && dsLPPieces.Tables.Count > 0 && dsLPPieces.Tables[0].Rows.Count > 0)
                            {
                                string pcs = Convert.ToString(dsLPPieces.Tables[0].Rows[0]["deli_pcs"]);
                                strLPPieces += " " + loadNo + "(" + pcs + "),";
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                }

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
                ws.Cells[j + lRowNo, 14].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[j + lRowNo, 15].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[j + lRowNo, 16].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[j + lRowNo, 17].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[j + lRowNo, 18].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[j + lRowNo, 19].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[j + lRowNo, 20].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[j + lRowNo, 21].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[j + lRowNo, 22].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[j + lRowNo, 23].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[j + lRowNo, 24].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[j + lRowNo, 25].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[j + lRowNo, 26].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[j + lRowNo, 27].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[j + lRowNo, 28].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[j + lRowNo, 29].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[j + lRowNo, 30].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[j + lRowNo, 31].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[j + lRowNo, 32].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[j + lRowNo, 33].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[j + lRowNo, 34].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                //ws.Cells[j + lRowNo, 35].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[j + lRowNo, 35].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                //ws.Cells[j + lRowNo, 36].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[j + lRowNo, 36].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[j + lRowNo, 37].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[j + lRowNo, 38].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[j + lRowNo, 39].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[j + lRowNo, 40].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[j + lRowNo, 41].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[j + lRowNo, 42].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                ws.Cells[j + lRowNo, 1].Value = j + 1;
                ws.Cells[j + lRowNo, 2].Value = Convert.ToString(dr["SOR_No"]);
                ws.Cells[j + lRowNo, 3].Value = Convert.ToString(dr["SALES_ORDER"]); //"SO No\n加工表号";
                ws.Cells[j + lRowNo, 4].Value = Convert.ToString(dr["PO_Number"]);
                ws.Cells[j + lRowNo, 5].Value = Convert.ToString(dr["BBS_No"]);
                ws.Cells[j + lRowNo, 6].Value = Convert.ToString(dr["Total_Tonnage"]);
                ws.Cells[j + lRowNo, 7].Value = Convert.ToString(dr["Int_Remark"]);
                ws.Cells[j + lRowNo, 8].Value = Convert.ToString(dr["PO_Date"]);
                ws.Cells[j + lRowNo, 9].Value = Convert.ToString(dr["REQ_DAT_FRM_STR"]);
                ////ws.Cells[j + lRowNo, 10].Value = (SearchProducts == "MTS" ? null : Convert.ToString(dr["REQ_DAT_TO_STR"]));
                ws.Cells[j + lRowNo, 10].Value = Convert.ToString(dr["REQ_DAT_TO_STR"]);
                ws.Cells[j + lRowNo, 11].Value = Convert.ToString(dr["CONF_DEL_DATE"]);
                ws.Cells[j + lRowNo, 12].Value = Convert.ToString(dr["WBS1"]);
                ws.Cells[j + lRowNo, 13].Value = Convert.ToString(dr["WBS2"]);
                ws.Cells[j + lRowNo, 14].Value = Convert.ToString(dr["WBS3"]);
                //ws.Cells[j + lRowNo, 15].Value = Convert.ToString(dr["LP_No_of_Pieces"]);
                ws.Cells[j + lRowNo, 15].Value = strLPPieces;
                ws.Cells[j + lRowNo, 16].Value = Convert.ToString(dr["WT_DATE"]);
                ws.Cells[j + lRowNo, 17].Value = Convert.ToString(dr["DELIVERY_STATUS"]);
                ws.Cells[j + lRowNo, 18].Value = Convert.ToString(dr["Product_Type"]);
                ws.Cells[j + lRowNo, 19].Value = Convert.ToString(dr["ST_ELEMENT_TYPE"]);
                ws.Cells[j + lRowNo, 20].Value = Convert.ToString(dr["Contract"]);
                ws.Cells[j + lRowNo, 21].Value = Convert.ToString(dr["Customer"]);
                ws.Cells[j + lRowNo, 22].Value = Convert.ToString(dr["Project"]);
                ws.Cells[j + lRowNo, 23].Value = Convert.ToString(dr["Ext_Remark"]);
                ws.Cells[j + lRowNo, 24].Value = Convert.ToString(dr["PROJ_COORD"]);
                ws.Cells[j + lRowNo, 25].Value = Convert.ToString(dr["USERID"]);
                ws.Cells[j + lRowNo, 26].Value = Convert.ToString(dr["SOR_STATUS"]);
                ws.Cells[j + lRowNo, 27].Value = Convert.ToString(dr["ORDER_GROUP_ID"]);
                ws.Cells[j + lRowNo, 28].Value = Convert.ToString(dr["ON_HOLD_IND"]);

                ws.Cells[j + lRowNo, 29].Value = Convert.ToString(dr["URG_ORD_IND"]);
                ws.Cells[j + lRowNo, 30].Value = Convert.ToString(dr["PRM_SVC_IND"]);
                ws.Cells[j + lRowNo, 31].Value = Convert.ToString(dr["CRN_BKD_IND"]);
                ws.Cells[j + lRowNo, 32].Value = Convert.ToString(dr["BRG_BKD_IND"]);
                ws.Cells[j + lRowNo, 33].Value = Convert.ToString(dr["POL_ESC_IND"]);
                ws.Cells[j + lRowNo, 34].Value = Convert.ToString(dr["ZERO_TOLERANCE_I"]);
                ws.Cells[j + lRowNo, 35].Value = Convert.ToString(dr["CALL_BEF_DEL_IND"]);
                ws.Cells[j + lRowNo, 36].Value = Convert.ToString(dr["CONQUAS_IND"]);
                ws.Cells[j + lRowNo, 37].Value = Convert.ToString(dr["SPECIAL_PASS_IND"]);
                ws.Cells[j + lRowNo, 38].Value = Convert.ToString(dr["DO_NOT_MIX_IND"]);
                ws.Cells[j + lRowNo, 39].Value = Convert.ToString(dr["LORRY_CRANE_IND"]);
                ws.Cells[j + lRowNo, 40].Value = Convert.ToString(dr["LOW_BED_VEH_IND"]);
                ws.Cells[j + lRowNo, 41].Value = Convert.ToString(dr["TON50_VEH_IND"]);
                //ws.Cells[j + lRowNo, 35].Value = Convert.ToString(dr["PRJ_CST_DATE"]);
                //ws.Cells[j + lRowNo, 36].Value = Convert.ToString(dr["MUST_HAVE_DATE"]);
                ws.Cells[j + lRowNo, 42].Value = Convert.ToString(dr["UNIT_MODE_IND"]);

                ws.Cells[j + lRowNo, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;  // PO_Date
                ws.Cells[j + lRowNo, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; // Conf Del Date
                ws.Cells[j + lRowNo, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;    // Required Date
                ws.Cells[j + lRowNo, 11].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;    // rev Req Date
                //ws.Cells[j + lRowNo, 35].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //ws.Cells[j + lRowNo, 36].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells[j + lRowNo, 28].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;    //URG_ORD_IND
                ws.Cells[j + lRowNo, 29].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;    //PRM_SVC_IND
                ws.Cells[j + lRowNo, 30].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;    //CRN_BKD_IND
                ws.Cells[j + lRowNo, 31].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;    //BRG_BKD_IND
                ws.Cells[j + lRowNo, 32].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;    //POL_ESC_IND
                ws.Cells[j + lRowNo, 33].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;    //ON_HOLD_IND
                ws.Cells[j + lRowNo, 34].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;    //ZERO_TOLERANCE_IND
                ws.Cells[j + lRowNo, 35].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;    //CALL_BEF_DEL_IND
                ws.Cells[j + lRowNo, 36].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;    //CONQUAS_IND
                ws.Cells[j + lRowNo, 37].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;    //SPECIAL_PASS_IND
                ws.Cells[j + lRowNo, 38].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;    //DO_NOT_MIX_IND
                ws.Cells[j + lRowNo, 39].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;    //LORRY_CRANE_IND
                ws.Cells[j + lRowNo, 40].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;    //LOW_BED_VEH_IND
                ws.Cells[j + lRowNo, 41].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;    //TON50_VEH_IND
                ws.Cells[j + lRowNo, 42].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                j = j + 1;
            }
            MemoryStream ms = new MemoryStream();
            package.SaveAs(ms);
            var bExcel = new byte[ms.Length];
            ms.Position = 0;
            ms.Read(bExcel, 0, bExcel.Length);
            //bPDF = ms.GetBuffer();
            ms.Flush();
            ms.Dispose();
            lProcessObj = null;
            lCmd = null;
            lndsCon = null;
            oda = null;
            //return Json(bExcel, JsonRequestBehavior.AllowGet); //commented by vidya
            return Ok(bExcel);
        }

        #endregion Main functionality

        #region Watchlist functionality

        //[HttpPost]
        //[Route("/Watchlist_Query_Result")]
        //public ActionResult Watchlist_Query_Result(string SORNumbers_to_amend)
        //{
        //    Save_To_Watchlist(SORNumbers_to_amend);
        //    var lCmd = new OracleCommand();
        //    var lcisCon = new OracleConnection();
        //    var lProcessObj = new ProcessController();
        //    OracleConnection cnIDB = new OracleConnection();    //// 'SCM IDB
        //    OracleCommand cmdIDB = new OracleCommand(); ////'SCM IDB
        //    OracleDataAdapter oda = new OracleDataAdapter();
        //    DataSet ds = new DataSet();
        //    var lReturn = (new[]{ new
        //        {
        //            SONo = "",
        //            Customer = "",
        //            Project = "",
        //            ContractNo = "",
        //            ProjCoord = "",
        //            ReqDateFr = "",
        //            ReqDateTo = "",
        //            ConfirmedDate = "",
        //            HallAssignment = "",
        //            OnHold = "",
        //            LorryCrane = "",
        //            DoNotMix = "",
        //            CallBefDelivery = "",
        //            SpecialPass = "",
        //           BargeBooked = "",
        //            CraneBooked = "",
        //            PoliceEscort = "",
        //            PremiumService = "",
        //            UrgentOrder = "",
        //            ConquasOrder = "",
        //            ZeroTolerance = "",
        //            LowBedVehicleAllowed = "",
        //            FiftyTonVehicleAllowed = "",
        //            UnitMode = "",
        //            //ProjectCastingDate = "",
        //            //MustHaveDate = "",
        //            SalesOrder = "",
        //            SORNo = "",
        //            GroupID = "",
        //            PONumber = "",
        //            PODate = "",
        //            BBSNo = "",
        //            //LOADNO = "",
        //            ProductType = "",
        //            IntRemark = "",
        //            ExtRemark = "",
        //            STELEMENTTYPE = "",
        //            WBS1 = "",
        //            WBS2 = "",
        //            WBS3 = ""
        //        }}).ToList();
        //    try
        //    {
        //        //string[] SORNumbers = SORNumbers_to_amend.Split(',');
        //        lCmd.CommandText = "select DISTINCT H.NAME_AG as CUSTOMER, H.NAME_WE AS PROJECT, H.CONTRACT, "
        //            + " (select NAME1 from SAPSR3.KNA1 WHERE MANDT = '" + lProcessObj.strClient + "' AND KUNNR = (SELECT KUNN2 FROM SAPSR3.KNVP WHERE MANDT = '" + lProcessObj.strClient + "' AND PARVW = 'Z3' AND KUNNR = H.KUNNR)) AS PROJ_COORD, "
        //            //+ " TO_CHAR(TO_DATE(H.REQD_DEL_DATE, 'YYYYMMDD'), 'YYYY - MM - DD') AS REQD_DEL_DATE_STR, TO_DATE(H.REQD_DEL_DATE, 'YYYYMMDD') AS REQD_DEL_DATE, "
        //            + " H.REQ_DAT_FRM, (case when H.REQ_DAT_FRM is not null and H.REQ_DAT_FRM <> ' ' then to_date(H.REQ_DAT_FRM, 'yyyy-mm-dd.hh24.mi.ss') end) as REQ_DAT_FRM_DATE, "
        //            + " (case when H.REQ_DAT_FRM is not null and H.REQ_DAT_FRM <> ' ' then TO_CHAR(to_date(H.REQ_DAT_FRM, 'yyyy-mm-dd.hh24.mi.ss'), 'DD/MM/YYYY') end) as REQ_DAT_FRM_STR, "
        //            + " H.REQ_DAT_TO, " //(case when H.REQ_DAT_TO is not null and H.REQ_DAT_TO <> ' ' then to_date(H.REQ_DAT_TO, 'yyyy-mm-dd.hh24.mi.ss') end) as REQ_DAT_TO_DATE, "
        //            + " (case when H.REQ_DAT_TO is not null and H.REQ_DAT_TO <> ' ' then TO_CHAR(to_date(H.REQ_DAT_TO, 'yyyy-mm-dd.hh24.mi.ss'), 'DD/MM/YYYY') end) as REQ_DAT_TO_STR, "
        //            + " (case when H.STATUS IN ('C','S','Z') THEN (select TO_CHAR(to_date(max(conf_del_date), 'yyyymmdd'), 'DD/MM/YYYY') from SAPSR3.YMSDT_ORDER_ITEM where order_request_no = H.ORDER_REQUEST_NO AND LENGTH(conf_del_date)=8 AND conf_del_date <> '00000000' AND conf_del_date NOT LIKE '%-%') END) AS CONF_DEL_DATE, "
        //            + " H.ON_HOLD_IND, H.ZERO_TOLERANCE_I, H.CALL_BEF_DEL_IND, H.CONQUAS_IND, H.SPECIAL_PASS_IND, H.DO_NOT_MIX_IND, H.LORRY_CRANE_IND, H.URG_ORD_IND, "
        //            + " H.PRM_SVC_IND, H.CRN_BKD_IND, H.BRG_BKD_IND, H.POL_ESC_IND, H.WBS1 as HALL_ASSIGNMENT, H.LOW_BED_VEH_IND, H.TON50_VEH_IND, H.UNIT_MODE_IND, "
        //            + " (case when trim(H.PRJ_CST_DAT) is not null then to_date(H.PRJ_CST_DAT, 'yyyymmdd') end) as PRJ_CST_DAT, "
        //            + " (case when trim(H.PRJ_CST_DAT) is not null then TO_CHAR(to_date(H.PRJ_CST_DAT, 'yyyymmdd'),'DD/MM/YYYY') end) as PRJ_CST_DATE, "
        //            + " (case when trim(H.MUST_HAVE_DAT) is not null then to_date(H.MUST_HAVE_DAT, 'yyyymmdsd') end) as MUST_HAVE_DAT, "
        //            + " (case when trim(H.MUST_HAVE_DAT) is not null then TO_CHAR(to_date(H.MUST_HAVE_DAT, 'yyyymmdd'),'DD/MM/YYYY') end) as MUST_HAVE_DATE, "
        //            + " H.SALES_ORDER, H.ORDER_REQUEST_NO AS SOR_NO, H.ORDER_GROUP_ID, H.PO_Number, C.Product_Type, H.Int_Remark, H.Ext_Remark, "
        //            + " C.ST_ELEMENT_TYPE, C.WBS1, C.WBS2, C.WBS3, C.BBS_No, (case when H.CUST_ORDER_DATE <> '00000000' AND H.CUST_ORDER_DATE <> ' ' then TO_CHAR(to_date(H.CUST_ORDER_DATE, 'YYYYMMDD'), 'DD/MM/YYYY') end) as PO_DATE, "
        //            //+ " H.UDATE, H.UTIME, "
        //            + " TO_DATE(CONCAT(TO_CHAR(TO_DATE(H.UDATE, 'YYYYMMDD'), 'YYYY-MM-DD '), TO_CHAR(TO_DATE(H.UTIME, 'HH24MISS'), 'HH24:MI:SS')), 'YYYY-MM-DD HH24:MI:SS') AS UDATETIME "
        //            + " from SAPSR3.YMSDT_REQ_DETAIL C INNER JOIN SAPSR3.YMSDT_ORDER_HDR H ON C.ORDER_REQUEST_NO = H.ORDER_REQUEST_NO ";
        //        //foreach (string SORNumber in SORNumbers)
        //        //{
        //        lCmd.CommandText = lCmd.CommandText + " AND H.ORDER_REQUEST_NO IN ('" + SORNumbers_to_amend.Replace(",", "', '") + "') ";
        //        //}
        //        lCmd.CommandText = lCmd.CommandText + "  ORDER BY UDATETIME DESC ";
        //       // lProcessObj.OpenCISConnection(ref lcisCon);
        //        oda.SelectCommand = lCmd;
        //        lCmd.Connection = lcisCon;
        //        lCmd.CommandTimeout = 15000;
        //        //lRst = lCmd.ExecuteReader();
        //        oda.Fill(ds);
        //        //if (!lRst.HasRows)
        //        //{
        //        //    pInputProj.RemoveAt(i);
        //        //}
        //        //lRst.Close();
        //        lProcessObj.CloseCISConnection(ref lcisCon);
        //        //if (ds != null && ds.Tables.Count > 0)
        //        //{
        //        //    if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
        //        //    {
        //        foreach (DataRow dr in ds.Tables[0].Rows)
        //        {
        //            lReturn.Add(new
        //            {
        //                SONo = Convert.ToString(dr["SALES_ORDER"]),
        //                Customer = Convert.ToString(dr["Customer"]),
        //                Project = Convert.ToString(dr["Project"]),
        //                ContractNo = Convert.ToString(dr["Contract"]),
        //                ProjCoord = Convert.ToString(dr["PROJ_COORD"]),
        //                //ReqDateFr = Convert.ToString(dr["REQ_DAT_FRM"]),
        //                ReqDateFr = Convert.ToString(dr["REQ_DAT_FRM_STR"]),
        //                //ReqDateFr = (!string.IsNullOrEmpty(Convert.ToString(dr["REQ_DAT_FRM"]))) ? Convert.ToDateTime(dr["REQ_DAT_FRM"]).ToShortDateString() : "",
        //                //ReqDateTo = Convert.ToString(dr["REQ_DAT_TO"]),
        //                ReqDateTo = Convert.ToString(dr["REQ_DAT_TO_STR"]),
        //                //ReqDateTo = (!string.IsNullOrEmpty(Convert.ToString(dr["REQ_DAT_TO"]))) ? Convert.ToDateTime(dr["REQ_DAT_TO"]).ToShortDateString() : "",
        //                ConfirmedDate = Convert.ToString(dr["CONF_DEL_DATE"]),
        //                //ConfirmedDate = (!string.IsNullOrEmpty(Convert.ToString(dr["CONF_DEL_DATE"]))) ? Convert.ToDateTime(dr["CONF_DEL_DATE"]).ToShortDateString() : "",
        //                HallAssignment = Convert.ToString(dr["Hall_Assignment"]),
        //                OnHold = Convert.ToString(dr["ON_HOLD_IND"]),
        //                LorryCrane = Convert.ToString(dr["LORRY_CRANE_IND"]),
        //                DoNotMix = Convert.ToString(dr["DO_NOT_MIX_IND"]),
        //                CallBefDelivery = Convert.ToString(dr["CALL_BEF_DEL_IND"]),
        //                SpecialPass = Convert.ToString(dr["SPECIAL_PASS_IND"]),
        //                BargeBooked = Convert.ToString(dr["BRG_BKD_IND"]),
        //                CraneBooked = Convert.ToString(dr["CRN_BKD_IND"]),
        //                PoliceEscort = Convert.ToString(dr["POL_ESC_IND"]),
        //                PremiumService = Convert.ToString(dr["PRM_SVC_IND"]),
        //                UrgentOrder = Convert.ToString(dr["URG_ORD_IND"]),
        //                ConquasOrder = Convert.ToString(dr["CONQUAS_IND"]),
        //                ZeroTolerance = Convert.ToString(dr["ZERO_TOLERANCE_I"]),
        //                LowBedVehicleAllowed = Convert.ToString(dr["LOW_BED_VEH_IND"]),
        //                FiftyTonVehicleAllowed = Convert.ToString(dr["TON50_VEH_IND"]),
        //                UnitMode = Convert.ToString(dr["UNIT_MODE_IND"]),
        //                //ProjectCastingDate = Convert.ToString(dr["PRJ_CST_DATE"]),
        //                //MustHaveDate = Convert.ToString(dr["MUST_HAVE_DATE"]),
        //                SalesOrder = Convert.ToString(dr["Sales_Order"]),
        //                SORNo = Convert.ToString(dr["SOR_No"]),
        //                GroupID = Convert.ToString(dr["ORDER_GROUP_ID"]),
        //                PONumber = Convert.ToString(dr["PO_Number"]),
        //                PODate = Convert.ToString(dr["PO_Date"]),
        //                BBSNo = Convert.ToString(dr["BBS_No"]),
        //                //LOADNO = Convert.ToString(dr["LOAD_NO"]),
        //                ProductType = Convert.ToString(dr["Product_Type"]),
        //                IntRemark = Convert.ToString(dr["Int_Remark"]),
        //                ExtRemark = Convert.ToString(dr["Ext_Remark"]),
        //                STELEMENTTYPE = Convert.ToString(dr["ST_ELEMENT_TYPE"]),
        //                WBS1 = Convert.ToString(dr["WBS1"]),
        //                WBS2 = Convert.ToString(dr["WBS2"]),
        //                WBS3 = Convert.ToString(dr["WBS3"])
        //            });
        //        }
        //        //    }
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    finally
        //    {
        //        lCmd = null;
        //        lcisCon = null;
        //        oda = null;
        //        ds = null;
        //        //if (lReturn.Count > 1)
        //        //{
        //        lReturn.RemoveAt(0);
        //        //}
        //    }
        //    //return Json(lReturn, JsonRequestBehavior.AllowGet);
        //    return Ok(lReturn);
        //}

        [HttpPost]
        [Route("/Save_To_Watchlist")]
        public ActionResult Save_To_Watchlist(string SORNumbers_to_amend)
        {
            var lCmd = new SqlCommand();
            var lCon = new SqlConnection();
            var lProcess = new ProcessController();
            DataSet ds = new DataSet();
            try
            {
                #region Communicate Data to NDS database
                lCmd.CommandText = "usp_Save_Delete_DigiOs_Watchlist";
                lCmd.CommandType = CommandType.StoredProcedure;
                lCmd.Parameters.AddWithValue("SORs", SORNumbers_to_amend);
                lCmd.Parameters.AddWithValue("UserId", User.Identity.GetUserId());
                lCmd.Parameters.AddWithValue("Db_Operation", 1);
                lCmd.Connection = lCon;
                if (lProcess.OpenNDSConnection(ref lCon) == true)
                {
                    lCmd.ExecuteNonQuery();
                    lProcess.CloseNDSConnection(ref lCon);
                }
                #endregion
                return Ok(1);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
            finally
            {
                lCmd = null;
                lCon = null;
                ds = null;
                lProcess = null;
            }
        }

        [HttpPost]
        [Route("/Delete_From_Watchlist")]
        public ActionResult Delete_From_Watchlist(string SORNumbers_to_amend)
        {
            var lCmd = new SqlCommand();
            var lCon = new SqlConnection();
            var lProcess = new ProcessController();
            DataSet ds = new DataSet();
            try
            {
                #region Communicate Data to NDS database
                lCmd.CommandText = "usp_Save_Delete_DigiOs_Watchlist";
                lCmd.CommandType = CommandType.StoredProcedure;
                lCmd.Parameters.AddWithValue("SORs", SORNumbers_to_amend);
                lCmd.Parameters.AddWithValue("UserId", User.Identity.GetUserId());
                lCmd.Parameters.AddWithValue("Db_Operation", 2);
                lCmd.Connection = lCon;
                if (lProcess.OpenNDSConnection(ref lCon) == true)
                {
                    lCmd.ExecuteNonQuery();
                    lProcess.CloseNDSConnection(ref lCon);
                }
                #endregion
                return Ok(1);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
            finally
            {
                lCmd = null;
                lCon = null;
                ds = null;
                lProcess = null;
            }
        }

        //[HttpPost]
        //[Route("/Watchlist_Search")]
        //public ActionResult Watchlist_Search(string SONumber, string SORNumber)
        //{
        //    var lReturn = (new[]{ new
        //        {
        //            SONo = "",
        //            SORNo = "",
        //            Customer = "",
        //            Project = "",
        //            //ContractNo = "",
        //            STATUS = "",
        //            EntryDate = ""
        //        }}).ToList();
        //    var lCmd = new OracleCommand();
        //    var lcisCon = new OracleConnection();
        //    var lProcessObj = new ProcessController();
        //    OracleDataAdapter oda = new OracleDataAdapter();
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        lCmd.CommandText = "select DISTINCT H.NAME_AG as CUSTOMER, H.NAME_WE AS PROJECT, H.CONTRACT, "
        //            + " H.SALES_ORDER, H.ORDER_REQUEST_NO AS SOR_NO, H.STATUS, TO_CHAR(SYSDATE, 'DD/MM/YYYY') as Entry_date, "
        //            + //" H.UDATE, H.UTIME, " +
        //            "TO_DATE(CONCAT(TO_CHAR(TO_DATE(H.UDATE, 'YYYYMMDD'), 'YYYY-MM-DD '), TO_CHAR(TO_DATE(H.UTIME, 'HH24MISS'), 'HH24:MI:SS')), 'YYYY-MM-DD HH24:MI:SS') AS UDATETIME "
        //            + " from SAPSR3.YMSDT_REQ_DETAIL C INNER JOIN SAPSR3.YMSDT_ORDER_HDR H ON C.ORDER_REQUEST_NO = H.ORDER_REQUEST_NO ";
        //        lCmd.CommandText = lCmd.CommandText + " where 1 = 1 ";
        //        if (!String.IsNullOrEmpty(SONumber))
        //            lCmd.CommandText = lCmd.CommandText + " AND H.SALES_ORDER LIKE '%" + SONumber + "%' ";
        //        if (!String.IsNullOrEmpty(SORNumber))
        //            lCmd.CommandText = lCmd.CommandText + " AND H.ORDER_REQUEST_NO LIKE  '%" + SORNumber + "%' ";
        //        lCmd.CommandText = lCmd.CommandText + "  ORDER BY UDATETIME DESC ";
        //        lProcessObj.OpenCISConnection(ref lcisCon);
        //        oda.SelectCommand = lCmd;
        //        lCmd.Connection = lcisCon;
        //        lCmd.CommandTimeout = 15000;
        //        oda.Fill(ds);
        //        lProcessObj.CloseCISConnection(ref lcisCon);
        //        foreach (DataRow dr in ds.Tables[0].Rows)
        //        {
        //            lReturn.Add(new
        //            {
        //                SONo = Convert.ToString(dr["SALES_ORDER"]),
        //                SORNo = Convert.ToString(dr["SOR_No"]),
        //                Customer = Convert.ToString(dr["Customer"]),
        //                Project = Convert.ToString(dr["Project"]),
        //                //ContractNo = Convert.ToString(dr["Contract"]),
        //                STATUS = Convert.ToString(dr["STATUS"]),
        //                EntryDate = Convert.ToString(dr["Entry_date"])
        //            });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    finally
        //    {
        //        lCmd = null;
        //        lcisCon = null;
        //        oda = null;
        //        ds = null;
        //        //if (lReturn.Count > 1)
        //        //{
        //        lReturn.RemoveAt(0);
        //        //}
        //    }
        //    //return Json(lReturn, JsonRequestBehavior.AllowGet);
        //    return Ok(lReturn);
        //}

        //[HttpPost]
        //[Route("/reload_Watchlist")]
        //public ActionResult reload_Watchlist()
        //{
        //    var lReturn = (new[]{ new
        //        {
        //            SONo = "",
        //            SORNo = "",
        //            Customer = "",
        //            Project = "",
        //            //ContractNo = "",
        //            STATUS = "",
        //            EntryDate = ""
        //        }}).ToList();
        //    StringBuilder SORNumbers_to_amend = new StringBuilder();
        //    #region Getting SOR's saved in watchlist for this particular user from ODOS Database
        //    var lNDSCmd = new SqlCommand();
        //    var lNDSCon = new SqlConnection();
        //    var oNDSda = new SqlDataAdapter();
        //    var lProcess = new ProcessController();
        //    DataSet dsNDS = new DataSet();
        //    try
        //    {
        //        lNDSCmd.CommandText = "SELECT SOR_Number, CONVERT(VARCHAR(20), CreatedDate, 103) as Entry_date FROM tbl_DigiOS_Watchlist WHERE UserId ='" + User.Identity.GetUserId() + "' AND ISNULL(IsActive, 0) = 1 AND ISNULL(IsDeleted, 0) = 0";
        //        lProcess.OpenNDSConnection(ref lNDSCon);
        //        oNDSda.SelectCommand = lNDSCmd;
        //        lNDSCmd.Connection = lNDSCon;
        //        lNDSCmd.CommandTimeout = 15000;
        //        oNDSda.Fill(dsNDS);
        //        lProcess.CloseNDSConnection(ref lNDSCon);
        //        if (dsNDS != null && dsNDS.Tables[0] != null)
        //        {
        //            foreach (DataRow dr in dsNDS.Tables[0].Rows)
        //            {
        //                if (SORNumbers_to_amend.Length == 0)
        //                {
        //                    SORNumbers_to_amend.Append(dr["SOR_Number"]);
        //                }
        //                else
        //                {
        //                    SORNumbers_to_amend.Append("," + dr["SOR_Number"]);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    finally
        //    {
        //        lNDSCmd = null;
        //        lNDSCon = null;
        //        lProcess = null;
        //        oNDSda = null;
        //    }
        //    #endregion
        //    var lCmd = new OracleCommand();
        //    var lcisCon = new OracleConnection();
        //    var lProcessObj = new ProcessController();
        //    OracleDataAdapter oda = new OracleDataAdapter();
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        lCmd.CommandText = "select DISTINCT H.NAME_AG as CUSTOMER, H.NAME_WE AS PROJECT, H.CONTRACT, "
        //            + " H.SALES_ORDER, H.ORDER_REQUEST_NO AS SOR_NO, H.STATUS, TO_CHAR(SYSDATE, 'DD/MM/YYYY') as Entry_date, "
        //            + //" H.UDATE, H.UTIME, " +
        //            " TO_DATE(CONCAT(TO_CHAR(TO_DATE(H.UDATE, 'YYYYMMDD'), 'YYYY-MM-DD '), TO_CHAR(TO_DATE(H.UTIME, 'HH24MISS'), 'HH24:MI:SS')), 'YYYY-MM-DD HH24:MI:SS') AS UDATETIME "
        //            + " from SAPSR3.YMSDT_REQ_DETAIL C INNER JOIN SAPSR3.YMSDT_ORDER_HDR H ON C.ORDER_REQUEST_NO = H.ORDER_REQUEST_NO ";
        //        lCmd.CommandText = lCmd.CommandText + " where 1 = 1 ";
        //        lCmd.CommandText = lCmd.CommandText + " AND H.ORDER_REQUEST_NO IN ('" + SORNumbers_to_amend.Replace(",", "', '") + "') ";
        //        lCmd.CommandText = lCmd.CommandText + "  ORDER BY UDATETIME DESC ";
        //        lProcessObj.OpenCISConnection(ref lcisCon);
        //        oda.SelectCommand = lCmd;
        //        lCmd.Connection = lcisCon;
        //        lCmd.CommandTimeout = 15000;
        //        oda.Fill(ds);
        //        lProcessObj.CloseCISConnection(ref lcisCon);
        //        string entry_date;
        //        foreach (DataRow dr in ds.Tables[0].Rows)
        //        {
        //            entry_date = null;
        //            foreach (DataRow drr in dsNDS.Tables[0].Rows)
        //            {
        //                if (Convert.ToString(drr["SOR_Number"]) == Convert.ToString(dr["SOR_No"]))
        //                {
        //                    entry_date = Convert.ToString(drr["Entry_date"]);
        //                }
        //            }
        //            lReturn.Add(new
        //            {
        //                SONo = Convert.ToString(dr["SALES_ORDER"]),
        //                SORNo = Convert.ToString(dr["SOR_No"]),
        //                Customer = Convert.ToString(dr["Customer"]),
        //                Project = Convert.ToString(dr["Project"]),
        //                //ContractNo = Convert.ToString(dr["Contract"]),
        //                STATUS = Convert.ToString(dr["STATUS"]),
        //                EntryDate = (!string.IsNullOrEmpty(entry_date) ? entry_date : Convert.ToString(dr["Entry_date"]))
        //            });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    finally
        //    {
        //        lCmd = null;
        //        lcisCon = null;
        //        oda = null;
        //        ds = null;
        //        //if (lReturn.Count > 1)
        //        //{
        //        lReturn.RemoveAt(0);
        //        //}
        //    }
        //    //return Json(lReturn, JsonRequestBehavior.AllowGet);
        //    return Ok(lReturn);
        //}

        #endregion Watchlist functionality

        #region Commented Code

        //[NonAction]
        //public ActionResult Test_OpenWatchlistPopup()
        //{
        //    //can send some data also.
        //    // return "<h1>This is Modal Popup Window</h1>";
        //    return View("OpenWatchlistPopup");
        //}
        //[HttpGet]
        //public ActionResult OpenWatchlistPopup()
        //{
        //    ViewData["Msg_Text_For_popup"] = "This is Modal Popup Window";
        //    //can send some data also.
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult Amendment_Proj_Casting_Date(string Project_Casting_Date_Change, string Must_Have_Date_Change, string SORNumbers_to_amend)
        //{
        //    string strReturn = "";
        //    var lCmd = new OracleCommand();
        //    var lcisCon = new OracleConnection();
        //    var lProcessObj = new ProcessController();
        //    OracleConnection cnIDB = new OracleConnection();    //// 'SCM IDB
        //    OracleCommand cmdIDB = new OracleCommand(); ////'SCM IDB
        //    try
        //    {
        //        string strSqlUpdateOrderChangeHistory = "";   //Update order change hisgory in DSS
        //        lProcessObj.OpenCISConnection(ref lcisCon);
        //        lProcessObj.OpenIDBConnection(ref cnIDB);
        //        int iCounterOrderChangeHistory = 0;
        //        string[] SORNumbers = SORNumbers_to_amend.Split(',');
        //        DateTime strCurrentTimeStamp = DateTime.Now;
        //        lCmd.Connection = lcisCon;
        //        lCmd.CommandTimeout = 15000;
        //        cmdIDB.Connection = cnIDB;
        //        cmdIDB.CommandTimeout = 15000;
        //        /////ORDER_REQUEST_NO === SONumber
        //        foreach (string SORNumber in SORNumbers)
        //        {
        //            if (!string.IsNullOrEmpty(SORNumber) && SORNumber != "")
        //            {
        //                /////'Added for CHG0031726
        //                /////'ymsdt_order_hdr @nshoracis
        //                lCmd.CommandText = "update SAPSR3.YMSDT_ORDER_HDR " ////& strDSSDBLink & _
        //                            + " SET  PRJ_CST_DAT ='" + Project_Casting_Date_Change.Replace("-", "")
        //                            + "', MUST_HAVE_DAT = '" + Must_Have_Date_Change.Replace("-", "")
        //                            + "' WHERE MANDT = '" + lProcessObj.strClient + "' AND ORDER_REQUEST_NO = '" + SORNumber + "'";
        //                lCmd.ExecuteNonQuery();
        //                /////'START SCM IDB Amending Proj Casting Date and Must Have date for selected orders
        //                cmdIDB.CommandText = "update order_header "
        //                    + " SET projected_casting_date ='" + Project_Casting_Date_Change.Replace("-", "")
        //                   + "', must_have_date ='" + Must_Have_Date_Change.Replace("-", "")
        //                   + "' where ORDER_REQUEST_NO ='" + SORNumber + "'";
        //                cmdIDB.ExecuteNonQuery();
        //                /////'END SCM IDB Amending Proj Casting Date and Must Have date for selected orders
        //                /////'Update order change history
        //                /////'Update order change history
        //                iCounterOrderChangeHistory += 1;
        //                //strOrderChangeTimestamp = strCurrentTimeStamp.ToString().Substring(0, 20);// + (900000 + iCounterOrderChangeHistory);
        //                strSqlUpdateOrderChangeHistory = "insert into SAPSR3.YMSDT_ORD_CLOG " +
        //                         "(och_timestamp_ch, och_sales_order, och_ord_item, och_par_chng,  och_old_value, och_new_value, och_chng_rsn, och_chng_userid)" +
        //                         " values ('" + strCurrentTimeStamp + "'," + "'" + SORNumber + "'," + "0," + "'PRJ CST DAT','"
        //                         + Project_Casting_Date_Change.Replace("-", "") + "'," + "'" + Project_Casting_Date_Change.Replace("-", "") + "'," + "'NA'," + "'DigiOS')";
        //                strSqlUpdateOrderChangeHistory = "insert into SAPSR3.YMSDT_ORD_CLOG " +
        //                         "(och_timestamp_ch, och_sales_order, och_ord_item, och_par_chng,  och_old_value, och_new_value, och_chng_rsn, och_chng_userid)" +
        //                         " values ('" + strCurrentTimeStamp + "'," + "'" + SORNumber + "'," + "0," + "' MUST HAVE DATE','"
        //                         + Must_Have_Date_Change.Replace("-", "") + "'," + "'" + Must_Have_Date_Change.Replace("-", "") + "'," + "'NA'," + "'DigiOS')";
        //                lCmd.CommandText = strSqlUpdateOrderChangeHistory;
        //                lCmd.ExecuteNonQuery();
        //            }
        //        }
        //        lProcessObj.CloseIDBConnection(ref cnIDB);
        //        lProcessObj.CloseCISConnection(ref lcisCon);
        //        strReturn = Convert.ToString("Project Casting and Must Have Date for " + iCounterOrderChangeHistory + " order(s) amended successfully.");
        //    }
        //    catch (Exception ex)
        //    {
        //        strReturn = "Error occurred while amending Project Casting and Must Have date, Please try again.!!!";
        //    }
        //    finally
        //    {
        //        cmdIDB = null;
        //        cnIDB = null;
        //        lCmd = null;
        //        lcisCon = null;
        //    }
        //    //return Json(strReturn, JsonRequestBehavior.AllowGet); 
        //}
        //[HttpPost]
        //public ActionResult Amendment_Conf_Del_Date(string Conf_Del_Date, string Reason_Change_Conf_Del_Date, string SORNumbers_to_amend)
        //{
        //    string strReturn = "";
        //    var lCmd = new OracleCommand();
        //    var lcisCon = new OracleConnection();
        //    var lProcessObj = new ProcessController();
        //    //OracleConnection cnIDB = new OracleConnection();    //// 'SCM IDB
        //    //OracleCommand cmdIDB = new OracleCommand(); ////'SCM IDB
        //    try
        //    {
        //        string strSqlUpdateOrderChangeHistory = "";   //Update order change hisgory in DSS
        //        lProcessObj.OpenCISConnection(ref lcisCon);
        //        //lProcessObj.OpenIDBConnection(ref cnIDB);
        //        int iCounterOrderChangeHistory = 0;
        //        string[] SORNumbers = SORNumbers_to_amend.Split(',');
        //        DateTime strCurrentTimeStamp = DateTime.Now;
        //        lCmd.Connection = lcisCon;
        //        lCmd.CommandTimeout = 15000;
        //        //cmdIDB.Connection = cnIDB;
        //        //cmdIDB.CommandTimeout = 15000;
        //        /////ORDER_REQUEST_NO === SONumber
        //        foreach (string SORNumber in SORNumbers)
        //        {
        //            if (!string.IsNullOrEmpty(SORNumber) && SORNumber != "")
        //            {
        //                //////'update SAP Y table
        //                /////ymsdt_order_item  @nshoracis
        //                lCmd.CommandText = "update SAPSR3.YMSDT_ORDER_ITEM "
        //                    + " SET CONF_DEL_DATE ='" + Conf_Del_Date.Replace("-", "")
        //                    //+ " SET CONF_DEL_DATE ='" + Conf_Del_Date
        //                    + "' WHERE MANDT = '" + lProcessObj.strClient + "' AND ORDER_REQUEST_NO = '" + SORNumber + "'";
        //                lCmd.ExecuteNonQuery();
        //                /////'Added for CHG0031726
        //                /////'ymsdt_order_hdr @nshoracis
        //                lCmd.CommandText = "update SAPSR3.YMSDT_ORDER_HDR " ////& strDSSDBLink & _
        //                    + " SET  ORDER_CONF_IND = 'Y' "
        //                    + " WHERE MANDT = '" + lProcessObj.strClient + "' AND ORDER_REQUEST_NO = '" + SORNumber + "'";
        //                lCmd.ExecuteNonQuery();
        //                /////'Update order change history
        //                iCounterOrderChangeHistory += 1;
        //                //strOrderChangeTimestamp = strCurrentTimeStamp.ToString().Substring(0, 20);// + (900000 + iCounterOrderChangeHistory);
        //                strSqlUpdateOrderChangeHistory = "insert into SAPSR3.YMSDT_ORD_CLOG " +
        //                    "(och_timestamp_ch, och_sales_order, och_ord_item, och_par_chng,  och_old_value, och_new_value, och_chng_rsn, och_chng_userid)" +
        //                    " values ('" + strCurrentTimeStamp + "'," + "'" + SORNumber + "'," + "0," + "'CONF DEL DATE','" + Conf_Del_Date.Replace("-", "")
        //                    + "'," + "'" + Conf_Del_Date.Replace("-", "") + "'," + "'" + Reason_Change_Conf_Del_Date + "'," + "'DigiOS')";
        //                lCmd.CommandText = strSqlUpdateOrderChangeHistory;
        //                lCmd.ExecuteNonQuery();
        //            }
        //        }
        //        //lProcessObj.CloseIDBConnection(ref cnIDB);               
        //        lProcessObj.CloseCISConnection(ref lcisCon);
        //        strReturn = Convert.ToString("Confirmed Delivery Date for " + iCounterOrderChangeHistory + " order(s) amended successfully.");
        //    }
        //    catch (Exception ex)
        //    {
        //        strReturn = "Error occurred while amending confirmed delivery date, Please try again.!!!";
        //    }
        //    finally
        //    {
        //        //cmdIDB = null;
        //        //cnIDB = null;
        //        lCmd = null;
        //        lcisCon = null;
        //    }
        //    //return Json(strReturn, JsonRequestBehavior.AllowGet); 
        //}
        //[HttpGet]
        //public PartialViewResult SomeAction()
        //{
        //    return PartialView("_LoginPartial");
        //}

        #endregion Commented Code

        [HttpGet]
        [Route("/GetDesignationData/{DesignationType}")]
        public ActionResult GetDesignationData(string DesignationType)
        {
            //var lCmd = new OracleCommand();
            var lProcessObj = new ProcessController();
            var lDa = new SqlDataAdapter();
            var lCmd = new SqlCommand();
            var lDs = new DataSet();
            var cnNDS = new SqlConnection();
            IList<OrderAmendmentModels> lmodel = new List<OrderAmendmentModels>();
            if (DesignationType == "1")
            {
                lCmd.CommandText = "SELECT DISTINCT (PROJECT_LEADER) AS employee_name FROM HMIProjectMaster ";
            }
            //else if (DesignationType == "2")
            //{
            //    lCmd.CommandText = "SELECT (NAME1 || NAME2) as employee_name, KUNNR as employee_id FROM SAPSR3.KNA1 " +
            //    "WHERE MANDT = '" + lProcessObj.strClient + "' AND KUNNR IN (SELECT DISTINCT KUNNR FROM SAPSR3.VBPA WHERE PARVW ='Z2' " +
            //    "AND MANDT = '" + lProcessObj.strClient + "' AND AUFSD <> '01') ORDER BY NAME1 ";
            //}
            //else if (DesignationType == "3")
            //{
            //    lCmd.CommandText = "SELECT (NAME1 || NAME2) as employee_name, KUNNR as employee_id FROM SAPSR3.KNA1 " +
            //   "WHERE MANDT = '" + lProcessObj.strClient + "' AND KUNNR IN (SELECT DISTINCT KUNNR FROM SAPSR3.VBPA WHERE PARVW ='Z3' " +
            //   "AND MANDT = '" + lProcessObj.strClient + "' AND AUFSD <> '01') ORDER BY NAME1 ";
            //}
           // lProcessObj.OpenCISConnection(ref lcisCon);
            lProcessObj.OpenNDSConnection(ref cnNDS);
            lCmd.Connection = cnNDS;
            lDa.SelectCommand = lCmd;
            lDs.Clear();
            lDa.Fill(lDs);
            // lProcessObj.CloseCISConnection(ref lcisCon);
            lProcessObj.CloseNDSConnection(ref cnNDS);
            OrderAmendmentModels obj;
            foreach (DataRow dr in lDs.Tables[0].Rows)
            {
                obj = new OrderAmendmentModels();
                //obj.EmployeeId = Convert.ToString(dr["employee_id"]);
                obj.EmployeeName = Convert.ToString(dr["employee_name"]);
                lmodel.Add(obj);
            }
            return Ok(lmodel);
        }

        [HttpGet]
        [Route("/GetCustomerAndProject")]
        public ActionResult GetCustomerAndProject()
        {
            UserAccessController lUa = new UserAccessController();
            var lUserType = "AD";//lUa.getUserType(User.Identity.GetUserName());
            var lGroupName = "jagdishh_ttl@natsteel.com.sg";//lUa.getGroupName(User.Identity.GetUserName());
            gUserType = lUserType;
            gGroupName = lGroupName;
            //ViewBag.UserType = lUserType;
            lUa = null;
            string lCustomerCode = "";
            string lProjectCode = "";
            List<AmendmentDto> lAmendmentDto = new List<AmendmentDto>();
            IEnumerable<ProjectListModels> content = new List<ProjectListModels> {
                new ProjectListModels
                {
                    CustomerCode = "",
                    ProjectCode = "",
                    ProjectTitle = " -- Select Project -- "
                } };
            if (lUserType == "AD" || lUserType == "PU" || lUserType == "PA" || lUserType == "PM" || lUserType == "P1" || lUserType == "P1" || lUserType == "TE")
            {
                var content1 = (from c in db.Customer
                                where (from m in db.Project select m.CustomerCode).Contains(c.CustomerCode)
                                orderby c.CustomerName
                                select c).ToList();
                var CustomerSelection = new SelectList(new List<SelectListItem>(content1.Select(h => new SelectListItem
                {
                    Value = h.CustomerCode,
                    Text = h.CustomerName
                })), "Value", "Text");
                if (content1.Count() > 0)
                {
                    lCustomerCode = content1.First().CustomerCode;
                    content = (from p in db.ProjectList
                               where p.CustomerCode == lCustomerCode &&
                               (from m in db.Project
                                where m.CustomerCode == lCustomerCode
                                select m.ProjectCode).Contains(p.ProjectCode)
                               orderby p.ProjectTitle
                               select p).ToList();
                    content = content.OrderBy(o => o.ProjectTitle).ToList();
                    //content = RemoveNonMESHProject(content.ToList());
                }
                if (content.Count() == 0)
                {
                    content = new List<ProjectListModels> {
                        new ProjectListModels
                        {
                            CustomerCode = "",
                            ProjectCode = "",
                            ProjectTitle = " -- Select Project -- "
                       }
                    };
                }
                else
                {
                    lProjectCode = content.First().ProjectCode;
                }
                var ProjectSelection = new SelectList(new List<SelectListItem>(content.Select(h => new SelectListItem
                {
                    Value = h.ProjectCode.Trim(),
                    Text = h.ProjectTitle + "(" + h.ProjectCode + ")"
                })), "Value", "Text");
                lAmendmentDto.Add(new AmendmentDto
                {

                    lProjectSelection = ProjectSelection,
                    lCustomerSelection = CustomerSelection
                    
                }
                     ) ;
                var result = lAmendmentDto;
                return Ok(result);
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
                var CustomerSelection = new SelectList(new List<SelectListItem>(content3.Select(h => new SelectListItem
                {
                    Value = h.CustomerCode,
                    Text = h.CustomerName
                })), "Value", "Text");
                if (content2.Count() > 0)
                {
                    lCustomerCode = content2.First().CustomerCode.Substring(0, 10);
                    content = (from p in db.ProjectList
                               where p.CustomerCode == lCustomerCode &&
                               (from u in db.UserAccess
                                where u.UserName == lGroupName &&
                                p.CustomerCode == lCustomerCode
                                select u.ProjectCode).Contains(p.ProjectCode)
                               orderby p.ProjectTitle
                               select p).ToList();
                    //content = RemoveNonMESHProject(content.ToList());
                }
                if (content.Count() == 0)
                {
                    content = new List<ProjectListModels> {
                        new ProjectListModels
                        {
                            CustomerCode = "",
                            ProjectCode = "",
                            ProjectTitle = " -- Select Project -- "
                        }
                    };
                }
                else
                {
                    lProjectCode = content.First().ProjectCode;
                }
                var ProjectSelection = new SelectList(new List<SelectListItem>(content.Select(h => new SelectListItem
                {
                    Value = h.ProjectCode,
                    Text = h.ProjectTitle + "(" + h.ProjectCode + ")"
                })), "Value", "Text");
                lAmendmentDto.Add(new AmendmentDto
                {

                    lProjectSelection = ProjectSelection,
                    lCustomerSelection = CustomerSelection

                }
                   );
                var result = lAmendmentDto;
                return Ok(result);
            }
          
        }


        //Added by aishwarya -For error resolution - error during serialization or deserialization using the json javascriptserializer. the length of the string exceeds the value
        //protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        //{
        //    return new JsonResult()
        //    {
        //        Data = data,
        //        ContentType = contentType,
        //        ContentEncoding = contentEncoding,
        //        JsonRequestBehavior = behavior,
        //        MaxJsonLength = Int32.MaxValue
        //    };
        //}
    }
}
