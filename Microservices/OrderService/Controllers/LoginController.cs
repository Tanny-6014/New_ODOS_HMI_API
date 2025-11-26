using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;

using OrderService.Models;
using System.Data.SqlClient;

namespace OrderService.Controllers
{
    public class LoginController : Controller
    {
        private DBContextModels db = new DBContextModels();

        [HttpGet]
        [Route("/GetCustomersByLoginUser")]
        public ActionResult GetCustomersByLoginUser()
        {
            
        var lCmd = new SqlCommand();
            SqlDataReader lRst;
            var lNDSCon = new SqlConnection();
            string lSQL = "";

            //var lEmail = new SendGridEmail();
            //lEmail.Execute("zbc@natsteelcom.sg", "DigiOSNoReply", "zbc@natsteelcom.sg", "", "Testing", "text/html", "Test...").Wait();

            string lCustomerCode = "";
            string lProjectCode = "";

            string lUserName = User.Identity.GetUserName();

            UserAccessController lUa = new UserAccessController();
            var lUserType = lUa.getUserType(User.Identity.GetUserName());
            var lGroupName = lUa.getGroupName(User.Identity.GetUserName());

            ViewBag.UserType = lUserType;

            //if (lUserName.IndexOf("@") > 0)
            //{
            //    lUserName = lUserName.Substring(0, lUserName.IndexOf("@"));
            //}
            ViewBag.UserName = lUserName;

            lUa = null;

            SharedAPIController lBackEnd = new SharedAPIController();

            var lCustSelectList = lBackEnd.getCustomerSelectList(lCustomerCode, lUserType, lGroupName);

            ViewBag.CustomerSelection = lCustSelectList;

            if (lCustomerCode.Length == 0)
            {
                if (lCustSelectList == null || lCustSelectList.Count() == 0)
                {
                    return Redirect("/Home/RegNeed");
                }

                lCustomerCode = lCustSelectList.First().Value;
                if (lCustomerCode == null)
                {
                    lCustomerCode = "";
                }
            }

            var lProjSelectList = lBackEnd.getProjectSelectList(lCustomerCode, lProjectCode, lUserType, lGroupName);
            ViewBag.ProjectSelection = lProjSelectList;

            if (lProjectCode.Length == 0)
            {
                lProjectCode = lProjSelectList.First().Value;
                if (lProjectCode == null)
                {
                    lProjectCode = "";
                }
            }

            lBackEnd = null;

            var lSubmission = "No";
            var lEditable = "No";

            //get Access right;
            if (lUserType == "CU" || lUserType == "CA" || lUserType == "CM")
            {
                var lAccess = db.UserAccess.Find(User.Identity.Name, lCustomerCode, lProjectCode);
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

            string lLeadTimeProdType = "";
            string lLeadTime = "";
            string lHolidays = "";

            var lProcessObj = new ProcessController();
            lProcessObj.OpenNDSConnection(ref lNDSCon);

            lSQL = "SELECT ProductType, LeadTime FROM dbo.OESLeadTime ORDER BY ProductType ";
            lCmd.CommandText = lSQL;
            lCmd.Connection = lNDSCon;
            lCmd.CommandTimeout = 300;
            lRst = lCmd.ExecuteReader();
            if (lRst.HasRows)
            {
                while (lRst.Read())
                {
                    lLeadTimeProdType = lLeadTimeProdType + "," + lRst.GetString(0);
                    lLeadTime = lLeadTime + "," + lRst.GetInt32(1).ToString();
                }
                lLeadTimeProdType = lLeadTimeProdType.Substring(1);
                lLeadTime = lLeadTime.Substring(1);
            }
            lRst.Close();

            lSQL = "SELECT Holiday FROM dbo.OESPublicHolidays ORDER BY Holiday ";
            lCmd.CommandText = lSQL;
            lCmd.Connection = lNDSCon;
            lCmd.CommandTimeout = 300;
            lRst = lCmd.ExecuteReader();
            if (lRst.HasRows)
            {
                while (lRst.Read())
                {
                    lHolidays = lHolidays + "," + lRst.GetString(0);
                }
                lHolidays = lHolidays.Substring(1);
            }
            lRst.Close();

            lProcessObj.CloseNDSConnection(ref lNDSCon);
            lProcessObj = null;

            ViewBag.LeadTimeProdType = lLeadTimeProdType;
            ViewBag.LeadTime = lLeadTime;
            ViewBag.Holidays = lHolidays;

            ViewBag.AlertMessage = new List<string>();
            //var lSharedPrg = new SharedAPIController();
            //ViewBag.AlertMessage = lSharedPrg.getAlertMessage(lCustomerCode, lProjectCode, lUserName, lSubmission, lEditable);
            //lSharedPrg = null;

            //if (lUserType == "MJ")
            //{
            //    return RedirectToAction("Drawings", "DrawingRepository");
            //}
            //else
            //{
            //    return View();
            //}//commented by ajit
            return Ok(lCustSelectList);

        }
         
    }
}
