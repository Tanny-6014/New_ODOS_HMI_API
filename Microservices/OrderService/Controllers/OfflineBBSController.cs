using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using OrderService.Dtos;
using OrderService.Models;
using System.Data.SqlClient;

namespace OrderService.Controllers
{
    public class OfflineBBSController : Controller
    {
        public string gUserType = "";
        public string gGroupName = "";

        struct struOrderList
        {
            public string OrderNo;
            public string OrderDesc;
        };

        private DBContextModels db = new DBContextModels();


        [HttpPost]
        [Route("BBSDetails_Post")]
        public ActionResult BBSDetails([FromBody]BBSDetailsDto bBSDetailsDto)
        {
            string appCustomerCode = bBSDetailsDto.appCustomerCode;
            string appProjectCode = bBSDetailsDto.appProjectCode;
            int appSelectedCount = bBSDetailsDto.appSelectedCount;
            string appSelectedSE = bBSDetailsDto.appSelectedSE;
            string appSelectedProd = bBSDetailsDto.appSelectedProd;
            string appSelectedPostID = bBSDetailsDto.appSelectedPostID;
            string appSelectedScheduled = bBSDetailsDto.appSelectedScheduled;
            string appSelectedWBS1 = bBSDetailsDto.appSelectedWBS1;
            string appSelectedWBS2 = bBSDetailsDto.appSelectedWBS2;
            string appSelectedWBS3 = bBSDetailsDto.appSelectedWBS3;
            string appSelectedWT = bBSDetailsDto.appSelectedWT;
            string appSelectedQty = bBSDetailsDto.appSelectedQty;
            string appWBS1S = bBSDetailsDto.appWBS1S;
            string appWBS2S = bBSDetailsDto.appWBS2S;
            string appWBS3S = bBSDetailsDto.appWBS3S;
            string appOrderNoS = bBSDetailsDto.appOrderNoS;
            string appStructureElement = bBSDetailsDto.appStructureElement;
            string appProductType = bBSDetailsDto.appProductType;
            string appScheduledProd = bBSDetailsDto.appScheduledProd;
            int appPostID = bBSDetailsDto.appPostID;
            string appOrderNo = bBSDetailsDto.appOrderNo;
            string appWBS1 = bBSDetailsDto.appWBS1;
            string appWBS2 = bBSDetailsDto.appWBS2;
            string appWBS3 = bBSDetailsDto.appWBS3;
            UserAccessController lUa = new UserAccessController();
            var lUserType = lUa.getUserType(bBSDetailsDto.UserName);
            var lGroupName = lUa.getGroupName(bBSDetailsDto.UserName);

            ViewBag.UserType = lUserType;

            string lUserName = bBSDetailsDto.UserName;

            //if (lUserName.IndexOf("@") > 0)
            //{
            //    lUserName = lUserName.Substring(0, lUserName.IndexOf("@"));
            //}

            ViewBag.UserName = lUserName;

            lUa = null;

            var lStransportMode = "";

            CustomerModels lCustomerModel = db.Customer.Find(appCustomerCode);
            var CustomerSelection = Json(new { Value = appCustomerCode, Text = lCustomerModel == null ? "" : lCustomerModel.CustomerName });
            // Commented by Ajit
            //START
            //ViewBag.CustomerSelection = new SelectList(new List<SelectListItem>
            //{ new SelectListItem
            //{
            //    Value = appCustomerCode,
            //    Text = lCustomerModel == null? "":lCustomerModel.CustomerName
            //}
            //}, "Value", "Text");

            var lProjectModel = (from p in db.ProjectList
                                 where p.ProjectCode == appProjectCode
                                 select p).First();
            var ProjectSelection = Json(new { Value = appProjectCode, Text = lProjectModel == null ? "" : lProjectModel.ProjectTitle });


            //ViewBag.ProjectSelection = new SelectList(new List<SelectListItem>
            //{ new SelectListItem
            //{
            //    Value = appProjectCode,
            //    Text = lProjectModel == null? "":lProjectModel.ProjectTitle
            //}
            //}, "Value", "Text");
            //END

            int lJobID = 0;
            string lOrderStatus = "New";
            string lPONumber = "";

            int lOrderNoT = 0;
            int.TryParse(appOrderNo, out lOrderNoT);

            var lSE = db.OrderProjectSE.Find(lOrderNoT, appStructureElement, appProductType, appScheduledProd);
            if (lSE != null)
            {
                lJobID = lSE.CABJobID;
                lOrderStatus = lSE.OrderStatus;
                lPONumber = lSE.PONumber;
                lStransportMode = lSE.TransportMode;
            }

            var lJobAdv = db.JobAdvice.Find(appCustomerCode, appProjectCode, lJobID);

            var lMain = db.OrderProject.Find(lOrderNoT);

            if (lOrderStatus == null || lOrderStatus == "Reserved")
            {
                lOrderStatus = "New";
            }

            if (lUserName.Split('@').Length == 2)
            {
                if (lOrderStatus == "Created*" && lUserName.Split('@')[1].ToLower().Trim() == "natsteel.com.sg")
                {
                    lOrderStatus = "Created";
                }
            }

            ViewBag.PONumber = lPONumber;
            ViewBag.StructureEle = appStructureElement;
            ViewBag.OrderNumber = lOrderNoT;
            ViewBag.JobID = lJobID;

            ViewBag.OrderStatus = lOrderStatus;

            ViewBag.SelectedCount = appSelectedCount;
            ViewBag.SelectedSE = appSelectedSE.Split(',');
            ViewBag.SelectedProd = appSelectedProd.Split(',');
            ViewBag.SelectedPostID = appSelectedPostID.Split(',');
            ViewBag.SelectedScheduled = appSelectedScheduled.Split(',');

            ViewBag.SelectedWT = appSelectedWT == null ? new string[] { } : appSelectedWT.Split(',');
            ViewBag.SelectedQty = appSelectedQty == null ? new string[] { } : appSelectedQty.Split(',');

            ViewBag.SelectedWBS1 = appSelectedWBS1.Split(',');
            ViewBag.SelectedWBS2 = appSelectedWBS2.Split(',');
            ViewBag.SelectedWBS3 = appSelectedWBS3.Split(',');

            ViewBag.WBS1 = appWBS1S.Split(',');
            ViewBag.WBS2 = appWBS2S.Split(',');
            ViewBag.WBS3 = appWBS3S.Split(',');

            ViewBag.WBS1T = appWBS1;
            ViewBag.WBS2T = appWBS2;
            ViewBag.WBS3T = appWBS3;
            ViewBag.OrderNo = appOrderNoS.Split(',');

            ViewBag.StandbyOrder = appScheduledProd;
            ViewBag.PostID = appPostID;

            ViewBag.TransportMode = lStransportMode;

            var lSubmission = "No";
            var lEditable = "No";

            //get Access right;
            if (lUserType == "CU" || lUserType == "CA" || lUserType == "CM")
            {
                var lAccess = db.UserAccess.Find(User.Identity.Name, appCustomerCode, appProjectCode);
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

            ViewBag.AlertMessage = new List<string>();
            //var lSharedPrg = new SharedAPIController();
            //ViewBag.AlertMessage = lSharedPrg.getAlertMessage(appCustomerCode, appProjectCode, lUserName, lSubmission, lEditable);
            //lSharedPrg = null;


            //return View();
            return Json(new { CustomerSelection = CustomerSelection, 
                ProjectSelection = ProjectSelection, 
                PONumber = lPONumber, StructureEle = appStructureElement, 
                OrderNumber = lOrderNoT, JobID = lJobID, OrderStatus = lOrderStatus,
                SelectedCount= appSelectedCount,
                SelectedSE= appSelectedSE.Split(','),
                SelectedProd= appSelectedProd.Split(','),
                SelectedPostID= appSelectedPostID.Split(','),
                SelectedScheduled= appSelectedScheduled.Split(','),
                SelectedWT= appSelectedWT.Split(','),
                SelectedQty= appSelectedQty.Split(','),
                SelectedWBS1= appSelectedWBS1.Split(','),
                SelectedWBS2= appSelectedWBS2.Split(','),
                SelectedWBS3= appSelectedWBS3.Split(","),
                WBS1= appWBS1.Split(","),
                WBS2= appWBS2.Split(","),
                WBS3= appWBS3.Split(";"),
                WBS1T= appWBS1.Split(","),
                WBS2T= appWBS2.Split(","),
                WBS3T= appWBS3.Split(","),
                OrderNo= appOrderNoS.Split(','),
                StandbyOrder= appScheduledProd,
                PostID= appPostID,
                TransportMode= lStransportMode,
                Submission= lSubmission,
                Editable= lEditable
            });
       
        }


        [HttpGet]
        [Route("OfflineBBS")]
        public ActionResult OfflineBBS()
        {
            return View();
        }

      
        [HttpGet]
        [Route("BBSDetails")]
        public ActionResult BBSDetails()
        {
            return View();
        }

        
 

        // Offline BBS Submission
        [HttpPost]
        [Route("saveOrder")]
        public ActionResult saveOrder([FromBody]SaveOrderDto saveOrderDto)
        {
            string CustomerCode = saveOrderDto.CustomerCode;
            string ProjectCode = saveOrderDto.ProjectCode;
            string WBS1 = saveOrderDto.WBS1;
            int OrderNumber = saveOrderDto.OrderNumber;
            string PONumber = saveOrderDto.PONumber;
            string PODesc = saveOrderDto.PODesc;
            string CouplerType = saveOrderDto.CouplerType;
            string Transport = saveOrderDto.Transport;
            var lErrorMsg = "";
            var lCmd = new SqlCommand();
            SqlDataReader lRst;
            var lNDSCon = new SqlConnection();
            int lFound = 0;

            var lSiteEngr_Name = "";
            var lSiteEngr_HP = "";
            var lSiteEngr_Email = "";
            var lScheduler_Name = "";
            var lScheduler_HP = "";
            var lScheduler_Email = "";
            var lDeliveryAddress = "";
            var lRemarks = "";
            var lJobID = 0;

            try
            {
                var lProcessObj = new ProcessController();
                if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
                {
                    int lLastJobID = 0;

                    if (WBS1 != null && WBS1.Trim() != "")
                    {
                        lCmd.CommandText =
                        "SELECT isNull(MAX(OrderJobID), 0) FROM dbo.OESProjOrder " +
                        "WHERE CustomerCode = '" + CustomerCode + "' " +
                        "AND ProjectCode = '" + ProjectCode + "' " +
                        "AND OrderStatus <> 'New' " +
                        "AND OrderStatus <> 'Reserved' " +
                        "AND OrderStatus <> 'Created' " +
                        "AND OrderStatus <> 'Deleted' " +
                        "AND OrderStatus <> 'Cancelled' " +
                        "AND WBS1 = '" + WBS1.Trim() + "' " +
                        "AND (SiteEngr_Name > '' " +
                        "OR Scheduler_Name > '' " +
                        "OR DeliveryAddress > '') ";

                        lCmd.Connection = lNDSCon;
                        lCmd.CommandTimeout = 300;
                        lRst = lCmd.ExecuteReader();
                        if (lRst.HasRows)
                        {
                            if (lRst.Read())
                            {
                                lLastJobID = lRst.GetInt32(0);
                            }
                        }
                        lRst.Close();
                    }

                    if (lLastJobID == 0)
                    {
                        lCmd.CommandText =
                            "SELECT isNull(MAX(OrderJobID), 0) FROM dbo.OESProjOrder " +
                            "WHERE CustomerCode = '" + CustomerCode + "' " +
                            "AND ProjectCode = '" + ProjectCode + "' " +
                            "AND OrderStatus <> 'New' " +
                            "AND OrderStatus <> 'Reserved' " +
                            "AND OrderStatus <> 'Created' " +
                            "AND OrderStatus <> 'Deleted' " +
                            "AND OrderStatus <> 'Cancelled' " +
                            "AND UpdateBy = '" + saveOrderDto.UserName + "' " +
                            "AND(SiteEngr_Name > '' " +
                            "OR Scheduler_Name > '' " +
                            "OR DeliveryAddress > '') ";

                        lCmd.Connection = lNDSCon;
                        lCmd.CommandTimeout = 300;
                        lRst = lCmd.ExecuteReader();
                        if (lRst.HasRows)
                        {
                            if (lRst.Read())
                            {
                                lLastJobID = lRst.GetInt32(0);
                            }
                        }
                        lRst.Close();
                    }

                    if (lLastJobID == 0)
                    {
                        lCmd.CommandText =
                            "SELECT isNull(MAX(OrderJobID), 0) FROM dbo.OESProjOrder " +
                            "WHERE CustomerCode = '" + CustomerCode + "' " +
                            "AND ProjectCode = '" + ProjectCode + "' " +
                            "AND OrderStatus <> 'New' " +
                            "AND OrderStatus <> 'Reserved' " +
                            "AND OrderStatus <> 'Created' " +
                            "AND OrderStatus <> 'Deleted' " +
                            "AND OrderStatus <> 'Cancelled' " +
                            "AND(SiteEngr_Name > '' " +
                            "OR Scheduler_Name > '' " +
                            "OR DeliveryAddress > '') ";

                        lCmd.Connection = lNDSCon;
                        lCmd.CommandTimeout = 300;
                        lRst = lCmd.ExecuteReader();
                        if (lRst.HasRows)
                        {
                            if (lRst.Read())
                            {
                                lLastJobID = lRst.GetInt32(0);
                            }
                        }
                        lRst.Close();
                    }

                    if (lLastJobID > 0)
                    {

                        lCmd.CommandText =
                        "SELECT isNull(SiteEngr_Name, ''), " +
                        "isNull(SiteEngr_HP, ''), " +
                        "isNull(SiteEngr_Email, ''), " +
                        "isNull(Scheduler_Name, ''), " +
                        "isNull(Scheduler_HP, ''), " +
                        "isNull(Scheduler_Email, ''), " +
                        "isNull(DeliveryAddress, ''), " +
                        "isNull(Remarks, '') " +
                        "FROM dbo.OESProjOrder " +
                        "WHERE CustomerCode = '" + CustomerCode + "' " +
                        "AND ProjectCode = '" + ProjectCode + "' " +
                        "AND OrderJobID = " + lLastJobID.ToString() + " ";

                        lCmd.Connection = lNDSCon;
                        lCmd.CommandTimeout = 300;
                        lRst = lCmd.ExecuteReader();
                        if (lRst.HasRows)
                        {
                            if (lRst.Read())
                            {
                                lSiteEngr_Name = lRst.GetString(0);
                                lSiteEngr_HP = lRst.GetString(1);
                                lSiteEngr_Email = lRst.GetString(2);
                                lScheduler_Name = lRst.GetString(3);
                                lScheduler_HP = lRst.GetString(4);
                                lScheduler_Email = lRst.GetString(5);
                                lDeliveryAddress = lRst.GetString(6);
                                lRemarks = lRst.GetString(7);
                            }
                        }
                        lRst.Close();

                    }
                    else
                    {
                        lCmd.CommandText =
                        "SELECT isNull(SiteEngr_Name, ''), " +
                        "isNull(SiteEngr_HP, ''), " +
                        "isNull(SiteEngr_Tel, ''), " +
                        "isNull(Scheduler_Name, ''), " +
                        "isNull(Scheduler_HP, ''), " +
                        "isNull(Scheduler_Tel, '') " +
                        "FROM dbo.OESProject " +
                        "WHERE CustomerCode = '" + CustomerCode + "' " +
                        "AND ProjectCode = '" + ProjectCode + "' ";

                        lCmd.Connection = lNDSCon;
                        lCmd.CommandTimeout = 300;
                        lRst = lCmd.ExecuteReader();
                        if (lRst.HasRows)
                        {
                            if (lRst.Read())
                            {
                                lSiteEngr_Name = lRst.GetString(0);
                                lSiteEngr_HP = lRst.GetString(1);
                                lSiteEngr_Email = lRst.GetString(2);
                                lScheduler_Name = lRst.GetString(3);
                                lScheduler_HP = lRst.GetString(4);
                                lScheduler_Email = lRst.GetString(5);
                                lDeliveryAddress = "";
                                lRemarks = "";
                            }
                        }
                        lRst.Close();
                    }

                    PODesc = PODesc.Replace("'", "");

                    lCmd.CommandText =
                    "UPDATE dbo.OESProjOrder " +
                    "SET PONumber = '" + PONumber + "', " +
                    "SiteEngr_Name = '" + lSiteEngr_Name + "', " +
                    "SiteEngr_HP = '" + lSiteEngr_HP + "', " +
                    "SiteEngr_Email = '" + lSiteEngr_Email + "', " +
                    "Scheduler_Name = '" + lScheduler_Name + "', " +
                    "Scheduler_HP = '" + lScheduler_HP + "', " +
                    "Scheduler_Email = '" + lScheduler_Email + "', " +
                    "DeliveryAddress = '" + lDeliveryAddress + "', " +
                    "Remarks = '" + PODesc + "', " +
                    "OrderSource = 'OL' " +
                    "WHERE OrderNumber = " + OrderNumber + " ";

                    lCmd.Connection = lNDSCon;
                    lCmd.CommandTimeout = 300;
                    lCmd.ExecuteNonQuery();

                    lCmd.CommandText =
                    "UPDATE dbo.OESProjOrdersSE " +
                    "SET PONumber = '" + PONumber + "' " +
                    "WHERE OrderNumber = " + OrderNumber + " ";

                    lCmd.Connection = lNDSCon;
                    lCmd.CommandTimeout = 300;
                    lCmd.ExecuteNonQuery();

                    lCmd.CommandText =
                    "SELECT CABJobID " +
                    "FROM dbo.OESProjOrdersSE " +
                    "WHERE OrderNumber = " + OrderNumber + " ";

                    lCmd.Connection = lNDSCon;
                    lCmd.CommandTimeout = 300;
                    lRst = lCmd.ExecuteReader();
                    if (lRst.HasRows)
                    {
                        if (lRst.Read())
                        {
                            lJobID = lRst.GetInt32(0);
                        }
                    }
                    lRst.Close();

                    if (lJobID > 0)
                    {
                        lCmd.CommandText =
                        "UPDATE dbo.OESJobAdvice " +
                        "SET PONumber = '" + PONumber + "', " +
                        "CouplerType = '" + CouplerType + "', " +
                        "TransportLimit = '" + Transport + "', " +
                        "OrderSource = 'OL' " +
                        "WHERE CustomerCode = '" + CustomerCode + "' " +
                        "AND ProjectCode = '" + ProjectCode + "' " +
                        "AND JobID = " + lJobID + " ";

                        lCmd.Connection = lNDSCon;
                        lCmd.CommandTimeout = 300;
                        lCmd.ExecuteNonQuery();

                    }

                    lProcessObj.CloseNDSConnection(ref lNDSCon);
                }


                lProcessObj = null;

            }
            catch (Exception ex)
            {
                lErrorMsg = ex.Message;
            }

            return Json(lJobID);
        }


        //save BBS
        [HttpPost]
        [Route("saveBBS_Offline")]
        public ActionResult saveBBS(List<BBSModels> bbsModels)
        {
            var lErrorMsg = "";
            if (bbsModels != null && bbsModels.Count > 0)
            {
                try
                {
                    for (int i = 0; i < bbsModels.Count; i++)
                    {
                        bbsModels[i].UpdateDate = DateTime.Now;
                        bbsModels[i].UpdateBy = User.Identity.GetUserName();
                        if (bbsModels[i].CustomerCode == null)
                        {
                            bbsModels[i].CustomerCode = "";
                        }
                        if (bbsModels[i].BBSNo == null)
                        {
                            bbsModels[i].BBSNo = "";
                        }
                        if (bbsModels[i].BBSDesc == null)
                        {
                            bbsModels[i].BBSDesc = "";
                        }
                        bbsModels[i].BBSNo = bbsModels[i].BBSNo.ToUpper();
                        bbsModels[i].BBSDesc = bbsModels[i].BBSDesc.ToUpper();

                        if (bbsModels[i].CustomerCode.Length > 0)
                        {
                            var oldBBS = db.BBS.Find(bbsModels[i].CustomerCode, bbsModels[i].ProjectCode, bbsModels[i].JobID, bbsModels[i].BBSID);
                            if (oldBBS == null)
                            {
                                db.BBS.Add(bbsModels[i]);
                            }
                            else
                            {
                                db.Entry(oldBBS).CurrentValues.SetValues(bbsModels[i]);
                            }
                        }
                    }
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    lErrorMsg = ex.Message;
                }
            }
            return Json(lErrorMsg);
        }

        //save BBS
        [HttpPost]
        [Route("saveBarDetails/{OrderNumber}/{LastRebars}")]
        public ActionResult saveBarDetails(int OrderNumber, int LastRebars, List<OrderDetailsListModels> orderDetailsListModels)
        {
            bool lSuccess = true;
            var lCmd = new SqlCommand();
            SqlDataReader lRst;
            var lNDSCon = new SqlConnection();
            string lErrorMsg = "";
            string lSQL = "";
            string lOldShape = "";

            decimal lTotalCABWT = 0;
            decimal lTotalSBWT = 0;
            decimal lTotalWT = 0;
            int lTotalCABPCS = 0;
            int lTotalSBPCS = 0;
            string lCustomerCode = "";
            string lProjectCode = "";
            int lJobID = 0;

            if (orderDetailsListModels.Count > 0)
            {
                try
                {
                    var lProcessObj = new ProcessController();
                    if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
                    {
                        for (int i = 0; i < orderDetailsListModels.Count; i++)
                        {
                            OrderDetailsModels orderDetailsModels = new OrderDetailsModels();
                            string lShapeCode = orderDetailsListModels[i].BarShapeCode;
                            if (lShapeCode == null)
                            {
                                lShapeCode = "";
                            }
                            if (lShapeCode != "")
                            {
                                while (lShapeCode.Length < 3)
                                {
                                    lShapeCode = "0" + lShapeCode;
                                }

                                orderDetailsListModels[i].BarShapeCode = lShapeCode;
                            }

                            if (orderDetailsListModels[i].BarMark == null)
                            {
                                orderDetailsListModels[i].BarMark = "";
                            }
                            if (orderDetailsListModels[i].BarMark.IndexOf(",") >= 0)
                            {
                                orderDetailsListModels[i].BarMark = orderDetailsListModels[i].BarMark.Replace(",", "");
                            }

                            if (orderDetailsListModels[i].CustomerCode == null)
                            {
                                orderDetailsListModels[i].CustomerCode = "";
                            }
                            if (orderDetailsListModels[i].CustomerCode.Length > 0)
                            {
                                lCustomerCode = orderDetailsListModels[i].CustomerCode;
                                lProjectCode = orderDetailsListModels[i].ProjectCode;
                                lJobID = orderDetailsListModels[i].JobID;
                                int lBBSID = orderDetailsListModels[i].BBSID;
                                int lBarID = orderDetailsListModels[i].BarID;

                                int lFound = 0;
                                lSQL = "SELECT BarID, BarShapeCode " +
                                "FROM dbo.OESOrderDetails WITH (NOLOCK) " +
                                "WHERE CustomerCode = '" + lCustomerCode + "' " +
                                "AND ProjectCode = '" + lProjectCode + "' " +
                                "AND JobID = " + lJobID.ToString() + " " +
                                "AND BBSID = " + lBBSID.ToString() + " " +
                                "AND BarID = " + lBarID.ToString() + " ";

                                lCmd.Connection = lNDSCon;
                                lCmd.CommandText = lSQL;
                                lCmd.CommandTimeout = 1200;
                                lRst = lCmd.ExecuteReader();
                                if (lRst.HasRows)
                                {
                                    lRst.Read();
                                    lOldShape = lRst.GetString(1);
                                    lFound = 1;
                                }
                                lRst.Close();

                                if (orderDetailsListModels[i].Remarks != null)
                                {
                                    orderDetailsListModels[i].Remarks = orderDetailsListModels[i].Remarks.Replace("'", "''");
                                }
                                if (orderDetailsListModels[i].ElementMark != null)
                                {
                                    orderDetailsListModels[i].ElementMark = orderDetailsListModels[i].ElementMark.Replace("'", "''");
                                }
                                if (orderDetailsListModels[i].BarMark != null)
                                {
                                    orderDetailsListModels[i].BarMark = orderDetailsListModels[i].BarMark.Replace("'", "''");
                                }

                                if (lFound == 0)
                                {
                                    lSQL = "" +
                                    "INSERT INTO dbo.OESOrderDetails " +
                                    "(CustomerCode " +
                                    ", ProjectCode " +
                                    ", JobID " +
                                    ", BBSID " +
                                    ", BarID " +
                                    ", BarSort " +
                                    ", Cancelled " +
                                    ", BarCAB " +
                                    ", BarSTD " +
                                    ", ElementMark " +
                                    ", BarMark " +
                                    ", BarType " +
                                    ", BarSize " +
                                    ", BarMemberQty " +
                                    ", BarEachQty " +
                                    ", BarTotalQty " +
                                    ", BarShapeCode " +
                                    ", A " +
                                    ", B " +
                                    ", C " +
                                    ", D " +
                                    ", E " +
                                    ", F " +
                                    ", G " +
                                    ", H " +
                                    ", I " +
                                    ", J " +
                                    ", K " +
                                    ", L " +
                                    ", M " +
                                    ", N " +
                                    ", O " +
                                    ", P " +
                                    ", Q " +
                                    ", R " +
                                    ", S " +
                                    ", T " +
                                    ", U " +
                                    ", V " +
                                    ", W " +
                                    ", X " +
                                    ", Y " +
                                    ", Z " +
                                    ", A2 " +
                                    ", B2 " +
                                    ", C2 " +
                                    ", D2 " +
                                    ", E2 " +
                                    ", F2 " +
                                    ", G2 " +
                                    ", H2 " +
                                    ", I2 " +
                                    ", J2 " +
                                    ", K2 " +
                                    ", L2 " +
                                    ", M2 " +
                                    ", N2 " +
                                    ", O2 " +
                                    ", P2 " +
                                    ", Q2 " +
                                    ", R2 " +
                                    ", S2 " +
                                    ", T2 " +
                                    ", U2 " +
                                    ", V2 " +
                                    ", W2 " +
                                    ", X2 " +
                                    ", Y2 " +
                                    ", Z2 " +
                                    ", BarLength " +
                                    ", BarLength2 " +
                                    ", BarWeight " +
                                    ", Remarks " +
                                    ", shapeTransport " +
                                    ", PinSize " +
                                    ", TeklaGUID " +
                                    ", PartGUID " +
                                    ", UpdateDate " +
                                    ", UpdateBy " +
                                    ", CreateDate " +
                                    ", CreateBy) " +
                                    "VALUES " +
                                    "( " +
                                    "'" + orderDetailsListModels[i].CustomerCode + "' " +
                                    ", '" + orderDetailsListModels[i].ProjectCode + "' " +
                                    ", " + orderDetailsListModels[i].JobID + " " +
                                    ", " + orderDetailsListModels[i].BBSID + " " +
                                    ", " + orderDetailsListModels[i].BarID + " " +
                                    ", " + orderDetailsListModels[i].BarSort + " " +
                                    ", " + (orderDetailsListModels[i].Cancelled == true ? "1" : "null") + " " +
                                    ", " + (orderDetailsListModels[i].BarSTD == true ? 0 : 1) + " " +
                                    ", " + (orderDetailsListModels[i].BarSTD == true ? "1" : "null") + " " +
                                    ", '" + orderDetailsListModels[i].ElementMark + "' " +
                                    ", '" + orderDetailsListModels[i].BarMark + "' " +
                                    ", '" + orderDetailsListModels[i].BarType + "' " +
                                    ", " + (orderDetailsListModels[i].BarSize == 0 || orderDetailsListModels[i].BarSize == null ? "null" : orderDetailsListModels[i].BarSize.ToString()) + " " +
                                    ", " + (orderDetailsListModels[i].BarMemberQty == 0 || orderDetailsListModels[i].BarMemberQty == null ? "null" : orderDetailsListModels[i].BarMemberQty.ToString()) + " " +
                                    ", " + (orderDetailsListModels[i].BarEachQty == 0 || orderDetailsListModels[i].BarEachQty == null ? "null" : orderDetailsListModels[i].BarEachQty.ToString()) + " " +
                                    ", " + (orderDetailsListModels[i].BarTotalQty == 0 || orderDetailsListModels[i].BarTotalQty == null ? "null" : orderDetailsListModels[i].BarTotalQty.ToString()) + " " +
                                    ", '" + orderDetailsListModels[i].BarShapeCode + "' " +
                                    ", " + getParaStringValue(orderDetailsListModels[i].A, 1) + " " +
                                    ", " + getParaStringValue(orderDetailsListModels[i].B, 1) + " " +
                                    ", " + getParaStringValue(orderDetailsListModels[i].C, 1) + " " +
                                    ", " + getParaStringValue(orderDetailsListModels[i].D, 1) + " " +
                                    ", " + getParaStringValue(orderDetailsListModels[i].E, 1) + " " +
                                    ", " + getParaStringValue(orderDetailsListModels[i].F, 1) + " " +
                                    ", " + getParaStringValue(orderDetailsListModels[i].G, 1) + " " +
                                    ", " + getParaStringValue(orderDetailsListModels[i].H, 1) + " " +
                                    ", " + getParaStringValue(orderDetailsListModels[i].I, 1) + " " +
                                    ", " + getParaStringValue(orderDetailsListModels[i].J, 1) + " " +
                                    ", " + getParaStringValue(orderDetailsListModels[i].K, 1) + " " +
                                    ", " + getParaStringValue(orderDetailsListModels[i].L, 1) + " " +
                                    ", " + getParaStringValue(orderDetailsListModels[i].M, 1) + " " +
                                    ", " + getParaStringValue(orderDetailsListModels[i].N, 1) + " " +
                                    ", " + getParaStringValue(orderDetailsListModels[i].O, 1) + " " +
                                    ", " + getParaStringValue(orderDetailsListModels[i].P, 1) + " " +
                                    ", " + getParaStringValue(orderDetailsListModels[i].Q, 1) + " " +
                                    ", " + getParaStringValue(orderDetailsListModels[i].R, 1) + " " +
                                    ", " + getParaStringValue(orderDetailsListModels[i].S, 1) + " " +
                                    ", " + getParaStringValue(orderDetailsListModels[i].T, 1) + " " +
                                    ", " + getParaStringValue(orderDetailsListModels[i].U, 1) + " " +
                                    ", " + getParaStringValue(orderDetailsListModels[i].V, 1) + " " +
                                    ", " + getParaStringValue(orderDetailsListModels[i].W, 1) + " " +
                                    ", " + getParaStringValue(orderDetailsListModels[i].X, 1) + " " +
                                    ", " + getParaStringValue(orderDetailsListModels[i].Y, 1) + " " +
                                    ", " + getParaStringValue(orderDetailsListModels[i].Z, 1) + " " +
                                    ", " + getParaStringValue(orderDetailsListModels[i].A, 2) + " " +
                                    ", " + getParaStringValue(orderDetailsListModels[i].B, 2) + " " +
                                    ", " + getParaStringValue(orderDetailsListModels[i].C, 2) + " " +
                                    ", " + getParaStringValue(orderDetailsListModels[i].D, 2) + " " +
                                    ", " + getParaStringValue(orderDetailsListModels[i].E, 2) + " " +
                                    ", " + getParaStringValue(orderDetailsListModels[i].F, 2) + " " +
                                    ", " + getParaStringValue(orderDetailsListModels[i].G, 2) + " " +
                                    ", " + getParaStringValue(orderDetailsListModels[i].H, 2) + " " +
                                    ", " + getParaStringValue(orderDetailsListModels[i].I, 2) + " " +
                                    ", " + getParaStringValue(orderDetailsListModels[i].J, 2) + " " +
                                    ", " + getParaStringValue(orderDetailsListModels[i].K, 2) + " " +
                                    ", " + getParaStringValue(orderDetailsListModels[i].L, 2) + " " +
                                    ", " + getParaStringValue(orderDetailsListModels[i].M, 2) + " " +
                                    ", " + getParaStringValue(orderDetailsListModels[i].N, 2) + " " +
                                    ", " + getParaStringValue(orderDetailsListModels[i].O, 2) + " " +
                                    ", " + getParaStringValue(orderDetailsListModels[i].P, 2) + " " +
                                    ", " + getParaStringValue(orderDetailsListModels[i].Q, 2) + " " +
                                    ", " + getParaStringValue(orderDetailsListModels[i].R, 2) + " " +
                                    ", " + getParaStringValue(orderDetailsListModels[i].S, 2) + " " +
                                    ", " + getParaStringValue(orderDetailsListModels[i].T, 2) + " " +
                                    ", " + getParaStringValue(orderDetailsListModels[i].U, 2) + " " +
                                    ", " + getParaStringValue(orderDetailsListModels[i].V, 2) + " " +
                                    ", " + getParaStringValue(orderDetailsListModels[i].W, 2) + " " +
                                    ", " + getParaStringValue(orderDetailsListModels[i].X, 2) + " " +
                                    ", " + getParaStringValue(orderDetailsListModels[i].Y, 2) + " " +
                                    ", " + getParaStringValue(orderDetailsListModels[i].Z, 2) + " " +
                                    ", " + getParaStringValue(orderDetailsListModels[i].BarLength, 1) + " " +
                                    ", " + getParaStringValue(orderDetailsListModels[i].BarLength, 2) + " " +
                                    ", " + (orderDetailsListModels[i].BarWeight == 0 || orderDetailsListModels[i].BarWeight == null ? "null" : orderDetailsListModels[i].BarWeight.ToString()) + " " +
                                    ", '" + (orderDetailsListModels[i].Remarks == null || orderDetailsListModels[i].Remarks == "undefined" ? "" : orderDetailsListModels[i].Remarks) + "' " +
                                    ", " + (orderDetailsListModels[i].shapeTransport == 0 || orderDetailsListModels[i].shapeTransport == null ? "null" : orderDetailsListModels[i].shapeTransport.ToString()) + " " +
                                    ", " + (orderDetailsListModels[i].PinSize == 0 || orderDetailsListModels[i].PinSize == null ? "null" : orderDetailsListModels[i].PinSize.ToString()) + " " +
                                    ", '' " +
                                    ", '' " +
                                    ", getDate() " +
                                    ", '" + User.Identity.GetUserName() + "' " +
                                    ", getDate() " +
                                    ", '" + User.Identity.GetUserName() + "' ) ";
                                }
                                else
                                {
                                    lSQL = "" +
                                    "UPDATE dbo.OESOrderDetails SET " +
                                    "BarSort = " + orderDetailsListModels[i].BarSort + " " +
                                    ", Cancelled = " + (orderDetailsListModels[i].Cancelled == true ? "1" : "null") + " " +
                                    ", BarCAB = " + (orderDetailsListModels[i].BarSTD == true ? 0 : 1) + " " +
                                    ", BarSTD = " + (orderDetailsListModels[i].BarSTD == true ? "1" : "null") + " " +
                                    ", ElementMark = '" + orderDetailsListModels[i].ElementMark + "' " +
                                    ", BarMark = '" + orderDetailsListModels[i].BarMark + "' " +
                                    ", BarType = '" + orderDetailsListModels[i].BarType + "' " +
                                    ", BarSize = " + (orderDetailsListModels[i].BarSize == 0 || orderDetailsListModels[i].BarSize == null ? "null" : orderDetailsListModels[i].BarSize.ToString()) + " " +
                                    ", BarMemberQty = " + (orderDetailsListModels[i].BarMemberQty == 0 || orderDetailsListModels[i].BarMemberQty == null ? "null" : orderDetailsListModels[i].BarMemberQty.ToString()) + " " +
                                    ", BarEachQty = " + (orderDetailsListModels[i].BarEachQty == 0 || orderDetailsListModels[i].BarEachQty == null ? "null" : orderDetailsListModels[i].BarEachQty.ToString()) + " " +
                                    ", BarTotalQty = " + (orderDetailsListModels[i].BarTotalQty == 0 || orderDetailsListModels[i].BarTotalQty == null ? "null" : orderDetailsListModels[i].BarTotalQty.ToString()) + " " +
                                    ", BarShapeCode = '" + orderDetailsListModels[i].BarShapeCode + "' " +
                                    ", A = " + getParaStringValue(orderDetailsListModels[i].A, 1) + " " +
                                    ", B = " + getParaStringValue(orderDetailsListModels[i].B, 1) + " " +
                                    ", C = " + getParaStringValue(orderDetailsListModels[i].C, 1) + " " +
                                    ", D = " + getParaStringValue(orderDetailsListModels[i].D, 1) + " " +
                                    ", E = " + getParaStringValue(orderDetailsListModels[i].E, 1) + " " +
                                    ", F = " + getParaStringValue(orderDetailsListModels[i].F, 1) + " " +
                                    ", G = " + getParaStringValue(orderDetailsListModels[i].G, 1) + " " +
                                    ", H = " + getParaStringValue(orderDetailsListModels[i].H, 1) + " " +
                                    ", I = " + getParaStringValue(orderDetailsListModels[i].I, 1) + " " +
                                    ", J = " + getParaStringValue(orderDetailsListModels[i].J, 1) + " " +
                                    ", K = " + getParaStringValue(orderDetailsListModels[i].K, 1) + " " +
                                    ", L = " + getParaStringValue(orderDetailsListModels[i].L, 1) + " " +
                                    ", M = " + getParaStringValue(orderDetailsListModels[i].M, 1) + " " +
                                    ", N = " + getParaStringValue(orderDetailsListModels[i].N, 1) + " " +
                                    ", O = " + getParaStringValue(orderDetailsListModels[i].O, 1) + " " +
                                    ", P = " + getParaStringValue(orderDetailsListModels[i].P, 1) + " " +
                                    ", Q = " + getParaStringValue(orderDetailsListModels[i].Q, 1) + " " +
                                    ", R = " + getParaStringValue(orderDetailsListModels[i].R, 1) + " " +
                                    ", S = " + getParaStringValue(orderDetailsListModels[i].S, 1) + " " +
                                    ", T = " + getParaStringValue(orderDetailsListModels[i].T, 1) + " " +
                                    ", U = " + getParaStringValue(orderDetailsListModels[i].U, 1) + " " +
                                    ", V = " + getParaStringValue(orderDetailsListModels[i].V, 1) + " " +
                                    ", W = " + getParaStringValue(orderDetailsListModels[i].W, 1) + " " +
                                    ", X = " + getParaStringValue(orderDetailsListModels[i].X, 1) + " " +
                                    ", Y = " + getParaStringValue(orderDetailsListModels[i].Y, 1) + " " +
                                    ", Z = " + getParaStringValue(orderDetailsListModels[i].Z, 1) + " " +
                                    ", A2 = " + getParaStringValue(orderDetailsListModels[i].A, 2) + " " +
                                    ", B2 = " + getParaStringValue(orderDetailsListModels[i].B, 2) + " " +
                                    ", C2 = " + getParaStringValue(orderDetailsListModels[i].C, 2) + " " +
                                    ", D2 = " + getParaStringValue(orderDetailsListModels[i].D, 2) + " " +
                                    ", E2 = " + getParaStringValue(orderDetailsListModels[i].E, 2) + " " +
                                    ", F2 = " + getParaStringValue(orderDetailsListModels[i].F, 2) + " " +
                                    ", G2 = " + getParaStringValue(orderDetailsListModels[i].G, 2) + " " +
                                    ", H2 = " + getParaStringValue(orderDetailsListModels[i].H, 2) + " " +
                                    ", I2 = " + getParaStringValue(orderDetailsListModels[i].I, 2) + " " +
                                    ", J2 = " + getParaStringValue(orderDetailsListModels[i].J, 2) + " " +
                                    ", K2 = " + getParaStringValue(orderDetailsListModels[i].K, 2) + " " +
                                    ", L2 = " + getParaStringValue(orderDetailsListModels[i].L, 2) + " " +
                                    ", M2 = " + getParaStringValue(orderDetailsListModels[i].M, 2) + " " +
                                    ", N2 = " + getParaStringValue(orderDetailsListModels[i].N, 2) + " " +
                                    ", O2 = " + getParaStringValue(orderDetailsListModels[i].O, 2) + " " +
                                    ", P2 = " + getParaStringValue(orderDetailsListModels[i].P, 2) + " " +
                                    ", Q2 = " + getParaStringValue(orderDetailsListModels[i].Q, 2) + " " +
                                    ", R2 = " + getParaStringValue(orderDetailsListModels[i].R, 2) + " " +
                                    ", S2 = " + getParaStringValue(orderDetailsListModels[i].S, 2) + " " +
                                    ", T2 = " + getParaStringValue(orderDetailsListModels[i].T, 2) + " " +
                                    ", U2 = " + getParaStringValue(orderDetailsListModels[i].U, 2) + " " +
                                    ", V2 = " + getParaStringValue(orderDetailsListModels[i].V, 2) + " " +
                                    ", W2 = " + getParaStringValue(orderDetailsListModels[i].W, 2) + " " +
                                    ", X2 = " + getParaStringValue(orderDetailsListModels[i].X, 2) + " " +
                                    ", Y2 = " + getParaStringValue(orderDetailsListModels[i].Y, 2) + " " +
                                    ", Z2 = " + getParaStringValue(orderDetailsListModels[i].Z, 2) + " " +
                                    ", BarLength = " + getParaStringValue(orderDetailsListModels[i].BarLength, 1) + " " +
                                    ", BarLength2 = " + getParaStringValue(orderDetailsListModels[i].BarLength, 2) + " " +
                                    ", BarWeight = " + (orderDetailsListModels[i].BarWeight == 0 || orderDetailsListModels[i].BarWeight == null ? "null" : orderDetailsListModels[i].BarWeight.ToString()) + " " +
                                    ", Remarks = '" + orderDetailsListModels[i].Remarks + "' " +
                                    ", shapeTransport = " + (orderDetailsListModels[i].shapeTransport == 0 || orderDetailsListModels[i].shapeTransport == null ? "null" : orderDetailsListModels[i].shapeTransport.ToString()) + " " +
                                    ", PinSize = " + (orderDetailsListModels[i].PinSize == 0 || orderDetailsListModels[i].PinSize == null ? "null" : orderDetailsListModels[i].PinSize.ToString()) + " " +
                                    ", UpdateDate = getDate() " +
                                    ", UpdateBy = '" + User.Identity.GetUserName() + "' " +
                                    "WHERE CustomerCode = '" + lCustomerCode + "' " +
                                    "AND ProjectCode = '" + lProjectCode + "' " +
                                    "AND JobID = " + lJobID.ToString() + " " +
                                    "AND BBSID = " + lBBSID.ToString() + " " +
                                    "AND BarID = " + lBarID.ToString() + " ";
                                }

                                lCmd.Connection = lNDSCon;
                                lCmd.CommandText = lSQL;
                                lCmd.CommandTimeout = 1200;
                                lCmd.ExecuteNonQuery();
                            }
                            else
                            {
                                lErrorMsg = "Invalid Customer Code";
                                lSuccess = false;
                            }
                        }

                        if (LastRebars == 1)
                        {
                            lSQL = "SELECT isNull(SUM(BarWeight),0), isNull(Sum(BarTotalQty),0) " +
                            "FROM dbo.OESOrderDetails WITH (NOLOCK) " +
                            "WHERE CustomerCode = '" + lCustomerCode + "' " +
                            "AND ProjectCode = '" + lProjectCode + "' " +
                            "AND JobID = " + lJobID.ToString() + " " +
                            "AND (Cancelled is null OR Cancelled = 0) " +
                            "AND BarSTD = 1 " +
                            "AND BarShapeCode is not null " +
                            "AND BarShapeCode <> '' ";

                            lCmd.Connection = lNDSCon;
                            lCmd.CommandText = lSQL;
                            lCmd.CommandTimeout = 1200;
                            lRst = lCmd.ExecuteReader();
                            if (lRst.HasRows)
                            {
                                lRst.Read();
                                lTotalSBWT = lRst.GetDecimal(0);
                                lTotalSBPCS = lRst.GetInt32(1);
                            }
                            lRst.Close();

                            lSQL = "SELECT isNull(SUM(BarWeight),0), isNull(Sum(BarTotalQty),0) " +
                            "FROM dbo.OESOrderDetails WITH (NOLOCK) " +
                            "WHERE CustomerCode = '" + lCustomerCode + "' " +
                            "AND ProjectCode = '" + lProjectCode + "' " +
                            "AND JobID = " + lJobID.ToString() + " " +
                            "AND (Cancelled is null OR Cancelled = 0) " +
                            "AND (BarSTD is null OR BarSTD = 0) " +
                            "AND BarShapeCode is not null " +
                            "AND BarShapeCode <> '' ";

                            lCmd.Connection = lNDSCon;
                            lCmd.CommandText = lSQL;
                            lCmd.CommandTimeout = 1200;
                            lRst = lCmd.ExecuteReader();
                            if (lRst.HasRows)
                            {
                                lRst.Read();
                                lTotalCABWT = lRst.GetDecimal(0);
                                lTotalCABPCS = lRst.GetInt32(1);
                            }
                            lRst.Close();

                            lSQL = "UPDATE dbo.OESJobAdvice " +
                            "SET OrderSource = 'OL', " +
                            "TotalCABWeight = " + lTotalCABWT.ToString("F3") + ", " +
                            "TotalSTDWeight = " + lTotalSBWT.ToString("F3") + ", " +
                            "TotalWeight = " + (lTotalSBWT + lTotalCABWT).ToString("F3") + " " +
                            "WHERE CustomerCode = '" + lCustomerCode + "' " +
                            "AND ProjectCode = '" + lProjectCode + "' " +
                            "AND JobID = " + lJobID + " ";

                            lCmd.Connection = lNDSCon;
                            lCmd.CommandText = lSQL;
                            lCmd.CommandTimeout = 1200;
                            lCmd.ExecuteNonQuery();

                            lSQL = "UPDATE dbo.OESProjOrdersSE " +
                            "SET TotalWeight = " + (lTotalSBWT + lTotalCABWT).ToString("F3") + ", " +
                            "TotalPCs = " + (lTotalCABPCS + lTotalSBPCS).ToString() + " " +
                            "WHERE OrderNumber = " + OrderNumber + " ";

                            lCmd.Connection = lNDSCon;
                            lCmd.CommandText = lSQL;
                            lCmd.CommandTimeout = 1200;
                            lCmd.ExecuteNonQuery();

                            lSQL = "UPDATE dbo.OESProjOrder " +
                            "SET OrderSource = 'OL', " +
                            "TotalWeight = " + (lTotalSBWT + lTotalCABWT).ToString("F3") + " " +
                            "WHERE OrderNumber = " + OrderNumber + " ";

                            lCmd.Connection = lNDSCon;
                            lCmd.CommandText = lSQL;
                            lCmd.CommandTimeout = 1200;
                            lCmd.ExecuteNonQuery();
                        }
                    }
                    lProcessObj.CloseNDSConnection(ref lNDSCon);
                    lProcessObj = null;
                }
                catch (Exception ex)
                {
                    lErrorMsg = ex.Message;
                    lSuccess = false;
                }
            }
            return Json(new { success = lSuccess, ErrorMessage = lErrorMsg });
        }

        private int? getParaValue(string pParameter, byte pPos)
        {
            int? lReturn = null;
            if (pParameter != null)
            {
                if (pParameter.Trim().Length > 0)
                {
                    int lVar = 0;
                    if (int.TryParse(pParameter, out lVar) == true && pPos == 1)
                    {
                        if (lVar > 0) lReturn = lVar;
                    }
                    else
                    {
                        var lArray = pParameter.Split(new[] { '-', '~' });
                        if (lArray != null)
                        {
                            if (lArray.Length >= 2)
                            {
                                lVar = 0;
                                if (pPos == 1)
                                {
                                    if (int.TryParse(lArray[0], out lVar) == true)
                                    {
                                        if (lVar > 0) lReturn = lVar;
                                    }
                                }
                                if (pPos == 2)
                                {
                                    if (int.TryParse(lArray[1], out lVar) == true)
                                    {
                                        if (lVar > 0) lReturn = lVar;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return lReturn;
        }

        private string getParaStringValue(string pParameter, byte pPos)
        {
            string lReturn = "null";
            if (pParameter != null)
            {
                if (pParameter.Trim().Length > 0)
                {
                    int lVar = 0;
                    if (int.TryParse(pParameter, out lVar) == true && pPos == 1)
                    {
                        if (lVar > 0) lReturn = lVar.ToString();
                    }
                    else
                    {
                        var lArray = pParameter.Split(new[] { '-', '~' });
                        if (lArray != null)
                        {
                            if (lArray.Length >= 2)
                            {
                                lVar = 0;
                                if (pPos == 1)
                                {
                                    if (int.TryParse(lArray[0], out lVar) == true)
                                    {
                                        if (lVar > 0) lReturn = lVar.ToString();
                                    }
                                }
                                if (pPos == 2)
                                {
                                    if (int.TryParse(lArray[1], out lVar) == true)
                                    {
                                        if (lVar > 0) lReturn = lVar.ToString();
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return lReturn;
        }

        // Offline backup PO
        [HttpPost]
        [Route("backupPO")]
        public ActionResult backupPO(List<OfflinePOModels> POData)
        {
            var lErrorMsg = "";
            var lCmd = new SqlCommand();
            SqlDataReader lRst;
            var lNDSCon = new SqlConnection();
            int lBackupID = 0;

            if (POData.Count > 0)
            {
                try
                {
                    var lProcessObj = new ProcessController();
                    if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
                    {
                        lCmd.CommandText =
                        "SELECT isNull(Max(BackupID), 0) " +
                        "FROM dbo.OESOfflBackup ";

                        lCmd.Connection = lNDSCon;
                        lCmd.CommandTimeout = 300;
                        lRst = lCmd.ExecuteReader();
                        if (lRst.HasRows)
                        {
                            if (lRst.Read())
                            {
                                lBackupID = lRst.GetInt32(0);
                            }
                        }
                        lRst.Close();

                        lBackupID = lBackupID + 1;

                        lCmd.CommandText =
                        "INSERT INTO dbo.OESOfflBackup " +
                        "(BackupID " +
                        ", BackupDate " +
                        ", BackupBy) " +
                        "VALUES " +
                        "(" + lBackupID + " " +
                        ",getDate() " +
                        ",'" + User.Identity.GetUserName() + "') ";

                        lCmd.Connection = lNDSCon;
                        lCmd.CommandTimeout = 300;
                        lCmd.ExecuteNonQuery();

                        lCmd.CommandText =
                        "DELETE FROM dbo.OESOfflPO " +
                        "WHERE BackupID = " + lBackupID + " ";

                        lCmd.Connection = lNDSCon;
                        lCmd.CommandTimeout = 300;
                        lCmd.ExecuteNonQuery();

                        lCmd.CommandText =
                        "DELETE FROM dbo.OESOfflBBS " +
                        "WHERE BackupID = " + lBackupID + " ";

                        lCmd.Connection = lNDSCon;
                        lCmd.CommandTimeout = 300;
                        lCmd.ExecuteNonQuery();

                        lCmd.CommandText =
                        "DELETE FROM dbo.OESOfflRebar " +
                        "WHERE BackupID = " + lBackupID + " ";

                        lCmd.Connection = lNDSCon;
                        lCmd.CommandTimeout = 300;
                        lCmd.ExecuteNonQuery();

                        for (int i = 0; i < POData.Count; i++)
                        {
                            lCmd.CommandText =
                            "INSERT INTO dbo.OESOfflPO " +
                            "(BackupID " +
                            ", POID " +
                            ", PONo " +
                            ", PODesc " +
                            ", StructureElement " +
                            ", BBSNo " +
                            ", BBSDesc " +
                            ", BBSTotalWT " +
                            ", BBSTotalPCs " +
                            ", CouplerType " +
                            ", Transport " +
                            ", CreatedDate " +
                            ", UpdatedDate " +
                            ", UpdatedBy) " +
                            "VALUES " +
                            "(" + lBackupID + " " +
                            "," + POData[i].POID + " " +
                            ",'" + POData[i].PONo + "' " +
                            ",'" + (POData[i].PODesc == null ? "" : POData[i].PODesc.Replace("'", "")) + "' " +
                            ",'" + (POData[i].StructureElement == null ? "" : POData[i].StructureElement.Replace("'", "")) + "' " +
                            ",'" + (POData[i].BBSNo == null ? "" : POData[i].BBSNo.Replace("'", "")) + "' " +
                            ",'" + (POData[i].BBSDesc == null ? "" : POData[i].BBSDesc.Replace("'", "")) + "' " +
                            "," + POData[i].BBSTotalWT.ToString("F3") + " " +
                            "," + POData[i].BBSTotalPCs.ToString() + " " +
                            ",'" + (POData[i].CouplerType == null ? "" : POData[i].CouplerType.Replace("'", "")) + "' " +
                            ",'" + (POData[i].Transport == null ? "" : POData[i].Transport.Replace("'", "")) + "' " +
                            ",'" + (POData[i].CreatedDate == null ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : POData[i].CreatedDate) + "' " +
                            ",'" + (POData[i].UpdatedDate == null ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : POData[i].UpdatedDate) + "'  " +
                            ",'" + User.Identity.GetUserName() + "') ";

                            lCmd.Connection = lNDSCon;
                            lCmd.CommandTimeout = 300;
                            lCmd.ExecuteNonQuery();

                        }

                        //Houskeeping
                        lCmd.CommandText =
                        "DELETE FROM dbo.OESOfflPO " +
                        "WHERE BackupID IN " +
                        "(SELECT BackupID FROM dbo.OESOfflBackup " +
                        "WHERE BackupID <> " + lBackupID + " " +
                        "AND BackupBy = '" + User.Identity.GetUserName() + "') ";

                        lCmd.Connection = lNDSCon;
                        lCmd.CommandTimeout = 300;
                        lCmd.ExecuteNonQuery();

                        lCmd.CommandText =
                        "DELETE FROM dbo.OESOfflBBS " +
                        "WHERE BackupID IN " +
                        "(SELECT BackupID FROM dbo.OESOfflBackup " +
                        "WHERE BackupID <> " + lBackupID + " " +
                        "AND BackupBy = '" + User.Identity.GetUserName() + "') ";

                        lCmd.Connection = lNDSCon;
                        lCmd.CommandTimeout = 300;
                        lCmd.ExecuteNonQuery();

                        lCmd.CommandText =
                        "DELETE FROM dbo.OESOfflRebar " +
                        "WHERE BackupID IN " +
                        "(SELECT BackupID FROM dbo.OESOfflBackup " +
                        "WHERE BackupID <> " + lBackupID + " " +
                        "AND BackupBy = '" + User.Identity.GetUserName() + "') ";

                        lCmd.Connection = lNDSCon;
                        lCmd.CommandTimeout = 300;
                        lCmd.ExecuteNonQuery();

                        lCmd.CommandText =
                        "DELETE FROM dbo.OESOfflBackup " +
                        "WHERE BackupID <> " + lBackupID + " " +
                        "AND BackupBy = '" + User.Identity.GetUserName() + "' ";

                        lCmd.Connection = lNDSCon;
                        lCmd.CommandTimeout = 300;
                        lCmd.ExecuteNonQuery();

                        lProcessObj.CloseNDSConnection(ref lNDSCon);
                    }


                    lProcessObj = null;

                }
                catch (Exception ex)
                {
                    lErrorMsg = ex.Message;
                    lBackupID = 0;
                }
            }
            return Json(lBackupID);
        }

        // Offline backup BBS
        [HttpPost]
        [Route("backupBBS/{BackupID}")]
        public ActionResult backupBBS(int BackupID, List<OfflineBBSModels> BBSData)
        {
            var lSuccess = true;
            var lErrorMsg = "";
            var lCmd = new SqlCommand();
            SqlDataReader lRst;
            var lNDSCon = new SqlConnection();

            if (BackupID > 0 && BBSData.Count > 0)
            {
                try
                {
                    var lProcessObj = new ProcessController();
                    if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
                    {
                        for (int i = 0; i < BBSData.Count; i++)
                        {
                            lCmd.CommandText =
                            "INSERT INTO dbo.OESOfflBBS " +
                            "(BackupID " +
                            ", POID " +
                            ", BBSID " +
                            ", BBSNo " +
                            ", BBSDesc " +
                            ", BBSStrucElem " +
                            ", BBSOrderCABWT " +
                            ", BBSOrderSTDWT " +
                            ", BBSTotalWT " +
                            ", UpdateDate " +
                            ", UpdateBy) " +
                            "VALUES " +
                            "(" + BackupID + " " +
                            "," + BBSData[i].POID + " " +
                            "," + BBSData[i].BBSID + " " +
                            ",'" + (BBSData[i].BBSNo == null ? "" : BBSData[i].BBSNo.Replace("'", "")) + "' " +
                            ",'" + (BBSData[i].BBSDesc == null ? "" : BBSData[i].BBSDesc.Replace("'", "")) + "' " +
                            ",'" + (BBSData[i].BBSStrucElem == null ? "" : BBSData[i].BBSStrucElem.Replace("'", "")) + "' " +
                            "," + BBSData[i].BBSOrderCABWT.ToString("F3") + " " +
                            "," + BBSData[i].BBSOrderSTDWT.ToString("F3") + " " +
                            "," + BBSData[i].BBSTotalWT.ToString("F3") + " " +
                            ",getDate() " +
                            ",'" + User.Identity.GetUserName() + "') ";

                            lCmd.Connection = lNDSCon;
                            lCmd.CommandTimeout = 300;
                            lCmd.ExecuteNonQuery();
                        }

                        lProcessObj.CloseNDSConnection(ref lNDSCon);
                    }


                    lProcessObj = null;

                }
                catch (Exception ex)
                {
                    lErrorMsg = ex.Message;
                    lSuccess = false;
                }
            }
            return Json(new { success = lSuccess, message = lErrorMsg });
        }

        // Offline backup BBS
        [HttpPost]
        [Route("backupRebar/{BackupID}")]
        public ActionResult backupRebar(int BackupID, List<OfflineRebarModels> RebarData)
        {
            var lSuccess = true;
            var lErrorMsg = "";
            var lCmd = new SqlCommand();
            SqlDataReader lRst;
            var lNDSCon = new SqlConnection();
            string lSQL = "";

            if (BackupID > 0 && RebarData.Count > 0)
            {
                try
                {
                    var lProcessObj = new ProcessController();
                    if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
                    {

                        for (int i = 0; i < RebarData.Count; i++)
                        {
                            if (RebarData[i].BarSort == null)
                            {
                                if (i > 0 && RebarData[i - 1].BarSort > 0)
                                {
                                    RebarData[i].BarSort = RebarData[i - 1].BarSort + 0.00001;
                                }
                                else
                                {
                                    RebarData[i].BarSort = 0;
                                }
                            }
                            if (Double.IsNaN((double)RebarData[i].BarSort) == true)
                            {
                                if (i > 0 && RebarData[i - 1].BarSort > 0)
                                {
                                    RebarData[i].BarSort = RebarData[i - 1].BarSort + 0.00001;
                                }
                                else
                                {
                                    RebarData[i].BarSort = 0;
                                }
                            }
                            if (RebarData[i].Remarks != null)
                            {
                                RebarData[i].Remarks = RebarData[i].Remarks.Replace("'", "''");
                            }
                            if (RebarData[i].ElementMark != null)
                            {
                                RebarData[i].ElementMark = RebarData[i].ElementMark.Replace("'", "''");
                            }
                            if (RebarData[i].ElementMark == "undefined")
                            {
                                RebarData[i].ElementMark = "";
                            }
                            if (RebarData[i].BarMark != null)
                            {
                                RebarData[i].BarMark = RebarData[i].BarMark.Replace("'", "''");
                            }

                            lSQL = "" +
                            "INSERT INTO dbo.OESOfflRebar " +
                            "(BackupID " +
                            ", POID " +
                            ", BBSID " +
                            ", BarID " +
                            ", BarSort " +
                            ", Cancelled " +
                            ", BarCAB " +
                            ", BarSTD " +
                            ", ElementMark " +
                            ", BarMark " +
                            ", BarType " +
                            ", BarSize " +
                            ", BarMemberQty " +
                            ", BarEachQty " +
                            ", BarTotalQty " +
                            ", BarShapeCode " +
                            ", A " +
                            ", B " +
                            ", C " +
                            ", D " +
                            ", E " +
                            ", F " +
                            ", G " +
                            ", H " +
                            ", I " +
                            ", J " +
                            ", K " +
                            ", L " +
                            ", M " +
                            ", N " +
                            ", O " +
                            ", P " +
                            ", Q " +
                            ", R " +
                            ", S " +
                            ", T " +
                            ", U " +
                            ", V " +
                            ", W " +
                            ", X " +
                            ", Y " +
                            ", Z " +
                            ", A2 " +
                            ", B2 " +
                            ", C2 " +
                            ", D2 " +
                            ", E2 " +
                            ", F2 " +
                            ", G2 " +
                            ", H2 " +
                            ", I2 " +
                            ", J2 " +
                            ", K2 " +
                            ", L2 " +
                            ", M2 " +
                            ", N2 " +
                            ", O2 " +
                            ", P2 " +
                            ", Q2 " +
                            ", R2 " +
                            ", S2 " +
                            ", T2 " +
                            ", U2 " +
                            ", V2 " +
                            ", W2 " +
                            ", X2 " +
                            ", Y2 " +
                            ", Z2 " +
                            ", BarLength " +
                            ", BarLength2 " +
                            ", BarWeight " +
                            ", Remarks " +
                            ", shapeTransport " +
                            ", PinSize " +
                            ", TeklaGUID " +
                            ", PartGUID " +
                            ", UpdateDate " +
                            ", UpdateBy " +
                            ", CreateDate " +
                            ", CreateBy) " +
                            "VALUES " +
                            "( " +
                            " " + BackupID + " " +
                            ", " + RebarData[i].POID + " " +
                            ", " + RebarData[i].BBSID + " " +
                            ", " + RebarData[i].BarID + " " +
                            ", " + RebarData[i].BarSort + " " +
                            ", " + (RebarData[i].Cancelled == true ? "1" : "null") + " " +
                            ", " + (RebarData[i].BarSTD == true ? 0 : 1) + " " +
                            ", " + (RebarData[i].BarSTD == true ? "1" : "null") + " " +
                            ", '" + RebarData[i].ElementMark + "' " +
                            ", '" + RebarData[i].BarMark + "' " +
                            ", '" + RebarData[i].BarType + "' " +
                            ", " + (RebarData[i].BarSize == 0 || RebarData[i].BarSize == null ? "null" : RebarData[i].BarSize.ToString()) + " " +
                            ", " + (RebarData[i].BarMemberQty == 0 || RebarData[i].BarMemberQty == null ? "null" : RebarData[i].BarMemberQty.ToString()) + " " +
                            ", " + (RebarData[i].BarEachQty == 0 || RebarData[i].BarEachQty == null ? "null" : RebarData[i].BarEachQty.ToString()) + " " +
                            ", " + (RebarData[i].BarTotalQty == 0 || RebarData[i].BarTotalQty == null ? "null" : RebarData[i].BarTotalQty.ToString()) + " " +
                            ", '" + RebarData[i].BarShapeCode + "' " +
                            ", " + getParaStringValue(RebarData[i].A, 1) + " " +
                            ", " + getParaStringValue(RebarData[i].B, 1) + " " +
                            ", " + getParaStringValue(RebarData[i].C, 1) + " " +
                            ", " + getParaStringValue(RebarData[i].D, 1) + " " +
                            ", " + getParaStringValue(RebarData[i].E, 1) + " " +
                            ", " + getParaStringValue(RebarData[i].F, 1) + " " +
                            ", " + getParaStringValue(RebarData[i].G, 1) + " " +
                            ", " + getParaStringValue(RebarData[i].H, 1) + " " +
                            ", " + getParaStringValue(RebarData[i].I, 1) + " " +
                            ", " + getParaStringValue(RebarData[i].J, 1) + " " +
                            ", " + getParaStringValue(RebarData[i].K, 1) + " " +
                            ", " + getParaStringValue(RebarData[i].L, 1) + " " +
                            ", " + getParaStringValue(RebarData[i].M, 1) + " " +
                            ", " + getParaStringValue(RebarData[i].N, 1) + " " +
                            ", " + getParaStringValue(RebarData[i].O, 1) + " " +
                            ", " + getParaStringValue(RebarData[i].P, 1) + " " +
                            ", " + getParaStringValue(RebarData[i].Q, 1) + " " +
                            ", " + getParaStringValue(RebarData[i].R, 1) + " " +
                            ", " + getParaStringValue(RebarData[i].S, 1) + " " +
                            ", " + getParaStringValue(RebarData[i].T, 1) + " " +
                            ", " + getParaStringValue(RebarData[i].U, 1) + " " +
                            ", " + getParaStringValue(RebarData[i].V, 1) + " " +
                            ", " + getParaStringValue(RebarData[i].W, 1) + " " +
                            ", " + getParaStringValue(RebarData[i].X, 1) + " " +
                            ", " + getParaStringValue(RebarData[i].Y, 1) + " " +
                            ", " + getParaStringValue(RebarData[i].Z, 1) + " " +
                            ", " + getParaStringValue(RebarData[i].A, 2) + " " +
                            ", " + getParaStringValue(RebarData[i].B, 2) + " " +
                            ", " + getParaStringValue(RebarData[i].C, 2) + " " +
                            ", " + getParaStringValue(RebarData[i].D, 2) + " " +
                            ", " + getParaStringValue(RebarData[i].E, 2) + " " +
                            ", " + getParaStringValue(RebarData[i].F, 2) + " " +
                            ", " + getParaStringValue(RebarData[i].G, 2) + " " +
                            ", " + getParaStringValue(RebarData[i].H, 2) + " " +
                            ", " + getParaStringValue(RebarData[i].I, 2) + " " +
                            ", " + getParaStringValue(RebarData[i].J, 2) + " " +
                            ", " + getParaStringValue(RebarData[i].K, 2) + " " +
                            ", " + getParaStringValue(RebarData[i].L, 2) + " " +
                            ", " + getParaStringValue(RebarData[i].M, 2) + " " +
                            ", " + getParaStringValue(RebarData[i].N, 2) + " " +
                            ", " + getParaStringValue(RebarData[i].O, 2) + " " +
                            ", " + getParaStringValue(RebarData[i].P, 2) + " " +
                            ", " + getParaStringValue(RebarData[i].Q, 2) + " " +
                            ", " + getParaStringValue(RebarData[i].R, 2) + " " +
                            ", " + getParaStringValue(RebarData[i].S, 2) + " " +
                            ", " + getParaStringValue(RebarData[i].T, 2) + " " +
                            ", " + getParaStringValue(RebarData[i].U, 2) + " " +
                            ", " + getParaStringValue(RebarData[i].V, 2) + " " +
                            ", " + getParaStringValue(RebarData[i].W, 2) + " " +
                            ", " + getParaStringValue(RebarData[i].X, 2) + " " +
                            ", " + getParaStringValue(RebarData[i].Y, 2) + " " +
                            ", " + getParaStringValue(RebarData[i].Z, 2) + " " +
                            ", " + getParaStringValue(RebarData[i].BarLength, 1) + " " +
                            ", " + getParaStringValue(RebarData[i].BarLength, 2) + " " +
                            ", " + (RebarData[i].BarWeight == 0 || RebarData[i].BarWeight == null ? "null" : RebarData[i].BarWeight.ToString()) + " " +
                            ", '" + (RebarData[i].Remarks == null || RebarData[i].Remarks == "undefined" ? "" : RebarData[i].Remarks) + "' " +
                            ", " + (RebarData[i].shapeTransport == 0 || RebarData[i].shapeTransport == null ? "null" : RebarData[i].shapeTransport.ToString()) + " " +
                            ", " + (RebarData[i].PinSize == 0 || RebarData[i].PinSize == null ? "null" : RebarData[i].PinSize.ToString()) + " " +
                            ", '' " +
                            ", '' " +
                            ", getDate() " +
                            ", '" + User.Identity.GetUserName() + "' " +
                            ", getDate() " +
                            ", '" + User.Identity.GetUserName() + "' ) ";

                            lCmd.CommandText = lSQL;
                            lCmd.Connection = lNDSCon;
                            lCmd.CommandTimeout = 300;
                            lCmd.ExecuteNonQuery();

                        }

                        lProcessObj.CloseNDSConnection(ref lNDSCon);
                    }


                    lProcessObj = null;

                }
                catch (Exception ex)
                {
                    lErrorMsg = ex.Message;
                    lSuccess = false;
                }
            }
            return Json(new { success = lSuccess, message = lErrorMsg });
        }

        // Offline Restore PO List
        [HttpPost]
        [Route("restorePOList")]
        public ActionResult restorePOList()
        {
            var lErrorMsg = "";
            var lCmd = new SqlCommand();
            SqlDataReader lRst;
            var lNDSCon = new SqlConnection();
            int lBackupID = 0;
            var lSuccess = true;
            DateTime lBackupDate = DateTime.Now;
            var lPOList = new List<OfflinePOModels>();

            try
            {
                var lProcessObj = new ProcessController();
                if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
                {
                    lCmd.CommandText =
                    "SELECT isNull(Max(BackupID), 0) " +
                    "FROM dbo.OESOfflBackup " +
                    "WHERE BackupBy = '" + User.Identity.GetUserName() + "' ";

                    lCmd.Connection = lNDSCon;
                    lCmd.CommandTimeout = 300;
                    lRst = lCmd.ExecuteReader();
                    if (lRst.HasRows)
                    {
                        if (lRst.Read())
                        {
                            lBackupID = lRst.GetInt32(0);
                        }
                    }
                    lRst.Close();

                    if (lBackupID > 0)
                    {
                        lCmd.CommandText =
                        "SELECT BackupDate " +
                        "FROM dbo.OESOfflBackup " +
                        "WHERE BackupID = " + lBackupID + " " +
                        "AND BackupBy = '" + User.Identity.GetUserName() + "' ";

                        lCmd.Connection = lNDSCon;
                        lCmd.CommandTimeout = 300;
                        lRst = lCmd.ExecuteReader();
                        if (lRst.HasRows)
                        {
                            if (lRst.Read())
                            {
                                lBackupDate = lRst.GetDateTime(0);
                            }
                        }
                        lRst.Close();

                        lCmd.CommandText =
                        "SELECT POID " +
                        ", PONo " +
                        ", PODesc " +
                        ", StructureElement " +
                        ", BBSNo " +
                        ", BBSDesc " +
                        ", BBSTotalWT " +
                        ", BBSTotalPCs " +
                        ", CouplerType " +
                        ", Transport " +
                        ", CreatedDate " +
                        ", UpdatedDate " +
                        ", UpdatedBy " +
                        "FROM dbo.OESOfflPO " +
                        "WHERE BackupID = " + lBackupID + " " +
                        "ORDER by PONo ";

                        lCmd.Connection = lNDSCon;
                        lCmd.CommandTimeout = 300;
                        lRst = lCmd.ExecuteReader();
                        if (lRst.HasRows)
                        {
                            while (lRst.Read())
                            {
                                var lPO = new OfflinePOModels
                                {
                                    POID = lRst.GetInt32(0),
                                    PONo = lRst.GetString(1),
                                    PODesc = lRst.GetString(2),
                                    StructureElement = lRst.GetString(3),
                                    BBSNo = lRst.GetString(4),
                                    BBSDesc = lRst.GetString(5),
                                    BBSTotalWT = lRst.GetDecimal(6),
                                    BBSTotalPCs = lRst.GetInt32(7),
                                    CouplerType = lRst.GetString(8),
                                    Transport = lRst.GetString(9),
                                    CreatedDate = lRst.GetDateTime(10).ToString("yyyy-MM-dd HH:mm:ss"),
                                    UpdatedDate = lRst.GetDateTime(11).ToString("yyyy-MM-dd HH:mm:ss"),
                                    UpdatedBy = lRst.GetString(12)
                                };

                                lPOList.Add(lPO);
                            }
                        }
                        lRst.Close();
                    }
                    lProcessObj.CloseNDSConnection(ref lNDSCon);
                }


                lProcessObj = null;

            }
            catch (Exception ex)
            {
                lErrorMsg = ex.Message;
                lSuccess = false;
            }

            return Json(new { success = lSuccess, message = lErrorMsg, backupid = lBackupID, backupdate = lBackupDate.ToString("yyyy-MM-dd"), polist = lPOList });
        }

        // Offline Restore BBS List
        [HttpGet]
        [Route("restoreBBS/{BackupID}/){POID}")]
        public ActionResult restoreBBS(int BackupID, int POID)
        {
            var lErrorMsg = "";
            var lCmd = new SqlCommand();
            SqlDataReader lRst;
            var lNDSCon = new SqlConnection();
            var lSuccess = true;
            var lBBSList = new List<OfflineBBSModels>();

            try
            {
                var lProcessObj = new ProcessController();
                if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
                {
                    if (BackupID > 0 && POID > 0)
                    {
                        lCmd.CommandText =
                        "SELECT POID " +
                        ",BBSID " +
                        ",BBSNo " +
                        ",BBSDesc " +
                        ",BBSStrucElem " +
                        ",BBSOrderCABWT " +
                        ",BBSOrderSTDWT " +
                        ",BBSTotalWT " +
                        ",UpdateDate " +
                        ",UpdateBy " +
                        "FROM dbo.OESOfflBBS " +
                        "WHERE BackupID = " + BackupID + " " +
                        "AND POID = " + POID + " " +
                        "ORDER by BBSID ";

                        lCmd.Connection = lNDSCon;
                        lCmd.CommandTimeout = 300;
                        lRst = lCmd.ExecuteReader();
                        if (lRst.HasRows)
                        {
                            while (lRst.Read())
                            {
                                var lBBS = new OfflineBBSModels
                                {
                                    POID = lRst.GetInt32(0),
                                    BBSID = lRst.GetInt32(1),
                                    BBSNo = lRst.GetString(2),
                                    BBSDesc = lRst.GetString(3),
                                    BBSStrucElem = lRst.GetString(4),
                                    BBSOrderCABWT = lRst.GetDecimal(5),
                                    BBSOrderSTDWT = lRst.GetDecimal(6),
                                    BBSTotalWT = lRst.GetDecimal(7),
                                    UpdateDate = lRst.GetDateTime(8).ToString("yyyy-MM-dd HH:mm:ss"),
                                    UpdateBy = lRst.GetString(9)
                                };

                                lBBSList.Add(lBBS);
                            }
                        }
                        lRst.Close();

                    }
                    lProcessObj.CloseNDSConnection(ref lNDSCon);
                }


                lProcessObj = null;

            }
            catch (Exception ex)
            {
                lErrorMsg = ex.Message;
                lSuccess = false;
            }

            return Json(new { success = lSuccess, message = lErrorMsg, bbslist = lBBSList });
        }

        // Offline Restore Rebar List
        [HttpGet]
        [Route("restoreRebar/{BackupID}/){POID}/{BBSID}")]
        public ActionResult restoreRebar(int BackupID, int POID, int BBSID)
        {
            var lErrorMsg = "";
            var lCmd = new SqlCommand();
            SqlDataReader lRst;
            var lNDSCon = new SqlConnection();
            var lSuccess = true;
            var lBBSList = new List<OfflineBBSModels>();

            var lOfflineRebarList = new List<OfflineRebarModels>();

            try
            {
                var content = (from p in db.OfflineRebarDB
                               join s in db.Shape
                               on p.BarShapeCode equals s.shapeCode into Shape1
                               from s1 in Shape1.DefaultIfEmpty()
                               where p.BackupID == BackupID &&
                               p.POID == POID &&
                               p.BBSID == BBSID
                               orderby p.BarSort, p.BarID
                               select new
                               {
                                   p.POID,
                                   p.BBSID,
                                   p.BarID,
                                   p.BarSort,
                                   p.Cancelled,
                                   p.ElementMark,
                                   p.BarMark,
                                   p.BarType,
                                   p.BarSize,
                                   p.BarCAB,
                                   p.BarSTD,
                                   p.BarMemberQty,
                                   p.BarEachQty,
                                   p.BarTotalQty,
                                   p.BarShapeCode,
                                   p.A,
                                   p.B,
                                   p.C,
                                   p.D,
                                   p.E,
                                   p.F,
                                   p.G,
                                   p.H,
                                   p.I,
                                   p.J,
                                   p.K,
                                   p.L,
                                   p.M,
                                   p.N,
                                   p.O,
                                   p.P,
                                   p.Q,
                                   p.R,
                                   p.S,
                                   p.T,
                                   p.U,
                                   p.V,
                                   p.W,
                                   p.X,
                                   p.Y,
                                   p.Z,
                                   p.A2,
                                   p.B2,
                                   p.C2,
                                   p.D2,
                                   p.E2,
                                   p.F2,
                                   p.G2,
                                   p.H2,
                                   p.I2,
                                   p.J2,
                                   p.K2,
                                   p.L2,
                                   p.M2,
                                   p.N2,
                                   p.O2,
                                   p.P2,
                                   p.Q2,
                                   p.R2,
                                   p.S2,
                                   p.T2,
                                   p.U2,
                                   p.V2,
                                   p.W2,
                                   p.X2,
                                   p.Y2,
                                   p.Z2,
                                   p.BarLength,
                                   p.BarLength2,
                                   p.BarWeight,
                                   p.Remarks,
                                   s1.shapeParameters,
                                   s1.shapeLengthFormula,
                                   s1.shapeParaValidator,
                                   s1.shapeTransportValidator,
                                   p.shapeTransport,
                                   p.PinSize,
                                   p.UpdateDate
                               }).ToList();

                if (content.Count > 0)
                {
                    lOfflineRebarList = new List<OfflineRebarModels>(
                        content.Select(h => new OfflineRebarModels
                        {
                            POID = h.POID,
                            BBSID = h.BBSID,
                            BarID = h.BarID,
                            BarSort = h.BarSort,
                            Cancelled = h.Cancelled,
                            ElementMark = h.ElementMark,
                            BarMark = h.BarMark,
                            BarType = h.BarType,
                            BarSize = h.BarSize,
                            BarCAB = h.BarCAB,
                            BarSTD = h.BarSTD == false ? null : h.BarSTD,
                            BarMemberQty = h.BarMemberQty,
                            BarEachQty = h.BarEachQty,
                            BarTotalQty = h.BarTotalQty,
                            BarShapeCode = h.BarShapeCode,
                            A = ((h.A == null) ? null : (h.A > 0 && h.A2 > 0) ? h.A.ToString() + '-' + h.A2.ToString() : h.A.ToString()),
                            B = ((h.B == null) ? null : (h.B > 0 && h.B2 > 0) ? h.B.ToString() + '-' + h.B2.ToString() : h.B.ToString()),
                            C = ((h.C == null) ? null : (h.C > 0 && h.C2 > 0) ? h.C.ToString() + '-' + h.C2.ToString() : h.C.ToString()),
                            D = ((h.D == null) ? null : (h.D > 0 && h.D2 > 0) ? h.D.ToString() + '-' + h.D2.ToString() : h.D.ToString()),
                            E = ((h.E == null) ? null : (h.E > 0 && h.E2 > 0) ? h.E.ToString() + '-' + h.E2.ToString() : h.E.ToString()),
                            F = ((h.F == null) ? null : (h.F > 0 && h.F2 > 0) ? h.F.ToString() + '-' + h.F2.ToString() : h.F.ToString()),
                            G = ((h.G == null) ? null : (h.G > 0 && h.G2 > 0) ? h.G.ToString() + '-' + h.G2.ToString() : h.G.ToString()),
                            H = ((h.H == null) ? null : (h.H > 0 && h.H2 > 0) ? h.H.ToString() + '-' + h.H2.ToString() : h.H.ToString()),
                            I = ((h.I == null) ? null : (h.I > 0 && h.I2 > 0) ? h.I.ToString() + '-' + h.I2.ToString() : h.I.ToString()),
                            J = ((h.J == null) ? null : (h.J > 0 && h.J2 > 0) ? h.J.ToString() + '-' + h.J2.ToString() : h.J.ToString()),
                            K = ((h.K == null) ? null : (h.K > 0 && h.K2 > 0) ? h.K.ToString() + '-' + h.K2.ToString() : h.K.ToString()),
                            L = ((h.L == null) ? null : (h.L > 0 && h.L2 > 0) ? h.L.ToString() + '-' + h.L2.ToString() : h.L.ToString()),
                            M = ((h.M == null) ? null : (h.M > 0 && h.M2 > 0) ? h.M.ToString() + '-' + h.M2.ToString() : h.M.ToString()),
                            N = ((h.N == null) ? null : (h.N > 0 && h.N2 > 0) ? h.N.ToString() + '-' + h.N2.ToString() : h.N.ToString()),
                            O = ((h.O == null) ? null : (h.O > 0 && h.O2 > 0) ? h.O.ToString() + '-' + h.O2.ToString() : h.O.ToString()),
                            P = ((h.P == null) ? null : (h.P > 0 && h.P2 > 0) ? h.P.ToString() + '-' + h.P2.ToString() : h.P.ToString()),
                            Q = ((h.Q == null) ? null : (h.Q > 0 && h.Q2 > 0) ? h.Q.ToString() + '-' + h.Q2.ToString() : h.Q.ToString()),
                            R = ((h.R == null) ? null : (h.R > 0 && h.R2 > 0) ? h.R.ToString() + '-' + h.R2.ToString() : h.R.ToString()),
                            S = ((h.S == null) ? null : (h.S > 0 && h.S2 > 0) ? h.S.ToString() + '-' + h.S2.ToString() : h.S.ToString()),
                            T = ((h.T == null) ? null : (h.T > 0 && h.T2 > 0) ? h.T.ToString() + '-' + h.T2.ToString() : h.T.ToString()),
                            U = ((h.U == null) ? null : (h.U > 0 && h.U2 > 0) ? h.U.ToString() + '-' + h.U2.ToString() : h.U.ToString()),
                            V = ((h.V == null) ? null : (h.V > 0 && h.V2 > 0) ? h.V.ToString() + '-' + h.V2.ToString() : h.V.ToString()),
                            W = ((h.W == null) ? null : (h.W > 0 && h.W2 > 0) ? h.W.ToString() + '-' + h.W2.ToString() : h.W.ToString()),
                            X = ((h.X == null) ? null : (h.X > 0 && h.X2 > 0) ? h.X.ToString() + '-' + h.X2.ToString() : h.X.ToString()),
                            Y = ((h.Y == null) ? null : (h.Y > 0 && h.Y2 > 0) ? h.Y.ToString() + '-' + h.Y2.ToString() : h.Y.ToString()),
                            Z = ((h.Z == null) ? null : (h.Z > 0 && h.Z2 > 0) ? h.Z.ToString() + '-' + h.Z2.ToString() : h.Z.ToString()),
                            BarLength = ((h.BarLength == null) ? null : (h.BarLength > 0 && h.BarLength2 > 0) ? h.BarLength.ToString() + '-' + h.BarLength2.ToString() : h.BarLength.ToString()),
                            BarWeight = h.BarWeight,
                            Remarks = h.Remarks,
                            shapeParameters = h.shapeParameters,
                            shapeLengthFormula = h.shapeLengthFormula,
                            shapeParaValidator = h.shapeParaValidator,
                            shapeTransportValidator = h.shapeTransportValidator,
                            shapeTransport = h.shapeTransport,
                            PinSize = h.PinSize,
                            UpdateDate = h.UpdateDate
                        })
                    );

                }

                if (lOfflineRebarList.Count > 0)
                {
                    var lProcessObj = new ProcessController();
                    if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
                    {
                        var lBarDet = new OrderDetailsController();

                        for (int i = 0; i < lOfflineRebarList.Count; i++)
                        {
                            if (lOfflineRebarList[i].BarShapeCode == "020" || lOfflineRebarList[i].BarShapeCode == "20")
                            {
                                lOfflineRebarList[i].shapeParameters = "A";
                                lOfflineRebarList[i].shapeLengthFormula = "A";
                                lOfflineRebarList[i].shapeParType = "S";
                                lOfflineRebarList[i].shapeDefaultValue = "";
                                lOfflineRebarList[i].shapeHeightCheck = "";
                                lOfflineRebarList[i].shapeAutoCalcFormula1 = "";
                                lOfflineRebarList[i].shapeAutoCalcFormula2 = "";
                                lOfflineRebarList[i].shapeAutoCalcFormula3 = "";
                            }
                            else
                            {
                                //Check same shape processed
                                var lFound = -1;
                                if (i > 0)
                                {
                                    for (int j = 0; j < i; j++)
                                    {
                                        if (lOfflineRebarList[j].BarShapeCode == lOfflineRebarList[i].BarShapeCode)
                                        {
                                            lFound = j;
                                            break;
                                        }
                                    }
                                }
                                if (lFound >= 0)
                                {
                                    lOfflineRebarList[i].shapeParameters = lOfflineRebarList[lFound].shapeParameters;
                                    lOfflineRebarList[i].shapeLengthFormula = lOfflineRebarList[lFound].shapeLengthFormula;
                                    lOfflineRebarList[i].shapeParType = lOfflineRebarList[lFound].shapeParType;
                                    lOfflineRebarList[i].shapeDefaultValue = lOfflineRebarList[lFound].shapeDefaultValue;
                                    lOfflineRebarList[i].shapeHeightCheck = lOfflineRebarList[lFound].shapeHeightCheck;
                                    lOfflineRebarList[i].shapeAutoCalcFormula1 = lOfflineRebarList[lFound].shapeAutoCalcFormula1;
                                    lOfflineRebarList[i].shapeAutoCalcFormula2 = lOfflineRebarList[lFound].shapeAutoCalcFormula2;
                                    lOfflineRebarList[i].shapeAutoCalcFormula3 = lOfflineRebarList[lFound].shapeAutoCalcFormula3;
                                }
                                else
                                {
                                    var lShapeCode = lOfflineRebarList[i].BarShapeCode;
                                    if (lShapeCode == null)
                                    {
                                        lShapeCode = "";
                                    }
                                    lShapeCode = lShapeCode.Trim();
                                    if (lShapeCode.Length < 3)
                                    {
                                        lShapeCode = "0" + lShapeCode;
                                    }
                                    var lReturn = lBarDet.getShapeInfoFunAsync("", "", 0, lShapeCode, lNDSCon, 0).Result;
                                    if (lReturn != null)
                                    {
                                        lOfflineRebarList[i].shapeParameters = lReturn.shapeParameters;
                                        lOfflineRebarList[i].shapeLengthFormula = lReturn.shapeLengthFormula;
                                        lOfflineRebarList[i].shapeParType = lReturn.shapeParType;
                                        lOfflineRebarList[i].shapeDefaultValue = lReturn.shapeDefaultValue;
                                        lOfflineRebarList[i].shapeHeightCheck = lReturn.shapeHeightCheck;
                                        lOfflineRebarList[i].shapeAutoCalcFormula1 = lReturn.shapeAutoCalcFormula1;
                                        lOfflineRebarList[i].shapeAutoCalcFormula2 = lReturn.shapeAutoCalcFormula2;
                                        lOfflineRebarList[i].shapeAutoCalcFormula3 = lReturn.shapeAutoCalcFormula3;
                                    }
                                }
                            }
                        }

                        lBarDet = null;
                        lProcessObj.CloseNDSConnection(ref lNDSCon);
                        lProcessObj = null;

                        lNDSCon = null;
                    }
                }

                if (lOfflineRebarList.Count > 0)
                {
                    for (int i = 0; i < lOfflineRebarList.Count; i++)
                    {
                        lOfflineRebarList[i].BarShapeCode = lOfflineRebarList[i].BarShapeCode == null ? null : (lOfflineRebarList[i].BarShapeCode.StartsWith("0") == true ? lOfflineRebarList[i].BarShapeCode.Substring(1) : lOfflineRebarList[i].BarShapeCode);
                    }
                }

            }
            catch (Exception ex)
            {
                lErrorMsg = ex.Message;
                lSuccess = false;
            }

            return Json(new { success = lSuccess, message = lErrorMsg, rebarlist = lOfflineRebarList });
        }

        // Offline Restore Rebar List
        [HttpGet]
        [Route("setActivation/{UserID}/){CustomerCode}/{ProjectCode}/{ActivatedDate}")]
        public ActionResult setActivation(string UserID, string CustomerCode, string ProjectCode, string ActivatedDate)
        {
            var lErrorMsg = "";
            var lCmd = new SqlCommand();
            SqlDataReader lRst;
            var lNDSCon = new SqlConnection();
            var lSuccess = true;
            var lMaxBarLen = 0;
            var lCustBar = "";
            var lSkipBendCheck = false;
            var lMthsExpired = 0;
            var lExpired = DateTime.Now;

            try
            {
                var lProcessObj = new ProcessController();
                if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
                {
                    lCmd.CommandText =
                    "SELECT " +
                    "isNull(MaxBarLength, 12000), " +
                    "isNull(CustomerBar, ''), " +
                    "isNull(SkipBendCheck, 0) " +
                    "FROM dbo.OESProject " +
                    "WHERE CustomerCode = '" + CustomerCode + "' " +
                    "AND ProjectCode = '" + ProjectCode + "' ";

                    lCmd.Connection = lNDSCon;
                    lCmd.CommandTimeout = 300;
                    lRst = lCmd.ExecuteReader();
                    if (lRst.HasRows)
                    {
                        if (lRst.Read())
                        {
                            lMaxBarLen = lRst.GetInt32(0);
                            lCustBar = lRst.GetString(1);
                            lSkipBendCheck = lRst.GetBoolean(2);
                        }
                    }
                    lRst.Close();

                    lCmd.CommandText =
                    "SELECT " +
                    "MonthsExpired " +
                    "FROM dbo.OESOfflCtl " +
                    "WHERE ControlID = 1 ";

                    lCmd.Connection = lNDSCon;
                    lCmd.CommandTimeout = 300;
                    lRst = lCmd.ExecuteReader();
                    if (lRst.HasRows)
                    {
                        if (lRst.Read())
                        {
                            lMthsExpired = lRst.GetInt32(0);
                        }
                    }
                    lRst.Close();

                    lExpired = lExpired.AddMonths(lMthsExpired);

                    lCmd.CommandText =
                    "INSERT INTO dbo.OESOfflActivated " +
                    "(UserID " +
                    ", CustomerCode " +
                    ", ProjectCode " +
                    ", ActivatedDate " +
                    ", ExpiredDate) " +
                    "VALUES " +
                    "('" + UserID + "' " +
                    ",'" + CustomerCode + "' " +
                    ",'" + ProjectCode + "' " +
                    ",'" + ActivatedDate + "' " +
                    ",'" + lExpired.ToString("yyyy-MM-dd HH:mm:ss") + "') ";

                    lCmd.Connection = lNDSCon;
                    lCmd.CommandTimeout = 300;
                    lRst = lCmd.ExecuteReader();


                    lProcessObj.CloseNDSConnection(ref lNDSCon);
                    lNDSCon = null;
                }
                lProcessObj = null;

            }
            catch (Exception ex)
            {
                lErrorMsg = ex.Message;
                lSuccess = false;
            }

            return Json(new
            {
                success = lSuccess,
                message = lErrorMsg,
                MaxBarLen = lMaxBarLen,
                CustBar = lCustBar,
                SkipBendCheck = (lSkipBendCheck == true ? "Y" : "N"),
                ExpiryDate = lExpired.ToString("yyyy-MM-dd HH:mm:ss")
            });
        }

        // Offline Restore Rebar List
        [HttpGet]
        [Route("getExpiryDate")]
        public ActionResult getExpiryDate()
        {
            var lErrorMsg = "";
            var lCmd = new SqlCommand();
            SqlDataReader lRst;
            var lNDSCon = new SqlConnection();
            var lSuccess = true;
            var lMthsExpired = 0;
            var lExpired = DateTime.Now;

            try
            {
                var lProcessObj = new ProcessController();
                if (lProcessObj.OpenNDSConnection(ref lNDSCon) == true)
                {
                    lCmd.CommandText =
                    "SELECT " +
                    "MonthsExpired " +
                    "FROM dbo.OESOfflCtl " +
                    "WHERE ControlID = 1 ";

                    lCmd.Connection = lNDSCon;
                    lCmd.CommandTimeout = 300;
                    lRst = lCmd.ExecuteReader();
                    if (lRst.HasRows)
                    {
                        if (lRst.Read())
                        {
                            lMthsExpired = lRst.GetInt32(0);
                        }
                    }
                    lRst.Close();

                    lExpired = lExpired.AddMonths(lMthsExpired);

                    lProcessObj.CloseNDSConnection(ref lNDSCon);
                    lNDSCon = null;
                }
                lProcessObj = null;

            }
            catch (Exception ex)
            {
                lErrorMsg = ex.Message;
                lSuccess = false;
            }

            return Json(new
            {
                success = lSuccess,
                message = lErrorMsg,
                ExpiryDate = lExpired.ToString("yyyy-MM-dd HH:mm:ss")
            });
        }

    }
}
