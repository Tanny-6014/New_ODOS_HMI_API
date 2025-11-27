using AutoMapper;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.Data.SqlClient;
using OrderService.Dtos;
using OrderService.Interfaces;
using Oracle.ManagedDataAccess.Client;
using Microsoft.AspNet.Identity;
using System.Data.SqlClient;
using System.Net;
using System.Globalization;
using OrderService.Models;
using SixLabors.ImageSharp.ColorSpaces;
using OrderService.Repositories;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.Security.Policy;
using System.Data;
using MongoDB.Bson;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Net.Http.Headers;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UpcomingOrdersController : Controller
    {
        public string strServer = "DEV";
        private readonly IOrder _OrderRepository;
        private readonly IMapper _mapper;

        private DBContextModels db = new DBContextModels();

        //public UpcomingOrdersController(IOrder orderService, IMapper mapper)
        //{
        //    _OrderRepository = orderService;
        //    _mapper = mapper;
        //}

        private string strCIS_Connection = "";
        private string strNDS_Connection = "";
        private string strIDB_Connection = "";
        private string strSTS_Connection = "";
        private string strNGW_Connection = "";
        //public UpcomingOrdersController(IOrder orderService, IMapper mapper)
        //{
        //    _OrderRepository = orderService;
        //    _mapper = mapper;

        //}

        //List of raised PO
        //List of raised PO
        //List of raised PO
        [HttpGet]
        [Route("/getUpcomingOrders/{pCustomerCode}/{pProjectCode}")]
        public async Task<List<UpcomingOrderListDto>> getUpcomingOrders(string pCustomerCode, string pProjectCode)
        {


            var lCmd = new SqlCommand();
            SqlDataReader lRst;
            var lNDSCon = new SqlConnection();
            //var lReturn = (new[]{ new
            //{
            //    SSNNo = 0,
            //    OrderNo = 0,
            //    WBS1 = "",
            //    WBS2 = "",
            //    WBS3 = "",
            //    StructureElement = "",
            //    ProdType = "",
            //    PONo = "",
            //    UpdateDate = "",
            //    RequiredDate = "",
            //    OrderWeight = "",
            //    CustomerCode = "",
            //    ProjectCode = "",
            //    ProjectTitle = ""
            //}}).ToList();
            // var lReturn = new List<dynamic>();
            var lReturn = new List<UpcomingOrderListDto>();
            if (lReturn.Count > 0)
            {
                lReturn.RemoveAt(0);
            }

            lCmd.CommandText =
            "SELECT " +
            "M.OrderNumber, " +
            "M.WBS1, " +
            "M.WBS2, " +
            "M.WBS3, " +
            "M.StructureElement, " +
            "M.ProductType, " +
            "M.LowerPONumber, " +
            "M.ForecastDate, " +
            "M.DeliveryDate, " +
            "M.FloorTonnage, " +
            "M.BBSNo, " +
            "M.BBSDesc, " +
            "M.ConvertedOrderNo, " +
            "M.OrderStatus, " +
            "M.NotifiedByEmail, " +
            "M.NotifiedEmailId, " +
            "M.NotifiedEmailDate, " +
             "M.ConvertOrderDate, " +
              "M.ConvertedOrderBy, " +
            "M.CustomerCode, " +
            "M.ProjectCode " +
            "FROM dbo.OESUpcomingOrder M Where M.CustomerCode = '" + pCustomerCode + "' and M.ProjectCode='" + pProjectCode + "' and M.OrderStatus='Y'";
            ;

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
                        // lReturn.Add(new
                        var upcomingOrder = new UpcomingOrderListDto
                        {
                            SSNNo = lSNo,
                            OrderNumber = lRst.GetInt32(0),
                            WBS1 = (lRst.GetValue(1) == DBNull.Value ? "" : lRst.GetString(1).Trim()),
                            WBS2 = (lRst.GetValue(2) == DBNull.Value ? "" : lRst.GetString(2).Trim()),
                            WBS3 = (lRst.GetValue(3) == DBNull.Value ? "" : lRst.GetString(3).Trim()),
                            StructureElement = (lRst.GetValue(4) == DBNull.Value ? "" : (lRst.GetString(4).Trim() == "NONWBS" ? "" : lRst.GetString(4).Trim())),
                            ProdType = (lRst.GetValue(5) == DBNull.Value ? "" : lRst.GetString(5).Trim()),
                            PONo = (lRst.GetValue(6) == DBNull.Value ? "" : (lRst.GetString(6).Trim().Length > 0 && lRst.GetString(6).Trim().Substring(0, 1) == "," ? lRst.GetString(6).Trim().Substring(1) : lRst.GetString(6).Trim())),
                            ForecastDate = (lRst.GetValue(7) == DBNull.Value ? "" : lRst.GetDateTime(7).ToString("yyyy-MM-dd")),
                            //ForecastDate = lRst.IsDBNull(7) ? (DateTime?)null : lRst.GetDateTime(7),
                            DeliveryDate = (lRst.GetValue(8) == DBNull.Value ? "" : lRst.GetDateTime(8).ToString("yyyy-MM-dd")),
                            //DeliveryDate = lRst.IsDBNull(8) ? (DateTime?)null : lRst.GetDateTime(8),
                            //FloorTonnage = (lRst.GetValue(9) == DBNull.Value ? "0.000" : (lRst.GetDecimal(9) / 1000).ToString("###,###,##0.000;(###,##0.000);")),//
                            FloorTonnage = lRst.IsDBNull(9) ? 0 : lRst.GetDecimal(9) / 1000,
                            BBSNo = (lRst.GetValue(10) == DBNull.Value ? "" : lRst.GetString(10).Trim()),
                            BBSDesc = (lRst.GetValue(11) == DBNull.Value ? "" : lRst.GetString(11).Trim()),
                            ConvertedOrderNo = (lRst.GetValue(12) == DBNull.Value ? 0 : lRst.GetInt32(12)),
                            OrderStatus = (lRst.GetValue(13) == DBNull.Value ? "" : lRst.GetString(13).Trim()),
                            NotifiedByEmail = (lRst.GetValue(14) == DBNull.Value ? "" : lRst.GetString(14).Trim()),
                            NotifiedEmailId = (lRst.GetValue(15) == DBNull.Value ? "" : lRst.GetString(15).Trim()),
                            //NotifiedEmailDate = (lRst.GetValue(16) == DBNull.Value ? "" : lRst.GetDateTime(16).ToString("yyyy-MM-dd")),
                            NotifiedEmailDate = lRst.IsDBNull(16) ? (DateTime?)null : lRst.GetDateTime(16),
                            ConvertOrderDate = (lRst.GetValue(18) == DBNull.Value ? "" : lRst.GetDateTime(17).ToString("yyyy-MM-dd")),
                            //ConvertOrderDate = lRst.IsDBNull(17) ? (DateTime?)null : lRst.GetDateTime(17),

                            ConvertedOrderBy = (lRst.GetValue(18) == DBNull.Value ? "" : lRst.GetString(18).Trim()),

                            CustomerCode = (lRst.GetValue(19) == DBNull.Value ? "" : lRst.GetString(19).Trim()),
                            ProjectCode = (lRst.GetValue(20) == DBNull.Value ? "" : lRst.GetString(20).Trim())
                        };
                        lReturn.Add(upcomingOrder);
                    }
                }
                lRst.Close();

                lProcessObj.CloseNDSConnection(ref lNDSCon);
            }
            lProcessObj = null;

            lCmd = null;
            lNDSCon = null;
            lRst = null;

            return lReturn;
        }

        [HttpGet]
        [Route("/DeleteUpcoming/{OrderNumber}")]
        public async Task<IActionResult> DeleteUpcoming(string OrderNumber)
        {
            var response = await _OrderRepository.DeleteUpcomingOrder(OrderNumber);
            return Ok(new { result = response, response = "success" });
        }

        [HttpGet]
        [Route("/UpdateConvertedOrder/{pOrderNumber}/{nOrderNumber}/{LoginUser}")]
        public async Task<IActionResult> UpdateConvertedOrder(string pOrderNumber, string nOrderNumber, string LoginUser)
        {
            var response = await _OrderRepository.ConvertedUpcomingOrder(pOrderNumber, nOrderNumber, LoginUser);
            return Ok(new { result = response, response = "success" });
        }

        [HttpGet]
        [Route("/GetSubmitByEmail/{OrderNumber}")]
        public ActionResult GetSubmitByEmail(string OrderNumber)
        {
            var lDa = new SqlDataAdapter();
            var lCmd = new SqlCommand();
            var lDs = new DataSet();
            var lNDSCon = new SqlConnection();
            SqlDataReader lRst;

            var lSubmitBy = "";

            var lProcess = new ProcessController();
            lProcess.OpenNDSConnection(ref lNDSCon);
            if (lNDSCon.State == ConnectionState.Open)
            {
                var lSQL = "SELECT TOP 1 SubmitBy FROM OESPROJORDER WHERE ORDERNUMBER= '" + OrderNumber + "' ";

                lCmd.CommandText = lSQL;
                lCmd.Connection = lNDSCon;
                lCmd.CommandTimeout = 300;
                lRst = lCmd.ExecuteReader();
                if (lRst.HasRows)
                {
                    if (lRst.Read())
                    {
                        if (!lRst.IsDBNull(0)) // Check if the value is not null
                        {
                            lSubmitBy = lRst.GetString(0);
                        }
                        else
                        {
                            lSubmitBy = "";
                        }

                        lProcess.CloseNDSConnection(ref lNDSCon);
                    }
                    lDa = null;
                    lCmd = null;
                    lDs = null;
                    lNDSCon = null;
                }
            }
            return Json(new { SubmitBy = lSubmitBy });
        }

        [HttpPost]
        [Route("/UpcomingMail")]
        public ActionResult UpcomingMail([FromBody] EmailNotificationDto emailNotificationDto)
        {
            string EmailTo = emailNotificationDto.EmailTo;
            string ProjectCode = emailNotificationDto.ProjectCode;
            string CustomerCode = emailNotificationDto.CustomerCode;
            //string OrderNumber = (emailNotificationDto.OrderNumber);
            //int OrderNumbers =  emailNotificationDto.OrderNumber;
            List<int> OrderNumbers = emailNotificationDto.OrderNumber;
            string EmailBy = emailNotificationDto.EmailBy;
            var lEmailObj = new SendGridEmailUpcoming();
            //foreach (var orderNumber in OrderNumbers)
            //{
            //    lEmailObj.SendOrderActionEmail1(EmailTo, CustomerCode, ProjectCode, orderNumber);
            //}

            var lReturn = true;
            try
            {
                lEmailObj.SendOrderActionEmail1(EmailTo, CustomerCode, ProjectCode, OrderNumbers);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                lReturn = false;
                return Ok(false);
            }

            if (lReturn == true)
            {
                UpdateUpcomingMailData(EmailTo, OrderNumbers, EmailBy);
                InsertNotificationData(EmailTo, OrderNumbers, EmailBy);
            }
            lEmailObj = null; //commented by ajit
            return Ok(true);

        }


        ActionResult UpdateUpcomingMailData(string pEmailTo, List<int> pOrderNumbers,string pEmailBy)
        {
            //Current logic will override the values of Emailby, Emailto & EmailDate in the table.

            //string pEmailBy = "Ajit.Kamble@tatatechnologies.com";

            var lCmd = new SqlCommand();
            SqlDataReader lRst;
            var lNDSCon = new SqlConnection();

            string lOrderNumber = string.Join(",", pOrderNumbers.Select(order => $"'{order}'"));

            lCmd.CommandText =
                "UPDATE OESUpcomingOrder SET " +
                "NotifiedByEmail = '" + pEmailBy + "', " +
                "NotifiedEmailId = '" + pEmailTo + "', " +
                "NotifiedEmailDate = GETDATE() " +
                "WHERE OrderNumber in (" + lOrderNumber + ")"
                ;

            var lProcessObj = new ProcessController();
            if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
            {
                lCmd.Connection = lNDSCon;
                lCmd.CommandTimeout = 300;
                lRst = lCmd.ExecuteReader();

                lRst.Close();
                lProcessObj.CloseNDSConnection(ref lNDSCon);
            }

            lProcessObj = null;
            lCmd = null;
            lNDSCon = null;
            lRst = null;

            return Ok(true);
        }

        ActionResult InsertNotificationData(string pEmailTo, List<int> pOrderNumbers, string pEmailBy)
        {
            //Current logic will override the values of Emailby, Emailto & EmailDate in the table.

            //string pEmailBy = "Ajit.Kamble@tatatechnologies.com";
            var lCmd = new SqlCommand();
            SqlDataReader lRst;
            var lNDSCon = new SqlConnection();

            for(var i=0;i< pOrderNumbers.Count; i++)
            {

             int lOrderNumber = pOrderNumbers[i]; //string.Join(",", pOrderNumbers.Select(order => $"'{order}'"));

            lCmd.CommandText =
                //"UPDATE OESUpcomingOrder SET " +
                //"NotifiedByEmail = '" + pEmailBy + "', " +
                //"NotifiedEmailId = '" + pEmailTo + "', " +
                //"NotifiedEmailDate = GETDATE() " +
                //"WHERE OrderNumber in (" + lOrderNumber + ")"
                //;

            "INSERT INTO dbo.OESUpcomingNotificationMail " +
                                               "(OrderNo " +
                                               ",EmailTo " +
                                               ",Date " +
                                                 ",EmailBy)" +
                                                "VALUES " +
                                                "('" + lOrderNumber + "' " +
                                                ",'" + pEmailTo + "' " +
                                                   ",  GETDATE()  " +
                                                      ",'" + pEmailBy + "' " +
                                                ")";
            var lProcessObj = new ProcessController();
            if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
            {
                lCmd.Connection = lNDSCon;
                lCmd.CommandTimeout = 300;
                lRst = lCmd.ExecuteReader();

                lRst.Close();
                lProcessObj.CloseNDSConnection(ref lNDSCon);
            }

            lProcessObj = null;
            }

            lCmd = null;
            lNDSCon = null;
            lRst = null;
            return Ok(true);
        }

        [HttpPost]
        [Route("/LoadDataForUpcoming")]
        public async Task<IActionResult> LoadDataForUpcoming(LoadDataForUpcoming loadDataForUpcoming)
        {
            //string customerCode = loadDataForUpcoming.customerCode;
            //    string projectCode = loadDataForUpcoming.projectCode;
            //var response = await _OrderRepository.LoadDataFor_UpcomingOrder(customerCode, projectCode);
            //return Ok(response);
            IEnumerable<UpcomingJobAdviceListDto> orderServiceList = await _OrderRepository.LoadDataFor_UpcomingOrder(loadDataForUpcoming.customerCode, loadDataForUpcoming.projectCode);
            var result = _mapper.Map<List<UpcomingJobAdviceListDto>>(orderServiceList);
            var lIDBCon = new OracleConnection();
            //var lCISCon = new OracleConnection();
            var lNDSCon = new SqlConnection();

            ProcessController controller = new ProcessController();

            OpenIDBConnection(ref lIDBCon);
            OpenNDSConnection(ref lNDSCon);
            var lTempList = await controller.addForeCastDateUpcoming(result, lNDSCon, lIDBCon);

            for (int i = 0; i < lTempList.Count; i++)
            {
                var temp = lTempList[i];
                //temp.ForecastDate=DateTime.Now;//Added by ajit for timebeing
                if (temp.ForecastDate == null)
                {
                    continue;
                }
                else
                {
                    //INSERT TO UPCOMINGORDER TABLE
                    InsertToUpcomingTable(temp);
                }
            }


            return Ok(result);
        }

        async Task<IActionResult> InsertToUpcomingTable(UpcomingJobAdviceListDto pItem)
        {
            try
            {
                InsertDataToUpcomingOrders(pItem);
                return Ok(true);
            }
            catch (Exception)
            {
                return Ok(false);
                //throw ex;
            }
            
        }

        [HttpPost]
        [Route("/InsertDataToUpcomingOrders")]
        public async Task<IActionResult> InsertDataToUpcomingOrders([FromBody] UpcomingJobAdviceListDto insertDataUpcomingOrdersDto)
        {
            try
            {
                string errorMessage = "";
                if (insertDataUpcomingOrdersDto != null)
                {
                    UpcomingOrderDto upcomingorderDto = new UpcomingOrderDto();
                    upcomingorderDto.OrderNumber = insertDataUpcomingOrdersDto.OrderNumber;
                    upcomingorderDto.OrderType = insertDataUpcomingOrdersDto.OrderType;
                    upcomingorderDto.CustomerCode = insertDataUpcomingOrdersDto.CustomerCode;
                    upcomingorderDto.ProjectCode = insertDataUpcomingOrdersDto.ProjectCode;
                    upcomingorderDto.WBS1 = insertDataUpcomingOrdersDto.WBS1;
                    upcomingorderDto.WBS2 = insertDataUpcomingOrdersDto.WBS2;
                    upcomingorderDto.WBS3 = insertDataUpcomingOrdersDto.WBS3;
                    upcomingorderDto.StructureElement = insertDataUpcomingOrdersDto.StructureElement;
                    upcomingorderDto.ProductType = insertDataUpcomingOrdersDto.ProductType;
                    upcomingorderDto.ForecastDate = insertDataUpcomingOrdersDto?.ForecastDate;
                    upcomingorderDto.DeliveryDate = insertDataUpcomingOrdersDto?.DeliveryDate;
                    upcomingorderDto.LowerPONumber = insertDataUpcomingOrdersDto.LowerPONumber;
                    upcomingorderDto.BBSNo = insertDataUpcomingOrdersDto.BBSNo;
                    upcomingorderDto.BBSDesc = insertDataUpcomingOrdersDto.BBSDesc;
                    upcomingorderDto.FloorTonnage = insertDataUpcomingOrdersDto.FloorTonnage;
                    upcomingorderDto.ConvertedOrderNo = insertDataUpcomingOrdersDto.ConvertedOrderNo;
                    upcomingorderDto.OrderStatus = insertDataUpcomingOrdersDto.OrderStatus;
                    upcomingorderDto.NotifiedByEmail = insertDataUpcomingOrdersDto.NotifiedByEmail;
                    upcomingorderDto.NotifiedEmailId = insertDataUpcomingOrdersDto.NotifiedEmailId;
                    upcomingorderDto.NotifiedEmailDate = insertDataUpcomingOrdersDto?.NotifiedEmailDate;

                    IEnumerable<UpcomingJobAdviceListDto> Columnproduct = await _OrderRepository.InsertIntoUpcomingOrderData(insertDataUpcomingOrdersDto);

                    if (errorMessage == "")
                    {
                        var result = Columnproduct;
                        return Ok(result);
                    }
                    else
                    {
                        return BadRequest(errorMessage);
                    }
                }
                else
                {
                    errorMessage = "Please contact System Administrator.";
                    return BadRequest(errorMessage);
                }


            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }



        [HttpGet]
        [Route("/DeleteSubmittedOrder/{COrderNumber}")]
        public async Task<IActionResult>DeleteSubmittedOrder(string COrderNumber)
        {
            var response = await _OrderRepository.DeletedSubmittedUpcomingOrder(COrderNumber);
            return Ok(new { result = response, response = "success" });
        }

        [HttpGet]
        [Route("/OpenCISConnection1")]
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

        [HttpGet]
        [Route("/OpenIDBConnection1")]
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
               // SaveErrorMsg(ex.Message, ex.StackTrace);
                string lErrorMsg = ex.Message;
                return false;
            }

            return true;
        }

        [HttpGet]
        [Route("/OpenNDSConnection1")]
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


        [HttpGet]
        [Route("/SetConnectString1")]
        public Boolean SetConnectString()
        {
            if (strServer == "PRD")
            {
                strNDS_Connection = "Data Source=NSPRDDB19\\MSSQL2022;Initial Catalog=ODOS;User ID=ndswebapps; Password=DBAdmin4*NDS; MultipleActiveResultSets=false; Connection Timeout=120";
                strCIS_Connection = "Data Source=(DESCRIPTION = (ADDRESS = (COMMUNITY = SAP.WORLD)(PROTOCOL = TCP)(HOST = 172.25.101.1)(PORT = 1527)) (CONNECT_DATA = (SID = NSP) (SERVER = DEDICATED) (SERVICE_NAME = NSP)));Persist Security Info=True;User ID=ORAINTSAP;Password=ORAintsap01;Connect Timeout=300";
                strIDB_Connection = "Data Source=(DESCRIPTION = (ENABLE=BROKEN)(ADDRESS = (COMMUNITY = IDBPROD)(PROTOCOL = TCP)(HOST = nsprdscmlsnr.natsteel.corp)(PORT = 1521)) (CONNECT_DATA = (SERVER = DEDICATED) (SERVICE_NAME = IDBPROD)));Persist Security Info=True;User ID=SCM_ADMIN;Password=scm4admin; Connection Timeout=300";
                strSTS_Connection = "Data Source=NSDB11,1433;Initial Catalog=STS;User ID=stsapp; Password=stsapp123; Connection Timeout=120";
                strNGW_Connection = "Data Source=NSDB11,1433;Initial Catalog=NGWS;User ID=stsapp; Password=stsapp123; Connection Timeout=120";


                //strCIS_Connection = "Data Source=(DESCRIPTION = (ADDRESS = (COMMUNITY = SAP.WORLD)(PROTOCOL = TCP)(HOST = 10.168.101.177)(PORT = 1527)) (CONNECT_DATA = (SID = NSP) (SERVER = DEDICATED) (SERVICE_NAME = NSP)));Persist Security Info=True;User ID=ORAINTSAP;Password=ORAintsap01;";
                //strCIS_Connection = "Data Source=(DESCRIPTION = (ADDRESS = (COMMUNITY = SAP.WORLD)(PROTOCOL = TCP)(HOST = 172.25.101.1)(PORT = 1527)) (CONNECT_DATA = (SID = NSP) (SERVER = DEDICATED) (SERVICE_NAME = NSP)));Persist Security Info=True;User ID=ORAINTSAP;Password=ORAintsap01;Connect Timeout=300";
                //strNDS_Connection = "Provider=sqloledb; Data Source=NSPRDDB19\\MSSQL2022;Initial Catalog=ODOS;User ID=ndswebapps; Password=DBAdmin4*NDS";
                //strNDS_Connection = "Provider=sqloledb; Data Source=NSPRDDB19\\MSSQL2022;Initial Catalog=ODOS;User ID=ndswebapps; Password=DBAdmin4*NDS";
                //strNDS_Connection = "Data Source=NSPRDDB19\\MSSQL2022;Initial Catalog=ODOS;User ID=ndswebapps; Password=DBAdmin4*NDS; MultipleActiveResultSets=false; Connection Timeout=120";
                //strIDB_Connection = "Data Source=(DESCRIPTION = (ADDRESS = (COMMUNITY = IDBPROD)(PROTOCOL = TCP)(HOST = nsprdscmlsnr.natsteel.corp)(PORT = 1521)) (CONNECT_DATA = (SERVER = DEDICATED) (SERVICE_NAME = IDBPROD)));Persist Security Info=True;User ID=SCM_ADMIN;Password=scm4admin;";
                //strIDB_Connection = "Data Source=(DESCRIPTION = (ADDRESS = (COMMUNITY = IDBPROD)(PROTOCOL = TCP)(HOST = nsprdscmvdb01)(PORT = 1521)) (CONNECT_DATA = (SERVER = DEDICATED) (SERVICE_NAME = IDBPROD)));Persist Security Info=True;User ID=SCM_ADMIN;Password=scm4admin; Connection Timeout=300";
                //strIDB_Connection = "Data Source=(DESCRIPTION = (ENABLE=BROKEN)(ADDRESS = (COMMUNITY = IDBPROD)(PROTOCOL = TCP)(HOST = nsprdscmlsnr.natsteel.corp)(PORT = 1521)) (CONNECT_DATA = (SERVER = DEDICATED) (SERVICE_NAME = IDBPROD)));Persist Security Info=True;User ID=SCM_ADMIN;Password=scm4admin; Connection Timeout=300";
                //strIDB_Connection = "Data Source=(DESCRIPTION = (ADDRESS = (COMMUNITY = SCMIDBQA)(PROTOCOL = TCP)(HOST = NSQADB18.natsteel.corp)(PORT = 1521)) (CONNECT_DATA = (SERVER = DEDICATED) (SERVICE_NAME = SCMIDBQA)));Persist Security Info=True;User ID=SCM_ADMIN;Password=scm4admin;Connection Timeout=300";
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
                //strNDS_Connection = "Provider=sqloledb; Data Source=NSPRDDB19\\MSSQL2022;Initial Catalog=NDSPRD;User ID=ots_support; Password=ots1nqa";
                strNDS_Connection = "Data Source=NSPRDDB19\\MSSQL2022; Initial Catalog=ODOS;User ID=ndswebapps; Password=DBAdmin4*NDS;MultipleActiveResultSets=false; Connection Timeout=120";
                //strIDB_Connection = "Data Source=(DESCRIPTION = (ADDRESS = (COMMUNITY = SCMIDBQA)(PROTOCOL = TCP)(HOST = 172.18.1.134)(PORT = 1525)) (CONNECT_DATA = (SERVER = DEDICATED) (SERVICE_NAME = SCMIDBQA)));Persist Security Info=True;User ID=SCM_ADMIN;Password=scm4admin;Connection Timeout=300";
                strIDB_Connection = "Data Source=(DESCRIPTION = (ADDRESS = (COMMUNITY = SCMIDBQA)(PROTOCOL = TCP)(HOST = NSQADB18.natsteel.corp)(PORT = 1521)) (CONNECT_DATA = (SERVER = DEDICATED) (SERVICE_NAME = SCMIDBQA)));Persist Security Info=True;User ID=SCM_ADMIN;Password=scm4admin;Connection Timeout=300";
                strSTS_Connection = "Data Source=NSQADB2,1433;Initial Catalog=STS;User ID=stsapp; Password=stsapp123";
                strNGW_Connection = "Data Source=NSQADB2,1433;Initial Catalog=NGWS;User ID=stsapp; Password=stsapp123";

            }

            return true;
        }

        [HttpGet]
        [Route("/CloseCISConnection1")]
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

        [HttpGet]
        [Route("/CloseIDBConnection1")]
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

        [HttpGet]
        [Route("/CloseNDSConnection1")]
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

        [HttpGet]
        [Route("/UpcomingNotificationMail/{COrderNumber}")]
        public async Task<IActionResult> UpcomingNotificationMail(string COrderNumber)
        {
            var response = await _OrderRepository.UpcomingNotificationMail(COrderNumber);
            return Ok(new { result = response, response = "success" });
        }

        [HttpGet]
        [Route("/GetUserNameUpcoming/{pCustomerCode}/{pProjectCode}")]
        public async Task<ActionResult> GetUserNameUpcoming(string pCustomerCode, string pProjectCode)
        {
            var lCmd = new SqlCommand();
            SqlDataReader lRst;
            var lNDSCon = new SqlConnection();
      
            var lReturn = new List<dynamic>();

            if (lReturn.Count > 0)
            {
                lReturn.RemoveAt(0);
            }

            lCmd.CommandText =
            "SELECT " +
            "M.UserName " +
            "FROM dbo.OESUSERACCESS M Where M.CustomerCode = '" + pCustomerCode + "' and M.ProjectCode='" + pProjectCode + "'";
            

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
                        lReturn.Add(lRst.GetValue(0) == DBNull.Value ? "" : lRst.GetString(0).Trim());
                        
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

        [HttpPost]
        //[ValidateAntiForgeryHeader]
        [Route("/ExportUpcoming")]
        public async Task<ActionResult> ExportUpcoming([FromBody] ExportUpcomingDto exportUpcomingDto)
        {

            string CustomerCode = exportUpcomingDto.CustomerCode;
            List<string> ProjectCode = exportUpcomingDto.ProjectCode;
            List<string> pColumnsID = exportUpcomingDto.pColumnsID;
            List<string> pColumnName = exportUpcomingDto.pColumnName;
            List<double> pColumnSize = exportUpcomingDto.pColumnSize;
            //bool Forecast = exportUpcomingDto.Forecast;
            string UserName = exportUpcomingDto.UserName;

            //var tReturn = new List<UpcomingOrderDto>();
            var lReturn = new List<UpcomingOrderListDto>();
            for (var i=0;i<ProjectCode.Count();i++)
            {
               var tReturn = await getUpcomingOrders(CustomerCode, ProjectCode[i]);
                lReturn.AddRange(tReturn);
            }

            ExcelPackage package = new ExcelPackage();
            ExcelWorksheet ws = package.Workbook.Worksheets.Add("Upcoming" + " ORDER LIST");

            ws.PrinterSettings.PaperSize = ePaperSize.A3;
            ws.PrinterSettings.BlackAndWhite = true;
            ws.PrinterSettings.Orientation = eOrientation.Landscape;

            int lRowNo = 1;
            int lRation = 1;

            //for (int j = 0; j < pColumnsID.Count; j++)
            //{
            //    if (pColumnsID[j] == "id")
            //    {
            //        lRation = (int)Math.Round(pColumnSize[j] / 5, 0);
            //        break;
            //    }
            //}

            //if (lRation < 1)
            //{
            //    lRation = 1;
            //}

            //for (int j = 0; j < pColumnsID.Count; j++)
            //{
            //    if (pColumnsID[j] != "linkTo")
            //    {
            //        ws.Column(j + 1).Width = pColumnSize[j] / lRation;
            //    }
            //}


            ws.Row(lRowNo).Height = 30;


            for (int j = 0; j < pColumnsID.Count; j++)
            {
                if (pColumnsID[j] != "linkTo")
                {
                    ws.Cells[lRowNo, j + 1].Value = pColumnName[j];
                }
            }

            for (int j = 0; j < pColumnsID.Count; j++)
            {
                ws.Cells[lRowNo, j + 1].Style.WrapText = true;
            }

            for (int j = 0; j < pColumnsID.Count; j++)
            {
                ws.Cells[lRowNo, j + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            }

            for (int j = 0; j < pColumnsID.Count; j++)
            {
                ws.Cells[lRowNo, j + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
            }

            for (int j = 0; j < pColumnsID.Count; j++)
            {
                ws.Cells[lRowNo, j + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#CCCCCC"));
            }

           
                lRowNo = 2;
                for (int i = 0; i < lReturn.Count; i++)
                {
                    for (int j = 0; j < pColumnsID.Count; j++)
                    {
                        ws.Cells[i + lRowNo, j + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    }

                    for (int j = 0; j < pColumnsID.Count; j++)
                    {
                        var lColID = pColumnsID[j];
                        if (lColID != "linkTo")
                        {
                            if (lColID == "id")
                            {
                                ws.Cells[i + lRowNo, j + 1].Value = i + 1;
                            }
                        else
                        {
                            string lValue = lReturn[i].GetType().GetProperty(lColID).GetValue(lReturn[i], null) == null ? "" : lReturn[i].GetType().GetProperty(lColID).GetValue(lReturn[i], null).ToString();
                            if (lValue == null)
                            {
                                lValue = "";
                            }
                            ws.Cells[i + lRowNo, j + 1].Value = lValue;
                        }
                            //else
                            //{
                            //    string lValue = lReturn[i].GetType().GetProperty(lColID).GetValue(lReturn[i], null) == null ? "" : lReturn[i].GetType().GetProperty(lColID).GetValue(lReturn[i], null).ToString();
                            //    if (lValue == null)
                            //    {
                            //        lValue = "";
                            //    }
                            //    if (lColID == "CustomerName")
                            //    {
                            //        lValue = lValue + " (" + lReturn[i].GetType().GetProperty("CustomerCode").GetValue(lReturn[i], null).ToString() + ")";
                            //    }
                            //    if (lColID == "ProjectTitle")
                            //    {
                            //        lValue = lValue + " (" + lReturn[i].GetType().GetProperty("ProjectCode").GetValue(lReturn[i], null).ToString() + ")";
                            //    }
                            //    if (lColID.ToUpper().Contains("WEIGHT"))
                            //    {
                            //        double lDValue = -1;
                            //        double.TryParse(lValue, out lDValue);
                            //        if (lDValue > 0)
                            //        {
                            //            lValue = lDValue.ToString("F3");
                            //        }
                            //        else if (lDValue == 0)
                            //        {
                            //            lValue = lDValue.ToString();
                            //        }
                            //    }
                            //    if (lColID.ToUpper().Contains("DATE"))
                            //    {
                            //        if (lValue.Length > 10)
                            //        {
                            //            lValue = lValue.Substring(0, 10);
                            //        }
                            //        var lDateA = lValue.Split(' ');
                            //        if (lDateA.Length >= 1 && lDateA[0] != null && lDateA[0] != "")
                            //        {
                            //            lValue = lDateA[0];
                            //        }
                            //    }

                            //    double lDoubleV = 0;
                            //    if (double.TryParse(lValue, out lDoubleV) &&
                            //        (lColID.ToUpper().IndexOf("WEIGHT") >= 0 || lColID.ToUpper().IndexOf("TotalWeight") >= 0 ||
                            //        lColID.ToUpper().IndexOf("TOTAL_MT") >= 0 || lColID.ToUpper().IndexOf("PCS") >= 0))
                            //    {
                            //        ws.Cells[i + lRowNo, j + 1].Value = lDoubleV;
                            //    }
                            //    else
                            //    {
                            //        DateTime lDateTValue = DateTime.Now;
                            //        if (DateTime.TryParse(lValue, out lDateTValue))
                            //        {
                            //            ws.Cells[i + lRowNo, j + 1].Value = lDateTValue;
                            //            ws.Cells[i + lRowNo, j + 1].Style.Numberformat.Format = "yyyy-mm-dd"; ;
                            //        }
                            //        else
                            //        {
                            //            ws.Cells[i + lRowNo, j + 1].Value = lValue;
                            //        }
                            //    }
                            //}
                        }
                    }
                }

            
            ws.Cells.AutoFitColumns();
            for (int j = 0; j < pColumnsID.Count; j++)
            {
                ws.Column(j + 1).AutoFit(8, 50);
            }


            MemoryStream ms = new MemoryStream();
            package.SaveAs(ms);

            var bExcel = new byte[ms.Length];
            ms.Position = 0;
            ms.Read(bExcel, 0, bExcel.Length);

            //bPDF = ms.GetBuffer();
            ms.Flush();
            ms.Dispose();
            var fileName = "abc";
            using (MemoryStream memoryStream = new MemoryStream(bExcel)) // Replace bExcelData with your Excel data byte array
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StreamContent(memoryStream);
                // Set content disposition with the appropriate file name and extension for Excel
                response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                response.Content.Headers.ContentDisposition.FileName = fileName + ".xlsx"; // Change the file extension to .xlsx for Excel
                                                                                           // Set the content type to Excel
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"); // Use the appropriate MIME type for Excel
                return File(response.Content.ReadAsByteArrayAsync().Result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"); // Use the appropriate MIME type for Excel
            }
            // return Json(bExcel, JsonRequestBehavior.AllowGet);
            //return Ok(bExcel);

        }




    }
}
