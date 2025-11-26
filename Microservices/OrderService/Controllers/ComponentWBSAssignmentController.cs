using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using OrderService.Models;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;

namespace OrderService.Controllers
{
    public class ComponentWBSAssignmentController : Controller
    {

        public string strServer = "PRD";
        public string strClient = "600";

        OracleConnection cnCIS = new OracleConnection();
        OracleConnection cnIDB = new OracleConnection();
        public SqlConnection cnNDS = new SqlConnection();

        SqlTransaction osqlTransNDS;

        private DBContextModels db = new DBContextModels();
        // GET: ComponentWBSAssignment
        public ActionResult Index(string rCustomerCode, string rProjectCode, string rProductType, string rStructureElement)
        {

            string lCustomerCode = rCustomerCode;
            string lProjectCode = rProjectCode;

            string lUserName = User.Identity.GetUserName();

            UserAccessController lUa = new UserAccessController();
            var lUserType = lUa.getUserType(User.Identity.GetUserName());
            var lGroupName = lUa.getGroupName(User.Identity.GetUserName());

            ViewBag.UserType = lUserType;

            ViewBag.UserName = lUserName;

            lUa = null;

            SharedAPIController lBackEnd = new SharedAPIController();

            var lCustSelectList = lBackEnd.getCustomerSelectList(lCustomerCode, lUserType, lGroupName);

            ViewBag.CustomerSelection = lCustSelectList;

            if (lCustomerCode == null || lCustomerCode.Length == 0)
            {
                lCustomerCode = lCustSelectList.Count() > 0 ? lCustSelectList.First().Value : "";
                if (lCustomerCode == null)
                {
                    lCustomerCode = "";
                }
            }

            var lProjSelectList = lBackEnd.getProjectSelectList(lCustomerCode, lProjectCode, lUserType, lGroupName);
            ViewBag.ProjectSelection = lProjSelectList;

            if (lProjectCode == null || lProjectCode.Length == 0)
            {
                lProjectCode = lProjSelectList.First().Value;
                if (lProjectCode == null)
                {
                    lProjectCode = "";
                }
            }

            ViewBag.ProductType = rProductType;
            ViewBag.StructureElement = rStructureElement;

            var lServerType = "DEV";
            var lProcess = new ProcessController();
            lServerType = lProcess.strServer;
            ViewBag.ServerType = lServerType;

            lProcess = null;

            return View();
        }

        [HttpPost]
        public ActionResult getComponentList(string CustomerCode, string ProjectCode, string ProductType, string StructureElement, bool StandardComponent)
        {
            //var lReturn = new List<ComponentModel>();

            var lReturn = new[] { new {
                ComponentID = 0
                ,CustomerCode = ""
                ,ProjectCode = ""
                ,StructureElement = ""
                ,ProductType = ""
                ,ComponentName = ""
                ,Revision = 0
                ,ParentID = 0
                ,TransportMode = ""
                ,TotalWeight = (decimal)0
                ,TotalPCs = 0
                ,Remarks = ""
            } }.ToList();

            lReturn.RemoveAt(0);

            var lCmd = new SqlCommand();
            SqlDataReader lRst;
            var lNDSCon = new SqlConnection();
            string lSQL = "";

            try
            {
                //if (StandardComponent == true)
                //{
                //    lReturn = (from c in db.OESComponent
                //               where c.CustomerCode == CustomerCode
                //               //&& c.ProjectCode == ProjectCode
                //               && c.ProductType == ProductType
                //               && c.StructureElement == StructureElement
                //               && c.Status == "Confirmed"
                //               && c.StandardInd > 0
                //               group c by c.ComponentName into g
                //               orderby g.Key descending
                //               select g.OrderByDescending(x => x.Revision).FirstOrDefault()
                //                     ).ToList();
                //}
                //else
                //{
                //    lReturn = (from c in db.OESComponent
                //               where c.CustomerCode == CustomerCode
                //               && c.ProjectCode == ProjectCode
                //               && c.ProductType == ProductType
                //               && c.StructureElement == StructureElement
                //               && c.Status == "Confirmed"
                //               group c by c.ComponentName into g
                //               orderby g.Key descending
                //               select g.OrderByDescending(x => x.Revision).FirstOrDefault()
                //                     ).ToList();

                //}

                var lProcessObj = new ProcessController();
                if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
                {

                    if (StandardComponent == true)
                    {
                        lSQL = "SELECT ComponentID " +
                        ",CustomerCode " +
                        ",ProjectCode " +
                        ",StructureElement " +
                        ",ProductType " +
                        ",ComponentName " +
                        ",Revision " +
                        ",ParentID " +
                        ",TransportMode " +
                        ",TotalWeight " +
                        ",TotalPCs " +
                        ",Remarks " +
                        "FROM dbo.OESComponent M " +
                        "WHERE CustomerCode = '" + CustomerCode + "' " +
                        "AND ProductType = '" + ProductType + "' " +
                        "AND StructureElement = '" + StructureElement + "' " +
                        "AND Status = 'Confirmed' " +
                        "AND StandardInd > 0 " +
                        "AND Revision = (SELECT MAX(Revision) " +
                        "FROM dbo.OESComponent " +
                        "WHERE ParentID = M.ParentID ) ";
                    }
                    else
                    {
                        lSQL = "SELECT ComponentID " +
                        ",CustomerCode " +
                        ",ProjectCode " +
                        ",StructureElement " +
                        ",ProductType " +
                        ",ComponentName " +
                        ",Revision " +
                        ",ParentID " +
                        ",TransportMode " +
                        ",TotalWeight " +
                        ",TotalPCs " +
                        ",Remarks " +
                        "FROM dbo.OESComponent M " +
                        "WHERE CustomerCode = '" + CustomerCode + "' " +
                        "AND ProjectCode = '" + ProjectCode + "' " +
                        "AND ProductType = '" + ProductType + "' " +
                        "AND StructureElement = '" + StructureElement + "' " +
                        "AND Status = 'Confirmed' " +
                        "AND Revision = (SELECT MAX(Revision) " +
                        "FROM dbo.OESComponent " +
                        "WHERE ParentID = M.ParentID ) ";
                    }

                    lCmd.CommandText = lSQL;
                    lCmd.Connection = lNDSCon;
                    lCmd.CommandTimeout = 300;
                    lRst = lCmd.ExecuteReader();
                    if (lRst.HasRows)
                    {
                        while (lRst.Read())
                        {
                            lReturn.Add(new
                            {
                                ComponentID = lRst.GetValue(0) == DBNull.Value ? 0 : lRst.GetInt32(0),
                                CustomerCode = lRst.GetValue(1) == DBNull.Value ? "" : lRst.GetString(1).Trim(),
                                ProjectCode = lRst.GetValue(2) == DBNull.Value ? "" : lRst.GetString(2).Trim(),
                                StructureElement = lRst.GetValue(3) == DBNull.Value ? "" : lRst.GetString(3).Trim(),
                                ProductType = lRst.GetValue(4) == DBNull.Value ? "" : lRst.GetString(4).Trim(),
                                ComponentName = lRst.GetValue(5) == DBNull.Value ? "" : lRst.GetString(5).Trim(),
                                Revision = lRst.GetValue(6) == DBNull.Value ? 0 : lRst.GetInt32(6),
                                ParentID = lRst.GetValue(7) == DBNull.Value ? 0 : lRst.GetInt32(7),
                                TransportMode = lRst.GetValue(8) == DBNull.Value ? "" : lRst.GetString(8).Trim(),
                                TotalWeight = lRst.GetValue(9) == DBNull.Value ? (decimal)0 : (decimal)lRst.GetDecimal(9),
                                TotalPCs = lRst.GetValue(10) == DBNull.Value ? 0 : lRst.GetInt32(10),
                                Remarks = lRst.GetValue(11) == DBNull.Value ? "" : lRst.GetString(11).Trim(),
                            });
                        }

                    }
                    lRst.Close();

                    lProcessObj.CloseNDSConnection(ref lNDSCon);
                }
                lProcessObj = null;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Server Error: " + ex.Message);
            }
            return Ok(lReturn);
        }

        string getContractNo(string pCustomerCode, string pProjectCode, string pProdType)
        {
            try
            {
                string lContractNo = "";
                var lCmd = new SqlCommand();
                SqlDataReader lRst;
                string lSQL = "";

                var lProces = new ProcessController();
                lProces.OpenNDSConnection(ref cnNDS);
                if (cnNDS.State == ConnectionState.Open)
                {
                    lSQL = "SELECT isNull(MIN(C.vchContractNumber), '') " +
                            "FROM dbo.ContractMaster C, dbo.CustomerMaster U,  " +
                            "dbo.ContractProductMapping M, dbo.SAPProjectMaster P " +
                            "WHERE C.intCustomerCode = U.intCustomerCode " +
                            "AND C.vchContractNumber = M.VBELN " +
                            "AND C.intContractID = P.intContractId " +
                            "AND U.vchCustomerNo = '" + pCustomerCode + "' " +
                            "AND P.vchProjectCode = '" + pProjectCode + "' ";

                    if (pProdType == "CAB")
                    {
                        lSQL = lSQL + "AND M.ytot_cab > 0 ";

                    }
                    else if (pProdType == "MSH")
                    {
                        lSQL = lSQL + "AND M.ytot_mesh > 0 ";

                    }
                    else if (pProdType == "PRC")
                    {
                        lSQL = lSQL + "AND M.ytot_precage > 0 ";

                    }
                    else if (pProdType == "BPC")
                    {
                        lSQL = lSQL + "AND M.ytot_bpc > 0 ";

                    }
                    else if (pProdType == "CAR")
                    {
                        lSQL = lSQL + "AND M.ytot_car > 0 ";

                    }

                    lCmd = new SqlCommand(lSQL, cnNDS);
                    lCmd.CommandTimeout = 1200;
                    lRst = lCmd.ExecuteReader();
                    if (lRst.HasRows)
                    {
                        lRst.Read();
                        lContractNo = lRst.GetString(0);
                    }
                    lRst.Close();

                    lProces.CloseNDSConnection(ref cnNDS);
                    cnNDS = null;
                    lCmd = null;
                }
                return lContractNo;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //var lContractNo = getContractNo(CustomerCode, ProjectCode, lProdType, lNDSCon);

        int getProjectID(string ProjectCode, string ContractNo)
        {
            try
            {


                int lProjectID = 0;
                string lSQL;
                SqlCommand lCmd;
                SqlDataReader adoRst;
                var lProces = new ProcessController();
                lProces.OpenNDSConnection(ref cnNDS);
                if (cnNDS.State == ConnectionState.Open)
                {

                    lSQL = "SELECT P.intProjectId FROM dbo.SAPProjectMaster P WHERE P.vchProjectCode = '" + ProjectCode + "' ";
                 
                    lCmd = new SqlCommand(lSQL, cnNDS);
                    //lCmd.Transaction = osqlTransNDS;
                    lCmd.CommandTimeout = 1200;
                    adoRst = lCmd.ExecuteReader();
                    if (adoRst.HasRows)
                    {
                        adoRst.Read();
                        lProjectID = (int)adoRst.GetValue(0);
                    }
                    adoRst.Close();

                    lProces.CloseNDSConnection(ref cnNDS);
                    cnNDS = null;
                    lCmd = null;
                }
                return lProjectID;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        [HttpPost]
        public ActionResult AssignWBSQty(string CustomerCode, string ProjectCode, string ProductType, string StructureElement, List<int> PostHeaderIDs, List<int> ComponentIDs, List<int> SetQty, List<int> SplitQty)
        {
            bool lSuccess = true;
            string lErrorMsg = "";
            string lSQL;
            SqlCommand lCmd;
            SqlDataReader adoRst;

            int lSplitNo = 0;
            int lSNo = 0;
            string lType = "QTY";

            try
            {
                if (ComponentIDs.Count > 0)
                {
                    //get max loop
                    int lMaxLoop = 0;
                    for (int i = 0; i < ComponentIDs.Count; i++)
                    {
                        int lSetQty = SetQty[i];
                        int lSplitQty = SplitQty[i];
                        if (lSplitQty > 0 && lSetQty > 0)
                        {
                            int lLoop = (int)Math.Ceiling((double)lSetQty / lSplitQty);
                            if (lLoop > lMaxLoop)
                            {
                                lMaxLoop = lLoop;
                            }
                        }
                    }

                    var lProces = new ProcessController();
                    lProces.OpenNDSConnection(ref cnNDS);
                    if (cnNDS.State == ConnectionState.Open)
                    {
                        for (int i = 0; i < lMaxLoop; i++)
                        {
                            for (int j = 0; j < ComponentIDs.Count; j++)
                            {
                                if (ComponentIDs[j] > 0 && SetQty[j] > 0 && SplitQty[j] > 0)
                                {
                                    int lCompID = ComponentIDs[j];
                                    var lCompDet = (from p in db.OESComponent
                                                    where p.ComponentID == lCompID
                                                    select p).ToList();

                                    if (lCompDet != null && lCompDet.Count > 0)
                                    {
                                        int lCompPCs = lCompDet[0].TotalPCs == null ? 0 : (int)lCompDet[0].TotalPCs;
                                        decimal lCompWT = lCompDet[0].TotalWeight == null ? (decimal)0 : (decimal)lCompDet[0].TotalWeight;

                                        int lLeftSet = SetQty[j] - SplitQty[j] * i;
                                        int lSplitQty = SplitQty[j];

                                        if (lLeftSet > 0)
                                        {
                                            if (lSplitQty > lLeftSet)
                                            {
                                                lSNo = lSNo + 1;
                                                SplitInsert(i + 1, lSNo, PostHeaderIDs, ComponentIDs[j], lLeftSet, lCompPCs, lCompWT, lType);
                                            }
                                            else
                                            {
                                                lSNo = lSNo + 1;
                                                SplitInsert(i + 1, lSNo, PostHeaderIDs, ComponentIDs[j], lSplitQty, lCompPCs, lCompWT, lType);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        lProces.CloseNDSConnection(ref cnNDS);
                    }
                    lProces = null;
                }
            }
            catch (Exception ex)
            {
                lSuccess = false;
                lErrorMsg = ex.Message;
            }

            return Ok(new { success = lSuccess, responseText = lErrorMsg });

        }

        [HttpPost]
        public ActionResult SaveAssignComponents(List<int> PostHeaderIDs, List<int> ComponentIDs, List<int> SplitIDs, List<int> SetQtys)
        {
            bool lSuccess = true;
            string lErrorMsg = "";
            string lSQL;
            SqlCommand lCmd;
            SqlDataReader adoRst;

            try
            {
                if (PostHeaderIDs != null && PostHeaderIDs.Count > 0 && ComponentIDs != null && ComponentIDs.Count > 0)
                {
                    var lProces = new ProcessController();
                    lProces.OpenNDSConnection(ref cnNDS);
                    if (cnNDS.State == ConnectionState.Open)
                    {
                        for (int i = 0; i < PostHeaderIDs.Count; i++)
                        {
                            int lPostHeaderID = PostHeaderIDs[i];
                            if (lPostHeaderID > 0)
                            {
                                for (int j = 0; j < ComponentIDs.Count; j++)
                                {
                                    int lComponentIDs = ComponentIDs[j];
                                    if (lComponentIDs > 0 && SplitIDs.Count > j && SetQtys.Count > j)
                                    {
                                        int lSplitID = SplitIDs[j];
                                        int lSetQty = SetQtys[j];
                                        if (lSetQty > 0)
                                        {
                                            lSQL = "UPDATE dbo.OESComponentWBS " +
                                            "SET NoOfSet = " + lSetQty + ", " +
                                            "TotalPCs = PCsPerSet * " + lSetQty + ", " +
                                            "TotalWeight = WTPerSet * " + lSetQty + " " +
                                            "WHERE PostHeaderID = " + lPostHeaderID + " " +
                                            "AND ComponentID = " + lComponentIDs + " " +
                                            "AND SplitID = " + lSplitID + " ";

                                            lCmd = new SqlCommand(lSQL, cnNDS);
                                            //lCmd.Transaction = osqlTransNDS;
                                            lCmd.CommandTimeout = 1200;
                                            lCmd.ExecuteNonQuery();
                                        }
                                    }
                                }
                            }
                        }

                        lProces.CloseNDSConnection(ref cnNDS);
                    }
                }
            }
            catch (Exception ex)
            {
                lSuccess = false;
                lErrorMsg = ex.Message;
            }

            lCmd = null;
            adoRst = null;

            return Ok(new { success = lSuccess, responseText = lErrorMsg });
        }

        [HttpPost]
        public ActionResult AssignWBS(string CustomerCode, string ProjectCode, string ProductType, string StructureElement, List<int> PostHeaderIDs, List<int> ComponentIDs, List<int> SetQty)
        {
            //Split 1. WT-DETAILS; 2. WT-SETQTY; 3. WT-GROUP
            bool lSuccess = true;
            string lErrorMsg = "";
            string lSQL;
            SqlCommand lCmd;
            SqlDataReader adoRst;

            try
            {
                int lThresHold = 0;
                int lSplitInd = 0;
                decimal lAssignedWT = 0;
                decimal? lTotalWT = 0;

                var lProces = new ProcessController();
                lProces.OpenNDSConnection(ref cnNDS);
                if (cnNDS.State == ConnectionState.Open)
                {
                    #region get Tonnage Split Threshold
                    lSQL = "SELECT SplitThresHold " +
                    "FROM dbo.OESComponentConfig " +
                    "WHERE ConfigureID = 1 ";

                    lCmd = new SqlCommand(lSQL, cnNDS);
                    lCmd.CommandTimeout = 1200;
                    adoRst = lCmd.ExecuteReader();
                    if (adoRst.HasRows)
                    {
                        adoRst.Read();
                        lThresHold = (int)adoRst.GetValue(0) * 1000;
                    }
                    adoRst.Close();
                    #endregion

                    if (lThresHold > 0)
                    {
                        #region get total weight of assignment

                        var lCompSetQty = new[]
                            { new {
                                lCompID = 0,
                                lSetQty = 0
                            }
                        }.ToList();
                        lCompSetQty.RemoveAt(0);

                        for (int i = 0; i < ComponentIDs.Count; i++)
                        {
                            lCompSetQty.Add(new
                            {
                                lCompID = ComponentIDs[i],
                                lSetQty = SetQty[i]
                            });
                        }

                        var lCompDB = (from p in db.OESComponent
                                       where ComponentIDs.Contains(p.ComponentID)
                                       select new
                                       {
                                           p.ComponentID,
                                           p.TotalWeight
                                       }).ToList();

                        lTotalWT = (from p in lCompDB
                                    join q in lCompSetQty
                                    on p.ComponentID equals q.lCompID
                                    where ComponentIDs.Contains(p.ComponentID)
                                    select p.TotalWeight * q.lSetQty).Sum();

                        #endregion

                        #region get total weight of assigned, get the max  wt in all selected PostID

                        var lPostIDWhere = "";
                        for (int i = 0; i < PostHeaderIDs.Count; i++)
                        {
                            lPostIDWhere = lPostIDWhere + ", " + PostHeaderIDs[i].ToString();
                        }
                        if (lPostIDWhere != "")
                        {
                            lPostIDWhere = lPostIDWhere.Substring(1);
                        }

                        lSQL = "SELECT isNULL(MAX(WT),0) FROM " +
                        "(SELECT SUM(TotalWeight) as WT, PostHeaderID " +
                        "FROM dbo.OESComponentWBS " +
                        "WHERE PostHeaderID in " +
                        "(" + lPostIDWhere + ") " +
                        "GROUP BY PostHeaderID) as a ";

                        lCmd = new SqlCommand(lSQL, cnNDS);
                        lCmd.CommandTimeout = 1200;
                        adoRst = lCmd.ExecuteReader();
                        if (adoRst.HasRows)
                        {
                            adoRst.Read();
                            lAssignedWT = adoRst.GetValue(0) == DBNull.Value ? (decimal)0 : adoRst.GetDecimal(0);
                        }
                        adoRst.Close();

                        #endregion

                        if (lTotalWT + lAssignedWT > lThresHold)
                        {
                            lSplitInd = 1;
                        }

                    }
                    if (lSplitInd == 0)
                    {
                        #region non-split wbs
                        for (int i = 0; i < PostHeaderIDs.Count; i++)
                        {
                            var lWBS1 = "";
                            var lWBS2 = "";
                            var lWBS3 = "";
                            var lBBSNo = "";
                            var lBBSDesc = "";

                            lSQL = "SELECT vchWBS1, vchWBS2, vchWBS3, vchBBSNo, BBS_DESC " +
                            "FROM dbo.WBSElements W, dbo.BBSPostHeader P " +
                            "WHERE W.intWBSElementId = P.intWBSElementId " +
                            "AND intPostHeaderId = " + PostHeaderIDs[i].ToString() + " " +
                            "ORDER BY vchWBS1, vchWBS2, vchWBS3, vchBBSNo ";

                            lCmd = new SqlCommand(lSQL, cnNDS);
                            lCmd.CommandTimeout = 1200;
                            adoRst = lCmd.ExecuteReader();
                            if (adoRst.HasRows)
                            {
                                adoRst.Read();
                                lWBS1 = adoRst.GetValue(0) == DBNull.Value ? "" : adoRst.GetString(0).Trim();
                                lWBS2 = adoRst.GetValue(1) == DBNull.Value ? "" : adoRst.GetString(1).Trim();
                                lWBS3 = adoRst.GetValue(2) == DBNull.Value ? "" : adoRst.GetString(2).Trim();
                                lBBSNo = adoRst.GetValue(3) == DBNull.Value ? "" : adoRst.GetString(3).Trim();
                                lBBSDesc = adoRst.GetValue(4) == DBNull.Value ? "" : adoRst.GetString(4).Trim();
                            }
                            adoRst.Close();

                            if (lWBS1 != "")
                            {
                                for (int j = 0; j < ComponentIDs.Count(); j++)
                                {
                                    lSQL = "INSERT INTO dbo.OESComponentWBS " +
                                    "(PostHeaderID " +
                                    ", ComponentID " +
                                    ", SSID " +
                                    ", ComponentName " +
                                    ", Revision " +
                                    ", SplitType " +
                                    ", SplitID " +
                                    ", NoOfSet " +
                                    ", PCsPerSet " +
                                    ", WTPerSet " +
                                    ", TotalPCs " +
                                    ", TotalWeight " +
                                    ", ProductType " +
                                    ", StructureElement " +
                                    ", WBS1 " +
                                    ", WBS2 " +
                                    ", WBS3 " +
                                    ", BBSNo " +
                                    ", BBSDesc " +
                                    ", UpdateDate " +
                                    ", UpdateBy) " +
                                    "SELECT " +
                                    " " + PostHeaderIDs[i].ToString() + " " +
                                    ", ComponentID " +
                                    ", " + (j + 1).ToString() + " " +
                                    ",ComponentName " +
                                    ",Revision " +
                                    ",'NO' " +
                                    ",0 " +
                                    ", " + SetQty[j] + " " +
                                    ",TotalPCs " +
                                    ",TotalWeight " +
                                    ",TotalPCs * " + SetQty[j] + " " +
                                    ",TotalWeight * " + SetQty[j] + " " +
                                    ",ProductType " +
                                    ",StructureElement " +
                                    ", '" + lWBS1 + "' " +
                                    ", '" + lWBS2 + "' " +
                                    ", '" + lWBS3 + "' " +
                                    ", '" + lBBSNo + "' " +
                                    ", '" + lBBSDesc + "' " +
                                    ", GetDate() " +
                                    ", '" + User.Identity.GetUserName() + "' " +
                                    "FROM dbo.OESComponent " +
                                    "WHERE ComponentID = " + ComponentIDs[j].ToString() + " ";

                                    lCmd = new SqlCommand(lSQL, cnNDS);
                                    lCmd.CommandTimeout = 1200;
                                    lCmd.ExecuteNonQuery();
                                }

                            }
                        }
                        #endregion
                    }
                    else
                    {
                        var lSplitNo = 0;
                        var lSNo = 0;
                        //split by tonnage
                        var lMultiComb = new[]
                        { new {
                                lCompID = 0,
                                lSetQty = 0,
                                lPCsSet = 0,
                                lWTSet = (decimal)0,
                                lWTTotal = (decimal)0
                            }
                        }.ToList();

                        lMultiComb.RemoveAt(0);

                        #region Add in assigned components to the ComponentsID list and delete the previous assigned components for newassignment

                        var lPostIDWhere = "";
                        var lComponentIDWhere = "";
                        for (int i = 0; i < PostHeaderIDs.Count; i++)
                        {
                            lPostIDWhere = lPostIDWhere + ", " + PostHeaderIDs[i].ToString();
                        }
                        if (lPostIDWhere != "")
                        {
                            lPostIDWhere = lPostIDWhere.Substring(1);
                        }

                        if (lPostIDWhere != "")
                        {
                            lSQL = "SELECT ComponentID " +
                            "FROM dbo.OESComponentWBS " +
                            "WHERE PostHeaderID in " +
                            "(" + lPostIDWhere + ") " +
                            "GROUP BY ComponentID HAVING COUNT(*) = " + PostHeaderIDs.Count + " " +
                            "ORDER BY ComponentID DESC ";

                            lCmd = new SqlCommand(lSQL, cnNDS);
                            lCmd.CommandTimeout = 1200;
                            adoRst = lCmd.ExecuteReader();
                            if (adoRst.HasRows)
                            {
                                while (adoRst.Read())
                                {
                                    ComponentIDs.Insert(0, adoRst.GetValue(0) == DBNull.Value ? 0 : adoRst.GetInt32(0));
                                    lComponentIDWhere = lComponentIDWhere + ", " + (adoRst.GetValue(0) == DBNull.Value ? 0 : adoRst.GetInt32(0)).ToString();
                                }
                            }
                            adoRst.Close();

                            if (lComponentIDWhere != "")
                            {
                                lComponentIDWhere = lComponentIDWhere.Substring(1);
                            }
                        }

                        if (lPostIDWhere != "" && lComponentIDWhere != "")
                        {
                            lSQL = "DELETE " +
                            "FROM dbo.OESComponentWBS " +
                            "WHERE PostHeaderID in (" + lPostIDWhere + ") " +
                            "AND ComponentID in (" + lComponentIDWhere + ") ";

                            lCmd = new SqlCommand(lSQL, cnNDS);
                            lCmd.CommandTimeout = 1200;
                            lCmd.ExecuteNonQuery();
                        }
                        #endregion

                        for (int i = 0; i < ComponentIDs.Count; i++)
                        {
                            var lType = "NO";
                            #region get current component PCS and WT for identify the split type
                            var lVar = ComponentIDs[i];
                            var lCompPCs = (from p in db.OESComponent
                                            where p.ComponentID == lVar
                                            select p.TotalPCs
                                            ).Sum();

                            var lCompWT = (from p in db.OESComponent
                                           where p.ComponentID == lVar
                                           select p.TotalWeight
                                            ).Sum();

                            if (lCompPCs == null)
                            {
                                lCompPCs = 0;
                            }
                            if (lCompWT == null)
                            {
                                lCompWT = 0;
                            }
                            #endregion

                            if (lCompWT > lThresHold)
                            {
                                #region split individual component having SetQty = 1
                                //1-Split Component
                                //usp_PostingBBSPosting_Insert
                                lType = "WT-DETAILS";
                                lVar = ComponentIDs[i];
                                var lCompDet = (from p in db.OESComponent
                                                where p.ComponentID == lVar
                                                select p).ToList();
                                if (lCompDet != null && lCompDet.Count > 0)
                                {
                                    var lJobID = 0;
                                    var lDetials = (new[] { new
                                    {
                                        ItemID = 0,
                                        MeshMemberQty = 0,
                                        MeshTotalWT = (decimal)0,
                                        ProductType="",
                                        StructureElement = ""
                                    }
                                    }).ToList();
                                    lDetials.RemoveAt(0);

                                    //take items from database
                                    if (lCompDet[0].ProductType == "CUT-TO-SIZE-MESH")
                                    {
                                        lJobID = lCompDet[0].MeshJobID == null ? 0 : (int)lCompDet[0].MeshJobID;
                                        lDetials = (from p in db.CTSMESHOthersDetails
                                                    where p.CustomerCode == CustomerCode &&
                                                    p.ProjectCode == ProjectCode &&
                                                    p.JobID == lJobID
                                                    orderby p.MeshID
                                                    select new
                                                    {
                                                        ItemID = p.MeshID,
                                                        MeshMemberQty = p.MeshMemberQty == null ? 0 : (int)p.MeshMemberQty,
                                                        MeshTotalWT = p.MeshTotalWT == null ? (decimal)0 : (decimal)p.MeshTotalWT,
                                                        ProductType = ProductType,
                                                        StructureElement = StructureElement
                                                    }).ToList();
                                    }
                                    else if (lCompDet[0].ProductType == "STIRRUP-LINK-MESH")
                                    {
                                        lJobID = lCompDet[0].MeshJobID == null ? 0 : (int)lCompDet[0].MeshJobID;
                                        lDetials = (from p in db.CTSMESHBeamDetails
                                                    where p.CustomerCode == CustomerCode &&
                                                    p.ProjectCode == ProjectCode &&
                                                    p.JobID == lJobID
                                                    orderby p.MeshID
                                                    select new
                                                    {
                                                        ItemID = p.MeshID,
                                                        MeshMemberQty = p.MeshMemberQty == null ? 0 : (int)p.MeshMemberQty,
                                                        MeshTotalWT = p.MeshTotalWT == null ? (decimal)0 : (decimal)p.MeshTotalWT,
                                                        ProductType = ProductType,
                                                        StructureElement = StructureElement
                                                    }).ToList();
                                    }
                                    else if (lCompDet[0].ProductType == "COLUMN-LINK-MESH")
                                    {
                                        lJobID = lCompDet[0].MeshJobID == null ? 0 : (int)lCompDet[0].MeshJobID;
                                        lDetials = (from p in db.CTSMESHColumnDetails
                                                    where p.CustomerCode == CustomerCode &&
                                                    p.ProjectCode == ProjectCode &&
                                                    p.JobID == lJobID
                                                    orderby p.MeshID
                                                    select new
                                                    {
                                                        ItemID = p.MeshID,
                                                        MeshMemberQty = p.MeshMemberQty == null ? 0 : (int)p.MeshMemberQty,
                                                        MeshTotalWT = p.MeshTotalWT == null ? (decimal)0 : (decimal)p.MeshTotalWT,
                                                        ProductType = ProductType,
                                                        StructureElement = StructureElement
                                                    }).ToList();
                                    }
                                    else if (lCompDet[0].ProductType == "CAB")
                                    {
                                        lJobID = lCompDet[0].CABJobID == null ? 0 : (int)lCompDet[0].CABJobID;
                                    }
                                    else if (lCompDet[0].ProductType == "PRC")
                                    {
                                        lJobID = lCompDet[0].PRCJobID == null ? 0 : (int)lCompDet[0].PRCJobID;
                                    }

                                    if (lDetials.Count > 0)
                                    {
                                        // check and split indiviual item
                                        for (int j = 0; j < lDetials.Count; j++)
                                        {
                                            if (lDetials[j].MeshTotalWT > lThresHold && lDetials[j].MeshMemberQty > 1)
                                            {
                                                // Split By MeshMenberQty
                                                int lNo = (int)Math.Ceiling((double)lDetials[j].MeshTotalWT / lThresHold);
                                                int lQty = (int)Math.Ceiling((double)lDetials[j].MeshMemberQty / lNo);
                                                if (lQty == 0)
                                                {
                                                    lQty = 1;
                                                }
                                                int lTotalMeshQty = lDetials[j].MeshMemberQty;

                                                for (int k = 0; k < lNo; k++)
                                                {
                                                    int lMeshQty = lQty;
                                                    if (lTotalMeshQty < lMeshQty)
                                                    {
                                                        lMeshQty = lTotalMeshQty;
                                                    }
                                                    lTotalMeshQty = lTotalMeshQty - lMeshQty;

                                                    var lNew = new
                                                    {
                                                        ItemID = lDetials[j].ItemID,
                                                        MeshMemberQty = lMeshQty,
                                                        MeshTotalWT = (decimal)lMeshQty * lDetials[j].MeshTotalWT / lDetials[j].MeshMemberQty,
                                                        ProductType = lDetials[j].ProductType,
                                                        StructureElement = lDetials[j].StructureElement
                                                    };
                                                    lDetials.Insert(j, lNew);
                                                }
                                                lDetials.RemoveAt(j + lNo);
                                                j = j + lNo - 1;
                                            }
                                        }

                                        decimal lWeight = 0;
                                        int lPCs = 0;
                                        lSplitNo = lSplitNo + 1;

                                        for (int m = 0; m < SetQty[i]; m++)
                                        {
                                            for (int j = 0; j < lDetials.Count; j++)
                                            {
                                                if (lWeight + lDetials[j].MeshTotalWT > lThresHold)
                                                {
                                                    lSNo = lSNo + 1;
                                                    SplitInsert(lSplitNo, lSNo, PostHeaderIDs, ComponentIDs[i], 1, lPCs, lWeight, lType);

                                                    lWeight = 0;
                                                    lPCs = 0;
                                                    lSplitNo = lSplitNo + 1;
                                                }
                                                lWeight = lWeight + lDetials[j].MeshTotalWT;
                                                lPCs = lPCs + lDetials[j].MeshMemberQty;

                                                #region Insert to Split Details
                                                for (int k = 0; k < PostHeaderIDs.Count; k++)
                                                {
                                                    lSQL = "INSERT INTO dbo.OESComponentSplit " +
                                                    "(PostHeaderID " +
                                                    ", ComponentID " +
                                                    ", SplitID " +
                                                    ", JobID " +
                                                    ", ItemID " +
                                                    ", MeshMemberQty " +
                                                    ", MeshTotalWT " +
                                                    ", ProductType " +
                                                    ", StructureElement " +
                                                    ", SplitBy " +
                                                    ", SplitDate) " +
                                                    "VALUES " +
                                                    "(" + PostHeaderIDs[k] + " " +
                                                    "," + ComponentIDs[i] + " " +
                                                    "," + lSplitNo + " " +
                                                    "," + lJobID + " " +
                                                    "," + lDetials[j].ItemID + " " +
                                                    "," + lDetials[j].MeshMemberQty + " " +
                                                    "," + lDetials[j].MeshTotalWT + " " +
                                                    ",'" + lDetials[j].ProductType + "' " +
                                                    ",'" + lDetials[j].StructureElement + "' " +
                                                    ",'" + User.Identity.GetUserName() + "' " +
                                                    ",getDate() ) ";

                                                    lCmd = new SqlCommand(lSQL, cnNDS);
                                                    lCmd.CommandTimeout = 1200;
                                                    lCmd.ExecuteNonQuery();
                                                }
                                                #endregion

                                            }
                                            if (lWeight > 0)
                                            {
                                                lSNo = lSNo + 1;
                                                SplitInsert(lSplitNo, lSNo, PostHeaderIDs, ComponentIDs[i], 1, lPCs, lWeight, lType);
                                            }
                                        }
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                if (lCompWT * SetQty[i] > lThresHold)
                                {
                                    #region Total Sets exceed tonnage threshold, just split SetQty
                                    //2-Split SetQty
                                    lType = "WT-SETQTY";

                                    int lSets = (int)Math.Floor((double)(lThresHold / lCompWT));
                                    int lSplitCT = (int)Math.Ceiling((double)SetQty[i] / lSets);

                                    int lLeftSet = SetQty[i];
                                    for (int j = 0; j < lSplitCT; j++)
                                    {
                                        lSplitNo = lSplitNo + 1;
                                        if (lLeftSet > lSets)
                                        {
                                            lSNo = lSNo + 1;
                                            SplitInsert(lSplitNo, lSNo, PostHeaderIDs, ComponentIDs[i], lSets, (int)lCompPCs, (decimal)lCompWT, lType);
                                            lLeftSet = lLeftSet - lSets;
                                        }
                                        else
                                        {
                                            lSNo = lSNo + 1;
                                            SplitInsert(lSplitNo, lSNo, PostHeaderIDs, ComponentIDs[i], lLeftSet, (int)lCompPCs, (decimal)lCompWT, lType);
                                            break;
                                        }
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region add to multiple components combination split
                                    //3-Split Combine multiple components
                                    lType = "WT-GROUP";
                                    lMultiComb.Add(new
                                    {
                                        lCompID = ComponentIDs[i],
                                        lSetQty = SetQty[i],
                                        lPCsSet = (int)lCompPCs,
                                        lWTSet = (decimal)lCompWT,
                                        lWTTotal = SetQty[i] * (decimal)lCompWT
                                    });
                                    #endregion
                                }
                            }
                        }
                        if (lMultiComb.Count > 0)
                        {
                            #region multiple components combination split logic
                            var lType = "WT-GROUP";
                            //Sorting from big to small
                            var lMultiComb1 = (from p in lMultiComb
                                               orderby p.lWTTotal descending
                                               select p).ToList();
                            //lMultiComb.Sort((a,b) => a.lWTTotal.CompareTo(b.lWTTotal));

                            for (int i = 0; i < lMultiComb1.Count; i++)
                            {
                                if (i >= lMultiComb1.Count)
                                {
                                    //no more
                                    break;
                                }
                                if (i == lMultiComb1.Count - 1)
                                {
                                    // save last one
                                    lSplitNo = lSplitNo + 1;
                                    lSNo = lSNo + 1;
                                    SplitInsert(lSplitNo, 1, PostHeaderIDs, lMultiComb1[i].lCompID, lMultiComb1[i].lSetQty, lMultiComb1[i].lPCsSet, lMultiComb1[i].lWTSet, lType);
                                    break;
                                }
                                var lTotal = lMultiComb1[i].lWTTotal;
                                for (int j = lMultiComb1.Count - 1; j > i; j--)
                                {
                                    lTotal = lTotal + lMultiComb1[j].lWTTotal;
                                    if (lTotal > lThresHold)
                                    {
                                        //save i and j+1 to last
                                        lSplitNo = lSplitNo + 1;
                                        lSNo = lSNo + 1;
                                        //save i
                                        SplitInsert(lSplitNo, lSNo, PostHeaderIDs, lMultiComb1[i].lCompID, lMultiComb1[i].lSetQty, lMultiComb1[i].lPCsSet, lMultiComb1[i].lWTSet, lType);

                                        // save j+1 to last
                                        for (int k = j + 1; k < lMultiComb1.Count; k++)
                                        {
                                            lSNo = lSNo + 1;
                                            SplitInsert(lSplitNo, lSNo, PostHeaderIDs, lMultiComb1[k].lCompID, lMultiComb1[k].lSetQty, lMultiComb1[k].lPCsSet, lMultiComb1[k].lWTSet, lType);
                                        }
                                        // remove from trail
                                        for (int k = lMultiComb1.Count - 1; k > j; k--)
                                        {
                                            lMultiComb1.RemoveAt(k);
                                        }
                                        lTotal = 0;
                                        break;
                                    }
                                }
                                if (lTotal > 0)
                                {
                                    // not found the exceed the thredhold
                                    // save the rest
                                    // save i to last

                                    lSplitNo = lSplitNo + 1;
                                    for (int k = i; k < lMultiComb1.Count; k++)
                                    {
                                        lSNo = lSNo + 1;
                                        SplitInsert(lSplitNo, lSNo, PostHeaderIDs, lMultiComb1[k].lCompID, lMultiComb1[k].lSetQty, lMultiComb1[k].lPCsSet, lMultiComb1[k].lWTSet, lType);
                                    }
                                    // remove from trail
                                    for (int k = lMultiComb1.Count - 1; k > i; k--)
                                    {
                                        lMultiComb1.RemoveAt(k);
                                    }
                                    lTotal = 0;
                                    break;
                                }
                            }
                            #endregion
                        }
                    }

                    lProces.CloseNDSConnection(ref cnNDS);
                }
            }
            catch (Exception ex)
            {
                lSuccess = false;
                lErrorMsg = ex.Message;
            }

            lCmd = null;
            adoRst = null;

            return Ok(new { success = lSuccess, responseText = lErrorMsg });
        }

        string SplitInsert(int SplitNo, int SNo, List<int> PostHeaderIDs, int ComponentID, int SetQty, int PCsPerSet, decimal WTPerSet, string SplitType)
        {
            string lErrorMsg = "";
            string lSQL;
            SqlCommand lCmd;
            SqlDataReader adoRst;

            for (int i = 0; i < PostHeaderIDs.Count; i++)
            {
                var lWBS1 = "";
                var lWBS2 = "";
                var lWBS3 = "";
                var lBBSNo = "";
                var lBBSDesc = "";

                lSQL = "SELECT vchWBS1, vchWBS2, vchWBS3, vchBBSNo, BBS_DESC " +
                "FROM dbo.WBSElements W, dbo.BBSPostHeader P " +
                "WHERE W.intWBSElementId = P.intWBSElementId " +
                "AND intPostHeaderId = " + PostHeaderIDs[i].ToString() + " " +
                "ORDER BY vchWBS1, vchWBS2, vchWBS3, vchBBSNo ";

                lCmd = new SqlCommand(lSQL, cnNDS);
                lCmd.CommandTimeout = 1200;
                adoRst = lCmd.ExecuteReader();
                if (adoRst.HasRows)
                {
                    adoRst.Read();
                    lWBS1 = adoRst.GetValue(0) == DBNull.Value ? "" : adoRst.GetString(0).Trim();
                    lWBS2 = adoRst.GetValue(1) == DBNull.Value ? "" : adoRst.GetString(1).Trim();
                    lWBS3 = adoRst.GetValue(2) == DBNull.Value ? "" : adoRst.GetString(2).Trim();
                    lBBSNo = adoRst.GetValue(3) == DBNull.Value ? "" : adoRst.GetString(3).Trim();
                    lBBSDesc = adoRst.GetValue(4) == DBNull.Value ? "" : adoRst.GetString(4).Trim();
                }
                adoRst.Close();

                if (lWBS1 != "")
                {
                    lSQL = "INSERT INTO dbo.OESComponentWBS " +
                    "(PostHeaderID " +
                    ", ComponentID " +
                    ", SplitID " +
                    ", SSID " +
                    ", ComponentName " +
                    ", Revision " +
                    ", SplitType " +
                    ", NoOfSet " +
                    ", PCsPerSet " +
                    ", WTPerSet " +
                    ", TotalPCs " +
                    ", TotalWeight " +
                    ", ProductType " +
                    ", StructureElement " +
                    ", WBS1 " +
                    ", WBS2 " +
                    ", WBS3 " +
                    ", BBSNo " +
                    ", BBSDesc " +
                    ", UpdateDate " +
                    ", UpdateBy) " +
                    "SELECT " +
                    " " + PostHeaderIDs[i].ToString() + " " +
                    ", ComponentID " +
                    "," + SplitNo + " " +
                    ", " + (SNo).ToString() + " " +
                    ",ComponentName " +
                    ",Revision " +
                    ",'" + SplitType + "' " +
                    ", " + SetQty + " " +
                    ", " + PCsPerSet + " " +
                    ", " + WTPerSet.ToString("F3") + " " +
                    ", " + PCsPerSet * SetQty + " " +
                    ", " + ((double)WTPerSet * SetQty).ToString("F3") + " " +
                    ",ProductType " +
                    ",StructureElement " +
                    ", '" + lWBS1 + "' " +
                    ", '" + lWBS2 + "' " +
                    ", '" + lWBS3 + "' " +
                    ", '" + lBBSNo + "' " +
                    ", '" + lBBSDesc + "' " +
                    ", GetDate() " +
                    ", '" + User.Identity.GetUserName() + "' " +
                    "FROM dbo.OESComponent " +
                    "WHERE ComponentID = " + ComponentID.ToString() + " ";

                    lCmd = new SqlCommand(lSQL, cnNDS);
                    lCmd.CommandTimeout = 1200;
                    lCmd.ExecuteNonQuery();
                }
            }
            return lErrorMsg;
        }

        [HttpPost]
        public ActionResult DeleteComponent(string CustomerCode, string ProjectCode, string ProductType, string StructureElement, List<int> PostHeaderIDs, List<int> ComponentIDs)
        {
            bool lSuccess = true;
            string lErrorMsg = "";
            string lSQL;
            SqlCommand lCmd;
            SqlDataReader adoRst;

            try
            {
                var lProces = new ProcessController();
                lProces.OpenNDSConnection(ref cnNDS);
                if (cnNDS.State == ConnectionState.Open)
                {
                    for (int i = 0; i < PostHeaderIDs.Count; i++)
                    {
                        if (PostHeaderIDs[i] > 0)
                        {
                            for (int j = 0; j < ComponentIDs.Count; j++)
                            {
                                if (ComponentIDs[j] > 0)
                                {
                                    lSQL = "DELETE FROM dbo.OESComponentWBS " +
                                    "WHERE PostHeaderID = " + PostHeaderIDs[i].ToString() + " " +
                                    "AND ComponentID = " + ComponentIDs[j].ToString() + " ";

                                    lCmd = new SqlCommand(lSQL, cnNDS);
                                    lCmd.CommandTimeout = 1200;
                                    lCmd.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                lSuccess = false;
                lErrorMsg = ex.Message;
            }
            return Ok(new { success = lSuccess, responseText = lErrorMsg });
        }

        [HttpPost]
        public ActionResult getAssignedComponentList(List<int> PostHeaderIDs, List<string> BBSNos, int PostedInd)
        {
            string lSQL;
            SqlCommand lCmd;
            SqlDataReader adoRst;
            var lSuccess = true;
            var lErrorMsg = "";
            var lReturn = new List<ComponentWBSModel>();

            if ((PostHeaderIDs != null && PostHeaderIDs.Count > 0) || (BBSNos != null && BBSNos.Count > 0))
            {
                try
                {
                    var lProces = new ProcessController();
                    lProces.OpenNDSConnection(ref cnNDS);
                    if (cnNDS.State == ConnectionState.Open)
                    {
                        if (PostedInd == 0)
                        {
                            var lPostID = "";
                            var lCT = 0;
                            for (int i = 0; i < PostHeaderIDs.Count; i++)
                            {
                                if (PostHeaderIDs[i] > 0)
                                {
                                    lPostID = lPostID + "," + PostHeaderIDs[i].ToString();
                                    lCT = lCT + 1;
                                }
                            }
                            if (lPostID.Length > 0)
                            {
                                lPostID = lPostID.Substring(1);
                            }
                            lSQL = "SELECT ComponentID " +
                            ", SSID " +
                            ", ComponentName " +
                            ", Revision " +
                            ", SplitType " +
                            ", SplitID " +
                            ", NoOfSet " +
                            ", PCsPerSet " +
                            ", WTPerSet " +
                            ", TotalPCs " +
                            ", TotalWeight " +
                            ", ProductType " +
                            ", StructureElement" +
                            ", MAX(PostHeaderID) " +
                            ", MAX(WBS1) " +
                            ", MAX(WBS2) " +
                            ", MAX(WBS3) " +
                            ", MIN(BBSNo) " +
                            ", MIN(BBSDesc) " +
                            ", MAX(UpdateDate) " +
                            ", MAX(UpdateBy) " +
                            "FROM dbo.OESComponentWBS " +
                            "WHERE PostHeaderID in (" + lPostID + ") " +
                            "GROUP BY " +
                            "ComponentID " +
                            ",SSID " +
                            ",ComponentName " +
                            ",Revision " +
                            ",SplitType " +
                            ",SplitID " +
                            ",NoOfSet " +
                            ",PCsPerSet " +
                            ",WTPerSet " +
                            ",TotalPCs " +
                            ",TotalWeight " +
                            ",ProductType " +
                            ",StructureElement " +
                            "HAVING COUNT(*) >= " + lCT.ToString() + " " +
                            "ORDER BY SplitID, SSID ";
                        }
                        else
                        {
                            var lBBSCond1 = "";
                            var lBBSCond2 = "";
                            var lCT = 0;
                            for (int i = 0; i < BBSNos.Count; i++)
                            {
                                if (BBSNos[i] != null)
                                {
                                    lBBSCond1 = lBBSCond1 + "OR (BBSNo + '-S' + CAST(SplitID AS varchar) = '" + BBSNos[i] + "') ";
                                    lBBSCond2 = lBBSCond2 + "OR (BBSNo = '" + BBSNos[i] + "') ";
                                    lCT = lCT + 1;
                                }
                            }

                            var lBBSCond = "1=1";
                            if (lBBSCond1.Length > 0)
                            {
                                lBBSCond1 = lBBSCond1.Substring(2);
                                lBBSCond2 = lBBSCond2.Substring(2);
                                lBBSCond = "(SplitType <> 'NO' AND SplitID > 1 AND (" + lBBSCond1 + ")) ";
                                lBBSCond = lBBSCond + "OR ((SplitType = 'NO' OR SplitID = 1) AND (" + lBBSCond2 + ")) ";
                            }

                            lSQL = "SELECT ComponentID " +
                            ", SSID " +
                            ", ComponentName " +
                            ", Revision " +
                            ", SplitType " +
                            ", SplitID " +
                            ", NoOfSet " +
                            ", PCsPerSet " +
                            ", WTPerSet " +
                            ", TotalPCs " +
                            ", TotalWeight " +
                            ", ProductType " +
                            ", StructureElement" +
                            ", MAX(PostHeaderID) " +
                            ", MAX(WBS1) " +
                            ", MAX(WBS2) " +
                            ", MAX(WBS3) " +
                            ", MIN(BBSNo) " +
                            ", MIN(BBSDesc) " +
                            ", MAX(UpdateDate) " +
                            ", MAX(UpdateBy) " +
                            "FROM dbo.OESComponentWBS " +
                            "WHERE " + lBBSCond + " " +
                            "GROUP BY " +
                            "ComponentID " +
                            ",SSID " +
                            ",ComponentName " +
                            ",Revision " +
                            ",SplitType " +
                            ",SplitID " +
                            ",NoOfSet " +
                            ",PCsPerSet " +
                            ",WTPerSet " +
                            ",TotalPCs " +
                            ",TotalWeight " +
                            ",ProductType " +
                            ",StructureElement " +
                            "HAVING COUNT(*) >= " + lCT.ToString() + " " +
                            "ORDER BY SplitID, SSID ";
                        }

                        lCmd = new SqlCommand(lSQL, cnNDS);
                        lCmd.CommandTimeout = 1200;
                        adoRst = lCmd.ExecuteReader();
                        if (adoRst.HasRows)
                        {
                            while (adoRst.Read())
                            {
                                lReturn.Add(new ComponentWBSModel
                                {
                                    ComponentID = adoRst.GetValue(0) == DBNull.Value ? 0 : adoRst.GetInt32(0),
                                    SSID = adoRst.GetValue(1) == DBNull.Value ? 0 : adoRst.GetInt32(1),
                                    ComponentName = adoRst.GetValue(2) == DBNull.Value ? "" : adoRst.GetString(2).Trim(),
                                    Revision = adoRst.GetValue(3) == DBNull.Value ? 0 : adoRst.GetInt32(3),
                                    SplitType = adoRst.GetValue(4) == DBNull.Value ? "" : adoRst.GetString(4),
                                    SplitID = adoRst.GetValue(5) == DBNull.Value ? 0 : adoRst.GetInt32(5),
                                    NoOfSet = adoRst.GetValue(6) == DBNull.Value ? 0 : adoRst.GetInt32(6),
                                    PCsPerSet = adoRst.GetValue(7) == DBNull.Value ? 0 : adoRst.GetInt32(7),
                                    WTPerSet = adoRst.GetValue(8) == DBNull.Value ? (decimal)0 : adoRst.GetDecimal(8),
                                    TotalPCs = adoRst.GetValue(9) == DBNull.Value ? 0 : adoRst.GetInt32(9),
                                    TotalWeight = adoRst.GetValue(10) == DBNull.Value ? (decimal)0 : adoRst.GetDecimal(10),
                                    ProductType = adoRst.GetValue(11) == DBNull.Value ? "" : adoRst.GetString(11).Trim(),
                                    StructureElement = adoRst.GetValue(12) == DBNull.Value ? "" : adoRst.GetString(12).Trim(),
                                    PostHeaderID = adoRst.GetValue(13) == DBNull.Value ? 0 : adoRst.GetInt32(13),
                                    WBS1 = adoRst.GetValue(14) == DBNull.Value ? "" : adoRst.GetString(14).Trim(),
                                    WBS2 = adoRst.GetValue(15) == DBNull.Value ? "" : adoRst.GetString(15).Trim(),
                                    WBS3 = adoRst.GetValue(16) == DBNull.Value ? "" : adoRst.GetString(16).Trim(),
                                    BBSNo = adoRst.GetValue(17) == DBNull.Value ? "" : adoRst.GetString(17).Trim(),
                                    BBSDesc = adoRst.GetValue(18) == DBNull.Value ? "" : adoRst.GetString(18).Trim(),
                                    UpdateDate = adoRst.GetDateTime(19),
                                    UpdateBy = adoRst.GetValue(20) == DBNull.Value ? "" : adoRst.GetString(20)
                                });
                            }
                        }
                        adoRst.Close();

                        lProces.CloseNDSConnection(ref cnNDS);
                    }
                }
                catch (Exception ex)
                {
                    lSuccess = false;
                    lErrorMsg = ex.Message;
                }
            }
            return Ok(new { success = lSuccess, responseText = lErrorMsg, content = lReturn });
        }

        public ComponentModelDateFormat CopyPropertyValues(object source, ComponentModelDateFormat destination)
        {
            var destProperties = destination.GetType().GetProperties();

            foreach (var sourceProperty in source.GetType().GetProperties())
            {
                foreach (var destProperty in destProperties)
                {
                    if (destProperty.Name == sourceProperty.Name &&
                destProperty.PropertyType.IsAssignableFrom(sourceProperty.PropertyType))
                    {
                        destProperty.SetValue(destination, sourceProperty.GetValue(
                            source, new object[] { }), new object[] { });

                        break;
                    }
                }
            }

            return destination;
        }

        [HttpPost]
        public ActionResult getWBSList(string CustomerCode, string ProjectCode, string ProductType, string StructureElement, bool ReleasedWBS)
        {
            string lSQL;
            SqlCommand lCmd;
            SqlDataReader adoRst;
            string lErrorMsg = "";
            bool lSuccess = true;

            var lReturn = new[] { new {
                PostHeaderId = 0,
                StructureElement = "",
                ProductType = "",
                PostedQty = 0,
                PostedWT = (decimal)0,
                PostedSet = 0,
                WBSStatus = "",
                WBS1 = "",
                WBS2 = "",
                WBS3 = "",
                BBSNo = "",
                BBSDesc = "",
                SOR = "",
                RequiredDate = "",
                AssignedPCs = 0,
                AssignedWT = (decimal)0,
                AssignedDate = "",
                PostedDate = "",
                ReleasedDate = "",
                AssignedBy = "",
                PostedBy = "",
                ReleasedBy = ""
            } }.ToList();

            lReturn.RemoveAt(0);

            if (ProductType == "MESH" ||
            ProductType == "CUT-TO-SIZE-MESH" ||
            ProductType == "STIRRUP-LINK-MESH" ||
            ProductType == "COLUMN-LINK-MESH")
            {
                ProductType = "MSH";
            }

            try
            {
                var lProces = new ProcessController();
                lProces.OpenNDSConnection(ref cnNDS);
                if (cnNDS.State == ConnectionState.Open)
                {
                    if (ReleasedWBS == true)
                    {
                        lSQL = "SELECT H.intPostHeaderId, S.vchStructureElementType, T.vchProductType, " +
                        "isNULl(H.intPostedQty, 0) + isNULL(intPostedCappingQty, 0) + isNULL(intPostedCLinkQty, 0), " +
                        "isNULL(numPostedWeight, 0) + isNULL(numPostedCappingWeight, 0) + isNULL(numPostedClinkWeight, 0), " +
                        "(SELECT isNULL(SUM(tntGroupQty),0) FROM dbo.PostGroupMarkingDetails where intPostHeaderId = H.intPostHeaderId), " +
                        "CASE WHEN (SELECT vchStatus FROM dbo.NDSStatusMaster M1, BBSReleaseDetails R1 " +
                        "WHERE M1.tntStatusId = R1.tntStatusId AND R1.intPostHeaderid = H.intPostHeaderId) is Null THEN " +
                        "(SELECT vchStatus FROM dbo.NDSStatusMaster WHERE tntStatusId = H.tntStatusId) " +
                        "ELSE (SELECT vchStatus FROM dbo.NDSStatusMaster M1, BBSReleaseDetails R1 " +
                        "WHERE M1.tntStatusId = R1.tntStatusId " +
                        "AND R1.intPostHeaderid = H.intPostHeaderId) END as WBSStatus, " +
                        "W.vchWBS1, W.vchWBS2, W.vchWBS3, H.vchBBSNo, H.BBS_DESC, " +
                        "(SELECT MAX(D1.ORD_REQ_NO) " +
                        "FROM dbo.SAP_REQUEST_DETAILS D1, dbo.SAP_REQUEST_HDR H1 " +
                        //"", dbo.ContractMaster C1 " +
                        "WHERE D1.ORD_REQ_NO = H1.ORD_REQ_NO " +
                        "AND H1.STATUS <> 'X' " +
                        //"AND H1.CONTR_NO = C1.vchContractNumber " +
                        "AND H1.PROJ_ID = P.vchProjectCode " +
                        "AND D1.STRUC_ELEM_TYPE = S.vchStructureElementType " +
                        "AND D1.PROD_TYPE = T.vchProductType " +
                        "AND D1.WBS1 = W.vchWBS1 " +
                        "AND D1.WBS2 = W.vchWBS2 " +
                        "AND D1.WBS3 = W.vchWBS3 " +
                        "AND D1.BBS_NO = H.vchBBSNo) as SOR, " +
                        "(SELECT MAX(D1.REQ_DELI_DATE) " +
                        "FROM dbo.SAP_REQUEST_DETAILS D1, dbo.SAP_REQUEST_HDR H1 " +
                        //", dbo.ContractMaster C1 " +
                        "WHERE D1.ORD_REQ_NO = H1.ORD_REQ_NO " +
                        "AND H1.STATUS <> 'X' " +
                        //"AND H1.CONTR_NO = C1.vchContractNumber " +
                        "AND H1.PROJ_ID = P.vchProjectCode " +
                        "AND D1.STRUC_ELEM_TYPE = S.vchStructureElementType " +
                        "AND D1.PROD_TYPE = T.vchProductType " +
                        "AND D1.WBS1 = W.vchWBS1 " +
                        "AND D1.WBS2 = W.vchWBS2 " +
                        "AND D1.WBS3 = W.vchWBS3 " +
                        "AND D1.BBS_NO = H.vchBBSNo) as ReqDate, " +
                        "CASE WHEN H.tntStatusId = 3 THEN " +
                        "(SELECT SUM(TotalPCs) FROM dbo.OESComponentWBS " +
                        "WHERE ((SplitType = 'NO' OR SplitID = 1) AND PostHeaderID = H.intPostHeaderId) OR ( SplitID > 1 AND SplitPostHeaderID = H.intPostHeaderId)) " +
                        "ELSE (SELECT SUM(TotalPCs) FROM dbo.OESComponentWBS " +
                        "WHERE PostHeaderID = H.intPostHeaderId) END as AssignedPCs, " +
                        "CASE WHEN H.tntStatusId = 3 THEN " +
                        "(SELECT SUM(TotalWeight) FROM dbo.OESComponentWBS " +
                        "WHERE ((SplitType = 'NO' OR SplitID = 1) AND PostHeaderID = H.intPostHeaderId) OR ( SplitID > 1 AND SplitPostHeaderID = H.intPostHeaderId)) " +
                        "ELSE (SELECT SUM(TotalWeight) FROM dbo.OESComponentWBS " +
                        "WHERE PostHeaderID = H.intPostHeaderId) END / 1000 as AssignedWT, " +
                        "(SELECT MAX(UpdateDate) FROM dbo.OESComponentWBS " +
                        "WHERE PostHeaderID = H.intPostHeaderId) as AssignedDate, " +
                        "H.datPostedDate, " +
                        "(SELECT MAX(datReleasedDate) FROM dbo.BBSReleaseDetails " +
                        "WHERE intPostHeaderid = H.intPostHeaderid) as ReleasedDate, " +
                        "(SELECT MAX(UpdateBy) FROM dbo.OESComponentWBS " +
                        "WHERE PostHeaderID = H.intPostHeaderid AND UpdateDate = (SELECT MAX(UpdateDate) " +
                        "FROM dbo.OESComponentWBS WHERE PostHeaderID = H.intPostHeaderid )) as AssignedBy, " +
                        "(SELECT vchUserName FROM dbo.NDSUsers WHERE intUserId = H.intPostedBy) as PostedBy, " +
                        "(SELECT vchUserName FROM dbo.NDSUsers,dbo.BBSReleaseDetails " +
                        "WHERE intPostHeaderid = H.intPostHeaderid AND intUserId = intReleaseBy) as ReleasedBy " +
                        "FROM dbo.BBSPostHeader H, dbo.WBSElements W, dbo.ProjectMaster P, dbo.StructureElementMaster S, dbo.ProductTypeMaster T " +
                        "WHERE P.intProjectId = H.intProjectId " +
                        "AND H.intWBSElementId = W.intWBSElementId " +
                        "AND S.intStructureElementTypeId = H.intStructureElementTypeId " +
                        "AND T.sitProductTypeID = H.sitProductTypeId " +
                        "AND vchProjectCode = '" + ProjectCode + "' " +
                        //"AND H.tntStatusId = 3 " +
                        //"AND NOT EXISTS (SELECT intReleaseId FROM dbo.BBSReleaseDetails WHERE intPostHeaderid = H.intPostHeaderid AND tntStatusId = 14) " +
                        "AND S.vchStructureElementType = '" + StructureElement + "' " +
                        "AND T.vchProductType = '" + ProductType + "' " +
                        "AND W.tntStatusId = 1 " +
                        "GROUP BY H.intPostHeaderId, S.vchStructureElementType, T.vchProductType, " +
                        "H.intPostedQty, intPostedCappingQty, intPostedCLinkQty, " +
                        "numPostedWeight, numPostedCappingWeight, numPostedClinkWeight, " +
                        "H.intPostedBy, " +
                        "H.datPostedDate, " +
                        "H.tntStatusId, " +
                        "W.vchWBS1, " +
                        "W.vchWBS2, " +
                        "W.vchWBS3, H.vchBBSNo, H.BBS_DESC, P.vchProjectCode " +
                        "ORDER BY W.vchWBS1, " +
                        "(case when PATINDEX('B[0-9]%', vchWBS2) > 0 then cast(left(substring(vchWBS2, (PATINDEX('%[0-9]%',vchWBS2)),len(vchWBS2)), " +
                        "PATINDEX('%[^0-9]%', substring(vchWBS2 + 'z', (PATINDEX('%[0-9]%', vchWBS2)), " +
                        "len(vchWBS2)) + 'z') ) as int)    " +
                        "else (case when PATINDEX('FDN[0-9]%', vchWBS2) > 0 then cast(left(substring(vchWBS2, (PATINDEX('%[0-9]%',vchWBS2)),len(vchWBS2)), " +
                        "PATINDEX('%[^0-9]%', substring(vchWBS2, (PATINDEX('%[0-9]%', vchWBS2)), " +
                        "len(vchWBS2)) + 'z') ) as int) else  " +
                        "case when(PATINDEX('B%', vchWBS2) > 0 OR PATINDEX('FDN%', vchWBS2) > 0) then 98  else 99 end " +
                        "end) end), " +
                        " " +
                        "(case when PATINDEX('[^0-9]%',vchWBS2) > 0 then vchWBS2 " +
                        "else '' end), " +
                        " " +
                        "(CASE WHEN PATINDEX('%[0-9]%',vchWBS2) > 0 THEN cast(left(substring(vchWBS2, (PATINDEX('%[0-9]%',vchWBS2)),len(vchWBS2)), " +
                        "PATINDEX('%[^0-9]%', substring(vchWBS2, (PATINDEX('%[0-9]%', vchWBS2) + 1), " +
                        "len(vchWBS2)) + 'z') ) as int) " +
                        "ELSE " +
                        "cast(left(substring(vchWBS2, (PATINDEX('%[0-9]%', vchWBS2)), len(vchWBS2)), " +
                        "PATINDEX('%[^0-9]%', substring(vchWBS2, (PATINDEX('%[0-9]%', vchWBS2)), " +
                        "len(vchWBS2)) + 'z') - 1) as int) " +
                        "END),  " +
                        "W.vchWBS3, " +
                        "S.vchStructureElementType, T.vchProductType, H.vchBBSNo ";
                    }
                    else
                    {
                        lSQL = "SELECT H.intPostHeaderId, S.vchStructureElementType, T.vchProductType, " +
                        "isNULl(H.intPostedQty, 0) + isNULL(intPostedCappingQty, 0) + isNULL(intPostedCLinkQty, 0), " +
                        "isNULL(numPostedWeight, 0) + isNULL(numPostedCappingWeight, 0) + isNULL(numPostedClinkWeight, 0), " +
                        "(SELECT isNULL(SUM(tntGroupQty),0) FROM dbo.PostGroupMarkingDetails where intPostHeaderId = H.intPostHeaderId), " +
                        "CASE WHEN (SELECT vchStatus FROM dbo.NDSStatusMaster M1, BBSReleaseDetails R1 " +
                        "WHERE M1.tntStatusId = R1.tntStatusId AND R1.intPostHeaderid = H.intPostHeaderId) is Null THEN " +
                        "(SELECT vchStatus FROM dbo.NDSStatusMaster WHERE tntStatusId = H.tntStatusId) " +
                        "ELSE (SELECT vchStatus FROM dbo.NDSStatusMaster M1, BBSReleaseDetails R1 " +
                        "WHERE M1.tntStatusId = R1.tntStatusId " +
                        "AND R1.intPostHeaderid = H.intPostHeaderId) END as WBSStatus, " +
                        "W.vchWBS1, W.vchWBS2, W.vchWBS3, H.vchBBSNo, H.BBS_DESC, " +
                        "(SELECT MAX(D1.ORD_REQ_NO) " +
                        "FROM dbo.SAP_REQUEST_DETAILS D1, dbo.SAP_REQUEST_HDR H1 " +
                        //"", dbo.ContractMaster C1 " +
                        "WHERE D1.ORD_REQ_NO = H1.ORD_REQ_NO " +
                        "AND H1.STATUS <> 'X' " +
                        //"AND H1.CONTR_NO = C1.vchContractNumber " +
                        "AND H1.PROJ_ID = P.vchProjectCode " +
                        "AND D1.STRUC_ELEM_TYPE = S.vchStructureElementType " +
                        "AND D1.PROD_TYPE = T.vchProductType " +
                        "AND D1.WBS1 = W.vchWBS1 " +
                        "AND D1.WBS2 = W.vchWBS2 " +
                        "AND D1.WBS3 = W.vchWBS3 " +
                        "AND D1.BBS_NO = H.vchBBSNo) as SOR, " +
                        "(SELECT MAX(D1.REQ_DELI_DATE) " +
                        "FROM dbo.SAP_REQUEST_DETAILS D1, dbo.SAP_REQUEST_HDR H1 " +
                        //", dbo.ContractMaster C1 " +
                        "WHERE D1.ORD_REQ_NO = H1.ORD_REQ_NO " +
                        "AND H1.STATUS <> 'X' " +
                        //"AND H1.CONTR_NO = C1.vchContractNumber " +
                        "AND H1.PROJ_ID = P.vchProjectCode " +
                        "AND D1.STRUC_ELEM_TYPE = S.vchStructureElementType " +
                        "AND D1.PROD_TYPE = T.vchProductType " +
                        "AND D1.WBS1 = W.vchWBS1 " +
                        "AND D1.WBS2 = W.vchWBS2 " +
                        "AND D1.WBS3 = W.vchWBS3 " +
                        "AND D1.BBS_NO = H.vchBBSNo) as ReqDate, " +
                        "CASE WHEN H.tntStatusId = 3 THEN " +
                        "(SELECT SUM(TotalPCs) FROM dbo.OESComponentWBS " +
                        "WHERE ((SplitType = 'NO' OR SplitID = 1) AND PostHeaderID = H.intPostHeaderId) OR ( SplitID > 1 AND SplitPostHeaderID = H.intPostHeaderId)) " +
                        "ELSE (SELECT SUM(TotalPCs) FROM dbo.OESComponentWBS " +
                        "WHERE PostHeaderID = H.intPostHeaderId) END as AssignedPCs, " +
                        "CASE WHEN H.tntStatusId = 3 THEN " +
                        "(SELECT SUM(TotalWeight) FROM dbo.OESComponentWBS " +
                        "WHERE ((SplitType = 'NO' OR SplitID = 1) AND PostHeaderID = H.intPostHeaderId) OR ( SplitID > 1 AND SplitPostHeaderID = H.intPostHeaderId)) " +
                        "ELSE (SELECT SUM(TotalWeight) FROM dbo.OESComponentWBS " +
                        "WHERE PostHeaderID = H.intPostHeaderId) END / 1000 as AssignedWT, " +
                        "(SELECT MAX(UpdateDate) FROM dbo.OESComponentWBS " +
                        "WHERE PostHeaderID = H.intPostHeaderId) as AssignedDate, " +
                        "H.datPostedDate, " +
                        "(SELECT MAX(datReleasedDate) FROM dbo.BBSReleaseDetails " +
                        "WHERE intPostHeaderid = H.intPostHeaderid) as ReleasedDate, " +
                        "(SELECT MAX(UpdateBy) FROM dbo.OESComponentWBS " +
                        "WHERE PostHeaderID = H.intPostHeaderid AND UpdateDate = (SELECT MAX(UpdateDate) " +
                        "FROM dbo.OESComponentWBS WHERE PostHeaderID = H.intPostHeaderid )) as AssignedBy, " +
                        "(SELECT vchUserName FROM dbo.NDSUsers WHERE intUserId = H.intPostedBy) as PostedBy, " +
                        "(SELECT vchUserName FROM dbo.NDSUsers,dbo.BBSReleaseDetails " +
                        "WHERE intPostHeaderid = H.intPostHeaderid AND intUserId = intReleaseBy) as ReleasedBy " +
                        "FROM dbo.BBSPostHeader H, dbo.WBSElements W, dbo.ProjectMaster P, dbo.StructureElementMaster S, dbo.ProductTypeMaster T " +
                        "WHERE P.intProjectId = H.intProjectId " +
                        "AND H.intWBSElementId = W.intWBSElementId " +
                        "AND S.intStructureElementTypeId = H.intStructureElementTypeId " +
                        "AND T.sitProductTypeID = H.sitProductTypeId " +
                        "AND vchProjectCode = '" + ProjectCode + "' " +
                        //"AND H.tntStatusId = 3 " +
                        "AND NOT EXISTS (SELECT intReleaseId FROM dbo.BBSReleaseDetails WHERE intPostHeaderid = H.intPostHeaderid AND (tntStatusId = 12 OR tntStatusId = 14)) " +
                        "AND S.vchStructureElementType = '" + StructureElement + "' " +
                        "AND T.vchProductType = '" + ProductType + "' " +
                        "AND W.tntStatusId = 1 " +
                        "GROUP BY H.intPostHeaderId, S.vchStructureElementType, T.vchProductType, " +
                        "H.intPostedQty, intPostedCappingQty, intPostedCLinkQty, " +
                        "numPostedWeight, numPostedCappingWeight, numPostedClinkWeight, " +
                        "H.intPostedBy, " +
                        "H.datPostedDate, " +
                        "H.tntStatusId, " +
                        "W.vchWBS1, " +
                        "W.vchWBS2, " +
                        "W.vchWBS3, H.vchBBSNo, H.BBS_DESC, P.vchProjectCode " +
                        "ORDER BY W.vchWBS1, " +
                        "(case when PATINDEX('B[0-9]%', vchWBS2) > 0 then cast(left(substring(vchWBS2, (PATINDEX('%[0-9]%',vchWBS2)),len(vchWBS2)), " +
                        "PATINDEX('%[^0-9]%', substring(vchWBS2 + 'z', (PATINDEX('%[0-9]%', vchWBS2)), " +
                        "len(vchWBS2)) + 'z') ) as int)    " +
                        "else (case when PATINDEX('FDN[0-9]%', vchWBS2) > 0 then cast(left(substring(vchWBS2, (PATINDEX('%[0-9]%',vchWBS2)),len(vchWBS2)), " +
                        "PATINDEX('%[^0-9]%', substring(vchWBS2, (PATINDEX('%[0-9]%', vchWBS2)), " +
                        "len(vchWBS2)) + 'z') ) as int) else  " +
                        "case when(PATINDEX('B%', vchWBS2) > 0 OR PATINDEX('FDN%', vchWBS2) > 0) then 98  else 99 end " +
                        "end) end), " +
                        " " +
                        "(case when PATINDEX('[^0-9]%',vchWBS2) > 0 then vchWBS2 " +
                        "else '' end), " +
                        " " +
                        "(CASE WHEN PATINDEX('%[0-9]%',vchWBS2) > 0 THEN cast(left(substring(vchWBS2, (PATINDEX('%[0-9]%',vchWBS2)),len(vchWBS2)), " +
                        "PATINDEX('%[^0-9]%', substring(vchWBS2, (PATINDEX('%[0-9]%', vchWBS2) + 1), " +
                        "len(vchWBS2)) + 'z') ) as int) " +
                        "ELSE " +
                        "cast(left(substring(vchWBS2, (PATINDEX('%[0-9]%', vchWBS2)), len(vchWBS2)), " +
                        "PATINDEX('%[^0-9]%', substring(vchWBS2, (PATINDEX('%[0-9]%', vchWBS2)), " +
                        "len(vchWBS2)) + 'z') - 1) as int) " +
                        "END) ,  " +
                        "W.vchWBS3 , " +
                        "S.vchStructureElementType, T.vchProductType, H.vchBBSNo ";
                    }

                    lCmd = new SqlCommand(lSQL, cnNDS);
                    lCmd.CommandTimeout = 1200;
                    adoRst = lCmd.ExecuteReader();
                    if (adoRst.HasRows)
                    {
                        while (adoRst.Read())
                        {
                            lReturn.Add(new
                            {
                                PostHeaderId = adoRst.GetValue(0) == DBNull.Value ? 0 : adoRst.GetInt32(0),
                                StructureElement = adoRst.GetValue(1) == DBNull.Value ? "" : adoRst.GetString(1).Trim(),
                                ProductType = adoRst.GetValue(2) == DBNull.Value ? "" : adoRst.GetString(2).Trim(),
                                PostedQty = adoRst.GetValue(3) == DBNull.Value ? 0 : adoRst.GetInt32(3),
                                PostedWT = adoRst.GetValue(4) == DBNull.Value ? (decimal)0 : (decimal)adoRst.GetDecimal(4),
                                PostedSet = adoRst.GetValue(5) == DBNull.Value ? 0 : adoRst.GetInt32(5),
                                WBSStatus = adoRst.GetValue(6) == DBNull.Value ? "" : adoRst.GetString(6).Trim(),
                                WBS1 = adoRst.GetValue(7) == DBNull.Value ? "" : adoRst.GetString(7).Trim(),
                                WBS2 = adoRst.GetValue(8) == DBNull.Value ? "" : adoRst.GetString(8).Trim(),
                                WBS3 = adoRst.GetValue(9) == DBNull.Value ? "" : adoRst.GetString(9).Trim(),
                                BBSNo = adoRst.GetValue(10) == DBNull.Value ? "" : adoRst.GetString(10).Trim(),
                                BBSDesc = adoRst.GetValue(11) == DBNull.Value ? "" : adoRst.GetString(11).Trim(),
                                SOR = adoRst.GetValue(12) == DBNull.Value ? "" : adoRst.GetString(12).Trim(),
                                RequiredDate = adoRst.GetValue(13) == DBNull.Value ? "" : DateTime.ParseExact(adoRst.GetString(13), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"),
                                AssignedPCs = adoRst.GetValue(14) == DBNull.Value ? 0 : adoRst.GetInt32(14),
                                AssignedWT = adoRst.GetValue(15) == DBNull.Value ? (decimal)0 : (decimal)adoRst.GetDecimal(15),
                                AssignedDate = adoRst.GetValue(16) == DBNull.Value ? "" : adoRst.GetDateTime(16).ToString("yyyy-MM-dd"),
                                PostedDate = adoRst.GetValue(17) == DBNull.Value ? "" : adoRst.GetDateTime(17).ToString("yyyy-MM-dd"),
                                ReleasedDate = adoRst.GetValue(18) == DBNull.Value ? "" : adoRst.GetDateTime(18).ToString("yyyy-MM-dd"),
                                AssignedBy = adoRst.GetValue(19) == DBNull.Value ? "" : adoRst.GetString(19).Trim(),
                                PostedBy = adoRst.GetValue(20) == DBNull.Value ? "" : adoRst.GetString(20).Trim(),
                                ReleasedBy = adoRst.GetValue(21) == DBNull.Value ? "" : adoRst.GetString(21).Trim()
                            });
                        }
                    }
                    adoRst.Close();

                    lProces.CloseNDSConnection(ref cnNDS);
                }
            }
            catch (Exception ex)
            {
                lSuccess = false;
                lErrorMsg = ex.Message;
            }
            adoRst = null;
            lCmd = null;
            cnNDS = null;

            return Ok(lSuccess);
            //return Json(new { success = lSuccess, responseText = lErrorMsg, content = lReturn }, JsonRequestBehavior.AllowGet);
        }

        public async Task<string> AutoGenerateBBSAsync(int intProjectID, int intStrEleID, int intProdTypeID)
        {
            string lErrorMsg = "";
            string lBBSNo = "";

            try
            {
                var myProcess = new ProcessController();

                //var ndsClient = new NDSPosting.BBSPostingServiceClient();ajit
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
                if (myProcess.strServer == "PRD")
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

                //ndsClient.Open();
                //var BBSNo = ndsClient.GenerateBBSNo(intProjectID, intStrEleID, intProdTypeID, out lBBSNo, out lErrorMsg);ajit
                var BBSNo = "";
                using (var client = new HttpClient())
                {
                    try
                    {
                        // Define the API endpoint URL
                        string apiUrl = "http://172.25.1.224:91/Generate_BBSNo"; //+ lPostHeaderID / +lGroupMarkId; // Replace with your API URL

                        string apiUrlWithParameters = string.Format("{0}/{1}/{2}", apiUrl, intProjectID, intStrEleID, intProdTypeID);
                        // Send an HTTP GET request
                        HttpResponseMessage response = await client.GetAsync(apiUrlWithParameters);

                        // Check if the request was successful
                        if (response.IsSuccessStatusCode)
                        {
                            // Read the response content as a string
                            string responseContent = await response.Content.ReadAsStringAsync();
                            BBSNo = JsonConvert.DeserializeObject<string>(responseContent);
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
                //usp_PostingBBSNumberGeneration_Get
                if (BBSNo!=""|| BBSNo != null)
                {
                    return lBBSNo.ToString();
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                return "";
            }
            return lBBSNo.ToString();//ajit
        }

        public async Task SaveWBSAsync(int intWBSElementID, int intProjectID, int intStrEleID, int intProdTypeID, string BBSNo)
        {
            try
            {
                var lProcess = new ProcessController();
                string lErrorMsg = "";
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
                if (lProcess.strServer == "PRD")
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
                //ndsClient = new NDSPosting.BBSPostingServiceClient(binding, address);ajit

                NDSPosting.PostingWBS postingWBS = new NDSPosting.PostingWBS();
                postingWBS.WBSElementId = intWBSElementID;
                postingWBS.ProjectId = intProjectID;
                postingWBS.StructureElementTypeId = intStrEleID;
                postingWBS.ProductTypeId = intProdTypeID;
                postingWBS.BBSNO = BBSNo;
                postingWBS.BBSSDesc = BBSNo;
                //ndsClient.Open();ajit
                bool result = true;
                //var saveWBS = ndsClient.SaveWBS(postingWBS, out result, out lErrorMsg);
                using (var client = new HttpClient())
                {
                    try
                    {
                        bool saveWBS = true;
                        string jsonData = JsonConvert.SerializeObject(postingWBS);
                        StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                        // Define the API endpoint URL

                        string apiUrl = "http://172.25.1.224:91/Save_Wbs";// Replace with your API URL
                        string apiUrlWithParameters = string.Format("{0}", apiUrl);
                        // Send an HTTP GET request
                        HttpResponseMessage response = await client.PostAsync(apiUrlWithParameters, content);

                        // Check if the request was successful
                        if (response.IsSuccessStatusCode)
                        {
                            // Read the response content as a string
                            string responseContent = await response.Content.ReadAsStringAsync();
                            result = JsonConvert.DeserializeObject<bool>(responseContent);
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
                if (result == true)
                {

                }
                //ndsClient.Close();ajit
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        private int getProductTypeID(string prodType)
        {
            try
            {
                string lSQL;
                SqlCommand lCmd;
                SqlDataReader adoRst;
                int lProdTypeID = 0;

                if (prodType == "MESH")
                {
                    prodType = "MSH";
                }
                var lProces = new ProcessController();
                lProces.OpenNDSConnection(ref cnNDS);
                if (cnNDS.State == ConnectionState.Open)
                {
                    lSQL = "SELECT sitProductTypeID " +
                            "FROM dbo.ProductTypeMaster " +
                            "WHERE vchProductType = '" + prodType + "' ";
                    lCmd = new SqlCommand(lSQL, cnNDS);
                    //lCmd.Transaction = osqlTransNDS;
                    lCmd.CommandTimeout = 1200;
                    adoRst = lCmd.ExecuteReader();
                    if (adoRst.HasRows)
                    {
                        adoRst.Read();
                        lProdTypeID = (Int16)adoRst.GetValue(0);
                    }

                    adoRst.Close();

                    lCmd = null;
                    lProces.CloseNDSConnection(ref cnNDS);

                }
                return lProdTypeID;
            }
            catch (Exception ex)
            {
                //lCmd = null;
                return 0;
            }
        }

        private int getStructureElementID(string StructureElement)
        {
            string lSQL;
            SqlCommand lCmd;
            SqlDataReader adoRst;
            int lStrEleID = 0;
            try
            {
                var lProces = new ProcessController();
                lProces.OpenNDSConnection(ref cnNDS);
                if (cnNDS.State == ConnectionState.Open)
                {
                    lSQL = "SELECT intStructureElementTypeId " +
                                "FROM dbo.StructureElementMaster " +
                                "WHERE vchStructureElementType = '" + StructureElement + "' ";
                    lCmd = new SqlCommand(lSQL, cnNDS);
                    //lCmd.Transaction = osqlTransNDS;
                    lCmd.CommandTimeout = 1200;
                    adoRst = lCmd.ExecuteReader();
                    if (adoRst.HasRows)
                    {
                        adoRst.Read();
                        lStrEleID = (int)adoRst.GetValue(0);
                    }
                    adoRst.Close();
                    lCmd = null;
                    lProces.CloseNDSConnection(ref cnNDS);
                }

                return lStrEleID;
            }
            catch (Exception ex)
            {
                lCmd = null;
                return 0;
            }
        }

        private int getWBSID(string lProjectID)
        {
            string lSQL;
            SqlCommand lCmd;
            SqlDataReader adoRst;
            int lWBSID = 0;
            try
            {
                var lProces = new ProcessController();
                lProces.OpenNDSConnection(ref cnNDS);
                if (cnNDS.State == ConnectionState.Open)
                {


                    // WBS ID

                    lSQL = "SELECT intWBSId FROM dbo.WBS " +
                                "WHERE intProjectid = " + lProjectID.ToString() + " " +
                                "AND intWBSTypeID = 1 ";

                    lCmd = new SqlCommand(lSQL, cnNDS);
                    //lCmd.Transaction = osqlTransNDS;
                    lCmd.CommandTimeout = 1200;
                    adoRst = lCmd.ExecuteReader();
                    if (adoRst.HasRows)
                    {
                        adoRst.Read();
                        lWBSID = (int)adoRst.GetValue(0);
                    }
                    adoRst.Close();
                    lCmd = null;
                    lProces.CloseNDSConnection(ref cnNDS);
                    cnNDS = null;
                    return lWBSID;
                }
                else
                {
                    lCmd = null;
                    cnNDS = null;
                    return lWBSID;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        async Task<string> BeamTransferAsync(
        int pProjectID,
        int pProdTypeID,
        int pStruEleID,
        int pSElevelID,
        string pBBSNo,
        string pBBSDesc,
        int pParam,
        List<CTSMESHBeamDetailsModels> pBBSDetails)
        {
            string lErrorMsg = "";
            bool lSuccess = true;

            string AmendDate;
            string lCageMark;
            int lCageQty;
            int lBeamWidth, lBeamDepth, lBeamSlope;
            int lCageWidth, lCageDepth, lCageSlope;
            int lClearSpan, lLinksNo;
            string lCageProdCode, lCapProdCode, lShape;
            bool lCappingBoon;
            int lParA, lParB, lParC, lParD, lParE, lParP, lParQ, lParHook, lParLeg;

            int lParentStructureMarkID = 0;

            string lMWBOM = "";
            string lCWBOM = "";

            int lCoverGap1, lCoverGap2, lCoverTop, lCoverBottom, lCoverLeft, lCoverRight, lCoverHook, lCoverLeg;
            lCoverGap1 = 0;
            lCoverGap2 = 0;
            lCoverTop = 0;
            lCoverBottom = 0;
            lCoverLeft = 0;
            lCoverRight = 0;
            lCoverHook = 0;
            lCoverLeg = 0;

            lSuccess = true;
            AmendDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            if (pBBSDetails.Count == 0)
            {
                lErrorMsg = "Invalide Stirrup Link Mesh details.";
                return lErrorMsg;
            }

            var myTransport = new HttpTransportBindingElement();
            var muEncoding = new BinaryMessageEncodingBindingElement();

            var binding = new CustomBinding();
            //var binding = new BasicHttpBinding();

            binding.Name = "customBinding0";
            binding.Elements.Add(muEncoding);
            binding.Elements.Add(myTransport);

            //Specify the address to be used for the client.

            //var address = new EndpointAddress("http://172.25.1.224:81/NDSWCF_CAB/BEAMService.svc");
            var address = new EndpointAddress("http://172.25.1.224:81/NDSWCF_CUBE/BEAMService.svc");

            if (strServer == "PRD")
            {
                address = new EndpointAddress("http://172.25.1.141:81/NDSWCF_PV/BEAMService.svc");
            }

            // Create a client that is configured with this address and binding.
            //var ndsClient = new NDSBeam.BeamServiceClient(binding, address);ajit


            //Dim ndsClient As NDSBeam.BeamServiceClient = New NDSBeam.BeamServiceClient("NatSteel.NDS.WCF.BeamService")
            var ndsBeamStructure = new NDSBeam.BeamStructure();
            var ndsShape = new NDSBeam.ShapeCode();
            var ndsShapes = new List<NDSBeam.ShapeCode>();
            var ndsProd = new NDSBeam.ProductCode();
            var ndsProds = new List<NDSBeam.ProductCode>();
            var ndsCapProd = new NDSBeam.ProductCode();
            var ndsShapeParam = new NDSBeam.ShapeParameter();
            var ndsBeamProd = new List<NDSBeam.BeamProduct>();
            var ndsProdMark = new List<NDSBeam.BeamProduct>().ToArray();

            try
            {
                //ndsClient.Open();
                lErrorMsg = "";
                string lbendCheck = "";
                if (pBBSDetails.Count > 0)
                {
                    for (int i = 0; i < pBBSDetails.Count; i++)
                    {

                        lShape = pBBSDetails[i].MeshShapeCode;
                        if (lShape != null && lShape != "")
                        {
                            lShape = lShape.Trim();
                            lCageMark = pBBSDetails[i].MeshMark;
                            lCageWidth = (int)pBBSDetails[i].MeshWidth;
                            lCageDepth = (int)pBBSDetails[i].MeshDepth;
                            lCageSlope = (int)pBBSDetails[i].MeshSlope;
                            lLinksNo = (int)pBBSDetails[i].MeshTotalLinks;
                            lClearSpan = (int)pBBSDetails[i].MeshSpan;
                            lCageProdCode = pBBSDetails[i].MeshProduct;
                            lCageQty = (int)pBBSDetails[i].MeshMemberQty;

                            lMWBOM = pBBSDetails[i].MWBOM;
                            lCWBOM = pBBSDetails[i].CWBOM;
                            if (lMWBOM == null)
                            {
                                lMWBOM = "";
                            }
                            if (lCWBOM == null)
                            {
                                lCWBOM = "";
                            }

                            lCappingBoon = pBBSDetails[i].MeshCapping;
                            lCapProdCode = pBBSDetails[i].MeshCPProduct == null ? "" : pBBSDetails[i].MeshCPProduct;

                            lBeamWidth = lCageWidth;
                            lBeamDepth = lCageDepth;
                            lBeamSlope = lCageSlope;

                            //lBeamLength = Trim(dgrdMESH.Rows.Item(i).Cells("mBeamLength").Value)

                            lParA = pBBSDetails[i].A == null ? 0 : (int)pBBSDetails[i].A;
                            lParB = pBBSDetails[i].B == null ? 0 : (int)pBBSDetails[i].B;
                            lParC = pBBSDetails[i].C == null ? 0 : (int)pBBSDetails[i].C;
                            lParD = pBBSDetails[i].D == null ? 0 : (int)pBBSDetails[i].D;
                            lParE = pBBSDetails[i].E == null ? 0 : (int)pBBSDetails[i].E;
                            lParP = pBBSDetails[i].P == null ? 0 : (int)pBBSDetails[i].P;
                            lParQ = pBBSDetails[i].Q == null ? 0 : (int)pBBSDetails[i].Q;
                            lParHook = pBBSDetails[i].HOOK == null ? 0 : (int)pBBSDetails[i].HOOK;
                            lParLeg = pBBSDetails[i].LEG == null ? 0 : (int)pBBSDetails[i].LEG;


                            lbendCheck = "";
                            lErrorMsg = "'";

                            ndsShape = new NDSBeam.ShapeCode();
                            var lSShape = lShape;
                            if (lShape.Length < 7)
                            {
                                var lSpace = "                ";
                                lSShape = lShape.Trim() + lSpace.Substring(0, 15 - lShape.Trim().Length);
                            }

                            HttpClient httpClient = new HttpClient();
                            bool isSuccess;
                            string apiUrl = "http://172.25.1.224:89/Detailing/FilterShapeCode_column";


                            string apiUrlWithParameters = string.Format("{0}/{1}", apiUrl, lSShape);
                            string jsonContent = null;

                            HttpResponseMessage response = await httpClient.GetAsync(apiUrlWithParameters);

                            if (response.IsSuccessStatusCode)
                            {
                                jsonContent = response.Content.ReadAsStringAsync().Result;
                                ndsShapes = JsonConvert.DeserializeObject<List<NDSBeam.ShapeCode>>(jsonContent);
                            }
                            else
                            {
                                if (response.StatusCode == HttpStatusCode.BadRequest)
                                {
                                    lErrorMsg = response.Content.ReadAsStringAsync().Result;
                                }
                            }
                            for (int j = 0; j < ndsShapes.Count; j++)
                            {
                                if (ndsShapes[j].ShapeCodeName == lShape)
                                {
                                    ndsShape = ndsShapes[j];
                                    break;
                                }
                            }

                            //ndsShape = ndsClient.FilterShapeCode(lSShape, out lErrorMsg)[0];ajit
                            HttpClient httpClient1 = new HttpClient();
                            bool isSuccess1;
                            string apiUrl1 = "http://172.25.1.224:89/Detailing/FilterShapeCode_slab";
                            string apiUrlWithParameters1 = string.Format("{0}/{1}", apiUrl1, lSShape);
                            string jsonContent1 = null;

                            HttpResponseMessage response1 = await httpClient1.GetAsync(apiUrlWithParameters1);

                            if (response1.IsSuccessStatusCode)
                            {
                                jsonContent1 = response1.Content.ReadAsStringAsync().Result;
                                ndsShape = JsonConvert.DeserializeObject<NDSBeam.ShapeCode>(jsonContent1);
                            }
                            else
                            {
                                if (response1.StatusCode == HttpStatusCode.BadRequest)
                                {
                                    lErrorMsg = response1.Content.ReadAsStringAsync().Result;
                                }
                            }
                            for (int j = 0; j < ndsShape.ShapeParam.Length; j++)
                            {
                                if (ndsShape.ShapeParam[j].ParameterName.ToUpper() == "A" && ndsShape.ShapeParam[j].EditFlag == true)
                                {
                                    ndsShape.ShapeParam[j].ParameterValue = lParA;
                                }
                                else if (ndsShape.ShapeParam[j].ParameterName.ToUpper() == "B" && ndsShape.ShapeParam[j].EditFlag == true)
                                {
                                    ndsShape.ShapeParam[j].ParameterValue = lParB;
                                }
                                else if (ndsShape.ShapeParam[j].ParameterName.ToUpper() == "C" && ndsShape.ShapeParam[j].EditFlag == true)
                                {
                                    ndsShape.ShapeParam[j].ParameterValue = lParC;
                                }
                                else if (ndsShape.ShapeParam[j].ParameterName.ToUpper() == "D" && ndsShape.ShapeParam[j].EditFlag == true)
                                {
                                    ndsShape.ShapeParam[j].ParameterValue = lParD;
                                }
                                else if (ndsShape.ShapeParam[j].ParameterName.ToUpper() == "E" && ndsShape.ShapeParam[j].EditFlag == true)
                                {
                                    ndsShape.ShapeParam[j].ParameterValue = lParE;
                                }
                                else if (ndsShape.ShapeParam[j].ParameterName.ToUpper() == "P" && ndsShape.ShapeParam[j].EditFlag == true)
                                {
                                    ndsShape.ShapeParam[j].ParameterValue = lParP;
                                }
                                else if (ndsShape.ShapeParam[j].ParameterName.ToUpper() == "Q" && ndsShape.ShapeParam[j].EditFlag == true)
                                {
                                    ndsShape.ShapeParam[j].ParameterValue = lParQ;
                                }
                                else if (ndsShape.ShapeParam[j].ParameterName.ToUpper() == "HOOK" && ndsShape.ShapeParam[j].EditFlag == true)
                                {
                                    ndsShape.ShapeParam[j].ParameterValue = lParHook;
                                }
                                else if (ndsShape.ShapeParam[j].ParameterName.ToUpper() == "LEG" && ndsShape.ShapeParam[j].EditFlag == true)
                                {
                                    ndsShape.ShapeParam[j].ParameterValue = lParLeg;
                                }
                            }
                            ndsProd = new NDSBeam.ProductCode();

                            //ndsProds = ndsClient.FilterProductCode(pStruEleID, pProdTypeID, lCageProdCode, out lErrorMsg).ToList();ajit
                            using (var client = new HttpClient())
                            {
                                try
                                {
                                    // Define the API endpoint URL
                                    string apiUrl8 = "http://172.25.1.224:89/Detailing/FilterCapProductCode"; //+ lPostHeaderID / +lGroupMarkId; // Replace with your API URL

                                    string apiUrlWithParameters8 = string.Format("{0}/{1}/{2}/{3}", apiUrl, pStruEleID, pProdTypeID, lCageProdCode);
                                    // Send an HTTP GET request
                                    HttpResponseMessage response8 = await client.GetAsync(apiUrlWithParameters8);

                                    // Check if the request was successful
                                    if (response8.IsSuccessStatusCode)
                                    {
                                        // Read the response content as a string
                                        string responseContent = await response8.Content.ReadAsStringAsync();
                                        ndsProds = JsonConvert.DeserializeObject<List<NDSBeam.ProductCode>>(responseContent);
                                        // Now you can work with the response content (e.g., parse JSON, process data, etc.)
                                        Console.WriteLine(responseContent);
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Error: {response8.StatusCode} - {response8.ReasonPhrase}");
                                    }
                                }
                                catch (HttpRequestException ex)
                                {
                                    Console.WriteLine($"HTTP request error: {ex.Message}");
                                }
                            }

                            for (int j = 0; j < ndsProds.Count; j++)
                            {
                                if (lCageProdCode.Trim() == ndsProds[j].ProductCodeName.Trim())
                                {
                                    ndsProd = ndsProds[j];
                                    break;
                                }
                            }

                            ndsCapProd = new NDSBeam.ProductCode();
                            if (lCappingBoon == true)
                            {
                                //ndsProds = ndsClient.FilterCapProductCode(lCapProdCode, out lErrorMsg).ToList();
                                using (var client = new HttpClient())
                                {
                                    try
                                    {
                                        // Define the API endpoint URL
                                        string apiUrl8 = "http://172.25.1.224:89/Detailing/FilterCapProductCode"; //+ lPostHeaderID / +lGroupMarkId; // Replace with your API URL

                                        string apiUrlWithParameters8 = string.Format("{0}/{1}", apiUrl, lCapProdCode);
                                        // Send an HTTP GET request
                                        HttpResponseMessage response8 = await client.GetAsync(apiUrlWithParameters8);

                                        // Check if the request was successful
                                        if (response8.IsSuccessStatusCode)
                                        {
                                            // Read the response content as a string
                                            string responseContent = await response8.Content.ReadAsStringAsync();
                                            ndsProds = JsonConvert.DeserializeObject<List<NDSBeam.ProductCode>>(responseContent);
                                            // Now you can work with the response content (e.g., parse JSON, process data, etc.)
                                            Console.WriteLine(responseContent);
                                        }
                                        else
                                        {
                                            Console.WriteLine($"Error: {response8.StatusCode} - {response8.ReasonPhrase}");
                                        }
                                    }
                                    catch (HttpRequestException ex)
                                    {
                                        Console.WriteLine($"HTTP request error: {ex.Message}");
                                    }
                                }
                                for (int j = 0; j < ndsProds.Count; j++)
                                {
                                    if (lCapProdCode.Trim() == ndsProds[j].ProductCodeName.Trim())
                                    {
                                        ndsCapProd = ndsProds[j];
                                        break;
                                    }
                                }
                                
                            }

                            ndsBeamStructure = new NDSBeam.BeamStructure();

                            ndsBeamStructure.StructureMarkName = lCageMark;
                            ndsBeamStructure.StructureMarkId = 0;
                            ndsBeamStructure.Width = lBeamWidth;
                            ndsBeamStructure.Depth = lBeamDepth;
                            ndsBeamStructure.Slope = lBeamSlope;
                            ndsBeamStructure.Stirupps = lLinksNo;
                            ndsBeamStructure.Span = lClearSpan;
                            ndsBeamStructure.Shape = ndsShape;
                            ndsBeamStructure.Qty = lCageQty;
                            ndsBeamStructure.ProductMark = ndsProdMark;
                            ndsBeamStructure.ProductCode = ndsProd;
                            ndsBeamStructure.PinSize = 32;
                            ndsBeamStructure.ParentStructureMarkId = lParentStructureMarkID;
                            ndsBeamStructure.IsCap = lCappingBoon;
                            ndsBeamStructure.CapProduct = ndsCapProd;
                            ndsBeamStructure.BendingCheckStatus = true;
                            ndsBeamStructure.ProductGenerationStatus = true;
                            ndsBeamStructure.ProduceInd = true;
                            ndsBeamStructure.BendingCheckInd = true;
                            //ndsBeamProd = ndsClient.EditEnd(ref ndsBeamStructure, lCoverGap1, lCoverGap2, lCoverTop, lCoverBottom, lCoverLeft, lCoverRight, lCoverHook, lCoverLeg, pSElevelID, out lErrorMsg, out lbendCheck).ToList();ajit
                            HttpClient httpClient4 = new HttpClient();
                            bool isSuccess4;
                            string apiUrl4 = "http://172.25.1.224:89/Detailing/EditEnd";
                            // Define your dictionary and populate it with key-value pairs if needed
                            // Dictionary<string, string> parameters = new Dictionary<string, string>();

                            string jsonData4 = JsonConvert.SerializeObject(ndsBeamStructure);
                            HttpContent content4 = new StringContent(jsonData4, Encoding.UTF8, "application/json");

                            string apiUrlWithParameters4 = string.Format("{0}/{1}/{2}/{3}/{4}/{5}/{6}/{7}/{8}/{9}", apiUrl4, lCoverGap1, lCoverGap2, lCoverTop, lCoverBottom, lCoverLeft, lCoverRight, lCoverHook, lCoverLeg, pSElevelID);
                            string jsonContent4 = null;

                            HttpResponseMessage response4 = httpClient4.PostAsync(apiUrlWithParameters4, content4).Result;

                            if (response4.IsSuccessStatusCode)
                            {
                                jsonContent4 = response4.Content.ReadAsStringAsync().Result;
                                ndsBeamProd = JsonConvert.DeserializeObject<List<NDSBeam.BeamProduct>>(jsonContent4);
                            }
                            else
                            {
                                if (response4.StatusCode == HttpStatusCode.BadRequest)
                                {
                                    lErrorMsg = response4.Content.ReadAsStringAsync().Result;
                                }
                            }
                            if (lErrorMsg.Trim().Length > 0)
                            {
                                lSuccess = false;
                                break;
                            }

                            if (lSuccess == true)
                            {
                                if ((lMWBOM.Length > 0 || lCWBOM.Length > 0) && ndsBeamProd.Count > 0)
                                {
                                    //Update BOM
                                    for (int j = 0; j < ndsBeamProd.Count; j++)
                                    {
                                        var lProcess = new ProcessController();
                                        lSuccess = lProcess.updateBeamBOM(ndsBeamProd[j].ProductMarkID, pStruEleID, lMWBOM, lCWBOM,
                                        ndsBeamProd[j].MO1, ndsBeamProd[j].MO2,
                                        ndsBeamProd[j].CO1, ndsBeamProd[j].CO2,
                                        ndsBeamProd[j].ProductionMO1, ndsBeamProd[j].ProductionMO2,
                                        ndsBeamProd[j].ProductionCO1, ndsBeamProd[j].ProductionCO2,
                                        0, 0);

                                        lProcess = null;

                                        if (lSuccess == false)
                                        {
                                            lSuccess = false;
                                            break;
                                        }
                                    }
                                }
                            }

                        }
                    }
                }

            }

            catch (Exception ex)
            {
                lSuccess = false;
            }
            myTransport = null;
            muEncoding = null;

            binding = null;
            address = null;
            //ndsClient = null;ajit
            ndsBeamStructure = null;
            ndsShape = null;
            ndsProd = null;
            ndsCapProd = null;
            ndsShapeParam = null;
            ndsBeamProd = null;
            ndsProdMark = null;

            return lErrorMsg;
        }

        async Task<string> ColumnTransferAsync(
            int pProjectID,
            int pProdTypeID,
            int pStruEleID,
            int pSElevelID,
            string pBBSNo,
            string pBBSDesc,
            int pParam,
            List<CTSMESHColumnDetailsModels> pBBSDetails)
        {
            string lErrorMsg = "";
            bool lSuccess = true;

            int lParentStructureMarkID = 0;
            string AmendDate;
            string lCageMark;
            int lCageQty;
            int lColumnWidth, lColumnLength;
            int lCageWidth, lCageLeng;
            int lHeight, lLinksNo;
            string lCageProdCode, lShape;
            int lRowsAtLen = 0;
            string lProdAtLen = "";
            int lRowsAtWidth = 0;
            string lProdAtWidth = "";
            int lParA, lParB, lParC, lParD, lParE, lParF, lParP, lParQ, lParLeg;

            string lMWBOM = "";
            string lCWBOM = "";

            int lCoverTop, lCoverBottom, lCoverLeft, lCoverRight, lCoverLeg;
            lCoverTop = 0;
            lCoverBottom = 0;
            lCoverLeft = 0;
            lCoverRight = 0;
            lCoverLeg = 0;

            lSuccess = true;
            AmendDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            if (pBBSDetails.Count == 0)
            {
                lErrorMsg = "Invalide Column Link Mesh details.";
                return lErrorMsg;
            }

            var myTransport = new HttpTransportBindingElement();
            var muEncoding = new BinaryMessageEncodingBindingElement();

            var binding = new CustomBinding();
            //var binding = new BasicHttpBinding();

            binding.Name = "customBinding0";
            binding.Elements.Add(muEncoding);
            binding.Elements.Add(myTransport);

            //Specify the address to be used for the client.

            //var address = new EndpointAddress("http://172.25.1.224:81/NDSWCF_CAB/ColumnService.svc");
            var address = new EndpointAddress("http://172.25.1.224:81/NDSWCF_CUBE/ColumnService.svc");

            if (strServer == "PRD")
            {
                address = new EndpointAddress("http://172.25.1.141:81/NDSWCF_PV/ColumnService.svc");
            }

            // Create a client that is configured with this address and binding.
            //var ndsClient = new NDSColumn.ColumnServiceClient(binding, address);ajit


            //Dim ndsClient As NDSColumn.ColumnServiceClient = New NDSColumn.ColumnServiceClient("NatSteel.NDS.WCF.ColumnService")
            var ndsColumnStructure = new NDSColumn.ColumnStructure();
            var ndsShape = new NDSColumn.ShapeCode();
            var ndsShapes = new List<NDSColumn.ShapeCode>();
            var ndsProd = new NDSColumn.ProductCode();
            var ndsProds = new List<NDSColumn.ProductCode>();
            var ndsLenProd = new NDSColumn.ProductCode();
            var ndsWidthProd = new NDSColumn.ProductCode();
            var ndsShapeParam = new NDSColumn.ShapeParameter();
            var ndsColumnProd = new List<NDSColumn.ColumnProduct>();
            var ndsProdMark = new List<NDSColumn.ColumnProduct>().ToArray();

            try
            {
                //ndsClient.Open();
                lErrorMsg = "";

                if (pBBSDetails.Count > 0)
                {
                    for (int i = 0; i < pBBSDetails.Count; i++)
                    {

                        lShape = pBBSDetails[i].MeshShapeCode;
                        if (lShape != null && lShape != "")
                        {
                            lShape = lShape.Trim();
                            lCageMark = pBBSDetails[i].MeshMark;
                            lCageWidth = (int)pBBSDetails[i].MeshWidth;
                            lCageLeng = (int)pBBSDetails[i].MeshLength;
                            lLinksNo = (int)pBBSDetails[i].MeshTotalLinks;
                            lHeight = (int)pBBSDetails[i].MeshHeight;
                            lCageProdCode = pBBSDetails[i].MeshProduct;
                            lCageQty = (int)pBBSDetails[i].MeshMemberQty;

                            lMWBOM = pBBSDetails[i].MWBOM;
                            lCWBOM = pBBSDetails[i].CWBOM;
                            if (lMWBOM == null)
                            {
                                lMWBOM = "";
                            }
                            if (lCWBOM == null)
                            {
                                lCWBOM = "";
                            }

                            lRowsAtLen = pBBSDetails[i].MeshCLinkRowsAtLen == null ? 0 : (int)pBBSDetails[i].MeshCLinkRowsAtLen;
                            lProdAtLen = pBBSDetails[i].MeshCLinkProductAtLen == null ? "" : pBBSDetails[i].MeshCLinkProductAtLen;
                            lRowsAtWidth = pBBSDetails[i].MeshCLinkRowsAtWidth == null ? 0 : (int)pBBSDetails[i].MeshCLinkRowsAtWidth;
                            lProdAtWidth = pBBSDetails[i].MeshCLinkProductAtWidth == null ? "" : pBBSDetails[i].MeshCLinkProductAtWidth;

                            lColumnWidth = lCageWidth;
                            lColumnLength = lCageLeng;

                            //lColumnLength = Trim(dgrdMESH.Rows.Item(i).Cells("mColumnLength").Value)

                            lParA = pBBSDetails[i].A == null ? 0 : (int)pBBSDetails[i].A;
                            lParB = pBBSDetails[i].B == null ? 0 : (int)pBBSDetails[i].B;
                            lParC = pBBSDetails[i].C == null ? 0 : (int)pBBSDetails[i].C;
                            lParD = pBBSDetails[i].D == null ? 0 : (int)pBBSDetails[i].D;
                            lParE = pBBSDetails[i].E == null ? 0 : (int)pBBSDetails[i].E;
                            lParF = pBBSDetails[i].F == null ? 0 : (int)pBBSDetails[i].F;
                            lParP = pBBSDetails[i].P == null ? 0 : (int)pBBSDetails[i].P;
                            lParQ = pBBSDetails[i].Q == null ? 0 : (int)pBBSDetails[i].Q;
                            lParLeg = pBBSDetails[i].LEG == null ? 0 : (int)pBBSDetails[i].LEG;


                            lErrorMsg = "'";

                            ndsShape = new NDSColumn.ShapeCode();

                            //ndsShapes = ndsClient.FilterShapeCode(lShape, out lErrorMsg).ToList();ajit
                            HttpClient httpClient1 = new HttpClient();
                            bool isSuccess1;
                            string apiUrl1 = "http://172.25.1.224:89/Detailing/FilterShapeCode_column";
                            string apiUrlWithParameters1 = string.Format("{0}/{1}", apiUrl1, lShape);
                            string jsonContent1 = null;

                            HttpResponseMessage response1 = await httpClient1.GetAsync(apiUrlWithParameters1);

                            if (response1.IsSuccessStatusCode)
                            {
                                jsonContent1 = response1.Content.ReadAsStringAsync().Result;
                                ndsShapes = JsonConvert.DeserializeObject<List<NDSColumn.ShapeCode>>(jsonContent1);
                            }
                            else
                            {
                                if (response1.StatusCode == HttpStatusCode.BadRequest)
                                {
                                    lErrorMsg = response1.Content.ReadAsStringAsync().Result;
                                }
                            }
                            for (int j = 0; j < ndsShapes.Count; j++)
                            {
                                if (ndsShapes[j].ShapeCodeName == lShape)
                                {
                                    ndsShape = ndsShapes[j];
                                    break;
                                }
                            }

                            for (int j = 0; j < ndsShape.ShapeParam.Length; j++)
                            {
                                if (ndsShape.ShapeParam[j].ParameterName.ToUpper() == "A" && ndsShape.ShapeParam[j].EditFlag == true)
                                {
                                    ndsShape.ShapeParam[j].ParameterValue = lParA;
                                }
                                else if (ndsShape.ShapeParam[j].ParameterName.ToUpper() == "B" && ndsShape.ShapeParam[j].EditFlag == true)
                                {
                                    ndsShape.ShapeParam[j].ParameterValue = lParB;
                                }
                                else if (ndsShape.ShapeParam[j].ParameterName.ToUpper() == "C" && ndsShape.ShapeParam[j].EditFlag == true)
                                {
                                    ndsShape.ShapeParam[j].ParameterValue = lParC;
                                }
                                else if (ndsShape.ShapeParam[j].ParameterName.ToUpper() == "D" && ndsShape.ShapeParam[j].EditFlag == true)
                                {
                                    ndsShape.ShapeParam[j].ParameterValue = lParD;
                                }
                                else if (ndsShape.ShapeParam[j].ParameterName.ToUpper() == "E" && ndsShape.ShapeParam[j].EditFlag == true)
                                {
                                    ndsShape.ShapeParam[j].ParameterValue = lParE;
                                }
                                else if (ndsShape.ShapeParam[j].ParameterName.ToUpper() == "F" && ndsShape.ShapeParam[j].EditFlag == true)
                                {
                                    ndsShape.ShapeParam[j].ParameterValue = lParF;
                                }
                                else if (ndsShape.ShapeParam[j].ParameterName.ToUpper() == "P" && ndsShape.ShapeParam[j].EditFlag == true)
                                {
                                    ndsShape.ShapeParam[j].ParameterValue = lParP;
                                }
                                else if (ndsShape.ShapeParam[j].ParameterName.ToUpper() == "Q" && ndsShape.ShapeParam[j].EditFlag == true)
                                {
                                    ndsShape.ShapeParam[j].ParameterValue = lParQ;
                                }
                                else if (ndsShape.ShapeParam[j].ParameterName.ToUpper() == "LEG" && ndsShape.ShapeParam[j].EditFlag == true)
                                {
                                    ndsShape.ShapeParam[j].ParameterValue = lParLeg;
                                }
                            }
                            ndsProd = new NDSColumn.ProductCode();

                            //ndsProds = ndsClient.FilterProductCode(pStruEleID, pProdTypeID, lCageProdCode, out lErrorMsg).ToList();ajit
                            HttpClient httpClient2 = new HttpClient();
                            bool isSuccess2;
                            string apiUrl2 = "http://172.25.1.224:89/Detailing/FilterProductCode";
                            string apiUrlWithParameters2 = string.Format("{0}/{1}/{2}/{3}", apiUrl1, pStruEleID, pProdTypeID, lCageProdCode);
                            string jsonContent2 = null;

                            HttpResponseMessage response2 = await httpClient2.GetAsync(apiUrlWithParameters2);

                            if (response2.IsSuccessStatusCode)
                            {
                                jsonContent2 = response2.Content.ReadAsStringAsync().Result;
                                ndsProds = JsonConvert.DeserializeObject<List<NDSColumn.ProductCode>>(jsonContent2);
                            }
                            else
                            {
                                if (response2.StatusCode == HttpStatusCode.BadRequest)
                                {
                                    lErrorMsg = response1.Content.ReadAsStringAsync().Result;
                                }
                            }
                            for (int j = 0; j < ndsProds.Count; j++)
                            {
                                if (lCageProdCode.Trim() == ndsProds[j].ProductCodeName.Trim())
                                {
                                    ndsProd = ndsProds[j];
                                    break;
                                }
                            }

                            if (lRowsAtLen > 0)
                            {
                                //ndsProds = ndsClient.FilterClinkProduct(lProdAtLen, out lErrorMsg).ToList();ajit
                                HttpClient httpClientClinkProduct = new HttpClient();
                                bool isSuccessClinkProduct;
                                string apiUrlClinkProduct = "http://172.25.1.224:89/Detailing/FilterClinkProduct";
                                string apiUrlWithParametersClinkProduct = string.Format("{0}/{1}", apiUrlClinkProduct, lProdAtLen);
                                string jsonContentClinkProduct = null;

                                HttpResponseMessage responseClinkProduct = await httpClient2.GetAsync(apiUrlWithParametersClinkProduct);

                                if (responseClinkProduct.IsSuccessStatusCode)
                                {
                                    jsonContentClinkProduct = responseClinkProduct.Content.ReadAsStringAsync().Result;
                                    ndsProds = JsonConvert.DeserializeObject<List<NDSColumn.ProductCode>>(jsonContentClinkProduct);
                                }
                                else
                                {
                                    if (responseClinkProduct.StatusCode == HttpStatusCode.BadRequest)
                                    {
                                        lErrorMsg = responseClinkProduct.Content.ReadAsStringAsync().Result;
                                    }
                                }
                                for (int j = 0; j < ndsProds.Count; j++)
                                {
                                    if (lProdAtLen.Trim() == ndsProds[j].ProductCodeName.Trim())
                                    {
                                        ndsLenProd = ndsProds[j];
                                        break;
                                    }
                                }
                            }

                            if (lRowsAtWidth > 0)
                            {
                                // ndsProds = ndsClient.FilterClinkProduct(lProdAtWidth, out lErrorMsg).ToList();ajit
                                HttpClient httpClientClinkProduct = new HttpClient();
                                bool isSuccessClinkProduct;
                                string apiUrlClinkProduct = "http://172.25.1.224:89/Detailing/FilterClinkProduct";
                                string apiUrlWithParametersClinkProduct = string.Format("{0}/{1}", apiUrlClinkProduct, lProdAtWidth);
                                string jsonContentClinkProduct = null;

                                HttpResponseMessage responseClinkProduct = await httpClient2.GetAsync(apiUrlWithParametersClinkProduct);

                                if (responseClinkProduct.IsSuccessStatusCode)
                                {
                                    jsonContentClinkProduct = responseClinkProduct.Content.ReadAsStringAsync().Result;
                                    ndsProds = JsonConvert.DeserializeObject<List<NDSColumn.ProductCode>>(jsonContentClinkProduct);
                                }
                                else
                                {
                                    if (responseClinkProduct.StatusCode == HttpStatusCode.BadRequest)
                                    {
                                        lErrorMsg = responseClinkProduct.Content.ReadAsStringAsync().Result;
                                    }
                                }
                                for (int j = 0; j < ndsProds.Count; j++)
                                {
                                    if (lProdAtWidth.Trim() == ndsProds[j].ProductCodeName.Trim())
                                    {
                                        ndsWidthProd = ndsProds[j];
                                        break;
                                    }
                                }
                            }

                            ndsColumnStructure = new NDSColumn.ColumnStructure();

                            ndsColumnStructure.StructureMarkingName = lCageMark;
                            ndsColumnStructure.StructureMarkId = 0;
                            ndsColumnStructure.ColumnWidth = lColumnWidth;
                            ndsColumnStructure.ColumnLength = lColumnLength;
                            ndsColumnStructure.TotalNoOfLinks = lLinksNo;

                            ndsColumnStructure.ProductCode = ndsProd;
                            ndsColumnStructure.Shape = ndsShape;

                            ndsColumnStructure.RowatLength = lRowsAtLen;
                            ndsColumnStructure.ClinkProductCodeIdAtLength = ndsLenProd.ProductCodeId;
                            ndsColumnStructure.ClinkProductLength = ndsLenProd;

                            ndsColumnStructure.MemberQty = lCageQty;

                            ndsColumnStructure.RowatWidth = lRowsAtWidth;
                            ndsColumnStructure.ClinkProductCodeIdAtWidth = ndsWidthProd.ProductCodeId;
                            ndsColumnStructure.ClinkProductWidth = ndsWidthProd;

                            if (lRowsAtLen > 0 || lRowsAtWidth > 0)
                            {
                                ndsColumnStructure.IsCLink = true;
                            }
                            else
                            {
                                ndsColumnStructure.IsCLink = false;
                            }

                            ndsColumnStructure.CLOnly = false;

                            ndsColumnStructure.ParamSetNumber = pParam;
                            ndsColumnStructure.SEDetailingID = pSElevelID;

                            ndsColumnStructure.ProduceIndicator = true;
                            ndsColumnStructure.ProductGenerationStatus = true;

                            ndsColumnStructure.BendingCheckInd = true;

                            ndsColumnStructure.PinSize = 32;
                            ndsColumnStructure.ParentStructureMarkId = lParentStructureMarkID;

                            //ndsColumnProd = ndsClient.InsertColumnStructureMarking(ref ndsColumnStructure, lCoverTop, lCoverBottom, lCoverLeft, lCoverRight, lCoverLeg, pSElevelID, 111, out lErrorMsg).ToList();ajit
                            HttpClient httpClient = new HttpClient();
                            bool isSuccess;
                            string apiUrl = "http://172.25.1.224:89/Detailing/InsertColumnStructureMarking";


                            string apiUrlWithParameters = string.Format("{0}/{1}/{2}/{3}/{4}/{5}/{6}/{7}", apiUrl, lCoverTop, lCoverBottom, lCoverLeft, lCoverRight, lCoverLeg, pSElevelID, 111);
                            string jsonContent = null;

                            HttpResponseMessage response = await httpClient.GetAsync(apiUrlWithParameters);

                            if (response.IsSuccessStatusCode)
                            {
                                jsonContent = response.Content.ReadAsStringAsync().Result;
                                ndsColumnProd = JsonConvert.DeserializeObject<List<NDSColumn.ColumnProduct>>(jsonContent);
                                lSuccess = bool.Parse(jsonContent);
                            }
                            else
                            {
                                if (response.StatusCode == HttpStatusCode.BadRequest)
                                {
                                    lErrorMsg = response.Content.ReadAsStringAsync().Result;
                                }
                            }
                            if (lErrorMsg.Trim().Length > 0)
                            {
                                lSuccess = false;
                                break;
                            }


                            if (lSuccess == true)
                            {
                                if ((lMWBOM.Length > 0 | lCWBOM.Length > 0) && ndsColumnProd.Count > 0)
                                {
                                    // Update BOM
                                    for (var j = 0; j < ndsColumnProd.Count; j++)
                                    {
                                        var lProcess = new ProcessController();
                                        lSuccess = lProcess.updateColumnBOM(ndsColumnProd[j].ProductMarkId, pStruEleID, lMWBOM, lCWBOM,
                                            ndsColumnProd[j].MO1, ndsColumnProd[j].MO2, ndsColumnProd[j].CO1, ndsColumnProd[j].CO2,
                                            ndsColumnProd[j].ProductionMO1, ndsColumnProd[j].ProductionMO2,
                                            ndsColumnProd[j].ProductionCO1, ndsColumnProd[j].ProductionCO2, ndsColumnProd[j].InvoiceMWQty, ndsColumnProd[j].InvoiceCWQty);

                                        lProcess = null;

                                        if (lSuccess == false)
                                        {
                                            lErrorMsg = "BOM Update Error. Please check the BOM details string.";
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                lSuccess = false;
                lErrorMsg = ex.Message;
            }
            myTransport = null;
            muEncoding = null;

            binding = null;
            address = null;
            //ndsClient = null;ajit
            ndsColumnStructure = null;
            ndsShape = null;
            ndsProd = null;
            ndsShapeParam = null;
            ndsColumnProd = null;
            ndsProdMark = null;


            return lErrorMsg;
        }

        async Task<string> SlabTransferAsync(
        int pProjectID,
        int pProdTypeID,
        int pStruEleID,
        int pSElevelID,
        string pBBSNo,
        string pBBSDesc,
        int pParam,
        List<CTSMESHOthersDetailsModels> pBBSDetails)
        {
            string lErrorMsg = "";
            bool lSuccess = true;
            string lSQL = "";
            SqlCommand lCmd;
            SqlDataReader adoRst;

            int lParentStructureMarkID = 0;

            string AmendDate;
            string lMESHMark = "";
            int lMWLen = 0;
            int lCWLen = 0;
            int lMO1 = 0;
            int lMO2 = 0;
            int lCO1 = 0;
            int lCO2 = 0;
            int lQty = 0;
            string lProduct = "";
            string lShape = "";
            int lParA, lParB, lParC, lParD, lParE, lParF;
            int lParG, lParH, lParI, lParJ, lParK, lParL;
            int lParM, lParN, lParO, lParP, lParQ, lParR;
            int lParS, lParT, lParU, lParV, lParW, lParX;
            int lParY, lParZ, lHook;

            int lStruEleID;
            int lProdTypeID = 0;

            string lGroupMark;
            int lSElevelID = 0;
            int lParam = pParam;

            string lMWBOM = "";
            string lCWBOM = "";

            string lParamValues = "";
            int lProjectID = 0;

            lSuccess = true;
            AmendDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            if (pBBSDetails.Count == 0)
            {
                lErrorMsg = "Invalide CTS mesh details.";
                return lErrorMsg;
            }

            lProjectID = pProjectID;

            lSElevelID = pSElevelID;

            lProdTypeID = pProdTypeID;

            lStruEleID = pStruEleID;

            var myTransport = new HttpTransportBindingElement();
            var muEncoding = new BinaryMessageEncodingBindingElement();

            myTransport.MaxBufferSize = 10 * 65536;
            myTransport.MaxReceivedMessageSize = 10 * 65536;
            //myTransport.AuthenticationScheme = AuthenticationSchemes.Anonymous;
            //myTransport.ProxyAuthenticationScheme = AuthenticationSchemes.Anonymous;ajit
            myTransport.UseDefaultWebProxy = true;

            var binding = new CustomBinding();
            //var binding = new BasicHttpBinding();

            binding.Name = "customBinding0";
            binding.Elements.Add(muEncoding);
            binding.Elements.Add(myTransport);

            //Specify the address to be used for the client.

            //var address = new EndpointAddress("http://172.25.1.224:81/NDSWCF_CAB/SlabService.svc");
            var address = new EndpointAddress("http://172.25.1.224:81/NDSWCF_CUBE/SlabService.svc");

            if (strServer == "PRD")
            {
                address = new EndpointAddress("http://172.25.1.141:81/NDSWCF_PV/SlabService.svc");
            }

            // Create a client that is configured with this address and binding.
            //var ndsClient = new NDSSlab.SlabServiceClient(binding, address);ajit

            var ndsSlabStructure = new NDSSlab.SlabStructure();
            var ndsShape = new NDSSlab.ShapeCode();
            var ndsShapes = new List<NDSSlab.ShapeCode>();
            var ndsShapeCodeParam = new NDSSlab.ShapeCodeParameterSet();
            var ndsShapeCodeParams = new List<NDSSlab.ShapeCodeParameterSet>();
            var ndsProd = new NDSSlab.ProductCode();
            var ndsProds = new List<NDSSlab.ProductCode>();
            var ndsShapeParam = new NDSSlab.ShapeParameter();
            var ndsShapeParamS = new List<NDSSlab.ShapeParameter>();
            var ndsSlabProd = new List<NDSSlab.SlabProduct>();
            var ndsSlabProductMarking = new NDSSlab.SlabProduct();
            var ndsSlabProductMarkingBK = new NDSSlab.SlabProduct();
            var ndsProdMark = new List<NDSSlab.SlabProduct>();

            try
            {
                // ndsClient.Open();
                lErrorMsg = "";

                if (pBBSDetails.Count > 0)
                {
                    for (int i = 0; i < pBBSDetails.Count; i++)
                    {
                        lParamValues = "";
                        lShape = pBBSDetails[i].MeshShapeCode;
                        if (lShape != null && lShape != "")
                        {
                            lShape = lShape.Trim();
                            lMESHMark = pBBSDetails[i].MeshMark == null ? "" : pBBSDetails[i].MeshMark;
                            lMWLen = (int)pBBSDetails[i].MeshMainLen;
                            lCWLen = (int)pBBSDetails[i].MeshCrossLen;
                            lMO1 = (int)pBBSDetails[i].MeshMO1;
                            lMO2 = (int)pBBSDetails[i].MeshMO2;
                            lCO1 = (int)pBBSDetails[i].MeshCO1;
                            lCO2 = (int)pBBSDetails[i].MeshCO2;
                            lQty = (int)pBBSDetails[i].MeshMemberQty;
                            lProduct = pBBSDetails[i].MeshProduct;

                            lMWBOM = pBBSDetails[i].MWBOM;
                            lCWBOM = pBBSDetails[i].CWBOM;
                            if (lMWBOM == null)
                            {
                                lMWBOM = "";
                            }
                            if (lCWBOM == null)
                            {
                                lCWBOM = "";
                            }

                            lParA = pBBSDetails[i].A == null ? 0 : (int)pBBSDetails[i].A;
                            lParB = pBBSDetails[i].B == null ? 0 : (int)pBBSDetails[i].B;
                            lParC = pBBSDetails[i].C == null ? 0 : (int)pBBSDetails[i].C;
                            lParD = pBBSDetails[i].D == null ? 0 : (int)pBBSDetails[i].D;
                            lParE = pBBSDetails[i].E == null ? 0 : (int)pBBSDetails[i].E;
                            lParF = pBBSDetails[i].F == null ? 0 : (int)pBBSDetails[i].F;
                            lParG = pBBSDetails[i].G == null ? 0 : (int)pBBSDetails[i].G;
                            lParH = pBBSDetails[i].H == null ? 0 : (int)pBBSDetails[i].H;
                            lParI = pBBSDetails[i].I == null ? 0 : (int)pBBSDetails[i].I;
                            lParJ = pBBSDetails[i].J == null ? 0 : (int)pBBSDetails[i].J;
                            lParK = pBBSDetails[i].K == null ? 0 : (int)pBBSDetails[i].K;
                            lParL = pBBSDetails[i].L == null ? 0 : (int)pBBSDetails[i].L;
                            lParM = pBBSDetails[i].M == null ? 0 : (int)pBBSDetails[i].M;
                            lParN = pBBSDetails[i].N == null ? 0 : (int)pBBSDetails[i].N;
                            lParO = pBBSDetails[i].O == null ? 0 : (int)pBBSDetails[i].O;
                            lParP = pBBSDetails[i].P == null ? 0 : (int)pBBSDetails[i].P;
                            lParQ = pBBSDetails[i].Q == null ? 0 : (int)pBBSDetails[i].Q;
                            lParR = pBBSDetails[i].R == null ? 0 : (int)pBBSDetails[i].R;
                            lParS = pBBSDetails[i].S == null ? 0 : (int)pBBSDetails[i].S;
                            lParT = pBBSDetails[i].T == null ? 0 : (int)pBBSDetails[i].T;
                            lParU = pBBSDetails[i].U == null ? 0 : (int)pBBSDetails[i].U;
                            lParV = pBBSDetails[i].V == null ? 0 : (int)pBBSDetails[i].V;
                            lParW = pBBSDetails[i].W == null ? 0 : (int)pBBSDetails[i].W;
                            lParX = pBBSDetails[i].X == null ? 0 : (int)pBBSDetails[i].X;
                            lParY = pBBSDetails[i].Y == null ? 0 : (int)pBBSDetails[i].Y;
                            lParZ = pBBSDetails[i].Z == null ? 0 : (int)pBBSDetails[i].Z;
                            lHook = pBBSDetails[i].HOOK == null ? 0 : (int)pBBSDetails[i].HOOK;

                            lErrorMsg = "";

                            ndsShape = new NDSSlab.ShapeCode();

                            // ndsShapes = ndsClient.FilterShapeCode(lShape, out lErrorMsg).ToList();ajit
                            HttpClient httpClient1 = new HttpClient();
                            bool isSuccess1;
                            string apiUrl1 = "http://172.25.1.224:89/Detailing/FilterShapeCode_column";
                            string apiUrlWithParameters1 = string.Format("{0}/{1}", apiUrl1, lShape);
                            string jsonContent1 = null;

                            HttpResponseMessage response1 = await httpClient1.GetAsync(apiUrlWithParameters1);

                            if (response1.IsSuccessStatusCode)
                            {
                                jsonContent1 = response1.Content.ReadAsStringAsync().Result;
                                ndsShapes = JsonConvert.DeserializeObject<List<NDSSlab.ShapeCode>>(jsonContent1);
                            }
                            else
                            {
                                if (response1.StatusCode == HttpStatusCode.BadRequest)
                                {
                                    lErrorMsg = response1.Content.ReadAsStringAsync().Result;
                                }
                            }
                            for (int j = 0; j < ndsShapes.Count; j++)
                            {
                                if (ndsShapes[j].ShapeCodeName == lShape)
                                {
                                    ndsShape = ndsShapes[j];
                                    break;
                                }
                            }

                            if (lShape == "F")
                            {
                                for (int j = 0; j < ndsShape.ShapeParam.Length; j++)
                                {
                                    if (ndsShape.ShapeParam[j].ParameterName == "A")
                                    {
                                        ndsShape.ShapeParam[j].ParameterValue = lMWLen;
                                        lParamValues = lParamValues + "A:" + lMWLen;
                                    }
                                }
                            }
                            else
                            {
                                for (int j = 0; j < ndsShape.ShapeParam.Length; j++)
                                {
                                    if (ndsShape.ShapeParam[j].ParameterName.ToUpper() == "A" && ndsShape.ShapeParam[j].EditFlag == true)
                                    {
                                        ndsShape.ShapeParam[j].ParameterValue = lParA;
                                        lParamValues = lParamValues + ";" + ndsShape.ShapeParam[j].ParameterName.Trim() + ":" + lParA;
                                    }
                                    else if (ndsShape.ShapeParam[j].ParameterName.ToUpper() == "B" && ndsShape.ShapeParam[j].EditFlag == true)
                                    {
                                        ndsShape.ShapeParam[j].ParameterValue = lParB;
                                        lParamValues = lParamValues + ";" + ndsShape.ShapeParam[j].ParameterName.Trim() + ":" + lParB;
                                    }
                                    else if (ndsShape.ShapeParam[j].ParameterName.ToUpper() == "C" && ndsShape.ShapeParam[j].EditFlag == true)
                                    {
                                        ndsShape.ShapeParam[j].ParameterValue = lParC;
                                        lParamValues = lParamValues + ";" + ndsShape.ShapeParam[j].ParameterName.Trim() + ":" + lParC;
                                    }
                                    else if (ndsShape.ShapeParam[j].ParameterName.ToUpper() == "D" && ndsShape.ShapeParam[j].EditFlag == true)
                                    {
                                        ndsShape.ShapeParam[j].ParameterValue = lParD;
                                        lParamValues = lParamValues + ";" + ndsShape.ShapeParam[j].ParameterName.Trim() + ":" + lParD;
                                    }
                                    else if (ndsShape.ShapeParam[j].ParameterName.ToUpper() == "E" && ndsShape.ShapeParam[j].EditFlag == true)
                                    {
                                        ndsShape.ShapeParam[j].ParameterValue = lParE;
                                        lParamValues = lParamValues + ";" + ndsShape.ShapeParam[j].ParameterName.Trim() + ":" + lParE;
                                    }
                                    else if (ndsShape.ShapeParam[j].ParameterName.ToUpper() == "F" && ndsShape.ShapeParam[j].EditFlag == true)
                                    {
                                        ndsShape.ShapeParam[j].ParameterValue = lParF;
                                        lParamValues = lParamValues + ";" + ndsShape.ShapeParam[j].ParameterName.Trim() + ":" + lParF;
                                    }
                                    else if (ndsShape.ShapeParam[j].ParameterName.ToUpper() == "G" && ndsShape.ShapeParam[j].EditFlag == true)
                                    {
                                        ndsShape.ShapeParam[j].ParameterValue = lParG;
                                        lParamValues = lParamValues + ";" + ndsShape.ShapeParam[j].ParameterName.Trim() + ":" + lParG;
                                    }
                                    else if (ndsShape.ShapeParam[j].ParameterName.ToUpper() == "H" && ndsShape.ShapeParam[j].EditFlag == true)
                                    {
                                        ndsShape.ShapeParam[j].ParameterValue = lParH;
                                        lParamValues = lParamValues + ";" + ndsShape.ShapeParam[j].ParameterName.Trim() + ":" + lParH;
                                    }
                                    else if (ndsShape.ShapeParam[j].ParameterName.ToUpper() == "I" && ndsShape.ShapeParam[j].EditFlag == true)
                                    {
                                        ndsShape.ShapeParam[j].ParameterValue = lParI;
                                        lParamValues = lParamValues + ";" + ndsShape.ShapeParam[j].ParameterName.Trim() + ":" + lParI;
                                    }
                                    else if (ndsShape.ShapeParam[j].ParameterName.ToUpper() == "J" && ndsShape.ShapeParam[j].EditFlag == true)
                                    {
                                        ndsShape.ShapeParam[j].ParameterValue = lParJ;
                                        lParamValues = lParamValues + ";" + ndsShape.ShapeParam[j].ParameterName.Trim() + ":" + lParJ;
                                    }
                                    else if (ndsShape.ShapeParam[j].ParameterName.ToUpper() == "K" && ndsShape.ShapeParam[j].EditFlag == true)
                                    {
                                        ndsShape.ShapeParam[j].ParameterValue = lParK;
                                        lParamValues = lParamValues + ";" + ndsShape.ShapeParam[j].ParameterName.Trim() + ":" + lParK;
                                    }
                                    else if (ndsShape.ShapeParam[j].ParameterName.ToUpper() == "L" && ndsShape.ShapeParam[j].EditFlag == true)
                                    {
                                        ndsShape.ShapeParam[j].ParameterValue = lParL;
                                        lParamValues = lParamValues + ";" + ndsShape.ShapeParam[j].ParameterName.Trim() + ":" + lParL;
                                    }
                                    else if (ndsShape.ShapeParam[j].ParameterName.ToUpper() == "M" && ndsShape.ShapeParam[j].EditFlag == true)
                                    {
                                        ndsShape.ShapeParam[j].ParameterValue = lParM;
                                        lParamValues = lParamValues + ";" + ndsShape.ShapeParam[j].ParameterName.Trim() + ":" + lParM;
                                    }
                                    else if (ndsShape.ShapeParam[j].ParameterName.ToUpper() == "N" && ndsShape.ShapeParam[j].EditFlag == true)
                                    {
                                        ndsShape.ShapeParam[j].ParameterValue = lParN;
                                        lParamValues = lParamValues + ";" + ndsShape.ShapeParam[j].ParameterName.Trim() + ":" + lParN;
                                    }
                                    else if (ndsShape.ShapeParam[j].ParameterName.ToUpper() == "O" && ndsShape.ShapeParam[j].EditFlag == true)
                                    {
                                        ndsShape.ShapeParam[j].ParameterValue = lParO;
                                        lParamValues = lParamValues + ";" + ndsShape.ShapeParam[j].ParameterName.Trim() + ":" + lParO;
                                    }
                                    else if (ndsShape.ShapeParam[j].ParameterName.ToUpper() == "P" && ndsShape.ShapeParam[j].EditFlag == true)
                                    {
                                        ndsShape.ShapeParam[j].ParameterValue = lParP;
                                        lParamValues = lParamValues + ";" + ndsShape.ShapeParam[j].ParameterName.Trim() + ":" + lParP;
                                    }
                                    else if (ndsShape.ShapeParam[j].ParameterName.ToUpper() == "Q" && ndsShape.ShapeParam[j].EditFlag == true)
                                    {
                                        ndsShape.ShapeParam[j].ParameterValue = lParQ;
                                        lParamValues = lParamValues + ";" + ndsShape.ShapeParam[j].ParameterName.Trim() + ":" + lParQ;
                                    }
                                    else if (ndsShape.ShapeParam[j].ParameterName.ToUpper() == "R" && ndsShape.ShapeParam[j].EditFlag == true)
                                    {
                                        ndsShape.ShapeParam[j].ParameterValue = lParR;
                                        lParamValues = lParamValues + ";" + ndsShape.ShapeParam[j].ParameterName.Trim() + ":" + lParR;
                                    }
                                    else if (ndsShape.ShapeParam[j].ParameterName.ToUpper() == "S" && ndsShape.ShapeParam[j].EditFlag == true)
                                    {
                                        ndsShape.ShapeParam[j].ParameterValue = lParS;
                                        lParamValues = lParamValues + ";" + ndsShape.ShapeParam[j].ParameterName.Trim() + ":" + lParS;
                                    }
                                    else if (ndsShape.ShapeParam[j].ParameterName.ToUpper() == "T" && ndsShape.ShapeParam[j].EditFlag == true)
                                    {
                                        ndsShape.ShapeParam[j].ParameterValue = lParT;
                                        lParamValues = lParamValues + ";" + ndsShape.ShapeParam[j].ParameterName.Trim() + ":" + lParT;
                                    }
                                    else if (ndsShape.ShapeParam[j].ParameterName.ToUpper() == "U" && ndsShape.ShapeParam[j].EditFlag == true)
                                    {
                                        ndsShape.ShapeParam[j].ParameterValue = lParU;
                                        lParamValues = lParamValues + ";" + ndsShape.ShapeParam[j].ParameterName.Trim() + ":" + lParU;
                                    }
                                    else if (ndsShape.ShapeParam[j].ParameterName.ToUpper() == "V" && ndsShape.ShapeParam[j].EditFlag == true)
                                    {
                                        ndsShape.ShapeParam[j].ParameterValue = lParV;
                                        lParamValues = lParamValues + ";" + ndsShape.ShapeParam[j].ParameterName.Trim() + ":" + lParV;
                                    }
                                    else if (ndsShape.ShapeParam[j].ParameterName.ToUpper() == "W" && ndsShape.ShapeParam[j].EditFlag == true)
                                    {
                                        ndsShape.ShapeParam[j].ParameterValue = lParW;
                                        lParamValues = lParamValues + ";" + ndsShape.ShapeParam[j].ParameterName.Trim() + ":" + lParW;
                                    }
                                    else if (ndsShape.ShapeParam[j].ParameterName.ToUpper() == "X" && ndsShape.ShapeParam[j].EditFlag == true)
                                    {
                                        ndsShape.ShapeParam[j].ParameterValue = lParX;
                                        lParamValues = lParamValues + ";" + ndsShape.ShapeParam[j].ParameterName.Trim() + ":" + lParX;
                                    }
                                    else if (ndsShape.ShapeParam[j].ParameterName.ToUpper() == "Y" && ndsShape.ShapeParam[j].EditFlag == true)
                                    {
                                        ndsShape.ShapeParam[j].ParameterValue = lParY;
                                        lParamValues = lParamValues + ";" + ndsShape.ShapeParam[j].ParameterName.Trim() + ":" + lParY;
                                    }
                                    else if (ndsShape.ShapeParam[j].ParameterName.ToUpper() == "Z" && ndsShape.ShapeParam[j].EditFlag == true)
                                    {
                                        ndsShape.ShapeParam[j].ParameterValue = lParZ;
                                        lParamValues = lParamValues + ";" + ndsShape.ShapeParam[j].ParameterName.Trim() + ":" + lParZ;
                                    }
                                    else if (ndsShape.ShapeParam[j].ParameterName.ToUpper() == "HOOK" && ndsShape.ShapeParam[j].EditFlag == true)
                                    {
                                        ndsShape.ShapeParam[j].ParameterValue = lParZ;
                                        lParamValues = lParamValues + ";" + ndsShape.ShapeParam[j].ParameterName.Trim() + ":" + lHook;
                                    }
                                }
                            }
                            ndsProd = new NDSSlab.ProductCode();

                            //ndsProds = ndsClient.FilterProductCode(lProduct, out lErrorMsg).ToList();ajit
                            HttpClient httpClientProductCode = new HttpClient();
                            bool isSuccessProductCode;
                            string apiUrlProductCode = "http://172.25.1.224:89/Detailing/FilterCapProductCode";
                            string apiUrlWithParametersProductCode = string.Format("{0}/{1}", apiUrlProductCode, lProduct);
                            string jsonContentProductCode = null;

                            HttpResponseMessage responseProductCode = await httpClientProductCode.GetAsync(apiUrlWithParametersProductCode);

                            if (responseProductCode.IsSuccessStatusCode)
                            {
                                jsonContentProductCode = responseProductCode.Content.ReadAsStringAsync().Result;
                                ndsProds = JsonConvert.DeserializeObject<List<NDSSlab.ProductCode>>(jsonContentProductCode);
                            }

                            for (int j = 0; j < ndsProds.Count; j++)
                            {
                                if (lProduct.Trim() == ndsProds[j].ProductCodeName.Trim())
                                {
                                    ndsProd = ndsProds[j];
                                    break;
                                }
                            }

                            //Check Mesh Lap setting and add lap for the prod code
                            int lVar = 0;
                            lSQL = "Select PA.tntParamSetNumber " +
                                "FROM dbo.ProjectParamLap LP, " +
                                "dbo.ProjectParameter PA, " +
                                "dbo.ProductCodeMaster PC " +
                                "WHERE LP.tntParamSetNumber = PA.tntParamSetNumber  " +
                                "AND LP.intProductCodeId = PC.intProductCodeId  " +
                                "AND sitProductTypeId = " + lProdTypeID + " " +
                                "AND LP.intStructureElementTypeId = " + lStruEleID.ToString() + "  " +
                                "AND LP.intProjectId = 0" + lProjectID + " " +
                                "AND PA.tntParamSetNumber =  " + lParam.ToString() + " " +
                                "AND PC.vchProductCode = '" + lProduct + "' ";

                            lCmd = new SqlCommand(lSQL, cnNDS);
                            lCmd.Transaction = osqlTransNDS;
                            lCmd.CommandTimeout = 1200;
                            adoRst = lCmd.ExecuteReader();
                            if (adoRst.HasRows)
                            {
                                adoRst.Read();
                                lVar = (Int32)adoRst.GetValue(0);
                            }
                            adoRst.Close();

                            if (lVar == 0)
                            {
                                lSQL = "INSERT INTO dbo.ProjectParamLap " +
                                    "(intProjectId " +
                                    ",tntParamSetNumber " +
                                    ",sitProductTypeL2Id " +
                                    ",intStructureElementTypeId " +
                                    ",intProductCodeId " +
                                    ",intConsignmentType " +
                                    ",sitMWLap " +
                                    ",sitCWLap " +
                                    ",tntStatusId " +
                                    ",intCreatedUId " +
                                    ",datCreatedDate " +
                                    ",intUpdatedUId " +
                                    ",datUpdatedDate) " +
                                    "VALUES " +
                                    "(" + lProjectID + " " +
                                    "," + lParam + " " +
                                    ",0 " +
                                    "," + lStruEleID + " " +
                                    "," + ndsProd.ProductCodeId + " " +
                                    ",6 " +
                                    ",0 " +
                                    ",0 " +
                                    ",1 " +
                                    ",111 " +
                                    ",getDate() " +
                                    ",111 " +
                                    ",getDate() )  ";
                                lCmd = new SqlCommand(lSQL, cnNDS);
                                lCmd.Transaction = osqlTransNDS;
                                lCmd.CommandTimeout = 1200;
                                lCmd.ExecuteNonQuery();
                            }

                            //check project parameter overhang
                            if (ndsProd.MainWireSpacing > 0 && ndsProd.CrossWireSpacing > 0)
                            {
                                lVar = 0;
                                lSQL = "Select PA.tntParamSetNumber " +
                                    "FROM dbo.ProjectParamOH OH, " +
                                    "dbo.ProjectParameter PA " +
                                    "WHERE OH.tntParamSetNumber = PA.tntParamSetNumber  " +
                                    "AND sitProductTypeId = " + lProdTypeID + " " +
                                    "AND OH.intStructureElementTypeId = " + lStruEleID.ToString() + "  " +
                                    "AND OH.intProjectId = 0" + lProjectID + " " +
                                    "AND PA.tntParamSetNumber = " + lParam + " " +
                                    "AND intConsignmentType = 6 " +
                                    "AND OH.intMWSpace = " + ndsProd.MainWireSpacing + " " +
                                    "AND OH.intCWSpace = " + ndsProd.CrossWireSpacing + " ";

                                lCmd = new SqlCommand(lSQL, cnNDS);
                                lCmd.Transaction = osqlTransNDS;
                                lCmd.CommandTimeout = 1200;
                                adoRst = lCmd.ExecuteReader();
                                if (adoRst.HasRows)
                                {
                                    adoRst.Read();
                                    lVar = (Int32)adoRst.GetValue(0);
                                }
                                adoRst.Close();

                                if (lVar == 0)
                                {
                                    lSQL = "INSERT INTO dbo.ProjectParamOH " +
                                    "(intProjectId " +
                                    ",intStructureElementTypeId " +
                                    ",sitProductTypeL2Id " +
                                    ",intConsignmentType " +
                                    ",tntParamSetNumber " +
                                    ",intMWSpace " +
                                    ",intCWSpace " +
                                    ",intEvenMO1 " +
                                    ",intEvenMO2 " +
                                    ",intEvenCO1 " +
                                    ",intEvenCO2 " +
                                    ",intOddMO1 " +
                                    ",intOddMO2 " +
                                    ",intOddCO1 " +
                                    ",intOddCO2 " +
                                    ",tntStatusId " +
                                    ",intCreatedUID " +
                                    ",datCreatedDate " +
                                    ",intUpdatedUID " +
                                    ",datUpdatedDate) " +
                                    "VALUES " +
                                    "(" + lProjectID + " " +
                                    "," + lStruEleID + " " +
                                    ",0 " +
                                    ",6 " +
                                    "," + lParam + " " +
                                    "," + ndsProd.MainWireSpacing + " " +
                                    "," + ndsProd.CrossWireSpacing + " " +
                                    ",100 " +
                                    ",100 " +
                                    ",100 " +
                                    ",100 " +
                                    ",100 " +
                                    ",100 " +
                                    ",100 " +
                                    ",100 " +
                                    ",1 " +
                                    ",111 " +
                                    ",getDate() " +
                                    ",111 " +
                                    ",getDate() )  ";

                                    lCmd = new SqlCommand(lSQL, cnNDS);
                                    lCmd.Transaction = osqlTransNDS;
                                    lCmd.CommandTimeout = 1200;
                                    lCmd.ExecuteNonQuery();
                                }
                            }

                            ndsShapeCodeParam = new NDSSlab.ShapeCodeParameterSet();

                            ndsShapeCodeParam.BottomCover = 0;
                            ndsShapeCodeParam.TopCover = 0;
                            ndsShapeCodeParam.LeftCover = 0;
                            ndsShapeCodeParam.RightCover = 0;
                            ndsShapeCodeParam.CWLap = 0;
                            ndsShapeCodeParam.MWLap = 0;
                            ndsShapeCodeParam.productCode = ndsProd;
                            ndsShapeCodeParam.ParameterSetNumber = lParam;
                            ndsShapeCodeParam.ParameterSetValue = 1;

                            // ndsShapeCodeParams = ndsClient.SlabParameterSetbyProjIdProdType(lProjectID, lProdTypeID, out lErrorMsg).ToList();ajit
                            HttpClient httpClient2 = new HttpClient();
                            bool isSuccess2;
                            string apiUrl2 = "http://172.25.1.224:89/Detailing/GetSlabParameterSetbyProjIdProdType";


                            string apiUrlWithParameters2 = string.Format("{0}/{1}/{2}", apiUrl2, lProjectID, lProdTypeID);
                            string jsonContent2 = null;

                            HttpResponseMessage response2 = await httpClient2.GetAsync(apiUrlWithParameters2);

                            if (response2.IsSuccessStatusCode)
                            {
                                jsonContent2 = response2.Content.ReadAsStringAsync().Result;
                                ndsShapeCodeParams = JsonConvert.DeserializeObject<List<NDSSlab.ShapeCodeParameterSet>>(jsonContent2);
                            }
                            else
                            {
                                if (response2.StatusCode == HttpStatusCode.BadRequest)
                                {
                                    lErrorMsg = response2.Content.ReadAsStringAsync().Result;
                                }
                            }

                            for (int j = 0; j < ndsShapeCodeParams.Count; j++)
                            {
                                if (ndsShapeCodeParams[j].ParameterSetNumber == lParam)
                                {
                                    ndsShapeCodeParam = ndsShapeCodeParams[j];
                                    break;
                                }
                            }

                            ndsProdMark.Add(new NDSSlab.SlabProduct());
                            ndsProdMark[0].CO1 = lCO1;
                            ndsProdMark[0].CO2 = lCO2;
                            ndsProdMark[0].MO1 = lMO1;
                            ndsProdMark[0].MO2 = lMO2;
                            ndsProdMark[0].MWLength = lMWLen;
                            ndsProdMark[0].CWLength = lCWLen;
                            ndsProdMark[0].ProductionMWLength = lMWLen;
                            ndsProdMark[0].ProductionCWLength = lCWLen;
                            ndsProdMark[0].shapecode = ndsShape;
                            ndsProdMark[0].ParamValues = lParamValues;
                            ndsProdMark[0].BOMIndicator = "S";
                            ndsProdMark[0].ProductionCO1 = lCO1;
                            ndsProdMark[0].ProductionCO2 = lCO2;
                            ndsProdMark[0].ProductionMO1 = lMO1;
                            ndsProdMark[0].ProductionMO2 = lMO2;
                            ndsProdMark[0].ProductMarkingName = lMESHMark;
                            ndsProdMark[0].MemberQty = 1;

                            ndsSlabStructure = new NDSSlab.SlabStructure();
                            ndsSlabStructure.StructureMarkingName = lMESHMark;
                            ndsSlabStructure.StructureMarkId = 0;
                            ndsSlabStructure.BendingCheck = false;
                            ndsSlabStructure.CrossWireLength = lCWLen;
                            ndsSlabStructure.MachineCheck = false;
                            ndsSlabStructure.MainWireLength = lMWLen;
                            ndsSlabStructure.MemberQty = lQty;
                            ndsSlabStructure.MultiMesh = false;
                            ndsSlabStructure.ParameterSet = ndsShapeCodeParam;
                            ndsSlabStructure.ParamSetNumber = lParam;
                            ndsSlabStructure.ParentStructureMarkId = lParentStructureMarkID;
                            ndsSlabStructure.PinSize = 32;
                            ndsSlabStructure.ProduceIndicator = true;
                            ndsSlabStructure.ProductCode = ndsProd;

                            ndsSlabStructure.ProductSplitUp = true;

                            ndsSlabStructure.SEDetailingID = lSElevelID;
                            //ndsSlabStructure.SlabProduct = ndsProdMark.ToArray();
                            ndsSlabStructure.SlabProduct = ndsProdMark;
                            ndsSlabStructure.TransportCheck = false;
                            ndsSlabStructure.ProductGenerationStatus = true;
                            //ndsSlabProd = ndsClient.InsertSlabStructureMarking(ref ndsSlabStructure, lStruEleID, 1, lProjectID, lProdTypeID, 111, out lErrorMsg).ToList();ajit

                            HttpClient httpClient = new HttpClient();
                            bool isSuccess;
                            string apiUrl = "http://172.25.1.224:89/Detailing/InsertSlabStructureMarking";
                            // Define your dictionary and populate it with key-value pairs if needed
                            // Dictionary<string, string> parameters = new Dictionary<string, string>();

                            string jsonData = JsonConvert.SerializeObject(ndsSlabStructure);
                            HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                            string apiUrlWithParameters = string.Format("{0}/{1}/{2}/{3}/{4}", apiUrl, lStruEleID, 1, lProjectID, lProdTypeID, 111);
                            string jsonContent = null;

                            HttpResponseMessage response = httpClient.PostAsync(apiUrlWithParameters, content).Result;

                            if (response.IsSuccessStatusCode)
                            {
                                jsonContent = response.Content.ReadAsStringAsync().Result;
                                ndsSlabProd = JsonConvert.DeserializeObject<List<NDSSlab.SlabProduct>>(jsonContent);
                            }
                            else
                            {
                                if (response.StatusCode == HttpStatusCode.BadRequest)
                                {
                                    lErrorMsg = response.Content.ReadAsStringAsync().Result;
                                }
                            }
                            if (lErrorMsg.Trim().Length > 0)
                            {
                                lSuccess = false;
                                break;
                            }
                           

                            if (ndsSlabProd.Count > 0)
                            {
                                for (int j = 0; j < ndsSlabProd.Count; j++)
                                {
                                    ndsSlabProductMarking = ndsSlabProd[j];
                                    ndsSlabProductMarkingBK = ndsSlabProd[j];
                                    ndsSlabStructure.StructureMarkId = ndsSlabProductMarking.StructureMarkId;

                                    ndsSlabProductMarking.CO1 = lCO1;
                                    ndsSlabProductMarking.CO2 = lCO2;
                                    ndsSlabProductMarking.MO1 = lMO1;
                                    ndsSlabProductMarking.MO2 = lMO2;
                                    ndsSlabProductMarking.shapecode = ndsShape;
                                    ndsSlabProductMarking.ParamValues = lParamValues;
                                    ndsSlabProductMarking.BOMIndicator = "S";
                                    ndsSlabProductMarking.ProductionCO1 = lCO1;
                                    ndsSlabProductMarking.ProductionCO2 = lCO2;
                                    ndsSlabProductMarking.ProductionMO1 = lMO1;
                                    ndsSlabProductMarking.ProductionMO2 = lMO2;
                                    ndsSlabProductMarking.ProductionMWLength = lMWLen;
                                    ndsSlabProductMarking.ProductionCWLength = lCWLen;
                                    ndsSlabProductMarking.BendingCheckInd = false;
                                    ndsSlabProductMarking.CWLength = lCWLen;
                                    ndsSlabProductMarking.MWLength = lMWLen;
                                    ndsSlabProductMarking.MemberQty = 1;

                                    ndsSlabProd[j] = ndsSlabProductMarking;
                                    ndsSlabStructure.SlabProduct[j] = ndsSlabProductMarking;

                                    if (lShape == "F")
                                    {
                                        if (j == 0)
                                        {
                                            var lMachineCheck = true;
                                            var lBendingCheck = "";
                                            var lTrandportCheck = true;
                                            lVar = 0;
                                            ndsSlabProductMarking.NoOfCrossWire = ((lMWLen - lMO1 - lMO2) / ndsSlabProductMarking.CWSpacing) + 1;
                                            ndsSlabProductMarking.ProductionCWQty = ((lMWLen - lMO1 - lMO2) / ndsSlabProductMarking.CWSpacing) + 1;

                                            ndsSlabProductMarking.NoOfMainWire = ((lCWLen - lCO1 - lCO2) / ndsSlabProductMarking.MWSpacing) + 1;
                                            ndsSlabProductMarking.ProductionMWQty = ((lCWLen - lCO1 - lCO2) / ndsSlabProductMarking.MWSpacing) + 1;

                                            //ndsSlabProductMarking = ndsClient.SaveProductMarkafterConfirmation(ndsSlabStructure, ndsSlabProductMarking, j, lStruEleID, 111, lErrorMsg)
                                            // ndsSlabProductMarking = ndsClient.UpdateSlabProductMarking(ndsSlabStructure, ref ndsSlabProductMarking, ref lVar, lStruEleID, 111, out lMachineCheck, out lBendingCheck, out lTrandportCheck, out lErrorMsg);ajit
                                            HttpClient httpClient4 = new HttpClient();
                                            bool isSuccess4;
                                            string apiUrl4 = "http://172.25.1.224:89/DetailingDetailing/UpdateSlabProductMarking";
                                            // Define your dictionary and populate it with key-value pairs if needed
                                            // Dictionary<string, string> parameters = new Dictionary<string, string>();

                                            string jsonData4 = JsonConvert.SerializeObject(ndsSlabStructure);
                                            HttpContent content4 = new StringContent(jsonData4, Encoding.UTF8, "application/json");

                                            string apiUrlWithParameters4 = string.Format("{0}/{1}/{2}", apiUrl4, lStruEleID, 111);
                                            string jsonContent4 = null;

                                            HttpResponseMessage response4 = httpClient.PostAsync(apiUrlWithParameters4, content4).Result;

                                            if (response4.IsSuccessStatusCode)
                                            {
                                                jsonContent = response4.Content.ReadAsStringAsync().Result;
                                                ndsSlabProductMarking = JsonConvert.DeserializeObject<NDSSlab.SlabProduct>(jsonContent);//ajit
                                            }
                                            else
                                            {
                                                if (response4.StatusCode == HttpStatusCode.BadRequest)
                                                {
                                                    lErrorMsg = response4.Content.ReadAsStringAsync().Result;
                                                }
                                            }
                                            if (lErrorMsg.Trim().Length > 0)
                                            {
                                                lSuccess = false;
                                                break;
                                            }

                                        }
                                        else
                                        {
                                            //ndsClient.DeleteProductMarking(ndsSlabProductMarkingBK, lSElevelID, out lErrorMsg);ajit
                                            HttpClient httpClient3 = new HttpClient();
                                            bool isSuccess3;
                                            string apiUrl3 = "http://172.25.1.224:89/Detailing/DeleteProductMarking";
                                            // Define your dictionary and populate it with key-value pairs if needed
                                            // Dictionary<string, string> parameters = new Dictionary<string, string>();

                                            string jsonData3 = JsonConvert.SerializeObject(ndsSlabProductMarkingBK);
                                            HttpContent content3 = new StringContent(jsonData3, Encoding.UTF8, "application/json");

                                            string apiUrlWithParameters3 = string.Format("{0}/{1}", apiUrl3, lSElevelID);
                                            string jsonContent3 = null;

                                            HttpResponseMessage response3 = httpClient.PostAsync(apiUrlWithParameters3, content3).Result;

                                            if (response3.IsSuccessStatusCode)
                                            {
                                                jsonContent = response3.Content.ReadAsStringAsync().Result;
                                                isSuccess = JsonConvert.DeserializeObject<bool>(jsonContent3);
                                            }
                                            else
                                            {
                                                if (response3.StatusCode == HttpStatusCode.BadRequest)
                                                {
                                                    lErrorMsg = response3.Content.ReadAsStringAsync().Result;
                                                }
                                            }
                                            if (lErrorMsg.Trim().Length > 0)
                                            {
                                                lSuccess = false;
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (j == 0)
                                        {
                                            var lMachineCheck = true;
                                            var lBendingCheck = "";
                                            var lTrandportCheck = true;
                                            lVar = 0;
                                            //ndsSlabProductMarking = ndsClient.UpdateSlabProductMarking(ndsSlabStructure, ref ndsSlabProductMarking, ref lVar, lStruEleID, 111, out lMachineCheck, out lBendingCheck, out lTrandportCheck, out lErrorMsg);
                                            HttpClient httpClient6 = new HttpClient();
                                            bool isSuccess6;
                                            string apiUrl6 = "http://172.25.1.224:89/Detailing/UpdateSlabProductMarking";
                                            // Define your dictionary and populate it with key-value pairs if needed
                                            // Dictionary<string, string> parameters = new Dictionary<string, string>();

                                            string jsonData6 = JsonConvert.SerializeObject(ndsSlabStructure);
                                            HttpContent content6 = new StringContent(jsonData6, Encoding.UTF8, "application/json");

                                            string apiUrlWithParameters6 = string.Format("{0}/{1}/{2}", apiUrl6, lStruEleID, 111);
                                            string jsonContent6 = null;

                                            HttpResponseMessage response6 = httpClient.PostAsync(apiUrlWithParameters6, content6).Result;

                                            if (response6.IsSuccessStatusCode)
                                            {
                                                jsonContent = response6.Content.ReadAsStringAsync().Result;
                                                ndsSlabProductMarking = JsonConvert.DeserializeObject<NDSSlab.SlabProduct>(jsonContent6);
                                            }
                                            else
                                            {
                                                if (response6.StatusCode == HttpStatusCode.BadRequest)
                                                {
                                                    lErrorMsg = response6.Content.ReadAsStringAsync().Result;
                                                }
                                            }
                                            if (lErrorMsg.Trim().Length > 0)
                                            {
                                                lSuccess = false;
                                                break;
                                            }
                                            if (lErrorMsg.Trim().Length > 0 && lErrorMsg != "Bend Spacing constraint is not defined.")
                                            {
                                                lSuccess = false;
                                                break;
                                            }
                                            

                                            var lNewShape = "";
                                            lSQL = "SELECT chrShapeCode " +
                                            "FROM dbo.ShapeMaster " +
                                            "WHERE intShapeID = " + ndsSlabProductMarking.shapecode.ShapeID.ToString() + " ";

                                            lCmd = new SqlCommand(lSQL, cnNDS);
                                            lCmd.Transaction = osqlTransNDS;
                                            lCmd.CommandTimeout = 1200;
                                            adoRst = lCmd.ExecuteReader();
                                            if (adoRst.HasRows)
                                            {
                                                adoRst.Read();
                                                lNewShape = adoRst.GetString(0);
                                            }
                                            adoRst.Close();
                                            if (lNewShape.Trim() == "F")
                                            {
                                                //MsgBox("Data Transfer Failed. Please check overhang and bending parameter as bending check failed in NDS.")
                                                lSuccess = false;
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            //ndsClient.DeleteProductMarking(ndsSlabProductMarkingBK, lSElevelID, out lErrorMsg);ajit
                                            HttpClient httpClient7 = new HttpClient();
                                            bool isSuccess7;
                                            string apiUrl7 = "http://172.25.1.224:89/Detailing/DeleteProductMarking";
                                            // Define your dictionary and populate it with key-value pairs if needed
                                            // Dictionary<string, string> parameters = new Dictionary<string, string>();

                                            string jsonData7 = JsonConvert.SerializeObject(ndsSlabProductMarkingBK);
                                            HttpContent content7 = new StringContent(jsonData, Encoding.UTF8, "application/json");

                                            string apiUrlWithParameters7 = string.Format("{0}/{1}", apiUrl, lSElevelID);
                                            string jsonContent7 = null;

                                            HttpResponseMessage response7 = httpClient.PostAsync(apiUrlWithParameters7, content).Result;

                                            if (response7.IsSuccessStatusCode)
                                            {
                                                jsonContent = response7.Content.ReadAsStringAsync().Result;
                                                isSuccess = JsonConvert.DeserializeObject<bool>(jsonContent7);
                                            }
                                            else
                                            {
                                                if (response7.StatusCode == HttpStatusCode.BadRequest)
                                                {
                                                    lErrorMsg = response7.Content.ReadAsStringAsync().Result;
                                                }
                                            }

                                            if (lErrorMsg.Trim().Length > 0)
                                            {
                                                lSuccess = false;
                                                break;
                                            }
                                        }
                                    }
                                    ndsSlabStructure.SlabProduct[j] = ndsSlabProductMarking;
                                    ndsSlabStructure.StructureMarkId = ndsSlabProductMarking.StructureMarkId;

                                    if (lSuccess == true)
                                    {
                                        if (j == 0 & (lMWBOM.Length > 0 | lCWBOM.Length > 0))
                                        {
                                            // Update BOM
                                            var lProcess = new ProcessController();
                                            lSuccess = lProcess.updateSlabBOM(ndsSlabProductMarking.ProductMarkId, lStruEleID, lMWBOM, lCWBOM, ndsSlabProductMarking.MO1, ndsSlabProductMarking.MO2, ndsSlabProductMarking.CO1, ndsSlabProductMarking.CO2, ndsSlabProductMarking.ProductionMO1, ndsSlabProductMarking.ProductionMO2, ndsSlabProductMarking.ProductionCO1, ndsSlabProductMarking.ProductionCO2, ndsSlabProductMarking.ProductionMWQty, ndsSlabProductMarking.ProductionCWQty);
                                            lProcess = null;
                                            if (lSuccess == false)
                                            {
                                                lErrorMsg = "BOM Update Error. Please check the BOM details string.";
                                                break;
                                            }
                                        }
                                    }

                                }
                            }
                        }
                    }
                    //Post the group marking
                    //var lGroupMarkList = new List<string>();
                    //var lVerList = new List<int>();
                    //var lQtyList = new List<int>();
                    //lGroupMarkList.Add(lGroupMark);
                    //lVerList.Add(lVersion);
                    //lQtyList.Add(1);

                    //if (lProdType != "PRC")
                    //{
                    //    var lReturn = BBSPosting(pCustomerCode, pProjectCode, pContract, pJobID, pBBSID, pStruEle, pProdType, pWBS1, pWBS2, pWBS3, pBBSDesc, lGroupMarkList, lVerList, lQtyList);
                    //    if (lReturn < 0)
                    //    {
                    //        lSuccess = false;
                    //    }
                    //}
                }
            }

            catch (Exception ex)
            {
                lErrorMsg = ex.Message;
                lSuccess = false;
            }
            myTransport = null;
            muEncoding = null;

            binding = null;
            address = null;
            //ndsClient = null;
            ndsSlabStructure = null;
            ndsShape = null;
            ndsProd = null;
            ndsShapeParam = null;
            ndsSlabProd = null;
            ndsProdMark = null;

            return lErrorMsg;
        }

        public async Task<string> BBSPostingAsync(
            int pPostHeaderID,
            int pProjectID,
            int pProdTypeID,
            int pStruEleID,
            int pWBSElementID,
            string pBBSNo,
            string pBBSDesc,
            string pUserName,
            List<string> pGroupMarking,
            List<int> pGroupMarkingID,
            List<int> pRev,
            List<int> pQty)
        {
            bool lSuccess = false;
            string lReturn = "";
            int lStatus, liVar;
            int lUserID;
            string lModular;

            string lSQL;
            SqlCommand lCmd;
            SqlDataReader adoRst;

            string AmendDate, lErrorMsg;

            List<NDSPosting.PostGroupMark> ndsGroupMarkings;
            NDSPosting.PostGroupMark ndsGM;

            string lResult;
            int lRet1, lRet2;

            var myTransport = new HttpTransportBindingElement();
            var muEncoding = new BinaryMessageEncodingBindingElement();

            var binding = new CustomBinding();
            //var binding = new BasicHttpBinding();

            //Specify the address to be used for the client.

            EndpointAddress address;
            var ndsCAPCLinks = new List<NDSPosting.CapClink>();
            List<NDSPosting.CapClink> ndsCAPCLinkList;

            //Create a client that is configured with this address and binding.
            //var ndsClient = new NDSPosting.BBSPostingServiceClient();ajit


            var ndsPostedGMs = new List<NDSPosting.PostGroupMark>();

            lSuccess = true;
            AmendDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            try
            {
                lStatus = 0;
                lUserID = 11;

                if (pUserName != null && pUserName.Split('@').Length == 2)
                {
                    lSQL = "SELECT intUserId FROM dbo.NDSUsers WHERE vchLoginId = '" + pUserName.Split('@')[0] + "' ";
                    lCmd = new SqlCommand(lSQL, cnNDS);
                    lCmd.Transaction = osqlTransNDS;
                    lCmd.CommandTimeout = 1200;
                    adoRst = lCmd.ExecuteReader();
                    if (adoRst.HasRows)
                    {
                        if (adoRst.Read())
                        {
                            lUserID = adoRst.GetValue(0) == DBNull.Value ? 111 : adoRst.GetInt32(0);
                        }
                    }
                    adoRst.Close();
                }


                if (pBBSNo.Length == 0)
                {
                    lReturn = "Invalid BBS No.";
                    return lReturn;
                }

                // Check the header ID
                if (pPostHeaderID == 0)
                {
                    lReturn = "Invalid Post Heaader ID.";
                    return lReturn;
                }

                lSQL = "SELECT tntStatusId FROM dbo.BBSPostHeader WHERE intPostHeaderid = " + pPostHeaderID + " ";

                lCmd = new SqlCommand(lSQL, cnNDS);
                lCmd.Transaction = osqlTransNDS;
                lCmd.CommandTimeout = 1200;
                adoRst = lCmd.ExecuteReader();
                if (adoRst.HasRows)
                {
                    if (adoRst.Read())
                    {
                        lStatus = adoRst.GetValue(0) == DBNull.Value ? 0 : adoRst.GetByte(0);
                    }
                }
                adoRst.Close();

                lSQL = "SELECT tntStatusId FROM dbo.BBSReleaseDetails WHERE intPostHeaderid = " + pPostHeaderID + " ";

                lCmd = new SqlCommand(lSQL, cnNDS);
                lCmd.Transaction = osqlTransNDS;
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

                if (lStatus == 12)
                {
                    //Released
                    lReturn = "The BBS has been released. You cannot post it it again.";
                    return lReturn;
                }

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
                //ndsClient = new NDSPosting.BBSPostingServiceClient(binding, address);ajit


                //ndsClient.Open();ajit
                lErrorMsg = "";
                lModular = "N";

                if (lStatus == 3)
                {
                    //Posted
                    //lSuccess = ndsClient.UnPostBBS(pPostHeaderID.ToString(), out lErrorMsg);
                    using (var client = new HttpClient())
                    {
                        try
                        {
                            // Define the API endpoint URL
                            string apiUrl = "http://172.25.1.224:91/UnPostBBS_Update/" + pPostHeaderID; // Replace with your API URL

                            // Send an HTTP GET request
                            HttpResponseMessage response = await client.GetAsync(apiUrl);

                            // Check if the request was successful
                            if (response.IsSuccessStatusCode)
                            {
                                // Read the response content as a string
                                string responseContent = await response.Content.ReadAsStringAsync();
                                lSuccess = bool.Parse(responseContent);
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
                        //}
                        lReturn = "Error on unpost BBS: " + lErrorMsg;
                        return lReturn;
                    }
                }

                ndsGroupMarkings = new List<NDSPosting.PostGroupMark>();
                if (pGroupMarking.Count > 0)
                {
                    //Delete the original 
                    lSQL = "DELETE FROM dbo.PostGroupMarkingDetails " +
                    "WHERE intPostHeaderId = " + pPostHeaderID.ToString() + " ";

                    lCmd = new SqlCommand(lSQL, cnNDS);
                    lCmd.Transaction = osqlTransNDS;
                    lCmd.CommandTimeout = 1200;
                    lCmd.ExecuteNonQuery();

                    for (int i = 0; i < pGroupMarking.Count; i++)
                    {
                        ndsGM = new NDSPosting.PostGroupMark();
                        ndsGM.BBSNo = pBBSNo;
                        ndsGM.BBSRemarks = pBBSDesc;
                        ndsGM.GroupMarkId = pGroupMarkingID[i];
                        ndsGM.GroupMarkingName = pGroupMarking[i];
                        ndsGM.GroupMarkingRevisionNumber = pRev[i];
                        ndsGM.GroupQty = pQty[i];
                        ndsGM.isModular = "N";
                        ndsGM.ProjectId = pProjectID;
                        ndsGM.Remarks = "";

                        ndsGroupMarkings.Add(ndsGM);
                    }
                }

                if (ndsGroupMarkings.Count > 0)
                {
                    lResult = "";
                    //lSuccess = ndsClient.SavePostGroupMarkingList(ndsGroupMarkings.ToArray(), pWBSElementID, pStruEleID, pProdTypeID, pBBSNo, pBBSDesc, lUserID, pPostHeaderID, pProjectID, out lResult, out lErrorMsg);
                    string apiUrl = "http://172.25.1.224:89/Detailing/SavePostGroupMarkingList";
                    string apiUrlWithParameters = string.Format("{0}/{1}/{2}/{3}/{4}/{5}/{6}/{7}/{8}", apiUrl, pWBSElementID, pStruEleID, pProdTypeID, pBBSNo, pBBSDesc, lUserID, pPostHeaderID, pProjectID);

                    // Serialize the ndsCAPCLinks object to JSON
                    string jsonData = JsonConvert.SerializeObject(ndsGroupMarkings);
                    StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                    using (HttpClient httpClient = new HttpClient())
                    {
                        HttpResponseMessage response = await httpClient.PostAsync(apiUrlWithParameters, content);
                        string jsonContent;
                        //string lErrorMsg = "";
                        //bool lSuccess;

                        if (response.IsSuccessStatusCode)
                        {
                            jsonContent = await response.Content.ReadAsStringAsync();
                            lSuccess = JsonConvert.DeserializeObject<bool>(jsonContent);
                        }
                        else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                        {
                            lErrorMsg = await response.Content.ReadAsStringAsync();
                            lSuccess = false;
                        }
                    }
                    if (lSuccess != true || lErrorMsg.Trim().Length > 0)
                    {
                        //if (ndsClient.State == System.ServiceModel.CommunicationState.Opened)
                        //{
                        //    ndsClient.Close();
                        //}
                        lReturn = "Error on saving group marking to ODOS: " + lErrorMsg;
                        return lReturn;
                    }
                    //ajit
                }

                if (osqlTransNDS != null) osqlTransNDS.Commit();
                osqlTransNDS = cnNDS.BeginTransaction(IsolationLevel.ReadUncommitted);

                {

                    //lSuccess = ndsClient.PostBBS(pPostHeaderID, lUserID, lModular, out lErrorMsg);

                    using (var client = new HttpClient())
                    {
                        try
                        {
                            // Define the API endpoint URL
                            string apiUrl = "http://172.25.1.224:91/PostBBSAsync/"; //+ lPostHeaderID / +lGroupMarkId; // Replace with your API URL

                            string apiUrlWithParameters = string.Format("{0}/{1}/{2}/{3}", apiUrl, pPostHeaderID, lUserID, lModular);
                            // Send an HTTP GET request
                            HttpResponseMessage response = await client.GetAsync(apiUrlWithParameters);

                            // Check if the request was successful
                            if (response.IsSuccessStatusCode)
                            {
                                // Read the response content as a string
                                string responseContent = await response.Content.ReadAsStringAsync();
                                lSuccess = bool.Parse(responseContent);
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
                        //}

                        lReturn = "Error on posting BBS: " + lErrorMsg;
                        return lReturn;
                    }
                    
                }

                if (pProdTypeID == 1)
                {
                    //ndsCAPCLinks = ndsClient.GetCapping(pPostHeaderID, out lErrorMsg).ToList();
                    using (var client = new HttpClient())
                    {
                        try
                        {
                            // Define the API endpoint URL
                            string apiUrl = "https://localhost:5006/GetCapping/" + pPostHeaderID; // Replace with your API URL

                            // Send an HTTP GET request
                            HttpResponseMessage response = await client.GetAsync(apiUrl);

                            // Check if the request was successful
                            if (response.IsSuccessStatusCode)
                            {
                                // Read the response content as a string

                                // ndsCAPCLinks = await response.Content.ReadAsStringAsync();
                                string jsonContent2 = response.Content.ReadAsStringAsync().Result;
                                //ndsCAPCLinks = JsonConvert.DeserializeObject<CapClink>(jsonContent2);
                                ndsCAPCLinks = JsonConvert.DeserializeObject<List<NDSPosting.CapClink>>(jsonContent2);

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
                    //    //if (ndsClient.State == System.ServiceModel.CommunicationState.Opened)
                    //    //{
                    //    //    ndsClient.Close();
                    //    //}

                    //    lReturn = "Error on getting capping: " + lErrorMsg;
                    //    return lReturn;
                    //}
                   // ajit

                    if (ndsCAPCLinks.Count > 0)
                    {
                        ndsCAPCLinkList = new List<NDSPosting.CapClink>();
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
                        //lSuccess = ndsClient.SaveCappingDetails(ndsCAPCLinks.ToArray(), lUserID, pWBSElementID, pStruEleID, pProdTypeID, pPostHeaderID, out lRet1, out lRet2, out lErrorMsg);
                        HttpClient httpClient = new HttpClient();

                        string apiUrl = "https://localhost:5006/AddPostingCLinkCCLMarkDetailsAsync";

                        string jsonData = JsonConvert.SerializeObject(ndsCAPCLinks);
                        HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                        string apiUrlWithParameters = string.Format("{0}/{1}/{2}/{3}/{4}/{5}", apiUrl, lUserID, pWBSElementID, pStruEleID, pProdTypeID, pPostHeaderID);
                        string jsonContent = null;

                        HttpResponseMessage response = httpClient.PostAsync(apiUrlWithParameters, content).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            jsonContent = response.Content.ReadAsStringAsync().Result;
                            lSuccess = JsonConvert.DeserializeObject<bool>(jsonContent);
                        }
                        else
                        {
                            if (response.StatusCode == HttpStatusCode.BadRequest)
                            {
                                lErrorMsg = response.Content.ReadAsStringAsync().Result;
                            }
                        }


                        if (lSuccess != true || lErrorMsg.Trim().Length > 0)
                        {
                            //if (ndsClient.State == System.ServiceModel.CommunicationState.Opened)
                            //{
                            //    ndsClient.Close();
                            //}

                            lReturn = "Error on saving capping: " + lErrorMsg;
                            return lReturn;
                        }
                        
                    }
                }

                if (pProdTypeID == 2)
                {
                    //ndsCAPCLinks = ndsClient.GetClink(pPostHeaderID, out lErrorMsg).ToList();
                    using (var client = new HttpClient())
                    {
                        try
                        {
                            // Define the API endpoint URL
                            string apiUrl = "https://localhost:5006/GetPostClinkInfoList/" + pPostHeaderID; // Replace with your API URL

                            // Send an HTTP GET request
                            HttpResponseMessage response = await client.GetAsync(apiUrl);

                            // Check if the request was successful
                            if (response.IsSuccessStatusCode)
                            {
                                string jsonContent2 = response.Content.ReadAsStringAsync().Result;
                                //ndsCAPCLinks = JsonConvert.DeserializeObject<CapClink>(jsonContent2);
                                ndsCAPCLinks = JsonConvert.DeserializeObject<List<NDSPosting.CapClink>>(jsonContent2);

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
                    if (lErrorMsg.Trim().Length > 0)
                    {
                        //if (ndsClient.State == System.ServiceModel.CommunicationState.Opened)
                        //{
                        //    ndsClient.Close();
                        //}

                        lReturn = "Error on getting c-link: " + lErrorMsg;
                        return lReturn;
                    }
                   

                    if (ndsCAPCLinks.Count > 0)
                    {
                        ndsCAPCLinkList = new List<NDSPosting.CapClink>();
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
                        //lSuccess = ndsClient.SaveCLinkDetails(ndsCAPCLinks.ToArray(), lUserID, pWBSElementID, pStruEleID, pProdTypeID, pPostHeaderID, out lErrorMsg);
                        using (var client = new HttpClient())
                        {
                            try
                            {

                                string jsonData = JsonConvert.SerializeObject(ndsCAPCLinks);
                                StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                                // Define the API endpoint URL

                                string apiUrl = "https://localhost:5006/SaveClink_Details";// Replace with your API URL
                                string apiUrlWithParameters = string.Format("{0}/{1}/{2}/{3}/{4}/{5}", apiUrl, lUserID, pWBSElementID, pStruEleID, pProdTypeID, pPostHeaderID);
                                // Send an HTTP GET request
                                HttpResponseMessage response = await client.PostAsync(apiUrlWithParameters, content);

                                // Check if the request was successful
                                if (response.IsSuccessStatusCode)
                                {
                                    // Read the response content as a string
                                    string responseContent = await response.Content.ReadAsStringAsync();
                                    lSuccess = JsonConvert.DeserializeObject<bool>(responseContent);
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
                            //}

                            lReturn = "Error on saving c-link: " + lErrorMsg;
                            return lReturn;
                        }
                    }
                }

                //if (ndsClient.State == System.ServiceModel.CommunicationState.Opened)
                //{
                //    ndsClient.Close();
                //}ajit
            }
            catch (Exception ex)
            {
                lReturn = "Error on posting BBS: " + ex.Message;
                //if (ndsClient.State == System.ServiceModel.CommunicationState.Opened)
                //{
                //    ndsClient.Close();
                //}ajit
            }

            myTransport = null;
            muEncoding = null;

            binding = null;
            address = null;
            //ndsClient = null;

            return lReturn;
        }

        [HttpPost]
        public ActionResult PostWBSBBS(string CustomerCode, string ProjectCode, string ProductType, string StructureElement, List<int> PostHeaderIDs)
        {
            bool lSuccess = true;
            string lErrorMsg = "";
            string lReturn = "";

            int lMultiBBSInd = 0;

            SqlCommand lCmd;
            SqlDataReader adoRst;
            string lSQL = "";

            int lProjectID = 0;
            int lWBSElementID = 0;
            int lStructureElementID = 0;
            int lProductTypeID = 0;

            string lUserID = User.Identity.GetUserName();

            try
            {
                if (PostHeaderIDs != null && PostHeaderIDs.Count > 0)
                {
                    var lProces = new ProcessController();
                    strClient = lProces.strClient;
                    strServer = lProces.strServer;
                    lProces.OpenNDSConnection(ref cnNDS);
                    if (cnNDS.State == ConnectionState.Open)
                    {
                        for (int i = 0; i < PostHeaderIDs.Count; i++)
                        {
                            int lHeaderID = PostHeaderIDs[i];
                            var lAssignedC =
                                (from p in db.ComponentWBS
                                 where p.PostHeaderID == lHeaderID
                                 orderby p.SplitID, p.SSID
                                 select p).ToList();

                            if (lAssignedC != null && lAssignedC.Count > 0)
                            {
                                lSQL = "SELECT intProjectId, intWBSElementId, intStructureElementTypeId, sitProductTypeId " +
                                "FROM dbo.BBSPostHeader " +
                                "WHERE intPostHeaderId = " + lHeaderID.ToString() + " ";

                                lCmd = new SqlCommand(lSQL, cnNDS);
                                lCmd.CommandTimeout = 1200;
                                lCmd.Transaction = osqlTransNDS;
                                adoRst = lCmd.ExecuteReader();
                                if (adoRst.HasRows)
                                {
                                    if (adoRst.Read())
                                    {
                                        lProjectID = adoRst.GetValue(0) == DBNull.Value ? 0 : adoRst.GetInt32(0);
                                        lWBSElementID = adoRst.GetValue(1) == DBNull.Value ? 0 : adoRst.GetInt32(1);
                                        lStructureElementID = adoRst.GetValue(2) == DBNull.Value ? 0 : adoRst.GetInt32(2);
                                        lProductTypeID = adoRst.GetValue(3) == DBNull.Value ? 0 : adoRst.GetInt16(3);
                                    }
                                }
                                adoRst.Close();

                                var lGMPost = new List<string>();
                                var lGMIDPost = new List<int>();
                                var lVerPost = new List<int>();
                                var lPostQtyPost = new List<int>();

                                //datatransfer
                                for (int j = 0; j < lAssignedC.Count; j++)
                                {
                                    int lNDSRev = 0;
                                    int lGroupmarkingID = 0;
                                    string lBBSNo = lAssignedC[j].BBSNo;
                                    string lBBSDesc = lAssignedC[j].BBSDesc;

                                    Task<string> lReturn1 = TransferComponentAsync(
                                        lProjectID,
                                        lWBSElementID,
                                        lStructureElementID,
                                        lProductTypeID,
                                        lAssignedC[j],
                                         lNDSRev,
                                        lGroupmarkingID);

                                    if (lReturn == "")
                                    {
                                        var lPostIDNew = lHeaderID;

                                        lGMPost.Add(lAssignedC[j].ComponentName);
                                        lGMIDPost.Add(lGroupmarkingID);
                                        lVerPost.Add(lNDSRev);
                                        lPostQtyPost.Add((int)lAssignedC[j].NoOfSet);

                                        if ((j == lAssignedC.Count - 1) ||
                                            lAssignedC[j].SplitType != lAssignedC[j + 1].SplitType ||
                                            lAssignedC[j].SplitID != lAssignedC[j + 1].SplitID)
                                        {
                                            if (lAssignedC[j].SplitType != "NO" && lAssignedC[j].SplitID > 1)
                                            {
                                                //Generate new BBS 
                                                var lBBSNoNew = lBBSNo + "-S" + lAssignedC[j].SplitID.ToString();
                                                var lBBSDescNew = lBBSDesc + "-S" + lAssignedC[j].SplitID.ToString();

                                                lSQL = "usp_PostingBBSPosting_Insert ";
                                                lCmd = new SqlCommand(lSQL, cnNDS);
                                                lCmd.Transaction = osqlTransNDS;
                                                lCmd.CommandTimeout = 1200;
                                                lCmd.CommandType = CommandType.StoredProcedure;

                                                lCmd.Parameters.Add("@INTWBSELEMENTID", SqlDbType.Int);
                                                lCmd.Parameters.Add("@INTPROJECID", SqlDbType.Int);
                                                lCmd.Parameters.Add("@INTSTRUCTUREELEMENTTYPEID", SqlDbType.Int);
                                                lCmd.Parameters.Add("@SITPRODUCTTYPEID", SqlDbType.Int);
                                                lCmd.Parameters.Add("@VCHBBSNO", SqlDbType.NVarChar);
                                                lCmd.Parameters.Add("@BBS_DESC", SqlDbType.NVarChar);

                                                lCmd.Parameters["@INTWBSELEMENTID"].Value = lWBSElementID;
                                                lCmd.Parameters["@INTPROJECID"].Value = lProjectID;
                                                lCmd.Parameters["@INTSTRUCTUREELEMENTTYPEID"].Value = lStructureElementID;
                                                lCmd.Parameters["@SITPRODUCTTYPEID"].Value = lProductTypeID;
                                                lCmd.Parameters["@VCHBBSNO"].Value = lBBSNoNew;
                                                lCmd.Parameters["@BBS_DESC"].Value = lBBSDescNew;

                                                lCmd.CommandTimeout = 1200;
                                                adoRst = lCmd.ExecuteReader();
                                                if (adoRst.HasRows)
                                                {
                                                    adoRst.Read();
                                                    var lBBSGenResult = (string)adoRst.GetValue(0);
                                                    if (lBBSGenResult != "SUCCESS")
                                                    {
                                                        lErrorMsg = "Error on generating BBS number: " + lBBSGenResult + " ";
                                                        lSuccess = false;
                                                        break;
                                                    }
                                                    lMultiBBSInd = 1;
                                                }
                                                adoRst.Close();


                                                lSQL = "SELECT intPostHeaderId " +
                                                "FROM dbo.BBSPostHeader " +
                                                "WHERE intProjectId = " + lProjectID.ToString() + " " +
                                                "AND intWBSElementId = " + lWBSElementID.ToString() + " " +
                                                "AND intStructureElementTypeId = " + lStructureElementID.ToString() + " " +
                                                "AND sitProductTypeId = " + lProductTypeID.ToString() + " " +
                                                "AND vchBBSNo = '" + lBBSNoNew + "' ";

                                                lCmd = new SqlCommand(lSQL, cnNDS);
                                                lCmd.CommandTimeout = 1200;
                                                lCmd.Transaction = osqlTransNDS;
                                                adoRst = lCmd.ExecuteReader();
                                                if (adoRst.HasRows)
                                                {
                                                    if (adoRst.Read())
                                                    {
                                                        lPostIDNew = adoRst.GetValue(0) == DBNull.Value ? 0 : adoRst.GetInt32(0);
                                                    }
                                                }
                                                adoRst.Close();

                                                lSQL = "UPDATE dbo.OESComponentWBS " +
                                                "SET SplitPostHeaderID = " + lPostIDNew + " " +
                                                "WHERE PostHeaderID = " + lHeaderID + " " +
                                                "AND ComponentID = " + lAssignedC[j].ComponentID + " " +
                                                "AND SplitID = " + lAssignedC[j].SplitID + " ";

                                                lCmd = new SqlCommand(lSQL, cnNDS);
                                                lCmd.CommandTimeout = 1200;
                                                lCmd.Transaction = osqlTransNDS;
                                                lCmd.ExecuteNonQuery();

                                            }

                                            var lPostStatus = BBSPostingAsync(
                                                lPostIDNew,
                                                lProjectID,
                                                lProductTypeID,
                                                lStructureElementID,
                                                lWBSElementID,
                                                lBBSNo,
                                                lBBSDesc,
                                                lUserID,
                                                lGMPost,
                                                lGMIDPost,
                                                lVerPost,
                                                lPostQtyPost);

                                            lGMPost = new List<string>();
                                            lGMIDPost = new List<int>();
                                            lVerPost = new List<int>();
                                            lPostQtyPost = new List<int>();

                                        }
                                    }
                                    else
                                    {
                                        lSuccess = false;
                                        lErrorMsg = lReturn;
                                        break;
                                    }
                                }
                                if (lSuccess == false)
                                {
                                    break;
                                }
                            }
                        }
                        lProces.CloseNDSConnection(ref cnNDS);
                    }

                    lProces = null;
                }
            }
            catch (Exception ex)
            {
                lSuccess = false;
                lErrorMsg = ex.Message;
            }

            lCmd = null;
            adoRst = null;

            if (lSuccess == true)
            {
                lErrorMsg = lMultiBBSInd.ToString();
            }

            return Ok(new { success = lSuccess, responseText = lErrorMsg });
        }

        public async Task<string> TransferComponentAsync(
            int pProjectID,
            int pWBSElementID,
            int pStructureElementID,
            int pProductTypeID,
            ComponentWBSModel pWBSCo,  int pNDSRev,  int pGroupMarkingID)
        {
            string lReturn = "";
            SqlCommand lCmd;
            SqlDataReader adoRst;

            string lCustomerCode = "";
            string lProjectCode = "";
            int lJobID = 0;

            bool lProcStatus = true;
            int lDataReady = 0;
            string lSQL = "";
            int lPostID = pWBSCo.PostHeaderID;
            int lComponentID = pWBSCo.ComponentID;
            string lComponentName = pWBSCo.ComponentName;
            int lComponentRev = pWBSCo.Revision;
            DateTime lComponentDate = pWBSCo.UpdateDate;
            string lTransportMode = "";
            int lSplitID = (int)pWBSCo.SplitID;

            int lProjectID = pProjectID;
            int lWBSElementID = pWBSElementID;
            int lStructureElementID = pStructureElementID;
            int lProductTypeID = pProductTypeID;

            int lGroupMarkingID = 0;
            int lSEID = 0;
            int lParamID = 0;
            int lNDSRev = 0;
            string lRemarks = "";
            DateTime? lNDSDate = null;

            if (lPostID > 0 && lComponentID > 0 && lComponentName != null && lComponentName.Trim() != "")
            {
                //Group marking validation

                lSQL = "SELECT G.intGroupMarkId, S.intSEDetailingId, G.vchRemarks, G.tntGroupRevNo, G.datCreatedDate " +
                "FROM dbo.GroupMarkingDetails G, dbo.SELevelDetails S " +
                "WHERE G.intGroupMarkId = S.intGroupMarkId " +
                "AND G.intProjectId = " + lProjectID.ToString() + " " +
                "AND S.intStructureElementTypeId = " + lStructureElementID + " " +
                "AND S.sitProductTypeId = " + lProductTypeID + " " +
                "AND G.vchGroupMarkingName = '" + lComponentName + "' " +
                "AND tntGroupRevNo = " +
                "(SELECT MAX(tntGroupRevNo) " +
                "FROM dbo.GroupMarkingDetails G1, dbo.SELevelDetails S1 " +
                "WHERE G1.intGroupMarkId = S1.intGroupMarkId " +
                "AND G1.intProjectId = " + lProjectID.ToString() + " " +
                "AND S1.intStructureElementTypeId = " + lStructureElementID + " " +
                "AND S1.sitProductTypeId = " + lProductTypeID + " " +
                "AND G1.vchGroupMarkingName = '" + lComponentName + "') ";

                lCmd = new SqlCommand(lSQL, cnNDS);
                lCmd.CommandTimeout = 1200;
                lCmd.Transaction = osqlTransNDS;
                adoRst = lCmd.ExecuteReader();
                if (adoRst.HasRows)
                {
                    if (adoRst.Read())
                    {
                        lGroupMarkingID = adoRst.GetValue(0) == DBNull.Value ? 0 : adoRst.GetInt32(0);
                        lSEID = adoRst.GetValue(1) == DBNull.Value ? 0 : adoRst.GetInt32(1);
                        lRemarks = adoRst.GetValue(2) == DBNull.Value ? "" : adoRst.GetString(2).Trim();
                        lNDSRev = adoRst.GetValue(3) == DBNull.Value ? 0 : adoRst.GetByte(3);
                        lNDSDate = adoRst.GetDateTime(4);
                    }
                }
                adoRst.Close();

                lSQL = "SELECT UpdatedDate " +
                "FROM dbo.OESComponent " +
                "WHERE ComponentID = " + lComponentID.ToString() + " ";

                lCmd = new SqlCommand(lSQL, cnNDS);
                lCmd.CommandTimeout = 1200;
                lCmd.Transaction = osqlTransNDS;
                adoRst = lCmd.ExecuteReader();
                if (adoRst.HasRows)
                {
                    if (adoRst.Read())
                    {
                        lComponentDate = adoRst.GetDateTime(0);
                    }
                }
                adoRst.Close();

                if (lGroupMarkingID > 0 && lRemarks == ("DIGIOS-V" + lComponentRev.ToString("##0")) && lNDSDate > lComponentDate)
                {
                    //Validate detail
                    int lNDSPCs = 0;
                    if (pWBSCo.ProductType == "CUT-TO-SIZE-MESH")
                    {
                        lSQL = "SELECT SUM(intMemberQty) " +
                        "FROM dbo.MeshStructureMarkingDetails " +
                        "WHERE intSEDetailingId = " + lSEID.ToString() + " ";
                    }
                    else if (pWBSCo.ProductType == "STIRRUP-LINK-MESH")
                    {
                        lSQL = "SELECT SUM(intMemberQty) " +
                        "FROM dbo.StructureMarkingDetails " +
                        "WHERE intSEDetailingId = " + lSEID.ToString() + " ";
                    }
                    else if (pWBSCo.ProductType == "COLUMN-LINK-MESH")
                    {
                        lSQL = "SELECT SUM(intMemberQty) " +
                        "FROM dbo.ColumnStructureMarkingDetails " +
                        "WHERE intSEDetailingID = " + lSEID.ToString() + " ";
                    }

                    lCmd = new SqlCommand(lSQL, cnNDS);
                    lCmd.CommandTimeout = 1200;
                    lCmd.Transaction = osqlTransNDS;
                    adoRst = lCmd.ExecuteReader();
                    if (adoRst.HasRows)
                    {
                        if (adoRst.Read())
                        {
                            lNDSPCs = adoRst.GetValue(0) == DBNull.Value ? 0 : adoRst.GetInt32(0);
                        }
                    }
                    adoRst.Close();

                    if (lNDSPCs != pWBSCo.PCsPerSet)
                    {
                        lNDSRev = lNDSRev + 1;
                        lDataReady = 0;
                    }
                    else
                    {
                        //Latest data tranfserred to NDS already, can post directly.
                        lDataReady = 1;
                    }
                }

                if (lDataReady == 0)
                {
                    //Create Group marking 
                    var lDetails = (from p in db.OESComponent
                                    where p.ComponentID == lComponentID
                                    select p).ToList();

                    if (lDetails != null && lDetails.Count > 0)
                    {
                        lTransportMode = lDetails[0].TransportMode;
                        lCustomerCode = lDetails[0].CustomerCode;
                        lProjectCode = lDetails[0].ProjectCode;
                        lJobID = (int)lDetails[0].MeshJobID;

                        var lRetStatus = createGMMesh(
                                    lProjectID,
                                    lComponentName,
                                    lNDSRev,
                                    lStructureElementID,
                                    lProductTypeID,
                                    lWBSElementID,
                                    lComponentRev,
                                    lTransportMode,
                                    pWBSCo.ProductType,
                                    ref lGroupMarkingID,
                                    ref lSEID,
                                    ref lParamID);

                        if (lRetStatus == true)
                        {
                            //prepare for data transfer
                            if (pWBSCo.ProductType == "CUT-TO-SIZE-MESH")
                            {
                                if (pWBSCo.SplitType == "WT-DETAILS")
                                {
                                    var lSplit = (from p in db.ComponentSplit
                                                  where p.PostHeaderID == lPostID &&
                                                  p.ComponentID == lComponentID &&
                                                  p.SplitID == lSplitID
                                                  orderby p.ItemID
                                                  select p).ToList();

                                    var lCTSMesh = (from p in db.CTSMESHOthersDetails
                                                    join q in lSplit
                                                    on p.MeshID equals q.ItemID
                                                    where p.CustomerCode == lCustomerCode &&
                                                    p.ProjectCode == lProjectCode &&
                                                    p.JobID == lJobID
                                                    orderby p.MeshID
                                                    select p).ToList();

                                    if (lCTSMesh != null && lCTSMesh.Count > 0)
                                    {
                                        for (int j = 0; j < lCTSMesh.Count; j++)
                                        {
                                            lCTSMesh[j].MeshMemberQty = lSplit[j].MeshMemberQty;
                                            lCTSMesh[j].MeshTotalWT = lSplit[j].MeshTotalWT;
                                        }
                                    }

                                    lCTSMesh = lCTSMesh.OrderBy(x => x.MeshSort).ToList();

                                    var lSlabResult = SlabTransferAsync(lProjectID,
                                        lProductTypeID,
                                        lStructureElementID,
                                        lSEID,
                                        pWBSCo.BBSNo,
                                        pWBSCo.BBSDesc,
                                        lParamID,
                                        lCTSMesh);

                                    string result = await lSlabResult;
                                    if (lSlabResult != null && result.Trim() != "")
                                    {
                                        lReturn = result;
                                    }

                                }
                                else
                                {
                                    var lCTSMesh = (from p in db.CTSMESHOthersDetails
                                                    where p.CustomerCode == lCustomerCode &&
                                                    p.ProjectCode == lProjectCode &&
                                                    p.JobID == lJobID
                                                    orderby p.MeshSort
                                                    select p).ToList();

                                    var lSlabResult = SlabTransferAsync(lProjectID,
                                        lProductTypeID,
                                        lStructureElementID,
                                        lSEID,
                                        pWBSCo.BBSNo,
                                        pWBSCo.BBSDesc,
                                        lParamID,
                                        lCTSMesh);

                                    string result = await lSlabResult;
                                    if (result != null && result.Trim() != "")
                                    {
                                        lReturn = result;
                                    }
                                }
                            }
                            else if (pWBSCo.ProductType == "STIRRUP-LINK-MESH")
                            {
                                if (pWBSCo.SplitType == "WT-DETAILS")
                                {
                                    var lSplit = (from p in db.ComponentSplit
                                                  where p.PostHeaderID == lPostID &&
                                                  p.ComponentID == lComponentID &&
                                                  p.SplitID == lSplitID
                                                  orderby p.ItemID
                                                  select p).ToList();

                                    var lCTSMesh = (from p in db.CTSMESHBeamDetails
                                                    join q in lSplit
                                                    on p.MeshID equals q.ItemID
                                                    where p.CustomerCode == lCustomerCode &&
                                                    p.ProjectCode == lProjectCode &&
                                                    p.JobID == lJobID
                                                    orderby p.MeshID
                                                    select p).ToList();

                                    if (lCTSMesh != null && lCTSMesh.Count > 0)
                                    {
                                        for (int j = 0; j < lCTSMesh.Count; j++)
                                        {
                                            lCTSMesh[j].MeshMemberQty = lSplit[j].MeshMemberQty;
                                            lCTSMesh[j].MeshTotalWT = lSplit[j].MeshTotalWT;
                                        }
                                    }

                                    lCTSMesh = lCTSMesh.OrderBy(x => x.MeshSort).ToList();

                                    var lBeamResult = BeamTransferAsync(lProjectID,
                                        lProductTypeID,
                                        lStructureElementID,
                                        lSEID,
                                        pWBSCo.BBSNo,
                                        pWBSCo.BBSDesc,
                                        lParamID,
                                        lCTSMesh);
                                    string result = await lBeamResult;
                                    if (result != null && result.Trim() != "")
                                    {
                                        lReturn = result;
                                    }
                                }
                                else
                                {
                                    var lCTSMesh = (from p in db.CTSMESHBeamDetails
                                                    where p.CustomerCode == lCustomerCode &&
                                                    p.ProjectCode == lProjectCode &&
                                                    p.JobID == lJobID
                                                    orderby p.MeshSort
                                                    select p).ToList();

                                    var lBeamResult = BeamTransferAsync(lProjectID,
                                        lProductTypeID,
                                        lStructureElementID,
                                        lSEID,
                                        pWBSCo.BBSNo,
                                        pWBSCo.BBSDesc,
                                        lParamID,
                                        lCTSMesh);
                                    string result = await lBeamResult;
                                    if (result != null && result.Trim() != "")
                                    {
                                        lReturn = result;
                                    }
                                }
                            }
                            else if (pWBSCo.ProductType == "COLUMN-LINK-MESH")
                            {
                                if (pWBSCo.SplitType == "WT-DETAILS")
                                {
                                    var lSplit = (from p in db.ComponentSplit
                                                  where p.PostHeaderID == lPostID &&
                                                  p.ComponentID == lComponentID &&
                                                  p.SplitID == lSplitID
                                                  orderby p.ItemID
                                                  select p).ToList();

                                    var lCTSMesh = (from p in db.CTSMESHColumnDetails
                                                    join q in lSplit
                                                    on p.MeshID equals q.ItemID
                                                    where p.CustomerCode == lCustomerCode &&
                                                    p.ProjectCode == lProjectCode &&
                                                    p.JobID == lJobID
                                                    orderby p.MeshID
                                                    select p).ToList();

                                    if (lCTSMesh != null && lCTSMesh.Count > 0)
                                    {
                                        for (int j = 0; j < lCTSMesh.Count; j++)
                                        {
                                            lCTSMesh[j].MeshMemberQty = lSplit[j].MeshMemberQty;
                                            lCTSMesh[j].MeshTotalWT = lSplit[j].MeshTotalWT;
                                        }
                                    }

                                    lCTSMesh = lCTSMesh.OrderBy(x => x.MeshSort).ToList();

                                    var lColumnResult = ColumnTransferAsync(lProjectID,
                                        lProductTypeID,
                                        lStructureElementID,
                                        lSEID,
                                        pWBSCo.BBSNo,
                                        pWBSCo.BBSDesc,
                                        lParamID,
                                        lCTSMesh);

                                    string result = await lColumnResult;
                                    if (result != null && result.Trim() != "")
                                    {
                                        lReturn = result;
                                    }
                                }
                                else
                                {
                                    var lCTSMesh = (from p in db.CTSMESHColumnDetails
                                                    where p.CustomerCode == lCustomerCode &&
                                                    p.ProjectCode == lProjectCode &&
                                                    p.JobID == lJobID
                                                    orderby p.MeshSort
                                                    select p).ToList();

                                    var lColumnResult = ColumnTransferAsync(lProjectID,
                                        lProductTypeID,
                                        lStructureElementID,
                                        lSEID,
                                        pWBSCo.BBSNo,
                                        pWBSCo.BBSDesc,
                                        lParamID,
                                        lCTSMesh);

                                    string result = await lColumnResult;
                                    if (result != null && result.Trim() != "")
                                    {
                                        lReturn = result;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        lReturn = "Invalid Component ID Assigned.";
                    }
                }

                pNDSRev = lNDSRev;
            }
            else
            {
                lReturn = "invalid Component Name.";
            }

            pGroupMarkingID = lGroupMarkingID;

            return lReturn;
        }

        public bool createGMMesh(
            int pProjectID,
            string pGroupMarking,
            int pVersion,
            int pStrELeID,
            int pProdTypeID,
            int pWBSElementID,
            int pDIGIRev,
            string pTransportMode,
            string pProductType,
            ref int pGroupMarkingID,
            ref int pSELevelID,
            ref int pParamID)
        {
            bool lReturn = true;
            int lSELevelID = 0;
            int lDetailingWBSID = 0;
            int lGroupMarkingID = 0;
            int lParamID = 0;
            string lSQL;
            SqlCommand lCmd;
            SqlDataReader adoRst;

            int lTransportMode = 0;

            //Get Transport Mode
            if (pTransportMode.Trim().Length > 0)
            {
                lSQL = "SELECT tntTransportModeId FROM dbo.TransportMaster WHERE vchTransportMode = '" + pTransportMode + "' ";
                lCmd = new SqlCommand(lSQL, cnNDS);
                lCmd.Transaction = osqlTransNDS;
                lCmd.CommandTimeout = 1200;
                adoRst = lCmd.ExecuteReader();
                if (adoRst.HasRows)
                {
                    adoRst.Read();
                    lTransportMode = (int)adoRst.GetByte(0);
                }
                adoRst.Close();
            }

            if (pProductType == "STIRRUP-LINK-MESH")
            {
                var lsitProductTypeL2Id = 20;

                lSQL = "SELECT tntParamSetNumber " +
                "FROM dbo.ProjectParameter M " +
                "WHERE M.intProjectId = " + pProjectID.ToString() + " " +
                "AND UPPER(vchParameterType) = 'BEAM' " +
                "AND sitProductTypeL2Id =  " + lsitProductTypeL2Id.ToString() + " " +
                "AND M.tntStatusID = 1 " +
                "AND M.bitStructureMarkingLevel = 0 " +
                "AND sitTopCover = 0 " +
                "AND sitBottomCover = 0 " +
                "AND sitLeftCover = 0 " +
                "AND sitRightCover = 0 " +
                "AND Exists " +
                "(SELECT tntParamSetNumber " +
                "FROM dbo.ProjectParameter M2 " +
                "WHERE M2.tntParamSetNumber = M.tntRefParamSetNumber " +
                "AND sitProductTypeL2Id = 21 " +
                "AND UPPER(vchParameterType) = 'CAPPING/CLINK' " +
                "AND exists (SELECT tntParamCageId FROM dbo.ProjectParamCage " +
                "WHERE tntParamSetNumber = M2.tntParamSetNumber " +
                "AND intDiameter = 8 " +
                "AND chrStandard <> 'Y' " +
                "AND sitLeg = 120 " +
                "AND Length = 2400) " +
                "AND exists(SELECT tntParamCageId FROM dbo.ProjectParamCage " +
                "WHERE tntParamSetNumber = M2.tntParamSetNumber " +
                "AND intDiameter = 10 " +
                "AND chrStandard <> 'Y' " +
                "AND sitLeg = 120 " +
                "AND Length = 2400) " +
                "AND exists(SELECT tntParamCageId FROM dbo.ProjectParamCage " +
                "WHERE tntParamSetNumber = M2.tntParamSetNumber " +
                "AND intDiameter = 13 " +
                "AND chrStandard <> 'Y' " +
                "AND sitLeg = 120 " +
                "AND Length = 2400) " +
                ")";

                lCmd = new SqlCommand(lSQL, cnNDS);
                lCmd.Transaction = osqlTransNDS;
                lCmd.CommandTimeout = 1200;
                adoRst = lCmd.ExecuteReader();

                if (adoRst.HasRows)
                {
                    adoRst.Read();
                    lParamID = adoRst.GetInt32(0);
                    adoRst.Close();
                }
                else
                {
                    adoRst.Close();
                    //insert parameter
                    //1.get Capping max set No
                    int lCappingSet = 0;
                    lSQL = "SELECT isNull(MAX(intParameteSet),0) " +
                    "FROM dbo.ProjectParameter M " +
                    "WHERE M.intProjectId = " + pProjectID.ToString() + " " +
                    "AND UPPER(vchParameterType) = 'CAPPING/CLINK' " +
                    "AND sitProductTypeL2Id = 21 " +
                    "AND M.tntStatusID = 1 " +
                    "AND M.bitStructureMarkingLevel = 0 ";

                    lCmd = new SqlCommand(lSQL, cnNDS);
                    lCmd.Transaction = osqlTransNDS;
                    lCmd.CommandTimeout = 1200;
                    adoRst = lCmd.ExecuteReader();
                    if (adoRst.HasRows)
                    {
                        adoRst.Read();
                        lCappingSet = adoRst.GetInt32(0);
                    }
                    adoRst.Close();
                    lCappingSet = lCappingSet + 1;

                    //2.Save Capping set No
                    lSQL = "INSERT INTO dbo.ProjectParameter " +
                    "(intProjectId " +
                    ", sitProductTypeL2Id " +
                    ", intParameteSet " +
                    ", tntStatusId " +
                    ", vchParameterType " +
                    ", bitStructureMarkingLevel " +
                    ", sitProductTypeId " +
                    ", intCreatedUID " +
                    ", datCreatedDate) " +
                    "VALUES " +
                    "(" + pProjectID.ToString() + " " +
                    ",21 " +
                    "," + lCappingSet.ToString() + " " +
                    ",1 " +
                    ",'Capping/Clink' " +
                    ",0 " +
                    ",0 " +
                    ",111 " +
                    ",getDate() ) ";

                    lCmd = new SqlCommand(lSQL, cnNDS);
                    lCmd.Transaction = osqlTransNDS;
                    lCmd.CommandTimeout = 1200;
                    lCmd.ExecuteNonQuery();

                    //3.get Capping Set No ID
                    int lCappingSetID = 0;
                    lSQL = "SELECT tntParamSetNumber " +
                    "FROM dbo.ProjectParameter M " +
                    "WHERE M.intProjectId = " + pProjectID.ToString() + " " +
                    "AND UPPER(vchParameterType) = 'CAPPING/CLINK' " +
                    "AND sitProductTypeL2Id = 21 " +
                    "AND M.tntStatusID = 1 " +
                    "AND M.bitStructureMarkingLevel = 0 " +
                    "AND intParameteSet = " + lCappingSet.ToString() + " ";

                    lCmd = new SqlCommand(lSQL, cnNDS);
                    lCmd.Transaction = osqlTransNDS;
                    lCmd.CommandTimeout = 1200;
                    adoRst = lCmd.ExecuteReader();
                    if (adoRst.HasRows)
                    {
                        adoRst.Read();
                        lCappingSetID = adoRst.GetInt32(0);
                    }
                    adoRst.Close();
                    //4.Save Capping set No
                    lSQL = "INSERT INTO dbo.ProjectParamCage " +
                    "(intProductCodeId " +
                    ", tntParamSetNumber " +
                    ", chrStandard " +
                    ", intDiameter " +
                    ", intStdCCLProductID " +
                    ", sitHook " +
                    ", sitLeg " +
                    ", Length) " +
                    "VALUES " +
                    "(0 " +
                    "," + lCappingSetID.ToString() + " " +
                    ",'N' " +
                    ",8 " +
                    ",0 " +
                    ",60 " +
                    ",120 " +
                    ",2400) ";

                    lCmd = new SqlCommand(lSQL, cnNDS);
                    lCmd.Transaction = osqlTransNDS;
                    lCmd.CommandTimeout = 1200;
                    lCmd.ExecuteNonQuery();

                    lSQL = "INSERT INTO dbo.ProjectParamCage " +
                    "(intProductCodeId " +
                    ", tntParamSetNumber " +
                    ", chrStandard " +
                    ", intDiameter " +
                    ", intStdCCLProductID " +
                    ", sitHook " +
                    ", sitLeg " +
                    ", Length) " +
                    "VALUES " +
                    "(0 " +
                    "," + lCappingSetID.ToString() + " " +
                    ",'N' " +
                    ",10 " +
                    ",0 " +
                    ",60 " +
                    ",120 " +
                    ",2400) ";

                    lCmd = new SqlCommand(lSQL, cnNDS);
                    lCmd.Transaction = osqlTransNDS;
                    lCmd.CommandTimeout = 1200;
                    lCmd.ExecuteNonQuery();

                    lSQL = "INSERT INTO dbo.ProjectParamCage " +
                    "(intProductCodeId " +
                    ", tntParamSetNumber " +
                    ", chrStandard " +
                    ", intDiameter " +
                    ", intStdCCLProductID " +
                    ", sitHook " +
                    ", sitLeg " +
                    ", Length) " +
                    "VALUES " +
                    "(0 " +
                    "," + lCappingSetID.ToString() + " " +
                    ",'N' " +
                    ",13 " +
                    ",0 " +
                    ",60 " +
                    ",120 " +
                    ",2400) ";

                    lCmd = new SqlCommand(lSQL, cnNDS);
                    lCmd.Transaction = osqlTransNDS;
                    lCmd.CommandTimeout = 1200;
                    lCmd.ExecuteNonQuery();

                    //5.get Beam max set No
                    int lBeamSet = 0;
                    lSQL = "SELECT isNull(MAX(intParameteSet),0) " +
                    "FROM dbo.ProjectParameter M " +
                    "WHERE M.intProjectId = " + pProjectID.ToString() + " " +
                    "AND UPPER(vchParameterType) = 'BEAM' " +
                    "AND sitProductTypeL2Id = " + lsitProductTypeL2Id.ToString() + " " +
                    "AND M.tntStatusID = 1 " +
                    "AND M.bitStructureMarkingLevel = 0 ";

                    lCmd = new SqlCommand(lSQL, cnNDS);
                    lCmd.Transaction = osqlTransNDS;
                    lCmd.CommandTimeout = 1200;
                    adoRst = lCmd.ExecuteReader();
                    if (adoRst.HasRows)
                    {
                        adoRst.Read();
                        lBeamSet = adoRst.GetInt32(0);
                    }
                    adoRst.Close();
                    lBeamSet = lBeamSet + 1;

                    //6.Save Beam Set Code
                    lSQL = "INSERT INTO dbo.ProjectParameter " +
                    "(intProjectId " +
                    ", sitProductTypeL2Id " +
                    ", intParameteSet " +
                    ", tntRefParamSetNumber " +
                    ", tntTransportModeId " +
                    ", sitTopCover " +
                    ", sitBottomCover " +
                    ", sitLeftCover " +
                    ", sitRightCover " +
                    ", sitGap1 " +
                    ", sitGap2 " +
                    ", sitHook " +
                    ", sitLeg " +
                    ", chrStandardCP " +
                    ", chrStandarCL " +
                    ", tntStatusId " +
                    ", vchParameterType " +
                    ", bitStructureMarkingLevel " +
                    ", sitProductTypeId " +
                    ", intCreatedUID " +
                    ", datCreatedDate " +
                    ", sitInnerCover " +
                    ", sitOuterCover ) " +
                    "VALUES " +
                    "(" + pProjectID.ToString() + " " +
                    ", " + lsitProductTypeL2Id.ToString() + " " +
                    "," + lBeamSet.ToString() + " " +
                    "," + lCappingSetID.ToString() + " " +
                    ",8 " +
                    ",0 " +
                    ",0 " +
                    ",0 " +
                    ",0 " +
                    ",50 " +
                    ",50 " +
                    ",60 " +
                    ",110 " +
                    ",'N' " +
                    ",'' " +
                    ",1 " +
                    ",'Beam' " +
                    ",0 " +
                    ",0 " +
                    ",111 " +
                    ",getDate() " +
                    ",0 " +
                    ",0 ) ";

                    lCmd = new SqlCommand(lSQL, cnNDS);
                    lCmd.Transaction = osqlTransNDS;
                    lCmd.CommandTimeout = 1200;
                    lCmd.ExecuteNonQuery();

                    //7.get Beam Set No ID
                    lSQL = "SELECT tntParamSetNumber " +
                    "FROM dbo.ProjectParameter M " +
                    "WHERE M.intProjectId = " + pProjectID.ToString() + " " +
                    "AND UPPER(vchParameterType) = 'Beam' " +
                    "AND sitProductTypeL2Id = " + lsitProductTypeL2Id.ToString() + " " +
                    "AND M.tntStatusID = 1 " +
                    "AND M.bitStructureMarkingLevel = 0 " +
                    "AND intParameteSet = " + lBeamSet.ToString() + " ";

                    lCmd = new SqlCommand(lSQL, cnNDS);
                    lCmd.Transaction = osqlTransNDS;
                    lCmd.CommandTimeout = 1200;
                    adoRst = lCmd.ExecuteReader();
                    if (adoRst.HasRows)
                    {
                        adoRst.Read();
                        lParamID = adoRst.GetInt32(0);
                    }
                    adoRst.Close();
                }

            }
            else if (pProductType == "COLUMN-LINK-MESH")
            {
                var lsitProductTypeL2Id = 18;

                lSQL = "SELECT tntParamSetNumber " +
                "FROM dbo.ProjectParameter M " +
                "WHERE M.intProjectId = " + pProjectID.ToString() + " " +
                "AND UPPER(vchParameterType) = 'COLUMN' " +
                "AND sitProductTypeL2Id = " + lsitProductTypeL2Id.ToString() + " " +
                "AND M.tntStatusID = 1 " +
                "AND M.bitStructureMarkingLevel = 0 " +
                "AND sitTopCover = 0 " +
                "AND sitBottomCover = 0 " +
                "AND sitLeftCover = 0 " +
                "AND sitRightCover = 0 " +
                "AND Exists " +
                "(SELECT tntParamSetNumber " +
                "FROM dbo.ProjectParameter M2 " +
                "WHERE M2.tntParamSetNumber = M.tntRefParamSetNumber " +
                "AND sitProductTypeL2Id = 19 " +
                "AND UPPER(vchParameterType) = 'CAPPING/CLINK' " +
                "AND exists (SELECT tntParamCageId FROM dbo.ProjectParamCage " +
                "WHERE tntParamSetNumber = M2.tntParamSetNumber " +
                "AND intDiameter = 6 " +
                "AND chrStandard <> 'Y' " +
                "AND sitLeg = 100 " +
                "AND Length = 0) " +
                "AND exists (SELECT tntParamCageId FROM dbo.ProjectParamCage " +
                "WHERE tntParamSetNumber = M2.tntParamSetNumber " +
                "AND intDiameter = 8 " +
                "AND chrStandard <> 'Y' " +
                "AND sitLeg = 100 " +
                "AND Length = 0) " +
                "AND exists(SELECT tntParamCageId FROM dbo.ProjectParamCage " +
                "WHERE tntParamSetNumber = M2.tntParamSetNumber " +
                "AND intDiameter = 10 " +
                "AND chrStandard <> 'Y' " +
                "AND sitLeg = 110 " +
                "AND Length = 0) " +
                "AND exists(SELECT tntParamCageId FROM dbo.ProjectParamCage " +
                "WHERE tntParamSetNumber = M2.tntParamSetNumber " +
                "AND intDiameter = 13 " +
                "AND chrStandard <> 'Y' " +
                "AND sitLeg = 100 " +
                "AND Length = 0) " +
                ")";

                lCmd = new SqlCommand(lSQL, cnNDS);
                lCmd.Transaction = osqlTransNDS;
                lCmd.CommandTimeout = 1200;
                adoRst = lCmd.ExecuteReader();
                if (adoRst.HasRows)
                {
                    adoRst.Read();
                    lParamID = adoRst.GetInt32(0);
                    adoRst.Close();
                }
                else
                {
                    adoRst.Close();
                    //insert parameter
                    //1.get Capping max set No
                    int lClinkSet = 0;
                    lSQL = "SELECT isNull(MAX(intParameteSet),0) " +
                    "FROM dbo.ProjectParameter M " +
                    "WHERE M.intProjectId = " + pProjectID.ToString() + " " +
                    "AND UPPER(vchParameterType) = 'CAPPING/CLINK' " +
                    "AND sitProductTypeL2Id = 19 " +
                    "AND M.tntStatusID = 1 " +
                    "AND M.bitStructureMarkingLevel = 0 ";

                    lCmd = new SqlCommand(lSQL, cnNDS);
                    lCmd.Transaction = osqlTransNDS;
                    lCmd.CommandTimeout = 1200;
                    adoRst = lCmd.ExecuteReader();
                    if (adoRst.HasRows)
                    {
                        adoRst.Read();
                        lClinkSet = adoRst.GetInt32(0);
                    }
                    adoRst.Close();
                    lClinkSet = lClinkSet + 1;

                    //2.Save Capping set No
                    lSQL = "INSERT INTO dbo.ProjectParameter " +
                    "(intProjectId " +
                    ", sitProductTypeL2Id " +
                    ", intParameteSet " +
                    ", tntStatusId " +
                    ", vchParameterType " +
                    ", bitStructureMarkingLevel " +
                    ", sitProductTypeId " +
                    ", intCreatedUID " +
                    ", datCreatedDate) " +
                    "VALUES " +
                    "(" + pProjectID.ToString() + " " +
                    ", 19 " +
                    "," + lClinkSet.ToString() + " " +
                    ",1 " +
                    ",'Capping/Clink' " +
                    ",0 " +
                    ",0 " +
                    ",111 " +
                    ",getDate() ) ";

                    lCmd = new SqlCommand(lSQL, cnNDS);
                    lCmd.Transaction = osqlTransNDS;
                    lCmd.CommandTimeout = 1200;
                    lCmd.ExecuteNonQuery();

                    //3.get Capping Set No ID
                    int lClinkSetID = 0;
                    lSQL = "SELECT tntParamSetNumber " +
                    "FROM dbo.ProjectParameter M " +
                    "WHERE M.intProjectId = " + pProjectID.ToString() + " " +
                    "AND UPPER(vchParameterType) = 'CAPPING/CLINK' " +
                    "AND sitProductTypeL2Id = 19 " +
                    "AND M.tntStatusID = 1 " +
                    "AND M.bitStructureMarkingLevel = 0 " +
                    "AND intParameteSet = " + lClinkSet.ToString() + " ";

                    lCmd = new SqlCommand(lSQL, cnNDS);
                    lCmd.Transaction = osqlTransNDS;
                    lCmd.CommandTimeout = 1200;
                    adoRst = lCmd.ExecuteReader();
                    if (adoRst.HasRows)
                    {
                        adoRst.Read();
                        lClinkSetID = adoRst.GetInt32(0);
                    }
                    adoRst.Close();
                    //4.Save Capping set No
                    lSQL = "INSERT INTO dbo.ProjectParamCage " +
                    "(intProductCodeId " +
                    ", tntParamSetNumber " +
                    ", chrStandard " +
                    ", intDiameter " +
                    ", intStdCCLProductID " +
                    ", sitHook " +
                    ", sitLeg " +
                    ", Length) " +
                    "VALUES " +
                    "(0 " +
                    "," + lClinkSetID.ToString() + " " +
                    ",'N' " +
                    ",6 " +
                    ",0 " +
                    ",60 " +
                    ",100 " +
                    ",0) ";

                    lCmd = new SqlCommand(lSQL, cnNDS);
                    lCmd.Transaction = osqlTransNDS;
                    lCmd.CommandTimeout = 1200;
                    lCmd.ExecuteNonQuery();

                    //4.Save Capping set No
                    lSQL = "INSERT INTO dbo.ProjectParamCage " +
                    "(intProductCodeId " +
                    ", tntParamSetNumber " +
                    ", chrStandard " +
                    ", intDiameter " +
                    ", intStdCCLProductID " +
                    ", sitHook " +
                    ", sitLeg " +
                    ", Length) " +
                    "VALUES " +
                    "(0 " +
                    "," + lClinkSetID.ToString() + " " +
                    ",'N' " +
                    ",8 " +
                    ",0 " +
                    ",60 " +
                    ",100 " +
                    ",0) ";

                    lCmd = new SqlCommand(lSQL, cnNDS);
                    lCmd.Transaction = osqlTransNDS;
                    lCmd.CommandTimeout = 1200;
                    lCmd.ExecuteNonQuery();

                    lSQL = "INSERT INTO dbo.ProjectParamCage " +
                    "(intProductCodeId " +
                    ", tntParamSetNumber " +
                    ", chrStandard " +
                    ", intDiameter " +
                    ", intStdCCLProductID " +
                    ", sitHook " +
                    ", sitLeg " +
                    ", Length) " +
                    "VALUES " +
                    "(0 " +
                    "," + lClinkSetID.ToString() + " " +
                    ",'N' " +
                    ",10 " +
                    ",0 " +
                    ",60 " +
                    ",110 " +
                    ",0) ";

                    lCmd = new SqlCommand(lSQL, cnNDS);
                    lCmd.Transaction = osqlTransNDS;
                    lCmd.CommandTimeout = 1200;
                    lCmd.ExecuteNonQuery();

                    lSQL = "INSERT INTO dbo.ProjectParamCage " +
                    "(intProductCodeId " +
                    ", tntParamSetNumber " +
                    ", chrStandard " +
                    ", intDiameter " +
                    ", intStdCCLProductID " +
                    ", sitHook " +
                    ", sitLeg " +
                    ", Length) " +
                    "VALUES " +
                    "(0 " +
                    "," + lClinkSetID.ToString() + " " +
                    ",'N' " +
                    ",13 " +
                    ",0 " +
                    ",60 " +
                    ",100 " +
                    ",0) ";

                    lCmd = new SqlCommand(lSQL, cnNDS);
                    lCmd.Transaction = osqlTransNDS;
                    lCmd.CommandTimeout = 1200;
                    lCmd.ExecuteNonQuery();

                    //5.get Column max set No
                    int lColumnSet = 0;
                    lSQL = "SELECT isNull(MAX(intParameteSet),0) " +
                    "FROM dbo.ProjectParameter M " +
                    "WHERE M.intProjectId = " + pProjectID.ToString() + " " +
                    "AND UPPER(vchParameterType) = 'COLUMN' " +
                    "AND sitProductTypeL2Id = " + lsitProductTypeL2Id.ToString() + " " +
                    "AND M.tntStatusID = 1 " +
                    "AND M.bitStructureMarkingLevel = 0 ";

                    lCmd = new SqlCommand(lSQL, cnNDS);
                    lCmd.Transaction = osqlTransNDS;
                    lCmd.CommandTimeout = 1200;
                    adoRst = lCmd.ExecuteReader();
                    if (adoRst.HasRows)
                    {
                        adoRst.Read();
                        lColumnSet = adoRst.GetInt32(0);
                    }
                    adoRst.Close();
                    lColumnSet = lColumnSet + 1;

                    //6.Save Column Set Code
                    lSQL = "INSERT INTO dbo.ProjectParameter " +
                    "(intProjectId " +
                    ", sitProductTypeL2Id " +
                    ", intParameteSet " +
                    ", tntRefParamSetNumber " +
                    ", tntTransportModeId " +
                    ", sitTopCover " +
                    ", sitBottomCover " +
                    ", sitLeftCover " +
                    ", sitRightCover " +
                    ", sitGap1 " +
                    ", sitGap2 " +
                    ", sitHook " +
                    ", sitLeg " +
                    ", chrStandardCP " +
                    ", chrStandarCL " +
                    ", tntStatusId " +
                    ", vchParameterType " +
                    ", bitStructureMarkingLevel " +
                    ", sitProductTypeId " +
                    ", intCreatedUID " +
                    ", datCreatedDate " +
                    ", sitInnerCover " +
                    ", sitOuterCover ) " +
                    "VALUES " +
                    "(" + pProjectID.ToString() + " " +
                    "," + lsitProductTypeL2Id.ToString() + " " +
                    "," + lColumnSet.ToString() + " " +
                    "," + lClinkSetID.ToString() + " " +
                    ",9 " +
                    ",0 " +
                    ",0 " +
                    ",0 " +
                    ",0 " +
                    ",0 " +
                    ",0 " +
                    ",60 " +
                    ",110 " +
                    ",'N' " +
                    ",'' " +
                    ",1 " +
                    ",'Column' " +
                    ",0 " +
                    ",0 " +
                    ",111 " +
                    ",getDate() " +
                    ",0 " +
                    ",0 ) ";

                    lCmd = new SqlCommand(lSQL, cnNDS);
                    lCmd.Transaction = osqlTransNDS;
                    lCmd.CommandTimeout = 1200;
                    lCmd.ExecuteNonQuery();

                    //7.get Column Set No ID
                    lSQL = "SELECT tntParamSetNumber " +
                    "FROM dbo.ProjectParameter M " +
                    "WHERE M.intProjectId = " + pProjectID.ToString() + " " +
                    "AND UPPER(vchParameterType) = 'Column' " +
                    "AND sitProductTypeL2Id = " + lsitProductTypeL2Id.ToString() + " " +
                    "AND M.tntStatusID = 1 " +
                    "AND M.bitStructureMarkingLevel = 0 " +
                    "AND intParameteSet = " + lColumnSet.ToString() + " ";

                    lCmd = new SqlCommand(lSQL, cnNDS);
                    lCmd.Transaction = osqlTransNDS;
                    lCmd.CommandTimeout = 1200;
                    adoRst = lCmd.ExecuteReader();
                    if (adoRst.HasRows)
                    {
                        adoRst.Read();
                        lParamID = adoRst.GetInt32(0);
                    }
                    adoRst.Close();
                }

            }
            else
            {

                lSQL = "SELECT tntParamSetNumber " +
                "FROM dbo.ProjectParameter M " +
                "WHERE M.intProjectId = " + pProjectID.ToString() + " " +
                "AND M.sitProductTypeId = " + pProdTypeID + " " +
                "AND UPPER(vchParameterType) = 'Mesh' " +
                "AND M.tntStatusID = 1 " +
                "AND M.bitStructureMarkingLevel = 0 " +
                "ORDER BY tntParamSetNumber ";

                lCmd = new SqlCommand(lSQL, cnNDS);
                lCmd.Transaction = osqlTransNDS;
                lCmd.CommandTimeout = 1200;
                adoRst = lCmd.ExecuteReader();

                if (adoRst.HasRows)
                {
                    adoRst.Read();
                    lParamID = adoRst.GetInt32(0);
                    adoRst.Close();
                }
                else
                {
                    adoRst.Close();
                    //insert parameter
                    //1.get Capping max set No
                    int lMeshSet = 1;

                    //2.Save Capping set No
                    lSQL = "INSERT INTO dbo.ProjectParameter " +
                    "(intProjectId " +
                    ", tntTransportModeID " +
                    ", intParameteSet " +
                    ", tntStatusId " +
                    ", vchParameterType " +
                    ", bitStructureMarkingLevel " +
                    ", sitProductTypeId " +
                    ", intCreatedUID " +
                    ", datCreatedDate) " +
                    "VALUES " +
                    "(" + pProjectID.ToString() + " " +
                    ",8 " +
                    "," + lMeshSet.ToString() + " " +
                    ",1 " +
                    ",'Mesh' " +
                    ",0 " +
                    "," + pProdTypeID.ToString() + " " +
                    ",111 " +
                    ",getDate() ) ";

                    lCmd = new SqlCommand(lSQL, cnNDS);
                    lCmd.Transaction = osqlTransNDS;
                    lCmd.CommandTimeout = 1200;
                    lCmd.ExecuteNonQuery();

                    lSQL = "SELECT tntParamSetNumber " +
                    "FROM dbo.ProjectParameter M " +
                    "WHERE M.intProjectId = " + pProjectID.ToString() + " " +
                    "AND M.sitProductTypeId = " + pProdTypeID + " " +
                    "AND UPPER(vchParameterType) = 'Mesh' " +
                    "AND M.tntStatusID = 1 " +
                    "AND M.bitStructureMarkingLevel = 0 " +
                    "ORDER BY tntParamSetNumber ";

                    lCmd = new SqlCommand(lSQL, cnNDS);
                    lCmd.Transaction = osqlTransNDS;
                    lCmd.CommandTimeout = 1200;
                    adoRst = lCmd.ExecuteReader();
                    if (adoRst.HasRows)
                    {
                        adoRst.Read();
                        lParamID = adoRst.GetInt32(0);
                    }
                    adoRst.Close();
                }
            }

            lSQL = "INSERT INTO dbo.GroupMarkingDetails " +
                "(tntGroupRevNo " +
                ",intProjectId " +
                ",intWBSTypeId " +
                ",vchGroupMarkingName " +
                ",vchRemarks " +
                ",tntTransportModeId " +
                ",tntStatusId " +
                ",intCreatedUId " +
                ",datCreatedDate " +
                ",intArmaid ) " +
                "VALUES " +
                "( " + pVersion + " " +
                ", " + pProjectID + " " +
                ",1 " +
                ",'" + pGroupMarking + "' " +
                ",'DIGIOS-V" + pDIGIRev.ToString("###0") + "' " +
                ", " + lTransportMode.ToString() + " " +
                ",1 " +
                ",111 " +
                ",getDate() " +
                ",0 )";

            lCmd = new SqlCommand(lSQL, cnNDS);
            lCmd.Transaction = osqlTransNDS;
            lCmd.CommandTimeout = 1200;
            lCmd.ExecuteNonQuery();

            //Get Group Marking ID
            lSQL = "SELECT max(intGroupMarkId) " +
                "FROM dbo.GroupMarkingDetails " +
                "WHERE intProjectId = " + pProjectID + " " +
                "AND tntGroupRevNo = " + pVersion + " " +
                "AND vchGroupMarkingName = '" + pGroupMarking + "' ";

            lCmd = new SqlCommand(lSQL, cnNDS);
            lCmd.Transaction = osqlTransNDS;
            lCmd.CommandTimeout = 1200;
            adoRst = lCmd.ExecuteReader();
            if (adoRst.HasRows)
            {
                adoRst.Read();
                lGroupMarkingID = (int)adoRst.GetValue(0);
            }
            adoRst.Close();

            lSQL = "INSERT INTO dbo.SELevelDetails " +
                "(intGroupMarkId " +
                ",intStructureElementTypeId " +
                ",sitProductTypeId " +
                ",tntParamSetNumber " +
                ",bitParentFlag) " +
                "VALUES " +
                "(" + lGroupMarkingID + " " +
                ", " + pStrELeID + " " +
                ", " + pProdTypeID + " " +
                ", " + lParamID + " " +
                ",1) ";
            lCmd = new SqlCommand(lSQL, cnNDS);
            lCmd.Transaction = osqlTransNDS;
            lCmd.CommandTimeout = 1200;
            lCmd.ExecuteNonQuery();

            lSQL = "SELECT max(intSEDetailingId) FROM dbo.SELevelDetails " +
                "WHERE intGroupMarkId = " + lGroupMarkingID + " " +
                "AND intStructureElementTypeId = " + pStrELeID + " " +
                "AND sitProductTypeId = " + pProdTypeID + "  ";

            lCmd = new SqlCommand(lSQL, cnNDS);
            lCmd.Transaction = osqlTransNDS;
            lCmd.CommandTimeout = 1200;
            adoRst = lCmd.ExecuteReader();
            if (adoRst.HasRows)
            {
                adoRst.Read();
                lSELevelID = (int)adoRst.GetValue(0);
            }
            adoRst.Close();


            //Check Detailing WBS Existing
            lDetailingWBSID = 0;
            lSQL = "SELECT intDetailingWBSId " +
                "FROM dbo.DetailingWBS " +
                "WHERE intGroupMarkId = " + lGroupMarkingID + " " +
                "AND tntGroupRevNo = " + pVersion + " " +
                "AND intWBSElementId = " + pWBSElementID + " ";

            lCmd = new SqlCommand(lSQL, cnNDS);
            lCmd.Transaction = osqlTransNDS;
            lCmd.CommandTimeout = 1200;
            adoRst = lCmd.ExecuteReader();
            if (adoRst.HasRows)
            {
                adoRst.Read();
                lDetailingWBSID = (int)adoRst.GetValue(0);
            }
            adoRst.Close();

            if (lDetailingWBSID == 0)
            {
                lSQL = "INSERT INTO dbo.DetailingWBS " +
                    "(intGroupMarkId " +
                    ",tntGroupRevNo " +
                    ",intWBSElementId " +
                    ",vchDrawingReference " +
                    ",chrDrawingVersion " +
                    ",vchDrawingRemarks " +
                    ",tntStatusId) " +
                    "VALUES " +
                    "(" + lGroupMarkingID + " " +
                    "," + pVersion + " " +
                    "," + pWBSElementID + " " +
                    ",'' " +
                    ",'' " +
                    ",'' " +
                    ",1) ";
                lCmd = new SqlCommand(lSQL, cnNDS);
                lCmd.Transaction = osqlTransNDS;
                lCmd.CommandTimeout = 1200;
                lCmd.ExecuteNonQuery();

            }


            pSELevelID = lSELevelID;
            pGroupMarkingID = lGroupMarkingID;
            pParamID = lParamID;

            lCmd.Dispose();
            adoRst = null;
            lCmd = null;
            return lReturn;
        }


        [HttpPost]
        public async Task<ActionResult> UnPostWBSBBSAsync(List<int> PostHeaderIDs)
        {
            bool lSuccess = true;
            string lErrorMsg = "";
            string lReturn = "";

            string lSQL;
            SqlCommand lCmd;
            SqlDataReader adoRst;

            int lStatus = 0;
            int liVar = 0;

            var myTransport = new HttpTransportBindingElement();
            var muEncoding = new BinaryMessageEncodingBindingElement();

            var binding = new CustomBinding();
            //var binding = new BasicHttpBinding();

            //var ndsClient = new NDSPosting.BBSPostingServiceClient();ajit

            //Specify the address to be used for the client.

            EndpointAddress address;

            string lUserID = User.Identity.GetUserName();

            try
            {
                if (PostHeaderIDs != null && PostHeaderIDs.Count > 0)
                {
                    var lProces = new ProcessController();
                    strClient = lProces.strClient;
                    strServer = lProces.strServer;
                    lProces.OpenNDSConnection(ref cnNDS);

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

                    for (int i = 0; i < PostHeaderIDs.Count; i++)
                    {
                        int lHeaderID = PostHeaderIDs[i];

                        lSQL = "SELECT tntStatusId FROM dbo.BBSPostHeader WHERE intPostHeaderid = " + lHeaderID + " ";

                        lCmd = new SqlCommand(lSQL, cnNDS);
                        lCmd.Transaction = osqlTransNDS;
                        lCmd.CommandTimeout = 1200;
                        adoRst = lCmd.ExecuteReader();
                        if (adoRst.HasRows)
                        {
                            if (adoRst.Read())
                            {
                                lStatus = adoRst.GetValue(0) == DBNull.Value ? 0 : adoRst.GetByte(0);
                            }
                        }
                        adoRst.Close();

                        lSQL = "SELECT tntStatusId FROM dbo.BBSReleaseDetails WHERE intPostHeaderid = " + lHeaderID + " ";

                        lCmd = new SqlCommand(lSQL, cnNDS);
                        lCmd.Transaction = osqlTransNDS;
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

                        if (lStatus == 3)
                        {

                            lErrorMsg = "";

                            //Posted
                            //lSuccess = ndsClient.UnPostBBS(lHeaderID.ToString(), out lErrorMsg);ajit
                            using (var client = new HttpClient())
                            {
                                try
                                {
                                    // Define the API endpoint URL
                                    string apiUrl1 = "http://172.25.1.224:91/UnPostBBS_Update/" + lHeaderID; // Replace with your API URL

                                    // Send an HTTP GET request
                                    HttpResponseMessage response1 = await client.GetAsync(apiUrl1);

                                    // Check if the request was successful
                                    if (response1.IsSuccessStatusCode)
                                    {
                                        // Read the response content as a string
                                        string responseContent = await response1.Content.ReadAsStringAsync();
                                        lSuccess = bool.Parse(responseContent);

                                        // Now you can work with the response content (e.g., parse JSON, process data, etc.)
                                        Console.WriteLine(responseContent);
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Error: {response1.StatusCode} - {response1.ReasonPhrase}");
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
                                //}
                                lReturn = "Error on unpost BBS: " + lErrorMsg;
                                lSuccess = false;
                                break;
                            }
                            
                        }
                        else
                        {
                            lSuccess = false;
                            lErrorMsg = "BBS unposting failed as there is a released / unposted BBS selected.";
                            break;
                        }
                    }

                    //if (ndsClient.State == System.ServiceModel.CommunicationState.Opened)
                    //{
                    //    ndsClient.Close();
                    //}ajit

                    lProces.CloseNDSConnection(ref cnNDS);

                    lProces = null;
                }

            }
            catch (Exception ex)
            {
                lSuccess = false;
                lErrorMsg = ex.Message;
            }

            return Ok(new { success = lSuccess, responseText = lErrorMsg });
        }

        [HttpPost]
        public ActionResult ReleaseWBSBBS(List<string> PostSORs, List<int> PostHeaderIDs)
        {
            bool lSuccess = true;
            string lErrorMsg = "";

            SqlCommand lCmd;
            SqlDataReader adoRst;
            string lSQL = "";

            int lProjectID = 0;
            int lWBSElementID = 0;
            string lBBSNo = "";

            string lUserID = User.Identity.GetUserName();
            int lNDSUserID = 111;

            int lStatus = 0;
            int liVar = 0;

            try
            {
                if (PostHeaderIDs != null && PostHeaderIDs.Count > 0)
                {
                    var lProces = new ProcessController();
                    strClient = lProces.strClient;
                    strServer = lProces.strServer;
                    lProces.OpenNDSConnection(ref cnNDS);
                    if (cnNDS.State == ConnectionState.Open)
                    {
                        if (lUserID != null && lUserID.Split('@').Length == 2)
                        {
                            lSQL = "SELECT intUserId FROM dbo.NDSUsers WHERE vchLoginId = '" + lUserID.Split('@')[0] + "' ";
                            lCmd = new SqlCommand(lSQL, cnNDS);
                            lCmd.Transaction = osqlTransNDS;
                            lCmd.CommandTimeout = 1200;
                            adoRst = lCmd.ExecuteReader();
                            if (adoRst.HasRows)
                            {
                                if (adoRst.Read())
                                {
                                    lNDSUserID = adoRst.GetValue(0) == DBNull.Value ? 111 : adoRst.GetInt32(0);
                                }
                            }
                            adoRst.Close();
                        }

                        for (int i = 0; i < PostHeaderIDs.Count; i++)
                        {
                            int lHeaderID = PostHeaderIDs[i];
                            string lSOR = PostSORs[i];

                            lSQL = "SELECT tntStatusId FROM dbo.BBSPostHeader WHERE intPostHeaderid = " + lHeaderID + " ";

                            lCmd = new SqlCommand(lSQL, cnNDS);
                            lCmd.Transaction = osqlTransNDS;
                            lCmd.CommandTimeout = 1200;
                            adoRst = lCmd.ExecuteReader();
                            if (adoRst.HasRows)
                            {
                                if (adoRst.Read())
                                {
                                    lStatus = adoRst.GetValue(0) == DBNull.Value ? 0 : adoRst.GetByte(0);
                                }
                            }
                            adoRst.Close();

                            lSQL = "SELECT tntStatusId FROM dbo.BBSReleaseDetails WHERE intPostHeaderid = " + lHeaderID + " ";

                            lCmd = new SqlCommand(lSQL, cnNDS);
                            lCmd.Transaction = osqlTransNDS;
                            lCmd.CommandTimeout = 1200;
                            adoRst = lCmd.ExecuteReader();
                            if (adoRst.HasRows)
                            {
                                if (adoRst.Read())
                                {
                                    liVar = adoRst.GetValue(0) == DBNull.Value ? 0 : adoRst.GetByte(0);
                                    if (liVar == 12 || liVar == 14)
                                    {
                                        lStatus = liVar;
                                    }
                                }
                            }
                            adoRst.Close();

                            if (lStatus == 3)
                            {
                                lSQL = "SELECT intProjectId, " +
                                "intWBSElementId, " +
                                "vchBBSNo " +
                                "FROM dbo.BBSPostHeader " +
                                "WHERE intPostHeaderId = " + lHeaderID.ToString() + " ";

                                lCmd = new SqlCommand(lSQL, cnNDS);
                                lCmd.CommandTimeout = 1200;
                                lCmd.Transaction = osqlTransNDS;
                                adoRst = lCmd.ExecuteReader();
                                if (adoRst.HasRows)
                                {
                                    if (adoRst.Read())
                                    {
                                        lProjectID = adoRst.GetValue(0) == DBNull.Value ? 0 : adoRst.GetInt32(0);
                                        lWBSElementID = adoRst.GetValue(1) == DBNull.Value ? 0 : adoRst.GetInt32(1);
                                        lBBSNo = adoRst.GetValue(2) == DBNull.Value ? "" : adoRst.GetString(2);
                                    }
                                }
                                adoRst.Close();

                                // BBS Auto-Release
                                lSQL = "dbo.BBSReleaseBySOR_Insert ";
                                lCmd = new SqlCommand(lSQL, cnNDS);
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
                                lCmd.Parameters["@chrBBSStatus"].Value = "R";
                                lCmd.Parameters["@UserID"].Value = lNDSUserID;
                                lCmd.CommandTimeout = 1200;
                                lCmd.ExecuteNonQuery();
                            }
                            else
                            {
                                lSuccess = false;
                                lErrorMsg = "BBS Release failed as there is an unposted WBS BBS selected.";
                            }
                        }
                    }
                    else
                    {
                        lSuccess = false;
                        lErrorMsg = "BBS release failed as database connection issue.";
                    }
                }
                else
                {
                    lSuccess = false;
                    lErrorMsg = "Invalid WBS and BBS provided for release.";
                }
            }
            catch (Exception ex)
            {
                lSuccess = false;
                lErrorMsg = ex.Message;
            }

            lCmd = null;
            adoRst = null;

            return Ok(new { success = lSuccess, responseText = lErrorMsg });
        }

        [HttpPost]
        public ActionResult getReportParam(List<int> PostHeaderIDs)
        {
            string lSQL;
            SqlCommand lCmd;
            SqlDataReader adoRst;
            var lSuccess = true;
            var lErrorMsg = "";

            string lPostHeaderIdCom = "";
            int lPostHeaderId_bk = 0;
            int lProjectId_bk = 0;
            int lStructureElementTypeId_bk = 0;
            int lProductTypeId_bk = 0;

            var lReturn = new[] { new {
                PostHeaderId = "",
                ProjectId = 0,
                StructureElementTypeId = 0,
                ProductTypeId = 0
            } }.ToList();

            lReturn.RemoveAt(0);

            if (PostHeaderIDs != null && PostHeaderIDs.Count > 0)
            {
                try
                {
                    var lProces = new ProcessController();
                    lProces.OpenNDSConnection(ref cnNDS);
                    if (cnNDS.State == ConnectionState.Open)
                    {
                        var lPostID = "";
                        var lCT = 0;
                        for (int i = 0; i < PostHeaderIDs.Count; i++)
                        {
                            if (PostHeaderIDs[i] > 0)
                            {
                                lPostID = lPostID + "," + PostHeaderIDs[i].ToString();
                                lCT = lCT + 1;
                            }
                        }
                        if (lPostID.Length > 0)
                        {
                            lPostID = lPostID.Substring(1);
                        }

                        lSQL = "SELECT intPostHeaderId " +
                        ", intProjectId " +
                        ", intStructureElementTypeId " +
                        ", sitProductTypeId " +
                        "FROM dbo.BBSPostHeader " +
                        "WHERE intPostHeaderId in (" + lPostID + ") " +
                        "ORDER BY intProjectId, " +
                        "intStructureElementTypeId, " +
                        "sitProductTypeId ";

                        lCmd = new SqlCommand(lSQL, cnNDS);
                        lCmd.CommandTimeout = 1200;
                        adoRst = lCmd.ExecuteReader();
                        if (adoRst.HasRows)
                        {
                            lPostHeaderIdCom = "";
                            while (adoRst.Read())
                            {
                                var lPostHeaderId = adoRst.GetValue(0) == DBNull.Value ? 0 : adoRst.GetInt32(0);
                                var lProjectId = adoRst.GetValue(1) == DBNull.Value ? 0 : adoRst.GetInt32(1);
                                var lStructureElementTypeId = adoRst.GetValue(2) == DBNull.Value ? 0 : adoRst.GetInt32(2);
                                var lProductTypeId = adoRst.GetValue(3) == DBNull.Value ? 0 : adoRst.GetInt16(3);
                                if (lPostHeaderIdCom != "" && lProjectId_bk != 0 &&
                                (lProjectId_bk != lProjectId ||
                                lStructureElementTypeId_bk != lStructureElementTypeId ||
                                lProductTypeId_bk != lProductTypeId))
                                {
                                    lReturn.Add(new
                                    {
                                        PostHeaderId = lPostHeaderIdCom,
                                        ProjectId = lProjectId_bk,
                                        StructureElementTypeId = lStructureElementTypeId_bk,
                                        ProductTypeId = lProductTypeId_bk
                                    });

                                    lPostHeaderIdCom = "";
                                }

                                if (lPostHeaderIdCom == "")
                                {
                                    lPostHeaderIdCom = lPostHeaderId.ToString();
                                }
                                else
                                {
                                    lPostHeaderIdCom = lPostHeaderIdCom + "," + lPostHeaderId.ToString();
                                }

                                lPostHeaderId_bk = lPostHeaderId;
                                lProjectId_bk = lProjectId;
                                lStructureElementTypeId_bk = lStructureElementTypeId;
                                lProductTypeId_bk = lProductTypeId;
                            }
                            if (lPostHeaderIdCom != "")
                            {
                                lReturn.Add(new
                                {
                                    PostHeaderId = lPostHeaderIdCom,
                                    ProjectId = lProjectId_bk,
                                    StructureElementTypeId = lStructureElementTypeId_bk,
                                    ProductTypeId = lProductTypeId_bk
                                });
                            }
                        }
                        adoRst.Close();

                        lProces.CloseNDSConnection(ref cnNDS);
                    }
                }
                catch (Exception ex)
                {
                    lSuccess = false;
                    lErrorMsg = ex.Message;
                }
            }
            return Ok(new { success = lSuccess, responseText = lErrorMsg, content = lReturn });
        }

    }
}


