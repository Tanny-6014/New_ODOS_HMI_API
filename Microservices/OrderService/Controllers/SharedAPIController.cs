using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.AspNet.Identity;

using Oracle.ManagedDataAccess.Client;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Text;
using OrderService.Repositories;
using NCalc;
using OrderService.Models;
using OrderService.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.SharePoint.Client;
using OrderService.Dtos;

namespace OrderService.Controllers
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


        public List<ProjectListModels> getProject(string pCustomerCode, string pUserType, string pGroupName)
        {
            // string lUserType = pUserType;
            string lUserType = "PL"; //for Testing CSS

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

                    //    lProcess.CloseCISConnection(ref lOcisCon);

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

            return content;
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
                            //lProjectObj.createProject(pCustomerCode, pProjectCode);
                            //  lProjectObj = null;
                        }

                        #region Get from NDS
                        SqlDataReader lRst;
                        var lOCmd = new SqlCommand();
                        var lONDSCon = new SqlConnection();
                        var lProcess = new ProcessController();
                        lOCmd.CommandText = @"
                            SELECT (project_name + project_name2) AS SHIP_TO_NAME
                            FROM HMIProjectMaster
                            WHERE  project_code = '" + pProjectCode + @"'
                        ";


                        if (lProcess.OpenNDSConnection(ref lONDSCon) == true)
                        {
                            lOCmd.Connection = lONDSCon;
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

                            lProcess.CloseNDSConnection(ref lONDSCon);

                        }
                        lOCmd = null;
                        lONDSCon = null;
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

        //List<ProjectListModels> RemoveNonCABProject_Oracle(List<ProjectListModels> pInputProj)
        //{
        //    List<ProjectListModels> lReturn = new List<ProjectListModels>();
        //    string lProjectCode = "";

        //    OracleDataReader lRst;
        //    var lCmd = new OracleCommand();
        //    var lcisCon = new OracleConnection();

        //    if (pInputProj.Count() > 0)
        //    {
        //        var lProcessObj = new ProcessController();
        //        if (lProcessObj.OpenCISConnection(ref lcisCon) == true)
        //        {
        //            var lSQL = "";
        //            for (int i = 0; i < pInputProj.Count(); i++)
        //            {
        //                if (lSQL == "")
        //                {
        //                    lSQL = " P.KUNNR = '" + pInputProj[i].ProjectCode + "' ";
        //                }
        //                else
        //                {
        //                    lSQL = lSQL + " OR " + "P.KUNNR = '" + pInputProj[i].ProjectCode + "' ";
        //                }
        //            }


        //            lCmd.CommandText = "SELECT distinct P.KUNNR " +
        //            "FROM SAPSR3.VBAK K, SAPSR3.VBPA P " +
        //            "WHERE K.MANDT = '" + lProcessObj.strClient + "' " +
        //            "AND (K.VBELN like '102%' OR K.VBELN like '_102%' OR K.VBELN like '112%' OR K.VBELN like '_112%') " +
        //            "AND P.MANDT = '" + lProcessObj.strClient + "' " +
        //            "AND (K.VKORG = '" + lProcessObj.strSalesOrg + "' " +
        //            "OR K.VKORG = '" + lProcessObj.strSalesExport + "') " +
        //            "AND K.KUNNR = '" + pInputProj[0].CustomerCode + "' " +
        //            "AND K.TRVOG = '4' " +
        //            "AND (K.ytot_cab > 0 " +
        //            "OR K.ytot_rebar > 0) " +
        //            "AND k.VBELN = P.VBELN " +
        //            "AND ( " + lSQL + " ) ";

        //            lCmd.Connection = lcisCon;
        //            lCmd.CommandTimeout = 1200;
        //            lRst = lCmd.ExecuteReader();
        //            if (lRst.HasRows)
        //            {
        //                while (lRst.Read())
        //                {
        //                    lProjectCode = lRst.GetString(0);

        //                    for (int i = 0; i < pInputProj.Count(); i++)
        //                    {
        //                        if (pInputProj[i].ProjectCode.Trim() == lProjectCode.Trim())
        //                        {
        //                            lReturn.Add(pInputProj[i]);
        //                        }
        //                    }

        //                }
        //            }
        //            lRst.Close();


        //            lProcessObj.CloseCISConnection(ref lcisCon);
        //        }
        //        lCmd = null;
        //        lRst = null;
        //        lProcessObj = null;
        //    }

        //    return lReturn;
        //}

        //private int orderStatusProcess(string CustomerCode, string ProjectCode)
        //{
        //    var lReturn = 0;
        //    var lCmd = new SqlCommand();
        //    var lDA = new SqlDataAdapter();

        //    SqlDataReader lRst;
        //    var lNDSCon = new SqlConnection();

        //    var lCISCon = new OracleConnection();
        //    var lOraCmd = new OracleCommand();
        //    OracleDataReader lOraRst;

        //    var lSOStr = "";
        //    var lSQL = "";
        //    var lSO = new List<string>();
        //    var lJob = new List<int>();
        //    var lBBSIDs = new List<int>();
        //    var lTotalWT = new List<decimal>();
        //    var lJobID = 0;
        //    var lBBSID = 0;
        //    var lSONo = "";
        //    var lPlanDate = "";
        //    var lVehicalType = "";
        //    var lVehicalID = "";
        //    var lTrailerNo = "";
        //    var lDelStatus = "";
        //    var lLoadQty = 0;
        //    decimal lLoadWT = 0;
        //    var lOutTime = "";
        //    var lLoadNo = "";
        //    int lCageID = 0;
        //    int lLoadID = 0;

        //    var lProcessObj = new ProcessController();
        //    if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
        //    {
        //        //Standard MESH
        //        lSO = new List<string>();
        //        lJob = new List<int>();
        //        lBBSIDs = new List<int>();
        //        lTotalWT = new List<decimal>();
        //        lSOStr = "";

        //        lCmd.CommandText = "DELETE " +
        //        "FROM dbo.OESStdSheetDelivery " +
        //        "WHERE CustomerCode = '" + CustomerCode + "' " +
        //        "AND ProjectCode = '" + ProjectCode + "' " +
        //        "AND Delivery_Status <> 'Delivered' ";

        //        lCmd.Connection = lNDSCon;
        //        lCmd.CommandTimeout = 300;
        //        lCmd.ExecuteNonQuery();

        //        lCmd = new SqlCommand();
        //        lCmd.CommandText = "SELECT SAPSONo, JobID, TotalWeight " +
        //        "FROM dbo.OESStdSheetJobAdvice A " +
        //        "WHERE A.CustomerCode = '" + CustomerCode + "' " +
        //        "AND A.ProjectCode = '" + ProjectCode + "' " +
        //        "AND A.SAPSONo > '' " +
        //        "AND A.TotalPcs > (" +
        //        "SELECT isnull(SUM(LoadQty), 0) " +
        //        "FROM dbo.OESStdSheetDelivery " +
        //        "WHERE CustomerCode = A.CustomerCode " +
        //        "AND ProjectCode = A.ProjectCode " +
        //        "AND JobID = A.JobID " +
        //        "AND Delivery_Status = 'Delivered') ";

        //        lCmd.Connection = lNDSCon;
        //        lCmd.CommandTimeout = 300;
        //        lRst = lCmd.ExecuteReader();
        //        if (lRst.HasRows)
        //        {
        //            while (lRst.Read())
        //            {
        //                lSO.Add(lRst.GetValue(0) == DBNull.Value ? "" : lRst.GetString(0).Trim());
        //                lJob.Add(lRst.GetValue(1) == DBNull.Value ? 0 : lRst.GetInt32(1));
        //                lTotalWT.Add(lRst.GetValue(2) == DBNull.Value ? 0 : lRst.GetDecimal(2));

        //                if (lSOStr == "")
        //                {
        //                    lSOStr = "'" + lRst.GetString(0).Trim() + "'";
        //                }
        //                else
        //                {
        //                    lSOStr = lSOStr + ",'" + lRst.GetString(0).Trim() + "'";
        //                }
        //            }
        //        }
        //        lRst.Close();

        //        if (lSOStr != "")
        //        {
        //            lCmd = new SqlCommand();
        //            lCmd.CommandText = "DELETE " +
        //            "FROM dbo.OESStdSheetDelivery " +
        //            "WHERE CustomerCode = '" + CustomerCode + "' " +
        //            "AND ProjectCode = '" + ProjectCode + "' " +
        //            "AND SONumber in ( " + lSOStr + ") ";

        //            lCmd.Connection = lNDSCon;
        //            lCmd.CommandTimeout = 300;
        //            lCmd.ExecuteNonQuery();

        //            lProcessObj.OpenCISConnection(ref lCISCon);

        //            lSQL = "SELECT  " +
        //            "H.LOAD_NO, " +
        //            "H.PLAN_DELIV_DATE, " +
        //            "H.VEHICLE_TYPE, " +
        //            "H.TRAILER_NO, " +
        //            "H.LP_STATUS, " +
        //            "C.SALES_ORDER, " +
        //            "NVL(SUM(C.PLAN_LOAD_QNTY), 0), " +
        //            "NVL(SUM(C.NO_PIECES), 0), " +
        //            "NVL(MAX(V.VEHICLE_ID), ' '), " +
        //            "NVL(MAX(V.WEIGH_OUT_TIME), ' ') " +
        //            "FROM (SAPSR3.YMPPT_LP_HDR H " +
        //            "INNER JOIN SAPSR3.YMPPT_LP_ITEM_C C " +
        //            "ON H.LOAD_NO = C.LOAD_NO) " +
        //            "LEFT OUTER JOIN SAPSR3.YMPPT_LOAD_VEHIC V " +
        //            "ON C.MANDT = V.MANDT " +
        //            "AND C.SALES_ORDER = V.VBELN " +
        //            "AND C.CLBD_ITEM = V.POSNR " +
        //            "WHERE H.LP_STATUS <> 'X' " +
        //            "AND C.SALES_ORDER IN (" + lSOStr + ") " +
        //            "GROUP BY " +
        //            "H.LOAD_NO, " +
        //            "H.PLAN_DELIV_DATE, " +
        //            "H.VEHICLE_TYPE, " +
        //            "H.TRAILER_NO, " +
        //            "H.LP_STATUS, " +
        //            "C.SALES_ORDER ";

        //            lOraCmd.CommandText = lSQL;
        //            lOraCmd.Connection = lCISCon;
        //            lOraCmd.CommandTimeout = 300;
        //            lOraRst = lOraCmd.ExecuteReader();
        //            if (lOraRst.HasRows)
        //            {
        //                while (lOraRst.Read())
        //                {
        //                    lLoadNo = lOraRst.GetString(0).Trim();
        //                    lPlanDate = lOraRst.GetString(1).Trim();
        //                    lVehicalType = lOraRst.GetString(2).Trim();
        //                    lTrailerNo = lOraRst.GetString(3).Trim();
        //                    lDelStatus = lOraRst.GetString(4).Trim();
        //                    lSONo = lOraRst.GetString(5);
        //                    lLoadWT = lOraRst.GetDecimal(6);
        //                    lLoadQty = lOraRst.GetInt32(7);
        //                    lVehicalID = lOraRst.GetString(8).Trim();
        //                    lOutTime = lOraRst.GetString(9).Trim();

        //                    if (lPlanDate != "20500101")
        //                    {
        //                        if (lLoadQty == 0)
        //                        {
        //                            lLoadQty = (int)lLoadWT;
        //                            lLoadWT = 0;
        //                        }

        //                        lDelStatus = "Planned for Delivery";
        //                        if (lOutTime != "")
        //                        {
        //                            lDelStatus = "Delivered";
        //                        }

        //                        if (lSO.Count > 0)
        //                        {
        //                            for (int i = 0; i < lSO.Count; i++)
        //                            {
        //                                if (lSO[i] == lSONo)
        //                                {
        //                                    lJobID = lJob[i];
        //                                    if (lLoadWT == 0)
        //                                    {
        //                                        lLoadWT = lTotalWT[i] / 1000;
        //                                    }
        //                                }
        //                            }
        //                        }

        //                        lSQL = "INSERT INTO dbo.OESStdSheetDelivery " +
        //                        "(CustomerCode " +
        //                        ", ProjectCode " +
        //                        ", JobID " +
        //                        ", LoadNo " +
        //                        ", SONumber " +
        //                        ", LoadQty " +
        //                        ", LoadWT " +
        //                        ", Delivery_Date " +
        //                        ", Delivery_Status " +
        //                        ", TransportMode " +
        //                        ", Vehicle_No " +
        //                        ", Vehicle_out_time) " +
        //                        "VALUES " +
        //                        "('" + CustomerCode + "' " +
        //                        ",'" + ProjectCode + "' " +
        //                        "," + lJobID.ToString() + " " +
        //                        ",'" + lLoadNo + "' " +
        //                        ",'" + lSONo + "' " +
        //                        ",'" + lLoadQty.ToString() + "' " +
        //                        ",'" + lLoadWT.ToString() + "' " +
        //                        ",Convert(datetime,'" + lPlanDate + "', 112) " +
        //                        ",'" + lDelStatus + "' " +
        //                        ",'" + lVehicalType + "' " +
        //                        ",'" + lVehicalID + "' " +
        //                        ",'" + lOutTime + "')";

        //                        lCmd = new SqlCommand();
        //                        lCmd.CommandText = lSQL;
        //                        lCmd.Connection = lNDSCon;
        //                        lCmd.CommandTimeout = 300;
        //                        lCmd.ExecuteNonQuery();
        //                    }
        //                }
        //            }
        //            lOraRst.Close();
        //            lProcessObj.CloseCISConnection(ref lCISCon);

        //        }

        //        //Rebar
        //        lSO = new List<string>();
        //        lJob = new List<int>();
        //        lBBSIDs = new List<int>();
        //        lTotalWT = new List<decimal>();
        //        lSOStr = "";

        //        lCmd.CommandText = "DELETE " +
        //        "FROM dbo.OESDelivery " +
        //        "WHERE CustomerCode = '" + CustomerCode + "' " +
        //        "AND ProjectCode = '" + ProjectCode + "' " +
        //        "AND Delivery_Status <> 'Delivered' ";

        //        lCmd.Connection = lNDSCon;
        //        lCmd.CommandTimeout = 300;
        //        lCmd.ExecuteNonQuery();

        //        //Standrad Store Bar
        //        lCmd = new SqlCommand();
        //        lCmd.CommandText = "SELECT BBSSAPSO, JobID, BBSOrderSTDWT, BBSID " +
        //        "FROM dbo.OESBBS A " +
        //        "WHERE A.CustomerCode = '" + CustomerCode + "' " +
        //        "AND A.ProjectCode = '" + ProjectCode + "' " +
        //        "AND A.BBSSAPSO > '' " +
        //        "AND A.BBSOrderSTDWT > (" +
        //        "SELECT isnull(SUM(LoadWT * 1000), 0) + 50 " +
        //        "FROM dbo.OESDelivery " +
        //        "WHERE CustomerCode = A.CustomerCode " +
        //        "AND ProjectCode = A.ProjectCode " +
        //        "AND JobID = A.JobID " +
        //        "AND SONumber = A.BBSSAPSO " +
        //        "AND Delivery_Status = 'Delivered') ";

        //        lCmd.Connection = lNDSCon;
        //        lCmd.CommandTimeout = 300;
        //        lRst = lCmd.ExecuteReader();
        //        if (lRst.HasRows)
        //        {
        //            while (lRst.Read())
        //            {
        //                lSO.Add(lRst.GetValue(0) == DBNull.Value ? "" : lRst.GetString(0).Trim());
        //                lJob.Add(lRst.GetValue(1) == DBNull.Value ? 0 : lRst.GetInt32(1));
        //                lTotalWT.Add(lRst.GetValue(2) == DBNull.Value ? 0 : lRst.GetDecimal(2));
        //                lBBSIDs.Add(lRst.GetValue(3) == DBNull.Value ? 0 : lRst.GetInt32(3));

        //                if (lSOStr == "")
        //                {
        //                    lSOStr = "'" + lRst.GetString(0).Trim() + "'";
        //                }
        //                else
        //                {
        //                    lSOStr = lSOStr + ",'" + lRst.GetString(0).Trim() + "'";
        //                }
        //            }
        //        }
        //        lRst.Close();

        //        if (lSOStr != "")
        //        {
        //            lCmd = new SqlCommand();
        //            lCmd.CommandText = "DELETE " +
        //            "FROM dbo.OESDelivery " +
        //            "WHERE CustomerCode = '" + CustomerCode + "' " +
        //            "AND ProjectCode = '" + ProjectCode + "' " +
        //            "AND SONumber in ( " + lSOStr + ") ";

        //            lCmd.Connection = lNDSCon;
        //            lCmd.CommandTimeout = 300;
        //            lCmd.ExecuteNonQuery();

        //            lProcessObj.OpenCISConnection(ref lCISCon);

        //            lSQL = "SELECT  " +
        //            "H.LOAD_NO, " +
        //            "H.PLAN_DELIV_DATE, " +
        //            "H.VEHICLE_TYPE, " +
        //            "H.TRAILER_NO, " +
        //            "H.LP_STATUS, " +
        //            "C.SALES_ORDER, " +
        //            "NVL(SUM(C.PLAN_LOAD_QNTY), 0), " +
        //            "NVL(SUM(C.NO_PIECES), 0), " +
        //            "NVL(MAX(V.VEHICLE_ID), ' '), " +
        //            "NVL(MAX(V.WEIGH_OUT_TIME), ' ') " +
        //            "FROM (SAPSR3.YMPPT_LP_HDR H " +
        //            "INNER JOIN SAPSR3.YMPPT_LP_ITEM_C C " +
        //            "ON H.LOAD_NO = C.LOAD_NO) " +
        //            "LEFT OUTER JOIN SAPSR3.YMPPT_LOAD_VEHIC V " +
        //            "ON C.MANDT = V.MANDT " +
        //            "AND C.SALES_ORDER = V.VBELN " +
        //            "AND C.CLBD_ITEM = V.POSNR " +
        //            "WHERE H.LP_STATUS <> 'X' " +
        //            "AND C.SALES_ORDER IN (" + lSOStr + ") " +
        //            "GROUP BY " +
        //            "H.LOAD_NO, " +
        //            "H.PLAN_DELIV_DATE, " +
        //            "H.VEHICLE_TYPE, " +
        //            "H.TRAILER_NO, " +
        //            "H.LP_STATUS, " +
        //            "C.SALES_ORDER ";

        //            lOraCmd.CommandText = lSQL;
        //            lOraCmd.Connection = lCISCon;
        //            lOraCmd.CommandTimeout = 300;
        //            lOraRst = lOraCmd.ExecuteReader();
        //            if (lOraRst.HasRows)
        //            {
        //                while (lOraRst.Read())
        //                {
        //                    lLoadNo = lOraRst.GetString(0).Trim();
        //                    lPlanDate = lOraRst.GetString(1).Trim();
        //                    lVehicalType = lOraRst.GetString(2).Trim();
        //                    lTrailerNo = lOraRst.GetString(3).Trim();
        //                    lDelStatus = lOraRst.GetString(4).Trim();
        //                    lSONo = lOraRst.GetString(5);
        //                    lLoadWT = lOraRst.GetDecimal(6);
        //                    lLoadQty = lOraRst.GetInt32(7);
        //                    lVehicalID = lOraRst.GetString(8).Trim();
        //                    lOutTime = lOraRst.GetString(9).Trim();

        //                    if (lPlanDate != "20500101")
        //                    {
        //                        if (lLoadQty == 0)
        //                        {
        //                            lLoadQty = (int)lLoadWT;
        //                            lLoadWT = 0;
        //                        }

        //                        lDelStatus = "Planned for Delivery";
        //                        if (lOutTime != "")
        //                        {
        //                            lDelStatus = "Delivered";
        //                        }

        //                        if (lSO.Count > 0)
        //                        {
        //                            for (int i = 0; i < lSO.Count; i++)
        //                            {
        //                                if (lSO[i] == lSONo)
        //                                {
        //                                    lJobID = lJob[i];
        //                                    lBBSID = lBBSIDs[i];
        //                                    if (lLoadWT == 0)
        //                                    {
        //                                        lLoadWT = lTotalWT[i];
        //                                    }
        //                                }
        //                            }
        //                        }

        //                        lSQL = "INSERT INTO dbo.OESDelivery " +
        //                        "(CustomerCode " +
        //                        ", ProjectCode " +
        //                        ", JobID " +
        //                        ", BBSID " +
        //                        ", LoadNo " +
        //                        ", SONumber " +
        //                        ", LoadQty " +
        //                        ", LoadWT " +
        //                        ", Delivery_Date " +
        //                        ", Delivery_Status " +
        //                        ", TransportMode " +
        //                        ", Vehicle_No " +
        //                        ", Vehicle_out_time) " +
        //                        "VALUES " +
        //                        "('" + CustomerCode + "' " +
        //                        ",'" + ProjectCode + "' " +
        //                        "," + lJobID.ToString() + " " +
        //                        "," + lBBSID.ToString() + " " +
        //                        ",'" + lLoadNo + "' " +
        //                        ",'" + lSONo + "' " +
        //                        ",'" + lLoadQty.ToString() + "' " +
        //                        ",'" + lLoadWT.ToString() + "' " +
        //                        ",Convert(datetime,'" + lPlanDate + "', 112) " +
        //                        ",'" + lDelStatus + "' " +
        //                        ",'" + lVehicalType + "' " +
        //                        ",'" + lVehicalID + "' " +
        //                        ",'" + lOutTime + "')";

        //                        lCmd = new SqlCommand();
        //                        lCmd.CommandText = lSQL;
        //                        lCmd.Connection = lNDSCon;
        //                        lCmd.CommandTimeout = 300;
        //                        lCmd.ExecuteNonQuery();
        //                    }
        //                }
        //            }
        //            lOraRst.Close();
        //            lProcessObj.CloseCISConnection(ref lCISCon);

        //        }

        //        //CAB & Coupler
        //        lSO = new List<string>();
        //        lJob = new List<int>();
        //        lBBSIDs = new List<int>();
        //        lTotalWT = new List<decimal>();
        //        lSOStr = "";

        //        lCmd = new SqlCommand();
        //        lCmd.CommandText = "SELECT BBSSOR, BBSSORCoupler, JobID, BBSOrderCABWT, BBSID " +
        //        "FROM dbo.OESBBS A " +
        //        "WHERE A.CustomerCode = '" + CustomerCode + "' " +
        //        "AND A.ProjectCode = '" + ProjectCode + "' " +
        //        "AND A.BBSSOR > '' " +
        //        "AND A.BBSOrderCABWT > (" +
        //        "SELECT isnull(SUM(LoadWT * 1000), 0) + 50 " +
        //        "FROM dbo.OESDelivery " +
        //        "WHERE CustomerCode = A.CustomerCode " +
        //        "AND ProjectCode = A.ProjectCode " +
        //        "AND JobID = A.JobID " +
        //        "AND (SONumber = A.BBSSOR " +
        //        "OR SONumber = A.BBSSORCoupler) " +
        //        "AND Delivery_Status = 'Delivered') ";

        //        lCmd.Connection = lNDSCon;
        //        lCmd.CommandTimeout = 300;
        //        lRst = lCmd.ExecuteReader();
        //        if (lRst.HasRows)
        //        {
        //            while (lRst.Read())
        //            {
        //                if ((lRst.GetValue(0) == DBNull.Value ? "" : lRst.GetString(0).Trim()) != "")
        //                {
        //                    lSO.Add(lRst.GetValue(0) == DBNull.Value ? "" : lRst.GetString(0).Trim());
        //                    lJob.Add(lRst.GetValue(2) == DBNull.Value ? 0 : lRst.GetInt32(2));
        //                    lTotalWT.Add(lRst.GetValue(3) == DBNull.Value ? 0 : lRst.GetDecimal(3));
        //                    lBBSIDs.Add(lRst.GetValue(4) == DBNull.Value ? 0 : lRst.GetInt32(4));
        //                    if (lSOStr == "")
        //                    {
        //                        lSOStr = "'" + lRst.GetString(0).Trim() + "'";
        //                    }
        //                    else
        //                    {
        //                        lSOStr = lSOStr + ",'" + lRst.GetString(0).Trim() + "'";
        //                    }
        //                }
        //                if ((lRst.GetValue(1) == DBNull.Value ? "" : lRst.GetString(1).Trim()) != "")
        //                {
        //                    lSO.Add(lRst.GetValue(1) == DBNull.Value ? "" : lRst.GetString(1).Trim());
        //                    lJob.Add(lRst.GetValue(2) == DBNull.Value ? 0 : lRst.GetInt32(2));
        //                    lTotalWT.Add(lRst.GetValue(3) == DBNull.Value ? 0 : lRst.GetDecimal(3));
        //                    lBBSIDs.Add(lRst.GetValue(4) == DBNull.Value ? 0 : lRst.GetInt32(4));
        //                    if (lSOStr == "")
        //                    {
        //                        lSOStr = "'" + lRst.GetString(1).Trim() + "'";
        //                    }
        //                    else
        //                    {
        //                        lSOStr = lSOStr + ",'" + lRst.GetString(1).Trim() + "'";
        //                    }
        //                }
        //            }
        //        }
        //        lRst.Close();

        //        if (lSOStr != "")
        //        {
        //            lCmd = new SqlCommand();
        //            lCmd.CommandText = "DELETE " +
        //            "FROM dbo.OESDelivery " +
        //            "WHERE CustomerCode = '" + CustomerCode + "' " +
        //            "AND ProjectCode = '" + ProjectCode + "' " +
        //            "AND SONumber in ( " + lSOStr + ") ";

        //            lCmd.Connection = lNDSCon;
        //            lCmd.CommandTimeout = 300;
        //            lCmd.ExecuteNonQuery();

        //            lProcessObj.OpenCISConnection(ref lCISCon);

        //            lSQL = "SELECT  " +
        //            "H.LOAD_NO, " +
        //            "H.PLAN_DELIV_DATE, " +
        //            "H.VEHICLE_TYPE, " +
        //            "H.TRAILER_NO, " +
        //            "H.LP_STATUS, " +
        //            "O.ORDER_REQUEST_NO, " +
        //            "NVL(SUM(C.PLAN_LOAD_QNTY), 0), " +
        //            "NVL(SUM(C.NO_PIECES), 0), " +
        //            "(SELECT NVL(MAX(VEHICLE_ID), ' ') " +
        //            "FROM SAPSR3.YMPPT_LOAD_VEHIC " +
        //            "WHERE MANDT = C.MANDT " +
        //            "AND VBELN = C.SALES_ORDER " +
        //            "), " +
        //            "(SELECT NVL(MAX(WEIGH_OUT_TIME), ' ') " +
        //            "FROM SAPSR3.YMPPT_LOAD_VEHIC " +
        //            "WHERE MANDT = C.MANDT " +
        //            "AND VBELN = C.SALES_ORDER " +
        //            ") " +
        //            "FROM SAPSR3.YMPPT_LP_HDR H,  " +
        //            "SAPSR3.YMPPT_LP_ITEM_C C, " +
        //            "SAPSR3.YMSDT_ORDER_HDR O " +
        //            "WHERE H.LOAD_NO = C.LOAD_NO " +
        //            "AND C.SALES_ORDER = O.SALES_ORDER " +
        //            "AND H.LP_STATUS <> 'X' " +
        //            "AND O.ORDER_REQUESt_NO IN (" + lSOStr + ") " +
        //            "GROUP BY " +
        //            "H.LOAD_NO, " +
        //            "H.PLAN_DELIV_DATE, " +
        //            "H.VEHICLE_TYPE, " +
        //            "H.TRAILER_NO, " +
        //            "H.LP_STATUS, " +
        //            "O.ORDER_REQUEST_NO, " +
        //            "C.MANDT, " +
        //            "C.SALES_ORDER ";

        //            lOraCmd.CommandText = lSQL;
        //            lOraCmd.Connection = lCISCon;
        //            lOraCmd.CommandTimeout = 300;
        //            lOraRst = lOraCmd.ExecuteReader();
        //            if (lOraRst.HasRows)
        //            {
        //                while (lOraRst.Read())
        //                {
        //                    lLoadNo = lOraRst.GetString(0).Trim();
        //                    lPlanDate = lOraRst.GetString(1).Trim();
        //                    lVehicalType = lOraRst.GetString(2).Trim();
        //                    lTrailerNo = lOraRst.GetString(3).Trim();
        //                    lDelStatus = lOraRst.GetString(4).Trim();
        //                    lSONo = lOraRst.GetString(5);
        //                    lLoadWT = lOraRst.GetDecimal(6);
        //                    lLoadQty = lOraRst.GetInt32(7);
        //                    lVehicalID = lOraRst.GetString(8).Trim();
        //                    lOutTime = lOraRst.GetString(9).Trim();

        //                    if (lPlanDate != "20500101")
        //                    {
        //                        if (lLoadQty == 0)
        //                        {
        //                            lLoadQty = (int)lLoadWT;
        //                            lLoadWT = 0;
        //                        }

        //                        lDelStatus = "Planned for Delivery";
        //                        if (lOutTime != "")
        //                        {
        //                            lDelStatus = "Delivered";
        //                        }

        //                        if (lSO.Count > 0)
        //                        {
        //                            for (int i = 0; i < lSO.Count; i++)
        //                            {
        //                                if (lSO[i] == lSONo)
        //                                {
        //                                    lJobID = lJob[i];
        //                                    lBBSID = lBBSIDs[i];
        //                                    if (lLoadWT == 0)
        //                                    {
        //                                        lLoadWT = lTotalWT[i] / 1000;
        //                                    }
        //                                }
        //                            }
        //                        }

        //                        lSQL = "INSERT INTO dbo.OESDelivery " +
        //                        "(CustomerCode " +
        //                        ", ProjectCode " +
        //                        ", JobID " +
        //                        ", BBSID " +
        //                        ", LoadNo " +
        //                        ", SONumber " +
        //                        ", LoadQty " +
        //                        ", LoadWT " +
        //                        ", Delivery_Date " +
        //                        ", Delivery_Status " +
        //                        ", TransportMode " +
        //                        ", Vehicle_No " +
        //                        ", Vehicle_out_time) " +
        //                        "VALUES " +
        //                        "('" + CustomerCode + "' " +
        //                        ",'" + ProjectCode + "' " +
        //                        "," + lJobID.ToString() + " " +
        //                        "," + lBBSID.ToString() + " " +
        //                        ",'" + lLoadNo + "' " +
        //                        ",'" + lSONo + "' " +
        //                        ",'" + lLoadQty.ToString() + "' " +
        //                        ",'" + lLoadWT.ToString() + "' " +
        //                        ",Convert(datetime,'" + lPlanDate + "', 112) " +
        //                        ",'" + lDelStatus + "' " +
        //                        ",'" + lVehicalType + "' " +
        //                        ",'" + lVehicalID + "' " +
        //                        ",'" + lOutTime + "')";

        //                        lCmd = new SqlCommand();
        //                        lCmd.CommandText = lSQL;
        //                        lCmd.Connection = lNDSCon;
        //                        lCmd.CommandTimeout = 300;
        //                        lCmd.ExecuteNonQuery();
        //                    }
        //                }
        //            }
        //            lOraRst.Close();
        //            lProcessObj.CloseCISConnection(ref lCISCon);
        //        }

        //        //BPC
        //        lJob = new List<int>();
        //        var lCageIDs = new List<int>();
        //        var lLoadIDs = new List<int>();
        //        var lLoadQtys = new List<int>();
        //        var lSORs = new List<string>();

        //        lCmd = new SqlCommand();
        //        lCmd.CommandText = "SELECT JobID, cage_id, load_id, load_qty, sor_no " +
        //        "FROM dbo.OESBPCDetailsProc A " +
        //        "WHERE A.CustomerCode = '" + CustomerCode + "' " +
        //        "AND A.ProjectCode = '" + ProjectCode + "' " +
        //        "AND A.SOR_NO > '' " +
        //        "AND A.load_qty > (" +
        //        "SELECT isNull(Max(loadQty), 0) " +
        //        "FROM dbo.OESBPCDelivery " +
        //        "WHERE CustomerCode = A.CustomerCode " +
        //        "AND ProjectCode = A.ProjectCode " +
        //        "AND JobID = A.JobID " +
        //        "AND cage_id = A.cage_id " +
        //        "AND load_id = A.load_id " +
        //        "AND Delivery_Status = 'Delivered') ";

        //        lCmd.Connection = lNDSCon;
        //        lCmd.CommandTimeout = 300;
        //        lRst = lCmd.ExecuteReader();
        //        if (lRst.HasRows)
        //        {
        //            while (lRst.Read())
        //            {
        //                if (lRst.GetValue(0) != DBNull.Value)
        //                {
        //                    lJob.Add(lRst.GetValue(0) == DBNull.Value ? 0 : lRst.GetInt32(0));
        //                    lCageIDs.Add(lRst.GetValue(1) == DBNull.Value ? 0 : lRst.GetInt32(1));
        //                    lLoadIDs.Add(lRst.GetValue(2) == DBNull.Value ? 0 : lRst.GetInt32(2));
        //                    lLoadQtys.Add(lRst.GetValue(3) == DBNull.Value ? 0 : lRst.GetInt32(3));
        //                    lSORs.Add(lRst.GetValue(4) == DBNull.Value ? "" : lRst.GetString(4).Trim());
        //                }
        //            }
        //        }
        //        lRst.Close();

        //        lSOStr = "";
        //        if (lSORs.Count > 0)
        //        {
        //            for (int i = 0; i < lSORs.Count; i++)
        //            {
        //                if (lSOStr == "")
        //                {
        //                    lSOStr = "'" + lSORs[i] + "'";
        //                }
        //                else
        //                {
        //                    lSOStr = lSOStr + ",'" + lSORs[i] + "'";
        //                }

        //            }
        //        }

        //        if (lSOStr != "")
        //        {
        //            lCmd = new SqlCommand();
        //            lCmd.CommandText = "DELETE " +
        //            "FROM dbo.OESBPCDelivery " +
        //            "WHERE CustomerCode = '" + CustomerCode + "' " +
        //            "AND ProjectCode = '" + ProjectCode + "' " +
        //            "AND SONumber in ( " + lSOStr + ") ";

        //            lCmd.Connection = lNDSCon;
        //            lCmd.CommandTimeout = 300;
        //            lCmd.ExecuteNonQuery();

        //            lProcessObj.OpenCISConnection(ref lCISCon);

        //            lSQL = "SELECT  " +
        //            "H.LOAD_NO, " +
        //            "H.PLAN_DELIV_DATE, " +
        //            "H.VEHICLE_TYPE, " +
        //            "H.TRAILER_NO, " +
        //            "H.LP_STATUS, " +
        //            "O.ORDER_REQUEST_NO, " +
        //            "NVL(SUM(C.PLAN_LOAD_QNTY), 0), " +
        //            "NVL(SUM(C.NO_PIECES), 0), " +
        //            "(SELECT NVL(MAX(VEHICLE_ID), ' ') " +
        //            "FROM SAPSR3.YMPPT_LOAD_VEHIC " +
        //            "WHERE MANDT = C.MANDT " +
        //            "AND VBELN = C.SALES_ORDER " +
        //            "), " +
        //            "(SELECT NVL(MAX(WEIGH_OUT_TIME), ' ') " +
        //            "FROM SAPSR3.YMPPT_LOAD_VEHIC " +
        //            "WHERE MANDT = C.MANDT " +
        //            "AND VBELN = C.SALES_ORDER " +
        //            ") " +
        //            "FROM SAPSR3.YMPPT_LP_HDR H,  " +
        //            "SAPSR3.YMPPT_LP_ITEM_C C, " +
        //            "SAPSR3.YMSDT_ORDER_HDR O " +
        //            "WHERE H.LOAD_NO = C.LOAD_NO " +
        //            "AND C.SALES_ORDER = O.SALES_ORDER " +
        //            "AND H.LP_STATUS <> 'X' " +
        //            "AND O.ORDER_REQUESt_NO IN (" + lSOStr + ") " +
        //            "GROUP BY " +
        //            "H.LOAD_NO, " +
        //            "H.PLAN_DELIV_DATE, " +
        //            "H.VEHICLE_TYPE, " +
        //            "H.TRAILER_NO, " +
        //            "H.LP_STATUS, " +
        //            "O.ORDER_REQUEST_NO, " +
        //            "C.MANDT, " +
        //            "C.SALES_ORDER ";

        //            lOraCmd.CommandText = lSQL;
        //            lOraCmd.Connection = lCISCon;
        //            lOraCmd.CommandTimeout = 300;
        //            lOraRst = lOraCmd.ExecuteReader();
        //            if (lOraRst.HasRows)
        //            {
        //                while (lOraRst.Read())
        //                {
        //                    lLoadNo = lOraRst.GetString(0).Trim();
        //                    lPlanDate = lOraRst.GetString(1).Trim();
        //                    lVehicalType = lOraRst.GetString(2).Trim();
        //                    lTrailerNo = lOraRst.GetString(3).Trim();
        //                    lDelStatus = lOraRst.GetString(4).Trim();
        //                    lSONo = lOraRst.GetString(5);
        //                    lLoadWT = lOraRst.GetDecimal(6);
        //                    lLoadQty = lOraRst.GetInt32(7);
        //                    lVehicalID = lOraRst.GetString(8).Trim();
        //                    lOutTime = lOraRst.GetString(9).Trim();

        //                    if (lPlanDate != "20500101")
        //                    {
        //                        if (lLoadQty == 0)
        //                        {
        //                            lLoadQty = (int)lLoadWT;
        //                            lLoadWT = 0;
        //                        }

        //                        lDelStatus = "Planned for Delivery";
        //                        if (lOutTime != "")
        //                        {
        //                            lDelStatus = "Delivered";
        //                        }

        //                        if (lSORs.Count > 0)
        //                        {
        //                            for (int i = 0; i < lSORs.Count; i++)
        //                            {
        //                                if (lSORs[i] == lSONo)
        //                                {
        //                                    lJobID = lJob[i];
        //                                    lCageID = lCageIDs[i];
        //                                    lLoadID = lLoadIDs[i];
        //                                }
        //                            }
        //                        }

        //                        lSQL = "INSERT INTO dbo.OESBPCDelivery " +
        //                        "(CustomerCode " +
        //                        ", ProjectCode " +
        //                        ", JobID " +
        //                        ", cage_id" +
        //                        ", load_id" +
        //                        ", LoadNo " +
        //                        ", SONumber " +
        //                        ", LoadQty " +
        //                        ", LoadWT " +
        //                        ", Delivery_Date " +
        //                        ", Delivery_Status " +
        //                        ", TransportMode " +
        //                        ", Vehicle_No " +
        //                        ", Vehicle_out_time) " +
        //                        "VALUES " +
        //                        "('" + CustomerCode + "' " +
        //                        ",'" + ProjectCode + "' " +
        //                        "," + lJobID.ToString() + " " +
        //                        "," + lCageID.ToString() + " " +
        //                        "," + lLoadID.ToString() + " " +
        //                        ",'" + lLoadNo + "' " +
        //                        ",'" + lSONo + "' " +
        //                        ",'" + lLoadQty.ToString() + "' " +
        //                        ",'" + lLoadWT.ToString() + "' " +
        //                        ",Convert(datetime,'" + lPlanDate + "', 112) " +
        //                        ",'" + lDelStatus + "' " +
        //                        ",'" + lVehicalType + "' " +
        //                        ",'" + lVehicalID + "' " +
        //                        ",'" + lOutTime + "')";

        //                        lCmd = new SqlCommand();
        //                        lCmd.CommandText = lSQL;
        //                        lCmd.Connection = lNDSCon;
        //                        lCmd.CommandTimeout = 300;
        //                        lCmd.ExecuteNonQuery();
        //                    }
        //                }
        //            }
        //            lOraRst.Close();
        //            lProcessObj.CloseCISConnection(ref lCISCon);

        //        }

        //        //CTS MESH
        //        lSO = new List<string>();
        //        lJob = new List<int>();
        //        lBBSIDs = new List<int>();
        //        lTotalWT = new List<decimal>();
        //        lSOStr = "";

        //        lCmd = new SqlCommand();
        //        lCmd.CommandText = "SELECT BBSSOR, JobID, BBSTotalWT, BBSID " +
        //        "FROM dbo.OESCTSMESHBBS A " +
        //        "WHERE A.CustomerCode = '" + CustomerCode + "' " +
        //        "AND A.ProjectCode = '" + ProjectCode + "' " +
        //        "AND A.BBSSOR > '' " +
        //        "AND A.BBSTotalWT > (" +
        //        "SELECT isnull(SUM(LoadWT * 1000), 0) + 50 " +
        //        "FROM dbo.OESCTSMESHDelivery " +
        //        "WHERE CustomerCode = A.CustomerCode " +
        //        "AND ProjectCode = A.ProjectCode " +
        //        "AND JobID = A.JobID " +
        //        "AND SONumber = A.BBSSOR " +
        //        "AND Delivery_Status = 'Delivered') ";

        //        lCmd.Connection = lNDSCon;
        //        lCmd.CommandTimeout = 300;
        //        lRst = lCmd.ExecuteReader();
        //        if (lRst.HasRows)
        //        {
        //            while (lRst.Read())
        //            {
        //                lSO.Add(lRst.GetValue(0) == DBNull.Value ? "" : lRst.GetString(0).Trim());
        //                lJob.Add(lRst.GetValue(1) == DBNull.Value ? 0 : lRst.GetInt32(1));
        //                lTotalWT.Add(lRst.GetValue(2) == DBNull.Value ? 0 : lRst.GetDecimal(2));
        //                lBBSIDs.Add(lRst.GetValue(3) == DBNull.Value ? 0 : lRst.GetInt32(3));
        //                if (lSOStr == "")
        //                {
        //                    lSOStr = "'" + lRst.GetString(0).Trim() + "'";
        //                }
        //                else
        //                {
        //                    lSOStr = lSOStr + ",'" + lRst.GetString(0).Trim() + "'";
        //                }
        //            }
        //        }
        //        lRst.Close();

        //        if (lSOStr != "")
        //        {
        //            lCmd = new SqlCommand();
        //            lCmd.CommandText = "DELETE " +
        //            "FROM dbo.OESCTSMESHDelivery " +
        //            "WHERE CustomerCode = '" + CustomerCode + "' " +
        //            "AND ProjectCode = '" + ProjectCode + "' " +
        //            "AND SONumber in ( " + lSOStr + ") ";

        //            lCmd.Connection = lNDSCon;
        //            lCmd.CommandTimeout = 300;
        //            lCmd.ExecuteNonQuery();

        //            lProcessObj.OpenCISConnection(ref lCISCon);

        //            lSQL = "SELECT  " +
        //            "H.LOAD_NO, " +
        //            "H.PLAN_DELIV_DATE, " +
        //            "H.VEHICLE_TYPE, " +
        //            "H.TRAILER_NO, " +
        //            "H.LP_STATUS, " +
        //            "O.ORDER_REQUEST_NO, " +
        //            "NVL(SUM(C.PLAN_LOAD_QNTY), 0), " +
        //            "NVL(SUM(C.NO_PIECES), 0), " +
        //            "(SELECT NVL(MAX(VEHICLE_ID), ' ') " +
        //            "FROM SAPSR3.YMPPT_LOAD_VEHIC " +
        //            "WHERE MANDT = C.MANDT " +
        //            "AND VBELN = C.SALES_ORDER " +
        //            "), " +
        //            "(SELECT NVL(MAX(WEIGH_OUT_TIME), ' ') " +
        //            "FROM SAPSR3.YMPPT_LOAD_VEHIC " +
        //            "WHERE MANDT = C.MANDT " +
        //            "AND VBELN = C.SALES_ORDER " +
        //            ") " +
        //            "FROM SAPSR3.YMPPT_LP_HDR H,  " +
        //            "SAPSR3.YMPPT_LP_ITEM_C C, " +
        //            "SAPSR3.YMSDT_ORDER_HDR O " +
        //            "WHERE H.LOAD_NO = C.LOAD_NO " +
        //            "AND C.SALES_ORDER = O.SALES_ORDER " +
        //            "AND H.LP_STATUS <> 'X' " +
        //            "AND O.ORDER_REQUESt_NO IN (" + lSOStr + ") " +
        //            "GROUP BY " +
        //            "H.LOAD_NO, " +
        //            "H.PLAN_DELIV_DATE, " +
        //            "H.VEHICLE_TYPE, " +
        //            "H.TRAILER_NO, " +
        //            "H.LP_STATUS, " +
        //            "O.ORDER_REQUEST_NO, " +
        //            "C.MANDT, " +
        //            "C.SALES_ORDER ";

        //            lOraCmd.CommandText = lSQL;
        //            lOraCmd.Connection = lCISCon;
        //            lOraCmd.CommandTimeout = 300;
        //            lOraRst = lOraCmd.ExecuteReader();
        //            if (lOraRst.HasRows)
        //            {
        //                while (lOraRst.Read())
        //                {
        //                    lLoadNo = lOraRst.GetString(0).Trim();
        //                    lPlanDate = lOraRst.GetString(1).Trim();
        //                    lVehicalType = lOraRst.GetString(2).Trim();
        //                    lTrailerNo = lOraRst.GetString(3).Trim();
        //                    lDelStatus = lOraRst.GetString(4).Trim();
        //                    lSONo = lOraRst.GetString(5);
        //                    lLoadWT = lOraRst.GetDecimal(6);
        //                    lLoadQty = lOraRst.GetInt32(7);
        //                    lVehicalID = lOraRst.GetString(8).Trim();
        //                    lOutTime = lOraRst.GetString(9).Trim();

        //                    if (lPlanDate != "20500101")
        //                    {
        //                        if (lLoadQty == 0)
        //                        {
        //                            lLoadQty = (int)lLoadWT;
        //                            lLoadWT = 0;
        //                        }

        //                        lDelStatus = "Planned for Delivery";
        //                        if (lOutTime != "")
        //                        {
        //                            lDelStatus = "Delivered";
        //                        }

        //                        if (lSO.Count > 0)
        //                        {
        //                            for (int i = 0; i < lSO.Count; i++)
        //                            {
        //                                if (lSO[i] == lSONo)
        //                                {
        //                                    lJobID = lJob[i];
        //                                    lBBSID = lBBSIDs[i];
        //                                    if (lLoadWT == 0)
        //                                    {
        //                                        lLoadWT = lTotalWT[i] / 1000;
        //                                    }
        //                                }
        //                            }
        //                        }

        //                        lSQL = "INSERT INTO dbo.OESCTSMESHDelivery " +
        //                        "(CustomerCode " +
        //                        ", ProjectCode " +
        //                        ", JobID " +
        //                        ", BBSID " +
        //                        ", LoadNo " +
        //                        ", SONumber " +
        //                        ", LoadQty " +
        //                        ", LoadWT " +
        //                        ", Delivery_Date " +
        //                        ", Delivery_Status " +
        //                        ", TransportMode " +
        //                        ", Vehicle_No " +
        //                        ", Vehicle_out_time) " +
        //                        "VALUES " +
        //                        "('" + CustomerCode + "' " +
        //                        ",'" + ProjectCode + "' " +
        //                        "," + lJobID.ToString() + " " +
        //                        "," + lBBSID.ToString() + " " +
        //                        ",'" + lLoadNo + "' " +
        //                        ",'" + lSONo + "' " +
        //                        ",'" + lLoadQty.ToString() + "' " +
        //                        ",'" + lLoadWT.ToString() + "' " +
        //                        ",Convert(datetime,'" + lPlanDate + "', 112) " +
        //                        ",'" + lDelStatus + "' " +
        //                        ",'" + lVehicalType + "' " +
        //                        ",'" + lVehicalID + "' " +
        //                        ",'" + lOutTime + "')";

        //                        lCmd = new SqlCommand();
        //                        lCmd.CommandText = lSQL;
        //                        lCmd.Connection = lNDSCon;
        //                        lCmd.CommandTimeout = 300;
        //                        lCmd.ExecuteNonQuery();
        //                    }
        //                }
        //            }
        //            lOraRst.Close();
        //            lProcessObj.CloseCISConnection(ref lCISCon);
        //        }

        //        lProcessObj.CloseNDSConnection(ref lNDSCon);
        //    }
        //    return lReturn;
        //}

        //public async Task<List<string>> getAlertMessage(string pCustomerCode, string pProjectCode, string pUserID, string pSubmission, string pEditable)
        //{
        //    List<string> lReturn = new List<string>();
        //    var lCISCon = new OracleConnection();
        //    var lOraCmd = new OracleCommand();
        //    OracleDataReader lOraRst;
        //    string lSQL = "";
        //    int lDeliveryToday = 0;
        //    int lDeliveryTomorrow = 0;


        //    var lSavedOrders = await (from p in db.OrderProject
        //                              where p.CustomerCode == pCustomerCode &&
        //                              p.ProjectCode == pProjectCode &&
        //                              p.UpdateBy == pUserID &&
        //                              (p.OrderStatus == null ||
        //                              p.OrderStatus == "New" ||
        //                              p.OrderStatus == "Created" ||
        //                              p.OrderStatus == "Reserved")
        //                              select p.OrderNumber).ToListAsync();

        //    if (lSavedOrders != null && lSavedOrders.Count > 0)
        //    {
        //        lReturn.Add("You have " + lSavedOrders.Count.ToString() + " drafted orders waiting for submission.");
        //    }

        //    if (pSubmission == "Yes")
        //    {
        //        var lOrdersApproval = await (from p in db.OrderProject
        //                                     where p.CustomerCode == pCustomerCode &&
        //                                     p.ProjectCode == pProjectCode &&
        //                                     p.OrderStatus == "Sent"
        //                                     select p.OrderNumber).ToListAsync();

        //        if (lOrdersApproval != null && lOrdersApproval.Count > 0)
        //        {
        //            lReturn.Add("You have " + lOrdersApproval.Count.ToString() + " orders waiting for your approval.");
        //        }
        //    }
        //    // Delivery

        //    var lProcessObj = new ProcessController();

        //    lProcessObj.OpenCISConnection(ref lCISCon);

        //    lSQL = "" +
        //    "SELECT " +
        //    "Count(DISTINCT M.order_request_no) " +
        //    "FROM SAPSR3.YMSDT_ORDER_HDR M LEFT OUTER JOIN SAPSR3.YMSDT_REQ_DETAIL D " +
        //    "ON M.order_request_no = D.order_request_no " +
        //    "WHERE M.MANDT = '" + lProcessObj.strClient + "' " +
        //    "AND M.PROJECT = '" + pProjectCode + "' " +
        //    "AND M.SALES_ORDER <> ' ' " +
        //    "AND M.SALES_ORDER is NOT Null " +
        //    "AND M.PO_NUMBER <> ' ' " +
        //    "AND M.PO_NUMBER is NOT Null " +
        //    "AND M.REQD_DEL_DATE >= TO_CHAR(SYSDATE-30, 'YYYYMMDD') " +
        //    "AND M.REQD_DEL_DATE <= TO_CHAR(SYSDATE+30, 'YYYYMMDD') " +
        //    "AND (EXISTS " +
        //    "(SELECT P.VGBEL FROM SAPSR3.LIPS P, SAPSR3.LIKP K " +
        //    "WHERE P.MANDT = K.MANDT " +
        //    "AND P.VBELN = K.VBELN " +
        //    "AND P.VGBEL = M.SALES_ORDER " +
        //    "AND K.LFDAT = TO_CHAR(SYSDATE, 'YYYYMMDD') " +
        //    "AND NOT EXISTS " +
        //    "(SELECT VBELN FROM SAPSR3.YMPPT_LOAD_VEHIC WHERE " +
        //    "VBELN = M.SALES_ORDER ) ) " +
        //    "OR EXISTS " +
        //    "(SELECT SALES_ORDER FROM SAPSR3.YMPPT_LP_ITEM_C LC, SAPSR3.YMPPT_LP_HDR LH " +
        //    "WHERE LC.MANDT = LH.MANDT " +
        //    "AND LC.LOAD_NO = LH.LOAD_NO " +
        //    "AND SALES_ORDER = M.SALES_ORDER " +
        //    "AND PLAN_DELIV_DATE = TO_CHAR(SYSDATE, 'YYYYMMDD') " +
        //    "AND NOT EXISTS " +
        //    "(SELECT P.VGBEL FROM SAPSR3.LIPS P, SAPSR3.LIKP K " +
        //    "WHERE P.MANDT = K.MANDT " +
        //    "AND P.VBELN = K.VBELN " +
        //    "AND P.VGBEL = M.SALES_ORDER ) " +
        //    "AND NOT EXISTS " +
        //    "(SELECT VBELN FROM SAPSR3.YMPPT_LOAD_VEHIC WHERE " +
        //    "VBELN = M.SALES_ORDER ) ) " +
        //    "OR EXISTS " +
        //    "(SELECT VBELN FROM SAPSR3.YMPPT_LOAD_VEHIC WHERE " +
        //    "VBELN = M.SALES_ORDER  AND LOAD_DATE = TO_CHAR(SYSDATE, 'YYYYMMDD')) ) ";

        //    lOraCmd.CommandText = lSQL;
        //    lOraCmd.Connection = lCISCon;
        //    lOraCmd.CommandTimeout = 300;
        //    lOraRst = (OracleDataReader)await lOraCmd.ExecuteReaderAsync();
        //    if (lOraRst.HasRows)
        //    {
        //        if (lOraRst.Read())
        //        {
        //            lDeliveryToday = (lOraRst.GetValue(0) == DBNull.Value ? 0 : lOraRst.GetInt32(0));
        //        }
        //    }

        //    lOraRst.Close();

        //    lSQL = "" +
        //    "SELECT " +
        //    "Count(DISTINCT M.order_request_no) " +
        //    "FROM SAPSR3.YMSDT_ORDER_HDR M LEFT OUTER JOIN SAPSR3.YMSDT_REQ_DETAIL D " +
        //    "ON M.order_request_no = D.order_request_no " +
        //    "WHERE M.MANDT = '" + lProcessObj.strClient + "' " +
        //    "AND M.PROJECT = '" + pProjectCode + "' " +
        //    "AND M.SALES_ORDER <> ' ' " +
        //    "AND M.SALES_ORDER is NOT Null " +
        //    "AND M.PO_NUMBER <> ' ' " +
        //    "AND M.PO_NUMBER is NOT Null " +
        //    "AND M.REQD_DEL_DATE >= TO_CHAR(SYSDATE-30, 'YYYYMMDD') " +
        //    "AND M.REQD_DEL_DATE <= TO_CHAR(SYSDATE+30, 'YYYYMMDD') " +
        //    "AND (EXISTS " +
        //    "(SELECT P.VGBEL FROM SAPSR3.LIPS P, SAPSR3.LIKP K " +
        //    "WHERE P.MANDT = K.MANDT " +
        //    "AND P.VBELN = K.VBELN " +
        //    "AND P.VGBEL = M.SALES_ORDER " +
        //    "AND K.LFDAT = TO_CHAR(SYSDATE + 1, 'YYYYMMDD') " +
        //    "AND NOT EXISTS " +
        //    "(SELECT VBELN FROM SAPSR3.YMPPT_LOAD_VEHIC WHERE " +
        //    "VBELN = M.SALES_ORDER ) ) " +
        //    "OR EXISTS " +
        //    "(SELECT SALES_ORDER FROM SAPSR3.YMPPT_LP_ITEM_C LC, SAPSR3.YMPPT_LP_HDR LH " +
        //    "WHERE LC.MANDT = LH.MANDT " +
        //    "AND LC.LOAD_NO = LH.LOAD_NO " +
        //    "AND SALES_ORDER = M.SALES_ORDER " +
        //    "AND PLAN_DELIV_DATE = TO_CHAR(SYSDATE + 1, 'YYYYMMDD') " +
        //    "AND NOT EXISTS " +
        //    "(SELECT P.VGBEL FROM SAPSR3.LIPS P, SAPSR3.LIKP K " +
        //    "WHERE P.MANDT = K.MANDT " +
        //    "AND P.VBELN = K.VBELN " +
        //    "AND P.VGBEL = M.SALES_ORDER ) " +
        //    "AND NOT EXISTS " +
        //    "(SELECT VBELN FROM SAPSR3.YMPPT_LOAD_VEHIC WHERE " +
        //    "VBELN = M.SALES_ORDER ) ) " +
        //    "OR EXISTS " +
        //    "(SELECT VBELN FROM SAPSR3.YMPPT_LOAD_VEHIC WHERE " +
        //    "VBELN = M.SALES_ORDER  AND LOAD_DATE = TO_CHAR(SYSDATE + 1, 'YYYYMMDD')) ) ";

        //    lOraCmd.CommandText = lSQL;
        //    lOraCmd.Connection = lCISCon;
        //    lOraCmd.CommandTimeout = 300;
        //    lOraRst = (OracleDataReader)await lOraCmd.ExecuteReaderAsync();
        //    if (lOraRst.HasRows)
        //    {
        //        if (lOraRst.Read())
        //        {
        //            lDeliveryTomorrow = (lOraRst.GetValue(0) == DBNull.Value ? 0 : lOraRst.GetInt32(0));
        //        }
        //    }
        //    lOraRst.Close();


        //    if (lDeliveryToday > 0)
        //    {
        //        lReturn.Add(lDeliveryToday.ToString() + " orders are confirmed to deliver today.");

        //    }

        //    if (lDeliveryTomorrow > 0)
        //    {
        //        lReturn.Add(lDeliveryTomorrow.ToString() + " orders are confirmed to deliver tomorrow.");

        //    }

        //    return lReturn;
        //}
        //public string checkPlanComplete(int OrderNumber, string UserName)
        //{
        //    string lCustomerCode = "";
        //    string lProjectCode = "";

        //    int lPMDOnly = 1;
        //    var lUserType = "";

        //    string lReturn = "Yes";

        //    if (UserName != null && UserName.IndexOf("@") > 0
        //    && UserName.Split('@')[1].ToUpper() == "NATSTEEL.COM.SG")
        //    {
        //        UserAccessController lUa = new UserAccessController();

        //        var lHeader = db.OrderProject.Find(OrderNumber);
        //        if (lHeader == null || (lHeader.OrderStatus != "New" && lHeader.OrderStatus != "Created" && lHeader.OrderStatus != "Sent" && lHeader.OrderStatus != "Created*"))
        //        {
        //            return lReturn;
        //        }

        //        var lSE = (from p in db.OrderProjectSE
        //                   where p.OrderNumber == OrderNumber &&
        //                   p.ProductType == "CAB"
        //                   select p).ToList();

        //        if (lSE == null || lSE.Count == 0)
        //        {
        //            return lReturn;
        //        }

        //        lCustomerCode = lHeader.CustomerCode;
        //        lProjectCode = lHeader.ProjectCode;

        //        for (int i = 0; i < lSE.Count; i++)
        //        {
        //            int lJobID = lSE[i].CABJobID;

        //            if (lJobID <= 0)
        //            {
        //                continue;
        //            }

        //            //check BBS
        //            var lBBS = (from p in db.BBS
        //                        where p.CustomerCode == lCustomerCode &&
        //                        p.ProjectCode == lProjectCode &&
        //                        p.JobID == lJobID
        //                        select p).ToList();
        //            for (int j = 0; j < lBBS.Count; j++)
        //            {
        //                int BBSID = lBBS[j].BBSID;
        //                var lUpdatedBy = (from p in db.OrderDetails
        //                                  where p.CustomerCode == lCustomerCode &&
        //                                  p.ProjectCode == lProjectCode &&
        //                                  p.JobID == lJobID &&
        //                                  p.BBSID == BBSID &&
        //                                  p.Cancelled == null &&
        //                                  ((p.BarShapeCode != null &&
        //                                  p.BarShapeCode != "") ||
        //                                  (p.BarSize != null &&
        //                                  p.BarSize > 0) ||
        //                                  (p.BarEachQty != null &&
        //                                  p.BarEachQty > 0) ||
        //                                  (p.BarMemberQty != null &&
        //                                  p.BarMemberQty > 0))
        //                                  select p.UpdateBy).Distinct().ToList();

        //                if (lUpdatedBy.Count > 0)
        //                {
        //                    for (int k = 0; k < lUpdatedBy.Count; k++)
        //                    {
        //                        lUserType = lUa.getUserType(lUpdatedBy[k]);

        //                        if (lUserType != "AD" && lUserType != "PM"
        //                        && lUserType != "PA" && lUserType != "P1"
        //                        && lUserType != "P2" && lUserType != "PU" && lUserType != "TE")
        //                        {
        //                            lPMDOnly = 0;
        //                            break;
        //                        }

        //                    }
        //                }

        //                if (lPMDOnly == 0)
        //                {
        //                    break;
        //                }
        //            }

        //            if (lPMDOnly == 0)
        //            {
        //                break;
        //            }
        //        }

        //        lUa = null;

        //        if (lPMDOnly == 0)
        //        {
        //            var lErrorMsg = checkCABDetails(OrderNumber, UserName);
        //            if (lErrorMsg != "")
        //            {
        //                lReturn = "No";
        //            }
        //        }

        //    }
        //    return lReturn;
        //}

        public string checkCABDetails(int OrderNumber, string UserName)
        {
            var lCmd = new SqlCommand();
            SqlDataReader lRst;
            string lCustomerCode = "";
            string lProjectCode = "";
            string lErrorMsg = "";
            int lMaxBarLen = 12000;
            bool lSkipBendingCheck = false;
            string lExtraBarType = "";
            bool lVarianceBarSplit = false;

            var lOrderDet = new OrderDetailsController();
            SqlConnection lNDSCon = new SqlConnection();

            var lPinMaster = db.PinMaster.ToList();

            var lAddnLimitShape = (from p in db.BBSAddnLimitShape
                                   orderby p.shape_code, p.shape_paras
                                   select p).ToList();
            var lAddnLimit = (from p in db.BBSAddnLimit
                              orderby p.dia
                              select p).ToList();
            var lAddnParLimit = (from p in db.BBSAddnParLimit
                                 orderby p.par_type, p.dia
                                 select p).ToList();

            var lHeader = db.OrderProject.Find(OrderNumber);
            if (lHeader == null || (lHeader.OrderStatus != "New" && lHeader.OrderStatus != "Created" && lHeader.OrderStatus != "Sent" && lHeader.OrderStatus != "Created*"))
            {
                return lErrorMsg;
            }

            var lSE = (from p in db.OrderProjectSE
                       where p.OrderNumber == OrderNumber &&
                       p.ProductType == "CAB"
                       select p).ToList();

            if (lSE == null || lSE.Count == 0)
            {
                return lErrorMsg;
            }

            lCustomerCode = lHeader.CustomerCode;
            lProjectCode = lHeader.ProjectCode;

            var lProj = db.Project.Find(lCustomerCode, lProjectCode);

            if (lProj != null)
            {
                lMaxBarLen = lProj.MaxBarLength;
                lSkipBendingCheck = lProj.SkipBendCheck == null ? false : (bool)lProj.SkipBendCheck;
                lExtraBarType = lProj.CustomerBar == null ? "" : (string)lProj.CustomerBar;
                lVarianceBarSplit = lProj.VarianceBarSplit == null ? false : (bool)lProj.VarianceBarSplit;
            }

            for (int i = 0; i < lSE.Count; i++)
            {
                int lJobID = lSE[i].CABJobID;

                if (lJobID <= 0)
                {
                    continue;
                }

                var lJobAdv = db.JobAdvice.Find(lCustomerCode, lProjectCode, lJobID);

                //Take other standard
                if (lJobAdv != null && lJobAdv.BBSStandard != null)
                {
                    var lBBSStd = lJobAdv.BBSStandard.Trim();
                    if (lBBSStd != "" && lBBSStd != "BS-8666")
                    {
                        lPinMaster = (from p in db.PinAdd
                                      where p.std_code == lBBSStd
                                      orderby p.grade, p.type, p.dia
                                      select new
                                      {
                                          grade = p.grade,
                                          type = p.type,
                                          dia = p.dia,
                                          pin = p.pin,
                                          bend_len_min = p.bend_len_min,
                                          hook_len_min = p.hook_len_min,
                                          hook_height_min = p.hook_height_min
                                      }).ToList().Select(x => new PinModels
                                      {
                                          grade = x.grade,
                                          type = x.type,
                                          dia = x.dia,
                                          pin = x.pin,
                                          bend_len_min = x.bend_len_min,
                                          hook_len_min = x.hook_len_min,
                                          hook_height_min = x.hook_height_min
                                      }).ToList();

                    }
                    else
                    {
                        lPinMaster = db.PinMaster.ToList();
                    }
                }

                int lNCouplerShape = 0;
                int lECouplerShape = 0;
                int lCouplerShapeInd = 0;
                //check BBS
                var lBBS = (from p in db.BBS
                            where p.CustomerCode == lCustomerCode &&
                            p.ProjectCode == lProjectCode &&
                            p.JobID == lJobID
                            select p).ToList();

                if (lBBS != null && lBBS.Count > 0)
                {
                    for (int j = 0; j < lBBS.Count; j++)
                    {
                        if (lBBS[j].BBSNo == null || lBBS[j].BBSNo.Trim() == "")
                        {
                            lErrorMsg = "Invalid BBS Number. Please enter BBS Number and its details.\n\n 加工表号码无效, 请输入加工表号码.";
                            return lErrorMsg;
                        }
                        if (lBBS[j].BBSDesc == null || lBBS[j].BBSDesc.Trim() == "")

                        {

                            lErrorMsg = "Invalid BBS Description. Please enter BBS Description in order details.\n\n 加工详细说明无效, 请输入订单明细中的加工详细说明.";

                            return lErrorMsg;

                        }
                        if (lBBS[j].BBSOrderCABWT > 0 && (lBBS[j].BBSStrucElem == null || lBBS[j].BBSStrucElem == ""))
                        {
                            lErrorMsg = "Invalid Structure Element. Please choose Structure Element.\n\n 无构件名称, 请选择构件名称.";
                            return lErrorMsg;
                        }


                        //check duplicated BBS.
                        var lBBSNo = "'" + lBBS[j].BBSNo + "'";
                        int lDuplicatedBBS = 0;

                        lCmd.CommandText = "SELECT BBS_NO " +
                       "FROM dbo.SAP_REQUEST_DETAILS D, dbo.SAP_REQUEST_HDR H " +
                       "WHERE D.ORD_REQ_NO = H.ORD_REQ_NO " +
                       "AND H.PROJ_ID = '" + lProjectCode + "' " +
                       "AND BBS_NO in (" + lBBSNo + ") " +
                       "AND D.PROD_TYPE = 'CAB' " +
                       "AND D.STATUS <> 'X' " +
                       "AND H.STATUS <> 'X' ";

                        var lProcessObj1 = new ProcessController();
                        if (lProcessObj1.OpenNDSConnection(ref lNDSCon) == true)
                        {
                            lCmd.Connection = lNDSCon;
                            lCmd.CommandTimeout = 300;
                            lRst = lCmd.ExecuteReader();
                            if (lRst.HasRows)
                            {
                                if (lRst.Read())
                                {
                                    lDuplicatedBBS = 1;
                                }
                            }
                            lRst.Close();

                            if (lDuplicatedBBS == 0)
                            {
                                lCmd = new SqlCommand();
                                lCmd.CommandText =
                                    "SELECT BBSNo " +
                                    "FROM dbo.OESBBS B, dbo.OESJobAdvice J " +
                                    "WHERE B.CustomerCode = '" + lCustomerCode + "' " +
                                    "AND B.ProjectCode = '" + lProjectCode + "' " +
                                    "AND J.CustomerCode = '" + lCustomerCode + "' " +
                                    "AND J.ProjectCode = '" + lProjectCode + "' " +
                                    "AND B.JobID = J.JobID " +
                                    "AND J.OrderStatus = 'Submitted' " +
                                    "AND B.JobID <> " + lJobID + " " +
                                    "AND B.BBSNo in (" + lBBSNo + ") " +
                                    "AND B.BBSNo is not null ";

                                lCmd.Connection = lNDSCon;
                                lCmd.CommandTimeout = 300;
                                lRst = lCmd.ExecuteReader();
                                if (lRst.HasRows)
                                {
                                    if (lRst.Read())
                                    {
                                        lDuplicatedBBS = 1;
                                    }
                                }
                                lRst.Close();
                            }

                            if (lDuplicatedBBS == 0)
                            {
                                int lOrderNumber = 0;
                                int lBBSIDc = 0;

                                lCmd = new SqlCommand();
                                lCmd.CommandText =
                                    "SELECT J.OrderNumber, Max(BBSID) " +
                                    "FROM  dbo.OESBBS B, dbo.OESProjOrder J, dbo.OESProjOrdersSE S " +
                                    "WHERE B.CustomerCode = '" + lCustomerCode + "' " +
                                    "AND B.ProjectCode = '" + lProjectCode + "' " +
                                    "AND J.CustomerCode = '" + lCustomerCode + "' " +
                                    "AND J.ProjectCode = '" + lProjectCode + "' " +
                                    "AND J.OrderNumber = S.OrderNumber " +
                                    "AND B.JobID = S.CABJobID " +
                                    "AND B.JobID = " + lJobID + " " +
                                    "AND B.BBSNo in (" + lBBSNo + ") " +
                                    "AND B.BBSNo is not null " +
                                    "GROUP BY J.OrderNumber ";

                                lCmd.Connection = lNDSCon;
                                lCmd.CommandTimeout = 300;
                                lRst = lCmd.ExecuteReader();
                                if (lRst.HasRows)
                                {
                                    if (lRst.Read())
                                    {
                                        lOrderNumber = lRst.GetInt32(0);
                                        lBBSIDc = lRst.GetInt32(1);
                                    }
                                }
                                lRst.Close();

                                lCmd = new SqlCommand();

                                lCmd.CommandText =
                                    "SELECT B.BBSNo " +
                                    "FROM  dbo.OESBBS B, dbo.OESProjOrder J, dbo.OESProjOrdersSE S " +
                                    "WHERE B.CustomerCode = '" + lCustomerCode + "' " +
                                    "AND B.ProjectCode = '" + lProjectCode + "' " +
                                    "AND J.CustomerCode = '" + lCustomerCode + "' " +
                                    "AND J.ProjectCode = '" + lProjectCode + "' " +
                                    "AND J.OrderNumber = S.OrderNumber " +
                                    "AND J.OrderNumber = " + lOrderNumber.ToString() + " " +
                                    "AND B.JobID = S.CABJobID " +
                                    "AND B.JobID <> " + lJobID + " " +
                                    "AND B.BBSNo in (" + lBBSNo + ") " +
                                    "AND B.BBSNo is not null ";

                                lCmd.Connection = lNDSCon;
                                lCmd.CommandTimeout = 300;
                                lRst = lCmd.ExecuteReader();
                                if (lRst.HasRows)
                                {
                                    if (lRst.Read())
                                    {
                                        lDuplicatedBBS = 1;
                                    }
                                }
                                lRst.Close();

                                // Check internal BBS duplicated
                                lCmd.CommandText =
                                    "SELECT B.BBSNo " +
                                    "FROM  dbo.OESBBS B, dbo.OESProjOrder J, dbo.OESProjOrdersSE S " +
                                    "WHERE B.CustomerCode = '" + lCustomerCode + "' " +
                                    "AND B.ProjectCode = '" + lProjectCode + "' " +
                                    "AND J.CustomerCode = '" + lCustomerCode + "' " +
                                    "AND J.ProjectCode = '" + lProjectCode + "' " +
                                    "AND J.OrderNumber = S.OrderNumber " +
                                    "AND J.OrderNumber = " + lOrderNumber.ToString() + " " +
                                    "AND B.JobID = S.CABJobID " +
                                    "AND B.JobID = " + lJobID + " " +
                                    "AND B.BBSID <> " + lBBSIDc + " " +
                                    "AND B.BBSNo in (" + lBBSNo + ") " +
                                    "AND B.BBSNo is not null ";

                                lCmd.Connection = lNDSCon;
                                lCmd.CommandTimeout = 300;
                                lRst = lCmd.ExecuteReader();
                                if (lRst.HasRows)
                                {
                                    if (lRst.Read())
                                    {
                                        lDuplicatedBBS = 1;
                                    }
                                }
                                lRst.Close();
                            }

                            lProcessObj1.CloseNDSConnection(ref lNDSCon);
                        }

                        lProcessObj1 = null;


                        if (lDuplicatedBBS == 1)
                        {
                            lErrorMsg = "Found duplicated BBS Number.\n\n 发现重复的加工表号码(BBS No).";
                            return lErrorMsg;
                        }

                        //Check Bar Details

                        int lBBSID = lBBS[j].BBSID;

                        var lBarDet = (from p in db.OrderDetails
                                       join s in db.Shape
                                       on p.BarShapeCode equals s.shapeCode into Shape1
                                       from s1 in Shape1.DefaultIfEmpty()
                                       where p.CustomerCode == lCustomerCode &&
                                       p.ProjectCode == lProjectCode &&
                                       p.JobID == lJobID &&
                                       p.BBSID == lBBSID
                                       orderby p.BarSort
                                       select new OrderDetailsRetModels
                                       {
                                           CustomerCode = p.CustomerCode,
                                           ProjectCode = p.ProjectCode,
                                           JobID = p.JobID,
                                           BBSID = p.BBSID,
                                           BarID = p.BarID,
                                           BarSort = p.BarSort,
                                           Cancelled = p.Cancelled,
                                           ElementMark = p.ElementMark,
                                           BarMark = p.BarMark,
                                           BarType = p.BarType,
                                           BarSize = p.BarSize,
                                           BarCAB = p.BarCAB,
                                           BarSTD = p.BarSTD,
                                           BarMemberQty = p.BarMemberQty,
                                           BarEachQty = p.BarEachQty,
                                           BarTotalQty = p.BarTotalQty,
                                           BarShapeCode = p.BarShapeCode,
                                           A = p.A,
                                           B = p.B,
                                           C = p.C,
                                           D = p.D,
                                           E = p.E,
                                           F = p.F,
                                           G = p.G,
                                           H = p.H,
                                           I = p.I,
                                           J = p.J,
                                           K = p.K,
                                           L = p.L,
                                           M = p.M,
                                           N = p.N,
                                           O = p.O,
                                           P = p.P,
                                           Q = p.Q,
                                           R = p.R,
                                           S = p.S,
                                           T = p.T,
                                           U = p.U,
                                           V = p.V,
                                           W = p.W,
                                           X = p.X,
                                           Y = p.Y,
                                           Z = p.Z,
                                           A2 = p.A2,
                                           B2 = p.B2,
                                           C2 = p.C2,
                                           D2 = p.D2,
                                           E2 = p.E2,
                                           F2 = p.F2,
                                           G2 = p.G2,
                                           H2 = p.H2,
                                           I2 = p.I2,
                                           J2 = p.J2,
                                           K2 = p.K2,
                                           L2 = p.L2,
                                           M2 = p.M2,
                                           N2 = p.N2,
                                           O2 = p.O2,
                                           P2 = p.P2,
                                           Q2 = p.Q2,
                                           R2 = p.R2,
                                           S2 = p.S2,
                                           T2 = p.T2,
                                           U2 = p.U2,
                                           V2 = p.V2,
                                           W2 = p.W2,
                                           X2 = p.X2,
                                           Y2 = p.Y2,
                                           Z2 = p.Z2,
                                           BarLength = p.BarLength,
                                           BarLength2 = p.BarLength2,
                                           BarWeight = p.BarWeight,
                                           Remarks = p.Remarks,
                                           shapeParameters = s1.shapeParameters,
                                           shapeLengthFormula = s1.shapeLengthFormula,
                                           shapeParaValidator = s1.shapeParaValidator,
                                           shapeTransportValidator = s1.shapeTransportValidator,
                                           shapeParType = "",
                                           shapeDefaultValue = "",
                                           shapeHeightCheck = "",
                                           shapeTransport = p.shapeTransport,
                                           PinSize = p.PinSize,
                                           UpdateDate = p.UpdateDate
                                       }).ToList();

                        //check transport 
                        var lTransport = lSE[i].TransportMode;
                        if (lTransport == null || lTransport == "")
                        {
                            lTransport = lHeader.TransportMode;
                        }

                        if (lTransport == "HC")
                        {
                            for (int k = 0; k < lBarDet.Count; k++)
                            {
                                var lShapeCode = lBarDet[k].BarShapeCode;
                                if (lShapeCode != null && lShapeCode != "")
                                {
                                    var lShapeTransportValidator = "";
                                    var lShapes = (from p in db.Shape where p.shapeCode == lShapeCode select p).ToList();
                                    if (lShapes != null && lShapes.Count > 0)
                                    {
                                        lShapeTransportValidator = lShapes[0].shapeTransportValidator;
                                    }

                                    if (lShapeTransportValidator != "" || lShapeCode == "020" || lShapeCode == "20")
                                    {
                                        if (ValidLC(lBarDet[k], lShapeTransportValidator) > 0)
                                        {
                                            lErrorMsg = "Rebar exceed Hiap Crane limitation in BBS SNo " + (j + 1) + " Line " + (k + 1) + "\n\n"
                                                + "钢筋超出起重货车的限制, 位于 BBS 号码 " + (j + 1) + " 行号 " + (k + 1) + ".";

                                            return lErrorMsg;

                                        }
                                    }
                                }
                            }

                        }

                        var lProcessObj = new ProcessController();
                        var lBarDetObj = new OrderDetailsController();
                        if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
                        {
                            for (int k = 0; k < lBarDet.Count; k++)
                            {
                                if (lBarDet[k].BarShapeCode == "020")
                                {
                                    lBarDet[k].shapeParameters = "A";
                                    lBarDet[k].shapeLengthFormula = "A";
                                    lBarDet[k].shapeParType = "S";
                                    lBarDet[k].shapeDefaultValue = "";
                                    lBarDet[k].shapeHeightCheck = "";
                                }
                                else
                                {
                                    //Check same shape processed
                                    var lFound = -1;
                                    if (k > 0)
                                    {
                                        for (int m = 0; m < i; m++)
                                        {
                                            if (lBarDet[m].BarShapeCode == lBarDet[k].BarShapeCode && lBarDet[m].shapeParType != null && lBarDet[m].shapeParType != "")
                                            {
                                                lFound = m;
                                                break;
                                            }
                                        }
                                    }
                                    if (lFound >= 0)
                                    {
                                        lBarDet[k].shapeParameters = lBarDet[lFound].shapeParameters;
                                        lBarDet[k].shapeLengthFormula = lBarDet[lFound].shapeLengthFormula;
                                        lBarDet[k].shapeParType = lBarDet[lFound].shapeParType;
                                        lBarDet[k].shapeDefaultValue = lBarDet[lFound].shapeDefaultValue;
                                        lBarDet[k].shapeHeightCheck = lBarDet[lFound].shapeHeightCheck;
                                    }
                                    else
                                    {
                                        var lReturn = lBarDetObj.getShapeInfoFunAsync(lCustomerCode, lProjectCode, lJobID, lBarDet[k].BarShapeCode, lNDSCon, 0).Result;
                                        if (lReturn != null)
                                        {
                                            lBarDet[k].shapeParameters = lReturn.shapeParameters;
                                            lBarDet[k].shapeLengthFormula = lReturn.shapeLengthFormula;
                                            lBarDet[k].shapeParType = lReturn.shapeParType;
                                            lBarDet[k].shapeDefaultValue = lReturn.shapeDefaultValue;
                                            lBarDet[k].shapeHeightCheck = lReturn.shapeHeightCheck;
                                        }
                                    }
                                }

                                if (lBarDet[k].shapeParameters == null || lBarDet[k].shapeParameters == "")
                                {
                                    var lCustomerShape = db.CustomerShape.Find(lCustomerCode, lProjectCode, lBarDet[k].BarShapeCode);
                                    if (lCustomerShape != null)
                                    {
                                        lBarDet[k].shapeParameters = lCustomerShape.shapeParameters;
                                        lBarDet[k].shapeLengthFormula = lCustomerShape.shapeLengthFormula;
                                        lBarDet[k].shapeParType = "";
                                        lBarDet[k].shapeDefaultValue = "";
                                        lBarDet[k].shapeHeightCheck = "";
                                    }
                                }
                            }
                            lProcessObj.CloseNDSConnection(ref lNDSCon);
                            lProcessObj = null;
                            lBarDetObj = null;
                            lNDSCon = null;

                            if (lBarDet != null && lBarDet.Count > 0)
                            {
                                for (int k = 0; k < lBarDet.Count; k++)
                                {
                                    if (lBarDet[k].Cancelled != true)
                                    {
                                        var lShape = lBarDet[k].BarShapeCode;
                                        var lType = lBarDet[k].BarType;
                                        var lDia = lBarDet[k].BarSize;
                                        var lQty = lBarDet[k].BarTotalQty;

                                        var EspliceShapeIndicator = 0;
                                        lCouplerShapeInd = 0;

                                        if (lShape != null && lShape.Length > 0 &&
                                            (lShape.Substring(0, 1) == "H" ||
                                            lShape.Substring(0, 1) == "I" ||
                                            lShape.Substring(0, 1) == "J" ||
                                            lShape.Substring(0, 1) == "K"))
                                        {
                                            lNCouplerShape = 1;
                                            lCouplerShapeInd = 1;
                                        }

                                        if (lShape != null && lShape.Length > 0 &&
                                            (lShape.Substring(0, 1) == "C" ||
                                            lShape.Substring(0, 1) == "N" ||
                                            lShape.Substring(0, 1) == "P" ||
                                            lShape.Substring(0, 1) == "S"))
                                        {
                                            lECouplerShape = 1;
                                            lCouplerShapeInd = 1;
                                            EspliceShapeIndicator = 1;
                                        }

                                        if (lQty != null && lQty > 0 && (lShape == null || lShape.Trim() == ""))
                                        {
                                            lErrorMsg = "Invalid shape code. BBS SNo " + (j + 1) + " Line " + (k + 1) + "\n\n"
                                                + "请检查图形码, 位于 BBS 号码 " + (j + 1) + " 行号 " + (k + 1) + ".";

                                            return lErrorMsg;

                                        }

                                        if (lShape != null && lShape.Trim() != "" && (lType == null || lType == "" || lType == " " || (lType.Trim() != "H" && lType.Trim() != "X" && lExtraBarType.IndexOf(lType.Trim()) < 0)))
                                        {
                                            lErrorMsg = "Invalid bar type found for shape code " + lShape + " BBS SNo " + (j + 1) + " Line " + (k + 1) + "\n\n"
                                                    + "请检查图形码" + lShape + "的型号, 位于 BBS 号码 " + (j + 1) + " 行号 " + (k + 1) + ".";
                                            return lErrorMsg;
                                        }

                                        if (lType == "C")
                                        {
                                            lType = "H";
                                        }
                                        if (lType == "E")
                                        {
                                            lType = "H";
                                        }
                                        if (lType == "N")
                                        {
                                            lType = "H";
                                        }

                                        if (lShape != null && lShape.Trim() != "" && (lDia == null || lDia == 0 || lDia <= 6))
                                        {
                                            lErrorMsg = "Invalid bar diameter found for shape code " + lShape + " BBS SNo " + (j + 1) + " Line " + (k + 1) + "\n\n"
                                                    + "请检查图形码" + lShape + "的直径, 位于 BBS 号码 " + (j + 1) + " 行号 " + (k + 1) + ".";
                                            return lErrorMsg;
                                        }
                                        if (lShape != null && lShape.Trim() != "" && (lQty == null || lQty == 0))
                                        {
                                            lErrorMsg = "Invalid qty found for shape code " + lShape + " BBS SNo " + (j + 1) + " Line " + (k + 1) + "\n\n"
                                                    + "请检查图形码" + lShape + "的数量, 位于 BBS 号码 " + (j + 1) + " 行号 " + (k + 1) + ".";
                                            return lErrorMsg;
                                        }

                                        if (lCouplerShapeInd == 1 && lDia != null && lDia < 16)
                                        {
                                            lErrorMsg = "For coupler, the smallest bar size is 16mm. BBS SNo " + (j + 1) + " Line " + (k + 1) + "\n\n"
                                                + "对有续接器的钢筋,仅允许钢筋直径大于或等于16mm, 位于 BBS 号码 " + (j + 1) + " 行号 " + (k + 1) + ".";
                                            return lErrorMsg;

                                        }

                                        if (lType == "X" && EspliceShapeIndicator == 1 && lDia != null && lDia < 40)
                                        {
                                            lErrorMsg = "For E-Splice coupler X rebar, the smallest bar size is 40mm.. BBS SNo " + (j + 1) + " Line " + (k + 1) + "\n\n"
                                                + "X等级E-Splice带续接器钢筋的最小直径为40mm, 位于 BBS 号码 " + (j + 1) + " 行号 " + (k + 1) + ".";
                                            return lErrorMsg;

                                        }

                                        //if (lShape == "79A" && lDia > 16) {
                                        //    alert("For shape 79A, only allow bar diameter <= 16mm. BBS SNo " + (i - 1) + " Line " + (j + 1) + "\n\n"
                                        //        + "对图形79A而言,仅允许钢筋直径小于或等于16mm, 位于 BBS 号码 " + (i - 1) + " 行号 " + (j + 1) + ".");
                                        //    return false;
                                        //}

                                        if (lShape == "R7A" && lDia >= 16)
                                        {
                                            lErrorMsg = "For shape R7A, only allow bar diameter < 16mm. BBS SNo " + (i - 1) + " Line " + (j + 1) + "\n\n"
                                                + "对图形R7A而言,仅允许钢筋直径小于16mm, 位于 BBS 号码 " + (i - 1) + " 行号 " + (j + 1) + ".";
                                            return lErrorMsg;
                                        }

                                        var lLength = lBarDet[k].BarLength;
                                        if (lBarDet[k].BarLength2 > lLength)
                                        {
                                            lLength = lBarDet[k].BarLength2;
                                        }

                                        var lLenLimit = lMaxBarLen;
                                        if (lDia <= 8)
                                        {
                                            lLenLimit = 6000;
                                        }
                                        else if (lDia <= 16)
                                        {
                                            lLenLimit = 12000;
                                        }

                                        if (lLength > lLenLimit)
                                        {
                                            int lDed = getDeduction((int)lLength, lBarDet[k]);
                                            if (lLength - lDed > lLenLimit)
                                            {
                                                lErrorMsg = "Total cut bar length is " + (lLength - lDed) + ", which is greater than " + lLenLimit + "mm for BBS SNo " + (j + 1) + " Line " + (k + 1) + ". \n\n"
                                                    + "钢筋的长度已超过" + lLenLimit + "mm.  请检查, 位于 BBS 号码 " + (j + 1) + " 行号 " + (k + 1) + ".";
                                                return lErrorMsg;
                                            }
                                        }

                                        //validate shape 
                                        int lShapeInt = 0;
                                        int.TryParse(lShape, out lShapeInt);

                                        if (lShape != "20" && lShape != "020" && lShape != null && lShape.Trim() != "" && lShapeInt < 950)
                                        {
                                            int lTotal = 0;
                                            lTotal = lTotal + (lBarDet[k].A == null ? 0 : (int)lBarDet[k].A);
                                            lTotal = lTotal + (lBarDet[k].B == null ? 0 : (int)lBarDet[k].B);
                                            lTotal = lTotal + (lBarDet[k].C == null ? 0 : (int)lBarDet[k].C);
                                            lTotal = lTotal + (lBarDet[k].D == null ? 0 : (int)lBarDet[k].D);
                                            lTotal = lTotal + (lBarDet[k].E == null ? 0 : (int)lBarDet[k].E);
                                            lTotal = lTotal + (lBarDet[k].F == null ? 0 : (int)lBarDet[k].F);
                                            lTotal = lTotal + (lBarDet[k].G == null ? 0 : (int)lBarDet[k].G);
                                            lTotal = lTotal + (lBarDet[k].H == null ? 0 : (int)lBarDet[k].H);
                                            lTotal = lTotal + (lBarDet[k].I == null ? 0 : (int)lBarDet[k].I);
                                            lTotal = lTotal + (lBarDet[k].J == null ? 0 : (int)lBarDet[k].J);
                                            lTotal = lTotal + (lBarDet[k].K == null ? 0 : (int)lBarDet[k].K);
                                            lTotal = lTotal + (lBarDet[k].L == null ? 0 : (int)lBarDet[k].L);
                                            lTotal = lTotal + (lBarDet[k].M == null ? 0 : (int)lBarDet[k].M);
                                            lTotal = lTotal + (lBarDet[k].N == null ? 0 : (int)lBarDet[k].N);
                                            lTotal = lTotal + (lBarDet[k].O == null ? 0 : (int)lBarDet[k].O);
                                            lTotal = lTotal + (lBarDet[k].P == null ? 0 : (int)lBarDet[k].P);
                                            lTotal = lTotal + (lBarDet[k].Q == null ? 0 : (int)lBarDet[k].Q);
                                            lTotal = lTotal + (lBarDet[k].R == null ? 0 : (int)lBarDet[k].R);
                                            lTotal = lTotal + (lBarDet[k].S == null ? 0 : (int)lBarDet[k].S);
                                            lTotal = lTotal + (lBarDet[k].T == null ? 0 : (int)lBarDet[k].T);
                                            lTotal = lTotal + (lBarDet[k].U == null ? 0 : (int)lBarDet[k].U);
                                            lTotal = lTotal + (lBarDet[k].V == null ? 0 : (int)lBarDet[k].V);
                                            lTotal = lTotal + (lBarDet[k].W == null ? 0 : (int)lBarDet[k].W);
                                            lTotal = lTotal + (lBarDet[k].X == null ? 0 : (int)lBarDet[k].X);
                                            lTotal = lTotal + (lBarDet[k].Y == null ? 0 : (int)lBarDet[k].Y);
                                            lTotal = lTotal + (lBarDet[k].Z == null ? 0 : (int)lBarDet[k].Z);
                                            if (lTotal <= 0)
                                            {
                                                lErrorMsg = "Invalid Shape for BBS SNo " + (j + 1) + " Line " + (k + 1) + ". \n\n"
                                                    + "请检查图形码, 位于 BBS 号码 " + (j + 1) + " 行号 " + (k + 1) + ".";
                                                return lErrorMsg;
                                            }
                                        }

                                        lLength = lBarDet[k].BarLength;
                                        if (lBarDet[k].BarLength2 != null && lBarDet[k].BarLength2 > 0 && lBarDet[k].BarLength2 < lLength)
                                        {
                                            lLength = lBarDet[k].BarLength2;
                                        }

                                        if (lDia >= 40 && lShape != "20" && lShape != "020" && lLength < 800)
                                        {
                                            int lDed = getDeduction((int)lLength, lBarDet[k]);
                                            if (lLength - lDed < 500)
                                            {
                                                lErrorMsg = "Total bar cut length is " + (lLength - lDed) + ", which is less than minimum value 500mm for BBS SNo " + (j + 1) + " Line " + (k + 1) + ". \n\n"
                                                    + "钢筋的长度已小于最小值500mm.  请检查, 位于 BBS 号码 " + (j + 1) + " 行号 " + (k + 1) + ".";
                                                return lErrorMsg;
                                            }
                                        }

                                        lLength = lBarDet[k].BarLength;

                                        if (lBarDet[k].BarLength2 > 0 && lLength > 0 && lBarDet[k].BarLength2 != lLength && lBarDet[k].BarMemberQty <= 1)
                                        {
                                            lErrorMsg = "Invalid member qty for various bars, it should be greater than 1 for BBS SNo " + (j + 1) + " Line " + (k + 1) + ". \n\n"
                                                + "对变动长钢筋而言, 构件数量不可以小于2.  请检查, 位于 BBS 号码 " + (j + 1) + " 行号 " + (k + 1) + ".";
                                            return lErrorMsg;
                                        }

                                        if (lBarDet[k].BarMemberQty <= 1 &&
                                            ((lBarDet[k].A != null && lBarDet[k].A2 != null && lBarDet[k].A > 0 && lBarDet[k].A2 > 0 && lBarDet[k].A != lBarDet[k].A2) ||
                                            (lBarDet[k].B != null && lBarDet[k].B2 != null && lBarDet[k].B > 0 && lBarDet[k].B2 > 0 && lBarDet[k].B != lBarDet[k].B2) ||
                                            (lBarDet[k].C != null && lBarDet[k].C2 != null && lBarDet[k].C > 0 && lBarDet[k].C2 > 0 && lBarDet[k].C != lBarDet[k].C2) ||
                                            (lBarDet[k].D != null && lBarDet[k].D2 != null && lBarDet[k].D > 0 && lBarDet[k].D2 > 0 && lBarDet[k].D != lBarDet[k].D2) ||
                                            (lBarDet[k].E != null && lBarDet[k].E2 != null && lBarDet[k].E > 0 && lBarDet[k].E2 > 0 && lBarDet[k].E != lBarDet[k].E2) ||
                                            (lBarDet[k].F != null && lBarDet[k].F2 != null && lBarDet[k].F > 0 && lBarDet[k].F2 > 0 && lBarDet[k].F != lBarDet[k].F2) ||
                                            (lBarDet[k].G != null && lBarDet[k].G2 != null && lBarDet[k].G > 0 && lBarDet[k].G2 > 0 && lBarDet[k].G != lBarDet[k].G2) ||
                                            (lBarDet[k].H != null && lBarDet[k].H2 != null && lBarDet[k].H > 0 && lBarDet[k].H2 > 0 && lBarDet[k].H != lBarDet[k].H2) ||
                                            (lBarDet[k].I != null && lBarDet[k].I2 != null && lBarDet[k].I > 0 && lBarDet[k].I2 > 0 && lBarDet[k].I != lBarDet[k].I2) ||
                                            (lBarDet[k].J != null && lBarDet[k].J2 != null && lBarDet[k].J > 0 && lBarDet[k].J2 > 0 && lBarDet[k].J != lBarDet[k].J2) ||
                                            (lBarDet[k].K != null && lBarDet[k].K2 != null && lBarDet[k].K > 0 && lBarDet[k].K2 > 0 && lBarDet[k].K != lBarDet[k].K2) ||
                                            (lBarDet[k].L != null && lBarDet[k].L2 != null && lBarDet[k].L > 0 && lBarDet[k].L2 > 0 && lBarDet[k].L != lBarDet[k].L2) ||
                                            (lBarDet[k].M != null && lBarDet[k].M2 != null && lBarDet[k].M > 0 && lBarDet[k].M2 > 0 && lBarDet[k].M != lBarDet[k].M2) ||
                                            (lBarDet[k].N != null && lBarDet[k].N2 != null && lBarDet[k].N > 0 && lBarDet[k].N2 > 0 && lBarDet[k].N != lBarDet[k].N2) ||
                                            (lBarDet[k].O != null && lBarDet[k].O2 != null && lBarDet[k].O > 0 && lBarDet[k].O2 > 0 && lBarDet[k].O != lBarDet[k].O2) ||
                                            (lBarDet[k].P != null && lBarDet[k].P2 != null && lBarDet[k].P > 0 && lBarDet[k].P2 > 0 && lBarDet[k].P != lBarDet[k].P2) ||
                                            (lBarDet[k].Q != null && lBarDet[k].Q2 != null && lBarDet[k].Q > 0 && lBarDet[k].Q2 > 0 && lBarDet[k].Q != lBarDet[k].Q2) ||
                                            (lBarDet[k].R != null && lBarDet[k].R2 != null && lBarDet[k].R > 0 && lBarDet[k].R2 > 0 && lBarDet[k].R != lBarDet[k].R2) ||
                                            (lBarDet[k].S != null && lBarDet[k].S2 != null && lBarDet[k].S > 0 && lBarDet[k].S2 > 0 && lBarDet[k].S != lBarDet[k].S2) ||
                                            (lBarDet[k].T != null && lBarDet[k].T2 != null && lBarDet[k].T > 0 && lBarDet[k].T2 > 0 && lBarDet[k].T != lBarDet[k].T2) ||
                                            (lBarDet[k].U != null && lBarDet[k].U2 != null && lBarDet[k].U > 0 && lBarDet[k].U2 > 0 && lBarDet[k].U != lBarDet[k].U2) ||
                                            (lBarDet[k].V != null && lBarDet[k].V2 != null && lBarDet[k].V > 0 && lBarDet[k].V2 > 0 && lBarDet[k].V != lBarDet[k].V2) ||
                                            (lBarDet[k].W != null && lBarDet[k].W2 != null && lBarDet[k].W > 0 && lBarDet[k].W2 > 0 && lBarDet[k].W != lBarDet[k].W2) ||
                                            (lBarDet[k].X != null && lBarDet[k].X2 != null && lBarDet[k].X > 0 && lBarDet[k].X2 > 0 && lBarDet[k].Y != lBarDet[k].X2) ||
                                            (lBarDet[k].Y != null && lBarDet[k].Y2 != null && lBarDet[k].Y > 0 && lBarDet[k].Y2 > 0 && lBarDet[k].Z != lBarDet[k].Y2) ||
                                            (lBarDet[k].Z != null && lBarDet[k].Z2 != null && lBarDet[k].Z > 0 && lBarDet[k].Z2 > 0 && lBarDet[k].A != lBarDet[k].Z2)
                                            ))
                                        {
                                            lErrorMsg = "Invalid member qty for various bars, it should be greater than 1 for BBS SNo " + (j + 1) + " Line " + (k + 1) + ". \n\n"
                                                + "对变动长钢筋而言, 构件数量不可以小于2.  请检查, 位于 BBS 号码 " + (j + 1) + " 行号 " + (k + 1) + ".";
                                            return lErrorMsg;
                                        }

                                        lLength = lBarDet[k].BarLength;
                                        if (lBarDet[k].BarLength2 > 0 && lBarDet[k].BarLength2 < lLength)
                                        {
                                            lLength = lBarDet[k].BarLength2;
                                        }

                                        if (UserName != null && UserName.IndexOf("@") > 0
                                            && UserName.Split('@')[1].ToUpper() == "NATSTEEL.COM.SG")
                                        {
                                            if (lShape != null && lShape.Length >= 3)
                                            {
                                                var lFirst = lShape.Substring(0, 1);
                                                var lLast = lShape.Substring(2, 1);
                                                //if (((lFirst == "H" || lFirst == "I" || lFirst == "J" || lFirst == "K")
                                                //&& (lLast == "H" || lLast == "I" || lLast == "J" || lLast == "K"))
                                                //|| ((lFirst == "C" || lFirst == "S" || lFirst == "P" || lFirst == "N")
                                                //&& (lLast == "C" || lLast == "S" || lLast == "P" || lLast == "N")))
                                                //{
                                                //    if (lDia <= 25 && lLength < 550)
                                                //    {
                                                //        lErrorMsg = "The minimum length is 550mm for coupler bar with 2 ends for BBS SNo " + (j + 1) + " Line " + (k + 1) + ". \n\n"
                                                //                + "两头都有续接器的钢筋,其最小长度为550mm.  请检查, 位于 BBS 号码 " + (j + 1) + " 行号 " + (k + 1) + ".";
                                                //        return lErrorMsg;
                                                //    }
                                                //    if (lDia > 25 && lDia <= 32 && lLength < 650)
                                                //    {
                                                //        lErrorMsg = "The minimum length is 650mm for coupler bar with 2 ends, diameter 32mm for BBS SNo " + (j + 1) + " Line " + (k + 1) + ". \n\n"
                                                //                + "两头都有续接器的钢筋,其最小长度为650mm.  请检查, 位于 BBS 号码 " + (j + 1) + " 行号 " + (k + 1) + ".";
                                                //        return lErrorMsg;
                                                //    }
                                                //    if (lDia > 32 && lLength < 700)
                                                //    {
                                                //        lErrorMsg = "The minimum length is 700mm for coupler bar with 2 ends, diameter 40mm and 50mm for BBS SNo " + (j + 1) + " Line " + (k + 1) + ". \n\n"
                                                //                + "两头都有续接器, 直径为40mm,50mm的钢筋,其最小长度为700mm.  请检查, 位于 BBS 号码 " + (j + 1) + " 行号 " + (k + 1) + ".";
                                                //        return lErrorMsg;
                                                //    }
                                                //}


                                                // Condition for H, I, J, K N-Splice
                                                if ((lFirst == "H" || lFirst == "I" || lFirst == "J" || lFirst == "K") &&
                                                    (lLast == "H" || lLast == "I" || lLast == "J" || lLast == "K"))
                                                {
                                                    if (lDia <= 20 && lLength < 550)
                                                    {
                                                        lErrorMsg = "The minimum length is 550mm for coupler bar with 2 ends for BBS SNo " + (j + 1) + " Line " + (k + 1) + ". \n\n"
                                                                   + "两头都有续接器的钢筋,其最小长度为550mm.  请检查, 位于 BBS 号码 " + (j + 1) + " 行号 " + (k + 1) + ".";
                                                        return lErrorMsg;
                                                    }
                                                    if (lDia > 20 && lDia <= 28 && lLength < 530)
                                                    {
                                                        lErrorMsg = "The minimum length is 530mm for coupler bar with 2 ends, diameter 25mm and 28mm for BBS SNo " + (j + 1) + " Line " + (k + 1) + ". \n\n"
                                                                   + "两头都有续接器的钢筋,其最小长度为530mm.  请检查, 位于 BBS 号码 " + (j + 1) + " 行号 " + (k + 1) + ".";
                                                        return lErrorMsg;
                                                    }
                                                    if (lDia > 28 && lDia <= 32 && lLength < 540)
                                                    {
                                                        lErrorMsg = "The minimum length is 540mm for coupler bar with 2 ends, diameter 32mm for BBS SNo " + (j + 1) + " Line " + (k + 1) + ". \n\n"
                                                                   + "两头都有续接器, 直径为32mm,其最小长度为540mm.  请检查, 位于 BBS 号码 " + (j + 1) + " 行号 " + (k + 1) + ".";
                                                        return lErrorMsg;
                                                    }
                                                    if (lDia == 40 && lLength < 600)
                                                    {
                                                        lErrorMsg = "The minimum length is 600mm for coupler bar with 2 ends, diameter 40mm for BBS SNo " + (j + 1) + " Line " + (k + 1) + ". \n\n"
                                                                   + "两头都有续接器, 直径为40mm,其最小长度为600mm.  请检查, 位于 BBS 号码 " + (j + 1) + " 行号 " + (k + 1) + ".";
                                                        return lErrorMsg;
                                                    }
                                                    if (lDia > 40 && lLength < 1100)
                                                    {
                                                        lErrorMsg = "The minimum length is 1100mm for coupler bar with 2 ends, diameter 50mm for BBS SNo " + (j + 1) + " Line " + (k + 1) + ". \n\n"
                                                                   + "两头都有续接器,50mm的钢筋,其最小长度为1100mm.  请检查, 位于 BBS 号码 " + (j + 1) + " 行号 " + (k + 1) + ".";
                                                        return lErrorMsg;
                                                    }
                                                }

                                                // Condition for C, S, P, N  E splice

                                                if ((lFirst == "C" || lFirst == "S" || lFirst == "P" || lFirst == "N") &&
                                                    (lLast == "C" || lLast == "S" || lLast == "P" || lLast == "N"))
                                                {
                                                    if (lDia > 16 && lDia <= 25 && lLength < 800)
                                                    {
                                                        lErrorMsg = "The minimum length is 800mm for coupler bar with 2 ends for BBS SNo " + (j + 1) + " Line " + (k + 1) + ". \n\n"
                                                                   + "两头都有续接器的钢筋,其最小长度为800mm.  请检查, 位于 BBS 号码 " + (j + 1) + " 行号 " + (k + 1) + ".";
                                                        return lErrorMsg;
                                                    }
                                                    if (lDia > 25 && lDia <= 50 && lLength < 1100)
                                                    {
                                                        lErrorMsg = "The minimum length is 1100mm for coupler bar with 2 ends, diameter 28mm, 32mm, 40mm and 50mm for BBS SNo " + (j + 1) + " Line " + (k + 1) + ". \n\n"
                                                                   + "两头都有续接器的钢筋,其最小长度为1100mm.  请检查, 位于 BBS 号码 " + (j + 1) + " 行号 " + (k + 1) + ".";
                                                        return lErrorMsg;
                                                    }

                                                }
                                            }
                                            if (lShape != null && lShape.Length >= 1 && lLength < 500)
                                            {
                                                var lFirst = lShape.Substring(0, 1);
                                                if (lFirst == "H" || lFirst == "I" || lFirst == "J" || lFirst == "K" ||
                                                lFirst == "C" || lFirst == "S" || lFirst == "P" || lFirst == "N")
                                                {
                                                    lErrorMsg = "The minimum length is 500mm for coupler bar with 1 end for BBS SNo " + (j + 1) + " Line " + (k + 1) + ". \n\n"
                                                        + "有续接器的钢筋,其最小长度为500mm. 请检查, 位于 BBS 号码 " + (j + 1) + " 行号 " + (k + 1) + ".";
                                                    return lErrorMsg;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //added by ajit
                                            if (lShape != null && lShape.Length >= 3)
                                            {
                                                var lFirst = lShape.Substring(0, 1);
                                                var lLast = lShape.Substring(2, 1);

                                                // Condition for H, I, J, K N-Splice
                                                if ((lFirst == "H" || lFirst == "I" || lFirst == "J" || lFirst == "K") &&
                                                    (lLast == "H" || lLast == "I" || lLast == "J" || lLast == "K"))
                                                {
                                                    if (lDia <= 20 && lLength < 550)
                                                    {
                                                        lErrorMsg = "The minimum length is 550mm for coupler bar with 2 ends for BBS SNo " + (j + 1) + " Line " + (k + 1) + ". \n\n"
                                                                   + "两头都有续接器的钢筋,其最小长度为550mm.  请检查, 位于 BBS 号码 " + (j + 1) + " 行号 " + (k + 1) + ".";
                                                        return lErrorMsg;
                                                    }
                                                    if (lDia > 20 && lDia <= 28 && lLength < 530)
                                                    {
                                                        lErrorMsg = "The minimum length is 530mm for coupler bar with 2 ends, diameter 25mm and 28mm for BBS SNo " + (j + 1) + " Line " + (k + 1) + ". \n\n"
                                                                   + "两头都有续接器的钢筋,其最小长度为530mm.  请检查, 位于 BBS 号码 " + (j + 1) + " 行号 " + (k + 1) + ".";
                                                        return lErrorMsg;
                                                    }
                                                    if (lDia > 28 &&  lDia <= 32 && lLength < 540)
                                                    {
                                                        lErrorMsg = "The minimum length is 540mm for coupler bar with 2 ends, diameter 32mm for BBS SNo " + (j + 1) + " Line " + (k + 1) + ". \n\n"
                                                                   + "两头都有续接器, 直径为32mm,其最小长度为540mm.  请检查, 位于 BBS 号码 " + (j + 1) + " 行号 " + (k + 1) + ".";
                                                        return lErrorMsg;
                                                    }
                                                    if (lDia == 40 && lLength < 600)
                                                    {
                                                        lErrorMsg = "The minimum length is 600mm for coupler bar with 2 ends, diameter 40mm for BBS SNo " + (j + 1) + " Line " + (k + 1) + ". \n\n"
                                                                   + "两头都有续接器, 直径为40mm,其最小长度为600mm.  请检查, 位于 BBS 号码 " + (j + 1) + " 行号 " + (k + 1) + ".";
                                                        return lErrorMsg;
                                                    }
                                                    if (lDia > 40 && lLength < 1100)
                                                    {
                                                        lErrorMsg = "The minimum length is 1100mm for coupler bar with 2 ends, diameter 50mm for BBS SNo " + (j + 1) + " Line " + (k + 1) + ". \n\n"
                                                                   + "两头都有续接器,50mm的钢筋,其最小长度为1100mm.  请检查, 位于 BBS 号码 " + (j + 1) + " 行号 " + (k + 1) + ".";
                                                        return lErrorMsg;
                                                    }
                                                }

                                                // Condition for C, S, P, N  E splice

                                                if ((lFirst == "C" || lFirst == "S" || lFirst == "P" || lFirst == "N") &&
                                                    (lLast == "C" || lLast == "S" || lLast == "P" || lLast == "N"))
                                                {
                                                    if (lDia > 16 && lDia <= 25 && lLength < 800)
                                                    {
                                                        lErrorMsg = "The minimum length is 800mm for coupler bar with 2 ends for BBS SNo " + (j + 1) + " Line " + (k + 1) + ". \n\n"
                                                                   + "两头都有续接器的钢筋,其最小长度为800mm.  请检查, 位于 BBS 号码 " + (j + 1) + " 行号 " + (k + 1) + ".";
                                                        return lErrorMsg;
                                                    }
                                                    if (lDia > 25 && lDia <= 50 && lLength < 1100)
                                                    {
                                                        lErrorMsg = "The minimum length is 1100mm for coupler bar with 2 ends, diameter 28mm, 32mm, 40mm and 50mm for BBS SNo " + (j + 1) + " Line " + (k + 1) + ". \n\n"
                                                                   + "两头都有续接器的钢筋,其最小长度为1100mm.  请检查, 位于 BBS 号码 " + (j + 1) + " 行号 " + (k + 1) + ".";
                                                        return lErrorMsg;
                                                    }

                                                }
                                            }

                                            if (lShape != null && lShape.Length >= 1)
                                            {
                                                var lFirst = lShape.Substring(0, 1);
                                                //if (lFirst == "H" || lFirst == "I" || lFirst == "J" || lFirst == "K" ||
                                                //lFirst == "C" || lFirst == "S" || lFirst == "P" || lFirst == "N")
                                                //{
                                                //    lErrorMsg = "The minimum length is 500mm for coupler bar with 1 end for BBS SNo " + (j + 1) + " Line " + (k + 1) + ". \n\n"
                                                //        + "有续接器的钢筋,其最小长度为500mm. 请检查, 位于 BBS 号码 " + (j + 1) + " 行号 " + (k + 1) + ".";
                                                //    return lErrorMsg;
                                                //}
                                                // Condition for H, I, J, K N-Splice
                                                if (lFirst == "H" || lFirst == "I" || lFirst == "J" || lFirst == "K") 
                                                {
                                                    if (lDia <= 20 && lLength < 550)
                                                    {
                                                        lErrorMsg = "The minimum length is 550mm for coupler bar with 1 end for BBS SNo " + (j + 1) + " Line " + (k + 1) + ". \n\n"
                                                                   + "两头都有续接器的钢筋,其最小长度为550mm.  请检查, 位于 BBS 号码 " + (j + 1) + " 行号 " + (k + 1) + ".";
                                                        return lErrorMsg;
                                                    }
                                                    if (lDia > 20 && lDia <= 28 && lLength < 530)
                                                    {
                                                        lErrorMsg = "The minimum length is 530mm for coupler bar with 1 end, diameter 25mm and 28mm for BBS SNo " + (j + 1) + " Line " + (k + 1) + ". \n\n"
                                                                   + "两头都有续接器的钢筋,其最小长度为530mm.  请检查, 位于 BBS 号码 " + (j + 1) + " 行号 " + (k + 1) + ".";
                                                        return lErrorMsg;
                                                    }
                                                    if (lDia > 28 && lDia <= 32 && lLength < 540)
                                                    {
                                                        lErrorMsg = "The minimum length is 540mm for coupler bar with 1 end, diameter 32mm for BBS SNo " + (j + 1) + " Line " + (k + 1) + ". \n\n"
                                                                   + "两头都有续接器, 直径为32mm,其最小长度为540mm.  请检查, 位于 BBS 号码 " + (j + 1) + " 行号 " + (k + 1) + ".";
                                                        return lErrorMsg;
                                                    }
                                                    if (lDia == 40 && lLength < 600)
                                                    {
                                                        lErrorMsg = "The minimum length is 600mm for coupler bar with 1 end, diameter 40mm for BBS SNo " + (j + 1) + " Line " + (k + 1) + ". \n\n"
                                                                   + "两头都有续接器, 直径为40mm,其最小长度为600mm.  请检查, 位于 BBS 号码 " + (j + 1) + " 行号 " + (k + 1) + ".";
                                                        return lErrorMsg;
                                                    }
                                                    if (lDia > 40 && lLength < 1100)
                                                    {
                                                        lErrorMsg = "The minimum length is 1100mm for coupler bar with 1 end, diameter 50mm for BBS SNo " + (j + 1) + " Line " + (k + 1) + ". \n\n"
                                                                   + "两头都有续接器,50mm的钢筋,其最小长度为1100mm.  请检查, 位于 BBS 号码 " + (j + 1) + " 行号 " + (k + 1) + ".";
                                                        return lErrorMsg;
                                                    }
                                                }

                                                // Condition for C, S, P, N  E splice

                                                if (lFirst == "C" || lFirst == "S" || lFirst == "P" || lFirst == "N") 
                                                {
                                                    if (lDia <= 25 && lLength < 700)
                                                    {
                                                        lErrorMsg = "The minimum length is 700mm for coupler bar with 1 end for BBS SNo " + (j + 1) + " Line " + (k + 1) + ". \n\n"
                                                                   + "两头都有续接器的钢筋,其最小长度为700mm.  请检查, 位于 BBS 号码 " + (j + 1) + " 行号 " + (k + 1) + ".";
                                                        return lErrorMsg;
                                                    }
                                                    if (lDia > 25 && lDia <= 50 && lLength < 1000)
                                                    {
                                                        lErrorMsg = "The minimum length is 1000mm for coupler bar with 1 end, diameter 28mm, 32mm, 40mm and 50mm for BBS SNo " + (j + 1) + " Line " + (k + 1) + ". \n\n"
                                                                   + "两头都有续接器的钢筋,其最小长度为1000mm.  请检查, 位于 BBS 号码 " + (j + 1) + " 行号 " + (k + 1) + ".";
                                                        return lErrorMsg;
                                                    }

                                                }
                                            }


                                        }

                                        var lSB = lBarDet[k].BarSTD;
                                        var lWT = lBarDet[k].BarWeight;

                                        if (lSB == true && lLength != null && (lLength == 12000) && lType != null && lType.Trim() != "R" && lType.Trim() != "X" && ((lWT % 2000) > 0 || lWT < 2000))
                                        {
                                            lErrorMsg = "Invalid weight value entered for  BBS SNo " + (j + 1) + " Line " + (k + 1) + ". 12m SB product can only order by bundles (2 tons per bundle). Please enter valid weight, such as 2000, 4000, 6000." +
                                                "(输入的重量无效, 位于 BBS 号码 " + (j + 1) + " 行号 " + (k + 1) + ". 只能按捆来订购12米标准直铁产品(每捆2吨). 您可输入 2000, 4000, 6000 等等.)";
                                            return lErrorMsg;
                                        }

                                        if (lSB == true && lLength != null && lType != null && (lLength == 14000 || (lLength == 12000 && lType.Trim() == "X")) && lType.Trim() != "R" && (lWT < 2000))
                                        {
                                            lErrorMsg = "Invalid weight value entered for  BBS SNo " + (j + 1) + " Line " + (k + 1) + ". 14m or 12m X grade SB product can only order by bundles (2+ tons per bundle). Please enter valid weight." +
                                                "(输入的重量无效, 位于 BBS 号码 " + (j + 1) + " 行号 " + (k + 1) + ". 只能按捆来订购14米标准直铁产品(每捆2吨).";
                                            return lErrorMsg;
                                        }

                                        if (lSB == true && lType != null && lLength != null && (lLength == 6000 || lType.Trim() == "R") && ((lWT % 1000) > 0 || lWT < 1000))
                                        {
                                            lErrorMsg = "Invalid weight value entered for  BBS SNo " + (j + 1) + " Line " + (k + 1) + ". 6m SB product can only order by bundles (1 tons per bundle). Please enter valid weight, such as 1000, 2000, 3000." +
                                                "(输入的重量无效, 位于 BBS 号码 " + (j + 1) + " 行号 " + (k + 1) + ". 只能按捆来订购6米标准直铁产品(每捆1吨). 您可输入 1000, 2000, 3000 等等.)";
                                            return lErrorMsg;
                                        }

                                        var lParameters = lBarDet[k].shapeParameters;
                                        if (lShape != null && lShape.Trim() != "" && lParameters != null && lParameters != "")
                                        {
                                            var lPinS = lPinMaster.Find(item => item.grade == (lBarDet[k].BarType == "C" ? "H" : lBarDet[k].BarType) &&
                                            item.type == "S" &&
                                            item.dia == lBarDet[k].BarSize);

                                            var lPinN = lPinMaster.Find(item => item.grade == (lBarDet[k].BarType == "C" ? "H" : lBarDet[k].BarType) &&
                                            item.type == "N" &&
                                            item.dia == lBarDet[k].BarSize);

                                            var lParaA = lParameters.Split(',');
                                            for (int m = 0; m < lParaA.Length; m++)
                                            {
                                                int lValue1 = 0;
                                                int lValue2 = 0;

                                                getParaValue(lBarDet[k], lParaA[m], ref lValue1, ref lValue2);

                                                if (lValue1 <= 0)
                                                {
                                                    lErrorMsg = "Invalid shape parameter found for shape code " + lShape + " parameter " + lParaA[m] + " BBS SNo " + (j + 1) + " Line " + (k + 1) + "\n\n"
                                                        + "请检擦图形码" + lShape + ", 参数" + lParaA[m] + "的数值, 位于 BBS 号码 " + (j + 1) + " 行号 " + (k + 1) + ".";
                                                    return lErrorMsg;
                                                }
                                                if (lValue1 > 0)
                                                {
                                                    if (lValue1 == lValue2)
                                                    {
                                                        lErrorMsg = "Invalid shape parameter found for shape code " + lShape + " variant parameter " + lValue1 + "-" + lValue2 + " BBS SNo " + (j + 1) + " Line " + (k + 1) + "\n\n"
                                                            + "请检擦图形码" + lShape + "的参数数值, 位于 BBS 号码 " + (j + 1) + " 行号 " + (k + 1) + ".";
                                                        return lErrorMsg;
                                                    }
                                                }

                                                if (lSkipBendingCheck != true)
                                                {
                                                    lErrorMsg = isValidValue(lParaA[m], lParameters, (int)lDia, lValue1, lValue2, lBarDet[k], lPinS, lPinN, lAddnLimitShape, lAddnLimit, lAddnParLimit);
                                                    if (lErrorMsg != "")
                                                    {
                                                        lErrorMsg = "Shape code " + lShape + " parameter " + lParaA[m] + " BBS SNo " + (j + 1) + " Line " + (k + 1) + "\n\n"
                                                            + "请检擦图形码" + lShape + ", 参数" + lParaA[m] + "的数值, 位于 BBS 号码 " + (j + 1) + " 行号 " + (k + 1) + ".\n\n"
                                                            + lErrorMsg;
                                                        return lErrorMsg;
                                                    }
                                                }

                                                //if (lShape != null && lShape != "20" && lShape != "020" && lDia == 8 && (lValue1 > 1800 || lValue2 > 1800))
                                                //if (lShape != null && ((lDia == 8 && (lShape == "020" || lShape == "20") && lValue1 != 6000 && (lValue1 > 1800 || lValue2 > 1800)) ||
                                                //(lDia == 8 && lShape != "020" && lShape != "20" && (lValue1 > 1800 || lValue2 > 1800))))
                                                //{

                                                //    lErrorMsg = "The maximum segment length is 1800mm for 8mm bar. (对 8mm 钢筋, 最大节长为1800mm)";
                                                //    return lErrorMsg;
                                                //}

                                                // Check various bars
                                                var lMbrQty = lBarDet[k].BarMemberQty;
                                                var lParaType = lBarDet[k].shapeParType;
                                                var lParaTypeA = lParaType.Split(',');
                                                if (lParaTypeA.Length > m && lParaTypeA[m] == "S" && lValue1 > 0 && lValue2 > 0 && lMbrQty != null)
                                                {
                                                    var lMax = getVarMaxValue((int)lValue1, (int)lValue2);
                                                    var lMin = getVarMinValue((int)lValue1, (int)lValue2);
                                                    if (lMbrQty > 1 && lMin > 0 && lMax > 0 && lMax > lMin)
                                                    {
                                                        var lHeight = (int)Math.Round((double)((double)(lMax - lMin) / ((int)lMbrQty - 1)));
                                                        if (lHeight < 18)
                                                        {
                                                            var lCheckLen = true;
                                                            var lLen1 = lBarDet[k].BarLength;
                                                            var lLen2 = lBarDet[k].BarLength2;
                                                            if (lLen1 != null && lLen2 != null)
                                                            {
                                                                lMax = getVarMaxValue((int)lLen1, (int)lLen2);
                                                                lMin = getVarMinValue((int)lLen1, (int)lLen2);
                                                                if (lMax > 0 || lMin > 0 || lMin < lMax)
                                                                {
                                                                    lCheckLen = false;
                                                                    lHeight = (int)Math.Round((double)((double)(lMax - lMin) / ((int)lMbrQty - 1)));
                                                                    if (lHeight < 18)
                                                                    {
                                                                        lCheckLen = true;
                                                                    }
                                                                }
                                                            }
                                                            if (lCheckLen == true)
                                                            {
                                                                lErrorMsg = "Invalid data entered. Difference of various bars is less than 18mm minimum limit. Please increase the difference of verious bar or reduce Member Qty.(输入数据无效, 可变钢筋的差值已小于18mm的最小值. 请增加差值或减少构件数)";
                                                                return lErrorMsg;
                                                            }
                                                        }
                                                    }
                                                }

                                            }
                                        }

                                        //check imported 
                                        #region Check Angle and Height that bring from imported Excel

                                        var lShapeNum = 0;
                                        int.TryParse(lShape, out lShapeNum);

                                        if (lShape != null && lShape.Trim() != "" && (lShapeNum == 0 || lShapeNum < 950 || lShapeNum > 999))
                                        {
                                            var lTestItem = new OrderDetailsModels
                                            {
                                                CustomerCode = lBarDet[k].CustomerCode,
                                                ProjectCode = lBarDet[k].ProjectCode,
                                                JobID = lBarDet[k].JobID,
                                                BBSID = lBarDet[k].BBSID,
                                                BarID = lBarDet[k].BarID,
                                                BarSort = lBarDet[k].BarSort,
                                                Cancelled = lBarDet[k].Cancelled,
                                                ElementMark = lBarDet[k].ElementMark,
                                                BarMark = lBarDet[k].BarMark,
                                                BarType = lBarDet[k].BarType,
                                                BarSize = lBarDet[k].BarSize,
                                                BarCAB = lBarDet[k].BarCAB,
                                                BarSTD = lBarDet[k].BarSTD,
                                                BarMemberQty = lBarDet[k].BarMemberQty,
                                                BarEachQty = lBarDet[k].BarEachQty,
                                                BarTotalQty = lBarDet[k].BarTotalQty,
                                                BarShapeCode = lBarDet[k].BarShapeCode,
                                                A = lBarDet[k].A,
                                                B = lBarDet[k].B,
                                                C = lBarDet[k].C,
                                                D = lBarDet[k].D,
                                                E = lBarDet[k].E,
                                                F = lBarDet[k].F,
                                                G = lBarDet[k].G,
                                                H = lBarDet[k].H,
                                                I = lBarDet[k].I,
                                                J = lBarDet[k].J,
                                                K = lBarDet[k].K,
                                                L = lBarDet[k].L,
                                                M = lBarDet[k].M,
                                                N = lBarDet[k].N,
                                                O = lBarDet[k].O,
                                                P = lBarDet[k].P,
                                                Q = lBarDet[k].Q,
                                                R = lBarDet[k].R,
                                                S = lBarDet[k].S,
                                                T = lBarDet[k].T,
                                                U = lBarDet[k].U,
                                                V = lBarDet[k].V,
                                                W = lBarDet[k].W,
                                                X = lBarDet[k].X,
                                                Y = lBarDet[k].Y,
                                                Z = lBarDet[k].Z,
                                                A2 = lBarDet[k].A2,
                                                B2 = lBarDet[k].B2,
                                                C2 = lBarDet[k].C2,
                                                D2 = lBarDet[k].D2,
                                                E2 = lBarDet[k].E2,
                                                F2 = lBarDet[k].F2,
                                                G2 = lBarDet[k].G2,
                                                H2 = lBarDet[k].H2,
                                                I2 = lBarDet[k].I2,
                                                J2 = lBarDet[k].J2,
                                                K2 = lBarDet[k].K2,
                                                L2 = lBarDet[k].L2,
                                                M2 = lBarDet[k].M2,
                                                N2 = lBarDet[k].N2,
                                                O2 = lBarDet[k].O2,
                                                P2 = lBarDet[k].P2,
                                                Q2 = lBarDet[k].Q2,
                                                R2 = lBarDet[k].R2,
                                                S2 = lBarDet[k].S2,
                                                T2 = lBarDet[k].T2,
                                                U2 = lBarDet[k].U2,
                                                V2 = lBarDet[k].V2,
                                                W2 = lBarDet[k].W2,
                                                X2 = lBarDet[k].X2,
                                                Y2 = lBarDet[k].Y2,
                                                Z2 = lBarDet[k].Z2,
                                                BarLength = lBarDet[k].BarLength,
                                                BarLength2 = lBarDet[k].BarLength2,
                                                BarWeight = lBarDet[k].BarWeight,
                                                Remarks = lBarDet[k].Remarks,
                                                shapeTransport = lBarDet[k].shapeTransport,
                                                PinSize = lBarDet[k].PinSize,
                                                UpdateDate = lBarDet[k].UpdateDate
                                            };

                                            var lShapeFormular = lOrderDet.getShapeInfoFunDB(lBarDet[k].CustomerCode, lBarDet[k].ProjectCode, lBarDet[k].JobID, lShape, 0).Result;

                                            lOrderDet.testDependValue(ref lTestItem, lShapeFormular, lBarDet[k].PinSize.ToString(), lBarDet[k].BarSize.ToString());
                                            var lError = 0;

                                            var lParaA = lBarDet[k].shapeParameters.Split(',');
                                            var lParaTypeA = lBarDet[k].shapeParType.Split(',');
                                            for (int m = 0; m < lParaA.Length; m++)
                                            {
                                                if (lParaTypeA[m] != null)
                                                {
                                                    var lValue1 = 0;
                                                    var lValue2 = 0;
                                                    getParaValue(lBarDet[k], lParaA[m], ref lValue1, ref lValue2);

                                                    var lMaxOld = getVarMaxValue(lValue1, lValue2);
                                                    var lMinOld = getVarMinValue(lValue1, lValue2);

                                                    lValue1 = 0;
                                                    lValue2 = 0;
                                                    getODParaValue(lTestItem, lParaA[m], ref lValue1, ref lValue2);
                                                    var lMaxNew = getVarMaxValue(lValue1, lValue2);
                                                    var lMinNew = getVarMinValue(lValue1, lValue2);

                                                    if (lParaTypeA[m] != "V")
                                                    {
                                                        var lMaxDiff = 10;
                                                        if ((lMaxOld > 0 && lMaxNew > 0 && Math.Abs(lMaxOld - lMaxNew) > lMaxDiff) ||
                                                            (lMinOld > 0 && lMinNew > 0 && Math.Abs(lMinOld - lMinNew) > lMaxDiff))
                                                        {
                                                            lError = 1;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        var lMaxDiff = 5;
                                                        if (Math.Abs(lMaxOld - lMaxNew) > lMaxDiff || Math.Abs(lMinOld - lMinNew) > lMaxDiff)
                                                        {
                                                            lError = 1;
                                                        }
                                                    }
                                                }
                                            }

                                            if (lError != 0)
                                            {
                                                var lTestItem2 = new OrderDetailsModels
                                                {
                                                    CustomerCode = lBarDet[k].CustomerCode,
                                                    ProjectCode = lBarDet[k].ProjectCode,
                                                    JobID = lBarDet[k].JobID,
                                                    BBSID = lBarDet[k].BBSID,
                                                    BarID = lBarDet[k].BarID,
                                                    BarSort = lBarDet[k].BarSort,
                                                    Cancelled = lBarDet[k].Cancelled,
                                                    ElementMark = lBarDet[k].ElementMark,
                                                    BarMark = lBarDet[k].BarMark,
                                                    BarType = lBarDet[k].BarType,
                                                    BarSize = lBarDet[k].BarSize,
                                                    BarCAB = lBarDet[k].BarCAB,
                                                    BarSTD = lBarDet[k].BarSTD,
                                                    BarMemberQty = lBarDet[k].BarMemberQty,
                                                    BarEachQty = lBarDet[k].BarEachQty,
                                                    BarTotalQty = lBarDet[k].BarTotalQty,
                                                    BarShapeCode = lBarDet[k].BarShapeCode,
                                                    A = lBarDet[k].A,
                                                    B = lBarDet[k].B,
                                                    C = lBarDet[k].C,
                                                    D = lBarDet[k].D,
                                                    E = lBarDet[k].E,
                                                    F = lBarDet[k].F,
                                                    G = lBarDet[k].G,
                                                    H = lBarDet[k].H,
                                                    I = lBarDet[k].I,
                                                    J = lBarDet[k].J,
                                                    K = lBarDet[k].K,
                                                    L = lBarDet[k].L,
                                                    M = lBarDet[k].M,
                                                    N = lBarDet[k].N,
                                                    O = lBarDet[k].O,
                                                    P = lBarDet[k].P,
                                                    Q = lBarDet[k].Q,
                                                    R = lBarDet[k].R,
                                                    S = lBarDet[k].S,
                                                    T = lBarDet[k].T,
                                                    U = lBarDet[k].U,
                                                    V = lBarDet[k].V,
                                                    W = lBarDet[k].W,
                                                    X = lBarDet[k].X,
                                                    Y = lBarDet[k].Y,
                                                    Z = lBarDet[k].Z,
                                                    A2 = lBarDet[k].A2,
                                                    B2 = lBarDet[k].B2,
                                                    C2 = lBarDet[k].C2,
                                                    D2 = lBarDet[k].D2,
                                                    E2 = lBarDet[k].E2,
                                                    F2 = lBarDet[k].F2,
                                                    G2 = lBarDet[k].G2,
                                                    H2 = lBarDet[k].H2,
                                                    I2 = lBarDet[k].I2,
                                                    J2 = lBarDet[k].J2,
                                                    K2 = lBarDet[k].K2,
                                                    L2 = lBarDet[k].L2,
                                                    M2 = lBarDet[k].M2,
                                                    N2 = lBarDet[k].N2,
                                                    O2 = lBarDet[k].O2,
                                                    P2 = lBarDet[k].P2,
                                                    Q2 = lBarDet[k].Q2,
                                                    R2 = lBarDet[k].R2,
                                                    S2 = lBarDet[k].S2,
                                                    T2 = lBarDet[k].T2,
                                                    U2 = lBarDet[k].U2,
                                                    V2 = lBarDet[k].V2,
                                                    W2 = lBarDet[k].W2,
                                                    X2 = lBarDet[k].X2,
                                                    Y2 = lBarDet[k].Y2,
                                                    Z2 = lBarDet[k].Z2,
                                                    BarLength = lBarDet[k].BarLength,
                                                    BarLength2 = lBarDet[k].BarLength2,
                                                    BarWeight = lBarDet[k].BarWeight,
                                                    Remarks = lBarDet[k].Remarks,
                                                    shapeTransport = lBarDet[k].shapeTransport,
                                                    PinSize = lBarDet[k].PinSize,
                                                    UpdateDate = lBarDet[k].UpdateDate
                                                };
                                                lOrderDet.testDependValueHeight(ref lTestItem2, lShapeFormular, lBarDet[k].PinSize.ToString(), lBarDet[k].BarSize.ToString());
                                                for (int m = 0; m < lParaA.Length; m++)
                                                {
                                                    if (lParaTypeA[m] != null)
                                                    {
                                                        var lValue1 = 0;
                                                        var lValue2 = 0;
                                                        getParaValue(lBarDet[k], lParaA[m], ref lValue1, ref lValue2);

                                                        var lMaxOld = getVarMaxValue(lValue1, lValue2);
                                                        var lMinOld = getVarMinValue(lValue1, lValue2);

                                                        lValue1 = 0;
                                                        lValue2 = 0;
                                                        getODParaValue(lTestItem2, lParaA[m], ref lValue1, ref lValue2);
                                                        var lMaxNew = getVarMaxValue(lValue1, lValue2);
                                                        var lMinNew = getVarMinValue(lValue1, lValue2);

                                                        if (lParaTypeA[m] != "V")
                                                        {
                                                            var lMaxDiff = 10;
                                                            if ((lMaxOld > 0 && lMaxNew > 0 && Math.Abs(lMaxOld - lMaxNew) > lMaxDiff) ||
                                                                (lMinOld > 0 && lMinNew > 0 && Math.Abs(lMinOld - lMinNew) > lMaxDiff))
                                                            {

                                                                lErrorMsg = "Invalid value for Shape code " + lShape + " parameter " + lParaA[m] + " at BBS SNo " + (j + 1) + " Line " + (k + 1)
                                                                    + ". Its value should be " + (lMaxNew == lMinNew ? lMaxNew.ToString() : lMinNew.ToString() + " - " + lMaxNew.ToString()) + "\n\n"
                                                                    + "请检擦图形码" + lShape + ", 参数" + lParaA[m] + "的数值, 位于 BBS 号码 " + (j + 1) + " 行号 " + (k + 1) + ".\n\n"
                                                                    + "它的数值应该是 " + (lMaxNew == lMinNew ? lMaxNew.ToString() : lMinNew.ToString() + " - " + lMaxNew.ToString());
                                                                return lErrorMsg;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            var lMaxDiff = 5;
                                                            if (Math.Abs(lMaxOld - lMaxNew) > lMaxDiff || Math.Abs(lMinOld - lMinNew) > lMaxDiff)
                                                            {
                                                                lErrorMsg = "Invalid value for Shape code " + lShape + " parameter " + lParaA[m] + " at BBS SNo " + (j + 1) + " Line " + (k + 1)
                                                                    + ". Its value should be " + (lMaxNew == lMinNew ? lMaxNew.ToString() : lMinNew.ToString() + " - " + lMaxNew.ToString()) + "\n\n"
                                                                    + "请检擦图形码" + lShape + ", 参数" + lParaA[m] + "的数值, 位于 BBS 号码 " + (j + 1) + " 行号 " + (k + 1) + ".\n\n"
                                                                    + "它的数值应该是 " + (lMaxNew == lMinNew ? lMaxNew.ToString() : lMinNew.ToString() + " - " + lMaxNew.ToString());
                                                                return lErrorMsg;
                                                            }
                                                        }
                                                    }
                                                }

                                            }
                                        }


                                        #endregion

                                    }
                                }
                            }
                        }

                        //Check max of various bar items <= 26
                        if (lVarianceBarSplit == true)
                        {
                            var lVBCt = (from p in db.OrderDetails
                                         where p.CustomerCode == lCustomerCode &&
                                         p.ProjectCode == lProjectCode &&
                                         p.JobID == lJobID &&
                                         p.BBSID == lBBSID &&
                                         (p.BarSTD == null ||
                                         p.BarSTD == false) &&
                                         (p.Cancelled == null ||
                                         p.Cancelled == false) &&
                                         p.BarEachQty > 0 &&
                                         p.BarMemberQty > 0 &&
                                         p.BarTotalQty >= 5 &&
                                         p.BarShapeCode != null &&
                                         p.BarShapeCode != "" &&
                                         (p.A2 > 0 ||
                                         p.B2 > 0 ||
                                         p.C2 > 0 ||
                                         p.D2 > 0 ||
                                         p.E2 > 0 ||
                                         p.F2 > 0 ||
                                         p.G2 > 0 ||
                                         p.H2 > 0 ||
                                         p.I2 > 0 ||
                                         p.J2 > 0 ||
                                         p.K2 > 0 ||
                                         p.L2 > 0 ||
                                         p.M2 > 0 ||
                                         p.N2 > 0 ||
                                         p.O2 > 0 ||
                                         p.P2 > 0 ||
                                         p.Q2 > 0 ||
                                         p.R2 > 0 ||
                                         p.S2 > 0 ||
                                         p.T2 > 0 ||
                                         p.U2 > 0 ||
                                         p.V2 > 0 ||
                                         p.W2 > 0 ||
                                         p.X2 > 0 ||
                                         p.Y2 > 0 ||
                                         p.Z2 > 0)
                                         select p
                                        ).Count();

                            if (lVBCt > 26)
                            {
                                lErrorMsg = "Exceed the maximum of various bar items 26 in BBS " + lBBS[j].BBSNo + ". Please split the BBS by creating another order / BBS.\n\n"
                                    + "在BBS号码" + lBBS[j].BBSNo + "中, 已超出变长钢筋最多行数的限制 26，请创建另一个订单/BBS来拆分此BBS.";

                                return lErrorMsg;

                            }
                        }

                    }


                    if (lJobAdv != null && lJobAdv.CouplerType != null && lJobAdv.CouplerType.ToUpper().Trim() != "N-SPLICE" && lNCouplerShape == 1)
                    {
                        lErrorMsg = "Please choose N-Splice Coupler Type before submission as the order includes N-Splice Coupler shape in CAB BBS details. \n\n请选择续接器类型再提交此订单.";
                        return lErrorMsg;
                    }

                    if (lJobAdv != null && lJobAdv.CouplerType != null && lJobAdv.CouplerType.ToUpper().Trim() != "E-SPLICE(S)" && lJobAdv.CouplerType.ToUpper().Trim() != "E-SPLICE(N)" && lECouplerShape == 1)
                    {
                        lErrorMsg = "Please choose E-Splice Coupler Type before submission as the order includes E-Splice Coupler shape in CAB BBS details. \n\n请选择续接器类型再提交此订单.";
                        return lErrorMsg;
                    }

                    if (lJobAdv != null && lECouplerShape == 0 && lNCouplerShape == 0 && (lJobAdv.CouplerType == null || lJobAdv.CouplerType.ToUpper().Trim() != "NO COUPLER"))
                    {
                        var lJobAdvNew = lJobAdv;
                        lJobAdvNew.CouplerType = "No Coupler";
                        db.Entry(lJobAdv).CurrentValues.SetValues(lJobAdvNew);

                        db.SaveChanges();
                    }

                    int lPMDOnly = 1;
                    var lUserType = "";

                    UserAccessController lUa = new UserAccessController();

                    for (int j = 0; j < lBBS.Count; j++)
                    {
                        int BBSID = lBBS[j].BBSID;
                        var lUpdatedBy = (from p in db.OrderDetails
                                          where p.CustomerCode == lCustomerCode &&
                                          p.ProjectCode == lProjectCode &&
                                          p.JobID == lJobID &&
                                          p.BBSID == BBSID &&
                                          p.Cancelled == null &&
                                          ((p.BarShapeCode != null &&
                                          p.BarShapeCode != "") ||
                                          (p.BarSize != null &&
                                          p.BarSize > 0) ||
                                          (p.BarEachQty != null &&
                                          p.BarEachQty > 0) ||
                                          (p.BarMemberQty != null &&
                                          p.BarMemberQty > 0))
                                          select p.UpdateBy).Distinct().ToList();

                        if (lUpdatedBy.Count > 0)
                        {
                            for (int k = 0; k < lUpdatedBy.Count; k++)
                            {
                                lUserType = lUa.getUserType(lUpdatedBy[k]);

                                if (lUserType != "AD" && lUserType != "PM"
                                && lUserType != "PA" && lUserType != "P1"
                                && lUserType != "P2" && lUserType != "PU"
                                && lUserType != "CU" && lUserType != "TE")
                                {
                                    lPMDOnly = 0;
                                    break;
                                }

                            }
                        }

                        if (lPMDOnly == 0)
                        {
                            break;
                        }
                    }

                    lUa = null;

                    if (UserName != null && UserName.IndexOf("@") > 0
                    && UserName.Split('@')[1].ToUpper() == "NATSTEEL.COM.SG"
                    && lJobAdv != null && lJobAdv.OrderSource != "UXE"
                    && lPMDOnly == 0)
                    {
                        var lReport = new Reports();

                        for (int j = 0; j < lBBS.Count; j++)
                        {
                            int BBSID = lBBS[j].BBSID;
                            var lOrderDetails = (from p in db.OrderDetails
                                                 where p.CustomerCode == lCustomerCode &&
                                                 p.ProjectCode == lProjectCode &&
                                                 p.JobID == lJobID &&
                                                 p.BBSID == BBSID &&
                                                 p.Cancelled == null &&
                                                 ((p.BarShapeCode != null &&
                                                 p.BarShapeCode != "") ||
                                                 (p.BarSize != null &&
                                                 p.BarSize > 0) ||
                                                 (p.BarEachQty != null &&
                                                 p.BarEachQty > 0) ||
                                                 (p.BarMemberQty != null &&
                                                 p.BarMemberQty > 0))
                                                 orderby p.BarShapeCode, p.A, p.B, p.C, p.D, p.E, p.F, p.G, p.H, p.I, p.J, p.K, p.L, p.M, p.N, p.O, p.P, p.Q, p.R, p.S, p.T, p.U, p.V, p.W, p.X, p.Y, p.Z, p.BarLength, p.BarType, p.BarSize, p.BarMemberQty, p.BarEachQty, p.BarWeight
                                                 select p).ToList();

                            var lOrderDetailsDBL = (from p in db.OrderDetailsDouble
                                                    where p.CustomerCode == lCustomerCode &&
                                                    p.ProjectCode == lProjectCode &&
                                                    p.JobID == lJobID &&
                                                    p.BBSID == BBSID &&
                                                    p.Cancelled == null &&
                                                    ((p.BarShapeCode != null &&
                                                    p.BarShapeCode != "") ||
                                                    (p.BarSize != null &&
                                                    p.BarSize > 0) ||
                                                    (p.BarEachQty != null &&
                                                    p.BarEachQty > 0) ||
                                                    (p.BarMemberQty != null &&
                                                    p.BarMemberQty > 0))
                                                    orderby p.BarShapeCode, p.A, p.B, p.C, p.D, p.E, p.F, p.G, p.H, p.I, p.J, p.K, p.L, p.M, p.N, p.O, p.P, p.Q, p.R, p.S, p.T, p.U, p.V, p.W, p.X, p.Y, p.Z, p.BarLength, p.BarType, p.BarSize, p.BarMemberQty, p.BarEachQty, p.BarWeight
                                                    select p).ToList();

                            if (lOrderDetails.Count > 0 && lOrderDetailsDBL.Count > 0 && lOrderDetails.Count == lOrderDetailsDBL.Count)
                            {
                                // check if the double capture empty 
                                int lFound = 0;

                                for (int m = 0; m < lOrderDetailsDBL.Count; m++)
                                {
                                    if ((lOrderDetailsDBL[m].BarEachQty != null && lOrderDetailsDBL[m].BarEachQty > 0) ||
                                        (lOrderDetailsDBL[m].BarMemberQty != null && lOrderDetailsDBL[m].BarMemberQty > 0) ||
                                        (lOrderDetailsDBL[m].BarShapeCode != null && lOrderDetailsDBL[m].BarShapeCode != "") ||
                                        (lOrderDetailsDBL[m].BarType != null && lOrderDetailsDBL[m].BarType != "") ||
                                        (lOrderDetailsDBL[m].BarSize != null && lOrderDetailsDBL[m].BarSize > 0))
                                    {
                                        lFound = 1;
                                        break;
                                    }

                                }
                                if (lFound == 0)
                                {
                                    continue;
                                }

                                var lPara = new System.Collections.Generic.List<string>();
                                lPara.Add("A");
                                lPara.Add("B");
                                lPara.Add("C");
                                lPara.Add("D");
                                for (int m = 0; m < lOrderDetails.Count; m++)
                                {
                                    lReport.AddParamaters("E", lOrderDetails[m].E, ref lPara);
                                    lReport.AddParamaters("F", lOrderDetails[m].F, ref lPara);
                                    lReport.AddParamaters("G", lOrderDetails[m].G, ref lPara);
                                    lReport.AddParamaters("H", lOrderDetails[m].H, ref lPara);
                                    lReport.AddParamaters("I", lOrderDetails[m].I, ref lPara);
                                    lReport.AddParamaters("J", lOrderDetails[m].J, ref lPara);
                                    lReport.AddParamaters("K", lOrderDetails[m].K, ref lPara);
                                    lReport.AddParamaters("L", lOrderDetails[m].L, ref lPara);
                                    lReport.AddParamaters("M", lOrderDetails[m].M, ref lPara);
                                    lReport.AddParamaters("N", lOrderDetails[m].N, ref lPara);
                                    lReport.AddParamaters("O", lOrderDetails[m].O, ref lPara);
                                    lReport.AddParamaters("P", lOrderDetails[m].P, ref lPara);
                                    lReport.AddParamaters("Q", lOrderDetails[m].Q, ref lPara);
                                    lReport.AddParamaters("R", lOrderDetails[m].R, ref lPara);
                                    lReport.AddParamaters("S", lOrderDetails[m].S, ref lPara);
                                    lReport.AddParamaters("T", lOrderDetails[m].T, ref lPara);
                                    lReport.AddParamaters("U", lOrderDetails[m].U, ref lPara);
                                    lReport.AddParamaters("V", lOrderDetails[m].V, ref lPara);
                                    lReport.AddParamaters("W", lOrderDetails[m].W, ref lPara);
                                    lReport.AddParamaters("X", lOrderDetails[m].X, ref lPara);
                                    lReport.AddParamaters("Y", lOrderDetails[m].Y, ref lPara);
                                    lReport.AddParamaters("Z", lOrderDetails[m].Z, ref lPara);
                                }
                                lPara.Sort();

                                var lParaDBL = new System.Collections.Generic.List<string>();
                                lParaDBL.Add("A");
                                lParaDBL.Add("B");
                                lParaDBL.Add("C");
                                lParaDBL.Add("D");
                                for (int m = 0; m < lOrderDetailsDBL.Count; m++)
                                {
                                    lReport.AddParamaters("E", lOrderDetailsDBL[m].E, ref lParaDBL);
                                    lReport.AddParamaters("F", lOrderDetailsDBL[m].F, ref lParaDBL);
                                    lReport.AddParamaters("G", lOrderDetailsDBL[m].G, ref lParaDBL);
                                    lReport.AddParamaters("H", lOrderDetailsDBL[m].H, ref lParaDBL);
                                    lReport.AddParamaters("I", lOrderDetailsDBL[m].I, ref lParaDBL);
                                    lReport.AddParamaters("J", lOrderDetailsDBL[m].J, ref lParaDBL);
                                    lReport.AddParamaters("K", lOrderDetailsDBL[m].K, ref lParaDBL);
                                    lReport.AddParamaters("L", lOrderDetailsDBL[m].L, ref lParaDBL);
                                    lReport.AddParamaters("M", lOrderDetailsDBL[m].M, ref lParaDBL);
                                    lReport.AddParamaters("N", lOrderDetailsDBL[m].N, ref lParaDBL);
                                    lReport.AddParamaters("O", lOrderDetailsDBL[m].O, ref lParaDBL);
                                    lReport.AddParamaters("P", lOrderDetailsDBL[m].P, ref lParaDBL);
                                    lReport.AddParamaters("Q", lOrderDetailsDBL[m].Q, ref lParaDBL);
                                    lReport.AddParamaters("R", lOrderDetailsDBL[m].R, ref lParaDBL);
                                    lReport.AddParamaters("S", lOrderDetailsDBL[m].S, ref lParaDBL);
                                    lReport.AddParamaters("T", lOrderDetailsDBL[m].T, ref lParaDBL);
                                    lReport.AddParamaters("U", lOrderDetailsDBL[m].U, ref lParaDBL);
                                    lReport.AddParamaters("V", lOrderDetailsDBL[m].V, ref lParaDBL);
                                    lReport.AddParamaters("W", lOrderDetailsDBL[m].W, ref lParaDBL);
                                    lReport.AddParamaters("X", lOrderDetailsDBL[m].X, ref lParaDBL);
                                    lReport.AddParamaters("Y", lOrderDetailsDBL[m].Y, ref lParaDBL);
                                    lReport.AddParamaters("Z", lOrderDetailsDBL[m].Z, ref lParaDBL);
                                }
                                lParaDBL.Sort();

                                for (int m = 0; m < lOrderDetails.Count; m++)
                                {
                                    lFound = lReport.checkDiscrepancy(lPara, lOrderDetails[m], lParaDBL, lOrderDetailsDBL[m]);
                                    if (lFound > 0)
                                    {
                                        lErrorMsg = "Found double capture discrepancy. Please correct the error before Submission. \n\n" +
                                            "(发现双重输入的数据差异, 请更正此错误再提交此订单.)";
                                        break;
                                    }
                                }
                            }
                            else if (lOrderDetails.Count > 0 && lOrderDetailsDBL.Count > 0 && lOrderDetails.Count != lOrderDetailsDBL.Count)
                            {
                                lErrorMsg = "Found double capture discrepancy. Please correct the error before Submission. \n\n" +
                                    "(发现双重输入的数据差异, 请更正此错误再提交此订单.)";
                                break;
                            }
                            else if ((lOrderDetails.Count > 0 && lOrderDetailsDBL.Count == 0) || (lOrderDetails.Count == 0 && lOrderDetailsDBL.Count > 0))
                            {
                                lErrorMsg = "Double capture is compulsory for natSteel users. Please complete the double capture validation. \n\n" +
                                    "(对大众钢铁的用户,双重输入验证是强制的, 请完成双重输入验证再提交此订单.)";
                                break;
                            }
                            if (lErrorMsg != "")
                            {
                                break;
                            }
                        }
                    }
                }
            }
            return lErrorMsg;
        }

        int getParaValue(OrderDetailsRetModels pRebar, string pParam, ref int pValue1, ref int pValue2)
        {
            int lReturn = 0;

            if (pParam == "A")
            {
                pValue1 = (int)(pRebar.A == null ? 0 : pRebar.A);
                pValue2 = (int)(pRebar.A2 == null ? 0 : pRebar.A2);
            }
            if (pParam == "B")
            {
                pValue1 = (int)(pRebar.B == null ? 0 : pRebar.B);
                pValue2 = (int)(pRebar.B2 == null ? 0 : pRebar.B2);
            }
            if (pParam == "C")
            {
                pValue1 = (int)(pRebar.C == null ? 0 : pRebar.C);
                pValue2 = (int)(pRebar.C2 == null ? 0 : pRebar.C2);
            }
            if (pParam == "D")
            {
                pValue1 = (int)(pRebar.D == null ? 0 : pRebar.D);
                pValue2 = (int)(pRebar.D2 == null ? 0 : pRebar.D2);
            }
            if (pParam == "E")
            {
                pValue1 = (int)(pRebar.E == null ? 0 : pRebar.E);
                pValue2 = (int)(pRebar.E2 == null ? 0 : pRebar.E2);
            }
            if (pParam == "F")
            {
                pValue1 = (int)(pRebar.F == null ? 0 : pRebar.F);
                pValue2 = (int)(pRebar.F2 == null ? 0 : pRebar.F2);
            }
            if (pParam == "G")
            {
                pValue1 = (int)(pRebar.G == null ? 0 : pRebar.G);
                pValue2 = (int)(pRebar.G2 == null ? 0 : pRebar.G2);
            }
            if (pParam == "H")
            {
                pValue1 = (int)(pRebar.H == null ? 0 : pRebar.H);
                pValue2 = (int)(pRebar.H2 == null ? 0 : pRebar.H2);
            }
            if (pParam == "I")
            {
                pValue1 = (int)(pRebar.I == null ? 0 : pRebar.I);
                pValue2 = (int)(pRebar.I2 == null ? 0 : pRebar.I2);
            }
            if (pParam == "J")
            {
                pValue1 = (int)(pRebar.J == null ? 0 : pRebar.J);
                pValue2 = (int)(pRebar.J2 == null ? 0 : pRebar.J2);
            }
            if (pParam == "K")
            {
                pValue1 = (int)(pRebar.K == null ? 0 : pRebar.K);
                pValue2 = (int)(pRebar.K2 == null ? 0 : pRebar.K2);
            }
            if (pParam == "L")
            {
                pValue1 = (int)(pRebar.L == null ? 0 : pRebar.L);
                pValue2 = (int)(pRebar.L2 == null ? 0 : pRebar.L2);
            }
            if (pParam == "M")
            {
                pValue1 = (int)(pRebar.M == null ? 0 : pRebar.M);
                pValue2 = (int)(pRebar.M2 == null ? 0 : pRebar.M2);
            }
            if (pParam == "N")
            {
                pValue1 = (int)(pRebar.N == null ? 0 : pRebar.N);
                pValue2 = (int)(pRebar.N2 == null ? 0 : pRebar.N2);
            }
            if (pParam == "O")
            {
                pValue1 = (int)(pRebar.O == null ? 0 : pRebar.O);
                pValue2 = (int)(pRebar.O2 == null ? 0 : pRebar.O2);
            }
            if (pParam == "P")
            {
                pValue1 = (int)(pRebar.P == null ? 0 : pRebar.P);
                pValue2 = (int)(pRebar.P2 == null ? 0 : pRebar.P2);
            }
            if (pParam == "Q")
            {
                pValue1 = (int)(pRebar.Q == null ? 0 : pRebar.Q);
                pValue2 = (int)(pRebar.Q2 == null ? 0 : pRebar.Q2);
            }
            if (pParam == "R")
            {
                pValue1 = (int)(pRebar.R == null ? 0 : pRebar.R);
                pValue2 = (int)(pRebar.R2 == null ? 0 : pRebar.R2);
            }
            if (pParam == "S")
            {
                pValue1 = (int)(pRebar.S == null ? 0 : pRebar.S);
                pValue2 = (int)(pRebar.S2 == null ? 0 : pRebar.S2);
            }
            if (pParam == "T")
            {
                pValue1 = (int)(pRebar.T == null ? 0 : pRebar.T);
                pValue2 = (int)(pRebar.T2 == null ? 0 : pRebar.T2);
            }
            if (pParam == "U")
            {
                pValue1 = (int)(pRebar.U == null ? 0 : pRebar.U);
                pValue2 = (int)(pRebar.U2 == null ? 0 : pRebar.U2);
            }
            if (pParam == "V")
            {
                pValue1 = (int)(pRebar.V == null ? 0 : pRebar.V);
                pValue2 = (int)(pRebar.V2 == null ? 0 : pRebar.V2);
            }
            if (pParam == "W")
            {
                pValue1 = (int)(pRebar.W == null ? 0 : pRebar.W);
                pValue2 = (int)(pRebar.W2 == null ? 0 : pRebar.W2);
            }
            if (pParam == "X")
            {
                pValue1 = (int)(pRebar.X == null ? 0 : pRebar.X);
                pValue2 = (int)(pRebar.X2 == null ? 0 : pRebar.X2);
            }
            if (pParam == "Y")
            {
                pValue1 = (int)(pRebar.Y == null ? 0 : pRebar.Y);
                pValue2 = (int)(pRebar.Y2 == null ? 0 : pRebar.Y2);
            }
            if (pParam == "Z")
            {
                pValue1 = (int)(pRebar.Z == null ? 0 : pRebar.Z);
                pValue2 = (int)(pRebar.Z2 == null ? 0 : pRebar.Z2);
            }

            return lReturn;
        }

        int getODParaValue(OrderDetailsModels pRebar, string pParam, ref int pValue1, ref int pValue2)
        {
            int lReturn = 0;

            if (pParam == "A")
            {
                pValue1 = (int)(pRebar.A == null ? 0 : pRebar.A);
                pValue2 = (int)(pRebar.A2 == null ? 0 : pRebar.A2);
            }
            if (pParam == "B")
            {
                pValue1 = (int)(pRebar.B == null ? 0 : pRebar.B);
                pValue2 = (int)(pRebar.B2 == null ? 0 : pRebar.B2);
            }
            if (pParam == "C")
            {
                pValue1 = (int)(pRebar.C == null ? 0 : pRebar.C);
                pValue2 = (int)(pRebar.C2 == null ? 0 : pRebar.C2);
            }
            if (pParam == "D")
            {
                pValue1 = (int)(pRebar.D == null ? 0 : pRebar.D);
                pValue2 = (int)(pRebar.D2 == null ? 0 : pRebar.D2);
            }
            if (pParam == "E")
            {
                pValue1 = (int)(pRebar.E == null ? 0 : pRebar.E);
                pValue2 = (int)(pRebar.E2 == null ? 0 : pRebar.E2);
            }
            if (pParam == "F")
            {
                pValue1 = (int)(pRebar.F == null ? 0 : pRebar.F);
                pValue2 = (int)(pRebar.F2 == null ? 0 : pRebar.F2);
            }
            if (pParam == "G")
            {
                pValue1 = (int)(pRebar.G == null ? 0 : pRebar.G);
                pValue2 = (int)(pRebar.G2 == null ? 0 : pRebar.G2);
            }
            if (pParam == "H")
            {
                pValue1 = (int)(pRebar.H == null ? 0 : pRebar.H);
                pValue2 = (int)(pRebar.H2 == null ? 0 : pRebar.H2);
            }
            if (pParam == "I")
            {
                pValue1 = (int)(pRebar.I == null ? 0 : pRebar.I);
                pValue2 = (int)(pRebar.I2 == null ? 0 : pRebar.I2);
            }
            if (pParam == "J")
            {
                pValue1 = (int)(pRebar.J == null ? 0 : pRebar.J);
                pValue2 = (int)(pRebar.J2 == null ? 0 : pRebar.J2);
            }
            if (pParam == "K")
            {
                pValue1 = (int)(pRebar.K == null ? 0 : pRebar.K);
                pValue2 = (int)(pRebar.K2 == null ? 0 : pRebar.K2);
            }
            if (pParam == "L")
            {
                pValue1 = (int)(pRebar.L == null ? 0 : pRebar.L);
                pValue2 = (int)(pRebar.L2 == null ? 0 : pRebar.L2);
            }
            if (pParam == "M")
            {
                pValue1 = (int)(pRebar.M == null ? 0 : pRebar.M);
                pValue2 = (int)(pRebar.M2 == null ? 0 : pRebar.M2);
            }
            if (pParam == "N")
            {
                pValue1 = (int)(pRebar.N == null ? 0 : pRebar.N);
                pValue2 = (int)(pRebar.N2 == null ? 0 : pRebar.N2);
            }
            if (pParam == "O")
            {
                pValue1 = (int)(pRebar.O == null ? 0 : pRebar.O);
                pValue2 = (int)(pRebar.O2 == null ? 0 : pRebar.O2);
            }
            if (pParam == "P")
            {
                pValue1 = (int)(pRebar.P == null ? 0 : pRebar.P);
                pValue2 = (int)(pRebar.P2 == null ? 0 : pRebar.P2);
            }
            if (pParam == "Q")
            {
                pValue1 = (int)(pRebar.Q == null ? 0 : pRebar.Q);
                pValue2 = (int)(pRebar.Q2 == null ? 0 : pRebar.Q2);
            }
            if (pParam == "R")
            {
                pValue1 = (int)(pRebar.R == null ? 0 : pRebar.R);
                pValue2 = (int)(pRebar.R2 == null ? 0 : pRebar.R2);
            }
            if (pParam == "S")
            {
                pValue1 = (int)(pRebar.S == null ? 0 : pRebar.S);
                pValue2 = (int)(pRebar.S2 == null ? 0 : pRebar.S2);
            }
            if (pParam == "T")
            {
                pValue1 = (int)(pRebar.T == null ? 0 : pRebar.T);
                pValue2 = (int)(pRebar.T2 == null ? 0 : pRebar.T2);
            }
            if (pParam == "U")
            {
                pValue1 = (int)(pRebar.U == null ? 0 : pRebar.U);
                pValue2 = (int)(pRebar.U2 == null ? 0 : pRebar.U2);
            }
            if (pParam == "V")
            {
                pValue1 = (int)(pRebar.V == null ? 0 : pRebar.V);
                pValue2 = (int)(pRebar.V2 == null ? 0 : pRebar.V2);
            }
            if (pParam == "W")
            {
                pValue1 = (int)(pRebar.W == null ? 0 : pRebar.W);
                pValue2 = (int)(pRebar.W2 == null ? 0 : pRebar.W2);
            }
            if (pParam == "X")
            {
                pValue1 = (int)(pRebar.X == null ? 0 : pRebar.X);
                pValue2 = (int)(pRebar.X2 == null ? 0 : pRebar.X2);
            }
            if (pParam == "Y")
            {
                pValue1 = (int)(pRebar.Y == null ? 0 : pRebar.Y);
                pValue2 = (int)(pRebar.Y2 == null ? 0 : pRebar.Y2);
            }
            if (pParam == "Z")
            {
                pValue1 = (int)(pRebar.Z == null ? 0 : pRebar.Z);
                pValue2 = (int)(pRebar.Z2 == null ? 0 : pRebar.Z2);
            }

            return lReturn;
        }

        string isValidValue(string pColumnName, string pParameters, int pDia, int pValue1, int pValue2, OrderDetailsRetModels pItem, PinModels pPinS, PinModels pPinN, List<BBSAddnLimitShapeModels> pAddnLimitShape, List<BBSAddnLimitModels> pAddnLimit, List<BBSAddnParLimitModels> pAddnParLimit)
        {

            var lReturn = true;
            var lErrorMsg = "";
            var lType = pItem.BarType;
            var lPinSize = pItem.PinSize;
            var lParType = pItem.shapeParType;
            var lDefaultValue = pItem.shapeDefaultValue;
            var lHeightCheck = pItem.shapeHeightCheck;

            if (lParType == null)
            {
                lParType = "";
            }
            if (lDefaultValue == null)
            {
                lDefaultValue = "";
            }
            if (lHeightCheck == null)
            {
                lHeightCheck = "";
            }
            if (lParType == "C")
            {
                lParType = "H";
            }
            if (lParType == "E")
            {
                lParType = "T";
            }
            if (lParType == "N")
            {
                lParType = "T";
            }

            if (lType != null && lType != "" &&
                pColumnName != null && pColumnName != "" &&
                pParameters != null && pParameters != "" &&
                pDia != 0 && pValue1 != 0)
            {
                var lParamType = "";
                var lParamTypes = lParType.Split(',');
                var lParas = pParameters.Split(',');
                var lDefaultValueAR = lDefaultValue.Split(',');
                var lHeightCheckAR = lHeightCheck.Split(',');

                var lDiaAR = pPinS.dia;
                var lMinLenAR = pPinS.bend_len_min;
                var lMinLenHkAR = pPinS.hook_len_min;
                var lMinHtHkAR = pPinS.hook_height_min;
                var lNonDiaAR = pPinN.dia;
                var lNonMinLenAR = pPinN.bend_len_min;
                var lNonMinLenHkAR = pPinN.hook_len_min;
                var lNonMinHtHkAR = pPinN.hook_height_min;
                var lStdFormerAR = pPinS.pin;
                var lNonFormerAR = pPinN.pin;

                if (lParamTypes.Length >= lParas.Length)
                {
                    for (var i = 0; i < lParas.Length; i++)
                    {
                        if (pColumnName == lParas[i])
                        {
                            lParamType = lParamTypes[i];

                            // Check whether the max value > 1800 (EVG cannot bend) - 2023-03-08 - zbc
                            var lMaxSegLen = 0;
                            for (var m = 0; m < lParas.Length; m++)
                            {
                                if (lParamTypes[m] == "S")
                                {
                                    int lHValue1 = 0;
                                    int lHValue2 = 0;
                                    getParaValue(pItem, lParas[m], ref lHValue1, ref lHValue2);
                                    if (lHValue1 > lMaxSegLen)
                                    {
                                        lMaxSegLen = lHValue1;
                                    }
                                    if (lHValue2 > lMaxSegLen)
                                    {
                                        lMaxSegLen = lHValue2;
                                    }
                                }
                            }

                            if (lParamType == "S" && pParameters != "A" && (pDia > 16 || lMaxSegLen > 1800) && lReturn != false)
                            {
                                //inner segments bending check for double bender
                                // lDefValue == 1 first seg; 2 inner seg; 3 last seg
                                var lDefValue = 0;
                                int.TryParse(lDefaultValueAR[i], out lDefValue);
                                if (lDefValue == 2)
                                {
                                    var lStdFormer = (int)(lStdFormerAR == null ? 0 : lStdFormerAR);
                                    var lNonFormer = (int)(lNonFormerAR == null ? 0 : lNonFormerAR);
                                    //var lStdMinLen = (parseInt(lStdFormer) + parseInt(pDia)) * 2 + 20;
                                    //var lNonMinLen = (parseInt(lNonFormer) + parseInt(pDia)) * 2 + 20;
                                    var lStdMinLen = lStdFormer * 2 + 20;
                                    var lNonMinLen = lNonFormer * 2 + 20;
                                    var lInputValue = getVarMinValue(pValue1, pValue2);
                                    if (lInputValue < lNonMinLen)
                                    {
                                        lErrorMsg = "Invalid data entered. The minimum bending length of internal segments is "
                                            + lNonMinLen + "mm. "
                                            + "\n(输入数据无效, 此图型参数值已小于它的最小长度 " + lNonMinLen + "mm. ";
                                        break;

                                    }
                                    else if (lInputValue >= lNonMinLen && lInputValue < lStdMinLen && pItem.PinSize != lNonFormer)
                                    {
                                        lErrorMsg = "Invalid data entered. The minimum bending length is "
                                            + lStdMinLen + "mm for standard "
                                            + lStdFormer + " former. "
                                            + "\n(输入数据无效, 标准曲模"
                                            + lStdFormer + "的最小弯曲长度为 " + lStdMinLen + "mm. ";
                                        break;

                                    }
                                }
                            }

                            if (pParameters != "A" && pDia <= 16 && lReturn != false)
                            {
                                lReturn = true;
                                if (pItem.BarShapeCode == null)
                                {
                                    pItem.BarShapeCode = "";
                                }
                                var lShapeCode = pItem.BarShapeCode.Trim();
                                if (lShapeCode.Length < 3) lShapeCode = "0" + lShapeCode;
                                for (var j = 0; j < pAddnLimitShape.Count; j++)
                                {
                                    if (pAddnLimitShape[j].shape_code == lShapeCode && pAddnLimitShape[j].shape_paras == pColumnName)
                                    {
                                        var lLenLimit = 0;
                                        for (var k = 0; k < pAddnLimit.Count; k++)
                                        {
                                            if (pAddnLimit[k].dia == pDia)
                                            {
                                                if (pAddnLimitShape[j].hook_shape == true)
                                                {
                                                    lLenLimit = pAddnLimit[k].hook_height_max;
                                                }
                                                else
                                                {
                                                    lLenLimit = pAddnLimit[k].bend_len_min;
                                                }
                                                break;
                                            }
                                        }

                                        if (pAddnLimitShape[j].hook_shape == true)
                                        {
                                            var lInputValue = getVarMinValue(pValue1, pValue2);
                                            if (lInputValue > lLenLimit)
                                            {
                                                lErrorMsg = "Invalid data entered for shape " + lShapeCode + ", parameter " + pColumnName + ". Its value is greater than the maximum hook height "
                                                    + lLenLimit + "mm. You may want to replace the shape by " + pAddnLimitShape[j].replace_shape + "."
                                                    + "\n(输入图型" + lShapeCode + ", 参数" + pColumnName + "的数值无效. 此参数值已大于它的最大高度 " + lLenLimit + "mm. 您可以用" + pAddnLimitShape[j].replace_shape.Replace("or", "或") + "来替换此图形) ";
                                                lReturn = false;
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            var lInputValue = getVarMinValue(pValue1, pValue2);
                                            if (lInputValue < lLenLimit)
                                            {
                                                lErrorMsg = "Invalid data entered for shape " + lShapeCode + ", parameter " + pColumnName + ". Its value is less than the minimum bending length of internal segment "
                                                    + lLenLimit + "mm. You may want to replace the shape by " + pAddnLimitShape[j].replace_shape + "."
                                                    + "\n(输入图型" + lShapeCode + ", 参数" + pColumnName + "的数值无效. 此图型参数值已小于它的最小长度 " + lLenLimit + "mm. 您可以用" + pAddnLimitShape[j].replace_shape.Replace("or", "或") + "来替换此图形) ";
                                                lReturn = false;
                                                break;
                                            }
                                        }
                                    }
                                }
                                if (lReturn == false)
                                {
                                    break;
                                }
                            }


                            //Validate hook length
                            var lLastInd = 0;
                            if (lParamType == "S" && i > 0 && lParamTypes[i - 1] == "R")
                            {
                                lLastInd = 1;
                                if (i + 1 < lParas.Length)
                                {
                                    for (int j = i + 1; j < lParas.Length; j++)
                                    {
                                        if (lParamTypes[j] == "S")
                                        {
                                            lLastInd = 0;
                                            break;
                                        }
                                    }
                                }
                            }

                            if ((lParamTypes.Contains("RL") == false) && ((lParamType == "S" && i == 0 && lParas.Length > 1 && lParamTypes[i + 1] == "R")
                                || (lParamType == "S" && i > 0 && lParamTypes[i - 1] == "R" && lLastInd == 1)))
                            {
                                //Check hook length >= hook height - 2022-06-07
                                int lHValue1 = 0;
                                int lHValue2 = 0;

                                if (i == 0 && lParas.Length > 1 && lParamTypes[i + 1] == "R")
                                {
                                    getParaValue(pItem, lParas[i + 1], ref lHValue1, ref lHValue2);
                                }
                                if (i > 0 && lParamTypes[i - 1] == "R" && lLastInd == 1)
                                {
                                    getParaValue(pItem, lParas[i - 1], ref lHValue1, ref lHValue2);
                                }
                                if ((lHValue1 > 0 && pValue1 > 0 && pValue1 < lHValue1) ||
                                    (lHValue2 > 0 && pValue2 > 0 && pValue2 < lHValue2) ||
                                    (lHValue2 == 0 && pValue2 > 0 && pValue2 < lHValue1) ||
                                    (lHValue2 > 0 && pValue2 == 0 && pValue1 < lHValue2))
                                {
                                    lErrorMsg = "Invalid data entered. The hook length ("
                                        + pValue1.ToString() + (pValue2 == 0 ? "" : ("-" + pValue2.ToString()))
                                        + ") must be more than or equal to hook height ("
                                        + lHValue1.ToString() + (lHValue2 == 0 ? "" : ("-" + lHValue2.ToString()))
                                        + "). \n输入数据无效, 挂钩钩长 ("
                                        + pValue1.ToString() + (pValue2 == 0 ? "" : ("-" + pValue2.ToString()))
                                        + ") 必须大于或等于挂钩钩高 ("
                                        + lHValue1.ToString() + (lHValue2 == 0 ? "" : ("-" + lHValue2.ToString()))
                                        + ").";
                                    lReturn = false;
                                    break;
                                }

                                //Min Len validation
                                var lStdMinLen = lMinLenHkAR;
                                if (lStdMinLen > 0)
                                {
                                    if (getVarMinValue(pValue1, pValue2) < lStdMinLen)
                                    {
                                        var lNonMinLen = lNonMinLenHkAR;
                                        var lStdFormer = lStdFormerAR;
                                        var lNonFormer = lNonFormerAR;
                                        if (getVarMinValue(pValue1, pValue2) < lNonMinLen)
                                        {
                                            if (lStdFormer == lNonFormer)
                                            {
                                                lErrorMsg = "Invalid data entered. The minimum hook length is "
                                                    + lStdMinLen + "mm for "
                                                    + lStdFormer + "mm former. \n(输入数据无效, 曲模"
                                                    + lStdFormer + "mm的最小弯曲长度为 " + lStdMinLen + "mm.)";
                                            }
                                            else
                                            {
                                                lErrorMsg = "Invalid data entered. The minimum hook length is "
                                                    + lStdMinLen + "mm for standard "
                                                    + lStdFormer + "mm former, " + lNonMinLen + "mm for non-standard "
                                                    + lNonFormer + "mm former. \n(输入数据无效, 标准曲模"
                                                    + lStdFormer + "mm的最小弯曲长度为 " + lStdMinLen + "mm. 非标准曲模"
                                                    + lNonFormer + "mm的最小弯曲长度为 " + lNonMinLen + "mm.)";
                                            }
                                            lReturn = false;
                                        }
                                        else
                                        {
                                            if (lPinSize != lNonFormer)
                                            {
                                                lErrorMsg = "Invalid data entered. The minimum bending length is "
                                                    + lStdMinLen + "mm for standard "
                                                    + lStdFormer + "mm former, " + lNonMinLen + "mm for non-standard "
                                                    + lNonFormer + "mm former. \n(输入数据无效, 标准曲模"
                                                    + lStdFormer + "mm的最小弯曲长度为 " + lStdMinLen + "mm. 非标准曲模"
                                                    + lNonFormer + "mm的最小弯曲长度为 " + lNonMinLen + "mm.)";
                                            }
                                        }
                                    }
                                    //break;
                                }
                            }

                            if ((lParamType == "S" || lParamType == "HK") && pParameters != "A")
                            {
                                //Check various parameters range
                                if (pValue2 > 0)
                                {
                                    var lMax = getVarMaxValue(pValue1, pValue2);
                                    var lMin = getVarMinValue(pValue1, pValue2);

                                    if (lMax <= 0 || lMin <= 0 || lMin == lMax)
                                    {
                                        lErrorMsg = "Invalid data entered. Please enter valid numeric range.(输入数据无效, 请输入数字范围)";
                                        lReturn = false;
                                    }
                                    var lMbrQty = pItem.BarMemberQty;
                                    if (lMbrQty > 1)
                                    {
                                        //var lHeight = (int)Math.Round((double)((double)(lMax - lMin) / ((int)lMbrQty - 1)));
                                        //if (lHeight < 18)
                                        //{
                                        //    lErrorMsg = "Invalid data entered. Difference of various bars is less than 18mm minimum limit. Please increase the difference of verious bar or reduce Member Qty.(输入数据无效, 可变钢筋的差值已小于18mm的最小值. 请增加差值或减少构件数)";
                                        //    lReturn = false;
                                        //}
                                    }
                                    else
                                    {
                                        lErrorMsg = "Invalid data entered on Member Qty field. It should be greater than 1 for varied length bars.(构建数输入数据无效. 对变长钢筋而言,它应该大于1)";
                                        lReturn = false;
                                    }
                                }
                                //Min Len validation
                                var lStdMinLen = lMinLenAR;
                                if (lStdMinLen > 0)
                                {
                                    if (getVarMinValue(pValue1, pValue2) < lStdMinLen)
                                    {
                                        var lNonMinLen = lNonMinLenAR;
                                        var lStdFormer = lStdFormerAR;
                                        var lNonFormer = lNonFormerAR;
                                        if (getVarMinValue(pValue1, pValue2) < lNonMinLen)
                                        {
                                            if (lStdFormer == lNonFormer)
                                            {
                                                lErrorMsg = "Invalid data entered. The minimum bending length is "
                                                    + lStdMinLen + "mm for "
                                                    + lStdFormer + "mm former. \n(输入数据无效, 曲模"
                                                    + lStdFormer + "mm的最小弯曲长度为 " + lStdMinLen + "mm.)";
                                            }
                                            else
                                            {
                                                lErrorMsg = "Invalid data entered. The minimum bending length is "
                                                    + lStdMinLen + "mm for standard "
                                                    + lStdFormer + "mm former, " + lNonMinLen + "mm for non-standard "
                                                    + lNonFormer + "mm former. \n(输入数据无效, 标准曲模"
                                                    + lStdFormer + "mm的最小弯曲长度为 " + lStdMinLen + "mm. 非标准曲模"
                                                    + lNonFormer + "mm的最小弯曲长度为 " + lNonMinLen + "mm.)";
                                            }
                                            lReturn = false;
                                        }
                                        else
                                        {
                                            if (lPinSize > lNonFormer)
                                            {
                                                lErrorMsg = "Invalid data entered. The minimum bending length is "
                                                    + lStdMinLen + "mm for standard "
                                                    + lStdFormer + "mm former, " + lNonMinLen + "mm for non-standard "
                                                    + lNonFormer + "mm former. \n(输入数据无效, 标准曲模"
                                                    + lStdFormer + "mm的最小弯曲长度为 " + lStdMinLen + "mm. 非标准曲模"
                                                    + lNonFormer + "mm的最小弯曲长度为 " + lNonMinLen + "mm.)";
                                                lReturn = false;

                                            }
                                        }
                                    }

                                    // check 061/079/79A leg
                                    if (pItem.BarShapeCode != null)
                                    {
                                        var lShapeCode = pItem.BarShapeCode.Trim();
                                        if (lShapeCode.Length < 3) lShapeCode = "0" + lShapeCode;

                                        if (lShapeCode == "079" && (pColumnName == "C" || pColumnName == "D") && pItem.A != null)
                                        {
                                            if (getVarMaxValue(pValue1, pValue2) > pItem.A)
                                            {
                                                lErrorMsg = "Invalid data entered. For shape code 079, its leg length cannot be more then parameter A . (输入数据无效, 对图形079而言, 脚长不可超过其A边的长度)";
                                                lReturn = false;
                                                break;
                                            }
                                        }
                                        if (lShapeCode == "079" && pColumnName == "A" && pItem.C != null && pItem.D != null)
                                        {
                                            if (getVarMinValue(pValue1, pValue2) < pItem.C || pValue1 < pItem.D)
                                            {
                                                lErrorMsg = "Invalid data entered. For shape code 079, parameter A cannot be less then its leg length C and D. (输入数据无效, 对图形079而言, A边的长度不可小过其脚长C或D)";
                                                lReturn = false;
                                                break;
                                            }
                                        }

                                        if (lShapeCode == "061" && pColumnName == "C")
                                        {
                                            if (getVarMaxValue(pValue1, pValue2) > pItem.B)
                                            {
                                                lErrorMsg = "Invalid data entered. For shape code 061, its leg length C cannot be more then parameter B . (输入数据无效, 对图形061而言, 脚长C不可超过其B边的长度)";
                                                lReturn = false;
                                                break;
                                            }
                                        }
                                        if (lShapeCode == "061" && pColumnName == "D")
                                        {
                                            if (getVarMaxValue(pValue1, pValue2) > pItem.A)
                                            {
                                                lErrorMsg = "Invalid data entered. For shape code 061, its leg length D cannot be more then parameter A . (输入数据无效, 对图形061而言, 脚长D不可超过其A边的长度)";
                                                lReturn = false;
                                                break;
                                            }
                                        }
                                        if (lShapeCode == "061" && pColumnName == "A" && pItem.D != null)
                                        {
                                            if (getVarMinValue(pValue1, pValue2) < pItem.D)
                                            {
                                                lErrorMsg = "Invalid data entered. For shape code 061, parameter A cannot be less than its leg length D. (输入数据无效, 对图形061而言, 边长A不可小过其脚长D)";
                                                lReturn = false;
                                                break;
                                            }
                                        }
                                        if (lShapeCode == "061" && pColumnName == "B" && pItem.C != null)
                                        {
                                            if (getVarMinValue(pValue1, pValue2) < pItem.C)
                                            {
                                                lErrorMsg = "Invalid data entered. For shape code 061, parameter B cannot be less than its leg length C. (输入数据无效, 对图形061而言, 边长B不可小过其脚长C)";
                                                lReturn = false;
                                                break;
                                            }
                                        }
                                        if (lShapeCode == "79A" && pColumnName == "C" && pItem.A != null)
                                        {
                                            if (getVarMaxValue(pValue1, pValue2) > pItem.A)
                                            {
                                                lErrorMsg = "Invalid data entered. For shape code 79A, its leg length C cannot be more than parameter A . (输入数据无效, 对图形79A而言, 脚长C不可超过其A边的长度)";
                                                lReturn = false;
                                                break;
                                            }
                                        }
                                        if (lShapeCode == "79A" && pColumnName == "A" && pItem.C != null)
                                        {
                                            if (getVarMinValue(pValue1, pValue2) < pItem.C)
                                            {
                                                lErrorMsg = "Invalid data entered. For shape code 79A, its parameter A cannot be less than its leg C length. (输入数据无效, 对图形79A而言, A边的长度不可小过其脚长C)";
                                                lReturn = false;
                                                break;
                                            }
                                        }
                                    }
                                    //break;
                                }
                            }

                            //check 8mm maximum segment length
                            if ((lParamType == "S" || lParamType == "HK") && pParameters != "A" && pDia == 8)
                            {
                                if (getVarMaxValue(pValue1, pValue2) > 1800)
                                {
                                    lErrorMsg = "Invalid data entered. The maximum segment length is 1800mm for 8mm bar. (输入数据无效. 对8mm钢筋, 最大节长为1800mm)";
                                    lReturn = false;
                                    break;
                                }
                            }

                            if (pItem.BarShapeCode != null && pItem.BarShapeCode == "R7A" && pColumnName == "C")
                            {
                                if (getVarMaxValue(pValue1, pValue2) > 500)
                                {
                                    lErrorMsg = "Invalid data entered. For shape code R7A, its parameter  C(Height) cannot be more than 500mm. (输入数据无效, 对图形R7A, 参数C的高度不可大于500mm.)";
                                    lReturn = false;
                                    break;
                                }
                            }
                            if (pItem.BarShapeCode != null && pItem.BarShapeCode == "R7A" && pColumnName == "A")
                            {
                                if (getVarMaxValue(pValue1, pValue2) > 900)
                                {
                                    lErrorMsg = "Invalid data entered. For shape code R7A, its parameter A (Circular Diameter) cannot be more than 900mm. (输入数据无效, 对图形R7A, 参数A不可超过900mm)";
                                    lReturn = false;
                                    break;
                                }
                                if (getVarMinValue(pValue1, pValue2) < 200)
                                {
                                    lErrorMsg = "Invalid data entered. For shape code R7A, its parameter A (Circular Diameter) cannot be less than 200mm. (输入数据无效, 对图形R7A, 参数A不可小于200mm)";
                                    lReturn = false;
                                    break;
                                }
                            }

                            if (pItem.BarShapeCode != null && (pItem.BarShapeCode == "039" || pItem.BarShapeCode == "39") && pColumnName == "A")
                            {
                                if (pDia <= 20 && getVarMaxValue(pValue1, pValue2) > 3500 && getVarMaxValue(pItem.C.ToString()) > 3500)
                                {
                                    lErrorMsg = "Invalid data entered. For shape 39 with diameter <= 20mm, it does not allow both value of parameter A and C greater than 3500mm. (输入数据无效, 对直径 <= 20mm 的图形 39, 不允许参数 A 和 C 的值都大于 3500mm)";
                                    lReturn = false;
                                    break;
                                }
                                if (pDia > 20 && getVarMaxValue(pValue1, pValue2) > 2500 && pItem.C > 2500)
                                {
                                    lErrorMsg = "Invalid data entered. For shape 39 with diameter >= 25mm, it does not allow both value of parameter A and C greater than 2500mm. (输入数据无效, 对直径 >= 25mm 的图形 39,不允许参数 A 和 C 的值都大于 2500mm)";
                                    lReturn = false;
                                    break;
                                }
                            }
                            if (pItem.BarShapeCode != null && (pItem.BarShapeCode == "039" || pItem.BarShapeCode == "39") && pColumnName == "C")
                            {
                                if (pDia <= 20 && getVarMaxValue(pValue1, pValue2) > 3500 && getVarMaxValue(pItem.A.ToString()) > 3500)
                                {
                                    lErrorMsg = "Invalid data entered. For shape 39 with diameter <= 20mm, it does not allow both value of parameter A and C greater than 3500mm. (输入数据无效, 对直径 <= 20mm 的图形 39, 不允许参数 A 和 C 的值都大于 3500mm)";
                                    lReturn = false;
                                    break;
                                }
                                if (pDia > 20 && getVarMaxValue(pValue1, pValue2) > 2500 && pItem.A > 2500)
                                {
                                    lErrorMsg = "Invalid data entered. For shape 39 with diameter >= 25mm, it does not allow both value of parameter A and C greater than 2500mm. (输入数据无效, 对直径 >= 25mm 的图形 39,不允许参数 A 和 C 的值都大于 2500mm)";
                                    lReturn = false;
                                    break;
                                }
                            }

                            if (pItem.BarShapeCode != null && pItem.BarShapeCode == "314" && pColumnName == "B")
                            {
                                if (pDia >= 40 && getVarMinValue(pValue1, pValue2) < 400)
                                {
                                    lErrorMsg = "Invalid data entered. For shape 314 with diameter >= 40mm, minimum value of parameter B is 400mm. (输入数据无效, 对直径40mm, 50mm的图形 314, 参数B的最小值为 400mm)";
                                    lReturn = false;
                                    break;
                                }
                            }

                            if (lParamType == "R")
                            {
                                var lShapeCode = "";
                                if (pItem.BarShapeCode != null)
                                {
                                    lShapeCode = pItem.BarShapeCode.Trim();
                                }
                                if (lShapeCode.Length < 3) lShapeCode = "0" + lShapeCode;

                                if (lShapeCode == "R84")
                                {
                                    //Min Len validation
                                    var lStdMinLen = lMinLenAR;
                                    if (lStdMinLen > 0)
                                    {
                                        if (getVarMinValue(pValue1, pValue2) < lStdMinLen)
                                        {
                                            var lNonMinLen = lNonMinLenAR;
                                            var lStdFormer = lStdFormerAR;
                                            var lNonFormer = lNonFormerAR;
                                            if (getVarMinValue(pValue1, pValue2) < lNonMinLen)
                                            {
                                                if (lStdFormer == lNonFormer)
                                                {
                                                    lErrorMsg = "Invalid data entered. The minimum bending length is "
                                                        + lStdMinLen + "mm for "
                                                        + lStdFormer + "mm former. \n(输入数据无效, 曲模"
                                                        + lStdFormer + "mm的最小弯曲长度为 " + lStdMinLen + "mm.)";
                                                }
                                                else
                                                {
                                                    lErrorMsg = "Invalid data entered. The minimum bending length is "
                                                        + lStdMinLen + "mm for standard "
                                                        + lStdFormer + "mm former, " + lNonMinLen + "mm for non-standard "
                                                        + lNonFormer + "mm former. \n(输入数据无效, 标准曲模"
                                                        + lStdFormer + "mm的最小弯曲长度为 " + lStdMinLen + "mm. 非标准曲模"
                                                        + lNonFormer + "mm的最小弯曲长度为 " + lNonMinLen + "mm.)";
                                                }
                                                lReturn = false;
                                            }
                                            else
                                            {
                                                if (lPinSize > lNonFormer)
                                                {
                                                    lErrorMsg = "Invalid data entered. The minimum bending length is "
                                                        + lStdMinLen + "mm for standard "
                                                        + lStdFormer + "mm former, " + lNonMinLen + "mm for non-standard "
                                                        + lNonFormer + "mm former. \n(输入数据无效, 标准曲模"
                                                        + lStdFormer + "mm的最小弯曲长度为 " + lStdMinLen + "mm. 非标准曲模"
                                                        + lNonFormer + "mm的最小弯曲长度为 " + lNonMinLen + "mm.)";
                                                    lReturn = false;

                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    //Hook limimum height validation
                                    var lStdMinLen = lMinHtHkAR;
                                    if (lStdMinLen > 0)
                                    {
                                        if (getVarMinValue(pValue1, pValue2) < lStdMinLen)
                                        {
                                            var lNonMinLen = lNonMinHtHkAR;
                                            var lStdFormer = lStdFormerAR;
                                            var lNonFormer = lNonFormerAR;
                                            if (getVarMinValue(pValue1, pValue2) < lNonMinLen)
                                            {
                                                if (lStdFormer == lNonFormer)
                                                {
                                                    lErrorMsg = "Invalid data entered. The minimum hook height is "
                                                        + lStdMinLen + "mm for "
                                                        + lStdFormer + "mm former. \n(输入数据无效, 曲模"
                                                        + lStdFormer + "mm的最小铁钩高度为 " + lStdMinLen + "mm.)";
                                                }
                                                else
                                                {
                                                    lErrorMsg = "Invalid data entered. The minimum hook height is "
                                                        + lStdMinLen + "mm for standard "
                                                        + lStdFormer + "mm former, " + lNonMinLen + "mm for non-standard "
                                                        + lNonFormer + "mm former. \n(输入数据无效, 标准曲模"
                                                        + lStdFormer + "mm的最小铁钩高度为 " + lStdMinLen + "mm. 非标准曲模"
                                                        + lNonFormer + "mm的最小铁钩高度为 " + lNonMinLen + "mm.)";
                                                }
                                                lReturn = false;
                                            }
                                            else
                                            {
                                                if (lPinSize > lNonFormer)
                                                {
                                                    lErrorMsg = "Invalid data entered. The minimum hook height is "
                                                        + lStdMinLen + "mm for standard "
                                                        + lStdFormer + "mm former, " + lNonMinLen + "mm for non-standard "
                                                        + lNonFormer + "mm former. (输入数据无效, 标准曲模"
                                                        + lStdFormer + "mm的最小铁钩高度为 " + lStdMinLen + "mm. 非标准曲模"
                                                        + lNonFormer + "mm的最小铁钩高度为 " + lNonMinLen + "mm.)";
                                                    lReturn = false;
                                                }
                                            }
                                        }
                                        break;
                                    }
                                }
                            }

                            if (lParamType == "H" || lParamType == "O")
                            {
                                //Height validation
                                if (getVarMinValue(pValue1, pValue2) < pDia + pDia - (int)Math.Round((double)pDia / 2))
                                {
                                    // less than dia + 10 (first dia)
                                    lErrorMsg = "Invalid data entered. The height is measured with out-out. If it join with " + pDia + "mm bar, the minimum height is " + (pDia + pDia - (int)Math.Round((double)pDia / 2)) + ". (输入数据无效, 高度是从铁的外边度量. 如果它与" + pDia + "mm铁连接, 其最小值为" + (pDia + pDia - (int)Math.Round((double)pDia / 2)) + ")";
                                    lReturn = false;
                                    break;
                                }

                                int lHValue1 = 0;
                                int lHValue2 = 0;

                                getParaValue(pItem, lHeightCheckAR[i], ref lHValue1, ref lHValue2);

                                //var lHypo = pItem[lHeightCheckAR[i]];
                                var lHypoMin = getVarMinValue(lHValue1, lHValue2);
                                var lHypoMax = getVarMaxValue(lHValue1, lHValue2);

                                var lShapeCode = "";
                                if (pItem.BarShapeCode != null)
                                {
                                    lShapeCode = pItem.BarShapeCode.Trim();
                                }

                                if (lHypoMax > 0 && lShapeCode.Length > 0 && lShapeCode.Substring(0, 1) != "R")
                                {
                                    //Sathiya found the height will greater than hypo when angle greater than 90.
                                    //for save, it plus Dia and half of Pin
                                    //2018-09-10

                                    if (getVarMaxValue(pValue1, pValue2) > lHypoMax + pDia + (int)Math.Round((double)(lPinSize * lPinSize / lHypoMax)))
                                    {
                                        // Greater than 90
                                        lErrorMsg = "Invalid data entered. Height can not be more than its adjusted hypotenuse length (" + (lHypoMax + pDia + (int)Math.Round((double)(lPinSize * lPinSize / lHypoMax))).ToString() + "). (输入数据无效, 高度不应该大于其斜边调整后的长度 (" + (lHypoMax + pDia + (int)Math.Round((double)(lPinSize * lPinSize / lHypoMax))).ToString() + ").";
                                        lReturn = false;
                                        break;
                                    }
                                }
                                if (lHypoMin > 0 && lShapeCode.Length > 0 && lShapeCode.Substring(0, 1) != "R")
                                {
                                    if (getVarMinValue(pValue1, pValue2) > lHypoMin + pDia + (int)Math.Round((double)(lPinSize * lPinSize / lHypoMin)))
                                    {
                                        // Greater than 90
                                        lErrorMsg = "Invalid data entered. Height can not be more than its adjusted hypotenuse length (" + (lHypoMin + pDia + (int)Math.Round((double)(lPinSize * lPinSize / lHypoMin))).ToString() + "). (输入数据无效, 高度不应该大于其斜边调整后的长度 (" + (lHypoMin + pDia + (int)Math.Round((double)(lPinSize * lPinSize / lHypoMin))).ToString() + ").";
                                        lReturn = false;
                                        break;
                                    }
                                }
                            }
                            if (lParamType == "V")
                            {
                                var lDefValue = 0;
                                int.TryParse(lDefaultValueAR[i], out lDefValue);
                                if (lDefValue >= 80 && lDefValue < 90)
                                {
                                    //Angle validation 1-179
                                    if (getVarMaxValue(pValue1, pValue2) >= 180 || getVarMaxValue(pValue1, pValue2) <= 0)
                                    {
                                        lErrorMsg = "Invalid data entered. The angle value should be between 1-179. (输入数据无效, 角度的数值应该在 1-179 之间)";
                                        lReturn = false;
                                        break;
                                    }
                                }
                                if (lDefValue < 80)
                                {
                                    //Angle validation 1-89
                                    if (getVarMaxValue(pValue1, pValue2) >= 90 || getVarMaxValue(pValue1, pValue2) <= 0)
                                    {
                                        lErrorMsg = "Invalid data entered. The angle value should be between 1-89. (输入数据无效, 角度的数值应该在 1-89 之间)";
                                        lReturn = false;
                                        break;
                                    }
                                }
                                if (lDefValue > 90 && lDefValue < 100)
                                {
                                    //Angle validation 91-179
                                    if (getVarMaxValue(pValue1, pValue2) >= 180 || getVarMaxValue(pValue1, pValue2) <= 0)
                                    {
                                        lErrorMsg = "Invalid data entered. The angle value should be between 1-179. (输入数据无效, 角度的数值应该在 1-179 之间)";
                                        lReturn = false;
                                        break;
                                    }
                                }
                                if (lDefValue >= 100)
                                {
                                    //Angle validation 91-179
                                    if (getVarMaxValue(pValue1, pValue2) >= 180 || getVarMinValue(pValue1, pValue2) <= 90)
                                    {
                                        lErrorMsg = "Invalid data entered. The angle value should be between 91-179. (输入数据无效, 角度的数值应该在 91-179 之间)";
                                        lReturn = false;
                                        break;
                                    }
                                }
                            }

                            //Arc Reduis Validation
                            if (lParamType == "RL")
                            {
                                var lStdFormer = (int)(lStdFormerAR == null ? 0 : lStdFormerAR);
                                if (getVarMinValue(pValue1, pValue2) <= Math.Ceiling((double)lStdFormer / 2))
                                {
                                    // Arc Reduis should greater than standard form 
                                    lErrorMsg = "Invalid data entered. The arc raduis should be greater than the standard pin raduis (" + Math.Ceiling((double)lStdFormer / 2) + "). You also can choose another similar shape with standard bending. (输入数据无效, 圆弧半径应大于标准曲模半径 " + Math.Ceiling((double)lStdFormer / 2) + ". 您也可以选择其他类似的标准曲模图形.)";
                                    lReturn = false;
                                    break;
                                }
                            }

                            if (pItem.BarShapeCode == "20" || pItem.BarShapeCode == "020")
                            {
                                if (pDia == 8)
                                {
                                    if (getVarMaxValue(pValue1, pValue2) > 1800 && getVarMaxValue(pValue1, pValue2) != 6000)
                                    {
                                        lErrorMsg = "Invalid data entered. The maximum length for straight bar is "
                                            + "1800mm for diameter "
                                            + pDia + "mm. \n(输入数据无效, "
                                            + pDia + "mm铁的最大长度为 1800mm.)";
                                        lReturn = false;
                                    }
                                }
                                if (pDia < 13)
                                {
                                    if (getVarMinValue(pValue1, pValue2) < 25)
                                    {
                                        lErrorMsg = "Invalid data entered. The minimum length for straight bar is "
                                            + "25mm for diameter "
                                            + pDia + "mm. \n(输入数据无效, "
                                            + pDia + "mm铁的最小长度为 25mm.)";
                                        lReturn = false;
                                    }
                                }
                                else if (pDia < 20)
                                {
                                    if (getVarMinValue(pValue1, pValue2) < 50)
                                    {
                                        lErrorMsg = "Invalid data entered. The minimum length for straight bar is "
                                            + "50mm for diameter "
                                            + pDia + "mm. \n(输入数据无效, "
                                            + pDia + "mm铁的最小长度为 50mm.)";
                                        lReturn = false;
                                    }
                                }
                                else if (pDia <= 25)
                                {
                                    if (getVarMinValue(pValue1, pValue2) < 180)
                                    {
                                        lErrorMsg = "Invalid data entered. The minimum length for straight bar is "
                                            + "180mm for diameter "
                                            + pDia + "mm. \n(输入数据无效, "
                                            + pDia + "mm铁的最小长度为 180mm.)";
                                        lReturn = false;
                                    }
                                }
                                else if (pDia < 40)
                                {
                                    if (getVarMinValue(pValue1, pValue2) < 180)
                                    {
                                        lErrorMsg = "Invalid data entered. The minimum length for straight bar is "
                                            + "180mm for diameter "
                                            + pDia + "mm. \n(输入数据无效, "
                                            + pDia + "mm铁的最小长度为 180mm.)";
                                        lReturn = false;
                                    }
                                }
                                else if (pDia < 50)
                                {
                                    if (getVarMinValue(pValue1, pValue2) < 500)
                                    {
                                        lErrorMsg = "Invalid data entered. The minimum length for straight bar is "
                                            + "500mm for diameter "
                                            + pDia + "mm. \n(输入数据无效, "
                                            + pDia + "mm铁的最小长度为 500mm.)";
                                        lReturn = false;
                                    }
                                }
                                else
                                {
                                    if (getVarMinValue(pValue1, pValue2) < 1000)
                                    {
                                        lErrorMsg = "Invalid data entered. The minimum length for straight bar is "
                                            + "1000mm for diameter "
                                            + pDia + "mm. \n(输入数据无效, "
                                            + pDia + "mm铁的最小长度为 1000mm.)";
                                        lReturn = false;
                                    }
                                }
                            }

                            //check addition limit for special parameter type. For example "D" (Diameter) 
                            if (lReturn == true && pAddnParLimit != null && pAddnParLimit.Count > 0)
                            {
                                for (int j = 0; j < pAddnParLimit.Count; j++)
                                {
                                    if (pAddnParLimit[j].par_type == lParamType && pAddnParLimit[j].dia == pDia)
                                    {
                                        if (getVarMinValue(pValue1, pValue2) < pAddnParLimit[j].bend_len_min)
                                        {
                                            lErrorMsg = "Invalid data entered.  For bar diameter " + pDia + ", "
                                                + "the minimum value of parameter "
                                                + pColumnName + " is " + pAddnParLimit[j].bend_len_min
                                                + "mm. \n(输入数据无效, 对直径为"
                                                + pDia + "mm的钢筋, 其参数" + pColumnName + "的最小值为 "
                                                + pAddnParLimit[j].bend_len_min + "mm.)";
                                            lReturn = false;
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return lErrorMsg;
        }

        int getVarMinValue(int pValue1, int pValue2)
        {
            int lReturn = pValue1;
            if (pValue2 > 0)
            {
                if (pValue2 < lReturn)
                {
                    lReturn = pValue2;
                }
            }
            return lReturn;
        }

        int getVarMaxValue(int pValue1, int pValue2)
        {
            int lReturn = pValue1;
            if (pValue2 > 0)
            {
                if (pValue2 > lReturn)
                {
                    lReturn = pValue2;
                }
            }
            return lReturn;
        }

        public int getDeduction(int pInvLength, OrderDetailsRetModels pOrderItem)
        {
            string[] arrLength = { "ACTUAL", "NORMAL", "OFFSET", "OFFSET+NORMAL" };
            string[] arr3Dlength = { "UPDOT", "UPARROW", "UPRIGHT", "UPLEFT", "DNDOT", "DNARROW", "DNRIGHT", "DNLEFT" };
            string[] arrAngle = { "ANGLE" };
            string lSQL = "";
            int lValue = 0;
            int lValuePre = 0;
            int lTotalDed = 0;
            string lProdFormula = "";
            SqlCommand lCmd;
            SqlDataReader adoRst;
            SqlDataAdapter lDa = new SqlDataAdapter();
            DataSet ds = new DataSet();

            SqlConnection lNDSCn = new SqlConnection();

            string lShapeCode = pOrderItem.BarShapeCode == null ? "" : pOrderItem.BarShapeCode.Trim();

            if (lShapeCode.Length == 2)
            {
                lShapeCode = "0" + lShapeCode;
            }

            if (lShapeCode == "020" || lShapeCode == "20" || lShapeCode == "")
            {
                return lTotalDed;
            }

            if (lShapeCode.Substring(0, 1) == "J")
            {
                lTotalDed = lTotalDed + getCoupleDed((int)pOrderItem.BarSize);
            }

            if (lShapeCode.Length == 3 && lShapeCode.Substring(2, 1) == "J")
            {
                lTotalDed = lTotalDed + getCoupleDed((int)pOrderItem.BarSize);
            }

            var lProcess = new ProcessController();

            try
            {
                lProcess.OpenNDSConnection(ref lNDSCn);

                lSQL = "SELECT ISNULL(VCHPRODLENGTHFORMULA, '') " +
                    "FROM dbo.shapemaster_cube " +
                    "WHERE chrShapeCode = '" + lShapeCode + "'";

                lCmd = new SqlCommand(lSQL, lNDSCn);
                adoRst = lCmd.ExecuteReader();
                if (adoRst.HasRows)
                {
                    if (adoRst.Read())
                    {
                        lProdFormula = adoRst.GetValue(0) == DBNull.Value ? "" : adoRst.GetString(0).Trim();
                    }
                }
                adoRst.Close();

                if (lProdFormula != null && lProdFormula.Length > 0)
                {
                    int lDed = 0;

                    var lItem = pOrderItem;
                    var lF = lProdFormula;
                    var lVarMax = 0;

                    lF = lF.Replace("$", pOrderItem.BarSize.ToString());
                    lF = lF.Replace("#", pOrderItem.PinSize.ToString());
                    lF = lF.Replace("@", pOrderItem.PinSize.ToString());

                    while (lF.IndexOf("^") > 0)
                    {
                        lF = lF.Replace(" ", "");
                        int lPos = lF.IndexOf("^");
                        if (lF.Substring(lPos - 1, 1) == ")")
                        {
                            int lStart = getPreviousBracket(lF, lPos - 1);
                            int lEnd = getNextPos(lF, lPos + 1);
                            if (lStart == 0)
                            {
                                if (lEnd > 0)
                                {
                                    if (lEnd >= lF.Length - 1)
                                    {
                                        lF = "pow(" + lF.Substring(0, lPos) + "," + lF.Substring(lPos + 1, lEnd - lPos) + ")";
                                    }
                                    else
                                    {
                                        lF = "pow(" + lF.Substring(0, lPos) + "," + lF.Substring(lPos + 1, lEnd - lPos) + ")" + lF.Substring(lEnd + 1);
                                    }
                                }
                            }
                            else
                            {
                                if (lEnd > 0)
                                {
                                    if (lEnd >= lF.Length - 1)
                                    {
                                        lF = lF.Substring(0, lStart) + "pow(" + lF.Substring(lStart, lPos - lStart) + "," + lF.Substring(lPos + 1, lEnd - lPos) + ")";
                                    }
                                    else
                                    {
                                        lF = lF.Substring(0, lStart) + "pow(" + lF.Substring(lStart, lPos - lStart) + "," + lF.Substring(lPos + 1, lEnd - lPos) + ")" + lF.Substring(lEnd + 1);
                                    }
                                }
                            }

                        }
                        else
                        {
                            int lStart = getPreviousPos(lF, lPos - 1);
                            int lEnd = getNextPos(lF, lPos + 1);
                            if (lStart == 0)
                            {
                                if (lEnd > 0)
                                {
                                    if (lEnd >= lF.Length - 1)
                                    {
                                        lF = "pow(" + lF.Substring(0, lPos - 1) + "," + lF.Substring(lPos + 1, lEnd - lPos) + ")";
                                    }
                                    else
                                    {
                                        lF = "pow(" + lF.Substring(0, lPos - 1) + "," + lF.Substring(lPos + 1, lEnd - lPos) + ")" + lF.Substring(lEnd + 1);
                                    }
                                }
                            }
                            else
                            {
                                if (lEnd > 0)
                                {
                                    if (lEnd >= lF.Length - 1)
                                    {
                                        lF = lF.Substring(0, lStart) + "pow(" + lF.Substring(lStart, lPos - lStart) + "," + lF.Substring(lPos + 1, lEnd - lPos) + ")";
                                    }
                                    else
                                    {
                                        lF = lF.Substring(0, lStart) + "pow(" + lF.Substring(lStart, lPos - lStart) + "," + lF.Substring(lPos + 1, lEnd - lPos) + ")" + lF.Substring(lEnd + 1);
                                    }
                                }
                            }

                        }
                    }

                    if (lF.IndexOf("SQRT") >= 0)
                    {
                        lF = lF.Replace("SQRT", "sqrt");
                    }

                    if (lF.IndexOf("SIN") >= 0)
                    {
                        lF = lF.Replace("SIN", "sin");
                    }

                    if (lF.IndexOf("COS") >= 0)
                    {
                        lF = lF.Replace("COS", "cos");
                    }

                    if (lF.IndexOf("TG") >= 0)
                    {
                        lF = lF.Replace("TG", "tan");
                    }

                    if (lF.IndexOf("TAN") >= 0)
                    {
                        lF = lF.Replace("TAN", "tan");
                    }

                    if (lF.IndexOf("MAX") >= 0)
                    {
                        lF = lF.Replace("MAX", "max");
                    }

                    if (lF.IndexOf("MIN") >= 0)
                    {
                        lF = lF.Replace("MIN", "min");
                    }

                    if (lF.IndexOf("POW") >= 0)
                    {
                        lF = lF.Replace("POW", "pow");
                    }

                    if (lF.IndexOf("Sqrt") >= 0)
                    {
                        lF = lF.Replace("Sqrt", "sqrt");
                    }

                    if (lF.IndexOf("Sin") >= 0)
                    {
                        lF = lF.Replace("Sin", "sin");
                    }

                    if (lF.IndexOf("Cos") >= 0)
                    {
                        lF = lF.Replace("Cos", "cos");
                    }

                    if (lF.IndexOf("Tg") >= 0)
                    {
                        lF = lF.Replace("Tg", "tan");
                    }

                    if (lF.IndexOf("Tan") >= 0)
                    {
                        lF = lF.Replace("Tan", "tan");
                    }

                    if (lF.IndexOf("Max") >= 0)
                    {
                        lF = lF.Replace("Max", "max");
                    }

                    if (lF.IndexOf("Min") >= 0)
                    {
                        lF = lF.Replace("Min", "min");
                    }

                    if (lF.IndexOf("Pow") >= 0)
                    {
                        lF = lF.Replace("Pow", "pow");
                    }

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
                    if (lF.IndexOf("PI") >= 0)
                    {
                        lF = lF.Replace("PI", "3.14159");
                    }
                    if (lF.IndexOf("Pi") >= 0)
                    {
                        lF = lF.Replace("Pi", "3.14159");
                    }
                    if (lF.IndexOf("pI") >= 0)
                    {
                        lF = lF.Replace("pI", "3.14159");
                    }
                    if (lF.IndexOf("pi") >= 0)
                    {
                        lF = lF.Replace("pi", "3.14159");
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

                    if (lF.IndexOf("tg") >= 0)
                    {
                        lF = lF.Replace("tg", "Tan");
                    }

                    if (lF.IndexOf("tan") >= 0)
                    {
                        lF = lF.Replace("tan", "Tan");
                    }

                    if (lF.IndexOf("max") >= 0)
                    {
                        lF = lF.Replace("max", "Max");
                    }

                    if (lF.IndexOf("min") >= 0)
                    {
                        lF = lF.Replace("min", "Min");
                    }

                    if (lF.IndexOf("pow") >= 0)
                    {
                        lF = lF.Replace("pow", "Pow");
                    }


                    if (lF.IndexOf("SQRT") >= 0)
                    {
                        lF = lF.Replace("SQRT", "Sqrt");
                    }

                    if (lF.IndexOf("SIN") >= 0)
                    {
                        lF = lF.Replace("SIN", "Sin");
                    }

                    if (lF.IndexOf("COS") >= 0)
                    {
                        lF = lF.Replace("COS", "Cos");
                    }

                    if (lF.IndexOf("TG") >= 0)
                    {
                        lF = lF.Replace("TG", "Tan");
                    }

                    if (lF.IndexOf("TAN") >= 0)
                    {
                        lF = lF.Replace("TAN", "Tan");
                    }

                    if (lF.IndexOf("MAX") >= 0)
                    {
                        lF = lF.Replace("MAX", "Max");
                    }

                    if (lF.IndexOf("MIN") >= 0)
                    {
                        lF = lF.Replace("MIN", "Min");
                    }

                    if (lF.IndexOf("POW") >= 0)
                    {
                        lF = lF.Replace("POW", "Pow");
                    }

                    Expression lExp = new Expression(lF);

                    if (!lExp.HasErrors())
                    {
                        var lSResult = lExp.Evaluate().ToString();
                        if (lSResult.IndexOf('.') > 0) lSResult = lSResult.Substring(0, lSResult.IndexOf('.'));
                        int.TryParse(lSResult, out lVarMax);
                        if (lVarMax > 0)
                        {
                            lDed = pInvLength - lVarMax;
                            if (lDed < 0)
                            {
                                lDed = 0;
                            }
                        }
                    }

                    lProcess.CloseNDSConnection(ref lNDSCn);
                    lProcess = null;

                    return lDed;
                }

                var lPreValue = 0;

                lSQL = "SELECT CSD_SEQ_NO, CSD_MATCH_PAR1, CSD_TYPE, CSD_PAR2, CSD_VISIBLE, CSD_ProdLength,CSD_INPUT_TYPE " +
                "FROM dbo.T_CAB_SHAPE_DTLS C, " +
                "dbo.shapeparamdetails_cube P, " +
                "dbo.shapemaster_cube M " +
                "WHERE C.CSD_SHAPE_ID = M.chrShapeCode " +
                "AND M.intShapeID = P.intShapeID " +
                "AND C.CSD_SEQ_NO = P.intParamSeq " +
                "AND CSD_SHAPE_ID = '" + lShapeCode + "' " +
                "ORDER BY P.intParamSeq, P.chrParamName ";

                lCmd = new SqlCommand(lSQL, lNDSCn);
                adoRst = lCmd.ExecuteReader();
                if (adoRst.HasRows)
                {
                    while (adoRst.Read())
                    {
                        int lSeq = (int)Math.Round(adoRst.GetDecimal(0));
                        string lParName = adoRst.GetValue(1) == DBNull.Value ? "" : adoRst.GetString(1).Trim().ToUpper();
                        string lParType = adoRst.GetValue(2) == DBNull.Value ? "" : adoRst.GetString(2).Trim().ToUpper();
                        int lParValue = adoRst.GetValue(3) == DBNull.Value ? 0 : (int)Math.Round((decimal)adoRst.GetDecimal(3));
                        string lParVisible = adoRst.GetValue(4) == DBNull.Value ? "" : adoRst.GetString(4).Trim().ToUpper();
                        string lParPLInd = adoRst.GetValue(5) == DBNull.Value ? "" : adoRst.GetString(5).Trim().ToUpper();
                        string lInpuType = adoRst.GetValue(6) == DBNull.Value ? "" : adoRst.GetString(6).Trim().ToUpper();

                        lValue = 0;
                        if (lParVisible.ToUpper() == "INVISIBLE" && lParValue > 0)
                        {
                            lValue = lParValue;
                        }
                        else
                        {
                            int lValue1 = 0;
                            int lValue2 = 0;
                            getParaValue(pOrderItem, lParName, ref lValue1, ref lValue2);

                            lValue = lValue1;

                            if (lValue < lValue2)
                            {
                                lValue = lValue2;
                            }
                        }

                        if (lSeq > 1)
                        {
                            if (lParType.ToUpper().Equals("ANGLE") || lParType.ToUpper().Equals("ANGLE_3D"))
                            {
                                #region Angle
                                //Invisible value is same as previously
                                if (lParVisible.ToUpper() == "INVISIBLE" && lParValue != 90 && lParValue != 180 && lPreValue > 0)
                                {
                                    lValue = lPreValue;
                                }
                                else
                                {
                                    lPreValue = lValue;
                                }

                                int lDed = 0;
                                if (lValue < 90)
                                {
                                    lDed = Convert.ToInt32((2 * ((Convert.ToDouble(pOrderItem.PinSize) / 2) +
                                        Convert.ToDouble(pOrderItem.BarSize)) * Convert.ToDouble(Math.Tan(Convert.ToDouble(lValue) * Math.PI / 360))) -
                                        Convert.ToDouble((Math.PI * Convert.ToDouble(lValue) * ((Convert.ToDouble(pOrderItem.PinSize) / 2) + (0.5 * Convert.ToDouble(pOrderItem.BarSize)))) / 180));
                                }
                                else if (lValue == 90)
                                {
                                    lDed = Convert.ToInt32(1.215 * Convert.ToDouble(pOrderItem.BarSize)) + Convert.ToInt32(0.215 * Convert.ToDouble(pOrderItem.PinSize));
                                }
                                else if (lValue > 90)
                                {
                                    lDed -= Convert.ToInt32(2 * ((Convert.ToDouble(pOrderItem.PinSize) / 2) + Convert.ToDouble(pOrderItem.BarSize)) -
                                        Convert.ToDouble((Math.PI * Convert.ToDouble(lValue) * ((Convert.ToDouble(pOrderItem.PinSize) / 2) + (0.5 * Convert.ToDouble(pOrderItem.BarSize)))) / 180));
                                }
                                if (lDed > 0)
                                {
                                    lTotalDed = lTotalDed + lDed;
                                }

                                #endregion
                            }
                            else if (lParType.ToUpper().Equals("ARC"))
                            {
                                int lDed = 0;
                                if (lInpuType.Equals("RAD+ARCLENGTH"))
                                {
                                    lDed = lValue - Convert.ToInt32((0.57 * lValue) - (1.57 * Convert.ToDouble(pOrderItem.BarSize)));
                                }
                                else if (lInpuType.Equals("RAD+SW_ANGLE"))
                                {
                                }
                                else if (lInpuType.Equals("DIA+SW_ANGLE"))
                                {
                                    if (lParValue > 90)
                                    {
                                        lDed = Convert.ToInt32((2 * ((Convert.ToDouble(pOrderItem.PinSize) / 2) +
                                            Convert.ToDouble(pOrderItem.BarSize)) * Convert.ToDouble(Math.Tan(lParValue) * Math.PI / 360) -
                                            Convert.ToDouble((Math.PI * lParValue) * ((Convert.ToDouble(pOrderItem.PinSize) / 2) + (0.5 * Convert.ToDouble(pOrderItem.BarSize)))) / 180));
                                    }
                                    else
                                    {
                                        lDed = Convert.ToInt32(2 * ((Convert.ToDouble(pOrderItem.PinSize) / 2) + Convert.ToDouble(pOrderItem.BarSize)) -
                                            Convert.ToDouble((Math.PI * lParValue * ((Convert.ToDouble(pOrderItem.PinSize) / 2) + (0.5 * Convert.ToDouble(pOrderItem.BarSize)))) / 180));
                                    }
                                }

                                if (lDed > 0)
                                {
                                    lTotalDed = lTotalDed + lDed;
                                }
                            }
                            else if (lParType.ToUpper().Equals("ARC LENGTH"))
                            {
                                int lDed = 0;
                                if (lInpuType.Equals("RAD+SW_ANGLE"))
                                {
                                    if (lValuePre > 0)
                                    {
                                        lDed = lValuePre - Convert.ToInt32((lValuePre * (lValue + (Convert.ToDouble(pOrderItem.BarSize) / 2))) / (lValue + Convert.ToDouble(pOrderItem.BarSize)));
                                    }
                                }

                                if (lDed > 0)
                                {
                                    lTotalDed = lTotalDed + lDed;
                                }
                            }
                        }
                        lValuePre = lValue;
                    }
                }

                adoRst.Close();
                lProcess.CloseNDSConnection(ref lNDSCn);
            }
            catch (Exception ex)
            {
                lProcess.CloseNDSConnection(ref lNDSCn);
                var lErrorMsg = ex.Message;
            }
            lProcess = null;
            return lTotalDed;
        }

        int getCoupleDed(int pBarSize)
        {
            int lDed = 0;

            if (pBarSize >= 20 && pBarSize < 25)
            {
                lDed = 5;
            }
            else if (pBarSize >= 25 && pBarSize < 28)
            {
                lDed = 10;
            }
            else if (pBarSize >= 28 && pBarSize < 40)
            {
                lDed = 15;
            }
            else if (pBarSize >= 40 && pBarSize < 50)
            {
                lDed = 25;
            }
            else if (pBarSize >= 50)
            {
                lDed = 30;
            }

            return lDed;
        }

        int getNextPos(string pInput, int pPos)
        {
            int lReturn = 99999;

            int lTry = pInput.IndexOf("+", pPos);
            if (lTry > 0 && lTry < lReturn)
            {
                lReturn = lTry - 1;
            }

            lTry = pInput.IndexOf("-", pPos);
            if (lTry > 0 && lTry < lReturn)
            {
                lReturn = lTry - 1;
            }

            lTry = pInput.IndexOf("*", pPos);
            if (lTry > 0 && lTry < lReturn)
            {
                lReturn = lTry - 1;
            }

            lTry = pInput.IndexOf("/", pPos);
            if (lTry > 0 && lTry < lReturn)
            {
                lReturn = lTry - 1;
            }

            lTry = pInput.IndexOf("^", pPos);
            if (lTry > 0 && lTry < lReturn)
            {
                lReturn = lTry - 1;
            }

            lTry = pInput.IndexOf("(", pPos);
            if (lTry > 0 && lTry < lReturn)
            {
                lReturn = lTry - 1;
            }

            lTry = pInput.IndexOf(")", pPos);
            if (lTry > 0 && lTry < lReturn)
            {
                lReturn = lTry - 1;
            }

            if (lReturn == 99999)
            {
                lReturn = pInput.Length - 1;
            }
            return lReturn;
        }

        int getPreviousPos(string pInput, int pPos)
        {
            int lReturn = 0;

            int lTry = pInput.LastIndexOf("+", pPos);
            if (lTry > 0 && lTry > lReturn)
            {
                lReturn = lTry + 1;
            }

            lTry = pInput.LastIndexOf("-", pPos);
            if (lTry > 0 && lTry > lReturn)
            {
                lReturn = lTry + 1;
            }

            lTry = pInput.LastIndexOf("*", pPos);
            if (lTry > 0 && lTry > lReturn)
            {
                lReturn = lTry + 1;
            }

            lTry = pInput.LastIndexOf("/", pPos);
            if (lTry > 0 && lTry > lReturn)
            {
                lReturn = lTry + 1;
            }

            lTry = pInput.LastIndexOf("^", pPos);
            if (lTry > 0 && lTry > lReturn)
            {
                lReturn = lTry + 1;
            }

            lTry = pInput.LastIndexOf("(", pPos);
            if (lTry > 0 && lTry > lReturn)
            {
                lReturn = lTry + 1;
            }

            lTry = pInput.LastIndexOf(")", pPos);
            if (lTry > 0 && lTry > lReturn)
            {
                lReturn = lTry + 1;
            }

            return lReturn;
        }

        int getPreviousBracket(string pInput, int pPos)
        {
            int lReturn = 0;

            int lTry = pInput.LastIndexOf("(", pPos);

            if (lTry >= 0)
            {
                int lTry2 = pInput.IndexOf(")", lTry);
                if (lTry2 > 0 && lTry2 == pPos)
                {
                    lReturn = lTry;
                }
                else if (lTry2 > 0 && lTry2 < pPos)
                {
                    int lCount = 0;
                    while (lTry2 > 0 && lTry2 < pPos)
                    {
                        lTry2 = pInput.IndexOf(")", lTry2 + 1);
                        lCount++;
                        if (lCount > 1000)
                        {
                            break;
                        }
                    }
                    if (lCount >= 0)
                    {
                        lTry = pPos;
                        for (int i = 0; i <= lCount; i++)
                        {
                            lTry = pInput.LastIndexOf("(", lTry - 1);
                        }

                        if (lTry >= 0)
                        {
                            lReturn = lTry;
                        }
                    }
                }
            }

            return lReturn;
        }

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

        /// <summary>
        /// Method to get Coupler refix with dia from the integer formed in UI.
        /// </summary>
        /// <param name="couplerVal"></param>
        /// <returns></returns>
        public string GetCoupler(string couplerVal)
        {
            string couplerWithDia = "";
            int couplerIndex = Convert.ToInt32(couplerVal.Substring(0, 2));
            string dia = couplerVal.Substring(2, 2);
            try
            {
                switch (couplerIndex)
                {
                    case 10:
                        couplerWithDia = "ESCO" + dia;
                        break;
                    case 11:
                        couplerWithDia = "ESCN" + dia;
                        break;
                    case 12:
                        couplerWithDia = "ESCS" + dia;
                        break;
                    case 13:
                        couplerWithDia = "EECO" + dia;
                        break;
                    case 14:
                        couplerWithDia = "EECN" + dia;
                        break;
                    case 15:
                        couplerWithDia = "EECS" + dia;
                        break;
                    case 16:
                        couplerWithDia = "EBCS" + dia;
                        break;
                    case 17:
                        couplerWithDia = "EESO" + dia;
                        break;
                    case 18:
                        couplerWithDia = "EESN" + dia;
                        break;
                    case 19:
                        couplerWithDia = "EESS" + dia;
                        break;
                    case 20:
                        couplerWithDia = "ESSO" + dia;
                        break;
                    case 21:
                        couplerWithDia = "ESSN" + dia;
                        break;
                    case 22:
                        couplerWithDia = "ESSS" + dia;
                        break;
                    case 23:
                        couplerWithDia = "ELCS" + dia;
                        break;
                    case 24:
                        couplerWithDia = "ELSS" + dia;
                        break;
                    case 25:
                        couplerWithDia = "DEC" + dia;
                        break;
                    case 26:
                        couplerWithDia = "DES" + dia;
                        break;
                    case 27:
                        couplerWithDia = "DSC" + dia;
                        break;
                    case 28:
                        couplerWithDia = "DSS" + dia;
                        break;
                    case 29:
                        couplerWithDia = "DLC" + dia;
                        break;
                    case 30:
                        couplerWithDia = "DLN" + dia;
                        break;
                    case 31:
                        couplerWithDia = "DNEC" + dia;
                        break;
                    case 32:
                        couplerWithDia = "DNES" + dia;
                        break;
                    case 33:
                        couplerWithDia = "DNSC" + dia;
                        break;
                    case 34:
                        couplerWithDia = "DNSS" + dia;
                        break;
                    case 35:
                        couplerWithDia = "DNLC" + dia;
                        break;
                    case 36:
                        couplerWithDia = "DNLN" + dia;
                        break;
                    default:
                        couplerWithDia = "";
                        break;
                }
            }
            catch (Exception ex)
            {
                return "";
            }
            return couplerWithDia;
        }


        public int ValidLC(OrderDetailsRetModels pItem, string pshapeTransportValidator)
        {
            // return max length of bar in transport
            var lWidth = 0;
            var lLeng = 0;
            var lItem = pItem;
            var lF = pshapeTransportValidator;
            var lResult = 0;
            if (lF != null && lF != "")
            {
                var lOr = lF.Split(new string[] { "||" }, StringSplitOptions.None);
                for (var i = 0; i < lOr.Length; i++)
                {
                    var lAnd = lOr[i].Split(new string[] { "&&" }, StringSplitOptions.None);
                    for (var j = 0; j < lAnd.Length; j++)
                    {
                        if (lAnd[j].IndexOf("A") >= 0) if (lItem.A != null) lAnd[j] = lAnd[j].Replace("A", getVarMaxValue(lItem.A.ToString()).ToString()); else lAnd[j] = lAnd[j].Replace("A", "0");
                        if (lAnd[j].IndexOf("B") >= 0) if (lItem.B != null) lAnd[j] = lAnd[j].Replace("B", getVarMaxValue(lItem.B.ToString()).ToString()); else lAnd[j] = lAnd[j].Replace("B", "0");
                        if (lAnd[j].IndexOf("C") >= 0) if (lItem.C != null) lAnd[j] = lAnd[j].Replace("C", getVarMaxValue(lItem.C.ToString()).ToString()); else lAnd[j] = lAnd[j].Replace("C", "0");
                        if (lAnd[j].IndexOf("D") >= 0) if (lItem.D != null) lAnd[j] = lAnd[j].Replace("D", getVarMaxValue(lItem.D.ToString()).ToString()); else lAnd[j] = lAnd[j].Replace("D", "0");
                        if (lAnd[j].IndexOf("E") >= 0) if (lItem.E != null) lAnd[j] = lAnd[j].Replace("E", getVarMaxValue(lItem.E.ToString()).ToString()); else lAnd[j] = lAnd[j].Replace("E", "0");
                        if (lAnd[j].IndexOf("F") >= 0) if (lItem.F != null) lAnd[j] = lAnd[j].Replace("F", getVarMaxValue(lItem.F.ToString()).ToString()); else lAnd[j] = lAnd[j].Replace("F", "0");
                        if (lAnd[j].IndexOf("G") >= 0) if (lItem.G != null) lAnd[j] = lAnd[j].Replace("G", getVarMaxValue(lItem.G.ToString()).ToString()); else lAnd[j] = lAnd[j].Replace("G", "0");
                        if (lAnd[j].IndexOf("H") >= 0) if (lItem.H != null) lAnd[j] = lAnd[j].Replace("H", getVarMaxValue(lItem.H.ToString()).ToString()); else lAnd[j] = lAnd[j].Replace("H", "0");
                        if (lAnd[j].IndexOf("I") >= 0) if (lItem.I != null) lAnd[j] = lAnd[j].Replace("I", getVarMaxValue(lItem.I.ToString()).ToString()); else lAnd[j] = lAnd[j].Replace("I", "0");
                        if (lAnd[j].IndexOf("J") >= 0) if (lItem.J != null) lAnd[j] = lAnd[j].Replace("J", getVarMaxValue(lItem.J.ToString()).ToString()); else lAnd[j] = lAnd[j].Replace("J", "0");
                        if (lAnd[j].IndexOf("K") >= 0) if (lItem.K != null) lAnd[j] = lAnd[j].Replace("K", getVarMaxValue(lItem.K.ToString()).ToString()); else lAnd[j] = lAnd[j].Replace("K", "0");
                        if (lAnd[j].IndexOf("L") >= 0) if (lItem.L != null) lAnd[j] = lAnd[j].Replace("L", getVarMaxValue(lItem.L.ToString()).ToString()); else lAnd[j] = lAnd[j].Replace("L", "0");
                        if (lAnd[j].IndexOf("M") >= 0) if (lItem.M != null) lAnd[j] = lAnd[j].Replace("M", getVarMaxValue(lItem.M.ToString()).ToString()); else lAnd[j] = lAnd[j].Replace("M", "0");
                        if (lAnd[j].IndexOf("N") >= 0) if (lItem.N != null) lAnd[j] = lAnd[j].Replace("N", getVarMaxValue(lItem.N.ToString()).ToString()); else lAnd[j] = lAnd[j].Replace("N", "0");
                        if (lAnd[j].IndexOf("O") >= 0) if (lItem.O != null) lAnd[j] = lAnd[j].Replace("O", getVarMaxValue(lItem.O.ToString()).ToString()); else lAnd[j] = lAnd[j].Replace("O", "0");
                        if (lAnd[j].IndexOf("P") >= 0) if (lItem.P != null) lAnd[j] = lAnd[j].Replace("P", getVarMaxValue(lItem.P.ToString()).ToString()); else lAnd[j] = lAnd[j].Replace("P", "0");
                        if (lAnd[j].IndexOf("Q") >= 0) if (lItem.Q != null) lAnd[j] = lAnd[j].Replace("Q", getVarMaxValue(lItem.Q.ToString()).ToString()); else lAnd[j] = lAnd[j].Replace("Q", "0");
                        if (lAnd[j].IndexOf("R") >= 0) if (lItem.R != null) lAnd[j] = lAnd[j].Replace("R", getVarMaxValue(lItem.R.ToString()).ToString()); else lAnd[j] = lAnd[j].Replace("R", "0");
                        if (lAnd[j].IndexOf("S") >= 0) if (lItem.S != null) lAnd[j] = lAnd[j].Replace("S", getVarMaxValue(lItem.S.ToString()).ToString()); else lAnd[j] = lAnd[j].Replace("S", "0");
                        if (lAnd[j].IndexOf("T") >= 0) if (lItem.T != null) lAnd[j] = lAnd[j].Replace("T", getVarMaxValue(lItem.T.ToString()).ToString()); else lAnd[j] = lAnd[j].Replace("T", "0");
                        if (lAnd[j].IndexOf("U") >= 0) if (lItem.U != null) lAnd[j] = lAnd[j].Replace("U", getVarMaxValue(lItem.U.ToString()).ToString()); else lAnd[j] = lAnd[j].Replace("U", "0");
                        if (lAnd[j].IndexOf("V") >= 0) if (lItem.V != null) lAnd[j] = lAnd[j].Replace("V", getVarMaxValue(lItem.V.ToString()).ToString()); else lAnd[j] = lAnd[j].Replace("V", "0");
                        if (lAnd[j].IndexOf("W") >= 0) if (lItem.W != null) lAnd[j] = lAnd[j].Replace("W", getVarMaxValue(lItem.W.ToString()).ToString()); else lAnd[j] = lAnd[j].Replace("W", "0");
                        if (lAnd[j].IndexOf("X") >= 0) if (lItem.X != null) lAnd[j] = lAnd[j].Replace("X", getVarMaxValue(lItem.X.ToString()).ToString()); else lAnd[j] = lAnd[j].Replace("X", "0");
                        if (lAnd[j].IndexOf("Y") >= 0) if (lItem.Y != null) lAnd[j] = lAnd[j].Replace("Y", getVarMaxValue(lItem.Y.ToString()).ToString()); else lAnd[j] = lAnd[j].Replace("Y", "0");
                        if (lAnd[j].IndexOf("Z") >= 0) if (lItem.Z != null) lAnd[j] = lAnd[j].Replace("Z", getVarMaxValue(lItem.Z.ToString()).ToString()); else lAnd[j] = lAnd[j].Replace("Z", "0");

                        if (lAnd[j].IndexOf("math.") >= 0)
                        {
                            lAnd[j] = lAnd[j].Replace("math.", "");
                        }
                        if (lAnd[j].IndexOf("pi") >= 0)
                        {
                            lAnd[j] = lAnd[j].Replace("pi", "3.14159");
                        }
                        if (lAnd[j].IndexOf("PI") >= 0)
                        {
                            lAnd[j] = lAnd[j].Replace("PI", "3.14159");
                        }
                        if (lAnd[j].IndexOf("Pi") >= 0)
                        {
                            lAnd[j] = lAnd[j].Replace("Pi", "3.14159");
                        }
                        if (lAnd[j].IndexOf("pI") >= 0)
                        {
                            lAnd[j] = lAnd[j].Replace("pI", "3.14159");
                        }

                        if (lAnd[j].IndexOf("sqrt") >= 0)
                        {
                            lAnd[j] = lAnd[j].Replace("sqrt", "Sqrt");
                        }

                        if (lAnd[j].IndexOf("sin") >= 0)
                        {
                            lAnd[j] = lAnd[j].Replace("sin", "Sin");
                        }

                        if (lAnd[j].IndexOf("cos") >= 0)
                        {
                            lAnd[j] = lAnd[j].Replace("cos", "Cos");
                        }

                        if (lAnd[j].IndexOf("max") >= 0)
                        {
                            lAnd[j] = lAnd[j].Replace("max", "Max");
                        }

                        if (lAnd[j].IndexOf("min") >= 0)
                        {
                            lAnd[j] = lAnd[j].Replace("min", "Min");
                        }

                        // Expression lExp = new Expression(lAnd[j]);  //commented temp
                        //lExp.Parameters["A"] = (int)(lItem.A == null ? 0 : lItem.A);
                        //lExp.Parameters["B"] = (int)(lItem.B == null ? 0 : lItem.B);
                        //lExp.Parameters["C"] = (int)(lItem.C == null ? 0 : lItem.C);
                        //lExp.Parameters["D"] = (int)(lItem.D == null ? 0 : lItem.D);
                        //lExp.Parameters["E"] = (int)(lItem.E == null ? 0 : lItem.E);
                        //lExp.Parameters["F"] = (int)(lItem.F == null ? 0 : lItem.F);
                        //lExp.Parameters["G"] = (int)(lItem.G == null ? 0 : lItem.G);
                        //lExp.Parameters["H"] = (int)(lItem.H == null ? 0 : lItem.H);
                        //lExp.Parameters["I"] = (int)(lItem.I == null ? 0 : lItem.I);
                        //lExp.Parameters["J"] = (int)(lItem.J == null ? 0 : lItem.J);
                        //lExp.Parameters["K"] = (int)(lItem.K == null ? 0 : lItem.K);
                        //lExp.Parameters["L"] = (int)(lItem.L == null ? 0 : lItem.L);
                        //lExp.Parameters["M"] = (int)(lItem.M == null ? 0 : lItem.M);
                        //lExp.Parameters["N"] = (int)(lItem.N == null ? 0 : lItem.N);
                        //lExp.Parameters["O"] = (int)(lItem.O == null ? 0 : lItem.O);
                        //lExp.Parameters["P"] = (int)(lItem.P == null ? 0 : lItem.P);
                        //lExp.Parameters["Q"] = (int)(lItem.Q == null ? 0 : lItem.Q);
                        //lExp.Parameters["R"] = (int)(lItem.R == null ? 0 : lItem.R);
                        //lExp.Parameters["S"] = (int)(lItem.S == null ? 0 : lItem.S);
                        //lExp.Parameters["T"] = (int)(lItem.T == null ? 0 : lItem.T);
                        //lExp.Parameters["U"] = (int)(lItem.U == null ? 0 : lItem.U);
                        //lExp.Parameters["V"] = (int)(lItem.V == null ? 0 : lItem.V);
                        //lExp.Parameters["W"] = (int)(lItem.W == null ? 0 : lItem.W);
                        //lExp.Parameters["X"] = (int)(lItem.X == null ? 0 : lItem.X);
                        //lExp.Parameters["Y"] = (int)(lItem.Y == null ? 0 : lItem.Y);
                        //lExp.Parameters["Z"] = (int)(lItem.Z == null ? 0 : lItem.Z);

                        try
                        {
                            //commented temp
                            //if (!lExp.HasErrors())
                            //{
                            //    var lSResult = lExp.Evaluate().ToString();
                            //    if (lSResult.IndexOf('.') > 0) lSResult = lSResult.Substring(0, lSResult.IndexOf('.'));
                            //    int.TryParse(lSResult, out lResult);
                            //    if (j == 0)
                            //    {
                            //        lWidth = lResult;
                            //    }
                            //    else
                            //    {
                            //        lLeng = lResult;
                            //    }     
                            //}
                            //commented temp

                        }
                        catch (Exception ex)
                        {
                            lWidth = 0;
                        }
                    }
                }
            }
            lResult = 0;
            if (lWidth > 7000)
            {
                if (pItem != null && pItem.BarShapeCode == "037")
                {
                    if (lLeng > 0 && lLeng < 2400)
                    {
                        if (Math.Sqrt(lWidth * lWidth - (2400 - lLeng) * (2400 - lLeng)) > 7000)
                        {
                            lResult = 1;
                        }
                    }
                    else
                    {
                        lResult = 1;
                    }

                }
                else
                {
                    lResult = 1;
                }
            }
            if (lLeng > 7000)
            {
                if (pItem != null && pItem.BarShapeCode == "037")
                {
                    if (lWidth > 0 && lWidth < 2400)
                    {
                        if (Math.Sqrt(lLeng * lLeng - (2400 - lWidth) * (2400 - lWidth)) > 7000)
                        {
                            lResult = 1;
                        }
                    }
                    else
                    {
                        lResult = 1;
                    }
                }
                else
                {
                    lResult = 1;
                }
            }
            if (lWidth > 2400 && lLeng > 2400) lResult = 2;
            if (lResult == 0 && pItem != null && pItem.BarShapeCode != null && pItem.A != null)
            {
                if (pItem.BarShapeCode == "020" || pItem.BarShapeCode == "20")
                {
                    if (getVarMaxValue(pItem.A.ToString()) > 7200)
                    {
                        lResult = 1;
                    }
                }

            }
            return lResult;
        }


        public string checkPlanComplete(int OrderNumber, string UserName)
        {
            string lCustomerCode = "";
            string lProjectCode = "";

            int lPMDOnly = 1;
            var lUserType = "";

            string lReturn = "Yes";

            if (UserName != null && UserName.IndexOf("@") > 0
            && UserName.Split('@')[1].ToUpper() == "NATSTEEL.COM.SG")
            {
                UserAccessController lUa = new UserAccessController();

                var lHeader = db.OrderProject.Find(OrderNumber);
                if (lHeader == null || (lHeader.OrderStatus != "New" && lHeader.OrderStatus != "Created" && lHeader.OrderStatus != "Sent" && lHeader.OrderStatus != "Created*"))
                {
                    return lReturn;
                }

                var lSE = (from p in db.OrderProjectSE
                           where p.OrderNumber == OrderNumber &&
                           p.ProductType == "CAB"
                           select p).ToList();

                if (lSE == null || lSE.Count == 0)
                {
                    return lReturn;
                }

                lCustomerCode = lHeader.CustomerCode;
                lProjectCode = lHeader.ProjectCode;

                for (int i = 0; i < lSE.Count; i++)
                {
                    int lJobID = lSE[i].CABJobID;

                    if (lJobID <= 0)
                    {
                        continue;
                    }

                    //check BBS
                    var lBBS = (from p in db.BBS
                                where p.CustomerCode == lCustomerCode &&
                                p.ProjectCode == lProjectCode &&
                                p.JobID == lJobID
                                select p).ToList();
                    for (int j = 0; j < lBBS.Count; j++)
                    {
                        int BBSID = lBBS[j].BBSID;
                        var lUpdatedBy = (from p in db.OrderDetails
                                          where p.CustomerCode == lCustomerCode &&
                                          p.ProjectCode == lProjectCode &&
                                          p.JobID == lJobID &&
                                          p.BBSID == BBSID &&
                                          p.Cancelled == null &&
                                          ((p.BarShapeCode != null &&
                                          p.BarShapeCode != "") ||
                                          (p.BarSize != null &&
                                          p.BarSize > 0) ||
                                          (p.BarEachQty != null &&
                                          p.BarEachQty > 0) ||
                                          (p.BarMemberQty != null &&
                                          p.BarMemberQty > 0))
                                          select p.UpdateBy).Distinct().ToList();

                        if (lUpdatedBy.Count > 0)
                        {
                            for (int k = 0; k < lUpdatedBy.Count; k++)
                            {
                                lUserType = lUa.getUserType(lUpdatedBy[k]);

                                if (lUserType != "AD" && lUserType != "PM"
                                && lUserType != "PA" && lUserType != "P1"
                                && lUserType != "P2" && lUserType != "PU" && lUserType != "TE")
                                {
                                    lPMDOnly = 0;
                                    break;
                                }

                            }
                        }

                        if (lPMDOnly == 0)
                        {
                            break;
                        }
                    }

                    if (lPMDOnly == 0)
                    {
                        break;
                    }
                }

                lUa = null;

                if (lPMDOnly == 0)
                {
                    var lErrorMsg = checkCABDetails(OrderNumber, UserName);
                    if (lErrorMsg != "")
                    {
                        lReturn = "No";
                    }
                }

            }
            return lReturn;
        }


        public List<HmiProjectAddress> GetHmiAddress(string projectcodes)
        {
            List<HmiProjectAddress> addressList = new List<HmiProjectAddress>();
            SqlConnection lNDSCon = null;

            var lProcessObj = new ProcessController();

            // Split comma-separated project codes
            var projectCodeArray = projectcodes.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).ToList();

            if (projectCodeArray.Count == 0)
                return addressList;

            // Create parameter names dynamically: @p0, @p1, @p2...
            var parameterNames = projectCodeArray.Select((code, index) => $"@p{index}").ToList();

            // SQL with IN clause
            string lSQL = $"SELECT project_address_code, project_address FROM HMIProjectAddress WHERE project_code IN ({string.Join(",", parameterNames)})";

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

            return addressList;
        }

    }
}
