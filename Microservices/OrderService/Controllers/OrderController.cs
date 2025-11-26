using Apitron.PDF.Kit.FixedLayout.Content;
using AutoMapper;
using iText.StyledXmlParser.Jsoup.Select;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using OrderService.Dtos;
using OrderService.Interfaces;
using OrderService.Models;
using OrderService.Repositories;
using System.Data;
using System.Data.SqlClient;
using ZstdSharp;


namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private DBContextModels db = new DBContextModels();

        private readonly IOrder _OrderRepository;
        private readonly IMapper _mapper;

        public OrderController(IOrder orderService, IMapper mapper, IESMOrderProcessingService eSMOrderProcessingService)
        {
            _OrderRepository = orderService;
            _mapper = mapper;

        }
        //HMI Updated function
        [HttpGet]
        [Route("/GetProjects/{pCustomerCode}/{pUserType}/{pGroupName}")]
        public async Task<IActionResult> GetProjects(string pCustomerCode, string pUserType, string pGroupName)
        {
            string lUserType = pUserType;
            string lGroupName = pGroupName;
            string lErrorMsg = "";

            OracleDataReader lRst;
            var lCmd = new OracleCommand();
            var lcisCon = new OracleConnection();


            List<ProjectListModels> content = new List<ProjectListModels> {
            new ProjectListModels
            {
                CustomerCode = "",
                ProjectCode = "",
                ProjectTitle = ""
            } };

            try
            {
                if (lUserType == "PL" || lUserType == "AD" || lUserType == "PM" || lUserType == "PA" || lUserType == "P1" || lUserType == "P2" || lUserType == "PU" || lUserType == "TE")
                {
                    //content = new List<ProjectListModels>();
                    //#region Get from SAP
                    //var lDa = new OracleDataAdapter();
                    //var lOCmd = new OracleCommand();
                    //var lDs = new DataSet();
                    //var lDStatus = new DataSet();
                    //var lOcisCon = new OracleConnection();
                    //var lProcess = new ProcessController();

                    //lOCmd.CommandText = "SELECT (NAME1 || NAME2) AS SHIP_TO_NAME,KUNNR AS SHIP_TO_PARTY FROM SAPSR3.KNA1 " +
                    //"WHERE KTOKD = 'Y001' AND MANDT ='" + lProcess.strClient + "' " +
                    //"AND KUNNR IN (SELECT KUNNR FROM SAPSR3.VBPA WHERE MANDT='" + lProcess.strClient + "' " +
                    //"AND VBELN IN (SELECT VBELN FROM SAPSR3.VBAK WHERE MANDT ='" + lProcess.strClient + "' " +
                    //"AND (VBELN like 'NSH%' OR VBELN like '102%' OR VBELN like '_102%' OR VBELN like '112%' OR VBELN like '_112%') " +
                    //"AND (ytot_cab > 0 " +
                    //"OR ytot_mesh > 0 " +
                    //"OR ytot_bpc > 0 " +
                    //"OR ytotal_wr > 0 " +
                    //"OR ytot_cold_roll > 0 " +
                    //"OR ytotal_pcstrand > 0 " +
                    //"OR ytot_rebar > 0 " +
                    //"OR ytot_car > 0 " +
                    //"OR ytot_pre_cutwr > 0 " +
                    //"OR ytot_precage > 0 " +
                    //"OR ytot_coupler > '000000' ) " +
                    //"AND KUNNR ='" + pCustomerCode + "' AND TRVOG ='4' " +
                    //"AND to_date(GUEEN, 'yyyymmdd') >=  (SYSDATE - 31) )) " +
                    //"ORDER BY 1 ";

                    //if (lProcess.OpenCISConnection(ref lOcisCon) == true)
                    //{
                    //    lOCmd.Connection = lOcisCon;
                    //    lOCmd.CommandTimeout = 300;
                    //    lDa.SelectCommand = lOCmd;
                    //    lDs.Clear();
                    //    lDa.Fill(lDs);
                    //    if (lDs.Tables[0].Rows.Count > 0)
                    //    {
                    //        for (int i = 0; i < lDs.Tables[0].Rows.Count; i++)
                    //        {
                    //            string lName = ((string)lDs.Tables[0].Rows[i].ItemArray[0]).Trim();
                    //            string lCode = ((string)lDs.Tables[0].Rows[i].ItemArray[1]).Trim();
                    //            content.Add(new ProjectListModels
                    //            {
                    //                CustomerCode = pCustomerCode,
                    //                ProjectCode = lCode,
                    //                ProjectTitle = lName + " (" + lCode + ")"
                    //            });
                    //        }
                    //    }

                      //  lProcess.CloseCISConnection(ref lOcisCon);

                        if (pCustomerCode != "")
                        {
                            SqlDataReader lNDSRst;
                            var lNDSCmd = new SqlCommand();
                            var lNDSCon = new SqlConnection();
                            var lNDSDa = new SqlDataAdapter();
                            var lNDSDs = new DataSet();

                            content = new List<ProjectListModels>();
                            //var lDa = new OracleDataAdapter();
                            //var lOCmd = new OracleCommand();
                            var lDs = new DataSet();
                            var lDStatus = new DataSet();
                            //var lOcisCon = new OracleConnection();
                            var lProcess = new ProcessController();

                            //if (lProcess.OpenCISConnection(ref lOcisCon) == true)
                            //{
                            //lOCmd.Connection = lOcisCon;
                            //lOCmd.CommandTimeout = 300;
                            //lDa.SelectCommand = lOCmd;
                            //lDs.Clear();
                            //lDa.Fill(lDs);
                            //if (lDs.Tables[0].Rows.Count > 0)
                            //{
                            //    for (int i = 0; i < lDs.Tables[0].Rows.Count; i++)
                            //    {
                            //        string lName = ((string)lDs.Tables[0].Rows[i].ItemArray[0]).Trim();
                            //        string lCode = ((string)lDs.Tables[0].Rows[i].ItemArray[1]).Trim();
                            //        content.Add(new ProjectListModels
                            //        {
                            //            CustomerCode = pCustomerCode,
                            //            ProjectCode = lCode,
                            //            ProjectTitle = lName + " (" + lCode + ")"
                            //        });
                            //    }
                            //}

                            lProcess.OpenNDSConnection(ref lNDSCon);

                            // TEMPORARY HMI CODE
                            #region -- TEMPORARY HMI CODE
                            lNDSCmd.CommandText = "SELECT PM.project_name as ProjectTitle , PM.project_code as ProjectCode " +
                            "FROM HMIProjectMaster PM INNER JOIN HMIContractproject CP " +
                            "ON PM.Project_Code = CP.Project_Code WHERE CP.contract_no in" +
                            " (SELECT Contract_no FROM HMIContractMaster WHERE cust_id = '" + pCustomerCode + "')";

                            lNDSCmd.Connection = lNDSCon;
                            lNDSDa.SelectCommand = lNDSCmd;
                            lNDSDs.Clear();
                            lNDSDa.Fill(lNDSDs);
                            if (lNDSDs.Tables[0].Rows.Count > 0)
                            {
                                for (int i = 0; i < lNDSDs.Tables[0].Rows.Count; i++)
                                {
                                    string lName = ((string)lNDSDs.Tables[0].Rows[i].ItemArray[0]).Trim();
                                    string lCode = ((string)lNDSDs.Tables[0].Rows[i].ItemArray[1]).Trim();
                                    var lExists = content.Find(x => x.ProjectCode == lCode);
                                    if (lExists == null || lExists.ProjectCode == null || lExists.ProjectCode == "")
                                    {
                                        content.Add(new ProjectListModels
                                        {
                                            CustomerCode = pCustomerCode,
                                            ProjectCode = lCode,
                                            ProjectTitle = lName + " (" + lCode + ")"
                                        });
                                    }
                                }
                            }
                            #endregion

                            lNDSCmd.CommandText = "SELECT ProjectTitle, ProjectCode " +
                            "FROM dbo.OESProject P " +
                            "WHERE CustomerCode = '" + pCustomerCode + "' " +
                            "AND exists " +
                            "(SELECT ProjectCode FROM dbo.OESProjOrder " +
                            "WHERE CustomerCode = P.CustomerCode " +
                            "AND ProjectCode = P.ProjectCode " +
                            "AND (OrderStatus = 'Submitted' " +
                            "OR OrderStatus = 'Created*' " +
                            "OR OrderStatus = 'Submitted*' " +
                            "OR OrderStatus = 'Reviewed' " +
                            "OR OrderStatus = 'Production')) ";

                            lNDSCmd.Connection = lNDSCon;
                            lNDSDa.SelectCommand = lNDSCmd;
                            lNDSDs.Clear();
                            lNDSDa.Fill(lNDSDs);
                            if (lNDSDs.Tables[0].Rows.Count > 0)
                            {
                                for (int i = 0; i < lNDSDs.Tables[0].Rows.Count; i++)
                                {
                                    string lName = ((string)lNDSDs.Tables[0].Rows[i].ItemArray[0]).Trim();
                                    string lCode = ((string)lNDSDs.Tables[0].Rows[i].ItemArray[1]).Trim();
                                    var lExists = content.Find(x => x.ProjectCode == lCode);
                                    if (lExists == null || lExists.ProjectCode == null || lExists.ProjectCode == "")
                                    {
                                        content.Add(new ProjectListModels
                                        {
                                            CustomerCode = pCustomerCode,
                                            ProjectCode = lCode,
                                            ProjectTitle = lName + " (" + lCode + ")"
                                        });
                                    }
                                }
                            }

                            lNDSCmd.CommandText = "SELECT ProjectTitle, ProjectCode " +
                            "FROM dbo.OESProject, dbo.CustomerMaster " +
                            "WHERE vchCustomerNo = '" + pCustomerCode + "' " +
                            "AND CONTRY_KEY = country_code " +
                            "AND CustomerCode = '0000000000' AND ProjectCode <> '0000000000' ";

                            lNDSCmd.Connection = lNDSCon;
                            lNDSDa.SelectCommand = lNDSCmd;
                            lNDSDs.Clear();
                            lNDSDa.Fill(lNDSDs);
                            if (lNDSDs.Tables[0].Rows.Count > 0)
                            {
                                for (int i = 0; i < lNDSDs.Tables[0].Rows.Count; i++)
                                {
                                    string lName = ((string)lNDSDs.Tables[0].Rows[i].ItemArray[0]).Trim();
                                    string lCode = ((string)lNDSDs.Tables[0].Rows[i].ItemArray[1]).Trim();
                                    content.Add(new ProjectListModels
                                    {
                                        CustomerCode = pCustomerCode,
                                        ProjectCode = lCode,
                                        ProjectTitle = lName + " (" + lCode + ")"
                                    });
                                }
                            }

                            lProcess.CloseNDSConnection(ref lNDSCon);

                            //lDa = null;
                            //    lOCmd = null;
                            lDs = null;
                            lDStatus = null;
                            //lOcisCon = null;
                            lProcess = null;
                    }
                        
                }           
                else if (pUserType == "MJ")
                {
                    content = new List<ProjectListModels>();
                    var lCmdNDS = new SqlCommand();
                    SqlDataReader lRstNDS;
                    var lNDSCon = new SqlConnection();

                    var lProcessObj = new ProcessController();
                    if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
                    {
                        var lUserName = pGroupName.Trim();
                        if (lUserName.IndexOf("@") > 0)
                        {
                            lUserName = lUserName.Substring(0, lUserName.IndexOf("@"));
                        }

                        var lSQL = "SELECT P.CustomerCode, P.ProjectCode, P.ProjectTitle " +
                        "FROM dbo.OESProjectIncharges H, dbo.OESProject P " +
                        "WHERE P.ProjectCode = H.ProjectCode " +
                        "AND ((',' + Ltrim(RTRIM(DetailingIncharge)) + ',') LIKE '%," + lUserName + ",%' " +
                        "OR (',' + Ltrim(RTRIM(DetailingIncharge)) + ',') LIKE '%," + lUserName + " ,%' " +
                        "OR (',' + Ltrim(RTRIM(DetailingIncharge)) + ',') LIKE '%, " + lUserName + ",%' " +
                        "OR (',' + Ltrim(RTRIM(DetailingIncharge)) + ',') LIKE '%, " + lUserName + " ,%' " +
                        "OR (';' + Ltrim(RTRIM(DetailingIncharge)) + ';') LIKE '%;" + lUserName + ";%' " +
                        "OR (';' + Ltrim(RTRIM(DetailingIncharge)) + ';') LIKE '%;" + lUserName + " ;%' " +
                        "OR (';' + Ltrim(RTRIM(DetailingIncharge)) + ';') LIKE '%; " + lUserName + ";%' " +
                        "OR (';' + Ltrim(RTRIM(DetailingIncharge)) + ';') LIKE '%; " + lUserName + " ;%') " +
                        "ORDER BY P.ProjectTitle ";

                        lCmdNDS = new SqlCommand();
                        lCmdNDS.Connection = lNDSCon;
                        lCmdNDS.CommandText = lSQL;
                        lCmdNDS.CommandTimeout = 300;
                        lRstNDS = lCmdNDS.ExecuteReader();
                        if (lRstNDS.HasRows)
                        {
                            while (lRstNDS.Read())
                            {
                                var lCustomerCode = lRstNDS.GetValue(0) == DBNull.Value ? "" : lRstNDS.GetString(0).Trim();
                                var lProjectCode = lRstNDS.GetValue(1) == DBNull.Value ? "" : lRstNDS.GetString(1).Trim();
                                var lProjectTitle = lRstNDS.GetValue(2) == DBNull.Value ? "" : lRstNDS.GetString(2).Trim();
                                if (lCustomerCode != "" && lProjectCode != "" && lProjectTitle != "")
                                {
                                    content.Add(new ProjectListModels
                                    {
                                        CustomerCode = lCustomerCode.Trim(),
                                        ProjectCode = lProjectCode.Trim(),
                                        ProjectTitle = lProjectTitle.Trim()
                                    });
                                }
                            }
                        }
                        lRstNDS.Close();
                    }
                }
                else
                {
                    content = (from p in db.ProjectList
                               where p.CustomerCode == pCustomerCode &&
                               (from u in db.UserAccess
                                where u.UserName == lGroupName &&
                                u.CustomerCode == pCustomerCode
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
                    ProjectTitle = ""
                }
            };
                }

            }
            catch (Exception e)
            {
                lErrorMsg = e.Message;
            }

            return Ok(content);
        }


        [HttpGet]
        [Route("/GetCustomers/{pUserType}/{pGroupName}")]
        public async Task<IActionResult> GetCustomers(string pUserType, string pGroupName)
        {

            if (pUserType == "PL" || pUserType == "AD" || pUserType == "PM" || pUserType == "PA" || pUserType == "P1" || pUserType == "P2" || pUserType == "PU" || pUserType == "TE")
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

                return Ok(content1);
            }
            else if (pUserType == "MJ")
            {
                var content3 = new List<CustomerModels>();
                var lCmd = new SqlCommand();
                SqlDataReader lRst;
                var lNDSCon = new SqlConnection();

                var lProcessObj = new ProcessController();
                if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
                {
                    var lUserName = pGroupName.Trim();
                    if (lUserName.IndexOf("@") > 0)
                    {
                        lUserName = lUserName.Substring(0, lUserName.IndexOf("@"));
                    }
                    var lSQL = "SELECT C.vchCustomerNo, C.vchCustomername " +
                    "FROM dbo.OESProjectIncharges H, dbo.OESProject P, dbo.CustomerMaster C " +
                    "WHERE C.vchCustomerNo = P.CustomerCode " +
                    "AND P.ProjectCode = H.ProjectCode " +
                    "AND ((',' + Ltrim(RTRIM(DetailingIncharge)) + ',') LIKE '%," + lUserName + ",%' " +
                    "OR (',' + Ltrim(RTRIM(DetailingIncharge)) + ',') LIKE '%," + lUserName + " ,%' " +
                    "OR (',' + Ltrim(RTRIM(DetailingIncharge)) + ',') LIKE '%, " + lUserName + ",%' " +
                    "OR (',' + Ltrim(RTRIM(DetailingIncharge)) + ',') LIKE '%, " + lUserName + " ,%' " +
                    "OR (';' + Ltrim(RTRIM(DetailingIncharge)) + ';') LIKE '%;" + lUserName + ";%' " +
                    "OR (';' + Ltrim(RTRIM(DetailingIncharge)) + ';') LIKE '%;" + lUserName + " ;%' " +
                    "OR (';' + Ltrim(RTRIM(DetailingIncharge)) + ';') LIKE '%; " + lUserName + ";%' " +
                    "OR (';' + Ltrim(RTRIM(DetailingIncharge)) + ';') LIKE '%; " + lUserName + " ;%') ";

                    lCmd = new SqlCommand();
                    lCmd.Connection = lNDSCon;
                    lCmd.CommandText = lSQL;
                    lCmd.CommandTimeout = 300;
                    lRst = lCmd.ExecuteReader();
                    if (lRst.HasRows)
                    {
                        while (lRst.Read())
                        {
                            var lCustomerCode = lRst.GetValue(0) == DBNull.Value ? "" : lRst.GetString(0).Trim();
                            var lCustomerName = lRst.GetValue(1) == DBNull.Value ? "" : lRst.GetString(1).Trim();
                            if (lCustomerCode != "" && lCustomerName != "")
                            {
                                content3.Add(new CustomerModels
                                {
                                    CustomerCode = lCustomerCode.Trim(),
                                    CustomerName = lCustomerName.Trim()
                                });
                            }
                        }
                    }
                    lRst.Close();
                }

                content3 = content3.GroupBy(o => o.CustomerCode).Select(x => x.First()).ToList();


                return Ok(content3);

            }
            else
            {
                var content2 = (from p in db.Customer
                                where (from u in db.UserAccess
                                       where u.UserName == pGroupName &&
                                       p.CustomerCode != "0000000000"
                                       select u.CustomerCode).Contains(p.CustomerCode)
                                orderby p.CustomerName
                                select p).ToList();

                return Ok(content2);
            }
        }


        [HttpGet]
        [Route("/getGroupName/{UserName}")]
        public string getGroupName(string UserName)
        {
            var lErrorMsg = "";
            var lGroupName = UserName;
            var lUserType = "";
            try
            {
                var content = db.UserAccess.Find(new object[] { UserName, "0000000000", "0000000000" });
                if (content != null)
                {
                    lUserType = content.UserType.Trim();
                    if (lUserType == "P1")
                    {
                        lGroupName = "PMG1@natsteel.com.sg";
                    }
                    else if (lUserType == "P2")
                    {
                        lGroupName = "PMG2@natsteel.com.sg";
                    }
                    else if (lUserType == "P3")
                    {
                        lGroupName = "PMG3@natsteel.com.sg";
                    }
                    else if (lUserType == "P4")
                    {
                        lGroupName = "PMG4@natsteel.com.sg";
                    }

                }
                return lGroupName;
            }
            catch (Exception ex)
            {
                lErrorMsg = ex.Message;
            }
            return "";

        }

    //    [HttpGet]
    //    [Route("/GetSAPData")]
    //    public async Task<IActionResult> GetSAPData()
    //    {
    //        string lErrorMsg = "";
    //        var lCISCon = new OracleConnection();
    //        var lOraCmd = new OracleCommand();
    //        OracleDataReader lOraRst;

    //        var lCmd = new OracleCommand();
    //        var lcisCon = new OracleConnection();
    //        List<SAPTempData> content = new List<SAPTempData> {
    //    new SAPTempData
    //    {
    //        documentNumber = "",
    //        documentType = "",
    //    }
    //};

    //        try
    //        {
    //            var ldocumentNumber = "";
    //            var ldocumentType = "";

    //            content = new List<SAPTempData>();
    //            var lCmdNDS = new SqlCommand();
    //            SqlDataReader lRstNDS;
    //            var lNDSCon = new SqlConnection();

    //            var lProcessObj = new ProcessController();
    //            if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
    //            {
    //                var lSQL = "SELECT documentNumber, documentType FROM dbo.SAPDataInsert_Temp";

    //                lCmdNDS = new SqlCommand();
    //                lCmdNDS.Connection = lNDSCon;
    //                lCmdNDS.CommandText = lSQL;
    //                lCmdNDS.CommandTimeout = 300;
    //                lRstNDS = lCmdNDS.ExecuteReader();
    //                if (lRstNDS.HasRows)
    //                {
    //                    while (lRstNDS.Read())
    //                    {
    //                        ldocumentNumber = lRstNDS.GetValue(0) == DBNull.Value ? "" : lRstNDS.GetString(0).Trim();
    //                        ldocumentType = lRstNDS.GetValue(1) == DBNull.Value ? "" : lRstNDS.GetString(1).Trim();
    //                        if (ldocumentNumber != "" && ldocumentType != "")
    //                        {
    //                            content.Add(new SAPTempData
    //                            {
    //                                documentNumber = ldocumentNumber.Trim(),
    //                                documentType = ldocumentType.Trim(),
    //                            });
    //                        }
    //                    }
    //                }
    //                lRstNDS.Close();
    //            }

    //            var lProcess = new ProcessController();
    //            if (lProcess.OpenCISConnection(ref lCISCon) == true)
    //            {
    //                // Ensure the connection is open
    //                if (lCISCon.State != ConnectionState.Open)
    //                {
    //                    lCISCon.Open();
    //                }

    //                for (int i = 0; i < content.Count; i++)
    //                {
    //                    // Check if the record already exists
    //                    var checkQuery = "SELECT COUNT(1) FROM sapsr3.YMSDT_CUS_LOG " +
    //                                     "WHERE DOCU_NO = :DOCU_NO AND DOCU_TP = :DOCU_TP";

    //                    using (var checkCmd = new OracleCommand(checkQuery, lCISCon))
    //                    {
    //                        checkCmd.Parameters.Add(new OracleParameter(":DOCU_NO", content[i].documentNumber));
    //                        checkCmd.Parameters.Add(new OracleParameter(":DOCU_TP", content[i].documentType));

    //                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());

    //                        if (count == 0)
    //                        {
    //                            // Record doesn't exist, proceed with insert
    //                            using (var insertCmd = new OracleCommand())
    //                            {
    //                                insertCmd.Connection = lCISCon;
    //                                insertCmd.CommandText = @"
    //    INSERT INTO sapsr3.YMSDT_CUS_LOG 
    //    (MANDT, DOCU_NO, DOCU_TP, UPDT_DT, SENT_DT, STATUS, REMARK) 
    //    VALUES (:MANDT, :DOCU_NO, :DOCU_TP, TO_CHAR(SYSDATE, 'YYYYMMDD'), TO_CHAR(SYSDATE, 'YYYYMMDD'), :STATUS, :REMARK)";

    //                                insertCmd.Parameters.Add(new OracleParameter(":MANDT", "600"));
    //                                insertCmd.Parameters.Add(new OracleParameter(":DOCU_NO", content[i].documentNumber));
    //                                insertCmd.Parameters.Add(new OracleParameter(":DOCU_TP", content[i].documentType));

    //                                // Convert DateTime to string in 'YYYYMMDD' format
    //                                //string formattedDate = DateTime.Now.ToString("yyyyMMdd");
    //                                //insertCmd.Parameters.Add(new OracleParameter(":UPDT_DT", OracleDbType.Varchar2)).Value = formattedDate;

    //                                //// Assuming you want to set SENT_DT to current date
    //                                //insertCmd.Parameters.Add(new OracleParameter(":SENT_DT", OracleDbType.Varchar2)).Value = DateTime.Now.ToString("yyyyMMdd");

    //                                insertCmd.Parameters.Add(new OracleParameter(":STATUS", "P"));
    //                                insertCmd.Parameters.Add(new OracleParameter(":REMARK", " ")); // Use an empty string or proper value
    //                                insertCmd.ExecuteNonQuery();
    //                            }

    //                        }


    //                        else
    //                        {
    //                            // Record exists, proceed with update
    //                            using (var updateCmd = new OracleCommand())
    //                            {
    //                                updateCmd.Connection = lCISCon;
    //                                updateCmd.CommandText = "UPDATE sapsr3.YMSDT_CUS_LOG " +
    //                        "SET UPDT_DT = TO_CHAR(SYSDATE, 'YYYYMMDD'), " +
    //                        "STATUS = :STATUS " +
    //                        "WHERE DOCU_NO = :DOCU_NO AND DOCU_TP = :DOCU_TP";


    //                                //updateCmd.CommandText = "UPDATE sapsr3.YMSDT_CUS_LOG " +
    //                                //                        "SET UPDT_DT = '20241231', " +
    //                                //                        " SENT_DT = ' ' ," +
    //                                //                         " REMARK = ' ' " +
    //                                //                        "WHERE DOCU_NO = '1041730114' AND DOCU_TP = 'Signed DO'";

    //                                updateCmd.Parameters.Add(new OracleParameter(":STATUS", "P"));
    //                                updateCmd.Parameters.Add(new OracleParameter(":DOCU_NO", content[i].documentNumber));
    //                                updateCmd.Parameters.Add(new OracleParameter(":DOCU_TP", content[i].documentType));


    //                                updateCmd.ExecuteNonQuery();
    //                            }
    //                        }
    //                    }
    //                }

    //                // Close the connection after the loop
    //                lProcess.CloseCISConnection(ref lCISCon);
    //            }
    //            else
    //            {
    //                // Handle the case where the connection could not be opened
    //                throw new Exception("Failed to open CIS connection.");
    //            }
    //        }
    //        catch (Exception e)
    //        {
    //            throw e;
    //        }

    //        return Ok(content);
    //    }



        //[HttpGet]
        //[Route("/GetSAPDataByDocNoAndDocTy/{documentNumber}/{documentType}")]
        //public async Task<IActionResult> GetSAPDataByDocNoAndDocTy(string documentNumber, string documentType)
        //{
        //    OracleDataReader lRst;
        //    var lCmd = new OracleCommand();
        //    var lcisCon = new OracleConnection();
        //    string lReturn = "0,0,0,0";

        //    List<YMSDT_CUS_LOGDto> content = new List<YMSDT_CUS_LOGDto>
        //    {
        //        //new YMSDT_CUS_LOGDto
        //        //{
        //        //    MANDT = "",
        //        //    Docu_No = "",
        //        //    Docu_tp="",
        //        //    Updt_Dt=DateTime.Now,
        //        //    Sent_Dt=DateTime.Now,
        //        //    Status="",
        //        //    Remark=""
        //        //}
        //    };
        //    var lProcessObj = new ProcessController();
        //    if (lProcessObj.OpenCISConnection(ref lcisCon) == true)
        //    {
        //        string lMANDT = "";
        //        string lDocu_No = "";
        //        string lDocu_tp = "";
        //        DateTime? lUpdt_Dt = null; // Nullable DateTime
        //        DateTime? lSent_Dt = null; // Nullable DateTime
        //        string lStatus = "";
        //        string lRemark = "";
        //        lCmd.CommandText = "SELECT MANDT,Docu_No,Docu_tp,Updt_Dt,Sent_Dt,Status,Remark FROM sapsr3.YMSDT_CUS_LOG " +
        //            "WHERE Docu_No ='" + documentNumber + "' " +
        //            "AND Docu_tp='" + documentType + "' ";

        //        lCmd.Connection = lcisCon;
        //        lCmd.CommandTimeout = 1200;
        //        lRst = lCmd.ExecuteReader();
        //        if (lRst.HasRows)
        //        {
        //            while (lRst.Read())
        //            {
        //                lMANDT = lRst.GetValue(0) == DBNull.Value ? "" : lRst.GetString(0).Trim();
        //                lDocu_No = lRst.GetValue(1) == DBNull.Value ? "" : lRst.GetString(1).Trim();
        //                lDocu_tp = lRst.GetValue(2) == DBNull.Value ? "" : lRst.GetString(2).Trim();

        //                string updtDtRaw = lRst.GetValue(3) == DBNull.Value ? null : lRst.GetString(3).Trim();
        //                lUpdt_Dt = string.IsNullOrEmpty(updtDtRaw) || updtDtRaw == "00000000"
        //                    ? (DateTime?)null
        //                    : DateTime.ParseExact(updtDtRaw, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);

        //                string sentDtRaw = lRst.GetValue(4) == DBNull.Value ? null : lRst.GetString(4).Trim();
        //                lSent_Dt = string.IsNullOrEmpty(sentDtRaw) || sentDtRaw == "00000000"
        //                    ? (DateTime?)null
        //                    : DateTime.ParseExact(sentDtRaw, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);

        //                lStatus = lRst.GetValue(5) == DBNull.Value ? "" : lRst.GetString(5).Trim(); // Assuming index 5 for status
        //                lRemark = lRst.GetValue(6) == DBNull.Value ? "" : lRst.GetString(6).Trim(); // Assuming index 6 for remark

        //                content.Add(new YMSDT_CUS_LOGDto
        //                {
        //                    MANDT = lMANDT,
        //                    Docu_No = lDocu_No,
        //                    Docu_tp = lDocu_tp,
        //                    Updt_Dt = lUpdt_Dt,
        //                    Sent_Dt = lSent_Dt,
        //                    Status = lStatus,
        //                    Remark = lRemark
        //                });
        //            }


        //        }
        //        lRst.Close();

        //    }

        //    lProcessObj.CloseCISConnection(ref lcisCon);
        //    lCmd = null;
        //    lRst = null;
        //    lProcessObj = null;

        //    return Ok(content);
        //}


        #region Pricing Notification

        private bool SendEmailNotificationData(List<EmailNotificationDataDto> emailNotificationDataDtos)
        {
            OracleDataReader lRst;
            var lCmd = new OracleCommand();
            var lcisCon = new OracleConnection();
            try
            {

                var lProcess = new ProcessController();
                var EmailTo = "";
                for (int i = 0; i < emailNotificationDataDtos.Count; i++)
                {
                    var TempEmailTo = lProcess.GetEmailTo(emailNotificationDataDtos[i].SONUMBER);
                    if (TempEmailTo != "")
                    {
                        EmailTo = EmailTo + ";" + TempEmailTo;
                    }
                }

                if (EmailTo == "" || EmailTo == null)
                {
                    return false;
                }

                var lEmailContent = "";
                var lEmailFrom = "eprompt@natsteel.com.sg";
                var lEmailFromName = "Digital Ordering Email Services";
                var lEmailTo = "ajit.kamble@tatatechnologies.com"; // Replace with actual recipient//EmailTo;
                var lEmailCc = "kunal.ayer@tatatechnologies.com";//"csd1@natsteel.com.sg";//"kunal.ayer@tatatechnologies.com"; // Replace with actual CC
                var lEmailSubject = "Order created with Incompletion log";
                var lOESEmail = new SendGridEmail();
                lEmailContent = "<p align='center'>Order created with Incompletion log</p>";
                if (emailNotificationDataDtos.Count > 0)
                {
                    lEmailContent = lEmailContent + "<table border='1' width=100%><tr>";
                    lEmailContent = lEmailContent + "<td width='20%'><b>Customer</b></td>";

                    lEmailContent = lEmailContent + "<td>" + emailNotificationDataDtos[0].CustomerTitle + "</td></tr>";

                    lEmailContent = lEmailContent + "<tr><td><b>Project</b></td>";

                    lEmailContent = lEmailContent + "<td>" + emailNotificationDataDtos[0].ProjectTitle + "</td></tr></table>";

                    // Build email content as an HTML table
                    lEmailContent += "<table border='1' style='width:100%; border-collapse:collapse;'>";
                    lEmailContent += "<thead><tr>";
                    //lEmailContent += "<th>Customer Name</th>";
                    //lEmailContent += "<th>Project Name</th>";
                    lEmailContent += "<th>PONumber</th>";
                    lEmailContent += "<th>RequiredDelDate</th>";
                    lEmailContent += "<th>OrderDate</th>";
                    lEmailContent += "<th>SONumber</th>";
                    lEmailContent += "</tr></thead>";
                    lEmailContent += "<tbody>";

                    foreach (EmailNotificationDataDto row in emailNotificationDataDtos)
                    {
                        lEmailContent += "<tr>";
                        //lEmailContent += $"<td>{row.CustomerTitle}</td>";
                        //lEmailContent += $"<td>{row.ProjectTitle}</td>";
                        lEmailContent += $"<td>{row.PONUMBER}</td>";
                        lEmailContent += $"<td>{row.REQUIREDDELDATE}</td>";
                        lEmailContent += $"<td>{row.ORDERDATE}</td>";
                        lEmailContent += $"<td>{row.SONUMBER}</td>";
                        lEmailContent += "</tr>";
                    }
                    lEmailContent += "</tbody></table>";
                }
                string lEmailFromAddress = "eprompt@natsteel.com.sg";

                //dynamic response = lOESEmail.Execute(lEmailFromAddress, lEmailFromName, lEmailTo, lEmailCc, lEmailSubject, "text/html", lEmailContent);
                lOESEmail.Execute(lEmailFromAddress, lEmailFromName, lEmailTo, lEmailCc, lEmailSubject, "text/html", lEmailContent).Wait();
                lOESEmail = null;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }


    //    [HttpGet]
    //    [Route("/GetEmailNotificationData")]
    //    public async Task<IActionResult> GetEmailNotificationData()
    //    {
    //        var lReturn = true;
    //        var lCmd = new SqlCommand();
    //        SqlDataReader lRst;
    //        var lNDSCon = new SqlConnection();
    //        List<string> sorList = new List<string>(); // To store SOR values
    //        List<EmailNotificationDataDto> content = new List<EmailNotificationDataDto>();

    //        var lProcessObj = new ProcessController();

    //        // Step 1: Fetch SOR values from SQL Server
    //        if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
    //        {
    //            var lSQL = "SELECT Sales_Order FROM dbo.Incompletelog WHERE Status ='P'";
    //            lCmd = new SqlCommand
    //            {
    //                Connection = lNDSCon,
    //                CommandText = lSQL,
    //                CommandTimeout = 300
    //            };

    //            lRst = lCmd.ExecuteReader();
    //            if (lRst.HasRows)
    //            {
    //                while (lRst.Read())
    //                {
    //                    var lSOR = lRst.GetValue(0) == DBNull.Value ? "" : lRst.GetString(0).Trim();
    //                    sorList.Add(lSOR); // Add SOR to the list
    //                }
    //            }
    //            lRst.Close();
    //        }

    //        // Exit early if no SOR values are found
    //        if (sorList.Count == 0)
    //        {
    //            return Ok(new { Message = "No records found in Incompletelog table.", Data = content });
    //        }
    //        var lOESEmail = new SendGridEmail();

    //        #region Get from SAP
    //        var lDa = new OracleDataAdapter();
    //        var lOCmd = new OracleCommand();
    //        var lDs = new DataSet();
    //        var lOcisCon = new OracleConnection();
    //        var lProcess = new ProcessController();

    //        // Step 2: Convert SOR list to comma-separated string
    //        string sorListString = string.Join(",", sorList.Select(s => $"'{s}'")); // Ensures each value is enclosed in single quotes

    //        // Oracle query
    //        lOCmd.CommandText = $@"
    //SELECT 
    //    KUNAG AS CUSOTMERCODE,
    //    NAME_AG AS CUSTOMERNAME,
    //    KUNNR AS PPROJECTCODE,
    //    NAME_WE AS PROJECTNAME,
    //    PO_NUMBER AS PONUMBER,
    //    REQD_DEL_DATE AS REQUIREDDELDATE,
    //    CUST_ORDER_DATE AS ORDERDATE,
    //    SALES_ORDER AS SONUMBER
    //FROM 
    //    SAPSR3.YMSDT_ORDER_HDR
    //WHERE 
    //    SALES_ORDER IN ({sorListString})";

    //        if (lProcess.OpenCISConnection(ref lOcisCon) == true)
    //        {
    //            lOCmd.Connection = lOcisCon;
    //            lOCmd.CommandTimeout = 300;
    //            lDa.SelectCommand = lOCmd;
    //            lDs.Clear();
    //            lDa.Fill(lDs);
    //            foreach (DataRow row in lDs.Tables[0].Rows)
    //            {
    //                // Add to the response content list
    //                content.Add(new EmailNotificationDataDto
    //                {
    //                    CUSOTMERCODE = $"{row["CUSOTMERCODE"]}",
    //                    PPROJECTCODE = $"{row["PPROJECTCODE"]}",
    //                    CustomerTitle = $"{row["CUSTOMERNAME"]} ({row["CUSOTMERCODE"]})",
    //                    ProjectTitle = $"{row["PROJECTNAME"]} ({row["PPROJECTCODE"]})",
    //                    PONUMBER = row["PONUMBER"].ToString().Trim(),
    //                    REQUIREDDELDATE = row["REQUIREDDELDATE"].ToString().Trim(),
    //                    ORDERDATE = row["ORDERDATE"].ToString().Trim(),
    //                    SONUMBER = row["SONUMBER"].ToString().Trim()
    //                });
    //            }

    //            lProcess.CloseCISConnection(ref lOcisCon);
    //        }
    //        else
    //        {
    //            lReturn = false;
    //        }

    //        lDa = null;
    //        lOCmd = null;
    //        lDs = null;
    //        lOcisCon = null;
    //        lProcess = null;
    //        #endregion
    //        if (lReturn == true)
    //        {
    //            List<dynamic> lTempList = new List<dynamic>();

    //            for (int i = 0; i < content.Count(); i++)
    //            {
    //                var lItem = content[i];
    //                lTempList.Add(new
    //                {
    //                    CUSOTMERCODE = lItem.CUSOTMERCODE,
    //                    PPROJECTCODE = lItem.PPROJECTCODE,
    //                    ProjectTitle = lItem.ProjectTitle,
    //                    CustomerTitle = lItem.CustomerTitle
    //                });
    //            }//all data
    //            List<dynamic> query = lTempList.DistinctBy(p => new { p.CUSOTMERCODE, p.PPROJECTCODE }).ToList();//distict
    //            List<dynamic> lTempObj = new List<dynamic>();
    //            for (int i = 0; i < query.Count; i++)
    //            {
    //                List<EmailNotificationDataDto> TempObj = new List<EmailNotificationDataDto>();
    //                for (int j = 0; j < content.Count; j++)
    //                {
    //                    if (query[i].CUSOTMERCODE == content[j].CUSOTMERCODE && query[i].PPROJECTCODE == content[j].PPROJECTCODE)
    //                    {
    //                        TempObj.Add(content[j]);
    //                    }
    //                }
    //                //Send the list for send email
    //                if (SendEmailNotificationData(TempObj))
    //                {
    //                    //Update stauts for particular records using SONumber
    //                    UpdateStatusOfLog(TempObj);
    //                }
    //                else
    //                {
    //                    lTempObj.Add(query[i]);
    //                }
    //            }

    //            if (lTempObj.Count != 0)
    //            {
    //                //var json = new JavaScriptSerializer().Serialize(lTempObj);
    //                //SaveErrorMsg(ex.Message, ex.StackTrace);
    //                var ListMessage = "Email Not Sent Incomplete Log for following Customer And Project";
    //                for(int i = 0; i < lTempObj.Count; i++)
    //                {
    //                    ListMessage = ListMessage + "," + lTempObj[i].CUSOTMERCODE +";"+ lTempObj[i].PPROJECTCODE;
    //                }
    //                lProcessObj.SaveErrorMsg_Pricing(ListMessage, "");
    //                return Ok(false);
    //            }

    //        }
    //        return Ok(lReturn);
    //    }


        [HttpPost]
        [Route("/UpdateStatusOfLog")]
        public async Task<IActionResult> UpdateStatusOfLog([FromBody] List<EmailNotificationDataDto> emailNotificationDataDtos)
        {

            var lCmd = new SqlCommand();
            SqlDataReader lRst;
            var lNDSCon = new SqlConnection();
            List<string> sorList = new List<string>(); // To store SOR values
            List<EmailNotificationDataDto> content = new List<EmailNotificationDataDto>();
            string SOR = "";
            for (int i = 0; i < emailNotificationDataDtos.Count; i++)
            {
                if (i == 0)
                {
                    SOR = "('" + emailNotificationDataDtos[i].SONUMBER + "'";
                }
                else
                {
                    SOR = SOR + ", '" + emailNotificationDataDtos[i].SONUMBER + "'";
                }
            }
            SOR = SOR + ")";
            var lProcessObj = new ProcessController();

            // Step 1: Fetch SOR values from SQL Server
            if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
            {
                var lSQL = "Update dbo.Incompletelog set Status='C' WHERE Sales_Order in " + SOR + "";
                lCmd = new SqlCommand
                {
                    Connection = lNDSCon,
                    CommandText = lSQL,
                    CommandTimeout = 300
                };

                lRst = lCmd.ExecuteReader();

                lRst.Close();
            }
            #endregion
            return Ok();
        }


        [HttpGet]
        [Route("/GetHmiAddress/{projectcodes}")]
        public async Task<IActionResult> GetHmiAddress(string projectcodes)
        {
            List<HmiProjectAddress> addressList = new List<HmiProjectAddress>();
            SqlConnection lNDSCon = null;

            var lProcessObj = new ProcessController();

            // Split comma-separated project codes
            var projectCodeArray = projectcodes.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).ToList();

            if (projectCodeArray.Count == 0)
                return Ok(addressList);

            // Create parameter names dynamically: @p0, @p1, @p2...
            var parameterNames = projectCodeArray.Select((code, index) => $"@p{index}").ToList();

            // SQL with IN clause
            string lSQL = $"SELECT DISTINCT project_address_code, project_address FROM HMIProjectAddress WHERE project_code IN ({string.Join(",", parameterNames)})";

            if (lProcessObj.OpenNDSConnection(ref lNDSCon))
            {
                using (lNDSCon)
                using (var lCmd = new SqlCommand(lSQL, lNDSCon))
                {
                    lCmd.CommandTimeout = 300;

                    // Add parameters safely
                    for (int i = 0; i < projectCodeArray.Count; i++)
                    {
                        lCmd.Parameters.AddWithValue($"@p{i}", projectCodeArray[i]);
                    }

                    using (var lRst = lCmd.ExecuteReader())
                    {
                        while (lRst.Read())
                        {
                            HmiProjectAddress lObj = new HmiProjectAddress();

                            lObj.id = lRst.IsDBNull(0) ? "" : lRst.GetString(0).Trim();
                            lObj.projectAddress = lRst.IsDBNull(1)
                                ? ""
                                : lRst.GetString(1).Trim();

                            addressList.Add(lObj);
                        }
                    }
                }
            }

            return Ok(addressList);
        }




    }
}
