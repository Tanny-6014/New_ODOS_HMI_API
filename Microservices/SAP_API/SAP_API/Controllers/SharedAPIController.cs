
using SAP_API.Models;
using SAP_API.Modelss;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;



namespace SAP_API.Controllers
{
    public class SharedAPIController : Controller
    {
        private DBContextModels db = new DBContextModels();

        public SelectList getCustomerSelectList(string pCustomerCode, string pUserType, string pGroupName)
        {

            SelectList lSelectList = null;


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
                content1.Insert(0, new CustomerModels
                {
                    CustomerCode = "",
                    CustomerName = ""
                });

                lSelectList = new SelectList(new List<SelectListItem>(content1.Select(h => new SelectListItem
                {
                    Value = h.CustomerCode.Trim(),
                    Text = h.CustomerName.Trim() + (h.CustomerCode.Trim() == "" ? "" : (" (" + h.CustomerCode.Trim() + ")")),
                })), "Value", "Text", pCustomerCode);

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

                lSelectList = new SelectList(new List<SelectListItem>(content3.Select(h => new SelectListItem
                {
                    Value = h.CustomerCode,
                    Text = h.CustomerName
                })), "Value", "Text", pCustomerCode);


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

                var content3 = new List<CustomerModels>(content2.Select(h => new CustomerModels
                {
                    CustomerCode = h.CustomerCode.Trim(),
                    CustomerName = h.CustomerName.Trim()
                }));

                content3 = content3.GroupBy(o => o.CustomerCode).Select(x => x.First()).ToList();

                lSelectList = new SelectList(new List<SelectListItem>(content3.Select(h => new SelectListItem
                {
                    Value = h.CustomerCode,
                    Text = h.CustomerName
                })), "Value", "Text", pCustomerCode);


            }


            return lSelectList;
        }

        //public List<CustomerModels> getCustomer(string pUserType, string pGroupName)
        //{

        //    if (pUserType == "PL" || pUserType == "AD" || pUserType == "PM" || pUserType == "PA" || pUserType == "P1" || pUserType == "P2" || pUserType == "PU" || pUserType == "TE")
        //    {
        //        var content1 = (from p in db.Customer
        //                        where !p.CustomerName.StartsWith("(D)") &&
        //                        !p.CustomerName.Contains("DONOT USE") &&
        //                        !p.CustomerName.Contains("DONT USE") &&
        //                        !p.CustomerName.StartsWith("-CANCEL-") &&
        //                        !p.CustomerCode.Trim().Equals("") &&
        //                        !p.CustomerName.Trim().Equals("") &&
        //                        !p.CustomerName.Trim().Equals(".")
        //                        orderby p.CustomerName
        //                        select p
        //                       ).ToList();

        //        return content1;
        //    }
        //    else if (pUserType == "MJ")
        //    {
        //        var content3 = new List<CustomerModels>();
        //        var lCmd = new SqlCommand();
        //        SqlDataReader lRst;
        //        var lNDSCon = new SqlConnection();

        //        var lProcessObj = new ProcessController();
        //        if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
        //        {
        //            var lUserName = pGroupName.Trim();
        //            if (lUserName.IndexOf("@") > 0)
        //            {
        //                lUserName = lUserName.Substring(0, lUserName.IndexOf("@"));
        //            }
        //            var lSQL = "SELECT C.vchCustomerNo, C.vchCustomername " +
        //            "FROM dbo.OESProjectIncharges H, dbo.OESProject P, dbo.CustomerMaster C " +
        //            "WHERE C.vchCustomerNo = P.CustomerCode " +
        //            "AND P.ProjectCode = H.ProjectCode " +
        //            "AND ((',' + Ltrim(RTRIM(DetailingIncharge)) + ',') LIKE '%," + lUserName + ",%' " +
        //            "OR (',' + Ltrim(RTRIM(DetailingIncharge)) + ',') LIKE '%," + lUserName + " ,%' " +
        //            "OR (',' + Ltrim(RTRIM(DetailingIncharge)) + ',') LIKE '%, " + lUserName + ",%' " +
        //            "OR (',' + Ltrim(RTRIM(DetailingIncharge)) + ',') LIKE '%, " + lUserName + " ,%' " +
        //            "OR (';' + Ltrim(RTRIM(DetailingIncharge)) + ';') LIKE '%;" + lUserName + ";%' " +
        //            "OR (';' + Ltrim(RTRIM(DetailingIncharge)) + ';') LIKE '%;" + lUserName + " ;%' " +
        //            "OR (';' + Ltrim(RTRIM(DetailingIncharge)) + ';') LIKE '%; " + lUserName + ";%' " +
        //            "OR (';' + Ltrim(RTRIM(DetailingIncharge)) + ';') LIKE '%; " + lUserName + " ;%') ";

        //            lCmd = new SqlCommand();
        //            lCmd.Connection = lNDSCon;
        //            lCmd.CommandText = lSQL;
        //            lCmd.CommandTimeout = 300;
        //            lRst = lCmd.ExecuteReader();
        //            if (lRst.HasRows)
        //            {
        //                while (lRst.Read())
        //                {
        //                    var lCustomerCode = lRst.GetValue(0) == DBNull.Value ? "" : lRst.GetString(0).Trim();
        //                    var lCustomerName = lRst.GetValue(1) == DBNull.Value ? "" : lRst.GetString(1).Trim();
        //                    if (lCustomerCode != "" && lCustomerName != "")
        //                    {
        //                        content3.Add(new CustomerModels
        //                        {
        //                            CustomerCode = lCustomerCode.Trim(),
        //                            CustomerName = lCustomerName.Trim()
        //                        });
        //                    }
        //                }
        //            }
        //            lRst.Close();
        //        }

        //        content3 = content3.GroupBy(o => o.CustomerCode).Select(x => x.First()).ToList();


        //        return content3;

        //    }
        //    else
        //    {
        //        var content2 = (from p in db.Customer
        //                        where (from u in db.UserAccess
        //                               where u.UserName == pGroupName &&
        //                               p.CustomerCode != "0000000000"
        //                               select u.CustomerCode).Contains(p.CustomerCode)
        //                        orderby p.CustomerName
        //                        select p).ToList();

        //        return content2;
        //    }

        //}

        public SelectList getProjectSelectList(string pCustomerCode, string pProjectCode, string pUserType, string pGroupName)
        {
            if (pCustomerCode == null)
            {
                pCustomerCode = "";
            }
            pCustomerCode = pCustomerCode.Trim();

            if (pProjectCode == null)
            {
                pProjectCode = "";
            }
            pProjectCode = pProjectCode.Trim();

            SelectList lSelectList = null;

            //var lPorjList = getProject(pCustomerCode, pUserType, pGroupName);

            //if (lPorjList.Count == 0)
            //{
            //    lPorjList = new List<ProjectListModels> {
            //    new ProjectListModels
            //    {
            //        CustomerCode = "",
            //        ProjectCode = "",
            //        ProjectTitle = ""
            //    } };

            //}

            //lSelectList = new SelectList(new List<SelectListItem>(lPorjList.Select(h => new SelectListItem
            //{
            //    Value = h.ProjectCode,
            //    Text = h.ProjectTitle
            //})), "Value", "Text", pProjectCode);



            return lSelectList;
        }


        
    }
}
